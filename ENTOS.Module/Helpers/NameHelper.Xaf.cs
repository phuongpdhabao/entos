using DevExpress.Xpo;

namespace ENTOS.Module.Helpers
{
    public static partial class NameHelper
    {

        /// <summary>
        /// Lấy tên file cache cho URL từ thư mục cache.
        /// </summary>
        /// <param name="session">Session để lấy thông tin thư mục cache</param>
        /// <param name="url">URL cần tạo tên file cache</param>
        /// <returns>Tên file cache nếu tồn tại, null nếu không</returns>
        public static string GetCacheFileName(Session session, string url)
        {
            var folder = Module.Helpers.ParameterHelper.GetValueOrDefault(session, "CacheHtmlFolder", "\\\\dc\\Habao$\\Company\\HBD");
            var fileName = GetFileName(url, folder);
            if (System.IO.File.Exists(fileName))
                return fileName;
            return null;
        }



    }
}
