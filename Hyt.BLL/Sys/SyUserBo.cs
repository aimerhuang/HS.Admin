using Hyt.DataAccess.Sys;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Infrastructure.Memory;
using Hyt.Infrastructure.Caching;


namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 系统用户
    /// </summary>
    /// <remarks></remarks>
    public class SyUserBo : BOBase<SyUserBo>
    {
        #region 用户
        /// <summary>
        /// 创建系统用户
        /// </summary>
        /// <param name="syUser">系统用户</param>
        /// <returns>返回SysNo</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public int InsertSyUser(SyUser syUser)
        {
            syUser.SysNo = ISyUserDao.Instance.InsertSyUser(syUser);
            return syUser.SysNo;
        }

        /// <summary>
        /// 更新系统用户
        /// </summary>
        /// <param name="syUser">系统用户</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-08  黄志勇 创建
        /// 2014-5-13 杨文兵 增加清除缓存代码
        /// </remarks>
        public void UpdateSyUser(SyUser syUser)
        {
            ISyUserDao.Instance.UpdateSyUser(syUser);            
            var cacheKey = string.Format("CACHE_SYUSER_{0}", syUser.SysNo);
            MemoryProvider.Default.Remove(cacheKey);
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2013-08-12 黄志勇 Crearte</remarks>
        public void UpdateSyUserStatus(int sysNo, int status)
        {
            ISyUserDao.Instance.UpdateSyUserStatus(sysNo, status);
            var cacheKey = string.Format("CACHE_SYUSER_{0}", sysNo);
            MemoryProvider.Default.Remove(cacheKey);
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>新密码(6位数)</returns>
        /// <remarks>2013-10-23 朱家宏 创建</remarks>
        public string ResetUserPassword(int userSysNo)
        {
            var newPass = Util.WebUtil.Number(6, false); //随机6位数字
            var user = SyUserBo.Instance.GetSyUser(userSysNo);
            user.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt(newPass);
            SyUserBo.Instance.UpdateSyUser(user);
            return newPass;
        }

        #endregion

        #region 授权
        /// <summary>
        /// 创建授权
        /// </summary>
        /// <param name="syPermission">授权</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public void InsertSyPermission(SyPermission syPermission)
        {
            syPermission.SysNo = ISyPermissionDao.Instance.Insert(syPermission);
        }

        /// <summary>
        /// 删除授权
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">>目标:菜单(10),角色(20),权限(30)</param>
        /// <param name="targetSysNo">目标编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public void DelSyPermission(int source, int sourceSysNo, int target, int targetSysNo)
        {
            ISyPermissionDao.Instance.Delete(source, sourceSysNo, target, targetSysNo);
        }
        #endregion

        #region 仓库
        /// <summary>
        /// 插入用户仓库权限
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  黄志勇 创建</remarks>
        public int InsertSyUserWarehouse(SyUserWarehouse entity)
        {
            return ISyUserDao.Instance.InsertSyUserWarehouse(entity);
        }

        /// <summary>
        /// 删除用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>受影响条数</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public int DeleteSyUserWarehouse(int userSysNo, int warehouseSysNo)
        {
            return ISyUserDao.Instance.DeleteSyUserWarehouse(userSysNo, warehouseSysNo);
        }

        /// <summary>
        /// 判断是否存在用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>true\false</returns>
        /// <remarks>2013-10-31  朱成果 创建</remarks>
        public bool ExistsSyUserWarehouse(int userSysNo, int warehouseSysNo)
        {
            return ISyUserDao.Instance.ExistsSyUserWarehouse(userSysNo, warehouseSysNo);
        }
        #endregion

        #region 用户登录，获取用户信息
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <returns>返回登录结果</returns>
        /// <remarks>2013-06-19 周唐炬 Crearte</remarks>
        public Result<SyUser> Login(string account, string password)
        {
            var result = new Result<SyUser>() { StatusCode = -1 };
            try
            {
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
                {
                    result.Message = "用户账号或用户密码不能为空!";
                }
                else
                {
                    var syUser = ISyUserDao.Instance.GetSyUser(account, password);
                    if (syUser != null)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                        result.Message = string.Format("{0}登录成功", account);
                        result.Data = syUser;
                    }
                    else
                    {
                        result.Message = "用户不存在或者密码错误!";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="oldPassword">旧密码(加密后)</param>
        /// <param name="newPassword">新密码(加密后)</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-19 周唐炬 修改</remarks>
        /// <remarks>2013-11-5 黄志勇 修改</remarks>
        public Result ModifyPassword(string account, string oldPassword, string newPassword)
        {
            var result = new Result() { StatusCode = -1, Status = false };
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    result.StatusCode = -2;
                    result.Message = "新密码不能为空";
                }
                else
                {
                    var user = ISyUserDao.Instance.GetSyUser(account);
                    if (user != null)
                    {
                        if (EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(oldPassword, user.Password))
                        {
                            user.Password = EncryptionUtil.EncryptWithMd5AndSalt(newPassword);
                            SyUserBo.Instance.UpdateSyUser(user);                            
                            result.StatusCode = 0;
                            result.Status = true;
                            result.Message = string.Format("{0}密码修改成功!", user.Account);
                            
                        }
                        else
                        {
                            result.StatusCode = -3;
                            result.Message = "旧密码错误!";
                        }
                    }
                    else
                    {
                        result.StatusCode = -4;
                        result.Message = "用户不存在!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 通过SysNo获取用户信息
        /// </summary>
        /// <param name="sysNo">用户系统编号</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-06-19 周唐炬 修改
        /// 2014-5-13 杨文兵 增加缓存调用
        /// </remarks>
        public SyUser GetSyUser(int sysNo)
        {
            var cacheKey = string.Format("CACHE_SYUSER_{0}", sysNo);
            return MemoryProvider.Default.Get<SyUser>(cacheKey, 60, () =>
            {
                return ISyUserDao.Instance.GetSyUser(sysNo);
            }, CachePolicy.Absolute);

            //return ISyUserDao.Instance.GetSyUser(sysNo);
        }

        /// <summary>
        /// 通过系统编号获取系统用户姓名
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>系统用户姓名</returns>
        /// <remarks>2013-06-28 吴文强 创建</remarks>
        public string GetUserName(int sysNo)
        {
            var user = GetSyUser(sysNo);
            return user == null ? "系统执行" : user.UserName;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-12-31 黄伟 创建
        /// </remarks>
        public List<SyUser> GetSyUsersByName(string userName)
        {
            return ISyUserDao.Instance.GetSyUsersByName(userName);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        /// <remarks></remarks>
        public SyUser GetSyUser(string account, string password)
        {
            return ISyUserDao.Instance.GetSyUser(account, password);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns>用户信息</returns>
        /// <remarks></remarks>
        public SyUser GetSyUser(string account)
        {
            //ZCheckTreeNode xx = new ZCheckTreeNode();
            return ISyUserDao.Instance.GetSyUser(account);
        }

        /// <summary>
        /// 系统用户分页列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>用户分页列表</returns>
        /// <remarks>
        /// 2013-08-05 黄志勇 创建
        /// 2016-03-23 陈海裕 修改 若用户为代理商，则只能看到自身及其创建的用户
        /// </remarks>
        public Pager<CBSyUser> GetSyUser(ParaSyUserFilter filter)
        {
            if (Authentication.AdminAuthenticationBo.Instance.Current.IsAgent && !Authentication.AdminAuthenticationBo.Instance.Current.IsBindAllDealer )
            {
                filter.CreatedBy = Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            }
            var pager = ISyUserDao.Instance.GetSyUser(filter);
            return pager;
        }

        /// <summary>
        /// 根据用户编号获取用户分组
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>用户分组列表</returns>
        /// <remarks>2013-08-06  黄志勇 创建</remarks>
        public IList<SyGroupUser> GetGroupUser(int userSysNo)
        {
            return ISyGroupUserDao.Instance.GetGroupUser(userSysNo);
        }

        /// <summary>
        /// 通过用户组取用户列表
        /// </summary>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-11-27 余勇 创建
        /// </remarks>
        public IList<SyUser> GetSyUserByGroupSysNo(int groupSysNo)
        {
            return ISyUserDao.Instance.GetSyUserByGroupSysNo(groupSysNo);
        }

        /// <summary>
        /// 读取出去指定列表的其余有效用户
        /// </summary>
        /// <param name="users">排除用户系统编号列表</param>
        /// <returns>系统用户列表</returns>
        /// <remarks>2014-03-05  邵斌 创建</remarks>
        public IList<SyUser> GetUserListWithoutSysNoList(IList<int> users)
        {
            return ISyUserDao.Instance.GetUserListWithoutSysNoList(users);
        }

        #endregion

        #region 用户菜单,权限
        /// <summary>
        /// 获取用户菜单权限树
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        public List<ZCheckTreeNode> GetTreeListByUser(int userID)
        {
            List<ZCheckTreeNode> lstResult = new List<ZCheckTreeNode>();
            var lstM = GetUserMenu(userID);//用户对于菜单
            var lstP = GetUserPermission(userID);//用户对于权限
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
        /// 生成树形
        /// </summary>
        /// <param name="pmeunID">上级菜单编号</param>
        /// <param name="lst">所有菜单</param>
        /// <param name="lstM">用户对应的菜单</param>
        /// <param name="lstP">用户对应的权限</param>
        /// <param name="lstMP">所有菜单权限</param>
        /// <param name="lstResult">结果</param>
        /// <returns>true\false</returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        private void BuildTree(int pmeunID, IList<SyMenu> lst, List<SyUserMenu> lstM, List<SyUserMenu> lstP, IList<CBSyPrivilege> lstMP, List<ZCheckTreeNode> lstResult)
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
        /// 获取用户对应菜单
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        public List<SyUserMenu> GetUserMenu(int userID)
        {
            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.系统用户, userID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.菜单);
            if (lst != null)
            {
                return lst.Select(m => new SyUserMenu { MenuID = m.TargetSysNo, UserID = userID, MenuType = 0 }).ToList();
            }
            else
            {
                return new List<SyUserMenu>();
            }
        }

        /// <summary>
        /// 获取用户对应权限
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        public List<SyUserMenu> GetUserPermission(int userID)
        {
            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.系统用户, userID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.权限);
            if (lst != null)
            {
                return lst.Select(m => new SyUserMenu { MenuID = m.TargetSysNo, UserID = userID, MenuType = 1 }).ToList();
            }
            else
            {
                return new List<SyUserMenu>();
            }
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>用户角色列表</returns>
        /// <remarks>2013-08-02 朱成果 创建</remarks>
        public List<SyUserRole> GetUserRole(int userID)
        {
            List<SyPermission> lst = Hyt.DataAccess.Sys.ISyPermissionDao.Instance.GetList((int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.系统用户, userID, (int)Hyt.Model.WorkflowStatus.SystemStatus.授权目标.角色);
            if (lst != null)
            {
                return lst.Select(m => new SyUserRole { RoleID = m.TargetSysNo, UserID = userID }).ToList();
            }
            else
            {
                return new List<SyUserRole>();
            }
        }
        #endregion



        public SyUser GetSyUserByOpenId(string openId)
        {
            return ISyUserDao.Instance.GetSyUserByOpenId(openId);
        }


        /// <summary>
        /// 模糊查询姓名、注册账号、手机
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public  List<SyUser> GetUtilLike(string keyWord)
        {
            return ISyUserDao.Instance.GetUtilLike(keyWord);
        }
    }
}
