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
    public class CrCustomerQuestionTaskThread:BaseTaskThread
    {
        public CrCustomerQuestionTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "客户疑问"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<CrCustomerQuestion> list;

        protected override void Read()
        {
            
            string sSql = "select " +
                            "CreateCustomerSysNo as customersysno," +
                            "ReferenceSysNo as productsysno," +
                            "(CASE WHEN ReferenceType=5 THEN 10 " +			 	
		                        "WHEN ReferenceType =6 THEN 30 " +
		                         "WHEN ReferenceType=3 THEN 20 " +
		                        "ELSE " +
			                    "40 " + 
	                        "END) AS questiontype," +
                            "content1 as question," +
                            "a.createdate as questiondate, " +
                            "b.CreateUserSysNo as answersysno, " +
                            "b.ReplyContent AS answer," + 
                            "b.CreateDate AS answerdate," + 
                            "(CASE WHEN a.status=-2 THEN -10 " +
		                            "WHEN (a.Status=-1 OR a.Status=1) THEN 10 " +
		                            "WHEN (a.status=2 OR a.Status=2) THEN 20 " +
	                        "END) AS status " +
                        "from Review_Master  a left JOIN Review_Reply b ON a.SysNo=b.ReviewSysNo " +
                        "WHERE ReviewType=4 " +
                        "ORDER BY a.sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<CrCustomerQuestion>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<CrCustomerQuestion>("CrCustomerQuestion", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
