using Hyt.DataAccess.Feedback;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Feedback
{
    public class CrFeedbackTypeDaoImpl : ICrFeedbackTypeDao
    {
        public override IList<CrFeedbackType> GetFeedbackTypeList()
        {
            return Context.Sql("SELECT * FROM CrFeedbackType")
                           .QueryMany<CrFeedbackType>();
        }

        public override IList<CrFeedbackType> GetFeedbackTypeList(CustomerStatus.意见反馈类型来源 source)
        {
            return Context.Sql("SELECT * FROM CrFeedbackType WHERE Source=@Source")
                .Parameter("Source", (int)source)
                           .QueryMany<CrFeedbackType>();
        }
    }
}
