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

    public partial class RecognitionPositionService : BaseService
    {

        public RecognitionPositionService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public RecognitionPositionService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4522ImportCode
        
        internal static void DrawBox(OpenCvSharp.Mat img, int x, int y, int size)
        {
            OpenCvSharp.Cv2.Rectangle(img, new OpenCvSharp.Rect(x, y, size, size), OpenCvSharp.Scalar.Red, 2);
        }

        internal static  bool IsImage(string path)
        {
            string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".tiff";
        }

        internal static bool IsVideo(string path)
        {
            string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            return ext == ".mp4" || ext == ".avi" || ext == ".mov" || ext == ".mkv" || ext == ".wmv";
		}
        #endregion SourceCode4522ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
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
		//public object LinkToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Link != null) 
		//			return Link;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VerticalToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Vertical != null) 
		//			return Vertical;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object HorizontalToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Horizontal != null) 
		//			return Horizontal;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ReliabilityToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Reliability != null) 
		//			return Reliability;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SizeToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Size != null) 
		//			return Size;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Image != null) 
		//			return Image;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BeginToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Begin != null) 
		//			return Begin;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (End != null) 
		//			return End;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BeginFrameToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (BeginFrame != null) 
		//			return BeginFrame;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndFrameToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (EndFrame != null) 
		//			return EndFrame;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionObjectToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (RecognitionObject != null) 
		//			return RecognitionObject;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageFrameToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (ImageFrame != null) 
		//			return ImageFrame;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object YawToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Yaw != null) 
		//			return Yaw;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RollToolTipControllerText(View view, Module.BusinessObjects.RecognitionPosition recognitionposition)
        //{
        //    if (Roll != null) 
		//			return Roll;
        //    return null;
        //}
    

	    #endregion
  

    }
}
