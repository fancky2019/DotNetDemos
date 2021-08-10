using CRM.Model.EntityModels.WMS;
using Demos.Common;
using Demos.Demos2019;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class AdoTest
    {
        //
        /*连接池默认开启：
         * System.Data.SqlClient.SqlConnectionString
         * internal sealed class SqlConnectionString : DbConnectionOptions
         * internal const bool Pooling = true;
         * 
         * 
         * 
         * 
         * SqlConnection 构造函数内将 （ConnectionString = connectionString;）connectionString赋值给属性ConnectionString，属性设置
         * 调用  ConnectionString_Set 其内部设置了 PoolGroup， 
         *  internal DbConnectionOptions ConnectionOptions => PoolGroup?.ConnectionOptions;
         *  
         *  SqlConnection构造函数内：
         * SqlConnectionString opt = (SqlConnectionString)ConnectionOptions;
         * 
         * 这就将数据库连接参数设置给了SqlConnectionString
         * public override string ConnectionString
        {
            get
            {
                return ConnectionString_Get();
            }
            set
            {
                if (_credential != null || _accessToken != null)
                {
                    SqlConnectionString sqlConnectionString = new SqlConnectionString(value);
                    if (_credential != null)
                    {
                        if (UsesActiveDirectoryIntegrated(sqlConnectionString))
                        {
                            throw SQL.SettingIntegratedWithCredential();
                        }
                        CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(sqlConnectionString);
                    }
                    else if (_accessToken != null)
                    {
                        CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken(sqlConnectionString);
                    }
                }
                ConnectionString_Set(new SqlConnectionPoolKey(value, _credential, _accessToken));
                _connectionString = value;
                CacheConnectionStringProperties();
            }
        }
         */
        public void Test()
        {
            try
            {
                //ConcurrentTransactionTest();
                ConcurrentTransactionException();
                //ParameterCommand();
                // Procedure();
                //ProcedureSingle();
                // ProcedureOutPutParam();
                // PageData();

                //SqlBulkCopyTest();
                //PrepareCommand();
                //ConnectionPool();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        private void ConcurrentTransactionTest()
        {
            Task.Run(() =>
            {
                ConcurrentTransaction();
            });
            Thread.Sleep(1000);
            Task.Run(() =>
            {
                ConcurrentTransaction(); 
            });

        }
        private void ConcurrentTransaction()
        {
            string insertCommand = $@"INSERT INTO [Demo].[dbo].[Person]([Name],[Age])
                                      VALUES (@Name,@Age)";
            var conString = "server=.;database=Demo;user=sa;pwd=123456";
            using (SqlConnection sqlConnection = new SqlConnection(conString))
            {
                SqlCommand sqlCommand = null;
                try
                {
                    Console.WriteLine($"Thread - {Thread.CurrentThread.ManagedThreadId} enter.");
                    Thread.Sleep(3000);
                    sqlCommand = new SqlCommand(insertCommand, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Name", "fancky");
                    sqlCommand.Parameters.AddWithValue("@Age", 27);
                    sqlConnection.Open();
                    sqlCommand.Transaction = sqlConnection.BeginTransaction();
                    int result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Transaction.Commit();
                    Console.WriteLine($"Thread - {Thread.CurrentThread.ManagedThreadId} done.");
                }
                catch (Exception ex)
                {
                    sqlCommand?.Transaction?.Rollback();
                    Console.WriteLine(ex.ToString());
                }

            }
        }


        private void ConcurrentTransactionException()
        {
            string insertCommand = $@"INSERT INTO [Demo].[dbo].[Person]([Name],[Age])
                                      VALUES (@Name,@Age)";
            var conString = "server=.;database=Demo;user=sa;pwd=123456";
            using (SqlConnection sqlConnection = new SqlConnection(conString))
            {
                SqlCommand sqlCommand = null;
                try
                {
                    Console.WriteLine($"Thread - {Thread.CurrentThread.ManagedThreadId} enter.");
                    Thread.Sleep(3000);
                    sqlCommand = new SqlCommand(insertCommand, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Name", "fancky");
                    sqlCommand.Parameters.AddWithValue("@Age", 27);
                    sqlConnection.Open();
                    sqlCommand.Transaction = sqlConnection.BeginTransaction();
                    int result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Transaction.Commit();//Commit 之后 Transaction=null
                    Console.WriteLine($"Thread - {Thread.CurrentThread.ManagedThreadId} done.");

                    // result = sqlCommand.ExecuteNonQuery();
                    //sqlCommand.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    sqlCommand?.Transaction?.Rollback();
                    Console.WriteLine(ex.ToString());
                }

            }
        }



        private void ParameterCommand()
        {

            string insertCommand = $@"INSERT INTO [Demo].[dbo].[Person]([Name],[Age])
                                      VALUES (@Name,@Age)";
            var conString = "server=.;database=Demo;user=sa;pwd=123456";
            using (SqlConnection sqlConnection = new SqlConnection(conString))
            {

                SqlCommand sqlCommand = new SqlCommand(insertCommand, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", "fancky");
                sqlCommand.Parameters.AddWithValue("@Age", 27);
                sqlConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 连接池默认开启
        /// 开启连接池会快几毫秒
        /// </summary>
        private void ConnectionPool()
        {
            //var conString = "server=.;database=test;user=sa;pwd=12345678;Connect Timeout=90;Connection Lifetime=30;Max Pool Size=100;Min Pool Size=0;Pooling=True";
            //var conString = "server=.;database=test;user=sa;pwd=12345678";
            //关闭连接池
            var conString = "server=.;database=test;user=sa;pwd=12345678;Pooling=False";
            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            for (int n = 0; n < 30; n++)
            {
                stopwatch.Restart();
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand mycommand = new SqlCommand())
                    {
                        mycommand.Connection = connection;

                        mycommand.CommandText = $"insert into Person (name,age) values ('message - {1}',3);";
                        //速度是快了
                        mycommand.Prepare();
                        mycommand.ExecuteNonQuery();


                    }
                }

                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// command.Prepare() 效果明显，减少耗时
        ///尽管command.Prepare()， 插入还是在100-200ms
        /// </summary>
        private void PrepareCommand()
        {
            var conString = "server=.;database=test;user=sa;pwd=12345678";
            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand mycommand = new SqlCommand())
                {
                    mycommand.Connection = connection;
                    /*
                     * 不能循环插，太慢，
                     */
                    for (int n = 0; n < 10; n++)
                    {
                        stopwatch.Restart();
                        mycommand.CommandText = $"insert into Person (name,age) values ('message - {1}',3);";
                        //速度是快了
                        mycommand.Prepare();
                        mycommand.ExecuteNonQuery();

                        //Thread.Sleep(1000);
                        stopwatch.Stop();
                        Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    }
                }
            }
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
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

        private void SqlBulkCopyTest()
        {

            //DataTable dataTable = SelectDatableStructFromDB("WMSData.dbo.Sku");
            //DataTableModels.FillDataTable<Sku>(GetSkus(), dataTable);
            //Stopwatch stopwatch = Stopwatch.StartNew();
            //stopwatch.Restart();
            //BatchInsert(dataTable, "WMSData.dbo.Sku");//1120ms
            //stopwatch.Stop();
            Stopwatch stopwatch = Stopwatch.StartNew();

            //List<DateTime> dateTimes = new List<DateTime>();
            //stopwatch.Restart();
            //for (int i = 0; i < 2000000; i++)
            //{
            //    dateTimes.Add(GetDateTime());
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"GroupBy() {stopwatch.ElapsedMilliseconds}ms");
            //var dis = dateTimes.Distinct().ToList();


            DataTable dataTable = SelectDatableStructFromDB("WMSData.dbo.Product");
            stopwatch.Restart();
            var dataList = GetProducts();//11748
            stopwatch.Stop();
            Console.WriteLine($"GetProducts() {stopwatch.ElapsedMilliseconds}ms");//11748


            stopwatch.Restart();
            var re = dataList.GroupBy(p => p.CreateTime).Select(p => new { CreateTime = p.Key, Count = p.Count() }).ToList();
            stopwatch.Stop();
            Console.WriteLine($"GroupBy() {stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Restart();
            DataTableModels.FillDataTable<Product>(dataList, dataTable);//8968
            stopwatch.Stop();
            Console.WriteLine($"FillDataTable() {stopwatch.ElapsedMilliseconds}ms");//8968


            //插入50W数据14063ms
            stopwatch.Restart();
            BatchInsert(dataTable, "WMSData.dbo.Product");//14063
            stopwatch.Stop();
            Console.WriteLine($"BatchInsert {stopwatch.ElapsedMilliseconds}ms");//14063
        }

        /// <summary>
        /// 批量插入
        /// Insert(column1,..Column5)values(),(),()这种一次插入多条。
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="destinationTableName"></param>
        private void BatchInsert(DataTable dataTable, string destinationTableName)
        {
            string connectionString = Config.WMSConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    //单位s:解决：执行已超时。完成操作之前
                    bulkCopy.BulkCopyTimeout = 72000;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.DestinationTableName = destinationTableName;
                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            dataTable.Dispose();
        }



        private static DataTable MakeTable()
        {
            DataTable newProducts = new DataTable("NewProducts");
            SqlCommand cmd;
            // Add three column objects to the table. 
            DataColumn productID = new DataColumn();
            productID.DataType = System.Type.GetType("System.Int32");
            productID.ColumnName = "ProductID";
            productID.AutoIncrement = true;
            newProducts.Columns.Add(productID);

            DataColumn productName = new DataColumn();
            productName.DataType = System.Type.GetType("System.String");
            productName.ColumnName = "Name";
            newProducts.Columns.Add(productName);

            DataColumn productNumber = new DataColumn();
            productNumber.DataType = System.Type.GetType("System.String");
            productNumber.ColumnName = "ProductNumber";
            newProducts.Columns.Add(productNumber);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = productID;
            newProducts.PrimaryKey = keys;

            // Add some new rows to the collection. 
            DataRow row = newProducts.NewRow();
            row["Name"] = "CC-101-WH";
            row["ProductNumber"] = "Cyclocomputer - White";

            newProducts.Rows.Add(row);
            row = newProducts.NewRow();
            row["Name"] = "CC-101-BK";
            row["ProductNumber"] = "Cyclocomputer - Black";

            newProducts.Rows.Add(row);
            row = newProducts.NewRow();
            row["Name"] = "CC-101-ST";
            row["ProductNumber"] = "Cyclocomputer - Stainless";
            newProducts.Rows.Add(row);
            newProducts.AcceptChanges();

            // Return the new DataTable. 
            return newProducts;
        }

        private DataTable SelectDatableStructFromDB(string tableName)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(Config.WMSConnectionString))
            {
                var timeout = con.ConnectionTimeout;
                //SqlConnection.ClearPool(con);

                var selectCommand = $"select  *  from {tableName} Where ID=-1;";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand, con);
                sqlDataAdapter.Fill(dataTable);
            }
            return dataTable;
        }

        //private void SetDataTableData(DataTable dataTable)
        //{
        //    var dataList = CreateProducts();
        //    DataTableModels.FillDataTable<Product>(dataList, dataTable);
        //}

        private List<Product> GetProducts()
        {
            List<Product> list = new List<Product>();
            Random random = new Random();

            for (int i = 0; i < 5000000; i++)
            {


                Product product = new Product();
                product.GUID = Guid.NewGuid();
                product.SkuID = random.Next(1, 10001);
                product.ProductName = AlterPassword.Instance.CreateNewPassword(random.Next(1, 17));
                product.ProductStyle = AlterPassword.Instance.CreateNewPassword(random.Next(1, 17));
                product.Price = random.Next(1, 10000);
                product.CreateTime = GetDateTime();
                product.Status = (short)random.Next(0, 2);
                product.Count = random.Next(1, 100000);
                product.ModifyTime = product.CreateTime;
                list.Add(product);
            }

            return list;
        }

        private DateTime GetDateTime()
        {
            DateTime dateTime = DateTime.Parse("1970-01-01 00:00:00");

            // Random 不设置seed 同一时刻会产生大量重复值
            //Random random = new Random();
            ////dateTime= dateTime.AddMonths(random.Next(0, 600));
            ////dateTime = dateTime.AddDays(random.Next(0, 30));
            ////dateTime = dateTime.AddHours(random.Next(0, 24));
            ////dateTime = dateTime.AddMinutes(random.Next(0, 60));
            ////dateTime = dateTime.AddSeconds(random.Next(0, 60));
            //dateTime = dateTime.AddSeconds(random.Next(0, 600 * 30 * 24 * 60 * 60));



            dateTime = dateTime.AddSeconds(RandomSeed.GetRandom(0, 600 * 30 * 24 * 60 * 60));
            return dateTime;
        }




        private List<Sku> GetSkus()
        {
            List<Sku> list = new List<Sku>();
            for (int i = 0; i < 10000; i++)
            {
                Sku sku = new Sku();
                sku.Unit = $"个{i.ToString()}";
                sku.GUID = Guid.NewGuid();
                list.Add(sku);
            }
            return list;
        }





    }
}
