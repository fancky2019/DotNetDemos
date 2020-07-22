using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.SQLite
{
    public class SQLiteDemo
    {
        private string _conString = "";
        public SQLiteDemo()
        {
            var dbName = ConfigurationManager.AppSettings["SQLiteDb"];
            //Password=myPassword;UseUTF16Encoding=True;
            _conString = $"Data Source={System.Environment.CurrentDirectory}\\SQLite\\{dbName};version=3;UseUTF16Encoding=True;Pooling=False;Max Pool Size=100;";
        }
        public void Test()
        {
            InsertSingle();
            //InsertBatch();
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
