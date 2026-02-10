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
    public partial class TermViewController: BaseViewController<Module.BusinessObjects.Term>
    {      
        
        public TermViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Term);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
     
        private TermLocationService termLocationService;
        protected TermLocationService _termLocationService => termLocationService ??= new TermLocationService(this);        
         
        private VideoService videoService;
        protected VideoService _videoService => videoService ??= new VideoService(this);        
      
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.TermService termService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             termService = new Module.Services.TermService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1497            Oid: 3bf85fb5-82e2-4340-bd2f-222c8809d884
		private void OverlapTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OverlapTerm), "Thuật ngữ đè");              
      
            #region OverlapTermImportCode
            int termCount = 0, termLocationCount = 0;
            foreach (Module.BusinessObjects.Term term in View.SelectedObjects.Cast<Module.BusinessObjects.Term>().ToList())
            {
                foreach (var termLocation in term.TermLocationList)
                {
                    termLocationService.OverlapTermPosition(termLocation, e.SelectedChoiceActionItem.Id, ref termCount, ref termLocationCount);
                    //Kiểm tra thuật ngữ TN2 đè 1 từ với thuật vị đầu tiên của TN1 (theo trái phải và độ đài từ, đè 1 từ, chấp nhận cả TN1 là từ đơn
                    break;
                }
            }
            string message = "Có " + termCount + " thuật ngữ được xử lý";
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", message);



            #endregion OverlapTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0945            Oid: 0c9ecf62-b559-4b72-8ccf-a47da36db1b8
		private void EditWordTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(EditWordTerm), "Soạn thảo");              
      
            #region EditWordTermImportCode
