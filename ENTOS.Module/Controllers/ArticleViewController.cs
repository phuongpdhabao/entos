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
    public partial class ArticleViewController: BaseViewController<Module.BusinessObjects.Article>
    {      
        
        public ArticleViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Article);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.ArticleService articleService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             articleService = new Module.Services.ArticleService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3754            Oid: 374f91bc-ef17-40ff-afb4-e8ab4b5d625c
		private void KnowledgeShare_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(KnowledgeShare), "Chia sẻ kiến thức");              
      
            #region KnowledgeShareImportCode
articleService.KnowledgeShare(GetSelectedObjects());
            #endregion KnowledgeShareImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}