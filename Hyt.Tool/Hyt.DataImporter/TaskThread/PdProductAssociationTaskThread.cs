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
    public  class PdProductAssociationTaskThread :BaseTaskThread
    {
        public PdProductAssociationTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdProductAssociation> list;

        protected override void Read()
        {
            
            string sSql = "SELECT " +
                            "Attribute2SysNo AS attributesysno," +
                            "ProductSysNo," +
                            "NULL AS relationcode," +
                            "1 AS displayorder," +
                            "2 AS createdby," +
                            "GETDATE() AS createddate," +
                            "2 AS lastupdateby," +
                            "GETDATE() AS lastupdatedate " +
                            "FROM  dbo.Product_Attribute2 ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdProductAssociation>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdProductAssociation>("PdProductAssociation", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }

    }
}
