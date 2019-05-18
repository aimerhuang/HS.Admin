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
    public class LgSettlementTaskThread :BaseTaskThread
    {
        public LgSettlementTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "结算单"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgSettlement> list;

        protected override void Read()
        {

            string sSql = "SELECT " +
                            "sysno, " +
                            "FreightUserSysNo as deliveryusersysno, " +
                            "ARAmt as totalamount, " +
                            "IncomeAmt paidamount, " +
                            "(ARAmt -IncomeAmt) as codamount, " +
                            "(CASE WHEN  status=0 THEN 10 " +
	                            "WHEN Status=2 THEN 20 " +
	                            "WHEN STATUS=-1 THEN -10 " +
                                "END) AS STATUS, " +
                            "memo AS  remarks, " +
                            "AuditUserSysNo as auditorsysno, " +
                            "CreateUserSysNo as createdby,  " +
                            "CreateTime as createddate, " +
                            "null  AS lastupdateby, " +
                            "NULL AS  lastupdatedate, " +
                            "AuditTime AS auditdate " +
                       "FROM dbo.DS_Master " +
                        "ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgSettlement>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgSettlement>("LgSettlement", list[rowIndex]).AutoMap(x=>x.Items).Execute();
        }
    }
}
