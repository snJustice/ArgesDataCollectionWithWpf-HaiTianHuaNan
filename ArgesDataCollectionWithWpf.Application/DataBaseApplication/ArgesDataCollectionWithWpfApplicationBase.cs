//zy


using ArgesDataCollectionWithWpf.Core;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication
{
    public abstract class ArgesDataCollectionWithWpfApplicationBase
    {
        protected readonly DbContextConnection _dbContextClinet;
        protected readonly ILogger _logger;
        protected readonly IMapper _objectMapper;
        public ArgesDataCollectionWithWpfApplicationBase(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper)
        {
            _logger = logger;
            _dbContextClinet = sugarClinet;
            _objectMapper = objectMapper;
        }
    }
}
