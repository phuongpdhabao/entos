using ENTOS.Application.Features.TranslateText;
using ENTOS.Module.Helpers;
using ENTOS.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ENTOS.Infrastructure.Services.Translate
{
    /// <summary>
    /// Dịch vụ dịch thuật sử dụng Google Translate (có thể dùng API key hoặc miễn phí web scrape).
    /// </summary>
    public class BaseTranslateDataService : ISingletonDependency
    {
        private const int MaxTextLength = 3000; // Giới hạn an toàn cho mỗi request Google Translate
        private const int TimeoutSeconds = 300; // Timeout cho mỗi request
        private readonly IHttpClientFactory _httpClientFactory;
        private Application.DTOs.DataServiceDto _dataServiceDto;
        protected Module.Services.DataServiceService _dataServiceService;
        private readonly bool _useHttpHelper;

        /// <summary>
        /// Khởi tạo dịch vụ, không cần DI, sẽ dùng HttpHelper static để gửi request (không resilience nâng cao).
        /// </summary>
        public BaseTranslateDataService(Application.DTOs.DataServiceDto dataServiceDto, Module.Services.DataServiceService dataServiceService)
        {
            Initialize(dataServiceDto);
            _dataServiceService = dataServiceService;
            _useHttpHelper = true;
        }
        public BaseTranslateDataService()
        {
            _useHttpHelper = true;
        }
        protected void Initialize(Application.DTOs.DataServiceDto dataServiceDto)
        {
            if (dataServiceDto != null)
                _dataServiceDto = dataServiceDto;
        }



        /// <summary>
        /// Cấu hình HttpClient cho Google Translate với các settings tối ưu
        /// </summary>
        public static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("Translate", client =>
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

        public virtual void ParseTranslateInput(object[] inputs) { }

        public virtual async Task ParseTranslateResponse(TranslateSegmentBase segment, HttpContent httpContent)
        {
            if (!string.IsNullOrEmpty(ResultKey))
            {
                var resultText = await GetResultResponse(httpContent, ResultKey);
                if (!string.IsNullOrEmpty(resultText))
                    segment.TranslatedText = resultText;
            }
        }

        protected string ResultKey = string.Empty;

        public virtual async Task<string> GetResultResponse(HttpContent httpContent, string keyPath)
        {
            var json = await httpContent.ReadAsStringAsync();
            return Module.Helpers.JsonHelper.GetValueFromJson(json, keyPath);
        }

        public virtual async Task ParseTranslateBlockResponse(TranslateSegmentBlock block, HttpContent httpContent, string startWrapper = "<li>", string endWrapper = "</li>\n")
        {
            if (!string.IsNullOrEmpty(ResultKey))
            {
                var resultText = await GetResultResponse(httpContent, ResultKey);
                if (!string.IsNullOrEmpty(resultText))
                {
                    var resultArray = resultText.Split(new[] { startWrapper, endWrapper }, StringSplitOptions.RemoveEmptyEntries);
                    if (resultArray.Length == block.TranslateSegments.Count)
                    {
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            if (i >= block.TranslateSegments.Count)
                                break;
                            block.TranslateSegments[i].TranslatedText = resultArray[i];
                        }
                    }
                    else
                    {
                        var matches = System.Text.RegularExpressions.Regex.Matches(resultText, @$"{startWrapper}(.*?){endWrapper}", RegexOptions.Multiline);
                        if (matches.Count <= block.TranslateSegments.Count)
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                if (i >= block.TranslateSegments.Count)
                                    break;
                                var match = matches[i];
                                if (match.Success)
                                {
                                    var translatedText = match.Groups[1].Value;
                                    block.TranslateSegments[i].TranslatedText = translatedText;
                                }
                            }
                        }

                    }


                }
            }

        }

        // AutoDiscoverResponseParsers đã được chuyển sang InterfaceDiscoveryHelper

        public delegate Task AfterSendRequestEventHandler(TranslateSegmentBase segment, HttpContent httpContent, Application.DTOs.DataServiceDto dataServiceDto);

        public event AfterSendRequestEventHandler AfterSendRequest;


        /// <summary>
        /// Phương thức nền gọi Google Translate API, trả về mảng kết quả từng câu.
        /// Dùng cho cả dữ liệu ngắn và batch.
        /// </summary>
        private async Task BaseTranslateAsync(TranslateSegmentBase translateSegmentBase, string targetLanguage, string sourceLanguage)
        {
            using var client = HttpHelper.CreateHttpClient(30, true);
            var inputs = new object[] { translateSegmentBase.OriginalText, targetLanguage, sourceLanguage };
            ParseTranslateInput(inputs);
            var responseContent = await _dataServiceService.GetResultAsync(client, _dataServiceDto, inputs);
            await ParseTranslateResponse(translateSegmentBase, responseContent);
            if (AfterSendRequest != null)
            {
                await AfterSendRequest.Invoke(translateSegmentBase, responseContent, _dataServiceDto);
            }
        }

        public delegate Task AfterSendBlockRequestEventHandler(TranslateSegmentBlock block, HttpContent httpContent, Application.DTOs.DataServiceDto dataServiceDto);

        public event AfterSendBlockRequestEventHandler AfterSendBlockRequest;

        private async Task TranslateBlockAsync(TranslateSegmentBlock block, string targetLanguage, string sourceLanguage, string startWrapper = "<li>", string endWrapper = "</li>\n", CancellationToken cancellationToken = default, string prompt = null)
        {
            using var client = HttpHelper.CreateHttpClient(30, true);
            //System.Diagnostics.Debug.WriteLine($"TranslateBlockAsync bắt đầu: {client.DefaultRequestHeaders.UserAgent} gửi dữ liệu block: {block.StartIndex}");
            var inputs = new object[] { block.TranslatedTextBlock, targetLanguage, sourceLanguage };
            ParseTranslateInput(inputs);
            var responseContent = await _dataServiceService.GetResultAsync(client, _dataServiceDto, inputs);
            await ParseTranslateBlockResponse(block, responseContent, startWrapper, endWrapper);
            if (AfterSendRequest != null)
            {
                await AfterSendBlockRequest.Invoke(block, responseContent, _dataServiceDto);
            }
            //System.Diagnostics.Debug.WriteLine($"TranslateBlockAsync kết thúc: {client.DefaultRequestHeaders.UserAgent} cho block: {block.StartIndex}");
        }


        private string RemoveXmlNode(string input)
        {
            // Bước 1: Nếu dòng kết thúc không còn tag thì xóa \r hoặc \n ở cuối
            string noTags = Regex.Replace(input, @"(?<!</[^>]+>)[\r\n]+$", "", RegexOptions.IgnoreCase);
            // Bước 2: Xóa tất cả tag XML/HTML kèm khoảng trắng sau
            return Regex.Replace(noTags, @"</?[^>]+?>\s*", "");
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
                    var tempTranslateSegmentBase = new TranslateSegmentBase()
                    {
                        OriginalText = text,
                    };
                    await BaseTranslateAsync(tempTranslateSegmentBase, targetLanguage, sourceLanguage);
                    translatedSentences.AddRange(tempTranslateSegmentBase.TranslatedText);
                }
                return string.Join(" ", translatedSentences);
            }
            // Nếu text ngắn, dịch trực tiếp qua BaseTranslateAsync
            var translateSegmentBase = new TranslateSegmentBase()
            {
                OriginalText = text,
            };
            await BaseTranslateAsync(translateSegmentBase, targetLanguage, sourceLanguage);
            return translateSegmentBase.TranslatedText;
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
                var translateSegmentBase = new TranslateSegmentBase()
                {
                    OriginalText = text
                };
                using var client = HttpHelper.CreateHttpClient(30, true);
                var responseContent = await _dataServiceService.GetResultAsync(client, _dataServiceDto, new object[] { translateSegmentBase.TranslatedText });

                var arr = JsonDocument.Parse(await responseContent.ReadAsStringAsync()).RootElement;
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
                        await BaseTranslateAsync(segment, targetLanguage, sourceLanguage);
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
            string startWrapper = $"<{tagName}>";
            string endWrapper = $"</{tagName}>\n";
            var texts = segments.Select(s => s.OriginalText ?? "").ToArray();
            var blocks = CreateTranslateBlock(segments, maxBatchLength, startWrapper, endWrapper, urlEncode);

            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var tasks = new List<Task<int>>();
            CancellationToken cancellationToken = context?.Control?.CancellationToken ?? default;
            int currentIndex = 0;
            int total = blocks.Count;
            foreach (var block in blocks)
            {
                await semaphore.WaitAsync(cancellationToken);
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        //System.Diagnostics.Debug.WriteLine($"TranslateContextAsync bắt đầu block: {block.StartIndex} / {blocks.Count}");
                        cancellationToken.ThrowIfCancellationRequested();
                        await TranslateBlockAsync(block, targetLanguage, sourceLanguage, startWrapper, endWrapper, cancellationToken);
                        if (context?.Progress != null && total > 1)
                        {
                            //System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress bắt đầu Interlocked: {block.StartIndex} / {blocks.Count}");
                            Interlocked.Increment(ref currentIndex);
                            //System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress qua Interlocked: {block.StartIndex} / {blocks.Count}");
                            int percentComplete = context.StepProgressConfig.MapStepProgressPercent(currentIndex, total);
                            //System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress percentComplete: {block.StartIndex} / {percentComplete}");
                            if (context?.Control == null || !context.Control.IsMinimized || percentComplete % 20 == 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress context start: {block.StartIndex} / {percentComplete}");
                                context.Progress.PercentComplete = percentComplete;
                                System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress percentComplete: {block.StartIndex} / {percentComplete}");
                                context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig?.CurrentStepName} {currentIndex + 1}/{total} - {percentComplete}%";
                                System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress context end: {block.StartIndex} / {percentComplete}");
                            }
                            //System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress xong Interlocked: {block.StartIndex} / {blocks.Count}");
                        }
                        //System.Diagnostics.Debug.WriteLine($"TranslateContextAsync kết thúc block: {block.StartIndex} / {blocks.Count}");
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
            System.Diagnostics.Debug.WriteLine($"TranslateContextAsync xong kết quả: {segments.Count}");
        }




        /// <summary>
        /// Tạo các block dịch thuật từ danh sách segments với thông tin vị trí để hỗ trợ đa luồng
        /// </summary>
        private List<TranslateSegmentBlock> CreateTranslateBlock(List<TranslateSegmentBase> segments, int maxBatchLength, string startWrapper = "<li>", string endWrapper = "</li>\n", bool urlEncode = false)
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
                var xmlItem = $"{startWrapper}{(urlEncode ? System.Web.HttpUtility.UrlEncode(segment.OriginalText ?? "") : segment.OriginalText)}{endWrapper}";

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
                    //System.Diagnostics.Debug.WriteLine($"Tạo block StartIndex: {startIndex}: Index {blockIndex}");
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




    }
}
