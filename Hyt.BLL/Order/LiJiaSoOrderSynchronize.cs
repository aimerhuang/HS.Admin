using Extra.Erp.Eas.SaleIssueBillFacade;
using Hyt.BLL.Log;
using Hyt.Model.Generated;
using Hyt.Model.LiJiaModel;
using Hyt.Model.Order;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Mvc;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 利嘉订单订单同步
    /// </summary>
    /// 2017-05-19 罗勤尧 创建
    public class LiJiaSoOrderSynchronize
    {
        //服务器地址
        public static string host = "http://szxyerp.nitago.com/aar/wwwroot/index.aardio?_method=";

        //修改分销商接口
        //public string pathUpdateDsDealer = "member.update";
        public static string Token = "057956B16BE5F60AE503A86BA13C7B41";
        public static String methodTs = "POST";
        //string postData = "";

        #region 会员接口
        /// <summary>
        /// 利嘉新增会员
        /// </summary>
        /// <param name="info">会员信息</param>
        /// <returns>利嘉会员系统编号</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia AddLiJiaMemerber(DsDealerLiJia info)
        {
            //创建新增分销商请求参数
            string addds = JsonHelper.ToJson(info);
           
            //新增分销商接口
            string pathAddDsDealer = "member.add";
            string postData = "";
            int id = 0;
            ResultLiJia Result = null;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAddDsDealer, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {

                    Result = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Result.Success)
                    {
                        return Result;
                    }else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result = new ResultLiJia()
                {
                    Success = false,
                    Message = ex.ToString()
                };
            }
            return Result;
        }


        //string postData = "";
        /// <summary>
        /// 利嘉修改会员信息
        /// </summary>
        /// <param name="info">会员信息</param>
        /// <returns>是否修改成</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool UpdateLiJiaMemerber(DsDealerLiJiaEdit info)
        {
            //创建新增分销商请求参数
            string UpdateJson = JsonHelper.ToJson(info);
            //修改分销商接口
            string pathUpdateDsDealer = "member.update";
            string postData = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathUpdateDsDealer, methodTs, Token, postData, UpdateJson);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉查询会员信息
        /// </summary>
        /// <param name="info">查询数据</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static MemberSerch SeachLiJiaMemerber(LiJiaMemberSearch info)
        {
            //创建查询分销商请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询分销商会员接口
            //Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(searchjosn, "LiJia");
            string searchpath = "member.search";
            string postData = "";
            bool Result = false;
            MemberSerch resultList = new MemberSerch();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    //Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    var Assreturn = JsonSerializationHelper.JsonToObject<MemberSerch>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(searchjosn, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }

        #endregion
        //string postData = "";

        #region 订单接口
        /// <summary>
        /// 利嘉新增订单
        /// </summary>
        /// <param name="info">订单编号</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia AddLiJiaOrder(LiJiaOrderModel info)
        {
            //创建新增分销商请求参数 
            string addorderstr = JsonHelper.ToJson(info);
            //新增订单接口
            //Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addorderstr, "LiJia");
            string addorderpath = "orderInfo.add";
            string postData = "";
            //bool Result = false;
            ResultLiJia Result=null;
            try
            {
                string responseAddDsDealer = MainHttpT(host, addorderpath, methodTs, Token, postData, addorderstr);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {

                     Result = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                     if (Result.Success)
                    {
                        return Result;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addorderstr, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result = new ResultLiJia()
                    {
                        Success=false,
                        Message = ex.ToString()
                    };
            }
            return Result;
        }


        //string postData = "";
        /// <summary>
        /// 利嘉查询订单
        /// </summary>
        /// <param name="info">订单信息</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia SeachLiJiaOrder(LiJiaMemberSearch info)
        {
            //创建查询请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询接口
            string searchpath = "orderInfo.search";
            string postData = "";
            bool Result = false;
            ResultLiJia resultList = new ResultLiJia();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }

        /// <summary>
        /// 利嘉取消订单
        /// </summary>
        /// <param name="info">订单编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool CancelLiJiaOrder(LiJiaOrderCancel info)
        {
            //创建新增分销商请求参数 
            string addorderstr = JsonHelper.ToJson(info);
            //新增订单接口
            //Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addorderstr, "LiJia");
            string addorderpath = "orderInfo.cancel";
            string postData = "";
            bool Result = false;

            try
            {
                string responseAddDsDealer = MainHttpT(host, addorderpath, methodTs, Token, postData, addorderstr);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                   // Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addorderstr, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }
        #endregion


        #region 商品分类接口
        /// <summary>
        /// 利嘉新增商品分类
        /// </summary>
        /// <param name="info">商品分类信息</param>
        /// <returns>利嘉商品分类系统编号</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static int AddLiJiaProductCategory(ProductCategoryLiJia info)
        {
            //创建新增商品分类请求参数
            string addds = JsonHelper.ToJson(info);
            //新增新增商品分类接口
            string pathAddPdCa = "category.add";
            string postData = "";
            int id = 0;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAddPdCa, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        id = Assreturn.CategoryId;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return id;
            }
            return id;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉修改商品分类
        /// </summary>
        /// <param name="info">商品分类信息</param>
        /// <returns>是否修改成</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool UpdateLiJiaProductCategory(CategoryLiJiaEdit info)
        {
            //创建修改商品分类请求参数
            string UpdateJson = JsonHelper.ToJson(info);
            //修改商品分类接口
            string pathUpdateDsDealer = "category.update";
            string postData = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathUpdateDsDealer, methodTs, Token, postData, UpdateJson);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(UpdateJson, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉查询商品分类
        /// </summary>
        /// <param name="info">商品分类</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia SeachLiJiaProductCategory(LiJiaMemberSearch info)
        {
            //创建查询请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询接口
            string searchpath = "category.search";
            string postData = "";
            bool Result = false;
            ResultLiJia resultList = new ResultLiJia();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(searchjosn, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }
        #endregion


        #region 商品接口

        #endregion

        #region 新增采购入库单接口
        /// <summary>
        /// 利嘉采购入库单
        /// </summary>
        /// <param name="info">采购入库单信息</param>
        /// <returns>利嘉采购入库单系统编号</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static string AddLiJiaPurchaseInfo(LiJiaPurchaseInfo Info)
        {
            //创建新增采购入库单请求参数
            string addds = JsonHelper.ToJson(Info);
            //新增采购入库单接口
            string pathAddPdCa = "purchaseInfo.add";
            string postData = "";
            string Code = "";
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAddPdCa, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Code = Assreturn.PurchaseInboundOrderNo;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Code;
            }
            return Code;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉修改采购入库单
        /// </summary>
        /// <param name="info">采购入库单信息</param>
        /// <returns>是否修改成</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool UpdateLiJiaPurchaseInfo(LiJiaStoreEdit info)
        {
            //创建修改采购入库单请求参数
            string UpdateJson = JsonHelper.ToJson(info);
            //修改采购入库单接口
            string pathUpdateDsDealer = "store.update";
            string postData = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathUpdateDsDealer, methodTs, Token, postData, UpdateJson);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉查询采购入库单
        /// </summary>
        /// <param name="info">采购入库单</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia SeachLiJiaPurchaseInfo(LiJiaMemberSearch info)
        {
            //创建查询请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询接口
            string searchpath = "store.search";
            string postData = "";
            bool Result = false;
            ResultLiJia resultList = new ResultLiJia();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }
        #endregion

        #region 订单出库接口
        /// <summary>
        /// 利嘉订单出库发货接口
        /// </summary>
        /// <param name="info">出库信息</param>
        /// <returns>是否成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool LiJiaOrderInfoTracking(OrderInfoTracking Info)
        {
            //创建出库发货请求参数
            string addds = JsonHelper.ToJson(Info);
            //出库发货接口
            string pathAddPdCa = "orderInfo.tracking";
            string postData = "";
            string Code = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAddPdCa, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }
        #endregion

        #region 仓库接口
        /// <summary>
        /// 利嘉新增仓库
        /// </summary>
        /// <param name="info">仓库信息</param>
        /// <returns>利嘉仓库系统编号</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static string AddLiJiaStore(LiJiaStoreAdd info)
        {
            //创建新增仓库请求参数
            string addds = JsonHelper.ToJson(info);
            //新增仓库接口
            string pathAddPdCa = "store.add";
            string postData = "";
            string StoreCode = "";
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAddPdCa, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        StoreCode = Assreturn.StoreCode;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return StoreCode;
            }
            return StoreCode;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉修改仓库
        /// </summary>
        /// <param name="info">仓库信息</param>
        /// <returns>是否修改成</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool UpdateLiJiaStore(LiJiaStoreEdit info)
        {
            //创建修改仓库请求参数
            string UpdateJson = JsonHelper.ToJson(info);
            //修改仓库接口
            string pathUpdateDsDealer = "store.update";
            string postData = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathUpdateDsDealer, methodTs, Token, postData, UpdateJson);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉查询仓库
        /// </summary>
        /// <param name="info">仓库</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia SeachLiJiaStore(LiJiaMemberSearch info)
        {
            //创建查询请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询接口
            string searchpath = "store.search";
            string postData = "";
            bool Result = false;
            ResultLiJia resultList = new ResultLiJia();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }
        #endregion


        #region 商品品牌接口
        /// <summary>
        /// 利嘉新增商品品牌
        /// </summary>
        /// <param name="info">商品品牌信息</param>
        /// <returns>利嘉商品品牌系统编号</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static int AddLiJiaBrand(LiJiaBrandAdd info)
        {
            //创建新增商品品牌请求参数
            string addds = JsonHelper.ToJson(info);
            //新增商品品牌接口
            string pathAdd = "brand.add";
            string postData = "";
            int id = 0;
            string BraCode = "";
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathAdd, methodTs, Token, postData, addds);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        id = Assreturn.BrandId;
                        BraCode = Assreturn.BrandCode;
                    }
                    else
                    {
                        //记录失败日志
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "LiJia");
                        Hyt.BLL.Log.LocalLogBo.Instance.WriteReturn(responseAddDsDealer, "LiJia");
                    }
                }
            }
            catch (Exception ex)
            {
                return id;
            }
            return id;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉修改商品品牌
        /// </summary>
        /// <param name="info">商品品牌信息</param>
        /// <returns>是否修改成</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static bool UpdateLiJiaBrand(LiJiaBrandEdit info)
        {
            //创建修改商品品牌请求参数
            string UpdateJson = JsonHelper.ToJson(info);
            //修改商品品牌接口
            string pathUpdateDsDealer = "brand.update";
            string postData = "";
            bool Result = false;
            try
            {
                string responseAddDsDealer = MainHttpT(host, pathUpdateDsDealer, methodTs, Token, postData, UpdateJson);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return Result;
            }
            return Result;
        }

        //string postData = "";
        /// <summary>
        /// 利嘉查询商品品牌
        /// </summary>
        /// <param name="info">商品品牌查询参数</param>
        /// <returns>结果对象</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public static ResultLiJia SeachLiJiaBrand(LiJiaMemberSearch info)
        {
            //创建查询请求参数
            string searchjosn = JsonHelper.ToJson(info);
            //查询接口
            string searchpath = "brand.search";
            string postData = "";
            bool Result = false;
            ResultLiJia resultList = new ResultLiJia();
            try
            {
                string responseAddDsDealer = MainHttp(host, searchpath, methodTs, Token, postData, searchjosn);
                if (!string.IsNullOrEmpty(responseAddDsDealer))
                {
                    var Assreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(responseAddDsDealer);
                    if (Assreturn.Success)
                    {
                        resultList = Assreturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return resultList;
            }
            return resultList;
        }
        #endregion
        public static string MainHttpT(string host, string path, string method, string Token, string querys, string bodys)
        {
            //String querys = "preCarNum=%E4%BA%AC&province=%E5%8C%97%E4%BA%AC";
            //String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            //httpRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.Method = method;
            httpRequest.Headers.Add("Token", Token);
            //httpRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string retString = reader.ReadToEnd();
            //Console.WriteLine(reader.ReadToEnd());
            //Console.WriteLine("\n");
            reader.Close();
            st.Close();
            return retString;

        }
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="strData">Post数据</param>
        /// <returns></returns>
        public static string POSTSend(string host, string path, string method, string Token, string querys, string strData)
        {
            String url = host + path;
            string retString = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Token", Token);
            byte[] bytes = Encoding.UTF8.GetBytes(strData);
            request.ContentLength = bytes.Length;
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = 5;
            request.Timeout = 20000;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
            request.Proxy = null;

            bool IsIn = true;
            try
            {
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(bytes, 0, bytes.Length);
                myRequestStream.Close();
            }
            catch (WebException wex)
            {
                retString = wex.Status.ToString();
                IsIn = false;
            }
            if (IsIn == false)
            {
                return "访问超时.";
            }

            StreamReader myStreamReader = null;
            Stream myResponseStream = null;

            WebResponse wr = null;

            try
            {
                wr = request.GetResponse();
            }
            catch (WebException wex)
            {
                retString = wex.Status.ToString();
            }

            if (wr != null)
            {
                try
                {

                    HttpWebResponse response = (HttpWebResponse)wr;
                    myResponseStream = response.GetResponseStream();
                    if (myResponseStream != null)
                    {
                        myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                        retString = myStreamReader.ReadToEnd();

                        if (retString == null)
                        {
                            retString = string.Empty;
                        }
                    }

                }
                catch (Exception ex)
                {
                    retString = ex.Message;
                }
                finally
                {
                    myStreamReader.Close();
                    myResponseStream.Close();

                }
            }

            return retString;
        }
        public static string MainHttp(string host, string path, string method, string Token, string querys, string bodys)
        {
            //String querys = "preCarNum=%E4%BA%AC&province=%E5%8C%97%E4%BA%AC";
            //String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            //httpRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.Method = method;
            httpRequest.Headers.Add("Token", Token);
            //httpRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            //Stream st = httpResponse.GetResponseStream();
            //MemoryStream ms = new MemoryStream();
            //byte[] b = new byte[4096];
            //int len;
            //while ((len = st.Read(b, 0, b.Length)) > 0)
            //    ms.Write(b, 0, len);
            //string strNew = Encoding.UTF8.GetString(b);//将字节数组转换为字符串
            //StreamReader reader = new StreamReader(ms, Encoding.GetEncoding("utf-8"));
            //string retString = reader.ReadToEnd();
            //Console.WriteLine(reader.ReadToEnd());
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string retString = reader.ReadToEnd();
            reader.Close();
            st.Close();
            //Console.WriteLine("\n");
            return retString;

        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
