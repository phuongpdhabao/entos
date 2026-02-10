using DevExpress.ExpressApp;
using Elasticsearch.Net;
using ENTOS.Module.SystemObjects;
using Microsoft.VisualBasic;
using NAudio.Wave;
using System.Diagnostics;
using System.Globalization;

namespace ENTOS.Module.Helpers
{
    public static partial class MediaHelper
    {

        public static byte[] RemoveBackgroundApi(byte[] images, string api = null, IObjectSpace objectSpace = null)
        {
            if (string.IsNullOrEmpty(api))
            {
                api = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "ApiKeyRemove.bg", "thdpo4psPsDX4LCEYk9171SD");
            }
            if (string.IsNullOrEmpty(api))
                return null;
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Headers.Add("X-Api-Key", api);
                formData.Add(new ByteArrayContent(images), "image_file", "file.png");
                formData.Add(new StringContent("auto"), "size");
                var response = client.PostAsync("https://api.remove.bg/v1.0/removebg", formData).Result;

                if (response.IsSuccessStatusCode)
                {
                    //FileStream fileStream = new FileStream("no-bg.png", FileMode.Create, FileAccess.Write, FileShare.None);                    
                    //response.Content.CopyToAsync(fileStream).ContinueWith((copyTask) => { fileStream.Close(); });
                    using (MemoryStream stream = new MemoryStream())
                    {
                        byte[] result = null;
                        var task = response.Content.CopyToAsync(stream);
                        int wait = 0;
                        while (!task.IsCompleted && wait < 10)
                        {
                            task.Wait(1000);
                            wait++;
                        }
                        if (task.IsCompleted)
                        {
                            result = stream.ToArray();
                        }
                        return result;
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + response.Content.ReadAsStringAsync().Result);
                    return null;
                }
            }
            return null;
        }
        public static string CopyFfmpegToApplication(IObjectSpace objectSpace)
        {
            var ffmpegFile = Directory.GetCurrentDirectory() + "\\" + "ffmpeg.exe";
            if (!File.Exists(ffmpegFile))
            {
                var ffmpegUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "FfmpegUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffmpeg.exe");
                if (!File.Exists(ffmpegUrl))
                {
                    return null;
                }
                //Copy FFMpeg vào thư mục đang chạy
                File.Copy(ffmpegUrl, ffmpegFile);
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
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }
                    else
                    {
                        //Kiểm tra nếu chưa có thư mục thì tạo
                        var outputFileInfo = new System.IO.FileInfo(outputFile);
                        if (!string.IsNullOrEmpty(outputFileInfo.DirectoryName) && !Directory.Exists(outputFileInfo.DirectoryName))
                            Directory.CreateDirectory(outputFileInfo.DirectoryName);
                    }


                    var ffmpegFile = CopyFfmpegToApplication(objectSpace);
                    //var ffprobeUrl = Module.SystemObjects.Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (File.Exists(ffmpegFile))
                    {
                        //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ffmpegUrl);
                        //psi.RedirectStandardOutput = false;
                        ///psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        //psi.UseShellExecute = false;
                        Process process = new Process();
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
                                Thread.Sleep(1000);
                            }
                        }
                        ////Create a streamreader to capture the output of ischk

                        return true;

                    }
                }
                catch (Exception ex)
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
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }
                    else
                    {
                        //Kiểm tra nếu chưa có thư mục thì tạo
                        var outputFileInfo = new System.IO.FileInfo(outputFile);
                        if (!string.IsNullOrEmpty(outputFileInfo.DirectoryName) && !Directory.Exists(outputFileInfo.DirectoryName))
                            Directory.CreateDirectory(outputFileInfo.DirectoryName);
                    }


                    var ffmpegFile = CopyFfmpegToApplication(objectSpace);
                    //var ffprobeUrl = Module.SystemObjects.Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (File.Exists(ffmpegFile))
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
                catch (Exception ex)
                {

                }
            }
            return false;
        }


        public static void MixMp3(string output, string[] inputs)
        {
            var audioFileReaders = Array.ConvertAll(inputs, a => new AudioFileReader(a).ToStereo());
            NAudio.Wave.SampleProviders.MixingSampleProvider mixer = new NAudio.Wave.SampleProviders.MixingSampleProvider(audioFileReaders);
            //foreach (string input in inputs)
            //{
            //    NAudio.Wave.AudioFileReader readerAudio = new NAudio.Wave.AudioFileReader(input);
            //    mixer.AddMixerInput(readerAudio);
            //}

            //WaveFileWriter.CreateWaveFile16(tempFolder + "\\mixed.mp3", mixer);
            var converted16Bit = new NAudio.Wave.SampleProviders.SampleToWaveProvider16(mixer);
            //Convert ra mp3
            using (var resampled = new MediaFoundationResampler(converted16Bit, new WaveFormat(44100, 1)))
            {
                var desiredBitRate = 0; // ask for lowest available bitrate 
                                        //int desiredBitRate = 128000;
                MediaFoundationEncoder.EncodeToMp3(resampled, output, desiredBitRate);
            }
        }

    }
}
