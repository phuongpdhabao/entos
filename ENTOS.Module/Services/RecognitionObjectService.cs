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

    public partial class RecognitionObjectService : BaseService
    {

        public RecognitionObjectService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public RecognitionObjectService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3446ImportCode
                           public static byte[]? GenerateAvatar(string filePath, int x, int y, int size, int frameIdx)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(filePath));
        File.Copy(filePath, tempFilePath, true);

        try
        {
            int newWidth = (int)(size * 1.8);
            int newHeight = (int)(size * 2.4);
            int newX = Math.Max(0, x - (newWidth - size) / 2);
            int newY = Math.Max(0, y - (newHeight - size) / 2);

            OpenCvSharp.Mat avatarMat = null;

            if (IsImageFile(tempFilePath))
            {
                using var mat = OpenCvSharp.Cv2.ImRead(tempFilePath);
                if (mat.Empty()) return null;

                AdjustCropSize(mat.Width, mat.Height, ref newX, ref newY, ref newWidth, ref newHeight);
                var cropRect = new OpenCvSharp.Rect(newX, newY, newWidth, newHeight);
                avatarMat = new OpenCvSharp.Mat(mat, cropRect).Clone();
            }
            else if (Module.Helpers.MediaHelper.CheckVideoSupport(tempFilePath))
            {
                using var capture = new OpenCvSharp.VideoCapture(tempFilePath);
                if (!capture.IsOpened() || frameIdx < 0 || frameIdx >= capture.FrameCount)
                    return null;

                if (!capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, frameIdx))
                    return null;

                using var mat = new OpenCvSharp.Mat();
                if (capture.Read(mat) && !mat.Empty())
                {
                    AdjustCropSize(mat.Width, mat.Height, ref newX, ref newY, ref newWidth, ref newHeight);
                    var cropRect = new OpenCvSharp.Rect(newX, newY, newWidth, newHeight);
                    avatarMat = new OpenCvSharp.Mat(mat, cropRect).Clone();
                }
            }

            if (avatarMat != null)
            {
                using var ms = avatarMat.ToMemoryStream(".jpg");
                return ms.ToArray();
            }

            return null;
        }
        finally
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }

        #endregion SourceCode3446ImportCode

        #region SourceCode4521ImportCode
                        internal static string GenerateSilence(double durationSeconds, int index)
{
    string output = Path.Combine(Path.GetTempPath(), $"silence_{index}_{DateTime.Now:yyyyMMddHHmmss}.m4a");
    var args = $"-f lavfi -i anullsrc=r=44100:cl=stereo -t {durationSeconds.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -acodec aac -b:a 128k \"{output}\"";
    var (o, e, code) = RunFFmpegCommand(args);
    return code == 0 && File.Exists(output) ? output : null;
}

