//zy


using S7.Net.Types;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgesDataCollectionWithWpf.DbModels.Enums;

namespace ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel
{
    public class DataItemModel
    {//

        public int DataInDatabaseIndex { get; set; }


        public string DataAddressDescription { get; set; }//地址功能描述

        // 摘要:
        //     Memory area to read
        public DataType DataType { get; set; }

        //
        // 摘要:
        //     Type of data to be read (default is bytes)
        public VarType VarType { get; set; }

        //public EnumAddressFunction Func { get; set; }//地址的功能
        //
        // 摘要:
        //     Address of memory area to read (example: for DB1 this value is 1, for T45 this
        //     value is 45)
        public int DB { get; set; }

        //
        // 摘要:
        //     Address of the first byte to read
        public int StartByteAdr { get; set; }

        //
        // 摘要:
        //     Addess of bit to read from StartByteAdr
        public byte BitAdr { get; set; }

        //
        // 摘要:
        //     Number of variables to read
        public int Count { get; set; }

        //
        // 摘要:
        //     Contains the value of the memory area after the read has been executed
        public object? Value { get; set; }

        
        //
        // 摘要:
        //     Create an instance of DataItem
        public DataItemModel()
        {
            VarType = VarType.Byte;
            Count = 1;
        }

        

        //
        // 摘要:
        //     Create an instance of S7.Net.Types.DataItem from the supplied address.
        //
        // 参数:
        //   address:
        //     The address to create the DataItem for.
        //
        // 返回结果:
        //     A new S7.Net.Types.DataItem instance with properties parsed from address.
        //
        // 言论：
        //     The S7.Net.Types.DataItem.Count property is not parsed from the address.
        

        //
        // 摘要:
        //     Create an instance of S7.Net.Types.DataItem from the supplied address and value.
        //
        // 参数:
        //   address:
        //     The address to create the DataItem for.
        //
        //   value:
        //     The value to be applied to the DataItem.
        //
        // 返回结果:
        //     A new S7.Net.Types.DataItem instance with properties parsed from address and
        //     the supplied value set.
        

        
    }
}
