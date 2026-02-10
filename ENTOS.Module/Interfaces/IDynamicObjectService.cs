namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface thao tác đối tượng động: lấy/đặt giá trị thuộc tính, lấy mô tả thuộc tính.
    /// </summary>
    public interface IDynamicObjectService
    {
        /// <summary>
        /// Đặt giá trị cho thuộc tính.
        /// </summary>
        void SetPropertyValue(object obj, string propertyName, object value);
        /// <summary>
        /// Lấy giá trị thuộc tính.
        /// </summary>
        object GetPropertyValue(object obj, string propertyName);
        /// <summary>
        /// Lấy mô tả thuộc tính (caption).
        /// </summary>
        string GetCaption(object obj, string propertyName);
    }
} 