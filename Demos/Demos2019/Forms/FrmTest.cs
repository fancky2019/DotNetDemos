using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}
