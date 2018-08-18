using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace  Common.IDatabase
{
    public interface IGetDatabase
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        void BeginTrans();
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        void Commit();
        /// <summary>
        /// 回滚操作
        /// </summary>
        void Rollback();
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
        #region 执行sql语句
        /// <summary>
        /// 执行带参数的sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        int ExecuteBysql(string sql, object dbParameter);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        int ExecuteByProc(string procName);
        int ExecuteByProc(string procName, object dbParameter);

        #endregion
        #region 对实体的增删改查
        int ExecuteNoneQuery(string sql, object dbParamter=null);
        IEnumerable<T> FindList<T>(string sql,object dbParamter=null) where T : class;
        T FindEntity<T>(string strSql, object dbParameter=null) where T : class, new();
        #endregion
    }
}
