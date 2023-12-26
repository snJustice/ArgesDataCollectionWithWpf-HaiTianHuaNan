//zy


using ArgesDataCollectionWithWpf.Application.SerializeApplication;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public abstract class CommunicationBase
    {
        private readonly byte[] _deserializeData;
        protected readonly IMapper _mapper;

        public CommunicationBase(byte[] deserializeData, IMapper mapper)
        {
            this._deserializeData = deserializeData;
            this._mapper = mapper;
        }

        protected object Deserialize()
        {
            return CustomerSerialize.DeSerializeBinaryToObject(this._deserializeData);
        }
    }
}
