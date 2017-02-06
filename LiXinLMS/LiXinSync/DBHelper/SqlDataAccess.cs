using System.Data;
using System.Data.SqlClient;

namespace LiXinSync
{
    internal class SqlDataAccess : DataAccess
    {
        public SqlDataAccess(string connectString)
        {
            ConnectString = connectString;
        }

        private string ConnectString;

        internal override DataSet GetDataSet(string sql)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        internal override int ExecuteSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                return cmd.ExecuteNonQuery();

            }
        }

        internal override object ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

    }
}
