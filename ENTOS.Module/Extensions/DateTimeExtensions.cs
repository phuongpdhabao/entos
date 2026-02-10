using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension method mở rộng cho kiểu DateTime, hữu ích cho các nghiệp vụ ERP.
    /// </summary>
    public static class DateTimeExtensions
    {
        // --- Utility Methods (từ DateTimeHelper) ---

        /// <summary>
        /// Lấy ngày giờ hệ thống hiện tại (theo múi giờ local).
        /// </summary>
        public static DateTime GetSystemDateTime()
        {
            return DateTime.Now;
        }

        // --- Định dạng theo chuẩn ---

        /// <summary>
        /// Chuyển DateTime sang chuỗi theo định dạng tùy chỉnh.
        /// </summary>
        public static string ToStringFormat(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Định dạng ngày theo chuẩn Việt Nam (dd/MM/yyyy).
        /// </summary>
        public static string ToVnDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Định dạng ngày giờ theo chuẩn Việt Nam (dd/MM/yyyy HH:mm:ss).
        /// </summary>
        public static string ToVnDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Định dạng ngày giờ theo chuẩn ISO 8601 (yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        public static string ToIso8601Format(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("o");
        }

        // --- Thao tác với tuần, tháng, quý, năm ---

        /// <summary>
        /// Lấy ngày đầu tiên của tuần (mặc định tuần bắt đầu từ thứ Hai).
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Lấy ngày cuối cùng của tuần (mặc định tuần bắt đầu từ thứ Hai).
        /// </summary>
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dt.StartOfWeek(startOfWeek).AddDays(6);
        }

        /// <summary>
        /// Lấy ngày đầu tiên của tháng.
        /// </summary>
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Lấy ngày cuối cùng của tháng.
        /// </summary>
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        /// <summary>
        /// Lấy ngày đầu tiên của quý.
        /// </summary>
        public static DateTime StartOfQuarter(this DateTime dateTime)
        {
            int quarterNumber = (dateTime.Month - 1) / 3 + 1;
            return new DateTime(dateTime.Year, (quarterNumber - 1) * 3 + 1, 1);
        }

        /// <summary>
        /// Lấy ngày cuối cùng của quý.
        /// </summary>
        public static DateTime EndOfQuarter(this DateTime dateTime)
        {
            var startOfQuarter = dateTime.StartOfQuarter();
            return startOfQuarter.AddMonths(3).AddDays(-1);
        }

        /// <summary>
        /// Lấy ngày đầu tiên của năm.
        /// </summary>
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        /// <summary>
        /// Lấy ngày cuối cùng của năm.
        /// </summary>
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31);
        }

        /// <summary>
        /// Lấy số thứ tự của tuần trong năm (theo chuẩn ISO 8601).
        /// </summary>
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Lấy quý hiện tại (1, 2, 3, 4).
        /// </summary>
        public static int GetQuarter(this DateTime dateTime)
        {
            return (dateTime.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Lấy ngày kế tiếp của một thứ trong tuần (ví dụ: Next(DayOfWeek.Monday) để tìm thứ Hai tuần tới).
        /// </summary>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays <= 0) offsetDays += 7;
            return current.AddDays(offsetDays);
        }

        /// <summary>
        /// Lấy ngày trước đó của một thứ trong tuần (ví dụ: Previous(DayOfWeek.Monday) để tìm thứ Hai tuần trước).
        /// </summary>
        public static DateTime Previous(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = current.DayOfWeek - dayOfWeek;
            if (offsetDays <= 0) offsetDays += 7;
            return current.AddDays(-offsetDays);
        }

        // --- Tính toán khoảng thời gian & tuổi ---

        /// <summary>
        /// Tính tuổi chính xác.
        /// </summary>
        public static int Age(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Tính số ngày làm việc giữa hai mốc thời gian (không tính ngày cuối tuần).
        /// </summary>
        public static int BusinessDaysUntil(this DateTime startDate, DateTime endDate, DayOfWeek[] holidays = null)
        {
            if (startDate > endDate)
                return -BusinessDaysUntil(endDate, startDate, holidays);

            int businessDays = 0;
            DateTime current = startDate;
            while (current <= endDate)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (holidays == null || !holidays.Contains(current.DayOfWeek))
                    {
                        businessDays++;
                    }
                }
                current = current.AddDays(1);
            }
            return businessDays;
        }

        /// <summary>
        /// Tính số giây đã trôi qua so với một mốc thời gian.
        /// </summary>
        public static double ElapsedSeconds(this DateTime dateTime)
        {
            return (DateTime.Now - dateTime).TotalSeconds;
        }

        /// <summary>
        /// Tính số mili giây đã trôi qua so với một mốc thời gian.
        /// </summary>
        public static double ElapsedMilliseconds(this DateTime dateTime)
        {
            return (DateTime.Now - dateTime).TotalMilliseconds;
        }

        // --- Kiểm tra & so sánh ---

        /// <summary>
        /// Kiểm tra DateTime có phải là ngày cuối tuần không.
        /// </summary>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Kiểm tra có phải ngày trong tuần làm việc (Thứ 2 - Thứ 6).
        /// </summary>
        public static bool IsWeekday(this DateTime dateTime)
        {
            return !IsWeekend(dateTime);
        }

        /// <summary>
        /// Kiểm tra có phải là ngày trong tương lai.
        /// </summary>
        public static bool IsFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        /// <summary>
        /// Kiểm tra có phải là ngày trong quá khứ.
        /// </summary>
        public static bool IsPast(this DateTime dateTime)
        {
            return dateTime < DateTime.Now;
        }

        /// <summary>
        /// Kiểm tra có phải là ngày hôm nay.
        /// </summary>
        public static bool IsToday(this DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }

        /// <summary>
        /// Kiểm tra một ngày có nằm giữa hai ngày khác (bao gồm cả hai biên).
        /// </summary>
        public static bool IsBetween(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        // --- Thao tác với giờ & múi giờ ---

        /// <summary>
        /// Chuyển đổi DateTime sang một múi giờ khác theo ID (ví dụ: "SE Asia Standard Time").
        /// </summary>
        public static DateTime ToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            try
            {
                TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTime(dateTime, targetZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Trả về ngày giờ gốc nếu không tìm thấy múi giờ
                return dateTime;
            }
        }

        /// <summary>
        /// Lấy thời điểm bắt đầu của ngày (00:00:00).
        /// </summary>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Lấy thời điểm kết thúc của ngày (23:59:59.999).
        /// </summary>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Chuyển đổi DateTime sang Unix timestamp (giây).
        /// </summary>
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Chuyển đổi DateTime sang Unix timestamp (mili giây).
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Chuyển Unix timestamp (giây) sang DateTime.
        /// </summary>
        public static DateTime FromUnixTimeSeconds(long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
        }

        /// <summary>
        /// Sinh chuỗi mô tả thời gian tương đối (ví dụ: "cách đây 2 ngày", "3 giờ trước").
        /// </summary>
        public static string ToRelativeTimeString(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            if (ts.TotalDays >= 1) return $"{(int)ts.TotalDays} ngày trước";
            if (ts.TotalHours >= 1) return $"{(int)ts.TotalHours} giờ trước";
            if (ts.TotalMinutes >= 1) return $"{(int)ts.TotalMinutes} phút trước";
            return "vừa xong";
        }

        /// <summary>
        /// Kiểm tra một ngày có phải ngày nghỉ lễ (truyền vào danh sách ngày nghỉ).
        /// </summary>
        public static bool IsHoliday(this DateTime dateTime, IEnumerable<DateTime> holidays)
        {
            return holidays != null && holidays.Any(d => d.Date == dateTime.Date);
        }

        /// <summary>
        /// Tính số ngày làm việc thực tế giữa hai mốc thời gian (loại trừ ngày nghỉ lễ, cuối tuần).
        /// </summary>
        public static int WorkingDaysUntil(this DateTime startDate, DateTime endDate, IEnumerable<DateTime> holidays = null)
        {
            if (startDate > endDate)
                return -WorkingDaysUntil(endDate, startDate, holidays);
            int workingDays = 0;
            DateTime current = startDate;
            while (current <= endDate)
            {
                if (!current.IsWeekend() && (holidays == null || !holidays.Any(d => d.Date == current.Date)))
                    workingDays++;
                current = current.AddDays(1);
            }
            return workingDays;
        }

        /// <summary>
        /// Lấy danh sách các ngày làm việc trong một khoảng thời gian.
        /// </summary>
        public static List<DateTime> GetWorkingDays(this DateTime startDate, DateTime endDate, IEnumerable<DateTime> holidays = null)
        {
            var list = new List<DateTime>();
            DateTime current = startDate;
            while (current <= endDate)
            {
                if (!current.IsWeekend() && (holidays == null || !holidays.Any(d => d.Date == current.Date)))
                    list.Add(current);
                current = current.AddDays(1);
            }
            return list;
        }

        /// <summary>
        /// Lấy danh sách các ngày nghỉ trong một khoảng thời gian (cuối tuần + ngày lễ).
        /// </summary>
        public static List<DateTime> GetHolidays(this DateTime startDate, DateTime endDate, IEnumerable<DateTime> holidays = null)
        {
            var list = new List<DateTime>();
            DateTime current = startDate;
            while (current <= endDate)
            {
                if (current.IsWeekend() || (holidays != null && holidays.Any(d => d.Date == current.Date)))
                    list.Add(current);
                current = current.AddDays(1);
            }
            return list;
        }

        /// <summary>
        /// Chuyển đổi DateTime sang múi giờ UTC.
        /// </summary>
        public static DateTime ToUtc(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Chuyển đổi DateTime sang múi giờ Việt Nam (GMT+7).
        /// </summary>
        public static DateTime ToVnTime(this DateTime dateTime)
        {
            var vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(dateTime, vnZone);
        }

        /// <summary>
        /// Lấy tên múi giờ hiện tại.
        /// </summary>
        public static string GetTimeZoneName(this DateTime dateTime)
        {
            return TimeZoneInfo.Local.DisplayName;
        }

        /// <summary>
        /// Lấy số tuần trong tháng của một ngày.
        /// </summary>
        public static int GetWeekOfMonth(this DateTime dateTime)
        {
            var first = new DateTime(dateTime.Year, dateTime.Month, 1);
            return ((dateTime.Day + (int)first.DayOfWeek - 1) / 7) + 1;
        }

        /// <summary>
        /// Lấy số tuần trong năm.
        /// </summary>
        public static int GetWeeksInYear(this DateTime dateTime)
        {
            var lastDay = new DateTime(dateTime.Year, 12, 31);
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(lastDay, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Sinh chuỗi mô tả thời gian còn lại đến deadline (ví dụ: "Còn 2 ngày 3 giờ").
        /// </summary>
        public static string ToTimeLeftString(this DateTime deadline)
        {
            var ts = deadline - DateTime.Now;
            if (ts.TotalSeconds <= 0) return "Đã hết hạn";
            if (ts.TotalDays >= 1) return $"Còn {(int)ts.TotalDays} ngày {(ts.Hours)} giờ";
            if (ts.TotalHours >= 1) return $"Còn {(int)ts.TotalHours} giờ {(ts.Minutes)} phút";
            if (ts.TotalMinutes >= 1) return $"Còn {(int)ts.TotalMinutes} phút";
            return $"Còn {ts.Seconds} giây";
        }

        /// <summary>
        /// Lấy danh sách tất cả múi giờ hệ thống.
        /// </summary>
        public static List<string> GetSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones().Select(z => z.Id).ToList();
        }

        /// <summary>
        /// Định dạng ngày tháng theo ngôn ngữ/culture bất kỳ.
        /// </summary>
        public static string ToCultureDateString(this DateTime dateTime, string culture = "vi-VN", string format = "d")
        {
            return dateTime.ToString(format, new CultureInfo(culture));
        }

        /// <summary>
        /// Sinh danh sách ngày recurring theo tuần (ví dụ: các thứ 2, 4, 6 từ start đến end).
        /// </summary>
        public static List<DateTime> GetRecurringWeeklyDates(DateTime start, DateTime end, DayOfWeek[] days)
        {
            var result = new List<DateTime>();
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                if (days.Contains(d.DayOfWeek)) result.Add(d);
            return result;
        }

        /// <summary>
        /// Sinh danh sách ngày recurring theo tháng (ví dụ: ngày 1, 15, 28 mỗi tháng).
        /// </summary>
        public static List<DateTime> GetRecurringMonthlyDates(DateTime start, DateTime end, int[] daysOfMonth)
        {
            var result = new List<DateTime>();
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                if (daysOfMonth.Contains(d.Day)) result.Add(d);
            return result;
        }

        /// <summary>
        /// Sinh danh sách ngày recurring theo năm (ví dụ: 1/1, 2/9 mỗi năm).
        /// </summary>
        public static List<DateTime> GetRecurringYearlyDates(DateTime start, DateTime end, (int month, int day)[] md)
        {
            var result = new List<DateTime>();
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                if (md.Any(x => x.month == d.Month && x.day == d.Day)) result.Add(d);
            return result;
        }

        /// <summary>
        /// Kiểm tra một ngày có thuộc chuỗi recurring theo tuần không.
        /// </summary>
        public static bool IsRecurringWeekly(this DateTime date, DateTime start, DayOfWeek[] daysOfWeek)
        {
            return date >= start && daysOfWeek.Contains(date.DayOfWeek);
        }

        /// <summary>
        /// Định dạng TimeSpan thành chuỗi tiếng Việt hoặc Anh.
        /// </summary>
        public static string ToHumanReadable(this TimeSpan ts, string lang = "vi")
        {
            if (lang == "en")
                return $"{(int)ts.TotalDays} days {ts.Hours} hours {ts.Minutes} minutes";
            return $"{(int)ts.TotalDays} ngày {ts.Hours} giờ {ts.Minutes} phút";
        }

        /// <summary>
        /// Chuyển đổi <see cref="TimeSpan"/> thành chuỗi định dạng số liên tục dạng {Ngày}{Giờ}{Phút}{Giây}.
        /// Ví dụ: với <c>TimeSpan(1, 0, 1, 22)</c> sẽ trả về <c>"1000122"</c>.
        /// </summary>
        /// <param name="ts">Giá trị <see cref="TimeSpan"/> cần chuyển đổi.</param>
        public static string ToFormatTimeSpanAsCompactDigits(this TimeSpan ts)
        {
            return $"{ts.Days}{ts.Hours:D2}{ts.Minutes:D2}{ts.Seconds:D2}";
        }

        /// <summary>
        /// Sinh DateTime random trong khoảng.
        /// </summary>
        public static DateTime RandomDate(DateTime from, DateTime to)
        {
            var rand = new Random();
            var range = (to - from).Days;
            return from.AddDays(rand.Next(range));
        }

        /// <summary>
        /// Đếm số ngày cuối tuần trong khoảng.
        /// </summary>
        public static int CountWeekendDays(this DateTime start, DateTime end)
        {
            int count = 0;
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                if (d.IsWeekend()) count++;
            return count;
        }

        /// <summary>
        /// Đếm số ngày làm việc trong khoảng.
        /// </summary>
        public static int CountWorkingDays(this DateTime start, DateTime end, IEnumerable<DateTime> holidays = null)
        {
            int count = 0;
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                if (!d.IsWeekend() && (holidays == null || !holidays.Any(h => h.Date == d.Date))) count++;
            return count;
        }

        /// <summary>
        /// Tổng hợp tổng số giờ làm việc trong khoảng (giả định 8h/ngày làm việc).
        /// </summary>
        public static int SumWorkingHours(this DateTime start, DateTime end, IEnumerable<DateTime> holidays = null)
        {
            return CountWorkingDays(start, end, holidays) * 8;
        }

        /// <summary>
        /// Tổng hợp tổng số giờ cuối tuần trong khoảng (giả định 0h làm việc cuối tuần).
        /// </summary>
        public static int SumWeekendHours(this DateTime start, DateTime end)
        {
            return CountWeekendDays(start, end) * 0;
        }

        /// <summary>
        /// Placeholder: Chuyển đổi ngày dương lịch sang âm lịch (cần thư viện ngoài hoặc thuật toán riêng).
        /// </summary>
        public static string ToLunarDateString(this DateTime dateTime)
        {
            return "(Chưa hỗ trợ lịch âm)";
        }
    }
}
