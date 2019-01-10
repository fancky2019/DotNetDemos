using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Demos.Demos.Reflection
{
    class AttributeDemo
    {
        public void Test()
        {
            GetJobs()?.ForEach(p =>
            {
                p.Starter();
            });
        }

        private List<IJob> GetJobs()
        {
            //Type iJobType = typeof(IJob);
            //Type job1Type = typeof(Job1);
            ////反射判断Type是否实现了接口，或继承
           // typeof(IJob).IsAssignableFrom(job1Type);
            //typeof(IJob).IsAssignableFrom(typeof(Job1));

            ////反射判断继承
            ////typeof(ChildClass).IsSubclassOf(typeof(ParentClass))

            //Assembly assembly = Assembly.Load("Demos");
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> jobTypeList = new List<Type>();
            List<IJob> iJobList = new List<IJob>();
            for (int i = 0; i < types.Length; i++)
            {
                //找到所有加 [Job]特性的类
                JobAttribute jobAttribute = (JobAttribute)types[i].GetCustomAttribute(typeof(JobAttribute));
                if (jobAttribute != null)
                {
                    jobTypeList.Add(types[i]);
                    //创建这些加[Job]特性的类的实例
                    object instance = types[i].Assembly.CreateInstance(types[i].FullName);
                    //实现IJob的类
                    if (instance is IJob job)
                    {
                        iJobList.Add(job);
                    }
                }
            }
            return iJobList;
        }
    }
}
