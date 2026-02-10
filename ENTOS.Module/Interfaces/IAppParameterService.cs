namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface lấy tham số hệ thống hoặc tham số người dùng.
    /// </summary>
    public interface IAppParameterService
    {
        /// <summary>
        /// Lấy tham số hệ thống theo key.
        /// </summary>
        string GetSystemParameter(string key);
        /// <summary>
        /// Lấy tham số người dùng theo userId và key.
        /// </summary>
        string GetUserParameter(string userId, string key);
    }
} 