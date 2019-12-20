using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.DataAccess;
using TableTennis.DataModels;
using MySql.Data.MySqlClient;
using SPUtility;

namespace TableTennis.BusinessLogic
{
    public class ManagerLogic: SPConnectionBase
    {       
        public void Add(ref MANAGER dataObj)
        {
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.Add(ref dataObj);
                string Mail = TextRegister(dataObj);
                EmailSender.Sub = " : ขอรหัสเพื่อสมัครการแข่งขัน";
                EmailSender.Send(dataObj.EMAIL, Mail);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        private string TextRegister(MANAGER dataObj)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(@"<h3>เรียนคุณ "+ dataObj.FIRST_NAME+" "+dataObj.LAST_NAME+"</h3>");
            text.AppendLine("<br/>");
            text.AppendLine("<br/>");
            text.AppendLine(@"<h3>คุณลงชื่อเพื่อของรหัสในการสมัครการแข่งขันเรียบร้อย</h2>");
            text.AppendLine(@"<h3><small>รหัสที่ใช้สมัครการแข่งขัน คือ </small>"+dataObj.ID.ToString()+"</h3>");
            text.AppendLine(@"<h3>คุณสามารถเขาระบบผ่านลิ้ง : "+ EmailSender.url + dataObj.ID + "</h3>");
            text.AppendLine("<br/>");
            text.AppendLine("<a target=\"_blank\" href=\"" + EmailSender.url + dataObj.ID + "\" style=\" background-color:#5cb85c;padding:10px;font-size:18px;color:white;border-radius:5px;\"> เข้าสู่ระบบ</a>");
            text.AppendLine("<br/>");
            text.AppendLine("<br/>");
            text.AppendLine(@"<h3>ขอบคุณ</h3>");
            text.AppendLine(@"<h3>สมาคมกีฬาเทเบิลเทนนิสแห่งประเทศไทย</h3>");
            text.AppendLine(@"<h3>Tel (66)02 170 9474</h3>");
            text.AppendLine(@"<h3>Fax (66)02 170 9475</h3>");
            return text.ToString();
        }

        public MANAGER Get(long? id)
        {
            if (id == null)
                return null;
            MANAGER dataObj = new MANAGER();
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public MANAGER SignIn(string email, int? id)
        {
            if (id == null)
            {
                return null;
            }
            MANAGER dataObj = new MANAGER();
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.SignIn(email, id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public MANAGER SendRecover(string email)
        {
            if (email == null)
            {
                return null;
            }
            MANAGER dataObj = new MANAGER();
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            try
            { 
                dataObj = access.SendRecover(email);
                if (dataObj == null)
                    throw new Exception("ไม่พบข้อมูล กรุณาตรวจอีเมลอีกครั้ง");

                string Mail = TextRecover(dataObj);
                EmailSender.Sub = " : ขอรหัสเพื่อสมัครการแข่งขัน";
                EmailSender.Send(dataObj.EMAIL, Mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        private string TextRecover(MANAGER dataObj)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(@"<h3>เรียนคุณ " + dataObj.FIRST_NAME + " " + dataObj.LAST_NAME + "</h3>");
            text.AppendLine("<br/>");
            text.AppendLine("<br/>");
            text.AppendLine(@"<h3><small>รหัสที่ใช้สมัครการแข่งขัน คือ </small>" + dataObj.ID.ToString() + "</h3>");
            text.AppendLine(@"<h3>คุณสามารถเขาระบบผ่านลิ้ง : " + EmailSender.url + dataObj.ID + "</h3>");
            text.AppendLine("<br/>");
            text.AppendLine("<a target=\"_blank\" href=\"" + EmailSender.url + dataObj.ID + "\" style=\"background-color:#5cb85c;padding:10px;font-size:18px;color:white;border-radius:5px;\"> เข้าสู่ระบบ</a>");
            text.AppendLine("<br/>");
            text.AppendLine("<br/>");
            text.AppendLine(@"<h3>ขอบคุณ</h3>");
            text.AppendLine(@"<h3>สมาคมกีฬาเทเบิลเทนนิสแห่งประเทศไทย</h3>");
            text.AppendLine(@"<h3>Tel (66)02 170 9474</h3>");
            text.AppendLine(@"<h3>Fax (66)02 170 9475</h3>");
            return text.ToString();
        }

        public List<MANAGER> GridManager(ParamManager param)
        {
            if (param == null)
            {
                return null;
            }
            List<MANAGER> dataObj = new List<MANAGER>();
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GridManager(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public void Update(ref MANAGER dataObj)
        {
            ManagerAccess access = new ManagerAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.Update(ref dataObj); 
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
    }
}
