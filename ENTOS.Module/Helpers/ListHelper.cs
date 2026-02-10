using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Added for Task
using System.Reflection; // Added for PropertyInfo
using System.Linq.Expressions;
using ENTOS.Module.Interfaces;
using ENTOS.Module.SystemObjects; // Added for Expression
namespace ENTOS.Module.Helpers;
public static class ListHelper
{
    /// <summary>
    /// Kiểm tra danh sách null hoặc rỗng.
    /// Cách dùng: ListHelper.IsNullOrEmpty(list)
    /// </summary>
    public static bool IsNullOrEmpty<T>(ICollection<T> list)
    {
        return list == null || list.Count == 0;
    }

    /// <summary>
    /// Thêm phần tử vào danh sách nếu chưa tồn tại.
    /// Cách dùng: ListHelper.AddIfNotExists(list, item)
    /// </summary>
    public static void AddIfNotExists<T>(ICollection<T> list, T item)
    {
        if (list == null) throw new ArgumentNullException(nameof(list), "Danh sách không được null.");
        if (!list.Contains(item))
            list.Add(item);
    }

    /// <summary>
    /// Thêm nhiều phần tử vào danh sách, bỏ qua phần tử trùng lặp.
    /// Cách dùng: ListHelper.AddRangeIfNotExists(list, items)
    /// </summary>
    public static void AddRangeIfNotExists<T>(ICollection<T> list, IEnumerable<T> items)
    {
        if (list == null) throw new ArgumentNullException(nameof(list), "Danh sách không được null.");
        foreach (var item in items)
        {
            if (!list.Contains(item))
                list.Add(item);
        }
    }

    /// <summary>
    /// Xóa tất cả phần tử thỏa mãn điều kiện khỏi danh sách.
    /// Cách dùng: ListHelper.RemoveAll(list, predicate)
    /// </summary>
    public static void RemoveAll<T>(ICollection<T> list, Func<T, bool> predicate)
    {
        if (list == null) throw new ArgumentNullException(nameof(list), "Danh sách không được null.");
        var itemsToRemove = list.Where(predicate).ToList();
        foreach (var item in itemsToRemove)
        {
            list.Remove(item);
        }
    }

    /// <summary>
    /// Lấy phần tử đầu tiên của danh sách, hoặc giá trị mặc định nếu danh sách null hoặc rỗng.
    /// Cách dùng: ListHelper.FirstOrDefaultSafe(list)
    /// </summary>
    public static T FirstOrDefaultSafe<T>(IEnumerable<T> list)
    {
        return list != null ? list.FirstOrDefault() : default(T);
    }

    /// <summary>
    /// Nối các phần tử của danh sách thành chuỗi, phân tách bởi ký tự chỉ định.
    /// Cách dùng: ListHelper.JoinToString(list, ",")
    /// </summary>
    public static string JoinToString<T>(IEnumerable<T> list, string separator = ",")
    {
        if (list == null) return string.Empty;
        return string.Join(separator, list);
    }

    /// <summary>
    /// Sao chép giá trị property từ từng phần tử của danh sách nguồn sang phần tử tương ứng của danh sách đích.
    /// Cách dùng: ListHelper.CopyProperty(sourceList, targetList, x => x.PropA, y => y.PropB)
    /// </summary>
    public static void CopyProperty<TSource, TTarget, TProperty>(
        List<TSource> sourceList,
        List<TTarget> targetList,
        Expression<Func<TSource, TProperty>> sourceSelector,
        Expression<Func<TTarget, TProperty>> targetSelector)
    {
        if (sourceList == null || targetList == null)
            throw new ArgumentNullException("sourceList/targetList", "Danh sách nguồn hoặc đích không được null.");

        var sourceProp = GetPropertyInfo(sourceSelector);
        var targetProp = GetPropertyInfo(targetSelector);

        if (!targetProp.CanWrite)
            throw new InvalidOperationException($"Thuộc tính '{targetProp.Name}' chỉ đọc.");

        int count = Math.Min(sourceList.Count, targetList.Count);

        for (int i = 0; i < count; i++)
        {
            var value = sourceProp.GetValue(sourceList[i]);
            targetProp.SetValue(targetList[i], value);
        }
    }

