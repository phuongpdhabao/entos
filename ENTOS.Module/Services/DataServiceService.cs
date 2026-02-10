using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Helpers;
using ENTOS.Module.Extensions;
using ENTOS.Module.SystemServices;
using ENTOS.Module.Services;


 
namespace ENTOS.Module.Services 
{

    public partial class DataServiceService : BaseService
    {

        public DataServiceService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public DataServiceService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3370ImportCode
        
        /// Lấy DataService mặc định theo loại 
        public DataService GetDefaultDataService(DataServiceType dataServiceType)
        {
            var query = GetObjectsQuery<DataService>();
            return query.FirstOrDefault(x => x.DataServiceType == dataServiceType && !x.Pause && x.IsDefault);
            //var criteria = CriteriaOperator.Parse("DataServiceType = ? and Not [Pause] and [IsDefault] = True", dataServiceType);
            //return objectSpace.FindObject<DataService>(criteria);
        }


        /// Lấy DataService mặc định theo loại 
        public DataService GetDefaultDataService(string softwareServiceTypeCode)
        {
            var query = GetObjectsQuery<DataService>();
            return query.FirstOrDefault(x => x.SoftwareServiceType.Code == softwareServiceTypeCode && !x.Pause && x.IsDefault);
            //var criteria = CriteriaOperator.Parse("[SoftwareServiceType.Code] = ? and Not [Pause] and [IsDefault] = True", softwareServiceTypeCode);
            //return objectSpace.FindObject<DataService>(criteria);
        }


        /// Lấy DataService mặc định theo loại và user
        public DataService GetDataService(string softwareServiceTypeCode, object currentUser)
        {     
            ArgumentNullException.ThrowIfNull(currentUser, nameof(currentUser));

            if (currentUser is not Member user)
                throw new InvalidOperationException("Người dùng hiện tại không hợp lệ.");

            var memberDataService = user.MemberDataServiceList
                                         .FirstOrDefault(x => x.SoftwareServiceType.Code == softwareServiceTypeCode);

            return memberDataService?.DataService ?? GetDefaultDataService(softwareServiceTypeCode);
        }



        #endregion SourceCode3370ImportCode

        #region SourceCode4543ImportCode
                public static DataService GetDataService(ViewController viewController, DataServiceType dataServiceType)
        {
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("DataServiceType = ? and Not [Pause]", dataServiceType);
            return Helpers.XafXpoHelper.ShowSelectObjectDialog<DataService>(viewController, criteria);
        }

        /// Chọn DataService từ danh sách
        public DataService GetDataService(ViewController viewController, string softwareServiceTypeCode)
        {
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("[SoftwareServiceType.Code] = ? and Not [Pause]", softwareServiceTypeCode);
            return Helpers.XafXpoHelper.ShowSelectObjectDialog<DataService>(viewController, criteria);
        }
        #endregion SourceCode4543ImportCode

        #region SourceCode3294ImportCode
                public string CheckResultType(object result)
        {
            if (result is string)
            {
                if (((string)result).StartsWith("{") && ((string)result).EndsWith("}"))
                {
                    return "json";
                }
                else if (((string)result).StartsWith("<") && ((string)result).EndsWith(">"))
                {
                    return "xml";
                }
                else if (((string)result).StartsWith("1") && ((string)result).Contains(" --> "))
                {
                    return "srt";
                }
                else
                {
                    return "text";
                }
            }
            else if (result is byte[])
            {
                return "file";
            }
            return null;
        }
        
        public object GetResult(DataService dataService, ViewController view, object[] inputs, string[] outputs, string logContent = null)
        {
            if (dataService.ApiMethodType == ApiMethodType.File)
            {
                return GetFileResult(dataService, view, inputs, outputs, logContent);
            }
            if (!string.IsNullOrEmpty(dataService.Address))
            {
                if (System.IO.File.Exists(dataService.Address))
                {
                    Module.Helpers.AudioVideoHelper.AutoInstalledFfmpeg(view.View.ObjectSpace);
                    if (dataService.Name.Contains("pyannote", StringComparison.OrdinalIgnoreCase))
                    {
                        //Pyanote chỉ hỗ trợ wav
                        if (inputs[0] is string)
                        {
                            var fileName = inputs[0] as string;
                            if (fileName.StartsWith('"'))
                                fileName = fileName.Substring(1);
                            if (fileName.EndsWith('"'))
                                fileName = fileName.Substring(0, fileName.Length - 1);
                            string extension = ".wav";
                            if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                            {
                                var wavFile = fileName + extension;
                                if (System.IO.File.Exists(wavFile))
                                    System.IO.File.Delete(wavFile);
                                string convertArguments = $"-i \"{fileName}\" -acodec pcm_s16le -ar 16000 \"{wavFile}\"";
                                Module.Helpers.ProcessHelper.RunProcessOutside("ffmpeg", convertArguments);
                                if (System.IO.File.Exists(wavFile))
                                {
                                    if (((string)inputs[0]).StartsWith('"'))
                                        wavFile = '"' + wavFile + '"';
                                    inputs[0] = wavFile;
                                }

                            }
                        }
                    }
                    //Dùng dịch vụ offline
                    string arguments = ParameterToString(dataService, inputs);
                    Module.Helpers.ProcessHelper.RunProcessOutside(dataService.Address, arguments, 0, true);

                    if (outputs != null)
                    {
                        foreach (var output in outputs)
                        {
                            if (System.IO.File.Exists(output))
                                return System.IO.File.ReadAllText(output);
                        }
                    }
                    return null;
                }
                else if (dataService.Address.StartsWith("http"))
                {
                    //Dịch vụ online
                    if (dataService.Address.StartsWith("https://replicate.com/") || dataService.Address.StartsWith("https://api.replicate.com/"))
                    {
                        return GetReplicateResult(dataService, view, inputs, logContent);
                    }
                    else if (dataService.Address.StartsWith("https://api.openai.com/"))
                    {
                        return GetResultFromOpenAI(dataService, view, inputs, logContent);
                    }
                    else if (dataService.Address.StartsWith("https://api.fpt.ai/"))
                    {
                        var result = GetResultFromFptAI(dataService, view, inputs, logContent);
                        //if (result is string && ((string)result).StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    return Tools.GetFileFromLink((string)result);
                        //}
                        return result;
                    }
                    else
                    {
                        return GetBaseResult(dataService, view, inputs, logContent);
                        //return GetResultLocalApi(dataService, view, inputs, logContent);
                    }
                }
            }
            else
            {
                //Nếu dùng piper
                if (!string.IsNullOrEmpty(dataService.APIKey) && dataService.APIKey.Contains("Piper", StringComparison.OrdinalIgnoreCase))
                {
                    return GetResultFromPiperOffline(dataService, view, inputs, logContent);
                }
            }
            return null;
        }

