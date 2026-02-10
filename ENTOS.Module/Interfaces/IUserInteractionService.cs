namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Service cung cấp các phương thức tương tác với người dùng như xác nhận, nhập liệu, chọn file/thư mục, hoặc hiển thị dialog nhiều bước.
    /// </summary>
    public interface IUserInteractionService
    {
        /// <summary>
        /// Hiển thị popup xác nhận với người dùng (ví dụ: Yes/No, OK/Cancel).
        /// </summary>
        /// <param name="message">Nội dung thông báo xác nhận.</param>
        /// <param name="caption">Tiêu đề popup.</param>
        /// <returns>Trả về true nếu người dùng xác nhận, false nếu từ chối.</returns>
        bool ShowConfirmation(string message, string caption = "Xác nhận");

        /// <summary>
        /// Hiển thị popup với các lựa chọn Yes (true), No (false), Cancel (null).
        /// </summary>
        /// <returns></returns>
        bool? ShowYesNoCancel(string message, string caption = "Xác nhận");
        /// <summary>
        /// Hiển thị popup cho phép người dùng nhập dữ liệu (input).
        /// </summary>
        /// <param name="message">Nội dung hướng dẫn nhập liệu.</param>
        /// <param name="caption">Tiêu đề popup.</param>
        /// <param name="defaultValue">Giá trị mặc định (nếu có).</param>
        /// <returns>Trả về chuỗi người dùng nhập, hoặc null nếu hủy.</returns>
        string? ShowInputDialog(string message, string caption = "Nhập liệu", string? defaultValue = null);

        /// <summary>
        /// Hiển thị UI modal/dialog nhiều bước (wizard) hoặc tương tác phức tạp.
        /// </summary>
        /// <param name="wizardSteps">Danh sách các bước wizard, mỗi bước là một đối tượng mô tả UI và logic.</param>
        /// <param name="caption">Tiêu đề dialog.</param>
        /// <returns>Trả về kết quả cuối cùng của wizard, hoặc null nếu người dùng hủy.</returns>
        object? ShowWizardDialog(IEnumerable<IWizardStep> wizardSteps, string caption = "Hướng dẫn nhiều bước");

        /// <summary>
        /// Hiển thị dialog cho phép người dùng chọn một thư mục (mở hoặc upload thư mục).
        /// </summary>
        /// <param name="description">Mô tả/hướng dẫn cho người dùng.</param>
        /// <returns>Trả về đường dẫn thư mục được chọn, hoặc null nếu hủy.</returns>
        string? SelectFolder(string description = "Chọn thư mục");

        /// <summary>
        /// Hiển thị dialog cho phép người dùng chọn một hoặc nhiều file (mở hoặc upload file).
        /// </summary>
        /// <param name="filter">Bộ lọc định dạng file, ví dụ: "Excel Files|*.xlsx;*.xls|All Files|*.*".</param>
        /// <param name="multiSelect">Cho phép chọn nhiều file hay không.</param>
        /// <param name="description">Mô tả/hướng dẫn cho người dùng.</param>
        /// <returns>Trả về danh sách đường dẫn file được chọn, hoặc rỗng/null nếu hủy.</returns>
        IEnumerable<string>? SelectFiles(string filter = "All Files|*.*", bool multiSelect = false, string description = "Chọn file");

   
        T? SelectFromList<T>(IEnumerable<T> items, string caption = "Chọn giá trị");

        /// <summary>
        /// Hiển thị dialog cho phép người dùng chọn nhiều giá trị từ danh sách.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của giá trị.</typeparam>
        /// <param name="items">Danh sách lựa chọn.</param>
        /// <param name="caption">Tiêu đề dialog.</param>
        /// <returns>Trả về danh sách giá trị được chọn, hoặc rỗng nếu hủy.</returns>
        IEnumerable<T> MultiSelectFromList<T>(IEnumerable<T> items, string caption = "Chọn nhiều giá trị");
    }

    /// <summary>
    /// Interface mô tả một bước trong wizard/multi-step dialog.
    /// </summary>
    public interface IWizardStep
    {
        /// <summary>
        /// Tiêu đề bước.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Render UI cho bước này (có thể là WinForms Control, hoặc ViewModel tuỳ nền tảng).
        /// </summary>
        object GetStepContent();

        /// <summary>
        /// Xử lý logic khi người dùng nhấn Next/Finish ở bước này.
        /// </summary>
        /// <returns>Trả về true nếu hợp lệ, false nếu cần nhập lại.</returns>
        bool Validate();
    }

} 