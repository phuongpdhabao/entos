using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System.Globalization;

namespace ENTOS.Module.SystemObjects
{
    //Lưu ý, file này có thể sẽ bị xóa toàn bộ
    public static partial class Tools
    {

        /// <summary>
        /// Lấy giá trị tham số theo business module và key name.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị tham số hoặc chuỗi rỗng nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi session, module hoặc key là null</exception>
        /// <example>
        /// <code>
        /// string maxSize = Tools.GetValue(session, "System", "MaxFileSize");
        /// string theme = Tools.GetValue(session, "UI", "DefaultTheme");
        /// </code>
        /// </example>
        [Obsolete]
        public static string GetValue(Session session, string module, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string criteria = string.Format("Business == '{0}' && Name=='{1}'", module, key);
                Module.BusinessObjects.Parameter settingParameter = session.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
                return settingParameter?.Value ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số theo key name (không phân biệt module).
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị tham số hoặc chuỗi rỗng nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi session hoặc key là null</exception>
        /// <example>
        /// <code>
        /// string appName = Tools.GetValue(session, "ApplicationName");
        /// string version = Tools.GetValue(session, "Version");
        /// </code>
        /// </example>
        public static string GetValue(Session session, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string criteria = string.Format("Name=='{0}'", key);
                Module.BusinessObjects.Parameter settingParameter = session.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
                return settingParameter?.Value ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số hoặc giá trị mặc định nếu không tìm thấy (IObjectSpace).
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị tham số hoặc giá trị mặc định</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace hoặc key là null</exception>
        [Obsolete]
        public static string GetValueOrDefault(IObjectSpace objectSpace, string key, string defaultValue)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                return GetParameterValueOrDefault(objectSpace, key, defaultValue).Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}' với default: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số hoặc giá trị mặc định nếu không tìm thấy (Session).
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị tham số hoặc giá trị mặc định</returns>
        /// <exception cref="ArgumentNullException">Khi session hoặc key là null</exception>
        [Obsolete]
        public static string GetValueOrDefault(Session session, string key, string defaultValue)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                return GetParameterValueOrDefault(session, key, defaultValue).Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}' với default: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị boolean của tham số hoặc giá trị mặc định (Session).
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị boolean của tham số hoặc giá trị mặc định</returns>
        /// <exception cref="ArgumentNullException">Khi session hoặc key là null</exception>
        [Obsolete]
        public static bool GetBooleanOrDefault(Session session, string key, bool defaultValue)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                return GetParameterValueOrDefault(session, key, defaultValue.ToString()).GetBooleanValue();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị boolean parameter '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị boolean của tham số hoặc giá trị mặc định (IObjectSpace).
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị boolean của tham số hoặc giá trị mặc định</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace hoặc key là null</exception>
        [Obsolete]
        public static bool GetBooleanOrDefault(IObjectSpace objectSpace, string key, bool defaultValue)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                return GetParameterValueOrDefault(objectSpace, key, defaultValue.ToString()).GetBooleanValue();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị boolean parameter '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tham số hệ thống theo key name.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Parameter object hoặc null nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi session hoặc key là null</exception>
        [Obsolete]
        public static Module.BusinessObjects.Parameter GetSettingParameter(Session session, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string criteria = string.Format("Name=='{0}' and PermissionPolicyUser is null", key);
                return session.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy setting parameter '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số theo business module và key name sử dụng IObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị tham số hoặc chuỗi rỗng nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace, module hoặc key là null</exception>
        [Obsolete]
        public static string GetValue(IObjectSpace objectSpace, string module, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string criteria = string.Format("Module == '{0}' && Name=='{1}'", module, key);
                Module.BusinessObjects.Parameter settingParameter =
                    objectSpace.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
                return settingParameter?.Value ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số theo key name sử dụng IObjectSpace (không phân biệt module).
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị tham số hoặc chuỗi rỗng nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace hoặc key là null</exception>
        [Obsolete]
        public static string GetValue(IObjectSpace objectSpace, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string criteria = string.Format("Name=='{0}'", key);
                Module.BusinessObjects.Parameter settingParameter =
                    objectSpace.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
                return settingParameter?.Value ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị parameter '{key}': {ex.Message}", ex);
            }
        }
        #region Numeric Value Methods

        /// <summary>
        /// Lấy giá trị double từ tham số hệ thống theo business module và key.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị double hoặc 0.0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi session, module hoặc key là null</exception>
        /// <example>
        /// <code>
        /// double maxSize = Tools.GetDoubleValue(session, "System", "MaxFileSize");
        /// double timeout = Tools.GetDoubleValue(session, "Network", "ConnectionTimeout");
        /// </code>
        /// </example>
        public static double GetDoubleValue(Session session, string module, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(session, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToDouble(value);
                return 0.0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị double parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị decimal từ tham số hệ thống theo business module và key.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị decimal hoặc 0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi session, module hoặc key là null</exception>
        public static decimal GetDecimalValue(Session session, string module, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(session, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToDecimal(value, new CultureInfo("en-US"));
                return decimal.Zero;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị decimal parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị double từ tham số hệ thống theo business module và key sử dụng IObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị double hoặc 0.0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace, module hoặc key là null</exception>
        [Obsolete]
        public static double GetDoubleValue(IObjectSpace objectSpace, string module, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(objectSpace, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToDouble(value);
                return 0.0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị double parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị decimal từ tham số hệ thống theo business module và key sử dụng IObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị decimal hoặc 0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace, module hoặc key là null</exception>
        [Obsolete]
        public static decimal GetDecimalValue(IObjectSpace objectSpace, string module, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(objectSpace, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToDecimal(value, new CultureInfo("en-US"));
                return decimal.Zero;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị decimal parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị integer từ tham số hệ thống theo business module và key.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị integer hoặc 0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi session, module hoặc key là null</exception>
        /// <example>
        /// <code>
        /// int maxUsers = Tools.GetIntValue(session, "System", "MaxUsers");
        /// int timeout = Tools.GetIntValue(session, "Network", "TimeoutSeconds");
        /// </code>
        /// </example>
        [Obsolete]
        public static int GetIntValue(Session session, string module, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(session, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToInt32(value);
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị int parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị integer từ tham số hệ thống theo business module và key sử dụng IObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị integer hoặc 0 nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace, module hoặc key là null</exception>
        [Obsolete]
        public static int GetIntValue(IObjectSpace objectSpace, string module, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string value = GetValue(objectSpace, module, key);
                if (!string.IsNullOrEmpty(value))
                    return Convert.ToInt32(value);
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị int parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        [Obsolete]
        public static decimal GetDecimalOrDefault(Session session, string key, decimal defaultValue)
        {
            var enCul = new CultureInfo("en-US");
            string criteria = string.Format("Name=='{0}'", (object)key);
            Module.BusinessObjects.Parameter settingParameter =
                session.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
            if (settingParameter is null)
            {
                settingParameter = new Module.BusinessObjects.Parameter(session);
                settingParameter.Name = key;
                settingParameter.Value = defaultValue.ToString(enCul);
                settingParameter.Session.CommitTransaction();
            }
            return Convert.ToDecimal(settingParameter.Value, enCul);
        }
        [Obsolete]
        public static decimal GetDecimalOrDefault(IObjectSpace objectSpace, string key, decimal defaultValue)
        {
            var enCul = new CultureInfo("en-US");
            string criteria = string.Format("Name=='{0}'", (object)key);
            Module.BusinessObjects.Parameter settingParameter =
                objectSpace.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria));
            if (settingParameter is null)
            {
                settingParameter = objectSpace.CreateObject<Module.BusinessObjects.Parameter>();
                settingParameter.Name = key;
                settingParameter.Value = defaultValue.ToString(enCul);
                settingParameter.Session.CommitTransaction();
            }
            return Convert.ToDecimal(settingParameter.Value, enCul);
        }
        [Obsolete]
        public static int GetIntOrDefault(Session session, string key, int defaultValue)
        {
            return GetParameterValueOrDefault(session, key, defaultValue.ToString(new CultureInfo("en-US"))).GetIntValue();
        }
        [Obsolete]
        public static int GetIntOrDefault(IObjectSpace objectSpace, string key, int defaultValue)
        {
            return GetParameterValueOrDefault(objectSpace, key, defaultValue.ToString(new CultureInfo("en-US"))).GetIntValue();
        }

        /// <summary>
        /// Lấy giá trị boolean từ tham số hệ thống theo business module và key.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị boolean hoặc false nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi session, module hoặc key là null</exception>
        [Obsolete]
        public static bool GetToBooleanValue(Session session, string module, string key)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string str = Tools.GetValue(session, module, key);
                if (!string.IsNullOrEmpty(str))
                    return Convert.ToBoolean(str);
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị boolean parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị boolean từ tham số hệ thống theo business module và key sử dụng IObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="module">Tên business module</param>
        /// <param name="key">Tên khóa tham số</param>
        /// <returns>Giá trị boolean hoặc false nếu không tìm thấy hoặc không convert được</returns>
        /// <exception cref="ArgumentNullException">Khi objectSpace, module hoặc key là null</exception>
        [Obsolete]
        public static bool GetToBooleanValue(IObjectSpace objectSpace, string module, string key)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(module))
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            try
            {
                string str = Tools.GetValue(objectSpace, module, key);
                if (!string.IsNullOrEmpty(str))
                    return Convert.ToBoolean(str);
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy giá trị boolean parameter '{key}' từ module '{module}': {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Thêm tham số vào hệ thống với đầy đủ thông tin.
        /// Tạo parameter mới nếu chưa tồn tại, không tạo trùng lặp.
        /// </summary>

        //public static void AddParameter(IObjectSpace objectSpace, Module.BusinessObjects.SoftwareBusiness softwareBusiness, string name, object value,
        //    Module.BusinessObjects.ParameterFormat format = Module.BusinessObjects.ParameterFormat.Text,
        //    bool isUser = true, int order = 0, string note = null,
        //    DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser user = null, System.Type systemType = null)
        //{
        //    if (objectSpace == null)
        //        throw new ArgumentNullException(nameof(objectSpace));
        //    if (string.IsNullOrEmpty(name))
        //        throw new ArgumentNullException(nameof(name));

        //    if (!(objectSpace is XPObjectSpace))
        //        return;

        //    try
        //    {
        //        CriteriaOperator criteria = CriteriaOperator.Parse("Name = ?", name);
        //        if (user == null)
        //            criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("PermissionPolicyUser is null"));
        //        else
        //            criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("PermissionPolicyUser.Oid = ?", user.Oid));

        //        if (objectSpace.FindObject<Module.BusinessObjects.Parameter>(criteria) == null)
        //        {
        //            Module.BusinessObjects.Parameter parameter = objectSpace.CreateObject<Module.BusinessObjects.Parameter>();
        //            parameter.ParameterFormat = format;
        //            parameter.SoftwareBusiness = softwareBusiness;
        //            parameter.Name = name;
        //            parameter.Value = value is string ? (string)value : Convert.ToString(value, new CultureInfo("en"));
        //            parameter.Order = order;
        //            parameter.Note = note;
        //            parameter.User = isUser;
        //            parameter.PermissionPolicyUser = user;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException($"Lỗi khi thêm parameter '{name}': {ex.Message}", ex);
        //    }
        //}

        /// <summary>
        /// Lấy giá trị tham số hệ thống hoặc tạo mới với giá trị mặc định nếu chưa tồn tại.
        /// Sử dụng IObjectSpace để thao tác database.
        /// </summary>

        [Obsolete]
        public static Module.BusinessObjects.Parameter GetParameterValueOrDefault(IObjectSpace objectSpace, string name, string defaultValue)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            try
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Name = ? and PermissionPolicyUser is null", name);
                Module.BusinessObjects.Parameter settingParameter = objectSpace.FindObject<Module.BusinessObjects.Parameter>(criteria);

                if (settingParameter == null)
                {
                    settingParameter = objectSpace.CreateObject<Module.BusinessObjects.Parameter>();
                    settingParameter.Name = name;
                    settingParameter.Value = defaultValue;
                    settingParameter.Session.CommitTransaction();
                }

                return settingParameter;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy parameter '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số hệ thống hoặc tạo mới với giá trị mặc định nếu chưa tồn tại.
        /// Sử dụng Session để thao tác database.
        /// </summary>

        [Obsolete]
        public static Module.BusinessObjects.Parameter GetParameterValueOrDefault(Session session, string name, string defaultValue)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            try
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Name = ? and PermissionPolicyUser is null", name);
                Module.BusinessObjects.Parameter settingParameter = session.FindObject<Module.BusinessObjects.Parameter>(criteria);

                if (settingParameter == null)
                {
                    settingParameter = new Module.BusinessObjects.Parameter(session);
                    settingParameter.Name = name;
                    settingParameter.Value = defaultValue;
                    session.CommitTransaction();
                }

                return settingParameter;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy parameter '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số của user cụ thể hoặc tạo mới từ system parameter.
        /// Sử dụng IObjectSpace để thao tác database.
        /// </summary>

        [Obsolete]
        public static Module.BusinessObjects.Parameter GetParameterValueOrDefault(IObjectSpace objectSpace, string name, string defaultValue, object userId)
        {
            if (objectSpace == null)
                throw new ArgumentNullException(nameof(objectSpace));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            try
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Name = ? and PermissionPolicyUser.Oid = ?", name, userId);
                Module.BusinessObjects.Parameter parameter = objectSpace.FindObject<Module.BusinessObjects.Parameter>(criteria);

                if (parameter == null)
                {
                    var settingParameter = GetParameterValueOrDefault(objectSpace, name, defaultValue);
                    parameter = settingParameter.CopyToNewParameter();
                    parameter.PermissionPolicyUser = objectSpace.GetObjectByKey<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>(userId);
                    parameter.Session.CommitTransaction();
                }

                return parameter;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy parameter '{name}' cho user '{userId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá trị tham số của user cụ thể hoặc tạo mới từ system parameter.
        /// Sử dụng Session để thao tác database.
        /// </summary>

        [Obsolete]
        public static Module.BusinessObjects.Parameter GetParameterValueOrDefault(Session session, string name, string defaultValue, object userId)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            try
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Name = ? and PermissionPolicyUser.Oid = ?", name, userId);
                Module.BusinessObjects.Parameter parameter = session.FindObject<Module.BusinessObjects.Parameter>(criteria);

                if (parameter == null)
                {
                    var settingParameter = GetParameterValueOrDefault(session, name, defaultValue);
                    parameter = settingParameter.CopyToNewParameter();
                    parameter.PermissionPolicyUser = session.GetObjectByKey<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>(userId);
                    parameter.Session.CommitTransaction();
                }

                return parameter;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy parameter '{name}' cho user '{userId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm tham số vào hệ thống sử dụng Session (phiên bản đơn giản).
        /// Tạo parameter mới nếu chưa tồn tại với tên được chỉ định.
        /// </summary>

        //public static void AddParameter(Session session, Module.BusinessObjects.SoftwareBusiness category, string key, object value,
        //    int displayOrder = 0, Module.BusinessObjects.ParameterFormat format = Module.BusinessObjects.ParameterFormat.Text,
        //    double? doubleValue = null)
        //{
        //    if (session == null)
        //        throw new ArgumentNullException(nameof(session));
        //    if (string.IsNullOrEmpty(key))
        //        throw new ArgumentNullException(nameof(key));

        //    try
        //    {
        //        string criteria = string.Format("Name=='{0}'", key);
        //        if (session.FindObject<Module.BusinessObjects.Parameter>(CriteriaOperator.Parse(criteria)) == null)
        //        {
        //            Module.BusinessObjects.Parameter settingParameter = new Module.BusinessObjects.Parameter(session);
        //            settingParameter.SoftwareBusiness = category;
        //            settingParameter.Name = key;
        //            settingParameter.Value = value is string ? (string)value : Convert.ToString(value, new CultureInfo("en"));
        //            settingParameter.Order = displayOrder;
        //            settingParameter.ParameterFormat = format;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException($"Lỗi khi thêm parameter '{key}': {ex.Message}", ex);
        //    }
        //}

        #endregion
    }
}