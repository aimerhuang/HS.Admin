using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.DataAccess.Sys;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Manual;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 用户组管理
    /// </summary>
    /// <remarks>2013-07-30 朱成果 创建</remarks>
    public class SyUserGroupBo : BOBase<SyUserGroupBo>
    {
        /// <summary>
        /// 判断当前用户是否拥有所有仓库权限
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>true:拥有所有仓库;false:不拥有所有仓库</returns>
        /// <remarks>2013-07-04 吴文强 创建</remarks>
        public bool IsHasAllWarehouse(int userSysNo)
        {
           //return MemoryProvider.Default.Get(string.Format(KeyConstant.HasAllWarehouse, userSysNo),30, () => ISyGroupUserDao.Instance.GroupContainsUser(UserGroup.包含所有仓库的用户组, userSysNo));
            return GroupContainsUser(UserGroup.包含所有仓库的用户组, userSysNo);
        }
        /// <summary>
        /// 判断当前用户是否拥有所有分销商权限
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>true:拥有所有分销商;false:不拥有所有分销商</returns>
        /// <remarks>2015-10-04 王耀发 创建</remarks>
        public bool IsHasAllDealer(int userSysNo)
        {
            //return MemoryProvider.Default.Get(string.Format(KeyConstant.HasAllWarehouse, userSysNo),30, () => ISyGroupUserDao.Instance.GroupContainsUser(UserGroup.包含所有仓库的用户组, userSysNo));
            return GroupContainsUser(UserGroup.包含所有分销商的用户组, userSysNo);
        }
        /// <summary>
        /// 判断当前用户是否拥有代理商权限
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>true:拥有代理商;false:不拥有代理商</returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public bool IsHasAgentGroup(int userSysNo)
        {
            return GroupContainsUser(UserGroup.包含代理商的用户组, userSysNo);
        }
        /// <summary>
        /// 判断当前用户是否拥有分销商权限
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>true:拥有分销商;false:不拥有分销商</returns>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public bool IsHasDealerGroup(int userSysNo)
        {
            return GroupContainsUser(UserGroup.包含分销商的用户组, userSysNo);
        }
        /// <summary>
        /// 组是否包含该用户 （对数据修改之后5分钟生效，因为缓存保存5分钟）
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <param name="userSysNo"></param>
        /// <returns>
        /// 2014-5-13 杨文兵 创建 
        /// </returns>
        public bool GroupContainsUser(int groupSysNo, int userSysNo) {
            var cacheKey = string.Format("CACHE_GROUPUSER_{0}_{1}", groupSysNo, userSysNo);

            return MemoryProvider.Default.Get<bool>(cacheKey, 5, () =>
            {
                return ISyGroupUserDao.Instance.GroupContainsUser(groupSysNo, userSysNo);
            }, CachePolicy.Absolute);
        }


        /// <summary>
        /// 插入,更新用户组数据
        /// </summary>
        /// <param name="userSysNO">用户编号</param>
        /// <param name="lst">用户组对应关系列表</param>
        /// <param name="currectUser">当前操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 朱成果 创建</remarks>
        public void SetSyGroupUserList(int userSysNO, List<SyGroupUser> lst, SyUser currectUser)
        {
            if (lst == null && lst.Count < 1)
            {
                throw new Exception("用户组不能为空");
            }
            Hyt.DataAccess.Sys.ISyGroupUserDao.Instance.DeleteByUserSysNo(userSysNO);
            foreach (SyGroupUser item in lst)
            {
                item.UserSysNo = userSysNO;
                if (item.GroupSysNo < 1)
                {
                    throw new Exception("未能获取到用户组编号");
                }
                item.CreatedBy = currectUser.CreatedBy;
                item.CreatedDate = DateTime.Now;
                item.LastUpdateBy = currectUser.CreatedBy;
                item.LastUpdateDate = DateTime.Now;
                Hyt.DataAccess.Sys.ISyGroupUserDao.Instance.Insert(item);
            }
        }

        /// <summary>
        /// 获取用户组数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public SyUserGroup GetEntity(int sysNo)
        {
            return Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 根据用户组状态获取用户组，不传参获取所有用户组
        /// </summary>
        /// <param name="status">用户组状态</param>
        /// <returns>用户组列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public IList<SyUserGroup> GetSyGroupByStatus(int? status)
        {
            var list = DataAccess.Sys.ISyUserGroupDao.Instance.GetAllSyGroup();  
            if (list != null && list.Count > 0 && status.HasValue)
            {
                var r = list.Where(item => item.Status == status);
                if (r.Count() > 0) return r.ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-05 朱成果 创建</remarks>
        public IList<SyUserGroup> GetList()
        {
            return DataAccess.Sys.ISyUserGroupDao.Instance.GetAllSyGroup();
        }

        /// <summary>
        ///根据用户组名获取用户组
        /// </summary>
        /// <param name="groupName">用户组名</param>
        /// <returns></returns>
        /// <remarks>2013-08-05  朱成果 创建</remarks>
        public SyUserGroup GetByGroupName(string groupName)
        {
            return Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.GetByGroupName(groupName);
        }

        /// <summary>
        /// 检查用户组是否在使用
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2013-08-05  朱成果 创建</remarks>
        public bool IsBeingUsed(int sysNo)
        {
            return Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.IsBeingUsed(sysNo);
        }

        #region 用户组对应菜单，权限

        /// <summary>
        /// 获取用户组对应菜单
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-02 朱成果 创建</remarks>
        public List<SyUserGroupMenu> GetUserGroupMenu(int userGroupID)
        {
            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组, userGroupID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.菜单);
            if (lst != null)
            {
                return lst.Select(m => new SyUserGroupMenu { MenuID = m.TargetSysNo, UserGroupID = userGroupID, MenuType = 0 }).ToList();
            }
            else
            {
                return new List<SyUserGroupMenu>();
            }
        }

        /// <summary>
        /// 获取用户组对应权限
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-02 朱成果 创建</remarks>
        public List<SyUserGroupMenu> GetUserGroupPermission(int userGroupID)
        {

            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组, userGroupID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.权限);
            if (lst != null)
            {
                return lst.Select(m => new SyUserGroupMenu { MenuID = m.TargetSysNo, UserGroupID = userGroupID, MenuType = 1 }).ToList();
            }
            else
            {
                return new List<SyUserGroupMenu>();
            }
        }

        /// <summary>
        /// 生成树形
        /// </summary>
        /// <param name="pmeunID">上级菜单编号</param>
        /// <param name="lst">所有菜单</param>
        /// <param name="lstM">用户组对应的菜单</param>
        /// <param name="lstP">用户组对应的权限</param>
        /// <param name="lstMP">所有菜单权限</param>
        /// <param name="lstResult">结果</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        private void BuildTree(int pmeunID, IList<SyMenu> lst, List<SyUserGroupMenu> lstM, List<SyUserGroupMenu> lstP, IList<CBSyPrivilege> lstMP, List<ZCheckTreeNode> lstResult)
        {
            List<SyMenu> sublist = lst.Where(m => m.ParentSysNo == pmeunID && m.Status == (int)Hyt.Model.WorkflowStatus.SystemStatus.菜单状态.启用).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                foreach (SyMenu s in sublist)
                {
                    //添加子菜单
                    lstResult.Add(new ZCheckTreeNode
                    {
                        id = "m_" + s.SysNo,
                        name = s.MenuName,
                        nodetype = 0,
                        open = true,
                        @checked = lstM.Any(m => m.MenuID == s.SysNo),
                        pId = "m_" + s.ParentSysNo
                    });
                    BuildTree(s.SysNo, lst, lstM, lstP, lstMP, lstResult);
                }
            }

            //检查菜单下面的权限
            if (lstMP != null && lstMP.Count > 0)
            {
                List<CBSyPrivilege> childlist = lstMP.Where(m => m.MenuSysNo == pmeunID).ToList();
                if (childlist != null && childlist.Count > 0)
                {
                    foreach (CBSyPrivilege c in childlist)
                    {
                        lstResult.Add(new ZCheckTreeNode
                        {
                            id = "p_" + c.SysNo,
                            name = c.Name,
                            nodetype = 1,
                            open = true,
                            @checked = lstP.Any(m => m.MenuID == c.SysNo),
                            pId = "m_" + pmeunID,
                            icon = "/Theme/images/icons/operate.png",
                        });
                    }
                }
            }

        }

        /// <summary>
        /// 获取用户组菜单权限树
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        public List<ZCheckTreeNode> GetTreeListByUserGroup(int userGroupID)
        {
            List<ZCheckTreeNode> lstResult = new List<ZCheckTreeNode>();
            var lstM = GetUserGroupMenu(userGroupID);//用户组对应菜单
            var lstP = GetUserGroupPermission(userGroupID);//用户组对应权限
            IList<SyMenu> lst = Hyt.DataAccess.Sys.ISyMenuDao.Instance.GetAll();//所有菜单
            IList<CBSyPrivilege> lstMP = Hyt.DataAccess.Sys.ISyPrivilegeDao.Instance.GetMenuPrivilege();//所有菜单权限
            if (lst != null)
            {
                //子菜单
                BuildTree(0, lst, lstM, lstP, lstMP, lstResult);
            }
            return lstResult;
        }

        /// <summary>
        /// 获取用户组对应的角色
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns>用户组对应的角色列表</returns>
        /// <remarks>2013-08-02 朱成果 创建</remarks>
        public List<SyUserGroupRole> GetUserGroupRole(int userGroupID)
        {
            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组, userGroupID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.角色);
            if (lst != null)
            {
                return lst.Select(m => new SyUserGroupRole { RoleID = m.TargetSysNo, UserGroupID = userGroupID }).ToList();
            }
            else
            {
                return new List<SyUserGroupRole>();
            }
        }

        #endregion
        #region 用户组新建，更新,删除,启用禁用
        /// <summary>
        /// 用户组
        /// </summary>
        /// <param name="model">用户组</param>
        /// <param name="groupMeuns">菜单权限</param>
        /// <param name="groupRoles">角色</param>
        /// <returns></returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        public void SaveUserGroup(SyUserGroup model, List<SyUserGroupMenu> groupMeuns, List<SyUserGroupRole> groupRoles)
        {
            if (model.SysNo < 1)//新建
            {
                #region 新建
                if (GetByGroupName(model.GroupName) != null)
                {
                    throw new Exception("用户组名已经存在");

                }
                model.SysNo = Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.Insert(model);//添加用户组
                if (groupMeuns != null && model.SysNo > 0)//添加菜单权限
                {
                    foreach (SyUserGroupMenu item in groupMeuns)
                    {
                        var p = new SyPermission
                                            {
                                                CreatedBy = model.CreatedBy,
                                                CreatedDate = DateTime.Now,
                                                EffectiveDate = DateTime.Now,
                                                ExpirationDate = DateTime.MaxValue,
                                                LastUpdateBy = model.CreatedBy,
                                                LastUpdateDate = DateTime.Now,
                                                Source = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组,
                                                SourceSysNo = model.SysNo,
                                                Target = item.MenuType == 1 ? (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.权限 : (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.菜单,
                                                TargetSysNo = item.MenuID
                                            };
                        Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Insert(p);
                    }
                }
                if (groupRoles != null && model.SysNo > 0)//添加角色
                {
                    foreach (SyUserGroupRole item in groupRoles)
                    {
                        var r = new SyPermission
                        {
                            CreatedBy = model.CreatedBy,
                            CreatedDate = DateTime.Now,
                            EffectiveDate = DateTime.Now,
                            ExpirationDate = DateTime.MaxValue,
                            LastUpdateBy = model.CreatedBy,
                            LastUpdateDate = DateTime.Now,
                            Source = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组,
                            SourceSysNo = model.SysNo,
                            Target = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.角色,
                            TargetSysNo = item.RoleID
                        };
                        Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Insert(r);
                    }
                }
                #endregion
            }
            else //编辑
            {
                #region 编辑
                Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.Update(model);
                Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Delete((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组, model.SysNo);
                if (groupMeuns != null && model.SysNo > 0)//添加菜单权限
                {
                    foreach (SyUserGroupMenu item in groupMeuns)
                    {
                        var p = new SyPermission
                        {
                            CreatedBy = model.CreatedBy,
                            CreatedDate = DateTime.Now,
                            EffectiveDate = DateTime.Now,
                            ExpirationDate =(DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue,// DateTime.MaxValue,
                            LastUpdateBy = model.CreatedBy,
                            LastUpdateDate = DateTime.Now,
                            Source = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组,
                            SourceSysNo = model.SysNo,
                            Target = item.MenuType == 1 ? (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.权限 : (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.菜单,
                            TargetSysNo = item.MenuID
                        };
                        Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Insert(p);
                    }
                }
                if (groupRoles != null && model.SysNo > 0)//添加角色
                {
                    foreach (SyUserGroupRole item in groupRoles)
                    {
                        var r = new SyPermission
                        {
                            CreatedBy = model.CreatedBy,
                            CreatedDate = DateTime.Now,
                            EffectiveDate = DateTime.Now,
                            ExpirationDate =(DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue,// DateTime.MaxValue,
                            LastUpdateBy = model.CreatedBy,
                            LastUpdateDate = DateTime.Now,
                            Source = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组,
                            SourceSysNo = model.SysNo,
                            Target = (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.角色,
                            TargetSysNo = item.RoleID
                        };
                        Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Insert(r);
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="sysNo">用户组编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks>
        public void DeleteUserGroup(int sysNo, SyUser user)
        {
            if (IsBeingUsed(sysNo))
            {
                throw new Exception("当前用户组已经包括用户数据，不能删除");
            }
            Hyt.DataAccess.Sys.ISyPermissionDao.Instance.Delete((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组, sysNo);
            Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 启用，禁用用户组
        /// </summary>
        /// <param name="sysNo">用户组编号</param>
        /// <param name="disabled">true 禁用 false 启用</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013－08-05 朱成果 创建</remarks> 
        public void DisabledUserGroup(int sysNo, bool disabled, SyUser user)
        {
            var entity = GetEntity(sysNo);
            if (entity == null)
            {
                throw new Exception("用户组不存在");
            }
            entity.Status = disabled ? (int)Hyt.Model.WorkflowStatus.SystemStatus.用户组状态.禁用 : (int)Hyt.Model.WorkflowStatus.SystemStatus.用户组状态.启用;
            entity.LastUpdateBy = user.SysNo;
            entity.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Sys.ISyUserGroupDao.Instance.Update(entity);
        }
        #endregion

        /// <summary>
        /// 向指定用户组添加所有的菜单权限
        /// </summary>
        /// <param name="userGroupSysNo">用户组编号</param>
        /// <returns>void</returns>
        /// <remarks>2013－10-11 朱家宏 创建</remarks> 
        public void AssignAllMenuPrivilegeToUserGroup(int userGroupSysNo)
        {
            var userGroup = DataAccess.Sys.ISyUserGroupDao.Instance.GetEntity(userGroupSysNo);
            var groupMeuns = new List<SyUserGroupMenu>();

            var menus = DataAccess.Sys.ISyMenuDao.Instance.GetAll();

            foreach (var menu in menus)
            {
                groupMeuns.Add(new SyUserGroupMenu()
                {
                    UserGroupID = userGroup.SysNo,
                    MenuID = menu.SysNo,
                    MenuType = 0
                });

                var privileges = DataAccess.Sys.ISyPrivilegeDao.Instance.GetListByMenu(menu.SysNo);
                foreach (var p in privileges)
                {
                    groupMeuns.Add(new SyUserGroupMenu()
                    {
                        UserGroupID = userGroup.SysNo,
                        MenuID = p.SysNo,
                        MenuType = 1
                    });
                }
            }

            DataAccess.Sys.ISyPermissionDao.Instance.Delete((int)SystemStatus.授权来源.用户组, userGroup.SysNo);

            if (userGroup.SysNo > 0)//添加菜单权限
            {
                foreach (SyUserGroupMenu item in groupMeuns)
                {
                    var p = new SyPermission
                    {
                        CreatedBy = userGroup.CreatedBy,
                        CreatedDate = DateTime.Now,
                        EffectiveDate = DateTime.Now,
                        ExpirationDate = DateTime.MaxValue,
                        LastUpdateBy = userGroup.CreatedBy,
                        LastUpdateDate = DateTime.Now,
                        Source = (int)SystemStatus.授权来源.用户组,
                        SourceSysNo = userGroup.SysNo,
                        Target = item.MenuType == 1 ? (int)SystemStatus.授权目标.权限 : (int)SystemStatus.授权目标.菜单,
                        TargetSysNo = item.MenuID
                    };
                    DataAccess.Sys.ISyPermissionDao.Instance.Insert(p);
                }
            }
        }
    }
}
