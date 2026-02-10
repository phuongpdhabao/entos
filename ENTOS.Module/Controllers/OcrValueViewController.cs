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
    public partial class OcrValueViewController: BaseViewController<Module.BusinessObjects.OcrValue>
    {      
        
        public OcrValueViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.OcrValue);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.OcrValueService ocrValueService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             ocrValueService = new Module.Services.OcrValueService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3747            Oid: e82db362-a587-41dd-b515-d7d5d10feacf
		private void ViewValue_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ViewValue), "Xem giá trị");              
      
            #region ViewValueImportCode
            var ocrValue = View.CurrentObject as Module.BusinessObjects.OcrValue;

            if (ocrValue == null || ocrValue.OcrPage == null || string.IsNullOrEmpty(ocrValue.OcrPage.PageLink))
                return;

            if (!File.Exists(ocrValue.OcrPage.PageLink))
                return;

            // Tạo bitmap từ ảnh gốc
            using (var original = new System.Drawing.Bitmap(ocrValue.OcrPage.PageLink))
            using (var bmp = new System.Drawing.Bitmap(original))
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            using (var pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3))
            {
                // Vẽ bbox
                var rect = new System.Drawing.RectangleF(
                    (float)ocrValue.X,
                    (float)ocrValue.Y,
                    (float)ocrValue.Width,
                    (float)ocrValue.Height
                );
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

                // Tạo file tạm
                string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
                bmp.Save(tempFile);
                
                
                try
                {
                    // Mở bằng app mặc định
                    using (var proc = new System.Diagnostics.Process())
                    {
                        proc.StartInfo.FileName = tempFile;
                        proc.StartInfo.UseShellExecute = true;
                        proc.Start();

                        Thread.Sleep(500);

                        // Chờ user đóng ảnh
                        proc.WaitForExit();
                    }
                }
                finally
                {
                    // Xóa file tạm
                    if (File.Exists(tempFile))
                    {
                        try { File.Delete(tempFile); } catch { /* ignore */ }
                    }
                }
            }

            #endregion ViewValueImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3748            Oid: f6376a9a-236d-43e0-a7b4-46a2dfb7a5a4
		private void ValidationCheck_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ValidationCheck), "Kiểm tra hơp lệ");              
      
            #region ValidationCheckImportCode
            foreach (Module.BusinessObjects.OcrValue ocrValue in View.SelectedObjects)
            {
                if(ocrValue.ExtractionKey is null)
                    continue;
                ocrValue.Invalid = !Module.Services.OcrValueService.ValidateOcrValue(ocrValue);
            }

            #endregion ValidationCheckImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}