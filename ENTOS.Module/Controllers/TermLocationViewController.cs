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
    public partial class TermLocationViewController: BaseViewController<Module.BusinessObjects.TermLocation>
    {      
        
        public TermLocationViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.TermLocation);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
     
        private TermService termService;
        protected TermService _termService => termService ??= new TermService(this);        
      
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.TermLocationService termLocationService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             termLocationService = new Module.Services.TermLocationService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1048            Oid: 838d8eb1-1952-4ed3-b459-7acc96e83021
		private void ReplaceTermLocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ReplaceTermLocation), "Thay thế");              
      
            #region ReplaceTermLocationImportCode
var term = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Term;
            if (term is null)
                return;
            if (string.IsNullOrEmpty(term.Name))
                return;
            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.AcceptAction.Caption = "Thay thế";
            dc.SaveOnAccept = true;
            var showViewParameters = new ShowViewParameters
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                NewWindowTarget = NewWindowTarget.Separate
            };
            dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
            {
                if (args.AcceptActionArgs.CurrentObject is Module.SystemObjects.ReplaceObject)
                {
                    var replaceObjectControl = (Module.SystemObjects.ReplaceObject)args.AcceptActionArgs.CurrentObject;
                    if (string.IsNullOrEmpty(replaceObjectControl.Replace))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Từ được thay thế không được phép trống", InformationType.Error);
                        return;
                    }
                    //084:  Chức năng Thay thê(multi select) : chọn thuật vị, nhập từ cần thay,
                    //sau khi thay thì hạ cờ của các Thuật vị đã thay,
                    //nếu không tồn tại Thuật vị dựng cờ thì hạ nốt cờ của Thuật ngữ
                    var existedTerm = TermService.FindTermByName(term, replaceObjectControl.Replace);
                    foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                    { 
                        if(existedTerm is null)
                            existedTerm = TermService.FindTermByName(term, replaceObjectControl.Replace);
                        TermLocationService.ReplaceWord(termLocation,replaceObjectControl.Replace, false, existedTerm);
                    }
                    if (term.Flag)
                    {
                        //084: nếu không tồn tại Thuật vị dựng cờ thì hạ nốt cờ của Thuật ngữ
                        bool termFlag = false;
                        foreach (var tl in term.TermLocationList)
                        {
                            if (tl.Flag)
                            {
                                termFlag = true;
                                break;
                            }
                        }
                        if (!termFlag)
                            term.Flag = false;
                    }
                            
                    //Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + " từ được thay thành công");
                }
            };
            showViewParameters.Controllers.Add(dc);
            Module.SystemObjects.ReplaceObject replaceObject = new Module.SystemObjects.ReplaceObject();
            replaceObject.Find = term.Name;
            showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), replaceObject, true);
            showViewParameters.Context = TemplateContext.PopupWindow;
            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));


            #endregion ReplaceTermLocationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0944            Oid: 50af8a6b-6525-48f6-8553-292fabab02bd
		private void EditWordLocation_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(EditWordLocation), "Soạn thảo");              
      
            #region EditWordLocationImportCode
//Mở hộp thoại nhập giá trị cần chèn, nếu thuật ngữ là đầu câu và chèn trước thì Viết hoa từ chèn và bỏ hoa thuật ngữ,
            //      áp dụng mọi thuật vị kể cả chưa dịch hoặc chưa thay từ 
            //Chức năng tương tự trên Thuật ngữ, sẽ thực hiện mọi thuật vị thuộc thuật ngữ được chọn,
            //  nhưng chỉ SingleChoice k như Thuật vị là Multichoice
            
            if (e.SelectedChoiceActionItem.Id.Equals("InsertBefore") || e.SelectedChoiceActionItem.Id.Equals("InsertAfter"))
            {
                var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
                dc.AcceptAction.Caption = "Chèn";
                dc.SaveOnAccept = true;
                var showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate
                };
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    if (args.AcceptActionArgs.CurrentObject is Module.SystemObjects.InsertText)
                    {
                        var insertTextControl = (Module.SystemObjects.InsertText)args.AcceptActionArgs.CurrentObject;
                        if (string.IsNullOrEmpty(insertTextControl.Word))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Từ được chèn không được phép trống", InformationType.Error);
                            return;
                        }                        
                        int result = 0;
                        foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                        {
                            result += termLocationService.InsertWord(termLocation, View.SelectedObjects.Count, e.SelectedChoiceActionItem.Id.Equals("InsertBefore"), insertTextControl.Word);                            
                        }
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + View.SelectedObjects.Count + " được xử lý", InformationType.Info, 10000);
                    }
                };
                showViewParameters.Controllers.Add(dc);
                Module.SystemObjects.InsertText insertText = new Module.SystemObjects.InsertText();
                showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), insertText, true);
                showViewParameters.Context = TemplateContext.PopupWindow;

                Application.ShowViewStrategy.ShowView(showViewParameters,
                    new ShowViewSource(Frame, dc.AcceptAction));
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("DeleteBefore") || e.SelectedChoiceActionItem.Id.Equals("DeleteAfter"))
            {
                int result = 0;                
                foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    result += termLocationService.DeleteWord(termLocation, View.SelectedObjects.Count, e.SelectedChoiceActionItem.Id.Equals("DeleteBefore"));                    
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + View.SelectedObjects.Count + " được xử lý", InformationType.Info, 10000);
            }

            #endregion EditWordLocationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0924            Oid: 5d743b55-da55-4008-bf1c-a7f18d300cbc
		private void ReplaceTranslateLocation_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ReplaceTranslateLocation), "Thay dịch");              
      
            #region ReplaceTranslateLocationImportCode
            char charTag = '{';
            int result = 0;
            foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
            {
                if(View.SelectedObjects.Count == 1)
                {
                    if (termLocation.ReplaceTranslate)
                    {
                        if (e.SelectedChoiceActionItem.Id.Equals("Replace"))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không thể thay thế khi đang dựng cờ", InformationType.Error);
                            return;
                        }
                    }
                    else
                    {
                        if (e.SelectedChoiceActionItem.Id.Equals("UnReplace"))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không thể trả lại khi không đang dựng cờ", InformationType.Error);
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(termLocation.Translate))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Dịch không được phép trống", InformationType.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(termLocation.MachineTranslate))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Máy dịch không được phép trống", InformationType.Error);
                        return;
                    }
                }
                if (termLocationService.ReplaceUnReplaceTranslate(termLocation, e.SelectedChoiceActionItem.Id,e.SelectedChoiceActionItem.Caption, charTag))
                    result++;
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + " từ được thay thành công");



            #endregion ReplaceTranslateLocationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0946            Oid: 722bb6af-1fe4-4534-b5c7-bf35035191b2
		private void MoveForward_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MoveForward), "Dịch tiến");              
      
            #region MoveForwardImportCode
//Tiến hoặc lùi thuật ngữ tại thuật vị được chọn 1 bước (1 từ)
            int result = 0;
            foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                result += termLocationService.MoveWord(termLocation, View.SelectedObjects.Count, true);
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + " từ được dịch chuyển thành công");

            #endregion MoveForwardImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0947            Oid: a731c39a-62bc-45de-acfe-a0f37391e8b0
		private void MoveBackward_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MoveBackward), "Dịch lùi");              
      
            #region MoveBackwardImportCode
