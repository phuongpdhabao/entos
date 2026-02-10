﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class ChangeLanguageController : WindowController
    {   
        public ChangeLanguageController()
        {
            InitializeComponent();
            this.TargetWindowType = WindowType.Main;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (Application != null && Application.Model != null && !string.IsNullOrEmpty(Application.Model.PreferredLanguage))
            {
                var lastedSelect = ActionChooseLanguage.Items.FindItemByID(Application.Model.PreferredLanguage);
                if (lastedSelect != null)
                {
                    ActionChooseLanguage.SelectedItem = lastedSelect;
                }
            }            
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
   
        private void chooseLanguage_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            Application.SetLanguage(e.SelectedChoiceActionItem.Data as string);            
        }
        
    }
}
