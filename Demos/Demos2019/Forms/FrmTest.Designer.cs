namespace Demos.Demos2019.Forms
{
    partial class FrmTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnBeginInvoke = new System.Windows.Forms.Button();
            this.serviceController1 = new System.ServiceProcess.ServiceController();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // btnBeginInvoke
            // 
            this.btnBeginInvoke.Location = new System.Drawing.Point(485, 36);
            this.btnBeginInvoke.Name = "btnBeginInvoke";
            this.btnBeginInvoke.Size = new System.Drawing.Size(87, 23);
            this.btnBeginInvoke.TabIndex = 0;
            this.btnBeginInvoke.Text = "beginInvoke";
            this.btnBeginInvoke.UseVisualStyleBackColor = true;
            this.btnBeginInvoke.Click += new System.EventHandler(this.btnBeginInvoke_Click);
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 450);
            this.Controls.Add(this.btnBeginInvoke);
            this.Name = "FrmTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBeginInvoke;
        private System.ServiceProcess.ServiceController serviceController1;
        private System.IO.Ports.SerialPort serialPort1;
    }
}