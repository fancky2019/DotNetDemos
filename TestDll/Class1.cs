using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDll
{
    //  //https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers
    //在命名空间内直接声明（换句话说，不嵌套在其他类或结构中）的类和结构可以为公共或内部。 如果未指定任何访问修饰符，则默认设置为内部。
    public class Class1
    {
        //可以使用六种访问类型中的任意一种声明类成员（包括嵌套的类和结构）。 结构成员无法声明为受保护，因为结构不支持继承。
      
    }

    class Class2
    {

    }
    //接口可以声明为公共或内部，就像类和结构一样，接口默认设置为内部访问。 接口成员始终为公共的
    public interface Interface1
    {
       // private protected
    }

    interface Interface2
    {

    }
}
