using Demos.Common;
using Demos.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos
{
    class AdoTest
    {
        public void Test()
        {
            try
            {
                // Procedure();
                ProcedureSingle();
               // ProcedureOutPutParam();
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
    }
}
