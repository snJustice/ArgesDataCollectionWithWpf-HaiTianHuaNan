//zy


using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public interface ISocketHeart
    {
        Task<bool> ReConnnect();


        bool WriteHeart(DataItemModel socketDataitem);
    }
}
