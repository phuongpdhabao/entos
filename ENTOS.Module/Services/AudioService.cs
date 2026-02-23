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

    public partial class AudioService : BaseService
    {

        public AudioService() : base()
        {
        }
        #region DependencyInjection
  
     
        private VideoService videoService;  
        protected VideoService _videoService => videoService ??= new VideoService(ViewController);        
       
  
        #endregion DependencyInjection

        public AudioService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3374ImportCode
                public static void RearrangeElement(
            List<Audio> list1,
            List<(List<int?>, bool)> mapping,
            DevExpress.ExpressApp.XafApplication application)
        {
            // Sắp xếp list1 theo thời gian bắt đầu
            list1.Sort((a, b) => a.Start.Value.CompareTo(b.Start.Value));

            if (mapping.Count > list1.Count)
                return;

            // Lưu lại bản sao giá trị gốc
            var originalValues = list1
                .Select(audio => new
                {
                    audio.Order,
                    audio.Subtitle,
                    audio.Spelling
                })
                .ToList();

            // XÓA dữ liệu cũ
            foreach (var audio in list1)
            {
                audio.Order = null;
                audio.Subtitle = null;
                audio.Spelling = null;
                audio.Flag = false; // Reset Flag
            }

            // GÁN lại dữ liệu theo ánh xạ nhiều-đến-một
            for (int toIndex = 0; toIndex < mapping.Count; toIndex++)
            {
                var (fromIndexes, isWeaker) = mapping[toIndex];
                if (fromIndexes == null || fromIndexes.Count == 0)
                    continue;

                if (toIndex < 0 || toIndex >= list1.Count)
                    continue;

                var subtitles = new List<string>();
                var spellings = new List<string>();
                int? lastOrder = null;

                foreach (var fromNullable in fromIndexes)
                {
                    if (!fromNullable.HasValue)
                        continue;

                    int fromIndex = fromNullable.Value;
                    if (fromIndex < 0 || fromIndex >= originalValues.Count)
                        continue;

                    var source = originalValues[fromIndex];

                    if (!string.IsNullOrEmpty(source.Subtitle))
                        subtitles.Add(source.Subtitle);
                    if (!string.IsNullOrEmpty(source.Spelling))
                        spellings.Add(source.Spelling);

                    lastOrder = source.Order; // lấy Order cuối cùng
                }

                var target = list1[toIndex];
                target.Subtitle = subtitles.Count > 0 ? string.Join(" ", subtitles) : null;
                target.Spelling = spellings.Count > 0 ? string.Join(" ", spellings) : null;
                target.Order = lastOrder;

                if (isWeaker)
                {
                    target.Flag = true;
                }
            }
        }


        #endregion SourceCode3374ImportCode

        #region SourceCode3349ImportCode
                public static List<(List<int?>, bool)> SemanticMatchLine(
            List<string> list1,
            List<(string Content, bool Flag)> list2,
            DataService dataService)
        {
            // Khởi tạo kết quả: mỗi phần tử là (danh sách index đã khớp, bool isWeaker)
            var matchResult = new List<(List<int?>, bool)>();
            for (int i = 0; i < list1.Count; i++)
                matchResult.Add((new List<int?>(), false));

            int m = 3;
            int currentIndex = list1.Count;

            for (int i = list2.Count - 1; i >= 0; i--)
            {
                var item = list2[i];

                if (item.Flag)
                {
                    int toIndex = list2.IndexOf(item);
                    if (toIndex >= 0 && toIndex < matchResult.Count)
                    {
                        matchResult[toIndex].Item1.Add(i);
                        // Không cần đánh dấu IsWeaker vì dòng này là gán cứng
                        currentIndex = toIndex;
                    }
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.Content))
                    continue;

                var (matchedIndex, isWeaker) = BestMatchSemantic(dataService, list1, item.Content, currentIndex, m, i);

                if (matchedIndex.HasValue)
                {
                    int toIndex = matchedIndex.Value;
                    if (toIndex >= 0 && toIndex < matchResult.Count)
                    {
                        matchResult[toIndex].Item1.Add(i);

                        // Nếu có ít nhất một dòng khớp mà isWeaker = true → đánh dấu
                        if (isWeaker)
                            matchResult[toIndex] = (matchResult[toIndex].Item1, true);

                        currentIndex = toIndex;
                    }
                }
            }

            return matchResult;
        }


        #endregion SourceCode3349ImportCode

        #region SourceCode3340ImportCode
                public static bool ShiftSubtitle(
          int lineCount,
          bool up,
          List<Audio> selectedAudios,
          DevExpress.ExpressApp.XafApplication app)
        {
            if (selectedAudios == null || !selectedAudios.Any())
                return false;

            var elementBatch = selectedAudios[0].ElementBatch;

            if (!selectedAudios.All(a => a.ElementBatch == elementBatch))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(app, "Lỗi", "Các thành phần được chọn phải thuộc cùng một batch", InformationType.Error);
                return false;
            }

            selectedAudios = selectedAudios.OrderBy(a => a.Start).ToList();

            var allAudios = elementBatch.AudioList.OrderBy(a => a.Start).ToList();

            int firstIdx = allAudios.FindIndex(a => a == selectedAudios.First());
            int lastIdx = allAudios.FindIndex(a => a == selectedAudios.Last());

            // Tính toán hướng dịch
            int startIdx = up ? firstIdx : lastIdx;
            int maxIdx = allAudios.Count - 1;

            // Kiểm tra giới hạn
            if ((up && startIdx - lineCount < 0) || (!up && startIdx + lineCount > maxIdx))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(app, "Lỗi", "Bước nhảy không hợp lệ", InformationType.Error);
                return false;
            }


            for (int j = 1; j <= lineCount; j++)
            {
                int targetIdx = up ? firstIdx - j : lastIdx + j;
                var check = allAudios[targetIdx];
                if (!string.IsNullOrEmpty(check.Subtitle) || !string.IsNullOrEmpty(check.Spelling))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(app, "Lỗi",
                        $"Dòng {(up ? "phía trên" : "phía dưới")} thứ {j} của đoạn đã có dữ liệu",
                        InformationType.Error);
                    return false;
                }

            }

            if (up)
            {
                for (int i = 0; i < selectedAudios.Count; i++)
                {
                    int idx = firstIdx + i;

                    var target = allAudios[idx - lineCount];
                    var source = allAudios[idx];

                    target.Subtitle = source.Subtitle;
                    target.Spelling = source.Spelling;
                    target.Order = source.Order;

                    source.Subtitle = null;
                    source.Spelling = null;
                    source.Order = null;

                }
            }
            else
            {
                for (int i = selectedAudios.Count - 1; i >= 0; i--)
                {
                    int idx = lastIdx - (selectedAudios.Count - 1 - i);


                    var target = allAudios[idx + lineCount];
                    var source = allAudios[idx];

                    target.Subtitle = source.Subtitle;
                    target.Spelling = source.Spelling;
                    target.Order = source.Order;

                    source.Subtitle = null;
                    source.Spelling = null;
                    source.Order = null;

                }
            }

            return true;
        }

        public static void MatchLineUnTranslate(List<Audio> selectedAudios, DevExpress.ExpressApp.XafApplication app)
        {
            var elementBatch = selectedAudios[0].ElementBatch;
            if (!selectedAudios.All(a => a.ElementBatch == elementBatch))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(app, "Lỗi", "Các thành phần được chọn phải thuộc cùng một batch", InformationType.Error);
                return;
            }

            Video video = selectedAudios[0].Video;

            var shiftList = new System.Collections.Generic.List<Audio>();
            var firstAudio = selectedAudios.Last(a => !string.IsNullOrEmpty(a.Subtitle));
            var firstIndex = selectedAudios.IndexOf(firstAudio);
            List<Audio> remainingAudio = elementBatch.AudioList.OrderBy(a => a.Start).ToList();

            bool shift = true;

            for (int i = firstIndex; i >= 0; i--)
            {
                Audio audio = selectedAudios[i];

                if (audio.TermLocationList.Count == 0)
                {
                    shiftList.Add(audio);
                    continue;
                }
                else
                {
                    var validTypes = new[] { TermType.UpperCase, TermType.Number, TermType.UpperCaseAll, TermType.Short };
                    var termLocations = audio.TermLocationList
                        .Where(x => x.Term.Language == video.LanguageTranslate &&
                                    validTypes.Contains(x.Term.TermType))
                        .ToList();


                    if (termLocations.Count > 0)
                    {
                        foreach (var termLocation in termLocations)
                        {
                            if (string.IsNullOrEmpty(audio.Subtitle))
                            {
                                continue;
                            }
                            if (!audio.Subtitle.Contains(termLocation.Term.Name))
                            {
                                if (!shiftList.Contains(audio))
                                    shiftList.Add(audio); 
                                continue;
                            }
                            else
                            {
                                int difference = FindCorrectAudioIndex(audio, termLocation.Term, remainingAudio);

                                if (difference == 999)
                                {
                                    if (!shiftList.Contains(audio))
                                        shiftList.Add(audio);

                                    continue;
                                }
                                else
                                {
                                    int lineCount = System.Math.Abs(difference);
                                    bool up = difference < 0;
                                    if (!shiftList.Contains(audio))
                                        shiftList.Add(audio);

                                    if (lineCount > 0)
                                    {
                                        shift = ShiftSubtitle(lineCount, up, shiftList, app);

                                        if (!shift)
                                        {
                                            return;
                                        }

                                        if (!up)
                                        {

                                            int index = remainingAudio.IndexOf(audio) + lineCount;
                                            remainingAudio[index].Flag = true;

                                            remainingAudio.RemoveRange(index, remainingAudio.Count - index);
                                        }


                                        shiftList.Clear();
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        shiftList.Add(audio);
                        continue;
                    }

                }
            }
        }
        public static int FindCorrectAudioIndex(Audio audio, Term term, List<Audio> remainingAudio)
        {
            var elementBatch = audio.ElementBatch;

            var index = remainingAudio.FindIndex(a => a == audio);

            for (int i = remainingAudio.Count - 1; i >= 0; i--)
            {
                var currentAudio = remainingAudio[i];
                if (Module.Helpers.TextHelper.NormalizeString(currentAudio.Content).Contains(Module.Helpers.TextHelper.NormalizeString(term.Name)))
                {
                    return i - index;
                }

            }

            return 999;
        }

        #endregion SourceCode3340ImportCode

        #region SourceCode4519ImportCode
                internal async System.Threading.Tasks.Task<string> GetSpellCorrectionAsync(string textToCorrect, string languageCode, string apiKey)
        {
            string serpApiUrl = "https://serpapi.com/search.json";
            using (var client = new System.Net.Http.HttpClient())
            {
                var queryParams = new System.Collections.Generic.Dictionary<string, string>
        {
            { "q", textToCorrect },
            { "hl", languageCode },
            { "gl", "vi" },
            { "api_key", apiKey }
        };

                string queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={System.Net.WebUtility.UrlEncode(kvp.Value)}"));

                var response = await client.GetAsync($"{serpApiUrl}?{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Phân tích dữ liệu JSON
                    Newtonsoft.Json.Linq.JObject jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(responseData);
                    return jsonResponse["search_information"]?["spelling_fix"]?.ToString() ?? ".";
                }
                else
                {
                    System.Console.WriteLine("Error: " + response.StatusCode);
                    return ".";
                }
            }
        }


        internal DataService GetDataService(ViewController viewController)
        {
            DataService _dataService = null;
            if (_dataService is null)
            {
                using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            viewController.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                {
                    dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                    {
                        _dataService = (DataService)args?.AcceptActionArgs?.CurrentObject;
                    };
                    var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("SoftwareServiceType.Code = 'Translate'");
                    Module.Helpers.XafXpoHelper.PopupDialogControllerListView(viewController, dc, typeof(DataService), viewController.View.ObjectSpace, "BookmarkImport", criteria, false, null, false, true);
                }
            }
            return _dataService;
        }
                public void ExportTrainToFile(string saveFolder, List<AudioProcessingItem> audioItems, string trainFormat)
        {
            try
            {
                var parts = trainFormat?.Split('/');
                var wavsFolder = (parts != null && parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0])) ? parts[0] : "wavs";
                if (!System.IO.Directory.Exists(saveFolder))
                    System.IO.Directory.CreateDirectory(saveFolder);
                var wavFolder = System.IO.Path.Combine(saveFolder, wavsFolder);
                if (!System.IO.Directory.Exists(wavFolder))
                    System.IO.Directory.CreateDirectory(wavFolder);
                if (audioItems.Count == 0)
                    return;
                //System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                Tools.ShowOrCloseWaitFormWithCancelButton();
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                int index = 0, total = audioItems.Count;
                stopWatch.Start();
                Tools.ShowOrCloseDefaultWaitForm("Đang xử lý", null, stopWatch.Elapsed, true);
                //var trainFormat = Module.Helpers.ParameterHelper.GetValueOrDefault(audios[0].Session, "TrainFormat", wavsFolder + "/{0}|{1}");


                //object locker = new object();
                //Dữ liệu train không cần đúng thứ tự quá, có thể bỏ qua
                System.Collections.Concurrent.ConcurrentQueue<string> trainLines = new System.Collections.Concurrent.ConcurrentQueue<string>();
                //Xử lý đa luồng để tăng tốc
                Parallel.ForEach(audioItems, (item) =>
                {
                    ExportTrainToFile(wavFolder, trainFormat, item, trainLines, total, stopWatch);
                });
                //foreach (var audio in audios)
                //{
                //    if (audio.Start is null || audio.FileData == null || audio.FileData.IsEmpty)
                //        continue;
                //    var fileInfo = new System.IO.FileInfo(audio.FileData.FileName);
                //    string fileName = $"{audio.Start.Value.ToString(@"dd\.hhmmss")}{fileInfo.Extension}";
                //    string filePath = System.IO.Path.Combine(wavFolder, fileName);
                //    using (System.IO.FileStream streamWriter = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                //    {
                //        streamWriter.Write(audio.FileData.Content, 0, audio.FileData.Content.Length);
                //    }
                //    string extension = ".wav";
                //    if (!fileInfo.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                //    {
                //        //Convert sang định dang wav 16kHz 16bit mono
                //        var wavFile = $"{audio.Start.Value.ToString(@"dd\.hhmmss")}{extension}";
                //        var wavFilePath = System.IO.Path.Combine(wavFolder, wavFile);
                //        if (System.IO.File.Exists(wavFile))
                //            System.IO.File.Delete(wavFile);
                //        string convertArguments = $"-i \"{filePath}\" -acodec pcm_s16le -ar 22050 \"{wavFilePath}\"";
                //        Module.Helpers.ProcessHelper.RunProcessOutside("ffmpeg", convertArguments);
                //        if (System.IO.File.Exists(wavFilePath))
                //        {
                //            fileName = wavFile;
                //            System.IO.File.Delete(filePath);
                //        }
                //    }
                //    stringBuilder.AppendLine(string.Format(trainFormat, fileName, audio.Content));
                //    if (Tools.DefaultSplashScreenManager is null)
                //        continue;
                //    index++;
                //    Tools.ShowOrCloseDefaultWaitForm(null, $"{index.ToString("D")}/{total.ToString("D")}", stopWatch.Elapsed, true);
                //}
                //using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(System.IO.Path.Combine(saveFolder, "train.txt"), false, System.Text.Encoding.UTF8))
                //{
                //    streamWriter.Write(stringBuilder.ToString());                                 
                //}

                //string fullText = string.Join(Environment.NewLine, trainLines);

                //File.WriteAllText(System.IO.Path.Combine(saveFolder, "train.txt"), fullText, System.Text.Encoding.UTF8);
                WriteToTrainFile(trainLines.ToList(), saveFolder);
                stopWatch.Stop();
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {

                Tools.ShowOrCloseDefaultWaitForm(null, null);
            }
            //object locker = new object();

            //TimeSpan timeout = TimeSpan.FromSeconds(30);
            //try
            //{
            //    if (Monitor.TryEnter(locker, timeout))
            //    {
            //        // xử lý an toàn trong lock
            //        //stringBuilder.AppendLine(...);
            //    }
            //    else
            //    {
            //        // Không lấy được lock sau 3 giây -> có thể đang bị treo
            //        Console.WriteLine("Cảnh báo: Không thể lấy lock, có thể bị treo.");
            //        // Ghi log hoặc gửi cảnh báo
            //    }
            //}
            //finally
            //{
            //    Monitor.Exit(locker);
            //}
        }
        #endregion SourceCode4519ImportCode

        #region SourceCode3279ImportCode
                public Application.DTOs.AudioDto CreateDtoFromObject(Audio source)
        {
            if (source == null) return null;
            var audioDto = new Application.DTOs.AudioDto
            {
                Start = source.Start,
                End = source.End,
                Content = source.Content,
            };
            return audioDto;
        }

        #endregion SourceCode3279ImportCode

        

        #region SourceCode4546ImportCode
        
        public static bool ElementFlagUpperCase(Video video, string audioContent, string column = "Content", bool upperCaseMany = false)
        //Nếu upperCaseMany = null là chỉ xác định chữ cái đầu
        //Nếu upperCaseMany = true xác định nhiều ký tự hoa hơn hay ký tự thường hơn
        //Nếu upperCaseMany = false kiểm tra có viết hoa chữ cái đầu không
        {
            //string upperCaseEnglishAcceptWords = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "UpperCaseEnglishAcceptWords", "and, or, of, via, in, with, to, for");
            //upperCaseEnglishAcceptWords = upperCaseEnglishAcceptWords.Replace(" ", "");
            //var upperCaseEnglishAcceptWordsArray = upperCaseEnglishAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
            //string upperCaseVietnameseAcceptWords = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "UpperCaseVietnameseAcceptWords", "và, hoặc, của, qua, trong, với, tới, cho");
            //upperCaseVietnameseAcceptWords = upperCaseVietnameseAcceptWords.Replace(" ", "");
            //var upperCaseVietnameseAcceptWordsArray = upperCaseVietnameseAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
            //2023-08-21: Có viết hoa > Hoa đầu mỗi từ : Hoa Đầu Mỗi Từ (Có thể tồn tại từ viết tắt).
            //Tuy nhiên chấp nhận các từ chữ thường trong danh sách: 
            //-and, or, of, via, in, with, to, for (bổ sung dần)
            //    -và, hoặc, của, qua, trong, với, tới, cho
            //2023-08-21: Hoa đầu mỗi từ không được chứa viết hoa toàn bộ
            if (audioContent.Equals(audioContent.ToUpper()))
                return false;
            var upperCaseAcceptWordsArray = video.GetUpperCaseAcceptWords( column == "Content");
            //Có viết hoa > em phải không được tính chữ hoa đầu câu => 2023-08-21: đổi cấu trúc
            var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            int upperCount = 0;
            int lowerCount = 0;
            bool upper = false;
            bool lower = false;
            foreach (var row in rows)
            {
                //Không coi là từ Hoa với các từ viết hoa đầu câu hoặc sau dâu: " ; ( ; { ; [ ; : (2 chấm)
                var childContents = row.Split(Module.Helpers.TextHelper.SeperateChars, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var childContent in childContents)
                {
                    var content = childContent.Trim();
                    //2023-08-02: Cờ thành phần > Có viết hoa: không được tính viết tắt
                    var words = content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length == 1 && childContents.Length == 1 && words.Length == 1)
                    {
                        //2025-02-19:  002 xác định Đầu hoa với thành phần chỉ có 1 từ > chỉ là Thường
                        return false;
                    }
                    //for (int i = 1; i < words.Length; i++)
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (char.IsUpper(words[i][0]))
                        {
                            //2023-10-09: 001 Hoa đầu mỗi từ cần chấp nhận các từ viết tắt, nhưng phân biệt với hoa toàn bộ ở chỗ có tồn tại chữ thường
                            if (words[i].Length > 1 && words[i].ToUpper().Equals(words[i]))
                            {
                                lower = true;
                                break;
                            }
                            upper = true;
                            upperCount++;
                        }
                        else if (char.IsLower(words[i][0]))
                        {
                            lowerCount++;
                            if (!upperCaseMany)
                            {
                                //Kiểm tra xem có chữ thường ko
                                bool hasLower = false;
                                foreach (var w in words[i])
                                {
                                    if (char.IsLower(w))
                                    {
                                        hasLower = true;
                                        break;
                                    }
                                }
                                if (hasLower)
                                {
                                    //2023-10-09: 001 Hoa đầu mỗi từ cần chấp nhận các từ viết tắt, nhưng phân biệt với hoa toàn bộ ở chỗ có tồn tại chữ thường
                                    var word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(words[i]);
                                    if (!string.IsNullOrEmpty(word))
                                    {
                                        //2023-08-21: Có viết hoa > Hoa đầu mỗi từ : Hoa Đầu Mỗi Từ (Có thể tồn tại từ viết tắt).
                                        //Tuy nhiên chấp nhận các từ chữ thường trong danh sách: 
                                        //-and, or, of, via, in, with, to, for (bổ sung dần)
                                        //    -và, hoặc, của, qua, trong, với, tới, cho
                                        //2025-06-04: các từ chấp nhận không được đứng đầu câu
                                        if (i > 0 && upperCaseAcceptWordsArray != null && upperCaseAcceptWordsArray.Contains(word))
                                            continue;
                                        lower = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    if (lower)
                        break;
                }
                if (lower)
                    break;
            }
            if (upperCaseMany)
            {
                return upperCount > lowerCount;
            }
            else if (upper && !lower)
            {
                return true;
            }
            return false;

        }

        public void UpdateTermLocationAfterMerge(Module.BusinessObjects.Audio audio, Module.BusinessObjects.Audio firstAudio)
        {
            var sentencesArray = Module.Helpers.TextHelper.GetSentences(firstAudio.Content);
            foreach (var termLocation in audio.TermLocationList)
            {
                //ghép các thuật vị sau thuật vị hiện tại                            
                if (EndContentIsBreakLine(firstAudio))
                {
                    termLocation.Sentence += sentencesArray.Length;
                }
                else
                {
                    if (termLocation.Sentence == 1)
                    {
                        termLocation.Location += sentencesArray[sentencesArray.Length - 1].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }
                    termLocation.Sentence += sentencesArray.Length - 1;
                }
                //Cờ thuật vị bị sai vị trí
                if (System.Diagnostics.Debugger.IsAttached)
                    termLocation.Flag = true;
            }
            //firstAudio.TermLocationList.AddRange(TermLocationList.ToList());
        }

        //[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
        //[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        //[DevExpress.Xpo.DisplayName("Thuật vị liên quan")]
        //public XPCollection<TVOS.Module.BusinessObjects.TermLocation> TermLocations
        //{
        //    get => new XPCollection<TVOS.Module.BusinessObjects.TermLocation>(Session, CriteriaOperator.Parse("Term.Video = ? and Element = ?", Video, Start));

        //}


        private System.Net.Http.HttpClient _client = null;
        public System.Net.Http.HttpClient GetHttpClient(string apiKey, string voiceCode, decimal speed = 1)
        {
            if (_client is null)
            {
                _client = new System.Net.Http.HttpClient();
                //phamphuong88@gmail.com Key
                //client.DefaultRequestHeaders.Add("api-key", "q69whAQkVa5x8JjsUkLHUozOdGAqTqWz");
                //phuongpd@habao.com.vn
                //client.DefaultRequestHeaders.Add("api-key", "JRMfjgsbluFQTMe9UJQX8s2ZaU6B8H34");
                //chiennh@habao.com.vn
                //client.DefaultRequestHeaders.Add("api-key", "nn97Zu89nM2xStDlrukCh3pueQeJJsIK");
                _client.DefaultRequestHeaders.Add("api-key", apiKey);
                //client.DefaultRequestHeaders.Add("speed", "");
                //client.DefaultRequestHeaders.Add("voice", "banmai");
            }
            else
            {
                _client.DefaultRequestHeaders.Remove("speed");
                _client.DefaultRequestHeaders.Remove("voice");
            }
            _client.DefaultRequestHeaders.Add("speed", speed.ToString("N1", new System.Globalization.CultureInfo("en-us")));
            _client.DefaultRequestHeaders.Add("voice", voiceCode);
            return _client;
        }

        public string GetUrlFrontContent(string apiKey, string voiceCode, string content, decimal speed = 1, int maxSecondWait = 300)
        {
            //String result = SendRequestAsync(audio.Content, cultureInfo).GetAwaiter().GetResult();
            String result = System.Threading.Tasks.Task.Run(async () =>
            {
                var response = await GetHttpClient(apiKey, voiceCode, speed).PostAsync("https://api.fpt.ai/hmi/tts/v5", new System.Net.Http.StringContent(content));
                return await response.Content.ReadAsStringAsync();
            }).GetAwaiter().GetResult();

            int count = 0;
            //Nếu quá 5 phút thì dừng lại
            while (string.IsNullOrEmpty(result) && count < maxSecondWait)
            {
                //Đợi 1s để tránh vượt qua giới hạn của API
                System.Threading.Thread.Sleep(1000);
                count++;
            }
            if (!string.IsNullOrEmpty(result))
            {
                var resultArray = result.Split(',');
                if (resultArray.Length >= 2 && resultArray[1] == "\"error\":0" && resultArray[0].IndexOf(':') > 0 && !resultArray[0].EndsWith(':'))
                {
                    //Kết quả: [0] = "{\"async\":\"https://file01.fpt.ai/text2speech-v5/long/2023-05-24/7f026219212fca2834473c42fc85d95e.mp3\""                   
                    return resultArray[0].Substring(resultArray[0].IndexOf(':') + 1).Replace("\"", "");
                }
            }
            //FptTextToSpeech fptTextToSpeech = Newtonsoft.Json.JsonConvert.DeserializeObject<FptTextToSpeech>(result);
            //int count = 0;
            ////Nếu quá 5 phút thì dừng lại
            //while (fptTextToSpeech.Error != null && fptTextToSpeech.Error.Equals("0") && count < maxSecondWait)
            //{
            //    //Đợi 1s để tránh vượt qua giới hạn của API
            //    System.Threading.Thread.Sleep(1000);
            //    count++;
            //    return fptTextToSpeech.Async;
            //}
            return null;
        }

        public bool DownloadFileFromUrl(Audio audio, int maxSecondWait = 300)
        {
            //Khi hiện url thì download về
            if (!string.IsNullOrEmpty(audio.URL))
            {
                if (audio.FileData is null)
                {
                    audio.FileData = new DevExpress.Persistent.BaseImpl.FileData(audio.Session);
                    if (!string.IsNullOrEmpty(audio.Content))
                    {
                        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
                        var audioName = new string(audio.Content.Where(m => !invalidChars.Contains(m)).ToArray<char>());
                        audioName = audio.Content.Length > 200 ? audio.Content.Substring(0, 200) : audio.Content;
                        audio.FileData.FileName = Module.Helpers.TextHelper.RemoveUnicode(audioName) + ".mp3";
                    }
                    else
                    {
                        audio.FileData.FileName = audio.Oid + ".mp3";
                    }

                }
                if (audio.FileData != null && audio.FileData.Content is null)
                {
                    int second = 1;
                    //Đợi không quá 5 phút
                    while (audio.FileData.Content is null && second < maxSecondWait)
                    {
                        using (System.Net.WebClient webClient = new System.Net.WebClient())
                        {
                            try
                            {
                                //Download lần 1, để chậm 1s để nó tránh lỗi hàng loại                                
                                audio.FileData.Content = webClient.DownloadData(audio.URL);
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        System.Threading.Thread.Sleep(second * 1000);
                        second = 2 * second;
                        //if (second > 60)
                        //{

                        //}
                    }
                    if (audio.FileData.Content != null)
                        return true;
                }
            }
            return false;
        }


        #endregion SourceCode4546ImportCode

        #region SourceCode3327ImportCode
        public static float CalculateWordSimilarity(string text1, string text2)
        {
            // Tách thành các từ bằng Regex \p{L}+ (ký tự chữ unicode)
            System.Collections.Generic.HashSet<string> words1 = new System.Collections.Generic.HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text1.ToLower(), @"\p{L}+")
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value)
            );

            System.Collections.Generic.HashSet<string> words2 = new System.Collections.Generic.HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text2.ToLower(), @"\p{L}+")
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value)
            );

            // Đếm số từ giống nhau
            int matchCount = words1.Intersect(words2).Count();
            if (words1.Count == 0 || words2.Count == 0)
                return 0;

            // Tính độ tương đồng dựa trên số từ giống nhau và tổng số từ nhiều hơn
            return (float)(matchCount / words1.Count + matchCount/words2.Count)/2;
        }


        #endregion SourceCode3327ImportCode

        #region SourceCode3281ImportCode
        //Thay đổi Nội dung dấu thành xuống dòng
public void SplitContentByNewLine(List<Audio> audios, string[] splitStrings, char[] splitChars)
{
    var selectedEndIsNullList = audios.Where(m => m.End is null).ToList();
    if (selectedEndIsNullList.Count > 0)
    {
        foreach (var audio in selectedEndIsNullList)
        {
            foreach (var splitString in splitStrings)
            {
                if (!string.IsNullOrEmpty(audio.Content))
                {
                    string content = Module.Helpers.TextHelper.SplitStringByNewLine(audio.Content, splitChars);
                    if (audio.Content != content)
                        audio.Content = content;
                }
                if (!string.IsNullOrEmpty(audio.Subtitle))
                {
                    string content = Module.Helpers.TextHelper.SplitStringByNewLine(audio.Subtitle, splitChars);
                    if (audio.Subtitle != content)
                        audio.Subtitle = content;
                }
                if (!string.IsNullOrEmpty(audio.Spelling))
                {
                    string content = Module.Helpers.TextHelper.SplitStringByNewLine(audio.Spelling, splitChars);
                    if (audio.Spelling != content)
                        audio.Spelling = content;
                }
            }
        }
    }
}
        #endregion SourceCode3281ImportCode

        #region SourceCode3372ImportCode
                
        private static Module.Services.DataServiceService dataServiceService1 = null;
        public static (int? Index, bool IsWeaker) BestMatchSemantic(
            DataService dataService,
            List<string> list1,
            string content,
            int currentIndex,
            int m,
            int itemIndex)
        {
            if (dataServiceService1 is null)
                dataServiceService1 = new Services.DataServiceService();

            var scoredCandidates = new List<(int Index, double SemanticScore, decimal WordScore)>();

            int start = Math.Max(0, currentIndex - m);
            int end = Math.Min(list1.Count - 1, currentIndex);

            for (int i = start; i <= end; i++)
            {
                var candidateContent = list1[i];
                if (string.IsNullOrWhiteSpace(candidateContent))
                    continue;

                double semanticScore = Task
                    .Run(() => dataServiceService1.GetSentenceSimilarityAsync(dataService, candidateContent, content))
                    .Result;

                float wordSim = Module.Helpers.TextHelper.CalculateWordSimilarity(candidateContent, content);

                decimal wordScore = (float.IsNaN(wordSim) || float.IsInfinity(wordSim) ||
                                    wordSim > (float)decimal.MaxValue || wordSim < (float)decimal.MinValue)
                    ? 0m
                    : Convert.ToDecimal(wordSim);

                scoredCandidates.Add((i, semanticScore, wordScore));
            }

            if (scoredCandidates.Count == 0)
                return (null, false);

            var validCandidates = scoredCandidates
                .Where(c => c.Index <= currentIndex)
                .ToList();

            if (validCandidates.Count == 0)
                return (null, false);

            validCandidates.Sort((a, b) => b.SemanticScore.CompareTo(a.SemanticScore));

            var top1 = validCandidates[0];
            var top2 = validCandidates.Count > 1 ? validCandidates[1] : top1;

            double semanticDiff = top1.SemanticScore - top2.SemanticScore;

            bool isWeaker = top1.WordScore < top2.WordScore;

            if (semanticDiff == 0)
            {
                return (Math.Max(top1.Index, top2.Index), isWeaker);
            }

            //if (semanticDiff <= 0.02 || top1.SemanticScore < 0.45)
            //{
            //    var best = validCandidates
            //        .Select(c => new
            //        {
            //            Index = c.Index,
            //            TotalScore = 0.5 * c.SemanticScore + 0.5 * (double)c.WordScore
            //        })
            //        .OrderByDescending(x => x.TotalScore)
            //        .ThenByDescending(x => x.Index)
            //        .First();

            //    return (best.Index, isWeaker);
            //}
            else
            {
                return (top1.Index, isWeaker);
            }
        }


        #endregion SourceCode3372ImportCode

        #region SourceCode3376ImportCode
                public static string RearrangeString(
            string input,
            List<(List<int?>, bool)> mapping,
            int lineCount,
            DevExpress.ExpressApp.XafApplication application)
        {
            // Cắt chuỗi thành các dòng
            var lines = input.Split('\n').ToList();

            // Bảo đảm đủ dòng tương ứng lineCount
            while (lines.Count < lineCount)
                lines.Add("");

            var result = Enumerable.Repeat(string.Empty, lineCount).ToList();

            // Gán nội dung theo mapping: toIndex => list of fromIndex?
            for (int toIndex = 0; toIndex < mapping.Count; toIndex++)
            {
                var (fromIndexes, isWeaker) = mapping[toIndex];
                if (fromIndexes == null || fromIndexes.Count == 0)
                    continue;

                var mergedParts = new List<string>();

                foreach (var fromNullable in fromIndexes.OrderBy(i => i))
                {
                    if (!fromNullable.HasValue)
                        continue;

                    int fromIndex = fromNullable.Value;
                    if (fromIndex < 0 || fromIndex >= lines.Count)
                        continue;

                    var content = lines[fromIndex].Trim();
                    if (!string.IsNullOrEmpty(content))
                        mergedParts.Add(content);
                }

                if (mergedParts.Count > 0)
                {
                    result[toIndex] = string.Join(" ", mergedParts);

                    if (isWeaker)
                    {
                        result[toIndex] += " *";
                    }
                }
            }

            // Giữ lại các dòng gốc chưa bị ánh xạ đi
            var allFromIndexes = mapping
                .Where(t => t.Item1 != null)
                .SelectMany(t => t.Item1)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToHashSet();

            for (int i = 0; i < Math.Min(lines.Count, lineCount); i++)
            {
                bool wasMappedFrom = allFromIndexes.Contains(i);
                bool wasMappedTo = i < mapping.Count && mapping[i].Item1 != null && mapping[i].Item1.Count > 0;

                if (!wasMappedFrom && !wasMappedTo && string.IsNullOrWhiteSpace(result[i]))
                {
                    result[i] = lines[i];
                }
            }

            return string.Join("\n", result);
        }



        #endregion SourceCode3376ImportCode

        #region SourceCode3282ImportCode
                public void SplitAudiosUseWhisperOffline(List<Audio> selectedList, string tempFolder, Video video, char[] splitChars, string choiceId)
        {
            var iDicUrl = new System.Collections.Generic.Dictionary<string, string>();
            var total = 0;
            //Trường hợp nhiều dòng
            var audioFileList = new string[selectedList.Count];
            for (int i = 0; i < selectedList.Count; i++)
            {
                var audio = selectedList[i] as Audio;
                //string languageCode = video.LanguageOrigin != null ? video.LanguageOrigin.Code : null;
                if (string.IsNullOrEmpty(audio.Content) || audio.Start is null || audio.End is null)
                {
                    _notificationService.Notify("Lỗi", "Đối tượng được chọn còn tồn tại thành phần không đủ điều kiện: dấu ngăn cách, Bắt đầu, Kết Thúc", InformationType.Error);
                    continue;
                }
                //Dời chữ sau dấu chấm và dấu phẩy vào câu sau
                //System.Collections.Generic.IList<Audio> addList =
                //    new System.Collections.Generic.List<Audio>();
                //Bỏ ký tự đặc biệt
                audio.Content = audio.Content.Trim().Replace(" ", " ");
                //Bỏ 2 dấu cách
                audio.Content = audio.Content.Trim().Replace("  ", " ");
                //object audioTextWords = null;
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
                            //2025-06-09: Hủy dùng từ phụ đề không rõ nguyên nhân
                            //var subTitlePath = $"{tempFolder}\\{Module.Helpers.FileSystemHelper.GetValidFileName(videoYoutube.Title).Replace("   ", " ").Replace("  ", " ")}.srt";
                            //if (Path.GetFullPath(subTitlePath)
                            //        .StartsWith(Path.GetFullPath(tempFolder).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar,
                            //                    StringComparison.OrdinalIgnoreCase))
                            //    audioUrl = subTitlePath;
                            //else
                            audioUrl = Module.Utils.YouTubeUtils.DownloadFromYoutube(audio.BookMark.URL, tempFolder, false);
                        }
                        else if (Module.Helpers.MediaHelper.CheckVideoSupport(audio.BookMark.URL) ||
                            Module.Utils.OpenAiUtils.CheckOpenAIAudioSupport(audio.BookMark.URL))
                            audioUrl = audio.BookMark.URL;
                        else
                        {

                        }
                    }
                    if (!string.IsNullOrEmpty(audioUrl))
                    {
                        if (Module.Helpers.TextHelper.CheckUnicode(audioUrl))
                        {
                            _notificationService.Notify("Lỗi", $"Đường dẫn không được chứa tiếng việt: {audio.BookMark.URL}", InformationType.Warning);
                            return;
                        }
                        var inputFileInfo = new System.IO.FileInfo(audioUrl);
                        var trimFile = tempFolder + "\\" + audio.Oid.ToString() + "_trim" + inputFileInfo.Extension;
                        //Cắt file audio
                        if (Module.Helpers.AudioVideoHelper.TrimAudio(ObjectSpace, audioUrl, trimFile, audio.GetRealTimeSpan(audio.Start), audio.GetRealTimeSpan(audio.End)) &&
                            System.IO.File.Exists(trimFile))
                        {
                            audioFileList[i] = trimFile;
                            //string logContent = $"Tư liệu {video?.Code} - {video?.Oid} - {e.SelectedChoiceActionItem.Caption}: {audio.Content}";
                            //audioTextWords = Module.Utils.OpenAiUtils.DownloadFromYoutubeToWords(ObjectSpace, xafApplication, trimFile, logContent, ref audioClient, ref audioModel);
                            if (System.IO.Directory.Exists(trimFile))
                                System.IO.Directory.Delete(trimFile, true);
                        }
                    }
                    else if (!iDicUrl.ContainsKey(audio.BookMark.URL))
                    {
                        _notificationService.Notify("Cảnh báo", $"Không hỗ trợ {audio.BookMark.URL}", InformationType.Warning);
                    }
                    if (!iDicUrl.ContainsKey(audio.BookMark.URL))
                        iDicUrl.Add(audio.BookMark.URL, audioUrl);
                }
            }
            if (!audioFileList.Any(x => !string.IsNullOrEmpty(x)))
            {
                _notificationService.Notify("Lỗi", $"{audioFileList.Length} thành phần không có dữ liệu ", InformationType.Error);
                return;
            }
            string pythonDir = null, whisperModelDir = null;
            var audioTextWordsList = Module.Utils.PythonUtils.PythonWhisperTranscriptionToWords(ObjectSpace, Application, audioFileList, ref pythonDir, ref whisperModelDir);
            for (int a = 0; a < selectedList.Count; a++)
            {
                var audioTextWords = audioTextWordsList[a];
                var audio = selectedList[a] as Audio;
                string languageCode = video.LanguageOrigin != null ? video.LanguageOrigin.Code : null;
                if (string.IsNullOrEmpty(audio.Content) || audio.Start is null || audio.End is null)
                {
                    //Module.Helpers.XafXpoHelper.ShowMessage(xafApplication, "Lỗi", "Đối tượng được chọn không đủ điều kiện: dấu ngăn cách, Bắt đầu, Kết Thúc", InformationType.Error);
                    continue;
                }
                //Dời chữ sau dấu chấm và dấu phẩy vào câu sau
                System.Collections.Generic.IList<Audio> addList =
                    new System.Collections.Generic.List<Audio>();

                //Dùng cách cũ
                System.Collections.Generic.IList<int> dotcommaIndexs = new System.Collections.Generic.List<int>();
                // char splitChar = choiceId.Contains("Comma") ? ',' : '.';
                //Bỏ xuống dòng bằng ký tự ngăn cách
                audio.Content = audio.Content.Replace("\r\n", choiceId.Contains("Comma") ? "," : ".");
                //Các từ tương đương dấu chấm, anh xem có bổ sung gì không ạ: dấu hỏi, chấm than, hai chấm, ba chấm

                for (int j = audio.Content.Length - 2; j > 1; j--)
                {
                    if (audio.Content[j] == ' ' && splitChars.Contains(audio.Content[j - 1]))
                        dotcommaIndexs.Add(j);
                }
                if (choiceId.Contains("Comma"))
                {
                    var content = audio.Content;
                    var middle = content.Length / 2;
                    var quarter = content.Length / 4;

                    int? bestCommaIndex = null;
                    int minDistanceFromMiddle = int.MaxValue;

                    for (int i = 0; i < content.Length; i++)
                    {
                        if (content[i] == ',')
                        {
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
                }

                //Fix nguyên gốc
                string audioContent = audio.Content;

                if (dotcommaIndexs.Count > 0)
                {
                    total++;
                    foreach (var dotcommaIndex in dotcommaIndexs)
                    {
                        var newSubtitle = new Audio(audio.Session);
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
            if (total > 0)
                _notificationService.Notify("Kết quả", total + "/" + selectedList.Count + " dòng được tách theo dấu", InformationType.Info);

        }

        #endregion SourceCode3282ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
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
		//public object StartToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VoiceToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Voice != null) 
		//			return Voice;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VoiceSpeedToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SubtitleToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpellingToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementTranslateListToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (ElementTranslateList != null) 
		//			return ElementTranslateList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TermLocationListToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (TermLocationList != null) 
		//			return TermLocationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaListToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (MediaList != null) 
		//			return MediaList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Video != null) 
		//			return Video;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphStyleToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object URLToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (URL != null) 
		//			return URL;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FileDataToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (FileData != null) 
		//			return FileData;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioLinkToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (AudioLink != null) 
		//			return AudioLink;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioDurationToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (AudioDuration != null) 
		//			return AudioDuration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioRateToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DurationToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Duration != null) 
		//			return Duration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SilenceGapToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (SilenceGap != null) 
		//			return SilenceGap;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextRateToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SubtitleTimeToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (SubtitleTime != null) 
		//			return SubtitleTime;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpellingTimeToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (SpellingTime != null) 
		//			return SpellingTime;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioTimeToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (AudioTime != null) 
		//			return AudioTime;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SplittedToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Splitted != null) 
		//			return Splitted;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DeleteToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Delete != null) 
		//			return Delete;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NextElementToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (NextElement != null) 
		//			return NextElement;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PreviousElementToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (PreviousElement != null) 
		//			return PreviousElement;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object StatusToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Status != null) 
		//			return Status;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperElementToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (UpperElement != null) 
		//			return UpperElement;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextNodeToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (TextNode != null) 
		//			return TextNode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NotAdjacentToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (NotAdjacent != null) 
		//			return NotAdjacent;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinePageContentToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinePageTranslateToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParentTagToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (ParentTag != null) 
		//			return ParentTag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CaseTypeToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (CaseType != null) 
		//			return CaseType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BookMarkToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (BookMark != null) 
		//			return BookMark;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateObjectToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (TranslateObject != null) 
		//			return TranslateObject;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Note2ToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Note2 != null) 
		//			return Note2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpeakerToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Speaker != null) 
		//			return Speaker;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementBatchToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (ElementBatch != null) 
		//			return ElementBatch;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageTranslateToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphToolTipControllerText(View view, Module.BusinessObjects.Audio audio)
        //{
        //    if (Paragraph != null) 
		//			return Paragraph;
        //    return null;
        //}
    

	    #endregion
  

    }
}
