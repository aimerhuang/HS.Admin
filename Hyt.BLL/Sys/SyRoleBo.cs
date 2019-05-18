using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Manual;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>2013-08-05 朱成果 创建</remarks>
    public class SyRoleBo : BOBase<SyRoleBo>
    {
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns>所有角色列表</returns>
        /// <remarks>2013-08-05 朱成果 创建</remarks>
        public IList<SyRole> GetList()
        {
            return Hyt.DataAccess.Sys.ISyRoleDao.Instance.SelectAll();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="sysNo">角色编号</param>
        /// <returns>角色实体</returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public SyRole Get(int sysNo)
        {
            return Hyt.DataAccess.Sys.ISyRoleDao.Instance.Select(sysNo);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="stat">状态</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Pager<SyRole> GetPagerList(int? stat, int currentPage, int pageSize)
        {
            return Hyt.DataAccess.Sys.ISyRoleDao.Instance.Query(stat, currentPage, pageSize);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">角色编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Result Delete(int sysNo)
        {
            Result res = new Result();
            var lstM = GetRoleMenu(sysNo);
            var lstP = GetRolePermission(sysNo);//角色对应权限
            if (lstM.Count > 0 || lstP.Count > 0)
            {
                throw new Exception("该角色已分配菜单权限不能删除");
            }
            var r = Hyt.DataAccess.Sys.ISyRoleDao.Instance.Delete(sysNo);
            if (r) res.Status = true;
            return res;
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <param name="groupMeuns">菜单权限组</param>
        /// <param name="userId">用户编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Result Save(SyRole model, List<SyRoleMenuPrivilege> groupMeuns,int userId)
        {
            var res = new Result();
            if (GetByRoleName(model.RoleName, model.SysNo) != null)
            {
                throw new Exception("角色名已经存在");
            }
           
            if (model.SysNo > 0)
            {
                #region 修改

                model.LastUpdateBy = userId;
                model.LastUpdateDate = DateTime.Now;
                Hyt.DataAccess.Sys.ISyRoleDao.Instance.Update(model); //修改
                if (model.SysNo > 0) //添加菜单权限
                {
                    Hyt.DataAccess.Sys.ISyRoleMenuDao.Instance.DeleteByRoleSysNo(model.SysNo);
                    Hyt.DataAccess.Sys.ISyRolePrivilegeDao.Instance.DeleteByRoleSysNo(model.SysNo);
                    if (groupMeuns != null)
                    {
                        foreach (SyRoleMenuPrivilege item in groupMeuns)
                        {
                            if (item.MenuType == 0)
                            {
                                var m = new SyRoleMenu
                                    {
                                        CreatedBy = model.CreatedBy,
                                        CreatedDate = DateTime.Now,
                                        LastUpdateBy = model.CreatedBy,
                                        LastUpdateDate = DateTime.Now,
                                        MenuSysNo = item.MenuID,
                                        RoleSysNo = model.SysNo
                                    };
                                Hyt.DataAccess.Sys.ISyRoleMenuDao.Instance.Insert(m);
                            }
                            else if (item.MenuType == 1)
                            {
                                var p = new SyRolePrivilege
                                    {
                                        CreatedBy = model.CreatedBy,
                                        CreatedDate = DateTime.Now,
                                        LastUpdateBy = model.CreatedBy,
                                        LastUpdateDate = DateTime.Now,
                                        RoleSysNo = model.SysNo,
                                        PrivilegeSysNo = item.MenuID
                                    };
                                Hyt.DataAccess.Sys.ISyRolePrivilegeDao.Instance.Insert(p);
                            }
                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            else //新增
            {
                #region 新增
                if (groupMeuns == null)
                {
                    throw new Exception("菜单权限组不能为空");
                }
                model.CreatedBy = userId;
                model.CreatedDate = DateTime.Now;
                model.LastUpdateDate = DateTime.Parse("1900-01-01");
                model.SysNo = Hyt.DataAccess.Sys.ISyRoleDao.Instance.Insert(model);
                if (model.SysNo > 0) //添加菜单权限
                {
                    foreach (SyRoleMenuPrivilege item in groupMeuns)
                    {
                        if (item.MenuType == 0)
                        {
                            var m = new SyRoleMenu
                                {
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateBy = model.CreatedBy,
                                    LastUpdateDate = DateTime.Now,
                                    MenuSysNo = item.MenuID,
                                    RoleSysNo = model.SysNo
                                };
                            Hyt.DataAccess.Sys.ISyRoleMenuDao.Instance.Insert(m);
                        }
                        else if (item.MenuType == 1)
                        {
                            var p = new SyRolePrivilege
                                {
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateBy = model.CreatedBy,
                                    LastUpdateDate = DateTime.Now,
                                    RoleSysNo = model.SysNo,
                                    PrivilegeSysNo = item.MenuID
                                };
                            Hyt.DataAccess.Sys.ISyRolePrivilegeDao.Instance.Insert(p);
                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            return res;
        }

        /// <summary>
        /// 是否存在相同的角色名
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="sysNo">sysNo</param>
        /// <returns>系统角色</returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        private SyRole GetByRoleName(string roleName, int sysNo)
        {
            return Hyt.DataAccess.Sys.ISyRoleDao.Instance.GetByRoleName(roleName, sysNo);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Result Update(SyRole model)
        {
            Result res = new Result();
            var r = Hyt.DataAccess.Sys.ISyRoleDao.Instance.Update(model);
            if (r) res.Status = true;
            return res;
        }

        /// <summary>
        /// 插入角色
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Result Insert(SyRole model)
        {
            Result res = new Result();
            var r = Hyt.DataAccess.Sys.ISyRoleDao.Instance.Insert(model);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 启用（禁用）角色
        /// </summary>
        /// <param name="sysNo">角色编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public Result ChangeStatus(int sysNo)
        {
            var res = new Result();
            var model = Hyt.DataAccess.Sys.ISyRoleDao.Instance.Select(sysNo);
            var changedStatus = model.Status == (int)SystemStatus.角色状态.禁用
                                  ? (int)SystemStatus.角色状态.启用
                                  : (int)SystemStatus.角色状态.禁用;
            var r = Hyt.DataAccess.Sys.ISyRoleDao.Instance.ChangeStatus(sysNo, changedStatus);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 获取角色菜单权限树
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>角色菜单权限树列表</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public List<ZCheckTreeNode> GetTreeListByRoleSysNo(int roleSysNo)
        {
            var lstResult = new List<ZCheckTreeNode>();
            var lstM = GetRoleMenu(roleSysNo);//角色对应菜单
            var lstP = GetRolePermission(roleSysNo);//角色对应权限
            var lst = Hyt.DataAccess.Sys.ISyMenuDao.Instance.GetAll();//所有菜单
            var lstMp = Hyt.DataAccess.Sys.ISyPrivilegeDao.Instance.GetMenuPrivilege();//所有菜单权限
            if (lst != null)
            {
                //子菜单
                BuildTree(0, lst, lstM, lstP, lstMp, lstResult);
            }
            return lstResult;
        }

        /// <summary>
        /// 通过角色获得所属权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>角色获得所属权限列表</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        private List<SyRoleMenuPrivilege> GetRolePermission(int roleSysNo)
        {
            IList<SyRolePrivilege> lst = Hyt.DataAccess.Sys.ISyRolePrivilegeDao.Instance.GetListByRoleSysNo(roleSysNo);
            if (lst != null)
            {
                return lst.Select(m => new SyRoleMenuPrivilege { MenuID = m.PrivilegeSysNo, MenuType = 1, RoleSysNo = roleSysNo }).ToList();
            }
            else
            {
                return new List<SyRoleMenuPrivilege>();
            }
        }

        /// <summary>
        /// 通过角色获得所属菜单
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>角色所属菜单列表</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        private List<SyRoleMenuPrivilege> GetRoleMenu(int roleSysNo)
        {
            IList<SyRoleMenu> lst = Hyt.DataAccess.Sys.ISyRoleMenuDao.Instance.GetListByRoleSysNo(roleSysNo);
            if (lst != null)
            {
                return lst.Select(m => new SyRoleMenuPrivilege { MenuID = m.MenuSysNo, MenuType = 0, RoleSysNo = roleSysNo }).ToList();
            }
            else
            {
                return new List<SyRoleMenuPrivilege>();
            }
        }

        /// <summary>
        /// 生成树形
        /// </summary>
        /// <param name="pmeunId">上级菜单编号</param>
        /// <param name="lst">所有菜单</param>
        /// <param name="lstM">用户组对应的菜单</param>
        /// <param name="lstP">用户组对应的权限</param>
        /// <param name="lstMp">所有菜单权限</param>
        /// <param name="lstResult">结果</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        private void BuildTree(int pmeunId, IEnumerable<SyMenu> lst, List<SyRoleMenuPrivilege> lstM, IEnumerable<SyRoleMenuPrivilege> lstP, IList<CBSyPrivilege> lstMp, List<ZCheckTreeNode> lstResult)
        {
            var syMenus = lst as SyMenu[] ?? lst.ToArray();
            List<SyMenu> sublist = syMenus.Where(m => m.ParentSysNo == pmeunId && m.Status == (int)Hyt.Model.WorkflowStatus.SystemStatus.菜单状态.启用).ToList();
            if (sublist.Count > 0)
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
                    BuildTree(s.SysNo, syMenus, lstM, lstP, lstMp, lstResult);
                }
            }

            //检查菜单下面的权限
            if (lstMp != null && lstMp.Count > 0)
            {
                List<CBSyPrivilege> childlist = lstMp.Where(m => m.MenuSysNo == pmeunId).ToList();
                if (childlist.Count > 0)
                {
                    lstResult.AddRange(childlist.Select(c => new ZCheckTreeNode
                        {
                            id = "p_" + c.SysNo,
                            name = c.Name,
                            nodetype = 1,
                            open = true,
                            @checked = lstP.Any(m => m.MenuID == c.SysNo),
                            pId = "m_" + pmeunId,
                            icon = "/Theme/images/icons/operate.png",
                        }));
                }
            }

        }

    }
}
