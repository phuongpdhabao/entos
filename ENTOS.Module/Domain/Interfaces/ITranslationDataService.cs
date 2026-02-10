using ENTOS.Application.Features.TranslateText;

namespace ENTOS.Domain.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ dịch thuật văn bản.
    /// </summary>
    public interface ITranslationDataService : IDataServiceHandle
    {

        Task ParseTranslateResponse(TranslateSegmentBase segment, HttpContent httpContent);
        Task ParseTranslateBlockResponse(TranslateSegmentBlock block, HttpContent httpContent, string startWrapper = "<li>", string endWrapper = "</li>\n");
        /// <summary>
        /// Dịch một đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích.
        /// </summary>
        /// <param name="text">Văn bản cần dịch</param>
        /// <param name="targetLanguage">Mã ngôn ngữ đích (ví dụ: "vi", "en")</param>
        /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null, ví dụ: "en")</param>
        /// <param name="prompt">Prompt cho AI để hỗ trợ dịch thuật (có thể null)</param>
        /// <returns>Văn bản đã được dịch</returns>
        Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, string prompt = null);

        /// <summary>
        /// Dịch nhiều câu văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích.
        /// </summary>
        /// <param name="texts">Mảng văn bản cần dịch</param>
        /// <param name="targetLanguage">Mã ngôn ngữ đích</param>
        /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null)</param>
        /// <param name="prompt">Prompt cho AI để hỗ trợ dịch thuật (có thể null)</param>
        /// <returns>Mảng văn bản đã được dịch</returns>
        Task<string[]> TranslateAsync(string[] texts, string targetLanguage, string sourceLanguage = null, string prompt = null);

        /// <summary>
        /// Phát hiện ngôn ngữ của một đoạn văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần phát hiện ngôn ngữ</param>
        /// <returns>Mã ngôn ngữ phát hiện được (ví dụ: "en", "vi")</returns>
        Task<string> DetectLanguageAsync(string text);

        /// <summary>
        /// Dịch mảng văn bản theo ngữ cảnh, ghép các phần tử thành từng đoạn (batch) với độ dài tối đa, dịch từng batch, sau đó tách kết quả.
        /// Dữ liệu sẽ được xử lý trực tiếp trong List<TranslateSegmentBase> được truyền vào.
        /// </summary>
        /// <param name="segments">Danh sách segments cần dịch (sẽ được cập nhật kết quả trực tiếp)</param>
        /// <param name="targetLanguage">Mã ngôn ngữ đích</param>
        /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null)</param>
        /// <param name="maxConcurrency">Số lượng tối đa các task đồng thời</param>
        /// <param name="urlEncode">Nếu dùng Http Get Method thì phải mã hóa url</param> 
        /// <param name="context">Hiển thị phần trăm</param> 
        /// <param name="prompt">Prompt cho AI để hỗ trợ dịch thuật (có thể null)</param>
        Task TranslateAsync(
            List<TranslateSegmentBase> segments,
            string targetLanguage,
            string sourceLanguage = null,
            int maxConcurrency = 10,
            bool urlEncode = false,
            Module.SystemObjects.LongTaskContext context = null,
            string prompt = null);

        /// <summary>
        /// Dịch mảng văn bản theo ngữ cảnh, ghép các phần tử thành từng đoạn (batch) với độ dài tối đa, dịch từng batch, sau đó tách kết quả.
        /// Dữ liệu sẽ được xử lý trực tiếp trong List<TranslateSegmentBase> được truyền vào.
        /// </summary>
        /// <param name="segments">Danh sách segments cần dịch (sẽ được cập nhật kết quả trực tiếp)</param>
        /// <param name="targetLanguage">Mã ngôn ngữ đích</param>
        /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null)</param>
        /// <param name="maxBatchLength">Độ dài tối đa của mỗi batch (ký tự)</param>
        /// <param name="tagName">Tên tag XML để phân tách</param>
        /// <param name="maxConcurrency">Số lượng tối đa các task đồng thời</param>
        /// <param name="urlEncode">Nếu dùng Http Get Method thì phải mã hóa url</param> 
        /// <param name="context">Hiển thị phần trăm</param> 
        /// <param name="prompt">Prompt cho AI để hỗ trợ dịch thuật (có thể null)</param>
        Task TranslateContextAsync(
            List<TranslateSegmentBase> segments,
            string targetLanguage,
            string sourceLanguage = null,
            int maxBatchLength = 4000,
            string tagName = "␟␟␟",
            int maxConcurrency = 10,
            bool urlEncode = false,
            Module.SystemObjects.LongTaskContext context = null,
            string prompt = null);


    }
}