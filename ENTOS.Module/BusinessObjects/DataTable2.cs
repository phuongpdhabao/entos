using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
using ENTOS.Module.SystemObjects;
using ENTOS.Module;
using ENTOS.Domain.Abstractions;
using ENTOS.Module.FilterControllers;


namespace ENTOS.Module.BusinessObjects 
{
    [ModelDefault("Caption", "Bảng dữ liệu"), ImageName("DataTable2")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "DataTable2_LookupListView", TargetItems = nameof(c20)+ "," + nameof(c01)+ "," + nameof(c02))]
	[MobileColumnAttribute(Context = "DataTable2_ListView", TargetItems = nameof(c14)+ "," + nameof(Image3)+ "," + nameof(c12)+ "," + nameof(Update)+ "," + nameof(c16)+ "," + nameof(c18)+ "," + nameof(Image4)+ "," + nameof(c07)+ "," + nameof(c20)+ "," + nameof(c15)+ "," + nameof(Image2)+ "," + nameof(c01)+ "," + nameof(c05)+ "," + nameof(c04)+ "," + nameof(Image1)+ "," + nameof(c17)+ "," + nameof(c09)+ "," + nameof(c19)+ "," + nameof(c10)+ "," + nameof(c13)+ "," + nameof(c02)+ "," + nameof(c11)+ "," + nameof(c03)+ "," + nameof(c06)+ "," + nameof(c08))]
 
