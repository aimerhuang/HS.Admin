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
    public class LgDeliveryScopeTaskThread:BaseTaskThread
    {
        public LgDeliveryScopeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "配送范围"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgDeliveryScope> list;

        protected override void Read()
        {
            
            string sSql = "SELECT " +
                                "CitySysNo AS areasysno," +
		                        "position AS mapscope," +
		                        "comment AS DESCRIPTION," +
		                        "NULL AS  createdby," +
		                        "NULL AS createddate," +
		                        "NULL AS lastupdateby," +
		                        "NULL AS lastupdatedate " +
                           "FROM dbo.City_Map " +
						   "ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgDeliveryScope>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgDeliveryScope>("LgDeliveryScope", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
