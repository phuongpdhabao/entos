namespace ENTOS.Module.Helpers
{
    public static partial class NameHelper
    {
        //Thuật toán chống ghi đè file
        /// <summary>
        /// Tạo tên file duy nhất để tránh ghi đè file hiện có.
        /// Thêm số thứ tự vào tên file nếu file đã tồn tại.
        /// </summary>
        /// <param name="fileName">Tên file gốc</param>
        /// <returns>Tên file duy nhất</returns>
        /// <example>
        /// string uniqueName = Tools.GetUniqueFileName("document.txt");
        /// // Nếu document.txt đã tồn tại, kết quả: "document (1).txt"
        /// // Nếu document (1).txt cũng tồn tại, kết quả: "document (2).txt"
        /// </example>
        public static string GetUniqueFileName(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return fileName;
            string path = System.IO.Path.GetDirectoryName(fileName);
            string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);
            int i = 1;
            while (System.IO.File.Exists(fileName))
            {
                fileName = System.IO.Path.Combine(path, name + " (" + i + ")" + extension);
                i++;
            }
            return fileName;
        }


        /// <summary>
        /// Tạo tên file từ URL trong thư mục chỉ định.
        /// </summary>
        /// <param name="url">URL cần tạo tên file</param>
        /// <param name="folder">Thư mục chứa file</param>
        /// <param name="createFolder">Có tạo thư mục nếu chưa tồn tại không</param>
        /// <returns>Đường dẫn file đầy đủ</returns>
        public static string GetFileName(string url, string folder, bool createFolder = false)
        {
            System.Uri myUri = new System.Uri(url);
            string directory = folder + "\\" + myUri.Host;
            if (createFolder && !System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            var fileName = directory + @"\";
            fileName += GetFileName(url);
            return fileName;
        }

        /// <summary>
        /// Tạo tên file từ URL.
        /// </summary>
        /// <param name="url">URL cần tạo tên file</param>
        /// <returns>Tên file được tạo từ URL</returns>
        public static string GetFileName(string url)
        {
            System.Uri myUri = new System.Uri(url);
            var fileName = "";
            if (!string.IsNullOrEmpty(myUri.PathAndQuery))
                fileName += myUri.PathAndQuery.Substring(1).Replace('\\', ';').Replace('/', ';').Replace('*', ';').Replace('?', ';').Replace('<', ';').Replace('>', ';').Replace('|', ';');
            else
                fileName += System.Guid.NewGuid();
            if (fileName.Length > 210)
                fileName = fileName.Substring(0, 210);
            if (fileName.EndsWith(".html") || fileName.EndsWith(".htm"))
                return fileName;
            //var fileInfo = new System.IO.FileInfo(url);
            //if (string.IsNullOrEmpty(fileInfo.Extension))
            fileName += ".html";
            return fileName;
        }

    }
}
