using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos
{
    class Config
    {

        public static string ConStr =>  ConfigurationManager.AppSettings["ConStr"];
        public static string WMSConnectionString => ConfigurationManager.ConnectionStrings["WMSConnectionString"].ConnectionString;
        static Config()
        {

        }
    }
}
