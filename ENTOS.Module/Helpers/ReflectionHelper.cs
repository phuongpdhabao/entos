using ENTOS.Module.Extensions;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Tìm tất cả các kiểu (Type) thực thi một interface cụ thể trong các assembly đã load.
        /// </summary>
        /// <typeparam name="TInterface">Interface cần tìm các implement</typeparam>
        /// <returns>Danh sách các Type thực thi interface</returns>
        public static List<Type> GetImplementationsOfInterface<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"{interfaceType.FullName} is not an interface.");

            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type == null || !type.IsClass || type.IsAbstract)
                        continue;

                    if (interfaceType.IsAssignableFrom(type))
                        result.Add(type);
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy tất cả các kiểu (Type) có gắn attribute cụ thể trong các assembly đã load.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần tìm</typeparam>
        /// <returns>Danh sách các Type có attribute</returns>
        public static List<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type == null) continue;
                    if (type.GetCustomAttributes(typeof(TAttribute), true).Any())
                        result.Add(type);
                }
            }

            return result;
        }

        /// <summary>
        /// Tạo instance của một kiểu (Type) bằng tên hoặc Type.
        /// </summary>
        /// <param name="type">Kiểu cần tạo instance</param>
        /// <param name="args">Tham số khởi tạo</param>
        /// <returns>Instance của kiểu, hoặc null nếu không tạo được</returns>
        public static object? CreateInstanceOfType(Type type, params object[] args)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Lấy tất cả các Type đã load trong AppDomain hiện tại.
        /// </summary>
        /// <returns>Danh sách các Type đã load</returns>
        public static List<Type> GetAllLoadedTypes()
        {
            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types.AddRange(ex.Types.Where(t => t != null));
                }
            }
            return types;
        }

        /// <summary>
        /// Lấy giá trị property động.
        /// </summary>
        /// <param name="obj">Đối tượng</param>
        /// <param name="propertyName">Tên property</param>
        /// <returns>Giá trị property hoặc null nếu không tìm thấy</returns>
        public static object? GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var prop = obj.GetType().GetProperty(propertyName);
            return prop?.GetValue(obj);
        }

        /// <summary>
        /// Gán giá trị property động.
        /// </summary>
        /// <param name="obj">Đối tượng</param>
        /// <param name="propertyName">Tên property</param>
        /// <param name="value">Giá trị cần gán</param>
        public static void SetPropertyValue(object obj, string propertyName, object? value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var prop = obj.GetType().GetProperty(propertyName);
            prop?.SetValue(obj, value);
        }

        /// <summary>
        /// Tìm tất cả các class kế thừa một abstract class cụ thể trong các assembly đã load.
        /// </summary>
        /// <typeparam name="TBase">Abstract class cần tìm các implement</typeparam>
        /// <returns>Danh sách các Type kế thừa abstract class</returns>
        public static List<Type> GetImplementationsOfAbstractClass<TBase>()
        {
            var baseType = typeof(TBase);
            if (!baseType.IsClass || !baseType.IsAbstract)
                throw new ArgumentException($"{baseType.FullName} is not an abstract class.");

            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type == null || !type.IsClass || type.IsAbstract)
                        continue;

                    if (baseType.IsAssignableFrom(type))
                        result.Add(type);
                }
            }

            return result;
        }

        /// <summary>
        /// Tạo instance của một kiểu (Type) bằng tên đầy đủ (FullName).
        /// </summary>
        /// <param name="typeName">Tên đầy đủ của Type</param>
        /// <param name="args">Tham số khởi tạo</param>
        /// <returns>Instance của kiểu, hoặc null nếu không tạo được</returns>
        public static object? CreateInstanceByTypeName(string typeName, params object[] args)
        {
            if (string.IsNullOrEmpty(typeName)) throw new ArgumentNullException(nameof(typeName));

            var type = Type.GetType(typeName);
            if (type == null)
            {
                // Tìm trong các assembly đã load
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(typeName);
                    if (type != null) break;
                }
            }

            return type != null ? Activator.CreateInstance(type, args) : null;
        }

        /// <summary>
        /// Gọi method động theo tên.
        /// </summary>
        /// <param name="obj">Đối tượng</param>
        /// <param name="methodName">Tên method</param>
        /// <param name="args">Tham số</param>
        /// <returns>Kết quả từ method hoặc null</returns>
        public static object? InvokeMethod(object obj, string methodName, params object[] args)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));

            var method = obj.GetType().GetMethod(methodName);
            return method?.Invoke(obj, args);
        }

        /// <summary>
        /// Gọi static method động theo tên.
        /// </summary>
        /// <param name="type">Type chứa static method</param>
        /// <param name="methodName">Tên method</param>
        /// <param name="args">Tham số</param>
        /// <returns>Kết quả từ method hoặc null</returns>
        public static object? InvokeStaticMethod(Type type, string methodName, params object[] args)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));

            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            return method?.Invoke(null, args);
        }

        /// <summary>
        /// Lấy tất cả các property có attribute cụ thể.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần tìm</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>Danh sách PropertyInfo có attribute</returns>
        public static List<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var result = new List<PropertyInfo>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(TAttribute), true).Any())
                    result.Add(property);
            }

            return result;
        }

        /// <summary>
        /// Lấy tất cả các method có attribute cụ thể.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần tìm</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>Danh sách MethodInfo có attribute</returns>
        public static List<MethodInfo> GetMethodsWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var result = new List<MethodInfo>();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.GetCustomAttributes(typeof(TAttribute), true).Any())
                    result.Add(method);
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một Type có implement interface cụ thể không.
        /// </summary>
        /// <typeparam name="TInterface">Interface cần kiểm tra</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>True nếu Type implement interface</returns>
        public static bool ImplementsInterface<TInterface>(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"{interfaceType.FullName} is not an interface.");

            return interfaceType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Kiểm tra một Type có gắn attribute cụ thể không.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần kiểm tra</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>True nếu Type có attribute</returns>
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.GetCustomAttributes(typeof(TAttribute), true).Any();
        }

        /// <summary>
        /// Lấy attribute cụ thể từ Type.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần lấy</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>Attribute hoặc null nếu không tìm thấy</returns>
        public static TAttribute? GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var attributes = type.GetCustomAttributes(typeof(TAttribute), true);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
        }

        /// <summary>
        /// Lấy tất cả các Type con của một Type cụ thể.
        /// </summary>
        /// <param name="baseType">Type cha</param>
        /// <returns>Danh sách các Type con</returns>
        public static List<Type> GetDerivedTypes(Type baseType)
        {
            if (baseType == null) throw new ArgumentNullException(nameof(baseType));

            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type == null || type == baseType || !baseType.IsAssignableFrom(type))
                        continue;

                    result.Add(type);
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra xem một assembly có chứa Type cụ thể không.
        /// </summary>
        /// <param name="assembly">Assembly cần kiểm tra</param>
        /// <param name="typeName">Tên Type</param>
        /// <returns>True nếu assembly chứa Type</returns>
        public static bool AssemblyContainsType(Assembly assembly, string typeName)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(typeName)) throw new ArgumentNullException(nameof(typeName));

            try
            {
                return assembly.GetType(typeName) != null;
            }
            catch (ReflectionTypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy tất cả các field có attribute cụ thể.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute cần tìm</typeparam>
        /// <param name="type">Type cần kiểm tra</param>
        /// <returns>Danh sách FieldInfo có attribute</returns>
        public static List<FieldInfo> GetFieldsWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var result = new List<FieldInfo>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.GetCustomAttributes(typeof(TAttribute), true).Any())
                    result.Add(field);
            }

            return result;
        }

        /// <summary>
        /// Copy giá trị các property từ object này sang object khác (cùng loại property).
        /// </summary>
        /// <param name="source">Object nguồn</param>
        /// <param name="destination">Object đích</param>
        /// <param name="includePrivate">Có copy private property không</param>
        /// <param name="excludeProperties">Danh sách tên property cần bỏ qua</param>
        public static void CopyProperties(object source, object destination, bool includePrivate = false, params string[] excludeProperties)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var sourceType = source.GetType();
            var destinationType = destination.GetType();
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (includePrivate)
                bindingFlags |= BindingFlags.NonPublic;

            var sourceProperties = sourceType.GetProperties(bindingFlags);
            var destinationProperties = destinationType.GetProperties(bindingFlags);

            // Tạo HashSet để tìm kiếm nhanh các property cần bỏ qua
            var excludeSet = excludeProperties != null ? new HashSet<string>(excludeProperties, StringComparer.OrdinalIgnoreCase) : new HashSet<string>();

            foreach (var sourceProp in sourceProperties)
            {
                if (!sourceProp.CanRead) continue;

                // Bỏ qua property nếu nằm trong danh sách exclude
                if (excludeSet.Contains(sourceProp.Name)) continue;

                var destinationProp = destinationProperties.FirstOrDefault(p =>
                    p.Name == sourceProp.Name &&
                    p.PropertyType == sourceProp.PropertyType &&
                    p.CanWrite);

                if (destinationProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    destinationProp.SetValue(destination, value);
                }
            }
        }

        /// <summary>
        /// Copy giá trị các property từ object này sang object khác với danh sách exclude dạng IEnumerable.
        /// </summary>
        /// <param name="source">Object nguồn</param>
        /// <param name="destination">Object đích</param>
        /// <param name="excludeProperties">Danh sách tên property cần bỏ qua</param>
        /// <param name="includePrivate">Có copy private property không</param>
        public static void CopyProperties(object source, object destination, IEnumerable<string> excludeProperties, bool includePrivate = false)
        {
            CopyProperties(source, destination, includePrivate, excludeProperties?.ToArray() ?? Array.Empty<string>());
        }

        /// <summary>
        /// Copy giá trị các property từ object này sang object khác với predicate để quyết định property nào được copy.
        /// </summary>
        /// <param name="source">Object nguồn</param>
        /// <param name="destination">Object đích</param>
        /// <param name="propertyFilter">Predicate để quyết định property có được copy không</param>
        /// <param name="includePrivate">Có copy private property không</param>
        public static void CopyProperties(object source, object destination, Func<PropertyInfo, bool> propertyFilter, bool includePrivate = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (propertyFilter == null) throw new ArgumentNullException(nameof(propertyFilter));

            var sourceType = source.GetType();
            var destinationType = destination.GetType();
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (includePrivate)
                bindingFlags |= BindingFlags.NonPublic;

            var sourceProperties = sourceType.GetProperties(bindingFlags);
            var destinationProperties = destinationType.GetProperties(bindingFlags);

            foreach (var sourceProp in sourceProperties)
            {
                if (!sourceProp.CanRead) continue;

                // Áp dụng filter để quyết định có copy property này không
                if (!propertyFilter(sourceProp)) continue;

                var destinationProp = destinationProperties.FirstOrDefault(p =>
                    p.Name == sourceProp.Name &&
                    p.PropertyType == sourceProp.PropertyType &&
                    p.CanWrite);

                if (destinationProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    destinationProp.SetValue(destination, value);
                }
            }
        }

        /// <summary>
        /// Lấy PropertyInfo từ biểu thức lambda.
        /// </summary>
        public static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo prop)
                return prop;

            if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression innerMember && innerMember.Member is PropertyInfo propInfo)
                return propInfo;

            throw new ArgumentException("Biểu thức không hợp lệ: cần truy cập property.");
        }

        /// <summary>
        /// Tạo biểu thức lambda Expression<Func<T, TProperty>> từ tên thuộc tính.
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng.</typeparam>
        /// <typeparam name="TProperty">Kiểu của thuộc tính.</typeparam>
        /// <param name="propertyName">Tên thuộc tính.</param>
        /// <returns>Biểu thức lambda tương ứng.</returns>
        public static Expression<Func<T, TProperty>> BuildPropertySelector<T, TProperty>(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(param, propertyName);
            var converted = Expression.Convert(property, typeof(TProperty));
            return Expression.Lambda<Func<T, TProperty>>(converted, param);
        }

        /// <summary>
        /// Tạo một hàm lấy giá trị string từ thuộc tính có tên chỉ định trong object bất kỳ.
        /// </summary>
        /// <param name="propertyName">Tên thuộc tính cần đọc.</param>
        /// <returns>
        /// Delegate Func<object, string> để đọc giá trị property.
        /// Trả về null nếu object null hoặc property không tồn tại.
        /// </returns>
        public static Func<object, string> BuildStringPropertyAccessor(string propertyName)
        {
            return obj =>
            {
                if (obj == null) return null;

                var type = obj.GetType();
                var prop = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (prop == null) return null;

                var value = prop.GetValue(obj);
                return value?.ToString();
            };
        }

        /// <summary>
        /// Lấy tên module từ Type, loại bỏ prefix "ENTOS." nếu có.
        /// Phương thức này hữu ích để xác định module context từ assembly name.
        /// </summary>
        /// <param name="type">Type cần lấy tên module</param>
        /// <returns>Tên module đã được xử lý (loại bỏ prefix "ENTOS.")</returns>
        /// <exception cref="ArgumentNullException">Khi type là null</exception>
        public static string GetModuleName(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            var rootNamespace = AppDomain.CurrentDomain.FriendlyName.Split('.')[0];
            string assemblyName = type.Module.Assembly.GetName().Name;
            if (assemblyName.StartsWith(rootNamespace))
                assemblyName = assemblyName.Substring(rootNamespace.Length + 1); // Loại bỏ "ENTOS."

            return assemblyName;
        }


        /// <summary>
        /// Chuyển đổi kiểu dữ liệu của object với hỗ trợ Nullable types.
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <param name="conversion">Kiểu dữ liệu đích</param>
        /// <returns>Object đã được chuyển đổi kiểu</returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }
            else if (value != null)
            {
                //Fix lỗi đối tượng thừa kế không thể convert
                var valueType = value.GetType();
                if (valueType.IsSubclassOf(conversion))
                {
                    return value;
                }
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Lấy giá trị thuộc tính từ object với hỗ trợ truy cập lồng nhau và gọi method.
        /// </summary>
        /// <param name="currentObject">Object cần lấy thuộc tính</param>
        /// <param name="property">Tên thuộc tính hoặc method (có thể lồng nhau bằng dấu chấm)</param>
        /// <returns>Giá trị thuộc tính hoặc null nếu không tìm thấy</returns>
        public static object GetPropertyValueInObject(object currentObject, string property)
        {

            int dotIndex = property.IndexOf('.');
            if (dotIndex > 0)
            {
                string objPropretyName = property.Substring(0, dotIndex);
                var currentObjProperty = currentObject.GetPropertyValue(objPropretyName);
                if (currentObjProperty != null)
                {
                    return GetPropertyValueInObject(currentObjProperty, property.Substring(dotIndex + 1));
                }
            }
            else
            {
                object result = null;
                if (property.Contains("()"))
                {
                    var currentType = currentObject.GetType();
                    System.Reflection.MethodInfo theMethod = currentType.GetMethod(property.Replace("()", ""));
                    if (theMethod != null)
                    {
                        result = theMethod.Invoke(currentObject, null);
                    }
                }
                else
                {
                    result = currentObject.GetPropertyValue(property);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Thiết lập giá trị cho thuộc tính của object.
        /// Tự động chuyển đổi kiểu dữ liệu phù hợp.
        /// </summary>
        /// <param name="obj">Object cần thiết lập thuộc tính</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <param name="objValue">Giá trị cần thiết lập</param>
        /// <param name="objType">Kiểu dữ liệu của object (mặc định lấy từ obj)</param>
        public static void SetPropertyValue(object obj, string propertyName, string objValue, Type objType = null)
        {
            if (objType is null)
            {
                objType = obj?.GetType();
            }
            var property = objType?.GetProperty(propertyName);
            if (property == null)
                property = objType?.GetProperty(propertyName.ToLower());
            if (property != null)
            {
                if (property.MemberType.Equals(typeof(int)) || property.MemberType.Equals(typeof(int?)))
                {
                    property.SetValue(objValue, NumberHelper.GetNumberInText(propertyName));
                }
                else if (NumberHelper.IsNumber(property.PropertyType))
                {
                    var sb = new StringBuilder();
                    foreach (char c in objValue)
                    {
                        if (Char.IsDigit(c) || c == '.' || c == ',')
                            sb.Append(c);
                    }
                    var resultString = sb.ToString();
                    if (!string.IsNullOrEmpty(resultString))
                    {
                        if (property.MemberType.Equals(typeof(decimal)) || property.MemberType.Equals(typeof(decimal?)))
                        {
                            decimal number;
                            if (decimal.TryParse(resultString, out number))
                            {
                                property.SetValue(obj, number);
                            }
                        }
                        else
                        {
                            object fieldValue = Convert.ChangeType(objValue, property.PropertyType);
                            if (fieldValue != null)
                                property.SetValue(obj, fieldValue);
                        }
                    }
                }
                else if (property.PropertyType.Equals(typeof(DateTime)) || property.PropertyType.Equals(typeof(DateTime?)))
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(objValue, out dateTime))
                    {
                        property.SetValue(obj, dateTime);
                    }
                }
                else if (property.PropertyType.Equals(typeof(byte[])))
                {
                    if (!string.IsNullOrEmpty(objValue) && (objValue.StartsWith("http") || objValue.StartsWith("www")))
                    {
                        var photoValue = Task.Run(() => HttpHelper.DownloadFileAsync(objValue)).Result;
                        if (photoValue != null)
                            property.SetValue(obj, photoValue);
                    }
                }
                else if (property.PropertyType.Equals(typeof(bool)) || property.PropertyType.Equals(typeof(bool?)))
                {
                    property.SetValue(obj, true);
                }
                else
                {
                    property.SetValue(obj, objValue);
                }
            }
        }
        /// <summary>
        /// Thiết lập giá trị thuộc tính cho object với hỗ trợ truy cập lồng nhau.
        /// </summary>
        /// <param name="currentObject">Object cần thiết lập thuộc tính</param>
        /// <param name="property">Tên thuộc tính (có thể lồng nhau bằng dấu chấm)</param>
        /// <param name="value">Giá trị cần thiết lập</param>
        /// <returns>True nếu thiết lập thành công, false nếu thất bại</returns>
        public static bool SetPropertyValueInObject(object currentObject, string property, object value)
        {
            int dotIndex = property.IndexOf('.');
            if (dotIndex > 0)
            {
                string objPropretyName = property.Substring(0, dotIndex);
                var currentObjProperty = currentObject.GetPropertyValue(objPropretyName);
                if (currentObjProperty != null)
                {
                    return SetPropertyValueInObject(currentObjProperty, property.Substring(dotIndex + 1), value);
                }
            }
            else
            {
                var result = currentObject.GetPropertyValue(property);
                if (value != result)
                {
                    var currentType = currentObject.GetType();
                    var currentProperty = currentType.GetProperty(property);
                    if (currentProperty != null)
                    {
                        currentProperty.SetValue(currentObject,
                            Module.Helpers.ReflectionHelper.ChangeType(value, currentProperty.PropertyType));
                        return true;
                    }

                }
            }

            return false;
        }
        /// <summary>
        /// Làm gọn tên đầy đủ của một kiểu dữ liệu .NET.
        /// </summary>
        /// <param name="fullTypeName">Tên đầy đủ kiểu dữ liệu, ví dụ "System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=...]]"</param>
        /// <returns>
        /// Tên kiểu dữ liệu đã được làm gọn, chỉ giữ tên namespace và type, ví dụ "System.Nullable`1[[System.DateTime]]"
        /// </returns>
        public static string SimplifyTypeName(string fullTypeName)
        {
            if (string.IsNullOrWhiteSpace(fullTypeName))
                return fullTypeName;

            string pattern = @"(\[\[)([^,\]]+),[^]]+(\]\])";
            string result = Regex.Replace(fullTypeName, pattern, "$1$2$3");

            return result;
        }
        /// <summary>
        /// Xác định kiểu CLR thực sự của một Type.
        /// </summary>
        /// <param name="propertyType">Kiểu của property lấy từ reflection.</param>
        /// <returns>
        /// Kiểu CLR thực sự của property; nếu không phải nullable hoặc collection
        /// thì trả về chính propertyType.
        /// </returns>
        public static Type GetRealType(Type propertyType)
        {
            if (propertyType == null)
                return null;

            // Nullable<T> → T
            var nullable = Nullable.GetUnderlyingType(propertyType);
            if (nullable != null)
                return nullable;

            // Array → element type
            if (propertyType.IsArray)
                return propertyType.GetElementType();

            // Generic collection → T
            if (propertyType.IsGenericType)
            {
                var genDef = propertyType.GetGenericTypeDefinition();

                if (genDef == typeof(ICollection<>)
                 || genDef == typeof(IEnumerable<>)
                 || genDef == typeof(IList<>)
                 || genDef == typeof(List<>))
                {
                    return propertyType.GetGenericArguments()[0];
                }

                // XPCollection<T>
                if (genDef.FullName.StartsWith("DevExpress.Xpo.XPCollection"))
                {
                    return propertyType.GetGenericArguments()[0];
                }
            }

            return propertyType;
        }

        #region Enum & Reflection Utilities

        /// <summary>
        /// Lấy attribute của một enum value sử dụng reflection.
        /// Extension method để dễ dàng truy cập attribute từ enum values.
        /// </summary>
        /// <typeparam name="T">Loại attribute cần lấy (phải kế thừa từ System.Attribute)</typeparam>
        /// <param name="enumVal">Enum value cần lấy attribute</param>
        /// <returns>Attribute instance hoặc null nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi enumVal là null</exception>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            if (enumVal == null)
                throw new ArgumentNullException(nameof(enumVal));

            try
            {
                var type = enumVal.GetType();
                var memInfo = type.GetMember(enumVal.ToString());

                if (memInfo.Length == 0)
                    return null;

                var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
                return (attributes.Length > 0) ? (T)attributes[0] : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy attribute từ enum: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chuyển đổi một chuỗi thành giá trị enum tương ứng.
        /// </summary>
        /// <typeparam name="T">
        /// Kiểu enum cần parse. Phải là kiểu <see cref="System.Enum"/>.
        /// </typeparam>
        /// <param name="value">
        /// Chuỗi cần chuyển đổi. Nếu null hoặc rỗng sẽ trả về giá trị mặc định.
        /// </param>
        /// <returns>
        /// Giá trị enum tương ứng nếu parse thành công; 
        /// nếu không parse được thì trả về <c>default(T)</c>.
        /// </returns>
        public static T ParseEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return default;

            if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
                return result;

            return default; // fallback nếu không parse được
        }

        #endregion


    }
}
