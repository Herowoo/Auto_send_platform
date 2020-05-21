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
    public partial class Form_JZDJ : Form
    {
        IniFile inifile = new IniFile(@".\auto_send_platform.ini");

        public Form_JZDJ()
        {
            InitializeComponent();
        }

        private void Form_JZDJ_Load(object sender, EventArgs e)
        {
            int tick = 0;
            string timer = inifile.IniReadValue("setting", "timer","60");
            tick = int.Parse(timer) * 1000;

            timer1.Interval = tick;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string dt_datetime = inifile.IniReadValue("setting", "start_date",System.DateTime.Now.ToString("yyyy-MM-dd"));

            #region 个人档案
            string sql_fileinfo = string.Format("select t.patient_id,t.operator operator_name t.id_no  from pat_master_index t where t.create_date >= date'{0}' and t.charge_type <> '新生儿'  and  t.flag is null and t.id_no is not null", dt_datetime);
            DataTable dt_fileinfo = BaseDB.textExecuteDataset(sql_fileinfo);
            dgv_fileinfo.DataSource = dt_fileinfo;

            Func func1 = new Func();
            StringBuilder msg = new StringBuilder();
            if (dt_fileinfo.Rows.Count>0)
            {
                foreach (DataRow dr in dt_fileinfo.Rows)
                {
                    string pid = dr[0].ToString();
                    string op_name = dr[1].ToString();
                    string id_no = dr[2].ToString();

                    // 查询建档信息
                    int is_addfile = func1.DC_File_Query(id_no,msg);
                    if (1 == is_addfile)
                    {
                        string sql = string.Format("update pat_master_index a set a.flag = '1' where a.patient_id = '{0}'", pid);
                        BaseDB.spExecuteNonQuery(sql);
                        Func.WriteLog("已建档：" + pid, "\\interface_his_log\\");
                    }
                    else
                    {
                        int ret = func1.DC_File_Add(pid, op_name, msg);
                        if (1 == ret)
                        {
                            string sql = string.Format("update pat_master_index a set a.flag = '1' where a.patient_id = '{0}'", pid);
                            BaseDB.spExecuteNonQuery(sql);
                            Func.WriteLog("个人档案信息：" + pid, "\\interface_his_log\\");

                        }
                        else
                        {
                            string sql = string.Format("update pat_master_index a set a.flag = '2' where a.patient_id = '{0}'", pid);
                            BaseDB.spExecuteNonQuery(sql);
                            Func.WriteLog("个人档案信息发送给舒心就医平台时出错！" + pid, "\\interface_his_log\\");
                        }
                    }
                }
            }
            #endregion

            #region 就诊登记
            string sql_jzdj = string.Format("select '0' in_type,t.clinic_no in_patientstrno from clinic_master t where t.visit_date >= date'{0}'  and t.registration_status = 2 and t.health_evn_id is null and t.flag is null union select '1', 'P' || t.patient_id || 'V' || t.visit_id from pat_visit t where t.admission_date_time >= date'{0}' and t.health_evn_id is null and t.flag is null", dt_datetime);
            DataTable dt_checkin = BaseDB.textExecuteDataset(sql_jzdj);
            dgv_checkin.DataSource = dt_checkin;

            if (dt_checkin.Rows.Count>0)
            {
                foreach (DataRow dr in dt_checkin.Rows)
                {
                    string in_type = dr[0].ToString();
                    string in_patientstrno = dr[1].ToString();

                    int ret = func1.DC_CheckIn(in_type, in_patientstrno, msg);
                    if (1 == ret)
                    {
                        string sql = string.Empty;
                        if ("0" == in_type)
                        {
                             sql = string.Format("update clinic_master a  set a.flag = '1' where to_char(a.visit_date,'yyyymmdd')||a.visit_no = '{0}'", in_patientstrno);

                        }
                        else
                        {
                             sql = string.Format("update pat_visit a  set a.flag = '1' where 'P'||a.patient_id||'V'||a.visit_id = '{0}'", in_patientstrno);

                        }
                        BaseDB.spExecuteNonQuery(sql);
                        Func.WriteLog("就诊登记信息：" + in_patientstrno, "\\interface_his_log\\");
                    }
                    else
                    {
                        string sql = string.Empty;
                        if ("0" == in_type)
                        {
                            sql = string.Format("update clinic_master a  set a.flag = '2' where to_char(a.visit_date,'yyyymmdd')||a.visit_no = '{0}'", in_patientstrno);

                        }
                        else
                        {
                            sql = string.Format("update pat_visit a  set a.flag = '2' where 'P'||a.patient_id||'V'||a.visit_id = '{0}'", in_patientstrno);

                        }
                        BaseDB.spExecuteNonQuery(sql);
                        Func.WriteLog("就诊登记信息发送给舒心就医平台时出错！" + in_patientstrno, "\\interface_his_log\\");
                    }
                }

            }
            #endregion

            #region 门诊就诊确认
            string sql_opconfirm = string.Format("select '0' in_type,to_char(t.visit_date,'yyyymmdd')||t.visit_no in_patientstrno from clinic_master t where t.visit_date >= date'{0}'  and  t.flag = '1'   and (t.visit_date,t.visit_no) in (select  a.visit_date,a.visit_no from outp_wait_queue a where a.worked_indicator = 3)", dt_datetime);
            DataTable dt_opconfirm = BaseDB.textExecuteDataset(sql_opconfirm);
            dgv_opconfirm.DataSource = dt_opconfirm;

            if (dt_opconfirm.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_opconfirm.Rows)
                {
                    string in_type = dr[0].ToString();
                    string in_patientstrno = dr[1].ToString();

                    int ret = func1.DC_opconfirm(in_type, in_patientstrno, msg);
                    if (1 == ret)
                    {
                        string sql = string.Format("update clinic_master a  set a.flag = '5' where to_char(a.visit_date,'yyyymmdd')||a.visit_no = '{0}'", in_patientstrno);
                        BaseDB.spExecuteNonQuery(sql);
                        Func.WriteLog("门诊就诊确认信息：" + in_patientstrno, "\\interface_his_log\\");

                    }
                    else
                    {
                        Func.WriteLog("门诊就诊确认发送给舒心就医平台时出错！" + in_patientstrno, "\\interface_his_log\\");
                    }
                }
            }
            #endregion

            #region 住院就诊确认
            string sql_ipconfirm = string.Format("select 'P'||t.patient_id||'V'||t.visit_id  in_patientstrno from pat_visit t where t.admission_date_time >= date'{0}' and t.discharge_date_time is not null and t.health_evn_id is null and t.flag = '1'", dt_datetime);
            DataTable dt_ipconfirm = BaseDB.textExecuteDataset(sql_ipconfirm);
            dgv_ipconfirm.DataSource = dt_ipconfirm;

            if (dt_ipconfirm.Rows.Count>0)
            {
                foreach (DataRow dr in dt_ipconfirm.Rows)
                {
                    string in_patientstrno = dr[0].ToString();
                    int ret = func1.DC_ipconfirm(in_patientstrno, msg);

                    if (1 == ret)
                    {
                        string sql = string.Format("update pat_visit a  set a.flag = '5' where 'P'||a.patient_id||'V'||a.visit_id = '{0}'", in_patientstrno);
                        BaseDB.spExecuteNonQuery(sql);
                        Func.WriteLog("住院就诊确认信息：" + in_patientstrno, "\\interface_his_log\\");

                    }
                    else
                    {
                        Func.WriteLog("住院就诊确认发送给舒心就医平台时出错！" + in_patientstrno, "\\interface_his_log\\");
                    }
                }
            }
            #endregion
        }

    }
}
