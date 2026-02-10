namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface gửi các yêu cầu HTTP (GET, POST), tải lên/xuống file qua HTTP.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Gửi yêu cầu GET.
        /// </summary>
        Task<string> GetAsync(string url, IDictionary<string, string> headers = null);
        /// <summary>
        /// Gửi yêu cầu POST.
        /// </summary>
        Task<string> PostAsync(string url, object data, IDictionary<string, string> headers = null);
        /// <summary>
        /// Tải file từ server qua HTTP.
        /// </summary>
        Task<byte[]> DownloadFileAsync(string url, IDictionary<string, string> headers = null);
        /// <summary>
        /// Tải file lên server qua HTTP.
        /// </summary>
        Task UploadFileAsync(string url, byte[] data, IDictionary<string, string> headers = null);
    }
} 