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
    public class FeProductCommentImageTaskThread :BaseTaskThread
    {
        public FeProductCommentImageTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "晒单图片"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FeProductCommentImage> list;

        protected override void Read()
        {
            //type 10 新闻 20 帮助  ;status 20 已审核
            string sSql = "select " +
                                "ReviewSysNo AS commentsysno, " +
                                "b.CreateCustomerSysNo as customersysno," + 
                                "Photo AS imagepath," + 
                                "(CASE WHEN a.Status=-1 THEN -10 " +
                            "ELSE " +
                                "20 " +
                        "END) AS  status " +
                        "from ReviewPhoto a INNER JOIN Review_Master b ON a.ReviewSysNo=b.SysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<FeProductCommentImage>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<FeProductCommentImage>("FeProductCommentImage", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
