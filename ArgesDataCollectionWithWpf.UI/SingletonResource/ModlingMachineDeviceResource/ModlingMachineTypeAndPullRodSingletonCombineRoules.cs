using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.Utils;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.CSV;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;

namespace ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource
{
    public class ModlingMachineTypeAndPullRodSingletonCombineRoules:ISingletonDependency
    {

        private readonly IAppConfigureRead _appConfigRead;
        public ModlingMachineTypeAndPullRodSingletonCombineRoules(IAppConfigureRead appConfigRead)
        {
            this._appConfigRead = appConfigRead;
            ModlingMachineTypeAndPullRodCombines = new Dictionary<string, ModlingMachineTypeAndPullRodCombine>();
            InitRoules();
        }

        public Dictionary<string,ModlingMachineTypeAndPullRodCombine> ModlingMachineTypeAndPullRodCombines { get; set; }


        public void InitRoules()
        {
            IGetDataFromFile excel = new CsvOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"));
            var tabless = excel.GetDataTable();


            for (int i = 0; i < tabless.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(tabless.Rows[i][1].ToString()))
                {
                    continue;
                }
                string key = tabless.Rows[i][1].ToString().Trim();
                if (ModlingMachineTypeAndPullRodCombines.ContainsKey(key))
                {
                    ModlingMachineTypeAndPullRodCombines[key].PollRods.Add(
                        new PollRodType
                        {
                            PollRodDescription = tabless.Rows[i][6].ToString(),
                            PollRodSendToPlcID = Convert.ToInt32(tabless.Rows[i][7].ToString())
                        });
                }
                else
                {
                    ModlingMachineTypeAndPullRodCombines.Add(key, new ModlingMachineTypeAndPullRodCombine
                    {

                        ModlingMachineTypeName = key,
                        ModlingMachineTypeSendToPlcID = Convert.ToInt32(tabless.Rows[i][0].ToString()),
                        PollRods = new List<PollRodType> { new PollRodType { PollRodDescription = tabless.Rows[i][6].ToString() ,PollRodSendToPlcID = Convert.ToInt32(tabless.Rows[i][7].ToString()) } }



                    }); ; 
                }
            }

            
        }

        public void Clear()
        {
            ModlingMachineTypeAndPullRodCombines.Clear();
        }
    }
}
