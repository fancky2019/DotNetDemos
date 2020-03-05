using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers
    /// 命名空间内的访问修饰符:public、internal
    /// 
    /// 
    /// 成员访问修饰符：
    /// public: 访问不受限制
    /// private:包含类的类型。
    /// protected:访问限于包含类或派生自包含类的类型。
    /// internal:当前程序集。
    /// protected internal：当前程序集或派生自包含类的类型(另一个程序集的派生类内，其他类内访问不了)。
    /// private protected：包含类或当前程序集内派生自包含类的类型。
    /// </summary>
    public class AccessDecorateDemo
    {
        protected internal int var1;
        public void Test()
        {
            //c# 接口成员的访问修饰符只能是公有的。

            //public：访问不受限制。
            //protected：访问限于包含类或派生自包含类的类型。
            //internal：访问限于当前程序集。
            //protected internal：访问限于当前程序集或派生自包含类的类型(另一个程序集的派生类内，其他类内访问不了)。
            //private：访问限于包含类。
            //private protected：访问限于包含类或当前程序集中派生自包含类的类型。


            // https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/accessibility-levels
            //   成员        默认成员可访问性    允许的成员的声明的可访问性       继承
            //   enum        public              无                                无
            //   class       private             public                            单
            //                                   protected
            //                                   internal
            //                                   private
            //                                   protected internal
            //                                   private protected
            //  interface    public              只能是public                      多
            //  struct       private             public                            无
            //                                   internal
            //                                   private
        }

        public class Te
        {

        }
        protected class Te1
        {

        }
        internal class Te2
        {

        }
        internal class Te3
        {

        }

        protected internal class Te4
        {

        }

        private protected class Te5
        {

        }

        protected interface IeTest
        {

        }

        private protected struct Stru
        {

        }

    }
}
