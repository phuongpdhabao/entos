using System;
using System.Collections.Generic;
using System.Linq;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho IEnumerable (collection): kiểm tra rỗng, duyệt, nối chuỗi, phân trang, v.v.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Kiểm tra collection null hoặc rỗng.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Duyệt qua từng phần tử và thực hiện action.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        /// <summary>
        /// Nối các phần tử thành chuỗi với dấu phân cách.
        /// </summary>
        public static string JoinToString<T>(this IEnumerable<T> source, string separator = ", ")
        {
            return string.Join(separator, source);
        }

        ///// <summary>
        ///// Lấy các phần tử duy nhất theo thuộc tính.
        ///// </summary>
        //public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        //{
        //    var seen = new HashSet<TKey>();
        //    foreach (var item in source)
        //        if (seen.Add(keySelector(item))) yield return item;
        //}

        /// <summary>
        /// Chia collection thành các batch nhỏ.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;
            foreach (var item in source)
            {
                if (bucket == null) bucket = new T[size];
                bucket[count++] = item;
                if (count != size) continue;
                yield return bucket;
                bucket = null;
                count = 0;
            }
            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }

        /// <summary>
        /// Xáo trộn ngẫu nhiên collection.
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var rnd = new Random();
            return source.OrderBy(_ => rnd.Next());
        }

        /// <summary>
        /// Lấy phần tử nhỏ nhất theo thuộc tính.
        /// </summary>
        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            return source.Aggregate((a, b) => selector(a).CompareTo(selector(b)) < 0 ? a : b);
        }

        /// <summary>
        /// Lấy phần tử lớn nhất theo thuộc tính.
        /// </summary>
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            return source.Aggregate((a, b) => selector(a).CompareTo(selector(b)) > 0 ? a : b);
        }

        /// <summary>
        /// GroupBy nhiều thuộc tính.
        /// </summary>
        public static IEnumerable<IGrouping<TKey, TSource>> GroupByMany<TSource, TKey>(this IEnumerable<TSource> source, params Func<TSource, TKey>[] keySelectors)
        {
            IEnumerable<IGrouping<TKey, TSource>> result = null;
            foreach (var keySelector in keySelectors)
                result = (result == null ? source.GroupBy(keySelector) : result.SelectMany(g => g.GroupBy(keySelector)));
            return result;
        }

        /// <summary>
        /// Chuyển collection sang Dictionary an toàn (không lỗi nếu trùng key).
        /// </summary>
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var dict = new Dictionary<TKey, TSource>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!dict.ContainsKey(key)) dict[key] = item;
            }
            return dict;
        }

        /// <summary>
        /// Lấy phần tử ngẫu nhiên trong collection.
        /// </summary>
        public static T RandomItem<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            if (!list.Any()) return default;
            var rnd = new Random();
            return list[rnd.Next(list.Count)];
        }

        /// <summary>
        /// Phân trang collection.
        /// </summary>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Lấy phần tử đầu tiên hoặc mặc định nếu collection null.
        /// </summary>
        public static T FirstOrDefaultSafe<T>(this IEnumerable<T> source)
        {
            return source == null ? default : source.FirstOrDefault();
        }

        /// <summary>
        /// Lấy phần tử cuối cùng hoặc mặc định nếu collection null.
        /// </summary>
        public static T LastOrDefaultSafe<T>(this IEnumerable<T> source)
        {
            return source == null ? default : source.LastOrDefault();
        }

        /// <summary>
        /// Lấy phần tử duy nhất hoặc mặc định nếu collection null.
        /// </summary>
        public static T SingleOrDefaultSafe<T>(this IEnumerable<T> source)
        {
            return source == null ? default : source.SingleOrDefault();
        }

        /// <summary>
        /// Lấy vị trí của phần tử trong collection.
        /// </summary>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            int i = 0;
            foreach (var item in source)
            {
                if (Equals(item, value)) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Chia collection thành các mảng nhỏ (chunk).
        /// </summary>
        public static IEnumerable<List<T>> Chunk<T>(this IEnumerable<T> source, int size)
        {
            var chunk = new List<T>(size);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == size)
                {
                    yield return new List<T>(chunk);
                    chunk.Clear();
                }
            }
            if (chunk.Count > 0) yield return chunk;
        }

        /// <summary>
        /// Lấy N phần tử cuối cùng.
        /// </summary>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            var list = source.ToList();
            return list.Skip(Math.Max(0, list.Count - n));
        }

        /// <summary>
        /// Bỏ qua N phần tử cuối cùng.
        /// </summary>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n)
        {
            var list = source.ToList();
            return list.Take(Math.Max(0, list.Count - n));
        }

        /// <summary>
        /// Lấy value theo key, trả về mặc định nếu không có (Dictionary).
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
        {
            return dict != null && dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Gộp 2 dictionary, ưu tiên dictionary thứ 2 nếu trùng key.
        /// </summary>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2)
        {
            var result = new Dictionary<TKey, TValue>(dict1);
            foreach (var kv in dict2)
                result[kv.Key] = kv.Value;
            return result;
        }

        /// <summary>
        /// Lấy giao của nhiều collection.
        /// </summary>
        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> collections)
        {
            return collections.Aggregate((prev, next) => prev.Intersect(next));
        }

        /// <summary>
        /// Lấy hợp của nhiều collection.
        /// </summary>
        public static IEnumerable<T> UnionAll<T>(this IEnumerable<IEnumerable<T>> collections)
        {
            return collections.SelectMany(x => x).Distinct();
        }

        /// <summary>
        /// Lấy phần tử có ở collection 1 mà không có ở collection 2.
        /// </summary>
        public static IEnumerable<T> ExceptAll<T>(this IEnumerable<T> source, IEnumerable<T> except)
        {
            return source.Except(except);
        }

        /// <summary>
        /// Duyệt cây, trả về danh sách phẳng.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
        {
            foreach (var item in source)
            {
                yield return item;
                var children = childrenSelector(item);
                if (children != null)
                    foreach (var child in children.Flatten(childrenSelector))
                        yield return child;
            }
        }

        /// <summary>
        /// Chuyển danh sách phẳng thành cây (cần truyền hàm lấy id/parentId).
        /// </summary>
        public static IEnumerable<T> ToTree<T, TKey>(this IEnumerable<T> source, Func<T, TKey> idSelector, Func<T, TKey> parentIdSelector)
        {
            var items = source.ToList();
            var lookup = items.ToLookup(parentIdSelector);
            List<T> Build(TKey parentId)
            {
                return lookup[parentId].Select(item =>
                {
                    var children = Build(idSelector(item));
                    var prop = item.GetType().GetProperty("Children");
                    if (prop != null && prop.PropertyType.IsAssignableFrom(typeof(List<T>)))
                        prop.SetValue(item, children);
                    return item;
                }).ToList();
            }
            return Build(default);
        }

        /// <summary>
        /// Chuyển list tuple thành dictionary.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionaryFromTuple<TKey, TValue>(this IEnumerable<(TKey, TValue)> source)
        {
            return source.ToDictionary(x => x.Item1, x => x.Item2);
        }

        /// <summary>
        /// GroupBy và chuyển thành Dictionary.
        /// </summary>
        public static Dictionary<TKey, List<TSource>> GroupJoinToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.ToList());
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Chuyển sang ImmutableList.
        /// </summary>
        public static System.Collections.Immutable.ImmutableList<T> ToImmutableListSafe<T>(this IEnumerable<T> source)
        {
            return System.Collections.Immutable.ImmutableList.CreateRange(source);
        }

        /// <summary>
        /// Chuyển sang ImmutableDictionary.
        /// </summary>
        public static System.Collections.Immutable.ImmutableDictionary<TKey, TValue> ToImmutableDictionarySafe<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            return System.Collections.Immutable.ImmutableDictionary.CreateRange(source.ToDictionary(keySelector, valueSelector));
        }
