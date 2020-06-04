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

namespace Auto_send_platform
{
    public partial class Form1 : Form
    {
        IniFile inifile = new IniFile(@".\auto_send_platform.ini");
        DataTable dt_zysf = new DataTable();
        DataTable dt_mzsf = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (bgw_load.IsBusy)
            {

            }
            else
            {
                bgw_load.RunWorkerAsync();
            }
            //int tick = 0;
            //string timer = inifile.IniReadValue("setting", "timer","60");
            //tick = int.Parse(timer) * 1000;
            //timer1.Interval = tick;
            //timer1.Start();
        }
        private void LoadDt_mzsf()
        {
            string dt_datetime = inifile.IniReadValue("setting", "start_date", System.DateTime.Now.ToString("yyyy-MM-dd"));

            // 门诊收费确认
            string sql_mzsf = string.Format("select distinct a.rcpt_no, c.health_evn_id from outp_rcpt_master a, outp_order_desc b, clinic_master c where a.rcpt_no = b.rcpt_no and b.clinic_no = c.clinic_no and c.health_evn_id is not null and a.sxjy_flag is null and a.visit_date >= date'{0}' and rownum<100", dt_datetime);
            dt_mzsf = BaseDB.textExecuteDataset(sql_mzsf);
        }
        private void LoadDt_zysf()
        {
            // 住院收费确认
            string dt_datetime = inifile.IniReadValue("setting", "start_date", System.DateTime.Now.ToString("yyyy-MM-dd"));

            string sql_zysf = string.Format("select b.rcpt_no,a.health_evn_id from pat_visit a, inp_settle_master b where a.patient_id = b.patient_id and a.visit_id = b.visit_id and b.settling_date >= date'{0}' and a.health_evn_id is not null and a.flag is null and rownum<100", dt_datetime);
            dt_zysf = BaseDB.textExecuteDataset(sql_zysf);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bgw_zysf.IsBusy)
            {
            }
            else
            {
                bgw_zysf.RunWorkerAsync();
            }

            if (bgw_mzsf.IsBusy)
            {

            }
            else
                bgw_mzsf.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //string dt_datetime = inifile.IniReadValue("setting", "start_date", System.DateTime.Now.ToString("yyyy-MM-dd"));
            Func.WriteLog("backgroundWorker1_DoWork", "\\interface_his_log\\");
            Func func1 = new Func();
            StringBuilder msg = new StringBuilder();

            if (dt_zysf.Rows.Count > 0)
            {
                for(int i=0; i<dt_zysf.Rows.Count;i++)
                {
                    string rcpt_no = dt_zysf.Rows[i][0].ToString();
                    string health_evn_id = dt_zysf.Rows[i][1].ToString();

                    int ret = func1.DC_Payment("4", rcpt_no, health_evn_id, msg);
                    if (1 == ret)
                    {
                        string sql_update5 = string.Format("update pat_visit set flag = '5' where health_evn_id = '{0}'", health_evn_id);
                        BaseDB.spExecuteNonQuery(sql_update5);
                        Func.WriteLog("住院收费确认信息：" + health_evn_id, "\\interface_his_log\\");
                    }
                    else
                    {
                        string sql_update1 = string.Format("update pat_visit set flag = '1' where health_evn_id = '{0}'", health_evn_id);

                        BaseDB.spExecuteNonQuery(sql_update1);
                        Func.WriteLog("住院收费确认发送给舒心就医平台时出错！ " + health_evn_id, "\\interface_his_log\\");
                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Func.WriteLog("backgroundWorker1_RunWorkerCompleted", "\\interface_his_log\\");

            LoadDt_zysf();
            dgv_zysf.DataSource = dt_zysf;
            bgw_zysf.RunWorkerAsync();
        }

       

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Func.WriteLog("backgroundWorker2_DoWork", "\\interface_his_log\\");

            #region 门诊收费确认
            // 门诊收费确认
            Func func1 = new Func();
            StringBuilder msg = new StringBuilder();

            if (dt_mzsf.Rows.Count > 0)
            {
                //
                #region 循环取值
                for (int i = 0; i < dt_mzsf.Rows.Count; i++)
                {
                    string rcpt_no = dt_mzsf.Rows[i][0].ToString();
                    string health_evn_id = dt_mzsf.Rows[i][1].ToString();

                    int ret = func1.DC_Payment("1", rcpt_no, health_evn_id, msg);
                    if (1 == ret)
                    {
                        string sql_update5 = string.Format("update outp_rcpt_master  a  set a.sxjy_flag = '5' where a.rcpt_no  = '{0}'", rcpt_no);
                        BaseDB.spExecuteNonQuery(sql_update5);
                        Func.WriteLog("门诊收费确认信息：" + health_evn_id, "\\interface_his_log\\");
                    }
                    else
                    {
                        string sql_update1 = string.Format("update outp_rcpt_master  a  set a.sxjy_flag = '1' where a.rcpt_no  = '{0}'", rcpt_no);
                        BaseDB.spExecuteNonQuery(sql_update1);
                        Func.WriteLog("门诊收费确认发送给舒心就医平台时出错！ " + health_evn_id, "\\interface_his_log\\");
                    }
                }
                #endregion
            }
            else
            {
                Thread.Sleep(3000);
            }
            #endregion
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Func.WriteLog("backgroundWorker2_RunWorkerCompleted", "\\interface_his_log\\");

            LoadDt_mzsf();
            dgv_mzsf.DataSource = dt_mzsf;
            bgw_mzsf.RunWorkerAsync();
        }

        private void bgw_load_DoWork(object sender, DoWorkEventArgs e)
        {
            Func.WriteLog("bgw_load_DoWork", "\\interface_his_log\\");

            LoadDt_mzsf();
            LoadDt_zysf();
        }

        private void bgw_load_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Func.WriteLog("bgw_load_RunWorkerCompleted", "\\interface_his_log\\");

            dgv_mzsf.DataSource = dt_mzsf;
            dgv_zysf.DataSource = dt_zysf;

            if (bgw_mzsf.IsBusy)
            {

            }
            else
            {
                bgw_mzsf.RunWorkerAsync();
            }

            if (bgw_zysf.IsBusy)
            {

            }
            else
            {
                bgw_zysf.RunWorkerAsync();
            }
        }
    }
}