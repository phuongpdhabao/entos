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
using System.IO.Compression;

 
namespace ENTOS.Module.Services 
{

    public partial class RecognitionService : BaseService
    {

        public RecognitionService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public RecognitionService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3396ImportCode
                        public static int ProcessVideo(string videoPath, string outputFolder, int imageIndex, int gapFrame)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            string originalFileName = System.IO.Path.GetFileName(videoPath);
            string videoName = System.IO.Path.GetFileNameWithoutExtension(videoPath);
            string extension = System.IO.Path.GetExtension(videoPath)?.TrimStart('.').ToLower();

            string safeVideoPath = videoPath;
            string tempVideoPath = string.Empty;

            if (Module.Helpers.TextHelper.CheckUnicode(originalFileName))
            {
                tempVideoPath = PreparePathUnUnicode(videoPath);
                safeVideoPath = tempVideoPath;
            }

            string tempFolderName = $"{Module.Helpers.FileSystemHelper.SanitizeFileName(videoName)}__{extension}";
            if (string.IsNullOrWhiteSpace(tempFolderName))
                tempFolderName = System.Guid.NewGuid().ToString();
            string tempOutputFolder = System.IO.Path.Combine(outputFolder, tempFolderName);
            if (!System.IO.Directory.Exists(tempOutputFolder))
                System.IO.Directory.CreateDirectory(tempOutputFolder);

            string originalFolderName = $"{videoName}__{extension}";
            string finalOutputFolder = System.IO.Path.Combine(outputFolder, originalFolderName);

            string ffmpegPath = "ffmpeg";
            string arguments = $"-i \"{safeVideoPath}\" -vf \"select='eq(pict_type,PICT_TYPE_I)',showinfo\" -vsync vfr \"{tempOutputFolder}\\frame_n%04d.jpg\"";

            try
            {
                int totalFrameCount = Module.Helpers.AudioVideoHelper.GetTotalFrameCount(videoPath);
                int frameDigits = ComputeFrameDigits(totalFrameCount); // luôn tối thiểu 3 chữ số

                var processInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var errorLines = new System.Collections.Generic.List<string>();
                using (var process = new System.Diagnostics.Process { StartInfo = processInfo })
                {
                    process.Start();
                    while (!process.StandardError.EndOfStream)
                    {
                        var line = process.StandardError.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line) && line.Contains("showinfo"))
                            errorLines.Add(line);
                    }
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                        return imageIndex;
                }

                double fps = Module.Helpers.AudioVideoHelper.GetFrameRate(videoPath);
                var regex = new System.Text.RegularExpressions.Regex(@"n:\s*(\d+).*pts_time:(\d+(\.\d+)?)");
                var frameNumbers = new System.Collections.Generic.List<int>();
                int renamedCount = 0;

                foreach (var line in errorLines)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        int n = int.Parse(match.Groups[1].Value) + 1;
                        double ptsTime = double.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

                        int frameNumber = ComputeFrameNumber(ptsTime, fps);

                        string sourcePath = System.IO.Path.Combine(tempOutputFolder, $"frame_n{n:D4}.jpg");
                        string destPath = System.IO.Path.Combine(tempOutputFolder, $"frame_{frameNumber.ToString().PadLeft(frameDigits, '0')}.jpg");

