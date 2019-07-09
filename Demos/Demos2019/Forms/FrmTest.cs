using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demos.Demos2019.Forms
{
    public partial class FrmTest :Form
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private void btnBeginInvoke_Click(object sender, EventArgs e)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            this.BeginInvoke((MethodInvoker)(() =>
            {
                //id 和 threadId的值一样，即：线程ID一样
                var threadId = Thread.CurrentThread.ManagedThreadId;
            }));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string updatePath = @"D:\fancky\C#\Demos\WinformFormUpdate\bin\Debug\WinformFormUpdate.exe";
            Process.Start(updatePath);
            //Application.Exit();
            Environment.Exit(0);
        }

        private async void BtnAsync_Click(object sender, EventArgs e)
        {
            // await  虽然在等待，但是不会阻碍UI线程。
            //string x = await DoWork();
            //this.btnAsync.Text = x;

            int contentLength = await AccessTheWebAsync();


        }

        private async Task<string> DoWork()
        {
            //await Task.Delay(5000);
            //return "str";


            return await Task.Run(() =>
            {
                Thread.Sleep(5000);
                return "str";
            });
        }

        // Three things to note in the signature:
        //  - The method has an async modifier. 
        //  - The return type is Task or Task<T>. (See "Return Types" section.)
        //    Here, it is Task<int> because the return statement returns an integer.
        //  - The method name ends in "Async."
        async Task<int> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.
            //  - AccessTheWebAsync can't continue until getStringTask is complete.
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.
            //  - Control resumes here when getStringTask is complete. 
            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }
        void DoIndependentWork()
        {
            this.btnAsync.Text = "Working . . . . . . .\r\n";
        }

    }
}
