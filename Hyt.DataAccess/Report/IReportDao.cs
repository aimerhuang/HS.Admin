using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Data;

namespace Hyt.DataAccess.Report
{
    /// <summary>
    /// IReportDao
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public abstract class IReportDao : DaoBase<IReportDao>
    {
        #region 升舱明细

        /// <summary>
        /// 升舱明细查询
        /// </summary>
        /// <param name="para">CBReportDsorderDetail</param>
        /// <param name="totalAmount">输出，付款金额合计</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,升舱明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public abstract Dictionary<int, List<ReportDsorderDetail>> QueryUpgradeDetails(CBReportDsorderDetail para, ref decimal totalAmount, int currPageIndex = 1, int pageSize = 10);

        /// <summary>
        /// DsMallType查询
        /// </summary>
        /// <returns>DsMallType集合</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public abstract List<DsMallType> GetMallType();

        #endregion

        #region 销售明细

        /// <summary>
        /// 销售明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,销售明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public abstract Dictionary<int, List<RP_销售明细>> QuerySaleDetails(ref List<CBRptPaymentRecord> PaymentRecords, SalesRmaParams para,
                                                                                   List<int> whSelected,int userSysNo,
                                                                                   int currPageIndex = 1,
                                                                          int pageSize = 10);

        #endregion

        #region 退换货明细

