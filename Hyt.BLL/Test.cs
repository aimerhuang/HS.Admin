using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.BLL.Log;
using Hyt.DataAccess.Log;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL
{
    public class Test : BOBase<Test>
    {
        public Test()
        {

        }
        //public string GetName(string s)
        //{
        //    return Hyt.IDataAccess.ITest.Instance.GetName(s);
        //}      

        public int Count()
        {
            return Hyt.DataAccess.ITest.Instance.Count();
        }

        public void InsSysLog(string lopIp)
        {
            SySystemLog systemLog = new SySystemLog();
            systemLog.Exception = "";
            systemLog.LogLevel = 10;
            systemLog.Source = 20;
            systemLog.Operator = 1;
            systemLog.LogIp = lopIp;
            systemLog.LogDate = DateTime.Now;
            systemLog.Message = "事物方法测试1！";

            ISySystemLogDao.Instance.Create(systemLog);
        }
 

    }
}
