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
    public partial class ProductAttributeViewController: BaseViewController<Module.BusinessObjects.ProductAttribute>
    {      
        
        public ProductAttributeViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ProductAttribute);    
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


        
        //Code: 1257            Oid: 31dbc102-6fd0-4e9a-b51a-28d07ce566c8
		private void ProductAttributeImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ProductAttributeImport), "Nạp thuộc tính");              
      
            #region ProductAttributeImportImportCode
                        var product = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Product;
            if (product is null)
                return;
            if(product.Type is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Sản phẩm chưa chọn loại", InformationType.Error);
                return;
            }
            var oldAttributeCount = product.ProductAttributeList.Count;
            product.SetDefaultProductAttributeList(true);
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", (product.ProductAttributeList.Count - oldAttributeCount) + " thuộc tính được nạp", InformationType.Info);


            #endregion ProductAttributeImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}