using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Tuan;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Tuan
{
    /// <summary>
    /// 团购数据访问  
    /// </summary>
    /// <remarks>2013-09-03 苟治国 创建</remarks>
    public class GsGroupShoppingDaoImpl : IGsGroupShoppingDao
    {
        /// <summary>
        /// 今日推荐团购列表
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public override Pager<GsGroupShopping> GetPagingList(Pager<GsGroupShopping> pager)
        {
            #region sql条件

            string sqlWhere =@"(@GroupType is null or ggs.GroupType =@GroupType) and (@Status is null or ggs.Status =@Status) and (ggs.StartTime >= @StartTime) and (ggs.StartTime <= @EndTime)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<GsGroupShopping>("ggs.*")
                                     .From("GsGroupShopping ggs")
                                     .Where(sqlWhere)
                                     .Parameter("GroupType", pager.PageFilter.GroupType)
                                   //  .Parameter("GroupType", pager.PageFilter.GroupType)
                                     .Parameter("Status", pager.PageFilter.Status)
                                    // .Parameter("Status", pager.PageFilter.Status)
                                     .Parameter("StartTime", pager.PageFilter.StartTime)
                                     .Parameter("EndTime", pager.PageFilter.EndTime)
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .OrderBy("ggs.DisplayOrder desc")
                                     .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                          .From("GsGroupShopping ggs")
                                          .Where(sqlWhere)
                                          .Parameter("GroupType", pager.PageFilter.GroupType)
                                         // .Parameter("GroupType", pager.PageFilter.GroupType)
                                          .Parameter("Status", pager.PageFilter.Status)
                                         // .Parameter("Status", pager.PageFilter.Status)
                                          .Parameter("StartTime", pager.PageFilter.StartTime)
                                          .Parameter("EndTime", pager.PageFilter.EndTime)
                                          .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 团购分页列表
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public override Pager<GsGroupShopping> GetList(Pager<GsGroupShopping> pager)
        {
            #region sql条件

            string sqlWhere =
                @"(@GroupType is null or ggs.GroupType =@GroupType) and (@Status is null or ggs.Status =@Status) and (@StartTime is null or ggs.StartTime >= @StartTime) and (@EndTime is null or ggs.EndTime <= @EndTime)";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<GsGroupShopping>("ggs.*")
                                     .From("GsGroupShopping ggs")
                                     .Where(sqlWhere)
                                     .Parameter("GroupType", pager.PageFilter.GroupType)
                                     //.Parameter("GroupType", pager.PageFilter.GroupType)
                                     .Parameter("Status", pager.PageFilter.Status)
                                    // .Parameter("Status", pager.PageFilter.Status)
                                     .Parameter("StartTime", pager.PageFilter.StartTime)
                                    // .Parameter("StartTime", pager.PageFilter.StartTime)
                                     .Parameter("EndTime", pager.PageFilter.EndTime)
                                    // .Parameter("EndTime", pager.PageFilter.EndTime)
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .OrderBy("ggs.DisplayOrder desc")
                                     .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                          .From("GsGroupShopping ggs")
                                          .Where(sqlWhere)
                                          .Parameter("GroupType", pager.PageFilter.GroupType)
                                          //.Parameter("GroupType", pager.PageFilter.GroupType)
                                          .Parameter("Status", pager.PageFilter.Status)
                                          //.Parameter("Status", pager.PageFilter.Status)
                                          .Parameter("StartTime", pager.PageFilter.StartTime)
                                         // .Parameter("StartTime", pager.PageFilter.StartTime)
                                          .Parameter("EndTime", pager.PageFilter.EndTime)
                                          //.Parameter("EndTime", pager.PageFilter.EndTime)
                                          .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 团购分页列表 已审核,未过期的
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        public override Pager<GsGroupShopping> GetGroupShoppingList(Pager<GsGroupShopping> pager)
        {
            #region sql条件
            string sqlWhere =
                @"(@GroupType is null or ggs.GroupType =@GroupType) 
                        and (@Status is null or ggs.Status =@Status) 
                        and (@StartTime is null or ggs.StartTime >= @StartTime) 
                        and (2EndTime is null or ggs.EndTime >= @EndTime)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<GsGroupShopping>("ggs.*")
                                     .From("GsGroupShopping ggs")
                                     .Where(sqlWhere)
                                     .Parameter("GroupType", pager.PageFilter.GroupType)
                                    // .Parameter("GroupType", pager.PageFilter.GroupType)
                                     .Parameter("Status", pager.PageFilter.Status)
                                   //  .Parameter("Status", pager.PageFilter.Status)
                                     .Parameter("StartTime", pager.PageFilter.StartTime)
                                   //  .Parameter("StartTime", pager.PageFilter.StartTime)
                                     .Parameter("EndTime", pager.PageFilter.EndTime)
                                  //   .Parameter("EndTime", pager.PageFilter.EndTime)
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .OrderBy("ggs.DisplayOrder desc")
                                     .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                          .From("GsGroupShopping ggs")
                                          .Where(sqlWhere)
                                          .Parameter("GroupType", pager.PageFilter.GroupType)
                                       //   .Parameter("GroupType", pager.PageFilter.GroupType)
                                          .Parameter("Status", pager.PageFilter.Status)
                                       //   .Parameter("Status", pager.PageFilter.Status)
                                          .Parameter("StartTime", pager.PageFilter.StartTime)
                                      //    .Parameter("StartTime", pager.PageFilter.StartTime)
                                          .Parameter("EndTime", pager.PageFilter.EndTime)
                                      //    .Parameter("EndTime", pager.PageFilter.EndTime)
                                          .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 查询一条未过期的团购
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>一条未过期的团购</returns>
        /// <remarks>2013-09-24 苟治国 创建</remarks>
        public override GsGroupShopping GetGroupShopping(Pager<GsGroupShopping> pager)
        {
            return Context.Sql(@"select * from (select * from GsGroupShopping 
where Status=@Status and GroupType=@GroupType and (@StartTime is null or StartTime >= @StartTime) and (@EndTime is null or EndTime >= @EndTime) order by CreatedDate desc) a where rownum <=1")
                          .Parameter("Status", PromotionStatus.促销状态.已审)
                          .Parameter("GroupType", PromotionStatus.促销应用平台.PC网站)
                          .Parameter("StartTime", pager.PageFilter.StartTime)
                          //.Parameter("StartTime", pager.PageFilter.StartTime)
                          .Parameter("EndTime", pager.PageFilter.EndTime)
                          //.Parameter("EndTime", pager.PageFilter.EndTime)
                          .QueryMany<GsGroupShopping>().FirstOrDefault();
        }

        /// <summary>
        /// 为手机客户端查询最新的一条团购
        /// </summary>
        /// <returns>最新的一条团购信息</returns>
        /// <remarks>2013-09-24 周瑜 创建</remarks>
        public override GsGroupShopping GetNewGroupShoppingForApp()
        {
            return Context.Sql(@"select * from (select * from GsGroupShopping 
where Status=@Status and (GroupType=@GroupType or GroupType=@GroupType1)
order by CreatedDate desc) a where rownum <=1")
                          .Parameter("Status", GroupShoppingStatus.团购状态.已审)
                          .Parameter("GroupType", GroupShoppingStatus.团购类型.手机App)
                          .Parameter("GroupType1", GroupShoppingStatus.团购类型.全部)
                          .QueryMany<GsGroupShopping>().FirstOrDefault();
        }

        /// <summary>
        /// 获取团购数量
        /// </summary>
        /// <param name="type">团购类型</param>
        /// <param name="status">团购状态</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>团购数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public override int GetCount(GroupShoppingStatus.团购类型 type, GroupShoppingStatus.团购状态 status,
                                     DateTime? startTime,
                                     DateTime? endTime)
        {
            #region sql条件

            const string sqlWhere =
                @"(@GroupType is null or ggs.GroupType =@GroupType) and (@Status is null or ggs.Status =@Status) and (@StartTime is null or ggs.StartTime >= @StartTime) and (@EndTime is null or ggs.EndTime <= @EndTime)";

            #endregion

            return Context.Select<int>("count(1)")
                          .From("GsGroupShopping ggs")
                          .Where(sqlWhere)
                          .Parameter("GroupType", (int) type)
                          //.Parameter("GroupType", (int) type)
                          //.Parameter("Status", (int) status)
                          .Parameter("Status", (int) status)
                          //.Parameter("StartTime", startTime)
                          .Parameter("StartTime", startTime)
                          //.Parameter("EndTime", endTime)
                          .Parameter("EndTime", endTime)
                          .QuerySingle();
        }

        /// <summary>
        /// 通过商品系统编号获取团购系统编号
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品系统编号 0：表示没有团购</returns>
        /// <remarks>2013-09-09 邵斌 创建</remarks>
        public override int GetGroupShoppingSysNoByProduct(int productSysNo)
        {
            #region 测试 SQL 通过商品查询团购明细表并关联查询团购表查询团购状态

            /*
            select
                gs.sysno 
            from 
                GsGroupShoppingItem gsi
                inner join GsGroupShopping gs on gs.sysno = gsi.groupshoppingsysno
            where 
                gsi.productsysno=41 and gs.status = 20 and sysdate between gs.starttime and gs.endtime
                order by gs.DisplayOrder,gs.LastUpdateDate desc
             */

            return Context.Sql(@"
                        select
                            gs.sysno 
                        from 
                            GsGroupShoppingItem gsi
                            inner join GsGroupShopping gs on gs.sysno = gsi.groupshoppingsysno
                        where 
                            gsi.productsysno=@0 and gs.status = @1 and getdate() between gs.starttime and gs.endtime
                            order by gs.DisplayOrder,gs.LastUpdateDate desc
                    ", productSysNo, (int) GroupShoppingStatus.团购状态.已审).QuerySingle<int>();

            #endregion
        }

        /// <summary>
        /// 根据商品系统编号 获取团购价格
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns>团购价格</returns>
        /// <remarks>2013-10-31 杨浩 创建</remarks>
        public override decimal GetGroupShoppingPrice(int sysNo)
        {
            return
                Context.Sql("SELECT GroupShoppingPrice from GsGroupShoppingItem where ProductSysNo=@0", sysNo)
                       .QuerySingle<decimal>();
        }
    }
}
