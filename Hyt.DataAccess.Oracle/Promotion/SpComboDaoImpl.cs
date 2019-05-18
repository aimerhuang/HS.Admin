using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

using System;
using System.Linq;
using System.Text;


namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 组合套餐
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpComboDaoImpl : ISpComboDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpCombo entity)
        {
            entity.SysNo = Context.Insert("SpCombo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"数据表实体</param>
        /// <returns>数据表实体（带编号）</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public override SpCombo InsertEntity(SpCombo entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SpCombo", entity)
                                             .AutoMap(o => o.SysNo)
                                            .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Update(SpCombo entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            Context.Update("SpCombo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 根据组合套餐系统编号更新已销售数
        /// </summary>
        /// <param name="sysNo">组合套餐系统编号</param>
        /// <param name="quantity">使用套餐数量</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public override void UpdateSaleQuantity(int sysNo, int quantity)
        {
            const string strSql = @"
                            update SpCombo 
                            set SaleQuantity = SaleQuantity + @quantity 
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
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpCombo GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpCombo where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpCombo>();
        }

        /// <summary>
        /// 获取促销对应的组合套餐列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销对应的组合套餐列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<SpCombo> GetListByPromotionSysNo(int promotionSysNo)
        {
            return Context.Sql("select * from SpCombo where PromotionSysNo=@PromotionSysNo")
                  .Parameter("PromotionSysNo", promotionSysNo)
                  .QueryMany<SpCombo>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpCombo where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 根据组主商品系统编号获取组合套餐信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>组合套餐集合</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public override IList<SpCombo> GetComboByMasterProductSysNo(int productSysNo)
        {
            string sql = @"
                        select c.* 
                        from SpCombo c
                            inner join SpComboItem ci
                              on c.sysno = ci.combosysno
                        where ci.ismaster = @ismaster
                          and ci.productsysno = @productsysno
                        ";

            return Context.Sql(sql)
                          .Parameter("ismaster", (int)PromotionStatus.是否是套餐主商品.是)
                          .Parameter("productsysno", productSysNo)
                          .QueryMany<SpCombo>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public override Pager<CBSpCombo> Query(ParaSpComboFilter filter)
        {
            const string sql = @"(select a.*,b.Name from spcombo a left join sppromotion b on a.promotionsysno = b.sysno
                                where 
                                (@0 is null or charindex(a.title,@0)>0) and         
                                (@1 is null or exists (select 1 from splitstr(@1,',') tmp where tmp.col = a.status)) and            ----
                                (@2 is null or a.StartTime>=@2) and                                                                               --日期(起)
                                (@3 is null or a.EndTime<=@3)                                                                                        --日期(止) 
                                ) tb";

            var statuses = (filter.Statuses != null && filter.Statuses.Count > 0) ? string.Join(",", filter.Statuses) : null;

            var paras = new object[]
                {
                    filter.Title,
                    statuses,
                    filter.BeginDate,
                    filter.EndDate
                };

            var dataList = Context.Select<CBSpCombo>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBSpCombo>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }
        #endregion
    }
}
