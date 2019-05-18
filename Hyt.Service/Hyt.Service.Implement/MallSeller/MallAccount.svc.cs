using System.Linq;
using Hyt.BLL.Distribution;
using Hyt.BLL.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.MallSeller;
using System;
using System.Collections.Generic;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.BLL.Log;
using Hyt.Service.Implement;
using Hyt.Model.UpGrade;

namespace Hyt.Service.Implement.MallSeller
{
    /// <summary>
    /// 分销工具账户服务实现
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    public class MallAccount : BaseService, IMallAccount
    {
        /// <summary>
        /// 分销商账号登录工具
        /// </summary>
        /// <param name="account">商城后台账号</param>
        /// <param name="password">商城后台密码</param>
        /// <returns>授权信息</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result<CBDsDealerMall> HytLogin(string account, string password)
        {
            //登录成功查询分销商所有商城授权信息，并返回
            var result = new Result<CBDsDealerMall> { Status = false };
            try
            {
                var syUser = SyUserBo.Instance.GetSyUser(account);
                if (syUser != null)
                {
                    if (syUser.Status == (int)Hyt.Model.WorkflowStatus.DistributionStatus.分销商账号状态.禁用)
                    {
                        result.Message = "该账户已被禁用,请联系管理员！";
                    }
                    else if (!Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(password, syUser.Password))
                    {
                        result.Message = "密码不正确！";
                    }
                    else
                    {
                        var model = DsOrderBo.Instance.GetAuthorizationByUserID(syUser.SysNo);
                        if (model != null)
                        {
                            var info = new CBDsDealerMall();
                            Hyt.Util.Reflection.ReflectionUtils.Transform(model, info);
                            result.Data = info;
                            result.Status = true;
                        }
                        else
                        {
                            result.Message = "该商城帐号没有对应的商城账号";
                        }
                    }
                }
                else
                {
                    result.Message = "该账户未添加,请联系管理员！";
                }
                //如果入登录成功，则在分销商用户中查找
                if (!result.Status)
                {
                    var dsUser = DsUserBo.Instance.GetEntity(account);
                    if (dsUser != null)
                    {
                        if (dsUser.Status == (int)Model.WorkflowStatus.SystemStatus.系统用户状态.禁用)
                        {
                            result.Message = "该账户已被禁用,请联系管理员！";
                            return result;
                        }
                        if (!Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(password, dsUser.Password))
                        {
                            result.Message = "密码不正确！";
                            return result;
                        }
                        var dsDeal = DsDealerBo.Instance.GetDsDealerByDsUser(dsUser.SysNo);
                        if (dsDeal != null)
                        {
                            syUser = SyUserBo.Instance.GetSyUser(dsDeal.UserSysNo);
                            if (syUser != null)
                            {
                                var model = DsOrderBo.Instance.GetAuthorizationByUserID(syUser.SysNo);
                                if (model != null)
                                {
                                    var info = new CBDsDealerMall();
                                    Hyt.Util.Reflection.ReflectionUtils.Transform(model, info);
                                    info.DsUserSysNo = dsUser.SysNo;
                                    result.Data = info;
                                    result.Status = true;
                                }
                                else
                                {
                                    result.Message = "该分销商用户帐号没有对应的商城账号";
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }

        /// <summary>
        /// 第三方账号登录成功回调获取分销商所有商城授权信息
        /// </summary>
        /// <param name="mallAccount">第三方账号</param>
        /// <param name="mallType">第三方商城类型</param>
        /// <param name="authorizationCode">授权码</param>
        /// <returns>分销商商城授权信息</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result<DsDealerMall> MallLogin(string mallAccount, int mallType, string authorizationCode)
        {
            //若该第三方账户为首次授权，将授权信息写入表，若非首次授权，则将授权信息表里的授权码更新
            //获取分销商商城
            var res = GetDsDealerMallByShopAccount(mallAccount, mallType);
            if (res.Status)
            {
                var dsDealerMall = res.Data;
                dsDealerMall.AuthCode = authorizationCode;
                dsDealerMall.LastUpdateDate = DateTime.Now;
                UpdateDsAuthorization(dsDealerMall);

                #region 登录后天猫、淘宝同步授权码
                int inputType = 0;
                if (mallType == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城类型预定义.天猫商城)
                {
                    inputType = (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城类型预定义.淘宝分销;
                }
                else if (mallType == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城类型预定义.淘宝分销)
                {
                    inputType = (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城类型预定义.天猫商城;
                }
                if (inputType != 0)
                {
                    var anotherInfo = GetDsDealerMallByShopAccount(mallAccount, inputType);
                    if (anotherInfo.Status && anotherInfo.Data != null)
                    {
                        var inputInfo = anotherInfo.Data;
                        inputInfo.AuthCode = authorizationCode;
                        inputInfo.LastUpdateDate = DateTime.Now;
                        UpdateDsAuthorization(inputInfo);
                    }
                }
                #endregion
            }
            else
            {
                return new Result<DsDealerMall>() { Status = false, StatusCode = -1, Message = "您的账号尚未获得商城审批，无法登录商城升舱平台" };
            }
            return res;
        }

        /// <summary>
        /// 根据店铺账号获取分销商商城
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商商城</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public Result<DsDealerMall> GetDsDealerMallByShopAccount(string shopAccount, int mallTypeSysNo)
        {
            var result = new Result<DsDealerMall>();

            var model = DsOrderBo.Instance.GetDsDealerMallByShopAccount(shopAccount, mallTypeSysNo);
            if (model != null)
            {
                result.Status = true;
                result.Data = model;
            }
            else
            {
                result.Status = false;
            }
            return result;
        }

        /// <summary>
        /// 根据分销商系统编号获取分销商商城列表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>分销商商城列表</returns>
        /// <remarks>2013-09-13 黄志勇 创建</remarks>
        public Result<List<DsDealerMall>> GetDsAuthorizations(int dealerSysNo)
        {
            var result = new Result<List<DsDealerMall>>() { Status = false };
            var model = DsOrderBo.Instance.GetDsAuthorizations(dealerSysNo);
            if (model != null && model.Count > 0)
            {
                var list = new List<DsDealerMall>();
                foreach (var m in model)
                {
                    if (m.Status == (int)Model.WorkflowStatus.DistributionStatus.分销商商城状态.启用)
                    {
                        var info = new DsDealerMall();
                        Util.Reflection.ReflectionUtils.Transform(m, info);
                        list.Add(info);
                    }
                }
                result.Data = list;
                result.Status = true;
            }
            return result;
        }

        /// <summary>
        /// 更新分销商商城
        /// </summary>
        /// <param name="info">分销商商城</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public int UpdateDsAuthorization(DsDealerMall info)
        {
            var model = new Hyt.Model.DsDealerMall();
            Hyt.Util.Reflection.ReflectionUtils.Transform(info, model);
            return DsOrderBo.Instance.UpdateDsAuthorization(model);
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public Result<UpGradeAccount> GetAccountInfo(string shopAccount, int mallTypeSysNo)
        {
            var result = new Result<UpGradeAccount>();
            var info = new UpGradeAccount();
            var model = DsOrderBo.Instance.GetAccountInfo(shopAccount, mallTypeSysNo);
            if (model != null)
            {
                Hyt.Util.Reflection.ReflectionUtils.Transform(model, info);
                result.Status = true;
                result.Data = info;
            }
            return result;
        }

        /// <summary>
        /// 分页查询分销商预存款往来账明细
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-9-6 黄志勇 创建</remarks>
        public Result<PagedList<DsPrePaymentItem>> GetDsPrePaymentItemList(MallAccountParameters param)
        {
            var filter = new ParaDsPrePaymentItemFilter()
            {
                PageIndex = param.PageIndex <= 0 ? 1 : param.PageIndex,
                PageSize = param.PageSize,
                DealerSysNo = param.HytDealerSysNo,
                BeginDate = param.BeginDate,
                EndDate = param.EndDate
            };

            var dsItems = DsOrderBo.Instance.QueryPrePaymentItem(filter);

            var list = new List<DsPrePaymentItem>();

            foreach (var order in dsItems.Rows)
            {
                var mallOrderInfo = new DsPrePaymentItem();
                Hyt.Util.Reflection.ReflectionUtils.Transform(order, mallOrderInfo);
                list.Add(mallOrderInfo);
            }

            var result = new Result<PagedList<DsPrePaymentItem>>
            {
                Data = new PagedList<DsPrePaymentItem>
                {
                    TotalItemCount = dsItems.TotalRows,
                    CurrentPageIndex = dsItems.CurrentPage,
                    TData = list
                },
                Status = true
            };

            return result;
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public Result<UpGradeAuthorization> SetLoginInfo(string shopAccount, int mallTypeSysNo)
        {
            var account = shopAccount;
            int dsUserSysNo = 0;
            if (shopAccount.Contains("|"))
            {
                string[] arr = shopAccount.Split('|');
                if (arr.Length == 2)
                {
                    account = shopAccount.Split('|').First();
                    int.TryParse(shopAccount.Split('|').Last(), out dsUserSysNo);
                }
            }
            var model = DsOrderBo.Instance.SetLoginInfo(account, mallTypeSysNo);
            var result = new Result<UpGradeAuthorization>();
            if (model != null)
            {
                var info = new UpGradeAuthorization();
                Hyt.Util.Reflection.ReflectionUtils.Transform(model, info);
                info.DsUserSysNo = dsUserSysNo;
                result.Data = info;
                result.Status = true;
            }
            return result;
        }

        /// <summary>
        /// 获取分销商预存款账务信息
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 朱成果 创建</remarks>
        public Result<DsPrePayment> GetDsPrePayment(int dealerSysNo)
        {
            Result<DsPrePayment> result = new Result<DsPrePayment>()
            {

                Status = false
            };
            try
            {
                result.Data = DsPrePaymentToModel(Hyt.BLL.Distribution.DsPrePaymentBo.Instance.GetDsPrePayment(dealerSysNo));
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, " 获取分销商预存款账务信息 " + ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="oldPassword">旧密码（已加密）</param>
        /// <param name="newPassword">新密码（已加密）</param>
        /// <param name="dsUserSysNo">分销商用户编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-11-5 黄志勇 创建</remarks>
        public Result ModifyPassword(int dealerSysNo, string oldPassword, string newPassword, int dsUserSysNo=0)
        {
            var result = new Result() { StatusCode = -1, Status = false };
            try
            {
                if (dsUserSysNo > 0)
                {
                    var dsUser = BLL.Distribution.DsUserBo.Instance.GetEntity(dsUserSysNo);
                    if (Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(oldPassword, dsUser.Password))
                    {
                        dsUser.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt(newPassword);
                        DsUserBo.Instance.Update(dsUser);
                        result.Status = true;
                        result.StatusCode = 0;
                    }
                    else
                    {
                        result.Message = "旧密码错误!";
                    }
                   
                }
                else
                {
                    var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(dealerSysNo);
                    var syUser = SyUserBo.Instance.GetSyUser(dealer.UserSysNo);
                    if (Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(oldPassword, syUser.Password))
                    {
                        syUser.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt(newPassword);
                        SyUserBo.Instance.UpdateSyUser(syUser);
                        result.Status = true;
                        result.StatusCode = 0;
                    }
                    else
                    {
                        result.Message = "旧密码错误!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        #region 返回实体与数据库实体映射

        /// <summary>
        /// 预存款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        private DsPrePayment DsPrePaymentToModel(Model.DsPrePayment entity)
        {
            if (entity == null) return null;
            DsPrePayment m = new DsPrePayment()
                {
                    SysNo = entity.SysNo,
                    AvailableAmount = entity.AvailableAmount,
                    DealerSysNo = entity.DealerSysNo,
                    FrozenAmount = entity.FrozenAmount,
                    TotalPrestoreAmount = entity.TotalPrestoreAmount,
                    CreatedBy = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate,
                    LastUpdateBy = entity.LastUpdateBy,
                    LastUpdateDate = entity.LastUpdateDate,
                    AlertAmount = entity.AlertAmount
                };
            return m;
        }
        #endregion

        /// <summary>
        /// 更新分销商预存款余额提醒额度
        /// </summary>
        /// <param name="alertAmount">余额提示额</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>t:设置成功 f:失败</returns>
        /// <remarks>2014-03-21 朱家宏 创建</remarks>
        public Result SaveAlertSetting(decimal alertAmount, int dealerSysNo)
        {
            var result = new Result() { StatusCode = -1, Status = false };
            try
            {
                //var model = DataAccess.Distribution.IDsPrePaymentDao.Instance.GetEntityByDealerSysNo(dealerSysNo);
                //if (model == null)
                //{
                //    result.Status = false;
                //}
                //else
                //{
                //model.AlertAmount = alertAmount;
                //result.Status = DataAccess.Distribution.IDsPrePaymentDao.Instance.Update(model) > 0;
                result.Status = DataAccess.Distribution.IDsPrePaymentDao.Instance.UpdateAlertAmount(dealerSysNo, alertAmount);
                if (result.Status == true) result.StatusCode = 0;
                //}
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取商城类型详情
        /// </summary>
        /// <param name="sysNo">商城类型编号</param>
        /// <returns>获取商城类型详情</returns>
        /// <remarks>2014-06-11 朱成果 创建</remarks>
       public Result<DsMallType> GetDsMallType(int sysNo)
        {
            Result<DsMallType> res = new Result<DsMallType>() { Status = false };
            try
            {
                res.Data = DsMallTypeBo.Instance.GetDsMallType(sysNo);
                if (res.Data != null)
                {
                    res.Status = true;
                }
                else
                {
                    res.Message = "商城类型不存在";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
            }
            return res;
        }

       /// <summary>
       /// 获取分销商商城信息(授权码已加密)
       /// </summary>
       /// <param name="sysNo">商城编号</param>
       /// <returns>分销商商城信息</returns>
       /// <remarks>2014-06-11 朱成果 创建</remarks>
      public   Result<DsDealerMall> GetDsDealerMall(int sysNo)
       {
           Result<DsDealerMall> res = new Result<DsDealerMall>() { Status = false };
           try
           {
               res.Data = DsDealerMallBo.Instance.GetEntity(sysNo);
               if (res.Data != null)
               {
                   res.Data.AuthCode = Hyt.Util.EncryptionUtil.EncryptDES(res.Data.AuthCode, Hyt.Util.EncryptionUtil.EncryptKey);//加密授权码
                   res.Status = true;
               }
               else
               {
                   res.Message = "分销商商城信息不存在";
               }
           }
           catch (Exception ex)
           {
               res.Message = ex.Message;
               res.Status = false;
           }
           return res;
       }
    }
}
