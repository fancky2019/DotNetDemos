using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.Reflection
{
    /*
     *[Job]
     *[Job("Job1")]
     *[Job("Job2", "1000", "")]
     *[Job(Name ="fancky",Salary ="1000",Description ="ke")]
     *    
     */
    [Job("Job1")]
    class Job1 : IJob
    {
        public void Starter()
        {
            Console.WriteLine("Job1.Starter()");
        }
    }
    [Job("Job2", 1000, "")]
    class Job2 : IJob
    {
        public void Starter()
        {
            Console.WriteLine("Job2.Starter()");
        }
    }

    [Job]
    class Job3
    {
    }

    [Job(Name ="fancky",Age =1000,Description ="ke")]
    class Job4
    {
    }
}
