using DevExpress.ExpressApp;

namespace ENTOS.Module.Utils
{
    public static class OpenAiUtils
    {
        //private static Module.SystemObjects.CustomAudioTranscriptionOptions customAudioTranscriptionOptions = null;
        private static bool? useWhisperOffline = null;


        /// <summary>
        ///   AudioTranscriptionFormat = Verbose để lấy vị trí của từng từ
        /// </summary>
        public static string OpenAIAudioTranscription(IObjectSpace objectSpace, XafApplication application, string url, string logContent, ref OpenAI.Audio.AudioClient audioClient, ref string audioModel)
        {
            //Dùng thư viện whisper offline để chuyển audio sang text
            if (useWhisperOffline is null)
                useWhisperOffline = Convert.ToBoolean(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAIUseWhisperOffline", "False").Value);
            if (useWhisperOffline == true)
            {
                string pythonDir = null, whisperModelDir = null;
                string subtitleText = "";
                //Chạy trức tiếp Python trên server
                return Module.Utils.PythonUtils.PythonWhisperTranscriptionToSrt(objectSpace, application, url, ref pythonDir, ref whisperModelDir);
                //Dùng python trên C#, có thể chậm
                var segments = Module.Utils.PythonUtils.PythonWhisperTranscription(objectSpace, application, url, ref pythonDir, ref whisperModelDir);
                foreach (var segment in segments)
                {
                    var id = segment.GetItem("id").ToInt32(new System.Globalization.CultureInfo("en"));
                    var start = segment.GetItem("start").ToSingle(new System.Globalization.CultureInfo("en"));
                    var end = segment.GetItem("end").ToSingle(new System.Globalization.CultureInfo("en"));
                    var text = segment.GetItem("text").ToString();
                    var startTime = System.TimeSpan.FromSeconds(start);
                    var endTime = System.TimeSpan.FromSeconds(end);
                    subtitleText += (id + 1).ToString("D");
                    subtitleText += System.Environment.NewLine;
                    subtitleText += string.Format("{0} --> {1}", startTime.ToString(@"hh\:mm\:ss\,fff"), endTime.ToString(@"hh\:mm\:ss\,fff"));
                    subtitleText += System.Environment.NewLine;
                    subtitleText += text;
                    subtitleText += System.Environment.NewLine;
                    subtitleText += System.Environment.NewLine;
                }
                return subtitleText;
            }
            else
            {
                //var fileName = GetValidFileName(url);
                //if (fileName.Length > 200)
                //    fileName = fileName.Substring(fileName.Length - 200);
                //fileName = System.IO.Path.GetTempPath() + @"\" + fileName + ".srt"; 
                //if (System.IO.File.Exists(fileName))
                //{
                //    return System.IO.File.ReadAllText(fileName);
                //}
                //else
                //{
                //var translation = OpenAIAudioTranscriptionTimestampGranularities(objectSpace, application, url, logContent, ref audioClient, ref audioModel, OpenAI.Audio.AudioTimestampGranularities.Segment);
                //if (translation != null)
                //{
                //    string subtitleText = "";
                //    foreach (var segment in translation.Segments)
                //    {
                //        subtitleText += (segment.Id + 1).ToString("D");
                //        subtitleText += System.Environment.NewLine;
                //        subtitleText += string.Format("{0} --> {1}", segment.Start.ToString(@"hh\:mm\:ss\,fff"), segment.End.ToString(@"hh\:mm\:ss\,fff"));
                //        subtitleText += System.Environment.NewLine;
                //        subtitleText += segment.Text;
                //        subtitleText += System.Environment.NewLine;
                //        subtitleText += System.Environment.NewLine;
                //    }
                //    //System.IO.File.WriteAllText(fileName, subtitleText);
                //    return subtitleText;
                //}
                //}
                //Code này không hiển thị milisecond
                if (audioClient is null)
                {
                    var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                    audioModel = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAIAudioModel", "whisper-1").Value;
                    OpenAI.Models.ModelClient modelClient = new OpenAI.Models.ModelClient(openAIKey);
                    audioClient = new OpenAI.Audio.AudioClient(audioModel, openAIKey);
                }
                var customAudioTranscriptionOptions = new Module.SystemObjects.CustomAudioTranscriptionOptions(OpenAI.Audio.AudioTranscriptionFormat.Srt, null, OpenAI.Audio.AudioTimestampGranularities.Segment);
                var translation = audioClient.TranscribeAudio(url, customAudioTranscriptionOptions);
                if (translation != null)
                {
                    //Log chat GPT
                    var logObjectSpace = application.CreateObjectSpace(typeof(Module.SystemObjects.ChatGPTTokenUsage));
                    var chatGPTTokenUsage = logObjectSpace.CreateObject<Module.SystemObjects.ChatGPTTokenUsage>();
                    //chatGPTTokenUsage.InputTokens = chatCompletion.Usage.InputTokens;
                    //chatGPTTokenUsage.OutputTokens = chatCompletion.Usage.OutputTokens;
                    chatGPTTokenUsage.Duration = translation.Value?.Duration;
                    if (logContent.Length > 200)
                        logContent = logContent.Substring(0, 200);
                    chatGPTTokenUsage.Content = logContent;
                    chatGPTTokenUsage.AIModel = audioModel;
                    chatGPTTokenUsage.Session.CommitTransaction();

                    return translation.Value?.Text;
                }

            }
            return null;
        }


        public static object OpenAIAudioTranscriptionToWords(IObjectSpace objectSpace, XafApplication application, string url, string logContent, ref OpenAI.Audio.AudioClient audioClient, ref string audioModel)
        {
            //Dùng thư viện whisper offline để chuyển audio sang text
            if (useWhisperOffline is null)
                useWhisperOffline = Convert.ToBoolean(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAIUseWhisperOffline", "False").Value);
            if (useWhisperOffline == true)
            {
                string pythonDir = null, whisperModelDir = null;
                string subtitleText = "";
                //Chạy trức tiếp Python trên server
                return Module.Utils.PythonUtils.PythonWhisperTranscriptionToWords(objectSpace, application, url, ref pythonDir, ref whisperModelDir);
                //Dùng python trên C#, có thể chậm
                //Chưa viết code hỗ trợ
                //var segments = PythonWhisperTranscription(objectSpace, application, url, ref pythonDir, ref whisperModelDir);
                //foreach (var segment in segments)
                //{
                //    var id = segment.GetItem("id").ToInt32(new System.Globalization.CultureInfo("en"));
                //    var start = segment.GetItem("start").ToSingle(new System.Globalization.CultureInfo("en"));
                //    var end = segment.GetItem("end").ToSingle(new System.Globalization.CultureInfo("en"));
                //    var text = segment.GetItem("text").ToString();
                //    var startTime = System.TimeSpan.FromSeconds(start);
                //    var endTime = System.TimeSpan.FromSeconds(end);
                //    subtitleText += (id + 1).ToString("D");
                //    subtitleText += System.Environment.NewLine;
                //    subtitleText += string.Format("{0} --> {1}", startTime.ToString(@"hh\:mm\:ss\,fff"), endTime.ToString(@"hh\:mm\:ss\,fff"));
                //    subtitleText += System.Environment.NewLine;
                //    subtitleText += text;
                //    subtitleText += System.Environment.NewLine;
                //    subtitleText += System.Environment.NewLine;
                //}
                //return subtitleText;
            }
            else
            {
                return OpenAIAudioTranscriptionTimestampGranularities(objectSpace, application, url, logContent, ref audioClient, ref audioModel);
            }
            return null;
        }

        public static OpenAI.Audio.AudioTranscription OpenAIAudioTranscriptionTimestampGranularities(IObjectSpace objectSpace, XafApplication application, string url, string logContent, ref OpenAI.Audio.AudioClient audioClient, ref string audioModel, OpenAI.Audio.AudioTimestampGranularities audioTimestampGranularities = OpenAI.Audio.AudioTimestampGranularities.Word)
        {
            if (audioClient is null)
            {
                var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                audioModel = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAIAudioModel", "whisper-1").Value;
                OpenAI.Models.ModelClient modelClient = new OpenAI.Models.ModelClient(openAIKey);
                audioClient = new OpenAI.Audio.AudioClient(audioModel, openAIKey);
            }
            var translation = audioClient.TranscribeAudio(url, new OpenAI.Audio.AudioTranscriptionOptions() { ResponseFormat = OpenAI.Audio.AudioTranscriptionFormat.Verbose, Granularities = audioTimestampGranularities });
            if (translation != null)
            {
                //Log chat GPT
                var logObjectSpace = application.CreateObjectSpace(typeof(Module.SystemObjects.ChatGPTTokenUsage));
                var chatGPTTokenUsage = logObjectSpace.CreateObject<Module.SystemObjects.ChatGPTTokenUsage>();
                //chatGPTTokenUsage.InputTokens = chatCompletion.Usage.InputTokens;
                //chatGPTTokenUsage.OutputTokens = chatCompletion.Usage.OutputTokens;
                chatGPTTokenUsage.Duration = translation.Value?.Duration;
                if (logContent.Length > 200)
                    logContent = logContent.Substring(0, 200);
                chatGPTTokenUsage.Content = logContent;
                chatGPTTokenUsage.AIModel = audioModel;
                chatGPTTokenUsage.Session.CommitTransaction();

                return translation.Value;
            }
            return null;
        }

        public static bool CheckOpenAIAudioSupport(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var extension = System.IO.Path.GetExtension(url);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    if (extension == ".mp3" || extension == ".mp4" || extension == ".mpeg" || extension == ".mpga"
                        || extension == ".m4a" || extension == ".wav" || extension == ".webm")
                        return true;
                }
            }
            return false;
        }
        //Sử dụng thư viện OpenAI để dịch nội dung từ Content sang Subtitle        //Sử dụng thư viện OpenAI để dịch nội dung từ Content sang Subtitle
        public static string OpenAITranslate(IObjectSpace objectSpace, XafApplication application, string content, string logContent, ref OpenAI.Chat.ChatClient chatClient, string prefix = "Dịch tất cả đoạn văn bản html sau từ Tiếng Anh sang Tiếng Việt và không được bỏ <br/> giữ nguyên tất cả <br/>, ngắt dòng, HTML tags: ")
        {
            if (chatClient is null)
            {
                var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                var model = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAITranslateModel", "gpt-3.5-turbo", SecuritySystem.CurrentUserId).Value;
                chatClient = new(model, openAIKey);
            }
            OpenAI.Chat.ChatMessage[] chatMessages = new OpenAI.Chat.ChatMessage[]
            {
                OpenAI.Chat.ChatMessage.CreateSystemMessage(prefix),
                OpenAI.Chat.ChatMessage.CreateUserMessage(content)
            };
            OpenAI.Chat.ChatCompletion chatCompletion = chatClient.CompleteChat(chatMessages);
            if (chatCompletion?.Content?.Count > 0)
            {
                LogChatGPT(chatCompletion, application, logContent);
                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                //    string splitText = " <br/>&nbsp;\n";
                //    var result= chatCompletion.Content[0].Text.Split(new string[] {splitText, splitText.Trim() }, System.StringSplitOptions.None);
                //    var input = content.Split(new string[] { splitText, splitText.Trim() }, System.StringSplitOptions.None);
                //    if(result.Length != input.Length)
                //    {

                //    }

                //}

                return chatCompletion.Content[0].Text;
            }
            return null;
        }

