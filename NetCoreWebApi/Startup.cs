using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace NetCoreWebApi
{
    public class Startup
    {
        private const string _securityKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
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



            //CS1591 缺少对公共可见类型或成员的 XML 注释
            //

            //添加Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DemoAPI", Version = "v1" });
                //添加xml文件 SwaggerUI  上显示接口注释
                //在项目属性--生成--勾选 生成xml文档
                //NetCoreWebApi\bin\Debug\netcoreapp2.1\NetCoreWebApi.xml
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetCoreWebApi.xml"));
            });

            //JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = "yourdomain.com",//Audience
                        ValidIssuer = "yourdomain.com",//Issuer，这两项和前面签发jwt的设置一致
                       ClockSkew = TimeSpan.Zero,//校验时间是否过期时，设置的时钟偏移量
                       //ClockSkew = TimeSpan.FromSeconds(15),//校验时间是否过期时，设置的时钟偏移量
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey)),//拿到SecurityKey
                    };

                   //o.TokenValidationParameters = new TokenValidationParameters
                   //{
                   //    NameClaimType = JwtClaimTypes.Name,
                   //    RoleClaimType = JwtClaimTypes.Role,

                   //    ValidIssuer = "http://localhost:5200",
                   //    ValidAudience = "api",
                   //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Consts.Secret))

                   //    /***********************************TokenValidationParameters的参数默认值***********************************/
                   //    // RequireSignedTokens = true,
                   //    // SaveSigninToken = false,
                   //    // ValidateActor = false,
                   //    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                   //    // ValidateAudience = true,
                   //    // ValidateIssuer = true, 
                   //    // ValidateIssuerSigningKey = false,
                   //    // 是否要求Token的Claims中必须包含Expires
                   //    // RequireExpirationTime = true,
                   //    // 允许的服务器时间偏移量
                   //    // ClockSkew = TimeSpan.FromSeconds(300),
                   //    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                   //    // ValidateLifetime = true
                   //};

               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 添加中间件组件到管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
            //启动JWT
            app.UseAuthentication();
            app.UseMvc();


            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI V1");
            });

            // 修改默认页面（Peoperties下的lunchSetting.json）：将LunchUrl改为“swagger”


            ////顺序
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseAuthentication();
            //app.UseSession();
            //app.UseMvc();
        }
    }
}
