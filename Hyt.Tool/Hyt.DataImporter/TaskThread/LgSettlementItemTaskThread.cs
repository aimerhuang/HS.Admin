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
    public class LgSettlementItemTaskThread :BaseTaskThread
    {
        public LgSettlementItemTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "结算单明细"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgSettlementItem> list;

        protected override void Read()
        {

            string sSql = "SELECT " +
	                        "dssysno as settlementsysno, " +
	                        "DLSysNo as deliverysysno, " +
	                        "NULL AS  stockoutsysno, " +
	                        "paytype as paytype, " +
	                        "PayAmt as payamount, " +
	                        "NULL AS  payno, " +
	                        "NULL AS  createdby, " +
	                        "NULL AS createddate, " +
	                        "NULL AS lastupdateby, " +
	                        "NULL AS lastupdatedate, " +
	                        "NULL AS transactionsysno " +
                        "FROM DS_Item " +
                        "ORDER BY sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgSettlementItem>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgSettlementItem>("LgSettlementItem", list[rowIndex]).AutoMap(x => x.SysNo, x => x.DeliveryItemStatus).Execute();
        }
    }
}
