namespace ENTOS.Module.Controllers
{
    partial class WebsiteViewController
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
			// CreateWebsite
            this.CreateWebsite = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteSetup = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFiles = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteSyncMenuAndHomepage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteDatabase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFolder = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFoldermenu_category = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFoldermenu_page_tabs = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFoldermenu_product_cat = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFoldermenu_page = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFoldercategory = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFolderpage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFolderEdit_menu = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteFolderproduct_cat = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteLogo = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemCreateWebsiteDatabaseFilesSetup = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // CreateWebsite
            // 
            this.CreateWebsite.Caption = "Tạo Website";
            this.CreateWebsite.Category = "Edit";
            this.CreateWebsite.ConfirmationMessage = null;
            this.CreateWebsite.Id = "CreateWebsite";
            this.CreateWebsite.ImageName = "Action_CreateWebsite";
            this.CreateWebsite.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.CreateWebsite.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CreateWebsite.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.CreateWebsite.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.CreateWebsite.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemCreateWebsiteDatabase.Caption = "Cơ sở dữ liệu";
            choiceActionItemCreateWebsiteDatabase.Data = "Database";
            choiceActionItemCreateWebsiteDatabase.Id = "Database";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteDatabase);

            
            //
            //Root Choice
            choiceActionItemCreateWebsiteFiles.Caption = "Tệp tin";
            choiceActionItemCreateWebsiteFiles.Data = "Files";
            choiceActionItemCreateWebsiteFiles.Id = "Files";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteFiles);

            
            //
            //Root Choice
            choiceActionItemCreateWebsiteSetup.Caption = "Cài đặt";
            choiceActionItemCreateWebsiteSetup.Data = "Setup";
            choiceActionItemCreateWebsiteSetup.Id = "Setup";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteSetup);

            
            //
            //Root Choice
            choiceActionItemCreateWebsiteDatabaseFilesSetup.Caption = "Tạo CSDL, tệp và cài đặt";
            choiceActionItemCreateWebsiteDatabaseFilesSetup.Data = "DatabaseFilesSetup";
            choiceActionItemCreateWebsiteDatabaseFilesSetup.Id = "DatabaseFilesSetup";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteDatabaseFilesSetup);

            
            //
            //Root Choice
            choiceActionItemCreateWebsiteLogo.Caption = "Biểu tượng";
            choiceActionItemCreateWebsiteLogo.Data = "Logo";
            choiceActionItemCreateWebsiteLogo.Id = "Logo";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteLogo);

            
            //
            //Root Choice
            choiceActionItemCreateWebsiteFolder.Caption = "Từ thư mục";
            choiceActionItemCreateWebsiteFolder.Data = "Folder";
            choiceActionItemCreateWebsiteFolder.Id = "Folder";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteFolder);

            
            //
            //Choice
            choiceActionItemCreateWebsiteFolderproduct_cat.Caption = "Danh mục SP";
            choiceActionItemCreateWebsiteFolderproduct_cat.Data = "product_cat";
            choiceActionItemCreateWebsiteFolderproduct_cat.Id = "product_cat";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFolderproduct_cat);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFoldermenu_product_cat.Caption = "Danh mục SP và Thực đơn";
            choiceActionItemCreateWebsiteFoldermenu_product_cat.Data = "menu_product_cat";
            choiceActionItemCreateWebsiteFoldermenu_product_cat.Id = "menu_product_cat";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFoldermenu_product_cat);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFoldercategory.Caption = "Chuyên mục TT";
            choiceActionItemCreateWebsiteFoldercategory.Data = "category";
            choiceActionItemCreateWebsiteFoldercategory.Id = "category";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFoldercategory);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFoldermenu_category.Caption = "Chuyên mục TT và Thực đơn";
            choiceActionItemCreateWebsiteFoldermenu_category.Data = "menu_category";
            choiceActionItemCreateWebsiteFoldermenu_category.Id = "menu_category";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFoldermenu_category);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFolderEdit_menu.Caption = "Sửa thực đơn";
            choiceActionItemCreateWebsiteFolderEdit_menu.Data = "Edit_menu";
            choiceActionItemCreateWebsiteFolderEdit_menu.Id = "Edit_menu";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFolderEdit_menu);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFolderpage.Caption = "Trang";
            choiceActionItemCreateWebsiteFolderpage.Data = "page";
            choiceActionItemCreateWebsiteFolderpage.Id = "page";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFolderpage);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFoldermenu_page.Caption = "Trang và Thực đơn";
            choiceActionItemCreateWebsiteFoldermenu_page.Data = "menu_page";
            choiceActionItemCreateWebsiteFoldermenu_page.Id = "menu_page";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFoldermenu_page);
             
            //
            //Choice
            choiceActionItemCreateWebsiteFoldermenu_page_tabs.Caption = "Trang và Thực đơn có tab";
            choiceActionItemCreateWebsiteFoldermenu_page_tabs.Data = "menu_page_tabs";
            choiceActionItemCreateWebsiteFoldermenu_page_tabs.Id = "menu_page_tabs";
            choiceActionItemCreateWebsiteFolder.Items.Add(choiceActionItemCreateWebsiteFoldermenu_page_tabs);
             
            //
            //Root Choice
            choiceActionItemCreateWebsiteSyncMenuAndHomepage.Caption = "Trang chủ";
            choiceActionItemCreateWebsiteSyncMenuAndHomepage.Data = "SyncMenuAndHomepage";
            choiceActionItemCreateWebsiteSyncMenuAndHomepage.Id = "SyncMenuAndHomepage";
            this.CreateWebsite.Items.Add(choiceActionItemCreateWebsiteSyncMenuAndHomepage);

            this.CreateWebsite.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CreateWebsite_Execute);
            // 
            // WebsiteViewController
            // 
            this.Actions.Add(this.CreateWebsite);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction CreateWebsite;
    }
}