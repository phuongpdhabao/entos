using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho enum: lấy tên, giá trị, mô tả, parse, liệt kê, chuyển đổi.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Lấy tên enum.
        /// </summary>
        public static string GetName<T>(this T enumValue) where T : Enum
        {
            return Enum.GetName(typeof(T), enumValue);
        }

        /// <summary>
        /// Lấy giá trị int của enum.
        /// </summary>
        public static int ToInt<T>(this T enumValue) where T : Enum
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// Parse từ string sang enum (không phân biệt hoa thường).
        /// </summary>
        public static T ParseEnum<T>(this string value, T defaultValue = default) where T : struct, Enum
        {
            return Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Lấy mô tả (Description) của enum nếu có.
        /// </summary>
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attr?.Description ?? enumValue.ToString();
        }

        /// <summary>
        /// Liệt kê tất cả giá trị enum.
        /// </summary>
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Chuyển enum sang string.
        /// </summary>
        public static string ToStringValue<T>(this T enumValue) where T : Enum
        {
            return enumValue.ToString();
        }

        /// <summary>
        /// Kiểm tra giá trị có hợp lệ với enum không.
        /// </summary>
        public static bool IsDefinedEnumValue<T>(this T enumValue) where T : Enum
        {
            return Enum.IsDefined(typeof(T), enumValue);
        }

        // --- Flags Enum Helpers ---

        /// <summary>
        /// Kiểm tra enum có chứa flag.
        /// </summary>
        public static bool HasFlag<T>(this T enumValue, T flag) where T : Enum
        {
            return enumValue.HasFlag(flag);
        }

        /// <summary>
        /// Thêm flag vào enum.
        /// </summary>
        public static T AddFlag<T>(this T enumValue, T flag) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), enumValue.ToInt() | flag.ToInt());
        }

        /// <summary>
        /// Xóa flag khỏi enum.
        /// </summary>
        public static T RemoveFlag<T>(this T enumValue, T flag) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), enumValue.ToInt() & ~flag.ToInt());
        }

        /// <summary>
        /// Đảo ngược flag (thêm nếu chưa có, xóa nếu đã có).
        /// </summary>
        public static T ToggleFlag<T>(this T enumValue, T flag) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), enumValue.ToInt() ^ flag.ToInt());
        }

        /// <summary>
        /// Lấy danh sách các flag riêng lẻ đang được thiết lập.
        /// </summary>
        public static IEnumerable<T> GetFlags<T>(this T enumValue) where T : Enum
        {
            return GetValues<T>().Where(v => enumValue.HasFlag(v) && v.ToInt() != 0);
        }

        // --- Conversion & Parsing ---

        /// <summary>
        /// Tìm giá trị enum từ chuỗi mô tả [Description].
        /// </summary>
        public static T TryGetFromDescription<T>(this string description, T defaultValue = default) where T : struct, Enum
        {
            foreach (var value in GetValues<T>())
            {
                if (value.GetDescription().Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Chuyển enum thành Dictionary (int, string).
        /// </summary>
        public static Dictionary<int, string> ToDictionary<T>() where T : Enum
        {
            return GetValues<T>().ToDictionary(v => v.ToInt(), v => v.GetDescription());
        }

        /// <summary>
        /// Chuyển enum thành danh sách KeyValuePair.
        /// </summary>
        public static IEnumerable<KeyValuePair<int, string>> ToKeyValuePairs<T>() where T : Enum
        {
            return GetValues<T>().Select(v => new KeyValuePair<int, string>(v.ToInt(), v.GetDescription()));
        }

        // --- Attribute & Reflection ---

        /// <summary>
        /// Lấy một attribute bất kỳ từ một giá trị enum.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute, TEnum>(this TEnum enumValue) 
            where TAttribute : Attribute where TEnum : Enum
        {
            return enumValue.GetType()
                            .GetField(enumValue.ToString())
                            .GetCustomAttributes(typeof(TAttribute), false)
                            .FirstOrDefault() as TAttribute;
        }

        /// <summary>
        /// Kiểm tra một giá trị enum có được gán một attribute cụ thể không.
        /// </summary>
        public static bool HasAttribute<TAttribute, TEnum>(this TEnum enumValue)
            where TAttribute : Attribute where TEnum : Enum
        {
            return enumValue.GetAttribute<TAttribute, TEnum>() != null;
        }
        
        /// <summary>
        /// Lấy giá trị lớn nhất được định nghĩa trong enum.
        /// </summary>
        public static T GetMaxValue<T>() where T : Enum
        {
            return GetValues<T>().Max();
        }

        /// <summary>
        /// Lấy giá trị nhỏ nhất được định nghĩa trong enum.
        /// </summary>
        public static T GetMinValue<T>() where T : Enum
        {
            return GetValues<T>().Min();
        }
    }
} 