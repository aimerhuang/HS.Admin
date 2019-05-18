using Hyt.BLL.Basic;
using Hyt.BLL.Mobile;
using Hyt.BLL.Sys;
using Hyt.Model;
using Hyt.Model.Mobile;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.MobileControl
{
    public class CustomerAddressControl
    {
        /// <summary>
        /// 保存会员地址
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="address">地址</param>
        /// <returns></returns>
        public object SaveCustomerAddress(string token, CrReceiveAddress address)
        {
            Result result = new Result();
            try
            {
                CrCustomerMobileLogin Sylogin = CrCustomerMobileLoginBo.Instance.GetModByTokenAndDeviceCode(token, "");
                if (Sylogin != null)
                {
                    address.CustomerSysNo = Sylogin.CustomerSysNo;
                    Hyt.BLL.CRM.CrReceiveAddressBo.Instance.InsertReceiveAddress(address);
                    result.Status = true;
                    result.Message = "保存成功";
                }
                else
                {
                    result.Status = false;
                    result.Message = "未登录，请登录后再操作";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取会员的地址列表
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>返回json数据</returns>
        public object GetCustomerAddressList(string token)
        {
            Result result = new Result();
            try
            {
                CrCustomerMobileLogin Sylogin = CrCustomerMobileLoginBo.Instance.GetModByTokenAndDeviceCode(token, "");
                if (Sylogin != null)
                {
                    List<BsArea> areaList = BasicAreaBo.Instance.GetAll();
                    var addressList = Hyt.BLL.CRM.CrReceiveAddressBo.Instance.GetCrReceiveAddressByCustomerSysNo(Sylogin.CustomerSysNo, 50);
                    List<object> objAddressList = new List<object>();
                    foreach (var mod in addressList)
                    {
                        BsArea area = areaList.Find(p => p.SysNo == mod.AreaSysNo);
                        BsArea city = areaList.Find(p => p.SysNo == area.ParentSysNo);
                        BsArea pro = areaList.Find(p => p.SysNo == city.ParentSysNo);
                        objAddressList.Add(new { 
                            SysNo=mod.SysNo,
                            Name = mod.Name, 
                            Phone = mod.PhoneNumber,
                            ProvinceCode = pro.SysNo,
                            Province = pro.AreaName,
                            CityCode = city.SysNo,
                            City = city.AreaName,
                            DistrictCode = area.SysNo,
                            District = area.AreaName,
                            IDCardNo = mod.IDCardNo
                        });
                    }
                    return new { Status = true, Message = "完成", List = objAddressList };
                }
                else
                {
                    result.Status = false;
                    result.Message = "未登录，请登录后再操作";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取地址数据
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="SysNo">编号</param>
        /// <returns>返回地址json数据</returns>
        public object GetCustomerAddress(string token, int SysNo)
        {
            Result result = new Result();
            try
            {
                CrCustomerMobileLogin Sylogin = CrCustomerMobileLoginBo.Instance.GetModByTokenAndDeviceCode(token, "");
                if (Sylogin != null)
                {

                    var mod = Hyt.BLL.CRM.CrReceiveAddressBo.Instance.GetCrReceiveAddress(SysNo);
                    List<BsArea> areaList = BasicAreaBo.Instance.GetAll();
                    BsArea area = areaList.Find(p => p.SysNo == mod.AreaSysNo);
                    BsArea city = areaList.Find(p => p.SysNo == area.ParentSysNo);
                    BsArea pro = areaList.Find(p => p.SysNo == city.ParentSysNo);
                    return new
                    {
                        Status = true,
                        Message = "完成",
                        Data = new
                        {
                            SysNo = mod.SysNo,
                            Name = mod.Name,
                            Phone = mod.PhoneNumber,
                            ProvinceCode = pro.SysNo,
                            Province = pro.AreaName,
                            CityCode = city.SysNo,
                            City = city.AreaName,
                            DistrictCode = area.SysNo,
                            District = area.AreaName,
                            IDCardNo = mod.IDCardNo
                        }
                    };
                }
                else
                {
                    result.Status = false;
                    result.Message = "未登录，请登录后再操作";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="deviceName">手机设备名称</param>
        /// <param name="deviceCode">设备编码</param>
        /// <returns>登录检查</returns>
        public object CheckUserLogin(string account, string password, string deviceName, string deviceCode)
        {
            Result result = new Result();
            var syUser = SyUserBo.Instance.GetSyUser(account);
            if (syUser != null)
            {
                if (syUser.Status == (int)SystemStatus.系统用户状态.禁用)
                {
                    result.Status = false;
                    result.Message = "该账户已被禁用,请联系管理员！";
                    return result;
                }
                if (!Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(password, syUser.Password))
                {
                    result.Status = false;
                    result.Message = "账户名或密码错误！";
                    return result;
                }
                
                CrCustomerMobileLogin login = CrCustomerMobileLoginBo.Instance.GetModByCustomerSysNo(syUser.SysNo);
                if (login == null)
                {
                    login = new CrCustomerMobileLogin();
                    login.CustomerSysNo = syUser.SysNo;
                    login.CustomerToken = Hyt.Util.EncryptionUtil.MD5Encrypt(account);
                    login.SysNo = CrCustomerMobileLoginBo.Instance.InsertMod(login);
                }

                login.LastLoginTime = DateTime.Now;
                login.CustomerLoginKey = Hyt.Util.EncryptionUtil.MD5Encrypt(account + deviceCode + login.LastLoginTime.ToString("yyyyMMddHHmmss"));
                login.CustomerLoginDevice = deviceName;
                login.CustomerLoginDeviceCode = deviceCode;
                CrCustomerMobileLoginBo.Instance.UpdateMod(login);
                ///手机登录记录
                CrCustomerMobileLoginHistory historyMod = new CrCustomerMobileLoginHistory() {
                    CustomerMobileSysNo = login.SysNo,
                    CustomerLoginDevice = login.CustomerLoginDevice,
                    CustomerLoginDeviceCode = login.CustomerLoginDeviceCode,
                    CustomerLoginKey = login.CustomerLoginKey,
                    CustomerLoginTime = login.LastLoginTime 
                };

                CrCustomerMobileLoginBo.Instance.InserHistoryMod(historyMod);

                return new { Status = true, Message = "登录成功", Token = login.CustomerToken, LoginKey = login.CustomerLoginKey };
            }
            else
            {
                result.Status = false;
                result.Message = "账户名或密码错误！";
                return result;
            }
        }
    }
}
