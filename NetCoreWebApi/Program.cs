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
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

              //添加其他的配置文件
              //通过WebHostBuilder将我们添加的json文件添加进asp.net core的配置
              .ConfigureAppConfiguration((webHostBuilderContext, iConfigurationBuilder) => {
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
    }
}