    /// <summary>
    /// Sao chép giá trị property từ từng phần tử của danh sách nguồn sang phần tử tương ứng của danh sách đích, chỉ định property bằng tên chuỗi.
    /// Cách dùng: ListHelper.CopyProperty(sourceList, targetList, "TenPropertyNguon", "TenPropertyDich")
    /// </summary>
    public static void CopyProperty<TSource, TTarget>(
        List<TSource> sourceList,
        List<TTarget> targetList,
        string sourcePropertyName,
        string targetPropertyName)
    {
        if (sourceList == null || targetList == null)
            throw new ArgumentNullException("sourceList/targetList", "Danh sách nguồn hoặc đích không được null.");
        if (string.IsNullOrWhiteSpace(sourcePropertyName) || string.IsNullOrWhiteSpace(targetPropertyName))
            throw new ArgumentException("Tên property không được để trống.");

        var sourceProp = typeof(TSource).GetProperty(sourcePropertyName);
        var targetProp = typeof(TTarget).GetProperty(targetPropertyName);

        if (sourceProp == null)
            throw new ArgumentException($"Không tìm thấy property '{sourcePropertyName}' trong kiểu {typeof(TSource).Name}.");
        if (targetProp == null)
            throw new ArgumentException($"Không tìm thấy property '{targetPropertyName}' trong kiểu {typeof(TTarget).Name}.");
        if (!targetProp.CanWrite)
            throw new InvalidOperationException($"Thuộc tính '{targetProp.Name}' chỉ đọc.");

        int count = Math.Min(sourceList.Count, targetList.Count);
        for (int i = 0; i < count; i++)
        {
            var value = sourceProp.GetValue(sourceList[i]);
            targetProp.SetValue(targetList[i], value);
        }
    }

    /// <summary>
    /// Sao chép nhiều thuộc tính từ từng phần tử của danh sách nguồn sang phần tử tương ứng của danh sách đích, chỉ định danh sách tên property.
    /// Cách dùng: ListHelper.CopyProperty(sourceList, targetList, new[] {"A", "B"}, new[] {"X", "Y"})
    /// </summary>
    public static void CopyProperty<TSource, TTarget>(
        List<TSource> sourceList,
        List<TTarget> targetList,
        string[] sourcePropertyNames,
        string[] targetPropertyNames)
    {
        if (sourceList == null || targetList == null)
            throw new ArgumentNullException("sourceList/targetList", "Danh sách nguồn hoặc đích không được null.");
        if (sourcePropertyNames == null || targetPropertyNames == null)
            throw new ArgumentNullException("sourcePropertyNames/targetPropertyNames", "Danh sách tên thuộc tính không được null.");
        if (sourcePropertyNames.Length != targetPropertyNames.Length)
            throw new ArgumentException("Số lượng thuộc tính nguồn và đích phải bằng nhau.");
        if (sourcePropertyNames.Length == 0)
            return;

        var sourceProps = sourcePropertyNames.Select(name => typeof(TSource).GetProperty(name)).ToList();
        var targetProps = targetPropertyNames.Select(name => typeof(TTarget).GetProperty(name)).ToList();

        for (int i = 0; i < sourceProps.Count; i++)
        {
            if (sourceProps[i] == null)
                throw new ArgumentException($"Không tìm thấy property '{sourcePropertyNames[i]}' trong kiểu {typeof(TSource).Name}.");
            if (targetProps[i] == null)
                throw new ArgumentException($"Không tìm thấy property '{targetPropertyNames[i]}' trong kiểu {typeof(TTarget).Name}.");
            if (!targetProps[i].CanWrite)
                throw new InvalidOperationException($"Thuộc tính '{targetProps[i].Name}' chỉ đọc.");
        }

        int count = Math.Min(sourceList.Count, targetList.Count);
        for (int idx = 0; idx < count; idx++)
        {
            for (int p = 0; p < sourceProps.Count; p++)
            {
                var value = sourceProps[p].GetValue(sourceList[idx]);
                targetProps[p].SetValue(targetList[idx], value);
            }
        }
    }



    /// <summary>
    /// Sao chép giá trị property từ từng phần tử của danh sách nguồn sang phần tử tương ứng của danh sách đích, có hỗ trợ theo dõi tiến trình.
    /// Cách dùng: await ListHelper.CopyPropertyWithProgress(context, sourceList, targetList, x => x.PropA, y => y.PropB)
    /// </summary>
    public static async Task CopyPropertyWithProgress<TSource, TTarget, TProperty>(
        Module.SystemObjects.LongTaskContext context,
        IList<TSource> sourceList,
        IList<TTarget> targetList,
        Expression<Func<TSource, TProperty>> sourceSelector,
        Expression<Func<TTarget, TProperty>> targetSelector)
    {
        if (sourceList == null || targetList == null)
            throw new ArgumentNullException("sourceList/targetList", "Dữ liệu đầu vào hoặc dữ liệu đầu ra không được null.");

        var sourceProp = GetPropertyInfo(sourceSelector);
        var targetProp = GetPropertyInfo(targetSelector);

        if (!targetProp.CanWrite)
            throw new InvalidOperationException($"Thuộc tính '{targetProp.Name}' chỉ đọc.");

        // Khởi tạo progress
        if (context?.Progress != null && context?.StepProgressConfig != null)
        {
            context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig.CurrentStepName} {sourceList.Count} dòng...";
        }
        System.Diagnostics.Debug.WriteLine($"Tool.CopyPropertyWithProgress: {sourceProp.Name} {targetProp.Name} ");
        int total = Math.Min(sourceList.Count, targetList.Count);

