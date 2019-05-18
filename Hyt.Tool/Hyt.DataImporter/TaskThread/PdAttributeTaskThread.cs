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
    public class PdAttributeTaskThread :BaseTaskThread
    {
        public PdAttributeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品属性"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdAttribute> list;

        protected override void Read()
        {
            
            string sSql = "SELECT " +
                            "SysNo," + 
                           "Attribute2Name as AttributeName," +
                            "Attribute2Name  as backendname," + 
                            "1 AS isrelationflag, " +
                            "(case Attribute2Type when 0 then 10 " +
						    "when 1 then 30 " +
						    "when 2 then 30 " +
                            " end) as attributetype," +		
                            "1 AS issearchkey, " +
                            "status, " +
                            "2 as createdby, " +
                            "GETDATE() AS  createddate, " +
                            "2 AS lastupdateby, " +
                            "GETDATE() lastupdatedate " +
                            
                            "FROM  dbo.Category_Attribute2 " +
                            "ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdAttribute>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdAttribute>("PdAttribute", list[rowIndex]).AutoMap().Execute();
        }
    }
}
