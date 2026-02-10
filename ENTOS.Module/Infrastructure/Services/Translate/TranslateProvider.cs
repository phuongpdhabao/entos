using ENTOS.Module.BusinessObjects;
using System.Text;

namespace ENTOS.Module.Services;

/// <summary>
/// Interface provider cho dịch vụ dịch thuật văn bản.
/// </summary>
public interface ITranslateProvider
{
    /// <summary>
    /// Dịch một đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích.
    /// </summary>
    /// <param name="text">Văn bản cần dịch</param>
    /// <param name="targetLanguage">Mã ngôn ngữ đích (ví dụ: "vi", "en")</param>
    /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null, ví dụ: "en")</param>
    /// <param name="prompt">Prompt tùy chỉnh cho việc dịch thuật (có thể null)</param>
    /// <returns>Văn bản đã được dịch</returns>
    Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, string prompt = null);

    /// <summary>
    /// Dịch hàng loạt các đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích.
    /// </summary>
    /// <param name="texts">Danh sách văn bản cần dịch</param>
    /// <param name="targetLanguage">Mã ngôn ngữ đích (ví dụ: "vi", "en")</param>
    /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null, ví dụ: "en")</param>
    /// <param name="prompt">Prompt tùy chỉnh cho việc dịch thuật (có thể null)</param>
    /// <returns>Danh sách văn bản đã được dịch</returns>
    Task<List<string>> TranslateBatchAsync(List<string> texts, string targetLanguage, string sourceLanguage = null, string prompt = null);

    /// <summary>
    /// Dịch theo ngữ cảnh - dịch nhiều văn bản cùng lúc để duy trì ngữ cảnh.
    /// </summary>
    /// <param name="texts">Mảng văn bản cần dịch</param>
    /// <param name="targetLanguage">Mã ngôn ngữ đích</param>
    /// <param name="sourceLanguage">Mã ngôn ngữ nguồn (có thể null)</param>
    /// <param name="prompt">Prompt tùy chỉnh cho việc dịch thuật (có thể null)</param>
    /// <param name="maxBatchLength">Độ dài tối đa của mỗi batch</param>
    /// <param name="separator">Ký tự phân cách giữa các văn bản</param>
    /// <returns>Mảng văn bản đã được dịch</returns>
    Task<string[]> TranslateContextAsync(string[] texts, string targetLanguage, string sourceLanguage = null, string prompt = null, int maxBatchLength = 4000, string separator = "␟␟␟");
}

/// <summary>
/// Triển khai provider cho dịch vụ dịch thuật văn bản (giả lập).
/// </summary>
public class TranslateProvider : ITranslateProvider
{
    /// <summary>
    /// Độ dài tối đa của văn bản cho phép dịch thuật.
    /// </summary>
    public const int MaxTextLength = 3000;

    /// <summary>
    /// Thời gian timeout mặc định cho việc dịch thuật (giây).
    /// </summary>
    public const int TimeoutSeconds = 300;

    private readonly DataService _dataService;
    private readonly DataServiceService _dataServiceService;

    private AutoMapper.IMapper _mapper;

    /// <summary>
    /// Khởi tạo TranslateProvider với DataService và DataServiceService.
    /// </summary>
    /// <param name="dataService">Dịch vụ dữ liệu</param>
    /// <param name="dataServiceService">Service xử lý dịch vụ</param>
    public TranslateProvider(DataService dataService, DataServiceService dataServiceService, AutoMapper.IMapper mapper)
    {
        _dataService = dataService;// ?? throw new ArgumentNullException(nameof(dataService));
        _dataServiceService = dataServiceService;// ?? throw new ArgumentNullException(nameof(dataServiceService));
        _mapper = mapper;
    }

