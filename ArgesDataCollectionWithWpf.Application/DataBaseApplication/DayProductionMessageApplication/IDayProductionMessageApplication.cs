using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication
{
    public interface IDayProductionMessageApplication: IApplicationService
    {
        int InsertOrUpdateDayProductionMessage(AddOrInsertDayProductionMessageInput addOrInsertDayProductionMessageInput);

        List<QuerryDayProductionMessageOutput> QuerryDayProductionMessageByMonth(DateTime Month);
        QuerryDayProductionMessageOutput QuerryDayProductionMessageByDay(DateTime day);
    }
}
