using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.SQLite
{
    /*
     * NUGET:System.Data.SQLite
     * 如果只用ADO.NET则安装System.Data.SQLite.Core
     */
    public class SQLiteDemo
    {
        private string _conString = "";
        public SQLiteDemo()
        {
            var dbName = ConfigurationManager.AppSettings["SQLiteDb"];
            //Password=myPassword;UseUTF16Encoding=True;
            _conString = $"Data Source={System.Environment.CurrentDirectory}\\SQLite\\{dbName};version=3;UseUTF16Encoding=True;Pooling=true;Max Pool Size=100;";
        }
        public void Test()
        {
            //InsertSingle();
            //InsertBatch();
            PrepareCommand();
        }


        private void InsertSingle()
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            using (SQLiteConnection connection = new SQLiteConnection(_conString))
            {
                connection.Open();

                using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                {
                    /*
                     * 不能循环插，太慢，用事务批量.插入耗时470ms
                     */
                    //for (int n = 0; n < 10000; n++)
                    //{
                    mycommand.CommandText = $"insert into Person (name) values ('message - {1}');";
                    mycommand.ExecuteNonQuery();
                    //}
                }
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// command.Prepare() 效果明显，减少耗时
        ///尽管command.Prepare()， 插入还是在100-200ms
        /// </summary>
        private void PrepareCommand()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            using (SQLiteConnection connection = new SQLiteConnection(_conString))
            {
                connection.Open();

                using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                {
                 
                    /*
                     * 不能循环插，太慢，用事务批量.插入耗时470ms
                     */
                    for (int n = 0; n < 10; n++)
                    {
                        stopwatch.Restart();
                        mycommand.CommandText = $"insert into Person (name) values ('message - {1}');";
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

        /// <summary>
        /// 开启事务批量插入10W耗时2s左右
        /// </summary>
        private void InsertBatch()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (SQLiteConnection myconnection = new SQLiteConnection(_conString))
            {
                myconnection.Open();
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        SQLiteParameter myparam = new SQLiteParameter();
                        int n;

                        //mycommand.CommandText = "INSERT INTO [MyTable] ([MyId]) VALUES(?)";
                        mycommand.CommandText = $"insert into Person (name) values(?);";
                        mycommand.Parameters.Add(myparam);

                        for (n = 0; n < 100000; n++)
                        {
                            myparam.Value = $"message - {n}";
                            mycommand.ExecuteNonQuery();
                        }
                    }
                    mytransaction.Commit();
                }
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }


        #region
        #endregion
        #region
        #endregion
        #region
        #endregion
    }
}
