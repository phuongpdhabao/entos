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

    public partial class MediaService : BaseService
    {

        public MediaService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public MediaService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4508ImportCode
        
        public void QuantityMedia(List<Media> mediaes, string actionId, Func<Media, List<Media>> getRelatedMedias)
        {
            foreach (Media media in mediaes)
            {
                if (media == null) continue;

                if (media.Quantity != null)
                    media.Quantity = null;
                ProcessQuantity(media, actionId, getRelatedMedias);
            }
        }
        public void ProcessQuantity(
                Media media,
                string actionId,
                Func<Media, List<Media>> getRelatedMedias)
        {
            if (media == null)
                return;

            media.Quantity = null;

            switch (actionId)
            {
                case "TextWord":
                    CalculateTextWord(media);
                    break;

                case "SameGroup":
                    CalculateSameGroup(media, getRelatedMedias);
                    break;

                case "ChildElement":
                    CalculateChildElement(media, getRelatedMedias);
                    break;

                case "ChildTextbox":
                    CalculateChildTextbox(media, getRelatedMedias);
                    break;
            }

        }
        
        #endregion SourceCode4508ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Media media)
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
		//public object StartToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Start != null) 
		//			return Start;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (End != null) 
		//			return End;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaFileToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MediaFile != null) 
		//			return MediaFile;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaStartToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MediaStart != null) 
		//			return MediaStart;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaDurationToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MediaDuration != null) 
		//			return MediaDuration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaSpeedToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MediaSpeed != null) 
		//			return MediaSpeed;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PhotoToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Photo != null) 
		//			return Photo;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Paragraph != null) 
		//			return Paragraph;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioListToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (AudioList != null) 
		//			return AudioList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Video != null) 
		//			return Video;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DurationToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Duration != null) 
		//			return Duration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioDurationToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (AudioDuration != null) 
		//			return AudioDuration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Text != null) 
		//			return Text;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextPreviousToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (TextPrevious != null) 
		//			return TextPrevious;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextNextToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (TextNext != null) 
		//			return TextNext;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParagraphStyleToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (ParagraphStyle != null) 
		//			return ParagraphStyle;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MediaTypeToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MediaType != null) 
		//			return MediaType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BookMarkToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (BookMark != null) 
		//			return BookMark;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ShapeTypeTextToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (ShapeTypeText != null) 
		//			return ShapeTypeText;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object HeightToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Height != null) 
		//			return Height;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WidthToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Width != null) 
		//			return Width;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextWrappingTypeToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (TextWrappingType != null) 
		//			return TextWrappingType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AllowOverlapToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (AllowOverlap != null) 
		//			return AllowOverlap;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Alignment != null) 
		//			return Alignment;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentRelativeToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (AlignmentRelative != null) 
		//			return AlignmentRelative;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MoveWithTextToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (MoveWithText != null) 
		//			return MoveWithText;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperMediaToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (UpperMedia != null) 
		//			return UpperMedia;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TextWrappingTypeNewToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (TextWrappingTypeNew != null) 
		//			return TextWrappingTypeNew;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AlignmentNewToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (AlignmentNew != null) 
		//			return AlignmentNew;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ShapeIdToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (ShapeId != null) 
		//			return ShapeId;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ShapeNameToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (ShapeName != null) 
		//			return ShapeName;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TopToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Top != null) 
		//			return Top;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PageNumberToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (PageNumber != null) 
		//			return PageNumber;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ResizeWithTextToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (ResizeWithText != null) 
		//			return ResizeWithText;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FillColorToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (FillColor != null) 
		//			return FillColor;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LineColorToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (LineColor != null) 
		//			return LineColor;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FillCodeToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (FillCode != null) 
		//			return FillCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LineCodeToolTipControllerText(View view, Module.BusinessObjects.Media media)
        //{
        //    if (LineCode != null) 
		//			return LineCode;
        //    return null;
        //}
    

	    #endregion
  

    }
}
