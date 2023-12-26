//zy


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EnumReadOrWrite
    {
        [Description("读-read")]
        Read = 0,


        [Description("写-write")]
        Write = 1
    }
}
