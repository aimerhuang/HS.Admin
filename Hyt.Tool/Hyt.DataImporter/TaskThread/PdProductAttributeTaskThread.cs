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
    public class PdProductAttributeTaskThread :BaseTaskThread
    {
        public PdProductAttributeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品属性关联"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdProductAttribute> list;

        protected override void Read()
        {

            string sSql = "SELECT  " +
                            "a.Attribute2SysNo AS attributesysno," +
                             "a.ProductSysNo AS productsysno, " +
                              "b.Attribute2Name AS attributename," +
                               "c.SysNo AS AttributeGroupSysNo," +
                                "d.SysNo AS attributeoptionsysno, " +
                                "a.Attribute2Value  AS attributetext," +
                                "NULL AS attributeimage," +
                                "(CASE b.Attribute2Type WHEN 0 THEN 10 " +
						        "WHEN 1 THEN 30 " +
						        "WHEN 2 THEN 30 " +
                                "end) AS attributetype," +
                                "b.OrderNum AS displayorder," +
                                "1 AS status," +
                                "2 AS createdby," +
                                "GETDATE() AS createddate," +
                                "2 AS lastupdateby," +  
                                "GETDATE() AS lastupdatedate " +
                                "FROM dbo.Product_Attribute2 a INNER JOIN Category_Attribute2 AS b ON a.Attribute2SysNo=b.SysNo " +
								                            "INNER JOIN dbo.Category_Attribute1 AS c ON b.A1SysNo=c.SysNo " +		
								                            "INNER JOIN dbo.Category_Attribute2_Option AS d ON a.Attribute2SysNo=d.Attribute2SysNo AND a.Attribute2OptionSysNo=d.SysNo ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdProductAttribute>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdProductAttribute>("PdProductAttribute", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
