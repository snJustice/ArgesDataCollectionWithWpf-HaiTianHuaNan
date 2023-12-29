using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationTableApplication
{
    public interface ILineStationTableApplication: IApplicationService
    {
        int InsertTable(string tableName);
        
    }
}
