using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;

namespace ENTOS.Module.Controllers 
{
    public partial class IQuantityViewController: ViewController
    {      
        
        public IQuantityViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ENTOS.Module.BusinessObjects.IQuantity);    
            //TargetViewNesting = Nesting.Nested;
        }
		
		protected override void OnActivated()
        {
            base.OnActivated();
            if (View is DetailView)
            {   
                //if (Frame is WinWindow)
                    //((WinWindow) Frame).KeyDown += WindowController1_KeyDown;
                //if (Frame is WebWindow)
                    //((WebWindow) Frame).PagePreRender += CurrentRequestWindow_PagePreRender;           
            }else if (View is ListView){
                //var parent = View.ObjectSpace.Owner as DetailView;
            }
        }
        
        
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             if(View is ListView){
                
             }
        }
        
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        
		private void Quantity_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region QuantityImportCode
                        string column = "";
            if (View is ListView && ((ListView)View).Editor != null)
            {
                var focusedColumnMemberName = ((ListView)View).Editor.GetPropertyValue("FocusedColumnMemberName");
                if (focusedColumnMemberName != null && focusedColumnMemberName is string)
                {
                    column = (string)focusedColumnMemberName;
                }
                else
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Cột được chọn không phải kiểu văn bản", InformationType.Error);
                    return;
                }
            }
            string languageCode = "";              
            foreach (Module.BusinessObjects.IQuantity iQuantity in View.SelectedObjects)
            {
                string text = iQuantity.GetPropertyValue(column) as string;
                if(string.IsNullOrEmpty(text))
                    continue;
                if (e.SelectedChoiceActionItem.Id.Equals("Word"))
                {
                    iQuantity.Quantity = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Character"))
                {
                    iQuantity.Quantity = text.Length;
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Syllable"))
                {
                    //Âm tiết, chỉ có Tiếng Anh mới có âm tiết
                    if (string.IsNullOrEmpty(languageCode))
                    {
                        languageCode = iQuantity.GetPropertyValue("LanguageCode") as string;
                        if (string.IsNullOrEmpty(languageCode))
                        {
                            if (e.SelectedChoiceActionItem.Id.Equals("Syllable"))
                            {
                                using (DevExpress.ExpressApp.SystemModule.DialogController dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                                {
                                    dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                                    {
                                        if (args?.AcceptActionArgs?.CurrentObject is Module.BusinessObjects.Language)
                                            languageCode = ((Module.BusinessObjects.Language)args?.AcceptActionArgs?.CurrentObject).Code;
                                    };
                                    var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Not IsNullOrEmpty(Code)");
                                    Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.Language), View.ObjectSpace, "QuantitySyllable", criteria, false, null, false, true);
                                }
                            }
                        }
                    }
                    
                    iQuantity.Quantity = System.Convert.ToInt32(Module.Helpers.TextHelper.GetWordVowelWeight(languageCode, text));
                }                
            }

            #endregion QuantityImportCode
		}
     }   
}