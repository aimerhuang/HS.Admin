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
    public  class FnInvoiceTypeTaskThread :BaseTaskThread
    {
        public FnInvoiceTypeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "发票类型"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FnInvoiceType> list;

        protected override void Read()
        {
            string sSql = "select *from FnInvoiceType order by sysno";
            
            list = DataProvider.Instance.Sql(sSql).QueryMany<FnInvoiceType>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<FnInvoiceType>("FnInvoiceType", list[rowIndex]).AutoMap().Execute();
        }
    }
}
