using ArgesDataCollectionWithWpf.DbModels.Models;
using SqlSugar;
using System.Data;
using System.Reflection;

namespace ArgesDataCollectionWithWpf.CodeFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //数据库链接字符串
            //string ConnectionString2 = "DataSource=" + Environment.CurrentDirectory + @"\DB\\testDb.db";
            string ConnectionString2 = "Host=127.0.0.1;Port=5432;User Id=postgres;Password=123456@Qq;Database=HT_HN_data;";

            ConnectionConfig config2 = new ConnectionConfig()
            {
                ConnectionString = ConnectionString2,
                DbType = SqlSugar.DbType.PostgreSQL,

                InitKeyType = InitKeyType.Attribute, //初始化主键和自增列信息到ORM的方式,
                MoreSettings = new ConnMoreSettings()
                {
                    EnableCodeFirstUpdatePrecision = true,//启用
                                                          //5.1.4.1.6-preview17+
                }
            };

            



            //通过实体生成数据库 
            Assembly assembly = Assembly.LoadFrom("ArgesDataCollectionWithWpf.DbModels.dll");
            IEnumerable<Type> typelist

                = assembly.GetTypes().Where(c => (!string.IsNullOrWhiteSpace(c.Namespace)) 
                && (c.Namespace.StartsWith("ArgesDataCollectionWithWpf.DbModels.Models") || c.Namespace.StartsWith("ArgesDataCollectionWithWpf.DbModels.DataBaseModels") )
                && c.IsClass
                && c.GetCustomAttribute(typeof(MappingToDatabaseAttribute)) !=null);

            bool Backup = false;  //是否备份


            using (SqlSugarClient Client = new SqlSugarClient(config2))
            {

                Client.Aop.OnLogExecuting = (sql, par) =>
                {
                    Console.WriteLine($"sql语句：{sql}");
                };

                //Client.DbMaintenance.CreateDatabase(); //创建一个数据库出来
                if (Backup)
                {
                    Client.CodeFirst.BackupTable().InitTables(typelist.ToArray());
                }
                else
                {
                    Client.CodeFirst.InitTables(typelist.ToArray());
                    //Client.CodeFirst.(typelist.ToArray());
                }
            }
        }
    }
}