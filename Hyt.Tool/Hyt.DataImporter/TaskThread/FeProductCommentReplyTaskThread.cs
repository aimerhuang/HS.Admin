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
    public class FeProductCommentReplyTaskThread:BaseTaskThread
    {
        public FeProductCommentReplyTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "评论回复"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FeProductCommentReply> list;

        protected override void Read()
        {
            //type 10 新闻 20 帮助  ;status 20 已审核
            string sSql = "select " + 
                                "ReviewSysNo as commentsysno," + 
                                "b.CreateCustomerSysNo as customersysno," + 
                                "replycontent," + 
                                "a.CreateDate as replydate," + 
                                "(CASE WHEN a.status=0 THEN 20 " +
		                        "WHEN a.Status=1 THEN  -10 " +
		                        "WHEN a.Status=2 THEN 10 " +
		                   "ELSE " +
			                    "-10 " +
                           "END ) AS status " +
                        "from Review_Reply a INNER JOIN dbo.Review_Master b ON a.ReviewSysNo=b.SysNo " +
                            "ORDER BY a.sysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<FeProductCommentReply>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<FeProductCommentReply>("FeProductCommentReply", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
