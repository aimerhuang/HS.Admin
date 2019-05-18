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
    /// 惠源币数据访问  
    /// </summary>
    /// <remarks>2013-08-21 苟治国 创建</remarks>
    public class CrExperienceCoinLogDaoImpl : ICrExperienceCoinLogDao
    {
        /// <summary>
        /// 根据条件获取惠源币的列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="exp">惠源币查询条件</param>
        /// <returns>惠源币列表</returns>
        /// <remarks>2013-08-21 苟治国 创建</remarks>
        public override Pager<Model.CrExperienceCoinLog> SeachPager(Pager<CrExperienceCoinLog> pager, ParaCrExperienceCoinLogFilter exp)
        {
            #region 测试SQL
            /*增加惠源币*/
            //select * from CrExperienceCoinLog ce where customersysno=1004 and Increased > 0
            /*减少惠源币*/
            //select * from CrExperienceCoinLog ce where customersysno=1004 and Decreased > 0
            #endregion

            #region sql条件
            string sqlWhere = "";
            if (exp.Type == 0)
                sqlWhere = @"(@CustomerSysNo=-1 or ce.CustomerSysNo =@CustomerSysNo) and (@beginTime is null or ce.CreatedDate>=@beginTime) and (@endTime is null or ce.CreatedDate<=@endTime)";
            else if (exp.Type == 10)
                sqlWhere = @"(@CustomerSysNo=-1 or ce.CustomerSysNo =@CustomerSysNo) and (@beginTime is null or ce.CreatedDate>=@beginTime) and (@endTime is null or ce.CreatedDate<=@endTime) and ce.Increased > 0";
            else if (exp.Type == 20)
                sqlWhere = @"(@CustomerSysNo=-1 or ce.CustomerSysNo =@CustomerSysNo) and (@beginTime is null or ce.CreatedDate>=@beginTime) and (@endTime is null or ce.CreatedDate<=@endTime) and ce.Decreased <> 0";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CrExperienceCoinLog>("ce.*")
                                    .From("CrExperienceCoinLog ce")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", exp.CustomerSysNo)
                                    .Parameter("beginTime", exp.BeginDate)
                                    .Parameter("endTime", exp.EndDate)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("ce.CreatedDate desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("CrExperienceCoinLog ce")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", exp.CustomerSysNo)
                                    .Parameter("beginTime", exp.BeginDate)
                                    .Parameter("endTime", exp.EndDate)
                                    .QuerySingle();
            }
            return pager;
        }
    }
}
