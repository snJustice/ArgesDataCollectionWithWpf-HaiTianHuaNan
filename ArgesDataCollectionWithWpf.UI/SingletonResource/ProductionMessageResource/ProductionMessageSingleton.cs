using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.SingletonResource.ProductionMessageResource
{
    public class ProductionMessageSingleton: ISingletonDependency
    {
        private readonly IDayProductionMessageApplication _dayProductionMessageApplication;
        private readonly IMonthProductionMessageApplication _monthProductionMessageApplication;

        public QuerryDayProductionMessageOutput DayProduction { get; set; }
        public QuerryMonthProductionMessageOutput MonthProduction { get; set; }

       
        public ProductionMessageSingleton(IDayProductionMessageApplication dayProductionMessageApplication, IMonthProductionMessageApplication monthProductionMessageApplication)
        {
            this._dayProductionMessageApplication = dayProductionMessageApplication;
            this._monthProductionMessageApplication = monthProductionMessageApplication;


         


            var currentDayCount  = this._dayProductionMessageApplication.QuerryDayProductionMessageByDay(DateTime.Now);
            if (currentDayCount.ID <=-1)
            {
                //新增加一条
                this._dayProductionMessageApplication.InsertOrUpdateDayProductionMessage(
                    
                    new AddOrInsertDayProductionMessageInput { 
                    
                        DayCount=0,
                        Time=DateTime.Now
                    }
                    );

                this.DayProduction = this._dayProductionMessageApplication.QuerryDayProductionMessageByDay(DateTime.Now); 
            }
            else
            {
                this.DayProduction = currentDayCount;
            }



            var currentMonthCount = this._monthProductionMessageApplication.QuerryMonthProductionMessageByMonth(DateTime.Now);
            if (currentMonthCount.ID <= -1)
            {
                //新增加一条
                this._monthProductionMessageApplication.InsertOrUpdateMonthProductionMessage(

                    new AddOrInsertMonthProductionMessageInput
                    {

                        MonthCount = 0,
                        Time = DateTime.Now
                    }
                    );

                this.MonthProduction = _monthProductionMessageApplication.QuerryMonthProductionMessageByMonth(DateTime.Now);
            }
            else
            {
                this.MonthProduction = currentMonthCount;
            }



        }
    
   
    
        public int AddDayAndMonthProduction()
        {
            try
            {
                //表示日产量要增加一条新记录
                if (this.DayProduction.Time.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    this._dayProductionMessageApplication.InsertOrUpdateDayProductionMessage(

                        new AddOrInsertDayProductionMessageInput
                        {
                            DayCount = 1,
                            Time = DateTime.Now
                        }
                        );

                    this.DayProduction = this._dayProductionMessageApplication.QuerryDayProductionMessageByDay(DateTime.Now);
                }

                else
                {
                    this.DayProduction.DayCount++;
                    this._dayProductionMessageApplication.InsertOrUpdateDayProductionMessage(new AddOrInsertDayProductionMessageInput
                    {


                        ID = this.DayProduction.ID,
                        DayCount = this.DayProduction.DayCount,
                        Time = this.DayProduction.Time
                    });
                }



                //表示月产量要增加一条新记录
                if (this.MonthProduction.Time.ToString("yyyy-MM") != DateTime.Now.ToString("yyyy-MM"))
                {
                    this._monthProductionMessageApplication.InsertOrUpdateMonthProductionMessage(

                        new AddOrInsertMonthProductionMessageInput
                        {
                            MonthCount = 1,
                            Time = DateTime.Now
                        }
                        );

                    this.MonthProduction = this._monthProductionMessageApplication.QuerryMonthProductionMessageByMonth(DateTime.Now);
                }
                else
                {
                    this.MonthProduction.MonthCount++;
                    this._monthProductionMessageApplication.InsertOrUpdateMonthProductionMessage(new AddOrInsertMonthProductionMessageInput
                    {


                        ID = this.MonthProduction.ID,
                        MonthCount = this.MonthProduction.MonthCount,
                        Time = this.MonthProduction.Time
                    });
                }


                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
            


        }
    
    }
}
