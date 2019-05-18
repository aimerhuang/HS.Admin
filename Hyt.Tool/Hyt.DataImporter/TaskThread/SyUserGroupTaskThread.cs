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
    public class SyUserGroupTaskThread:BaseTaskThread
    {
        public SyUserGroupTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "用户分组"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyUserGroup> list;

        protected override void Read()
        {

            string sSql = "SELECT * FROM dbo.SyUserGroup ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SyUserGroup>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyUserGroup>("SyUserGroup", list[rowIndex]).AutoMap().Execute();
        }
    }
}
