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
    public partial class RecognitionPositionViewController: BaseViewController<Module.BusinessObjects.RecognitionPosition>
    {      
        
        public RecognitionPositionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.RecognitionPosition);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.RecognitionPositionService recognitionPositionService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             recognitionPositionService = new Module.Services.RecognitionPositionService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2790            Oid: 401810f9-0069-4985-9056-1d6a6abe38eb
		private void ObjectView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ObjectView), "Xem đối tượng");              
      
            #region ObjectViewImportCode

            Module.BusinessObjects.RecognitionPosition obj = this.View.CurrentObject as Module.BusinessObjects.RecognitionPosition;
            if (obj == null) return;

            string sourcePath = obj.Link;

            // Đảm bảo xử lý UNC path với \\?\
            if (string.IsNullOrWhiteSpace(sourcePath) || !System.IO.File.Exists(sourcePath)) return;

            // Xử lý nếu là UNC path
            string uncPath = sourcePath.StartsWith(@"\\") ? @"\\?\" + sourcePath : sourcePath;

            // Tạo đường dẫn file tạm với tên file Unicode
            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString() + System.IO.Path.GetExtension(uncPath));
            System.IO.File.Copy(uncPath, tempFile, true); // copy để mở an toàn

            try
            {
                if (Services.RecognitionPositionService.IsImage(tempFile))
                {
                    OpenCvSharp.Mat image = OpenCvSharp.Cv2.ImRead(tempFile, OpenCvSharp.ImreadModes.Color); // hỗ trợ Unicode
                    if (!image.Empty())
                    {
                        Services.RecognitionPositionService.DrawBox(image, obj.Horizontal.GetValueOrDefault(), obj.Vertical.GetValueOrDefault(), obj.Size.GetValueOrDefault());
                        OpenCvSharp.Cv2.ImShow("Xem ảnh", image);
                        OpenCvSharp.Cv2.WaitKey();
                        OpenCvSharp.Cv2.DestroyAllWindows();
                    }
                    image.Dispose();
                }
                else if (Services.RecognitionPositionService.IsVideo(tempFile))
                {
                    OpenCvSharp.VideoCapture capture = new OpenCvSharp.VideoCapture(tempFile);
                    if (capture.IsOpened())
                    {
                        // Đặt frame theo chỉ số Unicode-safe
                        capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, (double)obj.ImageFrame.GetValueOrDefault());

                        OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
                        if (capture.Read(frame) && !frame.Empty())
                        {
                            Services.RecognitionPositionService.DrawBox(frame, obj.Horizontal.GetValueOrDefault(), obj.Vertical.GetValueOrDefault(), obj.Size.GetValueOrDefault());
                            OpenCvSharp.Cv2.ImShow("Khung video", frame);
                            OpenCvSharp.Cv2.WaitKey();
                            OpenCvSharp.Cv2.DestroyAllWindows();
                        }

                        frame.Dispose();
                    }
                    capture.Release();
                    capture.Dispose();
                }
            }
            finally
            {
                try
                {
                    // Xoá file tạm có Unicode name
                    if (System.IO.File.Exists(tempFile))
                    {
                        System.IO.File.Delete(tempFile);
                    }
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Không thể xóa file tạm: " + ex.Message);
                }
            }
        

            #endregion ObjectViewImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}