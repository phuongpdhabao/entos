using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;
using System;



namespace ENTOS.Module.Controllers
{
    public partial class AutoSaveChatController : ViewController<ListView>
    {
        public AutoSaveChatController()
        {

            TargetObjectType = typeof(Module.BusinessObjects.ChatSession);
            //TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.CurrentObjectChanged += OnViewCurrentObjectChanged;
        }
        private void OnViewCurrentObjectChanged(object sender, EventArgs e)
        {


            if (View.ObjectSpace.IsModified)
            {
                          }


        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View is ListView)
            {
                // Your logic here
            }
        }

        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= OnViewCurrentObjectChanged;
            base.OnDeactivated();
        }

    }
}
