namespace WinformFormUpdate
{
    partial class frmUpdate
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpLoad = new System.Windows.Forms.Button();
            this.pbUpLoad = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDownLoad = new System.Windows.Forms.Button();
            this.pbDownLoad = new System.Windows.Forms.ProgressBar();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.btnWCDownLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpLoad
            // 
            this.btnUpLoad.Location = new System.Drawing.Point(421, 19);
            this.btnUpLoad.Name = "btnUpLoad";
            this.btnUpLoad.Size = new System.Drawing.Size(75, 23);
            this.btnUpLoad.TabIndex = 0;
            this.btnUpLoad.Text = "UpLoad";
            this.btnUpLoad.UseVisualStyleBackColor = true;
            this.btnUpLoad.Click += new System.EventHandler(this.btnUpLoad_Click);
            // 
            // pbUpLoad
            // 
            this.pbUpLoad.Location = new System.Drawing.Point(60, 19);
            this.pbUpLoad.Name = "pbUpLoad";
            this.pbUpLoad.Size = new System.Drawing.Size(316, 23);
            this.pbUpLoad.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // btnDownLoad
            // 
            this.btnDownLoad.Location = new System.Drawing.Point(421, 71);
            this.btnDownLoad.Name = "btnDownLoad";
            this.btnDownLoad.Size = new System.Drawing.Size(75, 23);
            this.btnDownLoad.TabIndex = 4;
            this.btnDownLoad.Text = "DownLoad";
            this.btnDownLoad.UseVisualStyleBackColor = true;
            this.btnDownLoad.Click += new System.EventHandler(this.btnDownLoad_Click);
            // 
            // pbDownLoad
            // 
            this.pbDownLoad.Location = new System.Drawing.Point(60, 71);
            this.pbDownLoad.Name = "pbDownLoad";
            this.pbDownLoad.Size = new System.Drawing.Size(316, 22);
            this.pbDownLoad.TabIndex = 5;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(60, 99);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(316, 18);
            this.progressBarControl1.TabIndex = 6;
            // 
            // btnWCDownLoad
            // 
            this.btnWCDownLoad.Location = new System.Drawing.Point(421, 148);
            this.btnWCDownLoad.Name = "btnWCDownLoad";
            this.btnWCDownLoad.Size = new System.Drawing.Size(102, 23);
            this.btnWCDownLoad.TabIndex = 7;
            this.btnWCDownLoad.Text = "WCDownLoad";
            this.btnWCDownLoad.UseVisualStyleBackColor = true;
            this.btnWCDownLoad.Click += new System.EventHandler(this.btnWCDownLoad_Click);
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 288);
            this.Controls.Add(this.btnWCDownLoad);
            this.Controls.Add(this.progressBarControl1);
            this.Controls.Add(this.pbDownLoad);
            this.Controls.Add(this.btnDownLoad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbUpLoad);
            this.Controls.Add(this.btnUpLoad);
            this.Name = "frmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpLoad;
        private System.Windows.Forms.ProgressBar pbUpLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDownLoad;
        private System.Windows.Forms.ProgressBar pbDownLoad;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private System.Windows.Forms.Button btnWCDownLoad;
    }
}

