using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Helpers;
using ENTOS.Module.Extensions;
using ENTOS.Module.SystemServices;
using ENTOS.Module.Services;


 
namespace ENTOS.Module.Services 
{

    public partial class VideoService : BaseService
    {

        public VideoService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public VideoService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4544ImportCode
                        public void ExportMedia(Video video, System.Collections.Generic.Dictionary<BookMark, System.Collections.Generic.List<Media>> bookMarkList, string saveFolder)
        {
            foreach (var bookMark in bookMarkList.Keys)
            {
                if (string.IsNullOrEmpty(bookMark.URL))
                    continue;
                string fileName = bookMark.URL;
                var fileInfo = new System.IO.FileInfo(fileName);
                if (fileInfo.Extension.ToLower() == ".doc" || fileInfo.Extension.ToLower() == ".docx")
                {
                    if (!System.IO.File.Exists(fileName))
                    {
                        _notificationService.NotifyError("Lỗi", $"Không tồn tại tập tin{fileName}");
                        continue;
                    }

                    var saveFile = Module.Helpers.NameHelper.GetUniqueFileName(saveFolder + "\\" + fileInfo.Name);
                    ExportMedia(video, fileInfo, bookMark, saveFile, bookMarkList[bookMark]);
                    if (bookMark != null)
                        bookMark.Note = saveFile;
                }
            }
        }
        public void ExportMedia(Video video, System.IO.FileInfo fileInfo, BookMark bookmark, string saveFile, System.Collections.Generic.List<Media> listedMedia = null)
        {
            bool replaceOverlap = false;
            using (DevExpress.XtraRichEdit.RichEditDocumentServer wordProcessor = new DevExpress.XtraRichEdit.RichEditDocumentServer())
            {
                wordProcessor.LoadDocument(fileInfo.FullName);
                var nonExportMedia = new System.Collections.Generic.List<Media>();
                var childItemsUngroup = new System.Collections.Generic.List<int>();
                var shapeList = wordProcessor.Document.Shapes.ToList();

                foreach (var media in listedMedia)
                {
                    if (nonExportMedia.Contains(media) || media.Start is null)
                        continue;
                    var indexMedia = System.Convert.ToInt32(media.Start.Value.TotalSeconds) - 1;
                    if (shapeList.Count <= indexMedia)
                        continue;
                    var currentShape = shapeList[indexMedia];

                    if (media.MediaType == MediaType.UnGroup)
                    {
                        if (shapeList[indexMedia].Type == DevExpress.XtraRichEdit.API.Native.ShapeType.Group)
                        {
                            var items = currentShape.GroupItems.ToList();
                            foreach (var shapeItem in items)
                            {
                                childItemsUngroup.Add(shapeItem.Id);
                                if (!replaceOverlap)
                                    replaceOverlap = true;
                                shapeItem.Offset = new System.Drawing.PointF(shapeItem.Offset.X + currentShape.Offset.X, shapeItem.Offset.Y + currentShape.Offset.Y);
                            }
                            var textWrapping = currentShape.TextWrapping;

                            currentShape.GroupItems.Ungroup();

                            foreach (var shapeItem in items)
                            {
                                if (shapeItem is DevExpress.XtraRichEdit.API.Native.Shape)
                                {
                                    var shape = (DevExpress.XtraRichEdit.API.Native.Shape)shapeItem;
                                    shape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.Column;
                                    shape.RelativeVerticalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Paragraph;
                                    if (shape.TextWrapping != textWrapping)
                                        shape.TextWrapping = textWrapping;
                                }
                                //else if (shapeItem is DevExpress.XtraRichEdit.API.Native.DrawingObject)
                                else
                                {
                                    foreach (var shape in wordProcessor.Document.Shapes)
                                    {
                                        if (shapeItem.Id == shape.Id)
                                        {
                                            shape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.Column;
                                            shape.RelativeVerticalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Paragraph;
                                            shape.RelativeHorizontalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalSize.Margin;
                                            shape.RelativeVerticalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalSize.Margin;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else if (media.End != null)
                    {
                        //wordProcessor.Document.Shapes.Gr


                        var newGroup = wordProcessor.Document.Shapes.InsertGroup(currentShape.Range.Start);
                        newGroup.TextWrapping = currentShape.TextWrapping;
                        newGroup.Offset = currentShape.Offset;
                        //newGroup.GroupItems.AddShape(DevExpress.XtraRichEdit.API.Native.ShapeGeometryPreset.Custom, currentShape.Range.Start);
                        var newitem = newGroup.GroupItems.AddGroup();

                        var nested = currentShape.GroupItems.AddGroup();
                        //var itemsIngroup = media.Video.MediaList.Where(x => x != media && x.End == media.End).ToList();
                        //nonExportMedia.AddRange(itemsIngroup);
                        //foreach (var itemInGroup in itemsIngroup)
                        //{

                        //}  
                    }
                    else if (media != null)
                    {
                        //if (media.TextWrappingType != currentShape.TextWrapping)
                        //    currentShape.TextWrapping = media.TextWrappingType;

                        //if (media.Width != Convert.ToDecimal(currentShape.Size.Width) ||
                        //    media.Height != Convert.ToDecimal(currentShape.Size.Height))
                        //    currentShape.Size = new System.Drawing.SizeF(Convert.ToSingle(media.Width), Convert.ToSingle(media.Height));
                        //if (media.Alignment == BusinessObjects.Alignment.Left)
                        //    currentShape.HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Left;
                        //else if (media.Alignment == BusinessObjects.Alignment.Centered)
                        //    currentShape.HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Center;
                        //else if (media.Alignment == BusinessObjects.Alignment.Right)
                        //    currentShape.HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Right;
                        ////Không hỗ trợ
                        ////else if (media.Alignment == BusinessObjects.Alignment.Justified)
                        ////    currentShape.HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Inside;

                        //if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.Margin)
                        //    currentShape.RelativeHorizontalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalSize.Margin;
                        //else if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.Page)
                        //    currentShape.RelativeHorizontalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalSize.Page;
                        ////else if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.Column)
                        ////    currentShape.RelativeHorizontalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalSize.Column;
                        ////else if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.Character)
                        ////    currentShape.RelativeHorizontalSize = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalSize.Character;
                        //if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.LeftMargin)
                        //    currentShape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.LeftMargin;
                        //else if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.RightMargin)
                        //    currentShape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.RightMargin;
                        //else if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.InsideMargin)
                        //    currentShape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.InsideMargin;
                        //if (media.AlignmentRelative == BusinessObjects.AlignmentRelative.OutsideMargin)
                        //    currentShape.RelativeHorizontalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition.OutsideMargin;

                        if (media.MoveWithText)
                        {
                            if (currentShape.RelativeVerticalPosition != DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Line ||
                                currentShape.RelativeVerticalPosition != DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Paragraph)
                                currentShape.RelativeVerticalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Paragraph;
                        }
                        else
                        {
                            if (currentShape.RelativeVerticalPosition == DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Line ||
                                currentShape.RelativeVerticalPosition == DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Paragraph)
                                currentShape.RelativeVerticalPosition = DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition.Page;
                        }
                    }
                }
                if (replaceOverlap)
                {

                    var tempFile = Module.Helpers.NameHelper.GetUniqueFileName(System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileInfo.Name));

                    wordProcessor.SaveDocument(tempFile, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

                    if (System.IO.File.Exists(tempFile))
                    {
                        ReplaceAllowOverlapInDoc(video, new System.IO.FileInfo(tempFile), childItemsUngroup, "allowOverlap", "1", saveFile);
                    }
                }
                else
                {
                    //Save luôn
                    wordProcessor.SaveDocument(saveFile, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                }

            }

        }


        public string GetVideoTempFolder(Video video, bool currentDirectory = true)
        {
            var tempFolder = currentDirectory ? System.IO.Path.Join(System.IO.Directory.GetCurrentDirectory(), "Temp") : System.IO.Path.GetTempPath();
            tempFolder = System.IO.Path.Join(tempFolder, video.Oid.ToString().Substring(0, 10));
            if (!System.IO.Directory.Exists(tempFolder))
                System.IO.Directory.CreateDirectory(tempFolder);
            return tempFolder;
        }
        public void ReplaceAllowOverlapInDoc(Video video, System.IO.FileInfo fileInfo, System.Collections.Generic.List<int> idList, string attributeName, string newAttributeValue, string saveFile)
        {
            var tempFolder = GetVideoTempFolder(video);
            // var fileName = System.IO.Path.Join(tempFolder, fileInfo.Name);
            Module.SystemObjects.Tools.ZipFileExtractToDirectory(fileInfo.FullName, tempFolder, true);
            var xmlFile = System.IO.Path.Join(tempFolder, "word", "document.xml");

            if (System.IO.File.Exists(xmlFile))
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(xmlFile);
                ReplaceAllowOverlapInDoc(doc, idList, "allowOverlap", "1");
                doc.Save(xmlFile);
                //Xóa file đã có
                if (System.IO.File.Exists(saveFile))
                {
                    System.IO.File.Delete(saveFile);
                }
                System.IO.Compression.ZipFile.CreateFromDirectory(tempFolder, saveFile);
            }
        }


        public void ReplaceAllowOverlapInDoc(System.Xml.XmlDocument doc, System.Collections.Generic.List<int> idList, string attributeName, string newAttributeValue)
        {
            if (idList?.Count > 0 && doc != null)
            {
                foreach (var id in idList)
                {
                    System.Xml.XmlNodeList nodes = doc.DocumentElement.SelectNodes($"//*[@id='{id.ToString("D")}']");
                    foreach (System.Xml.XmlNode wpDocPrNode in nodes)
                    {
                        if (wpDocPrNode.Name == "wp:docPr")
                        {
                            if (wpDocPrNode.ParentNode != null && wpDocPrNode.ParentNode.Name == "wp:anchor")
                            {
                                var allowOverlap = wpDocPrNode.ParentNode.Attributes[attributeName];
                                if (allowOverlap != null)
                                {
                                    allowOverlap.Value = newAttributeValue;
                                }
                                else
                                {
                                    var newAttribute = doc.CreateAttribute(attributeName);
                                    newAttribute.Value = newAttributeValue;
                                    wpDocPrNode.Attributes.Append(newAttribute);
                                }
                            }
                        }
                    }
                }


            }
        }
        //public System.Collections.Generic.Dictionary<string, string> FillExportFileList(Video video, System.Collections.Generic.Dictionary<string, BookMark> listBookMark, string saveFolder, string choice)
        //{
        //    var listFile = new System.Collections.Generic.Dictionary<string, string>();
        //    foreach (var url in listBookMark.Keys)
        //    {
        //        var fileName = url;
        //        if (fileName.EndsWith("/"))
        //            fileName = fileName.Substring(0, fileName.Length - 1);
        //        if (listFile.ContainsKey(fileName))
        //            continue;
        //        string name = "";
        //        if (choice == "Subtitle")
        //        {
        //            if (Module.Utils.YouTubeUtils.IsYoutubeUrl(url))
        //            {
        //                //Xuất dịch hoặc nội dung từ youtube sẽ ra file srt
        //                if (!string.IsNullOrEmpty(listBookMark[url].Name))
        //                    name = listBookMark[url].Name + ".srt";
        //            }else if(fileName.EndsWith(".srt"))
        //            {
        //                name = System.IO.Path.GetFileName(fileName);
        //            }
        //        }
        //        else if (choice == "TranslateDocument" || (choice == "ContentDocument"))
        //        {

        //        }                
        //        else if (choice == "Audio" || choice == "Video")
        //        {

        //        }

        //        if (Module.Utils.YouTubeUtils.IsYoutubeUrl(url))
        //        {
        //            //Xuất dịch hoặc nội dung từ youtube sẽ ra file srt
        //            if (!string.IsNullOrEmpty(listBookMark[url].Name))
        //                name = listBookMark[url].Name + ".srt";
        //        }
        //        if (fileName.StartsWith("http") || fileName.StartsWith("www"))
        //        {
        //            System.Uri myUri = new System.Uri(fileName);
        //            if (!string.IsNullOrEmpty(myUri.Query))
        //                fileName = fileName.Replace(myUri.Query, "");
        //        }


        //        var fileInfo = new System.IO.FileInfo(fileName);
        //        if (string.IsNullOrEmpty(name))
        //        {
        //            name = fileInfo.Name;
        //            if (string.IsNullOrEmpty(fileInfo.Extension) && fileName.StartsWith("http"))
        //            {
        //                name += ".html";
        //            }
        //        }
        //        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        //        {
        //            name = name.Replace(c, '_');
        //        }
        //        var saveFile = Module.Helpers.NameHelper.GetUniqueFileName(saveFolder + "\\" + name);
        //        listFile.Add(fileName, saveFile);
        //    }
        //    if (listFile.Count == 0)
        //    {
        //        Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Lỗi", $"Liên kết được chọn không hỗ trợ chức năng này", InformationType.Error);
        //        return null;
        //    }
        //    return listFile;
        //}

        // Export
        //private int order = 0;
        private void GetUnionStyle(ParagraphStyle paragraphStyle, System.Collections.Generic.List<Guid> paragraphStyleUnion)
        {
            if (paragraphStyle is null || paragraphStyleUnion.Contains(paragraphStyle.Oid))
                return;
            paragraphStyleUnion.Add(paragraphStyle.Oid);
            GetUnionStyle(paragraphStyle.UpperStyle, paragraphStyleUnion);
        }

        public void ExportTranslateDocument(Video video, System.Collections.Generic.Dictionary<string, string> listFile, System.Collections.Generic.Dictionary<string, BookMark> listBookMark, string choice = "TranslateDocument")
        {
            int index = 1;
            int fileIndex = 1;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            //string xpath = GetXPathFromClipboard();
            string xpath = CheckTextIsXpath(video.Path) ? video.Path : null;
            foreach (var fileName in listFile)
            {
                BookMark bookMark = listBookMark.ContainsKey(fileName.Key) ? listBookMark[fileName.Key] : null;
                var fileInfo = new System.IO.FileInfo(fileName.Key);
                //Trường hợp file srt
                //048: nếu Địa chỉ là MP3 hoặc SRT thì xuất SRT
                if (fileInfo.Extension.ToLower() == ".srt" || fileName.Value.EndsWith(".srt", System.StringComparison.OrdinalIgnoreCase) || Module.Utils.OpenAiUtils.CheckOpenAIAudioSupport(fileName.Key))
                {
                    string subtitleText = "";
                    var subtitleWithSort = video.GetAudioListWithSort(bookMark, true, null);
                    int indexSubtitle = 1;
                    foreach (var subTitle in subtitleWithSort)
                    {
                        string contentText = choice == "TranslateDocument" ? subTitle.Subtitle : subTitle.Content;
                        if (!string.IsNullOrEmpty(contentText) && subTitle.Start != null && subTitle.End != null)
                        {
                            subtitleText += indexSubtitle.ToString("D");
                            subtitleText += System.Environment.NewLine;
                            subtitleText += string.Format("{0} --> {1}", subTitle.Start.Value.ToString(@"hh\:mm\:ss\,fff"), subTitle.End.Value.ToString(@"hh\:mm\:ss\,fff"));
                            subtitleText += System.Environment.NewLine;
                            subtitleText += contentText;
                            subtitleText += System.Environment.NewLine;
                            subtitleText += System.Environment.NewLine;
                            indexSubtitle++;
                        }
                    }
                    if (!string.IsNullOrEmpty(subtitleText))
                    {
                        System.IO.File.WriteAllText(fileName.Value, subtitleText);

                        //048: Với liên kết: File kết quả sẽ được lưu đường dẫn tại trường Ghi chú
                        if (bookMark != null)
                            bookMark.SetBookMarkNote(fileName.Value);

                    }
                    continue;
                }

                //var audioList = bookMark != null ? video.AudioList.Where(m => m.Start != null &&  m.BookMark != null && m.BookMark.Oid.Equals(bookMark.Oid)).OrderBy(m => m.Start).ToList() : video.AudioList.Where(m => m.Start != null).OrderBy(m => m.Start).ToList();

                if (bookMark != null)
                {
                    var firstAudio = video.AudioList.Where(m => m.Start != null && m.BookMark != null && m.BookMark.Oid.Equals(bookMark.Oid)).OrderBy(m => m.Start).FirstOrDefault();
                    if (firstAudio != null)
                    {

                        //Trường hợp khác
                        //index = System.Convert.ToInt32(firstAudio.Start.Value.TotalSeconds) - 1;
                        index = System.Convert.ToInt32(firstAudio.Start.Value.TotalSeconds);
                    }
                    else
                    {
                        continue;
                    }
                }
                var waitCaption = fileIndex.ToString("D") + "/" + listFile.Count.ToString("D");
                ShowWaitForm("Xuất dữ liệu", waitCaption, stopWatch.Elapsed);

                if (fileInfo.Extension.ToLower() == ".doc" || fileInfo.Extension.ToLower() == ".docx")
                {
                    var audioList = video.GetAudioListWithSort(bookMark);
                    //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
                    //bool useUpperElement = audioList.FirstOrDefault(m => m.UpperElement != null) != null;
                    //Xử lý file đầu vào
                    var tempFolder = System.IO.Directory.GetCurrentDirectory() + "\\Temp\\" + fileInfo.Name;
                    bool includesSpacingIndentationAlignmentStyleInWordDocument = GetValueOrDefault<bool>("IncludesSpacingIndentationAlignmentStyleInWordDocument", true);
                    //if (!System.IO.Directory.Exists(tempFolder))
                    //    System.IO.Directory.CreateDirectory(tempFolder);
                    //System.IO.Compression.ZipFile.ExtractToDirectory(openFileDialog.FileName, tempFolder, true);
                    //Lưu file chính
                    Module.SystemObjects.Tools.ZipFileExtractToDirectory(fileName.Key, tempFolder, true);
                    var orderCode = bookMark?.GetOrderCode() + "";
                    if (!video.OriginStyleExport && (!video.ImportByNode || video.UpperElementImport))
                    {
                        //Bổ sung thêm style
                        var xmlStyleFile = tempFolder + "\\word\\styles.xml";
                        System.Xml.XmlDocument styleDoc = new System.Xml.XmlDocument();
                        styleDoc.Load(xmlStyleFile);

                        //Lấy toàn bộ Style có sẵn:
                        var dictionaryStyles = new System.Collections.Generic.Dictionary<string, System.Xml.XmlNode>();
                        //Tạo danh sách style cần dùng, tránh trường hợp thừa
                        var paragraphsStyleUnion = new System.Collections.Generic.List<Guid>();
                        audioList.Where(m => m.ParagraphStyle != null).ToList()
                            .ForEach(audio => GetUnionStyle(audio.ParagraphStyle, paragraphsStyleUnion));
                        if (video.ImportParagraph)
                            video.ParagraphList.Where(m => m.ParagraphStyle != null && m.ParagraphStyle.Link == bookMark).ToList()
                                .ForEach(paragraph => GetUnionStyle(paragraph.ParagraphStyle, paragraphsStyleUnion));
                        var currentParagraphStyleList = video.ParagraphStyleList.Where(x => x.Link == bookMark).OrderBy(m => m.Name);
                        foreach (System.Xml.XmlNode node in styleDoc.ChildNodes)
                        {
                            if (node.Name == "w:styles")
                            {
                                foreach (System.Xml.XmlNode styleNode in node.ChildNodes)
                                {
                                    if (styleNode.Name == "w:style")
                                    {
                                        //Lấy toàn bộ Style có sẵn:
                                        var styleId = GetAttributeInNode(styleNode, "w:styleId");
                                        if (!string.IsNullOrEmpty(styleId) && !dictionaryStyles.ContainsKey(styleId))
                                            dictionaryStyles.Add(styleId, styleNode);
                                    }
                                }

                                int colorStyle = 0;
                                foreach (var paragraphStyle in currentParagraphStyleList)
                                {
                                    colorStyle++;
                                    if (string.IsNullOrEmpty(paragraphStyle.Name))
                                        continue;
                                    if (paragraphStyle.Name == "docDefaults")
                                    {
                                        //Gán giá trị cho style có sẵn
                                        foreach (System.Xml.XmlNode stylesNode in styleDoc.ChildNodes)
                                        {
                                            if (stylesNode.Name == "w:styles" && stylesNode.FirstChild != null && stylesNode.FirstChild.Name == "w:docDefaults")
                                            {
                                                foreach (System.Xml.XmlNode nodeDefault in node.FirstChild.ChildNodes)
                                                {
                                                    if (nodeDefault.Name == "w:rPrDefault")
                                                    {
                                                        //Gán style cho char
                                                        if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:rPr")
                                                        {
                                                            SetStyleForExistStyle(nodeDefault.FirstChild, paragraphStyle, video, styleDoc);
                                                        }
                                                    }
                                                    else if (nodeDefault.Name == "w:pPrDefault")
                                                    {
                                                        //Gán style cho paragraph
                                                        if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:pPr")
                                                        {
                                                            SetStyleForExistStyle(nodeDefault.FirstChild, paragraphStyle, video, styleDoc);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                    //2023-06-22: Tên Style để là S01, S02 -> S99. Trường hợp khác cầu trúc và trong file có nhiều Style hơn 99 thì không hỗ trợ
                                    //2023-07-04: Chốt Sytle: Tất cả style trên TVOS sẽ bị ghi đè nếu trùng tên (Xóa Style cũ và tạo Style mới)
                                    //if (paragraphStyle.Name.Length == 3 && paragraphStyle.Name.StartsWith("S") && char.IsNumber(paragraphStyle.Name[1]) && char.IsNumber(paragraphStyle.Name[2]))
                                    if (!paragraphsStyleUnion.Contains(paragraphStyle.Oid))
                                        continue;
                                    string styleName = paragraphStyle.Name;
                                    if (styleName.StartsWith(orderCode))
                                    {
                                        if (!video.CreateWordStyle)
                                            continue;
                                        styleName = styleName.Substring(orderCode.Length);
                                    }
                                    string paragraphStyleName = "";
                                    string charStyleName = "";

                                    if (paragraphStyle.ParagraphStyleType == ParagraphStyleType.Empty ||
                                        paragraphStyle.ParagraphStyleType == ParagraphStyleType.Linked)
                                    {
                                        if (paragraphStyle.Name.EndsWith("Char"))
                                        {
                                            paragraphStyleName = paragraphStyle.Name.Substring(orderCode.Length);
                                            charStyleName = paragraphStyle.Name;
                                        }
                                        else
                                        {
                                            paragraphStyleName = paragraphStyle.Name;
                                            charStyleName = paragraphStyle.Name + "Char";
                                        }
                                    }
                                    else
                                    {
                                        if (paragraphStyle.ParagraphStyleType == ParagraphStyleType.Character)
                                            charStyleName = styleName;
                                        else
                                            paragraphStyleName = styleName; //Nếu là loại khác tường đương paragraph
                                    }
                                    System.Xml.XmlNode styleParagraphNode = null;

                                    if (!string.IsNullOrEmpty(paragraphStyleName) && !dictionaryStyles.ContainsKey(paragraphStyleName))
                                    {
                                        //Tạo style mới
                                        styleParagraphNode = styleDoc.CreateElement("w", "style", node.NamespaceURI);
                                        var resultParagraphNode = node.AppendChild(styleParagraphNode);
                                        AddAttributeInNode(styleParagraphNode, "w:type", "paragraph");
                                        AddAttributeInNode(styleParagraphNode, "w:customStyle", "1");
                                        AddAttributeInNode(styleParagraphNode, "w:styleId", styleName);

                                        styleParagraphNode.InnerXml += "<w:name w:val=\"" + styleName + "\"/>";
                                        styleParagraphNode.InnerXml += "<w:basedOn w:val=\"Normal\"/>";
                                        if (!string.IsNullOrEmpty(charStyleName))
                                            styleParagraphNode.InnerXml += "<w:link w:val=\"" + charStyleName + "\"/>";
                                        styleParagraphNode.InnerXml += "<w:qFormat/>";
                                        styleParagraphNode.InnerXml += "<w:rsid w:val=\"00EF1B70\"/>";

                                        //2023-06-28: Hỗ trợ BusinessObjects.Alignment, Indentation, Spacing
                                        if (includesSpacingIndentationAlignmentStyleInWordDocument)
                                        {
                                            var pPrNode = styleDoc.CreateElement("w", "pPr", node.NamespaceURI);
                                            //pPrNode.InnerXml += paragraphStyle.Spacing;
                                            //pPrNode.InnerXml += paragraphStyle.Indentation;
                                            //2023-06-29
                                            //pPrNode.InnerXml += paragraphStyle.Alignment;
                                            if (video.Alignment && paragraphStyle.Alignment != Module.BusinessObjects.Alignment.Empty)
                                            {
                                                var alignmentNode = styleDoc.CreateElement("w", "jc", node.NamespaceURI);
                                                if (paragraphStyle.Alignment == Module.BusinessObjects.Alignment.Left)
                                                    AddAttributeInNode(alignmentNode, "w:val", "left");
                                                else if (paragraphStyle.Alignment == Module.BusinessObjects.Alignment.Right)
                                                    AddAttributeInNode(alignmentNode, "w:val", "right");
                                                else if (paragraphStyle.Alignment == Module.BusinessObjects.Alignment.Centered)
                                                    AddAttributeInNode(alignmentNode, "w:val", "center");
                                                else if (paragraphStyle.Alignment == Module.BusinessObjects.Alignment.Justified)
                                                    AddAttributeInNode(alignmentNode, "w:val", "both");
                                                pPrNode.AppendChild(alignmentNode);
                                            }
                                            styleParagraphNode.AppendChild(pPrNode);
                                            if (video.Outline && paragraphStyle.Outline != null && paragraphStyle.Outline != 0)
                                                pPrNode.InnerXml += string.Format("<w:outlineLvl w:val=\"{0:n0}\"/>", paragraphStyle.Outline - 1);
                                            if (!video.ImportParagraph)
                                            {
                                                var rPrNode = CreateRprNodeFromStyle(styleParagraphNode, paragraphStyle, video);
                                            }
                                        }
                                    }
                                    System.Xml.XmlNode styleCharNode = null;
                                    if (!string.IsNullOrEmpty(charStyleName) && !dictionaryStyles.ContainsKey(charStyleName))
                                    {

                                        //Style char
                                        styleCharNode = styleDoc.CreateElement("w", "style", node.NamespaceURI);

                                        var resultStyleNode = node.AppendChild(styleCharNode);
                                        AddAttributeInNode(styleCharNode, "w:type", "character");
                                        AddAttributeInNode(styleCharNode, "w:customStyle", "1");
                                        AddAttributeInNode(styleCharNode, "w:styleId", charStyleName + "");

                                        styleCharNode.InnerXml += "<w:name w:val=\"" + charStyleName + "\"/>";
                                        styleCharNode.InnerXml += "<w:basedOn w:val=\"DefaultParagraphFont\"/>";
                                        styleCharNode.InnerXml += "<w:uiPriority w:val=\"1\"/>";
                                        styleCharNode.InnerXml += "<w:qFormat/>";
                                        if (!string.IsNullOrEmpty(paragraphStyleName))
                                            styleCharNode.InnerXml += "<w:link w:val=\"" + paragraphStyleName + "\"/>";
                                        styleCharNode.InnerXml += "<w:rsid w:val=\"00EF1B70\"/>";

                                        var rPrNode = CreateRprNodeFromStyle(styleCharNode, paragraphStyle, video);
                                        //var rPrNode = styleDoc.CreateElement("w", "rPr", node.NamespaceURI);
                                        //if (styleParagraphNode != null)
                                        //    styleParagraphNode.AppendChild(rPrNode);

                                        //string font = string.IsNullOrEmpty(paragraphStyle.TranslateFont) ? paragraphStyle.Font : paragraphStyle.TranslateFont;
                                        //if (!string.IsNullOrEmpty(font))
                                        //{
                                        //    //var fontText = string.Format("<w:rFonts w:ascii=\"{0}\" w:eastAsia=\"Times New Roman\" w:hAnsi=\"{0}\" w:cs=\"Helvetica\"/>", font);
                                        //    //rPrNode.InnerXml += fontText;
                                        //    var fontNode = styleDoc.CreateElement("w", "rFonts", node.NamespaceURI);
                                        //    AddAttributeInNode(fontNode, "w:ascii", font);
                                        //    AddAttributeInNode(fontNode, "w:eastAsia", "Times New Roman");
                                        //    AddAttributeInNode(fontNode, "w:hAnsi", font);
                                        //    AddAttributeInNode(fontNode, "w:cs", "Helvetica");
                                        //    rPrNode.AppendChild(fontNode);
                                        //}    

                                        //var rPrNodeChar = rPrNode.Clone();
                                        //styleCharNode.AppendChild(rPrNodeChar);
                                        //if (video.FontBold && paragraphStyle.Bold)
                                        //{
                                        //    rPrNode.InnerXml += "<w:b /><w:bCs />";
                                        //    //rPrNodeChar.InnerXml += "<w:b w:val=\"0\"/><w:bCs />";
                                        //    rPrNodeChar.InnerXml += "<w:b/>";
                                        //    rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "b", node.NamespaceURI));
                                        //    rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "b", node.NamespaceURI));
                                        //    rPrNodeChar.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "b", node.NamespaceURI));
                                        //}
                                        //if (video.FontItalic && paragraphStyle.Italic)
                                        //{
                                        //    rPrNode.InnerXml += "<w:i /><w:iCs />";
                                        //    //rPrNodeChar.InnerXml += "<w:i w:val=\"0\"/><w:iCs />";
                                        //    rPrNodeChar.InnerXml += "<w:i/>";
                                        //}
                                        //if (video.FontUnderline && paragraphStyle.Underline)
                                        //{
                                        //    rPrNode.InnerXml += "<w:u w:val=\"single\" />";
                                        //    rPrNodeChar.InnerXml += "<w:u w:val=\"single\" />";
                                        //}
                                        //if (video.FontColor && paragraphStyle.Color != null)
                                        //{
                                        //    //rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\" w:themeColor=\"accent{1:0}\" w:themeShade=\"BF\" />", paragraphStyle.Color.Value.Name.ToString(), colorStyle);
                                        //    string hexColor = ColorTranslator.ToHtml(paragraphStyle.Color.Value);
                                        //    //Bỏ dấu # ở đầu
                                        //    hexColor = hexColor.Substring(1);
                                        //    rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                                        //    rPrNodeChar.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                                        //}
                                        //if (paragraphStyle.Size != null)
                                        //{
                                        //    rPrNode.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                                        //    rPrNodeChar.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                                        //}

                                    }
                                    if (styleParagraphNode is not null && styleCharNode is null)
                                    {
                                        //Thay đổi cấu trúc style sẵn có
                                        foreach (var wStyleName in dictionaryStyles.Keys)
                                        {
                                            if (wStyleName == paragraphStyleName || wStyleName == charStyleName)
                                            {
                                                foreach (System.Xml.XmlNode fontNode in dictionaryStyles[wStyleName].ChildNodes)
                                                {
                                                    SetStyleForExistStyle(fontNode, paragraphStyle, video, styleDoc);
                                                }
                                            }
                                        }

                                    }

                                }

                            }
                        }
                        styleDoc.Save(xmlStyleFile);

                    }

                    var xmlFile = tempFolder + "\\word\\document.xml";
                    if (System.IO.File.Exists(xmlFile))
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.Load(xmlFile);
                        System.Xml.XmlNode rootNode;
                        if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].ChildNodes.Count == 1)
                            rootNode = doc.ChildNodes[1].ChildNodes[0];
                        else
                            rootNode = doc.ChildNodes[0];
                        //order = 1;
                        //Lấy danh sách nội dung đã dịch                                                                       
                        //var dictionaryAudio = new Dictionary<int, Audio>();

                        //Tối ưu file word

                        //ReplaceNodeContent(rootNode);
                        //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt

                        if (video.ImportByNode && !video.UpperElementImport)
                        {
                            //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
                            audioList = audioList.Where(m => m.UpperElement != null).ToList();
                        }
                        else
                        {
                            if (video.UpperElementImport)
                            {
                                var listParentNotImport = new System.Collections.Generic.List<System.Guid>();
                                foreach (var audio in audioList)
                                {
                                    if (audio.UpperElement != null && !listParentNotImport.Contains(audio.UpperElement.Oid))
                                        listParentNotImport.Add(audio.UpperElement.Oid);
                                }
                                audioList = audioList.Where(m => !listParentNotImport.Contains(m.Oid)).ToList();
                            }
                            //Tối ưu hóa file doc
                            ShowWaitForm("Đang tối ưu hóa tập tin", " ");
                            OptimalDocument(video, doc);
                            ShowWaitForm(null, null);
                            //2023-06-27: Chỉnh margin của file word thành Normal
                            if (!video.OriginStyleExport)
                            {
                                if (GetValueOrDefault<bool>("PageLayoutMarginsIsNormal", false))
                                {
                                    var pgMarNodes = doc.GetElementsByTagName("w:pgMar");
                                    foreach (System.Xml.XmlNode node in pgMarNodes)
                                    {
                                        foreach (System.Xml.XmlAttribute attribute in node.Attributes)
                                        {
                                            if (attribute.Name == "w:top" || attribute.Name == "w:right" || attribute.Name == "w:bottom" || attribute.Name == "w:left")
                                                attribute.Value = "1440";
                                            else if (attribute.Name == "w:header" || attribute.Name == "w:footer")
                                                attribute.Value = "720";
                                        }
                                    }
                                }
                                else if (GetValueOrDefault<bool>("PageLayoutSynchronizedMarginsLeftAndRight", true))
                                {
                                    //2023-06-27: Đồng bộ hóa Margins Left và Margins Right
                                    var pgMarNodes = doc.GetElementsByTagName("w:pgMar");
                                    foreach (System.Xml.XmlNode node in pgMarNodes)
                                    {
                                        var leftValue = GetAttributeInNode(node, "w:left");
                                        if (!string.IsNullOrEmpty(leftValue))
                                        {
                                            foreach (System.Xml.XmlAttribute attribute in node.Attributes)
                                            {
                                                if (attribute.Name == "w:right")
                                                    attribute.Value = leftValue;
                                            }
                                        }

                                    }
                                }
                            }

                        }

                        //for (int i = 0; i < audioList.Count; i++)
                        //{
                        //    dictionaryAudio.Add(i + 1, audioList[i]);
                        //}

                        //2023-06-14: Thay thế theo w:p                        
                        //ReplaceNodeContent(rootNode, dictionaryAudio);
                        //Thay thế theo w:t đã optimal                   
                        System.Xml.XmlNodeList wtNodes = null;
                        if (!video.FootNote)
                        {
                            wtNodes = doc.GetElementsByTagName("w:t");
                        }
                        else
                        {
                            System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
                            namespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                            //Dấu chấm . ở đầu biểu thức chỉ rõ rằng việc tìm kiếm bắt đầu từ node hiện tại 
                            wtNodes = doc.SelectNodes(".//w:t | .//w:footnoteReference", namespaceManager);
                            //wtNodes = doc.SelectNodes("//w:t | //w:footnoteReference", namespaceManager) //Kết quả tương đương vì tìm từ gốc;
                        }
                        //Xử lý ghép nội dung
                        var parentNodeList = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>>();
                        //2025-02-24: Hỗ trợ nạp paragraph
                        if (video.ImportParagraph)
                        {
                            var wpNodes = doc.GetElementsByTagName("w:p");
                            foreach (System.Xml.XmlNode wpNode in wpNodes)
                            {
                                parentNodeList.Add(wpNode, new System.Collections.Generic.List<System.Xml.XmlNode>());
                            }
                        }
                        foreach (System.Xml.XmlNode node in wtNodes)
                        {
                            var parentNode = GetParentNode(node);
                            if (parentNode != null)
                            {
                                if (parentNodeList.ContainsKey(parentNode))
                                    parentNodeList[parentNode].Add(node);
                                else
                                    parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                            }
                            else
                            {
                                if (parentNodeList.ContainsKey(node))
                                    parentNodeList[node].Add(node);
                                else
                                    parentNodeList.Add(node, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                            }
                        }
                        System.Xml.XmlDocument footnotesDoc = null;
                        var xmlFootnotesFile = tempFolder + "\\word\\footnotes.xml";
                        if (video.FootNote)
                        {
                            //Xuất Footnotes                                               
                            footnotesDoc = new System.Xml.XmlDocument();
                            footnotesDoc.Load(xmlFootnotesFile);
                            if (!video.ImportByNode)
                                OptimalDocument(video, footnotesDoc);//Tối ưu Footnotes
                            var wtFootnotesNodes = footnotesDoc.GetElementsByTagName("w:t");
                            foreach (System.Xml.XmlNode node in wtFootnotesNodes)
                            {
                                var parentNode = GetParentNode(node);
                                if (parentNode != null)
                                {
                                    if (parentNodeList.ContainsKey(parentNode))
                                        parentNodeList[parentNode].Add(node);
                                    else
                                        parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                                }
                            }
                        }
                        int audioIndex = 0;
                        int paragraphIndex = 0;
                        var parentKeysNodeList = parentNodeList.Keys.ToList();
                        foreach (var keyNode in parentKeysNodeList)
                        {
                            //int debug = 205;
                            //if (audioIndex >= debug)
                            //{

                            //}

                            bool editedParagraph = false;
                            var nodeList = parentNodeList[keyNode];
                            if (video.ImportParagraph)
                            {
                                paragraphIndex++;
                            }
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                if (nodeList[i].Name == "w:footnoteReference")
                                {

                                }
                                else
                                {
                                    if (!WtContentIsValidate(video, nodeList[i]))
                                        continue;
                                    if (audioIndex >= audioList.Count)
                                    {
                                        break;
                                    }
                                    //var currentAudio = dictionaryAudio[index];
                                    var currentAudio = audioList[audioIndex];
                                    //Nếu xuất giữ kiểu thì không tạo style và chỉnh sửa style
                                    if (!video.OriginStyleExport && video.ImportParagraph && currentAudio.Paragraph != null && !editedParagraph && currentAudio.Paragraph.ParagraphStyle != null)
                                    {
                                        //Nếu style chưa sửa thì sửa
                                        editedParagraph = true;
                                        //Hỗ trợ thay đổi style

                                        if (video.CreateWordStyle)
                                        {
                                            if (keyNode.Name == "w:p" && !string.IsNullOrEmpty(currentAudio.Paragraph?.ParagraphStyle?.Name))
                                            {
                                                if (includesSpacingIndentationAlignmentStyleInWordDocument)
                                                {
                                                    //string styleName = currentAudio.Paragraph.ParagraphStyle.Name;
                                                    //if (styleName.StartsWith(orderCode))
                                                    //    styleName = styleName.Substring(orderCode.Length);
                                                    //System.Xml.XmlNode pPrNode = GetOrCreatePPr(keyNode, styleName);
                                                    //Luôn tạo style
                                                    ApplyStyleValue(video, currentAudio, keyNode, null);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Nếu style tự sinh thì gán trực tiếp
                                            if (!string.IsNullOrEmpty(currentAudio.Paragraph?.ParagraphStyle?.Name)
                                                    && currentAudio.Paragraph.ParagraphStyle.Name.StartsWith(orderCode))
                                            {
                                                //Gán font trực tiếp vào text node
                                                SetStyleForExistStyle(keyNode.FirstChild, currentAudio.Paragraph.ParagraphStyle, video, doc);
                                            }
                                            else
                                            {
                                                //Gán style nếu có
                                                bool changedStyle = ChangePprStyle(video, currentAudio, keyNode);
                                                //Nếu không gán được style thì gán trực tiếp trên note
                                                if (!changedStyle && keyNode.FirstChild != null && keyNode.FirstChild.Name == "w:pPr")
                                                {
                                                    //Gán font trực tiếp vào text node
                                                    SetStyleForExistStyle(keyNode.FirstChild, currentAudio.Paragraph.ParagraphStyle, video, doc);
                                                }
                                            }

                                        }

                                    }

                                    //if (dictionaryAudio.ContainsKey(index))
                                    //{
                                    //2023-07-20: Xóa trường được đánh dấu xóa
                                    if (currentAudio.Delete)
                                    {
                                        if (nodeList.Count == 1)
                                        {
                                            if (keyNode.ParentNode != null)
                                                keyNode.ParentNode.RemoveChild(keyNode);
                                            else
                                                keyNode.RemoveAll();
                                        }
                                        else
                                        {
                                            if (nodeList[i].ParentNode != null)
                                            {
                                                //2024-09-10: Xóa luôn nút xuống dòng sau đó
                                                if (nodeList[i].NextSibling != null && nodeList[i].NextSibling.Name == "w:br" && nodeList[i].NextSibling.Attributes.Count == 0)
                                                {
                                                    nodeList[i].ParentNode.RemoveChild(nodeList[i].NextSibling);
                                                }
                                                var oldParentNode = nodeList[i].ParentNode;
                                                nodeList[i].ParentNode.RemoveChild(nodeList[i]);
                                                if (oldParentNode != null && oldParentNode.ParentNode != null && oldParentNode.ChildNodes.Count == 1 && oldParentNode.FirstChild.Name.EndsWith(":rPr"))
                                                {
                                                    oldParentNode.ParentNode.RemoveChild(oldParentNode);
                                                }
                                            }
                                            else
                                                nodeList[i].RemoveAll();
                                        }
                                        audioIndex++;
                                        continue;
                                    }
                                    //2023-08-24: Những Paragraph dựng cờ (Có check TextNode) sẽ xuất TextNode còn không thì xuất kiểu cũ
                                    bool notMergeNode = currentAudio.UpperElement != null && !currentAudio.UpperElement.TextNode;
                                    //2023-08-24 Nếu là nạp theo đoạn có parent thì tương đương có ghép
                                    //2025-03-04: Cấp trên là để dùng cho footnote nếu không phải import By Node
                                    if (!video.FootNote && notMergeNode && !video.UpperElementImport)
                                    {
                                        //Nạp theo nốt
                                        //Trường hợp node i đầu tiên không hợp lệ, i có thể là các node khác
                                        if (choice.Contains("TranslateDocument"))
                                            SetTextNoteInnerText(nodeList[i], currentAudio.UpperElement.Subtitle);
                                        else if (choice.Contains("ContentDocument"))
                                            SetTextNoteInnerText(nodeList[i], currentAudio.UpperElement.Content);
                                        if (video.CreateWordStyle)
                                            ApplyStyleValue(video, currentAudio, video.ImportParagraph ? null : keyNode, nodeList[i]);
                                        else if (nodeList[i].ParentNode?.FirstChild != null)
                                        {
                                            if (!string.IsNullOrEmpty(currentAudio.ParagraphStyle?.Name)
                                                    && currentAudio.ParagraphStyle.Name.StartsWith(orderCode))
                                            {
                                                //Gán font trực tiếp vào text node
                                                SetStyleForExistStyle(nodeList[i].ParentNode?.FirstChild, currentAudio.ParagraphStyle, video, doc);
                                            }
                                            else
                                            {
                                                bool changedStyle = ChangeRprStyle(currentAudio, nodeList[i]);
                                                //không gán được thì Giữ style trên text Node
                                                if (!changedStyle && nodeList[i].ParentNode?.FirstChild != null && nodeList[i].ParentNode.FirstChild.Name == "w:rPr")
                                                {
                                                    //Gán font trực tiếp vào text node
                                                    SetStyleForExistStyle(nodeList[i].ParentNode.FirstChild, currentAudio.ParagraphStyle, video, doc);
                                                }
                                            }
                                        }
                                        //Xóa các textNode sau đó                                        
                                        if (nodeList.Count > 1)
                                        {
                                            var totalChild = audioList.Where(m => m.UpperElement != null && m.UpperElement.Oid.Equals(currentAudio.UpperElement.Oid)).Count();
                                            //ChildValidate + thêm hiện tại
                                            int childValidate = 1;
                                            for (int j = i + 1; j < nodeList.Count; j++)
                                            {
                                                if (WtContentIsValidate(video, nodeList[j]))
                                                {
                                                    if (childValidate > totalChild)
                                                    {
                                                        //Nếu trường hợp này xảy ra thì là bị lỗi
                                                    }
                                                    //Trường hợp node i đầu tiên không hợp lệ, i có thể là các node khác                                                    
                                                    //index++;
                                                    childValidate++;
                                                }
                                                if (nodeList[j].ParentNode != null)
                                                {
                                                    nodeList[j].ParentNode.RemoveChild(nodeList[j]);
                                                    audioIndex++;
                                                    index++;
                                                }

                                                //Nếu ghép xong thì bỏ
                                                //if (childValidate == totalChild)
                                                //    break;
                                            }

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //Nạp theo đoạn
                                        //Thay thế nội dung đã dịch
                                        if (choice.Contains("TranslateDocument"))
                                            SetTextNoteInnerText(nodeList[i], currentAudio.Subtitle);
                                        else if (choice.Contains("ContentDocument"))
                                            SetTextNoteInnerText(nodeList[i], currentAudio.Content);
                                        //Gán style trường hợp Style tự tạo
                                        //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
                                        //2024-08-30: Thêm Option: 
                                        //CreateWordStyle: Tạo Word Style
                                        //(thay cho chức năng hiện tại của OrinalStyleExport)
                                        // OriginalStyleExport / Xuất giữ kiểu: khi tích vào và xuất tư liệu sẽ không thay đổi Style và format gì của tài liệu gốc(đã tối ưu theo Option)
                                        if (currentAudio.ParagraphStyle != null && !video.OriginStyleExport)
                                        {
                                            if (video.CreateWordStyle)
                                            {
                                                if (!string.IsNullOrEmpty(currentAudio.ParagraphStyle?.Name))
                                                {
                                                    //Nếu ImportParagraph thì nạp ở trên

                                                    if (!video.ImportParagraph && keyNode.Name == "w:p")
                                                    {
                                                        bool applyPStyle = parentNodeList[keyNode].Count == 1;
                                                        //Trường hợp áp dụng cho paragraph                                                        
                                                        //Nếu có Outline thì buộc phải apply style
                                                        if (includesSpacingIndentationAlignmentStyleInWordDocument && (applyPStyle || currentAudio.ParagraphStyle.Outline > 0))
                                                        {
                                                            ApplyStyleValue(video, currentAudio, keyNode, null);
                                                        }
                                                    }
                                                    //Trường hợp áp dụng cho từ
                                                    if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.Name == "w:r")
                                                    {
                                                        ApplyStyleValue(video, currentAudio, null, nodeList[i]);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                //Nếu style tự sinh thì gán trực tiếp
                                                if (!string.IsNullOrEmpty(currentAudio.ParagraphStyle?.Name)
                                                    && currentAudio.ParagraphStyle.Name.StartsWith(orderCode))
                                                {
                                                    //Gán font trực tiếp vào text node
                                                    SetStyleForExistStyle(nodeList[i].ParentNode.FirstChild, currentAudio.ParagraphStyle, video, doc);
                                                }
                                                else
                                                {
                                                    //Gán style nếu có
                                                    bool changedStyle = ChangeRprStyle(currentAudio, nodeList[i]);
                                                    //không gán được thì Giữ style trên text Node
                                                    if (!changedStyle && nodeList[i].ParentNode.FirstChild != null && nodeList[i].ParentNode.FirstChild.Name == "w:rPr")
                                                    {
                                                        //Gán font trực tiếp vào text node
                                                        SetStyleForExistStyle(nodeList[i].ParentNode.FirstChild, currentAudio.ParagraphStyle, video, doc);
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    //}
                                    audioIndex++;
                                    index++;


                                    if (video.ImportParagraph && currentAudio.Paragraph?.Order != paragraphIndex)
                                    {
                                        //Chuyển thành phần này sang parent khác
                                        if (i == 0 && !nodeList[i].InnerText.StartsWith(" "))
                                        {
                                            nodeList[i].InnerText = " " + nodeList[i].InnerText;
                                            var spacePreserveNode = GetAttributeInNode(nodeList[i], "xml:space");
                                            if (spacePreserveNode is null)
                                            {
                                                AddAttributeInNode(nodeList[i], "xml:space", "preserve");
                                            }
                                        }
                                        var otherPara = parentKeysNodeList[currentAudio.Paragraph.Order.Value - 1];
                                        if (otherPara != null)
                                            otherPara.AppendChild(nodeList[i].ParentNode);
                                        //Xóa key node là trống
                                        if (keyNode.ChildNodes.Count == 0 || (keyNode.ChildNodes.Count == 1 && keyNode.FirstChild.Name.Equals("w:pPr")))
                                        {
                                            keyNode.ParentNode.RemoveChild(keyNode);
                                        }
                                    }
                                }

                            }
                            var percent = (Convert.ToDecimal(index) / wtNodes.Count).ToString("p0");
                            ShowWaitForm(percent, bookMark != null ? bookMark.Name : " ");
                        }
                        //if (IsPhoto)
                        //{
                        //    ImportExportMedia(doc, tempFolder, fileInfo, null, null, null, bookMark, "", true);
                        //}
                        //Ghi đề file xml                        
                        //Lưu lại thành file word
                        doc.Save(xmlFile);
                        if (video.ImportParagraph && footnotesDoc != null)
                        {
                            footnotesDoc.Save(xmlFootnotesFile); //Save footnotes
                        }
                        //Xóa file đã có
                        if (System.IO.File.Exists(fileName.Value))
                        {
                            System.IO.File.Delete(fileName.Value);
                        }
                        System.IO.Compression.ZipFile.CreateFromDirectory(tempFolder, fileName.Value);
                        if (bookMark != null)
                            bookMark.Note = fileName.Value;


                        ShowWaitForm(null, null);
                        //Xóa thư mục bộ nhớ tạm
                        if (!System.Diagnostics.Debugger.IsAttached)
                            System.IO.Directory.Delete(tempFolder, true);
                        //Giải phóng bộ nhớ
                        try
                        {
                            parentNodeList = null;
                            doc = null;
                            GC.Collect();
                        }
                        catch (System.Exception) { }

                        //Mở kết quả
                        //if (bookMark is null && MessageBox.Show("Bạn có muốn mở kết quả không?", "Thành công", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        //    startInfo.UseShellExecute = true;
                        //    startInfo.FileName = fileName.Value;
                        //    System.Diagnostics.Process.Start(startInfo);
                        //}
                    }
                }
                if (fileInfo.Extension.ToLower() == ".pptx")
                {
                    ExportPowerPoint(video, fileInfo, fileName.Key, choice, bookMark);
                }
                else if (fileName.Key.StartsWith("http") || fileInfo.Extension.ToLower() == ".html" || fileInfo.Extension.ToLower() == ".htm")
                {

                    HtmlAgilityPack.HtmlDocument htmlDocument = null;
                    if (fileName.Key.StartsWith("http"))
                    {
                        var cacheFile = Module.Helpers.NameHelper.GetCacheFileName(video.Session, fileName.Key);
                        if (!string.IsNullOrEmpty(cacheFile))
                        {
                            htmlDocument = new HtmlAgilityPack.HtmlDocument();
                            htmlDocument.Load(cacheFile);
                        }
                        else
                        {
                            string url = fileName.Key;
                            if (bookMark != null && !string.IsNullOrEmpty(bookMark.Note) && System.IO.File.Exists(bookMark.Note))
                                url = bookMark.Note;
                            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                            //ShowWaitForm("Đang load", waitCaption, stopWatch.Elapsed);
                            htmlDocument = web.Load(url);
                            Module.SystemObjects.Tools.ReplaceIndirectLink(url, htmlDocument);
                        }

                    }
                    else
                    {
                        htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.Load(fileName.Key);
                    }
                    if (htmlDocument.DocumentNode != null)
                    {
                        var htmlRange = htmlDocument.DocumentNode.SelectSingleNode(string.IsNullOrEmpty(xpath) ? "//body" : xpath);
                        FillContentFromHtmlNode(video, ref index, htmlRange, null, bookMark, choice.Contains("TranslateDocument"));
                        htmlDocument.Save(fileName.Value);
                        if (bookMark != null)
                            bookMark.Note = fileName.Value;
                        //if (listFile.Count == 1 && MessageBox.Show("Bạn có muốn mở kết quả không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    System.Diagnostics.Process.Start("explorer.exe", fileName.Value);
                        //}
                    }
                }
                fileIndex++;
            }
            stopWatch.Stop();
            ShowWaitForm(null, null);
        }

        private System.Xml.XmlNode CreateRprNodeFromStyle(System.Xml.XmlNode parentNode, ParagraphStyle paragraphStyle, Video video)
        {
            var rPrNode = parentNode.OwnerDocument.CreateElement("w", "rPr", parentNode.NamespaceURI);
            parentNode.AppendChild(rPrNode);

            string font = string.IsNullOrEmpty(paragraphStyle.TranslateFont) ? paragraphStyle.Font : paragraphStyle.TranslateFont;
            if (!string.IsNullOrEmpty(font))
            {
                //var fontText = string.Format("<w:rFonts w:ascii=\"{0}\" w:eastAsia=\"Times New Roman\" w:hAnsi=\"{0}\" w:cs=\"Helvetica\"/>", font);
                //rPrNode.InnerXml += fontText;
                var fontNode = parentNode.OwnerDocument.CreateElement("w", "rFonts", parentNode.NamespaceURI);
                AddAttributeInNode(fontNode, "w:ascii", font);
                AddAttributeInNode(fontNode, "w:eastAsia", "Times New Roman");
                AddAttributeInNode(fontNode, "w:hAnsi", font);
                AddAttributeInNode(fontNode, "w:cs", "Helvetica");
                rPrNode.AppendChild(fontNode);
            }

            if (video.FontBold && paragraphStyle.Bold)
            {
                //rPrNode.InnerXml += "<w:b /><w:bCs />";
                ////rPrNodeChar.InnerXml += "<w:b w:val=\"0\"/><w:bCs />";
                //rPrNodeChar.InnerXml += "<w:b/>";
                rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "b", parentNode.NamespaceURI));
                rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "bCs", parentNode.NamespaceURI));
            }
            if (video.FontItalic && paragraphStyle.Italic)
            {
                //rPrNode.InnerXml += "<w:i /><w:iCs />";
                ////rPrNodeChar.InnerXml += "<w:i w:val=\"0\"/><w:iCs />";
                //rPrNodeChar.InnerXml += "<w:i/>";
                rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "i", parentNode.NamespaceURI));
                rPrNode.AppendChild(rPrNode.OwnerDocument.CreateElement("w", "iCs", parentNode.NamespaceURI));
            }
            if (video.FontUnderline && paragraphStyle.Underline)
            {
                //rPrNode.InnerXml += "<w:u w:val=\"single\" />";
                //rPrNodeChar.InnerXml += "<w:u w:val=\"single\" />";
                var uNode = rPrNode.OwnerDocument.CreateElement("w", "u", parentNode.NamespaceURI);
                AddAttributeInNode(uNode, "w:val", "single");
                rPrNode.AppendChild(uNode);
            }
            if (video.FontColor && paragraphStyle.Color != null)
            {
                //rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\" w:themeColor=\"accent{1:0}\" w:themeShade=\"BF\" />", paragraphStyle.Color.Value.Name.ToString(), colorStyle);
                string hexColor = System.Drawing.ColorTranslator.ToHtml(paragraphStyle.Color.Value);
                //Bỏ dấu # ở đầu
                hexColor = hexColor.Substring(1);
                //rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                //rPrNodeChar.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                var colorNode = rPrNode.OwnerDocument.CreateElement("w", "color", parentNode.NamespaceURI);
                AddAttributeInNode(colorNode, "w:val", hexColor);
                rPrNode.AppendChild(colorNode);
            }
            if (paragraphStyle.Size != null)
            {
                //rPrNode.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                //rPrNodeChar.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                var sizeNode = rPrNode.OwnerDocument.CreateElement("w", "sz", parentNode.NamespaceURI);
                AddAttributeInNode(sizeNode, "w:val", string.Format("{0:n0}", paragraphStyle.Size * 2));
                rPrNode.AppendChild(sizeNode);
            }
            return rPrNode;
        }

        private string GetStyleName(ParagraphStyle paragraphStyle)
        {
            if (paragraphStyle.Link?.Order != null)
            {
                var orderCode = paragraphStyle.Link.GetOrderCode() + "";
                string styleName = paragraphStyle.Name;
                if (styleName.StartsWith(orderCode))
                    styleName = styleName.Substring(orderCode.Length);
                return styleName;
            }
            return paragraphStyle.Name;
        }

        private bool ChangeRprStyle(Audio currentAudio, System.Xml.XmlNode currentNode)
        {
            bool changedStyle = false;
            //Kiểm tra theo Paragraph
            var paragraphStyle = currentAudio.ParagraphStyle;
            if (paragraphStyle != null)
            {

                //Hỗ trợ thay đổi style
                if (paragraphStyle != null && !string.IsNullOrEmpty(paragraphStyle.Name) && currentAudio.BookMark != null &&
                    currentAudio.BookMark.Order != null && paragraphStyle.Name.StartsWith(currentAudio.BookMark.GetOrderCode()))
                {
                    string styleName = GetStyleName(paragraphStyle);
                    //Hủy, chỉ hõ trợ gắn style cho paragraph Kiểm tra xem có rStyle không
                    if (currentNode?.ParentNode != null)
                    {
                        //System.Xml.XmlNode rPrNode = null;                        
                        if (currentNode.ParentNode.Name == "w:r" && currentNode.ParentNode.FirstChild?.Name == "w:rPr" &&
                            currentNode.ParentNode.FirstChild?.FirstChild?.Name == "w:rStyle")
                        {
                            var rStyleValue = GetAttributeInNode(currentNode.ParentNode.FirstChild.FirstChild);
                            if (!string.IsNullOrEmpty(rStyleValue))
                            {
                                if (rStyleValue == styleName)
                                {
                                    return false;
                                }
                                else
                                {
                                    currentNode.ParentNode.FirstChild.FirstChild.Attributes[0].Value = styleName;
                                    return true;
                                }

                            }
                        }
                    }
                }
            }
            return changedStyle;
        }

        private bool ChangePprStyle(Video video, Audio currentAudio, System.Xml.XmlNode keyNode)
        {
            bool changedStyle = false;
            //Kiểm tra theo Paragraph
            var paragraphStyle = video.ImportParagraph ? currentAudio.Paragraph?.ParagraphStyle : currentAudio.ParagraphStyle;
            //Hỗ trợ thay đổi style
            if (paragraphStyle != null && !string.IsNullOrEmpty(paragraphStyle.Name) && currentAudio.BookMark != null &&
                currentAudio.BookMark.Order != null && paragraphStyle.Name.StartsWith(currentAudio.BookMark.GetOrderCode()))
            {
                string styleName = GetStyleName(paragraphStyle);
                //Hủy, chỉ hõ trợ gắn style cho paragraph Kiểm tra xem có rStyle không
                //System.Xml.XmlNode rPrNode = null;                        
                if (keyNode.FirstChild?.Name == "w:pPr" &&
                    keyNode.FirstChild?.FirstChild?.Name == "w:pStyle")
                {
                    var pStyleValue = GetAttributeInNode(keyNode.FirstChild.FirstChild);
                    if (!string.IsNullOrEmpty(pStyleValue))
                    {
                        if (pStyleValue == styleName)
                        {
                            return false;
                        }
                        else
                        {
                            keyNode.FirstChild.FirstChild.Attributes[0].Value = styleName;
                            return true;
                        }

                    }
                }
            }
            return changedStyle;
        }

        private bool ApplyStyleValue(Video video, Audio currentAudio, System.Xml.XmlNode keyNode, System.Xml.XmlNode currentNode)
        {
            bool changedStyle = false;
            //Kiểm tra theo Paragraph
            var paragraphStyle = currentNode is null ? currentAudio.Paragraph?.ParagraphStyle : currentAudio.ParagraphStyle;
            if (paragraphStyle != null)
            {

                //Hỗ trợ thay đổi style
                if (paragraphStyle != null && !string.IsNullOrEmpty(paragraphStyle.Name) && currentAudio.BookMark != null &&
                    currentAudio.BookMark.Order != null && paragraphStyle.Name.StartsWith(currentAudio.BookMark.GetOrderCode()))
                {
                    var orderCode = currentAudio.BookMark.GetOrderCode() + "";
                    string styleName = paragraphStyle.Name;
                    if (styleName.StartsWith(orderCode))
                        styleName = styleName.Substring(orderCode.Length);
                    if (!video.ImportParagraph)
                        styleName += "Char";
                    //Hủy, chỉ hõ trợ gắn style cho paragraph Kiểm tra xem có rStyle không
                    if (currentNode?.ParentNode != null)
                    {
                        //System.Xml.XmlNode rPrNode = null;                        
                        if (currentNode.ParentNode.Name == "w:r" && currentNode.ParentNode.FirstChild?.Name == "w:rPr" &&
                            currentNode.ParentNode.FirstChild?.FirstChild?.Name == "w:rStyle")
                        {
                            System.Xml.XmlNode rPrNode = null;

                            if (currentNode.FirstChild.Name.Equals("w:rPr"))
                            {
                                rPrNode = currentNode.FirstChild;
                                //Xóa các Node Style con và đưa vào file Style.xml 
                                rPrNode.RemoveAll();
                            }
                            else
                            {
                                rPrNode = currentNode.OwnerDocument.CreateElement("w", "rPr", currentNode.NamespaceURI);
                                currentNode.InsertBefore(rPrNode, currentNode.FirstChild);
                                changedStyle = true;
                            }
                            System.Xml.XmlNode rStyleNode = null;
                            if (rPrNode.FirstChild != null && rPrNode.FirstChild.Name == "w:rStyle")
                            {
                                rStyleNode = rPrNode.FirstChild;
                            }
                            else
                            {
                                rStyleNode = currentNode.OwnerDocument.CreateElement("w", "rStyle", currentNode.NamespaceURI);
                                if (rPrNode.FirstChild != null)
                                    rPrNode.InsertBefore(rStyleNode, rPrNode.FirstChild);
                                else
                                    rPrNode.AppendChild(rStyleNode);
                                changedStyle = true;
                            }
                            System.Xml.XmlAttribute xmlAttribute = null;
                            foreach (System.Xml.XmlAttribute currentAttribute in rStyleNode.Attributes)
                            {
                                if (currentAttribute.Name == "w:val")
                                {
                                    xmlAttribute = currentAttribute;
                                    break;
                                }
                            }
                            if (xmlAttribute is null)
                            {
                                xmlAttribute = AddAttributeInNode(rStyleNode, "w:val", styleName);
                                changedStyle = true;
                            }
                            else if (xmlAttribute.Name == "w:val" && xmlAttribute.Value != styleName)
                            {
                                xmlAttribute.Value = styleName;
                                changedStyle = true;

                            }
                        }
                    }
                    //Kiểm tra xem có phải là style tự sinh không?
                    if (keyNode?.Name == "w:p" && keyNode.FirstChild != null)
                    {
                        System.Xml.XmlNode pPrNode = null;
                        if (keyNode.FirstChild.Name.Equals("w:pPr"))
                        {
                            pPrNode = keyNode.FirstChild;
                        }
                        else
                        {
                            pPrNode = keyNode.OwnerDocument.CreateElement("w", "pPr", keyNode.NamespaceURI);
                            keyNode.InsertBefore(pPrNode, keyNode.FirstChild);
                            changedStyle = true;
                        }
                        System.Xml.XmlNode pStyleNode = null;
                        if (pPrNode.FirstChild != null && pPrNode.FirstChild.Name == "w:pStyle")
                        {
                            pStyleNode = pPrNode.FirstChild;
                            //Xóa các Node Style con và đưa vào file Style.xml 
                            pStyleNode.RemoveAll();
                        }
                        else
                        {
                            pStyleNode = keyNode.OwnerDocument.CreateElement("w", "pStyle", keyNode.NamespaceURI);
                            if (pPrNode.FirstChild != null)
                                pPrNode.InsertBefore(pStyleNode, pPrNode.FirstChild);
                            else
                                pPrNode.AppendChild(pStyleNode);
                            changedStyle = true;
                        }
                        System.Xml.XmlAttribute xmlAttribute = null;
                        foreach (System.Xml.XmlAttribute currentAttribute in pStyleNode.Attributes)
                        {
                            if (currentAttribute.Name == "w:val")
                            {
                                xmlAttribute = currentAttribute;
                                break;
                            }
                        }
                        if (xmlAttribute is null)
                        {
                            xmlAttribute = AddAttributeInNode(pStyleNode, "w:val", styleName);
                            changedStyle = true;
                        }
                        else if (xmlAttribute.Name == "w:val" && xmlAttribute.Value != styleName)
                        {
                            xmlAttribute.Value = styleName;
                            changedStyle = true;

                        }
                    }
                }
            }

            return changedStyle;
        }
        private void SetTextNoteInnerText(System.Xml.XmlNode node, string innerText)
        {
            if (!string.IsNullOrEmpty(innerText))
            {
                var innerTexts = innerText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (innerTexts.Length > 1 && node.ParentNode != null && node.OwnerDocument != null)
                {
                    for (int i = innerTexts.Length - 1; i >= 0; i--)
                    {
                        if (i == 0)
                            node.InnerText = innerTexts[i];
                        else
                        {
                            //Tạo note xuống dòng
                            var brNode = node.OwnerDocument.CreateElement("w", "br", node.NamespaceURI);
                            node.ParentNode.InsertAfter(brNode, node);

                            var newNode = node.Clone();
                            newNode.InnerText = innerTexts[i];
                            node.ParentNode.InsertAfter(newNode, brNode);
                        }
                    }
                }
                else
                {

                    if (node.InnerText != innerText)
                    {
                        if (innerText.Trim() != node.InnerText.Trim())
                        {

                        }
                    }
                    node.InnerText = innerText;
                }
            }
            else
            {
                if (node.InnerText != innerText)
                {

                }
                node.InnerText = innerText;
            }
        }

        private string ExportPowerPoint(Video video, System.IO.FileInfo openFileInfo, string saveFileName, string choice, BookMark bookMark)
        {
            //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
            bool useUpperElement = video.AudioList.FirstOrDefault(m => m.UpperElement != null) != null;
            //Xử lý file đầu vào
            var tempFolder = System.IO.Directory.GetCurrentDirectory() + "\\Temp\\" + openFileInfo.Name;
            bool includesSpacingIndentationAlignmentStyleInWordDocument = GetValueOrDefault<bool>("IncludesSpacingIndentationAlignmentStyleInWordDocument", true);
            //if (!System.IO.Directory.Exists(tempFolder))
            //    System.IO.Directory.CreateDirectory(tempFolder);
            //System.IO.Compression.ZipFile.ExtractToDirectory(openFileDialog.FileName, tempFolder, true);
            //Lưu file chính
            Module.SystemObjects.Tools.ZipFileExtractToDirectory(openFileInfo.FullName, tempFolder, true);
            if (!useUpperElement || video.UpperElementImport)
            {
                ////Bổ sung thêm style
                //var xmlStyleFile = tempFolder + "\\word\\styles.xml";
                //System.Xml.XmlDocument styleDoc = new System.Xml.XmlDocument();
                //styleDoc.Load(xmlStyleFile);

                ////Lấy toàn bộ Style có sẵn:
                //var dictionaryStyles = new Dictionary<string, System.Xml.XmlNode>();
                //foreach (System.Xml.XmlNode node in styleDoc.ChildNodes)
                //{
                //    if (node.Name == "w:styles")
                //    {
                //        foreach (System.Xml.XmlNode styleNode in node.ChildNodes)
                //        {
                //            if (styleNode.Name == "w:style")
                //            {
                //                //Lấy toàn bộ Style có sẵn:
                //                var styleId = this.GetAttributeInNode(styleNode, "w:styleId");
                //                if (!string.IsNullOrEmpty(styleId) && !dictionaryStyles.ContainsKey(styleId))
                //                    dictionaryStyles.Add(styleId, styleNode);
                //            }
                //        }
                //        int colorStyle = 0;
                //        foreach (var paragraphStyle in video.ParagraphStyleList.OrderBy(m => m.Name))
                //        {
                //            colorStyle++;
                //            if (string.IsNullOrEmpty(paragraphStyle.Name))
                //                continue;
                //            if (paragraphStyle.Name == "docDefaults")
                //            {
                //                //Gán giá trị cho style có sẵn
                //                foreach (System.Xml.XmlNode stylesNode in styleDoc.ChildNodes)
                //                {
                //                    if (stylesNode.Name == "w:styles" && stylesNode.FirstChild != null && stylesNode.FirstChild.Name == "w:docDefaults")
                //                    {
                //                        foreach (System.Xml.XmlNode nodeDefault in node.FirstChild.ChildNodes)
                //                        {
                //                            if (nodeDefault.Name == "w:rPrDefault")
                //                            {
                //                                //Gán style cho char
                //                                if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:rPr")
                //                                {
                //                                    SetStyleForExistStyle(nodeDefault.FirstChild, paragraphStyle, video, styleDoc);
                //                                }
                //                            }
                //                            else if (nodeDefault.Name == "w:pPrDefault")
                //                            {
                //                                //Gán style cho paragraph
                //                                if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:pPr")
                //                                {
                //                                    SetStyleForExistStyle(nodeDefault.FirstChild, paragraphStyle, video, styleDoc);
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //            //2023-06-22: Tên Style để là S01, S02 -> S99. Trường hợp khác cầu trúc và trong file có nhiều Style hơn 99 thì không hỗ trợ
                //            //2023-07-04: Chốt Sytle: Tất cả style trên TVOS sẽ bị ghi đè nếu trùng tên (Xóa Style cũ và tạo Style mới)
                //            //if (paragraphStyle.Name.Length == 3 && paragraphStyle.Name.StartsWith("S") && char.IsNumber(paragraphStyle.Name[1]) && char.IsNumber(paragraphStyle.Name[2]))
                //            string paragraphStyleName = "";
                //            string charStyleName = "";
                //            if (paragraphStyle.Name.EndsWith("Char"))
                //            {
                //                paragraphStyleName = paragraphStyle.Name.Substring(0, paragraphStyle.Name.Length - 4);
                //                charStyleName = paragraphStyle.Name;
                //            }
                //            else
                //            {
                //                paragraphStyleName = paragraphStyle.Name;
                //                charStyleName = paragraphStyle.Name + "Char";
                //            }
                //            if (!dictionaryStyles.ContainsKey(paragraphStyleName) && !dictionaryStyles.ContainsKey(charStyleName))
                //            {
                //                //Tạo style mới
                //                var styleParagraphNode = styleDoc.CreateElement("w", "style", node.NamespaceURI);
                //                var resultParagraphNode = node.AppendChild(styleParagraphNode);
                //                AddAttributeInNode(styleParagraphNode, "w:type", "paragraph");
                //                AddAttributeInNode(styleParagraphNode, "w:customStyle", "1");
                //                AddAttributeInNode(styleParagraphNode, "w:styleId", paragraphStyle.Name);

                //                styleParagraphNode.InnerXml += "<w:name w:val=\"" + paragraphStyle.Name + "\"/>";
                //                styleParagraphNode.InnerXml += "<w:basedOn w:val=\"Normal\"/>";
                //                styleParagraphNode.InnerXml += "<w:link w:val=\"" + paragraphStyle.Name + "Char\"/>";
                //                styleParagraphNode.InnerXml += "<w:qFormat/>";
                //                styleParagraphNode.InnerXml += "<w:rsid w:val=\"00EF1B70\"/>";

                //                //2023-06-28: Hỗ trợ BusinessObjects.Alignment, Indentation, Spacing
                //                if (includesSpacingIndentationAlignmentStyleInWordDocument)
                //                {
                //                    var pPrNode = styleDoc.CreateElement("w:pPr", node.NamespaceURI);
                //                    //pPrNode.InnerXml += paragraphStyle.Spacing;
                //                    //pPrNode.InnerXml += paragraphStyle.Indentation;
                //                    //2023-06-29
                //                    //pPrNode.InnerXml += paragraphStyle.Alignment;
                //                    if (video.Alignment && paragraphStyle.Alignment != BusinessObjects.Alignment.Empty)
                //                    {
                //                        var alignmentNode = styleDoc.CreateElement("w:jc", node.NamespaceURI);
                //                        if (paragraphStyle.Alignment == BusinessObjects.Alignment.Left)
                //                            AddAttributeInNode(alignmentNode, "w:val", "left");
                //                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Right)
                //                            AddAttributeInNode(alignmentNode, "w:val", "right");
                //                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Centered)
                //                            AddAttributeInNode(alignmentNode, "w:val", "center");
                //                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Justified)
                //                            AddAttributeInNode(alignmentNode, "w:val", "both");
                //                        pPrNode.AppendChild(alignmentNode);
                //                    }
                //                    styleParagraphNode.AppendChild(pPrNode);
                //                    if (video.Outline && paragraphStyle.Outline != null && paragraphStyle.Outline != 0)
                //                        pPrNode.InnerXml += string.Format("<w:outlineLvl w:val=\"{0:n0}\"/>", paragraphStyle.Outline - 1);
                //                }

                //                //Style char
                //                var styleCharNode = styleDoc.CreateElement("w", "style", node.NamespaceURI);

                //                var resultStyleNode = node.AppendChild(styleCharNode);
                //                AddAttributeInNode(styleCharNode, "w:type", "character");
                //                AddAttributeInNode(styleCharNode, "w:customStyle", "1");
                //                AddAttributeInNode(styleCharNode, "w:styleId", paragraphStyle.Name + "Char");

                //                styleCharNode.InnerXml += "<w:name w:val=\"" + paragraphStyle.Name + " Char\"/>";
                //                styleCharNode.InnerXml += "<w:basedOn w:val=\"DefaultParagraphFont\"/>";
                //                styleCharNode.InnerXml += "<w:link w:val=\"" + paragraphStyle.Name + "\"/>";
                //                styleCharNode.InnerXml += "<w:rsid w:val=\"00EF1B70\"/>";

                //                var rPrNode = styleDoc.CreateElement("w:rPr", node.NamespaceURI);
                //                styleParagraphNode.AppendChild(rPrNode);

                //                string font = string.IsNullOrEmpty(paragraphStyle.TranslateFont) ? paragraphStyle.Font : paragraphStyle.TranslateFont;
                //                if (!string.IsNullOrEmpty(font))
                //                    rPrNode.InnerXml += string.Format("<w:rFonts w:ascii=\"{0}\" w:eastAsia=\"Times New Roman\" w:hAnsi=\"{0}\" w:cs=\"Helvetica\"/>", font);
                //                var rPrNodeChar = rPrNode.Clone();
                //                styleCharNode.AppendChild(rPrNodeChar);
                //                if (video.FontBold && paragraphStyle.Bold)
                //                {
                //                    rPrNode.InnerXml += "<w:b /><w:bCs />";
                //                    //rPrNodeChar.InnerXml += "<w:b w:val=\"0\"/><w:bCs />";
                //                    rPrNodeChar.InnerXml += "<w:b/>";
                //                }
                //                if (video.FontItalic && paragraphStyle.Italic)
                //                {
                //                    rPrNode.InnerXml += "<w:i /><w:iCs />";
                //                    //rPrNodeChar.InnerXml += "<w:i w:val=\"0\"/><w:iCs />";
                //                    rPrNodeChar.InnerXml += "<w:i/>";
                //                }
                //                if (video.FontUnderline && paragraphStyle.Underline)
                //                {
                //                    rPrNode.InnerXml += "<w:u w:val=\"single\" />";
                //                    rPrNodeChar.InnerXml += "<w:u w:val=\"single\" />";
                //                }
                //                if (video.FontColor && paragraphStyle.Color != null)
                //                {
                //                    //rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\" w:themeColor=\"accent{1:0}\" w:themeShade=\"BF\" />", paragraphStyle.Color.Value.Name.ToString(), colorStyle);
                //                    string hexColor = ColorTranslator.ToHtml(paragraphStyle.Color.Value);
                //                    //Bỏ dấu # ở đầu
                //                    hexColor = hexColor.Substring(1);
                //                    rPrNode.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                //                    rPrNodeChar.InnerXml += string.Format("<w:color w:val=\"{0}\"/>", hexColor, colorStyle);
                //                }
                //                if (paragraphStyle.Size != null)
                //                {
                //                    rPrNode.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                //                    rPrNodeChar.InnerXml += string.Format("<w:sz w:val=\"{0:n0}\"/>", paragraphStyle.Size * 2);
                //                }
                //            }
                //            else
                //            {
                //                //Thay đổi cấu trúc style sẵn có
                //                foreach (var wStyleName in dictionaryStyles.Keys)
                //                {
                //                    if (wStyleName == paragraphStyleName || wStyleName == charStyleName)
                //                    {
                //                        foreach (System.Xml.XmlNode fontNode in dictionaryStyles[wStyleName].ChildNodes)
                //                        {
                //                            SetStyleForExistStyle(fontNode, paragraphStyle, video, styleDoc);
                //                        }
                //                    }
                //                }

                //            }

                //        }

                //    }
                //}
                //styleDoc.Save(xmlStyleFile);

            }
            char prefix = 'a';

            //Gán style cho theme
            //-Title Font: majorFont
            //- Body Font: minorFont                    
            var xmlThemeFile = tempFolder + "\\ppt\\theme\\theme1.xml";
            System.Xml.XmlDocument themeDoc = new System.Xml.XmlDocument();
            themeDoc.Load(xmlThemeFile);

            foreach (System.Xml.XmlNode node in themeDoc.ChildNodes)
            {
                if (node.Name == prefix + ":theme" && node.FirstChild != null && node.FirstChild.Name == prefix + ":themeElements")
                {
                    foreach (System.Xml.XmlNode fontScheme in node.FirstChild.ChildNodes)
                    {
                        if (fontScheme.Name == prefix + ":fontScheme")
                        {
                            foreach (System.Xml.XmlNode fontNode in fontScheme.ChildNodes)
                            {
                                if (fontNode.Name == prefix + ":majorFont")
                                {
                                    if (fontNode.FirstChild != null && fontNode.FirstChild.Name == prefix + ":latin")
                                    {
                                        var titleFontParagraphStyle = video.ParagraphStyleList.FirstOrDefault(m => m.Name == "Title Font");
                                        if (titleFontParagraphStyle != null)
                                        {
                                            SetNodeAttribute(fontNode.FirstChild, "typeface", titleFontParagraphStyle.Font);
                                        }
                                    }
                                }
                                else if (fontNode.Name == prefix + ":minorFont")
                                {
                                    if (fontNode.FirstChild != null && fontNode.FirstChild.Name == prefix + ":latin")
                                    {
                                        var titleFontParagraphStyle = video.ParagraphStyleList.FirstOrDefault(m => m.Name == "Body Font");
                                        if (titleFontParagraphStyle != null)
                                        {
                                            SetNodeAttribute(fontNode.FirstChild, "typeface", titleFontParagraphStyle.Font);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }


            var slidesRefFolder = tempFolder + "\\ppt\\slides\\_rels";
            string slideRefLayoutFolder = tempFolder + "\\ppt\\slideLayouts\\_rels\\";
            var slideRefNames = System.IO.Directory.GetFiles(slidesRefFolder, "*.rels");
            var slideRefNamesWithSort = slideRefNames.OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
                                    .ThenBy(x =>
                                    {
                                        int number;
                                        if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                                            return number;
                                        return -1;
                                    }).ToList();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            var slideLayoutNames = new System.Collections.Generic.List<string>();
            var dictionarySlides = new System.Collections.Generic.Dictionary<string, string>();
            var dictionarySlideLayoutsWithLayouts = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, ParagraphStyle>>();
            var slideLayoutSlideMasterDictionary = new System.Collections.Generic.Dictionary<string, string>();
            var slideMasterLayout = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ParagraphStyle>>();

            foreach (var slideRefName in slideRefNamesWithSort)
            {
                doc.Load(slideRefName);
                var relationshipNodes = doc.GetElementsByTagName("Relationship");
                if (relationshipNodes.Count > 0)
                {
                    var slideLayoutName = GetAttributeInNode(relationshipNodes[0], "Target");
                    if (!string.IsNullOrEmpty(slideLayoutName))
                    {
                        slideLayoutName = slideLayoutName.Replace("../slideLayouts/", "");
                        if (!slideLayoutNames.Contains(slideLayoutName))
                        {
                            slideLayoutNames.Add(slideLayoutName);
                            dictionarySlideLayoutsWithLayouts.Add(slideLayoutName, new System.Collections.Generic.Dictionary<string, ParagraphStyle>());
                            var slideLayoutMasterFile = slideRefLayoutFolder + slideLayoutName + ".rels";
                            if (System.IO.File.Exists(slideLayoutMasterFile))
                            {
                                doc.Load(slideLayoutMasterFile);
                                relationshipNodes = doc.GetElementsByTagName("Relationship");
                                if (relationshipNodes.Count > 0)
                                {
                                    var slideMasterName = GetAttributeInNode(relationshipNodes[0], "Target");
                                    if (!string.IsNullOrEmpty(slideMasterName))
                                    {
                                        slideMasterName = slideMasterName.Replace("../slideMasters/", "");
                                        if (!slideLayoutSlideMasterDictionary.ContainsKey(slideLayoutName))
                                        {
                                            slideLayoutSlideMasterDictionary.Add(slideLayoutName, slideMasterName);
                                            if (!slideMasterLayout.ContainsKey(slideMasterName))
                                            {
                                                slideMasterLayout.Add(slideMasterName, new System.Collections.Generic.List<ParagraphStyle>());
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        string slideName = slideRefName.Substring(slideRefName.LastIndexOf('\\') + 1).Replace(".rels", "");
                        dictionarySlides.Add(slideName, slideLayoutName);
                    }
                }
            }
            //Gán dữ liệu cho slideMaster
            string slideMasterFolder = tempFolder + "\\ppt\\slideMasters\\";
            foreach (var slideMasterName in slideMasterLayout.Keys)
            {
                var slideLayoutMasterFile = slideMasterFolder + slideMasterName;
                if (System.IO.File.Exists(slideLayoutMasterFile))
                {
                    doc.Load(slideLayoutMasterFile);
                    var txStylesNodes = doc.GetElementsByTagName("p:txStyles");
                    if (txStylesNodes.Count > 0)
                    {
                        string masterName = "SM" + slideMasterName.Replace("slideMaster", "").Replace(".xml", "");
                        foreach (System.Xml.XmlNode lstStyleNode in txStylesNodes[0].ChildNodes)
                        {
                            if (lstStyleNode.Name == "p:titleStyle")
                            {
                                if (lstStyleNode.FirstChild != null && lstStyleNode.FirstChild.Name == "a:lvl1pPr")
                                {
                                    string styleName = masterName + ".title";
                                    var paragraphStyle = video.ParagraphStyleList.FirstOrDefault(m => m.Name == styleName);
                                    if (paragraphStyle != null)
                                        SetLvlpPrNodeFromParagraphStyle(lstStyleNode.FirstChild, paragraphStyle, video, bookMark);
                                }

                            }
                            else if (lstStyleNode.Name == "p:bodyStyle")
                            {
                                foreach (System.Xml.XmlNode lvlpPrNode in lstStyleNode.ChildNodes)
                                {
                                    if (lvlpPrNode.Name.StartsWith("a:lvl") && lvlpPrNode.Name.EndsWith("pPr"))
                                    {
                                        int level = System.Convert.ToInt32(lvlpPrNode.Name.Replace("a:lvl", "").Replace("pPr", ""));
                                        string styleName = masterName + ".body";
                                        if (level > 1)
                                            styleName += ".LV" + (level - 1);
                                        var paragraphStyle = video.ParagraphStyleList.FirstOrDefault(m => m.Name == styleName);
                                        if (paragraphStyle != null)
                                            SetLvlpPrNodeFromParagraphStyle(lstStyleNode.FirstChild, paragraphStyle, video, bookMark);
                                    }
                                }

                            }
                        }
                    }

                }
            }
            decimal countNumber = 0;
            int index = 0;
            System.Collections.Generic.IDictionary<System.Xml.XmlNode, bool> flagNodes = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, bool>();
            foreach (var xmlSlide in dictionarySlides.Keys)
            {
                var xmlFile = tempFolder + "\\ppt\\slides\\" + xmlSlide;
                if (!System.IO.File.Exists(xmlFile))
                    continue;

                doc.Load(xmlFile);

                //order = 1;
                //Lấy danh sách nội dung đã dịch                                                                       
                var dictionaryAudio = new System.Collections.Generic.Dictionary<int, Audio>();
                var audioList = video.GetAudioListWithSort(bookMark, true, null);
                //Tối ưu file word

                //ReplaceNodeContent(rootNode);
                //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt

                if (useUpperElement && !video.UpperElementImport)
                {
                    //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
                    audioList = audioList.Where(m => m.UpperElement != null).ToList();
                }
                else
                {
                    if (video.UpperElementImport)
                    {
                        var listParentNotImport = new System.Collections.Generic.List<System.Guid>();
                        foreach (var audio in audioList)
                        {
                            if (audio.UpperElement != null && !listParentNotImport.Contains(audio.UpperElement.Oid))
                                listParentNotImport.Add(audio.UpperElement.Oid);
                        }
                        audioList = audioList.Where(m => !listParentNotImport.Contains(m.Oid)).ToList();
                    }
                    //Tối ưu hóa file doc
                    ShowWaitForm("Đang tối ưu hóa tập tin", " ");

                    OptimalDocument(video, doc, flagNodes, prefix);
                    //if (xmlSlide == "slide45.xml" || xmlSlide == "slide47.xml")
                    //{

                    //}
                    ShowWaitForm(null, null);
                    //2023-06-27: Chỉnh margin của file word thành Normal                                
                }

                for (int i = 0; i < audioList.Count; i++)
                {
                    dictionaryAudio.Add(i + 1, audioList[i]);
                }


                //2023-06-14: Thay thế theo w:p                        
                //ReplaceNodeContent(rootNode, dictionaryAudio);
                //Thay thế theo w:t đã optimal                   
                var wtNodes = doc.GetElementsByTagName(prefix + ":t");
                //Xử lý ghép nội dung
                System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>> parentNodeList = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>>();
                foreach (System.Xml.XmlNode node in wtNodes)
                {
                    var parentNode = GetParentNode(node, prefix + ":p");
                    if (parentNode != null)
                    {
                        if (parentNodeList.ContainsKey(parentNode))
                            parentNodeList[parentNode].Add(node);
                        else
                            parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                    }
                    else
                    {
                        if (parentNodeList.ContainsKey(node))
                            parentNodeList[node].Add(node);
                        else
                            parentNodeList.Add(node, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                    }
                }

                foreach (var keyNode in parentNodeList.Keys)
                {
                    var nodeList = parentNodeList[keyNode];
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        if (!WtContentIsValidate(video, nodeList[i]))
                            continue;
                        index++;
                        if (dictionaryAudio.ContainsKey(index))
                        {
                            //2023-07-20: Xóa trường được đánh dấu xóa
                            if (dictionaryAudio[index].Delete)
                            {
                                if (nodeList.Count == 1)
                                {
                                    if (keyNode.ParentNode != null)
                                        keyNode.ParentNode.RemoveChild(keyNode);
                                    else
                                        keyNode.RemoveAll();
                                }
                                else
                                {
                                    if (nodeList[i].ParentNode != null)
                                        nodeList[i].ParentNode.RemoveChild(nodeList[i]);
                                    else
                                        nodeList[i].RemoveAll();
                                }
                                continue;
                            }
                            //2023-08-24: Những Paragraph dựng cờ (Có check TextNode) sẽ xuất TextNode còn không thì xuất kiểu cũ
                            bool notMergeNode = dictionaryAudio[index].UpperElement != null && !dictionaryAudio[index].UpperElement.TextNode;
                            //2023-08-24 Nếu là nạp theo đoạn có parent thì tương đương có ghép
                            if (notMergeNode && !video.UpperElementImport)
                            {
                                //Nạp theo nốt
                                //Trường hợp node i đầu tiên không hợp lệ, i có thể là các node khác
                                if (choice.Contains("TranslateDocument"))
                                    nodeList[i].InnerText = dictionaryAudio[index].UpperElement.Subtitle;
                                else if (choice.Contains("ContentDocument"))
                                    nodeList[i].InnerText = dictionaryAudio[index].UpperElement.Content;
                                //Xóa các textNode sau đó                                        
                                if (nodeList.Count > 1)
                                {
                                    var totalChild = audioList.Where(m => m.UpperElement != null && m.UpperElement.Oid.Equals(dictionaryAudio[index].UpperElement.Oid)).Count();
                                    //ChildValidate + thêm hiện tại
                                    int childValidate = 1;
                                    for (int j = i + 1; j < nodeList.Count; j++)
                                    {
                                        if (WtContentIsValidate(video, nodeList[j]))
                                        {
                                            if (childValidate > totalChild)
                                            {
                                                //Nếu trường hợp này xảy ra thì là bị lỗi
                                            }
                                            //Trường hợp node i đầu tiên không hợp lệ, i có thể là các node khác                                                    
                                            index++;
                                            childValidate++;
                                        }
                                        if (nodeList[j].ParentNode != null)
                                            nodeList[j].ParentNode.RemoveChild(nodeList[j]);
                                        //Nếu ghép xong thì bỏ
                                        //if (childValidate == totalChild)
                                        //    break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                //Nạp theo đoạn
                                //Thay thế nội dung đã dịch
                                if (choice.Contains("TranslateDocument"))
                                {
                                    //if (System.Diagnostics.Debugger.IsAttached)
                                    //    nodeList[i].InnerText = dictionaryAudio[index].Subtitle.Length <= nodeList[i].InnerText.Length ? dictionaryAudio[index].Subtitle : dictionaryAudio[index].Subtitle.Substring(0, nodeList[i].InnerText.Length);
                                    //else
                                    nodeList[i].InnerText = dictionaryAudio[index].Subtitle;
                                }

                                else if (choice.Contains("ContentDocument"))
                                    nodeList[i].InnerText = dictionaryAudio[index].Content;
                                //Gán style trường hợp Style tự tạo
                                //2023-08-24: Xuất tư liệu: Nhận dạng tư liệu được nạp theo Đoạn hay theo Nốt(tồn tại TextNode) từ đó Xuất nguyên vẹn Style các nốt
                                int numberStyle = 0;
                                //Nếu là style tự tạo (nằm trên node thì mới sửa style
                                if (dictionaryAudio[index].ParagraphStyle != null && System.Int32.TryParse(dictionaryAudio[index].ParagraphStyle.Name, out numberStyle))
                                {
                                    if (video.OriginStyleExport)
                                    {
                                        //Giữ style trên text Node
                                        if (nodeList[i].PreviousSibling != null && nodeList[i].PreviousSibling.Name == prefix + ":rPr")
                                        {
                                            //Gán font trực tiếp vào text node
                                            //SetStyleForExistStyle(nodeList[i].PreviousSibling, dictionaryAudio[index].ParagraphStyle, video, doc, prefix);
                                            SetStyleInNode(nodeList[i].PreviousSibling, dictionaryAudio[index].ParagraphStyle, video, false, prefix);
                                        }

                                    }
                                    else if (!string.IsNullOrEmpty(dictionaryAudio[index].ParagraphStyle.Name))
                                    {
                                        bool applyPStyle = parentNodeList[keyNode].Count == 1;
                                        if (keyNode.Name == prefix + ":p")
                                        {
                                            //Trường hợp áp dụng cho paragraph
                                            System.Xml.XmlNode pPrNode = null;
                                            foreach (System.Xml.XmlNode childNode in keyNode.ChildNodes)
                                            {
                                                if (childNode.Name == prefix + ":pPr")
                                                {
                                                    pPrNode = childNode;
                                                    break;
                                                }
                                            }
                                            if (pPrNode == null)
                                            {
                                                pPrNode = keyNode.OwnerDocument.CreateElement(prefix.ToString(), "pPr", keyNode.NamespaceURI);
                                                keyNode.PrependChild(pPrNode);
                                            }
                                            SetNodeFromParagraphStyle(pPrNode, dictionaryAudio[index].ParagraphStyle, video, null, bookMark, false, prefix);
                                        }

                                        //Trường hợp áp dụng cho từ
                                        if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.Name == prefix + ":r")
                                        {
                                            System.Xml.XmlNode rPrNode = null;
                                            foreach (System.Xml.XmlNode childNode in nodeList[i].ParentNode.ChildNodes)
                                            {
                                                if (childNode.Name == prefix + ":rPr")
                                                {
                                                    rPrNode = childNode;
                                                    break;
                                                }
                                            }
                                            if (rPrNode == null)
                                                rPrNode = nodeList[i].OwnerDocument.CreateElement("w", "rPr", nodeList[i].NamespaceURI);
                                            SetStyleInNode(rPrNode, dictionaryAudio[index].ParagraphStyle, video, false, prefix);
                                        }

                                    }
                                }

                            }
                        }
                    }
                    var percent = (Convert.ToDecimal(index) / wtNodes.Count).ToString("p0");
                    ShowWaitForm(percent, " ");
                }
                //Ghi đề file xml

                //Lưu lại thành file word
                doc.Save(xmlFile);

                //Xóa file đã có
                if (System.IO.File.Exists(saveFileName))
                {
                    System.IO.File.Delete(saveFileName);
                }
            }
            System.IO.Compression.ZipFile.CreateFromDirectory(tempFolder, saveFileName);
            ShowWaitForm(null, null);
            //Xóa thư mục bộ nhớ tạm
            if (!System.Diagnostics.Debugger.IsAttached)
                System.IO.Directory.Delete(tempFolder, true);
            if (bookMark != null)
                bookMark.SetBookMarkNote(saveFileName);
            return saveFileName;
            //Mở kết quả
            //if (MessageBox.Show("Bạn có muốn mở kết quả không?", "Thành công", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //    startInfo.UseShellExecute = true;
            //    startInfo.FileName = saveFileName;
            //    System.Diagnostics.Process.Start(startInfo);
            //}
        }

        private bool? SetNodeAttribute(System.Xml.XmlNode node, string name, string value)
        {
            if (node is null)
                return null;
            foreach (System.Xml.XmlAttribute att in node.Attributes)
            {
                if (att.Name == name)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (att.Value == value)
                            return false;
                        else
                        {
                            att.Value = value;
                            return true;
                        }

                    }
                    else
                    {
                        //Nếu giá trị là trống thì xóa thuộc tính
                        node.Attributes.Remove(att);
                        return true;
                    }

                }
            }
            if (!string.IsNullOrEmpty(value))
            {
                //Nếu không tồn tại thuộc tính thì bổ sung thêm 
                return AddAttributeInNode(node, name, value) != null;
            }
            return null;
        }
        private bool SetLvlpPrNodeFromParagraphStyle(System.Xml.XmlNode lvlpPrNode, ParagraphStyle paragraphStyle, Video video, BookMark bookMark)
        {
            bool hasValue = SetNodeFromParagraphStyle(lvlpPrNode, paragraphStyle, video, null, bookMark, false, 'a');
            foreach (System.Xml.XmlNode defRPrNode in lvlpPrNode.ChildNodes)
            {
                if (defRPrNode.Name == "a:defRPr")
                {
                    if (SetStyleInNode(defRPrNode, paragraphStyle, video, false, 'a'))
                        hasValue = true;
                }
                else if (defRPrNode.Name == "a:buFont")
                {
                    if (SetNodeAttribute(defRPrNode, "typeface", paragraphStyle.Font) == true)
                        hasValue = true;
                }
            }
            return hasValue;
        }
        private bool SetStyleInNode(System.Xml.XmlNode fontNode, ParagraphStyle paragraphStyle, Video video, bool overrideName = false, char prefix = 'w')
        {
            bool hasValue = false;
            if (fontNode.Name == prefix + ":rPr" || fontNode.Name == prefix + ":defRPr")
            {
                bool changedFont = false;
                System.Xml.XmlNode solidFillNode = null;
                System.Xml.XmlNode srgbClrNode = null;
                bool changedFontColor = false;
                bool changeSize = false;
                bool changeBold = false;
                bool changeItalic = false;
                bool changeUnderline = false;

                if (prefix == 'a')
                {
                    //Nap PowerPoint
                    string font = string.IsNullOrEmpty(paragraphStyle.TranslateFont) ? paragraphStyle.Font : paragraphStyle.TranslateFont;
                    foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                    {
                        if (styleNode.Name == prefix + ":cs" || styleNode.Name == prefix + ":latin")
                        {
                            if (string.IsNullOrEmpty(font))
                                continue;
                            if (!string.IsNullOrEmpty(font))
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == "typeface")
                                    {
                                        if (string.IsNullOrEmpty(att.Value))
                                            continue;
                                        if (att.Value.StartsWith("+m") && att.Value.Contains("-"))
                                            continue;
                                        //Word
                                        if (att.Value != font)
                                            att.Value = font;
                                        hasValue = true;
                                        //break;
                                        changedFont = true;
                                    }
                                }
                        }
                        else if (styleNode.Name == prefix + ":solidFill")
                        {
                            if (video.FontColor)
                            {
                                foreach (System.Xml.XmlNode childNode in styleNode.ChildNodes)
                                {
                                    if (childNode.Name == prefix + ":srgbClr")
                                    {
                                        foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                                        {
                                            if (att.Name == "val")
                                            {
                                                string hexColor = System.Drawing.ColorTranslator.ToHtml(paragraphStyle.Color.Value);
                                                //Bỏ dấu # ở đầu
                                                hexColor = hexColor.Substring(1);
                                                if (att.Value != hexColor)
                                                {
                                                    att.Value = hexColor;
                                                }

                                                hasValue = true;
                                                changedFontColor = true;
                                                break;

                                            }
                                        }
                                        srgbClrNode = childNode;
                                    }
                                }
                                solidFillNode = styleNode;

                            }

                        }
                    }
                    if (!changedFont && !string.IsNullOrEmpty(font))
                    {
                        //Tạo mới Font nếu trống
                        var latinNode = fontNode.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, "a:latin", fontNode.NamespaceURI);
                        AddAttributeInNode(latinNode, "typeface", font);
                        fontNode.AppendChild(latinNode);

                        var csNode = fontNode.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, "a:cs", fontNode.NamespaceURI);
                        AddAttributeInNode(csNode, "typeface", font);
                        fontNode.AppendChild(csNode);
                    }
                    if (!changedFontColor && video.FontColor && paragraphStyle.Color != null)
                    {
                        //Tạo mới Font nếu trống
                        if (solidFillNode is null)
                        {
                            solidFillNode = fontNode.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, "a:solidFill", fontNode.NamespaceURI);
                            fontNode.AppendChild(solidFillNode);
                        }
                        if (srgbClrNode is null)
                        {
                            srgbClrNode = fontNode.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, "a:srgbClr", fontNode.NamespaceURI);
                            solidFillNode.AppendChild(srgbClrNode);
                        }
                        AddAttributeInNode(srgbClrNode, "val", System.Drawing.ColorTranslator.ToHtml(paragraphStyle.Color.Value).Substring(1));
                    }
                    for (int a = fontNode.Attributes.Count - 1; a >= 0; a--)
                    {
                        var fontAttribute = fontNode.Attributes[a];
                        if (fontAttribute.Name == "sz")
                        {
                            if (paragraphStyle.Size != null)
                            {
                                //paragraphStyle.Size = Convert.ToDecimal(fontAttribute.Value) / 100;
                                var value = System.Convert.ToInt32(paragraphStyle.Size.Value * 100).ToString("D");
                                if (fontAttribute.Value != value)
                                    fontAttribute.Value = value;
                            }
                            else
                            {
                                fontNode.Attributes.Remove(fontAttribute);
                            }
                            changeSize = true;
                            hasValue = true;
                        }
                        else if (fontAttribute.Name == "b")
                        {
                            if (video.FontBold)
                            {
                                if (paragraphStyle.Bold)
                                {
                                    if (fontAttribute.Value == "0")
                                        fontAttribute.Value = "1";
                                }
                                else
                                {
                                    if (fontAttribute.Value is null)
                                        fontNode.Attributes.Remove(fontAttribute);
                                    if (fontAttribute.Value == "1")
                                        fontAttribute.Value = "0";
                                }
                            }
                            changeBold = true;
                            hasValue = true;
                        }
                        else if (fontAttribute.Name == "i")
                        {
                            if (video.FontItalic)
                            {
                                if (paragraphStyle.Italic)
                                {
                                    if (fontAttribute.Value == "0")
                                        fontAttribute.Value = "1";
                                }
                                else
                                {
                                    if (fontAttribute.Value is null)
                                        fontNode.Attributes.Remove(fontAttribute);
                                    if (fontAttribute.Value == "1")
                                        fontAttribute.Value = "0";
                                }
                            }
                            changeItalic = true;
                            hasValue = true;
                        }
                        else if (fontAttribute.Name == "u")
                        {
                            if (video.FontUnderline)
                            {
                                if (paragraphStyle.Underline)
                                {
                                    if (fontAttribute.Value != null && fontAttribute.Value != "heavy")
                                        fontAttribute.Value = "sng";
                                }
                                else
                                {
                                    if (fontAttribute.Value != "sng")
                                        fontNode.Attributes.Remove(fontAttribute);
                                }
                            }
                            changeUnderline = true;
                            hasValue = true;
                        }

                    }
                    if (!changeSize && paragraphStyle.Size != null)
                    {
                        AddAttributeInNode(fontNode, "sz", System.Convert.ToInt32(paragraphStyle.Size.Value * 100).ToString("D"));
                    }
                    if (!changeBold && paragraphStyle.Bold)
                    {
                        AddAttributeInNode(fontNode, "b", "1");
                    }
                    if (!changeItalic && paragraphStyle.Italic)
                    {
                        AddAttributeInNode(fontNode, "i", "1");
                    }
                    if (!changeUnderline && paragraphStyle.Underline)
                    {
                        AddAttributeInNode(fontNode, "u", "sng");
                    }
                }
                //Trương hợp style cho word xử lý riêng
                //else
                //{
                //    //Nạp Word
                //    foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                //    {
                //        if (styleNode.Name == prefix + ":rFonts")
                //        {
                //            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                //            {
                //                if (att.Name == prefix + ":ascii" || att.Name == prefix + ":cstheme")
                //                {
                //                    //Word
                //                    paragraphStyle.Font = att.Value;
                //                    hasValue = true; ;
                //                    break;
                //                }
                //                //else if (att.Name == prefix + ":eastAsia")
                //                //{
                //                //    paragraphStyle.Font = att.Value;
                //                //    break;
                //                //}
                //            }
                //        }
                //        else if (styleNode.Name == prefix + ":color")
                //        {
                //            if (video.FontColor)
                //            {
                //                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                //                {
                //                    if (att.Name == prefix + ":val")
                //                    {
                //                        if (att.Value.Equals("auto"))
                //                        {
                //                            //Màu automatic (màu đen) trong word
                //                        }
                //                        else
                //                        {
                //                            paragraphStyle.Color = System.Drawing.ColorTranslator.FromHtml("#" + att.Value);
                //                            hasValue = true;
                //                        }
                //                        break;

                //                    }
                //                }
                //            }

                //        }
                //        else if (styleNode.Name == prefix + ":sz")
                //        {
                //            string attValue = paragraphStyle.Size is null ? null : (paragraphStyle.Size.Value * 2).ToString("D");
                //            if (SetNodeAttribute(styleNode, prefix + ":sz", attValue) == true)
                //                hasValue = true;                            
                //        }
                //        else if (styleNode.Name == prefix + ":b")
                //        {
                //            if (video.FontBold)
                //            {
                //                if (SetNodeAttribute(styleNode, prefix + ":b", paragraphStyle.Bold ? "1" : null) == true)
                //                    hasValue = true;                                
                //            }
                //        }
                //        else if (styleNode.Name == prefix + ":i")
                //        {
                //            if (video.FontItalic)
                //            {
                //                if (SetNodeAttribute(styleNode, prefix + ":i", paragraphStyle.Bold ? "1" : null) == true)
                //                    hasValue = true;                                
                //            }
                //        }
                //        else if (styleNode.Name == prefix + ":u")
                //        {
                //            if (video.FontUnderline)
                //            {
                //                if (SetNodeAttribute(styleNode, prefix + ":i", paragraphStyle.Bold ? "1" : "none") == true)
                //                    hasValue = true;

                //            }
                //        }                        
                //    }
                //}

            }
            return hasValue;
        }
        private bool SetNodeFromParagraphStyle(System.Xml.XmlNode paragraphNodeStyle, ParagraphStyle paragraphStyle, Video video, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, BookMark bookMark, bool overrideName = false, char prefix = 'w')
        {
            bool hasValue = false;
            if (paragraphNodeStyle.Name == prefix + ":pPr" || (paragraphNodeStyle.Name.StartsWith(prefix + ":lvl") && paragraphNodeStyle.Name.EndsWith("pPr")))
            {
                if (prefix == 'a')
                {
                    //Power Point
                    if (video.Alignment)
                    {
                        string value = "";
                        if (paragraphStyle.Alignment == BusinessObjects.Alignment.Left)
                        {
                            value = "1";
                        }
                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Right)
                        {
                            value = "r";
                        }
                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Centered)
                        {
                            value = "ctr";
                            //Nếu trống mặc địch là căn giữa
                        }
                        else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Justified)
                        {
                            value = "just";
                        }
                        if (SetNodeAttribute(paragraphNodeStyle, "algn", value) == true)
                        {
                            hasValue = true;
                        }
                    }
                    if (video.Indent)
                    {
                        string valueIndent = paragraphStyle.IndentLeft != null ? System.Convert.ToInt32(paragraphStyle.IndentLeft.Value * 360000).ToString("D") : null;
                        if (SetNodeAttribute(paragraphNodeStyle, "marL", valueIndent) == true)
                        {
                            hasValue = true;
                        }
                        string valueIndentFirstLine = paragraphStyle.IndentFirstLine != null ? System.Convert.ToInt32(paragraphStyle.IndentFirstLine.Value * 360000).ToString("D") : null;
                        if (SetNodeAttribute(paragraphNodeStyle, "indent", valueIndentFirstLine) == true)
                        {
                            hasValue = true;
                        }
                    }
                    if (video.Outline)
                    {
                        string value = paragraphStyle.Outline != null ? System.Convert.ToInt32(paragraphStyle.Outline.Value - 1).ToString("D") : null;
                        //Nếu là Power Point thì nằm trong cấp của Level                        
                        if (SetNodeAttribute(paragraphNodeStyle, "lvl", value) == true)
                        {
                            if (prefix != 'a')
                                hasValue = true;
                        }
                    }

                    if (video.Spacing)
                    {
                        System.Xml.XmlNode lnSpcNode = null;
                        System.Xml.XmlNode spcBefNode = null;
                        System.Xml.XmlNode spcAftNode = null;

                        bool lnSpc = false;
                        bool spcBef = false;
                        bool spcAft = false;

                        foreach (System.Xml.XmlNode styleNode in paragraphNodeStyle.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":lnSpc" || styleNode.Name == prefix + ":spcBef" || styleNode.Name == prefix + ":spcAft")
                            {
                                if (styleNode.FirstChild != null)
                                {
                                    decimal? value = null;
                                    if (styleNode.Name == prefix + ":lnSpc")
                                    {
                                        //paragraphStyle.SpacingLineAt = result;
                                        value = paragraphStyle.SpacingLineAt;
                                        lnSpcNode = styleNode;
                                    }
                                    else if (styleNode.Name == prefix + ":spcBef")
                                    {
                                        value = paragraphStyle.SpacingBefore;
                                        spcBefNode = styleNode;
                                    }
                                    else if (styleNode.Name == prefix + ":spcAft")
                                    {
                                        value = paragraphStyle.SpacingAfter;
                                        spcAftNode = styleNode;
                                    }
                                    string valueText = "";
                                    if (value != null)
                                    {
                                        if (styleNode.FirstChild.Name == prefix + ":spcPct")
                                        {
                                            //result = Convert.ToDecimal(att.Value) / 120000;
                                            valueText = System.Convert.ToInt32(value.Value * 120000).ToString("D");
                                        }
                                        else if (styleNode.FirstChild.Name == prefix + ":spcPts")
                                        {
                                            //result = Convert.ToDecimal(att.Value) / 100;
                                            valueText = System.Convert.ToInt32(value.Value * 100).ToString("D");
                                        }
                                    }
                                    if (SetNodeAttribute(styleNode.FirstChild, "val", valueText) == true)
                                    {
                                        hasValue = true;
                                        if (styleNode.Name == prefix + ":lnSpc")
                                            lnSpc = true;
                                        else if (styleNode.Name == prefix + ":spcBef")
                                            spcBef = true;
                                        else if (styleNode.Name == prefix + ":spcAft")
                                            spcAft = true;
                                    }
                                }
                            }

                        }
                        //Bổ sung thêm giá trị nếu trống
                        if (paragraphStyle.SpacingLineAt != null && !lnSpc && lnSpcNode is null)
                        {
                            lnSpcNode = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":lnSpc", paragraphNodeStyle.NamespaceURI);
                            paragraphNodeStyle.AppendChild(lnSpcNode);

                            var spcPts = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":spcPts", paragraphNodeStyle.NamespaceURI);
                            lnSpcNode.AppendChild(spcPts);

                            var valueText = System.Convert.ToInt32(paragraphStyle.SpacingLineAt.Value * 100).ToString("D");
                            if (SetNodeAttribute(spcPts, "val", valueText) == true)
                            {

                            }
                        }
                        if (paragraphStyle.SpacingBefore != null && !spcBef && spcBefNode is null)
                        {
                            spcBefNode = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":spcBef", paragraphNodeStyle.NamespaceURI);
                            paragraphNodeStyle.AppendChild(spcBefNode);

                            var spcPts = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":spcPts", paragraphNodeStyle.NamespaceURI);
                            spcBefNode.AppendChild(spcPts);

                            var valueText = System.Convert.ToInt32(paragraphStyle.SpacingBefore.Value * 100).ToString("D");
                            if (SetNodeAttribute(spcPts, "val", valueText) == true)
                            {

                            }
                        }
                        if (paragraphStyle.SpacingAfter != null && !spcAft && spcAftNode is null)
                        {
                            spcAftNode = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":spcAft", paragraphNodeStyle.NamespaceURI);
                            paragraphNodeStyle.AppendChild(spcAftNode);

                            var spcPts = paragraphNodeStyle.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, prefix + ":spcPts", paragraphNodeStyle.NamespaceURI);
                            spcAftNode.AppendChild(spcPts);

                            var valueText = System.Convert.ToInt32(paragraphStyle.SpacingAfter.Value * 100).ToString("D");
                            if (SetNodeAttribute(spcPts, "val", valueText) == true)
                            {

                            }
                        }
                    }

                }
                else
                {
                    //Word
                    foreach (System.Xml.XmlNode styleNode in paragraphNodeStyle.ChildNodes)
                    {
                        if (styleNode.Name == prefix + ":pStyle")
                        {
                            //2023-09-07: Style là style tạo mới, style có sẵn thì nạp sau
                            //Nạp style có thừa kế
                            string styleName = GetAttributeInNode(styleNode);
                            if (!string.IsNullOrEmpty(paragraphStyle.Name))
                            {
                                //Trường hợp nạp style có sẵn
                            }
                            if (string.IsNullOrEmpty(paragraphStyle.Name) && paragraphStyle.UpperStyle is null)
                            {
                                paragraphStyle.UpperStyle = GetDefaultUpperStyle(video, styleName, listExistedParagraphStyle, bookMark);
                            }
                            //if (string.IsNullOrEmpty(paragraphStyle.Name) || overrideName)
                            //{                            
                            //    if (!string.IsNullOrEmpty(paparagraphStyleName))
                            //        paragraphStyle.Name = paparagraphStyleName;
                            //}                        
                        }
                        else if (styleNode.Name == prefix + ":rPr")
                        {
                            if (FillCharStyleNode(video, styleNode, paragraphStyle, listExistedParagraphStyle, bookMark, true, prefix, true))
                                hasValue = true;
                        }
                        else if (styleNode.Name == prefix + ":spacing")
                        {
                            if (video.Spacing)
                            {
                                //paragraphStyle.Spacing = styleNode.OuterXml;
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == prefix + ":before")
                                    {
                                        paragraphStyle.SpacingBefore = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":after")
                                    {
                                        paragraphStyle.SpacingAfter = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":line")
                                    {
                                        paragraphStyle.SpacingLineAt = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":lineRule")
                                    {
                                        paragraphStyle.SpacingLine = att.Value;
                                        hasValue = true;
                                    }
                                }
                            }
                        }
                        else if (styleNode.Name == prefix + ":jc")
                        {
                            if (video.Alignment)
                            {
                                var nodeValue = GetAttributeInNode(styleNode);
                                if (nodeValue.Equals("left"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Left;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("right"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Right;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("center"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Centered;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("both"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Justified;
                                    hasValue = true;
                                }
                            }

                        }
                        else if (styleNode.Name == prefix + ":ind")
                        {
                            if (video.Indent)
                            {
                                //paragraphStyle.Indentation = styleNode.OuterXml;
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == prefix + ":left")
                                    {
                                        paragraphStyle.IndentLeft = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":right")
                                    {
                                        paragraphStyle.IndentRight = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":firstLine")
                                    {
                                        paragraphStyle.IndentFirstLine = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }

                                }
                            }
                        }
                        else if (styleNode.Name == prefix + ":outlineLvl")
                        {
                            if (video.Outline)
                            {
                                var nodeValue = GetAttributeInNode(styleNode);
                                if (!string.IsNullOrEmpty(nodeValue))
                                {
                                    paragraphStyle.Outline = Convert.ToInt32(nodeValue) + 1;
                                    hasValue = true;
                                }

                            }
                        }
                    }
                }

            }
            return hasValue;
        }

        private void SetStyleForExistStyle(System.Xml.XmlNode fontNode, ParagraphStyle paragraphStyle, Video video, System.Xml.XmlDocument styleDoc, char prefix = 'w')
        {
            if (fontNode.Name == prefix + ":rPr")
            {
                System.Xml.XmlNode rFontsNode = null;
                System.Xml.XmlNode colorNode = null;
                System.Xml.XmlNode sizeNode = null;
                System.Xml.XmlNode boldNode = null;
                System.Xml.XmlNode italicNode = null;
                System.Xml.XmlNode underlineNode = null;
                foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                {
                    if (styleNode.Name == "w:rFonts")
                    {
                        rFontsNode = styleNode;
                    }
                    else if (styleNode.Name == "w:color")
                    {
                        colorNode = styleNode;
                    }
                    else if (styleNode.Name == "w:sz" || styleNode.Name == "w:szCs")
                    {
                        if (sizeNode is null)
                            sizeNode = styleNode;
                    }
                    else if (styleNode.Name == "w:b" || styleNode.Name == "w:bCs")
                    {
                        if (boldNode is null)
                            boldNode = styleNode;
                    }
                    else if (styleNode.Name == "w:i" || styleNode.Name == "w:iCs")
                    {
                        if (italicNode is null)
                            italicNode = styleNode;
                    }
                    else if (styleNode.Name == "w:u" || styleNode.Name == "w:uCs")
                    {
                        if (underlineNode is null)
                            underlineNode = styleNode;
                    }

                }

                string font = string.IsNullOrEmpty(paragraphStyle.TranslateFont) ? paragraphStyle.Font : paragraphStyle.TranslateFont;
                if (!string.IsNullOrEmpty(font))
                {
                    if (rFontsNode != null)
                    {
                        foreach (System.Xml.XmlAttribute att in rFontsNode.Attributes)
                        {
                            att.Value = font;
                        }
                    }
                    else
                    {
                        var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:rFonts", fontNode.NamespaceURI);
                        AddAttributeInNode(newNode, "w:ascii", font);
                        AddAttributeInNode(newNode, "w:hAnsi", font);
                        AddAttributeInNode(newNode, "w:cstheme", font);
                        AddAttributeInNode(newNode, "w:asciiTheme", font);
                        fontNode.AppendChild(newNode);
                    }


                }
                else if (rFontsNode != null)
                    rFontsNode.ParentNode?.RemoveChild(rFontsNode);

                if (paragraphStyle.Color != null)
                {
                    string hexColor = System.Drawing.ColorTranslator.ToHtml(paragraphStyle.Color.Value);
                    //Bỏ dấu # ở đầu
                    hexColor = hexColor.Substring(1);
                    if (colorNode != null)
                    {
                        SetNodeAttribute(colorNode, "w:val", hexColor);
                    }
                    else
                    {
                        var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:color", fontNode.NamespaceURI);
                        AddAttributeInNode(newNode, "w:val", hexColor);
                        fontNode.AppendChild(newNode);
                    }
                }
                else if (sizeNode != null)
                {
                    sizeNode.ParentNode?.RemoveChild(sizeNode);
                }

                if (paragraphStyle.Size != null)
                {
                    var sizeValue = System.Convert.ToInt32(paragraphStyle.Size.Value * 2).ToString("D");
                    if (sizeNode != null)
                    {
                        SetNodeAttribute(sizeNode, "w:val", sizeValue);
                    }
                    else
                    {
                        var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:sz", fontNode.NamespaceURI);
                        AddAttributeInNode(newNode, "w:val", sizeValue);
                        fontNode.AppendChild(newNode);
                    }
                }
                else if (sizeNode != null)
                {
                    sizeNode.ParentNode?.RemoveChild(sizeNode);
                }

                if (video.FontBold)
                {
                    if (paragraphStyle.Bold)
                    {
                        if (boldNode == null)
                        {
                            var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:b", fontNode.NamespaceURI);
                            AddAttributeInNode(newNode, "w:type", "character");
                            fontNode.AppendChild(newNode);
                        }
                        else
                        {
                            foreach (System.Xml.XmlAttribute att in boldNode.Attributes)
                            {
                                if (att.Name == "w:val" && att.Value == "0")
                                {
                                    att.Value = "1";
                                }
                            }
                        }

                    }
                    else if (boldNode != null)
                    {
                        var styleValue = GetAttributeInNode(boldNode);
                        //Nếu giá trị trống hoặc khác 0 (override thì xóa) 
                        if (styleValue is null || styleValue != "0")
                            fontNode.RemoveChild(boldNode);
                    }
                }
                if (video.FontItalic)
                {
                    if (paragraphStyle.Italic)
                    {
                        if (italicNode == null)
                        {
                            var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:i", fontNode.NamespaceURI);
                            fontNode.AppendChild(newNode);
                        }
                        else
                        {
                            foreach (System.Xml.XmlAttribute att in italicNode.Attributes)
                            {
                                if (att.Name == "w:val" && att.Value == "0")
                                {
                                    att.Value = "1";
                                }
                            }
                        }

                    }
                    else if (italicNode != null)
                    {
                        var styleValue = GetAttributeInNode(italicNode);
                        //Nếu giá trị trống hoặc khác 0 (override thì xóa) 
                        if (styleValue is null || styleValue != "0")
                            fontNode.RemoveChild(italicNode);
                    }
                }
                if (video.FontUnderline)
                {
                    if (paragraphStyle.Underline)
                    {
                        if (underlineNode == null)
                        {
                            var newNode = fontNode.OwnerDocument.CreateNode(System.Xml.XmlNodeType.Element, "w:u", fontNode.NamespaceURI);
                            AddAttributeInNode(newNode, "w:val", "single");
                            fontNode.AppendChild(newNode);
                        }
                        else
                        {
                            foreach (System.Xml.XmlAttribute att in underlineNode.Attributes)
                            {
                                if (att.Name == "w:val" && att.Value == "none")
                                {
                                    att.Value = "single";
                                }
                            }
                        }

                    }
                    else if (underlineNode != null)
                    {
                        var styleValue = GetAttributeInNode(italicNode);
                        //Nếu giá trị trống hoặc khác none (override thì xóa) 
                        if (styleValue is null || styleValue != "none")
                            fontNode.RemoveChild(underlineNode);
                    }
                }
            }
            else if (fontNode.Name == prefix + ":pPr")
            {
                foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                {
                    if (styleNode.Name == "w:spacing")
                    {
                        if (video.Spacing)
                        {
                            //if (!string.IsNullOrEmpty(paragraphStyle.Spacing) && styleNode.ParentNode != null)
                            //{
                            //    styleNode.ParentNode.InnerXml += paragraphStyle.Spacing;
                            //    styleNode.ParentNode.RemoveChild(styleNode);
                            //}
                            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                            {
                                if (att.Name == "w:before")
                                {
                                    if (paragraphStyle.SpacingBefore != null)
                                    {
                                        //paragraphStyle.SpacingBefore = Convert.ToDecimal(att.Value) / 20;
                                        att.Value = Convert.ToInt32(paragraphStyle.SpacingBefore.Value * 20).ToString();
                                    }
                                }
                                else if (att.Name == "w:after")
                                {
                                    //paragraphStyle.SpacingAfter = Convert.ToDecimal(att.Value) / 20;
                                    if (paragraphStyle.SpacingAfter != null)
                                    {
                                        att.Value = Convert.ToInt32(paragraphStyle.SpacingAfter.Value * 20).ToString();
                                    }
                                }
                                else if (att.Name == "w:line")
                                {
                                    //paragraphStyle.SpacingLineAt = Convert.ToDecimal(att.Value) / 20;
                                    if (paragraphStyle.SpacingLineAt != null)
                                    {
                                        att.Value = Convert.ToInt32(paragraphStyle.SpacingLineAt.Value * 20).ToString();
                                    }
                                }
                                else if (att.Name == "w:lineRule")
                                {
                                    //paragraphStyle.SpacingLine = att.Value;
                                    if (string.IsNullOrEmpty(paragraphStyle.SpacingLine))
                                    {
                                        att.Value = paragraphStyle.SpacingLine;
                                    }
                                }
                            }
                        }
                    }
                    else if (styleNode.Name == "w:jc")
                    {
                        if (video.Alignment)
                        {
                            foreach (System.Xml.XmlAttribute attribute in styleNode.Attributes)
                            {
                                if (attribute.Name == "w:val")
                                {
                                    if (paragraphStyle.Alignment == BusinessObjects.Alignment.Left)
                                        attribute.Value = "left";
                                    else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Right)
                                        attribute.Value = "right";
                                    else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Centered)
                                        attribute.Value = "center";
                                    else if (paragraphStyle.Alignment == BusinessObjects.Alignment.Justified)
                                        attribute.Value = "both";
                                }
                            }
                        }

                    }
                    else if (styleNode.Name == "w:ind")
                    {
                        //if (!string.IsNullOrEmpty(paragraphStyle.Indentation) && styleNode.ParentNode != null)
                        //{
                        //    styleNode.ParentNode.InnerXml += paragraphStyle.Indentation;
                        //    styleNode.ParentNode.RemoveChild(styleNode);
                        //}
                        if (video.Indent)
                        {
                            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                            {
                                if (att.Name == ":left")
                                {
                                    if (paragraphStyle.IndentLeft != null)
                                    {
                                        att.Value = Convert.ToInt32(paragraphStyle.IndentLeft.Value * 20).ToString();
                                    }
                                }
                                else if (att.Name == ":right")
                                {
                                    if (paragraphStyle.IndentRight != null)
                                    {
                                        att.Value = Convert.ToInt32(paragraphStyle.IndentRight.Value * 20).ToString();
                                    }
                                }
                                else if (att.Name == ":firstLine")
                                {
                                    if (paragraphStyle.IndentFirstLine != null)
                                    {
                                        att.Value = Convert.ToInt32(paragraphStyle.IndentFirstLine.Value * 20).ToString();
                                    }
                                }

                            }
                        }

                    }
                    else if (styleNode.Name == "w:outlineLvl")
                    {
                        if (video.Outline && paragraphStyle.Outline != null && paragraphStyle.Outline != 0)
                        {
                            foreach (System.Xml.XmlAttribute attribute in styleNode.Attributes)
                            {
                                if (attribute.Name == "w:val")
                                {
                                    attribute.Value = (paragraphStyle.Outline.Value - 1).ToString("D");
                                }
                            }
                        }
                    }
                    else if (styleNode.Name == "w:rPr")
                    {
                        SetStyleForExistStyle(styleNode, paragraphStyle, video, styleDoc, prefix);
                    }
                }
            }
        }

        // end Export

        private bool AddAudioInTermLocation(Audio audio, TermLocation tl, bool force = false, int startIndex = 0)
        {
            if (tl.MachineTranslate.Contains("Kì"))
            {

            }
            if (audio.Content.Length <= startIndex)
                return false;
            var indexPosition = audio.Content.IndexOf(tl.MachineTranslate, startIndex);
            if (indexPosition >= 0)
            {
                var indexPositionInContent = indexPosition;
                var rows = Module.Helpers.TextHelper.GetSentences(audio.Content);
                for (int r = 0; r < rows.Length; r++)

                {
                    if (rows[r].Length <= startIndex)
                    {
                        startIndex -= rows[r].Length;
                        continue;
                    }
                    if (startIndex < 0)
                        return false;
                    indexPosition = force ? rows[r].IndexOf(tl.MachineTranslate, startIndex) : Module.Helpers.TextHelper.GetIndexWordInContent(tl.MachineTranslate, rows[r], null, startIndex, System.StringComparison.CurrentCulture);

                    //if (indexPosition < 0)
                    //{
                    //    var trimWord = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(tl.MachineTranslate);
                    //    if (!tl.MachineTranslate.Equals(trimWord))
                    //        indexPosition = force ? rows[r].IndexOf(trimWord) : Module.Helpers.TextHelper.GetIndexWordInContent(trimWord, rows[r]);
                    //}
                    if (indexPosition >= 0)
                    {
                        if (startIndex == 0)
                            audio.TermLocationList.Add(tl);
                        tl.Sentence = r + 1;
                        tl.Location = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(rows[r].Substring(0, indexPosition), false).Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                        break;
                        //tl.TranslateLocation = tl.Location;
                    }
                }
                if (indexPosition < 0 && force)
                {
                    if (startIndex == 0)
                        audio.TermLocationList.Add(tl);
                    if (indexPositionInContent == 0)
                    {
                        tl.Sentence = 1;
                        tl.Location = 1;
                    }
                    else
                    {
                        var startContent = audio.Content.Substring(0, indexPositionInContent);
                        rows = Module.Helpers.TextHelper.GetSentences(startContent);

                        tl.Sentence = rows.Length;
                        tl.Location = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(rows[rows.Length > 0 ? rows.Length - 1 : 0], false).Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                    }

                }
                //Kiểm tra xem có tồn tại thuật vị tương tự không
                var overlap = audio.TermLocationList.FirstOrDefault(t => !t.Oid.Equals(tl.Oid) && t.Sentence == tl.Sentence && t.Location == tl.Location);
                if (indexPosition >= 0 && audio.Content.Length > indexPosition + 1 && overlap != null)
                {
                    if (startIndex == 0)
                        AddAudioInTermLocation(audio, tl, force, indexPosition + 1);
                    else
                    {
                        tl.Overlap = true;
                        overlap.Overlap = true;
                    }
                }
            }
            return tl.Sentence != null;
        }

        private void ImportDocFromParagraphStruct(Video video, System.Xml.XmlDocument doc, System.Diagnostics.Stopwatch stopWatch, ref int index, BookMark bookMark = null, string waitCaption = " ")
        {
            //fd
            ShowWaitForm(null, null);
        }

        private bool waitFirst = true;

        //private string SavedFolder = null;
        public HtmlAgilityPack.HtmlDocument GetHtmlDocumentByChrome(string url, string xpath, string waitCaption, System.Diagnostics.Stopwatch stopWatch)
        {
            var driver = new OpenQA.Selenium.Chrome.ChromeDriver(GetChromeDriverService(), GetChromeOptions());
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl(url);
            if (CheckUrlUsedChromeWaitList(url))
            {
                System.Threading.Thread.Sleep(1000);
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                if (!string.IsNullOrEmpty(xpath))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        ShowWaitForm("Đang đợi (" + (10 - i) + " giây)", waitCaption, stopWatch.Elapsed);
                        System.Threading.Thread.Sleep(1000);
                        //Đợi tối đa 10s
                        try
                        {
                            //var ps = driver.PageSource;
                            var element = driver.FindElement(OpenQA.Selenium.By.XPath(xpath));
                            if (element != null)
                                break;

                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
            }
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            var pageSource = driver.PageSource;
            htmlDocument.LoadHtml(pageSource);

            //var no = htmlDocument.DocumentNode.SelectSingleNode(string.IsNullOrEmpty(xpath) ? "//body" : xpath);            
            //driver.Close();
            //driver.Dispose();
            driver.Quit();
            return htmlDocument;
        }
        private string[] _usedChromeWaitList = null;
        private bool CheckUrlUsedChromeWaitList(string url)
        {
            if (_usedChromeWaitList is null)
            {
                string address = "https://www.bhphotovideo.com/\r\nhttps://chat.zalo.me/";
                var addressUsedChromeWait = GetValueOrDefault("AddressUsedChromeWait", address);
                _usedChromeWaitList = addressUsedChromeWait.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            if (!string.IsNullOrEmpty(url))
            {
                foreach (var website in _usedChromeWaitList)
                {
                    if (url.StartsWith(website))
                        return true;
                }
            }
            return false;
        }
        private string[] _usedChromeList = null;
        public bool CheckUrlUsedChromeList(string url)
        {
            if (_usedChromeList is null)
            {
                string address = "https://www.bhphotovideo.com/\r\nhttps://chat.zalo.me/\r\nhttps://www.adorama.com";
                var addressUsedChrome = GetValueOrDefault("AddressUsedChrome", address);
                _usedChromeList = addressUsedChrome.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            if (!string.IsNullOrEmpty(url))
            {
                foreach (var website in _usedChromeList)
                {
                    if (url.StartsWith(website))
                        return true;
                }
            }
            return false;
        }

        private OpenQA.Selenium.Chrome.ChromeDriverService _chromeDriverService = null;
        private OpenQA.Selenium.Chrome.ChromeDriverService GetChromeDriverService()
        {
            if (_chromeDriverService is null)
            {
                _chromeDriverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService();
                if (!AddressShowChromeWindows())
                    _chromeDriverService.HideCommandPromptWindow = true;
            }
            return _chromeDriverService;
        }

        private OpenQA.Selenium.Chrome.ChromeDriver _chromeDriver = null;
        private OpenQA.Selenium.Chrome.ChromeDriver GetChromeDriver()
        {
            if (_chromeDriver is null)
            {
                _chromeDriver = new OpenQA.Selenium.Chrome.ChromeDriver(GetChromeDriverService(), GetChromeOptions());
            }
            return _chromeDriver;
        }

        private bool? _addressShowChromeWindows = null;
        private bool AddressShowChromeWindows()
        {
            if (_addressShowChromeWindows is null)
            {
                _addressShowChromeWindows = GetValueOrDefault<bool>("AddressShowChromeWindows", false);
            }
            return _addressShowChromeWindows.Value;
        }

        private OpenQA.Selenium.Chrome.ChromeOptions _chomeOptions = null;
        private OpenQA.Selenium.Chrome.ChromeOptions GetChromeOptions()
        {
            if (_chomeOptions is null)
            {
                _chomeOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
                //_chomeOptions.AddArgument("--disable-javascript");
                if (!AddressShowChromeWindows())
                    _chomeOptions.AddArguments("--headless=new");
                else
                {
                    //_chomeOptions.AddArgument("disable-infobars");
                    //Map<String, Object> prefs = new HashMap<String, Object>();
                    //prefs.put("safebrowsing.enabled", "true");
                    //_chomeOptions.Ex("prefs", prefs);
                    _chomeOptions.AddArgument("--disable-notifications");
                    _chomeOptions.AddArgument("--start-maximized");

                    //_chomeOptions.AddExcludedArguments("excludeSwitches", Arrays.asList("disable-popup-blocking", "enable-automation"));
                    //_chomeOptions.AddArguments("--remote-allow-origins=*");
                    //_chomeOptions.AddArguments("--start-maximized");
                }
                _chomeOptions.AddArgument("no-sandbox");
                _chomeOptions.AddArguments("--disable-extensions");
                _chomeOptions.AddArgument("disable-infobars");
                System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
                ls.Add("enable-automation");
                ls.Add("excludeSwitches");
                ls.Add("enable-logging");
                ls.Add("disable-popup-blocking");
                _chomeOptions.AddExcludedArguments(ls);
                //_chomeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                //_chomeOptions.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.6099.225 Safari/537.36");
                _chomeOptions.AddArgument("--remote-debugging-port=9223");
            }
            return _chomeOptions;
        }

        OpenAI.Audio.AudioClient audioClient = null;
        private string audioModel = null;
        Module.SystemObjects.CustomAudioTranscriptionOptions customAudioTranscriptionOptions = null;

        public void ChromeDriverQuit()
        {
            if (_chromeDriver != null)
                _chromeDriver.Quit();
        }

        private Module.Services.DataServiceService dataServiceService1 = null;
        public void ImportOpenAIAudio(Video video, Application.DTOs.DataServiceDto dataServiceDto, string url, ref int index, BookMark bookMark = null)
        {
            //Nếu đã tồn tại thì không nạp
            if (bookMark != null && video.AudioList.FirstOrDefault(m => m.BookMark == bookMark) != null)
            {
                _notificationService.NotifyError("Thông báo", $"{bookMark.Name} đã tồn tại trong thành phần");
                return;
            }

            if (!System.IO.File.Exists(url))
                return;
            if (dataServiceDto is null)
                return;
            string logContent = $"Tư liệu {video.Code} - {video.Oid} - Liên kết: {url}";
            var inputInfo = new System.IO.FileInfo(url);
            var textFile1 = url + "_spk.txt";
            var textFile2 = System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name + "_spk.txt";
            var srt1File = Module.Helpers.FileSystemHelper.ReplaceExtension(url, ".srt");
            if (dataServiceService1 is null) dataServiceService1 = new Module.Services.DataServiceService();
            var srt2File = Module.Helpers.FileSystemHelper.ReplaceExtension(System.IO.Directory.GetCurrentDirectory() + "\\" + inputInfo.Name, ".srt");
            var result = Task.Run(() => dataServiceService1.GetResultAsync(dataServiceDto, new string[] { url, inputInfo.Directory.FullName })).GetAwaiter().GetResult();

            string resultType = dataServiceService1.CheckResultType(result);
            if (resultType == "srt")
            {
                ImportAudiosFromSrtString(video, ((string)result), bookMark);

            }
            else if (resultType == "json")
            {
                ImportAudiosFromJsonString(video, ((string)result), bookMark);
            }
            else if (resultType == "text")
            {
                //Dữ liệu pyanote
                if (((string)result).Contains(" SPEAKER_00"))
                {
                    ImportAudiosFromPyanoteString(video, ((string)result), bookMark);
                }
                else
                {
                    ImportAudiosFromTextString(video, ((string)result), bookMark);
                }
            }
            //Dùng code mới
            return;

            //string logContent = $"Tư liệu {video.Code} - {video.Oid} - Liên kết: {url}";
            //var subTitleText = Module.Utils.OpenAiUtils.DownloadFromYoutube(ObjectSpace, Application, url, logContent, ref audioClient, ref audioModel);
            //if (!string.IsNullOrEmpty(subTitleText))
            //{
            //    var lines = subTitleText.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            //    var cultureInfo = new System.Globalization.CultureInfo("vi-VN");
            //    string srtIndex = "";
            //    string timeline = "";
            //    string content = "";
            //    foreach (var s in lines)
            //    {
            //        if (string.IsNullOrEmpty(srtIndex))
            //            srtIndex = s;
            //        else if (string.IsNullOrEmpty(timeline))
            //            timeline = s;
            //        else if (string.IsNullOrEmpty(content))
            //            content = s;
            //        else if (!string.IsNullOrEmpty(s))
            //            content += " " + s;
            //        if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(srtIndex) && !string.IsNullOrEmpty(timeline))
            //        {
            //            try
            //            {
            //                var timer = timeline.Split(' ');

            //                var timeStart = TimeSpan.Parse(timer[0], cultureInfo);
            //                var timeEnd = TimeSpan.Parse(timer[2], cultureInfo);
            //                if (bookMark != null && bookMark.Order != null)
            //                {
            //                    timeStart = timeStart.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
            //                    timeEnd = timeEnd.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
            //                }
            //                //Nạp ngôn ngữ gốc
            //                var audio = new Audio(video.Session);
            //                audio.Content = content.Trim().Replace(" ", " ");
            //                audio.Start = timeStart;
            //                audio.End = timeEnd;
            //                if (bookMark != null)
            //                    audio.BookMark = bookMark;
            //                video.AudioList.Add(audio);
            //            }
            //            catch (Exception)
            //            {

            //            }
            //            srtIndex = "";
            //            timeline = "";
            //            content = "";
            //        }
            //        //do minimal amount of work here
            //    }

            //}

        }
        private char importCharTag = '[';
        public bool ImportWord(Video video, string url, System.Diagnostics.Stopwatch stopWatch, ref int index, ref int styleIndex, BookMark bookMark = null, string choice = "", string xpath = null, string waitCaption = " ", System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle = null)
        {
            try
            {
                var fileInfo = new System.IO.FileInfo(url);
                if (choice.Contains("Media"))
                {
                    ImportInteropMedia(video, choice, fileInfo, listExistedParagraphStyle, stopWatch, bookMark, waitCaption);
                }
                else
                {
                    Module.SystemObjects.Tools.ShowOrCloseWaitFormWithCancelButton();
                    ShowWaitForm("Đang nạp tài liệu", null, stopWatch.Elapsed, true);
                    var tempFolder = System.IO.Directory.GetCurrentDirectory() + "\\Temp\\" + fileInfo.Name;
                    //if (!System.IO.Directory.Exists(tempFolder))
                    //    System.IO.Directory.CreateDirectory(tempFolder);                        
                    //System.IO.Compression.ZipFile.ExtractToDirectory(url, tempFolder, true);
                    Module.SystemObjects.Tools.ZipFileExtractToDirectory(url, tempFolder, true);
                    var xmlFile = tempFolder + "\\word\\document.xml";
                    if (System.IO.File.Exists(xmlFile))
                    {

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.Load(xmlFile);
                        //System.Xml.XmlNode rootNode;
                        //if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].ChildNodes.Count == 1)
                        //    rootNode = doc.ChildNodes[1].ChildNodes[0];
                        //else
                        //    rootNode = doc.ChildNodes[0];
                        //order = 1;
                        //2024-08-22: AbbyyTermLocation bổ sung tính năng nạp thành phần theo paragraph
                        //if (video.AbbyyTermLocation)
                        //{
                        //    ImportDocFromParagraphStruct(doc, xafApplication, video, stopWatch, ref index, bookMark, waitCaption);
                        //}
                        //else
                        //{

                        //}
                        if (listExistedParagraphStyle is null)
                            listExistedParagraphStyle = new System.Collections.Generic.List<ParagraphStyle>();
                        if (choice == "XmlMedia")
                        {
                            ImportExportMedia(video, doc, tempFolder, fileInfo, null, listExistedParagraphStyle, stopWatch, bookMark, waitCaption);
                        }
                        else
                        {
                            System.Collections.Generic.IDictionary<System.Xml.XmlNode, bool> flagNodes = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, bool>();
                            var abbyyTermLocationList = video.AbbyyTermLocation ? new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<TermLocation>>() : null;
                            var abbyyAudioList = video.AbbyyTermLocation ? new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<Audio>>() : null;
                            var existedMediaDictionary = video.ImportParagraph ? video.MediaList.Where(x => x.ShapeId != null && x.BookMark == bookMark).DistinctBy(d => d.ShapeId).ToDictionary(k => k.ShapeId.Value, v => v) : null;
                            if (!video.ImportByNode)
                            {
                                ShowWaitForm("Đang tối ưu hóa tập tin", null, stopWatch.Elapsed, true);
                                OptimalDocument(video, doc, flagNodes, 'w', abbyyTermLocationList);
                                ShowWaitForm(null, null);
                            }
                            ShowWaitForm(waitCaption, null, stopWatch.Elapsed, true);
                            System.Xml.XmlNodeList wtNodes = null;
                            if (!video.FootNote)
                            {
                                wtNodes = doc.GetElementsByTagName("w:t");
                            }
                            else
                            {
                                System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
                                namespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                                //Dấu chấm . ở đầu biểu thức chỉ rõ rằng việc tìm kiếm bắt đầu từ node hiện tại 
                                wtNodes = doc.SelectNodes(".//w:t | .//w:footnoteReference", namespaceManager);
                                //wtNodes = doc.SelectNodes("//w:t | //w:footnoteReference", namespaceManager) //Kết quả tương đương vì tìm từ gốc;
                            }


                            //Tạo defaultStyle
                            string docDefaultsParagraphStyleName = "docDefaults";

                            var docDefaultsParagraphStyle = listExistedParagraphStyle.FirstOrDefault(x => x.Name == docDefaultsParagraphStyleName);
                            if (docDefaultsParagraphStyle is null)
                            {
                                docDefaultsParagraphStyle = CreateObject<ParagraphStyle>();
                                if (bookMark != null)
                                    docDefaultsParagraphStyle.Link = bookMark;
                                docDefaultsParagraphStyle.Name = "docDefaults";
                                docDefaultsParagraphStyle.Video = video;
                                listExistedParagraphStyle.Add(docDefaultsParagraphStyle);
                            }

                            var upperElements = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, Audio>();
                            var upperElementsMultiChildList = new System.Collections.Generic.List<System.Guid>();
                            var upperElementsAdjacentList = new System.Collections.Generic.List<System.Guid>();
                            var upperElementContentForStyle = new System.Collections.Generic.Dictionary<System.Guid, string>();

                            System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>> parentNodeList = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>>();
                            //2025-02-24: Hỗ trợ nạp paragraph
                            if (video.ImportParagraph)
                            {
                                var wpNodes = doc.GetElementsByTagName("w:p");
                                if (System.Diagnostics.Debugger.IsAttached)
                                {
                                    video.Name += " (P:" + wpNodes.Count + " )";
                                }
                                foreach (System.Xml.XmlNode wpNode in wpNodes)
                                {
                                    parentNodeList.Add(wpNode, new System.Collections.Generic.List<System.Xml.XmlNode>());
                                }
                            }


                            foreach (System.Xml.XmlNode node in wtNodes)
                            {
                                var parentNode = GetParentNode(node);
                                if (parentNode != null)
                                {
                                    if (parentNodeList.ContainsKey(parentNode))
                                    {
                                        parentNodeList[parentNode].Add(node);
                                    }
                                    else
                                    {
                                        parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                                    }
                                }
                            }

                            if (video.FootNote)
                            {
                                //Nạp Footnotes                   
                                var xmlFootnotesFile = tempFolder + "\\word\\footnotes.xml";
                                System.Xml.XmlDocument footnotesDoc = new System.Xml.XmlDocument();
                                footnotesDoc.Load(xmlFootnotesFile);
                                if (!video.ImportByNode)
                                {
                                    OptimalDocument(video, footnotesDoc, flagNodes, 'w', abbyyTermLocationList);
                                }
                                var wtFootnotesNodes = footnotesDoc.GetElementsByTagName("w:t");
                                foreach (System.Xml.XmlNode node in wtFootnotesNodes)
                                {
                                    var parentNode = GetParentNode(node);
                                    if (parentNode != null)
                                    {
                                        if (parentNodeList.ContainsKey(parentNode))
                                        {
                                            parentNodeList[parentNode].Add(node);
                                        }
                                        else
                                        {
                                            parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                                        }
                                    }
                                }
                            }


                            var audioList = new System.Collections.Generic.List<Audio>();
                            var newTermLocationList = new System.Collections.Generic.List<TermLocation>();
                            decimal countNumber = 0;
                            //2023-09-22: Chỉnh lại điều kiện Không kề sau : áp dụng trường hợp khi 2 Thành phần thuộc 2 Paragraph không cạnh nhau
                            System.Xml.XmlNode lastedWpNode = null;
                            var wpNodesIndexDictionary = video.IsPhoto ? new System.Collections.Generic.Dictionary<System.Xml.XmlNode, int>() : null;
                            var footNoteAudioDictionary = video.FootNote ? new System.Collections.Generic.Dictionary<int, Audio>() : null;
                            //var listedMedia = IsPhoto ? MediaList.Where(x => x.Start != null && x.ParagraphStyle != null && x.ParagraphStyle.Link == bookMark).ToDictionary(k => System.Convert.ToInt32(k.Start.Value.TotalSeconds), v => v) : null;
                            int paragraphIndex = 0;
                            var paragraphList = new System.Collections.Generic.List<Paragraph>();
                            int inlineIndex = 0;
                            int paragraphStyleIndex = 0; //Debug
                            foreach (var wpNode in parentNodeList.Keys)
                            {

                                if (lastedWpNode != null && audioList.Count > 0 && !audioList[audioList.Count - 1].NotAdjacent)
                                {
                                    if (wpNode.PreviousSibling is null)
                                        audioList[audioList.Count - 1].NotAdjacent = true;
                                    else if (wpNode.PreviousSibling != lastedWpNode)
                                        audioList[audioList.Count - 1].NotAdjacent = true;
                                }
                                lastedWpNode = wpNode;
                                string parentTag = "";
                                if (wpNode.ParentNode != null && wpNode.ParentNode.Name != "w:body")
                                {
                                    parentTag = wpNode.ParentNode.Name;
                                }

                                //2025-02-22: Hỗ trợ paragraph
                                Paragraph paragraph = null;
                                if (video.ImportParagraph)
                                {
                                    paragraphIndex++;
                                    paragraph = CreateObject<Paragraph>();
                                    paragraph.Order = paragraphIndex;
                                    paragraph.BookMark = bookMark;
                                    var paraId = GetAttributeInNode(wpNode, "w14:paraId");
                                    if (!string.IsNullOrEmpty(paraId))
                                        paragraph.Code = paraId;
                                    paragraphList.Add(paragraph);
                                    if (wpNode != null && wpNode.FirstChild != null && wpNode.FirstChild.Name.Equals("w:pPr"))
                                    {
                                        //2025-02-22: Hỗ trợ paragraph Style
                                        var pStyle = CreateObject<ParagraphStyle>();
                                        pStyle.ParagraphStyleType = ParagraphStyleType.Paragraph;
                                        if (System.Diagnostics.Debugger.IsAttached)
                                        {
                                            paragraphStyleIndex++;
                                            pStyle.TranslateFont = paragraphStyleIndex.ToString("D");
                                        }

                                        if (FillParagraphStyleNode(video, wpNode.FirstChild, pStyle, listExistedParagraphStyle, bookMark, false, 'w', true))
                                        {
                                            var existStyle = FindExistParagraphStyle(video, pStyle, listExistedParagraphStyle, ParagraphStyleType.Paragraph);
                                            if (existStyle is null)
                                            {
                                                paragraph.ParagraphStyle = pStyle;
                                                if (bookMark != null)
                                                    pStyle.Link = bookMark;
                                                pStyle.Video = video;
                                                if (!string.IsNullOrEmpty(pStyle.Name))
                                                    pStyle.ParagraphStyleType = ParagraphStyleType.Paragraph;
                                                listExistedParagraphStyle.Add(pStyle);
                                            }
                                            else
                                            {
                                                paragraph.ParagraphStyle = existStyle;
                                                pStyle.Delete();
                                            }

                                        }
                                        else
                                        {
                                            if (pStyle.UpperStyle != null)
                                                paragraph.ParagraphStyle = pStyle.UpperStyle;
                                            pStyle.Delete();
                                            //if (listExistedParagraphStyle != null && listExistedParagraphStyle.Contains(pStyle))
                                            //    listExistedParagraphStyle.Remove(pStyle);
                                        }
                                    }

                                    var shapeList = GetShapeNodesId(doc, wpNode);
                                    foreach (System.Xml.XmlNode shape in shapeList)
                                    {
                                        if (shape.Name == "wp:inline")
                                        {
                                            inlineIndex--;
                                            paragraph.ShapeIdList += $"({inlineIndex})";
                                            if (existedMediaDictionary != null && existedMediaDictionary.ContainsKey(inlineIndex))
                                                existedMediaDictionary[inlineIndex].Paragraph = paragraph;
                                        }
                                        else
                                        {
                                            var shapeId = GetAttributeInNode(shape, "id");
                                            if (!string.IsNullOrEmpty(shapeId))
                                            {
                                                var shapeIdInt = System.Convert.ToInt32(shapeId);
                                                paragraph.ShapeIdList += $"({shapeIdInt})";
                                                if (existedMediaDictionary != null && existedMediaDictionary.ContainsKey(shapeIdInt))
                                                    existedMediaDictionary[shapeIdInt].Paragraph = paragraph;
                                            }
                                        }

                                    }
                                }

                                var nodeList = parentNodeList[wpNode];
                                for (int i = 0; i < nodeList.Count; i++)
                                {
                                    if (nodeList[i].Name == "w:footnoteReference")
                                    {
                                        //Lỗi
                                        var idAttribute = nodeList[i].Attributes["w:id"];
                                        if (idAttribute != null)
                                        {
                                            //if(paragraph != null)
                                            //{                                                
                                            //    paragraph.ShapeIdList += $"({idAttribute.Value})";
                                            //}                                            

                                            var footnoteIndex = System.Convert.ToInt32(idAttribute.Value);
                                            if (!footNoteAudioDictionary.ContainsKey(footnoteIndex))
                                            {
                                                if (audioList.Count > 0)
                                                    footNoteAudioDictionary.Add(footnoteIndex, audioList[audioList.Count - 1]);
                                                else
                                                    _notificationService.NotifyError("Lỗi", $"Không tồn tại thành phần trước {idAttribute.Value}");
                                            }
                                            else
                                                _notificationService.NotifyError("Lỗi", $"footnote {idAttribute.Value} bị trùng");
                                        }
                                        else
                                            _notificationService.NotifyError("Lỗi", $"Tồn tại footnote không chứa Id");
                                        continue;
                                    }
                                    else if (nodeList[i].Name == "w:t")
                                    {
                                        if (!WtContentIsValidate(video, nodeList[i]))
                                            continue;
                                        if (wpNodesIndexDictionary != null)
                                        {
                                            if (!wpNodesIndexDictionary.ContainsKey(wpNode))
                                                wpNodesIndexDictionary.Add(wpNode, index);
                                        }
                                        //var audio = new Audio(Session);
                                        var audio = CreateObject<Audio>();
                                        audioList.Add(audio);
                                        if (paragraph != null)
                                            audio.Paragraph = paragraph;
                                        //audio.Video = video;
                                        //AudioList.Add(audio);
                                        string content = nodeList[i].InnerText.Replace(" ", " ");
                                        if (!video.KeepSpace)
                                            content = content.Trim();
                                        audio.Content = content;
                                        //2025-02-19: CV006: Nếu Thành phần thuộc textbox thì dựng cờ và Ghi chú [TextBox]
                                        //if( GetParentNode(nodeList[i], "w:txbxContent", false) != null)
                                        //{
                                        //    audio.Flag = true;
                                        //    audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, importCharTag, "Trong Text Box");
                                        //}

                                        //audio.Order = index;
                                        audio.Start = TimeSpan.FromSeconds(index);

                                        if (bookMark != null)
                                            audio.BookMark = bookMark;
                                        audio.Flag2 = false;
                                        if (parentTag == "w:txbxContent")
                                        {
                                            audio.Flag2 = true;
                                            audio.Note2 = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note2, importCharTag, "Trong Text Box");
                                        }
                                        else if (parentTag == "w:footnote")
                                        {
                                            //Trường hợp footnote
                                            var footnoteIdAttribute = wpNode.ParentNode.Attributes["w:id"];
                                            if (footnoteIdAttribute != null)
                                            {
                                                //2025-02-22: Khi nạp thành phần, cần dựng cờ 2 các thành phần  có Footnote Mark, đồng thời có chức năng Cờ 1 cho thành phần có footnote trỏ vào
                                                var footnoteId = System.Convert.ToInt32(footnoteIdAttribute.Value);
                                                audio.Quantity = footnoteId;
                                                audio.Flag2 = true;
                                                audio.Note2 = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note2, importCharTag, "Foot note");
                                                if (footNoteAudioDictionary != null && footNoteAudioDictionary.ContainsKey(footnoteId))
                                                {
                                                    var audioFootNote = footNoteAudioDictionary[footnoteId];
                                                    audioFootNote.Flag = true;
                                                    audioFootNote.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, importCharTag, "Có Foot note");
                                                    audio.UpperElement = audioFootNote;
                                                }
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(parentTag))
                                            audio.ParentTag = parentTag;
                                        if (flagNodes.ContainsKey(nodeList[i]))
                                        {
                                            audio.Flag2 = flagNodes[nodeList[i]];
                                            audio.Note2 = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note2, importCharTag, "Bỏ style đậm, nghiêng, gạch");
                                        }
                                        if (abbyyTermLocationList != null && wpNode != null && abbyyTermLocationList.ContainsKey(wpNode) && abbyyTermLocationList[wpNode].Count > 0)
                                        {
                                            if (abbyyAudioList.ContainsKey(wpNode))
                                                abbyyAudioList[wpNode].Add(audio);
                                            else
                                                abbyyAudioList.Add(wpNode, new System.Collections.Generic.List<Audio>() { audio });
                                            foreach (var tl in abbyyTermLocationList[wpNode])
                                            {
                                                //TermLocationList.Add(tl);
                                                newTermLocationList.Add(tl);
                                                AddAudioInTermLocation(audio, tl);
                                            }
                                        }
                                        //2023-08-29
                                        //Đánh dấu trường NotAdjacent khi
                                        //-Paragraph chứa tag ở cuối
                                        if (nodeList[i].NextSibling != null && nodeList[i].NextSibling.Name == "w:tab")
                                            audio.NotAdjacent = true;
                                        else if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.LastChild != null && nodeList[i].ParentNode.LastChild.Name == "w:tab")
                                            audio.NotAdjacent = true;
                                        //foreach (System.Xml.XmlNode tabNode in nodeList[i].ChildNodes)
                                        //{
                                        //    //-Paragraph chứa tag Column ở cuối
                                        //    if (tabNode.Name == "w:tab")
                                        //    {                                        
                                        //        audio.NotAdjacent = true;
                                        //        break;
                                        //    }
                                        //}                                
                                        if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.Name == "w:r")
                                        {
                                            //2023-06-20: Xử lý nhập kiểu cách
                                            var paragraphStyle = CreateObject<ParagraphStyle>();
                                            if (System.Diagnostics.Debugger.IsAttached)
                                            {
                                                paragraphStyleIndex++;
                                                paragraphStyle.TranslateFont = paragraphStyleIndex.ToString("D");
                                            }
                                            if (bookMark != null)
                                                paragraphStyle.Link = bookMark;
                                            System.Xml.XmlNode characterNodeStyle = nodeList[i].ParentNode.FirstChild.Name.Equals("w:rPr") ? nodeList[i].ParentNode.FirstChild : null;
                                            System.Xml.XmlNode paragraphNodeStyle = (wpNode != null && wpNode.FirstChild.Name.Equals("w:pPr")) ? wpNode.FirstChild : null;
                                            paragraphNodeStyle = wpNode.FirstChild;

                                            //if (characterNodeStyle is null && paragraphNodeStyle is null)
                                            //    continue;
                                            //Nạp style của paragraph 
                                            bool hasValue = false;
                                            //Nếu không nạp cấu trúc paragraph thì phải nạp style của paragraph
                                            if (!video.ImportParagraph && paragraphNodeStyle != null)
                                            {
                                                if (FillParagraphStyleNode(video, paragraphNodeStyle, paragraphStyle, listExistedParagraphStyle, bookMark))
                                                    hasValue = true;
                                                //2024-08-15: Nhận dạng: Numbering, List Bullet của thành phần > nếu chữ thường > chuyển hoa + dựng cờ
                                                if (video.UpcaseNumbering)
                                                {
                                                    foreach (System.Xml.XmlNode styleNode in paragraphNodeStyle.ChildNodes)
                                                    {
                                                        if (styleNode.Name == "w:numPr")
                                                        {
                                                            var firstCharLower = Module.SystemObjects.Tools.GetFirstCharLower(audio.Content);
                                                            if (firstCharLower >= 0)
                                                            {
                                                                firstCharLower++;
                                                                audio.Content = audio.Content.Substring(0, firstCharLower).ToUpper() + audio.Content.Substring(firstCharLower);
                                                                audio.Flag2 = true;
                                                                audio.Note2 = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note2, importCharTag, "Tự động hoa đầu");
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            //Nạp style của character 
                                            if (characterNodeStyle != null)
                                            {
                                                if (FillCharStyleNode(video, characterNodeStyle, paragraphStyle, listExistedParagraphStyle, bookMark))
                                                    hasValue = true;
                                            }
                                            if (hasValue)
                                            {
                                                if (video.RightIndent && paragraphStyle.IndentRight != null)
                                                {
                                                    //2024-08-29
                                                    //Khi có option Thụt phải / RightIndent trong phần Nạp
                                                    //Sẽ lưu giá trị Thụt phải của mỗi Thành phần vào trường Số lượng(Trường này đang là Int sẽ cần chuyển sang Decimal)
                                                    audio.Quantity = paragraphStyle.IndentRight;
                                                }
                                                //Kiểm tra xem có stype này chưa
                                                ParagraphStyle existStyle = null;
                                                if (video.ImportParagraph)
                                                    existStyle = FindExistParagraphStyle(video, paragraphStyle, listExistedParagraphStyle);
                                                else
                                                    existStyle = FindExistParagraphStyle(video, paragraphStyle, listExistedParagraphStyle, ParagraphStyleType.Character);
                                                if (existStyle != null)
                                                {
                                                    //if (!string.IsNullOrEmpty(existStyle.Content))
                                                    //    existStyle.Content += System.Environment.NewLine;
                                                    //existStyle.Content += audio.Content;
                                                    paragraphStyle.Delete();
                                                    audio.ParagraphStyle = existStyle;
                                                }
                                                else
                                                {
                                                    //paragraphStyle.Content = i.ToString();
                                                    //paragraphStyle.Content = paragraphNodeStyle.InnerXml.Length < 1000 ? paragraphNodeStyle.InnerXml : paragraphNodeStyle.InnerXml.Substring(0, 1000);
                                                    paragraphStyle.Video = video;
                                                    //paragraphList.Add(paragraphStyle);
                                                    //ParagraphStyleList.Add(paragraphStyle);                                            
                                                    audio.ParagraphStyle = paragraphStyle;
                                                    if (video.ImportParagraph && string.IsNullOrEmpty(paragraphStyle.Name))
                                                        paragraphStyle.ParagraphStyleType = ParagraphStyleType.Character;
                                                    listExistedParagraphStyle.Add(paragraphStyle);
                                                }

                                            }
                                            else
                                            {
                                                //Nếu không có giá trị thì style hiện tại là style của upper
                                                if (paragraphStyle.UpperStyle != null)
                                                {
                                                    audio.ParagraphStyle = paragraphStyle.UpperStyle;
                                                }
                                                else
                                                {
                                                    audio.ParagraphStyle = docDefaultsParagraphStyle;
                                                }
                                                paragraphStyle.Delete();
                                                if (listExistedParagraphStyle != null && listExistedParagraphStyle.Contains(paragraphStyle))
                                                    listExistedParagraphStyle.Remove(paragraphStyle);
                                            }
                                            if (audio.ParagraphStyle != null)
                                            {
                                                if (audio.ParagraphStyle.UpperStyle is null)
                                                    audio.ParagraphStyle.UpperStyle = docDefaultsParagraphStyle;
                                            }


                                            //2023-08-29
                                            //Đánh dấu trường NotAdjacent khi
                                            //-Paragraph chứa tag Column ở cuối
                                            //- Paragraph sau nó chứa tag Column ở đầu
                                            //- Sau nó là các Pargraph trống rồi mới tới Thành phần tiếp
                                            //Trường hợp thẻ trống phía sau là wr có tab
                                            //if (nodeList[i].ParentNode.InnerText.Contains("1: signed byte"))
                                            //{

                                            //}
                                            if (!audio.NotAdjacent && nodeList[i].ParentNode.NextSibling != null && nodeList[i].ParentNode.NextSibling.InnerText == "")
                                            {
                                                foreach (System.Xml.XmlNode tabNode in nodeList[i].ParentNode.NextSibling.ChildNodes)
                                                {
                                                    //-Paragraph chứa tag Column ở cuối
                                                    if (tabNode.Name == "w:tab")
                                                    {
                                                        audio.NotAdjacent = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!audio.NotAdjacent && wpNode != null && wpNode.NextSibling != null && wpNode.NextSibling.Name == "w:p")
                                            {
                                                if (string.IsNullOrEmpty(wpNode.NextSibling.InnerText))
                                                {
                                                    //Sau nó là các Pargraph trống rồi mới tới Thành phần tiếp
                                                    audio.NotAdjacent = true;
                                                }
                                                //else if(wpNode.NextSibling.FirstChild != null && wpNode.NextSibling.FirstChild.Name == "w:pPr")
                                                //{
                                                //    foreach (System.Xml.XmlNode tabsNode in wpNode.NextSibling.FirstChild.ChildNodes)
                                                //    {
                                                //        if (tabsNode.Name == "w:tabs")
                                                //        {
                                                //            audio.NotAdjacent = true;
                                                //            break;
                                                //        }
                                                //    }
                                                //}
                                                else if (wpNode.NextSibling.InnerXml.Contains("w:br w:type=\"column\""))
                                                {
                                                    //Paragraph sau nó chứa tag Column ở đầu
                                                    audio.NotAdjacent = true;
                                                    //Kiểm tra xem node tiếp theo có chứa <w:br w:type="column"/>

                                                }
                                            }
                                        }
                                        else
                                        {
                                        }

                                        if (video.ImportByNode || video.UpperElementImport && nodeList.Count > 1)
                                        {
                                            //2023-08-24: Khi nhập theo nốt, TextNode sẽ trỏ vào Paragraph, sẽ nhập Paragraph như cũ,
                                            //còn TextNode trỏ vào Paragraph mẹ đồng thời Nốt = TRUE, Style vẫn áp dụng cho cả TextNode và Paragraph
                                            //Các khái niệm Gộp liền kề chỉ áp dụng cho cùng loại(Paragraph hoặc TextNode)
                                            //và TextNode chỉ trong phạm vi cùng Paragraph, khi gộp 2 Paragraph thì các TextNode con trỏ về cùng mẹ
                                            //2023-08-24: Dùng trường cấp trên để xác định TextNode
                                            //audio.TextNode = true;
                                            if (wpNode != null && upperElements.ContainsKey(wpNode))
                                            {
                                                audio.UpperElement = upperElements[wpNode];
                                                //=> Lưu ý: Trong trường hợp có ở giữa 2 node có thể có node không nạp(không chứa ký tự) thì chấp nhận mất dữ liệu trong nội dung của node
                                                //upperElement.Content = wpNode.InnerText.Replace(" ", " ");
                                                if (!string.IsNullOrEmpty(audio.UpperElement.Content) && !string.IsNullOrEmpty(audio.Content)
                                                    && (!audio.UpperElement.Content.EndsWith(' ') || !audio.Content.EndsWith(' ')))
                                                    audio.UpperElement.Content += ' ';
                                                audio.UpperElement.Content += audio.Content;
                                                //Nếu nội dụng cũng style ngắn hơn style hiện tại thì style là style của dài hơn
                                                if (video.UpperElementImport && nodeList.Count > 1 && audio.ParagraphStyle != null && !string.IsNullOrEmpty(audio.Content))
                                                {
                                                    if (audio.UpperElement.ParagraphStyle is null)
                                                        audio.UpperElement.ParagraphStyle = audio.ParagraphStyle;
                                                    else if (!audio.UpperElement.ParagraphStyle.Oid.Equals(audio.ParagraphStyle.Oid) &&
                                                        upperElementContentForStyle.ContainsKey(audio.UpperElement.Oid) &&
                                                        !string.IsNullOrEmpty(upperElementContentForStyle[audio.UpperElement.Oid]) &&
                                                        upperElementContentForStyle[audio.UpperElement.Oid].Length < audio.Content.Length)
                                                    {
                                                        audio.UpperElement.ParagraphStyle = audio.ParagraphStyle;
                                                        upperElementContentForStyle[audio.UpperElement.Oid] = audio.Content;
                                                    }
                                                }
                                                if (video.UpperElementImport && !upperElementsMultiChildList.Contains(audio.UpperElement.Oid))
                                                    upperElementsMultiChildList.Add(audio.UpperElement.Oid);
                                            }
                                            else
                                            {
                                                //Tao UpperElement
                                                var upperElement = new Audio(audio.Session);
                                                upperElement.BookMark = audio.BookMark;
                                                upperElement.TranslateObject = audio.TranslateObject;
                                                //upperElement.Video = video;
                                                //AudioList.Add(upperElement);
                                                audioList.Add(upperElement);
                                                //2023-09-13: Nội dung của Cấp trên là cộng text các cấp dưới, không để bị dính nhau
                                                //=> Lưu ý: Trong trường hợp có ở giữa 2 node có thể có node không nạp(không chứa ký tự) thì chấp nhận mất dữ liệu trong nội dung của node
                                                //upperElement.Content = wpNode.InnerText.Replace(" ", " ");
                                                upperElement.Content = audio.Content;
                                                //audio.Order = index;
                                                upperElement.Start = TimeSpan.FromSeconds(index);
                                                if (bookMark != null)
                                                    audio.BookMark = bookMark;
                                                audio.UpperElement = upperElement;
                                                upperElements.Add(wpNode, upperElement);


                                                upperElement.ParagraphStyle = audio.ParagraphStyle;
                                                if (video.UpperElementImport && nodeList.Count > 1)
                                                    upperElementContentForStyle.Add(upperElement.Oid, audio.Content);
                                            }
                                            if (!audio.NotAdjacent && audio.UpperElement != null && !upperElementsAdjacentList.Contains(audio.UpperElement.Oid))
                                                upperElementsAdjacentList.Add(audio.UpperElement.Oid);
                                        }
                                        countNumber++;
                                        if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                            break;
                                        var percent = countNumber / wtNodes.Count;
                                        if (percent < 100)
                                            ShowWaitForm(null, percent.ToString("p0"), stopWatch.Elapsed, true);
                                        index++;
                                    }
                                    else
                                    {
                                        //Xác định paragraph shape
                                    }
                                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                        break;
                                }
                                if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                    break;
                            }


                            //Chức năng tìm thuật vị chính xác
                            if (abbyyTermLocationList != null && abbyyAudioList != null)
                            {
                                int test1 = 0, test2 = 0, test3 = 0;
                                try
                                {
                                    var lastedKeyNode = abbyyTermLocationList.Keys.FirstOrDefault();
                                    foreach (var keyNode in abbyyTermLocationList.Keys)
                                    {
                                        test1++;
                                        var abbyyTermLocationByKeyList = abbyyTermLocationList[keyNode];
                                        if (abbyyTermLocationByKeyList is null)
                                            continue;
                                        foreach (var tl in abbyyTermLocationByKeyList)
                                        {
                                            test2++;
                                            if (tl.Audio is null)
                                            {
                                                if (abbyyAudioList.ContainsKey(keyNode))
                                                {
                                                    //Tìm thành phần chính xác
                                                    foreach (var audio in abbyyAudioList[keyNode])
                                                        AddAudioInTermLocation(audio, tl);
                                                    if (tl.Audio is null)
                                                    {
                                                        foreach (var audio in abbyyAudioList[keyNode])
                                                            AddAudioInTermLocation(audio, tl, true);
                                                        if (tl.Audio is null)
                                                        {
                                                            //Có thể là thuật ngữ đấy bị tách 2 câu
                                                            foreach (var audio in abbyyAudioList[keyNode])
                                                            {
                                                                test3++;
                                                                if (tl.MachineTranslate.Contains(audio.Content))
                                                                {
                                                                    tl.Audio = audio;
                                                                    break;
                                                                }
                                                            }
                                                            if (tl.Audio is null)
                                                            {
                                                                if (abbyyAudioList[keyNode].Count == 1)
                                                                    tl.Audio = abbyyAudioList[keyNode][0];
                                                                if (tl.Audio is null)
                                                                {
                                                                    //ép phải gán và đánh dấu cờ: 
                                                                    tl.Audio = abbyyAudioList[keyNode][0];
                                                                    tl.Flag = true;
                                                                    tl.Translate = "Lệch thành phần";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (lastedKeyNode != null && abbyyAudioList.ContainsKey(lastedKeyNode))
                                                {
                                                    //Lỗi
                                                    tl.Audio = abbyyAudioList[lastedKeyNode]?.LastOrDefault();
                                                    tl.Flag = true;
                                                    tl.Translate = "Không có thành phần";
                                                }
                                            }
                                        }
                                        lastedKeyNode = keyNode;
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }


                            }
                            ShowWaitForm("Đang nạp, vui lòng đợi!", null, stopWatch.Elapsed, true);

                            if (video.UpperElementImport)
                            {
                                //Bỏ những đối tượng audio có 1 dòng
                                if (upperElementsMultiChildList.Count > 0)
                                {
                                    for (int j = audioList.Count - 1; j >= 0; j--)
                                    {
                                        if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                            break;
                                        if (audioList[j].UpperElement != null && !upperElementsMultiChildList.Contains(audioList[j].UpperElement.Oid))
                                        {
                                            //Xóa dòng cấp trên, và xóa UpperElement
                                            audioList.Remove(audioList[j].UpperElement);
                                            audioList[j].UpperElement.Delete();
                                            audioList[j].UpperElement = null;
                                        }
                                    }
                                }
                                //Bỏ những đối tượng audio có toàn bộ là kề sau
                                if (upperElementsAdjacentList.Count > 0)
                                {
                                    for (int j = audioList.Count - 1; j >= 0; j--)
                                    {
                                        if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                            break;
                                        if (upperElementsMultiChildList.Contains(audioList[j].Oid) && !upperElementsAdjacentList.Contains(audioList[j].Oid))
                                        {
                                            audioList[j].Delete();
                                            audioList.Remove(audioList[j]);
                                            //audioList[j].Note = "Xóa";
                                        }
                                        else if (audioList[j].UpperElement != null && upperElementsMultiChildList.Contains(audioList[j].UpperElement.Oid) && !upperElementsAdjacentList.Contains(audioList[j].UpperElement.Oid))
                                        {
                                            audioList[j].UpperElement = null;
                                            //audioList[j].Note = "Xóa upper";
                                        }
                                    }
                                }
                            }
                            video.AudioList.AddRange(audioList);
                            video.ParagraphList.AddRange(paragraphList);
                            video.TermLocationList.AddRange(newTermLocationList);
                            //ParagraphStyleList.AddRange(paragraphList);
                            if (video.IsPhoto)
                            {
                                ImportExportMedia(video, doc, tempFolder, fileInfo, wpNodesIndexDictionary, listExistedParagraphStyle, stopWatch, bookMark, waitCaption);
                            }
                            //Xứ lý Style đã có sẵn
                            //Lấy defaultStyle                        
                            var xmlStyleFile = tempFolder + "\\word\\styles.xml";
                            System.Xml.XmlDocument styleDoc = new System.Xml.XmlDocument();
                            styleDoc.Load(xmlStyleFile);
                            //2023-09-07: Nạp defaultStyle
                            foreach (System.Xml.XmlNode node in styleDoc.ChildNodes)
                            {
                                if (node.Name == "w:styles" && node.FirstChild != null && node.FirstChild.Name == "w:docDefaults")
                                {
                                    foreach (System.Xml.XmlNode nodeDefault in node.FirstChild.ChildNodes)
                                    {
                                        if (nodeDefault.Name == "w:rPrDefault")
                                        {
                                            //Nạp style cho char
                                            if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:rPr")
                                            {
                                                FillCharStyleNode(video, nodeDefault.FirstChild, docDefaultsParagraphStyle, listExistedParagraphStyle, bookMark);
                                            }
                                        }
                                        else if (nodeDefault.Name == "w:pPrDefault")
                                        {
                                            //Nạp style cho char
                                            if (nodeDefault.FirstChild != null && nodeDefault.FirstChild.Name == "w:pPr")
                                            {
                                                FillParagraphStyleNode(video, nodeDefault.FirstChild, docDefaultsParagraphStyle, listExistedParagraphStyle, bookMark);
                                            }
                                        }
                                    }
                                }
                            }
                            //Nạp dữ liệu cho style đã có trong tài liệu
                            var existedNewParagraphStyleList = listExistedParagraphStyle.Where(m => !string.IsNullOrEmpty(m.Name) && video.Session.IsNewObject(m));
                            //2023-09-07: Nạp Upper style
                            foreach (var paragraphStyle in existedNewParagraphStyleList.ToList())
                            {
                                if (paragraphStyle.UpperStyle != null)
                                {
                                    //Debug
                                }
                                foreach (System.Xml.XmlNode stylesNode in styleDoc.ChildNodes)
                                {
                                    if (stylesNode.Name != "w:styles")
                                        continue;
                                    foreach (System.Xml.XmlNode styleNode in stylesNode.ChildNodes)
                                    {
                                        if (styleNode.Name != "w:style")
                                            continue;
                                        if (!paragraphStyle.Name.Equals(GetAttributeInNode(styleNode, "w:styleId")))
                                            continue;
                                        foreach (System.Xml.XmlNode prNode in styleNode.ChildNodes)
                                        {
                                            if (prNode.Name == "w:basedOn")
                                            {
                                                var upperStyleName = GetAttributeInNode(prNode);
                                                if (!string.IsNullOrEmpty(upperStyleName))
                                                {
                                                    var upperStyle = GetDefaultUpperStyle(video, upperStyleName, listExistedParagraphStyle, bookMark);
                                                    if (upperStyle != null)
                                                    {
                                                        paragraphStyle.UpperStyle = upperStyle;
                                                        if (listExistedParagraphStyle != null && !listExistedParagraphStyle.Contains(paragraphStyle))
                                                            listExistedParagraphStyle.Add(paragraphStyle);
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                //Không cần hủy nạp style
                                //if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                //    break;
                            }
                            foreach (var paragraphStyle in existedNewParagraphStyleList.ToList())
                            {
                                foreach (System.Xml.XmlNode stylesNode in styleDoc.ChildNodes)
                                {
                                    if (stylesNode.Name != "w:styles")
                                        continue;
                                    foreach (System.Xml.XmlNode styleNode in stylesNode.ChildNodes)
                                    {
                                        if (styleNode.Name != "w:style")
                                            continue;
                                        if (!paragraphStyle.Name.Equals(GetAttributeInNode(styleNode, "w:styleId")))
                                            continue;
                                        var psStyle = GetAttributeInNode(styleNode, "w:type");
                                        switch (psStyle)
                                        {
                                            case "paragraph":
                                                paragraphStyle.ParagraphStyleType = ParagraphStyleType.Paragraph;
                                                break;
                                            case "character":
                                                paragraphStyle.ParagraphStyleType = ParagraphStyleType.Character;
                                                break;
                                            //case "linked":
                                            //    paragraphStyle.ParagraphStyleType = ParagraphStyleType.Linked;
                                            //    break;
                                            case "table":
                                                paragraphStyle.ParagraphStyleType = ParagraphStyleType.Table;
                                                break;
                                            case "numbering":
                                                paragraphStyle.ParagraphStyleType = ParagraphStyleType.List;
                                                break;
                                            case "list":
                                                paragraphStyle.ParagraphStyleType = ParagraphStyleType.List;
                                                break;
                                            case null:
                                                break;
                                        }
                                        foreach (System.Xml.XmlNode prNode in styleNode.ChildNodes)
                                        {
                                            if (prNode.Name == "w:link")
                                            {
                                                //Hủy loại linked
                                                //paragraphStyle.ParagraphStyleType = ParagraphStyleType.Linked;
                                            }
                                            else if (prNode.Name == "w:pPr")
                                            {
                                                FillParagraphStyleNode(video, prNode, paragraphStyle, listExistedParagraphStyle, bookMark);
                                                //if (!Outline && !Alignment && !Spacing && !Indent)
                                                //    continue;
                                                //foreach (System.Xml.XmlNode childNode in prNode.ChildNodes)
                                                //{
                                                //    //if (childNode.Name == "w:spacing")
                                                //    //{
                                                //    //    paragraphStyle.Spacing = childNode.OuterXml;
                                                //    //}
                                                //    //else 
                                                //    if (styleNode.Name == "w:spacing")
                                                //    {
                                                //        if (Spacing)
                                                //        {
                                                //            //paragraphStyle.Spacing = styleNode.OuterXml;
                                                //            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                                //            {
                                                //                if (att.Name == "w:before")
                                                //                {
                                                //                    paragraphStyle.SpacingBefore = Convert.ToDecimal(att.Value) / 20;
                                                //                }
                                                //                else if (att.Name == "w:after")
                                                //                {
                                                //                    paragraphStyle.SpacingAfter = Convert.ToDecimal(att.Value) / 20;
                                                //                }
                                                //                else if (att.Name == "w:line")
                                                //                {
                                                //                    paragraphStyle.SpacingLineAt = Convert.ToDecimal(att.Value) / 20;
                                                //                }
                                                //                else if (att.Name == "w:lineRule")
                                                //                {
                                                //                    paragraphStyle.SpacingLine = att.Value;
                                                //                }
                                                //            }
                                                //        }
                                                //    }else if (Alignment && childNode.Name == "w:jc")
                                                //    {
                                                //        var nodeValue = this.GetAttributeInNode(childNode);
                                                //        if (nodeValue.Equals("left"))
                                                //            paragraphStyle.Alignment = BusinessObjects.Alignment.Left;
                                                //        else if (nodeValue.Equals("right"))
                                                //            paragraphStyle.Alignment = BusinessObjects.Alignment.Right;
                                                //        else if (nodeValue.Equals("center"))
                                                //            paragraphStyle.Alignment = BusinessObjects.Alignment.Centered;
                                                //        else if (nodeValue.Equals("both"))
                                                //            paragraphStyle.Alignment = BusinessObjects.Alignment.Justified;
                                                //    }
                                                //    else if (childNode.Name == "w:ind")
                                                //    {
                                                //        //paragraphStyle.Indentation = childNode.OuterXml;
                                                //        if (Indent)
                                                //        {
                                                //            //paragraphStyle.Indentation = styleNode.OuterXml;
                                                //            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                                //            {
                                                //                if (att.Name == "w:before")
                                                //                {
                                                //                    paragraphStyle.IndentLeft = Convert.ToDecimal(att.Value) / 20;
                                                //                }
                                                //                else if (att.Name == "w:after")
                                                //                {
                                                //                    paragraphStyle.IndentRight = Convert.ToDecimal(att.Value) / 20;
                                                //                }
                                                //                else if (att.Name == "w:line")
                                                //                {
                                                //                    paragraphStyle.IndentFirstLine = Convert.ToDecimal(att.Value) / 20;
                                                //                }

                                                //            }
                                                //        }
                                                //    }
                                                //    else if (Outline && childNode.Name == "w:outlineLvl")
                                                //    {
                                                //        var nodeValue = this.GetAttributeInNode(childNode);
                                                //        if (!string.IsNullOrEmpty(nodeValue))
                                                //            paragraphStyle.Outline = Convert.ToInt32(nodeValue) + 1;
                                                //    }
                                                //}
                                            }
                                            else if (prNode.Name == "w:rPr")
                                            {
                                                FillCharStyleNode(video, prNode, paragraphStyle, listExistedParagraphStyle, bookMark);
                                                //FillCharStyleNode(prNode, paragraphStyle, listExistedParagraphStyle, bookMark,false, 'w', true);
                                            }
                                        }
                                    }
                                }
                                //Không cần hủy nạp style
                                //if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                                //    break;
                            }

                            //2023-09-07: Bỏ yêu cầu kích thước và Font không được phép trống
                            ////Xử lý kích thước Font không được phép trống
                            //var invalidsParagraphStyleCount = ParagraphStyleList.Count(m => m.Size is null || m.Font is null);
                            //if(invalidsParagraphStyleCount > 0)
                            //{
                            //    decimal? defaultSize = null;
                            //    string defaultFont = null;
                            //    foreach (System.Xml.XmlNode node in styleDoc.ChildNodes)
                            //    {
                            //        if (node.Name == "w:styles" && node.FirstChild != null && node.FirstChild.Name == "w:docDefaults"
                            //                && node.FirstChild.FirstChild != null && node.FirstChild.FirstChild.Name == "w:rPrDefault"
                            //                && node.FirstChild.FirstChild.FirstChild != null && node.FirstChild.FirstChild.FirstChild.Name == "w:rPr")
                            //        {
                            //            foreach (System.Xml.XmlNode childNode in node.FirstChild.FirstChild.FirstChild.ChildNodes)
                            //            {
                            //                if (childNode.Name == "w:sz")
                            //                {
                            //                    foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                            //                    {
                            //                        if (att.Name == "w:val")
                            //                        {
                            //                            defaultSize = Convert.ToDecimal(att.Value) / 2;
                            //                            break;
                            //                        }
                            //                    }
                            //                }
                            //                else if (childNode.Name == "w:rFonts")
                            //                {
                            //                    foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                            //                    {
                            //                        if (att.Name == "w:ascii")
                            //                        {
                            //                            defaultFont = att.Value;
                            //                            break;
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            break;
                            //        }
                            //    }
                            //    //Lấy style theo paragraph
                            //    if (defaultSize is null || string.IsNullOrEmpty(defaultFont))
                            //    {
                            //        foreach (System.Xml.XmlNode node in styleDoc.ChildNodes)
                            //        {
                            //            if (node.Name != "w:styles")
                            //                continue;
                            //            foreach (System.Xml.XmlNode styleNode in node.ChildNodes)
                            //            {
                            //                if (styleNode.Name != "w:style")
                            //                    continue;
                            //                foreach (System.Xml.XmlAttribute styleAttribute in styleNode.Attributes)
                            //                {
                            //                    if (styleAttribute.Name == "w:type" && styleAttribute.Value == "paragraph")
                            //                    {
                            //                        foreach (System.Xml.XmlNode rPrNode in styleNode.ChildNodes)
                            //                        {
                            //                            if (rPrNode.Name == "w:rPr")
                            //                            {
                            //                                foreach (System.Xml.XmlNode childNode in rPrNode.ChildNodes)
                            //                                {
                            //                                    if (childNode.Name == "w:sz")
                            //                                    {
                            //                                        if (defaultSize is null)
                            //                                        {
                            //                                            foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                            //                                            {
                            //                                                if (att.Name == "w:val")
                            //                                                {
                            //                                                    defaultSize = Convert.ToDecimal(att.Value) / 2;
                            //                                                    break;
                            //                                                }
                            //                                            }
                            //                                        }

                            //                                    }
                            //                                    else if (childNode.Name == "w:rFonts")
                            //                                    {
                            //                                        if (string.IsNullOrEmpty(defaultFont))
                            //                                        {
                            //                                            foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                            //                                            {
                            //                                                if (att.Name == "w:ascii")
                            //                                                {
                            //                                                    defaultFont = att.Value;
                            //                                                    break;
                            //                                                }
                            //                                            }
                            //                                        }
                            //                                    }
                            //                                }
                            //                                break;
                            //                            }
                            //                        }
                            //                        break;
                            //                    }
                            //                }
                            //                if (defaultSize != null && !string.IsNullOrEmpty(defaultFont))
                            //                    break;
                            //            }
                            //        }
                            //    }
                            //    if (defaultSize != null || !string.IsNullOrEmpty(defaultFont))
                            //    {
                            //        foreach (var paragraphStyle in ParagraphStyleList)
                            //        {
                            //            if (paragraphStyle.Size is null && defaultSize != null)
                            //            {
                            //                paragraphStyle.Size = defaultSize;
                            //            }
                            //            if (string.IsNullOrEmpty(paragraphStyle.Font) && !string.IsNullOrEmpty(defaultFont))
                            //            {
                            //                paragraphStyle.Font = defaultFont;
                            //            }
                            //        }
                            //    }
                            //}
                            //Giải phóng bộ nhớ
                            try
                            {
                                parentNodeList = null;
                                doc = null;
                                GC.Collect();
                            }
                            catch (System.Exception) { }

                        }

                        //var paragraphStyleList = ParagraphStyleList.Where(m => string.IsNullOrEmpty(m.Name)).OrderBy(m => m.Size).ToList();

                        var paragraphStyleList = listExistedParagraphStyle.Where(m => string.IsNullOrEmpty(m.Name)).OrderBy(m => m.Size).ToList();
                        //2023-06-22: Tên Style để là S01, S02 và tăng dần, có tính năng sửa tên sau khi sort theo độ lớn font để thứ tự từ 01 > 99
                        for (int i = 0; i < paragraphStyleList.Count; i++)
                        {
                            var newIndex = styleIndex + i;
                            string styleName = (newIndex + 1).ToString();
                            if (newIndex < 9)
                                styleName = "00" + styleName;
                            else if (newIndex < 99)
                                styleName = "0" + styleName;
                            //else if (newIndex >= 999)
                            //    styleName += "(Không hỗ trợ)";
                            //2024-08-26: Tên kiểu cách là duy nhất và đặt theo công thức: xxyyy trong đó xx là số thứ tự của tài liệu quy về 2 chữ số, yyy là số tăng dần của Kiểu cách trong 1 tài liệu
                            if (bookMark != null)
                            {
                                styleName = bookMark.GetOrderCode() + styleName;
                            }
                            paragraphStyleList[i].Name += styleName;
                        }

                    }
                    return true;
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ShowWaitForm(null, null);
            }
            return false;

        }

        public void ExportWordMLMedia(Video video, string openFileName, BookMark bookmark, string saveFile, System.Diagnostics.Stopwatch stopWatch, string waitCaption = " ", System.Collections.Generic.List<Media> listedMedia = null)
        {
            ShowWaitForm("Đang mở tệp", waitCaption, stopWatch.Elapsed);

            try
            {
                var nonExportMedia = new System.Collections.Generic.List<Media>();
                var childItemsUngroup = new System.Collections.Generic.List<int>();
                using (DocumentFormat.OpenXml.Packaging.WordprocessingDocument wordDoc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(openFileName, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    // Tìm tất cả các shape trong tài liệu
                    var shapesList = body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>().ToList();
                    var shapeList = new System.Collections.Generic.Dictionary<int, DocumentFormat.OpenXml.Wordprocessing.Drawing>();
                    foreach (var drawing in shapesList)
                    {
                        if (drawing.Anchor != null)
                        {
                            var id = drawing.Anchor.AnchorId;
                            drawing.Anchor.HorizontalPosition.HorizontalAlignment = new DocumentFormat.OpenXml.Drawing.Wordprocessing.HorizontalAlignment(drawing.Anchor.HorizontalPosition.HorizontalAlignment.InnerText);
                            var graphic = drawing.Anchor.Descendants<DocumentFormat.OpenXml.Drawing.Graphic>();
                            if (graphic != null)
                            {
                                //graphic.
                            }
                        }
                        else if (drawing.Inline != null)
                        {

                        }
                        //s
                        //if (shapeObj is Microsoft.Office.Interop.Word.Shape)
                        //{
                        //    var shape = (Microsoft.Office.Interop.Word.Shape)shapeObj;
                        //    if (shapeList.ContainsKey(shape.ID))
                        //    {
                        //        //Lỗi
                        //        Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Lỗi", $"Đã tồn tại {shape.ID}", InformationType.Error);
                        //    }
                        //    else
                        //        shapeList.Add(shape.ID, shape);
                        //}
                        //else
                        //{WordDoc.SaveAs2
                        //    Module.Helpers.XafXpoHelper.ShowMessage(view.Application, "Lỗi", $"Đã tồn tại {shapeObj.GetType()}", InformationType.Error);
                        //}
                    }
                    foreach (var drawing in shapesList)
                    {
                        // Chỉnh sửa theo nhu cầu để lấy thông tin về shape

                    }

                    //wordDoc.MainDocumentPart.Document.Append(body);
                    // wordDoc.MainDocumentPart.Document.Save();
                }



            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
            }
            finally
            {

            }


        }

        private string GetShapeTypeText(Microsoft.Office.Core.MsoShapeType shapeType)
        {
            string shapeTypeText = System.Enum.GetName(typeof(Microsoft.Office.Core.MsoShapeType), shapeType);
            //Loại bỏ chữ mso ở đầu
            if (!string.IsNullOrEmpty(shapeTypeText))
                return shapeTypeText.Substring(3);
            return "Không xác định";
        }

        private string GetShapeTypeText(Microsoft.Office.Interop.Word.WdInlineShapeType shapeType)
        {
            string shapeTypeText = System.Enum.GetName(typeof(Microsoft.Office.Interop.Word.WdInlineShapeType), shapeType);
            //Loại bỏ chữ WdInlineShape ở đầu
            if (!string.IsNullOrEmpty(shapeTypeText))
                return shapeTypeText.Substring(13);
            return "Không xác định";
        }
        private void ImportInteropMedia(Video video, string choice, System.IO.FileInfo fileInfo, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, System.Diagnostics.Stopwatch stopWatch, BookMark bookMark = null, string waitCaption = " ", bool export = false, System.Collections.Generic.Dictionary<int, Media> listedMedia = null)
        {
            if (listedMedia is null)
                listedMedia = video.MediaList.Where(x => x.Start != null && x.BookMark == bookMark && x.UpperMedia is null).DistinctBy(d => d.Start).ToDictionary(k => System.Convert.ToInt32(k.Start.Value.TotalSeconds), v => v);
            Module.SystemObjects.Tools.ShowOrCloseWaitFormWithCancelButton();
            ShowWaitForm("Đang mở tệp", null, stopWatch.Elapsed, true);
            var paragraphHasShapeList = video.ImportParagraph ? video.ParagraphList.Where(x => x.BookMark == bookMark && !string.IsNullOrEmpty(x.ShapeIdList)) : null;

            string mediaFolder = null;
            if (!export)
            {
                mediaFolder = fileInfo.Directory + "\\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                mediaFolder = Module.Helpers.NameHelper.GetUniqueFileName(mediaFolder);
                if (!System.IO.Directory.Exists(mediaFolder))
                    System.IO.Directory.CreateDirectory(mediaFolder);
            }
            var newMediaFolder = System.IO.Path.Combine(mediaFolder, "media");

            Microsoft.Office.Interop.Word.Application WordApp = null;
            Microsoft.Office.Interop.Word.Documents WordDocs = null;
            Microsoft.Office.Interop.Word.Document WordDoc = null;

            WordApp = new Microsoft.Office.Interop.Word.Application();
            if (System.Diagnostics.Debugger.IsAttached)
                WordApp.Visible = true;
            WordDocs = WordApp.Documents;

            object MissingValue = System.Reflection.Missing.Value;

            object fileName = fileInfo.FullName, oConfirmConversions = false, oReadOnly = true, oAddToRecentFiles = false, oRevert = true, oVisible = true, oOpenAndRepair = true, oNoEncodingDialog = true;

            try
            {
                //Tạo danh sách đã select để tránh bị thay đổi select khi xử lý
                WordDoc = WordDocs.OpenNoRepairDialog(ref fileName, ref oConfirmConversions, ref oReadOnly, ref oAddToRecentFiles, ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref oVisible, ref oOpenAndRepair, ref MissingValue,
                    ref oNoEncodingDialog, ref MissingValue);
                WordApp.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;
                var totalShape = WordDoc.Shapes.Count;
                var nonExportMedia = new System.Collections.Generic.List<Media>();
                var childItemsUngroup = new System.Collections.Generic.List<int>();
                var shapeList = new System.Collections.Generic.Dictionary<int, object>();
                int mediaIndex = 1;
                //var mf = WordDoc.Shapes[1];
                if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                    return;
                ShowWaitForm(waitCaption, null, stopWatch.Elapsed, true);
                int totalShapes = WordDoc.Shapes.Count + WordDoc.InlineShapes.Count;
                foreach (Microsoft.Office.Interop.Word.Shape shapeObj in WordDoc.Shapes)
                {
                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    ShowWaitForm(null, $"Đang nạp Shape {mediaIndex}/{totalShapes}", stopWatch.Elapsed);

                    //Nếu đã tồn tại thì ko nạp nữa
                    if (listedMedia.ContainsKey(mediaIndex))
                        continue;
                    var media = CreateObject<Media>();
                    video.MediaList.Add(media);
                    media.Order = shapeObj.ZOrderPosition;
                    media.Start = System.TimeSpan.FromMilliseconds(shapeObj.Anchor.Start);
                    //shape thì xác định theo id
                    media.ShapeId = shapeObj.ID;
                    if (video.ImportParagraph)
                    {
                        var sId = $"({shapeObj.ID.ToString("D")})";
                        var paragraph = paragraphHasShapeList.FirstOrDefault(x => x.ShapeIdList.Contains(sId));
                        if (paragraph != null)
                            media.Paragraph = paragraph;
                    }
                    media.ShapeName = shapeObj.Name; //2025-02-04: Xác định theo tên để tăng tốc độ
                    media.BookMark = bookMark;
                    media.ShapeTypeText = GetShapeTypeText(shapeObj.Type);
                    media.Width = System.Convert.ToDecimal(shapeObj.Width);
                    media.Height = System.Convert.ToDecimal(shapeObj.Height);
                    media.TextWrappingType = shapeObj.WrapFormat.Type;
                    media.TextWrappingTypeNew = media.TextWrappingType;
                    media.AllowOverlap = shapeObj.WrapFormat.AllowOverlap == 1;

                    if (shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                        media.ResizeWithText = (shapeObj.TextFrame.AutoSize != 0);
                    int bgrFill = shapeObj.Fill.ForeColor.RGB;
                    int bgrLine = shapeObj.Line.ForeColor.RGB;

                    // Chuyển đổi từ BGR sang RGB
                    System.Drawing.Color colorFill = System.Drawing.Color.FromArgb(
                        (bgrFill & 0xFF),            // Red
                        (bgrFill >> 8) & 0xFF,       // Green
                        (bgrFill >> 16) & 0xFF       // Blue
                    );
                    System.Drawing.Color colorLine = System.Drawing.Color.FromArgb(
                        (bgrLine & 0xFF),            // Red
                        (bgrLine >> 8) & 0xFF,       // Green
                        (bgrLine >> 16) & 0xFF       // Blue
                    );

                    // Gán màu cho đối tượng media
                    media.FillColor = colorFill;
                    media.LineColor = colorLine;

                    if (shapeObj.WrapFormat.Type != Microsoft.Office.Interop.Word.WdWrapType.wdWrapInline && shapeObj.Top > -9999)
                    {
                        media.Top = System.Convert.ToDecimal(shapeObj.Top);
                    }
                    //media.Alignment = 
                    media.AlignmentRelative = shapeObj.RelativeHorizontalPosition;
                    //media = CreateObject<ParagraphStyle>();
                    media.MoveWithText = (shapeObj?.RelativeVerticalPosition == Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionLine ||
                                shapeObj?.RelativeVerticalPosition == Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionParagraph);
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        shapeObj.Select();
                        shapeObj.Anchor.Select();
                    }
                    if (shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoPicture)
                    {
                        media.MediaType = MediaType.Image;
                        string result = SaveImage(shapeObj, newMediaFolder, WordApp);
                        if (!string.IsNullOrEmpty(result))
                            media.MediaFile = result;
                    }
                    else if (shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoLinkedPicture)
                    {
                        media.MediaType = MediaType.Image;
                        //shapeObj.Select();
                        //shapeObj.Anchor.Select();
                        media.MediaFile = shapeObj.LinkFormat?.SourceFullName;

                    }
                    else if (shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox ||
                        shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoAutoShape)
                    {
                        media.MediaType = MediaType.TextBox;
                        //shapeObj.Select();
                        //shapeObj.Anchor.Select();
                        // Lấy nội dung từ TextBox
                        var text = shapeObj.TextFrame?.TextRange?.Text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Length > 250)
                                text = text.Substring(0, 250);
                            media.Text += text;
                        }
                    }
                    else if (shapeObj.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                    {
                        media.MediaType = MediaType.Group;

                        // Lấy nội dung từ TextBox trong nhóm
                        string allText = "";
                        // Cập nhật nội dung của media cha
                        if (!string.IsNullOrEmpty(allText))
                        {
                            if (allText.Length > 250)
                                allText = allText.Substring(0, 250);
                            media.Text += allText;
                        }

                        string groupImageResult = ExtractImagesFromGroup(shapeObj.GroupItems, newMediaFolder, WordApp);
                        if (!string.IsNullOrEmpty(groupImageResult))
                            media.MediaFile = groupImageResult;
                    }

                    media.Content = shapeObj.Name;
                    SetTextNextPrevious(shapeObj.Anchor, media);
                    SetMediaText(shapeObj.Anchor, media);
                    mediaIndex++;
                }
                //ShowWaitForm("Đang nạp InlineShape", waitCaption, stopWatch.Elapsed);
                int inlineShapeIndex = -1;
                foreach (Microsoft.Office.Interop.Word.InlineShape shapeObj in WordDoc.InlineShapes)
                {
                    if (Module.SystemObjects.Tools.DefaultSplashScreenManager is null)
                        break;
                    ShowWaitForm(null, $"Đang nạp Shape {mediaIndex}/{totalShapes}", stopWatch.Elapsed, true);
                    //Nếu đã tồn tại thì ko nạp nữa
                    if (listedMedia.ContainsKey(mediaIndex))
                        continue;
                    var media = CreateObject<Media>();
                    video.MediaList.Add(media);
                    //inline thì xác định theo vị trí
                    media.Order = mediaIndex;
                    media.ShapeId = inlineShapeIndex;
                    if (video.ImportParagraph)
                    {
                        var sId = $"({inlineShapeIndex.ToString("D")})";
                        var paragraph = paragraphHasShapeList.FirstOrDefault(x => x.ShapeIdList.Contains(sId));
                        if (paragraph != null)
                            media.Paragraph = paragraph;
                    }
                    //media.ShapeName = shapeObj.Name;//Inline shape không có trường tên
                    media.Start = System.TimeSpan.FromMilliseconds(shapeObj.Range.Start);

                    media.BookMark = bookMark;
                    media.ShapeTypeText = GetShapeTypeText(shapeObj.Type);
                    media.Width = System.Convert.ToDecimal(shapeObj.Width);
                    media.Height = System.Convert.ToDecimal(shapeObj.Height);
                    media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapInline;
                    media.TextWrappingTypeNew = media.TextWrappingType;
                    media.AllowOverlap = false; // Mặc định false

                    // Lấy giá trị màu từ Word (BGR format)
                    int bgrFill = shapeObj.Fill.ForeColor.RGB;
                    int bgrLine = shapeObj.Line.ForeColor.RGB;

                    // Chuyển đổi từ BGR sang RGB
                    System.Drawing.Color colorFill = System.Drawing.Color.FromArgb(
                        (bgrFill & 0xFF),            // Red
                        (bgrFill >> 8) & 0xFF,       // Green
                        (bgrFill >> 16) & 0xFF       // Blue
                    );
                    System.Drawing.Color colorLine = System.Drawing.Color.FromArgb(
                        (bgrLine & 0xFF),            // Red
                        (bgrLine >> 8) & 0xFF,       // Green
                        (bgrLine >> 16) & 0xFF       // Blue
                    );

                    // Gán màu cho đối tượng media
                    media.FillColor = colorFill;
                    media.LineColor = colorLine;


                    //Không có media.Alignment
                    //Không có media.AlignmentRelative
                    //media = CreateObject<ParagraphStyle>();
                    media.MoveWithText = true; //Mặc định bằng true
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        shapeObj.Select();
                        shapeObj.Range.Select();
                    }
                    if (shapeObj.Type == Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapePicture)
                    {
                        media.MediaType = MediaType.Image;
                        string result = SaveImage(shapeObj, newMediaFolder, WordApp);
                        if (!string.IsNullOrEmpty(result))
                            media.MediaFile = result;
                    }
                    else if (shapeObj.Type == Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeLinkedPicture)
                    {
                        media.MediaType = MediaType.Image;
                        media.MediaFile = shapeObj.LinkFormat?.SourceFullName;
                    }
                    //else if (shapeObj.Type == Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeTextBox)
                    //{
                    //    // Lấy nội dung từ TextBox
                    //    //media.Text = shapeObj.TextFrame.TextRange.Text;
                    //}
                    if (!string.IsNullOrEmpty(shapeObj.Range.Text))
                        media.Content = shapeObj.Range.Text;
                    SetTextNextPrevious(shapeObj.Range, media);
                    SetMediaText(shapeObj.Range, media);
                    mediaIndex++;
                    inlineShapeIndex--;
                }

                ShowWaitForm("Đóng văn bản", null, stopWatch.Elapsed, true);

                //WordDoc.SaveAs2(fileName + " - Tiếng Việt.docx");
                object missing = Type.Missing;
                object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

                WordDocs.Close(doNotSaveChanges, missing, missing);
                WordApp.Quit(doNotSaveChanges, missing, missing);
                ShowWaitForm(null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
            }
            finally
            {
                try
                {
                    if (WordDoc != null)
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WordDoc);
                    if (WordDocs != null)
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WordDocs);
                    if (WordApp != null)
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WordApp);
                }
                catch { }

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                ShowWaitForm(null);
            }
        }

        private void SetMediaText(Microsoft.Office.Interop.Word.Range range, Media media, bool force = false)
        {
            if (force || string.IsNullOrEmpty(media.Text))
            {
                if (range.Paragraphs.Count > 0)
                {
                    string allText = "";
                    for (int i = 1; i <= range.Paragraphs.Count; i++)
                    {
                        var paragraph = range.Paragraphs[i];
                        if (!string.IsNullOrEmpty(paragraph.Range.Text) && !string.IsNullOrEmpty(paragraph.Range.Text.Trim()))
                        {
                            allText += paragraph.Range.Text;
                        }
                    }
                    if (!string.IsNullOrEmpty(allText))
                    {
                        if (allText.Length > 250)
                            allText = allText.Substring(0, 250);
                        media.Text += allText;
                    }
                }
            }
        }
        private void SetTextNextPrevious(Microsoft.Office.Interop.Word.Range range, Media media)
        {
            //Văn bản trước
            var previousContent = GetNextRangeInShape(range, false)?.Text;
            if (!string.IsNullOrEmpty(previousContent))
            {
                if (previousContent.Length > 250)
                    previousContent = previousContent.Substring(0, 250);
                media.TextPrevious = previousContent;
            }

            //Văn bản sau
            var nextContent = GetNextRangeInShape(range, true)?.Text;
            if (!string.IsNullOrEmpty(nextContent))
            {
                if (nextContent.Length > 250)
                    nextContent = nextContent.Substring(0, 250);
                media.TextNext = nextContent;
            }

        }
        //private string GetTextInShape(Microsoft.Office.Interop.Word.Range range,bool next)
        //{
        //    //int startIndex = range.Start;
        //    var nextRange = next ? range.Next(Microsoft.Office.Interop.Word.WdUnits.wdParagraph, 1) : range.Previous(Microsoft.Office.Interop.Word.WdUnits.wdParagraph);
        //    if (nextRange != null)
        //    {
        //        var text = nextRange.Text;
        //        if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text.Trim()))
        //        {
        //            if (text.Length > 250)
        //                text = text.Substring(0, 250);
        //            return text;
        //        }
        //        else
        //            return GetTextInShape(nextRange, next);
        //    }
        //    return null;
        //}

        public Microsoft.Office.Interop.Word.Range GetNextRangeInShape(Microsoft.Office.Interop.Word.Range range, bool next)
        {
            //int startIndex = range.Start;
            var nextRange = next ? range.Next(Microsoft.Office.Interop.Word.WdUnits.wdParagraph, 1) : range.Previous(Microsoft.Office.Interop.Word.WdUnits.wdParagraph);
            if (nextRange != null)
            {
                if (ParagraphIsValidate(nextRange))
                    return nextRange;
                else
                    return GetNextRangeInShape(nextRange, next);
            }
            return null;
        }

        public static bool ParagraphIsValidate(Microsoft.Office.Interop.Word.Range paragraphRange)
        {
            if (string.IsNullOrEmpty(paragraphRange.Text))
            {
                //Những paragraph trống hoặc có mục đích phân trang, phân cột không xử lý                                                            
                return false;
            }
            var trimText = paragraphRange.Text.Trim();
            if (string.IsNullOrEmpty(trimText))
            {
                //Những paragraph trống hoặc có mục đích phân trang, phân cột không xử lý                                                            
                return false;
            }
            if (trimText.Length == 1 && !char.IsLetterOrDigit(trimText[0]))
                return false;
            return true;
        }

        public Microsoft.Office.Interop.Word.Range GetNextRangeInShape(Microsoft.Office.Interop.Word.Range range, bool next, ref int level)
        {
            //int startIndex = range.Start;
            var nextRange = next ? range.Next(Microsoft.Office.Interop.Word.WdUnits.wdParagraph, 1) : range.Previous(Microsoft.Office.Interop.Word.WdUnits.wdParagraph);
            if (next) level++;
            else level--;
            if (nextRange != null)
            {
                var text = nextRange.Text;
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text.Trim()))
                {
                    return nextRange;
                }
                else
                    return GetNextRangeInShape(nextRange, next, ref level);
            }
            return null;
        }

        private string ExtractImagesFromGroup(Microsoft.Office.Interop.Word.GroupShapes groupShapes, string outputFolder, Microsoft.Office.Interop.Word.Application wordApp)
        {
            foreach (Microsoft.Office.Interop.Word.Shape shape in groupShapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoPicture)
                {
                    string result = SaveImage(shape, outputFolder, wordApp);
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
                else if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                {
                    string result = ExtractImagesFromGroup(shape.GroupItems, outputFolder, wordApp);
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }
            return null;
        }

        private string SaveImage(Microsoft.Office.Interop.Word.Shape shape, string outputFolder, Microsoft.Office.Interop.Word.Application wordApp)
        {
            try
            {
                string extension = ".jpg"; // Hoặc .png tùy thuộc vào yêu cầu
                string fileName = System.IO.Path.Combine(outputFolder, $"{shape.Name}{extension}");
                byte[] emfBits = (byte[])shape.Anchor.EnhMetaFileBits;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(emfBits))
                {
                    using (System.Drawing.Imaging.Metafile metafile = new System.Drawing.Imaging.Metafile(ms))
                    {
                        if (metafile.RawFormat == System.Drawing.Imaging.ImageFormat.Png)
                            fileName = System.IO.Path.Combine(outputFolder, $"{shape.Name}.png");
                        metafile.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg); // Hoặc ImageFormat.Jpeg
                    }
                }
                return fileName;
            }
            catch (System.Exception) { }
            return null;
        }

        private string SaveImage(Microsoft.Office.Interop.Word.InlineShape shape, string outputFolder, Microsoft.Office.Interop.Word.Application wordApp)
        {
            try
            {
                string extension = ".jpg"; // Hoặc .png tùy thuộc vào yêu cầu
                string fileName = System.IO.Path.Combine(outputFolder, $"{shape.AnchorID}{extension}");
                byte[] emfBits = (byte[])shape.Range.EnhMetaFileBits;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(emfBits))
                {
                    using (System.Drawing.Imaging.Metafile metafile = new System.Drawing.Imaging.Metafile(ms))
                    {
                        if (metafile.RawFormat == System.Drawing.Imaging.ImageFormat.Png)
                            fileName = System.IO.Path.Combine(outputFolder, $"{shape.AnchorID}.png");
                        metafile.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg); // Hoặc ImageFormat.Jpeg
                    }
                }
                return fileName;
            }
            catch (System.Exception) { }
            return null;
        }

        public Microsoft.Office.Core.MsoShapeType ConvertWdInlineShapeTypeToMsoShapeType(Microsoft.Office.Interop.Word.WdInlineShapeType wdType)
        {
            switch (wdType)
            {
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeEmbeddedOLEObject:
                    return Microsoft.Office.Core.MsoShapeType.msoEmbeddedOLEObject;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeLinkedOLEObject:
                    return Microsoft.Office.Core.MsoShapeType.msoLinkedOLEObject;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapePicture:
                    return Microsoft.Office.Core.MsoShapeType.msoPicture;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeLinkedPicture:
                    return Microsoft.Office.Core.MsoShapeType.msoLinkedPicture;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeOLEControlObject:
                    return Microsoft.Office.Core.MsoShapeType.msoOLEControlObject;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeHorizontalLine:
                    return Microsoft.Office.Core.MsoShapeType.msoLine; //Không chính xác
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapePictureHorizontalLine:
                    return Microsoft.Office.Core.MsoShapeType.msoPicture; //Không chính xác
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeLinkedPictureHorizontalLine:
                    return Microsoft.Office.Core.MsoShapeType.msoLinkedPicture; // Không chính xác
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapePictureBullet:
                    return Microsoft.Office.Core.MsoShapeType.msoPicture; //Không chính xác
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeScriptAnchor:
                    return Microsoft.Office.Core.MsoShapeType.msoScriptAnchor;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeOWSAnchor:
                    return Microsoft.Office.Core.MsoShapeType.msoScriptAnchor; // Không chính xác
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeChart:
                    return Microsoft.Office.Core.MsoShapeType.msoLinkedPicture;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeDiagram:
                    return Microsoft.Office.Core.MsoShapeType.msoDiagram;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeLockedCanvas:
                    return Microsoft.Office.Core.MsoShapeType.msoCanvas;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeSmartArt:
                    return Microsoft.Office.Core.MsoShapeType.msoSmartArt;
                case Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeWebVideo:
                    return Microsoft.Office.Core.MsoShapeType.msoWebVideo;
                default:
                    return Microsoft.Office.Core.MsoShapeType.msoTextBox;
            }
        }
        public static void ExportMediaByRichEdit(string filePath, string savePath, System.Collections.Generic.List<Media> listedMedia = null)
        {
            using (DevExpress.XtraRichEdit.RichEditDocumentServer wordProcessor = new DevExpress.XtraRichEdit.RichEditDocumentServer())
            {
                wordProcessor.LoadDocument(filePath);
                var shapeDictionary = wordProcessor.Document.Shapes.ToDictionary(x => x.Id, x => x);
                foreach (var media in listedMedia)
                {
                    if (media.Alignment != media.AlignmentNew)
                    {
                        if (media.ShapeId != null)
                        {
                            DevExpress.XtraRichEdit.API.Native.Shape shape = null;
                            if (media.ShapeId > 0 && shapeDictionary.ContainsKey(media.ShapeId.Value))
                            {
                                shape = shapeDictionary[media.ShapeId.Value];
                            }
                            else if (media.ShapeId < 0 && media.TextWrappingType == Microsoft.Office.Interop.Word.WdWrapType.wdWrapInline)
                            {
                                //Tìm inline shape
                                int inlineShape = -1;
                                foreach (var refShape in shapeDictionary.Values)
                                {
                                    if (refShape.TextWrapping == DevExpress.XtraRichEdit.API.Native.TextWrappingType.InLineWithText)
                                    {
                                        if (inlineShape == media.ShapeId)
                                        {
                                            shape = refShape;
                                            break;
                                        }
                                        inlineShape--;
                                    }
                                }
                            }
                            if (shape != null)
                            {
                                if (media.AlignmentNew == BusinessObjects.Alignment.Left)
                                    shapeDictionary[media.ShapeId.Value].HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Left;
                                else if (media.AlignmentNew == BusinessObjects.Alignment.Right)
                                    shapeDictionary[media.ShapeId.Value].HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Right;
                                else if (media.AlignmentNew == BusinessObjects.Alignment.Centered)
                                    shapeDictionary[media.ShapeId.Value].HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.Center;
                                else if (media.AlignmentNew == BusinessObjects.Alignment.Justified)
                                    shapeDictionary[media.ShapeId.Value].HorizontalAlignment = DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment.None;
                            }
                        }
                    }
                }
                wordProcessor.SaveDocument(savePath, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
            }
        }
        private void ImportExportMedia(Video video, System.Xml.XmlDocument doc, string tempFolder, System.IO.FileInfo fileInfo, System.Collections.Generic.Dictionary<System.Xml.XmlNode, int> wpNodesIndexDictionary, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, System.Diagnostics.Stopwatch stopWatch, BookMark bookMark = null, string waitCaption = " ", bool export = false, System.Collections.Generic.Dictionary<int, Media> listedMedia = null)
        {
            if (listedMedia is null)
                listedMedia = video.MediaList.Where(x => x.Start != null && x.BookMark == bookMark).ToDictionary(k => System.Convert.ToInt32(k.Start.Value.TotalSeconds), v => v);
            int startIndex = 1;
            if (!export)
                ShowWaitForm("Đang nạp hình ảnh", waitCaption, stopWatch.Elapsed);

            var relsDocument = tempFolder + "\\word\\_rels\\document.xml.rels";
            var refMedia = new System.Collections.Generic.Dictionary<string, string>();
            System.Xml.XmlDocument mediaDoc = new System.Xml.XmlDocument();
            mediaDoc.Load(relsDocument);
            var relationshipsNodes = mediaDoc.GetElementsByTagName("Relationship");
            foreach (System.Xml.XmlNode relationshipsNode in relationshipsNodes)
            {
                var target = GetAttributeInNode(relationshipsNode, "Target");
                if (!string.IsNullOrEmpty(target) && target.StartsWith("media/"))
                {
                    var mediaId = GetAttributeInNode(relationshipsNode, "Id");
                    if (!string.IsNullOrEmpty(mediaId))
                        refMedia.Add(mediaId, target.Replace('/', '\\'));
                }
            }
            string mediaFolder = null;
            if (!export)
            {
                mediaFolder = fileInfo.Directory + "\\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                mediaFolder = Module.Helpers.NameHelper.GetUniqueFileName(mediaFolder);
                if (!System.IO.Directory.Exists(mediaFolder))
                    System.IO.Directory.CreateDirectory(mediaFolder);
            }

            bool hasMedia = false;
            string xpathExpression = GetValueOrDefault("ImportShapesXpathExpression", "//w:drawing | //w:object");
            System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            //System.Xml.XmlNodeList nodes = doc.SelectNodes(xpathExpression, namespaceManager);
            //foreach (System.Xml.XmlNode node in nodes.Cast<System.Xml.XmlNode>().ToList())
            //var drawingShapes = doc.GetElementsByTagName("w:drawing");
            var drawingShapes = doc.SelectNodes(xpathExpression, namespaceManager);
            int pictureIndex = 1;
            //Vị trí của Inline sẽ được xác định theo số âm để phân biệt
            int inlineIndex = -1;
            foreach (System.Xml.XmlNode node in drawingShapes)
            {
                //Nếu trong node mc:Fallback thì bỏ qua
                if (GetParentNode(node, "mc:Fallback") != null)
                    continue;
                //Nếu trong node w:drawing thì bỏ qua
                if (GetParentNode(node, "w:drawing", false) != null)
                    continue;
                if (!export)
                {
                    if (listedMedia.ContainsKey(startIndex))
                    {
                        startIndex++;
                        continue;
                    }
                    var media = CreateObject<Media>();
                    video.MediaList.Add(media);
                    media.BookMark = bookMark;
                    media.Start = System.TimeSpan.FromSeconds(startIndex);
                    FillMedia(doc, media, node, mediaFolder, refMedia, wpNodesIndexDictionary, ref hasMedia, ref inlineIndex, export);
                }
                else if (listedMedia.ContainsKey(startIndex))
                {
                    var media = listedMedia[startIndex];
                    if (media.MediaType == MediaType.UnGroup)
                    {
                        if (node.Name == "wpg:wgp" && node.ParentNode != null)
                        {
                            var wrNode = GetParentNode(node, "w:r");
                            var mcAlternateContentNode = GetParentNode(node, "mc:AlternateContent");
                            var drawingNode = GetParentNode(node, "w:drawing");
                            if (wrNode != null && mcAlternateContentNode != null && drawingNode != null &&
                                drawingNode.FirstChild != null && drawingNode.FirstChild.Name == "wp:anchor")
                            {
                                var deleted = new System.Collections.Generic.List<System.Xml.XmlNode>();
                                //Xóa node thừa
                                foreach (System.Xml.XmlNode childNode in node.ChildNodes.Cast<System.Xml.XmlNode>().ToList())
                                {
                                    if (childNode.Name == "wpg:cNvGrpSpPr" || childNode.Name == "wpg:grpSpPr")
                                    {
                                        if (childNode.ParentNode != null)
                                            childNode.ParentNode.RemoveChild(childNode);
                                    }
                                }
                                //Chuyển hết ảnh lên node khác
                                foreach (System.Xml.XmlNode childNode in node.ChildNodes.Cast<System.Xml.XmlNode>().ToList())
                                {
                                    if (childNode.Name == "pic:pic")
                                    {
                                        var newDrawingNode = drawingNode.Clone();
                                        SetNodeAttribute(newDrawingNode.FirstChild, "behindDoc", "1");
                                        foreach (System.Xml.XmlNode picChildNode in newDrawingNode.FirstChild.ChildNodes.Cast<System.Xml.XmlNode>().ToList())
                                        {
                                            if (picChildNode.Name == "wp:wrapNone")
                                            {
                                                childNode.ParentNode.RemoveChild(childNode);
                                            }
                                            else if (picChildNode.Name == "a:graphic")
                                            {
                                                foreach (System.Xml.XmlNode graphicDataNode in picChildNode.ChildNodes)
                                                {
                                                    if (graphicDataNode.Name == "a:graphicData")
                                                    {
                                                        graphicDataNode.RemoveAll();
                                                        graphicDataNode.AppendChild(childNode);
                                                        SetNodeAttribute(graphicDataNode, "uri", "http://schemas.openxmlformats.org/drawingml/2006/picture");
                                                    }
                                                }
                                            }
                                            else if (picChildNode.Name == "wp:docPr")
                                            {
                                                foreach (System.Xml.XmlAttribute att in picChildNode.Attributes)
                                                {
                                                    if (att.Name == "id")
                                                    {

                                                    }
                                                    else if (att.Name == "name")
                                                    {
                                                        att.Value = "Picture " + pictureIndex;
                                                        pictureIndex++;
                                                    }
                                                }
                                            }
                                        }
                                        var wrapNode = childNode.OwnerDocument?.CreateNode(System.Xml.XmlNodeType.Element, "wp:wrapTopAndBottom", drawingNode.FirstChild.NamespaceURI);
                                        newDrawingNode.FirstChild.AppendChild(wrapNode);
                                        //newDrawingNode.FirstChild.InnerXml += "<wp:wrapTopAndBottom/>";
                                        wrNode.InsertBefore(newDrawingNode, mcAlternateContentNode);
                                    }
                                }
                                //Đổi thành text node
                                bool hasTextNode = false;
                                foreach (System.Xml.XmlNode childNode in node.ChildNodes)
                                {
                                    if (childNode.Name == "wps:wsp")
                                    {
                                        hasTextNode = true;
                                        break;
                                    }
                                }
                                if (hasTextNode)
                                {
                                    //Chuyển hết textbox lên graphicData
                                    if (node.ParentNode?.Name == "a:graphicData")
                                    {
                                        SetNodeAttribute(node.ParentNode, "uri", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
                                        foreach (System.Xml.XmlNode wpswspNode in node.ChildNodes.Cast<System.Xml.XmlNode>().ToList())
                                        {
                                            if (wpswspNode.Name == "wps:wsp")
                                            {
                                                node.ParentNode.AppendChild(wpswspNode);
                                            }
                                        }
                                        node.ParentNode.RemoveChild(node);
                                    }

                                    //Chuyển đổi group thành text node                                    
                                    foreach (System.Xml.XmlNode mcNode in mcAlternateContentNode.ChildNodes)
                                    {
                                        if (mcNode.Name == "mc:Choice")
                                        {

                                        }
                                        else if (mcNode.Name == "mc:Fallback" && mcNode.FirstChild?.Name == "w:pict" && mcNode.FirstChild.FirstChild?.Name == "v:group")
                                        {
                                            //Xóa ảnh thừa
                                            foreach (System.Xml.XmlNode vShapeNode in mcNode.FirstChild.FirstChild.ChildNodes.Cast<System.Xml.XmlNode>().ToList())
                                            {
                                                if (vShapeNode.Name == "v:shapetype")
                                                {
                                                    mcNode.FirstChild.AppendChild(vShapeNode);
                                                }
                                                else if (vShapeNode.Name == "v:shape")
                                                {
                                                    if (vShapeNode.FirstChild?.Name == "v:textbox")
                                                    {
                                                        mcNode.FirstChild.AppendChild(vShapeNode);
                                                    }
                                                    else
                                                    {
                                                        vShapeNode.ParentNode?.RemoveChild(vShapeNode);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Xóa group
                                    mcAlternateContentNode.ParentNode?.RemoveChild(mcAlternateContentNode);
                                }
                            }


                        }
                    }
                    else if (media != null)
                    {

                    }
                }
                startIndex++;
            }
            if (!export)
            {
                if (hasMedia && refMedia.Count > 0)
                {
                    //Copy ảnh sang thư mục
                    var oldMediaFolder = System.IO.Path.Combine(tempFolder, "word\\media");
                    var newMediaFolder = System.IO.Path.Combine(mediaFolder, "media");
                    //Di chuyển thư mục cũ sang thư mục mới
                    Module.SystemObjects.Tools.CopyFilesRecursively(oldMediaFolder, newMediaFolder);
                }
                ShowWaitForm(null, null);
            }

        }

        private System.Xml.XmlNodeList GetGroupNode(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            //Dấu chấm . ở đầu biểu thức chỉ rõ rằng việc tìm kiếm bắt đầu từ node hiện tại 
            string xpathExpression = ".//wpg:wgp";
            System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            return currentNode.SelectNodes(xpathExpression, namespaceManager);
        }
        private System.Xml.XmlNodeList GetShapeNode(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            string xpathExpression = ".//a:blip | .//wps:txbx | .//c:chart | .//o:OLEObject | .//w14:contentPart";
            System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
            namespaceManager.AddNamespace("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
            namespaceManager.AddNamespace("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            namespaceManager.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            namespaceManager.AddNamespace("w14", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");

            return currentNode.SelectNodes(xpathExpression, namespaceManager);
        }
        private System.Xml.XmlNodeList GetShapeNodesId(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            string xpathExpression = ".//wp:docPr | .//wp:inline";
            System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            namespaceManager.AddNamespace("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
            namespaceManager.AddNamespace("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            namespaceManager.AddNamespace("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            namespaceManager.AddNamespace("wne", "http://schemas.microsoft.com/office/word/2006/wordml");


            return currentNode.SelectNodes(xpathExpression, namespaceManager);
        }
        private void FillMedia(System.Xml.XmlDocument doc, Media media, System.Xml.XmlNode drawingNode, string mediaFolder, System.Collections.Generic.Dictionary<string, string> refMedia, System.Collections.Generic.Dictionary<System.Xml.XmlNode, int> wpNodesIndexDictionary, ref bool hasMedia, ref int inlineIndex, bool export = false)
        {
            var wpNode = GetParentNode(drawingNode);
            if (wpNode != null)
            {
                if (!export)
                {
                    if (media.MediaStart is null && wpNodesIndexDictionary != null && wpNodesIndexDictionary.ContainsKey(wpNode))
                    {
                        var nodeIndex = wpNodesIndexDictionary[wpNode];
                        media.MediaStart = System.TimeSpan.FromSeconds(nodeIndex);
                    }
                    //media.Order = nodeIndex;
                    if (!string.IsNullOrEmpty(wpNode.InnerText))
                    {
                        var innerText = GetParagraphText(wpNode);
                        //077: Khi nạp ảnh sẽ nạp dữ liệu text: của Paragraph cùng, trước, sau với ảnh vào trường Text, TextPrevious, TextNext: cắt độ dài 250 kí tự nếu dài quá, với TextPrevious thì cắt từ đuôi
                        if (innerText.Length > 250)
                            innerText = innerText.Substring(0, 250);
                        media.Text = innerText;
                    }
                    if (wpNode.NextSibling != null)
                    {
                        var nextNode = wpNode.NextSibling;
                        while (nextNode != null)
                        {
                            if (wpNodesIndexDictionary != null)
                            {
                                if (wpNodesIndexDictionary.ContainsKey(nextNode))
                                {
                                    if (media.MediaStart is null)
                                    {
                                        var nodeIndex = wpNodesIndexDictionary[nextNode];
                                        media.MediaStart = System.TimeSpan.FromSeconds(nodeIndex);
                                        //media.Order = nodeIndex;
                                    }
                                    if (!string.IsNullOrEmpty(nextNode.InnerText))
                                    {
                                        var innerText = GetParagraphText(nextNode);
                                        //077: Khi nạp ảnh sẽ nạp dữ liệu text: của Paragraph cùng, trước, sau với ảnh vào trường Text, TextPrevious, TextNext: cắt độ dài 250 kí tự nếu dài quá, với TextPrevious thì cắt từ đuôi
                                        if (innerText.Length > 250)
                                            innerText = innerText.Substring(0, 250);
                                        media.TextNext = innerText;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(nextNode.InnerText))
                                {
                                    var innerText = GetParagraphText(nextNode);
                                    //077: Khi nạp ảnh sẽ nạp dữ liệu text: của Paragraph cùng, trước, sau với ảnh vào trường Text, TextPrevious, TextNext: cắt độ dài 250 kí tự nếu dài quá, với TextPrevious thì cắt từ đuôi
                                    if (innerText.Length > 250)
                                        innerText = innerText.Substring(0, 250);
                                    media.TextNext = innerText;
                                    break;
                                }
                            }

                            nextNode = nextNode.NextSibling;
                        }
                    }
                    if (wpNode.PreviousSibling != null)
                    {
                        var nextNode = wpNode.PreviousSibling;
                        while (nextNode != null)
                        {
                            if (wpNodesIndexDictionary != null)
                            {
                                if (wpNodesIndexDictionary.ContainsKey(nextNode))
                                {
                                    if (media.MediaStart is null)
                                    {
                                        var nodeIndex = wpNodesIndexDictionary[nextNode];
                                        media.MediaStart = System.TimeSpan.FromSeconds(nodeIndex);
                                        //media.Order = nodeIndex;
                                    }
                                    if (!string.IsNullOrEmpty(nextNode.InnerText))
                                    {
                                        var innerText = GetParagraphText(nextNode);
                                        //077: Khi nạp ảnh sẽ nạp dữ liệu text: của Paragraph cùng, trước, sau với ảnh vào trường Text, TextPrevious, TextNext: cắt độ dài 250 kí tự nếu dài quá, với TextPrevious thì cắt từ đuôi
                                        if (innerText.Length > 250)
                                            innerText = innerText.Substring(innerText.Length - 250);
                                        media.TextPrevious = innerText;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(nextNode.InnerText))
                                {
                                    var innerText = GetParagraphText(nextNode);
                                    //077: Khi nạp ảnh sẽ nạp dữ liệu text: của Paragraph cùng, trước, sau với ảnh vào trường Text, TextPrevious, TextNext: cắt độ dài 250 kí tự nếu dài quá, với TextPrevious thì cắt từ đuôi
                                    if (innerText.Length > 250)
                                        innerText = innerText.Substring(innerText.Length - 250);
                                    media.TextPrevious = innerText;
                                    break;
                                }
                            }

                            nextNode = nextNode.PreviousSibling;
                        }
                    }

                }

            }

            //wp:anchor <wp:wrapTopAndBottom/>
            //wp: inline
            //< wp:positionV relativeFrom = "page" > : Fix postion on Page
            //<wp:positionV relativeFrom = "paragraph" > Move with text
            if (drawingNode.FirstChild.Name == "wp:anchor")
            {
                //behindDoc="0"
                var behindDocValue = GetAttributeInNode(drawingNode.FirstChild, "behindDoc");
                if (!string.IsNullOrEmpty(behindDocValue))
                {
                    if (behindDocValue == "1")
                    {
                        //media.ObjectLayout = ObjectLayout.BehindText;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapBehind;
                    }
                    else if (behindDocValue == "0")
                    {
                        //media.ObjectLayout = ObjectLayout.InfrontOfText;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapFront;
                    }
                }
                var allowOverlapValue = GetAttributeInNode(drawingNode.FirstChild, "allowOverlap");
                if (!string.IsNullOrEmpty(allowOverlapValue))
                {
                    media.AllowOverlap = allowOverlapValue == "1";
                }
                foreach (System.Xml.XmlNode childNode in drawingNode.FirstChild.ChildNodes)
                {
                    if (childNode.Name == "wp:wrapTopAndBottom")
                    {
                        //media.ObjectLayout = ObjectLayout.TopAndBottom;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapTopBottom;
                    }
                    else if (childNode.Name == "wp:wrapSquare")
                    {
                        //<wp:wrapSquare wrapText="bothSides"/>
                        //media.ObjectLayout = ObjectLayout.Square;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapSquare;
                    }
                    else if (childNode.Name == "wp:wrapTight")
                    {
                        //<wp:wrapTight wrapText="bothSides">
                        //	<wp:wrapPolygon edited="0">
                        //		<wp:start x="0" y="0"/>
                        //		<wp:lineTo x="0" y="21541"/>
                        //		<wp:lineTo x="21531" y="21541"/>
                        //		<wp:lineTo x="21531" y="0"/>
                        //		<wp:lineTo x="0" y="0"/>
                        //	</wp:wrapPolygon>
                        //</wp:wrapTight>
                        //media.ObjectLayout = ObjectLayout.Tight;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapTight;
                    }
                    else if (childNode.Name == "wp:wrapThrough")
                    {
                        //<wp:wrapThrough wrapText="bothSides">
                        //	<wp:wrapPolygon edited="0">
                        //		<wp:start x="0" y="0"/>
                        //		<wp:lineTo x="0" y="21541"/>
                        //		<wp:lineTo x="21531" y="21541"/>
                        //		<wp:lineTo x="21531" y="0"/>
                        //		<wp:lineTo x="0" y="0"/>
                        //	</wp:wrapPolygon>
                        //</wp:wrapThrough>
                        //media.ObjectLayout = ObjectLayout.Through;
                        media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapThrough;
                    }
                    else if (childNode.Name == "wp:wrapNone")
                    {
                        //<wp:wrapNone/>
                    }
                    else if (childNode.Name == "wp:positionH")
                    {
                        foreach (System.Xml.XmlNode alignNode in childNode.ChildNodes)
                        {
                            if (alignNode.Name == "wp:align")
                            {
                                if (!export)
                                {
                                    if (alignNode.InnerText == "center")
                                        media.Alignment = BusinessObjects.Alignment.Centered;
                                    else if (alignNode.InnerText == "right")
                                        media.Alignment = BusinessObjects.Alignment.Right;
                                    else if (alignNode.InnerText == "left")
                                        media.Alignment = BusinessObjects.Alignment.Left;
                                    media.AlignmentNew = media.Alignment;
                                }
                                else
                                {
                                    var currentAlign = System.Enum.GetName(typeof(Module.BusinessObjects.Alignment), media.Alignment).ToLower();
                                    if (alignNode.InnerText != currentAlign)
                                        alignNode.InnerText = currentAlign;
                                }

                            }
                        }
                        var relativeFrom = GetAttributeInNode(childNode, "relativeFrom");
                        if (!string.IsNullOrEmpty(relativeFrom))
                        {
                            //var relativeFromValue = (AlignmentRelative)Enum.Parse(typeof(AlignmentRelative), relativeFrom);
                            switch (relativeFrom)
                            {
                                case "page":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionPage;
                                    break;
                                case "margin":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionMargin;
                                    break;
                                case "column":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionColumn;
                                    break;
                                case "character":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionCharacter;
                                    break;
                                case "leftMargin":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionLeftMarginArea;
                                    break;
                                case "rightMargin":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionRightMarginArea;
                                    break;
                                case "insideMargin":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionInnerMarginArea;
                                    break;
                                case "outsideMargin":
                                    media.AlignmentRelative = Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionOuterMarginArea;
                                    break;
                            }
                        }
                    }
                    else if (childNode.Name == "wp:positionV")
                    {
                        var relativeFrom = GetAttributeInNode(childNode, "relativeFrom");
                        if (!string.IsNullOrEmpty(relativeFrom))
                        {
                            media.MoveWithText = (relativeFrom == "paragraph" || relativeFrom == "line");
                        }
                    }
                    else if (childNode.Name == "wp:docPr")
                    {
                        //Dùng trường Order để lưu Id của node
                        var id = GetAttributeInNode(childNode, "id");
                        if (!string.IsNullOrEmpty(id))
                        {
                            media.ShapeId = System.Convert.ToInt32(id);
                        }
                        //2025-02-04: Dùng trường tên dể định danh
                        var sName = GetAttributeInNode(childNode, "name");
                        if (!string.IsNullOrEmpty(sName))
                        {
                            media.ShapeName = sName;
                        }
                    }
                    else if (childNode.Name == "wp:extent")
                    {
                        //Quy đổi ra inch theo hệ số 914400
                        var cxValue = GetAttributeInNode(childNode, "cx");
                        if (!string.IsNullOrEmpty(cxValue))
                            media.Width = Convert.ToDecimal(cxValue) / 12700;//Quy ra point// / 914400;
                        var cyValue = GetAttributeInNode(childNode, "cy");
                        if (!string.IsNullOrEmpty(cyValue))
                            media.Height = Convert.ToDecimal(cyValue) / 12700;//Quy ra point// / 914400;
                    }
                }
            }
            else if (drawingNode.FirstChild.Name == "wp:inline")
            {
                //Tất cả các thuộc tính khác đều không áp dụng
                media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapInline;
                media.MoveWithText = true; //Mặc định bằng true nếu la inline
                media.ShapeId = inlineIndex;
                //media.Order = inlineIndex;
                inlineIndex--;
                foreach (System.Xml.XmlNode childNode in drawingNode.FirstChild.ChildNodes)
                {
                    if (childNode.Name == "wp:docPr")
                    {
                        //Dùng trường Order để lưu Id của node
                        var id = GetAttributeInNode(childNode, "id");
                        if (!string.IsNullOrEmpty(id))
                        {
                            //Không lưu lại vì không có tác dụng trong interop
                            //media.ShapeId = System.Convert.ToInt32(id);
                        }
                        var sName = GetAttributeInNode(childNode, "name");
                        if (!string.IsNullOrEmpty(sName))
                        {
                            //Không lưu lại vì không có tác dụng trong interop
                            media.ShapeName = sName;
                        }
                    }
                    else if (childNode.Name == "wp:extent")
                    {
                        //Quy đổi ra inch theo hệ số 914400
                        var cxValue = GetAttributeInNode(childNode, "cx");
                        if (!string.IsNullOrEmpty(cxValue))
                            media.Width = Convert.ToDecimal(cxValue) / 12700;//Quy ra point// / 914400;
                        var cyValue = GetAttributeInNode(childNode, "cy");
                        if (!string.IsNullOrEmpty(cyValue))
                            media.Height = Convert.ToDecimal(cyValue) / 12700;//Quy ra point// / 914400;
                    }
                }
            }
            else if (drawingNode.Name == "w:object")
            {
                media.TextWrappingType = Microsoft.Office.Interop.Word.WdWrapType.wdWrapInline;
                media.MoveWithText = true; //Mặc định bằng true nếu la inline
                media.ShapeId = inlineIndex;
                //media.Order = inlineIndex;
                inlineIndex--;
            }

            //string mediaContent = null;
            //System.TimeSpan? mediaStart = null;
            //MediaType mediaType = MediaType.Video;
            //Microsoft.Office.Core.MsoShapeType shapeType = Microsoft.Office.Core.MsoShapeType.msoAutoShape;
            var groupNodes = GetGroupNode(doc, drawingNode);
            if (groupNodes.Count > 0)
            {

                if (!export)
                {
                    media.MediaType = MediaType.Group;
                    media.ShapeTypeText = "Group";
                    string mediaContent = GetParagraphText(groupNodes[0]);
                    if (mediaContent.Length > 250)
                        mediaContent = mediaContent.Substring(0, 250);
                    media.Content = mediaContent;
                }

            }
            else
            {
                var shapeNodes = GetShapeNode(doc, drawingNode);
                if (shapeNodes.Count > 0)
                {
                    foreach (System.Xml.XmlNode node in shapeNodes)
                    {
                        if (node.Name == "a:blip")
                        {
                            hasMedia = true;
                            //Nếu là trong group thì hủy                                    
                            if (GetParentNode(node, ":wgp") != null)
                                continue;
                            var embedId = GetAttributeInNode(node, "r:embed");
                            if (!string.IsNullOrEmpty(embedId) && refMedia.ContainsKey(embedId))
                                media.MediaFile = mediaFolder + "\\" + refMedia[embedId];
                            //mediaType = MediaType.Image;
                            //shapeType = Microsoft.Office.Core.MsoShapeType.msoPicture;
                            media.MediaType = MediaType.Image;
                            media.ShapeTypeText = "Picture";

                        }
                        else if (node.Name == "wps:txbx")
                        {
                            //Nếu là trong group thì hủy                                    
                            //if (GetParentNode(node, ":wgp") != null)
                            //    continue;
                            if (!export)
                            {
                                media.MediaType = MediaType.TextBox;
                                //shapeType = Microsoft.Office.Core.MsoShapeType.msoTextBox;
                                foreach (System.Xml.XmlNode txbxContentNode in node)
                                {
                                    foreach (System.Xml.XmlNode childNode in txbxContentNode)
                                    {
                                        if (childNode.Name == "w:p")
                                        {
                                            string mediaContent = GetParagraphText(childNode);
                                            if (mediaContent.Length > 250)
                                                mediaContent = mediaContent.Substring(0, 250);
                                            media.Content = mediaContent;
                                            if (wpNodesIndexDictionary != null && wpNodesIndexDictionary.ContainsKey(childNode))
                                            {
                                                var nodeIndex = wpNodesIndexDictionary[childNode];
                                                media.MediaStart = System.TimeSpan.FromSeconds(nodeIndex);
                                            }
                                        }
                                    }
                                }
                                media.ShapeTypeText = "TextBox";
                            }

                        }

                        else if (node.Name == "c:chart")
                        {
                            //mediaType = MediaType.Video;
                            //shapeType = Microsoft.Office.Core.MsoShapeType.msoChart;
                            media.ShapeTypeText = "Chart";
                        }
                        else if (node.Name == "o:OLEObject")
                        {
                            //mediaType = MediaType.Video;
                            //shapeType = Microsoft.Office.Core.MsoShapeType.msoOLEControlObject;
                            media.ShapeTypeText = "OLEObject";
                        }
                        else if (node.Name == "w14:contentPart")
                        {
                            //mediaType = MediaType.Video;
                            //Cần xem lại loại này
                            //shapeType = DevExpress.XtraRichEdit.API.Native.ShapeType.Connector;
                            //shapeType = Microsoft.Office.Core.MsoShapeType.msoCanvas;
                            media.ShapeTypeText = "Canvas";
                        }
                    }
                }
                else
                {
                    media.ShapeTypeText = "Không xác định";
                }

            }



            //media.MediaType = mediaType;
            //media.ShapeTypeText = GetShapeTypeText(shapeType);
            //if (!string.IsNullOrEmpty(embedId) && refMedia.ContainsKey(embedId))
            //    media.MediaFile = mediaFolder + "\\" + refMedia[embedId];
            //if (!string.IsNullOrEmpty(mediaContent))
            //{
            //    if (mediaContent.Length > 250)
            //        mediaContent = mediaContent.Substring(0, 250);
            //    media.Content = mediaContent;
            //}
            //if (mediaStart != null)
            //    media.MediaStart = mediaStart;
            //var picNode = GetParentNode(mediaNode, "pic:pic");
            //if(picNode is null)
            //    picNode = GetParentNode(mediaNode, "wps:wsp"); ;
            //if (picNode != null)
            //{
            //    foreach (System.Xml.XmlNode childNode in picNode.ChildNodes)
            //    {
            //        if (childNode.Name.EndsWith(":spPr"))
            //        {
            //            foreach (System.Xml.XmlNode xfrmNode in childNode.ChildNodes)
            //            {
            //                if (xfrmNode.Name == "a:xfrm")
            //                {
            //                    foreach (System.Xml.XmlNode extNode in xfrmNode.ChildNodes)
            //                    {
            //                        if (extNode.Name == "a:ext")
            //                        {
            //                            //Quy đổi ra inch theo hệ số 914400
            //                            var cxValue = GetAttributeInNode(extNode, "cx");
            //                            if (!string.IsNullOrEmpty(cxValue))
            //                                media.Width = Convert.ToDecimal(cxValue) / 914400;
            //                            var cyValue = GetAttributeInNode(extNode, "cy");
            //                            if (!string.IsNullOrEmpty(cyValue))
            //                                media.Height = Convert.ToDecimal(cyValue) / 914400;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

        }

        private string GetParagraphText(System.Xml.XmlNode wpNode, bool checkTextBox = true)
        {
            var text = "";
            if (wpNode != null)
            {
                System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(wpNode.OwnerDocument.NameTable);
                namespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                if (wpNode.Name == "w:p")
                {
                    //Dấu chấm . ở đầu biểu thức chỉ rõ rằng việc tìm kiếm bắt đầu từ node hiện tại (paragraphNode).
                    string xpathExpression = ".//w:t | .//w:br | .//w:tab";

                    System.Xml.XmlNodeList nodes = wpNode.SelectNodes(xpathExpression, namespaceManager);
                    foreach (System.Xml.XmlNode textNodeChild in nodes)
                    {
                        if (checkTextBox && IsTextBox(textNodeChild))
                            continue;
                        if (textNodeChild.Name == "w:t")
                        {
                            text += textNodeChild.InnerText;
                        }
                        else if (textNodeChild.Name == "w:br")
                        {
                            text += "\r\n";
                        }
                        else if (textNodeChild.Name == "w:tab")
                        {
                            text += "\t";
                        }
                    }
                    //var textNode = wpNode.FirstChild;
                    //while (textNode != null)
                    //{
                    //    if (textNode.Name == "w:r")
                    //    {
                    //        var textNodeChild = textNode.FirstChild;
                    //        while (textNodeChild != null)
                    //        {
                    //            if (textNodeChild.Name == "w:t")
                    //            {
                    //                text += textNodeChild.InnerText;
                    //            }
                    //            else if (textNodeChild.Name == "w:br")
                    //            {
                    //                text += "\r\n";
                    //            }
                    //            else if (textNodeChild.Name == "w:tab")
                    //            {
                    //                text += "\t";
                    //            }
                    //            textNodeChild = textNodeChild.NextSibling;
                    //        }
                    //    }
                    //    textNode = textNode.NextSibling;
                    //}
                }
                else
                {
                    //Dấu chấm . ở đầu biểu thức chỉ rõ rằng việc tìm kiếm bắt đầu từ node hiện tại (paragraphNode).
                    string xpathExpression = ".//w:p";
                    System.Xml.XmlNodeList nodes = wpNode.SelectNodes(xpathExpression, namespaceManager);
                    foreach (System.Xml.XmlNode textNodeChild in nodes)
                    {
                        //IsTextBox(textNodeChild);
                        //Chỉ bỏ text trong text box
                        var textBoxNode = GetParentNode(textNodeChild, ":textbox");
                        if (textBoxNode != null)
                            continue;
                        else
                        {
                            var resultText = GetParagraphText(textNodeChild, false);
                            if (!string.IsNullOrEmpty(resultText))
                            {
                                if (!string.IsNullOrEmpty(text))
                                    text += "\r\n\r\n";
                                text += resultText;
                            }
                        }
                    }

                }
            }
            return text;
        }

        public ParagraphStyle FindExistParagraphStyle(Video video, ParagraphStyle paragraphStyle, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, ParagraphStyleType? paragraphStyleType = null)
        {
            //2024-08-29                               
            if (!string.IsNullOrEmpty(paragraphStyle.Name))
            {
                //2004-08-30: Nếu có tên thì kiểm tra xem tên đã tồn tại chưa
                var existedStyle = listExistedParagraphStyle.FirstOrDefault(m => m.Name == paragraphStyle.Name);
                if (existedStyle != null)
                    return existedStyle;
            }
            foreach (var style in listExistedParagraphStyle)
            {
                //2023-09-07 Bỏ kiểm tra theo tên, vì có trường upper style
                if (!string.IsNullOrEmpty(style.Name))
                    continue;
                if (paragraphStyleType != null && style.ParagraphStyleType != ParagraphStyleType.Empty)
                {
                    if (paragraphStyle.ParagraphStyleType == ParagraphStyleType.Paragraph)
                    {
                        if (style.ParagraphStyleType != ParagraphStyleType.Paragraph &&
                            style.ParagraphStyleType != ParagraphStyleType.Linked)
                            continue;
                    }
                    else if (paragraphStyle.ParagraphStyleType == ParagraphStyleType.Character)
                    {
                        if (style.ParagraphStyleType != ParagraphStyleType.Character &&
                            style.ParagraphStyleType != ParagraphStyleType.Linked)
                            continue;
                    }
                    else if (paragraphStyle.ParagraphStyleType != style.ParagraphStyleType)
                    {
                        continue;
                    }
                }
                //if (!string.IsNullOrEmpty(paragraphStyle.Name))
                //{
                //    //Trường hợp custom style
                //    if (paragraphStyle.Name == style.Name)
                //    {
                //        existStyle = style;
                //        break;
                //    }
                //}
                //else 
                if (style.Font == paragraphStyle.Font && style.Size == paragraphStyle.Size
                    //&& style.UpperStyle == paragraphStyle.UpperStyle
                    && style.Color == paragraphStyle.Color && style.Bold == paragraphStyle.Bold
                    && style.Italic == paragraphStyle.Italic && style.Underline == paragraphStyle.Underline)
                {
                    //2025 - 03 - 27: Xử lý theo điều kiện
                    if (video.Outline && (style.Outline != paragraphStyle.Outline))
                        continue;
                    if (video.Indent && (style.IndentLeft != paragraphStyle.IndentLeft || style.IndentRight != paragraphStyle.IndentRight
                                    || style.IndentFirstLine != paragraphStyle.IndentFirstLine))
                        continue;
                    if (video.Spacing && (style.SpacingAfter != paragraphStyle.SpacingAfter || style.SpacingLineAt != paragraphStyle.SpacingLineAt
                                    || style.SpacingBefore != paragraphStyle.SpacingBefore || style.SpacingBefore != paragraphStyle.SpacingAfter))
                        continue;

                    //2023-06-28: Bổ sung thêm style cho paragraph
                    //if (style.Spacing == paragraphStyle.Spacing
                    //    && style.Indentation == paragraphStyle.Indentation
                    //    && style.Alignment == paragraphStyle.Alignment)
                    //2023-06-29:
                    //- Xác định Style chỉ căn cứ vào Font và BusinessObjects.Alignment, BusinessObjects.Alignment thì chuyển eNum: Trái / Giữa / Phải để dễ nhìn
                    //-Indentation và Spacing sẽ tuân theo Style mới đầu tiên tìm thấy: có thể bỏ 2 trường này và âm thầm lưu giá trị vào Style khi xuất file thôi
                    if (style.Alignment == paragraphStyle.Alignment)
                    {
                        if (!video.RightIndent || style.IndentRight == paragraphStyle.IndentRight)
                        {
                            //Kiểm tra thêm trường hợp parent Style
                            if (paragraphStyle.UpperStyle != null && style.UpperStyle != paragraphStyle.UpperStyle)
                                continue;
                            return style;
                            break;
                        }
                    }
                }
            }
            return null;
        }

        public void ImportPowerPoint(Video video, string url, System.Diagnostics.Stopwatch stopWatch, ref int index, ref int styleIndex, BookMark bookMark = null, string waitCaption = " ", string choice = "")
        {
            //Nạp dữ liệu cho PowerPoint

            char prefix = 'a';
            var fileInfo = new System.IO.FileInfo(url);
            var tempFolder = System.IO.Directory.GetCurrentDirectory() + "\\Temp\\" + fileInfo.Name;
            //if (!System.IO.Directory.Exists(tempFolder))
            //    System.IO.Directory.CreateDirectory(tempFolder);                        
            //System.IO.Compression.ZipFile.ExtractToDirectory(url, tempFolder, true);
            Module.SystemObjects.Tools.ZipFileExtractToDirectory(url, tempFolder, true);

            System.Collections.Generic.IDictionary<System.Xml.XmlNode, bool> flagNodes = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, bool>();
            ShowWaitForm("Đang nạp kiểu cách", waitCaption, stopWatch.Elapsed);
            var paragraphStyleListAdd = new System.Collections.Generic.List<ParagraphStyle>();
            var paragraphStyleEmptyNameList = video.ParagraphStyleList.Where(m => m.Link == bookMark).ToList();
            //paragraphStyleEmptyNameList = new System.Collections.Generic.List<ParagraphStyle>();
            //Nạp 2 ParagraphStyle mặc định chỉ nhập font:
            //-Title Font: majorFont
            //- Body Font: minorFont                    
            var xmlThemeFile = tempFolder + "\\ppt\\theme\\theme1.xml";
            System.Xml.XmlDocument themeDoc = new System.Xml.XmlDocument();
            themeDoc.Load(xmlThemeFile);
            ParagraphStyle titleFontParagraphStyle = null;
            ParagraphStyle bodyFontParagraphStyle = null;
            foreach (System.Xml.XmlNode node in themeDoc.ChildNodes)
            {
                if (node.Name == prefix + ":theme" && node.FirstChild != null && node.FirstChild.Name == prefix + ":themeElements")
                {
                    foreach (System.Xml.XmlNode fontScheme in node.FirstChild.ChildNodes)
                    {
                        if (fontScheme.Name == prefix + ":fontScheme")
                        {
                            foreach (System.Xml.XmlNode fontNode in fontScheme.ChildNodes)
                            {
                                if (fontNode.Name == prefix + ":majorFont")
                                {
                                    if (fontNode.FirstChild != null && fontNode.FirstChild.Name == prefix + ":latin")
                                    {
                                        var fontName = GetAttributeInNode(fontNode.FirstChild, "typeface");
                                        if (!string.IsNullOrEmpty(fontName))
                                        {
                                            titleFontParagraphStyle = CreateObject<ParagraphStyle>();
                                            if (bookMark != null)
                                                titleFontParagraphStyle.Link = bookMark;
                                            titleFontParagraphStyle.Name = "Title Font";
                                            titleFontParagraphStyle.Font = fontName;
                                            //titleFontParagraphStyle.Video = video;
                                            paragraphStyleListAdd.Add(titleFontParagraphStyle);
                                        }
                                    }
                                }
                                else if (fontNode.Name == prefix + ":minorFont")
                                {
                                    var fontName = GetAttributeInNode(fontNode.FirstChild, "typeface");
                                    if (!string.IsNullOrEmpty(fontName))
                                    {
                                        bodyFontParagraphStyle = CreateObject<ParagraphStyle>();
                                        if (bookMark != null)
                                            bodyFontParagraphStyle.Link = bookMark;
                                        bodyFontParagraphStyle.Name = "Body Font";
                                        bodyFontParagraphStyle.Font = fontName;
                                        //titleFontParagraphStyle.Video = video;
                                        paragraphStyleListAdd.Add(bodyFontParagraphStyle);
                                    }
                                }
                            }
                        }

                    }
                }
            }


            var slidesRefFolder = tempFolder + "\\ppt\\slides\\_rels";
            string slideRefLayoutFolder = tempFolder + "\\ppt\\slideLayouts\\_rels\\";
            var slideRefNames = System.IO.Directory.GetFiles(slidesRefFolder, "*.rels");
            var slideRefNamesWithSort = slideRefNames.OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
                                    .ThenBy(x =>
                                    {
                                        int number;
                                        if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                                            return number;
                                        return -1;
                                    }).ToList();
            var dictionarySlides = new System.Collections.Generic.Dictionary<string, string>();
            //var dictionarySlideLayouts = new System.Collections.Generic.Dictionary<string, ParagraphStyle>();
            var dictionarySlideLayoutsWithLayouts = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, ParagraphStyle>>();
            var slideLayoutNames = new System.Collections.Generic.List<string>();
            var slideLayoutSlideMasterDictionary = new System.Collections.Generic.Dictionary<string, string>();
            var slideMasterLayout = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ParagraphStyle>>();
            foreach (var slideRefName in slideRefNamesWithSort)
            {
                themeDoc.Load(slideRefName);
                var relationshipNodes = themeDoc.GetElementsByTagName("Relationship");
                if (relationshipNodes.Count > 0)
                {
                    var slideLayoutName = GetAttributeInNode(relationshipNodes[0], "Target");
                    if (!string.IsNullOrEmpty(slideLayoutName))
                    {
                        slideLayoutName = slideLayoutName.Replace("../slideLayouts/", "");
                        if (!slideLayoutNames.Contains(slideLayoutName))
                        {
                            slideLayoutNames.Add(slideLayoutName);
                            dictionarySlideLayoutsWithLayouts.Add(slideLayoutName, new System.Collections.Generic.Dictionary<string, ParagraphStyle>());
                            var slideLayoutMasterFile = slideRefLayoutFolder + slideLayoutName + ".rels";
                            if (System.IO.File.Exists(slideLayoutMasterFile))
                            {
                                themeDoc.Load(slideLayoutMasterFile);
                                relationshipNodes = themeDoc.GetElementsByTagName("Relationship");
                                if (relationshipNodes.Count > 0)
                                {
                                    var slideMasterName = GetAttributeInNode(relationshipNodes[0], "Target");
                                    if (!string.IsNullOrEmpty(slideMasterName))
                                    {
                                        slideMasterName = slideMasterName.Replace("../slideMasters/", "");
                                        if (!slideLayoutSlideMasterDictionary.ContainsKey(slideLayoutName))
                                        {
                                            slideLayoutSlideMasterDictionary.Add(slideLayoutName, slideMasterName);
                                            if (!slideMasterLayout.ContainsKey(slideMasterName))
                                            {
                                                slideMasterLayout.Add(slideMasterName, new System.Collections.Generic.List<ParagraphStyle>());
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        string slideName = slideRefName.Substring(slideRefName.LastIndexOf('\\') + 1).Replace(".rels", "");
                        dictionarySlides.Add(slideName, slideLayoutName);
                    }
                }
            }
            //Nạp dữ liệu cho slideMaster
            string slideMasterFolder = tempFolder + "\\ppt\\slideMasters\\";
            foreach (var slideMasterName in slideMasterLayout.Keys)
            {
                var slideLayoutMasterFile = slideMasterFolder + slideMasterName;
                if (System.IO.File.Exists(slideLayoutMasterFile))
                {
                    themeDoc.Load(slideLayoutMasterFile);
                    var txStylesNodes = themeDoc.GetElementsByTagName("p:txStyles");
                    if (txStylesNodes.Count > 0)
                    {
                        string masterName = "SM" + slideMasterName.Replace("slideMaster", "").Replace(".xml", "");
                        foreach (System.Xml.XmlNode lstStyleNode in txStylesNodes[0].ChildNodes)
                        {
                            if (lstStyleNode.Name == "p:titleStyle")
                            {
                                if (lstStyleNode.FirstChild != null && lstStyleNode.FirstChild.Name == "a:lvl1pPr")
                                {
                                    var newParagraphStyle = CreateObject<ParagraphStyle>();
                                    if (bookMark != null)
                                        newParagraphStyle.Link = bookMark;
                                    newParagraphStyle.Name = masterName + ".title";
                                    if (FillParagraphStyleInLvlpPrNode(video, lstStyleNode.FirstChild, newParagraphStyle, bookMark))
                                    {
                                        paragraphStyleListAdd.Add(newParagraphStyle);
                                        slideMasterLayout[slideMasterName].Add(newParagraphStyle);
                                        if (titleFontParagraphStyle != null)
                                            newParagraphStyle.UpperStyle = titleFontParagraphStyle;
                                    }
                                    else
                                    {
                                        newParagraphStyle.Delete();
                                    }
                                }

                            }
                            else if (lstStyleNode.Name == "p:bodyStyle")
                            {
                                foreach (System.Xml.XmlNode lvlpPrNode in lstStyleNode.ChildNodes)
                                {
                                    if (lvlpPrNode.Name.StartsWith("a:lvl") && lvlpPrNode.Name.EndsWith("pPr"))
                                    {
                                        int level = System.Convert.ToInt32(lvlpPrNode.Name.Replace("a:lvl", "").Replace("pPr", ""));
                                        var newParagraphStyle = CreateObject<ParagraphStyle>();
                                        if (bookMark != null)
                                            newParagraphStyle.Link = bookMark;
                                        newParagraphStyle.Name = masterName + ".body";
                                        if (level > 1)
                                            newParagraphStyle.Name += ".LV" + (level - 1);
                                        else if (bodyFontParagraphStyle != null)
                                            newParagraphStyle.UpperStyle = bodyFontParagraphStyle;
                                        if (FillParagraphStyleInLvlpPrNode(video, lvlpPrNode, newParagraphStyle, bookMark))
                                        {
                                            paragraphStyleListAdd.Add(newParagraphStyle);
                                            slideMasterLayout[slideMasterName].Add(newParagraphStyle);
                                        }
                                        else
                                        {
                                            newParagraphStyle.Delete();
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
            }

            var upperElements = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, Audio>();
            var upperElementsMultiChildList = new System.Collections.Generic.List<System.Guid>();
            var upperElementsAdjacentList = new System.Collections.Generic.List<System.Guid>();
            var upperElementContentForStyle = new System.Collections.Generic.Dictionary<System.Guid, string>();
            var audioList = new System.Collections.Generic.List<Audio>();
            var paragraphStyleUsingList = new System.Collections.Generic.List<ParagraphStyle>();
            decimal countNumber = 0;
            foreach (var xmlSlide in dictionarySlides.Keys)
            {
                var xmlFile = tempFolder + "\\ppt\\slides\\" + xmlSlide;

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(xmlFile);
                //System.Xml.XmlNode rootNode;
                //if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].ChildNodes.Count == 1)
                //    rootNode = doc.ChildNodes[1].ChildNodes[0];
                //else
                //    rootNode = doc.ChildNodes[0];
                //order = 1;
                var dictionaryLayoutInSlide = new System.Collections.Generic.Dictionary<string, ParagraphStyle>();

                //2023-06-14: Lấy dữ liệu theo a-t                        
                OptimalDocument(video, doc, flagNodes, prefix);
                //if (xmlSlide == "slide45.xml" || xmlSlide == "slide47.xml")
                //{

                //}
                var wtNodes = doc.GetElementsByTagName(prefix + ":t");


                System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>> parentNodeList = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>>();
                foreach (System.Xml.XmlNode node in wtNodes)
                {
                    var parentNode = GetParentNode(node, prefix + ":p");
                    if (parentNode != null)
                    {
                        if (parentNodeList.ContainsKey(parentNode))
                        {
                            parentNodeList[parentNode].Add(node);
                        }
                        else
                        {
                            parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                        }
                    }
                }

                foreach (var wpNode in parentNodeList.Keys)
                {
                    var nodeList = parentNodeList[wpNode];
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        if (!WtContentIsValidate(video, nodeList[i]))
                            continue;

                        var audio = new Audio(video.Session);
                        audioList.Add(audio);
                        //audio.Video = video;
                        //video.AudioList.Add(audio);
                        string content = nodeList[i].InnerText.Replace(" ", " ");
                        if (!video.KeepSpace)
                            content = content.Trim();
                        audio.Content = content;
                        //audio.Order = index;
                        audio.Start = TimeSpan.FromSeconds(index);
                        if (bookMark != null)
                            audio.BookMark = bookMark;
                        audio.Flag2 = false;
                        if (flagNodes.ContainsKey(nodeList[i]))
                        {
                            audio.Flag2 = flagNodes[nodeList[i]];
                            audio.Note2 = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note2, importCharTag, "Bỏ style đậm, nghiêng, gạch");
                        }
                        if (System.Diagnostics.Debugger.IsAttached)
                        {

                        }
                        //2023-08-29
                        //Đánh dấu trường NotAdjacent khi
                        //-Paragraph chứa tag ở cuối
                        if (nodeList[i].NextSibling != null && nodeList[i].NextSibling.Name == prefix + ":tab")
                            audio.NotAdjacent = true;
                        else if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.LastChild != null && nodeList[i].ParentNode.LastChild.Name == prefix + ":tab")
                            audio.NotAdjacent = true;

                        if (nodeList[i].ParentNode != null && nodeList[i].ParentNode.Name == prefix + ":r")
                        {
                            //2023-06-20: Xử lý nhập kiểu cách
                            var paragraphStyle = CreateObject<ParagraphStyle>();
                            if (bookMark != null)
                                paragraphStyle.Link = bookMark;
                            //paragraphStyle.Name = nodeList[i].InnerText;

                            System.Xml.XmlNode characterNodeStyle = nodeList[i].ParentNode.FirstChild.Name.Equals(prefix + ":rPr") ? nodeList[i].ParentNode.FirstChild : null;
                            System.Xml.XmlNode paragraphNodeStyle = (wpNode != null && wpNode.FirstChild.Name.Equals(prefix + ":pPr")) ? wpNode.FirstChild : null;
                            paragraphNodeStyle = wpNode.FirstChild;

                            //if (characterNodeStyle is null && paragraphNodeStyle is null)
                            //    continue;
                            //Nạp style của paragraph 
                            bool hasValue = false;
                            if (paragraphNodeStyle != null)
                            {
                                if (nodeList[i].InnerText.Contains("Order data with drawing"))
                                {

                                }
                                if (FillParagraphStyleNode(video, paragraphNodeStyle, paragraphStyle, null, bookMark, false, prefix))
                                    hasValue = true;
                            }
                            //Nạp style của character 
                            if (characterNodeStyle != null)
                            {
                                if (FillCharStyleNode(video, characterNodeStyle, paragraphStyle, null, bookMark, false, prefix))
                                    hasValue = true;
                            }
                            if (video.RightIndent && paragraphStyle.IndentRight != null)
                            {
                                //2024-08-29
                                //Khi có option Thụt phải / RightIndent trong phần Nạp
                                //Sẽ lưu giá trị Thụt phải của mỗi Thành phần vào trường Số lượng(Trường này đang là Int sẽ cần chuyển sang Decimal)
                                audio.Quantity = paragraphStyle.IndentRight;
                            }
                            if (dictionarySlides.ContainsKey(xmlSlide) &&
                                //dictionarySlideLayouts.ContainsKey(dictionarySlides[xmlSlide]) &&
                                dictionarySlideLayoutsWithLayouts.ContainsKey(dictionarySlides[xmlSlide]))
                            {
                                //Nạp Upper style nếu có
                                var spNode = GetParentNode(nodeList[i], "p:sp");
                                //if (spNode != null && spNode.FirstChild != null && spNode.FirstChild.Name == "p:nvSpPr" &&
                                //      spNode.FirstChild.FirstChild != null && spNode.FirstChild.FirstChild.Name == "p:cNvPr")
                                if (spNode != null)
                                {
                                    //var currentStyleLayoutName = GetAttributeInNode(spNode.FirstChild.FirstChild, "name");
                                    var currentStyleLayoutName = GetPowerPointStyleName(spNode, dictionarySlides[xmlSlide], xmlSlide);
                                    if (!string.IsNullOrEmpty(currentStyleLayoutName))
                                    {
                                        ParagraphStyle existLayout = null;
                                        if (currentStyleLayoutName.StartsWith("SL"))
                                        {
                                            if (dictionaryLayoutInSlide.ContainsKey(currentStyleLayoutName))
                                                existLayout = dictionaryLayoutInSlide[currentStyleLayoutName];
                                        }
                                        else
                                        {
                                            if (dictionarySlideLayoutsWithLayouts[dictionarySlides[xmlSlide]].ContainsKey(currentStyleLayoutName))
                                                existLayout = dictionarySlideLayoutsWithLayouts[dictionarySlides[xmlSlide]][currentStyleLayoutName];
                                        }
                                        if (existLayout != null)
                                        {
                                            if (hasValue)
                                                paragraphStyle.UpperStyle = existLayout;
                                            else
                                            {
                                                paragraphStyle.Delete();
                                                paragraphStyleListAdd.Remove(paragraphStyle);
                                                audio.ParagraphStyle = existLayout;
                                            }
                                        }
                                        else
                                        {
                                            var newUpperParagraphStyle = hasValue ? CreateObject<ParagraphStyle>() : paragraphStyle;
                                            if (bookMark != null && newUpperParagraphStyle.Link is null)
                                                newUpperParagraphStyle.Link = bookMark;
                                            newUpperParagraphStyle.Name = currentStyleLayoutName;
                                            //newUpperParagraphStyle.UpperStyle = dictionarySlideLayouts[dictionarySlides[xmlSlide]];

                                            if (currentStyleLayoutName.StartsWith("SL"))
                                            {
                                                //Nạp style trên Slide                                                
                                                if (FillParagraphStyleNodeInPowerPoint(video, spNode, newUpperParagraphStyle, bookMark))
                                                {
                                                    //Nếu có dữ liệu thì mới add vào
                                                    paragraphStyleListAdd.Add(newUpperParagraphStyle);
                                                    audio.ParagraphStyle = newUpperParagraphStyle;
                                                    dictionaryLayoutInSlide.Add(currentStyleLayoutName, newUpperParagraphStyle);
                                                    if (hasValue)
                                                        paragraphStyle.UpperStyle = newUpperParagraphStyle;
                                                    //Nêu là SL thì không nạp masterStyle theo level 
                                                }
                                                else
                                                {
                                                    newUpperParagraphStyle.Delete();
                                                }
                                            }
                                            else
                                            {
                                                //Nếu là style trên Layout thì bổ sung
                                                paragraphStyleListAdd.Add(newUpperParagraphStyle);
                                                audio.ParagraphStyle = newUpperParagraphStyle;
                                                dictionarySlideLayoutsWithLayouts[dictionarySlides[xmlSlide]].Add(currentStyleLayoutName, newUpperParagraphStyle);
                                                if (hasValue)
                                                    paragraphStyle.UpperStyle = newUpperParagraphStyle;
                                                //Nạp masterStyle theo level
                                                bool isbodyStyle = currentStyleLayoutName.Contains(".body");
                                                if (!isbodyStyle)
                                                {
                                                    //Trường hợp không có type
                                                    int idx = 0;
                                                    isbodyStyle = System.Int32.TryParse(currentStyleLayoutName.Substring(currentStyleLayoutName.LastIndexOf('.') + 1), out idx);
                                                }
                                                foreach (var masterParagraphStyle in slideMasterLayout[slideLayoutSlideMasterDictionary[dictionarySlides[xmlSlide]]])
                                                {
                                                    if ((currentStyleLayoutName.EndsWith(".title") && masterParagraphStyle.Name.EndsWith(".title")) ||
                                                        (isbodyStyle && masterParagraphStyle.Name.EndsWith(".body")))
                                                    {
                                                        newUpperParagraphStyle.UpperStyle = masterParagraphStyle;
                                                        if (!paragraphStyleUsingList.Contains(masterParagraphStyle))
                                                            paragraphStyleUsingList.Add(masterParagraphStyle);
                                                    }
                                                }

                                            }

                                            //newUpperParagraphStyle.Video = video;

                                        }
                                        //Nạp SlideLayout bao gồm:  Tên Layout là Layout1, Layout2 bỏ Slide
                                        //-TitleLayout
                                        //- Level1, Level2, ... (Level 0) không có tìm hiểu sau
                                        //Nhập Level nếu có                                                
                                        if (wpNode.FirstChild != null && wpNode.FirstChild.Name == "a:pPr")
                                        {
                                            var lvlValue = GetAttributeInNode(wpNode.FirstChild, "lvl");
                                            if (!string.IsNullOrEmpty(lvlValue))
                                            {
                                                string name = currentStyleLayoutName + ".LV" + lvlValue;
                                                ParagraphStyle existLevelParagraphStyle = null;
                                                //foreach (var existParagraphStyle in ParagraphStyleList)
                                                //{
                                                //    if (existParagraphStyle.Name == name && existParagraphStyle.UpperStyle != null &&
                                                //        existParagraphStyle.UpperStyle.Oid == paragraphStyle.UpperStyle.Oid
                                                //        )
                                                //    {
                                                //        existLevelParagraphStyle = existParagraphStyle;
                                                //    }
                                                //}
                                                foreach (var existParagraphStyle in paragraphStyleListAdd)
                                                {
                                                    if (existParagraphStyle.Name == name)
                                                    {
                                                        existLevelParagraphStyle = existParagraphStyle;
                                                        break;
                                                    }
                                                }
                                                if (existLevelParagraphStyle != null)
                                                {
                                                    if (hasValue)
                                                        paragraphStyle.UpperStyle = existLevelParagraphStyle;
                                                    else
                                                        audio.ParagraphStyle = existLevelParagraphStyle;
                                                }
                                                else
                                                {
                                                    //Tạo Upper stype mới                                                    
                                                    var newUpperParagraphStyle = CreateObject<ParagraphStyle>();
                                                    if (bookMark != null)
                                                        newUpperParagraphStyle.Link = bookMark;
                                                    newUpperParagraphStyle.Name = name;
                                                    newUpperParagraphStyle.Outline = Convert.ToInt32(lvlValue);
                                                    if (hasValue)
                                                        newUpperParagraphStyle.UpperStyle = paragraphStyle.UpperStyle;
                                                    else
                                                        newUpperParagraphStyle.UpperStyle = paragraphStyle;
                                                    if (currentStyleLayoutName.StartsWith("SL"))
                                                    {
                                                        FillParagraphStyleNodeInPowerPoint(video, spNode, newUpperParagraphStyle, bookMark);
                                                    }
                                                    else
                                                    {
                                                        dictionarySlideLayoutsWithLayouts[dictionarySlides[xmlSlide]].Add(name, newUpperParagraphStyle);
                                                        //Nạp style cấp trên
                                                        foreach (var masterParagraphStyle in slideMasterLayout[slideLayoutSlideMasterDictionary[dictionarySlides[xmlSlide]]])
                                                        {
                                                            if (masterParagraphStyle.Name.EndsWith(".LV" + lvlValue))
                                                            {
                                                                newUpperParagraphStyle.UpperStyle = masterParagraphStyle;
                                                                if (!paragraphStyleUsingList.Contains(masterParagraphStyle))
                                                                    paragraphStyleUsingList.Add(masterParagraphStyle);
                                                            }
                                                        }
                                                    }
                                                    if (hasValue)
                                                        paragraphStyle.UpperStyle = newUpperParagraphStyle;
                                                    else
                                                        audio.ParagraphStyle = newUpperParagraphStyle;
                                                    //newUpperParagraphStyle.Video = video;
                                                    paragraphStyleListAdd.Add(newUpperParagraphStyle);
                                                }
                                            }
                                        }
                                    }


                                }
                            }


                            if (hasValue && string.IsNullOrEmpty(paragraphStyle.Name))
                            {
                                ParagraphStyle existStyle = null;
                                //Kiểm tra xem có stype này chưa
                                foreach (var style in paragraphStyleEmptyNameList)
                                {
                                    //2023-09-07 Bỏ kiểm tra theo tên, vì có trường upper style
                                    if (!string.IsNullOrEmpty(style.Name) || style.Link != bookMark)
                                        continue;

                                    if (style.Font == paragraphStyle.Font && style.Size == paragraphStyle.Size
                                        //&& style.UpperStyle == paragraphStyle.UpperStyle
                                        && style.Color == paragraphStyle.Color && style.Bold == paragraphStyle.Bold
                                        && style.Italic == paragraphStyle.Italic && style.Underline == paragraphStyle.Underline)
                                    {
                                        //2023-06-28: Bổ sung thêm style cho paragraph
                                        //if (style.Spacing == paragraphStyle.Spacing
                                        //    && style.Indentation == paragraphStyle.Indentation
                                        //    && style.Alignment == paragraphStyle.Alignment)
                                        //2023-06-29:
                                        //- Xác định Style chỉ căn cứ vào Font và BusinessObjects.Alignment, BusinessObjects.Alignment thì chuyển eNum: Trái / Giữa / Phải để dễ nhìn
                                        //-Indentation và Spacing sẽ tuân theo Style mới đầu tiên tìm thấy: có thể bỏ 2 trường này và âm thầm lưu giá trị vào Style khi xuất file thôi
                                        if (style.Alignment == paragraphStyle.Alignment && style.Outline == paragraphStyle.Outline)
                                        {
                                            //2024-08-29
                                            if (!video.RightIndent || style.IndentRight == paragraphStyle.IndentRight)
                                            {
                                                existStyle = style;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (existStyle != null)
                                {

                                    paragraphStyle.Delete();
                                    audio.ParagraphStyle = existStyle;
                                }
                                else
                                {
                                    //paragraphStyle.Content = i.ToString();
                                    //paragraphStyle.Content = paragraphNodeStyle.InnerXml.Length < 1000 ? paragraphNodeStyle.InnerXml : paragraphNodeStyle.InnerXml.Substring(0, 1000);
                                    //paragraphStyle.Video = video;
                                    paragraphStyleListAdd.Add(paragraphStyle);
                                    paragraphStyleEmptyNameList.Add(paragraphStyle);
                                    //paragraphList.Add(paragraphStyle);
                                    //ParagraphStyleList.Add(paragraphStyle);                                            
                                    audio.ParagraphStyle = paragraphStyle;
                                }

                            }



                            //2023-08-29
                            //Đánh dấu trường NotAdjacent khi
                            //-Paragraph chứa tag Column ở cuối
                            //- Paragraph sau nó chứa tag Column ở đầu
                            //- Sau nó là các Pargraph trống rồi mới tới Thành phần tiếp
                            //Trường hợp thẻ trống phía sau là wr có tab
                            if (nodeList[i].ParentNode.InnerText.Contains("1: signed byte"))
                            {

                            }
                            if (!audio.NotAdjacent && nodeList[i].ParentNode.NextSibling != null && nodeList[i].ParentNode.NextSibling.InnerText == "")
                            {
                                foreach (System.Xml.XmlNode tabNode in nodeList[i].ParentNode.NextSibling.ChildNodes)
                                {
                                    //-Paragraph chứa tag Column ở cuối
                                    if (tabNode.Name == prefix + ":tab")
                                    {
                                        audio.NotAdjacent = true;
                                        break;
                                    }
                                }
                            }
                            if (!audio.NotAdjacent && wpNode != null && wpNode.NextSibling != null && wpNode.NextSibling.Name == prefix + ":p")
                            {
                                if (string.IsNullOrEmpty(wpNode.NextSibling.InnerText))
                                {
                                    //Sau nó là các Pargraph trống rồi mới tới Thành phần tiếp
                                    audio.NotAdjacent = true;
                                }
                                //else if(wpNode.NextSibling.FirstChild != null && wpNode.NextSibling.FirstChild.Name == prefix + ":pPr")
                                //{
                                //    foreach (System.Xml.XmlNode tabsNode in wpNode.NextSibling.FirstChild.ChildNodes)
                                //    {
                                //        if (tabsNode.Name == prefix + ":tabs")
                                //        {
                                //            audio.NotAdjacent = true;
                                //            break;
                                //        }
                                //    }
                                //}
                                else if (wpNode.NextSibling.InnerXml.Contains(prefix + ":br w:type=\"column\""))
                                {
                                    //Paragraph sau nó chứa tag Column ở đầu
                                    audio.NotAdjacent = true;
                                    //Kiểm tra xem node tiếp theo có chứa <w:br w:type="column"/>

                                }
                            }
                        }
                        else
                        {
                        }

                        if (video.ImportByNode || video.UpperElementImport && nodeList.Count > 1)
                        {
                            //2023-08-24: Khi nhập theo nốt, TextNode sẽ trỏ vào Paragraph, sẽ nhập Paragraph như cũ,
                            //còn TextNode trỏ vào Paragraph mẹ đồng thời Nốt = TRUE, Style vẫn áp dụng cho cả TextNode và Paragraph
                            //Các khái niệm Gộp liền kề chỉ áp dụng cho cùng loại(Paragraph hoặc TextNode)
                            //và TextNode chỉ trong phạm vi cùng Paragraph, khi gộp 2 Paragraph thì các TextNode con trỏ về cùng mẹ
                            //2023-08-24: Dùng trường cấp trên để xác định TextNode
                            //audio.TextNode = true;
                            if (wpNode != null && upperElements.ContainsKey(wpNode))
                            {
                                audio.UpperElement = upperElements[wpNode];
                                //Nếu nội dụng cũng style ngắn hơn style hiện tại thì style là style của dài hơn
                                if (video.UpperElementImport && nodeList.Count > 1 && audio.ParagraphStyle != null && !string.IsNullOrEmpty(audio.Content))
                                {
                                    if (audio.UpperElement.ParagraphStyle is null)
                                        audio.UpperElement.ParagraphStyle = audio.ParagraphStyle;
                                    else if (!audio.UpperElement.ParagraphStyle.Oid.Equals(audio.ParagraphStyle.Oid) &&
                                        upperElementContentForStyle.ContainsKey(audio.UpperElement.Oid) &&
                                        !string.IsNullOrEmpty(upperElementContentForStyle[audio.UpperElement.Oid]) &&
                                        upperElementContentForStyle[audio.UpperElement.Oid].Length < audio.Content.Length)
                                    {
                                        audio.UpperElement.ParagraphStyle = audio.ParagraphStyle;
                                        upperElementContentForStyle[audio.UpperElement.Oid] = audio.Content;
                                    }
                                }
                                if (video.UpperElementImport && !upperElementsMultiChildList.Contains(audio.UpperElement.Oid))
                                    upperElementsMultiChildList.Add(audio.UpperElement.Oid);
                            }
                            else
                            {
                                //Tao UpperElement
                                var upperElement = new Audio(audio.Session);
                                upperElement.BookMark = audio.BookMark;
                                upperElement.TranslateObject = audio.TranslateObject;
                                //upperElement.Video = video;
                                //AudioList.Add(upperElement);
                                audioList.Add(upperElement);

                                string pContent = wpNode.InnerText.Replace(" ", " ");
                                if (!video.KeepSpace)
                                    pContent = pContent.Trim();
                                upperElement.Content = pContent;
                                if (bookMark != null)
                                    upperElement.BookMark = bookMark;
                                //audio.Order = index;
                                upperElement.Start = TimeSpan.FromSeconds(index);

                                audio.UpperElement = upperElement;
                                upperElements.Add(wpNode, upperElement);

                                upperElement.ParagraphStyle = audio.ParagraphStyle;
                                if (video.UpperElementImport && nodeList.Count > 1)
                                    upperElementContentForStyle.Add(upperElement.Oid, audio.Content);
                            }
                            if (!audio.NotAdjacent && audio.UpperElement != null && !upperElementsAdjacentList.Contains(audio.UpperElement.Oid))
                                upperElementsAdjacentList.Add(audio.UpperElement.Oid);
                        }

                        index++;
                    }

                }
                countNumber++;
                var percent = countNumber / dictionarySlides.Keys.Count;
                if (percent < 100)
                    ShowWaitForm(percent.ToString("p0"), waitCaption, stopWatch.Elapsed);
            }
            ShowWaitForm("Đang nạp, Vui lòng đợi!", waitCaption);

            //Nạp style trong Layout
            string slideLayoutFolder = tempFolder + "\\ppt\\slideLayouts\\";
            foreach (var slideLayoutName in slideLayoutNames)
            {
                var slideLayoutFile = slideLayoutFolder + slideLayoutName;
                var slideLayoutsWithLayouts = dictionarySlideLayoutsWithLayouts[slideLayoutName];

                //layoutParagraphStyle.Font = fontName;
                //Nạp style cho Layout
                themeDoc.Load(slideLayoutFile);
                var spNodes = themeDoc.GetElementsByTagName("p:sp");
                foreach (System.Xml.XmlNode spNode in spNodes)
                {
                    var styleName = GetPowerPointStyleName(spNode, slideLayoutName);
                    if (!string.IsNullOrEmpty(styleName) && slideLayoutsWithLayouts.ContainsKey(styleName))
                    {
                        foreach (var key in slideLayoutsWithLayouts.Keys)
                        {
                            if (key == styleName || (key.StartsWith(styleName) && key.Contains(".LV")))
                            {
                                //Nạp cấp trên cho style                                
                                if (FillParagraphStyleNodeInPowerPoint(video, spNode, slideLayoutsWithLayouts[key], bookMark))
                                {
                                    //Nếu style này không sử dụng thì có thể đổi chỗ cấp trên
                                }
                            }
                        }

                    }
                }

            }
            //Loại bỏ các style không sử dụng

            foreach (var slideMasterName in slideMasterLayout.Keys)
            {
                for (int j = slideMasterLayout[slideMasterName].Count - 1; j >= 0; j--)
                {
                    if (!paragraphStyleUsingList.Contains(slideMasterLayout[slideMasterName][j]))
                    {
                        paragraphStyleListAdd.Remove(slideMasterLayout[slideMasterName][j]);
                        slideMasterLayout[slideMasterName][j].Delete();
                        slideMasterLayout[slideMasterName].Remove(slideMasterLayout[slideMasterName][j]);
                    }
                }
            }
            if (video.UpperElementImport)
            {
                //Bỏ những đối tượng audio có 1 dòng
                if (upperElementsMultiChildList.Count > 0)
                {
                    for (int j = audioList.Count - 1; j >= 0; j--)
                    {
                        if (audioList[j].UpperElement != null && !upperElementsMultiChildList.Contains(audioList[j].UpperElement.Oid))
                        {
                            audioList[j].Delete();
                            audioList.Remove(audioList[j]);
                        }
                    }
                }
                //Bỏ những đối tượng audio có toàn bộ là kề sau
                if (upperElementsAdjacentList.Count > 0)
                {
                    for (int j = audioList.Count - 1; j >= 0; j--)
                    {
                        if (upperElementsMultiChildList.Contains(audioList[j].Oid) && !upperElementsAdjacentList.Contains(audioList[j].Oid))
                        {
                            audioList[j].Delete();
                            audioList.Remove(audioList[j]);
                            //audioList[j].Note = "Xóa";
                        }
                        else if (audioList[j].UpperElement != null && upperElementsMultiChildList.Contains(audioList[j].UpperElement.Oid) && !upperElementsAdjacentList.Contains(audioList[j].UpperElement.Oid))
                        {
                            audioList[j].UpperElement = null;
                            //audioList[j].Note = "Xóa upper";
                        }
                    }
                }
            }
            video.AudioList.AddRange(audioList);
            //Add kiểu cách hàng loạt sẽ nhanh hơn nạp từng cái
            video.ParagraphStyleList.AddRange(paragraphStyleListAdd);


            var paragraphStyleList = video.ParagraphStyleList.Where(m => string.IsNullOrEmpty(m.Name)).OrderBy(m => m.Size).ToList();
            //2023-06-22: Tên Style để là S01, S02 và tăng dần, có tính năng sửa tên sau khi sort theo độ lớn font để thứ tự từ 01 > 99
            for (int i = 0; i < paragraphStyleList.Count; i++)
            {
                var newIndex = styleIndex + i;
                string styleName = (newIndex + 1).ToString();
                if (newIndex < 9)
                    styleName = "00" + styleName;
                else if (newIndex < 99)
                    styleName = "0" + styleName;
                //else if (newIndex >= 999)
                //    styleName += "(Không hỗ trợ)";
                //2024-08-26: Tên kiểu cách là duy nhất và đặt theo công thức: xxyyy trong đó xx là số thứ tự của tài liệu quy về 2 chữ số, yyy là số tăng dần của Kiểu cách trong 1 tài liệu
                if (bookMark != null)
                {
                    styleName = bookMark.GetOrderCode() + styleName;
                }
                paragraphStyleList[i].Name += styleName;
            }
            ShowWaitForm(null, null);
        }

        private bool FillParagraphStyleNodeInPowerPoint(Video video, System.Xml.XmlNode spNode, ParagraphStyle paragraphStyle, BookMark bookMark)
        {
            bool hasValue = false;
            foreach (System.Xml.XmlNode txBodyNode in spNode.ChildNodes)
            {
                if (txBodyNode.Name == "p:txBody")
                {
                    foreach (System.Xml.XmlNode lstStyleNode in txBodyNode.ChildNodes)
                    {
                        if (lstStyleNode.Name == "a:lstStyle")
                        {
                            int level = 1;
                            if (!string.IsNullOrEmpty(paragraphStyle.Name))
                            {
                                var lvIndex = paragraphStyle.Name.IndexOf(".LV");
                                if (lvIndex > 0)
                                {
                                    level = Convert.ToInt32(paragraphStyle.Name.Substring(lvIndex + 3)) + 1;
                                }
                            }
                            foreach (System.Xml.XmlNode lvlpPrNode in lstStyleNode.ChildNodes)
                            {
                                if (lvlpPrNode.Name == "a:lvl" + level + "pPr")
                                {
                                    hasValue = FillParagraphStyleInLvlpPrNode(video, lvlpPrNode, paragraphStyle, bookMark);
                                }
                            }
                        }
                    }
                }
            }
            return hasValue;
        }
        private bool FillParagraphStyleInLvlpPrNode(Video video, System.Xml.XmlNode lvlpPrNode, ParagraphStyle paragraphStyle, BookMark bookMark)
        {
            bool hasValue = FillParagraphStyleNode(video, lvlpPrNode, paragraphStyle, null, bookMark, false, 'a');
            foreach (System.Xml.XmlNode defRPrNode in lvlpPrNode.ChildNodes)
            {
                if (defRPrNode.Name == "a:defRPr")
                {
                    if (FillCharStyleNode(video, defRPrNode, paragraphStyle, null, bookMark, false, 'a'))
                        hasValue = true;
                }
                else if (defRPrNode.Name == "a:buFont")
                {
                    var font = GetAttributeInNode(defRPrNode, "typeface");
                    if (!string.IsNullOrEmpty(font))
                    {
                        paragraphStyle.Font = font;
                        hasValue = true;
                    }
                }
            }
            return hasValue;
        }
        private string GetPowerPointStyleName(System.Xml.XmlNode spNode, string layoutName, string styleName = null)
        {
            foreach (System.Xml.XmlNode nvSpPrNode in spNode.ChildNodes)
            {
                if (nvSpPrNode.Name == "p:nvSpPr")
                {
                    if (!string.IsNullOrEmpty(layoutName))
                    {
                        foreach (System.Xml.XmlNode nvPrNode in nvSpPrNode.ChildNodes)
                        {
                            if (nvPrNode.Name == "p:nvPr")
                            {

                                foreach (System.Xml.XmlNode phNode in nvPrNode.ChildNodes)
                                {
                                    if (phNode.Name == "p:ph" && phNode.Attributes != null && phNode.Attributes.Count > 0)
                                    {
                                        string type = "";
                                        string sz = "";
                                        string idx = "";
                                        foreach (System.Xml.XmlAttribute att in phNode.Attributes)
                                        {
                                            if (att.Name == "type")
                                                type = att.Value;
                                            //Bỏ thuộc tính sz trong tên
                                            //else if (att.Name == "sz")
                                            //    sz = att.Value;
                                            else if (att.Name == "idx")
                                                idx = att.Value;
                                        }
                                        return "LO" + layoutName.Replace("slideLayout", "").Replace(".xml", "") + "." + type + sz + idx;
                                    }
                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(styleName))
                    {
                        foreach (System.Xml.XmlNode cNvPrNode in nvSpPrNode.ChildNodes)
                        {
                            if (cNvPrNode.Name == "p:cNvPr")
                            {
                                //Trả về Id tên slile và Id của Node
                                foreach (System.Xml.XmlAttribute att in cNvPrNode.Attributes)
                                {
                                    if (att.Name == "id")
                                    {
                                        return "SL" + styleName.Replace("slide", "").Replace(".xml", "") + ".ID" + att.Value;
                                    }
                                }
                            }

                        }
                    }

                }
            }
            return null;
        }

        public bool WtContentIsValidate(Video video, System.Xml.XmlNode wtNode)
        {
            //Kiểm tra xem có ký tự không            
            if (!NodeContentIsValidate(video, wtNode.InnerText))
                return false;
            //Fix Trường hợp text nằm trong Textbox bị trùng
            //var textboxNode = GetParentNode(wtNode, "v:txbx");
            //var textboxNode = GetParentNode(wtNode, "wps:txbx");
            return !IsTextBox(wtNode);
            //return true;
        }

        public bool IsTextBox(System.Xml.XmlNode wtNode)
        {
            //Là textbox thì không nạp, còn là txbx thì nạp
            var textBoxNode = GetParentNode(wtNode, ":textbox");
            if (textBoxNode is null)
            {
                //Nếu là trong group cũng không nạp
                return InGroup(wtNode);
            }
            return textBoxNode != null;
        }

        public bool InGroup(System.Xml.XmlNode wtNode)
        {
            //Là textbox thì không nạp, còn là txbx thì nạp
            //2024-10-01: Nếu là trong group thì không nạp dữ liệu vào
            var groupNode = GetParentNode(wtNode, ":wgp");
            if (groupNode is null)
            {
                groupNode = GetParentNode(wtNode, ":group");
                if (groupNode != null)
                {
                    //2024-10-08: Nếu là group không chứa ảnh thì vẫn nạp
                    foreach (System.Xml.XmlNode childNode in groupNode.ChildNodes)
                    {
                        if (childNode.Name == "v:shape")
                            foreach (System.Xml.XmlNode imagedataNode in childNode.ChildNodes)
                            {
                                if (childNode.Name == "v:imagedata")
                                    return true;
                            }
                    }
                    return false;
                }
            }
            else
            {
                //2024-10-08: Nếu là group không chứa ảnh thì vẫn nạp
                foreach (System.Xml.XmlNode childNode in groupNode.ChildNodes)
                {
                    if (childNode.Name == ":pic")
                        return true;
                }
                return false;
            }
            return groupNode != null;
        }

        public bool NodeContentIsValidate(Video video, string content)
        {
            //Kiểm tra xem có ký tự không            
            if (string.IsNullOrEmpty(content))
                return false;
            bool hasChar = false;
            foreach (var c in content)
            {
                //Nạp số/Number : Sẽ nạp cả các số đơn lẻ
                if (video.Number && char.IsNumber(c))
                    return true;
                else if (char.IsLetter(c))
                {
                    hasChar = true;
                    break;
                }
            }
            //2024-9-11: Option Number sẽ nạp cả các ký tự đơn
            if (!video.Number && content.Trim().Length < 2)
                return false;
            if (!hasChar)
                return false;
            return true;
        }


        public ParagraphStyle GetDefaultUpperStyle(Video video, string styleName, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, BookMark bookMark = null)
        {
            if (!string.IsNullOrEmpty(styleName))
            {
                //Kiểm tra xem đã có style này chưa
                if (listExistedParagraphStyle != null)
                {
                    foreach (var paragraphStyle in listExistedParagraphStyle)
                    {
                        if (styleName == paragraphStyle.Name)
                            return paragraphStyle;
                    }
                }
                else
                {
                    foreach (var paragraphStyle in video.ParagraphStyleList)
                    {
                        if (styleName == paragraphStyle.Name)
                            return paragraphStyle;
                    }
                }

                //Nếu chưa có thì tạo mới
                var newParagraphStyle = CreateObject<ParagraphStyle>();
                if (bookMark != null)
                    newParagraphStyle.Link = bookMark;
                newParagraphStyle.Video = video;
                newParagraphStyle.Name = styleName;
                if (listExistedParagraphStyle != null && !listExistedParagraphStyle.Contains(newParagraphStyle))
                    listExistedParagraphStyle.Add(newParagraphStyle);
                return newParagraphStyle;
            }
            return null;
        }

        public ParagraphStyle FindParagraphStyleByName(Video video, string styleName, BookMark bookMark = null)
        {
            if (!string.IsNullOrEmpty(styleName))
            {
                return video.ParagraphStyleList.FirstOrDefault(x => x.Name == styleName && x.Link == bookMark);
            }
            return null;
        }

        private bool FillParagraphStyleNode(Video video, System.Xml.XmlNode paragraphNodeStyle, ParagraphStyle paragraphStyle, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, BookMark bookMark, bool overrideName = false, char prefix = 'w', bool isParagraph = false)
        {
            bool hasValue = false;
            if (paragraphNodeStyle.Name == prefix + ":pPr" || (paragraphNodeStyle.Name.StartsWith(prefix + ":lvl") && paragraphNodeStyle.Name.EndsWith("pPr")))
            {
                if (prefix == 'a')
                {
                    //Power Point
                    foreach (System.Xml.XmlAttribute fontAttribute in paragraphNodeStyle.Attributes)
                    {
                        if (fontAttribute.Name == "algn")
                        {
                            if (video.Alignment)
                            {
                                if (fontAttribute.Value.Equals("1"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Left;
                                }
                                else if (fontAttribute.Value.Equals("r"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Right;
                                }
                                else if (fontAttribute.Value.Equals("ctr"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Centered;
                                    //Nếu trống mặc địch là căn giữa
                                }
                                else if (fontAttribute.Value.Equals("just"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Justified;
                                }
                            }
                            hasValue = true;
                        }
                        else if (fontAttribute.Name == "marL" || fontAttribute.Name == "indent")
                        {
                            if (video.Indent)
                            {
                                if (fontAttribute.Name == "marL")
                                {
                                    paragraphStyle.IndentLeft = Convert.ToDecimal(fontAttribute.Value) / 360000;
                                    hasValue = true;
                                }
                                else if (fontAttribute.Name == "indent")
                                {
                                    paragraphStyle.IndentFirstLine = Convert.ToDecimal(fontAttribute.Value) / 360000;
                                    hasValue = true;
                                }
                            }
                        }
                        else if (fontAttribute.Name == "lvl")
                        {
                            if (video.Outline)
                            {
                                paragraphStyle.Outline = Convert.ToInt32(fontAttribute.Value) + 1;
                                //Nếu là Power Point thì nằm trong cấp của Level
                                if (paragraphNodeStyle.Name == prefix + ":pPr")
                                    hasValue = true;

                            }
                        }

                    }
                    if (video.Spacing)
                    {
                        foreach (System.Xml.XmlNode styleNode in paragraphNodeStyle.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":lnSpc" || styleNode.Name == prefix + ":spcBef" || styleNode.Name == prefix + ":spcAft")
                            {
                                if (styleNode.FirstChild != null)
                                {
                                    decimal? result = null;
                                    foreach (System.Xml.XmlAttribute att in styleNode.FirstChild.Attributes)
                                    {
                                        if (att.Name == "val")
                                        {
                                            if (styleNode.FirstChild.Name == prefix + ":spcPct")
                                            {
                                                result = Convert.ToDecimal(att.Value) / 120000;
                                                break;
                                            }
                                            else if (styleNode.FirstChild.Name == prefix + ":spcPts")
                                            {
                                                result = Convert.ToDecimal(att.Value) / 100;
                                                break;
                                            }


                                        }
                                    }
                                    if (result != null)
                                    {
                                        if (styleNode.Name == prefix + ":lnSpc")
                                        {
                                            paragraphStyle.SpacingLineAt = result;
                                        }
                                        else if (styleNode.Name == prefix + ":spcBef")
                                        {
                                            paragraphStyle.SpacingBefore = result;

                                        }
                                        else if (styleNode.Name == prefix + ":spcAft")
                                        {
                                            paragraphStyle.SpacingAfter = result;
                                        }
                                        hasValue = true;
                                    }

                                }
                            }

                        }

                    }

                }
                else
                {
                    //Word
                    foreach (System.Xml.XmlNode styleNode in paragraphNodeStyle.ChildNodes)
                    {
                        if (styleNode.Name == prefix + ":pStyle")
                        {
                            //2023-09-07: Style là style tạo mới, style có sẵn thì nạp sau
                            //Nạp style có thừa kế
                            string styleName = GetAttributeInNode(styleNode);
                            if (!string.IsNullOrEmpty(paragraphStyle.Name))
                            {
                                //Trường hợp nạp style có sẵn
                            }
                            if (string.IsNullOrEmpty(paragraphStyle.Name) && paragraphStyle.UpperStyle is null)
                            {
                                paragraphStyle.UpperStyle = GetDefaultUpperStyle(video, styleName, listExistedParagraphStyle, bookMark);
                            }
                            //if (string.IsNullOrEmpty(paragraphStyle.Name) || overrideName)
                            //{                            
                            //    if (!string.IsNullOrEmpty(paparagraphStyleName))
                            //        paragraphStyle.Name = paparagraphStyleName;
                            //}                        
                        }
                        else if (styleNode.Name == prefix + ":rPr")
                        {
                            if (FillCharStyleNode(video, styleNode, paragraphStyle, listExistedParagraphStyle, bookMark, true, prefix, !isParagraph))
                                hasValue = true;
                        }
                        else if (styleNode.Name == prefix + ":spacing")
                        {
                            if (video.Spacing)
                            {
                                //paragraphStyle.Spacing = styleNode.OuterXml;
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == prefix + ":before")
                                    {
                                        paragraphStyle.SpacingBefore = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":after")
                                    {
                                        paragraphStyle.SpacingAfter = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":line")
                                    {
                                        paragraphStyle.SpacingLineAt = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":lineRule")
                                    {
                                        paragraphStyle.SpacingLine = att.Value;
                                        hasValue = true;
                                    }
                                }
                            }
                        }
                        else if (styleNode.Name == prefix + ":jc")
                        {
                            if (video.Alignment)
                            {
                                var nodeValue = this.GetAttributeInNode(styleNode);
                                if (nodeValue.Equals("left"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Left;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("right"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Right;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("center"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Centered;
                                    hasValue = true;
                                }
                                else if (nodeValue.Equals("both"))
                                {
                                    paragraphStyle.Alignment = BusinessObjects.Alignment.Justified;
                                    hasValue = true;
                                }
                            }

                        }
                        else if (styleNode.Name == prefix + ":ind")
                        {
                            if (video.Indent || video.RightIndent)
                            {
                                //paragraphStyle.Indentation = styleNode.OuterXml;
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == prefix + ":left")
                                    {
                                        if (video.Indent)
                                        {
                                            paragraphStyle.IndentLeft = Convert.ToDecimal(att.Value) / 20;
                                            hasValue = true;
                                        }
                                    }
                                    else if (att.Name == prefix + ":right")
                                    {
                                        paragraphStyle.IndentRight = Convert.ToDecimal(att.Value) / 20;
                                        hasValue = true;
                                    }
                                    else if (att.Name == prefix + ":firstLine")
                                    {
                                        if (video.Indent)
                                        {
                                            paragraphStyle.IndentFirstLine = Convert.ToDecimal(att.Value) / 20;
                                            hasValue = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (styleNode.Name == prefix + ":outlineLvl")
                        {
                            if (video.Outline)
                            {
                                var nodeValue = this.GetAttributeInNode(styleNode);
                                if (!string.IsNullOrEmpty(nodeValue))
                                {
                                    paragraphStyle.Outline = Convert.ToInt32(nodeValue) + 1;
                                    hasValue = true;
                                }

                            }
                        }
                    }
                }

            }
            return hasValue;
        }

        public bool FillCharStyleNode(Video video, System.Xml.XmlNode fontNode, ParagraphStyle paragraphStyle, System.Collections.Generic.List<ParagraphStyle> listExistedParagraphStyle, BookMark bookMark, bool overrideName = false, char prefix = 'w', bool parent = false)
        {
            bool hasValue = false;
            if (fontNode.Name == prefix + ":rPr" || fontNode.Name == prefix + ":defRPr")
            {
                if (prefix == 'a')
                {
                    //Nap PowerPoint
                    foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                    {
                        if (styleNode.Name == prefix + ":cs" || styleNode.Name == prefix + ":latin")
                        {
                            if (string.IsNullOrEmpty(paragraphStyle.Font))
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == "typeface")
                                    {
                                        if (string.IsNullOrEmpty(att.Value))
                                            continue;
                                        if (att.Value.StartsWith("+m") && att.Value.Contains("-"))
                                            continue;
                                        //Word
                                        paragraphStyle.Font = att.Value;
                                        hasValue = true;
                                        break;
                                    }
                                }
                        }
                        else if (styleNode.Name == prefix + ":solidFill")
                        {
                            if (video.FontColor)
                            {
                                foreach (System.Xml.XmlNode childNode in styleNode.ChildNodes)
                                {
                                    if (childNode.Name == prefix + ":srgbClr")
                                    {
                                        foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                                        {
                                            if (att.Name == "val")
                                            {
                                                if (att.Value.Equals("auto"))
                                                {
                                                    //Màu automatic (màu đen) trong word
                                                }
                                                else
                                                {
                                                    paragraphStyle.Color = System.Drawing.ColorTranslator.FromHtml("#" + att.Value);
                                                    hasValue = true;
                                                }
                                                break;

                                            }
                                        }
                                    }
                                }

                            }

                        }

                        else if (styleNode.Name == prefix + ":rStyle")
                        {
                            string styleName = GetAttributeInNode(styleNode);
                            var upperStyle = GetDefaultUpperStyle(video, styleName, listExistedParagraphStyle, bookMark);
                            if (!string.IsNullOrEmpty(paragraphStyle.Name) && upperStyle != null)
                            {
                                //Debug
                            }
                            if (string.IsNullOrEmpty(paragraphStyle.Name) && upperStyle != null)
                            {
                                paragraphStyle.UpperStyle = upperStyle;
                            }
                            //2023-09-07: Nạp style vào trường upper nếu có
                            //if (string.IsNullOrEmpty(paragraphStyle.Name) || overrideName)
                            //{
                            //    string nodeValue = GetAttributeInNode(styleNode);
                            //    if (!string.IsNullOrEmpty(nodeValue))
                            //    {
                            //        if (nodeValue.EndsWith("Char"))
                            //        {
                            //            //var wpNode = GetParentNode(fontNode);
                            //            //if(wpNode != null && wpNode.FirstChild.Name == prefix + ":pPr" && wpNode.FirstChild.FirstChild != null && wpNode.FirstChild.FirstChild.Name == prefix + ":pStyle")
                            //            //{
                            //            //    var wpId = GetAttributeInNode(wpNode.FirstChild.FirstChild);
                            //            //    if(!string.IsNullOrEmpty(wpId))
                            //            //        paragraphStyle.Name = wpId;
                            //            //}
                            //            paragraphStyle.Name = nodeValue.Substring(0, nodeValue.Length - 4);
                            //        }
                            //        else
                            //        {
                            //            paragraphStyle.Name = nodeValue;
                            //        }
                            //    }
                            //}                        
                        }
                    }
                    foreach (System.Xml.XmlAttribute fontAttribute in fontNode.Attributes)
                    {
                        if (fontAttribute.Name == "sz")
                        {
                            paragraphStyle.Size = Convert.ToDecimal(fontAttribute.Value) / 100;
                            hasValue = true;
                        }
                        else if (fontAttribute.Name == "b")
                        {
                            if (video.FontBold)
                            {
                                paragraphStyle.Bold = (fontAttribute.Value is null || fontAttribute.Value != "0");
                                hasValue = true;
                            }
                        }
                        else if (fontAttribute.Name == "i")
                        {
                            if (video.FontItalic)
                            {
                                paragraphStyle.Italic = (fontAttribute.Value is null || fontAttribute.Value != "0");
                                hasValue = true;
                            }
                        }
                        else if (fontAttribute.Name == "u")
                        {
                            if (video.FontUnderline)
                            {
                                paragraphStyle.Underline = (fontAttribute.Value is null || fontAttribute.Value != "sng");
                                hasValue = true;
                            }
                        }

                    }
                }
                else
                {
                    //Nạp Word
                    foreach (System.Xml.XmlNode styleNode in fontNode.ChildNodes)
                    {
                        if (styleNode.Name == prefix + ":rFonts")
                        {
                            foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                            {
                                if (att.Name == prefix + ":ascii" || att.Name == prefix + ":cstheme")
                                {
                                    //Word
                                    paragraphStyle.Font = att.Value;
                                    hasValue = true; ;
                                    break;
                                }
                                //else if (att.Name == prefix + ":eastAsia")
                                //{
                                //    paragraphStyle.Font = att.Value;
                                //    break;
                                //}
                            }
                        }
                        else if (styleNode.Name == prefix + ":color")
                        {
                            //2024-09-06: parent = true: nếu là trên paragraph thì không áp dụng
                            if (!parent && video.FontColor)
                            {
                                foreach (System.Xml.XmlAttribute att in styleNode.Attributes)
                                {
                                    if (att.Name == prefix + ":val")
                                    {
                                        if (att.Value.Equals("auto"))
                                        {
                                            //Màu automatic (màu đen) trong word
                                        }
                                        else
                                        {
                                            paragraphStyle.Color = System.Drawing.ColorTranslator.FromHtml("#" + att.Value);
                                            hasValue = true;
                                        }
                                        break;

                                    }
                                }
                            }

                        }
                        else if (styleNode.Name == prefix + ":sz")
                        {
                            //2024-09-06: parent = true: nếu là trên paragraph thì không áp dụng
                            if (!parent)
                            {
                                string nodeValue = GetAttributeInNode(styleNode);
                                if (!string.IsNullOrEmpty(nodeValue))
                                {
                                    paragraphStyle.Size = Convert.ToDecimal(nodeValue) / 2;
                                    hasValue = true;
                                }
                            }

                        }
                        else if (styleNode.Name == prefix + ":b")
                        {
                            //2024-09-06: parent = true: nếu là trên paragraph thì không áp dụng
                            if (!parent && video.FontBold)
                            {
                                var styleValue = GetAttributeInNode(styleNode);
                                paragraphStyle.Bold = (styleValue is null || styleValue != "0");
                                hasValue = true;
                            }
                        }
                        else if (styleNode.Name == prefix + ":i")
                        {
                            //2024-09-06: parent = true: nếu là trên paragraph thì không áp dụng
                            if (!parent && video.FontItalic)
                            {
                                var styleValue = GetAttributeInNode(styleNode);
                                paragraphStyle.Italic = (styleValue is null || styleValue != "0");
                                hasValue = true;
                            }
                        }
                        else if (styleNode.Name == prefix + ":u")
                        {
                            //2024-09-06: parent = true: nếu là trên paragraph thì không áp dụng
                            if (!parent && video.FontUnderline)
                            {
                                var styleValue = GetAttributeInNode(styleNode);
                                paragraphStyle.Underline = (styleValue is null || styleValue != "none");
                                hasValue = true;
                            }
                        }
                        else if (styleNode.Name == prefix + ":rStyle")
                        {
                            string styleName = GetAttributeInNode(styleNode);
                            var upperStyle = GetDefaultUpperStyle(video, styleName, listExistedParagraphStyle, bookMark);
                            if (!string.IsNullOrEmpty(paragraphStyle.Name) && upperStyle != null)
                            {
                                //Debug
                            }
                            if (string.IsNullOrEmpty(paragraphStyle.Name) && upperStyle != null)
                            {
                                paragraphStyle.UpperStyle = upperStyle;
                            }
                            //2023-09-07: Nạp style vào trường upper nếu có
                            //if (string.IsNullOrEmpty(paragraphStyle.Name) || overrideName)
                            //{
                            //    string nodeValue = GetAttributeInNode(styleNode);
                            //    if (!string.IsNullOrEmpty(nodeValue))
                            //    {
                            //        if (nodeValue.EndsWith("Char"))
                            //        {
                            //            //var wpNode = GetParentNode(fontNode);
                            //            //if(wpNode != null && wpNode.FirstChild.Name == prefix + ":pPr" && wpNode.FirstChild.FirstChild != null && wpNode.FirstChild.FirstChild.Name == prefix + ":pStyle")
                            //            //{
                            //            //    var wpId = GetAttributeInNode(wpNode.FirstChild.FirstChild);
                            //            //    if(!string.IsNullOrEmpty(wpId))
                            //            //        paragraphStyle.Name = wpId;
                            //            //}
                            //            paragraphStyle.Name = nodeValue.Substring(0, nodeValue.Length - 4);
                            //        }
                            //        else
                            //        {
                            //            paragraphStyle.Name = nodeValue;
                            //        }
                            //    }
                            //}                        
                        }
                    }
                }

            }
            return hasValue;
        }

        public string GetAttributeInNode(System.Xml.XmlNode node, string attributeName = "w:val")
        {
            if (node is null)
                return null;
            foreach (System.Xml.XmlAttribute att in node.Attributes)
            {
                if (att.Name == attributeName)
                {
                    return att.Value;
                }
            }
            return null;
        }


        private System.Xml.XmlNode GetFontNode(System.Xml.XmlNode node)
        {
            var parentNode = GetParentNode(node, "w:r");
            if (parentNode != null)
            {
                foreach (System.Xml.XmlNode childNode in parentNode.ChildNodes)
                {
                    if (childNode.Name == "w:rPr")
                        return childNode;
                }
            }
            parentNode = GetParentNode(node, "w:p");
            if (parentNode != null)
            {
                foreach (System.Xml.XmlNode childNode in parentNode.ChildNodes)
                {
                    if (childNode.Name == "w:pPr")
                    {
                        foreach (System.Xml.XmlNode n in childNode.ChildNodes)
                        {
                            if (n.Name == "w:rPr")
                            {
                                return n;
                            }
                            else if (n.Name == "w:pStyle")
                            {
                                return n;
                            }
                        }
                    }
                }
            }
            return null;
        }


        //Tối ưu tài liệu document

        private bool IsSimilarToBlack(string colorValue)
        {
            // Convert color value to RGB

            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#" + colorValue);
            int red = color.R;
            int green = color.G;
            int blue = color.B;

            // Calculate color difference
            int difference = System.Math.Abs(red - 0) + System.Math.Abs(green - 0) + System.Math.Abs(blue - 0);

            // Check if color is similar to black
            if (difference <= 30) // Adjust the threshold as needed
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void OptimalDocument(Video video, System.Xml.XmlDocument doc, System.Collections.Generic.IDictionary<System.Xml.XmlNode, bool> flagNodes = null, char prefix = 'w', System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<TermLocation>> abbyyTermLocationList = null)
        {
            //var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Video;
            //Xóa dấu xuống dòng thừa
            if (System.Diagnostics.Debugger.IsAttached)
                doc.Save("C:\\Temp\\Root1" + System.Guid.NewGuid() + ".xml");
            //Thay dấu tự động xuôgns dòng bàng dấu cách            
            var newLineNodes = doc.GetElementsByTagName(prefix + ":cr");
            for (int i = newLineNodes.Count - 1; i >= 0; i--)
            {
                var newNode = doc.CreateElement(prefix.ToString(), "t", newLineNodes[i].NamespaceURI);
                newNode.InnerText = " ";
                newLineNodes[i].ParentNode.ReplaceChild(newNode, newLineNodes[i]);
            }

            System.Collections.Generic.IList<System.Xml.XmlNode> deleteNodesList = new System.Collections.Generic.List<System.Xml.XmlNode>();
            var proofErrNodes = doc.GetElementsByTagName(prefix + ":proofErr");
            foreach (System.Xml.XmlNode node in proofErrNodes)
            {
                deleteNodesList.Add(node);
            }
            var bookmarkStartNodes = doc.GetElementsByTagName(prefix + ":bookmarkStart");
            foreach (System.Xml.XmlNode node in bookmarkStartNodes)
            {
                deleteNodesList.Add(node);
            }
            var bookmarkEndNodes = doc.GetElementsByTagName(prefix + ":bookmarkEnd");
            foreach (System.Xml.XmlNode node in bookmarkEndNodes)
            {
                deleteNodesList.Add(node);
            }
            //Xóa các màu gần giống màu đen
            //var colorNodes = doc.GetElementsByTagName(prefix + ":color");
            //foreach (System.Xml.XmlNode node in colorNodes)
            //{
            //    var wrNode = GetParentNode(node, prefix + ":r");
            //    if (wrNode is null)
            //        continue;
            //    var colorText = GetAttributeInNode(node);
            //    if (IsSimilarToBlack(colorText))
            //        deleteNodesList.Add(node);
            //}
            //067: Khi Option:Bỏ xuống dòng (BrLine) = True
            //Bỏ tất cả các tag xuống dòng<br> khi tối ưu file Word trong quá trình nạp thành phần
            if (video.BrLine)
            {
                var brLineNodes = doc.GetElementsByTagName(prefix + ":br");
                foreach (System.Xml.XmlNode node in brLineNodes)
                {
                    if (node.Attributes.Count == 0)
                    {

                        if (!((node.NextSibling != null && node.NextSibling.Name == prefix + ":t" &&
                            !string.IsNullOrEmpty(node.NextSibling.InnerText) && node.NextSibling.InnerText.StartsWith(' ')) ||
                            (node.PreviousSibling != null && node.PreviousSibling.Name == prefix + ":t" &&
                            !string.IsNullOrEmpty(node.PreviousSibling.InnerText) && node.PreviousSibling.InnerText.EndsWith(' '))))
                        {
                            //Tự động thêm dấu cách
                            if (node.NextSibling != null && node.NextSibling.Name == prefix + ":t" &&
                                !string.IsNullOrEmpty(node.NextSibling.InnerText))
                                node.NextSibling.InnerText = " " + node.NextSibling.InnerText;
                            else if (node.PreviousSibling != null && node.PreviousSibling.Name == prefix + ":t" &&
                                !string.IsNullOrEmpty(node.PreviousSibling.InnerText))
                                node.PreviousSibling.InnerText += " ";
                            //Nếu nhận dạng được br không liền dấu cách thì không xoá br, để tự ghép thành phần sau

                        }
                        deleteNodesList.Add(node);

                    }

                }
            }
            //var fldCharNodes = doc.GetElementsByTagName(prefix + ":fldChar");
            //foreach (System.Xml.XmlNode node in fldCharNodes)
            //{
            //    var deleteNode = GetParentNode(node, prefix + ":r");
            //    if (deleteNode != null)
            //    {
            //        deleteNodesList.Add(deleteNode);
            //    }
            //}
            for (int i = deleteNodesList.Count - 1; i >= 0; i--)
            {
                if (deleteNodesList[i] != null && deleteNodesList[i].ParentNode != null)
                {
                    deleteNodesList[i].ParentNode.RemoveChild(deleteNodesList[i]);
                }
            }
            //Nếu file doc chuyển từ pdf
            bool pdfToDoc = newLineNodes.Count > 0;
            if (System.Diagnostics.Debugger.IsAttached)
                doc.Save("C:\\Temp\\Root2" + System.Guid.NewGuid() + ".xml");
            var wtNodes = doc.GetElementsByTagName(prefix + ":t");
            if (wtNodes.Count == 0)
                return;
            //Xử lý ghép nội dung
            System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>> parentNodeList = new System.Collections.Generic.Dictionary<System.Xml.XmlNode, System.Collections.Generic.List<System.Xml.XmlNode>>();
            foreach (System.Xml.XmlNode node in wtNodes)
            {
                var parentNode = GetParentNode(node, prefix + ":p");
                if (parentNode != null)
                {
                    if (parentNodeList.ContainsKey(parentNode))
                    {
                        parentNodeList[parentNode].Add(node);
                    }
                    else
                    {
                        parentNodeList.Add(parentNode, new System.Collections.Generic.List<System.Xml.XmlNode> { node });
                    }
                    //Nạp thuật vị lỗi
                    if (abbyyTermLocationList != null && node.ParentNode != null && node.ParentNode.FirstChild != null
                            && node.ParentNode.FirstChild.Name == prefix + ":rPr" &&
                            node.ParentNode.FirstChild.InnerXml.Contains("<w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"80FFFF\"") &&
                            !string.IsNullOrWhiteSpace(node.InnerText)
                            && !IsTextBox(node)
                            //&& WtContentIsValidate(node) //Thuật vị lỗi thì không cần kiểm tra từ
                            )
                    {

                        string textNode = node.InnerText;
                        //string debugText = "tt";
                        //if (textNode == debugText) 
                        //{
                        //}
                        if (!textNode.StartsWith(' '))
                            GetWordByAbbyy(video, node, ref textNode, false);
                        if (!textNode.EndsWith(' '))
                            GetWordByAbbyy(video, node, ref textNode, true);
                        //Fix lỗi khi cắt dòng bị mất từ
                        //if (textNode.EndsWith('.'))
                        //    textNode = textNode.Substring(0, textNode.Length - 1);
                        //Xóa ký tự đặc biệt đầu vào cuối
                        textNode = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(textNode);
                        if (textNode.Contains("tửphospholip"))
                        {

                        }
                        if (NodeContentIsValidate(video, textNode))
                        {
                            foreach (var newLineChar in Module.Helpers.TextHelper.NewLineText)
                            {
                                if (textNode.EndsWith(newLineChar))
                                    textNode = textNode.Substring(0, textNode.Length - newLineChar.Length);
                            }
                            if (abbyyTermLocationList.ContainsKey(parentNode))
                            {
                                if (abbyyTermLocationList[parentNode].FirstOrDefault(m => m.MachineTranslate == textNode) is null)
                                    abbyyTermLocationList[parentNode].Add(new TermLocation(video.Session) { MachineTranslate = textNode });
                            }
                            else
                            {
                                abbyyTermLocationList.Add(parentNode, new System.Collections.Generic.List<TermLocation> { new TermLocation(video.Session) { MachineTranslate = textNode } });
                            }
                        }

                    }
                }
            }
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //var abbyyTermLocationListText = string.Join("\n", abbyyTermLocationList.Values.Select(x => x.Select(m => m.MachineTranslate)));
            }
            //Xử lý xóa hyperlink ở những câu có nhiều hyperlink
            //2023-06-21: Bỏ hyper link
            //if(Module.Helpers.ParameterHelper.GetBooleanOrDefault(ObjectSpace, "RemoveHyperlinkWhenOptimalWordDocument", true))
            if (!video.NodeLink)
            {
                foreach (var keyNode in parentNodeList.Keys)
                {
                    var nodeList = parentNodeList[keyNode];
                    if (nodeList.Count > 1)
                    {
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            var hyperlinkNode = GetParentNode(nodeList[i], prefix + ":hyperlink");
                            if (hyperlinkNode != null && hyperlinkNode.ParentNode != null)
                            {
                                //var wrNode = GetParentNode(nodeList[i], prefix + ":r");
                                for (int j = hyperlinkNode.ChildNodes.Count - 1; j >= 0; j--)
                                {
                                    hyperlinkNode.ParentNode.InsertAfter(hyperlinkNode.ChildNodes[j], hyperlinkNode);
                                }
                                //foreach (System.Xml.XmlNode childNode in hyperlinkNode.ChildNodes)
                                //{
                                //    hyperlinkNode.ParentNode.InsertBefore(childNode, hyperlinkNode);
                                //}
                                hyperlinkNode.ParentNode.RemoveChild(hyperlinkNode);
                            }
                        }

                    }
                }
            }

            //Ghép node
            foreach (var keyNode in parentNodeList.Keys)
            {
                var nodeList = parentNodeList[keyNode];
                if (nodeList.Count > 1)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        string debugText = "Phương pháp nghiên cứu và học";
                        if (keyNode.InnerText.Contains(debugText))
                        {

                        }
                    }
                    //Xử lý những chỗ node có từ đơn khi ghép, node từ đơn này bị xóa
                    //Tạo danh sách hyperlink cần xóa
                    //Dictionary<System.Xml.XmlNode, List<System.Xml.XmlNode>> deleteNodes = new Dictionary<System.Xml.XmlNode, List<System.Xml.XmlNode>>();                    
                    System.Collections.Generic.Dictionary<int, string> nodeContent = new System.Collections.Generic.Dictionary<int, string>();
                    int? joinStartIndex = null;
                    var addSpaceList = new System.Collections.Generic.List<int>();
                    //2023-06-29: Style của ghép là style có nội dung lớn nhất
                    int maxLengthText = 0;
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        nodeContent.Add(i, nodeList[i].InnerText);

                        //Kiểm tra nếu có tabNode  thì bỏ qua                        
                        if (joinStartIndex == null)
                        {
                            if (nodeList[i].NextSibling != null)
                            {
                                //Nếu node cuối là tabNode thì bỏ qua
                                if (nodeList[i].NextSibling.Name.Equals(prefix + ":tab"))
                                    continue;
                                //Nếu là xuống dòng thì bỏ qua
                                if (//AbbyyTermLocation && 
                                    nodeList[i].NextSibling.Name.Equals(prefix + ":br"))
                                    continue;
                                if (video.FootNote && nodeList[i].NextSibling.Name.Equals(prefix + ":footnoteReference"))
                                    continue;
                            }

                            //if (ExistedTabNode(nodeList, i))
                            //    continue;                           
                            joinStartIndex = i;
                            //2023-06-29: Style của ghép là style có nội dung lớn nhất             
                            if (flagNodes == null && !string.IsNullOrEmpty(nodeList[i].InnerText))
                                maxLengthText = nodeList[i].InnerText.Length;

                            if (video.BlankSpacing > 0 && !nodeList[i].InnerText.EndsWith(" "))
                            {
                                //2023-09-28: Khoảng trắng/BlankSpacing: Space nhỏ nhất để chèn kí tự trắng khi ghép 2 node lúc nạp                            
                                var startSpacingValue = GetSpacingValueInWord(nodeList[i]);
                                if (startSpacingValue != null && startSpacingValue >= video.BlankSpacing)
                                    addSpaceList.Add(i);
                            }
                            continue;
                        }
                        //2023-08-29                                                    
                        var wrParentNode = GetParentNode(nodeList[i], prefix + ":r");
                        if (wrParentNode is null)
                            continue;
                        bool brother = nodeList[i].PreviousSibling != null && nodeList[i].PreviousSibling == nodeList[joinStartIndex.Value];
                        if (!brother)
                        {
                            var joinParent = GetParentNode(nodeList[joinStartIndex.Value], prefix + ":r");
                            if (joinParent is null)
                                continue;
                            if (wrParentNode.PreviousSibling != null && wrParentNode.PreviousSibling != joinParent)
                            {
                                //Kiểm tra xem 2 đối tượng có liền nhau không
                                joinStartIndex = i;
                                continue;
                            }

                            //Nạp thành phần > nhận dạng Style theo Option
                            if (BreakJoinNode(video, nodeList, i, wrParentNode, joinParent))
                            {
                                joinStartIndex = i;
                                continue;
                            }
                            //Nếu là dấu cách cuối thì không cần ghép
                            //if(nodeList[i].InnerText == " " && i < nodeList.Count - 1)
                            //{
                            //    var nextParentNode = GetParentNode(nodeList[i + 1], prefix + ":r");                           
                            //    if (nextParentNode != null && BreakJoinNode(video, nodeList, i, nextParentNode, joinParent))
                            //    {
                            //        joinStartIndex = i + 1;
                            //        continue;
                            //    }
                            //}
                            //2023-06-24: Chỉ join khi font giống nhau:
                            //Kiểm tra và ghép theo font                           
                            if (joinParent.FirstChild != null && joinParent.FirstChild.Name.Equals(prefix + ":rPr") &&
                                    wrParentNode.FirstChild != null && wrParentNode.FirstChild.Name.Equals(prefix + ":rPr"))
                            {
                                if (joinParent.FirstChild.InnerXml != wrParentNode.FirstChild.InnerXml)
                                {
                                    if (GetValueOrDefault<bool>("EqualFontStyleWhenOptimalWordDocument", true))
                                    {
                                        //Khác style thì không ghép
                                        joinStartIndex = i;
                                        continue;
                                    }
                                    else
                                    {
                                        //2023-06-24: Nếu khác style mà vẫn ghép thì phải dựng cờ và bỏ style đậm, nghiêng, gạch
                                        if (flagNodes != null && !flagNodes.ContainsKey(nodeList[joinStartIndex.Value]))
                                            flagNodes.Add(nodeList[joinStartIndex.Value], true);

                                        //for(int n = joinParent.FirstChild.ChildNodes.Count - 1; n >= 0; n--)
                                        //{
                                        //    System.Xml.XmlNode node = joinParent.FirstChild.ChildNodes[n];
                                        //    if (node.Name == prefix + ":b" || node.Name == prefix + ":i" || node.Name == prefix + ":u")
                                        //    {
                                        //        joinParent.FirstChild.RemoveChild(node);
                                        //    }
                                        //}
                                        ////2023-06-29: Style của ghép là style có nội dung lớn nhất                                    
                                        if (!string.IsNullOrEmpty(nodeList[i].InnerText) && nodeList[i].InnerText.Length > maxLengthText)
                                        {
                                            maxLengthText = nodeList[i].InnerText.Length;
                                            joinParent.ReplaceChild(wrParentNode.FirstChild.Clone(), joinParent.FirstChild);
                                        }
                                    }

                                }
                                else
                                {

                                }
                            }

                        }
                        //2023-06-29: Style của ghép là style có nội dung lớn nhất
                        //Trường hợp flagNodes == null là Xuất dịch
                        if (flagNodes == null && !string.IsNullOrEmpty(nodeList[i].InnerText))
                            maxLengthText = nodeList[i].InnerText.Length;

                        //Thêm dấu khoảng cách giữa 2 từ
                        //2023-06-29: Thay xuống dòng tự động bằng dấu cách
                        //if (pdfToDoc && !nodeList[joinStartIndex.Value].InnerText.EndsWith(" ") && !nodeList[i].InnerText.StartsWith(" "))
                        //{
                        //    var previousText = nodeContent[i - 1];
                        //    if (!((nodeList[i].InnerText.Length == 1 && char.IsLetter(nodeList[i].InnerText[0])) ||
                        //         (previousText.Length == 1 && char.IsLetter(previousText[0]))))
                        //        nodeList[joinStartIndex.Value].InnerText += " ";
                        //}

                        if (video.BlankSpacing > 0)
                        {
                            //2023-09-28: Khoảng trắng/BlankSpacing: Space nhỏ nhất để chèn kí tự trắng khi ghép 2 node lúc nạp 
                            if (!nodeList[i].InnerText.EndsWith(" "))
                            {
                                var spacingValue = GetSpacingValueInWord(nodeList[i]);
                                if (spacingValue != null && spacingValue >= video.BlankSpacing)
                                    addSpaceList.Add(i);

                            }
                            if (!nodeList[joinStartIndex.Value].InnerText.EndsWith(" ") && !nodeList[i].InnerText.StartsWith(" ") &&
                                addSpaceList.Contains(i - 1))
                            {
                                nodeList[joinStartIndex.Value].InnerText += " ";
                            }
                        }
                        nodeList[joinStartIndex.Value].InnerText += nodeList[i].InnerText;
                        nodeList[i].InnerText = "";


                        if (flagNodes == null)
                        {
                            //Fix lỗi bị dính từ khi xuất
                            //<w:t xml:space="preserve"> </w:t>
                            if (!nodeList[joinStartIndex.Value].OuterXml.Contains("xml:space=\"preserve\"") &&
                                nodeList[i].OuterXml.Contains("xml:space=\"preserve\""))
                            {
                                //Bổ sung thuộc tính
                                AddAttributeInNode(nodeList[joinStartIndex.Value], "xml:space", "preserve");
                            }
                            //spacing là khoảng cách giữa 2 ký tự trong 1 từ nên không được phép chuyển spacing
                            //if (OriginStyleExport)
                            //{
                            //    //Chuyển spacing hiện tại sang spacing cần ghép nếu spacing > 50                                  
                            //}                            
                        }

                        if (nodeList[i].NextSibling != null)
                        {
                            //Nếu sau node hiện tại là tab nốt thì di chuyển tabNode kèm theo node hiện tại và reset lại từ đâu
                            if (nodeList[i].NextSibling.Name == prefix + ":tab")
                            {
                                if (nodeList[joinStartIndex.Value].ParentNode != null)
                                    nodeList[joinStartIndex.Value].ParentNode.InsertAfter(nodeList[i].NextSibling, nodeList[joinStartIndex.Value]);
                                //if (!string.IsNullOrEmpty(wrParentNode.InnerText))
                                //{
                                //    //Nếu ngoài node này còn node khác thì reset lại join và xóa w:t hiện tại                                                                                       
                                //}
                                //Sau node này là tabNode thì reset lại
                                joinStartIndex = null;
                            }
                            //Nếu sau node hiện tại là xuống dongf
                            if (nodeList[i].NextSibling != null && nodeList[i].NextSibling.Name == prefix + ":br")
                            {
                                //Nêú là xuống dòng cũng move theo
                                if (nodeList[joinStartIndex.Value].ParentNode != null)
                                    nodeList[joinStartIndex.Value].ParentNode.InsertAfter(nodeList[i].NextSibling, nodeList[joinStartIndex.Value]);

                                joinStartIndex = null;
                            }
                        }
                        if (!string.IsNullOrEmpty(wrParentNode.InnerText))
                        {
                            //Nếu ngoài node này còn node khác thì xóa node này vào join tiếp nếu có thể
                            //joinStartIndex = i;
                            if (nodeList[i].ParentNode != null)
                                nodeList[i].ParentNode.RemoveChild(nodeList[i]);
                            //continue;
                        }
                        else
                        {
                            //Nếu không còn dữ liệu thì xóa bớt
                            //Tìm node cấp dưới của parentNode để xóa                        
                            if (wrParentNode != null && wrParentNode.ParentNode != null)
                            {
                                //deletedNodes.Add(wrParentNode);
                                wrParentNode.ParentNode.RemoveChild(wrParentNode);
                            }
                        }

                    }
                }
            }
            if (System.Diagnostics.Debugger.IsAttached)
                doc.Save("C:\\Temp\\Root3" + System.Guid.NewGuid() + ".xml");
        }

        private bool GetWordByAbbyy(Video video, System.Xml.XmlNode currrentNode, ref string word, bool isNext = true, bool firstLevel = true)
        {
            if (currrentNode is null)
                return false;
            var nextNode = firstLevel ? (isNext ? currrentNode.NextSibling : currrentNode.PreviousSibling) : currrentNode;
            bool found = false;
            while (!found && nextNode != null)
            {
                if (nextNode.Name == "w:rPr" && video.NodeSuper)
                {
                    bool isSuperscript = nextNode.InnerXml.Contains("w:val=\"superscript\"");
                    if (isSuperscript)
                    {
                        return true;
                    }
                }
                else
                {
                    if (nextNode.Name == "w:br" || nextNode.Name == "w:tab")
                    {
                        return true;
                    }
                    if (nextNode.Name == "w:t" && !string.IsNullOrEmpty(nextNode.InnerText))
                    {
                        if (isNext)
                        {
                            var spaceIndex = nextNode.InnerText.IndexOf(' ');
                            if (spaceIndex != 0)
                            {
                                //Nếu là các ký tự xuống dòng cũng hợp lệ
                                foreach (var newLineText in Module.Helpers.TextHelper.NewLineText)
                                {
                                    var newLineIndex = nextNode.InnerText.IndexOf(newLineText);
                                    if (newLineIndex >= 0)
                                    {
                                        if (spaceIndex < 0 || spaceIndex > newLineIndex)
                                            spaceIndex = newLineIndex;
                                    }
                                }
                            }
                            if (spaceIndex == 0)
                                return true;

                            else if (spaceIndex > 0)
                            {
                                word += nextNode.InnerText.Substring(0, spaceIndex);
                                return true;
                            }
                            else
                            {
                                word += nextNode.InnerText;
                            }
                        }
                        else
                        {
                            var spaceIndex = nextNode.InnerText.LastIndexOf(' ');
                            if (spaceIndex != nextNode.InnerText.Length - 1)
                            {
                                //Nếu là các ký tự xuống dòng cũng hợp lệ
                                foreach (var newLineText in Module.Helpers.TextHelper.NewLineText)
                                {
                                    var newLineIndex = nextNode.InnerText.LastIndexOf(newLineText);
                                    if (newLineIndex >= 0)
                                    {
                                        if (spaceIndex < 0 || spaceIndex < newLineIndex)
                                            spaceIndex = newLineIndex;
                                    }
                                }
                            }
                            if (spaceIndex == nextNode.InnerText.Length - 1)
                                return true;
                            else if (spaceIndex >= 0)
                            {
                                word = nextNode.InnerText.Substring(spaceIndex + 1) + word;
                                return true;
                            }
                            else
                            {
                                word = nextNode.InnerText + word;
                            }
                        }

                    }
                }

                nextNode = isNext ? nextNode.NextSibling : nextNode.PreviousSibling;
            }
            if (!found && currrentNode.ParentNode != null)
            {
                var continueNode = isNext ? currrentNode.ParentNode.NextSibling : currrentNode.ParentNode.PreviousSibling;
                while (!found && continueNode != null && continueNode.ParentNode != null)
                {
                    if (string.IsNullOrEmpty(continueNode.InnerXml) || !continueNode.InnerXml.Contains("</w:t>"))
                    {
                        //Nếu không chứa text node thì trả về
                        return true;
                    }
                    if (isNext)
                    {
                        found = GetWordByAbbyy(video, continueNode.FirstChild, ref word, isNext, false);
                    }
                    else
                    {
                        found = GetWordByAbbyy(video, continueNode.LastChild, ref word, isNext, false);
                    }
                    continueNode = isNext ? continueNode.ParentNode.NextSibling : continueNode.ParentNode.PreviousSibling;
                }
            }
            return found;
        }

        private System.Xml.XmlAttribute AddAttributeInNode(System.Xml.XmlNode node, string name, string value)
        {
            System.Xml.XmlAttribute attribute = null;
            if (name.Contains(":"))
                attribute = node.OwnerDocument.CreateAttribute(name, node.NamespaceURI);
            else
                attribute = node.OwnerDocument.CreateAttribute(name);
            if (!string.IsNullOrEmpty(value))
                attribute.Value = value;
            node.Attributes.Append(attribute);
            return attribute;
        }

        private bool ExistedTabNode(System.Collections.Generic.List<System.Xml.XmlNode> nodeList, int current, char prefix = 'w')
        {
            var wrParentNode = GetParentNode(nodeList[current], prefix + ":r");
            if (wrParentNode != null)
            {
                //Kiểm tra nếu có tabNode  thì bỏ qua
                foreach (System.Xml.XmlNode tabNode in wrParentNode.ChildNodes)
                {
                    if (tabNode.Name == prefix + ":tab")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private int? GetSpacingValueInWord(System.Xml.XmlNode wtNode)
        {
            System.Xml.XmlNode rPrNode = null;
            if (wtNode.PreviousSibling != null && wtNode.PreviousSibling.Name == "w:rPr")
                rPrNode = wtNode.PreviousSibling;
            else if (wtNode.ParentNode != null && wtNode.ParentNode.FirstChild != null && wtNode.ParentNode.FirstChild.Name == "w:rPr")
                rPrNode = wtNode.ParentNode.FirstChild;
            if (rPrNode != null)
            {
                foreach (System.Xml.XmlNode childNode in rPrNode.ChildNodes)
                    if (childNode.Name == "w:spacing")
                        return Int32.Parse(this.GetAttributeInNode(childNode));
            }
            return null;
        }
        private System.Xml.XmlNode GetParagraphRprNode(System.Xml.XmlNode node, char prefix = 'w')
        {
            var wpNode = GetParentNode(node, prefix + ":p", false);
            if (wpNode != null && wpNode.FirstChild.Name == prefix + ":pPr")
            {
                foreach (System.Xml.XmlNode styleNode in wpNode.FirstChild.ChildNodes)
                {
                    if (styleNode.Name == prefix + ":rPr")
                        return styleNode;
                }
            }
            return null;
        }
        private bool BreakJoinNode(Video video, System.Collections.Generic.List<System.Xml.XmlNode> nodeList, int current, System.Xml.XmlNode wrParentNode, System.Xml.XmlNode wrJoinParent, char prefix = 'w')
        {
            if (nodeList[current] != null)
            {
                if (nodeList[current].PreviousSibling != null)
                {
                    if (nodeList[current].PreviousSibling.Name == prefix + ":tab")
                        return true;
                    else if (video.AbbyyTermLocation && nodeList[current].PreviousSibling.Name == prefix + ":br")
                        return true;
                }
                if (video.ElementSpacing != null)
                {
                    //Khoảng cách giữa 2 node, trên word có thể là 2 từ theo chiều ngang
                    if (string.IsNullOrEmpty(nodeList[current].InnerText) || nodeList[current].InnerText == " ")
                    {
                        //Trường hợp ký tự ngăn cách
                        if (prefix == 'w')
                        {
                            var spacingValue = GetSpacingValueInWord(nodeList[current]);
                            //if (nodeList[current].PreviousSibling != null && nodeList[current].PreviousSibling.Name == prefix + ":rPr")
                            //{
                            //    foreach (System.Xml.XmlNode childNode in nodeList[current].PreviousSibling.ChildNodes)
                            //    {
                            //        if (childNode.Name == prefix + ":spacing")
                            //        {
                            //            spacingValue = Int32.Parse(this.GetAttributeInNode(childNode));
                            //            //foreach (System.Xml.XmlAttribute att in childNode.Attributes)
                            //            //{
                            //            //    if (att.Name == prefix + ":val")
                            //            //    {
                            //            //        spacingValue = Int32.Parse(att.Value);
                            //            //        break;
                            //            //    }
                            //            //}
                            //            break;
                            //        }
                            //    }
                            //}
                            //if (spacingValue > 100)
                            if (spacingValue != null && spacingValue > video.ElementSpacing.Value)
                            {
                                return true;
                            }
                        }
                        else if (prefix == 'a')
                        {
                            //Trường hợp power point thì không xử lý
                        }

                    }
                }
                if (video.NodeSuper)
                {
                    if (wrParentNode.FirstChild != null && wrParentNode.FirstChild.Name == prefix + ":rPr"
                        && wrParentNode.FirstChild.InnerXml.Contains("<" + prefix + ":vertAlign w:val=\"superscript\""))
                    {
                        return true;
                    }
                }
            }

            if (wrParentNode != null && wrParentNode.FirstChild != null &&
                     wrJoinParent != null && wrJoinParent.FirstChild != null)
            {
                //Xác định style cho wrParentNode
                string wrParentNodeFont = "";
                string wrParentNodeSize = "";
                string wrParentNodeFontColor = "";
                string wrParentNodeFontBold = "";
                string wrParentNodeFontItalic = "";
                string wrParentNodeFontUnderline = "";
                string wrParentNodeOutline = "";
                string wrParentNodeAlignment = "";
                string wrParentNodeSpacing = "";
                string wrParentNodeIndent = "";
                var wrParentNodeRprNode = wrParentNode.FirstChild.Name == prefix + ":rPr" ? wrParentNode.FirstChild : GetParagraphRprNode(wrParentNode, prefix);
                if (wrParentNodeRprNode != null && wrParentNodeRprNode.Name == prefix + ":rPr")
                {
                    if (prefix == 'w')
                    {
                        //Word
                        foreach (System.Xml.XmlNode styleNode in wrParentNode.FirstChild.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":rFonts")
                            {
                                wrParentNodeFont = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":sz")
                            {
                                wrParentNodeSize = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":color")
                            {
                                wrParentNodeFontColor = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":b")
                            {
                                wrParentNodeFontBold = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":i")
                            {
                                wrParentNodeFontItalic = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":u")
                            {
                                wrParentNodeFontUnderline = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":outlineLvl")
                            {
                                wrParentNodeOutline = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":jc")
                            {
                                wrParentNodeAlignment = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":spacing")
                            {
                                wrParentNodeSpacing = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":ind")
                            {
                                wrParentNodeIndent = styleNode.OuterXml;
                            }
                        }
                    }
                    else if (prefix == 'a')
                    {
                        //Power Point
                        foreach (System.Xml.XmlNode styleNode in wrParentNode.FirstChild.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":cs")
                            {
                                wrParentNodeFont = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":solidFill")
                            {
                                wrParentNodeFontColor = styleNode.OuterXml;
                            }

                        }
                        foreach (System.Xml.XmlAttribute attribute in wrParentNode.FirstChild.Attributes)
                        {
                            if (attribute.Name == prefix + ":sz")
                            {
                                wrParentNodeSize = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":b")
                            {
                                wrParentNodeFontBold = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":i")
                            {
                                wrParentNodeFontItalic = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":u")
                            {
                                wrParentNodeFontUnderline = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":lvl")
                            {
                                wrParentNodeOutline = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":algn")
                            {
                                wrParentNodeAlignment = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":lnSpc" || attribute.Name == prefix + ":spcBef" || attribute.Name == prefix + ":spcAft")
                            {
                                wrParentNodeSpacing += attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":indent" || attribute.Name == prefix + ":marL")
                            {
                                wrParentNodeIndent += attribute.OuterXml;
                            }

                        }

                    }

                }
                else
                {
                    //Nếu không có style thì luôn luôn ghép
                    //return false;
                }
                string wrJoinParentFont = "";
                string wrJoinParentSize = "";
                string wrJoinParentFontColor = "";
                string wrJoinParentFontBold = "";
                string wrJoinParentFontItalic = "";
                string wrJoinParentFontUnderline = "";
                string wrJoinParentOutline = "";
                string wrJoinParentAlignment = "";
                string wrJoinParentSpacing = "";
                string wrJoinParentIndent = "";
                var wrJoinParentRprNode = wrJoinParent.FirstChild.Name == prefix + ":rPr" ? wrJoinParent.FirstChild : GetParagraphRprNode(wrJoinParent, prefix);
                if (wrJoinParentRprNode != null && wrJoinParentRprNode.Name == prefix + ":rPr")
                {
                    if (prefix == 'w')
                    {
                        //Word
                        foreach (System.Xml.XmlNode styleNode in wrJoinParent.FirstChild.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":rFonts")
                            {
                                //Nếu khác phông mặc định là không ghép
                                wrJoinParentFont = styleNode.OuterXml;
                                if (wrParentNodeFont != wrJoinParentFont)
                                    return true;
                            }
                            else if (styleNode.Name == prefix + ":sz")
                            {
                                //Nếu khác kích thước mặc định là không ghép
                                wrJoinParentSize = styleNode.OuterXml;
                                if (wrParentNodeSize != wrJoinParentSize)
                                    return true;
                            }
                            else if (styleNode.Name == prefix + ":color")
                            {
                                wrJoinParentFontColor = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":b")
                            {
                                wrJoinParentFontBold = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":i")
                            {
                                wrJoinParentFontItalic = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":u")
                            {
                                wrJoinParentFontUnderline = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":outlineLvl")
                            {
                                wrJoinParentOutline = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":jc")
                            {
                                wrJoinParentAlignment = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":spacing")
                            {
                                wrJoinParentSpacing = styleNode.OuterXml;
                            }
                            else if (styleNode.Name == prefix + ":ind")
                            {
                                wrJoinParentIndent = styleNode.OuterXml;
                            }
                        }
                    }
                    else if (prefix == 'a')
                    {
                        //Power Point
                        foreach (System.Xml.XmlNode styleNode in wrJoinParent.FirstChild.ChildNodes)
                        {
                            if (styleNode.Name == prefix + ":cs")
                            {
                                //Nếu khác phông mặc định là không ghép
                                wrJoinParentFont = styleNode.OuterXml;
                                if (wrParentNodeFont != wrJoinParentFont)
                                    return true;
                            }
                            else if (styleNode.Name == prefix + ":solidFill")
                            {
                                wrJoinParentFontColor = styleNode.OuterXml;
                            }
                        }
                        foreach (System.Xml.XmlAttribute attribute in wrJoinParent.FirstChild.Attributes)
                        {
                            if (attribute.Name == prefix + ":sz")
                            {
                                //Nếu khác kích thước mặc định là không ghép
                                wrJoinParentSize = attribute.OuterXml;
                                if (wrParentNodeSize != wrJoinParentSize)
                                    return true;
                            }
                            else if (attribute.Name == prefix + ":b")
                            {
                                wrJoinParentFontBold = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":i")
                            {
                                wrJoinParentFontItalic = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":u")
                            {
                                wrJoinParentFontUnderline = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":lvl")
                            {
                                wrJoinParentOutline = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":algn")
                            {
                                wrJoinParentAlignment = attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":lnSpc" || attribute.Name == prefix + ":spcBef" || attribute.Name == prefix + ":spcAft")
                            {
                                wrJoinParentSpacing += attribute.OuterXml;
                            }
                            else if (attribute.Name == prefix + ":indent" || attribute.Name == prefix + ":marL")
                            {
                                wrJoinParentIndent += attribute.OuterXml;
                            }

                        }
                    }

                }
                else
                {
                    //Nếu không có style thì luôn luôn ghép
                    //return false;
                }

                //Nếu khác phông mặc định là không ghép               
                if (wrParentNodeFont != wrJoinParentFont)
                    return true;

                //Nếu khác kích thước mặc định là không ghép             
                if (wrParentNodeSize != wrJoinParentSize)
                    return true;

                if (video.NodeFontColor && wrParentNodeFontColor != wrJoinParentFontColor)
                    return true;
                if (video.NodeFontBold && wrParentNodeFontBold != wrJoinParentFontBold)
                    return true;
                if (video.NodeFontItalic && wrParentNodeFontItalic != wrJoinParentFontItalic)
                    return true;
                if (video.NodeFontUnderline && wrParentNodeFontColor != wrJoinParentFontColor)
                    return true;
                //if (NodeOutline && wrParentNodeOutline != wrJoinParentOutline)
                //    return true;
                //if (NodeAlignment && wrParentNodeAlignment != wrJoinParentAlignment)
                //    return true;
                //if (NodeSpacing && wrParentNodeSpacing != wrJoinParentSpacing)
                //    return true;
                //if (NodeIndent && wrParentNodeIndent != wrJoinParentIndent)
                //    return true;
            }
            return false;
        }

        public System.Xml.XmlNode GetParentNode(System.Xml.XmlNode node, string nodeName = "w:p", bool current = true)
        {
            if (node == null) return null;
            if (current)
            {
                //Nếu là node hiện tại
                if (node.Name == nodeName)
                {
                    return node;
                }
                if (nodeName.StartsWith(":") && node.Name.EndsWith(nodeName))
                {
                    return node;
                }
                else
                {
                    return GetParentNode(node.ParentNode, nodeName, current);
                }
            }
            else if (node.ParentNode != null)
            {
                //Nếu tìm Node cấp độ con
                if (node.ParentNode.Name == nodeName)
                {
                    return node;
                }
                if (nodeName.StartsWith(":") && node.ParentNode.Name.EndsWith(nodeName))
                {
                    return node;
                }
                else
                {
                    return GetParentNode(node.ParentNode, nodeName, current);
                }
            }
            return null;
        }


        //private int order = 0;        //Trường hợp có thứ tự
        //private void AddAudioFromNodes(Video video, System.Xml.XmlNode node, string caption, int level, int index1 = 1, int index2 = 0, int index3 = 0, int index4 = 0, int index5 = 0)
        //{
        //    if (node is null)
        //        return;
        //    if (node.ChildNodes is null)
        //        return;
        //    for (int i = 0; i < node.ChildNodes.Count; i++)
        //    {
        //        if (node.ChildNodes[i].ChildNodes.Count == 1)
        //        {
        //            if (string.IsNullOrEmpty(node.ChildNodes[i].InnerText))
        //                continue;
        //            //Nếu không có ký tự thường thì bỏ qua
        //            bool hasChar = false;
        //            foreach (var c in node.ChildNodes[i].InnerText)
        //            {
        //                if (char.IsLetter(c))
        //                {
        //                    hasChar = true;
        //                    break;
        //                }
        //            }
        //            if (!hasChar)
        //                continue;
        //            var audio = new Audio(Session);
        //            audio.Video = video;
        //            AudioList.Add(audio);
        //            string content = node.ChildNodes[i].InnerText.Replace(" ", " ");
        //            if (!KeepSpace)
        //                content = content.Trim();
        //            audio.Content = content;
        //            //audio.Order = order;
        //            audio.Start = TimeSpan.FromDays(index1);
        //            if (level >= 1)
        //                audio.Start = audio.Start.Value.Add(TimeSpan.FromHours(index1));
        //            if (level >= 2)
        //                audio.Start = audio.Start.Value.Add(TimeSpan.FromMinutes(index2));
        //            if (level >= 4)
        //                audio.Start = audio.Start.Value.Add(TimeSpan.FromSeconds(index3));
        //            if (level >= 5)
        //                audio.Start = audio.Start.Value.Add(TimeSpan.FromMilliseconds(index4));
        //            order++;
        //        }
        //        else
        //        {
        //            AddAudioFromNodes(video, node.ChildNodes[i], caption, level + 1, level == 0 ? i + 1 : index1,
        //                level == 1 ? i + 1 : index2, level == 2 ? i + 1 : index3, level == 3 ? i + 1 : index4, level >= 4 ? i + 1 : index5);
        //        }
        //    }
        //}
        //Trường hợp gộp text
        //private void AddAudioFromNodes(Video video, System.Xml.XmlNode node, string caption, char prefix = 'w')
        //{
        //    if (node is null)
        //        return;
        //    if (node.ChildNodes is null)
        //        return;
        //    for (int i = 0; i < node.ChildNodes.Count; i++)
        //    {
        //        if (node.ChildNodes[i].Name == prefix + ":p" || node.ChildNodes[i].ChildNodes.Count == 1)
        //        {
        //            string text = node.ChildNodes[i].InnerText;
        //            if (string.IsNullOrEmpty(text))
        //                continue;
        //            //Nếu không có ký tự thường thì bỏ qua
        //            bool hasChar = false;
        //            foreach (var c in text)
        //            {
        //                if (char.IsLetter(c))
        //                {
        //                    hasChar = true;
        //                    break;
        //                }
        //            }
        //            if (!hasChar)
        //                continue;
        //            var audio = new Audio(Session);
        //            audio.Video = video;
        //            AudioList.Add(audio);
        //            if (!KeepSpace)
        //                text = text.Trim();                   
        //            audio.Content = text.Replace(" ", " ");
        //            //audio.Order = order;
        //            audio.Start = TimeSpan.FromSeconds(order);
        //            order++;
        //        }
        //        else
        //        {
        //            AddAudioFromNodes(video, node.ChildNodes[i], caption);
        //        }
        //    }
        //}
        //public void ImportPdf(string file, ref int index, BookMark bookMark = null)
        //{
        //    PdfSharp.Pdf.PdfDocument document = PdfReader.Open(file, PdfDocumentOpenMode.Modify);

        //    for (int pageIdx = 0; pageIdx < document.PageCount; pageIdx++)
        //    {
        //        var page = document.Pages[pageIdx];
        //        // Extract text from the page (This example uses a simple approach)
        //        // In real-world scenarios, you'd need a more complex text extraction method
        //        //string pageText = ExtractTextFromPdfPage(page);

        //        //if (pageText.Contains(searchText))
        //        //{
        //        //    // Replace text
        //        //    string updatedText = pageText.Replace(searchText, replaceText);

        //        //    // In real-world scenarios, you would then need to redraw the text onto the page
        //        //    // However, PDFSharp does not provide an out-of-the-box method to replace text while preserving format.
        //        //    // Therefore, you need to handle the text positioning and styling manually.
        //        //}
        //    }
        //    //using (iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(file)))
        //    //{
        //    //    var audioList = new System.Collections.Generic.List<Audio>();
        //    //    var pageNumbers = pdfDocument.GetNumberOfPages();

        //    //    for (int i = 1; i <= pageNumbers; i++)
        //    //    {
        //    //        LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
        //    //        PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
        //    //        parser.ProcessPageContent(pdfDocument.GetFirstPage());
        //    //        string text = strategy.GetResultantText();

        //    //        var audio = new Audio(Session);
        //    //        audioList.Add(audio);
        //    //        audio.Content = text.Replace(" ", " ");
        //    //        //audio.Order = index;
        //    //        audio.Start = TimeSpan.FromSeconds(index);
        //    //        if (bookMark != null)
        //    //            audio.BookMark = bookMark;                    
        //    //        index++;
        //    //    }

        //    //    AudioList.AddRange(audioList);
        //    //    //ParagraphStyleList.AddRange(paragraphStyleList);
        //    //    //paragraphStyleList = ParagraphStyleList.Where(m => string.IsNullOrEmpty(m.Name)).OrderBy(m => m.Size).ToList();
        //    //    ////2023-06-22: Tên Style để là S01, S02 và tăng dần, có tính năng sửa tên sau khi sort theo độ lớn font để thứ tự từ 01 > 99
        //    //    //for (int i = 0; i < paragraphStyleList.Count; i++)
        //    //    //{
        //    //    //    var newIndex = i + 1;
        //    //    //    string styleName = (newIndex + 1).ToString();
        //    //    //    if (newIndex < 9)
        //    //    //        styleName = "00" + styleName;
        //    //    //    else if (newIndex < 99)
        //    //    //        styleName = "0" + styleName;
        //    //    //    else if (newIndex >= 999)
        //    //    //        styleName += "(Không hỗ trợ)";
        //    //    //    paragraphStyleList[i].Name += styleName;
        //    //    //}
        //    //    ShowWaitForm(null, null);
        //    //    //Console.WriteLine(pageText);
        //    //}
        //}

        public System.Collections.Generic.List<string> GetWordNotIsLanguageTranslate(Video video, string text, ref System.Collections.Generic.List<string> wordsNotIsLanguageTranslate)
        {
            if (string.IsNullOrEmpty(text))
                return wordsNotIsLanguageTranslate;
            if (video.LanguageOrigin is null || video.LanguageTranslate is null)
            {
                _notificationService.NotifyError("Lỗi", "Chưa chọn ngôn ngữ gốc hoặc ngôn ngữ dịch");
                return wordsNotIsLanguageTranslate;
            }
            if (!string.IsNullOrEmpty(video.LanguageOrigin.Code) || !string.IsNullOrEmpty(video.LanguageTranslate.Code))
            {
                _notificationService.NotifyError("Lỗi", "Mã ngôn ngữ gốc hoặc ngôn ngữ dịch bị trống");
                return wordsNotIsLanguageTranslate;
            }
            string[] words = text.Split(' ');
            if (wordsNotIsLanguageTranslate is null)
                wordsNotIsLanguageTranslate = new System.Collections.Generic.List<string>();
            //// Tạo đối tượng LanguageDetector
            var detector = new LanguageDetection.LanguageDetector();
            detector.AddLanguages(new string[] { video.LanguageTranslate.Code, video.LanguageOrigin.Code });
            var languageDetector = new LanguageDetection.LanguageDetector();
            foreach (var word in words)
            {
                if (wordsNotIsLanguageTranslate.Contains(word))
                    continue;
                // Phát hiện ngôn ngữ của từng từ
                var language = languageDetector.Detect(word);
                if (!video.LanguageTranslate.Code.Equals(language))
                    wordsNotIsLanguageTranslate.Add(word);
            }

            return wordsNotIsLanguageTranslate;
        }

        public System.Collections.Generic.List<string> GetWordNotIsLanguageTranslate(Video video, string text, string checkLanguageCode, ref System.Collections.Generic.List<string> wordsNotIsLanguageTranslate)
        {
            if (string.IsNullOrEmpty(text))
                return wordsNotIsLanguageTranslate;

            string[] words = text.Split(' ');
            if (wordsNotIsLanguageTranslate is null)
                wordsNotIsLanguageTranslate = new System.Collections.Generic.List<string>();
            //// Tạo đối tượng LanguageDetector


            //var languageDetector3 = new LanguageDetection.LanguageDetector();
            //var result5 = languageDetector3.DetectLanguage(text);
            //detector.AddLanguages(new string[] { LanguageTranslate.Code, LanguageOrigin.Code });
            var languageDetector = new LanguageDetection.LanguageDetector();
            if (checkLanguageCode.Equals(checkLanguageCode.StartsWith("en", System.StringComparison.OrdinalIgnoreCase)))
                languageDetector.AddLanguages();
            else
                languageDetector.AddLanguages(new string[] { video.LanguageTranslate.GetLanguageCodeIso6392(), video.LanguageOrigin.GetLanguageCodeIso6392() });
            foreach (var word in words)
            {
                var trimWord = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(word);
                if (string.IsNullOrEmpty(trimWord))
                    continue;
                //var result4 = languageDetector2.DetectLanguage(word);
                if (wordsNotIsLanguageTranslate.Contains(trimWord))
                    continue;
                // Phát hiện ngôn ngữ của từng từ
                var language = languageDetector.Detect(trimWord);
                if (string.IsNullOrEmpty(language))
                    wordsNotIsLanguageTranslate.Add(trimWord);
                else if (!language.StartsWith(checkLanguageCode, System.StringComparison.OrdinalIgnoreCase))
                    wordsNotIsLanguageTranslate.Add(trimWord);
            }
            return wordsNotIsLanguageTranslate;
        }

        //public System.Collections.Generic.List<string> GetWordNotIsLanguageTranslateCLD2Net(Video video, string text, string checkLanguageCode, ref System.Collections.Generic.List<string> wordsNotIsLanguageTranslate)
        //{
        //    if (string.IsNullOrEmpty(text))
        //        return wordsNotIsLanguageTranslate;

        //    string[] words = text.Split(' ');
        //    if (wordsNotIsLanguageTranslate is null)
        //        wordsNotIsLanguageTranslate = new System.Collections.Generic.List<string>();
        //    //// Tạo đối tượng LanguageDetector
        //    var languageDetector = new CLD2Net.LanguageDetector();
        //    foreach (var word in words)
        //    {
        //        if (wordsNotIsLanguageTranslate.Contains(word))
        //            continue;
        //        // Phát hiện ngôn ngữ của từng từ
        //        var language = languageDetector.DetectLanguage(word);
        //        if (!checkLanguageCode.Equals(language, System.StringComparison.OrdinalIgnoreCase))
        //            wordsNotIsLanguageTranslate.Add(word);
        //    }

        //    return wordsNotIsLanguageTranslate;
        //}
        public void ImportAudiosFromPyanoteString(Video video, string input, BookMark bookMark)
        {
            var lines = ((string)input).Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            var cultureInfo = new System.Globalization.CultureInfo("en-US");
            foreach (var line in lines)
            {
                try
                {
                    var textArray = line.Split(' ', 4);
                    if (textArray.Length != 4)
                        continue;
                    var timeStart = TimeSpan.FromSeconds(double.Parse(textArray[0], cultureInfo));
                    var timeEnd = TimeSpan.FromSeconds(double.Parse(textArray[1], cultureInfo));
                    if (bookMark != null && bookMark.Order != null)
                    {
                        timeStart = timeStart.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                        timeEnd = timeEnd.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                    }
                    //Nạp ngôn ngữ gốc
                    var audio = CreateObject<Audio>();
                    string content = textArray[3].Trim().Replace(" ", " ");
                    if (!video.KeepSpace)
                        content = content.Trim();
                    audio.Content = content;
                    audio.Start = timeStart;
                    audio.End = timeEnd;
                    audio.Note = textArray[2];
                    if (bookMark != null)
                        audio.BookMark = bookMark;
                    video.AudioList.Add(audio);
                }
                catch (Exception)
                {

                }
            }
        }

        public void ImportAudiosFromJsonString(Video video, string input, BookMark bookMark)
        {
            _notificationService.NotifyWarning("Thông báo", "Chữa hỗ trợ");
        }

        public void ImportAudiosFromTextString(Video video, string input, BookMark bookMark)
        {
            _notificationService.NotifyWarning("Thông báo", "Chữa hỗ trợ");
        }

        public void ImportAudiosFromSrtFile(Video video, string input, BookMark bookMark)
        {
            using (var fileStream = File.OpenRead(input))
            {
                var parser = new SubtitlesParser.Classes.Parsers.SrtParser();
                var items = parser.ParseStream(fileStream, System.Text.Encoding.UTF8);
                foreach (var item in items)
                {
                    var audio = CreateObject<Audio>();
                    if (bookMark != null && bookMark.Order != null)
                    {
                        audio.Start = TimeSpan.FromMilliseconds(item.StartTime).Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                        audio.End = TimeSpan.FromMilliseconds(item.EndTime).Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                    }
                    audio.Content = string.Join(" ", item.Lines);

                    if (bookMark != null)
                        audio.BookMark = bookMark;

                    if (video.AudioList.Count > 0)
                    {
                        var lastAudio = video.AudioList[video.AudioList.Count - 1];
                        if (lastAudio.End > audio.Start)
                            lastAudio.End = audio.Start;
                    }

                    video.AudioList.Add(audio);
                }

            }
        }
        public static void ImportAudiosFromSrtString(Video video, string input, BookMark bookMark)
        {
            var cultureInfo = new System.Globalization.CultureInfo("vi-VN");
            var addList = new System.Collections.Generic.List<Audio>();

            var lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            string srtIndex = null;
            string timeline = null;
            List<string> contentLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                // Nếu dòng là số nguyên, nghĩa là bắt đầu block mới
                if (int.TryParse(line, out int currentIndex))
                {
                    // Nếu đang có block cũ chưa xử lý, tạo Audio từ block cũ trước
                    if (!string.IsNullOrEmpty(srtIndex) && !string.IsNullOrEmpty(timeline))
                    {
                        // Xử lý block cũ
                        ProcessBlock(video, srtIndex, timeline, contentLines, bookMark, cultureInfo, addList);
                    }

                    // Reset block mới
                    srtIndex = line;
                    timeline = null;
                    contentLines.Clear();
                }
                else if (srtIndex != null && timeline == null)
                {
                    // Dòng timeline
                    timeline = line;
                }
                else if (srtIndex != null && timeline != null)
                {
                    // Dòng nội dung
                    contentLines.Add(line);
                }
                // Nếu dòng rỗng hoặc khác biệt thì bỏ qua, tự động gom nội dung
            }

            // Xử lý block cuối cùng nếu còn dữ liệu
            if (!string.IsNullOrEmpty(srtIndex) && !string.IsNullOrEmpty(timeline))
            {
                ProcessBlock(video, srtIndex, timeline, contentLines, bookMark, cultureInfo, addList);
            }

            video.AudioList.AddRange(addList);
        }

        // Hàm tách riêng để xử lý block phụ đề
        private static void ProcessBlock(Video video, string srtIndex, string timeline, List<string> contentLines, BookMark bookMark, System.Globalization.CultureInfo cultureInfo, List<Audio> addList)
        {
            try
            {
                var timer = timeline.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var timeStart = TimeSpan.Parse(timer[0], cultureInfo);
                var timeEnd = TimeSpan.Parse(timer[2], cultureInfo);

                if (bookMark != null && bookMark.Order != null)
                {
                    timeStart = timeStart.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                    timeEnd = timeEnd.Add(TimeSpan.FromDays(Convert.ToInt32(bookMark.Order)));
                }

                // Gộp nội dung nhiều dòng thành 1 dòng, cách nhau bởi khoảng trắng
                var content = string.Join(" ", contentLines).Trim();

                if (!video.KeepSpace)
                    content = content.Trim();
                if (!string.IsNullOrEmpty(content))
                {
                    var audio = CreateObjectFromObject<Audio>(video);
                    audio.Content = content;
                    audio.Start = timeStart;
                    audio.End = timeEnd;
                    if (bookMark != null)
                        audio.BookMark = bookMark;

                    addList.Add(audio);
                }
                else
                {
                    if (addList.Count > 0)
                    {
                        var lastAudio = addList[addList.Count - 1];
                        lastAudio.End = timeStart;
                    }
                }
            }
            catch (Exception)
            {
                // Log hoặc xử lý lỗi nếu muốn
            }
        }


        System.Diagnostics.Process _process = null;
        private System.Diagnostics.Process GetProcess()
        {
            if (_process == null)
            {
                var ffmpegUrl = GetValueOrDefault("FfmpegUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffmpeg.exe");
                //Copy FFMpeg vào thư mục đang chạy
                var ffmpegFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "ffmpeg.exe";
                if (!System.IO.File.Exists(ffmpegFile))
                    System.IO.File.Copy(ffmpegUrl, ffmpegFile);

                _process = new System.Diagnostics.Process();
                _process.StartInfo.FileName = "ffmpeg.exe";
                //process.StartInfo.FileName = ffmpegUrl;
                _process.EnableRaisingEvents = false;
                //process.StartInfo.WorkingDirectory = @"D:\pipetest\pipetest\ffmpegx86";
                //Giảm âm lượng file gốc xuống còn 30%
                //process.StartInfo.Arguments = string.Format("-y -i \"{0}\" -af \"volume=0.3\" \"{1}\"", Path, rootAudio);
                //process.StartInfo.UseShellExecute = false;
                _process.StartInfo.RedirectStandardInput = true;
                _process.StartInfo.CreateNoWindow = false;
            }
            return _process;
        }
        public void ExportAudioListToMp3(Video video, System.Collections.Generic.List<Audio> audioWithSort, string audioPath)
        {
            var tempFolder = "Temp" + video.Oid.ToString().Substring(0, 11);
            var tempFolderFull = System.IO.Directory.GetCurrentDirectory() + "\\" + tempFolder;
            if (!System.IO.Directory.Exists(tempFolderFull))
                System.IO.Directory.CreateDirectory(tempFolderFull);
            NAudio.Wave.SampleProviders.MixingSampleProvider mixer = null;
            int audioIndex = 1;
            var cultureInfo = new System.Globalization.CultureInfo("en-us");
            var process = GetProcess();
            foreach (Audio audio in audioWithSort)
            {
                if (audio.Start != null && audio.FileData != null && audio.FileData.Size > 0)
                {
                    //File Audio sẽ Save lại
                    var audioStart = audio.GetRealTimeSpan(audio.Start);
                    string audioFile = $"{tempFolder}\\{audioStart.Value.Hours}h{audioStart.Value.Minutes}m{audioStart.Value.Seconds}s {audio.Oid}";
                    string audioWithSpeedFile = audioFile + "WithSpeed.mp3";
                    audioFile += ".mp3";

                    float voiceSpeed = 1f;
                    System.IO.File.WriteAllBytes(audioFile, audio.FileData.Content);
                    while (!System.IO.File.Exists(audioFile))
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    if (audio.VoiceSpeed != null && audio.VoiceSpeed != (decimal)0)
                        voiceSpeed = Convert.ToSingle(audio.VoiceSpeed.Value);
                    if (voiceSpeed == 1)
                    {
                        System.IO.File.WriteAllBytes(audioWithSpeedFile, audio.FileData.Content);
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(audioFile, audio.FileData.Content);
                        //Convert Audio đã sửa tốc độ và volumn
                        //process.StartInfo.Arguments = string.Format("-i \"{0}\" -filter:a \"volume=10\" -filter:a \"atempo={2}\" -vn \"{1}\"", audioFile, audioWithSpeedFile, audio.StretchRatio.ToString(cultureInfo));
                        //Convert Audio đã sửa tốc độ

                        process.StartInfo.Arguments = string.Format("-y -i \"{0}\" -filter:a \"atempo={2}\" -vn \"{1}\"", audioFile, audioWithSpeedFile, voiceSpeed.ToString("n2", cultureInfo));
                        process.StartInfo.CreateNoWindow = true;
                        if (process.Start())
                        {
                            while (!process.HasExited)
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }
                    while (!System.IO.File.Exists(audioWithSpeedFile))
                    {
                        System.Threading.Thread.Sleep(1000);
                    }


                    NAudio.Wave.AudioFileReader readerAudio = new NAudio.Wave.AudioFileReader(audioWithSpeedFile);

                    if (mixer == null)
                    {
                        mixer = new NAudio.Wave.SampleProviders.MixingSampleProvider(readerAudio.WaveFormat);
                    }
                    NAudio.Wave.SampleProviders.OffsetSampleProvider delayAudio = new NAudio.Wave.SampleProviders.OffsetSampleProvider(readerAudio);

                    if (audioStart.Value.TotalMilliseconds > 0)
                    {
                        int sampleRate = delayAudio.WaveFormat.SampleRate;
                        int channels = delayAudio.WaveFormat.Channels;
                        TimeSpan delay = audioStart.Value; // set to whatever you like
                        int samplesToDelay = (int)(sampleRate * delay.TotalSeconds) * channels;
                        delayAudio.DelayBySamples = samplesToDelay;
                    }

                    mixer.AddMixerInput(delayAudio);
                    ShowWaitForm((Convert.ToDecimal(audioIndex) / audioWithSort.Count).ToString("p0"), " ");
                    audioIndex++;
                }
            }

            if (mixer != null)
            {
                //Tạo thư mục audio
                //WaveFileWriter.CreateWaveFile16(tempFolder + "\\mixed.mp3", mixer);
                var converted16Bit = new NAudio.Wave.SampleProviders.SampleToWaveProvider16(mixer);
                //Convert ra mp3
                using (var resampled = new NAudio.Wave.MediaFoundationResampler(converted16Bit, new NAudio.Wave.WaveFormat(44100, 1)))
                {
                    var desiredBitRate = 0; // ask for lowest available bitrate 
                                            //int desiredBitRate = 128000;

                    NAudio.Wave.MediaFoundationEncoder.EncodeToMp3(resampled, audioPath, desiredBitRate);
                }
            }

            ShowWaitForm(null, null);
            System.IO.Directory.Delete(tempFolderFull, true);
        }




        public bool CheckTextIsXpath(string text)
        {
            if (!string.IsNullOrEmpty(text) && (text.StartsWith("/html") || text.StartsWith("//*[")))
            {
                return true;
            }
            return false;
        }

        public void LogToNote(Video video, System.DateTime startTime, string function, int select, int resultCount, System.TimeSpan elapsed)
        {
            video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startTime.ToString("dd/MM/yyyy h:mm"), function, select, resultCount, System.Math.Round(elapsed.TotalMinutes, 0));
        }
        //public ENTOS.Audio GetAudioFromElement(System.TimeSpan element)
        //{
        //    if (AudioList != null && AudioList.Count > 0)
        //    {
        //        return AudioList.OrderBy(m => m.Start).FirstOrDefault(m => m.Start.Value == element);
        //    }
        //    return null;
        //}

        public static System.Collections.Generic.List<Term> GetTermsByLength(Video video, int length)
        {
            return video.TermList.Where(m => !string.IsNullOrEmpty(m.Name) && m.Name.Length == length).ToList();
        }

        public static bool CheckAndUpdateLocationInTermIsValidate(Video video, string termName, Audio currentElement, int currentSentence, int currentPosition, bool requireTerm, ref bool overlap, ref bool flag, bool upperCase = false, bool byName = false, Term currentTerm = null)
        {
            char charTag = '('; // phương thức này dành cho nhiều chức năng nên cố định tag
            if (string.IsNullOrEmpty(termName))
                return false;
            bool termIsLower = char.IsLower(termName[0]);
            bool validate = true;
            var relation1TermLocationList = new System.Collections.Generic.List<TermLocation>();
            var relation2TermLocationList = new System.Collections.Generic.List<TermLocation>();
            //Thuật ngữ cần kiểm tra nằm trong các thuật ngữ này
            var belongTermLocationList = new System.Collections.Generic.List<TermLocation>();
            //Thuật ngữ cần kiểm tra chứa các thuật ngữ này
            var containTermLocationList = new System.Collections.Generic.List<TermLocation>();
            var termNameArray = termName.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int wordLength = termNameArray.Length;
            bool currentIsCorrect = Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(termName);
            if (!currentIsCorrect && video.CheckSpelling && video.GetDictionary() != null)
            {
                currentIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), termName);
            }
            foreach (var relationTermLocation in currentElement.TermLocationList)
            {
                if (relationTermLocation.Sentence != currentSentence)
                    continue;
                var relation = TermLocationService.TermPositionRelation(relationTermLocation, currentElement, currentSentence, currentPosition, termName);
                if (byName && relation != 4)
                {
                    //Kiểm tra lại bằng tên

                    var relationTermNameArray = relationTermLocation.Term?.Name?.ToLower()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var intersect = termNameArray.Intersect(relationTermNameArray);
                    var intersectCount = intersect.Count();
                    if (intersectCount == 0)
                        relation = 4;
                    else if (intersectCount < termNameArray.Length && intersectCount < relationTermNameArray.Length)
                        relation = 1;
                    else if (intersectCount == termNameArray.Length && intersectCount == relationTermNameArray.Length)
                        relation = 0;
                    else if (intersectCount == termNameArray.Length)
                        relation = 3;
                    else if (intersectCount == relationTermNameArray.Length)
                        relation = 2;
                    else
                        relation = 0;
                }
                if (relation == 0)
                {
                    //Lỗi
                    if (relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name) &&
                        (termName.Contains(relationTermLocation.Term.Name) || relationTermLocation.Term.Name.Contains(termName)))
                    {
                        validate = false;
                        break;
                    }
                    else
                    {
                        //var audio = relationTermLocation.GetAudioFromElement().Content;
                    }
                }
                else if (relation == 1)
                {
                    //overlap                    
                    relation1TermLocationList.Add(relationTermLocation);
                }
                else if (relation == 2)
                {
                    //2- TV1 belong (thuộc về) TV2
                    //091: Các chức năng nạp: ghép 2, ghép 3, hoa,... : khi belong thì cũng dựa vào luật: Loại bên thua
                    //Nếu thuật ngữ này sai chính tả thì so sánh tiếp, nếu đúng chính tả xóa luôn thuật ngữ trong vòng for
                    //Thuật ngữ trong vòng for này thuộc về thuật ngữ cần so sánh
                    if (video.CheckSpelling && video.GetDictionary() != null && !currentIsCorrect)
                        containTermLocationList.Add(relationTermLocation);
                    else if (upperCase && relationTermLocation.Term != null
                        && relationTermLocation.Term.TermType == TermType.MergeTerm)
                    {
                        //2025-2-16:Loại bên thua không chỉ sử dụng khi xử lý overlap mà còn khi nạp Thuật ngữ có cướp Thuật vị hiện có không hay Tạo thuật vị Overlap để review. Loại bên thua cần đưa yếu tố Loại thuật ngữ vào: Từ hoa sẽ thua Từ ghép đúng chính tả nhưng thắng Từ ghép sai chính tả
                        //2025-2-5: Nạp từ phức thực hiện trước Nạp hoa, Thuật vị hoa k được cướp thuật vị là Thuật ngữ từ phức                        
                        validate = false;
                        break;

                    }
                    else
                        relation2TermLocationList.Add(relationTermLocation);

                }
                else if (relation == 3)
                {
                    //3 - TV2 belong (thuộc về) TV1
                    //Thuật ngữ so sánh thuộc về thuật ngữ trong vòng for này
                    //Nạp viết hoa: Cho phép nạp Overlap(1) và belong TV2(2) đều dụng cờ overlap,
                    //nếu thuật vị trùng thì đổi Loại thuật ngữ thành Viết hoa / Toàn hoa(tồn tại cả viết thường thì dựng cờ: Lẫn hoa thường)
                    //Kiểm tra chỗ viết hoa viết thường
                    if (upperCase && relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name) && termIsLower != char.IsLower(relationTermLocation.Term.Name[0]))
                    {
                        flag = true;
                        relationTermLocation.Term.Flag = true;
                        //relationTermLocation.Term.AddTextNode("Chữ hoa hoặc chữ thường");
                        if (!string.IsNullOrEmpty(relationTermLocation.Term.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            relationTermLocation.Term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(relationTermLocation.Term.Note, charTag, false);
                        }
                        relationTermLocation.Term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(relationTermLocation.Term.Note, charTag, "Chữ hoa hoặc chữ thường");
                    }
                    //Nếu thuật ngữ này đúng chính tả thì so sánh tiếp, nếu sai chính tả thì coi như là không hợp lệ
                    if (video.CheckSpelling && video.GetDictionary() != null && currentIsCorrect)
                    {
                        //091: Nạp viết hoa cũng áp dụng luật: Loại bên yếu:
                        //-So sánh 2 yếu tố: Độ dài từ và Đúng chính tả: nếu tổng điểm bằng nhau thì giữ cả 2 TV để con người đánh giá, nếu không thì bên thua điểm sẽ bị xóa
                        //Các chức năng nạp: ghép 2, ghép 3, hoa,... : khi belong thì cũng dựa vào luật: Loại bên thua
                        //overlap = true;                        
                        //relationTermLocation.Overlap = true;

                        //Đưa vào danh sách overlap để kiểm tra
                        //Thuật vị cần kiểm tra chứa các thuật vị này
                        //Thuật ngữ cần kiểm tra nằm trong các thuật ngữ này
                        belongTermLocationList.Add(relationTermLocation);

                    }
                    else
                    {
                        validate = false;
                        break;
                    }
                }
            }
            //validate = Module.SystemObjects.Tools.CheckCurrentIndexIsNotParentIndex(content, Name, termIndex, parrentTerms.ToArray());
            if (validate)
            {

                var relationOverlapListTermLocationList = new System.Collections.Generic.List<TermLocation>();
                var relationRemoveListTermLocationList = new System.Collections.Generic.List<TermLocation>();
                //Thuật ngữ cần kiểm tra nằm trong các thuật ngữ này
                if (validate && belongTermLocationList.Count > 0)
                {
                    foreach (var relationTermLocation in belongTermLocationList)
                    {
                        //var element = relationTermLocation.GetAudioFromElement().Content;
                        //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                        //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();
                        //-Khi đánh giá bên thua: Từ hoa tương đương Đúng chính tả
                        var refIsCorrect = Services.TermLocationService.CheckRealNameIsUpperCaseFirstAll(relationTermLocation, null);
                        if (!refIsCorrect && video.CheckSpelling && video.GetDictionary() != null &&
                                relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name))
                            refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                        if (refIsCorrect)
                        {
                            //Thuật ngữ này không hợp lệ
                            validate = false;
                            break;
                        }
                        else
                        {
                            relationRemoveListTermLocationList.Add(relationTermLocation);
                        }
                    }
                }
                //Nếu thuật ngữ này đúng chính tả thì so sánh tiếp, nếu sai chính tả thì coi như là không hợp lệ
                //Thuật ngữ so sánh thuộc về thuật ngữ trong vòng for này
                if (validate && containTermLocationList.Count > 0)
                {
                    foreach (var relationTermLocation in containTermLocationList)
                    {
                        //var element = relationTermLocation.GetAudioFromElement().Content;
                        //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                        //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();                            
                        //-Khi đánh giá bên thua: Từ hoa tương đương Đúng chính tả
                        var refIsCorrect = Services.TermLocationService.CheckRealNameIsUpperCaseFirstAll(relationTermLocation, null);
                        if (!refIsCorrect && video.CheckSpelling && video.GetDictionary() != null &&
                                relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name))
                            refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                        if (refIsCorrect)
                        {
                            validate = false;
                            break;
                        }
                        else
                        {
                            relationRemoveListTermLocationList.Add(relationTermLocation);
                        }
                    }
                }


                if (validate && relation1TermLocationList.Count > 0)
                {
                    foreach (var relationTermLocation in relation1TermLocationList)
                    {
                        //var element = relationTermLocation.GetAudioFromElement().Content;
                        //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                        //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();                            
                        //-Khi đánh giá bên thua: Từ hoa tương đương Đúng chính tả
                        int termPoint = TermLocationService.Spelling(relationTermLocation, video, termName);
                        termPoint += TermLocationService.ExistWord(relationTermLocation, video, termName);
                        termPoint += TermLocationService.Longer(relationTermLocation, video, termName);
                        termPoint += TermLocationService.MoreOverlap(relationTermLocation, video, relation1TermLocationList, requireTerm);
                        termPoint += TermLocationService.NoneOverlapPart(relationTermLocation, video, termName, currentPosition);
                        termPoint += TermLocationService.OverlapCaseType(relationTermLocation, video, termName);
                        if (termPoint == 0)
                        {
                            overlap = true;
                            relationOverlapListTermLocationList.Add(relationTermLocation);
                        }
                        else if (termPoint < 0)
                        {
                            relationRemoveListTermLocationList.Add(relationTermLocation);

                        }
                        else
                        {
                            //Thuật ngữ này không hợp lệ
                            validate = false;
                            break;
                        }
                        //var refIsCorrect = relationTermLocation.CheckRealNameIsUpperCaseFirstAll(null);
                        //if (!refIsCorrect && video.CheckSpelling && video.GetDictionary() != null &&
                        //        relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name))
                        //    refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                        //if (currentIsCorrect == refIsCorrect)
                        //{
                        //    overlap = true;
                        //    relationOverlapListTermLocationList.Add(relationTermLocation);
                        //}
                        //else if (currentIsCorrect)
                        //{
                        //    relationRemoveListTermLocationList.Add(relationTermLocation);

                        //}
                        //else if (refIsCorrect)
                        //{
                        //    //Thuật ngữ này không hợp lệ
                        //    validate = false;
                        //    break;
                        //}
                    }
                }

                if (validate)
                {
                    foreach (var relationTermLocation in relationOverlapListTermLocationList)
                    {
                        relationTermLocation.Overlap = true;
                        if (relationTermLocation.Term != null && !relationTermLocation.Term.Overlap)
                            relationTermLocation.Term.Overlap = relationTermLocation.Overlap;
                    }
                    foreach (var relationTermLocation in relationRemoveListTermLocationList)
                    {
                        //Loại bỏ thuật ngữ tham chiếu
                        System.Collections.Generic.List<TermLocation> overlapList = null;
                        if (relationTermLocation.Overlap)
                        {
                            //Lấy danh sách overlap liên quan trước khi xóa
                            overlapList = TermLocationService.GetOverlap(relationTermLocation, requireTerm);
                        }
                        if (relationTermLocation.Term.Quantity == 1)
                        {
                            relationTermLocation.Term.Delete();
                        }
                        else
                        {
                            relationTermLocation.Term.Quantity = relationTermLocation.Term.TermLocationList.Count - 1;
                        }

                        relationTermLocation.Delete();
                        if (overlapList != null)
                        {
                            foreach (var checkTl in overlapList)
                            {
                                checkTl.Overlap = TermLocationService.CheckOverlap(checkTl, requireTerm);
                                if (!checkTl.Overlap && checkTl.Term != null)
                                {
                                    checkTl.Term.Overlap = checkTl.Term.GetDefaultOverlap();
                                }

                            }
                        }
                    }
                }
                #region Cấu trúc trước Khi đánh giá bên thua: Từ hoa tương đương Đúng chính tả 

                //if (CheckSpelling && video.GetDictionary() != null)
                //{
                //    var relationOverlapListTermLocationList = new System.Collections.Generic.List<TermLocation>();
                //    var relationRemoveListTermLocationList = new System.Collections.Generic.List<TermLocation>();
                //    //Thuật ngữ cần kiểm tra nằm trong các thuật ngữ này
                //    if (validate && belongTermLocationList.Count > 0)
                //    {
                //        foreach (var relationTermLocation in belongTermLocationList)
                //        {
                //            //var element = relationTermLocation.GetAudioFromElement().Content;
                //            //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                //            //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();                            
                //            var refIsCorrect = relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name);
                //            if (refIsCorrect)
                //                refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                //            if (refIsCorrect)
                //            {
                //                //Thuật ngữ này không hợp lệ
                //                validate = false;
                //                break;
                //            }
                //            else
                //            {
                //                relationRemoveListTermLocationList.Add(relationTermLocation);
                //            }
                //        }
                //    }
                //    //Nếu thuật ngữ này đúng chính tả thì so sánh tiếp, nếu sai chính tả thì coi như là không hợp lệ
                //    //Thuật ngữ so sánh thuộc về thuật ngữ trong vòng for này
                //    if (validate && containTermLocationList.Count > 0)
                //    {
                //        foreach (var relationTermLocation in containTermLocationList)
                //        {
                //            //var element = relationTermLocation.GetAudioFromElement().Content;
                //            //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                //            //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();                            
                //            var refIsCorrect = relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name);
                //            if (refIsCorrect)
                //                refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                //            if (refIsCorrect)
                //            {
                //                validate = false;
                //                break;
                //            }
                //            else
                //            {
                //                relationRemoveListTermLocationList.Add(relationTermLocation);
                //            }
                //        }
                //    }


                //    if (validate && relation1TermLocationList.Count > 0)
                //    {
                //        foreach (var relationTermLocation in relation1TermLocationList)
                //        {
                //            //var element = relationTermLocation.GetAudioFromElement().Content;
                //            //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                //            //var lowerName = Module.Helpers.TextHelper.RemoveUnicode(termName).ToLower();                            
                //            var refIsCorrect = relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name);
                //            if (refIsCorrect)
                //                refIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), relationTermLocation.Term.Name);
                //            if (currentIsCorrect == refIsCorrect)
                //            {
                //                overlap = true;
                //                relationOverlapListTermLocationList.Add(relationTermLocation);
                //            }
                //            else if (currentIsCorrect)
                //            {
                //                relationRemoveListTermLocationList.Add(relationTermLocation);

                //            }
                //            else if (refIsCorrect)
                //            {
                //                //Thuật ngữ này không hợp lệ
                //                validate = false;
                //                break;
                //            }
                //        }
                //    }

                //    if (validate)
                //    {
                //        foreach (var relationTermLocation in relationOverlapListTermLocationList)
                //        {
                //            relationTermLocation.Overlap = true;
                //        }
                //        foreach (var relationTermLocation in relationRemoveListTermLocationList)
                //        {
                //            //Loại bỏ thuật ngữ tham chiếu                                                
                //            if (relationTermLocation.Term.Quantity == 1)
                //            {
                //                relationTermLocation.Term.Delete();
                //            }
                //            else
                //            {
                //                relationTermLocation.Term.Quantity = relationTermLocation.Term.TermLocationList.Count - 1;
                //            }
                //            relationTermLocation.Delete();
                //        }
                //    }

                //}
                //else if (relation1TermLocationList.Count > 0)
                //{
                //    //dựng cờ overlap(để xem xét)
                //    overlap = true;
                //    foreach (var relationTermLocation in relation1TermLocationList)
                //    {
                //        relationTermLocation.Overlap = true;
                //    }
                //}
                #endregion
                if (validate && relation2TermLocationList.Count > 0)
                {

                    foreach (var relationTermLocation in relation2TermLocationList)
                    {
                        //var m = termName.Equals(relationTermLocation.Term.Name);
                        //var element = relationTermLocation.GetAudioFromElement().Content;
                        //Thuật ngữ tham chiếu thuộc về thuật ngữ này
                        if (relationTermLocation.Term.Quantity == 1)
                        {
                            relationTermLocation.Term.Delete();
                        }
                        else
                        {
                            relationTermLocation.Term.Quantity = relationTermLocation.Term.TermLocationList.Count - 1;
                        }
                        relationTermLocation.Delete();
                    }
                }
            }

            return validate;
        }



        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> translateDictionary;
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> originDictionary;

        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> GetDictionarySpelling(Video video, bool useLanguageTranslate)
        {
            if (useLanguageTranslate == false)
            {
                if (originDictionary == null)
                {
                    if (!LoadDictionaryFromDatabase(video, useLanguageTranslate) && !TryLoadDictionaryFromFile(video, useLanguageTranslate))
                    {
                        return null;
                    }
                }
                return originDictionary;
            }
            else
            {
                if (translateDictionary == null)
                {
                    if (!LoadDictionaryFromDatabase(video, useLanguageTranslate) && !TryLoadDictionaryFromFile(video, useLanguageTranslate))
                    {
                        return null;
                    }
                }
                return translateDictionary;
            }
        }


        private bool LoadDictionaryFromDatabase(Video video, bool useLanguageTranslate)
        {
            var language = useLanguageTranslate ? video.LanguageTranslate : video.LanguageOrigin;
            if (language == null) return false;

            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", language.Oid);
            var words = new XPCollection<Word>(video.Session, criteria);
            if (words.Count == 0) return false;
            if (useLanguageTranslate)
            {
                translateDictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>>();

                foreach (var word in words)
                {
                    string wordName = word.Name;

                    if (string.IsNullOrEmpty(wordName)) continue; // Bỏ qua từ nếu Name không hợp lệ

                    int wordLength = wordName.Split(' ').Length;

                    if (!translateDictionary.TryGetValue(wordLength, out var wordGroup))
                    {
                        wordGroup = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>();
                        translateDictionary[wordLength] = wordGroup;
                    }

                    if (!wordGroup.TryGetValue(wordName, out var nameSet))
                    {
                        nameSet = new System.Collections.Generic.HashSet<string>();
                        wordGroup[wordName] = nameSet;
                    }

                    nameSet.Add(word.Name);
                }
                return true;
            }
            else
            {
                originDictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>>();

                foreach (var word in words)
                {
                    string wordName = word.Name;

                    if (string.IsNullOrEmpty(wordName)) continue; // Bỏ qua từ nếu Name không hợp lệ

                    int wordLength = wordName.Split(' ').Length;

                    if (!originDictionary.TryGetValue(wordLength, out var wordGroup))
                    {
                        wordGroup = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>();
                        originDictionary[wordLength] = wordGroup;
                    }

                    if (!wordGroup.TryGetValue(wordName, out var nameSet))
                    {
                        nameSet = new System.Collections.Generic.HashSet<string>();
                        wordGroup[wordName] = nameSet;
                    }

                    nameSet.Add(word.Name);
                }
                return true;
            }
        }

        private bool TryLoadDictionaryFromFile(Video video, bool useLanguageTranslate)
        {
            var language = useLanguageTranslate ? video.LanguageTranslate : video.LanguageOrigin;
            if (language == null)
            {
                _notificationService.NotifyError("Lỗi", useLanguageTranslate
                    ? "Ngữ dịch không được phép trống (Language Translate không hợp lệ)"
                    : "Ngữ dịch không được phép trống (Language Origin không hợp lệ)");
                return false;
            }

            string folder = GetValueOrDefault("CheckDictionaryFolder", "\\\\rd\\CodeGen\\packages\\Dictionary");
            if (string.IsNullOrEmpty(folder))
            {
                _notificationService.NotifyError("Lỗi", "Không tìm thấy thư mục chứa từ điển, vui lòng kiểm tra lại tham số");
                return false;
            }

            string fileName = System.IO.Path.Combine(folder.TrimEnd('\\'), language.Code + "Compounds.txt");
            if (!System.IO.File.Exists(fileName))
            {
                _notificationService.NotifyError("Lỗi", "Không tìm thấy từ điển từ ghép cho ngôn ngữ " + language.Code + ", vui lòng kiểm tra lại");
                return false;
            }

            var wordsText = System.IO.File.ReadAllText(fileName);
            if (useLanguageTranslate)
            {
                translateDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>>>(wordsText);
                return true;
            }
            else
            {
                originDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>>>(wordsText);
                return true;
            }
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> nosignDictionary;

        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> GetNoSignDictionary(Video video, bool useLanguageTranslate)
        {
            if (useLanguageTranslate == false)
            {
                if (nosignDictionary == null)
                {
                    if (!LoadNoSignDictionaryFromDatabase(video, useLanguageTranslate) && !TryLoadDictionaryFromFile(video, useLanguageTranslate))
                    {
                        return null;
                    }
                }
                return nosignDictionary;
            }
            else
            {
                if (nosignDictionary == null)
                {
                    if (!LoadNoSignDictionaryFromDatabase(video, useLanguageTranslate) && !TryLoadDictionaryFromFile(video, useLanguageTranslate))
                    {
                        return null;
                    }
                }
                return nosignDictionary;
            }
        }

        private bool LoadNoSignDictionaryFromDatabase(Video video, bool useLanguageTranslate)
        {
            var language = useLanguageTranslate ? video.LanguageTranslate : video.LanguageOrigin;
            if (language == null) return false;

            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", language.Oid);
            var words = new XPCollection<Word>(video.Session, criteria);
            if (words.Count == 0) return false;

            nosignDictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>>();

            foreach (var word in words)
            {
                string noSignWord = word.NoSignWord; // Sử dụng NoSignWord thay vì Name

                if (string.IsNullOrEmpty(noSignWord)) continue; // Bỏ qua từ nếu NoSignWord không hợp lệ

                int wordLength = noSignWord.Split(' ').Length;

                if (!nosignDictionary.TryGetValue(wordLength, out var wordGroup))
                {
                    wordGroup = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>();
                    nosignDictionary[wordLength] = wordGroup;
                }

                if (!wordGroup.TryGetValue(noSignWord, out var nameSet))
                {
                    nameSet = new System.Collections.Generic.HashSet<string>();
                    wordGroup[noSignWord] = nameSet;
                }

                nameSet.Add(word.NoSignWord); // Thêm từ không dấu vào danh sách
            }

            return true;
        }


        public void SetNodeText(HtmlAgilityPack.HtmlNode htmlNode, string content, string source = null)
        {
            if (content is null)
                content = "";
            if (htmlNode is HtmlAgilityPack.HtmlTextNode)
            {
                var innerText = htmlNode.InnerText.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("<br>", "\r\n").Replace("<br/>", "\r\n");
                innerText = System.Web.HttpUtility.HtmlDecode(innerText).Replace("\n", "\r\n");
                if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(htmlNode.InnerText) && htmlNode.InnerText != source &&
                    (htmlNode.InnerText.Trim() == source || innerText.Trim() == source))
                {
                    //Hỗ trợ xuất dữ liệu không mất khoảng trắng
                    if (htmlNode.InnerText.Trim() == source)
                        ((HtmlAgilityPack.HtmlTextNode)htmlNode).Text = htmlNode.InnerText.Replace(source, content);
                    else
                    {
                        ((HtmlAgilityPack.HtmlTextNode)htmlNode).Text = innerText.Replace(source, content);
                    }
                }
                else
                {
                    ((HtmlAgilityPack.HtmlTextNode)htmlNode).Text = content;
                }
            }

            else if (htmlNode.ChildNodes.Count == 1)
            {
                SetNodeText(htmlNode.ChildNodes[0], content, source);
            }
            else
            {
                //if (htmlNode.InnerHtml.Contains(htmlNode.InnerText))
                //{
                //    htmlNode.InnerHtml.Replace(htmlNode.InnerText, content);
                //}
                //Xóa các node thừa
                var childNodes = htmlNode.ChildNodes.ToList();
                bool replaced = false;
                foreach (HtmlAgilityPack.HtmlNode node in childNodes)
                {
                    if (node is HtmlAgilityPack.HtmlTextNode)
                    {
                        if (((HtmlAgilityPack.HtmlTextNode)node).InnerText == htmlNode.InnerText || ((HtmlAgilityPack.HtmlTextNode)node).InnerText?.Trim() == htmlNode.InnerText?.Trim())
                        {
                            replaced = true;
                            SetNodeText(node, content, source);
                            break;
                        }

                    }
                    //var nodeType = node.GetType();
                }
                if (!replaced)
                {
                    //Kiểm tra xem có node a hoặc node img không?
                    var aNodes = htmlNode.Descendants("a");
                    var imgNodes = htmlNode.Descendants("img");
                    if (aNodes?.Count() > 0 || imgNodes?.Count() > 0)
                    {
                        //Fix lỗi mất link hoặc mất ảnh
                        if (aNodes?.Count() > 0)
                        {
                            foreach (var aNode in aNodes)
                            {
                                if (aNode.InnerText == htmlNode.InnerText || (aNode.InnerText?.Trim() == htmlNode.InnerText?.Trim()))
                                {
                                    replaced = true;
                                    aNode.InnerHtml = content;
                                    break;
                                }
                            }
                        }
                        if (!replaced)
                        {
                            //Trường hợp không fix được
                            //var innerHtmlLines = htmlNode.InnerHtml.Split("<br>", System.StringSplitOptions.RemoveEmptyEntries);
                            //var contentHtmlLines = content.Replace("").Split("<br>", System.StringSplitOptions.RemoveEmptyEntries);
                            //if(innerHtmlLines.Length == contentHtmlLines.Length)
                            //{

                            //}
                        }
                    }
                }
                if (!replaced)
                {
                    if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(htmlNode.InnerHtml) && htmlNode.InnerHtml != source && htmlNode.InnerHtml.Trim() == source)
                    {
                        //Hỗ trợ xuất dữ liệu không mất khoảng trắng
                        htmlNode.InnerHtml = htmlNode.InnerHtml.Replace(source, content);
                    }
                    else if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(htmlNode.InnerText) && htmlNode.InnerText != source && htmlNode.InnerText.Trim() == source)
                    {
                        //Hỗ trợ xuất dữ liệu không mất khoảng trắng
                        if (htmlNode is HtmlAgilityPack.HtmlTextNode)
                            ((HtmlAgilityPack.HtmlTextNode)htmlNode).Text = htmlNode.InnerText.Replace(source, content);
                        else htmlNode.InnerHtml = htmlNode.InnerText.Replace(source, content);
                    }
                    else
                    {
                        var innerText = htmlNode.InnerText.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("<br>", "\r\n").Replace("<br/>", "\r\n");
                        innerText = System.Web.HttpUtility.HtmlDecode(innerText).Replace("\n", "\r\n");
                        if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(htmlNode.InnerText) && htmlNode.InnerText != source && innerText.Trim() == source)
                        {
                            htmlNode.InnerHtml = innerText.Replace(source, content);
                        }
                        else
                        {
                            htmlNode.InnerHtml = content;
                        }

                    }

                    replaced = true;
                }
                if (!replaced)
                {
                    foreach (HtmlAgilityPack.HtmlNode node in childNodes)
                    {
                        if (!replaced && node is HtmlAgilityPack.HtmlTextNode)
                        {
                            ((HtmlAgilityPack.HtmlTextNode)node).Text = content;
                            replaced = true;
                        }
                        else if (node.ChildNodes.Count == 0 || string.IsNullOrEmpty(node.InnerText) || string.IsNullOrEmpty(node.InnerText.Trim()))
                            htmlNode.RemoveChild(node);
                    }
                }

                if (!replaced)
                {
                    //Test
                    //var nodeType = htmlNode.GetType();
                }
            }
        }
        string[] tabNames = new string[] { "p", "span", "font", "b", "i", "u", "caption", "h1", "h2", "h3", "h4", "h5", "h6", "h7", "#text", "cite", "code", "data", "option", "embed", "label", "pre", "small", "source", "time", "textarea", "tt", "var" };
        string[] wrapperTabName = new string[] { "p", "b", "u", "font" };
        //string testText = "Quad channel HD";
        private string debugText = "Wowza, YouTube, Twitch";

        public void FillContentFromHtmlNode(Video video, ref int index, HtmlAgilityPack.HtmlNode htmlNode, TranslateObject translateObject = null, BookMark bookMark = null, bool? translate = null)
        {
            if (htmlNode is null || htmlNode.Name.Equals("script"))
                return;
            var keepAllStyle = video.OriginStyleExport || video.NodeFontColor || video.NodeFontBold || video.NodeFontItalic || video.NodeFontUnderline || video.NodeLink;
            if (htmlNode.ChildNodes.Count > 0 &&
                (htmlNode.Descendants("table").Count() > 0 ||
                (video.NodeFontColor && (htmlNode.Descendants("color").Count() > 0 || htmlNode.SelectSingleNode("//font[string(@color)]") != null) || htmlNode.SelectSingleNode("//*[contains(@style, 'color')]") != null) ||
                (video.NodeFontBold && (htmlNode.Descendants("b").Count() > 0 || htmlNode.SelectSingleNode("//*[contains(@style, 'font-weight')]") != null)) ||
                (video.NodeFontItalic && (htmlNode.Descendants("i").Count() > 0 || htmlNode.SelectSingleNode("//*[contains(@style, 'italic')]") != null)) ||
                (video.NodeFontUnderline && (htmlNode.Descendants("u").Count() > 0 || htmlNode.SelectSingleNode("//*[contains(@style, 'underline')]") != null)) ||
                (video.NodeLink && (htmlNode.Descendants("a").Count() > 0)) ||
                (!tabNames.Contains(htmlNode.Name) &&
                (htmlNode.FirstChild?.Name != "#text" || string.IsNullOrEmpty(htmlNode.FirstChild?.InnerText?.Trim())) &&
                (htmlNode.LastChild?.Name != "#text" || string.IsNullOrEmpty(htmlNode.LastChild?.InnerText?.Trim())))))
            {
                // && htmlNode.FirstChild != null && !string.IsNullOrEmpty(htmlNode.FirstChild.InnerText)
                foreach (var child in htmlNode.ChildNodes)
                {
                    FillContentFromHtmlNode(video, ref index, child, translateObject, bookMark, translate);
                }
            }
            else
            {
                //Trường hợp Node P có ảnh hoặc link bên trong thì chia nhỏ nội dung nữa
                //Dựng cờ thì kiểm tra có liên kết trong nội dung của node
                if (translateObject != null && translateObject.Flag && wrapperTabName.Contains(htmlNode.Name) && (htmlNode.Descendants("a")?.Count() > 0 || htmlNode.Descendants("img")?.Count() > 0))
                {
                    foreach (var child in htmlNode.ChildNodes)
                    {
                        FillContentFromHtmlNode(video, ref index, child, translateObject, bookMark, translate);
                    }
                }
                else if (bookMark != null && bookMark.Flag && wrapperTabName.Contains(htmlNode.Name) && (htmlNode.Descendants("a")?.Count() > 0 || htmlNode.Descendants("img")?.Count() > 0))
                {
                    foreach (var child in htmlNode.ChildNodes)
                    {
                        FillContentFromHtmlNode(video, ref index, child, translateObject, bookMark, translate);
                    }
                }
                else
                {

                    var rootInnerHtml = htmlNode.InnerHtml;
                    //Bỏ dữ liệu thừa
                    htmlNode.InnerHtml = htmlNode.InnerHtml.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("<br>", "\r\n").Replace("<br/>", "\r\n");
                    var innerText = htmlNode.InnerText;
                    if (string.IsNullOrEmpty(innerText))
                        return;
                    innerText = innerText.Trim();
                    if (string.IsNullOrEmpty(innerText))
                        return;
                    var content = System.Web.HttpUtility.HtmlDecode(htmlNode.InnerText).Replace("\n", "\r\n");

                    var trimContent = content.Trim();
                    if (string.IsNullOrEmpty(trimContent))
                        return;
                    //Chỉ nạp nội dung khi có số hoặc ký tự hoặc text lơn hơn 1 ký tự
                    //if (trimContent.Length == 1 && !char.IsLetterOrDigit(trimContent[0]))
                    //    return;

                    //if (!keepAllStyle)
                    content = trimContent;
                    //Độ dài quá ngắn, không có ý nghĩa dịch
                    if (trimContent.Length == 1)
                        return;
                    if (content.Contains(debugText))
                    {

                    }
                    //var classElement = htmlNode.GetAttributeValue("class", null);
                    //var styleElement = htmlNode.GetAttributeValue("style", null);
                    //if (styleElement != null)
                    //{

                    //}
                    //Nạp dữ liệu vào thành phần
                    //index++;
                    if (translate is null)
                    {
                        //index++;
                        var audio = CreateObject<Audio>();
                        audio.Video = video;
                        video.AudioList.Add(audio);
                        if (content.Length >= 2000)
                        {
                            audio.Note = "/Nội dung quá dài/";
                        }
                        if (!video.KeepSpace)
                            content = content.Trim();
                        audio.Content = content;
                        //audio.Order = index;
                        audio.Start = System.TimeSpan.FromSeconds(index);
                        if (translateObject != null)
                            audio.TranslateObject = translateObject;
                        else if (bookMark != null)
                            audio.BookMark = bookMark;
                    }
                    else
                    {
                        var startTime = System.TimeSpan.FromSeconds(index);
                        Audio audio = null;
                        foreach (var element in video.AudioList)
                        {
                            if (element.TranslateObject != null && translateObject != null && !translateObject.Equals(element.TranslateObject))
                                continue;
                            if (element.BookMark != null && bookMark != null && !bookMark.Equals(element.BookMark))
                                continue;
                            if (element.Start == startTime)
                            {
                                audio = element;
                                break;
                            }
                        }
                        if (audio != null)
                        {
                            if (translateObject != null && audio.TranslateObject != null && !translateObject.Oid.Equals(audio.TranslateObject.Oid))
                                return;
                            if (bookMark != null && audio.BookMark != null && !bookMark.Oid.Equals(audio.BookMark.Oid))
                                return;
                            //htmlNode.InnerText = translate ? audio.Subtitle : audio.Content;
                            var newContent = translate.Value ? audio.Subtitle : audio.Content;
                            if (string.IsNullOrEmpty(newContent))
                            {
                                //Test
                            }
                            if (!content.Equals(newContent))
                            {
                                if (htmlNode.InnerHtml != rootInnerHtml)
                                    htmlNode.InnerHtml = rootInnerHtml;
                                if (!string.IsNullOrEmpty(newContent))
                                    newContent = newContent.Replace("\r\n", "<br>");
                                if (newContent is null)
                                    newContent = "";
                                //if (htmlNode.InnerHtml.Contains(testText))
                                //{

                                //}
                                SetNodeText(htmlNode, newContent, audio.Content);
                            }

                        }
                        else
                        {
                            //Test 
                        }
                        //index++;
                    }
                    index++;

                }

            }
        }





        #endregion SourceCode4544ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
        //    return result;
        //}
		
		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DocumentTypeToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (DocumentType != null) 
		//			return DocumentType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DateToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Date != null) 
		//			return Date;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageOriginToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (LanguageOrigin != null) 
		//			return LanguageOrigin;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageTranslateToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (LanguageTranslate != null) 
		//			return LanguageTranslate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PathToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Path != null) 
		//			return Path;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object StatusToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Status != null) 
		//			return Status;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (AudioList != null) 
		//			return AudioList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementBatchListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ElementBatchList != null) 
		//			return ElementBatchList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TermListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (TermList != null) 
		//			return TermList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TermLocationListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (TermLocationList != null) 
		//			return TermLocationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (MediaList != null) 
		//			return MediaList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ParagraphList != null) 
		//			return ParagraphList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphStyleListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ParagraphStyleList != null) 
		//			return ParagraphStyleList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (LanguageList != null) 
		//			return LanguageList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateObjectListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (TranslateObjectList != null) 
		//			return TranslateObjectList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FileListToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FileList != null) 
		//			return FileList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FontColorToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FontColor != null) 
		//			return FontColor;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FontBoldToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FontBold != null) 
		//			return FontBold;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FontItalicToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FontItalic != null) 
		//			return FontItalic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FontUnderlineToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FontUnderline != null) 
		//			return FontUnderline;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OutlineToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Outline != null) 
		//			return Outline;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Alignment != null) 
		//			return Alignment;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpacingToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Spacing != null) 
		//			return Spacing;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IndentToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Indent != null) 
		//			return Indent;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeFontColorToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeFontColor != null) 
		//			return NodeFontColor;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeFontBoldToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeFontBold != null) 
		//			return NodeFontBold;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeFontItalicToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeFontItalic != null) 
		//			return NodeFontItalic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeFontUnderlineToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeFontUnderline != null) 
		//			return NodeFontUnderline;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeLinkToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeLink != null) 
		//			return NodeLink;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NodeSuperToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (NodeSuper != null) 
		//			return NodeSuper;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperElementImportToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (UpperElementImport != null) 
		//			return UpperElementImport;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NumberToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Number != null) 
		//			return Number;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CheckSpellingToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (CheckSpelling != null) 
		//			return CheckSpelling;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WithTermPositionToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (WithTermPosition != null) 
		//			return WithTermPosition;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementSpacingToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ElementSpacing != null) 
		//			return ElementSpacing;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BlankSpacingToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (BlankSpacing != null) 
		//			return BlankSpacing;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeUniqueToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (CodeUnique != null) 
		//			return CodeUnique;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpcaseNumberingToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (UpcaseNumbering != null) 
		//			return UpcaseNumbering;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AbbyyTermLocationToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (AbbyyTermLocation != null) 
		//			return AbbyyTermLocation;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object KeepSpaceToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (KeepSpace != null) 
		//			return KeepSpace;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImportByNodeToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ImportByNode != null) 
		//			return ImportByNode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImportParagraphToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (ImportParagraph != null) 
		//			return ImportParagraph;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RightIndentToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (RightIndent != null) 
		//			return RightIndent;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OriginStyleExportToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (OriginStyleExport != null) 
		//			return OriginStyleExport;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreateWordStyleToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (CreateWordStyle != null) 
		//			return CreateWordStyle;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OpenToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Open != null) 
		//			return Open;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FolderToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (Folder != null) 
		//			return Folder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BrLineToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (BrLine != null) 
		//			return BrLine;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IsPhotoToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (IsPhoto != null) 
		//			return IsPhoto;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextObjectGroupToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (TextObjectGroup != null) 
		//			return TextObjectGroup;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FootNoteToolTipControllerText(View view, Module.BusinessObjects.Video video)
        //{
        //    if (FootNote != null) 
		//			return FootNote;
        //    return null;
        //}
    

	    #endregion
  

    }
}
