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
    public  class PdAttributeGroupAssociationTaskThread :BaseTaskThread
    {

        public PdAttributeGroupAssociationTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "属性分组关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdAttributeGroupAssociation> list;

        protected override void Read()
        {
            
            string sSql = "select " +
                            "b.orderNum AS displayorder," +           
                            "a.SysNo AS  attributesysno," +       
                           // "b.Attribute1ID AS categorysysno," +
                            "2 as createdby," +			
                            "GETDATE() as createddate," +
                            "2 as lastupdateby," +
                            "GETDATE() as lastupdatedate " +
 	                    " from  dbo.Category_Attribute2 a inner join  dbo.Category_Attribute1 b on a.A1SysNo=b.sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdAttributeGroupAssociation>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdAttributeGroupAssociation>("PdAttributeGroupAssociation", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }

    }
}
