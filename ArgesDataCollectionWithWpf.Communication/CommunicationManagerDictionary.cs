//zy


using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Communication.S7Communication;
using AutoMapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public class CommunicationManagerDictionary:ISingletonDependency
    {
        private readonly ICommunicationDetailsAndInstanceApplication _communicationDetailsAndInstanceApplication;
        private readonly IMapper _mapper;
        ConcurrentDictionary<int ,IDevice> _coms = new ConcurrentDictionary<int ,IDevice>();
        public CommunicationManagerDictionary(ICommunicationDetailsAndInstanceApplication  communicationDetailsAndInstanceApplication,IMapper mapper)
        {
            this._communicationDetailsAndInstanceApplication = communicationDetailsAndInstanceApplication;
            this._mapper = mapper;
        }

        public IDevice this[int i]
        {
            get { return this._coms[i]; }
            set { this._coms[i] = value; }
        }

        public void Clear()
        {
            _coms.Clear();
        }

        public void Close()
        {
            foreach (var item in _coms)
            {
                item.Value.Close();
            }
        }



        public void Load()
        {
            var currentCommunicationConfigs = this._communicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceAll();


            //this.Close();
            this.Clear();

            foreach (var item in currentCommunicationConfigs)
            {
                this._coms.TryAdd(item.ID, CommunicationFactoryFunc(item));
                
            }

        }

        public void Open()
        {
            Parallel.ForEach(this._coms.Values, async (xx) =>
            {

                await xx.OpenAsync();

            });
        }





        private IDevice CommunicationFactoryFunc(QuerryCommunicationDetailsAndInstanceOutput qcd)
        {
            switch (qcd.ConnectType)
            {
                case DbModels.Enums.ConnectType.Simens:
                    return new ArgesSimensS7Communication(qcd.SerialResult, this._mapper);
                    
                case DbModels.Enums.ConnectType.ModbusTCP:
                    throw new NotImplementedException("no such communication");
                    
                default:
                    throw new NotImplementedException("no such communication");
                    
            }
        }

    }
}