[OptimisticLocking(true)]
    public partial class DataTable2:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public DataTable2(Session session)
            : base(session) {              
        }

				public string ToolTipControllerText(View view)
        {
            var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
            return result;
        }
		        private System.Collections.Generic.Dictionary<string, bool> _cacheAppearanceDisableDelete;
		[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                if (Session.IsNewObject(this))
                    return false;
                                
                return false;
            }
        }

        public void OnViewObjectSpaceCommitted(View view)
        {

           
        }
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)

		[Key(true)]
		[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]     
        public Guid Oid { get; set; }
               

		//private string _c01;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("01")]
        [ToolTip("01")]
		//[Index(0)]		

 		[Size(250)]
		public string c01
        { 
		    #region 3147ImportCode 
get;  set;
#endregion 3147ImportCode
			
        }
		//Tooltip for Object
		public object c01ToolTipControllerText(View view)
        {
        //    if (c01 != null) 
		//			return c01;
            return null;
        }
		//Get Default Value
        public string GetDefaultc01(View view = null)
        { 
			return c01;
        }
		//Set Default Value
		public void SetDefaultc01(View view = null)
        {
            //if (c01 is null){
            //    var result = GetDefaultc01(view);
            //    if (result != null && result != c01){
			//          c01 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c01IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc01();
				//if (result != null && c01 != null){
				//	return !c01.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c02;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("02")]
        [ToolTip("02")]
		//[Index(1)]		

 		[Size(250)]
		public string c02
        { 
		    #region 3148ImportCode 
get;  set;
#endregion 3148ImportCode
			
        }
		//Tooltip for Object
		public object c02ToolTipControllerText(View view)
        {
        //    if (c02 != null) 
		//			return c02;
            return null;
        }
		//Get Default Value
        public string GetDefaultc02(View view = null)
        { 
			return c02;
        }
		//Set Default Value
		public void SetDefaultc02(View view = null)
        {
            //if (c02 is null){
            //    var result = GetDefaultc02(view);
            //    if (result != null && result != c02){
			//          c02 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c02IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc02();
				//if (result != null && c02 != null){
				//	return !c02.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c03;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("03")]
        [ToolTip("03")]
		//[Index(2)]		

 		[Size(250)]
		public string c03
        { 
		    #region 3149ImportCode 
get;  set;
#endregion 3149ImportCode
			
        }
		//Tooltip for Object
		public object c03ToolTipControllerText(View view)
        {
        //    if (c03 != null) 
		//			return c03;
            return null;
        }
		//Get Default Value
        public string GetDefaultc03(View view = null)
        { 
			return c03;
        }
		//Set Default Value
		public void SetDefaultc03(View view = null)
        {
            //if (c03 is null){
            //    var result = GetDefaultc03(view);
            //    if (result != null && result != c03){
			//          c03 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c03IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc03();
				//if (result != null && c03 != null){
				//	return !c03.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c04;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("04")]
        [ToolTip("04")]
		//[Index(3)]		

 		[Size(250)]
		public string c04
        { 
		    #region 3150ImportCode 
get;  set;
#endregion 3150ImportCode
			
        }
		//Tooltip for Object
		public object c04ToolTipControllerText(View view)
        {
        //    if (c04 != null) 
		//			return c04;
            return null;
        }
		//Get Default Value
        public string GetDefaultc04(View view = null)
        { 
			return c04;
        }
		//Set Default Value
		public void SetDefaultc04(View view = null)
        {
            //if (c04 is null){
            //    var result = GetDefaultc04(view);
            //    if (result != null && result != c04){
			//          c04 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c04IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc04();
				//if (result != null && c04 != null){
				//	return !c04.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c05;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("05")]
        [ToolTip("05")]
		//[Index(4)]		

 		[Size(250)]
		public string c05
        { 
		    #region 3151ImportCode 
get;  set;
#endregion 3151ImportCode
			
        }
		//Tooltip for Object
		public object c05ToolTipControllerText(View view)
        {
        //    if (c05 != null) 
		//			return c05;
            return null;
        }
		//Get Default Value
        public string GetDefaultc05(View view = null)
        { 
			return c05;
        }
		//Set Default Value
		public void SetDefaultc05(View view = null)
        {
            //if (c05 is null){
            //    var result = GetDefaultc05(view);
            //    if (result != null && result != c05){
			//          c05 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c05IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc05();
				//if (result != null && c05 != null){
				//	return !c05.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c06;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("06")]
        [ToolTip("06")]
		//[Index(5)]		

 		[Size(250)]
		public string c06
        { 
		    #region 3152ImportCode 
get;  set;
#endregion 3152ImportCode
			
        }
		//Tooltip for Object
		public object c06ToolTipControllerText(View view)
        {
        //    if (c06 != null) 
		//			return c06;
            return null;
        }
		//Get Default Value
        public string GetDefaultc06(View view = null)
        { 
			return c06;
        }
		//Set Default Value
		public void SetDefaultc06(View view = null)
        {
            //if (c06 is null){
            //    var result = GetDefaultc06(view);
            //    if (result != null && result != c06){
			//          c06 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c06IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc06();
				//if (result != null && c06 != null){
				//	return !c06.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c07;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("07")]
        [ToolTip("07")]
		//[Index(6)]		

 		[Size(250)]
		public string c07
        { 
		    #region 3153ImportCode 
get;  set;
#endregion 3153ImportCode
			
        }
		//Tooltip for Object
		public object c07ToolTipControllerText(View view)
        {
        //    if (c07 != null) 
		//			return c07;
            return null;
        }
		//Get Default Value
        public string GetDefaultc07(View view = null)
        { 
			return c07;
        }
		//Set Default Value
		public void SetDefaultc07(View view = null)
        {
            //if (c07 is null){
            //    var result = GetDefaultc07(view);
            //    if (result != null && result != c07){
			//          c07 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c07IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc07();
				//if (result != null && c07 != null){
				//	return !c07.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c08;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("08")]
        [ToolTip("08")]
		//[Index(7)]		

 		[Size(250)]
		public string c08
        { 
		    #region 3154ImportCode 
get;  set;
#endregion 3154ImportCode
			
        }
		//Tooltip for Object
		public object c08ToolTipControllerText(View view)
        {
        //    if (c08 != null) 
		//			return c08;
            return null;
        }
		//Get Default Value
        public string GetDefaultc08(View view = null)
        { 
			return c08;
        }
		//Set Default Value
		public void SetDefaultc08(View view = null)
        {
            //if (c08 is null){
            //    var result = GetDefaultc08(view);
            //    if (result != null && result != c08){
			//          c08 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c08IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc08();
				//if (result != null && c08 != null){
				//	return !c08.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c09;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("09")]
        [ToolTip("09")]
		//[Index(8)]		

 		[Size(250)]
		public string c09
        { 
		    #region 3155ImportCode 
get;  set;
#endregion 3155ImportCode
			
        }
		//Tooltip for Object
		public object c09ToolTipControllerText(View view)
        {
        //    if (c09 != null) 
		//			return c09;
            return null;
        }
		//Get Default Value
        public string GetDefaultc09(View view = null)
        { 
			return c09;
        }
		//Set Default Value
		public void SetDefaultc09(View view = null)
        {
            //if (c09 is null){
            //    var result = GetDefaultc09(view);
            //    if (result != null && result != c09){
			//          c09 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c09IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc09();
				//if (result != null && c09 != null){
				//	return !c09.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c10;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("10")]
        [ToolTip("10")]
		//[Index(9)]		

 		[Size(250)]
		public string c10
        { 
		    #region 3156ImportCode 
get;  set;
#endregion 3156ImportCode
			
        }
		//Tooltip for Object
		public object c10ToolTipControllerText(View view)
        {
        //    if (c10 != null) 
		//			return c10;
            return null;
        }
		//Get Default Value
        public string GetDefaultc10(View view = null)
        { 
			return c10;
        }
		//Set Default Value
		public void SetDefaultc10(View view = null)
        {
            //if (c10 is null){
            //    var result = GetDefaultc10(view);
            //    if (result != null && result != c10){
			//          c10 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c10IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc10();
				//if (result != null && c10 != null){
				//	return !c10.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c11;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("11")]
        [ToolTip("11")]
		//[Index(10)]		

 		[Size(250)]
		public string c11
        { 
		    #region 3157ImportCode 
get;  set;
#endregion 3157ImportCode
			
        }
		//Tooltip for Object
		public object c11ToolTipControllerText(View view)
        {
        //    if (c11 != null) 
		//			return c11;
            return null;
        }
		//Get Default Value
        public string GetDefaultc11(View view = null)
        { 
			return c11;
        }
		//Set Default Value
		public void SetDefaultc11(View view = null)
        {
            //if (c11 is null){
            //    var result = GetDefaultc11(view);
            //    if (result != null && result != c11){
			//          c11 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c11IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc11();
				//if (result != null && c11 != null){
				//	return !c11.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c12;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("12")]
        [ToolTip("12")]
		//[Index(11)]		

 		[Size(250)]
		public string c12
        { 
		    #region 3158ImportCode 
get;  set;
#endregion 3158ImportCode
			
        }
		//Tooltip for Object
		public object c12ToolTipControllerText(View view)
        {
        //    if (c12 != null) 
		//			return c12;
            return null;
        }
		//Get Default Value
        public string GetDefaultc12(View view = null)
        { 
			return c12;
        }
		//Set Default Value
		public void SetDefaultc12(View view = null)
        {
            //if (c12 is null){
            //    var result = GetDefaultc12(view);
            //    if (result != null && result != c12){
			//          c12 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c12IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc12();
				//if (result != null && c12 != null){
				//	return !c12.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c13;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("13")]
        [ToolTip("13")]
		//[Index(12)]		

 		[Size(250)]
		public string c13
        { 
		    #region 3159ImportCode 
get;  set;
#endregion 3159ImportCode
			
        }
		//Tooltip for Object
		public object c13ToolTipControllerText(View view)
        {
        //    if (c13 != null) 
		//			return c13;
            return null;
        }
		//Get Default Value
        public string GetDefaultc13(View view = null)
        { 
			return c13;
        }
		//Set Default Value
		public void SetDefaultc13(View view = null)
        {
            //if (c13 is null){
            //    var result = GetDefaultc13(view);
            //    if (result != null && result != c13){
			//          c13 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c13IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc13();
				//if (result != null && c13 != null){
				//	return !c13.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c14;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("14")]
        [ToolTip("14")]
		//[Index(13)]		

 		[Size(250)]
		public string c14
        { 
		    #region 3160ImportCode 
get;  set;
#endregion 3160ImportCode
			
        }
		//Tooltip for Object
		public object c14ToolTipControllerText(View view)
        {
        //    if (c14 != null) 
		//			return c14;
            return null;
        }
		//Get Default Value
        public string GetDefaultc14(View view = null)
        { 
			return c14;
        }
		//Set Default Value
		public void SetDefaultc14(View view = null)
        {
            //if (c14 is null){
            //    var result = GetDefaultc14(view);
            //    if (result != null && result != c14){
			//          c14 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c14IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc14();
				//if (result != null && c14 != null){
				//	return !c14.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c15;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("15")]
        [ToolTip("15")]
		//[Index(14)]		

 		[Size(250)]
		public string c15
        { 
		    #region 3161ImportCode 
get;  set;
#endregion 3161ImportCode
			
        }
		//Tooltip for Object
		public object c15ToolTipControllerText(View view)
        {
        //    if (c15 != null) 
		//			return c15;
            return null;
        }
		//Get Default Value
        public string GetDefaultc15(View view = null)
        { 
			return c15;
        }
		//Set Default Value
		public void SetDefaultc15(View view = null)
        {
            //if (c15 is null){
            //    var result = GetDefaultc15(view);
            //    if (result != null && result != c15){
			//          c15 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c15IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc15();
				//if (result != null && c15 != null){
				//	return !c15.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c16;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("16")]
        [ToolTip("16")]
		//[Index(15)]		

 		[Size(250)]
		public string c16
        { 
		    #region 3162ImportCode 
get;  set;
#endregion 3162ImportCode
			
        }
		//Tooltip for Object
		public object c16ToolTipControllerText(View view)
        {
        //    if (c16 != null) 
		//			return c16;
            return null;
        }
		//Get Default Value
        public string GetDefaultc16(View view = null)
        { 
			return c16;
        }
		//Set Default Value
		public void SetDefaultc16(View view = null)
        {
            //if (c16 is null){
            //    var result = GetDefaultc16(view);
            //    if (result != null && result != c16){
			//          c16 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c16IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc16();
				//if (result != null && c16 != null){
				//	return !c16.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c17;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("17")]
        [ToolTip("17")]
		//[Index(16)]		

 		[Size(250)]
		public string c17
        { 
		    #region 3163ImportCode 
get;  set;
#endregion 3163ImportCode
			
        }
		//Tooltip for Object
		public object c17ToolTipControllerText(View view)
        {
        //    if (c17 != null) 
		//			return c17;
            return null;
        }
		//Get Default Value
        public string GetDefaultc17(View view = null)
        { 
			return c17;
        }
		//Set Default Value
		public void SetDefaultc17(View view = null)
        {
            //if (c17 is null){
            //    var result = GetDefaultc17(view);
            //    if (result != null && result != c17){
			//          c17 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c17IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc17();
				//if (result != null && c17 != null){
				//	return !c17.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c18;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("18")]
        [ToolTip("18")]
		//[Index(17)]		

 		[Size(250)]
		public string c18
        { 
		    #region 3164ImportCode 
get;  set;
#endregion 3164ImportCode
			
        }
		//Tooltip for Object
		public object c18ToolTipControllerText(View view)
        {
        //    if (c18 != null) 
		//			return c18;
            return null;
        }
		//Get Default Value
        public string GetDefaultc18(View view = null)
        { 
			return c18;
        }
		//Set Default Value
		public void SetDefaultc18(View view = null)
        {
            //if (c18 is null){
            //    var result = GetDefaultc18(view);
            //    if (result != null && result != c18){
			//          c18 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c18IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc18();
				//if (result != null && c18 != null){
				//	return !c18.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c19;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("19")]
        [ToolTip("19")]
		//[Index(18)]		

 		[Size(250)]
		public string c19
        { 
		    #region 3165ImportCode 
get;  set;
#endregion 3165ImportCode
			
        }
		//Tooltip for Object
		public object c19ToolTipControllerText(View view)
        {
        //    if (c19 != null) 
		//			return c19;
            return null;
        }
		//Get Default Value
        public string GetDefaultc19(View view = null)
        { 
			return c19;
        }
		//Set Default Value
		public void SetDefaultc19(View view = null)
        {
            //if (c19 is null){
            //    var result = GetDefaultc19(view);
            //    if (result != null && result != c19){
			//          c19 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c19IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc19();
				//if (result != null && c19 != null){
				//	return !c19.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _c20;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("20")]
        [ToolTip("20")]
		//[Index(19)]		

 		[Size(250)]
		public string c20
        { 
		    #region 3146ImportCode 
get;  set;
#endregion 3146ImportCode
			
        }
		//Tooltip for Object
		public object c20ToolTipControllerText(View view)
        {
        //    if (c20 != null) 
		//			return c20;
            return null;
        }
		//Get Default Value
        public string GetDefaultc20(View view = null)
        { 
			return c20;
        }
		//Set Default Value
		public void SetDefaultc20(View view = null)
        {
            //if (c20 is null){
            //    var result = GetDefaultc20(view);
            //    if (result != null && result != c20){
			//          c20 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool c20IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultc20();
				//if (result != null && c20 != null){
				//	return !c20.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh 1")]
        [ToolTip("Ảnh 1")]
		//[Index(20)]		
		[Appearance("Ảnh 1Background", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image1
        { 
		    #region 3166ImportCode 
get;  set;
#endregion 3166ImportCode
			
        }
		//Tooltip for Object
		public object Image1ToolTipControllerText(View view)
        {
        //    if (Image1 != null) 
		//			return Image1;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage1(View view = null)
        { 
			return Image1;
        }
		//Set Default Value
		public void SetDefaultImage1(View view = null)
        {
            //if (Image1 is null){
            //    var result = GetDefaultImage1(view);
            //    if (result != null && result != Image1){
			//          Image1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Image1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage1();
				//if (result != null && Image1 != null){
				//	return !Image1.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh 2")]
        [ToolTip("Ảnh 2")]
		//[Index(21)]		
		[Appearance("Ảnh 2Background", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image2
        { 
		    #region 3167ImportCode 
get;  set;
#endregion 3167ImportCode
			
        }
		//Tooltip for Object
		public object Image2ToolTipControllerText(View view)
        {
        //    if (Image2 != null) 
		//			return Image2;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage2(View view = null)
        { 
			return Image2;
        }
		//Set Default Value
		public void SetDefaultImage2(View view = null)
        {
            //if (Image2 is null){
            //    var result = GetDefaultImage2(view);
            //    if (result != null && result != Image2){
			//          Image2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Image2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage2();
				//if (result != null && Image2 != null){
				//	return !Image2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image3;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh 3")]
        [ToolTip("Ảnh 3")]
		//[Index(22)]		
		[Appearance("Ảnh 3Background", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image3
        { 
		    #region 3168ImportCode 
get;  set;
#endregion 3168ImportCode
			
        }
		//Tooltip for Object
		public object Image3ToolTipControllerText(View view)
        {
        //    if (Image3 != null) 
		//			return Image3;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage3(View view = null)
        { 
			return Image3;
        }
		//Set Default Value
		public void SetDefaultImage3(View view = null)
        {
            //if (Image3 is null){
            //    var result = GetDefaultImage3(view);
            //    if (result != null && result != Image3){
			//          Image3 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Image3IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage3();
				//if (result != null && Image3 != null){
				//	return !Image3.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image4;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh 4")]
        [ToolTip("Ảnh 4")]
		//[Index(23)]		
		[Appearance("Ảnh 4Background", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image4
        { 
		    #region 3169ImportCode 
get;  set;
#endregion 3169ImportCode
			
        }
		//Tooltip for Object
		public object Image4ToolTipControllerText(View view)
        {
        //    if (Image4 != null) 
		//			return Image4;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage4(View view = null)
        { 
			return Image4;
        }
		//Set Default Value
		public void SetDefaultImage4(View view = null)
        {
            //if (Image4 is null){
            //    var result = GetDefaultImage4(view);
            //    if (result != null && result != Image4){
			//          Image4 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Image4IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage4();
				//if (result != null && Image4 != null){
				//	return !Image4.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _isheader;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiêu đề")]
        [ToolTip("Tiêu đề")]
		//[Index(24)]		
		public bool IsHeader
        { 
		    #region 3172ImportCode 
get; set;
#endregion 3172ImportCode
			
        }
		//Tooltip for Object
		public object IsHeaderToolTipControllerText(View view)
        {
        //    if (IsHeader != null) 
		//			return IsHeader;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIsHeader(View view = null)
        { 
			return IsHeader;
        }
		//Set Default Value
		public void SetDefaultIsHeader(View view = null)
        {
            //if (IsHeader is null){
            //    var result = GetDefaultIsHeader(view);
            //    if (result != null && result != IsHeader){
			//          IsHeader = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IsHeaderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIsHeader();
				//if (result != null && IsHeader != null){
				//	return !IsHeader.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.BookMark _link;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(25)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LinkCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.BookMark Link
        { 
		    get => GetPropertyValue<Module.BusinessObjects.BookMark>("Link");                         
			set => SetPropertyValue<Module.BusinessObjects.BookMark>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.BookMark GetDefaultLink(View view = null)
        { 
			return Link;
        }
		//Set Default Value
		public void SetDefaultLink(View view = null)
        {
            //if (Link is null){
            //    var result = GetDefaultLink(view);
            //    if (result != null && result != Link){
			//          Link = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLink();
				//if (result != null && Link != null){
				//	return !Link.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LinkCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Link));
            }
        }
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(26)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Update
        { 
		    get => GetPropertyValue<DateTime?>("Update");                         
			set => SetPropertyValue<DateTime?>("Update", value); 
			
        }
		//Tooltip for Object
		public object UpdateToolTipControllerText(View view)
        {
        //    if (Update != null) 
		//			return Update;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdate();
				//if (result != null && Update != null){
				//	return !Update.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultc01(View view = null);
        //SetDefaultc02(View view = null);
        //SetDefaultc03(View view = null);
        //SetDefaultc04(View view = null);
        //SetDefaultc05(View view = null);
        //SetDefaultc06(View view = null);
        //SetDefaultc07(View view = null);
        //SetDefaultc08(View view = null);
        //SetDefaultc09(View view = null);
        //SetDefaultc10(View view = null);
        //SetDefaultc11(View view = null);
        //SetDefaultc12(View view = null);
        //SetDefaultc13(View view = null);
        //SetDefaultc14(View view = null);
        //SetDefaultc15(View view = null);
        //SetDefaultc16(View view = null);
        //SetDefaultc17(View view = null);
        //SetDefaultc18(View view = null);
        //SetDefaultc19(View view = null);
        //SetDefaultc20(View view = null);
        //SetDefaultImage1(View view = null);
        //SetDefaultImage2(View view = null);
        //SetDefaultImage3(View view = null);
        //SetDefaultImage4(View view = null);
        //SetDefaultIsHeader(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultUpdate(View view = null);
			
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
        }

        private bool alreadySaving = false;        
        protected override void OnSaving()
        {
             base.OnSaving();
//            Update = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
    		if (!(Session is NestedUnitOfWork)&& (Session.DataLayer != null))
            {
   //             if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
   //             {
   //                 //Khi đang mở Object
   //             }
   //             else if ((Session.ObjectLayer is DevExpress.Xpo.SimpleObjectLayer))
   //             {
   //                 //Từ popup form con về form chính
   //             }
             }
        }
        
        protected override void OnSaved()
        {
             base.OnSaved();
        }

        protected override void OnDeleting()
        {
             base.OnDeleting();
  
        }

        protected override void OnDeleted()
        {
             base.OnDeleted();
            
        }

		protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {

                  
            }
        }

   


		//protected override XPCollection<T> CreateCollection<T>(DevExpress.Xpo.Metadata.XPMemberInfo property)
        //{
        //    var collection = base.CreateCollection<T>(property);
        //    collection.ListChanged += OnItemListChanged;
        //    return collection;
        //}

        //private void OnItemListChanged(object sender, ListChangedEventArgs e)
        //{            
            //if (e.ListChangedType == ListChangedType.ItemAdded)
            //{
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
