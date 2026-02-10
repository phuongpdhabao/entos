using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AddressType
    {
					[XafDisplayName("Địa chỉ")]
        Address,
					[XafDisplayName("Email")]
        Email,
					[XafDisplayName("Phone")]
        Phone,
					[XafDisplayName("Mobile")]
        Mobile,
					[XafDisplayName("Zalo")]
        Zalo,
					[XafDisplayName("Facebook")]
        Facebook,
					[XafDisplayName("Whatsapp")]
        Whatsapp,
					[XafDisplayName("Viber")]
        Viber,
					[XafDisplayName("Telegram")]
        Telegram,
					[XafDisplayName("Google")]
        Google,
					[XafDisplayName("Apple")]
        Apple,
					[XafDisplayName("Microsoft")]
        Microsoft,
	    }

}