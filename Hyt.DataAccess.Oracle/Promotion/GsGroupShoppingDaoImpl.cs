using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 团购
    /// </summary>
    /// <remarks>2013-08-20 朱家宏 创建</remarks>
    public class GsGroupShoppingDaoImpl : IGsGroupShoppingDao
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-20 朱家宏 创建</remarks>
        public override Pager<GsGroupShopping> Query(ParaGroupShoppingFilter filter)
        {
            const string sql = @"(select * from gsgroupshopping a 
                                where 
                                (@0 is null or charindex(a.title,@0)>0) and 
                                (@1 is null or exists (select 1 from splitstr(@1,',') tmp where tmp.col = a.status)) and            --
                                (@2 is null or a.StartTime>=@2) and                                                                               --日期(起)
                                (@3 is null or a.EndTime<@3)                                                                                        --日期(止) 
                                ) tb";

            var statuses = (filter.Statuses != null && filter.Statuses.Count > 0) ? string.Join(",", filter.Statuses) : null;

            var paras = new object[]
                {
                    filter.Title,
                    statuses,
                    filter.BeginDate,
                    filter.EndDate
                };

            var dataList = Context.Select<GsGroupShopping>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<GsGroupShopping>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 插入团购表
        /// </summary>
        /// <param name="entity"团购表实体</param>
        /// <returns>团购表实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public override GsGroupShopping InsertEntity(GsGroupShopping entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("GsGroupShopping", entity)
                                             .AutoMap(o => o.SysNo)
                                            .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <returns>团购的覆盖地区列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public IList<BsArea> GetAreaByGroupShoppingSysNo()
        {
            return GetAreaByGroupShoppingSysNo(0);
        }

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <param name="groupShoppingSysNo">团购编号</param>
        /// <returns>团购的覆盖地区列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public override IList<BsArea> GetAreaByGroupShoppingSysNo(int groupShoppingSysNo)
        {
            return Context.Sql(@"
select b.* from GsSupportArea a inner join bsarea b
on a.areasysno = b.sysno
where a.GroupShoppingSysNo = @sysno")
                       .Parameter("sysno", groupShoppingSysNo)
                          .QueryMany<BsArea>();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public override void Update(GsGroupShopping model)
        {
            Context.Update("GsGroupShopping", model)
                  .AutoMap(o => o.SysNo)
                  .Where("SysNo", model.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 根据团购系统编号更新已团数量
        /// </summary>
        /// <param name="sysNo">团购系统编号</param>
        /// <param name="quantity">已团数量</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public override void UpdateHaveQuantity(int sysNo, int quantity)
        {
            const string strSql = @"
                            update GsGroupShopping 
                            set HaveQuantity = HaveQuantity + @quantity 
                            where SysNo = @SysNo
                            ";
            Context.Sql(strSql)
                         .Parameter("quantity", quantity)
                         .Parameter("SysNo", sysNo)
                         .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public override GsGroupShopping Get(int sysNo)
        {

            return Context.Sql("select * from GsGroupShopping where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<GsGroupShopping>();
        }

        /// <summary>
        /// 根据商品系统编号获取团购信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>团购信息集合</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public override IList<GsGroupShopping> GetGroupShoppingByProductSysNo(int productSysNo)
        {
            string sql = @"
                        select gs.* 
                        from GsGroupShopping gs
                            inner join GsGroupShoppingItem gsi
                              on gs.sysno = gsi.GroupShoppingSysNo
                        where gsi.productsysno = @productsysno
                        ";

            return Context.Sql(sql)
                          .Parameter("productsysno", productSysNo)
                          .QueryMany<GsGroupShopping>();
        }
    }
}
