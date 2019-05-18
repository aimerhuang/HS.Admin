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
    public class FeCommentSupportTaskThread:BaseTaskThread
    {
        public FeCommentSupportTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "评论支持"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<FeCommentSupport> list;

        protected override void Read()
        {
            //type 10 新闻 20 帮助  ;status 20 已审核
            string sSql = "SELECT " +
                                "ReviewID as productcommentsysno, " +
                                "b.CreateCustomerSysNo as customersysno," +
                                "ApplyYesCount as supportcount," +
                                "ApplyNoCount as unsupportcount," + 
                                "CreateUser  AS createdby," + 
                                "a.CreateDate  AS createdate," + 
                                "NULL as lastupdateby," + 
                                "NULL AS  lastupdatedate " +
                            "from Review_Apply a INNER JOIN dbo.Review_Master b ON a.ReviewID=b.SysNo " +
                            "ORDER BY a.sysno "; 

            list = DataProvider.Instance.Sql(sSql).QueryMany<FeCommentSupport>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<FeCommentSupport>("FeCommentSupport", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