//Mở hộp thoại nhập giá trị cần chèn, nếu thuật ngữ là đầu câu và chèn trước thì Viết hoa từ chèn và bỏ hoa thuật ngữ,
            //      áp dụng mọi thuật vị kể cả chưa dịch hoặc chưa thay từ 
            //Chức năng tương tự trên Thuật ngữ, sẽ thực hiện mọi thuật vị thuộc thuật ngữ được chọn,
            //  nhưng chỉ SingleChoice k như Thuật vị là Multichoice
            var term = View.CurrentObject as Module.BusinessObjects.Term;
            if (term is null) return;
            if (!term.TermLocationList.Count.Equals(term.Quantity))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Số lượng thuật vị không khớp, vui lòng cập nhật thuật vị trước khi sử dụng tính năng này", InformationType.Warning, 10000);
                return;
            }
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
                        foreach (Module.BusinessObjects.TermLocation termLocation in term.TermLocationList)
                        {
                            result += termLocationService.InsertWord(termLocation, term.TermLocationList.Count, e.SelectedChoiceActionItem.Id.Equals("InsertBefore"), insertTextControl.Word);
                        }
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + term.TermLocationList.Count + " thuật vị được xử lý", InformationType.Info, 10000);
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
                foreach (Module.BusinessObjects.TermLocation termLocation in term.TermLocationList)
                {
                    result += termLocationService.DeleteWord(termLocation, term.TermLocationList.Count, e.SelectedChoiceActionItem.Id.Equals("DeleteBefore"));
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + term.TermLocationList.Count + " thuật vị được xử lý", InformationType.Info, 10000);
            }


            #endregion EditWordTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0942            Oid: 13b7ded1-927b-44e2-b1c1-ddaf265b0b07
		private void Dictionary_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(Dictionary), "Tra từ điển");              
      
            #region DictionaryImportCode
            var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null) return;
            if (video.LanguageOrigin is null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Ngữ gốc bị trống", InformationType.Error, 10000);
                return;
            }
            if (video.LanguageTranslate is null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Ngữ dịch bị trống", InformationType.Error, 10000);
                return;
            }
            char charTag = '{';
            if (e.SelectedChoiceActionItem.Id.Equals("WordMatch"))
            {
                //char charTag = '[';
                //Chức năng Tra từ điển > Từ vựng giống : Nạp danh sách vào ghi chú
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    var likeTermList = term.GetLikeWordList();
                    if(likeTermList != null)
                    {
                        term.AddTextNode(charTag, string.Join(", ", likeTermList), false);
                    }
                }
                return;
            }
            //Chọn 1 từ điển để tra rồi nạp các thuật ngữ của từ điển có tồn tại trong bài vào tab Thuật ngữ                
            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                        Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
                {
                    if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                        ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                    {
                        ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                    }
                };
                ShowViewParameters showViewParameters = new ShowViewParameters()
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    Context = TemplateContext.LookupWindow,
                };

                showViewParameters.Controllers.Add(dc);
                System.Type type = typeof(Module.BusinessObjects.Dictionary);
                string viewId = Application.FindLookupListViewId(type);
                //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                if (string.IsNullOrEmpty(viewId))
                    return;
                var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(dictionaryObjectSpace,
                        type, viewId, CollectionSourceMode.Normal);
                var listview = Application.CreateListView(viewId, collectionSource, false);
                //dc.AcceptAction.Caption = "Chọn " + caption;
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    if (args.AcceptActionArgs.CurrentObject is Dictionary)
                    {
                        var dictionary = (Dictionary)args.AcceptActionArgs.CurrentObject;
                        //string[] SuffixList = new string[] { "ed", "er", "ing","es", "er", "s", "d", "ly", "th" };

                        //2023-08-04 
                        //Lấy giá trị dịch ngữ ưu tiên 1 để đưa vào Dịch của Thuật vị có dịch máy khác, và của Thuật ngữ nếu tồn tại Thuật vị được dịch
                        //Đưa toàn bộ danh sách Dịch tra từ Dịch ngữ vào trường Từ điển của Thuật ngữ theo thứ tự ưu tiên và ngăn cách bằng phẩy hoặc chấm phẩy
                        //Trường Dịch của Thuật ngữ và Thuật vị sẽ sổ Combo chọn các giá trị trong danh sách trên
                        //char charTag = '{';
                        foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                        {
                            if (string.IsNullOrEmpty(term.Name))
                                continue;
                            if (e.SelectedChoiceActionItem.Id.Equals("Matching"))
                            {
                                var dictionaryWord = termService.GetDictionaryWord(term.Name, video.LanguageOrigin, dictionary);
                                if (dictionaryWord != null)
                                {
                                    //Matching là khớp hoàn toàn và sẽ cập nhật giá trị Dịch ngữ vào thuật vị cũng như Dhch của Thuật ngữ > chuyển trạng thái Từ điển
                                    term.TermType = TermType.Dictionary;
                                    var translateWordList = dictionaryWord.TranslateWordList
                                          .Where(m => !string.IsNullOrEmpty(m.Name) && m.Language != null && m.Language.Oid.Equals(video.LanguageTranslate.Oid))
                                                 .OrderBy(m => m.Order).ToList();
                                    if (translateWordList.Count > 0)
                                    {
                                        term.Translate = translateWordList[0].Name;
                                        foreach (var translateWord in translateWordList)
                                        {
                                            //Đưa toàn bộ danh sách Dịch tra từ Dịch ngữ vào trường Từ điển của Thuật ngữ theo thứ tự ưu tiên và ngăn cách bằng phẩy hoặc chấm phẩy
                                            //2023-08-04 Đưa các giá trị từ điển vào trong móc vuông[]
                                            term.Note += "[" + translateWord.Name + "]";
                                        }
                                    }
                                    else
                                    {
                                        term.Translate = dictionaryWord.Translate;
                                    }
                                }
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("ContainTerm"))
                            {
                                //Bao thuật ngữ
                                //Contain(bao gồm) khi Thuật ngữ bao Từ ngữ hoặc ngược lại thì sẽ dựng cờ Thuật ngữ để xem xét
                                var criteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.Parse(
                                            "(StartsWith([Name], ?) Or EndsWith([Name], ?) Or Contains([Name], ?)) And LanguageOrigin.Oid = ?", 
                                            term.Name + " ", " " + term.Name, " " + term.Name + " ", video.LanguageOrigin.Oid);
                                criteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.Or(criteriaOperator,
                                    DevExpress.Data.Filtering.CriteriaOperator.Parse(
                                        "TranslateWordList[(StartsWith([Name], ?) Or EndsWith([Name], ?) Or Contains([Name], ?)) And Language.Oid = ?]", 
                                        term.Name + " ", " " + term.Name, " " + term.Name + " ", video.LanguageOrigin.Oid));
                                criteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.And(criteriaOperator,
                                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Dictionary.Oid = ?", dictionary.Oid));
                                var dictionaryWord = term.Session.FindObject<Module.BusinessObjects.DictionaryWord>(criteriaOperator);
                                if(dictionaryWord != null)
                                {
                                    //Từ ngữ bao gồm thuật ngữ
                                    term.Flag = true;
                                    if (!string.IsNullOrEmpty(term.Note))
                                    {
                                        //Xóa ghi chú tag trước đó
                                        term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                                    }
                                    term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, e.SelectedChoiceActionItem.Caption);                                    
                                }                                
                            }
                            else if (e.SelectedChoiceActionItem.Id.Equals("TermContain"))
                            {
                                //Thuật ngữ bao
                                if (term.Name.Contains(" "))
                                {
                                    //Thuật ngữ bao gồm từ ngữ
                                    var termNames = term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);                                    
                                    foreach (string termName in termNames)
                                    {
                                        var dictionaryWord = termService.GetDictionaryWord(termName, video.LanguageOrigin, dictionary);
                                        if (dictionaryWord != null)
                                        {
                                            term.Flag = true;
                                            if (!string.IsNullOrEmpty(term.Note))
                                                term.Note += "; ";
                                            term.Note += e.SelectedChoiceActionItem.Caption;
                                            continue;
                                        }
                                    }                                    
                                }
                            }
                        }
                    };
                };
                dc.SaveOnAccept = false;
                dc.CancelAction.Active.SetItemValue("", false);
                showViewParameters.CreatedView = listview;
                Application.ShowViewStrategy.ShowView(showViewParameters,
                    new ShowViewSource(Frame, dc.AcceptAction));
            }        



            #endregion DictionaryImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0958            Oid: 10f2f478-f226-4c75-8aec-7a30f85d5440
		private void NumberValue_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(NumberValue), "Trị số");              
      
            #region NumberValueImportCode
            //Lấy giá trị tiếp theo của giá trị hiện tại trong NumberValue  trong danh sách số trích xuất từ trường Tên, nếu giá trị hiện tại k nằm trong số đó thì lấy giá trị đầu tiên
            foreach(Module.BusinessObjects.Term term in View.SelectedObjects)
            {
                if (term.NumberValue is null)
                    term.NumberValue = term.GetDefaultNumberValue();
                else if(!string.IsNullOrEmpty(term.Name))
                {
                    var numberText = term.NumberValue.Value.ToString();
                    //numberText = numberText.Replace(".", "").Replace(",", "");
                    if(term.Name.Length > numberText.Length)
                    {
                        for(int i = 0; i < term.Name.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(numberText) && !char.IsNumber(numberText[0]) && numberText[0] == term.Name[i])
                            {
                                numberText = numberText.Substring(1);
                            }
                            else
                            {
                                //Bắt đầu từ ký tự này
                                var newText = term.Name.Substring(i);
                                term.NumberValue = Module.SystemObjects.Tools.TryConvertTextToNumber(newText);
                                break;
                            }
                        }
                    }
                }
            }



            #endregion NumberValueImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1047            Oid: ea33a85e-6b05-4ea5-94b7-6813d845c9c9
		private void SpellingTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SpellingTerm), "Chính tả");              
      
            #region SpellingTermImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            decimal countNumber = 0;
            decimal total = View.SelectedObjects.Count;
            char charTag = '(';
            var termSelected = View.SelectedObjects.Cast<Module.BusinessObjects.Term>().ToList();
            if (e.SelectedChoiceActionItem.Id.Equals("ConfirmTerm") || e.SelectedChoiceActionItem.Id.Equals("NotTerm"))
            {
                int deleteTerm = 0;
                var termList = new System.Collections.Generic.List<Module.BusinessObjects.Term>();
                foreach (Module.BusinessObjects.Term termSelect in View.SelectedObjects)
                    termList.Add(termSelect);
                foreach (Module.BusinessObjects.Term termSelect in termList)
                {
                    if (e.SelectedChoiceActionItem.Id.Equals("ConfirmTerm"))
                        termSelect.Overlap = false;
                    foreach (var termLocation in termSelect.TermLocationList.ToList())
                    {
                        deleteTerm = termLocationService.ConfirmOrNotTerm(termLocation, e.SelectedChoiceActionItem.Id.Equals("ConfirmTerm"), deleteTerm, true);                        
                    }
                    if (e.SelectedChoiceActionItem.Id.Equals("NotTerm"))
                    {
                        termSelect.Delete();
                    }
                    countNumber++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), " ", stopWatch.Elapsed);
                }
                if(deleteTerm > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteTerm + " thuật ngữ bị xóa", InformationType.Info, 10000);
                stopWatch.Stop();
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                return;
            }
            if (video.LanguageOrigin is null || string.IsNullOrEmpty(video.LanguageOrigin?.Code))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy ngôn ngữ gốc", InformationType.Error);
                return;
            }
            var dictionary = video.GetDictionary();
            if(dictionary is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy từ điển", InformationType.Error);
                return;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("CancelWrongTerm"))
            {
                //Loại bên sai
                var termList = new System.Collections.Generic.List<Module.BusinessObjects.Term>();
                foreach (Module.BusinessObjects.Term termSelect in View.SelectedObjects)
                    if (!string.IsNullOrEmpty(termSelect.Name))
                        termList.Add(termSelect);
                int deleteTerm = 0;
                int deleteTermLocation = 0;
                foreach (Module.BusinessObjects.Term termSelect in termList)
                {
                    if (termSelect.TermLocationList.Count == 0)
                        continue;
                    //098: chức năng loại bên thua
                    foreach (var termLocation in termSelect.TermLocationList.ToList())
                    {
                        termLocationService.CancelWrongTerm(termLocation, ref deleteTerm, ref deleteTermLocation);
                        
                    }
                    countNumber++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), " ", stopWatch.Elapsed);
                }
                stopWatch.Stop();
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                if (deleteTerm > 0 || deleteTermLocation > 0)
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteTerm + " thuật ngữ bị xóa \r\n" + deleteTermLocation + " thuật vị bị xóa", InformationType.Info, 10000);
                return;
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("Check"))
            {
                var dictionaryTranslate = videoService.GetDictionarySpelling(video, true);
                var dictionaryOrigin = videoService.GetDictionarySpelling(video, false);
                var dictionarySpell = dictionaryOrigin;
                var spellLanguage = video.LanguageOrigin;

                foreach (Module.BusinessObjects.Term term in termSelected)
                {
                    if (string.IsNullOrEmpty(term.Name))
                        continue;

                    if (e.SelectedChoiceActionItem.Id.Equals("Check"))
                    {
                        spellLanguage = video.LanguageOrigin;
                        dictionarySpell = dictionaryOrigin;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Contains("Translate"))
                    {
                        spellLanguage = video.LanguageTranslate;
                        dictionarySpell = dictionaryTranslate;
                    }

                    if (spellLanguage.Code == "en" || spellLanguage.Code == "vi")
                    {
                        if (dictionarySpell == null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy từ điển dịch", InformationType.Error);
                            return;
                        }

                        bool existsInDictionary = TermService.CheckTermInDictionary(term.Name, dictionarySpell, spellLanguage, term.WordQuantity.Value);

                        // Gán giá trị dựa trên kết quả kiểm tra
                        term.Flag = !existsInDictionary;
                        term.Language = existsInDictionary ? spellLanguage : null;
                    }
                }
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
                    System.Type type = typeof(Module.BusinessObjects.TermCorrection);
                    string viewId = Application.FindListViewId(type);
                    //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                    if (string.IsNullOrEmpty(viewId))
                        return;
                    //var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
                           type, viewId, CollectionSourceMode.Normal);
                    if (e.SelectedChoiceActionItem.Id.Contains("Single"))
                    {
                        foreach (Module.BusinessObjects.Term termSelect in termSelected)
                        {
                            if (string.IsNullOrEmpty(termSelect.Name))
                                continue;
                            var lowerName = termSelect.Name.ToLower();
                            var termCorrection = new TermCorrection(termSelect.Session);
                            termCorrection.Term = termSelect;
                            //termCorrection.Caption = termSelect.Name;
                            collectionSource.Add(termCorrection);
                            var termUnicodeList = new System.Collections.Generic.List<string>();
                            var resultList = new System.Collections.Generic.List<string>();
                            var stringList = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();
                            //Kiểm tra từng từ                                                   
                            var termNameArray = lowerName.Split(' ');
                            foreach (var childName in termNameArray)
                            {
                                var childNameNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(childName);
                                if (dictionary[1].ContainsKey(childNameNoneUnicode))
                                {
                                    var childStringList = new System.Collections.Generic.List<string>();
                                    foreach (var suggest in dictionary[1][childNameNoneUnicode])
                                    {
                                        childStringList.Add(suggest);
                                    }
                                    stringList.Add(childStringList);
                                }

                            }
                            var textList = GenerateAllPermutations(stringList);
                            foreach (var text in textList)
                            {
                                var word = string.Join(" ", text);
                                if (!resultList.Contains(word))
                                    resultList.Add(word);
                            }
                            //var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                            //foreach (var result in resultList)
                            //{
                            //    var correctionOption = new CorrectionOption(termSelect.Session);
                            //    correctionOption.Name = result;
                            //    correctionOption.TermCorrection = termCorrection;
                            //    correctionOptions.Add(correctionOption);

                            //}
                            //termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);
                            termCorrection.AddTerm(termSelect, resultList);
                            //var termLocationCorrections = new System.Collections.Generic.List<TermLocationCorrection>();
                            //foreach (var termLocation in termSelect.TermLocationList)
                            //{
                            //    var termLocationCorrection = new TermLocationCorrection(termSelect.Session);
                            //    //termLocationCorrection.
                            //    termLocationCorrection.TermLocation = termLocation;

                            //    var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                            //    foreach (var result in resultList)
                            //    {
                            //        var correctionOption = new CorrectionOption(termSelect.Session);
                            //        correctionOption.Name = result;
                            //        correctionOption.TermLocationCorrection = termLocationCorrection;
                            //        correctionOptions.Add(correctionOption);

                            //    }
                            //    termLocationCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);

                            //    termLocationCorrections.Add(termLocationCorrection);
                            //}
                            //termCorrection.TermLocationCorrectionList = new DevExpress.Xpo.XPCollection<TermLocationCorrection>(termSelect.Session, termLocationCorrections);
                        }
                    }
                    else// if (e.SelectedChoiceActionItem.Id.Contains("Compound"))
                    {
                        foreach (Module.BusinessObjects.Term termSelect in termSelected)
                        {
                            if (string.IsNullOrEmpty(termSelect.Name))
                                continue;
                            var lowerName = termSelect.Name.ToLower();
                            var termNameLength = lowerName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                            var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);

                            var termCorrection = new TermCorrection(termSelect.Session);
                            termCorrection.Term = termSelect;
                            //termCorrection.Caption = termSelect.Name;
                            collectionSource.Add(termCorrection);
                            if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                            {
                                termCorrection.AddTerm(termSelect, dictionary[termNameLength][termNoneUnicode]);
                                //var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                                //foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                                //{
                                //    //Kiểm tra từng thuật vị                                    
                                //    var correctionOption = new CorrectionOption(termSelect.Session);
                                //    correctionOption.Name = sugget;
                                //    correctionOption.TermCorrection = termCorrection;
                                //    correctionOptions.Add(correctionOption);
                                //}
                                //termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);

                                //var termLocationCorrections = new System.Collections.Generic.List<TermLocationCorrection>();
                                //foreach (var termLocation in termSelect.TermLocationList)
                                //{
                                //    var termLocationCorrection = new TermLocationCorrection(termSelect.Session);
                                //    //termLocationCorrection.
                                //    termLocationCorrection.TermLocation = termLocation;

                                //    var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
                                //    foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                                //    {
                                //        var correctionOption = new CorrectionOption(termSelect.Session);
                                //        correctionOption.Name = sugget;
                                //        correctionOption.TermLocationCorrection = termLocationCorrection;
                                //        correctionOptions.Add(correctionOption);

                                //    }
                                //    termLocationCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);

                                //    termLocationCorrections.Add(termLocationCorrection);
                                //}
                                //termCorrection.TermLocationCorrectionList = new DevExpress.Xpo.XPCollection<TermLocationCorrection>(termSelect.Session, termLocationCorrections);
                            }                            
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
                foreach (Module.BusinessObjects.Term termSelect in termSelected)
                {
                    if (string.IsNullOrEmpty(termSelect.Name))
                        continue;
                    //Nếu không có thuật vị thì xóa luôn
                    if (termSelect.TermLocationList.Count == 0)
                    {
                        termSelect.Delete();
                        continue;
                    }    
                    var likeWordList = termSelect.GetLikeWordList();
                    if (likeWordList is null || likeWordList.Count == 0)
                        continue;
                    var replaceWord = likeWordList[0];
                    var existedTerm = TermService.FindTermByName(termSelect, replaceWord);
                    foreach (Module.BusinessObjects.TermLocation termLocation in termSelect.TermLocationList.ToList())
                    {
                        if (existedTerm is null)
                            existedTerm = TermService.FindTermByName(termSelect, replaceWord);
                        TermLocationService.ReplaceWord(termLocation, replaceWord, true, existedTerm);
                    }
                    //Nếu không có thuật vị thì xóa luôn
                    if (termSelect.TermLocationList.Count == 0)
                        termSelect.Delete();

                }
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("AutoCorrect"))
            {

                //088 Thuật toán sửa tự động như sau

                //- Nếu trị số > 1 và Từ giống = 1 : xét điều kiện 1 cần / 1 đủ để thay
                //Điều kiện cần đủ để sửa tự động

                //- Đk cần: Không overlap
                //-Đk đủ:
                //                +Sai chỉ 1 từ đơn so với TN đúng
                //+Có từ đơn sai chính tả
                //+ Tồn tại thuật ngữ đúng trong list TN
                int termLocationResult = 0;
                foreach (Module.BusinessObjects.Term termSelect in termSelected)
                {
                    if (string.IsNullOrEmpty(termSelect.Name))
                        continue;
                    var lowerName = termSelect.Name.ToLower();
                    var termNames = termSelect.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);
                    if (dictionary.ContainsKey(termNames.Length) && dictionary[termNames.Length].ContainsKey(termNoneUnicode))
                    {
                        var termDictionary = dictionary[termNames.Length][termNoneUnicode];
                        if (termDictionary.Count < 1)
                            continue;
                        var termLocationList = termSelect.TermLocationList.ToList();
                        int termLocationResultLocal = 0;
                        foreach (Module.BusinessObjects.TermLocation termLocation in termLocationList)
                        {
                            //- Đk cần: Không overlap
                            if (termLocation.Overlap)
                                continue;
                            //- Đk đủ:
                            //+Sai chỉ 1 từ đơn so với TN đúng
                            //+Có từ đơn sai chính tả
                            //+ Tồn tại thuật ngữ đúng trong list TN
                            foreach (var replaceWord in termDictionary)
                            {
                                var existedTerm = TermService.FindTermByName(termSelect, replaceWord);
                                if (termDictionary.Count == 1 || existedTerm != null)
                                {
                                    //088 -Nếu Trị số = 1 : xét điều kiện 1 cần / 2 đủ để thay
                                    var replacetermNames = replaceWord.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                    var intersect = replacetermNames.Intersect(termNames);
                                    //+Sai chỉ 1 từ đơn so với TN đúng
                                    bool intersectCorrect = intersect != null && intersect.Count() >= termNames.Length - 1;
                                    if (existedTerm is null && !intersectCorrect)
                                    {
                                        // Nếu Trị số = 1 : sai 2 điều kiện thì không đủ điều kiện thay thế
                                        // -Nếu trị số > 1 existedTerm khác null thì làm tiếp các kiểm tra khác
                                        continue;
                                    }
                                    else if (existedTerm != null && intersectCorrect)
                                    {
                                        if (TermLocationService.ReplaceWord(termLocation, replaceWord, true, existedTerm))
                                        {
                                            termLocationResult++;
                                            termLocationResultLocal++;
                                        }
                                    }
                                    else
                                    {
                                        //Nếu không tồn tại thuật ngữ đơn thì bỏ qua
                                        if (!dictionary.ContainsKey(1))
                                            continue;
                                        //Có từ đơn sai chính tả
                                        int notCorrect = 0;
                                        foreach (var termName in termNames)
                                        {
                                            if (!Module.Helpers.TextHelper.CheckSimpleWordIsCorrect(dictionary[1], termName))
                                                notCorrect++;
                                        }
                                        if (notCorrect > 0)
                                        {
                                            if (TermLocationService.ReplaceWord(termLocation, replaceWord, true, existedTerm))
                                            {
                                                termLocationResult++;
                                                termLocationResultLocal++;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if (!termSelect.IsDeleted && termSelect.Flag && termLocationResultLocal == termLocationList.Count)
                            termSelect.Flag = false;
                    }
                    //Nếu không có thuật vị thì xóa luôn
                    if (termSelect.TermLocationList.Count == 0)
                        termSelect.Delete();
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", termLocationResult + " thuật vị được thay thế", termLocationResult > 0 ? InformationType.Info : InformationType.Error);
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("CountLikeTerm") || e.SelectedChoiceActionItem.Id.StartsWith("CountLikeWord"))
            {
                int successCount = 0;
                string noteCaption = Module.Helpers.TextHelper.GetFirstLetterToUpper(e.SelectedChoiceActionItem.Caption);
                foreach (Module.BusinessObjects.Term termSelect in termSelected)
                {
                    if (string.IsNullOrEmpty(termSelect.Name))
                        continue;

                    var likeList = e.SelectedChoiceActionItem.Id.Contains("LikeTerm") ? termSelect.GetLikeTermList() : termSelect.GetLikeWordList();
                    //if (likeList is null || likeList.Count == 0)
                    //    continue;
                    if(e.SelectedChoiceActionItem.Id.Contains("LikeTerm"))
                        termSelect.LikeTerm = (likeList is null || likeList.Count == 0) ? null : likeList.Count;
                    else
                        termSelect.LikeWord = (likeList is null || likeList.Count == 0) ? null : likeList.Count;
                    if (likeList != null && likeList.Count > 0)
                        termSelect.AddTextNode(charTag, noteCaption + " " + string.Join(", ", likeList), false);
                    countNumber++;
                    successCount++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), " ", stopWatch.Elapsed);
                }
                stopWatch.Stop();
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", $"Đã xử lý {successCount}/{total} thuật ngữ", InformationType.Info, 10000);
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("FirstLikeTerm"))
            {
                int termLocationResult = 0;
                foreach (Module.BusinessObjects.Term termSelect in termSelected)
                {
                    if (string.IsNullOrEmpty(termSelect.Name))
                        continue;
                    //Nếu không có thuật vị thì xóa luôn
                    if (termSelect.TermLocationList.Count == 0)
                        termSelect.Delete();
                    var likeTermList = termSelect.GetLikeTermList(true);
                    if (likeTermList is null || likeTermList.Count == 0)
                        continue;
                    string replaceWord = likeTermList[0];                    
                    Module.BusinessObjects.Term existedTerm = null;
                    int termLocationResultLocal = 0;
                    var termLocationList = termSelect.TermLocationList.ToList();                
                    foreach (Module.BusinessObjects.TermLocation termLocation in termLocationList)
                    {
                        if (existedTerm is null)
                            existedTerm = TermService.FindTermByName(termSelect,replaceWord);
                        if (TermLocationService.ReplaceWord(termLocation, replaceWord, true, existedTerm))
                        {
                            termLocationResult++;
                            termLocationResultLocal++;
                        }
                    }
                    //Nếu không có thuật vị thì xóa luôn
                    if (termSelect.TermLocationList.Count == 0)
                        termSelect.Delete();
                    if (!termSelect.IsDeleted && termSelect.Flag && termLocationResultLocal == termLocationList.Count)
                        termSelect.Flag = false;
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", termLocationResult + " thuật vị được thay thế", termLocationResult > 0 ? InformationType.Info : InformationType.Error);
            }
            else if (e.SelectedChoiceActionItem.Id.StartsWith("StickingSplit"))
            {
                IObjectSpace objectSpace = View.ObjectSpace;

                var dictionaryTranslate = videoService.GetDictionarySpelling(video, true);
                var dictionaryOrigin = videoService.GetDictionarySpelling(video, false);
                var checkDictionary = dictionaryOrigin;
                var spellLanguage = video.LanguageOrigin;
                bool use = true;
                if (e.SelectedChoiceActionItem.Id.Equals("StickingSplitOrigin"))
                {
                    checkDictionary = dictionaryOrigin;
                    spellLanguage = video.LanguageOrigin;
                    use = false;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("StickingSplitTranslate"))
                {
                    checkDictionary = dictionaryTranslate;
                    spellLanguage = video.LanguageTranslate;
                    use = true;
                }

                if (spellLanguage.Code == "vi")
                {
                    var nosignDictionary = videoService.GetNoSignDictionary(video, use);
                    checkDictionary = nosignDictionary;
                }
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (spellLanguage.Code == "vi")
                    {

                        var nosignTermName = Module.Helpers.TextHelper.RemoveUnicode(term.Name);
                        if ((TermService.CheckTermInDictionary(nosignTermName, checkDictionary, spellLanguage, 1)))
                            continue;
                        else
                        {
                            foreach (string line in term.Note.Split('\n'))
                            {
                                string temp = line.Replace(",", "");
                                string part1 = temp.Split(' ')[0];
                                string part2 = temp.Split(' ')[1];

                                string tempWord = Module.Helpers.TextHelper.RemoveUnicode(temp);

                                // nếu từ ghép tách ra có trong từ điển thì xóa từ ghép nếu đã tồn tại thuật ngữ trong danh sách, hoặc cập nhật thuật ngữ nếu chưa tồn tại thuật ngữ
                                if (TermService.CheckTermInDictionary(tempWord, checkDictionary, spellLanguage, 2))
                                {
                                    bool DeleteTerm = false;
                                    foreach (var refTerm in term.Video.TermList)
                                    {
                                        if (!DeleteTerm && !refTerm.Oid.Equals(term.Oid) && temp.Equals(refTerm.Name, System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            termService.UpdatePosition(refTerm, true);
                                            term.Delete();
                                            DeleteTerm = true;
                                            break;
                                        }
                                    }
                                    if (!DeleteTerm)
                                    {
                                        term.Name = temp;
                                    }
                                }
                                else
                                {
                                    foreach (TermLocation termLocation in term.TermLocationList)
                                    {
                                        var termNext1 = termLocation.Audio.TermLocationList.Where(x => x.Sentence == termLocation.Sentence).OrderBy(x => x.Location).FirstOrDefault(x => x.Location > termLocation.Location);
                                        var termNext2 = termLocation.Audio.TermLocationList.Where(x => x.Sentence == termNext1.Sentence).OrderBy(x => x.Location).FirstOrDefault(x => x.Location > termNext1.Location);

                                        if (termNext1 != null && termNext2 != null)
                                        {

                                            var trioTemp = part2 + " " + termNext1.Term.Name + " " + termNext2.Term.Name;
                                            trioTemp = Module.Helpers.TextHelper.RemoveUnicode(trioTemp);
                                            if (TermService.CheckTermInDictionary(trioTemp, checkDictionary, spellLanguage, 3))
                                            {
                                                var mergedTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { termNext1, termNext2 }, part2, true);
                                                continue;
                                            }
                                        }
                                        if (termNext1 != null)
                                        {
                                            var duoTemp = part2 + " " + termNext1.Term.Name;
                                            duoTemp = Module.Helpers.TextHelper.RemoveUnicode(duoTemp);
                                            if (TermService.CheckTermInDictionary(duoTemp, checkDictionary, spellLanguage, 2))
                                            {
                                                var mergedTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { termNext1 }, part2, true);
                                                continue;
                                            }
                                        }
                                        var newTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { }, part2, true);
                                    }
                                    foreach (TermLocation termLocation in term.TermLocationList)
                                    {
                                        var termPre1 = termLocation.Audio.TermLocationList.Where(x => x.Sentence == termLocation.Sentence).OrderBy(x => x.Location).FirstOrDefault(x => x.Location < termLocation.Location);
                                        var termPre2 = termLocation.Audio.TermLocationList.Where(x => x.Sentence == termPre1.Sentence).OrderBy(x => x.Location).FirstOrDefault(x => x.Location < termPre1.Location);

                                        if (termPre1 != null && termPre2 != null)
                                        {

                                            var trioTemp = termPre2.Term.Name + " " + termPre1.Term.Name + " " + part1;
                                            trioTemp = Module.Helpers.TextHelper.RemoveUnicode(trioTemp);
                                            if (TermService.CheckTermInDictionary(trioTemp, checkDictionary, spellLanguage, 3))
                                            {
                                                var mergedTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { termPre2, termPre1 }, part1, false);
                                                continue;
                                            }
                                        }
                                        if (termPre1 != null)
                                        {
                                            var duoTemp = termPre1.Term.Name + " " + part1;
                                            duoTemp = Module.Helpers.TextHelper.RemoveUnicode(duoTemp);
                                            if (TermService.CheckTermInDictionary(duoTemp, checkDictionary, spellLanguage, 2))
                                            {
                                                var mergedTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { termPre1 }, part1, false);
                                                continue;
                                            }
                                        }
                                        var newTerm = termService.MergeAdjacentTerms( new System.Collections.Generic.List<TermLocation> { }, part1, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            stopWatch.Stop();
            #region Code cũ dùng spelling
            //if (e.SelectedChoiceActionItem.Id.StartsWith("Correct"))
            //{
            //    using (DevExpress.ExpressApp.SystemModule.DialogController dc =
            //                Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            //    {
            //        //dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
            //        //{
            //        //    if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
            //        //        ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
            //        //    {
            //        //        ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((Controller)o).Frame.Template).IsSearchEnabled = true;
            //        //    }
            //        //};
            //        ShowViewParameters showViewParameters = new ShowViewParameters()
            //        {
            //            TargetWindow = TargetWindow.NewModalWindow,
            //            CreateAllControllers = true,
            //            Context = TemplateContext.LookupWindow,
            //        };
            //        showViewParameters.Controllers.Add(dc);
            //        System.Type type = typeof(Module.BusinessObjects.TermCorrection);
            //        string viewId = Application.FindListViewId(type);
            //        //string viewId = Application.FindListViewId(typeof(IpcLineItem));
            //        if (string.IsNullOrEmpty(viewId))
            //            return;
            //        //var dictionaryObjectSpace = Application.CreateObjectSpace(type);
            //        CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
            //               type, viewId, CollectionSourceMode.Normal);
            //        if (e.SelectedChoiceActionItem.Id.Contains("Single"))
            //        {
            //            string aff = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryAffVN.aff";
            //            string dic = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryVN.dic";
            //            using (NHunspell.Hunspell hunspell = new NHunspell.Hunspell(aff, dic))
            //            {
            //                foreach (Module.BusinessObjects.Term termSelect in termSelected)
            //                {
            //                    if (string.IsNullOrEmpty(termSelect.Name))
            //                        continue;
            //                    var termCorrection = new TermCorrection(termSelect.Session);
            //                    termCorrection.Term = termSelect;
            //                    collectionSource.Add(termCorrection);
            //                    var termUnicodeList = new System.Collections.Generic.List<string>();
            //                    var resultList = new System.Collections.Generic.List<string>();
            //                    foreach (var termLocation in termSelect.TermLocationList)
            //                    {
            //                        var termUnicode = termLocation.GetUnicodeWord();
            //                        if (string.IsNullOrEmpty(termUnicode))
            //                            continue;
            //                        termUnicode = termUnicode.ToLower();
            //                        if (!termUnicodeList.Contains(termUnicode))
            //                        {
            //                            termUnicodeList.Add(termUnicode);
            //                            //Kiểm tra từ
            //                            var termNames = termUnicode.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            //                            var stringList = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();
            //                            int wordArrayLength = 1;
            //                            foreach (var termName in termNames)
            //                            {
            //                                var childStringList = new System.Collections.Generic.List<string>();
            //                                //if (hunspell.Spell(termName))
            //                                //{
            //                                //    childArrayList.Add(termName);
            //                                //}
            //                                //else
            //                                //{
            //                                //var stemList = hunspell.Stem(termName);
            //                                var suggestList = hunspell.Suggest(termName);
            //                                if (suggestList.Count == 0)
            //                                {
            //                                    childStringList.Add(termName);
            //                                }
            //                                else
            //                                {
            //                                    wordArrayLength = wordArrayLength * suggestList.Count;
            //                                    foreach (var suggest in suggestList)
            //                                    {
            //                                        childStringList.Add(suggest);
            //                                    }
            //                                }
            //                                if (!childStringList.Contains(termName) && hunspell.Spell(termName))
            //                                {
            //                                    childStringList.Add(termName);
            //                                }
            //                                //}                                            
            //                                stringList.Add(childStringList);
            //                            }
            //                            var textList = GenerateAllPermutations(stringList);
            //                            foreach (var text in textList)
            //                            {
            //                                var word = string.Join(" ", text);
            //                                if (!resultList.Contains(word))
            //                                    resultList.Add(word);
            //                            }
            //                        }
            //                    }
            //                    var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
            //                    foreach (var result in resultList)
            //                    {
            //                        var correctionOption = new CorrectionOption(termSelect.Session);
            //                        correctionOption.Name = result;
            //                        correctionOption.TermCorrection = termCorrection;
            //                        correctionOptions.Add(correctionOption);

            //                    }
            //                    termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);
            //                }
            //            }
            //        }
            //        else if (e.SelectedChoiceActionItem.Id.Contains("Compound"))
            //        {
            //            string folder = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "CheckDictionaryFolder", "\\\\rd\\CodeGen\\packages\\Dictionary");
            //            if (string.IsNullOrEmpty(folder))
            //            {
            //                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thư mục chứa từ điển, vui lòng kiểm tra lại tham số", InformationType.Error);
            //                return;
            //            }
            //            if (!folder.EndsWith("\\"))
            //                folder += "\\";
            //            string fileName = folder + video.LanguageOrigin.Code + ".txt";
            //            if (!System.IO.File.Exists(fileName))
            //            {
            //                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy từ điển từ ghép, vui lòng kiểm tra lại", InformationType.Error);
            //                return;
            //            }
            //            var wordsText = System.IO.File.ReadAllText(fileName);
            //            var dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>(wordsText);
            //            foreach (Module.BusinessObjects.Term termSelect in termSelected)
            //            {
            //                if (string.IsNullOrEmpty(termSelect.Name))
            //                    continue;
            //                var termCorrection = new TermCorrection(termSelect.Session);
            //                termCorrection.Term = termSelect;
            //                collectionSource.Add(termCorrection);
            //                if (!dictionary.ContainsKey(termSelect.Name))
            //                    continue;
            //                var termNames = termSelect.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            //                var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
            //                foreach (var word in dictionary[termSelect.Name])
            //                {
            //                    var correctionOption = new CorrectionOption(termSelect.Session);
            //                    correctionOption.Name = word;
            //                    correctionOption.TermCorrection = termCorrection;
            //                    correctionOptions.Add(correctionOption);
            //                    //termCorrection.CorrectionOptionList.Add(correctionOption);
            //                }
            //                termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);
            //                //correctionOptionList.AddRange(correctionOptions);
            //            }

            //        }
            //        else if (e.SelectedChoiceActionItem.Id.Contains("Variaty"))
            //        {
            //            foreach (Module.BusinessObjects.Term termSelect in termSelected)
            //            {
            //                if (string.IsNullOrEmpty(termSelect.Name))
            //                    continue;
            //                var termCorrection = new TermCorrection(termSelect.Session);
            //                termCorrection.Term = termSelect;
            //                collectionSource.Add(termCorrection);
            //                if (termSelect.TermLocationList.Count() > 0)
            //                {
            //                    var listWord = new System.Collections.Generic.List<string>();
            //                    var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
            //                    foreach (var termLocation in termSelect.TermLocationList)
            //                    {
            //                        var word = termLocation.GetUnicodeWord();
            //                        if (string.IsNullOrEmpty(word))
            //                            continue;
            //                        if (!listWord.Contains(word))
            //                            listWord.Add(word);
            //                    }
            //                    foreach (var word in listWord)
            //                    {
            //                        var correctionOption = new CorrectionOption(termSelect.Session);
            //                        correctionOption.Name = word;
            //                        correctionOption.TermCorrection = termCorrection;
            //                        correctionOptions.Add(correctionOption);
            //                        //termCorrection.CorrectionOptionList.Add(correctionOption);
            //                    }
            //                    termCorrection.CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(termSelect.Session, correctionOptions);
            //                    //correctionOptionList.AddRange(correctionOptions);
            //                }

            //            }

            //        }
            //        var listview = Application.CreateListView(viewId, collectionSource, false);
            //        listview.AllowNew["Popup"] = false;
            //        //dc.AcceptAction.Caption = "Chọn " + caption;
            //        dc.AcceptAction.Active.SetItemValue("", false);
            //        dc.SaveOnAccept = false;
            //        dc.CancelAction.Active.SetItemValue("", false);
            //        showViewParameters.CreatedView = listview;
            //        Application.ShowViewStrategy.ShowView(showViewParameters,
            //            new ShowViewSource(Frame, dc.AcceptAction));
            //    }
            //}
            //Tính năng sửa tự động
            //var text = JsonConvert.SerializeObject(dic);
            //string filePath = @"C:\Code\words.txt";
            //System.IO.File.WriteAllText(filePath, text);

            //var txtLoad = System.IO.File.ReadAllText(filePath);
            //var newDic = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(txtLoad);
            ////var items = JsonConvert.DeserializeObject<List<ItemTest>>(txtLoad);
            //using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    using (TextWriter tw = new StreamWriter(fs))

            //        foreach (KeyValuePair<string, List<string>> kvp in dic)
            //        {
            //            tw.WriteLine(string.Format("{0};{1}", kvp.Key, kvp.Value));
            //        }
            //}

            //var dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
            //    (System.IO.File.ReadAllText(openFileDialog.FileName));
            #endregion

        }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> GenerateAllPermutations<T>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> listOfList)
        {
            var results = new System.Collections.Generic.List<System.Collections.Generic.List<T>>();

            ForEachPermutationDo(listOfList, (permutation) => {
                results.Add((System.Collections.Generic.List<T>)permutation);
                return true;
            });

            return results;
        }

        void ForEachPermutationDo<T>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> listOfList, System.Func<System.Collections.Generic.IEnumerable<T>, bool> whatToDo)
        {
            var numCols = listOfList.Count();
            var numRows = listOfList.Aggregate(1, (a, b) => a * b.Count());
            var continueGenerating = true;

            var permutation = new System.Collections.Generic.List<T>();
            for (var r = 0; r < numRows; r++)
            {
                var repeatFactor = 1;
                for (var c = 0; c < numCols; c++)
                {
                    var aList = listOfList.ElementAt(c);
                    permutation.Add(aList.ElementAt((r / repeatFactor) % aList.Count()));
                    repeatFactor *= aList.Count();
                }

                continueGenerating = whatToDo(permutation.ToList()); // send duplicate
                if (!continueGenerating) break;

                permutation.Clear();
            }

















            #endregion SpellingTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0905            Oid: b1f93f18-7e77-4617-8616-53fa9940b80c
		private void TermFlag_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TermFlag), "Cờ thuật ngữ");              
      
            #region TermFlagImportCode
            
            if (Module.Helpers.ParameterHelper.GetBooleanOrDefault(ObjectSpace, "RemoveAllFlagWhenExecute", false))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video is null)
                    return;
                foreach (Module.BusinessObjects.Term term in video.TermList)
                {
                    term.Flag = false;
                    term.Flag2 = false;
                }

            }
            int totalCheck = 0;
            int totalChange = 0;
            int totalFlag = 0;
            decimal totalSelectObject = View.SelectedObjects.Count;
            decimal countNumber = 0;
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary = null;
            if (e.SelectedChoiceActionItem.Id.Equals("SpellingMistake"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video is null)
                    return;
                //Lỗi chính tả                
                if (video.LanguageOrigin is null || string.IsNullOrEmpty(video.LanguageOrigin?.Code))
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy ngôn ngữ gốc", InformationType.Error);
                    return;
                }
                dictionary = video.GetDictionary();
                if (dictionary is null)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy từ điển", InformationType.Error);
                    return;
                }
            }
            //027: Kết thúc mọi dựng cờ luôn thông báo thời gian thực hiện/tổng số bản ghi đã kiểm tra, tổng số bản ghi hiệu lực: Số bản ghi được dựng cờ, số bản ghi thay đổi (thuật vị)
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            var startime = System.DateTime.Now;
            stopWatch.Start();
            if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck") || e.SelectedChoiceActionItem.Id.Equals("TermPositionOverlap"))
            {                                
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    var overlap = false;
                    if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck"))
                    {
                        foreach (var termLocation in term.TermLocationList)
                        {
                            totalCheck++;
                            //2023-11-09: Chỉ kiểm tra những thuật vị có check Overlap
                            //if (termLocation.Overlap) //2024-12-20 Kiểm tra đè > kiểm tra tất
                            {
                                var termLocationOverlap = TermLocationService.CheckOverlap(termLocation, true);
                                if (termLocation.Overlap != termLocationOverlap)
                                {
                                    totalChange++;
                                    termLocation.Overlap = termLocationOverlap;
                                }
                            }
                            if (termLocation.Overlap)
                            {
                                overlap = true;
                                
                            }
                        }
                        //if (term.Flag)
                        //    term.AddTextNode(e.SelectedChoiceActionItem.Caption);
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / totalSelectObject).ToString("p0"), " ", stopWatch.Elapsed);
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("TermPositionOverlap"))
                    {
                        //2023-24-10 Dựng cờ Thuật ngữ cho thuật ngữ nào tồn tại Thuật vị có Overlap = True

                        foreach (var termLocation in term.TermLocationList)
                        {
                            if (termLocation.Overlap)
                            {
                                overlap = true;
                                break;
                            }
                        }
                        //if (term.Flag)
                        //    term.AddTextNode(e.SelectedChoiceActionItem.Caption);
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / totalSelectObject).ToString("p0"), " ", stopWatch.Elapsed);
                    }
                    if(term.Overlap != overlap)
                        term.Overlap = overlap;
                    if (term.Overlap)
                        totalFlag++;
                }
                
            }
            else
            {
                char charTag = '<';
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (e.SelectedChoiceActionItem.Id.Equals("Clear"))
                    {
                        term.Flag = false;
                        term.Flag2 = false;
                        continue;
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("CopyToFlag2"))
                    {
                        term.Flag2 = term.Flag;
                        //term.Flag = false;
                        continue;
                    }
                    bool termFlag = false;
                    if (!string.IsNullOrEmpty(term.Name))
                    {
                        if (e.SelectedChoiceActionItem.Id.Equals("SuffixES"))
                        {
                            if (term.Name.EndsWith("es", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SuffixS"))
                        {
                            if (term.Name.EndsWith("s", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SuffixER"))
                        {
                            if (term.Name.EndsWith("er", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SuffixING"))
                        {
                            if (term.Name.EndsWith("ing", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SuffixED"))
                        {
                            if (term.Name.EndsWith("ed", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("Apostrophe"))
                        {
                            if (term.Name.Contains("'", System.StringComparison.OrdinalIgnoreCase) || term.Name.Contains("`", System.StringComparison.OrdinalIgnoreCase))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("TranslateNotFound"))
                        {
                            foreach (var termLocation in term.TermLocationList)
                            {
                                if (string.IsNullOrEmpty(termLocation.MachineTranslate))
                                {
                                    termFlag = true;
                                    break;
                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("TranslateDifferent"))
                        {
                            var termLocationList = term.TermLocationList.Where(m => !string.IsNullOrEmpty(m.MachineTranslate)).ToList();
                            if (termLocationList.Count > 1)
                            {
                                var firstTranslate = termLocationList[0].MachineTranslate;
                                foreach (var termLocation in termLocationList)
                                {
                                    if (!termLocation.MachineTranslate.Equals(firstTranslate, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        termFlag = true;                                        
                                        break;
                                    }
                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("TranslateSameOrigin"))
                        {
                            //Dịch giữ nguyên
                            //Dựng cờ khi: phát hiện Máy dịch hoặc Dịch có giá trị = từ gốc(tồn tại thuật vị như vậy)
                            if (!string.IsNullOrEmpty(term.Name))
                            {
                                foreach (var termLocation in term.TermLocationList)
                                {
                                    if (!string.IsNullOrEmpty(termLocation.MachineTranslate)
                                        && termLocation.MachineTranslate.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        termFlag = true;
                                        break;
                                    }else if (!string.IsNullOrEmpty(termLocation.Translate)
                                        && termLocation.Translate.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        termFlag = true;
                                        break;
                                    }
                                }
                            }                            
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("UpcaseSecond"))
                        {
                            if (!char.IsUpper(term.Name[0]))
                                continue;
                            foreach (var termLocation in term.TermLocationList)
                            {
                                var audio = termLocation.GetAudioFromElement();
                                if (audio is null)
                                    continue;
                                if (!string.IsNullOrEmpty(audio.Content))
                                {
                                    var sentencesArray = Module.Helpers.TextHelper.GetSentences(audio.Content);
                                    var contents = sentencesArray[termLocation.Sentence.Value - 1].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                                    if (contents.Length > 1 && char.IsUpper(contents[1][0]) && contents[1].Equals(term.Name, System.StringComparison.OrdinalIgnoreCase) && termLocation.Location == 2)
                                        termFlag = true;
                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("UpperCaseTerm"))
                        {
                            //Những từ khi Nạp thuật ngữ, Tra từ điển nhưng nạp vào là hoa (thường là đầu câu)
                            if ((term.TermType == TermType.Term || term.TermType == TermType.Dictionary) && char.IsUpper(term.Name[0]))
                                termFlag = true;
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SameSentenceWord"))
                        {
                            //2023-07-05 Từ cùng câu / SameSentenceWord : mục đích xử lý các từ liên quan khi đang xem xét 1 từ
                            if (term.Video is null)
                                continue;
                            foreach (var termLocation in term.TermLocationList)
                            {
                                var audio = termLocation.GetAudioFromElement();
                                if (audio is null)
                                    continue;
                                if (!string.IsNullOrEmpty(audio.Content))
                                {
                                    string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
                                    var rows = audio.Content.Split(newLineText, System.StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var rowContent in rows)
                                    {
                                        if (Module.Helpers.TextHelper.GetIndexWordInContent(term.Name, rowContent) >= 0)
                                        {
                                            //Chỉ tìm trong câu trùng
                                            foreach (var referenceTerm in term.Video.TermList)
                                            {
                                                if (referenceTerm.Flag || string.IsNullOrEmpty(referenceTerm.Name)) continue;
                                                if (referenceTerm.Oid.Equals(term.Oid))
                                                {
                                                    referenceTerm.Flag = true;
                                                    referenceTerm.AddTextNode(charTag, e.SelectedChoiceActionItem.Caption);
                                                    continue;
                                                }
                                                if (Module.Helpers.TextHelper.GetIndexWordInContent(referenceTerm.Name, rowContent) >= 0)
                                                {
                                                    referenceTerm.Flag = true;
                                                    referenceTerm.AddTextNode(charTag, e.SelectedChoiceActionItem.Caption);
                                                }

                                            }
                                        }
                                    }
                                    //var contents = audio.Content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("MachineTranslateDifferent"))
                        {
                            //Cờ thuật ngữ > Máy dịch khác gốc: tồn tại thuật vị mà Máy dịch khác gốc
                            foreach (var termLocation in term.TermLocationList)
                            {
                                if (!term.Name.Equals(termLocation.MachineTranslate, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    termFlag = true;                                    
                                    break;
                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("UpperLowerMix"))
                        {
                            if (string.IsNullOrEmpty(term.Name))
                                continue;
                            System.Collections.Generic.List<string> termNames = new System.Collections.Generic.List<string>();
                            foreach (var termLocation in term.TermLocationList)
                            {
                                var audio = termLocation.GetAudioFromElement();
                                if (audio is null)
                                    continue;
                                if (!string.IsNullOrEmpty(audio.Content))
                                {
                                    var content = audio.Content.Replace("  ", " ");
                                    //Xóa ký tự đặc biệt đầu câu 
                                    content = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(content, true);
                                    int index = TermLocationService.GetIndexContent(termLocation, content, term.Name);
                                    if (index > 0)
                                    {
                                        var termName = content.Substring(index, term.Name.Length);
                                        if (termName.Equals(termName.ToLower()) || termName.Equals(termName.ToUpper()))
                                        {
                                            if (!termNames.Contains(termName))
                                                termNames.Add(termName);
                                        }
                                        else
                                        {
                                            if (char.IsUpper(termName[0]))
                                            {
                                                //string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
                                                //char[] newLineChar = new char[] { '.', '?', '!', '\n',':' };
                                                //var rows = audio.Content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                                                var sentencesArray = Module.Helpers.TextHelper.GetSentences(audio.Content);
                                                var tempIndex = 0;
                                                bool validate = true;
                                                foreach (var rowContent in sentencesArray)
                                                {
                                                    if (tempIndex < index && index < tempIndex + rowContent.Length + 1)
                                                    {
                                                        var childContent = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(rowContent, true);
                                                        var diffIndex = index - tempIndex;
                                                        if (diffIndex > 0)
                                                            diffIndex--;
                                                        var newIndex = childContent.IndexOf(term.Name, diffIndex, System.StringComparison.OrdinalIgnoreCase);
                                                        //Hoa đầu câu không tính là hoa 
                                                        if (newIndex == 0)
                                                        {
                                                            validate = false;
                                                        }
                                                        else
                                                        {
                                                            validate = !Module.Helpers.TextHelper.CheckPositonIsStartSentence(childContent, newIndex);
                                                        }
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        tempIndex += rowContent.Length + 1;
                                                    }
                                                }
                                                if (!validate)
                                                    continue;
                                                //Hoa đầu câu không tính là hoa
                                                if (Module.Helpers.TextHelper.CheckPositonIsStartSentence(content, index))
                                                    continue;
                                                //var beforeIndex = index - 1;
                                                //for (int i = 0; i < content.Length; i++)
                                                //{
                                                //    //Xử lý trường hợp dấu cách đầu câu
                                                //    if (content[beforeIndex] == ' ' && beforeIndex > i)
                                                //        beforeIndex--;
                                                //}
                                                //if (newLineChar.Contains(content[beforeIndex]))                                            
                                                //    continue;
                                            }
                                            if (!termNames.Contains(termName))
                                                termNames.Add(termName);
                                        }
                                        if (termNames.Count == 2)
                                        {
                                            termFlag = true;                                           
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (e.SelectedChoiceActionItem.Id.Equals("SpellingMistake"))
                        {
                            var termNameLength = term.Name.Split(' ').Length;
                            var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(term.Name).ToLower();
                            if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                            {
                                bool flag = true;
                                foreach (var sugget in dictionary[termNameLength][termNoneUnicode])
                                {
                                    //Kiểm tra từng thuật vị
                                    if (sugget.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                termFlag = flag;
                            }
                            else
                            {
                                //Nếu không chứa thuật vị
                                termFlag = true;
                            }                            
                        }
                        
                    }
                    if(term.Flag != termFlag)
                        term.Flag = termFlag;
                    if (term.Flag)
                    {
                        totalFlag++;
                        if (!string.IsNullOrEmpty(term.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                        }
                        //term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, e.SelectedChoiceActionItem.Caption);
                        term.AddTextNode(charTag, e.SelectedChoiceActionItem.Caption);
                    }
                }
            }
            if (e.SelectedChoiceActionItem.Id.StartsWith("StickingWord"))
            {
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                var dictionaryTranslate = videoService.GetDictionarySpelling(video, true);
                var dictionaryOrigin = videoService.GetDictionarySpelling(video, false);     
                var dictionarySpell = dictionaryOrigin;
                var spellLanguage = video.LanguageOrigin;
                bool use = true;
                if (e.SelectedChoiceActionItem.Id.Equals("StickingWordOrigin"))
                {
                    dictionarySpell = dictionaryOrigin;
                    spellLanguage = video.LanguageOrigin;
                    use = false;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("StickingWordTranslate"))
                {
                    dictionarySpell = dictionaryTranslate;
                    spellLanguage = video.LanguageTranslate;
                    use = true;
                }
                var checkDictionary = dictionarySpell;
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (string.IsNullOrEmpty(term.Name))
                        continue;
                    if (TermService.CheckTermInDictionary(term.Name, dictionarySpell, spellLanguage, term.WordQuantity.Value))
                        continue;
                    // Define function to divide string
                    string[] DivideString(string term, int n, int m)
                    {
                        string part1 = term.Substring(0, m);
                        string part2 = term.Substring(m, n - m);
                        return new string[] { part1, part2 };
                    }
                    var nosignTermName = Module.Helpers.TextHelper.RemoveUnicode(term.Name);
                    for (int m = 2; m <= nosignTermName.Length - 2; m++)
                    {
                        // Divide term name into two parts
                        string[] parts = DivideString(nosignTermName, nosignTermName.Length, m);
                        string[] partsNote = DivideString(term.Name, term.Name.Length, m);

                        string part1 = parts[0];
                        string part2 = parts[1];

                        string part1Note = partsNote[0];
                        string part2Note = partsNote[1];

                        if (spellLanguage.Code == "vi")
                        {
                            var nosignDictionary = videoService.GetNoSignDictionary(video, use);
                            checkDictionary = nosignDictionary;
                        }
                        // Check if both parts are in the Word object
                        bool flag1 = TermService.CheckTermInDictionary(part1, checkDictionary, spellLanguage, 1);
                        bool flag2 = TermService.CheckTermInDictionary(part2, checkDictionary, spellLanguage, 1);

                        // If both parts are in the Word object, update Flag and Note and break out of loop
                        if (flag1 && flag2)
                        {
                            term.Flag = true;
                            term.Note += part1Note + ", " + part2Note + "\n";                          
                        }
                    }
                }
            }

            if (!e.SelectedChoiceActionItem.Id.Equals("Clear") && !e.SelectedChoiceActionItem.Id.Equals("CopyToFlag2"))
            {
                //027: Kết thúc mọi dựng cờ luôn thông báo thời gian thực hiện/tổng số bản ghi đã kiểm tra,
                //tổng số bản ghi hiệu lực: Số bản ghi được dựng cờ, số bản ghi thay đổi (thuật vị)
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                string message = "";
                //2024-12-21: Cờ OverlapCheck sẽ chỉ hiện thông tin cờ
                //if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck")){
                //    message = "\r\nTổng số thuật vị kiểm tra: " + totalCheck.ToString("N0");
                //    message += "\r\nTổng số thuật vị thay đổi: " + totalChange.ToString("N0");
                //    if(totalChange == 0 && totalFlag != System.Convert.ToInt32(totalSelectObject))
                //    {
                //        message += "\r\nThuật ngữ bị sai cờ đè : " + (totalSelectObject - totalFlag).ToString("N0");
                //    }
                //}
                //else
                {
                    message = "\r\nTổng số thuật ngữ kiểm tra: " +  totalSelectObject.ToString("N0");
                    message += "\r\nTổng số thuật ngữ hiệu lực: " +  totalFlag.ToString("N0");
                }
                
                string caption = System.String.Format("Thời gian thực hiện: {0:00}:{1:00}:{2:00}",
                                    stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, caption, message, InformationType.Info, 10000);
            }
            if (stopWatch.Elapsed.TotalMinutes > 1)
            {
                //Nếu nhỏ hơn 1 phút thì không log
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video != null)
                {
                    //StartTime : Chức năng : SL multiselect : SL kết quả : Tổng thời gian xử lý (phút làm tròn)
                    //video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startime.ToString("dd/MM/yyyy h:mm"), ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, totalSelectObject, totalFlag, System.Math.Round(stopWatch.Elapsed.TotalMinutes, 0));
                    videoService.LogToNote(video, startime, e.SelectedChoiceActionItem.Caption, System.Convert.ToInt32(totalSelectObject), totalFlag, stopWatch.Elapsed);
                }
            }
            stopWatch.Stop();











            #endregion TermFlagImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0539            Oid: 2028f009-115a-4dff-ac73-11612568e652
		private void ImportTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ImportTerm), "Nạp thuật ngữ");              
      
            #region ImportTermImportCode
            // 2023 - 05 - 31: Start: Bỏ trường hợp này
            //Thuật ngữ được nạp vào theo tình huống được gán Loại thuật ngữ
            //- Tra từ điển
            //-Chữ hoa: Viết tắt, Viết hoa
            //- Nạp nội dung: Chữ số, Phi thuật
            //- Gộp thuật ngữ: Từ gộp
            //2023-07-12: - Gộp chức năng Nạp viết hoa và Tra từ điển vào Nạp thuật ngữ (với các lựa chọn)
            var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null) return;
            if (video.AudioList is null)
                return;
            if (video.AudioList.Count == 0)
                return;
            Module.SystemObjects.Tools.ShowOrCloseWaitFormWithCancelButton();
            var startime = System.DateTime.Now;

            bool isReversed = e.SelectedChoiceActionItem.Id.ToLower().Contains("translate");
            var filterLanguage = isReversed ? video.LanguageTranslate : video.LanguageOrigin;
            var existedTermsList = video.TermList
                .Where(term =>
                    !string.IsNullOrEmpty(term.Name) &&
                    term.Language != null &&
                    term.Language.Oid == filterLanguage?.Oid)
                .ToDictionary(term => term.Name.ToLower(), term => term);


            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            int existCount = 0;
            int newCount = 0;            
            if (e.SelectedChoiceActionItem.Id.Contains("Dictionary") || e.SelectedChoiceActionItem.Id.Equals("All"))
            {
                if (video.LanguageOrigin is null)
                {
                    Tools.ShowMessage(Application, "Lỗi", "Ngữ gốc bị trống", InformationType.Error, 10000);
                    return;
                }
                if (video.LanguageTranslate is null)
                {
                    Tools.ShowMessage(Application, "Lỗi", "Ngữ dịch bị trống", InformationType.Error, 10000);
                    return;
                }
                //Chọn 1 từ điển để tra rồi nạp các thuật ngữ của từ điển có tồn tại trong bài vào tab Thuật ngữ                
                using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                {
                    dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
                    {
                        if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                            ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                        {
                            ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                        }
                    };
                    ShowViewParameters showViewParameters = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        CreateAllControllers = true,
                        Context = TemplateContext.LookupWindow,
                    };
                    showViewParameters.Controllers.Add(dc);
                    System.Type type = typeof(Module.BusinessObjects.Dictionary);
                    string viewId = Application.FindLookupListViewId(type);
                    //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                    if (string.IsNullOrEmpty(viewId))
                        return;
                    var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(dictionaryObjectSpace,
                            type, viewId, CollectionSourceMode.Normal);
                    var listview = Application.CreateListView(viewId, collectionSource, false);
                    //dc.AcceptAction.Caption = "Chọn " + caption;
                    dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                    {
                        bool isReversed = e.SelectedChoiceActionItem.Id.Contains("Translate");
                        bool searchInSubtitle = isReversed;
                        string caption = e.SelectedChoiceActionItem.Id.Equals("All") ? "Đang nạp từ phức" : "Đang nạp từ";

                        termService.ImportTermsFromDictionaries(video,args.AcceptActionArgs.SelectedObjects.Cast<Dictionary>(), View.ObjectSpace, existedTermsList, stopWatch, isReversed, searchInSubtitle);
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                    };
                    dc.SaveOnAccept = false;
                    dc.CancelAction.Active.SetItemValue("", false);
                    showViewParameters.CreatedView = listview;
                    Application.ShowViewStrategy.ShowView(showViewParameters,
                        new ShowViewSource(Frame, dc.AcceptAction));
                }
            }
            if (e.SelectedChoiceActionItem.Id.Contains("CompoundWord"))
            {
                //Khi tìm từ ghép bỏ qua các từ đơn phi thuật
                var exceptionWordList = new System.Collections.Generic.List<string>();
                if (video != null && video.LanguageOrigin != null)
                {
                    //2023-08-10 Chức năng Nạp thuật ngữ > Từ phức trong từ điển: Sẽ lấy danh sách từ phức (2 từ trở lên) của từ điển để tra trong bài nếu tồn tại thì nạp
                    var baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ? and Contains([Name], ' ')", video.LanguageOrigin.Oid);
                    exceptionWordList = ObjectSpace.GetObjects<ExceptionWord>(baseCriteria).Select(m => m.Name).ToList();
                }

                termService.ImportCompoundWordFromDictionary(video, exceptionWordList, existedTermsList, e, stopWatch, ref existCount, isReversed);
            }
            if (e.SelectedChoiceActionItem.Id.Contains("UpperCase") || e.SelectedChoiceActionItem.Id.Equals("All"))
            {
                int successCount = termService.ImportTermUpperCaseAndNumberCharacter(video, existedTermsList, true, stopWatch, isReversed);                
            }
            if (e.SelectedChoiceActionItem.Id.Contains("NumberCharacter") || e.SelectedChoiceActionItem.Id.Equals("All"))
            {
                int successCount = termService.ImportTermUpperCaseAndNumberCharacter( video, existedTermsList, false, stopWatch, isReversed);
            }
            if (e.SelectedChoiceActionItem.Id.Contains("DateTime") || e.SelectedChoiceActionItem.Id.Equals("All"))
            {
                //Nạp tham số
                //2025-02-12: Bỏ tham số này
                //var parameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MaxTermLocationWhenImport", "1");
                //int maxTermLocation = parameter.GetIntValue();                

                // 2023 - 06 - 01: Start: Bỏ trường hợp này
                //string[] removeWords = new string[] { "I", "I'm","I've","I'd","I'll" };
                //System.Collections.Generic.IList<string> result = new System.Collections.Generic.List<string>();
                //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
                var removesTerms = new System.Collections.Generic.List<string>();
                var resultTerm = new System.Collections.Generic.Dictionary<string, Module.BusinessObjects.Term>();
                int add = 0;
                System.Collections.Generic.IDictionary<string, int> resultQuantity = new System.Collections.Generic.Dictionary<string, int>();
                //System.Collections.Generic.IDictionary<string, int> resultPosition = new System.Collections.Generic.Dictionary<string, int>();
                //int position = 0;

                decimal countNumber = 0;
                int total = video.AudioList.Count;
                string caption = e.SelectedChoiceActionItem.Id.Equals("All") ? "Đang nạp thời gian" : " ";

                var IsReversed = e.SelectedChoiceActionItem.Id.Contains("Translate");

                foreach (var audio in video.GetAudioListWithSort())
                {
                    var content = audio.Content;
                    if (IsReversed)
                        content = audio.Subtitle;

                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    if (string.IsNullOrEmpty(content))
                        continue;
                    //int position = 0;
                    //Cắt theo dòng                    
                    //var rows = audio.Content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    var sentencesArray = Module.Helpers.TextHelper.GetSentences(content);
                    for (int m = 0; m < sentencesArray.Count(); m++)
                    {
                        //Vị trí ở câu sẽ giảm số thuật vị phải kiểm tra                        
                        var contents = sentencesArray[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        //int rowPosition = 0;
                        for (int n = 0; n < contents.Length; n++)
                        {
                            int workPositionInRow = n;
                            string word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[n]);
                            string dateTimeText = "";
                            bool hasNumber = false;
                            foreach (var c in word)
                            {
                                if (char.IsNumber(c))
                                {
                                    hasNumber = true;
                                    dateTimeText += c;
                                }
                                else if (hasNumber)
                                {
                                    if (c == '-' || c == '/') //|| c == ':')
                                        dateTimeText += c;
                                    else
                                        break;
                                }
                            }
                            var lowerWord = word.ToLower();
                            //Nếu đã tồn tại thì bỏ qua
                            if (existedTermsList.ContainsKey(lowerWord))
                                continue;
                            if (hasNumber && !removesTerms.Contains(lowerWord))
                            {
                                System.DateTime datetime;
                                if (System.DateTime.TryParse(dateTimeText, out datetime))
                                {
                                    Module.BusinessObjects.Term term = null;
                                    if (!resultTerm.ContainsKey(lowerWord))
                                    {
                                        resultQuantity.Add(word, 1);
                                        //Tạo mới thuật ngữ và thuật vị
                                        //Nếu tồn tại thì bỏ qua
                                        add++;
                                        term = ObjectSpace.CreateObject<Module.BusinessObjects.Term>();
                                        term.Name = word;
                                        term.Quantity = resultQuantity[word];
                                        term.DateValue = datetime;
                                        //video.TermList.Add(term);
                                        resultTerm.Add(lowerWord, term);
                                        //Thêm vào thuật vị
                                        //Thêm 1 thuật vị
                                    }
                                    else
                                    {
                                        term = resultTerm[lowerWord];
                                    }                                    
                                    if (video.WithTermPosition || term.TermLocationList.Count == 0)
                                    {
                                        var currentSentence = m + 1;
                                        var currentLocation = workPositionInRow + 1;
                                        bool overlap = false;
                                        bool validate = true;
                                        if(term.TermLocationList.Count > 0 && video.WithTermPosition)
                                        {
                                            if (audio.TermLocationList != null && audio.TermLocationList.Count > 0)
                                            {
                                                //validate = resultTerm[lowerWord].CheckLocationIsValidate(listTermLocations, audio.Start.Value, currentSentence, currentLocation, ref overlap);
                                                bool flag = false;
                                                validate = VideoService.CheckAndUpdateLocationInTermIsValidate(video, word, audio, currentSentence, currentLocation, true, ref overlap, ref flag);
                                            }
                                        }
                                        if (validate)
                                        {
                                            term.TermLocationList.Add(new Module.BusinessObjects.TermLocation(term.Session)
                                            {
                                                Term = term,
                                                //Vị trí của mảng nhỏ hơn 1 so với vị trí thực tế
                                                //Location = position + workPositionInRow + 1,
                                                Location = currentLocation,
                                                Audio = audio,
                                                Sentence = currentSentence,
                                                Overlap = overlap
                                            });
                                            if (overlap)
                                                term.Overlap = true;
                                        }
                                        
                                    }
                                }
                            }
                        }
                        //Cắt row để không bị lệch khi thay đổi vị trí
                        //position += sentencesArray[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }
                    
                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    if (total > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / total).ToString("p0"), stopWatch.Elapsed, true);
                    }
                }
                if (add > 0)
                {
                    //Thêm những thuật ngữ mới vào đối tượng
                    foreach (var key in resultTerm.Keys)
                    {
                        //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
                        if (resultTerm[key].Video is null && !removesTerms.Contains(key))
                            video.TermList.Add(resultTerm[key]);
                    }

                    Tools.RefreshGridView(View);
                    string message = "Có " + add + " được nạp";
                    Tools.ShowMessage(Application, "Thành công", message, InformationType.Success, 10000);
                }                
                if (total > 5)
                {
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                }
                if (stopWatch.Elapsed.TotalMinutes > 1)
                {
                    //Nếu nhỏ hơn 1 phút thì không log
                    //StartTime : Chức năng : SL multiselect : SL kết quả : Tổng thời gian xử lý (phút làm tròn)
                    //video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startime.ToString("dd/MM/yyyy h:mm"), ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, total, add, System.Math.Round(stopWatch.Elapsed.TotalMinutes, 0));
                    //video.LogToNote(startime, ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, total, System.Convert.ToInt32(countNumber), stopWatch.Elapsed);
                }
                //if (e.SelectedChoiceActionItem.Id.Equals("DateTime"))
                //    Tools.ShowMessage(Application, "Lỗi", "Chức năng này không khả dụng", InformationType.Error);

            }
            if (e.SelectedChoiceActionItem.Id.Equals("Term") || e.SelectedChoiceActionItem.Id.Equals("All"))
            {                
                //2023-06-01: End : Bỏ trường hợp này
                //System.Collections.Generic.IDictionary<string, int> resultQuantity = new System.Collections.Generic.Dictionary<string, int>();
                //System.Collections.Generic.IDictionary<string, int> resultPosition = new System.Collections.Generic.Dictionary<string, int>();
                //TermLocation
                System.Collections.Generic.IDictionary<string, Module.BusinessObjects.Term> resultTerm
                    = new System.Collections.Generic.Dictionary<string, Module.BusinessObjects.Term>();
                //foreach(var existTerm in video.TermList)
                //{
                //    resultTerm.Add(existTerm.Name, existTerm);
                //}
                int add = 0;

                //Nạp tham số
                //2025-02-12: Bỏ tham số này
                //var parameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MaxTermLocationWhenImport", "1");
                //int maxTermLocation = parameter.GetIntValue();

                decimal countNumber = 0;
                decimal total = video.AudioList.Count * (decimal)1.3;
                string caption = e.SelectedChoiceActionItem.Id.Equals("All") ? "Đang nạp thuật ngữ" : " ";
                var audioListWithSort = video.GetAudioListWithSort();
                foreach (var audio in audioListWithSort)
                {
                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    if (string.IsNullOrEmpty(audio.Content))
                        continue;
                    
                    //int position = 0;
                    //Kiểm tra xem thuật ngữ có sẵn thì loại                                               
                    //var rows = audio.Content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    var sentencesArray = Module.Helpers.TextHelper.GetSentences(audio.Content);
                    for (int i = 0; i < sentencesArray.Count(); i++)
                    {
                        //var contents = sentencesArray[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        var wordsArray = Module.Helpers.TextHelper.GetWords(sentencesArray[i]);
                        //Tách position và rowPosition để tránh trường hợp từ thay thế khác từ hiện tại
                        int rowPosition = 0;
                        foreach (var word in wordsArray)
                        {
                            rowPosition++;
                            if (string.IsNullOrEmpty(word))
                                continue;
                            //2023-06-01: Bỏ trường hợp nạp số  
                            bool hasNumber = false;
                            foreach (var c in word)
                            {
                                if (char.IsNumber(c))
                                {
                                    hasNumber = true;
                                    break;
                                }
                            }
                            if (hasNumber)
                                continue;
                            var termName = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(word);
                            if (string.IsNullOrEmpty(termName))
                                continue;
                            var lowerWord = termName.ToLower();
                            //Nếu thuật ngữ đã tồn tại trong danh sách thì bỏ qua, không xử lý tiếp
                            if (existedTermsList.ContainsKey(lowerWord))
                                continue;
                            var currentSentence = i + 1;
                            bool overlap = false;
                            bool flag = false;
                            if (video.WithTermPosition && audio.TermLocationList != null && audio.TermLocationList.Count > 0)
                            {                               
                                if (!VideoService.CheckAndUpdateLocationInTermIsValidate(video, termName, audio, currentSentence, rowPosition, true, ref overlap, ref flag))
                                {
                                    continue;
                                }
                            }
                            
                            //2023-10-27: Đơn giản hóa tách câu tách từ : bỏ 2 list kí tự để Thuật vị chính xác                           
                            if (resultTerm.ContainsKey(lowerWord))
                            {
                                //resultQuantity[key] = resultQuantity[key] + 1;   
                                var term = resultTerm[lowerWord];
                                if (term.Video is null)
                                {
                                    //Chỉ tăng số lượng những thuật ngữ mới
                                    term.Quantity++;
                                    //Nếu kèm thuật vị khi nạp
                                    if (video.WithTermPosition)
                                    {
                                        var newLocation = new Module.BusinessObjects.TermLocation(term.Session)
                                        {
                                            Term = term,
                                            //Location = position + rowPosition,
                                            Location = rowPosition,
                                            Audio = audio,
                                            Sentence = currentSentence,
                                            Overlap = overlap
                                        };
                                        term.TermLocationList.Add(newLocation);                                        
                                    }
                                }
                                if (overlap && !term.Overlap)
                                    term.Overlap = true;
                            }
                            else
                            {
                                //Kiểm tra team này có trog phi thuật không không
                                DevExpress.Data.Filtering.CriteriaOperator baseCriteria =
                                    (video != null && video.LanguageOrigin != null) ?
                                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", video.LanguageOrigin.Oid) : null;
                                var exceptionWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", lowerWord), baseCriteria);
                                var exceptionWord = ObjectSpace.FindObject<ExceptionWord>(exceptionWordCriteria);
                                if (exceptionWord is null)
                                {
                                    //2023-05-31: Khi Nạp thuật ngữ và kiểm tra phi thuật tiếng Anh thì các trường hợp sau là phi thuật hợp lệ
                                    //2023-06-09 Khớp Phi thuật:
                                    //-Phi thuật = Thuật ngữ hoặc =                                            
                                    string otherWord = termName;
                                    string otherWord2 = null;
                                    if (otherWord.EndsWith("ed") || otherWord.EndsWith("er"))
                                    {
                                        //- Thuật ngữ - es
                                        //- Thuật ngữ - ed
                                        otherWord = otherWord.Substring(0, otherWord.Length - 2);
                                    }
                                    else if (otherWord.EndsWith("ing"))
                                    {
                                        //- Thuật ngữ - ing(+e hoặc không)
                                        otherWord = otherWord.Substring(0, otherWord.Length - 3);
                                        otherWord2 = otherWord + "e";
                                    }
                                    //else if (otherWord.EndsWith("inge"))
                                    //{
                                    //    //- Thuật ngữ - ing(+e hoặc không)
                                    //    otherWord = otherWord.Substring(0, otherWord.Length - 4);
                                    //}
                                    else if (otherWord.EndsWith("es"))
                                    {
                                        // -Thuật ngữ - s(phi thuật danh từ)
                                        //-Khi thuật ngữ -s hoặc es mà khớp phi thuật thì phi thuật có thể là danh từ hoặc động từ
                                        otherWord = otherWord.Substring(0, otherWord.Length - 2);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ? or WordType = ?", WordType.Noun, WordType.Verb), baseCriteria);
                                    }
                                    else if (otherWord.EndsWith("er"))
                                    {
                                        //- Thuật ngữ - er > danh từ
                                        otherWord = otherWord.Substring(0, otherWord.Length - 2);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ?", WordType.Noun), baseCriteria);
                                    }
                                    else if (otherWord.EndsWith("s"))
                                    {
                                        // -Thuật ngữ - s(phi thuật danh từ)
                                        //-Khi thuật ngữ -s hoặc es mà khớp phi thuật thì phi thuật có thể là danh từ hoặc động từ
                                        otherWord = otherWord.Substring(0, otherWord.Length - 1);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ? or WordType = ?", WordType.Noun, WordType.Verb), baseCriteria);
                                    }
                                    else if (otherWord.EndsWith("d"))
                                    {
                                        //- Thuật ngữ - d(phi thuật động từ)
                                        otherWord = otherWord.Substring(0, otherWord.Length - 1);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ?", WordType.Verb), baseCriteria);
                                    }
                                    else if (otherWord.EndsWith("ly"))
                                    {
                                        //- Thuật ngữ - ly(phi thuật tính từ)
                                        otherWord = otherWord.Substring(0, otherWord.Length - 2);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ?", WordType.Adjective), baseCriteria);
                                    }
                                    else if (otherWord.EndsWith("th"))
                                    {
                                        //- Thuật ngữ - th(phi thuật số từ)
                                        otherWord = otherWord.Substring(0, otherWord.Length - 2);
                                        baseCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("WordType = ?", WordType.Numeral), baseCriteria);
                                    }
                                    if (!termName.Equals(otherWord, System.StringComparison.OrdinalIgnoreCase))
                                    {
                                        DevExpress.Data.Filtering.CriteriaOperator nameCriteria =
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", otherWord);
                                        if (!string.IsNullOrEmpty(otherWord2))
                                        {
                                            //- Thuật ngữ - ing(+e hoặc không)
                                            //Trường hợp bỏ ing thêm e
                                            nameCriteria = DevExpress.Data.Filtering.CriteriaOperator.Or(nameCriteria,
                                                DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", otherWord2));
                                        }
                                        exceptionWord = ObjectSpace.FindObject<ExceptionWord>
                                            (DevExpress.Data.Filtering.CriteriaOperator.And(nameCriteria, baseCriteria));
                                    }
                                }

                                add++;
                                var term = ObjectSpace.CreateObject<Module.BusinessObjects.Term>();
                                term.Name = termName;
                                if (exceptionWord != null)
                                {
                                    //term.NoneTerm = true;
                                    term.TermType = TermType.NoneTerm;
                                    term.WordType = exceptionWord.WordType;
                                }
                                term.Overlap = overlap;
                                term.Quantity = 1;
                                //term.Position = position + rowPosition;                                   
                                //video.TermList.Add(term);
                                resultTerm.Add(lowerWord, term);
                                //Thêm vào thuật vị
                                if (video.WithTermPosition)
                                {
                                    term.TermLocationList.Add(new Module.BusinessObjects.TermLocation(term.Session)
                                    {
                                        Term = term,
                                        //Location = position + rowPosition,
                                        Location = rowPosition,
                                        Audio = audio,
                                        Sentence = i + 1
                                    });
                                }
                            }                           
                        }
                        //position += sentencesArray[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }
                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    if (total > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / total).ToString("p0"), stopWatch.Elapsed, true);
                    }
                }

                if (add > 0)
                {
                    //Cấu trúc mới
                    video.TermList.AddRange(resultTerm.Values);
                    #region Cấu trúc cũ
                    //Xử lý giảm trừ số lượng thuật ngữ                    
                    //foreach (var w in resultTerm.Keys)
                    //{
                    //    //Bỏ những video cũ trước đó
                    //    if (resultTerm[w].Video != null)
                    //        continue;
                    //    foreach (Module.BusinessObjects.Term videoTerm in video.TermList)
                    //    {
                    //        bool exist = false;
                    //        if (videoTerm.Name.Contains(' '))
                    //        {
                    //            //Trường hợp thuật ngữ nhiều hơn 1 từ
                    //            var videoTermNames = videoTerm.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    //            foreach (var videoTermName in videoTermNames)
                    //            {
                    //                if (videoTermName.Equals(w, System.StringComparison.OrdinalIgnoreCase))
                    //                {
                    //                    if (videoTerm.Quantity != null)
                    //                    {
                    //                        resultTerm[w].Quantity -= videoTerm.Quantity.Value;
                    //                        exist = true;
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //            if (exist)
                    //                break;
                    //        }
                    //    }
                    //    //resultTerm[w].Video = video;
                    //    if (resultTerm[w].Quantity > 0)
                    //    {
                    //        video.TermList.Add(resultTerm[w]);
                    //    }
                    //    else
                    //    {
                    //        resultTerm[w].Delete();
                    //    }
                    //    if (total > 5 && countNumber < total)
                    //    {
                    //        countNumber++;
                    //        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), caption, stopWatch.Elapsed);
                    //    }
                    //}
                    #endregion
                    Tools.RefreshGridView(View);
                    Tools.ShowMessage(Application, "Thành công", "Có " + add + " được nạp");
                }
                else
                {
                    Tools.ShowMessage(Application, "Kết quả", "Không có thuật ngữ nào");
                }
                
                if (total > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                if (stopWatch.Elapsed.TotalMinutes > 1)
                {
                    //Nếu nhỏ hơn 1 phút thì không log
                    //StartTime : Chức năng : SL multiselect : SL kết quả : Tổng thời gian xử lý (phút làm tròn)
                    //video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startime.ToString("dd/MM/yyyy h:mm"), ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, total, add, System.Math.Round(stopWatch.Elapsed.TotalMinutes, 0));
                    //video.LogToNote(startime, ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, video.AudioList.Count, add, stopWatch.Elapsed);
                }
            }

            foreach (Module.BusinessObjects.Term term in video.TermList)
            {
                if (e.SelectedChoiceActionItem.Id.Contains("Translate") && term.Language == null)
                    term.Language = video.LanguageTranslate;
                else if (term.Language == null)
                    term.Language = video.LanguageOrigin;
            }

            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
            stopWatch.Stop();
            //string message = $"{newCount} thuật ngữ được tạo mới";
            if(existCount > 0)
            {
                Tools.ShowMessage(Application, "Kêt quả", $"{existCount} thuật vị đã tồn tại", InformationType.Info, 10000);
            }

        



            #endregion ImportTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0600            Oid: 2eed095b-3531-4145-b313-7a156417d12f
		private void UpdatePosition_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(UpdatePosition), "Thuật vị");              
      
            #region UpdatePositionImportCode
            //var parameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MaxTermLocationWhenImport", "1");
            //int maxTermLocation = parameter.GetIntValue();

            if (e.SelectedChoiceActionItem.Id.Equals("Open"))
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
                    System.Type type = typeof(Module.BusinessObjects.TermLocation);
                    //string viewId = Application.FindListViewId(type);
                    string viewId = "Term_TermLocationList_ListView";
                    if (string.IsNullOrEmpty(viewId))
                        return;
                    //var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                    CollectionSource collectionSource = new CollectionSource(View.ObjectSpace, type);
                    DevExpress.Xpo.XPBaseCollection xpCollectionSource = collectionSource.Collection as DevExpress.Xpo.XPBaseCollection;
                    xpCollectionSource.LoadingEnabled = false;
                    foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                    {
                        xpCollectionSource.BaseAddRange(term.TermLocationList);
                    }
                    //CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
                    //       type, viewId, CollectionSourceMode.Normal);
                    //collectionSource.BeginUpdateCriteria();
                    //collectionSource.Criteria["OpenTermLocations"] = criteria;
                    //collectionSource.EndUpdateCriteria();

                    var listView = Application.CreateListView(viewId, collectionSource, false);
                    listView.AllowNew["OpenTermLocations"] = false;
                    //dc.AcceptAction.Caption = "Chọn " + caption;
                    dc.AcceptAction.Active.SetItemValue("", false);
                    dc.SaveOnAccept = false;
                    dc.CancelAction.Active.SetItemValue("", false);
                    showViewParameters.CreatedView = listView;
                    Application.ShowViewStrategy.ShowView(showViewParameters,
                        new ShowViewSource(Frame, dc.AcceptAction));
                }


                return;
            }            
            char charTagNote = '(';
            var total = View.SelectedObjects.Count;
            if (total < 1)
                return;
            var startime = System.DateTime.Now;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();            
            decimal countNumber = 0;
            //Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Xóa thuật vị", stopWatch.Elapsed);
            if (e.SelectedChoiceActionItem.Id.Equals("OverlapCheck"))
            {                
                int totalCheck = 0;
                int totalChange = 0;
                int totalFlag = 0;
                decimal totalSelectObject = View.SelectedObjects.Count;
                
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    var overlap = false;
                    foreach (var termLocation in term.TermLocationList)
                    {
                        totalCheck++;
                        //2023-11-09: Chỉ kiểm tra những thuật vị có check Overlap
                        if (termLocation.Overlap)
                        {
                            var termLocationOverlap = TermLocationService.CheckOverlap(termLocation, true);
                            if (termLocation.Overlap != termLocationOverlap)
                            {
                                totalChange++;
                                termLocation.Overlap = termLocationOverlap;
                            }
                        }
                        if (termLocation.Overlap)
                        {
                            overlap = true;

                        }
                    }
                    //if (term.Flag)
                    //    term.AddTextNode(e.SelectedChoiceActionItem.Caption);
                    countNumber++;
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / totalSelectObject).ToString("p0"), " ", stopWatch.Elapsed);
                    if (term.Overlap != overlap)
                        term.Overlap = overlap;
                    if (term.Overlap)
                        totalFlag++;
                }
                stopWatch.Stop();
                return;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("All"))
            {
                //Xóa thuật vị trước 
                foreach (Term term in View.SelectedObjects)
                {
                    term.Session.Delete(term.TermLocationList);
                    if(total > 200)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Xóa thuật vị", stopWatch.Elapsed);
                    }                    
                }
            }
            
            countNumber = 0;
            var status = View.ObjectSpace.FindObject<Module.SystemObjects.Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'Location'"));
            foreach (Term term in View.SelectedObjects)
            {
                if(e.SelectedChoiceActionItem.Id.Equals("All"))
                    termService.UpdateTermPosition(term, true, charTagNote);
                else if (e.SelectedChoiceActionItem.Id.Equals("Quantity"))
                {
                    //Cập nhật số lượng
                    if (term.TermLocationList is null || term.TermLocationList.Count == 0)
                        termService.UpdateTermPosition(term,true, charTagNote);
                    else
                        term.Quantity = term.TermLocationList.Count;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Location"))
                    termService.UpdatePositionLocation(term,true, charTagNote);
                if (term.Status is null || (term.Status != null && !"Location".Equals(term.Status.Code)))
                {                    
                    if (status != null)
                        term.Status = status;
                }
                countNumber++;                
                Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Cập nhật thuật vị", stopWatch.Elapsed);
            }
            if(stopWatch.Elapsed.TotalMinutes > 1)
            {
                //Nếu nhỏ hơn 1 phút thì không log
                var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                if (video != null)
                {
                    //StartTime : Chức năng : SL multiselect : SL kết quả : Tổng thời gian xử lý (phút làm tròn)
                    //video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startime.ToString("dd/MM/yyyy h:mm"), UpdatePosition.Caption, total, countNumber, System.Math.Round(stopWatch.Elapsed.TotalMinutes, 0));
                    videoService.LogToNote(video, startime, UpdatePosition.Caption, total, System.Convert.ToInt32(countNumber), stopWatch.Elapsed);
                }
            }
            stopWatch.Stop();
            Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);











            #endregion UpdatePositionImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0916            Oid: 4e226c44-2d5e-408e-a8e8-740f47ad9bc7
		private void OpenTermElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OpenTermElement), "Mở thành phần");              
      
            #region OpenTermElementImportCode
            var currentObject = View.CurrentObject as Module.BusinessObjects.Term;
            if (currentObject is null)
                return;
            var firstTermLocation = termService.GetTermLocationsByOrder(currentObject).FirstOrDefault();
            if(firstTermLocation is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thành phần", InformationType.Error);
                return;
            }
            var element = firstTermLocation.GetAudioFromElement();
            if (element is null)
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



            #endregion OpenTermElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0927            Oid: db0e2ea5-264a-47ff-b12b-4a288ae992ab
		private void SplitTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SplitTerm), "Tách thuật ngữ");              
      
            #region SplitTermImportCode
//2023-07-25: YC: Chức năng mới Tách thuật ngữ: chọn Tách đầu/Tách cuối : giữ mọi giá trị trường ban đầu, Thuật vị và Dịch ngữ cảnh lại
            //Từ sau khi tách phân loại Viết tắt, Viết hoa, Số và ký tự
            var term = View.CurrentObject as Module.BusinessObjects.Term;
            if (term is null)
                return;            
            if (string.IsNullOrEmpty(term.Name) || term.Video is null)
                return;
            int susscess = 0;
            int termLocationCount = 0;
            bool deletedTerm = false;
            bool existedTerm = false;
            string newTermText = null;
            if (e.SelectedChoiceActionItem.Id.Equals("First"))
            {
                var index = term.Name.IndexOf(' ');
                if(index > 0)
                {
                    newTermText = term.Name.Substring(0, index).Trim();                    
                    //Xử lý thuật ngữ hiện tại
                    term.Name = term.Name.Substring(index).Trim();                                        
                }
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Last"))
            {
                var index = term.Name.LastIndexOf(' ');
                if (index > 0)
                {
                    newTermText = term.Name.Substring(index).Trim();                    
                    //Xử lý thuật ngữ hiện tại
                    term.Name = term.Name.Substring(0, index).Trim();                    
                }
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("TwoFirst"))
            {
                //Hai từ đầu
                var index = term.Name.IndexOf(' ');
                if (index > 0 && index < term.Name.Length - 1)
                {
                    var index2 = term.Name.IndexOf(' ', index + 1);
                    if(index2 > 0)
                    {
                        newTermText = term.Name.Substring(0, index2).Trim();
                        //Xử lý thuật ngữ hiện tại
                        term.Name = term.Name.Substring(index2).Trim();
                    }
                    
                }
            }
            if (!string.IsNullOrEmpty(newTermText))
            {
                foreach (var refTerm in term.Video.TermList)
                {
                    if (!deletedTerm && !refTerm.Oid.Equals(term.Oid) && term.Name.Equals(refTerm.Name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        //Nếu tồn tại thuật ngữ thì xóa thuật ngữ hiện tại đi                        
                        deletedTerm = true;
                    }
                    if (!existedTerm && newTermText.Equals(refTerm.Name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        existedTerm = true;                        
                        termService.UpdatePosition(refTerm, true);
                        termService.TranslateTerm(refTerm, ref susscess, ref termLocationCount);
                    }
                    if (existedTerm && term.IsDeleted)
                        break;
                }
                //Kiểm tra thuật ngữ mới nếu chưa tồn tại thì tạo mới                                        
                if (!existedTerm)
                {
                    var newTerm = new Module.BusinessObjects.Term(term.Session);
                    newTerm.Video = term.Video;
                    term.Video.TermList.Add(newTerm);
                    newTerm.Name = newTermText;
                    termService.UpdateTermType(newTerm);
                    termService.UpdatePosition(newTerm, true);
                    termService.TranslateTerm(newTerm, ref susscess, ref termLocationCount);
                    newTerm.SetDefaultUpdate();

                }
                if (deletedTerm)
                {
                    //Nếu tồn tại thì phải xóa
                    term.Delete();
                }
                else
                {
                    //Nếu chưa tồn tại thì phải cập nhật lại thuật vị và dịch ngữ cảnh
                    term.GoogleTranslate = null;
                    term.Translate = null;
                    termService.UpdatePosition(term, true);
                    termService.TranslateTerm(term, ref susscess, ref termLocationCount);
                    term.SetDefaultUpdate();
                }
                View.CurrentObject = term;                
            }


            #endregion SplitTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0914            Oid: d93be025-b803-4a17-9656-bac43399d233
		private void ExportTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExportTerm), "Xuất thuật ngữ");              
      
            #region ExportTermImportCode
            //29-05-2023
            //Phi thuật có thêm trường Nghĩa (Meaning) để tham khảo
            //Chức năng Xuất phi thuật: Cần copy trường Dịch máy trên Thuật ngữ vào Nghĩa trên Phi thuật,
            //đồng thời cũng tạo Phi thuật cho Ngữ dịch với  giá trị đảo ngược của Phi thuật Ngữ gốc
            var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            if (e.SelectedChoiceActionItem.Id.Equals("NonTerm"))
            {
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (!string.IsNullOrEmpty(term.Name))
                    {
                        //2023-06-01: Lưu ý khi xuất phi thuật và xuất từ điển thì phải chuyển về chữ thường không để chữ hoa
                        string termName = term.Name.ToLower();
                        var exceptionWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", termName);
                        if (video != null && video.LanguageOrigin != null)
                            exceptionWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(exceptionWordCriteria,
                                DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", video.LanguageOrigin.Oid));
                        if (ObjectSpace.FindObject<ExceptionWord>(exceptionWordCriteria) == null)
                        {
                            var objectSpaceExceptionWord = Application.CreateObjectSpace(typeof(ExceptionWord));
                            var exceptionWord = objectSpaceExceptionWord.CreateObject<ExceptionWord>();
                            exceptionWord.Name = termName;
                            if (!string.IsNullOrEmpty(term.GoogleTranslate))
                                exceptionWord.Meaning = term.GoogleTranslate.ToLower();
                            if (video != null && video.LanguageOrigin != null)
                                exceptionWord.Language = exceptionWord.Session.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageOrigin.Oid);
                            exceptionWord.WordType = term.WordType;
                            exceptionWord.Session.CommitTransaction();
                        }
                        //29-05-2023 tạo Phi thuật cho Ngữ dịch với giá trị đảo ngược của Phi thuật Ngữ gốc
                        if (video.LanguageTranslate != null && !string.IsNullOrEmpty(term.GoogleTranslate))
                        {
                            var objectSpaceExceptionWord = Application.CreateObjectSpace(typeof(ExceptionWord));
                            termName = term.GoogleTranslate.ToLower();
                            exceptionWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.Parse(
                                "Name = ? and Language.Oid = ?", termName, video.LanguageTranslate.Oid);
                            if (ObjectSpace.FindObject<ExceptionWord>(exceptionWordCriteria) == null)
                            {
                                var exceptionWord = ObjectSpace.CreateObject<ExceptionWord>();
                                exceptionWord.Name = termName;
                                exceptionWord.Meaning = term.Name.ToLower();
                                exceptionWord.Language = exceptionWord.Session.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageTranslate.Oid);
                                exceptionWord.WordType = term.WordType;
                                exceptionWord.Session.CommitTransaction();
                            }
                        }
                    }
                    //2023-08-02 bỏ trường này
                    //term.NoneTerm = true;
                    term.TermType = TermType.NoneTerm;
                }
                Tools.RefreshGridView(View);
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Dictionary"))
            {
                if (video.LanguageOrigin is null || video.LanguageTranslate is null)
                {
                    Tools.ShowMessage(Application, "Lỗi", "Ngữ gốc hoặc ngữ dịch không được trống", InformationType.Error);
                    return;
                }
                //Copy các từ được chọn vào từ điển chỉ định: Tên/Dịch/Ngữ gốc/Ngữ dịch/Loại
                using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                {
                    dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
                    {
                        if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                            ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                        {
                            ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                        }
                    };
                    ShowViewParameters showViewParameters = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        CreateAllControllers = true,
                        Context = TemplateContext.LookupWindow,
                    };

                    showViewParameters.Controllers.Add(dc);
                    System.Type type = typeof(Module.BusinessObjects.Dictionary);
                    string viewId = Application.FindLookupListViewId(type);
                    //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                    if (string.IsNullOrEmpty(viewId))
                        return;
                    var dictionaryObjectSpace = Application.CreateObjectSpace(type);
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(dictionaryObjectSpace,
                            type, viewId, CollectionSourceMode.Normal);
                    var listview = Application.CreateListView(viewId, collectionSource, false);
                    //dc.AcceptAction.Caption = "Chọn " + caption;
                    dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                    {
                        if (args.AcceptActionArgs.CurrentObject is Dictionary)
                        {
                            //2023-08-04:
                            //Mỗi thuật vị mà Dịch có giá trị sẽ được xuất vào Dịch ngữ với
                            //- STT: ưu tiên số lượng nhiều
                            //- Ngữ cảnh: 1 cấu(ngữ gốc) chứa Dịch đó < 250 kí tự
                            //Nếu thuật ngữ đó chưa tồn tại trong từ điển thì tạo mới:
                            //            -Tạo và copy đủ trường: Tên / Dịch / Ngữ gốc / Ngữ dịch / Ngữ cảnh(với Dịch là giá trị ưu tiên 1(số đông))
                            //            - Tuy nhiên: tạo Dịch ngữ cho cả 2 giá trị ở trên với STT ưu tiên là 1
                            //Nếu thuật ngữ đã tồn tại thì xem xét bổ sung Dịch nếu khác các giá trị dịch đang sẵn có

                            var dictionary = (Dictionary)args.AcceptActionArgs.CurrentObject;
                            int result = 0;
                            foreach (Term term in View.SelectedObjects)
                            {
                                if (string.IsNullOrEmpty(term.Name))
                                    continue;
                                var listWord = new System.Collections.Generic.Dictionary<string, int>();
                                var listContext = new System.Collections.Generic.Dictionary<string, string>();
                                foreach (var termLocation in term.TermLocationList)
                                {
                                    if (string.IsNullOrEmpty(termLocation.Translate))
                                        continue;
                                    string translate = termLocation.Translate.ToLower();
                                    if (listWord.ContainsKey(translate))
                                    {
                                        listWord[translate]++;
                                    }
                                    else
                                    {
                                        listWord.Add(translate, 1);
                                        var audio = termLocation.GetAudioFromElement();
                                        if (audio != null && !string.IsNullOrEmpty(audio.Content))
                                        {
                                            string content = audio.Content;
                                            var rows = audio.Content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                                            if (termLocation.Sentence != null && (termLocation.Sentence > 0 && (termLocation.Sentence - 1 < rows.Length)))
                                            {
                                                content = rows[termLocation.Sentence.Value - 1];
                                            }
                                            if (content.Length > 250)
                                            {
                                                //Nếu dài hơn thì lấy từ giữa câu
                                                if (termLocation.Location != null && termLocation.Location > 250)
                                                    content = audio.Content.Substring(termLocation.Location.Value - 125, 250);
                                                else
                                                    content = content.Substring(0, 250);
                                            }
                                            if (!string.IsNullOrEmpty(content))
                                            {
                                                listContext.Add(translate, content);
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(term.Translate) && string.IsNullOrEmpty(Module.Helpers.TextHelper.KeyListContains(listWord.Keys, term.Translate)))
                                {
                                    listWord.Add(term.Translate.ToLower(), 1);
                                }
                                if (listWord.Keys.Count == 0)
                                    continue;
                                bool modify = false;
                                var dictionaryWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ? and Dictionary.Oid = ?", term.Name, dictionary.Oid);
                                if (video.LanguageOrigin != null)
                                    dictionaryWordCriteria = DevExpress.Data.Filtering.CriteriaOperator.And(dictionaryWordCriteria,
                                        DevExpress.Data.Filtering.CriteriaOperator.Parse("LanguageOrigin.Oid = ?", video.LanguageOrigin.Oid));
                                DictionaryWord dictionaryWord = ObjectSpace.FindObject<DictionaryWord>(dictionaryWordCriteria);
                                //Sort theo giá trị lớn nhất
                                listWord = listWord.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                if (dictionaryWord == null)
                                {
                                    string maxKey = listWord.Keys.First();
                                    //Chỉ lưu vào dịch máy
                                    if (!string.IsNullOrEmpty(maxKey))
                                    {
                                        //2023-06-01: Chức năng Xuất phi thuật và Xuất từ điển: cần chuyển đổi hết từ về chữ thường
                                        //Copy các từ được chọn vào từ điển chỉ định: Tên/Dịch/Ngữ gốc/Ngữ dịch/Loại
                                        dictionaryWord = new DictionaryWord(dictionary.Session);
                                        dictionaryWord.Dictionary = dictionary;
                                        dictionary.DictionaryWordList.Add(dictionaryWord);
                                        //2023-08-04: - Tạo và copy đủ trường: Tên/Dịch/Ngữ gốc/Ngữ dịch/Ngữ cảnh (với Dịch là giá trị ưu tiên 1 (số đông))
                                        dictionaryWord.Name = term.Name.ToLower();
                                        dictionaryWord.Translate = maxKey;
                                        if (listContext.ContainsKey(maxKey))
                                            dictionaryWord.Sentence = listContext[maxKey];
                                        dictionaryWord.WordType = term.WordType;
                                        dictionaryWord.LanguageOrigin = dictionaryWord.Session.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageOrigin.Oid);
                                        dictionaryWord.LanguageTranslate = dictionaryWord.Session.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageTranslate.Oid);
                                        dictionaryWord.Session.CommitTransaction();
                                        modify = true;
                                    }

                                }
                                int order = 0;
                                foreach (var translateWord in dictionaryWord.TranslateWordList)
                                {
                                    if (translateWord.Language != null && translateWord.Language.Oid.Equals(video.LanguageTranslate.Oid))
                                    {
                                        if (translateWord.Order != null && translateWord.Order > order)
                                            order = translateWord.Order.Value;
                                    }
                                }
                                foreach (var word in listWord.Keys)
                                {
                                    bool exist = false;
                                    foreach (var translateWord in dictionaryWord.TranslateWordList)
                                    {
                                        if (translateWord.Language != null && translateWord.Language.Oid.Equals(video.LanguageTranslate.Oid)
                                                && word.Equals(translateWord.Name, System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            exist = true;
                                            break;
                                        }
                                    }
                                    if (!exist)
                                    {
                                        order++;
                                        //2023-06-01: Chức năng Xuất phi thuật và Xuất từ điển: cần chuyển đổi hết từ về chữ thường
                                        var translateWord = new TranslateWord(dictionaryWord.Session);
                                        dictionaryWord.TranslateWordList.Add(translateWord);
                                        translateWord.DictionaryWord = dictionaryWord;
                                        //2023-08-04: - Tạo và copy đủ trường: Tên/Dịch/Ngữ gốc/Ngữ dịch/Ngữ cảnh (với Dịch là giá trị ưu tiên 1 (số đông))
                                        // 2023 - 05 - 31: Nạp vào thì nạp dịch máy vào từ điển
                                        //2023-08-04: lưu dịch, không phải dịch máy
                                        //translateWord.Name = term.GoogleTranslate.ToLower();
                                        translateWord.Name = word;
                                        translateWord.Language = translateWord.Session.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageTranslate.Oid);
                                        if (listContext.ContainsKey(word))
                                            translateWord.Context = listContext[word];
                                        translateWord.Order = order;
                                        translateWord.Session.CommitTransaction();
                                        modify = true;
                                    }
                                }
                                if (modify)
                                    result++;
                            }
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, result + "/" + View.SelectedObjects.Count + "được xuất", "Kết quả");
                        };
                    };
                    dc.SaveOnAccept = false;
                    dc.CancelAction.Active.SetItemValue("", false);
                    showViewParameters.CreatedView = listview;
                    Application.ShowViewStrategy.ShowView(showViewParameters,
                        new ShowViewSource(Frame, dc.AcceptAction));
                }
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Word") || e.SelectedChoiceActionItem.Id.Equals("DeleteWord"))
            {

                if (video.LanguageOrigin is null)
                {
                    Tools.ShowMessage(Application, "Lỗi", "Ngữ gốc không được trống", InformationType.Error);
                    return;
                }
                if (View.SelectedObjects.Count > 0)
                {
                    int result = 0;
                    var otherObjectSpace = Application.CreateObjectSpace();
                    var dictionary = video.GetDictionary();
                    if (e.SelectedChoiceActionItem.Id.Equals("Word"))
                    {
                        //092 -ExportTerm > Word :  đưa Thuật ngữ vào Từ vựng
                        foreach (Term term in View.SelectedObjects)
                        {
                            if (!string.IsNullOrEmpty(term.Name))
                            {
                                var existed = otherObjectSpace.FindObject<Module.BusinessObjects.Word>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ? and Language.Oid =?", term.Name, video.LanguageOrigin.Oid));
                                if (existed is null)
                                {
                                    var newWord = otherObjectSpace.CreateObject<Module.BusinessObjects.Word>();
                                    newWord.Name = term.Name;
                                    newWord.NoSignWord = Module.Helpers.TextHelper.RemoveUnicode(term.Name);
                                    newWord.Language = otherObjectSpace.GetObjectByKey<Module.BusinessObjects.Language>(video.LanguageOrigin.Oid);
                                    newWord.Session.CommitTransaction();
                                    result++;
                                    //Nạp vào đối tượng từ điển trogn Video                                   
                                    var termNameLength = term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                                    var lowerName = term.Name.ToLower();
                                    var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);
                                    if (!dictionary.ContainsKey(termNameLength))
                                    {
                                        dictionary.Add(termNameLength, new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>());
                                    }
                                    if (dictionary[termNameLength].ContainsKey(termNoneUnicode))
                                        dictionary[termNameLength][termNoneUnicode].Add(term.Name);
                                    else
                                        dictionary[termNameLength].Add(termNoneUnicode, new System.Collections.Generic.List<string>() { term.Name });
                                }
                            }
                        }
                        Tools.ShowMessage(Application, "Kết quả", result + " từ vựng được tạo", InformationType.Info);
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("DeleteWord"))
                    {
                        //092 ExportTerm > DeleteWord : Xóa từ vựng > xóa từ vựng đầu tiên trong list hover Trị số                        
                        if (dictionary is null)
                            return;
                        foreach (Term term in View.SelectedObjects)
                        {
                            if (!string.IsNullOrEmpty(term.Name))
                            {
                                var lowerName = term.Name.ToLower();
                                var termNameLength = lowerName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                                var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);

                                if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode) &&
                                    dictionary[termNameLength][termNoneUnicode].Count > 0)
                                {
                                    var wordName = dictionary[termNameLength][termNoneUnicode][0];
                                    //Xóa trong đối tượng Video
                                    if (dictionary[termNameLength][termNoneUnicode].Count == 1)
                                        dictionary[termNameLength].Remove(termNoneUnicode);
                                    else if (dictionary[termNameLength][termNoneUnicode].Contains(wordName))
                                        dictionary[termNameLength][termNoneUnicode].Remove(wordName);
                                    //Xóa trong CSDL
                                    var existed = otherObjectSpace.FindObject<Module.BusinessObjects.Word>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ? and Language.Oid =?", wordName, video.LanguageOrigin.Oid));
                                    if (existed != null)
                                    {
                                        existed.Delete();
                                        existed.Session.CommitTransaction();
                                        result++;
                                        //chỉ xóa từ đầu tiên
                                        break;
                                    }
                                }
                            }
                        }
                        Tools.ShowMessage(Application, "Kết quả", result + " từ vựng được tạo", InformationType.Info);
                    }

                }

            }






            #endregion ExportTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0919            Oid: ca550e2c-2b5d-4991-a073-bb75c1d65eaa
		private void MergeTermAdjacent_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MergeTermAdjacent), "Gộp liền kề");              
      
            #region MergeTermAdjacentImportCode
                                    //Sử dụng thuật vị đầu tiên để tìm từ liền kề, sau gộp là Cập nhật lại thuật vị/Dịch ngữ cảnh lại:  2 từ cũ và 1 từ mới
            var currentObject = View.CurrentObject as Module.BusinessObjects.Term;
            if (currentObject is null) return;
            if (currentObject.Video is null || string.IsNullOrEmpty(currentObject.Name) || currentObject.TermLocationList.Count == 0) return;
            //Kiểm tra xem có tồn tại term không?
            Module.BusinessObjects.Term existedTerm = null;
            var parrentTerms = TermService.GetParrentTerms(currentObject);
            string otherTermText = "";
            string newTermText = "";
            foreach (var termLocation in termService.GetTermLocationsByOrder(currentObject, true))
            {
                if (existedTerm != null)
                    break;
                var element = termLocation.GetAudioFromElement();
                if (element != null && !string.IsNullOrEmpty(element.Content))
                {
                    string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
                    var contents = element.Content.Split(newLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    string content = "";
                    if (termLocation.Sentence > 0 && termLocation.Sentence < contents.Length)
                    {
                        content = contents[termLocation.Sentence.Value - 1];
                    }
                    if (string.IsNullOrEmpty(content))
                        foreach (var tempContent in contents)
                        {
                            var index = Module.Helpers.TextHelper.GetIndexWordInContent(termLocation.Term.Name, tempContent, parrentTerms.ToArray());
                            if (index >= 0)
                            {
                                content = tempContent;
                            }
                        }
                    if (!string.IsNullOrEmpty(content))
                    {
                        var audioContent = content.Trim();
                        var index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Name, audioContent, parrentTerms.ToArray());
                        if (index < 0)
                        {
                            continue;
                            //Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Thuật ngữ này nằm trong thuật ngữ khác, vui lòng cập nhật lại Thuật vị", InformationType.Error);                            
                            //return;
                        }
                        while (string.IsNullOrEmpty(otherTermText) && index >= 0)
                        {
                            if (index >= 0)
                            {
                                //Cố tìm từ đúng
                                if (e.SelectedChoiceActionItem.Id.Contains("Previous"))
                                {
                                    if (index == 0)
                                        index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Name, audioContent.Substring(1));
                                    if (index >= 0)
                                    {
                                        //Bỏ qua dấu cách trước đó
                                        for (int j = index - 1; j >= 0; j--)
                                        {
                                            if (char.IsLetterOrDigit(audioContent[j]) || audioContent[j] == '.' || audioContent[j] == ',')
                                            {
                                                otherTermText = audioContent[j] + otherTermText;
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
                                            newTermText = otherTermText + " " + currentObject.Name;
                                    }
                                }
                                else if (e.SelectedChoiceActionItem.Id.Contains("Next"))
                                {
                                    //Thêm cả khoảng cách đằng sau từ tìm thấy
                                    index += currentObject.Name.Length;
                                    if (index < audioContent.Length - 1)
                                    {
                                        //Bỏ qua dấu cách trước đó
                                        for (int j = index + 1; j < audioContent.Length; j++)
                                        {
                                            if (char.IsLetterOrDigit(audioContent[j]) || audioContent[j] == '.' || audioContent[j] == ',')
                                            {
                                                otherTermText += audioContent[j];
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
                                            newTermText = currentObject.Name + " " + otherTermText;
                                    }

                                }
                                if (string.IsNullOrEmpty(otherTermText) && index > 0)
                                {
                                    //Xử lý tìm từ tiếp theo nếu từ trước đó trống
                                    var otherContent = audioContent.Substring(index).Trim();
                                    index = Module.Helpers.TextHelper.GetIndexWordInContent(currentObject.Name, otherContent);
                                }
                                if (!string.IsNullOrEmpty(otherTermText) && !string.IsNullOrEmpty(newTermText))
                                {
                                    foreach (var term in currentObject.Video.TermList)
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
                        //var contents = audioContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        //for (int i = 0; i < contents.Length; i++)
                        //{
                        //    if (existedTerm != null)
                        //        break;
                        //    2023 - 07 - 22: Chỉ áp dụng cho thuật ngữ đơn
                        //    string word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[i]);
                        //    if (!string.IsNullOrEmpty(word) && word.Equals(currentObject.Name, System.StringComparison.OrdinalIgnoreCase))
                        //    {

                        //        if (e.SelectedChoiceActionItem.Id.Contains("Previous"))
                        //        {
                        //            if (i > 0)
                        //            {
                        //                otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[i - 1]);
                        //                if (!string.IsNullOrEmpty(otherTermText))
                        //                    newTermText = otherTermText + " " + word;
                        //            }
                        //        }
                        //        else if (e.SelectedChoiceActionItem.Id.Contains("Next"))
                        //        {
                        //            if (i < contents.Length - 1)
                        //            {
                        //                otherTermText = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[i + 1]);
                        //                if (!string.IsNullOrEmpty(otherTermText))
                        //                    newTermText = word + " " + otherTermText;
                        //            }
                        //        }
                        //        if (!string.IsNullOrEmpty(otherTermText) && !string.IsNullOrEmpty(newTermText))
                        //        {
                        //            foreach (var term in currentObject.Video.TermList)
                        //            {
                        //                if (newTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                        //                {
                        //                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Thuật ngữ đã tồn tại: " + term.Name, InformationType.Error);
                        //                    return;
                        //                }
                        //                else if (otherTermText.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                        //                {
                        //                    existedTerm = term;
                        //                    //break;
                        //                }
                        //            }
                        //            if (existedTerm != null)
                        //                break;

                        //        }

                        //    }

                        //}
                    }                    
                }
            }
            if(!string.IsNullOrEmpty(newTermText))
            {
                var tempTerm = ObjectSpace.CreateObject<Term>();
                tempTerm.Video = currentObject.Video;
                currentObject.Video.TermList.Add(tempTerm);
                tempTerm.Quantity = null;
                //tempTerm.Position = null;
                tempTerm.Name = newTermText;
                //2023-08-11: Bỏ loại gộp
                //tempTerm.TermType = TermType.MergeTerm;
                tempTerm.TermType = currentObject.TermType;
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
                    int termLocationCount = 0;
                    int susscess = 0;
                    termService.TranslateTerm(tempTerm, ref termLocationCount, ref susscess);
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Tìm thấy " + tempTerm.Quantity + " thuật ngữ: " + newTermText, InformationType.Info,5000);
                    //Giảm trừ số lượng
                    //2023-07-22 Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
                    termService.UpdatePosition(currentObject, false);
                    if (!(currentObject.Quantity > 0))
                        ObjectSpace.Delete(currentObject);
                    else
                        termService.TranslateTerm(currentObject, ref termLocationCount, ref susscess);
                    if (existedTerm != null)
                    {
                        //2023-07-22 Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
                        termService.UpdatePosition(existedTerm, false);
                        if (!(existedTerm.Quantity > 0))
                            ObjectSpace.Delete(existedTerm);
                        else
                            termService.TranslateTerm(existedTerm, ref termLocationCount, ref susscess);
                    }
                }
            }
            else
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy Thuật ngữ liền kề", InformationType.Error, 5000);
            }
            View.CurrentObject = currentObject;





            #endregion MergeTermAdjacentImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0551            Oid: 7a484f6f-69ef-4c5f-a422-8fc674ee2de5
		private void TranslateTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TranslateTerm), "Dịch thuật ngữ");              
      
            #region TranslateTermImportCode
            char charTag = '{';
            decimal countNumber = 0;
            if (e.SelectedChoiceActionItem.Id.Contains("Independent"))
            {
                //Độc lập: dịch google lưu trường Dịch máy                
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (!string.IsNullOrEmpty(term.Name))
                    {
                        var result = Module.SystemObjects.Tools.TranslateText(term.Name);
                        if (!string.IsNullOrEmpty(result))
                        {
                            term.GoogleTranslate = result;
                        }
                    }
                    if (View.SelectedObjects.Count > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / View.SelectedObjects.Count).ToString("p0"), " ");
                    }
                }
                if (View.SelectedObjects.Count > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
            }
            else if (e.SelectedChoiceActionItem.Id.Contains("TranslateTermContextUpcase"))
            {
                //-Ngữ cảnh: Tìm từ chung trong các câu bên Dịch nội dung ứng với Thuật vị,
                //2023-06-29 So sánh dịch theo google translate 2 lần
                int termLocationCount = 0;
                int susscess = 0;
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    term.Flag = false;
                    if (term.TermLocationList.Count == 0)
                        continue;
                    termLocationCount += term.TermLocationList.Count;
                    ////Xóa trắng dịch máy 
                    //term.GoogleTranslate = null;
                    if (!string.IsNullOrEmpty(term.Name) && term.TermLocationList != null && term.TermLocationList.Count >= 1
                        && (string.IsNullOrEmpty(term.Translate) || string.IsNullOrEmpty(term.GoogleTranslate)))
                    {
                        //System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
                        //TermLocation currentTermLocation = null;
                        //string currentTermLocationContent = null;
                        //var result = new System.Collections.Generic.List<string>();
                        System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
                        foreach (var termLocation in term.TermLocationList)
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
                            //    continue;                            //Xóa trắng dịch máy 
                            //termLocation.MachineTranslate = null;
                            //Thay thế từ được dịch bằng từ khác
                            //string replacedText = "AM5KJZL19K";
                            //string newContent = "";
                            //var firstIndex = 0;
                            //var index = audio.Content.IndexOf(term.Name, System.StringComparison.OrdinalIgnoreCase);
                            //while (index >= 0)
                            //{                                
                            //    newContent += audio.Content.Substring(firstIndex, index - firstIndex);
                            //    firstIndex = index + term.Name.Length;

                            //    //var afterCharIndex = firstIndex + term.Name.Length;                                
                            //    bool validate = true;
                            //    if (firstIndex < audio.Content.Length - 1 && !char.IsWhiteSpace(audio.Content[firstIndex])
                            //    && char.IsLetterOrDigit(audio.Content[firstIndex]) && !audio.Content.Substring(firstIndex).StartsWith(", "))
                            //    {
                            //        //var charText = text[firstIndex];
                            //        validate = false;
                            //    }
                            //    else if (!string.IsNullOrEmpty(newContent) && char.IsLetterOrDigit(newContent[newContent.Length - 1]))
                            //    {
                            //        //var charText = text[firstIndex];
                            //        validate = false;
                            //    }                                
                            //    if (validate)
                            //    {
                            //        //Ký tự đặc biệt được thay thế
                            //        newContent += replacedText;
                            //        //newContent += audio.Content.Substring(index, term.Name.Length);                                    
                            //    }
                            //    else
                            //    {
                            //        newContent += audio.Content.Substring(index, term.Name.Length);
                            //    }
                            //    if (firstIndex >= audio.Content.Length)
                            //        break;                                
                            //    index = audio.Content.IndexOf(term.Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);                              
                            //}
                            //newContent += audio.Content.Substring(firstIndex);
                            //var newtranlate = Module.SystemObjects.Tools.TranslateText(newContent);
                            ////So sánh 2 câu dịch                            
                            //var firstText = audio.Subtitle.Split(' ');
                            //var secondText = newtranlate.Split(' ');                            
                            //var difference = firstText.Except(secondText).ToArray();
                            //if(difference.Length == 1)
                            //{
                            //    result.Add(difference[0]);
                            //}
                            //else
                            //{
                            //    for(int j = 0; j < difference.Length - 1; j++)
                            //    {
                            //        //var word = 
                            //    }
                            //    //Xem kết quả có trùng không
                            //}
                            //2023-07-03: từ cần dịch thì viết hoa, các từ khác viết thường                     
                            //string newUpperContent = "";
                            //string newLowerContent = "";
                            //var firstIndex = 0;
                            //var upperContent = audio.Content.ToLower();
                            //var lowerContent = audio.Content.ToUpper();
                            //var index = audio.Content.IndexOf(term.Name, System.StringComparison.OrdinalIgnoreCase);
                            //while (index >= 0)
                            //{
                            //    newUpperContent += upperContent.Substring(firstIndex, index - firstIndex);
                            //    newLowerContent += lowerContent.Substring(firstIndex, index - firstIndex);
                            //    firstIndex = index + term.Name.Length;

                            //    //var afterCharIndex = firstIndex + term.Name.Length;                                
                            //    bool validate = true;
                            //    if (firstIndex < upperContent.Length - 1 && !char.IsWhiteSpace(upperContent[firstIndex])
                            //    && char.IsLetterOrDigit(audio.Content[firstIndex]) && !upperContent.Substring(firstIndex).StartsWith(", "))
                            //    {
                            //        //var charText = text[firstIndex];
                            //        validate = false;
                            //    }
                            //    else if (!string.IsNullOrEmpty(upperContent) && char.IsLetterOrDigit(newUpperContent[newUpperContent.Length - 1]))
                            //    {
                            //        //var charText = text[firstIndex];
                            //        validate = false;
                            //    }
                            //    if (validate)
                            //    {
                            //        newUpperContent += upperContent.Substring(index, term.Name.Length).ToUpper();
                            //        newLowerContent += upperContent.Substring(index, term.Name.Length).ToLower();
                            //    }
                            //    else
                            //    {
                            //        newUpperContent += upperContent.Substring(index, term.Name.Length);
                            //        newLowerContent += lowerContent.Substring(index, term.Name.Length);
                            //    }
                            //    if (firstIndex >= upperContent.Length)
                            //        break;
                            //    index = upperContent.IndexOf(term.Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);
                            //}
                            //newUpperContent += upperContent.Substring(firstIndex);
                            //newLowerContent += lowerContent.Substring(firstIndex);                            
                            //var newtranlateLowerContent = Module.SystemObjects.Tools.TranslateText(newLowerContent);
                            //if (string.IsNullOrEmpty(newtranlateLowerContent))
                            //    continue;                                                    
                            //int startIndex = -1;
                            //int endIndex = -1;
                            //string newtranlateContent = "";
                            //if (newtranlateLowerContent.Equals(audio.Subtitle, System.StringComparison.OrdinalIgnoreCase))
                            //{
                            //    //Nếu lower mà dịch giống phụ đề
                            //    for (int i = 0; i < newtranlateLowerContent.Length; i++)
                            //    {
                            //        if (startIndex < 0 && char.IsLower(newtranlateLowerContent[i]))
                            //        {
                            //            startIndex = i;
                            //        }
                            //        if (startIndex >= 0 && !char.IsLower(newtranlateLowerContent[i]) && newtranlateLowerContent[i] != ' ')
                            //        {
                            //            if (i == startIndex + 1)
                            //            {
                            //                //Trường hợp google tự sửa viết hoa đầu dòng
                            //                startIndex = -1;
                            //                continue;
                            //            }
                            //            var endText = newtranlateLowerContent.Substring(0, i);
                            //            endIndex = i;
                            //            break;
                            //        }
                            //    }
                            //    newtranlateContent = newtranlateLowerContent;
                            //}
                            //else
                            //{
                            //    var newtranlateUpperContent = Module.SystemObjects.Tools.TranslateText(newUpperContent);
                            //    //Dịch theo Upper
                            //    int lastedIndex = -1;
                            //    for (int i = 0; i < newtranlateUpperContent.Length; i++)
                            //    {
                            //        if (startIndex < 0 && char.IsUpper(newtranlateUpperContent[i]))
                            //        {
                            //            startIndex = i;
                            //        }
                            //        if (startIndex >= 0 && !char.IsUpper(newtranlateUpperContent[i]) && newtranlateUpperContent[i] != ' ')
                            //        {
                            //            if (i == startIndex + 1)
                            //            {
                            //                //Trường hợp google tự sửa viết hoa đầu dòng
                            //                lastedIndex = startIndex;
                            //                startIndex = -1;
                            //                continue;
                            //            }
                            //            var endText = newtranlateUpperContent.Substring(0, i);
                            //            endIndex = i;
                            //            break;
                            //        }
                            //    }
                            //    if(startIndex < 0 && lastedIndex > 0)
                            //    {
                            //        startIndex = lastedIndex;
                            //    }
                            //    newtranlateContent = newtranlateUpperContent;
                            //}

                            //if (startIndex >= 0 || endIndex > 0)
                            //{
                            //    //Nếu tìm thấy từ viết hoa
                            //    if (startIndex < 0)
                            //        startIndex = 0;
                            //    if (endIndex < 0)
                            //        endIndex = newtranlateContent.Length;
                            //    string newTranlate = newtranlateContent.Substring(startIndex, endIndex - startIndex);
                            //    int newStartIndex = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                            //    if (newStartIndex >= 0)
                            //    {
                            //        newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
                            //    }
                            //    //return newTranlate.Trim();

                            //    //var newTranlate = Module.SystemObjects.Tools.TranslateContext(audio.Content, term.Name, false, audio.Subtitle);
                            //    newTranlate = newTranlate.Trim();
                            //    if (string.IsNullOrEmpty(newTranlate))
                            //    {
                            //        newTranlate = Module.SystemObjects.Tools.TranslateContext(audio.Content, term.Name, true, audio.Subtitle);
                            //    }
                            //    if (string.IsNullOrEmpty(newTranlate))
                            //    {

                            //    }
                            //    else if (string.IsNullOrEmpty(Module.Helpers.TextHelper.ReplaceSpecialCharacters(dictionaryResult.Keys, newTranlate)))
                            //    {
                            //        dictionaryResult.Add(newTranlate, audio.Subtitle.IndexOf(newTranlate) < 0);
                            //    }
                            //}
                            var subtitle = TermLocationService.GetSentenceTextFromContent(termLocation, audio.Subtitle);
                            string newTranlate = null;
                            foreach (var key in dictionaryResult.Keys)
                            {
                                var index = Module.Helpers.TextHelper.GetIndexWordInContent(key, subtitle);
                                if (index >= 0)
                                {
                                    newTranlate = subtitle.Substring(index, key.Length);
                                }
                            }
                            if (string.IsNullOrEmpty(newTranlate))
                            {
                                string newContent = "";
                                var firstIndex = 0;
                                //var content = audio.Content.ToLower();
                                //Hỗ trợ câu
                                var content = TermLocationService.GetSentenceTextFromContent(termLocation, audio.Content);
                                var index = content.IndexOf(term.Name, System.StringComparison.OrdinalIgnoreCase);
                                while (index >= 0)
                                {
                                    newContent += content.Substring(firstIndex, index - firstIndex);
                                    firstIndex = index + term.Name.Length;

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
                                        newContent += Module.Helpers.TextHelper.RemoveAccents(term.Name).ToUpper();
                                    }
                                    else
                                    {
                                        newContent += content.Substring(index, term.Name.Length);
                                    }
                                    if (firstIndex >= content.Length)
                                        break;
                                    index = content.IndexOf(term.Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);
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
                                    int newStartIndex = subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                                    if (newStartIndex < 0)
                                    {
                                        //Từ được dịch không hợp lệ
                                        newTranlate = null;
                                    }
                                    else
                                    {
                                        newTranlate = subtitle.Substring(newStartIndex, newTranlate.Length);
                                    }
                                }

                                if (string.IsNullOrEmpty(newTranlate))
                                {
                                    //Dịch thử thông thường
                                    var gTranslate = Module.SystemObjects.Tools.TranslateText(term.Name);
                                    int newStartIndex = subtitle.IndexOf(gTranslate, System.StringComparison.OrdinalIgnoreCase);
                                    if (newStartIndex >= 0)
                                    {
                                        newTranlate = gTranslate;
                                        newTranlate = subtitle.Substring(newStartIndex, newTranlate.Length);
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
                                if (newTranlate.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                                    newTranlate = term.Name;
                                termLocation.MachineTranslate = newTranlate;
                                //2023--08-14 - Khi dịch máy manual trên Thuật ngữ hoặc Thuật vị sẽ xác định bằng tìm kiếm nếu kết quả thấy 1 thì cập nhật
                                //  , 0 thấy hoặc 2 trở lên thì phải cập nhật Vị trí dịch bằng manual
                                int count = 0;
                                var index = -1;
                                while (true)
                                {
                                    var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(newTranlate, subtitle, null, index + 1);
                                    if (newIndex < 0)
                                        break;
                                    index = newIndex;
                                    count++;
                                }
                                if (count == 1)
                                {
                                    var firstText = subtitle.Substring(0, index);
                                    var rows = firstText.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                                    int position = 0;
                                    for (int m = 0; m < rows.Count(); m++)
                                    {
                                        var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                        position += contents.Length;
                                    }
                                    //Tổng số lượng trong mảng mảng nhỏ hơn 1 so với vị trí thực tế
                                    termLocation.TranslateLocation = position + 1;
                                }
                                // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                                //termLocation.Flag = subtitle.IndexOf(newTranlate) < 0;
                                if (!flag)
                                    flag = subtitle.IndexOf(newTranlate) < 0;
                                var key = Module.Helpers.TextHelper.KeyListContains(dictionaryResult.Keys, newTranlate);
                                if (string.IsNullOrEmpty(key))
                                {
                                    var newTranlates = Module.SystemObjects.Tools.TranslateText(term.Name);
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
                                    flag = subtitle.IndexOf(newTranlate) < 0;
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
                                    term.Flag = true;
                                    if (!string.IsNullOrEmpty(term.Note))
                                    {
                                        //Xóa ghi chú tag trước đó
                                        term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                                    }
                                    term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Không tìm thấy " + e.SelectedChoiceActionItem.Caption);
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
                                term.GoogleTranslate = maxKey;
                            }
                        }
                        else
                        {
                            term.Flag = true;
                            if (!string.IsNullOrEmpty(term.Note))
                            {
                                //Xóa ghi chú tag trước đó
                                term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                            }
                            term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Không tìm thấy " + e.SelectedChoiceActionItem.Caption);
                        }
                    }
                    if (!term.Flag)
                    {
                        var status = term.Session.FindObject<Module.SystemObjects.Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'MachineTranslate'"));
                        if (status != null)
                            term.Status = status;
                    }
                    if (View.SelectedObjects.Count > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / View.SelectedObjects.Count).ToString("p0"), " ");
                    }
                }
                if (View.SelectedObjects.Count > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Nạp " + View.SelectedObjects.Count + " thuật ngữ",
                        "Có " + susscess + "/" + termLocationCount + " thuật vị được nạp", InformationType.Info);

            }
            else if (e.SelectedChoiceActionItem.Id.Contains("TranslateTermContext"))
            {
                //Xác định Dịch máy bằng 3 phương pháp
                int termLocationCount = 0;
                int susscess = 0;
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    // chỉ dịch khi Trạng thái là Soạn hoặc Thuật vị
                    if (term.Status != null && term.Status.Code != "New")
                        continue;
                    foreach (Module.BusinessObjects.TermLocation termLocation in term.TermLocationList)
                    {
                        termLocationService.TranslateTermLocation(termLocation, ref susscess, ref termLocationCount, e.SelectedChoiceActionItem.Id, Application);
                    }
                    termService.UpdateGoogleTranslate(term);
                    //term.TranslateTermContext(ref susscess, ref termLocationCount, Application);
                    if (!term.Flag)
                    {
                        var status = term.Session.FindObject<Module.SystemObjects.Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'MachineTranslate'"));
                        if (status != null)
                            term.Status = status;
                    }
                    if (View.SelectedObjects.Count > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / View.SelectedObjects.Count).ToString("p0"), " ");
                    }
                }
                if (View.SelectedObjects.Count > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Dịch " + View.SelectedObjects.Count + " thuật ngữ",
                        "Có " + susscess + "/" + termLocationCount + " thuật vị được dịch", InformationType.Info);

            }
            else if (e.SelectedChoiceActionItem.Id.Contains("KeepOrigin"))
            {
                //Nguyên gốc: Copy từ nguyên gốc vào trường Dịch của Thuật ngữ từ đó sẽ vào trường Dịch của Thuật vị
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (!string.IsNullOrEmpty(term.Name))
                    {
                        //Chức năng Dịch>Giữ nguyên (keepOrigin) 
                        //2023-08-02 Copy ghi đè Dịch của Thuật ngữ
                        //-Copy vào Dịch(nếu null) của những Thuật vị nào mà Dịch máy khác Gốc
                        //if (string.IsNullOrEmpty(term.Translate))
                        //{
                        term.Translate = term.Name;
                        //}
                        //2023-07-31: Chức năng Dịch>Giữ nguyên (keepOrigin)
                        //- cần ghi đè Dịch của Thuật ngữ chứ không cần null và khác Dịch máy như Thuật vị
                        //-Chỉ ghi lên Dịch của Thuật ngữ khi tồn tại thuật vị mà Dịch máy khác gốc
                        //Cả Giữ nguyên lẫn Đồng bộ đều ghi đè Dịch thuật vị nếu Dịch máy khác Dịch như Dịch trên Thuật vị
                        foreach (var termLocation in term.TermLocationList)
                        {
                            //2023-08-01: cần ghi đè Dịch của Thuật ngữ chứ không cần null và khác Dịch máy như Thuật vị
                            if (!term.Translate.Equals(termLocation.MachineTranslate))
                            {
                                termLocation.Translate = term.Translate;
                            }
                        }
                        //2023-08-02: Chuuyển trạng thái của Thuật ngữ về Dịch
                        term.Status = term.Session.FindObject<Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'Translate'"));
                    }

                    if (View.SelectedObjects.Count > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / View.SelectedObjects.Count).ToString("p0"), " ");
                    }
                }
                if (View.SelectedObjects.Count > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
            }
            else if (e.SelectedChoiceActionItem.Id.Contains("SyncTermTranslate"))
            {
                //SyncTermTranslate: Copy Dịch của Thuật ngữ lên những Dịch (nếu null) tại toàn bộ thuật vị
                foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
                {
                    if (!string.IsNullOrEmpty(term.Translate))
                    {

                        bool clearTranslate = term.TermLocationList.Count > 0;
                        //2023-08-02: Chuuyển trạng thái của Thuật ngữ về Dịch
                        term.Status = term.Session.FindObject<Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'Translate'"));
                        foreach (var termLocation in term.TermLocationList)
                        {
                            //2023-08-01: Cả Giữ nguyên lẫn Đồng bộ đều ghi đè Dịch thuật vị nếu Dịch máy khác Dịch như Dịch trên Thuật vị
                            if (string.IsNullOrEmpty(termLocation.Translate) || !term.Name.Equals(termLocation.MachineTranslate))
                            {
                                termLocation.Translate = term.Translate;
                            }
                            if (!term.Name.Equals(termLocation.MachineTranslate))
                            {
                                clearTranslate = false;
                            }
                        }
                        //2023-08-02:Nếu mọi thuật vị đều có dịch máy giống Dịch thì xóa trắng Dịch của Thuật ngữ
                        if (clearTranslate)
                            term.Translate = null;
                    }
                    if (View.SelectedObjects.Count > 5)
                    {
                        countNumber++;
                        Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm((countNumber / View.SelectedObjects.Count).ToString("p0"), " ");
                    }
                }
                if (View.SelectedObjects.Count > 5)
                    Module.SystemObjects.Tools.ShowOrCloseDefaultWaitForm(null);
            }






            #endregion TranslateTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0552            Oid: 52843e09-ba82-4674-ba73-7d5dc6484e28
		private void ReplaceTranslate_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ReplaceTranslate), "Thay dịch");              
      
            #region ReplaceTranslateImportCode
            char charTag = '{';
            //Tính năng thay từ: tìm Dịch máy trong Dịch nội dung và thay bằng Dịch
            int result = 0;
            //2023-08-01: Thay dịch theo ngữ cảnh trên các thuật vị có dịch khác máy dịch của các thuật ngữ được chọn
            foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
            {
                //Nếu là phi thuật thì bỏ qua không dịch
                //if (term.NoneTerm)
                //    continue;
                //2023-08-02: Chức năng này không chạy do Checkbox Thuật vị > bỏ điều kiện này đi
                if (term.TermLocationList != null && term.TermLocationList.Count > 0)
                {
                    foreach (var termLocation in term.TermLocationList)
                    {
                        if (View.SelectedObjects.Count == 1 && term.TermLocationList.Count == 1)
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
                }
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + " từ được thay thành công");




            #endregion ReplaceTranslateImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0930            Oid: 91427d4b-c2c3-4b86-9ef8-21ac33e63056
		private void UpperLowerTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(UpperLowerTerm), "Chỉnh viết hoa");              
      
            #region UpperLowerTermImportCode
//Xử lý Tên nếu cờ False, xử lý Dịch/ Dịch thuật vị nếu cờ True
            bool nameColumn = true;            
            if (View is ListView && ((ListView)View).Editor != null)
            {
                var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                {
                    if ((string)focusedColumnMemberName == "Name")
                        nameColumn = true;
                    else if ((string)focusedColumnMemberName == "Translate")
                        nameColumn = false;
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn cột Tên hoặc cột Dịch trước khi thực hiện tính năng này", InformationType.Error);
                        return;
                    }
                }
            }
            foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
            {
                //2023-08-10: Bỏ xử lý theo cờ thay bằng vị trị Con trỏ (ô active) ở cột nào thì xử lý cột đó,
                //nếu không rơi vào 2 cột Tên/Dịch thì không xử lý, trường hợp Web không xác định con trỏ thì xử lý Tên
                //nameColumn = !term.Flag;
                if (e.SelectedChoiceActionItem.Id.Equals("Upper"))
                {
                    //Upper: Hoa chữ đầu: upper/UPPER > Upper
                    if (nameColumn)
                    {
                        if (!string.IsNullOrEmpty(term.Name))
                            term.Name = Module.Helpers.TextHelper.UpperFirst(term.Name);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(term.Translate))
                            term.Translate = Module.Helpers.TextHelper.UpperFirst(term.Translate);
                        foreach (var termLocation in term.TermLocationList)
                            if (!string.IsNullOrEmpty(termLocation.Translate))
                                termLocation.Translate = Module.Helpers.TextHelper.UpperFirst(termLocation.Translate);
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Lower"))
                {
                    if (nameColumn)
                    {
                        if (!string.IsNullOrEmpty(term.Name))
                            term.Name = term.Name.ToLower();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(term.Translate))
                            term.Translate = term.Translate.ToLower();
                        foreach (var termLocation in term.TermLocationList)
                            if (!string.IsNullOrEmpty(termLocation.Translate))
                                termLocation.Translate = termLocation.Translate.ToLower();
                    }

                }
                else if (e.SelectedChoiceActionItem.Id.Equals("UpperAll"))
                {
                    if (nameColumn)
                    {
                        if (!string.IsNullOrEmpty(term.Name))
                            term.Name = term.Name.ToUpper();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(term.Translate))
                            term.Translate = term.Translate.ToUpper();
                        foreach (var termLocation in term.TermLocationList)
                            if (!string.IsNullOrEmpty(termLocation.Translate))
                                termLocation.Translate = termLocation.Translate.ToUpper();
                    }
                }                
            }


            #endregion UpperLowerTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0608            Oid: ba856076-86fd-4d2e-b8b2-83a407d17826
		private void LookupWordType_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(LookupWordType), "Từ loại");              
      
            #region LookupWordTypeImportCode
