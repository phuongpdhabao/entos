namespace ENTOS.Module.Controllers 
{
    partial class IUpperObjectViewController
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
			this.UpperObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectDelete = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReference = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReferencePost = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReferenceFolder = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReferenceOrder = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReferenceWork = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperObjectReferenceLink = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UpperObject
            // 
            this.UpperObject.Caption = "Đối tượng tham chiếu";
            this.UpperObject.Category = "Edit";
            this.UpperObject.ConfirmationMessage = null;
            this.UpperObject.Id = "UpperObject";
            this.UpperObject.ImageName = "Action_UpperObject";
            this.UpperObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.UpperObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UpperObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.UpperObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.UpperObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUpperObjectOpen.Caption = "Mở";
            choiceActionItemUpperObjectOpen.Data = "Open";
            choiceActionItemUpperObjectOpen.Id = "Open";
            this.UpperObject.Items.Add(choiceActionItemUpperObjectOpen);

            
            //
            //Root Choice
            choiceActionItemUpperObjectReference.Caption = "Tham chiếu";
            choiceActionItemUpperObjectReference.Data = "Reference";
            choiceActionItemUpperObjectReference.Id = "Reference";
            this.UpperObject.Items.Add(choiceActionItemUpperObjectReference);

            
            //
            //Choice
            choiceActionItemUpperObjectReferenceWork.Caption = "Công việc";
            choiceActionItemUpperObjectReferenceWork.Data = "Work";
            choiceActionItemUpperObjectReferenceWork.Id = "Work";
            choiceActionItemUpperObjectReference.Items.Add(choiceActionItemUpperObjectReferenceWork);
             
            //
            //Choice
            choiceActionItemUpperObjectReferencePost.Caption = "Bài viết";
            choiceActionItemUpperObjectReferencePost.Data = "Post";
            choiceActionItemUpperObjectReferencePost.Id = "Post";
            choiceActionItemUpperObjectReference.Items.Add(choiceActionItemUpperObjectReferencePost);
             
            //
            //Choice
            choiceActionItemUpperObjectReferenceOrder.Caption = "Đơn hàng";
            choiceActionItemUpperObjectReferenceOrder.Data = "Order";
            choiceActionItemUpperObjectReferenceOrder.Id = "Order";
            choiceActionItemUpperObjectReference.Items.Add(choiceActionItemUpperObjectReferenceOrder);
             
            //
            //Choice
            choiceActionItemUpperObjectReferenceFolder.Caption = "Thư mục";
            choiceActionItemUpperObjectReferenceFolder.Data = "Folder";
            choiceActionItemUpperObjectReferenceFolder.Id = "Folder";
            choiceActionItemUpperObjectReference.Items.Add(choiceActionItemUpperObjectReferenceFolder);
             
            //
            //Choice
            choiceActionItemUpperObjectReferenceLink.Caption = "Liên kết";
            choiceActionItemUpperObjectReferenceLink.Data = "Link";
            choiceActionItemUpperObjectReferenceLink.Id = "Link";
            choiceActionItemUpperObjectReference.Items.Add(choiceActionItemUpperObjectReferenceLink);
             
            //
            //Root Choice
            choiceActionItemUpperObjectDelete.Caption = "Xóa";
            choiceActionItemUpperObjectDelete.Data = "Delete";
            choiceActionItemUpperObjectDelete.Id = "Delete";
            this.UpperObject.Items.Add(choiceActionItemUpperObjectDelete);

            this.UpperObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UpperObject_Execute);
            // 
            // IUpperObjectViewController
            // 
            this.Actions.Add(this.UpperObject);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UpperObject;
    }
}