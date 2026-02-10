using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;


namespace ENTOS.Module.Helpers
{
    public static partial class MediaHelper
    {
        public static string SaveImageFromByte(byte[] imageByte, string fileName)
        {
            try
            {
                string tempPath = Path.GetTempPath();
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                using (var ms = new MemoryStream(imageByte))
                {
                    Image image = Image.FromStream(ms);
                    string fullFileName = tempPath + "\\" + fileName + Module.Helpers.ImageHelper.GetFileExtension(image.RawFormat);
                    image.Save(fullFileName);
                    return fullFileName;
                    //using (var fs = new FileStream(fullFileName, FileMode.Create))
                    //{
                    //    ms.WriteTo(fs);
                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static Bitmap ConvertArrayToBitmap(byte[] source)
        {
            Bitmap resultBitmap;
            using (var ms = new MemoryStream(source))
            {
                resultBitmap = new Bitmap(ms);
            }
            return resultBitmap;
        }
        public static byte[] ConvertBitmapToArray(Image source)
        {
            byte[] result = null;
            using (var stream = new MemoryStream())
            {
                source.Save(stream, ImageFormat.Png);
                result = stream.ToArray();
            }
            return result;
        }

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

        public static Bitmap ResizeToFaceAvatar(Image source, Rectangle faceRectangle, decimal zoom = (decimal)1.2)
        {
            var newWidth = Convert.ToInt32(faceRectangle.Width * zoom);
            var newHeight = Convert.ToInt32(faceRectangle.Height * zoom);
            var x = faceRectangle.X - (newHeight - faceRectangle.Height) / 2;
            if (x < 0)
                x = 0;
            var y = faceRectangle.Y - (newWidth - faceRectangle.Width) / 2;
            if (y < 0)
                y = 0;
            return Module.Helpers.ImageHelper.CropImage(source, x, y, newWidth, newHeight);
        }

        public static Bitmap ResizeToFaceCard(Image source, Rectangle faceRectangle)
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
            return Module.Helpers.ImageHelper.CropImage(source, firstLeft, firstTop, newWidth, newHeight);
        }

        public static Bitmap RemoveBackground(Bitmap bmp, int sizing, Color? firstColor = null)
        {
            Color pixel = firstColor != null ? firstColor.Value : bmp.GetPixel(1, 1);
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
                        Color currentColor = bmp.GetPixel(x, y);
                        if (CheckColorIsNearly(currentColor, pixel, sizing))
                        {
                            bmp.SetPixel(x, y, Color.Transparent);
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
            MemoryStream ms = new MemoryStream(p);
            Image img = Image.FromStream(ms);
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
            Size s = new Size(width, htOfImage);
            Image resizedImg = Resize(img, s, true);
            using (MemoryStream memStream = new MemoryStream())
            {
                if (ImageFormat.Png.Equals(img.RawFormat))
                {
                    resizedImg.Save(memStream, ImageFormat.Png);
                }
                else //of course you could check for bmp, jpg, etc depending on what you allowed
                {
                    resizedImg.Save(memStream, ImageFormat.Jpeg);
                }
                return memStream.ToArray();
            }
        }
        private static Image Resize(Image image,
            Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = size.Width / (float)originalWidth;
                float percentHeight = size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)Math.Round(originalWidth * percent, 0);
                newHeight = (int)Math.Round(originalHeight * percent, 0);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            Image newImage = new Bitmap(newWidth, newHeight, image.PixelFormat);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
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

        public static Bitmap CropAndResizeToSquare(Bitmap bmp, int padding, int margin, int sizing = 0)
        {
            var result = CropWhiteSpace(bmp, true, true, sizing);
            return result;
        }
        private static bool GetAllColorInRow(Bitmap bmp, int r, Color color, int sizing = 0)
        {
            for (int i = 0; i < bmp.Width; ++i)
            {
                var currentColor = bmp.GetPixel(i, r);
                if (!CheckColorIsNearly(currentColor, color, sizing))
                    return false;
            }
            return true;
        }
        private static bool GetAllColorInColumn(Bitmap bmp, int c, Color color, int sizing = 0)
        {
            for (int i = 0; i < bmp.Height; ++i)
            {
                var currentColor = bmp.GetPixel(c, i);
                if (!CheckColorIsNearly(currentColor, color, sizing))
                    return false;
            }
            return true;
        }

        private static bool CheckColorIsNearly(Color firstColor, Color secondColor, int sizing = 0)
        {
            if (sizing == 0)
            {
                if (firstColor != secondColor)
                    return false;
            }
            else
            {
                var rDiff = Math.Abs(firstColor.R - secondColor.R);
                var bDiff = Math.Abs(firstColor.B - secondColor.B);
                var gDiff = Math.Abs(firstColor.G - secondColor.G);
                if (rDiff + bDiff + gDiff > sizing * 3)
                    return false;
            }
            return true;
        }
        public static Bitmap CropWhiteSpace(Bitmap bmp, bool whiteIsTransparent = true, bool cropLine = true, int sizing = 0)
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
            if (whiteIsTransparent && topmost == 0 && bottommost == 0 && leftmost == 0 && rightmost == 0)
            {
                var whiteColor = Color.White;
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
            if (cropLine && topmost == 0 && bottommost == 0 && leftmost == 0 && rightmost == 0)
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

        public static Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static Bitmap ResizeToSquare(Bitmap bmp)
        {
            var largestDimension = Math.Max(bmp.Height, bmp.Width);
            return AddTransparent(bmp, largestDimension, largestDimension);
        }

        public static Bitmap AddTransparentAndResize(Bitmap bmp, int width, int? height = null, int? top = null, int? left = null, ImageFormat rawBitmap = null, int removeBackgroundRate = 0)
        {
            //Tạo ảnh vuông
            //int largestDimension = Math.Max(bmp.Height, bmp.Width);
            if (bmp is null)
                return bmp;
            Size size = new Size(width, height != null ? height.Value : width);
            bmp = (Bitmap)Resize(bmp, size, true);
            return AddTransparent(bmp, width, height, top, left, rawBitmap, removeBackgroundRate);
        }
        public static Color FindMostCommonColor(Bitmap image)
        {
            //https://stackoverflow.com/questions/23625917/how-do-i-get-the-background-color-of-bitmap-image-in-c
            // Avoid unnecessary getter calls
            int height = image.Height;
            int width = image.Width;
            int stride;
            byte[] imageData;
            // Expose bytes as 32bpp ARGB
            BitmapData sourceData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            stride = sourceData.Stride;
            imageData = new byte[stride * height];
            Marshal.Copy(sourceData.Scan0, imageData, 0, imageData.Length);
            image.UnlockBits(sourceData);
            // Store colour frequencies in a dictionary.
            Dictionary<Color, int> colorFreq = new Dictionary<Color, int>();
            for (int y = 0; y < height; y++)
            {
                // Reset offset on every line, since stride is not guaranteed to always be width * pixel size.
                int inputOffs = y * stride;
                //Final offset = y * line length in bytes + x * pixel length in bytes.
                //To avoid recalculating that offset each time we just increase it with the pixel size at the end of each x iteration.
                for (int x = 0; x < width; x++)
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
            int max = colorFreq.Values.Max();
            // Get the first colour that matches that maximum.
            return colorFreq.FirstOrDefault(x => x.Value == max).Key;
            // In case you want to know if there are multiple with the exact same frequency,
            // this could be expanded to give an array with all maxima like this:
            // Color[] maxCols = colorFreq.Where(x => x.Value == max).Select(kvp => kvp.Key).ToArray();
        }
        private static void FillColorDictionary(Dictionary<Color, List<Color>> colorDic, Color color, int removeBackgroundRate)
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

        public static Bitmap AddTransparent(Bitmap bmp, int width, int? height = null, int? top = null, int? left = null, ImageFormat rawBitmap = null, int removeBackgroundRate = 0)
        {
            //Tạo ảnh vuông
            //int largestDimension = Math.Max(bmp.Height, bmp.Width);
            if (bmp is null)
                return bmp;
            Size size = new Size(width, height != null ? height.Value : width);
            Bitmap image = new Bitmap(size.Width, size.Height);
            //Phóng to hoặc thu nhỏ ảnh

            using (Graphics graphics = Graphics.FromImage(image))
            {
                Brush brush = null;
                if (rawBitmap.Equals(ImageFormat.Png) || rawBitmap.Equals(ImageFormat.Tiff) ||
                        rawBitmap.Equals(ImageFormat.Gif) || rawBitmap.Equals(ImageFormat.Icon))
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
                                brush = Brushes.Transparent;
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
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                if (top == null)
                {
                    top = size.Height / 2 - bmp.Height / 2;
                    if (top < 0)
                        top = 0;
                }
                if (left == null)
                {
                    left = size.Width / 2 - bmp.Width / 2;
                    if (left < 0)
                        left = 0;
                }
                graphics.DrawImage(bmp, left.Value, top.Value, bmp.Width, bmp.Height);
            }
            //image.Save("C:\\Code\\T1.jpg");
            return image;
        }


        public static Bitmap Resize(Bitmap bmp, int newWidth, int height)
        {

            Bitmap image = new Bitmap(newWidth, newWidth);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, 0, 0, newWidth, newWidth);
            }
            return image;
        }

        // Xử lý Video
        public static bool CheckVideoSupport(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var extension = Path.GetExtension(url);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    if (extension == ".mkv" || extension == ".mp4" || extension == ".mpeg" || extension == ".qt"
                        || extension == ".wmv" || extension == ".m4p" || extension == ".mpv" || extension == ".flv"
                        || extension == ".mov" || extension == ".avi" || extension == ".webm")
                        return true;
                }
            }
            return false;
        }

        

    }
}
