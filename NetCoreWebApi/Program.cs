using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetCoreWebApi
{
    public class Program
    {
        /*
         * 无法启动dotnet.exe
         * 系统环境变量的Path中添加";C:\Program Files\dotnet"
         * 
         * swagger:
         * nuget 安装"Swashbuckle.AspNetCore"
         * */
        public static void Main(string[] args)
        {
            NewConfiguratio();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

              //添加其他的配置文件
              //通过WebHostBuilder将我们添加的json文件添加进asp.net core的配置
              .ConfigureAppConfiguration((webHostBuilderContext, iConfigurationBuilder) =>
              {
                  iConfigurationBuilder.SetBasePath(webHostBuilderContext.HostingEnvironment.ContentRootPath)
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("hostsettings.json", optional: true);


              })
             .UseShutdownTimeout(TimeSpan.FromSeconds(10))
             //.ConfigureAppConfiguration((iConfigurationBuilder) => {

             //    iConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
             //            .AddJsonFile("hostsettings.json", optional: true)
             //            .AddJsonFile("Ocelot.json");
             //})
             .UseSetting("https_port", "8080")
             .UseStartup<Startup>();
        //指定端口，不然在centos中部署外网访问不到
        //    .UseStartup<Startup>().UseUrls("http://*:5000;https://*:5001");


        public static void NewConfiguratio()
        {
            var builder = new ConfigurationBuilder();
            //将配置节点添加到内存
            //builder.AddInMemoryCollection();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            //设置配置文件
            builder.AddJsonFile("appsettings.json");
            builder.AddJsonFile("appsettingsextention.json");

            IConfiguration configuration = builder.Build();
            //添加key到内存
            //configuration["con"] = "val";   
            var str = configuration["ConnectionStrings:WMSConnectionString:ConnectionString"];
            var stt = configuration["Swagger:Enable"];
            //获取的都是string:   索引：string this[string key]
            var bo = configuration["Swagger:EnableBool"];
            var num = configuration["Swagger:EnableCount"];

        }
    }
}
