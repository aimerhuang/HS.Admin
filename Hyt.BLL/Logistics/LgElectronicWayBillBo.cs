using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Pisen.Framework.Service.Proxy;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 电子面单业务操作类
    /// </summary>
    /// <remarks>2015-9-29 杨浩 创建</remarks>
    public class LgElectronicWayBillBo : BOBase<LgElectronicWayBillBo>
    {
        /// <summary>
        /// 插入电子面单信息
        /// </summary>
        /// <param name="model"></param>
        /// <remarks> 
        /// 2015-9-29 杨浩 创建
        /// </remarks>
        public int Insert(LgElectronicWayBill model)
        {
            return ILgElectronicWayBillDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新电子面单状态
        /// </summary>
        /// <param name="whstockoutsysno">出库单编号</param>
        /// <param name="status">状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>结果（成功返回true,否则返回false）</returns>
        /// <remarks>2015-9-29 杨浩 创建</remarks>
        public bool UpdateStatus(int whstockoutsysno, LogisticsStatus.电子面单状态 status, int userSysNo)
        {
            return ILgElectronicWayBillDao.Instance.UpdateStatus(whstockoutsysno, status, userSysNo);
        }

        /// <summary>
        /// 获取电子面单
        /// </summary>
        /// <param name="stockOuts">出库单集合</param>
        /// <returns></returns>
        /// <remarks>2015-9-29 杨浩 创建</remarks>
        public List<LgElectronicWayBill> GetElectronicWayBill(List<WhStockOut> stockOuts, int userSysNo)
        {
            //var response = LgBestExpressSurfaceBillServiceBo.Instance.GetSurfaceBillNo(stockOuts);
            List<LgElectronicWayBill> list = new List<LgElectronicWayBill>();
            //var applyBillResult = response.ApplyBillResult;
            //if (response.IsSuccess)
            //{
               
            //    if (applyBillResult.Flag == "SUCCESS")
            //    {
            //        var detailList = applyBillResult.DetailList;
            //        foreach (var item in detailList)
            //        {
            //            list.Add(new LgElectronicWayBill
            //            {
            //                BillProvideSiteCode = item.BillProvideSiteCode,
            //                BillProvideSiteName = item.BillProvideSiteName,
            //                CreateDate = DateTime.Now,
            //                CreatedBy = userSysNo,
            //                LastUpdateBy = userSysNo,
            //                LastUpdateDate = DateTime.Now,
            //                MarkDestination = item.MarkDestination,
            //                PackageCode = item.PkgCode,
            //                SortingSiteCode = item.SortingSiteCode,
            //                SortingCode = item.SortingCode,
            //                SortingSiteName = item.SortingSiteName,
            //                Status = (int)LogisticsStatus.电子面单状态.未确认,
            //                WayBillNo = item.MailNo,
            //                WhStockOutSysNo = Convert.ToInt32(item.CustomerOrderCode),
            //            });
            //        }
            //        return list;
            //    }
            //}
            //foreach (var item in stockOuts)
            //{
            //    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台,
            //         " 获取百世汇通电子面单未成功",
            //         LogStatus.系统日志目标类型.出库单, item.SysNo, new Exception(response.ApplyBillXmlResult), string.Empty, userSysNo);
            //}
            //string errormsg = applyBillResult.ErrorDescription;//错误文字
            //if (string.IsNullOrEmpty(errormsg))
            //{
            //    errormsg = applyBillResult.ErrorCode;//错误代码
            //}
            //throw new HytException(errormsg);//输出消息
            return list;
        }

        /// <summary>
        /// 确认或者取消电子面单
        /// </summary>
        /// <param name="stockOut">出库单</param>
        /// <param name="actionType">电子面单操作类型</param>
        /// <param name="userid">用户编号</param>
        /// <remarks>2015-10-8 杨浩 创建</remarks>
        public bool ConfirmElectronicWayBill(WhStockOut stockOut, LogisticsStatus.电子面单操作类型 actionType,int userid)
        {
            //UpdateStatus(stockOut.SysNo, actionType == LogisticsStatus.电子面单操作类型.取消订单 ? LogisticsStatus.电子面单状态.作废 : LogisticsStatus.电子面单状态.已确认, userid);//更新本地数据库中的电子面单状态
            //var response =  LgBestExpressSurfaceBillServiceBo.Instance.UpdateBill(stockOut, actionType);
            //if (response.IsSuccess)
            //{
            //    if (response.ExpressOrderResult.OrderStatus == "ACCEPT")//接单成功
            //    {
            //        return true;
            //    }
            //    var firstOrDefault = response.ExpressOrderResult.Errors.ErrorList.FirstOrDefault();
            //    if (firstOrDefault != null && actionType != LogisticsStatus.电子面单操作类型.取消订单)
            //    {
            //        throw new HytException(firstOrDefault.ErrorDescription);//接单失败
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //else
            //{
            //    var error = response.ExpressOrderResult.Errors.ErrorList.FirstOrDefault();
            //    if (error != null)
            //    {
            //        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台,
            //            " 确认或取消百世汇通电子面单未成功，出库单号：" + response.ExpressOrderResult.CustomerOrderCode + "错误编号：" + error.ErrorCode + "错误描述：" + error.ErrorDescription,
            //            LogStatus.系统日志目标类型.出库单, stockOut.SysNo, null, string.Empty, userid);
            //        if (actionType != LogisticsStatus.电子面单操作类型.取消订单)
            //        {
            //            throw new HytException(error.ErrorCode);
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    if (actionType != LogisticsStatus.电子面单操作类型.取消订单)
            //    {
            //        throw new HytException(response.ExpressOrderXmlResult);
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            return true;
        }


        /// <summary>
        /// 确认或者取消电子面单
        /// </summary>
        /// <param name="stockOutsysno">出库单编号</param>
        /// <param name="actionType">电子面单操作类型</param>
        /// <param name="userid">用户编号</param>
        /// <remarks>2015-10-8 杨浩 创建</remarks>
        public bool UpdateElectronicWayBill(int stockOutsysno, LogisticsStatus.电子面单操作类型 actionType, int userid)
        {
            var stockOut =Hyt.BLL.Warehouse.WhWarehouseBo.Instance.Get(stockOutsysno);//出库单
            if (stockOut != null && stockOut.DeliveryTypeSysNo==Hyt.Model.SystemPredefined.DeliveryType.百世汇通电子面单)//百事汇通
            {
                if (actionType == LogisticsStatus.电子面单操作类型.取消订单)
                {
                    var electronModel = LgElectronicWayBillBo.Instance.GetElectronicWayBillByStockOutSysNo(stockOutsysno);//获取电子面单
                    if (electronModel == null)
                    {
                        return false;
                    }
                }
                return ConfirmElectronicWayBill(stockOut, actionType, userid);//确认或者取消电子面单
            }
            return false;
        }

        /// <summary>
        /// 根据出库单编号获取电子面单信息
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns>电子面单信息</returns>
        /// <remarks>2015-9-29 杨浩 创建</remarks>
        public LgElectronicWayBill GetElectronicWayBillByStockOutSysNo(int stockOutSysNo)
        {
            return ILgElectronicWayBillDao.Instance.GetElectronicWayBillByStockOutSysNo(stockOutSysNo);
        }
    }
}
