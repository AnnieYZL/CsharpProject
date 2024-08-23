using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SV21T1080067.DataLayers.SQLServer
{
    /// <summary>
    /// Lớp đóng vai trò là lớp cha cho các lớp cài đặt các phép xử lí dữ liệu trên CSDL SQL Server
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối đến CSDL SQL Server
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
