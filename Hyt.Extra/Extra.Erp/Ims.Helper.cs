using Extra.Erp.Eas.Authentication;
using Extra.Erp.Model;
using Extra.Erp.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using Hyt.Util.Serialization;
using System.Diagnostics;
using System.IO;
using System.Web.Services.Protocols;
using System.Xml.Linq;
//using Extra.Erp.IMSEntity;
using Hyt.Util;
using System.Collections.Specialized;
using System.Net;
namespace Extra.Erp
{
    /// <summary>
    /// ImsHelper
    /// </summary>
    /// <remarks>2016-1-4 王江 创建</remarks>
    public class ImsHelper
    {

        private const string xsi = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
        private const string xsd = " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
        private const string defaultJiamengCode = "000000";//没有启用IMS的仓库编码


        public static ImsHelper Instance
        {
            get { return new ImsHelper(); }
        }
        ///// <summary>
        ///// 销售出库单、销售退货入库单同步接口
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <remarks>2016-1-4 王江 创建</remarks>
        //public TpcStockInOutResponse SyncTpcStockInOut(TpcStockInOutRequest request)
        //{
        //    var json = request.ToJson();
        //    var param = new NameValueCollection()
        //    {
        //        {"json",json},
        //        {"sign",EncryptionUtil.EncryptWithMd5(json + Extra.Erp.Properties.Settings.Default.IMS_TPC_SysncSignature)}
        //    };
        //    var client = new WebClient();
        //    var bytes = client.UploadValues(Extra.Erp.Properties.Settings.Default.Extra_Ims_SyncTpcStockInOutProxyService, param);
        //    return JsonUtil.ToObject<TpcStockInOutResponse>(Encoding.UTF8.GetString(bytes));
        //}

        ///// <summary>
        ///// 收款单同步接口
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <remarks>2016-1-4 王江 创建</remarks>
        //public TpcReceivePaymentResponse SyncTpcReceivePayment(TpcReceivePaymentRequest request)
        //{
        //    var json = request.ToJson();
        //    var param = new NameValueCollection()
        //    {
        //        {"json",json},
        //        {"sign",EncryptionUtil.EncryptWithMd5(json + Extra.Erp.Properties.Settings.Default.IMS_TPC_SysncSignature)}
        //    };
        //    var client = new WebClient();
        //    var bytes = client.UploadValues(Extra.Erp.Properties.Settings.Default.Extra_Ims_SyncTpcReceivePaymentProxyService, param);
        //    return JsonUtil.ToObject<TpcReceivePaymentResponse>(Encoding.UTF8.GetString(bytes));
        //}

        ///// <summary>
        ///// IMS库存查询接口
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <remarks>2016-1-11 王江 创建</remarks>
        //public TpcInventoryQueryResponse GetInventorySearch(TpcInventoryQueryRequest request)
        //{
        //    var json = request.ToJson();
        //    var param = new NameValueCollection()
        //    {
        //        {"json",json},
        //        {"sign",EncryptionUtil.EncryptWithMd5(json + Extra.Erp.Properties.Settings.Default.IMS_TPC_SysncSignature)}
        //    };
        //    var client = new WebClient();
        //    var bytes = client.UploadValues(Extra.Erp.Properties.Settings.Default.Extra_Ims_SyncGetInventoryProxyService, param);
        //    return JsonUtil.ToObject<TpcInventoryQueryResponse>(Encoding.UTF8.GetString(bytes));
        //}

