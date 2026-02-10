﻿namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Định nghĩa các phương thức cơ bản cho cache service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Lấy giá trị từ cache hoặc tạo mới nếu không tồn tại
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị cache</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="createFunc">Hàm tạo giá trị mới nếu cache không tồn tại</param>
        /// <param name="cacheMinutes">Thời gian cache tính bằng phút (mặc định 5 phút)</param>
        /// <returns>Giá trị từ cache hoặc giá trị mới được tạo</returns>
        T GetOrCreate<T>(string key, Func<T> createFunc, int cacheMinutes = 5);

        /// <summary>
        /// Lấy giá trị từ cache hoặc tạo mới nếu không tồn tại (async)
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị cache</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="createFunc">Hàm async tạo giá trị mới nếu cache không tồn tại</param>
        /// <param name="cacheMinutes">Thời gian cache tính bằng phút (mặc định 5 phút)</param>
        /// <returns>Task chứa giá trị từ cache hoặc giá trị mới được tạo</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createFunc, int cacheMinutes = 5);

        /// <summary>
        /// Lưu giá trị vào cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị cần lưu</param>
        /// <param name="cacheMinutes">Thời gian cache tính bằng phút (mặc định 5 phút)</param>
        void Set<T>(string key, T value, int cacheMinutes = 5);

        /// <summary>
        /// Lưu giá trị vào cache (async)
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị cần lưu</param>
        /// <param name="cacheMinutes">Thời gian cache tính bằng phút (mặc định 5 phút)</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, int cacheMinutes = 5);

        /// <summary>
        /// Thử lấy giá trị từ cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị lấy được từ cache</param>
        /// <returns>True nếu tìm thấy, False nếu không tìm thấy</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Thử lấy giá trị từ cache (async)
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <returns>Task chứa tuple (found, value)</returns>
        Task<(bool found, T value)> TryGetAsync<T>(string key);

        /// <summary>
        /// Xóa một mục khỏi cache
        /// </summary>
        /// <param name="key">Khóa cache cần xóa</param>
        void Remove(string key);

        /// <summary>
        /// Xóa một mục khỏi cache (async)
        /// </summary>
        /// <param name="key">Khóa cache cần xóa</param>
        /// <returns>Task</returns>
        Task RemoveAsync(string key);
    }

    /// <summary>
    /// Interface cho Redis cache service
    /// </summary>
    public interface IRedisCacheService : ICacheService { }

    /// <summary>
    /// Interface cho Memory cache service
    /// </summary>
    public interface IMemoryCacheService : ICacheService { }
}
