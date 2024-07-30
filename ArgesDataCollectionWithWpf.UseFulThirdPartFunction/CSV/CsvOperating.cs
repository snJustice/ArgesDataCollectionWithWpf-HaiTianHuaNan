using ArgesDataCollectionWithWpf.Core.Utils;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UseFulThirdPartFunction.CSV
{
    public class CsvOperating : IGetDataFromFile
    {
        private readonly string _fileName;

        public CsvOperating(string fileName)
        {
            this._fileName = fileName;
        }
        public bool DataTableToFile(DataTable dt, string filepath)
        {

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            try
            {
                List<string> headername = new List<string>();
                string header = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    headername.Add(dt.Columns[i].ColumnName);
                    
                }

                
                header = String.Join(";", headername);
                CustomerCsvHelper.WriteHeader(filepath, header);

                List<string> valueNames = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    valueNames.Clear();
                    string data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        
                        valueNames.Add(dt.Rows[i][j].ToString());
                    }
                    data = String.Join(";", valueNames);
                    CustomerCsvHelper.WriteOneLine(filepath, data);
                }

                return true;
            }
            catch (Exception ex )
            {

                return false;
            }
            
        }

        public DataTable GetDataTable()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string[] lines = System.IO.File.ReadAllLines(this._fileName, Encoding.GetEncoding("GB2312"));
            int count = lines.Count();
            if (count<=0)
            {
                return null;
            }


            DataTable dataTable = new DataTable();
            var columnsName = lines[0].Split(",");
            int columsCount = columnsName.Count(); 
            for (int i = 0; i < columsCount; i++)
            {
                dataTable.Columns.Add(columnsName[i]);  
            }
            int rowsCount = count - 1;
            for (int i = 0; i < rowsCount; i++)
            {
                var newRow = dataTable.NewRow();
                var datas = lines[i + 1].Split(",") ;
                for (int j = 0; j < datas.Count(); j++)
                {
                    newRow[j] = datas[j];
                }

                dataTable.Rows.Add(newRow);


            }

            return dataTable;


        }
    }
}
