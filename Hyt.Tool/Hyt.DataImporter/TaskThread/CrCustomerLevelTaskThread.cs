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
    public class CrCustomerLevelTaskThread :BaseTaskThread
    {
        public CrCustomerLevelTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "客户级别"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<CrCustomerLevel> list;

        protected override void Read()
        {

            string sSql = "SELECT *FROM CrCustomerLevel ORDER BY SYSNO";

            list = DataProvider.Instance.Sql(sSql).QueryMany<CrCustomerLevel>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<CrCustomerLevel>("CrCustomerLevel", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
