using System;
using System.Collections;
using System.Linq;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 优惠券
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpCouponDaoImpl : ISpCouponDao
    {

        #region 数据记录增，删，改，查

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpCoupon entity)
        {
            if (entity.AuditDate == DateTime.MinValue)
            {
                entity.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SpCoupon", entity)
                                  .AutoMap(o => o.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响的行</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Update(SpCoupon entity)
        {
            int SysNo = entity.SysNo;
            SpCoupon Entity = GetEntity(SysNo);

            entity.AuditDate = Entity.AuditDate;
            entity.CreatedDate = Entity.CreatedDate;

            return Context.Update("SpCoupon", entity)
                          .AutoMap(o => o.SysNo)
                          .Where("SysNo", entity.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 根据优惠券代码更新优惠券已使用数量
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public override void UpdateUsedQuantity(string couponCode)
        {
            const string strSql = @"
                            update SpCoupon 
                            set UsedQuantity = UsedQuantity + 1 ,
                                Status = CASE UseQuantity WHEN UsedQuantity + 1 THEN @Status ELSE Status END
                            where CouponCode = @CouponCode
                            ";
            Context.Sql(strSql)
                         .Parameter("Status", PromotionStatus.优惠券状态.已使用)
                         .Parameter("CouponCode", couponCode)
                         .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpCoupon GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpCoupon where SysNo=@SysNo")
                          .Parameter("SysNo", sysNo)
                          .QuerySingle<SpCoupon>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpCoupon where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        /// 分页获取优惠券
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>
        /// 2013-08-21 黄志勇 创建
        /// 2013-12-06 朱家宏 增加 过期时间、优惠卷代码、允许使用数量查询
        /// 2014-01-07 朱家宏 重构
        /// </remarks>
        public override Pager<CBSpCoupon> GetCoupon(ParaCoupon filter)
        {
            var sql =
                @"SpCoupon a 
                  left join CrCustomer b on a.CustomerSysNo = b.SysNo
                  left join SpPromotion c on a.PromotionSysNo = c.SysNo
                  where {0} ";

            #region 构造sql

            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                //客户名
                where += " and charindex(b.Name,@p0p" + i + ")>0";
                paras.Add(filter.CustomerName);
                i++;
            }
            if (filter.Type != null)
            {
                //优惠券类型
                where += " and a.Type= @p0p" + i + "";
                paras.Add(filter.Type);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.SourceDescription))
            {
                //来源描述
                where += " and charindex(a.SourceDescription,@p0p" + i + ">0";
                paras.Add(filter.SourceDescription);
                i++;
            }
            if (filter.Status != null)
            {
                //状态
                where += " and a.Status=@p0p" + i + "";
                paras.Add(filter.Status);
                i++;
            }
            if (filter.StartTime != null)
            {
                //有效时间(起)
                where += " and a.StartTime>=@p0p" + i + "";
                paras.Add(filter.StartTime);
                i++;
            }
            if (filter.EndTime != null)
            {
                //有效时间(止) 
                where += " and a.EndTime<@p0p" + i + "";
                paras.Add(filter.EndTime);
                i++;
            }
            if (filter.ExpiredTime != null)
            {
                //过期时间
                where += " and a.EndTime>=@p0p" + i + "";
                paras.Add(filter.ExpiredTime);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CouponCode))
            {
                //优惠卷代码
                where += " and a.CouponCode=@p0p" + i + "";
                paras.Add(filter.CouponCode);
                i++;
            }
            if (filter.UseQuantity != null)
            {
                //允许使用数量
                where += " and a.UseQuantity>=@p0p" + i + " and a.UsedQuantity < a.UseQuantity";
                paras.Add(filter.UseQuantity);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                //描述
                where += " and charindex(a.Description,@p0p" + i + ")>0";
                paras.Add(filter.Description);
                i++;
            }
            if (filter.IsCouponCard != null)
            {
                //优惠卡
                where += " and a.IsCouponCard=@p0p" + i + "";
                paras.Add(filter.IsCouponCard);
                i++;
            }

            if (filter.WebPlatform != null)
            {
                where += " and a.WebPlatform=@p0p" + i + "";
                paras.Add(filter.WebPlatform);
                i++;
            }
            if (filter.ShopPlatform != null)
            {
                where += " and a.ShopPlatform=@p0p" + i + "";
                paras.Add(filter.ShopPlatform);
                i++;
            }
            if (filter.MallAppPlatform != null)
            {
                where += " and a.MallAppPlatform=@p0p" + i + "";
                paras.Add(filter.MallAppPlatform);
                i++;
            }
            if (filter.LogisticsAppPlatform != null)
            {
                where += " and a.LogisticsAppPlatform=@p0p" + i + "";
                paras.Add(filter.LogisticsAppPlatform);
                i++;
            }

            if (filter.Permissions != null && filter.Permissions.Contains(true))
            {
                var tmpWhere = "";
                if (filter.Permissions[0])
                {
                    tmpWhere += "a.WebPlatform=" + (int)PromotionStatus.商城使用.是 + " or";
                }
                if (filter.Permissions[1])
                {
                    tmpWhere += " a.ShopPlatform=" + (int)PromotionStatus.门店使用.是 + " or";
                }
                if (filter.Permissions[2])
                {
                    tmpWhere += " a.MallAppPlatform=" + (int)PromotionStatus.手机商城使用.是 + " or";
                }
                if (filter.Permissions[3])
                {
                    tmpWhere += " a.LogisticsAppPlatform=" + (int)PromotionStatus.物流App使用.是 + " or";
                }
                tmpWhere = tmpWhere.Substring(0, tmpWhere.Length - 2);
                where += " and (" + tmpWhere + ")";
            }

            sql = string.Format(sql, where);

            #endregion

            var dataList =
                Context.Select<CBSpCoupon>(
                    @"a.sysno, a.promotionsysno, a.couponcode, a.couponamount, a.requirementamount, 
                                            a.starttime, a.endtime, a.type, a.sourcedescription, a.usequantity, a.usedquantity, 
                                            a.customersysno, a.status, a.description, a.auditorsysno, a.auditdate, a.createdby, 
                                            a.createddate, a.lastupdateby, a.lastupdatedate, a.parentsysno, a.iscouponcard, 
                                            a.webplatform, a.shopplatform, a.mallappplatform,a.LogisticsAppPlatform,
                                            b.Name CustomerName,c.Name PromotionName").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<CBSpCoupon>
                {
                    CurrentPage = filter.Id.Value,
                    PageSize = filter.PageSize
                };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>优惠券</returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        public override SpCoupon GetCoupon(string couponCode)
        {
            return Context.Sql("select * from SpCoupon where CouponCode=@couponCode and Status != -10")
                          .Parameter("couponCode", couponCode)
                          .QuerySingle<SpCoupon>();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>优惠券扩展</returns>
        /// <remarks>2013-08-27  黄志勇 创建</remarks>
        public override CBSpCoupon GetCoupon(int sysNo)
        {
            return Context.Sql("select a.*,b.Name CustomerName,c.Name PromotionName from SpCoupon a " +
                               "left join CrCustomer b on a.CustomerSysNo=b.SysNo " +
                               "left join SpPromotion c on a.PromotionSysNo = c.SysNo " +
                               "where a.SysNo=@SysNo")
                          .Parameter("SysNo", sysNo)
                          .QuerySingle<CBSpCoupon>();
        }

        #endregion

        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(0:所有)</param>
        /// <param name="platformType">使用平台</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public override IList<SpCoupon> GetCustomerCoupons(int customerSysNo, PromotionStatus.优惠券状态 status, PromotionStatus.促销使用平台[] platformType)
        {
            if (platformType == null)
            {
                platformType = new PromotionStatus.促销使用平台[] { };
            }

            const string strSql = @"
                        select * from SpCoupon 
                        where CustomerSysNo = @CustomerSysNo
                          and (WebPlatform = (@WebPlatform)
                              or ShopPlatform = (@ShopPlatform)
                              or MallAppPlatform = (@MallAppPlatform)
                              or LogisticsAppPlatform = (@LogisticsAppPlatform)
                                )
                          and (0 = @Status or Status = @Status)";

            var entity = Context.Sql(strSql)
                                .Parameter("CustomerSysNo", customerSysNo)
                                .Parameter("WebPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.PC商城) ? (int)PromotionStatus.商城使用.是 : -1)
                                .Parameter("ShopPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.门店) ? (int)PromotionStatus.门店使用.是 : -1)
                                .Parameter("MallAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.手机商城) ? (int)PromotionStatus.手机商城使用.是 : -1)
                                .Parameter("LogisticsAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.物流App) ? (int)PromotionStatus.物流App使用.是 : -1)
                                .Parameter("Status", status)
                //.Parameter("Status", status)
                                .QueryMany<SpCoupon>();
            return entity;
        }

        /// <summary>
        /// 获取优惠卷信息分页方法
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="status">优惠券状态</param>
        /// <param name="nowTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="type">优惠券状态</param>
        /// <param name="count">总数</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-09-16 杨晗 创建</remarks>
        public override IList<SpCoupon> Seach(int pageIndex, int pageSize, int customerSysNo, PromotionStatus.优惠券状态? status,
                                     DateTime? nowTime, DateTime? endTime, int type, out int count)
        {
            IList<SpCoupon> entity;
            string sqlWhere = @"
                         CustomerSysNo = @CustomerSysNo
                          and (0 = @Status or Status = @Status)
          and(@NowTime is null or (StartTime<=@NowTime and EndTime>=@NowTime))
and (@EndTime is null or EndTime <= @EndTime)
";
            #region 他们后台业务优惠券处理操作的时候 没有及时运算优惠券使用状态 并冗余到状态字段中 由于系统已发布后台业务不好修改 所以在这里的查询加了计算状态的sql 以后效率出现问题就修改这里 2013-12-1 by 杨晗
            if (type == 1)
            {
                sqlWhere += "and UsedQuantity < UseQuantity ";
            }
            else if (type == 2)
            {
                sqlWhere += "and UsedQuantity = UseQuantity ";
            }
            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                count = context.Select<int>("count(1)")
                                .From("SpCoupon sn")
                                .Where(sqlWhere)
                                .Parameter("CustomerSysNo", customerSysNo)
                                .Parameter("Status", (int)status)//PromotionStatus.优惠券状态.已审核
                    //.Parameter("Status", (int)status)//PromotionStatus.优惠券状态.已审核
                                .Parameter("NowTime", nowTime)
                    //.Parameter("NowTime", nowTime)
                                .Parameter("NowTime", nowTime)
                    //.Parameter("EndTime", endTime)
                                .Parameter("EndTime", endTime)
                                .QuerySingle();
                entity = context.Select<SpCoupon>("sn.*")
                                 .From("SpCoupon sn")
                                 .Where(sqlWhere)
                                 .Parameter("CustomerSysNo", customerSysNo)
                                 .Parameter("Status", (int)status)
                    //.Parameter("Status", (int)status)
                    //.Parameter("NowTime", nowTime)
                                 .Parameter("NowTime", nowTime)
                    //.Parameter("NowTime", nowTime)
                    // .Parameter("EndTime", endTime)
                                 .Parameter("EndTime", endTime)
                                 .Paging(pageIndex, pageSize)
                                 .OrderBy("LastUpdateDate desc")
                                 .QueryMany();
            }
            return entity;
        }

        /// <summary>
        /// 根据优惠券代码获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>优惠券信息</returns>
        /// <remarks>2014-03-28 唐永勤 创建</remarks>
        public override SpCoupon GetSpCouponByCouponCode(string couponCode)
        {
            SpCoupon entity = Context.Select<SpCoupon>("*")
                .From("SpCoupon")
                .Where("CouponCode = @CouponCode")
                .Parameter("CouponCode", couponCode)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息(已经优惠卡号)
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(Null:所有)</param>
        /// <param name="platformType">使用平台</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2014-06-18 朱成果 创建</remarks>
        public override IList<CBSpCoupon> GetCustomerCouponsWithCard(int customerSysNo, PromotionStatus.优惠券状态 status, PromotionStatus.促销使用平台[] platformType)
        {
            if (platformType == null)
            {
                platformType = new PromotionStatus.促销使用平台[] { };
            }

            const string strSql = @"
                        select a.*,b.CouponCardNo from SpCoupon a
                        left join SpCouponReceiveLog b on  a.SysNo=b.CouponSysNo
                        where CustomerSysNo = @CustomerSysNo
                          and (WebPlatform = (@WebPlatform)
                              or ShopPlatform = (@ShopPlatform)
                              or MallAppPlatform = (@MallAppPlatform)
                              or LogisticsAppPlatform = (@LogisticsAppPlatform)
                                )
                          and (0 = @Status or Status = @Status)";

            var entity = Context.Sql(strSql)
                                .Parameter("CustomerSysNo", customerSysNo)
                                .Parameter("WebPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.PC商城) ? (int)PromotionStatus.商城使用.是 : -1)
                                .Parameter("ShopPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.门店) ? (int)PromotionStatus.门店使用.是 : -1)
                                .Parameter("MallAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.手机商城) ? (int)PromotionStatus.手机商城使用.是 : -1)
                                .Parameter("LogisticsAppPlatform", platformType.Any(p => p == PromotionStatus.促销使用平台.物流App) ? (int)PromotionStatus.物流App使用.是 : -1)
                //.Parameter("Status", status)
                                .Parameter("Status", status)
                                .QueryMany<CBSpCoupon>();
            return entity;
        }
    }
}
