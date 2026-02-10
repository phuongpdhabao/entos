using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper cache in-memory đơn giản, thread-safe, hỗ trợ timeout, nhóm, lazy cache. Chỉ dùng .NET chuẩn.
    /// </summary>
    public static class CacheHelper
    {
        private static readonly ConcurrentDictionary<string, (object value, Timer timer)> _cache = new();
        private static readonly ConcurrentDictionary<string, string> _groups = new();

        /// <summary>
        /// Lưu giá trị vào cache với key (không timeout).
        /// </summary>
        public static void Set(string key, object value)
        {
            Remove(key);
            _cache[key] = (value, null);
        }
        /// <summary>
        /// Lưu giá trị vào cache với key và timeout (tự động xóa sau expire).
        /// </summary>
        public static void SetWithExpire(string key, object value, TimeSpan expire)
        {
            Remove(key);
            Timer timer = new Timer(_ => Remove(key), null, expire, Timeout.InfiniteTimeSpan);
            _cache[key] = (value, timer);
        }
        /// <summary>
        /// Lấy giá trị cache theo key (object).
        /// </summary>
        public static object Get(string key)
        {
            return _cache.TryGetValue(key, out var entry) ? entry.value : null;
        }
        /// <summary>
        /// Lấy giá trị cache theo key (generic).
        /// </summary>
        public static T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out var entry) && entry.value is T t ? t : default;
        }
        /// <summary>
        /// Kiểm tra key có tồn tại trong cache không.
        /// </summary>
        public static bool Exists(string key) => _cache.ContainsKey(key);
        /// <summary>
        /// Xóa cache theo key.
        /// </summary>
        public static void Remove(string key)
        {
            if (_cache.TryRemove(key, out var entry) && entry.timer != null)
                entry.timer.Dispose();
        }
        /// <summary>
        /// Xóa toàn bộ cache.
        /// </summary>
        public static void Clear()
        {
            foreach (var key in _cache.Keys)
                Remove(key);
        }
        /// <summary>
        /// Lấy hoặc tạo cache mới nếu chưa có (lazy cache).
        /// </summary>
        public static T GetOrAdd<T>(string key, Func<T> valueFactory, TimeSpan? expire = null)
        {
            if (Exists(key)) return Get<T>(key);
            var value = valueFactory();
            if (expire.HasValue) SetWithExpire(key, value, expire.Value); else Set(key, value);
            return value;
        }
        /// <summary>
        /// Ghi đè hoặc thêm mới cache, hỗ trợ timeout.
        /// </summary>
        public static void SetOrUpdate(string key, object value, TimeSpan? expire = null)
        {
            if (expire.HasValue) SetWithExpire(key, value, expire.Value); else Set(key, value);
        }
        /// <summary>
        /// Lưu cache theo nhóm (group), hỗ trợ timeout.
        /// </summary>
        public static void SetGroup(string key, object value, string group, TimeSpan? expire = null)
        {
            SetOrUpdate(key, value, expire);
            _groups[key] = group;
        }
        /// <summary>
        /// Xóa toàn bộ cache thuộc group.
        /// </summary>
        public static void ClearGroup(string group)
        {
            var keys = _groups.Where(kv => kv.Value == group).Select(kv => kv.Key).ToList();
            foreach (var key in keys) { Remove(key); _groups.TryRemove(key, out _); }
        }
        /// <summary>
        /// Lấy danh sách key hiện có trong cache.
        /// </summary>
        public static IEnumerable<string> GetKeys() => _cache.Keys;
        /// <summary>
        /// Lấy danh sách value hiện có trong cache.
        /// </summary>
        public static IEnumerable<object> GetValues() => _cache.Values.Select(v => v.value);
        /// <summary>
        /// Đếm số lượng cache hiện tại.
        /// </summary>
        public static int Count => _cache.Count;
        /// <summary>
        /// Lấy hoặc tạo cache thread-safe cho object bất kỳ.
        /// </summary>
        public static T GetOrAddThreadSafe<T>(string key, Func<T> valueFactory, TimeSpan? expire = null)
        {
            return (T)_cache.GetOrAdd(key, _ => (valueFactory(), null)).value;
        }
    }
} 