        public static string OpenAITranslate(IObjectSpace objectSpace, XafApplication application, OpenAI.Chat.ChatMessage[] chatMessages, string logContent, ref OpenAI.Chat.ChatClient chatClient)
        {
            //Cấu trúc này chỉ chạy trên model 4o
            if (chatClient is null)
            {
                var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                var model = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAITranslateModel", "gpt-3.5-turbo", SecuritySystem.CurrentUserId).Value;
                chatClient = new(model, openAIKey);
            }
            OpenAI.Chat.ChatCompletion chatCompletion = chatClient.CompleteChat(chatMessages);
            if (chatCompletion?.Content?.Count > 0)
            {
                LogChatGPT(chatCompletion, application, logContent);
                return chatCompletion.Content[0].Text;
            }
            return null;
        }
        public static System.Collections.Generic.IReadOnlyList<OpenAI.Chat.ChatMessageContentPart> OpenAIChat(IObjectSpace objectSpace, XafApplication application, OpenAI.Chat.ChatMessageContentPart[] chatMessageContentPart, string logContent, ref OpenAI.Chat.ChatClient chatClient)
        {
            OpenAI.Chat.ChatMessage[] chatMessages = new OpenAI.Chat.ChatMessage[]
            {
                new OpenAI.Chat.UserChatMessage(chatMessageContentPart)
            };
            if (chatClient is null)
            {
                var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                var model = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAITranslateModel", "gpt-3.5-turbo").Value;
                chatClient = new(model, openAIKey);
            }
            OpenAI.Chat.ChatCompletion chatCompletion = chatClient.CompleteChat(chatMessages);
            if (chatCompletion?.Content?.Count > 0)
            {
                LogChatGPT(chatCompletion, application, logContent);
                return chatCompletion.Content;
            }
            return null;

        }

