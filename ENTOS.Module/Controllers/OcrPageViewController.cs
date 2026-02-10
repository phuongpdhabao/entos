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
    public partial class OcrPageViewController: BaseViewController<Module.BusinessObjects.OcrPage>
    {      
        
        public OcrPageViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.OcrPage);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.OcrPageService ocrPageService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             ocrPageService = new Module.Services.OcrPageService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3751            Oid: a6d6d2d7-b4cf-4bcc-a23a-13e81310f0b1
		private void OcrPageImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrPageImport), "Nạp trang");              
      
            #region OcrPageImportImportCode
            var doc = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.OcrDocument;
            if (doc == null) return;

            string link = doc.DocumentLink;

            if (Directory.Exists(link))
            {
                // Trường hợp thư mục → nạp tất cả ảnh
                var images = Directory.GetFiles(link, "*.*", SearchOption.TopDirectoryOnly)
                                      .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                  f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                  f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                  f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                  f.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                                                  f.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase))
                                      .OrderBy(f => f)
                                      .ToList();

                foreach (var img in images)
                    ocrPageService.AddPage(doc, img);
            }
            else if (File.Exists(link))
            {
                var ext = Path.GetExtension(link).ToLower();
                if (ext == ".pdf")
                {
                    // Thư mục lưu ảnh: cùng thư mục PDF + tên file không đuôi
                    string pdfDir = Path.GetDirectoryName(link)!;
                    string pdfName = Path.GetFileNameWithoutExtension(link);
                    string outputDir = Path.Combine(pdfDir, pdfName + "PDF");

                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    using (var pdfDoc = PdfiumViewer.PdfDocument.Load(link))
                    {

                        for (int pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
                        {
                            using (var image = pdfDoc.Render(
                                pageIndex,
                                600, 600,
                                PdfiumViewer.PdfRenderFlags.ForPrinting))
                            {
                                string tempFile = Path.Combine(
                                    outputDir,
                                    $"ocrpage_{Guid.NewGuid()}_{pageIndex + 1}.png");

                                image.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png);
                                ocrPageService.AddPage(doc, tempFile);
                            }
                        }

                    }
                }
                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" ||
                         ext == ".bmp" || ext == ".tif" || ext == ".tiff")
                {
                    // Trường hợp ảnh đơn
                    ocrPageService.AddPage(doc, link);
                }
            }
            else
            {
                var files = userInteractionService.SelectFiles("All files (*.*)|*.*", true, "Chọn file ảnh hoặc PDF");
                if (files == null)
                    return;

                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        var ext = Path.GetExtension(file).ToLower();
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" ||
                                 ext == ".bmp" || ext == ".tif" || ext == ".tiff")
                        {
                            // Nạp ảnh đơn
                            ocrPageService.AddPage(doc, file);
                        }
                        else if (ext == ".pdf")
                        {
                            string pdfDir = Path.GetDirectoryName(file)!;
                            string pdfName = Path.GetFileNameWithoutExtension(file);
                            string outputDir = Path.Combine(pdfDir, pdfName + "PDF");

                            if (!Directory.Exists(outputDir))
                                Directory.CreateDirectory(outputDir);

                            using (var pdfDoc = PdfiumViewer.PdfDocument.Load(file))
                            {

                                for (int pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
                                {
                                    using (var image = pdfDoc.Render(
                                        pageIndex,
                                        600, 600,
                                        PdfiumViewer.PdfRenderFlags.ForPrinting))
                                    {
                                        string tempFile = Path.Combine(
                                            outputDir,
                                            $"ocrpage_{pageIndex + 1}.png");

                                        image.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png);
                                        ocrPageService.AddPage(doc, tempFile);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        


            #endregion OcrPageImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3723            Oid: cf4f3062-84c6-4a2a-bf09-ee3a2fecc4ec
		private void OcrPageStructure_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrPageStructure), "Nhận dạng cấu trúc");              
      
            #region OcrPageStructureImportCode
            var dataServiceService = new Module.Services.DataServiceService();

            var dataService = dataServiceService.GetDataService(this, "StructureOcr");
            var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);

            foreach (Module.BusinessObjects.OcrPage item in View.SelectedObjects.Cast<Module.BusinessObjects.OcrPage>())
            {
                if (item != null)
                {
                    var fileBytes = System.IO.File.ReadAllBytes(item.PageLink);

                    var result = Task.Run(() => Module.Services.SoftwareServiceTypeService.StructureOcrService(dataServiceDto, fileBytes)).GetAwaiter().GetResult();
                    if (result != null)
                    {
                        item.OcrJson = result.Json;
                        item.OcrMarkdown = result.Markdown;
                    }
                }
                else
                {
                    Module.Helpers.LogHelper.Warn($"Không tìm thấy cấu trúc OCR cho trang: {item.Oid}");
                }
            }

            #endregion OcrPageStructureImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3724            Oid: bde56dd8-0483-457d-8eab-15e8f451e36a
		private void OcrPageExtract_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrPageExtract), "Trích thông tin");              
      
            #region OcrPageExtractImportCode
            var dataServiceService = new Module.Services.DataServiceService();

            var dataService = dataServiceService.GetDataService(this, "KIE");
            if (dataService == null)
            {
                Application.ShowViewStrategy.ShowMessage("Vui lòng chọn service.", InformationType.Warning);
                return;
            }

            var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);

            foreach (Module.BusinessObjects.OcrPage ocrPage in View.SelectedObjects)
            {
                string jsonContent = ocrPage.OcrJson;

                if (jsonContent == null)
                    continue;

                var keyList = ocrPage.ExtractionTemplate?.ExtractionKeyList;
                var headerKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Header).Select(k => k.Name).ToArray() ?? new string[] { };
                var tableKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Table).Select(k => k.Name).ToArray() ?? new string[] { };
                var footerKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Footer).Select(k => k.Name).ToArray() ?? new string[] { };
                var bodyKeyList = /*keyList?.Where(k => k.DataLayout == DataLayout.Body).Select(k => k.Name).ToArray() ??*/ new string[] { };

                try
                {
                    // Nếu trống thì gán "{}"
                    string rawJson = string.IsNullOrEmpty(jsonContent) ? "{}" : jsonContent;

                    // Tạo file tạm
                    string tempFile = Path.GetTempFileName();
                    string fileJson = Path.ChangeExtension(tempFile, ".json"); // đổi sang .json cho rõ ràng
                    File.WriteAllText(fileJson, rawJson, System.Text.Encoding.UTF8);

                    // Gọi service
                    var kieResults = Task.Run(() => Module.Services.SoftwareServiceTypeService.GetKIEResult(dataServiceDto, fileJson, headerKeyList, footerKeyList, tableKeyList, bodyKeyList)).GetAwaiter().GetResult();
                    if (kieResults == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Không có kết quả nào từ KieService.", InformationType.Warning);
                        return;
                    }

                    using var doc = System.Text.Json.JsonDocument.Parse(kieResults);
                    var root = doc.RootElement;

                    ocrPage.ValueMarkdown = root.GetProperty("MarkdownKIE").GetString();

                    var jsonKIE = root.GetProperty("JsonKIE").GetRawText();
                    var ocrValueResult = Module.Services.SoftwareServiceTypeService.KieService(jsonKIE);

                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    // chuyển list -> string
                    foreach (var kr in ocrValueResult)
                    {
                        var ocrValue = ObjectSpace.CreateObject<OcrValue>();
                        ocrValue.Name = kr.Name.Split('.').Last();
                        if (!string.IsNullOrEmpty(ocrValue.Name))
                        {
                            Module.BusinessObjects.ExtractionKey extractionKey = ocrPage.ExtractionTemplate.ExtractionKeyList.FirstOrDefault(k => k.Name == ocrValue.Name);
                            if (extractionKey != null)
                            {
                                ocrValue.ExtractionKey = extractionKey;
                            }
                        }
                        ocrValue.Value = kr.Value;
                        ocrValue.Confidence = kr.Confidence;
                        ocrValue.Height = kr.Height;
                        ocrValue.Width = kr.Width;
                        ocrValue.X = kr.X;
                        ocrValue.Y = kr.Y;
                        ocrValue.OcrPage = ocrPage;
                        ocrValue.OcrDocument = ocrPage.OcrDocument;
                    }

                    Application.ShowViewStrategy.ShowMessage("Đã tạo OcrValue từ KieService thành công.", InformationType.Success);
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($"Lỗi: {ex.Message}", InformationType.Error);
                }
            }




            #endregion OcrPageExtractImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3735            Oid: 53940bc2-e80c-47b4-af6d-ee0b5ca35b85
		private void OcrPageObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrPageObject), "Đối tượng");              
      
            #region OcrPageObjectImportCode
            var page = View.CurrentObject as Module.BusinessObjects.OcrPage;
            var doc = page?.OcrDocument;

            List<object> createObject = new();

            if (doc.MultiPage)
            {
                Application.ShowViewStrategy.ShowMessage("Trang thuộc tài liệu nhiều trang. Vui lòng sử dụng chức năng 'tạo đối tượng' trên 'tài liệu nhận dạng'.", InformationType.Warning);
                return;
            }
            var objectSpace = Application.CreateObjectSpace();
            var cs = new CollectionSource(objectSpace, page.ExtractionTemplate.SystemType);

            if (e.SelectedChoiceActionItem.Id.Equals("Create"))
            {
                string firstCode = string.Empty;

                foreach (Module.BusinessObjects.OcrPage page1 in View.SelectedObjects)
                {
                    var existed = objectSpace.FindObject(page.ExtractionTemplate.SystemType, DevExpress.Data.Filtering.CriteriaOperator.Parse("OcrID = ?", page1.Oid)); 
                    if (existed != null)
                    {
                        Application.ShowViewStrategy.ShowMessage($"Trang {page1.Oid} đã được tạo đối tượng.", InformationType.Warning);
                        continue;
                    }

                    var obj = ocrPageService.CreateOcrPageObject(page1);
                    var prop = obj.GetType().GetProperty("OcrID");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(obj, page1.Oid);
                    }

                    var prop2 = obj.GetType().GetProperty("Code");
                    if (prop2 != null && prop2.CanWrite)
                    {
                        var firstSelected = View.SelectedObjects.Cast<object>().FirstOrDefault();
                        if (page1 == firstSelected)
                        {
                            Module.Helpers.ReflectionHelper.InvokeMethod(obj, "SetDefaultCode", new object[] { null });
                            firstCode = prop2.GetValue(obj) as string ?? string.Empty;
                        }
                        else
                            prop2.SetValue(obj, Module.Helpers.TextHelper.GetNextObjectCode(firstCode));

                    }


                    createObject.Add(obj);
                }
            }

            if (View.SelectedObjects.Count > 1)
            {
                var guids = View.SelectedObjects.Cast<OcrPage>().Select(p => p.Oid).ToList();
                cs.Criteria["FilterByOcrID"] = new DevExpress.Data.Filtering.InOperator("OcrID", guids);
            }
            else
                cs.Criteria["FilterByOcrID"] = DevExpress.Data.Filtering.CriteriaOperator.Parse("OcrID = ?", page.Oid);

            foreach (var obj in createObject) cs.Add(obj);
            

            if (View.SelectedObjects.Count > 1)
            {
                // Tạo Id đúng theo quy tắc
                if (cs.List.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage("Không tìm thấy đối tượng nào để hiển thị.", InformationType.Warning);
                    return;
                }
                var systemTypeName = page.ExtractionTemplate.SystemType.Name;
                var objectCode = systemTypeName.StartsWith("Ocr")
                    ? systemTypeName.Substring(3)   // cắt "Ocr" ở đầu
                    : systemTypeName;

                var listViewId = $"Ocr{objectCode}_ListView"; var listView = Application.CreateListView(listViewId, cs, true);
                Application.ShowViewStrategy.ShowView(new ShowViewParameters(listView) { CreateAllControllers = true, TargetWindow = TargetWindow.NewModalWindow}, new ShowViewSource(Frame, null));
            }
            else
            {
                if (cs.List.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage("Không tìm thấy đối tượng nào để hiển thị.", InformationType.Warning);
                    return;
                }
                var detailObject = cs.List[0];
                var detailView = Application.CreateDetailView(objectSpace, detailObject);
                Application.ShowViewStrategy.ShowView(new ShowViewParameters(detailView) { CreateAllControllers = true }, new ShowViewSource(Frame, null));
            }





            #endregion OcrPageObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3856            Oid: 13025363-8bb2-4137-91ad-faeda034bfec
		private void OcrPageMarkdown_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrPageMarkdown), "Hoán đổi markdown hiển thị");              
      
            #region OcrPageMarkdownImportCode
            var ocrPage = View.CurrentObject as Module.BusinessObjects.OcrPage;
            if (ocrPage is null)
            {
                logger.LogWarning("Không có tài liệu nào để xử lý.");
                return;
            }

            if (ocrPage.Markdown is null) {
                ocrPage.Markdown = ocrPage.OcrMarkdown;
            }
            else if (ocrPage.Markdown == ocrPage.OcrMarkdown)
            {
                ocrPage.Markdown = ocrPage.ValueMarkdown;
            }
            else
            {
                ocrPage.Markdown = ocrPage.OcrMarkdown;
            }




            #endregion OcrPageMarkdownImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}