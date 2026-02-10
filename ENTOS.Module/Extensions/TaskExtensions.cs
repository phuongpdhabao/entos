namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension methods cho Task để hỗ trợ timeout và các tiện ích mở rộng khác.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Đặt timeout cho một Task (không trả về giá trị).
        /// </summary>
        public static async Task WithTimeout(this Task task, int timeoutMs)
        {
            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) != task)
                throw new TimeoutException("Task bị timeout.");
            await task; // Đảm bảo exception của task gốc được ném ra nếu có
        }

        /// <summary>
        /// Đặt timeout cho một Task (có trả về giá trị).
        /// </summary>
        public static async Task<T> WithTimeout<T>(this Task<T> task, int timeoutMs)
        {
            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) != task)
                throw new TimeoutException("Task bị timeout.");
            return await task; // Đảm bảo exception của task gốc được ném ra nếu có
        }

    }
}