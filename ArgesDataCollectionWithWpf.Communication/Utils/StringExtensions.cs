//zy


using S7.Net;
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
            if (!target.StartsWith("DB"))
            {
                return target;
            }

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
            if (!target.StartsWith("DB"))
            {
                return target;
            }

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
            if (!target.StartsWith("DB"))
            {
                return target;
            }

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
            if (!target.StartsWith("DB"))
            {
                return target;
            }

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

            //if (!target.StartsWith("DB"))
            //{
            //    return target;
            //}

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



        public static object CastingTargetType(this string target, VarType varType)
        {
            object value = 0;
            switch (varType)
            {
                case VarType.Bit:
                    break;
                case VarType.Byte:
                    break;
                case VarType.Word:
                    try
                    {
                        value = (ushort)Convert.ToUInt32(target);
                    }
                    catch (Exception)
                    {


                    }
                    break;
                case VarType.DWord:
                    try
                    {
                        value = Convert.ToUInt32(target);
                    }
                    catch (Exception)
                    {


                    }
                    break;
                case VarType.Int:
                    try
                    {
                        value =  (ushort)Convert.ToInt32(target);
                    }
                    catch (Exception)
                    {

                        
                    }
                    break;
                case VarType.DInt:
                    try
                    {
                        value = Convert.ToInt32(target);
                    }
                    catch (Exception)
                    {


                    }
                    break;
                case VarType.Real:
                    try
                    {
                        value = Convert.ToSingle(target);
                    }
                    catch (Exception)
                    {


                    }
                    break;
                case VarType.LReal:
                    try
                    {
                        value = Convert.ToDouble(target);
                    }
                    catch (Exception)
                    {


                    }
                    break;
                case VarType.String:
                    value = target.ToString();
                    break;
                case VarType.S7String:
                    value = target.ToString();
                    break;
                case VarType.S7WString:
                    break;
                case VarType.Timer:
                    break;
                case VarType.Counter:
                    break;
                case VarType.DateTime:
                    break;
                case VarType.DateTimeLong:
                    break;
                default:
                    break;

                   
            }
            return value;
        }



        public static string GetDBName(this string target)
        {
            if (target.Contains("."))
            {
                return target.Split('.')[0];
            }
            else
            {
                return target;
            }
        }
        public static string GetDBUnit(this string target)
        {
            if (target.Contains("."))
            {
                return target.Split('.')[1];
            }
            else
            {
                return "";
            }
        }
    }
}
