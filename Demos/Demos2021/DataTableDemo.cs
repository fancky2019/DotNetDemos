using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021
{
    class DataTableDemo
    {
        public void Test()
        {

        }

        private void  Fum()
        {
            DataTable dt = new DataTable();
            List<string> sysUserGuidList = dt.AsEnumerable().Select(c => c.Field<string>("SysUserGuid")).ToList();
        }
    }
}
