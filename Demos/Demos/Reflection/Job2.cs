using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos.Reflection
{
    [Job("Job2")]
    class Job2 : IJob
    {
        public void Starter()
        {
            Console.WriteLine("Job2.Starter()");
        }
    }
}
