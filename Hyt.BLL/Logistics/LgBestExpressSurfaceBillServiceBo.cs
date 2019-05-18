using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Express;
using Hyt.BLL.Web;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.WorkflowStatus;
using Pisen.Framework.Service.Proxy;

using SoOrderBo = Hyt.BLL.Order.SoOrderBo;
using WhWarehouseBo = Hyt.BLL.Warehouse.WhWarehouseBo;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 百世汇通电子面单服务操作类
    /// </summary>
    /// <remarks>2015-10-13 谭显锋 创建</remarks>
    public class LgBestExpressSurfaceBillServiceBo : BOBase<LgBestExpressSurfaceBillServiceBo>
    {
        ///// <summary>
        ///// 申请电子面单
        ///// </summary>
        ///// <param name="stockOuts">出库单实体集合</param>
        ///// <returns>响应实体</returns>
        ///// <remarks>2015-10-13 谭显锋 创建</remarks>
        //public GetSurfaceBillNoResponse GetSurfaceBillNo(List<WhStockOut> stockOuts)
        //{
        //    var billList = new List<SurfaceBill>();
        //    LgDeliveryCompanyAccount entity = null;
        //    foreach (var whStockOut in stockOuts)
        //    {
        //        entity = ElectronicsSurfaceBo.Instance.GetEntityByWarehouseSysNo(whStockOut.WarehouseSysNo);//物流公司账号表实体
        //        if (entity == null)
        //        {
        //            throw new HytException("当前仓库的电子面单账号信息不存在，请先在'基础管理>电子面单账号管理'中关联仓库!");
        //        }
        //        var order = SoOrderBo.Instance.GetEntity(whStockOut.OrderSysNO);
        //        var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
        //        var receivearea = BsAreaBo.Instance.GetAreaDetail(receiveAddress.AreaSysNo);
        //        var senderAddress = WhWarehouseBo.Instance.GetWarehouse(whStockOut.WarehouseSysNo);
        //        var senderArea = BsAreaBo.Instance.GetAreaDetail(senderAddress.AreaSysNo);
        //        billList.Add(new SurfaceBill()
        //        {
        //            CustomerOrderCode = whStockOut.SysNo.ToString(),
        //            Node = whStockOut.Remarks,
        //            // ParcelWeight = 2.1,
        //            ProjectCode = entity.AccountId,
        //            Recipient = new Recipient()
        //            {
        //                Province = receivearea.Province,
        //                City = receivearea.City,
        //                District = receivearea.Region,
        //                Name = receiveAddress.Name,
        //                PhoneNumber = GetPhoneNumber(receiveAddress.PhoneNumber, receiveAddress.MobilePhoneNumber),
        //                PostalCode = receiveAddress.ZipCode,
        //                ShippingAddress = receiveAddress.StreetAddress
        //            },
        //            Sender = new Sender()
        //            {
        //                SenderAddress = senderAddress.StreetAddress,
        //                SenderCity = senderArea.City,
        //                SenderDistrict = senderArea.Region,
        //                SenderMobile = senderAddress.Phone,
        //                SenderName = senderAddress.WarehouseName,
        //                SenderPhone = senderAddress.Phone,
        //                SenderPostalcode = "",
        //                SenderProvince = senderArea.Province
        //            }
        //        });
        //    }
        //    var request = new GetSurfaceBillNoRequest()
        //    {
        //        ApplyBill = new ApplyBill()
        //        {
        //            SurfaceBill = billList
        //        },
        //        PartnerId = entity.AccountId,
        //        PartnerKey = entity.AccountSecretKey
        //    };
        //    //调用服务获取电子面单信息
        //    GetSurfaceBillNoResponse response = new GetSurfaceBillNoResponse();
        //    using (var client = new ServiceProxy<IBestExpressSurfaceBillService>())
        //    {
        //        response = client.Channel.GetSurfaceBillNo(request);
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// 修改运单（确认、取消、修改信息），不支持批量
        ///// </summary>
        ///// <param name="whStockOut">出库单实体</param>
        ///// <param name="actionType">电子面单操作类型(取消，确认)</param>
        ///// <returns>响应实体</returns>
        ///// <remarks>2015-10-13 谭显锋 创建</remarks>
        //public UpdateBillResponse UpdateBill(WhStockOut whStockOut, LogisticsStatus.电子面单操作类型 actionType)
        //{
        //    UpdateBillResponse response = new UpdateBillResponse();
        //    string type = "CONFIRM";
        //    if (actionType == LogisticsStatus.电子面单操作类型.取消订单)
        //    {
        //        type = "CANCEL";
        //    }
        //    var entity = ElectronicsSurfaceBo.Instance.GetEntityByWarehouseSysNo(whStockOut.WarehouseSysNo);//物流公司账号表实体
        //    if (entity == null)
        //    {
        //        throw new HytException("当前仓库的电子面单账号信息不存在，请先在'基础管理>电子面单账号管理'中关联仓库!");
        //    }
        //    var electronModel = LgElectronicWayBillBo.Instance.GetElectronicWayBillByStockOutSysNo(whStockOut.SysNo);
        //    var order = SoOrderBo.Instance.GetEntity(whStockOut.OrderSysNO);
        //    var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
        //    var receivearea = BsAreaBo.Instance.GetAreaDetail(receiveAddress.AreaSysNo);
        //    var senderAddress = WhWarehouseBo.Instance.GetWarehouse(whStockOut.WarehouseSysNo);
        //    var senderArea = BsAreaBo.Instance.GetAreaDetail(senderAddress.AreaSysNo);
        //    var request = new UpdateBillRequest()
        //    {
        //        ExpressOrder = new ExpressOrder()
        //        {
        //            ActionType = type,
        //            //ArrivalTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
        //            BusinessType = "BEX",//快递
        //            CustomerOrderCode = whStockOut.SysNo.ToString(),
        //            //DeliveryRequire = "上班时间送",
        //            DeliveryTypeId = "1",//0-自提；1-送货
        //            Note = whStockOut.Remarks,
        //            //OrderTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
        //            PackageAmount = "1",
        //            Packages = new Packages()
        //            {
        //                PackageList = new List<Package>()
        //                    {
        //                        new Package()
        //                        {
        //                            LogisticsProviderCode = "SF",
        //                            PackageAmount = "1",
        //                            PackageNumber =whStockOut.SysNo.ToString(),
        //                            //PackageVolume = "0.3",
        //                            //PackageWeight = 0.1,
        //                            ShippingOrderNo= electronModel.WayBillNo
        //                        }
        //                    }
        //            },
        //            ProjectCode = entity.AccountId,
        //            Recipient = new Recipient()
        //            {
        //                Province = receivearea.Province,
        //                City = receivearea.City,
        //                District = receivearea.Region,
        //                Name = receiveAddress.Name,
        //                PhoneNumber = GetPhoneNumber(receiveAddress.PhoneNumber, receiveAddress.MobilePhoneNumber),
        //                MobileNumber = receiveAddress.MobilePhoneNumber,
        //                PostalCode = receiveAddress.ZipCode,
        //                ShippingAddress = receiveAddress.StreetAddress
        //            },
        //            Sender = new Sender()
        //            {
        //                SenderAddress = senderAddress.StreetAddress,
        //                SenderCity = senderArea.City,
        //                SenderDistrict = senderArea.Region,
        //                SenderMobile = senderAddress.Phone,
        //                SenderName = senderAddress.WarehouseName,
        //                SenderPhone = senderAddress.Phone,
        //                SenderPostalcode = "",
        //                SenderProvince = senderArea.Province
        //            }
        //        },
        //        PartnerId = entity.AccountId,
        //        PartnerKey = entity.AccountSecretKey
        //    };
        //    using (var client = new ServiceProxy<IBestExpressSurfaceBillService>())
        //    {
        //        response = client.Channel.UpdateBill(request);
        //    }
        //    return response;
        //}

        /// <summary>
        /// 获取收件人电话(根据百世要求,手机号座机号都传入同一个字段)
        /// </summary>
        /// <param name="phoneNumber">座机号</param>
        /// <param name="mobilePhoneNumer">手机号</param>
        /// <returns>收件人电话</returns>
        /// <remarks>2015-11-5 谭显锋 创建</remarks>
        private string GetPhoneNumber(string phoneNumber, string mobilePhoneNumer)
        {
            if (String.IsNullOrEmpty(mobilePhoneNumer) && !String.IsNullOrEmpty(phoneNumber))
            {
                return phoneNumber;
            }
            if (String.IsNullOrEmpty(phoneNumber) && !String.IsNullOrEmpty(mobilePhoneNumer))
            {
                return mobilePhoneNumer;
            }
            if (String.IsNullOrEmpty(phoneNumber) && String.IsNullOrEmpty(mobilePhoneNumer))
            {
                return "";
            }
            return mobilePhoneNumer + "/" + phoneNumber;
        }
    }
}
