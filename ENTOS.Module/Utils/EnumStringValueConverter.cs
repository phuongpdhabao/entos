using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Module.Utils
{

    public class EnumStringValueConverter<TEnum> : ValueConverter where TEnum : struct, Enum
    {
        public override Type StorageType => typeof(string);

        public override object ConvertFromStorageType(object value)
        {
            if (value is string str && Enum.TryParse<TEnum>(str, out var result))
                return result;
            return default(TEnum);
        }

        public override object ConvertToStorageType(object value)
        {
            return value?.ToString();

        }
    }

}
