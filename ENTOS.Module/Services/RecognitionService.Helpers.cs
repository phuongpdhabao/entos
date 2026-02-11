namespace ENTOS.Module.Services
{
    public partial class RecognitionService
    {
        #region ProcessVideo Helpers

        private static string ProcessVideo_GetSafeVideoName(string videoName, string extension)
        {
            string safeName = Module.Helpers.FileSystemHelper.SanitizeFileName(videoName);
            if (string.IsNullOrWhiteSpace(safeName))
            {
                return System.Guid.NewGuid().ToString();
            }
            return safeName + "__" + extension;
        }

        private static int ProcessVideo_GetFrameDigits(int totalFrameCount)
        {
            return System.Math.Max(3, totalFrameCount.ToString().Length);
        }

        private static string ProcessVideo_BuildFfmpegArguments(string videoPath, string outputFolder)
        {
            return string.Format("-i \"{0}\" -vf \"select='eq(pict_type,PICT_TYPE_I)',showinfo\" -vsync vfr \"{1}\\frame_n%04d.jpg\"",
                videoPath, outputFolder);
        }

        private static int ProcessVideo_CalculateFrameNumber(double ptsTime, double fps)
        {
            if (ptsTime % 1 == 0)
            {
                return (int)ptsTime;
            }
            return (int)System.Math.Round(ptsTime * fps);
        }

        private static string ProcessVideo_BuildFramePath(string outputFolder, int frameNumber, int frameDigits)
        {
            return System.IO.Path.Combine(outputFolder, "frame_" + frameNumber.ToString().PadLeft(frameDigits, '0') + ".jpg");
        }

        private static string ProcessVideo_BuildSourceFramePath(string outputFolder, int n)
        {
            return System.IO.Path.Combine(outputFolder, string.Format("frame_n{0:D4}.jpg", n));
        }

        private static bool ProcessVideo_ShouldExtractExtraFrame(int distance, int gapFrame)
        {
            return distance > gapFrame;
        }

        #endregion

        #region ProcessImageFolder Helpers

        private static bool ProcessImageFolder_IsImageFile(string extension)
        {
            string ext = extension.ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".webp" || ext == ".bmp";
        }

        private static string ProcessImageFolder_BuildOutputPath(string outputFolder, int imageIndex)
        {
            return System.IO.Path.Combine(outputFolder, imageIndex.ToString("D4") + ".jpg");
        }

        #endregion

        #region ProcessSingleImage Helpers

        private static string ProcessSingleImage_BuildOutputFileName(int imageIndex)
        {
            return imageIndex.ToString("D4") + ".jpg";
        }

        #endregion

        #region DownloadYouTubeVideo Helpers

        private static bool DownloadYouTubeVideo_IsValidUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && Module.Utils.YouTubeUtils.IsYoutubeUrl(url);
        }

        private static string DownloadYouTubeVideo_BuildOutputPath(string rootPath, string userName, string videoId)
        {
            return System.IO.Path.Combine(rootPath, userName, videoId + ".mp4");
        }

        #endregion

        #region File Path Helpers

        private static string GetFileExtensionLowerTrimmed(string filePath)
        {
            return System.IO.Path.GetExtension(filePath)?.TrimStart('.').ToLower();
        }

        private static string GetFileNameWithoutExtensionSafe(string filePath)
        {
            return System.IO.Path.GetFileNameWithoutExtension(filePath);
        }

        private static bool FileExistsAtPath(string path)
        {
            return System.IO.File.Exists(path);
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}
