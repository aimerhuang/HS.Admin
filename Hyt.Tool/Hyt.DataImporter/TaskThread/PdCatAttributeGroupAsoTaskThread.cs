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
    public  class PdCatAttributeGroupAsoTaskThread :BaseTaskThread
    {
        public PdCatAttributeGroupAsoTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品分类与属性分类关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdCatAttributeGroupAso> list;

        protected override void Read()
        {

            string sSql = "select " + 
                            "b.orderNum AS displayorder," +                                       
                            "b.C3SysNo AS productcategorysysno," +
                            //"b.Attribute1ID AS AttributeGroupSysNo," +
                            "2 as createdby," +			
                            "GETDATE() as createddate," +
                            "2 as lastupdateby," +
                            "GETDATE() as lastupdatedate " +
    " from  dbo.Category_Attribute2 a inner join  dbo.Category_Attribute1 b on a.A1SysNo=b.sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdCatAttributeGroupAso>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdCatAttributeGroupAso>("PdCatAttributeGroupAso", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
