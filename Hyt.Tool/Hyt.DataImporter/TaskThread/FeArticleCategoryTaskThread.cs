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
    public class FeArticleCategoryTaskThread:BaseTaskThread
    {
        public FeArticleCategoryTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "文章类型"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FeArticleCategory> list;

        protected override void Read()
        {
            //type 10 新闻 20 帮助  ;status 20 已审核
            string sSql = "select ROW_NUMBER() OVER(ORDER BY type ) AS SysNo,type,name,DESCRIPTION,displayorder,STATUS " +
                          "from  " +
                                "(select " +
                                    "20 AS type,title AS  name,note as description,ordernum as displayorder,20 AS STATUS from dbo.ArticleTheme " +
                                  "UNION all " +
                                    "select 10 AS type,'商城公告' AS  name,'商城公告' as description,1 as displayorder,20 AS STATUS " +
                                  "UNION all " +
                                    "select 10 AS type,'促销活动' AS  name,'促销活动' as description,2 as displayorder,20 AS STATUS " +
                                ") a"; 

            list = DataProvider.Instance.Sql(sSql).QueryMany<FeArticleCategory>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<FeArticleCategory>("FeArticleCategory", list[rowIndex]).AutoMap().Execute();
        }
    }
}