        for (int i = 0; i < total; i++)
        {
            context?.Control?.CancellationToken.ThrowIfCancellationRequested();

            // Xử lý sao chép dữ liệu
            var sourceValue = sourceProp.GetValue(sourceList[i]);
            var targetObject = targetList[i];
            if (context?.UiContext != null)
                context.UiContext.Post(_ =>
                {
                    targetProp.SetValue(targetObject, sourceValue);
                }, null);
            else
                targetProp.SetValue(targetObject, sourceValue);



            // Cập nhật UI nếu không thu nhỏ hoặc mỗi 20%
            if (context?.Progress != null && context?.StepProgressConfig != null && context?.Control != null)
            {
                //System.Diagnostics.Debug.WriteLine($"Tool.CopyPropertyWithProgress cập nhật %: {sourceProp.Name} {targetProp.Name} ");
                // Tính phần trăm hoàn thành
                int percentComplete = context.StepProgressConfig.MapStepProgressPercent(i + 1, total);
                if (!context.Control.IsMinimized || percentComplete % 20 == 0)
                {
                    context.Progress.PercentComplete = percentComplete;
                    context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig.CurrentStepName} {i + 1}/{total} - {percentComplete}%";
                }

            }
        }

        // Hoàn thành
        if (context?.Progress != null && context?.StepProgressConfig != null)
        {
            context.Progress.ProgressMessage = $"✅ {context.StepProgressConfig.CurrentStepName}  hoàn thành!";
        }
    }

    /// <summary>
    /// Sao chép giá trị property từ từng phần tử của danh sách nguồn sang phần tử tương ứng của danh sách đích, có hỗ trợ theo dõi tiến trình và điều kiện lọc.
    /// Cách dùng: await ListHelper.CopyPropertyWithProgressAndFilter(context, sourceList, targetList, x => x.PropA, y => y.PropB, filterCondition)
    /// </summary>
    public static async Task CopyPropertyWithProgressAndFilter<TSource, TTarget, TProperty>(
        Module.SystemObjects.LongTaskContext context,
        IList<TSource> sourceList,
        IList<TTarget> targetList,
        Expression<Func<TSource, TProperty>> sourceSelector,
        Expression<Func<TTarget, TProperty>> targetSelector,
        Func<TSource, bool> filterCondition)
    {
        if (sourceList == null || targetList == null)
            throw new ArgumentNullException("sourceList/targetList", "Danh sách nguồn hoặc đích không được null.");

        var sourceProp = GetPropertyInfo(sourceSelector);
        var targetProp = GetPropertyInfo(targetSelector);

        if (!targetProp.CanWrite)
            throw new InvalidOperationException($"Thuộc tính '{targetProp.Name}' chỉ đọc.");

        // Khởi tạo progress
        if (context?.Progress != null && context?.StepProgressConfig != null)
        {
            context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig.CurrentStepName} {sourceList.Count} dòng...";
        }

        int total = Math.Min(sourceList.Count, targetList.Count);
        int processedCount = 0;

        for (int i = 0; i < total; i++)
        {
            context?.Control?.CancellationToken.ThrowIfCancellationRequested();

            var sourceItem = sourceList[i];
            // Kiểm tra điều kiện lọc
            if (filterCondition(sourceItem))
            {
                var sourceValue = sourceProp.GetValue(sourceItem);
                if (context?.UiContext != null)
                    context.UiContext.Post(_ =>
                    {
                        targetProp.SetValue(targetList[i], sourceValue);
                    }, null);
                else
                    targetProp.SetValue(targetList[i], sourceValue);
                processedCount++;
            }

            // Tính phần trăm hoàn thành
            if (context?.Progress != null && context?.StepProgressConfig != null && context?.Control != null)
            {
                System.Diagnostics.Debug.WriteLine($"Tool.CopyPropertyWithProgress cập nhật %: {sourceProp.Name} {targetProp.Name} ");
                // Tính phần trăm hoàn thành
                int percentComplete = context.StepProgressConfig.MapStepProgressPercent(i + 1, total);
                if (!context.Control.IsMinimized || percentComplete % 20 == 0)
                {
                    context.Progress.PercentComplete = percentComplete;
                    context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig.CurrentStepName} {i + 1}/{total} - {percentComplete}%";
                }

            }
        }

        // Hoàn thành
        if (context?.Progress != null && context?.StepProgressConfig != null)
        {
            context.Progress.ProgressMessage = $"✅ {context.StepProgressConfig.CurrentStepName} hoàn thành! Đã xử lý {processedCount} dòng.";
        }
    }

    /// <summary>
    /// Lấy PropertyInfo từ biểu thức lambda.
    /// </summary>
    private static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo prop)
            return prop;

        if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression innerMember && innerMember.Member is PropertyInfo propInfo)
            return propInfo;

        throw new ArgumentException("Biểu thức không hợp lệ: cần truy cập property.");
    }
}
