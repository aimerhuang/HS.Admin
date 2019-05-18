using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Web;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.CRM;
using Hyt.BLL.Finance;
using Hyt.BLL.Log;
using Hyt.BLL.Promotion;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Caching;
using Hyt.Model.LogisApp;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.LogisApp;
using Hyt.BLL.Sys;
using Hyt.BLL.Logistics;
using Hyt.Model;
using Hyt.Model.Transfer;
using System.ServiceModel.Activation;
using Hyt.Util;
using Hyt.Util.Validator;
using CrCustomerBo = Hyt.BLL.CRM.CrCustomerBo;
using PdCategoryBo = Hyt.BLL.Product.PdCategoryBo;
using PdProductBo = Hyt.BLL.Product.PdProductBo;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;
using Hyt.BLL.Product;
using Newtonsoft.Json;
using Hyt.BLL.Order;

using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Hyt.Util.Net;
using System.Xml;
using Hyt.Model.Common;
namespace Hyt.Service.Implement.LogisApp
{
    /// <summary>
    /// 物流APP服务
    /// </summary>
    /// <remarks>2014-01-08 周唐炬 添加注释</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Logistics : ILogistics
    {
        //附件ftp信息
        //private readonly AttachmentConfig _attachmentConfig = BLL.Config.Config.Instance.GetAttachmentConfig();
        ////图片配置
        //private readonly ProductImageConfig _imageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig();
        //private readonly AttachmentConfig _attrConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();

        #region 配送员 (周唐炬)

        /// <summary>
        /// 用户认证信息
        /// </summary>
        /// <returns>是否登录</returns>
        /// <remarks>2013-06-27 周唐炬 创建</remarks>
        private Result<SyUser> Authorization
        {
            get
            {
                //未登录成功消息码为 -2
                var result = new Result<SyUser> { StatusCode = -2 };
                if (null != HttpContext.Current.Session["SysUser"])
                {
                    var syUser = HttpContext.Current.Session["SysUser"] as SyUser;
                    result.Data = syUser;

                    result.StatusCode = 0;
                    result.Status = true;
                    result.Message = "用户已经登录!";
                }
                else
                {
                    result.Message = "用户尚未登录!";
                }
                return result;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <returns>返回登录结果</returns>
        /// <remarks>2013-06-19 周唐炬 创建
        /// 2013-07-18 黄伟 修改
        /// </remarks>
        public Result Login(string account, string password)
        {
            var result = new Result { Status = false, StatusCode = -1 };
            try
            {
                var loginResult = AdminAuthenticationBo.Instance.Login(account, password);
                if (loginResult != null && loginResult.Status)
                {
                    var user = loginResult.Data;
                    var userGroupList = SyUserBo.Instance.GetGroupUser(user.SysNo);
                    if (userGroupList != null && userGroupList.Any())
                    {
                        if (userGroupList.Any(userGroup => userGroup.GroupSysNo == UserGroup.业务员组))
                        {
                            HttpContext.Current.Session.Add("SysUser", user);
                            CookieUtil.SetCookie(Constant.ADMIN_LOGINHISTORYUSERNAME_COOKIE, user.UserName, DateTime.Now.AddDays(Constant.ADMIN_LOGINHISTORYUSERNAME_COOKIE_EXPIRY));

                            SysLog.Instance.Info(LogStatus.系统日志来源.物流App, string.Format("{0}用户登录成功!", user.Account),
                                                 LogStatus.系统日志目标类型.用户, 0, user.SysNo);
                            result.Status = loginResult.Status;
                            result.StatusCode = loginResult.StatusCode;
                        }
                        else
                        {
                            result.Message = "用户不属于业务员组，不能登录！";
                            SysLog.Instance.Info(LogStatus.系统日志来源.物流App, string.Format("{0}用户不属于业务员组，不能登录！！", user.Account),
                                                 LogStatus.系统日志目标类型.用户, 0, user.SysNo);
                        }
                    }
                    else
                    {
                        result.Message = "用户不属于业务员组，不能登录！";
                        SysLog.Instance.Info(LogStatus.系统日志来源.物流App, string.Format("{0}用户不属于业务员组，不能登录！！", user.Account),
                                             LogStatus.系统日志目标类型.用户, 0, user.SysNo);
                    }
                }
                else
                {
                    result.Message = loginResult == null ? "登录失败" : loginResult.Message;
                    HttpContext.Current.Session.Clear();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "用户登录" + ex.Message, LogStatus.系统日志目标类型.用户, 0, ex);
            }

            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-19 周唐炬 创建</remarks>
        public Result ModifyPassword(string oldPassword, string newPassword)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    //WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized; 
                    var account = Authorization.Data.Account;
                    if (!string.IsNullOrWhiteSpace(account))
                    {
                        result = SyUserBo.Instance.ModifyPassword(account, oldPassword, newPassword);
                        SysLog.Instance.Info(LogStatus.系统日志来源.物流App,
                                             string.Format("{0}修改密码!{1}", account, result.Message),
                                             LogStatus.系统日志目标类型.用户, 0, Authorization.Data.SysNo);
                    }
                    else
                    {
                        result.Message = "用户还未登录或错误,请重新登录或重试!";
                        SysLog.Instance.Info(LogStatus.系统日志来源.物流App, result.Message,
                                             LogStatus.系统日志目标类型.用户, 0, Authorization.Data.SysNo);
                    }
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.Message = Authorization.Message;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "修改密码" + ex.Message, LogStatus.系统日志目标类型.用户, 0, ex);
            }

            return result;
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param></param>
        /// <returns>用户登出结果</returns>
        /// <remarks>2013-06-19 周唐炬 创建</remarks>
        public Result LogOut()
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    SysLog.Instance.Info(LogStatus.系统日志来源.物流App,
                                         string.Format("{0}用户注销!", Authorization.Data.Account), LogStatus.系统日志目标类型.用户, 0,
                                         Authorization.Data.SysNo);
                    HttpContext.Current.Session.Clear();
                    Authorization.Status = false;
                    result.StatusCode = 0;
                    result.Status = true;
                    result.Message = "用户注销成功!";
                }
                else
                {
                    result = Authorization;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "修改密码" + ex.Message, LogStatus.系统日志目标类型.用户, 0, ex);
            }
            return result;
        }

        /// <summary>
        /// 更新GPS位置数据
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        /// <param name="gpsDate">定位时间</param>
        /// <param name="locationType">定位类型</param>
        /// <param name="radius">误差</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>       
        public Result UpdateGpsForJson(decimal latitude, decimal longitude, string gpsDate, int locationType,
                                       float radius)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var data = new LgDeliveryUserLocation
                        {
                            Latitude = latitude,
                            Longitude = longitude,
                            GpsDate = Convert.ToDateTime(gpsDate),
                            LocationType = locationType,
                            Radius = Convert.ToDecimal(radius)
                        };

                    if (null != Authorization.Data)
                    {
                        var accountSysNo = Authorization.Data.SysNo;
                        data.DeliveryUserSysNo = accountSysNo;
                    }

                    data.CreatedDate = DateTime.Now;
                    var id = DeliveryUserLocationBo.Instance.Create(data);
                    if (id > 0)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.Message = Authorization.Message;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "位置数据" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 批量更新GPS位置数据
        /// </summary>
        /// <param name="list">批量GPS数据</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>
        public Result BatchUpdateGpsForJson(List<APPCBLgDeliveryUserLocation> list)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var accountSysNo = Authorization.Data.SysNo;
                    if (null != list && list.Any())
                    {
                        list.ForEach(x =>
                            {
                                var userLocation = new LgDeliveryUserLocation
                                {
                                    DeliveryUserSysNo = accountSysNo,
                                    CreatedDate = DateTime.Now,
                                    GpsDate = Convert.ToDateTime(x.GpsDate),
                                    Latitude = x.Latitude,
                                    Longitude = x.Longitude,
                                    Radius = x.Radius,
                                    LocationType = x.LocationType
                                };
                                DeliveryUserLocationBo.Instance.Create(userLocation);
                            });
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.Message = Authorization.Message;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// APP上传图片保存
        /// </summary>
        /// <param name="noteType">单据类型(出库单/取货单)</param>
        /// <param name="noteSysNo">单据系统编号</param>
        /// <param name="imgBase64">图片Base64编码</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>     
        public Result UploadSign(int noteType, int noteSysNo, string imgBase64)
        {
            var result = new Result { StatusCode = -1 };
          
            try
            {
                if (Authorization.Status)
                {
                    if (noteSysNo <= 0)
                    {
                        result.Message = "单据系统编号有误！";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(imgBase64))
                        {
                            result.Message = "图片数据不能为空！";
                        }
                        else
                        {
                            var fileName = string.Format("{0}.jpg", EncryptionUtil.EncryptWithMd5(noteType + "-" + noteSysNo));
                            var inputBytes = Convert.FromBase64String(imgBase64);
                            var saveFolder = string.Format("App\\Logistics\\{0}\\{1}\\", fileName.Substring(0, 1), fileName.Substring(1, 2));

                            using (var service = new Infrastructure.Communication.ServiceProxy<Contract.FileProcessor.IUploadService>())
                            {
                                result.Status = service.Channel.UploadFile(saveFolder, fileName, inputBytes);
                            }

                            if (result.Status)
                            {
                                if (LogisticsStatus.配送单据类型.出库单.GetHashCode() == noteType)
                                {
                                    var stockOut = BLL.Warehouse.WhWarehouseBo.Instance.Get(noteSysNo);
                                    if (stockOut != null)
                                    {
                                        stockOut.SignImage = fileName;
                                        BLL.Warehouse.WhWarehouseBo.Instance.UpdateStockOut(stockOut);
                                        result.StatusCode = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.Message = Authorization.Message;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "上传图片保存" + ex.Message, ex);
            }

            return result;
        }

        #endregion

        #region 会员相关业务 (周唐炬)

        /// <summary>
        /// 获取某地地区的子地区数据
        /// </summary>
        /// <param name="areaSysNo">地区父级系统号</param>
        /// <returns>省市区数据</returns>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        public Result<IList<CBBsArea>> SelectArea(int areaSysNo)
        {
            var result = new Result<IList<CBBsArea>> { StatusCode = -1 };

            try
            {
                if (Authorization.Status)
                {
                    var list = BasicAreaBo.Instance.SelectArea(areaSysNo).Select(x =>
                                                                                 new CBBsArea
                                                                                     {
                                                                                         SysNo = x.SysNo,
                                                                                         ParentSysNo = x.ParentSysNo,
                                                                                         AreaName = x.AreaName,
                                                                                         AreaCode = x.AreaCode,
                                                                                         AreaLevel = x.AreaLevel
                                                                                     }).ToList();
                    result.Data = list;
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取省市区数据" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获取用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>用户的收货地址列表</returns>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        public Result<IList<AppCrReceiveAddress>> GetCustomerAddressList(int customerSysNo)
        {
            var result = new Result<IList<AppCrReceiveAddress>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var list = SoOrderBo.Instance.LoadCustomerAddress(customerSysNo).Select(x =>
                                                                                            new AppCrReceiveAddress
                                                                                                {
                                                                                                    SysNo = x.AreaSysNo,
                                                                                                    CustomerSysNo = x.CustomerSysNo,
                                                                                                    AreaSysNo = x.AreaSysNo,
                                                                                                    StreetAddress = x.StreetAddress,
                                                                                                    AreaNameList = GetFullAreaName(x.AreaSysNo),
                                                                                                    Name = x.Name,
                                                                                                    PhoneNumber = x.PhoneNumber,
                                                                                                    MobilePhoneNumber = x.MobilePhoneNumber,
                                                                                                    ZipCode = x.ZipCode
                                                                                                }).ToList();
                    result.Data = list;
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取用户的收货地址" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 根据区县编号获取省市区全称
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>地址全称</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        private string GetFullAreaName(int sysNo)
        {
            var strList = new List<string>();
            BsArea area;
            BsArea city;
            var province = BasicAreaBo.Instance.GetProvinceEntity(sysNo, out city, out area);
            if (province != null)
            {
                strList.Add(province.AreaName);
            }
            if (city != null)
            {
                strList.Add(city.AreaName);
            }
            if (area != null)
            {
                strList.Add(area.AreaName);
            }
            return string.Join(" ", strList);
        }

        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="customerInfo">会员实体</param>
        /// <returns>返回结果</returns>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        public Result<int> CreateCustomer(AppCBCrCustomer customerInfo)
        {
            var result = new Result<int> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    if (VHelper.Do(customerInfo.MobilePhoneNumber, VType.Mobile))
                    {
                        if (BLL.Web.CrCustomerBo.Instance.GetCustomerByCellphone(customerInfo.MobilePhoneNumber) != null)
                        {
                            result.Message = "该手机号已经注册！";
                        }
                        else
                        {
                            const string password = "123456";
                            var customer = new CrCustomer
                            {
                                Account = customerInfo.MobilePhoneNumber,
                                Password =password,  //EncryptionUtil.EncryptWithMd5AndSalt(password), 余勇修改 2014-09-12
                                Name = customerInfo.Name,
                                StreetAddress = customerInfo.StreetAddress,
                                MobilePhoneNumber = customerInfo.MobilePhoneNumber,
                                EmailStatus = customerInfo.EmailStatus,
                                MobilePhoneStatus = customerInfo.MobilePhoneStatus,
                                RegisterDate = DateTime.Now,
                                LevelSysNo = CustomerLevel.初级,
                                LevelPoint = 0,
                                AvailablePoint = 0,
                                ExperienceCoin = 0,
                                ExperiencePoint = 0,
                                RegisterSourceSysNo = Authorization.Data.SysNo.ToString(CultureInfo.InvariantCulture),
                                RegisterSource = CustomerStatus.注册来源.物流App.GetHashCode(),
                                IsReceiveEmail = CustomerStatus.是否接收邮件.是.GetHashCode(),
                                IsReceiveShortMessage = CustomerStatus.是否接收短信.是.GetHashCode(),
                                IsExperienceCoinFixed = CustomerStatus.惠源币是否固定.不固定.GetHashCode(),
                                IsExperiencePointFixed = CustomerStatus.经验积分是否固定.不固定.GetHashCode(),
                                IsLevelFixed = CustomerStatus.等级是否固定.不固定.GetHashCode(),
                                Status = CustomerStatus.会员状态.有效.GetHashCode()
                            };

                            var address = new CrReceiveAddress
                            {
                                ZipCode = customerInfo.ZipCode,
                                AreaSysNo = customerInfo.AreaSysNo,
                                Name = customerInfo.Name,
                                MobilePhoneNumber = customerInfo.MobilePhoneNumber,
                                StreetAddress = customerInfo.StreetAddress,
                                IsDefault = 1
                            };
                            if (SoOrderBo.Instance.CreateCustomer(customer, address))
                            {
                                BLL.Extras.SmsBO.Instance.发送注册成功短信(customer.MobilePhoneNumber, customer.Account, password);

                                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建会员",
                                                     LogStatus.系统日志目标类型.客户管理, Authorization.Data.SysNo, null, WebUtil.GetUserIp(),
                                                     Authorization.Data.SysNo);
                                result.Status = true;
                                result.StatusCode = 0;
                                result.Data = customer.SysNo;
                            }
                            else
                            {
                                result.Status = false;
                                result.Message = "单点登录服务调用失败，请联系系统管理员。";
                            }
                        }
                    }
                    else
                    {
                        result.Message = "请输入有效的会员手机号！";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "添加会员" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 查询会员(模糊查询)
        /// </summary>
        /// <param name="keyword">姓名、手机号</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013-07-10 周唐炬 创建</remarks>
        public Result<IList<AppCBCrCustomer>> SearchCustomer(string keyword)
        {
            var result = new Result<IList<AppCBCrCustomer>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        var list = SoOrderBo.Instance.SearchCustomer(keyword, 20).Select(x =>
                                                                                            new AppCBCrCustomer
                                                                                                {
                                                                                                    SysNo = x.SysNo,
                                                                                                    Name = x.Name,
                                                                                                    MobilePhoneNumber = x.MobilePhoneNumber,
                                                                                                    StreetAddress = x.StreetAddress
                                                                                                }).ToList();
                        if (list.Any())
                        {
                            list.ForEach(x =>
                                {
                                    var defaultAddress = SoOrderBo.Instance.LoadCustomerDefaultAddress(x.SysNo);
                                    if (defaultAddress != null)
                                    {
                                        x.DefaultAddress = new AppCrReceiveAddress
                                            {
                                                SysNo = defaultAddress.AreaSysNo,
                                                CustomerSysNo = defaultAddress.CustomerSysNo,
                                                AreaSysNo = defaultAddress.AreaSysNo,
                                                StreetAddress = defaultAddress.StreetAddress,
                                                AreaNameList = GetFullAreaName(defaultAddress.AreaSysNo),
                                                Name = defaultAddress.Name,
                                                PhoneNumber = defaultAddress.PhoneNumber,
                                                MobilePhoneNumber = defaultAddress.MobilePhoneNumber,
                                                ZipCode = defaultAddress.ZipCode
                                            };
                                    }
                                });
                        }
                        result.Data = list;
                        result.Status = true;
                        result.StatusCode = 0;
                    }
                    else
                    {
                        result.Message = "关键字不能为空！";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "查询会员信息" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="receiveAddress">会员收货地址</param>
        /// <returns>返回结果</returns>
        /// <remarks>2014-02-28 周唐炬 创建</remarks>
        public Result ReceiveAddressCreate(AppCrReceiveAddress receiveAddress)
        {
            var result = new Result { StatusCode = -1 };
            if (receiveAddress != null)
            {
                if (receiveAddress.CustomerSysNo <= 0)
                {
                    result.Message = "会员信息错误!";
                }
                else
                {
                    var address = new CrReceiveAddress
                    {
                        CustomerSysNo = receiveAddress.CustomerSysNo,
                        AreaSysNo = receiveAddress.AreaSysNo,
                        StreetAddress = receiveAddress.StreetAddress,
                        Name = receiveAddress.Name,
                        PhoneNumber = receiveAddress.PhoneNumber,
                        MobilePhoneNumber = receiveAddress.MobilePhoneNumber,
                        ZipCode = receiveAddress.ZipCode
                    };
                    if (SoOrderBo.Instance.InsertReceiveAddress(address) > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        result.Message = "添加会员收货地址成功!";
                    }
                }
            }
            return result;
        }

        #endregion

        #region 商品 (周唐炬)
        /// <summary>
        /// 获取业务员最低限价
        /// </summary>
        /// <param name="productSysnos">产品编号(逗号分隔)</param>
        /// <returns> 
        /// [
        ///     {SysNo:111,Price:xxx},
        /// ]
        /// </returns>
        /// <remarks>2014-09-16 朱成果 创建</remarks>
        public Result<String> SelectProductLowestPrice(string productSysnos)
        {
            var result = new Result<String> { StatusCode = -1, Status = false };
            try
            {
                List<KeyValuePair<int, decimal>> lst = new List<KeyValuePair<int, decimal>>();
                var productids = productSysnos.Split(',').Select(m => int.Parse(m)).ToList();
                if (productids != null)
                {
                    productids.ForEach(p =>
                    {

                        var item = PdPriceBo.Instance.GetProductPrice(p, new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.业务员销售限价 }).FirstOrDefault();
                        KeyValuePair<int, decimal> pitem = new KeyValuePair<int, decimal>(p, item == null ? 0M : item.Price);
                        lst.Add(pitem);

                    });
                    result.Data = JsonConvert.SerializeObject(lst.Select(m => new { SysNo = m.Key, Price = m.Value }).ToList());
                }
                result.Status = true;
                result.StatusCode = 1;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取业务员最低限价" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取产品价格
        /// </summary>
        /// <param name="productSysnos">产品id字符串  逗号分隔 </param>
        /// <returns>
        /// [
        ///     {SysNo:111,PriceList:[{Name:'初级',Price:xxx},{Name:'中级',Price:xxx},{Name:'高级',Price:xxx},{Name:'最低价',Price:xxx}]},
        ///     {SysNo:112,PriceList:[{Name:'初级',Price:xxx},{Name:'中级',Price:xxx},{Name:'高级',Price:xxx},{Name:'最低价',Price:xxx}]},
        /// ]
        /// </returns>
        ///  <remarks>2014-09-16 余勇 创建</remarks>
        public Result<String> SelectProductPrice(string productSysnos)
        {
            var result = new Result<String> { StatusCode = -1, Status = false };
            try
            {
                var lst = new List<KeyValuePair<int, IList<CBPdPrice>>>();
                if (string.IsNullOrWhiteSpace(productSysnos))
                {
                    result.Message = "产品编号不能为空";
                    return result;
                }
                var productids = productSysnos.Split(',').Select(m => int.Parse(m)).ToList();
                if (productids != null)
                {
                    productids.ForEach(p =>
                    {
                        var items = PdPriceBo.Instance.GetProductLevelPrice(p);
                        var itemlimt = PdPriceBo.Instance.GetProductPrice(p, new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.业务员销售限价 }).FirstOrDefault();//业务员限价
                        items.Add(
                           new CBPdPrice()
                            {
                                Price = itemlimt == null ? 0 : itemlimt.Price,
                                PriceName = "最低价"
                            }
                        );
                        var pitem = new KeyValuePair<int, IList<CBPdPrice>>(p, items);
                        lst.Add(pitem);                     
                    });
              

                    result.Data = JsonConvert.SerializeObject(lst.Select(m => new { SysNo = m.Key, PriceList = m.Value.Select(i => new { Name = i.PriceName, Price = i.Price }) }).ToList());
                    result.Status = true;
                    result.StatusCode = 1;
                }
               
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取产品价格" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 通过客户系统编号获取借货单商品列表
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        public Result<IList<CBPdProductJson>> GetProductLendItmeList(int customerSysNo)
        {
            var result = new Result<IList<CBPdProductJson>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var list = BLL.Warehouse.WhWarehouseBo.Instance.GetProductLendItmeList(
                            Authorization.Data.SysNo,
                            customerSysNo,
                            WarehouseStatus.借货单状态.已出库,
                            ProductStatus.产品价格来源.会员等级价);
                    if (list != null && list.Any())
                    {
                        foreach (var item in list)
                        {
                            item.ImageUrl = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image120,
                                                                                        item.ProductSysNo);
                            item.ProductName = PdProductBo.Instance.GetProductEasName(item.ProductSysNo);
                        }
                    }
                    result.Data = list;
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "通过配送员系统编号获取借货单商品列表" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 返回所有的商品分类
        /// </summary>
        /// <returns>所有的商品分类</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        public Result<IList<AppPdCategory>> GetProductCategoryList()
        {
            var result = new Result<IList<AppPdCategory>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var list = CacheManager.Get(CacheKeys.Items.ProductCategory,
                                                                   () => PdCategoryBo.Instance.GetAllCategory());
                    var data = (list.Where(x => x.ParentSysNo == 0).Select(x =>
                                                                           new AppPdCategory
                                                                               {
                                                                                   SysNo = x.SysNo,
                                                                                   ParentSysNo = x.ParentSysNo,
                                                                                   CategoryName = x.CategoryName,
                                                                                   ItemList =
                                                                                       PdCategoryChild(x.SysNo, list)
                                                                               })).ToList();

                    result.Data = data;
                    result.StatusCode = 0;
                    result.Status = true;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "返回所有的商品分类" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 迭代商品分类生成节点树
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="list">所有商品分类</param>
        /// <returns>带节点分类树</returns>
        /// <remarks>2013-07-31 周唐炬 创建</remarks>
        private List<AppPdCategory> PdCategoryChild(int sysNo, IList<PdCategory> list)
        {
            return (list.Where(x => x.ParentSysNo == sysNo).Select(x => new AppPdCategory()
                {
                    SysNo = x.SysNo,
                    ParentSysNo = x.ParentSysNo,
                    CategoryName = x.CategoryName,
                    ItemList = PdCategoryChild(x.SysNo, list)
                })).ToList();
        }

        /// <summary>
        /// 获取所有的分类下商品列表
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>所有的商品分类</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        public Result<Pager<AppProduct>> GetProductList(int? categorySysNo, int? customerSysNo, string keyword,
                                                        int currentPageIndex, int pageSize)
        {
            var result = new Result<Pager<AppProduct>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var level = CustomerLevel.初级; //会员默认等级
                    if (customerSysNo.HasValue)
                    {
                        var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo.Value);
                        if (null != customer)
                        {
                            level = customer.LevelSysNo; //加入会员等级
                        }
                    }

                    //总页数
                    //int reCount = 0, pageIndex = currentPageIndex, pageCount = pageSize;
                    //var plist = ProductIndexBo.Instance.Search(keyword,
                    //    categorySysNo,
                    //    null,
                    //    pageSize,
                    //    ref pageIndex,
                    //    ref pageCount,
                    //    ref reCount,
                    //    sort: CommonEnum.LuceneProductSortType.销量.GetHashCode(),
                    //    isDescending: true,//true 为降序 false为升序
                    //    priceSourceSysNo: level, showNotFrontEndOrder: true);    //余勇添加搜索前台不能下单的商品 2014-05-27
                    var category = categorySysNo.HasValue ? categorySysNo.Value : 0;
                    var pager = PdProductBo.Instance.GetAppProductListAndPartPrice(category, level, keyword,
                        currentPageIndex, pageSize);
                    var plist = pager.Rows;
                    if (plist != null && plist.Any())
                    {
                        plist.ToList().ForEach(x =>
                            {
                                x.Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, x.SysNo);
                                x.CanFrontEndOrder = (int)ProductStatus.商品是否前台下单.是; // x.CanFrontEndOrder 因app客户端无法发布，暂时把CanFrontEndOrder设为1
                            });
                        result.Data = pager;
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = "未找到相关商品!";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取所有的分类下商品列表" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>返回商品详情</returns>
        ///  <remarks>2013-07-30 周唐炬 创建</remarks>
        public Result<AppProduct> GetProductDetails(int productSysNo, int customerSysNo)
        {
            var result = new Result<AppProduct> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var data = PdProductBo.Instance.GetProduct(productSysNo);
                    if (null != data)
                    {
                        var model = new AppProduct
                            {
                                SysNo = data.SysNo,
                                //ProductName = data.ProductName,
                                ProductName = data.EasName,
                                ProductDesc = data.ProductDesc,
                                ProductCommentScore = Convert.ToDouble(data.ProductCommentScore.Value),
                                CommentTimesCount = data.CommentTimesCount.Value,
                                CanFrontEndOrder = (int)ProductStatus.商品是否前台下单.是   // data.CanFrontEndOrder  因app客户端无法发布，暂时把CanFrontEndOrder设为1
                            };
                        //var list = BLL.Product.PdProductImageBo.Instance.GetProductImg(productSysNo);
                        if (data.PdProductImage.Value != null)
                        {
                            model.ImgList = new List<ProductImage>();
                            foreach (var item in data.PdProductImage.Value)
                            {
                                //格式化图片路径，并取200大小
                                var imageUrl = ProductImageBo.Instance.GetProductImagePath(item.ImageUrl, ProductThumbnailType.Image200);
                                model.ImgList.Add(new ProductImage
                                    {
                                        SysNo = item.Status,
                                        ProductSysNo = item.ProductSysNo,
                                        ImageUrl = imageUrl,
                                        Status = item.Status,
                                        DisplayOrder = item.DisplayOrder
                                    });
                            }
                        }
                        //商品属性分组数据
                        var attributeGroupList = PdProductBo.Instance.GetProductAttributeByProductSysNo(productSysNo);
                        if (null != attributeGroupList)
                        {
                            model.AttributeGroupList = new List<AppProductAttributeGroup>();
                            foreach (var item in attributeGroupList)
                            {
                                List<AppProductAttribute> attributeList = null;
                                if (null != item.ProductAtttributeList)
                                {
                                    attributeList = item.ProductAtttributeList.Select(x =>
                                                                                      new AppProductAttribute
                                                                                          {
                                                                                              SysNo = x.SysNo,
                                                                                              AttributeName =
                                                                                                  x.AttributeName,
                                                                                              AttributeText =
                                                                                                  x.AttributeText,
                                                                                              AttributeImage =
                                                                                                  x.AttributeImage,
                                                                                              AttributeType =
                                                                                                  x.AttributeType
                                                                                          }).ToList();
                                }
                                model.AttributeGroupList.Add(new AppProductAttributeGroup
                                    {
                                        GroupSysNo = item.AttributeGroupSysNo,
                                        GroupName = item.AttributeGroupName,
                                        AttributeList = attributeList
                                    });
                            }
                        }
                        if (data.PdPrice.Value != null)
                        {
                            var price = data.PdPrice.Value.SingleOrDefault(x => x.PriceSource == 0);
                            if (price != null)
                            {
                                //商品基础价格
                                model.Price = price.Price;
                            }
                            var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
                            var level = 1; //会员默认等级
                            if (null != customer)
                            {
                                level = customer.LevelSysNo; //加入会员等级
                            }
                            //会员等级价
                            var levelPrice =
                                data.PdPrice.Value.SingleOrDefault(x => x.PriceSource == 10 && x.SourceSysNo == level);
                            model.LevelPrice = levelPrice != null ? levelPrice.Price : model.Price;
                        }

                        result.Data = model;
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = string.Format("未找到编号为{0}的商品!", productSysNo);
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取商品详情" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 购物车 (沈强)

        /// <summary>
        /// 添加一个或多个商品至购物车
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="products">商品系统编号集合</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>当前添加商品的购物车信息对象(包括促销信息)</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        /// <remarks>2013-09-16 沈强 修改</remarks>
        public Result<LogisShoppingCart> AddProductsToShopCart(int? customerSysNo, IList<int> products,
                                                               CustomerStatus.购物车商品来源 shopCartSource, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                var tmp = customerSysNo ?? 0;
                foreach (int product in products)
                {
                    CrShoppingCartBo.Instance.Add(tmp, product, 1, shopCartSource);
                }

                if (isReturn)
                {
                    var data = GetShoppingCart(tmp); //余勇 修改 CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, tmp,false,false);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "添加一个或多个商品至购物车" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> ConvertGroupToShopCartItems(int customerSysNo, int groupSysNo, int quantity,
                                                                     int promotionSysNo,
                                                                     CustomerStatus.购物车商品来源 shopCartSource,
                                                                     bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.Add(customerSysNo, groupSysNo, quantity,
                                              promotionSysNo,
                                              shopCartSource);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改 CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "将组促销转换为购物车明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> AddGiftToShopCart(int customerSysNo, int productSysNo, int promotionSysNo,
                                                           CustomerStatus.购物车商品来源 source, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.AddGift(customerSysNo, productSysNo, promotionSysNo,
                                                  source);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo); 
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "添加促销赠品至购物车" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> RemoveShopCartItems(int customerSysNo, int[] sysNo, bool isReturn)
        {

            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.Remove(customerSysNo, sysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> RemoveShopCartGroup(int customerSysNo, string groupCode, string promotionSysNo,
                                                             bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.Remove(customerSysNo, groupCode, promotionSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo); 
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车组商品" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo,
                                                    bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.RemoveGift(customerSysNo, productSysNo, promotionSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除促销赠品" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-09-24 沈强 创建</remarks>
        public Result RemoveAll(int customerSysNo)
        {
            var result = new Result { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.RemoveAll(customerSysNo);
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车所有明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-24 沈强 创建</remarks>
        public Result<LogisShoppingCart> RemoveCheckedItem(int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.RemoveCheckedItem(customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo); 
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车选中的明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> UpdateItemsQuantity(int customerSysNo, int[] sysNo, int quantity, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.UpdateQuantity(customerSysNo, sysNo, quantity);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车明细商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> UpdateGroupQuantity(int customerSysNo, string groupCode, string promotionSysNo,
                                                             int quantity, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.UpdateQuantity(customerSysNo, groupCode, promotionSysNo, quantity);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> CheckedAll(int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.CheckedAll(customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> UncheckedAll(int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.UncheckedAll(customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> CheckedItem(int customerSysNo, int[] itemSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.CheckedItem(customerSysNo, itemSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "选择购物车明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> UncheckedItem(int customerSysNo, int[] itemSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.UncheckedItem(customerSysNo, itemSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "取消选择购物车明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> CheckedGroupItem(int customerSysNo, string groupCode, string promotionSysNo,
                                                          bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartBo.Instance.CheckedItem(customerSysNo, groupCode, promotionSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "选择购物车组明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        public Result<LogisShoppingCart> UncheckedGroupItem(int customerSysNo, string groupCode, string promotionSysNo,
                                                            bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.UncheckedItem(customerSysNo, groupCode, promotionSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(customerSysNo); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "取消选择购物车组明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取当前用户所有的购物车信息
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        /// <remarks>2013-09-16 沈强 修改</remarks>
        public Result<LogisShoppingCart> GetShopCart(int customerSysNo, bool isChecked)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                var data = GetShoppingCart(customerSysNo, isChecked); //余勇 修改  CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo, isChecked);

                result.Data = ConvertShopCart(data);
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取当前用户所有的购物车信息" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-16 沈强 修改</remarks>
        public Result<LogisShoppingCart> GetShoppingCart(int customerSysNo, int[] sysNo, int? areaSysNo,
                                                         int? deliveryTypeSysNo, string promotionCode, string couponCode,
                                                         bool isChecked)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                var data = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo, sysNo, areaSysNo,
                                                                     deliveryTypeSysNo, promotionCode, couponCode,
                                                                     isChecked, false);                                 //余勇 修改 isFrontProduct为false
                result.Data = ConvertShopCart(data);
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取购物车对象" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取当前购物车有效可使用的优惠券//--------------------------------------------------------------
        /// </summary>
        /// <param name="customerSysNo">当前登录用户系统编号</param>
        /// <returns>返回有效可使用的优惠券集合</returns>
        /// <remarks>2013-09-28 沈强 修改</remarks>
        /// <remarks>2013-12-30 吴文强 修改 添加优惠券使用平台类型</remarks>
        public Result<List<AppSpCoupon>> GetCurrentCartValidCoupons(int customerSysNo)
        {
            var result = new Result<List<AppSpCoupon>> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            var data = GetShoppingCart(customerSysNo); //余勇 修改 CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
            return GetAppCurrentCartValidCoupons(data, customerSysNo, "获取当前购物车有效可使用的优惠券");
        }

        /// <summary>
        /// 获取当前购物车有效可使用的优惠券
        /// </summary>
        /// <param name="shoppingCart">当前购物车</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="message">需要记录的消息</param>
        /// <returns>可使用的优惠券</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        private Result<List<AppSpCoupon>> GetAppCurrentCartValidCoupons(CrShoppingCart shoppingCart, int customerSysNo, string message)
        {
            var result = new Result<List<AppSpCoupon>> { StatusCode = -1 };
            try
            {
                //2013-12-30 吴文强 修改 添加优惠券使用平台类型
                var currentCartValidCoupons = SpCouponBo.Instance.GetCurrentCartValidCoupons(customerSysNo, shoppingCart, new[] { PromotionStatus.促销使用平台.物流App });
                if (currentCartValidCoupons != null)
                {
                    var data = currentCartValidCoupons.Select(coupon => new AppSpCoupon
                    {
                        AuditDate = DateTime.Now,
                        AuditorSysNo = coupon.AuditorSysNo,
                        CouponAmount = coupon.CouponAmount,
                        CouponCode = coupon.CouponCode,
                        CreatedBy = coupon.CreatedBy,
                        CreatedDate = coupon.CreatedDate,
                        CustomerSysNo = coupon.CustomerSysNo,
                        Description = coupon.Description,
                        EndTime = coupon.EndTime,
                        LastUpdateBy = coupon.LastUpdateBy,
                        LastUpdateDate = coupon.LastUpdateDate,
                        PromotionSysNo = coupon.PromotionSysNo,
                        RequirementAmount = coupon.RequirementAmount,
                        SourceDescription = coupon.SourceDescription,
                        StartTime = coupon.StartTime,
                        Status = coupon.Status,
                        SysNo = coupon.SysNo,
                        Type = coupon.Type,
                        UseQuantity = coupon.UseQuantity,
                        UsedQuantity = coupon.UsedQuantity
                    }).ToList();
                    result.Data = data;
                    result.StatusCode = 0;
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, message + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 转换购物车实体
        /// </summary>
        /// <param name="crShoppingCart">购物车对象</param>
        /// <returns>客户端购物车对象</returns>
        /// <remarks>2013-09-28 沈强 创建</remarks>
        private LogisShoppingCart ConvertShopCart(CrShoppingCart crShoppingCart)
        {
            var logisShoppingCart = new LogisShoppingCart
                {
                    CouponAmount = crShoppingCart.CouponAmount,
                    CouponCode = crShoppingCart.CouponCode,
                    FreightAmount = crShoppingCart.FreightAmount,
                    FreightDiscountAmount = crShoppingCart.FreightDiscountAmount,
                    GroupPromotions = crShoppingCart.GroupPromotions,
                    ProductAmount = crShoppingCart.ProductAmount,
                    ProductDiscountAmount = crShoppingCart.ProductDiscountAmount,
                    SettlementAmount = crShoppingCart.SettlementAmount,
                    SettlementDiscountAmount = crShoppingCart.SettlementDiscountAmount,
                    ShoppingCartGroups = crShoppingCart.ShoppingCartGroups
                };

            if (logisShoppingCart.ShoppingCartGroups != null)
            {
                foreach (var shopGroup in logisShoppingCart.ShoppingCartGroups)
                {
                    if (shopGroup.ShoppingCartItems != null)
                    {
                        foreach (var item in shopGroup.ShoppingCartItems)
                        {
                            item.ProductName = PdProductBo.Instance.GetProductEasName(item.ProductSysNo);
                        }
                    }
                    if (shopGroup.GroupPromotions == null) continue;
                    foreach (var item in shopGroup.GroupPromotions)
                    {
                        if (item.GiftProducts != null)
                        {
                            foreach (var gift in item.GiftProducts)
                            {
                                gift.ProductName = PdProductBo.Instance.GetProductEasName(gift.ProductSysNo);
                            }
                        }
                        if (item.UsedGiftProducts == null) continue;
                        foreach (var used in item.UsedGiftProducts)
                        {
                            used.ProductName = PdProductBo.Instance.GetProductEasName(used.ProductSysNo);
                        }
                    }
                }
            }
            if (logisShoppingCart.GroupPromotions != null)
            {
                foreach (var item in logisShoppingCart.GroupPromotions)
                {
                    if (item.GiftProducts != null)
                    {
                        foreach (var gift in item.GiftProducts)
                        {
                            gift.ProductName = PdProductBo.Instance.GetProductEasName(gift.ProductSysNo);
                        }
                    }
                    if (item.UsedGiftProducts == null) continue;
                    foreach (var used in item.UsedGiftProducts)
                    {
                        used.ProductName = PdProductBo.Instance.GetProductEasName(used.ProductSysNo);
                    }
                }
            }


            return logisShoppingCart;
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <returns>返回支付类型集合</returns>
        /// <remarks>
        /// 2013-7-12 杨浩 创建
        /// 2013-9-29 沈强 修改
        /// </remarks>
        private IList<Model.B2CApp.PaymentType> GetPaymentType(int areaSysNo)
        {
            var item = new Model.B2CApp.PaymentType
                {
                    SysNo = PaymentType.支付宝,
                    Type = PaymentType.支付宝,
                    PaymentName = "支付宝"
                };

            var data = new List<Model.B2CApp.PaymentType> { item };

            var itemo = new Model.B2CApp.PaymentType();
            if (SoOrderBo.Instance.IsInDeliveryScope(areaSysNo))
            {
                itemo.SysNo = PaymentType.现金;
                itemo.Type = PaymentType.现金;
                itemo.PaymentName = "货到付款";
                data.Add(itemo);
            }

            return data;
        }

        /// <summary>
        /// App提交订单//-----------------------------------------------------------
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <param name="order">App订单信息</param>
        /// <returns>返回订单</returns>
        /// <remarks>2013-09-29 沈强 修改</remarks>
        public Result<AppSoOrder> CreatedOrder(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order)
        {
            var result = new Result<AppSoOrder> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CrShoppingCartToCacheBo.Instance.RemoveAll(cacheKey, 0);
            }
            var shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, order.CustomerSysNo, null, receiveAddress.AreaSysNo,
                                                                 order.DeliveryTypeSysNo, null, order.CouponCode,
                                                                 true, false);               // 余勇 修改isFrontProduct为false 
            return CreateAppOrder(shoppingCart, receiveAddress, order, "App提交订单",false);
        }

        /// <summary>
        /// App提交订单cache
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <param name="order">App订单信息</param>
        /// <returns>返回订单</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        public Result<AppSoOrder> CreateOrderCache(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order)
        {
            var result = new Result<AppSoOrder> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            var shoppingCart = CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, 0, null, receiveAddress.AreaSysNo,
                                                                 order.DeliveryTypeSysNo, null, order.CouponCode,
                                                                 true, false);               //余勇 修改isFrontProducty为false
            return CreateAppOrder(shoppingCart, receiveAddress, order, "App提交订单cache");
        }

        /// <summary>
        /// App提交订单
        /// </summary>
        /// <param name="shoppingCart">当前购物车</param>
        /// <param name="receiveAddress">收货地址</param>
        /// <param name="order">App订单信息</param>
        /// <param name="message">日志信息</param>
        /// <param name="isDelivery">是否立刻配送</param>
        /// <returns>返回订单</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        /// <remarks>2014-09-29 朱成果 修改</remarks>
        private Result<AppSoOrder> CreateAppOrder(CrShoppingCart shoppingCart, AppSoReceiveAddress receiveAddress, AppShopCartOrder order, string message, bool isDelivery=false)
        {
            var result = new Result<AppSoOrder> { StatusCode = -1 };

            if (isDelivery)//直接送货,必须先收钱
            {
                var paytype = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetPaymentTypeFromMemory(order.PayTypeSysNo);
                if (paytype == null)
                {
                    result.Status = false;
                    result.Message = "付款方式不存在";
                    return result;
                }
                if (paytype.PaymentType == (int)BasicStatus.支付方式类型.到付)//此情况不允许下到付订单
                {
                    result.Status = false;
                    result.Message = "此流程必须先收款。";
                    return result;
                }
            }
            //得到当前用户
            var user = Authorization.Data;
            var soReceive = ConveryReceiveAddress(receiveAddress);

            #region 配送前是否联系

            var beforeDelivery = OrderStatus.配送前是否联系.否;

            if (order.ContactBeforeDelivery == 1)
            {
                beforeDelivery = OrderStatus.配送前是否联系.是;
            }

            #endregion

            #region 构建发票对象

            FnInvoice invoice = null;
            if (order.Invoice != null)
            {
                invoice = new FnInvoice
                {
                    InvoiceTypeSysNo = order.Invoice.InvoiceTypeSysNo,
                    InvoiceTitle = order.Invoice.InvoiceTitle,
                    InvoiceRemarks = order.Invoice.InvoiceRemarks,
                    Status = FinanceStatus.发票状态.待开票.GetHashCode(),
                    CreatedBy = Authorization.Data.SysNo,
                    LastUpdateBy = Authorization.Data.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                };
            }

            #endregion

            try
            {
                SoOrder newOrder;
                var appSpCoupons = new List<AppSpCoupon>();
                using (var tran = new TransactionScope())
                {
                    int warehouseSysNo = BLL.Warehouse.WhWarehouseBo.Instance.GetDeliveryUserWarehouseSysNo(user.SysNo);//业务员配送仓库
                    int? defaultwarehouse = null;
                    if (isDelivery)//直接送货
                    {
                        defaultwarehouse = warehouseSysNo;//默认仓库为业务员所在仓库
                        order.DeliveryTypeSysNo = DeliveryType.普通百城当日达;
                    }
                    newOrder = new SoOrder();
                    //newOrder = SoOrderBo.Instance.CreateOrder(user.SysNo, order.CustomerSysNo, soReceive,
                    //                                              defaultwarehouse, order.DeliveryTypeSysNo,
                    //                                              order.PayTypeSysNo, shoppingCart, 0, invoice,
                    //                                              OrderStatus.销售单来源.业务员下单, user.SysNo,
                    //                                              OrderStatus.销售方式.普通订单, null,
                    //                                              OrderStatus.销售单对用户隐藏.否, order.CustomerMessage,
                    //                                              order.InternalRemarks,
                    //                                              order.DeliveryRemarks, order.DeliveryTime,
                    //                                              beforeDelivery,
                    //                                              string.Empty,0);
                    #region 获取优惠卷信息

                    if (!string.IsNullOrWhiteSpace(order.CouponCode))
                    {
                        var coupon = DataAccess.Promotion.ISpCouponDao.Instance.GetCoupon(order.CouponCode);

                        if (coupon != null)
                        {
                            var appSp = new AppSpCoupon
                            {
                                AuditDate = DateTime.Now,
                                AuditorSysNo = coupon.AuditorSysNo,
                                CouponAmount = coupon.CouponAmount,
                                CouponCode = coupon.CouponCode,
                                CreatedBy = coupon.CreatedBy,
                                CreatedDate = coupon.CreatedDate,
                                CustomerSysNo = coupon.CustomerSysNo,
                                Description = coupon.Description,
                                EndTime = coupon.EndTime,
                                LastUpdateBy = coupon.LastUpdateBy,
                                LastUpdateDate = coupon.LastUpdateDate,
                                PromotionSysNo = coupon.PromotionSysNo,
                                RequirementAmount = coupon.RequirementAmount,
                                SourceDescription = coupon.SourceDescription,
                                StartTime = coupon.StartTime,
                                Status = coupon.Status,
                                SysNo = coupon.SysNo,
                                Type = coupon.Type,
                                UseQuantity = coupon.UseQuantity,
                                UsedQuantity = coupon.UsedQuantity
                            };
                            appSpCoupons.Add(appSp);
                        }
                    }
                    #endregion

                    #region 现场下单现金/刷卡预付,订单支付状态为已支付,并且自动创建收款单明细

                    if (order.PayTypeSysNo == PaymentType.现金预付 || order.PayTypeSysNo == PaymentType.刷卡预付)
                    {
                        //修改订单收款状态为已支付
                        //SoOrderBo.Instance.UpdatePayStatus(newOrder.SysNo, OrderStatus.销售单支付状态.已支付);

                        //创建收款单明细
                       // var receipt = FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(newOrder.SysNo);
                        var receipt = FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(0);
                        var receiptVoucherItem = new FnReceiptVoucherItem
                        {
                            Amount = 0,//newOrder.CashPay,
                            CreatedBy = user.SysNo,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = user.SysNo,
                            LastUpdateDate = DateTime.Now,
                            CreditCardNumber = "",
                            PaymentTypeSysNo = order.PayTypeSysNo,
                            ReceiptVoucherSysNo = receipt.SysNo,
                            Status = 1,
                            TransactionSysNo = newOrder.TransactionSysNo,
                            VoucherNo = ""
                        };
                        //获取支付方式
                        var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(order.PayTypeSysNo);

                        var creditCardNum = order.CreditCardNumber;
                        if (payType.RequiredCardNumber == 1)
                        {
                            if (string.IsNullOrWhiteSpace(creditCardNum))
                            {
                                throw new Exception(payType.PaymentName + "需要输入卡号");
                            }
                            receiptVoucherItem.CreditCardNumber = creditCardNum;
                        }
                        receiptVoucherItem.VoucherNo = order.VoucherNo;

                        if (order.PayTypeSysNo == PaymentType.现金预付)
                        {
                           
                            var easinfo = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetFnReceiptTitleAssociation(warehouseSysNo, payType.SysNo).OrderByDescending(m => m.IsDefault).FirstOrDefault();//eas信息
                            receiptVoucherItem.EasReceiptCode = easinfo == null ? string.Empty : easinfo.EasReceiptCode;//eas收款科目
                            receiptVoucherItem.ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库;//收款方来源
                            receiptVoucherItem.ReceivablesSideSysNo = warehouseSysNo;
                            FnReceiptVoucherBo.Instance.AddReceiptVoucherItem(receiptVoucherItem);
                            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(newOrder.SysNo, user);//收现金自动确认收款单
                            TwoSaleBo.Instance.InsertTwoSaleCashHistory(
                                      new Rp_业务员二次销售()
                                      {
                                          CreateDate = newOrder.CreateDate,
                                          DeliveryUserSysNo = user.SysNo,
                                          DeliveryUserName = user.UserName,
                                          StockSysNo = warehouseSysNo,
                                          StockName = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(warehouseSysNo),
                                          OrderSysNo = newOrder.SysNo,
                                          OrderAmount = newOrder.CashPay
                                      }
                            );
                        }
                        else
                        {
                            FnReceiptVoucherBo.Instance.AddReceiptVoucherItem(receiptVoucherItem);
                        }
                    }
                    //同步支付时间的到订单主表
                    ISoOrderDao.Instance.UpdateOrderPayDteById(newOrder.SysNo);
                    #endregion

                    if (isDelivery)//直接送货
                    {
                        AppOrder entity = new AppOrder();
                        entity.SoOrder = newOrder;
                        entity.Products = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(newOrder.SysNo);
                        entity.SoReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(newOrder.ReceiveAddressSysNo);
                        entity.Invoice = Hyt.BLL.Order.SoOrderBo.Instance.GetFnInvoice(newOrder.InvoiceSysNo);
                        Hyt.BLL.Order.TwoSaleBo.Instance.DeliveryTwoSaleSoOrder(entity, user);
                    }

                    result.StatusCode = 0;
                    result.Status = true;
                    tran.Complete();
                }
                if(result.Status)
                {
                    var app = CreateAppSoOrder(order, newOrder);
                    app.CouponAmount = shoppingCart.CouponAmount;
                    app.AppSpCoupons = appSpCoupons;
                    result.Data = app;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, message + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 创建App端接收对象
        /// </summary>
        /// <param name="order">App订单信息</param>
        /// <param name="newOrder">已创建的订单</param>
        /// <returns>返回订单</returns>
        /// <remarks>2013-10-22 沈强 创建</remarks>
        /// <remarks>2014-05-14 余勇 获取发票信息改为调用业务层方法</remarks>
        private AppSoOrder CreateAppSoOrder(AppShopCartOrder order, SoOrder newOrder)
        {
            //获取支付方式
            var bsPaymentType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(newOrder.PayTypeSysNo);

            var app = new AppSoOrder
                {
                    AuditorDate =
                        (newOrder.AuditorDate == DateTime.MinValue ? DateTime.Now : newOrder.AuditorDate),
                    AuditorSysNo = newOrder.AuditorSysNo,
                    CancelDate = (newOrder.CancelDate == DateTime.MinValue ? DateTime.Now : newOrder.CancelDate),
                    CancelUserSysNo = newOrder.CancelUserSysNo,
                    CancelUserType = newOrder.CancelUserType,
                    CashPay = newOrder.CashPay,
                    CoinPay = newOrder.CoinPay,
                    ContactBeforeDelivery = newOrder.ContactBeforeDelivery,
                    CouponAmount = newOrder.CouponAmount,
                    CreateDate = (newOrder.CreateDate == DateTime.MinValue ? DateTime.Now : newOrder.CreateDate),
                    CustomerMessage = newOrder.CustomerMessage,
                    CustomerSysNo = newOrder.CustomerSysNo,
                    DefaultWarehouseSysNo = newOrder.DefaultWarehouseSysNo,
                    DeliveryRemarks = newOrder.DeliveryRemarks,
                    DeliveryTime = newOrder.DeliveryTime,
                    DeliveryTypeSysNo = newOrder.DeliveryTypeSysNo,
                    FreightAmount = newOrder.FreightAmount,
                    FreightChangeAmount = newOrder.FreightChangeAmount,
                    FreightDiscountAmount = newOrder.FreightDiscountAmount,
                    InternalRemarks = newOrder.InternalRemarks,
                    InvoiceSysNo = newOrder.InvoiceSysNo,
                    IsHiddenToCustomer = newOrder.IsHiddenToCustomer,
                    LastUpdateBy = newOrder.LastUpdateBy,
                    LastUpdateDate =
                        (newOrder.LastUpdateDate == DateTime.MinValue ? DateTime.Now : newOrder.LastUpdateDate),
                    OnlineStatus = newOrder.OnlineStatus,
                    OrderAmount = newOrder.OrderAmount,
                    OrderCreatorSysNo = newOrder.OrderCreatorSysNo,
                    OrderDiscountAmount = newOrder.OrderDiscountAmount,
                    OrderSource = newOrder.OrderSource,
                    OrderSourceSysNo = newOrder.OrderSourceSysNo,
                    PayStatus = newOrder.PayStatus,
                    PayTypeSysNo = newOrder.PayTypeSysNo,
                    ProductAmount = newOrder.ProductAmount,
                    ProductChangeAmount = newOrder.ProductChangeAmount,
                    ProductDiscountAmount = newOrder.ProductDiscountAmount,
                    ReceiveAddressSysNo = newOrder.ReceiveAddressSysNo,
                    Remarks = newOrder.Remarks,
                    SalesSysNo = newOrder.SalesSysNo,
                    SalesType = newOrder.SalesType,
                    Stamp = (newOrder.Stamp == DateTime.MinValue ? DateTime.Now : newOrder.Stamp),
                    Status = newOrder.Status,
                    SysNo = newOrder.SysNo,
                    TransactionSysNo = newOrder.TransactionSysNo,
                    UsedPromotions = newOrder.UsedPromotions,
                    DeliveryTypeName = DeliveryTypeBo.Instance.GetDeliveryTypeName(newOrder.DeliveryTypeSysNo),
                    CreditCardNumber = order.CreditCardNumber,
                    VoucherNo = order.VoucherNo,
                    PaymentTypeName = bsPaymentType.PaymentName,
                    PaymentType = bsPaymentType.PaymentType,
                    RequiredCardNumber = bsPaymentType.RequiredCardNumber
                };

            var appSoOrderItems = new List<AppSoOrderItem>();

            #region 获取订单详情

            //获取订单详情
            newOrder.OrderItemList =
                ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(newOrder.SysNo);

            if (newOrder.OrderItemList != null)
            {
                appSoOrderItems.AddRange(newOrder.OrderItemList.Select(soOrderItem => new AppSoOrderItem
                    {
                        ChangeAmount = soOrderItem.ChangeAmount,
                        DiscountAmount = soOrderItem.DiscountAmount,
                        GroupCode = soOrderItem.GroupCode,
                        GroupName = soOrderItem.GroupName,
                        OrderSysNo = soOrderItem.OrderSysNo,
                        OriginalPrice = soOrderItem.OriginalPrice,
                        ProductName = PdProductBo.Instance.GetProductEasName(soOrderItem.ProductSysNo),
                        ProductSalesType = soOrderItem.ProductSalesType,
                        ProductSalesTypeSysNo = soOrderItem.ProductSalesTypeSysNo,
                        ProductSysNo = soOrderItem.ProductSysNo,
                        Quantity = soOrderItem.Quantity,
                        RealStockOutQuantity = soOrderItem.RealStockOutQuantity,
                        SalesAmount = soOrderItem.SalesAmount,
                        SalesUnitPrice = soOrderItem.SalesUnitPrice,
                        SysNo = soOrderItem.SysNo,
                        TransactionSysNo = soOrderItem.TransactionSysNo,
                        UsedPromotions = soOrderItem.UsedPromotions,
                        ProductImage = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image120, soOrderItem.ProductSysNo)
                    }));
            }

            app.OrderItemList = appSoOrderItems;

            #endregion

            #region 获取订单收货地址

            newOrder.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(newOrder.ReceiveAddressSysNo);
            //ISoReceiveAddressDao.Instance.GetOrderReceiveAddress(newOrder.ReceiveAddressSysNo);

            if (newOrder.ReceiveAddress != null)
            {
                BsArea cityEntity;
                BsArea areaEntity;
                //优化var provinceEntity =DataAccess.BaseInfo.IBsAreaDao.Instance.GetProvinceEntity(newOrder.ReceiveAddress.AreaSysNo, out cityEntity, out areaEntity);
                var provinceEntity = BasicAreaBo.Instance.GetProvinceEntity(newOrder.ReceiveAddress.AreaSysNo, out cityEntity, out areaEntity);
                app.ReceiveAddress = new AppSoReceiveAddress
                    {
                        AreaSysNo = newOrder.ReceiveAddress.AreaSysNo,
                        EmailAddress = newOrder.ReceiveAddress.EmailAddress,
                        FaxNumber = newOrder.ReceiveAddress.FaxNumber,
                        Gender = newOrder.ReceiveAddress.Gender,
                        MobilePhoneNumber = newOrder.ReceiveAddress.MobilePhoneNumber,
                        Name = newOrder.ReceiveAddress.Name,
                        PhoneNumber = newOrder.ReceiveAddress.PhoneNumber,
                        StreetAddress =
                            provinceEntity.AreaName + cityEntity.AreaName + areaEntity.AreaName +
                            newOrder.ReceiveAddress.StreetAddress,
                        SysNo = newOrder.ReceiveAddress.SysNo,
                        ZipCode = newOrder.ReceiveAddress.ZipCode
                    };
            }

            #endregion

            #region 获取订单发票信息
            if (newOrder.InvoiceSysNo > 0)
            {
                newOrder.OrderInvoice = FnInvoiceBo.Instance.GetFnInvoice(newOrder.InvoiceSysNo); //IFnInvoiceDao.Instance.GetFnInvoice(newOrder.InvoiceSysNo);
                if (newOrder.OrderInvoice != null)
                {
                    //获取发票类型名称
                    IList<FnInvoiceType> invoiceType = FnInvoiceTypeBo.Instance.GetAll();
                    FnInvoiceType fnInvoice =
                        invoiceType.SingleOrDefault(i => i.SysNo == newOrder.OrderInvoice.InvoiceTypeSysNo);
                    string invoiceTypeName = string.Empty;
                    if (fnInvoice != null)
                    {
                        invoiceTypeName = fnInvoice.Name;
                    }

                    app.OrderInvoice = new AppFnInvoice
                        {
                            CreatedBy = newOrder.OrderInvoice.CreatedBy,
                            CreatedDate =
                                (newOrder.OrderInvoice.CreatedDate == DateTime.MinValue
                                     ? DateTime.Now
                                     : newOrder.OrderInvoice.CreatedDate),
                            InvoiceAmount = newOrder.OrderInvoice.InvoiceAmount,
                            InvoiceCode = newOrder.OrderInvoice.InvoiceCode,
                            InvoiceNo = newOrder.OrderInvoice.InvoiceNo,
                            InvoiceRemarks = newOrder.OrderInvoice.InvoiceRemarks,
                            InvoiceTitle = newOrder.OrderInvoice.InvoiceTitle,
                            InvoiceTypeSysNo = newOrder.OrderInvoice.InvoiceTypeSysNo,
                            LastUpdateBy = newOrder.OrderInvoice.LastUpdateBy,
                            LastUpdateDate =
                                (newOrder.OrderInvoice.LastUpdateDate == DateTime.MinValue
                                     ? DateTime.Now
                                     : newOrder.OrderInvoice.LastUpdateDate),
                            Status = newOrder.OrderInvoice.Status,
                            SysNo = newOrder.OrderInvoice.SysNo,
                            TransactionSysNo = newOrder.OrderInvoice.TransactionSysNo,
                            InvoiceTypeName = invoiceTypeName
                        };
                }

            }

            #endregion

            #region 获取优惠卷

            var spCoupons = SoOrderBo.Instance.GetCouponByOrderSysNo(newOrder.SysNo);
            if (spCoupons != null)
            {
                var appSpCoupons = spCoupons.Select(spCoupon => new AppSpCoupon
                    {
                        AuditDate = (spCoupon.AuditDate == DateTime.MinValue ? DateTime.Now : spCoupon.AuditDate),
                        AuditorSysNo = spCoupon.AuditorSysNo,
                        CouponAmount = spCoupon.CouponAmount,
                        CouponCode = spCoupon.CouponCode,
                        CreatedBy = spCoupon.CreatedBy,
                        CreatedDate = (spCoupon.CreatedDate == DateTime.MinValue ? DateTime.Now : spCoupon.CreatedDate),
                        CustomerSysNo = spCoupon.CustomerSysNo,
                        Description = spCoupon.Description,
                        EndTime = (spCoupon.EndTime == DateTime.MinValue ? DateTime.Now : spCoupon.EndTime),
                        LastUpdateBy = spCoupon.LastUpdateBy,
                        LastUpdateDate = (spCoupon.LastUpdateDate == DateTime.MinValue ? DateTime.Now : spCoupon.LastUpdateDate),
                        PromotionSysNo = spCoupon.PromotionSysNo,
                        RequirementAmount = spCoupon.RequirementAmount,
                        SourceDescription = spCoupon.SourceDescription,
                        StartTime = (spCoupon.StartTime == DateTime.MinValue ? DateTime.Now : spCoupon.StartTime),
                        Status = spCoupon.Status,
                        SysNo = spCoupon.SysNo,
                        Type = spCoupon.Type,
                        UsedQuantity = spCoupon.UsedQuantity,
                        UseQuantity = spCoupon.UseQuantity
                    }).ToList();
                app.AppSpCoupons = appSpCoupons;
            }

            #endregion

            return app;
        }

        /// <summary>
        /// App订单作废
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>返回操作结果</returns>
        /// <remarks>
        /// 2013-10-11 沈强 创建
        /// 2014-01-03 朱家宏 修改了作废订单方法，增加作废人类型
        /// </remarks>
        public Result CancelSoOrder(int orderSysNo)
        {
            var result = new Result { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            //得到当前用户
            var user = (SyUser)HttpContext.Current.Session["SysUser"];
            try
            {
                var msg = string.Empty;
                bool status = false;
                using (var tran = new TransactionScope())
                {
                    status = SoOrderBo.Instance.CancelSoOrder(orderSysNo, user.SysNo, OrderStatus.销售单作废人类型.后台用户, ref msg);
                    tran.Complete();
                }
                result.StatusCode = 0;
                result.Status = status;
                result.Message = msg;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "App订单作废" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 订单 (周唐炬)

        /// <summary>
        /// 创建APP订单
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        public Result CreateSoOrder(AppOrder order)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    if (null != order)
                    {
                        if (null != order.SoOrder)
                        {
                            order.SoOrder.OrderCreatorSysNo = order.SoOrder.LastUpdateBy = Authorization.Data.SysNo;
                            order.SoOrder.CreateDate = order.SoOrder.LastUpdateDate = DateTime.Now;
                            order.SoOrder.DefaultWarehouseSysNo =
                                BLL.Warehouse.WhWarehouseBo.Instance.GetDeliveryUserWarehouseSysNo(
                                    Authorization.Data.SysNo);
                        }
                        if (null != order.Invoice)
                        {
                            order.Invoice.Status = FinanceStatus.发票状态.待开票.GetHashCode();
                            order.Invoice.CreatedBy = order.Invoice.LastUpdateBy = Authorization.Data.SysNo;
                            order.Invoice.CreatedDate = order.Invoice.LastUpdateDate = DateTime.Now;
                        }
                        if (null != order.Invoice && order.Invoice.InvoiceTypeSysNo == 0)
                        {
                            order.Invoice = null;
                        }
                        using (var tran = new TransactionScope())
                        {
                            result = SoOrderBo.Instance.CreateSoOrder(order.SoOrder, order.SoReceiveAddress,
                                                                      order.Products.ToArray(), order.Invoice,
                                                                      Authorization.Data);
                            tran.Complete();
                        }
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = "订单不能为空!";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取商品详情" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取该区域支持的配送方式 
        /// </summary>
        /// <param name="areaNo">地区系统编号</param>
        /// <param name="address">地区全称</param>
        /// <returns>该区域支持的配送方式 </returns>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        public Result<IList<AppLgDeliveryType>> GetDeliveryTypeList(int areaNo, string address)
        {
            var result = new Result<IList<AppLgDeliveryType>> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                //优化 var area = DataAccess.BaseInfo.IBsAreaDao.Instance.GetArea(areaNo);
                //优化 var city = DataAccess.BaseInfo.IBsAreaDao.Instance.GetArea(area.ParentSysNo);
                var area = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(areaNo);
                var city = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(area.ParentSysNo);

                //获取当前地区是否在白城当日送范围内
                var gercoderResult = BLL.Map.BaiduMapAPI.Instance.Geocoder(city.AreaName + area.AreaName + address);
                var isInMap = LgDeliveryScopeBo.Instance.IsInScope(city.SysNo,
                                                     new Coordinate { X = gercoderResult.Lng, Y = gercoderResult.Lat });

                var list = SoOrderBo.Instance.LoadDeliveryTypeByAreaNo(areaNo, city.SysNo, isInMap)
                                    .Where(x => x.Status == 1)
                                    .Select(x => new AppLgDeliveryType
                                                {
                                                    SysNo = x.SysNo,
                                                    DeliveryTypeName = x.DeliveryTypeName,
                                                    DisplayOrder = x.DisplayOrder,
                                                    OptGroup = (x.IsThirdPartyExpress != 1 && x.ParentSysNo == 0),
                                                    ParentSysNo = x.ParentSysNo
                                                }).ToList();

                result.Data = list;
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取该区域支持的配送方式" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 根据配送方式获取支付方式---------------------------------------------------------------
        /// </summary>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>支付方式</returns>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        public Result<IList<AppBsPaymentType>> GetPayTypeList(int deliverySysNo)
        {
            var result = new Result<IList<AppBsPaymentType>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var list =
                        SoOrderBo.Instance.LoadPayTypeListByDeliverySysNo(deliverySysNo)
                                 .Where(x => x.Status == 1)
                                 .Select(x =>
                                         new AppBsPaymentType
                                             {
                                                 SysNo = x.SysNo,
                                                 PaymentName = x.PaymentName,
                                                 DisplayOrder = x.DisplayOrder,
                                                 PaymentType = x.PaymentType,
                                                 RequiredCardNumber = x.RequiredCardNumber
                                             }).ToList();

                    var config = SyConfigBo.Instance.GetModel("AppPaymentType", SystemStatus.系统配置类型.支付配置);
                    if (config != null && !string.IsNullOrWhiteSpace(config.Value))
                    {
                        var sysnos = config.Value.Split(',');
                        var types = new List<AppBsPaymentType>();
                        foreach (var sysno in sysnos)
                        {
                            types.AddRange(list.Where(l => l.SysNo == int.Parse(sysno)));
                        }

                        result.Data = types.OrderByDescending(t => t.SysNo).ToList();
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "根据配送方式获取支付方式" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取发票类型列表
        /// </summary>
        /// <returns>发票类型列表</returns>
        ///<remarks>2013-08-01 周唐炬 创建</remarks>
        public Result<IList<AppFnInvoiceType>> GetInvoiceTypeList()
        {
            var result = new Result<IList<AppFnInvoiceType>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {

                    var list = CacheManager.Get<IList<AppFnInvoiceType>>(CacheKeys.Items.InvoiceTypeList,
                                                                         () =>
                                                                         FnInvoiceBo.Instance.GetFnInvoiceTypeList()
                                                                                    .Select(x =>
                                                                                            new AppFnInvoiceType
                                                                                                {
                                                                                                    SysNo = x.SysNo,
                                                                                                    Name = x.Name
                                                                                                }).ToList());

                    result.Data = list;
                    result.StatusCode = 0;
                    result.Status = true;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "根据配送方式获取支付方式" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 配送单 黄伟

        /// <summary>
        /// 获取今天指定的配送单列表
        /// </summary>
        /// <param name="sysNos">要排除的配送单id集合</param>
        /// <returns>今天已过滤的配送单列表</returns>
        /// <remarks>2013-06-21 黄伟 创建</remarks>
        public Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNos)
        {
            var result = new Result<List<CBWCFLgDelivery>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    result = LgDeliveryBo.Instance.GetDeliveryList(sysNos, Authorization.Data.SysNo);
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                    result.Message = Authorization.Message;
                }
                //result = LgDeliveryBo.Instance.GetDeliveryList(sysNos,1501);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取今天指定的配送单列表" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获取该配送员所有配送在途的配送单列表
        /// </summary>
        /// <returns>配送单列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        public Result<List<CBWCFLgDelivery>> GetDeliveryListAll()
        {
            var result = new Result<List<CBWCFLgDelivery>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    result = LgDeliveryBo.Instance.GetLgDeliveryListAll(Authorization.Data.SysNo);
                    //result = LgDeliveryBo.Instance.GetLgDeliveryListAll(1501);
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                    result.Message = Authorization.Message;
                }
                //result = LgDeliveryBo.Instance.GetLgDeliveryListAll(1501);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取该配送员所有配送在途的配送单列表" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 根据配送单编号获取配送单明细
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        public Result<List<LogisticsDeliveryItem>> GetItemListByDeliverySysNo(int sysNo)
        {
            var result = new Result<List<LogisticsDeliveryItem>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    result = LgDeliveryBo.Instance.GetItemListByDeliverySysNo(sysNo);
                    if (result != null && result.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            if (item.Items == null) continue;
                            foreach (var data in item.Items)
                            {
                                data.ProductName = PdProductBo.Instance.GetProductEasName(data.ProductSysNo);
                            }
                        }
                    }
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                    result.Message = Authorization.Message;
                }
            }
            catch (Exception ex)
            {
                if (result != null) result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取配送单明细" + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="lstStatus">需要更新的单据编号及状态集合</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-06-24 黄伟 创建</remarks>
        /// <remarks>2014-01-15 沈强 修改</remarks>
        /// <remarks>2014-03-20 周唐炬 修改</remarks>
        public Result Sign(List<CBWCFStatusUpdate> lstStatus)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                result = Authorization.Status ? LgDeliveryBo.Instance.UpdateStatus(lstStatus, Authorization.Data) : Authorization;
                //LgDeliveryBo.Instance.UpdateStatus(lstStatus, Authorization.Data);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新单据状态" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取常用文本
        /// </summary>
        /// <param name="codeType">文本类型</param>
        /// <returns>常用文本</returns>
        /// <remarks>2014-03-20 周唐炬 创建</remarks>
        public Result<List<AppBsCode>> GetSignCode(int codeType)
        {
            var result = new Result<List<AppBsCode>> { StatusCode = -1 };         
            try
            {
                List<AppBsCode> list = null;
                if (codeType == Code.物流App配送拒收)
                {
                    list = BsCodeBo.Instance.GetListByParentSysNo(Code.物流App配送拒收).Select(x => new AppBsCode
                        {
                            SysNo = x.SysNo,
                            ParentSysNo = x.ParentSysNo,
                            CodeName = x.CodeName
                        }).ToList();
                }
                else if (codeType == Code.物流App配送未送达)
                {
                    list = BsCodeBo.Instance.GetListByParentSysNo(Code.物流App配送未送达).Select(x => new AppBsCode
                        {
                            SysNo = x.SysNo,
                            ParentSysNo = x.ParentSysNo,
                            CodeName = x.CodeName
                        }).ToList();
                }

                result.Data = list;
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取常用文本" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 部分签收
        /// </summary>
        /// <param name="cbCreateSettlement">CBCreateSettlement entity</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-07-15 黄伟 创建</remarks>
        public Result PartialSign(CBCreateSettlement cbCreateSettlement)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    result = LgSettlementBo.Instance.CreateSettlement(cbCreateSettlement,
                                                                      HttpContext.Current.Request.ServerVariables[
                                                                          "REMOTE_ADDR"]);
                }
                else
                {
                    result = Authorization;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "部分签收" + ex.Message, ex);
            }
            //return LgSettlementBo.Instance.CreateSettlement(cbCreateSettlement);
            return result;
        }

        /// <summary>
        /// 补单
        /// </summary>
        /// <param name="order">ParaLogisticsControllerAdditionalOrders实体</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-07-19 黄伟 创建</remarks>
        public Result AddOrder(ParaLogisticsControllerAdditionalOrders order)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    //配送人员编号+商品编号 确定仓库编号
                    if (order == null || !order.OrderInformations.Any())
                        return new Result { StatusCode = -1, Status = false, Message = "列表中没有商品!" };
                    var warehouseSysNo = ProductLendBo.Instance.GetWhSysNoByDelUserAndProduct(Authorization.Data.SysNo,
                                                                                              order.OrderInformations
                                                                                                   .First()
                                                                                                   .ProductSysNo);
                    order.WarehouseSysNo = warehouseSysNo;
                    order.DeliverymanSysNo = Authorization.Data.SysNo;
                    //order.PaymentTypeSysNo = (int) Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付;
                    order.PaymentTypeSysNo = 1; //现金
                    result = LogisticsOrderBo.Instance.AddOrders(order, Authorization.Data);
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                    result.Message = Authorization.Message;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取商品详情" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 部分签收修改数量后返回该订单总金额
        /// </summary>
        /// <param name="model">CBWCFParamGetAmount实体</param>
        /// <returns>CBWCFParamGetAmountOrderAmount实体:相关金额</returns>
        /// <remarks>2013-07-19 黄伟 创建</remarks>
        public Result<CBWCFParamGetAmountOrderAmount> GetAmount(CBRMAOrderInfo model)
        {
            var result = new Result<CBWCFParamGetAmountOrderAmount> { StatusCode = -1 };

            try
            {
                if (Authorization.Status)
                {

                    //var signInfos = model.lstRMAOrderItemInfo.Select(p => new CBRMAOrderItemInfo
                    //    {
                    //        OrderItemSysNo = p.OrderItemSysNo,
                    //        
                    //        Qty = SoOrderBo.Instance.GetOrderItem(p.OrderItemSysNo).Quantity-p.Qty
                    //    });
                    //model.lstRMAOrderItemInfo = signInfos.ToList();

                    var returnAmount = LgSettlementBo.Instance.GetProductPrice(model);
                    //return new Result<decimal>{Data=100,Message = "Order amount returned successfully",Status=true,StatusCode = 0};
                    result = new Result<CBWCFParamGetAmountOrderAmount>
                        {
                            Data =
                                new CBWCFParamGetAmountOrderAmount
                                    {
                                        ReturnAmount = returnAmount
                                    },
                            Status = true
                        };
                }
                else
                {
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                    result.Message = Authorization.Message;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "部分签收修改数量后返回该订单总金额" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 手机App购物车补单接口

        /// <summary>
        /// 添加一个或多个商品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="products">商品系统编号集合</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>当前添加商品的购物车信息对象(包括促销信息)</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        public Result<LogisShoppingCart> AddProductsToShopCartCache(string cacheKey,
                                                                    IList<int> products,
                                                                    CustomerStatus.购物车商品来源 shopCartSource, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                foreach (var product in products)
                {
                    CrShoppingCartToCacheBo.Instance.Add(cacheKey, 0, product, 1, shopCartSource);
                }

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey);   //余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, 0);
                    if (data != null)
                    {
                        result.Data = ConvertShopCart(data);
                    }
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "添加一个或多个商品至购物车" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：false）</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2014-05-29 余勇 创建</remarks>
        private CrShoppingCart GetShoppingCart(string cacheKey, int customerSysNo = 0, bool isChecked = false, bool isFrontProduct = false)
        {
            return CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo, isChecked, isFrontProduct);
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：false）</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2014-05-29 余勇 创建</remarks>
        private CrShoppingCart GetShoppingCart(int customerSysNo = 0, bool isChecked = false, bool isFrontProduct = false)
        {
            return CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo, isChecked, isFrontProduct);
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        public Result<LogisShoppingCart> GetShoppingCartCache(string cacheKey)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                var data = GetShoppingCart(cacheKey); //余勇 修改CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey,new[] { PromotionStatus.促销使用平台.物流App }, 0);
                if (data != null)
                {
                    result.Data = ConvertShopCart(data);
                    result.StatusCode = 0;
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取购物车对象" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        public Result<LogisShoppingCart> ConvertShopCartCacheToShopCart(string cacheKey, int customerSysNo, CustomerStatus.购物车商品来源 shopCartSource)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {
                CrShoppingCartBo.Instance.RemoveAll(customerSysNo);
                var shoppingCartCache = GetShoppingCart(cacheKey);  //余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, 0);
                var shoppingCartItems = shoppingCartCache.GetShoppingCartItem();
                if (shoppingCartItems.Any())
                {
                    shoppingCartItems.ForEach(x =>
                    {
                        if (x.ProductSalesType != CustomerStatus.商品销售类型.赠品.GetHashCode())
                        {
                            CrShoppingCartBo.Instance.Add(customerSysNo, x.ProductSysNo, x.Quantity, shopCartSource);
                        }
                    });
                }
                var gifs = shoppingCartCache.Gifts();
                if (gifs.Any())
                {
                    gifs.ForEach(x => CrShoppingCartBo.Instance.AddGift(customerSysNo, x.ProductSysNo, x.PromotionSysNo, shopCartSource));
                }

                //if (shoppingCartCache != null && shoppingCartCache.ShoppingCartGroups.Any())
                //{
                //    shoppingCartCache.ShoppingCartGroups.ForEach(x =>
                //        {
                //            if (x.ShoppingCartItems.Any())
                //            {
                //                x.ShoppingCartItems.ForEach(
                //                    y =>
                //                    CrShoppingCartBo.Instance.Add(customerSysNo, y.ProductSysNo, y.Quantity, shopCartSource));
                //            }
                //        });
                //    //CrShoppingCartToCacheBo.Instance.RemoveAll(cacheKey, 0);
                //}
                var data = GetShoppingCart(customerSysNo, true);  // 余勇 修改 CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo, true);
                result.Data = ConvertShopCart(data);

                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "将组促销转换为购物车明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// cache获取当前购物车有效可使用的优惠券
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>可使用的优惠券</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        public Result<List<AppSpCoupon>> GetCurrentCartValidCouponsCache(string cacheKey, int customerSysNo)
        {
            var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
            return GetAppCurrentCartValidCoupons(data, customerSysNo, "cache获取当前购物车有效可使用的优惠券");
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> AddGiftToShopCartCache(string cacheKey, int customerSysNo, int productSysNo,
                                                                int promotionSysNo,
                                                                CustomerStatus.购物车商品来源 source, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.AddGift(cacheKey, customerSysNo, productSysNo, promotionSysNo,
                                                         source);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo,false,true);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "添加促销赠品至购物车" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> RemoveShopCartItemsCache(string cacheKey, int customerSysNo, int[] sysNo,
                                                                  bool isReturn)
        {

            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.Remove(cacheKey, customerSysNo, sysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> RemoveShopCartGroupCache(string cacheKey, int customerSysNo, string groupCode,
                                                                  string promotionSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.Remove(cacheKey, customerSysNo, groupCode, promotionSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车组商品" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> RemoveGiftCache(string cacheKey, int customerSysNo, int productSysNo,
                                                         int promotionSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.RemoveGift(cacheKey, customerSysNo, productSysNo, promotionSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除促销赠品" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result RemoveAllCache(string cacheKey, int customerSysNo)
        {
            var result = new Result { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.RemoveAll(cacheKey, customerSysNo);
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车所有明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> RemoveCheckedItemCache(string cacheKey, int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.RemoveCheckedItem(cacheKey, customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "删除购物车选中的明细" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> UpdateItemsQuantityCache(string cacheKey, int customerSysNo, int[] sysNo,
                                                                  int quantity, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.UpdateQuantity(cacheKey, customerSysNo, sysNo, quantity);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车明细商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> UpdateGroupQuantityCache(string cacheKey, int customerSysNo, string groupCode,
                                                                  string promotionSysNo, int quantity, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.UpdateQuantity(cacheKey, customerSysNo, groupCode, promotionSysNo,
                                                                quantity);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> CheckedAllCache(string cacheKey, int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.CheckedAll(cacheKey, customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> UncheckedAllCache(string cacheKey, int customerSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.UncheckedAll(cacheKey, customerSysNo);

                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.StatusCode = 0;
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "更新购物车组组商品数量" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> CheckedItemCache(string cacheKey, int customerSysNo, int[] itemSysNo,
                                                          bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.CheckedItem(cacheKey, customerSysNo, itemSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "选择购物车明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> UncheckedItemCache(string cacheKey, int customerSysNo, int[] itemSysNo,
                                                            bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.UncheckedItem(cacheKey, customerSysNo, itemSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "取消选择购物车明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> CheckedGroupItemCache(string cacheKey, int customerSysNo, string groupCode,
                                                               string promotionSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.CheckedItem(cacheKey, customerSysNo, groupCode, promotionSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "选择购物车组明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>返回购物车</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result<LogisShoppingCart> UncheckedGroupItemCache(string cacheKey, int customerSysNo, string groupCode,
                                                                 string promotionSysNo, bool isReturn)
        {
            var result = new Result<LogisShoppingCart>();
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            try
            {

                CrShoppingCartToCacheBo.Instance.UncheckedItem(cacheKey, customerSysNo, groupCode, promotionSysNo);
                if (isReturn)
                {
                    var data = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);
                    result.Data = ConvertShopCart(data);
                }
                result.Status = true;
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "取消选择购物车组明细项目" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 清除购物车缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        public Result ClearCache(string cacheKey)
        {
            CrShoppingCartToCacheBo.Instance.Clear(cacheKey);
            return new Result
                {
                    Status = true
                };
        }

        #region 被否定的方法
        /// <summary>
        /// 配送员确认补单  ---此需求被否定---
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <returns>返回确认状态</returns>
        /// <remarks>2013-09-27 沈强 创建</remarks>
        public Result ConfromAddOrders(string cacheKey, int customerSysNo, AppSoReceiveAddress receiveAddress)
        {
            //AppSoReceiveAddress   SoReceiveAddress
            var result = new Result { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            //得到当前用户
            var user = (SyUser)HttpContext.Current.Session["SysUser"];
            try
            {

                //得到购物车
                CrShoppingCart crShoppingCart = GetShoppingCart(cacheKey, customerSysNo);  // 余勇 修改 CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { PromotionStatus.促销使用平台.物流App }, customerSysNo);

                #region 获取仓库编号

                int warehouseSysNo;

                //filter.DeliveryUserSysNo = user.SysNo;
                var whProductLends = IProductLendDao.Instance.GetWhProductLendList(user.SysNo);
                List<WhProductLend> whProducts = whProductLends.Where(
                    t =>
                    t.Status != (int)WarehouseStatus.借货单状态.作废 &&
                    t.Status != (int)WarehouseStatus.借货单状态.已完成).ToList();

                if (whProducts.Any())
                {
                    warehouseSysNo = whProducts[0].WarehouseSysNo;
                }
                else
                {
                    throw new Exception("编号为 " + user.SysNo + "的配送员，没有任何满足要求的借货单");
                }

                #endregion

                var soReceive = ConveryReceiveAddress(receiveAddress);

                Result remedyOrderResult = LogisticsOrderBo.Instance.RemedyOrder(user.SysNo, customerSysNo, soReceive,
                                                                                 warehouseSysNo,
                                                                                 crShoppingCart, user);
                if (remedyOrderResult.Status)
                {
                    result.StatusCode = 0;
                    result.Status = true;
                }
                else
                {
                    result.Message = "补单失败";
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取购物车对象" + ex.Message, ex);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 转换收货地址对象
        /// </summary>
        /// <param name="receiveAddress">接口参数收货地址对象</param>
        /// <returns>Model地址对象</returns>
        /// <remarks>2013-09-29 沈强 创建</remarks>
        private SoReceiveAddress ConveryReceiveAddress(AppSoReceiveAddress receiveAddress)
        {
            return new SoReceiveAddress
                {

                    MobilePhoneNumber = receiveAddress.MobilePhoneNumber,
                    Name = receiveAddress.Name,
                    PhoneNumber = receiveAddress.PhoneNumber,
                    StreetAddress = receiveAddress.StreetAddress,
                    SysNo = receiveAddress.SysNo,
                    ZipCode = receiveAddress.ZipCode,
                    AreaSysNo = receiveAddress.AreaSysNo,
                    EmailAddress = receiveAddress.EmailAddress,
                    FaxNumber = receiveAddress.FaxNumber,
                    Gender = receiveAddress.Gender
                };
        }

        #endregion

        #region 业务员库存 周唐炬 2014-03-05 创建

        /// <summary>
        /// 获取业务员库存
        /// </summary>
        /// <param></param>
        /// <returns>业务员库存</returns>
        /// <remarks>2014-03-05 周唐炬 创建</remarks>
        public Result<List<AppInventory>> GetInventoryProductList()
        {
            var result = new Result<List<AppInventory>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var list =
                        IProductLendDao.Instance.GetInventoryProductList(new ParaProductLendFilter
                            {
                                DeliveryUserSysNo = Authorization.Data.SysNo,
                                Status = WarehouseStatus.借货单状态.已出库.GetHashCode(),
                                CurrentPage = 1,
                                PageSize = Int32.MaxValue
                            });
                    if (list != null && list.Rows != null && list.Rows.Any())
                    {
                        var data = list.Rows.Select(x => new AppInventory
                            {
                                SysNo = x.SysNo,
                                ProductLendSysNo = x.ProductLendSysNo,
                                ProductSysNo = x.ProductSysNo,
                                ProductName = x.ProductName,
                                LendQuantity = x.LendQuantity,
                                SaleQuantity = x.SaleQuantity,
                                ReturnQuantity = x.ReturnQuantity,
                                ForceCompleteQuantity = x.ForceCompleteQuantity
                            }).ToList();
                        result.Data = data;
                    }
                    result.StatusCode = 0;
                    result.Status = true;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取业务员库存" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 用户优惠券 周唐炬 2014-03-06 创建

        /// <summary>
        /// 获取客户优惠卷
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>客户优惠卷列表</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result<List<AppSpCoupon>> GetUserCoupon(int customerSysNo)
        {
            var result = new Result<List<AppSpCoupon>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
                    if (customer != null && customer.Status != CustomerStatus.会员状态.无效.GetHashCode())
                    {
                        var platformTypes = new[] { PromotionStatus.促销使用平台.物流App };
                        var coupons =
                            SpCouponBo.Instance.GetCustomerCoupons(customerSysNo, 0, platformTypes)
                                      .OrderByDescending(o => o.SysNo);
                        if (coupons.Any())
                        {
                            result.Data = coupons.Select(x => new AppSpCoupon
                            {
                                AuditDate = x.AuditDate > DateTime.MinValue ? x.AuditDate : DateTime.Now,
                                AuditorSysNo = x.AuditorSysNo,
                                CouponAmount = x.CouponAmount,
                                CouponCode = x.CouponCode,
                                CreatedBy = x.CreatedBy,
                                CreatedDate = x.CreatedDate > DateTime.MinValue ? x.CreatedDate : DateTime.Now,
                                CustomerSysNo = x.CustomerSysNo,
                                Description = x.Description,
                                EndTime = x.EndTime,
                                LastUpdateBy = x.LastUpdateBy,
                                LastUpdateDate = x.LastUpdateDate > DateTime.MinValue ? x.LastUpdateDate : DateTime.Now,
                                PromotionSysNo = x.PromotionSysNo,
                                RequirementAmount = x.RequirementAmount,
                                SourceDescription = x.SourceDescription,
                                StartTime = x.StartTime,
                                Status = x.Status,
                                SysNo = x.SysNo,
                                Type = x.Type,
                                UseQuantity = x.UseQuantity,
                                UsedQuantity = x.UsedQuantity
                            }).ToList();
                        }
                        result.Status = true;
                        result.StatusCode = 0;
                    }
                    else
                    {
                        result.Message = "该会员无效，或被禁用！";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取客户优惠卷" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 用户绑定优惠券审核
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result UserCouponAudit(int sysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    Audit(sysNo);
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "用户绑定优惠券审核" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 用户绑定优惠券作废
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result UserCouponCancel(int sysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    result.Status = SpCouponBo.Instance.Cancel(sysNo, Authorization.Data);
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "用户绑定优惠券作废" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 通过优惠卡号获取优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>优惠卡</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result<AppCouponCard> GetAggregatedCouponCard(string couponCardNo)
        {
            var result = new Result<AppCouponCard> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    if (!string.IsNullOrWhiteSpace(couponCardNo))
                    {
                        var card = SpCouponCardBo.Instance.GetAggregatedCouponCard(couponCardNo);
                        if (card != null)
                        {
                            var data = new AppCouponCard();
                            if (card.CouponCard != null)
                            {
                                data.SysNo = card.CouponCard.SysNo;
                                data.ActivationTime = card.CouponCard.ActivationTime > DateTime.MinValue ? card.CouponCard.ActivationTime : DateTime.Now;
                                data.CardTypeSysNo = card.CouponCard.CardTypeSysNo;
                                data.CouponCardNo = card.CouponCard.CouponCardNo;
                                data.Status = card.CouponCard.Status;
                                data.TerminationTime = card.CouponCard.TerminationTime > DateTime.MinValue ? card.CouponCard.TerminationTime : DateTime.Now;
                                if (card.CouponCardType != null)
                                {
                                    data.CouponCardType = new AppSpCouponCardType
                                        {
                                            SysNo = card.CouponCardType.SysNo,
                                            TypeName = card.CouponCardType.TypeName,
                                            TypeDescription = card.CouponCardType.TypeDescription,
                                            StartTime = card.CouponCardType.StartTime,
                                            EndTime = card.CouponCardType.EndTime,
                                            Status = card.CouponCardType.Status
                                        };
                                }
                                if (card.Coupons.Any())
                                {
                                    data.Coupons = card.Coupons.Select(x =>
                                        {
                                            var model = new AppSpCoupon
                                                {
                                                    AuditDate =
                                                        x.AuditDate > DateTime.MinValue ? x.AuditDate : DateTime.Now,
                                                    AuditorSysNo = x.AuditorSysNo,
                                                    CouponAmount = x.CouponAmount,
                                                    CouponCode = x.CouponCode,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate =
                                                        x.CreatedDate > DateTime.MinValue ? x.CreatedDate : DateTime.Now,
                                                    CustomerSysNo = x.CustomerSysNo,
                                                    Description = x.Description,
                                                    EndTime = x.EndTime,
                                                    LastUpdateBy = x.LastUpdateBy,
                                                    LastUpdateDate =
                                                        x.LastUpdateDate > DateTime.MinValue
                                                            ? x.LastUpdateDate
                                                            : DateTime.Now,
                                                    PromotionSysNo = x.PromotionSysNo,
                                                    RequirementAmount = x.RequirementAmount,
                                                    SourceDescription = x.SourceDescription,
                                                    StartTime = x.StartTime,
                                                    Status = x.Status,
                                                    SysNo = x.SysNo,
                                                    Type = x.Type,
                                                    UseQuantity = x.UseQuantity,
                                                    UsedQuantity = x.UsedQuantity
                                                };
                                            var spCouponCardAssociate =
                                                card.Associations.FirstOrDefault(
                                                    o =>
                                                    o.CardTypeSysNo == card.CouponCardType.SysNo &&
                                                    o.CouponSysNo == x.SysNo);
                                            if (spCouponCardAssociate != null)
                                            {
                                                model.BindNumber = spCouponCardAssociate.BindNumber;
                                            }
                                            return model;
                                        }).ToList();
                                }
                                result.Data = data;
                            }
                        }
                        else
                        {
                            result.Message = "未找到优惠卡,请确认输入的优惠卡号是否正确!";
                        }
                        result.Status = true;
                        result.StatusCode = 0;
                    }
                    else
                    {
                        result.Message = "优惠卡号不能为空!";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "通过优惠卡号获取优惠卡" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 绑定优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        /// <remarks>绑定优惠卡后，有自动审核动作</remarks>
        public Result AssignCouponCard(string couponCardNo, int customerSysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var card = SpCouponCardBo.Instance.GetAggregatedCouponCard(couponCardNo);
                    if (card != null && card.Coupons.Any())
                    {

                        var i = 1;
                        foreach (var item in card.Coupons)
                        {
                            result.StatusCode += SpCouponCardBo.Instance.AssignToCustomer(couponCardNo, item.SysNo, customerSysNo, i, Authorization.Data, Audit);
                            i++;
                        }

                        result.Status = result.StatusCode > 0;
                        if (!result.Status)
                        {
                            result.Message = "优惠卡绑定失败!";
                        }
                    }
                    else
                    {
                        result.Message = "未找到优惠卡!";
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "绑定优惠卡" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 用户绑定优惠券审核
        /// </summary>
        /// <param name="couponSysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        private void Audit(int couponSysNo)
        {
            SpCouponBo.Instance.Audit(couponSysNo, Authorization.Data);
        }

        /// <summary>
        /// 待绑定到用户的优惠卷
        /// </summary>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="description">优惠卷描述</param>
        /// <returns>分页列表</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result<Pager<AppSpCoupon>> GetCouponsToBeAssigned(int currentPageIndex, int pageSize, string description)
        {
            var result = new Result<Pager<AppSpCoupon>> { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {
                    var filter = new ParaCoupon
                        {
                            Id = currentPageIndex,
                            PageSize = pageSize,
                            LogisticsAppPlatform = PromotionStatus.物流App使用.是.GetHashCode(),
                            Description = description
                        };
                    var pager = SpCouponBo.Instance.GetCouponsToBeAssigned(filter);
                    if (pager != null && pager.Rows != null && pager.Rows.Any())
                    {
                        var list = new Pager<AppSpCoupon>
                            {
                                CurrentPage = currentPageIndex,
                                PageSize = pageSize,
                                TotalRows = pager.TotalRows,
                                PageFilter = null,
                                Rows = pager.Rows.Select(x => new AppSpCoupon
                                    {
                                        AuditDate = x.AuditDate > DateTime.MinValue ? x.AuditDate : DateTime.Now,
                                        AuditorSysNo = x.AuditorSysNo,
                                        CouponAmount = x.CouponAmount,
                                        CouponCode = x.CouponCode,
                                        CreatedBy = x.CreatedBy,
                                        CreatedDate = x.CreatedDate > DateTime.MinValue ? x.CreatedDate : DateTime.Now,
                                        CustomerSysNo = x.CustomerSysNo,
                                        Description = x.Description,
                                        EndTime = x.EndTime,
                                        LastUpdateBy = x.LastUpdateBy,
                                        LastUpdateDate = x.LastUpdateDate > DateTime.MinValue ? x.LastUpdateDate : DateTime.Now,
                                        PromotionSysNo = x.PromotionSysNo,
                                        RequirementAmount = x.RequirementAmount,
                                        SourceDescription = x.SourceDescription,
                                        StartTime = x.StartTime,
                                        Status = x.Status,
                                        SysNo = x.SysNo,
                                        Type = x.Type,
                                        UseQuantity = x.UseQuantity,
                                        UsedQuantity = x.UsedQuantity
                                    }).ToList()
                            };

                        result.Data = list;
                    }
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "待绑定到用户的优惠卷" + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 绑定优惠券
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        public Result AssignUserCoupon(int sysNo, int customerSysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (Authorization.Status)
                {

                    var carid = SpCouponBo.Instance.AssignToCustomer(sysNo, customerSysNo, Authorization.Data);
                    SpCouponBo.Instance.Audit(carid, Authorization.Data);

                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "绑定优惠券" + ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region 二次销售接口 朱成果 2014-09-17
        /// <summary>
        /// 创建业务员调价APP订单 到结算步骤
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        /// <remarks>
        /// 2014-09-16 杨文兵 创建
        /// 注意：需要在服务器端做验证，验证调价之后价格是否小于最低价格，小于则不允许。
        /// 注意：服务器端需要做验证，业务员是否做了调价操作，对订单做不同的标识
        /// </remarks>
        public Result<AppSoOrder> CreateSoOrderToSettlement(AppOrder2 order)
        {
            var bll = TwoSaleBo.Instance;
            Result<AppSoOrder> result = new Result<AppSoOrder>() { Status = true };
            try
            {
                if (Authorization.Status)//是否已经登录
                { 
                   var user = Authorization.Data;//用户信息
                   // var user = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(1501);
                    AppOrder neworder = Hyt.BLL.Order.TwoSaleBo.Instance.MapToAppOrder(order);//订单对象转换
                    using (var tran = new TransactionScope())
                    {
                        neworder = bll.CreateTwoSaleSoOrder(neworder, user, true);
                        bll.DeliveryTwoSaleSoOrder(neworder, user);
                        tran.Complete();
                    }
                    result.Status = true;
                    result.StatusCode = neworder.SoOrder.SysNo;
                    if(neworder!=null&&neworder.SoOrder!=null)
                    {
                        //neworder.SoOrder = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(neworder.SoOrder.SysNo);//获取最新的订单数据
                        result.Data = CreateAppSoOrder(order.Order, neworder.SoOrder);//返回APP接收数据
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (HttpException ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            catch (Exception exx)
            {
                result.Status = false;
                result.Message = exx.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "创建业务员调价APP订单到订单审核步骤错误", exx);
            }
            return result;
        }

        /// <summary>
        /// 创建业务员调价APP订单 到订单审核步骤 将订单加入任务池
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>返回结果</returns>
        ///  <remarks>2014-09-16 杨文兵 创建
        ///  注意：需要在服务器端做验证，验证调价之后价格是否小于最低价格，小于则不允许。
        ///  注意：服务器端需要做验证，业务员是否做了调价操作，对订单做不同的标识
        ///  </remarks>
        public Result<AppSoOrder> CreateSoOrderToAudit(AppOrder2 order)
        {
            var bll = TwoSaleBo.Instance;
            Result<AppSoOrder> result = new Result<AppSoOrder>() { Status = true };
            try
            {
                if (Authorization.Status)//是否已经登录
                {
                    var user = Authorization.Data;//用户信息
                    AppOrder neworder = Hyt.BLL.Order.TwoSaleBo.Instance.MapToAppOrder(order);//订单对象转换
                    using (var tran = new TransactionScope())
                    {
                        neworder = bll.CreateTwoSaleSoOrder(neworder, user, false);
                        tran.Complete();
                    }
                    result.Status = true;
                    result.StatusCode = neworder.SoOrder.SysNo;
                    if (neworder != null && neworder.SoOrder != null)
                    {
                        //neworder.SoOrder = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(neworder.SoOrder.SysNo);//获取最新的订单数据
                        result.Data = CreateAppSoOrder(order.Order, neworder.SoOrder);//返回APP接收数据
                    }
                }
                else
                {
                    result.Message = Authorization.Message;
                    result.Status = Authorization.Status;
                    result.StatusCode = Authorization.StatusCode;
                }
            }
            catch (HttpException ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            catch (Exception exx)
            {
                result.Status = false;
                result.Message = exx.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "创建业务员调价APP订单到订单审核步骤错误", exx);
            }
            return result;

        }
        /// <summary>
        /// APP 下单（带促销）到待结算
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <param name="order">App订单信息</param>
        /// <returns></returns>
        /// <remarks>2014-09-29 朱成果 修改</remarks>
        public  Result<AppSoOrder> CreatedOrderAndDelivery(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order)
        {

            var result = new Result<AppSoOrder> { StatusCode = -1 };
            if (!Authorization.Status)
            {
                result.Message = Authorization.Message;
                result.Status = Authorization.Status;
                result.StatusCode = Authorization.StatusCode;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CrShoppingCartToCacheBo.Instance.RemoveAll(cacheKey, 0);
            }
            var shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.物流App }, order.CustomerSysNo, null, receiveAddress.AreaSysNo,
                                                                 order.DeliveryTypeSysNo, null, order.CouponCode,
                                                                 true, false);               // 余勇 修改isFrontProduct为false 
            return CreateAppOrder(shoppingCart, receiveAddress, order, "App提交订单,并配送",true);
        }
        #endregion

        #region 心怡科技物流
        public static string appkey_xinyi = "1111";
        public static string AppSecret_xinyi = "1111";
        public static string datatype_xinyi = "json";
        public static string method_xinyi = "OMS_PUSH_PLT_PRODUCT_SKU_V2";
        public static string url_xinyi = "http://183.63.175.210:9919/gateway.aspx";
        public static string timestamp_xinyi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 推送订单到心怡科技物流
        /// </summary>
        /// <returns>2015-10-10 王耀发 创建</returns>
        public Result SendOrderToXinYi(string postdata)
        {
            //postdata = "{\"Goods\": [{\"ShortName\": \"试商\",\"ShorthandCodes\": \"123\",\"SecBARCode\": \"123456789\",\"Manufactory\": \"心怡\",";
            //postdata += "\"Brand\": \"第一品牌\",\"Quality\": \"12\",\"Original\": \"110\",\"PurchasePlace\": \"1101\",\"PackageType\": \"005\",\"QualityCertify\": \"合格\",\"GoodsBatchNo\": \"合格\",";
            //postdata += "\"IttNumber\": \"5000000\",\"GrossWt\": \"30\",\"NetWt\": \"28\",\"Volume\": \"10\",\"ExpirationDate\": \"360\",\"CodeTS\": \"123456\",\"DecPrice\": \"99\",\"PostTariffCode\": 2060000,\"IEFlag\": \"I\",";
            //postdata += "\"PostTariffName\": \"行邮税\",\"GNote\": \"http://www.baidu.com\",\"HSTax\": \"0\",\"PostTax\": \"0\",\"HSCode\": \"1234\",\"TradeCountry\": \"110\",\"WarehouseCode\": \"STORE_GZAP\",\"OnNumber\": \"1013\",";
            //postdata += "\"GNo\": \"5\",\"CopGNo\": \"HT-B75-000009\",\"ProGNo\": \"871101201510121033\",\"GName\": \"全绿商品881101201503253550\",\"GModel\": \"测试规格\",\"BARCode\": \"1234567890\",\"Unit\": \"7\",";
            //postdata += "\"SecUnit\": \"7\",\"GoodsMes\": \"测试商品\",\"Notes\": \"Test\",\"ItSkuColor\": \"颜色\",\"ItSkuSize\": \"大小\",\"OpType\": \"新增\"}]}";
            Result result = new Result();
            ///获得接口返回值
            var sAPIResult = "";
            try
            {
                sAPIResult = Post(url_xinyi, postdata);
                sAPIResult = sAPIResult.Trim('{').Trim('}');
                string[] a = sAPIResult.Split(',');
                string[] b;
                string c = "";
                foreach (string sArray in a)
                {
                    b = sArray.Split(':');
                    if (c == "")
                    {
                        c = b[1].Trim('"');
                    }
                    else
                    {
                        c += ',' + b[1].Trim('"');
                    }
                }
                string d = c.Trim('"');
                string[] e = d.Split(',');
                if (e[0] == "1")
                {
                    result.Status = true;
                    result.StatusCode = int.Parse(e[0]);
                    result.Message = e[3];
                }
                if (e[0] == "0")
                {
                    result.Status = false;
                    result.StatusCode = int.Parse(e[0]);
                    //根据错误编号，判断错误信息
                    switch(e[2].ToString())
                    {
                        case "S0001":
                            result.Message = "请求参数不正确";
                            break;
                        case "S0002":
                            result.Message = "数据签名错误";
                            break;
                        case "S0003":
                            result.Message = "非法的服务名称";
                            break;
                        case "S0004":
                            result.Message = "非法的数据类型";
                            break;
                        case "S0005":
                            result.Message = "非法的合作伙伴";
                            break;
                        case "S0006":
                            result.Message = "非法的请求方法";
                            break;
                        case "S9998":
                            result.Message = "非法的请求方法，只有用POST提交";
                            break;
                        case "S9999":
                            result.Message = "系统异常，请重试";
                            break;
                        default:
                            result.Message = e[3].ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.StatusCode = 0;
                sAPIResult = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns>2015-10-10 王耀发 创建</returns>
        public string Post(string url, string postdata)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appkey", appkey_xinyi);
            parameters.Add("method", method_xinyi);
            parameters.Add("timestamp", timestamp_xinyi);
            parameters.Add("datatype", datatype_xinyi);
            parameters.Add("postdata", System.Web.HttpUtility.UrlEncode(postdata));
            string sign = CreateSign(postdata, timestamp_xinyi, method_xinyi, AppSecret_xinyi);

            url += "?appkey=" + parameters["appkey"];
            url += "&method=" + parameters["method"];
            url += "&timestamp=" + parameters["timestamp"];
            url += "&datatype=" + parameters["datatype"];
            url += "&sign=" + sign;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("postdata={0}", parameters["postdata"]);
            return HttpResponse(url, "utf-8", sb.ToString(), "post");
        }
        /// <summary>
        /// post 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns>2015-10-10 王耀发 创建</returns>
        static string HttpResponse(string url, string encode = "utf-8", string data = "", string method = "get")
        {
            HttpWebRequest request;
            HttpWebResponse response;
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "zh-cn");
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.2; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; InfoPath.2; CIBA; .NET4.0C; .NET4.0E)";
            request.Method = (string.IsNullOrEmpty(method) ? "get" : method);


            if (request.Method.ToLower().Equals("post") && !string.IsNullOrEmpty(data))
            {
                byte[] b = Encoding.GetEncoding(encode).GetBytes(data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = b.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(b, 0, b.Length);
                }
            }
            string html = string.Empty;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encode)))
                {

                    html = reader.ReadToEnd();
                }
            }
            return html;
        }

        /// <summary>
        /// 给请求签名
        /// </summary>
        /// <param name="postData">请求的数据包</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="method">API接口名称</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public string CreateSign(string postData, string timestamp, string method, string secret)
        {
            string content = postData + timestamp + method + secret;
            return EncrypMD5(content);

        }
        /// <summary>
        /// base64 MD5加密
        /// </summary>
        /// <param name="content">要加密的字串</param>
        /// <returns>加密后的数字字串</returns>
        public string EncrypMD5(string content)
        {
            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(bytes);
        }
        #endregion

        #region 海关
        /// <summary>
        /// 获取海关配置信息
        /// </summary>
        private static CustomsConfig customsConfig = Hyt.BLL.Config.Config.Instance.GetCustomsConfig();

        /// <summary>
        /// 广州海关报文采用标准的AES加密，加密的密钥 Key
        /// </summary>
        private static String Key = customsConfig.Key;
        /// <summary>
        /// 报文类型 880020-电子订单
        /// </summary>
        public static string MessageType = customsConfig.MessageType;
        /// <summary>
        /// 信营企业备案号
        /// </summary>
        public static string XinYinSenderID = customsConfig.SenderID;
        /// <summary>
        /// 信营海关FTP地址
        /// </summary>
        public static string FtpUrl = customsConfig.FtpUrl;
        /// <summary>
        /// 信营海关FTP地址用户名
        /// </summary>
        public static string FtpUserName = customsConfig.FtpUserName;
        /// <summary>
        /// 信营海关FTP地址密码
        /// </summary>
        public static string FtpPassword = customsConfig.FtpPassword;
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version = customsConfig.Version;
        /// <summary>
        /// 上传xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText"></param>
        public string UploadXmlFile(string RequestText)
        {
            //获取当前时间
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            int Day = DateTime.Now.Day;
            int Hour = DateTime.Now.Hour;
            int Minute = DateTime.Now.Minute;
            int Second = DateTime.Now.Second;
            string SendTime = Year.ToString() + (Month < 10 ? "0" + Month.ToString() : Month.ToString()) + (Day < 10 ? "0" + Day.ToString() : Day.ToString());
            SendTime += (Hour < 10 ? "0" + Hour.ToString() : Hour.ToString()) + (Minute < 10 ? "0" + Minute.ToString() : Minute.ToString()) + (Second < 10 ? "0" + Second.ToString() : Second.ToString());

            var jsonObject = JObject.Parse(RequestText);

            string MessageIDNo = jsonObject["MessageIDNo"].ToString();
            string OrderId = jsonObject["Declaration"]["EOrder"]["OrderId"].ToString();
            string IEFlag = jsonObject["Declaration"]["EOrder"]["IEFlag"].ToString();
            string OrderStatus = jsonObject["Declaration"]["EOrder"]["OrderStatus"].ToString();
            string EntRecordNo = jsonObject["Declaration"]["EOrder"]["EntRecordNo"].ToString();
            string EntRecordName = jsonObject["Declaration"]["EOrder"]["EntRecordName"].ToString();
            string OrderName = jsonObject["Declaration"]["EOrder"]["OrderName"].ToString();
            string OrderDocType = jsonObject["Declaration"]["EOrder"]["OrderDocType"].ToString();
            string OrderDocId = jsonObject["Declaration"]["EOrder"]["OrderDocId"].ToString();
            string OrderPhone = jsonObject["Declaration"]["EOrder"]["OrderPhone"].ToString();
            string OrderGoodTotal = jsonObject["Declaration"]["EOrder"]["OrderGoodTotal"].ToString();
            string OrderGoodTotalCurr = jsonObject["Declaration"]["EOrder"]["OrderGoodTotalCurr"].ToString();
            string Freight = jsonObject["Declaration"]["EOrder"]["Freight"].ToString();
            string FreightCurr = jsonObject["Declaration"]["EOrder"]["FreightCurr"].ToString();
            string Tax = jsonObject["Declaration"]["EOrder"]["Tax"].ToString();
            string TaxCurr = jsonObject["Declaration"]["EOrder"]["TaxCurr"].ToString();
            string Note = jsonObject["Declaration"]["EOrder"]["Note"].ToString();
            string OrderDate = jsonObject["Declaration"]["EOrder"]["OrderDate"].ToString();


            string strxml = "";
            strxml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strxml += "<Manifest>";
            strxml += "<Head>";
            strxml += "<MessageID>" + MessageType + SendTime + MessageIDNo + "</MessageID>";
            strxml += "<MessageType>" + MessageType + "</MessageType>";
            strxml += "<SenderID>" + XinYinSenderID + "</SenderID>";
            strxml += "<SendTime>" + SendTime + "</SendTime>";
            strxml += "<Version>" + Version + "</Version>";
            strxml += "</Head>";
            strxml += "<Declaration>";
            strxml += "<EOrder>";
            strxml += "<OrderId>" + OrderId + "</OrderId>";
            strxml += "<IEFlag>" + IEFlag + "</IEFlag>";
            strxml += "<OrderStatus>" + OrderStatus + "</OrderStatus>";
            strxml += "<EntRecordNo>" + EntRecordNo + "</EntRecordNo>";
            strxml += "<EntRecordName>" + EntRecordName + "</EntRecordName>";
            strxml += "<OrderName>" + OrderName + "</OrderName>";
            strxml += "<OrderDocType>" + OrderDocType + "</OrderDocType>";
            strxml += "<OrderDocId>" + OrderDocId + "</OrderDocId>";
            strxml += "<OrderPhone>" + OrderPhone + "</OrderPhone>";
            strxml += "<OrderGoodTotal>" + OrderGoodTotal + "</OrderGoodTotal>";
            strxml += "<OrderGoodTotalCurr>" + OrderGoodTotalCurr + "</OrderGoodTotalCurr>";
            strxml += "<Freight>" + Freight + "</Freight>";
            strxml += "<FreightCurr>" + FreightCurr + "</FreightCurr>";
            strxml += "<Tax>" + Tax + "</Tax>";
            strxml += "<TaxCurr>" + TaxCurr + "</TaxCurr>";
            strxml += "<Note>" + Note + "</Note>";
            strxml += "<OrderDate>" + OrderDate + "</OrderDate>";
            strxml += "</EOrder>";
            strxml += "<EOrderGoods>";

            foreach (JObject item in jsonObject["Declaration"]["EOrderGoods"])
            {
                strxml += "<EOrderGood>";
                strxml += "<GNo>" + item["GNo"].ToString() + "</GNo>";
                strxml += "<ChildOrderNo>" + item["ChildOrderNo"].ToString() + "</ChildOrderNo>";
                strxml += "<StoreRecordNo>" + item["StoreRecordNo"].ToString() + "</StoreRecordNo>";
                strxml += "<StoreRecordName>" + item["StoreRecordName"].ToString() + "</StoreRecordName>";
                strxml += "<CopGNo>" + item["CopGNo"].ToString() + "</CopGNo>";
                strxml += "<CustomsListNO>" + item["CustomsListNO"].ToString() + "</CustomsListNO>";
                strxml += "<DecPrice>" + item["DecPrice"].ToString() + "</DecPrice>";
                strxml += "<Unit>" + item["Unit"].ToString() + "</Unit>";
                strxml += "<GQty>" + item["GQty"].ToString() + "</GQty>";
                strxml += "<DeclTotal>" + item["DeclTotal"].ToString() + "</DeclTotal>";
                strxml += "<Notes>" + item["Notes"].ToString() + "</Notes>";
                strxml += "</EOrderGood>";
            }
            strxml += "</EOrderGoods>";
            strxml += "</Declaration>";
            strxml += "</Manifest>";
            try
            {
                strxml = Encrypt(strxml);
                //上传xml文件
                MemoryStream stream = new MemoryStream();
                byte[] buffer = Encoding.Default.GetBytes(strxml);
                string msg = "";
                string _ftpImageServer = FtpUrl + "UPLOAD/";
                string _ftpUserName = FtpUserName;
                string _ftpPassword = FtpPassword;
                FtpUtil ftp = new FtpUtil(_ftpImageServer, _ftpUserName, _ftpPassword);
                //上传xml文件
                ftp.UploadFile(_ftpImageServer, MessageType + SendTime + MessageIDNo + ".xml", buffer, out msg);
                return MessageType + SendTime + MessageIDNo + ".xml";
            }
            catch (Exception e)
            {
                //显示错误信息  
                return "";
            }
        }
        /// <summary>
        /// 下载xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText"></param>
        public string DownloadXmlFile(string RequestText)
        {
            var jsonObject = JObject.Parse(RequestText);
            string FileName = jsonObject["FileName"].ToString();
            string _ftpImageServer = FtpUrl + "DOWNLOAD/";
            string _ftpUserName = FtpUserName;
            string _ftpPassword = FtpPassword;
            FtpUtil ftp = new FtpUtil(_ftpImageServer, _ftpUserName, _ftpPassword);
            string msg = "";
            ftp.DownloadFile(_ftpImageServer + FileName, HttpContext.Current.Server.MapPath("~/Xml"), out msg);
            StreamReader objReader = new StreamReader(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName);
            string sLine = objReader.ReadToEnd();
            objReader.Close();
            //删除对应文件
            if (File.Exists(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName);
            }
            return Decrypt(sLine);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #region 支付宝对接海关
        #region 字段
        /// <summary>
        /// 获取支付宝海关配置信息
        /// </summary>
        private static AlipayCustomsConfig alipaycustomsConfig = Hyt.BLL.Config.Config.Instance.GetAlipayCustomsConfig();

        //支付宝网关地址（新）
        private static string GATEWAY_NEW = alipaycustomsConfig.GATEWAY_NEW;
        //商户的私钥
        private static string _key = alipaycustomsConfig.key;
        //编码格式
        private static string _input_charset = alipaycustomsConfig.input_charset;
        //签名方式
        private static string _sign_type = alipaycustomsConfig.sign_type;
        //合作者身份ID
        public static string partner = alipaycustomsConfig.partner;
        //接口名称
        public static string service = alipaycustomsConfig.service;
        //商户在海关备案的编号
        public static string merchant_customs_code = alipaycustomsConfig.merchant_customs_code;
        //商户在海关备案的名称
        public static string merchant_customs_name = alipaycustomsConfig.merchant_customs_name;
        //海关编号
        public static string customs_place = alipaycustomsConfig.customs_place;
        #endregion
        //把请求参数打包成数组
        public Result SendOrderToAlipay(string postdata)
        {
            //postdata = "{\"out_request_no\": \"20151015001\",\"trade_no\": \"2015051446800462\",\"amount\": \"200.15\"}";
            var jsonObject = JObject.Parse(postdata);
            string out_request_no = jsonObject["out_request_no"].ToString();
            string trade_no = jsonObject["trade_no"].ToString();
            string amount = jsonObject["amount"].ToString();

            Result result = new Result();
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("_input_charset", _input_charset.ToLower());
            sParaTemp.Add("service", service);
            sParaTemp.Add("out_request_no", out_request_no);
            sParaTemp.Add("trade_no", trade_no);
            sParaTemp.Add("merchant_customs_code", merchant_customs_code);
            sParaTemp.Add("merchant_customs_name", merchant_customs_name);
            sParaTemp.Add("amount", amount);
            sParaTemp.Add("customs_place", customs_place);

            //建立请求
            string sHtmlText = BuildRequest(sParaTemp);
            string msg = "";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                string is_success = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (is_success == "F")
                {
                    string error = xmlDoc.SelectSingleNode("/alipay/error").InnerText;
                    switch (error)
                    {
                        case "ILLEGAL_SIGN":
                            msg = "签名不正确";
                            break;
                        case "ILLEGAL_DYN_MD5_KEY":
                            msg = "动态密钥信息错误";
                            break;
                        case "ILLEGAL_ENCRYPT":
                            msg = "加密不正确";
                            break;
                        case "ILLEGAL_SERVICE":
                            msg = "Service参数不正确";
                            break;
                        case "ILLEGAL_USER":
                            msg = "用户ID不正确";
                            break;
                        case "ILLEGAL_PARTNER":
                            msg = "合作伙伴ID不正确";
                            break;
                        case "ILLEGAL_EXTERFACE":
                            msg = "接口配置不正确";
                            break;
                        case "ILLEGAL_PARTNER_EXTERFACE":
                            msg = "合作伙伴接口信息不正确";
                            break;
                        case "ILLEGAL_SECURITY_PROFILE":
                            msg = "未找到匹配的密钥配置";
                            break;
                        case "ILLEGAL_AGENT":
                            msg = "代理ID不正确";
                            break;
                        case "ILLEGAL_SIGN_TYPE":
                            msg = "签名类型不正确";
                            break;
                        case "ILLEGAL_CHARSET":
                            msg = "字符集不合法";
                            break;
                        case "HAS_NO_PRIVILEGE":
                            msg = "无权访问";
                            break;
                        case "INVALID_CHARACTER_SET":
                            msg = "字符集无效";
                            break;
                        case "SYSTEM_ERROR":
                            msg = "支付宝系统错误";
                            break;
                        case "SESSION_TIMEOUT":
                            msg = "session 超时";
                            break;
                        case "ILLEGAL_TARGET_SERVICE":
                            msg = "错误的target_service";
                            break;
                        case "ILLEGAL_ACCESS_SWITCH_SYSTEM":
                            msg = "partner 不允许访问该类型的系统";
                            break;
                        case "EXTERFACE_IS_CLOSED":
                            msg = "接口已关闭";
                            break;
                        default:
                            msg = "";
                            break;
                    }
                    result.Status = false;
                    result.Message = msg;
                }
                if (is_success == "T")
                {
                    result.Status = true;
                    result.Message = sHtmlText;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }


        #region sumit
        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = CreateLinkString(sPara);

            //把最终的字符串签名，获得签名结果
            string mysign = "";
            switch (_sign_type)
            {
                case "MD5":
                    mysign = Sign(prestr, _key, _input_charset);
                    break;
                default:
                    mysign = "";
                    break;
            }

            return mysign;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //签名结果
            string mysign = "";

            //过滤签名参数数组
            sPara = FilterPara(sParaTemp);

            //获得签名结果
            mysign = BuildRequestMysign(sPara);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }


        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp)
        {
            Encoding code = Encoding.GetEncoding(_input_charset);

            //待请求参数数组字符串
            string strRequestData = BuildRequestParaToString(sParaTemp, code);

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = code.GetBytes(strRequestData);

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }

            return strResult;
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果，带文件上传功能
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">文件内容类型</param>
        /// <param name="lengthFile">文件长度</param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string fileName, byte[] data, string contentType, int lengthFile)
        {

            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //设置HttpWebRequest基本信息
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //设置请求方式：get、post
            request.Method = strMethod;
            //设置boundaryValue
            string boundaryValue = DateTime.Now.Ticks.ToString("x");
            string boundary = "--" + boundaryValue;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + boundaryValue;
            //设置KeepAlive
            request.KeepAlive = true;
            //设置请求数据，拼接成字符串
            StringBuilder sbHtml = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dicPara)
            {
                sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"" + key.Key + "\"\r\n\r\n" + key.Value + "\r\n");
            }
            sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"withhold_file\"; filename=\"");
            sbHtml.Append(fileName);
            sbHtml.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string postHeader = sbHtml.ToString();
            //将请求数据字符串类型根据编码格式转换成字节流
            Encoding code = Encoding.GetEncoding(_input_charset);
            byte[] postHeaderBytes = code.GetBytes(postHeader);
            byte[] boundayBytes = Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
            //设置长度
            long length = postHeaderBytes.Length + lengthFile + boundayBytes.Length;
            request.ContentLength = length;

            //请求远程HTTP
            Stream requestStream = request.GetRequestStream();
            Stream myStream;
            try
            {
                //发送数据请求服务器
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(data, 0, lengthFile);
                requestStream.Write(boundayBytes, 0, boundayBytes.Length);
                HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
                myStream = HttpWResp.GetResponseStream();
            }
            catch (WebException e)
            {
                return e.ToString();
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }

            //读取支付宝返回处理结果
            StreamReader reader = new StreamReader(myStream, code);
            StringBuilder responseData = new StringBuilder();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }
            myStream.Close();
            return responseData.ToString();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public static string Query_timestamp()
        {
            string url = GATEWAY_NEW + "service=query_timestamp&partner=" + partner + "&_input_charset=" + _input_charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }
        #endregion

        #region Core
        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void LogResult(string sWord)
        {
            string strPath = HttpContext.Current.Server.MapPath("log");
            strPath = strPath + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
            StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="sFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(Stream sFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(sFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="dataFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(byte[] dataFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(dataFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        #endregion

        #region MD5
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign">签名结果</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>验证结果</returns>
        public static bool Verify(string prestr, string sign, string key, string _input_charset)
        {
            string mysign = Sign(prestr, key, _input_charset);
            if (mysign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion
    }
}
