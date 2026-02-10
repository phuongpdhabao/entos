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

    public partial class AccountEntryService : BaseService
    {

        public AccountEntryService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public AccountEntryService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4517ImportCode
                private System.Collections.Generic.IList<AccountingTemplate> allAccoutingTemplates = null;
        public void GetDefaultEntryMenu(ViewController viewController,  DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction)
        {
            if (viewController.View is null)
                return;

            var masterObject = Tools.GetMasterObjectFromView(viewController.View) as Order;
            if (masterObject is null)
                return;
            if (allAccoutingTemplates == null)
                allAccoutingTemplates = viewController.View.ObjectSpace.GetObjects<AccountingTemplate>(new DevExpress.Data.Filtering.BinaryOperator("ObjectType", masterObject.GetType()));
            var listAccoutingTemplates = allAccoutingTemplates;
            if (listAccoutingTemplates == null)
                return;
            foreach (var accoutingTemplate in listAccoutingTemplates)
            {
                foreach (var entryTemplate in accoutingTemplate.EntryTemplates)
                {
                    singleChoiceAction.Items.Add(new DevExpress.ExpressApp.Actions.ChoiceActionItem(entryTemplate.EntryFolder.Name, entryTemplate.Oid));
                }
            }


        }
        #endregion SourceCode4517ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AmountToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Amount != null) 
		//			return Amount;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EntryFolderToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (EntryFolder != null) 
		//			return EntryFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PartyAccountFolderToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (PartyAccountFolder != null) 
		//			return PartyAccountFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DebitToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Debit != null) 
		//			return Debit;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DateToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Date != null) 
		//			return Date;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberFolderToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (MemberFolder != null) 
		//			return MemberFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinkToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Link != null) 
		//			return Link;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Book1ToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Book1 != null) 
		//			return Book1;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Book2ToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Book2 != null) 
		//			return Book2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InActiveToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (InActive != null) 
		//			return InActive;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object StatisticToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Statistic != null) 
		//			return Statistic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PartyStatisticToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (PartyStatistic != null) 
		//			return PartyStatistic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatorToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Creator != null) 
		//			return Creator;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AmountDebitToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (AmountDebit != null) 
		//			return AmountDebit;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AmountCreditToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (AmountCredit != null) 
		//			return AmountCredit;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ConsumeToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Consume != null) 
		//			return Consume;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AssetToolTipControllerText(View view, Module.BusinessObjects.AccountEntry accountentry)
        //{
        //    if (Asset != null) 
		//			return Asset;
        //    return null;
        //}
    

	    #endregion
  

    }
}
