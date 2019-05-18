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
    public class BsDeliveryPaymentTaskThread:BaseTaskThread
    {
        public BsDeliveryPaymentTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "配送支付对应关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<BsDeliveryPayment> list;

        protected override void Read()
        {
            
            string sSql = "select * from BsDeliveryPayment order by sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<BsDeliveryPayment>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<BsDeliveryPayment>("BsDeliveryPayment", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }

    }
}
