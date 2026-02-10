using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects 
{
    [ModelDefault("Caption", "Từ"), ImageName("InsertText")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [NonPersistent]
    public partial class InsertText//: NonPersistentBaseObject   //, INoIndexColumn      //, HbBaseObject
    {
        
        public InsertText()            
        {
        }
   

		//private string _find;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)] 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ"),ToolTip("Từ")]
		//[Index(0)]		 		
		public string Word {get;set;}
		//Tooltip for Object

    }
}