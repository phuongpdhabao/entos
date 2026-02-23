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
using ENTOS.Module.BusinessObjects;
using DevExpress.ExpressApp.DC;
using System.Reflection;

 
namespace ENTOS.Module.Services 
{

    public partial class DataTypeService : BaseService
    {

        public DataTypeService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public DataTypeService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4533ImportCode
                public Type GetDataTypeType(DataType dataType)
        {
            return ResolveDataType(dataType);
        }
        #endregion SourceCode4533ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
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
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataTypeCategoryToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (DataTypeCategory != null) 
		//			return DataTypeCategory;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareClassTypeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SoftwareClassType != null) 
		//			return SoftwareClassType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InheritDataTypeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (InheritDataType != null) 
		//			return InheritDataType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AccessModifierToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (AccessModifier != null) 
		//			return AccessModifier;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataTypeModifierToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (DataTypeModifier != null) 
		//			return DataTypeModifier;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParameterToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Parameter != null) 
		//			return Parameter;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FullNameToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (FullName != null) 
		//			return FullName;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinkToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Link != null) 
		//			return Link;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object GenericTypeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (GenericType != null) 
		//			return GenericType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataTypeT1ToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (DataTypeT1 != null) 
		//			return DataTypeT1;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataTypeT2ToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (DataTypeT2 != null) 
		//			return DataTypeT2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SourceCodeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SourceCode != null) 
		//			return SourceCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareObjectAttributeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SoftwareObjectAttribute != null) 
		//			return SoftwareObjectAttribute;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FieldAttributeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (FieldAttribute != null) 
		//			return FieldAttribute;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InterfaceDataTypeListToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (InterfaceDataTypeList != null) 
		//			return InterfaceDataTypeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SourceCodeBufferToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SourceCodeBuffer != null) 
		//			return SourceCodeBuffer;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InheritedDataTypeListToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (InheritedDataTypeList != null) 
		//			return InheritedDataTypeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataTypeListToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (DataTypeList != null) 
		//			return DataTypeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareObjectTypeToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LineQuantityToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (LineQuantity != null) 
		//			return LineQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InActiveToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (InActive != null) 
		//			return InActive;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractToolTipControllerText(View view, Module.BusinessObjects.DataType datatype)
        //{
        //    if (Extract != null) 
		//			return Extract;
        //    return null;
        //}
    

	    #endregion
  

    }
}