internal static void ConcatenateAudioFiles(string listFilePath, string outputPath)
{
    var args = $"-avoid_negative_ts make_zero -f concat -safe 0 -i \"{listFilePath}\" -c copy \"{outputPath}\"";

    var result = RunFFmpegCommand(args);

    if (!string.IsNullOrWhiteSpace(result.error))
    {
        // In lỗi chi tiết từ FFmpeg
        Console.WriteLine("❌ FFmpeg Error: " + result.error);
    }

    // Kiểm tra mã thoát của FFmpeg (0 là thành công)
    if (result.exitCode != 0)
    {
        Console.WriteLine("❌ Lỗi khi ghép các file âm thanh.");
    }
    else
    {
        Console.WriteLine("✅ Ghép âm thanh thành công!");
    }
}
internal static string ExtractAudioSegment(string videoPath, int beginFrame, int endFrame, int fps, int index)
{
    // Tính thời gian bắt đầu và độ dài đoạn âm thanh (đơn vị: giây)
    double startTime = beginFrame / (double)fps;
    double duration = (endFrame - beginFrame + 1) / (double)fps;

    // Tạo file tạm để chứa đoạn audio
    string outputPath = Path.Combine(Path.GetTempPath(), $"audio_part_{index}_{DateTime.Now:yyyyMMddHHmmss}.m4a");

    var args = $"-y -i \"{videoPath}\" -ss {startTime.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -t {duration.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -vn -acodec aac -b:a 128k \"{outputPath}\"";
    var (output, error, exitCode) = RunFFmpegCommand(args);

    if (!string.IsNullOrWhiteSpace(error))
    {
        Console.WriteLine("❌ FFmpeg Error: " + error);
    }

    return exitCode == 0 && File.Exists(outputPath) ? outputPath : null;
}
internal void MergeAudioToVideo(string videoFilePath, string audioFilePath)
{
    string tempOutput = Path.Combine(Path.GetTempPath(), $"merged_{DateTime.Now:yyyyMMddHHmmss}.mp4");

    var args = $"-i \"{videoFilePath}\" -i \"{audioFilePath}\" -c:v copy -c:a aac -shortest \"{tempOutput}\"";
    var (output, error, exitCode) = RunFFmpegCommand(args);

    if (!string.IsNullOrWhiteSpace(error))
    {
        Console.WriteLine("❌ FFmpeg Error: " + error);
    }

    if (exitCode == 0)
    {
        File.Delete(videoFilePath);
        File.Move(tempOutput, videoFilePath);
    }
    else
    {
        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "❌ Ghép âm thanh thất bại.", InformationType.Error);
    }
}
internal static (string output, string error, int exitCode) RunFFmpegCommand(string arguments)
{
    var psi = new System.Diagnostics.ProcessStartInfo("ffmpeg", arguments)
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = new System.Diagnostics.Process { StartInfo = psi };
    process.Start();

    // Read the output and error streams concurrently
    var outputTask = Task.Run(() => process.StandardOutput.ReadToEnd());
    var errorTask = Task.Run(() => process.StandardError.ReadToEnd());

    process.WaitForExit();

    // Wait for both tasks to complete
    Task.WhenAll(outputTask, errorTask).Wait();

    return (outputTask.Result, errorTask.Result, process.ExitCode);
}
internal string CreateTemporaryFile(string path, int index)
{
    try
    {
        string extension = Path.GetExtension(path);

        string tempDir = Path.Combine(Path.GetTempPath(), "VideoProcessing");
        Directory.CreateDirectory(tempDir);

        string tempPath = Path.Combine(tempDir, $"{index}{extension}");

        File.Copy(path, tempPath, true);

        return tempPath;
    }
    catch (Exception ex)
    {
        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi tạo tệp tạm thời: {ex.Message}", InformationType.Error);
        return path; // Trả về đường dẫn gốc nếu không thể tạo tệp tạm thời
    }
}
internal void DeleteTemporaryFile(string tempPath)
{
    try
    {
        if (File.Exists(tempPath))
        {
            File.Delete(tempPath);  // Xóa tệp tạm
        }
    }
    catch (Exception ex)
    {
        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi xóa tệp tạm: {ex.Message}", InformationType.Error);
    }
}
private static string RemoveUnicode(string input)
{
    // Đưa chuỗi về dạng chuẩn FormD (nối các dấu vào ký tự cơ bản)
    var formD = input.Normalize(System.Text.NormalizationForm.FormD);

    // Thay thế các ký tự có dấu thành ký tự không dấu, bao gồm chữ Đ và đ
    var normalized = new System.Text.StringBuilder();
    foreach (var c in formD)
    {
        // Thay thế các ký tự Đ -> D và đ -> d
        if (c == 'Đ')
            normalized.Append('D');
        else if (c == 'đ')
            normalized.Append('d');
        else
            normalized.Append(c);
    }

    // Dùng Regex để loại bỏ các ký tự không phải là chữ cái, số và dấu cách
    var regEx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
    return regEx.Replace(normalized.ToString(), string.Empty).Normalize(System.Text.NormalizationForm.FormC);
}
internal static  void DrawInfo(OpenCvSharp.Mat frame, string name, RecognitionPosition pos, float timestamp)
{
    var rect = new OpenCvSharp.Rect(pos.Horizontal.Value, pos.Vertical.Value, pos.Size.Value, pos.Size.Value);
    OpenCvSharp.Cv2.Rectangle(frame, rect, OpenCvSharp.Scalar.Red, 2);

    // Loại bỏ dấu trong tên
    string cleanedName = RemoveUnicode(name);

    // Đổi giây thành TimeSpan rồi format hh:mm:ss
    var time = TimeSpan.FromSeconds(timestamp);
    string label = $"{cleanedName} - {time:hh\\:mm\\:ss}";

    OpenCvSharp.Cv2.PutText(frame, label, new OpenCvSharp.Point(rect.X, rect.Y - 10),
        OpenCvSharp.HersheyFonts.HersheySimplex, 0.8, OpenCvSharp.Scalar.Yellow, 2);
}

