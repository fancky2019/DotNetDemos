using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace NetCoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //在nuget 在线中安装EntityFramework
            //.net core 在nuget 在线中安装 Microsoft.EntityFrameworkCore 和  Microsoft.EntityFrameworkCore.SqlServer
            //安装对应版本和项目的.net Core 版本一致
            services.AddDbContext<WMSDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:WMSConnectionString:ConnectionString"]);
            });

            //services.AddSingleton<IConfig, Config>(p => new Config(Configuration));
            //services.AddSingleton<IConfig, Config>(p => typeof(Config));


            //添加Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DemoAPI", Version = "v1" });
                //添加xml文件 SwaggerUI  上显示接口注释
                //在项目属性--生成--勾选 生成xml文档
                //NetCoreWebApi\bin\Debug\netcoreapp2.1\NetCoreWebApi.xml
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetCoreWebApi.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();


            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI V1");
            });

           // 修改默认页面（Peoperties下的lunchSetting.json）：将LunchUrl改为“swagger”

        }
    }
}
