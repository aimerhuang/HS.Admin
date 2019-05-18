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
    public class PdBrandTaskThread :BaseTaskThread
    {
        public PdBrandTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品品牌"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdBrand> list;

        protected override void Read()
        {
            
            string sSql = "select *from PdBrand order by sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdBrand>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdBrand>("PdBrand", list[rowIndex]).AutoMap().Execute();
        }
    }
}
