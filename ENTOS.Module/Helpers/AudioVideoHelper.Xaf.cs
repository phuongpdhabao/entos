using DevExpress.ExpressApp;
using Elasticsearch.Net;
using NAudio.Wave;
using System.Globalization;
using System.IO.Compression;

namespace ENTOS.Module.Helpers
{
    public static partial class AudioVideoHelper
    {

        /// <summary>
        /// Sao chép file FFmpeg vào thư mục ứng dụng
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để truy cập tham số</param>
        /// <returns>Đường dẫn file FFmpeg</returns>
        public static string CopyFfmpegToApplication(IObjectSpace objectSpace)
        {
            var ffmpegFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "ffmpeg.exe";
            if (!System.IO.File.Exists(ffmpegFile))
            {
                var ffmpegUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "FfmpegUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffmpeg.exe");
                if (!System.IO.File.Exists(ffmpegUrl))
                {
                    return null;
                }
                //Copy FFMpeg vào thư mục đang chạy
                System.IO.File.Copy(ffmpegUrl, ffmpegFile);
            }
            return ffmpegFile;
        }

        /// <summary>
        ///   Dùng FFmpeg để xử lý video và audio (input2: âm thanh sẽ ghép với video ở input 1, volume là sẽ giảm âm lượng âm thanh, mặc điịnh là trích xuất âm từ video )
        /// </summary>
        public static bool ConvertOrMergeToAudio(IObjectSpace objectSpace, string inputFile, string outputFile, decimal? volume = null, string inputFile2 = null)
        {
            if (!string.IsNullOrEmpty(inputFile))
            {
                try
                {
                    if (System.IO.File.Exists(outputFile))
                    {
                        System.IO.File.Delete(outputFile);
                    }
                    else
                    {
                        //Kiểm tra nếu chưa có thư mục thì tạo
                        var outputFileInfo = new System.IO.FileInfo(outputFile);
                        if (!string.IsNullOrEmpty(outputFileInfo.DirectoryName) && !System.IO.Directory.Exists(outputFileInfo.DirectoryName))
                            System.IO.Directory.CreateDirectory(outputFileInfo.DirectoryName);
                    }


                    var ffmpegFile = CopyFfmpegToApplication(objectSpace);
                    //var ffprobeUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (System.IO.File.Exists(ffmpegFile))
                    {
                        //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ffmpegUrl);
                        //psi.RedirectStandardOutput = false;
                        ///psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        //psi.UseShellExecute = false;
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "ffmpeg.exe";
                        process.EnableRaisingEvents = false;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.CreateNoWindow = true;
                        //tham số -y là luôn ghi đè file nếu đã tồn tại
                        if (!string.IsNullOrEmpty(inputFile2))
                        {
                            process.StartInfo.Arguments = $"-y -i \"{inputFile}\" -i \"{inputFile2}\"  -map 0:v -map 1:a -c:v copy -shortest \"{outputFile}\"";
                        }
                        else if (volume != null)
                        {
                            //Giảm âm lượng âm thanh
                            var volumeString = volume.Value.ToString(new CultureInfo("en"));
                            process.StartInfo.Arguments = $"-y -i \"{inputFile}\" -af \"volume={volumeString}\" \"{outputFile}\"";
                        }
                        else
                        {
                            //Xuất âm thanh từ video ra mp3
                            process.StartInfo.Arguments = $"-y -i \"{inputFile}\" -q:a 0 -map a \"{outputFile}\"";
                        }
                        //process.WaitForExit();
                        //Đợi tối đa 5 phút
                        int max = 5 * 60;
                        int totalSeconds = 0;
                        if (process.Start())
                        {
                            while (!process.HasExited)
                            {
                                totalSeconds++;
                                if (totalSeconds > max)
                                    break;
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                        ////Create a streamreader to capture the output of ischk

                        return true;

                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            return false;
        }


        /// <summary>
        ///   Dùng FFmpeg để Cắt âm thanh đầu và cuối 
        /// </summary>
        public static bool TrimAudio(IObjectSpace objectSpace, string inputFile, string outputFile, TimeSpan? timeStart, TimeSpan? timeEnd)
        {
            if (!string.IsNullOrEmpty(inputFile))
            {
                try
                {
                    if (System.IO.File.Exists(outputFile))
                    {
                        System.IO.File.Delete(outputFile);
                    }
                    else
                    {
                        //Kiểm tra nếu chưa có thư mục thì tạo
                        var outputFileInfo = new System.IO.FileInfo(outputFile);
                        if (!string.IsNullOrEmpty(outputFileInfo.DirectoryName) && !System.IO.Directory.Exists(outputFileInfo.DirectoryName))
                            System.IO.Directory.CreateDirectory(outputFileInfo.DirectoryName);
                    }


                    var ffmpegFile = CopyFfmpegToApplication(objectSpace);
                    //var ffprobeUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (System.IO.File.Exists(ffmpegFile))
                    {
                        //tham số -y là luôn ghi đè file nếu đã tồn tại
                        //Xuất âm thanh từ video ra mp3
                        string arguments = $"-y -i \"{inputFile}\"";
                        if (timeStart != null)
                            arguments += $" -ss {timeStart.Value.ToString(@"hh\:mm\:ss\.fff")}";
                        if (timeEnd != null)
                            arguments += $" -to {timeEnd.Value.ToString(@"hh\:mm\:ss\.fff")}";
                        arguments += $" -c copy \"{outputFile}\"";
                        //Đợi tối đa 5 phút
                        Module.Helpers.ProcessHelper.RunProcessOutside("ffmpeg.exe", arguments, 5 * 60);
                        return true;

                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            return false;
        }

        /// <summary>
        /// Trộn nhiều file MP3 thành một file
        /// </summary>
        /// <param name="output">File đầu ra</param>
        /// <param name="inputs">Mảng các file đầu vào</param>
        public static void MixMp3(string output, string[] inputs)
        {
            var audioFileReaders = System.Array.ConvertAll(inputs, a => new NAudio.Wave.AudioFileReader(a).ToStereo());
            NAudio.Wave.SampleProviders.MixingSampleProvider mixer = new NAudio.Wave.SampleProviders.MixingSampleProvider(audioFileReaders);
            //foreach (string input in inputs)
            //{
            //    NAudio.Wave.AudioFileReader readerAudio = new NAudio.Wave.AudioFileReader(input);
            //    mixer.AddMixerInput(readerAudio);
            //}

            //WaveFileWriter.CreateWaveFile16(tempFolder + "\\mixed.mp3", mixer);
            var converted16Bit = new NAudio.Wave.SampleProviders.SampleToWaveProvider16(mixer);
            //Convert ra mp3
            using (var resampled = new NAudio.Wave.MediaFoundationResampler(converted16Bit, new NAudio.Wave.WaveFormat(44100, 1)))
            {
                var desiredBitRate = 0; // ask for lowest available bitrate 
                                        //int desiredBitRate = 128000;
                NAudio.Wave.MediaFoundationEncoder.EncodeToMp3(resampled, output, desiredBitRate);
            }


        }


        /// <summary>
        /// Lấy thời lượng của file âm thanh
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để truy cập tham số</param>
        /// <param name="audioFile">Đường dẫn file âm thanh</param>
        /// <returns>Thời lượng file âm thanh</returns>
        public static decimal? GetDuration(IObjectSpace objectSpace, string audioFile)
        {
            if (!string.IsNullOrEmpty(audioFile))
            {
                try
                {
                    //var ffmpegUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FfmpegUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffmpeg.exe");
                    var ffprobeUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (System.IO.File.Exists(ffprobeUrl) && System.IO.File.Exists(audioFile))
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ffprobeUrl);
                        psi.Arguments = "-i \"" + audioFile + "\" -show_entries format=duration -v quiet -of csv=\"p=0\"";
                        psi.RedirectStandardOutput = true;
                        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.EnableRaisingEvents = true;
                        process.WaitForExit();
                        ////Create a streamreader to capture the output of ischk
                        System.IO.StreamReader ischkout = process.StandardOutput;
                        process.WaitForExit();
                        if (process.HasExited)
                        {
                            string output = ischkout.ReadToEnd();
                            if (!string.IsNullOrEmpty(output))
                            {
                                output = output.Replace("\r", "").Replace("\n", "");
                                return decimal.Parse(output, new System.Globalization.CultureInfo("en-us"));
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            return null;
        }


        /// <summary>
        /// Tự động cài đặt FFmpeg nếu chưa có trong hệ thống.
        /// Tải và cài đặt FFmpeg từ nguồn chính thức để xử lý audio/video.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <example>
        /// // Tự động cài đặt FFmpeg khi cần
        /// Tools.AutoInstalledFfmpeg(objectSpace);
        /// 
        /// // Sau đó có thể sử dụng các phương thức xử lý audio/video
        /// Tools.ConvertOrMergeToAudio(objectSpace, "input.mp4", "output.mp3");
        /// </example>
        public static void AutoInstalledFfmpeg(IObjectSpace objectSpace)
        {
            string ffmpegUrl = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "FfmpegUrlInLocal", "C:\\Ffmpeg\\ffmpeg.exe", SecuritySystem.CurrentUserId).Value;
            if (!System.IO.File.Exists(ffmpegUrl))
            {
                var fileInfo = new System.IO.FileInfo(ffmpegUrl);
                string ffmpegUrlDownloadUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "FfmpegUrlDownloadUrl", "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip");
                string fFmpegDownloadPath = "ffmpeg-release-essentials.zip";
                try
                {
                    if (!System.IO.Directory.Exists(fileInfo.Directory.FullName))
                        System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);
                    // Tải FFmpeg
                    if (!System.IO.File.Exists(fFmpegDownloadPath))
                    {
                        System.Threading.Tasks.Task.Run(async () =>
                        {
                            using HttpClient client = new HttpClient();
                            using var response = await client.GetAsync(ffmpegUrlDownloadUrl, HttpCompletionOption.ResponseHeadersRead);
                            response.EnsureSuccessStatusCode();

                            await using var stream = await response.Content.ReadAsStreamAsync();
                            await using var fileStream = new FileStream(fFmpegDownloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
                            await stream.CopyToAsync(fileStream);
                        }).Wait();
                        //using (WebClient client = new WebClient())
                        //{
                        //    client.DownloadFile(ffmpegUrlDownloadUrl, fFmpegDownloadPath);
                        //}
                    }
                    if (System.IO.Directory.Exists(fileInfo.Directory.FullName))
                        System.IO.Directory.Delete(fileInfo.Directory.FullName, true);
                    // Giải nén FFmpeg
                    ZipFile.ExtractToDirectory(fFmpegDownloadPath, fileInfo.Directory.FullName);
                    // Di chuyển và cấu hình lại các thư mục
                    // Tìm thư mục con vừa được giải nén
                    string extractedFolder = Directory.GetDirectories(fileInfo.Directory.FullName)[0]; // Lấy thư mục con đầu tiên
                    string ffmpegBinPath = Path.Combine(extractedFolder, "bin");

                    // Kiểm tra xem thư mục bin có tồn tại không
                    if (!Directory.Exists(ffmpegBinPath))
                    {
                        throw new DirectoryNotFoundException($"The bin directory could not be found in {extractedFolder}");
                    }
                    //string finalPath = Path.Combine(fileInfo.Directory.FullName, "bin");

                    //if (System.IO.Directory.Exists(finalPath))
                    //{
                    //    System.IO.Directory.Delete(finalPath, true);
                    //}
                    foreach (var file in System.IO.Directory.GetFiles(ffmpegBinPath))
                    {
                        System.IO.File.Move(file, Path.Combine(fileInfo.Directory.FullName, Path.GetFileName(file)));
                    }

                    //System.IO.Directory.Move(ffmpegBinPath, finalPath);

                    // Thêm vào PATH
                    string pathVariable = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                    if (!pathVariable.Split(';').Contains(fileInfo.Directory.FullName))
                    {
                        System.Environment.SetEnvironmentVariable("PATH", pathVariable + ";" + fileInfo.Directory.FullName, EnvironmentVariableTarget.User);
                    }
                }
                catch (Exception)
                {
                    if (System.IO.File.Exists(fFmpegDownloadPath))
                    {
                        System.IO.File.Delete(fFmpegDownloadPath);
                    }
                }

            }
            else
            {
                // Thêm vào PATH
                string pathVariable = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                var fileInfo = new System.IO.FileInfo(ffmpegUrl);
                if (!pathVariable.Split(';').Contains(fileInfo.Directory.FullName))
                {
                    System.Environment.SetEnvironmentVariable("PATH", pathVariable + ";" + fileInfo.Directory.FullName, EnvironmentVariableTarget.User);
                }
            }
        }
    }
}
