using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel
{
    public interface IExcelGetData
    {
        DataTable GetDataTable();

        bool DataTableToExcel(DataTable dt, string filepath);
    }
}
