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
    public partial class BookMarkViewController: BaseViewController<Module.BusinessObjects.BookMark>
    {      
        
        public BookMarkViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.BookMark);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.BookMarkService bookMarkService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             bookMarkService = new Module.Services.BookMarkService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2623            Oid: b8440d37-b8da-4d8b-be3a-20b718b97306
		private void UrlPasteLink_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(UrlPasteLink), "Dán URL");              
      
            #region UrlPasteLinkImportCode
bookMarkService.UrlPasteLink(e.SelectedChoiceActionItem.Id,GetCurrentObject());

            #endregion UrlPasteLinkImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1098            Oid: e4c5091e-c49d-4737-ace3-b926c06e02b0
		private void FlagLink_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(FlagLink), "Cờ");              
      
            #region FlagLinkImportCode
                        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();            
            stopWatch.Start();
            decimal total = View.SelectedObjects.Count;
            decimal countNumber = 0;
            HtmlAgilityPack.HtmlWeb web = null;
            foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
            {
                bool bookMarkFlag = false;
                //Cờ dấu trang: Toàn hoa, Đầu hoa, Không hoa (tồn tại từ viết thường), dựng cờ, xóa cờ
                if (e.SelectedChoiceActionItem.Id.Equals("Upper"))
                {
                    if (!string.IsNullOrEmpty(bookMark.Name))
                    {
                        bookMarkFlag = Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(bookMark.Name);
                    }                    
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("UpperAll"))
                {
                    if (!string.IsNullOrEmpty(bookMark.Name))
                    {
                        bookMarkFlag = bookMark.Name.Equals(bookMark.Name.ToUpper());
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Lower"))
                {
                    //Không hoa (tồn tại từ viết thường)                    
                    if (!string.IsNullOrEmpty(bookMark.Name))
                    {
                        foreach(var c in bookMark.Name)
                        {
                            if(char.IsLower(c))
                            {
                                bookMarkFlag = true;
                                break;
                            }
                        }
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Raise"))
                {
                    bookMarkFlag = true;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("UrlNotFound"))
                {
                    if (!string.IsNullOrEmpty(bookMark.URL))
                    {
                        string cacheUrl = bookMark.URL;
                        if (bookMark.URL.StartsWith("http") || bookMark.URL.StartsWith("www"))
                        {
                            var cacheFile = Module.Helpers.NameHelper.GetCacheFileName(bookMark.Session, bookMark.URL);
                            if (!string.IsNullOrEmpty(cacheFile))
                                cacheUrl = cacheFile;
                            if(web is null)
                                web = new HtmlAgilityPack.HtmlWeb();
                            try
                            {
                                HtmlAgilityPack.HtmlDocument doc = web.Load(cacheUrl);
                                if (doc.DocumentNode == null)
                                    bookMarkFlag = true;
                                else
                                {
                                    var xpathUrlNotFound = bookMarkService.GetXpathUrlNotFound(bookMark.URL);
                                    if (!string.IsNullOrEmpty(xpathUrlNotFound))
                                    {
                                        var element = doc.DocumentNode.SelectSingleNode(xpathUrlNotFound);
                                        if (element != null)
                                            bookMarkFlag = true;
                                    }
                                    
                                }
                            }
                            catch(System.Exception ex) 
                            {
                                bookMarkFlag = true;
                            }
                            
                        }

                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("MultiFolder"))
                {
                    if (!string.IsNullOrEmpty(bookMark.URL))
                    {

                        var otherBookmark = View.ObjectSpace.FindObject<Module.BusinessObjects.BookMark>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid <> ? and URL = ? ", bookMark.Oid, bookMark.URL));
                        if (otherBookmark != null)
                        {
                            bookMarkFlag = true;
                        }
                    }
                }
                if (!bookMark.Equals(bookMarkFlag))
                    bookMark.Flag = bookMarkFlag;
                countNumber++;
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), " ", stopWatch.Elapsed);
            }
            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, null);




            #endregion FlagLinkImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1414            Oid: 568d5409-c3f2-4e80-86e4-5b421f0175bb
		private void QuantityFunction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(QuantityFunction), "Đếm");              
      
            #region QuantityFunctionImportCode
            foreach(Module.BusinessObjects.BookMark bookMark in View.SelectedObjects) 
            {
                if (e.SelectedChoiceActionItem.Id.Equals("NameLength"))
                {
                    bookMark.Quantity = Name?.Length;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Duration"))
                {                    
                    if (Module.Utils.YouTubeUtils.IsYoutubeUrl(bookMark.URL))
                    {
                        var youtube = new YoutubeExplode.YoutubeClient();
                        var videoId = YoutubeExplode.Videos.VideoId.Parse(bookMark.URL);
                        var downloadTask = System.Threading.Tasks.Task.Run(() => youtube.Videos.GetAsync(videoId));
                        downloadTask.Wait();
                        var videoYoutube = downloadTask.Result;
                        bookMark.Quantity = System.Convert.ToInt32(videoYoutube.Result.Duration?.TotalSeconds);
                    }
                    else if (Module.Helpers.MediaHelper.CheckVideoSupport(bookMark.URL) || Module.Utils.OpenAiUtils.CheckOpenAIAudioSupport(bookMark.URL))
                    {
                        var quantity = Module.Helpers.AudioVideoHelper.GetDuration(View.ObjectSpace, bookMark.URL);
                        if(quantity != null)
                            bookMark.Quantity = System.Convert.ToInt32(quantity);
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Sum"))
                {
                    var audioList = View.ObjectSpace.GetObjects<Module.BusinessObjects.Audio>(DevExpress.Data.Filtering.CriteriaOperator.Parse("BookMark = ?", bookMark));
                    bookMark.Quantity = audioList?.Count;
                }
            }


            #endregion QuantityFunctionImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2629            Oid: 57ad4f0d-b2b2-4d32-a14d-ab53eda7725a
		private void ObjectSearch_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ObjectSearch), "Tìm đối tượng");              
      
            #region ObjectSearchImportCode

        var recognition = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Recognition;
        var fileCode = recognition.Code;

        var userName = SecuritySystem.CurrentUserName;
        string rootPath = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "NssmFolder", "null").Value;
        rootPath = rootPath == "null" ? null : rootPath;

        DataService dataService = this.ObjectSpace.FindObject<DataService>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", "008"));
        var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);

        System.Collections.Generic.List<string> urls = new System.Collections.Generic.List<string>();

        int minSize, gapFrame;
        try
        {
            minSize = int.Parse(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MinImageSizeToKeep", "40", SecuritySystem.CurrentUserId).Value);
            gapFrame = int.Parse(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "GapFrame", "3", SecuritySystem.CurrentUserId).Value);
        }
        catch (System.Exception ex)
        {
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi lấy tham số từ hệ thống: {ex.Message}", InformationType.Error);
            return;
        }

        System.Collections.Generic.List<Module.BusinessObjects.BookMark> bookMarks = new System.Collections.Generic.List<Module.BusinessObjects.BookMark>();

        foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
        {
            if (!string.IsNullOrEmpty(bookMark.URL))
            {
                bookMarks.Add(bookMark);
            }
        }

        // Nếu không có URL hợp lệ thì không làm gì
        if (bookMarks.Count == 0)
        {
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "⚠️ Không có Bookmark hợp lệ để xử lý.", InformationType.Error);
            return;
        }

        string processedFolder = null;


        var t1 = System.Threading.Tasks.Task.Run(() =>
        {
            // Đảm bảo không tiếp tục trên UI thread
            processedFolder = bookMarkService.PreprocessDataForDetect(bookMarks, userName, fileCode, rootPath, gapFrame);
        });

        t1.Wait();

        if (!string.IsNullOrEmpty(processedFolder))
        {
                bookMarkService.ProcessFaceFind(dataServiceDto, recognition, rootPath, userName, processedFolder, minSize, gapFrame);
        }




            #endregion ObjectSearchImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1409            Oid: b3390cb7-b2c5-4f3f-bc02-7267daf0da6a
		private void LinkVoiceSpeed_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(LinkVoiceSpeed), "Nạp tốc độ");              
      
            #region LinkVoiceSpeedImportCode
