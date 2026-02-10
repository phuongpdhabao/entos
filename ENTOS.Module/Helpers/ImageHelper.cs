using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ENTOS.Module.SystemObjects;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý hình ảnh cơ bản.
    /// </summary>
    public static partial class ImageHelper
    {
        /// <summary>
        /// Kiểm tra file có phải SVG không.
        /// </summary>
        public static bool IsSvg(string filePath)
        {
            return Path.GetExtension(filePath)?.ToLower() == ".svg";
        }

        /// <summary>
        /// Đổi kích thước ảnh Bitmap.
        /// </summary>
        //public static Bitmap Resize(Bitmap image, int width, int height)
        //{
        //    return new Bitmap(image, new Size(width, height));
        //}
        public static System.Drawing.Bitmap Resize(System.Drawing.Bitmap bmp, int newWidth, int height)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(newWidth, newWidth);
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, 0, 0, newWidth, newWidth);
            }
            return image;
        }
        /// <summary>
        /// Kiểm tra dữ liệu có phải SVG không (từ byte array).
        /// </summary>
        public static bool IsSvg(byte[] data)
        {
            if (data == null || data.Length < 5) return false;
            var text = System.Text.Encoding.UTF8.GetString(data, 0, data.Length > 1024 ? 1024 : data.Length);
            return text.TrimStart().StartsWith("<svg", System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Đổi kích thước ảnh từ byte[] (chỉ hỗ trợ định dạng bitmap thông dụng, không hỗ trợ SVG).
        /// </summary>
        public static byte[] Resize(byte[] imageData, int width, int height)
        {
            using var ms = new MemoryStream(imageData);
            using var original = new Bitmap(ms);
            using var resized = new Bitmap(original, new Size(width, height));
            using var outStream = new MemoryStream();
            resized.Save(outStream, original.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Kiểm tra dữ liệu có phải PNG không.
        /// </summary>
        public static bool IsPng(byte[] data)
        {
            if (data == null || data.Length < 8) return false;
            return data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47 && data[4] == 0x0D && data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A;
        }

        public static bool IsJpeg(byte[] data)
        {
            if (data == null || data.Length < 3) return false;
            return data[0] == 0xFF && data[1] == 0xD8 && data[data.Length - 2] == 0xFF && data[data.Length - 1] == 0xD9;
        }

        public static bool IsGif(byte[] data)
        {
            if (data == null || data.Length < 6) return false;
            var header = System.Text.Encoding.ASCII.GetString(data, 0, 6);
            return header == "GIF87a" || header == "GIF89a";
        }

        public static bool IsBmp(byte[] data)
        {
            if (data == null || data.Length < 2) return false;
            return data[0] == 0x42 && data[1] == 0x4D;
        }

        /// <summary>
        /// Chuyển đổi định dạng ảnh (PNG, JPEG, BMP, GIF, TIFF). Quality chỉ áp dụng cho JPEG.
        /// </summary>
        public static byte[] ConvertFormat(byte[] imageData, System.Drawing.Imaging.ImageFormat format, long quality = 90L)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            using var outStream = new MemoryStream();
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                var encoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
                var encParams = new System.Drawing.Imaging.EncoderParameters(1);
                encParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                image.Save(outStream, encoder, encParams);
            }
            else
            {
                image.Save(outStream, format);
            }
            return outStream.ToArray();
        }

        /// <summary>
        /// Crop ảnh theo vùng chỉ định.
        /// </summary>
        public static byte[] Crop(byte[] imageData, Rectangle cropArea)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            using var cropped = image.Clone(cropArea, image.PixelFormat);
            using var outStream = new MemoryStream();
            cropped.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Lấy kích thước ảnh (width, height).
        /// </summary>
        public static (int width, int height) GetImageSize(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            return (image.Width, image.Height);
        }

        /// <summary>
        /// Tạo thumbnail giữ tỉ lệ.
        /// </summary>
        public static byte[] CreateThumbnail(byte[] imageData, int maxWidth, int maxHeight, bool crop = false)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            int w = image.Width, h = image.Height;
            float ratio = Math.Min((float)maxWidth / w, (float)maxHeight / h);
            int tw = (int)(w * ratio), th = (int)(h * ratio);
            using var thumb = new Bitmap(image, new Size(tw, th));
            using var outStream = new MemoryStream();
            thumb.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Chuyển ảnh sang grayscale.
        /// </summary>
        public static byte[] ToGrayscale(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    var c = image.GetPixel(x, y);
                    int g = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    image.SetPixel(x, y, Color.FromArgb(c.A, g, g, g));
                }
            using var outStream = new MemoryStream();
            image.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Lấy thông tin EXIF cơ bản (nếu có).
        /// </summary>
        public static Dictionary<string, string> GetExifInfo(byte[] imageData)
        {
            var result = new Dictionary<string, string>();
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            foreach (var prop in image.PropertyItems)
            {
                result[$"0x{prop.Id:X4}"] = System.Text.Encoding.UTF8.GetString(prop.Value);
            }
            return result;
        }

        /// <summary>
        /// Thêm watermark text lên ảnh.
        /// </summary>
        public static byte[] AddTextWatermark(byte[] imageData, string text, Font font, Color color, Point position)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            using var g = Graphics.FromImage(image);
            using var brush = new SolidBrush(color);
            g.DrawString(text, font, brush, position);
            using var outStream = new MemoryStream();
            image.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Xoay ảnh theo góc bất kỳ.
        /// </summary>
        public static byte[] Rotate(byte[] imageData, float angle)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            using var rotated = new Bitmap(image.Width, image.Height);
            using (var g = Graphics.FromImage(rotated))
            {
                g.TranslateTransform(image.Width / 2, image.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2, -image.Height / 2);
                g.DrawImage(image, new Point(0, 0));
            }
            using var outStream = new MemoryStream();
            rotated.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Lật ảnh ngang hoặc dọc.
        /// </summary>
        public static byte[] Flip(byte[] imageData, bool horizontal)
        {
            using var ms = new MemoryStream(imageData);
            using var image = new Bitmap(ms);
            image.RotateFlip(horizontal ? RotateFlipType.RotateNoneFlipX : RotateFlipType.RotateNoneFlipY);
            using var outStream = new MemoryStream();
            image.Save(outStream, image.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Kiểm tra ảnh có hợp lệ không (không bị lỗi/corrupt).
        /// </summary>
        public static bool IsValidImage(byte[] imageData)
        {
            try
            {
                using var ms = new MemoryStream(imageData);
                using var image = new Bitmap(ms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tạo ảnh trống với màu nền.
        /// </summary>
        public static byte[] CreateBlankImage(int width, int height, Color bgColor)
        {
            using var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(bgColor);
            }
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        /// <summary>
        /// Kiểu căn chỉnh ảnh khi thêm padding.
        /// </summary>
        public enum ImageAlignment
        {
            Center,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        /// <summary>
        /// Resize ảnh theo danh sách kích thước, có thể thêm vùng trắng để không bị crop, hỗ trợ căn chỉnh.
        /// </summary>
        public static Dictionary<(int width, int height), byte[]> ResizeToPresets(
            byte[] imageData,
            List<(int width, int height)> sizes,
            bool addWhitePadding = false,
            ImageAlignment alignment = ImageAlignment.Center)
        {
            var result = new Dictionary<(int, int), byte[]>();
            using var ms = new MemoryStream(imageData);
            using var original = new Bitmap(ms);
            foreach (var (targetW, targetH) in sizes)
            {
                float ratio = Math.Min((float)targetW / original.Width, (float)targetH / original.Height);
                int newW = (int)(original.Width * ratio);
                int newH = (int)(original.Height * ratio);
                using var resized = new Bitmap(original, new Size(newW, newH));
                Bitmap finalBmp;
                if (addWhitePadding && (newW != targetW || newH != targetH))
                {
                    finalBmp = new Bitmap(targetW, targetH);
                    using (var g = Graphics.FromImage(finalBmp))
                    {
                        g.Clear(Color.White);
                        int x = alignment switch
                        {
                            ImageAlignment.Center => (targetW - newW) / 2,
                            ImageAlignment.Top => (targetW - newW) / 2,
                            ImageAlignment.Bottom => (targetW - newW) / 2,
                            ImageAlignment.Left => 0,
                            ImageAlignment.Right => targetW - newW,
                            ImageAlignment.TopLeft => 0,
                            ImageAlignment.TopRight => targetW - newW,
                            ImageAlignment.BottomLeft => 0,
                            ImageAlignment.BottomRight => targetW - newW,
                            _ => (targetW - newW) / 2
                        };
                        int y = alignment switch
                        {
                            ImageAlignment.Center => (targetH - newH) / 2,
                            ImageAlignment.Top => 0,
                            ImageAlignment.Bottom => targetH - newH,
                            ImageAlignment.Left => (targetH - newH) / 2,
                            ImageAlignment.Right => (targetH - newH) / 2,
                            ImageAlignment.TopLeft => 0,
                            ImageAlignment.TopRight => 0,
                            ImageAlignment.BottomLeft => targetH - newH,
                            ImageAlignment.BottomRight => targetH - newH,
                            _ => (targetH - newH) / 2
                        };
                        g.DrawImage(resized, x, y, newW, newH);
                    }
                }
                else
                {
                    finalBmp = new Bitmap(resized);
                }
                using var outStream = new MemoryStream();
                finalBmp.Save(outStream, original.RawFormat);
                result[(targetW, targetH)] = outStream.ToArray();
                finalBmp.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Crop loại bỏ vùng trắng quanh ảnh (tự động phát hiện biên trắng, trả về ảnh đã cắt).
        /// </summary>
        public static byte[] CropWhiteBorder(byte[] imageData, int threshold = 250)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            int left = bmp.Width, right = 0, top = bmp.Height, bottom = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.R < threshold || c.G < threshold || c.B < threshold)
                    {
                        if (x < left) left = x;
                        if (x > right) right = x;
                        if (y < top) top = y;
                        if (y > bottom) bottom = y;
                    }
                }
            }
            if (right <= left || bottom <= top)
                return imageData; // Không tìm thấy vùng cần crop
            var rect = new Rectangle(left, top, right - left + 1, bottom - top + 1);
            using var cropped = bmp.Clone(rect, bmp.PixelFormat);
            using var outStream = new MemoryStream();
            cropped.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Xóa nền ảnh theo màu nền chỉ định (mặc định là trắng), trả về PNG với nền trong suốt.
        /// </summary>
        /// <param name="imageData">Ảnh gốc</param>
        /// <param name="backgroundColor">Màu nền cần xóa (mặc định: trắng)</param>
        /// <param name="tolerance">Độ chênh lệch màu cho phép (0-255, mặc định: 10)</param>
        /// <returns>byte[] ảnh PNG với nền trong suốt</returns>
        public static byte[] RemoveBackground(byte[] imageData, Color? backgroundColor = null, int tolerance = 10)
        {
            var bg = backgroundColor ?? Color.White;
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (Math.Abs(c.R - bg.R) <= tolerance && Math.Abs(c.G - bg.G) <= tolerance && Math.Abs(c.B - bg.B) <= tolerance)
                        result.SetPixel(x, y, Color.FromArgb(0, c)); // Transparent
                    else
                        result.SetPixel(x, y, Color.FromArgb(255, c.R, c.G, c.B));
                }
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            return outStream.ToArray();
        }

        /// <summary>
        /// Tự động phát hiện màu nền xung quanh vật thể (lấy mẫu từ viền ảnh) và xóa nền, trả về PNG nền trong suốt.
        /// </summary>
        /// <param name="imageData">Ảnh gốc</param>
        /// <param name="tolerance">Độ chênh lệch màu cho phép (0-255, mặc định: 10)</param>
        /// <returns>byte[] ảnh PNG với nền trong suốt</returns>
        public static byte[] AutoRemoveBackground(byte[] imageData, int tolerance = 10)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            // Lấy mẫu màu nền từ các viền ảnh
            var borderColors = new List<Color>();
            for (int x = 0; x < bmp.Width; x++)
            {
                borderColors.Add(bmp.GetPixel(x, 0)); // Top
                borderColors.Add(bmp.GetPixel(x, bmp.Height - 1)); // Bottom
            }
            for (int y = 1; y < bmp.Height - 1; y++)
            {
                borderColors.Add(bmp.GetPixel(0, y)); // Left
                borderColors.Add(bmp.GetPixel(bmp.Width - 1, y)); // Right
            }
            // Tìm màu xuất hiện nhiều nhất (giả định là màu nền)
            var bg = borderColors
                .GroupBy(c => (c.R / 8, c.G / 8, c.B / 8)) // Nhóm theo block màu gần giống
                .OrderByDescending(g => g.Count())
                .First().First();
            // Xóa nền
            var result = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (Math.Abs(c.R - bg.R) <= tolerance && Math.Abs(c.G - bg.G) <= tolerance && Math.Abs(c.B - bg.B) <= tolerance)
                        result.SetPixel(x, y, Color.FromArgb(0, c)); // Transparent
                    else
                        result.SetPixel(x, y, Color.FromArgb(255, c.R, c.G, c.B));
                }
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            return outStream.ToArray();
        }

        /// <summary>
        /// Lấy màu chủ đạo (dominant color) của ảnh.
        /// </summary>
        public static Color GetDominantColor(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var colorCount = new Dictionary<int, int>();
            for (int y = 0; y < bmp.Height; y += 2)
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    var c = bmp.GetPixel(x, y);
                    int key = (c.R / 8 << 16) | (c.G / 8 << 8) | (c.B / 8);
                    if (!colorCount.ContainsKey(key)) colorCount[key] = 0;
                    colorCount[key]++;
                }
            var max = colorCount.OrderByDescending(kv => kv.Value).First().Key;
            return Color.FromArgb(((max >> 16) & 0xFF) * 8, ((max >> 8) & 0xFF) * 8, (max & 0xFF) * 8);
        }

        /// <summary>
        /// Lấy bảng màu phổ biến nhất trong ảnh (palette).
        /// </summary>
        public static List<Color> GetPalette(byte[] imageData, int colorCount = 5)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var dict = new Dictionary<int, int>();
            for (int y = 0; y < bmp.Height; y += 2)
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    var c = bmp.GetPixel(x, y);
                    int key = (c.R / 16 << 8) | (c.G / 16 << 4) | (c.B / 16);
                    if (!dict.ContainsKey(key)) dict[key] = 0;
                    dict[key]++;
                }
            return dict.OrderByDescending(kv => kv.Value).Take(colorCount)
                .Select(kv => Color.FromArgb(((kv.Key >> 8) & 0xF) * 16, ((kv.Key >> 4) & 0xF) * 16, (kv.Key & 0xF) * 16)).ToList();
        }

        /// <summary>
        /// Làm mờ ảnh (Box blur đơn giản).
        /// </summary>
        public static byte[] Blur(byte[] imageData, int radius = 2)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    int r = 0, g = 0, b = 0, count = 0;
                    for (int dy = -radius; dy <= radius; dy++)
                        for (int dx = -radius; dx <= radius; dx++)
                        {
                            int nx = x + dx, ny = y + dy;
                            if (nx >= 0 && nx < bmp.Width && ny >= 0 && ny < bmp.Height)
                            {
                                var c = bmp.GetPixel(nx, ny);
                                r += c.R; g += c.G; b += c.B; count++;
                            }
                        }
                    result.SetPixel(x, y, Color.FromArgb(r / count, g / count, b / count));
                }
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Làm sắc nét ảnh (Sharpen đơn giản).
        /// </summary>
        public static byte[] Sharpen(byte[] imageData)
        {
            int[,] kernel = { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 1; y < bmp.Height - 1; y++)
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    int r = 0, g = 0, b = 0;
                    for (int ky = -1; ky <= 1; ky++)
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            var c = bmp.GetPixel(x + kx, y + ky);
                            int k = kernel[ky + 1, kx + 1];
                            r += c.R * k; g += c.G * k; b += c.B * k;
                        }
                    r = Math.Min(255, Math.Max(0, r));
                    g = Math.Min(255, Math.Max(0, g));
                    b = Math.Min(255, Math.Max(0, b));
                    result.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            using var outStream = new MemoryStream();
            result.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Điều chỉnh độ sáng.
        /// </summary>
        public static byte[] AdjustBrightness(byte[] imageData, int delta)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    int r = Math.Min(255, Math.Max(0, c.R + delta));
                    int g = Math.Min(255, Math.Max(0, c.G + delta));
                    int b = Math.Min(255, Math.Max(0, c.B + delta));
                    bmp.SetPixel(x, y, Color.FromArgb(c.A, r, g, b));
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Điều chỉnh tương phản.
        /// </summary>
        public static byte[] AdjustContrast(byte[] imageData, float contrast)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            float c = (100.0f + contrast) / 100.0f;
            c *= c;
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var px = bmp.GetPixel(x, y);
                    float r = px.R / 255.0f;
                    float g = px.G / 255.0f;
                    float b = px.B / 255.0f;
                    r = (((r - 0.5f) * c) + 0.5f) * 255.0f;
                    g = (((g - 0.5f) * c) + 0.5f) * 255.0f;
                    b = (((b - 0.5f) * c) + 0.5f) * 255.0f;
                    bmp.SetPixel(x, y, Color.FromArgb(px.A, (int)Math.Min(255, Math.Max(0, r)), (int)Math.Min(255, Math.Max(0, g)), (int)Math.Min(255, Math.Max(0, b))));
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Điều chỉnh gamma.
        /// </summary>
        public static byte[] AdjustGamma(byte[] imageData, float gamma)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i)
                gammaArray[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / gamma)) + 0.5));
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    bmp.SetPixel(x, y, Color.FromArgb(c.A, gammaArray[c.R], gammaArray[c.G], gammaArray[c.B]));
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Áp dụng bộ lọc màu Sepia.
        /// </summary>
        public static byte[] ApplySepia(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    int tr = (int)(0.393 * c.R + 0.769 * c.G + 0.189 * c.B);
                    int tg = (int)(0.349 * c.R + 0.686 * c.G + 0.168 * c.B);
                    int tb = (int)(0.272 * c.R + 0.534 * c.G + 0.131 * c.B);
                    bmp.SetPixel(x, y, Color.FromArgb(c.A, Math.Min(255, tr), Math.Min(255, tg), Math.Min(255, tb)));
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Áp dụng bộ lọc màu Negative.
        /// </summary>
        public static byte[] ApplyNegative(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    bmp.SetPixel(x, y, Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B));
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Vẽ hình chữ nhật lên ảnh.
        /// </summary>
        public static byte[] DrawRectangle(byte[] imageData, Rectangle rect, Color color, int thickness = 2)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            using var g = Graphics.FromImage(bmp);
            using var pen = new Pen(color, thickness);
            g.DrawRectangle(pen, rect);
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Vẽ đường thẳng lên ảnh.
        /// </summary>
        public static byte[] DrawLine(byte[] imageData, Point p1, Point p2, Color color, int thickness = 2)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            using var g = Graphics.FromImage(bmp);
            using var pen = new Pen(color, thickness);
            g.DrawLine(pen, p1, p2);
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Overlay ảnh nhỏ lên ảnh lớn tại vị trí chỉ định.
        /// </summary>
        public static byte[] OverlayImage(byte[] baseImage, byte[] overlayImage, Point position, float opacity = 1f)
        {
            using var msBase = new MemoryStream(baseImage);
            using var bmpBase = new Bitmap(msBase);
            using var msOverlay = new MemoryStream(overlayImage);
            using var bmpOverlay = new Bitmap(msOverlay);
            using var g = Graphics.FromImage(bmpBase);
            var cm = new System.Drawing.Imaging.ColorMatrix { Matrix33 = opacity };
            var ia = new System.Drawing.Imaging.ImageAttributes();
            ia.SetColorMatrix(cm, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
            g.DrawImage(bmpOverlay, new Rectangle(position, bmpOverlay.Size), 0, 0, bmpOverlay.Width, bmpOverlay.Height, GraphicsUnit.Pixel, ia);
            using var outStream = new MemoryStream();
            bmpBase.Save(outStream, bmpBase.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Crop ảnh thành hình tròn.
        /// </summary>
        public static byte[] CropCircle(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            int size = Math.Min(bmp.Width, bmp.Height);
            var result = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(result))
            {
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, size, size);
                g.SetClip(path);
                g.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle((bmp.Width - size) / 2, (bmp.Height - size) / 2, size, size), GraphicsUnit.Pixel);
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            return outStream.ToArray();
        }

        /// <summary>
        /// Crop bo góc mềm.
        /// </summary>
        public static byte[] CropRoundedCorners(byte[] imageData, int radius)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(result))
            {
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(bmp.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(bmp.Width - radius, bmp.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, bmp.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();
                g.SetClip(path);
                g.Clear(Color.Transparent);
                g.DrawImage(bmp, 0, 0);
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            return outStream.ToArray();
        }

        /// <summary>
        /// Tạo ảnh từ text.
        /// </summary>
        public static byte[] CreateImageFromText(string text, Font font, Color textColor, Color bgColor, int width, int height)
        {
            using var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(bgColor);
                using var brush = new SolidBrush(textColor);
                g.DrawString(text, font, brush, new PointF(0, 0));
            }
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        /// <summary>
        /// Chuyển ảnh sang trắng đen (thresholding).
        /// </summary>
        public static byte[] ToBinary(byte[] imageData, int threshold = 128)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    int gray = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bmp.SetPixel(x, y, gray < threshold ? Color.Black : Color.White);
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Làm mờ pixel (pixelate).
        /// </summary>
        public static byte[] Pixelate(byte[] imageData, int pixelSize = 8)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            for (int y = 0; y < bmp.Height; y += pixelSize)
                for (int x = 0; x < bmp.Width; x += pixelSize)
                {
                    int w = Math.Min(pixelSize, bmp.Width - x);
                    int h = Math.Min(pixelSize, bmp.Height - y);
                    Color avg = bmp.GetPixel(x, y);
                    for (int dy = 0; dy < h; dy++)
                        for (int dx = 0; dx < w; dx++)
                            bmp.SetPixel(x + dx, y + dy, avg);
                }
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Resize chất lượng cao (bicubic).
        /// </summary>
        public static byte[] HighQualityResize(byte[] imageData, int width, int height)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(width, height);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, width, height);
            }
            using var outStream = new MemoryStream();
            result.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Phát hiện biên (Sobel edge detection đơn giản).
        /// </summary>
        public static byte[] EdgeDetect(byte[] imageData)
        {
            int[,] gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            var result = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 1; y < bmp.Height - 1; y++)
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    int sx = 0, sy = 0;
                    for (int ky = -1; ky <= 1; ky++)
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            var c = bmp.GetPixel(x + kx, y + ky);
                            int gray = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                            sx += gx[ky + 1, kx + 1] * gray;
                            sy += gy[ky + 1, kx + 1] * gray;
                        }
                    int mag = Math.Min(255, (int)Math.Sqrt(sx * sx + sy * sy));
                    result.SetPixel(x, y, Color.FromArgb(mag, mag, mag));
                }
            using var outStream = new MemoryStream();
            result.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }

        /// <summary>
        /// Đọc/ghi DPI của ảnh.
        /// </summary>
        public static (float dpiX, float dpiY) GetDpi(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            return (bmp.HorizontalResolution, bmp.VerticalResolution);
        }
        public static byte[] SetDpi(byte[] imageData, float dpiX, float dpiY)
        {
            using var ms = new MemoryStream(imageData);
            using var bmp = new Bitmap(ms);
            bmp.SetResolution(dpiX, dpiY);
            using var outStream = new MemoryStream();
            bmp.Save(outStream, bmp.RawFormat);
            return outStream.ToArray();
        }


        /// <summary>
        /// Lấy phần mở rộng file từ định dạng hình ảnh
        /// </summary>
        /// <param name="imageFormat">Định dạng hình ảnh</param>
        /// <returns>Phần mở rộng file</returns>
        public static string GetFileExtension(System.Drawing.Imaging.ImageFormat imageFormat)
        {
            var extension = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
                .Where(ie => ie.FormatID == imageFormat.Guid)
                .Select(ie => ie.FilenameExtension
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Trim('*')
                    .ToLower())
                .FirstOrDefault();

            return extension ?? string.Format(".{0}", imageFormat.ToString().ToLower());
        }

        /// <summary>
        /// Lưu hình ảnh từ mảng byte vào file tạm
        /// </summary>
        /// <param name="imageByte">Mảng byte chứa dữ liệu hình ảnh</param>
        /// <param name="fileName">Tên file</param>
        /// <returns>Đường dẫn file đã lưu</returns>
        public static string SaveImageFromByte(Byte[] imageByte, string fileName)
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                using (var ms = new System.IO.MemoryStream(imageByte))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                    string fullFileName = tempPath + "\\" + fileName + GetFileExtension(image.RawFormat);
                    image.Save(fullFileName);
                    return fullFileName;
                    //using (var fs = new FileStream(fullFileName, FileMode.Create))
                    //{
                    //    ms.WriteTo(fs);
                    //}
                }
            }
            catch (System.Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// Chuyển đổi mảng byte thành bitmap
        /// </summary>
        /// <param name="source">Mảng byte chứa dữ liệu hình ảnh</param>
        /// <returns>Bitmap object</returns>
        public static System.Drawing.Bitmap ConvertArrayToBitmap(System.Byte[] source)
        {
            System.Drawing.Bitmap resultBitmap;
            using (var ms = new System.IO.MemoryStream(source))
            {
                resultBitmap = new System.Drawing.Bitmap(ms);
            }
            return resultBitmap;
        }
        /// <summary>
        /// Chuyển đổi bitmap thành mảng byte
        /// </summary>
        /// <param name="source">Hình ảnh cần chuyển đổi</param>
        /// <returns>Mảng byte chứa dữ liệu hình ảnh</returns>
        public static System.Byte[] ConvertBitmapToArray(System.Drawing.Image source)
        {
            System.Byte[] result = null;
            using (var stream = new MemoryStream())
            {
                source.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                result = stream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra file có phải là SVG không
        /// </summary>
        /// <param name="bytes">Mảng byte chứa dữ liệu file</param>
        /// <returns>True nếu là file SVG</returns>
        public static bool IsSvgFile(byte[] bytes)
        {
            try
            {
                Stream stream = new MemoryStream(bytes);
                using (var xmlReader = System.Xml.XmlReader.Create(stream))
                {
                    return xmlReader.MoveToContent() == System.Xml.XmlNodeType.Element && "svg".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thay đổi kích thước hình ảnh để tạo avatar từ khuôn mặt
        /// </summary>
        /// <param name="source">Hình ảnh gốc</param>
        /// <param name="faceRectangle">Hình chữ nhật chứa khuôn mặt</param>
        /// <param name="zoom">Tỷ lệ phóng to</param>
        /// <returns>Bitmap đã thay đổi kích thước</returns>
        public static System.Drawing.Bitmap ResizeToFaceAvatar(System.Drawing.Image source, System.Drawing.Rectangle faceRectangle, decimal zoom = (decimal)1.2)
        {
            var newWidth = Convert.ToInt32(faceRectangle.Width * zoom);
            var newHeight = Convert.ToInt32(faceRectangle.Height * zoom);
            var x = faceRectangle.X - (newHeight - faceRectangle.Height) / 2;
            if (x < 0)
                x = 0;
            var y = faceRectangle.Y - (newWidth - faceRectangle.Width) / 2;
            if (y < 0)
                y = 0;
            return CropImage(source, x, y, newWidth, newHeight);
        }

        /// <summary>
        /// Thay đổi kích thước hình ảnh để tạo thẻ từ khuôn mặt
        /// </summary>
        /// <param name="source">Hình ảnh gốc</param>
        /// <param name="faceRectangle">Hình chữ nhật chứa khuôn mặt</param>
        /// <returns>Bitmap đã thay đổi kích thước</returns>
        public static System.Drawing.Bitmap ResizeToFaceCard(System.Drawing.Image source, System.Drawing.Rectangle faceRectangle)
        {
            int centerX = faceRectangle.X + faceRectangle.Width / 2;
            int centerY = faceRectangle.Y + faceRectangle.Height / 2;
            int newWidth = faceRectangle.Width * 3;
            int newHeight = faceRectangle.Height * 4;


            var firstLeft = faceRectangle.X - faceRectangle.Width;
            if (firstLeft < 0)
            {
                //Kích thước bị giảm đi 2 lần
                newWidth += firstLeft * 2;
                firstLeft = 0;
            }
            //var right = source.Width - newWidth - firstLeft;
            //if (right < 0)
            //{
            //    newWidth += right * 2;
            //}

            var firstTop = faceRectangle.Y - faceRectangle.Height;
            if (firstTop < 0)
            {
                newHeight += firstTop * 2;
                firstTop = 0;
            }
            //var bottom = source.Width - newHeight - firstTop;
            //if (bottom < 0)
            //{
            //    newHeight += bottom * 2;
            //}
            //Tỉ lệ ảnh là 3 x4;
            var otherWidth = newWidth * 4;
            var otherHeight = newHeight * 3;
            if (otherWidth > otherHeight)
            {
                //Co nhỏ chiều rộng
                var w = otherWidth - otherHeight;
                firstLeft += w / 2 / 4;
                newWidth -= w / 4;
            }
            else if (otherWidth < otherHeight)
            {
                //Co nhỏ chiều cao
                var h = otherHeight - otherWidth;
                firstTop += h / 2 / 3;
                newHeight -= h / 3;
            }
            var ww = newWidth * 4;
            var hh = newHeight * 3;
            return CropImage(source, firstLeft, firstTop, newWidth, newHeight);
        }

        public static System.Drawing.Bitmap RemoveBackground(System.Drawing.Bitmap bmp, int sizing, System.Drawing.Color? firstColor = null)
        {
            System.Drawing.Color pixel = firstColor != null ? firstColor.Value : bmp.GetPixel(1, 1);
            if (pixel.A != 0)
            {
                // Make backColor transparent for myBitmap.
                //bmp.MakeTransparent(pixel);
                //bmp.MakeTransparent(Color.Transparent);                        
                bmp.MakeTransparent();
                //var rMax = pixel.R + sizing;
                //var rMin = pixel.R - sizing;
                //var gMax = pixel.G + sizing;
                //var gMin = pixel.G - sizing;
                //var bMax = pixel.B + sizing;
                //var bMin = pixel.B - sizing;
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        System.Drawing.Color currentColor = bmp.GetPixel(x, y);
                        if (CheckColorIsNearly(currentColor, pixel, sizing))
                        {
                            bmp.SetPixel(x, y, System.Drawing.Color.Transparent);
                        }
                    }
                }
                //bmp.MakeTransparent(MakeTransparent(pixel,100));   
            }
            return bmp;
        }

        

        public static byte[] ResizeImage(byte[] img, int maxSize)
        {
            if (img != null)
            {
                int maxHeight = 304;
                while (img.Length > maxSize)
                {
                    maxHeight = maxHeight * 95 / 100;
                    img = ResizeImage(img, maxHeight, 0);
                }
            }
            return img;
        }
        public static byte[] ResizeImage(byte[] p, int htOfImage, int maxOfWidth = 0)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(p);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            int width = Convert.ToInt32(Convert.ToDouble(img.Width) *
                                        (Convert.ToDouble(htOfImage) / Convert.ToDouble(img.Height)));
            if (maxOfWidth != 0)
            {
                if (width > maxOfWidth)
                {
                    htOfImage = Convert.ToInt32(Convert.ToDouble(img.Height) *
                                                (Convert.ToDouble(maxOfWidth) / Convert.ToDouble(img.Width)));
                    width = maxOfWidth;
                }
            }
            System.Drawing.Size s = new System.Drawing.Size(width, htOfImage);
            System.Drawing.Image resizedImg = Resize(img, s, true);
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                if (System.Drawing.Imaging.ImageFormat.Png.Equals(img.RawFormat))
                {
                    resizedImg.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
                }
                else //of course you could check for bmp, jpg, etc depending on what you allowed
                {
                    resizedImg.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                return memStream.ToArray();
            }
        }
        private static System.Drawing.Image Resize(System.Drawing.Image image,
            System.Drawing.Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(System.Math.Round(originalWidth * percent, 0));
                newHeight = (int)(System.Math.Round(originalHeight * percent, 0));
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            System.Drawing.Image newImage = new System.Drawing.Bitmap(newWidth, newHeight, image.PixelFormat);
            using (System.Drawing.Graphics graphicsHandle = System.Drawing.Graphics.FromImage(newImage))
            {
                //if(image is System.Drawing.Bitmap)
                //{
                //    var firstPixel = ((System.Drawing.Bitmap)image).GetPixel(1, 1);
                //    var brush = new SolidBrush(firstPixel);
                //    graphicsHandle.FillRectangle(brush, 0, 0, newWidth, newHeight);
                //}
                //graphicsHandle.Clear(Color.Black);
                graphicsHandle.CompositingMode = CompositingMode.SourceCopy;
                graphicsHandle.CompositingQuality = CompositingQuality.HighQuality;
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.SmoothingMode = SmoothingMode.HighQuality;
                graphicsHandle.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphicsHandle.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    //graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                }

            }
            return newImage;
        }

        public static System.Drawing.Bitmap CropAndResizeToSquare(System.Drawing.Bitmap bmp, int padding, int margin, int sizing = 0)
        {
            var result = CropWhiteSpace(bmp, true, true, sizing);
            return result;
        }
        private static bool GetAllColorInRow(System.Drawing.Bitmap bmp, int r, System.Drawing.Color color, int sizing = 0)
        {
            for (int i = 0; i < bmp.Width; ++i)
            {
                var currentColor = bmp.GetPixel(i, r);
                if (!CheckColorIsNearly(currentColor, color, sizing))
                    return false;
            }
            return true;
        }
        private static bool GetAllColorInColumn(System.Drawing.Bitmap bmp, int c, System.Drawing.Color color, int sizing = 0)
        {
            for (int i = 0; i < bmp.Height; ++i)
            {
                var currentColor = bmp.GetPixel(c, i);
                if (!CheckColorIsNearly(currentColor, color, sizing))
                    return false;
            }
            return true;
        }

        private static bool CheckColorIsNearly(System.Drawing.Color firstColor, System.Drawing.Color secondColor, int sizing = 0)
        {
            if (sizing == 0)
            {
                if (firstColor != secondColor)
                    return false;
            }
            else
            {
                var rDiff = System.Math.Abs(firstColor.R - secondColor.R);
                var bDiff = System.Math.Abs(firstColor.B - secondColor.B);
                var gDiff = System.Math.Abs(firstColor.G - secondColor.G);
                if ((rDiff + bDiff + gDiff) > sizing * 3)
                    return false;
            }
            return true;
        }
        public static System.Drawing.Bitmap CropWhiteSpace(System.Drawing.Bitmap bmp, bool whiteIsTransparent = true, bool cropLine = true, int sizing = 0)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int white = 0xffffff;
            Func<int, bool> allWhiteRow = r =>
            {
                for (int i = 0; i < w; ++i)
                    if ((bmp.GetPixel(i, r).ToArgb() & white) != white)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = c =>
            {
                for (int i = 0; i < h; ++i)
                    if ((bmp.GetPixel(c, i).ToArgb() & white) != white)
                        return false;
                return true;
            };
            //Fix ảnh từ remove.bg
            int white0 = 0x0;
            Func<int, bool> allWhite0Row = r =>
            {
                for (int i = 0; i < w; ++i)
                {
                    //if ((bmp.GetPixel(i, r).ToArgb() & white0) != white0)                    
                    if (bmp.GetPixel(i, r).ToArgb() != white0)
                        return false;
                }
                return true;
            };

            Func<int, bool> allWhite0Column = c =>
            {
                for (int i = 0; i < h; ++i)
                    //if ((bmp.GetPixel(c, i).ToArgb() & white0) != white0)
                    if (bmp.GetPixel(c, i).ToArgb() != white0)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (!allWhiteRow(row))
                    break;
                topmost = row;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (!allWhiteRow(row))
                    break;
                bottommost = row;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (!allWhiteColumn(col))
                    break;
                leftmost = col;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (!allWhiteColumn(col))
                    break;
                rightmost = col;
            }
            if (topmost == 0 && bottommost == 0 && leftmost == 0 && rightmost == 0)
            {
                //Fix ảnh từ remove.bg                
                for (int row = 0; row < h; ++row)
                {
                    if (!allWhite0Row(row))
                        break;
                    topmost = row;
                }
                for (int row = h - 1; row >= 0; --row)
                {
                    if (!allWhite0Row(row))
                        break;
                    bottommost = row;
                }
                for (int col = 0; col < w; ++col)
                {
                    if (!allWhite0Column(col))
                        break;
                    leftmost = col;
                }
                for (int col = w - 1; col >= 0; --col)
                {
                    if (!allWhite0Column(col))
                        break;
                    rightmost = col;
                }
            }
            if (whiteIsTransparent && (topmost == 0 && bottommost == 0 && leftmost == 0 && rightmost == 0))
            {
                var whiteColor = System.Drawing.Color.White;
                //Ảnh trắng coi như ảnh trong suốt              
                for (int row = 0; row < h; ++row)
                {
                    if (!GetAllColorInRow(bmp, row, whiteColor))
                        break;
                    topmost = row;
                }
                for (int row = h - 1; row >= 0; --row)
                {
                    if (!GetAllColorInRow(bmp, row, whiteColor))
                        break;
                    bottommost = row;
                }
                for (int col = 0; col < w; ++col)
                {
                    if (!GetAllColorInColumn(bmp, col, whiteColor))
                        break;
                    leftmost = col;
                }
                for (int col = w - 1; col >= 0; --col)
                {
                    if (!GetAllColorInColumn(bmp, col, whiteColor))
                        break;
                    rightmost = col;
                }
            }
            if (cropLine && (topmost == 0 && bottommost == 0 && leftmost == 0 && rightmost == 0))
            {
                var firstColor = bmp.GetPixel(0, 0);
                var lastColor = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
                for (int row = 0; row < h; ++row)
                {
                    if (!GetAllColorInRow(bmp, row, firstColor, sizing))
                        break;
                    topmost = row;
                }
                for (int row = h - 1; row >= 0; --row)
                {
                    if (!GetAllColorInRow(bmp, row, lastColor, sizing))
                        break;
                    bottommost = row;
                }
                for (int col = 0; col < w; ++col)
                {
                    if (!GetAllColorInColumn(bmp, col, firstColor, sizing))
                        break;
                    leftmost = col;
                }
                for (int col = w - 1; col >= 0; --col)
                {
                    if (!GetAllColorInColumn(bmp, col, lastColor, sizing))
                        break;
                    rightmost = col;
                }
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                return CropImage(bmp, leftmost, topmost, croppedWidth, croppedHeight);
                //var target = new System.Drawing.Bitmap(croppedWidth, croppedHeight, bmp.PixelFormat);
                //using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(target))
                //{
                //    g.DrawImage(bmp,
                //      new System.Drawing.RectangleF(0, 0, croppedWidth, croppedHeight),
                //      new System.Drawing.RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                //      System.Drawing.GraphicsUnit.Pixel);
                //}
                ////target.RawFormat
                //return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);
            }
        }

        public static System.Drawing.Bitmap CropImage(System.Drawing.Image source, int x, int y, int width, int height)
        {
            System.Drawing.Rectangle crop = new System.Drawing.Rectangle(x, y, width, height);

            var bmp = new System.Drawing.Bitmap(crop.Width, crop.Height);
            using (var gr = System.Drawing.Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), crop, System.Drawing.GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static System.Drawing.Bitmap ResizeToSquare(System.Drawing.Bitmap bmp)
        {
            var largestDimension = Math.Max(bmp.Height, bmp.Width);
            return AddTransparent(bmp, largestDimension, largestDimension);
        }

        public static System.Drawing.Bitmap AddTransparentAndResize(System.Drawing.Bitmap bmp, int width, int? height = null, int? top = null, int? left = null, System.Drawing.Imaging.ImageFormat rawBitmap = null, int removeBackgroundRate = 0)
        {
            //Tạo ảnh vuông
            //int largestDimension = Math.Max(bmp.Height, bmp.Width);
            if (bmp is null)
                return bmp;
            System.Drawing.Size size = new System.Drawing.Size(width, height != null ? height.Value : width);
            bmp = (System.Drawing.Bitmap)Resize(bmp, size, true);
            return AddTransparent(bmp, width, height, top, left, rawBitmap, removeBackgroundRate);
        }
        public static Color FindMostCommonColor(Bitmap image)
        {
            //https://stackoverflow.com/questions/23625917/how-do-i-get-the-background-color-of-bitmap-image-in-c
            // Avoid unnecessary getter calls
            Int32 height = image.Height;
            Int32 width = image.Width;
            Int32 stride;
            Byte[] imageData;
            // Expose bytes as 32bpp ARGB
            BitmapData sourceData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            stride = sourceData.Stride;
            imageData = new Byte[stride * height];
            Marshal.Copy(sourceData.Scan0, imageData, 0, imageData.Length);
            image.UnlockBits(sourceData);
            // Store colour frequencies in a dictionary.
            Dictionary<Color, Int32> colorFreq = new Dictionary<Color, Int32>();
            for (Int32 y = 0; y < height; y++)
            {
                // Reset offset on every line, since stride is not guaranteed to always be width * pixel size.
                Int32 inputOffs = y * stride;
                //Final offset = y * line length in bytes + x * pixel length in bytes.
                //To avoid recalculating that offset each time we just increase it with the pixel size at the end of each x iteration.
                for (Int32 x = 0; x < width; x++)
                {
                    //Get colour components out. "ARGB" is actually the order in the final integer which is read as little-endian, so the real order is BGRA.
                    Color col = Color.FromArgb(imageData[inputOffs + 3], imageData[inputOffs + 2], imageData[inputOffs + 1], imageData[inputOffs]);
                    Color bareCol = Color.FromArgb(255, col);
                    // Only look at nontransparent pixels; cut off at 127.
                    if (col.A > 127)
                    {
                        if (!colorFreq.ContainsKey(bareCol))
                            colorFreq.Add(bareCol, 1);
                        else
                            colorFreq[bareCol]++;
                    }
                    // Increase the offset by the pixel width. For 32bpp ARGB, each pixel is 4 bytes.
                    inputOffs += 4;
                }
            }
            if (colorFreq.Values.Count == 0)
                return Color.Transparent;
            // Get the maximum value in the dictionary values
            Int32 max = colorFreq.Values.Max();
            // Get the first colour that matches that maximum.
            return colorFreq.FirstOrDefault(x => x.Value == max).Key;
            // In case you want to know if there are multiple with the exact same frequency,
            // this could be expanded to give an array with all maxima like this:
            // Color[] maxCols = colorFreq.Where(x => x.Value == max).Select(kvp => kvp.Key).ToArray();
        }
        private static void FillColorDictionary(Dictionary<System.Drawing.Color, List<System.Drawing.Color>> colorDic, System.Drawing.Color color, int removeBackgroundRate)
        {
            foreach (var key in colorDic.Keys)
            {
                if (CheckColorIsNearly(key, color, removeBackgroundRate))
                {
                    colorDic[key].Add(color);
                    return;
                }
            }
            colorDic.Add(color, new List<Color> { color });
        }

        public static System.Drawing.Bitmap AddTransparent(System.Drawing.Bitmap bmp, int width, int? height = null, int? top = null, int? left = null, System.Drawing.Imaging.ImageFormat rawBitmap = null, int removeBackgroundRate = 0)
        {
            //Tạo ảnh vuông
            //int largestDimension = Math.Max(bmp.Height, bmp.Width);
            if (bmp is null)
                return bmp;
            System.Drawing.Size size = new System.Drawing.Size(width, height != null ? height.Value : width);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(size.Width, size.Height);
            //Phóng to hoặc thu nhỏ ảnh

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image))
            {
                System.Drawing.Brush brush = null;
                if (rawBitmap.Equals(System.Drawing.Imaging.ImageFormat.Png) || rawBitmap.Equals(System.Drawing.Imaging.ImageFormat.Tiff) ||
                        rawBitmap.Equals(System.Drawing.Imaging.ImageFormat.Gif) || rawBitmap.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                {
                    int white = 0xffffff;
                    int white0 = 0x0;
                    for (int w = 0; w < bmp.Width; ++w)
                    {
                        for (int h = 0; h < bmp.Height; ++h)
                        {
                            var pixelColor = bmp.GetPixel(w, h).ToArgb();
                            if ((pixelColor & white) == white || pixelColor == white0)
                            {
                                brush = System.Drawing.Brushes.Transparent;
                            }
                        }
                    }
                }
                if (brush is null)
                {
                    //Tìm màu nền outsource                   
                    brush = new SolidBrush(FindMostCommonColor(bmp));
                    //// Tìm màu sắc thật của ảnh
                    ////Tìm 8 điểm của bức ảnh

                    //var colorDic = new Dictionary<System.Drawing.Color, List<System.Drawing.Color>>();
                    //var pixelIndex = 1;
                    ////Topleft
                    //var topLeft = bmp.GetPixel(pixelIndex, pixelIndex);
                    //FillColorDictionary(colorDic, topLeft, removeBackgroundRate);
                    ////topCenter
                    //FillColorDictionary(colorDic, bmp.GetPixel(bmp.Width / 2, pixelIndex), removeBackgroundRate);
                    ////var topRight =
                    //FillColorDictionary(colorDic, bmp.GetPixel(bmp.Width - pixelIndex, pixelIndex), removeBackgroundRate);
                    ////var centerLeft = 
                    //FillColorDictionary(colorDic, bmp.GetPixel(1, bmp.Height / 2), removeBackgroundRate);
                    ////var centerRight =
                    //FillColorDictionary(colorDic, bmp.GetPixel(bmp.Width - pixelIndex, bmp.Height / 2), removeBackgroundRate);
                    ////var bottomLeft = 
                    //FillColorDictionary(colorDic, bmp.GetPixel(1, bmp.Height - pixelIndex), removeBackgroundRate);
                    ////var bottomCenter = 
                    //FillColorDictionary(colorDic, bmp.GetPixel(bmp.Width / 2, bmp.Height - pixelIndex), removeBackgroundRate);
                    ////var bottomRight = 
                    //FillColorDictionary(colorDic, bmp.GetPixel(bmp.Width - pixelIndex, bmp.Height - pixelIndex), removeBackgroundRate);
                    //int max = 0;
                    //System.Drawing.Color brushColor = colorDic.Keys.First();
                    //foreach (var key in colorDic.Keys)
                    //{
                    //    if (colorDic[key].Count > max)
                    //    {
                    //        max = colorDic[key].Count;
                    //        brushColor = key;
                    //    }
                    //    else if (colorDic[key].Count == max)
                    //    {
                    //        max = 0;
                    //    }
                    //}                   

                    //brush = new SolidBrush(brushColor);

                }
                graphics.FillRectangle(brush, 0, 0, size.Width, size.Height);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (top == null)
                {
                    top = (size.Height / 2) - (bmp.Height / 2);
                    if (top < 0)
                        top = 0;
                }
                if (left == null)
                {
                    left = (size.Width / 2) - (bmp.Width / 2);
                    if (left < 0)
                        left = 0;
                }
                graphics.DrawImage(bmp, left.Value, top.Value, bmp.Width, bmp.Height);
            }
            //image.Save("C:\\Code\\T1.jpg");
            return image;
        }



    }
}