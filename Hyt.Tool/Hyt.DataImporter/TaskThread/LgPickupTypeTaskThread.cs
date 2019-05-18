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
    public class LgPickupTypeTaskThread:BaseTaskThread
    {

        public LgPickupTypeTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "取件方式"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<LgPickupType> list;

        protected override void Read()
        {

            string sSql = "SELECT " +
                                "ParentSysNo," +
                                "PickupTypeName," +
                                "PickupTypeDescription," +
                                "PickupLevel," +
                                "PickupTime," +
                                "TraceUrl," +
                                "DisplayOrder," +
                                "Provider," +
                                "IsOnlineVisible," +
                                "Freight," +
                                "Status " +
                            "from LgPickupType " +
                            " order by Sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<LgPickupType>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<LgPickupType>("LgPickupType", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
