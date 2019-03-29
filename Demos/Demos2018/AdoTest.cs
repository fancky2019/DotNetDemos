using Demos.Common;
using Demos.Demos2018.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Demos.Demos2018
{
    class AdoTest
    {
        public void Test()
        {
            try
            {
                // Procedure();
                //ProcedureSingle();
                // ProcedureOutPutParam();
                PageData();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        private void Procedure()
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection(Config.ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                //存储过程名
                cmd.CommandText = "GetProductProc";
                cmd.Parameters.AddWithValue("@price", 13);
                cmd.Parameters.AddWithValue("@productName", "jdbc");
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt = new DataTable();
                    dt.Load(reader);
                }
            }
            //DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            var list = dt.ToList<Product>();
        }

        //存储过程 return  返回
        private void ProcedureSingle()
        {
            using (SqlConnection con = new SqlConnection(Config.ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                //存储过程名
                cmd.CommandText = "UpdateProductProc";
                cmd.Parameters.AddWithValue("@productName", "jdbc");
                cmd.Parameters.AddWithValue("@id", 49);
                SqlParameter parReturn = new SqlParameter("@parReturn", SqlDbType.Int);
                parReturn.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(parReturn);
                cmd.ExecuteNonQuery();
                var val = parReturn.Value;
                //由于结果是select 1;返回受影响的行数被 SET NOCOUNT ON;
                //re=-1
                //int re = (int)cmd.ExecuteNonQuery();
            }

        }

        //存储过程输出参数
        private void ProcedureOutPutParam()
        {
            using (SqlConnection con = new SqlConnection(Config.ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //   cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                //存储过程名
                cmd.CommandText = "UpdateProductOutParamProc";
                cmd.Parameters.AddWithValue("@productName", "jdbc");
                cmd.Parameters.AddWithValue("@id", 49);
                SqlParameter sqlParameter = new SqlParameter("@result", SqlDbType.Int);
                sqlParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParameter);
                cmd.ExecuteNonQuery();
                var rrr = sqlParameter.Value;
                var param = cmd.Parameters["@result"].Value;
                //由于结果是select 1;返回受影响的行数被 SET NOCOUNT ON;
                //re=-1
                //int re = (int)cmd.ExecuteNonQuery();


            }

        }

        private void PageData()
        {
            using (SqlConnection con = new SqlConnection(Config.ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //   cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                //存储过程名
                cmd.CommandText = "pageData";
                cmd.Parameters.AddWithValue("@pageIndex", 2);
                cmd.Parameters.AddWithValue("@pageSize", 15);
                SqlParameter sqlParameter = new SqlParameter("@totalCount", SqlDbType.Int);
                sqlParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParameter);
                DataTable dt = new DataTable();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                dt.Load(sqlDataReader);
                var list = dt.ToList<Product>();
                var rrr = sqlParameter.Value;
                var param = cmd.Parameters["@totalCount"].Value;
                //由于结果是select 1;返回受影响的行数被 SET NOCOUNT ON;
                //re=-1
                //int re = (int)cmd.ExecuteNonQuery();
            }

        }
    }
}
