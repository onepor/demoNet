using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Base
{
    public class Mysql_Conn
    {
        private static string ConnectionString1 = ConfigurationManager.AppSettings["DBConnection"];//add
        public static string ConnectionString()
        {

            string Sqlon = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            return Sqlon;

        }

        /// <summary>
        /// SQLsugar Connect  --Mysql
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient Conn()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString(),//必填, 数据库连接字符串
                DbType = DbType.MySql,         //必填, 数据库类型
                IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.SystemTable    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
            });
            return db;
        }

        /// <summary>
        /// SQLsugar Connect  --Sqlserver
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient BusinessDBConn()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SFY_BusinessDB"].ConnectionString,//必填, 数据库连接字符串
                DbType = DbType.SqlServer,         //必填, 数据库类型
                IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.SystemTable    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
            });
            return db;
        }
        /// <summary>
        /// SQLsugar Connect  --Sqlserver
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient ManagerDBConn()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SFY_ManagerDB"].ConnectionString,//必填, 数据库连接字符串
                DbType = DbType.SqlServer,         //必填, 数据库类型
                IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.SystemTable    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
            });
            return db;
        }
    }
}
