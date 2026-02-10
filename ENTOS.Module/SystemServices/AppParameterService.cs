using ENTOS.Module.Interfaces;
using System;

namespace ENTOS.Module.SystemServices
{
    /// <summary>
    /// Triển khai lấy tham số hệ thống hoặc tham số người dùng.
    /// </summary>
    public class AppParameterService : IAppParameterService
    {
        /// <summary>
        /// Lấy tham số hệ thống theo key.
        /// </summary>
        public string GetSystemParameter(string key)
        {
            // TODO: Lấy tham số hệ thống (ví dụ: từ appsettings hoặc database)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy tham số người dùng theo userId và key.
        /// </summary>
        public string GetUserParameter(string userId, string key)
        {
            // TODO: Lấy tham số người dùng (ví dụ: từ database)
            throw new NotImplementedException();
        }
    }
} 