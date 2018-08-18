using Common.IDatabase;
using Database.Sqlserver;
using IDatabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public class DbFactory
    {
        public static IGetDatabase GetDatabase()
        {
            string providerName = ConfigurationManager.ConnectionStrings["BaseDb"].ProviderName;
            return GetDbType(providerName);
        }
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="providerName">驱动名称</param>
        /// <returns></returns>
        private static IGetDatabase GetDbType(string providerName)
        {
            IGetDatabase database;
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    database = new Database.Sqlserver.Database("BaseDb");
                    break;
                case "MySql.Data.MySqlClient"://预留另外数据库的链接接口
                    database = null;
                    break;
                case "Oracle.ManagedDataAccess.Client":
                    database = null;
                    break;
                default:
                    database = new Database.Sqlserver.Database("BaseDb");
                    break;
            }
            return database;
        }
    }
}
