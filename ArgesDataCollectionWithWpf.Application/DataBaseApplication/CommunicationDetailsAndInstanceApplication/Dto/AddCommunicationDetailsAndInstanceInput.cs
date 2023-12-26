//zy


using ArgesDataCollectionWithWpf.DbModels.Enums;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto
{
    public class AddCommunicationDetailsAndInstanceInput
    {
        public int ID { get; set; }

        public ConnectType ConnectType { get; set; }

        //唯一的识别code
        public string UniqueCode { get; set; }

        //序列化后的参数结果
        public byte[] SerialResult { get; set; }
    }
}
