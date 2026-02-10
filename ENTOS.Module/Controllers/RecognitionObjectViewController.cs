using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class RecognitionObjectViewController: BaseViewController<Module.BusinessObjects.RecognitionObject>
    {      
        
        public RecognitionObjectViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.RecognitionObject);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.RecognitionObjectService recognitionObjectService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             recognitionObjectService = new Module.Services.RecognitionObjectService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2536            Oid: 7b1ff433-ef9a-4f2f-877d-75a16b008319
		private void ObjectVideo_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ObjectVideo), "Tạo video");              
      
            #region ObjectVideoImportCode
            var recognitionObjects = View.SelectedObjects.Cast<RecognitionObject>().ToList();
            int frameWidth = 1280;
            int frameHeight = 720;
            int fps = 25;
            int index = 0;
            int audioIndex = 0;
            int silenceIndex = 0;

            string outputVideoPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                $"output_{DateTime.Now:yyyyMMddHHmmss}.mp4"
            );

            List<string> audioSegments = new();
            Dictionary<string, string> safeFilePaths = new();

            try
            {
                // Tạo VideoWriter để ghi video vào tệp tạm
                using var writer = new OpenCvSharp.VideoWriter(outputVideoPath, OpenCvSharp.FourCC.H264, fps, new OpenCvSharp.Size(frameWidth, frameHeight));

                foreach (var obj in recognitionObjects)
                {
                    foreach (var pos in obj.RecognitionPositionList)
                    {
                        string path = pos.Link;
                        if (!File.Exists(path))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"❌ Không tìm thấy file: {path}", InformationType.Error);
                            continue;
                        }

                        string safeFilePath = safeFilePaths.TryGetValue(path, out var existing) ? existing : recognitionObjectService.CreateTemporaryFile(path, ++index);
                        if (!safeFilePaths.ContainsKey(path))
                            safeFilePaths[path] = safeFilePath;

                        // 1) XỬ LÝ ẢNH
                        if (Services.RecognitionObjectService.IsImageFile(safeFilePath))
                        {
                            using var image = OpenCvSharp.Cv2.ImRead(safeFilePath);
                            if (image.Empty()) continue;

                            if (e.SelectedChoiceActionItem.Id.Equals("HasFrame"))
                                Services.RecognitionObjectService.DrawInfo(image, obj.Name, pos, 0);

                            // Hiển thị 1 giây (fps frame)
                            for (int i = 0; i < fps; i++)
                            {
                                var resized = Services.RecognitionObjectService.ResizeWithPadding(image, frameWidth, frameHeight);
                                writer.Write(resized);
                            }

                            // Thêm 1s im lặng
                            var silencePath = Services.RecognitionObjectService.GenerateSilence(1.0, silenceIndex++);
                            if (!string.IsNullOrEmpty(silencePath))
                                audioSegments.Add(silencePath);
                        }
                        // 2) XỬ LÝ VIDEO
                        else if (Services.RecognitionObjectService.IsVideoFile(safeFilePath))
                        {
                            using var cap = new OpenCvSharp.VideoCapture(safeFilePath);
                            if (!cap.IsOpened()) continue;

                            int begin = pos.BeginFrame ?? 0;
                            int end = pos.EndFrame ?? 0;
                            double duration = (end - begin + 1) / (double)fps;

                            // Ghi hình
                            cap.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, begin);
                            for (int i = begin; i <= end; i++)
                            {
                                using var frame = new OpenCvSharp.Mat();
                                if (!cap.Read(frame) || frame.Empty()) break;
                                if (e.SelectedChoiceActionItem.Id.Equals("HasFrame"))
                                    Services.RecognitionObjectService.DrawInfo(frame, obj.Name, pos, (i) / (float)fps);
                                var resized = Services.RecognitionObjectService.ResizeWithPadding(frame, frameWidth, frameHeight);
                                writer.Write(resized);
                            }

                            // Trích audio đúng độ dài
                            var audioPath = Services.RecognitionObjectService.ExtractAudioSegment(safeFilePath, begin, end, fps, audioIndex++);
                            if (!string.IsNullOrEmpty(audioPath))
                                audioSegments.Add(audioPath);
                        }
                    }
                }

                // Đóng writer
                writer.Release();

                // Gộp audio segments (bao gồm silence)
                if (audioSegments.Count > 0)
                {
                    string concatFile = Path.Combine(Path.GetTempPath(), $"concat_list_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    File.WriteAllLines(concatFile, audioSegments.Select(p => $"file '{p.Replace("'", "\\'")}'"));

                    string mergedAudio = Path.Combine(Path.GetTempPath(), $"merged_audio_{DateTime.Now:yyyyMMddHHmmss}.m4a");
                    Services.RecognitionObjectService.ConcatenateAudioFiles(concatFile, mergedAudio);
                    recognitionObjectService.MergeAudioToVideo(outputVideoPath, mergedAudio);

                    File.Delete(concatFile);
                    File.Delete(mergedAudio);
                    audioSegments.ForEach(File.Delete);
                }

                // Dùng FFmpeg để điều chỉnh bitrate ngay từ khi ghi video (không phải nén lại)
                string finalOutputPath = Path.Combine(Path.GetDirectoryName(outputVideoPath), $"final_{Path.GetFileName(outputVideoPath)}");
                var (output, error, exitCode) = Services.RecognitionObjectService.RunFFmpegCommand($"-i \"{outputVideoPath}\" -b:v 2000k -vcodec libx264 -acodec aac \"{finalOutputPath}\"");

                if (exitCode != 0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"❌ Lỗi khi điều chỉnh bitrate: {error}", InformationType.Error);
                    return;
                }

                // Xóa video tạm sau khi điều chỉnh bitrate
                File.Delete(outputVideoPath);

                safeFilePaths.Values.ToList().ForEach(recognitionObjectService.DeleteTemporaryFile);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{finalOutputPath}\"");
            }
            catch (Exception ex)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi tạo video: {ex.Message}", InformationType.Error);
            }




            #endregion ObjectVideoImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2807            Oid: a9ad000d-c19c-4af3-a46f-a245cb56d2b2
		private void ObjectAvatar_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ObjectAvatar), "Phóng ảnh");              
      
            #region ObjectAvatarImportCode
            foreach (var recognitionObject in View.SelectedObjects.OfType<RecognitionObject>())
            {
                var position = recognitionObject?.RecognitionPosition;
                if (position == null) continue;

                var originalFilePath = Path.GetFullPath(position.Link);
                if (!File.Exists(originalFilePath)) continue;

                var avatarBytes = Module.Services.RecognitionObjectService.GenerateAvatar(originalFilePath,
                    position.Horizontal ?? 0,
                    position.Vertical ?? 0,
                    position.Size ?? 0,
                    position.ImageFrame ?? 0);

                if (avatarBytes != null)
                    recognitionObject.Image = avatarBytes;
            }
            #endregion ObjectAvatarImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}