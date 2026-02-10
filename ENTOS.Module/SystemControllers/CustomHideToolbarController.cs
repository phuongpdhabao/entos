﻿using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomHideToolbarController : ViewController
    {
        public CustomHideToolbarController()
        {
            //InitializeComponent();
            TargetViewType = ViewType.ListView;
            //TargetObjectType = typeof(ICustomHideToolbar);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Application.CustomizeTemplate += CustomApplication_CustomizeTemplate;
            // Perform various tasks depending on the target View.
        }

        void CustomApplication_CustomizeTemplate(object sender, CustomizeTemplateEventArgs e)
        {
            if (e.Context == TemplateContext.NestedFrame)
            {
                ISupportActionsToolbarVisibility template =
                    e.Template as ISupportActionsToolbarVisibility;
                if (template != null)
                {
                    if (View.ObjectTypeInfo.Type.GetInterfaces().Contains(typeof(ICustomHideToolbar)))
                    {
                        //template.SetVisible(false);
                    }

                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            Application.CustomizeTemplate -= CustomApplication_CustomizeTemplate;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
