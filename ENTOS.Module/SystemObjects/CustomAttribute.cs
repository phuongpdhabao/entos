using System;

namespace ENTOS.Module.SystemObjects
{

    [System.AttributeUsage(System.AttributeTargets.Class |System.AttributeTargets.Struct,AllowMultiple = true)]
    public class CustomFilter : System.Attribute
    {
        public string Name;
        public string Criteria;

        public CustomFilter()
        {

        }
        public CustomFilter(string name, string criteria)
        {
            this.Name = name;
            this.Criteria = criteria;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class CustomRadioGroupValues : System.Attribute
    {
        public string Name;
        public bool? Value;

        public CustomRadioGroupValues()
        {
            
        }
        public CustomRadioGroupValues(string name)
        {
            Name = name;
        }
        public CustomRadioGroupValues(string name, bool value)
        {
            Name = name;
            this.Value = value;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class CustomLinkUnLinkAttribute : System.Attribute
    {
        public string Name;
        public Type Type;
        public string Field;
        public string ViewId;
        public string Criteria;        
        public bool Existed;
        public bool InvertSelect;

        public CustomLinkUnLinkAttribute()
        {

        }

        public CustomLinkUnLinkAttribute(string name, Type type, string field, string viewId, string criteria, bool existed)
        {
            this.Name = name;
            this.Type = type;
            this.Field = field;
            this.ViewId = viewId;
            this.Criteria = criteria;
            this.Existed = existed;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class CustomUnLinkAttribute : System.Attribute
    {
        public string ViewId;
        public string Criteria;

        public CustomUnLinkAttribute()
        {

        }

        public CustomUnLinkAttribute(string viewId, string criteria)
        {
            this.ViewId = viewId;
            this.Criteria = criteria;
        }
    }


    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class AllowSetDefaultAttribute : System.Attribute
    {
        public string TargetItems;
        public string Context;
        public AllowSetDefaultAttribute()
        {

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class ShowToolTipAttribute : System.Attribute
    {
        public string TargetItems;
        public string Context;
        public ShowToolTipAttribute()
        {

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class StatusColorAttribute : System.Attribute
    {
        public string TargetItems;
        public string Context;
        public StatusColorAttribute()
        {

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    //Bao gồm thuộc tính kèm theo là DataSourceCriteriaAttribute
    public class AllowChoiceItemsAttribute : System.Attribute
    {   
        public string TargetItems;
        public string ViewId;
        public AllowChoiceItemsAttribute()
        {

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    //Bao gồm thuộc tính kèm theo là DataSourceCriteriaAttribute
    public class AddTextItemsAttribute : System.Attribute
    {
        public string TargetItems;
        public string ViewId;
        public AddTextItemsAttribute()
        {

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class MobileColumnAttribute : System.Attribute
    {
        public string TargetItems;
        public string Context;
        public MobileColumnAttribute()
        {

        }
    }

}