        public async System.Threading.Tasks.Task<string> TranslateUsingGoogleAsync(DataService dataService, string inputText, string sourceLang, string targetLang)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputText))
                    return string.Empty;

                // Escape các tham số
                string escapedSourceLang = Uri.EscapeDataString(sourceLang);
                string escapedTargetLang = Uri.EscapeDataString(targetLang);
                string escapedText = Uri.EscapeDataString(inputText);

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                // Thay thế các placeholder trong URL
                string url = string.Format(
                    "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                    sourceLang,
                    targetLang,
                    System.Web.HttpUtility.UrlEncode(inputText)
                );

                // Thêm User-Agent để tránh lỗi 403
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                var response = await client.GetAsync(url).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Google Translate API error: {content}");
                    return $"Error: {content}";
                }

                using var doc = System.Text.Json.JsonDocument.Parse(content);
                var sentenceArray = doc.RootElement[0];
                var translatedText = new System.Text.StringBuilder();

                foreach (var sentence in sentenceArray.EnumerateArray())
                {
                    translatedText.Append(sentence[0].GetString());
                }

                return translatedText.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in TranslateUsingGoogleAsync: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public async Task<float> GetSentenceSimilarityAsync(DataService dataService, string sentence1, string sentence2)
        {
            using var client = new HttpClient();
            var url = dataService.Address;


            var payload = new Dictionary<string, object>
            {
                { "sentence1", sentence1 },
                { "sentence2", sentence2 },
                { "x-api-key", dataService.APIKey}
            };

            foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
            {
                payload[parameter.Name] = parameter.Value;
            }

            // Serialize payload
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var method = dataService.ApiMethodType == ApiMethodType.Get ? HttpMethod.Get : HttpMethod.Post;
            HttpResponseMessage response;

            if (method == HttpMethod.Get)
            {
                // Chuyển payload thành query string (nếu cần)
                var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
                foreach (var prop in payload.GetType().GetProperties())
                {
                    var value = prop.GetValue(payload)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                        queryParams[prop.Name] = value;
                }

                var uriBuilder = new UriBuilder(url)
                {
                    Query = queryParams.ToString()
                };

                response = await client.GetAsync(uriBuilder.Uri);
            }
            else // POST
            {
                // Có thể thêm headers như APIKey nếu cần:
                // client.DefaultRequestHeaders.Add("X-Api-Key", "your-api-key");
                response = await client.PostAsync(url, content);
            }

            // Kiểm tra lỗi HTTP (400, 500...)
            response.EnsureSuccessStatusCode();


            string responseContent = await response.Content.ReadAsStringAsync();
            var json = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
            float similarity = (float)(Newtonsoft.Json.Linq.JObject.Parse(responseContent)["similarity"] ?? 0f);

            return similarity;
        }
        public async System.Threading.Tasks.Task<byte[]> GetImageResult(DataService dataService, byte[] imageBytes, int borderSize = 20)
        {
            using var client = new System.Net.Http.HttpClient
            {
                Timeout = TimeSpan.FromSeconds(600) // Tăng timeout lên 60 giây
            };
            using var form = new System.Net.Http.MultipartFormDataContent();

            // Xác định định dạng ảnh
            System.Drawing.Imaging.ImageFormat imageFormat;
            using (var imageStream = new System.IO.MemoryStream(imageBytes))
            {
                var image = System.Drawing.Image.FromStream(imageStream);
                imageFormat = image.RawFormat;
            }

            var fileStream = new System.IO.MemoryStream(dataService.PreviousDataService != null ? await GetImageResult(dataService.PreviousDataService, imageBytes, borderSize) : imageBytes);
            var fileContent = new System.Net.Http.StreamContent(fileStream);

            // Chọn Content-Type dựa trên định dạng ảnh
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/" + imageFormat.ToString().ToLower());

            form.Add(fileContent, "file", "image");

            // Thêm tham số 
            foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
            {
                // Thêm tham số model
                // Thêm tham số response_format
                form.Add(new System.Net.Http.StringContent(parameter.Value), parameter.Name);
            }

            try
            {
                // Bổ sung APIKey vào header của yêu cầu HTTP
                client.DefaultRequestHeaders.Add("x-api-key", dataService.APIKey);
                // Gửi yêu cầu POST và nhận phản hồi
                var response = await client.PostAsync(dataService.Address, form);
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu ảnh từ phản hồi
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                return responseBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }
        }

        public async System.Threading.Tasks.Task<string> InsertAccents(DataService dataService, string text)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var url = dataService.Address;
                var requestBody = new
                {
                    text = text
                };

                // Chuyển đổi đối tượng thành JSON bằng cách sử dụng String.Format
                var json = $"{{\"text\":\"{text}\"}}";
                var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                System.Net.Http.HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        public object GetResultLocalApi(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            if (inputs.Length == 0) return null;
            // Read the audio file as bytes
            // Create the HTTP client
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                if (dataService.Address.EndsWith("?") && inputs.Length > 0 && GetDataServiceParameterListWithSort(dataService).Count() > 0)
                {
                    //Dùng phương thức Get
                    var queryString = dataService.Address;
                    foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                    {
                        queryString += $"{parameter.Name}={parameter.Value}&";
                    }
                    queryString = string.Format(queryString, inputs).TrimEnd('&');
                    // Gửi yêu cầu GET
                    var responseTask = System.Threading.Tasks.Task.Run(() => client.GetAsync(queryString));
                    responseTask.Wait();
                    var response = responseTask.Result;
                    response.EnsureSuccessStatusCode();

                    // Kiểm tra xem yêu cầu có thành công hay không
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc phản hồi như một luồng nhị phân
                        var responseBytesTask = System.Threading.Tasks.Task.Run(() => response.Content.ReadAsByteArrayAsync());
                        responseBytesTask.Wait();

                        // Log
                        CreateLog(dataService, view, logContent);

                        // Trả về dữ liệu nhị phân
                        return responseBytesTask.Result;
                    }
                    else
                    {
                        throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                    }
                }
                else if (dataService.DataServiceType == DataServiceType.ImageSearch)
                {
                    // Thêm file vào nội dung của form      
                    using (System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent())
                    {
                        if (inputs[0] is string)
                        {
                            string fileName = inputs[0] as string;
                            if (!System.IO.File.Exists(fileName)) return null;
                            var imageFileInfo = new System.IO.FileInfo(fileName);
                            var fileContent = new System.Net.Http.ByteArrayContent(File.ReadAllBytes(fileName));
                            // Tự động nhận diện Content-Type dựa trên định dạng của file
                            string mimeType = imageFileInfo.Extension switch
                            {
                                ".jpg" => "image/jpeg",
                                ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".bmp" => "image/bmp",
                                _ => "application/octet-stream",
                            };
                            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(mimeType);
                            form.Add(fileContent, "file", Path.GetFileName(fileName));
                        }
                        foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                        {
                            // Thêm tham số model
                            // Thêm tham số response_format
                            form.Add(new System.Net.Http.StringContent(parameter.Value), parameter.Name);
                        }
                        // Gửi yêu cầu POST tới API
                        //System.Net.Http.HttpResponseMessage response = client.PostAsync(Address, form).Result;
                        // Gửi yêu cầu POST để xóa nền
                        var responseTask = System.Threading.Tasks.Task.Run(() => client.PostAsync(dataService.Address, form));
                        responseTask.Wait();
                        var response = responseTask.Result;
                        response.EnsureSuccessStatusCode();

                        // Kiểm tra xem yêu cầu có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc phản hồi như một luồng nhị phân
                            var responseBytesTask = System.Threading.Tasks.Task.Run(() => response.Content.ReadAsByteArrayAsync());
                            responseBytesTask.Wait();
                            //Log
                            CreateLog(dataService, view, logContent);
                            return responseBytesTask.Result;
                        }
                        else
                        {
                            throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                        }
                    }

                }
                else if (dataService.DataServiceType == DataServiceType.STT)
                {
                    using (System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent())
                    {

                        // Đọc file âm thanh
                        if (dataService.DataServiceType == DataServiceType.STT)
                        {
                            string fileName = inputs[0] as string;
                            fileName = GetRealFileName(fileName);
                            if (!System.IO.File.Exists(fileName)) return null;
                            var audioFileInfo = new System.IO.FileInfo(fileName);
                            var fileContent = new System.Net.Http.ByteArrayContent(File.ReadAllBytes(fileName));
                            // Tự động nhận diện Content-Type dựa trên định dạng của file
                            string mimeType = audioFileInfo.Extension switch
                            {
                                ".mp3" => "audio/mpeg",
                                ".wav" => "audio/wav",
                                ".ogg" => "audio/ogg",
                                ".flac" => "audio/flac",
                                ".mp4" => "audio/mpeg",
                                _ => "application/octet-stream",
                            };
                            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(mimeType);

                            // Thêm file vào nội dung của form
                            form.Add(fileContent, "file", Path.GetFileName(fileName));
                        }
                        foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                        {
                            // Thêm tham số model
                            // Thêm tham số response_format
                            form.Add(new System.Net.Http.StringContent(parameter.Value), parameter.Name);
                        }

                        // Thêm tham số model
                        //form.Add(new StringContent(model), "model");

                        // Thêm tham số response_format
                        //form.Add(new StringContent(responseFormat), "response_format");

                        // Gửi yêu cầu POST tới API
                        System.Net.Http.HttpResponseMessage response = client.PostAsync(dataService.Address, form).Result;

                        // Kiểm tra xem yêu cầu có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung phản hồi từ API
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            //Log
                            CreateLog(dataService, view, logContent);
                            return responseData;
                        }
                        else
                        {
                            throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                        }
                    }
                }
                else if (dataService.DataServiceType == DataServiceType.TTS)
                {
                    //var requestData = new
                    //{
                    //    input = inputs[0] as string,
                    //    voice = inputs[1] as string,
                    //    speed = inputs[2] as string
                    //};
                    //var re1 = requestData.GetType();
                    //var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                    var requestData2 = new System.Collections.Generic.Dictionary<string, string>();
                    requestData2.Add("input", inputs[0] as string);
                    requestData2.Add("voice", inputs[1] as string);
                    requestData2.Add("speed", inputs[2] as string);
                    foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                    {
                        requestData2.Add(parameter.Name, parameter.Value);
                    }

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData2);
                    var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    try
                    {
                        System.Net.Http.HttpResponseMessage response = client.PostAsync("https://api.openai.com/v1/audio/speech", content).Result;
                        //string responseString = response.Content.ReadAsStringAsync().Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // Process the response data
                            CreateLog(dataService, view, logContent);
                            return response.Content.ReadAsByteArrayAsync().Result;
                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                            //Console.WriteLine("Details: " + responseString);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);
                    }

                    return null;
                }
                else if (dataService.DataServiceType == DataServiceType.RemoveBackground)
                {
                    //Dùng phương thức Post
                    using (System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent())
                    {
                        foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                        {
                            // Thêm tham số model
                            // Thêm tham số response_format
                            if (parameter.Value.Contains("{") && parameter.Value.Contains("}"))
                            {
                                form.Add(new System.Net.Http.StringContent(string.Format(parameter.Value, inputs)), parameter.Name);
                            }
                            else
                            {
                                form.Add(new System.Net.Http.StringContent(parameter.Value), parameter.Name);
                            }

                        }
                        // Gửi yêu cầu POST tới API
                        //System.Net.Http.HttpResponseMessage response = client.PostAsync(Address, form).Result;
                        // Gửi yêu cầu POST để xóa nền
                        var responseTask = System.Threading.Tasks.Task.Run(() => client.PostAsync(dataService.Address, form));
                        responseTask.Wait();
                        var response = responseTask.Result;
                        response.EnsureSuccessStatusCode();

                        // Kiểm tra xem yêu cầu có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc phản hồi như một luồng nhị phân
                            var responseBytesTask = System.Threading.Tasks.Task.Run(() => response.Content.ReadAsByteArrayAsync());
                            responseBytesTask.Wait();
                            //Log
                            CreateLog(dataService, view, logContent);
                            return responseBytesTask.Result;
                        }
                        else
                        {
                            throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                        }
                    }
                }
            }
            return null;
        }
        public object GetFileResult(DataService dataService, ViewController view, object[] inputs, string[] outputs, string logContent = null)
        {
            if (!System.IO.File.Exists(dataService.Address))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Tập tin dịch vụ không tồn tại", dataService.Address, InformationType.Error);
                return null;
            }
            Module.Helpers.AudioVideoHelper.AutoInstalledFfmpeg(view.View.ObjectSpace);
            if (dataService.Name.Contains("pyannote", StringComparison.OrdinalIgnoreCase))
            {
                //Pyanote chỉ hỗ trợ wav
                if (inputs[0] is string)
                {
                    var fileName = inputs[0] as string;
                    if (fileName.StartsWith('"'))
                        fileName = fileName.Substring(1);
                    if (fileName.EndsWith('"'))
                        fileName = fileName.Substring(0, fileName.Length - 1);
                    string extension = ".wav";
                    if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        var wavFile = fileName + extension;
                        if (System.IO.File.Exists(wavFile))
                            System.IO.File.Delete(wavFile);
                        string convertArguments = $"-i \"{fileName}\" -acodec pcm_s16le -ar 16000 \"{wavFile}\"";
                        Module.Helpers.ProcessHelper.RunProcessOutside("ffmpeg", convertArguments);
                        if (System.IO.File.Exists(wavFile))
                        {
                            if (((string)inputs[0]).StartsWith('"'))
                                wavFile = '"' + wavFile + '"';
                            inputs[0] = wavFile;
                        }

                    }
                }
            }
            //Dùng dịch vụ offline
            string arguments = ParameterToString(dataService, inputs);
            Module.Helpers.ProcessHelper.RunProcessOutside(dataService.Address, arguments, 0, true);

            if (outputs != null)
            {
                foreach (var output in outputs)
                {
                    if (System.IO.File.Exists(output))
                        return System.IO.File.ReadAllText(output);
                }
            }
            return null;

        }
        public async Task<object> GetGetResultAsync(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            var queryParams = new List<string>();
            foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
            {
                var encodedName = Uri.EscapeDataString(parameter.Name);
                var encodedValue = Uri.EscapeDataString(parameter.Value?.ToString() ?? "");
                queryParams.Add($"{encodedName}={encodedValue}");
            }

            var queryString = string.Join("&", queryParams);
            var formattedUrl = string.Format(dataService.Address, inputs);
            var fullUrl = $"{formattedUrl}?{queryString}";

            using var client = new HttpClient(); // Nếu không dùng DI
            var response = await client.GetAsync(fullUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");

            var responseBytes = await response.Content.ReadAsByteArrayAsync();

            // Log
            CreateLog(dataService, view, logContent);

            return responseBytes;
        }
        public async Task<object> GetPostResultAsync(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            using (var client = new HttpClient())
            {
                var stringInputs = inputs.Select(i => i?.ToString() ?? "").ToArray();
                var parameters = GetDataServiceParameterListWithSort(dataService);

                SetDefaultRequestHeaders(client, parameters, stringInputs);
                //Lấy nội dung yêu cầu từ phương thức GetMultipartFormDataContent nếu có
                var parametersMultipartFormDataList = parameters.Where(x => x.DataServiceParameterOption == DataServiceParameterOption.MultipartFormData);
                if (parametersMultipartFormDataList.Any())
                {
                    return await GetMultipartFormDataContent(client, parametersMultipartFormDataList, stringInputs, dataService, view, logContent); //OpenAI
                }
                //Nếu không có nội dung yêu cầu thì lấy nội dung JSON
                var parametersJsonList = parameters.Where(x => x.DataServiceParameterOption == DataServiceParameterOption.Json);
                if (parametersJsonList.Any())
                {
                    return await GetJsonDataContent(client, parametersJsonList, stringInputs, dataService, view, logContent); //OpenAI
                }
                try
                {
                    if (stringInputs.Length > 0)
                    {
                        //Trường hợp FptAI
                        System.Net.Http.HttpResponseMessage response = client.PostAsync(dataService.Address, new System.Net.Http.StringContent(stringInputs[0] as string)).Result;
                        //string responseString = response.Content.ReadAsStringAsync().Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // Process the response data
                            CreateLog(dataService, view, logContent);
                            var resultUrl = response.Content.ReadAsStringAsync().Result;
                            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(resultUrl) as Newtonsoft.Json.Linq.JObject;
                            if (jsonObject.ContainsKey("async"))
                            {
                                var result = jsonObject["async"] as Newtonsoft.Json.Linq.JValue;
                                //var re = result.Value.GetType();
                                return result?.Value;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                            //Console.WriteLine("Details: " + responseString);
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }

            }
            return null;
        }
        public object GetNotSupportResult(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Phương thức này không được hỗ trợ", "Lỗi", InformationType.Error);
            return null;
        }
        public object GetBaseResult(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            //if (inputs.Length == 0) return null;
            // Read the audio file as bytes
            // Create the HTTP client
            switch (dataService.ApiMethodType)
            {
                case ApiMethodType.Get:
                    {
                        var responseTask = System.Threading.Tasks.Task.Run(() => GetGetResultAsync(dataService, view, inputs, logContent));
                        responseTask.Wait();
                        return responseTask.Result;
                    }
                case ApiMethodType.Post:
                    {
                        var responseTask = System.Threading.Tasks.Task.Run(() => GetPostResultAsync(dataService, view, inputs, logContent));
                        responseTask.Wait();
                        return responseTask.Result;
                    }
                default:
                    Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Phương thức này không được hỗ trợ", "Lỗi", InformationType.Error);
                    return null;
            }
        }



        //public object GetAudioResult(ViewController view, Voice voice,, string text, string logContent = null)
        //{
        //    if (!string.IsNullOrEmpty(Address))
        //    {
        //        if (System.IO.File.Exists(Address))
        //        {
        //            //Dùng dịch vụ offline
        //            string arguments = ParameterToString(inputs);
        //            Module.Helpers.ProcessHelper.RunProcessOutside(Address, arguments, 0, true);

        //            if (outputs != null)
        //            {
        //                foreach (var output in outputs)
        //                {
        //                    if (System.IO.File.Exists(output))
        //                        return System.IO.File.ReadAllText(output);
        //                }
        //            }
        //            return null;
        //        }
        //        else if (dataService.Address.StartsWith("http"))
        //        {
        //            //Dịch vụ online
        //            if (dataService.Address.StartsWith("https://replicate.com/") || Address.StartsWith("https://api.replicate.com/"))
        //            {
        //                return GetReplicateResult(view, inputs, logContent);
        //            }
        //            if (dataService.Address.StartsWith("https://api.openai.com"))
        //            {
        //                return GetResultFromOpenAI(view, inputs, logContent);
        //            }
        //        }
        //    }
        //    return null;
        //}
        private string ParameterToString(DataService currentDataService, object[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] is string)
                {
                    if (!((string)inputs[i]).StartsWith('"'))
                        inputs[i] = '"' + ((string)inputs[i]);
                    if (!((string)inputs[i]).EndsWith('"'))
                        inputs[i] = ((string)inputs[i]) + '"';
                }

            }
            string result = "";
            if (currentDataService.DataServiceParameterList != null && currentDataService.DataServiceParameterList.Count > 0)
            {
                bool usedParameter = false;
                foreach (var dataService in GetDataServiceParameterListWithSort(currentDataService))
                {

                    if (!string.IsNullOrEmpty(dataService.Name))
                    {
                        result += " " + dataService.Name;
                    }
                    if (!string.IsNullOrEmpty(dataService.Value))
                    {
                        if (dataService.Value.Contains("{") && dataService.Value.Contains("{"))
                        {
                            usedParameter = true;
                        }
                        result += " " + dataService.Value;
                    }
                }
                if (usedParameter)
                    result = string.Format(result, (object[])inputs);
                else
                    result += " " + string.Join(" ", inputs);//ngăn bới dấu cách

            }
            else
            {
                result += " " + string.Join(" ", inputs); //ngăn bới dấu cách
            }
            return result;

        }

        //W:\bmdhanoi\wp-content\uploads\Entos
        private string _cacheFolder = null;
        public string GetReplicateCacheFolder(DataService dataService, ViewController view)
        {
            if (_cacheFolder == null)
            {
                _cacheFolder = Module.Helpers.ParameterHelper.GetValueOrDefault(dataService.Session, "ReplicateCacheFolder", "\\\\dc\\Websites$\\bmdhanoi\\wp-content\\uploads\\Entos");
            }
            return _cacheFolder;
        }

        private string _httpFolder = null;
        public string GetReplicateHttpFolder(DataService dataService, ViewController view)
        {
            if (_httpFolder == null)
            {
                _httpFolder = Module.Helpers.ParameterHelper.GetValueOrDefault(dataService.Session, "ReplicateHttpFolder", "https://habao.vn/wp-content/uploads/Entos");
            }
            return _httpFolder;
        }

        private string GetModelVersion(DataService dataService)
        {
            return GetDataServiceParameterListWithSort(dataService).FirstOrDefault(x => x.Name == "version")?.Value;
        }
        public string GetReplicateResult(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            var cacheFolder = GetReplicateCacheFolder(dataService, view);
            if (!System.IO.Directory.Exists(cacheFolder))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Thư mục cache của Replicate không tồn tại", cacheFolder, InformationType.Error);
                return null;
            }
            var fileName = GetRealFileName(inputs[0] as string);
            if (!System.IO.File.Exists(fileName))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Không tồn tại tập tin", fileName, InformationType.Error);
                return null;
            }
            var modelVersion = GetModelVersion(dataService);
            if (string.IsNullOrEmpty(modelVersion))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Không có phiên bản model", null, InformationType.Error);
                return null;
            }
            var fileInfo = new System.IO.FileInfo(fileName);
            //copy file to cache folder
            var cacheFileName = System.IO.Path.Combine(cacheFolder, System.IO.Path.GetFileName(fileInfo.Name));
            System.IO.File.Copy(fileName, cacheFileName, true);
            var httpFileName = System.IO.Path.Combine(GetReplicateHttpFolder(dataService, view), fileInfo.Name);
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            //string apiToken = ""; // API token của bạn
            //string modelEndpoint = "https://api.replicate.com/v1/predictions";
            //string modelVersion = "be69de6b9dc57b3361dff4122ef4d6876ad4234bf5c879287b48d35c20ce3e83";
            //string audioUrl = "https://habao.vn/wp-content/uploads/Motivation.mp3"; // URL của tệp âm thanh
            var inputRequestData = new System.Collections.Generic.Dictionary<string, string>();
            foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
            {
                if (parameter.Name == "version") continue;
                inputRequestData.Add(parameter.Name, string.Format(parameter.Value, httpFileName));
            }
            var requestData = new
            {
                version = modelVersion,
                input = inputRequestData
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", dataService.APIKey);

            try
            {
                System.Net.Http.HttpResponseMessage response = client.PostAsync(dataService.Address, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Success: " + responseString);

                    // Xử lý dữ liệu JSON để lấy các segment
                    var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(responseString);

                    if (responseData.TryGetValue("output", out var output) && output != null)
                    {
                        return output.ToString();
                        // Nếu output là mảng JSON, bạn có thể chuyển đổi nó thành danh sách các segment
                        //var segments = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>>(output.ToString());

                        //foreach (var segment in segments)
                        //{
                        //    Console.WriteLine($"Segment: {segment}");
                        //}
                    }
                    else
                    {
                        Console.WriteLine("No output found in the response.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    Console.WriteLine("Details: " + responseString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            if (System.IO.File.Exists(cacheFileName))
            {
                System.IO.File.Delete(cacheFileName);
            }
            return null;
        }
        public void CreateLog(DataService dataService, ViewController view, string logContent, int? quantity = null)
        {
            if (view is null) return;
            var logObjectSpace = view.Application.CreateObjectSpace();
            var dataServiceLog = logObjectSpace.CreateObject<DataServiceLog>();
            //Ngăn chặn bị dài quá
            if (!string.IsNullOrEmpty(logContent) && logContent.Length > 250)
                logContent = logContent.Substring(0, 250);
            dataServiceLog.Note = logContent;
            dataServiceLog.DataService = logObjectSpace.GetObjectByKey<DataService>(dataService.Oid);
            dataServiceLog.Quantity = quantity;
            dataServiceLog.CreatedDate = (DateTime)logObjectSpace.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
            dataServiceLog.Creator = logObjectSpace.GetObjectByKey<Member>(SecuritySystem.CurrentUserId);
        }

        private string GetRealFileName(string fileName)
        {
            if (fileName.StartsWith('"'))
                fileName = fileName.Substring(1);
            if (fileName.EndsWith('"'))
                fileName = fileName.Substring(0, fileName.Length - 1);
            return fileName;
        }

        public object GetResultFromOpenAI(DataService dataService, ViewController view, object[] inputs, string logContent = null)
        {
            if (inputs.Length == 0) return null;
            //Loại bỏ 2 dấu ngoặc kép ở đầu và cuối nếu có

            //if (DataServiceType == DataServiceType.STT)
            //{
            //    var audioClient = new OpenAI.Audio.AudioClient("whisper-1", APIKey);
            //    var translation = audioClient.TranscribeAudio(fileName, new OpenAI.Audio.AudioTranscriptionOptions { ResponseFormat = AudioTranscriptionFormat.Srt, Granularities = AudioTimestampGranularities.Segment});
            //    return translation.Value?.Text;
            //}


            //string openAIKey = "YOUR_OPENAI_API_KEY"; // Your OpenAI API key

            // Read the audio file as bytes
            // Create the HTTP client
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                // Set the OpenAI API key in the request headers
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dataService.APIKey);
                //string openAIEndpoint = "https://api.openai.com/v1/audio/transcriptions"; // OpenAI API endpoint
                // Create the multipart form data content
                if (dataService.DataServiceType == DataServiceType.STT)
                {
                    using (System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent())
                    {

                        // Đọc file âm thanh
                        if (dataService.DataServiceType == DataServiceType.STT)
                        {
                            string fileName = inputs[0] as string;
                            fileName = GetRealFileName(fileName);
                            if (!System.IO.File.Exists(fileName)) return null;
                            var audioFileInfo = new System.IO.FileInfo(fileName);
                            var fileContent = new System.Net.Http.ByteArrayContent(File.ReadAllBytes(fileName));
                            // Tự động nhận diện Content-Type dựa trên định dạng của file
                            string mimeType = audioFileInfo.Extension switch
                            {
                                ".mp3" => "audio/mpeg",
                                ".wav" => "audio/wav",
                                ".ogg" => "audio/ogg",
                                ".flac" => "audio/flac",
                                ".mp4" => "audio/mpeg",
                                _ => "application/octet-stream",
                            };
                            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(mimeType);

                            // Thêm file vào nội dung của form
                            form.Add(fileContent, "file", Path.GetFileName(fileName));
                        }
                        foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                        {
                            // Thêm tham số model
                            // Thêm tham số response_format
                            form.Add(new System.Net.Http.StringContent(parameter.Value), parameter.Name);
                        }

                        // Thêm tham số model
                        //form.Add(new StringContent(model), "model");

                        // Thêm tham số response_format
                        //form.Add(new StringContent(responseFormat), "response_format");

                        // Gửi yêu cầu POST tới API
                        System.Net.Http.HttpResponseMessage response = client.PostAsync(dataService.Address, form).Result;

                        // Kiểm tra xem yêu cầu có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung phản hồi từ API
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            //Log
                            CreateLog(dataService, view, logContent);
                            return responseData;
                        }
                        else
                        {
                            throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                        }
                    }
                }
                else if (dataService.DataServiceType == DataServiceType.TTS)
                {
                    //var requestData = new
                    //{
                    //    input = inputs[0] as string,
                    //    voice = inputs[1] as string,
                    //    speed = inputs[2] as string
                    //};
                    //var re1 = requestData.GetType();
                    //var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                    var requestData2 = new System.Collections.Generic.Dictionary<string, string>();
                    requestData2.Add("input", inputs[0] as string);
                    requestData2.Add("voice", inputs[1] as string);
                    requestData2.Add("speed", inputs[2] as string);
                    foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                    {
                        requestData2.Add(parameter.Name, parameter.Value);
                    }

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData2);
                    var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    try
                    {
                        System.Net.Http.HttpResponseMessage response = client.PostAsync("https://api.openai.com/v1/audio/speech", content).Result;
                        //string responseString = response.Content.ReadAsStringAsync().Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // Process the response data
                            CreateLog(dataService, view, logContent);
                            return response.Content.ReadAsByteArrayAsync().Result;
                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                            //Console.WriteLine("Details: " + responseString);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);
                    }

                    return null;
                }
            }
            return null;
        }


        public object GetResultFromFptAI(DataService dataService, ViewController view, object[] inputs, string logContent = null, int maxSecondWait = 600)
        {
            if (inputs.Length == 0) return null;


            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                // Set the OpenAI API key in the request headers                
                client.DefaultRequestHeaders.Add("api-key", dataService.APIKey);
                client.DefaultRequestHeaders.Add("voice", inputs[1] as string);
                client.DefaultRequestHeaders.Add("speed", inputs[2] as string);
                //String result = SendRequestAsync(audio.Content, cultureInfo).GetAwaiter().GetResult();
                try
                {
                    System.Net.Http.HttpResponseMessage response = client.PostAsync(dataService.Address, new System.Net.Http.StringContent(inputs[0] as string)).Result;
                    //string responseString = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Process the response data
                        CreateLog(dataService, view, logContent);
                        var resultUrl = response.Content.ReadAsStringAsync().Result;
                        var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(resultUrl) as Newtonsoft.Json.Linq.JObject;
                        if (jsonObject.ContainsKey("async"))
                        {
                            var result = jsonObject["async"] as Newtonsoft.Json.Linq.JValue;
                            //var re = result.Value.GetType();
                            return result?.Value;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        //Console.WriteLine("Details: " + responseString);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
            return null;
        }

        public object GetResultFromPiperOffline(DataService dataService, ViewController view, object[] inputs, string logContent = null, int maxSecondWait = 600)
        {
            if (inputs.Length == 0)
                return null;
            try
            {

                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string executableLocation = null;
                string workingDirectory = null;
                foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                {
                    if (parameter.Name == "ExecutableLocation")
                    {
                        executableLocation = parameter.Value;
                    }
                    else if (parameter.Name == "WorkingDirectory")
                    {
                        workingDirectory = parameter.Value;
                    }
                }
                if (string.IsNullOrEmpty(executableLocation))
                    executableLocation = System.IO.Path.Join(System.IO.Directory.GetCurrentDirectory(), "piper", "piper.exe");
                var fileInfo = new System.IO.FileInfo(executableLocation);
                if (string.IsNullOrEmpty(workingDirectory))
                    workingDirectory = fileInfo.Directory.FullName;
                //var executableLocation = System.IO.Path.Join(currentDirectory, "piper", "piper.exe");                
                if (!System.IO.File.Exists(executableLocation))
                {

                    Tools.ShowOrCloseDefaultWaitForm("Đang tải piper...", " ");
                    //Fix lỗi download bị treo
                    var downloadTask = System.Threading.Tasks.Task.Run(() => PiperSharp.PiperDownloader.DownloadPiper());
                    downloadTask.Wait();
                    var piperPaths = PiperSharp.PiperDownloader.ExtractPiper(downloadTask.Result, fileInfo.Directory.Parent.FullName);
                    //await PiperDownloader.DownloadPiper().ExtractPiper(currentDirectory); // Downloads and extracts piper to cwd/piper directory                          
                    // Downloads and extracts piper to cwd/piper directory
                }


                // You can get list of models from hugging face using
                //var models = PiperSharp.PiperDownloader.GetHuggingFaceModelList().Result; // Returns a dictionary with model key as key
                PiperSharp.Models.VoiceModel model = null;
                string modelName = inputs[1] as string;
                //modelName = "vi_VN-vivos-x_low";
                //var model = models[modelName];

                // or you can do
                //var model = await PiperDownloader.GetModelByKey("ar_JO-kareem-low");

                // Before you can use the model you need to download it using
                //model = await model.DownloadModel(cwd);
                // Or if its downloaded you can load it from directory
                //model = await VoiceModel.LoadModel(modelPath);

                // Now you can also do
                if (!System.IO.File.Exists(modelName) && !System.IO.Directory.Exists(System.IO.Path.Join(currentDirectory, "models", modelName)))
                {
                    Tools.ShowOrCloseDefaultWaitForm("Đang tải model giọng...", " ");
                    var downloadTask = System.Threading.Tasks.Task.Run(() => PiperSharp.PiperDownloader.DownloadModelByKey(modelName));
                    downloadTask.Wait();
                    model = downloadTask.Result;
                }
                Tools.ShowOrCloseDefaultWaitForm("Đang nạp model giọng...", inputs[0] as string);
                //Đang load model
                // Or if its downloaded you can load it by key aswell
                var loadTask = System.Threading.Tasks.Task.Run(() => PiperSharp.Models.VoiceModel.LoadModelByKey(modelName));
                loadTask.Wait();
                model = loadTask.Result;
                //model = await PiperSharp.Models.VoiceModel.LoadModelByKey(modelName);

                Tools.ShowOrCloseDefaultWaitForm("Đang tạo âm thanh...", inputs[0] as string);
                // To start generating audio use PiperProvider
                var piperOption = new PiperSharp.Models.PiperConfiguration()
                {
                    ExecutableLocation = executableLocation, //System.IO.Path.Join(executableLocation, "piper", "piper.exe"), // Path to piper executable
                    WorkingDirectory = workingDirectory, //System.IO.Path.Join(workingDirectory, "piper"), // Path to piper working directory
                    Model = model, // Loaded/downloaded VoiceModel
                                   //SpeakerId = System.Convert.ToUInt32(44)
                };
                var speed = System.Convert.ToDouble(inputs[2], new System.Globalization.CultureInfo("en"));
                if (speed > 1 && model.NumSpeakers > speed)
                {
                    //Dùng trường speed làm giọng
                    piperOption.SpeakerId = System.Convert.ToUInt32(speed);
                }
                foreach (var parameter in GetDataServiceParameterListWithSort(dataService))
                {
                    if (parameter.Name == "SpeakerId")
                    {
                        piperOption.SpeakerId = System.Convert.ToUInt32(parameter.Value);
                    }
                    else if (parameter.Name == "SpeakingRate")
                    {
                        piperOption.SpeakingRate = System.Convert.ToSingle(parameter.Value);
                    }
                    else if (parameter.Name == "UseCuda")
                    {
                        piperOption.UseCuda = System.Convert.ToBoolean(parameter.Value);
                    }
                    else if (parameter.Name == "ExecutableLocation")
                    {
                        piperOption.ExecutableLocation = parameter.Value;
                    }
                    else if (parameter.Name == "WorkingDirectory")
                    {
                        piperOption.WorkingDirectory = parameter.Value;
                    }
                }
                var piperModel = new PiperSharp.PiperProvider(piperOption);

                // Generate audio, currently supported formats are Mp3, Wav, Raw
                //await piperModel.InferAsync(inputs[0] as string, PiperSharp.Models.AudioOutputType.Mp3);
                var inferTask = System.Threading.Tasks.Task.Run(() => piperModel.InferAsync(inputs[0] as string, PiperSharp.Models.AudioOutputType.Mp3));
                inferTask.Wait();
                Tools.ShowOrCloseDefaultWaitForm(null, null);
                return inferTask.Result;
            }
            catch (System.Exception ex)
            {
                Tools.ShowOrCloseDefaultWaitForm(null, null);
                throw ex;
            }
            return null;
        }

        public System.Linq.IOrderedEnumerable<DataServiceParameter> GetDataServiceParameterListWithSort(DataService dataService)
        {
            return dataService.DataServiceParameterList.Where(a => !a.InActive).OrderBy(x => System.Convert.ToInt32(x.Order));
        }

        private void SetDefaultRequestHeaders(System.Net.Http.HttpClient client, System.Linq.IOrderedEnumerable<DataServiceParameter> dataServiceParameters, object[] inputs)
        {

            foreach (var dataServiceParameter in dataServiceParameters)
            {
                switch (dataServiceParameter.DataServiceParameterOption)
                {

                    case DataServiceParameterOption.DefaultRequestHeaders:
                        client.DefaultRequestHeaders.Add(dataServiceParameter.Name, GetParametersValue(dataServiceParameter, inputs));
                        break;
                    case DataServiceParameterOption.AuthenticationHeaderValue:
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(dataServiceParameter.Name, GetParametersValue(dataServiceParameter, inputs));
                        break;
                    default:
                        continue;
                }
            }

        }

        private async Task<object> GetMultipartFormDataContent(System.Net.Http.HttpClient client, System.Collections.Generic.IEnumerable<DataServiceParameter> dataServiceParameters, object[] inputs, DataService dataService, ViewController view, string logContent = null)
        {
            using (System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent())
            {
                foreach (var dataServiceParameter in dataServiceParameters)
                {
                    form.Add(new System.Net.Http.StringContent(GetParametersValue(dataServiceParameter, inputs)), dataServiceParameter.Name);
                }
                // Gửi yêu cầu POST
                var response = await client.PostAsync(dataService.Address, form);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                // Log
                CreateLog(dataService, view, logContent);
                return responseBytes;
            }

            return null;
        }
        private async Task<object> GetJsonDataContent(System.Net.Http.HttpClient client, System.Collections.Generic.IEnumerable<DataServiceParameter> dataServiceParameters, object[] inputs, DataService dataService, ViewController view, string logContent = null)
        {
            string json = null;
            if (dataServiceParameters.Count() == 1)
            {
                json = GetParametersValue(dataServiceParameters.First(), inputs);
            }
            else
            {
                var requestData2 = new System.Collections.Generic.Dictionary<string, string>();
                foreach (var parameter in dataServiceParameters)
                {
                    requestData2.Add(parameter.Name, GetParametersValue(parameter, inputs));
                }
                json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData2);
            }


            var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                System.Net.Http.HttpResponseMessage response = await client.PostAsync(dataService.Address, content);
                //string responseString = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    // Process the response data
                    CreateLog(dataService, view, logContent);
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    //Console.WriteLine("Details: " + responseString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return null;
        }

        private string GetParametersValue(DataServiceParameter parameter, object[] inputs)
        {
            if (string.IsNullOrEmpty(parameter.Value))
                return parameter.Value;

            if (parameter.Value.Contains("{") && parameter.Value.Contains("}"))
            {
                StringBuilder stringBuilder = new StringBuilder(parameter.Value);
                //Lấy giá trị từ các tham số khác                
                foreach (var dataServiceParameter in parameter.DataService?.DataServiceParameterList.Where(x => !x.InActive && x.DataServiceParameterOption == DataServiceParameterOption.Empty))
                {
                    stringBuilder.Replace("{" + dataServiceParameter.Name + "}", dataServiceParameter.Value);
                }
                if (!string.IsNullOrEmpty(parameter.DataService?.APIKey))
                {
                    stringBuilder.Replace("{api-key}", parameter.DataService.APIKey);
                }
                if (inputs != null)
                {
                    for (global::System.Int32 i = 0; i < inputs.Length; i++)
                    {
                        //Không dùng format vì không thay thế đúng vị trí
                        if (inputs[i] is string)
                            stringBuilder.Replace("{" + i.ToString("D") + "}", (string)inputs[i]);
                    }
                }

                return stringBuilder.ToString();
                //return string.Format(stringBuilder.ToString(), inputs);
            }
            return parameter.Value;

        }










        #endregion SourceCode3294ImportCode

        #region SourceCode3567ImportCode
                public async Task<object> GetResultAsync(Application.DTOs.DataServiceDto dataServiceDto, object[] inputs)
        {
            using var client = Module.Helpers.HttpHelper.CreateHttpClient(30, true);
            var responseContent = await GetResultAsync(client, dataServiceDto, inputs);

            var contentType = responseContent.Headers.ContentType?.MediaType?.ToLower();

            if (contentType != null)
            {
                if (contentType.StartsWith("text/") || contentType == "application/json" || contentType == "application/xml")
                {
                    // Trả về dạng chuỗi
                    return await responseContent.ReadAsStringAsync();
                }
                else if (contentType.StartsWith("application/octet-stream") || contentType.StartsWith("application/pdf"))
                {
                    // Trả về byte[]
                    return await responseContent.ReadAsByteArrayAsync();
                }
                else if (contentType.StartsWith("image/"))
                {
                    // Trả về stream nếu là ảnh lớn
                    return await responseContent.ReadAsStreamAsync();
                }
            }

            // Mặc định: trả về byte[]
            return await responseContent.ReadAsByteArrayAsync();
        }

        public async Task<HttpContent> GetResultAsync(System.Net.Http.HttpClient client, Application.DTOs.DataServiceDto dataServiceDto, object[] inputs)
        {
            //Lọc bớt tham số không dùng
            dataServiceDto.DataServiceParameterList = dataServiceDto.DataServiceParameterList.Where(x => !x.InActive).ToList();
            //Sắp xếp tham số theo thứ tự
            dataServiceDto.DataServiceParameterList.Sort((x, y) => Nullable.Compare(x.Order, y.Order));
            //if (inputs.Length == 0) return null;
            // Read the audio file as bytes
            // Create the HTTP client
            //Tạo header
            var headerParameters = dataServiceDto.DataServiceParameterList.Where(x => x.DataServiceParameterOption == DataServiceParameterOption.DefaultRequestHeaders).ToList();
            var headers = GetDefaultHeaders(dataServiceDto, headerParameters, inputs);
            //Tạo AuthenticationHeaderValue nếu có
            var authenticationHeaderValue = GetAuthenticationHeaderValue(dataServiceDto, inputs);
            //Tạo các loại Header khác
            Module.Helpers.HttpHelper.ApplyCustomHeaders(client, headers, authenticationHeaderValue);
            switch (dataServiceDto.ApiMethodType)
            {
                case ApiMethodType.Get:
                    {
                        return await GetGetResultAsync(client, dataServiceDto, inputs);
                    }
                case ApiMethodType.Post:
                    {
                        return await GetPostResultAsync(client, dataServiceDto, inputs);
                    }
                default:
                    throw new Exception($"Phương thức này không được hỗ trợ:  {dataServiceDto.ApiMethodType}");
            }
        }

        private async Task<HttpContent> GetPostResultAsync(System.Net.Http.HttpClient client, Application.DTOs.DataServiceDto dataServiceDto, object[] inputs)
        {
            var parameters = dataServiceDto.DataServiceParameterList;

            //Lấy nội dung yêu cầu từ phương thức GetMultipartFormDataContent nếu có
            var parametersMultipartFormDataList = parameters.Where(x => x.DataServiceParameterOption == DataServiceParameterOption.MultipartFormData || x.DataServiceParameterOption == DataServiceParameterOption.FileMultipartFormData);
            if (parametersMultipartFormDataList.Any())
            {
               
                return await GetMultipartFormDataContent(client, inputs, dataServiceDto, parametersMultipartFormDataList); //OpenAI
            }
            var stringInputs = inputs.Select(i => i?.ToString() ?? "").ToArray();
            //Nếu không có nội dung yêu cầu thì lấy nội dung JSON
            var parametersJsonList = parameters.Where(x => x.DataServiceParameterOption == DataServiceParameterOption.Json);
            if (parametersJsonList.Any())
            {
                return await GetJsonDataContent(client, dataServiceDto, parametersJsonList, stringInputs); //OpenAI
            }
            if (stringInputs.Length > 0)
            {
                //Trường hợp FptAI
                var response = await client.PostAsync(dataServiceDto.Address, new System.Net.Http.StringContent(stringInputs[0] as string));
                //string responseString = response.Content.ReadAsStringAsync().Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
                return response.Content;

            }
            return null;
        }

        private async Task<HttpContent> GetGetResultAsync(System.Net.Http.HttpClient client, Application.DTOs.DataServiceDto dataServiceDto, object[] inputs, string logContent = null)
        {
            var queryParams = new List<string>();
            foreach (var parameter in dataServiceDto.DataServiceParameterList)
            {
                var encodedName = Uri.EscapeDataString(parameter.Name);
                var encodedValue = GetParametersValue(dataServiceDto, parameter, inputs);
                if (!string.IsNullOrEmpty(encodedValue) && encodedValue.Contains("{"))//Thiếu tham số thì bỏ qua
                    continue;
                queryParams.Add($"{encodedName}={encodedValue}");
            }
            Dictionary<string, string> headers = new Dictionary<string, string>();

            var queryString = string.Join("&", queryParams);
            var formattedUrl = string.Format(dataServiceDto.Address, inputs);
            var separator = formattedUrl.Contains("?") ? "&" : "?";
            var fullUrl = $"{formattedUrl}{separator}{queryString}";


            var response = await client.GetAsync(fullUrl);
            if (inputs.Count() > 0)
                System.Diagnostics.Debug.WriteLine($"DataServiceService.GetGetResultAsync Gửi dữ liệu: {inputs[0].ToString().Substring(0, 10)} Sử dụng trình duyệt: {client.DefaultRequestHeaders.UserAgent} ");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
            if (inputs.Count() > 0)
                System.Diagnostics.Debug.WriteLine($"DataServiceService.GetGetResultAsync Nhận dữ liệu: {inputs[0].ToString().Substring(0, 10)} Sử dụng trình duyệt: {client.DefaultRequestHeaders.UserAgent} ");
            return response.Content;
        }

        private void SetDefaultRequestHeaders(System.Net.Http.HttpClient client, Application.DTOs.DataServiceDto dataServiceDto, IList<Application.DTOs.DataServiceParameterDto> dataServiceParameters, object[] inputs)
        {

            foreach (var dataServiceParameter in dataServiceParameters)
            {
                switch (dataServiceParameter.DataServiceParameterOption)
                {

                    case DataServiceParameterOption.DefaultRequestHeaders:
                        client.DefaultRequestHeaders.Add(dataServiceParameter.Name, GetParametersValue(dataServiceDto, dataServiceParameter, inputs));
                        break;
                    case DataServiceParameterOption.AuthenticationHeaderValue:
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(dataServiceParameter.Name, GetParametersValue(dataServiceDto, dataServiceParameter, inputs));
                        break;
                    default:
                        continue;
                }
            }

        }
        private System.Net.Http.Headers.AuthenticationHeaderValue GetAuthenticationHeaderValue(Application.DTOs.DataServiceDto dataServiceDto, object[] inputs)
        {
            var firstAuthHeader = dataServiceDto.DataServiceParameterList
                .FirstOrDefault(x => x.DataServiceParameterOption == DataServiceParameterOption.AuthenticationHeaderValue);
            if (firstAuthHeader != null)
                return new System.Net.Http.Headers.AuthenticationHeaderValue(
                    firstAuthHeader.Name,
                    GetParametersValue(dataServiceDto, firstAuthHeader, inputs));
            return null;
        }

        private Dictionary<string, string> GetDefaultHeaders(Application.DTOs.DataServiceDto dataServiceDto, IList<Application.DTOs.DataServiceParameterDto> dataServiceParameters, object[] inputs)
        {

            var defaultHeaders = new Dictionary<string, string>();
            foreach (var dataServiceParameter in dataServiceParameters)
            {
                if (defaultHeaders.ContainsKey(dataServiceParameter.Name))
                    continue; // Tránh trùng lặp tên header
                defaultHeaders.Add(dataServiceParameter.Name, GetParametersValue(dataServiceDto, dataServiceParameter, inputs));
            }
            return defaultHeaders;
        }

        private string GetParametersValue(Application.DTOs.DataServiceDto dataServiceDto, Application.DTOs.DataServiceParameterDto parameter, object[] inputs)
        {
            if (string.IsNullOrEmpty(parameter.Value))
                return parameter.Value;

            if (parameter.Value.Contains("{") && parameter.Value.Contains("}"))
            {
                StringBuilder stringBuilder = new StringBuilder(parameter.Value);
                //Lấy giá trị từ các tham số khác                
                foreach (var dataServiceParameter in dataServiceDto.DataServiceParameterList.Where(x => !x.InActive && x.DataServiceParameterOption == DataServiceParameterOption.Empty))
                {
                    stringBuilder.Replace("{" + dataServiceParameter.Name + "}", dataServiceParameter.Value);
                }
                if (!string.IsNullOrEmpty(dataServiceDto?.APIKey))
                {
                    stringBuilder.Replace("{api-key}", dataServiceDto.APIKey);
                }
                if (inputs != null)
                {
                    for (global::System.Int32 i = 0; i < inputs.Length; i++)
                    {
                        //Không dùng format vì không thay thế đúng vị trí
                        if (inputs[i] is string)
                            stringBuilder.Replace("{" + i.ToString("D") + "}", (string)inputs[i]);
                    }
                }

                return stringBuilder.ToString();
                //return string.Format(stringBuilder.ToString(), inputs);
            }
            return parameter.Value;

        }

        private async Task<HttpContent> GetMultipartFormDataContent(System.Net.Http.HttpClient client, object[] inputs, Application.DTOs.DataServiceDto dataServiceDto, System.Collections.Generic.IEnumerable<Application.DTOs.DataServiceParameterDto> dataServiceParameters)
        {
            using var form = new System.Net.Http.MultipartFormDataContent();
            foreach (var dataServiceParameter in dataServiceParameters)
            {
                if (dataServiceParameter.DataServiceParameterOption == DataServiceParameterOption.MultipartFormData)
                {
                    if (inputs[0] is byte[] && dataServiceParameter.Value.Contains("{0}"))
                    {
                        var fileContent = new ByteArrayContent((byte[])inputs[0]);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        form.Add(fileContent, dataServiceParameter.Name, "upload.png"); // 👈 thêm filename
                    }

                    else
                        form.Add(new System.Net.Http.StringContent(GetParametersValue(dataServiceDto, dataServiceParameter, inputs)), dataServiceParameter.Name);
                }
                else
                {
                    string fileName = inputs[0] as string;
                    fileName = GetRealFileName(fileName);
                    if (!System.IO.File.Exists(fileName)) return null;
                    //Gán giá trị file nếu có
                    var fileContent = new System.Net.Http.ByteArrayContent(System.IO.File.ReadAllBytes(fileName));
                    var fileInfo = new System.IO.FileInfo(fileName);
                    string mimeType = GetMimeType(fileName);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                    form.Add(fileContent, dataServiceParameter.Name, System.IO.Path.GetFileName(GetParametersValue(dataServiceDto, dataServiceParameter, inputs)));
                }


            }
            // Gửi yêu cầu POST
            var response = await client.PostAsync(dataServiceDto.Address, form);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
            return response.Content;
        }

        public static string GetMimeType(string filePathOrExtension)
        {
            // Nếu truyền đường dẫn, lấy phần mở rộng
            string ext = Path.GetExtension(filePathOrExtension)?.ToLowerInvariant();

            return ext switch
            {
                // Audio
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".ogg" => "audio/ogg",
                ".flac" => "audio/flac",
                ".aac" => "audio/aac",
                ".m4a" => "audio/mp4",

                // Video
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".mov" => "video/quicktime",
                ".wmv" => "video/x-ms-wmv",
                ".webm" => "video/webm",
                ".mkv" => "video/x-matroska",

                // Image
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".tiff" => "image/tiff",
                ".ico" => "image/vnd.microsoft.icon",

                // Text
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".html" or ".htm" => "text/html",
                ".css" => "text/css",
                ".js" => "text/javascript",
                ".json" => "application/json",
                ".xml" => "application/xml",

                // Document
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

                // Archive
                ".zip" => "application/zip",
                ".rar" => "application/vnd.rar",
                ".7z" => "application/x-7z-compressed",
                ".tar" => "application/x-tar",
                ".gz" => "application/gzip",

                // Code
                ".cs" => "text/plain",
                ".java" => "text/plain",
                ".py" => "text/plain",
                ".cpp" => "text/plain",
                ".c" => "text/plain",
                ".jsonld" => "application/ld+json",

                // Default
                _ => "application/octet-stream"
            };
        }


        private async Task<HttpContent> GetJsonDataContent(System.Net.Http.HttpClient client, Application.DTOs.DataServiceDto dataServiceDto, System.Collections.Generic.IEnumerable<Application.DTOs.DataServiceParameterDto> dataServiceParameters, object[] inputs)
        {
            string json = null;
            if (dataServiceParameters.Count() == 1)
            {
                json = GetParametersValue(dataServiceDto, dataServiceParameters.First(), inputs);
            }
            else
            {
                var requestData2 = new System.Collections.Generic.Dictionary<string, string>();
                foreach (var parameter in dataServiceParameters)
                {
                    requestData2.Add(parameter.Name, GetParametersValue(dataServiceDto, parameter, inputs));
                }
                json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData2);
            }
            var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
            System.Net.Http.HttpResponseMessage response = await client.PostAsync(dataServiceDto.Address, content);
            //string responseString = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy dữ liệu từ API. Mã trạng thái: {response.StatusCode}");
            return response.Content;

        }






        #endregion SourceCode3567ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
        //    return result;
        //}
		
		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AddressToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (Address != null) 
		//			return Address;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ServiceCodeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (ServiceCode != null) 
		//			return ServiceCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataServiceTypeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (DataServiceType != null) 
		//			return DataServiceType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ApiMethodTypeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (ApiMethodType != null) 
		//			return ApiMethodType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object APIKeyToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (APIKey != null) 
		//			return APIKey;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PreviousDataServiceToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (PreviousDataService != null) 
		//			return PreviousDataService;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MaxConcurrencyToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (MaxConcurrency != null) 
		//			return MaxConcurrency;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SourceCodeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (SourceCode != null) 
		//			return SourceCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PauseToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (Pause != null) 
		//			return Pause;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IsDefaultToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (IsDefault != null) 
		//			return IsDefault;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataServiceParameterListToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (DataServiceParameterList != null) 
		//			return DataServiceParameterList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VoiceListToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (VoiceList != null) 
		//			return VoiceList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareServiceTypeToolTipControllerText(View view, Module.BusinessObjects.DataService dataservice)
        //{
        //    if (SoftwareServiceType != null) 
		//			return SoftwareServiceType;
        //    return null;
        //}
    

	    #endregion
  

    }
}
