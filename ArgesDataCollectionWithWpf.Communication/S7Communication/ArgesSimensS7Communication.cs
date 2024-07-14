//zy
/*
 * 数据的地址 DB1000.10.1 ,数据类型再自己定义
 * 
 * 
 * 
 */

using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel.SimensS7;
using AutoMapper;
using Castle.Windsor.Installer;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication.S7Communication
{
    public class ArgesSimensS7Communication: CommunicationBase, IConnected, IDevice, ISender, ISocketHeart
    {
        //要有一种地址数据的解析方式,和s7的数据地址类型匹配
        private PlcSimensConnectionPara _para;
        private Plc s7Plc;


        public event Action ReConnectOKEvent;



        public ArgesSimensS7Communication(byte[] deserializeData, IMapper mapper, PlcSimensConnectionPara para = null) :base(deserializeData, mapper) 
        {
            if (para != null)
            {
                this._para = para;
            }
            else
            {
                this._para = (PlcSimensConnectionPara)Deserialize();
            }

            this.s7Plc = new Plc(this._para.CpuType, this._para.PLCIPAddress, this._para.PLCPort, (short)this._para.Rack, (short)this._para.Slot);
              
            

            

        }

        public bool IsConnected { get ; set ; }

        public bool Close()
        {
            this.s7Plc.Close();
            this.IsConnected = false;
            return true;
        }

        public Task<bool> CloseAsync()
        {
            return Task.Run(() => { return TaskTimeOutExtensions.TaskTimeOut<bool>(CloseAsyncCus(), 500); });
        }

        private async  Task<bool> CloseAsyncCus()
        {
            await Task.Delay(2);
            return Close();
        }


        public bool Open()
        {

            try
            {
                IsConnected = false;
                this.s7Plc.Open();
                IsConnected = true;
                return true;    
            }
            catch (Exception)
            {

                IsConnected = false;
                return false;
            }
        }

        public Task<bool> OpenAsync()
        {
            return Task.Run(() => { return TaskTimeOutExtensions.TaskTimeOut(OpenAsyncCus(), 1000); });
        }

        private async Task<bool> OpenAsyncCus()
        {
            try
            {
                IsConnected = false;
                await Task.Delay(1);
                this.s7Plc.Open();
                IsConnected = true;

            }
            catch (Exception)
            {
                

            }
            return true;



        }

        public bool SendData(List<DataItemModel> dataItemModels)
        {
            var s7Dataitem = from m in dataItemModels select this._mapper.Map<DataItem>(m);

            try
            {
                s7Plc.Write(s7Dataitem.ToArray());
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            

            
        }

        public List<DataItemModel> GetData(List<DataItemModel> dataItemModels)
        {

            var s7Dataitem = (from m in dataItemModels select this._mapper.Map<DataItem>(m)).ToList();

            s7Plc.ReadMultipleVars(s7Dataitem);

            var resultModels = (from m in s7Dataitem select this._mapper.Map<DataItemModel>(m)).ToList();

            for (int i = 0; i < s7Dataitem.Count(); i++)
            {
                resultModels[i].DataInDatabaseIndex = dataItemModels[i].DataInDatabaseIndex;
            }
            return resultModels.ToList();
        }

        public bool ClearData(List<DataItemModel> dataItemModels)
        {
            foreach (var item in dataItemModels)
            {
                item.Value = null;
            }

            return true;
        }

        public Task<bool> ReConnnect()
        {

            return Task.Run(() => { return TaskTimeOutExtensions.TaskTimeOut(OpenAsyncCus(), 500); });
            
            
        }


        

        public bool WriteHeart(DataItemModel socketDataitem)
        {

            //如果心跳信号写入失败，则认为断开连接，需要重新进行连接
            try
            {
                this.s7Plc. Write(this._mapper.Map<DataItem>(socketDataitem));
                return true;
                
            }
            catch (Exception ex)
            {
                
                
                this.IsConnected= false;
                return false;
            }
            

            


        }


        
        public DataItemModel GetData(DataItemModel dataItemModel)
        {
            //S7.Net.PlcException,读取触发地址会比心跳块，所以不能知道到底是否还存在连接
            var s7Dataitem = this._mapper.Map<DataItem>(dataItemModel);
            try
            {
                if (IsConnected)
                {
                    

                    s7Dataitem.Value = s7Plc.Read(s7Dataitem.DataType, s7Dataitem.DB, s7Dataitem.StartByteAdr, s7Dataitem.VarType, s7Dataitem.Count, s7Dataitem.BitAdr);


                    return this._mapper.Map<DataItemModel>(s7Dataitem); ;
                }
                else
                {
                    return dataItemModel;
                }
            }
            catch (S7.Net.PlcException ex )
            {
                return dataItemModel;


            }
            
            
        }
    }
}
