using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Common.IDatabase;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Database.Sqlserver
{
    public class Database : IGetDatabase
    {
        #region 属性
        public IDbConnection db { get; set; }
        public IDbTransaction dbTransaction { get; set; }
        #endregion
        public Database(string connString)
        {
            var obj = ConfigurationManager.ConnectionStrings[connString];
            string connectionStr = obj == null ? connString : obj.ConnectionString;
            db = new SqlConnection(connectionStr);
        }
        public void BeginTrans()
        {
            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }
            dbTransaction = db.BeginTransaction(IsolationLevel.Serializable);
        }

        public void Close()
        {
            db.Dispose();
        }
        public void Rollback()
        {
            this.dbTransaction.Rollback();
            this.dbTransaction.Dispose();
            this.Close();
        }
        public void Commit()
        {
            try
            {
                dbTransaction.Commit();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dbTransaction == null)
                {
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 执行插入，更新，删除等操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParamter">参数</param>
        /// <returns></returns>
        public int ExecuteNoneQuery(string sql,object dbParamter=null)
        {
            if (dbTransaction != null)
            {
               return  db.Execute(sql, dbParamter, dbTransaction, null, CommandType.Text);
            }else
            {
                return db.Execute(sql, dbParamter, null, null, CommandType.Text);
            }
       
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName)
        {
            if (dbTransaction != null)
            {
                return db.Execute(procName, null, dbTransaction, null, CommandType.StoredProcedure);
            }
            else
            {
                return db.Execute(procName, null, dbTransaction, null, CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// 执行存储过程名称
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, object dbParameter)
        {
            if (dbTransaction != null)
            {
                return db.Execute(procName, dbParameter, dbTransaction, null, CommandType.StoredProcedure);
            }else
            {
                return db.Execute(procName, dbParameter, null, null, CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteBysql(string sql, object dbParameter = null)
        {
            if (dbTransaction != null)
            {
                return db.Execute(sql, dbParameter, dbTransaction);
            }
            else
            {
                return db.Execute(sql, dbParameter);
            }

        }
        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbParamter">参数</param>
        /// <returns></returns>
        public IEnumerable<T> FindList<T>(string sql,object dbParamter=null) where T : class
        {
            if (dbTransaction != null)
            {
                return db.Query<T>(sql, dbParamter, dbTransaction);
            }
            else
            {
                return db.Query<T>(sql, dbParamter);
            }
           
        }
        /// <summary>
        /// 寻找一个实体
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public T FindEntity<T>(string strSql, object dbParameter = null) where T : class, new()
        {
            var data = db.Query<T>(strSql, dbParameter);
            return data.FirstOrDefault();
        }
    }
}
