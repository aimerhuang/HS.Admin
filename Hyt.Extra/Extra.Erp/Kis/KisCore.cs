using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Extra.Erp.Eas.Authentication;
using Extra.Erp.Model;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Receiving;
using Extra.Erp.Model.Sale;
using Extra.Erp.Properties;
using Hyt.DataAccess.Base;
using Hyt.Infrastructure.Memory;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Serialization;
using Hyt.Model.Common;
using Extra.Erp.Model.BaseData;
using Extra.Erp.DataContract;
using Grand.Platform.Api.Contract.DataContract;
using Grand.Platform.Api.Contract.DataContract.Kis;
using Grand.Platform.Api.Contract.Kis.DataContract;

namespace Extra.Erp.Kis
{
    /// <summary>
    /// Kis接口处理
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    internal sealed class KisCore
    {
        #region Private
        private static EASLoginProxyService account = new EASLoginProxyService { Timeout = Settings.Default.Timeout };
        private static string _userName = Settings.Default.EasLogin_UserName;
        private const string dateFormater = "yyyy-MM-dd HH:mm:ss.fffffff";
        private const string xsi = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
        private const string xsd=" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";


        private static ErpConfig config=ErpConfigs.Instance.GetErpConfig();


        private static object log = new object();
        #endregion
        
        public static KisCore Instance
        {
            get { return new KisCore(); }
        }
        /// <summary>
        /// 初始化请求数据
        /// </summary>
        /// <param name="data">请求非公共的数据</param>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 创建</remarks>
        private static string InitializeRequestData(string data)
        {
           string requestStr = "{\"APP_Key\":\"" + config.AppKey + "\",\"APP_scode\":\"" + config.AppScode + "\"";

            if(data!="")
                requestStr+=","+data;

            return requestStr+="}";
        }


        /// <summary>
        /// 请求接口
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="data">请求数据</param>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 添加</remarks>
        private static Result<T> RequestApi<T>(string method, string data)
        {
            var result = new Result<T>(){ Status=false,StatusCode="00000"};
            string requestStr = data; //InitializeRequestData(data);
            string apiUrl = config.Gateway + method;
            string exception = "";
            string responseStr = Hyt.Util.MyHttp.PostJsonData(apiUrl, requestStr,ref exception);
            if (exception == "")
            {
                var _result = responseStr.ToObject<KisResult<T>>();
                if (_result != null)
                {
                    result.Message = _result.message;
                    result.Status = _result.success;
                    result.Data = _result.data;
                    result.StatusCode = _result.error_code.ToString();
                    //result.PageIndex = _result.pageIndex;
                    //result.PageSize = _result.pageSize;
                    //result.RecordCount = _result.recordCount;
                }
                else
                    result.Message = "接口无回执信息";
            }
            else
            {
                result.Message = exception;
                result.Status = false;
            }                         
                    
            return result;
        }

        /// <summary>
        /// 获取接口响应对象
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 添加</remarks>
        private static string GetApiResponse(string method, string data)
        {
            string requestStr = data; //InitializeRequestData(data);
            string apiUrl = config.Gateway + method;
            string exception = "";
            string responseStr = Hyt.Util.MyHttp.PostJsonData(apiUrl, requestStr, ref exception);
            return responseStr;
        }
        /// <summary>
        /// 在SoapHeader中添加SessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        /// <remarks>2014-7-3 杨浩 创建</remarks>
        private static XmlElement GetSessionHeader(string sessionId)
        {
            var doc = new XmlDocument();
            var element = doc.CreateElement("ns1", "SessionId", "http://login.webservice.bos.kingdee.com");
            element.InnerText = sessionId;
            return element;
        }

        /// <summary>
        /// 生成单据随机编号
        /// </summary>
        /// <returns>随机编号</returns>
        /// <remarks>2013-9-18 杨浩 创建</remarks>
        private static string Number
        {
            get { return DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000000); }
        }

        /// <summary>
        /// 沈阳小北、郑州新天地、西安海星对应的组织映射为出库单的销售组织、销售组对应的是沈阳、郑州、西安，
        /// 收款单的部门对应的也是沈阳、郑州、西安
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetOrganizeCode(string code)
        {
            var dic = new Dictionary<string, string>
            {
                {"3012060103", "3012060101"},
                {"3012030105", "3012030101"},
                {"3012020107", "3012020101"}
            };

            if (code != null && dic.ContainsKey(code))
                return dic[code];
            return code;
        }

