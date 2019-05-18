using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.DataImporter.TaskThread;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter.TaskThread
{
    public class SyUserTaskThread :BaseTaskThread
    {
        public SyUserTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
            : base(OnTaskBegin, OnTaskGoing)
        {
            Read();
        }

        public override int order
        {
            get { return 1; }
        }
        
        public override string name
        {
            get { return "系统用户"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyUser> list;

        protected override void Read()
        {

            string sSql = "SELECT distinct " +
                            "sysno,"+
                            "userid AS account," +
                            "Pwd AS password," +
                            "UserName AS username," +
                            "MobilePhone AS mobilephonenumber," +
                            "Email AS emailaddress, " +
                            "(CASE WHEN status=0 THEN 1 " +
		                            "WHEN status=-1 THEN  0 " +
	                        "END) AS STATUS," +
                            "GETDATE() AS createddate," +
                            "NULL AS createdby," +
                            "NULL AS lastupdatedate," +
                             "NULL AS lastupdateby " +
                            "FROM dbo.Sys_User " +
                            "ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SyUser>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyUser>("SyUser", list[rowIndex]).AutoMap().Execute();
        }
    }
}
