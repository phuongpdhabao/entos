namespace ENTOS.Module.Services
{
    public partial class RecognitionPositionService
    {
        private static readonly string[] ImageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff" };
        private static readonly string[] VideoExtensions = new string[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv" };

        private static bool HasFileExtension(string path, string[] extensions)
        {
            string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            foreach (var extension in extensions)
            {
                if (ext == extension)
                    return true;
            }
            return false;
        }
    }
}
