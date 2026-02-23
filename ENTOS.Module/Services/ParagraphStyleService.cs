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

    public partial class ParagraphStyleService : BaseService
    {

        public ParagraphStyleService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public ParagraphStyleService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4537ImportCode
                internal void AdjustName(System.Collections.IList displayObject)
{
            //if (gridListEditor != null && gridListEditor.GridView != null)
            //{
                //Tạo list paragraphStyles để tránh trường hợp bị sort lại khi đang sửa
                var paragraphStyles = new List<ParagraphStyle>();
                //for (int i = 0; i < gridListEditor.GridView.RowCount; i++)
                //{
                //    var paragraphStyle = gridListEditor.GridView.GetRow(i) as ParagraphStyle;                    
                //}
                foreach (ParagraphStyle paragraphStyle in displayObject)
                {
                    if (paragraphStyle != null)
                        paragraphStyles.Add(paragraphStyle);
                }
                for (int i = 0; i < paragraphStyles.Count; i++)
                {
                    string styleName = BuildStyleName(i);
                    paragraphStyles[i].Name = styleName;
                }
            //}
}


        #endregion SourceCode4537ImportCode

        #region SourceCode4536ImportCode
                internal void AssignFont(IEnumerable<ParagraphStyle> selectedObjects, Video video)
{
            //2023-6-22: Tìm Font có size lớn hơn gần nhất
            string message = "";
            var paragraphStyleList = video.ParagraphStyleList.Where(m => m.Font != "inherit" && m.Size != null).OrderBy(m => m.Size).ToList();
            foreach(ParagraphStyle paragraphStyle in selectedObjects)
            {
                var replaceParagraphStyleList = paragraphStyleList.Where(m => m.Size >= paragraphStyle.Size && !m.Oid.Equals(paragraphStyle.Oid)).OrderBy(m => m.Size).ToList();
                if (replaceParagraphStyleList.Count == 0)
                    replaceParagraphStyleList = paragraphStyleList.Where(m => m.Size < paragraphStyle.Size && !m.Oid.Equals(paragraphStyle.Oid)).OrderByDescending(m => m.Size).ToList();
                if (replaceParagraphStyleList.Count != 0)
                {
                    //Tìm style trùng
                    foreach (var replaceParagraphStyle in replaceParagraphStyleList)
                    {
                        if (paragraphStyle.Size == replaceParagraphStyle.Size &&
                                paragraphStyle.Color == replaceParagraphStyle.Color &&
                                    paragraphStyle.Bold == replaceParagraphStyle.Bold &&
                                        paragraphStyle.Italic == replaceParagraphStyle.Italic &&
                                            paragraphStyle.Underline == replaceParagraphStyle.Underline)
                        {
                            //Nếu có style giống thì thay thế style này
                            foreach (var audio in video.AudioList)
                            {
                                if (audio.ParagraphStyle != null && paragraphStyle.Oid == audio.ParagraphStyle.Oid)
                                {
                                    audio.ParagraphStyle = replaceParagraphStyle;
                                }
                            }
                            message = AppendReplacementMessage(message, paragraphStyle.Name, replaceParagraphStyle.Name);
                            paragraphStyle.Delete();                          
                            break;
                        }
                    }
                    if (!paragraphStyle.IsDeleted)
                    {
                        paragraphStyle.Font = replaceParagraphStyleList[0].Font;
                    }
                }
            }
            if(!string.IsNullOrEmpty(message))
            {
                _notificationService.Notify("Thông báo", message, InformationType.Info, 10000);
            }
}


        #endregion SourceCode4536ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FontToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateFontToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (TranslateFont != null) 
		//			return TranslateFont;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SizeToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ColorToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Color != null) 
		//			return Color;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BoldToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Bold != null) 
		//			return Bold;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ItalicToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Italic != null) 
		//			return Italic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UnderlineToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Underline != null) 
		//			return Underline;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OutlineToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Outline != null) 
		//			return Outline;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Alignment != null) 
		//			return Alignment;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpacingBeforeToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (SpacingBefore != null) 
		//			return SpacingBefore;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpacingAfterToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (SpacingAfter != null) 
		//			return SpacingAfter;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpacingLineToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (SpacingLine != null) 
		//			return SpacingLine;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpacingLineAtToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (SpacingLineAt != null) 
		//			return SpacingLineAt;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IndentLeftToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (IndentLeft != null) 
		//			return IndentLeft;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IndentRightToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (IndentRight != null) 
		//			return IndentRight;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IndentFirstLineToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (IndentFirstLine != null) 
		//			return IndentFirstLine;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperStyleToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (UpperStyle != null) 
		//			return UpperStyle;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Video != null) 
		//			return Video;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementQuantityToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (ElementQuantity != null) 
		//			return ElementQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinkToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Link != null) 
		//			return Link;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object HeightToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Height != null) 
		//			return Height;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WidthToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (Width != null) 
		//			return Width;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectLayoutToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (ObjectLayout != null) 
		//			return ObjectLayout;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentRelativeToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (AlignmentRelative != null) 
		//			return AlignmentRelative;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MoveWithTextToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (MoveWithText != null) 
		//			return MoveWithText;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextWrappingTypeToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (TextWrappingType != null) 
		//			return TextWrappingType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphStyleTypeToolTipControllerText(View view, Module.BusinessObjects.ParagraphStyle paragraphstyle)
        //{
        //    if (ParagraphStyleType != null) 
		//			return ParagraphStyleType;
        //    return null;
        //}
    

	    #endregion
  

    }
}
