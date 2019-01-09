using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos.Reflection
{
    [Job("Job1")]
    class Job1 : IJob
    {
        public void Starter()
        {
            Console.WriteLine("Job1.Starter()");
        }
    }
}
