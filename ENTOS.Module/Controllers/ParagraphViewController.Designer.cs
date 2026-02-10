namespace ENTOS.Module.Controllers
{
    partial class ParagraphViewController
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
			// ParagraphFlag
            this.ParagraphFlag = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagBegin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagBeginBeginAbbreviationOrNumber = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagBeginBeginNotUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagBeginBeginSignSpecialCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagBeginBeginSpaces = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEnd = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEndEndAbbreviationOrNumber = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEndEndSignOrSpecialCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEndEndSpaces = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEndEndComma = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemParagraphFlagEndEndNormalCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ParagraphFlag
            // 
            this.ParagraphFlag.Caption = "Cờ đoạn văn bản";
            this.ParagraphFlag.Category = "Edit";
            this.ParagraphFlag.ConfirmationMessage = null;
            this.ParagraphFlag.Id = "ParagraphFlag";
            this.ParagraphFlag.ImageName = "Action_ParagraphFlag";
            this.ParagraphFlag.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ParagraphFlag.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ParagraphFlag.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ParagraphFlag.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ParagraphFlag.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemParagraphFlagBegin.Caption = "Bắt đầu";
            choiceActionItemParagraphFlagBegin.Data = "Begin";
            choiceActionItemParagraphFlagBegin.Id = "Begin";
            this.ParagraphFlag.Items.Add(choiceActionItemParagraphFlagBegin);

            
            //
            //Choice
            choiceActionItemParagraphFlagBeginBeginNotUpperCase.Caption = "Ký tự thường";
            choiceActionItemParagraphFlagBeginBeginNotUpperCase.Data = "BeginNotUpperCase";
            choiceActionItemParagraphFlagBeginBeginNotUpperCase.Id = "BeginNotUpperCase";
            choiceActionItemParagraphFlagBegin.Items.Add(choiceActionItemParagraphFlagBeginBeginNotUpperCase);
             
            //
            //Choice
            choiceActionItemParagraphFlagBeginBeginAbbreviationOrNumber.Caption = "Viết tắt hoặc số";
            choiceActionItemParagraphFlagBeginBeginAbbreviationOrNumber.Data = "BeginAbbreviationOrNumber";
            choiceActionItemParagraphFlagBeginBeginAbbreviationOrNumber.Id = "BeginAbbreviationOrNumber";
            choiceActionItemParagraphFlagBegin.Items.Add(choiceActionItemParagraphFlagBeginBeginAbbreviationOrNumber);
             
            //
            //Choice
            choiceActionItemParagraphFlagBeginBeginSignSpecialCharacter.Caption = "Dấu, ký tự đặc biệt";
            choiceActionItemParagraphFlagBeginBeginSignSpecialCharacter.Data = "BeginSignSpecialCharacter";
            choiceActionItemParagraphFlagBeginBeginSignSpecialCharacter.Id = "BeginSignSpecialCharacter";
            choiceActionItemParagraphFlagBegin.Items.Add(choiceActionItemParagraphFlagBeginBeginSignSpecialCharacter);
             
            //
            //Choice
            choiceActionItemParagraphFlagBeginBeginSpaces.Caption = "Nhiều dấu cách";
            choiceActionItemParagraphFlagBeginBeginSpaces.Data = "BeginSpaces";
            choiceActionItemParagraphFlagBeginBeginSpaces.Id = "BeginSpaces";
            choiceActionItemParagraphFlagBegin.Items.Add(choiceActionItemParagraphFlagBeginBeginSpaces);
             
            //
            //Root Choice
            choiceActionItemParagraphFlagEnd.Caption = "Kết thúc";
            choiceActionItemParagraphFlagEnd.Data = "End";
            choiceActionItemParagraphFlagEnd.Id = "End";
            this.ParagraphFlag.Items.Add(choiceActionItemParagraphFlagEnd);

            
            //
            //Choice
            choiceActionItemParagraphFlagEndEndNormalCharacter.Caption = "Ký tự thường";
            choiceActionItemParagraphFlagEndEndNormalCharacter.Data = "EndNormalCharacter";
            choiceActionItemParagraphFlagEndEndNormalCharacter.Id = "EndNormalCharacter";
            choiceActionItemParagraphFlagEnd.Items.Add(choiceActionItemParagraphFlagEndEndNormalCharacter);
             
            //
            //Choice
            choiceActionItemParagraphFlagEndEndComma.Caption = "Dấu phẩy";
            choiceActionItemParagraphFlagEndEndComma.Data = "EndComma";
            choiceActionItemParagraphFlagEndEndComma.Id = "EndComma";
            choiceActionItemParagraphFlagEnd.Items.Add(choiceActionItemParagraphFlagEndEndComma);
             
            //
            //Choice
            choiceActionItemParagraphFlagEndEndSignOrSpecialCharacter.Caption = "Dấu, ký tự đặc biệt";
            choiceActionItemParagraphFlagEndEndSignOrSpecialCharacter.Data = "EndSignOrSpecialCharacter";
            choiceActionItemParagraphFlagEndEndSignOrSpecialCharacter.Id = "EndSignOrSpecialCharacter";
            choiceActionItemParagraphFlagEnd.Items.Add(choiceActionItemParagraphFlagEndEndSignOrSpecialCharacter);
             
            //
            //Choice
            choiceActionItemParagraphFlagEndEndAbbreviationOrNumber.Caption = "Viết tắt hoặc số";
            choiceActionItemParagraphFlagEndEndAbbreviationOrNumber.Data = "EndAbbreviationOrNumber";
            choiceActionItemParagraphFlagEndEndAbbreviationOrNumber.Id = "EndAbbreviationOrNumber";
            choiceActionItemParagraphFlagEnd.Items.Add(choiceActionItemParagraphFlagEndEndAbbreviationOrNumber);
             
            //
            //Choice
            choiceActionItemParagraphFlagEndEndSpaces.Caption = "Nhiều dấu cách";
            choiceActionItemParagraphFlagEndEndSpaces.Data = "EndSpaces";
            choiceActionItemParagraphFlagEndEndSpaces.Id = "EndSpaces";
            choiceActionItemParagraphFlagEnd.Items.Add(choiceActionItemParagraphFlagEndEndSpaces);
             this.ParagraphFlag.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ParagraphFlag_Execute);
            // 
            // ParagraphViewController
            // 
            this.Actions.Add(this.ParagraphFlag);
			// MergeParagraph
            this.MergeParagraph = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeParagraphMergeDown = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeParagraphMergeUp = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MergeParagraph
            // 
            this.MergeParagraph.Caption = "Gộp";
            this.MergeParagraph.Category = "Edit";
            this.MergeParagraph.ConfirmationMessage = null;
            this.MergeParagraph.Id = "MergeParagraph";
            this.MergeParagraph.ImageName = "Action_MergeParagraph";
            this.MergeParagraph.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.MergeParagraph.TargetObjectsCriteria = "AudioList.Count() > 0";  
            this.MergeParagraph.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.MergeParagraph.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.MergeParagraph.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MergeParagraph.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMergeParagraphMergeUp.Caption = "Gộp trên";
            choiceActionItemMergeParagraphMergeUp.Data = "MergeUp";
            choiceActionItemMergeParagraphMergeUp.Id = "MergeUp";
            this.MergeParagraph.Items.Add(choiceActionItemMergeParagraphMergeUp);

            
            //
            //Root Choice
            choiceActionItemMergeParagraphMergeDown.Caption = "Gộp dưới";
            choiceActionItemMergeParagraphMergeDown.Data = "MergeDown";
            choiceActionItemMergeParagraphMergeDown.Id = "MergeDown";
            this.MergeParagraph.Items.Add(choiceActionItemMergeParagraphMergeDown);

            this.MergeParagraph.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MergeParagraph_Execute);
            // 
            // ParagraphViewController
            // 
            this.Actions.Add(this.MergeParagraph);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ParagraphFlag;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MergeParagraph;
    }
}