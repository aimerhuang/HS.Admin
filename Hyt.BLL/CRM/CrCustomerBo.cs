using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.BLL.Web;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Pager;
using Hyt.DataAccess.LevelPoint;
using Hyt.Model.WorkflowStatus;
using Pisen.Framework.Service.Proxy;
using Pisen.Service.Share.SSO.Contract;
using Pisen.Service.Share.SSO.Contract.DataContract;
using Hyt.Model.SellBusiness;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Util;
using System.IO;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 会员业务逻辑
    /// </summary>
    /// <remarks>2013－06-25 朱成果 创建</remarks>
    public class CrCustomerBo : BOBase<CrCustomerBo>
    {

        /// <summary>
        ///  不注册会员直接购买,模拟一个会员
        /// </summary>
        /// <param name="shopid">门店</param>
        /// <param name="userid">用户编号</param>
        /// <param name="isnew">是否必须新生成一个唯一账号</param>
        /// <returns></returns>
        /// <remarks>2016－05-25 杨浩 创建</remarks>
        public CrCustomer AutoNoMobileShopCustomer(int shopid, int userid, bool isnew = true)
        {
            string defaultAccount = "d" + userid.ToString().PadLeft(10, '0');
            CrCustomer customer = new CrCustomer();
            customer.Account = defaultAccount;
            if (isnew == true)
            {
                customer.Account = "d" + DateTime.Now.Ticks.ToString(); //生成唯一编码，尽量避免重复
            }
            customer.NickName = "非会员顾客";
            customer.Name = "非会员顾客";
            customer.EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证;
            customer.MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证;
            customer.RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.门店;
            customer.RegisterSourceSysNo = shopid.ToString();
            customer.IsReceiveShortMessage = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.否;
            customer.IsReceiveEmail = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收邮件.否;
            customer.IsPublicAccount = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否是公共账户.否;
            customer.RegisterDate = DateTime.Now;
            customer.LevelSysNo = CustomerLevel.初级;
            customer.Password = WebUtil.GeneratePwd(1, 5);
            customer.Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.无效;
            customer.CreatedBy = userid;
            customer.CreatedDate = DateTime.Now;
            customer.IsExperienceCoinFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.惠源币是否固定.固定;
            customer.IsExperiencePointFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.经验积分是否固定.固定;
            customer.IsLevelFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.等级是否固定.固定;
            customer.PSysNo = 0;
            var oldCustomer = BLL.Web.CrCustomerBo.Instance.GetCustomerByCellphone(customer.Account);//当前账号不存在

            if (oldCustomer == null)
            {
                Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(customer, null);//创建新账号
            }
            else if (isnew == false)//不必新建，用原始账号
            {
                customer = oldCustomer;
            }
            else
            {
                throw new HytException("请求未响应，请重试！");
            }
            return customer;
        }
        /// <summary>
        /// 更新客户可提返点
        /// </summary>
        /// <param name="brokerage">可提返点,可为负数</param>
        /// <param name="sysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public  int UpdateCustomerBrokerage(decimal brokerage, int sysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.UpdateCustomerBrokerage(brokerage,sysNo);
        }

        /// <summary>
        /// 根据手机搜索会员列表
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－08-06 沈强 创建</remarks>
        public IList<CrCustomer> SearchCustomerByMobile(string mobile)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.SearchCustomerByMobile(mobile);
        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public Model.CrCustomer GetCrCustomerItem(int sysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(sysNo);
        }

        /// <summary>
        /// 判断分销商是否包含该会员
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        public CrCustomer GetCustomerBySysNoDearler(int SysNo, int DealerSysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCustomerBySysNoDearler(SysNo, DealerSysNo);
        }

        /// <summary>
        /// 根据帐号搜索会员列表
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="rownum">返回条数</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－06-26 朱成果 创建</remarks>
        /// <remarks>2013－07-25 黄志勇 修改</remarks>
        public IList<Model.CrCustomer> SearchCustomerByAccount(string account, int rownum = 1)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.SearchCustomerByAccount(account, rownum);
        }

        /// <summary>
        /// 根据会员id查询默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员id</param>
        /// <returns>默认收货地址</returns>
        /// <remarks>2013-07-1 黄志勇  创建</remarks>
        public CrReceiveAddress SearchReceiveAddressByCustomerSysNo(int customerSysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.SearchReceiveAddressByCustomerSysNo(customerSysNo);
        }

        /// <summary>
        /// 根据会员等级ID获取等级信息
        /// </summary>
        /// <param name="sysNo">会员等级ID</param>
        /// <returns>等级信息</returns>
        /// <remarks>2013－07-01 黄志勇 创建</remarks>
        public Model.CrCustomerLevel SearchCustomerLevel(int sysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCustomerLevel(sysNo);
        }

        /// <summary>
        /// 根据用户账号获取前台用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>返回前台用户</returns>
        /// <remarks>2013-07-09 杨浩 创建</remarks>
        public CBCrCustomer GetCrCustomer(string account)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomer(account);
        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CBCrCustomer GetModel(int sysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据条件获取会员列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="status">会员状态</param>
        /// <param name="levelSysNo">级别</param>
        /// <param name="emailStatus">邮箱状态</param>
        /// <param name="mobilePhoneStatus">手机状态</param>
        /// <param name="isReceiveEmail">是否接收邮</param>
        /// <param name="isReceiveShortMessage">是否接收短信</param>
        /// <param name="isPublicAccount">是否是公共账户</param>
        /// <param name="isLevelFixed">等级是否固定</param>
        /// <param name="isExperiencePointFixed">经验积分是否固定</param>
        /// <param name="isExperienceCoinFixed">惠源币是否固定</param>
        /// <param name="account">会员手机号</param>
        /// <param name="filte">会员经销商 王耀发 2016-2-17 创建</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public PagedList<CBCrCustomer> Seach(int pageIndex, int? status, int? levelSysNo, int? emailStatus, int? mobilePhoneStatus, int? isReceiveEmail, int? isReceiveShortMessage, int? isPublicAccount, int? isLevelFixed, int? isExperiencePointFixed, int? isExperienceCoinFixed, string account = null, ParaIsDealerFilter filte = null, int SellBusinessGradeId = -1)
        {
            #region 赋初值
            if (status == null)
            {
                status = -1;
            }
            if (levelSysNo == null)
            {
                levelSysNo = -1;
            }
            if (emailStatus == null)
            {
                emailStatus = -1;
            }
            if (mobilePhoneStatus == null)
            {
                mobilePhoneStatus = -1;
            }

            if (isReceiveEmail == null)
            {
                isReceiveEmail = -1;
            }
            if (isReceiveShortMessage == null)
            {
                isReceiveShortMessage = -1;
            }
            if (isPublicAccount == null)
            {
                isPublicAccount = -1;
            }
            if (isLevelFixed == null)
            {
                isLevelFixed = -1;
            }
            if (isExperiencePointFixed == null)
            {
                isExperiencePointFixed = -1;
            }
            if (isExperienceCoinFixed == null)
            {
                isExperienceCoinFixed = -1;
            }
            #endregion

            var list = new PagedList<CBCrCustomer>();
            var pager = new Pager<CBCrCustomer>();

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBCrCustomer
            {
                Status = (int)status,
                LevelSysNo = (int)levelSysNo,
                EmailStatus = (int)emailStatus,
                MobilePhoneStatus = (int)mobilePhoneStatus,
                IsReceiveEmail = (int)isReceiveEmail,
                IsReceiveShortMessage = (int)isReceiveShortMessage,
                IsPublicAccount = (int)isPublicAccount,
                IsLevelFixed = (int)isLevelFixed,
                IsExperiencePointFixed = (int)isExperiencePointFixed,
                IsExperienceCoinFixed = (int)isExperienceCoinFixed,
                Account = account,
                IsBindDealer = filte.IsBindAllDealer,
                IsBindAllDealer = filte.IsBindAllDealer,
                DealerCreatedBy = filte.DealerCreatedBy,
                DealerSysNo = filte.DealerSysNo,
                SelectedAgentSysNo = filte.SelectedAgentSysNo,
                SelectedDealerSysNo = filte.SelectedDealerSysNo,
                SellBusinessGradeId = SellBusinessGradeId
            };
            pager.PageSize = list.PageSize;
            pager = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Seach(pager);

            return pager.Map();
        }

        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public int Update(Model.CrCustomer model)
        {
            int result = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Update(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改会员{0}", model.Account), LogStatus.系统日志目标类型.客户管理,
                    Authentication.AdminAuthenticationBo.Instance.Current == null ? 0 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改会员{0}失败", model.Account), LogStatus.系统日志目标类型.客户管理,
                    Authentication.AdminAuthenticationBo.Instance.Current == null ? 0 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 调用SSO服务修改客户密码
        /// </summary>
        /// <param name="customSysNo">客户编号</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <remarks>2014－06-27 余勇 创建</remarks>
        public bool UpdateSSOPassword(int customSysNo, string oldPwd, string newPwd)
        {
            return true;
            //var r = new ResetPasswordReponse();
            //var ssoAss = CrSsoCustomerAssociationBo.Instance.GetByCustomerSysNo(customSysNo);
            //var ssoId = ssoAss != null ? ssoAss.SsoId : 0;
            //if (ssoId > 0)
            //{
            //    try
            //    {
            //        //调用SSO服务修改客户密码
            //        using (var client = new ServiceProxy<ICustomerService>())
            //        {
            //            //调用新添加接口重置密码 余勇 2014-07-15
            //            r = client.Channel.ResetPassword(ssoId, newPwd);
            //        }
            //        if (r.IsError)
            //        {
            //            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台,
            //                "调用SSO服务修改客户密码未成功，失败原因：" + r.ErrMsg,
            //                LogStatus.系统日志目标类型.客户管理, customSysNo, null);
            //            throw new NotImplementedException(r.ErrMsg);
            //        }
            //        return r.IsResetPasswordSucceed;
            //    }
            //    catch (Exception ex)
            //    {
            //        SysLog.Instance.Error(LogStatus.系统日志来源.后台, "调用SSO服务修改客户密码错误", LogStatus.系统日志目标类型.客户管理, customSysNo,
            //            ex);
            //        throw ex;
            //    }
            //}
            //else
            //{
            //    SysLog.Instance.Error(LogStatus.系统日志来源.后台, "客户关系表CrSsoCustomerAssociation中无该会员记录", LogStatus.系统日志目标类型.客户管理, customSysNo,
            //         null);
            //    throw new NotImplementedException("客户关系表中无该会员记录:" + customSysNo);
            //}
            return false;
        }

        /// <summary>
        /// 获取 SSO帐号登录成功之后
        /// </summary>
        /// <param name="account">帐号.</param>
        /// <param name="password">密码（明文).</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014－7-11 杨文兵 创建
        /// </remarks>
        public CrCustomer SSOGetCustomerByLogin(string account, string password)
        {

            int ssoID = -1;
            using (var client = new ServiceProxy<ICustomerService>())
            {
                ssoID = client.Channel.GetCustomerIdByAccount(account, password).Id;
            }
            if (ssoID < 1) return null;

            var r = Hyt.BLL.CRM.CrSsoCustomerAssociationBo.Instance.GetBySsoID(ssoID);

            if (r == null) return null;


            return Hyt.BLL.Web.CrCustomerBo.Instance.GetModel(r.CustomerSysNo);

        }

        /// <summary>
        /// 获取SSO用户customerID
        /// </summary>
        /// <param name="account">帐号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014－7-11 杨文兵 创建
        /// </remarks>
        public int SSOGetCustomerIDByAccount(string account)
        {
            try
            {
                using (var client = new ServiceProxy<ICustomerService>())
                {
                    return client.Channel.GetCustomerByAccount(account).Id;
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "调用 获取SSO用户customerID", LogStatus.系统日志目标类型.客户管理, 0, ex);
                return -1;
            }
        }

        /// <summary>
        /// 获取SSO用户旧密码
        /// </summary>
        /// <param name="account">帐号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014－7-11 杨文兵 创建
        /// </remarks>
        public string SSOGetOldPasswordByAccount(string account)
        {
            var r = new GetCustomerByAccountResponse();
            try
            {
                using (var client = new ServiceProxy<ICustomerService>())
                {
                    return client.Channel.GetCustomerByAccount(account).Password;
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "调用 获取SSO用户旧密码", LogStatus.系统日志目标类型.客户管理, 0, ex);
                return string.Empty;
            }
        }



        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="customer">会员实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-09 苟治国 创建</remarks>
        public int CreateCustomer(CrCustomer customer)
        {

            bool result = Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(customer, null);
            if (result)
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("创建会员{0}", customer.Account), LogStatus.系统日志目标类型.客户管理, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return customer.SysNo;
            }
            else
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("创建会员{0}失败", customer.Account), LogStatus.系统日志目标类型.客户管理, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return 0;
            }
        }

        /// <summary>
        /// 注册前台会员
        /// 初始化相关数据
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码(未加密的)</param>
        /// <param name="phoneStatus">电话验证状态</param>
        /// <param name="mailStatus">邮箱验证状态</param>
        /// <param name="source">注册来源</param>
        /// <returns>返回成功Or失败</returns>
        /// <remarks>2013－10-29 黄波 创建
        /// 2016-03-15 杨云奕 修改 添加会员字段基础信息
        /// </remarks>
        public int RegisterFrontCustomer(
            string account
            , string password
            , Hyt.Model.WorkflowStatus.CustomerStatus.手机状态 phoneStatus = Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.已验证
            , Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态 mailStatus = Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证
            , Hyt.Model.WorkflowStatus.CustomerStatus.注册来源 source = Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.PC网站
        )
        {
            var customer = new CrCustomer
            {
                Account = account,
                Password = password, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(password),  余勇修改 2014-09-12
                MobilePhoneNumber = account,
                MobilePhoneStatus = (int)phoneStatus,
                EmailStatus = (int)mailStatus,
                RegisterSource = ((int)source),
                LevelSysNo = 1,
                AvailablePoint = 0,
                ExperienceCoin = 0,
                ExperiencePoint = 0,
                IsExperienceCoinFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.惠源币是否固定.不固定,
                IsExperiencePointFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.经验积分是否固定.不固定,
                IsLevelFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.等级是否固定.不固定,
                IsReceiveEmail = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收邮件.是,
                IsReceiveShortMessage = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.是,
                Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                LevelPoint = 0,
                RegisterDate = DateTime.Now,
                Birthday = new DateTime(1880, 1, 1),
                CreatedDate = new DateTime(1880, 1, 1),
                LastLoginDate = new DateTime(1880, 1, 1),
                Subscribe = "0",
                IsSellBusiness = 0
            };


            bool result = Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(customer, null);
            if (result)
            {
                try
                {
                    //计算用户等级
                    IPointDao.Instance.UpdateCustomerLevel(customer.SysNo);
                }
                catch
                { }
            }
            return result ? 1 : 0;
        }
        /// <summary>
        /// 查询所有的用户
        /// </summary>     
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2015-09-19 王耀发 创建
        public IList<CrCustomer> GetCrCustomerList()
        {
            return ICrCustomerDao.Instance.GetCrCustomerList();
        }

        /// <summary>
        /// 执行分销商返利
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <returns></returns>
        /// <remarks>2015－09-11 杨浩 创建  2015-10-28 王耀发引用</remarks>
        public void ExecuteSellBusinessRebates(SoOrder order)
        {
            CrCustomer customer = GetModel(order.OrderCreatorSysNo);
            if (customer != null)
            {
                int indirect2Id = 0;
                int indirect1Id = 0;
                int recommendId = customer.PSysNo;

                string[] ids = customer.CustomerSysNos.Trim(',').Split(',');
                int length = ids.Length;

                if (length >= 3)
                    indirect1Id = int.Parse(ids[length - 3]);

                if (length >= 4)
                    indirect2Id = int.Parse(ids[length - 4]);

                CrSellBusinessRebatesResult sellBusinessRebatesResult = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.ExecuteSellBusinessRebates(customer.PSysNo, order.OrderCreatorSysNo, indirect2Id, indirect1Id, order.SysNo, "2", order.ProductAmount);
            }
        }
        /// <summary>
        /// 更新会员对应 Brokerage，可提佣金 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public void UpdateCustomerValue(int SysNo, decimal Value)
        {
            ICrCustomerDao.Instance.UpdateCustomerValue(SysNo, Value);
        }

        /// <summary>
        /// 更新会员对应 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public void UpdateCustomerValueConfirm(int SysNo, decimal Value)
        {
            ICrCustomerDao.Instance.UpdateCustomerValueConfirm(SysNo, Value);
        }
        /// <summary>
        /// 获取分销商对应的会员
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        /// <remarks>2016-4-29 刘伟豪 添加关键字搜索</remarks>
        public IList<CrCustomer> GetCrCustomerListByDealerSyNo(int DealerSysNo, string keyword = "")
        {
            return ICrCustomerDao.Instance.GetCrCustomerListByDealerSyNo(DealerSysNo, keyword);
        }

        /// <summary>
        /// 获取分销商对应的会员
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public List<CrCustomer> GetCrCustomerListByDealerSyNoQuery(int DealerSysNo)
        {
            return ICrCustomerDao.Instance.GetCrCustomerListByDealerSyNoQuery(DealerSysNo);
        }
        /// <summary>
        /// 获取分销商对应的会员
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public List<CrCustomer> QueryCrCustomer(int DealerSysNo, string Account, bool getParent = false)
        {
            var alllst = GetCrCustomerListByDealerSyNoQuery(DealerSysNo);
            List<CrCustomer> lst = new List<CrCustomer>();
            var searchlst = alllst.Where(m => string.IsNullOrEmpty(m.Account) == false && m.Account.IndexOf(Account) > -1).ToList();
            if (searchlst != null)
            {
                searchlst.ForEach((item) =>
                {
                    if (!getParent)//向下级联
                    {
                        lst.Add(item);
                        lst.AddRange(GetDownList(item.SysNo, alllst));
                    }
                    else//向上级联
                    {
                        while (item != null)
                        {
                            lst.Add(item);
                            item = GetArea(item.PSysNo);
                        }
                    }
                });
            }
            return lst;
        }

        /// <summary>
        /// 获取向下级联数据
        /// </summary>
        /// <param name="parentID">父亲编号</param>
        /// <param name="allLst">所有会员信息</param>
        /// <returns>向下级联的会员信息</returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        private List<CrCustomer> GetDownList(int parentID, List<CrCustomer> allLst)
        {
            List<CrCustomer> lst = new List<CrCustomer>();
            var sublist = allLst.Where(m => m.Status == 1 && m.PSysNo == parentID).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                lst.AddRange(sublist);
                foreach (var item in sublist)
                    lst.AddRange(GetDownList(item.SysNo, allLst));
            }
            return lst;
        }
        /// <summary>
        /// 获取向上级联数据
        /// </summary>
        /// <param name="parentID">父亲编号</param>
        /// <param name="allLst">所有会员信息</param>
        /// <returns>向上级联级联的会员信息</returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public CrCustomer GetArea(int sysNo)
        {
            return ICrCustomerDao.Instance.GetModel(sysNo);
        }
        /// <summary>
        /// 查询用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2016－04-16 王耀发 创建</remarks>
        public IList<CrReceiveAddress> LoadCustomerAddress(int customerSysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.LoadCustomerAddress(customerSysNo);
        }

        public List<CBCrCustomer> SeachCanBeParentList(int id, string keyword = "")
        {
            return ICrCustomerDao.Instance.SeachCanBeParentList(id, keyword);
        }

        public bool CustomerToParent(int cSysNo, int pSysNo)
        {
            return ICrCustomerDao.Instance.CustomerToParent(cSysNo, pSysNo);
        }

        public bool UpdateInviteAndIndirectNum(int customerSysNo)
        {
            return ICrCustomerDao.Instance.UpdateInviteAndIndirectNum(customerSysNo);
        }

        public void UpdateIsSellBusiness(int SysNo, string IsSellBusiness)
        {
            ICrCustomerDao.Instance.UpdateIsSellBusiness(SysNo, IsSellBusiness);
        }

        public bool UpdateSellBusinessGrade(int id, int gid)
        {
            return ICrCustomerDao.Instance.UpdateSellBusinessGrade(id, gid);
        }

        #region 会员导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2016-07-2 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {               
                {"Name", "姓名"},
                {"MobilePhoneNumber", "手机"}
            };
                /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Hyt.Model.Result ImportExcel(Stream stream, int operatorSysno)
        {
            System.Data.DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Hyt.Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Hyt.Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            var excellst = new List<CrCustomer>();
            var lstToInsert = new List<CrCustomer>();
            var lstToUpdate = new List<CrCustomer>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;
                for (var j = 0; j < 2; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        return new Hyt.Model.Result
                            {
                                Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1)),
                                Status = false
                            };
                    }
                }
                //姓名
                var Name = dt.Rows[i][DicColsMapping["Name"]].ToString().Trim();
                //手机
                var MobilePhoneNumber = dt.Rows[i][DicColsMapping["MobilePhoneNumber"]].ToString();

                var model = new CrCustomer
                {
                    Account = MobilePhoneNumber,
                    Name = Name,
                    NickName = Name,
                    Password = null,
                    MobilePhoneNumber = MobilePhoneNumber,
                    MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                    EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                    RegisterSource = ((int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.门店),
                    LevelSysNo = 1,
                    AvailablePoint = 0,
                    ExperienceCoin = 0,
                    ExperiencePoint = 0,
                    IsExperienceCoinFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.惠源币是否固定.不固定,
                    IsExperiencePointFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.经验积分是否固定.不固定,
                    IsLevelFixed = (int)Hyt.Model.WorkflowStatus.CustomerStatus.等级是否固定.不固定,
                    IsReceiveEmail = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收邮件.是,
                    IsReceiveShortMessage = (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.是,
                    Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                    LevelPoint = 0,
                    RegisterDate = DateTime.Now,
                    Birthday = new DateTime(1880, 1, 1),
                    CreatedDate = new DateTime(1880, 1, 1),
                    LastLoginDate = new DateTime(1880, 1, 1),
                    Subscribe = "0",
                    IsSellBusiness = 0
                };
                excellst.Add(model);                


            }
            var lstExisted = ICrCustomerDao.Instance.GetAllCustomer();

            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.Account == excelModel.Account))
                {
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                ICrCustomerDao.Instance.CreateCrCustomer(lstToInsert);
                //ICrCustomerDao.Instance.UpdateExcelCrCustomer(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入会员信息",
                                         LogStatus.系统日志目标类型.导入会员, 0, ex, null, operatorSysno);
                return new Hyt.Model.Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                return new Hyt.Model.Result
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Hyt.Model.Result
            {
                Message = msg,
                Status = true
            };
        }

        /// <summary>
        /// 导入POS机会员
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2016-08-18 罗远康 创建</remarks>
        public Hyt.Model.Result ImportPosCustomer(List<CrCustomer> CustomerList, int operatorSysno)
        {
            var lstToInsert = new List<CrCustomer>();
            var lstUpInsert = new List<CrCustomer>();
            var lstExisted = ICrCustomerDao.Instance.GetAllCustomer();//获取所以会员信息

            if (CustomerList.Count == 0)
            {
                return new Hyt.Model.Result
                {
                    Message = string.Format("POS机获取的会员信息为空！"),
                    Status = false
                };
            }
            foreach (var excelModel in CustomerList)
            {
                if (lstExisted.Any(e => e.Account == excelModel.Account))
                {
                    lstUpInsert.Add(excelModel);//已有相同的数据
                }
                else 
                {
                    lstToInsert.Add(excelModel);//没有相同数据
                }
            }
            if (lstToInsert.Count == 0)
            {
                return new Hyt.Model.Result
                {
                    Message = "POS机同步数据为空!",
                    Status = false
                };
            }
            try
            {
                ICrCustomerDao.Instance.CreateCrCustomer(lstToInsert);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "同步POS机会员信息",
                                         LogStatus.系统日志目标类型.导入会员, 0, ex, null, operatorSysno);
                return new Hyt.Model.Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功同步POS机{0}条数据!", lstToInsert.Count) : "";
            return new Hyt.Model.Result
            {
                Message = msg,
                Status = true
            };
        }
        #endregion

        /// <summary>
        /// 更新用户冻结佣金
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public  bool UpdateCustomerBrokerageFreeze(int customerSysNo, decimal amount)
        {
            return ICrCustomerDao.Instance.UpdateCustomerBrokerageFreeze(customerSysNo, amount);
         
        }
    }
}