#endif

        /// <summary>
        /// Duyệt collection bất đồng bộ tuần tự.
        /// </summary>
        public static async System.Threading.Tasks.Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, System.Threading.Tasks.Task> action)
        {
            foreach (var item in source)
                await action(item);
        }

        /// <summary>
        /// Duyệt collection song song (Parallel).
        /// </summary>
        public static void ParallelForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            System.Threading.Tasks.Parallel.ForEach(source, action);
        }

        /// <summary>
        /// Lọc bỏ phần tử null.
        /// </summary>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source) where T : class
        {
            return source.Where(x => x != null);
        }

        /// <summary>
        /// Lọc theo điều kiện động (chỉ Where nếu điều kiện đúng).
        /// </summary>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// Tính tổng theo thuộc tính.
        /// </summary>
        public static decimal SumBy<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            return source.Sum(selector);
        }

        /// <summary>
        /// Tính trung bình theo thuộc tính.
        /// </summary>
        public static double AvgBy<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Average(selector);
        }

        /// <summary>
        /// Tính trung vị (median) của collection số.
        /// </summary>
        public static double Median(this IEnumerable<double> source)
        {
            var sorted = source.OrderBy(x => x).ToList();
            int count = sorted.Count;
            if (count == 0) return 0;
            if (count % 2 == 1) return sorted[count / 2];
            return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
        }

        /// <summary>
        /// Tính mode (giá trị xuất hiện nhiều nhất) của collection số.
        /// </summary>
        public static double Mode(this IEnumerable<double> source)
        {
            return source.GroupBy(x => x).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
        }

        /// <summary>
        /// Nối các phần tử với dấu nháy kép (cho SQL, CSV).
        /// </summary>
        public static string JoinQuoted<T>(this IEnumerable<T> source, string separator = ", ")
        {
            return string.Join(separator, source.Select(x => $"\"{x}\""));
        }

        /// <summary>
        /// Chuyển collection thành chuỗi CSV.
        /// </summary>
        public static string ToCsv<T>(this IEnumerable<T> source, string separator = ",")
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Duyệt collection lồng nhiều cấp (flatten deep).
        /// </summary>
        public static IEnumerable<T> FlattenDeep<T>(this IEnumerable<IEnumerable<T>> source)
        {
            foreach (var sub in source)
                foreach (var item in sub)
                    yield return item;
        }

        /// <summary>
        /// Duyệt đệ quy với điều kiện dừng tùy ý.
        /// </summary>
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
        {
            foreach (var item in source)
            {
                yield return item;
                var children = childrenSelector(item);
                if (children != null)
                    foreach (var child in children.SelectRecursive(childrenSelector))
                        yield return child;
            }
        }

        /// <summary>
        /// Duyệt collection với lock (cho đa luồng).
        /// </summary>
        public static void LockingForEach<T>(this IEnumerable<T> source, Action<T> action, object lockObj)
        {
            foreach (var item in source)
            {
                lock (lockObj)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// Tính tổng thời gian của collection TimeSpan.
        /// </summary>
        public static TimeSpan Sum(this IEnumerable<TimeSpan> source)
        {
            return new TimeSpan(source.Sum(ts => ts.Ticks));
        }

        /// <summary>
        /// Tính tổng số lượng phần tử thỏa mãn điều kiện.
        /// </summary>
        public static int CountIf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.Count(predicate);
        }

        /// <summary>
        /// Lấy phần tử duy nhất thỏa mãn điều kiện hoặc mặc định.
        /// </summary>
        public static T SingleOrDefaultIf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Lấy phần tử đầu tiên thỏa mãn điều kiện hoặc mặc định.
        /// </summary>
        public static T FirstOrDefaultIf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Lấy phần tử cuối cùng thỏa mãn điều kiện hoặc mặc định.
        /// </summary>
        public static T LastOrDefaultIf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.LastOrDefault(predicate);
        }

        /// <summary>
        /// Kiểm tra collection có chứa tất cả phần tử của collection khác không.
        /// </summary>
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var set = new HashSet<T>(source);
            return items.All(set.Contains);
        }

        /// <summary>
        /// Kiểm tra collection có chứa ít nhất một phần tử của collection khác không.
        /// </summary>
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var set = new HashSet<T>(source);
            return items.Any(set.Contains);
        }

        /// <summary>
        /// Kiểm tra collection có rỗng hoặc chỉ chứa null không.
        /// </summary>
        public static bool IsNullOrAllNull<T>(this IEnumerable<T> source) where T : class
        {
            return source == null || source.All(x => x == null);
        }

        /// <summary>
        /// Kiểm tra collection có duy nhất một phần tử không.
        /// </summary>
        public static bool HasSingle<T>(this IEnumerable<T> source)
        {
            return source != null && source.Skip(1).Any() == false && source.Any();
        }

        /// <summary>
        /// Kiểm tra collection có nhiều hơn một phần tử không.
        /// </summary>
        public static bool HasMany<T>(this IEnumerable<T> source)
        {
            return source != null && source.Skip(1).Any();
        }

        /// <summary>
        /// Kiểm tra collection có đúng N phần tử không.
        /// </summary>
        public static bool HasCount<T>(this IEnumerable<T> source, int count)
        {
            return source != null && source.Count() == count;
        }

        /// <summary>
        /// Kiểm tra collection có rỗng không (không null).
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && !source.Any();
        }
    }
} 