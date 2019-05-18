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
    public  class BsPaymentTypeTaskThread :BaseTaskThread
    {
        public BsPaymentTypeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "支付类型"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<BsPaymentType> list;

        protected override void Read()
        {
            
            string sSql = "select * from BsPaymentType order by sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<BsPaymentType>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<BsPaymentType>("BsPaymentType", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
