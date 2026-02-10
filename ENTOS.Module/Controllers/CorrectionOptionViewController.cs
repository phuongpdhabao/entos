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
    public partial class CorrectionOptionViewController: BaseViewController<Module.BusinessObjects.CorrectionOption>
    {      
        
        public CorrectionOptionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.CorrectionOption);    
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


        
        //Code: 1057            Oid: 5384684a-90f8-4c71-ad11-c0eed326ac61
		private void TermLocationCorrect_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
      
            #region TermLocationCorrectImportCode
            //Chọn sửa
            var correctionOption = View.CurrentObject as Module.BusinessObjects.CorrectionOption;
            if (correctionOption is null || correctionOption.TermLocationCorrection is null || 
                correctionOption.TermLocationCorrection.TermLocation is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không tìm thấy thuật vị", InformationType.Error);
                return;
            }
            Module.BusinessObjects.Term existedTerm = null;
            if (correctionOption.TermLocationCorrection.TermLocation.Term != null)
                existedTerm = TermService.FindTermByName(correctionOption.TermLocationCorrection.TermLocation.Term, correctionOption.Name);
            if(TermLocationService.ReplaceWord(correctionOption.TermLocationCorrection.TermLocation, correctionOption.Name, false, existedTerm))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Sửa thành công");
            }
            else
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không có thuật vị nào được sửa", InformationType.Error);
            }



            #endregion TermLocationCorrectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1448            Oid: d1f1d724-1b47-45ba-985c-a9bc53f86331
		private void TermCorrect_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
      
            #region TermCorrectImportCode
            //Chọn sửa
            var correctionOption = View.CurrentObject as Module.BusinessObjects.CorrectionOption;
            if (correctionOption is null || correctionOption.TermLocationCorrection is null || 
                correctionOption.TermLocationCorrection.TermLocation is null ||
                correctionOption.TermLocationCorrection.TermLocation.Term is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy thuật ngữ", InformationType.Error);
                return;
            }
            var termLocationList = correctionOption.TermLocationCorrection.TermLocation.Term.TermLocationList.ToList();
            int count = 0, total = termLocationList.Count;
            Module.BusinessObjects.Term existedTerm = null;
            foreach (var termLocation in termLocationList)
            {
                if(existedTerm is null)
                    existedTerm = TermService.FindTermByName(correctionOption.TermLocationCorrection.TermLocation.Term, correctionOption.Name);
                if (TermLocationService.ReplaceWord(termLocation,correctionOption.Name, false, existedTerm))
                    count++;
            }
            if (count == termLocationList.Count)
            {
                correctionOption.TermLocationCorrection.TermLocation.Term.Flag = false;
                correctionOption.TermLocationCorrection.TermLocation.Term.Name = correctionOption.Name.ToLower();
            }                        
            if (count == 0)
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không có thuật vị nào được sửa", InformationType.Error);
            else
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả",
                    string.Format("Sửa thành công {0}/{1}", count, total));





            #endregion TermCorrectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1065            Oid: d3e19801-6474-4bdc-8552-9a92ac3a4ac5
		private void DeleteWord_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
      
            #region DeleteWordImportCode
            if(View.SelectedObjects.Count > 0)
            {
                int result = 0;
                var otherObjectSpace = Application.CreateObjectSpace();
                var selectObjects = View.SelectedObjects.Cast<Module.BusinessObjects.CorrectionOption>().ToList();                
                foreach (Module.BusinessObjects.CorrectionOption correctionOption in selectObjects)
                {
                    if (correctionOption.TermLocationCorrection is null || correctionOption.TermLocationCorrection.TermLocation is null)
                        continue;                    
                    if (correctionOption.TermLocationCorrection.TermLocation.Audio != null && correctionOption.TermLocationCorrection.TermLocation.Audio.Video != null)
                    {
                        if (correctionOption.TermLocationCorrection.TermLocation.Audio.Video.LanguageOrigin is null)
                        {
                            Tools.ShowMessage(Application, "Lỗi", "Ngữ gốc không được trống", InformationType.Error);
                            return;
                        }
                        //Xóa trong đối tượng video
                        var lowerName = correctionOption.Name.ToLower();
                        var termNameLength = lowerName.Split(' ').Length;
                        var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);
                        var dictionary = correctionOption.TermLocationCorrection.TermLocation.Audio.Video.GetDictionary();
                        if (dictionary != null && dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode) &&
                            dictionary[termNameLength][termNoneUnicode].Count > 0)
                        {
                            if (dictionary[termNameLength][termNoneUnicode].Count == 1)
                                dictionary[termNameLength].Remove(termNoneUnicode);
                            else if(dictionary[termNameLength][termNoneUnicode].Contains(correctionOption.Name))
                                dictionary[termNameLength][termNoneUnicode].Remove(correctionOption.Name);
                        }
                        //Xóa trong csdl
                        var existed = otherObjectSpace.FindObject<Module.BusinessObjects.Word>
                            (DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ? and Language.Oid =?", 
                            correctionOption.Name, correctionOption.TermLocationCorrection.TermLocation.Audio.Video.LanguageOrigin.Oid));                        
                        if (existed != null)
                        {
                            existed.Delete();
                            existed.Session.CommitTransaction();
                            result++;
                            //chỉ xóa từ đầu tiên
                            correctionOption.TermLocationCorrection.CorrectionOptionList.Remove(correctionOption);
                            break;
                        }
                    }
                    correctionOption.TermLocationCorrection.CorrectionOptionList.Remove(correctionOption);


                }
                Tools.ShowMessage(Application, "Kết quả", result + " từ vựng bị xóa", InformationType.Info);
            }





            #endregion DeleteWordImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}