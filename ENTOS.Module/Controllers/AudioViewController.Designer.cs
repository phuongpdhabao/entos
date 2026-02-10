namespace ENTOS.Module.Controllers
{
    partial class AudioViewController
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
			// ElementTranslateSync
            this.ElementTranslateSync = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTranslateSyncRead = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTranslateSyncSave = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ElementTranslateSync
            // 
            this.ElementTranslateSync.Caption = "Đồng bộ Dịch ngữ";
            this.ElementTranslateSync.Category = "Edit";
            this.ElementTranslateSync.ConfirmationMessage = null;
            this.ElementTranslateSync.Id = "ElementTranslateSync";
            this.ElementTranslateSync.ImageName = "Action_ElementTranslateSync";
            this.ElementTranslateSync.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ElementTranslateSync.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ElementTranslateSync.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ElementTranslateSync.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ElementTranslateSync.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemElementTranslateSyncRead.Caption = "Đọc";
            choiceActionItemElementTranslateSyncRead.Data = "Read";
            choiceActionItemElementTranslateSyncRead.Id = "Read";
            this.ElementTranslateSync.Items.Add(choiceActionItemElementTranslateSyncRead);

            
            //
            //Root Choice
            choiceActionItemElementTranslateSyncSave.Caption = "Lưu";
            choiceActionItemElementTranslateSyncSave.Data = "Save";
            choiceActionItemElementTranslateSyncSave.Id = "Save";
            this.ElementTranslateSync.Items.Add(choiceActionItemElementTranslateSyncSave);

            this.ElementTranslateSync.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ElementTranslateSync_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ElementTranslateSync);
			// FindCaseType
            this.FindCaseType = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FindCaseType
            // 
            this.FindCaseType.Caption = "Xác định Kiểu chữ";
            this.FindCaseType.Category = "Edit";
            this.FindCaseType.ConfirmationMessage = null;
            this.FindCaseType.Id = "FindCaseType";
            this.FindCaseType.ImageName = "Action_FindCaseType";
            this.FindCaseType.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.FindCaseType.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.FindCaseType.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.FindCaseType.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.FindCaseType.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FindCaseType_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.FindCaseType);
			// PreviousNextElement
            this.PreviousNextElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PreviousNextElement
            // 
            this.PreviousNextElement.Caption = "Trước sau";
            this.PreviousNextElement.Category = "Edit";
            this.PreviousNextElement.ConfirmationMessage = null;
            this.PreviousNextElement.Id = "PreviousNextElement";
            this.PreviousNextElement.ImageName = "Action_PreviousNextElement";
            this.PreviousNextElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.PreviousNextElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.PreviousNextElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.PreviousNextElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.PreviousNextElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviousNextElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.PreviousNextElement);
			// UpperLowerElement
            this.UpperLowerElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerElementUpperAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerElementUpperFirstLetter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerElementLowerKeepAbbreviation = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerElementUpperElementBegin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpperLowerElementLowerAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UpperLowerElement
            // 
            this.UpperLowerElement.Caption = "Chỉnh viết hoa";
            this.UpperLowerElement.Category = "Edit";
            this.UpperLowerElement.ConfirmationMessage = null;
            this.UpperLowerElement.Id = "UpperLowerElement";
            this.UpperLowerElement.ImageName = "Action_UpperLowerElement";
            this.UpperLowerElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.UpperLowerElement.ToolTip = "Cần chọn cột Nội dung hoặc Dịch để thực hiện chức năng này";  
            this.UpperLowerElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UpperLowerElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.UpperLowerElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.UpperLowerElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUpperLowerElementUpperElementBegin.Caption = "Hoa đầu thành phần";
            choiceActionItemUpperLowerElementUpperElementBegin.Data = "UpperElementBegin";
            choiceActionItemUpperLowerElementUpperElementBegin.Id = "UpperElementBegin";
            this.UpperLowerElement.Items.Add(choiceActionItemUpperLowerElementUpperElementBegin);

            
            //
            //Root Choice
            choiceActionItemUpperLowerElementUpperFirstLetter.Caption = "Hoa đầu mỗi từ";
            choiceActionItemUpperLowerElementUpperFirstLetter.Data = "UpperFirstLetter";
            choiceActionItemUpperLowerElementUpperFirstLetter.Id = "UpperFirstLetter";
            this.UpperLowerElement.Items.Add(choiceActionItemUpperLowerElementUpperFirstLetter);

            
            //
            //Root Choice
            choiceActionItemUpperLowerElementUpperAll.Caption = "Hoa toàn phần";
            choiceActionItemUpperLowerElementUpperAll.Data = "UpperAll";
            choiceActionItemUpperLowerElementUpperAll.Id = "UpperAll";
            this.UpperLowerElement.Items.Add(choiceActionItemUpperLowerElementUpperAll);

            
            //
            //Root Choice
            choiceActionItemUpperLowerElementLowerKeepAbbreviation.Caption = "Bỏ hoa giữ tắt";
            choiceActionItemUpperLowerElementLowerKeepAbbreviation.Data = "LowerKeepAbbreviation";
            choiceActionItemUpperLowerElementLowerKeepAbbreviation.Id = "LowerKeepAbbreviation";
            this.UpperLowerElement.Items.Add(choiceActionItemUpperLowerElementLowerKeepAbbreviation);

            
            //
            //Root Choice
            choiceActionItemUpperLowerElementLowerAll.Caption = "Bỏ hoa toàn bộ";
            choiceActionItemUpperLowerElementLowerAll.Data = "LowerAll";
            choiceActionItemUpperLowerElementLowerAll.Id = "LowerAll";
            this.UpperLowerElement.Items.Add(choiceActionItemUpperLowerElementLowerAll);

            this.UpperLowerElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UpperLowerElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.UpperLowerElement);
			// ElementVoiceSpeed
            this.ElementVoiceSpeed = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementVoiceSpeedAverage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ElementVoiceSpeed
            // 
            this.ElementVoiceSpeed.Caption = "Nạp tốc độ";
            this.ElementVoiceSpeed.Category = "Edit";
            this.ElementVoiceSpeed.ConfirmationMessage = null;
            this.ElementVoiceSpeed.Id = "ElementVoiceSpeed";
            this.ElementVoiceSpeed.ImageName = "Action_ElementVoiceSpeed";
            this.ElementVoiceSpeed.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ElementVoiceSpeed.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ElementVoiceSpeed.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ElementVoiceSpeed.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ElementVoiceSpeed.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemElementVoiceSpeedAverage.Caption = "Trung bình";
            choiceActionItemElementVoiceSpeedAverage.Data = "Average";
            choiceActionItemElementVoiceSpeedAverage.Id = "Average";
            this.ElementVoiceSpeed.Items.Add(choiceActionItemElementVoiceSpeedAverage);

            this.ElementVoiceSpeed.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ElementVoiceSpeed_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ElementVoiceSpeed);
			// ImportElementTerm
            this.ImportElementTerm = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermDictionary = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermNumber = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermCompoundWord3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermSingleWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermDateTime = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportElementTermCompoundWord2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ImportElementTerm
            // 
            this.ImportElementTerm.Caption = "Nạp thuật ngữ";
            this.ImportElementTerm.Category = "Edit";
            this.ImportElementTerm.ConfirmationMessage = null;
            this.ImportElementTerm.Id = "ImportElementTerm";
            this.ImportElementTerm.ImageName = "Action_ImportElementTerm";
            this.ImportElementTerm.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ImportElementTerm.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportElementTerm.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ImportElementTerm.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ImportElementTerm.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemImportElementTermDictionary.Caption = "Từ điển";
            choiceActionItemImportElementTermDictionary.Data = "Dictionary";
            choiceActionItemImportElementTermDictionary.Id = "Dictionary";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermDictionary);

            
            //
            //Root Choice
            choiceActionItemImportElementTermDateTime.Caption = "Thời gian";
            choiceActionItemImportElementTermDateTime.Data = "DateTime";
            choiceActionItemImportElementTermDateTime.Id = "DateTime";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermDateTime);

            
            //
            //Root Choice
            choiceActionItemImportElementTermUpperCase.Caption = "Từ hoa";
            choiceActionItemImportElementTermUpperCase.Data = "UpperCase";
            choiceActionItemImportElementTermUpperCase.Id = "UpperCase";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermUpperCase);

            
            //
            //Root Choice
            choiceActionItemImportElementTermCompoundWord3.Caption = "Từ ghép 3";
            choiceActionItemImportElementTermCompoundWord3.Data = "CompoundWord3";
            choiceActionItemImportElementTermCompoundWord3.Id = "CompoundWord3";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermCompoundWord3);

            
            //
            //Root Choice
            choiceActionItemImportElementTermCompoundWord2.Caption = "Từ ghép 2";
            choiceActionItemImportElementTermCompoundWord2.Data = "CompoundWord2";
            choiceActionItemImportElementTermCompoundWord2.Id = "CompoundWord2";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermCompoundWord2);

            
            //
            //Root Choice
            choiceActionItemImportElementTermNumber.Caption = "Số";
            choiceActionItemImportElementTermNumber.Data = "Number";
            choiceActionItemImportElementTermNumber.Id = "Number";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermNumber);

            
            //
            //Root Choice
            choiceActionItemImportElementTermSingleWord.Caption = "Từ đơn";
            choiceActionItemImportElementTermSingleWord.Data = "SingleWord";
            choiceActionItemImportElementTermSingleWord.Id = "SingleWord";
            this.ImportElementTerm.Items.Add(choiceActionItemImportElementTermSingleWord);

            this.ImportElementTerm.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ImportElementTerm_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ImportElementTerm);
			// SpellingAudio
            this.SpellingAudio = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioSpellCorrect = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioSpellCorrectSpellCorrectOrigin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioSpellCorrectSpellCorrectTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioSpellCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSpellingAudioRepeatChar = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SpellingAudio
            // 
            this.SpellingAudio.Caption = "Chính tả";
            this.SpellingAudio.Category = "Edit";
            this.SpellingAudio.ConfirmationMessage = null;
            this.SpellingAudio.Id = "SpellingAudio";
            this.SpellingAudio.ImageName = "Action_SpellingAudio";
            this.SpellingAudio.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SpellingAudio.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SpellingAudio.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.SpellingAudio.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.SpellingAudio.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSpellingAudioTranslate.Caption = "Dịch";
            choiceActionItemSpellingAudioTranslate.Data = "Translate";
            choiceActionItemSpellingAudioTranslate.Id = "Translate";
            this.SpellingAudio.Items.Add(choiceActionItemSpellingAudioTranslate);

            
            //
            //Root Choice
            choiceActionItemSpellingAudioSpellCheck.Caption = "Kiểm tra";
            choiceActionItemSpellingAudioSpellCheck.Data = "SpellCheck";
            choiceActionItemSpellingAudioSpellCheck.Id = "SpellCheck";
            this.SpellingAudio.Items.Add(choiceActionItemSpellingAudioSpellCheck);

            
            //
            //Root Choice
            choiceActionItemSpellingAudioSpellCorrect.Caption = "Sửa lỗi ";
            choiceActionItemSpellingAudioSpellCorrect.Data = "SpellCorrect";
            choiceActionItemSpellingAudioSpellCorrect.Id = "SpellCorrect";
            this.SpellingAudio.Items.Add(choiceActionItemSpellingAudioSpellCorrect);

            
            //
            //Choice
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectOrigin.Caption = "Ngữ gốc";
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectOrigin.Data = "SpellCorrectOrigin";
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectOrigin.Id = "SpellCorrectOrigin";
            choiceActionItemSpellingAudioSpellCorrect.Items.Add(choiceActionItemSpellingAudioSpellCorrectSpellCorrectOrigin);
             
            //
            //Choice
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectTranslate.Caption = "Ngữ dịch";
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectTranslate.Data = "SpellCorrectTranslate";
            choiceActionItemSpellingAudioSpellCorrectSpellCorrectTranslate.Id = "SpellCorrectTranslate";
            choiceActionItemSpellingAudioSpellCorrect.Items.Add(choiceActionItemSpellingAudioSpellCorrectSpellCorrectTranslate);
             
            //
            //Root Choice
            choiceActionItemSpellingAudioRepeatChar.Caption = "Kí tự lặp";
            choiceActionItemSpellingAudioRepeatChar.Data = "RepeatChar";
            choiceActionItemSpellingAudioRepeatChar.Id = "RepeatChar";
            this.SpellingAudio.Items.Add(choiceActionItemSpellingAudioRepeatChar);

            this.SpellingAudio.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SpellingAudio_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.SpellingAudio);
			// ConvertTo
            this.ConvertTo = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemConvertToTextbox = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ConvertTo
            // 
            this.ConvertTo.Caption = "Chuyển đổi";
            this.ConvertTo.Category = "Edit";
            this.ConvertTo.ConfirmationMessage = null;
            this.ConvertTo.Id = "ConvertTo";
            this.ConvertTo.ImageName = "Action_ConvertTo";
            this.ConvertTo.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ConvertTo.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ConvertTo.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ConvertTo.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.ConvertTo.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemConvertToTextbox.Caption = "Textbox";
            choiceActionItemConvertToTextbox.Data = "Textbox";
            choiceActionItemConvertToTextbox.Id = "Textbox";
            this.ConvertTo.Items.Add(choiceActionItemConvertToTextbox);

            this.ConvertTo.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ConvertTo_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ConvertTo);
			// ElementFlag
            this.ElementFlag = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagBegin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagBeginBeginNotUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagBeginBeginSignSpecialCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagBeginBeginAbbreviationOrNumber = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagBeginBeginSpaces = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagDifferentSentences = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagUpperCase = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagMergeNext = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagUpperCaseSecond = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagClear = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagMultiSentence = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagMultiLine = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagMergePrevious = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagUpperCaseAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagAudioOverlap = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagTwin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagUpperCaseMany = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagSpellCheck = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagNext = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndPart = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagRepeatWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagWordUpperCaseAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEnd = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndEndNormalCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndEndComma = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndEndSpaces = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndEndAbbreviationOrNumber = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEndEndSignOrSpecialCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagCompareSubtitleAndSpelling = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagPrevious = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagContain = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagEnglish = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagCharacterError = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagComma = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementFlagHaveFootnote = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ElementFlag
            // 
            this.ElementFlag.Caption = "Cờ thành phần";
            this.ElementFlag.Category = "Edit";
            this.ElementFlag.ConfirmationMessage = null;
            this.ElementFlag.Id = "ElementFlag";
            this.ElementFlag.ImageName = "Action_ElementFlag";
            this.ElementFlag.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ElementFlag.ToolTip = "Ngăn Note dùng ký tự < >";  
            this.ElementFlag.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ElementFlag.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ElementFlag.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ElementFlag.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemElementFlagBegin.Caption = "Bắt đầu";
            choiceActionItemElementFlagBegin.Data = "Begin";
            choiceActionItemElementFlagBegin.Id = "Begin";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagBegin);

            
            //
            //Choice
            choiceActionItemElementFlagBeginBeginNotUpperCase.Caption = "Ký tự thường";
            choiceActionItemElementFlagBeginBeginNotUpperCase.Data = "BeginNotUpperCase";
            choiceActionItemElementFlagBeginBeginNotUpperCase.Id = "BeginNotUpperCase";
            choiceActionItemElementFlagBegin.Items.Add(choiceActionItemElementFlagBeginBeginNotUpperCase);
             
            //
            //Choice
            choiceActionItemElementFlagBeginBeginAbbreviationOrNumber.Caption = "Viết tắt hoặc số";
            choiceActionItemElementFlagBeginBeginAbbreviationOrNumber.Data = "BeginAbbreviationOrNumber";
            choiceActionItemElementFlagBeginBeginAbbreviationOrNumber.Id = "BeginAbbreviationOrNumber";
            choiceActionItemElementFlagBegin.Items.Add(choiceActionItemElementFlagBeginBeginAbbreviationOrNumber);
             
            //
            //Choice
            choiceActionItemElementFlagBeginBeginSignSpecialCharacter.Caption = "Dấu, ký tự đặc biệt";
            choiceActionItemElementFlagBeginBeginSignSpecialCharacter.Data = "BeginSignSpecialCharacter";
            choiceActionItemElementFlagBeginBeginSignSpecialCharacter.Id = "BeginSignSpecialCharacter";
            choiceActionItemElementFlagBegin.Items.Add(choiceActionItemElementFlagBeginBeginSignSpecialCharacter);
             
            //
            //Choice
            choiceActionItemElementFlagBeginBeginSpaces.Caption = "Nhiều dấu cách";
            choiceActionItemElementFlagBeginBeginSpaces.Data = "BeginSpaces";
            choiceActionItemElementFlagBeginBeginSpaces.Id = "BeginSpaces";
            choiceActionItemElementFlagBegin.Items.Add(choiceActionItemElementFlagBeginBeginSpaces);
             
            //
            //Root Choice
            choiceActionItemElementFlagEnd.Caption = "Kết thúc";
            choiceActionItemElementFlagEnd.Data = "End";
            choiceActionItemElementFlagEnd.Id = "End";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagEnd);

            
            //
            //Choice
            choiceActionItemElementFlagEndEndNormalCharacter.Caption = "Ký tự thường";
            choiceActionItemElementFlagEndEndNormalCharacter.Data = "EndNormalCharacter";
            choiceActionItemElementFlagEndEndNormalCharacter.Id = "EndNormalCharacter";
            choiceActionItemElementFlagEnd.Items.Add(choiceActionItemElementFlagEndEndNormalCharacter);
             
            //
            //Choice
            choiceActionItemElementFlagEndEndComma.Caption = "Dấu phẩy";
            choiceActionItemElementFlagEndEndComma.Data = "EndComma";
            choiceActionItemElementFlagEndEndComma.Id = "EndComma";
            choiceActionItemElementFlagEnd.Items.Add(choiceActionItemElementFlagEndEndComma);
             
            //
            //Choice
            choiceActionItemElementFlagEndEndSignOrSpecialCharacter.Caption = "Dấu, ký tự đặc biệt";
            choiceActionItemElementFlagEndEndSignOrSpecialCharacter.Data = "EndSignOrSpecialCharacter";
            choiceActionItemElementFlagEndEndSignOrSpecialCharacter.Id = "EndSignOrSpecialCharacter";
            choiceActionItemElementFlagEnd.Items.Add(choiceActionItemElementFlagEndEndSignOrSpecialCharacter);
             
            //
            //Choice
            choiceActionItemElementFlagEndEndAbbreviationOrNumber.Caption = "Viết tắt hoặc số";
            choiceActionItemElementFlagEndEndAbbreviationOrNumber.Data = "EndAbbreviationOrNumber";
            choiceActionItemElementFlagEndEndAbbreviationOrNumber.Id = "EndAbbreviationOrNumber";
            choiceActionItemElementFlagEnd.Items.Add(choiceActionItemElementFlagEndEndAbbreviationOrNumber);
             
            //
            //Choice
            choiceActionItemElementFlagEndEndSpaces.Caption = "Nhiều dấu cách";
            choiceActionItemElementFlagEndEndSpaces.Data = "EndSpaces";
            choiceActionItemElementFlagEndEndSpaces.Id = "EndSpaces";
            choiceActionItemElementFlagEnd.Items.Add(choiceActionItemElementFlagEndEndSpaces);
             
            //
            //Root Choice
            choiceActionItemElementFlagMergePrevious.Caption = "Có thể gộp trên";
            choiceActionItemElementFlagMergePrevious.Data = "MergePrevious";
            choiceActionItemElementFlagMergePrevious.Id = "MergePrevious";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagMergePrevious);

            
            //
            //Root Choice
            choiceActionItemElementFlagMergeNext.Caption = "Có thể gộp dưới";
            choiceActionItemElementFlagMergeNext.Data = "MergeNext";
            choiceActionItemElementFlagMergeNext.Id = "MergeNext";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagMergeNext);

            
            //
            //Root Choice
            choiceActionItemElementFlagPrevious.Caption = "Kề trên";
            choiceActionItemElementFlagPrevious.Data = "Previous";
            choiceActionItemElementFlagPrevious.Id = "Previous";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagPrevious);

            
            //
            //Root Choice
            choiceActionItemElementFlagNext.Caption = "Kề dưới";
            choiceActionItemElementFlagNext.Data = "Next";
            choiceActionItemElementFlagNext.Id = "Next";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagNext);

            
            //
            //Root Choice
            choiceActionItemElementFlagUpperCase.Caption = "Đầu hoa";
            choiceActionItemElementFlagUpperCase.Data = "UpperCase";
            choiceActionItemElementFlagUpperCase.Id = "UpperCase";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagUpperCase);

            
            //
            //Root Choice
            choiceActionItemElementFlagUpperCaseAll.Caption = "Toàn hoa";
            choiceActionItemElementFlagUpperCaseAll.Data = "UpperCaseAll";
            choiceActionItemElementFlagUpperCaseAll.Id = "UpperCaseAll";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagUpperCaseAll);

            
            //
            //Root Choice
            choiceActionItemElementFlagUpperCaseMany.Caption = "Nhiều hoa";
            choiceActionItemElementFlagUpperCaseMany.Data = "UpperCaseMany";
            choiceActionItemElementFlagUpperCaseMany.Id = "UpperCaseMany";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagUpperCaseMany);

            
            //
            //Root Choice
            choiceActionItemElementFlagDifferentSentences.Caption = "Khác số câu";
            choiceActionItemElementFlagDifferentSentences.Data = "DifferentSentences";
            choiceActionItemElementFlagDifferentSentences.Id = "DifferentSentences";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagDifferentSentences);

            
            //
            //Root Choice
            choiceActionItemElementFlagEndPart.Caption = "Phần cuối";
            choiceActionItemElementFlagEndPart.Data = "EndPart";
            choiceActionItemElementFlagEndPart.Id = "EndPart";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagEndPart);

            
            //
            //Root Choice
            choiceActionItemElementFlagRepeatWord.Caption = "Từ lặp trong câu";
            choiceActionItemElementFlagRepeatWord.Data = "RepeatWord";
            choiceActionItemElementFlagRepeatWord.Id = "RepeatWord";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagRepeatWord);

            
            //
            //Root Choice
            choiceActionItemElementFlagEnglish.Caption = "Tiếng Anh trong phiên âm";
            choiceActionItemElementFlagEnglish.Data = "English";
            choiceActionItemElementFlagEnglish.Id = "English";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagEnglish);

            
            //
            //Root Choice
            choiceActionItemElementFlagClear.Caption = "Xóa cờ";
            choiceActionItemElementFlagClear.Data = "Clear";
            choiceActionItemElementFlagClear.Id = "Clear";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagClear);

            
            //
            //Root Choice
            choiceActionItemElementFlagSpellCheck.Caption = "Lỗi chính tả";
            choiceActionItemElementFlagSpellCheck.Data = "SpellCheck";
            choiceActionItemElementFlagSpellCheck.Id = "SpellCheck";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagSpellCheck);

            
            //
            //Root Choice
            choiceActionItemElementFlagCompareSubtitleAndSpelling.Caption = "Dịch khác Phiên âm";
            choiceActionItemElementFlagCompareSubtitleAndSpelling.Data = "CompareSubtitleAndSpelling";
            choiceActionItemElementFlagCompareSubtitleAndSpelling.Id = "CompareSubtitleAndSpelling";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagCompareSubtitleAndSpelling);

            
            //
            //Root Choice
            choiceActionItemElementFlagMultiSentence.Caption = "Có ngắt câu";
            choiceActionItemElementFlagMultiSentence.Data = "MultiSentence";
            choiceActionItemElementFlagMultiSentence.Id = "MultiSentence";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagMultiSentence);

            
            //
            //Root Choice
            choiceActionItemElementFlagComma.Caption = "Có dấu phẩy";
            choiceActionItemElementFlagComma.Data = "Comma";
            choiceActionItemElementFlagComma.Id = "Comma";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagComma);

            
            //
            //Root Choice
            choiceActionItemElementFlagAudioOverlap.Caption = "Đè âm";
            choiceActionItemElementFlagAudioOverlap.Data = "AudioOverlap";
            choiceActionItemElementFlagAudioOverlap.Id = "AudioOverlap";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagAudioOverlap);

            
            //
            //Root Choice
            choiceActionItemElementFlagTwin.Caption = "Song sinh";
            choiceActionItemElementFlagTwin.Data = "Twin";
            choiceActionItemElementFlagTwin.Id = "Twin";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagTwin);

            
            //
            //Root Choice
            choiceActionItemElementFlagContain.Caption = "Bao hàm";
            choiceActionItemElementFlagContain.Data = "Contain";
            choiceActionItemElementFlagContain.Id = "Contain";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagContain);

            
            //
            //Root Choice
            choiceActionItemElementFlagCharacterError.Caption = "Ký tự lỗi";
            choiceActionItemElementFlagCharacterError.Data = "CharacterError";
            choiceActionItemElementFlagCharacterError.Id = "CharacterError";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagCharacterError);

            
            //
            //Root Choice
            choiceActionItemElementFlagUpperCaseSecond.Caption = "Hoa từ 2";
            choiceActionItemElementFlagUpperCaseSecond.Data = "UpperCaseSecond";
            choiceActionItemElementFlagUpperCaseSecond.Id = "UpperCaseSecond";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagUpperCaseSecond);

            
            //
            //Root Choice
            choiceActionItemElementFlagHaveFootnote.Caption = "Có footnote";
            choiceActionItemElementFlagHaveFootnote.Data = "HaveFootnote";
            choiceActionItemElementFlagHaveFootnote.Id = "HaveFootnote";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagHaveFootnote);

            
            //
            //Root Choice
            choiceActionItemElementFlagWordUpperCaseAll.Caption = "Từ toàn hoa";
            choiceActionItemElementFlagWordUpperCaseAll.Data = "WordUpperCaseAll";
            choiceActionItemElementFlagWordUpperCaseAll.Id = "WordUpperCaseAll";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagWordUpperCaseAll);

            
            //
            //Root Choice
            choiceActionItemElementFlagMultiLine.Caption = "Nhiều dòng";
            choiceActionItemElementFlagMultiLine.Data = "MultiLine";
            choiceActionItemElementFlagMultiLine.Id = "MultiLine";
            this.ElementFlag.Items.Add(choiceActionItemElementFlagMultiLine);

            this.ElementFlag.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ElementFlag_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ElementFlag);
			// SplitElement
            this.SplitElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitElementSplitSubtitleDot = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitElementContainBegin = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitElementSplitSubtitleComma = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemSplitElementContainEnd = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // SplitElement
            // 
            this.SplitElement.Caption = "Tách dòng";
            this.SplitElement.Category = "Edit";
            this.SplitElement.ConfirmationMessage = null;
            this.SplitElement.Id = "SplitElement";
            this.SplitElement.ImageName = "Action_SplitElement";
            this.SplitElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.SplitElement.ToolTip = "Không thể gộp khi đã có thuật vị";  
			
			this.SplitElement.TargetObjectsCriteria = "TermLocationList.Count() = 0";  
            this.SplitElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SplitElement.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.SplitElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.SplitElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemSplitElementSplitSubtitleDot.Caption = "Tách nhiều theo chấm";
            choiceActionItemSplitElementSplitSubtitleDot.Data = "SplitSubtitleDot";
            choiceActionItemSplitElementSplitSubtitleDot.Id = "SplitSubtitleDot";
            this.SplitElement.Items.Add(choiceActionItemSplitElementSplitSubtitleDot);

            
            //
            //Root Choice
            choiceActionItemSplitElementSplitSubtitleComma.Caption = "Tách đôi theo phẩy";
            choiceActionItemSplitElementSplitSubtitleComma.Data = "SplitSubtitleComma";
            choiceActionItemSplitElementSplitSubtitleComma.Id = "SplitSubtitleComma";
            this.SplitElement.Items.Add(choiceActionItemSplitElementSplitSubtitleComma);

            
            //
            //Root Choice
            choiceActionItemSplitElementContainBegin.Caption = "Bao hàm đầu";
            choiceActionItemSplitElementContainBegin.Data = "ContainBegin";
            choiceActionItemSplitElementContainBegin.Id = "ContainBegin";
            this.SplitElement.Items.Add(choiceActionItemSplitElementContainBegin);

            
            //
            //Root Choice
            choiceActionItemSplitElementContainEnd.Caption = "Bao hàm cuối";
            choiceActionItemSplitElementContainEnd.Data = "ContainEnd";
            choiceActionItemSplitElementContainEnd.Id = "ContainEnd";
            this.SplitElement.Items.Add(choiceActionItemSplitElementContainEnd);

            this.SplitElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SplitElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.SplitElement);
			// MergeTwoElement
            this.MergeTwoElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTwoElementToBelow = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTwoElementAbove = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTwoElementBelow = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeTwoElementToAbove = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MergeTwoElement
            // 
            this.MergeTwoElement.Caption = "Gộp trên dưới";
            this.MergeTwoElement.Category = "Edit";
            this.MergeTwoElement.ConfirmationMessage = null;
            this.MergeTwoElement.Id = "MergeTwoElement";
            this.MergeTwoElement.ImageName = "Action_MergeTwoElement";
            this.MergeTwoElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MergeTwoElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MergeTwoElement.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.MergeTwoElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MergeTwoElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMergeTwoElementBelow.Caption = "Dòng kề dưới";
            choiceActionItemMergeTwoElementBelow.Data = "Below";
            choiceActionItemMergeTwoElementBelow.Id = "Below";
            this.MergeTwoElement.Items.Add(choiceActionItemMergeTwoElementBelow);

            
            //
            //Root Choice
            choiceActionItemMergeTwoElementAbove.Caption = "Dòng kề trên";
            choiceActionItemMergeTwoElementAbove.Data = "Above";
            choiceActionItemMergeTwoElementAbove.Id = "Above";
            this.MergeTwoElement.Items.Add(choiceActionItemMergeTwoElementAbove);

            
            //
            //Root Choice
            choiceActionItemMergeTwoElementToBelow.Caption = "Xuống dòng dưới";
            choiceActionItemMergeTwoElementToBelow.Data = "ToBelow";
            choiceActionItemMergeTwoElementToBelow.Id = "ToBelow";
            this.MergeTwoElement.Items.Add(choiceActionItemMergeTwoElementToBelow);

            
            //
            //Root Choice
            choiceActionItemMergeTwoElementToAbove.Caption = "Lên dòng trên";
            choiceActionItemMergeTwoElementToAbove.Data = "ToAbove";
            choiceActionItemMergeTwoElementToAbove.Id = "ToAbove";
            this.MergeTwoElement.Items.Add(choiceActionItemMergeTwoElementToAbove);

            this.MergeTwoElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MergeTwoElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.MergeTwoElement);
			// AudioRecord
            this.AudioRecord = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AudioRecord
            // 
            this.AudioRecord.Caption = "Thu âm";
            this.AudioRecord.Category = "Edit";
            this.AudioRecord.ConfirmationMessage = null;
            this.AudioRecord.Id = "AudioRecord";
            this.AudioRecord.ImageName = "Action_AudioRecord";
            this.AudioRecord.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.AudioRecord.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.AudioRecord.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.AudioRecord.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.AudioRecord.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AudioRecord_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.AudioRecord);
			// TextQuantity
            this.TextQuantity = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextQuantityLineBreak = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextQuantityCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextQuantitySyllable = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextQuantityWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TextQuantity
            // 
            this.TextQuantity.Caption = "Số lượng";
            this.TextQuantity.Category = "Edit";
            this.TextQuantity.ConfirmationMessage = null;
            this.TextQuantity.Id = "TextQuantity";
            this.TextQuantity.ImageName = "Action_TextQuantity";
            this.TextQuantity.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TextQuantity.ToolTip = "Ngăn Note dùng ký tự ()";  
            this.TextQuantity.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TextQuantity.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TextQuantity.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TextQuantity.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTextQuantityWord.Caption = "Từ";
            choiceActionItemTextQuantityWord.Data = "Word";
            choiceActionItemTextQuantityWord.Id = "Word";
            this.TextQuantity.Items.Add(choiceActionItemTextQuantityWord);

            
            //
            //Root Choice
            choiceActionItemTextQuantityCharacter.Caption = "Kí tự";
            choiceActionItemTextQuantityCharacter.Data = "Character";
            choiceActionItemTextQuantityCharacter.Id = "Character";
            this.TextQuantity.Items.Add(choiceActionItemTextQuantityCharacter);

            
            //
            //Root Choice
            choiceActionItemTextQuantitySyllable.Caption = "Âm tiết";
            choiceActionItemTextQuantitySyllable.Data = "Syllable";
            choiceActionItemTextQuantitySyllable.Id = "Syllable";
            this.TextQuantity.Items.Add(choiceActionItemTextQuantitySyllable);

            
            //
            //Root Choice
            choiceActionItemTextQuantityLineBreak.Caption = "Ngắt câu";
            choiceActionItemTextQuantityLineBreak.Data = "LineBreak";
            choiceActionItemTextQuantityLineBreak.Id = "LineBreak";
            this.TextQuantity.Items.Add(choiceActionItemTextQuantityLineBreak);

            this.TextQuantity.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TextQuantity_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.TextQuantity);
			// MergeElement
            this.MergeElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeElementMergeMultiElementComma = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeElementMergeMultiElementNewLine = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMergeElementMergeMultiElementDot = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MergeElement
            // 
            this.MergeElement.Caption = "Gộp nhiều";
            this.MergeElement.Category = "Edit";
            this.MergeElement.ConfirmationMessage = null;
            this.MergeElement.Id = "MergeElement";
            this.MergeElement.ImageName = "Action_MergeElement";
            this.MergeElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.MergeElement.ToolTip = "Dòng đầu không kết thúc dấu chấm. Và dòng sau không được bắt đầu chữ Hoa";  
            this.MergeElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MergeElement.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.MergeElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MergeElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMergeElementMergeMultiElementDot.Caption = "Chấm";
            choiceActionItemMergeElementMergeMultiElementDot.Data = "MergeMultiElementDot";
            choiceActionItemMergeElementMergeMultiElementDot.Id = "MergeMultiElementDot";
            this.MergeElement.Items.Add(choiceActionItemMergeElementMergeMultiElementDot);

            
            //
            //Root Choice
            choiceActionItemMergeElementMergeMultiElementComma.Caption = "Phẩy";
            choiceActionItemMergeElementMergeMultiElementComma.Data = "MergeMultiElementComma";
            choiceActionItemMergeElementMergeMultiElementComma.Id = "MergeMultiElementComma";
            this.MergeElement.Items.Add(choiceActionItemMergeElementMergeMultiElementComma);

            
            //
            //Root Choice
            choiceActionItemMergeElementMergeMultiElementNewLine.Caption = "Ngắt đoạn";
            choiceActionItemMergeElementMergeMultiElementNewLine.Data = "MergeMultiElementNewLine";
            choiceActionItemMergeElementMergeMultiElementNewLine.Id = "MergeMultiElementNewLine";
            this.MergeElement.Items.Add(choiceActionItemMergeElementMergeMultiElementNewLine);

            this.MergeElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MergeElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.MergeElement);
			// TextRate
            this.TextRate = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextRateWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextRateCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTextRateSyllable = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TextRate
            // 
            this.TextRate.Caption = "Tỉ suất";
            this.TextRate.Category = "Edit";
            this.TextRate.ConfirmationMessage = null;
            this.TextRate.Id = "TextRate";
            this.TextRate.ImageName = "Action_TextRate";
            this.TextRate.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TextRate.ToolTip = "Ngăn Note dùng ký tự ||";  
            this.TextRate.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TextRate.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TextRate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.TextRate.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTextRateWord.Caption = "Từ";
            choiceActionItemTextRateWord.Data = "Word";
            choiceActionItemTextRateWord.Id = "Word";
            this.TextRate.Items.Add(choiceActionItemTextRateWord);

            
            //
            //Root Choice
            choiceActionItemTextRateCharacter.Caption = "Kí tự";
            choiceActionItemTextRateCharacter.Data = "Character";
            choiceActionItemTextRateCharacter.Id = "Character";
            this.TextRate.Items.Add(choiceActionItemTextRateCharacter);

            
            //
            //Root Choice
            choiceActionItemTextRateSyllable.Caption = "Âm tiết";
            choiceActionItemTextRateSyllable.Data = "Syllable";
            choiceActionItemTextRateSyllable.Id = "Syllable";
            this.TextRate.Items.Add(choiceActionItemTextRateSyllable);

            this.TextRate.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TextRate_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.TextRate);
			// ElementTextReplace
            this.ElementTextReplace = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceKeepOnlySpeakerName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceCharacterError = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceDeleteSpeakerName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceWordCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceWordCaseMarkWordCaseNoMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceWordCaseMarkWordCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseNoMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceStringCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseNoMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceStringCaseMarkStringCaseNoMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceStringCaseMarkStringCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseMark = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ElementTextReplace
            // 
            this.ElementTextReplace.Caption = "Thay thế";
            this.ElementTextReplace.Category = "Edit";
            this.ElementTextReplace.ConfirmationMessage = null;
            this.ElementTextReplace.Id = "ElementTextReplace";
            this.ElementTextReplace.ImageName = "Action_ElementTextReplace";
            this.ElementTextReplace.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ElementTextReplace.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.ElementTextReplace.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ElementTextReplace.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ElementTextReplace.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemElementTextReplaceWordCaseMark.Caption = "Từ";
            choiceActionItemElementTextReplaceWordCaseMark.Data = "WordCaseMark";
            choiceActionItemElementTextReplaceWordCaseMark.Id = "WordCaseMark";
            this.ElementTextReplace.Items.Add(choiceActionItemElementTextReplaceWordCaseMark);

            
            //
            //Choice
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseMark.Caption = "Khớp hoa khớp dấu";
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseMark.Data = "WordCaseMark";
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseMark.Id = "WordCaseMark";
            choiceActionItemElementTextReplaceWordCaseMark.Items.Add(choiceActionItemElementTextReplaceWordCaseMarkWordCaseMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseNoMark.Caption = "Khớp hoa không dấu";
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseNoMark.Data = "WordCaseNoMark";
            choiceActionItemElementTextReplaceWordCaseMarkWordCaseNoMark.Id = "WordCaseNoMark";
            choiceActionItemElementTextReplaceWordCaseMark.Items.Add(choiceActionItemElementTextReplaceWordCaseMarkWordCaseNoMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseMark.Caption = "Tùy hoa khớp dấu";
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseMark.Data = "WordNoCaseMark";
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseMark.Id = "WordNoCaseMark";
            choiceActionItemElementTextReplaceWordCaseMark.Items.Add(choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseNoMark.Caption = "Tùy hoa không dấu";
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseNoMark.Data = "WordNoCaseNoMark";
            choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseNoMark.Id = "WordNoCaseNoMark";
            choiceActionItemElementTextReplaceWordCaseMark.Items.Add(choiceActionItemElementTextReplaceWordCaseMarkWordNoCaseNoMark);
             
            //
            //Root Choice
            choiceActionItemElementTextReplaceStringCaseMark.Caption = "Kí tự";
            choiceActionItemElementTextReplaceStringCaseMark.Data = "StringCaseMark";
            choiceActionItemElementTextReplaceStringCaseMark.Id = "StringCaseMark";
            this.ElementTextReplace.Items.Add(choiceActionItemElementTextReplaceStringCaseMark);

            
            //
            //Choice
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseMark.Caption = "Khớp hoa Khớp dấu";
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseMark.Data = "StringCaseMark";
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseMark.Id = "StringCaseMark";
            choiceActionItemElementTextReplaceStringCaseMark.Items.Add(choiceActionItemElementTextReplaceStringCaseMarkStringCaseMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseNoMark.Caption = "Khớp hoa Không dấu";
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseNoMark.Data = "StringCaseNoMark";
            choiceActionItemElementTextReplaceStringCaseMarkStringCaseNoMark.Id = "StringCaseNoMark";
            choiceActionItemElementTextReplaceStringCaseMark.Items.Add(choiceActionItemElementTextReplaceStringCaseMarkStringCaseNoMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseMark.Caption = "Tùy hoa khớp dấu";
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseMark.Data = "StringNoCaseMark";
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseMark.Id = "StringNoCaseMark";
            choiceActionItemElementTextReplaceStringCaseMark.Items.Add(choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseMark);
             
            //
            //Choice
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseNoMark.Caption = "Tùy hoa Không dấu";
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseNoMark.Data = "StringNoCaseNoMark";
            choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseNoMark.Id = "StringNoCaseNoMark";
            choiceActionItemElementTextReplaceStringCaseMark.Items.Add(choiceActionItemElementTextReplaceStringCaseMarkStringNoCaseNoMark);
             
            //
            //Root Choice
            choiceActionItemElementTextReplaceCharacterError.Caption = "Ký tự lỗi";
            choiceActionItemElementTextReplaceCharacterError.Data = "CharacterError";
            choiceActionItemElementTextReplaceCharacterError.Id = "CharacterError";
            this.ElementTextReplace.Items.Add(choiceActionItemElementTextReplaceCharacterError);

            
            //
            //Root Choice
            choiceActionItemElementTextReplaceDeleteSpeakerName.Caption = "Xóa người nói";
            choiceActionItemElementTextReplaceDeleteSpeakerName.Data = "DeleteSpeakerName";
            choiceActionItemElementTextReplaceDeleteSpeakerName.Id = "DeleteSpeakerName";
            this.ElementTextReplace.Items.Add(choiceActionItemElementTextReplaceDeleteSpeakerName);

            
            //
            //Root Choice
            choiceActionItemElementTextReplaceKeepOnlySpeakerName.Caption = "Giữ người nói";
            choiceActionItemElementTextReplaceKeepOnlySpeakerName.Data = "KeepOnlySpeakerName";
            choiceActionItemElementTextReplaceKeepOnlySpeakerName.Id = "KeepOnlySpeakerName";
            this.ElementTextReplace.Items.Add(choiceActionItemElementTextReplaceKeepOnlySpeakerName);

            this.ElementTextReplace.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ElementTextReplace_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.ElementTextReplace);
			// AlignElement
            this.AlignElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAlignElementAlignSubtitleBeginAlign = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAlignElementAlignSubtitleAudioAlign = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAlignElementAlignSubtitleEndAlign = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // AlignElement
            // 
            this.AlignElement.Caption = "Dịch chuyển";
            this.AlignElement.Category = "Edit";
            this.AlignElement.ConfirmationMessage = null;
            this.AlignElement.Id = "AlignElement";
            this.AlignElement.ImageName = "Action_AlignElement";
            this.AlignElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.AlignElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.AlignElement.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.AlignElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.AlignElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemAlignElementAlignSubtitleAudioAlign.Caption = "Đồng bộ âm";
            choiceActionItemAlignElementAlignSubtitleAudioAlign.Data = "AlignSubtitleAudioAlign";
            choiceActionItemAlignElementAlignSubtitleAudioAlign.Id = "AlignSubtitleAudioAlign";
            this.AlignElement.Items.Add(choiceActionItemAlignElementAlignSubtitleAudioAlign);

            
            //
            //Root Choice
            choiceActionItemAlignElementAlignSubtitleBeginAlign.Caption = "Tịnh tiến";
            choiceActionItemAlignElementAlignSubtitleBeginAlign.Data = "AlignSubtitleBeginAlign";
            choiceActionItemAlignElementAlignSubtitleBeginAlign.Id = "AlignSubtitleBeginAlign";
            this.AlignElement.Items.Add(choiceActionItemAlignElementAlignSubtitleBeginAlign);

            
            //
            //Root Choice
            choiceActionItemAlignElementAlignSubtitleEndAlign.Caption = "Tịnh lùi";
            choiceActionItemAlignElementAlignSubtitleEndAlign.Data = "AlignSubtitleEndAlign";
            choiceActionItemAlignElementAlignSubtitleEndAlign.Id = "AlignSubtitleEndAlign";
            this.AlignElement.Items.Add(choiceActionItemAlignElementAlignSubtitleEndAlign);

            this.AlignElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AlignElement_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.AlignElement);
			// TimestampByDuration
            this.TimestampByDuration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TimestampByDuration
            // 
            this.TimestampByDuration.Caption = "Chỉnh theo âm";
            this.TimestampByDuration.Category = "Edit";
            this.TimestampByDuration.ConfirmationMessage = null;
            this.TimestampByDuration.Id = "TimestampByDuration";
            this.TimestampByDuration.ImageName = "Action_TimestampByDuration";
            this.TimestampByDuration.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.TimestampByDuration.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TimestampByDuration.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.TimestampByDuration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.TimestampByDuration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TimestampByDuration_Execute);
            // 
            // AudioViewController
            // 
            this.Actions.Add(this.TimestampByDuration);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ElementFlag;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ElementTranslateSync;
		private DevExpress.ExpressApp.Actions.SimpleAction FindCaseType;
		private DevExpress.ExpressApp.Actions.SimpleAction PreviousNextElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MergeTwoElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ElementTextReplace;
		private DevExpress.ExpressApp.Actions.SimpleAction TimestampByDuration;
		private DevExpress.ExpressApp.Actions.SimpleAction AudioRecord;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UpperLowerElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction AlignElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ElementVoiceSpeed;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TextRate;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SplitElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TextQuantity;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ImportElementTerm;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MergeElement;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction SpellingAudio;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ConvertTo;
    }
}