using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Sys;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.BLL.CRM;
using Hyt.Util.JPush;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Newtonsoft.Json;
using Hyt.BLL.Warehouse;
using Hyt.Admin.Models;
using Hyt.BLL.Report;
using Hyt.Model.Common;
using Hyt.Model.Generated;
using Hyt.Model.UpGrade;
using Hyt.BLL.ApiFactory;
using System.IO;
using Hyt.DataAccess.Order;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 异步请求处理类
    /// </summary>
    /// <remarks>2013-06-13 杨浩 添加</remarks>
    public class AjaxController : BaseController
    {
        /// <summary>
        /// 同步Kis订单100
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-03-01 罗勤瑶 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult GetGegeJia()
        {
            var result = new Result()
            {
                Message = "同步成功！",
                Status = true
            };
            try
            {
                result = Hyt.BLL.Order.SoOrderBo.Instance.ImportDsMallOrderNew();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write(ex.StackTrace, "SynchronizeOrder100ToMallLog");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 同步订单支付时间
        /// <summary>
        /// 同步订单支付时间
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤 尧 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult UpdateAllOrderPayDte()
        {
            var result = new Result()
            {
                Message = "同步成功！",
                Status = true
            };
            try
            {
                ISoOrderDao.Instance.UpdateAllOrderPayDte();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                //BLL.Log.LocalLogBo.Instance.Write(ex.StackTrace, "SynchronizeOrder100ToMallLog");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  ERP接口
        /// <summary>
        /// 同步Kis订单100
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-03-01 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeOrder100ToMall()
        {
            var result = new Result()
            {
                Message = "同步成功！",
                Status = true
            };
            try
            {
                result=BLL.Order.SoOrderBo.Instance.ImportERPOrder100();
            }
            catch (Exception ex)
            {             
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write(ex.StackTrace, "SynchronizeOrder100ToMallLog");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同步三方订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-06-20 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeOrderHaiDaiToMall(HaiOrderParameters parameter)
        {
            var result = new Result()
            {
                Message = "同步成功！",
                Status = true
            };
            try
            {
                result = BLL.Order.SoOrderBo.Instance.ImportERPOrderHaiDai(parameter);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write(ex.StackTrace, "SynchronizeOrderToMallLog");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 同步Kis库存
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-1-11 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeKisStock(int warehouseSysNo, string pids="")
        {
            var result = new Result()
            {
                Message = "同步成功！",
                Status = true
            };
            try
            {
                result.Status = BLL.Warehouse.PdProductStockBo.Instance.SynchronizeErpStock(warehouseSysNo,pids);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 同步数据到ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-07 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeToErp(int[] sysNos=null)
        {
            var result = new Result<dynamic>
            {
                Message="同步成功！",
                Status = true
            };

            var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
  
            if (sysNos != null)
            {
                var list = Hyt.BLL.Sys.EasBo.Instance.GetList(sysNos);

               if (list.Count <= 0)
               {
                   result.Status = false;
                   result.Message = "没有可同步的数据";
               }
               int index = 0;
               for (; index < list.Count; index++)
               {
                   if (list[index].Status == 5 || list[index].Status == 0)
                   {
                       var _result = client.Resynchronization(list[index].SysNo);
                       //StatusCode == "9999"  标示已经导入过了
                       if (result.Status || result.StatusCode.ToString() == "9999")
                       {
                           continue;
                       }               
                   }
                 
               }
            }
            else
            {
                var list = Hyt.BLL.Sys.EasBo.Instance.GetSyncList(10);

                int index = 0;
                for (; index < list.Count; index++)
                {
                    var _result = client.Resynchronization(list[index].SysNo);
                    //StatusCode == "9999"  标示已经导入过了
                    if (result.Status || result.StatusCode.ToString() == "9999")
                    {
                        continue;
                    }
                    else
                    {
                        if (result.StatusCode.ToString() == "9998")//传递中
                            index = index - 1;
                        index = list.FindLastIndex(x => x.FlowIdentify == list[index].FlowIdentify);
                    }
                }

                if (list.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "没有可同步的数据";
                }
            }
            
           return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同步数据到ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-07 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeToLiJiaErp(int[] sysNos = null)
        {
            var result = new Result<dynamic>
            {
                Message = "同步成功！",
                Status = true
            };

            var client = Extra.Erp.LiJia.LiJiaProviderFactory.CreateProvider();

            if (sysNos != null)
            {
                var list = Hyt.BLL.Sys.LiJiaBo.Instance.GetList(sysNos);

                if (list.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "没有可同步的数据";
                }
                int index = 0;
                for (; index < list.Count; index++)
                {
                    if (list[index].Status == 5 || list[index].Status == 0)
                    {
                        var _result = client.Resynchronization(list[index].SysNo);
                        result.Message = _result.Message;
                        result.Status = _result.Status;
                    }

                }
            }
            else
            {
                var list = Hyt.BLL.Sys.LiJiaBo.Instance.GetSyncList(10);

                int index = 0;
                for (; index < list.Count; index++)
                {
                    var _result = client.Resynchronization(list[index].SysNo);
                    result.Message = _result.Message;
                    result.Status = _result.Status;
                }

                if (list.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "没有可同步的数据";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同步数据到ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2018-03-22 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SynchronizeToXingYeErp(int[] sysNos = null)
        {
            var result = new Result<dynamic>
            {
                Message = "同步成功！",
                Status = true
            };

            var client = Extra.Erp.XingYe.XingYeProviderFactory.CreateProvider();

            if (sysNos != null)
            {
                var list = Hyt.BLL.Sys.XingYeBo.Instance.GetList(sysNos);

                if (list.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "没有可同步的数据";
                }
                int index = 0;
                for (; index < list.Count; index++)
                {
                    if (list[index].Status == 5 || list[index].Status == 0)
                    {
                        var _result = client.Resynchronization(list[index].SysNo);
                        //StatusCode == "9999"  标示已经导入过了
                        if (result.Status || result.StatusCode.ToString() == "9999")
                        {
                            continue;
                        }
                    }

                }
            }
            else
            {
                var list = Hyt.BLL.Sys.EasBo.Instance.GetSyncList(10);

                int index = 0;
                for (; index < list.Count; index++)
                {
                    var _result = client.Resynchronization(list[index].SysNo);
                    //StatusCode == "9999"  标示已经导入过了
                    if (result.Status || result.StatusCode.ToString() == "9999")
                    {
                        continue;
                    }
                    else
                    {
                        if (result.StatusCode.ToString() == "9998")//传递中
                            index = index - 1;
                        index = list.FindLastIndex(x => x.FlowIdentify == list[index].FlowIdentify);
                    }
                }

                if (list.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "没有可同步的数据";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 代理分销联动
        /// <summary>
        /// 获取代理商
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-9-12 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult GetAgent(int dealerSysNo)
        {
            /*
              1.检查是否为所有分销商组
              2.检查是否为代理商组
                         
            */
            var result = new Result<dynamic>
            {
                Status = true
            };

            try
            {

                CBDsDealer dealerInfo = null;

                if (dealerSysNo >= 0)
                {
                    dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(dealerSysNo);
                }

                var currentInfo = Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current;

                IList<SyUser> agentUsers = new List<SyUser>();

                //检查是否绑定所有分销商组
                if (currentInfo.IsBindAllDealer)
                {
                    //获取所有的代理商用户
                    agentUsers = BLL.Sys.SyUserBo.Instance.GetSyUserByGroupSysNo(Hyt.Model.SystemPredefined.UserGroup.包含代理商的用户组);
                }
                //检查是否为代理商组
                if (currentInfo.IsAgent)
                {
                    //清空已有的数据，只显示当前的代理商 
                    agentUsers.Clear();
                    agentUsers.Add(currentInfo.Base);
                }

                if (agentUsers != null && agentUsers.Count > 0)
                {
                    result.Data = agentUsers.Select(x => new
                    {
                        SysNo = x.SysNo,
                        Name = x.UserName
                    }).ToList();
                }

                result.StatusCode = dealerInfo != null ? dealerInfo.CreatedBy : -100;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                result.StatusCode = -1;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取经销商
        /// </summary>
        /// <param name="agentSysNo">代理商系统编码</param>
        /// <returns></returns>
        /// <remarks>2016-9-12 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult GetDealer(int agentSysNo)
        {
            var result = new Result<dynamic>
            {
                Status = true
            };
            try
            {
                var currentInfo = Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                IList<DsDealer> dealerUsers = new List<DsDealer>();
                if (agentSysNo > 0)
                {
                    dealerUsers = BLL.Distribution.DsDealerBo.Instance.GetUserDealerList(agentSysNo);
                    result.Data = dealerUsers.Select(x => new
                    {
                        UserSysNo = x.UserSysNo,
                        SysNo = x.SysNo,
                        Name = x.DealerName
                    });

                    result.StatusCode = currentInfo.Base.SysNo;
                }
            }
            catch (Exception ex)
            {

                result.Status = false;
                result.Message = ex.Message;
                result.StatusCode = -1;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region  手动确认订单收货
        /// <summary>
        /// 手动确认订单收货
        /// </summary>
        /// <param name="sysNos">订单系统编号列表</param>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public JsonResult ConfirmationOfReceipt(string sysNos)
        {
            var result = new Result
            {
                Status = true
            };

            try
            {

                var orderList = SoOrderBo.Instance.GetOrderListBySysNos(sysNos.Trim(','));
                foreach (var orderInfo in orderList)
                {
                    if (orderInfo.Status == (int)OrderStatus.销售单状态.出库待接收)
                    {
                        SoOrderBo.Instance.UpdateOrderStatusAndOnlineStatus(orderInfo, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.UserName,
                          Constant.OlineStatusType.已完成, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 微信相关
        /// <summary>
        /// 创建经销商的微信菜单
        /// </summary>
        /// <param name="sysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult CreateWeiXinMenu(int sysNo)
        {
            var result = new Result
            {
                Status = false
            };

            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.WeiXin.IWebChatService>())
            {
                result = service.Channel.CreateMenu(sysNo);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 订单推送相关接口
        /// <summary>
        /// 推送订单至海关
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="isCancel">是否取消1：是</param>
        /// <returns></returns>
        /// <remarks>2016-1-2 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult PushOrderToCustoms(int orderId, int warehouseSysNo, int isCancel = 0)
        {
            var result = new Result
            {
                Status = false
            };
            var warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);
            if (warehouseInfo == null)
            {
                result.Status = false;
                result.Message = warehouseInfo.WarehouseName + "不存在！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (warehouseInfo.Customs < 0)
            {
                result.Status = false;
                result.Message = warehouseInfo.WarehouseName + "没有绑定海关接口！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (isCancel == 1)
                result = BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance(warehouseInfo.Customs).CancelOrder(orderId, warehouseSysNo);
            else
                result = BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance(warehouseInfo.Customs).PushOrder(orderId, warehouseSysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询订单在海关的状态
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-2 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult SearchStatusInCustoms(int orderId, int warehouseSysNo)
        {
            var result = new Result
            {
                Status = false
            };
            var warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);
            if (warehouseInfo == null)
            {
                result.Status = false;
                result.Message = warehouseInfo.WarehouseName + "不存在！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (warehouseInfo.Customs < 0)
            {
                result.Status = false;
                result.Message = warehouseInfo.WarehouseName + "没有绑定海关接口！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance(warehouseInfo.Customs).SearchCustomsOrder(orderId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 推送支付信息到海关
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="payCode">支付编码</param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult PayApplyToCustoms(string orderIds)
        {
            var result = new Result
            {
                Status = false
            };


            if (string.IsNullOrWhiteSpace(orderIds))
            {
                result.Message = "请选择订单号";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var instance=ApiProviderFactory.GetPayInstance((int)CommonEnum.PayCode.易宝);

            
            var orderList=SoOrderBo.Instance.GetOrderListBySysNos(orderIds);
            foreach (var orderId in orderIds.Split(','))
            {

                var soOrder = SoOrderBo.Instance.GetEntity(int.Parse(orderId));
                if (soOrder == null)
                {
                    result.Message = "订单在系统中不存在！";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (soOrder.PayTypeSysNo != (int)CommonEnum.PayCode.易宝)
                {
                    result.Message = "请选择易宝支付的订单！";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    result = instance.ApplyToCustoms(soOrder);
                    if (result.Status)
                        result.Message = "提交成功！";
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
                     
            return Json(result, JsonRequestBehavior.AllowGet);
        }

         /// <summary>
         /// 易宝海关推单异步回执
         /// </summary>
         /// <returns></returns>
         /// <remarks>2017-09-08 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ContentResult NotifyReceipt()
        {
              Request.InputStream.Position = 0;
              StreamReader reader = new StreamReader(Request.InputStream);
              string requeststr = reader.ReadToEnd();
              Request.InputStream.Position = 0;
              BLL.Log.LocalLogBo.Instance.Write(requeststr,"YiBaoNotifyReceiptLog");
              var result=ApiProviderFactory.GetPayInstance((int)CommonEnum.PayCode.易宝).NotifyReceipt(requeststr);
              return Content("SUCCESS");
        }



        /// <summary>
        /// 查询海关支付信息报关结果
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="payCode">支付编码</param> 
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SearchCustomsPay(int orderId, int payCode)
        {
            var result = new Result
            {
                Status = false
            };
             
            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.ApiPay.IPay>())
            {
                result = service.Channel.CustomsQuery(orderId, payCode);
                if (result.Status)
                    result.Message = "报关成功！";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region
        /// <summary>
        /// 推送的订单执行方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="code">编码</param>
        /// <param name="soOrderSysNo">订单SysNo</param>
        /// <returns></returns>
        /// <returns>2016-4-1 王耀发 创建</returns>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult PushOrderToIcp(string orderId, string icpType)
        {
            var result = new Result
            {
                Status = false
            };
            int intIcpType = int.Parse(icpType);
            try
            {
                result = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance(intIcpType).PushOrder(int.Parse(orderId));
            }
            catch (Exception ex)
            {

                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 物流接口
        /// <summary>
        /// 推送订单至物流
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        /// <remarks>2016-4-2 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult PushOrderToLogistics(int orderId, int warehouseId)
        {
            var result = new Result
            {
                Status = false
            };

            try
            {
                int logistics = 0;
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseId);
                if (warehouse != null)
                {
                    logistics = warehouse.Logistics;
                }

                if (logistics <= 0)
                {
                    result.Message = "订单" + orderId + "对应的仓库(" + warehouseId + ")没有选择物流";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var logisticeIns = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(logistics);
                result = logisticeIns.AddOrderTrade(orderId);
                //if (result.Status == true)
                //{
                //    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderId);
                //}
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单" + orderId + "推送到物流：" + result.Message, LogStatus.系统日志目标类型.订单, orderId,
                    CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单" + orderId + "推送到物流失败。", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询订单物流状态
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseId">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-28 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult SearchLogisticsStatus(int orderId, int warehouseId)
        {
            var result = new Result
            {
                Status = false
            };

            try
            {
                int logistics = 0;
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseId);
                if (warehouse != null)
                    logistics = warehouse.Logistics;


                if (logistics <= 0)
                {
                    result.Message = "订单" + orderId + "对应的仓库(" + warehouseId + ")没有选择物流";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var logisticeIns = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(logistics);
                result = logisticeIns.GetLogisticsTracking(orderId);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


   

        /// <summary>
        /// 推送订单至物流
        /// </summary>
        /// <param name="orderId">订单系统编号</param>
        /// <param name="code">物流编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-25 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult PushOrderToLog(CBOrderLogistics Entity)
        {
            var result = new Result
            {
                Status = false
            };
            int intcode = int.Parse(Entity.code);
            try
            {
                Entity.Order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(Entity.orderId);
                //result = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(intcode).PushOrderToLog(Entity);
                //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单" + Entity.orderId + "推送到物流：" + result.Message, LogStatus.系统日志目标类型.订单, Entity.orderId,
                //CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单" + Entity.orderId + "推送到物流失败。", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询订单至物流
        /// </summary>
        /// <param name="orderId">订单系统编号</param>
        /// <param name="code">物流编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-25 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult SelOrderToLog(int orderId, string code)
        {
            var result = new Result
            {
                Status = false
            };
            int intcode = int.Parse(code);
            try
            {
                SoOrder order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(orderId);
                //result = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(intcode).SelOrder(order);
            }
            catch (Exception ex)
            {

                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 运费
        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="productSysNoAndNumber">产品编号和数量的链接字符(产品编号_数量,...产品编号_数量)</param>
        /// <param name="deliveryTypeSysNo"></param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="asreaSysNo">城市地区编号</param>
        /// <param name="deliverySysNo">配送方式</param>
        /// <returns></returns>
        /// <remarks>2015-12-24 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005901)]
        public JsonResult CalculateFreight(string productSysNoAndNumber, int deliveryTypeSysNo, int warehouseSysNo, int asreaSysNo)
        {

            var freight = Hyt.BLL.FreightModule.FreightModuleDaoBo.Instance.GetFareTotal(asreaSysNo, warehouseSysNo, productSysNoAndNumber, deliveryTypeSysNo);

            if (freight == null)
                freight = new FareTotal() { Name = "", Freigh = -1, DeliveryTypeSysNo = deliveryTypeSysNo };

            return Json(freight, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 配送方式
        /// <summary>
        /// 获取所有配送方式
        /// </summary>
        /// <returns>配送方式</returns>
        ///<remarks>2013－06-13 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005901)]
        public JsonResult LoadAllShipType()
        {
            var list = SoOrderBo.Instance.LoadAllDeliveryType().Select(i => new
            {
                text = i.DeliveryTypeName,
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 支付方式
        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-06-28 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public JsonResult LoadAllPayType()
        {
            var list = SoOrderBo.Instance.LoadAllPayType().Select(i => new
            {
                text = i.PaymentName,
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据配送方式获取支付方式
        /// </summary>
        /// <param name="deliverySysNo">配送方式</param>
        /// <returns>支付方式</returns>
        /// <remarks>2013-06-17 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005903)]
        public JsonResult LoadPayTypeListByDeliverySysNo(int deliverySysNo)
        {
            var list = SoOrderBo.Instance.LoadPayTypeListByDeliverySysNo(deliverySysNo).Select(i => new
            {
                text = i.PaymentName,
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 收货地址
        /// <summary>
        /// 根据收货地址编号查询收货地址列表
        /// </summary>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <returns>返回收货地址json</returns>
        /// <remarks>2013-07-09 余勇 创建</remarks> 
        [Privilege(PrivilegeCode.CM1005904)]
        [HttpGet]
        public JsonResult LoadOrderReceive(int receiveAddressSysNo)
        {
            if (receiveAddressSysNo == 0)
                return null;
            var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(receiveAddressSysNo);
            var model = new
            {
                text = string.Format("{0} {1} {2} {3}", receiveAddress.Name, GetFullAreaName(receiveAddress.AreaSysNo), receiveAddress.StreetAddress, receiveAddress.MobilePhoneNumber),
                value = receiveAddress.SysNo,
                areasysno = receiveAddress.AreaSysNo,
                street = receiveAddress.StreetAddress
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据会员ID查询收货地址列表
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>返回收货地址json</returns>
        /// <remarks>2013-06-13 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005905, PrivilegeCode.SO1001101, PrivilegeCode.SO1003101)]
        [HttpGet]
        public JsonResult LoadCustomerAddress(int sysNo)
        {
            var list = SoOrderBo.Instance.LoadCustomerAddress(sysNo).Select(i => new
            {
                text = string.Format("{0} {1} {2} {3}", i.Name, GetFullAreaName(i.AreaSysNo), i.StreetAddress, i.MobilePhoneNumber),
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据区县编号获取省市区全称
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>地址全称</returns>
        /// <remarks>2013-07-4 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005906)]
        private string GetFullAreaName(int sysNo)
        {
            BsArea area;
            BsArea city;
            BsArea province = BasicAreaBo.Instance.GetProvinceEntity(sysNo, out city, out area);
            return (province != null ? province.AreaName : "") + (city != null ? city.AreaName : "") + (area != null ? area.AreaName : "");
        }

        /// <summary>
        /// 根据会员ID查询会员默认收货地址
        /// </summary>
        /// <param name="customerSysNo">customerSysNo</param>
        /// <returns>返回会员默认收货地址json</returns>
        /// <remarks>2013-06-24 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005907)]
        [HttpGet]
        public JsonResult SearchReceiveAddressByCustomerSysNo(int customerSysNo)
        {
            var receiveAddress = CrCustomerBo.Instance.SearchReceiveAddressByCustomerSysNo(customerSysNo);
            return Json(receiveAddress, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据SysNo查询收货地址
        /// </summary>
        /// <param name="sysNo">sysNO</param>
        /// <returns>返回收货地址json</returns>
        /// <remarks>2013-06-13 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005908)]
        [HttpGet]
        public JsonResult SearchCustomerReceiveAddress(int sysNo)
        {
            var row = SoOrderBo.Instance.GetCustomerAddressBySysNo(sysNo);
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 会员信息
        /// <summary>
        /// 根据会员卡号获取会员信息
        /// </summary>
        /// <param name="cardNumber">会员卡号</param>
        /// <returns></returns>
        /// <remarks>2017-1-17 杨浩 创建</remarks>
        public JsonResult GetCustomerByCardNumber(string cardNumber)
        {
            var result = new Result<CrCustomer>()
            {
                StatusCode = 1,
                Status = false,
            };
            var customerSysNo = CrCustomerShipCardBo.Instance.GetCustomerSysNo(cardNumber);

            if(customerSysNo>0)
            {
               result.Data=BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);             
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 绑定会员卡号
        /// </summary>
        /// <param name="sysNo">客户账号</param>
        /// <param name="cardNumber">会员卡号</param>
        /// <returns></returns>
        /// <remarks>2017-1-17 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005909, PrivilegeCode.SO1001101, PrivilegeCode.SO1003101)]
        [HttpGet]
        public JsonResult BindMemberShipCard(string account, string cardNumber)
        {
            var result = new Result()
            {
                StatusCode=1,
                Status=false,
                Message = "会员卡号已绑定过，请更换卡号再点绑定！",
            };
            try
            {
                if (account=="")
                {
                    result.StatusCode = 2;
                    result.Status = false;
                    result.Message = "请填写会员账号";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var customerInfo=BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(account);
                if (customerInfo == null)
                {
                    result.StatusCode = 3;
                    result.Status = false;
                    result.Message = "请填写会员账号在系统中不存在，请先注册！";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                int sysNo = customerInfo.SysNo;
               
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    result.StatusCode = 4;
                    result.Status = false;
                    result.Message = "请选输入会员卡号";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var customerSysNo = CrCustomerShipCardBo.Instance.GetCustomerSysNo(cardNumber);
                if (customerSysNo <= 0)
                {

                    var _cardNumber = CrCustomerShipCardBo.Instance.GetCardNumber(sysNo);
                    if (string.IsNullOrEmpty(_cardNumber))
                    {
                        var model = new CrCustomerShipCard();
                        model.CardNumber = cardNumber;
                        model.CustomerSysNo = sysNo;
                        CrCustomerShipCardBo.Instance.Insert(model);
                        result.Status = true;
                        result.StatusCode = 0;
                        result.Message = "绑定成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode =2;
                result.Message = ex.Message;
            }
          
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="sysNo">sysNO</param>
        /// <returns>返回会员json</returns>
        /// <remarks>2013-06-13 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005909, PrivilegeCode.SO1001101, PrivilegeCode.SO1003101)]
        [HttpGet]
        public JsonResult SearchCustomerBySysNo(int sysNo)
        {
            var customer = SoOrderBo.Instance.SearchCustomer(sysNo);
            var levelName = string.Empty;
            if (customer != null && customer.Name == null) customer.Name = "";
            decimal balance=0m;
            if (customer != null)
            {
                var level = CrCustomerBo.Instance.SearchCustomerLevel(customer.LevelSysNo);
                levelName = level != null ? level.LevelName : "";

                var balanceInfo=BLL.Balance.CrRechargeBo.Instance.GetCrABalanceEntity(sysNo);
                if (balanceInfo != null)
                    balance = balanceInfo.AvailableBalance;
            }
            var result = new { customer, levelName, balance };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="sysNo">sysNO</param>
        /// <returns>返回会员json</returns>
        /// <remarks>2017-01-19 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005909, PrivilegeCode.SO1001101, PrivilegeCode.SO1003101)]
        [HttpPost]
        public JsonResult SearchCustomerByCardNumber(string cardNumber)
        {
            int customerSysNo=BLL.CRM.CrCustomerShipCardBo.Instance.GetCustomerSysNo(cardNumber);
            if (customerSysNo <= 0)
            {
                return Json(new { Status=0 }, JsonRequestBehavior.AllowGet);
            }
            return SearchCustomerBySysNo(customerSysNo);       
        }
        /// <summary>
        /// 不注册会员直接购买,勾选
        /// </summary>
        /// <param name="shopid">门店编号</param>
        /// <returns></returns>
        ///  <remarks>2016-05-25 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201, PrivilegeCode.LG1004101)]
        [HttpPost]
        public JsonResult NoMobileShopCustomerCheck(int shopid)
        {
            Result result = new Result() { Status = false, Message = "暂时不支持此操作!" };
            int uid = CurrentUser.Base.SysNo;
            string identity = string.Format("AutoNoMobileShopCustomer_{0}", uid);
            bool isnew = false;
            if (YwbUtil.Enter(identity) == false)
            {
                isnew = true;
            }
            try
            {
                var customer = CrCustomerBo.Instance.AutoNoMobileShopCustomer(shopid, uid, isnew);
                result = new Result() { Status = true, StatusCode = customer.SysNo, Message = isnew ? string.Empty : identity };
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                if (!isnew)
                {
                    YwbUtil.Exit(identity);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 创建一个会员
        /// </summary>
        /// <returns>json</returns>
        /// <remarks>
        /// 2013-06-09 黄志勇 创建
        /// 2013-10-15 朱家宏 增加手机验证码
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201, PrivilegeCode.LG1004101)]
        public JsonResult CustomerCreate()
        {
            var customer = new CrCustomer();
            var address = new CrReceiveAddress();
            var error = string.Empty;
            try
            {
                customer.Account = Request.Form["Account"];
                if (BLL.Web.CrCustomerBo.Instance.GetCustomerByCellphone(customer.Account) != null)
                {
                    error = "用户名已注册";
                }
                else
                {
                    //短信验证
                    var smsCode = Request.Form["mobileValidation"];
                    var mobile = customer.Account;
                    var smsValidated = ValidateMobileCode(mobile, smsCode);
                    if (!smsValidated)
                    {
                        error = "创建失败，手机验证码错误";
                        return Json(new { customer, address, error });
                    }

                    var password = "123456";
                    customer.Gender = int.Parse(Request.Form["Gender"]);
                    customer.Name = Request.Form["Name"];
                    customer.MobilePhoneNumber = Request.Form["MobilePhoneNumber"];
                    customer.EmailAddress = Request.Form["EmailAddress"];
                    customer.EmailStatus = (int)CustomerStatus.邮箱状态.未验证;
                    customer.MobilePhoneStatus = (int)CustomerStatus.手机状态.未验证;
                    customer.RegisterSource = (int)CustomerStatus.注册来源.门店;
                    customer.IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是;
                    customer.IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是;
                    customer.IsPublicAccount = (int)CustomerStatus.是否是公共账户.否;


                    if (Request.QueryString["from"] != null)
                    {
                        var reg = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
                        var from = Request.QueryString["from"].ToString().Trim();
                        var fromNo = Request.QueryString["fromNo"].ToString().Trim();
                        if (reg.IsMatch(from)) //判断是否为正整数
                        {
                            customer.RegisterSource = int.Parse(from);
                            if (reg.IsMatch(fromNo)) customer.RegisterSourceSysNo = fromNo;
                        }
                    }
                    customer.RegisterDate = DateTime.Now;
                    customer.LevelSysNo = CustomerLevel.初级;
                    customer.Password = password; // EncryptionUtil.EncryptWithMd5AndSalt(password); 余勇修改 2014-09-12
                    customer.Status = (int)CustomerStatus.会员状态.有效;
                    customer.CreatedBy = CurrentUser.Base.SysNo;
                    customer.CreatedDate = DateTime.Now;

                    address.SysNo = 0;
                    address.AreaSysNo = int.Parse(Request.Form["dpdArea"]);
                    address.Name = Request.Form["ReceiveName"];
                    address.PhoneNumber = Request.Form["PhoneNumber"];
                    address.MobilePhoneNumber = Request.Form["MobilePhoneNumber1"];
                    address.StreetAddress = Request.Form["StreetAddress"];
                    address.ZipCode = Request.Form["ZipCode"];
                    address.IsDefault = 1;
                    SoOrderBo.Instance.CreateCustomer(customer, address);
                    if (VHelper.Do(customer.MobilePhoneNumber, VType.Mobile))
                    {
                        BLL.Extras.SmsBO.Instance.发送注册成功短信(customer.MobilePhoneNumber, customer.Account, password);
                    }

                    ClearMobileValidation(mobile);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建会员",
                                         LogStatus.系统日志目标类型.客户管理, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
                    //注册成功后写门店新增会员明细
                    if (customer.RegisterSource == (int)CustomerStatus.注册来源.门店)
                    {
                        var wareHouse =
                            WhWarehouseBo.Instance.GetWarehouseEntity(int.Parse(customer.RegisterSourceSysNo));
                        var indoorStaff = SyUserBo.Instance.GetSyUser(customer.CreatedBy);
                        ReportBO.Instance.InsertShopNewCustomerDetail(new rp_ShopNewCustomerDetail
                        {
                            CustomerSysno = customer.SysNo,
                            WarehouseSysNo = wareHouse.SysNo,
                            Warehousename = wareHouse.WarehouseName,
                            IndoorStaffSysNo = indoorStaff.SysNo,
                            IndoorStaffName = indoorStaff.UserName,
                            CustomerName = customer.Name,
                            MobilePhoneNumber = customer.Account,
                            Amount = 0,
                            RegisterDate = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建会员错误", LogStatus.系统日志目标类型.客户管理, CurrentUser.Base.SysNo, ex);
            }

            var result = new { customer, address, error };
            return Json(result);
        }

        /// <summary>
        /// 创建一个会员
        /// </summary>
        /// <returns>json</returns>
        /// <remarks>
        /// 2013-06-09 黄志勇 创建
        /// 2013-10-15 朱家宏 增加手机验证码
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201, PrivilegeCode.LG1004101)]
        public JsonResult CustomerEdit()
        {
            var customer = new CrCustomer();
            var address = new CrReceiveAddress();
            var error = string.Empty;
            try
            {
                var sysNo = Request.Form["SysNo"] != null ? int.Parse(Request.Form["SysNo"]) : 0;
                customer = BLL.Web.CrCustomerBo.Instance.GetModel(sysNo);
                if (customer != null)
                {
                    ////短信验证
                    //var smsCode = Request.Form["mobileValidation"];
                    //var mobile = customer.Account;
                    //var smsValidated = ValidateMobileCode(mobile, smsCode);
                    //if (!smsValidated)
                    //{
                    //    error = "创建失败，手机验证码错误";
                    //    return Json(new { customer, address, error });
                    //}
                    var mobilePhoneNumber = Request.Form["MobilePhoneNumber"];
                    if (mobilePhoneNumber != customer.MobilePhoneNumber)
                    {
                        customer.MobilePhoneNumber = mobilePhoneNumber;
                        customer.MobilePhoneStatus = (int)CustomerStatus.手机状态.未验证;
                    }

                    var emailAddress = Request.Form["EmailAddress"];
                    if (emailAddress != customer.EmailAddress)
                    {
                        customer.EmailAddress = emailAddress;
                        customer.EmailStatus = (int)CustomerStatus.邮箱状态.未验证;
                    }
                    customer.Gender = !string.IsNullOrEmpty(Request.Form["Gender"]) ? int.Parse(Request.Form["Gender"]) : 0;
                    customer.Name = Request.Form["Name"];
                    customer.NickName = Request.Form["NickName"];
                    customer.Birthday = DateTime.Parse(Request.Form["Birthday"]);
                    customer.IDCardNo = Request.Form["IDCardNo"];
                    customer.MaritalStatus = !string.IsNullOrEmpty(Request.Form["MaritalStatus"]) ? int.Parse(Request.Form["MaritalStatus"]) : 0;
                    customer.MonthlyIncome = Request.Form["MonthlyIncome"];
                    customer.Hobbies = Request.Form["Hobbies"];
                    if (!string.IsNullOrEmpty(Request.Form["dpdArea"]))
                    {
                        customer.AreaSysNo = int.Parse(Request.Form["dpdArea"]);
                    }

                    BLL.Web.CrCustomerBo.Instance.Update(customer);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "编辑会员",
                                         LogStatus.系统日志目标类型.客户管理, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            var result = new { customer, address, error };
            return Json(result);
        }

        /// <summary>
        /// 查询会员(模糊)
        /// </summary>
        /// <param name="word">中文姓名，帐号</param>
        /// <returns>返回列表json</returns>
        /// <remarks>2013-06-13 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005905, PrivilegeCode.SO1001101, PrivilegeCode.SO1003101)]
        [HttpGet]
        public JsonResult SearchCustomer(string word, int dealer = -1)
     {
            var list = SoOrderBo.Instance.SearchCustomer(word, dealer);
            if (list != null && list.Count > 0)
            {
                list.ApplyParallel(i =>
                {
                    if (i.Name == null) i.Name = "";
                    if (i.MobilePhoneNumber == null) i.MobilePhoneNumber = "";
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// 检查帐号名是否存在
        /// </summary>
        /// <param name="account">帐号</param>
        /// <returns>true:false</returns>
        /// <remarks>2013-06-26 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201, PrivilegeCode.CM1005911)]
        public JsonResult NotExistCustomer(string account)
        {
            bool flg = true;
            var u = BLL.Web.CrCustomerBo.Instance.GetCustomerByCellphone(account);
            if (u != null)
            {
                flg = false;
            }
            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// 检查输入员帐号
        /// </summary>
        /// <param name="account">帐号</param>
        /// <returns>true:false</returns>
        /// <remarks>2014-01-16 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1003101)]
        public JsonResult ExistCustomer(string account)
        {
            bool flg = false;
            var u = BLL.Web.CrCustomerBo.Instance.GetCustomerByCellphone(account);
            if (u != null)
            {
                flg = true;
            }
            return Json(flg, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region [省市区联动] 2013-06-13 杨晗 创建

        /// <summary>
        /// 获取省市区
        /// </summary>
        /// <param name="parentSysNo">区域上级系统号</param>
        /// <returns>省市区数据列表</returns>
        /// <remarks>2013-06-13 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.CM1005912)]
        public ActionResult GetArea(int? parentSysNo)
        {
            parentSysNo = parentSysNo ?? 0;
            return Json(BasicAreaBo.Instance.SelectArea((int)parentSysNo));
        }

        /// <summary>
        /// 根据区域系统号获取所属的省市系统号
        /// </summary>
        /// <param name="areaSysNo">地区编号</param>
        /// <returns>省市系统号</returns>
        /// <remarks>2013-06-13 杨晗 创建</remarks>
        /// <remarks>2013-06-14 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.CM1005912)]
        public ActionResult GetAreaSysNo(int areaSysNo)
        {

            int countryNo = -1;
            int provinceNo = -1;
            int cityNo = -1;
            int areaNo = -1;
            Model.BsArea provinceEntity;
            Model.BsArea cityEntity;
            Model.BsArea areaEntity;
            var countryEntity = BasicAreaBo.Instance.GetCountryEntity(areaSysNo, out provinceEntity, out cityEntity, out areaEntity);
            if (countryEntity != null)
            {
                countryNo = countryEntity.SysNo;
            }
            if (provinceEntity != null)
            {
                provinceNo = provinceEntity.SysNo;
            }
            if (cityEntity != null)
            {
                cityNo = cityEntity.SysNo;
            }
            if (areaEntity != null)
            {
                areaNo = areaEntity.SysNo;
            }
            return Json(new { success = true, a = areaNo, c = cityNo, p = provinceNo, y = countryNo });
        }

        #endregion

        #region [省市区联动]

        /// <summary>
        /// 获取省下面的城市列表
        /// </summary>
        /// <param name="provinceSysNo">省编号</param>
        /// <returns>返回列表json</returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        /// 
        [Privilege(PrivilegeCode.CM1005912)]
        [HttpGet]
        public ActionResult LoadCity(int provinceSysNo)
        {
            var list =
                Hyt.BLL.Order.SoOrderBo.Instance.LoadCity(provinceSysNo)
                   .Select(m => new SelectListItem { Text = m.AreaName, Value = m.SysNo.ToString() });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取市下面的区县信息
        /// </summary>
        /// <param name="citySysNo">城市编号</param>
        /// <returns>返回列表json</returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005912)]
        [HttpGet]
        public ActionResult LoadArea(int citySysNo)
        {
            var list =
                Hyt.BLL.Order.SoOrderBo.Instance.LoadArea(citySysNo)
                   .Select(m => new SelectListItem { Text = m.AreaName, Value = m.SysNo.ToString() });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 该区域支持的配送方式信息

        /// <summary>
        /// 该区域支持的配送方式信息
        /// </summary>
        /// <param name="areaNo">区域编号</param>
        /// <returns>该区域支持的配送方式信息</returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005913)]
        [HttpGet]
        public ActionResult LoadDeliveryTypeByAreaNo(int areaNo)
        {
            var list = SoOrderBo.Instance.LoadDeliveryTypeByAreaNo(areaNo).Where(i => i.Status == 1)
                   .Select(m => new HtmlOption
                   {
                       Text = m.DeliveryTypeName,
                       Value = m.SysNo.ToString(),
                       label = m.DeliveryTypeName,
                       optgroup = (m.IsThirdPartyExpress != 1 && m.ParentSysNo == 0)
                   });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 该区域支持的配送方式信息
        /// </summary>
        /// <param name="areaNo">区域编号</param>
        /// <param name="cityNo">城市编号</param>
        /// <param name="x">经度</param>
        /// <param name="y">维度</param>
        /// <returns>该区域支持的配送方式信息</returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005913)]
        [HttpGet]
        public ActionResult LoadDeliveryTypeByAreaAndMap(int areaNo, int cityNo, double x, double y)
        {
            var res = Hyt.BLL.Logistics.WhDeliveryScopeBo.Instance.IsInScope(cityNo, x, y);//是否在加盟商当日达配送范围内     
            var list = SoOrderBo.Instance.LoadDeliveryTypeByAreaNo(areaNo, cityNo, res.Status).Where(i => i.Status == 1)
                .Select(m => new HtmlOption
                {
                    IsInMap = res.Status,
                    Text = m.DeliveryTypeName,
                    Value = m.SysNo.ToString(),
                    label = m.DeliveryTypeName,
                    optgroup = (m.IsThirdPartyExpress != 1 && m.ParentSysNo == 0),
                    WarehouseNo = (m.ParentSysNo == DeliveryType.百城当日达 && res.Data != null) ? res.Data.SysNo : 0,//仓库
                    WarehouseName = (m.ParentSysNo == DeliveryType.百城当日达 && res.Data != null) ? res.Data.WarehouseName : string.Empty,//仓库
                }).ToList();//当日达将默认仓库返回
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [地区仓库/门店树]
        /// <summary>
        /// 地区仓库/门店树，待完善
        /// </summary>
        /// <param name="id">ztree id</param>
        /// <param name="deliveryTypeSysNo">配送方式</param>
        ///  <param name="cityID">城市编号</param>
        /// <returns>结合ztree树形控件展现</returns>
        ///<remarks>2013-06-17 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.CM1005914)]
        public ActionResult GetAreaWarehouseTree(int? id, int? deliveryTypeSysNo, int? cityID)
        {
            int? warehouseType = null;
            if (deliveryTypeSysNo == 0) deliveryTypeSysNo = null;
            if (id.HasValue && id.Value != 0)
            {
                var entity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(id.Value, BasicStatus.地区状态.有效);
                //地区信息
                if (entity != null && entity.AreaLevel < 3)
                {
                    //加载市，地区
                    var sublist = BasicAreaBo.Instance.SelectAreaWithWarehouse(id.Value, warehouseType, deliveryTypeSysNo).Select(m => new
                    {
                        id = m.SysNo,
                        name = m.AreaName,
                        target = "area",
                        isParent = "true"
                    });
                    return Json(sublist, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //加载仓库
                    //注意筛选门店或者仓库 （待实现)
                    var warehouseList =
                        Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWarehouseListByArea(id.Value, warehouseType, deliveryTypeSysNo).Select(m => new
                        {
                            id = m.SysNo,
                            name = m.BackWarehouseName,
                            target = "Warehouse",
                            isParent = "false",
                            icon = m.IsSelfSupport == (int)WarehouseStatus.是否自营.是 ? Url.Content("~/Theme/images/icons/Warehouse.png") : Url.Content("~/Theme/images/icons/Warehouse1.png")
                        });
                    return Json(warehouseList, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //默认加载省
                var list = BasicAreaBo.Instance.SelectAreaWithWarehouse(0, warehouseType, deliveryTypeSysNo).Select(m => new
                {
                    id = m.SysNo,
                    name = m.AreaName,
                    target = m.AreaLevel,
                    isParent = "true"
                });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>仓库信息</returns>
        /// <remarks>2013-06-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.CM1005914)]
        public ActionResult GetWarehouseInfo(int sysNo)
        {
            return Json(Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(sysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取地区支持该配送方式的默认仓库
        /// </summary>
        /// <param name="areaSysNo">地区编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <returns>地区支持该配送方式的默认仓库</returns>
        /// <remarks>2013-06-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.CM1005914)]
        public ActionResult GetDefaultWarehouse(int areaSysNo, int deliveryTypeSysNo)
        {
            Result r = new Result() { Status = false };
            var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(areaSysNo, null, deliveryTypeSysNo, WarehouseStatus.仓库状态.启用).FirstOrDefault();
            if (warehouse != null)
            {
                r.Message = warehouse.WarehouseName;
                r.Status = true;
                r.StatusCode = warehouse.SysNo;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 商品分类管理

        /// <summary>
        /// 获取商品类别树
        /// </summary>
        /// <param name="id">商品父类别编号</param>
        /// <param name="all">是否是全部节点</param>
        /// <param name="isShowDisable">是否显示禁用</param>
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-06-18 黄志勇 创建</remarks>
        /// <remarks>2013-06-24 邵斌 读取实际商品分类</remarks>
        /// <remarks>2013-07-06 邵斌 添加读取全部分类方法</remarks>
        /// <remarks>2016-04-28 刘伟豪 添加是否显示禁用</remarks>
        [Privilege(PrivilegeCode.CM1005915)]
        public JsonResult GetProductCategoryZTreeList(int? id, bool all = false, bool isShowDisable = false)
        {
            //读取商品分类中状态为有效的列表。
            IList<PdCategory> list;

            if (!id.HasValue || all)
            {
                var categoryJsonList = PdCategoryBo.Instance.GetAllCategoryJsonList(!isShowDisable);
                return Json(categoryJsonList, JsonRequestBehavior.AllowGet);
            }
            list = PdCategoryBo.Instance.GetCategoryList(id);

            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo,
                            name = c.CategoryName,
                            open = false,
                            pId = c.ParentSysNo,
                            status = c.Status,
                            isOnline = c.IsOnline
                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取指定的商品类别
        /// </summary>
        /// <param name="id">商品类别系统编号</param>
        /// <returns>返回单个商品分类数据</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CM1005915)]
        public JsonResult GetProductCategory(int id)
        {
            return Json(
                PdCategoryBo.Instance.GetCategory(id, true)
                , JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        /// 读取所以商品分类包括无效分类
        /// </summary>
        /// <returns>返回单个商品分类数据</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CM1005915)]
        public JsonResult GetAllProductCategory()
        {
            return Json(
                PdCategoryBo.Instance.GetAllCategory()
                , JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        /// 读取分类的属性组列表
        /// </summary>
        /// <param name="id">商品分类系统编号</param>
        /// <returns>返回属性组列表</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CM1005915)]
        public JsonResult GetProductCategoryAttributeGroupList(int? id)
        {
            if (id.HasValue)
            {
                if (id.Value != 0)
                    return Json(PdAttributeGroupBo.Instance.GetPdCategoryAttributeGroupList(id.Value), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #endregion

        #region 商品价格维护

        /// <summary>
        /// 读取自定商品的价格
        /// </summary>
        /// <param name="SysNo">商品的系统编号</param>
        /// <param name="showPrice">要显示出来的价格类型字符串，字符串用"，"分隔 如：基础价格+会员价格 = 0,10  0-基础价格枚举值 10-会员价格枚举值</param>
        /// <returns>返回将商品在价格表中对应的所有价格(Json数据格式)</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CM1005916)]
        public JsonResult GetProductPrice(int sysNo, string showPrice)
        {
            //生产价格来源枚举数组
            ProductStatus.产品价格来源[] showSourceList = ConvertStringToPriceSource(showPrice);

            //读取商品价格
            IList<CBPdPrice> pricelist = PdPriceBo.Instance.GetProductPrice(sysNo, showSourceList);

            //返回Json数组
            return Json(pricelist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取价格用户等级（用户包括前台会员，分销商，配送员等非系统管理员）
        /// </summary>
        /// <param name="showPrice">设置可以用来显示的用户等级</param>
        /// <returns>返回商品的价格包括用户等级名称的Json数组</returns>
        /// <remarks>2013-06-28 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CM1005917)]
        public JsonResult GetUserLevelShowList(string showPrice)
        {
            //生产价格来源枚举数组
            ProductStatus.产品价格来源[] showSourceList = ConvertStringToPriceSource(showPrice);

            //返回显示用的价格集 
            IList<CBPdPrice> pricelist = new List<CBPdPrice>();

            //将用于显示和调整的价格添加到结果集

            //添加基础价格
            if (showSourceList.Contains(ProductStatus.产品价格来源.基础价格))
            {
                pricelist.Add(new CBPdPrice()
                {
                    Price = 0
                    ,
                    PriceName = ProductStatus.产品价格来源.基础价格.ToString()
                   ,
                    SourceSysNo = 0
                        ,
                    PriceSource = (int)ProductStatus.产品价格来源.基础价格
                    ,
                    SysNo = 0
                });
            }

            //添加会员等级价格
            if (showSourceList.Contains(ProductStatus.产品价格来源.会员等级价))
            {
                //读取会员等级
                IList<CrCustomerLevel> customerLevelList = CrCustomerLevelBo.Instance.List();

                //将所有等级添加到结果集中用于前台显示
                foreach (CrCustomerLevel level in customerLevelList)
                {
                    pricelist.Add(new CBPdPrice()
                    {
                        Price = 0
                        ,
                        PriceName = level.LevelName
                        ,
                        SourceSysNo = level.SysNo
                        ,
                        PriceSource = (int)ProductStatus.产品价格来源.会员等级价
                        ,
                        SysNo = level.SysNo
                    });
                }
            }

            //添加配送员价格
            if (showSourceList.Contains(ProductStatus.产品价格来源.配送员进货价))
            {
                pricelist.Add(new CBPdPrice()
                {
                    Price = 0
                    ,
                    PriceName = ProductStatus.产品价格来源.配送员进货价.ToString()
                   ,
                    SourceSysNo = 0
                        ,
                    PriceSource = (int)ProductStatus.产品价格来源.配送员进货价
                    ,
                    SysNo = 0
                });
            }

            //ToDo 如果有其他类型要用户调价显示请将类型安上面的方法添加，如有问题请问邵斌

            //返回结果
            return Json(pricelist, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 保存价格申请数据到价格申请表
        /// </summary>
        /// <param name="priceHistories">申请调价Json数据（申请调价数组）</param>
        /// <param name="productSysNoList">商品系统编号</param>
        /// <returns>返回调价申请是否成功 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-06-28 邵斌 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001202)]
        public JsonResult SaveProductPrice(string priceHistories, string productSysNoList = null)
        {

            //返回标志
            bool success = true;

            //返回JSON数据
            dynamic jsonData = new
            {
                success = true,
                message = ""
            };

            //判断参数
            if (string.IsNullOrWhiteSpace(priceHistories))
            {
                jsonData.message = "保存失败，请正确填写您的调价申请！！";
                success = false;
            }

            IList<CBPdPriceHistory> pdPriceHistoryList = null;  //调价对象
            IList<int> pdProdcutSysNoList = null;               //产品ID

            try
            {
                //反序列化Json字符串为调价申请数组对象
                pdPriceHistoryList = JsonConvert.DeserializeObject<IList<CBPdPriceHistory>>(priceHistories);

                if (!string.IsNullOrWhiteSpace(productSysNoList))
                    //序列化产品ID
                    pdProdcutSysNoList = JsonConvert.DeserializeObject<IList<int>>(productSysNoList);

            }
            catch
            {
                //出错表示格式不正确
                jsonData.message = "保存失败，填写的数据不合法！！";
                success = false;
            }

            //根据前面数据验证来决定执行操作
            if (success)
            {

                //如果productSysNoList为空，表示是对单个商品调价
                if (string.IsNullOrWhiteSpace(productSysNoList))
                {

                    //价格验证
                    Result result = ValidPrice(pdPriceHistoryList);
                    if (!result.Status)
                    {
                        jsonData.message = result.Message;
                        jsonData.success = false;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    //保存单个商品的调价
                    success = PdPriceHistoryBo.Instance.SavePdPriceHistory(pdPriceHistoryList.ToArray<PdPriceHistory>());
                }
                else
                {

                    //保存多个商品调价
                    Result result = PdPriceHistoryBo.Instance.SavePdPriceHistories(pdPriceHistoryList.ToArray(), pdProdcutSysNoList.ToArray());

                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, "批量调价失败：" + result.Status.ToString(), LogStatus.系统日志目标类型.未捕获异常, 0, null,
                                    string.Empty,
                                      0);
                    if (!result.Status)
                    {
                        jsonData = new
                        {
                            success = false,
                            message = result.Message
                        };

                        //jsonData.success = false;
                        //jsonData.message = result.Message;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }

                if (!success)
                {
                    jsonData.message = "保存失败！！";
                    jsonData.success = false;
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量修改分销商商品价格
        /// </summary>
        /// <param name="ProductSysNos">被选择商品组</param>
        /// <param name="DealerSysNo">分销商编号</param>
        /// <param name="Percentage">调整百分比</param>
        /// <returns></returns>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1005301)]
        public JsonResult UpdateDealerPrice(string ProductSysNos, int DealerSysNo, decimal Percentage)
        {
            Result result = new Result();

            //判断调整的价格是否在分销商上下限之间
            if (DealerSysNo >= 0)
            {
                var dealerleve = BLL.Distribution.DsDealerLevelBo.Instance.GetDealerLevelByDealerSysNo(DealerSysNo);
                if (Percentage < (100 - dealerleve.SalePriceLower) || Percentage > (100 + dealerleve.SalePriceUpper))
                {
                    result.Status = false;
                    result.Message = "请填写" + dealerleve.SalePriceLower + "到" + dealerleve.SalePriceUpper + "之间数字";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result.Status = false;
                result.Message = "分销商编号异常";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (ProductSysNos == "0")
            {
                ProductSysNos = null;
            }
            Percentage = Percentage / 100;

            //执行更新分销商价格
            int affectRows = BLL.Distribution.DsSpecialPriceBo.Instance.ProUpdateSpecialPrice(ProductSysNos, DealerSysNo, Percentage);
            if (affectRows >= 0)
            {
                result.Status = true;
                result.Message = "更新成功";
            }
            else
            {
                result.Status = false;
                result.Message = "更新失败";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 搜索类别
        /// 2017-06-09 罗熙 创建
        /// </summary>
        /// <returns></returns>
        public JsonResult Getlb(string lbSysNo)
        {
            //待完善

            string Condition = string.Format("where PdCategory.ParentSysNo = '{0}'", lbSysNo);
            return Json(Condition);
        }

        #region 商品管理辅助方法（私有方法）

        /// <summary>
        /// 将字符串转换成价格来源枚举
        /// </summary>
        /// <param name="value">被转换的字符串</param>
        /// <returns>返回：产品价格来源枚举数字</returns>
        /// <remarks>2013-06-28 邵斌 创建</remarks>
        private ProductStatus.产品价格来源[] ConvertStringToPriceSource(string value)
        {
            string[] showPriceType = value.Split(',');
            //转换要显示的价格类型为产品价格来源枚举
            ProductStatus.产品价格来源[] showSourceList = new ProductStatus.产品价格来源[showPriceType.Length];
            for (int i = 0; i < showPriceType.Length; i++)
            {
                showSourceList[i] = (ProductStatus.产品价格来源)(int.Parse(showPriceType[i]));
            }

            return showSourceList;
        }

        /// <summary>
        /// 验证价格列表中的价格，
        /// </summary>
        /// <param name="pdPriceHistoryList">新申请的价格列表</param>
        /// <returns>返回是否验证成功</returns>
        /// <remarks>2013-06-28 邵斌 创建</remarks>
        private Result ValidPrice(IList<CBPdPriceHistory> pdPriceHistoryList)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = 1
            };

            #region 检查会员等级价格

            //查找所有商品会员等级价格
            var queryLevelPrice = pdPriceHistoryList.Where(p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价).ToList();

            //必须有等级价格，并且等级价格要超过1个才有可比性
            if (queryLevelPrice.Count > 1)
            {
                //第一个对比价格
                CBPdPriceHistory tempPrice = queryLevelPrice[0];

                //检查会员等级价格
                for (int i = 1; i < queryLevelPrice.Count; i++)
                {
                    if (queryLevelPrice[i].ApplyPrice > tempPrice.ApplyPrice)
                    {
                        result.Message = ProductStatus.产品价格来源.会员等级价.ToString() + "设置不正确，价格必须从高到底设置";
                        result.Status = false;
                        result.StatusCode = -1;
                        return result;
                    }

                    //缓存对比价格
                    tempPrice = queryLevelPrice[i];
                }
            }

            #endregion

            return result;
        }

        #endregion

        #endregion

        #region 商品管理

        /// <summary>
        /// 汉字转拼音
        /// </summary>
        /// <param name="chinese">汉字</param>
        /// <returns>商品名称的汉语拼音</returns>
        /// <remarks>2013-09-28 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007001, PrivilegeCode.PD1007201)]
        public JsonResult ChineseToPinYin(string chinese)
        {
            var result = "";

            if (!string.IsNullOrWhiteSpace(chinese))
            {
                var charArray = chinese.ToArray<char>();
                foreach (var c in charArray)
                {
                    result += Hyt.Util.CHS2PinYin.Convert(c.ToString(), true) + " ";
                }
            }

            return Json(result.TrimEnd(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 订单来源

        /// <summary>
        /// 获取订单来源列表
        /// </summary>
        /// <returns>订单来源json数据</returns>
        /// <remarks>2013-06-27 朱家宏 创建</remarks>
        /// 
        [Privilege(PrivilegeCode.CM1005918)]
        [HttpGet]
        public ActionResult LoadOrderSourceList()
        {
            var list = new List<SelectListItem>();
            EnumUtil.ToListItem<OrderStatus.销售单来源>(ref list);
            return Json(list.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 订单状态

        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        /// <returns>订单状态json数据</returns>
        /// <remarks>2013-06-27 朱家宏 创建</remarks>
        /// 
        [Privilege(PrivilegeCode.CM1005919)]
        [HttpGet]
        public ActionResult LoadOrderStatusList()
        {
            var list = new List<SelectListItem>();
            EnumUtil.ToListItem<OrderStatus.销售单状态>(ref list);
            return Json(list.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 取件方式
        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <param name="wareHouseSysNo">仓库编号</param>
        /// <param name="handleDepartment">处理部门</param>
        /// <returns>取件方式json数据</returns>
        /// <remarks>2013-07-12 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005920)]
        [HttpGet]
        public ActionResult GetLgPickupType(int wareHouseSysNo, int handleDepartment)
        {
            IEnumerable<LgPickupType> warehouseFilter = new List<LgPickupType>();
            var isShop = handleDepartment == (int)RmaStatus.退换货处理部门.门店;
            var warehouseList = WhWarehouseBo.Instance.GetPickupTypeListByWarehouse(wareHouseSysNo);
            if (warehouseList != null && warehouseList.Count > 0)
            {
                List<int> support;
                if (isShop)
                {
                    //门店支持取件方式
                    support = new List<int> { PickupType.送货至门店 };

                }
                else
                {
                    //客服支持取件方式
                    support = new List<int>
                    {
                        PickupType.百城当日取件,
                        PickupType.普通取件,
                        PickupType.加急取件,
                        PickupType.定时取件,
                        PickupType.快递至仓库
                    };
                }
                warehouseFilter = warehouseList.Where(i => support.Contains(i.SysNo));
            }
            return Json(warehouseFilter, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RMA

        /// <summary>
        /// 获取RMA状态列表
        /// </summary>
        /// <returns>RMA状态json数据</returns>
        /// <remarks>2013-07-12 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005921)]
        [HttpGet]
        public ActionResult LoadRmaStatusList()
        {
            var list = new List<SelectListItem>();
            EnumUtil.ToListItem<RmaStatus.退换货状态>(ref list);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取RMA类型列表
        /// </summary>
        /// <returns>RMA类型json数据</returns>
        /// <remarks>2013-07-12 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005922)]
        [HttpGet]
        public ActionResult LoadRmaTypeList()
        {
            var list = new List<SelectListItem>();
            EnumUtil.ToListItem<RmaStatus.RMA类型>(ref list);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 退款方式
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="handleDepartment">处理部门</param>
        /// <param name="shopNo">门店编号</param>
        /// <returns>退款方式列表</returns>
        /// <remarks>2013-07-15 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.CM1005923)]
        public JsonResult LoadRefundType(int orderSysNo, int handleDepartment, int? shopNo = null)
        {
            var list = new List<SelectListItem>();
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            if (order != null)
            {
                if (order.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
                {//分销商升舱只能退款到预存款 2013/09/28 朱成果
                    list.Add(new SelectListItem
                    {
                        Text = RmaStatus.退换货退款方式.分销商预存.ToString(),
                        Value = ((int)RmaStatus.退换货退款方式.分销商预存).ToString()
                    });
                    //在线支付
                    list.Add(new SelectListItem
                    {
                        Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                        Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                    });
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                #region 其他处理
                if (handleDepartment == (int)RmaStatus.退换货处理部门.门店)
                {
                    //主收款单 
                    bool flg = false;
                    var main = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(order.SysNo);
                    if (main != null && shopNo.HasValue)
                    {
                        var sum = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(main.SysNo)
                                     .Where(m =>
                                            m.Status == (int)FinanceStatus.收款单明细状态.有效 &&
                                            m.ReceivablesSideType == (int)FinanceStatus.收款方类型.仓库 &&
                                            m.ReceivablesSideSysNo == shopNo.Value &&
                                            (m.PaymentTypeSysNo == PaymentType.现金 ||
                                             m.PaymentTypeSysNo == PaymentType.现金预付))
                                     .Sum(m => m.Amount);
                        if (sum >= order.CashPay) flg = true;
                    }
                    //门店退换货
                    if (flg)
                    {
                        //门店现付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.门店退款.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.门店退款).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                    else if (order.PayTypeSysNo == PaymentType.支付宝 || order.PayTypeSysNo == PaymentType.网银)
                    {
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.原路返回.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.原路返回).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                    else
                    {
                        //货到付款
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.至银行卡.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.至银行卡).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                }
                else
                {
                    //客服退换货
                    if (order.PayTypeSysNo == PaymentType.支付宝 || order.PayTypeSysNo == PaymentType.网银 || order.PayTypeSysNo == PaymentType.微信支付 || order.PayTypeSysNo == PaymentType.易宝支付)
                    {
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.原路返回.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.原路返回).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                    else if (order.PayTypeSysNo == PaymentType.余额支付)
                    {
                        //余额支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.账户余额.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.账户余额).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                    else
                    {
                        //门店现付或者货到付款
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.至银行卡.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.至银行卡).ToString()
                        });
                        //在线支付
                        list.Add(new SelectListItem
                        {
                            Text = RmaStatus.退换货退款方式.所退款作为换货.ToString(),
                            Value = ((int)RmaStatus.退换货退款方式.所退款作为换货).ToString()
                        });
                    }
                }
                #endregion
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 手机验证码
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>json</returns>
        /// <remarks>2013-10-14 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005924)]
        public JsonResult SendMobileValidation(string mobile)
        {
            const int expiredTime = 300;    //驗證碼過期時間，秒
            const int retryTime = 30;       //重新發送時間
            const int randomSize = 6;       //隨機碼長度

            var code = WebUtil.Number(randomSize, false);

            var r = new Result
            {
                Message = "手机验证码发送失败，稍候再试",
                Status = false,
                StatusCode = -1
            };

            if (!VHelper.ValidatorRule(new Rule_Mobile(mobile)).IsPass)
            {
                r.Message = "无效的手机号码";
            }
            else
            {
                try
                {
                    var lastValidation = CookieUtil.Get("mobileValidation_expired_" + mobile);
                    if (!string.IsNullOrEmpty(lastValidation))
                    {
                        var lastExpiredTime = DateTime.Parse(CookieUtil.Get("mobileValidation_expired_" + mobile));
                        var timeSpan = lastExpiredTime - DateTime.Now;

                        r.StatusCode = (int)timeSpan.TotalSeconds;
                        r.Message = "该手机验证码尚未过期，请稍候再重新发送";
                    }
                    else
                    {
                        var smsResult = BLL.Extras.SmsBO.Instance.发送手机验证短信(mobile, code);

                        r.Status = smsResult.Status == Extra.SMS.SmsResultStatus.Success;

                        if (r.Status)
                        {
                            CookieUtil.SetCookie("mobileValidation_" + mobile, code,
                                                 DateTime.Now.AddSeconds(expiredTime));

                            var expried = DateTime.Now.AddSeconds(retryTime);
                            CookieUtil.SetCookie("mobileValidation_expired_" + mobile, expried.ToString(),
                                                 DateTime.Now.AddSeconds(retryTime));

                            r.Message = "发送成功";
                            r.StatusCode = retryTime;
                        }
                    }
                }
                catch
                {
                    r.Status = false;
                }
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 短信码验证
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">短信码</param>
        /// <returns>T:通过 F：未通过</returns>
        /// <remarks>2013-10-15 朱家宏 创建</remarks>
        private static bool ValidateMobileCode(string mobile, string code)
        {
            var result = true;

            if (!string.IsNullOrWhiteSpace(code))
            {
                code = code.Replace(" ", "").Replace("　", "");
                var mobileValidationSession = CookieUtil.Get("mobileValidation_" + mobile);
                if (!string.IsNullOrEmpty(mobileValidationSession) && code != mobileValidationSession)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 清除短信验证
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>void!</returns>
        /// <remarks>2013-10-15 朱家宏 创建</remarks>
        private static void ClearMobileValidation(string mobile)
        {
            CookieUtil.SetCookie("mobileValidation_" + mobile, null, DateTime.Now);
            CookieUtil.SetCookie("mobileValidation_expired_" + mobile, null, DateTime.Now);
        }
        #endregion

        #region 收款科目关联
        /// <summary>
        /// 收款科目关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="paymentTypeSysNo">配送方式编号</param>
        /// <returns>收款科目关联</returns>
        /// <remarks>2013-11-11 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1004201, PrivilegeCode.SO1005101)]
        public JsonResult GetReceiptTitleAssociation(int warehouseSysNo, int paymentTypeSysNo)
        {
            var lst = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetFnReceiptTitleAssociation(warehouseSysNo, paymentTypeSysNo).Select(m => new SelectListItem()
            {
                Text = m.EasReceiptName,
                Value = m.EasReceiptCode,
                Selected = m.IsDefault == 1
            }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 流量分析报表

        /// <summary>
        /// 获取流量分析报表数据
        /// </summary>
        /// <param name="sourceType">数据源类型</param>
        /// <param name="dataType">数据采样类型</param>
        /// <param name="isMobilePlatform">是否是手机平台</param>
        /// <returns>数据列表</returns>
        /// <remarks>2013-11-11 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.RT101101)]
        public JsonResult GetTotalReportDataSource(string sourceType, string dataType, bool isMobilePlatform = false)
        {
            CBMkTrafficStatisticsPVAndIPMonthReport inTimeDataSource = new CBMkTrafficStatisticsPVAndIPMonthReport();

            if (sourceType.Trim().ToLower() == "total")
            {
                switch (dataType)
                {
                    case "1":
                        inTimeDataSource = MkTrafficStatisticsBo.Instance.GetPVAndIP7DayReport(isMobilePlatform);
                        break;
                    case "2":
                        inTimeDataSource = MkTrafficStatisticsBo.Instance.GetPVAndIP12HourReport(isMobilePlatform);
                        break;
                    case "3":
                        inTimeDataSource = MkTrafficStatisticsBo.Instance.GetPVAndIP10MinuteReport(isMobilePlatform);
                        break;
                    default:
                        inTimeDataSource = MkTrafficStatisticsBo.Instance.GetPVAndIPMothReport(DateTime.Now.Year, isMobilePlatform);
                        break;
                }
                inTimeDataSource = inTimeDataSource ?? new CBMkTrafficStatisticsPVAndIPMonthReport();
                return Json(inTimeDataSource);
            }
            else if (sourceType.Trim().ToLower() == "sample")
            {
                //简单报表数据
                return Json(MkTrafficStatisticsBo.Instance.GetSampleReportData());
            }
            else if (sourceType.Trim().ToLower() == "top10")
            {
                //Top10报表
                IList<CBMkTrafficStatisticsPagePVAndIPReport> top10Result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();
                switch (dataType)
                {
                    case "1":
                        top10Result = MkTrafficStatisticsBo.Instance.GetViewerSevenDayTotalTop10();
                        break;
                    case "2":
                        top10Result = MkTrafficStatisticsBo.Instance.GetLocationSevenDayTotalTop10();
                        break;

                    case "3":
                        top10Result = MkTrafficStatisticsBo.Instance.GetScreenSevenDayTotalTop10();
                        break;
                    case "4":
                        top10Result = MkTrafficStatisticsBo.Instance.GetBrowserSevenDayTotalTop10();
                        break;
                    case "5":
                        top10Result = MkTrafficStatisticsBo.Instance.GetProductSevenDayTotalTop10();
                        break;
                }

                return Json(top10Result);

            }

            return Json("");
        }


        /// <summary>
        /// 获取时间段内数据
        /// </summary>
        /// <param name="sourceType">数据源类型</param>
        /// <param name="dataType">数据采样类型</param>
        /// <param name="isMobilePlatform">是否是手机平台</param>
        /// <returns>数据列表</returns>
        /// <remarks>2013-11-11 邵斌 创建</remarks>
        public JsonResult GetRangeTimeDateSource(string sourceType, string dateType, bool isMobilePlatform = false)
        {
            if (sourceType.Trim().ToLower() == "total")
            {

            }
            return Json("");
        }

        #endregion

        #region App消息推送

        /// <summary>
        /// 创建推送服务
        /// </summary>
        /// <param name="model">新推送服务</param>
        /// <returns>返回创建结果</returns>
        /// <remarks>2014-01-16 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002102)]
        public JsonResult AddAppPushService(CBApPushService model)
        {
            Result result = new Result();
            try
            {
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
                model.Status = (int)AppStatus.App推送服务状态.待审;

                result.Status = BLL.AppContent.AppContentBo.Instance.CreateApPushService(model);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 修改推送内容（在审核之前都可以进行修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.AP1002102)]
        public JsonResult EditAppPushService(CBApPushService model)
        {

            Result result = new Result();

            if (model.SysNo > 0)
            {

                try
                {
                    var entity = BLL.AppContent.AppContentBo.Instance.GetApPushService(model.SysNo);

                    if (entity.Status != (int)AppStatus.App推送服务状态.待审)
                    {
                        result.Message = "当前[" + ((AppStatus.App推送服务状态)entity.Status).ToString() + "]状态下不能修改推送内容";
                        return Json(result);
                    }

                    entity.LastUpdateBy = CurrentUser.Base.SysNo;
                    entity.LastUpdateDate = DateTime.Now;

                    entity.Title = model.Title;
                    entity.Content = model.Content;
                    entity.Parameter = model.Parameter;
                    entity.CustomerSysNo = model.CustomerSysNo;
                    entity.CustomerAccount = model.CustomerAccount;
                    entity.CustomerName = model.CustomerName;
                    entity.ServiceType = model.ServiceType;
                    entity.AppType = model.AppType;

                    result.Status = BLL.AppContent.AppContentBo.Instance.UpdateApPushService(entity);
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                }

            }
            else
            {
                result.Message = "没有正确选择推送消息";
            }
            return Json(result);
        }

        /// <summary>
        /// 审核推送消息
        /// </summary>
        /// <param name="sysNo">消息系统编号</param>
        /// <returns>返回 true:审核成功 false:审核失败</returns>
        /// <remarks>2014-01-15 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002103)]
        public JsonResult AuditAppPushService(int sysNo)
        {
            Result result = new Result();

            try
            {
                //审核
                result.Status = BLL.AppContent.AppContentBo.Instance.UpdateApPushServiceStatus(sysNo,
                                                                                               AppStatus.App推送服务状态
                                                                                                        .已审核,
                                                                                               CurrentUser.Base
                                                                                                          .SysNo);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 作废推送消息
        /// </summary>
        /// <param name="sysNo">消息系统编号</param>
        /// <returns>返回 true:审核成功 false:审核失败</returns>
        /// <remarks>2014-01-15 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002103)]
        [HttpPost]
        public JsonResult InvalidAppPushService(int sysNo)
        {

            Result result = new Result();

            try
            {
                //审核
                result.Status = BLL.AppContent.AppContentBo.Instance.UpdateApPushServiceStatus(sysNo, AppStatus.App推送服务状态.作废,
                                                                                    CurrentUser.Base.SysNo);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="sysNo">消息系统编号</param>
        /// <returns>返回推送结果对象</returns>
        /// <remarks>2014-01-15 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002103)]
        [HttpPost]
        public JsonResult PushAppPushService(int sysNo)
        {

            //推送
            var model = BLL.AppContent.AppContentBo.Instance.GetApPushService(sysNo);
            Result result = new Result();
            if (model != null)
            {
                //判读状态是否是可发状态
                if (model.Status == (int)AppStatus.App推送服务状态.已审核)
                {
                    //push数据
                    var pushSuccess = PushInfo(model, string.IsNullOrWhiteSpace(model.CustomerAccount));        //如果用户账号为空表示是群发消息
                    result = PushReslutDecorators(pushSuccess);


                    try
                    {
                        //更具发送返回结果来更新推送消息状态
                        if (result.Status)
                        {
                            //推送成功，更新状态为已发送
                            result.Status = BLL.AppContent.AppContentBo.Instance.UpdateApPushServiceStatus(sysNo,
                                                                                                           AppStatus
                                                                                                               .App推送服务状态
                                                                                                               .已发送,
                                                                                                           CurrentUser
                                                                                                               .Base
                                                                                                               .SysNo);
                            //更具专题填写消息内容
                            if (result.Status)
                            {
                                result.Message = "发送成功";
                            }
                            else
                            {

                                result.Message = "发送失败";
                            }
                        }
                        else
                        {
                            //推送失败，更新状态为失败
                            BLL.AppContent.AppContentBo.Instance.UpdateApPushServiceStatus(sysNo,
                                                                                                           AppStatus
                                                                                                               .App推送服务状态
                                                                                                               .失败,
                                                                                                           CurrentUser
                                                                                                               .Base
                                                                                                               .SysNo);
                        }

                    }
                    catch (Exception e)
                    {
                        result.Status = false;
                        result.Message = e.Message;
                    }

                }
                else
                {
                    result.Message = "推送消息还没有被审核，请先审核！";
                }

            }

            return Json(result);
        }

        /// <summary>
        /// 推送接口
        /// </summary>
        /// <param name="model">要推送的消息</param>
        /// <param name="isGroup">是否群发消息</param>
        /// <returns>返回 true：推送成功 false：推送失败</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        private PushResult PushInfo(ApPushService model, bool isGroup)
        {
            JPushHelper pushBulider = new JPushHelper();
            PushResult pResult = new PushResult() { Code = -1, Message = "发送失败" };

            PlatformEnum plat = PlatformEnum.All;

            switch (model.AppType)
            {
                case (int)AppStatus.App推送App类型.Android:
                    plat = PlatformEnum.Android;
                    break;
                case (int)AppStatus.App推送App类型.Iphone:
                    plat = PlatformEnum.Ios;
                    break;
            }

            //执行群发消息
            if (isGroup)
            {
                return pushBulider.Send(model.SysNo, model.Title, plat, model.Content, model.ServiceType,
                                           model.Parameter);
            }

            switch (model.ServiceType)
            {
                case (int)AppStatus.App推送服务类型.个人消息:
                    pResult = pushBulider.SendToSingle(model.SysNo, model.Title, model.CustomerSysNo, plat, model.Content, model.ServiceType, "", 1, "推送管理");
                    break;
                //case (int)AppStatus.App推送服务类型.优惠券:
                //    return pushBulider.SendToSingle(model.SysNo,model.Title,model.CustomerSysNo,plat,model.Content,model.ServiceType,"",1, "推送管理");
                default:
                    //默认为广播推送
                    pResult = pushBulider.Send(model.SysNo, model.Title, plat, model.Content, model.ServiceType,
                                            model.Parameter);
                    break;
            }

            return pResult;
        }

        /// <summary>
        /// 推送结果信息装饰器
        /// </summary>
        /// <param name="pushResult">推送结果</param>
        /// <returns>返回Result类结果</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        private Result PushReslutDecorators(PushResult pushResult)
        {
            Result result = new Result();
            result.Status = pushResult.Code == 0;

            //发送成功则直接返回
            if (result.Status)
            {
                result.Message = "发送成功";
                return result;
            }

            //更具错误码进行错误包装
            switch (pushResult.Code)
            {
                case 1011:
                    result.Message = "发送失败：没有满足条件的推送目标，原因可能是该用户还没有安装或使用客户端登录商城APP";
                    break;
                default:
                    result.Message = "发送失败：" + pushResult.Message;
                    break;
            }
            return result;
        }

        #endregion

        #region 短信咨询

        /// <summary>
        /// 作废客户短信咨询
        /// </summary>
        /// <param name="sysNo">质询系统编号</param>
        /// <returns>是否操作成功</returns>
        /// <remarks>2014-02-20 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1004092)]
        public JsonResult InvalidCustomerSms(int sysNo)
        {
            return Json(CrSmsQuestionBo.Instance.UpdateStatus(sysNo, CustomerStatus.短信咨询状态.作废));
        }

        /// <summary>
        /// 回复用户短信提问
        /// </summary>
        /// <param name="sysNo">回复短信系统编号</param>
        /// <param name="answer">回复内容</param>
        /// <returns>返回回复结果</returns>
        /// <remarks>2014-02-20 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1004093)]
        public JsonResult AnswerCustomer(int sysNo, string answer)
        {
            var result = CrSmsQuestionBo.Instance.Answer(sysNo, CurrentUser.Base.SysNo, answer);
            return Json(result);
        }

        #endregion

        /// <summary>
        /// 获取商品所属分类
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult GetProductsCategories(string productSysNoList)
        {
            List<int> parameter = new List<int>();
            var sysNos = productSysNoList.Split(',');
            if (sysNos != null && sysNos.Length > 0)
            {
                parameter = (from sysNo in sysNos select TConvert.ToInt32(sysNo)).ToList();
                IList<int> result = BLL.Product.PdProductBo.Instance.GetProductsCategories(parameter);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 快速查询仓库
        /// </summary>
        /// <param name="autNumber">查询关键字</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageNo">页码</param>
        /// <param name="deliveryType">仓库配送方式</param>
        /// <param name="warehouseType">仓库类型( 仓库 = 10,门店 = 20)</param>
        /// <param name="isAll">是否获取所有仓库</param>
        /// <returns>仓库信息json</returns>
        /// <remarks>2016-5-27 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        public ActionResult QuickSearchWarehouse(string autNumber, int pageSize, int pageNo, int? deliveryType, int? warehouseType, bool isAll = false)
        {
            //需要服务器端传输的数据格式：“{'result':[{'id':'4048','text':'4808','name':'CHINA169-BJ'},{'id':'4048','text':'4808','name':'CHINA169-BJ'}],'total':'1'}”
            var condition = new WarehouseSearchCondition()
            {
                DeliveryType = deliveryType == 0 ? null : deliveryType,
                WarehouseName = autNumber,
                Status = WarehouseStatus.仓库状态.启用.GetHashCode(),
                WarehouseType = warehouseType
            };
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            int? userSysNo = CurrentUser.Base.SysNo;
            if (hasAllWarehouse || isAll)
            {
                userSysNo = null;//如果有所有仓库权限，不加where条件
            }
            var page = WhWarehouseBo.Instance.QuickSearch(condition, pageNo, pageSize, userSysNo);
            var data = page.TData;//根据配送方式获取可选仓库
            var results = data.Select(x => new { id = x.SysNo, text = x.WarehouseName, name = x.IsSelfSupport == (int)WarehouseStatus.是否自营.是 ? x.WarehouseName + "(自营)" : x.WarehouseName + "(非自营)" });
            var result = new { result = results, total = page.TotalItemCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




    }
}
