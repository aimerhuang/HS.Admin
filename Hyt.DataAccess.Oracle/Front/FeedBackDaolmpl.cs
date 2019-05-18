using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Feedback;
using Hyt.Model.Generated;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Front
{
    public class FeedBackDaolmpl : FeedBackDao
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="State">状态</param>
        /// <param name="count">抛出总条数</param>
        /// <returns>商品信息通知列表</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override IList<FFeedBack> Seach(int State, Pager<FFeedBack> pager)
        {
            using (var content = Context.UseSharedConnection(true))
            {
                string sql = "";
                if (State != -1)
                    sql += string.Format("and State={0}", State);

                pager.Rows =
                    content.Select<FFeedBack>("dsa.*")
                           .From("FeedBack dsa")
                           .Where("dsa.IsDelete=0 " + sql + "")
                           .OrderBy("dsa.FeedAddTime desc")
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .QueryMany();

                pager.TotalRows =
                    content.Sql(@"select count(dsa.sysno) from FeedBack dsa where dsa.IsDelete=0 " + sql + "")
                           .QuerySingle<int>();
            }

            return pager.Rows;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public override int Update(string sysNoItems)
        {
            return Context.Sql("update FeedBack set State=1 where sysNo in(" + sysNoItems + ")")
                //.Parameter("SysNo", sysNoItems)
                   .Execute();
        }
    }
}
