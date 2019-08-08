using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 协变和逆变能够实现数组类型、委托类型和泛型类型参数的隐式引用转换
    /// 
    /// 泛型之间的隐式转换：泛型中类型参数可转换。
    /// 
    /// out（泛型修饰符）:该类型参数是协变
    /// in（泛型修饰符） :该类型参数是逆变
    /// 协变（Covariance）：子类型-->父类型
    /// 逆变（Contravariance）：父类型-->子类型
    /// </summary>
    public class CovarianceContravarianceDemo
    {
        public void Test()
        {
            Fun1();
        }

        private void Fun1()
        {
            List<string> listStr = new List<string>();
            List<object> listObj = new List<object>();
            //listStr = listObj;

            //协变
            //IEnumerable<out T>
            IEnumerable<string> strings = new List<string>();
            IEnumerable<object> objects = new List<object>();
            IEnumerable<string> strings1 = strings;

            ////IComparer<in T>
            //IComparer<string> stringsIComparable = StringComparer.CurrentCulture;


            //协变：子类型 --> 父类型
            // Func <out TResult >
            Func<string> funcStr = () => "fun";
            Func<object> funcObj = funcStr;
            funcObj();



            //Func<in T1, in T2, out TResult>
            Func<Demos.Model.ParentClass, Demos.Model.ParentClass, Demos.Model.ChildClass> funcChildClass = (c1, c2) =>
              {
                  Demos.Model.ChildClass childClass3 = new Model.ChildClass();
                  childClass3.Name = $"{c1.Name},{c2.Name}";
                  childClass3.Age = c1.Age + c2.Age;
                  return childClass3;
              };
            Func<Demos.Model.ChildClass, Demos.Model.ChildClass, Demos.Model.ParentClass> funcParentClass = funcChildClass;

            Demos.Model.ParentClass pc = funcParentClass(new Model.ChildClass { Age = 1, Name = "name1" }, new Model.ChildClass { Age = 2, Name = "name2" });


            //逆变：父类型 --> 子类型
            //Action<in T>
            Action<Demos.Model.ParentClass> actionParentClass = (o) => Console.WriteLine((string)o);
            Action<Demos.Model.ChildClass> actionChildClass = actionParentClass;

            Demos.Model.ChildClass childClass = new Demos.Model.ChildClass();
            childClass.Name = "ChildClass Model";
            actionChildClass(childClass);

            //actionParentClass(childClass);
            //actionChildClass(pc);//逆变针对泛型，不能作用于实参



            //Action<in T1, in T2>
            Action<Demos.Model.ParentClass, Demos.Model.ParentClass> actionParentClass2 = (s1, s2) => Console.WriteLine($"{s1.ToString()},{(string)s2}");
            Action<Demos.Model.ChildClass, Demos.Model.ChildClass> actionChildClass2 = actionParentClass2;


            Demos.Model.ChildClass childClass1 = new Demos.Model.ChildClass();
            childClass1.Name = "li";
            childClass1.Age = 1;

            Demos.Model.ChildClass childClass2 = new Demos.Model.ChildClass();
            childClass2.Name = "rui";
            childClass2.Age = 2;

            actionChildClass2(childClass1, childClass2);


        }

        public string ConvertToTTTimeInForce(string zdTimeInForce) =>
          zdTimeInForce switch
          {
              "1" => "0",

              "2" => "1",

              "4" => "3",

              "5" => "4",
              _ => "default"
          };

        public string ConvertToTTTimeInForce1(string zdTimeInForce)
        {
            string t = zdTimeInForce switch
            {
                "1" => "0",
                "2" => "1",
                "4" => "3",
                "5" => "4",
                "6" => "4",
                _ => "default"
            };
            return t;
            //string ttTimeInForce = "";
            //zdTimeInForce switch
            //{
            //    "1" =>
            //       ttTimeInForce = "0",

            //    "2" =>
            //       ttTimeInForce = "1",

            //    "4" =>
            //       ttTimeInForce = "3",

            //    "5" =>
            //       ttTimeInForce = "4",
            //    _ => ttTimeInForce = "default"
            //};
            //return ttTimeInForce;
        }


        private void Test(string str) => Console.WriteLine(str);

    }


}
