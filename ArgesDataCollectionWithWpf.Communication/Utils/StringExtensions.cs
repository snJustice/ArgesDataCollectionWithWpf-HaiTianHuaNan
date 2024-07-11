//zy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication.Utils
{
    public static  class StringExtensions
    {
        public static string GetCharacters(this string target)
        {
            //string pattern = @"^[a-zA-Z0-9]+";
            string pattern = @"[DBMIQTC]+";

            Match m = Regex.Match(target, pattern);   // 匹配正则表达式，把this.textBox1.Text跟pattern正则对比

            if (m.Success)   // 判断输入的是不是英文和数字，不是进入
            {
                return m.Value;
            }
            else   //输入的是英文和数字
            {
                return "error";
            }

        }


        public static string GetNumberBeforePoint(this string target)
        {
            //string pattern = @"^[a-zA-Z0-9]+";
            string pattern = @"\.{0}[0-9]+";

            Match m = Regex.Match(target, pattern);   // 匹配正则表达式，把this.textBox1.Text跟pattern正则对比

            if (m.Success)   // 判断输入的是不是英文和数字，不是进入
            {
                return m.Value;
            }
            else   //输入的是英文和数字
            {
                return "error";
            }
        }


        public static string GetNumberMiddlePoint(this string target)
        {
            //string pattern = @"^[a-zA-Z0-9]+";
            string pattern = @"\.+[0-9]+\.+";

            Match m = Regex.Match(target, pattern);   // 匹配正则表达式，把this.textBox1.Text跟pattern正则对比

            if (m.Success)   // 判断输入的是不是英文和数字，不是进入
            {
                return m.Value.Replace(".","");
            }
            else   //输入的是英文和数字
            {
                return "error";
            }
        }


        public static string GetNumberAfterPoint(this string target)
        {
            //string pattern = @"^[a-zA-Z0-9]+";
            string pattern = @"\.+[0-9]+$";

            Match m = Regex.Match(target, pattern);   // 

            if (m.Success)   // 判断输入的是不是英文和数字，不是进入
            {
                return m.Value.Replace(".", "");
            }
            else   //输入的是英文和数字
            {
                return "error";
            }
        }


        public static string GetNumberWith_(this string target)
        {
            //string pattern = @"^[a-zA-Z0-9]+";
            string pattern = @"[0-9]+_[0-9]+";

            Match m = Regex.Match(target, pattern);   // 

            if (m.Success)   // 判断输入的是不是英文和数字，不是进入
            {
                return m.Value.Replace(".", "");
            }
            else   //输入的是英文和数字
            {
                return "error";
            }
        }
    }
}
