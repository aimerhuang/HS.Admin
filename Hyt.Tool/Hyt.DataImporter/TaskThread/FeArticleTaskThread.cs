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
    public class FeArticleTaskThread :BaseTaskThread
    {
        logRecord log = new logRecord();

        public FeArticleTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "文章"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FeArticle> list;

        protected override void Read()
        {
            //type 10 新闻 20 帮助  ;status 20 已审核
            string sSql = "SELECT *FROM  FeArticle";

            list = DataProvider.Instance.Sql(sSql).QueryMany<FeArticle>();
        }

        protected override void Write(int rowIndex)
        {
            try
            {
                DataProvider.OracleInstance.Insert<FeArticle>("FeArticle", list[rowIndex]).AutoMap().Execute();
            }
            catch
            {

                log.CheckLog("导入表【CrReceiveAddress】失败，" + "系统号：" + list[rowIndex].SysNo.ToString());
            }
        }
    }
}
