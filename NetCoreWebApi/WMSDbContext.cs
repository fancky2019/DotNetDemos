using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace NetCoreWebApi
{
    //在nuget 在线中安装EntityFramework
    //.net core 在nuget 在线中安装 Microsoft.EntityFrameworkCore 和  Microsoft.EntityFrameworkCore.SqlServer
    public class WMSDbContext : DbContext
    {

        public WMSDbContext(DbContextOptions options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  modelBuilder.Remove<PluralizingTableNameConvention>();//移除复数表名的契约

            ////数据库与实体映射--可以不设置
            //modelBuilder.Entity<Stock>().ToTable("Stock", "dbo");
            //modelBuilder.Entity<Sku>().ToTable("Sku", "dbo");
            //modelBuilder.Entity<BarCode>().ToTable("BarCode", "dbo");
            modelBuilder.Entity<Product>().ToTable("Product", "dbo");
            //modelBuilder.Entity<Product>().ToTable("Product", "dbo").Property(p => p.TimeStamp).IsRowVersion();
            //modelBuilder.Entity<Product>().ToTable("Product", "dbo").Property(p => p.ProductName).IsConcurrencyToken();单列并发控制
            ////联合主键
            //modelBuilder.Entity<PersonJob>().HasKey(t => new { t.PersonID, t.JobID }).ToTable("PersonJob", "dbo");
            //modelBuilder.Entity<Spouse>().ToTable("Spouse", "dbo");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"data source=(local);initial catalog=WMS;persist security info=True;user id=sa;password=123456");
       

        //public virtual DbSet<Sku> Sku { get; set; }
        //public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        //public virtual DbSet<BarCode> BarCode { get; set; }
        //public virtual DbSet<InOutStockDetail> InOutStockDetail { get; set; }
        //public virtual DbSet<InOutStock> InOutStock { get; set; }
        //public virtual DbSet<Sku> Sku { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">存储过程名称</param>
        /// <param name="parameters">SqlParameter</param>
        /// <returns></returns>
        public List<T> QueryByProcedure<T>(string sql, params SqlParameter[] parameters) where T : new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                var dr = cmd.ExecuteReader();
                var columnSchema = dr.GetColumnSchema();
                var data = new List<T>(); ;
                while (dr.Read())
                {
                    T item = new T();
                    Type type = item.GetType();
                    foreach (var kv in columnSchema)
                    {
                        var propertyInfo = type.GetProperty(kv.ColumnName);
                        if (kv.ColumnOrdinal.HasValue && propertyInfo != null)
                        {
                            //注意需要转换数据库中的DBNull类型
                            var value = dr.IsDBNull(kv.ColumnOrdinal.Value) ? null : dr.GetValue(kv.ColumnOrdinal.Value);
                            propertyInfo.SetValue(item, value);
                        }
                    }
                    data.Add(item);
                }
                // cmd.ExecuteScalar
                dr.Dispose();
                return data;
            }
        }

        public T ExecuteScalarByProcedure<T>(string sql, params SqlParameter[] parameters) where T : new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                var result = (T)cmd.ExecuteScalar();
                return result;
            }
        }
    }
}
