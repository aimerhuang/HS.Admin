
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.Parameter;
using System;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销规则
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpPromotionRuleDaoImpl : ISpPromotionRuleDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        /// <remarks>2015-08-3  王耀发 修改</remarks>
        public override int Insert(SpPromotionRule entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SpPromotionRule", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Update(SpPromotionRule entity)
        {

            return Context.Update("SpPromotionRule", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpPromotionRule GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpPromotionRule where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotionRule>();
        }

        /// <summary>
        /// 获取促销规则
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<SpPromotionRule> GetListByPromotionSysNo(int promotionSysNo)
        {
            return Context.Sql(@"
                                select t1.*
                                from  SpPromotionRule t1 
                                inner join SpPromotionRuleCondition t2
                                on t1.sysno=t2.promotionrulesysno
                                where t2.promotionsysno=@promotionsysno")
                .Parameter("promotionsysno", promotionSysNo)
               .QueryMany<SpPromotionRule>();
        }

        /// <summary>
        /// 分页获取促销规则
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public override Pager<SpPromotionRule> GetPromotionRule(ParaPromotionRule filter)
        {
            const string sql =
                @"(select a.*
                  from SpPromotionRule a 
where 
                    (@0 is null or charindex(a.Name,@0)>0) and                                                                                                     --规则名称
                    (@1 is null or a.PromotionType=@1) and 
                                              --促销类型
                    (@2 is null or a.RuleType=@2) and 
                                              --促销类型
                    (@3 is null or charindex(a.Description,@3)>0) and 
                                              --规则描述   
                    (@4 is null or a.Status=@4)
                                              --状态  
                ) tb";

            var dataList = Context.Select<SpPromotionRule>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            var paras = new object[]
                {
                    filter.Name,
                    filter.PromotionType,
                    filter.RuleType,
                    filter.Description,
                    filter.Status
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<SpPromotionRule>
                {
                    CurrentPage = filter.Id,
                    PageSize = filter.PageSize
                };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpPromotionRule where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        /// 判断规则名称是否存在
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="excludesysNo">排除的规则编号</param>
        /// <returns>true/false</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public override bool ExistsRule(string ruleName, int excludesysNo)
        {
            return Context.Sql("select count(1) from SpPromotionRule where Name=@Name and SysNo<>@SysNo")
                  .Parameter("Name", ruleName)
                  .Parameter("SysNo", excludesysNo)
                  .QuerySingle<int>() > 0;
        }

    }
}
