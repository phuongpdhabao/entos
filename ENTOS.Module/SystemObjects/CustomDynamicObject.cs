using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using System.ComponentModel;

[DomainComponent]
[ModelDefault("Caption", "Đối tượng động")]
[DefaultProperty("DisplayName")]
public class CustomDynamicObject : NonPersistentLiteObject
{
    private object marker;

    [Browsable(false)]
    public object Marker => marker;

    [DevExpress.Xpo.DisplayName("Tên hiển thị"), ToolTip("Tên hiển thị")]
    public string DisplayName { get; private set; }

    public CustomDynamicObject(object marker)
    {
        Guard.ArgumentNotNull(marker, "marker");
        this.marker = marker;
        DisplayName = CaptionHelper.GetDisplayText(marker);
    }

    public CustomDynamicObject(object marker, string displayName)
    {
        Guard.ArgumentNotNull(marker, "marker");
        this.marker = marker;
        DisplayName = displayName;
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static CustomDynamicObject GetMarkerObjectFromMarkerValue(string markerValue, Type type, IObjectSpace objectSpace)
    {
        CustomDynamicObject result = null;
        if (type != null && type.IsEnum)
        {
            if (markerValue != null)
            {
                try
                {
                    object obj = Enum.Parse(type, markerValue);
                    if (obj != null)
                    {
                        result = new CustomDynamicObject(obj);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        else
        {
            try
            {
                if (!string.IsNullOrEmpty(markerValue))
                {
                    result = new CustomDynamicObject(objectSpace.GetObjectByHandle(markerValue));
                }
            }
            catch (Exception)
            {
            }
        }

        return result;
    }
}
//Để dùng cần 2 trường sau
//[Browsable(false)]
//[Size(-1)]
//[ObjectValidatorIgnoreIssue(new Type[] { typeof(ObjectValidatorLargeNonDelayedMember) })]
//public string MarkerValue
//{
//    get
//    {
//        return GetPropertyValue<string>("MarkerValue");
//    }
//    set
//    {
//        SetPropertyValue("MarkerValue", value);
//    }
//}

//private IObjectSpace _objectSpace;
//private IObjectSpace GetObjectSpace()
//{
//    if (_objectSpace is null)
//        _objectSpace = DevExpress.ExpressApp.Xpo.XPObjectSpace.FindObjectSpaceByObject(this);
//    return _objectSpace;
//}


//[NonPersistent]
//[DataSourceProperty("AvailableMarkerObjects", new string[] { })]
//[ImmediatePostData]
//public CustomDynamicObject Marker
//{
//    get
//    {
//        return Module.SystemObjects.Tools.GetMarkerObjectFromMarkerValue(MarkerValue, this.GetType(), GetObjectSpace());
//    }
//    set
//    {
//        MarkerValue = Module.SystemObjects.Tools.GetMarkerValueFromMarkerObject(value, this.GetType(), GetObjectSpace());
//        OnChanged("Caption");
//    }
//}