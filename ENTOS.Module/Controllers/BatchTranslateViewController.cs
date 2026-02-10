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
    public partial class BatchTranslateViewController: BaseViewController<Module.BusinessObjects.BatchTranslate>
    {      
        
        public BatchTranslateViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.BatchTranslate);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.BatchTranslateService batchTranslateService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             batchTranslateService = new Module.Services.BatchTranslateService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3296            Oid: 9d686de5-614f-464e-be97-d34da4214421
		private void BatchTranslateImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchTranslateImport), "Nạp Dịch lô");              
      
            #region BatchTranslateImportImportCode
            var elementBatch = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.ElementBatch;
            try
            {
                int skip = Module.Services.BatchTranslateService.CreateBatchTranslate(elementBatch);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã nạp dịch khối thành công.", InformationType.Success);
                if (skip > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", $"Đã bỏ qua {skip} ngôn ngữ đã tồn tại trong danh sách dịch khối.", InformationType.Info);
            }
            catch (Exception ex)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thất bại", $"Không thể nạp dịch khối : {ex.Message}", InformationType.Error);
            }

            #endregion BatchTranslateImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3298            Oid: 186e3c27-0ecb-4274-9c07-7ab0b837f045
		private void TranslateCommand_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TranslateCommand), "Lệnh dịch");              
      
            #region TranslateCommandImportCode
            var batch = View.CurrentObject as BatchTranslate;
            if (batch == null) return;

            string choice = e.SelectedChoiceActionItem.Id;

            if (choice == "ReverseTranslate")
            {
                string result = batchTranslateService.BuildReverseTranslatePrompt(
                    batch.Content,
                    batch.OriginLanguage?.Name,
                    batch.Language?.Name);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    try
                    {
                        Module.SystemObjects.Tools.ClipboardSetText(result);
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã sao chép nội dung ReverseTranslate vào clipboard.", InformationType.Success);
                    }
                    catch (Exception ex)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"Không thể sao chép vào clipboard: {ex.Message}", InformationType.Error);
                    }
                }
                else
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Không có nội dung để sao chép.", InformationType.Info);
                }
            }
            else if (choice.StartsWith("Translate"))
            {
                string symbol = "↩";
                string result = batchTranslateService.BuildTranslateClipboardText(batch.ElementBatch, batch.Language?.Name);

                if (choice.Contains("Symbol"))
                    result = batchTranslateService.BuildTranslateClipboardText(batch.ElementBatch, batch.Language?.Name, symbol);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    try
                    {
                        Module.SystemObjects.Tools.ClipboardSetText(result);
                        Module.Helpers.XafXpoHelper.ShowMessage(
                            Application, "Thành công", "Đã sao chép nội dung lệnh dịch vào clipboard.", InformationType.Success);
                    }
                    catch (Exception ex)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(
                            Application, "Thất bại", $"Không thể sao chép vào clipboard: {ex.Message}", InformationType.Error);
                    }
                }
                else
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(
                        Application, "Thông báo", "Không có nội dung để sao chép.", InformationType.Info);
                }
            }

            #endregion TranslateCommandImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3351            Oid: d807083c-f355-4d7c-b916-7d430bf2fffa
		private void BatchTranslateTranslation_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchTranslateTranslation), "Dịch thuật lô");              
      
            #region BatchTranslateTranslationImportCode
            var batch = View.CurrentObject as BatchTranslate;
            if (batch == null) return;

            string choice = e.SelectedChoiceActionItem.Id;

            if (choice == "Translate2")
            {
				var selectedItems = e.SelectedObjects.Cast<BatchTranslate>().ToList();

				foreach (var item in selectedItems)
				{
					item.Translate2 = Module.SystemObjects.Tools.LineByLineTranslate(
						input: item.Content,
						destination: item.OriginLanguage?.Code ?? "vi",
						source: item.Language?.Code ?? "en"
					);
				}

				ObjectSpace.CommitChanges();
				View.ObjectSpace.Refresh();

            }
            else
            {
				return;
            }

            #endregion BatchTranslateTranslationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3352            Oid: f870ea4a-5f92-4422-9356-16068f85e28a
		private void MatchlineBatch_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MatchlineBatch), "Khớp dòng lô");              
      
            #region MatchlineBatchImportCode
            var elementBatch = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.ElementBatch;
            var video = elementBatch.Video;

            if (elementBatch == null || video == null)
            {
                return;
            }
            DataService dataService = ObjectSpace.FindObject<DataService>(
                DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", "017")
            );
            if (dataService == null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy dataservice", InformationType.Error);
                return;
            }

            if (e.SelectedChoiceActionItem.Id.Equals("Synchronize"))
            {
                var bestBatchTranslate = elementBatch.BatchTranslateList
                    .Where(x => x.Language == video.LanguageTranslate)
                    .FirstOrDefault();

                if (bestBatchTranslate == null)
                {
                    Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy lô mẫu", InformationType.Error);
                    return;
                }

                foreach (Module.BusinessObjects.BatchTranslate batchTranslate in View.SelectedObjects)
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
                        batchTranslate.Content = Module.Services.BatchTranslateService.FillBlankLines(bestBatchTranslate.Content, batchTranslate.Translate, dataService, video, bestBatchTranslate.Language, batchTranslate.Language);

                        batchTranslate.Translate2 = Module.Services.AudioService.RearrangeString(batchTranslate.Translate2, matchLines, list1.Count, Application);
                        batchTranslate.Translate2 = Module.Services.BatchTranslateService.FillBlankLines(bestBatchTranslate.Translate2, batchTranslate.Translate2, dataService, video, video.LanguageOrigin, video.LanguageOrigin);

                    }

                }
            }
            if (e.SelectedChoiceActionItem.Id.Equals("Translate"))
            {
                foreach (Module.BusinessObjects.BatchTranslate batchTranslate in View.SelectedObjects)
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




            #endregion MatchlineBatchImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3315            Oid: 813c2739-10bf-43ae-b081-afa21c6cc642
		private void ExportElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExportElement), "Xuất thành phần");              
      
            #region ExportElementImportCode

            var batchTranslate = View.CurrentObject as Module.BusinessObjects.BatchTranslate;
            var elementBatch = batchTranslate.ElementBatch;

            List<Audio> audioList = elementBatch?.AudioList.ToList();
            if (audioList == null || audioList.Count == 0)
                return;

            audioList.Sort((a, b) => a.Start.Value.CompareTo(b.Start.Value));

            // Tách từng dòng dịch
            var lines = batchTranslate.Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int count = Math.Min(lines.Length, audioList?.Count ?? 0);

            for (int i = 0; i < count; i++)
            {
                var audio = audioList.FirstOrDefault(x => x.Order == i + 1);
                if (audio == null)
                    continue;

                audio.Subtitle = lines[i].Trim();
            }
            #endregion ExportElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3304            Oid: e3fc401e-75da-41ac-9d9b-2e6879c52bb1
		private void BatchLanguageTranslate_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(BatchLanguageTranslate), "Dịch ngữ");              
      
            #region BatchLanguageTranslateImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Export"))
            {
                foreach (Module.BusinessObjects.BatchTranslate batch in View.SelectedObjects)
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
            else if (e.SelectedChoiceActionItem.Id.Equals("Delete"))
            {
                foreach (Module.BusinessObjects.BatchTranslate batch in View.SelectedObjects)
                {
                    foreach(var audio in batch.ElementBatch.AudioList)
                    {
                        // Xóa tất cả bản dịch của audio
                        var translations = audio.ElementTranslateList.FirstOrDefault(x => x.Language == batch.Language);
                        if (translations != null)
                        {
                            audio.ElementTranslateList.Remove(translations);
                            translations.Delete();
                        }
                    }
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thành công", "Đã xóa dịch thành phần thành công.", InformationType.Success);

            }

            #endregion BatchLanguageTranslateImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}