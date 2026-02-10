namespace ENTOS.Module.Dto
{
    /// <summary>
    /// DTO chứa kết quả tra cứu thông tin địa chỉ IP WAN.
    /// </summary>
    public class IpInfoResult
    {
        /// <summary>Địa chỉ IP</summary>
        public string Ip { get; set; }
        /// <summary>Quốc gia</summary>
        public string Country { get; set; }
        /// <summary>Mã quốc gia</summary>
        public string CountryCode { get; set; }
        /// <summary>Khu vực (Region)</summary>
        public string Region { get; set; }
        /// <summary>Thành phố</summary>
        public string City { get; set; }
        /// <summary>Nhà mạng/ISP</summary>
        public string Org { get; set; }
        /// <summary>Kinh độ</summary>
        public string Lon { get; set; }
        /// <summary>Vĩ độ</summary>
        public string Lat { get; set; }
        /// <summary>Tên miền reverse (nếu có)</summary>
        public string Hostname { get; set; }
        /// <summary>Thông tin bổ sung khác (nếu có)</summary>
        public string RawJson { get; set; }
    }
} 