        ///// <summary>
        ///// 从IMS系统获取加盟商库存
        ///// </summary>
        ///// <param name="enterpriseID">加盟商客户编码</param>
        ///// <param name="erpCode">商品编码</param>
        ///// <param name="wareHouse">仓库信息</param>
        ///// <returns></returns>
        ///// <remarks>2016-01-05 朱成果 创建</remarks>
        //public static Result<List<Inventory>> GetInventory(string[] erpCode, Hyt.Model.WhWarehouse wareHouse)
        //{
        //    Result<List<Inventory>> r=new Result<List<Inventory>>(){
        //         Status=false
        //    };
        //    if(erpCode!=null&&wareHouse!=null&&wareHouse.IsSelfSupport==(int)Hyt.Model.WorkflowStatus.WarehouseStatus.是否自营.否)
        //    {
        //        string enterpriseID=GetJiaMengCustomer(wareHouse.SysNo);//加盟商EAS 客户编码
        //        if (!string.IsNullOrEmpty(enterpriseID) && !string.IsNullOrEmpty(wareHouse.ErpCode) && wareHouse.ErpCode.Trim() != defaultJiamengCode)
        //        {
        //            TpcInventoryQueryRequest request=new TpcInventoryQueryRequest();
        //            request.enterpriseID=enterpriseID;//加盟商客户编码
        //            request.warehouseIDs=new List<string>();
        //            request.warehouseIDs.Add(wareHouse.ErpCode.Trim());
        //            request.productIDs=erpCode.ToList();
        //            try 
        //            {
        //                var response= ImsHelper.Instance.GetInventorySearch(request);
        //                if(response!=null)
        //                {
        //                    r.Status=response.Status;
        //                    r.Message=response.Message;
        //                    if(response.Data!=null)
        //                    {
        //                        r.Data=new List<Inventory>();
        //                        response.Data.ForEach((m)=>{
        //                            r.Data.Add(
        //                                new Inventory()
        //                                {
        //                                    Quantity = (int)m.Quantity,
        //                                    MaterialNumber = m.ProductCode,
        //                                    WarehouseSysNo = wareHouse.SysNo,
        //                                    WarehouseName = wareHouse.WarehouseName,
        //                                    WarehouseSysName = wareHouse.WarehouseName,
        //                                    WarehouseNumber = m.WareHouseCode
        //                                }

        //                            );
                            
        //                        });
        //                    }
        //                }
        //            }
        //            catch(Exception ex)
        //            {
        //                r.Status=false;
        //                r.Message=ex.Message;
        //            }
        //        }
        //    }
        //    return r;
        //}
        ///// <summary>
        ///// 获取加盟商客户信息
        ///// </summary>
        ///// <param name="wareHouseSysNo">仓库信息</param>
        ///// <returns></returns>
        ///// <remarks>2016-01-05 朱成果 创建</remarks>
        //private static string GetJiaMengCustomer(int wareHouseSysNo)
        //{
        //    string enterpriseID = string.Empty;
        //    var dw= Hyt.DataAccess.Distribution.IDsDealerWharehouse.Instance.GetByWarehousSysNo(wareHouseSysNo);
        //    if(dw!=null)
        //    {
        //       var dd= Hyt.DataAccess.Distribution.IDsDealerDao.Instance.GetDsDealer(dw.DealerSysNo);
        //       if(dd!=null)
        //        {
        //            enterpriseID = dd.ErpCode;
        //        }
        //    }
        //    return enterpriseID;
        //}

        ///// <summary>
        ///// IMS 出库单和退换货单同步
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="status"></param>
        ///// <returns>返回单据号码</returns>
        ///// <remarks>2016-01-05 朱成果 创建</remarks>
        //public static Result<string> SyncTpcStockInOut(TpcStockInOutData datainfo, ImportStatus status)
        //{
        //    string xml = Helper.XmlSerialize(datainfo)
        //                           .Replace(xsi, "")
        //                           .Replace(xsd, "")
        //                           .Replace("utf-16", "UTF-8")
        //                           .Trim();
        //    if (status.isData)
        //    {
        //        return new Result<string>
        //        {
        //            Data = xml
        //        };
        //    }
        //    if (System.Configuration.ConfigurationManager.AppSettings["EnableIMSSync"] != "true")//接口关闭
        //    {
        //        return new Result<string>
        //        {
                  
