using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public interface ISender
    {
        bool SendData(List<DataItemModel > dataItemModels);

        List<DataItemModel> GetData(List<DataItemModel> dataItemModels);
        DataItemModel GetData(DataItemModel dataItemModel);

        bool ClearData(List<DataItemModel> dataItemModels);
    }
}
