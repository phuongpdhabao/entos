using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;

namespace ENTOS.Module.Controllers 
{
    public partial class IUpperObjectViewController: ViewController
    {      
        
        public IUpperObjectViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ENTOS.Module.BusinessObjects.IUpperObject);    
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

        
		private void UpperObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region UpperObjectImportCode
                        var currentObject = View?.CurrentObject as Module.BusinessObjects.IUpperObject;
            if (currentObject is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không hợp lệ", InformationType.Error);
                return;
            }
            if (currentObject.SystemType is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không tồn tại kiểu dữ liệu", InformationType.Error);
                return;
            }
            var objectSpace = e.SelectedChoiceActionItem.Id.Equals("Open") ? Application.CreateObjectSpace() : View.ObjectSpace;
            var parentObject = objectSpace.GetObjectByKey(currentObject.SystemType, currentObject.ObjectID) as DevExpress.Xpo.PersistentBase;
            if (parentObject is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy đối tượng cấp trên", InformationType.Error);
                return;
            }
            if (e.SelectedChoiceActionItem.Id.Equals("Open"))
            {
                Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, parentObject, objectSpace, true);
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Delete"))
            {
                View.ObjectSpace.Delete(parentObject);
                currentObject.ObjectID = System.Guid.Empty;
                currentObject.SystemType = null;
                var deleteName = DevExpress.ExpressApp.Utils.CaptionHelper.GetDisplayText(parentObject);
                if (string.IsNullOrEmpty(deleteName))
                    deleteName = "1 đối tượng";
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteName + " bị xóa");
            }

            #endregion UpperObjectImportCode
		}
     }   
}