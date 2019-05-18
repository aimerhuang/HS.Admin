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
    public class SyUserWarehouseTaskThread :BaseTaskThread
    {
        public SyUserWarehouseTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "用户仓库"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyUserWarehouse> list;

        protected override void Read()
        {
            string sSql = "SELECT " +
                            "UserSysNo," +
                            "StockSysno AS WarehouseSysno," +
                            "NULL AS CREATEDDATE," +
                            "NULL AS CREATEDBY," +
                            "NULL AS LASTUPDATEDATE," +
                            "NULL AS  LASTUPDATEBY " +
                        "FROM dbo.Sys_User_Stock " +
                        " order by UserSysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SyUserWarehouse>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyUserWarehouse>("SyUserWarehouse", list[rowIndex]).AutoMap(x=>x.SysNo).Execute();
        }
    }
}
