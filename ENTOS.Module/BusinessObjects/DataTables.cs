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
    [ModelDefault("Caption", "Bảng dữ liệu"), ImageName("DataTables")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
 
 
 
 
	[MobileColumnAttribute(Context = "DataTables_LookupListView", TargetItems = nameof(CurrentAddress))]
	[MobileColumnAttribute(Context = "DataTables_ListView", TargetItems = nameof(Col11)+ "," + nameof(Col6)+ "," + nameof(Col3)+ "," + nameof(Col4)+ "," + nameof(Col17)+ "," + nameof(Image3)+ "," + nameof(Col14)+ "," + nameof(Col19)+ "," + nameof(Col12)+ "," + nameof(Image1)+ "," + nameof(Col8)+ "," + nameof(Col13)+ "," + nameof(Image2)+ "," + nameof(Col0)+ "," + nameof(Col16)+ "," + nameof(CurrentAddress)+ "," + nameof(Col15)+ "," + nameof(Image4)+ "," + nameof(Col10)+ "," + nameof(Col18)+ "," + nameof(Col7)+ "," + nameof(Image5)+ "," + nameof(Col2)+ "," + nameof(Col1)+ "," + nameof(Col5)+ "," + nameof(Col9))]
	[DefaultProperty("CurrentAddress")]
 
	[NonPersistent()]
[OptimisticLocking(true)]
    public partial class DataTables: DevExpress.ExpressApp.NonPersistentLiteObject   , INoIndexColumn      //, HbBaseObject
    {

               

		//private string _col0;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cột 0")]
        [ToolTip("Cột 0")]
		//[Index(0)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col0
        { 
		    #region 0319ImportCode 
get;  set;
#endregion 0319ImportCode
			
        }
		//Tooltip for Object
		public object Col0ToolTipControllerText(View view)
        {
        //    if (Col0 != null) 
		//			return Col0;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol0(View view = null)
        { 
			return Col0;
        }
		//Set Default Value
		public void SetDefaultCol0(View view = null)
        {
            //if (Col0 is null){
            //    var result = GetDefaultCol0(view);
            //    if (result != null && result != Col0){
			//          Col0 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col0IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol0();
				//if (result != null && Col0 != null){
				//	return !Col0.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 1")]
        [ToolTip("Cột 1")]
		//[Index(1)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col1
        { 
		    #region 0265ImportCode 
get;  set;
#endregion 0265ImportCode
			
        }
		//Tooltip for Object
		public object Col1ToolTipControllerText(View view)
        {
        //    if (Col1 != null) 
		//			return Col1;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol1(View view = null)
        { 
			return Col1;
        }
		//Set Default Value
		public void SetDefaultCol1(View view = null)
        {
            //if (Col1 is null){
            //    var result = GetDefaultCol1(view);
            //    if (result != null && result != Col1){
			//          Col1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol1();
				//if (result != null && Col1 != null){
				//	return !Col1.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 2")]
        [ToolTip("Cột 2")]
		//[Index(2)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col2
        { 
		    #region 0302ImportCode 
get;  set;
#endregion 0302ImportCode
			
        }
		//Tooltip for Object
		public object Col2ToolTipControllerText(View view)
        {
        //    if (Col2 != null) 
		//			return Col2;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol2(View view = null)
        { 
			return Col2;
        }
		//Set Default Value
		public void SetDefaultCol2(View view = null)
        {
            //if (Col2 is null){
            //    var result = GetDefaultCol2(view);
            //    if (result != null && result != Col2){
			//          Col2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol2();
				//if (result != null && Col2 != null){
				//	return !Col2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col3;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 3")]
        [ToolTip("Cột 3")]
		//[Index(3)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col3
        { 
		    #region 0294ImportCode 
get;  set;
#endregion 0294ImportCode
			
        }
		//Tooltip for Object
		public object Col3ToolTipControllerText(View view)
        {
        //    if (Col3 != null) 
		//			return Col3;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol3(View view = null)
        { 
			return Col3;
        }
		//Set Default Value
		public void SetDefaultCol3(View view = null)
        {
            //if (Col3 is null){
            //    var result = GetDefaultCol3(view);
            //    if (result != null && result != Col3){
			//          Col3 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col3IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol3();
				//if (result != null && Col3 != null){
				//	return !Col3.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col4;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 4")]
        [ToolTip("Cột 4")]
		//[Index(4)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col4
        { 
		    #region 0283ImportCode 
get;  set;
#endregion 0283ImportCode
			
        }
		//Tooltip for Object
		public object Col4ToolTipControllerText(View view)
        {
        //    if (Col4 != null) 
		//			return Col4;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol4(View view = null)
        { 
			return Col4;
        }
		//Set Default Value
		public void SetDefaultCol4(View view = null)
        {
            //if (Col4 is null){
            //    var result = GetDefaultCol4(view);
            //    if (result != null && result != Col4){
			//          Col4 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col4IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol4();
				//if (result != null && Col4 != null){
				//	return !Col4.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col5;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 5")]
        [ToolTip("Cột 5")]
		//[Index(5)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col5
        { 
		    #region 0313ImportCode 
get;  set;
#endregion 0313ImportCode
			
        }
		//Tooltip for Object
		public object Col5ToolTipControllerText(View view)
        {
        //    if (Col5 != null) 
		//			return Col5;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol5(View view = null)
        { 
			return Col5;
        }
		//Set Default Value
		public void SetDefaultCol5(View view = null)
        {
            //if (Col5 is null){
            //    var result = GetDefaultCol5(view);
            //    if (result != null && result != Col5){
			//          Col5 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col5IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol5();
				//if (result != null && Col5 != null){
				//	return !Col5.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col6;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 6")]
        [ToolTip("Cột 6")]
		//[Index(6)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col6
        { 
		    #region 0321ImportCode 
get;  set;
#endregion 0321ImportCode
			
        }
		//Tooltip for Object
		public object Col6ToolTipControllerText(View view)
        {
        //    if (Col6 != null) 
		//			return Col6;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol6(View view = null)
        { 
			return Col6;
        }
		//Set Default Value
		public void SetDefaultCol6(View view = null)
        {
            //if (Col6 is null){
            //    var result = GetDefaultCol6(view);
            //    if (result != null && result != Col6){
			//          Col6 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col6IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol6();
				//if (result != null && Col6 != null){
				//	return !Col6.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col7;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 7")]
        [ToolTip("Cột 7")]
		//[Index(7)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col7
        { 
		    #region 0263ImportCode 
get;  set;
#endregion 0263ImportCode
			
        }
		//Tooltip for Object
		public object Col7ToolTipControllerText(View view)
        {
        //    if (Col7 != null) 
		//			return Col7;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol7(View view = null)
        { 
			return Col7;
        }
		//Set Default Value
		public void SetDefaultCol7(View view = null)
        {
            //if (Col7 is null){
            //    var result = GetDefaultCol7(view);
            //    if (result != null && result != Col7){
			//          Col7 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col7IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol7();
				//if (result != null && Col7 != null){
				//	return !Col7.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col8;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 8")]
        [ToolTip("Cột 8")]
		//[Index(8)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col8
        { 
		    #region 0304ImportCode 
get;  set;
#endregion 0304ImportCode
			
        }
		//Tooltip for Object
		public object Col8ToolTipControllerText(View view)
        {
        //    if (Col8 != null) 
		//			return Col8;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol8(View view = null)
        { 
			return Col8;
        }
		//Set Default Value
		public void SetDefaultCol8(View view = null)
        {
            //if (Col8 is null){
            //    var result = GetDefaultCol8(view);
            //    if (result != null && result != Col8){
			//          Col8 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col8IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol8();
				//if (result != null && Col8 != null){
				//	return !Col8.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col9;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 9")]
        [ToolTip("Cột 9")]
		//[Index(9)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col9
        { 
		    #region 0295ImportCode 
get;  set;
#endregion 0295ImportCode
			
        }
		//Tooltip for Object
		public object Col9ToolTipControllerText(View view)
        {
        //    if (Col9 != null) 
		//			return Col9;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol9(View view = null)
        { 
			return Col9;
        }
		//Set Default Value
		public void SetDefaultCol9(View view = null)
        {
            //if (Col9 is null){
            //    var result = GetDefaultCol9(view);
            //    if (result != null && result != Col9){
			//          Col9 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col9IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol9();
				//if (result != null && Col9 != null){
				//	return !Col9.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col10;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 10")]
        [ToolTip("Cột 10")]
		//[Index(10)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col10
        { 
		    #region 0285ImportCode 
get;  set;
#endregion 0285ImportCode
			
        }
		//Tooltip for Object
		public object Col10ToolTipControllerText(View view)
        {
        //    if (Col10 != null) 
		//			return Col10;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol10(View view = null)
        { 
			return Col10;
        }
		//Set Default Value
		public void SetDefaultCol10(View view = null)
        {
            //if (Col10 is null){
            //    var result = GetDefaultCol10(view);
            //    if (result != null && result != Col10){
			//          Col10 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col10IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol10();
				//if (result != null && Col10 != null){
				//	return !Col10.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col11;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 11")]
        [ToolTip("Cột 11")]
		//[Index(11)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col11
        { 
		    #region 0290ImportCode 
get;  set;
#endregion 0290ImportCode
			
        }
		//Tooltip for Object
		public object Col11ToolTipControllerText(View view)
        {
        //    if (Col11 != null) 
		//			return Col11;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol11(View view = null)
        { 
			return Col11;
        }
		//Set Default Value
		public void SetDefaultCol11(View view = null)
        {
            //if (Col11 is null){
            //    var result = GetDefaultCol11(view);
            //    if (result != null && result != Col11){
			//          Col11 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col11IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol11();
				//if (result != null && Col11 != null){
				//	return !Col11.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col12;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 12")]
        [ToolTip("Cột 12")]
		//[Index(12)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col12
        { 
		    #region 0264ImportCode 
get;  set;
#endregion 0264ImportCode
			
        }
		//Tooltip for Object
		public object Col12ToolTipControllerText(View view)
        {
        //    if (Col12 != null) 
		//			return Col12;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol12(View view = null)
        { 
			return Col12;
        }
		//Set Default Value
		public void SetDefaultCol12(View view = null)
        {
            //if (Col12 is null){
            //    var result = GetDefaultCol12(view);
            //    if (result != null && result != Col12){
			//          Col12 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col12IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol12();
				//if (result != null && Col12 != null){
				//	return !Col12.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col13;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 13")]
        [ToolTip("Cột 13")]
		//[Index(13)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col13
        { 
		    #region 0292ImportCode 
get;  set;
#endregion 0292ImportCode
			
        }
		//Tooltip for Object
		public object Col13ToolTipControllerText(View view)
        {
        //    if (Col13 != null) 
		//			return Col13;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol13(View view = null)
        { 
			return Col13;
        }
		//Set Default Value
		public void SetDefaultCol13(View view = null)
        {
            //if (Col13 is null){
            //    var result = GetDefaultCol13(view);
            //    if (result != null && result != Col13){
			//          Col13 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col13IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol13();
				//if (result != null && Col13 != null){
				//	return !Col13.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col14;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 14")]
        [ToolTip("Cột 14")]
		//[Index(14)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col14
        { 
		    #region 0274ImportCode 
get;  set;
#endregion 0274ImportCode
			
        }
		//Tooltip for Object
		public object Col14ToolTipControllerText(View view)
        {
        //    if (Col14 != null) 
		//			return Col14;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol14(View view = null)
        { 
			return Col14;
        }
		//Set Default Value
		public void SetDefaultCol14(View view = null)
        {
            //if (Col14 is null){
            //    var result = GetDefaultCol14(view);
            //    if (result != null && result != Col14){
			//          Col14 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col14IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol14();
				//if (result != null && Col14 != null){
				//	return !Col14.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col15;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 15")]
        [ToolTip("Cột 15")]
		//[Index(15)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col15
        { 
		    #region 0314ImportCode 
get;  set;
#endregion 0314ImportCode
			
        }
		//Tooltip for Object
		public object Col15ToolTipControllerText(View view)
        {
        //    if (Col15 != null) 
		//			return Col15;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol15(View view = null)
        { 
			return Col15;
        }
		//Set Default Value
		public void SetDefaultCol15(View view = null)
        {
            //if (Col15 is null){
            //    var result = GetDefaultCol15(view);
            //    if (result != null && result != Col15){
			//          Col15 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col15IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol15();
				//if (result != null && Col15 != null){
				//	return !Col15.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col16;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 16")]
        [ToolTip("Cột 16")]
		//[Index(16)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col16
        { 
		    #region 0271ImportCode 
get;  set;
#endregion 0271ImportCode
			
        }
		//Tooltip for Object
		public object Col16ToolTipControllerText(View view)
        {
        //    if (Col16 != null) 
		//			return Col16;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol16(View view = null)
        { 
			return Col16;
        }
		//Set Default Value
		public void SetDefaultCol16(View view = null)
        {
            //if (Col16 is null){
            //    var result = GetDefaultCol16(view);
            //    if (result != null && result != Col16){
			//          Col16 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col16IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol16();
				//if (result != null && Col16 != null){
				//	return !Col16.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col17;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 17")]
        [ToolTip("Cột 17")]
		//[Index(17)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col17
        { 
		    #region 0315ImportCode 
get;  set;
#endregion 0315ImportCode
			
        }
		//Tooltip for Object
		public object Col17ToolTipControllerText(View view)
        {
        //    if (Col17 != null) 
		//			return Col17;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol17(View view = null)
        { 
			return Col17;
        }
		//Set Default Value
		public void SetDefaultCol17(View view = null)
        {
            //if (Col17 is null){
            //    var result = GetDefaultCol17(view);
            //    if (result != null && result != Col17){
			//          Col17 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col17IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol17();
				//if (result != null && Col17 != null){
				//	return !Col17.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col18;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 18")]
        [ToolTip("Cột 18")]
		//[Index(18)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col18
        { 
		    #region 0308ImportCode 
get;  set;
#endregion 0308ImportCode
			
        }
		//Tooltip for Object
		public object Col18ToolTipControllerText(View view)
        {
        //    if (Col18 != null) 
		//			return Col18;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol18(View view = null)
        { 
			return Col18;
        }
		//Set Default Value
		public void SetDefaultCol18(View view = null)
        {
            //if (Col18 is null){
            //    var result = GetDefaultCol18(view);
            //    if (result != null && result != Col18){
			//          Col18 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col18IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol18();
				//if (result != null && Col18 != null){
				//	return !Col18.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _col19;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cột 19")]
        [ToolTip("Cột 19")]
		//[Index(19)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Col19
        { 
		    #region 0299ImportCode 
get;  set;
#endregion 0299ImportCode
			
        }
		//Tooltip for Object
		public object Col19ToolTipControllerText(View view)
        {
        //    if (Col19 != null) 
		//			return Col19;
            return null;
        }
		//Get Default Value
        public string GetDefaultCol19(View view = null)
        { 
			return Col19;
        }
		//Set Default Value
		public void SetDefaultCol19(View view = null)
        {
            //if (Col19 is null){
            //    var result = GetDefaultCol19(view);
            //    if (result != null && result != Col19){
			//          Col19 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Col19IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCol19();
				//if (result != null && Col19 != null){
				//	return !Col19.Equals(result);
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
		    #region 0301ImportCode 
get;  set;
#endregion 0301ImportCode
			
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
		    #region 0282ImportCode 
get;  set;
#endregion 0282ImportCode
			
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
		    #region 0275ImportCode 
get;  set;
#endregion 0275ImportCode
			
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
		    #region 0268ImportCode 
get;  set;
#endregion 0268ImportCode
			
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

	
       
		//private byte[] _image5;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh 5")]
        [ToolTip("Ảnh 5")]
		//[Index(24)]		
		[Appearance("Ảnh 5Background", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image5
        { 
		    #region 0317ImportCode 
get;  set;
#endregion 0317ImportCode
			
        }
		//Tooltip for Object
		public object Image5ToolTipControllerText(View view)
        {
        //    if (Image5 != null) 
		//			return Image5;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage5(View view = null)
        { 
			return Image5;
        }
		//Set Default Value
		public void SetDefaultImage5(View view = null)
        {
            //if (Image5 is null){
            //    var result = GetDefaultImage5(view);
            //    if (result != null && result != Image5){
			//          Image5 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Image5IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage5();
				//if (result != null && Image5 != null){
				//	return !Image5.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _currentaddress;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(25)]		

 		[Size(220)]
		public string CurrentAddress
        { 
		    #region 0278ImportCode 
get;  set;
#endregion 0278ImportCode
			
        }
		//Tooltip for Object
		public object CurrentAddressToolTipControllerText(View view)
        {
        //    if (CurrentAddress != null) 
		//			return CurrentAddress;
            return null;
        }
		//Get Default Value
        public string GetDefaultCurrentAddress(View view = null)
        { 
			return CurrentAddress;
        }
		//Set Default Value
		public void SetDefaultCurrentAddress(View view = null)
        {
            //if (CurrentAddress is null){
            //    var result = GetDefaultCurrentAddress(view);
            //    if (result != null && result != CurrentAddress){
			//          CurrentAddress = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrentAddressIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrentAddress();
				//if (result != null && CurrentAddress != null){
				//	return !CurrentAddress.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _isheader;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiêu đề")]
        [ToolTip("Tiêu đề")]
		//[Index(26)]		
		public bool IsHeader
        { 
		    #region 0318ImportCode 
get; set;
#endregion 0318ImportCode
			
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

	
       
        #region Các phương thức code gen từ Software Task
        #endregion
//Mã nguồn bổ sung
#region DataTablesImportCode
public bool SetPropertyValue(int index, string value)
        {
            try
            {
                this.SetMemberValue("Col" + index, value);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
#endregion DataTablesImportCode
		 		 
    }
}
