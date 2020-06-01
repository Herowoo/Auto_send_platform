using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Threading;

namespace Auto_send_platform
{
    class Func
    {
        WebReference1.Service service = new WebReference1.Service();
        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        private int HisExePro(string proname,string instr)
        {
            service.Timeout = 600000;
            string result = string.Empty;
            try
            {
                result = service.HisExePro(proname, instr);

            }
            catch (Exception)
            {

            }
            string msg = Split_xml(result, "code");
            if (msg == "1")
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }
        public int DC_Payment(string as_type, string as_rcpt_no, string as_health_evn_id, StringBuilder message)
        {
            //WebReference1.Service service = new WebReference1.Service();
            string proname = "dc_payment";
            string instr = "<interface><health_evn_id>" + as_health_evn_id + "</health_evn_id><in_type>" + as_type + "</in_type><in_rcpt_no>" + as_rcpt_no + "</in_rcpt_no></interface>";
            string result = service.HisExePro(proname, instr);
            string msg = Split_xml(result, "message");
            if (msg == "操作成功")
            {
                return 1;
            }
            else
                return -1;
        }
        public int DC_File_Query(string as_id_no,StringBuilder message)
        {
            string proname = "dc_file_query";
            string instr = "<interface><IdNo>" + as_id_no + "</IdNo></interface>";
            return HisExePro(proname, instr);
        }
        public int DC_File_Add(string patient_id, string username, StringBuilder message)
        {
            string proname = "dc_file_add";
            string instr = "<interface><PatientId>" + patient_id + "</PatientId><OperatorName>" + username + "</OperatorName></interface>";
            return HisExePro(proname, instr);

        }
        public int DC_CheckIn(string as_type,string as_patientstrno,StringBuilder message)
        {
            string proname = "dc_checkin";
            string instr = "<interface><in_type>" + as_type + "</in_type><in_patientstrno>" + as_patientstrno + "</in_patientstrno></interface>";

            return HisExePro(proname, instr);
        }
        public int DC_ipconfirm(string patientstrno,StringBuilder message)
        {
            WebReference1.Service service = new WebReference1.Service();
            string proname = "dc_opconfirm";
            string instr = "<interface><in_patientstrno>"+patientstrno+"</in_patientstrno></interface>";
            string result = service.HisExePro(proname, instr);
            string msg = Split_xml(result, "code");
            if (msg == "1")
            {
                return 1;
            }
            else
                return -1;
        }
        public int DC_opconfirm(string as_type,string as_patientstrno,StringBuilder message)
        {
            WebReference1.Service service = new WebReference1.Service();
            string proname = "dc_opconfirm";
            string instr = "<interface><in_type>"+as_type+"</in_type><in_patientstrno>"+as_patientstrno+"</in_patientstrno></interface>";
            string result = service.HisExePro(proname, instr);
            string msg = Split_xml(result, "code");
            if (msg == "1")
            {
                return 1;
            }
            else
                return -1;
        }
        public string Split_xml(string source,string keywords)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(source);
            XmlNodeList top_interface = xmldoc.SelectNodes("//interface");
            string msg = string.Empty;
            foreach (XmlElement elem in top_interface)
            {
                msg = elem.GetElementsByTagName(keywords)[0].InnerText;
            }
            return msg;
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="str">写入内容</param>
        /// <param name="rpath">相对路径</param>
        public static void WriteLog(string str,string rpath)
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                DateTime d = DateTime.Now;
                string path = AppDomain.CurrentDomain.BaseDirectory + rpath;
                string fileName = d.ToString("yyyy-MM-dd-HH") + "platformlog.txt";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (StreamWriter file = new StreamWriter(path + fileName, true))
                {
                    if (string.IsNullOrEmpty(str))
                        file.WriteLine("");
                    else
                    {
                        file.WriteLine(d.ToString("yyyy-MM-dd HH:mm:ss"));
                        file.WriteLine(str);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }
        }
    }
}
