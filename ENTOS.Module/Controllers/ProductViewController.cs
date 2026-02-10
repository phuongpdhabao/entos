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
    public partial class ProductViewController: BaseViewController<Module.BusinessObjects.Product>
    {      
        
        public ProductViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Product);    
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


        
        //Code: 1258            Oid: 7dd08fd7-ad1a-4e18-9d7d-36fd7009989e
		private void ProductVariationImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ProductVariationImport), "Nạp biến thể");              
      
            #region ProductVariationImportImportCode
                                    var product = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Product;
            if (product is null)
                return;
            var productAttributeList = product.ProductAttributeList.Where(m => m.Variation);
            if (productAttributeList.Count() == 0)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng nhập thuộc tính biến thể sản phẩm", InformationType.Error);
                return;
            }
            var variationAttributeList = productAttributeList.OrderBy(m => m.ProductTypeAttribute.Order).Select(x => new System.Collections.Generic.List<Module.BusinessObjects.ProductAttributeValue>(x.ProductAttributeValue.OrderBy(m => m.Order))).ToList();
            var optionsList = Module.SystemObjects.Tools.GenerateAllPermutations(variationAttributeList);
            foreach (var optionsVariation in optionsList)
            {
                var productVariation = new Module.BusinessObjects.Product(product.Session);
                product.ProductList.Add(productVariation);
                productVariation.ProductAttributeValueList.AddRange(optionsVariation);
                productVariation.Name = string.Join(", ", productVariation.ProductAttributeValueList.Where(m => m.ProductTypeAttribute != null).OrderBy(m => m.ProductTypeAttribute.Order).Select(x => x.ProductTypeAttribute.Name + ": " + x.Name));
            }




            #endregion ProductVariationImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        
		private void CheckDomainShare_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CheckDomainShare), "Thị phần");              
      
            #region CheckDomainShareImportCode

            #endregion CheckDomainShareImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}