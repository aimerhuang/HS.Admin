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
    public class PdCategoryAssociationTaskThread :BaseTaskThread
    {
        public PdCategoryAssociationTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品分类关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdCategoryAssociation> list;

        protected override void Read()
        {
            
            string sSql = "SELECT " +
                            "C3SysNo AS categorysysno," +
                            "sysno AS ProductSysNo, " +
                            "OrderNum AS displayorder," +
                            "1 AS IsMaster," +	
                            "2 AS createdby," +
                            "GETDATE() AS createddate," +
                            "2 AS lastupdateby," +
                            "GETDATE() AS lastupdatedate " +
                            "FROM dbo.Product ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdCategoryAssociation>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdCategoryAssociation>("PdCategoryAssociation", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
