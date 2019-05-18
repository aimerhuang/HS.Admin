using Hyt.DataAccess.Feedback;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Feedback
{
    public class CrFeedbackDaoImpl : ICrFeedbackDao
    {
        public override int Create(CrFeedback model)
        {
            var result = Context.Insert<CrFeedback>("CrFeedback", model)
                                    .AutoMap(x => x.SysNo)
                                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        public override void GetFeedbacks(ref Pager<CBCrFeedback> pager)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows =
                    context.Select<CBCrFeedback>("*")
                           .From("CrFeedback")
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("CreatedDate desc")
                           .QueryMany();

                pager.TotalRows = context.Select<int>("COUNT(1)")
                                         .From("CrFeedback")
                                         .QuerySingle();
            }
        }

        public override int Update(CrFeedback model)
        {
            int rowsAffected = Context.Update<CrFeedback>("CrFeedback", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
    }
}
