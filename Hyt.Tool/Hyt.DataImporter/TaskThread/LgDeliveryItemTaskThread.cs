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
    public class LgDeliveryItemTaskThread :BaseTaskThread
    {
        
        public LgDeliveryItemTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "配送明细"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgDeliveryItem> list;

        protected override void Read()
        {
            string sSql = "select DLSysNo as deliverysysno," + 
                                    "NULL as transactionsysno," + 
                                    "(CASE WHEN ItemType=1 THEN 10 " +
		                                "ELSE " +
			                                "20 " +
                                      "END) as notetype," + 
                                        "NULL AS  notesysno," +	 
                        "(CASE WHEN PayAmt=0  THEN   0 " +             
	                        "ELSE " +
	                            "1 " +
                            "END)  AS  iscod," +						 
                         "0  AS  stockoutamount, " +			 	
                         "PayAmt AS  receivable," +				
                        "(CASE WHEN PayAmt=0  THEN  10 " +
		                        "WHEN PayAmt>0 THEN 20 " +
                        "end ) as paymenttype," +
                        "NULL AS payno," +
                        "(CASE WHEN  a.status=-1 THEN -10 " +
		                        "WHEN a.Status=1 AND (b.Status=0 OR b.Status=1) THEN 10 " +
                         "END) AS STATUS," +
                        "NULL AS  remarks," + 
                        "NULL AS  createdby," + 
                        "NULL AS  createddate," + 
                        "NULL AS  lastupdateby," + 
                        "NULL AS  lastupdatedate," + 
                        "NULL AS  addresssysno, " +
                        "NULL AS  expressno " +
                    "from  DL_Item a LEFT JOIN dbo.DL_Master b ON a.DLSysNo=b.SysNo " +
                        "ORDER BY a.SysNo " ;

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgDeliveryItem>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgDeliveryItem>("LgDeliveryItem", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
