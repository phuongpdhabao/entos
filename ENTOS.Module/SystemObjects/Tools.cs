using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Data;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace ENTOS.Module.SystemObjects
{
    /// <summary>
    /// Cung cấp các tiện ích và phương thức hỗ trợ cho toàn bộ hệ thống
    /// Bao gồm các chức năng: xử lý connection string, quản lý tham số, xử lý hình ảnh,
    /// dịch thuật, xử lý âm thanh/video, AI/ML, và nhiều tiện ích khác.
    /// </summary>
    /// <remarks>
    /// Lớp này chứa các phương thức tĩnh được sử dụng rộng rãi trong toàn bộ hệ thống.
    /// Các phương thức được nhóm theo chức năng:
    /// - Database & Connection: PatchConnectionString, GetDatabaseFromConnectionString   
    /// - String Processing: RemoveAccents, GetCode
    /// - Image Processing: ResizeImage, CropImage, RemoveBackground
    /// - Audio/Video: GetAudioFromVideo, TranscribeAudio  
    /// - Translation: Translate, DetectLanguage 
    /// </remarks>
    [Obsolete]
    public static partial class Tools
    {

    /// <summary>
    /// Kiểm tra type có đủ 3 property DataType, DataTypeT1, DataTypeT2 và đều là kiểu DataType hay không.
    /// </summary>
    public static bool HasGenericDataType(Type type, Type dataTypeClrType = null)
    {
        if (type == null) return false;

        // Nếu bạn có class DataType cụ thể, truyền vào qua dataTypeClrType; 
        // nếu không, suy ra từ property "DataType" (nếu có).
        PropertyInfo pDataType   = type.GetProperty("DataType",   BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo pDataTypeT1 = type.GetProperty("DataTypeT1", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo pDataTypeT2 = type.GetProperty("DataTypeT2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (pDataType == null || pDataTypeT1 == null || pDataTypeT2 == null) return false;

        var dtType = dataTypeClrType ?? pDataType.PropertyType;
        if (dtType == null) return false;

        // “đều là kiểu DataType” → so khớp đúng kiểu (không chấp nhận kế thừa)
        return pDataType.PropertyType == dtType
            && pDataTypeT1.PropertyType == dtType
            && pDataTypeT2.PropertyType == dtType;
    }

    /// <summary>
    /// Tính GenericCode theo mô tả. Trả về null nếu type không hợp lệ.
    /// </summary>
    public static string GenericDataTypeCode(object instance)
    {
        if (instance == null) return null;
        var type = instance.GetType();

        // Lấy 3 property
        PropertyInfo pDataType   = type.GetProperty("DataType",   BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo pDataTypeT1 = type.GetProperty("DataTypeT1", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo pDataTypeT2 = type.GetProperty("DataTypeT2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Không đủ 3 property → null
        if (pDataType == null || pDataTypeT1 == null || pDataTypeT2 == null) return null;

        // Đảm bảo cùng kiểu DataType
        var dtType = pDataType.PropertyType;
        if (pDataTypeT1.PropertyType != dtType || pDataTypeT2.PropertyType != dtType) return null;

        // Đọc đối tượng DataType / T1 / T2
        var dt  = pDataType.GetValue(instance);
        var t1  = pDataTypeT1.GetValue(instance);
        var t2  = pDataTypeT2.GetValue(instance);

        if (dt == null) return null; // không có DataType chính thì không thể tính

        // Lấy Code và GenericType từ DataType
        var codeProp  = dtType.GetProperty("Code", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var genProp   = dtType.GetProperty("GenericType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (codeProp == null || genProp == null) return null;

        string baseCode = codeProp.GetValue(dt) as string;

        // Nếu chưa có code thì coi như null
        if (string.IsNullOrWhiteSpace(baseCode)) return null;

        // Đọc enum nullable GenericType? → có thể null
        object genVal = genProp.GetValue(dt); // boxed nullable
        if (genVal == null)
        {
            // GenericType is null → trả về DataType.Code
            return baseCode;
        }

        // So sánh theo tên enum để tránh lệ thuộc strong type (Module.BusinessObjects.GenericType)
        string genName = genVal.ToString(); // "Generic1" hoặc "Generic2"

        // Lấy Code của T1/T2 (có thể null)
        string t1Code = t1 != null ? (dtType.GetProperty("Code")?.GetValue(t1) as string) : null;
        string t2Code = t2 != null ? (dtType.GetProperty("Code")?.GetValue(t2) as string) : null;

        // Regex: “từ chứa ký tự T (upper case)”
        // - Giới hạn bởi biên từ \b
        // - Tránh ăn vào ký tự phân tách generic phổ biến (<, >, , , khoảng trắng)
        var tokenRegex = new Regex(@"\b[^,\s<>]*T[^,\s<>]*\b");

        if (genName == "Generic1")
        {
            if (string.IsNullOrEmpty(t1Code)) return baseCode; // không có T1.Code thì giữ nguyên
            return ReplaceNthMatch(baseCode, tokenRegex, 1, t1Code);
        }
        else if (genName == "Generic2")
        {
            // Thay lần 1 bằng T1.Code, lần 2 bằng T2.Code (nếu thiếu thì giữ nguyên)
            string result = baseCode;
            if (!string.IsNullOrEmpty(t1Code))
                result = ReplaceNthMatch(result, tokenRegex, 1, t1Code);
            if (!string.IsNullOrEmpty(t2Code))
                result = ReplaceNthMatch(result, tokenRegex, 2, t2Code);
            return result;
        }
        else
        {
            // Trường hợp enum khác 2 giá trị nêu trên → giữ nguyên
            return baseCode;
        }
    }

    /// <summary>
    /// Thay thế match thứ n (1-based) của regex trong input bằng replacement.
    /// Nếu không đủ số match thì trả về input gốc.
    /// </summary>
    private static string ReplaceNthMatch(string input, Regex regex, int n, string replacement)
    {
        if (string.IsNullOrEmpty(input) || n <= 0) return input;

        int count = 0;
        return regex.Replace(input, m =>
        {
            count++;
            return count == n ? replacement ?? string.Empty : m.Value;
        });
    }
        #region Database & Connection String Methods

        /// <summary>
        /// Cập nhật tên database trong connection string.
        /// Hỗ trợ cả "Initial Catalog" và "database" parameter cho SQL Server và MySQL.
        /// </summary>
        /// <param name="databaseName">Tên database mới cần thiết lập</param>
        /// <param name="connectionString">Connection string gốc cần cập nhật</param>
        /// <returns>Connection string đã được cập nhật với database name mới</returns>
        /// <exception cref="ArgumentNullException">Khi connectionString là null hoặc empty</exception>
        public static string PatchConnectionString(string databaseName, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            try
            {
                DevExpress.Xpo.DB.Helpers.ConnectionStringParser helper = new DevExpress.Xpo.DB.Helpers.ConnectionStringParser(connectionString);

                if (helper.PartExists("Initial Catalog"))
                {
                    helper.UpdatePartByName("Initial Catalog", databaseName);
                }
                else if (helper.PartExists("database"))
                {
                    helper.UpdatePartByName("database", databaseName);
                }

                return helper.GetConnectionString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi cập nhật connection string: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Trích xuất tên database từ connection string.
        /// Hỗ trợ cả "Initial Catalog" (SQL Server) và "database" (MySQL) parameter.
        /// </summary>
        /// <param name="connectionString">Connection string cần trích xuất tên database</param>
        /// <returns>Tên database hoặc null nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi connectionString là null hoặc empty</exception>
        public static string GetDatabaseFromConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            try
            {
                DevExpress.Xpo.DB.Helpers.ConnectionStringParser helper = new DevExpress.Xpo.DB.Helpers.ConnectionStringParser(connectionString);

                if (helper.PartExists("Initial Catalog"))
                {
                    return helper.GetPartByName("Initial Catalog");
                }
                else if (helper.PartExists("database"))
                {
                    return helper.GetPartByName("database");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi trích xuất database name từ connection string: {ex.Message}", ex);
            }
        }

        #endregion



        #region Static Table & Code Generation


        /// <summary>
        /// Tạo mã code tự động dựa trên pattern và key.
        /// Hỗ trợ tạo mã tuần tự với prefix theo năm và kiểm tra trùng lặp.
        /// </summary>
        /// <param name="type">Loại object cần tạo mã</param>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="id">ID của object hiện tại (để loại trừ khỏi kiểm tra)</param>
        /// <param name="key">Prefix của mã code</param>
        /// <param name="keylength">Độ dài phần số của mã (mặc định: 4)</param>
        /// <param name="filter">Điều kiện lọc bổ sung (mặc định: "")</param>
        /// <param name="propertyName">Tên property chứa mã code (mặc định: "Code")</param>
        /// <param name="isExactType">Có kiểm tra chính xác type không (mặc định: false)</param>
        /// <returns>Mã code mới được tạo, hoặc null nếu không tạo được</returns>
        /// <example>
        /// <code>
        /// // Tạo mã cho khách hàng: KH24001, KH24002, ...
        /// string newCode = Tools.GetCode(typeof(Customer), session, Guid.Empty, "KH", 3);
        /// 
        /// // Tạo mã cho sản phẩm: SP240001, SP240002, ...
        /// string productCode = Tools.GetCode(typeof(Product), session, Guid.Empty, "SP", 4);
        /// </code>
        /// </example>
        public static string GetCode(Type type, Session session, Guid id, string key, int keylength = 4,
                string filter = "", string propertyName = "Code", bool isExactType = false)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            //if (string.IsNullOrEmpty(key))
            //    throw new ArgumentNullException(nameof(key));

            try
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                    return null;

                string resultCode = "";
                string keyPrefix = string.IsNullOrEmpty(filter)
                    ? string.Format(key + "{0:yy}", DateTime.Now)
                    : key;

                var criteriaOperator = CriteriaOperator.Parse($"Oid <> ? and StartsWith([{propertyName}], ?){filter}",
                    id, keyPrefix);

                if (isExactType)
                {
                    criteriaOperator = CriteriaOperator.And(criteriaOperator,
                        CriteriaOperator.Parse("IsExactType(This,?)", type.FullName));
                }

                var sort = new SortProperty(propertyName, SortingDirection.Descending);
                var lastedCode = GetLastedCode(type, session, property, criteriaOperator, sort, false);
                var lastedTransactionCode = GetLastedCode(type, session, property, criteriaOperator, sort, true);

                if (string.IsNullOrEmpty(lastedCode))
                    lastedCode = lastedTransactionCode;
                else if (!string.IsNullOrEmpty(lastedTransactionCode) && lastedCode.CompareTo(lastedTransactionCode) < 0)
                    lastedCode = lastedTransactionCode;

                if (!string.IsNullOrEmpty(lastedCode) &&
                    int.TryParse(lastedCode.Substring(keyPrefix.Length), out int lastNumber))
                {
                    for (int i = 0; i < 100; i++) // Tối đa 100 lần thử
                    {
                        lastNumber++;
                        var builder = new StringBuilder(keyPrefix);
                        builder.Append('0', keylength - lastNumber.ToString().Length);
                        builder.Append(lastNumber);
                        resultCode = builder.ToString();

                        if (resultCode.Length > keylength + key.Length)
                        {
                            var overCriteriaOperator = CriteriaOperator.Parse(
                                $"Oid <> ? and Len([{propertyName}]) >= ? and StartsWith([{propertyName}], ?){filter}",
                                id, keylength + key.Length, keyPrefix);

                            XPCollection overXpCollection = new XPCollection(session, type, overCriteriaOperator,
                                new SortProperty[] { new SortProperty(propertyName, SortingDirection.Descending) })
                            {
                                TopReturnedObjects = 1
                            };

                            if (overXpCollection.Count > 0)
                            {
                                string overCode = property.GetValue(overXpCollection[0]) as string;
                                if (!string.IsNullOrEmpty(overCode) &&
                                    int.TryParse(overCode.Substring(keyPrefix.Length), out lastNumber))
                                {
                                    lastNumber++;
                                    resultCode = keyPrefix + lastNumber.ToString();
                                }
                            }
                        }

                        var duplicateCheck = session.FindObject(
                            PersistentCriteriaEvaluationBehavior.InTransaction,
                            type,
                            CriteriaOperator.And(criteriaOperator,
                                CriteriaOperator.Parse($"{propertyName} = ?", resultCode)));

                        if (duplicateCheck == null)
                            break;
                    }
                }
                else
                {
                    // Tạo mã đầu tiên
                    int firstNumber = 1;
                    var builder = new StringBuilder(keyPrefix);
                    builder.Append('0', keylength - firstNumber.ToString().Length);
                    builder.Append(firstNumber);
                    resultCode = builder.ToString();
                }

                return resultCode;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi tạo mã code cho type '{type.Name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy mã code lớn nhất từ database dựa trên criteria và sort.
        /// Phương thức hỗ trợ cho GetCode().
        /// </summary>
        /// <param name="type">Loại object</param>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="property">Property chứa mã code</param>
        /// <param name="criteriaOperator">Điều kiện lọc</param>
        /// <param name="sort">Cách sắp xếp</param>
        /// <param name="inTransaction">Có kiểm tra trong transaction không</param>
        /// <returns>Mã code lớn nhất hoặc null nếu không có</returns>
        private static string GetLastedCode(Type type, Session session, PropertyInfo property,
            CriteriaOperator criteriaOperator, SortProperty sort, bool inTransaction = false)
        {
            try
            {
                XPCollection xpCollection = new XPCollection(
                    inTransaction ? PersistentCriteriaEvaluationBehavior.InTransaction : PersistentCriteriaEvaluationBehavior.BeforeTransaction,
                    session, type, criteriaOperator);

                xpCollection.Sorting.Add(sort);
                xpCollection.TopReturnedObjects = 1;

                if (xpCollection.Count > 0)
                {
                    return property.GetValue(xpCollection[0]) as string;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi lấy mã code cuối cùng: {ex.Message}", ex);
            }
        }

        #endregion

        #region Number & Code Utilities

        /// <summary>
        /// Tạo mã số với độ dài cố định, thêm số 0 đằng trước nếu cần.
        /// </summary>
        /// <param name="position">Số cần chuyển thành mã</param>
        /// <param name="length">Độ dài mong muốn của mã</param>
        /// <returns>Mã số với độ dài cố định</returns>
        /// <exception cref="ArgumentException">Khi position âm hoặc length nhỏ hơn 1</exception>
        /// <example>
        /// <code>
        /// string code1 = Tools.GetNumberCode(123, 5);  // "00123"
        /// string code2 = Tools.GetNumberCode(45, 3);   // "045"
        /// string code3 = Tools.GetNumberCode(999, 2);  // "999" (không thêm 0)
        /// </code>
        /// </example>
        public static string GetNumberCode(int position, int length)
        {
            if (position < 0)
                throw new ArgumentException("Position không thể âm", nameof(position));
            if (length < 1)
                throw new ArgumentException("Length phải lớn hơn 0", nameof(length));

            int currentLength = GetNumberMaxLength(position);
            string result = position.ToString("D");

            // Thêm số 0 đằng trước
            for (int index = currentLength; index < length; index++)
                result = "0" + result;

            return result;
        }

        /// <summary>
        /// Lấy độ dài tối đa của một số khi được chuyển thành chuỗi.
        /// </summary>
        /// <param name="number">Số cần kiểm tra độ dài</param>
        /// <returns>Số chữ số của number</returns>
        /// <example>
        /// <code>
        /// int length1 = Tools.GetNumberMaxLength(123);   // 3
        /// int length2 = Tools.GetNumberMaxLength(9999);  // 4
        /// int length3 = Tools.GetNumberMaxLength(1);     // 1
        /// </code>
        /// </example>
        public static int GetNumberMaxLength(int number)
        {
            return number.ToString("D").Length;
        }

        #endregion




        /// <summary>
        /// Làm tròn số double theo số chữ số thập phân hoặc hàng chục.
        /// </summary>
        /// <param name="number">Số cần làm tròn</param>
        /// <param name="round">Số chữ số thập phân (dương) hoặc hàng chục (âm)</param>
        /// <returns>Số đã được làm tròn</returns>
        /// <example>
        /// <code>
        /// double result1 = Tools.RoundNumber(123.456, 2);  // 123.46
        /// double result2 = Tools.RoundNumber(123.456, -1); // 120.0
        /// double result3 = Tools.RoundNumber(123.456, -2); // 100.0
        /// </code>
        /// </example>
        public static double RoundNumber(double number, int round)
        {
            if (round >= 0)
                return Math.Round(number, round, MidpointRounding.AwayFromZero);
            int num = 1;
            for (int index = 0; index > round; --index)
                num *= 10;
            return Math.Round(number / (double)num, 0, MidpointRounding.AwayFromZero) * (double)num;
        }

        /// <summary>
        /// Làm tròn số decimal theo số chữ số thập phân hoặc hàng chục.
        /// </summary>
        /// <param name="number">Số cần làm tròn</param>
        /// <param name="round">Số chữ số thập phân (dương) hoặc hàng chục (âm)</param>
        /// <returns>Số đã được làm tròn</returns>
        /// <example>
        /// <code>
        /// decimal result1 = Tools.RoundNumber(123.456m, 2);  // 123.46
        /// decimal result2 = Tools.RoundNumber(123.456m, -1); // 120.0
        /// decimal result3 = Tools.RoundNumber(123.456m, -2); // 100.0
        /// </code>
        /// </example>
        public static Decimal RoundNumber(Decimal number, int round)
        {
            if (round >= 0)
                return Math.Round(number, round, MidpointRounding.AwayFromZero);
            int num = 1;
            for (int index = 0; index > round; --index)
                num *= 10;
            return Math.Round(number / (Decimal)num, 0, MidpointRounding.AwayFromZero) * (Decimal)num;
        }


        /// <summary>
        /// Tính toán biểu thức toán học từ chuỗi.
        /// Hỗ trợ các phép toán cơ bản và ký hiệu %.
        /// </summary>
        /// <param name="expression">Biểu thức toán học dạng chuỗi</param>
        /// <returns>Kết quả tính toán hoặc -1 nếu có lỗi</returns>
        public static double Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return -1.0;

            double num;
            try
            {
                if (expression.Contains("%"))
                    expression = expression.Replace("%", "/ 100");
                DataTable dataTable = new DataTable();
                DataColumn column = new DataColumn("Eval", typeof(double), expression);
                dataTable.Columns.Add(column);
                dataTable.Rows.Add((object)0);
                num = (double)dataTable.Rows[0]["Eval"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1.0;
            }

            return num;
        }

        /// <summary>
        /// Tính toán biểu thức toán học từ chuỗi sử dụng DataTable.Compute.
        /// </summary>
        /// <param name="input">Biểu thức toán học dạng chuỗi</param>
        /// <returns>Kết quả tính toán hoặc -1 nếu có lỗi</returns>
        /// <example>
        /// <code>
        /// double result1 = Tools.Eval("2 + 3 * 4");     // 14
        /// double result2 = Tools.Eval("10 / 5");        // 2
        /// double result3 = Tools.Eval("(2 + 3) * 4");  // 20
        /// </code>
        /// </example>
        /// <summary>
        /// Tính toán biểu thức toán học từ chuỗi sử dụng DataTable.
        /// </summary>
        /// <param name="input">Biểu thức toán học dạng chuỗi</param>
        /// <returns>Kết quả tính toán hoặc -1 nếu có lỗi</returns>
        public static double Eval(String input)
        {
            if (string.IsNullOrEmpty(input))
                return -1.0;

            try
            {
                DataTable dt = new DataTable();
                var v = dt.Compute(input, "");
                if (v != null)
                    return Convert.ToDouble(v);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Lấy giá trị thuộc tính từ object theo tên thuộc tính.
        /// Hỗ trợ truy cập thuộc tính lồng nhau bằng dấu chấm.
        /// </summary>
        /// <param name="obj">Object cần lấy thuộc tính</param>
        /// <param name="name">Tên thuộc tính (có thể lồng nhau bằng dấu chấm)</param>
        /// <returns>Giá trị thuộc tính hoặc null nếu không tìm thấy</returns>
        public static object GetPropertyValue(this object obj, string name)
        {
            string str = name;
            char[] chArray = new char[1] { '.' };
            foreach (string name1 in str.Split(chArray))
            {
                if (obj == null)
                    return (object)null;
                PropertyInfo property = obj.GetType().GetProperty(name1);
                if (property == (PropertyInfo)null)
                    return (object)null;
                obj = property.GetValue(obj, (object[])null);
            }

            return obj;
        }

        /// <summary>
        /// Lấy CriteriaOperator cho FilterCriteria theo type và field hoặc viewId.
        /// </summary>
        /// <param name="currentType">Kiểu dữ liệu cần lọc</param>
        /// <param name="fieldName">Tên field (tùy chọn)</param>
        /// <param name="viewId">ID của view (tùy chọn)</param>
        /// <param name="session">Session để thao tác database</param>
        /// <returns>CriteriaOperator kết hợp từ các FilterCriteria</returns>
        [Obsolete]
        public static CriteriaOperator GetCriteriaOperator(Type currentType, string fieldName = null, string viewId = null, Session session = null)
        {
            if (currentType != null)
            {
                var criteria = CriteriaOperator.Parse("Active and ObjectType = ?", currentType);
                if (currentType.BaseType != null)
                {
                    criteria = CriteriaOperator.Or(criteria,
                        CriteriaOperator.Parse("ObjectType = ? and AllowInherit",
                            currentType.BaseType));
                }
                if (!string.IsNullOrEmpty(fieldName))
                {
                    criteria = CriteriaOperator.And(criteria,
                        CriteriaOperator.Parse("IsListView = False and EndsWith([Field], ?)",
                            System.Environment.NewLine + fieldName));
                }
                else if (!string.IsNullOrEmpty(viewId))
                {
                    criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("IsListView and ViewId = ?", viewId));
                }
                else
                {
                    return null;
                }

                IList<FilterCriteria> filtersCriteria = null;
                if (session != null)
                {
                    filtersCriteria = new XPCollection<FilterCriteria>(session, criteria).ToList();
                }
                if (filtersCriteria == null)
                {
                    IObjectSpace objectSpace = XPObjectSpace.FindObjectSpaceByObject(SecuritySystem.CurrentUser);
                    if (session == null)
                    {
                        session = ((XPObjectSpace)objectSpace).Session;
                    }
                    filtersCriteria = objectSpace.GetObjects<FilterCriteria>(criteria);
                }
                if (filtersCriteria != null && filtersCriteria.Count > 0)
                {
                    CriteriaOperator result = null;
                    foreach (var filterCriteria in filtersCriteria)
                    {
                        if (!string.IsNullOrEmpty(filterCriteria.Condition))
                        {
                            result = CriteriaOperator.And(result, session.ParseCriteria(filterCriteria.Condition));
                        }
                    }
                    //if (!(result is null) && session != null)
                    //    return session.ParseCriteria(result.LegacyToString());
                    return result;
                }
            }

            return null;
        }


        /// <summary>
        /// Lấy caption của giá trị enum.
        /// </summary>
        /// <param name="type">Kiểu enum</param>
        /// <param name="obj">Giá trị enum</param>
        /// <returns>Caption của enum hoặc null nếu không tìm thấy</returns>
        [Obsolete]
        public static string GetCaptionEnum(Type type, object obj)
        {
            if (obj != null)
            {
                if (type != null)
                {
                    EnumDescriptor myDescriptor = new EnumDescriptor(type);
                    foreach (object enumValue in myDescriptor.Values)
                    {
                        if (obj.Equals(enumValue))
                            return myDescriptor.GetCaption(enumValue);
                    }
                }
                return obj.ToString();
            }
            return null;
        }

        /// <summary>
        /// Lấy thời gian hiện tại từ server database.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <returns>Thời gian hiện tại từ server</returns>
        [Obsolete]
        public static DateTime GetDateTimeNowFromServer(Session session)
        {
            return (DateTime)session.Evaluate(typeof(XPObjectType),
                (CriteriaOperator)new FunctionOperator(FunctionOperatorType.Now, new CriteriaOperator[0]),
                (CriteriaOperator)null);
        }

        /// <summary>
        /// Lấy giá trị thuộc tính từ object với hỗ trợ truy cập lồng nhau và gọi method.
        /// </summary>
        /// <param name="currentObject">Object cần lấy thuộc tính</param>
        /// <param name="property">Tên thuộc tính hoặc method (có thể lồng nhau bằng dấu chấm)</param>
        /// <returns>Giá trị thuộc tính hoặc null nếu không tìm thấy</returns>
        [Obsolete]
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

        public static IOrderedEnumerable<KeyValuePair<object, decimal>> ReSortDictionaryByValue(IDictionary<object, decimal> dictionarys, bool ascending)
        {
            if (ascending)
            {
                return from pair in dictionarys orderby pair.Value ascending select pair;
            }
            else
            {
                return from pair in dictionarys orderby pair.Value descending select pair;
            }

        }

        private static MessageOptions messageOptions = null;
        public static void ShowMessage(XafApplication application, string caption, string message,
            InformationType informationType = InformationType.Success, int duration = 2000)
        {
            if (messageOptions == null)
                messageOptions = new MessageOptions();
            messageOptions.Duration = duration;
            messageOptions.Message = message;
            messageOptions.Type = informationType;
            messageOptions.Web.Position = InformationPosition.Right;
            messageOptions.Win.Caption = caption;
            messageOptions.Win.Type = WinMessageType.Alert;
            application.ShowViewStrategy.ShowMessage(messageOptions);
        }

        public static object GetMasterObjectFromView(DevExpress.ExpressApp.View view)
        {
            if (view is ListView && ((ListView)view).CollectionSource is PropertyCollectionSource)
            {
                return ((PropertyCollectionSource)((ListView)view).CollectionSource).MasterObject;
            }
            return null;
        }

        public static object CallObjectMethod(object obj, string method, object[] parameters = null)
        {
            if (obj != null && !string.IsNullOrEmpty(method))
            {
                Type type = obj.GetType();
                System.Reflection.MethodInfo theMethod =
                    type.GetMethod(method);
                if (theMethod != null)
                {
                    if (theMethod.ContainsGenericParameters || theMethod.GetParameters().Length > 0)
                        return theMethod.Invoke(obj, parameters);
                    else
                        return
                            theMethod.Invoke(obj, null);
                }
            }

            return null;
        }

        public static void RefreshGridView(View view)
        {
            if (view is ListView && ((ListView)view).Editor != null)
            {
                //var gridView =
                //    ((ListView)view).Editor.GetPropertyValue("GridView") as DevExpress.XtraGrid.Views.Grid.GridView;
                var gridView =
                    ((ListView)view).Editor.GetPropertyValue("GridView");
                if (gridView != null)
                {
                    //gridView.RefreshData();
                    var methodRefreshData = gridView.GetType().GetMethod("RefreshData");
                    if (methodRefreshData != null)
                        methodRefreshData.Invoke(gridView, null);

                }
            }
        }
        public static string GetTypeImage(Type currentType)
        {
            if (currentType != null)
            {
                var typeInfo = XafTypesInfo.Instance.FindTypeInfo(currentType);
                if (typeInfo != null)
                {
                    var imageNameAttribute = typeInfo.FindAttribute<ImageNameAttribute>();
                    if (imageNameAttribute != null)
                    {
                        return imageNameAttribute.ImageName;
                    }
                }
            }
            return null;
        }

        public static bool IsNumber(Type type)
        {
            if (type == null) return false;
            if (type.IsEnum) return false;
            // from http://stackoverflow.com/a/5182747/172132
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumber(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        public static bool IsNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            foreach (var c in text)
            {
                if (!char.IsDigit(c) && !c.Equals(',') && !c.Equals(','))
                    return false;
            }
            return true;
        }

        public static bool IsNumber(string text, char[] unitCharacter)
        {
            if (string.IsNullOrEmpty(text)) return false;
            foreach (var c in text)
            {
                if (!char.IsDigit(c) && !c.Equals('.') && !c.Equals(',') && !unitCharacter.Contains(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Trích xuất số từ chuỗi văn bản.
        /// Loại bỏ tất cả ký tự không phải số và trả về số đầu tiên tìm được.
        /// </summary>
        /// <param name="text">Chuỗi văn bản cần trích xuất số</param>
        /// <returns>Số được trích xuất hoặc null nếu không tìm thấy</returns>
        /// <example>
        /// var number1 = Tools.GetNumberInText("abc123def"); // 123
        /// var number2 = Tools.GetNumberInText("Price: $1,234.56"); // 1234
        /// var number3 = Tools.GetNumberInText("No number here"); // null
        /// </example>
        public static int? GetNumberInText(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (Char.IsDigit(c))
                    sb.Append(c);
            }
            var resultString = sb.ToString();
            if (!string.IsNullOrEmpty(resultString))
                return Int32.Parse(resultString);
            return null;
        }

        public static int GetFirstCharLower(string content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (char.IsSymbol(content[i]))
                    continue;
                if (char.IsLetter(content[i]))
                {
                    if (char.IsLower(content[i]))
                        return i;
                    else return -1;
                    //break;
                }
                else if (char.IsNumber(content[i]))
                {
                    break;
                }
                else if (char.IsWhiteSpace(content[0]))
                {
                    break;
                }
            }
            return -1;
        }
        /// <summary>
        /// Chuyển đổi tọa độ từ định dạng độ-phút-giây sang số thập phân.
        /// </summary>
        /// <param name="oldValue">Chuỗi tọa độ dạng độ-phút-giây</param>
        /// <returns>Số thập phân tương ứng hoặc giá trị gốc nếu không chuyển đổi được</returns>
        public static object ConvertingDecimalDegrees(string oldValue)
        {
            if (!string.IsNullOrEmpty(oldValue))
            {
                var newValue = ((string)oldValue).Replace('\'', '′').Replace('\"', '″').Trim();
                int degreesIndex = newValue.IndexOf('°');
                int minutesIndex = newValue.IndexOf('′');
                int secondsIndex = newValue.IndexOf('″');
                if (degreesIndex > 0 && minutesIndex > 0 && degreesIndex < minutesIndex &&
                    (minutesIndex < secondsIndex || secondsIndex < 0))
                {
                    var degreesText = newValue.Substring(0, degreesIndex).Trim();
                    var minutesText = newValue.Substring(degreesIndex + 1, minutesIndex - degreesIndex - 1).Trim();
                    var secondsText = secondsIndex < 0 ? "0" : newValue.Substring(minutesIndex + 1, secondsIndex - minutesIndex - 1).Trim();
                    try
                    {
                        double degrees = Convert.ToDouble(degreesText);
                        double minutes = Convert.ToDouble(minutesText);
                        double seconds = Convert.ToDouble(secondsText);
                        double result = Math.Round(degrees + minutes / 60 + seconds / 3600, 7,
                            MidpointRounding.AwayFromZero);
                        if (newValue.EndsWith("S") || newValue.EndsWith("W") || newValue.EndsWith("T"))
                        {
                            result = result * (-1);
                        }
                        return result;
                    }
                    catch (Exception) { }
                }
            }

            return oldValue;
        }

        /// <summary>
        /// Lấy object từ session theo key value sử dụng DefaultLookupField.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="type">Kiểu dữ liệu của object</param>
        /// <param name="lookupKey">Giá trị key để tìm kiếm</param>
        /// <returns>Object tìm được hoặc null</returns>
        public static object GetObjectFromKeyValue(Session session, Type type, object lookupKey)
        {
            if (type.IsSubclassOf(typeof(PersistentBase)) && lookupKey != null)
            {
                var lookupDefault = session.FindObject<DefaultLookupField>(
                    CriteriaOperator.Parse(nameof(DefaultLookupField.ObjectType) + " = ?",
                        type));
                if (lookupDefault != null && lookupDefault.Field != null)
                {
                    return session.FindObject(type,
                        CriteriaOperator.Parse(lookupDefault.Field.Value + " = ?", lookupKey));
                }
            }
            return null;
        }

        /// <summary>
        /// Xác thực chứng chỉ SSL server.
        /// Luôn trả về true để bỏ qua lỗi chứng chỉ.
        /// </summary>
        /// <param name="sender">Object gửi sự kiện</param>
        /// <param name="certificate">Chứng chỉ SSL</param>
        /// <param name="chain">Chuỗi chứng chỉ</param>
        /// <param name="sslPolicyErrors">Lỗi chính sách SSL</param>
        /// <returns>Luôn trả về true</returns>
        public static bool ValidateServerCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Tải file từ link URL hoặc đường dẫn local.
        /// </summary>
        /// <param name="link">URL hoặc đường dẫn file</param>
        /// <returns>Nội dung file dạng byte array hoặc null nếu có lỗi</returns>
        public static byte[] GetFileFromLink(string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                if (link.StartsWith("http") || link.StartsWith("www"))
                {
                    byte[] result = null;

                    try
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                        using (var client = new WebClient())
                        {
                            client.Headers.Add(HttpRequestHeader.UserAgent, "Other");
                            client.UseDefaultCredentials = true;
                            client.Encoding = System.Text.Encoding.UTF8;
                            //client.FixSSL();
                            result = client.DownloadData(link);
                        }
                        //System.Net.ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13; 
                        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                        //using (var client = new CookieWebClient())
                        //{
                        //    client.UseDefaultCredentials = true;
                        //    client.Encoding = System.Text.Encoding.UTF8;
                        //    client.FixSSL();                            
                        //    result = client.DownloadData(link);
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    try
                    {
                        if (result is null)
                        {
                            var httpRequest = (HttpWebRequest)WebRequest.Create(link);
                            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                            var downloadStrm = httpResponse.GetResponseStream();
                            var donloadStrm = new StreamReader(downloadStrm);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                donloadStrm.BaseStream.CopyTo(ms);
                                //var g= System.Convert.ToBase64String(ms.ToArray());
                                result = ms.ToArray();
                            }
                            // Do whatever you want with donloadStrm
                            downloadStrm.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    return result;
                }
                else if (File.Exists(link))
                {
                    //Link từ máy tính
                    return File.ReadAllBytes(link);
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy tên file cache cho URL từ thư mục cache.
        /// </summary>
        /// <param name="session">Session để lấy thông tin thư mục cache</param>
        /// <param name="url">URL cần tạo tên file cache</param>
        /// <returns>Tên file cache nếu tồn tại, null nếu không</returns>
        public static string GetCacheFileName(Session session, string url)
        {
            var folder = GetValueOrDefault(session, "CacheHtmlFolder", "\\\\dc\\Habao$\\Company\\HBD");
            var fileName = GetFileName(url, folder);
            if (System.IO.File.Exists(fileName))
                return fileName;
            return null;
        }

        /// <summary>
        /// Tạo tên file từ URL trong thư mục chỉ định.
        /// </summary>
        /// <param name="url">URL cần tạo tên file</param>
        /// <param name="folder">Thư mục chứa file</param>
        /// <param name="createFolder">Có tạo thư mục nếu chưa tồn tại không</param>
        /// <returns>Đường dẫn file đầy đủ</returns>
        public static string GetFileName(string url, string folder, bool createFolder = false)
        {
            System.Uri myUri = new System.Uri(url);
            string directory = folder + "\\" + myUri.Host;
            if (createFolder && !System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            var fileName = directory + @"\";
            fileName += GetFileName(url);
            return fileName;
        }

        /// <summary>
        /// Tạo tên file từ URL.
        /// </summary>
        /// <param name="url">URL cần tạo tên file</param>
        /// <returns>Tên file được tạo từ URL</returns>
        public static string GetFileName(string url)
        {
            System.Uri myUri = new System.Uri(url);
            var fileName = "";
            if (!string.IsNullOrEmpty(myUri.PathAndQuery))
                fileName += myUri.PathAndQuery.Substring(1).Replace('\\', ';').Replace('/', ';').Replace('*', ';').Replace('?', ';').Replace('<', ';').Replace('>', ';').Replace('|', ';');
            else
                fileName += System.Guid.NewGuid();
            if (fileName.Length > 210)
                fileName = fileName.Substring(0, 210);
            if (fileName.EndsWith(".html") || fileName.EndsWith(".htm"))
                return fileName;
            //var fileInfo = new System.IO.FileInfo(url);
            //if (string.IsNullOrEmpty(fileInfo.Extension))
            fileName += ".html";
            return fileName;
        }



        /// <summary>
        /// Dịch văn bản sử dụng Google Translate API
        /// </summary>
        /// <param name="input">Văn bản cần dịch</param>
        /// <param name="destination">Ngôn ngữ đích</param>
        /// <param name="source">Ngôn ngữ nguồn</param>
        /// <returns>Văn bản đã được dịch</returns>
        public static string TranslateText(string input, string destination = "vi", string source = "en")
        {
            if (string.IsNullOrEmpty(input))
                return null;
            string url = System.String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             source, destination, System.Web.HttpUtility.UrlEncode(input));
            string translation = "";
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                //HttpClient httpClient = new HttpClient();
                string result = client.GetStringAsync(url).Result;
                //var jsonData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<dynamic>>(result);
                var jsonData = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<dynamic>>(result);
                //var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<dynamic>>(result);
                // Extract just the first array element (This is the only data we are interested in)
                if (jsonData[0] is null)
                    return null;
                var translationItems = jsonData[0];
                if (translationItems is System.Text.Json.JsonElement translationItemsJsonElement)
                {
                    if (translationItemsJsonElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement item in translationItemsJsonElement.EnumerateArray())
                        {
                            if (item.ValueKind == JsonValueKind.Array)
                            {
                                var innerArray = item.EnumerateArray();
                                var enumerator = innerArray.GetEnumerator();
                                if (enumerator.MoveNext())
                                {
                                    string translatedText = enumerator.Current.GetString();
                                    translation += " " + translatedText;
                                }
                            }
                        }
                    }
                }
                else
                    foreach (object item in translationItems)
                    {
                        System.Collections.IEnumerable translationLineObject = item as System.Collections.IEnumerable;
                        if (translationLineObject != null)
                        {
                            System.Collections.IEnumerator translationLineString = translationLineObject.GetEnumerator();
                            translationLineString.MoveNext();
                            translation += string.Format(" {0}", System.Convert.ToString(translationLineString.Current));
                        }
                    }

                if (translation.Length > 1)
                {
                    translation = translation.Substring(1);
                    if (input.ToLower() == input)
                    {
                        translation = translation.ToLower();
                    }
                    else if (input.ToUpper() == input)
                    {
                        translation = translation.ToUpper();
                    }
                    else if (char.IsUpper(input[0]) && input.Length > 1 && !char.IsUpper(input[1]))
                    {
                        //Trường hợp viết hoa ký tự đầu
                        translation = translation.Substring(0, 1).ToUpper() + translation.Substring(1);
                    }
                };
            }

            return translation;
        }


        public static string LineByLineTranslate(string input, string destination = "vi", string source = "en")
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string normalized = input.Replace("\r\n", "\n");
            var lines = normalized.Split('\n').ToList();
            List<string> finalTranslatedLines = new();

            ProcessChunkRecursive(lines, source, destination, finalTranslatedLines);

            return string.Join("\n", finalTranslatedLines);
        }
        private static void ProcessChunkRecursive(List<string> chunkLines, string sourceLang, string targetLang, List<string> resultCollector, int errorDepth = 0)
        {
            try
            {
                string joined = string.Join("\n", chunkLines);
                string translated = Module.SystemObjects.Tools.TranslateText(joined, destination: targetLang, source: sourceLang);
                var translatedLines = translated.Replace("\r\n", "\n").Split('\n');

                if (translatedLines.Length == chunkLines.Count)
                {
                    resultCollector.AddRange(translatedLines);
                }
                else
                {
                    SplitAndRecurse(chunkLines, sourceLang, targetLang, resultCollector, errorDepth);
                }
            }
            catch
            {
                SplitAndRecurse(chunkLines, sourceLang, targetLang, resultCollector, errorDepth + 1);
            }
        }
        private static void SplitAndRecurse(List<string> chunkLines, string sourceLang, string targetLang, List<string> resultCollector, int errorDepth)
        {
            if (errorDepth >= 5)
            {
                // Dừng chia tiếp, tránh mất thời gian nếu API bị ngắt hoặc lỗi định kỳ
                for (int i = 0; i < chunkLines.Count; i++)
                {
                    resultCollector.Add(""); // fallback an toàn
                }
                return;
            }

            if (chunkLines.Count == 1)
            {
                try
                {
                    var translatedLine = Module.SystemObjects.Tools.TranslateText(chunkLines[0], destination: targetLang, source: sourceLang);
                    resultCollector.Add(translatedLine);
                }
                catch
                {
                    resultCollector.Add(""); // fallback an toàn cho dòng đơn lẻ
                }
            }
            else
            {
                int mid = chunkLines.Count / 2;
                var firstHalf = chunkLines.GetRange(0, mid);
                var secondHalf = chunkLines.GetRange(mid, chunkLines.Count - mid);

                ProcessChunkRecursive(firstHalf, sourceLang, targetLang, resultCollector, errorDepth);
                ProcessChunkRecursive(secondHalf, sourceLang, targetLang, resultCollector, errorDepth);
            }
        }
        /// <summary>
        /// Chuyển đổi văn bản thành số thập phân
        /// </summary>
        /// <param name="text">Văn bản cần chuyển đổi</param>
        /// <returns>Số thập phân hoặc null nếu không thể chuyển đổi</returns>
        public static decimal? ConvertTextToNumber(string text)
        {
            try
            {
                var dotIndex = text.LastIndexOf('.');
                var commaIndex = text.LastIndexOf(',');
                if (dotIndex > 0)
                {
                    if (commaIndex < 0)
                    {
                        if (text.Length >= 5 && (text.Length - (dotIndex + 1)) % 3 == 0 && text[0] != '0')
                        {
                            var cultureInfo = new System.Globalization.CultureInfo("vi");
                            return Decimal.Parse(text, cultureInfo);
                        }
                        else
                        {
                            var cultureInfo = new System.Globalization.CultureInfo("en");
                            return Decimal.Parse(text, cultureInfo);
                        }
                    }
                    else
                    {
                        if (dotIndex < commaIndex)
                        {
                            var cultureInfo = new System.Globalization.CultureInfo("vi");
                            return Decimal.Parse(text, cultureInfo);
                        }
                        else
                        {
                            var cultureInfo = new System.Globalization.CultureInfo("en");
                            return Decimal.Parse(text, cultureInfo);
                        }

                    }
                }
                else if (commaIndex > 0)
                {
                    if (text.Length >= 5 && (text.Length - (commaIndex + 1)) % 3 == 0)
                    {
                        var cultureInfo = new System.Globalization.CultureInfo("en");
                        return Decimal.Parse(text, cultureInfo);
                    }
                    else
                    {
                        var cultureInfo = new System.Globalization.CultureInfo("vi");
                        return Decimal.Parse(text, cultureInfo);
                    }
                }
                else
                {
                    return Int32.Parse(text);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// Thử chuyển đổi văn bản thành số thập phân, lấy số đầu tiên từ trái sang phải
        /// </summary>
        /// <param name="text">Văn bản cần chuyển đổi</param>
        /// <returns>Số thập phân hoặc null nếu không thể chuyển đổi</returns>
        public static decimal? TryConvertTextToNumber(string text)
        {
            //nếu có 2 hoặc nhiều giá trị số thì chọn giá trị đầu từ trái sang phải
            //Lấy ký tự số
            decimal numberValue = 0;
            string number = "";
            bool hasNumber = false;
            foreach (var c in text)
            {
                if (char.IsNumber(c))
                {
                    hasNumber = true;
                    number += c;
                }
                else if (hasNumber)
                {
                    if (c == '.' || c == ',')
                        number += c;
                    else
                        break;
                }
            }
            if (hasNumber)
            {
                return ConvertTextToNumber(number);
            }
            return null;
        }
        /// <summary>
        /// Dịch nội dung HTML từ ngôn ngữ nguồn sang ngôn ngữ đích
        /// </summary>
        /// <param name="input">Nội dung HTML cần dịch</param>
        /// <param name="destination">Ngôn ngữ đích</param>
        /// <param name="source">Ngôn ngữ nguồn</param>
        /// <returns>Nội dung HTML đã được dịch</returns>
        public static string TranslateHtml(string input, string destination = "vi", string source = "en")
        {
            HtmlAgilityPack.HtmlDocument currentDoc = new HtmlAgilityPack.HtmlDocument();
            currentDoc.LoadHtml(input);
            TranslateHtmlElement(currentDoc.DocumentNode, destination, source);
            return currentDoc.DocumentNode.OuterHtml;
        }

        private static void TranslateHtmlElement(HtmlAgilityPack.HtmlNode htmlNode, string destination = "vi", string source = "en")
        {

            try
            {
                bool translate = false;
                if (string.IsNullOrEmpty(htmlNode.InnerText))
                {
                    return;
                }
                if (htmlNode.InnerText == htmlNode.InnerHtml)
                {
                    translate = true;
                }
                else if (htmlNode.ChildNodes.Count > 0)
                {
                    if (htmlNode.ChildNodes.Count == 1 && !htmlNode.Name.Equals("div"))
                    {

                    }
                    foreach (var childNode in htmlNode.ChildNodes)
                        TranslateHtmlElement(childNode, destination, source);
                }
                else if (htmlNode is HtmlAgilityPack.HtmlTextNode)
                {
                    translate = true;
                }
                else
                {

                }
                if (translate)
                {
                    if (!string.IsNullOrEmpty(htmlNode.InnerHtml))
                    {
                        if (htmlNode.InnerHtml.Length < 20)
                        {
                            //Kiểm tra xem có dữ liệu không
                            var testText = htmlNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                            if (string.IsNullOrEmpty(testText)) return;
                        }
                        var oldImages = htmlNode.Descendants("img")
                                .Select(e => e.OuterHtml).ToList();
                        var oldAs = htmlNode.Descendants("a")
                                .Select(e => e.GetAttributeValue("href", null)).ToList();
                        try
                        {
                            if (htmlNode is HtmlAgilityPack.HtmlTextNode)
                            {
                                var htmlTextNode = (HtmlAgilityPack.HtmlTextNode)htmlNode;
                                htmlTextNode.Text = Module.SystemObjects.Tools.TranslateText(htmlTextNode.Text, destination, source)?.Replace("<hình", "<figure").Replace("</ hình>", "</figure>");
                            }
                            else
                            {
                                htmlNode.InnerHtml = Module.SystemObjects.Tools.TranslateText(htmlNode.InnerHtml, destination, source)?.Replace("<hình", "<figure").Replace("</ hình>", "</figure>");
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }


                        if (oldImages.Count() > 0)
                        {
                            var newImages = htmlNode.Descendants("img").ToList();
                            if (oldImages.Count() == newImages.Count())
                            {
                                for (int i = 0; i < newImages.Count(); i++)
                                {
                                    newImages[i].ParentNode.ReplaceChild(HtmlAgilityPack.HtmlTextNode.CreateNode(oldImages[i]), newImages[i]);
                                }
                            }
                            else
                            {
                                foreach (var oldImage in oldImages)
                                {
                                    if (string.IsNullOrEmpty(oldImage))
                                        continue;
                                    foreach (var newImage in newImages)
                                    {
                                        if (oldImage.Equals(newImage.OuterHtml, System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            newImage.ParentNode.ReplaceChild(HtmlAgilityPack.HtmlTextNode.CreateNode(oldImage), newImage);
                                        }
                                    }
                                }
                            }

                        }
                        if (oldAs.Count() > 0)
                        {
                            var newAs = htmlNode.Descendants("a").ToList();
                            if (oldAs.Count() == newAs.Count())
                            {
                                for (int i = 0; i < newAs.Count(); i++)
                                {
                                    newAs[i].SetAttributeValue("href", oldAs[i]);
                                }
                            }
                            else
                            {
                                foreach (var oldA in oldAs)
                                {
                                    if (string.IsNullOrEmpty(oldA))
                                        continue;
                                    foreach (var newA in newAs)
                                    {
                                        if (oldA.Equals(newA.GetAttributeValue("href", null), System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            newA.SetAttributeValue("href", oldA);
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// Thay thế các link gián tiếp trong HTML thành link tuyệt đối
        /// </summary>
        /// <param name="url">URL gốc</param>
        /// <param name="doc">Document HTML cần xử lý</param>
        public static void ReplaceIndirectLink(string url, HtmlAgilityPack.HtmlDocument doc)
        {
            var uri = new System.Uri(url);
            if (string.IsNullOrEmpty(uri.Host))
                return;
            string homePage = uri.Scheme + "://" + uri.Host;
            if (doc != null)
            {
                var aNodes = doc.DocumentNode.Descendants("a");
                if (aNodes.Count() > 0)
                {
                    foreach (var aNode in aNodes)
                    {
                        if (aNode.Attributes["href"] != null && !string.IsNullOrEmpty(aNode.Attributes["href"].Value))
                        {
                            if (aNode.Attributes["href"].Value.StartsWith("//"))
                                aNode.Attributes["href"].Value = uri.Scheme + ":" + aNode.Attributes["href"].Value;
                            else if (aNode.Attributes["href"].Value.StartsWith("/"))
                                aNode.Attributes["href"].Value = homePage + aNode.Attributes["href"].Value;
                            else if (aNode.Attributes["href"].Value.StartsWith(".."))
                                aNode.Attributes["href"].Value = homePage + aNode.Attributes["href"].Value.Substring(2);
                        }
                    }
                }
                var imgNodes = doc.DocumentNode.Descendants("img");
                if (imgNodes.Count() > 0)
                {
                    foreach (var imgNode in imgNodes)
                    {
                        if (imgNode.Attributes["src"] != null && !string.IsNullOrEmpty(imgNode.Attributes["src"].Value))
                        {
                            if (imgNode.Attributes["src"].Value.StartsWith("//"))
                                imgNode.Attributes["src"].Value = uri.Scheme + ":" + imgNode.Attributes["src"].Value;
                            else if (imgNode.Attributes["src"].Value.StartsWith("/"))
                                imgNode.Attributes["src"].Value = homePage + imgNode.Attributes["src"].Value;
                            else if (imgNode.Attributes["src"].Value.StartsWith(".."))
                                imgNode.Attributes["src"].Value = homePage + imgNode.Attributes["src"].Value.Substring(2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lấy URL trang chủ từ URL đầy đủ
        /// </summary>
        /// <param name="fullUrl">URL đầy đủ</param>
        /// <returns>URL trang chủ hoặc null nếu không hợp lệ</returns>
        public static string? GetHomepageUrl(string? fullUrl)
        {
            if (string.IsNullOrWhiteSpace(fullUrl))
            {
                return null; // Trả về null nếu chuỗi rỗng hoặc chỉ chứa khoảng trắng
            }

            try
            {
                string startUrl = "https://";
                if (!fullUrl.Contains("://"))
                {
                    // Mặc định là http nếu không có scheme nào được cung cấp
                    // Bạn có thể muốn xử lý logic này khác đi nếu cần
                    fullUrl = startUrl + fullUrl;
                    //Nếu không có scheme, hàm Uri có thể gặp lỗi hoặc hiểu sai
                }
                Uri uri = new Uri(fullUrl);

                // Uri.GetLeftPart(UriPartial.Authority) trả về phần Scheme + Host + Port (nếu port khác mặc định)
                // Đây chính xác là định nghĩa thông thường của "trang chủ"
                var result = uri.GetLeftPart(UriPartial.Authority);
                if (!fullUrl.Contains("://") && result.StartsWith(startUrl))
                {
                    result = result.Substring(startUrl.Length);
                }
                return result;

                // Các cách khác (ít phổ biến hơn cho "trang chủ"):
                // return $"{uri.Scheme}://{uri.Host}"; // Chỉ lấy Scheme và Host, bỏ qua port
                // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped); // Tương tự GetLeftPart(UriPartial.Authority)
            }
            catch (Exception ex)
            {
                // Xử lý trường hợp chuỗi không phải là một URL hợp lệ
                //Console.WriteLine($"Lỗi phân tích URL: '{fullUrl}'. Chi tiết: {ex.Message}");                
            }
            return fullUrl;
        }

        public static void ReplaceDirectLink(string url, HtmlAgilityPack.HtmlDocument doc)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Host))
            {
                return;
            }

            string text = uri.Scheme + "://" + uri.Host;
            if (doc == null)
            {
                return;
            }

            var aNodes = doc.DocumentNode.Descendants("a");
            if (aNodes.Count() > 0)
            {
                foreach (var item in aNodes)
                {
                    if (item.Attributes["href"] != null && !string.IsNullOrEmpty(item.Attributes["href"].Value) && item.Attributes["href"].Value.StartsWith(text))
                    {
                        item.Attributes["href"].Value = item.Attributes["href"].Value.Substring(text.Length);
                    }
                }
            }

            var imgNodes = doc.DocumentNode.Descendants("img");
            if (imgNodes.Count() <= 0)
            {
                return;
            }

            foreach (var item2 in imgNodes)
            {
                if (item2.Attributes["src"] != null && !string.IsNullOrEmpty(item2.Attributes["src"].Value) && item2.Attributes["src"].Value.StartsWith(text))
                {
                    item2.Attributes["src"].Value = item2.Attributes["src"].Value.Substring(text.Length);
                }
            }
        }

        public static string TranslateContext(string text, string word, bool upper = true, string refenceText = null)
        {
            //2023-07-03: từ cần dịch thì viết hoa, các từ khác viết thường
            if (string.IsNullOrEmpty(refenceText))
                refenceText = TranslateText(text);
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(refenceText) || string.IsNullOrEmpty(word))
                return null;
            string newContent = "";
            var firstIndex = 0;
            var content = upper ? text.ToLower() : text.ToUpper();
            var index = content.IndexOf(word, System.StringComparison.OrdinalIgnoreCase);
            while (index >= 0)
            {
                newContent += content.Substring(firstIndex, index - firstIndex);
                firstIndex = index + word.Length;

                //var afterCharIndex = firstIndex + word.Length;                                
                bool validate = true;
                if (firstIndex < content.Length - 1 && !char.IsWhiteSpace(content[firstIndex])
                && char.IsLetterOrDigit(content[firstIndex]) && !content.Substring(firstIndex).StartsWith(", "))
                {
                    //var charText = text[firstIndex];
                    validate = false;
                }
                else if (!string.IsNullOrEmpty(newContent) && char.IsLetterOrDigit(newContent[newContent.Length - 1]))
                {
                    //var charText = text[firstIndex];
                    validate = false;
                }
                if (validate)
                {
                    newContent += content.Substring(index, word.Length).ToUpper();
                }
                else
                {
                    newContent += content.Substring(index, word.Length);
                }
                if (firstIndex >= content.Length)
                    break;
                index = content.IndexOf(word, firstIndex, System.StringComparison.OrdinalIgnoreCase);
            }
            newContent += content.Substring(firstIndex);
            var newtranlateContent = Module.SystemObjects.Tools.TranslateText(newContent);
            if (string.IsNullOrEmpty(newtranlateContent))
                return null;
            int startIndex = -1;
            int endIndex = -1;
            for (int i = 0; i < newtranlateContent.Length; i++)
            {
                if (startIndex < 0 && char.IsUpper(newtranlateContent[i]))
                {
                    startIndex = i;
                }
                if (startIndex >= 0 && !char.IsUpper(newtranlateContent[i]) && newtranlateContent[i] != ' ')
                {
                    if (i == startIndex + 1)
                    {
                        //Trường hợp google tự sửa viết hoa đầu dòng
                        startIndex = -1;
                        continue;
                    }
                    var endText = newtranlateContent.Substring(0, i);
                    endIndex = i;
                    break;
                }
            }
            if (startIndex >= 0 || endIndex > 0)
            {
                //Nếu tìm thấy từ viết hoa
                if (startIndex < 0)
                    startIndex = 0;
                if (endIndex < 0)
                    endIndex = newtranlateContent.Length;
                string newTranlate = newtranlateContent.Substring(startIndex, endIndex - startIndex);
                int newStartIndex = refenceText.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                if (newStartIndex >= 0)
                {
                    newTranlate = refenceText.Substring(newStartIndex, newTranlate.Length);
                }
                return newTranlate.Trim();
            }
            return null;
        }


        public static void ZipFileExtractToDirectory(string fileName, string folder, bool overrideFiles = true)
        {
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            string newFileName = folder + "Copy";
            try
            {
                //Kiểm tra xem file hiện tại có đang được mở không
                using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("because it is being used by another process."))
                {

                    System.IO.File.Copy(fileName, newFileName, true);
                    fileName = newFileName;
                }
            }
            using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(fileName))
            {
                //Loops through each file in the zip file
                archive.ExtractToDirectory(folder, overrideFiles);
            }
            try
            {
                if (System.IO.File.Exists(newFileName))
                    System.IO.File.Delete(newFileName);
            }
            catch (Exception ex)
            {
            }
        }
        public static void ShowOrCloseDefaultWaitForm(string caption, string description = null, System.TimeSpan? currentTimeSpan = null, bool defaultSplashScreenManager = false)
        {
            try
            {
                var type = GetSplashScreenManager();
                if (type != null)
                {
                    if (string.IsNullOrEmpty(caption) && string.IsNullOrEmpty(description))
                    {

                        var methodCloseForm = type.GetMethod("CloseForm", BindingFlags.Public | BindingFlags.Static, new Type[] { });
                        if (methodCloseForm != null)
                            methodCloseForm.Invoke(null, null);
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(description) && currentTimeSpan != null)
                            description += " - ";
                        if (currentTimeSpan != null)
                        {
                            description += String.Format("{0:00}:{1:00}:{2:00}",
                                    currentTimeSpan.Value.Hours, currentTimeSpan.Value.Minutes, currentTimeSpan.Value.Seconds);
                        }
                        if (defaultSplashScreenManager)
                        {
                            var property = type.GetProperty("Default");
                            if (property != null)
                            {
                                var propertyValue = property.GetValue(null);
                                if (propertyValue is null)
                                {
                                    ShowOrCloseWaitFormWithCancelButton();
                                    propertyValue = property.GetValue(null);
                                }
                                if (propertyValue != null)
                                {
                                    if (!string.IsNullOrEmpty(caption))
                                    {
                                        var method = property.PropertyType.GetMethod("SetWaitFormCaption");
                                        if (method != null)
                                            method.Invoke(propertyValue, new object[] { caption });
                                    }
                                    else if (!string.IsNullOrEmpty(description))
                                    {
                                        var method = property.PropertyType.GetMethod("SetWaitFormDescription");
                                        if (method != null)
                                            method.Invoke(propertyValue, new object[] { description });
                                    }
                                }
                                else
                                {
                                    ShowOrCloseDefaultWaitForm(caption, description, currentTimeSpan, false);
                                }
                            }
                            else
                            {
                                //Form đã bị đóng
                                var method = type.GetMethod("ShowDefaultWaitForm", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string), typeof(string) });
                                if (method != null)
                                    method.Invoke(null, new object[] { caption, description });
                            }

                        }
                        else
                        {
                            var method = type.GetMethod("ShowDefaultWaitForm", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string), typeof(string) });
                            if (method != null)
                                method.Invoke(null, new object[] { caption, description });
                        }

                    }
                }
            }
            catch (System.Exception) { }

        }

        public static void ShowOrCloseWaitFormWithCancelButton()
        {
            try
            {
                var type = GetSplashScreenManager();
                if (type is null)
                    return;
                var formType = ReflectionHelper.FindType("System.Windows.Forms.Form");
                if (formType is null)
                    return;
                var method = type.GetMethod("ShowForm", BindingFlags.Public | BindingFlags.Static, new Type[] { formType, typeof(Type), typeof(bool), typeof(bool), typeof(bool) });
                if (method is null)
                    return;
                var waitType = ReflectionHelper.FindType("WaitFormWithCancelButton");
                if (waitType is null)
                    return;
                method.Invoke(null, new object[] { null, waitType, true, true, false });
            }
            catch (System.Exception) { }

        }
        private static PropertyInfo defaultSplashScreenManagerProperty = null;
        private static Type defaultSplashScreenManagerType = null;
        public static object DefaultSplashScreenManager
        {
            get
            {
                if (defaultSplashScreenManagerProperty is null)
                {
                    if (defaultSplashScreenManagerType is null)
                        defaultSplashScreenManagerType = GetSplashScreenManager();
                    if (defaultSplashScreenManagerType != null)
                    {
                        defaultSplashScreenManagerProperty = defaultSplashScreenManagerType.GetProperty("Default");
                    }
                }
                if (defaultSplashScreenManagerProperty != null)
                    return defaultSplashScreenManagerProperty.GetValue(null);
                return null;
            }
        }

        private static Type GetSplashScreenManager()
        {
            string typeName = "DevExpress.XtraSplashScreen.SplashScreenManager";
            //var objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo("DevExpress.XtraSplashScreen.SplashScreenManager");
            return ReflectionHelper.FindType(typeName);
        }


        public static DevExpress.ExpressApp.SystemModule.DialogController CreateDialogControllerDetailView(DevExpress.ExpressApp.Controller controller, DevExpress.ExpressApp.SystemModule.DialogController dc, object currentObject, IObjectSpace objectSpace, bool saveOnAccept = true, ShowViewParameters showViewParameters = null)
        {
            if (dc is null)
                dc = controller.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = saveOnAccept;
            if (showViewParameters is null)
            {
                showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate
                };
            }
            showViewParameters.Controllers.Add(dc);
            showViewParameters.CreatedView = controller.Application.CreateDetailView(objectSpace, currentObject, saveOnAccept);
            showViewParameters.Context = TemplateContext.View;
            controller.Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(controller.Frame, dc.AcceptAction));
            return dc;
        }

        public static DevExpress.ExpressApp.SystemModule.DialogController PopupDialogControllerListView(DevExpress.ExpressApp.Controller controller, DevExpress.ExpressApp.SystemModule.DialogController dc, Type objectType, IObjectSpace objectSpace, string criteriaName = null, DevExpress.Data.Filtering.CriteriaOperator criteriaOperator = null, bool saveOnAccept = true, ShowViewParameters showViewParameters = null, bool showFind = true, bool lookupView = false, DevExpress.ExpressApp.CollectionSourceDataAccessMode? collectionSourceDataAccessMode = null)
        {
            if (dc is null)
                dc = controller.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = saveOnAccept;
            if (showViewParameters is null)
            {
                showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate,
                    Context = TemplateContext.LookupWindow,
                    //Context = TemplateContext.View
                };
            }
            if (showFind)
            {
                dc.WindowTemplateChanged += delegate (object o, EventArgs args)
                {
                    if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                        ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                    {
                        ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                    }
                };
            }

            showViewParameters.Controllers.Add(dc);
            if (showViewParameters.CreatedView is null)
            {
                string viewId = !lookupView ? controller.Application.FindListViewId(objectType) : controller.Application.FindLookupListViewId(objectType);
                if (!string.IsNullOrEmpty(viewId))
                {
                    var modelListView = controller.Application.FindModelView(viewId) as DevExpress.ExpressApp.Model.IModelListView;
                    if (modelListView != null)
                    {
                        if (collectionSourceDataAccessMode is null)
                            collectionSourceDataAccessMode = modelListView.DataAccessMode;
                        //Fix lỗi TreeListEditor không hỗ trợ chế độ server
                        if (collectionSourceDataAccessMode.Value == DevExpress.ExpressApp.CollectionSourceDataAccessMode.Server &&
                            modelListView.EditorType != null && modelListView.EditorType.Name == "TreeListEditor")
                        {
                            collectionSourceDataAccessMode = DevExpress.ExpressApp.CollectionSourceDataAccessMode.Client;
                        }
                        CollectionSourceBase collectionSource = controller.Application.CreateCollectionSource(objectSpace,
                            objectType, viewId, collectionSourceDataAccessMode.Value, CollectionSourceMode.Normal);
                        if (!string.IsNullOrEmpty(criteriaName) && !(criteriaOperator is null))
                        {
                            collectionSource.BeginUpdateCriteria();
                            collectionSource.Criteria[criteriaName] = criteriaOperator;
                            collectionSource.EndUpdateCriteria();
                        }
                        var listView = controller.Application.CreateListView(viewId, collectionSource, saveOnAccept);
                        showViewParameters.CreatedView = listView;

                        //dc.SaveOnAccept = false;
                    }
                }
            }

            controller.Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(controller.Frame, dc.AcceptAction));
            return dc;
        }

        //Tạo ma trận join từng giá trị với nhau theo cột và hàng
        //Cross Join List, Array
        //Matrix Join List, Array
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> GenerateAllPermutations<T>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> listOfList)
        {
            var results = new System.Collections.Generic.List<System.Collections.Generic.List<T>>();

            ForEachPermutationDo(listOfList, (permutation) =>
            {
                results.Add((System.Collections.Generic.List<T>)permutation);
                return true;
            });

            return results;
        }

        static void ForEachPermutationDo<T>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> listOfList, System.Func<System.Collections.Generic.IEnumerable<T>, bool> whatToDo)
        {
            var numCols = listOfList.Count();
            var numRows = listOfList.Aggregate(1, (a, b) => a * b.Count());
            var continueGenerating = true;

            var permutation = new System.Collections.Generic.List<T>();
            for (var r = 0; r < numRows; r++)
            {
                var repeatFactor = 1;
                for (var c = 0; c < numCols; c++)
                {
                    var aList = listOfList.ElementAt(c);
                    permutation.Add(aList.ElementAt((r / repeatFactor) % aList.Count()));
                    repeatFactor *= aList.Count();
                }

                continueGenerating = whatToDo(permutation.ToList()); // send duplicate
                if (!continueGenerating) break;

                permutation.Clear();
            }

        }

        public static string FindBestMatch(string stringToCompare, System.Collections.Generic.IEnumerable<string> strs)
        {
            System.Collections.Generic.HashSet<string> strCompareHash = stringToCompare.Split(' ').ToHashSet();

            int maxIntersectCount = 0;
            string bestMatch = string.Empty;

            foreach (string str in strs)
            {
                System.Collections.Generic.HashSet<string> strHash = str.Split(' ').ToHashSet();
                int intersectCount = strCompareHash.Intersect(strHash).Count();
                if (intersectCount > maxIntersectCount)
                {
                    maxIntersectCount = intersectCount;
                    bestMatch = str;
                }
            }
            return bestMatch;
        }




        //Thuật toán chống ghi đè file
        /// <summary>
        /// Tạo tên file duy nhất để tránh ghi đè file hiện có.
        /// Thêm số thứ tự vào tên file nếu file đã tồn tại.
        /// </summary>
        /// <param name="fileName">Tên file gốc</param>
        /// <returns>Tên file duy nhất</returns>
        /// <example>
        /// string uniqueName = Tools.GetUniqueFileName("document.txt");
        /// // Nếu document.txt đã tồn tại, kết quả: "document (1).txt"
        /// // Nếu document (1).txt cũng tồn tại, kết quả: "document (2).txt"
        /// </example>
        public static string GetUniqueFileName(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return fileName;
            string path = System.IO.Path.GetDirectoryName(fileName);
            string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);
            int i = 1;
            while (System.IO.File.Exists(fileName))
            {
                fileName = System.IO.Path.Combine(path, name + " (" + i + ")" + extension);
                i++;
            }
            return fileName;
        }

        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            if (!System.IO.Directory.Exists(targetPath))
                System.IO.Directory.CreateDirectory(targetPath);
            foreach (string dirPath in System.IO.Directory.GetDirectories(sourcePath, "*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public static void ClipboardSetText(string text)
        {
            try
            {
                string typeName = "System.Windows.Forms.Clipboard";
                var type = ReflectionHelper.FindType(typeName);
                if (type != null)
                {
                    var method = type.GetMethod("SetText", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string) });
                    if (method != null)
                        method.Invoke(null, new object[] { text });
                }
            }
            catch (System.Exception) { }

        }

        public static string ClipboardGetText()
        {
            try
            {
                string typeName = "System.Windows.Forms.Clipboard";
                var type = ReflectionHelper.FindType(typeName);
                if (type != null)
                {
                    var method = type.GetMethod("GetText", BindingFlags.Public | BindingFlags.Static);
                    if (method != null)
                        return method.Invoke(null, null) as string;
                }
            }
            catch (System.Exception) { }
            return null;
        }

    }
}