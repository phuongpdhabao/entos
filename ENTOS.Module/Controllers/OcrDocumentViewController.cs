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
    public partial class OcrDocumentViewController: BaseViewController<Module.BusinessObjects.OcrDocument>
    {      
        
        public OcrDocumentViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.OcrDocument);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
     
        private OcrPageService ocrPageService;
        protected OcrPageService _ocrPageService => ocrPageService ??= new OcrPageService(this);        
      
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3725            Oid: 321bc01e-29d6-4111-8dc4-a66127bb682a
		private void OcrDocumentStructure_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrDocumentStructure), "Nhận dạng cấu trúc");              
      
            #region OcrDocumentStructureImportCode
            foreach (Module.BusinessObjects.OcrDocument ocrDocument in View.SelectedObjects)
            {
                if (ocrDocument is null || ocrDocument.OcrPageList.Count == 0)
                {
                    logger.LogWarning("Không có tài liệu hoặc trang nào để xử lý.");
                    notificationService.NotifyWarning("Nhận dạng cấu trúc", "Không có tài liệu hoặc trang nào để xử lý.");
                    return;
                }

                var dataServiceService = new Module.Services.DataServiceService();
                var dataService = dataServiceService.GetDataService(this, "StructureOcr");
                if (dataService == null)
                {
                    Application.ShowViewStrategy.ShowMessage("Vui lòng chọn service.", InformationType.Warning);
                    return;
                }
                var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);
                if (dataServiceDto is null)
                {
                    logger.LogError("Dịch vụ dữ liệu 'StructureOcr' không được cấu hình đúng.");
                    notificationService.NotifyError("Nhận dạng cấu trúc", "Dịch vụ dữ liệu 'StructureOcr' không được cấu hình đúng.");
                    return;
                }

                try
                {
                    foreach (var page in ocrDocument.OcrPageList)
                    {
                        if (!string.IsNullOrEmpty(page.OcrJson) || !string.IsNullOrEmpty(page.OcrMarkdown))
                            continue;
                        if (!string.IsNullOrWhiteSpace(page.PageLink))
                        {
                            var fileBytes = System.IO.File.ReadAllBytes(page.PageLink);

                            var result = Task.Run(() => Module.Services.SoftwareServiceTypeService.StructureOcrService(dataServiceDto, fileBytes)).GetAwaiter().GetResult();
                            if (result != null)
                            {
                                page.OcrJson = result.Json;
                                page.OcrMarkdown = result.Markdown;
                            }
                        }
                    }

                    if (ocrDocument.MultiPage && ocrDocument.OcrPageList.Count > 1)
                    {
                        var ocrPageService = new Module.Services.OcrPageService();
                        var mergedPageList = ocrDocument.OcrPageList.OrderBy(x => x.Order).ToList();
                                
                        //json
                        string mergedJson = string.Empty;
                        foreach (var page in mergedPageList)
                        {
                            if (string.IsNullOrWhiteSpace(page.OcrJson))
                                continue;
                            else
                            {
                                // Giả sử page.OcrJson là JSON string dạng { "field1": ..., "field2": ... }
                                mergedJson += "{ \"pageIndex\": " + page.Order + ", " + page.OcrJson.Substring(1);
                                if ( page != mergedPageList.Last())
                                    mergedJson += ",";
                            }

                        }
                        if (!string.IsNullOrWhiteSpace(mergedJson))
                        {
                            ocrDocument.OcrJson = "[" + mergedJson + "]"; 
                        }                        //markdown
                        string mergedMarkdown = ocrPageService.MarkdownMerging(mergedPageList);
                        if (!string.IsNullOrWhiteSpace(mergedMarkdown))
                        {
                            ocrDocument.OcrMarkdown = mergedMarkdown;
                        }
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError("Lỗi trong quá trình nhận dạng cấu trúc.", ex);
                    notificationService.NotifyError("Nhận dạng cấu trúc", $"Lỗi trong quá trình nhận dạng cấu trúc: {ex.Message}");
                }
            }
            #endregion OcrDocumentStructureImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3726            Oid: 3199b993-7136-4919-a338-42fd408c535a
		private void OcrDocumentExtract_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrDocumentExtract), "Trích thông tin");              
      
            #region OcrDocumentExtractImportCode
            var OcrDocuments = View.CurrentObject as Module.BusinessObjects.OcrDocument;
            if (OcrDocuments is null || OcrDocuments.OcrPageList.Count == 0)
            {
                logger.LogWarning("Không có tài liệu hoặc trang nào để xử lý.");
                notificationService.NotifyWarning("Trích thông tin", "Không có tài liệu hoặc trang nào để xử lý.");
                return;
            }

            var dataServiceService = new Module.Services.DataServiceService();
            var dataService = dataServiceService.GetDataService(this, "KIE");
            if (dataService == null)
            {
                Application.ShowViewStrategy.ShowMessage("Vui lòng chọn service.", InformationType.Warning);
                return;
            }

            var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(dataService);

            foreach (Module.BusinessObjects.OcrDocument ocrDocument in View.SelectedObjects)
            {

                if (ocrDocument.MultiPage)
                {
                    string jsonContent = ocrDocument.OcrJson;

                    if (jsonContent == null)
                        continue;

                    var keyList = ocrDocument.ExtractionTemplate?.ExtractionKeyList;
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

                        ocrDocument.ValueMarkdown = root.GetProperty("MarkdownKIE").GetString();

                        // Nếu JsonKIE là object
                        var jsonKIE = root.GetProperty("JsonKIE").GetRawText();
                        var ocrValueResult = Module.Services.SoftwareServiceTypeService.KieService(jsonKIE);


                        if (kieResults == null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Không có kết quả nào từ KieService.", InformationType.Warning);
                            return;
                        }

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
                                Module.BusinessObjects.ExtractionKey extractionKey = ocrDocument.ExtractionTemplate.ExtractionKeyList.FirstOrDefault(k => k.Name == ocrValue.Name);
                                if ( extractionKey != null)
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
                            if (kr.PageIndex.HasValue)
                                ocrValue.OcrPage = ocrDocument.OcrPageList.FirstOrDefault(p => p.Order == kr.PageIndex.Value);
                            ocrValue.OcrDocument = ocrDocument;
                        }

                        Application.ShowViewStrategy.ShowMessage("Đã tạo OcrValue từ KieService thành công.", InformationType.Success);
                    }
                    catch (Exception ex)
                    {
                        Application.ShowViewStrategy.ShowMessage($"Lỗi: {ex.Message}", InformationType.Error);
                    }
                }
                else
                {
                    foreach (Module.BusinessObjects.OcrPage ocrPage in ocrDocument.OcrPageList)
                    {
                        string jsonContent = ocrPage.OcrJson;

                        if (jsonContent == null)
                            continue;

                        var keyList = ocrPage.ExtractionTemplate?.ExtractionKeyList;
                        var headerKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Header).Select(k => k.Name).ToArray() ?? new string[] { };
                        var tableKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Table).Select(k => k.Name).ToArray() ?? new string[] { };
                        var footerKeyList = keyList?.Where(k => k.DataLayout == DataLayout.Footer).Select(k => k.Name).ToArray() ?? new string[] { };
                        var bodyKeyList = /*keyList?.Where(k => k.DataLayout == DataLayout.Body).Select(k => k.Name).ToArray() ?? */new string[] { };

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
                                    Module.BusinessObjects.ExtractionKey extractionKey = ocrDocument.ExtractionTemplate.ExtractionKeyList.FirstOrDefault(k => k.Name == ocrValue.Name);
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
                                ocrValue.OcrDocument = ocrDocument;
                            }

                            Application.ShowViewStrategy.ShowMessage("Đã tạo OcrValue từ KieService thành công.", InformationType.Success);
                        }
                        catch (Exception ex)
                        {
                            Application.ShowViewStrategy.ShowMessage($"Lỗi: {ex.Message}", InformationType.Error);
                        }
                    }

                }
            }





            #endregion OcrDocumentExtractImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3736            Oid: 95bb16d8-d966-47f0-88d6-f46c4310632d
		private void OcrDocumentObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrDocumentObject), "Đối tượng");              
      
            #region OcrDocumentObjectImportCode
            var ocrDocument = View.CurrentObject as Module.BusinessObjects.OcrDocument;
            if (ocrDocument == null || ocrDocument.ExtractionTemplate == null)
                return;

            var objectSpace = Application.CreateObjectSpace();
            var systemType = ocrDocument.ExtractionTemplate.SystemType;
            var tableSystemType = ocrDocument.ExtractionTemplate.TableSystemType;

            if (systemType == null) return;

            // === Multipage ===
            if (ocrDocument.MultiPage == true)
            {
                var mainObject = objectSpace.FindObject(systemType, DevExpress.Data.Filtering.CriteriaOperator.Parse("OcrID = ?", ocrDocument.Oid));

                if (e.SelectedChoiceActionItem.Id.Equals("Create"))
                {
                    // --- Tạo đối tượng chính ---
                    if (mainObject != null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Đối tượng chính đã tồn tại, không thể tạo mới.", InformationType.Warning);
                        return;
                    }
                    else
                    {
                        mainObject = objectSpace.CreateObject(systemType);
                        var prop = mainObject.GetType().GetProperty("OcrID");
                        if (prop != null && prop.CanWrite)
                        {
                            prop.SetValue(mainObject, ocrDocument.Oid);
                        }

                        // --- Map field không thuộc Table ---
                        var typeInfo = XafTypesInfo.Instance.FindTypeInfo(systemType);

                        foreach (var val in ocrDocument.OcrValueList
                                                       .Where(v => v.ExtractionKey != null && v.ExtractionKey.DataLayout != DataLayout.Table))
                        {
                            var memberInfo = typeInfo.FindMember(val.ExtractionKey.Code);
                            if (memberInfo != null && !memberInfo.IsReadOnly && !memberInfo.IsKey && !memberInfo.IsAssociation)
                            {
                                object value = Module.Services.OcrValueService.CastValue(val.Value, val.ExtractionKey.DataType?.Name);
                                memberInfo.SetValue(mainObject, value);
                            }
                        }


                        // --- Xử lý Table ---
                        if (tableSystemType != null)
                        {
                            var mainTypeInfo = XafTypesInfo.Instance.FindTypeInfo(systemType);

                            // Tìm property list trỏ đến bảng (collection hoặc object đơn)
                            var tableMember = mainTypeInfo.Members
                                .FirstOrDefault(m =>
                                    m.MemberType == tableSystemType ||
                                    m.ListElementType == tableSystemType);

                            if (tableMember != null)
                            {
                                var tableValues = ocrDocument.OcrValueList
                                    .Where(v => v.ExtractionKey != null && v.ExtractionKey.DataLayout == DataLayout.Table)
                                    .ToList();

                                // Group theo Y (tolerance 5px) - tránh null
                                var rowGroups = tableValues
                                    .Where(v => v.Y.HasValue && v.OcrPage.Order.HasValue)
                                    .GroupBy(v => new
                                    {
                                        Page = v.OcrPage.Order.Value,
                                        Row = (int)(v.Y.Value / 5)
                                    })
                                    .OrderBy(g => g.Key.Page)
                                    .ThenBy(g => g.Key.Row);


                                foreach (var row in rowGroups)
                                {
                                    // Tạo mới row object
                                    var tableRowObj = objectSpace.CreateObject(tableSystemType);
                                    var tableTypeInfo = XafTypesInfo.Instance.FindTypeInfo(tableSystemType);

                                    foreach (var val in row)
                                    {
                                        var memberInfo = tableTypeInfo.FindMember(val.ExtractionKey.Code);
                                        if (memberInfo != null && !memberInfo.IsReadOnly)
                                        {
                                            string dataTypeName = val.ExtractionKey.DataType?.Name ?? "String";
                                            object value = Module.Services.OcrValueService.CastValue(val.Value, dataTypeName);
                                            memberInfo.SetValue(tableRowObj, value);
                                        }
                                    }

                                    // add row vào collection hoặc set property
                                    if (tableMember.IsList)
                                    {
                                        // non-generic IList vì tableSystemType chỉ biết ở runtime
                                        var list = (System.Collections.IList)tableMember.GetValue(mainObject);
                                        if (list == null)
                                        {
                                            var listType = typeof(List<>).MakeGenericType(tableSystemType);
                                            list = (System.Collections.IList)Activator.CreateInstance(listType);
                                            tableMember.SetValue(mainObject, list);
                                        }
                                        list.Add(tableRowObj);
                                    }
                                    else
                                    {
                                        tableMember.SetValue(mainObject, tableRowObj);
                                    }
                                }
                            }
                        }
                    }
                }
                if (mainObject == null)
                {
                    Application.ShowViewStrategy.ShowMessage("Chưa có đối tượng chính, vui lòng chọn 'Create' để tạo mới.", InformationType.Warning);
                    return;
                }
                // --- Show DetailView cho đối tượng chính ---
                var dv = Application.CreateDetailView(objectSpace, mainObject);
                dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                Application.ShowViewStrategy.ShowView(new ShowViewParameters(dv), new ShowViewSource(Frame, null));
            }
            else
            {
                // === Nếu Multipage = false, gọi cho từng OcrPage ===
                var page = ocrDocument.OcrPageList.FirstOrDefault();
                var cs = new CollectionSource(objectSpace, page.ExtractionTemplate.SystemType);
                List<object> createObject = new();

                if (e.SelectedChoiceActionItem.Id.Equals("Create"))
                {
                    string firstCode = string.Empty;
                    var firstSelected = ocrDocument.OcrPageList.First();

                    foreach (var page1 in ocrDocument.OcrPageList)
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

                var guids = ocrDocument.OcrPageList.Select(p => p.Oid).ToList();
                cs.Criteria["FilterByOcrID"] = new DevExpress.Data.Filtering.InOperator("OcrID", guids);

                foreach (var obj in createObject) cs.Add(obj);

                if (cs.List.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage("Không tìm thấy đối tượng nào để hiển thị.", InformationType.Warning);
                    return;
                }

                var systemTypeName = page.ExtractionTemplate.SystemType.Name;
                var objectCode = systemTypeName.StartsWith("Ocr")
                    ? systemTypeName.Substring(3)   // cắt "Ocr" ở đầu
                    : systemTypeName;

                // Tạo Id đúng theo quy tắc
                var listViewId = $"Ocr{objectCode}_ListView"; var listView = Application.CreateListView(listViewId, cs, true);

                Application.ShowViewStrategy.ShowView(new ShowViewParameters(listView) { CreateAllControllers = true, TargetWindow = TargetWindow.NewModalWindow }, new ShowViewSource(Frame, null));
            }
        





            #endregion OcrDocumentObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3857            Oid: 94845167-6888-4f53-ad8a-68f8d3a7e42a
		private void OcrDocumentMarkdown_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OcrDocumentMarkdown), "Hoán đổi markdown hiển thị");              
      
            #region OcrDocumentMarkdownImportCode
            var ocrDocument = View.CurrentObject as Module.BusinessObjects.OcrDocument;
            if (ocrDocument is null )
            {
                logger.LogWarning("Không có tài liệu nào để xử lý.");
                return;
            }

            if (ocrDocument.Markdown is null)
            {
                ocrDocument.Markdown = ocrDocument.OcrMarkdown;
            }
            else if (ocrDocument.Markdown == ocrDocument.OcrMarkdown)
            {
                ocrDocument.Markdown = ocrDocument.ValueMarkdown;
            }
            else
            {
                ocrDocument.Markdown = ocrDocument.OcrMarkdown;
            }

            #endregion OcrDocumentMarkdownImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}