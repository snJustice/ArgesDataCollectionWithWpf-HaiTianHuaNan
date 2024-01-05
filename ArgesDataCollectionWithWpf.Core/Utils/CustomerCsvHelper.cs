using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Core.Utils
{
    public class CustomerCsvHelper
    {

        public static void CreateFile(string FileName)
        {
            if (FileName.Contains(".csv"))
            {


                if (!File.Exists(FileName))
                {
                    using (File.Create(FileName))
                    {

                    }
                }
            }
            else
            {
                throw new Exception("不是csv文件，无法创建");
            }


        }

        public static bool IsExistFile(string FileName)
        {

            return File.Exists(FileName);

        }

        public static void WriteHeader(string _fileName, string header)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var writer = new StreamWriter(_fileName, true, Encoding.GetEncoding("GB2312"));
            using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))//,CultureInfo.InvariantCulture
            {
                //csv.Configuration.HasHeaderRecord = false;





                csv.WriteField(header.Split(';'));

                csv.NextRecord();


            }
        }

        public static void WriteOneLine(string _fileName, string data)
        {




            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);//CultureInfo.InvariantCulture
            csvConfiguration.Encoding = Encoding.Unicode;

            var writer = new StreamWriter(_fileName, true, Encoding.GetEncoding("GB2312"));//
            using (var csv = new CsvWriter(writer, csvConfiguration))
            {

                //csv.Configuration.HasHeaderRecord = false;
                //csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.None;

                csv.WriteField(data.Split(';'));


                csv.NextRecord();




            }
        }

    }
}
