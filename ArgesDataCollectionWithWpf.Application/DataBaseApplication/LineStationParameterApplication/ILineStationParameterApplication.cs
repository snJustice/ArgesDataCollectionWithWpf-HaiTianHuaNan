using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication
{
    public interface ILineStationParameterApplication : IApplicationService
    {
        //插入
        int InsertLineStationParameter(AddLineStationParameterInput addLineStationParameterInput);

        //查询所有
        List<QuerryLineStationParameterOutput> QuerryAllLineStationParameters();


        //删除
        int DeleteLineStationParameterByID(DeleteLineStationParameterByIdInput deleteLineStationParameterByIdInput);



        int ModifyLineStationParameterById(ModifyLineStationParameterInput modifyLineStationParameterInput);
    }
}
