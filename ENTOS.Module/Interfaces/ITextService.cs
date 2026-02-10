namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface xử lý chuỗi: tách dòng, loại bỏ unicode, ký tự đặc biệt, kiểm tra viết hoa/thường, dịch, chuyển số, validate.
    /// </summary>
    public interface ITextService
    {
        /// <summary>
        /// Tách chuỗi thành mảng dòng.
        /// </summary>
        string[] SplitLines(string text);
        /// <summary>
        /// Loại bỏ ký tự unicode.
        /// </summary>
        string RemoveUnicode(string text);
        /// <summary>
        /// Loại bỏ ký tự đặc biệt.
        /// </summary>
        string RemoveSpecialCharacters(string text);
        /// <summary>
        /// Dịch chuỗi sang ngôn ngữ khác.
        /// </summary>
        string Translate(string text, string targetLanguage);
        /// <summary>
        /// Chuyển chuỗi sang số.
        /// </summary>
        int? ConvertToNumber(string text);
        /// <summary>
        /// Kiểm tra chuỗi theo pattern.
        /// </summary>
        bool Validate(string text, string pattern);
        /// <summary>
        /// Kiểm tra chuỗi có phải toàn chữ hoa.
        /// </summary>
        bool IsAllUpper(string text);
        /// <summary>
        /// Kiểm tra chuỗi có phải toàn chữ thường.
        /// </summary>
        bool IsAllLower(string text);
        /// <summary>
        /// Viết hoa chữ cái đầu tiên.
        /// </summary>
        string CapitalizeFirstLetter(string text);
    }
} 