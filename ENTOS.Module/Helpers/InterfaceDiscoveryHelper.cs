using System.Reflection;
using ENTOS.Module.Interfaces;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper để tự động khám phá và cache các interface implementations
    /// Sử dụng reflection để tìm tất cả classes implement interface T
    /// Cache toàn ứng dụng để tối ưu hiệu suất
    /// </summary>
    public static class InterfaceDiscoveryHelper
    {
        private static readonly Dictionary<Type, List<object>> _globalParsers = new Dictionary<Type, List<object>>();
        private static readonly object _parserLock = new object();
        private static readonly HashSet<Type> _initializedTypes = new HashSet<Type>();

        // THÊM VÀO: Giới hạn cache size
        private const int MaxCacheSize = 50; // Giới hạn tối đa 50 interface types
        private static readonly Queue<Type> _cacheOrder = new Queue<Type>(); // LRU cache

        /// <summary>
        /// Lấy danh sách implementations cho interface type cụ thể
        /// </summary>
        /// <typeparam name="T">Interface type (ví dụ: ITranslateResponseParser)</typeparam>
        /// <returns>Danh sách implementations đã được cache</returns>
        public static List<T> GetImplementations<T>() where T : class
        {
            var interfaceType = typeof(T);

            if (!_initializedTypes.Contains(interfaceType))
            {
                lock (_parserLock)
                {
                    if (!_initializedTypes.Contains(interfaceType))
                    {
                        AutoDiscoverImplementations<T>();
                        _initializedTypes.Add(interfaceType);
                    }
                }
            }

            if (_globalParsers.TryGetValue(interfaceType, out var parsers))
            {
                return parsers.Cast<T>().ToList();
            }

            return new List<T>();
        }

        /// <summary>
        /// Tự động khám phá tất cả implementations của interface T
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        private static void AutoDiscoverImplementations<T>() where T : class
        {
            lock (_parserLock)
            {
                var interfaceType = typeof(T);

                if (_globalParsers.ContainsKey(interfaceType) && _globalParsers[interfaceType].Count > 0)
                {
                    // Cập nhật LRU cache order
                    //_cacheOrder.Remove(interfaceType);
                    _cacheOrder.Enqueue(interfaceType);
                    return; // Đã khởi tạo rồi
                }

                // THÊM VÀO: LRU cache eviction
                if (_globalParsers.Count >= MaxCacheSize)
                {
                    var oldestType = _cacheOrder.Dequeue();
                    _globalParsers.Remove(oldestType);
                    _initializedTypes.Remove(oldestType);
                }

                var parsers = new List<object>();

                try
                {
                    // Lấy assembly hiện tại
                    var currentAssembly = Assembly.GetExecutingAssembly();

                    // Tìm tất cả types implement interface T
                    var implementationTypes = currentAssembly.GetTypes()
                        .Where(type => interfaceType.IsAssignableFrom(type)
                                    && !type.IsInterface
                                    && !type.IsAbstract
                                    && type.GetConstructor(Type.EmptyTypes) != null) // Có constructor không tham số
                        .ToList();

                    // Khởi tạo từng implementation
                    foreach (var implementationType in implementationTypes)
                    {
                        try
                        {
                            var implementation = Activator.CreateInstance(implementationType);
                            if (implementation != null)
                            {
                                parsers.Add(implementation);
#if DEBUG
                                System.Diagnostics.Debug.WriteLine($"Đã khởi tạo {interfaceType.Name}: {implementationType.Name}");
#endif
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo {interfaceType.Name} {implementationType.Name}: {ex.Message}");
#endif
                        }
                    }

                    _globalParsers[interfaceType] = parsers;
                    _cacheOrder.Enqueue(interfaceType); // Thêm vào LRU cache

#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Đã tìm thấy và khởi tạo {parsers.Count} {interfaceType.Name}(s)");
#endif
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Lỗi tự động tìm {interfaceType.Name}: {ex.Message}");
#endif
                    _globalParsers[interfaceType] = new List<object>();
                }
            }
        }

        /// <summary>
        /// Xóa cache cho interface type cụ thể (dùng khi reload)
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        public static void ClearCache<T>() where T : class
        {
            var interfaceType = typeof(T);
            lock (_parserLock)
            {
                _globalParsers.Remove(interfaceType);
                _initializedTypes.Remove(interfaceType);
            }
        }

        /// <summary>
        /// Xóa toàn bộ cache
        /// </summary>
        public static void ClearAllCache()
        {
            lock (_parserLock)
            {
                _globalParsers.Clear();
                _initializedTypes.Clear();
            }
        }

        /// <summary>
        /// Lấy thống kê cache
        /// </summary>
        /// <returns>Thông tin cache hiện tại</returns>
        public static string GetCacheStats()
        {
            lock (_parserLock)
            {
                var stats = new List<string>();
                stats.Add($"Cache size: {_globalParsers.Count}/{MaxCacheSize}");
                stats.Add($"Memory usage: ~{EstimateMemoryUsage()} KB");

                foreach (var kvp in _globalParsers)
                {
                    stats.Add($"{kvp.Key.Name}: {kvp.Value.Count} implementations");
                }
                return string.Join(", ", stats);
            }
        }

        /// <summary>
        /// Ước tính memory usage của cache
        /// </summary>
        private static int EstimateMemoryUsage()
        {
            int totalSize = 0;
            foreach (var kvp in _globalParsers)
            {
                // Ước tính: mỗi parser instance ~1KB
                totalSize += kvp.Value.Count * 1024;
            }
            return totalSize / 1024; // Convert to KB
        }
    }
}