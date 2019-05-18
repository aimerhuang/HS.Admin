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
    /// 经验积分数据访问  
    /// </summary>
    /// <remarks>2013-11-1 苟治国 创建</remarks>
    public class CrExperiencePointLogDaoImpl : ICrExperiencePointLogDao
    {
        /// <summary>
        /// 根据条件获取经验积分的列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="exp">经验积分查询条件</param>
        /// <returns>经验积分列表</returns>
        /// <remarks>2013-11-1 苟治国 创建</remarks>
        public override Pager<Model.CrExperiencePointLog> SeachPager(Pager<CrExperiencePointLog> pager, ParaCrExperiencePointLogFilter exp)
        {
            #region 测试SQL
            /*增加惠源币*/
            //select * from CrExperiencePointLog ce where customersysno=1004 and Increased > 0
            /*减少惠源币*/
            //select * from CrExperiencePointLog ce where customersysno=1004 and Decreased > 0
            #endregion

            #region sql条件
            string sqlWhere = "";
            if (exp.Type == 0)
                sqlWhere = @"(@CustomerSysNo=-1 or cep.CustomerSysNo =@CustomerSysNo) and (@beginTime is null or cep.CreatedDate>=@beginTime) and (@endTime is null or cep.CreatedDate<=@endTime) and cep.Increased > 0";
            else if (exp.Type == 10)
                sqlWhere = @"(@CustomerSysNo=-1 or cep.CustomerSysNo =@CustomerSysNo) and (@beginTime is null or cep.CreatedDate>=@beginTime) and (@endTime is null or cep.CreatedDate<=@endTime) and cep.Decreased <> 0";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrExperiencePointLog>("cep.*")
                                    .From("CrExperiencePointLog cep")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", exp.CustomerSysNo)
                                    .Parameter("beginTime", exp.BeginDate)
                                    .Parameter("endTime", exp.EndDate)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cep.CreatedDate desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From("CrExperiencePointLog cep")
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
