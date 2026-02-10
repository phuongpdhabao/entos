namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface chạy ứng dụng/process bên ngoài.
    /// </summary>
    public interface IProcessService
    {
        /// <summary>
        /// Chạy process ngoài với tham số.
        /// </summary>
        Task<int> RunProcessAsync(string fileName, string arguments);
    }
} 