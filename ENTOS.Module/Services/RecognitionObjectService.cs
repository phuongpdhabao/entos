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

    public partial class RecognitionObjectService : BaseService
    {

        public RecognitionObjectService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public RecognitionObjectService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3446ImportCode
                           public static byte[]? GenerateAvatar(string filePath, int x, int y, int size, int frameIdx)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(filePath));
        File.Copy(filePath, tempFilePath, true);

        try
        {
            int newWidth = (int)(size * 1.8);
            int newHeight = (int)(size * 2.4);
            int newX = Math.Max(0, x - (newWidth - size) / 2);
            int newY = Math.Max(0, y - (newHeight - size) / 2);

            OpenCvSharp.Mat avatarMat = null;

            if (IsImageFile(tempFilePath))
            {
                using var mat = OpenCvSharp.Cv2.ImRead(tempFilePath);
                if (mat.Empty()) return null;

                AdjustCropSize(mat.Width, mat.Height, ref newX, ref newY, ref newWidth, ref newHeight);
                var cropRect = new OpenCvSharp.Rect(newX, newY, newWidth, newHeight);
                avatarMat = new OpenCvSharp.Mat(mat, cropRect).Clone();
            }
            else if (Module.Helpers.MediaHelper.CheckVideoSupport(tempFilePath))
            {
                using var capture = new OpenCvSharp.VideoCapture(tempFilePath);
                if (!capture.IsOpened() || frameIdx < 0 || frameIdx >= capture.FrameCount)
                    return null;

                if (!capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, frameIdx))
                    return null;

                using var mat = new OpenCvSharp.Mat();
                if (capture.Read(mat) && !mat.Empty())
                {
                    AdjustCropSize(mat.Width, mat.Height, ref newX, ref newY, ref newWidth, ref newHeight);
                    var cropRect = new OpenCvSharp.Rect(newX, newY, newWidth, newHeight);
                    avatarMat = new OpenCvSharp.Mat(mat, cropRect).Clone();
                }
            }

            if (avatarMat != null)
            {
                using var ms = avatarMat.ToMemoryStream(".jpg");
                return ms.ToArray();
            }

            return null;
        }
        finally
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }

        #endregion SourceCode3446ImportCode

        

        

        

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionTypeToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionType != null) 
		//			return RecognitionType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Image != null) 
		//			return Image;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ReliabilityToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Reliability != null) 
		//			return Reliability;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SizeToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Size != null) 
		//			return Size;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionPositionListToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionPositionList != null) 
		//			return RecognitionPositionList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Recognition != null) 
		//			return Recognition;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FrameToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (Frame != null) 
		//			return Frame;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RecognitionPositionToolTipControllerText(View view, Module.BusinessObjects.RecognitionObject recognitionobject)
        //{
        //    if (RecognitionPosition != null) 
		//			return RecognitionPosition;
        //    return null;
        //}
    

	    #endregion
  

    }
}
