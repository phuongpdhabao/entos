using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FixCreateNewObjectViewController : ViewController<ListView>
    {
        private NewObjectViewController newObjectViewController;
        public FixCreateNewObjectViewController()
        {
            TargetViewNesting = Nesting.Nested;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            newObjectViewController = Frame.GetController<NewObjectViewController>();
            if (newObjectViewController != null)
            {
                newObjectViewController.ObjectCreating += NewObjectViewControllerOnObjectCreating;
            }
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            if (newObjectViewController != null)
            {
                newObjectViewController.ObjectCreating -= NewObjectViewControllerOnObjectCreating;
            }
            base.OnDeactivated();
        }

        private void NewObjectViewControllerOnObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            if (View != null && View.ObjectSpace != null)
            {
                e.ObjectSpace = View.ObjectSpace;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        

    }
}
