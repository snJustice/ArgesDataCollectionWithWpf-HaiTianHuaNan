using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel
{
    public interface IGetDataFromFile
    {
        DataTable GetDataTable();

        bool DataTableToFile(DataTable dt, string filepath);
    }
}
