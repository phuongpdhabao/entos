using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class AudioViewController: BaseViewController<Module.BusinessObjects.Audio>
    {      
        
        public AudioViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Audio);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
     
        private TermLocationService termLocationService;
        protected TermLocationService _termLocationService => termLocationService ??= new TermLocationService(this);        
         
        private VideoService videoService;
        protected VideoService _videoService => videoService ??= new VideoService(this);        
      
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.AudioService audioService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             audioService = new Module.Services.AudioService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3369            Oid: 0ffd07ef-4f80-4a65-a259-630b28623e1a
		private void ElementTranslateSync_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ElementTranslateSync), "Đồng bộ Dịch ngữ");              
      
            #region ElementTranslateSyncImportCode
            var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null || video.LanguageTranslate is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn Video có Dịch ngữ trước khi thực hiện tính năng này", InformationType.Error);
                return;
            }

            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                if (e.SelectedChoiceActionItem.Id.Equals("Read"))
                {
                    Module.BusinessObjects.ElementTranslate elementTranslate = audio.ElementTranslateList.FirstOrDefault(x => x.Language == video.LanguageTranslate);
                    if (elementTranslate != null)
                    {
                        audio.Subtitle = elementTranslate.Content;
                        audio.Voice = elementTranslate.Voice;
                        audio.VoiceSpeed = elementTranslate.VoiceSpeed;
                        audio.AudioDuration = elementTranslate.AudioDuration;
                        audio.AudioLink = elementTranslate.AudioLink;
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"Không tìm thấy Dịch ngữ cho Thành phần {audio.Start} cho ngôn ngữ {video.LanguageTranslate.Name}", InformationType.Error);
                        continue;
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Save"))
                {
                    // save
                    if (!View.ObjectSpace.ModifiedObjects.Contains(audio))
                        continue;

                    Module.BusinessObjects.ElementTranslate elementTranslate = audio.ElementTranslateList.FirstOrDefault(x => x.Language == video.LanguageTranslate);
                    if (elementTranslate == null)
                    {
                        elementTranslate = new ElementTranslate(audio.Session)
                        {
                            Audio = audio,
                            Content = audio.Subtitle,
                            Language = video.LanguageTranslate,
                            VoiceSpeed = audio.VoiceSpeed,
                            AudioDuration = audio.AudioDuration,
                            AudioLink = audio.AudioLink
                        };
                    }
                    else
                    {
                        // Cập nhật thông tin Dịch ngữ
                        elementTranslate.Content = audio.Subtitle;
                        elementTranslate.Voice = audio.Voice;
                        elementTranslate.VoiceSpeed = audio.VoiceSpeed;
                        elementTranslate.AudioDuration = audio.AudioDuration;
                        elementTranslate.AudioLink = audio.AudioLink;
                    }
                }
                //2023-08-10: Đồng bộ Dịch ngữ từ Thành phần sang Phiên âm
                audio.Spelling = audio.GetDefaultSpelling();
            }

            #endregion ElementTranslateSyncImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1570            Oid: 82a84e4a-26d8-4179-a518-19211cd521c5
		private void FindCaseType_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(FindCaseType), "Xác định Kiểu chữ");              
      
            #region FindCaseTypeImportCode
            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                if (!string.IsNullOrEmpty(audio.Content))
                {
                    audio.CaseType = audio.GetDefaultCaseType();
                }
            }


            #endregion FindCaseTypeImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1526            Oid: 550e56d2-600f-4359-a17e-67f16931be5a
		private void PreviousNextElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(PreviousNextElement), "Trước sau");              
      
            #region PreviousNextElementImportCode
            //foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            //{
            //    audio.PreviousElement = audio.GetDefaultPreviousElement();
            //    audio.NextElement = audio.GetDefaultNextElement();
            //}
            //return;
            var audioDictionary = new System.Collections.Generic.Dictionary<System.Guid, System.Collections.Generic.List<Module.BusinessObjects.Audio>>();
            bool fastMode = true;//Tăng tốc độ, tự động thêm đối ứng trước sau và xóa những thành phần đã kiểm tra
            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                if (audio.Video is null || audio.Start is null)
                    continue;
                //Bỏ qua những thành phần đã nạp để tối ưu tốc độ
                if (audio.NextElement != null && audio.PreviousElement != null)
                    continue;
                //Tạo cache cho danh sách Audio
                System.Guid keyNode = audio.BookMark != null ? audio.BookMark.Oid : audio.TranslateObject != null ? audio.TranslateObject.Oid : System.Guid.Empty;
                System.Collections.Generic.List<Module.BusinessObjects.Audio> audioList = null;
                if (audioDictionary.ContainsKey(keyNode))
                {
                    audioList = audioDictionary[keyNode];
                }
                else
                {
                    audioList = audio.Video.GetAudioListWithSort(audio.BookMark, true, audio.TranslateObject);
                    audioDictionary.Add(keyNode, audioList);
                }
                //Tìm vị trí của thành phần hiện tại
                int index = audioList.FindIndex(m => m.Oid == audio.Oid);
                if (index >= 0)
                {
                    if (index > 0)
                    {
                        audio.PreviousElement = audioList[index - 1];
                        if (fastMode)
                            audio.PreviousElement.NextElement = audio;
                    }
                    if (index < audioList.Count - 1)
                    {
                        audio.NextElement = audioList[index + 1];
                        if (fastMode)
                            audio.NextElement.PreviousElement = audio;
                    }
                    //Xóa luôn dòng hiện tại để tăng tốc độ
                    if (fastMode)
                        audioList.RemoveAt(index);
                }
            }


            #endregion PreviousNextElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0943            Oid: b12715a2-4c7c-46b2-a8d6-2f539e20fd3e
		private void UpperLowerElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(UpperLowerElement), "Chỉnh viết hoa");              
      
            #region UpperLowerElementImportCode
            //Chuyển các chức năng UpperCase và LowerCaseElement về đây
            //Cờ False thì xử lý cột Nội dung, cờ dựng thì xử lý cột Dịch
            //2023-08-10: Bỏ xử lý theo cờ thay bằng vị trị Con trỏ(ô active) ở cột nào thì xử lý cột đó,
            //nếu không rơi vào 2 cột Tên/ Dịch thì không xử lý, trường hợp Web không xác định con trỏ thì xử lý Tên
            bool contentColumn = true;
            if (View is ListView && ((ListView)View).Editor != null)
            {
                var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                {
                    if ((string)focusedColumnMemberName == "Content")
                    {
                        contentColumn = true;
                    }
                    else if ((string)focusedColumnMemberName == "Subtitle")
                    {
                        contentColumn = false;
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Nội dung hoặc cột dịch trước khi thực hiện tính năng này", InformationType.Error);
                        return;
                    }
                }
            }
            char[] splitChar = new[] { ',', '\r', '\n' };
            string upperCaseEnglishAcceptWords = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "UpperCaseEnglishAcceptWords", "and, or, of, via, in, with, to, for");
            upperCaseEnglishAcceptWords = upperCaseEnglishAcceptWords.Replace(" ", "");
            var upperCaseEnglishAcceptWordsArray = upperCaseEnglishAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
            string upperCaseVietnameseAcceptWords = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "UpperCaseVietnameseAcceptWords", "và, hoặc, của, qua, trong, với, tới, cho");
            upperCaseVietnameseAcceptWords = upperCaseVietnameseAcceptWords.Replace(" ", "");
            var upperCaseVietnameseAcceptWordsArray = upperCaseVietnameseAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                //if (contentColumn is null)
                //    contentColumn = !audio.Flag;
                if (e.SelectedChoiceActionItem.Id.Equals("UpperFirstLetter"))
                {
                    //Hoa chữ đầu > phải viết hoa chữ đầu cho tất cả các từ trong Thành phần
                    //2023-08-21: Hoa chữ đầu (trừ danh sách sách: and, or, of, for, via .... và, hoặc, của, cho, qua....
                    string content = contentColumn ? audio.Content : audio.Subtitle;
                    if (string.IsNullOrEmpty(content))
                        continue;
                    //2023-08-21: Những từ viết tắt (hoa toàn bộ) cần chuyển theo định dạng hoa chữ đầu, chữ sau không viết hoa
                    content = content.ToLower();
                    string result = "";
                    for (int i = 0; i < content.Length; i++)
                    {
                        string word = content[i].ToString();
                        for (int j = i + 1; j < content.Length; j++)
                        {
                            if (!char.IsLetterOrDigit(content[j]))
                                break;
                            else
                            {
                                word += content[j].ToString();
                            }
                        }
                        if ((contentColumn && upperCaseEnglishAcceptWordsArray.Contains(word)) || (!contentColumn && upperCaseVietnameseAcceptWordsArray.Contains(word)))
                        {
                            result += content[i];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                result += char.ToUpper(content[i]);
                            }
                            else if (content[i - 1] == ' ' || content[i - 1] == '\n')
                            {
                                result += char.ToUpper(content[i]);
                            }
                            else
                                result += content[i];
                        }

                    }
                    if (!content.Equals(result))
                    {
                        if (contentColumn)
                            audio.Content = result;
                        else
                            audio.Subtitle = result;

                    }
                    //if (contentColumn)
                    //{
                    //    if (!string.IsNullOrEmpty(audio.Content))
                    //    {
                    //        if(audio.Content.Length == 1)
                    //            audio.Content = audio.Content.ToUpper();
                    //        else
                    //            audio.Content = char.ToUpper(audio.Content[0]) + audio.Content.Substring(1);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(audio.Subtitle))
                    //    {
                    //        if (audio.Subtitle.Length == 1)
                    //            audio.Subtitle = audio.Subtitle.ToUpper();
                    //        else
                    //            audio.Subtitle = char.ToUpper(audio.Subtitle[0]) + audio.Subtitle.Substring(1);
                    //    }                            
                    //}
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("UpperAll"))
                {
                    if (contentColumn)
                    {
                        if (!string.IsNullOrEmpty(audio.Content))
                            audio.Content = audio.Content.ToUpper();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(audio.Subtitle))
                            audio.Subtitle = audio.Subtitle.ToUpper();
                    }

                }
                else if (e.SelectedChoiceActionItem.Id.Equals("LowerKeepAbbreviation") || e.SelectedChoiceActionItem.Id.Equals("LowerAll"))
                {
                    //Bỏ viết hoa toàn bộ Thành phần trừ từ đầu tiên, viết tắt giữ nguyên hay không tùy theo Option
                    string content = contentColumn ? audio.Content : audio.Subtitle;
                    if (string.IsNullOrEmpty(content)) continue;
                    var words = content.Split(' ');
                    string result = words[0];
                    if (!string.IsNullOrEmpty(words[0]) && e.SelectedChoiceActionItem.Id.Equals("LowerAll"))
                    {
                        //Giữ viết tắt từ đầu tiên nếu có
                        if (char.IsUpper(words[0][0]))
                        {
                            result = words[0][0].ToString();
                            if (words[0].Length > 1)
                            {
                                result += words[0].Substring(1).ToLower();
                            }
                        }
                    }
                    for (int i = 1; i < words.Length; i++)
                    {
                        result += " ";
                        //Trường hợp giữ viết tắt
                        if (string.IsNullOrEmpty(words[i]))
                            continue;
                        if (e.SelectedChoiceActionItem.Id.Equals("LowerKeepAbbreviation") && char.IsUpper(words[i][0]) && words[i].Length > 1 && char.IsUpper(words[i][1]))
                            result += words[i];
                        else
                            result += words[i].ToLower();
                    }
                    if (!content.Equals(result))
                    {
                        if (contentColumn)
                            audio.Content = result;
                        else
                            audio.Subtitle = result;

                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("UpperElementBegin"))
                {
                    //Hoa đầu thành phần
                    //2023-08-21: Hoa chữ đầu (trừ danh sách sách: and, or, of, for, via .... và, hoặc, của, cho, qua....
                    string content = contentColumn ? audio.Content : audio.Subtitle;
                    if (string.IsNullOrEmpty(content))
                        continue;

                    string result = "";
                    int firstCharIndex = 0;
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (char.IsSymbol(content[i]))
                            continue;
                        if (char.IsLetter(content[i]))
                        {
                            firstCharIndex = i;
                            break;
                        }
                        else if (char.IsNumber(content[i]))
                        {
                            break;
                        }
                        else if (char.IsWhiteSpace(content[0]))
                        {
                            break;
                        }
                    }
                    firstCharIndex += 1;
                    result = content.Substring(0, firstCharIndex).ToUpper() + content.Substring(firstCharIndex);
                    if (!content.Equals(result))
                    {
                        if (contentColumn)
                            audio.Content = result;
                        else
                            audio.Subtitle = result;
                    }
                    if (!content.Equals(result))
                    {
                        if (contentColumn)
                            audio.Content = result;
                        else
                            audio.Subtitle = result;
                    }
                    //if (contentColumn)
                    //{
                    //    if (!string.IsNullOrEmpty(audio.Content))
                    //    {
                    //        if(audio.Content.Length == 1)
                    //            audio.Content = audio.Content.ToUpper();
                    //        else
                    //            audio.Content = char.ToUpper(audio.Content[0]) + audio.Content.Substring(1);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(audio.Subtitle))
                    //    {
                    //        if (audio.Subtitle.Length == 1)
                    //            audio.Subtitle = audio.Subtitle.ToUpper();
                    //        else
                    //            audio.Subtitle = char.ToUpper(audio.Subtitle[0]) + audio.Subtitle.Substring(1);
                    //    }                            
                    //}
                }
            }


            #endregion UpperLowerElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1408            Oid: 4e3c9a01-a430-4eef-9ab5-d38bc974a526
		private void ElementVoiceSpeed_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ElementVoiceSpeed), "Nạp tốc độ");              
      
            #region ElementVoiceSpeedImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Average"))
            {
                decimal totalDuration = 0, totalAudioDuration = 0;

                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    decimal audioDuration = audio.AudioDuration ?? 0m;
                    decimal duration = audio.Duration ?? 0m;

                    if (audioDuration > 0 && duration > 0)
                    {
                        totalAudioDuration += audioDuration;
                        totalDuration += duration;
                    }
                }

                if (totalDuration > 0)
                {
                    decimal voiceSpeed = totalAudioDuration / totalDuration;
                    if (voiceSpeed > 0)
                    {
                        foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                        {
                            decimal duration = audio.Duration ?? 0m;
                            if (duration > 0)
                            {
                                audio.VoiceSpeed = voiceSpeed;
                            }
                        }
                    }
                }
            }

            #endregion ElementVoiceSpeedImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 4548            Oid: 115440f6-2131-41e2-9ec7-d90dcd8f048e
		private void ImportElementTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ImportElementTerm), "Nạp thuật ngữ");              
      
            #region ImportElementTermImportCode


            #endregion ImportElementTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1499            Oid: 1d79fddc-0551-4da2-b333-818de31f0f4c
		private void SpellingAudio_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SpellingAudio), "Chính tả");              
      
            #region SpellingAudioImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Translate"))
            {
                var dataService = audioService.GetDataService(this);
                var dataServiceService = new Module.Services.DataServiceService();
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    if (audio.Content is null) continue;

                    // Gọi dịch vụ và nhận phản hồi
                    string response = System.Threading.Tasks.Task.Run(() => dataServiceService.InsertAccents(dataService, audio.Content)).Result;

                    // Kiểm tra phản hồi
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        // Xử lý phản hồi rỗng hoặc không hợp lệ
                        System.Console.WriteLine("Phản hồi từ dịch vụ là rỗng hoặc không hợp lệ.");
                        continue;
                    }

                    // Phân tích phản hồi JSON
                    try
                    {
                        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                        var accentedWords = jsonResponse.accented_words.ToObject<System.Collections.Generic.List<string>>();

                        // Kết hợp các từ thành một chuỗi
                        string contents = string.Join(" ", accentedWords);
                        audio.Subtitle = contents;

                        var contentWords = audio.Content.Split(' ').ToList();
                        var subtitleWords = audio.Subtitle.Split(' ').ToList();

                        // Giữ lại các từ khác nhau
                        var differentWords = subtitleWords.Where(word => !contentWords.Contains(word)).ToList();

                        // Kết hợp lại thành chuỗi mới
                        audio.Note = string.Join(",", differentWords);
                    }
                    catch (Newtonsoft.Json.JsonReaderException ex)
                    {
                        // Xử lý lỗi phân tích JSON
                        System.Console.WriteLine($"Lỗi phân tích JSON: {ex.Message}");
                        System.Console.WriteLine($"Nội dung phản hồi: {response}");
                    }

                }
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("SpellCorrect"))
            {
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    string textToCorrect = audio.Content;
                    string apiKey = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "APISerpKey", "", SecuritySystem.CurrentUserId).Value;

                    var languageCode = e.SelectedChoiceActionItem.Id.Equals("SpellCorrectTranslate")
                        ? audio.Video.LanguageTranslate.Code
                        : audio.Video.LanguageOrigin.Code;

                    // Gọi hàm async bằng Task.Run() và tránh deadlock
                    string spellingFix = System.Threading.Tasks.Task.Run(async () => await audioService.GetSpellCorrectionAsync(textToCorrect, languageCode, apiKey)).GetAwaiter().GetResult();

                    audio.Spelling = spellingFix;

                    if (spellingFix != ".")
                    {
                        var contentWords = audio.Content.Split(' ').ToList();
                        var spellingWords = audio.Spelling.Split(' ').ToList();

                        var differentWords = contentWords.Except(spellingWords).ToList();

                        // Ghi lại những từ khác nhau vào Note
                        audio.Note = string.Join(", ", differentWords);
                    }
                }
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("RepeatChar"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                var languageOrigin = video.LanguageOrigin;
                var languageTranslate = video.LanguageTranslate;

                string column = "Content";
                if (View is ListView && ((ListView)View).Editor != null)
                {
                    var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                    if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                    {
                        if ((string)focusedColumnMemberName == "Content" || (string)focusedColumnMemberName == "Subtitle" || (string)focusedColumnMemberName == "Spelling")
                        {
                            column = (string)focusedColumnMemberName;
                        }
                        else
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Nội dung hoặc cột dịch hoặc phiên âm trước khi thực hiện tính năng này", InformationType.Error);
                            return;
                        }
                    }
                }

                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {

                    string tempContent = audio.Content;
                    if (column == "Content")
                    {
                        tempContent = audio.Content;
                    }
                    else if (column == "Subtitle")
                    {
                        tempContent = audio.Subtitle;
                    }
                    else if (column == "Spelling")
                    {
                        tempContent = audio.Spelling;
                    }

                    var audioFlag = false;
                    var audioNote = "";
                    var words = tempContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    if (string.IsNullOrEmpty(tempContent))
                        continue;
                    var allowedRepeatChar = languageOrigin.RepeatCharacter + " " + languageTranslate.RepeatCharacter;
                    var pairs = allowedRepeatChar.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        var wordLower = word.ToLower();
                        if (wordLower.Contains("ưu") || wordLower.Contains("ứu") || wordLower.Contains("ừu") || wordLower.Contains("ửu") || wordLower.Contains("ữu") || wordLower.Contains("ựu"))
                        {
                            continue;
                        }
                        var noSignWord = Module.Helpers.TextHelper.RemoveUnicode(word);

                        // Tìm tất cả phần bị lặp (2 hoặc 3 ký tự liên tiếp)
                        var matches = System.Text.RegularExpressions.Regex.Matches(noSignWord, @"(.)\1{1,2}");

                        bool hasTripleLetterRepeat = false;
                        bool allRepeatsAreNumbersOrDots = true;

                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            string repeatedPart = match.Value;

                            if (System.Text.RegularExpressions.Regex.IsMatch(repeatedPart, @"^[0-9.]+$")) // Phần lặp chỉ chứa số hoặc dấu `.`
                            {
                                continue;
                            }

                            if (repeatedPart.Length >= 3) // Nếu lặp từ 3 ký tự trở lên
                            {
                                hasTripleLetterRepeat = true;
                                allRepeatsAreNumbersOrDots = false;
                                break;
                            }

                            allRepeatsAreNumbersOrDots = false;
                        }

                        // Nếu tất cả phần lặp đều là số hoặc `.`, bỏ qua từ này
                        if (allRepeatsAreNumbersOrDots && matches.Count > 0)
                        {
                            continue;
                        }

                        // Nếu có phần lặp từ 3 ký tự trở lên, đánh cờ lỗi
                        if (hasTripleLetterRepeat)
                        {
                            audioFlag = true;
                            audioNote += word + " ";
                            continue;
                        }

                        // Kiểm tra lặp 2 ký tự liên tiếp (chỉ chữ)
                        if (System.Text.RegularExpressions.Regex.IsMatch(noSignWord, @"([a-zA-Z])\1"))
                        {
                            bool isAllowed = false;

                            foreach (var pair in pairs)
                            {
                                if (noSignWord.Contains(pair))
                                {
                                    isAllowed = true;
                                    break;
                                }
                            }

                            if (!isAllowed)
                            {
                                audioFlag = true;
                                audioNote += word + " ";
                            }
                        }
                    }


                    audio.Flag = audioFlag;
                    audio.Note = audioNote;
                }
            }

            else if (e.SelectedChoiceActionItem.Id.Equals("SpellCheck"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                string column = "Content";
                if (View is ListView && ((ListView)View).Editor != null)
                {
                    var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                    if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                    {
                        if ((string)focusedColumnMemberName == "Content" || (string)focusedColumnMemberName == "Subtitle" || (string)focusedColumnMemberName == "Spelling")
                        {
                            column = (string)focusedColumnMemberName;
                        }
                        else
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Nội dung hoặc cột dịch hoặc phiên âm trước khi thực hiện tính năng này", InformationType.Error);
                            return;
                        }
                    }
                }
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> dictionaryTranslate = null;

                if (video.LanguageOrigin != null)
                    dictionaryTranslate = videoService.GetDictionarySpelling(video, true);

                var dictionaryOrigin = videoService.GetDictionarySpelling(video, false);

                var dictionaryCheck = dictionaryOrigin;
                var spellLanguage = video.LanguageOrigin;


                // check ngữ gốc rồi đến ngữ dịch
                for (int step = 1; step <= 2; step++)
                {
                    if (step == 1)
                    {
                        if (video.LanguageOrigin == null)
                            continue;
                        dictionaryCheck = dictionaryOrigin;
                        spellLanguage = video.LanguageOrigin;
                    }
                    if (step == 2)
                    {
                        if (video.LanguageTranslate == video.LanguageOrigin)
                            continue;
                        if (video.LanguageTranslate == null)
                            continue;

                        dictionaryCheck = dictionaryTranslate;
                        spellLanguage = video.LanguageTranslate;
                    }

                    foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                    {
                        bool audioFlag = false;
                        var audioNote = "";

                        string tempContent = audio.Content;
                        if (column == "Content")
                        {
                            tempContent = audio.Content;
                        }
                        else if (column == "Subtitle")
                        {
                            tempContent = audio.Subtitle;
                        }
                        else if (column == "Spelling")
                        {
                            tempContent = audio.Spelling;
                        }

                        if (string.IsNullOrEmpty(tempContent))
                            continue;

                        if (spellLanguage != null && spellLanguage.Code != "vi")
                        {
                            var words = tempContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                            foreach (var word in words)
                            {
                                bool existsInDictionary = TermService.CheckTermInDictionary(word, dictionaryCheck, spellLanguage, 1);
                                if (!existsInDictionary)
                                {
                                    audioFlag = true;
                                    audio.Note += word + ", ";
                                }
                            }
                        }
                        else
                        {
                            var extractedSegments = new System.Collections.Generic.List<string>();


                            // Danh sách các cặp dấu ngoặc hợp lệ
                            var bracketPairs = new (string open, string close)[]
                            {

                           ("\\(", "\\)"),
                           ("\\[", "\\]"),
                           ("\\{", "\\}"),
                           ("<", ">"),
                           ("\"", "\""),
                           ("“", "”")
                            };

                            // Tách nội dung trong dấu ngoặc trước
                            foreach (var (open, close) in bracketPairs)
                            {
                                string pattern = $"{open}([^\\{open}\\{close}]+){close}"; // Nội dung hợp lệ trong ngoặc
                                var matches = System.Text.RegularExpressions.Regex.Matches(tempContent, pattern);

                                foreach (System.Text.RegularExpressions.Match match in matches)
                                {
                                    extractedSegments.Add(match.Groups[1].Value.Trim()); // Lưu nội dung bên trong và loại khoảng trắng
                                    tempContent = tempContent.Replace(match.Value, " "); // Thay bằng khoảng trắng để giữ đúng vị trí
                                }
                            }

                            // Tách phần còn lại theo dấu câu, nhưng không tách giữa các số
                            var segments = System.Text.RegularExpressions.Regex.Split(tempContent, @"(?<!\d)(?<!\s)(?<!\p{L})-?(?!\s)(?!\d)|[.,?!;:/]+")
                              .Where(s => !string.IsNullOrWhiteSpace(s) && s.Any(char.IsLetter))
                              .ToList();


                            // Gộp các phần đã tách
                            extractedSegments.AddRange(segments);

                            // Xử lý danh sách cụm từ
                            var phrases = new System.Collections.Generic.List<string>();
                            foreach (var segment in extractedSegments)
                            {
                                var words = segment.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                                for (int length = words.Count; length > 0; length--)  // Bao gồm cả từ đơn lẻ
                                {
                                    for (int start = 0; start <= words.Count - length; start++)
                                    {
                                        string phrase = string.Join(" ", words.Skip(start).Take(length));
                                        phrases.Add(phrase);
                                    }
                                }
                            }



                            phrases = phrases.Distinct().OrderByDescending(p => p.Split(' ').Length).ToList();
                            var checkedWords = new System.Collections.Generic.HashSet<string>();
                            var checkedPhrases = new System.Collections.Generic.HashSet<string>();
                            var currencySymbols = new System.Collections.Generic.HashSet<char>("₫¢$€£¥₮৲৳௹฿៛₠₡₢₣₤₥₦₧₨₩₪₭₯₰₱₲₳₴₵￥﷼¤ƒ");

                            foreach (var phrase in phrases)
                            {
                                if (checkedPhrases.Contains(phrase))
                                    continue;

                                int wordCount = phrase.Trim().Split(' ').Length;

                                bool existsInDictionary = TermService.CheckTermInDictionary(
                                    phrase, dictionaryCheck, spellLanguage, wordCount);

                                if (existsInDictionary)
                                {
                                    foreach (var word in phrase.Split(' '))
                                    {
                                        checkedWords.Add(word);
                                    }
                                    checkedPhrases.Add(phrase);

                                    for (int i = 0; i < phrases.Count; i++)
                                    {
                                        var candidatePhrase = phrases[i];
                                        if (candidatePhrase.Split(' ').All(w => checkedWords.Contains(w)))
                                        {
                                            checkedPhrases.Add(candidatePhrase);
                                        }
                                    }
                                }
                            }

                            foreach (var segment in extractedSegments)
                            {
                                var words = segment.Trim()
                                                   .Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries)
                                                   .Where(w => !checkedWords.Contains(w));
                                foreach (var word in words)
                                {
                                    if (word == "=" || word == "+" || word == "<" || word == ">" || word == "-" || word == "•")
                                        continue;
                                    if (word.Any(char.IsDigit))
                                    {
                                        // Kiểm tra nếu số hợp lệ (có thể chứa ký tự đặc biệt hoặc đơn vị tiền tệ, nhưng không bắt buộc)
                                        bool isValidNumber =
                                            word.Any(c => char.IsDigit(c)) &&
                                            word.All(c => char.IsDigit(c) || ".,-/%°^".Contains(c) || currencySymbols.Contains(c)) ||
                                            word.All(char.IsUpper) ||
                                            (char.IsUpper(word[0]) && word.Skip(1).Any(char.IsLower));

                                        if (isValidNumber)
                                            continue;
                                    }

                                    bool existsInDictionary = TermService.CheckTermInDictionary(
                                        word, dictionaryCheck, spellLanguage, 1);
                                    if (!existsInDictionary)
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(word, @"^[A-Z]+$"))
                                        {
                                            continue;
                                        }
                                        if (word.EndsWith(")") || word.EndsWith(",") || word.EndsWith("/"))
                                        {
                                            continue;
                                        }
                                        audioFlag = true;
                                        audioNote = audioNote + word + ", ";
                                    }
                                }
                            }
                        }

                        if (audioFlag)
                        {
                            audio.Flag = true;
                            audio.Note = audioNote;
                        }
                    }
                }
            }



            #endregion SpellingAudioImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 4549            Oid: 1ebdf903-23f0-4479-bbc9-023b3d923fce
		private void ConvertTo_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ConvertTo), "Chuyển đổi");              
      
            #region ConvertToImportCode


            #endregion ConvertToImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0906            Oid: 53229e25-3198-40b0-b2a7-768ee6409e43
		private void ElementFlag_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ElementFlag), "Cờ thành phần");              
      
            #region ElementFlagImportCode
            char charTag = '<';
            string choiceCaption = e.SelectedChoiceActionItem.Caption;
            decimal totalSelectObject = View.SelectedObjects.Count;
            decimal countNumber = 0;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            if (e.SelectedChoiceActionItem.Id.Equals("Clear"))
            {
                //Xóa cờ
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    audio.Flag = false;
                    //Xóa Note nếu có dữ liệu
                    if (!string.IsNullOrEmpty(audio.Note))
                        audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                }
                return;
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Previous") || e.SelectedChoiceActionItem.Id.Equals("Next"))
            {
                //2023-08-21: Chức năng chuyển cờ trước hoặc sau
                var audioList = View.SelectedObjects.Cast<Module.BusinessObjects.Audio>().OrderBy(m => m.Start).ToList();
                foreach (var audio in audioList)
                {
                    if (e.SelectedChoiceActionItem.Id.Equals("Previous"))
                    {
                        var previousAudio = audio.Video.GetAudioListWithSort(false).FirstOrDefault(m => m.Start < audio.Start);
                        if (previousAudio != null)
                        {
                            previousAudio.Flag = true;
                            previousAudio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(previousAudio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                            //previousAudio.AddTextWithTagNode(charTag, choiceCaption);
                        }
                    }
                    else
                    {
                        var nextAudio = audio.Video.GetAudioListWithSort(true).FirstOrDefault(m => m.Start > audio.Start);
                        if (nextAudio != null)
                        {
                            nextAudio.Flag = true;
                            nextAudio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(nextAudio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                            //nextAudio.AddTextWithTagNode(charTag, choiceCaption);
                        }
                    }
                }
                return;
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("CompareSubtitleAndSpelling") || e.SelectedChoiceActionItem.Id.Equals("AudioOverlap")
                || e.SelectedChoiceActionItem.Id.Equals("Twin") || e.SelectedChoiceActionItem.Id.Equals("Contain") ||
                e.SelectedChoiceActionItem.Id.Equals("EndPart"))
            {
                var selectedAudioList = View.SelectedObjects.Cast<Module.BusinessObjects.Audio>().ToList();
                if (e.SelectedChoiceActionItem.Id.Equals("Twin") || e.SelectedChoiceActionItem.Id.Equals("Contain") ||
                e.SelectedChoiceActionItem.Id.Equals("EndPart"))
                {
                    if (selectedAudioList.FirstOrDefault(x => x.BookMark != null && string.IsNullOrEmpty(x.BookMark.Note)) != null)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Cảnh báo", "Ghi chú của liên kết bị trống, chức năng thực hiện có thể bị sai", InformationType.Warning);
                    }
                }

                //bool markContainOrEndPart = e.SelectedChoiceActionItem.Id.Equals("Twin") ? false : System.Convert.ToBoolean(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "ElementFlagMarkContainOrEndPart", "False", SecuritySystem.CurrentUserId).Value);
                int markIndex = 1;
                if (e.SelectedChoiceActionItem.Id.Equals("Contain") || e.SelectedChoiceActionItem.Id.Equals("EndPart"))
                {
                    //2024-08-06: Đánh số bao hàm và song sinh cần tiếp nối số đang có trong dữ liệu
                    foreach (Module.BusinessObjects.Audio audio in selectedAudioList)
                    {
                        if (audio.Video != null)
                        {
                            var numbers = audio.Video.AudioList.Where(m => !string.IsNullOrEmpty(m.Note) && m.Note.Contains(charTag + e.SelectedChoiceActionItem.Caption)).Select(text =>
                            {
                                var match = System.Text.RegularExpressions.Regex.Match(text.Note, @"\d+");
                                return match.Success ? int.Parse(match.Value) : 0;
                            });
                            if (numbers.Count() > 0)
                                markIndex = numbers.Max() + 1;
                            break;
                        }
                    }
                }
                var bookMarkAudioList = new System.Collections.Generic.Dictionary<System.Guid?, System.Collections.Generic.List<Module.BusinessObjects.Audio>>();
                //Các chức năng không liên quan đến cột Nội dung hay Dịch

                foreach (Module.BusinessObjects.Audio audio in selectedAudioList)
                {
                    if (audio.Delete || e.SelectedChoiceActionItem.Id.Equals("Clear"))
                        continue;
                    bool audioFlag = false;
                    //Xóa Note nếu có dữ liệu
                    if (!string.IsNullOrEmpty(audio.Note))
                        audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                    string noteCaption = e.SelectedChoiceActionItem.Caption;
                    if (e.SelectedChoiceActionItem.Id.Equals("CompareSubtitleAndSpelling"))
                    {
                        //var audioCaseType = CaseType.General;
                        if (!string.IsNullOrEmpty(audio.Subtitle))
                        {
                            audioFlag = !audio.Subtitle.Equals(audio.Spelling, System.StringComparison.OrdinalIgnoreCase);
                        }
                        else if (!string.IsNullOrEmpty(audio.Spelling))
                            audioFlag = true;
                        //if (audio.CaseType != audioCaseType)
                        //    audio.CaseType = audioCaseType;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("AudioOverlap"))
                    {
                        //Đè âm
                        if (audio.AudioDuration > 0)
                        {
                            System.Guid? bookmarkKey = audio.BookMark != null ? audio.BookMark.Oid : (audio.TranslateObject != null ? audio.TranslateObject.Oid : null);
                            System.Collections.Generic.List<Module.BusinessObjects.Audio> audioList = null;
                            if (bookMarkAudioList.ContainsKey(bookmarkKey))
                            {
                                audioList = bookMarkAudioList[bookmarkKey];
                            }
                            else
                            {
                                audioList = audio.Video.GetAudioListWithSort().Where(m => !m.Delete && m.BookMark == audio.BookMark && m.TranslateObject == audio.TranslateObject).ToList();
                                bookMarkAudioList.Add(bookmarkKey, audioList);
                            }
                            var nextElement = audioList.Where(m => m.Start > audio.Start).FirstOrDefault();
                            if (nextElement != null)
                            {
                                //Cho phép sai số 0.1s
                                var audioEnd = audio.GetRealEnd();
                                if (audioEnd != null && nextElement.Start < audioEnd.Value.Add(System.TimeSpan.FromMilliseconds(10)))
                                {
                                    audioFlag = true;
                                    //audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, tag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                        }

                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Twin") || e.SelectedChoiceActionItem.Id.Equals("Contain") || e.SelectedChoiceActionItem.Id.Equals("EndPart"))
                    {
                        //2024-08-17: Cờ thành phần > Song sinh/Twin : Tìm thấy thành phần giống sẽ dựng cờ cả 2 > Từ đó lọc ra những thành phần không cờ để phân tích text flow đúng hay sai từ đó sửa file Word để nạp lại
                        if (!string.IsNullOrEmpty(audio.Content) && audio.Video != null && audio.BookMark != null)
                        {
                            //054
                            //Cờ Bao hàm và Song sinh sẽ xử lý kiểm tra các thành phần:
                            //-Thuộc liên kết cùng cặp với liên kết của thành phần được chọn
                            //-Cờ thành phần đang ở trạng thái False
                            //Cờ Bao hàm: kiểm tra các thành phần thỏa mãn ở trên đồng thời được chứa trong thành phần đã chọn khi đó sẽ dựng cờ cả 2 thành phần
                            System.Guid? bookmarkKey = audio.BookMark != null ? audio.BookMark.Oid : null;
                            System.Collections.Generic.List<Module.BusinessObjects.Audio> audioList = null;
                            if (bookMarkAudioList.ContainsKey(bookmarkKey))
                            {
                                audioList = bookMarkAudioList[bookmarkKey];
                            }
                            else
                            {
                                audioList = audio.Video.GetAudioListWithSort().Where(m => !m.Delete && m.BookMark != null && m.BookMark != audio.BookMark && m.BookMark.Note == audio.BookMark.Note && !m.Flag && !string.IsNullOrEmpty(m.Content)).ToList();
                                bookMarkAudioList.Add(bookmarkKey, audioList);
                            }


                            var resultAudios = audioList.Where(x => !x.Flag && x.Oid != audio.Oid && (e.SelectedChoiceActionItem.Id.Equals("Twin") ? audio.Content == x.Content : (e.SelectedChoiceActionItem.Id.Equals("EndPart") ? (x.Content.Length > audio.Content.Length && x.Content.EndsWith(audio.Content)) : audio.Content.Contains(x.Content))));
                            //var twinAudios = audio.Video.AudioList.Where(x => !x.Flag && x.Oid != audio.Oid && !string.IsNullOrEmpty(x.Content) && (e.SelectedChoiceActionItem.Id.Equals("Twin") ? audio.Content == x.Content : audio.Content.Contains(x.Content)));
                            //if(audio.BookMark != null)
                            //    twinAudios = twinAudios.Where(x => x.BookMark != audio.BookMark && x.BookMark.Note == audio.BookMark.Note);
                            if (resultAudios.Count() > 0)
                            {
                                audioFlag = true;
                                if (resultAudios.Count() > 1)
                                {
                                    if (e.SelectedChoiceActionItem.Id.Equals("EndPart"))
                                    {
                                        //Thuật toán xác định phù hợp nhất
                                        resultAudios = resultAudios.OrderBy(x => x.Content.Length);
                                    }
                                    else if (e.SelectedChoiceActionItem.Id.Equals("Contain"))
                                    {
                                        //Thuật toán xác định phù hợp nhất
                                        resultAudios = resultAudios.OrderByDescending(x => x.Content.Length);
                                    }
                                }
                                foreach (var resultAudio in resultAudios)
                                {
                                    if (e.SelectedChoiceActionItem.Id.Equals("Contain"))
                                    {
                                        //062
                                        //Chỉnh lại chức năng bao hàm: ghi chú xác định 3 loại
                                        //< Bao hàm đầu>, < Bao hàm cuối>, < Bao hàm giữa>
                                        if (audio.Content.StartsWith(resultAudio.Content))
                                        {
                                            noteCaption = "Bao hàm đầu";
                                        }
                                        else if (audio.Content.EndsWith(resultAudio.Content))
                                        {
                                            noteCaption = "Bao hàm cuối";
                                        }
                                        else
                                        {
                                            noteCaption = "Bao hàm giữa";
                                        }
                                    }
                                    resultAudio.Flag = true;
                                    //if(markContainOrEndPart)
                                    //{
                                    noteCaption += " " + markIndex;
                                    markIndex++;
                                    //}
                                    resultAudio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(resultAudio.Note, charTag, noteCaption);
                                    //2024-08-20: chỉ xác định 1 thành phần song sinh hoặc bao hàm
                                    break;
                                }
                            }
                        }
                    }

                    if (audioFlag == true)
                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, noteCaption);
                    if (audio.Flag != audioFlag)
                        audio.Flag = audioFlag;
                    countNumber++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / totalSelectObject).ToString("p0"), " ", stopWatch.Elapsed);
                }
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
                return;
            }

            else if (e.SelectedChoiceActionItem.Id.Equals("HaveFootnote"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;

                if (video != null)
                {
                    System.Collections.Generic.List<Module.BusinessObjects.Audio> audioList = new System.Collections.Generic.List<Module.BusinessObjects.Audio>(); // Khởi tạo danh sách
                    foreach (Module.BusinessObjects.Audio audio in video.AudioList)
                    {
                        if (audio.UpperElement != null)
                        {
                            audioList.Add(audio.UpperElement);
                        }
                    }
                    foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                    {
                        if (audioList.Contains(audio))
                        {
                            audio.Flag = true;
                        }
                    }
                }
                return;

            }

            //Toàn bộ chức năng cờ sẽ xử lý cột Nội dung hay Dịch phụ thuộc vị trí con trỏ nằm ở cột nào
            string column = "Content";
            if (View is ListView && ((ListView)View).Editor != null)
            {
                //Xác định cột tại vị trí con trỏ
                var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                {
                    if ((string)focusedColumnMemberName == "Content")
                    {
                        column = "Content";
                    }
                    else if ((string)focusedColumnMemberName == "Subtitle")
                    {
                        column = "Subtitle";
                    }
                    else if ((string)focusedColumnMemberName == "Spelling")
                    {
                        column = "Spelling";
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Nội dung hoặc cột dịch trước khi thực hiện tính năng này", InformationType.Error);
                        return;
                    }
                }
            }
            if (Module.Helpers.ParameterHelper.GetBooleanOrDefault(ObjectSpace, "RemoveAllFlagWhenExecute", false))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video is null)
                    return;
                foreach (Module.BusinessObjects.Audio audio in video.AudioList)
                    audio.Flag = false;
            }
            char[] splitChar = new[] { ',', '\r', '\n' };

            if (e.SelectedChoiceActionItem.Id.Equals("SpellCheck"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video is null)
                    return;
                string languageCode = null;
                if (column == "Content")
                {
                    if (video.LanguageOrigin != null)
                        languageCode = video.LanguageOrigin.Code;
                }
                else
                {
                    if (video.LanguageTranslate != null)
                        languageCode = video.LanguageTranslate.Code;
                }
                //languageCode = "vi";
                if (!string.IsNullOrEmpty(languageCode))
                {
                    string aff = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryAff" + languageCode + ".aff";
                    string dic = "\\\\rd\\CodeGen\\packages\\Dictionaries\\Dictionary" + languageCode + ".dic";
                    using (NHunspell.Hunspell hunspell = new NHunspell.Hunspell(aff, dic))
                    {

                        //Thêm từ custom
                        var dictionariesText = Module.Helpers.ParameterHelper.GetValueOrDefault(ObjectSpace, "ViDictionaries", "THPT,THCS");
                        if (!string.IsNullOrEmpty(dictionariesText))
                        {
                            var dictionaries = dictionariesText.Split(',');
                            foreach (var dictionary in dictionaries)
                                hunspell.Add(dictionary.Trim());
                        }
                        foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                        {
                            var audioFlag = false;
                            //Xóa Note nếu có dữ liệu
                            if (!string.IsNullOrEmpty(audio.Note))
                                audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                            if (audio.Delete || e.SelectedChoiceActionItem.Id.Equals("Clear"))
                                continue;
                            string audioContent = audio.Content;
                            if (column == "Subtitle")
                                audioContent = audio.Subtitle;
                            else if (column == "Spelling")
                                audioContent = audio.Spelling;
                            if (!string.IsNullOrEmpty(audioContent))
                            {
                                var rows = audioContent.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
                                var resutl = new System.Collections.Generic.List<string>();
                                foreach (var row in rows)
                                {
                                    var words = row.Split(' ');
                                    foreach (var w in words)
                                    {
                                        var word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(w);
                                        if (string.IsNullOrEmpty(w))
                                            continue;
                                        if (!hunspell.Spell(word) && !Module.Helpers.TextHelper.IsRoman(word))
                                        {
                                            if (!resutl.Contains(word))
                                                resutl.Add(word);
                                            //Từ sửa chữa thay thế
                                            if (System.Diagnostics.Debugger.IsAttached)
                                            {
                                                var suggestions = hunspell.Suggest(word);
                                                foreach (string suggestion in suggestions)
                                                {

                                                }
                                                if (suggestions.Count == 1)
                                                {

                                                }
                                            }

                                        }
                                    }
                                }
                                if (resutl.Count > 0)
                                {
                                    audioFlag = true;
                                    string tag = e.SelectedChoiceActionItem.Caption;
                                    tag += ": " + string.Join(", ", resutl.ToArray());
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, tag);
                                }
                            }
                            audio.Flag = audioFlag;
                        }
                    }
                }

            }
            else if (e.SelectedChoiceActionItem.Id.Equals("UpperCaseSecond"))
            {
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    var audioContent = audio.Content;
                    if (column == "Subtitle")
                        audioContent = audio.Subtitle;
                    else if (column == "Spelling")
                        audioContent = audio.Spelling;
                    if (string.IsNullOrEmpty(audioContent))
                        continue;

                    var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    var cnt = 1;
                    bool audioFlag = false;
                    var audioNote = "HTH:";
                    foreach (string row in rows)
                    {
                        var tempRow = System.Text.RegularExpressions.Regex.Replace(row, @"^[\s\-\+=.,!?;:…]+", "").Trim();

                        var words = tempRow.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                        if (words.Length < 2)
                            continue;
                        var word = words[1];
                        if (char.IsUpper(word[0]))
                        {
                            audioFlag = true;
                            audioNote += " " + cnt;
                        }
                        cnt++;
                    }
                    audio.Flag = audioFlag;

                    if (audio.Flag)
                        audio.Note = audioNote;
                }

            }
            else
            {
                string charList = null;
                System.Collections.Generic.List<char> characterErrorList = null;
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    //1
                    bool audioFlag = false;
                    //var audioCaseType = CaseType.General;

                    //Xóa Note nếu có dữ liệu
                    if (!string.IsNullOrEmpty(audio.Note))
                        audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                    if (audio.Delete || e.SelectedChoiceActionItem.Id.Equals("Clear"))
                        continue;
                    if (e.SelectedChoiceActionItem.Id.Equals("CompareSubtitleAndSpelling"))
                    {
                        if (!string.IsNullOrEmpty(audio.Subtitle))
                        {
                            audioFlag = !audio.Subtitle.Equals(audio.Spelling, System.StringComparison.OrdinalIgnoreCase);
                        }
                        else if (!string.IsNullOrEmpty(audio.Spelling))
                            audioFlag = true;
                        if (audioFlag == true)
                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    }
                    else
                    {
                        string audioContent = audio.Content;
                        if (column == "Subtitle")
                            audioContent = audio.Subtitle;
                        else if (column == "Spelling")
                            audioContent = audio.Spelling;
                        if (!string.IsNullOrEmpty(audioContent))
                        {
                            if (e.SelectedChoiceActionItem.Id.Equals("BeginNotUpperCase"))
                            {
                                //2023-09-25: chấp nhận 1 kí tự trắng ở đầu
                                if (audioContent[0] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(1);
                                //2
                                //Đúng là bắt đầu chữ thường, nhưng tên vẫn đề là Không viết hoa như cũ
                                //Bắt đầu chữ thường
                                //if (!char.IsUpper(audioContent[0]))

                                if (char.IsLower(audioContent[0]))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }

                            }
                            //else if (e.SelectedChoiceActionItem.Id.Equals("BeginLowercase"))
                            //{
                            //    //Bỏ chức năng này
                            //    //Bắt đầu chữ thường
                            //    if (char.IsLower(audioContent[0]))
                            //    {
                            //        audio.Flag = true;
                            //        if (!string.IsNullOrEmpty(audio.Note))
                            //            audio.Note += "; ";
                            //        audio.Note += e.SelectedChoiceActionItem.Caption;
                            //    }
                            //}
                            else if (e.SelectedChoiceActionItem.Id.Equals("BeginAbbreviationOrNumber"))
                            {
                                //Viết tắt hoặc số
                                //2023-09-25: chấp nhận 1 kí tự trắng ở đầu
                                //2023-09-25 : (Loại bỏ Hoa toàn bộ và Hoa chữ đầu)
                                if (audioContent[0] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(1);
                                if (Module.Helpers.TextHelper.CheckUpperCaseAll(audioContent))
                                    continue;
                                if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                    continue;
                                if (char.IsNumber(audioContent[0]) || (audioContent.Length > 1 && char.IsUpper(audioContent[0]) && char.IsUpper(audioContent[1])))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("BeginSignSpecialCharacter"))
                            {
                                //Dấu, ký tự đặc biệt
                                //2023-09-25: chấp nhận 1 kí tự trắng ở đầu
                                if (audioContent[0] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(1);
                                if (!char.IsLetterOrDigit(audioContent[0]))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("BeginSpaces"))
                            {
                                //Nhiều dấu cách                    
                                if (audioContent[0] == ' ' && audioContent.Length > 1 && audioContent[1] == ' ')
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            //else if (e.SelectedChoiceActionItem.Id.Equals("AbbreviationOrNumber"))
                            //{
                            //    //Bỏ chức năng này
                            //    //3
                            //    //Bắt đầu viết tắt hoặc số, hoặc dấu cách
                            //    if (audioContent[0] == ' ' || char.IsNumber(audioContent[0]) || (audioContent.Length > 1 && char.IsUpper(audioContent[0]) && char.IsUpper(audioContent[1])))
                            //    {
                            //        audioFlag = true;
                            //        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                            //    }
                            //}
                            else if (e.SelectedChoiceActionItem.Id.Equals("EndNormalCharacter"))
                            {
                                //7
                                // Kết thúc kí tự thường: chữ cái, chữ số
                                //2023-09-25: chấp nhận 1 kí tự trắng ở đầu
                                if (audioContent[audioContent.Length - 1] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(0, audioContent.Length - 1);
                                if (char.IsLetterOrDigit(audioContent[audioContent.Length - 1]))
                                {
                                    //2023-09-26: Loại bỏ trường  hợp hoa chữ đầu
                                    if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                        continue;
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("EndComma"))
                            {
                                //6
                                //Kết thúc phẩy
                                //2023-09-25: chấp nhận 1 kí tự trắng ở cuối
                                if (audioContent[audioContent.Length - 1] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(0, audioContent.Length - 1);
                                if (audioContent.EndsWith(",", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("EndSignOrSpecialCharacter"))
                            {
                                //7
                                //Kết thúc dấu và kí tự đặc biệt (gồm dấu cách) (NOT chấm, phẩy, chữ cái, chữ số)
                                //2025-06-04: Thêm dấu chấm hỏi
                                //2023-09-22: Kết thúc ký tự đặc biệt, dấu cách
                                if (audioContent[audioContent.Length - 1] == ' ' || (audioContent[audioContent.Length - 1] != '.' && audioContent[audioContent.Length - 1] != ','
                                         && audioContent[audioContent.Length - 1] != '?' && !char.IsLetterOrDigit(audioContent[audioContent.Length - 1])))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("EndAbbreviationOrNumber"))
                            {
                                //Kết thúc Viết tắt hoặc số
                                //2023-09-25: chấp nhận 1 kí tự trắng ở cuối
                                //2023-09-25 : (Loại bỏ Hoa toàn bộ và Hoa chữ đầu)
                                if (audioContent[audioContent.Length - 1] == ' ' && audioContent.Length > 1)
                                    audioContent = audioContent.Substring(0, audioContent.Length - 1);
                                if (Module.Helpers.TextHelper.CheckUpperCaseAll(audioContent))
                                    continue;
                                if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                    continue;
                                if (char.IsNumber(audioContent[audioContent.Length - 1]) || (audioContent.Length > 1 && char.IsUpper(audioContent[audioContent.Length - 1]) && char.IsUpper(audioContent[audioContent.Length - 2])))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("EndSpaces"))
                            {
                                //Kết thúc Nhiều dấu cách                    
                                if (audioContent[audioContent.Length - 1] == ' ' && audioContent.Length > 2 && audioContent[audioContent.Length - 2] == ' ')
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            //else if (e.SelectedChoiceActionItem.Id.Equals("EndNoPunctiuation"))
                            //{
                            //    //Bỏ chức năng này
                            //    //Kết thúc không chấm
                            //    if (!audioContent.EndsWith(".", System.StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        audioFlag = true;
                            //        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                            //    }
                            //}

                            else if (e.SelectedChoiceActionItem.Id.Equals("EndNoPunctiuationComma"))
                            {
                                //Kết thúc không chấm phẩy
                                //2023-07-26 Kết thúc không chấm phẩy > Kết thúc không dấu(dấu hỏi, nháy kép, chấm than, chấm phẩy ; ... các dấu kết thúc câu
                                string[] chars = new string[] { ".", ",", "?", "\"", "!", ";" };
                                bool endChar = false;
                                foreach (var c in chars)
                                {
                                    if (audioContent.EndsWith(c, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        endChar = true;
                                        break;
                                    }
                                }
                                if (!endChar)
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }

                            else if (e.SelectedChoiceActionItem.Id.Equals("UpperCaseAll"))
                            {
                                //4
                                //Toàn bộ viết hoa hoặc tắt
                                //2023-08-21 Toàn bộ viết hoa hoặc tắt > Hoa toàn phần : HOA TOÀN PHẦN(không tồn tại ký tự thường)
                                if (Module.Helpers.TextHelper.CheckUpperCaseAll(audioContent))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                    audio.CaseType = Module.BusinessObjects.CaseType.UpperCaseAll;
                                }

                                // Toàn bộ viết hoa hoặc tắt
                                //bool flag = true;
                                //var words = audioContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                //if(words.Length == 1)
                                //{
                                //    if(!char.IsUpper((char)audioContent[0]) || (audioContent.Length > 1 && !char.IsUpper((char)audioContent[1])))
                                //        flag = false;
                                //}
                                //else
                                //{
                                //    foreach (var word in words)
                                //    {
                                //        if (!char.IsUpper((char)word[0]))
                                //        {
                                //            flag = false;
                                //            break;
                                //        }
                                //    }
                                //}                        
                                //if (flag)
                                //    audioFlag = true;
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("UpperCase"))
                            {
                                //5
                                if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                    audio.CaseType = Module.BusinessObjects.CaseType.UpperCase;
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("UpperCaseMany"))
                            {
                                //001 UpperCaseMany / Nhiều hoa: > 50 % số từ là Đầu hoa hoặc Toàn hoa
                                if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column, true))
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                    audio.CaseType = Module.BusinessObjects.CaseType.UpperCaseMany;
                                }
                            }

                            else if (e.SelectedChoiceActionItem.Id.Equals("NextSameStyle"))
                            {
                                //8
                                //Kề sau cùng kiểu cách
                                if (audio.Start != null && audio.Video != null)
                                {
                                    var nextElement = audio.Video.AudioList.Where(m => !m.Delete && m.Start > audio.Start).OrderBy(m => m.Start).FirstOrDefault();
                                    if (nextElement != null)
                                    {
                                        if (audio.ParagraphStyle is null && nextElement.ParagraphStyle is null)
                                        {
                                            audioFlag = true;
                                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                        }
                                        else if (audio.ParagraphStyle != null && nextElement.ParagraphStyle != null && audio.ParagraphStyle.Oid.Equals(nextElement.ParagraphStyle.Oid))
                                        {
                                            audioFlag = true;
                                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                        }
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("MergeNext"))
                            {
                                //Có thể gộp sau
                                //Đáp ứng 3 điều kiện
                                //-Chữ thường ở cuối(không phải Viết hoa đầu mỗi từ) hoặc đầu
                                //-Kề sau hoặc Kề trước cùng kiểu
                                //-Bản thân hoặc kề trước có Không kề sau = False
                                //Nếu bản thân là không kề sau thì không hợp lệ
                                if (audio.UpperElement != null)
                                    continue;
                                if (audio.NotAdjacent)
                                    continue;
                                //Kiểm tra nếu không phải chữ thường hoặc số ở cuối thì bỏ qua
                                //2023-09-25
                                //- Chữ thường ở cuối / Dấu phẩy / Viết tắt hoặc số (chấp nhận 1 kí tự trắng cuối, không thuộc Hoa toàn bộ, Hoa chữ đầu)                       
                                if (audioContent[audioContent.Length - 1] == ' ')
                                    audioContent = audioContent.Substring(0, audioContent.Length - 1);
                                if (!char.IsLetterOrDigit(audioContent[audioContent.Length - 1]) && audioContent[audioContent.Length - 1] != ','
                                    && !(audioContent.Length > 1 && char.IsUpper(audioContent[audioContent.Length - 1]) && char.IsUpper(audioContent[audioContent.Length - 2])))
                                    continue;
                                if (audio.Start != null && audio.Video != null)
                                {
                                    var nextElement = audio.Video.AudioList.Where(m => !m.Delete && m.Start > audio.Start).OrderBy(m => m.Start).FirstOrDefault();
                                    if (nextElement != null)
                                    {
                                        if (nextElement.UpperElement != null)
                                            continue;
                                        string nextAudioContent = nextElement.Content;
                                        if (column == "Subtitle")
                                            nextAudioContent = nextElement.Subtitle;
                                        else if (column == "Spelling")
                                            nextAudioContent = nextElement.Spelling;
                                        //Nếu dòng tiếp theo là trống thì bỏ qua
                                        if (string.IsNullOrEmpty(nextAudioContent))
                                            continue;
                                        //2023-09-25 -Dòng kề sau bắt đầu chữ thường, 1 dấu cách +chữ thường
                                        if (nextAudioContent[0] == ' ')
                                            nextAudioContent = nextAudioContent.Substring(1);
                                        //Nếu dòng tiếp theo không phải là chữ thường ở đầu hoặc số thì bỏ qua
                                        if (!char.IsLower(nextAudioContent[0]))
                                            continue;
                                        if ((audio.ParagraphStyle is null && nextElement.ParagraphStyle is null) ||
                                            (audio.ParagraphStyle != null && nextElement.ParagraphStyle != null && audio.ParagraphStyle.Oid.Equals(nextElement.ParagraphStyle.Oid)))
                                        {
                                            //Kiểm tra không phải Viết hoa đầu mỗi từ
                                            if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                                continue;
                                            //Kiểm tra không phải Viết hoa đầu mỗi từ
                                            if (AudioService.ElementFlagUpperCase(audio.Video, nextAudioContent, column))
                                                continue;
                                            audioFlag = true;
                                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                        }
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("MergePrevious"))
                            {
                                if (audio.UpperElement != null)
                                    continue;
                                //Kiểm tra nếu không phải chữ thường ở cuối thì bỏ qua
                                if (audioContent[0] == ' ')
                                {
                                    audioContent = audioContent.Substring(1);
                                }
                                //2023-09-25: - Chữ thường ở đầu / 1 dấu cách + chữ thường
                                if (!char.IsLower(audioContent[0]))
                                    continue;
                                if (audio.Start != null && audio.Video != null)
                                {
                                    var previousElement = audio.Video.AudioList.Where(m => !m.Delete && m.Start < audio.Start).OrderByDescending(m => m.Start).FirstOrDefault();
                                    if (previousElement != null)
                                    {
                                        if (previousElement.UpperElement != null)
                                            continue;
                                        //Có thể gộp trước
                                        if (previousElement.NotAdjacent)
                                            continue;
                                        string nextAudioContent = previousElement.Content;
                                        if (column == "Subtitle")
                                            nextAudioContent = previousElement.Subtitle;
                                        else if (column == "Spelling")
                                            nextAudioContent = previousElement.Spelling;
                                        //2023-09-25: - Dòng kề trước: Chữ thường ở cuối / Dấu phẩy / Viết tắt hoặc số (chấp nhận 1 kí tự trắng cuối, không thuộc Hoa toàn bộ, Hoa chữ đầu)
                                        if (nextAudioContent[nextAudioContent.Length - 1] == ' ')
                                            nextAudioContent = nextAudioContent.Substring(0, nextAudioContent.Length - 1);
                                        //Nếu dòng tiếp theo là trống thì bỏ qua
                                        if (string.IsNullOrEmpty(nextAudioContent))
                                            continue;
                                        //Nếu dòng tiếp theo không phải là chữ thường hoặc số thì bỏ qua
                                        if (!char.IsLetterOrDigit(nextAudioContent[nextAudioContent.Length - 1]))
                                            continue;
                                        if ((audio.ParagraphStyle is null && previousElement.ParagraphStyle is null) ||
                                            (audio.ParagraphStyle != null && previousElement.ParagraphStyle != null && audio.ParagraphStyle.Oid.Equals(previousElement.ParagraphStyle.Oid)))
                                        {
                                            //Kiểm tra không phải Viết hoa đầu mỗi từ
                                            if (AudioService.ElementFlagUpperCase(audio.Video, audioContent, column))
                                                continue;
                                            //Kiểm tra không phải Viết hoa đầu mỗi từ
                                            if (AudioService.ElementFlagUpperCase(audio.Video, nextAudioContent, column))
                                                continue;
                                            //2023-09-26 Cờ có thể gộp trước
                                            //- Chữ thường ở đầu / 1 dấu cách +chữ thường
                                            //- Dòng kề trước: Chữ thường ở cuối / Dấu phẩy / Viết tắt hoặc số(chấp nhận 1 kí tự trắng cuối, không thuộc Hoa toàn bộ, Hoa chữ đầu)
                                            if (!char.IsLetterOrDigit(nextAudioContent[nextAudioContent.Length - 1]) && nextAudioContent[nextAudioContent.Length - 1] != ','
                                                        && !(nextAudioContent.Length > 1 && char.IsUpper(nextAudioContent[nextAudioContent.Length - 1]) && char.IsUpper(nextAudioContent[nextAudioContent.Length - 2])))
                                                continue;
                                            audioFlag = true;
                                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                        }
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("RepeatWord") || e.SelectedChoiceActionItem.Id.Equals("RepeatWordContent") || e.SelectedChoiceActionItem.Id.Equals("RepeatWordTranslate"))
                            {
                                //9,10
                                //Từ lặp trong nội dung
                                System.Collections.Generic.Dictionary<string, int> wordsDictionary = new System.Collections.Generic.Dictionary<string, int>();
                                //var contents = Module.Helpers.TextHelper.ReplaceSpecialCharacters(
                                //    e.SelectedChoiceActionItem.Id.Equals("RepeatWordContent") ? audio.Content : audio.Subtitle, new char[] { '.', ',' }, " ").Split(" ");
                                var contents = Module.Helpers.TextHelper.ReplaceSpecialCharacters(audioContent, new char[] { '.', ',' }, " ").Split(" ");
                                foreach (string content in contents)
                                {
                                    var word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(content);
                                    if (string.IsNullOrEmpty(word))
                                        continue;
                                    if (word.Length < 2)
                                        continue;
                                    //Nếu ký tự số thì bỏ qua
                                    bool hasNumber = false;
                                    foreach (var c in word)
                                    {
                                        if (char.IsNumber(c))
                                        {
                                            hasNumber = true;
                                            break;
                                        }
                                    }
                                    if (hasNumber)
                                        continue;
                                    word = word.ToLower();
                                    //2023-07-22 Sẽ tìm từ lặp lại trong cùng 1 câu, nếu tồn tại thì dựng cờ, lưu ra note(ghi đè) và khi hover Nội dung / Dịch: sẽ bôi đậm từ trong Note
                                    string key = Module.Helpers.TextHelper.KeyListContains(wordsDictionary.Keys, word);
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        wordsDictionary[key]++;
                                        audioFlag = true;
                                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                    }
                                    else
                                    {
                                        wordsDictionary.Add(word, 1);
                                    }
                                }

                                foreach (var key in wordsDictionary.Keys)
                                {
                                    //2023-07-22 Sẽ tìm từ lặp lại trong cùng 1 câu, nếu tồn tại thì dựng cờ, lưu ra note(ghi đè) và khi hover Nội dung / Dịch: sẽ bôi đậm từ trong Note
                                    if (wordsDictionary[key] > 1 && Module.Helpers.TextHelper.GetIndexWordInContent(key, audio.Note) < 0)
                                    {
                                        //if (!string.IsNullOrEmpty(audio.Note))
                                        //    audio.Note += ", ";
                                        audio.Note += Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, key);
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("English"))
                            {
                                //11
                                //Tiếng Anh
                                var wordNoSignList = ObjectSpace.GetObjects<Module.BusinessObjects.WordNoSign>().Select(m => m.Name.ToLower());
                                var contents = Module.Helpers.TextHelper.ReplaceSpecialCharacters(
                                    e.SelectedChoiceActionItem.Id.Equals("RepeatWordContent") ? audioContent : audio.Subtitle, new char[] { '.', ',' }, " ").Split(" ");
                                foreach (string content in contents)
                                {
                                    var word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(content);
                                    if (string.IsNullOrEmpty(word))
                                        continue;
                                    if (word.Length < 2)
                                        continue;
                                    //Nếu ký tự số thì bỏ qua
                                    bool hasNumber = false;
                                    foreach (var c in word)
                                    {
                                        if (char.IsNumber(c))
                                        {
                                            hasNumber = true;
                                            break;
                                        }
                                    }
                                    if (hasNumber)
                                        continue;
                                    if (!Module.Helpers.TextHelper.CheckUnicode(word) && !wordNoSignList.Contains(word))
                                    {
                                        audioFlag = true;
                                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                        break;
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("DifferentSentences"))
                            {
                                //12
                                //YC: Dịch thành phần: khi khác số câu không ghi vào Note nữa mà Dựng cờ,
                                //làm thêm chức năng Cờ thành phần > Khác số câu(Số câu sẽ cân cứ vào List kí tự ngăn câu)
                                if (!string.IsNullOrEmpty(audio.Subtitle) && !string.IsNullOrEmpty(audio.Content))
                                {
                                    if (audio.Subtitle.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.None).Length != audio.Content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.None).Length)
                                    {
                                        audioFlag = true;
                                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("MultiSentence"))
                            {
                                //Có ngắt câu
                                //a Phong cho em hỏi nếu ngắt câu hoặc dấu phẩy ở cuối câu có được tính không ạ
                                //Không, cái đó có cờ khác rồi, cờ này là để xem xét các TP có thể tách
                                if (audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("Comma"))
                            {
                                //Có dấu phẩy
                                //a Phong cho em hỏi nếu ngắt câu hoặc dấu phẩy ở cuối câu có được tính không ạ
                                //Không, cái đó có cờ khác rồi, cờ này là để xem xét các TP có thể tách
                                if (audioContent.Split(", ", System.StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                                {
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("CharacterError"))
                            {
                                if (audio.Video is null || audio.Video.LanguageOrigin is null)
                                {
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn ngôn ngữ gốc trước khi thực hiện tính năng này", InformationType.Error);
                                    break;
                                }
                                if (string.IsNullOrEmpty(charList))
                                {
                                    charList = audio.Video.LanguageOrigin.Character;
                                    if (audio.Video.LanguageTranslate != null)
                                    {
                                        if (string.IsNullOrEmpty(charList))
                                            charList = audio.Video.LanguageTranslate.Character;
                                        else if (!string.IsNullOrEmpty(audio.Video.LanguageTranslate.Character))
                                            charList = new string(charList.Union(audio.Video.LanguageTranslate.Character).ToArray());
                                    }
                                    if (string.IsNullOrEmpty(charList))
                                    {
                                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Ngôn ngữ không có danh sách ký tự", InformationType.Error);
                                        break;
                                    }
                                }
                                var characterErrors = Module.Helpers.TextHelper.GetExistCharacterError(charList, audioContent);
                                if (characterErrors?.Count() > 0)
                                {
                                    if (characterErrorList is null)
                                        characterErrorList = new System.Collections.Generic.List<char>();
                                    foreach (var c in characterErrors)
                                    {
                                        if (!characterErrorList.Contains(c))
                                            characterErrorList.Add(c);
                                    }
                                    audioFlag = true;
                                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, Module.Helpers.TextHelper.GetFirstLetterToUpper(e.SelectedChoiceActionItem.Caption) + ": " + string.Join(", ", characterErrors), false);
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("MultiLine"))
                            {
                                audioFlag = Module.Helpers.TextHelper.IsMultiLine(audioContent);
                                audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                            }
                        }
                    }
                    if (audio.Flag != audioFlag)
                        audio.Flag = audioFlag;
                    //if (audio.CaseType != audioCaseType)
                    //    audio.CaseType = audioCaseType;
                    countNumber++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / totalSelectObject).ToString("p0"), " ", stopWatch.Elapsed);
                }
                if (characterErrorList?.Count > 0)
                {
                    //characterErrorList.Sort();
                    Module.SystemObjects.Tools.ClipboardSetText(string.Join('|', characterErrorList.OrderBy(x => x.ToString(), System.StringComparer.OrdinalIgnoreCase)));
                }
            }

            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
            stopWatch.Stop();



            #endregion ElementFlagImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0559            Oid: 3788fbf3-5ca7-4a2b-a557-7887aaa1768c
		private void SplitElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SplitElement), "Tách dòng");              
      
            #region SplitElementImportCode
            if (e.SelectedChoiceActionItem.Id.StartsWith("Split"))
            {
                //Các từ tương đương dấu chấm, anh xem có bổ sung gì không ạ: dấu hỏi, chấm than, hai chấm, ba chấm
                char[] splitChars = e.SelectedChoiceActionItem.Id.Contains("Comma") ? new char[] { ',' }
                            : new char[] { '.', '?', '!', ':' };
                string[] splitStrings = splitChars.Select(x => x + " ").ToArray();
                var selectedAudio = View.SelectedObjects.Cast<Module.BusinessObjects.Audio>().ToList();
                audioService.SplitContentByNewLine(selectedAudio, splitStrings, splitChars);
                var selectedList = selectedAudio.Where(m => m.End != null).ToList();
                if (selectedList.Count > 0)
                {
                    var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                    var tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Temp" + video.Oid.ToString().Substring(0, 10));
                    if (!System.IO.Directory.Exists(tempFolder))
                        System.IO.Directory.CreateDirectory(tempFolder);
                    OpenAI.Audio.AudioClient audioClient = null;
                    string audioModel = null;

                    var useWhisperOffline = System.Convert.ToBoolean(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(View.ObjectSpace, "OpenAIUseWhisperOffline", "False").Value);

                    if (useWhisperOffline
                        //&& selectedList.Count > 1  //2025-06-12: bỏ yêu cầu lớn hơn 1
                        )
                    {
                        audioService.SplitAudiosUseWhisperOffline(selectedList, tempFolder, video, splitChars, e.SelectedChoiceActionItem.Id);
                    }
                    else
                    {
                        var iDicUrl = new System.Collections.Generic.Dictionary<string, string>();
                        foreach (Module.BusinessObjects.Audio audio in selectedList)
                        {
                            string languageCode = video.LanguageOrigin != null ? video.LanguageOrigin.Code : null;
                            if (string.IsNullOrEmpty(audio.Content) || audio.Start is null || audio.End is null)
                            {
                                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không đủ điều kiện: dấu ngăn cách, Bắt đầu, Kết Thúc", InformationType.Error);
                                continue;
                            }
                            //Dời chữ sau dấu chấm và dấu phẩy vào câu sau
                            System.Collections.Generic.IList<Module.BusinessObjects.Audio> addList =
                                new System.Collections.Generic.List<Module.BusinessObjects.Audio>();
                            //Bỏ ký tự đặc biệt
                            audio.Content = audio.Content.Trim().Replace(" ", " ");
                            //Bỏ 2 dấu cách
                            audio.Content = audio.Content.Trim().Replace("  ", " ");
                            object audioTextWords = null;
                            if (audio.BookMark != null && audio.BookMark.URL != null)
                            {
                                //Cách mới
                                string audioUrl = "";
                                if (iDicUrl.ContainsKey(audio.BookMark.URL))
                                    audioUrl = iDicUrl[audio.BookMark.URL];
                                else
                                {
                                    if (Module.Utils.YouTubeUtils.IsYoutubeUrl(audio.BookMark.URL))
                                    {
                                        var youtube = new YoutubeExplode.YoutubeClient();
                                        var videoId = YoutubeExplode.Videos.VideoId.Parse(audio.BookMark.URL);
                                        var videoYoutube = System.Threading.Tasks.Task.Run(async () =>
                                        {
                                            return await youtube.Videos.GetAsync(videoId).ConfigureAwait(false);
                                        }).Result;
                                        var subTitlePath = $"{tempFolder}\\{Module.Helpers.FileSystemHelper.GetValidFileName(videoYoutube.Title).Replace("   ", " ").Replace("  ", " ")}.srt";
                                        if (Path.GetFullPath(subTitlePath)
                                                .StartsWith(Path.GetFullPath(tempFolder).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar,
                                                            StringComparison.OrdinalIgnoreCase))
                                            audioUrl = subTitlePath;
                                        else
                                            audioUrl = Module.Utils.YouTubeUtils.DownloadFromYoutube(audio.BookMark.URL, tempFolder, false);
                                    }

                                    else if (Module.Helpers.MediaHelper.CheckVideoSupport(audio.BookMark.URL) ||
                                        Module.Utils.OpenAiUtils.CheckOpenAIAudioSupport(audio.BookMark.URL))
                                        audioUrl = audio.BookMark.URL;
                                }
                                if (!string.IsNullOrEmpty(audioUrl))
                                {
                                    if (!iDicUrl.ContainsKey(audio.BookMark.URL))
                                        iDicUrl.Add(audio.BookMark.URL, audioUrl);
                                    var inputFileInfo = new System.IO.FileInfo(audioUrl);
                                    var trimFile = tempFolder + "\\" + audio.Oid.ToString() + "_trim" + inputFileInfo.Extension;
                                    //Cắt file audio
                                    if (Module.Helpers.AudioVideoHelper.TrimAudio(ObjectSpace, audioUrl, trimFile, audio.GetRealTimeSpan(audio.Start), audio.GetRealTimeSpan(audio.End)) &&
                                        System.IO.File.Exists(trimFile))
                                    {
                                        string logContent = $"Tư liệu {video?.Code} - {video?.Oid} - {e.SelectedChoiceActionItem.Caption}: {audio.Content}";
                                        audioTextWords = Module.Utils.OpenAiUtils.OpenAIAudioTranscriptionToWords(ObjectSpace, Application, trimFile, logContent, ref audioClient, ref audioModel);
                                        if (System.IO.Directory.Exists(trimFile))
                                            System.IO.Directory.Delete(trimFile, true);
                                    }
                                }
                            }
                            //Dùng cách cũ
                            System.Collections.Generic.IList<int> dotcommaIndexs = new System.Collections.Generic.List<int>();
                            // char splitChar = e.SelectedChoiceActionItem.Id.Contains("Comma") ? ',' : '.';
                            //Bỏ xuống dòng bằng ký tự ngăn cách
                            audio.Content = audio.Content.Replace("\r\n", e.SelectedChoiceActionItem.Id.Contains("Comma") ? "," : ".");
                            //Các từ tương đương dấu chấm, anh xem có bổ sung gì không ạ: dấu hỏi, chấm than, hai chấm, ba chấm
                            //char[] splitChars = e.SelectedChoiceActionItem.Id.Contains("Comma") ? new char[] { ',' }: new char[] { '.', '?', '!', ':' };
                            for (int j = audio.Content.Length - 2; j > 1; j--)
                            {
                                if (audio.Content[j] == ' ' && splitChars.Contains(audio.Content[j - 1]))
                                    dotcommaIndexs.Add(j);
                            }
                            if (e.SelectedChoiceActionItem.Id.Contains("Comma"))
                            {
                                var content = audio.Content;
                                var middle = content.Length / 2;
                                var quarter = content.Length / 4;
                                var cnt = 0;

                                int? bestCommaIndex = null;
                                int minDistanceFromMiddle = int.MaxValue;

                                for (int i = 0; i < content.Length; i++)
                                {
                                    if (content[i] == ',')
                                    {
                                        cnt++;
                                        int distanceFromMiddle = Math.Abs(i - middle);
                                        if (distanceFromMiddle <= quarter && distanceFromMiddle < minDistanceFromMiddle)
                                        {
                                            bestCommaIndex = i;
                                            minDistanceFromMiddle = distanceFromMiddle;
                                        }
                                    }
                                }

                                dotcommaIndexs.Clear();
                                if (bestCommaIndex.HasValue)
                                    dotcommaIndexs.Add(bestCommaIndex.Value + 1); // +1 để bắt đầu cắt từ sau dấu phẩy
                                else if (cnt > 0 && !bestCommaIndex.HasValue)
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không đủ điều kiện để tách", InformationType.Error);
                                else if (cnt == 0 && !bestCommaIndex.HasValue)
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không có dấu để tách tương ứng", InformationType.Error);
                            }
                            else
                            {
                                for (int j = audio.Content.Length - 2; j > 1; j--)
                                {
                                    if (audio.Content[j] == ' ' && splitChars.Contains(audio.Content[j - 1]))
                                        dotcommaIndexs.Add(j);
                                }
                            }
                            //Fix nguyên gốc
                            string audioContent = audio.Content;
                            if (dotcommaIndexs.Count > 0)
                            {
                                foreach (var dotcommaIndex in dotcommaIndexs)
                                {
                                    var newSubtitle = new Module.BusinessObjects.Audio(audio.Session);
                                    newSubtitle.BookMark = audio.BookMark;
                                    newSubtitle.TranslateObject = audio.TranslateObject;
                                    addList.Add(newSubtitle);
                                    newSubtitle.End = audio.End;
                                    var endText = audio.Content.Substring(dotcommaIndex).Trim();
                                    //Ghép text dấu chấm, dấu phẩy                        
                                    newSubtitle.Content = endText;
                                    //subtitleWithSort[i + 1].Content = endText + " " + audio.Content;

                                    //Bỏ phần text thừa
                                    audio.Content = audio.Content.Substring(0, dotcommaIndex).Trim();

                                    if (audioTextWords != null)
                                    {
                                        var startSecond = audio.GetRealTimeSpan(audio.Start).Value.TotalSeconds;
                                        var firstWords = audio.Content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                        var endWords = endText.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                        //Dùng AI để xử lý text
                                        if (audioTextWords is string)
                                        {
                                            var jsonText = Newtonsoft.Json.JsonConvert.DeserializeObject((string)audioTextWords) as Newtonsoft.Json.Linq.JObject;
                                            var segments = jsonText.ContainsKey("segments") ? jsonText.Property("segments").First : null;
                                            Newtonsoft.Json.Linq.JArray words = null;
                                            if (segments != null && segments.First is Newtonsoft.Json.Linq.JObject)
                                            {
                                                foreach (var segment in segments)
                                                {
                                                    if (((Newtonsoft.Json.Linq.JObject)segment).ContainsKey("words"))
                                                    {
                                                        if (words is null)
                                                            words = ((Newtonsoft.Json.Linq.JObject)segment).Property("words").First as Newtonsoft.Json.Linq.JArray;
                                                        else
                                                            words.Merge(((Newtonsoft.Json.Linq.JObject)segment).Property("words").First as Newtonsoft.Json.Linq.JArray);
                                                    }
                                                }
                                                if (words != null && firstWords.Length <= words.Count)
                                                {
                                                    var currentWord = words[firstWords.Length - 1] as Newtonsoft.Json.Linq.JObject;
                                                    var nextWord = words.Count > firstWords.Length ? words[firstWords.Length] as Newtonsoft.Json.Linq.JObject : null;
                                                    var currentText = currentWord.Property("text").Value.ToString();
                                                    if (!currentText.Equals(firstWords[firstWords.Length - 1], System.StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        //Xác định lại từ đúng: có thể dịch chuyển +- 2 vị trí                                            
                                                        for (int i = firstWords.Length - 2; i < words.Count; i++)
                                                        {
                                                            if (i < 0)
                                                                continue;
                                                            var jsonTextWord = ((Newtonsoft.Json.Linq.JObject)words[i]).Property("text").Value.ToString();
                                                            if (jsonTextWord.Equals(firstWords[firstWords.Length - 1], System.StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                currentWord = words[i] as Newtonsoft.Json.Linq.JObject;
                                                                if (words.Count > i + 1)
                                                                    nextWord = words[i + 1] as Newtonsoft.Json.Linq.JObject;
                                                                break;
                                                            }
                                                            else if (i > 0 && endWords.Length > 0 && jsonTextWord.Equals(endWords[0], System.StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                currentWord = words[i - 1] as Newtonsoft.Json.Linq.JObject;
                                                                nextWord = words[i] as Newtonsoft.Json.Linq.JObject;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (currentWord.Property("end")?.Value != null)
                                                    {
                                                        var audioEnd = System.TimeSpan.FromSeconds(System.Convert.ToDouble(currentWord.Property("end")?.Value));
                                                        //Công thêm ngày
                                                        if (audio.Start != null && audio.Start.Value.Days > 0)
                                                            audioEnd = audioEnd.Add(System.TimeSpan.FromDays(audio.Start.Value.Days));
                                                        //Cộng thêm thời gian bắt đầu trước khi cắt khi làm tròn xuống
                                                        audioEnd = audioEnd.Add(System.TimeSpan.FromSeconds(startSecond));
                                                        audio.End = audioEnd;
                                                    }

                                                    if (nextWord != null)
                                                    {
                                                        var audioStart = System.TimeSpan.FromSeconds(System.Convert.ToDouble(nextWord.Property("start").Value));
                                                        if (audio.Start != null && audio.Start.Value.Days > 0)
                                                            audioStart = audioStart.Add(System.TimeSpan.FromDays(audio.Start.Value.Days));
                                                        //Cộng thêm thời gian bắt đầu trước khi cắt khi làm tròn xuống
                                                        audioStart = audioStart.Add(System.TimeSpan.FromSeconds(startSecond));
                                                        newSubtitle.Start = audioStart;
                                                    }
                                                }
                                            }
                                        }
                                        else if (audioTextWords is OpenAI.Audio.AudioTranscription)
                                        {
                                            var audioTranscription = (OpenAI.Audio.AudioTranscription)audioTextWords;
                                            if (audioTranscription.Words != null)
                                            {
                                                if (firstWords.Length < audioTranscription.Words.Count)
                                                {
                                                    var currentText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(firstWords[firstWords.Length - 1]);
                                                    var currentWord = audioTranscription.Words[firstWords.Length - 1];
                                                    //var nextWord = audioTranscription.Words[firstWords.Length];
                                                    if (!currentWord.Word.Equals(firstWords[firstWords.Length - 1], System.StringComparison.OrdinalIgnoreCase) &&
                                                        currentText.Equals(firstWords[firstWords.Length - 1], System.StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        //Xác định lại từ đúng: có thể dịch chuyển +- 2 vị trí
                                                        for (int i = firstWords.Length - 2; i < audioTranscription.Words.Count; i++)
                                                        {
                                                            if (i < 0)
                                                                continue;
                                                            var jsonTextWord = audioTranscription.Words[i].Word;
                                                            if (jsonTextWord.Equals(firstWords[firstWords.Length - 1], System.StringComparison.OrdinalIgnoreCase) ||
                                                                jsonTextWord.Equals(currentText, System.StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                currentWord = audioTranscription.Words[i];
                                                                //if (audioTranscription.Words.Count > i + 1)
                                                                //    nextWord = audioTranscription.Words[i + 1];
                                                                break;
                                                            }
                                                            else if (i > 0 && endWords.Length > 0 && jsonTextWord.Equals(endWords[0], System.StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                currentWord = audioTranscription.Words[i - 1];
                                                                //nextWord = audioTranscription.Words[i];
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    var audioEnd = currentWord.End;
                                                    //Công thêm ngày
                                                    if (audio.Start != null && audio.Start.Value.Days > 0)
                                                        audioEnd = audioEnd.Add(System.TimeSpan.FromDays(audio.Start.Value.Days));
                                                    //Cộng thêm thời gian bắt đầu trước khi cắt khi làm tròn xuống
                                                    audioEnd = audioEnd.Add(System.TimeSpan.FromSeconds(startSecond));
                                                    audio.End = audioEnd;
                                                    if (audioTranscription.Words.Count > firstWords.Length)
                                                    {
                                                        var audioStart = audioTranscription.Words[firstWords.Length].Start;
                                                        if (audio.Start != null && audio.Start.Value.Days > 0)
                                                            audioStart = audioStart.Add(System.TimeSpan.FromDays(audio.Start.Value.Days));
                                                        //Cộng thêm thời gian bắt đầu trước khi cắt khi làm tròn xuống
                                                        audioStart = audioStart.Add(System.TimeSpan.FromSeconds(startSecond));
                                                        newSubtitle.Start = audioStart;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    if (newSubtitle.Start is null)
                                    {
                                        var totalWordVowelWeight = Module.Helpers.TextHelper.GetWordVowelWeight(languageCode, audioContent);
                                        var endContentLength = Module.Helpers.TextHelper.GetWordVowelWeight(languageCode, newSubtitle.Content);
                                        var wordLength = totalWordVowelWeight != (decimal)0 ? (audio.End.Value - audio.Start.Value).TotalMilliseconds / System.Convert.ToInt32(totalWordVowelWeight) : 0;
                                        var timeStart = audio.End.Value - System.TimeSpan.FromMilliseconds(wordLength * System.Convert.ToDouble(endContentLength));
                                        newSubtitle.Start = System.TimeSpan.FromMilliseconds(System.Math.Round(timeStart.TotalMilliseconds / 100) * 100);
                                        audio.End = newSubtitle.Start;
                                    }
                                    audio.Quantity = audio.GetDefaultQuantity();
                                    audio.Splitted = true;
                                    if (audio.End is null)
                                        audio.End = newSubtitle.Start;

                                }

                            }
                            audio.Video.AudioList.AddRange(addList);
                        }

                    }

                    Module.SystemObjects.Tools.RefreshGridView(View);

                }




            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("Contain"))
            {
                int result = 0;
                int total = View.SelectedObjects.Count;
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    if (audio.Video is null || string.IsNullOrEmpty(audio.Content))
                        continue;
                    var otherAudio = audio.Video.AudioList.Where(m => m.Oid != audio.Oid && audio.Note == m.Note && audio.BookMark != m.BookMark).FirstOrDefault();
                    if (otherAudio != null)
                    {
                        var startIndex = audio.Content.IndexOf(otherAudio.Content);
                        if (startIndex >= 0)
                        {
                            if (e.SelectedChoiceActionItem.Id.Equals("ContainBegin"))
                            {
                                if (startIndex == 0 && audio.Content.Length != otherAudio.Content.Length)
                                {
                                    //var newAudio = new Module.BusinessObjects.Audio(audio.Session);
                                    //newAudio.BookMark = audio.BookMark;
                                    //newAudio.TranslateObject = audio.TranslateObject;
                                    //newAudio.Content = audio.Content.Substring(otherAudio.Content.Length);
                                    //if (audio.Start != null)
                                    //    newAudio.Start = audio.Start.Value.Add(System.TimeSpan.FromMilliseconds(100));
                                    //newAudio.End = audio.End;
                                    //newAudio.Note = audio.Note;
                                    //newAudio.Subtitle = audio.Subtitle;
                                    //newAudio.Spelling = audio.Spelling;
                                    //newAudio.Flag = audio.Flag;
                                    //newAudio.CaseType = audio.CaseType;
                                    //newAudio.Quantity = audio.Quantity;
                                    //newAudio.Splitted = audio.Splitted;
                                    //newAudio.ParagraphStyle = audio.ParagraphStyle;
                                    ////newAudio.Video = audio.Video;
                                    //audio.Video.AudioList.Add(newAudio);
                                    //audio.Content = audio.Content.Substring(0, otherAudio.Content.Length);
                                    audio.Content = audio.Content.Substring(0, otherAudio.Content.Length) + "\r\n" + audio.Content.Substring(otherAudio.Content.Length).TrimStart();
                                    result++;
                                }

                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("ContainEnd"))
                            {
                                if (startIndex > 0)
                                {
                                    //var newAudio = new Module.BusinessObjects.Audio(audio.Session);
                                    //newAudio.BookMark = audio.BookMark;
                                    //newAudio.TranslateObject = audio.TranslateObject;
                                    //newAudio.Content = audio.Content.Substring(0, startIndex);
                                    //if (audio.Start != null)
                                    //    newAudio.Start = audio.Start.Value.Add(System.TimeSpan.FromMilliseconds(-100));
                                    //newAudio.End = audio.End;
                                    //newAudio.Note = audio.Note;
                                    //newAudio.Subtitle = audio.Subtitle;
                                    //newAudio.Spelling = audio.Spelling;
                                    //newAudio.Flag = audio.Flag;
                                    //newAudio.CaseType = audio.CaseType;
                                    //newAudio.Quantity = audio.Quantity;
                                    //newAudio.Splitted = audio.Splitted;
                                    //newAudio.ParagraphStyle = audio.ParagraphStyle;
                                    ////newAudio.Video = audio.Video;
                                    //audio.Video.AudioList.Add(newAudio);
                                    //audio.Content = audio.Content.Substring(startIndex);
                                    audio.Content = audio.Content.Substring(0, startIndex) + "\r\n" + audio.Content.Substring(startIndex).TrimStart();
                                    result++;
                                }
                            }
                            else
                            {
                                if (startIndex > 0)
                                {
                                    var newAudio = new Module.BusinessObjects.Audio(audio.Session);
                                    newAudio.BookMark = audio.BookMark;
                                    newAudio.TranslateObject = audio.TranslateObject;
                                    newAudio.Content = audio.Content.Substring(startIndex, otherAudio.Content.Length);
                                    if (audio.Start != null)
                                        newAudio.Start = audio.Start.Value.Add(System.TimeSpan.FromMilliseconds(100));
                                    newAudio.End = audio.End;
                                    newAudio.Note = audio.Note;
                                    newAudio.Subtitle = audio.Subtitle;
                                    newAudio.Spelling = audio.Spelling;
                                    newAudio.Flag = audio.Flag;
                                    //newAudio.CaseType = audio.CaseType;
                                    newAudio.Quantity = audio.Quantity;
                                    newAudio.Splitted = audio.Splitted;
                                    newAudio.ParagraphStyle = audio.ParagraphStyle;
                                    //newAudio.Video = audio.Video;
                                    audio.Video.AudioList.Add(newAudio);
                                }

                                if (startIndex + otherAudio.Content.Length < audio.Content.Length)
                                {
                                    var newAudio = new Module.BusinessObjects.Audio(audio.Session);
                                    newAudio.BookMark = audio.BookMark;
                                    newAudio.TranslateObject = audio.TranslateObject;
                                    newAudio.Content = audio.Content.Substring(startIndex + otherAudio.Content.Length);
                                    if (audio.Start != null)
                                        newAudio.Start = audio.Start.Value.Add(System.TimeSpan.FromMilliseconds(200));
                                    newAudio.End = audio.End;
                                    newAudio.Note = audio.Note;
                                    newAudio.Subtitle = audio.Subtitle;
                                    newAudio.Spelling = audio.Spelling;
                                    newAudio.Flag = audio.Flag;
                                    //newAudio.CaseType = audio.CaseType;
                                    newAudio.Quantity = audio.Quantity;
                                    newAudio.Splitted = audio.Splitted;
                                    //newAudio.Video = audio.Video;
                                    audio.Video.AudioList.Add(newAudio);
                                }

                                audio.Content = audio.Content.Substring(0, startIndex);
                                result++;
                            }
                        }
                    }
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", $"{result}/{total} được thực hiện", InformationType.Info);
            }






            #endregion SplitElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0548            Oid: 699bcfdc-d9a9-4805-a789-b5a0d0046081
		private void MergeTwoElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MergeTwoElement), "Gộp trên dưới");              
      
            #region MergeTwoElementImportCode
            decimal total = View.SelectedObjects.Count;
            decimal countNumber = 0;
            if (View.SelectedObjects.Count != 2 &&
                (e.SelectedChoiceActionItem.Id.Equals("ToBelow") || e.SelectedChoiceActionItem.Id.Equals("ToAbove")))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Lựa chọn này phải chọn 2 dòng", InformationType.Error);
                return;
            }
            var deleteAudios = new System.Collections.Generic.List<Module.BusinessObjects.Audio>();
            //Khi ghép sẽ ghép từ dưới lên, để có thể gộp dòng dưới khi cần
            var selectedAudios = new System.Collections.Generic.List<Module.BusinessObjects.Audio>();
            foreach (Module.BusinessObjects.Audio currentAudio in View.SelectedObjects)
            {
                if (!currentAudio.Delete)
                    selectedAudios.Add(currentAudio);
            }
            if (e.SelectedChoiceActionItem.Id.Equals("Below") || e.SelectedChoiceActionItem.Id.Equals("ToBelow"))
                selectedAudios = selectedAudios.OrderByDescending(m => m.Start).ToList();
            else if (e.SelectedChoiceActionItem.Id.Equals("Above") || e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                selectedAudios = selectedAudios.OrderBy(m => m.Start).ToList();
            if (selectedAudios.Count < 2 && (e.SelectedChoiceActionItem.Id.Equals("ToBelow") || e.SelectedChoiceActionItem.Id.Equals("ToAbove")))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Dòng lựa chọn đã bị đánh dấu xóa", InformationType.Error);
                return;
            }
            Module.BusinessObjects.Audio lastedAudio = null;
            foreach (Module.BusinessObjects.Audio currentAudio in selectedAudios)
            {
                if (currentAudio.Start is null || currentAudio.Video is null || currentAudio.IsDeleted) continue;
                Module.BusinessObjects.Audio removeAudio = null;
                //Dòng dưới
                if (e.SelectedChoiceActionItem.Id.Equals("Below"))
                    removeAudio = currentAudio.Video.AudioList.Where(m => !m.Delete).OrderBy(m => m.Start).Where(m => m.Start > currentAudio.Start).FirstOrDefault();
                //Dòng trên
                else if (e.SelectedChoiceActionItem.Id.Equals("Above"))
                    removeAudio = currentAudio.Video.AudioList.Where(m => !m.Delete).OrderByDescending(m => m.Start).Where(m => m.Start < currentAudio.Start).FirstOrDefault();
                else if (e.SelectedChoiceActionItem.Id.Equals("ToBelow"))
                    removeAudio = selectedAudios[1];
                else if (e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                    removeAudio = selectedAudios[1];
                if (removeAudio != null)
                {
                    string newContent = currentAudio.Content;
                    string newSubtitle = currentAudio.Subtitle;
                    string newSpelling = currentAudio.Spelling;
                    if (e.SelectedChoiceActionItem.Id.Equals("Below") || e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                    {
                        if (!string.IsNullOrEmpty(newContent) && !newContent.EndsWith(' '))
                            newContent += " ";
                        newContent += removeAudio.Content;

                        if (!string.IsNullOrEmpty(newSubtitle) && !newSubtitle.EndsWith(' '))
                            newSubtitle += " ";
                        newSubtitle += removeAudio.Subtitle;

                        if (!string.IsNullOrEmpty(newSpelling) && !newSpelling.EndsWith(' '))
                            newSpelling += " ";
                        newSpelling += removeAudio.Spelling;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Above") || e.SelectedChoiceActionItem.Id.Equals("ToBelow"))
                    {
                        if (!string.IsNullOrEmpty(newContent) && !newContent.StartsWith(' '))
                            newContent = " " + newContent;
                        newContent = removeAudio.Content + newContent;

                        if (!string.IsNullOrEmpty(newSubtitle) && !newSubtitle.StartsWith(' '))
                            newSubtitle = " " + newSubtitle;
                        newSubtitle = removeAudio.Subtitle + newSubtitle;

                        if (!string.IsNullOrEmpty(newSpelling) && !newSpelling.StartsWith(' '))
                            newSpelling = " " + newSpelling;
                        newSpelling = removeAudio.Spelling + newSpelling;
                    }

                    if (newContent.Length > 2000)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Nội dung quá dài", InformationType.Error);
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
                        return;
                    }
                    if (newSubtitle.Length > 2000)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Phụ đề quá dài", InformationType.Error);
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
                        return;
                    }
                    if (newContent.Length > 2000)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Phiên âm quá dài", InformationType.Error);
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
                        return;
                    }
                    if (removeAudio.TermLocationList.Count > 0)
                    {
                        //2024-28-08
                        //Thành phần A(n câu và câu cuối có m từ) gộp với thành phần B
                        //Thay đổi thuật vị trước khi nội dung thay đổi để tránh lỗi
                        //- Trường hợp cuối A không có ngắt câu khi đó các thuật vị thuộc B sẽ chỉnh
                        //+Thành phần = Thành phần A
                        //+Câu = n + số câu hiện tại - 1
                        //+ Vị trí = vị trí hiện tại + m(nếu là câu 1 của B)
                        //+ Vị trí không đổi nếu là câu 2 trở đi của B

                        //-Trường hợp cuối A có ngắt câu
                        //+Thành phần = Thành phần A
                        //+Câu = n + số câu hiện tại
                        //+ Vị trí không đổi                        
                        if (e.SelectedChoiceActionItem.Id.Equals("Below") || e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                        {
                            audioService.UpdateTermLocationAfterMerge(removeAudio, currentAudio);
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("Above") || e.SelectedChoiceActionItem.Id.Equals("ToBelow"))
                        {
                            audioService.UpdateTermLocationAfterMerge(currentAudio, removeAudio);
                        }
                        currentAudio.TermLocationList.AddRange(removeAudio.TermLocationList.ToList());
                    }
                    currentAudio.Content = newContent;
                    removeAudio.Content = "";
                    currentAudio.Subtitle = newSubtitle;
                    removeAudio.Subtitle = "";
                    currentAudio.Spelling = newSpelling;
                    removeAudio.Spelling = "";
                    removeAudio.Delete = true;
                    //Nếu là audio thì xóa, còn tài liệu thì không xóa
                    if (currentAudio.End != null)
                    {
                        if (e.SelectedChoiceActionItem.Id.Equals("Below"))
                        {
                            currentAudio.End = removeAudio.End;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("Above"))
                        {
                            currentAudio.Start = removeAudio.Start;
                        }
                        currentAudio.Splitted = removeAudio.Splitted;
                        deleteAudios.Add(removeAudio);
                    }
                    if (removeAudio.FileData != null && !removeAudio.FileData.IsEmpty)
                    {
                        if (currentAudio.FileData is null)
                            currentAudio.FileData = new DevExpress.Persistent.BaseImpl.FileData(currentAudio.Session);
                        if (currentAudio.FileData.IsEmpty)
                            currentAudio.FileData.Content = removeAudio.FileData.Content;
                        else
                        {
                            var concatenateByte = new System.Byte[currentAudio.FileData.Content.Length + removeAudio.FileData.Content.Length];

                            if (e.SelectedChoiceActionItem.Id.Equals("Below") || e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                            {
                                System.Array.Copy(currentAudio.FileData.Content, 0, concatenateByte, 0, currentAudio.FileData.Content.Length);
                                System.Array.Copy(removeAudio.FileData.Content, 0, concatenateByte, currentAudio.FileData.Content.Length, removeAudio.FileData.Content.Length);
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("Above") || e.SelectedChoiceActionItem.Id.Equals("ToBelow"))
                            {
                                System.Array.Copy(removeAudio.FileData.Content, 0, concatenateByte, 0, removeAudio.FileData.Content.Length);
                                System.Array.Copy(currentAudio.FileData.Content, 0, concatenateByte, removeAudio.FileData.Content.Length, currentAudio.FileData.Content.Length);
                            }
                            //System.IO.File.WriteAllBytes(currentAudio.FileData.FileName, concatenateByte);
                            currentAudio.FileData.Content = concatenateByte;
                        }
                    }
                    currentAudio.AudioDuration += removeAudio.AudioDuration;


                    lastedAudio = currentAudio;
                    //Tools.RefreshGridView(View);
                }
                //Nếu là chức năng gộp dòng trên dưới thì chỉ lựa chọn 1 dòng
                if (e.SelectedChoiceActionItem.Id.Equals("ToBelow") || e.SelectedChoiceActionItem.Id.Equals("ToAbove"))
                    break;
                countNumber++;
                var percent = countNumber / total;
                if (percent < 100)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(percent.ToString("p0"), " ");
            }
            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
            if (deleteAudios.Count > 0)
            {
                foreach (Module.BusinessObjects.Audio currentAudio in deleteAudios)
                {
                    currentAudio.Delete();
                }
            }
            if (lastedAudio != null)
                View.CurrentObject = lastedAudio;






            #endregion MergeTwoElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 4547            Oid: 35cf4a10-eb55-40c7-8899-fc084d87a51b
		private void AudioRecord_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AudioRecord), "Thu âm");              
      
            #region AudioRecordImportCode


            #endregion AudioRecordImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1032            Oid: da34dec9-bdb9-4b66-abfa-d2c9a5211c9e
		private void TextQuantity_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TextQuantity), "Số lượng");              
      
            #region TextQuantityImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            var languageOriginCode = "en";
            var languageTranslateCode = "";
            if (video.LanguageOrigin != null && !string.IsNullOrEmpty(video.LanguageOrigin.Code))
                languageOriginCode = video.LanguageOrigin.Code;
            if (video.LanguageTranslate != null && !string.IsNullOrEmpty(video.LanguageTranslate.Code))
                languageTranslateCode = video.LanguageTranslate.Code;

            string column = "Content";
            if (View is ListView && ((ListView)View).Editor != null)
            {
                var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                {
                    if ((string)focusedColumnMemberName == "Content" || (string)focusedColumnMemberName == "Subtitle" || (string)focusedColumnMemberName == "Spelling")
                    {
                        column = (string)focusedColumnMemberName;
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Nội dung hoặc cột dịch hoặc phiên âm trước khi thực hiện tính năng này", InformationType.Error);
                        return;
                    }
                }
            }
            char charTagNote = '(';
            string noteTag = "";
            var languageCode = "";
            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                string text = "";
                if (column == "Content")
                {
                    text = audio.Content;
                    noteTag = Module.Helpers.TextHelper.GetTagNode(charTagNote, e.SelectedChoiceActionItem.Caption + " Nội dung");
                    languageCode = languageOriginCode;
                }
                else if (column == "Subtitle")
                {
                    text = audio.Subtitle;
                    noteTag = Module.Helpers.TextHelper.GetTagNode(charTagNote, e.SelectedChoiceActionItem.Caption + " Dịch");
                    languageCode = languageTranslateCode;
                }
                else if (column == "Spelling")
                {
                    text = audio.Spelling;
                    noteTag = Module.Helpers.TextHelper.GetTagNode(charTagNote, e.SelectedChoiceActionItem.Caption + " Phiên âm");
                    languageCode = languageTranslateCode;
                }
                if (!string.IsNullOrEmpty(text))
                {
                    if (e.SelectedChoiceActionItem.Id.Equals("Word"))
                    {
                        audio.Quantity = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Character"))
                    {
                        audio.Quantity = text.Length;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Syllable"))
                    {
                        //Âm tiết, chỉ có Tiếng Anh mới có âm tiết

                        audio.Quantity = System.Convert.ToInt32(Module.Helpers.TextHelper.GetWordVowelWeight(languageCode, text));
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("LineBreak"))
                    {
                        audio.Quantity = text.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }
                    if (!string.IsNullOrEmpty(audio.Note))
                    {
                        //Xóa ghi chú tag trước đó
                        audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTagNote, false);
                    }
                    //audio.Note += noteTag;                    
                }
            }




            #endregion TextQuantityImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0549            Oid: be7787ad-63a3-44e8-b7e9-0302dd4dcedd
		private void MergeElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MergeElement), "Gộp nhiều");              
      
            #region MergeElementImportCode
            decimal total = e.SelectedObjects.Count;
            decimal countNumber = 0;
            //Đ/k nhiều dòng liên tục và không có Khoảng lặng, cuối câu k có dấu chấm, phẩy, đầu dòng tiếp theo chữ không hoa
            //2023-05-29: Ghép theo dấu chấm hoặc dấu phẩy
            string endText = e.SelectedChoiceActionItem.Id.Contains("Dot") ? "." : ",";
            //Nạp tham số
            string keyMergeMultiSubtitleGap = "MergeMultiSubtitleGap";
            var parameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MergeMultiSubtitleGap", "0.1");
            double mergeMultiSubtitleGap = parameter.GetDoubleValue();
            //Tạo danh sách select với sort
            var audioList = e.SelectedObjects.Cast<Module.BusinessObjects.Audio>();
            var audioWithSort = audioList.OrderBy(m => m.Start).ToList();
            System.Collections.Generic.IList<Module.BusinessObjects.Audio> keepList =
                new System.Collections.Generic.List<Module.BusinessObjects.Audio>();

            for (int i = 0; i < audioWithSort.Count; i++)
            {
                if (!string.IsNullOrEmpty(audioWithSort[i].Content))
                {
                    keepList.Add(audioWithSort[i]);
                    //Nếu là audio cuối cùng thì không cần ghép nữa
                    if (i == audioWithSort.Count - 1)
                        break;
                    if (!string.IsNullOrEmpty(audioWithSort[i + 1].Content) && audioWithSort[i].Start != null && audioWithSort[i].End != null
                        && audioWithSort[i + 1].Start != null && audioWithSort[i + 1].End != null)
                    {
                        string currentContent = audioWithSort[i].Content.Trim();
                        string nextContent = audioWithSort[i + 1].Content.Trim();
                        //Kiểm tra xem Audio hiện tại có cần ghép không
                        if ((audioWithSort[i + 1].Start.Value - audioWithSort[i].End.Value).TotalSeconds > mergeMultiSubtitleGap
                            || currentContent.EndsWith(endText) || (char.IsUpper(nextContent[0])
                            && nextContent.Length >= 2 && !char.IsUpper(nextContent[1])))
                        {
                            //Nếu đúng điều kiện này thì sẽ không cần ghép
                            //Đ/k nhiều dòng liên tục và không có Khoảng lặng, cuối câu k có dấu chấm, phẩy, đầu dòng tiếp theo chữ không hoa
                            continue;
                        }
                        int increaseIndex = 0;
                        for (int j = i + 1; j < audioWithSort.Count; j++)
                        {

                            //bool continueMerge = true;

                            string continueContent = audioWithSort[j].Content.Trim();
                            //Kiểm tra xem Audio này có cần ghép không
                            if (audioWithSort[j].Start is null)
                                break;
                            else if ((audioWithSort[j].Start.Value - audioWithSort[i].End.Value).TotalSeconds > mergeMultiSubtitleGap)
                                break;
                            else if (currentContent.EndsWith(endText))
                                break;
                            else if (char.IsUpper(continueContent[0]) && continueContent.Length >= 2 && !char.IsUpper(continueContent[1]))
                                break;

                            //Trường hợp mặc định
                            string newContent = audioWithSort[i].Content;
                            if (!string.IsNullOrEmpty(newContent) && !newContent.EndsWith(' '))
                                newContent += " ";
                            newContent += audioWithSort[j].Content;
                            currentContent = audioWithSort[i].Content.Trim();
                            if (newContent.Length > 2000)
                                break;

                            string newSubtitle = audioWithSort[i].Subtitle;
                            if (!string.IsNullOrEmpty(newSubtitle) && !newSubtitle.EndsWith(' '))
                                newSubtitle += " ";
                            newSubtitle += audioWithSort[j].Subtitle;
                            if (newSubtitle.Length > 2000)
                                break;

                            string newSpelling = audioWithSort[i].Spelling;
                            if (!string.IsNullOrEmpty(newSpelling) && !newSpelling.EndsWith(' '))
                                newSpelling += " ";
                            newSpelling += audioWithSort[j].Spelling;
                            if (newSpelling.Length > 2000)
                                break;
                            //2024-08-28: Cập nhật lại vị trí thuật vị sau khi ghép
                            audioService.UpdateTermLocationAfterMerge(audioWithSort[j], audioWithSort[i]);
                            audioWithSort[i].TermLocationList.AddRange(audioWithSort[j].TermLocationList.ToList());

                            audioWithSort[i].Content = newContent;
                            audioWithSort[i].Subtitle = newSubtitle;
                            audioWithSort[i].Spelling = newSpelling;

                            audioWithSort[i].End = audioWithSort[j].End;
                            audioWithSort[i].Splitted = audioWithSort[j].Splitted;
                            increaseIndex++;
                            //if (!continueMerge)
                            //    break;

                        }
                        i += increaseIndex;
                    }

                }
                countNumber++;
                var percent = countNumber / total;
                if (percent < 100)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(percent.ToString("p0"), " ");
            }
            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);
            int deleted = 0;
            //Xóa phụ đề thừa
            for (int i = audioWithSort.Count - 1; i >= 0; i--)
            {
                if (!keepList.Contains(audioWithSort[i]))
                {
                    audioWithSort[i].Delete();
                    deleted++;
                }
            }
            if (keepList.Count > 0)
            {
                View.CurrentObject = keepList[0];
            }
            if (deleted == 0)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Thành phần được chọn không đáp ứng điều kiện", InformationType.Error);
            }



            #endregion MergeElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1033            Oid: 62034514-7e6a-4bbe-863b-70fc90f5c0eb
		private void TextRate_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TextRate), "Tỉ suất");              
      
            #region TextRateImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            var languageOriginCode = "en";
            var languageTranslateCode = "";
            if (video.LanguageOrigin != null && !string.IsNullOrEmpty(video.LanguageOrigin.Code))
                languageOriginCode = video.LanguageOrigin.Code;
            if (video.LanguageTranslate != null && !string.IsNullOrEmpty(video.LanguageTranslate.Code))
                languageTranslateCode = video.LanguageTranslate.Code;
            char charTag = '|';
            //string noteTag = "|" + e.SelectedChoiceActionItem.Caption + "|";
            if (e.SelectedChoiceActionItem.Id.Equals("Syllable"))
            {
                //2024-08-13: Số âm tiết/tổng âm tiết) / (thời lượng/tổng thời lượng)
                //số âm tiết là âm tiết của phiên âm
                decimal totalSpellingTextRate = 0, totalContentDuration = 0;
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    totalSpellingTextRate += Module.Helpers.TextHelper.GetWordVowelWeight(languageTranslateCode, audio.Spelling);
                    totalContentDuration += audio.Duration ?? 0m;
                }
                if (totalSpellingTextRate == 0 || totalContentDuration == 0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không thể tính tỉ suất với dữ liệu hiện tại", InformationType.Error);
                    return;
                }
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    var spellingTextRate = Module.Helpers.TextHelper.GetWordVowelWeight(languageTranslateCode, audio.Spelling);
                    audio.TextRate = (spellingTextRate / totalSpellingTextRate) / ((audio.Duration ?? 0m) / totalContentDuration);
                    if (!string.IsNullOrEmpty(audio.Note))
                    {
                        //Xóa ghi chú tag trước đó
                        audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                    }
                    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                }
                return;
            }
            foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
            {
                if (e.SelectedChoiceActionItem.Id.Equals("Character"))
                {
                    //Tỉ suất = Số lượng Dịch / Số lượng Nội dung(Nếu chọn menu Số kí tự / Số từ)
                    if (!string.IsNullOrEmpty(audio.Subtitle) && !string.IsNullOrEmpty(audio.Content))
                    {
                        audio.TextRate = System.Convert.ToDecimal(audio.Subtitle.Length) / audio.Content.Length;
                        if (!string.IsNullOrEmpty(audio.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                        }
                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Word"))
                {
                    //Tỉ suất = Số lượng Dịch / Số lượng Nội dung(Nếu chọn menu Số kí tự / Số từ)
                    if (!string.IsNullOrEmpty(audio.Subtitle) && !string.IsNullOrEmpty(audio.Content))
                    {
                        audio.TextRate = System.Convert.ToDecimal(audio.Subtitle.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length) / audio.Content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                        if (!string.IsNullOrEmpty(audio.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                        }
                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Syllable"))
                {
                    //Tỉ suất = Số lượng Phiên âm / Số lượng Nội dung (Nếu chọn menu Âm tiết)
                    if (!string.IsNullOrEmpty(audio.Spelling) && !string.IsNullOrEmpty(audio.Content))
                    {
                        var spellingTextRate = Module.Helpers.TextHelper.GetWordVowelWeight(languageTranslateCode, audio.Spelling);
                        var contentTextRate = Module.Helpers.TextHelper.GetWordVowelWeight(languageOriginCode, audio.Content);
                        audio.TextRate = spellingTextRate / contentTextRate;
                        if (!string.IsNullOrEmpty(audio.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, charTag, false);
                        }
                        audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    }
                }
            }

            #endregion TextRateImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0889            Oid: 7f690346-68a3-4d0b-92b9-495f68fd6cd7
		private void ElementTextReplace_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ElementTextReplace), "Thay thế");              
      
            #region ElementTextReplaceImportCode
            string columnName = "Content";
            var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
            if (focusedColumnMemberName != null && focusedColumnMemberName is string)
            {
                columnName = (string)focusedColumnMemberName;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("CharacterError"))
            {
                int total = 0;
                var characterErrorReplaceText = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "CharacterErrorReplaceText", "Ð|Ɖ|Ɩ|ɡ|ʺ|;|Α|Β|Ε|Ζ|Η|Ι|Κ|Μ|Ν|Ο|Τ|Υ|Χ|ν|ο|ό|ϲ|Ϲ|Ѕ|І|Ј|А|В|Е|М|Н|О|Р|С|Т|Х|а|е|о|р|с|у|х|ѐ|ѕ|і|һ|Ӏ|ӏ|ԁ|Ԛ|ԛ|Ԝ|ԝ|Տ|Օ|հ|ո|ց|օ|։|׃|ᴄ|ᴏ|ᴠ|‒|–|т|и\r\nĐ|Đ|l|g|\"|;|A|B|E|Z|H|I|K|M|N|O|T|Y|X|v|o|ó|c|C|S|I|J|A|B|E|M|H|O|P|C|T|X|a|e|o|p|c|y|x|è|s|i|h|l|l|d|Q|q|W|w|S|O|h|n|g|o|:|:|c|o|v|-|-|m|u").Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (characterErrorReplaceText.Length != 2)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Tham số CharacterErrorReplaceText không đủ điều kiện để thực hiện chức năng này", InformationType.Error);
                }
                var findWords = characterErrorReplaceText[0].Split('|');
                var replaceWords = characterErrorReplaceText[1].Split('|');
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {
                    bool replaced = false;
                    var columnValue = audio.GetPropertyValue(columnName);
                    if (columnValue != null)
                    {
                        if (!(columnValue is string))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Cột được chọn không phải là văn bản", InformationType.Error);
                            return;
                        }
                        var content = audio.GetPropertyValue(columnName) as string;
                        string newContent = content;
                        for (int i = 0; i < findWords.Length; i++)
                            newContent = newContent.Replace(findWords[i], replaceWords[i]);
                        if (content != newContent)
                        {
                            replaced = true;
                            Module.Helpers.ReflectionHelper.SetPropertyValueInObject(audio, columnName, newContent);
                        }
                    }
                    if (replaced)
                        total++;
                }
                if (total > 0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", total + "/" + View.SelectedObjects.Count + " dòng được thay thế từ", InformationType.Info);
                }
                else
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không tìm thấy từ cần thay thế", InformationType.Info);
                }
                return;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("DeleteSpeakerName") || e.SelectedChoiceActionItem.Id.Equals("KeepOnlySpeakerName"))
            {
                int total = 0;
                foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                {

                    var columnValue = audio.GetPropertyValue(columnName);
                    if (columnValue != null)
                    {
                        if (!(columnValue is string))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Cột được chọn không phải là văn bản", InformationType.Error);
                            return;
                        }
                        var content = audio.GetPropertyValue(columnName) as string;
                        if (!string.IsNullOrEmpty(content))
                        {
                            // Regex: bắt đầu chuỗi (^) -> các chữ in hoa và khoảng trắng `([A-Z\s]+)` -> dấu `:` -> khoảng trắng tùy chọn `\s*`
                            var match = System.Text.RegularExpressions.Regex.Match(content, @"^([\p{Lu}0-9\s]+):\s*");

                            if (match.Success)
                            {
                                var newContent = content.Substring(match.Length);
                                // Nếu phần trước dấu `:` chỉ toàn là chữ hoa và khoảng trắng, thì xóa đoạn đó
                                if (e.SelectedChoiceActionItem.Id.Equals("DeleteSpeakerName"))
                                {
                                    // Xóa phần người nói
                                    newContent = content.Substring(match.Length);
                                }
                                else if (e.SelectedChoiceActionItem.Id.Equals("KeepOnlySpeakerName"))
                                {
                                    // Giữ lại phần người nói, xóa phần sau dấu `:` (bao gồm cả dấu `:` và các dấu space)
                                    var match2 = System.Text.RegularExpressions.Regex.Match(content, @"^([\p{Lu}0-9\s]+):\s*");
                                    if (match2.Success)
                                    {
                                        newContent = content.Substring(0, match2.Groups[1].Length).TrimEnd();
                                    }
                                    else
                                    {
                                        newContent = content.Substring(0, match.Length); // không match thì giữ nguyên
                                    }
                                }

                                if (newContent != content)
                                {
                                    Module.Helpers.ReflectionHelper.SetPropertyValueInObject(audio, columnName, newContent);
                                    total++;
                                }
                            }
                        }
                    }
                }
                if (total > 0 && e.SelectedChoiceActionItem.Id.Equals("DeleteSpeakerName"))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", total + "/" + View.SelectedObjects.Count + " dòng được xóa người nói", InformationType.Info);
                }
                if (total > 0 && e.SelectedChoiceActionItem.Id.Equals("KeepOnlySpeakerName"))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", total + "/" + View.SelectedObjects.Count + " dòng được giữ tên người nói", InformationType.Info);
                }
                return;
            }

            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.AcceptAction.Caption = "Thay từ";
            dc.SaveOnAccept = true;
            var showViewParameters = new ShowViewParameters
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                NewWindowTarget = NewWindowTarget.Separate
            };
            dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
            {
                if (args.AcceptActionArgs.CurrentObject is Module.SystemObjects.ReplaceObject)
                {
                    var popupControl = (Module.SystemObjects.ReplaceObject)args.AcceptActionArgs.CurrentObject;
                    if (string.IsNullOrEmpty(popupControl.Find))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Tìm không được phép trống", InformationType.Error);
                        return;
                    }
                    if (popupControl.Find.Equals(popupControl.Replace))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Tìm và thay thế không được giống nhau", InformationType.Error);
                        return;
                    }
                    var findList = popupControl.Find.Split('|');
                    var replaceList = string.IsNullOrEmpty(popupControl.Replace) ? System.Linq.Enumerable.Repeat("", findList.Length).ToArray() : popupControl.Replace.Split('|');
                    if (!findList.Length.Equals(replaceList.Length))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Danh sách từ tìm và thay thế không giống nhau", $"Tìm: {findList.Length} \r\nThay thế: {replaceList.Length}", InformationType.Error);
                        return;
                    }

                    int total = 0;
                    System.StringComparison stringComparison = e.SelectedChoiceActionItem.Id.Contains("NoCase") ? System.StringComparison.OrdinalIgnoreCase : System.StringComparison.Ordinal;
                    bool nonUnicode = e.SelectedChoiceActionItem.Id.Contains("NoMark");
                    foreach (Module.BusinessObjects.Audio audio in View.SelectedObjects)
                    {
                        bool replaced = false;
                        if (e.SelectedChoiceActionItem.Id.StartsWith("String"))
                        {
                            var columnValue = audio.GetPropertyValue(columnName);
                            if (columnValue != null)
                            {
                                if (!(columnValue is string))
                                {
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Cột được chọn không phải là văn bản", InformationType.Error);
                                    return;
                                }
                                var content = audio.GetPropertyValue(columnName) as string;
                                string newContent = content;
                                if (e.SelectedChoiceActionItem.Id == "StringCaseMark")
                                {
                                    //Phân biệt hoa thường
                                    for (int i = 0; i < findList.Length; i++)
                                        newContent = newContent.Replace(findList[i], replaceList[i]);
                                }
                                else if (e.SelectedChoiceActionItem.Id == "StringCaseNoMark")
                                {
                                    //Phân biệt hoa thường nhưng không phân biệt dấu
                                    for (int i = 0; i < findList.Length; i++)
                                        newContent = Module.Helpers.TextHelper.FindAndReplaceWordNonUnicode(newContent, findList[i], replaceList[i]);
                                }
                                else if (e.SelectedChoiceActionItem.Id == "StringNoCaseMark")
                                {
                                    //Không phân biệt hoa thường nhưng phân biệt dấu
                                    for (int i = 0; i < findList.Length; i++)
                                        newContent = newContent.Replace(findList[i], replaceList[i], stringComparison);
                                }
                                else if (e.SelectedChoiceActionItem.Id == "StringNoCaseNoMark")
                                {
                                    //Không phân biệt hoa thường và không phân biệt dấu
                                    for (int i = 0; i < findList.Length; i++)
                                        newContent = Module.Helpers.TextHelper.FindAndReplaceWordNonUnicode(newContent, findList[i], replaceList[i], stringComparison);
                                }
                                else
                                {
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Lựa chọn không khả dụng", InformationType.Error);
                                    return;
                                }
                                if (content != newContent)
                                {
                                    replaced = true;
                                    Module.Helpers.ReflectionHelper.SetPropertyValueInObject(audio, columnName, newContent);
                                }
                            }
                        }
                        else
                        {

                            //Mặc định thay từ theo Word
                            if (columnName == "Content")
                            {
                                //Thay thế nội dung
                                if (!string.IsNullOrEmpty(audio.Content))
                                {
                                    var result = audio.Content;
                                    for (int i = 0; i < findList.Length; i++)
                                        result = Module.Helpers.TextHelper.ReplaceWordInContent(result, findList[i], replaceList[i], null, null, stringComparison, nonUnicode);
                                    if (!audio.Content.Equals(result))
                                    {
                                        replaced = true;
                                        audio.Content = result;
                                        //Cập nhật trường Vị trí của Thuật vị trong CN Thay thế (Thành phần): những từ đứng sau thuật ngữ  khi số từ của thuật ngữ thay đổi                                    
                                        for (int i = 0; i < findList.Length; i++)
                                        {
                                            if (findList[i].Split(' ').Length != replaceList[i].Split(' ').Length)
                                            {
                                                //Tìm thuật ngữ của từ tìm thấy
                                                var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Term.Video.Oid = ? and Element = ?", audio.Video.Oid, audio.Start);
                                                System.Collections.Generic.IList<DevExpress.Xpo.SortProperty> sorting = new System.Collections.Generic.List<DevExpress.Xpo.SortProperty>();
                                                sorting.Add(new DevExpress.Xpo.SortProperty("Location", DevExpress.Xpo.DB.SortingDirection.Ascending));
                                                var currentTermLocationList = View.ObjectSpace.GetObjects<Module.BusinessObjects.TermLocation>((DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                                                                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Term.Name = ? ", findList[i]))), sorting, true);
                                                Module.BusinessObjects.TermLocation currentTermLocation = currentTermLocationList.Count() > 0 ? currentTermLocationList[0] : null;

                                                if (currentTermLocation is null)
                                                {
                                                    var containTermLocationList = View.ObjectSpace.GetObjects<Module.BusinessObjects.TermLocation>
                                                            (DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                                                            DevExpress.Data.Filtering.CriteriaOperator.Parse("StartsWith([Term.Name], ?) Or EndsWith([Term.Name], ?) Or Contains([Term.Name], ?)", findList[i] + " ", " " + findList[i], " " + findList[i] + " ")));
                                                    if (containTermLocationList.Count > 0)
                                                        currentTermLocation = containTermLocationList[0];
                                                }
                                                //if(currentTermLocation != null && currentTermLocation.Location != null)
                                                //{
                                                //    criteria = DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                                                //        DevExpress.Data.Filtering.CriteriaOperator.Parse("Location >= ? and Term <> ? ", currentTermLocation.Location, currentTermLocation.Term));

                                                //    foreach (var termLocation in currentTermLocation.Term.TermLocationList)
                                                //    {
                                                //        //Vì thuật vị bị thay thế nên từ tìm thấy bị đổi
                                                //        if(termLocation.Element != null && termLocation.Element.Equals(audio.Start) && termLocation.Location != null && termLocation.Location > currentTermLocation.Location)
                                                //        {

                                                //        }
                                                //    }
                                                //}
                                                var termLocations = View.ObjectSpace.GetObjects<Module.BusinessObjects.TermLocation>(criteria, sorting, true).ToList();
                                                foreach (var termLocation in termLocations)
                                                {
                                                    if (termLocation.Term != null && termLocation.Term.Oid != currentTermLocation.Term.Oid)
                                                        TermLocationService.UpdatePositionLocation(termLocation, false);
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            else if (columnName == "Subtitle")
                            //if (e.SelectedChoiceActionItem.Id.Contains("Translate") || e.SelectedChoiceActionItem.Id.Contains("All"))
                            {
                                //Thay thế phụ đề
                                if (!string.IsNullOrEmpty(audio.Subtitle))
                                {
                                    var result = audio.Subtitle;
                                    for (int i = 0; i < findList.Length; i++)
                                        result = Module.Helpers.TextHelper.ReplaceWordInContent(result, findList[i], replaceList[i], null, null, stringComparison, nonUnicode);
                                    if (!audio.Subtitle.Equals(result))
                                    {
                                        replaced = true;
                                        audio.Subtitle = result;
                                    }
                                }
                            }
                            else if (columnName == "Spelling")
                            //if (e.SelectedChoiceActionItem.Id.Contains("Spelling") || e.SelectedChoiceActionItem.Id.Contains("All"))
                            {
                                //Thay thế nội dung
                                if (!string.IsNullOrEmpty(audio.Spelling))
                                {
                                    var result = audio.Spelling;
                                    for (int i = 0; i < findList.Length; i++)
                                        result = Module.Helpers.TextHelper.ReplaceWordInContent(result, findList[i], replaceList[i], null, null, stringComparison, nonUnicode);
                                    if (!audio.Spelling.Equals(result))
                                    {
                                        replaced = true;
                                        audio.Spelling = result;
                                    }
                                }
                            }
                        }

                        if (replaced)
                            total++;
                    }
                    if (total > 0)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", total + "/" + View.SelectedObjects.Count + " dòng được thay thế từ", InformationType.Info);
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không tìm thấy từ cần thay thế", InformationType.Info);
                    }
                }
            };
            dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
            {
                if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                    ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                {
                    ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                }
            };
            showViewParameters.Controllers.Add(dc);
            Module.SystemObjects.ReplaceObject replaceObject = new Module.SystemObjects.ReplaceObject();
            showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), replaceObject, true);
            showViewParameters.Context = TemplateContext.PopupWindow;

            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));


            #endregion ElementTextReplaceImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0547            Oid: 2c5d63d8-fb5c-47fd-9f27-aaaa210a83f3
		private void AlignElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AlignElement), "Dịch chuyển");              
      
            #region AlignElementImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            var currentAudio = View.CurrentObject as Module.BusinessObjects.Audio;
            if (video != null && (video.AudioList != null && video.AudioList.Count > 0) && currentAudio != null && currentAudio.Start != null)
            {
                if (e.SelectedChoiceActionItem.Id.ToLower().Contains("begin"))
                {
                    foreach (var audio in video.AudioList.OrderBy(m => m.Start))
                    {
                        if (currentAudio.Start != null && currentAudio.End != null
                            && audio.Start != null && audio.Start > currentAudio.Start)
                        {
                            currentAudio.Start += audio.Start - currentAudio.End;
                            currentAudio.End = audio.Start;
                            Tools.RefreshGridView(View);
                            break;
                        }

                    }
                }
                else if (e.SelectedChoiceActionItem.Id.ToLower().Contains("end"))
                {
                    foreach (var audio in video.AudioList.OrderByDescending(m => m.Start))
                    {
                        if (currentAudio.Start != null && audio.End != null
                            && audio.Start != null && audio.Start < currentAudio.Start)
                        {
                            currentAudio.End += audio.End - currentAudio.Start;
                            currentAudio.Start = audio.End;
                            Tools.RefreshGridView(View);
                            break;
                        }

                    }
                }
                else
                {
                    //Trường hợp đồng bộ âm
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chức năng này không khả dụng", InformationType.Error);
                }
            }

            #endregion AlignElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0911            Oid: 2a5a382a-925c-4f4f-a329-10222ae583ac
		private void TimestampByDuration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TimestampByDuration), "Chỉnh theo âm");              
      
            #region TimestampByDurationImportCode
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chức năng này không khả dụng", InformationType.Error);

            #endregion TimestampByDurationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}