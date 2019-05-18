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
    public class WhstockOutItemTaskThread :BaseTaskThread
    {
        public WhstockOutItemTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "出库单明细"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<WhStockOutItem> list;

        protected override void Read()
        {
            string sSql = "select " +
                            "b.StockSysNo as stockoutsysno," + 
                            "NULL as transactionsysno, " +
                            "a.ReferSysNo AS ordersysno, " +
                            "productsysno, " +
                            "productname, " +
                            "Quantity as productquantity, " +
                            "weight, " +
                            "null AS measurement," + 
                            "price as originalprice," + 
                            "CashBackAmt AS  realsalesamount," + 
                            "NULL AS  remarks, " +
                            "NULL AS  status," +                    
                            "NULL AS  createdby," + 
                            "NULL AS  createddate," + 
                            "NULL AS  lastupdateby," + 
                            "NULL AS  lastupdatedate," + 
                            "NULL AS  returnquantity " +
                            "FROM  DO_Item a LEFT JOIN dbo.DO_Master b ON a.DOSysNo=b.SysNo " +
                            "ORDER BY a.sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<WhStockOutItem>();
        }

        protected override void Write(int rowIndex)
        {
            //DataProvider.OracleInstance.Insert<WhStockOutItem>("WhStockOutItem", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
            DataProvider.OracleInstance.Insert<WhStockOutItem>("WhStockOutItem", list[rowIndex]).AutoMap(x => x.SysNo,p => p.IsScaned, p => p.ScanedQuantity).Execute();
        }
    }
}