    /// <summary>
    /// Dịch một đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích (giả lập, chỉ trả về text kèm thông tin ngôn ngữ).
    /// </summary>
    public async Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, string prompt = null)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // Nếu văn bản ngắn hơn MaxTextLength, dịch trực tiếp
        if (text.Length <= MaxTextLength)
        {
            return await BaseTranslateAsync(text, targetLanguage, sourceLanguage, prompt);
        }

        // Tách văn bản thành các đoạn nhỏ hơn theo dấu xuống dòng
        var segments = SplitTextIntelligently(text, MaxTextLength);
        var translatedSegments = new List<string>();

        foreach (var segment in segments)
        {
            var translatedSegment = await BaseTranslateAsync(segment, targetLanguage, sourceLanguage, prompt);
            translatedSegments.Add(translatedSegment);
        }

        // Ghép lại các đoạn đã dịch
        return JoinTranslatedSegments(translatedSegments);
    }

    /// <summary>
    /// Dịch hàng loạt các đoạn văn bản từ ngôn ngữ nguồn sang ngôn ngữ đích (giả lập).
    /// </summary>
    public async Task<List<string>> TranslateBatchAsync(List<string> texts, string targetLanguage, string sourceLanguage = null, string prompt = null)
    {
        var results = new List<string>();

        foreach (var text in texts)
        {
            var translatedText = await BaseTranslateAsync(text, targetLanguage, sourceLanguage, prompt);
            results.Add(translatedText);
        }

        return results;
    }

    /// <summary>
    /// Dịch theo ngữ cảnh - dịch nhiều văn bản cùng lúc để duy trì ngữ cảnh.
    /// </summary>
    public async Task<string[]> TranslateContextAsync(string[] texts, string targetLanguage, string sourceLanguage = null, string prompt = null, int maxBatchLength = 4000, string separator = "␟␟␟")
    {
        if (texts == null || texts.Length == 0) return Array.Empty<string>();

        var blocks = Helpers.TextHelper.SplitArrayToBlocks(texts, maxBatchLength, separator);
        var results = new List<string>();

        foreach (var block in blocks)
        {
            var batchResult = await BaseTranslateAsync(block, targetLanguage, sourceLanguage, prompt);
            if (batchResult.Length > 0)
            {
                var split = batchResult.Split(new[] { separator }, StringSplitOptions.None);
                results.AddRange(split);
            }
        }

        return results.ToArray();
    }

    /// <summary>
    /// Tách văn bản một cách thông minh theo dấu xuống dòng và độ dài tối đa.
    /// </summary>
    /// <param name="text">Văn bản cần tách</param>
    /// <param name="maxLength">Độ dài tối đa của mỗi đoạn</param>
    /// <returns>Danh sách các đoạn văn bản</returns>
    protected virtual List<string> SplitTextIntelligently(string text, int maxLength)
    {
        var segments = new List<string>();
        var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var currentSegment = new StringBuilder();

        foreach (var line in lines)
        {
            // Nếu thêm dòng này vượt quá giới hạn
            if (currentSegment.Length + line.Length + 1 > maxLength && currentSegment.Length > 0)
            {
                // Lưu đoạn hiện tại
                segments.Add(currentSegment.ToString().Trim());
                currentSegment.Clear();
            }

            // Thêm dòng vào đoạn hiện tại
            if (currentSegment.Length > 0)
                currentSegment.AppendLine();
            currentSegment.Append(line);
        }

        // Thêm đoạn cuối cùng
        if (currentSegment.Length > 0)
        {
            segments.Add(currentSegment.ToString().Trim());
        }

        // Nếu vẫn có đoạn quá dài, tách thêm theo từ
        var finalSegments = new List<string>();
        foreach (var segment in segments)
        {
            if (segment.Length <= maxLength)
            {
                finalSegments.Add(segment);
            }
            else
            {
                var words = segment.Split(' ');
                var currentChunk = new StringBuilder();

                foreach (var word in words)
                {
                    if (currentChunk.Length + word.Length + 1 > maxLength && currentChunk.Length > 0)
                    {
                        finalSegments.Add(currentChunk.ToString().Trim());
                        currentChunk.Clear();
                    }

                    if (currentChunk.Length > 0)
                        currentChunk.Append(' ');
                    currentChunk.Append(word);
                }

                if (currentChunk.Length > 0)
                {
                    finalSegments.Add(currentChunk.ToString().Trim());
                }
            }
        }

        return finalSegments;
    }

    /// <summary>
    /// Ghép lại các đoạn đã dịch thành văn bản hoàn chỉnh.
    /// </summary>
    /// <param name="translatedSegments">Danh sách các đoạn đã dịch</param>
    /// <returns>Văn bản hoàn chỉnh đã dịch</returns>
    protected virtual string JoinTranslatedSegments(List<string> translatedSegments)
    {
        if (translatedSegments == null || translatedSegments.Count == 0)
            return string.Empty;

        if (translatedSegments.Count == 1)
            return translatedSegments[0];

        var result = new StringBuilder();
        for (int i = 0; i < translatedSegments.Count; i++)
        {
            if (i > 0)
                result.AppendLine();
            result.Append(translatedSegments[i]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Phương thức cơ sở để xử lý dịch thuật thực tế.
    /// </summary>
    /// <param name="text">Văn bản cần dịch (đã được kiểm tra độ dài)</param>
    /// <param name="targetLanguage">Mã ngôn ngữ đích</param>
    /// <param name="sourceLanguage">Mã ngôn ngữ nguồn</param>
    /// <param name="prompt">Prompt tùy chỉnh</param>
    /// <returns>Văn bản đã được dịch</returns>
    protected virtual async Task<string> BaseTranslateAsync(string text, string targetLanguage, string sourceLanguage = null, string prompt = null)
    {
        if (_dataService is null || _dataServiceService is null)
        {
            return Module.SystemObjects.Tools.TranslateText(text, targetLanguage, sourceLanguage);
        }
        var dataServiceDto = _mapper.Map<Application.DTOs.DataServiceDto>(_dataService);
        var result = await _dataServiceService.GetResultAsync(dataServiceDto, new object[] { text, targetLanguage, sourceLanguage });
        return result as string ?? string.Empty;
    }
}