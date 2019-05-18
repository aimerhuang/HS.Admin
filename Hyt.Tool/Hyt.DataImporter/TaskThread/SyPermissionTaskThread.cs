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
    public  class SyPermissionTaskThread :BaseTaskThread
    {
        public SyPermissionTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "系统许可"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyPermission> list;

        protected override void Read()
        {

            string sSql = "SELECT *FROM SyPermission ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SyPermission>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyPermission>("SyPermission", list[rowIndex]).AutoMap().Execute();
        }
    }
}
