using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace ENTOS.Module.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Chuyển object sang JSON (dùng System.Text.Json).
        /// </summary>
        public static string ToJson(this object obj, bool indented = false)
        {
            var opts = new JsonSerializerOptions { WriteIndented = indented };
            return JsonSerializer.Serialize(obj, opts);
        }

        /// <summary>
        /// Parse JSON thành object kiểu T.
        /// </summary>
        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Deep clone object qua JSON.
        /// </summary>
        public static T DeepClone<T>(this T obj)
        {
            var json = obj.ToJson();
            return json.FromJson<T>();
        }

        /// <summary>
        /// Kiểm tra object null.
        /// </summary>
        public static bool IsNull(this object obj) => obj == null;

        /// <summary>
        /// Kiểm tra object khác null.
        /// </summary>
        public static bool IsNotNull(this object obj) => obj != null;

        /// <summary>
        /// Safe cast object sang kiểu T (trả về default nếu không cast được).
        /// </summary>
        public static T SafeCast<T>(this object obj)
        {
            try { return (T)obj; } catch { return default; }
        }

        /// <summary>
        /// Lấy giá trị property theo tên, hỗ trợ deep (A.B.C).
        /// </summary>
        public static object GetPropertyValue(this object obj, string propertyPath)
        {
            if (obj == null || string.IsNullOrEmpty(propertyPath)) return null;
            var parts = propertyPath.Split('.');
            object current = obj;
            foreach (var part in parts)
            {
                if (current == null) return null;
                var prop = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;
                current = prop.GetValue(current);
            }
            return current;
        }

        /// <summary>
        /// Gán giá trị property theo tên, hỗ trợ deep (A.B.C). Nếu property trung gian null sẽ tự tạo instance nếu có ctor không tham số.
        /// </summary>
        public static void SetPropertyValue(this object obj, string propertyPath, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyPath)) return;
            var parts = propertyPath.Split('.');
            object current = obj;
            PropertyInfo prop = null;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                prop = current.GetType().GetProperty(parts[i], BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return;
                var next = prop.GetValue(current);
                if (next == null)
                {
                    if (prop.PropertyType.GetConstructor(Type.EmptyTypes) != null)
                    {
                        next = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(current, next);
                    }
                    else return;
                }
                current = next;
            }
            prop = current.GetType().GetProperty(parts[^1], BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(current, value);
        }

        /// <summary>
        /// Chuyển object sang Dictionary<string, object> (dùng reflection).
        /// </summary>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var dict = new Dictionary<string, object>();
            if (obj == null) return dict;
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dict[prop.Name] = prop.GetValue(obj);
            }
            return dict;
        }

        /// <summary>
        /// Dump thông tin object (property:value) dạng string.
        /// </summary>
        public static string Dump(this object obj)
        {
            if (obj == null) return "null";
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var list = new List<string>();
            foreach (var p in props)
                list.Add($"{p.Name}: {p.GetValue(obj)}");
            return string.Join(", ", list);
        }

        /// <summary>
        /// So sánh sâu 2 object (bằng JSON).
        /// </summary>
        public static bool DeepEquals<T>(this T obj, T other)
        {
            if (obj == null && other == null) return true;
            if (obj == null || other == null) return false;
            return obj.ToJson() == other.ToJson();
        }

        /// <summary>
        /// Copy property cùng tên từ object này sang object khác (có thể khác kiểu).
        /// </summary>
        public static void CopyPropertiesTo(this object source, object target)
        {
            if (source == null || target == null) return;
            var srcProps = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var tgtProps = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var sp in srcProps)
            {
                var tp = Array.Find(tgtProps, p => p.Name == sp.Name && p.CanWrite);
                if (tp != null)
                {
                    var value = sp.GetValue(source);
                    tp.SetValue(target, value);
                }
            }
        }

        /// <summary>
        /// Chuyển object lồng nhau thành Dictionary phẳng (key dạng a.b.c).
        /// </summary>
        public static Dictionary<string, object> Flatten(this object obj, string prefix = "")
        {
            var dict = new Dictionary<string, object>();
            if (obj == null) return dict;
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(obj);
                var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                if (value != null && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string) && !prop.PropertyType.IsEnum && !prop.PropertyType.IsArray && !typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    foreach (var kv in value.Flatten(key))
                        dict[kv.Key] = kv.Value;
                }
                else
                {
                    dict[key] = value;
                }
            }
            return dict;
        }

        /// <summary>
        /// Lấy tất cả property (kể cả từ base class).
        /// </summary>
        public static IEnumerable<PropertyInfo> GetAllProperties(this object obj)
        {
            if (obj == null) yield break;
            var type = obj.GetType();
            while (type != null)
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    yield return prop;
                type = type.BaseType;
            }
        }

        /// <summary>
        /// Lấy giá trị field (dùng reflection).
        /// </summary>
        public static object GetFieldValue(this object obj, string fieldName)
        {
            if (obj == null || string.IsNullOrEmpty(fieldName)) return null;
            var field = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(obj);
        }

        /// <summary>
        /// Gán giá trị field (dùng reflection).
        /// </summary>
        public static void SetFieldValue(this object obj, string fieldName, object value)
        {
            if (obj == null || string.IsNullOrEmpty(fieldName)) return;
            var field = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                field.SetValue(obj, value);
        }

        /// <summary>
        /// Kiểm tra object có phải anonymous type không.
        /// </summary>
        public static bool IsAnonymousType(this object obj)
        {
            if (obj == null) return false;
            var type = obj.GetType();
            return Attribute.IsDefined(type, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute))
                && type.IsGenericType && type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"));
        }

        /// <summary>
        /// Lấy tên kiểu đầy đủ của object.
        /// </summary>
        public static string GetTypeName(this object obj)
        {
            return obj?.GetType().FullName ?? "null";
        }

        /// <summary>
        /// Chuyển object sang kiểu T (dùng Convert.ChangeType nếu có thể).
        /// </summary>
        public static T ToType<T>(this object obj)
        {
            if (obj == null) return default;
            if (obj is T t) return t;
            try { return (T)Convert.ChangeType(obj, typeof(T)); } catch { return default; }
        }

        /// <summary>
        /// Tạo object mới kiểu T, copy property cùng tên từ object gốc (mapping động).
        /// </summary>
        public static T MapTo<T>(this object source) where T : new()
        {
            if (source == null) return default;
            var target = new T();
            source.CopyPropertiesTo(target);
            return target;
        }

        /// <summary>
        /// Chuyển object sang ExpandoObject (dynamic).
        /// </summary>
        public static System.Dynamic.ExpandoObject ToExpando(this object obj)
        {
            var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            if (obj == null) return (System.Dynamic.ExpandoObject)expando;
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                expando[prop.Name] = prop.GetValue(obj);
            return (System.Dynamic.ExpandoObject)expando;
        }

        /// <summary>
        /// Lấy tất cả field (kể cả private, từ base class).
        /// </summary>
        public static IEnumerable<FieldInfo> GetAllFields(this object obj)
        {
            if (obj == null) yield break;
            var type = obj.GetType();
            while (type != null)
            {
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    yield return field;
                type = type.BaseType;
            }
        }

        /// <summary>
        /// Lấy tất cả attribute kiểu T trên object hoặc property.
        /// </summary>
        public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute
        {
            if (obj == null) yield break;
            var type = obj.GetType();
            foreach (var attr in type.GetCustomAttributes(typeof(T), true))
                yield return (T)attr;
        }

        /// <summary>
        /// Kiểm tra object hoặc property có attribute kiểu T không.
        /// </summary>
        public static bool HasAttribute<T>(this object obj) where T : Attribute
        {
            if (obj == null) return false;
            var type = obj.GetType();
            return type.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        /// <summary>
        /// So sánh 2 object về mặt giá trị (không quan tâm reference, dùng JSON).
        /// </summary>
        public static bool IsEquivalentTo(this object obj, object other)
        {
            if (obj == null && other == null) return true;
            if (obj == null || other == null) return false;
            return obj.ToJson() == other.ToJson();
        }

        /// <summary>
        /// Xuất thông tin object dạng cây (tree) cho debug.
        /// </summary>
        public static string PrintTree(this object obj, int indent = 0)
        {
            if (obj == null) return new string(' ', indent) + "null\n";
            var type = obj.GetType();
            if (type.IsPrimitive || obj is string || obj is DateTime || obj is decimal)
                return new string(' ', indent) + obj + "\n";
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new System.Text.StringBuilder();
            foreach (var p in props)
            {
                var value = p.GetValue(obj);
                result.Append(new string(' ', indent) + p.Name + ": ");
                if (value == null)
                    result.AppendLine("null");
                else if (p.PropertyType.IsPrimitive || value is string || value is DateTime || value is decimal)
                    result.AppendLine(value.ToString());
                else
                    result.Append(PrintTree(value, indent + 2));
            }
            return result.ToString();
        }

        /// <summary>
        /// Chuyển object sang dynamic (ExpandoObject).
        /// </summary>
        public static dynamic ToDynamic(this object obj) => obj.ToExpando();

        /// <summary>
        /// Lấy tất cả giá trị property của object dưới dạng Dictionary<string, object> (hỗ trợ collection).
        /// </summary>
        public static Dictionary<string, object> GetPropertyValues(this object obj)
        {
            var dict = new Dictionary<string, object>();
            if (obj == null) return dict;
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(obj);
                if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    var list = new List<object>();
                    foreach (var item in enumerable)
                        list.Add(item);
                    dict[prop.Name] = list;
                }
                else
                {
                    dict[prop.Name] = value;
                }
            }
            return dict;
        }

        /// <summary>
        /// Gán nhiều property từ Dictionary<string, object> (hỗ trợ collection).
        /// </summary>
        public static void SetPropertiesFromDictionary(this object obj, Dictionary<string, object> dict)
        {
            if (obj == null || dict == null) return;
            foreach (var kv in dict)
            {
                var prop = obj.GetType().GetProperty(kv.Key, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                {
                    if (kv.Value is System.Collections.IEnumerable enumerable && prop.PropertyType != typeof(string) && typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType))
                    {
                        // Gán collection nếu cần (chỉ hỗ trợ List<T> đơn giản)
                        var listType = typeof(List<>).MakeGenericType(prop.PropertyType.GetGenericArguments()[0]);
                        var list = Activator.CreateInstance(listType) as System.Collections.IList;
                        foreach (var item in (System.Collections.IEnumerable)kv.Value)
                            list.Add(item);
                        prop.SetValue(obj, list);
                    }
                    else
                    {
                        prop.SetValue(obj, kv.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Lấy giá trị theo index/key nếu object là collection hoặc dictionary.
        /// </summary>
        public static object GetIndexedValue(this object obj, object index)
        {
            if (obj == null || index == null) return null;
            if (obj is System.Collections.IDictionary dict)
                return dict[index];
            if (obj is System.Collections.IList list && index is int i && i >= 0 && i < list.Count)
                return list[i];
            return null;
        }

        /// <summary>
        /// Gán giá trị theo index/key nếu object là collection hoặc dictionary.
        /// </summary>
        public static void SetIndexedValue(this object obj, object index, object value)
        {
            if (obj == null || index == null) return;
            if (obj is System.Collections.IDictionary dict)
                dict[index] = value;
            else if (obj is System.Collections.IList list && index is int i && i >= 0 && i < list.Count)
                list[i] = value;
        }
    }
} 