        /// <summary>
        /// 权限登录
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>2013-9-18 杨浩 创建</remarks>
        private static WSContext ErpLogin()
        {
            var context = account.login(Settings.Default.EasLogin_UserName, Settings.Default.EasLogin_Password,
                Settings.Default.EasLogin_SlnName, Settings.Default.EasLogin_DcName,
                Settings.Default.EasLogin_Language, Settings.Default.EasLogin_DbType);

            return context;
        }

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="storageOrgNumber">组织结构编码</param>
        /// <param name="erpCode">erp商品编码</param>
        /// <param name="erpWarehouseSysNo">EAS仓库编号</param>
        /// <param name="warehouseSysNo"></param>
        /// <returns>eas库存</returns>
        /// <remarks>2013-9-18 杨浩 创建</remarks>
        public static Result<IList<Inventory>> WebInventory(string storageOrgNumber, string[] erpCode, string erpWarehouseSysNo, int warehouseSysNo)
        {
            try
            {
                              
                if (erpWarehouseSysNo.Contains("-"))               
                    erpWarehouseSysNo = erpWarehouseSysNo.Split('-')[0];
                
                var request = new Grand.Platform.Api.Contract.DataContract.WebInventoryRequest()
                {
                    FNumbers = string.Join(",",erpCode),
                    FStockID = erpWarehouseSysNo,
                };
                using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IErpService>())
                {
                    var response = service.Channel.WebInventory(request);
                    //var result = RequestApi<IList<KisInventory>>("/base/InvList", request.ToJson());
                    if ((!response.IsError && response.InventoryList == null) || (!response.IsError && response.InventoryList != null && response.InventoryList.Count<=0) || response.IsError)
                    {
                        return new Result<IList<Inventory>>
                        {
                            Data = null,
                            Status = false,
                            Message = string.Format("没有找到商品数据。仓库:{0},商品:{1}", erpWarehouseSysNo, string.Join(";", erpCode))
                        };
                    }


                    var data = (from item in response.InventoryList
                                select new Inventory
                                {
                                    #region
                                    //StorageOrgNumber = item.Element("StorageOrgNumber").Value,
                                    //StorageOrgName = item.Element("StorageOrgName").Value,
                                    //MeasureunitNumber = item.Element("MeasureunitNumber").Value
                                    #endregion
                                    MaterialNumber = item.FNumber,
                                    MaterialName = item.FName,
                                    WarehouseNumber = item.FStockID,
                                    WarehouseName = item.FStockName,
                                    Quantity =(int)item.FQty,

                                }).ToList();


                    return new Result<IList<Inventory>>
                    {
                        Status = !response.IsError,
                        StatusCode = response.ErrCode,
                        Message = response.ErrMsg,
                        Data = data
                    };

                }
            }
            catch (Exception e)
            {
                return new Result<IList<Inventory>>
                {
                    Data = null,
                    Status = false,
                    Message =
                        string.Format("{0}。仓库:{1},商品:{2}", e.Message, erpWarehouseSysNo, string.Join(";", erpCode)+",kis:"+e.Message)
                };
            }
        }

        /// <summary>
        /// 配送员 借货、还货
        /// </summary>
        /// <param name="model">明细</param>
        /// <param name="type">借货状态</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify"></param>
        /// <param name="dataMd5">数据校验标识</param>
        /// <param name="isAgain">是否重新导入</param>
        /// <param name="enableEas">是否启用Eas</param>
        /// <param name="isData">是否只取xml数据</param>
        /// <returns>Result</returns>
        /// <remarks>2013-9-18 杨浩 创建</remarks>
        public static Result<string> OtherIssueBillFacade(List<BorrowInfo> model, 借货状态 type, string description, string flowIdentify, string dataMd5 = null,
                                                          bool isAgain = false, bool enableEas = true, bool isData = false)
        {
            var name = 借货状态.借货 == type ? "配送员借货" : "配送员还货";
            var watch = new Stopwatch();
            watch.Start();

            var result = new Eas.OtherIssueBillFacade.Result();

            #region 数据初始
            var firstModel = model.FirstOrDefault();
            int warehouseSysNo = firstModel != null ? firstModel.WarehouseSysNo : 0; //仓库编号
            decimal voucherAmount = model.Sum(q => q.Amount);
            var datajson = new BorrowInfoWraper
            {
                Model = model,
                Type = type,
                Description = description
            }.ToJson();

            var billHead = new Model.Borrowing.BillHead
                {
                    number = "QTCK" + Number, //单据抬头编号
                    hasEffected = 0,
                    fiVouchered = 0,
                    isReversed = 0,
                    isInitBill = 0,
                    baseStatus = 4,
                    year = DateTime.Now.Year,
                    period = DateTime.Now.ToString("yyyyMM"),
                    totalQty = model.Sum(q => q.Quantity),
                    totalAmount = model.Sum(q => q.Amount),
                    totalStandardCost = 0,
                    totalActualCost = 0,
                    creator = _userName,
                    lastUpdateUser = _userName,
                    bizType = 512, //512：借出出库
                    billType = (type == 借货状态.借货) ? 108 : 109, //108	其他出库单, 109 其他入库单
                    storageOrgUnit = string.Empty, //由eas查询，hyt不导入
                    transactionType = (type == 借货状态.借货) ? "0001" : "0002",
                    createTime = DateTime.Now.ToString(dateFormater),
                    lastUpdateTime = DateTime.Now.ToString(dateFormater),
                    bizDate = DateTime.Now.ToString(dateFormater),
                    handler = _userName,
                    description = description,
                };

            var billEntries = model.Select((q, i) => new Model.Borrowing.entry
                {
                    isPresent = q.IsPresent,
                    baseStatus = 2,
                    seq = 1 + i,
                    qty =(type == 借货状态.借货)? q.Quantity:-q.Quantity,
                    baseQty =(type == 借货状态.借货)? q.Quantity:-q.Quantity,
                    material = q.ErpCode,
                    storageOrgUnit = string.Empty, //由eas查询，hyt不导入
                    warehouse = q.WarehouseNumber,
                    stocker = string.Empty, //仓管员由eas查询
                    storeType = "G", //G:普通
                    storeStatus = 1, //可用
                    amount = 0,//q.Amount,金额可以不传
                    remark = q.Remark

                }).ToList();

            var data = new OtherIssueBill
                {
                    billHead = billHead,
                    billEntries = billEntries,
                    thirdSysBillID =dataMd5 ?? Helper.MD5Encrypt(datajson),
                    checkDuplicate = "true"
                };

            //按照Eas格式，去掉xmlns和空格
            string xml = Helper.XmlSerialize<OtherIssueBill>(data)
                               .Replace(xsi, "")
                               .Replace(xsd, "")
                               .Replace("utf-16", "UTF-8")
                               .Trim();
            if (isData)
            {
                return new Result<string>
                    {
                        Data = xml
                    };
            }

            #endregion

            #region 调用外部Eas接口

            同步状态 status;
            if (enableEas && isAgain)
            {
                try
                {
                    var context= ErpLogin();
                    var client = new Eas.OtherIssueBillFacade.WSOtherIssueBillFacadeSrvProxyService
                        {
                            Timeout = Settings.Default.Timeout,
                            SessionId = new SoapUnknownHeader { Element = GetSessionHeader(context.sessionId) }
                        };
                    if (context.sessionId != null)
                        result = client.importData(xml);
                    status = result.status ? 同步状态.成功 : 同步状态.失败;
                }
                catch (Exception e)
                {
                    result.message = e.Message;
                    result.status = false;
                    status = 同步状态.失败;
                }
            }
            else
            {
                result.message = result.message = enableEas ? Model.EasConstant.EAS_WAIT : Model.EasConstant.EAS_MESSAGE_CLOSE;
                result.status = false;
                status = 同步状态.等待同步;
            }

            #endregion

            watch.Stop();
            
            var resultData = new Result<string>
            {
                Data = (result.data != null ? result.data.ToString() : ""),
                Status = result.status,
                StatusCode = result.statusCode,
                Message = result.message
            };
            //记录日志
            WriteLog(datajson, resultData, watch, name,接口类型.配送员借货还货,warehouseSysNo, description, flowIdentify,voucherAmount, status, isAgain);
            return resultData;
        }


        /// <summary>
        /// 调拨单导入
        /// </summary>
        /// <param name="isSave">是否保存为提交状态</param>
        /// <param name="isAgain">是否重新导入</param>
        /// <param name="enableEas">是否启用Kis</param>
        /// <param name="isData">是否只取xml数据</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public static Result<string> TransferStockIssueBillFacade(Hyt.Model.EasSyncLog easModel, bool isSave = false, bool isAgain = false, bool enableEas = true, bool isData = false)
        {
            var sale = easModel.Data.ToObject<TransferStockInfoWraper>();
            List<TransferStockInfo> model = sale.Model;
            调拨状态 type = sale.Type;
            string description = sale.Description;
            string flowIdentify = easModel.FlowIdentify;
            string dataMd5 = easModel.DataMd5;
            var name = "调拨单导入";

            var watch = new Stopwatch();
            watch.Start();
            var result = new Eas.SaleIssueBillFacade.Result();
            string billNo = "DBDJ" + easModel.FlowIdentify + "T" + sale.Description;

            #region 数据初始
            var firstModel = model.FirstOrDefault();
            int warehouseSysNo = firstModel != null ? firstModel.WarehouseSysNo : 0; //仓库编号
            decimal voucherAmount = 0;

            string fDate = DateTime.Now.ToString();//请求接口日期

            var requset = new AddICStockBillRequset()
            {
                FBillNo = billNo,//单据编号
                Fdate = firstModel.FDate,//入库日期
                FConsignee = _userName,//发货人
                FDeptID = firstModel.FDeptID,//部门编码
                FEmpID = firstModel.FEmpID,//业务员编码            
                FExplanation = sale.Description, //摘要
                FSCStockID = firstModel.FSCStockID,
                FDCStockID = firstModel.WarehouseNumber, //调出仓库
                FROB = "1",
                TranType = 41,
            };

            requset.BillItemList = model.Select((q, i) => new BillItem()
            {
                FItemID = q.ErpCode,
                FConsignAmount = 0,
                Fnote = q.FNote,
                Fauxqty = q.Quantity,
                FUnitID = "",//单位  
            }).ToList();

            string json = requset.ToJson();

            var datajson = new TransferStockInfoWraper
            {
                Model = model,
                Type = type,
                Description = description

            }.ToJson();

            if (isData)
            {
                return new Result<string>
                {
                    Data = json
                };
            }
            #endregion

            #region 调用外部Kis接口
            同步状态 status;
            if (enableEas && isAgain)
            {
                try
                {
                    //实现调用接口
                    status = 同步状态.等待同步;
                    var _result = GetApiResponse("/rest/AddICStockBill", json);
                    var response = Hyt.Util.Serialization.JsonUtil.ToObject<Grand.Platform.Api.Contract.DataContract.Kis.AddICStockBillResponse>(_result);
                    status = !response.IsError ? 同步状态.成功 : 同步状态.失败;
                    result.status = !response.IsError;
                    result.message = response.ErrMsg;
                    result.statusCode = response.ErrCode;
                    result.data = response.FBillNo;
                }
                catch (Exception e)
                {
                    result.message = e.Message;
                    result.status = false;
                    status = 同步状态.失败;
                }
            }
            else
            {
                result.message = enableEas ? Model.EasConstant.EAS_WAIT : Model.EasConstant.EAS_MESSAGE_CLOSE;
                result.status = false;
                status = 同步状态.等待同步;
            }

            #endregion

            watch.Stop();
            var resultData = new Result<string>
            {
                Data = (result.data != null ? result.data.ToString() : ""),
                Status = result.status,
                StatusCode = result.statusCode,
                Message = result.message
            };
            //记录日志
            int sysno = WriteLog(datajson, resultData, watch, name, 接口类型.调拨单据导入, warehouseSysNo, description, flowIdentify, voucherAmount, status, isAgain);
            easModel.SysNo = sysno == 0 ? easModel.SysNo : sysno;

            //临时记录 检查是否重复
            var content = " 订单编号：" + description + " 外部编号：" + billNo + " 请求时间:" + fDate +
                " 返回结果：" + (result.status ? result.data : result.message) + " 返回状态：" + result.status + "," + result.statusCode + " ElapseTime:" + watch.ElapsedMilliseconds;
            WriteLog(easModel.SysNo, content, fDate, isAgain);

            return resultData;

        }

        /// <summary>
        /// 采购入库、退货
        /// </summary>
        /// <param name="easModel">Kis数据</param> 
        /// <param name="isSave">是否保存为提交状态</param>
        /// <param name="isAgain">是否重新导入</param>
        /// <param name="enableEas">是否启用Kis</param>
        /// <param name="isData">是否只取xml数据</param>
        /// <returns>Result</returns>
        /// <remarks>2017-12-29 杨浩 创建</remarks>
        public static Result<string> PurchaseIssueBillFacade(Hyt.Model.EasSyncLog easModel, bool isSave = false, bool isAgain = false, bool enableEas = true, bool isData = false)
        {
            var sale = easModel.Data.ToObject<PurchaseInfoWraper>();
            List<PurchaseInfo> model = sale.Model;
            采购状态 type = sale.Type;

            if (easModel.FlowIdentify == null && 采购状态.入库 == type)
            {
                var stockInInfo = Hyt.DataAccess.Warehouse.IInStockDao.Instance.GetWhStockIn(int.Parse(sale.Description));
                easModel.FlowIdentify = stockInInfo.SourceSysNO.ToString();
            }

            string description = sale.Description;
            string flowIdentify = easModel.FlowIdentify;
            string dataMd5 = easModel.DataMd5;
            var name = 采购状态.入库 == type ? "采购入库" : "采购退货";

            var watch = new Stopwatch();
            watch.Start();
            var result = new Eas.SaleIssueBillFacade.Result();


            #region 数据初始
            var firstModel = model.FirstOrDefault();
            int warehouseSysNo = firstModel != null ? firstModel.WarehouseSysNo : 0; //仓库编号
            decimal voucherAmount = model.Sum(q => q.Amount);
            if (采购状态.退货 == type) voucherAmount = -voucherAmount;

            string billNo = "CGRK" + easModel.FlowIdentify + "T" + sale.Description;


            string fDate = DateTime.Now.ToString();//请求接口日期

            var purchaseInfo = Hyt.DataAccess.Purchase.IPrPurchaseDao.Instance.GetPrPurchaseInfo(int.Parse(easModel.FlowIdentify));

            if (purchaseInfo != null)
            {
                string _supplyID = Hyt.DataAccess.Procurement.IPmBaseDataDao.Instance.GetManufacturerCode(purchaseInfo.ManufacturerSysNo);
                if (!string.IsNullOrWhiteSpace(_supplyID))
                    firstModel.FSupplyID = _supplyID;

                string _deptID = Hyt.DataAccess.Basic.IOrganizationDao.Instance.GetCode(purchaseInfo.DepartmentSysNo);
                if (!string.IsNullOrWhiteSpace(_deptID))
                    firstModel.FDeptID = _deptID;

                string _empId = Hyt.DataAccess.Sys.ISyKingdeeUserDao.Instance.GetKingdeeUserCode(purchaseInfo.SyUserSysNo);
                if (!string.IsNullOrWhiteSpace(_empId))
                    firstModel.FEmpID = _empId;
            }


            string pOOOrderBillNo = "";
            if (采购状态.退货 == type)
                pOOOrderBillNo = Hyt.DataAccess.Sys.IEasDao.Instance.GetVoucherNoByTlowIdentify((int)接口类型.采购入库退货, sale.PurchaseOrderSysNo);


            var requset = new AddPoOrderRequest()
            {
                POOrderBillNo = pOOOrderBillNo,
                SynchronizeCount = firstModel.SynchronizeCount,
                FBillNo = billNo,//单据编号
                FDate = firstModel.FDate,//入库日期
                FConsignee = _userName,//发货人
                FDeptID = firstModel.FDeptID,//部门编码
                FEmpID = firstModel.FEmpID,//业务员编码
                FSupplyID = firstModel.FSupplyID,//供应商编号
                FExplanation = sale.Description, //摘要
                FMangerID = firstModel.FEmpID,
                FDCStockID = firstModel.WarehouseNumber,
                FROB = 采购状态.入库 == type ? "1" : "-1",
                FSettleDate = DateTime.Parse(firstModel.SettleDate),
            };

            requset.PoOrderEntryList = model.Select((q, i) => new PoOrderItem()
            {
                PurchaseQty = q.PurchaseQty,
                FItemID = q.ErpCode,
                FAmount = q.Amount,
                FAllAmount = q.Amount,
                FAuxPrice = q.Price,
                FDate = q.FDate,
                FCess = 0,
                FDescount = 0,
                FNote = q.FNote,
                FPlanMode = 0,
                FQty = q.Quantity,
                FTaxAmount = 0,
                FTaxRate = 0,
                FUnitID = 0,//单位  

            }).ToList();

            //var saveRequest = requset;

            //saveRequest.FSupplyID = supplyID.ToString();
            //saveRequest.FDeptID = deptID.ToString();
            //saveRequest.FEmpID = empID.ToString();


            string json = requset.ToJson();

            var datajson = new PurchaseInfoWraper
            {
                Model = model,
                Type = type,
                Description = description,
                PurchaseOrderSysNo = sale.PurchaseOrderSysNo,

            }.ToJson();

            if (isData)
            {
                return new Result<string>
                {
                    Data = json
                };
            }
            #endregion

            #region 调用外部Kis接口
            同步状态 status;
            if (enableEas && isAgain)
            {
                try
                {
                    //实现调用接口
                    status = 同步状态.等待同步;
                    var _result = GetApiResponse("/rest/AddPoOrder", json);
                    var response = Hyt.Util.Serialization.JsonUtil.ToObject<Grand.Platform.Api.Contract.Kis.DataContract.AddPoOrderResponse>(_result);
                    status = !response.IsError ? 同步状态.成功 : 同步状态.失败;
                    result.status = !response.IsError;
                    result.message = response.ErrMsg;
                    result.statusCode = response.ErrCode;
                    result.data = response.FBillNo;
                }
                catch (Exception e)
                {
                    result.message = e.Message;
                    result.status = false;
                    status = 同步状态.失败;
                }
            }
            else
            {
                result.message = enableEas ? Model.EasConstant.EAS_WAIT : Model.EasConstant.EAS_MESSAGE_CLOSE;
                result.status = false;
                status = 同步状态.等待同步;
            }

            #endregion

            watch.Stop();
            var resultData = new Result<string>
            {
                Data = (result.data != null ? result.data.ToString() : ""),
                Status = result.status,
                StatusCode = result.statusCode,
                Message = result.message
            };
            //记录日志
            int sysno = WriteLog(datajson, resultData, watch, name, 接口类型.采购入库退货, warehouseSysNo, description, flowIdentify, voucherAmount, status, isAgain);
            easModel.SysNo = sysno == 0 ? easModel.SysNo : sysno;

            //临时记录 检查是否重复
            var content = " 订单编号：" + description + " 外部编号：" + billNo + " 请求时间:" + fDate +
                " 返回结果：" + (result.status ? result.data : result.message) + " 返回状态：" + result.status + "," + result.statusCode + " ElapseTime:" + watch.ElapsedMilliseconds;
            WriteLog(easModel.SysNo, content, fDate, isAgain);
            return resultData;
        }

        /// <summary>
        /// 销售出库、退货
        /// </summary>
        /// <param name="easModel">Kis数据</param> 
        /// <param name="isSave">是否保存为提交状态</param>
        /// <param name="isAgain">是否重新导入</param>
        /// <param name="enableEas">是否启用Kis</param>
        /// <param name="isData">是否只取xml数据</param>
        /// <returns>Result</returns>
        /// <remarks>2013-9-22 杨浩 创建</remarks>
        public static Result<string> SaleIssueBillFacade(Hyt.Model.EasSyncLog easModel,bool isSave = false, bool isAgain = false, bool enableEas = true, bool isData = false)
        {
            var sale = easModel.Data.ToObject<SaleInfoWraper>();
           
            List<SaleInfo> model = sale.Model;
            出库状态 type=sale.Type;
            string customer = sale.Customer;
            string description = sale.Description;
            string flowIdentify = easModel.FlowIdentify;
            string dataMd5 = easModel.DataMd5;

       
            var name = 出库状态.出库 == type ? "销售出库" : "销售退货";
            var watch = new Stopwatch();
            watch.Start();
            var result = new Eas.SaleIssueBillFacade.Result();

            #region 数据初始
            var firstModel = model.FirstOrDefault();
            int warehouseSysNo = firstModel != null ? firstModel.WarehouseSysNo : 0; //仓库编号
            decimal voucherAmount = model.Sum(q => q.Amount);
            var organizationCode = firstModel != null ? firstModel.OrganizationCode : string.Empty; //组织架构
            if (出库状态.退货 == type) voucherAmount = -voucherAmount;


            if (firstModel.WarehouseNumber.Contains("-"))
                firstModel.WarehouseNumber = firstModel.WarehouseNumber.Split('-')[0];


           var  saleInfo=model.FirstOrDefault();

           string billNo = "XSCK"+(saleInfo.ItemID??0)+easModel.FlowIdentify;

           var request=new  StockOutAddRequset()
           {
                FBillNo = billNo, 
                FEntryID = "0",
                Fdate = DateTime.Now.ToString("yyyy-MM-dd"),
                FConsignee = _userName,
                FDeptID ="",
                FEmpID = organizationCode,
                FExplanation = "",
                FFetchAdd = "",
                FFManagerID = "",
                FSaleStyle = "线上电商平台",
                FSManagerID = "",
                FCustID = string.IsNullOrWhiteSpace(saleInfo.DeliveryCustomer) ? config.FCustID : saleInfo.DeliveryCustomer,// "01.001.003",
                FDCStockID = firstModel.WarehouseNumber,
                FROB =  出库状态.出库 == type?"1":"-1",
           };
           
            request.item = model.Select((q, i) => new JsonItem()
            {
                FItemID = q.ErpCode,
                Fauxqty = q.Quantity.ToString(),
                FItemName = "",
                FDCStockID = q.WarehouseNumber.Contains("-") ? q.WarehouseNumber.Split('-')[0] : q.WarehouseNumber,
                FUnitID =config.FUnitID,// "ping",
                FConsignPrice = decimal.Round(q.Amount / q.Quantity, 2).ToString("0.00"),
                FConsignAmount = q.Amount.ToString("0.00"),
                FDiscountAmount = q.DiscountAmount.ToString("0.00"),

            }).ToList();

            string json = request.ToJson();

            var datajson = new SaleInfoWraper
            {
                Model = model,
                Type = type,
                Customer = customer,
                Description = description

            }.ToJson();
          
            if (isData)
            {
                return new Result<string>
                {
                    Data = json
                };
            }

            #endregion

            #region 调用外部Kis接口
            同步状态 status;
            if (enableEas && isAgain)
            {
                try
                {                
                    using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IErpService>())
                    {
                        var response=service.Channel.StockOutAdd(request);                    
                        status = !response.IsError ? 同步状态.成功 : 同步状态.失败;
                        result.status = !response.IsError;
                        result.message = response.ErrMsg;
                        result.statusCode = response.ErrCode;
                        result.data = response.FBillNo; //_result.Data != null && _result.Data.Count > 0 ? _result.Data.First().OutFBillNo : "";
                    }
                    
                }
                catch (Exception e)
                {
                    result.message = e.Message;
                    result.status = false;
                    status = 同步状态.失败;
                }
            }
            else
            {
                result.message = enableEas ? Model.EasConstant.EAS_WAIT : Model.EasConstant.EAS_MESSAGE_CLOSE;
                result.status = false;
                status = 同步状态.等待同步;
            }

            #endregion

            watch.Stop();

            var resultData = new Result<string>
                {
                    Data = (result.data != null ?result.data.ToString() :""),
                    Status = result.status,
                    StatusCode = result.statusCode,
                    Message = result.message
                };
            //记录日志
            int sysno= WriteLog(datajson, resultData, watch, name, 接口类型.销售出库退货,warehouseSysNo, description, flowIdentify,voucherAmount, status, isAgain);
            easModel.SysNo = sysno == 0 ? easModel.SysNo : sysno;
            //临时记录 检查是否重复
            var content = " 订单编号：" + description + " 外部编号：" + request.FBillNo+ " 请求时间:" + request.Fdate +
                " 返回结果：" +(result.status?result.data:result.message) + " 返回状态：" + result.status + "," + result.statusCode + " ElapseTime:" + watch.ElapsedMilliseconds;
            WriteLog(easModel.SysNo, content, request.Fdate, isAgain);
           
            return resultData;
        }

        /// <summary>
        /// 导入收款单据
        /// </summary>
        /// <param name="easSysNo"></param>
        /// <param name="model"></param>
        /// <param name="receivingType">收款单类型(5:商品收款单;10:服务收款单)</param>
        /// <param name="customer">送货客户(商城客户:3003999997(如果为升舱订单 客服编号就要对应))</param>
        /// <param name="flowIdentify"></param>
        /// <param name="dataMd5"></param>
        /// <param name="isAgain">是否开始同步</param>
        /// <param name="description"></param>
        /// <param name="enableEas">是否启用Eas</param>
        /// <param name="isData">是否只取xml数据</param>
        /// <returns>Result</returns>
        /// <remarks>2013-9-25 杨浩 创建</remarks>
        public static Result<string> ReceivingBillFacade(int easSysNo,List<ReceivingInfo> model, 收款单类型 receivingType, string customer, string description, string flowIdentify, string dataMd5 = null, bool isAgain = false, bool enableEas = true, bool isData = false)
        {
            var name = (receivingType == 收款单类型.退销售回款 ? "导入付款单" : "导入收款单");
            var watch = new Stopwatch();
            watch.Start();
            var result = new Eas.ReceivingBillFacade.Result();
            string sessionId = "";
            #region 数据初始
           
            var datajson = new ReceivingInfoWraper
            {
                Model = model,
                Description = description,
                Customer = customer,
                ReceivingType = receivingType
            }.ToJson();
            var firstModel = model.FirstOrDefault();
            var organizationCode = firstModel != null ? firstModel.OrganizationCode : string.Empty; //组织架构
            var payeeAccount = firstModel != null ? firstModel.PayeeAccount : string.Empty; //收款科目
            int warehouseSysNo = firstModel != null ? firstModel.WarehouseSysNo : 0; //仓库编号
            decimal voucherAmount = model.Sum(q => q.Amount);
            if (receivingType == 收款单类型.退销售回款) voucherAmount = -voucherAmount;
           
            var billHead = new Model.Receiving.BillHead
                {
                    number = "SK" + Number,
                    company = "30", //品牌管理有限公司
                    creator = _userName,
                    createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    lastUpdateUser = _userName,
                    lastUpdateTime = DateTime.Now.ToString(dateFormater),
                    bizdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    handler = _userName,
                    description = description,
                    auditor = _userName,
                    //recbilltype = 100, //销售回款
                    //paymenttype = "002", //赊销
                    fundtype = (receivingType == 收款单类型.商品收款单 || receivingType == 收款单类型.退销售回款) ? "100" : "101", //现金=100,银行=101,其他=102
                    rectype = (receivingType == 收款单类型.商品收款单) ? "100" : ((receivingType == 收款单类型.退销售回款) ? "102" : "999"), //收款类型 销售回款(应收系统)=100,,其他(出纳系统)=999
                    currency = "RMB",
                    actrecamt = 0, //0表头自动计算
                    actreclocamt = 0, //0表头自动计算
                    exchangerate = 1,
                    payeeaccount = payeeAccount, //收款科目
                    payeeaccountbank = firstModel != null ? firstModel.PayeeAccountBank : string.Empty, //收款账户，支付宝，网银需传入
                    settlementtype = firstModel != null ? firstModel.SettlementType : string.Empty, //结算方式 需hyt对应(对应)01:现金
                    biztype = string.Empty,
                    payeebank = string.Empty,
                    settlementnumber = string.Empty,
                    adminorgunit = GetOrganizeCode(organizationCode), //组织架构
                    person = firstModel != null ? firstModel.WarehouseNumber : string.Empty,//仓库编码
                    oppaccount = string.Empty,
                    costcenter = string.Empty,
                    oppbgitemnumber = string.Empty,
                    payertype = "00001", //客户
                    payernumber = customer, //商城客户
                    payerbank = string.Empty,
                    payeraccountbank = string.Empty,
                    sourcebillid = string.Empty,
                    sourcefunction = string.Empty,
                    auditdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    billstatus = "10", //保存=10,已提交=11,已审批=12,已收款=14,已付款=15,审批中=6,已审核=8
                    sourcebilltype = string.Empty,
                    sourcesystype = (receivingType == 收款单类型.商品收款单 || receivingType == 收款单类型.退销售回款) ? "100" : "103",//100:应收系统,103:出纳系统
                    sourcetype = (receivingType == 收款单类型.商品收款单 || receivingType == 收款单类型.退销售回款) ? "100" : "103",//100:应收系统,103:出纳系统
                    baseStatus = 14 //保存=10,已提交=11(提交即审核) 14=收款状态
                };

            var billEntries = model.Select((q, i) => new Model.Receiving.entry
                {
                    seq = 1 + i,
                    recbilltype = (receivingType == 收款单类型.商品收款单) ? 100 : 0, //100:销售回款(应收系统需要)
                    amount = (receivingType == 收款单类型.退销售回款) ? -q.Amount : q.Amount,
                    localamt = (receivingType == 收款单类型.退销售回款) ? -q.Amount : q.Amount,
                    rebate = 0,
                    rebatelocamt = 0,
                    actualamt = (receivingType == 收款单类型.退销售回款) ? -q.Amount : q.Amount,
                    actuallocamt = (receivingType == 收款单类型.退销售回款) ? -q.Amount : q.Amount,
                    remark = q.Remark,
                    corebilltype = (receivingType == 收款单类型.商品收款单) ? 102 : 0, //102:销售出库单(应收系统需要)
                    corebillnumber = string.Empty,
                    corebillentryseq = string.Empty,
                    project = string.Empty,
                    tracknumber = string.Empty,
                    contractnum = string.Empty,
                    contractentryseq = string.Empty,
                    lockamt = 0, //结算状态为收款金额
                    //unlockamt = q.Amount, //未结算状态为收款金额
                    amountvc = 0,
                    localamtvc = 0,
                    //unvcamount = q.Amount, //未结算状态为收款金额
                    //unvclocamount = q.Amount, //未结算状态为收款金额
                    matchedamount = 0,
                    matchedamountloc = 0,
                    oppaccount = string.Empty,
                    oppbgitemname = string.Empty,
                    ck =(payeeAccount==EasConstant.PayeeAccount?EasConstant.HytWharehouse: q.WarehouseNumber),//仓库Eas编号

                }).ToList();

            var data = new ReceivingBill
                {
                    billHead = billHead,
                    billEntries = billEntries,
                    thirdSysBillID = dataMd5??Helper.MD5Encrypt(datajson),
                    checkDuplicate="true"
                };

            //按照Eas格式，去掉xmlns和空格
            string xml = Helper.XmlSerialize(data)
                               .Replace(xsi, "")
                               .Replace(xsd, "")
                               .Replace("utf-16", "UTF-8")
                               .Trim();
            if (isData)
            {
                return new Result<string>
                {
                    Data = xml
                };
            }

            #endregion

            #region 调用外部Eas接口

            #region 检查基础资料是否完备
           
            string message = EasConstant.Information+":";
            bool isFull = true;
            if (string.IsNullOrEmpty(billHead.payeeaccount))
            {
                message += " 收款科目为空";
                isFull = false;
            }
            //仅在收款科目为1002开头时才是必录项
            if (!string.IsNullOrEmpty(billHead.payeeaccount)&&billHead.payeeaccount.StartsWith("1002"))
            {
                if (string.IsNullOrEmpty(billHead.payeeaccountbank))
                {
                    message += " 收款账户为空";
                    isFull = false;
                }
            }
            if (string.IsNullOrEmpty(billHead.adminorgunit))
            {
                message += " 部门为空";
                isFull = false;
            }
            if (string.IsNullOrEmpty(billHead.person))
            {
                message += " 人员为空";
                isFull = false;
            }
            #endregion
            同步状态 status;
            if (enableEas && isAgain && isFull)
            {
                try
                {
                    var context=ErpLogin();
                    sessionId = context.sessionId;
                    var client = new Eas.ReceivingBillFacade.WSReceivingBillFacadeSrvProxyService
                    {
                        Timeout = Settings.Default.Timeout,
                        SessionId = new SoapUnknownHeader { Element = GetSessionHeader(sessionId) }
                    };
                    if (sessionId != null)
                        result = client.importData(xml);
                    status = result.status ? 同步状态.成功 : 同步状态.失败;
                }
                catch (Exception e)
                {
                    result.message = e.Message;
                    result.status = false;
                    status = 同步状态.失败;
                }
            }
            else
            {
                if (enableEas)
                {
                    result.message = isFull ? Model.EasConstant.EAS_WAIT : message;
                }
                else
                    result.message = Model.EasConstant.EAS_MESSAGE_CLOSE;

                result.status = false;
                status = 同步状态.等待同步;
            }

            #endregion

            watch.Stop();

            var resultData = new Result<string>
                {
                    Data = (result.data != null ? result.data.ToString() : ""), //返回单据号
                    Status = result.status,
                    StatusCode = result.statusCode,
                    Message = result.message
                };
            //记录日志
            var sysno= WriteLog(datajson, resultData, watch, name, 接口类型.收款单据导入,warehouseSysNo, description, flowIdentify,voucherAmount, status, isAgain);
            easSysNo = sysno == 0 ? easSysNo : sysno;
            //临时记录 检查是否重复
            var content = " 订单编号：" + description + " 会话ID：" + sessionId + " 外部编号：" + data.thirdSysBillID + " 请求时间:" + billHead.lastUpdateTime +
                 " 返回结果：" + (result.status ? result.data : result.message) + " 返回状态：" + result.status + "," + result.statusCode + " ElapseTime:" + watch.ElapsedMilliseconds;
            WriteLog(easSysNo, content, billHead.createtime, isAgain);
            return resultData;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="datajson">同步数据</param>
        /// <param name="result">返回结果</param>
        /// <param name="watch">同步耗时</param>
        /// <param name="name">接口名称</param>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="remarks">记录商城原单据</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <param name="voucherAmount">单据金额</param>
        /// <param name="status">同步状态</param>
        /// <param name="isAgain">是否开始同步</param> 
        /// <remarks>2013-10-24 杨浩 创建</remarks>
        private static int WriteLog(string datajson, Result<string> result, Stopwatch watch, string name,
            接口类型 interfaceType, int warehouseSysNo, string remarks, string flowIdentify,decimal voucherAmount, 同步状态 status, bool isAgain)
        {
            //是否为自动同步
            if (isAgain)
                return 0;
            //是否为自营
            if (!IsSelfSupport(warehouseSysNo))
                return 0;
            int elapsedMilliseconds = 0;
            string message = string.Empty;
            var dataMd5 = Helper.MD5Encrypt(datajson);
            var isImport = Hyt.DataAccess.Sys.IEasDao.Instance.IsImport(dataMd5);
            if (isImport) return 0;

            elapsedMilliseconds = (int) watch.ElapsedMilliseconds;
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
                InterfaceType = (int) interfaceType,
                Message = message,
                Name = name,
                Status = (int) status,
                StatusCode = result.StatusCode,
                CreatedBy = Hyt.Model.SystemPredefined.User.SystemUser,
                Remarks = remarks,
                FlowIdentify = flowIdentify,
                FlowType = (接口类型.配送员借货还货 == interfaceType ? 20 : 10),
                LastupdateDate = DateTime.Now,
                WarehouseSysNo = warehouseSysNo,
                VoucherAmount = voucherAmount,
                LastsyncTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
            };

           return Hyt.DataAccess.Sys.IEasDao.Instance.EasSyncLogCreate(log);
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
            return w.IsSelfSupport == (int)WarehouseStatus.是否自营.是;
        }

        /// <summary>
        /// 临时记录Eas请求日志
        /// </summary>
        /// <param name="easSysNo">Eas编号</param>
        /// <param name="xmlContent">请求内容</param>
        /// <param name="dateTime">请求时间</param>
        public static void WriteLog(int easSysNo, string xmlContent,string dateTime,bool isAgain)
        {
            if (isAgain)
            {
                xmlContent = xmlContent.Length > 3000 ? xmlContent.Substring(0, 3000) : xmlContent;
                IDbContext context =
                    new DbContext().ConnectionString(ConfigurationManager.AppSettings["OracleConnectionString"],
                        new SqlServerProvider());
                context.Sql(
                    "insert into EasSyncRequestLog(EasSyncLogSysNo,Content,CreatedDate) values(@EasSyncLogSysNo,@Content,@CreatedDate)")
                    .Parameter("EasSyncLogSysNo", easSysNo)
                    .Parameter("Content", xmlContent)
                    // "CurrentThread:" + Thread.CurrentThread.ManagedThreadId + ". " +
                    .Parameter("CreatedDate", DateTime.Parse(dateTime))
                    .Execute();
            }
        }



        /// <summary>
        /// 获取kis中的单位
        /// </summary>
        /// <remarks>2016-11-22 杨浩 创建</remarks>
        public static Result<IList<KisUnit>> GetAllUnit()
        {         
            var request = new GetAllUnitRequest()
            {
                FAcctDB="",
                FNumber="",
                FName="",
                FUnitGroupName="",
                F="",
            };

            string data=request.ToJson();

            var result = RequestApi<IList<KisUnit>>("/base/unitList",data);

            return result;
        }

        /// <summary>
        /// 销售单查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-12-12 杨浩 创建</remarks>
        public static Result<List<SaleSearchResponse>> SaleSearch(SaleSearchRequest request)
        {
            var result = RequestApi<List<SaleSearchResponse>>("/base/SEOrderList", request.ToJson());

            return result;
        }
    }
}
