using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model.WorkflowStatus;
using System.Transactions;
using Hyt.BLL.Log;
using Hyt.BLL.CRM;

namespace Hyt.BLL.SellBusiness
{
    /// <summary>
    /// 分销商
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class CrCustomerRebatesRecordBo : BOBase<CrCustomerRebatesRecordBo>
    {

        #region 返利记录
        /// <summary>
        /// 分页获取返利记录
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<CBCrCustomerRebatesRecord> GetCrCustomerRebatesRecordList(ParaCustomerRebatesRecordFilter filter)
        {
            return ICrCustomerRebatesRecordDao.Instance.GetCrCustomerRebatesRecordList(filter);
        }

        /// <summary>
        /// 保存返利记录
        /// </summary>
        /// <param name="model">返利记录</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveCrSellBusinessGrade(CrCustomerRebatesRecord model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            CrCustomerRebatesRecord entity = ICrCustomerRebatesRecordDao.Instance.GetEntity(model.SysNo);
            if(entity != null)
            {
                model.SysNo = entity.SysNo;
                ICrCustomerRebatesRecordDao.Instance.Update(model);
                r.Status = true;
            }
            else
            {
                ICrCustomerRebatesRecordDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CrCustomerRebatesRecord GetEntity(int SysNo)
        {
            return ICrCustomerRebatesRecordDao.Instance.GetEntity(SysNo);
        }
        /// <summary>
        /// 删除返利记录
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = ICrCustomerRebatesRecordDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        /// <summary>
        /// 获取订单可返点的状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public int GetOrderRebatesStatus(int orderSysNo)
        {
            return ICrCustomerRebatesRecordDao.Instance.GetOrderRebatesStatus(orderSysNo);
        }

        /// <summary>
        /// 执行返利操作
        /// </summary>
        /// <param name="delayDay">获取可以执行返利的记录的天数</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <remarks>
        /// 2016-1-13 杨浩 创建
        /// </remarks>
        public void CrCustomerRebatesRecordToCustomer(int delayDay, int dealerSysNo, int orderSysNo=0)
        {
            //获得用户到期的返利记录（按订单编号升序排序）
            var list = ICrCustomerRebatesRecordDao.Instance.GetRebatesRecordList(delayDay, dealerSysNo, orderSysNo);

            //订单返点状态 0：不能返点 1：可返点 2：待返点
            int orderRebatesStatus = 2;

            //分销商返利列表
            var dsRebateList = Hyt.BLL.Distribution.DsPrePaymentItemBo.Instance.GetExpireListBySource((int)DistributionStatus.预存款明细来源.返利, delayDay, dealerSysNo,orderSysNo);

            //遍历分销商返利记录
            foreach (var dsItem in dsRebateList)
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        decimal brokerage = 0;

                        orderRebatesStatus = GetOrderRebatesStatus(dsItem.SourceSysNo);

                        var cRecords = list.Where(x => x.OrderSysNo == dsItem.SourceSysNo);

                        if (orderRebatesStatus == 2 || cRecords == null)
                            continue;

                        //遍历会员返利记录
                        foreach (CrCustomerRebatesRecord item in cRecords)
                        {
                            if (orderRebatesStatus == 0)
                            {
                                brokerage = -(item.Rebates);
                            }
                            else if (orderRebatesStatus == 1)
                            {
                                brokerage = item.Rebates;
                            }

                            BLL.CRM.CrCustomerBo.Instance.UpdateCustomerBrokerage(brokerage, item.RecommendSysNo);
                        }

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        BLL.Log.LocalLogBo.Instance.Write(ex, "CrCustomerRebatesRecordToCustomerLog");
                    }

                }
            }

        }
        /// <summary>
        /// 执行返利操作
        /// </summary>
        /// <param name="delayDay">获取可以执行返利的记录的天数</param>
        /// <remarks>
        /// 2015-10-19 杨云奕 添加
        /// 2016-1-13 杨浩 重构
        /// </remarks>
        public void CrCustomerRebatesRecordToCustomer(int delayDay = 0)
        {
            CrCustomerRebatesRecordToCustomer(delayDay,-1);                         
        }

        /// <summary>
        /// 设置返利佣金操作
        /// </summary>
        /// <param name="customerRebatesRecord">返利日志详情</param>
        /// <param name="rcReturnItems">退换货扣除返利列表</param>
        /// <remarks>
        /// 2015-10-19 杨云奕 添加
        /// 2016-1-5 杨浩 重构
        /// </remarks>
        public void SetCrCustomerRebatesRecordToCustomerBrokerage(CrCustomerRebatesRecord customerRebatesRecord, List<CBReurnDeductRebates> rcReturnItems)
        {                  
            //获取推荐人客户信息
            CBCrCustomer customer = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetModel(customerRebatesRecord.RecommendSysNo);

            //获当前的返利记录
            //rcReturnItems = rcReturnItems.Where(x => x.SysNo == customerRebatesRecord.SysNo).ToList();

            foreach (var item in rcReturnItems)
            {
                //item.ReturnStatus=0 代表订单无退换货
                if (item.ReturnStatus == 0 || item.ReturnStatus == (int)RmaStatus.退换货状态.作废)
                {
                    //更新返利列表状态，完成的部分
                    customerRebatesRecord.Status = "1";
                    Hyt.DataAccess.SellBusiness.ICrCustomerRebatesRecordDao.Instance.Update(customerRebatesRecord);
                    //减去用户的冻结资金
                    customer.BrokerageFreeze =customer.BrokerageFreeze-customerRebatesRecord.Rebates;
                    //customer.BrokerageTotal += recordMod.Rebates;

                    //增加用户的可提佣金
                    customer.Brokerage = customer.Brokerage+customerRebatesRecord.Rebates;
                    Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Update(customer);

                    //Hyt.BLL.Distribution.DsPrePaymentItemBo.Instance.UpdatePrePaymentItemStatus(customerRebatesRecord.OrderSysNo,(int)DistributionStatus.预存款明细来源.返利,(int)DistributionStatus.预存款明细状态.完结);
                    //DataAccess.Distribution.IDsPrePaymentDao.Instance.AddAvailableAmount(customerRebatesRecord.DealerSysNo,customerRebatesRecord.Rebates,0);
                }
                else if (item.ReturnStatus == (int)RmaStatus.退换货状态.已完成)
                {
                    //更新返利列表状态，作废的部分
                    customerRebatesRecord.Status ="2";

                    //更新返利记录状态
                    Hyt.DataAccess.SellBusiness.ICrCustomerRebatesRecordDao.Instance.Update(customerRebatesRecord);
                    //减去用户的冻结金额部分
                    customer.BrokerageFreeze =customer.BrokerageFreeze-item.DeductRebates;
                    Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Update(customer);



                }
            }
           
        }

        /// <summary>
        /// 提现订单导出
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-2-10 杨云奕 创建</remarks>
        public void ExportPredepositCashOrders(int id, string userIp, int operatorSysno)
        {
            try
            {
                // 查询订单
                var predepositCashInfo = BLL.SellBusiness.CrPredepositCashBo.Instance.GetModel(id);

                var preCashAssoInfo = CrPredepositCashRebatesRecordAssociationBo.Instance.GetModel(id, predepositCashInfo.PdcUserId);
                IList<CBCrCustomerRebatesRecord> orderList = new List<CBCrCustomerRebatesRecord>();
                if (preCashAssoInfo != null)
                {
                    var filter = new ParaCustomerRebatesRecordFilter();
                    filter.SysNoList = preCashAssoInfo.CrCustomerRebatesRecordSysNos;
                    filter.Id = 1;
                    filter.PageSize = 999999;
                    var pager = CrCustomerRebatesRecordBo.Instance.GetCrCustomerRebatesRecordList(filter);
                    orderList= pager.Rows;
                }
                List<int> orderSysNos = new List<int>();
                foreach (var mod in orderList)
                {
                    orderSysNos.Add(mod.OrderSysNo);
                }
                List<CBSoOrderItem> items = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNos.ToArray());

                List<OutputCustomerRebatesRecord> recordList = new List<OutputCustomerRebatesRecord>();

                foreach(var mod in orderList)
                {
                    List<CBSoOrderItem> tempItems = items.FindAll(p => p.OrderSysNo == mod.OrderSysNo);
                    recordList.Add(new OutputCustomerRebatesRecord()
                    {
                        订单编号 = mod.OrderSysNo.ToString(),
                        推荐人名称 = mod.RecommendName,
                        消费者名称 = mod.ComplyName,
                        返利金额 = mod.Rebates.ToString(),
                        返利状态 = mod.Status == "0" ? "冻结" : mod.Status == "1" ? "完结" : mod.Status == "2" ? "作废" : "",
                        分销商等级 = mod.Genre + "级佣金",
                        出库仓库 = mod.WarehouseName,
                        订单金额 = mod.OrderAmount.ToString(),
                         商品金额=mod.ProductAmount,
                        优惠金额 = mod.CouponAmount,
                         运费金额=mod.FreightAmount,
                          商品调价 = mod.ProductChangeAmount,
                        商品名称 = tempItems[0].ProductName,
                        商品编号 = tempItems[0].ProductSysNo.ToString(),
                        购买数量 = tempItems[0].Quantity.ToString(),
                        购买金额 = tempItems[0].SalesAmount.ToString()
                    });
                    for(int i=1;i<tempItems.Count;i++)
                    {
                        recordList.Add(new OutputCustomerRebatesRecord()
                        {
                            商品名称 = tempItems[i].ProductName,
                            商品编号 = tempItems[i].ProductSysNo.ToString(),
                            购买数量 = tempItems[i].Quantity.ToString(),
                            购买金额 = tempItems[i].SalesAmount.ToString()
                        });
                    }
                }


                var fileName = string.Format("会员提现订单明细({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));


                Util.ExcelUtil.ExportSoOrders<OutputCustomerRebatesRecord>(recordList,
                                    new List<string> {  
                                        "订单编号", "推荐人名称", "消费者名称", "分销商等级",
                                        "返利金额", "返利状态", "出库仓库","商品金额","商品调价","优惠金额", "运费金额","订单金额",
                                        "商品编号", "商品名称", "购买数量", "购买金额"
                                    },
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
        #endregion
    }

    public class OutputCustomerRebatesRecord
    {
        public string 订单编号 {   get;set; }
        public string 推荐人名称 { get; set; }
        public string 消费者名称 { get; set; }
        public string 分销商等级 { get; set; }
        public string 返利金额 { get; set; }
        public string 返利状态 { get; set; }

        public string 出库仓库 { get; set; }
        public string 商品金额 { get; set; }
        public string 商品调价 { get; set; }
        public string 优惠金额 { get; set; }
        public string 运费金额 { get; set; }
        public string 订单金额 { get; set; }
        public string 商品编号 { get; set; }
        public string 商品名称 { get; set; }
        public string 购买数量 { get; set; }
        public string 购买金额 { get; set; }
    }
}
