namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface lấy thông tin hệ thống như ngày giờ, tên máy.
    /// </summary>
    public interface ISystemInfoService
    {
        /// <summary>
        /// Lấy ngày giờ hệ thống.
        /// </summary>
        DateTime GetSystemDateTime();
        /// <summary>
        /// Lấy tên máy.
        /// </summary>
        string GetMachineName();
    }
} 