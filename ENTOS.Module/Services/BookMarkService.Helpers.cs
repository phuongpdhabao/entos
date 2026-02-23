namespace ENTOS.Module.Services
{
    public partial class BookMarkService
    {
        private static (string RecognizeFolder, string UserFolder, string FileFolder, string SessionFolder) BuildRecognitionFolders(string rootPath, string userName, string fileCode)
        {
            string recognizeFolder = System.IO.Path.Combine(rootPath, "recognize");
            string userFolder = System.IO.Path.Combine(recognizeFolder, userName);
            string fileFolder = System.IO.Path.Combine(userFolder, fileCode);
            string timeFolder = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string sessionFolder = System.IO.Path.Combine(fileFolder, timeFolder);
            return (recognizeFolder, userFolder, fileFolder, sessionFolder);
        }

        private static bool IsImageExtension(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";
        }

        private static bool IsVideoExtension(string extension)
        {
            return extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".mkv";
        }
    }
}
