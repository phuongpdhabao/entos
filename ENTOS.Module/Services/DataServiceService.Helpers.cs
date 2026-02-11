namespace ENTOS.Module.Services
{
    public partial class DataServiceService
    {
        #region CheckResultType Helpers

        private static bool CheckResultType_IsJson(string result)
        {
            return result.StartsWith("{") && result.EndsWith("}");
        }

        private static bool CheckResultType_IsXml(string result)
        {
            return result.StartsWith("<") && result.EndsWith(">");
        }

        private static bool CheckResultType_IsSrt(string result)
        {
            return result.StartsWith("1") && result.Contains(" --> ");
        }

        #endregion

        #region GetResult Helpers

        private static bool GetResult_IsLocalFile(string address)
        {
            return !string.IsNullOrEmpty(address) && System.IO.File.Exists(address);
        }

        private static bool GetResult_IsHttpAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && address.StartsWith("http");
        }

        private static bool GetResult_IsReplicateApi(string address)
        {
            return address.StartsWith("https://replicate.com/") || address.StartsWith("https://api.replicate.com/");
        }

        private static bool GetResult_IsOpenAiApi(string address)
        {
            return address.StartsWith("https://api.openai.com/");
        }

        private static bool GetResult_IsFptAiApi(string address)
        {
            return address.StartsWith("https://api.fpt.ai/");
        }

        private static bool GetResult_IsPiperOffline(string apiKey)
        {
            return !string.IsNullOrEmpty(apiKey) && apiKey.Contains("Piper", System.StringComparison.OrdinalIgnoreCase);
        }

        private static bool GetResult_IsPyannote(string name)
        {
            return name.Contains("pyannote", System.StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region TranslateUsingGoogleAsync Helpers

        private static string TranslateUsingGoogleAsync_BuildUrl(string sourceLang, string targetLang, string inputText)
        {
            return string.Format(
                "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                sourceLang,
                targetLang,
                System.Web.HttpUtility.UrlEncode(inputText)
            );
        }

        #endregion

        #region GetImageResult Helpers

        private static string GetImageResult_GetContentType(System.Drawing.Imaging.ImageFormat imageFormat)
        {
            return "image/" + imageFormat.ToString().ToLower();
        }

        #endregion

        #region InsertAccents Helpers

        private static string InsertAccents_BuildJsonPayload(string text)
        {
            return string.Format("{{\"text\":\"{0}\"}}", text);
        }

        #endregion

        #region ParameterToString Helpers

        private static string ParameterToString_RemoveQuotes(string fileName)
        {
            if (fileName.StartsWith('"'))
            {
                fileName = fileName.Substring(1);
            }
            if (fileName.EndsWith('"'))
            {
                fileName = fileName.Substring(0, fileName.Length - 1);
            }
            return fileName;
        }

        private static string ParameterToString_AddQuotes(string wavFile)
        {
            return '"' + wavFile + '"';
        }

        #endregion

        #region File Extension Helpers

        private static bool IsWavExtension(string fileName)
        {
            return fileName.EndsWith(".wav", System.StringComparison.OrdinalIgnoreCase);
        }

        private static string GetWavFileName(string fileName)
        {
            return fileName + ".wav";
        }

        private static string BuildFfmpegConvertArguments(string inputFile, string outputFile)
        {
            return string.Format("-i \"{0}\" -acodec pcm_s16le -ar 16000 \"{1}\"", inputFile, outputFile);
        }

        #endregion
    }
}
