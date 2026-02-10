﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ENTOS.Module.SystemObjects
{
    public class CustomListTypeConverter : LocalizedClassInfoTypeConverter
    {
        public override List<Type> GetSourceCollection(ITypeDescriptorContext context)
        {
            HashSet<Type> source = ((!(SecuritySystem.Instance is ISupportSecurityTypeManager supportSecurityTypeManager)) ? new HashSet<Type>(base.GetSourceCollection(context)) : new HashSet<Type>(supportSecurityTypeManager.GetSecuredTypes()));
            //var result = source.Where(x => x.FullName.Contains("BusinessObjects")).ToList();
            var result = source.Where(x => FullNameIsValidated(x.Name)).ToList();
            return result;
        }

        private bool FullNameIsValidated(string fullName)
        {
            var names = new string[] { "Org", "Contact", "Product", "Post" };
            foreach(var name in names)
            {
                if (fullName.Equals(name))
                    return true;
            }
            return false;
        }
    }

}