//Chức năng phân loại từ
            //https://flyer.vn/tu-loai-trong-tieng-anh/
            //Chức năng Từ loại:
            //-Hậu tố s, es loại trừ hậu tố ous: nếu tiền từ là Đại từ hoặc viết hoa thì sẽ là Động từ nếu không là Danh từ
            //- Tính từ: be / been / am / 'm/are/'re /is/ 's/was/were -ed
            //- Động từ có thêm nhận dạng tiền từ: will / ll / would / can / could / may / might / shall / should / (+not, n't)
            //Danh từ
            string[] nounList = new string[] { "tion", "sion", "ment", "ce", "er", "or", "ity", "ty", "ship", "ics", "dom", 
                "ture", "ism", "phy", "logy", "cy", "an", "ian",  "ette",  "itude",  "age",  "th",  "ry",  "try",  "hood", "es"};
            //Động từ
            //- Động từ: thêm hậu tố - ing; have / 've/has/had/'d - ed; đại từ/ viết Hoa - ed
            //Động từ: thêm 2 trường hợp -ing và -ed
            string[] verbList = new string[] { "ate", "en", "ify", "ise", "ize", "ing", "ed" };
            string[] equalVerbList = new string[] { "have", "ve", "has", "had", "d" };
            //Tính từ: bỏ trường hợp -ed
            string[] adjectiveList = new string[] { "al", "ful", "less", "ive", "able", "ous", "cult", "ish",
                "ese", "en", "ic", "i", "ian"};
            string[] equalAdjectiveList = new string[] { "be", "been", "am", "'m", "are", "'re", "is", "'s", "was", "were" };
            //Trạng từ
            string[] adverbList = new string[] { "ly", "ward", "wise"};
            //Giới từ
            //string[] prepositionList = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", };
            foreach (Module.BusinessObjects.Term term in View.SelectedObjects)
            {
                if (string.IsNullOrEmpty(term.Name) || term.WordType != WordType.Blank)
                    continue;                
                var name = term.Name.ToLower();
                //Kiểm tra là danh từ
                foreach (var word in nounList)
                {
                    if (name.EndsWith(word))
                    {
                        term.WordType = WordType.Noun; break;
                    }
                }
                if (term.WordType != WordType.Blank)
                    continue;
                //Kiểm tra là động từ
                foreach (var word in verbList)
                {
                    if (name.EndsWith(word))
                    {
                        term.WordType = WordType.Verb; break;
                    }
                }
                if (term.WordType != WordType.Blank)
                    continue;
                foreach (var word in equalVerbList)
                {
                    if (name.Equals(word))
                    {
                        term.WordType = WordType.Verb; break;
                    }
                }
                if (term.WordType != WordType.Blank)
                    continue;
                //Kiểm tra là tính từ
                foreach (var word in adjectiveList)
                {
                    if (name.EndsWith(word))
                    {
                        term.WordType = WordType.Adjective; break;
                    }
                }
                if (term.WordType != WordType.Blank)
                    continue;
                foreach (var word in equalAdjectiveList)
                {
                    if (name.Equals(word))
                    {
                        term.WordType = WordType.Adjective; break;
                    }
                }
                if (term.WordType != WordType.Blank)
                    continue;
                //Kiểm tra là trạng từ
                foreach (var word in adverbList)
                {
                    if (name.EndsWith(word))
                    {
                        term.WordType = WordType.Adverb; break;
                    }
                }                
            }


            #endregion LookupWordTypeImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0940            Oid: afb2f59d-6276-48bd-9681-0c28aee1598e
		private void SynTerm_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SynTerm), "Đồng bộ thuật ngữ");              
      
            #region SynTermImportCode
            //var video = Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            //if (video is null)
            //    return;
            //Thay đổi trên thuật ngữ và trên thành phần ứng với mọi thuật vị,
            //      tự động cập nhật thuật vị trước khi tiến hành, hoa / thường theo ngữ cảnh
            //Xong Dịch máy lại các Thành phần chứa và Dịch máy lại Thuật ngữ
            //Hiện hộp thoại điền giá trị cần thay
            var term = View.CurrentObject as Module.BusinessObjects.Term;
            if (term is null)
                return;
            if (e.SelectedChoiceActionItem.Id.Equals("ModifyTerm"))
            {
                var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
                dc.AcceptAction.Caption = "Thay từ";
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
                        var popupControl = (Module.SystemObjects.ReplaceObject)args.AcceptActionArgs.CurrentObject;
                        if (string.IsNullOrEmpty(popupControl.Find))
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Tìm không được phép trống", InformationType.Error);
                            return;
                        }
                        termService.UpdatePosition(term, true);
                        int total = 0;
                        foreach (Module.BusinessObjects.TermLocation termLocation in term.TermLocationList)
                        {
                            var audio = termLocation.GetAudioFromElement();
                            if (audio is null)
                                continue;
                            bool replaced = false;
                            //Thay thế nội dung
                            if (!string.IsNullOrEmpty(audio.Content))
                            {
                                var result = Module.Helpers.TextHelper.ReplaceWordInContent(audio.Content, popupControl.Find, popupControl.Replace, TermService.GetParrentTerms(term).ToArray());
                                if (!audio.Content.Equals(result))
                                {
                                    replaced = true;
                                    audio.Content = result;
                                    //Xong Dịch máy lại các Thành phần chứa và Dịch máy lại Thuật ngữ
                                    audio.Subtitle = Module.SystemObjects.Tools.TranslateText(audio.Content);
                                }

                            }
                            if (replaced)
                                total++;
                        }
                        //Xong Dịch máy lại các Thành phần chứa và Dịch máy lại Thuật ngữ
                        term.Name = popupControl.Replace;
                        term.GoogleTranslate = Module.SystemObjects.Tools.TranslateText(term.Name);
                        if (total > 0)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", total + " dòng được thay thế từ", InformationType.Info);
                        }
                        else
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không tìm thấy từ cần thay thế", InformationType.Info);
                        }
                    }
                };
                dc.WindowTemplateChanged += delegate (object o, System.EventArgs args)
                {
                    if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                        ((DevExpress.ExpressApp.Controller)o).Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)
                    {
                        ((DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate)((DevExpress.ExpressApp.Controller)o).Frame.Template).IsSearchEnabled = true;
                    }
                };
                showViewParameters.Controllers.Add(dc);
                Module.SystemObjects.ReplaceObject replaceObject = new Module.SystemObjects.ReplaceObject();
                replaceObject.Find = term.Name;
                showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), replaceObject, true);
                showViewParameters.Context = TemplateContext.PopupWindow;

                Application.ShowViewStrategy.ShowView(showViewParameters,
                    new ShowViewSource(Frame, dc.AcceptAction));
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("SynCaseTerm") || e.SelectedChoiceActionItem.Id.Equals("SynCaseTranslate"))
            {
                //Kiểm tra độ dài thuật ngữ: Tên/Dịch máy/Dịch trong Database và giá trị hiện tại, phải bằng nhau thì mới thực hiện Đồng bộ
                var checkObjectSpace = Application.CreateObjectSpace(typeof(Module.BusinessObjects.Term));
                var databaseTerm = checkObjectSpace.GetObjectByKey<Module.BusinessObjects.Term>(term.Oid);
                //Kiểm tra độ dài thuật ngữ: Tên/Dịch máy/Dịch trong Database và giá trị hiện tại, phải bằng nhau thì mới thực hiện Đồng bộ
                if (e.SelectedChoiceActionItem.Id.Equals("SynCaseTerm"))
                {                    
                    if (databaseTerm != null && !string.IsNullOrEmpty(databaseTerm.Name) && !string.IsNullOrEmpty(term.Name)
                        && databaseTerm.Name.Split(' ').Length != term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Số từ của thuật ngữ không được phép thay đổi", InformationType.Error);
                        return;
                    }
                }else
                {                    
                    if (databaseTerm != null && !string.IsNullOrEmpty(databaseTerm.GoogleTranslate) && !string.IsNullOrEmpty(term.GoogleTranslate)
                        && databaseTerm.GoogleTranslate.Split(' ').Length != term.GoogleTranslate.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Số từ của Dịch máy không được phép thay đổi", InformationType.Error);
                        return;
                    }
                    if (databaseTerm != null && !string.IsNullOrEmpty(databaseTerm.Translate) && !string.IsNullOrEmpty(term.Translate)
                        && databaseTerm.Translate.Split(' ').Length != term.Translate.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Số từ của Dịch không được phép thay đổi", InformationType.Error);
                        return;
                    }
                }
                int result = 0;
                foreach (var termLocation in term.TermLocationList)
                {
                    if (termLocationService.SynTerm(termLocation, e.SelectedChoiceActionItem.Id.Equals("SynCaseTerm")))
                        result++;
                }
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + term.TermLocationList.Count + " được đồng bộ");
            }




            #endregion SynTermImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}