using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_send_platform
{
    public partial class Form1 : Form
    {
        IniFile inifile = new IniFile(@".\auto_send_platform.ini");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int tick = 0;
            string timer = inifile.IniReadValue("setting", "timer","60");
            tick = int.Parse(timer) * 1000;

            timer1.Interval = tick;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 住院收费确认
            string dt_datetime = inifile.IniReadValue("setting", "start_date",System.DateTime.Now.ToString("yyyy-MM-dd"));
            
            string sql_zysf = string.Format("select b.rcpt_no,a.health_evn_id from pat_visit a, inp_settle_master b where a.patient_id = b.patient_id and a.visit_id = b.visit_id and b.settling_date >= date'{0}' and a.health_evn_id is not null and a.flag is null",dt_datetime);
            DataTable dt_1 = BaseDB.textExecuteDataset(sql_zysf);
            dataGridView1.DataSource = dt_1;

            Func func1 = new Func();
            StringBuilder msg = new StringBuilder();

            if (dt_1.Rows.Count > 0)
            {
                
                foreach (DataRow dr in dt_1.Rows)
                {
                    string rcpt_no = dr[0].ToString();
                    string health_evn_id = dr[1].ToString();

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
            // 门诊收费确认
            string sql_mzsf = string.Format("select distinct a.rcpt_no, c.health_evn_id from outp_rcpt_master a, outp_order_desc b, clinic_master c where a.rcpt_no = b.rcpt_no and b.clinic_no = c.clinic_no and c.health_evn_id is not null and a.sxjy_flag is null and a.visit_date >= date'{0}'",dt_datetime);
            DataTable dt_2 = BaseDB.textExecuteDataset(sql_mzsf);
            dataGridView2.DataSource = dt_2;

            foreach (DataRow dr in dt_2.Rows)
            {
                string rcpt_no = dr[0].ToString();
                string health_evn_id = dr[1].ToString();

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
        }
    }
}
