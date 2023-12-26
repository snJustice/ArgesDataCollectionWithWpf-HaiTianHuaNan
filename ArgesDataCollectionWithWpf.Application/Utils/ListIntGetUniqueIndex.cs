//zy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.Utils
{
    public static class ListIntGetUniqueIndex
    {
        public static int GetUniqueIndex<T>(this IEnumerable<T> objects,string propertyName)
        {
            int index = -1;
            Type aa = typeof(T);
            //获得ID属性
            var idpp = (from m in aa.GetProperties() where m.Name == propertyName select m).FirstOrDefault();
            HashSet<int> haaa = new HashSet<int>();
            if (idpp!=null)
            {
                foreach (var item in objects)
                {
                    haaa.Add(Convert.ToInt32(idpp.GetValue(item)));
                }
            }


            for (int i = 1; i < 10000; i++)
            {
                if (haaa.Contains(i))
                {

                }
                else
                {
                    index = i;
                    break;
                }
            }

            return index;

        }
    }
}
