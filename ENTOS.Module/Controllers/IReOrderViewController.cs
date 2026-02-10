using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;

namespace ENTOS.Module.Controllers 
{
    public partial class IReOrderViewController: ViewController
    {      
        
        public IReOrderViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ENTOS.Module.BusinessObjects.IReOrder);    
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

        
		private void ReOrder_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region ReOrderImportCode
                        //Chức năng ReOrder/Đánh số lại: trường số TT theo sắp xếp hiện tại, multichoice
            //Hướng dẫn: Trong các đối tượng được chọn, đánh số lại trường Order từ 1 tăng dần theo sắp xếp hiện tại, nếu vi phạm quy định duy nhất thì báo lỗi
            //Tạo 1 list khác từ list hiện tại để tránh thay đổi
            var reOrders = new System.Collections.Generic.List<Module.BusinessObjects.IReOrder>();
            foreach (Module.BusinessObjects.IReOrder reOrder in View.SelectedObjects)
                reOrders.Add(reOrder);
            for (int i = 0; i < reOrders.Count; i++)
            {
                var newOrder = i + 1;
                reOrders[i].Order = newOrder;
            }
            //Bỏ quy định duy nhất của ReOrder
            //for(int i = reOrders.Count -1; i >= 0; i--)
            //{
            //    var newOrder = i + 1;
            //    foreach (Module.BusinessObjects.IReOrder refOrderObject in ((ListView)View).CollectionSource.List)
            //    {
            //        if (!reOrders.Contains(refOrderObject) && newOrder == refOrderObject.Order)
            //        {
            //            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Bị trùng vị trí " + newOrder, InformationType.Error);
            //            return;
            //        }
            //    }
            //    reOrders[i].Order = i + 1;                
            //}

            #endregion ReOrderImportCode
		}
     }   
}