        /// <summary>
        /// 退换货明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,退换货明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public abstract Dictionary<int, List<RP_退换货明细>> QueryRmaDetails(SalesRmaParams para,
                                                                                   List<int> whSelected, int userSysNo,
                                                                                   int currPageIndex = 1,
                                                                          int pageSize = 10);

        #endregion

        #region 市场部赠送明细

        /// <summary>
        /// 市场部赠送明细
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,市场部赠送明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public abstract Dictionary<int, List<ReportMarketDepartmentSale>> QueryMarketPresentDetails(CBReportMarketDepartmentSale para,
                                                                                   int currPageIndex = 1,
                                                                                   int pageSize = 10);

        #endregion

        #region 业务员绩效

        /// <summary>
        /// 业务员绩效查询
        /// </summary>
        /// <param name="para">RP_绩效_业务员</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,业务员绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public abstract Dictionary<int, List<RP_绩效_业务员>> QueryBusinessManPerformance(ParaBusinessManPerformance para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10);

        #endregion

        #region 办事处绩效

        /// <summary>
        /// 办事处绩效查询
        /// </summary>
        /// <param name="para">rp_绩效_办事处</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,办事处绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public abstract Dictionary<int, List<rp_绩效_办事处>> QueryOfficePerformance(rp_绩效_办事处 para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10);

        #endregion

        #region 出库单报表
        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="condition">出库单查询条件</param>
        /// <returns></returns>
        public abstract DataTable GetStockOutList(StockOutSearchCondition condition);
        #endregion

        /// <summary>
        /// 支付方式记录信息
        /// </summary>
        /// <param name="filter">支付方式记录信息</param>
        /// <returns>支付方式记录信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public abstract Pager<CBRptPaymentRecord> GetMethodPaymentRecordList(MethodPaymentRecordFilter filter);

        /// <summary>
        /// 查询导出支付方式记录信息列表
        /// </summary>
        /// <param name="filter">支付方式记录信息</param>
        /// <returns></returns>
        ///  <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public abstract List<CBRptPaymentRecord> GetExportMethodPaymentRecordList(MethodPaymentRecordFilter filter);

        /// <summary>
        /// 销售排行
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-10-22 朱家宏 创建</remarks>
        public abstract IList<RptSalesRanking> SelectSalesRanking(ParaRptSalesRankingFilter filter);

        /// <summary>
        /// 运营综述日报
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-10-29 余勇 创建</remarks>
        public abstract IList<CBReportBusinessSummary> QueryBusinessSummary(ParaRptBusinessSummaryFilter filter);

        /// <summary>
        /// 运营综述月报
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        public abstract IList<CBReportBusinessSummary> QueryBusinessSummaryMonthly(ParaRptBusinessSummaryFilter filter);

        /// <summary>
        /// 电商中心绩效分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-07-16 黄志勇 添加</remarks>
        public abstract Pager<RP_绩效_电商中心> GetListEBusinessCenter(ParaEBusinessCenterPerformanceFilter filter);

        /// <summary>
        /// 客服绩效分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public abstract Pager<RP_绩效_客服> GetListServicePerformance(ParaServicePerformanceFilter filter);

        /// <summary>
        /// 门店新增会员分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public abstract Pager<RP_绩效_门店新增会员> GetListShopNewCustomer(ParaShopNewCustomerFilter filter);

        /// <summary>
        /// 门店新增会员明细分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public abstract Pager<rp_ShopNewCustomerDetail> GetListShopNewCustomerDetail(ParaShopNewCustomerFilter filter);

        /// <summary>
        /// 查询门店新增会员明细
        /// </summary>
        /// <param name="customerSysno">客户编号</param>
        /// <returns>门店新增会员明细</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public abstract rp_ShopNewCustomerDetail SelectShopNewCustomerDetail(int customerSysno);

        /// <summary>
        /// 新增门店新增会员明细
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public abstract int InsertShopNewCustomerDetail(rp_ShopNewCustomerDetail entity);

        /// <summary>
        /// 更新门店新增会员明细消费金额
        /// </summary>
        /// <param name="customerSysno">客户编号</param>
        /// <param name="amount">消费金额</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public abstract int UpdateShopNewCustomerDetail(int customerSysno, decimal amount);

        /// <summary>
        /// 获取全部客服
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public abstract List<SyUser> GetAllService();

        /// <summary>
        /// 获取仓库内勤绩效报表
        /// </summary>
        /// <param name="pagerFilter">页面传入的过滤条件</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否具有全部仓库权限</param>
        /// <returns>仓库内勤绩效报表列表</returns>
        /// <remarks>2013-12-11 沈强 创建</remarks>
        public abstract Pager<Model.rp_仓库内勤> SearchWarehouseInsideStaff(
            Pager<Hyt.Model.Parameter.ParaWarehouseInsideStaffFilter> pagerFilter, int currentUserSysNo,
            bool hasAllWarehouse);

        /// <summary>
        /// 门店会员消费报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        public abstract Pager<CBReportShopCustomerConsume> QueryShopCustomerConsume(
            ParaRptShopCustomerConsumeFilter filter);

        /// <summary>
        /// 获取配送报表数据
        /// </summary>
        /// <param name="yyyymm">年月</param>
        /// <param name="pageindex">页面索引</param>
        /// <returns>配送报表数据</returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks>
        public abstract List<CBPickingReportItem> GetPickingReport(string yyyymm, int pageindex);

        /// <summary>
        /// 优惠卡统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-02-26 朱家宏 创建</remarks>
        public abstract IList<CBRptCouponCard> QueryCouponCards(ParaRptCouponCardFilter filter);

        /// <summary>
        /// 优惠卡统计报表2
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-02 朱家宏 创建</remarks>
        public abstract Pager<CBRptCouponCard> QueryCouponCardsNew(ParaRptCouponCardFilter filter);

        /// <summary>
        /// 仓库销售排行报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>数据</returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        public abstract List<RptSalesRanking> QueryWarehouseProductSales(ParaWarehouseProductSalesFilter filter);

        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-08 朱家宏 创建</remarks>
        public abstract Pager<CBRptSales> QuerySales(ParaRptSalesFilter filter);
        
        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-08 朱家宏 创建</remarks>
        public abstract Pager<CBRptSales> QuerySalesByDay(ParaRptSalesFilter filter);

        /// <summary>
        /// 升舱销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-16 朱家宏 创建</remarks>
        public abstract Pager<CBRptUpgradeSales> QueryUpgradeSales(ParaRptUpgradeSalesFilter filter);

        /// <summary>
        /// 获取快递100服务月统计报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public abstract List<CBMonthExpress> GetMonthExpressListByYear(int year);

        /// <summary>
        /// 获取快递100服务明细
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public abstract Pager<CBLgExpressDetail> GetLgExpressList(ParaExpressInfoFilter filter);

        /// <summary>
        /// 快递100报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        [Obsolete]
        public abstract IList<CBRptLgExpressInfo> QueryLgExpress(ParaRptLgExpressFilter filter);

        /// <summary>
        /// 快递100报表明细查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        [Obsolete]
        public abstract Pager<LgExpressInfo> QueryLgExpressDetail(ParaRptLgExpressFilter filter);

        /// <summary>
        /// 区域销售统计报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="lstResultAll">合计</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-11 余勇 创建
        /// </remarks>
        public abstract Pager<CBRptRegionalSales> QueryRegionalSales(ParaRptRegionalSales filter,
            out CBRptRegionalSales lstResultAll);

        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public abstract Pager<RP_非自营销售明细> GetFranchiseesSaleDetail(ParaFranchiseesSaleDetail filter);

        /// <summary>
        /// 加盟商退换货对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public abstract Pager<RP_非自营退换货明细> GetFranchiseesRmaDetail(ParaFranchiseesSaleDetail filter);

          /// <summary>
        /// 获取二次销售报表相关数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public abstract Pager<CBTwoSale> GetTwoSaleList(ParaTwoSaleFilter filter);

         /// <summary>
        /// 获取二次销售详情
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public abstract Pager<CBTwoSaleDetail> GetTwoSaleDetailList(ParaTwoSaleFilter filter);

        /// <summary>
        /// 办事处快递发货量统计查询
        /// </summary>
        /// <param name="para">CBRptExpressLgDelivery</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,办事处绩效集合)</returns>
        /// <remarks>2014-09-24 余勇 创建</remarks>
        public abstract Dictionary<int, List<CBRptExpressLgDelivery>> QueryExpressLgDelivery(
            CBRptExpressLgDelivery para,
            List<int> warehouseSysNos,
            int currPageIndex = 1,
            int pageSize = 10);

        /// <summary>
        /// 会员涨势信息
        /// </summary>
        /// <param name="filter">会员涨势信息</param>
        /// <returns>返回会员涨势信息</returns>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        public abstract Pager<CBRptDealerSales> GetDealerSalesList(ParaRptDealerSalesFilter filter);
        /// <summary>
        /// 查询导出会员涨势信息列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputRptDealerSales> GetExportDealerSalesList(ParaRptDealerSalesFilter filter, List<int> sysNos);

        /// <summary>
        /// 经销商总销售量排名
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>
        /// 经销商总销量排序列表
        /// </returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public abstract List<Model.Common.ReportMod> DistributorSalesOrderReport(DateTime? startTime, DateTime? endTime, string orderBy);
         /// <summary>
        /// 经销商商品销售排行
        /// </summary>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// 2016-04-09 杨云奕 添加
        /// </returns>
        public abstract List<Model.Common.ReportMod> DistributorSalesProductReport(int DsSysNo, DateTime? startTime, DateTime? endTime, string orderBy);
        /// <summary>
        /// 单品经销商的销售排名
        /// </summary>
        /// <param name="ProSysNo">商品编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public abstract List<Model.Common.ReportMod> SalesProductDistributorReport(int ProSysNo, DateTime? startTime, DateTime? endTime, string orderBy);


        /// <summary>
        /// 年度销售统计
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public abstract List<Model.Manual.AnnualMod> AnnualSalesStatistics(int year, int DsSysNo);

        /// <summary>
        /// 实体店销售统计计算
        /// </summary>
        /// <param name="defaultWareSysNo">门店名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>实体店数据列表</returns>
        public abstract List<Model.Manual.EntityStatisticMod> SearchEntityShopStatistics(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime);

        /// <summary>
        /// 客户购买量统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public abstract List<Model.Manual.EntityStatisticMod> SearchCustomerPurchasesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo);

        /// <summary>
        /// 分销商统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public abstract List<Model.Manual.EntityStatisticMod> SearchDistributorsStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo);
        /// <summary>
        /// 网上购买统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public abstract List<Model.Manual.EntityStatisticMod> SearchOnlineSalesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo);
        /// <summary>
        /// 保税商品统计表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public abstract List<Model.Manual.EntityStatisticMod> SearchBondedSalesStatistics(DateTime? startTime, DateTime? endTime);

        /// <summary>
        /// 精准营销
        /// </summary>
        /// <param name="proCateSysNos">商品分类编码</param>
        /// <param name="ShopSysNo">门店编码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>统计数据</returns>
        public abstract List<Model.Manual.EntityStatisticMod> PrecisionMarketReport(string proCateSysNos, int? ShopSysNo, DateTime? startTime, DateTime? endTime);

        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返利记录信息</returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public abstract Pager<CBRptRebatesRecord> GetRebatesRecordList(ParaRptRebatesRecordFilter filter);
        /// <summary>
        /// 查询导出返利记录信息列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public abstract List<CBOutputRptRebatesRecord> GetExportRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos);

        /// <summary>
        /// 分销商返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返利记录信息</returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public abstract Pager<CBRptRebatesRecord> GetDealerRebatesRecordList(ParaRptRebatesRecordFilter filter);
        /// <summary>
        /// 查询导出分销商返利记录
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputRptRebatesRecord> GetExportDealerRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos);
        /// <summary>
        /// 同步销售单
        /// </summary>
        /// <returns>王耀发 2016-6-4 创建</returns>
        public abstract int ProCreateSaleDetail();


        /// <summary>
        /// 同步退换货单
        /// </summary>
        /// <returns>吴琨 2017-9-27 创建</returns>
        public abstract int SynchronousRma();
       
        /// <summary>
        /// 筛选线下收银的商品销售统计
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2016-08-05 杨云奕 添加</remarks>
        public abstract IList<RptSalesRanking> GetLineOutSaleRanking(ParaRptSalesRankingFilter filter);

        public abstract List<RptSalesRanking> GetLineOutSaleRankingByWarehouse(ParaWarehouseProductSalesFilter filter);

        
    }
}
