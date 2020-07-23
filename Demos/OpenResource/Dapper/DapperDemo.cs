using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Demos.OpenResource.Dapper
{
    /// <summary>
    /// A high performance Micro-ORM supporting SQL Server, MySQL, Sqlite, SqlCE, Firebird etc..
    /// </summary>
    public class DapperDemo
    {

        private string _conString_SQLite = "";
        public DapperDemo()
        {
            var dbName = ConfigurationManager.AppSettings["SQLiteDb"];
            //Password=myPassword;UseUTF16Encoding=True;
            _conString_SQLite = $"Data Source={System.Environment.CurrentDirectory}\\SQLite\\{dbName};version=3;UseUTF16Encoding=True;Pooling=False;Max Pool Size=100;";
        }
        public void Test()
        {
            Insert();
        }

        private void Insert()
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            using (SQLiteConnection connection = new SQLiteConnection(_conString_SQLite))
            {
                //connection.Execute("insert into Person(Name,Remark) values(@Name,@Remark)", person);
                var insertCommand = $"insert into Person (Name) values ('message - {1}');";
                connection.Execute(insertCommand);

             
                //connection.Query<Person>("select *  from Person");
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

    }
}
