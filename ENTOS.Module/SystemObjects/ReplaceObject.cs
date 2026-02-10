using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects 
{
    [ModelDefault("Caption", "Thay từ"), ImageName("ReplaceObject")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [NonPersistent]
    public partial class ReplaceObject//: NonPersistentBaseObject   //, INoIndexColumn      //, HbBaseObject
    {
        
        public ReplaceObject()            
        {
        }
   

		//private string _find;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tìm"),ToolTip("Hỗ trợ thay thế dạng sách ngăn cách bởi dấu gạch đứng |")]
		//[Index(0)]		 		
		public string Find {get;set;}
		//Tooltip for Object
		       
		//private string _replace;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ thay"),ToolTip("Phải bằng Tìm khi cùng dạng sách bằng dấu gạch đứng | ")]
		//[Index(1)]		 	
		public string Replace { get; set; }
        //Tooltip for Object
    }
}