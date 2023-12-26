//zy


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.Enums
{
    //自己定义的数据类型，之后可以扩展，要想个办法说明某个通讯具体支持哪些数据类型
    public enum VaraType
    {

        /// <summary>
        /// S7 Bit variable type (bool)
        /// </summary>
        ///
        [Description("位Bit")]
        Bit,

        /// <summary>
        /// S7 Byte variable type (8 bits)
        /// </summary>
        /// 
        [Description("字节Byte")]
        Byte,

        /// <summary>
        /// S7 Word variable type (16 bits, 2 bytes)
        /// </summary>
        /// 
        [Description("字Word")]
        Word,

        /// <summary>
        /// S7 DWord variable type (32 bits, 4 bytes)
        /// </summary>
        /// 
        [Description("双字DWord")]
        DWord,

        /// <summary>
        /// S7 Int variable type (16 bits, 2 bytes)
        /// </summary>
        /// 
        [Description("整型Int")]
        Int,

        /// <summary>
        /// DInt variable type (32 bits, 4 bytes)
        /// </summary>
        /// 
        [Description("双整型DInt")]
        DInt,

        /// <summary>
        /// Real variable type (32 bits, 4 bytes)
        /// </summary>
        /// 
        [Description("实数Real")]
        Real,

        /// <summary>
        /// LReal variable type (64 bits, 8 bytes)
        /// </summary>
        /// 
        [Description("长实数LReal")]
        LReal,

        /// <summary>
        /// Char Array / C-String variable type (variable)
        /// </summary>
        /// 
        [Description("文本String")]
        String,

        /// <summary>
        /// S7 String variable type (variable)
        /// </summary>
        /// 
        [Description("S7文本S7String")]
        S7String,

        /// <summary>
        /// S7 WString variable type (variable)
        /// </summary>
        /// 
        [Description("S7W文本S7WString")]
        S7WString,

        /// <summary>
        /// Timer variable type
        /// </summary>
        /// 
        [Description("定时器Timer")]
        Timer,

        /// <summary>
        /// Counter variable type
        /// </summary>
        /// 
        [Description("计数器Counter")]
        Counter,

        /// <summary>
        /// DateTIme variable type
        /// </summary>
        /// 
        [Description("日期DateTime")]
        DateTime,

        /// <summary>
        /// DateTimeLong variable type
        /// </summary>
        /// 
        [Description("长日期DateTimeLong")]
        DateTimeLong


    }
}