        //            Message = Model.EasConstant.EAS_MESSAGE_CLOSE,
        //            Status = false
        //        };
        //    }
        //    if (string.IsNullOrEmpty(datainfo.RequestData.Warehouse) || datainfo.RequestData.Warehouse.Trim() == defaultJiamengCode)//基础资料没有维护
        //    {
        //        return new Result<string>
        //        {
        //            Message = Model.EasConstant.Information,
        //            Status = false
        //        };
        //    }
        //    var datajson = datainfo.ToJson();
        //    var md5 = Helper.MD5Encrypt(datajson);
        //    Extra.Erp.Entity.Result result = new Extra.Erp.Entity.Result() { Status = false };
        //    同步状态 sendstatus = 同步状态.等待同步;
        //    var watch = new System.Diagnostics.Stopwatch();
        //    watch.Start();
        //    if (status.enableEas && status.isAgain)
        //    {
        //        try
        //        {
        //            datainfo.RequestData.Guid = md5;
        //            var response = ImsHelper.Instance.SyncTpcStockInOut(datainfo.RequestData);
        //            if (response != null)
        //            {
        //                result.Status = response.Status;
        //                result.Message = response.Message;
        //                result.data = response.Code ?? "";
        //                sendstatus = response.Status ? 同步状态.成功 : 同步状态.失败;
        //            }
        //            else
        //            {
        //                sendstatus = 同步状态.失败;
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            result.Message = e.Message;
        //            result.Status = false;
        //            sendstatus = 同步状态.失败;
        //        }
        //    }
        //    else
        //    {
        //        result.Message = status.enableEas ? Model.EasConstant.EAS_WAIT : Model.EasConstant.EAS_MESSAGE_CLOSE;
        //        result.Status = false;
        //        sendstatus = 同步状态.等待同步;
        //    }
        //    watch.Stop();
        //    var resultData = new Result<string>
        //    {
        //        Data = (result.data != null ? result.data.ToString() : ""),
        //        Status = result.Status,
        //        StatusCode = result.statusCode,
        //        Message = result.Message
        //    };
        //    decimal voucherAmount = datainfo.RequestData.ItemList.Sum(m => m.Quantity * m.UnitPrice - m.OriginalCurrency);
        //    if (datainfo.RequestData.InOutType == "22")
        //    {
        //        voucherAmount = 0 - voucherAmount;
        //    }
        //    var sysno = WriteQueue(datajson, md5, resultData, watch, 接口类型.IMS出入库, datainfo.WarehouseNo, datainfo.RequestData.Remarks, datainfo.Identifier, voucherAmount, sendstatus, status.isAgain);
        //    return resultData;

        //}
        #region 将日志写入数据库
        /// <summary>
        /// 将日志写入到同步队列
        /// </summary>
        /// <param name="datajson">同步数据</param>
        /// <param name="dataMd5">校验信息</param>
        /// <param name="result">返回结果</param>
        /// <param name="watch">同步耗时</param>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="remarks">记录当日达原单据</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <param name="voucherAmount">单据金额</param>
        /// <param name="status">同步状态</param>
        /// <param name="isAgain">是否开始同步</param> 
        /// <remarks>2015-09-29 朱成果 创建</remarks>
        private static int WriteQueue(string datajson, string dataMd5, Result<string> result, Stopwatch watch, 接口类型 interfaceType, int warehouseSysNo, string remarks, string flowIdentify, decimal voucherAmount, 同步状态 status, bool isAgain)
        {
            //是否为自动同步
            if (isAgain)
                return 0;

            int elapsedMilliseconds = 0;
            string message = string.Empty;
            if (string.IsNullOrEmpty(dataMd5))
            {
                dataMd5 = Helper.MD5Encrypt(datajson);
            }
            var isImport = Hyt.DataAccess.Sys.IEasDao.Instance.IsImport(dataMd5);
            if (isImport) return 0;

            elapsedMilliseconds = (int)watch.ElapsedMilliseconds;
            message = result.Message != null
                ? (result.Message.Length > 500 ? result.Message.Substring(0, 500) : result.Message)
                : string.Empty;

            var log = new Hyt.Model.EasSyncLog()
            {
                VoucherNo = result.Data,
                CreatedDate = DateTime.Now,
                Data = datajson,
                DataMd5 = dataMd5,
                ElapsedTime = elapsedMilliseconds,
                InterfaceType = (int)interfaceType,
                Message = message,
                Name = interfaceType.ToString(),
                Status = (int)status,
                StatusCode = result.StatusCode,
                CreatedBy = Hyt.Model.SystemPredefined.User.SystemUser,
                Remarks = remarks,
                FlowIdentify = flowIdentify,
                FlowType = (接口类型.配送员借货还货 == interfaceType ? 20 : 10),
                LastupdateDate = DateTime.Now,
                WarehouseSysNo = warehouseSysNo,
                VoucherAmount = voucherAmount
            };
            var r = Hyt.DataAccess.Sys.IEasDao.Instance.EasSyncLogCreate(log);
            return r;
        }

        /// <summary>
        /// 是否为自营店
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <returns></returns>
        private static bool IsSelfSupport(int warehouseSysNo)
        {
            var w = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWarehouseEntity(warehouseSysNo);
            if (w == null)
                return false;
            return w.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.是否自营.是;
        }


        #endregion

      
    }
}
