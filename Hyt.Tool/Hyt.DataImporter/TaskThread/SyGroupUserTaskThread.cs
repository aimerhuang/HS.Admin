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
    public class SyGroupUserTaskThread:BaseTaskThread
    {
        public SyGroupUserTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "分组用户"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyGroupUser> list;

        protected override void Read()
        {
            string sSql = "SELECT *FROM SyGroupUser ORDER BY sysno";
            
            list = DataProvider.Instance.Sql(sSql).QueryMany<SyGroupUser>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyGroupUser>("SyGroupUser", list[rowIndex]).AutoMap().Execute();
        }
    }
}