if (e.SelectedChoiceActionItem.Id.Equals("Average"))
{                
    foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
    {
        var audioList = View.ObjectSpace.GetObjects<Module.BusinessObjects.Audio>(
            DevExpress.Data.Filtering.CriteriaOperator.Parse("BookMark = ?", bookMark));

        if (audioList.Count == 0)
            continue;

        decimal totalDuration = 0m;
        decimal totalAudioDuration = 0m;

        foreach (Module.BusinessObjects.Audio audio in audioList)
        {
            decimal audioDuration = audio.AudioDuration.HasValue ? audio.AudioDuration.Value : 0m;
            decimal duration = audio.Duration.HasValue ? audio.Duration.Value : 0m;

            if (audioDuration > 0 && duration > 0)
            {
                totalAudioDuration += audioDuration;
                totalDuration += duration;
            }
        }

        if (totalDuration > 0) // tránh chia cho 0
        {
            decimal voiceSpeed = totalAudioDuration / totalDuration;

            if (voiceSpeed > 0)
            {
                foreach (Module.BusinessObjects.Audio audio in audioList)
                {
                    decimal duration = audio.Duration.HasValue ? audio.Duration.Value : 0m;
                    if (duration > 0)
                    {
                        audio.VoiceSpeed = voiceSpeed;
                    }
                }
            }
        }
    }                
}

            #endregion LinkVoiceSpeedImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2260            Oid: d8bc0e39-1bd7-4b2e-a103-cf46d8c6ef65
		private void Detection_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(Detection), "Nhận dạng");              
      
            #region DetectionImportCode
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            if (e.SelectedChoiceActionItem.Id.Equals("Face"))
            {
                var recognition = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Recognition;
                var fileCode = recognition.Code;

                var userName = SecuritySystem.CurrentUserName;
                string rootPath = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "NssmFolder", "null").Value;
                rootPath = rootPath == "null" ? null : rootPath;

                if (string.IsNullOrEmpty(rootPath))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "⚠️ Không tìm thấy thông tin đường dẫn mạng hoặc đường dẫn không hợp lệ.", InformationType.Error);
                    return;
                }

                DataService dataService = this.ObjectSpace.FindObject<DataService>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", "008"));
                var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);

                System.Collections.Generic.List<string> urls = new System.Collections.Generic.List<string>();

                int minSize, gapFrame;
                try
                {
                    minSize = int.Parse(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MinImageSizeToKeep", "40", SecuritySystem.CurrentUserId).Value);
                    gapFrame = int.Parse(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "GapFrame", "3", SecuritySystem.CurrentUserId).Value);
                }
                catch (System.Exception ex)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi lấy tham số từ hệ thống: {ex.Message}", InformationType.Error);
                    return;
                }
                System.Collections.Generic.List<Module.BusinessObjects.BookMark> bookMarks = new System.Collections.Generic.List<Module.BusinessObjects.BookMark>();

                foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
                {
                    if (!string.IsNullOrEmpty(bookMark.URL))
                    {
                        bookMarks.Add(bookMark);
                    }
                }

                // Nếu không có URL hợp lệ thì không làm gì
                if (bookMarks.Count == 0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "⚠️ Không có Bookmark hợp lệ để xử lý.", InformationType.Error);
                    return;
                }

                string processedFolder = null;


                var t1 = System.Threading.Tasks.Task.Run(() =>
                {
                    // Đảm bảo không tiếp tục trên UI thread
                    processedFolder = bookMarkService.PreprocessDataForDetect(bookMarks, userName, fileCode, rootPath, gapFrame);
                });

                t1.Wait();


                if (!string.IsNullOrEmpty(processedFolder))
                {
                    bookMarkService.ProcessFaceDetection(dataServiceDto, recognition, processedFolder, minSize, gapFrame, rootPath);
                }


            }
            stopWatch.Stop(); // Dừng đồng hồ khi hoàn tất




            #endregion DetectionImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3277            Oid: 1521c6fb-8ff7-4338-ac0b-f5a0dbe48367
		private void FlagLinkVideo_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(FlagLinkVideo), "Cờ");              
      
            #region FlagLinkVideoImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Subtitle"))
            {
                var video = GetMasterObject<Video>();
                var languageCode = video.LanguageOrigin?.Code;
                var total = 0;

                foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
                {
                    if (Module.Utils.YouTubeUtils.IsYoutubeUrl(bookMark.URL))
                    {
                        try
                        {
                            var trackManifest = Task.Run(() => Module.Utils.YouTubeUtils.GetTrackCaptionManifest(bookMark.URL)).Result;
                            var trackInfo = trackManifest.Tracks?
                                .FirstOrDefault(x => !x.IsAutoGenerated && x.Language.Code.StartsWith(languageCode));

                            if (trackInfo != null)
                            {
                                bookMark.Flag = true;
                                total++;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log hoặc hiển thị lỗi nếu cần
                        }
                    }
                }
                if (total > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", $"{total}/{View.SelectedObjects.Count} video có phụ đề", InformationType.Info);
                else
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", $"Không có video có phụ đề", InformationType.Info);


            }

            #endregion FlagLinkVideoImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 4541            Oid: 83213eb1-5162-43fe-885f-b881050cb04f
		private void PostContentImport_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(PostContentImport), "Nạp Nội dung Bài viết");              
      
            #region PostContentImportImportCode


            #endregion PostContentImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1211            Oid: 269cc9ae-e080-40b7-87d7-4b08d33962a7
		private void LinkNoteSync_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(LinkNoteSync), "Đồng bộ");              
      
            #region LinkNoteSyncImportCode
            int count = 0;
            foreach(Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
            {
                if (string.IsNullOrEmpty(bookMark.URL) || string.IsNullOrEmpty(bookMark.Note)) continue;
                var otherBookmarks = ObjectSpace.GetObjects<BookMark>((DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid <> ? and URL = ? ", bookMark.Oid, bookMark.URL)));
                foreach(var otherBookMark in otherBookmarks)
                {
                    otherBookMark.Note = bookMark.Note;
                    count++;
                }                
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", count + " được đồng bộ");


            #endregion LinkNoteSyncImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 4542            Oid: 309b294a-077d-48d3-b524-ee0ab8978dfd
		private void DataChatbotAI_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(DataChatbotAI), "Trích AI");              
      
            #region DataChatbotAIImportCode


            #endregion DataChatbotAIImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1339            Oid: 208ff922-55e3-4723-8dc3-5b77d654a9ce
		private void ObjectMatchingLink_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ObjectMatchingLink), "Khớp đối tượng");              
      
            #region ObjectMatchingLinkImportCode
                        System.Type objectType = null;
            if (e.SelectedChoiceActionItem.Id.Contains("Product"))
                objectType = typeof(Module.BusinessObjects.Product);
            else if (e.SelectedChoiceActionItem.Id.Contains("Contact"))
                objectType = typeof(Module.BusinessObjects.Contact);
            else if (e.SelectedChoiceActionItem.Id.Contains("Org"))
                objectType = typeof(Module.BusinessObjects.Org);
            if (objectType is null)
                return;
            int result = 0;
            int total = View.SelectedObjects.Count;
            var splitCharArray = new char[] { ' ', ',', '.', ':', ';', '"', '\'', '!', '?', '[', ']', '{', '}', '-', '~' };
            foreach (Module.BusinessObjects.BookMark bookMark in View.SelectedObjects)
            {
                if (e.SelectedChoiceActionItem.Id.Contains("Product"))
                {                    
                    if (bookMark.Product != null)
                        continue;
                }else if (e.SelectedChoiceActionItem.Id.Contains("Contact"))
                {
                    if (bookMark.Contact != null)
                        continue;
                }
                else if (e.SelectedChoiceActionItem.Id.Contains("Org"))
                {
                    if (bookMark.Org != null)
                        continue;
                }
                string content = "";
                if (e.SelectedChoiceActionItem.Id.Contains("Name"))
                {
                    content = bookMark.Name;
                }
                else if (e.SelectedChoiceActionItem.Id.Contains("Content") && !string.IsNullOrEmpty(bookMark.URL))
                {
                    HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                    string cacheUrl = bookMark.URL;
                    if (bookMark.URL.StartsWith("http") || bookMark.URL.StartsWith("www"))
                    {
                        var cacheFile = Module.Helpers.NameHelper.GetCacheFileName(bookMark.Session, bookMark.URL);
                        if (!string.IsNullOrEmpty(cacheFile))
                            cacheUrl = cacheFile;
                    }
                    HtmlAgilityPack.HtmlDocument doc = web.Load(cacheUrl);
                    if(doc.DocumentNode != null)
                    {
                        content = doc.DocumentNode.InnerText;
                    }
                }
                if (string.IsNullOrEmpty(content))
                    continue;
                var wordArray = content.ToLower().Split(splitCharArray, System.StringSplitOptions.RemoveEmptyEntries);
                DevExpress.Data.Filtering.CriteriaOperator criteriaOperator = null;
                foreach(var word in wordArray)
                {
                    criteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.Or(criteriaOperator,
                        DevExpress.Data.Filtering.CriteriaOperator.Parse("Contains(Name, ?)", word));
                }
                var itemList = View.ObjectSpace.GetObjects(objectType, criteriaOperator);
                if (itemList.Count > 0)
                {
                    System.Collections.Generic.HashSet<string> strCompareHash = wordArray.ToHashSet();
                    int maxIntersectCount = 0;
                    int maxDifferenceCount = 0;
                    object bestObj = null;
                    foreach (var item in itemList)
                    {
                        var name = item.GetPropertyValue("Name") as string;
                        //if (name.Contains("T715Pro"))
                        //{

                        //}
                        if (string.IsNullOrEmpty(name))
                            continue;
                        System.Collections.Generic.HashSet<string> strHash = name.ToLower().Split(splitCharArray, System.StringSplitOptions.RemoveEmptyEntries).ToHashSet();
                        var intersectWord = strCompareHash.Intersect(strHash);
                        int intersectCount = intersectWord.Count();
                        if (maxDifferenceCount > 0 && intersectCount == maxIntersectCount)
                        {
                            maxDifferenceCount++;
                        }
                        else if (intersectCount > maxIntersectCount)
                        {
                            maxIntersectCount = intersectCount;
                            bestObj = item;
                            maxDifferenceCount = 1;
                        }
                    }
                    if (bestObj != null)
                    {
                        bookMark.SetMemberValue(objectType.Name, bestObj);                        
                        if (maxDifferenceCount > 1)
                            bookMark.Flag = true;
                        result++;
                    }
                }
                //if (e.SelectedChoiceActionItem.Id.Contains("Product"))
                //{
                //    var productList = View.ObjectSpace.GetObjects<Module.BusinessObjects.Product>(criteriaOperator);
                //    if(productList.Count() > 0)
                //    {
                //        System.Collections.Generic.HashSet<string> strCompareHash = wordArray.ToHashSet();
                //        int maxIntersectCount = 0;
                //        int maxDifferenceCount = 0;
                //        Module.BusinessObjects.Product bestProduct = null;
                //        foreach (Module.BusinessObjects.Product product in productList)
                //        {
                //            if (string.IsNullOrEmpty(product.Name))
                //                continue;
                //            System.Collections.Generic.HashSet<string> strHash = product.Name.Split(' ').ToHashSet();
                //            int intersectCount = strCompareHash.Intersect(strHash).Count();
                //            if (intersectCount == maxIntersectCount)
                //            {
                //                maxDifferenceCount++;
                //            }
                //            else if (intersectCount > maxIntersectCount)
                //            {
                //                maxIntersectCount = intersectCount;
                //                bestProduct = product;
                //                maxDifferenceCount = 1;
                //            }
                //        }
                //        if(bestProduct != null)
                //        {
                //            bookMark.Product = bestProduct;
                //            if(maxDifferenceCount > 1)
                //                bookMark.Flag = true;
                //            result++;
                //        }
                //    }                    
                //}
                //else if (e.SelectedChoiceActionItem.Id.Contains("Contact"))
                //{
                //    var contactList = View.ObjectSpace.GetObjects<Module.BusinessObjects.Contact>(criteriaOperator);
                //    if (contactList.Count() > 0)
                //    {
                //        System.Collections.Generic.HashSet<string> strCompareHash = wordArray.ToHashSet();
                //        int maxIntersectCount = 0;
                //        int maxDifferenceCount = 0;
                //        Module.BusinessObjects.Contact bestContact = null;
                //        foreach (Module.BusinessObjects.Contact contact in contactList)
                //        {
                //            if (string.IsNullOrEmpty(contact.Name))
                //                continue;
                //            System.Collections.Generic.HashSet<string> strHash = contact.Name.Split(' ').ToHashSet();
                //            int intersectCount = strCompareHash.Intersect(strHash).Count();
                //            if (intersectCount == maxIntersectCount)
                //            {
                //                maxDifferenceCount++;
                //            }
                //            else if (intersectCount > maxIntersectCount)
                //            {
                //                maxIntersectCount = intersectCount;
                //                bestContact = contact;
                //                maxDifferenceCount = 1;
                //            }
                //        }
                //        if (bestContact != null)
                //        {
                //            bookMark.Contact = bestContact;
                //            if (maxDifferenceCount > 1)
                //                bookMark.Flag = true;
                //            result++;
                //        }
                //    }
                //} 
                //else if (e.SelectedChoiceActionItem.Id.Contains("Org"))
                //{
                //    var orgList = View.ObjectSpace.GetObjects<Module.BusinessObjects.Org>(criteriaOperator);
                //    if (orgList.Count() > 0)
                //    {
                //        System.Collections.Generic.HashSet<string> strCompareHash = wordArray.ToHashSet();
                //        int maxIntersectCount = 0;
                //        int maxDifferenceCount = 0;
                //        Module.BusinessObjects.Org bestOrg = null;
                //        foreach (Module.BusinessObjects.Org org in orgList)
                //        {
                //            if (string.IsNullOrEmpty(org.Name))
                //                continue;
                //            System.Collections.Generic.HashSet<string> strHash = org.Name.Split(' ').ToHashSet();
                //            int intersectCount = strCompareHash.Intersect(strHash).Count();
                //            if (intersectCount == maxIntersectCount)
                //            {
                //                maxDifferenceCount++;
                //            }
                //            else if (intersectCount > maxIntersectCount)
                //            {
                //                maxIntersectCount = intersectCount;
                //                bestOrg = org;
                //                maxDifferenceCount = 1;
                //            }
                //        }
                //        if (bestOrg != null)
                //        {
                //            bookMark.Org = bestOrg;
                //            if (maxDifferenceCount > 1)
                //                bookMark.Flag = true;
                //            result++;
                //        }
                //    }
                //}
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result.ToString("D") + "/" + total.ToString("D") +  " được cập nhật", InformationType.Info);



            #endregion ObjectMatchingLinkImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}