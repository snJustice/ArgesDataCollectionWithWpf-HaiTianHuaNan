using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication
{
    public interface IModlingCodesApplication: IApplicationService
    {
        int DeleteModlingCodesByContainerNo(DeleteModlingCodesInput deleteModlingCodesInput);

        QuerryModlingCodesOutput QuerryModlingCodesByTimeEarlyAndNotDone(DateTime startDate, DateTime endDate, string stationNumber);

        int InsertModlingCodes(AddModlingCodesInputs addModlingCodesInputs);

        int UpdateModlingCodesIsDoneByContainerNo(UpdateModlingCodesInput updateModlingCodesInput);
    }
}
