using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;


namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Clipboard"), ImageName("PasteFromClipboard")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]    
    //[Appearance("Hide CallDefaultMethod", TargetItems = "CallDefaultMethod", Criteria = "Not IsNullOrEmpty(Condition)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    public partial class PasteClipboard : GlobalFunctionInListView     //, HbBaseObject
    {

        public PasteClipboard(Session session)
            : base(session) {              
        }

				public string ToolTipControllerText()
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
				[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                                
                return false;
            }
        }

 
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //SetDefaultUser();
            //Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
        }
        
        protected override void OnSaving()
        {
            base.OnSaving();
			if (!(Session is NestedUnitOfWork)&& (Session.DataLayer != null))
            {
                //if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                //{
                //    //Khi đang mở Object
                //}
                //else if ((Session.ObjectLayer is DevExpress.Xpo.SimpleObjectLayer))
                //{
                //    //Từ popup form con về form chính
                //}
            }
        }
        
        protected override void OnSaved()
        {
            base.OnSaved();
        }

		protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                if (newValue != null)
                {
                }                    
            }
        }

		protected override XPCollection<T> CreateCollection<T>(DevExpress.Xpo.Metadata.XPMemberInfo property)
        {
            var collection = base.CreateCollection<T>(property);
            collection.ListChanged += OnItemListChanged;
            return collection;
        }

        private void OnItemListChanged(object sender, ListChangedEventArgs e)
        {            
            //if (e.ListChangedType == ListChangedType.ItemAdded)
            //{
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        }
		 
    }
}