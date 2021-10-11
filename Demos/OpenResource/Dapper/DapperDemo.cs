using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Demos.Model;

namespace Demos.OpenResource.Dapper
{
    /*
     * github:https://github.com/StackExchange/Dapper
     */



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
            //Insert();

            ParameterCommand();
        }

        private void Insert()
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            //String insertCommand = "insert into Person(name)values('fancky1');";
            using (SQLiteConnection connection = new SQLiteConnection(_conString_SQLite))
            {
                //参数赋值
                //connection.Execute("insert into Person(Name,Remark) values(@Name,@Remark)", person);
                //connection.Execute("insert into Person(Name,Remark) values(@Name,@Remark)", new { Name="fancky", Remark = "Remark" });
                var insertCommand = $"insert into Person (Name) values ('message - {1}');";
                connection.Execute(insertCommand);


                //connection.Query<Person>("select *  from Person");
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        private void ParameterCommand()
        {
            string insertCommand = $@"INSERT INTO [Demo].[dbo].[Person]([Name],[Age])
                                      VALUES (@Name,@Age)";
            var conString = "server=.;database=Demo;user=sa;pwd=123456";
            Person person = new Person();
            person.Name = "li";
            person.Age = 25;
            using (SqlConnection connection = new SqlConnection(conString))
            {

                var obj = new { Name = "匿名", Age = 27 };
                var r1 = connection.Execute(insertCommand, obj);


                var r2 = connection.Execute(insertCommand, person);


                DynamicParameters dynamicParameters = new DynamicParameters();
                //注意参数名前没有@,ado.net 的SqlParameter是要加@的。详情见AdoTest
                dynamicParameters.Add("Name", "dynamicParameters");
                dynamicParameters.Add("Age", 28);
                var r3 = connection.Execute(insertCommand, dynamicParameters);


                //connection.Query<Person>("select *  from Person");
            }
        }

    }
}
