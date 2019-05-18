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
    public class LgDeliveryTypeTaskThread :BaseTaskThread
    {
        public LgDeliveryTypeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "配送类型"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgDeliveryType> list;

        protected override void Read()
        {
            
            string sSql = "SELECT * FROM  dbo.LgDeliveryType ORDER BY sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgDeliveryType>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgDeliveryType>("LgDeliveryType", list[rowIndex]).AutoMap().Execute();
        }

    }
}
