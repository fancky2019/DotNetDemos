using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class StackTraceDemo
    {
        public void Test()
        {
            StackTraceInfo();
        }

        public void StackTraceInfo()
        {
            
            StackTrace stackTrace = new StackTrace(true);

            //StackTrace 声明 FrameCount =1
            //方法每调用一层  FrameCount +=1；
            StackFrame stackFrame =   stackTrace.GetFrame(1);//调用该方法
            int lineNumber=  stackFrame.GetFileLineNumber();//行
            MethodBase methodBase = stackFrame.GetMethod();
            string methodName= methodBase.Name;//方法
            string className = methodBase.DeclaringType.Name;//类
            string stackTraceInfo = $"Class:{className} Method:{methodName} Line:{lineNumber}";
           // string stackTraceInfo = $"Class:{className} Method:{methodName} Line:{lineNumber}\r\n"+"msg";
        }
    }
}
