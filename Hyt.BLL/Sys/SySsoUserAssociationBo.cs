using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Grand.Framework.Service.Proxy;
using Grand.Service.Share.SSO.Contract;
using Grand.Service.Share.SSO.Contract.DataContract;
using Hyt.BLL.Log;

namespace Hyt.BLL.Sys
{
    public class SySsoUserAssociationBo : BOBase<SySsoUserAssociationBo>
    {
        /// <summary>
        /// 插入SSO系统用户关联表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－10-14 谭显锋 创建 创建</remarks>
        public int Insert(SySsoUserAssociation model)
        {
            return ISySsoUserAssociationDao.Instance.Insert(model);
        }
        /// <summary>
        /// 通过系统用户编号获得SSO系统用户关联实体的ssoId
        /// </summary>
        /// <param name="costomerSysNo">系统用户编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public int GetSsoIdByUserSysNo(int userSysNo)
        {
            var model = ISySsoUserAssociationDao.Instance.GetByUserSysNo(userSysNo);
            if (model == null)          
                return -1;            
            return model.SsoId;
        }
        /// <summary>
        /// 通过系统用户编号获得SSO系统用户关联实体
        /// </summary>
        /// <param name="costomerSysNo">系统用户编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public SySsoUserAssociation GetByUserSysNo(int userSysNo)
        {
            return ISySsoUserAssociationDao.Instance.GetByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 通过ssoID获得SSO系统用户关联实体
        /// </summary>
        /// <param name="ssoID">ssoID</param>
        /// <returns>SSO系统用户关联o</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public int GetSyNoBySsoID(int ssoID)
        {
            var ssoInfo=ISySsoUserAssociationDao.Instance.GetBySsoId(ssoID);
            if (ssoInfo == null)
                return -1;
            return ssoInfo.UserSysNo;
        }

        /// <summary>
        /// 通过ssoID获得SSO系统用户关联实体表中的UserId
        /// </summary>
        /// <param name="ssoID">ssoID</param>
        /// <returns>SSO系统用户关联表对应的UserId</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public int GetUserIdBySsoID(int ssoID)
        {
            var model = ISySsoUserAssociationDao.Instance.GetBySsoId(ssoID);
            return model.UserSysNo;
        }

        /// <summary>
        /// 根据ssoUserId获取SSO用户信息
        /// </summary>
        /// <param name="userId">ssoUserId</param>
        /// <returns>返回SSo用户实体</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public GetEnterpriseUserResponse GetSsoUserInfoBySsoUserId(int userId)
        {
            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                var result = client.Channel.GetEnterpriseUserByEnterpriseUserId(userId);

                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                return result;
            }

        }
        /// <summary>
        /// 根据查询参数取得企业用户分页列表
        /// </summary>
        /// <param name="request">查询参数对象</param>
        /// <returns>分页列表对象</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public IEnumerable<EnterpriseUser> GetEnterpriseUserList(EnterpriseUserPageListRequest request,out int total)
        {
            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                var result = client.Channel.GetEnterpriseUserPageList(request);
                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                total = result.TotalCount;
                return result.EnterpriseUserList;
            }
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="filter">查询参数对象，包括key关键字(企业名称、联系人、电话),pageSize,pageIndex</param>
        /// <returns>企业列表</returns>
        /// <remarks>2014－10-15 谭显锋 创建</remarks>
        public IEnumerable<Enterprise> GetEnterprisePageList(ParaEnterpriseUserFilter filter)
        {
            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                var result = client.Channel.GetEnterprisePageList(filter.Key, filter.PageIndex, filter.PageSize);
                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                return result.EnterpriseList;
            }
        }

