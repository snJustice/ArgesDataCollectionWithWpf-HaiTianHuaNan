using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.OtherModelDto
{
    public class LoadMaterialAreaAndDownMaterialDto
    {

        public LoadOrDwonEnum LoadOrDownArea { get; set; }
    }


    public enum LoadOrDwonEnum
    {
        LoadMaterialArea,
        DownMaterialArea,
    }
}
