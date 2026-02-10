namespace ENTOS.Module.Controllers
{
    partial class PostViewController
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
			// RelationMatchingPost
            this.RelationMatchingPost = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostContact = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostContactName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostContactContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostProduct = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostProductContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostProductName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostOrg = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostOrgContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemRelationMatchingPostOrgName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // RelationMatchingPost
            // 
            this.RelationMatchingPost.Caption = "Khớp quan hệ";
            this.RelationMatchingPost.Category = "Edit";
            this.RelationMatchingPost.ConfirmationMessage = null;
            this.RelationMatchingPost.Id = "RelationMatchingPost";
            this.RelationMatchingPost.ImageName = "Action_RelationMatchingPost";
            this.RelationMatchingPost.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.RelationMatchingPost.TargetViewId = "Folder_PostList_ListView";  
            this.RelationMatchingPost.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.RelationMatchingPost.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.RelationMatchingPost.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.RelationMatchingPost.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemRelationMatchingPostProduct.Caption = "Sản phẩm";
            choiceActionItemRelationMatchingPostProduct.Data = "Product";
            choiceActionItemRelationMatchingPostProduct.Id = "Product";
            this.RelationMatchingPost.Items.Add(choiceActionItemRelationMatchingPostProduct);

            
            //
            //Choice
            choiceActionItemRelationMatchingPostProductName.Caption = "Tên";
            choiceActionItemRelationMatchingPostProductName.Data = "Name";
            choiceActionItemRelationMatchingPostProductName.Id = "Name";
            choiceActionItemRelationMatchingPostProduct.Items.Add(choiceActionItemRelationMatchingPostProductName);
             
            //
            //Choice
            choiceActionItemRelationMatchingPostProductContent.Caption = "Nội dung";
            choiceActionItemRelationMatchingPostProductContent.Data = "Content";
            choiceActionItemRelationMatchingPostProductContent.Id = "Content";
            choiceActionItemRelationMatchingPostProduct.Items.Add(choiceActionItemRelationMatchingPostProductContent);
             
            //
            //Root Choice
            choiceActionItemRelationMatchingPostContact.Caption = "Nhân vật";
            choiceActionItemRelationMatchingPostContact.Data = "Contact";
            choiceActionItemRelationMatchingPostContact.Id = "Contact";
            this.RelationMatchingPost.Items.Add(choiceActionItemRelationMatchingPostContact);

            
            //
            //Choice
            choiceActionItemRelationMatchingPostContactName.Caption = "Tên";
            choiceActionItemRelationMatchingPostContactName.Data = "Name";
            choiceActionItemRelationMatchingPostContactName.Id = "Name";
            choiceActionItemRelationMatchingPostContact.Items.Add(choiceActionItemRelationMatchingPostContactName);
             
            //
            //Choice
            choiceActionItemRelationMatchingPostContactContent.Caption = "Nội dung";
            choiceActionItemRelationMatchingPostContactContent.Data = "Content";
            choiceActionItemRelationMatchingPostContactContent.Id = "Content";
            choiceActionItemRelationMatchingPostContact.Items.Add(choiceActionItemRelationMatchingPostContactContent);
             
            //
            //Root Choice
            choiceActionItemRelationMatchingPostOrg.Caption = "Tổ chức";
            choiceActionItemRelationMatchingPostOrg.Data = "Org";
            choiceActionItemRelationMatchingPostOrg.Id = "Org";
            this.RelationMatchingPost.Items.Add(choiceActionItemRelationMatchingPostOrg);

            
            //
            //Choice
            choiceActionItemRelationMatchingPostOrgName.Caption = "Tên";
            choiceActionItemRelationMatchingPostOrgName.Data = "Name";
            choiceActionItemRelationMatchingPostOrgName.Id = "Name";
            choiceActionItemRelationMatchingPostOrg.Items.Add(choiceActionItemRelationMatchingPostOrgName);
             
            //
            //Choice
            choiceActionItemRelationMatchingPostOrgContent.Caption = "Nội dung";
            choiceActionItemRelationMatchingPostOrgContent.Data = "Content";
            choiceActionItemRelationMatchingPostOrgContent.Id = "Content";
            choiceActionItemRelationMatchingPostOrg.Items.Add(choiceActionItemRelationMatchingPostOrgContent);
             this.RelationMatchingPost.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.RelationMatchingPost_Execute);
            // 
            // PostViewController
            // 
            this.Actions.Add(this.RelationMatchingPost);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction RelationMatchingPost;
    }
}