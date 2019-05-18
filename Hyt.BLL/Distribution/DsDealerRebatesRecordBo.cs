using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Distribution;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Log;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class DsDealerRebatesRecordBo : BOBase<DsDealerRebatesRecordBo>
    {

        #region 返利记录
        /// <summary>
        /// 分页获取返利记录
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordList(ParaDealerRebatesRecordFilter filter)
        {
            return IDsDealerRebatesRecordDao.Instance.GetDsDealerRebatesRecordList(filter);
        }
        /// <summary>
        /// 分销汇总明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Pager<CBCCustomerRebatesRecord> GetDealerInfoSummaryList(ParaCBCCustomerRebatesRecordFilter filter)
        {
            return IDsDealerRebatesRecordDao.Instance.GetDealerInfoSummaryList(filter);
        }

        /// <summary>
        /// 删除返利记录
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IDsDealerRebatesRecordDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返回返利记录信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public IList<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordView(ParaDealerRebatesRecordFilter filter)
        {
            return IDsDealerRebatesRecordDao.Instance.GetDsDealerRebatesRecordView(filter);
        }
        /// <summary>
        /// 会员推荐分销订单查询
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="changeType"></param>
        /// <param name="pageIndex"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public PagedList<CBDsDealerRebatesRecord> GeDirectOrdersList(int customerSysNo, string changeType, int? pageIndex)
        {
            var returnValue = new PagedList<CBDsDealerRebatesRecord>();

            var pager = new Pager<CBDsDealerRebatesRecord>
            {
                PageFilter = new CBDsDealerRebatesRecord
                {
                    RecommendSysNo = customerSysNo,
                    Genre = changeType
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsDealerRebatesRecordDao.Instance.GeDirectOrdersList(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }
        /// <summary>
        /// 分销团队
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pager"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public PagedList<CBCCrCustomerList> GetMyDistTemsList(int customerSysNo, int? typeid, int? pageIndex)
        {
            var returnValue = new PagedList<CBCCrCustomerList>();

            var pager = new Pager<CBCCrCustomerList>
            {
                PageFilter = new CBCCrCustomerList
                {
                    SysNo = customerSysNo,
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsDealerRebatesRecordDao.Instance.GetMyDistTemsList(typeid,ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        #endregion

        public void ExportDealerRebatesRecordExcel(IList<CBDsDealerRebatesRecord> list, string userIp, int operatorSysno)
        {
            try
            {
                // 查询订单
                IList<CBOutputDealerRebatesRecord> exportOrders = new List<CBOutputDealerRebatesRecord>();
                foreach (var mod in list)
                {
                    exportOrders.Add(new CBOutputDealerRebatesRecord()
                    {
                        动作类型 = (mod.Action == "2" ? "购物" : "其他"),
                        返利金额 = mod.Rebates.ToString(),
                        返利类型 = mod.Genre == "1" ? "一级" : mod.Genre == "2" ? "二级" : mod.Genre == "3" ? "三级" : "",
                        返利时间 = mod.RebatesTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        来源编号 = mod.OrderSysNo.ToString(),
                        所属分销商 = mod.RDealerName,
                        推荐人名称 = mod.RecommendName,
                        推荐人账号 = mod.RecommendAccount,
                        消费者名称 = mod.ComplyName,
                        消费者账号 = mod.ComplyAccount,
                        状态 = mod.Status == "0" ? "冻结" : mod.Genre == "1" ? "成功" : mod.Genre == "2" ? "作废" : "",
                    });
                }
                var fileName = string.Format("返利记录管理({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                    序号
                    出货仓
                    销售日期
                    销售部门
                    收款账号
                    内部订单号
                    销售订单号
                    电商平台名称
                    电商平台备案号
                    电商客户名称
                    电商商户备案号
                    电商客户电话
                    订单人姓名
                    订单人证件类型
                    订单人证件号
                    订单人电话
                    订单日期
                    收件人姓名
                    收件人证件号
                    收件人地址
                    收件人电话
                    商品品名
                    商品货号
                    申报数量
                    申报单价
                    申报总价
                    调价金额
                    优惠劵金额
                    运费
                    保价费
                    税款
                    毛重
                    净重
                    选用的快递公司
                    网址
                    备注
                    客户代号
                    平台编码
                    支付交易号
                    支付企业名称
                    商品单位
                    商品条形码
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportSoOrders<CBOutputDealerRebatesRecord>(exportOrders,
                                    new List<string> { "所属分销商","推荐人账号","推荐人名称","消费者账号","消费者名称","动作类型","返利类型","返利金额","来源编号","状态","返利时间"},
                                    fileName);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
    }
}
