namespace ENTOS.Module.Controllers
{
    partial class BookMarkViewController
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
			// UrlPasteLink
            this.UrlPasteLink = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteLinkWebLink = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteLinkImageLink = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UrlPasteLink
            // 
            this.UrlPasteLink.Caption = "Dán URL";
            this.UrlPasteLink.Category = "Edit";
            this.UrlPasteLink.ConfirmationMessage = null;
            this.UrlPasteLink.Id = "UrlPasteLink";
            this.UrlPasteLink.ImageName = "Action_UrlPasteLink";
            this.UrlPasteLink.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.UrlPasteLink.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UrlPasteLink.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;            
			this.UrlPasteLink.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.UrlPasteLink.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUrlPasteLinkWebLink.Caption = "Liên kết trang";
            choiceActionItemUrlPasteLinkWebLink.Data = "WebLink";
            choiceActionItemUrlPasteLinkWebLink.Id = "WebLink";
            this.UrlPasteLink.Items.Add(choiceActionItemUrlPasteLinkWebLink);

            
            //
            //Root Choice
            choiceActionItemUrlPasteLinkImageLink.Caption = "Liên kết ảnh";
            choiceActionItemUrlPasteLinkImageLink.Data = "ImageLink";
            choiceActionItemUrlPasteLinkImageLink.Id = "ImageLink";
            this.UrlPasteLink.Items.Add(choiceActionItemUrlPasteLinkImageLink);

