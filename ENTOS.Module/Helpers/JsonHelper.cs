using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý JSON thuần túy, đặc biệt cho Google Text-to-Speech API
    /// Không có thao tác với file system, chỉ xử lý JSON và trả về đường link
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Trích xuất đường link từ JSON response của Google Text-to-Speech
        /// </summary>
        /// <param name="jsonInput">JSON string từ Google TTS API</param>
        /// <returns>Đường link (data URL cho audioContent, hoặc mediaLink từ storage) hoặc null nếu không tìm thấy</returns>
        public static string ExtractAudioLinkFromGoogleTts(string jsonInput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return null;

                // Parse JSON thành JsonNode
                var jsonNode = JsonNode.Parse(jsonInput);
                if (jsonNode == null)
                    return null;

                // Kiểm tra audioContent (Google TTS response)
                if (jsonNode["audioContent"]?.GetValue<string>() is string audioContent && !string.IsNullOrEmpty(audioContent))
                {
                    return ExtractAudioLinkFromAudioContent(audioContent);
                }

                // Kiểm tra mediaLink (Google Storage response)
                if (jsonNode["mediaLink"]?.GetValue<string>() is string mediaLink && !string.IsNullOrEmpty(mediaLink))
                {
                    return mediaLink;
                }

                // Tìm link trong JSON object
                return ExtractLinkFromJsonObject(jsonInput);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(jsonInput));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất đường link từ audio content (base64)
        /// </summary>
        /// <param name="audioContent">Audio content dạng base64</param>
        /// <returns>Đường link đến file audio (data URL)</returns>
        private static string ExtractAudioLinkFromAudioContent(string audioContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(audioContent))
                    return null;

                // Tạo data URL từ base64 audio content
                return $"data:audio/mp3;base64,{audioContent}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý audio content: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chuyển đổi base64 audio content thành byte array
        /// </summary>
        /// <param name="audioContent">Audio content dạng base64</param>
        /// <returns>Byte array của audio data</returns>
        public static byte[] ConvertBase64ToBytes(string audioContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(audioContent))
                    return null;

                return Convert.FromBase64String(audioContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi chuyển đổi base64 thành bytes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất base64 content từ JSON response của Google Text-to-Speech
        /// </summary>
        /// <param name="jsonInput">JSON string từ Google TTS API</param>
        /// <returns>Base64 string của audio content hoặc null nếu không tìm thấy</returns>
        public static string ExtractBase64AudioContent(string jsonInput)
        {
            return ExtractBase64Content(jsonInput, "audioContent");
        }

        /// <summary>
        /// Trích xuất base64 content từ JSON với key tùy chỉnh
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <param name="keyName">Tên key chứa base64 content</param>
        /// <returns>Base64 string hoặc null nếu không tìm thấy</returns>
        public static string ExtractBase64Content(string jsonInput, string keyName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return null;

                // Parse JSON thành JsonNode
                var jsonNode = JsonNode.Parse(jsonInput);
                if (jsonNode == null)
                    return null;

                // Kiểm tra key cụ thể
                if (jsonNode[keyName]?.GetValue<string>() is string base64Content && !string.IsNullOrEmpty(base64Content))
                {
                    return base64Content;
                }

                return null;
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(jsonInput));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất base64 content từ JSON bằng cách tìm kiếm các key phổ biến
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Base64 string đầu tiên tìm thấy hoặc null</returns>
        public static string ExtractBase64ContentAuto(string jsonInput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return null;

                // Parse JSON thành JsonNode
                var jsonNode = JsonNode.Parse(jsonInput);
                if (jsonNode == null)
                    return null;

                // Danh sách các key phổ biến chứa base64 content
                var base64Keys = new[]
                {
                    "audioContent", "content", "data", "file", "image", "video", "audio",
                    "base64", "base64Content", "base64Data", "base64File", "base64Image",
                    "base64Video", "base64Audio", "encodedContent", "encodedData",
                    "binaryContent", "binaryData", "fileContent", "fileData"
                };

                foreach (var key in base64Keys)
                {
                    if (jsonNode[key]?.GetValue<string>() is string base64Content && !string.IsNullOrEmpty(base64Content))
                    {
                        // Kiểm tra xem có phải base64 hợp lệ không
                        if (IsValidBase64(base64Content))
                        {
                            return base64Content;
                        }
                    }
                }

                // Tìm kiếm trong nested objects
                return FindBase64InJsonNode(jsonNode);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(jsonInput));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm base64 content trong JsonNode
        /// </summary>
        /// <param name="jsonNode">JsonNode để tìm kiếm</param>
        /// <returns>Base64 string đầu tiên tìm thấy hoặc null</returns>
        private static string FindBase64InJsonNode(JsonNode jsonNode)
        {
            if (jsonNode == null) return null;

            // Kiểm tra nếu là JsonObject
            if (jsonNode is JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    var value = property.Value;
                    if (value != null)
                    {
                        // Kiểm tra nếu là string và có thể là base64
                        if (value.GetValue<string>() is string stringValue && !string.IsNullOrEmpty(stringValue))
                        {
                            if (IsValidBase64(stringValue))
                            {
                                return stringValue;
                            }
                        }
                        else
                        {
                            // Đệ quy tìm trong nested objects
                            var nestedResult = FindBase64InJsonNode(value);
                            if (nestedResult != null)
                                return nestedResult;
                        }
                    }
                }
            }
            // Kiểm tra nếu là JsonArray
            else if (jsonNode is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                {
                    var result = FindBase64InJsonNode(item);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra xem string có phải là base64 hợp lệ không
        /// </summary>
        /// <param name="input">String cần kiểm tra</param>
        /// <returns>True nếu là base64 hợp lệ</returns>
        private static bool IsValidBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            try
            {
                // Kiểm tra độ dài tối thiểu
                if (input.Length < 4)
                    return false;

                // Kiểm tra chỉ chứa ký tự base64 hợp lệ
                var validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                if (!input.All(c => validChars.Contains(c)))
                    return false;

                // Thử decode để kiểm tra
                Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Trích xuất audio bytes từ JSON response của Google Text-to-Speech
        /// </summary>
        /// <param name="jsonInput">JSON string từ Google TTS API</param>
        /// <returns>Byte array của audio data hoặc null nếu không tìm thấy</returns>
        public static byte[] ExtractAudioBytesFromGoogleTts(string jsonInput)
        {
            try
            {
                var base64Content = ExtractBase64AudioContent(jsonInput);
                if (base64Content != null)
                {
                    return ConvertBase64ToBytes(base64Content);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất bytes từ JSON với key tùy chỉnh
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <param name="keyName">Tên key chứa base64 content</param>
        /// <returns>Byte array hoặc null nếu không tìm thấy</returns>
        public static byte[] ExtractBytesFromJson(string jsonInput, string keyName)
        {
            try
            {
                var base64Content = ExtractBase64Content(jsonInput, keyName);
                if (base64Content != null)
                {
                    return ConvertBase64ToBytes(base64Content);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất bytes từ JSON bằng cách tìm kiếm tự động
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Byte array đầu tiên tìm thấy hoặc null</returns>
        public static byte[] ExtractBytesFromJsonAuto(string jsonInput)
        {
            try
            {
                var base64Content = ExtractBase64ContentAuto(jsonInput);
                if (base64Content != null)
                {
                    return ConvertBase64ToBytes(base64Content);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất tất cả base64 content từ JSON
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Danh sách tất cả base64 content tìm thấy</returns>
        public static List<string> ExtractAllBase64Content(string jsonInput)
        {
            var base64List = new List<string>();
            
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return base64List;

                var jsonNode = JsonNode.Parse(jsonInput);
                if (jsonNode == null)
                    return base64List;

                CollectAllBase64FromJsonNode(jsonNode, base64List);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi extract base64 từ JSON: {ex.Message}", nameof(jsonInput));
            }

            return base64List.Distinct().ToList();
        }

        /// <summary>
        /// Thu thập tất cả base64 content từ JsonNode
        /// </summary>
        /// <param name="jsonNode">JsonNode</param>
        /// <param name="base64List">List để chứa base64 content</param>
        private static void CollectAllBase64FromJsonNode(JsonNode jsonNode, List<string> base64List)
        {
            if (jsonNode == null) return;

            // Kiểm tra nếu là JsonObject
            if (jsonNode is JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    var value = property.Value;
                    if (value != null)
                    {
                        // Kiểm tra nếu là string và có thể là base64
                        if (value.GetValue<string>() is string stringValue && !string.IsNullOrEmpty(stringValue))
                        {
                            if (IsValidBase64(stringValue))
                            {
                                base64List.Add(stringValue);
                            }
                        }
                        else
                        {
                            // Đệ quy tìm trong nested objects
                            CollectAllBase64FromJsonNode(value, base64List);
                        }
                    }
                }
            }
            // Kiểm tra nếu là JsonArray
            else if (jsonNode is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                {
                    CollectAllBase64FromJsonNode(item, base64List);
                }
            }
        }

        /// <summary>
        /// Trích xuất base64 content theo loại file (image, audio, video, document)
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <param name="fileType">Loại file ("image", "audio", "video", "document")</param>
        /// <returns>Base64 string đầu tiên tìm thấy hoặc null</returns>
        public static string ExtractBase64ByFileType(string jsonInput, string fileType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return null;

                var jsonNode = JsonNode.Parse(jsonInput);
                if (jsonNode == null)
                    return null;

                // Danh sách key theo loại file
                var keysByType = fileType.ToLower() switch
                {
                    "image" => new[] { "image", "imageContent", "base64Image", "imageData", "img", "photo", "picture" },
                    "audio" => new[] { "audio", "audioContent", "base64Audio", "audioData", "sound", "music", "voice" },
                    "video" => new[] { "video", "videoContent", "base64Video", "videoData", "movie", "clip" },
                    "document" => new[] { "document", "docContent", "base64Document", "documentData", "file", "pdf", "doc" },
                    _ => new[] { "content", "data", "file", "base64" }
                };

                foreach (var key in keysByType)
                {
                    if (jsonNode[key]?.GetValue<string>() is string base64Content && !string.IsNullOrEmpty(base64Content))
                    {
                        if (IsValidBase64(base64Content))
                        {
                            return base64Content;
                        }
                    }
                }

                return null;
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(jsonInput));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi xử lý JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo data URL từ base64 content với MIME type tự động xác định
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <param name="keyName">Tên key chứa base64 content</param>
        /// <returns>Data URL hoặc null nếu không tìm thấy</returns>
        public static string CreateDataUrlFromJson(string jsonInput, string keyName)
        {
            try
            {
                var base64Content = ExtractBase64Content(jsonInput, keyName);
                if (base64Content == null)
                    return null;

                // Sử dụng Base64Helper để xác định MIME type
                var mimeType = Base64Helper.GetMimeType(base64Content);
                return Base64Helper.ToDataUrl(base64Content, mimeType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi tạo data URL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo data URL từ base64 content với tìm kiếm tự động
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Data URL hoặc null nếu không tìm thấy</returns>
        public static string CreateDataUrlFromJsonAuto(string jsonInput)
        {
            try
            {
                var base64Content = ExtractBase64ContentAuto(jsonInput);
                if (base64Content == null)
                    return null;

                // Sử dụng Base64Helper để xác định MIME type
                var mimeType = Base64Helper.GetMimeType(base64Content);
                return Base64Helper.ToDataUrl(base64Content, mimeType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi tạo data URL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất link từ JSON object bằng cách tìm kiếm các key có thể chứa link
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Đường link đầu tiên tìm thấy hoặc null</returns>
        private static string ExtractLinkFromJsonObject(string jsonInput)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonInput);
                return FindLinkInJsonElement(document.RootElement);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tìm link trong JsonElement
        /// </summary>
        /// <param name="element">JsonElement để tìm kiếm</param>
        /// <returns>Link đầu tiên tìm thấy hoặc null</returns>
        private static string FindLinkInJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        // Kiểm tra các key có thể chứa link
                        if (IsLinkKey(property.Name))
                        {
                            var value = property.Value.GetString();
                            if (IsValidUrl(value))
                                return value;
                        }
                        
                        // Đệ quy tìm trong nested objects
                        var nestedLink = FindLinkInJsonElement(property.Value);
                        if (nestedLink != null)
                            return nestedLink;
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        var arrayLink = FindLinkInJsonElement(item);
                        if (arrayLink != null)
                            return arrayLink;
                    }
                    break;

                case JsonValueKind.String:
                    var stringValue = element.GetString();
                    if (IsValidUrl(stringValue))
                        return stringValue;
                    break;
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra xem key có thể chứa link không
        /// </summary>
        /// <param name="key">Key name</param>
        /// <returns>True nếu key có thể chứa link</returns>
        private static bool IsLinkKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            var linkKeywords = new[]
            {
                "url", "link", "href", "src", "media", "audio", "file", "download",
                "mediaLink", "selfLink", "downloadUrl", "streamUrl", "playUrl"
            };

            return linkKeywords.Any(k => key.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Kiểm tra xem string có phải là URL hợp lệ không
        /// </summary>
        /// <param name="url">String cần kiểm tra</param>
        /// <returns>True nếu là URL hợp lệ</returns>
        private static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Lấy JsonSerializerOptions với cấu hình phù hợp
        /// </summary>
        /// <returns>JsonSerializerOptions</returns>
        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        /// <summary>
        /// Parse JSON và trả về JsonNode
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>JsonNode object</returns>
        public static JsonNode ParseJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("JSON input không được null hoặc empty", nameof(json));

            try
            {
                return JsonNode.Parse(json);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(json));
            }
        }

        /// <summary>
        /// Parse JSON và trả về object
        /// </summary>
        /// <typeparam name="T">Kiểu object cần parse</typeparam>
        /// <param name="json">JSON string</param>
        /// <returns>Object đã parse</returns>
        public static T ParseJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("JSON input không được null hoặc empty", nameof(json));

            try
            {
                return JsonSerializer.Deserialize<T>(json, GetJsonOptions());
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Lỗi parse JSON: {ex.Message}", nameof(json));
            }
        }

        /// <summary>
        /// Convert object thành JSON string
        /// </summary>
        /// <param name="obj">Object cần convert</param>
        /// <returns>JSON string</returns>
        public static string ToJson(object obj)
        {
            if (obj == null)
                return "null";

            try
            {
                return JsonSerializer.Serialize(obj, GetJsonOptions());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi serialize object thành JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validate JSON string
        /// </summary>
        /// <param name="json">JSON string cần validate</param>
        /// <returns>True nếu JSON hợp lệ</returns>
        public static bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                using var document = JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Trích xuất tất cả các link từ JSON
        /// </summary>
        /// <param name="jsonInput">JSON string</param>
        /// <returns>Danh sách các link tìm thấy</returns>
        public static List<string> ExtractAllLinks(string jsonInput)
        {
            var links = new List<string>();
            
            try
            {
                if (string.IsNullOrWhiteSpace(jsonInput))
                    return links;

                using var document = JsonDocument.Parse(jsonInput);
                CollectLinksFromJsonElement(document.RootElement, links);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi extract links từ JSON: {ex.Message}", nameof(jsonInput));
            }

            return links.Distinct().ToList();
        }

        /// <summary>
        /// Thu thập tất cả links từ JsonElement
        /// </summary>
        /// <param name="element">JsonElement</param>
        /// <param name="links">List để chứa links</param>
        private static void CollectLinksFromJsonElement(JsonElement element, List<string> links)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        if (IsLinkKey(property.Name))
                        {
                            var value = property.Value.GetString();
                            if (IsValidUrl(value))
                                links.Add(value);
                        }
                        
                        CollectLinksFromJsonElement(property.Value, links);
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        CollectLinksFromJsonElement(item, links);
                    }
                    break;

                case JsonValueKind.String:
                    var stringValue = element.GetString();
                    if (IsValidUrl(stringValue))
                        links.Add(stringValue);
                    break;
            }
        }

        /// <summary>
        /// Lấy giá trị từ JSON bằng key path (ví dụ: "data.items[0].name")
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="keyPath">Đường dẫn đến key</param>
        /// <returns>Giá trị tìm thấy hoặc null</returns>
        public static string GetValueFromJson(string json, string keyPath)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                if (jsonNode == null) return null;

                var keys = keyPath.Split('.');
                var current = jsonNode;

                foreach (var key in keys)
                {
                    if (current == null) return null;

                    if (key.Contains('[') && key.Contains(']'))
                    {
                        // Xử lý array index
                        var arrayKey = key.Substring(0, key.IndexOf('['));
                        var indexStr = key.Substring(key.IndexOf('[') + 1, key.IndexOf(']') - key.IndexOf('[') - 1);
                        
                        if (int.TryParse(indexStr, out int index))
                        {
                            current = current[arrayKey]?[index];
                        }
                    }
                    else
                    {
                        current = current[key];
                    }
                }

                return current?.GetValue<string>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy giá trị chuỗi của một thuộc tính theo tên trường ở cấp ngoài cùng trong <see cref="System.Text.Json.JsonElement"/>.
        /// </summary>
        /// <param name="element">Đối tượng <see cref="System.Text.Json.JsonElement"/> cần đọc.</param>
        /// <param name="name">Tên thuộc tính cần tìm trong <paramref name="element"/>.</param>
        /// <returns>
        /// Giá trị chuỗi (<see cref="string"/>) của thuộc tính nếu tồn tại; 
        /// ngược lại trả về <c>null</c>.
        /// </returns>
        public static string GetProp(System.Text.Json.JsonElement element, string name)
            => element.TryGetProperty(name, out var prop) ? prop.GetString() : null;

        /// <summary>
        /// Kiểm tra xem JSON có chứa key không
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="key">Key cần kiểm tra</param>
        /// <returns>True nếu có key</returns>
        public static bool HasKey(string json, string key)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                return jsonNode?[key] != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy tất cả keys từ JSON object
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Danh sách các keys</returns>
        public static List<string> GetAllKeys(string json)
        {
            var keys = new List<string>();
            try
            {
                var jsonNode = JsonNode.Parse(json);
                if (jsonNode is JsonObject jsonObject)
                {
                    foreach (var property in jsonObject)
                    {
                        keys.Add(property.Key);
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
            return keys;
        }

        /// <summary>
        /// Tạo JSON object đơn giản từ key-value pairs
        /// </summary>
        /// <param name="keyValuePairs">Key-value pairs</param>
        /// <returns>JSON string</returns>
        public static string CreateJson(params (string key, object value)[] keyValuePairs)
        {
            var jsonObject = new JsonObject();
            
            foreach (var (key, value) in keyValuePairs)
            {
                if (value is string str)
                    jsonObject[key] = str;
                else if (value is int intVal)
                    jsonObject[key] = intVal;
                else if (value is double doubleVal)
                    jsonObject[key] = doubleVal;
                else if (value is bool boolVal)
                    jsonObject[key] = boolVal;
                else
                    jsonObject[key] = value?.ToString();
            }

            return jsonObject.ToJsonString();
        }

        #region JSON Transformation và Manipulation

        /// <summary>
        /// Merge hai JSON objects
        /// </summary>
        /// <param name="json1">JSON object đầu tiên</param>
        /// <param name="json2">JSON object thứ hai</param>
        /// <param name="overwrite">Có ghi đè key trùng không</param>
        /// <returns>JSON object đã merge</returns>
        public static string MergeJson(string json1, string json2, bool overwrite = true)
        {
            try
            {
                var node1 = JsonNode.Parse(json1);
                var node2 = JsonNode.Parse(json2);

                if (node1 is JsonObject obj1 && node2 is JsonObject obj2)
                {
                    foreach (var property in obj2)
                    {
                        if (overwrite || !obj1.ContainsKey(property.Key))
                        {
                            obj1[property.Key] = property.Value?.DeepClone();
                        }
                    }
                }

                return node1?.ToJsonString() ?? "{}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi merge JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Flatten JSON object thành key-value pairs
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="prefix">Prefix cho key</param>
        /// <returns>Dictionary chứa key-value pairs</returns>
        public static Dictionary<string, object> FlattenJson(string json, string prefix = "")
        {
            var result = new Dictionary<string, object>();
            
            try
            {
                var jsonNode = JsonNode.Parse(json);
                FlattenJsonNode(jsonNode, result, prefix);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi flatten JSON: {ex.Message}", nameof(json));
            }

            return result;
        }

        /// <summary>
        /// Flatten JsonNode thành Dictionary
        /// </summary>
        /// <param name="node">JsonNode</param>
        /// <param name="result">Dictionary kết quả</param>
        /// <param name="prefix">Prefix hiện tại</param>
        private static void FlattenJsonNode(JsonNode node, Dictionary<string, object> result, string prefix)
        {
            if (node == null) return;

            if (node is JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    var key = string.IsNullOrEmpty(prefix) ? property.Key : $"{prefix}.{property.Key}";
                    
                    if (property.Value is JsonObject || property.Value is JsonArray)
                    {
                        FlattenJsonNode(property.Value, result, key);
                    }
                    else
                    {
                        result[key] = property.Value?.GetValue<object>();
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    var key = $"{prefix}[{i}]";
                    
                    if (jsonArray[i] is JsonObject || jsonArray[i] is JsonArray)
                    {
                        FlattenJsonNode(jsonArray[i], result, key);
                    }
                    else
                    {
                        result[key] = jsonArray[i]?.GetValue<object>();
                    }
                }
            }
        }

        /// <summary>
        /// Unflatten Dictionary thành JSON object
        /// </summary>
        /// <param name="flattenedData">Dictionary đã flatten</param>
        /// <returns>JSON string</returns>
        public static string UnflattenJson(Dictionary<string, object> flattenedData)
        {
            try
            {
                var jsonObject = new JsonObject();
                
                foreach (var kvp in flattenedData)
                {
                    SetValueByPath(jsonObject, kvp.Key, kvp.Value);
                }

                return jsonObject.ToJsonString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi unflatten JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Set giá trị theo path trong JsonObject
        /// </summary>
        /// <param name="jsonObject">JsonObject</param>
        /// <param name="path">Path (ví dụ: "user.profile.name")</param>
        /// <param name="value">Giá trị cần set</param>
        private static void SetValueByPath(JsonObject jsonObject, string path, object value)
        {
            var parts = path.Split('.');
            var current = jsonObject;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];
                
                if (part.Contains('[') && part.Contains(']'))
                {
                    // Xử lý array
                    var arrayKey = part.Substring(0, part.IndexOf('['));
                    var indexStr = part.Substring(part.IndexOf('[') + 1, part.IndexOf(']') - part.IndexOf('[') - 1);
                    
                    if (int.TryParse(indexStr, out int index))
                    {
                        if (current[arrayKey] == null)
                            current[arrayKey] = new JsonArray();
                        
                        var array = current[arrayKey] as JsonArray;
                        while (array.Count <= index)
                            array.Add(null);
                        
                        if (i == parts.Length - 2)
                        {
                            array[index] = JsonValue.Create(value);
                        }
                        else
                        {
                            if (array[index] == null || array[index] is not JsonObject)
                                array[index] = new JsonObject();
                            current = array[index] as JsonObject;
                        }
                    }
                }
                else
                {
                    // Xử lý object
                    if (current[part] == null || current[part] is not JsonObject)
                        current[part] = new JsonObject();
                    current = current[part] as JsonObject;
                }
            }

            var lastPart = parts[parts.Length - 1];
            if (lastPart.Contains('[') && lastPart.Contains(']'))
            {
                var arrayKey = lastPart.Substring(0, lastPart.IndexOf('['));
                var indexStr = lastPart.Substring(lastPart.IndexOf('[') + 1, lastPart.IndexOf(']') - lastPart.IndexOf('[') - 1);
                
                if (int.TryParse(indexStr, out int index))
                {
                    if (current[arrayKey] == null)
                        current[arrayKey] = new JsonArray();
                    
                    var array = current[arrayKey] as JsonArray;
                    while (array.Count <= index)
                        array.Add(null);
                    
                    array[index] = JsonValue.Create(value);
                }
            }
            else
            {
                current[lastPart] = JsonValue.Create(value);
            }
        }

        /// <summary>
        /// Transform JSON bằng cách áp dụng function cho từng value
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="transformer">Function transform value</param>
        /// <returns>JSON string đã transform</returns>
        public static string TransformJson(string json, Func<string, object, object> transformer)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                TransformJsonNode(jsonNode, transformer);
                return jsonNode?.ToJsonString() ?? "{}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi transform JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Transform JsonNode
        /// </summary>
        /// <param name="node">JsonNode</param>
        /// <param name="transformer">Function transform</param>
        private static void TransformJsonNode(JsonNode node, Func<string, object, object> transformer)
        {
            if (node == null) return;

            if (node is JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    if (property.Value is JsonObject || property.Value is JsonArray)
                    {
                        TransformJsonNode(property.Value, transformer);
                    }
                    else
                    {
                        var originalValue = property.Value?.GetValue<object>();
                        var transformedValue = transformer(property.Key, originalValue);
                        jsonObject[property.Key] = JsonValue.Create(transformedValue);
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    if (jsonArray[i] is JsonObject || jsonArray[i] is JsonArray)
                    {
                        TransformJsonNode(jsonArray[i], transformer);
                    }
                    else
                    {
                        var originalValue = jsonArray[i]?.GetValue<object>();
                        var transformedValue = transformer(i.ToString(), originalValue);
                        jsonArray[i] = JsonValue.Create(transformedValue);
                    }
                }
            }
        }

        #endregion

        #region JSON Validation và Schema

        /// <summary>
        /// Validate JSON theo schema đơn giản
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="requiredKeys">Danh sách key bắt buộc</param>
        /// <param name="optionalKeys">Danh sách key tùy chọn</param>
        /// <returns>Validation result</returns>
        public static (bool IsValid, List<string> Errors) ValidateJsonSchema(string json, string[] requiredKeys, string[] optionalKeys = null)
        {
            var errors = new List<string>();
            
            try
            {
                var jsonNode = JsonNode.Parse(json);
                if (jsonNode is not JsonObject jsonObject)
                {
                    errors.Add("JSON phải là object");
                    return (false, errors);
                }

                // Kiểm tra required keys
                foreach (var key in requiredKeys)
                {
                    if (!jsonObject.ContainsKey(key))
                    {
                        errors.Add($"Thiếu key bắt buộc: {key}");
                    }
                }

                // Kiểm tra optional keys (nếu có)
                if (optionalKeys != null)
                {
                    var allValidKeys = requiredKeys.Concat(optionalKeys).ToArray();
                    foreach (var property in jsonObject)
                    {
                        if (!allValidKeys.Contains(property.Key))
                        {
                            errors.Add($"Key không được phép: {property.Key}");
                        }
                    }
                }

                return (errors.Count == 0, errors);
            }
            catch (Exception ex)
            {
                errors.Add($"Lỗi parse JSON: {ex.Message}");
                return (false, errors);
            }
        }

        /// <summary>
        /// Kiểm tra xem JSON có match với pattern không
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="pattern">Pattern JSON</param>
        /// <returns>True nếu match</returns>
        public static bool MatchJsonPattern(string json, string pattern)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                var patternNode = JsonNode.Parse(pattern);
                
                return MatchJsonNodes(jsonNode, patternNode);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// So sánh hai JsonNode
        /// </summary>
        /// <param name="actual">JsonNode thực tế</param>
        /// <param name="expected">JsonNode mong đợi</param>
        /// <returns>True nếu match</returns>
        private static bool MatchJsonNodes(JsonNode actual, JsonNode expected)
        {
            if (actual == null && expected == null) return true;
            if (actual == null || expected == null) return false;

            if (actual is JsonObject actualObj && expected is JsonObject expectedObj)
            {
                foreach (var property in expectedObj)
                {
                    if (!actualObj.ContainsKey(property.Key))
                        return false;
                    
                    if (!MatchJsonNodes(actualObj[property.Key], property.Value))
                        return false;
                }
                return true;
            }
            else if (actual is JsonArray actualArray && expected is JsonArray expectedArray)
            {
                if (actualArray.Count != expectedArray.Count)
                    return false;
                
                for (int i = 0; i < actualArray.Count; i++)
                {
                    if (!MatchJsonNodes(actualArray[i], expectedArray[i]))
                        return false;
                }
                return true;
            }
            else
            {
                return actual.ToJsonString() == expected.ToJsonString();
            }
        }

        #endregion

        #region JSON Query và Filter

        /// <summary>
        /// Query JSON bằng path expression đơn giản
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="query">Query expression (ví dụ: "users[0].name")</param>
        /// <returns>Kết quả query</returns>
        public static object QueryJson(string json, string query)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                return QueryJsonNode(jsonNode, query);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi query JSON: {ex.Message}", nameof(query));
            }
        }

        /// <summary>
        /// Query JsonNode theo path
        /// </summary>
        /// <param name="node">JsonNode</param>
        /// <param name="query">Query path</param>
        /// <returns>Kết quả</returns>
        private static object QueryJsonNode(JsonNode node, string query)
        {
            if (node == null || string.IsNullOrEmpty(query))
                return null;

            var parts = query.Split('.');
            var current = node;

            foreach (var part in parts)
            {
                if (current == null) return null;

                if (part.Contains('[') && part.Contains(']'))
                {
                    // Xử lý array
                    var arrayKey = part.Substring(0, part.IndexOf('['));
                    var indexStr = part.Substring(part.IndexOf('[') + 1, part.IndexOf(']') - part.IndexOf('[') - 1);
                    
                    if (int.TryParse(indexStr, out int index))
                    {
                        current = current[arrayKey]?[index];
                    }
                }
                else
                {
                    // Xử lý object
                    current = current[part];
                }
            }

            return current?.GetValue<object>();
        }

        /// <summary>
        /// Filter JSON array theo điều kiện
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="arrayPath">Path đến array</param>
        /// <param name="condition">Điều kiện filter</param>
        /// <returns>JSON array đã filter</returns>
        public static string FilterJsonArray(string json, string arrayPath, Func<JsonNode, bool> condition)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                var arrayNode = QueryJsonNode(jsonNode, arrayPath) as JsonArray;
                
                if (arrayNode == null)
                    return "[]";

                var filteredArray = new JsonArray();
                foreach (var item in arrayNode)
                {
                    if (condition(item))
                    {
                        filteredArray.Add(item.DeepClone());
                    }
                }

                return filteredArray.ToJsonString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi filter JSON array: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sort JSON array theo key
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="arrayPath">Path đến array</param>
        /// <param name="sortKey">Key để sort</param>
        /// <param name="ascending">Sort tăng dần hay giảm dần</param>
        /// <returns>JSON array đã sort</returns>
        public static string SortJsonArray(string json, string arrayPath, string sortKey, bool ascending = true)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                var arrayNode = QueryJsonNode(jsonNode, arrayPath) as JsonArray;
                
                if (arrayNode == null)
                    return "[]";

                var sortedItems = arrayNode
                    .Where(item => item is JsonObject)
                    .Cast<JsonObject>()
                    .OrderBy(item => item[sortKey]?.GetValue<object>())
                    .ToList();

                if (!ascending)
                    sortedItems.Reverse();

                var sortedArray = new JsonArray();
                foreach (var item in sortedItems)
                {
                    sortedArray.Add(item.DeepClone());
                }

                return sortedArray.ToJsonString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi sort JSON array: {ex.Message}", ex);
            }
        }

        #endregion

        #region JSON Statistics và Analysis

        /// <summary>
        /// Phân tích cấu trúc JSON
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Thông tin phân tích</returns>
        public static JsonAnalysisResult AnalyzeJson(string json)
        {
            var result = new JsonAnalysisResult();
            
            try
            {
                var jsonNode = JsonNode.Parse(json);
                AnalyzeJsonNode(jsonNode, result);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Lỗi phân tích JSON: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Phân tích JsonNode
        /// </summary>
        /// <param name="node">JsonNode</param>
        /// <param name="result">Kết quả phân tích</param>
        private static void AnalyzeJsonNode(JsonNode node, JsonAnalysisResult result)
        {
            if (node == null) return;

            result.TotalNodes++;

            if (node is JsonObject jsonObject)
            {
                result.ObjectCount++;
                result.MaxDepth = Math.Max(result.MaxDepth, result.CurrentDepth + 1);
                
                foreach (var property in jsonObject)
                {
                    result.Keys.Add(property.Key);
                    result.CurrentDepth++;
                    AnalyzeJsonNode(property.Value, result);
                    result.CurrentDepth--;
                }
            }
            else if (node is JsonArray jsonArray)
            {
                result.ArrayCount++;
                result.MaxDepth = Math.Max(result.MaxDepth, result.CurrentDepth + 1);
                result.MaxArrayLength = Math.Max(result.MaxArrayLength, jsonArray.Count);
                
                foreach (var item in jsonArray)
                {
                    result.CurrentDepth++;
                    AnalyzeJsonNode(item, result);
                    result.CurrentDepth--;
                }
            }
            else
            {
                result.ValueCount++;
                var value = node.GetValue<object>();
                if (value is string str)
                {
                    result.MaxStringLength = Math.Max(result.MaxStringLength, str.Length);
                    if (IsValidBase64(str))
                        result.Base64Count++;
                }
                else if (value is int || value is long)
                    result.NumberCount++;
                else if (value is bool)
                    result.BooleanCount++;
            }
        }

        /// <summary>
        /// Lấy thống kê về JSON
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Thống kê JSON</returns>
        public static JsonStatistics GetJsonStatistics(string json)
        {
            var analysis = AnalyzeJson(json);
            
            return new JsonStatistics
            {
                Size = json.Length,
                ObjectCount = analysis.ObjectCount,
                ArrayCount = analysis.ArrayCount,
                ValueCount = analysis.ValueCount,
                MaxDepth = analysis.MaxDepth,
                UniqueKeys = analysis.Keys.Count,
                MaxArrayLength = analysis.MaxArrayLength,
                MaxStringLength = analysis.MaxStringLength,
                Base64Count = analysis.Base64Count,
                NumberCount = analysis.NumberCount,
                BooleanCount = analysis.BooleanCount,
                IsValid = analysis.Errors.Count == 0
            };
        }

        #endregion

        #region JSON Comparison

        /// <summary>
        /// So sánh hai JSON objects
        /// </summary>
        /// <param name="json1">JSON thứ nhất</param>
        /// <param name="json2">JSON thứ hai</param>
        /// <returns>Kết quả so sánh</returns>
        public static JsonComparisonResult CompareJson(string json1, string json2)
        {
            var result = new JsonComparisonResult();
            
            try
            {
                var node1 = JsonNode.Parse(json1);
                var node2 = JsonNode.Parse(json2);
                
                CompareJsonNodes(node1, node2, "", result);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Lỗi so sánh JSON: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// So sánh hai JsonNode
        /// </summary>
        /// <param name="node1">Node thứ nhất</param>
        /// <param name="node2">Node thứ hai</param>
        /// <param name="path">Path hiện tại</param>
        /// <param name="result">Kết quả so sánh</param>
        private static void CompareJsonNodes(JsonNode node1, JsonNode node2, string path, JsonComparisonResult result)
        {
            if (node1 == null && node2 == null) return;
            if (node1 == null)
            {
                result.Added.Add(path);
                return;
            }
            if (node2 == null)
            {
                result.Removed.Add(path);
                return;
            }

            if (node1 is JsonObject obj1 && node2 is JsonObject obj2)
            {
                var keys1 = obj1.Select(p => p.Key).ToHashSet();
                var keys2 = obj2.Select(p => p.Key).ToHashSet();

                foreach (var key in keys1.Except(keys2))
                {
                    result.Removed.Add($"{path}.{key}");
                }

                foreach (var key in keys2.Except(keys1))
                {
                    result.Added.Add($"{path}.{key}");
                }

                foreach (var key in keys1.Intersect(keys2))
                {
                    var newPath = string.IsNullOrEmpty(path) ? key : $"{path}.{key}";
                    CompareJsonNodes(obj1[key], obj2[key], newPath, result);
                }
            }
            else if (node1 is JsonArray arr1 && node2 is JsonArray arr2)
            {
                var maxLength = Math.Max(arr1.Count, arr2.Count);
                for (int i = 0; i < maxLength; i++)
                {
                    var newPath = $"{path}[{i}]";
                    var item1 = i < arr1.Count ? arr1[i] : null;
                    var item2 = i < arr2.Count ? arr2[i] : null;
                    CompareJsonNodes(item1, item2, newPath, result);
                }
            }
            else
            {
                var value1 = node1.GetValue<object>();
                var value2 = node2.GetValue<object>();
                
                if (!Equals(value1, value2))
                {
                    result.Modified.Add(path);
                }
            }
        }

        #endregion

        #region JSON Utilities

        /// <summary>
        /// Minify JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>JSON đã minify</returns>
        public static string MinifyJson(string json)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                return jsonNode?.ToJsonString(new JsonSerializerOptions { WriteIndented = false }) ?? "{}";
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi minify JSON: {ex.Message}", nameof(json));
            }
        }

        /// <summary>
        /// Pretty print JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="indentSize">Kích thước indent</param>
        /// <returns>JSON đã format</returns>
        public static string PrettyPrintJson(string json, int indentSize = 2)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                return jsonNode?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? "{}";
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi pretty print JSON: {ex.Message}", nameof(json));
            }
        }

        /// <summary>
        /// Escape JSON string
        /// </summary>
        /// <param name="input">String cần escape</param>
        /// <returns>String đã escape</returns>
        public static string EscapeJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return JsonSerializer.Serialize(input);
        }

        /// <summary>
        /// Unescape JSON string
        /// </summary>
        /// <param name="input">String cần unescape</param>
        /// <returns>String đã unescape</returns>
        public static string UnescapeJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            try
            {
                return JsonSerializer.Deserialize<string>(input);
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// Tạo JSON template từ object
        /// </summary>
        /// <param name="template">Object template</param>
        /// <returns>JSON template string</returns>
        public static string CreateJsonTemplate(object template)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(template, GetJsonOptions());
                var jsonNode = JsonNode.Parse(jsonString);
                
                // Replace tất cả values với placeholders
                ReplaceValuesWithPlaceholders(jsonNode);
                
                return jsonNode?.ToJsonString(GetJsonOptions()) ?? "{}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi tạo JSON template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Replace values với placeholders
        /// </summary>
        /// <param name="node">JsonNode</param>
        private static void ReplaceValuesWithPlaceholders(JsonNode node)
        {
            if (node == null) return;

            if (node is JsonObject jsonObject)
            {
                foreach (var property in jsonObject)
                {
                    if (property.Value is JsonObject || property.Value is JsonArray)
                    {
                        ReplaceValuesWithPlaceholders(property.Value);
                    }
                    else
                    {
                        var value = property.Value?.GetValue<object>();
                        if (value is string str)
                        {
                            jsonObject[property.Key] = $"<{property.Key}>";
                        }
                        else if (value is int)
                        {
                            jsonObject[property.Key] = 0;
                        }
                        else if (value is double)
                        {
                            jsonObject[property.Key] = 0.0;
                        }
                        else if (value is bool)
                        {
                            jsonObject[property.Key] = false;
                        }
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    if (jsonArray[i] is JsonObject || jsonArray[i] is JsonArray)
                    {
                        ReplaceValuesWithPlaceholders(jsonArray[i]);
                    }
                    else
                    {
                        jsonArray[i] = "<item>";
                    }
                }
            }
        }

        #endregion
    }

    #region Supporting Classes

    /// <summary>
    /// Kết quả phân tích JSON
    /// </summary>
    public class JsonAnalysisResult
    {
        public int TotalNodes { get; set; }
        public int ObjectCount { get; set; }
        public int ArrayCount { get; set; }
        public int ValueCount { get; set; }
        public int MaxDepth { get; set; }
        public int CurrentDepth { get; set; }
        public int MaxArrayLength { get; set; }
        public int MaxStringLength { get; set; }
        public int Base64Count { get; set; }
        public int NumberCount { get; set; }
        public int BooleanCount { get; set; }
        public HashSet<string> Keys { get; set; } = new HashSet<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Thống kê JSON
    /// </summary>
    public class JsonStatistics
    {
        public int Size { get; set; }
        public int ObjectCount { get; set; }
        public int ArrayCount { get; set; }
        public int ValueCount { get; set; }
        public int MaxDepth { get; set; }
        public int UniqueKeys { get; set; }
        public int MaxArrayLength { get; set; }
        public int MaxStringLength { get; set; }
        public int Base64Count { get; set; }
        public int NumberCount { get; set; }
        public int BooleanCount { get; set; }
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// Kết quả so sánh JSON
    /// </summary>
    public class JsonComparisonResult
    {
        public List<string> Added { get; set; } = new List<string>();
        public List<string> Removed { get; set; } = new List<string>();
        public List<string> Modified { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();

        public bool HasChanges => Added.Count > 0 || Removed.Count > 0 || Modified.Count > 0;
        public int TotalChanges => Added.Count + Removed.Count + Modified.Count;
    }

    #endregion
} 