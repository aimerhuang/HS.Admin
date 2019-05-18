using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 等级积分日志数据访问  
    /// </summary>
    /// <remarks>2013-08-22 苟治国 创建</remarks>
    public class CrLevelPointLogDaoImpl : ICrLevelPointLogDao
    {
        /// <summary>
        /// 根据条件获取等级积分日志列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="type">tab类型</param>
        /// <returns>等级积分日志列表</returns>
        /// <remarks>2013-08-22 苟治国 创建</remarks>
        public override Pager<Model.CrLevelPointLog> SeachPager(Pager<CrLevelPointLog> pager, int type)
        {
            #region 测试SQL
            /*增加惠源币*/
            //select * from CrLevelPointLog ce where customersysno=1004 and Increased > 0
            /*减少惠源币*/
            //select * from CrLevelPointLog ce where customersysno=1004 and Decreased > 0
            #endregion

            #region sql条件
            string sqlWhere = "";
            if (type == 0)
                sqlWhere = @"(@CustomerSysNo=-1 or clp.CustomerSysNo =@CustomerSysNo)";
            else if (type == 10)
                sqlWhere = @"(@CustomerSysNo=-1 or clp.CustomerSysNo =@CustomerSysNo) and clp.Increased > 0";
            else if (type == 20)
                sqlWhere = @"(@CustomerSysNo=-1 or clp.CustomerSysNo =@CustomerSysNo) and clp.Decreased <> 0";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CrLevelPointLog>("clp.*")
                                    .From("CrLevelPointLog clp")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("clp.CreatedDate desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("CrLevelPointLog clp")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                    .QuerySingle();
            }
            return pager;
        }
    }
}
