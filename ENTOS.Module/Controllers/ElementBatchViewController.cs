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
    public partial class ElementBatchViewController: BaseViewController<Module.BusinessObjects.ElementBatch>
    {      
        
        public ElementBatchViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ElementBatch);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
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


        
        //Code: 3393            Oid: f5b4fdd0-2cb3-4954-9497-282a9ecd52c0
		private void MatchLineBatchElement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MatchLineBatchElement), "Khớp dòng lô");              
      
            #region MatchLineBatchElementImportCode
            var selectedElementBatches = View.SelectedObjects.Cast<Module.BusinessObjects.ElementBatch>().ToList();
            if (!selectedElementBatches.Any())
                return;

            DataService dataService = ObjectSpace.FindObject<DataService>(
                DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", "017")
            );
            if (dataService == null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy dataservice", InformationType.Error);
                return;
            }

            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video == null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy video gốc", InformationType.Error);
                return;
            }

            if (e.SelectedChoiceActionItem.Id.Equals("Synchronize"))
            {
                foreach (ElementBatch elementBatch in View.SelectedObjects)
                {
                    var bestBatchTranslate = elementBatch.BatchTranslateList
                        .Where(x => x.Language == video.LanguageTranslate)
                        .FirstOrDefault();

                    if (bestBatchTranslate == null)
                    {
                        Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy lô mẫu", InformationType.Error);
                        return;
                    }

                    foreach (Module.BusinessObjects.BatchTranslate batchTranslate in elementBatch.BatchTranslateList)
                    {
                        if (batchTranslate.Oid == bestBatchTranslate.Oid)
                            continue;
                        if (batchTranslate.LineQuantity == bestBatchTranslate.LineQuantity)
                            continue; // Không cần đồng bộ nếu số dòng giống nhau

                        // Tách content thành danh sách dòng có thứ tự
                        List<string> list1 = Module.Helpers.TextHelper.SplitContentToList(bestBatchTranslate.Translate);

                        // Tách match thành danh sách dòng (gắn thêm bool = false)
                        List<string> listmatch = Module.Helpers.TextHelper.SplitContentToList(batchTranslate.Translate);

                        var list2 = new List<(string, bool)>();
                        foreach (var item in listmatch)
                        {
                            list2.Add((item, false));
                        }

                        // Tìm ánh xạ dòng giữa content và match
                        var matchLines = Module.Services.AudioService.SemanticMatchLine(list1, list2, dataService);

                        // Nếu tìm được ánh xạ dòng → sắp xếp lại origin
                        if (matchLines.Count > 0)
                        {
                            batchTranslate.Translate = Module.Services.AudioService.RearrangeString(batchTranslate.Translate, matchLines, list1.Count, Application);
                            batchTranslate.Translate = Module.Services.BatchTranslateService.FillBlankLines(bestBatchTranslate.Translate, batchTranslate.Translate, dataService, video, video.LanguageOrigin, video.LanguageOrigin);

                            batchTranslate.Content = Module.Services.AudioService.RearrangeString(batchTranslate.Content, matchLines, list1.Count, Application);
                            batchTranslate.Content = Module.Services.BatchTranslateService.FillBlankLines(batchTranslate.Translate, batchTranslate.Content, dataService, video, bestBatchTranslate.Language, batchTranslate.Language);

                            batchTranslate.Translate2 = Module.Services.AudioService.RearrangeString(batchTranslate.Translate2, matchLines, list1.Count, Application);
                            batchTranslate.Translate2 = Module.Services.BatchTranslateService.FillBlankLines(bestBatchTranslate.Translate2, batchTranslate.Translate2, dataService, video, video.LanguageOrigin, video.LanguageOrigin);

                        }

                    }
                }
            }
            if (e.SelectedChoiceActionItem.Id.Equals("Translate"))
            {
                foreach (ElementBatch elementBatch in View.SelectedObjects)
                {
                    foreach (Module.BusinessObjects.BatchTranslate batchTranslate in elementBatch.BatchTranslateList)
                    {
                        string string1 = batchTranslate.Translate2;
                        string string2 = batchTranslate.Translate;

                        string result = Module.Services.BatchTranslateService.ProcessMatchLineAndRearrange(string1, string2, string2, dataService, Application);
                        result = Module.Services.BatchTranslateService.FillBlankLines(batchTranslate.Translate2, result, dataService, video, video.LanguageOrigin, video.LanguageOrigin);

                        if (!string.IsNullOrEmpty(result))
                        {
                            batchTranslate.Translate = result;
                        }
                    }
                }
            }





            #endregion MatchLineBatchElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3293            Oid: b899adff-c483-47c3-afb6-afa7b0152e99
		private void ElementBatchImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ElementBatchImport), "Nạp lô");              
      
            #region ElementBatchImportImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            var batchString = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(
                ObjectSpace, "SelectionQuantity", "3000", SecuritySystem.CurrentUserId);

            List<Module.BusinessObjects.Audio> audioList = video?.AudioList?.ToList();

            if (audioList == null || audioList.Count == 0)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Không có audio để xử lý", InformationType.Info);
                return;
            }

            if (audioList.Any(x => x.Quantity == null))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"Cần cập nhật cột số lượng của các thành phần trước khi thao tác", InformationType.Warning);
                return;
            }

            int batchLimit = Convert.ToInt32(batchString?.Value ?? "3000");

            // Sắp xếp theo thời gian bắt đầu
            audioList.Sort((a, b) => Nullable.Compare(a.Start, b.Start));

            // Chia thành các ElementBatch
            var currentBatch = ObjectSpace.CreateObject<Module.BusinessObjects.ElementBatch>();
            decimal currentSum = 0;
            var previousAudioBookmark = audioList.FirstOrDefault().BookMark;
            var order = 1;

            foreach (var audio in audioList)
            {
                decimal quantity = audio.Quantity ?? 0;
                audio.Order = order;
                order++;

                // Nếu vượt quá giới hạn batch
                if (currentSum + quantity > batchLimit || audio.BookMark != previousAudioBookmark)
                {
                    order = 1; // Reset order for new batch
                    audio.Order = order;
                    order++;

                    // Nếu batch hiện tại có audio, lưu lại
                    if (currentBatch.AudioList.Count > 0)
                    {
                        currentBatch.Video = video;
                        currentBatch = ObjectSpace.CreateObject<Module.BusinessObjects.ElementBatch>();
                        currentSum = 0;
                    }

                    // Nếu 1 audio vượt batch limit, tạo riêng 1 batch
                    if (quantity > batchLimit)
                    {
                        var singleBatch = ObjectSpace.CreateObject<Module.BusinessObjects.ElementBatch>();
                        singleBatch.Video = video;
                        singleBatch.AudioList.Add(audio);
                        continue;
                    }
                }
                previousAudioBookmark = audio.BookMark;
                currentBatch.AudioList.Add(audio);
                currentSum += quantity;
            }
            // Xử lý batch cuối cùng nếu còn audio
            if (currentBatch.AudioList.Count > 0)
            {
                currentBatch.Video = video;
            }

            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã tạo các ElementBatch từ audio", InformationType.Success);

            #endregion ElementBatchImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3297            Oid: 4fd79e88-f8a0-4926-90cb-e5b8c5cec6a4
		private void BatchTranslateImportElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchTranslateImportElement), "Nạp Dịch lô");              
      
            #region BatchTranslateImportElementImportCode
            try
            {
                int skip = 0;
                foreach (Module.BusinessObjects.ElementBatch elementBatch in View.SelectedObjects)
                {
                    skip += Module.Services.BatchTranslateService.CreateBatchTranslate(elementBatch);
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã nạp dịch khối thành công.", InformationType.Success);
                if (skip > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", $"Đã bỏ qua {skip} ngôn ngữ đã tồn tại trong danh sách dịch khối.", InformationType.Info);
            }
            catch (Exception ex)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thất bại", $"Không thể nạp dịch khối : {ex.Message}", InformationType.Error);
            }

            #endregion BatchTranslateImportElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3358            Oid: 37987eea-3037-4653-bf27-9b1d4ac74c91
		private void BatchTranslateTranslationElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchTranslateTranslationElement), "Dịch thuật lô");              
      
            #region BatchTranslateTranslationElementImportCode
    var selectedItems = e.SelectedObjects.Cast<ElementBatch>().ToList();

    foreach (var element in selectedItems)
    {
        foreach (var bt in element.BatchTranslateList)
        {
            if (!string.IsNullOrWhiteSpace(bt.Content) &&
                bt.Language?.Code != null &&
                bt.OriginLanguage?.Code != null)
            {
                bt.Translate2 = Module.SystemObjects.Tools.LineByLineTranslate(
                    bt.Content,
                    destination: bt.OriginLanguage.Code,
                    source: bt.Language.Code
                );
            }
        }
    }

    ObjectSpace.CommitChanges();
    View.ObjectSpace.Refresh();


            #endregion BatchTranslateTranslationElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3305            Oid: d660d912-5016-42af-8913-3f77f34f4974
		private void BatchLanguageTranslateElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchLanguageTranslateElement), "Dịch ngữ");              
      
            #region BatchLanguageTranslateElementImportCode
            foreach (Module.BusinessObjects.ElementBatch elementBatch in View.SelectedObjects)
            {
                foreach (Module.BusinessObjects.BatchTranslate batch in elementBatch.BatchTranslateList)
                {
                    try
                    {
                        Module.Services.BatchTranslateService.CreateElementTranslate(batch);
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã nạp dịch thành phần thành công.", InformationType.Success);
                    }
                    catch (Exception ex)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thất bại", $"Không thể nạp dịch thành phần: {ex.Message}", InformationType.Error);
                    }
                }
            }

            #endregion BatchLanguageTranslateElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}