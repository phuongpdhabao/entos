namespace ENTOS.Module.Controllers
{
    partial class WebExtractorViewController
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
			// UrlPasteWebExtractor
            this.UrlPasteWebExtractor = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteWebExtractorUrl = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteWebExtractorImage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteWebExtractorSearchImage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUrlPasteWebExtractorSearchPage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UrlPasteWebExtractor
            // 
            this.UrlPasteWebExtractor.Caption = "Dán URL";
            this.UrlPasteWebExtractor.Category = "Edit";
            this.UrlPasteWebExtractor.ConfirmationMessage = null;
            this.UrlPasteWebExtractor.Id = "UrlPasteWebExtractor";
            this.UrlPasteWebExtractor.ImageName = "Action_UrlPasteWebExtractor";
            this.UrlPasteWebExtractor.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.UrlPasteWebExtractor.ToolTip = "Lấy danh sách URL từ Clipboard hoặc kết quả tìm từ Google";  
            this.UrlPasteWebExtractor.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UrlPasteWebExtractor.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;            
			this.UrlPasteWebExtractor.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.UrlPasteWebExtractor.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUrlPasteWebExtractorUrl.Caption = "Liên kết trang";
            choiceActionItemUrlPasteWebExtractorUrl.Data = "Url";
            choiceActionItemUrlPasteWebExtractorUrl.Id = "Url";
            this.UrlPasteWebExtractor.Items.Add(choiceActionItemUrlPasteWebExtractorUrl);

            
            //
            //Root Choice
            choiceActionItemUrlPasteWebExtractorImage.Caption = "Liên kết ảnh";
            choiceActionItemUrlPasteWebExtractorImage.Data = "Image";
            choiceActionItemUrlPasteWebExtractorImage.Id = "Image";
            this.UrlPasteWebExtractor.Items.Add(choiceActionItemUrlPasteWebExtractorImage);

            
            //
            //Root Choice
            choiceActionItemUrlPasteWebExtractorSearchPage.Caption = "Tìm trang";
            choiceActionItemUrlPasteWebExtractorSearchPage.Data = "SearchPage";
            choiceActionItemUrlPasteWebExtractorSearchPage.Id = "SearchPage";
            this.UrlPasteWebExtractor.Items.Add(choiceActionItemUrlPasteWebExtractorSearchPage);

            
            //
            //Root Choice
            choiceActionItemUrlPasteWebExtractorSearchImage.Caption = "Tìm ảnh";
            choiceActionItemUrlPasteWebExtractorSearchImage.Data = "SearchImage";
            choiceActionItemUrlPasteWebExtractorSearchImage.Id = "SearchImage";
            this.UrlPasteWebExtractor.Items.Add(choiceActionItemUrlPasteWebExtractorSearchImage);

            this.UrlPasteWebExtractor.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UrlPasteWebExtractor_Execute);
            // 
            // WebExtractorViewController
            // 
            this.Actions.Add(this.UrlPasteWebExtractor);
			// WebExtractorResult
            this.WebExtractorResult = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultGet = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultGetLoginDownloadPdf = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultGetLoginDownloadImage = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultGetGet = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultGetLoginFileDownload = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultDomainName = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultQuick = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultQuickFileDownload = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWebExtractorResultQuickQuick = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // WebExtractorResult
            // 
            this.WebExtractorResult.Caption = "Dữ liệu";
            this.WebExtractorResult.Category = "Edit";
            this.WebExtractorResult.ConfirmationMessage = null;
            this.WebExtractorResult.Id = "WebExtractorResult";
            this.WebExtractorResult.ImageName = "Action_WebExtractorResult";
            this.WebExtractorResult.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.WebExtractorResult.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.WebExtractorResult.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;            
			this.WebExtractorResult.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.WebExtractorResult.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemWebExtractorResultQuick.Caption = "Trực tiếp";
            choiceActionItemWebExtractorResultQuick.Data = "Quick";
            choiceActionItemWebExtractorResultQuick.Id = "Quick";
            this.WebExtractorResult.Items.Add(choiceActionItemWebExtractorResultQuick);

            
            //
            //Choice
            choiceActionItemWebExtractorResultQuickQuick.Caption = "Lấy dữ liệu";
            choiceActionItemWebExtractorResultQuickQuick.Data = "Quick";
            choiceActionItemWebExtractorResultQuickQuick.Id = "Quick";
            choiceActionItemWebExtractorResultQuick.Items.Add(choiceActionItemWebExtractorResultQuickQuick);
             
            //
            //Choice
            choiceActionItemWebExtractorResultQuickFileDownload.Caption = "Tệp không login";
            choiceActionItemWebExtractorResultQuickFileDownload.Data = "FileDownload";
            choiceActionItemWebExtractorResultQuickFileDownload.Id = "FileDownload";
            choiceActionItemWebExtractorResultQuick.Items.Add(choiceActionItemWebExtractorResultQuickFileDownload);
             
            //
            //Root Choice
            choiceActionItemWebExtractorResultGet.Caption = "Qua Chrome";
            choiceActionItemWebExtractorResultGet.Data = "Get";
            choiceActionItemWebExtractorResultGet.Id = "Get";
            this.WebExtractorResult.Items.Add(choiceActionItemWebExtractorResultGet);

            
            //
            //Choice
            choiceActionItemWebExtractorResultGetGet.Caption = "Lấy dữ liệu";
            choiceActionItemWebExtractorResultGetGet.Data = "Get";
            choiceActionItemWebExtractorResultGetGet.Id = "Get";
            choiceActionItemWebExtractorResultGet.Items.Add(choiceActionItemWebExtractorResultGetGet);
             
            //
            //Choice
            choiceActionItemWebExtractorResultGetLoginDownloadImage.Caption = "Ảnh";
            choiceActionItemWebExtractorResultGetLoginDownloadImage.Data = "LoginDownloadImage";
            choiceActionItemWebExtractorResultGetLoginDownloadImage.Id = "LoginDownloadImage";
            choiceActionItemWebExtractorResultGet.Items.Add(choiceActionItemWebExtractorResultGetLoginDownloadImage);
             
            //
            //Choice
            choiceActionItemWebExtractorResultGetLoginDownloadPdf.Caption = "Pdf";
            choiceActionItemWebExtractorResultGetLoginDownloadPdf.Data = "LoginDownloadPdf";
            choiceActionItemWebExtractorResultGetLoginDownloadPdf.Id = "LoginDownloadPdf";
            choiceActionItemWebExtractorResultGet.Items.Add(choiceActionItemWebExtractorResultGetLoginDownloadPdf);
             
            //
            //Choice
            choiceActionItemWebExtractorResultGetLoginFileDownload.Caption = "Têp khác";
            choiceActionItemWebExtractorResultGetLoginFileDownload.Data = "LoginFileDownload";
            choiceActionItemWebExtractorResultGetLoginFileDownload.Id = "LoginFileDownload";
            choiceActionItemWebExtractorResultGet.Items.Add(choiceActionItemWebExtractorResultGetLoginFileDownload);
             
            //
            //Root Choice
            choiceActionItemWebExtractorResultOpen.Caption = "Mở kết quả";
            choiceActionItemWebExtractorResultOpen.Data = "Open";
            choiceActionItemWebExtractorResultOpen.Id = "Open";
            this.WebExtractorResult.Items.Add(choiceActionItemWebExtractorResultOpen);

            
            //
            //Root Choice
            choiceActionItemWebExtractorResultDomainName.Caption = "Copy tên miền";
            choiceActionItemWebExtractorResultDomainName.Data = "DomainName";
            choiceActionItemWebExtractorResultDomainName.Id = "DomainName";
            this.WebExtractorResult.Items.Add(choiceActionItemWebExtractorResultDomainName);

            this.WebExtractorResult.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.WebExtractorResult_Execute);
            // 
            // WebExtractorViewController
            // 
            this.Actions.Add(this.WebExtractorResult);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UrlPasteWebExtractor;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction WebExtractorResult;
    }
}