int result = 0;
            foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                result += termLocationService.MoveWord(termLocation, View.SelectedObjects.Count, false);
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + " từ được dịch chuyển thành công");
            //Tiến hoặc lùi thuật ngữ tại thuật vị được chọn 1 bước (1 từ)

            #endregion MoveBackwardImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1496            Oid: 3df02889-8625-4232-9512-beb4bc384a54
		private void OverlapTermPosition_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OverlapTermPosition), "Thuật ngữ đè");              
      
            #region OverlapTermPositionImportCode
            int termCount = 0, termLocationCount = 0;
            foreach(Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
            {
                termLocationService.OverlapTermPosition(termLocation, e.SelectedChoiceActionItem.Id, ref termCount, ref termLocationCount);
            }
            string message = "Có " + termLocationCount + " thuật vị được xử lý";
            if(termCount > 0) 
                message += "\r\n " + termCount + " thuật ngữ được xử lý";
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", message);


            #endregion OverlapTermPositionImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0933            Oid: 37068d8b-0f7a-45bb-be66-56edf37619c5
		private void TranslateLocationTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TranslateLocationTerm), "Dịch thuật vị");              
      
            #region TranslateLocationTermImportCode
            var currentObject = View.CurrentObject as Module.BusinessObjects.TermLocation;
            if (currentObject is null)
                return;
            if (e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextUpcase"))
            {
                //-Ngữ cảnh: Tìm từ chung trong các câu bên Dịch nội dung ứng với Thuật vị,
                //2023-06-29 So sánh dịch theo google translate 2 lần                
                int susscess = 0;

                if (!string.IsNullOrEmpty(currentObject.Term.Name) && (string.IsNullOrEmpty(currentObject.Term.Translate) || string.IsNullOrEmpty(currentObject.Term.GoogleTranslate)))
                {
                    System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
                    foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                    {
                        // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                        //termLocation.Flag = false;
                        bool flag = false;
                        var audio = termLocation.GetAudioFromElement();
                        if (audio is null)
                            continue;
                        if (string.IsNullOrEmpty(audio.Subtitle) || string.IsNullOrEmpty(audio.Content))
                            continue;
                        //2023-08-09: Dịch > Máy dịch của Thuật vị không thực hiện khi Máy dịch của Thuật ngữ khác null > cần cho phép
                        //if (!string.IsNullOrEmpty(termLocation.MachineTranslate))
                        //    continue;                        
                        string newTranlate = null;
                        foreach (var key in dictionaryResult.Keys)
                        {
                            var index = Module.Helpers.TextHelper.GetIndexWordInContent(key, audio.Subtitle);
                            if (index >= 0)
                            {
                                newTranlate = audio.Subtitle.Substring(index, key.Length);
                            }
                        }
                        if (string.IsNullOrEmpty(newTranlate))
                        {
                            string newContent = "";
                            var firstIndex = 0;
                            var content = audio.Content.ToLower();
                            var index = content.IndexOf(currentObject.Term.Name, System.StringComparison.OrdinalIgnoreCase);
                            while (index >= 0)
                            {
                                newContent += content.Substring(firstIndex, index - firstIndex);
                                firstIndex = index + currentObject.Term.Name.Length;

                                //var afterCharIndex = firstIndex + word.Length;                                
                                bool validate = true;
                                if (firstIndex < content.Length - 1 && !char.IsWhiteSpace(content[firstIndex])
                                && char.IsLetterOrDigit(content[firstIndex]) && !content.Substring(firstIndex).StartsWith(", "))
                                {
                                    //var charText = text[firstIndex];
                                    validate = false;
                                }
                                else if (!string.IsNullOrEmpty(newContent) && char.IsLetterOrDigit(newContent[newContent.Length - 1]))
                                {
                                    //var charText = text[firstIndex];
                                    validate = false;
                                }
                                if (validate)
                                {
                                    newContent += Module.Helpers.TextHelper.RemoveAccents(currentObject.Term.Name).ToUpper();
                                }
                                else
                                {
                                    newContent += content.Substring(index, currentObject.Term.Name.Length);
                                }
                                if (firstIndex >= content.Length)
                                    break;
                                index = content.IndexOf(currentObject.Term.Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);
                            }
                            newContent += content.Substring(firstIndex);
                            var newtranlateContent = Module.SystemObjects.Tools.TranslateText(newContent);
                            if (string.IsNullOrEmpty(newtranlateContent))
                                continue;
                            int startIndex = -1;
                            int endIndex = -1;
                            int lastedIndex = -1;
                            for (int i = 0; i < newtranlateContent.Length; i++)
                            {
                                if (startIndex < 0 && char.IsUpper(newtranlateContent[i]))
                                {
                                    startIndex = i;
                                }
                                if (startIndex >= 0 && !char.IsUpper(newtranlateContent[i]) && newtranlateContent[i] != ' ')
                                {
                                    if (i == startIndex + 1)
                                    {
                                        //Trường hợp google tự sửa viết hoa đầu dòng
                                        lastedIndex = startIndex;
                                        startIndex = -1;
                                        continue;
                                    }
                                    var endText = newtranlateContent.Substring(0, i);
                                    endIndex = i;
                                    break;
                                }
                            }
                            if (startIndex < 0 && lastedIndex > 0)
                            {
                                startIndex = lastedIndex;
                            }

                            if (startIndex >= 0 || endIndex > 0)
                            {
                                //Nếu tìm thấy từ viết hoa
                                if (startIndex < 0)
                                    startIndex = 0;
                                if (endIndex < 0)
                                    endIndex = newtranlateContent.Length;
                                newTranlate = newtranlateContent.Substring(startIndex, endIndex - startIndex);
                                int newStartIndex = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                                if (newStartIndex < 0)
                                {
                                    //Từ được dịch không hợp lệ
                                    newTranlate = null;
                                }
                                else
                                {
                                    newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
                                }
                            }

                            if (string.IsNullOrEmpty(newTranlate))
                            {
                                //Dịch thử thông thường
                                var gTranslate = Module.SystemObjects.Tools.TranslateText(currentObject.Term.Name);
                                int newStartIndex = audio.Subtitle.IndexOf(gTranslate, System.StringComparison.OrdinalIgnoreCase);
                                if (newStartIndex >= 0)
                                {
                                    newTranlate = gTranslate;
                                    newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
                                }
                                else
                                {
                                    //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
                                    //termLocation.Flag = true;
                                    flag = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(newTranlate))
                        {
                            newTranlate = newTranlate.Trim();
                            if (newTranlate.Equals(currentObject.Term.Name, System.StringComparison.OrdinalIgnoreCase))
                                newTranlate = currentObject.Term.Name;
                            termLocation.MachineTranslate = newTranlate;
                            //2023 - Khi dịch máy manual trên Thuật ngữ hoặc Thuật vị sẽ xác định bằng tìm kiếm nếu kết quả thấy 1 thì cập nhật
                            //  , 0 thấy hoặc 2 trở lên thì phải cập nhật Vị trí dịch bằng manual
                            int count = 0;
                            var index = -1;
                            while (true)
                            {
                                var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(newTranlate, audio.Subtitle, null, index + 1);
                                if (newIndex < 0)
                                    break;
                                index = newIndex;
                                count++;
                            }
                            if (count == 1)
                            {
                                var firstText = audio.Subtitle.Substring(0, index);
                                var rows = firstText.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                                int position = 0;
                                for (int m = 0; m < rows.Count(); m++)
                                {
                                    var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                    //Vị trí của mảng nhỏ hơn 1 so với vị trí thực tế, nên vị trí của từ cũng là vị trí của mảng
                                    position += contents.Length;
                                }
                                //Tổng số lượng trong mảng mảng nhỏ hơn 1 so với vị trí thực tế
                                termLocation.TranslateLocation = position + 1;
                            }
                            // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                            //termLocation.Flag = audio.Subtitle.IndexOf(newTranlate) < 0;
                            if (!flag)
                                flag = audio.Subtitle.IndexOf(newTranlate) < 0;
                            var key = Module.Helpers.TextHelper.KeyListContains(dictionaryResult.Keys, newTranlate);
                            if (string.IsNullOrEmpty(key))
                            {
                                var newTranlates = Module.SystemObjects.Tools.TranslateText(currentObject.Term.Name);
                                // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                                //dictionaryResult.Add(newTranlate, termLocation.Flag ? 0 : 1);
                                dictionaryResult.Add(newTranlate, flag ? 0 : 1);
                            }
                            //else if (!termLocation.Flag)
                            else if (!flag)
                            {
                                dictionaryResult[key]++;
                            }
                            susscess++;
                        }
                        else
                        {
                            // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                            //Nếu không tìm thấy thuật ngữ thì dựng cờ
                            if (!flag)
                                flag = audio.Subtitle.IndexOf(newTranlate) < 0;
                            //termLocation.Flag = true;
                        }
                    }
                    if (dictionaryResult.Keys.Count > 0)
                    {
                        int max = 0;
                        string maxKey = null;
                        foreach (var key in dictionaryResult.Keys)
                        {
                            if (dictionaryResult[key] == 0)
                            {
                                currentObject.Term.Flag = true;
                                if (!string.IsNullOrEmpty(currentObject.Term.Note))
                                    currentObject.Term.Note += "; ";
                                currentObject.Term.Note += "Không tìm thấy " + e.SelectedChoiceActionItem.Caption;
                            }
                            if (dictionaryResult[key] > max)
                            {
                                max = dictionaryResult[key];
                                maxKey = key;
                            }
                        }
                        //Chỉ lưu vào dịch máy
                        if (!string.IsNullOrEmpty(maxKey))
                        {
                            currentObject.Term.GoogleTranslate = maxKey;
                        }
                    }
                    else
                    {
                        currentObject.Term.Flag = true;
                        if (!string.IsNullOrEmpty(currentObject.Term.Note))
                            currentObject.Term.Note += "; ";
                        currentObject.Term.Note += "Không tìm thấy " + e.SelectedChoiceActionItem.Caption;

                    }
                }
                foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    if (termLocation.Term is null)
                        continue;
                    ////Xóa trắng dịch máy 
                    //term.GoogleTranslate = null;

                }
                //Module.Helpers.XafXpoHelper.ShowMessage(Application, "Nạp " + View.SelectedObjects.Count + " thuật ngữ",
                //        "Có " + susscess + "/" + termLocationCount + " thuật vị được nạp", InformationType.Info);

            }
            else if (e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextSlash") ||
                e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextApostrophe") ||
                e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextStrong"))
            {
                //-Ngữ cảnh: Tìm từ chung trong các câu bên Dịch nội dung ứng với Thuật vị,
                //2023-06-29 So sánh dịch theo google translate 2 lần
                //string seperateKey = e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextSlash") ? "/" : "'";
                //seperateKey = "'";
                int termLocationCount = 0;
                int susscess = 0;
                var termList = new System.Collections.Generic.List<Module.BusinessObjects.Term>();
                foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    if (termLocation.Term != null && !termList.Contains(termLocation.Term))
                        termList.Add(termLocation.Term);
                    termLocationService.TranslateTermLocation(termLocation, ref susscess, ref termLocationCount, e.SelectedChoiceActionItem.Id, Application);
                }
                foreach (var term in termList)
                    termService.UpdateGoogleTranslate(term);
                //Module.Helpers.XafXpoHelper.ShowMessage(Application, "Nạp " + View.SelectedObjects.Count + " thuật ngữ",
                //        "Có " + susscess + "/" + termLocationCount + " thuật vị được nạp", InformationType.Info);
            }
            else if (e.SelectedChoiceActionItem.Id.Contains("KeepOrigin"))
            {
                //Nguyên gốc: Copy từ nguyên gốc vào trường Dịch của Thuật ngữ từ đó sẽ vào trường Dịch của Thuật vị
                if (currentObject.Term != null && !string.IsNullOrEmpty(currentObject.Term.Name))
                {
                    if (string.IsNullOrEmpty(currentObject.Term.Translate))
                    {
                        currentObject.Term.Translate = currentObject.Term.Name;
                    }
                    //2023-07-31: Chức năng Dịch>Giữ nguyên (keepOrigin)
                    //ngoài Dịch của Thuật ngữ cũng sẽ Copy từ gốc vào Dịch(nếu null) của những Thuật vị nào mà Dịch máy khác Gốc
                    foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                    {
                        if (!currentObject.Term.Name.Equals(termLocation.MachineTranslate))
                        {
                            termLocation.Translate = currentObject.Term.Name;
                        }
                    }
                }
            }
            else if (e.SelectedChoiceActionItem.Id.Contains("SyncTermTranslate"))
            {
                //SyncTermTranslate: Copy Dịch của Thuật ngữ lên những Dịch (nếu null) tại toàn bộ thuật vị
                //if (currentObject.Term != null && !string.IsNullOrEmpty(currentObject.Term.Translate))
                //{
                //    foreach (var termLocation in currentObject.Term.TermLocationList)
                //    {
                //        termLocation.Translate = currentObject.Term.Translate;
                //    }
                //}
                //2024-06-26: thay đổi cấu trúc
                foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    if (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Translate))
                    {
                        termLocation.Translate = termLocation.Term.Translate;
                    }
                }
            }






            #endregion TranslateLocationTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1049            Oid: 72b7bc94-50aa-4c9c-8b98-bb38d285e56e
		private void SyncTermLocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SyncTermLocation), "Đồng bộ");              
      
            #region SyncTermLocationImportCode
            //084: Chức năng: Đồng bộ (1 select): chọn thuật vị chuẩn, đồng bộ toàn bộ các thuật vị của Thuật ngữ,
            //sau khi đồng bộ thì hạ cờ của toàn bộ Thuật vị và của Thuật ngữ
            var currentTermLocation = View.CurrentObject as Module.BusinessObjects.TermLocation;
            if (currentTermLocation is null)
                return;
            if (currentTermLocation.Audio is null || currentTermLocation.Term is null || currentTermLocation.Location is null)
                return;
            var currentElement = currentTermLocation.Audio;
            //if (currentElement is null)
            //    return;
            var currentContent = currentElement.Content.Replace("  ", " ");
            var currentContentNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(currentContent);
            var currentIndex = TermLocationService.GetIndexContent(currentTermLocation, currentContentNoneUnicode, currentTermLocation.Term.Name);
            if (currentIndex < 0)
                return;
            var termUnicode = currentContent.Substring(currentIndex, currentTermLocation.Term.Name.Length);
            if(currentTermLocation.Term.Name.Equals(Module.Helpers.TextHelper.RemoveUnicode(termUnicode), System.StringComparison.OrdinalIgnoreCase))
            {
                //Kiểm tra từ xem có hợp lệ không
                foreach (Module.BusinessObjects.TermLocation termLocation in currentTermLocation.Term.TermLocationList)
                {
                    if (termLocation.Oid.Equals(currentTermLocation.Oid))
                        continue;
                    var element = termLocation.GetAudioFromElement();
                    if (element is null || termLocation.Term is null)
                        continue;
                    if (string.IsNullOrEmpty(termLocation.Term.Name) || string.IsNullOrEmpty(element.Content))
                        continue;
                    var content = element.Content.Replace("  ", " ");
                    var contentNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(content);
                    var index = TermLocationService.GetIndexContent(termLocation, contentNoneUnicode, termLocation.Term.Name);
                    if (index < 0)
                        continue;
                    var newContent = content.Substring(0, index);
                    newContent += termUnicode;
                    newContent += content.Substring(index + termUnicode.Length);
                    element.Content = newContent;
                    termLocation.Flag = false;
                }
                currentTermLocation.Flag = false;
                currentTermLocation.Term.Flag = false;
            }
            else
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không xác định được thuật ngữ", InformationType.Error);
                return;
            }


            #endregion SyncTermLocationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0921            Oid: 646dc98a-3d2e-4124-ba3c-ef7fba78eca7
		private void MergeTermAdjacentPosition_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MergeTermAdjacentPosition), "Gộp liền kề");              
      
            #region MergeTermAdjacentPositionImportCode
                        //Kiểm tra xem nếu có dựng cờ thay dịch thì không cho ghép
            if(View.SelectedObjects.Cast<Module.BusinessObjects.TermLocation>().FirstOrDefault(x =>x.ReplaceTranslate) != null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không thể ghép khi đã thay dịch", InformationType.Error);
                return;
            }
            //Chọn 1 thuật vị sẽ thực hiện gộp thuật ngữ với từ liền trước hay liền sau với Thuật ngữ đó tại vị trí thuật vị, nhưng sẽ tìm toàn bộ để gộp
            //Sau gộp là Cập nhật lại thuật vị/ Dịch ngữ cảnh lại: 2 từ cũ và 1 từ mới
            //Chức năng này kết hợp với chức năng dựng cờ Từ hoa cận đầu để hộp từ viết hoa nhưng đứng đầu câu
            var currentObject = View.CurrentObject as Module.BusinessObjects.TermLocation;
            if (currentObject is null) return;
            if (currentObject.Term is null) return;
            if (string.IsNullOrEmpty(currentObject.Term.Name) || currentObject.Term.Video is null) return;
            //Kiểm tra xem có tồn tại term không?
            var parrentTerms = TermService.GetParrentTerms(currentObject.Term);
            Module.BusinessObjects.Term existedTerm = null;
            string otherTermText = "";
            string newTermText = "";
            //2024-08-09: code mới theo thuật vị
            if (currentObject.Audio != null && !string.IsNullOrEmpty(currentObject.Audio.Content))
            {
                //2024-08-09: dùng thuật vị để tìm vị trí
                string content = TermLocationService.GetSentenceTextFromContent(currentObject, currentObject.Audio.Content);
                int index = TermLocationService.GetIndexContent(currentObject, currentObject.Audio.Content, currentObject.Term.Name);
                if (index < 0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy vị trí thuật vị trong câu", InformationType.Error);
                    return;
                }
                while (string.IsNullOrEmpty(otherTermText) && index >= 0)
                {
                    if (index >= 0)
                    {
                        //Cố tìm từ đúng
                        if (e.SelectedChoiceActionItem.Id.Contains("Previous"))
                        {
                            if (index == 0)
                                index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, content.Substring(1));
                            if (index >= 0)
                            {
                                //Bỏ qua dấu cách trước đó
                                for (int j = index - 1; j >= 0; j--)
                                {
                                    if (char.IsLetterOrDigit(content[j]) || content[j] == '.' || content[j] == ',')
                                    {
                                        otherTermText = content[j] + otherTermText;
                                    }
                                    else if (!string.IsNullOrEmpty(otherTermText))
                                    {
                                        if (otherTermText.Length == 1 && !char.IsLetterOrDigit(otherTermText[0]))
                                            continue;
                                        else break;
                                    }
                                }
                                if (!string.IsNullOrEmpty(otherTermText))
                                    otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(otherTermText);
                                if (!string.IsNullOrEmpty(otherTermText))
                                    newTermText = otherTermText + " " + currentObject.Term.Name;
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Contains("Next"))
                        {
                            //Thêm cả khoảng cách đằng sau từ tìm thấy
                            index += currentObject.Term.Name.Length;
                            if (index < content.Length - 1)
                            {
                                //Bỏ qua dấu cách trước đó
                                for (int j = index + 1; j < content.Length; j++)
                                {
                                    if (char.IsLetterOrDigit(content[j]) || content[j] == '.' || content[j] == ',')
                                    {
                                        otherTermText += content[j];
                                    }
                                    else if (!string.IsNullOrEmpty(otherTermText))
                                    {
                                        if (otherTermText.Length == 1 && !char.IsLetterOrDigit(otherTermText[0]))
                                            continue;
                                        else break;
                                    }
                                }
                                if (!string.IsNullOrEmpty(otherTermText))
                                    otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(otherTermText);
                                if (!string.IsNullOrEmpty(otherTermText))
                                    newTermText = currentObject.Term.Name + " " + otherTermText;
                            }

                        }
                        if (string.IsNullOrEmpty(otherTermText) && index >= 0)
                        {
                            //Xử lý tìm từ tiếp theo nếu từ trước đó trống
                            var otherContent = content.Substring(index).Trim();
                            index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, otherContent);
                        }
                        if (!string.IsNullOrEmpty(otherTermText) && !string.IsNullOrEmpty(newTermText))
                        {
                            foreach (var term in currentObject.Term.Video.TermList)
                            {
                                if (newTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Thuật ngữ đã tồn tại: " + term.Name, InformationType.Error);
                                    return;
                                }
                                else if (otherTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    existedTerm = term;
                                    //break;
                                }
                            }
                            if (existedTerm != null)
                                break;
                        }
                    }
                }
                //}              
            }
            //2024-08-09: code cũ
            //var element = currentObject.GetAudioFromElement();
            //if (element != null && !string.IsNullOrEmpty(element.Content))
            //{
            //    string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
            //    var contents = element.Content.Split(newLineText, System.StringSplitOptions.RemoveEmptyEntries);
            //    string content = currentObject.GetSentenceTextFromContent(element.Content);                
            //    if (string.IsNullOrEmpty(content))
            //    {
            //        foreach (var tempContent in contents)
            //        {
            //            var index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, tempContent, parrentTerms.ToArray());
            //            if (index >= 0)
            //            {
            //                content = tempContent;
            //            }
            //        }
            //    }    
            //    if (!string.IsNullOrEmpty(content))
            //    {
            //        var audioContent = content.Trim();
            //        var index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, audioContent, parrentTerms.ToArray());
            //        if (index < 0)
            //        {
            //            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy vị trí thuật vị trong câu", InformationType.Error);
            //            return;
            //        }
            //        while (string.IsNullOrEmpty(otherTermText) && index >= 0)
            //        {
            //            if (index >= 0)
            //            {
            //                //Cố tìm từ đúng
            //                if (e.SelectedChoiceActionItem.Id.Contains("Previous"))
            //                {
            //                    if (index == 0)
            //                        index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, audioContent.Substring(1));
            //                    if (index >= 0)
            //                    {
            //                        //Bỏ qua dấu cách trước đó
            //                        for (int j = index - 1; j >= 0; j--)
            //                        {
            //                            if (char.IsLetterOrDigit(audioContent[j]) || audioContent[j] == '.' || audioContent[j] == ',')
            //                            {
            //                                otherTermText = audioContent[j] + otherTermText;
            //                            }
            //                            else if (!string.IsNullOrEmpty(otherTermText))
            //                            {
            //                                if (otherTermText.Length == 1 && !char.IsLetterOrDigit(otherTermText[0]))
            //                                    continue;
            //                                else break;
            //                            }
            //                        }
            //                        if (!string.IsNullOrEmpty(otherTermText))
            //                            otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(otherTermText);
            //                        if (!string.IsNullOrEmpty(otherTermText))
            //                            newTermText = otherTermText + " " + currentObject.Term.Name;
            //                    }
            //                }
            //                else if (e.SelectedChoiceActionItem.Id.Contains("Next"))
            //                {
            //                    //Thêm cả khoảng cách đằng sau từ tìm thấy
            //                    index += currentObject.Term.Name.Length;
            //                    if (index < audioContent.Length - 1)
            //                    {
            //                        //Bỏ qua dấu cách trước đó
            //                        for (int j = index + 1; j < audioContent.Length; j++)
            //                        {
            //                            if (char.IsLetterOrDigit(audioContent[j]) || audioContent[j] == '.' || audioContent[j] == ',')
            //                            {
            //                                otherTermText += audioContent[j];
            //                            }
            //                            else if (!string.IsNullOrEmpty(otherTermText))
            //                            {
            //                                if (otherTermText.Length == 1 && !char.IsLetterOrDigit(otherTermText[0]))
            //                                    continue;
            //                                else break;
            //                            }
            //                        }
            //                        if (!string.IsNullOrEmpty(otherTermText))
            //                            otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(otherTermText);
            //                        if (!string.IsNullOrEmpty(otherTermText))
            //                            newTermText = currentObject.Term.Name + " " + otherTermText;
            //                    }

            //                }
            //                if (string.IsNullOrEmpty(otherTermText) && index >= 0)
            //                {
            //                    //Xử lý tìm từ tiếp theo nếu từ trước đó trống
            //                    var otherContent = audioContent.Substring(index).Trim();
            //                    index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Term.Name, otherContent);
            //                }
            //                if (!string.IsNullOrEmpty(otherTermText) && !string.IsNullOrEmpty(newTermText))
            //                {
            //                    foreach (var term in currentObject.Term.Video.TermList)
            //                    {
            //                        if (newTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
            //                        {
            //                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Thuật ngữ đã tồn tại: " + term.Name, InformationType.Error);
            //                            return;
            //                        }
            //                        else if (otherTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
            //                        {
            //                            existedTerm = term;
            //                            //break;
            //                        }
            //                    }
            //                    if (existedTerm != null)
            //                        break;
            //                }
            //            }
            //        }
            //    }              
            //}
            
            if (!string.IsNullOrEmpty(newTermText))
            {
                if (!string.IsNullOrEmpty(otherTermText) && !string.IsNullOrEmpty(newTermText))
                {
                    foreach (var term in currentObject.Term.Video.TermList)
                    {
                        if (newTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Thuật ngữ đã tồn tại: " + term.Name, InformationType.Error);
                            return;
                        }
                        else if (otherTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                        {
                            existedTerm = term;
                            break;
                        }
                    }
                }
                var tempTerm = ObjectSpace.CreateObject<Term>();
                var currentTerm = currentObject.Term;
                tempTerm.Video = currentObject.Term.Video;
                currentObject.Term.Video.TermList.Add(tempTerm);
                tempTerm.Quantity = null;                
                tempTerm.Name = newTermText;
                //2023-08-09: thuật ngữ là thuật ngữ của từ được chọn
                //tempTerm.TermType = TermType.MergeTerm;
                tempTerm.TermType = currentObject.Term.TermType;
                if (View.CurrentObject != tempTerm)
                    View.CurrentObject = tempTerm;
                //Cập nhật thuật vị
                //2023-07-22 Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
                termService.UpdatePosition(tempTerm, true);
                if (tempTerm.Quantity is null && tempTerm.Quantity == (int)0)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thuật ngữ: " + newTermText, InformationType.Error, 5000);
                    ObjectSpace.Delete(tempTerm);
                }
                else
                {
                    //Dịch ngữ cảnh
                    //TranslateTerm.DoExecute(new ChoiceActionItem("TranslateTermContextApostrophe", "TranslateTermContextApostrophe", "TranslateTermContextApostrophe"));
                    int termLocationCount = 0;
                    int susscess = 0;
                    termService.TranslateTerm(tempTerm, ref termLocationCount, ref susscess);
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Tìm thấy " + tempTerm.Quantity + " thuật ngữ: " + newTermText, InformationType.Info, 5000);
                    //Giảm trừ số lượng
                    //Vì dùng hàm tempTerm.UpdatePosition(true); ở trên nên không cần giảm trừ số lượng
                    //currentTerm.Quantity -= tempTerm.Quantity;                    
                    if (!(currentTerm.Quantity > 0))
                        ObjectSpace.Delete(currentTerm);
                    else
                    {
                        System.Collections.Generic.List<TermLocation> deletedList = new System.Collections.Generic.List<TermLocation>();
                        //Nếu là liền trước thì vị trí trước 1 từ, liền sau thì vị trí là vị trí này
                        var positionIndex = e.SelectedChoiceActionItem.Id.Contains("Previous") ? 1 : 0;
                        //Xóa các thuật vị thừa
                        foreach (var mergeTermLocation in tempTerm.TermLocationList)
                        {
                            if (mergeTermLocation.Location is null) continue;
                            foreach (var currentTermLocation in currentTerm.TermLocationList)
                            {                               
                                if (deletedList.Contains(currentTermLocation) || currentTermLocation.Location is null) continue;
                                if (mergeTermLocation.Audio.Oid == currentTermLocation.Audio.Oid)
                                {
                                    if(mergeTermLocation.Location == currentTermLocation.Location - positionIndex)
                                    {
                                        deletedList.Add(currentTermLocation);
                                    }
                                }
                            }
                        }
                        if(deletedList.Count != tempTerm.Quantity)
                        {
                            var term = currentTerm;
                            //2023-07-22 Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
                            //2024-08-01: Bỏ cập nhật thuật vị để tránh lỗi xóa máy dịch
                            //term.UpdatePosition(false);
                            //Máy dịch lại
                            termService.TranslateTerm(term, ref termLocationCount, ref susscess);
                        }
                        //Xóa các thành phần thừa
                        View.ObjectSpace.Delete(deletedList);
                    }                       
                    if (existedTerm != null)
                    {
                        //2023-07-22 Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
                        //2024-08-01: Bỏ cập nhật thuật vị để tránh lỗi xóa máy dịch
                        //existedTerm.UpdatePosition(false);
                        if (!(existedTerm.Quantity > 0) || existedTerm.TermLocationList?.Count == 0)
                            ObjectSpace.Delete(existedTerm);
                        else
                        {
                            System.Collections.Generic.List<TermLocation> deletedList = new System.Collections.Generic.List<TermLocation>();
                            //Nếu là liền trước thì vị trí trước 1 từ, liền sau thì vị trí là vị trí này
                            var positionIndex = e.SelectedChoiceActionItem.Id.Contains("Previous") ? 0 : 1;
                            //Xóa các thuật vị thừa
                            foreach (var mergeTermLocation in tempTerm.TermLocationList)
                            {
                                if (mergeTermLocation.Location is null) continue;
                                foreach (var currentTermLocation in existedTerm.TermLocationList)
                                {
                                    if (deletedList.Contains(currentTermLocation) || currentTermLocation.Location is null) continue;
                                    if (mergeTermLocation.Audio.Oid == currentTermLocation.Audio.Oid)
                                    {
                                        if (mergeTermLocation.Location == currentTermLocation.Location - positionIndex)
                                        {
                                            deletedList.Add(currentTermLocation);
                                        }
                                    }
                                }
                            }
                            if (deletedList.Count > 0)
                                View.ObjectSpace.Delete(deletedList);
                            termService.TranslateTerm(existedTerm, ref termLocationCount, ref susscess);
                        }
                            
                    }
                    
                }
                if(!currentObject.IsDeleted)
                    ObjectSpace.Delete(currentObject);
                View.CurrentObject = currentObject;
            }
            else
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy Thuật ngữ liền kề", InformationType.Error, 5000);
            }




            #endregion MergeTermAdjacentPositionImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0917            Oid: 3be5ba0b-bfdf-40b5-8317-05af5972c54e
		private void OpenTermLocationElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OpenTermLocationElement), "Mở thành phần");              
      
            #region OpenTermLocationElementImportCode
            var currentObject = View.CurrentObject as Module.BusinessObjects.TermLocation;
            if (currentObject is null)
                return;
            var element = currentObject.GetAudioFromElement();
            if(element is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thành phần", InformationType.Error);
                return;
            }
            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = false;
            var showViewParameters = new ShowViewParameters
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                NewWindowTarget = NewWindowTarget.Separate,
                Context = TemplateContext.View
            };
            showViewParameters.Controllers.Add(dc);
            Module.SystemObjects.ReplaceObject replaceObject = new Module.SystemObjects.ReplaceObject();
            showViewParameters.CreatedView = Application.CreateDetailView(View.ObjectSpace, element, false);

            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction)); 

            #endregion OpenTermLocationElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1046            Oid: 8fea07bf-91d8-47e8-a022-e2bd812d3d80
		private void SpellingTermLocation_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SpellingTermLocation), "Chính tả");              
      
            #region SpellingTermLocationImportCode
            Module.BusinessObjects.Video video = null;
            Module.BusinessObjects.Term term = null;
            var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
            if(masterObject is Module.BusinessObjects.Video)
                video = masterObject as Module.BusinessObjects.Video;
            else
            {
                term = masterObject as Module.BusinessObjects.Term;
                if (term != null && term.Video != null)
                    video = term.Video;
            }
           

            if (video is null && View.CurrentObject is Module.BusinessObjects.TermLocation currentTermLocation)
            {
                if(currentTermLocation.Term != null && currentTermLocation.Term.Video != null)
                    video = currentTermLocation.Term.Video;
                else if (currentTermLocation.Audio != null && currentTermLocation.Audio.Video != null)
                    video = currentTermLocation.Audio.Video;
            }
            if (video is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy tư liệu", InformationType.Error, 10000);
            }
            

            var termLocationList = View.SelectedObjects.Cast<Module.BusinessObjects.TermLocation>().ToList();
            
            if (e.SelectedChoiceActionItem.Id.Equals("ConfirmTerm") || e.SelectedChoiceActionItem.Id.Equals("NotTerm"))
            {
                //if(term is null)
                //{
                //    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chức năng này không khả dụng", InformationType.Error, 10000);
                //    return;
                //}
                int deleteTerm = 0;                

                foreach (var termLocation in termLocationList)
                {
                    deleteTerm = termLocationService.ConfirmOrNotTerm(termLocation, e.SelectedChoiceActionItem.Id.Equals("ConfirmTerm"), deleteTerm, false);                    
                }
                if (e.SelectedChoiceActionItem.Id.Equals("NotTerm"))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", termLocationList.Count + " thuật vị bị xóa", InformationType.Info, 10000);
                    if (term != null && term.TermLocationList.Count == 0)
                        term.Session.Delete(term);

                }
                else
                {
                    if (deleteTerm > 0)
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteTerm + " thuật ngữ bị xóa", InformationType.Info, 10000);
                }
                if (term != null && !term.IsDeleted)
                    term.Overlap = term.GetDefaultOverlap();
                return;
            }
            
            if (video.LanguageOrigin is null || string.IsNullOrEmpty(video.LanguageOrigin?.Code))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy ngôn ngữ gốc", InformationType.Error);
                return;
            }
            var dictionary = video.GetDictionary();
            if (dictionary is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy từ điển", InformationType.Error);
                return;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("CancelWrongTerm"))
            {
                //if (term is null)
                //{
                //    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chức năng này khòng khả dụng", InformationType.Error, 10000);
                //    return;
                //}
                //Loại bên sai
                //var termList = new System.Collections.Generic.List<Module.BusinessObjects.Term>();
                //foreach (Module.BusinessObjects.Term termSelect in View.SelectedObjects)
                //    if (!string.IsNullOrEmpty(termSelect.Name))
                //        termList.Add(termSelect);
                
                int deleteTerm = 0;
                int deleteTermLocation = 0;                
                bool currentIsCorrect = term != null ? Module.Helpers.TextHelper.CheckWordIsCorrect(dictionary, term.Name) : false;
                if (currentIsCorrect)
                {
                    //Loại bỏ bên sai
                    foreach (var termLocation in termLocationList)
                    {
                        var overlapList = TermLocationService.GetOverlap(termLocation, false);
                        if (overlapList is null)
                            continue;
                        foreach (var overlapTermLocation in overlapList)
                        {
                            if (overlapTermLocation.Term != null && !string.IsNullOrEmpty(overlapTermLocation.Term.Name))
                            {
                                var overlapIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(dictionary, overlapTermLocation.Term.Name);
                                if (!overlapIsCorrect)
                                {
                                    //Loại bỏ bên sai
                                    if (overlapTermLocation.Term.Quantity == 1 || overlapTermLocation.Term.TermLocationList.Count == 1)
                                    {
                                        overlapTermLocation.Term.Delete();
                                        deleteTerm++;
                                    }
                                    else
                                        overlapTermLocation.Term.Quantity--;
                                    overlapTermLocation.Delete();
                                    deleteTermLocation++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Kiểm tra xem nếu có bên đúng
                    if (termLocationList.Count == 0)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thuật vị", InformationType.Error, 5000);
                        return;
                    }
                    foreach (var termLocation in termLocationList)
                    {
                        var overlapList = TermLocationService.GetOverlap(termLocation, false);
                        if (overlapList is null)
                            continue;
                        foreach (var overlapTermLocation in overlapList)
                        {
                            string overlapTermName = overlapTermLocation.Term != null ? overlapTermLocation.Term.Name: overlapTermLocation.MachineTranslate;                            
                            if (!string.IsNullOrEmpty(overlapTermName))
                            {
                                var overlapIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(dictionary, overlapTermName);
                                if (overlapIsCorrect)
                                {
                                    //Xóa thuật vị này
                                    termLocation.Delete();
                                    deleteTermLocation++;
                                }
                            }
                        }
                    }
                    if (termLocationList.Count == 0)
                    {
                        //Loại bỏ bên sai
                        if (term != null)
                            term.Delete();
                        deleteTerm++;
                    }
                }                
                if (deleteTerm > 0 || deleteTermLocation > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteTerm + " thuật ngữ bị xóa \r\n" + deleteTermLocation + " thuật vị bị xóa", InformationType.Info, 10000);
                return;
            }            
            else if (e.SelectedChoiceActionItem.Id.StartsWith("Correct"))
            {
                using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                {
                    ShowViewParameters showViewParameters = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        CreateAllControllers = true,
                        NewWindowTarget = NewWindowTarget.Separate,
                        Context = TemplateContext.View,
                    };
                    showViewParameters.Controllers.Add(dc);
                    System.Type type = typeof(Module.BusinessObjects.TermLocationCorrection);
                    string viewId = Application.FindListViewId(type);
                    //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                    if (string.IsNullOrEmpty(viewId))
                        return;
                    //var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
                           type, viewId, CollectionSourceMode.Normal);
                    //2024-09-16: Làm trên thuật vị thì chỉ thay đổi thuật vị
                    //if(term != null)
                    //{
                    //    if (string.IsNullOrEmpty(term.Name))
                    //        return;
                    //    var termlowerName = term.Name.ToLower();
                    //    var termNameLength = term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    //    var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(termlowerName);

                    //    var termCorrection = new TermCorrection(term.Session);
                    //    termCorrection.Term = term;
                    //    //termCorrection.Caption = term.Name;
                    //    collectionSource.Add(termCorrection);
                    //    if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                    //    {
                    //        //var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                    //        //foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                    //        //{
                    //        //    //Kiểm tra từng thuật vị                                    
                    //        //    var correctionOption = new CorrectionOption(term.Session);
                    //        //    correctionOption.Name = sugget;
                    //        //    correctionOption.TermCorrection = termCorrection;
                    //        //    correctionOptions.Add(correctionOption);
                    //        //}
                    //        //termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(term.Session, correctionOptions);
                    //        var termLocationCorrections = new System.Collections.Generic.List<TermLocationCorrection>();
                    //        foreach (var termLocation in term.TermLocationList)
                    //        {
                    //            var termLocationCorrection = new TermLocationCorrection(term.Session);
                    //            //termLocationCorrection.
                    //            termLocationCorrection.TermLocation = termLocation;

                    //            var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                    //            foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                    //            {
                    //                var correctionOption = new CorrectionOption(term.Session);
                    //                correctionOption.Name = sugget;
                    //                correctionOption.TermLocationCorrection = termLocationCorrection;
                    //                correctionOptions.Add(correctionOption);

                    //            }
                    //            termLocationCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(term.Session, correctionOptions);

                    //            termLocationCorrections.Add(termLocationCorrection);
                    //        }
                    //        termCorrection.TermLocationCorrectionList = new DevExpress.Xpo.XPCollection<TermLocationCorrection>(term.Session, termLocationCorrections);
                    //    }
                    //}
                    //else
                    //{
                    //    foreach(var termLocation in termLocationList)
                    //    {
                    //        if (string.IsNullOrEmpty(termLocation.MachineTranslate))
                    //            continue;
                    //        var termlowerName = termLocation.MachineTranslate.ToLower();
                    //        var termNameLength = termLocation.MachineTranslate.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    //        var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(termlowerName);

                    //        var termLocationCorrection = new TermLocationCorrection(termLocation.Session);
                    //        //termCorrection.Caption = termLocation.MachineTranslate;
                    //        termLocationCorrection.TermLocation = termLocation;
                    //        collectionSource.Add(termLocationCorrection);
                    //        if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                    //        {
                    //            var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                    //            foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                    //            {
                    //                //Kiểm tra từng thuật vị                                    
                    //                var correctionOption = new CorrectionOption(termLocation.Session);
                    //                correctionOption.Name = sugget;
                    //                correctionOption.TermLocationCorrection = termLocationCorrection;
                    //                correctionOptions.Add(correctionOption);
                    //            }
                    //            termLocationCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termLocation.Session, correctionOptions);
                    //        }                        
                    //    }
                    //}

                    foreach (var termLocation in termLocationList)
                    {
                        string termName = termLocation.Term != null ? termLocation.Term.Name : termLocation.MachineTranslate;
                        if (string.IsNullOrEmpty(termName))
                            continue;
                        var termLowerName = termName.ToLower();
                        var termNameLength = termName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                        var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(termLowerName);

                        var termLocationCorrection = new TermLocationCorrection(termLocation.Session);
                        //termCorrection.Caption = termName;
                        termLocationCorrection.TermLocation = termLocation;
                        collectionSource.Add(termLocationCorrection);
                        if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                        {
                            termLocationCorrection.AddTermLocation(termLocation, dictionary[termNameLength][termNoneUnicode]);                            
                        }
                    }

                    var listview = Application.CreateListView(viewId, collectionSource, false);
                    listview.AllowNew["Popup"] = false;
                    //dc.AcceptAction.Caption = "Chọn " + caption;
                    dc.AcceptAction.Active.SetItemValue("", false);
                    dc.SaveOnAccept = false;
                    dc.CancelAction.Active.SetItemValue("", false);
                    showViewParameters.CreatedView = listview;
                    Application.ShowViewStrategy.ShowView(showViewParameters,
                        new ShowViewSource(Frame, dc.AcceptAction));
                }
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("SelectFirstOption"))
            {
                if (term != null)
                {
                    if (string.IsNullOrEmpty(term.Name))
                        return;
                    var termNameLength = term.Name.Split(' ').Length;
                    var termlowerName = term.Name.ToLower();
                    var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(termlowerName);
                    if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                    {
                        var replaceWord = dictionary[termNameLength][termNoneUnicode][0];
                        var existedTerm = TermService.FindTermByName(term, replaceWord);
                        foreach (Module.BusinessObjects.TermLocation termLocation in termLocationList)
                        {
                            if (existedTerm is null)
                                existedTerm = TermService.FindTermByName(term, replaceWord);
                            TermLocationService.ReplaceWord(termLocation, replaceWord, false, existedTerm);
                        }
                    }
                }
                else
                {
                    foreach (var termLocation in termLocationList)
                    {
                        if (string.IsNullOrEmpty(termLocation.MachineTranslate))
                            continue;
                        var termNameLength = termLocation.MachineTranslate.Split(' ').Length;
                        var termlowerName = termLocation.MachineTranslate.ToLower();
                        var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(termlowerName);
                        if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                        {
                            var replaceWord = dictionary[termNameLength][termNoneUnicode][0];
                            TermLocationService.ReplaceWord(termLocation, replaceWord, false);
                        }
                    }
                }
                
            }

            #region bỏ cấu trúc cũ
            //string aff = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryAffVN.aff";
            //string dic = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryVN.dic";
            //var term = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Term;
            //if (term is null)
            //    return;
            //using (NHunspell.Hunspell hunspell = new NHunspell.Hunspell(aff, dic))
            //{
            //    var dictionariesText = Module.Helpers.ParameterHelper.GetValueOrDefault(ObjectSpace, "ViDictionaries", "THPT,THCS");
            //    if (!string.IsNullOrEmpty(dictionariesText))
            //    {
            //        var dictionaries = dictionariesText.Split(',');
            //        foreach (var dictionary in dictionaries)
            //            hunspell.Add(dictionary.Trim());
            //    }
            //    //084: -Trên Thuật vị(Independent) Dựng cờ thuật vị sai chính tả, dựng cờ Thuật ngữ nếu tồn tại Thuật vị sai chính tả//
            //    //foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
            //    foreach (Module.BusinessObjects.TermLocation termLocation in term.TermLocationList)
            //    {
            //        if (termLocation.CheckSpellingFlag(hunspell))
            //            termLocation.Term.Flag = true;
            //    }
            //}
            #endregion









            #endregion SpellingTermLocationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1398            Oid: d0809b50-9758-448d-aeaf-177e9386bbe9
		private void TermLocationFlag_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TermLocationFlag), "Cờ thuật vị");              
      
            #region TermLocationFlagImportCode

            if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck"))
            {
                //2024-12-21: Cờ đè xử lý cho trường Overlap
                foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    var overlap = TermLocationService.CheckOverlap(termLocation, false);
                    if (!termLocation.Overlap.Equals(overlap))
                        termLocation.Overlap = overlap;
                }                
                return;                
            }
            char charTag = '<';
            if (e.SelectedChoiceActionItem.Id.Equals("DuplicatedTranslate") 
                || e.SelectedChoiceActionItem.Id.Equals("NotExist")
                || e.SelectedChoiceActionItem.Id.Equals("OverlapCheck"))
            {
                foreach(Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                {
                    var flag = false;
                    //Xóa Note nếu có dữ liệu
                    if (!string.IsNullOrEmpty(termLocation.Note))
                        termLocation.Note = Module.Helpers.TextHelper.GetTextWithTagNode(termLocation.Note, charTag, false);
                    if (e.SelectedChoiceActionItem.Id.Equals("DuplicatedTranslate"))
                    {                        
                        if (termLocation.Audio is null || termLocation.Term is null ||
                            string.IsNullOrEmpty(termLocation.Audio.Subtitle) || string.IsNullOrEmpty(termLocation.MachineTranslate))
                        {
                            if (termLocation.Flag)
                                termLocation.Flag = flag;
                            continue;
                        }
                        var content = TermLocationService.GetSentenceTextFromContent(termLocation, termLocation.Audio.Content);
                        var firstIndex = Module.Helpers.TextHelper.GetIndexWordInContent(termLocation.MachineTranslate, content);
                        if (firstIndex >= 0)
                        {
                            var secondIndex = Module.Helpers.TextHelper.GetIndexWordInContent(termLocation.MachineTranslate, content, null, firstIndex + 1);
                            if (secondIndex > 0)
                            {
                                flag = true;
                            }
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("NotExist"))
                    {
                        if (termLocation.Audio is null || string.IsNullOrEmpty(termLocation.Audio.Content) ||
                            termLocation.Sentence is null || termLocation.Location is null)
                        {
                            if (termLocation.Flag)
                                termLocation.Flag = flag;
                            continue;
                        }
                        var sentenceText = TermLocationService.GetSentenceTextFromContent(termLocation, termLocation.Audio.Content);
                        //Kiểm tra theo thuật vị
                        string termName = (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Name)) ? termLocation.Term.Name : termLocation.MachineTranslate;
                        if(!string.IsNullOrEmpty(termName))
                        {
                            var index = sentenceText.IndexOf(termName);
                            int location = -1;
                            while (index >= 0 && location < termLocation.Location && index < sentenceText.Length - 1)
                            {
                                location = sentenceText.Substring(0, index).Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                                index = sentenceText.IndexOf(termName, index + 1);
                            }
                            if(location != termLocation.Location)
                                flag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck"))
                    {
                        flag = TermLocationService.CheckOverlap(termLocation, false);
                    }
                    if (flag == true)
                        termLocation.Note = Module.Helpers.TextHelper.AddTextWithTagNode(termLocation.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    if (!termLocation.Flag.Equals(flag))
                        termLocation.Flag = flag;
                }
                return;
            }
            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.AcceptAction.Caption = e.SelectedChoiceActionItem.Caption;
            dc.SaveOnAccept = true;
            var showViewParameters = new ShowViewParameters
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                NewWindowTarget = NewWindowTarget.Separate
            };
            dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
            {
                if (args.AcceptActionArgs.CurrentObject is Module.SystemObjects.InsertText)
                {
                    var insertTextControl = (Module.SystemObjects.InsertText)args.AcceptActionArgs.CurrentObject;
                    if (string.IsNullOrEmpty(insertTextControl.Word))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Từ không được phép trống", InformationType.Error);
                        return;
                    }
                   
                    foreach (Module.BusinessObjects.TermLocation termLocation in View.SelectedObjects)
                    {
                        if(termLocation.Flag)
                            termLocation.Flag = false;
                        //Xóa Note nếu có dữ liệu
                        if (!string.IsNullOrEmpty(termLocation.Note))
                            termLocation.Note = Module.Helpers.TextHelper.GetTextWithTagNode(termLocation.Note, charTag, false);
                        if (termLocation.Audio is null || termLocation.Term is null || string.IsNullOrEmpty(termLocation.Audio.Content))
                            continue;
                        var sentenceText = TermLocationService.GetSentenceTextFromContent(termLocation, termLocation.Audio.Content);
                        if (e.SelectedChoiceActionItem.Id.Equals("Inner"))
                        {
                            var index = Module.Helpers.TextHelper.GetIndexWordInContent(insertTextControl.Word, sentenceText, TermService.GetParrentTerms(termLocation.Term).ToArray());
                            if (index >= 0)
                            {
                                termLocation.Flag = true;
                            }
                        }
                        else
                        {
                            var index = termLocationService.GetIndexByLocation(termLocation, sentenceText, termLocation.Term.Name);
                            if (index >= 0)
                            {
                                //Phải tìm thấy từ
                                if (e.SelectedChoiceActionItem.Id.Equals("Previous"))
                                {
                                    var otherText = sentenceText.Substring(0, index).TrimEnd();
                                    if(otherText.EndsWith(insertTextControl.Word, System.StringComparison.OrdinalIgnoreCase))
                                        termLocation.Flag = true;

                                }
                                else if (e.SelectedChoiceActionItem.Id.Equals("Next"))
                                {
                                    var newIndex = index + 1 + termLocation.Term.Name.Length;
                                    if (newIndex < sentenceText.Length)
                                    {
                                        var otherText = sentenceText.Substring(newIndex).TrimStart();
                                        if (otherText.StartsWith(insertTextControl.Word, System.StringComparison.OrdinalIgnoreCase))
                                            termLocation.Flag = true;
                                    }
                                }
                            }
                        }
                        if (termLocation.Flag == true)
                            termLocation.Note = Module.Helpers.TextHelper.AddTextWithTagNode(termLocation.Note, charTag, e.SelectedChoiceActionItem.Caption);
                    }
                    
                }
            };
            showViewParameters.Controllers.Add(dc);
            Module.SystemObjects.InsertText insertText = new Module.SystemObjects.InsertText();
            showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), insertText, true);
            showViewParameters.Context = TemplateContext.PopupWindow;

            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));



            #endregion TermLocationFlagImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}