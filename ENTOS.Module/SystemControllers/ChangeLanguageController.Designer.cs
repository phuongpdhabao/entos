namespace ENTOS.Module.SystemControllers
{
    partial class ChangeLanguageController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
 
            DevExpress.ExpressApp.Actions.ChoiceActionItem defaultChoiceActionItem = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
  
			DevExpress.ExpressApp.Actions.ChoiceActionItem koChoiceActionItem = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
  
			DevExpress.ExpressApp.Actions.ChoiceActionItem jaChoiceActionItem = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
  
			DevExpress.ExpressApp.Actions.ChoiceActionItem enChoiceActionItem = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
 
            this.ActionChooseLanguage = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ActionChooseLanguage
            // 
            this.ActionChooseLanguage.Caption = "Ngôn ngữ";
            this.ActionChooseLanguage.Category = "Menu";
            this.ActionChooseLanguage.ConfirmationMessage = null;
            this.ActionChooseLanguage.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            this.ActionChooseLanguage.Id = "ActionChooseLanguage";
            this.ActionChooseLanguage.ImageMode = DevExpress.ExpressApp.Actions.ImageMode.UseItemImage;
 
            defaultChoiceActionItem.Caption = "Tiếng Việt";
            defaultChoiceActionItem.Data = "vi";
            defaultChoiceActionItem.Id = "vi";
            defaultChoiceActionItem.ImageName = "Language_vi";
			this.ActionChooseLanguage.Items.Add(defaultChoiceActionItem);
  
			koChoiceActionItem.Caption = "Tiếng Hàn";
            koChoiceActionItem.Data = "ko";
            koChoiceActionItem.Id = "ko";
            koChoiceActionItem.ImageName = "Language_ko";
			this.ActionChooseLanguage.Items.Add(koChoiceActionItem);
  
			jaChoiceActionItem.Caption = "Tiếng Nhật";
            jaChoiceActionItem.Data = "ja";
            jaChoiceActionItem.Id = "ja";
            jaChoiceActionItem.ImageName = "Language_ja";
			this.ActionChooseLanguage.Items.Add(jaChoiceActionItem);
  
			enChoiceActionItem.Caption = "Tiếng Anh";
            enChoiceActionItem.Data = "en";
            enChoiceActionItem.Id = "en";
            enChoiceActionItem.ImageName = "Language_en";
			this.ActionChooseLanguage.Items.Add(enChoiceActionItem);
 
            
            this.ActionChooseLanguage.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionChooseLanguage.QuickAccess = true;
            this.ActionChooseLanguage.ShowItemsOnClick = true;
            this.ActionChooseLanguage.ToolTip = null;
            this.ActionChooseLanguage.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.chooseLanguage_Execute);
            // 
            // ChangeLanguageController
            // 
            this.Actions.Add(this.ActionChooseLanguage);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SingleChoiceAction ActionChooseLanguage;
    }
}