            this.UrlPasteLink.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UrlPasteLink_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.UrlPasteLink);
			// FlagLink
            this.FlagLink = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkUpper = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkRaise = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkLower = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkClear = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkUpperAll = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkUrlNotFound = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkMultiFolder = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // FlagLink
            // 
            this.FlagLink.Caption = "Cờ";
            this.FlagLink.Category = "Edit";
            this.FlagLink.ConfirmationMessage = null;
            this.FlagLink.Id = "FlagLink";
            this.FlagLink.ImageName = "Action_FlagLink";
            this.FlagLink.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.FlagLink.TargetViewId = "Folder_BookMarkList_ListView";  
            this.FlagLink.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.FlagLink.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.FlagLink.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.FlagLink.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemFlagLinkUpper.Caption = "Đầu hoa";
            choiceActionItemFlagLinkUpper.Data = "Upper";
            choiceActionItemFlagLinkUpper.Id = "Upper";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkUpper);

            
            //
            //Root Choice
            choiceActionItemFlagLinkUpperAll.Caption = "Toàn hoa";
            choiceActionItemFlagLinkUpperAll.Data = "UpperAll";
            choiceActionItemFlagLinkUpperAll.Id = "UpperAll";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkUpperAll);

            
            //
            //Root Choice
            choiceActionItemFlagLinkLower.Caption = "Không hoa";
            choiceActionItemFlagLinkLower.Data = "Lower";
            choiceActionItemFlagLinkLower.Id = "Lower";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkLower);

            
            //
            //Root Choice
            choiceActionItemFlagLinkMultiFolder.Caption = "Đa thư mục";
            choiceActionItemFlagLinkMultiFolder.Data = "MultiFolder";
            choiceActionItemFlagLinkMultiFolder.Id = "MultiFolder";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkMultiFolder);

            
            //
            //Root Choice
            choiceActionItemFlagLinkRaise.Caption = "Dựng cờ";
            choiceActionItemFlagLinkRaise.Data = "Raise";
            choiceActionItemFlagLinkRaise.Id = "Raise";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkRaise);

            
            //
            //Root Choice
            choiceActionItemFlagLinkClear.Caption = "Xóa cờ";
            choiceActionItemFlagLinkClear.Data = "Clear";
            choiceActionItemFlagLinkClear.Id = "Clear";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkClear);

            
            //
            //Root Choice
            choiceActionItemFlagLinkUrlNotFound.Caption = "Không tồn tại";
            choiceActionItemFlagLinkUrlNotFound.Data = "UrlNotFound";
            choiceActionItemFlagLinkUrlNotFound.Id = "UrlNotFound";
            this.FlagLink.Items.Add(choiceActionItemFlagLinkUrlNotFound);

            this.FlagLink.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FlagLink_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.FlagLink);
			// QuantityFunction
            this.QuantityFunction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityFunctionSum = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityFunctionDuration = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityFunctionNameLength = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // QuantityFunction
            // 
            this.QuantityFunction.Caption = "Đếm";
            this.QuantityFunction.Category = "Edit";
            this.QuantityFunction.ConfirmationMessage = null;
            this.QuantityFunction.Id = "QuantityFunction";
            this.QuantityFunction.ImageName = "Action_QuantityFunction";
            this.QuantityFunction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.QuantityFunction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.QuantityFunction.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.QuantityFunction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.QuantityFunction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemQuantityFunctionNameLength.Caption = "Độ dài tên";
            choiceActionItemQuantityFunctionNameLength.Data = "NameLength";
            choiceActionItemQuantityFunctionNameLength.Id = "NameLength";
            this.QuantityFunction.Items.Add(choiceActionItemQuantityFunctionNameLength);

            
            //
            //Root Choice
            choiceActionItemQuantityFunctionDuration.Caption = "Thời lượng";
            choiceActionItemQuantityFunctionDuration.Data = "Duration";
            choiceActionItemQuantityFunctionDuration.Id = "Duration";
            this.QuantityFunction.Items.Add(choiceActionItemQuantityFunctionDuration);

            
            //
            //Root Choice
            choiceActionItemQuantityFunctionSum.Caption = "Tổng từ thành phần";
            choiceActionItemQuantityFunctionSum.Data = "Sum";
            choiceActionItemQuantityFunctionSum.Id = "Sum";
            this.QuantityFunction.Items.Add(choiceActionItemQuantityFunctionSum);

            this.QuantityFunction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.QuantityFunction_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.QuantityFunction);
			// ObjectSearch
            this.ObjectSearch = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ObjectSearch
            // 
            this.ObjectSearch.Caption = "Tìm đối tượng";
            this.ObjectSearch.Category = "Edit";
            this.ObjectSearch.ConfirmationMessage = null;
            this.ObjectSearch.Id = "ObjectSearch";
            this.ObjectSearch.ImageName = "Action_ObjectSearch";
            this.ObjectSearch.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ObjectSearch.TargetViewId = "Recognition_BookMarkList_ListView";  
            this.ObjectSearch.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.ObjectSearch.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ObjectSearch.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ObjectSearch.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ObjectSearch_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.ObjectSearch);
			// LinkVoiceSpeed
            this.LinkVoiceSpeed = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemLinkVoiceSpeedAverage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // LinkVoiceSpeed
            // 
            this.LinkVoiceSpeed.Caption = "Nạp tốc độ";
            this.LinkVoiceSpeed.Category = "Edit";
            this.LinkVoiceSpeed.ConfirmationMessage = null;
            this.LinkVoiceSpeed.Id = "LinkVoiceSpeed";
            this.LinkVoiceSpeed.ImageName = "Action_LinkVoiceSpeed";
            this.LinkVoiceSpeed.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.LinkVoiceSpeed.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.LinkVoiceSpeed.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.LinkVoiceSpeed.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.LinkVoiceSpeed.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemLinkVoiceSpeedAverage.Caption = "Trung bình";
            choiceActionItemLinkVoiceSpeedAverage.Data = "Average";
            choiceActionItemLinkVoiceSpeedAverage.Id = "Average";
            this.LinkVoiceSpeed.Items.Add(choiceActionItemLinkVoiceSpeedAverage);

            this.LinkVoiceSpeed.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.LinkVoiceSpeed_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.LinkVoiceSpeed);
			// Detection
            this.Detection = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDetectionFace = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDetectionCar = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDetectionProduct = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDetectionNumberPlate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDetectionMotorcycle = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // Detection
            // 
            this.Detection.Caption = "Nhận dạng";
            this.Detection.Category = "Edit";
            this.Detection.ConfirmationMessage = null;
            this.Detection.Id = "Detection";
            this.Detection.ImageName = "Action_Detection";
            this.Detection.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.Detection.TargetViewId = "Recognition_BookMarkList_ListView";  
            this.Detection.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.Detection.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.Detection.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.Detection.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemDetectionFace.Caption = "Khuôn mặt";
            choiceActionItemDetectionFace.Data = "Face";
            choiceActionItemDetectionFace.Id = "Face";
            this.Detection.Items.Add(choiceActionItemDetectionFace);

            
            //
            //Root Choice
            choiceActionItemDetectionNumberPlate.Caption = "Biển số";
            choiceActionItemDetectionNumberPlate.Data = "NumberPlate";
            choiceActionItemDetectionNumberPlate.Id = "NumberPlate";
            this.Detection.Items.Add(choiceActionItemDetectionNumberPlate);

            
            //
            //Root Choice
            choiceActionItemDetectionCar.Caption = "Ô tô";
            choiceActionItemDetectionCar.Data = "Car";
            choiceActionItemDetectionCar.Id = "Car";
            this.Detection.Items.Add(choiceActionItemDetectionCar);

            
            //
            //Root Choice
            choiceActionItemDetectionMotorcycle.Caption = "Xe máy";
            choiceActionItemDetectionMotorcycle.Data = "Motorcycle";
            choiceActionItemDetectionMotorcycle.Id = "Motorcycle";
            this.Detection.Items.Add(choiceActionItemDetectionMotorcycle);

            
            //
            //Root Choice
            choiceActionItemDetectionProduct.Caption = "Sản phẩm";
            choiceActionItemDetectionProduct.Data = "Product";
            choiceActionItemDetectionProduct.Id = "Product";
            this.Detection.Items.Add(choiceActionItemDetectionProduct);

            this.Detection.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Detection_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.Detection);
			// FlagLinkVideo
            this.FlagLinkVideo = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemFlagLinkVideoSubtitle = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // FlagLinkVideo
            // 
            this.FlagLinkVideo.Caption = "Cờ";
            this.FlagLinkVideo.Category = "Edit";
            this.FlagLinkVideo.ConfirmationMessage = null;
            this.FlagLinkVideo.Id = "FlagLinkVideo";
            this.FlagLinkVideo.ImageName = "Action_FlagLinkVideo";
            this.FlagLinkVideo.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.FlagLinkVideo.TargetViewId = "Video_FileList_ListView";  
            this.FlagLinkVideo.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.FlagLinkVideo.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.FlagLinkVideo.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.FlagLinkVideo.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemFlagLinkVideoSubtitle.Caption = "Có phụ đề";
            choiceActionItemFlagLinkVideoSubtitle.Data = "Subtitle";
            choiceActionItemFlagLinkVideoSubtitle.Id = "Subtitle";
            this.FlagLinkVideo.Items.Add(choiceActionItemFlagLinkVideoSubtitle);

            this.FlagLinkVideo.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FlagLinkVideo_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.FlagLinkVideo);
			// PostContentImport
            this.PostContentImport = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemPostContentImportNote = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemPostContentImportAddress = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // PostContentImport
            // 
            this.PostContentImport.Caption = "Nạp Nội dung Bài viết";
            this.PostContentImport.Category = "Edit";
            this.PostContentImport.ConfirmationMessage = null;
            this.PostContentImport.Id = "PostContentImport";
            this.PostContentImport.ImageName = "Action_PostContentImport";
            this.PostContentImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.PostContentImport.TargetViewId = "Post_BookMarkList_ListView";  
            this.PostContentImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.PostContentImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.PostContentImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.PostContentImport.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemPostContentImportAddress.Caption = "Từ liên kết";
            choiceActionItemPostContentImportAddress.Data = "Address";
            choiceActionItemPostContentImportAddress.Id = "Address";
            this.PostContentImport.Items.Add(choiceActionItemPostContentImportAddress);

            
            //
            //Root Choice
            choiceActionItemPostContentImportNote.Caption = "Từ liên kết phụ";
            choiceActionItemPostContentImportNote.Data = "Note";
            choiceActionItemPostContentImportNote.Id = "Note";
            this.PostContentImport.Items.Add(choiceActionItemPostContentImportNote);

            this.PostContentImport.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.PostContentImport_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.PostContentImport);
			// LinkNoteSync
            this.LinkNoteSync = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LinkNoteSync
            // 
            this.LinkNoteSync.Caption = "Đồng bộ";
            this.LinkNoteSync.Category = "Edit";
            this.LinkNoteSync.ConfirmationMessage = null;
            this.LinkNoteSync.Id = "LinkNoteSync";
            this.LinkNoteSync.ImageName = "Action_LinkNoteSync";
            this.LinkNoteSync.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.LinkNoteSync.TargetViewId = "Video_FileList_ListView";  
            this.LinkNoteSync.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.LinkNoteSync.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.LinkNoteSync.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.LinkNoteSync.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LinkNoteSync_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.LinkNoteSync);
			// DataChatbotAI
            this.DataChatbotAI = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDataChatbotAIObjectCreate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDataChatbotAIDataExtract = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // DataChatbotAI
            // 
            this.DataChatbotAI.Caption = "Trích AI";
            this.DataChatbotAI.Category = "Edit";
            this.DataChatbotAI.ConfirmationMessage = null;
            this.DataChatbotAI.Id = "DataChatbotAI";
            this.DataChatbotAI.ImageName = "Action_DataChatbotAI";
            this.DataChatbotAI.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.DataChatbotAI.ToolTip = "Chatbot trích dữ liệu trang web để tạo đối tượng";  
			
			this.DataChatbotAI.TargetViewId = "AIExtractor_BookMarkList_ListView";  
            this.DataChatbotAI.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.DataChatbotAI.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.DataChatbotAI.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.DataChatbotAI.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemDataChatbotAIDataExtract.Caption = "Lấy dữ liệu";
            choiceActionItemDataChatbotAIDataExtract.Data = "DataExtract";
            choiceActionItemDataChatbotAIDataExtract.Id = "DataExtract";
            this.DataChatbotAI.Items.Add(choiceActionItemDataChatbotAIDataExtract);

            
            //
            //Root Choice
            choiceActionItemDataChatbotAIObjectCreate.Caption = "Tạo đối tượng";
            choiceActionItemDataChatbotAIObjectCreate.Data = "ObjectCreate";
            choiceActionItemDataChatbotAIObjectCreate.Id = "ObjectCreate";
            this.DataChatbotAI.Items.Add(choiceActionItemDataChatbotAIObjectCreate);

            this.DataChatbotAI.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DataChatbotAI_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.DataChatbotAI);
			// ObjectMatchingLink
            this.ObjectMatchingLink = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkOrg = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkOrgOrgContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkOrgOrgName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkProduct = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkProductProductName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkProductProductContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkContact = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkContactContactName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectMatchingLinkContactContactContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ObjectMatchingLink
            // 
            this.ObjectMatchingLink.Caption = "Khớp đối tượng";
            this.ObjectMatchingLink.Category = "Edit";
            this.ObjectMatchingLink.ConfirmationMessage = null;
            this.ObjectMatchingLink.Id = "ObjectMatchingLink";
            this.ObjectMatchingLink.ImageName = "Action_ObjectMatchingLink";
            this.ObjectMatchingLink.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ObjectMatchingLink.TargetViewId = "Folder_BookMarkList_ListView";  
            this.ObjectMatchingLink.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ObjectMatchingLink.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ObjectMatchingLink.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ObjectMatchingLink.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemObjectMatchingLinkProduct.Caption = "Sản phẩm";
            choiceActionItemObjectMatchingLinkProduct.Data = "Product";
            choiceActionItemObjectMatchingLinkProduct.Id = "Product";
            this.ObjectMatchingLink.Items.Add(choiceActionItemObjectMatchingLinkProduct);

            
            //
            //Choice
            choiceActionItemObjectMatchingLinkProductProductName.Caption = "Tên";
            choiceActionItemObjectMatchingLinkProductProductName.Data = "ProductName";
            choiceActionItemObjectMatchingLinkProductProductName.Id = "ProductName";
            choiceActionItemObjectMatchingLinkProduct.Items.Add(choiceActionItemObjectMatchingLinkProductProductName);
             
            //
            //Choice
            choiceActionItemObjectMatchingLinkProductProductContent.Caption = "Nội dung";
            choiceActionItemObjectMatchingLinkProductProductContent.Data = "ProductContent";
            choiceActionItemObjectMatchingLinkProductProductContent.Id = "ProductContent";
            choiceActionItemObjectMatchingLinkProduct.Items.Add(choiceActionItemObjectMatchingLinkProductProductContent);
             
            //
            //Root Choice
            choiceActionItemObjectMatchingLinkContact.Caption = "Nhân vật";
            choiceActionItemObjectMatchingLinkContact.Data = "Contact";
            choiceActionItemObjectMatchingLinkContact.Id = "Contact";
            this.ObjectMatchingLink.Items.Add(choiceActionItemObjectMatchingLinkContact);

            
            //
            //Choice
            choiceActionItemObjectMatchingLinkContactContactName.Caption = "Tên";
            choiceActionItemObjectMatchingLinkContactContactName.Data = "ContactName";
            choiceActionItemObjectMatchingLinkContactContactName.Id = "ContactName";
            choiceActionItemObjectMatchingLinkContact.Items.Add(choiceActionItemObjectMatchingLinkContactContactName);
             
            //
            //Choice
            choiceActionItemObjectMatchingLinkContactContactContent.Caption = "Nội dung";
            choiceActionItemObjectMatchingLinkContactContactContent.Data = "ContactContent";
            choiceActionItemObjectMatchingLinkContactContactContent.Id = "ContactContent";
            choiceActionItemObjectMatchingLinkContact.Items.Add(choiceActionItemObjectMatchingLinkContactContactContent);
             
            //
            //Root Choice
            choiceActionItemObjectMatchingLinkOrg.Caption = "Tổ chức";
            choiceActionItemObjectMatchingLinkOrg.Data = "Org";
            choiceActionItemObjectMatchingLinkOrg.Id = "Org";
            this.ObjectMatchingLink.Items.Add(choiceActionItemObjectMatchingLinkOrg);

            
            //
            //Choice
            choiceActionItemObjectMatchingLinkOrgOrgName.Caption = "Tên";
            choiceActionItemObjectMatchingLinkOrgOrgName.Data = "OrgName";
            choiceActionItemObjectMatchingLinkOrgOrgName.Id = "OrgName";
            choiceActionItemObjectMatchingLinkOrg.Items.Add(choiceActionItemObjectMatchingLinkOrgOrgName);
             
            //
            //Choice
            choiceActionItemObjectMatchingLinkOrgOrgContent.Caption = "Nội dung";
            choiceActionItemObjectMatchingLinkOrgOrgContent.Data = "OrgContent";
            choiceActionItemObjectMatchingLinkOrgOrgContent.Id = "OrgContent";
            choiceActionItemObjectMatchingLinkOrg.Items.Add(choiceActionItemObjectMatchingLinkOrgOrgContent);
             this.ObjectMatchingLink.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ObjectMatchingLink_Execute);
            // 
            // BookMarkViewController
            // 
            this.Actions.Add(this.ObjectMatchingLink);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UrlPasteLink;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction FlagLink;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction QuantityFunction;
		private DevExpress.ExpressApp.Actions.SimpleAction ObjectSearch;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction LinkVoiceSpeed;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction Detection;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction FlagLinkVideo;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction PostContentImport;
		private DevExpress.ExpressApp.Actions.SimpleAction LinkNoteSync;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction DataChatbotAI;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ObjectMatchingLink;
    }
}