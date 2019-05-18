using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 广告项据访问 抽象类
    /// </summary>
    /// <remarks>2013-06-14 苟治国 创建</remarks>
    public class FeAdvertItemDaoImpl : IFeAdvertItemDao
    {
        /// <summary>
        /// 查看广告项
        /// </summary>
        /// <param name="sysNo">广告项编号</param>
        /// <returns>广告项</returns>
        /// <remarks>2013－06-14 苟治国 创建</remarks>
        public override Model.FeAdvertItem GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeAdvertItem where SysNO = @0", sysNo).QuerySingle<Model.FeAdvertItem>();
        }

        /// <summary>
        /// 根据条件获取广告项的列表
        /// </summary>
        /// <param name="pager">广告项查询条件</param>
        /// <param name="para">参数</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        public override Pager<Model.CBFeAdvertItem> Seach(Pager<CBFeAdvertItem> pager, ParaFeAdvertItem para)
        {
            #region sql条件

            var sqlWhere = "1=1";

            //判断是否绑定所有分销商
            if (!pager.PageFilter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (pager.PageFilter.IsBindDealer)
                {
                    sqlWhere += " and d.SysNo = " + pager.PageFilter.DealerSysNo;
                }
                else
                {
                    sqlWhere += " and d.CreatedBy = " + pager.PageFilter.DealerCreatedBy;
                }
            }
            if (pager.PageFilter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and d.SysNo = " + pager.PageFilter.SelectedDealerSysNo;
            }
            sqlWhere += @" and (@GroupSysNo is null or fe.GroupSysNo =@GroupSysNo)
                           and (@Status=-1 or fe.Status =@Status)
                           and ((@LinkTitle='' or fe.LinkTitle like @LinkTitle1) or (@Name='' or fe.Name like @Name1))";

            if (para.StartTime != null && para.StartTime != DateTime.MinValue)
                sqlWhere += " and fe.BeginDate >= @beginDate ";
            if (para.EndTime != null && para.EndTime != DateTime.MinValue)
                sqlWhere += "  and fe.EndDate <= @endDate ";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                if (para.StartTime == null || para.StartTime == DateTime.MinValue)
                {
                    para.StartTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                if (para.EndTime == null || para.EndTime == DateTime.MinValue)
                {
                    para.EndTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }

                pager.Rows = context.Select<CBFeAdvertItem>("fe.*,fg.Type,d.DealerName")
                    .From(@"FeAdvertItem fe LEFT JOIN FeAdvertGroup fg ON fe.GroupSysNo=fg.SysNO 
                         left join DsDealer d on fe.DealerSysNo = d.SysNo")
                    .Where(sqlWhere)
                    .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                    .Parameter("DealerCreatedBy", pager.PageFilter.DealerCreatedBy)
                    .Parameter("GroupSysNo", pager.PageFilter.GroupSysNo)
                    .Parameter("Status", pager.PageFilter.Status)
                    .Parameter("LinkTitle", (pager.PageFilter.LinkTitle ?? ""))
                    .Parameter("LinkTitle1", "%" + (pager.PageFilter.LinkTitle ?? "") + "%")
                    .Parameter("Name", (pager.PageFilter.Name ?? ""))
                    .Parameter("Name1", "%" + (pager.PageFilter.Name ?? "") + "%")
                    .Parameter("beginDate", para.StartTime.ToString())
                    .Parameter("endDate", para.EndTime.ToString())
                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("fe.DisplayOrder desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                    .From(@"FeAdvertItem fe LEFT JOIN FeAdvertGroup fg ON fe.GroupSysNo=fg.SysNO 
                          left join DsDealer d on fe.DealerSysNo = d.SysNo")
                    .Where(sqlWhere)
                    .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                    .Parameter("DealerCreatedBy", pager.PageFilter.DealerCreatedBy)
                    .Parameter("GroupSysNo", pager.PageFilter.GroupSysNo)
                    .Parameter("Status", pager.PageFilter.Status)
                    .Parameter("LinkTitle", (pager.PageFilter.LinkTitle ?? ""))
                    .Parameter("LinkTitle1", "%" + (pager.PageFilter.LinkTitle ?? "") + "%")
                    .Parameter("Name", (pager.PageFilter.Name ?? ""))
                    .Parameter("Name1", "%" + (pager.PageFilter.Name ?? "") + "%")
                    .Parameter("beginDate", para.StartTime)
                    .Parameter("endDate", para.EndTime)
                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 根据广告组获取所有广告项分类
        /// </summary>
        /// <param name="groupSysNo">广告组编号</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public override IList<FeAdvertItem> GetListByGroup(int groupSysNo)
        {
            #region sql条件
            string sql = @"(@GroupSysNo=-1 or GroupSysNo=@GroupSysNo)";
            #endregion

            var list = Context.Select<FeAdvertItem>("fa.*")
                                      .From("FeAdvertItem fa")
                                      .Where(sql)
                                      .Parameter("GroupSysNo", groupSysNo)
                                      .OrderBy("DisplayOrder desc").QueryMany();
            return list;
        }

        /// <summary>
        /// 新增广告项
        /// </summary>
        /// <param name="model">广告项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override int Insert(Model.FeAdvertItem model)
        {
            var result = Context.Insert<FeAdvertItem>("FeAdvertItem", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新广告项
        /// </summary>
        /// <param name="model">广告项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override int Update(Model.FeAdvertItem model)
        {
            int rowsAffected = Context.Update<Model.FeAdvertItem>("FeAdvertItem", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除广告项
        /// </summary>
        /// <param name="sysNo">广告项编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("FeAdvertItem")
                                      .Where("sysNo", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 同步总部已审核的广告
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public override int ProCreateFeAdvertItem(int GroupSysNo,int DealerSysNo, int CreatedBy)
        {
            string Sql = string.Format("pro_CreateFeAdvertItem {0},{1},{2}", GroupSysNo, DealerSysNo, CreatedBy);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }
    }
}
