namespace Auto_send_platform
{
    partial class Form_JZDJ
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgv_fileinfo = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgv_checkin = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgv_opconfirm = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgv_ipconfirm = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fileinfo)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_checkin)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_opconfirm)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ipconfirm)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 337);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv_fileinfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "个人建档";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgv_fileinfo
            // 
            this.dgv_fileinfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_fileinfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_fileinfo.Location = new System.Drawing.Point(6, 6);
            this.dgv_fileinfo.Name = "dgv_fileinfo";
            this.dgv_fileinfo.RowTemplate.Height = 23;
            this.dgv_fileinfo.Size = new System.Drawing.Size(540, 299);
            this.dgv_fileinfo.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgv_checkin);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(552, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "就诊登记信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_checkin
            // 
            this.dgv_checkin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_checkin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_checkin.Location = new System.Drawing.Point(6, 6);
            this.dgv_checkin.Name = "dgv_checkin";
            this.dgv_checkin.RowTemplate.Height = 23;
            this.dgv_checkin.Size = new System.Drawing.Size(540, 299);
            this.dgv_checkin.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgv_opconfirm);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(552, 311);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "门诊就诊确认";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgv_opconfirm
            // 
            this.dgv_opconfirm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_opconfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_opconfirm.Location = new System.Drawing.Point(6, 6);
            this.dgv_opconfirm.Name = "dgv_opconfirm";
            this.dgv_opconfirm.RowTemplate.Height = 23;
            this.dgv_opconfirm.Size = new System.Drawing.Size(540, 299);
            this.dgv_opconfirm.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgv_ipconfirm);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(552, 311);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "住院就诊确认";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgv_ipconfirm
            // 
            this.dgv_ipconfirm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_ipconfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ipconfirm.Location = new System.Drawing.Point(6, 6);
            this.dgv_ipconfirm.Name = "dgv_ipconfirm";
            this.dgv_ipconfirm.RowTemplate.Height = 23;
            this.dgv_ipconfirm.Size = new System.Drawing.Size(540, 299);
            this.dgv_ipconfirm.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form_JZDJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 361);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form_JZDJ";
            this.Text = "Form_JZDJ";
            this.Load += new System.EventHandler(this.Form_JZDJ_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fileinfo)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_checkin)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_opconfirm)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ipconfirm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgv_fileinfo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dgv_checkin;
        private System.Windows.Forms.DataGridView dgv_opconfirm;
        private System.Windows.Forms.DataGridView dgv_ipconfirm;
        private System.Windows.Forms.Timer timer1;
    }
}