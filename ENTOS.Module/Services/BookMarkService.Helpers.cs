namespace ENTOS.Module.Services
{
    public partial class BookMarkService
    {
        #region PreprocessDataForDetect Helpers

        private static string PreprocessDataForDetect_BuildSessionFolderPath(string rootPath, string userName, string fileCode, string timeFolder)
        {
            string recognizeFolder = System.IO.Path.Combine(rootPath, "recognize");
            string userFolder = System.IO.Path.Combine(recognizeFolder, userName);
            string fileFolder = System.IO.Path.Combine(userFolder, fileCode);
            return System.IO.Path.Combine(fileFolder, timeFolder);
        }

        private static string PreprocessDataForDetect_GetTimeFolder()
        {
            return System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        private static bool PreprocessDataForDetect_IsImageFile(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";
        }

        private static bool PreprocessDataForDetect_IsVideoFile(string extension)
        {
            return extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".mkv";
        }

        private static void PreprocessDataForDetect_EnsureDirectoryExists(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private static void PreprocessDataForDetect_CleanupSessionFolder(string sessionFolder)
        {
            if (System.IO.File.Exists(sessionFolder))
            {
                System.IO.File.Delete(sessionFolder);
            }
            else if (System.IO.Directory.Exists(sessionFolder))
            {
                System.IO.Directory.Delete(sessionFolder, true);
            }
        }

        #endregion

        #region File Path Helpers

        private static string GetFileExtensionLower(string filePath)
        {
            return System.IO.Path.GetExtension(filePath).ToLowerInvariant();
        }

        private static string BuildBookmarkFolderPath(string sessionFolder, System.Guid bookmarkOid)
        {
            return System.IO.Path.Combine(sessionFolder, bookmarkOid.ToString());
        }

        #endregion
    }
}
