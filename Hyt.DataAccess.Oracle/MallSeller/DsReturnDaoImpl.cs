using Hyt.DataAccess.MallSeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// 分销商退换货
    /// </summary>
    /// <remarks>2013-09-10 朱家宏 创建</remarks>
    public class DsReturnDaoImpl : IDsReturnDao
    {
        /// <summary>
        /// 根据hyt退换货单号获取实体
        /// </summary>
        /// <param name="value">hyt退换货单号</param>
        /// <returns>升舱退换货实体</returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public override DsReturn SelectByRmaSysNo(int value)
        {
            return Context.Sql("select * from DsReturn where RcReturnSysNo=@0", value).QuerySingle<DsReturn>();
        }

        /// <summary>
        /// 获取分销商退换货明细
        /// </summary>
        /// <param name="dsReturnSysNo">分销商退换货单编号</param>
        /// <returns>升舱退换货明细列表</returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public override IList<DsReturnItem> SelectItems(int dsReturnSysNo)
        {
            var items =
                Context.Select<DsReturnItem>("*")
                       .From("DsReturnItem")
                       .Where("dsReturnSysNo=@dsReturnSysNo")
                       .Parameter("dsReturnSysNo", dsReturnSysNo)
                       .QueryMany();
            return items;
        }

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns>分销商退换货单</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public override List<CBDsReturn> GetReturn(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            string sql = string.Format(@"select * from (select a.*,c.Status from DsReturn a
inner join DsDealerMall b
on a.DealerMallSysNo = b.SysNo
left join RcReturn c
on a.RcReturnSysNo = c.SysNo
where b.ShopAccount = @shopAccount and b.MallTypeSysNo = @mallTypeSysNo {0}
order by a.ApplicationTime desc) where rownum <= @top", !isFinish.HasValue ? "" : (isFinish.Value ? "and c.Status = 50" : "and c.Status != 50"));
            return Context.Sql(sql)
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .Parameter("top", top)
                          .QueryMany<CBDsReturn>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-10 余勇 创建</remarks>
        public override Pager<CBDsReturn> Query(ParaDsReturnFilter filter)
        {
            const string sql = @"(select a.*,b.Status from DsReturn a  left join RcReturn b on a.ReturnTransactionSysNo = b.TransactionSysNo
                                 left join soOrder c on b.OrderSysNo = c.SysNo
                                where a.DealerMallSysNo=@0 and 
                                (@1 is null or charindex(a.buyernick,@1)>0) and 
                                (@2 is null or a.MallOrderID=@2) and 
                                (@3 is null or a.ApplicationTime>=@3) and                                                                                   --日期(起)
                                (@4 is null or a.ApplicationTime<@4) and                                                                                        --日期(止) 
                                (@5 is null or exists (select 1 from DsReturnItem tmp where tmp.DsReturnSysNo=a.sysNo and charindex(tmp.MallProductName,@5)>0)) and       --商品名称
                                (@6 is null or exists (select 1 from DsReturnItem tmp where tmp.DsReturnSysNo=a.sysNo and tmp.MallProductId=@6))                          --商品编号
                                ) tb";

            var paras = new object[]
                {
                    filter.DealerMallSysNo,
                    filter.BuyerNick,
                    filter.MallOrderId,
                    filter.BeginDate,
                    filter.EndDate,
                    filter.MallProductName,
                    filter.MallProductId
                };

            var dataList = Context.Select<CBDsReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsReturn>
                {
                    PageSize = filter.PageSize,
                    CurrentPage = filter.PageIndex,
                    TotalRows = dataCount.QuerySingle(),
                    Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.PageIndex, filter.PageSize).QueryMany()
                };

            return pager;
        }

        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-09-12 朱家宏 创建</remarks>
        public override int Insert(DsReturn model)
        {
            var sysNo = Context.Insert("DsReturn", model)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-25  朱成果 创建</remarks>
        public override void Update(DsReturn entity)
        {

            Context.Update("DsReturn", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-09-12 朱家宏 创建</remarks>
        public override int InsertItem(DsReturnItem model)
        {
            var sysNo = Context.Insert("DsReturnItem", model)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }
    }
}
