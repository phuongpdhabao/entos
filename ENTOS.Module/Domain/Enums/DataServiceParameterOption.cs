using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataServiceParameterOption
    {
					[XafDisplayName(" ")]
        Empty,
					[XafDisplayName("Default Request Headers")]
        DefaultRequestHeaders,
					[XafDisplayName("Headers")]
        Headers,
					[XafDisplayName("Multipart FormData")]
        MultipartFormData,
					[XafDisplayName("Form Urlencoded")]
        FormUrlencoded,
					[XafDisplayName("Json")]
        Json,
					[XafDisplayName("Text")]
        Text,
					[XafDisplayName("Xml")]
        Xml,
					[XafDisplayName("AuthenticationHeaderValue")]
        AuthenticationHeaderValue,
					[XafDisplayName("File")]
        FileMultipartFormData,
	    }

}