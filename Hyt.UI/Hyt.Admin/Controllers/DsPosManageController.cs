using Com.Alipay.Business;
using Extra.AliF2F;
using Hyt.Admin;
using Hyt.BLL.CRM;
using Hyt.BLL.Distribution;
using Hyt.BLL.Pos;
using Hyt.BLL.Warehouse;
using Hyt.Model;
using Hyt.Model.Pos;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.Util.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Dzm.Admin.Controllers
{
    [CustomActionFilter(false)]
    public class DsPosManageController : BaseController
    {
        //
        // GET: /DsPosManage/

        public ActionResult Index()
        {
            ViewBag.dsList = new List<Model.Pos.DsPosManage>();
            if (CurrentUser.IsBindDealer)
            {
                string dsSysNo = CurrentUser.Dealer.SysNo.ToString();
                List<Model.Pos.CBDsPosManage> dsList = DsPosManageBo.Instance.GetEntityListByDsSysNo(Convert.ToInt32(dsSysNo));
                ViewBag.dsList = dsList;
                ViewBag.dsSysNo = dsSysNo;
            }
            else
            {
                string dsSysNo = "0";
                List<Model.Pos.CBDsPosManage> dsList = DsPosManageBo.Instance.GetEntityListByDsSysNo(Convert.ToInt32(dsSysNo));
                ViewBag.dsList = dsList;
                ViewBag.dsSysNo = dsSysNo;
            }

           
            return View();
        }
        #region 收银机获取数据

        #region 离线数据加载
         /// <summary>
        /// 获取商品档案数据
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="ProList">mac地址</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckProductOrUpdate(string Key, string Mac,string PosName,string ProList)
        {
            List<CBPdProductDetail> detailList = JsonUtil.ToObject<List<CBPdProductDetail>>(ProList);
             PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                DsDealerWharehouse warehouse = DsDealerWharehouseBo.Instance.GetByDsUserSysNo(nowMod.pos_DsSysNo);
                if (warehouse != null)
                {
                    result.Status = true;
                    List<Model.CBPdProductDetail> tempList = DsPosManageBo.Instance.GetPosProductDetailList(detailList, warehouse.WarehouseSysNo);
                    string jsonData = "";

                    int totalCount = tempList.Count / 100;
                    for (int i = 0; i < totalCount; i++)
                    {

                        jsonData += JsonUtil.ToJson2(tempList.GetRange(i * 100, 100));
                    }
                    if (tempList.Count > (totalCount) * 100)
                    {

                        jsonData += JsonUtil.ToJson2(tempList.GetRange(totalCount * 100, tempList.Count - (totalCount) * 100));
                    }
                    //jsonData = jsonData.Replace("][", ",");
                    result.Message = jsonData;
                }
                else
                {
                    result.Status = false;
                    result.Message = "未给当前经销商设置仓库。请设置操作后再进行该操作";
                }
                
            }
            return Content(JsonUtil.ToJson(result));
           
        }

        /// <summary>
        /// 业务销售人员的
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <param name="DsUsers">销售人员集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadCheckPosSeller(string Key, string Mac, string PosName, string DsUsers)
        {
            List<DsUser> detailList = JsonUtil.ToObject<List<DsUser>>(DsUsers);
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                result.Status = true;
                List<DsUser> sysUserList = Hyt.BLL.Distribution.DsUserBo.Instance.GetListByDealerSysNo(nowMod.pos_DsSysNo);
                foreach (DsUser mod in sysUserList)
                {
                    var tempDsUser = detailList.Find(p => p.Account == mod.Account);
                    if (tempDsUser != null)
                    {
                        mod.SysNo = tempDsUser.SysNo;
                        detailList.Remove(tempDsUser);
                    }
                    else
                    {
                        mod.SysNo = 0;
                    }
                }

                //jsonData = jsonData.Replace("][", ",");
                result.Message = JsonUtil.ToJson2(sysUserList);
            }
            return Content(JsonUtil.ToJson(result));

        }

        /// <summary>
        /// 加载会员数据
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadPosMemberDataList(string Key, string Mac, string PosName,string MemberDataList)
        {
            ///获取会员卡数据
            List<DsMembershipCard> memberCardList = JsonUtil.ToObject<List<DsMembershipCard>>(MemberDataList);
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                result.Status = true;
                List<DsMembershipCard> sysMemberCardList = DsMemberBo.Instance.GetMembershipCardList(nowMod.pos_DsSysNo);
                List<DsMembershipCard> clientMemberCardList = new List<DsMembershipCard>();
                foreach(DsMembershipCard card in sysMemberCardList)
                {
                    DsMembershipCard tempMod = memberCardList.Find(p => p.CardNumber == card.CardNumber);
                    if (tempMod != null)
                    {
                        if (tempMod.UserLevel != card.UserLevel)
                        {
                            card.SysNo = tempMod.SysNo;
                            clientMemberCardList.Add(card);
                        }
                    }
                    else
                    {
                        card.SysNo = 0;
                        clientMemberCardList.Add(card);
                    }
                    
                }
                result.Message = JsonUtil.ToJson2(sysMemberCardList);
            }
            return Content(JsonUtil.ToJson(result));

        }

        /// <summary>
        /// 会员等级数据
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadCheckPosMemberLevel(string Key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                result.Status = true;
                List<DsMembershioLevel> sysLevelList = DsMemberBo.Instance.GetDsMembershioLevelList(0);

                result.Message = JsonUtil.ToJson2(sysLevelList);
            }
            return Content(JsonUtil.ToJson(result));

        }

        /// <summary>
        /// 会员系统点数换金额数据
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadCheckPosPaymentToPoint(string Key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                result.Status = true;
                DsPaymentToPointConfig sysPaymentToPoint = DsMemberBo.Instance.GetDsPaymentToPointConfig(0);

                result.Message = JsonUtil.ToJson2(sysPaymentToPoint);
            }
            return Content(JsonUtil.ToJson(result));

        }
        #endregion

        #region 收银机门店配置

        /// <summary>
        /// 收银机通过key获取所有收银机配置
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Config(string Key, string Mac)
        {
            return Json(DsPosManageBo.Instance.GetEntityListByPosKey(Key));
        }
        /// <summary>
        /// 绑定收银机机其和管理的关系
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BindConfig(string Key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName)
                {
                    nowMod = mod;
                    break;
                }
            }
            nowMod.pos_MacData = Mac;
            nowMod.pos_BindTime = DateTime.Now;
            DsPosManageBo.Instance.Update(nowMod);
            return Json(nowMod);
        }

        /// <summary>
        /// 初始化验证收银机配置信息
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-1-25 杨云奕
        /// </remarks>
        [HttpPost]
        public JsonResult CheckConfig(string Key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod != null)
            {
                if (nowMod.pos_MacData == Mac)
                {
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                }
            }
            else
            {
                result.Status = false;
            }
            return Json(result);
        }

        #endregion

        #region 收银员操作
        /// <summary>
        /// 收银员登录操作
        /// </summary>
        /// <param name="accout">登录账号</param>
        /// <param name="pass">登录密码</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DsUserLogion(string accout, string pass, string key, string Mac, string PosName)
        {
            PosName = Microsoft.JScript.GlobalObject.unescape(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                DsUser dsUser = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(accout);
               
                //DsUser user = Hyt.BLL.Distribution.DsUserBo.Instance.GetListByDealerSysNo(nowMod.pos_DsSysNo, accout, pass);
                if (dsUser == null)
                {
                    result.Status = false;
                    result.Message = "账号有误";
                }
                else
                {
                    if (Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(pass, dsUser.Password))
                    {
                        result.Status = true;
                        result.Message = Hyt.Util.Serialization.JsonUtil.ToJson2(dsUser);
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "密码有误";
                    }
                }
            }
            return Json(result);
        }
        /// <summary>
        /// 通过条码说去商品信息
        /// </summary>
        /// <param name="code">商品条码</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        public JsonResult GetProductByBarCode(string code, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                CBPdProductDetail detail = DsPosManageBo.Instance.GetDealerProductByBarCode(code, nowMod.pos_DsSysNo);
                if (detail == null)
                {
                    result.Status = false;
                    result.Message = "当前条码商品或已下架，无法查询。";
                }
                else
                {
                    result.Status = true;
                    result.Message = Hyt.Util.Serialization.JsonUtil.ToJson2(detail);
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 添加Pos销售订单数据到服务器
        /// </summary>
        /// <param name="posOrder">销售订单</param>
        /// <param name="items">销售商品列表</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">MAC地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        public JsonResult InnerPosOrderToService(DBDsPosOrder posOrder, string items, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        DsDealerWharehouse warehouse = DsDealerWharehouseBo.Instance.GetByDsUserSysNo(nowMod.pos_DsSysNo);
                        posOrder.items = JsonUtil.ToObject<List<DsPosOrderItem>>(items);
                        posOrder.DsPosSysNo = nowMod.SysNo;
                        posOrder.DsSysNo = nowMod.pos_DsSysNo;
                        posOrder.PayTime = DateTime.Now;
                        int sysNo = DsPosOrderBo.Instance.Insert(posOrder);
                        foreach (DsPosOrderItem item in posOrder.items)
                        {
                            item.pSysNo = sysNo;
                            DsPosOrderBo.Instance.InsertItem(item);

                            PdProductStock stock = PdProductStockBo.Instance.GetEntityByWP(warehouse.WarehouseSysNo, item.ProSysNo);
                            if (stock==null)
                            {
                                stock = new PdProductStock() {
                                    WarehouseSysNo = warehouse.WarehouseSysNo,
                                    PdProductSysNo = item.ProSysNo,
                                    StockQuantity = item.WareNum,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateDate= DateTime.Now,
                                    Barcode = item.ProBarCode
                                };
                                PdProductStockBo.Instance.SavePdProductStock(stock);
                            }
                            PdProductStockBo.Instance.UpdateStockQuantity(warehouse.WarehouseSysNo, item.ProSysNo, item.ProNum);
                        }
                        if(!string.IsNullOrEmpty(posOrder.CardNumber))
                        {
                            ///获取订单总金额
                            CBCrCustomer customer = CrCustomerBo.Instance.GetCrCustomer(posOrder.CardNumber);
                            if (customer != null)
                            {
                                ///获取订单总金额
                                Hyt.BLL.LevelPoint.PointBo.Instance.UpdateAvailablePoint(
                                      customer,
                                       0,
                                       Model.WorkflowStatus.CustomerStatus.可用积分变更类型.门店交易,
                                       Convert.ToInt32(posOrder.OrderPoint ),
                                       posOrder.SerialNumber + " 购买增加",
                                       ""
                                   );
                                if (posOrder.UsePoint>0)
                                {
                                    ///积分抵消金额
                                    Hyt.BLL.LevelPoint.PointBo.Instance.UpdateAvailablePoint(
                                          customer,
                                           0,
                                           Model.WorkflowStatus.CustomerStatus.可用积分变更类型.门店交易,
                                           Convert.ToInt32(posOrder.UsePoint * -1),
                                           posOrder.SerialNumber + " 积分抵消金额：" + posOrder.PointMoney.ToString("C"),
                                           ""
                                       );
                                }
                            }
                        }
                        
                        //Hyt.BLL.Promotion.SpCouponBo.Instance.
                        CBSpCoupon coupon= Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(posOrder.CouponSysNo);
                        if (coupon!=null)
                        {
                            coupon.UsedQuantity++;
                            Hyt.BLL.Promotion.PromotionBo.Instance.SaveCoupon(coupon, 0);
                        }
                       
                        tran.Complete();
                        result.Status = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 添加退货单数据
        /// </summary>
        /// <param name="cbReturnOrder">退货单数据</param>
        /// <param name="items">退货单商品列表</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">MAC地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        public JsonResult InnerPosReturnOrderToService(CBDsPosReturnOrder cbReturnOrder,string items, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        cbReturnOrder.Items = JsonUtil.ToObject<List<DsPosReturnOrderItem>>(items);
                         List<DsPosOrderItem> orderItems = DsPosManageBo.Instance.GetPosOrderItemBySerialNumber(cbReturnOrder.SellOrderNumber);
                         cbReturnOrder.OrderSysNo = orderItems[0].pSysNo;
                         //cbReturnOrder.OrderSysNo = nowMod.SysNo;
                         //cbReturnOrder.DsSysNo = nowMod.pos_DsSysNo;
                        int sysNo = DsPosReturnOrderBo.Instance.Insert(cbReturnOrder);

                        ///退回积分
                        DsPosOrder posOrder = DsPosOrderBo.Instance.GetEntity(cbReturnOrder.OrderSysNo);
                        if (!string.IsNullOrEmpty(posOrder.CardNumber))
                        {
                            ///获取订单总金额
                            CBCrCustomer customer = CrCustomerBo.Instance.GetCrCustomer(posOrder.CardNumber);
                            if (customer != null)
                            {

                                Hyt.BLL.LevelPoint.PointBo.Instance.UpdateAvailablePoint(
                                      customer,
                                       0,
                                       Model.WorkflowStatus.CustomerStatus.可用积分变更类型.门店交易,
                                       Convert.ToInt32(cbReturnOrder.ReturnPoint * -1),
                                       cbReturnOrder.SerialNumber + " 退货减积分",
                                       ""
                                   );
                            }
                        }

                        DsDealerWharehouse warehouse = DsDealerWharehouseBo.Instance.GetByDsUserSysNo(nowMod.pos_DsSysNo);
                        foreach (DsPosReturnOrderItem item in cbReturnOrder.Items)
                        {
                            item.OrderItemSysNo = orderItems.Find(p => p.ProSysNo == item.ProSysNo).SysNo;
                            item.pSysNo = sysNo;
                            DsPosReturnOrderBo.Instance.InsertItem(item);

                            PdProductStock stock = PdProductStockBo.Instance.GetEntityByWP(warehouse.WarehouseSysNo, item.ProSysNo);
                            if (stock == null)
                            {
                                stock = new PdProductStock()
                                {
                                    WarehouseSysNo = warehouse.WarehouseSysNo,
                                    PdProductSysNo = item.ProSysNo,
                                    StockQuantity = 0,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateDate = DateTime.Now
                                };
                            }
                            PdProductStockBo.Instance.UpdateStockQuantity(warehouse.WarehouseSysNo, item.ProSysNo, item.ReturnNum * -1);
                        }
                        tran.Complete();
                        result.Status = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 添加仓库入库表
        /// </summary>
        /// <param name="stockIn"></param>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult PosInWareOrderData(PdProductStockIn stockIn,string items, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        DsDealerWharehouse warehouse = DsDealerWharehouseBo.Instance.GetByDsUserSysNo(nowMod.pos_DsSysNo);
                        stockIn.SysNo = 0;
                        Result tResult= PdProductStockInBo.Instance.SavePdProductStockIn(stockIn, new SyUser() { CreatedDate=DateTime.Now, LastUpdateDate= DateTime.Now });
                        List<PdProductStockInDetail> detailList = JsonUtil.ToObject<List<PdProductStockInDetail>>(items);
                        foreach(PdProductStockInDetail detail in detailList)
                        {
                            detail.SysNo = 0;
                            detail.WarehouseSysNo = warehouse.WarehouseSysNo;
                            detail.ProductStockInSysNo=tResult.StatusCode;
                            PdProductStockInDetailBo.Instance.SavePdProductStockInDetail(detail, new SyUser() { CreatedDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                            ///判断库存是否存在当前商品
                            PdProductStock tempStock = PdProductStockBo.Instance.GetEntityByWP(warehouse.WarehouseSysNo, detail.PdProductSysNo);
                            if (tempStock != null)
                            {
                                PdProductStockBo.Instance.UpdateStockQuantity(warehouse.WarehouseSysNo, detail.PdProductSysNo, detail.StorageQuantity * -1);
                            }
                            else
                            {
                                CBPdProduct pdproduct = Hyt.BLL.Product.PdProductBo.Instance.GetProduct(detail.PdProductSysNo);
                                tempStock = new PdProductStock()
                                {
                                    Barcode = pdproduct.Barcode,
                                    CostPrice = 0,
                                    LastUpdateBy = 0,
                                    CreatedBy = 0,
                                    CreatedDate = DateTime.Now,
                                    CustomsNo = "",
                                    LastUpdateDate = DateTime.Now,
                                    PdProductSysNo = pdproduct.SysNo,
                                    StockQuantity = detail.StorageQuantity,
                                    WarehouseSysNo = warehouse.WarehouseSysNo
                                };
                                PdProductStockBo.Instance.SavePdProductStock(tempStock);
                            }
                        }
                    }
                    tran.Complete();
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 通过商品销售流水号获取商品
        /// </summary>
        /// <param name="numberNo">销售流水号</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">Mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        public JsonResult GetPosOrderBySerialNumber(string numberNo, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {
                
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        List<DsPosOrderItem> items = DsPosManageBo.Instance.GetPosOrderItemBySerialNumber(numberNo);
                        result.Status = true;
                        result.Message = JsonUtil.ToJson2(items);
                    }
               
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
           
        }
        
        /// <summary>
        /// 获取网络会员信息
        /// </summary>
        public JsonResult GetMemberCardNumberData(string numberNo, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {

                List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                Model.Pos.DsPosManage nowMod = null;
                foreach (Model.Pos.DsPosManage mod in list)
                {
                    if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                    {
                        nowMod = mod;
                        break;
                    }
                }
                if (nowMod == null)
                {
                    result.Status = false;
                    result.Message = "Pos机有异，请检查是否有异常";
                }
                else
                {
                    CBCrCustomer customer =  CrCustomerBo.Instance.GetCrCustomer(numberNo);
                    if(customer!=null)
                    {
                        DsMembershipCard card = new DsMembershipCard();
                        if (!DsMemberBo.Instance.CheckMembershipCard(numberNo))
                         {
                             card.DsCustomSysNo = customer.SysNo;
                             card.CardNumber = numberNo;
                             card.Birthday = DateTime.Now;
                             card.DsSysNo = 0;
                             card.LinkTele = customer.MobilePhoneNumber;
                             card.OnWebType = 1;
                             card.PointIntegral = customer.AvailablePoint;
                             card.UserLevel = 0;
                             card.UserName = customer.Name;
                             int sysNo = DsMemberBo.Instance.InsertMembershipCard(card);
                         }
                        else
                        {
                            card = DsMemberBo.Instance.GetMembershipCardBySysNo(customer.Account);
                            card.PointIntegral = customer.AvailablePoint;
                            ///会员名称同步
                            customer.Name = card.UserName;
                            CrCustomerBo.Instance.Update(customer);
                        }
                        //List<DsPosOrderItem> items = DsPosManageBo.Instance.GetPosOrderItemBySerialNumber(numberNo);
                        result.Status = true;
                        result.Message = JsonUtil.ToJson2(card);
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "未查询到当前会员信息。";
                    }
                }

            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }

        #endregion

        #region 收银机的对账
        /// <summary>
        /// 收银机管理对账功能
        /// </summary>
        /// <param name="dateTime">对账时间</param>
        /// <param name="key">秘钥</param>
        /// <param name="Mac">Mac地址</param>
        /// <param name="PosName">Pos机名称</param>
        /// <returns></returns>
        /// 
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500601)]
        public ActionResult GetPosManageReconciliation(DateTime? startTime, DateTime? endTime)
        {
            if (endTime == null)
            {
                endTime = DateTime.Now;
            }

            if (startTime == null)
            {
                startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            List<Model.Pos.CBDsPosManage> list = new List<CBDsPosManage>();

            List<PosCheckValueMod> posCheckModList = new List<PosCheckValueMod>();

            if(CurrentUser.IsBindDealer)
            {
                list = DsPosManageBo.Instance.GetEntityListByDsSysNo(CurrentUser.Dealer.SysNo);
            }
            else
            {
                list = DsPosManageBo.Instance.GetEntityListByDsSysNo(0);
            }

            foreach (CBDsPosManage nowMod in list)
            {
                List<DsPosReturnOrder> retList = Hyt.DataAccess.Pos.IDsPosReturnOrderDao.Instance.GetPosReturnOrderList(nowMod.SysNo, startTime.Value, endTime.Value, nowMod.pos_DsSysNo);
                List<DBDsPosOrder> posList = Hyt.DataAccess.Pos.IDsPosOrderDao.Instance.GetPosOrderList(nowMod.SysNo, startTime.Value, endTime.Value, nowMod.pos_DsSysNo);
                decimal posOrginTotalValue = 0;
                decimal posTotalValue = 0;
                decimal posRetValue = 0;
                decimal PosDisValue = 0;
                decimal PointValue=0;
                decimal Cash = 0;
                decimal noCash = 0;

                decimal KimsVolume = 0;

                decimal BankValue = 0;
                decimal AliValue = 0;
                decimal WXValue = 0;

                ///积分转金额比例
                decimal RateValue =0;
                try
                {
                    RateValue = DsMemberBo.Instance.GetDsPaymentToPointConfig(0).PaymentMoney * 100;
                }
                catch { }

                foreach (DBDsPosOrder order in posList)
                {
                    posOrginTotalValue += order.TotalOrigValue;
                    PosDisValue += order.TotalDisValue;
                    posTotalValue += order.TotalSellValue;

                    PointValue -= (order.UsePoint);
                    ///代金卷金额
                    KimsVolume += order.CouponAmount;

                    if (order.PayType == "Cash")
                    {
                        Cash += order.TotalSellValue;
                    }
                    else if(order.PayType == "Bank")
                    {
                        BankValue += order.TotalSellValue;
                    }
                    else if (order.PayType == "Ali")
                    {
                        AliValue += order.TotalSellValue;
                    }
                    else if (order.PayType == "WX")
                    {
                        WXValue += order.TotalSellValue;
                    }
                    //else if (order.PayType != "Cash")
                    //{
                    //    noCash += order.TotalSellValue;
                    //}
                }
                foreach (DsPosReturnOrder order in retList)
                {
                    posRetValue += order.TotalReturnValue;

                    PointValue += (order.ReturnPoint);

                    if (order.PayType == "Cash")
                    {
                        Cash -= order.TotalReturnValue;
                    }
                    else if (order.PayType == "Bank")
                    {
                        BankValue -= order.TotalReturnValue;
                    }
                    else if (order.PayType == "Ali")
                    {
                        AliValue -= order.TotalReturnValue;
                    }
                    else if (order.PayType == "WX")
                    {
                        WXValue -= order.TotalReturnValue;
                    }
                    //else if (order.PayType != "Cash")
                    //{
                    //    noCash -= order.TotalReturnValue;
                    //}
                }
                PosCheckValueMod jsonMod = posCheckModList.Find(p => p.DealerName == nowMod.DealerName);
                if (jsonMod == null)
                {
                    jsonMod = new PosCheckValueMod()
                    {
                        DealerName = nowMod.DealerName,
                        PosName = nowMod.pos_posName,
                        CheckTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        PosOrderNum = posList.Count.ToString(),
                        PosTotalValue = posTotalValue.ToString("0.00"),
                        PosRetOrderNum = retList.Count.ToString(),
                        PosRetTotalValue = posRetValue.ToString("0.00"),
                        posOrginTotalValue = posOrginTotalValue.ToString("0.00"),
                        PosDisValue = PosDisValue.ToString("0.00"),
                        PosSellValue = (posTotalValue - posRetValue).ToString("0.00"),
                        Cash = Cash.ToString("0.00"),
                        NoCash = noCash.ToString("0.00"),
                        PointMoney = PointValue.ToString("0"),
                        ///非现金金额
                        KimsVolume = KimsVolume.ToString("0.00"),
                        BankValue = BankValue.ToString("0.00"),
                        AliValue = AliValue.ToString("0.00"),
                        WXValue = WXValue.ToString("0.00")
                    };
                    posCheckModList.Add(jsonMod);
                }
                else
                { 
                    jsonMod.PosOrderNum = (Convert.ToInt32(jsonMod.PosOrderNum)+ posList.Count).ToString();
                    jsonMod.PosTotalValue = (Convert.ToDecimal(jsonMod.PosTotalValue) + posTotalValue).ToString();
                    jsonMod.PosRetOrderNum = (Convert.ToInt32(jsonMod.PosRetOrderNum) + retList.Count).ToString();
                    jsonMod.PosRetTotalValue =(Convert.ToDecimal(jsonMod.PosRetTotalValue)+ posRetValue).ToString();
                    jsonMod.posOrginTotalValue =(Convert.ToDecimal(jsonMod.posOrginTotalValue)+ posOrginTotalValue).ToString();
                    jsonMod.PosDisValue = (Convert.ToDecimal(jsonMod.PosDisValue)+ PosDisValue).ToString();
                    jsonMod.PosSellValue = (Convert.ToDecimal(jsonMod.PosSellValue)+ posTotalValue - posRetValue).ToString();
                    jsonMod.Cash = (Convert.ToDecimal(jsonMod.Cash) +Cash).ToString("0.00");
                    jsonMod.NoCash = (Convert.ToDecimal(jsonMod.NoCash) + noCash).ToString("0.00");
                    jsonMod.PointMoney = (Convert.ToDecimal(jsonMod.PointMoney) + PointValue).ToString("0");

                    ///非现金金额
                    jsonMod.KimsVolume = (Convert.ToDecimal(jsonMod.KimsVolume) + KimsVolume).ToString("0.00");
                    jsonMod.BankValue = (Convert.ToDecimal(jsonMod.BankValue) + BankValue).ToString("0.00");
                    jsonMod.AliValue = (Convert.ToDecimal(jsonMod.AliValue) + AliValue).ToString("0.00");
                    jsonMod.WXValue = (Convert.ToDecimal(jsonMod.WXValue) + WXValue).ToString("0.00");
                }
                
            }
            posCheckModList = posCheckModList.OrderBy(p => p.DealerName).ToList();
            ViewBag.StartTime = startTime.Value.ToString("yyyy-MM-dd");
            ViewBag.EndTime = endTime.Value.ToString("yyyy-MM-dd");
            ViewBag.ModList = posCheckModList;
            //return Json(result);
            return View();
        }
        #endregion

        #region 同步更新数据
        public ActionResult UpdateSellData(string PosName, string key, string Mac, string stream)
        {
            

            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
                return Content("Pos机有异，请检查是否有异常");
            }
            else
            {

                string s = stream;
                

                string posOrder = "";
                string posOrderItems = "";
                string posRetOrder = "";
                string posRetOrderItems = "";
                string posTakeStockOrder = "";
                string posTakeStockItems = "";

                string posMembershipCard = "";
                string posMemberPointHistory = "";
                ///销售订单
                posOrder = s.Substring(0, s.IndexOf("-------posOrderItems-------"));
                s = s.Substring(s.IndexOf("-------posOrderItems-------") );
                posOrder = posOrder.Replace("-------posOrder-------", "");
                ///销售订单明细
                posOrderItems = s.Substring(0, s.IndexOf("-------posRetOrder-------"));
                s = s.Substring(s.IndexOf("-------posRetOrder-------") );
                posOrderItems = posOrderItems.Replace("-------posOrderItems-------", "");
                ///退货订单
                posRetOrder = s.Substring(0, s.IndexOf("-------posRetOrderItems-------") );
                s = s.Substring(s.IndexOf("-------posRetOrderItems-------") );
                posRetOrder = posRetOrder.Replace("-------posRetOrder-------", "");
                ///退货订单明细
                posRetOrderItems = s.Substring(0, s.IndexOf("-------posTakeStockOrder-------") );
                s = s.Substring(s.IndexOf("-------posTakeStockOrder-------") );
                posRetOrderItems = posRetOrderItems.Replace("-------posRetOrderItems-------", "");
                ///盘点订单
                posTakeStockOrder = s.Substring(0, s.IndexOf("-------posTakeStockItems-------") );
                s = s.Substring(s.IndexOf("-------posTakeStockItems-------") );
                posTakeStockOrder = posTakeStockOrder.Replace("-------posTakeStockOrder-------", "");
                ///盘点订单明细
                posTakeStockItems = s.Substring(0, s.IndexOf("-------MemberShipCardList-------"));
                s = s.Substring(s.IndexOf("-------MemberShipCardList-------"));
                posTakeStockItems = posTakeStockItems.Replace("-------posTakeStockItems-------", "");
                ///会员卡
                posMembershipCard = s.Substring(0, s.IndexOf("-------MemberPointHistoryList-------"));
                s = s.Substring(s.IndexOf("-------MemberPointHistoryList-------"));
                posMembershipCard = posMembershipCard.Replace("-------MemberShipCardList-------", "");
                ///会员积分使用记录
                posMemberPointHistory = s;
                posMemberPointHistory = posMemberPointHistory.Replace("-------MemberPointHistoryList-------", "");

                ///获取Pos单数据
                List<DsPosOrder> orderList = GetSellOrderData("DsPosOrder", posOrder) as List<DsPosOrder>;
                List<DsPosOrderItem> orderItemList = GetSellOrderData("DsPosOrderItem", posOrderItems) as List<DsPosOrderItem>;

                List<DsPosReturnOrder> retOrderList = GetSellOrderData("DsPosReturnOrder", posRetOrder) as List<DsPosReturnOrder>;
                List<DsPosReturnOrderItem> retOrderItemList = GetSellOrderData("DsPosReturnOrderItem", posRetOrderItems) as List<DsPosReturnOrderItem>;

                List<DsTakeStockOrder> takeOrderList = GetSellOrderData("DsTakeStockOrder", posTakeStockOrder) as List<DsTakeStockOrder>;
                List<DsTakeStockItem> takeOrderItemList = GetSellOrderData("DsTakeStockItem", posTakeStockItems) as List<DsTakeStockItem>;

                List<DsMembershipCard> cardList = GetSellOrderData("DsMembershipCard", posMembershipCard) as List<DsMembershipCard>;
                List<MemberPointHistory> cardHistoryList = GetSellOrderData("MemberPointHistory", posMemberPointHistory) as List<MemberPointHistory>; 

                Dictionary<int, int> dicList = new Dictionary<int, int>();
                ///销售单
                foreach (DsPosOrder order in orderList)
                {
                    order.DsSysNo = nowMod.pos_DsSysNo;
                    order.DsPosSysNo = nowMod.SysNo;
                    int sysNo = DsPosOrderBo.Instance.Insert(order);
                    dicList.Add(order.SysNo, sysNo);

                    List<DsPosOrderItem> tempOrderItem = orderItemList.FindAll(p => p.pSysNo == order.SysNo);
                    foreach (DsPosOrderItem orderItem in tempOrderItem)
                    {
                        orderItem.pSysNo = sysNo;
                        DsPosOrderBo.Instance.InsertItem(orderItem);
                    }
                    //order.SysNo=sysNo;
                }
                ///销售退货单
                foreach (DsPosReturnOrder order in retOrderList)
                {
                    try
                    {
                        order.OrderSysNo = dicList[order.OrderSysNo];
                    }
                    catch
                    { }
                    
                    int sysNo = DsPosReturnOrderBo.Instance.Insert(order);

                    List<DsPosReturnOrderItem> tempOrderItem = retOrderItemList.FindAll(p => p.pSysNo == order.SysNo);
                    foreach (DsPosReturnOrderItem orderItem in tempOrderItem)
                    {
                        orderItem.pSysNo = sysNo;
                        DsPosReturnOrderBo.Instance.InsertItem(orderItem);
                    }
                }

                ///添加会员卡
                foreach (DsMembershipCard card in cardList)
                {
                    if (!DsMemberBo.Instance.CheckMembershipCard(card.CardNumber))
                    {
                        ///绑定网站会员信息
                        Hyt.Model.Transfer.CBCrCustomer mod = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(card.CardNumber);
                        if (mod != null)
                        {
                            card.DsCustomSysNo = mod.SysNo;
                        }
                        else
                        {
                            ///无会员，创建网站会员
                            string pass = "123456";//Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt("123456");
                            var customerSysNo = BLL.CRM.CrCustomerBo.Instance.RegisterFrontCustomer(
                                                                                      card.CardNumber
                                                                                      , pass
                                                                                      , Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证
                                                                                      , Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证
                                                                                      , Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.门店
                                                                                  );
                            mod = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(card.CardNumber);
                            card.DsCustomSysNo = mod.SysNo;
                        }
                        card.DsSysNo = nowMod.pos_DsSysNo;
                        int sysNo = DsMemberBo.Instance.InsertMembershipCard(card);
                    }
                }

                Dictionary<string, CBCrCustomer> dicMemberList = new Dictionary<string, CBCrCustomer>();
                ///添加会员积分使用情况
                foreach (MemberPointHistory history in cardHistoryList)
                {
                    history.DsSysNo = nowMod.pos_DsSysNo;
                    int sysNo = DsMemberBo.Instance.InsertMemberPoint(history);

                    if (dicMemberList.ContainsKey(history.mph_CardNumber))
                    {
                        //Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateCrCustomerExperiencePoint(dicMemberList[history.mph_CardNumber], history.mph_Point);
                        Hyt.BLL.LevelPoint.PointBo.Instance.UpdateAvailablePoint(
                            dicMemberList[history.mph_CardNumber], 
                            0, 
                            Model.WorkflowStatus.CustomerStatus.可用积分变更类型.门店交易,
                            Convert.ToInt32(history.mph_Point), 
                            history.mph_OrderNumber + " " + history.mph_Text, 
                            ""
                        );
                    }
                    else
                    {

                        Hyt.Model.Transfer.CBCrCustomer mod = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(history.mph_CardNumber);
                        if (mod!=null)
                        {
                            dicMemberList.Add(history.mph_CardNumber, mod);
                            //Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateCrCustomerExperiencePoint(dicMemberList[history.mph_CardNumber], history.mph_Point);
                            Hyt.BLL.LevelPoint.PointBo.Instance.UpdateAvailablePoint(
                                   dicMemberList[history.mph_CardNumber],
                                   0,
                                   Model.WorkflowStatus.CustomerStatus.可用积分变更类型.门店交易,
                                   Convert.ToInt32(history.mph_Point),
                                   history.mph_OrderNumber + " " + history.mph_Text,
                                   ""
                               );
                        }
                    }
                   
                }

                DsDealerWharehouse Wharehouse = Hyt.BLL.Distribution.DsDealerWharehouseBo.Instance.GetByDsUserSysNo(nowMod.pos_DsSysNo);
                Dictionary<int, DsTakeStockItem> DicList = new Dictionary<int, DsTakeStockItem>();
                ///库存盘点
                foreach(DsTakeStockOrder order in takeOrderList )
                {
                    order.DsSysNo = nowMod.pos_DsSysNo;
                    int sysNo = DsTakeStockOrderBo.Instance.Insert(order);

                    ///添加盘点单明细
                    List<DsTakeStockItem> tempOrderItem = takeOrderItemList.FindAll(p => p.PSysNo == order.SysNo);
                    foreach (DsTakeStockItem orderItem in tempOrderItem)
                    {
                        orderItem.PSysNo = sysNo;
                        DsTakeStockOrderBo.Instance.InsertItem(orderItem);
                        ///保存商品字典信息
                        if(!DicList.ContainsKey(orderItem.ProductSysNo))
                        {
                            DicList.Add(orderItem.ProductSysNo, orderItem);
                        }
                    }
                }
                ///商品档案
                if (Wharehouse != null)
                {
                    List<PdProductStock> proStockList = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetPdProductStockList(Wharehouse.WarehouseSysNo);
                    foreach (int tkey in DicList.Keys)
                    {
                        PdProductStock tempProduct = proStockList.Find(p => p.PdProductSysNo == tkey);
                        if (tempProduct != null)
                        {
                            tempProduct.StockQuantity = DicList[tkey].ProNowNum;
                            tempProduct.Barcode = DicList[tkey].ProductBarCode;
                            tempProduct.LastUpdateDate = DateTime.Now;
                            PdProductStockBo.Instance.UpdateProductStockInfo(tempProduct);
                        }
                        else
                        {
                            tempProduct = new PdProductStock()
                            {
                                Barcode = DicList[tkey].ProductBarCode,
                                WarehouseSysNo = Wharehouse.WarehouseSysNo,
                                PdProductSysNo = DicList[tkey].ProductSysNo,
                                CostPrice = 0,
                                StockQuantity = DicList[tkey].ProNowNum,
                                CreatedDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now
                            };
                            PdProductStockBo.Instance.SavePdProductStock(tempProduct);
                        }
                    }
                }
            }
            return Content("true");
        }

        public object GetSellOrderData(string type,string message)
        {
            //List<Object> objList = new List<Object>();

            List<string> strJsonData = new List<string>();
            while (true)
            {
                string temp_txt = "";
                int indx = message.IndexOf("][");
                if (indx == -1)
                {
                    temp_txt = message;
                    message = "";
                    strJsonData.Add(temp_txt);
                    break;
                }
                temp_txt = message.Substring(0, indx + 1);
                message = message.Substring(indx + 1);
                strJsonData.Add(temp_txt);
            }

            ///生成需保存数据
            switch(type)
            {
                    ///pos销售订单
                case "DsPosOrder":
                    List<DsPosOrder> orderList = new List<DsPosOrder>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            orderList.AddRange(JsonUtil.ToObject<List<DsPosOrder>>(for_txt, 10000));
                        }
                    }
                    return orderList;
                    ///pos销售订单明细
                case "DsPosOrderItem":
                    List<DsPosOrderItem> orderItemList = new List<DsPosOrderItem>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            orderItemList.AddRange(JsonUtil.ToObject<List<DsPosOrderItem>>(for_txt, 10000));
                        }
                    }
                    return orderItemList;
                    ///pos销售退货订单
                case "DsPosReturnOrder":
                    List<DsPosReturnOrder> retOrderList = new List<DsPosReturnOrder>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            retOrderList.AddRange(JsonUtil.ToObject<List<DsPosReturnOrder>>(for_txt, 10000));
                        }
                    }
                    return retOrderList;
                    ///pos销售退货订单明细
                case "DsPosReturnOrderItem":
                    List<DsPosReturnOrderItem> retOrderItemList = new List<DsPosReturnOrderItem>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            retOrderItemList.AddRange(JsonUtil.ToObject<List<DsPosReturnOrderItem>>(for_txt, 10000));
                        }
                    }
                    return retOrderItemList;
                    ///Pos盘点单
                case "DsTakeStockOrder":
                    List<DsTakeStockOrder> takeStockList = new List<DsTakeStockOrder>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            takeStockList.AddRange(JsonUtil.ToObject<List<DsTakeStockOrder>>(for_txt, 10000));
                        }
                    }
                    return takeStockList;
                    ///pos盘点单明细
                case "DsTakeStockItem":
                    List<DsTakeStockItem> takeStockItemList = new List<DsTakeStockItem>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            takeStockItemList.AddRange(JsonUtil.ToObject<List<DsTakeStockItem>>(for_txt, 10000));
                        }
                    }
                    return takeStockItemList;
                case "DsMembershipCard":
                    List<DsMembershipCard> membershipCardList = new List<DsMembershipCard>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            membershipCardList.AddRange(JsonUtil.ToObject<List<DsMembershipCard>>(for_txt, 10000));
                        }
                    }
                    return membershipCardList;
                case "MemberPointHistory":
                    List<MemberPointHistory> memberPointHistoryList = new List<MemberPointHistory>();
                    foreach (string for_txt in strJsonData)
                    {
                        if (!string.IsNullOrEmpty(for_txt))
                        {
                            memberPointHistoryList.AddRange(JsonUtil.ToObject<List<MemberPointHistory>>(for_txt, 10000));
                        }
                    }
                    return memberPointHistoryList;
            }
            return null;
        }

        #endregion

        #region 版本验证
        /// <summary>
        /// 检测pos版本
        /// </summary>
        /// <param name="Key">键值</param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public ActionResult CheckVersionData(string Key, string Mac, string PosName,string version)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {

                DsPosVersion posVersion = DsPosVersionBo.Instance.GetPosVersionByDsSysNo(nowMod.pos_DsSysNo);
                if (posVersion != null)

                {

                    int oldVersionValue = 0 ;
                    int sysVersionValue = 0;
                    int.TryParse(version.Replace(".", ""), out oldVersionValue);
                    int.TryParse(posVersion.DsVersion.Replace(".", ""), out sysVersionValue);
                    if (oldVersionValue < sysVersionValue)
                    {
                        return Content(posVersion.DsVersion + "|" + posVersion.DsFilePath);
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    return Content("false");
                }
            }
            return Content("false");
        }
        #endregion

        #endregion

        #region 经销商Pos管理
        /// <summary>
        /// 添加和修改收银机管理
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS100101)]
        public ActionResult AddOrUpdateDsManage(int? sysNo)
        {
            if(CurrentUser.IsBindDealer)
            {
                DsPosManage mod = new DsPosManage();
                ViewBag.DSName = "";
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
                mod.pos_KeyData = Hyt.Util.RandomString.GetRndStrOnlyFor(10);
                if (sysNo != null)
                {
                    mod = DsPosManageBo.Instance.GetEntity(sysNo.Value);
                }
                DsDealer dsMod = Hyt.BLL.Distribution.DsDealerBo.Instance.GetDsDealer(CurrentUser.Dealer.SysNo);
                if (dsMod != null)
                {
                    ViewBag.DSName = dsMod.DealerName;
                }
                ViewBag.mod = mod;
            }
            
            return View();
        }
        /// <summary>
        /// 保持收银机信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="DealerName"></param>
        /// <param name="keyCode"></param>
        /// <param name="macCode"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS100101)]
        public JsonResult SaveDsManageData(int sysNo, string DealerName,
            string keyCode, string macCode, int dsSysNo, string Termid)
        {
            DsPosManage mod = new DsPosManage();
            mod.pos_dateTime = DateTime.Now;
            mod.pos_DsSysNo = dsSysNo;
            if (sysNo > 0)
            {
                 mod = DsPosManageBo.Instance.GetEntity(sysNo);
                
            }
            mod.pos_MacData = macCode;
            mod.pos_KeyData = keyCode;
            mod.pos_posName = DealerName;
            mod.pos_BindTime = DateTime.Now;
            mod.pos_dateTime = DateTime.Now;
            mod.Pos_TLTermid = Termid;
            if (sysNo > 0)
            {
                DsPosManageBo.Instance.Update(mod);
            }
            else
            {
                DsPosManageBo.Instance.Insert(mod);
            }
            return Json(new { Status = true });
        }
        #region 销售单金额

        /// <summary>
        /// 收银机销售单管理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-02-28 杨云奕 添加
        /// </remarks>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public ActionResult GetPosSellManagePage(int? id)
        {
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
                if (Request.IsAjaxRequest())
                {
                    var model = DsPosOrderBo.Instance.GetPosOrderListPagerByDsSysNo(id, CurrentUser.Dealer.SysNo,null,null);
                    return PartialView("_AjaxPosSellManagePage", model);
                }
            }
            else
            {
                ViewBag.dsSysNo = 0;
                if (Request.IsAjaxRequest())
                {
                    var model = DsPosOrderBo.Instance.GetPosOrderListPagerByDsSysNo(id, ViewBag.dsSysNo, null, null);
                    return PartialView("_AjaxPosSellManagePage", model);
                }
            }
            return View();
        }
        /// <summary>
        /// 读取销售单数据信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-02-28 杨云奕 添加
        /// </remarks>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public ActionResult GetPosSellManageViewData(int SysNo)
        {
            DsPosOrder posOrder = DsPosOrderBo.Instance.GetEntity(SysNo);
            List<DsPosOrderItem> posOrderItem = DsPosOrderBo.Instance.GetEntityItems(SysNo);
            ViewBag.Order = posOrder;
            ViewBag.ItemList = posOrderItem;
            return View();
        }

        /// <summary>
        /// 添加或者更新销售单
        /// </summary>
        /// <param name="SysNo">编号</param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public ActionResult AddOrUpdatePosSellManageData(int? SysNo)
        {
            DsPosOrder posOrder = new DsPosOrder();
            posOrder.SaleTime = DateTime.Now.ToString();
            List<DsPosOrderItem> posOrderItem = new List<DsPosOrderItem>();
            if (SysNo!=null)
            {
                posOrder = DsPosOrderBo.Instance.GetEntity(SysNo.Value);
                posOrderItem = DsPosOrderBo.Instance.GetEntityItems(SysNo.Value);
            }
            ViewBag.Order = posOrder;
            ViewBag.ItemList = posOrderItem;
            if (CurrentUser.IsBindAllDealer)
            {
                ViewBag.DealerList = DsDealerBo.Instance.GetAllDealerList().Where(p => p.SysNo > 0).ToList();
            }
            else if (CurrentUser.IsBindDealer)
            {
                ViewBag.DealerList = DsDealerBo.Instance.GetAllDealerList().Where(p => p.SysNo > 0&& p.SysNo==CurrentUser.Dealer.SysNo).ToList();
            }
            else
            {
                ViewBag.DealerList =new List<DsDealer>();
            }
            return View();
        }
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public JsonResult GetDsPosManage(int DsSysNo)
        {
            List<CBDsPosManage> posList = DsPosManageBo.Instance.GetEntityListByDsSysNo(DsSysNo);
            return Json(posList);
        }
        /// <summary>
        /// 通过条码获取商品详情
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public ActionResult GetProductListInfo(string barcode)
        {
            PdProduct product = Hyt.BLL.Product.PdProductBo.Instance.GetProductByBarcode(barcode);
            return Json(product);
        }
        /// <summary>
        /// 保存销售单的数据
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS200101)]
        public JsonResult SavePosOrderData(DsPosOrder posOrder, string Items)
        {
            Result result = new Result();
            try
            {
                List<DsPosOrderItem> items = Util.Serialization.JsonUtil.ToObject<List<DsPosOrderItem>>(Items);
                using (var trans = new TransactionScope())
                {
                    posOrder.PayTime = DateTime.Now;
                    int SysNo = DsPosOrderBo.Instance.Insert(posOrder);
                    foreach (var item in items)
                    {
                        item.pSysNo = SysNo;
                        DsPosOrderBo.Instance.InsertItem(item);
                    }
                    trans.Complete();
                }
                result.Status = true;
                result.Message = "保存成功";
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        #endregion

        #region 退货单管理
        /// <summary>
        /// 退货单管理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS300101)]
        public ActionResult GetPosReturnSellManagePage(int? id)
        {
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
                if (Request.IsAjaxRequest())
                {
                    var model = DsPosReturnOrderBo.Instance.GetPosReturnOrderListPagerByDsSysNo(id, CurrentUser.Dealer.SysNo);
                    return PartialView("_AjaxPosReturnSellManagePage", model);
                }
            }
            else
            {
                ViewBag.dsSysNo = 0;
                if (Request.IsAjaxRequest())
                {
                    var model = DsPosReturnOrderBo.Instance.GetPosReturnOrderListPagerByDsSysNo(id, CurrentUser.Dealer.SysNo);
                    return PartialView("_AjaxPosReturnSellManagePage", model);
                }
            }
            return View();
        }

        /// <summary>
        /// 读取退货单数据信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-02-28 杨云奕 添加
        /// </remarks>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS300101)]
        public ActionResult GetPosReturnSellManageViewData(int SysNo)
        {
            CBDsPosReturnOrder posReturnOrder = DsPosReturnOrderBo.Instance.GetCBPosReturnOrder(SysNo);
            List<CBDsPosReturnOrderItem> posReturnOrderItem = DsPosReturnOrderBo.Instance.GetEntityItemList(SysNo);
            ViewBag.Order = posReturnOrder;
            ViewBag.ItemList = posReturnOrderItem;
            return View();
        }
        #endregion

        #region 盘点单管理
        /// <summary>
        /// 获取盘点单列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS400101)]
        public ActionResult GetTakeStockOrderPage(int? id)
        {
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
                if (Request.IsAjaxRequest())
                {
                    var model = DsTakeStockOrderBo.Instance.GetTakeStockOrderListPagerByDsSysNo(id, CurrentUser.Dealer.SysNo,null,null);
                    return PartialView("_AjaxTakeStockOrderManagePage", model);
                }
            }
            else
            {
                ViewBag.dsSysNo = 0;
                if (Request.IsAjaxRequest())
                {
                    var model = DsTakeStockOrderBo.Instance.GetTakeStockOrderListPagerByDsSysNo(id, CurrentUser.Dealer.SysNo, null, null);
                    return PartialView("_AjaxTakeStockOrderManagePage", model);
                }
            }
            return View();
        }
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS400101)]
        public ActionResult GetTakeStockOrderViewData(int SysNo)
        {
            var takeStock = DsTakeStockOrderBo.Instance.GetTakeStockOrder(SysNo);
            var takeStockItems = DsTakeStockOrderBo.Instance.GetTakeStockOrderItems(SysNo);
            ViewBag.Order = takeStock;
            ViewBag.ItemList = takeStockItems;
            return View();
        }
        #endregion

        #region 门店会员管理
        /// <summary>
        /// 会员等级列表
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
       [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500101)]
        public ActionResult GetDsMembershioLevelList()
        {
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsList = DsMemberBo.Instance.GetDsMembershioLevelList(CurrentUser.Dealer.SysNo);
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
            }
            else
            {
                ViewBag.dsSysNo = 0;
                ViewBag.dsList = DsMemberBo.Instance.GetDsMembershioLevelList(0);
                
            }
            return View();
        }
       /// <summary>
       /// 保持修改会员等级信息
       /// </summary>
       /// <param name="level"></param>
       /// <returns></returns>
       /// 
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500101)]
        public JsonResult SaveMembershioLevelData(DsMembershioLevel level)
        {
            if (level.SysNo == 0)
            {
                DsMemberBo.Instance.InsertDsMembershioLevel(level);
            }
            else
            {
                DsMemberBo.Instance.UpdateDsMembershioLevel(level);
            }
            return Json(new { Status=true });
        }
        /// <summary>
        /// 添加和修改会员等级信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// 
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500101)]
        public ActionResult InserOrUpdateDsMembershipLevel(int? SysNo)
        {
            DsMembershioLevel levelMod = new DsMembershioLevel();
            ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
            if (SysNo != null)
            {
                levelMod = DsMemberBo.Instance.GetDsMembershioLevel(SysNo.Value);
            }
            ViewBag.IsBindDealer = CurrentUser.IsBindAllDealer;
            //if (CurrentUser.IsBindDealer)
            //{

            //}
            ViewBag.mod = levelMod;
            return View();
        }
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500201)]
        public ActionResult InnerOrUpdatePaymentToPointConfig()
        {
            DsPaymentToPointConfig config = DsMemberBo.Instance.GetDsPaymentToPointConfig(0);
            ViewBag.mod = config;
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
            }
            else
            {
                ViewBag.dsSysNo = 0;
            }
           
            return View();
        }
         [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500201)]
        public JsonResult SavePaymentToPointConfig(DsPaymentToPointConfig config)
        {
            DsPaymentToPointConfig configMod = DsMemberBo.Instance.GetDsPaymentToPointConfig(config.DsSysNo);
            if (configMod!=null)
            {
                config.SysNo = configMod.SysNo;
            }
            if(config.SysNo==0)
            {
                DsMemberBo.Instance.InsertDsPaymentToPointConfig(config);
            }
            else
            {
                DsMemberBo.Instance.UpdateDsPaymentToPointConfig(config);
            }
            return Json(new { Status = true });
        }
        /// <summary>
        /// 会员卡列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500301)]
        public ActionResult GetDsMembershipCardListPager(int? id)
        {
            if (CurrentUser.IsBindDealer)
            {
                ViewBag.dsSysNo = CurrentUser.Dealer.SysNo;
                if (Request.IsAjaxRequest())
                {
                    var model = DsMemberBo.Instance.GetMembershipCardListByPager(id, CurrentUser.Dealer.SysNo);
                    return PartialView("_AjaxDsMembershipCardPage", model);
                }
            }
            else
            {
                ViewBag.dsSysNo = 0;
                if (Request.IsAjaxRequest())
                {
                    var model = DsMemberBo.Instance.GetMembershipCardListByPager(id, ViewBag.dsSysNo);
                    return PartialView("_AjaxDsMembershipCardPage", model);
                }
            }
            return View();
        }
        [Hyt.Admin.Privilege(Hyt.Model.SystemPredefined.PrivilegeCode.POS500301)]
        public ActionResult GetDsMembershipCardViewData(int SysNo)
        {
            DsMembershipCard card = DsMemberBo.Instance.GetMembershipCardBySysNo(SysNo);
            List<MemberPointHistory> pointHistoryList = DsMemberBo.Instance.GetMemberPointHistoryList(card.CardNumber);
            ViewBag.Order = card;
            ViewBag.ItemList = pointHistoryList;
            return View();
        }
        #endregion
        #endregion

        #region 修改门店初始金额
        public ActionResult GetPosMoneyBoxPager(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                int DsSysNo = 0;
                if (id == null)
                {
                    id = 1;
                }
                if (CurrentUser.IsBindDealer)
                {
                    DsSysNo = CurrentUser.Dealer.SysNo;
                }
                var model = DsPosMoneyBoxBo.Instance.GetDsPosMoneyBoxListPagerByDsSysNo(id, DsSysNo);
                return PartialView("_AjaxPosMoneyBoxPager", model);
            }
            return View();
        }
        /// <summary>
        /// 修改钱箱数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public ActionResult EditPosMoneyBox(int? SysNo)
        {
            CBDsPosMoneyBox box = DsPosMoneyBoxBo.Instance.GetEntity(SysNo.Value);
            ViewBag.Mod = box;
            return View();
        }
        /// <summary>
        /// 保存钱箱数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public JsonResult SavePosMoneyBox(int SysNo, decimal _Money)
        {
            try
            {
                CBDsPosMoneyBox box = DsPosMoneyBoxBo.Instance.GetEntity(SysNo);
                box.SaveMoney = _Money;
                DsPosMoneyBoxBo.Instance.UpdateMod(box);
                return Json(new
                {
                    Status = true,
                    Message = ""
                });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }
        /// <summary>
        /// 获取钱柜额数据
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public JsonResult UpdateMoneyBoxValue(DsPosMoneyBox box, string Key, string Mac, string PosName)
        {
            
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(Key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            } 
            else
            {
                try
                {
                    box.DsSysNo = nowMod.pos_DsSysNo;
                    box.DsPosSysNo = nowMod.SysNo;
                    DsPosMoneyBoxBo.Instance.InsertMod(box);
                    return Json(new { Status = true, Message = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Status = false, Message = e.Message });
                }
            }
            return Json(result);
        }
        #endregion

        #region 添加收银部分网关验证
        /// <summary>
        /// 支付宝网站调用，网关验证
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ContentResult PosGateway(AliServicePostDataMod mod)
        {
            string str = "";
            foreach(string key in Request.Form.AllKeys)
            {
                str += key + "=" + Request.Form[key];
            }
            Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/PosGateway-Form.txt"), str);
            foreach (string key in Request.QueryString.AllKeys)
            {
                str += key + "=" + Request.Form[key];
            }
            Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/PosGateway-QueryString.txt"), str);

            GetewayControl gateway = new GetewayControl();
            string txt = gateway.InitGateway(mod
                , Request.MapPath("/RSA/rsa_private_key.pem")
                , Request.MapPath("/RSA/rsa_public_key.pem")
                , "2014072300007148");
            return Content(txt, "application/xml"); 
        }

        /// <summary>
        /// 按key获取get和post请求
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getRequestString(string key)
        {

            string value = null;
            if (Request.Form.Get(key) != null && Request.Form.Get(key).ToString() != "")
            {
                value = Request.Form.Get(key).ToString();
            }
            else if (Request.QueryString[key] != null && Request.QueryString[key].ToString() != "")
            {
                value = Request.QueryString[key].ToString();
            }

            return value;
        }

        /// <summary>
        /// 条码支付 功能操作
        /// </summary>
        /// <param name="posOrder">订单</param>
        /// <param name="items">订单明细</param>
        /// <param name="key">密钥</param>
        /// <param name="Mac">mac地址</param>
        /// <param name="PosName">pos名称</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-03-11 杨云奕 添加
        /// </remarks>
        public JsonResult ActionAliBarcodePay(DBDsPosOrder posOrder, string items,string auth_code, string key, string Mac, string PosName)
        {

            PosName = Server.UrlDecode(PosName);
            Result<AlipayF2FPayResult> result = new Result<AlipayF2FPayResult>();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        DsDealer dealerMod = DsDealerBo.Instance.GetDsDealer(nowMod.pos_DsSysNo);
                        ///条码支付，获取状态
                        BarcodePayControl payControl = new BarcodePayControl();

                        ///绑定收银机订单操作
                        posOrder.items = JsonUtil.ToObject<List<DsPosOrderItem>>(items);
                        posOrder.PosName = PosName;
                        posOrder.StoreName = dealerMod.DealerName;
                        posOrder.DsPosSysNo = nowMod.SysNo;
                        posOrder.DsSysNo = nowMod.pos_DsSysNo;
                        posOrder.PayType = Hyt.Model.WorkflowStatus.PosWebPayStatus.支付宝门店支付状态.支付宝条码付款.ToString();
                        //posOrder.PayAuthCode = auth_code;
                        ///进行支付验证
                        result = payControl.AlipayDsPosOrder(posOrder);

                        ///付款历史记录
                        DsPosBarcodePayLog payLog = new DsPosBarcodePayLog();
                        payLog.DsSysNo = nowMod.pos_DsSysNo;
                        payLog.Content = JsonUtil.ToJson2(result.Data.response);
                        payLog.PayType = Hyt.Model.WorkflowStatus.PosWebPayStatus.支付宝门店支付状态.支付宝条码付款.ToString();
                        payLog.PosOrderSysNo = 0;
                        payLog.ReceiptAmount = Convert.ToDecimal(result.Data.response.ReceiptAmount);
                        payLog.TotalAmount = Convert.ToDecimal(result.Data.response.TotalAmount);
                        payLog.TradeNo = result.Data.response.TradeNo;

                        ///返回订单验证
                        if (result.Status)
                        {
                            ///添加收银机账单
                            posOrder.Status = 1;
                            posOrder.TotalPayValue = Convert.ToDecimal(result.Data.response.ReceiptAmount);
                            posOrder.PayTime = DateTime.Now;
                            //posOrder.PayType
                            int sysNo = DsPosOrderBo.Instance.Insert(posOrder);
                            foreach (DsPosOrderItem item in posOrder.items)
                            {
                                item.pSysNo = sysNo;
                                DsPosOrderBo.Instance.InsertItem(item);
                            }

                            payLog.PosOrderSysNo = sysNo;

                            result.Status = true;
                            result.Message = JsonUtil.ToJson2(result.Data.response);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "条码支付失败-"+ (result.Data.response.Body);
                        }

                        DsPosBarcodePayLogBo.Instance.InnerDsPosBarcodePayLog(payLog);
                        tran.Complete();
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
           
        }

        /// <summary>
        /// 支付退款功能
        /// </summary>
        /// <param name="cbReturnOrder"></param>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-03-11 杨云奕 添加
        /// </remarks>
        public JsonResult ActionAliRefundPay(CBDsPosReturnOrder cbReturnOrder, string items, string key, string Mac, string PosName) 
        {
            PosName = Server.UrlDecode(PosName);
            Result<AlipayF2FRefundResult> result = new Result<AlipayF2FRefundResult>();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        DsDealer dealerMod = DsDealerBo.Instance.GetDsDealer(nowMod.pos_DsSysNo);
                        ///条码支付，获取状态
                        BarcodePayControl payControl = new BarcodePayControl();
                        cbReturnOrder.StoreName = dealerMod.DealerName;
                        cbReturnOrder.PosName = PosName;
                       
                        result = payControl.AlipayRefundDsPosOrder(cbReturnOrder);

                        ///条码支付历史记录
                        DsPosBarcodePayLog payLog = new DsPosBarcodePayLog();
                        payLog.DsSysNo = nowMod.pos_DsSysNo;
                        payLog.Content = JsonUtil.ToJson2(result.Data.response);
                        payLog.PayType = Hyt.Model.WorkflowStatus.PosWebPayStatus.支付宝门店支付状态.支付宝条码退款.ToString();
                        payLog.PosOrderSysNo = 0;
                        payLog.ReceiptAmount = Convert.ToDecimal(result.Data.response.RefundFee);
                        payLog.TotalAmount = Convert.ToDecimal(result.Data.response.RefundFee);
                        payLog.TradeNo = result.Data.response.TradeNo;

                        ////返回退款状态验证
                        if (result.Status)
                        {
                            cbReturnOrder.PayAuthCode = cbReturnOrder.AliPayNumber;
                            cbReturnOrder.PayType = Hyt.Model.WorkflowStatus.PosWebPayStatus.支付宝门店支付状态.支付宝条码退款.ToString();

                            cbReturnOrder.Items = JsonUtil.ToObject<List<DsPosReturnOrderItem>>(items);
                            List<DsPosOrderItem> orderItems = DsPosManageBo.Instance.GetPosOrderItemBySerialNumber(cbReturnOrder.SellOrderNumber);
                            cbReturnOrder.OrderSysNo = orderItems[0].pSysNo;
                            int sysNo = DsPosReturnOrderBo.Instance.Insert(cbReturnOrder);
                            foreach (DsPosReturnOrderItem item in cbReturnOrder.Items)
                            {
                                item.OrderItemSysNo = orderItems.Find(p => p.ProSysNo == item.ProSysNo).SysNo;
                                item.pSysNo = sysNo;
                                DsPosReturnOrderBo.Instance.InsertItem(item);
                            }

                            payLog.PosOrderSysNo = sysNo;
                           
                            result.Status = true;
                            result.Message = JsonUtil.ToJson2(result.Data.response);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "退款支付失败-" + result.Data.response.Body;
                        }

                        DsPosBarcodePayLogBo.Instance.InnerDsPosBarcodePayLog(payLog);
                        tran.Complete();
                        
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 阿里
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult ActionAliQuery(string Number, string items, string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result<AlipayF2FRefundResult> result = new Result<AlipayF2FRefundResult>();
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                    Model.Pos.DsPosManage nowMod = null;
                    foreach (Model.Pos.DsPosManage mod in list)
                    {
                        if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                        {
                            nowMod = mod;
                            break;
                        }
                    }
                    if (nowMod == null)
                    {
                        result.Status = false;
                        result.Message = "Pos机有异，请检查是否有异常";
                    }
                    else
                    {
                        DsDealer dealerMod = DsDealerBo.Instance.GetDsDealer(nowMod.pos_DsSysNo);
                        ///条码支付，获取状态
                        BarcodePayControl payControl = new BarcodePayControl();
                        ///获取支付单查询的反馈信息
                        Result<AlipayF2FQueryResult> resultMod = payControl.GetAliDsPosOrderQuery(Number);
                        if (resultMod.Status)
                        {
                            result.Status = true;
                            result.Message = JsonUtil.ToJson2(resultMod.Data.response);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "数据查询失败-" + resultMod.Data.response.Body;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        #endregion

        #region 获取验证手机验证码
        public JsonResult PhoneVerificationCode(string cardNumber, string key, string Mac, string PosName)
        {
            Result result = new Result();
            const int expiredTime = 300;    //驗證碼過期時間，秒
            const int retryTime = 30;       //重新發送時間
            const int randomSize = 6;       //隨機碼長度
            result.Status = true;
            PosName = Microsoft.JScript.GlobalObject.unescape(PosName);
           
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                var code = Hyt.Util.WebUtil.Number(randomSize, false);
                if (!Hyt.Util.Validator.VHelper.ValidatorRule(new Hyt.Util.Validator.Rule.Rule_Mobile(cardNumber)).IsPass)
                {
                    result.Message = "无效的手机号码";
                }
                else
                {
                    try
                    {
                        var smsResult = BLL.Extras.SmsBO.Instance.发送手机验证短信(cardNumber, code);
                        result.Status = smsResult.Status == Extra.SMS.SmsResultStatus.Success;

                        if (result.Status)
                        {

                            result.Message = code;
                            result.StatusCode = retryTime;
                        }

                    }
                    catch (Exception e)
                    {
                        result.Status = false;
                        result.Message = e.Message;
                    }
                }
            }
            return Json(result);
        }
        #endregion

        #region 获取用户优惠卷信息
        public JsonResult GetCustomerCoupon(string memberCardNumber, string key, string Mac, string PosName)
        {
            Result result = new Result();
            result.Status = true;
            PosName = Microsoft.JScript.GlobalObject.unescape(PosName);
           
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                 CBCrCustomer customer =  CrCustomerBo.Instance.GetCrCustomer(memberCardNumber);
                List<Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台> platformType=new List<Model.WorkflowStatus.PromotionStatus.促销使用平台>();
                platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.PC商城);
                platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.门店);
                platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.手机商城);
                platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.物流App);

                IList<SpCoupon> couponList =  Hyt.BLL.Promotion.SpCouponBo.Instance.GetCustomerCoupons(
                    customer.SysNo, 
                    Model.WorkflowStatus.PromotionStatus.优惠券状态.已审核, 
                    platformType.ToArray()
                );
                result.Status = true;
                result.Message = JsonUtil.ToJson2(couponList);
            }
            return Json(result);
        }
        /// <summary>
        /// 优惠卷条码
        /// </summary>
        /// <param name="memberCardNumber"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult GetCustomerCouponCode(string CouponCode, string key, string Mac, string PosName)
        {
            Result result = new Result();
            result.Status = true;
            PosName = Microsoft.JScript.GlobalObject.unescape(PosName);

            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                //CBCrCustomer customer = CrCustomerBo.Instance.GetCrCustomer(memberCardNumber);
                //List<Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台> platformType = new List<Model.WorkflowStatus.PromotionStatus.促销使用平台>();
                //platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.PC商城);
                //platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.门店);
                //platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.手机商城);
                //platformType.Add(Hyt.Model.WorkflowStatus.PromotionStatus.促销使用平台.物流App);
                IList<SpCoupon> couponList = new List<SpCoupon>();
                SpCoupon spMod = Hyt.BLL.Promotion.SpCouponBo.Instance.GetSpCouponByCouponCode(CouponCode);
                if (spMod != null)
                {
                    if (spMod.Status == 20 && spMod.UseQuantity > spMod.UsedQuantity)
                    {
                        couponList.Add(spMod);
                    }
                }
               
                result.Status = true;
                result.Message = JsonUtil.ToJson2(couponList);
            }
            return Json(result);
        }
        #endregion

        #region 获取会员信息
        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="telePhone"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult GetMemberCardData(string telePhone, string key, string Mac, string PosName)
        {
            Result result = new Result();
            result.Status = true;
            PosName = Microsoft.JScript.GlobalObject.unescape(PosName);

            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                result.Status = false;
                result.Message = "Pos机有异，请检查是否有异常";
            }
            else
            {
                CBCrCustomer customer = CrCustomerBo.Instance.GetCrCustomer(telePhone);
                if (customer != null)
                {
                    DsMembershipCard card = new DsMembershipCard();
                    if (!DsMemberBo.Instance.CheckMembershipCard(telePhone))
                    {
                        card.DsCustomSysNo = customer.SysNo;
                        card.CardNumber = telePhone;
                        card.Birthday = DateTime.Now;
                        card.DsSysNo = 0;
                        card.LinkTele = customer.MobilePhoneNumber;
                        card.OnWebType = 1;
                        card.PointIntegral = customer.AvailablePoint;
                        card.UserLevel = 0;
                        card.UserName = customer.Name;
                        int sysNo = DsMemberBo.Instance.InsertMembershipCard(card);
                    }
                    else
                    {
                        card = DsMemberBo.Instance.GetMembershipCardBySysNo(customer.Account);
                        card.PointIntegral = customer.AvailablePoint;
                    }
                    //List<DsPosOrderItem> items = DsPosManageBo.Instance.GetPosOrderItemBySerialNumber(numberNo);
                    result.Status = true;
                    result.Message = JsonUtil.ToJson2(card);
                }
                else
                {
                    result.Status = false;
                    result.Message = "未查询到当前会员信息。";
                }
            }
            return Json(result);
        }
        #endregion
        #region 获取经销商名称


        public JsonResult LoadDsDealerName(string key, string Mac, string PosName)
        {
            PosName = Server.UrlDecode(PosName);
            Result result = new Result();
            try
            {

                List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
                Model.Pos.DsPosManage nowMod = null;
                foreach (Model.Pos.DsPosManage mod in list)
                {
                    if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                    {
                        nowMod = mod;
                        break;
                    }
                }
                if (nowMod == null)
                {
                    result.Status = false;
                    result.Message = "Pos机有异，请检查是否有异常";
                }
                else
                {
                    result.Status = true;
                    result.Message = DsDealerBo.Instance.GetDsDealer(nowMod.pos_DsSysNo).DealerName;
                }

            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return Json(result);
        }
        #endregion


        #region 线下微信扫码支付

        string cusid = "990593054116638";
        string AppID = "00000514";
        string Key = "43df939f1e7f5c6909b3f4b63fg87ewl";
        string PayUrl = "https://vsp.allinpay.com/apiweb/weixin/pay";
        /// <summary>
        /// 微信支付单支付功能
        /// </summary>
        /// <param name="posOrder">订单信息</param>
        /// <param name="items">销售商品明细</param>
        /// <param name="auth_code">微信授权码</param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult WXPay(DBDsPosOrder posOrder, string items, string auth_code, string key, string Mac, string PosName)
        {
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                return Json(new { Status = false, Message = "Pos机有异，请检查是否有异常" });
            }
            else
            {

                posOrder.items = JsonUtil.ToObject<List<DsPosOrderItem>>(items);

                Dictionary<string, string> dicList = new Dictionary<string, string>();
                dicList.Add("cusid", cusid);
                dicList.Add("appid", AppID);
                dicList.Add("key", Key);
                dicList.Add("trxamt", Convert.ToInt32((posOrder.TotalPayValue - posOrder.TotalGetValue) * 100).ToString());
                dicList.Add("reqsn", posOrder.SerialNumber);
                dicList.Add("paytype", "3");
                dicList.Add("randomstr", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                dicList.Add("body", posOrder.SerialNumber);
                //dicList.Add("remark", "");
                //dicList.Add("validtime", DateTime.Now.ToString("yyyyMMddHHmmss"));
                dicList.Add("authcode", posOrder.PayAuthCode);
                //dicList.Add("openid", "");

                ///报文排序
                string paramStr = "";
                List<KeyValuePair<string, string>> lstorder = dicList.OrderBy(c => c.Key).ToList();
                foreach (KeyValuePair<string, string> item in lstorder)
                {
                    if (paramStr != "")
                    {
                        paramStr += "&";
                    }
                    paramStr += item.Key + "=" + item.Value;
                }
                string sign = Security.GetMD5(paramStr).Trim();
                paramStr += "&";
                paramStr += "sign=" + sign;
                string ResponseData = MyHttp.GetResponse(PayUrl, paramStr, "utf-8");
                WXPayResponse response = Util.Serialization.JsonUtil.ToObject<WXPayResponse>(ResponseData);
                if (response.retcode.ToUpper() == "SUCCESS")
                {
                    try
                    {
                        using (var trans = new TransactionScope())
                        {
                            if (string.IsNullOrEmpty(response.errmsg))
                            {
                                posOrder.PayType = "微信条码支付";
                                posOrder.PayTime = DateTime.Now;
                                int SysNo = DsPosOrderBo.Instance.Insert(posOrder);
                                foreach (DsPosOrderItem item in posOrder.items)
                                {
                                    item.pSysNo = SysNo;
                                    DsPosOrderBo.Instance.InsertItem(item);
                                }

                                ///付款历史记录
                                DsPosBarcodePayLog payLog = new DsPosBarcodePayLog();
                                payLog.DsSysNo = nowMod.pos_DsSysNo;
                                payLog.Content = ResponseData;
                                payLog.PayType = "微信条码支付";
                                payLog.PosOrderSysNo = 0;
                                payLog.ReceiptAmount = Convert.ToDecimal(posOrder.TotalSellValue);
                                payLog.TotalAmount = Convert.ToDecimal(posOrder.TotalSellValue);
                                payLog.TradeNo = response.chnltrxid;
                                payLog.PosOrderSysNo = SysNo;
                                DsPosBarcodePayLogBo.Instance.InnerDsPosBarcodePayLog(payLog);


                                trans.Complete();
                                return Json(new { Status = true, Message = "付款成功" });
                            }
                            else
                            {
                                return Json(new { Status = false, Message = response.errmsg });
                            }
                            
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(new { Status = false, Message = e.Message });
                    }
                    
                }
                ///付款历史记录
                DsPosBarcodePayLog payLog1 = new DsPosBarcodePayLog();
                payLog1.DsSysNo = nowMod.pos_DsSysNo;
                payLog1.Content = ResponseData;
                payLog1.PayType = "微信条码支付";
                payLog1.PosOrderSysNo = 0;
                payLog1.ReceiptAmount = Convert.ToDecimal(posOrder.TotalSellValue);
                payLog1.TotalAmount = Convert.ToDecimal(posOrder.TotalSellValue);
                payLog1.TradeNo = response.chnltrxid;
                //payLog1.PosOrderSysNo = SysNo;
                DsPosBarcodePayLogBo.Instance.InnerDsPosBarcodePayLog(payLog1);
                return Json(new { Status = false, Message = response.retmsg });
            }

        }

        string CancelUrl = "https://vsp.allinpay.com/apiweb/weixin/refund";
        public JsonResult WXRefund(CBDsPosReturnOrder retOrder, string items, string auth_code, string key, string Mac, string PosName)
        {
             List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                return Json(new { Status = false, Message = "Pos机有异，请检查是否有异常" });
            }
            else
            {
                DsPosOrder order = DsPosOrderBo.Instance.GetEntityBySellNumber(nowMod.pos_DsSysNo,retOrder.SellOrderNumber);
                if (order == null)
                {
                    return Json(new { Status = false, Message = "服务器没有销售单编号为：" + retOrder.SellOrderNumber+"的订单" });
                }
                else
                {
                    retOrder.Items = JsonUtil.ToObject<List<DsPosReturnOrderItem>>(items);
                    retOrder.OrderSysNo = order.SysNo;
                    Dictionary<string, string> dicList = new Dictionary<string, string>();
                    dicList.Add("cusid", cusid);
                    dicList.Add("appid", AppID);
                    dicList.Add("trxamt", Convert.ToInt32(retOrder.TotalReturnValue * 100).ToString());
                    dicList.Add("reqsn", retOrder.SerialNumber);
                    dicList.Add("oldreqsn", order.SerialNumber);
                    dicList.Add("oldreqsn", order.SerialNumber);
                    dicList.Add("randomstr", DateTime.Now.ToString("yyyyMMddHHmmssffff"));


                    ///报文排序
                    string paramStr = "";
                    List<KeyValuePair<string, string>> lstorder = dicList.OrderBy(c => c.Key).ToList();
                    foreach (KeyValuePair<string, string> item in lstorder)
                    {
                        if (paramStr != "")
                        {
                            paramStr += "&";
                        }
                        paramStr += item.Key + "=" + item.Value;
                    }
                    string sign = Security.GetMD5(paramStr).ToUpper().Trim();
                    paramStr += "&";
                    paramStr += "sign=" + sign;
                    string ResponseData = MyHttp.GetResponse(CancelUrl, paramStr, "utf-8");
                    WXPayResponse response = Util.Serialization.JsonUtil.ToObject<WXPayResponse>(ResponseData);
                    if (response.retcode.ToUpper() == "SUCCESS")
                    {
                        try
                        {
                            using (var trans = new TransactionScope())
                            {
                                retOrder.PayType = "微信条码支付";
                               
                                int SysNo = DsPosReturnOrderBo.Instance.Insert(retOrder);
                                foreach (DsPosReturnOrderItem item in retOrder.Items)
                                {
                                    item.pSysNo = SysNo;
                                    DsPosReturnOrderBo.Instance.InsertItem(item);
                                }

                                ///付款历史记录
                                DsPosBarcodePayLog payLog = new DsPosBarcodePayLog();
                                payLog.DsSysNo = nowMod.pos_DsSysNo;
                                payLog.Content = ResponseData;
                                payLog.PayType = "微信条码支付退款";
                                payLog.PosOrderSysNo = 0;
                                payLog.ReceiptAmount = Convert.ToDecimal(retOrder.TotalReturnValue);
                                payLog.TotalAmount = Convert.ToDecimal(retOrder.TotalReturnValue);
                                payLog.TradeNo = response.chnltrxid;
                                payLog.PosOrderSysNo = SysNo;
                                DsPosBarcodePayLogBo.Instance.InnerDsPosBarcodePayLog(payLog);

                                trans.Complete();
                                return Json(new { Status = true, Message = "退款成功" });
                            }
                        }
                        catch (Exception e) {
                            return Json(new { Status = false, Message = e.Message });
                        }
                    }
                    return Json(new { Status = false, Message = response.retmsg });
                }
            }
        }
        #endregion
        #region Pos机回调数据

        string PosCusId = "990581007420RTT";
        string PosAPPKEY = "81007420";
        string PosAPPID = "81007420";
        public JsonResult CreatePosOrderData(DsPosTLPosResult result, string key, string Mac, string PosName)
        {
             List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                return Json(new { Status = false, Message = "Pos机有异，请检查是否有异常" });
            }
            else
            {
                //var TLPos = DsPosTLPosResultBo.Instance.GetDsPosTLPosBizseq(result.bizseq, nowMod.Pos_TLTermid);

                result.PosKey = nowMod.SysNo.ToString();
                result.cusid = PosCusId;
                result.appid = PosAPPID;
                result.key = PosAPPKEY;
                result.termid = nowMod.Pos_TLTermid;
                result.trxcode = "T001";
                result.stauts = "-1";
                int sysno = DsPosTLPosResultBo.Instance.InsertMod(result);

                return Json(new { Status = true });
            }
        }

        public JsonResult GetTLPosDataSync(DsPosTLPosResult result)
        {
            DsPosTLPosResult mod = DsPosTLPosResultBo.Instance.GetDsPosTLPosBizseq( result.termid);

            POSResponse response = new POSResponse();
            decimal amount = 0;
           
            if (mod != null)
            {
                decimal.TryParse(mod.amount, out amount);
                if (mod.cusid == result.cusid && mod.appid == result.appid && mod.termid == result.termid)
                {
                    response.key = result.key;
                    response.amount = amount;
                    response.appid = mod.appid;
                    response.bizseq = mod.bizseq;
                    response.cusid = mod.cusid;
                    response.trxcode = "T001";
                    response.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    response.retcode = "0000";
                    response.trxreserve = result.trxreserve;
                    response.randomstr = mod.bizseq;

                    

                }
                else
                {
                    response.key = result.key;
                    response.amount = amount;
                    response.appid = result.appid;
                    response.bizseq = mod.bizseq;
                    response.cusid = result.cusid;
                    response.trxcode = "T001";
                    response.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    response.retcode = "9999";
                    response.trxreserve = result.trxreserve;
                    response.randomstr = mod.bizseq;
                    response.retmsg = "收银软件数据和POS机数据不一致";
                }
            }
            else
            {
                response.key = result.key;
                response.amount = amount;
                response.appid = PosAPPID;
                response.bizseq = result.bizseq;
                response.cusid = PosCusId;
                response.trxcode = "T001";
                response.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                response.retcode = "9999";
                response.trxreserve = result.trxreserve;
                response.randomstr = result.bizseq;
                response.retmsg = "未找到与当前订单数据";
            }

            Dictionary<string, string> dicList = new Dictionary<string, string>();
            dicList.Add("appid", response.appid);
            dicList.Add("cusid", response.cusid);
            dicList.Add("trxcode", response.trxcode);
            dicList.Add("timestamp", response.timestamp);
            dicList.Add("randomstr", response.randomstr);
            dicList.Add("bizseq", response.bizseq);
            dicList.Add("retcode", response.retcode);
            dicList.Add("key", response.key);
            if(!string.IsNullOrEmpty(response.retmsg))
            {
                dicList.Add("retmsg", response.retmsg);
            }
            dicList.Add("amount", response.amount.ToString());
            dicList.Add("trxreserve", response.trxreserve);

            ///报文排序
            string paramStr = "";
            List<KeyValuePair<string, string>> lstorder = dicList.OrderBy(c => c.Key).ToList();
            foreach (KeyValuePair<string, string> item in lstorder)
            {
                if (paramStr != "")
                {
                    paramStr += "&";
                }
                paramStr += item.Key + "=" + item.Value;
            }
            string sign = Security.GetMD5(paramStr).Trim();
            response.sign = sign;
           // response.errmsg=mod.
            return Json(response);
        }

        /// <summary>
        /// 结果回调连接
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ContentResult GetTLPosDataBack(DsPosTLPosResult result)
        {
            DsPosTLPosResult mod = DsPosTLPosResultBo.Instance.GetDsPosTLPosBizseq(result.bizseq, result.termid);
            mod.stauts = "0";
            if (mod != null)
            {
                DsPosTLPosResultBo.Instance.UpdateMod(mod);
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 查询销售单通联支付结果情况
        /// </summary>
        /// <param name="posOrder"></param>
        /// <param name="items"></param>
        /// <param name="auth_code"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult ClientGetPayOrderData(DBDsPosOrder posOrder, string items, string auth_code, string key, string Mac, string PosName)
        { 
            
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                return Json(new { Status = false, Message = "Pos机有异，请检查是否有异常" });
            }
            else
            {
                var TLPos = DsPosTLPosResultBo.Instance.GetDsPosTLPosResultData(posOrder.SerialNumber, nowMod.Pos_TLTermid, "0");
                if (TLPos != null)
                {
                    using (var tran = new TransactionScope())
                    {
                        posOrder.PayType = "通联支付";
                        posOrder.PayAuthCode = TLPos.traceno;
                        int sysNo = DsPosOrderBo.Instance.Insert(posOrder);
                        TLPos.stauts = "1";
                        TLPos.OrderSysNo = sysNo.ToString();
                        TLPos.SellType = 10;
                        DsPosTLPosResultBo.Instance.UpdateMod(TLPos);
                        tran.Complete();
                    }
                    return Json(new { Status = true, Message = "支付成功" });
                }
                else
                {
                    return Json(new { Status = false, Message = "等待支付回调" });
                }
            }
        
        }
        /// <summary>
        /// 查询退货通联退款结果情况
        /// </summary>
        /// <param name="posOrder"></param>
        /// <param name="items"></param>
        /// <param name="auth_code"></param>
        /// <param name="key"></param>
        /// <param name="Mac"></param>
        /// <param name="PosName"></param>
        /// <returns></returns>
        public JsonResult ClientGetPayReturnOrderData(CBDsPosReturnOrder retOrder, string items, string auth_code, string key, string Mac, string PosName)
        {
            List<Model.Pos.DsPosManage> list = DsPosManageBo.Instance.GetEntityListByPosKey(key);
            Model.Pos.DsPosManage nowMod = null;
            foreach (Model.Pos.DsPosManage mod in list)
            {
                if (mod.pos_posName == PosName && mod.pos_MacData == Mac)
                {
                    nowMod = mod;
                    break;
                }
            }
            if (nowMod == null)
            {
                return Json(new { Status = false, Message = "Pos机有异，请检查是否有异常" });
            }
            else
            {
                var TLPos = DsPosTLPosResultBo.Instance.GetDsPosTLPosResultData(retOrder.SerialNumber, nowMod.Pos_TLTermid, "0");
                if (TLPos != null)
                {
                    using (var tran = new TransactionScope())
                    {
                        retOrder.PayType = "通联支付";
                        retOrder.PayAuthCode = TLPos.traceno;
                        int sysNo = DsPosReturnOrderBo.Instance.Insert(retOrder);
                        TLPos.stauts = "1";
                        TLPos.OrderSysNo = sysNo.ToString();
                        TLPos.SellType = 20;
                        DsPosTLPosResultBo.Instance.UpdateMod(TLPos);
                        tran.Complete();
                    }
                    return Json(new { Status = true, Message = "退款成功" });
                }
                else
                {
                    return Json(new { Status = false, Message = "等待支付回调" });
                }
            }

        }

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    class WXPayResponse
    {
        public string retcode { get; set; }
        public string retmsg { get; set; }
        public string cusid { get; set; }
        public string timestamp { get; set; }
        public string trxcode { get; set; }
        public string appid { get; set; }
        public string trxid { get; set; }
        public string chnltrxid { get; set; }
        public string reqsn { get; set; }
        public string randomstr { get; set; }
        public string errmsg { get; set; }
        public string sign { get; set; }
        public string bizseq { get; set; }

        public decimal amount { get; set; }

        public string trxreserve { get; set; }

    }
    class POSResponse
    {
        public string retcode { get; set; }
        public string retmsg { get; set; }
        public string cusid { get; set; }
        public string timestamp { get; set; }
        public string trxcode { get; set; }
        public string appid { get; set; }
       
        public string randomstr { get; set; }
      
        public string sign { get; set; }
        public string bizseq { get; set; }

        public decimal amount { get; set; }

        public string trxreserve { get; set; }
        public string key { get; set; }

    }
}
