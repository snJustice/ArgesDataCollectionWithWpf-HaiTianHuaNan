using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication
{
    public interface IMonthProductionMessageApplication: IApplicationService
    {
        int InsertOrUpdateMonthProductionMessage(AddOrInsertMonthProductionMessageInput addOrInsertMonthProductionMessageInput);
        List<QuerryMonthProductionMessageOutput> QuerryMonthProductionMessageByYear(DateTime year);
        QuerryMonthProductionMessageOutput QuerryMonthProductionMessageByMonth(DateTime month);
    }
}
