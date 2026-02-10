using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý cơ bản cho audio/video (placeholder, cần thư viện ngoài để thực thi thực tế).
    /// </summary>
    public static partial class AudioVideoHelper
    {
        /// <summary>
        /// Tách audio sử dụng ffmpeg (cần ffmpeg.exe trong PATH).
        /// </summary>
        public static async Task<bool> SplitAudio(string inputPath, string outputPath, TimeSpan start, TimeSpan duration)
        {
            string args = $"-y -i \"{inputPath}\" -ss {start} -t {duration} -vn -acodec copy \"{outputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("SplitAudio failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Ghép nhiều file audio sử dụng ffmpeg (cần ffmpeg.exe trong PATH).
        /// </summary>
        public static async Task<bool> MergeAudio(IEnumerable<string> inputPaths, string outputPath)
        {
            // Tạo file list cho ffmpeg
            string listFile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllLines(listFile, inputPaths.Select(p => $"file '{p.Replace("'", "'\\''")}'"));
            string args = $"-y -f concat -safe 0 -i \"{listFile}\" -c copy \"{outputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            System.IO.File.Delete(listFile);
            if (!result.Success)
                Log.Error("MergeAudio failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Tách video sử dụng ffmpeg (cần ffmpeg.exe trong PATH).
        /// </summary>
        public static async Task<bool> SplitVideo(string inputPath, string outputPath, TimeSpan start, TimeSpan duration)
        {
            string args = $"-y -i \"{inputPath}\" -ss {start} -t {duration} -c copy \"{outputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("SplitVideo failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Ghép nhiều file video sử dụng ffmpeg (cần ffmpeg.exe trong PATH).
        /// </summary>
        public static async Task<bool> MergeVideo(IEnumerable<string> inputPaths, string outputPath)
        {
            // Tạo file list cho ffmpeg
            string listFile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllLines(listFile, inputPaths.Select(p => $"file '{p.Replace("'", "'\\''")}'"));
            string args = $"-y -f concat -safe 0 -i \"{listFile}\" -c copy \"{outputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            System.IO.File.Delete(listFile);
            if (!result.Success)
                Log.Error("MergeVideo failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Tách âm thanh khỏi video, cho phép chọn định dạng và bitrate.
        /// </summary>
        public static async Task<bool> ExtractAudioFromVideo(string videoPath, string audioOutputPath, string audioFormat = "mp3", string audioBitrate = null)
        {
            string bitrateArg = string.IsNullOrWhiteSpace(audioBitrate) ? "" : $"-b:a {audioBitrate}";
            string args = $"-y -i \"{videoPath}\" -vn -acodec {audioFormat} {bitrateArg} \"{audioOutputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("ExtractAudioFromVideo failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Giảm dung lượng âm thanh theo bitrate (compress audio).
        /// </summary>
        public static async Task<bool> CompressAudio(string inputAudioPath, string outputAudioPath, string audioBitrate)
        {
            string args = $"-y -i \"{inputAudioPath}\" -b:a {audioBitrate} \"{outputAudioPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("CompressAudio failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Ghép âm thanh vào video (thay thế audio cũ).
        /// </summary>
        public static async Task<bool> MergeAudioToVideo(string videoPath, string audioPath, string outputVideoPath)
        {
            string args = $"-y -i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -map 0:v:0 -map 1:a:0 -shortest \"{outputVideoPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("MergeAudioToVideo failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Trích xuất ảnh từ video tại thời điểm chỉ định (frame snapshot).
        /// </summary>
        public static async Task<bool> ExtractImageFromVideo(string videoPath, string imageOutputPath, TimeSpan timestamp)
        {
            string args = $"-y -i \"{videoPath}\" -ss {timestamp} -vframes 1 \"{imageOutputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("ExtractImageFromVideo failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Chuyển đổi định dạng video (ví dụ: mp4 -> avi, mkv, mov, ...).
        /// </summary>
        public static async Task<bool> ConvertVideoFormat(string inputPath, string outputPath, string videoCodec = null, string audioCodec = null)
        {
            string vcodec = string.IsNullOrWhiteSpace(videoCodec) ? "" : $"-c:v {videoCodec}";
            string acodec = string.IsNullOrWhiteSpace(audioCodec) ? "" : $"-c:a {audioCodec}";
            string args = $"-y -i \"{inputPath}\" {vcodec} {acodec} \"{outputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("ConvertVideoFormat failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Lấy metadata video/audio bằng ffprobe, trả về chuỗi JSON.
        /// </summary>
        public static async Task<string> GetVideoMetadata(string inputPath)
        {
            string args = $"-v quiet -print_format json -show_format -show_streams \"{inputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffprobe", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
            {
                Log.Error("GetVideoMetadata failed: {Error}", result.Error);
                return null;
            }
            return result.Output;
        }

        /// <summary>
        /// Lấy thời lượng video/audio (giây, trả về TimeSpan).
        /// </summary>
        public static async Task<TimeSpan?> GetMediaDuration(string inputPath)
        {
            string args = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{inputPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffprobe", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
            {
                Log.Error("GetMediaDuration failed: {Error}", result.Error);
                return null;
            }
            if (double.TryParse(result.Output?.Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double seconds))
                return TimeSpan.FromSeconds(seconds);
            return null;
        }

        /// <summary>
        /// Lấy dung lượng file media (byte).
        /// </summary>
        public static long GetMediaFileSize(string inputPath)
        {
            try { return new System.IO.FileInfo(inputPath).Length; }
            catch (Exception ex) { Log.Error("GetMediaFileSize failed: {Error}", ex.Message); return -1; }
        }

        /// <summary>
        /// Ghép audio vào video tại thời điểm chỉ định (overlay audio, giữ audio gốc hoặc thay thế).
        /// </summary>
        /// <param name="videoPath">Video gốc</param>
        /// <param name="audioPath">Audio cần ghép</param>
        /// <param name="outputVideoPath">Video đầu ra</param>
        /// <param name="startAt">Thời điểm bắt đầu ghép audio (TimeSpan)</param>
        /// <param name="replaceOriginal">Nếu true, thay thế audio gốc; nếu false, overlay audio mới lên audio cũ</param>
        public static async Task<bool> MergeAudioToVideoAt(string videoPath, string audioPath, string outputVideoPath, TimeSpan startAt, bool replaceOriginal = false)
        {
            string audioFilter = replaceOriginal
                ? $"adelay={startAt.TotalMilliseconds}|{startAt.TotalMilliseconds}" // chỉ dùng audio mới, delay theo startAt
                : $"[1]adelay={startAt.TotalMilliseconds}|{startAt.TotalMilliseconds}[a1];[0][a1]amix=inputs=2"; // overlay audio mới lên audio cũ
            string args = replaceOriginal
                ? $"-y -i \"{videoPath}\" -i \"{audioPath}\" -filter_complex \"[1]adelay={startAt.TotalMilliseconds}|{startAt.TotalMilliseconds}[a1];[0][a1]amix=inputs=2\" -map 0:v -map \"[a1]\" -c:v copy -shortest \"{outputVideoPath}\""
                : $"-y -i \"{videoPath}\" -i \"{audioPath}\" -filter_complex \"[1]adelay={startAt.TotalMilliseconds}|{startAt.TotalMilliseconds}[a1];[0:a][a1]amix=inputs=2[aout]\" -map 0:v -map \"[aout]\" -c:v copy -shortest \"{outputVideoPath}\"";
            var result = await ProcessHelper.RunProcessWithResultAsync("ffmpeg", args, new ProcessHelper.ProcessOptions { RedirectOutput = true, RedirectError = true, UseShellExecute = false });
            if (!result.Success)
                Log.Error("MergeAudioToVideoAt failed: {Error}", result.Error);
            return result.Success;
        }

        /// <summary>
        /// Lấy số khung hình mỗi giây (frame rate) của video sử dụng FFmpeg.
        /// </summary>
        /// <param name="videoPath">Đường dẫn đến file video.</param>
        /// <returns>Giá trị frame rate (fps) nếu lấy được; nếu không, trả về mặc định 30.0.</returns>
        public static double GetFrameRate(string videoPath)
        {
            string ffmpegPath = "ffmpeg";
            string arguments = $"-i \"{videoPath}\"";
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var process = new System.Diagnostics.Process { StartInfo = processInfo })
            {
                process.Start();
                string output = process.StandardError.ReadToEnd();
                process.WaitForExit();
                var regex = new System.Text.RegularExpressions.Regex(@"(\d+(\.\d+)?) fps");
                var match = regex.Match(output);
                if (match.Success)
                {
                    return double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return 30.0;
        }

        /// <summary>
        /// Lấy tổng số frame của video bằng cách sử dụng FFprobe.
        /// </summary>
        /// <param name="videoPath">Đường dẫn đến file video.</param>
        /// <returns>Tổng số frame nếu lấy được; nếu không, trả về 0.</returns>
        /// <remarks>
        /// Yêu cầu FFprobe phải được cài đặt và có trong PATH hệ thống.
        /// </remarks>
        public static int GetTotalFrameCount(string videoPath)
        {
            string ffprobePath = "ffprobe";
            var args = $"-v error -count_frames -select_streams v:0 -show_entries stream=nb_read_frames -of default=nokey=1:noprint_wrappers=1 \"{videoPath}\"";

            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffprobePath,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new System.Diagnostics.Process { StartInfo = processInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (int.TryParse(output.Trim(), out int frameCount))
                    return frameCount;
                return 0;
            }
        }

        /// <summary>
        /// Kiểm tra URL có hỗ trợ định dạng video không
        /// </summary>
        /// <param name="url">URL cần kiểm tra</param>
        /// <returns>True nếu hỗ trợ định dạng video</returns>
        public static bool CheckVideoSupport(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var extension = System.IO.Path.GetExtension(url);
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