        public static void LogChatGPT(OpenAI.Chat.ChatCompletion chatCompletion, XafApplication application, string content)
        {
            if (chatCompletion?.Usage != null)
            {
                var objectSpace = application.CreateObjectSpace(typeof(Module.SystemObjects.ChatGPTTokenUsage));
                var chatGPTTokenUsage = objectSpace.CreateObject<Module.SystemObjects.ChatGPTTokenUsage>();
                chatGPTTokenUsage.InputTokens = chatCompletion.Usage.InputTokens;
                chatGPTTokenUsage.OutputTokens = chatCompletion.Usage.OutputTokens;
                if (!string.IsNullOrEmpty(content) && content.Length > 200)
                    content = content.Substring(0, 200);
                chatGPTTokenUsage.Content = content;
                chatGPTTokenUsage.AIModel = chatCompletion.Model;
                chatGPTTokenUsage.Session.CommitTransaction();

            }
        }

        //Sử dụng thư viện OpenAI để chuyển nội dung từ text to speech

        public static BinaryData OpenAITextToSpeech(IObjectSpace objectSpace, XafApplication application, string content, string logContent, string voice, ref OpenAI.Audio.AudioClient audioClient, ref string textToSpeechModel)
        {
            if (!string.IsNullOrEmpty(content))
            {
                if (audioClient is null)
                {
                    var openAIKey = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "OpenAITranslateKey", "sk-proj-***");
                    textToSpeechModel = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "OpenAITextToSpeechModel", "tts-1", SecuritySystem.CurrentUserId).Value;
                    //OpenAI.Models.ModelClient modelClient = new OpenAI.Models.ModelClient(openAIKey);
                    audioClient = new OpenAI.Audio.AudioClient(textToSpeechModel, openAIKey);
                }
                var voiceEnum = (OpenAI.Audio.GeneratedSpeechVoice)System.Enum.Parse(typeof(OpenAI.Audio.GeneratedSpeechVoice), voice);
                var result = audioClient.GenerateSpeechFromText(content, voiceEnum);
                if (result != null)
                {
                    //Log chat GPT
                    var logObjectSpace = application.CreateObjectSpace(typeof(Module.SystemObjects.ChatGPTTokenUsage));
                    var chatGPTTokenUsage = logObjectSpace.CreateObject<Module.SystemObjects.ChatGPTTokenUsage>();
                    //chatGPTTokenUsage.InputTokens = chatCompletion.Usage.InputTokens;
                    //chatGPTTokenUsage.OutputTokens = chatCompletion.Usage.OutputTokens;
                    //chatGPTTokenUsage.Duration = translation.Value?.Duration;
                    if (logContent.Length > 200)
                        logContent = logContent.Substring(0, 200);
                    chatGPTTokenUsage.Content = logContent;
                    chatGPTTokenUsage.AIModel = textToSpeechModel;
                    chatGPTTokenUsage.Session.CommitTransaction();
                    return result.Value;
                }
            }
            return null;
        }

    }
}
