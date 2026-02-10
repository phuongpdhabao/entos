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
    public partial class TermLocationCorrectionViewController: BaseViewController<Module.BusinessObjects.TermLocationCorrection>
    {      
        
        public TermLocationCorrectionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.TermLocationCorrection);    
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


        
        //Code: 1052            Oid: 1bd47bce-a961-42e1-be24-8588c7493ab0
		private void AutoCorrect_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
      
            #region AutoCorrectImportCode
            int result = 0;
            var selectedObjects = View.SelectedObjects.Cast<Module.BusinessObjects.TermLocationCorrection>().ToList();            
            foreach (Module.BusinessObjects.TermLocationCorrection termLocationCorrection in selectedObjects)
            {
                if(termLocationCorrection != null && termLocationCorrection.CorrectionOptionList != null 
                    && termLocationCorrection.CorrectionOptionList.Count == 1 && !string.IsNullOrEmpty(termLocationCorrection.CorrectionOptionList[0].Name))
                {
                    //bool noReplace = false;
                    //bool replace = false;
                    //Module.BusinessObjects.Term existedTerm = null;
                    //if(termLocationCorrection.TermCorrection != null && termLocationCorrection.TermCorrection.Term != null)
                    //{
                    //    foreach (var termLocation in termLocationCorrection.TermCorrection.Term.TermLocationList.ToList())
                    //    {
                    //        if (existedTerm is null)
                    //            existedTerm = termLocationCorrection.TermCorrection.Term.FindTermByName(termLocationCorrection.CorrectionOptionList[0].Name);
                    //        if (termLocation.ReplaceWord(termLocationCorrection.CorrectionOptionList[0].Name))
                    //            replace = true;
                    //        else
                    //            noReplace = true;
                    //    }
                    //    if (replace)
                    //        result++;
                    //    if (!noReplace)
                    //    {
                    //        termLocationCorrection.TermCorrection.Term.Flag = false;
                    //        termLocationCorrection.TermCorrection.Term.Name = termLocationCorrection.CorrectionOptionList[0].Name.ToLower();
                    //    }
                    //}
                    //else 
                    if (termLocationCorrection.TermLocation != null)
                    {
                        bool noReplace = false;
                        bool replace = false;
                        Module.BusinessObjects.Term existedTerm = termLocationCorrection.TermLocation.Term != null ? TermService.FindTermByName(termLocationCorrection.TermLocation.Term, termLocationCorrection.CorrectionOptionList[0].Name) : null;
                        if (TermLocationService.ReplaceWord(termLocationCorrection.TermLocation, termLocationCorrection.CorrectionOptionList[0].Name, false, existedTerm))
                        {
                            replace = true;
                            result++;                            
                        }                            
                        else
                            noReplace = true;
                        if (!noReplace && termLocationCorrection.TermCorrection != null && termLocationCorrection.TermCorrection.Term != null)
                        {
                            termLocationCorrection.TermCorrection.Term.Flag = false;
                            termLocationCorrection.TermCorrection.Term.Name = termLocationCorrection.CorrectionOptionList[0].Name.ToLower();
                        }

                    }

                }
            }
            if (result == 0)
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Không có thuật ngữ nào được sửa", InformationType.Error);
            else
            {
                string message = string.Format("Sửa thành công {0}/{1}", result, View.SelectedObjects.Count);
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", message, InformationType.Info, 5000);
            }





            #endregion AutoCorrectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}