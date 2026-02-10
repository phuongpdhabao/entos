using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper hỗ trợ thao tác HTTP cơ bản.
    /// </summary>
    public static class HttpHelper
    {

        private static readonly Random _random = new Random();
        private static readonly SemaphoreSlim _rateLimitSemaphore = new SemaphoreSlim(20, 20); // Giới hạn 5 request đồng thời
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static readonly object _lockObject = new object();

        private static readonly string[] _userAgents = {
            // Chrome trên Windows
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36",

            // Edge mới trên Windows
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0",

            // Firefox trên Windows
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:127.0) Gecko/20100101 Firefox/127.0",

            // Chrome trên macOS
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36",

            // Safari trên macOS
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Safari/605.1.15",

            // Firefox trên macOS
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5; rv:127.0) Gecko/20100101 Firefox/127.0",

            // Chrome trên Linux
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36",

            // Firefox trên Linux
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:127.0) Gecko/20100101 Firefox/127.0",

            // Safari trên iPhone
            //"Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Mobile/15E148 Safari/604.1",

            // Chrome trên Android
            //"Mozilla/5.0 (Linux; Android 13; Pixel 6 Pro) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Mobile Safari/537.36",

            // Samsung Internet
            //"Mozilla/5.0 (Linux; Android 13; SAMSUNG SM-G991B) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/21.0 Chrome/125.0.0.0 Mobile Safari/537.36",

            // Opera
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 OPR/100.0.0.0",

            // Brave (dựa trên Chromium)
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Brave/125.0.0.0",

        };


        private static void SetRandomHeaders(HttpClient client, bool useRandomUserAgent = true)
        {
            if (useRandomUserAgent)
            {
                var userAgent = _userAgents[_random.Next(_userAgents.Length)];
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            }
            else
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Edge/120.0.0.0 Safari/537.36");
            }
#if DEBUG
            return;
