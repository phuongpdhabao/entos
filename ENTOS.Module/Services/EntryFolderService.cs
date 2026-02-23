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

    public partial class EntryFolderService : BaseService
    {

        public EntryFolderService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public EntryFolderService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3434ImportCode
                public static decimal CalculateTotalPropertyValue(EntryFolder folder, string choice)
        {
            decimal totalValue = 0;

            foreach (var accountEntry in folder.AccountEntryList)
            {
                totalValue += GetAccountEntryContribution(accountEntry, folder.EntryType, choice);
            }

            foreach (var partyAccount in folder.PartyAccountList)
            {
                totalValue += GetPartyAccountContribution(partyAccount, choice);
            }
            foreach (var childFolder in folder.LowerFolderList)
            {
                totalValue += CalculateTotalPropertyValue(childFolder, choice);
            }
            //if (folder.EntryType == EntryType.Debit)
            //{
            //    totalValue = -totalValue;
            //}
            folder.Quantity = totalValue;
            return totalValue;
	}

        #endregion SourceCode3434ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EntryTypeToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (EntryType != null) 
		//			return EntryType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PermissionPolicyRoleToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InActiveToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (InActive != null) 
		//			return InActive;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LowerFolderListToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (LowerFolderList != null) 
		//			return LowerFolderList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AccountEntryListToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (AccountEntryList != null) 
		//			return AccountEntryList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PartyAccountListToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (PartyAccountList != null) 
		//			return PartyAccountList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AllAccountEntryListToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (AllAccountEntryList != null) 
		//			return AllAccountEntryList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParentToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Parent != null) 
		//			return Parent;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ChildrenToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Children != null) 
		//			return Children;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Quantity != null) 
		//			return Quantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperFolderToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (UpperFolder != null) 
		//			return UpperFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberFolderToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (MemberFolder != null) 
		//			return MemberFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.EntryFolder entryfolder)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

	    #endregion
  

    }
}
