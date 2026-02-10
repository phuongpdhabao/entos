namespace ENTOS.Module.Controllers
{
    partial class TermLocationViewController
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
			// ReplaceTermLocation
            this.ReplaceTermLocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReplaceTermLocation
            // 
            this.ReplaceTermLocation.Caption = "Thay thế";
            this.ReplaceTermLocation.Category = "Edit";
            this.ReplaceTermLocation.ConfirmationMessage = null;
            this.ReplaceTermLocation.Id = "ReplaceTermLocation";
            this.ReplaceTermLocation.ImageName = "Action_ReplaceTermLocation";
            this.ReplaceTermLocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ReplaceTermLocation.ToolTip = "Thuật ngữ không được phép trống";  
			
			this.ReplaceTermLocation.TargetObjectsCriteria = "Term is not null";  
            this.ReplaceTermLocation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ReplaceTermLocation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ReplaceTermLocation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ReplaceTermLocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReplaceTermLocation_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.ReplaceTermLocation);
			// EditWordLocation
            this.EditWordLocation = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordLocationDeleteBefore = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordLocationDeleteAfter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordLocationInsertBefore = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordLocationInsertAfter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // EditWordLocation
            // 
            this.EditWordLocation.Caption = "Soạn thảo";
            this.EditWordLocation.Category = "Edit";
            this.EditWordLocation.ConfirmationMessage = null;
            this.EditWordLocation.Id = "EditWordLocation";
            this.EditWordLocation.ImageName = "Action_EditWordLocation";
            this.EditWordLocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.EditWordLocation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.EditWordLocation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.EditWordLocation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.EditWordLocation.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemEditWordLocationInsertBefore.Caption = "Chèn trước";
            choiceActionItemEditWordLocationInsertBefore.Data = "InsertBefore";
            choiceActionItemEditWordLocationInsertBefore.Id = "InsertBefore";
            this.EditWordLocation.Items.Add(choiceActionItemEditWordLocationInsertBefore);

            
            //
            //Root Choice
            choiceActionItemEditWordLocationInsertAfter.Caption = "Chèn sau";
            choiceActionItemEditWordLocationInsertAfter.Data = "InsertAfter";
            choiceActionItemEditWordLocationInsertAfter.Id = "InsertAfter";
            this.EditWordLocation.Items.Add(choiceActionItemEditWordLocationInsertAfter);

            
            //
            //Root Choice
            choiceActionItemEditWordLocationDeleteBefore.Caption = "Xóa trước";
            choiceActionItemEditWordLocationDeleteBefore.Data = "DeleteBefore";
            choiceActionItemEditWordLocationDeleteBefore.Id = "DeleteBefore";
            this.EditWordLocation.Items.Add(choiceActionItemEditWordLocationDeleteBefore);

            
            //
            //Root Choice
            choiceActionItemEditWordLocationDeleteAfter.Caption = "Xóa sau";
            choiceActionItemEditWordLocationDeleteAfter.Data = "DeleteAfter";
            choiceActionItemEditWordLocationDeleteAfter.Id = "DeleteAfter";
            this.EditWordLocation.Items.Add(choiceActionItemEditWordLocationDeleteAfter);

            this.EditWordLocation.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.EditWordLocation_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.EditWordLocation);
			// ReplaceTranslateLocation
            this.ReplaceTranslateLocation = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemReplaceTranslateLocationUnReplace = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemReplaceTranslateLocationReplace = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ReplaceTranslateLocation
            // 
            this.ReplaceTranslateLocation.Caption = "Thay dịch";
            this.ReplaceTranslateLocation.Category = "Edit";
            this.ReplaceTranslateLocation.ConfirmationMessage = null;
            this.ReplaceTranslateLocation.Id = "ReplaceTranslateLocation";
            this.ReplaceTranslateLocation.ImageName = "Action_ReplaceTranslateLocation";
            this.ReplaceTranslateLocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ReplaceTranslateLocation.ToolTip = "Ngăn Note dùng ký tự {}";  
            this.ReplaceTranslateLocation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ReplaceTranslateLocation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ReplaceTranslateLocation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ReplaceTranslateLocation.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemReplaceTranslateLocationReplace.Caption = "Thay thế";
            choiceActionItemReplaceTranslateLocationReplace.Data = "Replace";
            choiceActionItemReplaceTranslateLocationReplace.Id = "Replace";
            this.ReplaceTranslateLocation.Items.Add(choiceActionItemReplaceTranslateLocationReplace);

            
            //
            //Root Choice
            choiceActionItemReplaceTranslateLocationUnReplace.Caption = "Trả lại";
            choiceActionItemReplaceTranslateLocationUnReplace.Data = "UnReplace";
            choiceActionItemReplaceTranslateLocationUnReplace.Id = "UnReplace";
            this.ReplaceTranslateLocation.Items.Add(choiceActionItemReplaceTranslateLocationUnReplace);

            this.ReplaceTranslateLocation.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ReplaceTranslateLocation_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.ReplaceTranslateLocation);
			// MoveForward
            this.MoveForward = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // MoveForward
            // 
            this.MoveForward.Caption = "Dịch tiến";
            this.MoveForward.Category = "Edit";
            this.MoveForward.ConfirmationMessage = null;
            this.MoveForward.Id = "MoveForward";
            this.MoveForward.ImageName = "Action_MoveForward";
            this.MoveForward.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MoveForward.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MoveForward.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MoveForward.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.MoveForward.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MoveForward_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.MoveForward);
			// MoveBackward
            this.MoveBackward = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // MoveBackward
            // 
            this.MoveBackward.Caption = "Dịch lùi";
            this.MoveBackward.Category = "Edit";
            this.MoveBackward.ConfirmationMessage = null;
            this.MoveBackward.Id = "MoveBackward";
            this.MoveBackward.ImageName = "Action_MoveBackward";
            this.MoveBackward.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MoveBackward.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MoveBackward.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MoveBackward.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.MoveBackward.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MoveBackward_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.MoveBackward);
			// OverlapTermPosition
            this.OverlapTermPosition = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionOneLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionTwoRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionThreeRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionTwoLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionThreeLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermPositionOneRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // OverlapTermPosition
            // 
            this.OverlapTermPosition.Caption = "Thuật ngữ đè";
            this.OverlapTermPosition.Category = "Edit";
            this.OverlapTermPosition.ConfirmationMessage = null;
            this.OverlapTermPosition.Id = "OverlapTermPosition";
            this.OverlapTermPosition.ImageName = "Action_OverlapTermPosition";
            this.OverlapTermPosition.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OverlapTermPosition.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OverlapTermPosition.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OverlapTermPosition.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.OverlapTermPosition.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionOneLeft.Caption = "Một trái";
            choiceActionItemOverlapTermPositionOneLeft.Data = "OneLeft";
            choiceActionItemOverlapTermPositionOneLeft.Id = "OneLeft";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionOneLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionOneRight.Caption = "Một phải";
            choiceActionItemOverlapTermPositionOneRight.Data = "OneRight";
            choiceActionItemOverlapTermPositionOneRight.Id = "OneRight";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionOneRight);

            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionTwoLeft.Caption = "Hai trái";
            choiceActionItemOverlapTermPositionTwoLeft.Data = "TwoLeft";
            choiceActionItemOverlapTermPositionTwoLeft.Id = "TwoLeft";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionTwoLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionTwoRight.Caption = "Hai phải";
            choiceActionItemOverlapTermPositionTwoRight.Data = "TwoRight";
            choiceActionItemOverlapTermPositionTwoRight.Id = "TwoRight";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionTwoRight);

            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionThreeLeft.Caption = "Ba trái";
            choiceActionItemOverlapTermPositionThreeLeft.Data = "ThreeLeft";
            choiceActionItemOverlapTermPositionThreeLeft.Id = "ThreeLeft";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionThreeLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermPositionThreeRight.Caption = "Ba phải";
            choiceActionItemOverlapTermPositionThreeRight.Data = "ThreeRight";
            choiceActionItemOverlapTermPositionThreeRight.Id = "ThreeRight";
            this.OverlapTermPosition.Items.Add(choiceActionItemOverlapTermPositionThreeRight);

            this.OverlapTermPosition.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.OverlapTermPosition_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.OverlapTermPosition);
			// TranslateLocationTerm
            this.TranslateLocationTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateLocationTermKeepOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateLocationTermSyncTermTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateLocationTermTranslateTermContextApostrophe = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateLocationTermTranslateTermContextStrong = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TranslateLocationTerm
            // 
            this.TranslateLocationTerm.Caption = "Dịch thuật vị";
            this.TranslateLocationTerm.Category = "Edit";
            this.TranslateLocationTerm.ConfirmationMessage = null;
            this.TranslateLocationTerm.Id = "TranslateLocationTerm";
            this.TranslateLocationTerm.ImageName = "Action_TranslateLocationTerm";
            this.TranslateLocationTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.TranslateLocationTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TranslateLocationTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TranslateLocationTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TranslateLocationTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTranslateLocationTermTranslateTermContextApostrophe.Caption = "Máy dịch Nháy đơn";
            choiceActionItemTranslateLocationTermTranslateTermContextApostrophe.Data = "TranslateTermContextApostrophe";
            choiceActionItemTranslateLocationTermTranslateTermContextApostrophe.Id = "TranslateTermContextApostrophe";
            this.TranslateLocationTerm.Items.Add(choiceActionItemTranslateLocationTermTranslateTermContextApostrophe);

            
            //
            //Root Choice
            choiceActionItemTranslateLocationTermTranslateTermContextStrong.Caption = "Máy dịch Strong";
            choiceActionItemTranslateLocationTermTranslateTermContextStrong.Data = "TranslateTermContextStrong";
            choiceActionItemTranslateLocationTermTranslateTermContextStrong.Id = "TranslateTermContextStrong";
            this.TranslateLocationTerm.Items.Add(choiceActionItemTranslateLocationTermTranslateTermContextStrong);

            
            //
            //Root Choice
            choiceActionItemTranslateLocationTermKeepOrigin.Caption = "Giữ nguyên";
            choiceActionItemTranslateLocationTermKeepOrigin.Data = "KeepOrigin";
            choiceActionItemTranslateLocationTermKeepOrigin.Id = "KeepOrigin";
            this.TranslateLocationTerm.Items.Add(choiceActionItemTranslateLocationTermKeepOrigin);

            
            //
            //Root Choice
            choiceActionItemTranslateLocationTermSyncTermTranslate.Caption = "Đồng bộ từ thuật ngữ";
            choiceActionItemTranslateLocationTermSyncTermTranslate.Data = "SyncTermTranslate";
            choiceActionItemTranslateLocationTermSyncTermTranslate.Id = "SyncTermTranslate";
            this.TranslateLocationTerm.Items.Add(choiceActionItemTranslateLocationTermSyncTermTranslate);

            this.TranslateLocationTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TranslateLocationTerm_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.TranslateLocationTerm);
			// SyncTermLocation
            this.SyncTermLocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncTermLocation
            // 
            this.SyncTermLocation.Caption = "Đồng bộ";
            this.SyncTermLocation.Category = "Edit";
            this.SyncTermLocation.ConfirmationMessage = null;
            this.SyncTermLocation.Id = "SyncTermLocation";
            this.SyncTermLocation.ImageName = "Action_SyncTermLocation";
            this.SyncTermLocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SyncTermLocation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SyncTermLocation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.SyncTermLocation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.SyncTermLocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncTermLocation_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.SyncTermLocation);
			// MergeTermAdjacentPosition
            this.MergeTermAdjacentPosition = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTermAdjacentPositionNext = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTermAdjacentPositionPrevious = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MergeTermAdjacentPosition
            // 
            this.MergeTermAdjacentPosition.Caption = "Gộp liền kề";
            this.MergeTermAdjacentPosition.Category = "Edit";
            this.MergeTermAdjacentPosition.ConfirmationMessage = null;
            this.MergeTermAdjacentPosition.Id = "MergeTermAdjacentPosition";
            this.MergeTermAdjacentPosition.ImageName = "Action_MergeTermAdjacentPosition";
            this.MergeTermAdjacentPosition.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MergeTermAdjacentPosition.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MergeTermAdjacentPosition.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MergeTermAdjacentPosition.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.MergeTermAdjacentPosition.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMergeTermAdjacentPositionPrevious.Caption = "Liền trước";
            choiceActionItemMergeTermAdjacentPositionPrevious.Data = "Previous";
            choiceActionItemMergeTermAdjacentPositionPrevious.Id = "Previous";
            this.MergeTermAdjacentPosition.Items.Add(choiceActionItemMergeTermAdjacentPositionPrevious);

            
            //
            //Root Choice
            choiceActionItemMergeTermAdjacentPositionNext.Caption = "Liền sau";
            choiceActionItemMergeTermAdjacentPositionNext.Data = "Next";
            choiceActionItemMergeTermAdjacentPositionNext.Id = "Next";
            this.MergeTermAdjacentPosition.Items.Add(choiceActionItemMergeTermAdjacentPositionNext);

            this.MergeTermAdjacentPosition.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MergeTermAdjacentPosition_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.MergeTermAdjacentPosition);
			// OpenTermLocationElement
            this.OpenTermLocationElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OpenTermLocationElement
            // 
            this.OpenTermLocationElement.Caption = "Mở thành phần";
            this.OpenTermLocationElement.Category = "Edit";
            this.OpenTermLocationElement.ConfirmationMessage = null;
            this.OpenTermLocationElement.Id = "OpenTermLocationElement";
            this.OpenTermLocationElement.ImageName = "Action_OpenTermLocationElement";
            this.OpenTermLocationElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OpenTermLocationElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OpenTermLocationElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OpenTermLocationElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.OpenTermLocationElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenTermLocationElement_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.OpenTermLocationElement);
			// SpellingTermLocation
            this.SpellingTermLocation = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermLocationNotTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermLocationConfirmTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermLocationCancelWrongTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermLocationCorrect = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SpellingTermLocation
            // 
            this.SpellingTermLocation.Caption = "Chính tả";
            this.SpellingTermLocation.Category = "Edit";
            this.SpellingTermLocation.ConfirmationMessage = null;
            this.SpellingTermLocation.Id = "SpellingTermLocation";
            this.SpellingTermLocation.ImageName = "Action_SpellingTermLocation";
            this.SpellingTermLocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.SpellingTermLocation.ToolTip = "Thuật ngữ không được phép trống";  
			
			this.SpellingTermLocation.TargetObjectsCriteria = "Term is not null";  
            this.SpellingTermLocation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SpellingTermLocation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.SpellingTermLocation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.SpellingTermLocation.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSpellingTermLocationCorrect.Caption = "Lỗi chính tả";
            choiceActionItemSpellingTermLocationCorrect.Data = "Correct";
            choiceActionItemSpellingTermLocationCorrect.Id = "Correct";
            this.SpellingTermLocation.Items.Add(choiceActionItemSpellingTermLocationCorrect);

            
            //
            //Root Choice
            choiceActionItemSpellingTermLocationConfirmTerm.Caption = "Là thuật ngữ";
            choiceActionItemSpellingTermLocationConfirmTerm.Data = "ConfirmTerm";
            choiceActionItemSpellingTermLocationConfirmTerm.Id = "ConfirmTerm";
            this.SpellingTermLocation.Items.Add(choiceActionItemSpellingTermLocationConfirmTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermLocationNotTerm.Caption = "Là phi thuật";
            choiceActionItemSpellingTermLocationNotTerm.Data = "NotTerm";
            choiceActionItemSpellingTermLocationNotTerm.Id = "NotTerm";
            this.SpellingTermLocation.Items.Add(choiceActionItemSpellingTermLocationNotTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermLocationCancelWrongTerm.Caption = "Loại bên thua";
            choiceActionItemSpellingTermLocationCancelWrongTerm.Data = "CancelWrongTerm";
            choiceActionItemSpellingTermLocationCancelWrongTerm.Id = "CancelWrongTerm";
            this.SpellingTermLocation.Items.Add(choiceActionItemSpellingTermLocationCancelWrongTerm);

            this.SpellingTermLocation.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SpellingTermLocation_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.SpellingTermLocation);
			// TermLocationFlag
            this.TermLocationFlag = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagDuplicatedTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagNotExist = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagNext = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagPrevious = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagOverlapCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermLocationFlagInner = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TermLocationFlag
            // 
            this.TermLocationFlag.Caption = "Cờ thuật vị";
            this.TermLocationFlag.Category = "Edit";
            this.TermLocationFlag.ConfirmationMessage = null;
            this.TermLocationFlag.Id = "TermLocationFlag";
            this.TermLocationFlag.ImageName = "Action_TermLocationFlag";
            this.TermLocationFlag.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TermLocationFlag.ToolTip = "Ngăn Note dùng ký tự <>";  
            this.TermLocationFlag.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TermLocationFlag.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TermLocationFlag.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TermLocationFlag.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTermLocationFlagPrevious.Caption = "Từ kề trước";
            choiceActionItemTermLocationFlagPrevious.Data = "Previous";
            choiceActionItemTermLocationFlagPrevious.Id = "Previous";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagPrevious);

            
            //
            //Root Choice
            choiceActionItemTermLocationFlagNext.Caption = "Từ kề sau";
            choiceActionItemTermLocationFlagNext.Data = "Next";
            choiceActionItemTermLocationFlagNext.Id = "Next";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagNext);

            
            //
            //Root Choice
            choiceActionItemTermLocationFlagInner.Caption = "Từ trong câu";
            choiceActionItemTermLocationFlagInner.Data = "Inner";
            choiceActionItemTermLocationFlagInner.Id = "Inner";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagInner);

            
            //
            //Root Choice
            choiceActionItemTermLocationFlagDuplicatedTranslate.Caption = "Dịch máy lặp";
            choiceActionItemTermLocationFlagDuplicatedTranslate.Data = "DuplicatedTranslate";
            choiceActionItemTermLocationFlagDuplicatedTranslate.Id = "DuplicatedTranslate";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagDuplicatedTranslate);

            
            //
            //Root Choice
            choiceActionItemTermLocationFlagNotExist.Caption = "Không tồn tại";
            choiceActionItemTermLocationFlagNotExist.Data = "NotExist";
            choiceActionItemTermLocationFlagNotExist.Id = "NotExist";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagNotExist);

            
            //
            //Root Choice
            choiceActionItemTermLocationFlagOverlapCheck.Caption = "Đè";
            choiceActionItemTermLocationFlagOverlapCheck.Data = "OverlapCheck";
            choiceActionItemTermLocationFlagOverlapCheck.Id = "OverlapCheck";
            this.TermLocationFlag.Items.Add(choiceActionItemTermLocationFlagOverlapCheck);

            this.TermLocationFlag.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TermLocationFlag_Execute);
            // 
            // TermLocationViewController
            // 
            this.Actions.Add(this.TermLocationFlag);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ReplaceTermLocation;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction EditWordLocation;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ReplaceTranslateLocation;
		private DevExpress.ExpressApp.Actions.SimpleAction MoveForward;
		private DevExpress.ExpressApp.Actions.SimpleAction MoveBackward;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction OverlapTermPosition;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TranslateLocationTerm;
		private DevExpress.ExpressApp.Actions.SimpleAction SyncTermLocation;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MergeTermAdjacentPosition;
		private DevExpress.ExpressApp.Actions.SimpleAction OpenTermLocationElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TermLocationFlag;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SpellingTermLocation;
    }
}