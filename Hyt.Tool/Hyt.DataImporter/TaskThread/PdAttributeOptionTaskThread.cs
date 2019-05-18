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
    public  class PdAttributeOptionTaskThread :BaseTaskThread

    {
        public PdAttributeOptionTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品属性选项"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdAttributeOption> list;

        protected override void Read()
        {
            
            string sSql = "SELECT " +
                                "Attribute2OptionName AS attributetext ," +
                                "OrderNum AS displayorder ," +
                                "1 as Status ," +
                                "2 AS createdby ," +
                                "GETDATE() AS createddate ," +
                                "2 AS lastupdateby , " +
                                "GETDATE() AS lastupdatedate ," +
                                "Attribute2SysNo AS attributesysno " +
                                "FROM    dbo.Category_Attribute2_Option ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdAttributeOption>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdAttributeOption>("PdAttributeOption", list[rowIndex]).AutoMap(x=>x.SysNo).Execute();
        }
    }
}
