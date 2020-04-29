using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApplication.Models
{
    public class Menus
    {

        public int ID { get; set; }

        public int ParentID { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
    }
}
