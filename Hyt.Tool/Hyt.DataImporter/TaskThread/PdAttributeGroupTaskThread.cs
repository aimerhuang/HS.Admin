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
    public  class PdAttributeGroupTaskThread :BaseTaskThread
    {
        public PdAttributeGroupTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品属性分组"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdAttributeGroup> list;

        protected override void Read()
        {
            
            string sSql = "SELECT distinct a.Attribute1Name as Name,a.Attribute1Name  as backendname,a.OrderNum as displayorder,a.Status from  dbo.Category_Attribute1 a";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdAttributeGroup>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdAttributeGroup>("PdAttributeGroup", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
