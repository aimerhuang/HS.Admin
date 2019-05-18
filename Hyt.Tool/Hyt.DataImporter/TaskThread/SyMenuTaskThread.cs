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
    public class SyMenuTaskThread :BaseTaskThread
    {
        public SyMenuTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "系统菜单"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SyMenu> list;

        protected override void Read()
        {
            
            string sSql = "SELECT *FROM symenu ORDER BY sysno";

            list = DataProvider.Instance.Sql(sSql).QueryMany<SyMenu>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<SyMenu>("SyMenu", list[rowIndex]).AutoMap().Execute();
        }
    }
}
