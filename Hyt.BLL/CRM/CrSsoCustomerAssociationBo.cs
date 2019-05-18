using System;
using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Pisen.Framework.Service.Proxy;
using Pisen.Service.Share.SSO.Contract;
using Pisen.Service.Share.SSO.Contract.DataContract;

namespace Hyt.BLL.CRM
{
    /// <summary>
    ///SSO客户关系业务逻辑
    /// </summary>
    /// <remarks>2014－06-26 余勇 创建</remarks>
    public class CrSsoCustomerAssociationBo : BOBase<CrSsoCustomerAssociationBo>
    {
        /// <summary>
        /// 插入SSO客户关系表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public int Insert(CrSsoCustomerAssociation model)
        {
            return ICrSsoCustomerAssociationDao.Instance.Insert(model);
        }

        /// <summary>
        /// 通过会员编号获得客户关系实体
        /// </summary>
        /// <param name="costomerSysNo">会员编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public CrSsoCustomerAssociation GetByCustomerSysNo(int costomerSysNo)
        {
            return ICrSsoCustomerAssociationDao.Instance.GetByCustomerSysNo(costomerSysNo);
        }

        /// <summary>
        /// 通过ssoID获得客户关系实体
        /// </summary>
        /// <param name="ssoID">ssoID</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public CrSsoCustomerAssociation GetBySsoID(int ssoID)
        {
            return ICrSsoCustomerAssociationDao.Instance.GetBySsoId(ssoID);
        }

        /// <summary>
        /// 调用wcf服务获取SSO客户信息
        /// </summary>
        /// <param name="customer">客户信息</param>
        /// <returns>返回ssoID</returns>
        /// <remarks>
        /// 2014－06-26 余勇 创建
        /// </remarks>
        public int InsertCrSsoCustomerAssociation(CrCustomer customer)
        {
            //var ssoId = 0;
            var emailStatus = customer.EmailStatus == (int) Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.已验证 ? 1 : 0;
            var phoneStatus = customer.MobilePhoneStatus == (int) Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.已验证 ? 1 : 0;
            var request = new CustomerRegisterReturnIdRequest
            {
                Account=customer.Account,
                EmailAddress = customer.EmailAddress,
                IsValidateEmail = emailStatus,
                IsValidateMobilePhone = phoneStatus,
                MobilePhoneNumber = customer.MobilePhoneNumber,
                Password = customer.Password,
                RegisterDate = customer.RegisterDate,
                Status = customer.Status
            };
            try
            {
                //调用wcf服务通过客户账号获取SSO客户信息
                using (var client = new ServiceProxy<ICustomerService>())
                {
                    var r = client.Channel.CustomerRegisterReturnId(request);
                    if (!r.IsError)
                    {
                        //ssoId = r.Data;
                        return r.Id;
                    }
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建新会员调用wcf服务失败:"+r.ErrMsg, LogStatus.系统日志目标类型.客户管理, customer.SysNo, null);
                    return -1;
                }
           

            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建新会员调用wcf服务错误", LogStatus.系统日志目标类型.客户管理, customer.SysNo, ex);
                throw ex;
            }
        }
    }
}
