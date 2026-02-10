using System.Drawing;
namespace ENTOS.Module.Interfaces
{
/// <summary>
/// Interface xử lý hình ảnh: chuyển đổi, kiểm tra SVG, resize.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Chuyển Image sang Bitmap.
    /// </summary>
    Bitmap ImageToBitmap(Image image);
    /// <summary>
    /// Chuyển Bitmap sang Image.
    /// </summary>
    Image BitmapToImage(Bitmap bitmap);
    /// <summary>
    /// Kiểm tra file có phải SVG.
    /// </summary>
    bool IsSvg(string filePath);
    /// <summary>
    /// Thay đổi kích thước ảnh.
    /// </summary>
    Image Resize(Image image, int width, int height);
}
} 