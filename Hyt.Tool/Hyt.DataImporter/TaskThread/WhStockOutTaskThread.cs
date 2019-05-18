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
    public  class WhStockOutTaskThread:BaseTaskThread
    {
       
        public WhStockOutTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "出库单"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<WhStockOut> list;

        protected override void Read()
        {
            string sSql = "select SysNo, TransactionSysNo, WarehouseSysNo, OrderSysNO, ReceiveAddressSysNo, IsCOD, StockOutAmount, Receivable, Status, SignTime, IsPrintedPackageCover, IsPrintedPickupCover, CustomerMessage, PickUpDate, DeliveryRemarks, DeliveryTime, ContactBeforeDelivery, InvoiceSysNo, DeliveryTypeSysNo, Remarks, CreatedBy, CreatedDate, StockOutBy, StockOutDate, LastUpdateBy, LastUpdateDate from WhStockOut order by sysno ";
          
            list = DataProvider.Instance.Sql(sSql).QueryMany<WhStockOut>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<WhStockOut>("WhStockOut", list[rowIndex]).AutoMap(x => x.SoOrder,x=>x.Items).Execute();
        }
    }
}
