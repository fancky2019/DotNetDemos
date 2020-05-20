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

        /// <summary>
        ///  绑定页面的ID用
        /// </summary>
        public string MenuName { get; set; }
        public string URL { get; set; }
        public string IcoName { get; set; }


        /// <summary>
        /// 显示用
        /// </summary>
        public string DisplayName { get; set; }
    }
}
