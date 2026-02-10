namespace ENTOS.Module.Controllers
{
    partial class TermViewController
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
			// OverlapTerm
            this.OverlapTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermThreeRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermTwoLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermTwoRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermThreeLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermOneRight = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOverlapTermOneLeft = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // OverlapTerm
            // 
            this.OverlapTerm.Caption = "Thuật ngữ đè";
            this.OverlapTerm.Category = "Edit";
            this.OverlapTerm.ConfirmationMessage = null;
            this.OverlapTerm.Id = "OverlapTerm";
            this.OverlapTerm.ImageName = "Action_OverlapTerm";
            this.OverlapTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OverlapTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OverlapTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OverlapTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.OverlapTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemOverlapTermOneLeft.Caption = "Một trái";
            choiceActionItemOverlapTermOneLeft.Data = "OneLeft";
            choiceActionItemOverlapTermOneLeft.Id = "OneLeft";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermOneLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermOneRight.Caption = "Một phải";
            choiceActionItemOverlapTermOneRight.Data = "OneRight";
            choiceActionItemOverlapTermOneRight.Id = "OneRight";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermOneRight);

            
            //
            //Root Choice
            choiceActionItemOverlapTermTwoLeft.Caption = "Hai trái";
            choiceActionItemOverlapTermTwoLeft.Data = "TwoLeft";
            choiceActionItemOverlapTermTwoLeft.Id = "TwoLeft";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermTwoLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermTwoRight.Caption = "Hai phải";
            choiceActionItemOverlapTermTwoRight.Data = "TwoRight";
            choiceActionItemOverlapTermTwoRight.Id = "TwoRight";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermTwoRight);

            
            //
            //Root Choice
            choiceActionItemOverlapTermThreeLeft.Caption = "Ba trái";
            choiceActionItemOverlapTermThreeLeft.Data = "ThreeLeft";
            choiceActionItemOverlapTermThreeLeft.Id = "ThreeLeft";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermThreeLeft);

            
            //
            //Root Choice
            choiceActionItemOverlapTermThreeRight.Caption = "Ba phải";
            choiceActionItemOverlapTermThreeRight.Data = "ThreeRight";
            choiceActionItemOverlapTermThreeRight.Id = "ThreeRight";
            this.OverlapTerm.Items.Add(choiceActionItemOverlapTermThreeRight);

            this.OverlapTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.OverlapTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.OverlapTerm);
			// EditWordTerm
            this.EditWordTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordTermDeleteAfter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordTermInsertBefore = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordTermDeleteBefore = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemEditWordTermInsertAfter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // EditWordTerm
            // 
            this.EditWordTerm.Caption = "Soạn thảo";
            this.EditWordTerm.Category = "Edit";
            this.EditWordTerm.ConfirmationMessage = null;
            this.EditWordTerm.Id = "EditWordTerm";
            this.EditWordTerm.ImageName = "Action_EditWordTerm";
            this.EditWordTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.EditWordTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.EditWordTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.EditWordTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.EditWordTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemEditWordTermInsertBefore.Caption = "Chèn trước";
            choiceActionItemEditWordTermInsertBefore.Data = "InsertBefore";
            choiceActionItemEditWordTermInsertBefore.Id = "InsertBefore";
            this.EditWordTerm.Items.Add(choiceActionItemEditWordTermInsertBefore);

            
            //
            //Root Choice
            choiceActionItemEditWordTermInsertAfter.Caption = "Chèn sau";
            choiceActionItemEditWordTermInsertAfter.Data = "InsertAfter";
            choiceActionItemEditWordTermInsertAfter.Id = "InsertAfter";
            this.EditWordTerm.Items.Add(choiceActionItemEditWordTermInsertAfter);

            
            //
            //Root Choice
            choiceActionItemEditWordTermDeleteBefore.Caption = "Xóa trước";
            choiceActionItemEditWordTermDeleteBefore.Data = "DeleteBefore";
            choiceActionItemEditWordTermDeleteBefore.Id = "DeleteBefore";
            this.EditWordTerm.Items.Add(choiceActionItemEditWordTermDeleteBefore);

            
            //
            //Root Choice
            choiceActionItemEditWordTermDeleteAfter.Caption = "Xóa sau";
            choiceActionItemEditWordTermDeleteAfter.Data = "DeleteAfter";
            choiceActionItemEditWordTermDeleteAfter.Id = "DeleteAfter";
            this.EditWordTerm.Items.Add(choiceActionItemEditWordTermDeleteAfter);

            this.EditWordTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.EditWordTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.EditWordTerm);
			// Dictionary
            this.Dictionary = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDictionaryMatching = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDictionaryContainTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDictionaryWordMatch = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDictionaryTermContain = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // Dictionary
            // 
            this.Dictionary.Caption = "Tra từ điển";
            this.Dictionary.Category = "Edit";
            this.Dictionary.ConfirmationMessage = null;
            this.Dictionary.Id = "Dictionary";
            this.Dictionary.ImageName = "Action_Dictionary";
            this.Dictionary.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.Dictionary.ToolTip = "Ngăn Note dùng ký tự {}";  
            this.Dictionary.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.Dictionary.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.Dictionary.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.Dictionary.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemDictionaryMatching.Caption = "Chính xác";
            choiceActionItemDictionaryMatching.Data = "Matching";
            choiceActionItemDictionaryMatching.Id = "Matching";
            this.Dictionary.Items.Add(choiceActionItemDictionaryMatching);

            
            //
            //Root Choice
            choiceActionItemDictionaryContainTerm.Caption = "Bao thuật ngữ";
            choiceActionItemDictionaryContainTerm.Data = "ContainTerm";
            choiceActionItemDictionaryContainTerm.Id = "ContainTerm";
            this.Dictionary.Items.Add(choiceActionItemDictionaryContainTerm);

            
            //
            //Root Choice
            choiceActionItemDictionaryTermContain.Caption = "Thuật ngữ bao";
            choiceActionItemDictionaryTermContain.Data = "TermContain";
            choiceActionItemDictionaryTermContain.Id = "TermContain";
            this.Dictionary.Items.Add(choiceActionItemDictionaryTermContain);

            
            //
            //Root Choice
            choiceActionItemDictionaryWordMatch.Caption = "Từ vựng giống";
            choiceActionItemDictionaryWordMatch.Data = "WordMatch";
            choiceActionItemDictionaryWordMatch.Id = "WordMatch";
            this.Dictionary.Items.Add(choiceActionItemDictionaryWordMatch);

            this.Dictionary.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Dictionary_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.Dictionary);
			// NumberValue
            this.NumberValue = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // NumberValue
            // 
            this.NumberValue.Caption = "Trị số";
            this.NumberValue.Category = "Edit";
            this.NumberValue.ConfirmationMessage = null;
            this.NumberValue.Id = "NumberValue";
            this.NumberValue.ImageName = "Action_NumberValue";
            this.NumberValue.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.NumberValue.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.NumberValue.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.NumberValue.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.NumberValue.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NumberValue_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.NumberValue);
			// SpellingTerm
            this.SpellingTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermSelectFirstOption = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermStickingSplit = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermStickingSplitStickingSplitTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermStickingSplitStickingSplitOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermAutoCorrect = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCancelWrongTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCorrect = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCountLikeTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCheckCheckTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCheckCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermNotTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermCountLikeWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermConfirmTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingTermFirstLikeTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SpellingTerm
            // 
            this.SpellingTerm.Caption = "Chính tả";
            this.SpellingTerm.Category = "Edit";
            this.SpellingTerm.ConfirmationMessage = null;
            this.SpellingTerm.Id = "SpellingTerm";
            this.SpellingTerm.ImageName = "Action_SpellingTerm";
            this.SpellingTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SpellingTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SpellingTerm.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.SpellingTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.SpellingTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSpellingTermCheck.Caption = "Kiểm tra";
            choiceActionItemSpellingTermCheck.Data = "Check";
            choiceActionItemSpellingTermCheck.Id = "Check";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermCheck);

            
            //
            //Choice
            choiceActionItemSpellingTermCheckCheck.Caption = "Ngữ gốc";
            choiceActionItemSpellingTermCheckCheck.Data = "Check";
            choiceActionItemSpellingTermCheckCheck.Id = "Check";
            choiceActionItemSpellingTermCheck.Items.Add(choiceActionItemSpellingTermCheckCheck);
             
            //
            //Choice
            choiceActionItemSpellingTermCheckCheckTranslate.Caption = "Ngữ dịch";
            choiceActionItemSpellingTermCheckCheckTranslate.Data = "CheckTranslate";
            choiceActionItemSpellingTermCheckCheckTranslate.Id = "CheckTranslate";
            choiceActionItemSpellingTermCheck.Items.Add(choiceActionItemSpellingTermCheckCheckTranslate);
             
            //
            //Root Choice
            choiceActionItemSpellingTermCorrect.Caption = "Lỗi chính tả";
            choiceActionItemSpellingTermCorrect.Data = "Correct";
            choiceActionItemSpellingTermCorrect.Id = "Correct";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermCorrect);

            
            //
            //Root Choice
            choiceActionItemSpellingTermConfirmTerm.Caption = "Là thuật ngữ";
            choiceActionItemSpellingTermConfirmTerm.Data = "ConfirmTerm";
            choiceActionItemSpellingTermConfirmTerm.Id = "ConfirmTerm";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermConfirmTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermNotTerm.Caption = "Là phi thuật";
            choiceActionItemSpellingTermNotTerm.Data = "NotTerm";
            choiceActionItemSpellingTermNotTerm.Id = "NotTerm";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermNotTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermCancelWrongTerm.Caption = "Loại bên thua";
            choiceActionItemSpellingTermCancelWrongTerm.Data = "CancelWrongTerm";
            choiceActionItemSpellingTermCancelWrongTerm.Id = "CancelWrongTerm";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermCancelWrongTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermCountLikeTerm.Caption = "Thuật ngữ đồng dạng";
            choiceActionItemSpellingTermCountLikeTerm.Data = "CountLikeTerm";
            choiceActionItemSpellingTermCountLikeTerm.Id = "CountLikeTerm";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermCountLikeTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermCountLikeWord.Caption = "Từ vựng đồng dạng";
            choiceActionItemSpellingTermCountLikeWord.Data = "CountLikeWord";
            choiceActionItemSpellingTermCountLikeWord.Id = "CountLikeWord";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermCountLikeWord);

            
            //
            //Root Choice
            choiceActionItemSpellingTermFirstLikeTerm.Caption = "Sửa theo TN đồng dạng";
            choiceActionItemSpellingTermFirstLikeTerm.Data = "FirstLikeTerm";
            choiceActionItemSpellingTermFirstLikeTerm.Id = "FirstLikeTerm";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermFirstLikeTerm);

            
            //
            //Root Choice
            choiceActionItemSpellingTermSelectFirstOption.Caption = "Sửa theo TV đồng dạng";
            choiceActionItemSpellingTermSelectFirstOption.Data = "SelectFirstOption";
            choiceActionItemSpellingTermSelectFirstOption.Id = "SelectFirstOption";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermSelectFirstOption);

            
            //
            //Root Choice
            choiceActionItemSpellingTermAutoCorrect.Caption = "Sửa tự động";
            choiceActionItemSpellingTermAutoCorrect.Data = "AutoCorrect";
            choiceActionItemSpellingTermAutoCorrect.Id = "AutoCorrect";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermAutoCorrect);

            
            //
            //Root Choice
            choiceActionItemSpellingTermStickingSplit.Caption = "Tách từ dính";
            choiceActionItemSpellingTermStickingSplit.Data = "StickingSplit";
            choiceActionItemSpellingTermStickingSplit.Id = "StickingSplit";
            this.SpellingTerm.Items.Add(choiceActionItemSpellingTermStickingSplit);

            
            //
            //Choice
            choiceActionItemSpellingTermStickingSplitStickingSplitOrigin.Caption = "Ngữ gốc";
            choiceActionItemSpellingTermStickingSplitStickingSplitOrigin.Data = "StickingSplitOrigin";
            choiceActionItemSpellingTermStickingSplitStickingSplitOrigin.Id = "StickingSplitOrigin";
            choiceActionItemSpellingTermStickingSplit.Items.Add(choiceActionItemSpellingTermStickingSplitStickingSplitOrigin);
             
            //
            //Choice
            choiceActionItemSpellingTermStickingSplitStickingSplitTranslate.Caption = "Ngữ dịch";
            choiceActionItemSpellingTermStickingSplitStickingSplitTranslate.Data = "StickingSplitTranslate";
            choiceActionItemSpellingTermStickingSplitStickingSplitTranslate.Id = "StickingSplitTranslate";
            choiceActionItemSpellingTermStickingSplit.Items.Add(choiceActionItemSpellingTermStickingSplitStickingSplitTranslate);
             this.SpellingTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SpellingTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.SpellingTerm);
			// TermFlag
            this.TermFlag = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagTranslateSameOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagTranslateNotFound = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagUpcaseSecond = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagOverlapCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagClear = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagCopyToFlag2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSuffixES = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagTranslateDifferent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSuffixS = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagTermPositionOverlap = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSuffixED = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagApostrophe = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagStickingWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagStickingWordStickingWordOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagStickingWordStickingWordTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSpellingMistake = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSpellingMistakeSpellingMistakeTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSpellingMistakeSpellingMistake = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagUpperLowerMix = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSuffixER = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSuffixING = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagSameSentenceWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTermFlagUpperCaseTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TermFlag
            // 
            this.TermFlag.Caption = "Cờ thuật ngữ";
            this.TermFlag.Category = "Edit";
            this.TermFlag.ConfirmationMessage = null;
            this.TermFlag.Id = "TermFlag";
            this.TermFlag.ImageName = "Action_TermFlag";
            this.TermFlag.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.TermFlag.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TermFlag.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TermFlag.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TermFlag.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTermFlagApostrophe.Caption = "Dấu nháy đơn";
            choiceActionItemTermFlagApostrophe.Data = "Apostrophe";
            choiceActionItemTermFlagApostrophe.Id = "Apostrophe";
            this.TermFlag.Items.Add(choiceActionItemTermFlagApostrophe);

            
            //
            //Root Choice
            choiceActionItemTermFlagOverlapCheck.Caption = "Đè thuật vị - kiểm tra";
            choiceActionItemTermFlagOverlapCheck.Data = "OverlapCheck";
            choiceActionItemTermFlagOverlapCheck.Id = "OverlapCheck";
            this.TermFlag.Items.Add(choiceActionItemTermFlagOverlapCheck);

            
            //
            //Root Choice
            choiceActionItemTermFlagTermPositionOverlap.Caption = "Đè thuật vị - có cờ";
            choiceActionItemTermFlagTermPositionOverlap.Data = "TermPositionOverlap";
            choiceActionItemTermFlagTermPositionOverlap.Id = "TermPositionOverlap";
            this.TermFlag.Items.Add(choiceActionItemTermFlagTermPositionOverlap);

            
            //
            //Root Choice
            choiceActionItemTermFlagTranslateSameOrigin.Caption = "Dịch giữ nguyên";
            choiceActionItemTermFlagTranslateSameOrigin.Data = "TranslateSameOrigin";
            choiceActionItemTermFlagTranslateSameOrigin.Id = "TranslateSameOrigin";
            this.TermFlag.Items.Add(choiceActionItemTermFlagTranslateSameOrigin);

            
            //
            //Root Choice
            choiceActionItemTermFlagTranslateDifferent.Caption = "Dịch khác nhau";
            choiceActionItemTermFlagTranslateDifferent.Data = "TranslateDifferent";
            choiceActionItemTermFlagTranslateDifferent.Id = "TranslateDifferent";
            this.TermFlag.Items.Add(choiceActionItemTermFlagTranslateDifferent);

            
            //
            //Root Choice
            choiceActionItemTermFlagTranslateNotFound.Caption = "Dịch không thấy";
            choiceActionItemTermFlagTranslateNotFound.Data = "TranslateNotFound";
            choiceActionItemTermFlagTranslateNotFound.Id = "TranslateNotFound";
            this.TermFlag.Items.Add(choiceActionItemTermFlagTranslateNotFound);

            
            //
            //Root Choice
            choiceActionItemTermFlagSuffixED.Caption = "Hậu tố -ed";
            choiceActionItemTermFlagSuffixED.Data = "SuffixED";
            choiceActionItemTermFlagSuffixED.Id = "SuffixED";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSuffixED);

            
            //
            //Root Choice
            choiceActionItemTermFlagSuffixER.Caption = "Hậu tố -er";
            choiceActionItemTermFlagSuffixER.Data = "SuffixER";
            choiceActionItemTermFlagSuffixER.Id = "SuffixER";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSuffixER);

            
            //
            //Root Choice
            choiceActionItemTermFlagSuffixES.Caption = "Hậu tố -es";
            choiceActionItemTermFlagSuffixES.Data = "SuffixES";
            choiceActionItemTermFlagSuffixES.Id = "SuffixES";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSuffixES);

            
            //
            //Root Choice
            choiceActionItemTermFlagSuffixING.Caption = "Hậu tố -ing";
            choiceActionItemTermFlagSuffixING.Data = "SuffixING";
            choiceActionItemTermFlagSuffixING.Id = "SuffixING";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSuffixING);

            
            //
            //Root Choice
            choiceActionItemTermFlagSuffixS.Caption = "Hậu tố -s";
            choiceActionItemTermFlagSuffixS.Data = "SuffixS";
            choiceActionItemTermFlagSuffixS.Id = "SuffixS";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSuffixS);

            
            //
            //Root Choice
            choiceActionItemTermFlagUpperLowerMix.Caption = "Lẫn hoa thường";
            choiceActionItemTermFlagUpperLowerMix.Data = "UpperLowerMix";
            choiceActionItemTermFlagUpperLowerMix.Id = "UpperLowerMix";
            this.TermFlag.Items.Add(choiceActionItemTermFlagUpperLowerMix);

            
            //
            //Root Choice
            choiceActionItemTermFlagSpellingMistake.Caption = "Lỗi chính tả";
            choiceActionItemTermFlagSpellingMistake.Data = "SpellingMistake";
            choiceActionItemTermFlagSpellingMistake.Id = "SpellingMistake";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSpellingMistake);

            
            //
            //Choice
            choiceActionItemTermFlagSpellingMistakeSpellingMistake.Caption = "Ngữ gốc";
            choiceActionItemTermFlagSpellingMistakeSpellingMistake.Data = "SpellingMistake";
            choiceActionItemTermFlagSpellingMistakeSpellingMistake.Id = "SpellingMistake";
            choiceActionItemTermFlagSpellingMistake.Items.Add(choiceActionItemTermFlagSpellingMistakeSpellingMistake);
             
            //
            //Choice
            choiceActionItemTermFlagSpellingMistakeSpellingMistakeTranslate.Caption = "Ngữ dịch";
            choiceActionItemTermFlagSpellingMistakeSpellingMistakeTranslate.Data = "SpellingMistakeTranslate";
            choiceActionItemTermFlagSpellingMistakeSpellingMistakeTranslate.Id = "SpellingMistakeTranslate";
            choiceActionItemTermFlagSpellingMistake.Items.Add(choiceActionItemTermFlagSpellingMistakeSpellingMistakeTranslate);
             
            //
            //Root Choice
            choiceActionItemTermFlagCopyToFlag2.Caption = "Lưu sang cờ 2";
            choiceActionItemTermFlagCopyToFlag2.Data = "CopyToFlag2";
            choiceActionItemTermFlagCopyToFlag2.Id = "CopyToFlag2";
            this.TermFlag.Items.Add(choiceActionItemTermFlagCopyToFlag2);

            
            //
            //Root Choice
            choiceActionItemTermFlagUpperCaseTerm.Caption = "Thuật ngữ viết hoa";
            choiceActionItemTermFlagUpperCaseTerm.Data = "UpperCaseTerm";
            choiceActionItemTermFlagUpperCaseTerm.Id = "UpperCaseTerm";
            this.TermFlag.Items.Add(choiceActionItemTermFlagUpperCaseTerm);

            
            //
            //Root Choice
            choiceActionItemTermFlagSameSentenceWord.Caption = "Từ cùng câu";
            choiceActionItemTermFlagSameSentenceWord.Data = "SameSentenceWord";
            choiceActionItemTermFlagSameSentenceWord.Id = "SameSentenceWord";
            this.TermFlag.Items.Add(choiceActionItemTermFlagSameSentenceWord);

            
            //
            //Root Choice
            choiceActionItemTermFlagUpcaseSecond.Caption = "Từ hoa cận đầu";
            choiceActionItemTermFlagUpcaseSecond.Data = "UpcaseSecond";
            choiceActionItemTermFlagUpcaseSecond.Id = "UpcaseSecond";
            this.TermFlag.Items.Add(choiceActionItemTermFlagUpcaseSecond);

            
            //
            //Root Choice
            choiceActionItemTermFlagClear.Caption = "Xóa cờ";
            choiceActionItemTermFlagClear.Data = "Clear";
            choiceActionItemTermFlagClear.Id = "Clear";
            this.TermFlag.Items.Add(choiceActionItemTermFlagClear);

            
            //
            //Root Choice
            choiceActionItemTermFlagStickingWord.Caption = "Từ dính";
            choiceActionItemTermFlagStickingWord.Data = "StickingWord";
            choiceActionItemTermFlagStickingWord.Id = "StickingWord";
            this.TermFlag.Items.Add(choiceActionItemTermFlagStickingWord);

            
            //
            //Choice
            choiceActionItemTermFlagStickingWordStickingWordOrigin.Caption = "Ngữ gốc";
            choiceActionItemTermFlagStickingWordStickingWordOrigin.Data = "StickingWordOrigin";
            choiceActionItemTermFlagStickingWordStickingWordOrigin.Id = "StickingWordOrigin";
            choiceActionItemTermFlagStickingWord.Items.Add(choiceActionItemTermFlagStickingWordStickingWordOrigin);
             
            //
            //Choice
            choiceActionItemTermFlagStickingWordStickingWordTranslate.Caption = "Ngữ dịch";
            choiceActionItemTermFlagStickingWordStickingWordTranslate.Data = "StickingWordTranslate";
            choiceActionItemTermFlagStickingWordStickingWordTranslate.Id = "StickingWordTranslate";
            choiceActionItemTermFlagStickingWord.Items.Add(choiceActionItemTermFlagStickingWordStickingWordTranslate);
             this.TermFlag.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TermFlag_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.TermFlag);
			// ImportTerm
            this.ImportTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermCompoundWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermDictionary = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateDictionary = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateCompoundWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateNumberCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermTranslateTranslateUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTermNumberCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ImportTerm
            // 
            this.ImportTerm.Caption = "Nạp thuật ngữ";
            this.ImportTerm.Category = "Edit";
            this.ImportTerm.ConfirmationMessage = null;
            this.ImportTerm.Id = "ImportTerm";
            this.ImportTerm.ImageName = "Action_ImportTerm";
            this.ImportTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ImportTerm.ToolTip = "Ngăn Note dùng ký tự []";  
            this.ImportTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportTerm.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ImportTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.ImportTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemImportTermDictionary.Caption = "Từ điển";
            choiceActionItemImportTermDictionary.Data = "Dictionary";
            choiceActionItemImportTermDictionary.Id = "Dictionary";
            this.ImportTerm.Items.Add(choiceActionItemImportTermDictionary);

            
            //
            //Root Choice
            choiceActionItemImportTermCompoundWord.Caption = "Từ ghép";
            choiceActionItemImportTermCompoundWord.Data = "CompoundWord";
            choiceActionItemImportTermCompoundWord.Id = "CompoundWord";
            this.ImportTerm.Items.Add(choiceActionItemImportTermCompoundWord);

            
            //
            //Root Choice
            choiceActionItemImportTermUpperCase.Caption = "Từ hoa";
            choiceActionItemImportTermUpperCase.Data = "UpperCase";
            choiceActionItemImportTermUpperCase.Id = "UpperCase";
            this.ImportTerm.Items.Add(choiceActionItemImportTermUpperCase);

            
            //
            //Root Choice
            choiceActionItemImportTermNumberCharacter.Caption = "Số và ký tự";
            choiceActionItemImportTermNumberCharacter.Data = "NumberCharacter";
            choiceActionItemImportTermNumberCharacter.Id = "NumberCharacter";
            this.ImportTerm.Items.Add(choiceActionItemImportTermNumberCharacter);

            
            //
            //Root Choice
            choiceActionItemImportTermTerm.Caption = "Từ đơn";
            choiceActionItemImportTermTerm.Data = "Term";
            choiceActionItemImportTermTerm.Id = "Term";
            this.ImportTerm.Items.Add(choiceActionItemImportTermTerm);

            
            //
            //Root Choice
            choiceActionItemImportTermAll.Caption = "Nạp tất cả";
            choiceActionItemImportTermAll.Data = "All";
            choiceActionItemImportTermAll.Id = "All";
            this.ImportTerm.Items.Add(choiceActionItemImportTermAll);

            
            //
            //Root Choice
            choiceActionItemImportTermTranslate.Caption = "Ngữ dịch";
            choiceActionItemImportTermTranslate.Data = "Translate";
            choiceActionItemImportTermTranslate.Id = "Translate";
            this.ImportTerm.Items.Add(choiceActionItemImportTermTranslate);

            
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateDictionary.Caption = "Từ điển";
            choiceActionItemImportTermTranslateTranslateDictionary.Data = "TranslateDictionary";
            choiceActionItemImportTermTranslateTranslateDictionary.Id = "TranslateDictionary";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateDictionary);
             
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateCompoundWord.Caption = "Từ ghép";
            choiceActionItemImportTermTranslateTranslateCompoundWord.Data = "TranslateCompoundWord";
            choiceActionItemImportTermTranslateTranslateCompoundWord.Id = "TranslateCompoundWord";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateCompoundWord);
             
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateUpperCase.Caption = "Từ hoa";
            choiceActionItemImportTermTranslateTranslateUpperCase.Data = "TranslateUpperCase";
            choiceActionItemImportTermTranslateTranslateUpperCase.Id = "TranslateUpperCase";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateUpperCase);
             
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateNumberCharacter.Caption = "Số và ký tự";
            choiceActionItemImportTermTranslateTranslateNumberCharacter.Data = "TranslateNumberCharacter";
            choiceActionItemImportTermTranslateTranslateNumberCharacter.Id = "TranslateNumberCharacter";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateNumberCharacter);
             
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateTerm.Caption = "Từ đơn";
            choiceActionItemImportTermTranslateTranslateTerm.Data = "TranslateTerm";
            choiceActionItemImportTermTranslateTranslateTerm.Id = "TranslateTerm";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateTerm);
             
            //
            //Choice
            choiceActionItemImportTermTranslateTranslateAll.Caption = "Nạp tất cả";
            choiceActionItemImportTermTranslateTranslateAll.Data = "TranslateAll";
            choiceActionItemImportTermTranslateTranslateAll.Id = "TranslateAll";
            choiceActionItemImportTermTranslate.Items.Add(choiceActionItemImportTermTranslateTranslateAll);
             this.ImportTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ImportTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.ImportTerm);
			// UpdatePosition
            this.UpdatePosition = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpdatePositionQuantity = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpdatePositionLocation = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpdatePositionOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpdatePositionOverlapCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpdatePositionAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UpdatePosition
            // 
            this.UpdatePosition.Caption = "Thuật vị";
            this.UpdatePosition.Category = "Edit";
            this.UpdatePosition.ConfirmationMessage = null;
            this.UpdatePosition.Id = "UpdatePosition";
            this.UpdatePosition.ImageName = "Action_UpdatePosition";
            this.UpdatePosition.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.UpdatePosition.ToolTip = "Ngăn Note dùng ký tự ()";  
            this.UpdatePosition.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UpdatePosition.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.UpdatePosition.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.UpdatePosition.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUpdatePositionOpen.Caption = "Mở";
            choiceActionItemUpdatePositionOpen.Data = "Open";
            choiceActionItemUpdatePositionOpen.Id = "Open";
            this.UpdatePosition.Items.Add(choiceActionItemUpdatePositionOpen);

            
            //
            //Root Choice
            choiceActionItemUpdatePositionQuantity.Caption = "Cập nhật số lượng";
            choiceActionItemUpdatePositionQuantity.Data = "Quantity";
            choiceActionItemUpdatePositionQuantity.Id = "Quantity";
            this.UpdatePosition.Items.Add(choiceActionItemUpdatePositionQuantity);

            
            //
            //Root Choice
            choiceActionItemUpdatePositionLocation.Caption = "Cập nhật vị trí";
            choiceActionItemUpdatePositionLocation.Data = "Location";
            choiceActionItemUpdatePositionLocation.Id = "Location";
            this.UpdatePosition.Items.Add(choiceActionItemUpdatePositionLocation);

            
            //
            //Root Choice
            choiceActionItemUpdatePositionAll.Caption = "Cập nhật toàn bộ";
            choiceActionItemUpdatePositionAll.Data = "All";
            choiceActionItemUpdatePositionAll.Id = "All";
            this.UpdatePosition.Items.Add(choiceActionItemUpdatePositionAll);

            
            //
            //Root Choice
            choiceActionItemUpdatePositionOverlapCheck.Caption = "Kiểm tra đè";
            choiceActionItemUpdatePositionOverlapCheck.Data = "OverlapCheck";
            choiceActionItemUpdatePositionOverlapCheck.Id = "OverlapCheck";
            this.UpdatePosition.Items.Add(choiceActionItemUpdatePositionOverlapCheck);

            this.UpdatePosition.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UpdatePosition_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.UpdatePosition);
			// OpenTermElement
            this.OpenTermElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OpenTermElement
            // 
            this.OpenTermElement.Caption = "Mở thành phần";
            this.OpenTermElement.Category = "Edit";
            this.OpenTermElement.ConfirmationMessage = null;
            this.OpenTermElement.Id = "OpenTermElement";
            this.OpenTermElement.ImageName = "Action_OpenTermElement";
            this.OpenTermElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OpenTermElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OpenTermElement.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.OpenTermElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.OpenTermElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenTermElement_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.OpenTermElement);
			// SplitTerm
            this.SplitTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitTermFirst = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitTermLast = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitTermTwoFirst = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SplitTerm
            // 
            this.SplitTerm.Caption = "Tách thuật ngữ";
            this.SplitTerm.Category = "Edit";
            this.SplitTerm.ConfirmationMessage = null;
            this.SplitTerm.Id = "SplitTerm";
            this.SplitTerm.ImageName = "Action_SplitTerm";
            this.SplitTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SplitTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SplitTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.SplitTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.SplitTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSplitTermFirst.Caption = "Từ đầu";
            choiceActionItemSplitTermFirst.Data = "First";
            choiceActionItemSplitTermFirst.Id = "First";
            this.SplitTerm.Items.Add(choiceActionItemSplitTermFirst);

            
            //
            //Root Choice
            choiceActionItemSplitTermLast.Caption = "Từ cuối";
            choiceActionItemSplitTermLast.Data = "Last";
            choiceActionItemSplitTermLast.Id = "Last";
            this.SplitTerm.Items.Add(choiceActionItemSplitTermLast);

            
            //
            //Root Choice
            choiceActionItemSplitTermTwoFirst.Caption = "Hai từ đầu";
            choiceActionItemSplitTermTwoFirst.Data = "TwoFirst";
            choiceActionItemSplitTermTwoFirst.Id = "TwoFirst";
            this.SplitTerm.Items.Add(choiceActionItemSplitTermTwoFirst);

            this.SplitTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SplitTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.SplitTerm);
			// ExportTerm
            this.ExportTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportTermNonTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportTermWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportTermDeleteWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportTermDictionary = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ExportTerm
            // 
            this.ExportTerm.Caption = "Xuất thuật ngữ";
            this.ExportTerm.Category = "Edit";
            this.ExportTerm.ConfirmationMessage = null;
            this.ExportTerm.Id = "ExportTerm";
            this.ExportTerm.ImageName = "Action_ExportTerm";
            this.ExportTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ExportTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExportTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExportTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ExportTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemExportTermNonTerm.Caption = "Phi thuật";
            choiceActionItemExportTermNonTerm.Data = "NonTerm";
            choiceActionItemExportTermNonTerm.Id = "NonTerm";
            this.ExportTerm.Items.Add(choiceActionItemExportTermNonTerm);

            
            //
            //Root Choice
            choiceActionItemExportTermDictionary.Caption = "Từ điển";
            choiceActionItemExportTermDictionary.Data = "Dictionary";
            choiceActionItemExportTermDictionary.Id = "Dictionary";
            this.ExportTerm.Items.Add(choiceActionItemExportTermDictionary);

            
            //
            //Root Choice
            choiceActionItemExportTermWord.Caption = "Từ vựng";
            choiceActionItemExportTermWord.Data = "Word";
            choiceActionItemExportTermWord.Id = "Word";
            this.ExportTerm.Items.Add(choiceActionItemExportTermWord);

            
            //
            //Root Choice
            choiceActionItemExportTermDeleteWord.Caption = "Xóa từ vựng";
            choiceActionItemExportTermDeleteWord.Data = "DeleteWord";
            choiceActionItemExportTermDeleteWord.Id = "DeleteWord";
            this.ExportTerm.Items.Add(choiceActionItemExportTermDeleteWord);

            this.ExportTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ExportTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.ExportTerm);
			// MergeTermAdjacent
            this.MergeTermAdjacent = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTermAdjacentNext = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTermAdjacentPrevious = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MergeTermAdjacent
            // 
            this.MergeTermAdjacent.Caption = "Gộp liền kề";
            this.MergeTermAdjacent.Category = "Edit";
            this.MergeTermAdjacent.ConfirmationMessage = null;
            this.MergeTermAdjacent.Id = "MergeTermAdjacent";
            this.MergeTermAdjacent.ImageName = "Action_MergeTermAdjacent";
            this.MergeTermAdjacent.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MergeTermAdjacent.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MergeTermAdjacent.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MergeTermAdjacent.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.MergeTermAdjacent.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMergeTermAdjacentPrevious.Caption = "Kề trước";
            choiceActionItemMergeTermAdjacentPrevious.Data = "Previous";
            choiceActionItemMergeTermAdjacentPrevious.Id = "Previous";
            this.MergeTermAdjacent.Items.Add(choiceActionItemMergeTermAdjacentPrevious);

            
            //
            //Root Choice
            choiceActionItemMergeTermAdjacentNext.Caption = "Kề sau";
            choiceActionItemMergeTermAdjacentNext.Data = "Next";
            choiceActionItemMergeTermAdjacentNext.Id = "Next";
            this.MergeTermAdjacent.Items.Add(choiceActionItemMergeTermAdjacentNext);

            this.MergeTermAdjacent.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MergeTermAdjacent_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.MergeTermAdjacent);
			// TranslateTerm
            this.TranslateTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateTermTranslateTermContextApostrophe = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateTermKeepOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateTermTranslateTermContextStrong = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateTermSyncTermTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TranslateTerm
            // 
            this.TranslateTerm.Caption = "Dịch thuật ngữ";
            this.TranslateTerm.Category = "Edit";
            this.TranslateTerm.ConfirmationMessage = null;
            this.TranslateTerm.Id = "TranslateTerm";
            this.TranslateTerm.ImageName = "Action_TranslateTerm";
            this.TranslateTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TranslateTerm.ToolTip = "Ngăn Note dùng ký tự {}";  
            this.TranslateTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TranslateTerm.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.TranslateTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TranslateTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTranslateTermTranslateTermContextApostrophe.Caption = "Máy dịch Nháy đơn";
            choiceActionItemTranslateTermTranslateTermContextApostrophe.Data = "TranslateTermContextApostrophe";
            choiceActionItemTranslateTermTranslateTermContextApostrophe.Id = "TranslateTermContextApostrophe";
            this.TranslateTerm.Items.Add(choiceActionItemTranslateTermTranslateTermContextApostrophe);

            
            //
            //Root Choice
            choiceActionItemTranslateTermTranslateTermContextStrong.Caption = "Máy dịch Strong";
            choiceActionItemTranslateTermTranslateTermContextStrong.Data = "TranslateTermContextStrong";
            choiceActionItemTranslateTermTranslateTermContextStrong.Id = "TranslateTermContextStrong";
            this.TranslateTerm.Items.Add(choiceActionItemTranslateTermTranslateTermContextStrong);

            
            //
            //Root Choice
            choiceActionItemTranslateTermKeepOrigin.Caption = "Giữ nguyên";
            choiceActionItemTranslateTermKeepOrigin.Data = "KeepOrigin";
            choiceActionItemTranslateTermKeepOrigin.Id = "KeepOrigin";
            this.TranslateTerm.Items.Add(choiceActionItemTranslateTermKeepOrigin);

            
            //
            //Root Choice
            choiceActionItemTranslateTermSyncTermTranslate.Caption = "Đồng bộ thuật vị";
            choiceActionItemTranslateTermSyncTermTranslate.Data = "SyncTermTranslate";
            choiceActionItemTranslateTermSyncTermTranslate.Id = "SyncTermTranslate";
            this.TranslateTerm.Items.Add(choiceActionItemTranslateTermSyncTermTranslate);

            this.TranslateTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TranslateTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.TranslateTerm);
			// ReplaceTranslate
            this.ReplaceTranslate = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemReplaceTranslateReplace = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemReplaceTranslateUnReplace = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ReplaceTranslate
            // 
            this.ReplaceTranslate.Caption = "Thay dịch";
            this.ReplaceTranslate.Category = "Edit";
            this.ReplaceTranslate.ConfirmationMessage = null;
            this.ReplaceTranslate.Id = "ReplaceTranslate";
            this.ReplaceTranslate.ImageName = "Action_ReplaceTranslate";
            this.ReplaceTranslate.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ReplaceTranslate.ToolTip = "Ngăn Note dùng ký tự {}";  
            this.ReplaceTranslate.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ReplaceTranslate.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ReplaceTranslate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ReplaceTranslate.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemReplaceTranslateReplace.Caption = "Thay thế";
            choiceActionItemReplaceTranslateReplace.Data = "Replace";
            choiceActionItemReplaceTranslateReplace.Id = "Replace";
            this.ReplaceTranslate.Items.Add(choiceActionItemReplaceTranslateReplace);

            
            //
            //Root Choice
            choiceActionItemReplaceTranslateUnReplace.Caption = "Trả lại";
            choiceActionItemReplaceTranslateUnReplace.Data = "UnReplace";
            choiceActionItemReplaceTranslateUnReplace.Id = "UnReplace";
            this.ReplaceTranslate.Items.Add(choiceActionItemReplaceTranslateUnReplace);

            this.ReplaceTranslate.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ReplaceTranslate_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.ReplaceTranslate);
			// UpperLowerTerm
            this.UpperLowerTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerTermUpper = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerTermUpperAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerTermLower = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UpperLowerTerm
            // 
            this.UpperLowerTerm.Caption = "Chỉnh viết hoa";
            this.UpperLowerTerm.Category = "Edit";
            this.UpperLowerTerm.ConfirmationMessage = null;
            this.UpperLowerTerm.Id = "UpperLowerTerm";
            this.UpperLowerTerm.ImageName = "Action_UpperLowerTerm";
            this.UpperLowerTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.UpperLowerTerm.ToolTip = "Cần chọn cột Tên hoặc Dịch để thực hiện chức năng này";  
            this.UpperLowerTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UpperLowerTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.UpperLowerTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.UpperLowerTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUpperLowerTermUpper.Caption = "Đầu hoa";
            choiceActionItemUpperLowerTermUpper.Data = "Upper";
            choiceActionItemUpperLowerTermUpper.Id = "Upper";
            this.UpperLowerTerm.Items.Add(choiceActionItemUpperLowerTermUpper);

            
            //
            //Root Choice
            choiceActionItemUpperLowerTermLower.Caption = "Bỏ hoa";
            choiceActionItemUpperLowerTermLower.Data = "Lower";
            choiceActionItemUpperLowerTermLower.Id = "Lower";
            this.UpperLowerTerm.Items.Add(choiceActionItemUpperLowerTermLower);

            
            //
            //Root Choice
            choiceActionItemUpperLowerTermUpperAll.Caption = "Toàn hoa";
            choiceActionItemUpperLowerTermUpperAll.Data = "UpperAll";
            choiceActionItemUpperLowerTermUpperAll.Id = "UpperAll";
            this.UpperLowerTerm.Items.Add(choiceActionItemUpperLowerTermUpperAll);

            this.UpperLowerTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UpperLowerTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.UpperLowerTerm);
			// LookupWordType
            this.LookupWordType = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LookupWordType
            // 
            this.LookupWordType.Caption = "Từ loại";
            this.LookupWordType.Category = "Edit";
            this.LookupWordType.ConfirmationMessage = null;
            this.LookupWordType.Id = "LookupWordType";
            this.LookupWordType.ImageName = "Action_LookupWordType";
            this.LookupWordType.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.LookupWordType.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.LookupWordType.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.LookupWordType.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.LookupWordType.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LookupWordType_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.LookupWordType);
			// SynTerm
            this.SynTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSynTermModifyTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSynTermSynCaseTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSynTermSynCaseTerm = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SynTerm
            // 
            this.SynTerm.Caption = "Đồng bộ thuật ngữ";
            this.SynTerm.Category = "Edit";
            this.SynTerm.ConfirmationMessage = null;
            this.SynTerm.Id = "SynTerm";
            this.SynTerm.ImageName = "Action_SynTerm";
            this.SynTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SynTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SynTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.SynTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.SynTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSynTermSynCaseTerm.Caption = "Đồng bộ tên";
            choiceActionItemSynTermSynCaseTerm.Data = "SynCaseTerm";
            choiceActionItemSynTermSynCaseTerm.Id = "SynCaseTerm";
            this.SynTerm.Items.Add(choiceActionItemSynTermSynCaseTerm);

            
            //
            //Root Choice
            choiceActionItemSynTermSynCaseTranslate.Caption = "Đồng bộ dịch";
            choiceActionItemSynTermSynCaseTranslate.Data = "SynCaseTranslate";
            choiceActionItemSynTermSynCaseTranslate.Id = "SynCaseTranslate";
            this.SynTerm.Items.Add(choiceActionItemSynTermSynCaseTranslate);

            
            //
            //Root Choice
            choiceActionItemSynTermModifyTerm.Caption = "Sửa thuật ngữ";
            choiceActionItemSynTermModifyTerm.Data = "ModifyTerm";
            choiceActionItemSynTermModifyTerm.Id = "ModifyTerm";
            this.SynTerm.Items.Add(choiceActionItemSynTermModifyTerm);

            this.SynTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SynTerm_Execute);
            // 
            // TermViewController
            // 
            this.Actions.Add(this.SynTerm);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ExportTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction OverlapTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction EditWordTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SynTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TranslateTerm;
		private DevExpress.ExpressApp.Actions.SimpleAction LookupWordType;
		private DevExpress.ExpressApp.Actions.SimpleAction OpenTermElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UpperLowerTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ReplaceTranslate;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TermFlag;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ImportTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction Dictionary;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UpdatePosition;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SplitTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MergeTermAdjacent;
		private DevExpress.ExpressApp.Actions.SimpleAction NumberValue;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SpellingTerm;
    }
}