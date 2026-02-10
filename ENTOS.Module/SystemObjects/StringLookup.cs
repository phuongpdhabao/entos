﻿using System;
using System.ComponentModel;
using System.Globalization;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.Metadata;

namespace ENTOS.Module.SystemObjects
{
    [DomainComponent]
    public class StringLookup
    {
        public StringLookup(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Nhãn")]
        public string Name { get; set; }

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Mã")]
        public object Value { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class StringLookupToStringConverter : ValueConverter
    {
        public override object ConvertFromStorageType(object objectType)
        {                   
            if (objectType != null)
            {
                
                var values = ((string)objectType).Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (values.Length > 0)
                {
                    var result = new StringLookup(values[0], null);
                    if (values.Length > 1)
                        result.Value = values[1];
                    return result;
                }
            }                                   
            return null;            
            //return (object)new LookupString((string)stringObjectType);
        }

        public override object ConvertToStorageType(object objectType)
        {
            if (objectType == null)
                return (object)null;
            if (objectType is StringLookup)
            {
                return ((StringLookup)objectType).Name + System.Environment.NewLine + ((StringLookup)objectType).Value;
            }
            else if (objectType is string)
            {
                return (string) objectType + System.Environment.NewLine + (string) objectType;
            }
            
            return null;
        }

        public override Type StorageType
        {
            get
            {                
                return typeof(string);
            }
        }
    }

    public class MemberInfoToStringConverter : ValueConverter
    {
        public override object ConvertFromStorageType(object stringObjectType)
        {
            if (stringObjectType != null)
            {
                var parserMember = ((string)stringObjectType).Split('-');
                if (parserMember.Length == 2)
                {
                    var typeInfo = ReflectionHelper.FindTypeInfoByName(parserMember[0]);
                    if (typeInfo != null)
                    {
                        return typeInfo.FindMember(parserMember[1]);
                    }
                }
            }           
            return null;
        }

        public override object ConvertToStorageType(object objectType)
        {
            if (objectType == null)
                return (object)null;
            return (object)((IMemberInfo)objectType).ToString();
        }

        public override Type StorageType
        {
            get
            {
                return typeof(string);
            }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class MemberInfoConverterAttribute : Attribute
    {
        public static readonly TypeConverterAttribute Default = new TypeConverterAttribute();
        private string typeName;

        public MemberInfoConverterAttribute()
        {
            this.typeName = string.Empty;
        }

        public MemberInfoConverterAttribute(Type type)
        {
            this.typeName = type.AssemblyQualifiedName;
        }

        public MemberInfoConverterAttribute(string typeName)
        {
            typeName.ToUpper(CultureInfo.InvariantCulture);
            this.typeName = typeName;
        }

        public string ConverterTypeName
        {
            get
            {
                return this.typeName;
            }
        }

        public override bool Equals(object obj)
        {
            TypeConverterAttribute converterAttribute = obj as TypeConverterAttribute;
            if (converterAttribute != null)
                return converterAttribute.ConverterTypeName == this.typeName;
            return false;
        }

        public override int GetHashCode()
        {
            return this.typeName.GetHashCode();
        }
    }
}