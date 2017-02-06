using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LiXinSync
{
    public class Test
    {
        private DataAccess da;

        /// <summary>
        /// HR同步的字符串连接
        /// </summary>
        protected string HrConnectStr = "";

        /// <summary>
        /// 指纹库同步的字符串连接
        /// </summary>
        protected string UserFingerConnectStr = "";

        /// <summary>
        /// 原培训系统指纹库同步的字符串连接
        /// </summary>
        protected string TrainUserFingerConnectStr = "server=192.168.4.20;database=LiXinLMS_HR;uid=sa;pwd=123456";

        public Test()
        {
            HrConnectStr = ConfigurationManager.AppSettings["HRSqlConnection"];
            UserFingerConnectStr = ConfigurationManager.AppSettings["UserFingerSqlConnection"];
            //TrainUserFingerConnectStr = ConfigurationManager.AppSettings["TrainUserFingerSqlConnection"];
        }
        /// <summary>
        /// 同步人员的指纹信息
        /// </summary>
        public bool AddTest(byte[] image)
        {
            try
            {
                using (var conn = new SqlConnection(TrainUserFingerConnectStr))
                {
                    var cmd = new SqlCommand(string.Format("update TEMPLATE set TEMPLATE4=@image"), conn);
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("image", SqlDbType.Image) {Value = image});
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
