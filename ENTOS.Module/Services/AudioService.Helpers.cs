using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.Helpers;

namespace ENTOS.Module.Services
{
    public partial class AudioService
    {
        private void ExportTrainToFile(string wavFolder, string trainFormat, AudioProcessingItem item, System.Collections.Concurrent.ConcurrentQueue<string> trainLines, int total, System.Diagnostics.Stopwatch stopWatch)
        {
            string fileName = $"{item.Start.ToString(@"dd\.hhmmss")}{Path.GetExtension(item.FileName)}";
            string fullPath = Path.Combine(wavFolder, fileName);
            File.WriteAllBytes(fullPath, item.Content);
            string targetExt = ".wav";
            FfmpegAudioConverter(ref fileName, fullPath, wavFolder, item.Start);
            string line = string.Format(trainFormat, fileName, new string(item.ContentText.Where(c => VietnameseSymbols.Contains(c)).ToArray()));
            trainLines.Enqueue(line);
            Tools.ShowOrCloseDefaultWaitForm(null, $"{trainLines.Count.ToString("D")}/{total.ToString("D")}", stopWatch.Elapsed, true);
        }

        private void FfmpegAudioConverter(ref string fileName, string fullPath, string wavFolder, TimeSpan startTime)
        {
            string targetExt = ".wav";
            if (!fileName.EndsWith(targetExt, StringComparison.OrdinalIgnoreCase))
            {
                string wavFileName = $"{startTime.ToString(@"dd\.hhmmss")}{targetExt}";
                string wavPath = Path.Combine(wavFolder, wavFileName);
                if (File.Exists(wavPath)) File.Delete(wavPath);

                string args = $"-i \"{fullPath}\" -acodec pcm_s16le -ar 22050 \"{wavPath}\"";
                ProcessHelper.RunProcessOutside("ffmpeg", args);
                if (File.Exists(wavPath))
                {
                    File.Delete(fullPath);
                    fileName = wavFileName;
                }
            }
        }

        private void WriteToTrainFile(IList<string> lines, string saveFolder)
        {
            var allPath = Path.Combine(saveFolder, "all.txt");
            var trainPath = Path.Combine(saveFolder, "train.txt");
            var valPath = Path.Combine(saveFolder, "val.txt");

            File.WriteAllText(allPath, string.Join(Environment.NewLine, lines), System.Text.Encoding.UTF8);

            var random = new Random();
            var shuffled = lines.OrderBy(_ => random.Next()).ToList();

            int valCount = Math.Max(1, shuffled.Count / 10);
            var valLines = shuffled.Take(valCount);
            var trainLinesOnly = shuffled.Skip(valCount);

            File.WriteAllText(trainPath, string.Join(Environment.NewLine, trainLinesOnly), System.Text.Encoding.UTF8);
            File.WriteAllText(valPath, string.Join(Environment.NewLine, valLines), System.Text.Encoding.UTF8);
        }

        private bool EndContentIsBreakLine(Audio audio)
        {
            string content = audio.Content;
            if (string.IsNullOrEmpty(content))
                return false;
            content = content.TrimEnd();
            if (string.IsNullOrEmpty(content))
                return false;
            if (content.EndsWith('.'))
                return true;
            foreach (var endText in TextHelper.NewLineText)
                if (content.EndsWith(endText))
                    return true;
            return false;
        }

        private static readonly HashSet<char> VietnameseSymbols = new HashSet<char>(
        "aăâbcdđeêghiklmnoôơpqrstuưvxy" +
        "áàảãạắằẳẵặấầẩẫậéèẻẽẹếềểễệ" +
        "íìỉĩịóòỏõọốồổỗộớờởỡợ" +
        "úùủũụứừửữựýỳỷỹỵ" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
        "0123456789 .,?!| "
        .ToCharArray()
        );

        public record AudioProcessingItem(TimeSpan Start, string FileName, byte[] Content, string ContentText);
    }
}
