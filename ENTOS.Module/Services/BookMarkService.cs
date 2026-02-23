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
using Newtonsoft.Json.Linq;
using System.IO.Compression;

 
namespace ENTOS.Module.Services 
{

    public partial class BookMarkService : BaseService
    {

        public BookMarkService() : base()
        {
        }
        #region DependencyInjection
        private IClipboardService clipboardService;
        protected IClipboardService _clipboardService => clipboardService ??= Application.ServiceProvider.GetRequiredService<IClipboardService>();        
  
     
        private RecognitionService recognitionService;  
        protected RecognitionService _recognitionService => recognitionService ??= new RecognitionService(ViewController);        
       
  
        #endregion DependencyInjection

        public BookMarkService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3442ImportCode
                        public string PreprocessDataForDetect(System.Collections.Generic.List<BookMark> bookmarks, string userName, string fileCode, string rootPath, int gapFrame)
        {
            if (bookmarks == null || bookmarks.Count == 0 || rootPath == null)
                return null;

            string user = GetValueOrDefault("FileServerUser", "null");
            string pass = GetValueOrDefault("FileServerPassword", "null");

            var credentials = new System.Net.NetworkCredential(user, pass);
            try
            {
                using (new NetworkConnection(rootPath, credentials))
                {
                    var (recognizeFolder, userFolder, fileFolder, sessionFolder) = BuildRecognitionFolders(rootPath, userName, fileCode);

                    try
                    {
                        if (!System.IO.Directory.Exists(recognizeFolder))
                            System.IO.Directory.CreateDirectory(recognizeFolder);
                        if (!System.IO.Directory.Exists(userFolder))
                            System.IO.Directory.CreateDirectory(userFolder);
                        if (!System.IO.Directory.Exists(fileFolder))
                            System.IO.Directory.CreateDirectory(fileFolder);

                        if (System.IO.File.Exists(sessionFolder))
                            System.IO.File.Delete(sessionFolder);
                        else if (System.IO.Directory.Exists(sessionFolder))
                            System.IO.Directory.Delete(sessionFolder, true);

                        System.IO.Directory.CreateDirectory(sessionFolder);

                        int imageIndex = 1;

                        foreach (var bookMark in bookmarks)
                        {
                            if (string.IsNullOrEmpty(bookMark.URL))
                                continue;

                            string URL = bookMark.URL;

                            try
                            {
                                if (Module.Utils.YouTubeUtils.IsYoutubeUrl(URL))
                                {
                                    string downloadedVideoPath = Module.Services.RecognitionService.DownloadYouTubeVideoToNetworkFolder(URL, rootPath, userName);
                                    if (!string.IsNullOrEmpty(downloadedVideoPath))
                                    {
                                        URL = downloadedVideoPath;
                                    }
                                    else
                                    {
                                        _notificationService.Notify( "Lỗi", "❌ Lỗi khi tải video từ YouTube.", InformationType.Error);
                                        continue;
                                    }
                                }

                                string bookmarkFolder = System.IO.Path.Combine(sessionFolder, bookMark.Oid.ToString());
                                System.IO.Directory.CreateDirectory(bookmarkFolder);

                                if (System.IO.Directory.Exists(URL))
                                {
                                    System.Console.WriteLine($"📂 Xử lý folder ảnh: {URL}");
                                    Module.Services.RecognitionService.ProcessImageFolder(URL, bookmarkFolder, ref imageIndex);
                                }
                                else if (System.IO.File.Exists(URL))
                                {
                                    string extension = System.IO.Path.GetExtension(URL).ToLowerInvariant();

                                    if (IsImageExtension(extension))
                                    {
                                        System.Console.WriteLine($"🖼️ Xử lý ảnh đơn: {URL}");
                                        Module.Services.RecognitionService.ProcessSingleImage(URL, bookmarkFolder, ref imageIndex);
                                    }
                                    else if (IsVideoExtension(extension))
                                    {
                                        System.Console.WriteLine($"🎥 Xử lý video: {URL}");
                                        imageIndex = Module.Services.RecognitionService.ProcessVideo(URL, bookmarkFolder, imageIndex, gapFrame);
                                    }
                                    else
                                    {
                                        System.Console.WriteLine($"⚠️ Bỏ qua file không hỗ trợ: {URL}");
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                _notificationService.Notify( "Lỗi", $"❌ Lỗi khi xử lý bookmark {bookMark.Oid}: {ex.Message}", InformationType.Error);
                            }
                        }

                        System.Console.WriteLine($"✅ Dữ liệu đã lưu tại: {sessionFolder}");
                        return sessionFolder;
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine($"❌ Lỗi khi xử lý dữ liệu: {ex.Message}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Lỗi: {ex.Message}", ex);
            }

            //Module.Helpers.LogHelper.Info(logMessage + " - End");
        }





        #endregion SourceCode3442ImportCode

        #region SourceCode3444ImportCode
                        public  void ProcessFaceFind(Application.DTOs.DataServiceDto dataServiceDto, Recognition recognition, string rootPath, string userName, string URL, int minSize, int gapFrame)
        {
            string compareImagesDirectory = null;

            var compareImagePaths = new System.Collections.Generic.List<string>();
            var objectNames = new System.Collections.Generic.List<string>();

            string firstPath = System.IO.Path.Combine(rootPath, "recognize", userName, recognition.Code + "FindUpload");
            string secondPath = System.IO.Path.Combine(rootPath, "recognize", userName, recognition.Code);

            if (System.IO.Directory.Exists(firstPath))
            {
                System.IO.Directory.Delete(firstPath, true);
            }
            System.IO.Directory.CreateDirectory(firstPath);
            foreach (RecognitionObject recognitionObject in recognition.RecognitionObjectList)
            {
                if (recognitionObject.Flag == true)
                {
                    if (recognitionObject.Image != null)
                    {
                        string imagePath = System.IO.Path.Combine(firstPath, $"{recognitionObject.Name}.jpg");
                        System.IO.File.WriteAllBytes(imagePath, recognitionObject.Image);

                        compareImagePaths.Add(imagePath);
                        objectNames.Add(recognitionObject.Name);
                    }

                }
            }
            compareImagesDirectory = firstPath;
            try
            {
                // Gọi API nhận diện khuôn mặt đồng bộ
                string response = _recognitionService.GetFaceResponse(dataServiceDto, URL, minSize, compareImagesDirectory, gapFrame, rootPath);

                if (string.IsNullOrEmpty(response))
                {
                    _notificationService.Notify( "Lỗi", "⚠️ API trả về phản hồi rỗng.", InformationType.Error);
                    return;
                }

                var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(response);
                var results = jsonResponse["results"] as Newtonsoft.Json.Linq.JArray;

                if (results == null || results.Count == 0)
                {
                    _notificationService.Notify( "Lỗi", "⚠️ API không chứa danh sách kết quả.", InformationType.Error);
                    return;
                }

                try
                {
                    // Kiểm tra null cho recognition
                    if (recognition == null)
                    {
                        _notificationService.Notify( "Lỗi", "⚠️ Không tìm thấy Recognition.", InformationType.Error);
                        return;
                    }
                    System.Collections.Generic.Dictionary<string, double> fileFrameRates = new System.Collections.Generic.Dictionary<string, double>();

                    foreach (var face in results)
                    {
                        var recognitionObject = recognition.RecognitionObjectList.FirstOrDefault(x => x.Name.Equals(face["object_name"]?.ToString()));
                        if (recognitionObject == null)
                        {
                            _notificationService.Notify( "Lỗi", "⚠️ Không tìm thấy RecognitionObject trong ObjectSpace mới.", InformationType.Error);
                            return;
                        }
                        System.Collections.Generic.List<RecognitionPosition> recognitionPositions = new System.Collections.Generic.List<RecognitionPosition>();

                        // Xoá các RecognitionPosition cũ
                        var positionsToDelete = recognitionObject.RecognitionPositionList.ToList();
                        foreach (var recognitionPosition in positionsToDelete)
                        {
                            recognitionPosition.Delete();
                        }

                        var fileData = face["files"]?.ToObject<System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject>>() ?? new System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject>();

                        foreach (var file in fileData)
                        {
                            string fileName = file["fileName"]?.ToString() ?? "";
                            string ytPath = System.IO.Path.Combine(rootPath, "recognize", "upload");

                            var frameObjects = file["frames"]?.ToObject<System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject>>() ?? new System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject>();
                            foreach (var frameData in frameObjects)
                            {
                                int frameIdx = frameData["frame_idx"]?.ToObject<int>() ?? 0;
                                int beginFrame = frameData["begin_frame"]?.ToObject<int>() ?? 0;
                                int endFrame = frameData["end_frame"]?.ToObject<int>() ?? 0;
                                int x = frameData["x"]?.ToObject<int>() ?? 0;
                                int y = frameData["y"]?.ToObject<int>() ?? 0;
                                int size = frameData["size"]?.ToObject<int>() ?? 0;
                                decimal reliability = frameData["confidence"]?.ToObject<decimal>() ?? 0.0m;
                                decimal yaw = frameData["face"]["pose"]?["yaw"]?.ToObject<decimal>() ?? 0.0m;
                                decimal roll = frameData["face"]["pose"]?["roll"]?.ToObject<decimal>() ?? 0.0m;
                                var bookMark = recognition.BookMarkList.FirstOrDefault(x => x.Oid.ToString().Equals(frameData["link"]?.ToString()));
                                string fileLink = null;

                                if (bookMark != null)
                                {
                                    if (Module.Utils.YouTubeUtils.IsYoutubeUrl(bookMark.URL))
                                    {
                                        fileLink = System.IO.Path.Combine(ytPath, fileName);
                                    }
                                    else if (bookMark.URL.EndsWith(".mp4") || bookMark.URL.EndsWith(".avi") || bookMark.URL.EndsWith(".mov") || bookMark.URL.EndsWith(".mkv") || bookMark.URL.EndsWith(".png") || bookMark.URL.EndsWith(".jpg") || bookMark.URL.EndsWith(".jepg") || bookMark.URL.EndsWith(".webp"))
                                    {
                                        fileLink = System.IO.Path.Combine(bookMark.URL);
                                    }
                                    else
                                    {
                                        if (fileName.EndsWith("__mp4") || fileName.EndsWith("__avi") || fileName.EndsWith("__mov") || fileName.EndsWith("__mkv"))
                                        {
                                            int lastIndex = fileName.LastIndexOf("__");
                                            if (lastIndex >= 0)
                                            {
                                                fileName = fileName.Substring(0, lastIndex) + "." + fileName.Substring(lastIndex + 2);
                                            }
                                        }

                                        fileLink = System.IO.Path.Combine(bookMark.URL, fileName);
                                    }
                                }

                                if (!fileFrameRates.ContainsKey(fileLink))
                                {
                                    if (fileLink.EndsWith(".mp4") || fileLink.EndsWith(".avi") || fileLink.EndsWith(".mov") || fileLink.EndsWith(".mkv"))
                                    {
                                        double frameRate =  Module.Helpers.AudioVideoHelper.GetFrameRate(fileLink); // Hàm này bạn đã có
                                        fileFrameRates[fileLink] = frameRate;
                                    }
                                    else
                                    {
                                        fileFrameRates[fileLink] = 0.0; // hoặc gán mặc định
                                    }
                                }

                                double secondsPerFrame = 1.0 / (fileFrameRates.TryGetValue(fileLink, out var fr) && fr > 0 ? fr : 25); // fallback mặc định 25 fps

                                var recognitionPosition = new RecognitionPosition(recognitionObject.Session)
                                {
                                    Link = fileLink,
                                    BeginFrame = beginFrame,
                                    EndFrame = endFrame,
                                    ImageFrame = frameIdx,
                                    Horizontal = x,
                                    Vertical = y,
                                    Size = size,
                                    Reliability = reliability,
                                    Begin = System.TimeSpan.FromSeconds(beginFrame * secondsPerFrame),
                                    End = System.TimeSpan.FromSeconds(endFrame * secondsPerFrame),
                                    Yaw = yaw,
                                    Roll = roll,
                                    Image = !string.IsNullOrEmpty(frameData["face"]?["img"]?.ToString())
                                        ? System.Convert.FromBase64String(frameData["face"]?["img"]?.ToString())
                                        : null
                                };

                                recognitionPositions.Add(recognitionPosition);
                            }
                        }

                        recognitionObject.RecognitionPositionList.AddRange(recognitionPositions);
                        // Chọn vị trí có yaw nhỏ nhất làm ảnh đại diện
                        var bestPosition = recognitionPositions.OrderBy(rp => System.Math.Abs(rp.Yaw.Value)).FirstOrDefault();

                        // Cập nhật RecognitionObject với dữ liệu từ ảnh đại diện
                        if (bestPosition != null)
                        {
                            recognitionObject.Reliability = bestPosition.Reliability;
                            recognitionObject.Size = bestPosition.Size;
                            recognitionObject.Frame = bestPosition.ImageFrame;
                            recognitionObject.Image = bestPosition.Image;
                            recognitionObject.RecognitionPosition = bestPosition;

                        }

                    }
                }
                catch (System.Exception ex)
                {
                    _notificationService.Notify( "Lỗi", $"❌ Lỗi khi tạo các đối tượng: {ex.Message}", InformationType.Error);
                }
            }
            catch (System.Exception ex)
            {
                _notificationService.Notify( "Lỗi", $"❌ Lỗi khi gọi API DetectFacesAsync: {ex.Message}", InformationType.Error);
            }
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}


        #endregion SourceCode3444ImportCode

        #region SourceCode4515ImportCode
                        private string lastedHomeUrl = "";
private string lastedHomeUrlXpath = "";
public string GetXpathUrlNotFound(string url)
{
    System.Uri myUri = new System.Uri(url);
    //var homeUrl = myUri.Scheme + "://" + myUri.Host;
    if (lastedHomeUrl == myUri.Host)
    {
        return lastedHomeUrlXpath;
    }
    else
    {
        string key = "XpathUrlNotFound-" + myUri.Host;
        var xPath = GetValueOrDefault( key);
        if (string.IsNullOrEmpty(xPath) && myUri.Host.Equals("www.ebay.com"))
        {
            xPath =  GetValueOrDefault( key, "//*[@id=\"s0-1-0-11-0-0-status\"]");
        }
        if (!string.IsNullOrEmpty(xPath))
        {
            lastedHomeUrl = myUri.Host;
            lastedHomeUrlXpath = xPath;
        }
        return xPath;
    }
    return null;
    //Module.Helpers.LogHelper.Info(logMessage + " - End");
}


        #endregion SourceCode4515ImportCode

        #region SourceCode4538ImportCode
                        internal void UrlPasteLink(string choice, BookMark currentBookMark)
        {
            if (_clipboardService.GetDataPresent("Html"))
            {
                var htmlText = _clipboardService.GetData("Html") as string;
                if (string.IsNullOrEmpty(htmlText))
                {
                    htmlText = _clipboardService.GetData("UnicodeText") as string;
                    if (string.IsNullOrEmpty(htmlText))
                        return;

                }
                if (string.IsNullOrEmpty(htmlText))
                    return;

                if (htmlText.StartsWith("http") || htmlText.StartsWith("www"))
                {
                    //Hỗ trợ paste link trực tiếp                
                    currentBookMark.URL = System.Uri.UnescapeDataString(htmlText);
                    return;
                }
                //Không phải là cấu trúc html
                if (!htmlText.StartsWith("<") && !htmlText.StartsWith(">") && !htmlText.Contains("<html>") && !htmlText.Contains("</html>"))
                    return;
                var htmlDocument = new HtmlAgilityPack.HtmlDocument();

                htmlDocument.LoadHtml(htmlText);
                string nodeName = choice.Equals("WebLink") ? "a" : "img";
                string nodeAttribute = choice.Equals("WebLink") ? "href" : "src";
                var allLink = htmlDocument.DocumentNode.Descendants(nodeName);
                //string result = "";
                //int total = 0;
                //IList<string> links = new List<string>();
                bool sussess = false;
                foreach (var linkNode in allLink)
                {
                    string href = linkNode.GetAttributeValue(nodeAttribute, "default");
                    if (string.IsNullOrEmpty(href))
                        continue;
                    //Html Decode href
                    href = System.Uri.UnescapeDataString(href);
                    if (!string.IsNullOrEmpty(href))
                    {
                        currentBookMark.URL = System.Uri.UnescapeDataString(href);
                        sussess = true;
                    }
                }
                if (!sussess)
                    _notificationService.Notify("Lỗi", "Không tìm thấy liên kết trong clipboard", InformationType.Error);
            }


        }



        #endregion SourceCode4538ImportCode

        #region SourceCode3440ImportCode
                        public static string GetFileLink(BookMark bookMark, string fileName, string ytPath)
        {
            if (bookMark == null || string.IsNullOrEmpty(bookMark.URL))
                return "";

            if (Module.Utils.YouTubeUtils.IsYoutubeUrl(bookMark.URL))
            {
                return Path.Combine(ytPath, fileName);
            }

            if (bookMark.URL.EndsWith(".mp4") || bookMark.URL.EndsWith(".avi") || bookMark.URL.EndsWith(".mov") ||
                bookMark.URL.EndsWith(".mkv") || bookMark.URL.EndsWith(".png") || bookMark.URL.EndsWith(".jpg") ||
                bookMark.URL.EndsWith(".jpeg") || bookMark.URL.EndsWith(".webp"))
            {
                return bookMark.URL;
            }

            // Tên file có hậu tố __mp4, __avi,...
            if (fileName.Contains("__"))
            {
                var lastIndex = fileName.LastIndexOf("__");
                if (lastIndex >= 0)
                {
                    fileName = fileName.Substring(0, lastIndex) + "." + fileName.Substring(lastIndex + 2);
                }
            }

            return Path.Combine(bookMark.URL, fileName);
        }


        #endregion SourceCode3440ImportCode

        #region SourceCode3438ImportCode
                        public void ProcessFaceDetection(Application.DTOs.DataServiceDto dataServiceDto, Recognition recognition, string URL, int minSize, int gapFrame, string rootPath)
        {
            string compareImagesDirectory = null;
            try
            {
                string jsonResponse = _recognitionService.GetFaceResponse(dataServiceDto, URL, minSize, compareImagesDirectory, gapFrame, rootPath);
                if (string.IsNullOrEmpty(jsonResponse))
                    return;

                var results = ParseDetectionResults(jsonResponse);
                if (results == null || results.Count == 0)
                {
                    _notificationService.Notify("Lỗi", "⚠️ API không chứa danh sách kết quả.", InformationType.Error);
                    return;
                }

                BuildRecognitionObjectsFromResults(results, recognition, rootPath);
            }
            catch (Exception ex)
            {
                _notificationService.Notify("Lỗi", $"❌ Lỗi khi gọi API DetectFacesAsync: {ex.Message}", InformationType.Error);
            }
        }

        public JArray ParseDetectionResults(string jsonResponse)
        {
            try
            {
                var json = JObject.Parse(jsonResponse);
                return json["results"] as JArray;
            }
            catch (Exception ex)
            {
                _notificationService.Notify("Lỗi", $"❌ JSON không hợp lệ: {ex.Message}", InformationType.Error);
                return null;
            }
        }

        public static void BuildRecognitionObjectsFromResults(JArray results, Recognition recognition, string rootPath)
        {
            int count = 0;
            string userName = SecuritySystem.CurrentUserName;
            var fileFrameRates = new Dictionary<string, double>();

            foreach (var face in results)
            {
                var recognitionObject = new RecognitionObject(recognition.Session)
                {
                    Recognition = recognition,
                    Order = ++count,
                    Name = $"Person_{count}"
                };

                var recognitionPositions = new List<RecognitionPosition>();

                var fileData = face["files"]?.ToObject<List<JObject>>() ?? new List<JObject>();

                foreach (var file in fileData)
                {
                    recognitionPositions.AddRange(ParseRecognitionPositions(file, recognitionObject, recognition, rootPath, fileFrameRates));
                }

                recognitionObject.RecognitionPositionList.AddRange(recognitionPositions);

                // Ảnh đại diện là position có yaw nhỏ nhất
                var best = recognitionPositions.OrderBy(p => Math.Abs(p.Yaw ?? 0)).FirstOrDefault();
                if (best != null)
                {
                    recognitionObject.Reliability = best.Reliability;
                    recognitionObject.Size = best.Size;
                    recognitionObject.Frame = best.ImageFrame;
                    recognitionObject.Image = best.Image;
                    recognitionObject.RecognitionPosition = best;
                }
            }
        }

        public static IEnumerable<RecognitionPosition> ParseRecognitionPositions(JObject file, RecognitionObject recognitionObject, Recognition recognition, string rootPath, Dictionary<string, double> fileFrameRates)
        {
            string fileName = file["fileName"]?.ToString() ?? "";
            string ytPath = Path.Combine(rootPath, "recognize", "upload");

            var frameObjects = file["frames"]?.ToObject<List<JObject>>() ?? new List<JObject>();

            foreach (var frameData in frameObjects)
            {
                int frameIdx = frameData["frame_idx"]?.ToObject<int>() ?? 0;
                int beginFrame = frameData["begin_frame"]?.ToObject<int>() ?? 0;
                int endFrame = frameData["end_frame"]?.ToObject<int>() ?? 0;
                int x = frameData["x"]?.ToObject<int>() ?? 0;
                int y = frameData["y"]?.ToObject<int>() ?? 0;
                int size = frameData["size"]?.ToObject<int>() ?? 0;
                decimal reliability = frameData["confidence"]?.ToObject<decimal>() ?? 0.0m;
                decimal yaw = frameData["face"]?["pose"]?["yaw"]?.ToObject<decimal>() ?? 0.0m;
                decimal roll = frameData["face"]?["pose"]?["roll"]?.ToObject<decimal>() ?? 0.0m;

                var bookMark = recognition.BookMarkList.FirstOrDefault(x => x.Oid.ToString() == frameData["link"]?.ToString());
                string fileLink = GetFileLink(bookMark, fileName, ytPath);

                // Lấy frame rate
                if (!fileFrameRates.ContainsKey(fileLink))
                {
                    fileFrameRates[fileLink] = fileLink.EndsWith(".mp4") || fileLink.EndsWith(".avi") || fileLink.EndsWith(".mov") || fileLink.EndsWith(".mkv")
                        ? Module.Helpers.AudioVideoHelper.GetFrameRate(fileLink)
                        : 0.0;
                }

                double secondsPerFrame = 1.0 / (fileFrameRates[fileLink] > 0 ? fileFrameRates[fileLink] : 25);

                yield return new RecognitionPosition(recognitionObject.Session)
                {
                    Link = fileLink,
                    BeginFrame = beginFrame,
                    EndFrame = endFrame,
                    ImageFrame = frameIdx,
                    Horizontal = x,
                    Vertical = y,
                    Size = size,
                    Reliability = reliability,
                    Begin = TimeSpan.FromSeconds(beginFrame * secondsPerFrame),
                    End = TimeSpan.FromSeconds(endFrame * secondsPerFrame),
                    Yaw = yaw,
                    Roll = roll,
                    Image = !string.IsNullOrEmpty(frameData["face"]?["img"]?.ToString())
                        ? Convert.FromBase64String(frameData["face"]["img"].ToString())
                        : null
                };
            }
        }


        #endregion SourceCode3438ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object URLToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Image != null) 
		//			return Image;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractorDataListToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (ExtractorDataList != null) 
		//			return ExtractorDataList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContactToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Contact != null) 
		//			return Contact;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrgToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Org != null) 
		//			return Org;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AssetToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Asset != null) 
		//			return Asset;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProductListingToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (ProductListing != null) 
		//			return ProductListing;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Video != null) 
		//			return Video;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FolderToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Folder != null) 
		//			return Folder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProductToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Product != null) 
		//			return Product;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PostToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Post != null) 
		//			return Post;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WebsiteToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Website != null) 
		//			return Website;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinkTypeToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (LinkType != null) 
		//			return LinkType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WorkTypeToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (WorkType != null) 
		//			return WorkType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectIDToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (ObjectID != null) 
		//			return ObjectID;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareObjectTypeToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SourceCodeToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (SourceCode != null) 
		//			return SourceCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object XpathToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Xpath != null) 
		//			return Xpath;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Recognition != null) 
		//			return Recognition;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpaceToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Space != null) 
		//			return Space;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InvesterToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Invester != null) 
		//			return Invester;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CompanyToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Company != null) 
		//			return Company;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AIExtractorToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (AIExtractor != null) 
		//			return AIExtractor;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EquipmentToolTipControllerText(View view, Module.BusinessObjects.BookMark bookmark)
        //{
        //    if (Equipment != null) 
		//			return Equipment;
        //    return null;
        //}
    

	    #endregion
  

    }
}