        /// <summary>
        /// 获取所有企业用户分页列表
        /// </summary>
        /// <param name="filter">筛选参数</param>
        /// <returns>分页列表</returns>
        /// <remarks>2014-10-17 谭显锋 创建</remarks>
        public Pager<CBEnterpriseUser> GetAllEnterpriseUserPager(ParaEnterpriseUserFilter filter)
        {
            EnterpriseUserPageListRequest request = new EnterpriseUserPageListRequest();
            request.EnterpriseNO = 0;
            request.PageIndex = filter.Id==0?1:filter.Id;
            request.PageSize = filter.PageSize;
            request.Key = filter.Key==null?null:filter.Key.Trim();
            int total;
            var enteriseUserList = GetEnterpriseUserList(request,out total);
            filter.Key = null;
            var enteriseList = GetEnterprisePageList(filter);
            var model = from u in enteriseUserList
                        join e in enteriseList on u.EnterpriseNO equals e.EnterpriseNO
                        select new CBEnterpriseUser
                {
                    ID = u.Id,
                    EnterpriseNO = u.EnterpriseNO,
                    CompanyName = e.CompanyName,
                    Account = u.Account,
                    UserName = u.UserName,
                    EmailAddress = u.EmailAddress,
                    MobilePhoneNumber = u.MobilePhoneNumber,
                    RegisterDate = u.RegisterDate,
                    Status = u.Status
                };
            var pager = new Pager<CBEnterpriseUser>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id==0?1:filter.Id,
                TotalRows = total,
                Rows = model.ToList(),
            };
            return pager;
        }

        /// <summary>
        /// 修改企业用户
        /// </summary>
        /// <param name="enterpriseUserEditRequest">企业用户实体</param>
        /// <returns>Result</returns>
        /// <remarks>2014-10-24 谭显锋 创建</remarks>
        public Grand.Framework.Service.Proxy.Result EnterpriseUserEdit(SyUser syUser)
        {
            int ssoid = GetSsoIdByUserSysNo(syUser.SysNo);

            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                GetEnterpriseUserResponse getEnterpriseUser = client.Channel.GetEnterpriseUserByEnterpriseUserId(ssoid);
                getEnterpriseUser.Account = syUser.Account;
                getEnterpriseUser.EmailAddress = syUser.EmailAddress;
                getEnterpriseUser.MobilePhoneNumber = syUser.MobilePhoneNumber;
                getEnterpriseUser.Status = syUser.Status;
                getEnterpriseUser.UserName = syUser.UserName;
                EnterpriseUserEditRequest enterpriseUserEditRequest = new EnterpriseUserEditRequest();
                Hyt.Util.Reflection.ReflectionUtils.Transform(getEnterpriseUser, enterpriseUserEditRequest);
                var result = client.Channel.EnterpriseUserEdit(enterpriseUserEditRequest);
                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                return result;
            }
        }

        /// <summary>
        /// 修改企业用户密码
        /// </summary>
        /// <param name="enterpriseNo">企业编号</param>
        /// <param name="enterpriseUserId">企业用户Id</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改成功返回true,否则返回false</returns>
        /// <remarks>2014-10-27 谭显锋 创建</remarks>
        public bool EnterpriseUserEditPassword(int enterpriseNo, int enterpriseUserId, string oldPassword, string newPassword)
        {

            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                var result = client.Channel.EnterpriseUserEditPassword(enterpriseNo, enterpriseUserId, oldPassword, newPassword);
                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                return result.IsEnterpriseUserEditPassword;
            }
        }

        /// <summary>
        /// 重置企业用户密码
        /// </summary>
        /// <param name="enterpriseNo">企业编号</param>
        /// <param name="phone">手机</param>
        /// <param name="password">新密码</param>
        /// <returns>是否重置成功</returns>
        /// <remarks>2014-10-28 谭显锋 创建</remarks>
        public bool EnterpriseUserResetPassword(int enterpriseNo, string account, string phone, string password)
        {
            using (var client = new ServiceProxy<IEnterpriseUserService>())
            {
                var result = client.Channel.EnterpriseUserResetPassword(enterpriseNo, account, phone, password);
                if (result.IsError)
                {
                    throw new HytException(result.ErrMsg);
                }
                return result.IsResetPasswordSuccess;
            }
        }
    }
}
