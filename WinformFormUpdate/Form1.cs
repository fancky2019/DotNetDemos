using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformFormUpdate
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {

            #region 进度条
            progressBarControl1.Visible = true;
            //设置一个最小值
            progressBarControl1.Properties.Minimum = 0;
            //设置一个最大值
            progressBarControl1.Properties.Maximum = 100;
            //设置步长，即每次增加的数
            progressBarControl1.Properties.Step = 1;
            //设置进度条的样式
            progressBarControl1.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            //当前值
            progressBarControl1.Position = 0;
            //是否显示进度数据
            progressBarControl1.Properties.ShowTitle = true;
            //是否显示百分比
            progressBarControl1.Properties.PercentView = true;

            #endregion
        }


        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                FtpHelper.UploadFtp("D:\\Test\\file", "计算机网络（第7版）-谢希仁.pdf", @"192.168.1.105:21", "administrator", "li5rui3", (percent) =>
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        this.pbUpLoad.Value = percent;
                    }));

                });
            });

        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                FtpHelper.DownloadFtp("D:\\Test\\file", "计算机网络（第7版）-谢希仁.pdf", "计算机网络（第7版）-谢希仁.pdf", @"192.168.1.105:21", "administrator", "li5rui3", (percent) =>
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        this.pbDownLoad.Value = percent;

                        progressBarControl1.Position = percent;
                    }));

                });
            });
        }

        private void btnWCDownLoad_Click(object sender, EventArgs e)
        {
            WebClientDownLoad();
        }
        private void WebClientDownLoad()
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += (s, e) =>
            {

                this.BeginInvoke((MethodInvoker)(() =>
                {
                    this.pbDownLoad.Value = e.ProgressPercentage;
                    progressBarControl1.Position = e.ProgressPercentage;
                    this.label2.Text = e.ProgressPercentage + "%";
                }));

                //if (e.ProgressPercentage == 100)
                //{
                //    //下载完成之后开始覆盖
                // using ICSharpCode.SharpZipLib.Zip;
                //    ZipHelper.Unzip();//调用解压的类

                //}
            };

            //URI  是URL的一种
            Uri uri = new Uri("http://localhost:8002/a.pdf");

            //在debug目录下
            //wc.DownloadFileAsync(uri, "计算机网络Test1.pdf");
            //要下载文件的路径,保存路径(注意：如果路径错误无法下载)
            wc.DownloadFileAsync(uri, "D:\\Test\\file\\计算机网络Test1.pdf");

        }

    
    }
}
