using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class AccountEntryViewController: BaseViewController<Module.BusinessObjects.AccountEntry>
    {      
        
        public AccountEntryViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.AccountEntry);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.AccountEntryService accountEntryService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             accountEntryService = new Module.Services.AccountEntryService(this);
             
            #region AccountingTemplateOnViewControlsCreatedCode
		                var listview = View as ListView;
            if (listview != null)
            {
                if (AccountingTemplate.Items.Count > 0)
                {
                    if (AccountingTemplate.SelectedItem != null)
                    {
                        AccountingTemplate.DoExecute(AccountingTemplate.SelectedItem);
                    }
                }
                else
                if (AccountingTemplate.SelectedItem == null)
                {
                    //Hỗ trợ lazy loadz
                    //filteringCriterionAction.ShowItemsOnClick = true;
                    var masterObject = Tools.GetMasterObjectFromView(View);
                    if (masterObject is Order || masterObject is Consume)
                    {
                        AccountingTemplate.Items.Add(new ChoiceActionItem("Tạo hạch toán mẫu", "CreateDefaultEntry"));
                        accountEntryService.GetDefaultEntryMenu(this, AccountingTemplate);
                    }
                    //if (filteringCriterionAction.Items.Count > 0)
                    //{
                    //    if (filteringCriterionAction.SelectedItem == null)
                    //    {
                    //        filteringCriterionAction.SelectedIndex = 0;
                    //    }
                    //    else if (filteringCriterionAction.SelectedIndex != 0)
                    //    {
                    //        filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
                    //    }
                    //}
                }
            }

		    #endregion AccountingTemplateOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1554            Oid: 2c651588-0a56-43ee-a2c0-10428058815c
		private void AccountingTemplate_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AccountingTemplate), "Hạch toán mẫu");              
      
            #region AccountingTemplateImportCode
            var objSpace = Application.CreateObjectSpace();
            if (e.SelectedChoiceActionItem.Id.Equals("CreateAccountingTemplate"))
            {
                var newObject = objSpace.CreateObject(View.ObjectTypeInfo.Type);
                var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
                if (masterObject != null)
                {
                    var masterObjKey = objSpace.GetKeyValue(masterObject);
                    if (masterObjKey != null)
                    {
                        var masterObj = objSpace.GetObjectByKey(masterObject.GetType(), masterObjKey);
                        if (masterObj != null)
                        {
                            bool bindingList = false;
                            if (View is ListView)
                            {
                                var collection = ((PropertyCollectionSource)((ListView)View).CollectionSource);
                                if (collection.MemberInfo != null && collection.MemberInfo.AssociatedMemberInfo != null &&
                                    !collection.MemberInfo.AssociatedMemberInfo.IsList && !string.IsNullOrEmpty(collection.MemberInfo.AssociatedMemberInfo.Name))
                                {
                                    collection.MemberInfo.AssociatedMemberInfo.SetValue(newObject, masterObj);
                                    bindingList = true;
                                }
                            }
                            if (!bindingList)
                                ((Module.BusinessObjects.INewObjectSession)newObject).Folder = masterObj as Module.BusinessObjects.Folder;
                        }
                    }
                }
                Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, newObject, objSpace);
                if (!View.ObjectSpace.IsModified)
                    Module.SystemObjects.Tools.RefreshGridView(View);
                //var refObjKey = View.ObjectSpace.GetKeyValue(newObject);
                //if (refObjKey != null)
                //{

                //    var refObject = View.ObjectSpace.GetObjectByKey(View.ObjectTypeInfo.Type, refObjKey);
                //    if (refObject != null && View is ListView)
                //        ((ListView)View).CollectionSource.Add(refObject);
                //}
            }
            else
            {
                // Tạo đối tượng mới trong ObjectSpace hiện tại
                var newObject = objSpace.CreateObject(typeof(Module.BusinessObjects.AccountEntry));
                // Lấy masterObject từ View
                var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
                if (masterObject != null)
                {
                    var masterObjKey = objSpace.GetKeyValue(masterObject);
                    if (masterObjKey != null)
                    {
                        var masterObj = objSpace.GetObjectByKey(masterObject.GetType(), masterObjKey);
                        if (masterObj != null)
                        {
                            // Gán giá trị từ masterObj
                            bool bindingList = false;
                            if (View is ListView)
                            {
                                var collection = ((PropertyCollectionSource)((ListView)View).CollectionSource);
                                if (collection.MemberInfo != null && collection.MemberInfo.AssociatedMemberInfo != null &&
                                    !collection.MemberInfo.AssociatedMemberInfo.IsList && !string.IsNullOrEmpty(collection.MemberInfo.AssociatedMemberInfo.Name))
                                {
                                    collection.MemberInfo.AssociatedMemberInfo.SetValue(newObject, masterObj);
                                    bindingList = true;
                                }
                            }
                            if (!bindingList)
                            {
                                ((Module.BusinessObjects.INewObjectSession)newObject).Folder = masterObj as Module.BusinessObjects.Folder;
                            }
                        }
                        var entryTplKey = e.SelectedChoiceActionItem.Data;
                        if (entryTplKey != null)
                        {
                            var entryTpl = objSpace.GetObjectByKey(typeof(Module.BusinessObjects.EntryTemplate), entryTplKey);
                            if (entryTpl != null)
                            {
                                // Gán giá trị từ entryTemplate vào đối tượng mới
                                ((Module.BusinessObjects.AccountEntry)newObject).Name = ((Module.BusinessObjects.EntryTemplate)entryTpl).AccountingTemplate.Name;
                                ((Module.BusinessObjects.AccountEntry)newObject).Debit = ((Module.BusinessObjects.EntryTemplate)entryTpl).Debit;
                                ((Module.BusinessObjects.AccountEntry)newObject).PartyAccountFolder = ((Module.BusinessObjects.EntryTemplate)entryTpl).PartyFolder;
                                ((Module.BusinessObjects.AccountEntry)newObject).EntryFolder = ((Module.BusinessObjects.EntryTemplate)entryTpl).EntryFolder;
                                var NewAmount = ((Module.BusinessObjects.EntryTemplate)entryTpl).Amount;
                                var masterObjType = masterObj.GetType();
                                var members = XafTypesInfo.Instance.FindTypeInfo(masterObjType).Members;
                                foreach (var member in members)

                                    if (member.BindingName == NewAmount.Value.ToString())
                                    {
                                        var AmountInfo = member;
                                        if (AmountInfo != null)
                                            ((Module.BusinessObjects.AccountEntry)newObject).Amount = (decimal?)AmountInfo.GetValue(masterObj);
                                        break;
                                    }
                            }
                        }
                    }
                    Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, newObject, objSpace);
                    if (!View.ObjectSpace.IsModified)
                    {
                        Module.SystemObjects.Tools.RefreshGridView(View);
                    }
                }
            }




            #endregion AccountingTemplateImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}