                        if (System.IO.File.Exists(sourcePath))
                        {
                            if (System.IO.File.Exists(destPath))
                                System.IO.File.Delete(destPath);

                            System.IO.File.Move(sourcePath, destPath);
                            frameNumbers.Add(frameNumber);
                            renamedCount++;
                        }
                    }
                }

                frameNumbers.Sort();
                int extraFrameCount = 0;

                for (int i = 0; i < frameNumbers.Count - 1; i++)
                {
                    int start = frameNumbers[i];
                    int end = frameNumbers[i + 1];
                    var extraFrames = ComputeExtraFrames(start, end, gapFrame);

                    foreach (var extraFrame in extraFrames)
                    {
                        double timeInSeconds = extraFrame / fps;
                        string outputImage = System.IO.Path.Combine(tempOutputFolder, $"frame_{extraFrame.ToString().PadLeft(frameDigits, '0')}.jpg");
                        string extractArgs = $"-ss {timeInSeconds.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)} -i \"{safeVideoPath}\" -frames:v 1 \"{outputImage}\"";

                        var extractInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = extractArgs,
                            CreateNoWindow = false,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        };

                        using (var extractProcess = new System.Diagnostics.Process { StartInfo = extractInfo })
                        {
                            extractProcess.Start();
                            var outputTask = System.Threading.Tasks.Task.Run(() => extractProcess.StandardOutput.ReadToEnd());
                            var errorTask = System.Threading.Tasks.Task.Run(() => extractProcess.StandardError.ReadToEnd());
                            extractProcess.WaitForExit();
                            string output = outputTask.Result;
                            string error = errorTask.Result;

                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                Console.WriteLine("FFmpeg error: " + error);
                            }

                            if (System.IO.File.Exists(outputImage))
                                extraFrameCount++;
                        }
                    }
                }

                imageIndex += renamedCount + extraFrameCount;
                stopwatch.Stop();

                if (!string.IsNullOrEmpty(tempVideoPath) && System.IO.File.Exists(tempVideoPath))
                {
                    System.IO.File.Delete(tempVideoPath);
                }

                if (!System.IO.Directory.Exists(finalOutputFolder))
                {
                    System.IO.Directory.Move(tempOutputFolder, finalOutputFolder);
                }
                else
                {
                    foreach (var file in System.IO.Directory.GetFiles(tempOutputFolder))
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        string destFile = System.IO.Path.Combine(finalOutputFolder, fileName);

                        if (System.IO.File.Exists(destFile))
                            System.IO.File.Delete(destFile); // Ghi đè nếu trùng

                        System.IO.File.Move(file, destFile);
                    }

                    System.IO.Directory.Delete(tempOutputFolder, true);
                }
                return imageIndex;
            }
            catch
            {
                return imageIndex;
            }
        }


        #endregion SourceCode3396ImportCode

        #region SourceCode3400ImportCode
                public static void ProcessImageFolder(string sourceFolder, string outputFolder, ref int imageIndex)
        {
            var files = System.IO.Directory.GetFiles(sourceFolder, "*.*")
                .Where(f => f.EndsWith(".jpg", System.StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpeg", System.StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".webp", System.StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => System.IO.File.GetCreationTime(f))
                .ToList();

            foreach (var file in files)
            {
                string originalFileName = System.IO.Path.GetFileName(file);

                string tempPath = file;
                string tempFilePath = string.Empty;
                if (Module.Helpers.TextHelper.CheckUnicode(originalFileName))
                {
                    tempFilePath = PreparePathUnUnicode(file); // ✅ truyền đường dẫn gốc
                    tempPath = tempFilePath;
                }

                var img = OpenCvSharp.Cv2.ImRead(tempPath);
                if (img.Empty())
                {
                    System.Console.WriteLine($"❌ Không thể đọc ảnh: {file}");
                    continue;
                }

                // ✅ Ghi lại đúng tên gốc
                string newFileName = System.IO.Path.Combine(outputFolder, originalFileName);
                OpenCvSharp.Cv2.ImWrite(newFileName, img);
                imageIndex++;

                if (!string.IsNullOrEmpty(tempFilePath) && System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }

        #endregion SourceCode3400ImportCode

        #region SourceCode3398ImportCode
                public static void ProcessSingleImage(string imagePath, string outputFolder, ref int imageIndex)
        {
            if (!System.IO.File.Exists(imagePath))
                return;

            string originalFileName = System.IO.Path.GetFileName(imagePath);

            string tempPath = imagePath;
            string tempFilePath = string.Empty;
            if (Module.Helpers.TextHelper.CheckUnicode(originalFileName))
            {
                tempFilePath = PreparePathUnUnicode(imagePath); // 🔥 truyền full path
                tempPath = tempFilePath;
            }

            var img = OpenCvSharp.Cv2.ImRead(tempPath);
            if (img.Empty())
            {
                System.Console.WriteLine($"❌ Không thể đọc ảnh: {imagePath}");
                return;
            }

            // ✅ Ghi lại đúng tên gốc
            string destPath = System.IO.Path.Combine(outputFolder, originalFileName);
            OpenCvSharp.Cv2.ImWrite(destPath, img);
            imageIndex++;

            if (!string.IsNullOrEmpty(tempFilePath) && System.IO.File.Exists(tempFilePath))
            {
                System.IO.File.Delete(tempFilePath);
            }
        }

        #endregion SourceCode3398ImportCode

        #region SourceCode3402ImportCode
                public static string DownloadYouTubeVideoToNetworkFolder(string url, string rootPath, string userName)
        {
            // Thư mục lưu trữ tạm thời
            string saveFolder = System.IO.Path.Combine(rootPath, "recognize", "upload");

            // Gọi hàm DownloadFromYoutube có sẵn trong dự án
            string outputPath = Module.Utils.YouTubeUtils.DownloadFromYoutube(url, saveFolder, true, null);

            if (string.IsNullOrEmpty(outputPath) || !System.IO.File.Exists(outputPath))
            {
                System.Console.WriteLine($"❌ Failed to download video from URL: {url}");
                return string.Empty;
            }

            System.Console.WriteLine($"✅ Video downloaded successfully: {outputPath}");
            return outputPath;
        }

        #endregion SourceCode3402ImportCode

        #region SourceCode3419ImportCode
                public static string PreparePathUnUnicode(string originalPath)
        {
            if (originalPath.All(c => c <= 127))
            {
                // Không có ký tự Unicode, dùng luôn
                return originalPath;
            }

            // Copy file sang thư mục tạm
            string fileExt = System.IO.Path.GetExtension(originalPath);
            string tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TempUnicodeSafe");
            System.IO.Directory.CreateDirectory(tempFolder);

            string tempFile = System.IO.Path.Combine(tempFolder, System.Guid.NewGuid().ToString() + fileExt);
            System.IO.File.Copy(originalPath, tempFile, true);

            return tempFile;
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
        }

        #endregion SourceCode3419ImportCode

        #region SourceCode3406ImportCode
                public static void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destDir));

            foreach (var filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
                File.Copy(filePath, filePath.Replace(sourceDir, destDir), true);
        }

        #endregion SourceCode3406ImportCode

        #region SourceCode3404ImportCode
                        public Module.Services.DataServiceService dataServiceService1 = null;
        public string GetFaceResponse(Application.DTOs.DataServiceDto dataServiceDto , string folderPath, int minSize, string compareImagesDirectory, int gapFrame, string rootPath)
        {
            string zipPath1 = null;
            string zipPath2 = null;
            try
            {
                // 1. Kiểm tra folder ảnh chính
                if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                {
                    _notificationService.Notify( "Lỗi", "❌ Thư mục ảnh chính không tồn tại!", InformationType.Error);
                    return null;
                }

                // 2. Copy folderPath vào server
                string guid = Guid.NewGuid().ToString("N");
                string serverFolder1 = Path.Combine(rootPath, "input_" + guid);
                Directory.CreateDirectory(serverFolder1);
                CopyDirectory(folderPath, serverFolder1);

                // 3. Tạo file zip tạm trên server
                zipPath1 = Path.Combine(rootPath, $"input_{guid}.zip");
                if (File.Exists(zipPath1))
                    File.Delete(zipPath1);

                ZipFile.CreateFromDirectory(serverFolder1, zipPath1, CompressionLevel.Fastest, false);

                // 4. Nếu có ảnh so sánh
                if (!string.IsNullOrEmpty(compareImagesDirectory) && Directory.Exists(compareImagesDirectory))
                {
                    string compareGuid = Guid.NewGuid().ToString("N");
                    string serverFolder2 = Path.Combine(rootPath, "compare_" + compareGuid);
                    Directory.CreateDirectory(serverFolder2);
                    CopyDirectory(compareImagesDirectory, serverFolder2);

                    zipPath2 = Path.Combine(rootPath, $"compare_{compareGuid}.zip");
                    if (File.Exists(zipPath2))
                        File.Delete(zipPath2);

                    ZipFile.CreateFromDirectory(serverFolder2, zipPath2, CompressionLevel.Fastest, false);
                }
                object[] inputs = new object[] { zipPath1, zipPath2, gapFrame.ToString(), minSize.ToString() };

                // 6. Gọi GetResult
                if (dataServiceService1 is null)
                    dataServiceService1 = new Module.Services.DataServiceService();

                var response = Task.Run(() =>dataServiceService1.GetResultAsync(dataServiceDto, inputs )).Result;
                return response.ToString();
            }
            catch (Exception ex)
            {
                _notificationService.Notify( "Lỗi", $"❌ Lỗi khi xử lý nhận diện khuôn mặt: {ex.Message}", InformationType.Error);
                return null;
            }
        }



        #endregion SourceCode3404ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
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
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BookMarkListToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (BookMarkList != null) 
		//			return BookMarkList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionObjectListToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (RecognitionObjectList != null) 
		//			return RecognitionObjectList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.Recognition recognition)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

	    #endregion
  

    }
}
