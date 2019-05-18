using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Extra.Erp;
using Extra.Erp.Model.Sale;
using Hyt.Admin.Models;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.Infrastructure.Memory;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Serialization;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Hyt.Infrastructure.Caching;
using System.Data;
using Hyt.Util;
using Pisen.Framework.AppSdk;
using Pisen.Service.Share.SSO.Contract.DataContract;
using Hyt.DataAccess.Sys;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 系统管理
    /// </summary>
    /// <remarks>2013-08-09  黄志勇 创建</remarks>
    public class SysController : BaseController
    {
        /// <summary>
        /// 操作历史记录
        /// </summary>
        public class History
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 标识sysno
            /// </summary>
            public int sysno { get; set; }
            /// <summary>
            /// 动作 add、remove
            /// </summary>
            public string action { get; set; }
        }

        #region 用户
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>true:账号不存在 false:账号已存在</returns>
        /// <remarks>2013-08-09  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public JsonResult GetSyUser(string account)
        {
            bool result = SyUserBo.Instance.GetSyUser(account) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存系统用户信息
        /// </summary>
        /// <param name="syUser">系统用户</param>
        /// <param name="histories">执行操作</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-08  杨文彬 创建</remarks>
        /// <remarks>2013-08-08  黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1008201)]
        public ActionResult SaveSyUser(SyUser syUser, IList<History> histories, string ssoId)
        {
            var result = new Result { Message = "", Status = false };
            try
            {

                syUser.Account = syUser.Account.Trim();
                syUser.UserName = syUser.UserName.Trim();
                syUser.EmailAddress = string.IsNullOrWhiteSpace(syUser.EmailAddress) ? null : syUser.EmailAddress.Trim();
                syUser.MobilePhoneNumber = syUser.MobilePhoneNumber.Trim();
                var isCreateUser = syUser.SysNo < 1;
                if (isCreateUser)
                {
                    if (SyUserBo.Instance.GetSyUser(syUser.Account) != null)
                    {
                        result.Message = "用户账号已存在";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    syUser.CreatedBy = CurrentUser.Base.SysNo;
                    syUser.CreatedDate = DateTime.Now;
                    syUser.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt("88888888"); //创建用户默认密码
                    syUser.LastUpdateBy = CurrentUser.Base.SysNo;
                    syUser.LastUpdateDate = DateTime.Now;

                    //调用方法创建syUser; 
                    var CurrentsysNo = SyUserBo.Instance.InsertSyUser(syUser);

                    //创建sso用户关联
                    //SySsoUserAssociation ssoUser=new SySsoUserAssociation();
                    //ssoUser.UserSysNo = CurrentsysNo;
                    //ssoUser.SsoId = int.Parse(ssoId);
                    //SySsoUserAssociationBo.Instance.Insert(ssoUser);
                }
                else
                {
                    #region SSO模式
                    ////清缓存 谭显锋  添加
                    //MemoryProvider.Default.Remove(string.Format(KeyConstant.SyUser,syUser.SysNo));

                    //var dbSyUser = SyUserBo.Instance.GetSyUser(syUser.SysNo);
                    //if (dbSyUser.EmailAddress != syUser.EmailAddress || dbSyUser.MobilePhoneNumber != syUser.MobilePhoneNumber || dbSyUser.Status != syUser.Status || dbSyUser.UserName != syUser.UserName)
                    //{
                    //    dbSyUser.EmailAddress = syUser.EmailAddress;
                    //    dbSyUser.MobilePhoneNumber = syUser.MobilePhoneNumber;
                    //    dbSyUser.Status = syUser.Status;
                    //    dbSyUser.UserName = syUser.UserName;
                    //    dbSyUser.LastUpdateBy = CurrentUser.Base.SysNo;
                    //    dbSyUser.LastUpdateDate = DateTime.Now;
                    //    syUser = dbSyUser;

                    //    #region  如果是企业用户，则更新用户信息到企业用户 谭显锋 添加
                    //    int ssoid = SySsoUserAssociationBo.Instance.GetSsoIdByUserSysNo(syUser.SysNo);
                    //    if (ssoid > 0) //如果是企业用户，则更新用户信息到企业用户                    
                    //        SySsoUserAssociationBo.Instance.EnterpriseUserEdit(syUser); //同时更新用户信息到企业用户 
                      
                    //    #endregion
                    //    //调用方法更新syUser;
                    //    SyUserBo.Instance.UpdateSyUser(syUser);
                    //}
                    #endregion
                     
                    #region 非SSO模式
                    var dbSyUser = SyUserBo.Instance.GetSyUser(syUser.SysNo);
                    if (dbSyUser.EmailAddress != syUser.EmailAddress || dbSyUser.MobilePhoneNumber != syUser.MobilePhoneNumber || dbSyUser.Status != syUser.Status || dbSyUser.UserName != syUser.UserName)
                    {
                        dbSyUser.EmailAddress = syUser.EmailAddress;
                        dbSyUser.MobilePhoneNumber = syUser.MobilePhoneNumber;
                        dbSyUser.Status = syUser.Status;
                        dbSyUser.UserName = syUser.UserName;
                        dbSyUser.LastUpdateBy = CurrentUser.Base.SysNo;
                        dbSyUser.LastUpdateDate = DateTime.Now;
                        syUser = dbSyUser;

                        //调用方法更新syUser;
                        SyUserBo.Instance.UpdateSyUser(syUser);
                    }
                    #endregion
                }
                if (histories != null)
                {
                    foreach (var history in histories)
                    {
                        switch (history.type.ToLower())
                        {
                            case "usergroup":
                                if (history.action == "add")
                                {
                                    //添加用户 用户组关系
                                    SyGroupUserBo.Instance.Insert(new SyGroupUser
                                    {
                                        UserSysNo = syUser.SysNo,
                                        GroupSysNo = history.sysno,
                                        CreatedBy = CurrentUser.Base.SysNo,
                                        CreatedDate = DateTime.Now,
                                        LastUpdateBy = CurrentUser.Base.SysNo,
                                        LastUpdateDate = DateTime.Now
                                    });
                                }
                                else
                                {
                                    //删除用户 用户组关系
                                    SyGroupUserBo.Instance.Delete(syUser.SysNo, history.sysno);
                                }
                                break;
                            case "userwarehouse":
                                if (history.action == "add")
                                {
                                    //添加用户 仓库关系
                                    if (!SyUserBo.Instance.ExistsSyUserWarehouse(syUser.SysNo, history.sysno))
                                    {
                                        SyUserBo.Instance.InsertSyUserWarehouse(new SyUserWarehouse
                                        {
                                            UserSysNo = syUser.SysNo,
                                            WarehouseSysNo = history.sysno,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = CurrentUser.Base.SysNo,
                                            LastUpdateDate = DateTime.Now,
                                            LastUpdateBy = CurrentUser.Base.SysNo
                                        });
                                    }
                                }
                                else
                                {
                                    //删除用户 仓库关系
                                    SyUserBo.Instance.DeleteSyUserWarehouse(syUser.SysNo, history.sysno);
                                }
                                break;
                            case "menu":
                                if (history.action == "add")
                                {
                                    //添加用户 菜单关系
                                    InsertSyPermission(syUser, (int)SystemStatus.授权目标.菜单, history);
                                }
                                else
                                {
                                    //删除用户 菜单关系
                                    DelSyPermission(syUser, (int)SystemStatus.授权目标.菜单, history);
                                }
                                break;
                            case "privilege":
                                if (history.action == "add")
                                {
                                    //添加用户 权限关系
                                    InsertSyPermission(syUser, (int)SystemStatus.授权目标.权限, history);
                                }
                                else
                                {
                                    //删除用户 权限关系
                                    DelSyPermission(syUser, (int)SystemStatus.授权目标.权限, history);
                                }
                                break;
                            case "role":
                                if (history.action == "add")
                                {
                                    //添加用户 角色关系
                                    InsertSyPermission(syUser, (int)SystemStatus.授权目标.角色, history);
                                }
                                else
                                {
                                    //删除用户 角色关系
                                    DelSyPermission(syUser, (int)SystemStatus.授权目标.角色, history);
                                }
                                break;
                        }
                    }
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "保存系统用户信息",
                                        LogStatus.系统日志目标类型.系统管理, syUser.SysNo, null, null, CurrentUser.Base.SysNo);

                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存系统用户信息错误:" + ex.Message,
                                            LogStatus.系统日志目标类型.系统管理, syUser.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 插入授权
        /// </summary>
        /// <param name="syUser">系统用户</param>
        /// <param name="target">目标编号</param>
        /// <param name="history">操作历史记录</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        private void InsertSyPermission(SyUser syUser, int target, History history)
        {
            try
            {
                //添加用户 角色关系
                SyPermissionBo.Instance.Insert(new SyPermission
                    {
                        Source = (int)SystemStatus.授权来源.系统用户,
                        SourceSysNo = syUser.SysNo,
                        Target = target,
                        TargetSysNo = history.sysno,
                        CreatedBy = CurrentUser.Base.SysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = CurrentUser.Base.SysNo,
                        LastUpdateDate = DateTime.Now
                    });
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "插入授权",
                                           LogStatus.系统日志目标类型.系统管理, syUser.SysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "插入授权错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.系统管理, syUser.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
        }

        /// <summary>
        /// 删除授权
        /// </summary>
        /// <param name="syUser">系统用户</param>
        /// <param name="target">目标编号</param>
        /// <param name="history">操作历史记录</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        private void DelSyPermission(SyUser syUser, int target, History history)
        {
            try
            {
                SyPermissionBo.Instance.Delete((int)SystemStatus.授权来源.系统用户, syUser.SysNo, target, history.sysno);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除授权",
                                          LogStatus.系统日志目标类型.系统管理, syUser.SysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除授权错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.系统管理, syUser.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="id">用户SysNo</param>
        /// <param name="page">当前页码</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-06  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult UserInfo(int? id, int page)
        {
            CBSyUser model = null;
            if (id.HasValue)
            {
                ViewBag.Warehouse = WhWarehouseBo.Instance.GetUserWarehouseList((int)id);
                var user = SyUserBo.Instance.GetSyUser((int)id);
                model = new CBSyUser
                    {
                        Account = user.Account,
                        UserName = user.UserName,
                        MobilePhoneNumber = user.MobilePhoneNumber,
                        EmailAddress = user.EmailAddress,
                        Status = user.Status,
                        SysNo = user.SysNo
                    };
                model.GroupUsers = SyUserBo.Instance.GetGroupUser((int)id) as List<SyGroupUser>;
                model.UserMeuns = SyUserBo.Instance.GetUserMenu((int)id);
                model.UserRoles = SyUserBo.Instance.GetUserRole((int)id);
            }
            ViewBag.Page = page;
            return View("UserInfo", model);
        }

        /// <summary>
        /// 企业用户信息
        /// </summary>
        /// <param name="filter">筛选参数</param>
        /// <returns>视图</returns>
        /// <remarks>2014-10-15 谭显锋 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult SSOUserList(ParaEnterpriseUserFilter filter)
        {
            Pager<CBEnterpriseUser> pager = null;
            pager = SySsoUserAssociationBo.Instance.GetAllEnterpriseUserPager(filter);
            var list = new PagedList<CBEnterpriseUser>
            {
                TData = pager.Rows,
                PageSize = filter.PageSize,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SSOUserList", list);
            }
            return View(list);
        }
        /// <summary>
        /// 添加企业用户
        /// </summary>
        /// <param name="sysNo">用户ID</param>
        /// <returns>result</returns>
        /// <remarks>2014-10-16 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult AddEnterpriseUser(string id, string account, string userName, string emaillAddress, string mobilePhoneNumber)
        {
            if (SyUserBo.Instance.GetSyUser(account) != null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            EnterpriseUser ssoUser = new EnterpriseUser();
            ssoUser.Id = int.Parse(id);
            ssoUser.Account = account;
            ssoUser.UserName = userName;
            ssoUser.EmailAddress = emaillAddress;
            ssoUser.MobilePhoneNumber = mobilePhoneNumber;
            TempData["enterpriseUserInfo"] = ssoUser;
            return Json(ssoUser, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param></param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-05  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult SyUserList()
        {
            ViewBag.SyUserGroup = GetSyUserList((int)Hyt.Model.WorkflowStatus.SystemStatus.用户组状态.启用);
            return View();
        }

        /// <summary>
        /// 用户列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult DoSyUserQuery(ParaSyUserFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                if (VHelper.ValidatorRule(new Rule_Number(filter.Keyword)).IsPass)
                    filter.MobilePhoneNumber = filter.Keyword;
                else
                    filter.Account = filter.Keyword;
            }

            var pager = SyUserBo.Instance.GetSyUser(filter);
            var list = new PagedList<CBSyUser>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_SyUserListPager", list);
        }

        /// <summary>
        /// 用户选择
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-10 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult SysUserSelector()
        {
            ViewBag.SyUserGroup = GetSyUserList((int)SystemStatus.用户组状态.启用);
            return View();
        }

        /// <summary>
        /// 用户选择分页查询
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>2013-10-10 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult DoSysUserSelectorQuery(ParaSyUserFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                if (VHelper.ValidatorRule(new Rule_Number(filter.Keyword)).IsPass)
                    filter.MobilePhoneNumber = filter.Keyword;
                else
                    filter.Account = filter.Keyword;
            }

            var pager = SyUserBo.Instance.GetSyUser(filter);
            var list = new PagedList<CBSyUser>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_SysUserSelector", list);
        }

        /// <summary>
        /// 获取用户菜单权限树
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns>用户菜单权限树</returns>
        /// <remarks>2013-08-06 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1008101)]
        public ActionResult GetTreeListByUser(int id = 0)
        {
            var lst = SyUserBo.Instance.GetTreeListByUser(id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2013-08-12 黄志勇 Crearte</remarks>
        [Privilege(PrivilegeCode.SY1008201)]
        public ActionResult SetUserStatus(int sysNo, int status)
        {
            var result = new Result { Message = "", Status = false };
            try
            {
                SyUserBo.Instance.UpdateSyUserStatus(sysNo, status);

                MemoryProvider.Default.Remove(string.Format(KeyConstant.SyUser, sysNo));
                result.Status = true;

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新用户状态",
                                         LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新用户状态错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-10-23 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1008201)]
        public JsonResult ResetPwd(int sysNo)
        {
            var result = new Result
                {
                    Message = "重置密码失败",
                    Status = false
                };
            try
            {
                var newPass = SyUserBo.Instance.ResetUserPassword(sysNo);
                if (!string.IsNullOrWhiteSpace(newPass))
                {
                    result.Status = true;
                    result.Message = "密码重置成功。新密码为:" + newPass;
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "重置密码",
                                         LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "重置密码错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户组
        /// <summary>
        /// 获取全部用户组列表
        /// </summary>
        /// <returns>用户组列表</returns>
        /// <remarks>2013-08-05  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1007101)]
        public static IList<SyUserGroup> GetAllSyUserGroup()
        {
            return SyUserGroupBo.Instance.GetSyGroupByStatus(null);
        }

        /// <summary>
        ///根据状态获取用户组
        /// </summary>
        /// <param name="status">用户组状态</param>
        /// <returns>用户组列表</returns>
        /// <remarks>2013-08-05  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1007101)]
        public List<SelectListItem> GetSyUserList(int? status)
        {
            var syUserGroup = GetAllSyUserGroup();
            if (syUserGroup != null && syUserGroup.Count > 0)
            {
                var list = syUserGroup;
                if (status.HasValue) list = list.Where(i => i.Status == status).ToList();
                if (list.Count > 0)
                {
                    return list.Select(i => new SelectListItem
                    {
                        Text = i.GroupName,
                        Value = i.SysNo.ToString()
                    }).ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <returns>用户组列表页面</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1007101)]
        public ActionResult UserGroupList()
        {
            return View();
        }

        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <param></param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1007101)]
        public ActionResult UserGroupListData()
        {
            return PartialView("_UserGroupList", SyUserGroupBo.Instance.GetList());
        }

        /// <summary>
        /// 新增用户组
        /// </summary>
        /// <returns>用户组页面</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.SY1007201)]
        public ActionResult UserGroupCreate()
        {
            return View(new CBSyUserGroup());
        }

        /// <summary>
        /// 编辑用户组信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>视图</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.SY1007201)]
        public ActionResult UserGroupEdit(int id)
        {
            return View(SyUserGroupBo.Instance.GetEntity(id));
        }

        /// <summary>
        /// 用户组
        /// </summary>
        /// <param name="model">用户组</param>
        /// <param name="groupMeuns">菜单权限</param>
        /// <param name="groupRoles">角色</param>
        /// <returns>视图</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SY1007201)]
        public ActionResult UserGroupSave(SyUserGroup model, List<SyUserGroupMenu> groupMeuns, List<SyUserGroupRole> groupRoles)
        {
            Result r = new Result
            {
                Status = true
            };
            try
            {
                int userid = CurrentUser.Base.SysNo;
                if (model.SysNo > 0)
                {
                    model.LastUpdateBy = userid;
                    model.LastUpdateDate = DateTime.Now;
                }
                else
                {
                    model.CreatedBy = userid;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = userid;
                    model.LastUpdateDate = DateTime.Now;
                }

                SyUserGroupBo.Instance.SaveUserGroup(model, groupMeuns, groupRoles);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "保存用户组",
                                    LogStatus.系统日志目标类型.系统管理, model.SysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存用户组错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.系统管理, model.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="id">用户组编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SY1007401)]
        public ActionResult UserGroupDelete(int id)
        {
            Result r = new Result
            {
                Status = true
            };
            try
            {

                SyUserGroupBo.Instance.DeleteUserGroup(id, CurrentUser.Base);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除用户组",
                                    LogStatus.系统日志目标类型.系统管理, id, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除用户组错误:" + ex.Message,
                                       LogStatus.系统日志目标类型.系统管理, id, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查用户组是否存在
        /// </summary>
        /// <param name="groupName">用户组名称</param>
        /// <returns>json</returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1007101)]
        public ActionResult NotExistUserGroup(string groupName)
        {
            bool flg = SyUserGroupBo.Instance.GetByGroupName(groupName) == null;
            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户组菜单权限树
        /// </summary>
        /// <param name="id">用户组编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1007101)]
        public ActionResult GetTreeListByUserGroup(int id = 0)
        {
            var lst = SyUserGroupBo.Instance.GetTreeListByUserGroup(id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用启用用户分组
        /// </summary>
        /// <param name="id">用户组编号</param>
        /// <param name="disabled">true 禁用 false 启用</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1007701)]
        public ActionResult UserGroupDisabled(int id, bool disabled)
        {
            Result r = new Result
            {
                Status = true
            };
            try
            {
                SyUserGroupBo.Instance.DisabledUserGroup(id, disabled, CurrentUser.Base);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "禁用启用用户分组",
                                        LogStatus.系统日志目标类型.系统管理, id, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "禁用启用用户分组错误:" + ex.Message,
                                       LogStatus.系统日志目标类型.系统管理, id, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 权限管理 2013－08-05 朱家宏 创建

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005101)]
        public ActionResult Privileges()
        {
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<SystemStatus.权限状态>(null, null).ToString());
            return View();
        }

        /// <summary>
        /// 权限分页
        /// </summary>
        /// <param name="id">当前页号</param>
        /// <param name="keyword">权限名称/权限代码</param>
        /// <param name="status">权限状态</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005101)]
        public ActionResult DoPrivilegeQuery(int? id, string keyword, int? status)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = SyPrivilegeBo.Instance.GetPagerList(currentPage, pageSize, status, keyword);

            var list = new PagedList<SyPrivilege>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
            return PartialView("_PrivilegePager", list);
        }

        /// <summary>
        /// 权限状态切换
        /// </summary>
        /// <param name="sysNo">权限编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005701)]
        public JsonResult ChangePrivilegeStatus(int sysNo)
        {

            var r = new Result();
            r.Status = SyPrivilegeBo.Instance.ChangePrivilegeStatus(sysNo);

            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 权限表单
        /// </summary>
        /// <param name="id">权限编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005101)]
        public ActionResult Privilege(int? id)
        {
            var privilege = id != null ? SyPrivilegeBo.Instance.Get((int)id) : new SyPrivilege();
            return View("Privilege", privilege);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="sysNo">权限编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005401)]
        public JsonResult RemovePrivilege(int sysNo)
        {
            var r = new Result();
            try
            {

                r = SyPrivilegeBo.Instance.RemovePrivilege(sysNo);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除权限",
                                   LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除权限错误:" + ex.Message,
                                      LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005201)]
        public JsonResult SavePrivilege(SyPrivilege model)
        {
            var result = new Result();
            try
            {

                if (model.SysNo != 0)
                    result = SyPrivilegeBo.Instance.SavePrivilege(model); //修改
                else
                    result = SyPrivilegeBo.Instance.CreatePrivilege(model); //创建
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "保存权限",
                                    LogStatus.系统日志目标类型.系统管理, model.SysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存权限错误:" + ex.Message,
                                    LogStatus.系统日志目标类型.系统管理, model.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 权限查询
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005101)]
        public ActionResult QueryPrivilege()
        {
            return View();
        }

        /// <summary>
        /// 获取未绑定的权限分页
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1005101)]
        public ActionResult PrivilegeQuery(int? id, string keyword)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = SyPrivilegeBo.Instance.SearchUnUsedPrivileges(keyword, currentPage, pageSize);

            var list = new PagedList<CBSyPrivilege>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PrivilegeQueryPager", list);
        }

        #endregion

        #region 菜单管理 2013－08-06 朱家宏 创建

        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public ActionResult MenuManager()
        {
            //一级菜单列表
            //var topMenus = SyMenuBO.Instance.GetTopMenus().ToList();

            //var menus = new List<SyMenu>();
            //foreach (var menu in topMenus)
            //{
            //    if (menu.Level != 1)
            //    {
            //        var prefix = "├" + new string(char.Parse("-"), menu.Level * 2);
            //        menu.MenuName = prefix + menu.MenuName;
            //        menus.Add(menu);
            //    }
            //}

            //ViewBag.topMenus = topMenus;

            return View();
        }

        /// <summary>
        /// 菜单list box
        /// </summary>
        /// <param name="selectedNodeId">当前选中菜单编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult LoadMenuSelectTree(int? selectedNodeId)
        {
            var topMenus = SyMenuBO.Instance.GetTopMenus().ToList();

            var menus = new List<SyMenu>();
            foreach (var menu in topMenus)
            {
                if (menu.Level != 1)
                {
                    var prefix = "├" + new string(char.Parse("-"), menu.Level * 2);
                    menu.MenuName = prefix + menu.MenuName;
                }
                menus.Add(menu);
            }

            if (selectedNodeId != null)
            {
                //移出当前选择菜单及子菜单
                var childrenMenu = new List<SyMenu>();
                SyMenuBO.Instance.DoChildNodeRead((int)selectedNodeId, ref childrenMenu);
                var currentMenu = menus.SingleOrDefault(o => o.SysNo == selectedNodeId);
                if (currentMenu != null) menus.Remove(currentMenu);
                foreach (var item in childrenMenu)
                {
                    var child = menus.SingleOrDefault(o => o.SysNo == item.SysNo);
                    menus.Remove(child);
                }
            }

            var list = menus.Select(i => new
            {
                text = i.MenuName,
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单树
        /// </summary>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult GetMenuTree()
        {
            var tree = SyMenuBO.Instance.GetMenuTree();

            var nodes = from c in tree
                        select new
                            {
                                id = c.SysNo,
                                name = c.MenuName,
                                open = false,
                                pId = c.ParentSysNo,
                                status = c.Status
                            };

            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="id">sysNo</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult GetMenu(int id)
        {
            var menu = SyMenuBO.Instance.GetMenu(id);
            return Json(menu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单上移/下移
        /// </summary>
        /// <param name="sourceNodeId">原节点</param>
        /// <param name="targetNodeId">目标节点</param>
        /// <param name="direction">移动方向</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006201)]
        public JsonResult MoveTreeNode(int sourceNodeId, int targetNodeId, string direction)
        {

            var r = new Result { Status = SyMenuBO.Instance.MoveTreeNode(sourceNodeId, targetNodeId, direction) };

            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 菜单 添加/保存
        /// </summary>
        /// <param name="menu">菜单</param>
        /// <param name="privileges">权限</param>
        /// <returns>json</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006201)]
        public JsonResult SaveMenu(SyMenu menu, List<int> privileges)
        {
            var currentUser = CurrentUser.Base.SysNo;
            Result result = new Result();
            try
            {

                if (menu.SysNo != 0)
                {
                    menu.LastUpdateBy = currentUser;
                    menu.LastUpdateDate = DateTime.Now;

                    result = SyMenuBO.Instance.SaveMenu(menu, privileges); //修改
                }
                else
                {
                    menu.CreatedBy = currentUser;
                    menu.LastUpdateBy = currentUser;
                    result = SyMenuBO.Instance.CreateMenu(menu, privileges); //创建
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "菜单添加/保存",
                                    LogStatus.系统日志目标类型.系统管理, menu.SysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "菜单添加/保存错误:" + ex.Message,
                                   LogStatus.系统日志目标类型.系统管理, menu.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单 禁用/启用
        /// </summary>
        /// <param name="sysNo">菜单编号</param>
        /// <returns>json bool</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006701)]
        public JsonResult ChangeMenuStatus(int sysNo)
        {
            bool r = false;
            try
            {

                r = SyMenuBO.Instance.ChangeMenuStatus(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "菜单禁用/启用",
                                  LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "菜单禁用/启用错误:" + ex.Message,
                                   LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单 是否在导航显示
        /// </summary>
        /// <param name="sysNo">菜单编号</param>
        /// <returns>json bool</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult ShowInNavigator(int sysNo)
        {
            var r = SyMenuBO.Instance.ChangeInNavigatorStatus(sysNo);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="sysNo">菜单编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006401)]
        public JsonResult RemoveMenu(int sysNo)
        {
            bool r = false;
            try
            {
                r = SyMenuBO.Instance.RemoveMenu(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除菜单",
                                    LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除菜单错误:" + ex.Message,
                                   LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 拥有的权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>json list</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult GetUsedPrivileges(int menuSysNo)
        {
            var privileges = SyMenuBO.Instance.GetUsedPrivileges(menuSysNo);

            return Json(privileges, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询未使用过的权限
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <returns>json list</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public JsonResult SearchUnUsedPrivileges(string keyword)
        {
            var privileges =
                SyMenuBO.Instance.SearchUnUsedPrivileges(keyword)
                        .Select(o => new { name = o.Name, sysNo = o.SysNo, code = o.Code });

            return Json(privileges, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单图标
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1006101)]
        public ActionResult Icons()
        {
            var physicalFolderPath = Server.MapPath(Constant.MENUICO_FOLDER_NAME);

            if (!Directory.Exists(physicalFolderPath))
                throw new IOException("未找到文件夹。(" + Constant.MENUICO_FOLDER_NAME + ")");

            var icoFolder = new DirectoryInfo(physicalFolderPath);
            var icons = new List<MenuIcon>();

            foreach (var ico in icoFolder.GetFiles())
            {
                icons.Add(new MenuIcon
                    {
                        FileName = ico.Name,
                        FileSize = ico.Length
                    });
            }
            return View("Icons", icons);
        }

        #endregion

        #region 角色
        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns>角色列表</returns>
        /// <remarks>2013-08-06  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004101)]
        public static IList<SyRole> GetAllRole()
        {
            return SyRoleBo.Instance.GetList();
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 角色管理列表
        /// </summary>
        /// <returns>角色管理页面</returns>
        /// <remarks>2013-08-06  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004101)]
        public ActionResult SyRoleList()
        {
            return View();
        }

        /// <summary>
        /// 创建/编辑角色
        /// </summary>
        /// <param name="id">角色编号</param>
        /// <returns>角色管理页面</returns>
        /// <remarks>2013-08-06  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004201)]
        public ActionResult SyRoleCreate(int? id)
        {
            SyRole model = new SyRole();
            if (id != null)
            {
                model = SyRoleBo.Instance.Get(id.Value);
            }
            else
            {
                model.Status = (int)SystemStatus.角色状态.启用;   //新增时设置状态默认值为有效
            }
            return View(model);
        }

        /// <summary>
        /// 获取角色菜单权限树
        /// </summary>
        /// <param name="id">角色编号</param>
        /// <returns>角色菜单权限树</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004101)]
        public ActionResult GetTreeListByRoleSysNo(int? id)
        {
            var roleNo = id ?? 0;
            var lst = SyRoleBo.Instance.GetTreeListByRoleSysNo(roleNo);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="id">当前页号</param>
        /// <param name="status">状态</param>
        /// <returns>分页查询页面</returns>
        /// <remarks>2013-08-06  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004101)]
        public ActionResult DoRoleQuery(int? id, int? status)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = SyRoleBo.Instance.GetPagerList(status, currentPage, pageSize);

            var list = new PagedList<SyRole>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_SyRoleListPager", list);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sysNo">角色编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-06  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004401)]
        public JsonResult RemoveRole(int sysNo)
        {
            var result = new Result();
            try
            {
                result = SyRoleBo.Instance.Delete(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除角色",
                                      LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除角色错误:" + ex.Message,
                                 LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="model"></param>
        /// <param name="groupMeuns">角色菜单权限</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-06  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1004201)]
        public JsonResult SaveRole(SyRole model, List<SyRoleMenuPrivilege> groupMeuns)
        {
            Result result = new Result();
            try
            {

                result = SyRoleBo.Instance.Save(model, groupMeuns, CurrentUser.Base.SysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "保存角色",
                                    LogStatus.系统日志目标类型.系统管理, model.SysNo, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存角色错误:" + ex.Message,
                               LogStatus.系统日志目标类型.系统管理, model.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启用（禁用）角色
        /// </summary>
        /// <param name="sysNo">角色编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1004701)]
        public JsonResult ChangeRoleStatus(int sysNo)
        {
            var r = new Result();
            try
            {
                r = SyRoleBo.Instance.ChangeStatus(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "启用（禁用）角色",
                                      LogStatus.系统日志目标类型.系统管理, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "启用（禁用）角色错误:" + ex.Message,
                             LogStatus.系统日志目标类型.系统管理, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 内存管理

        #region 内存公用
        /// <summary>
        /// 缓存详情
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="type">memcached、memoryCache</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-15  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002101, PrivilegeCode.SY1003101)]
        public ActionResult CacheDetailList(string key, string type)
        {
            string attribute;
            object obj;
            if (type != null && type.ToLower() == "memcached")
            {
                var index = key.IndexOf('_');
                var prefix = index < 0 ? key : key.Substring(0, index + 1);
                var item = (CacheKeys.Items)Enum.Parse(typeof(CacheKeys.Items), prefix);
                attribute = item.GetDescription();
                obj = CacheManager.Instance.Get<object>(key);
            }
            else
            {
                attribute = "Description";
                obj = MemoryProvider.Default.Get(key);
            }
            if (obj != null)
            {
                Type t = obj.GetType().GetGenericArguments().Length > 0
                             ? obj.GetType().GetGenericArguments()[0]
                             : obj.GetType();
                var list = MemoryBo.Instance.GetPropertys(t);
                if (list != null && list.Count > 0) ViewBag.Propertys = list;
            }
            ViewBag.Key = key;
            ViewBag.Prompt = attribute;
            ViewBag.Type = type;
            return View();
        }

        /// <summary>
        /// 缓存详情分页查询
        /// </summary>
        /// <param name="id">当前页号</param>
        /// <param name="type">类型</param>
        /// <param name="key">key</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-13  余勇 创建</remarks>
        /// <remarks>2013-08-15  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1002101, PrivilegeCode.SY1003101)]
        public ActionResult DoCacheDetailQuery(int? id, string type, string key, string propertyName, string propertyValue)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            Pager<object> pager;
            if (type != null && type.ToLower() == "memcached")
                pager = MemoryBo.Instance.GetMemcacheData(key, propertyName, propertyValue, pageSize, currentPage);
            else
                pager = MemoryBo.Instance.GetNetData(key, propertyName, propertyValue, pageSize, currentPage);

            var list = new PagedList<object>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (pager.Rows != null && pager.Rows.Count > 0)
            {
                ViewBag.TableHead = GetTableHead(pager.Rows[0] as DataRow);
            }
            return PartialView("_CacheDetailListPager", list);
        }

        /// <summary>
        /// 获取DataTable表头
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns>DataTable表头</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks>
        public List<string> GetTableHead(DataRow dr)
        {
            var list = new List<string>();
            if (dr != null)
            {
                foreach (DataColumn col in dr.Table.Columns)
                {
                    list.Add(col.ColumnName);
                }
            }
            return list;
        }
        #endregion

        #region 内存键值对
        /// <summary>
        /// 获取所有内存键值对
        /// </summary>
        /// <param></param>
        /// <returns>所有内存键值对列表</returns>
        /// <remarks>2013-08-14 余勇 创建</remarks>
        /// <remarks>2013-08-14  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002101)]
        public static List<SelectListItem> GetAllmcachedPrefix()
        {
            var list = EnumUtil.ToListItem<CacheKeys.Items>();
            return list;
        }

        /// <summary>
        /// 内存键值对列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        /// <remarks>2013-08-14  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002101)]
        public ActionResult MemcachedList()
        {
            #region 测试
            //Pager<CBSoOrder> pager = new Pager<CBSoOrder> { PageSize = 100 };
            //var filter = new ParaOrderFilter { }; // new ParaOrderFilter { ExecutorSysNo = CurrentUser.Base.SysNo };
            //BLL.Order.SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
            //foreach (var order in pager.Rows)
            //{
            //    CacheManager.Get<CBSoOrder>(CacheKeys.Items.OrderList_, order.SysNo.ToString(), () => { return order; },
            //                                MethodOptions.ForcedUpdating);
            //}
            //CacheManager.Instance.Set("IntTest", 25);
            //CacheManager.Instance.Set("DateTest", DateTime.Now);
            //CacheManager.Instance.Set("ArrayTest", new int[]{1,2,3});
            #endregion
            ViewBag.KeyAmount = MemoryBo.Instance.GetMemcacheKeyCount();
            ViewBag.MemcacheCount = Hyt.Util.FormatUtil.FormatByteCount(MemoryBo.Instance.GetMemcacheTotal());
            return View();
        }

        /// <summary>
        /// 内存键值对分页查询
        /// </summary>
        /// <param name="id">当前页号</param>
        /// <param name="key">key</param>
        /// <param name="extension">后缀</param>
        /// <returns>分页查询页面</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        /// <remarks>2013-08-14  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1002101)]
        public ActionResult DoMemcachedQuery(int? id, string key, string extension)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            key = string.Format("{0}{1}", key, extension);
            var pager = MemoryBo.Instance.GetMemcachePagerList(key, pageSize, currentPage);

            var list = new PagedList<SyKeyInfo>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_MemcachedListPager", list);
        }

        /// <summary>
        /// 删除内存键值对
        /// </summary>
        /// <param name="key">健值对key</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        public JsonResult DeleteMemcached(string key)
        {
            Result result = new Result();
            try
            {
                result.Status = MemoryBo.Instance.DelMemcacheKey(key);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除内存键值对",
                                    LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除内存键值对错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除内存键值对
        /// </summary>
        /// <param name="keyPrefix">key前缀</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        /// <remarks>2013-08-19  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        public JsonResult DeleteMemcachedByPrefix(string keyPrefix)
        {
            Result result = new Result();
            try
            {
                result.Status = MemoryBo.Instance.DeleteByPrefix(keyPrefix);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "批量删除内存键值对",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "批量删除内存键值对错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 分销商删除缓存
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        /// <remarks>2016-1-20  王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1011506)]
        public JsonResult DeleteDealerMemcachedByPrefix(string dealerSysNo)
        {
            Result result = new Result();
            try
            {
                result.Status = MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoresProductList_.ToString() + dealerSysNo.ToString());
                result.Status = MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoreAll.ToString() + dealerSysNo.ToString());
                result.Status = MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoresProduct_.ToString() + dealerSysNo.ToString());
                result.Status = MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoresInfo_.ToString() + dealerSysNo.ToString());
                MemoryBo.Instance.DelAllWebSiteCache(int.Parse(dealerSysNo));
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除分销商商品内存键值对",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除分销商商品内存键值对错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除所有内存键值对
        /// </summary>
        /// <returns>执行结果josn对象</returns>
        /// <remarks>2013-11-20  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        public JsonResult DeleteAllMemcached()
        {
            Result result = new Result();
            try
            {

                MemoryBo.Instance.DelAllMemcache();
                result.Status = true;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除所有内存键值对",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除所有内存键值对错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据key返回基本信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>基本信息</returns>
        /// <remarks>2013-11-21 黄志勇 添加</remarks>
        [Privilege(PrivilegeCode.SY1002101)]
        public JsonResult GetMemcacheKeyInfo(string key)
        {
            Result result = new Result() { Status = false };
            try
            {
                var info = MemoryBo.Instance.GetMemcacheKeyInfo(key);
                if (info != null) result.Message = info.KeyValue;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 内存数据库
        /// <summary>
        /// 内存数据库列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        /// <remarks>2013-08-15  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1003101)]
        public ActionResult MemoryCacheList()
        {
            #region 测试
            //Pager<CBSoOrder> pager = new Pager<CBSoOrder> { PageSize = 100 };
            //var filter = new ParaOrderFilter { }; // { ExecutorSysNo = CurrentUser.Base.SysNo }};
            //BLL.Order.SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
            //ManagerProvider.MemoryCache.Set("OrderList", pager.Rows, 10);
            //ManagerProvider.MemoryCache.Set("IntTest", 25, 10);
            //ManagerProvider.MemoryCache.Set("DateTest", DateTime.Now, 10);
            #endregion
            ViewBag.KeyAmount = MemoryBo.Instance.GetNetKeyCount();
            ViewBag.MemcacheCount = Hyt.Util.FormatUtil.FormatByteCount(MemoryBo.Instance.GetNetTotal());
            return View();
        }

        /// <summary>
        /// 内存数据库列表分页查询
        /// </summary>
        /// <param name="id">当前页号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  余勇 创建</remarks>
        /// <remarks>2013-08-14  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1003101)]
        public ActionResult DoMemoryCacheQuery(int? id)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = MemoryBo.Instance.GetNetPagerList(pageSize, currentPage);

            var list = new PagedList<SyKeyInfo>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_MemoryCacheListPager", list);
        }

        /// <summary>
        /// 删除内存数据库
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-14  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        [HttpPost]
        public JsonResult DeleteMemory(string key)
        {
            var result = new Result { Status = false };
            try
            {

                MemoryProvider.Default.Remove(key);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除内存数据库",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除内存数据库错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除所有内存数据库
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-20  余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        [HttpPost]
        public JsonResult DeleteAllMemory(string key)
        {
            var result = new Result { Status = false };
            try
            {

                MemoryProvider.Default.Clear();
                result.Status = true;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除所有内存数据库",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除所有内存数据库错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除内存数据库
        /// </summary>
        /// <param name="keyPrefix">key前缀</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-12-5  黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SY1002401)]
        public JsonResult DeleteNetByPrefix(string keyPrefix)
        {
            var result = new Result() { Status = false };
            try
            {

                result.Status = MemoryBo.Instance.DeleteByNetPrefix(keyPrefix);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "批量删除内存数据库",
                                 LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);

                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "批量删除内存数据库错误:" + ex.Message,
                            LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据key返回基本信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>内存数据库信息</returns>
        /// <remarks>2013-10-21 黄志勇 添加</remarks>
        [Privilege(PrivilegeCode.SY1003101)]
        public JsonResult GetNetKeyInfo(string key)
        {
            Result result = new Result() { Status = false };
            try
            {
                var info = MemoryBo.Instance.GetNetKeyInfo(key);
                if (info != null) result.Message = info.KeyValue;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.DenyGet);

        }

        ///// <summary>
        ///// 重建
        ///// </summary>
        ///// <param name="key">健值对key</param>
        ///// <returns></returns>
        ///// <remarks>2013-08-14  余勇 创建</remarks>
        //[Privilege(PrivilegeCode.SY1002401)]
        //public JsonResult RebuildMemoryCache(string key)
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (var tran = new TransactionScope())
        //        {
        //            result = SyRoleBo.Instance.Save(null, null);  //须替换
        //            tran.Complete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #endregion

        #region 全文索引管理
        /// <summary>
        /// 全文索引管理
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1001101)]
        public ActionResult FullTextIndexing()
        {
            ViewBag.IndexDir = Hyt.Infrastructure.Lucene.ProductIndex.IndexStorePath;
            bool IsCreate = Hyt.Infrastructure.Lucene.ProductIndex.IsEnableCreated();
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(!IsCreate);//开始
            int? DocCount = Hyt.Infrastructure.Lucene.ProductIndex.GetDocCount();
            ViewBag.DocCount = DocCount;
            DateTime? LastUpdateTime = Hyt.Infrastructure.Lucene.ProductIndex.GetLastUpdateTime();
            if (LastUpdateTime != null)
            {
                ViewBag.LastUpdateTime = LastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm");
            }
            List<string> Fields = Hyt.Infrastructure.Lucene.ProductIndex.GetFields();
            ViewBag.Fields = Fields;
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CloseWithoutOptimize();//结束

            return View();
        }

        /// <summary>
        /// 分页获取全文索引数据
        /// </summary>
        /// <param name="id">页编号</param>
        /// <param name="fieldName">fieldName</param>
        /// <param name="keywords">查询关键字</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-15  朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SY1001101)]
        public ActionResult _FullTextIndexing(int? id, string fieldName, string keywords)
        {
            int pageindex = id.HasValue ? id.Value : 1;
            int pageSize = 30;
            int recordCount;
            var lst = Hyt.Infrastructure.Lucene.ProductIndex.Instance.QueryDoc(fieldName, keywords, pageindex, pageSize, out recordCount);
            var PageList = new PagedList<CBPdProductIndex>
            {
                TData = lst,
                CurrentPageIndex = pageindex,
                TotalItemCount = recordCount,
                PageSize = pageSize
            };
            return PartialView("_FullTextIndexing", PageList);
        }

        /// <summary>
        /// 创建全部产品索引
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1001301)]
        public ActionResult CreateAllProductIndex()
        {
            Result r = new Result();
            r.Status = true;
            try
            {
                var lsts = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct();
                if (lsts != null && lsts.Count > 0)
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(true);
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxMergeFactor = 301;
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxBufferedDocs = 301;
                    foreach (var item in lsts)
                    {
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.IndexString(item);
                    }
                    // Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close(); E:\\Pisen\\Hyt\\dict\\Dict.Dct
                    r.StatusCode = lsts.Count;
                    r.Message = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//用做索引更新时间
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建全部产品索引",
                                    LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);
                }
                else
                {
                    r.Message = "没有获取到产品!";
                    r.Status = false;
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = false;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "创建全部产品索引错误:" + ex.Message,
                           LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            finally
            {
                if (r.Status)
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close();
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 索引优化
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-16  朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1001301)]
        public ActionResult IndexingOptimization()
        {
            Result r = new Result();
            r.Status = true;
            try
            {
                Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(false);
                //Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close();
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "索引优化",
                                    LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = false;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "索引优化错误:" + ex.Message,
                           LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
            }
            finally
            {
                Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close();
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="sysNos">索引编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1001401)]
        public ActionResult IndexingDelete(int[] sysNos)
        {
            Result r = new Result();
            r.Status = true;
            if (sysNos != null && sysNos.Length > 0)
            {
                try
                {
                    for (int i = 0; i < sysNos.Length; i++)
                    {
                        var model = new PdProductIndex()
                        {
                            SysNo = sysNos[i]
                        };
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.DeleteIndex(model);
                    }
                    r.Message = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//用做索引更新时间
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除索引",
                                    LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);
                }
                catch (Exception ex)
                {
                    r.Status = false;
                    r.Message = ex.Message;
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除索引错误:" + ex.Message,
                          LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="sysNos">索引编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-14  朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1001301)]
        public ActionResult IndexingUpdate(int[] sysNos)
        {
            Result r = new Result();
            r.Status = true;
            if (sysNos != null && sysNos.Length > 0)
            {
                try
                {
                    var lsts = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct().Where(m => sysNos.Contains(m.SysNo)).ToList();
                    if (lsts != null && lsts.Count > 0)
                    {
                        foreach (var item in lsts)
                        {
                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.UpdateIndex(item);
                        }
                    }
                    r.Message = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//用做索引更新时间
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新索引",
                                   LogStatus.系统日志目标类型.系统管理, 0, null, null, CurrentUser.Base.SysNo);
                }
                catch (Exception ex)
                {
                    r.Status = false;
                    r.Message = ex.Message;
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新索引错误:" + ex.Message,
                          LogStatus.系统日志目标类型.系统管理, 0, ex, null, CurrentUser.Base.SysNo);
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 日志管理 2013-08-15 朱家宏创建

        /// <summary>
        /// 日志管理
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-23 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1009101)]
        public ActionResult Logs()
        {
            //日志级别list box
            ViewBag.LogLevel = MvcHtmlString.Create(MvcCreateHtml.EnumToString<LogStatus.SysLogLevel>(null, null).ToString());
            //日志来源list box
            ViewBag.Source = MvcHtmlString.Create(MvcCreateHtml.EnumToString<LogStatus.系统日志来源>(null, null).ToString());
            //操作对象类型list box
            ViewBag.TargetType = MvcHtmlString.Create(MvcCreateHtml.EnumToString<LogStatus.系统日志目标类型>(null, null).ToString());

            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分布视图</returns>
        /// <remarks>2013-10-23 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1009101)]
        public ActionResult DoLogPaging(ParaSystemLogFilter filter)
        {
            var pager = SysLog.Instance.GetLogs(filter);

            var list = new PagedList<SySystemLog>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_LogsPager", list);
        }

        #endregion

        #region Eas同步日志
        /// <summary>
        /// Eas同步日志列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult EasSyncLogList()
        {
            return View();
        }

        /// <summary>
        /// Eas同步日志列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>Eas同步日志列表</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoEasSyncLogQuery(ParaEasSyncLogFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);
            filter.Warehouses = string.Join(",", warehouses) + ",0";
            var pager = EasBo.Instance.GetList(filter);
            return PartialView("_EasSyncLogListPager", pager.Map());
        }

        /// <summary>
        ///  Eas重新同步
        /// </summary>
        /// <param name="sysNo">Eas同步日志表系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001108)]
        public ActionResult EasSyn(int sysNo)
        {
            Result result;
            var b = EasBo.Instance.IsRelate(sysNo);
            if (!b)
            {
                var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
                var isSave = CurrentUser.PrivilegeList.HasPrivilege(Model.SystemPredefined.PrivilegeCode.EAS1001110);
                var syncResult = client.Resynchronization(sysNo, isSave);
                result = new Result()
                {
                    Status = syncResult.Status,
                    Message = sysNo + " : " + syncResult.Message
                };
                //最新更新人
                var model = EasBo.Instance.GetEntity(sysNo);
                model.LastupdateBy = CurrentUser.Base.SysNo;
                EasBo.Instance.Update(model);

            }
            else
            {
                result = new Result()
                {
                    Status = false,
                    Message = "请先同步此单的历史关联单据,现在就去处理？"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Eas作废
        /// </summary>
        /// <param name="sysNo">Eas同步日志表系统编号</param>
        /// <param name="remarks"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-4-14 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult InvalidEas(int sysNo, string remarks)
        {
            var model = EasBo.Instance.GetEntity(sysNo);
            model.Status = (int)Extra.Erp.Model.同步状态.作废;
            model.LastupdateDate = DateTime.Now;
            model.LastupdateBy = CurrentUser.Base.SysNo;
            model.Remarks = model.Remarks + ":" + remarks;
            var r = EasBo.Instance.Update(model);
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Eas批量作废
        /// </summary>
        /// <param name="sysNos">Eas同步日志表系统编号数组</param>
        /// <param name="remarks">作废原因</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-07-02 余勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult BatchInvalidEas(int[] sysNos, string remarks)
        {
            foreach (var sysNo in sysNos)
            {
                var model = EasBo.Instance.GetEntity(sysNo);
                if (model != null && (model.Status != (int)Extra.Erp.Model.同步状态.作废))
                {
                    if (model.Status == (int)Extra.Erp.Model.同步状态.成功) continue;
                    model.Status = (int)Extra.Erp.Model.同步状态.作废;
                    model.LastupdateDate = DateTime.Now;
                    model.LastupdateBy = CurrentUser.Base.SysNo;
                    model.Remarks = model.Remarks + ":" + remarks;
                    var r = EasBo.Instance.Update(model);
                }
            }
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Eas同步日志数据
        /// </summary>
        /// <param name="sysNo">日志编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-10-23  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult EasDetailList(int sysNo)
        {
            ViewBag.Key = sysNo;
            return View();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sysNo">日志系统编号</param>
        /// <param name="id">当前页号</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>视图</returns>
        /// <remarks>2013-10-23  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoEasQuery(int sysNo, int id = 1, int pageSize = 10)
        {
            PagedList<object> list = null;
            var entity = EasBo.Instance.GetEntity(sysNo);
            if (entity != null && !string.IsNullOrEmpty(entity.Data))
            {
                var value = entity.Data.ToObject<SaleInfoWraper>();
                Pager<object> pager = EasBo.Instance.GetEasData(value, pageSize, id);
                list = new PagedList<object>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
                if (pager.Rows != null && pager.Rows.Count > 0)
                {
                    ViewBag.TableHead = GetTableHead(pager.Rows[0] as DataRow);
                }
            }
            return PartialView("_EasDetailListPager", list);
        }

        /// <summary>
        /// Eas日志导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public void ExportEasSyncLog(ParaEasSyncLogFilter filter)
        {
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);

            filter.Id = 1;
            filter.PageSize = 1048576;
            filter.Warehouses = string.Join(",", warehouses) + ",0";

            var result = new List<CBEasSyncLog>();
            var r = EasBo.Instance.GetList(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            Util.ExcelUtil.Export(result);
        }

        #endregion

        #region 利嘉同步日志
        /// <summary>
        /// 利嘉同步日志列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2017-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult LiJiaSyncLogList()
        {
            return View();
        }

        /// <summary>
        /// 利嘉同步日志列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>利嘉同步日志列表</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoLiJiaSyncLogQuery(ParaLiJiaSyncLogFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);
            filter.Warehouses = string.Join(",", warehouses) + ",0";
            var pager = LiJiaBo.Instance.GetList(filter);
            return PartialView("_LiJiaSyncLogListPager", pager.Map());
        }

        /// <summary>
        /// LiJia重新同步
        /// </summary>
        /// <param name="sysNo">LiJia同步日志表系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001108)]
        public ActionResult LiJiaSyn(int sysNo)
        {
            Result result;
            var b = LiJiaBo.Instance.IsRelate(sysNo);
            if (!b)
            {
                var client = Extra.Erp.LiJia.LiJiaProviderFactory.CreateProvider();
                var isSave = CurrentUser.PrivilegeList.HasPrivilege(Model.SystemPredefined.PrivilegeCode.EAS1001110);
                var syncResult = client.Resynchronization(sysNo, isSave);
                result = new Result()
                {
                    Status = syncResult.Status,
                    Message = sysNo + " : " + syncResult.Message
                };
                //最新更新人
                var model = LiJiaBo.Instance.GetEntity(sysNo);
                model.LastupdateBy = CurrentUser.Base.SysNo;
                LiJiaBo.Instance.Update(model);

            }
            else
            {
                result = new Result()
                {
                    Status = false,
                    Message = "请先同步此单的历史关联单据,现在就去处理？"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  LiJia作废
        /// </summary>
        /// <param name="sysNo">LiJia同步日志表系统编号</param>
        /// <param name="remarks"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult InvalidLiJia(int sysNo, string remarks)
        {
            var model = LiJiaBo.Instance.GetEntity(sysNo);
            model.Status = (int)Extra.Erp.Model.同步状态.作废;
            model.LastupdateDate = DateTime.Now;
            model.LastupdateBy = CurrentUser.Base.SysNo;
            model.Remarks = model.Remarks + ":" + remarks;
            var r = LiJiaBo.Instance.Update(model);
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  LiJia批量作废
        /// </summary>
        /// <param name="sysNos">LiJia同步日志表系统编号数组</param>
        /// <param name="remarks">作废原因</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult BatchInvalidLiJia(int[] sysNos, string remarks)
        {
            foreach (var sysNo in sysNos)
            {
                var model = LiJiaBo.Instance.GetEntity(sysNo);
                if (model != null && (model.Status != (int)Extra.Erp.Model.同步状态.作废))
                {
                    if (model.Status == (int)Extra.Erp.Model.同步状态.成功) continue;
                    model.Status = (int)Extra.Erp.Model.同步状态.作废;
                    model.LastupdateDate = DateTime.Now;
                    model.LastupdateBy = CurrentUser.Base.SysNo;
                    model.Remarks = model.Remarks + ":" + remarks;
                    var r = LiJiaBo.Instance.Update(model);
                }
            }
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LiJia同步日志数据
        /// </summary>
        /// <param name="sysNo">日志编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult LiJiaDetailList(int sysNo)
        {
            ViewBag.Key = sysNo;
            return View();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sysNo">日志系统编号</param>
        /// <param name="id">当前页号</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>视图</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoLiJiaQuery(int sysNo, int id = 1, int pageSize = 10)
        {
            PagedList<object> list = null;
            var entity = LiJiaBo.Instance.GetEntity(sysNo);
            if (entity != null && !string.IsNullOrEmpty(entity.Data))
            {
                var value = entity.Data.ToObject<SaleInfoWraper>();
                Pager<object> pager = LiJiaBo.Instance.GetLiJiaData(value, pageSize, id);
                list = new PagedList<object>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
                if (pager.Rows != null && pager.Rows.Count > 0)
                {
                    ViewBag.TableHead = GetTableHead(pager.Rows[0] as DataRow);
                }
            }
            return PartialView("_LiJiaDetailListPager", list);
        }

        /// <summary>
        /// LiJia日志导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>空</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public void ExportLiJiaSyncLog(ParaLiJiaSyncLogFilter filter)
        {
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);

            filter.Id = 1;
            filter.PageSize = 1048576;
            filter.Warehouses = string.Join(",", warehouses) + ",0";

            var result = new List<CBLiJiaSyncLog>();
            var r = LiJiaBo.Instance.GetList(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            Util.ExcelUtil.Export(result);
        }

        #endregion

        #region 兴业嘉同步日志
        /// <summary>
        /// 兴业嘉同步日志列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2017-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult XingYeSyncLogList()
        {
            return View();
        }

        /// <summary>
        /// 兴业嘉同步日志列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>兴业嘉同步日志列表</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoXingYeSyncLogQuery(ParaXingYeSyncLogFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);
            filter.Warehouses = string.Join(",", warehouses) + ",0";
            var pager = XingYeBo.Instance.GetList(filter);
            return PartialView("_XingYeSyncLogListPager", pager.Map());
        }

        /// <summary>
        /// XingYe重新同步
        /// </summary>
        /// <param name="sysNo">XingYe同步日志表系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001108)]
        public ActionResult XingYeSyn(int sysNo)
        {
            Result result;
            var b = XingYeBo.Instance.IsRelate(sysNo);
            if (!b)
            {
                var client = Extra.Erp.XingYe.XingYeProviderFactory.CreateProvider();
                var isSave = CurrentUser.PrivilegeList.HasPrivilege(Model.SystemPredefined.PrivilegeCode.EAS1001110);
                var syncResult = client.Resynchronization(sysNo, isSave);
                result = new Result()
                {
                    Status = syncResult.Status,
                    Message = sysNo + " : " + syncResult.Message
                };
                //最新更新人
                var model = XingYeBo.Instance.GetEntity(sysNo);
                model.LastupdateBy = CurrentUser.Base.SysNo;
                XingYeBo.Instance.Update(model);

            }
            else
            {
                result = new Result()
                {
                    Status = false,
                    Message = "请先同步此单的历史关联单据,现在就去处理？"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  XingYe作废
        /// </summary>
        /// <param name="sysNo">XingYe同步日志表系统编号</param>
        /// <param name="remarks"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult InvalidXingYe(int sysNo, string remarks)
        {
            var model = XingYeBo.Instance.GetEntity(sysNo);
            model.Status = (int)Extra.Erp.Model.同步状态.作废;
            model.LastupdateDate = DateTime.Now;
            model.LastupdateBy = CurrentUser.Base.SysNo;
            model.Remarks = model.Remarks + ":" + remarks;
            var r = XingYeBo.Instance.Update(model);
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  XingYe批量作废
        /// </summary>
        /// <param name="sysNos">XingYe同步日志表系统编号数组</param>
        /// <param name="remarks">作废原因</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001109)]
        public ActionResult BatchInvalidXingYe(int[] sysNos, string remarks)
        {
            foreach (var sysNo in sysNos)
            {
                var model = XingYeBo.Instance.GetEntity(sysNo);
                if (model != null && (model.Status != (int)Extra.Erp.Model.同步状态.作废))
                {
                    if (model.Status == (int)Extra.Erp.Model.同步状态.成功) continue;
                    model.Status = (int)Extra.Erp.Model.同步状态.作废;
                    model.LastupdateDate = DateTime.Now;
                    model.LastupdateBy = CurrentUser.Base.SysNo;
                    model.Remarks = model.Remarks + ":" + remarks;
                    var r = XingYeBo.Instance.Update(model);
                }
            }
            var result = new Result()
            {
                Status = true,
                Message = "作废成功"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// XingYe同步日志数据
        /// </summary>
        /// <param name="sysNo">日志编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult XingYeDetailList(int sysNo)
        {
            ViewBag.Key = sysNo;
            return View();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sysNo">日志系统编号</param>
        /// <param name="id">当前页号</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>视图</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public ActionResult DoXingYeQuery(int sysNo, int id = 1, int pageSize = 10)
        {
            PagedList<object> list = null;
            var entity = XingYeBo.Instance.GetEntity(sysNo);
            if (entity != null && !string.IsNullOrEmpty(entity.Data))
            {
                var value = entity.Data.ToObject<SaleInfoWraper>();
                Pager<object> pager = XingYeBo.Instance.GetXingYeData(value, pageSize, id);
                list = new PagedList<object>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
                if (pager.Rows != null && pager.Rows.Count > 0)
                {
                    ViewBag.TableHead = GetTableHead(pager.Rows[0] as DataRow);
                }
            }
            return PartialView("_XingYeDetailListPager", list);
        }

        /// <summary>
        /// XingYe日志导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>空</returns>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        [Privilege(PrivilegeCode.EAS1001101)]
        public void ExportXingYeSyncLog(ParaXingYeSyncLogFilter filter)
        {
            var warehouses = CurrentUser.Warehouses.Select(x => x.SysNo);

            filter.Id = 1;
            filter.PageSize = 1048576;
            filter.Warehouses = string.Join(",", warehouses) + ",0";

            var result = new List<CBXingYeSyncLog>();
            var r = XingYeBo.Instance.GetList(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            Util.ExcelUtil.Export(result);
        }

        #endregion

        #region 系统配置功能 2014-01-20 周唐炬

        /// <summary>
        /// 系统配置功能
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="filter">条件</param>
        /// <returns>系统配置功能列表页</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.SY1012101)]
        public ActionResult SyConfigManage(int? id, ParaSyConfigFilter filter)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = SyConfigBo.Instance.GetList(filter);
                    return PartialView("_SyConfigList", list);
                }
                InitSyConfigPageViewData(null);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "系统配置功能" + ex.Message, LogStatus.系统日志目标类型.系统配置功能, CurrentUser.Base.SysNo, ex);
            }

            return View();
        }

        /// <summary>
        /// 创建系统配置功能
        /// </summary>
        /// <param></param>
        /// <returns>创建系统配置功能页</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.SY1012201)]
        public ActionResult SyConfigCreate()
        {
            InitSyConfigPageViewData(null);
            return View();
        }

        /// <summary>
        /// 新增系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1012201)]
        public JsonResult SyConfigCreate(SyConfig model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    var verify = SyConfigBo.Instance.SyConfigVerify(model.Key, model.CategoryId, null);
                    if (verify)
                    {
                        model.CreatedBy = model.LastUpdateBy = CurrentUser.Base.SysNo;
                        model.CreatedDate = model.LastUpdateDate = DateTime.Now;
                        var id = SyConfigBo.Instance.Create(model);
                        if (id > 0)
                        {
                            result.Status = true;
                            result.StatusCode = 0;
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "新增系统配置功能" + model.Key, LogStatus.系统日志目标类型.系统配置功能, id, CurrentUser.Base.SysNo);
                        }
                    }
                    else
                    {
                        result.Message = string.Format("{0}－系统配置功能已经存在！", model.Key);
                    }
                }
                else
                {
                    result.Message = "新增系统配置功能数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "新增系统配置功能" + ex.Message, LogStatus.系统日志目标类型.系统配置功能, CurrentUser.Base.SysNo, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 修改系统配置功能
        /// </summary>
        /// <param name="id">系统配置功能系统编号</param>
        /// <returns>修改系统配置功能页</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.SY1012202)]
        public ActionResult SyConfigEdit(int id)
        {
            var model = SyConfigBo.Instance.GetModel(id);
            InitSyConfigPageViewData(model);
            return View(model);
        }

        /// <summary>
        /// 修改系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1012202)]
        public JsonResult SyConfigEdit(SyConfig model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    var verify = SyConfigBo.Instance.SyConfigVerify(model.Key, model.CategoryId, model.SysNo);
                    if (verify)
                    {
                        var entity = SyConfigBo.Instance.GetModel(model.SysNo);
                        if (entity != null)
                        {
                            entity.Key = model.Key;
                            entity.Value = model.Value;
                            entity.CategoryId = model.CategoryId;
                            entity.Description = model.Description;
                            entity.LastUpdateDate = DateTime.Now;
                            entity.LastUpdateBy = CurrentUser.Base.SysNo;
                            var id = SyConfigBo.Instance.Update(entity);
                            if (id > 0)
                            {
                                result.Status = true;
                                result.StatusCode = 0;
                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改系统配置功能" + model.Key, LogStatus.系统日志目标类型.系统配置功能, model.SysNo, CurrentUser.Base.SysNo);
                            }
                        }
                        else
                        {
                            result.Message = "该系统配置不存在，请刷请页面重试！";
                        }
                    }
                    else
                    {
                        result.Message = string.Format("{0}－系统配置功能已经存在！", model.Key);
                    }
                }
                else
                {
                    result.Message = "系统配置数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改系统配置功能" + ex.Message, LogStatus.系统日志目标类型.系统配置功能, CurrentUser.Base.SysNo, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 删除系统配置项
        /// </summary>
        /// <param name="id">系统配置系统编号</param>
        /// <returns>结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.SY1012202)]
        public JsonResult SyConfigRemove(int id)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var rowsAffected = SyConfigBo.Instance.Remove(id);
                if (rowsAffected > 0)
                {
                    result.Status = true;
                    result.StatusCode = 0;
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改系统配置功能" + id, LogStatus.系统日志目标类型.系统配置功能, id, CurrentUser.Base.SysNo);
                }
                else
                {
                    result.Message = "删除出错，请刷新页面重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改系统配置功能" + ex.Message, LogStatus.系统日志目标类型.系统配置功能, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始页面数据
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns></returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        private void InitSyConfigPageViewData(SyConfig model)
        {
            var text = model != null ? "全部" : "请选择";
            var item = new SelectListItem() { Text = text, Value = "", Selected = true };
            var categorys = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<SystemStatus.系统配置类型>(ref categorys);
            ViewBag.Category = model != null ? new SelectList(categorys, "Value", "Text", model.CategoryId) : new SelectList(categorys, "Value", "Text");
        }
        #endregion
        #region 订单自动处理配置
        /// <summary>
        /// 订单自动处理配置
        /// </summary>
        /// <returns>订单自动处理配置页面</returns>
        /// <remarks>2014-09-03 陈俊</remarks>
        [Privilege(PrivilegeCode.SY1012203)]
        public ActionResult OrderAutomaticProcessing()
        {
            var key = "global";
            var configModel = BLL.Sys.SyConfigBo.Instance.GetModel(key, Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自动处理配置);
            return View(configModel);
        }
        /// <summary>
        /// 保存订单自动处理配置信息
        /// </summary>
        /// <returns>保存结果</returns>
        /// <remarks>2014-09-03 陈俊</remarks>
        [Privilege(PrivilegeCode.SY1012203)]
        public JsonResult SaveOrderAutomaticProcessing(SyConfig model)
        {
            var key = "global";
            var res = new Result();
            if (string.IsNullOrEmpty(model.Value))
            {
                res.Message = "配置信息不能为空!";
            }
            else
            {
                try
                {
                    var result = Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(model.Value);
                    //先判断syconfig是否已有该记录.key("global")、系统配置类型(系统配置类型.升舱订单自动处理配置)
                    var configModel = BLL.Sys.SyConfigBo.Instance.GetModel(key, Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自动处理配置);
                    //有记录就执行update。没有insert  
                    if (configModel == null)
                    {
                        model.CategoryId = (int)SystemStatus.系统配置类型.升舱订单自动处理配置;
                        model.Key = key;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = CurrentUser.Base.SysNo;
                        model.LastUpdateDate =DateTime.Now;
         
                        var insertResult = BLL.Sys.SyConfigBo.Instance.Create(model);
                    }
                    if (configModel != null)
                    {
                        configModel.Value = model.Value;
                        configModel.LastUpdateBy = CurrentUser.Base.SysNo;
                        configModel.LastUpdateDate = DateTime.Now;
                        var updataResult = BLL.Sys.SyConfigBo.Instance.Update(configModel);
                    }
                    //清除缓存
                    Hyt.BLL.OrderRule.OrderEngine.Instance.Clear();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    //return  Content("未知原因，保存失败！");
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取自定义的命令
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SY1012203)]
        public ContentResult SelectVar()
        {
            var list = BLL.Sys.SyConfigBo.Instance.GetList(new ParaSyConfigFilter()
            {
                CategoryId = (int)Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自定义命令,
                CurrentPage = 1,
                PageSize = 10
            });

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(
                (from config in list.TData
                 select new
                 {
                     Key = config.Key,
                     Value = config.Value
                 }).ToList()
             ));
        }

        /// <summary>
        /// 保存自定义的命令
        /// </summary>
        /// <returns>ContentResult</returns>
        /// <remarks>2014-09-26 余勇</remarks>
        [Privilege(PrivilegeCode.SY1012203)]
        public ContentResult SaveVar(string key, string value)
        {
            key = key.Trim();
            var message = "保存成功！";
            if (string.IsNullOrEmpty(value))
            {
                message = "配置信息不能为空!";
            }
            else
            {
                try
                {
                    Hyt.BLL.OrderRule.OrderEngine.Instance.ParseCommand(value);
                    //先判断syconfig是否已有该记录.key("global")、系统配置类型(系统配置类型.升舱订单自动处理配置)
                    var configModel = BLL.Sys.SyConfigBo.Instance.GetModel(key, Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自定义命令);
                    //有记录就执行update。没有insert  
                    if (configModel == null)
                    {
                        var model = new SyConfig();
                        model.CategoryId = (int)SystemStatus.系统配置类型.升舱订单自定义命令;
                        model.Key = key;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = CurrentUser.Base.SysNo;
                        var insertResult = BLL.Sys.SyConfigBo.Instance.Create(model);
                    }
                    if (configModel != null)
                    {
                        configModel.Value = value;
                        configModel.LastUpdateBy = CurrentUser.Base.SysNo;
                        configModel.LastUpdateDate = DateTime.Now;
                        var updataResult = BLL.Sys.SyConfigBo.Instance.Update(configModel);
                    }
                    //清除缓存
                    MemoryProvider.Default.Remove(string.Format(KeyConstant.SysConfigInfo, key));
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return Content(message);
        }

        /// <summary>
        /// 获取命令的配置
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SY1012203)]
        public ContentResult GetVar(string key)
        {
            var configModel = MemoryProvider.Default.Get(string.Format(KeyConstant.SysConfigInfo, key), () => BLL.Sys.SyConfigBo.Instance.GetModel(key, Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自定义命令));

            if (configModel == null) return Content("");

            return Content(configModel.Value);

        }



        #endregion


        #region 第三方用户关联

        /// <summary>
        /// 第三方用户关联列表
        /// </summary>
        /// <returns></returns>
        /// 2018-1-5 吴琨 创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult ThirdPartyUser(ParaPrPurchaseFilter para)
        {
            if (Request.IsAjaxRequest())
            {
                PagedList<SyKingdeeUser> pageList = null;
                para.Id = para.Id > 0 ? para.Id : 1;
                var pager = ISyKingdeeUserDao.Instance.GetPages(para);
                if (pager != null)
                {
                    pageList = new PagedList<SyKingdeeUser>();
                    pageList.TData = pager.Rows;
                    pageList.TotalItemCount = pager.TotalRows;
                    pageList.CurrentPageIndex = para.Id;
                }
                return PartialView("_ThirdPartyUser", pageList);
            }
            return View();
        }

        /// <summary>
        /// 创建第三方用户关联
        /// </summary>
        /// <returns></returns>
        /// 2018-1-5 吴琨 创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult AddThirdPartyUser(SyKingdeeUser models)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0,
                Message = "失败"
            };
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<第三方用户类别>(null, null).ToString());
            if (Request.IsAjaxRequest())
            {
                if (models.SysNo > 0)
                {//修改
                    if (ISyKingdeeUserDao.Instance.UpModels(models))
                    {
                        result.Status = true;
                        result.StatusCode = 1;
                        result.Message = "操作成功";
                    }
                }
                else
                {
                    if (ISyKingdeeUserDao.Instance.AddModels(models))
                    {
                        result.Status = true;
                        result.StatusCode = 1;
                        result.Message = "操作成功";
                    }
                }

                return Json(result);
            }

            if (models.SysNo > 0)
            {//修改进入 
                models = ISyKingdeeUserDao.Instance.GetModels(models.SysNo);
            }
            return View(models == null ? new SyKingdeeUser() : models);
        }



        /// <summary>
        /// 创建第三方用户关联
        /// </summary>
        /// <returns></returns>
        /// 2018-1-5 吴琨 创建
        [HttpPost]
        [Privilege(PrivilegeCode.None)]
        public ActionResult DelThirdPartyUser(int SysNo)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0,
                Message = "失败"
            };
            if (ISyKingdeeUserDao.Instance.delModels(SysNo))
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "操作成功";
            }
            return Json(result);

        }


        #endregion
    }

}