#endif
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            client.DefaultRequestHeaders.Connection.Add("keep-alive");

            // Thêm headers chống phát hiện
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private static void SetDefaultHeaders(HttpClient client)
        {
            SetRandomHeaders(client, false);
        }

        private static async Task ApplyRateLimit(int minDelayMs = 500, int maxDelayMs = 2000)
        {
            await _rateLimitSemaphore.WaitAsync();

            lock (_lockObject)
            {
                var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
                var minDelay = TimeSpan.FromMilliseconds(minDelayMs);

                if (timeSinceLastRequest < minDelay)
                {
                    var delayNeeded = minDelay - timeSinceLastRequest;
                    Task.Delay(delayNeeded).Wait();
                }

                // Random delay để tránh pattern
                var randomDelay = _random.Next(minDelayMs, maxDelayMs);
                Task.Delay(randomDelay).Wait();

                _lastRequestTime = DateTime.Now;
            }
        }

        private static void ReleaseRateLimit()
        {
            _rateLimitSemaphore.Release();
        }

        public static HttpClient CreateHttpClient(int timeoutSeconds = 30, bool useRandomUserAgent = false, bool useProxy = false, string proxyAddress = null)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            // Cấu hình proxy nếu cần
            if (useProxy && !string.IsNullOrEmpty(proxyAddress))
            {
                handler.Proxy = new WebProxy(proxyAddress);
                handler.UseProxy = true;
            }

            // Tắt tự động redirect để kiểm soát tốt hơn
            handler.AllowAutoRedirect = false;

            var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

            if (useRandomUserAgent)
                SetRandomHeaders(client, true);
            else
                SetDefaultHeaders(client);

            return client;
        }

        public static void ApplyCustomHeaders(HttpClient client, Dictionary<string, string> headers, AuthenticationHeaderValue authenticationHeaderValue)
        {
            if (authenticationHeaderValue != null)
                client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            if (headers != null)
                foreach (var h in headers)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(h.Key, h.Value);
        }

        public static HttpClient CreateConfiguredHttpClient(Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30, bool useRandomUserAgent = false, bool useProxy = false, string proxyAddress = null)
        {
            var client = CreateHttpClient(timeoutSeconds, useRandomUserAgent, useProxy, proxyAddress);
            ApplyCustomHeaders(client, headers, authenticationHeaderValue);
            return client;
        }

        /// <summary>
        /// Gửi yêu cầu GET với chống block.
        /// </summary>
        public static async Task<HttpResponseMessage> GetWithAntiBlockAsync(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30, bool useRandomUserAgent = true, int minDelayMs = 500, int maxDelayMs = 2000)
        {
            await ApplyRateLimit(minDelayMs, maxDelayMs);
            try
            {
                using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds, useRandomUserAgent);
                return await client.GetAsync(url);
            }
            finally
            {
                ReleaseRateLimit();
            }
        }

        /// <summary>
        /// Gửi yêu cầu POST với chống block.
        /// </summary>
        public static async Task<HttpResponseMessage> PostWithAntiBlockAsync(string url, string content, string contentType = "application/json", Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30, bool useRandomUserAgent = true, int minDelayMs = 500, int maxDelayMs = 2000)
        {
            await ApplyRateLimit(minDelayMs, maxDelayMs);
            try
            {
                using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds, useRandomUserAgent);
                var httpContent = new StringContent(content, Encoding.UTF8, contentType);
                return await client.PostAsync(url, httpContent);
            }
            finally
            {
                ReleaseRateLimit();
            }
        }

        /// <summary>
        /// Gửi request với retry và exponential backoff.
        /// </summary>
        public static async Task<HttpResponseMessage> GetWithRetryAndAntiBlockAsync(string url, int maxRetries = 3, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            var delay = 1000; // 1 giây ban đầu
            Exception lastException = null;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await GetWithAntiBlockAsync(url, headers, authenticationHeaderValue, timeoutSeconds, true, 1000, 3000);
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("503")) // Rate limited hoặc service unavailable
                {
                    lastException = ex;
                    if (i == maxRetries - 1) break;

                    // Exponential backoff với jitter
                    var jitter = _random.Next(0, 1000);
                    await Task.Delay(delay + jitter);
                    delay *= 2; // Tăng gấp đôi thời gian chờ
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (i == maxRetries - 1) break;
                    await Task.Delay(delay);
                }
            }

            throw lastException ?? new HttpRequestException("Max retries exceeded");
        }

        /// <summary>
        /// Crawl nhiều URL với rate limiting.
        /// </summary>
        public static async Task<Dictionary<string, string>> CrawlMultipleUrlsAsync(IEnumerable<string> urls, int maxConcurrent = 2, int delayBetweenRequests = 2000)
        {
            var results = new Dictionary<string, string>();
            var semaphore = new SemaphoreSlim(maxConcurrent, maxConcurrent);

            var tasks = urls.Select(async url =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await Task.Delay(_random.Next(1000, delayBetweenRequests)); // Random delay
                    using var client = CreateConfiguredHttpClient(useRandomUserAgent: true);
                    var response = await client.GetStringAsync(url);
                    lock (results)
                    {
                        results[url] = response;
                    }
                }
                catch (Exception ex)
                {
                    lock (results)
                    {
                        results[url] = $"Error: {ex.Message}";
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            return results;
        }

        /// <summary>
        /// Gửi yêu cầu GET đơn giản.
        /// </summary>
        public static async Task<string> SimpleGetAsync(string url)
        {
            using var client = CreateHttpClient();
            return await client.GetStringAsync(url);
        }

        /// <summary>
        /// Gửi yêu cầu POST đơn giản với nội dung dạng chuỗi.
        /// </summary>
        public static async Task<string> SimplePostAsync(string url, string content)
        {
            using var client = CreateHttpClient();
            var response = await client.PostAsync(url, new StringContent(content));
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gửi yêu cầu GET với custom header, timeout, trả về HttpResponseMessage.
        /// </summary>
        public static async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            return await client.GetAsync(url);
        }

        /// <summary>
        /// Gửi yêu cầu POST với custom header, content-type, timeout, trả về HttpResponseMessage.
        /// </summary>
        public static async Task<HttpResponseMessage> PostAsync(string url, string content, string contentType = "application/json", Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            var httpContent = new StringContent(content, Encoding.UTF8, contentType);
            return await client.PostAsync(url, httpContent);
        }

        /// <summary>
        /// Gửi yêu cầu PUT.
        /// </summary>
        public static async Task<HttpResponseMessage> PutAsync(string url, string content, string contentType = "application/json", Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            var httpContent = new StringContent(content, Encoding.UTF8, contentType);
            return await client.PutAsync(url, httpContent);
        }

        /// <summary>
        /// Gửi yêu cầu DELETE.
        /// </summary>
        public static async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            return await client.DeleteAsync(url);
        }

        /// <summary>
        /// Gửi yêu cầu POST form-urlencoded.
        /// </summary>
        public static async Task<HttpResponseMessage> PostFormAsync(string url, Dictionary<string, string> formData, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            var content = new FormUrlEncodedContent(formData);
            return await client.PostAsync(url, content);
        }

        /// <summary>
        /// Gửi yêu cầu POST multipart/form-data (upload file).
        /// </summary>
        public static async Task<HttpResponseMessage> PostMultipartAsync(string url, Dictionary<string, string> formData, string fileField, string filePath, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 60)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            using var content = new MultipartFormDataContent();
            foreach (var kv in formData)
                content.Add(new StringContent(kv.Value), kv.Key);
            var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, fileField, Path.GetFileName(filePath));
            return await client.PostAsync(url, content);
        }

        /// <summary>
        /// Gửi/nhận JSON (POST), trả về object.
        /// </summary>
        public static async Task<T> PostJsonAsync<T>(string url, object data, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseString);
        }

        /// <summary>
        /// Gửi GET nhận JSON, trả về object.
        /// </summary>
        public static async Task<T> GetJsonAsync<T>(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 30)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseString);
        }

        /// <summary>
        /// Download file từ url về local.
        /// </summary>
        public static async Task DownloadFileAsync(string url, string localPath, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 60)
        {
            using var client = CreateConfiguredHttpClient(null, authenticationHeaderValue, timeoutSeconds);
            var bytes = await client.GetByteArrayAsync(url);
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(localPath));
            await File.WriteAllBytesAsync(localPath, bytes);
        }

        /// <summary>
        /// Download file từ url về byte[].
        /// </summary>
        public static async Task<byte[]> DownloadFileAsync(string url, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 60)
        {
            using var client = CreateConfiguredHttpClient(null, authenticationHeaderValue, timeoutSeconds);
            return await client.GetByteArrayAsync(url);
        }

        /// <summary>
        /// Upload file đơn giản (PUT).
        /// </summary>
        public static async Task<HttpResponseMessage> UploadFileAsync(string url, string filePath, Dictionary<string, string> headers = null, AuthenticationHeaderValue authenticationHeaderValue = null, int timeoutSeconds = 60)
        {
            using var client = CreateConfiguredHttpClient(headers, authenticationHeaderValue, timeoutSeconds);
            using var content = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return await client.PutAsync(url, content);
        }

        /// <summary>
        /// Kiểm tra trạng thái mạng (ping url).
        /// </summary>
        public static async Task<bool> IsNetworkAvailableAsync(string url, int timeoutSeconds = 5)
        {
            try
            {
                using var client = CreateHttpClient(timeoutSeconds);
                var response = await client.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gửi lại (retry) đơn giản cho GET.
        /// </summary>
        public static async Task<string> GetWithRetryAsync(string url, int maxRetry = 3, int delayMs = 1000)
        {
            for (int i = 0; i < maxRetry; i++)
            {
                try
                {
                    using var client = CreateHttpClient();
                    return await client.GetStringAsync(url);
                }
                catch
                {
                    if (i == maxRetry - 1) throw;
                    await Task.Delay(delayMs);
                }
            }
            return null;
        }



        /// <summary>
        /// Tải nội dung HTML từ URL với dữ liệu POST.
        /// </summary>
        /// <param name="url">URL cần tải nội dung</param>
        /// <param name="data">Dữ liệu POST</param>
        /// <returns>Nội dung HTML dạng byte array hoặc null nếu có lỗi</returns>
        public static byte[] GetHtmlContent(string url, string data)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            using (var client = new CookieWebClient())
            {

                string text = "";
                try
                {
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.UTF8;
                    if (url.EndsWith("?") || url.EndsWith("&"))
                    {

                        var result = client.DownloadData(url + data);
                        return result;
                    }
                    else
                    {
                        client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(data);
                        client.UploadString(url, "POST", data);
                        return client.DownloadData(url);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Tải nội dung từ URL sử dụng HttpWebRequest.
        /// </summary>
        /// <param name="url">URL cần tải nội dung</param>
        /// <returns>Nội dung dạng string hoặc null nếu có lỗi</returns>
        public static String GetHttpWebRequest(string url)
        {
            string data = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }
    }
}