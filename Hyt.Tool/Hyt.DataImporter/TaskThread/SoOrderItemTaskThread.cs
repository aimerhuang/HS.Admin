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
    public  class SoOrderItemTaskThread:BaseTaskThread
    {
        public SoOrderItemTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "销售单明细"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SoOrderItem> list;

        protected override void Read()
        {
            //基础价格(0) 配送员信用价格(70) 会员等级价格(10)
            string sSql = "select  " +
                                "SOSysNo AS  ordersysno, " +
                                "ProductSysNo AS  productsysno," +
                                "b.ProductName," + 
                                "a.Quantity," +
                                "OrderPrice AS originalprice," + 
                                "Price AS  realsalesamount," + 
                                "OutStockQty AS  realstockoutquantity," + 
                                "null transactionsysno," + 
                                "NULL AS  productsalestype," +                                  
                                "NULL AS  productsalestypesysno " +
                          "from dbo.SO_Item a INNER JOIN product AS b ON a.ProductSysNo=b.SysNo " +
                            "ORDER BY a.SysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SoOrderItem>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SoOrderItem>("SoOrderItem", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
