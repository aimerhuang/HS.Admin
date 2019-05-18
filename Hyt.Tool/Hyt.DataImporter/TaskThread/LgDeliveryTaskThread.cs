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
    public class LgDeliveryTaskThread :BaseTaskThread
    {
        logRecord log = new logRecord();

        public LgDeliveryTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "配送主表"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgDelivery> list;

        protected override void Read()
        {
            
            string sSql = "select  DISTINCT " +
                                    "a.sysno," + 
                                    "c.stocksysno," + 
                                    "a.FreightUserSysNo AS  deliveryusersysno, " +
                                    "HasPaidAmt AS paidamount," +				
                                    "a.CODAmt AS codamount," +
                                    "(CASE WHEN a.status=0 THEN 10 " +
	                                "WHEN a.Status=1 THEN 20 " +
	                                "WHEN a.Status=3 THEN 30 " +
	                                "WHEN a.Status=-1 THEN -10 " +
                                    "END) AS STATUS," + 
                                    "IsAllow AS isenforceallow," + 
                                    "a.FreightUserSysNo AS deliverytypesysno," + 
                                    "a.CreateUserSysNo AS createdby," + 
                                    "a.CreateTime AS createddate," + 
                                    "UpdateFreightManUserSysNo AS lastupdateby," + 
                                    "UpdateFreightManTime AS lastupdatedate," + 
                                    "null stamp " +
                            "from DL_Master a LEFT JOIN DL_Item b ON a.SysNo=b.DLSysNo " +
						                    "LEFT JOIN do_master c on b.itemid=c.doid " +
                                "ORDER BY a.SysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgDelivery>();
        }

        protected override void Write(int rowIndex)
        {
            try
            {
                DataProvider.OracleInstance.Insert<LgDelivery>("LgDelivery", list[rowIndex]).AutoMap(x => x.LgDeliveryItemList).Execute();
            }
            catch
            {

                log.CheckLog("导入表【LgDelivery】失败，" + "系统号：" + list[rowIndex].SysNo.ToString());
            }

            
        }
    }
}
