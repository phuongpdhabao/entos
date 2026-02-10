/// <summary>
/// Namespace chứa các marker interface để xác định vòng đời của các service khi đăng ký Dependency Injection.
/// </summary>
namespace ENTOS.SharedKernel.Interfaces
{
    /// <summary>
    /// Marker interface cho các service có vòng đời Transient.
    /// Mỗi lần được yêu cầu sẽ tạo một instance mới.
    /// </summary>
    public interface ITransientDependency { }

    /// <summary>
    /// Marker interface cho các service có vòng đời Scoped.
    /// Một instance được dùng trong suốt một scope (ví dụ: 1 HTTP request).
    /// </summary>
    public interface IScopedDependency { }

    /// <summary>
    /// Marker interface cho các service có vòng đời Singleton.
    /// Một instance duy nhất sẽ được dùng trong suốt vòng đời của ứng dụng.
    /// </summary>
    public interface ISingletonDependency { }
}
