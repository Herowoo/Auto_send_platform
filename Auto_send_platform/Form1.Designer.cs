namespace Auto_send_platform
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.tab_mzsf = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.proB_mzsf = new System.Windows.Forms.ProgressBar();
            this.dgv_mzsf = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.proB_zysf = new System.Windows.Forms.ProgressBar();
            this.dgv_zysf = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bgw_zysf = new System.ComponentModel.BackgroundWorker();
            this.bgw_mzsf = new System.ComponentModel.BackgroundWorker();
            this.bgw_load = new System.ComponentModel.BackgroundWorker();
            this.tab_mzsf.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_mzsf)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_zysf)).BeginInit();
            this.SuspendLayout();
            // 
            // tab_mzsf
            // 
            this.tab_mzsf.Controls.Add(this.tabPage1);
            this.tab_mzsf.Controls.Add(this.tabPage2);
            this.tab_mzsf.Location = new System.Drawing.Point(12, 12);
            this.tab_mzsf.Name = "tab_mzsf";
            this.tab_mzsf.SelectedIndex = 0;
            this.tab_mzsf.Size = new System.Drawing.Size(560, 367);
            this.tab_mzsf.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.proB_mzsf);
            this.tabPage1.Controls.Add(this.dgv_mzsf);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "门诊收费确认";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // proB_mzsf
            // 
            this.proB_mzsf.Location = new System.Drawing.Point(6, 311);
            this.proB_mzsf.Name = "proB_mzsf";
            this.proB_mzsf.Size = new System.Drawing.Size(540, 23);
            this.proB_mzsf.TabIndex = 1;
            // 
            // dgv_mzsf
            // 
            this.dgv_mzsf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_mzsf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_mzsf.Location = new System.Drawing.Point(6, 6);
            this.dgv_mzsf.Name = "dgv_mzsf";
            this.dgv_mzsf.RowTemplate.Height = 23;
            this.dgv_mzsf.Size = new System.Drawing.Size(540, 299);
            this.dgv_mzsf.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.proB_zysf);
            this.tabPage2.Controls.Add(this.dgv_zysf);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(552, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "住院收费确认";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // proB_zysf
            // 
            this.proB_zysf.Location = new System.Drawing.Point(6, 311);
            this.proB_zysf.Name = "proB_zysf";
            this.proB_zysf.Size = new System.Drawing.Size(540, 23);
            this.proB_zysf.TabIndex = 3;
            // 
            // dgv_zysf
            // 
            this.dgv_zysf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_zysf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_zysf.Location = new System.Drawing.Point(6, 6);
            this.dgv_zysf.Name = "dgv_zysf";
            this.dgv_zysf.RowTemplate.Height = 23;
            this.dgv_zysf.Size = new System.Drawing.Size(540, 299);
            this.dgv_zysf.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bgw_zysf
            // 
            this.bgw_zysf.WorkerReportsProgress = true;
            this.bgw_zysf.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.bgw_zysf.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.bgw_zysf.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // bgw_mzsf
            // 
            this.bgw_mzsf.WorkerReportsProgress = true;
            this.bgw_mzsf.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.bgw_mzsf.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.bgw_mzsf.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // bgw_load
            // 
            this.bgw_load.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_load_DoWork);
            this.bgw_load.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_load_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 401);
            this.Controls.Add(this.tab_mzsf);
            this.Name = "Form1";
            this.Text = "auto_send_platform";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tab_mzsf.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_mzsf)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_zysf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_mzsf;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgv_mzsf;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgv_zysf;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker bgw_zysf;
        private System.ComponentModel.BackgroundWorker bgw_mzsf;
        private System.Windows.Forms.ProgressBar proB_mzsf;
        private System.Windows.Forms.ProgressBar proB_zysf;
        private System.ComponentModel.BackgroundWorker bgw_load;
    }
}

