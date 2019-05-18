using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.DataAccess.RMA;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销
    /// </summary>
    /// <remarks>2013-08-13 吴文强 创建</remarks>
    public class SpPromotionDaoImpl : ISpPromotionDao
    {
        #region 吴文强

        #region 促销

        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <returns>有效促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotion> GetValidPromotions(PromotionStatus.促销使用平台[] platformType)
        {
            if (platformType == null)
            {
                platformType = new PromotionStatus.促销使用平台[] { };
            }

            const string strSql = @"
                        select * 
                        from SpPromotion 
                        where SYSDATETIME() between StartTime and EndTime 
                          and IsUsePromotionCode = 0 
                          and Status = 20
                          and (WebPlatform = (@WebPlatform)
                              or ShopPlatform = (@ShopPlatform)
                              or MallAppPlatform = (@MallAppPlatform)
                              or LogisticsAppPlatform = (@LogisticsAppPlatform))
                        ";

            var entity = Context.Sql(strSql)
                                .Parameter("WebPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.PC商城) ? (int)PromotionStatus.商城使用.是 : -1)
                                .Parameter("ShopPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.门店) ? (int)PromotionStatus.门店使用.是 : -1)
                                .Parameter("MallAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.手机商城) ? (int)PromotionStatus.手机商城使用.是 : -1)
                                .Parameter("LogisticsAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.物流App) ? (int)PromotionStatus.物流App使用.是 : -1)
                                .QueryMany<SpPromotion>();

            return entity;
        }

        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <param name="promotionCode">促销系统编号</param>
        /// <returns>有效促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotion> GetValidPromotions(string[] promotionCode)
        {
            if (promotionCode == null || !promotionCode.Any())
                return new List<SpPromotion>();

            const string strSql = @"
                        select * from SpPromotion 
                        where (SYSDATETIME() between StartTime and EndTime)
                          and (PromotionCode in(@PromotionCode))
                          and IsUsePromotionCode = @IsUsePromotionCode
                          and Status = 20";

            var entity = Context.Sql(strSql)
                                .Parameter("PromotionCode", promotionCode)
                                .Parameter("IsUsePromotionCode", (int)PromotionStatus.是否使用促销码.是)
                                .QueryMany<SpPromotion>();

            return entity;
        }

        /// <summary>
        /// 获取指定促销编号的促销
        /// </summary>
        /// <param name="sysNo">促销编号</param>
        /// <returns>促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotion> GetPromotions(int[] sysNo = null)
        {
            const string strSql = @"
                        select * from SpPromotion 
                        where (0 = @nosysno or sysno in(@sysNo))";

            var entity = Context.Sql(strSql)
                                .Parameter("nosysno", sysNo == null || sysNo.Count() == 0 ? 0 : sysNo.Length)
                                .Parameter("sysNo", sysNo)
                                .QueryMany<SpPromotion>();

            return entity;
        }

        #endregion

        #region 促销规则

        /// <summary>
        /// 获取促销规则关联集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则关联</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotionRuleCondition> GetPromotionRuleConditions(int[] promotionSysNo)
        {
            const string strSql = @"
                        select * from SpPromotionRuleCondition 
                        where PromotionSysNo in(@promotionSysNo)";

            var entity = Context.Sql(strSql)
                                .Parameter("promotionSysNo", promotionSysNo)
                                .QueryMany<SpPromotionRuleCondition>();

            return entity;
        }

        /// <summary>
        /// 获取促销规则集合
        /// </summary>
        /// <param name="sysNo">规则系统</param>
        /// <returns>促销规则集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotionRule> GetPromotionRules(int[] sysNo)
        {
            const string strSql = @"
                        select * from SpPromotionRule 
                        where SysNo in(@SysNo)";

            var entity = Context.Sql(strSql)
                                .Parameter("SysNo", sysNo)
                                .QueryMany<SpPromotionRule>();

            return entity;
        }

        #endregion

        #region 促销叠加

        /// <summary>
        /// 获取促销叠加集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销叠加集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotionOverlay> GetPromotionOverlays(int[] promotionSysNo)
        {
            const string strSql = @"
                        select * from SpPromotionOverlay 
                        where OverlayCode in (select distinct OverlayCode from SpPromotionOverlay 
                                                where PromotionSysNo in(@promotionSysNo))";

            var entity = Context.Sql(strSql)
                                .Parameter("promotionSysNo", promotionSysNo)
                                .QueryMany<SpPromotionOverlay>();
            return entity;
        }
        #endregion

        #region 促销赠品

        /// <summary>
        /// 获取促销赠品集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销赠品集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<CBSpPromotionGift> GetPromotionGifts(int[] promotionSysNo)
        {
            const string strSql = @"
                        select * from SpPromotionGift 
                        where PromotionSysNo in(@promotionSysNo)";

            var entity = Context.Sql(strSql)
                                .Parameter("promotionSysNo", promotionSysNo)
                                .QueryMany<CBSpPromotionGift>();
            return entity;
        }
        #endregion

        #region 促销规则值

        /// <summary>
        /// 获取促销规则值
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则值</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override List<SpPromotionRuleKeyValue> GetPromotionRuleKeyValues(int[] promotionSysNo)
        {
            const string strSql = @"
                        select * from SpPromotionRuleKeyValue 
                        where PromotionSysNo in(@promotionSysNo)";

            var entity = Context.Sql(strSql)
                                .Parameter("promotionSysNo", promotionSysNo)
                                .QueryMany<SpPromotionRuleKeyValue>();
            return entity;
        }
        #endregion

        #endregion

        #region 促销管理

        /// <summary>
        /// 分页获取促销
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>
        /// 2013-08-21 黄志勇 创建
        /// 2013-12-31 朱家宏 添加过期时间
        /// 2014-01-09 朱家宏 添加使用平台查询
        /// </remarks>
        public override Pager<SpPromotion> GetPromotion(ParaPromotion filter)
        {
            const string sql =
                @"(select a.*
                  from SpPromotion a 
                    where 
                    (@0 is null or charindex(a.Name,@0)>0) and   --促销名称
                    (@1 is null or a.PromotionType=@1) and                                                                                  
                           --优惠券类型                   
                    (@2 is null or a.Status=@2) and
                           --状态  
                    (@3 is null or a.StartTime>=@3) and                                                                                    
                           --开始时间
                    (@4 is null or a.EndTime<@4) and                                                                                         
                           --结束时间 
                    (@5 is null or a.IsUsePromotionCode=@5) and
                           --是否使用促销代码 
                    (@6 = 0 or not exists(select 1 from SpPromotionOverlay where PromotionSysNo=a.SysNo)) and 
                           --是否为促销叠加 
                    (@7 is null or a.EndTime>=@7) and                                                                                          
                           --过期时间
                    (@8 is null or a.WebPlatform=@8) and 
                    (@9 is null or a.ShopPlatform=@9) and 
                    (@10 is null or a.MallAppPlatform=@10) and 
                    (@11 is null or a.LogisticsAppPlatform=@11)
                ) tb";

            var dataList = Context.Select<SpPromotion>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            //查询日期上限+1
            filter.EndTime = filter.EndTime == null ? (DateTime?)null : filter.EndTime.Value.AddDays(1);
            var paras = new object[]
                {
                    filter.Name,
                    filter.PromotionType,
                    filter.Status,
                    filter.StartTime,
                    filter.EndTime,
                    filter.IsUsePromotionCode,
                    filter.IsOverlay,
                    filter.ExpiredTime,
                    filter.WebPlatform,
                    filter.ShopPlatform,
                    filter.MallAppPlatform,
                    filter.LogisticsAppPlatform
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<SpPromotion>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.CreatedDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }

        /// <summary>
        /// 分页获取促销(有分销商)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public override Pager<ParaDealerPromotion> GetDealerPromotion(ParaPromotionpager filter)
        {
            const string sql = @"SpPromotion a left join SpPromotionDealer pd on a.sysno=pd.PromotionSysNo left join DsDealer dea on pd.DealerSysNo = dea.SysNo";
            string where = @"(@0 is null or charindex(a.Name,@0)>0) and   --促销名称
                            (@1 is null or a.PromotionType=@1) and                                                                                  
                                   --优惠券类型                   
                            (@2 is null or a.Status=@2) and
                                   --状态  
                            (@3 is null or a.StartTime>=@3) and                                                                                    
                                   --开始时间
                            (@4 is null or a.EndTime<@4) and                                                                                         
                                   --结束时间 
                            (@5 is null or a.IsUsePromotionCode=@5) and
                                   --是否使用促销代码 
                            (@6 = 0 or not exists(select 1 from SpPromotionOverlay where PromotionSysNo=a.SysNo)) and 
                                   --是否为促销叠加 
                            (@7 is null or a.EndTime>=@7) and                                                                                          
                                   --过期时间
                            (@8 is null or a.WebPlatform=@8) and 
                            (@9 is null or a.ShopPlatform=@9) and 
                            (@10 is null or a.MallAppPlatform=@10) and 
                            (@11 is null or a.LogisticsAppPlatform=@11)";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and dea.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }
            var dataList = Context.Select<ParaDealerPromotion>("pd.DealerSysNo,a.*").From(sql).Where(where);
            var dataCount = Context.Select<int>("count(0)").From(sql).Where(where);
            //查询日期上限+1
            filter.EndTime = filter.EndTime == null ? (DateTime?)null : filter.EndTime.Value.AddDays(1);
            var paras = new object[]
                {
                    filter.Name,
                    filter.PromotionType,
                    filter.Status,
                    filter.StartTime,
                    filter.EndTime,
                    filter.IsUsePromotionCode,
                    filter.IsOverlay,
                    filter.ExpiredTime,
                    filter.WebPlatform,
                    filter.ShopPlatform,
                    filter.MallAppPlatform,
                    filter.LogisticsAppPlatform
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<ParaDealerPromotion>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.CreatedDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  黄志勇 创建</remarks>
        public override int Insert(SpPromotion entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }

            entity.SysNo = Context.Insert("SpPromotion", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-08-21  黄志勇 创建</remarks>
        public override int Update(SpPromotion entity)
        {

            return Context.Update("SpPromotion", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 根据促销系统编号更新促销已使用次数
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public override void UpdateUsedQuantity(int sysNo)
        {
            const string strSql = @"
                            update SpPromotion 
                            set PromotionUsedQuantity = PromotionUsedQuantity + 1 
                            where SysNo = @SysNo
                            ";
            Context.Sql(strSql)
                         .Parameter("SysNo", sysNo)
                         .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-26  余勇 创建</remarks>
        public override SpPromotion Get(int sysNo)
        {

            return Context.Sql("select * from SpPromotion where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotion>();
        }

        /// <summary>
        /// 通过促销码来获取数据
        /// </summary>
        /// <param name="promotionCode">促销码</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-12-9  朱家宏 创建</remarks>
        public override SpPromotion GetByPromotionCode(string promotionCode)
        {
            SpPromotion model = null;
            if (!string.IsNullOrWhiteSpace(promotionCode))
            {
                model = Context.Sql("select * from SpPromotion where promotionCode=@PromotionCode")
                               .Parameter("PromotionCode", promotionCode)
                               .QuerySingle<SpPromotion>();
            }
            return model;
        }

        #endregion

        /// <summary>
        /// 查询团购和组合以及促销规则中包含的所有商品
        /// </summary>
        /// <returns>促销商品</returns>
        /// <remarks>2013-08-21 余勇 创建</remarks>
        public override List<int> GetAllPromotionProduct()
        {

            const string strSql = @"select distinct productsysno  from SpComboItem 
                                        where ismaster = 1
                                   union
                                   select distinct productsysno from  GsGroupShoppingItem";
            const string strSql2 = @"select rulevalue from SpPromotionRuleKeyValue
                             where rulekey='product_sysno' and  promotionsysno in 
                                 (select sysno from SpPromotion where SYSDATETIME() between StartTime and EndTime 
                                                                and IsUsePromotionCode = 0 
                                                                and Status = 20  
                                                                and PromotionType in(10,20,110,200))";
            //查询团购和组合商品
            var res = Context.Sql(strSql)
                              .QueryMany<int>() ?? new List<int>();
            //查询促销规则商品
            var ruleValueList = Context.Sql(strSql2)
                                .QueryMany<string>() ?? new List<string>();
            //将保存的商品字符串数组转为整型
            foreach (var ruleValue in ruleValueList)
            {
                if (string.IsNullOrEmpty(ruleValue)) continue;
                var products = ruleValue.Split(';');
                foreach (var strProduct in products)
                {
                    if (string.IsNullOrEmpty(strProduct)) continue;
                    var product = Convert.ToInt32(strProduct);
                    if (!res.Contains(product))
                    {
                        res.Add(product);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 获得已审核，在有效时间内的促销
        /// </summary>
        /// <returns></returns>
        /// 王耀发 2016-1-15 创建
        public override List<SpPromotion> GetSpPromotionList(int PromotionType)
        {
            return Context.Sql(@"select a.SysNo,a.Name from SpPromotion a 
                              where a.PromotionType = @PromotionType and a.Status = 20 and GETDATE() between a.StartTime and a.EndTime")
                         .Parameter("PromotionType", PromotionType)
                         .QueryMany<SpPromotion>();
        }

        /// <summary>
        /// 通过促销编号获取详情
        /// </summary>
        /// <param name="PromotionSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public override SpPromotionDealer GetByPromotionSysNo(int PromotionSysNo)
        {
            return Context.Sql("select * from SpPromotionDealer where PromotionSysNo=@PromotionSysNo")
                          .Parameter("PromotionSysNo", PromotionSysNo)
                          .QuerySingle<SpPromotionDealer>();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int InsertPromotionDealer(SpPromotionDealer entity)
        {
            var sysNo = Context.Insert("SpPromotionDealer", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>2016-08-29 周 创建</remarks>
        public override void UpdatePromotionDealer(SpPromotionDealer entity)
        {
            var sysNo = Context.Update("SpPromotionDealer", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        public override List<SpPromotion> GetSpPromotionAllList(int PromotionType)
        {
            return Context.Sql(@"select a.SysNo,a.Name from SpPromotion a 
                              where a.PromotionType = @PromotionType ")
                         .Parameter("PromotionType", PromotionType)
                         .QueryMany<SpPromotion>();
        }
    }
}
