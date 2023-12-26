//zy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication.Utils
{
    public static  class TaskTimeOutExtensions
    {
        public static async Task<T> TaskTimeOut<T>(this Task<T> action, double time)
        {
            //Task<T> targetTask = new Task<T>(action);
            //targetTask.Start();

            Task timeTask = Task.Delay(TimeSpan.FromMilliseconds(time));



            Task resultTask = await Task.WhenAny(Task.Delay(Convert.ToInt32(time)), action);


            if (resultTask == action)
            {
                return await action.ConfigureAwait(false);
            }
            else
            {
                return default(T);
            }


        }
    }
}
