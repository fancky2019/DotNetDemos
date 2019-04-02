using CSCreateProcessAsUserFromService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public partial class StartProgramService : ServiceBase
    {
        public StartProgramService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            //启动TeamViewer
            //exe
            //if (File.Exists(@"C:\Program Files (x86)\TeamViewer\TeamViewer.exe"))
            //{
            //    System.Diagnostics.Process.Start(@"C:\Program Files (x86)\TeamViewer\TeamViewer.exe");
            //}
            //快捷方式
            //if (File.Exists("C:\\Users\\Administrator\\Desktop\\工具\\TeamViewer 13.lnk"))
            //{
            //    System.Diagnostics.Process.Start("C:\\Users\\Administrator\\Desktop\\工具\\TeamViewer 13.lnk");
            //}
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                var path = ConfigurationManager.AppSettings[key];
                if (path.EndsWith(".lnk") || path.EndsWith(".exe"))
                {
                    if (File.Exists(path))
                    {

                        // System.Diagnostics.Process.Start(path);
                        Task.Run(() =>
                        {
                            //CreateProcessAsUserWrapper:链接
                            //https://code.msdn.microsoft.com/CSCreateProcessAsUserFromSe-b682134e
                            //TeamViewer 启动不了，QQ都能启动，.Net 程序也能启动
                            CreateProcessAsUserWrapper.LaunchChildProcess(path);
 
                        });
                    }
                }
            }
          

        }

        protected override void OnStop()
        {
        }
    }
}
