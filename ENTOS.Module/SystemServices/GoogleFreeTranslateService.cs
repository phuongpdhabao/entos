using ENTOS.Application.Features.TranslateText;
using ENTOS.Module.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ENTOS.Module.SystemServices
{
    /// <summary>
    /// Dịch vụ dịch thuật sử dụng Google Translate (có thể dùng API key hoặc miễn phí web scrape).
    /// </summary>
    public class GoogleFreeTranslateService// : ITranslationDataService Hủy triển khai từ interface
    {
        //public bool CanHandle(DataServiceDto dataServiceDto)
        //{
        //    return false;
        //}
        private const int MaxTextLength = 3000; // Giới hạn an toàn cho mỗi request Google Translate
        private const int TimeoutSeconds = 300; // Timeout cho mỗi request
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly bool _useHttpHelper;

        /// <summary>
        /// Cấu hình HttpClient cho Google Translate với các settings tối ưu
        /// </summary>
        public static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("GoogleTranslate", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                MaxConnectionsPerServer = 10, // Giới hạn connection pool
                UseCookies = false
            });
        }

        /// <summary>
        /// Khởi tạo dịch vụ, inject IHttpClientFactory (DI) và truyền API key (nếu có). Sử dụng resilience hiện đại.
        /// </summary>
        public GoogleFreeTranslateService(IHttpClientFactory httpClientFactory, string apiKey = null)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
            _useHttpHelper = false;
        }

        /// <summary>
        /// Khởi tạo dịch vụ, không cần DI, sẽ dùng HttpHelper static để gửi request (không resilience nâng cao).
        /// </summary>
        public GoogleFreeTranslateService(string apiKey = null)
        {
            _apiKey = apiKey;
            _useHttpHelper = true;
        }

        /// <summary>
        /// Hàm chung gọi Google Translate API, có timeout, trả về response string.
        /// Nếu dùng DI sẽ dùng HttpClient với resilience, nếu không sẽ dùng HttpHelper.
        /// </summary>
        private async Task<string> CallGoogleTranslateApiAsync(string url, string jsonBody = null, CancellationToken cancellationToken = default)
        {
            if (_useHttpHelper)
            {
                if (jsonBody != null)
                {
                    var response = await HttpHelper.PostAsync(url, jsonBody, "application/json", null, null, 30);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var response = await HttpHelper.GetAsync(url, null, null, TimeoutSeconds);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                var httpClient = _httpClientFactory.CreateClient("GoogleTranslate");

                // Combine timeout với cancellation token
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                combinedCts.CancelAfter(TimeSpan.FromSeconds(TimeoutSeconds));

                HttpResponseMessage response;
                if (jsonBody != null)
                {
                    var content = new System.Net.Http.StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(url, content, combinedCts.Token);
                }
                else
                {
                    response = await httpClient.GetAsync(url, combinedCts.Token);
                }
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(combinedCts.Token);
            }
        }

        private async Task<HttpContent> CallGoogleFreeTranslateApiAsync(string text, string targetLanguage, string sourceLanguage, CancellationToken cancellationToken = default)
        {
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";
            if (_useHttpHelper)
            {
                var response = await HttpHelper.GetAsync(url, null, null, TimeoutSeconds);
                response.EnsureSuccessStatusCode();
                return response.Content;
            }
            else
            {
                var httpClient = _httpClientFactory.CreateClient("GoogleTranslate");

                // Combine timeout với cancellation token
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                combinedCts.CancelAfter(TimeSpan.FromSeconds(TimeoutSeconds));

                HttpResponseMessage response = await httpClient.GetAsync(url, combinedCts.Token);
                response.EnsureSuccessStatusCode();
                return response.Content;
            }
        }

        /// <summary>
        /// Phương thức nền gọi Google Translate API, trả về mảng kết quả từng câu.
        /// Dùng cho cả dữ liệu ngắn và batch.
        /// </summary>
        private async Task<string[]> BaseTranslateAsync(string text, string targetLanguage, string sourceLanguage)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_apiKey))
                {
                    // Dùng Google Cloud Translation API chính thức
                    string url = $"https://translation.googleapis.com/language/translate/v2?key={_apiKey}";
                    var body = JsonSerializer.Serialize(new
                    {
                        q = text,
                        source = sourceLanguage ?? "auto",
                        target = targetLanguage,
                        format = "text"
                    });
                    var response = await CallGoogleTranslateApiAsync(url, body);
                    var json = JsonDocument.Parse(response).RootElement;
                    var translations = json.GetProperty("data").GetProperty("translations");
                    var resultList = new List<string>();
                    foreach (var item in translations.EnumerateArray())
                        resultList.Add(item.GetProperty("translatedText").GetString());
                    return resultList.ToArray();
                }
                else
                {
                    // Dùng web scrape miễn phí
                    string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";
                    var response = await CallGoogleTranslateApiAsync(url);
                    var arr = JsonDocument.Parse(response).RootElement;
                    if (arr.GetArrayLength() > 0)
                    {
                        var resultList = new List<string>();
                        foreach (var item in arr[0].EnumerateArray())
                        {
                            if (item.GetArrayLength() > 0)
                                resultList.Add(item[0].GetString());
                        }
                        return resultList.ToArray();
                    }
                    return Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                return new[] { $"[Lỗi dịch thuật]: {ex.Message}" };
            }
        }
        private async Task BaseTranslateAsync(TranslateSegmentBase translateSegmentBase, string targetLanguage, string sourceLanguage)
        {
            using var client = HttpHelper.CreateHttpClient(30, true);
            var inputs = new object[] { translateSegmentBase.OriginalText, targetLanguage, sourceLanguage };
            ;
            var responseContent = await CallGoogleFreeTranslateApiAsync(translateSegmentBase.OriginalText, targetLanguage, sourceLanguage);
            await ParseTranslateResponse(translateSegmentBase, responseContent);

        }

        private async Task<(string, string)[]> BaseTranslateWithContentAsync(string text, string targetLanguage, string sourceLanguage, bool escapeData = true)
        {
            try
            {
                // Dùng web scrape miễn phí
                if (escapeData)
                    text = Uri.EscapeDataString(text);
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={text}";
                var response = await CallGoogleTranslateApiAsync(url);
                var arr = JsonDocument.Parse(response).RootElement;
                if (arr.GetArrayLength() > 0)
                {
                    var resultList = new List<(string, string)>();
                    foreach (var item in arr[0].EnumerateArray())
                    {
                        if (item.GetArrayLength() > 0)
                            resultList.Add((item[0].GetString(), item[1].GetString()));
                    }
                    return resultList.ToArray();
                }
                return Array.Empty<(string, string)>();
            }
            catch (Exception ex)
            {
                return new[] { ($"[Lỗi dịch thuật]: {ex.Message}", ex.Message) };
            }
        }
        private async Task TranslateBlockAsync(Application.Features.TranslateText.TranslateSegmentBlock block, string targetLanguage, string sourceLanguage, string startWrapper = "<li>", string endWrapper = "</li>\n", CancellationToken cancellationToken = default)
        {
            using var client = HttpHelper.CreateHttpClient(30, true);
            //System.Diagnostics.Debug.WriteLine($"TranslateBlockAsync bắt đầu: {client.DefaultRequestHeaders.UserAgent} gửi dữ liệu block: {block.StartIndex}");
            var inputs = new object[] { block.TranslatedTextBlock, targetLanguage, sourceLanguage };
            var responseContent = await CallGoogleFreeTranslateApiAsync(block.TranslatedTextBlock, targetLanguage, sourceLanguage);
            await ParseTranslateBlockResponse(block, responseContent, startWrapper, endWrapper);

            //            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={block.TranslatedTextBlock}";
            //            var response = await CallGoogleTranslateApiAsync(url, null, cancellationToken);
            //            var arr = JsonDocument.Parse(response).RootElement;

            //            int segmentIndex = 0, totalResults = 0;

            //            foreach (var item in arr[0].EnumerateArray())
            //            {
            //                try
            //                {

            //                    if (item.GetArrayLength() > 0)
            //                    {
            //                        var resultText = item[0].GetString();
            //                        if (string.IsNullOrEmpty(resultText))
            //                            continue;
            //                        var sourceText = item[1].GetString();

            //                        if (segmentIndex >= block.TranslateSegments.Count)
            //                            break; // Tránh lỗi nếu có nhiều kết quả hơn segments
            //                        var translateText = RemoveXmlNode(resultText);
            //#if DEBUG
            //                        if (!sourceText.Contains(block.TranslateSegments[segmentIndex].OriginalText))
            //                        {
            //                            System.Diagnostics.Debug.WriteLine($"Block {block.StartIndex} : Vào {sourceText} - Ra {block.TranslateSegments[segmentIndex].OriginalText}");

            //                        }
            //#endif
            //                        if (!string.IsNullOrEmpty(translateText))
            //                        {
            //                            if (segmentIndex > 0 && !sourceText.Contains(startWrapper)) // fix lỗi xuống dòng vô lý
            //                                block.TranslateSegments[segmentIndex - 1].TranslatedText += translateText;
            //                            else
            //                            {
            //                                block.TranslateSegments[segmentIndex].TranslatedText = translateText;
            //                                segmentIndex++;
            //                            }

            //                        }
            //#if DEBUG
            //                        else
            //                        {

            //                            System.Diagnostics.Debug.WriteLine($"Block {block.StartIndex} : Dòng dịch bị trống {resultText}");

            //                            //Lỗi dòng này, cần debug
            //                        }
            //#endif

            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    throw;
            //                }

            //                totalResults++;


            //            }
            //#if DEBUG

            //            if (totalResults != block.TranslateSegments.Count)
            //            {
            //                if (block.TranslateSegments.Count != segmentIndex)
            //                    System.Diagnostics.Debug.WriteLine($"Block {block.StartIndex} : Đàu ra {totalResults} dòng - đầu vảo {block.TranslateSegments.Count}");

            //            }
            //#endif
        }

        private string RemoveXmlNode(string input)
        {
            // Bước 1: Nếu dòng kết thúc không còn tag thì xóa \r hoặc \n ở cuối
            string noTags = System.Text.RegularExpressions.Regex.Replace(input, @"(?<!</[^>]+>)[\r\n]+$", "", RegexOptions.IgnoreCase);
            // Bước 2: Xóa tất cả tag XML/HTML kèm khoảng trắng sau
            return System.Text.RegularExpressions.Regex.Replace(noTags, @"</?[^>]+?>\s*", "");
        }

        /// <summary>
        /// Dịch một đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích sử dụng Google Translate.
        /// Nếu text quá dài sẽ tự động tách câu, gom batch, dịch batch rồi ghép lại đúng thứ tự.
        /// </summary>
        /// <param name="text">Văn bản cần dịch</param>
        /// <param name="targetLanguage">Mã ngôn ngữ đích (ví dụ: "vi", "en")</param>
        /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null)</param>
        /// <param name="prompt">Prompt cho AI để hỗ trợ dịch thuật (có thể null)</param>
        /// <returns>Văn bản đã được dịch</returns>
        /// <example>
        /// var service = new GoogleTranslateService();
        /// string result = await service.TranslateAsync("Hello world", "vi"); // "Xin chào thế giới"
        /// </example>
        public async Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, string prompt = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            sourceLanguage ??= "auto";

            // TODO: Sử dụng prompt để dịch bằng AI nếu có
            // if (!string.IsNullOrEmpty(prompt))
            // {
            //     // Gọi AI service để dịch với prompt
            //     // return await aiTranslationService.TranslateWithPromptAsync(text, targetLanguage, sourceLanguage, prompt);
            // }

            if (text.Length > MaxTextLength)
            {
                // 1. Tách câu thông minh
                var sentences = SplitSentences(text);
                // 2. Gom batch (mỗi batch < MaxTextLength)
                var batches = BatchSentences(sentences, MaxTextLength);
                var translatedSentences = new List<string>();
                foreach (var batch in batches)
                {
                    var batchText = string.Join("\n", batch);
                    var batchResult = await BaseTranslateAsync(batchText, targetLanguage, sourceLanguage);
                    translatedSentences.AddRange(batchResult);
                }
                return string.Join(" ", translatedSentences);
            }
            // Nếu text ngắn, dịch trực tiếp qua BaseTranslateAsync
            var singleResult = await BaseTranslateAsync(text, targetLanguage, sourceLanguage);
            return singleResult.Length > 0 ? singleResult[0] : string.Empty;
        }


        /// <summary>
        /// Tách văn bản thành các câu nhỏ dựa trên dấu câu mạnh và xuống dòng.
        /// </summary>
        private List<string> SplitSentences(string text)
        {
            // Tách theo . ! ? ; hoặc xuống dòng
            var sentences = Regex.Split(text, "(?<=[.!?;\n])").Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            return sentences;
        }

        /// <summary>
        /// Gom các câu thành các batch, mỗi batch có tổng độ dài < maxLength.
        /// </summary>
        private List<List<string>> BatchSentences(List<string> sentences, int maxLength)
        {
            var batches = new List<List<string>>();
            var currentBatch = new List<string>();
            int currentLength = 0;
            foreach (var sentence in sentences)
            {
                if (currentLength + sentence.Length + 1 > maxLength && currentBatch.Count > 0)
                {
                    batches.Add(new List<string>(currentBatch));
                    currentBatch.Clear();
                    currentLength = 0;
                }
                currentBatch.Add(sentence);
                currentLength += sentence.Length + 1; // +1 cho ký tự xuống dòng
            }
            if (currentBatch.Count > 0)
                batches.Add(currentBatch);
            return batches;
        }

        /// <summary>
        /// Dịch nhiều câu văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích.
        /// </summary>
        public async Task<string[]> TranslateAsync(string[] texts, string targetLanguage, string sourceLanguage = null, string prompt = null)
        {
            if (texts == null || texts.Length == 0) return Array.Empty<string>();
            var results = new List<string>(texts.Length);
            foreach (var text in texts)
            {
                var translated = await TranslateAsync(text, targetLanguage, sourceLanguage, prompt);
                results.Add(translated);
            }
            return results.ToArray();
        }

        /// <summary>
        /// Phát hiện ngôn ngữ của một đoạn văn bản sử dụng Google Translate.
        /// </summary>
        public async Task<string> DetectLanguageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            try
            {
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&q={Uri.EscapeDataString(text)}";
                var response = await CallGoogleTranslateApiAsync(url);
                var arr = System.Text.Json.JsonDocument.Parse(response).RootElement;
                // arr[2] là mã ngôn ngữ phát hiện được
                if (arr.GetArrayLength() > 2 && arr[2].GetString() != null)
                    return arr[2].GetString();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"[Lỗi phát hiện ngôn ngữ]: {ex.Message}";
            }
        }

        /// <summary>
        /// Dịch mảng văn bản theo ngữ cảnh với hỗ trợ CancellationToken
        /// Dữ liệu sẽ được xử lý trực tiếp trong List<TranslateSegmentBase> được truyền vào.
        /// </summary>
        public async Task TranslateAsync(List<TranslateSegmentBase> segments,
            string targetLanguage,
            string sourceLanguage = null,
            int maxConcurrency = 10,
            bool urlEncode = false,
            Module.SystemObjects.LongTaskContext context = null,
            string prompt = null)
        {
            if (segments == null || segments.Count == 0) return;

            if (context?.Progress != null)
            {
                context.Progress.PercentComplete = 0;
                context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig?.CurrentStepName} {segments.Count} dòng...";
            }

            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var tasks = new List<Task<int>>();
            CancellationToken cancellationToken = context?.Control?.CancellationToken ?? default;
            int currentIndex = 0;
            int total = segments.Count;
            foreach (var segment in segments)
            {
                await semaphore.WaitAsync(cancellationToken);
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await BaseTranslateAsync(segment.OriginalText, targetLanguage, sourceLanguage);
                        if (context?.Progress != null && total > 1)
                        {
                            Interlocked.Increment(ref currentIndex);
                            int percentComplete = context.StepProgressConfig.MapStepProgressPercent(currentIndex, total);
                            if (context?.Control == null || !context.Control.IsMinimized || percentComplete % 20 == 0)
                            {
                                context.Progress.PercentComplete = percentComplete;
                                context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig?.CurrentStepName} {currentIndex + 1}/{total} - {percentComplete}%";

                            }
                        }
                        return currentIndex;
                    }
                    catch (OperationCanceledException)
                    {
                        return -1;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, cancellationToken));
            }
            await Task.WhenAll(tasks);
            System.Diagnostics.Debug.WriteLine($"TranslateContextAsync xong kết quả: {segments.Count}");
        }

        /// <summary>
        /// Dịch mảng văn bản theo ngữ cảnh với hỗ trợ CancellationToken
        /// Dữ liệu sẽ được xử lý trực tiếp trong List<TranslateSegmentBase> được truyền vào.
        /// </summary>
        public async Task TranslateContextAsync(List<TranslateSegmentBase> segments,
            string targetLanguage,
            string sourceLanguage = null,
            int maxBatchLength = 4000,
            string tagName = "␟␟␟",
            int maxConcurrency = 10,
            bool urlEncode = true,
            Module.SystemObjects.LongTaskContext context = null,
            string prompt = null)
        {
            if (segments == null || segments.Count == 0) return;

            var progress = context?.Progress;
            var control = context?.Control;
            var prefixMessage = context?.StepProgressConfig != null ? $"[{context.StepProgressConfig.CurrentStepIndex}]" : string.Empty;

            if (progress != null)
            {
                progress.PercentComplete = 0;
                progress.ProgressMessage = $"{prefixMessage} Bắt đầu dịch...";
            }
            string startWrapper = $"<{tagName}>";
            string endWrapper = $"</{tagName}>\n";
            var texts = segments.Select(s => s.OriginalText ?? "").ToArray();
            var blocks = CreateTranslateBlock(segments, maxBatchLength, startWrapper, endWrapper);

            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var tasks = new List<Task<int>>();
            CancellationToken cancellationToken = control?.CancellationToken ?? default;
            int currentIndex = 0;
            int total = blocks.Count;
            foreach (var block in blocks)
            {
                await semaphore.WaitAsync(cancellationToken);
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await TranslateBlockAsync(block, targetLanguage, sourceLanguage, startWrapper, endWrapper, cancellationToken);
                        if (progress != null)
                        {
                            Interlocked.Increment(ref currentIndex);
                            int percentComplete = (int)((double)(currentIndex + 1) / total * 100);
                            if (control == null || !control.IsMinimized || percentComplete % 20 == 0)
                            {
                                progress.PercentComplete = percentComplete;
                                progress.ProgressMessage = $"🔄 Đang xử lý {currentIndex + 1}/{total} - {percentComplete}%";
                            }
                        }
                        return block.StartIndex;
                    }
                    catch (OperationCanceledException)
                    {
                        return -1;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }




        /// <summary>
        /// Tạo các block dịch thuật từ danh sách segments với thông tin vị trí để hỗ trợ đa luồng
        /// </summary>
        private List<TranslateSegmentBlock> CreateTranslateBlock(List<TranslateSegmentBase> segments, int maxBatchLength, string startWrapper = "<li>", string endWrapper = "</li>\n")
        {
            var blocks = new List<TranslateSegmentBlock>();
            var currentSegments = new List<TranslateSegmentBase>();
            var currentItems = new List<string>();
            int currentLength = 0;
            int startIndex = 0;
            int blockIndex = 0;

            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                var xmlItem = $"{startWrapper}{System.Web.HttpUtility.UrlEncode(segment.OriginalText ?? "")}{endWrapper}";

                if (currentLength + xmlItem.Length + 50 > maxBatchLength && currentItems.Count > 0) // +50 cho overhead XML
                {
                    blocks.Add(new TranslateSegmentBlock
                    {
                        StartIndex = startIndex,
                        TranslateSegments = new List<TranslateSegmentBase>(currentSegments),
                        TranslatedTextBlock = string.Join("", currentItems),
                        Index = blockIndex++
                    });
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Tạo block StartIndex: {startIndex}: Index {blockIndex}");
#endif
                    startIndex += currentSegments.Count;
                    currentSegments.Clear();
                    currentItems.Clear();
                    currentLength = 0;
                }

                currentSegments.Add(segment);
                currentItems.Add(xmlItem);
                currentLength += xmlItem.Length;
            }

            if (currentSegments.Count > 0)
            {
                blocks.Add(new TranslateSegmentBlock
                {
                    StartIndex = startIndex,
                    TranslateSegments = new List<TranslateSegmentBase>(currentSegments),
                    TranslatedTextBlock = string.Join("", currentItems),
                    Index = blockIndex
                });
            }

            return blocks;
        }

        public async Task ParseTranslateResponse(TranslateSegmentBase segment, HttpContent httpContent)
        {
            try
            {
                var json = await httpContent.ReadAsStringAsync();
                var arr = JsonDocument.Parse(json).RootElement;
                if (arr.GetArrayLength() > 0 && arr[0].ValueKind == JsonValueKind.Array)
                {
                    var translatedBuilder = new StringBuilder();
                    var originalBuilder = new StringBuilder();

                    foreach (var sentence in arr[0].EnumerateArray())
                    {
                        if (sentence.GetArrayLength() >= 2)
                        {
                            translatedBuilder.Append(sentence[0].GetString());
                            originalBuilder.Append(sentence[1].GetString());
                        }
                    }

                    segment.TranslatedText = translatedBuilder.ToString();
                    segment.OriginalText = originalBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                //segment.TranslatedText = $"[Lỗi dịch Google: {ex.Message}]";
            }
        }

        public async Task ParseTranslateBlockResponse(TranslateSegmentBlock block, HttpContent httpContent, string startWrapper = "<li>", string endWrapper = "</li>\n")
        {
            try
            {
                var json = await httpContent.ReadAsStringAsync();
                var arr = JsonDocument.Parse(json).RootElement;
                int segmentIndex = 0, totalResults = 0;
                // string startWrapper = "<li>"; // bỏ khai báo cục bộ
                //if(arr[0].EnumerateArray().Count() == 1)
                //{

                //}
                var total = arr[0].EnumerateArray();
                foreach (var item in arr[0].EnumerateArray())
                {
                    try
                    {
                        var itemLength = item.GetArrayLength();
                        if (item.GetArrayLength() > 0)
                        {
                            var resultText = item[0].GetString();
                            if (string.IsNullOrEmpty(resultText))
                                continue;
                            var sourceText = item[1].GetString();
                            if (segmentIndex >= block.TranslateSegments.Count)
                                break;
                            //Kiểm tra dữ liệu có được tách không
                            if (block.TranslateSegments.Count > 1)
                            {
                                var sourceTextArray = sourceText.Split(new[] { startWrapper, endWrapper }, StringSplitOptions.RemoveEmptyEntries);
                                if (sourceTextArray.Length > 1)
                                {
                                    var resultArray = resultText.Split(new[] { startWrapper, endWrapper }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < resultArray.Length && i < sourceTextArray.Length; i++)
                                    {
                                        if (segmentIndex >= block.TranslateSegments.Count)
                                            break;
                                        if (resultArray[i].EndsWith('\n') && !sourceTextArray[i].EndsWith('\n'))
                                            resultArray[i] = resultArray[i].Substring(0, resultArray[i].Length - 1); // Xóa ký tự xuống dòng cuối cùng nếu có
                                        block.TranslateSegments[segmentIndex].TranslatedText = resultArray[i];
                                        segmentIndex++;
                                    }
                                    continue;
                                }

                            }
                            var translateText = Module.Helpers.TextHelper.RemoveXmlNode(resultText);
                            if (!string.IsNullOrEmpty(translateText))
                            {
                                if (segmentIndex > 0 && !sourceText.Contains(startWrapper))
                                    block.TranslateSegments[segmentIndex - 1].TranslatedText += translateText;
                                else
                                {
                                    block.TranslateSegments[segmentIndex].TranslatedText = translateText;
                                    segmentIndex++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Có thể log lỗi từng dòng nếu cần
                    }
                    totalResults++;
                }
            }
            catch (Exception ex)
            {
                foreach (var seg in block.TranslateSegments)
                {
                    //seg.TranslatedText = $"[Lỗi dịch Google Block: {ex.Message}]";
                }
            }
        }





    }
}

