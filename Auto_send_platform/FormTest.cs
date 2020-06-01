using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Auto_send_platform
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("Process is running!");
            }
            else
            {
                backgroundWorker1.RunWorkerAsync();
            }    
        }
        private delegate void MyInovkeDelegate(string name);
        public void Test(object o)
        {
            richTextBox1.Text += string.Format("The object is {0} \r\n", o);
        }
        public void ThreadFunc(object b)
        {
            this.Invoke(new MyInovkeDelegate(Test),b);

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker temp_worker = sender as BackgroundWorker;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(200);
                temp_worker.ReportProgress(i);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Process Completed!");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
                MessageBox.Show("Process is not running");
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            
        }
    }
}
