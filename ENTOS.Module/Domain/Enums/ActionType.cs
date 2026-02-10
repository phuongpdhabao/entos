using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ActionType
    {
					[XafDisplayName("Simple Action")]
        SimpleAction,
					[XafDisplayName("Single Choice Action")]
        SingleChoiceAction,
					[XafDisplayName("Popup Window Show Action")]
        PopupWindowShowAction,
					[XafDisplayName("Parametrized Action")]
        ParametrizedAction,
					[XafDisplayName("Action Url")]
        ActionUrl,
	    }

}