internal static bool IsVideoFile(string path)
{
    var extensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
    return extensions.Contains(Path.GetExtension(path).ToLower());
}
internal static OpenCvSharp.Mat ResizeWithPadding(OpenCvSharp.Mat image, int targetWidth, int targetHeight)
{
    // Lấy kích thước gốc của ảnh
    var originalWidth = image.Width;
    var originalHeight = image.Height;

    // Tính tỷ lệ giữa chiều rộng và chiều cao
    float aspectRatio = (float)originalWidth / originalHeight;

    // Tính kích thước mới của ảnh để giữ tỷ lệ
    int newWidth, newHeight;

    if (originalWidth > originalHeight)
    {
        newWidth = targetWidth;
        newHeight = (int)(newWidth / aspectRatio);
    }
    else
    {
        newHeight = targetHeight;
        newWidth = (int)(newHeight * aspectRatio);
    }

    // Resize ảnh
    var resized = new OpenCvSharp.Mat();
    OpenCvSharp.Cv2.Resize(image, resized, new OpenCvSharp.Size(newWidth, newHeight));

    // Tạo một background trắng với kích thước targetWidth x targetHeight
    var result = new OpenCvSharp.Mat(new OpenCvSharp.Size(targetWidth, targetHeight), OpenCvSharp.MatType.CV_8UC3, new OpenCvSharp.Scalar(255, 255, 255));

    // Tính toán vị trí để đặt ảnh resized vào trung tâm của background
    int offsetX = (targetWidth - newWidth) / 2;
    int offsetY = (targetHeight - newHeight) / 2;

    // Chèn ảnh vào background
    resized.CopyTo(result[new OpenCvSharp.Rect(offsetX, offsetY, newWidth, newHeight)]);

    return result;
		}


        #endregion SourceCode4521ImportCode

        #region SourceCode3450ImportCode
          public static bool IsImageFile(string path) => Path.GetExtension(path).ToLower() is ".jpg" or ".jpeg" or ".png" or ".bmp";
        #endregion SourceCode3450ImportCode

        #region SourceCode3448ImportCode
            public static void AdjustCropSize(int imgWidth, int imgHeight, ref int x, ref int y, ref int width, ref int height)
    {
        if (x + width > imgWidth) width = imgWidth - x;
        if (y + height > imgHeight) height = imgHeight - y;
        if (width <= 0 || height <= 0)
        {
            width = 1;
            height = 1;
        }
    }
        #endregion SourceCode3448ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionTypeToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionType != null) 
		//			return RecognitionType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Image != null) 
		//			return Image;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ReliabilityToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Reliability != null) 
		//			return Reliability;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SizeToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Size != null) 
		//			return Size;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionPositionListToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionPositionList != null) 
		//			return RecognitionPositionList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Recognition != null) 
		//			return Recognition;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FrameToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Frame != null) 
		//			return Frame;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionPositionToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionPosition != null) 
		//			return RecognitionPosition;
        //    return null;
        //}
    

	    #endregion
  

    }
}
