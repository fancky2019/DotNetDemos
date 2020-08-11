using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    class AssemblyDemo
    {

        static void Test()
        {
            ////程序集太多会有问题
            ////var types=  AppDomain.CurrentDomain.GetAssemblies()
            ////.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITradeService))))
            ////.ToList();

            //ITradeService tradeService = null;
            //Assembly assembly = Assembly.GetAssembly(ITradeService.GetType());
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //var types = assembly.GetTypes();
            //var tradeServiceName = Configurations.Configuration["ZDFixService:ITradeService"];
            //var currentService = types.Where(p => p.Name == tradeServiceName).FirstOrDefault();
            //if (currentService != null)
            //{
            //    tradeService = (ITradeService)currentService.CreateInstance();
            //    tradeService = currentService.CreateInstance<ITradeService>();
            //    var serviceName = tradeService.GetType().Name;
            //}


        }
    }

}
