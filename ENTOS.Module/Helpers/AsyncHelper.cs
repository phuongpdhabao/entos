using ENTOS.Module.Extensions;

namespace ENTOS.Module.Helpers
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Chạy song song một danh sách với số lượng tối đa task đồng thời (maxDegreeOfParallelism).
        /// </summary>
        public static async Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> action, int maxDegreeOfParallelism = 4)
        {
            var throttler = new SemaphoreSlim(maxDegreeOfParallelism);
            var tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try { await action(item); }
                finally { throttler.Release(); }
            });
            await Task.WhenAll(tasks);
        }

        public static async Task<List<TResult>> ForEachAsync<T, TResult>(IEnumerable<T> source, Func<T, Task<TResult>> action, int maxDegreeOfParallelism = 4)
        {
            var throttler = new SemaphoreSlim(maxDegreeOfParallelism);
            var results = new List<TResult>();
            var tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try { var r = await action(item); lock (results) results.Add(r); }
                finally { throttler.Release(); }
            });
            await Task.WhenAll(tasks);
            return results;
        }

        /// <summary>
        /// Chạy tuần tự một danh sách các task.
        /// </summary>
        public static async Task RunSequentially(IEnumerable<Func<Task>> actions)
        {
            foreach (var act in actions)
                await act();
        }

        /// <summary>
        /// Lặp lại một task theo chu kỳ, cho đến khi hủy hoặc đủ số lần.
        /// </summary>
        public static async Task RepeatAsync(Func<Task> action, TimeSpan interval, CancellationToken token, int? maxRepeat = null)
        {
            int count = 0;
            while (!token.IsCancellationRequested && (!maxRepeat.HasValue || count < maxRepeat.Value))
            {
                await action();
                count++;
                await Task.Delay(interval, token);
            }
        }

        /// <summary>
        /// Chạy từng task trong danh sách, delay giữa các lần.
        /// </summary>
        public static async Task ForEachAsyncWithDelay<T>(IEnumerable<T> source, Func<T, Task> action, int delayMs)
        {
            foreach (var item in source)
            {
                await action(item);
                await Task.Delay(delayMs);
            }
        }

        /// <summary>
        /// Chạy task cho đến khi điều kiện dừng thỏa mãn.
        /// </summary>
        public static async Task RunUntilAsync(Func<Task<bool>> action, TimeSpan interval, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (await action()) break;
                await Task.Delay(interval, token);
            }
        }

        /// <summary>
        /// Chạy WhenAll nhưng mỗi task có timeout riêng.
        /// </summary>
        public static async Task WhenAllWithTimeout(IEnumerable<Task> tasks, int timeoutMsPerTask)
        {
            var timeoutTasks = tasks.Select(t => t.WithTimeout(timeoutMsPerTask));
            await Task.WhenAll(timeoutTasks);
        }

        /// <summary>
        /// Thêm hỗ trợ cancellation cho task.
        /// </summary>
        public static async Task<T> WithCancellation<T>(Task<T> task, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (token.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(token);
            }
            return await task;
        }

        /// <summary>
        /// Tạo Task từ event (interop, legacy code).
        /// </summary>
        public static Task<T> FromEvent<T>(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe, int timeoutMs = 30000)
        {
            var tcs = new TaskCompletionSource<T>();
            Action<T> handler = null;
            handler = (result) => { unsubscribe(handler); tcs.TrySetResult(result); };
            subscribe(handler);
            var timer = new Timer(_ => tcs.TrySetCanceled(), null, timeoutMs, Timeout.Infinite);
            return tcs.Task.ContinueWith(t => { timer.Dispose(); return t.Result; }, TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// Chạy song song với báo tiến trình (IProgress).
        /// </summary>
        public static async Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> action, IProgress<int> progress, int maxDegreeOfParallelism = 4)
        {
            int total = source.Count();
            int done = 0;
            var throttler = new SemaphoreSlim(maxDegreeOfParallelism);
            var tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try { await action(item); }
                finally { progress?.Report(Interlocked.Increment(ref done)); throttler.Release(); }
            });
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Chia task thành từng batch nhỏ, chạy lần lượt.
        /// </summary>
        public static async Task BatchAsync<T>(IEnumerable<T> source, Func<T, Task> action, int batchSize)
        {
            var batch = new List<T>(batchSize);
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    await Task.WhenAll(batch.Select(action));
                    batch.Clear();
                }
            }
            if (batch.Count > 0)
                await Task.WhenAll(batch.Select(action));
        }
    }

    /// <summary>
    /// Extension methods cho Task để hỗ trợ timeout.
    /// </summary>
    public static class TaskTimeoutExtensions
    {

    }
}