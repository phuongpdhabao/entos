using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Module.Utils
{
    public static class PythonUtils
    {

        public static string PythonConvertPdfToDocx(IObjectSpace objectSpace, XafApplication application, string url, ref string pythonDir, string saveFolder = null)
        {
            if (!System.IO.File.Exists(url))
            {
                return null;
            }
            if (string.IsNullOrEmpty(pythonDir))
                pythonDir = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "PythonDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\Python312\\python312.dll");
            var pythonInfo = new System.IO.FileInfo(pythonDir);
            var pdf2docxDir = $"{pythonInfo.Directory.FullName}\\Scripts\\pdf2docx.exe";
            if (!System.IO.File.Exists(pdf2docxDir))
            {
                return null;
            }

            var urlInfo = new System.IO.FileInfo(url);
            var outputUrl = Module.Helpers.FileSystemHelper.GetUniqueFileName(Module.Helpers.FileSystemHelper.ReplaceExtension(string.IsNullOrEmpty(saveFolder) ? url : (saveFolder + "\\" + urlInfo.Name), ".docx"));
            string arguments = $" convert \"{url}\" \"{outputUrl}\" --multi_processing=True";
            Module.Helpers.ProcessHelper.RunProcessOutside(pdf2docxDir, arguments, 0, true);
            if (System.IO.File.Exists(outputUrl))
                return outputUrl;
            return null;
        }

        public static string PythonWhisperTranscriptionToSrt(IObjectSpace objectSpace, XafApplication application, string url, ref string pythonDir, ref string whisperModelDir)
        {
            if (!System.IO.File.Exists(url))
            {
                return null;
            }
            if (string.IsNullOrEmpty(whisperModelDir))
                whisperModelDir = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "PythonWhisperModelDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\whisper\\base.pt", SecuritySystem.CurrentUserId).Value;

            if (string.IsNullOrEmpty(pythonDir))
                pythonDir = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "PythonDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\Python312\\python312.dll");
            var pythonInfo = new System.IO.FileInfo(pythonDir);
            var whisperDir = $"{pythonInfo.Directory.FullName}\\Scripts\\whisper.exe";
            if (!System.IO.File.Exists(whisperDir))
            {
                return null;
            }
            var urlInfo = new System.IO.FileInfo(url);
            string arguments = $" \"{url}\" --model {whisperModelDir} --word_timestamps True --output_dir \"{urlInfo.Directory.FullName}\"";
            Module.Helpers.ProcessHelper.RunProcessOutside(whisperDir, arguments, 0, true);

            var srtFile = Module.Helpers.FileSystemHelper.ReplaceExtension(url, ".srt");
            if (System.IO.File.Exists(srtFile))
                return System.IO.File.ReadAllText(srtFile);
            var inputInfo = new System.IO.FileInfo(url);

            srtFile = Module.Helpers.FileSystemHelper.ReplaceExtension(System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name, ".srt");
            if (System.IO.File.Exists(srtFile))
                return System.IO.File.ReadAllText(srtFile);
            return null;
        }

        /// <summary>
        /// Chuyển đổi audio thành text sử dụng Python Whisper và trả về danh sách từng từ.
        /// Sử dụng thư viện Python Whisper để nhận dạng giọng nói với độ chính xác cao.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thao tác database</param>
        /// <param name="application">XafApplication instance</param>
        /// <param name="url">Đường dẫn đến file audio</param>
        /// <param name="pythonDir">Thư mục chứa Python (ref để cập nhật)</param>
        /// <param name="whisperModelDir">Thư mục chứa Whisper model (ref để cập nhật)</param>
        /// <returns>Chuỗi text được nhận dạng từ audio</returns>
        /// <example>
        /// string pythonDir = @"C:\Python39";
        /// string whisperModelDir = @"C:\whisper-models";
        /// string text = Tools.PythonWhisperTranscriptionToWords(objectSpace, application, 
        ///     "audio.mp3", ref pythonDir, ref whisperModelDir);
        /// </example>
        public static string PythonWhisperTranscriptionToWords(IObjectSpace objectSpace, XafApplication application, string url, ref string pythonDir, ref string whisperModelDir)
        {
            if (!System.IO.File.Exists(url))
            {
                return null;
            }
            if (string.IsNullOrEmpty(whisperModelDir))
                whisperModelDir = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "PythonWhisperModelDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\whisper\\base.pt", SecuritySystem.CurrentUserId).Value;

            if (string.IsNullOrEmpty(pythonDir))
                pythonDir = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "PythonDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\Python312\\python312.dll");
            var pythonInfo = new System.IO.FileInfo(pythonDir);
            var whisperDir = $"{pythonInfo.Directory.FullName}\\Scripts\\whisper_timestamped.exe";
            if (!System.IO.File.Exists(whisperDir))
            {
                return null;
            }
            var urlInfo = new System.IO.FileInfo(url);
            string arguments = $" \"{url}\" --model {whisperModelDir} --output_dir \"{urlInfo.Directory.FullName}\"";
            Module.Helpers.ProcessHelper.RunProcessOutside(whisperDir, arguments, 0, true);
            //process.WaitForExit();                                   

            var srtFile = url + ".words.json";
            if (System.IO.File.Exists(srtFile))
                return System.IO.File.ReadAllText(srtFile);
            var inputInfo = new System.IO.FileInfo(url);
            srtFile = System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name + ".words.json";
            if (System.IO.File.Exists(srtFile))
                return System.IO.File.ReadAllText(srtFile);
            return null;
        }

        //Hỗ trợ chạy nhiều file cùng thời điểm
        public static string[] PythonWhisperTranscriptionToWords(IObjectSpace objectSpace, XafApplication application, string[] urls, ref string pythonDir, ref string whisperModelDir)
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var url in urls)
            {
                if (System.IO.File.Exists(url))
                {
                    if (!first)
                        sb.Append("\" \"");
                    sb.Append(url);
                    first = false;
                }
            }
            string inputFiles = sb.ToString();
            if (string.IsNullOrEmpty(whisperModelDir))
                whisperModelDir = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "PythonWhisperModelDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\whisper\\base.pt", SecuritySystem.CurrentUserId).Value;

            if (string.IsNullOrEmpty(pythonDir))
                pythonDir = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "PythonDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\Python312\\python312.dll");
            var pythonInfo = new System.IO.FileInfo(pythonDir);
            var whisperDir = $"{pythonInfo.Directory.FullName}\\Scripts\\whisper_timestamped.exe";
            if (!System.IO.File.Exists(whisperDir) || string.IsNullOrEmpty(inputFiles))
            {
                return null;
            }
            var urlInfo = new System.IO.FileInfo(urls[0]);
            if (!System.IO.Directory.Exists(urlInfo.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(urlInfo.Directory.FullName);
            }
            var whisperModelDirInfo = new System.IO.FileInfo(whisperModelDir);
            string arguments = $" \"{inputFiles}\" --model {whisperModelDir} --model_dir \"{whisperModelDirInfo.FullName}\" --output_dir \"{urlInfo.Directory.FullName}\"";
            Module.Helpers.ProcessHelper.RunProcessOutside(whisperDir, arguments, 0, true);
            //process.WaitForExit();                                   
            var results = new string[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                var srtFile = urls[i] + ".words.json";
                if (System.IO.File.Exists(srtFile))
                    results[i] = System.IO.File.ReadAllText(srtFile);
                else
                {
                    var inputInfo = new System.IO.FileInfo(urls[i]);
                    srtFile = System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name + ".words.json";
                    if (System.IO.File.Exists(srtFile))
                        results[i] = System.IO.File.ReadAllText(srtFile);
                    srtFile = urlInfo.Directory.FullName + "\\" + inputInfo.Name + ".words.json";
                    if (System.IO.File.Exists(srtFile))
                        results[i] = System.IO.File.ReadAllText(srtFile);
                }
            }
            //var srtFile = url + ".words.json";
            //if (System.IO.File.Exists(srtFile))
            //    return System.IO.File.ReadAllText(srtFile);
            //var inputInfo = new System.IO.FileInfo(url);
            //srtFile = System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name + ".words.json";
            //if (System.IO.File.Exists(srtFile))
            //    return System.IO.File.ReadAllText(srtFile);
            return results;
        }

        public static Python.Runtime.PyDict[] PythonWhisperTranscription(IObjectSpace objectSpace, XafApplication application, string url, ref string pythonDir, ref string whisperModelDir)
        {
            if (string.IsNullOrEmpty(whisperModelDir))
                whisperModelDir = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "PythonWhisperModelDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\whisper\\base.pt").Value;

            if (string.IsNullOrEmpty(Python.Runtime.Runtime.PythonDLL))
            {
                if (string.IsNullOrEmpty(pythonDir))
                    pythonDir = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "PythonDir", "\\\\dc\\Habao$\\Company\\HabaoAI\\Python312\\python312.dll");
                Python.Runtime.Runtime.PythonDLL = pythonDir; // Set the Python DLL path
                Python.Runtime.PythonEngine.Initialize(); // Initialize the Python engine
            }
            using (Python.Runtime.Py.GIL()) // Acquire the Python Global Interpreter Lock (GIL)
            {
                var whisper = Python.Runtime.Py.Import("whisper");
                // Import the Python script            
                var model = whisper.InvokeMethod("load_model", new Python.Runtime.PyString(whisperModelDir));
                var sr_result = model.InvokeMethod("transcribe", new Python.Runtime.PyString(url));
                dynamic segments = sr_result.GetItem("segments");
                return (Python.Runtime.PyDict[])segments;
                //foreach (var segment in (PyDict[])segments)
                //{
                //    var id = segment.GetItem("id").ToInt32(new System.Globalization.CultureInfo("en"));
                //    var start = segment.GetItem("start").ToSingle(new System.Globalization.CultureInfo("en"));
                //    var end = segment.GetItem("end").ToSingle(new System.Globalization.CultureInfo("en"));
                //    var text = segment.GetItem("text").ToString();                    
                //}

            }
            return null;
        }
    }
}
