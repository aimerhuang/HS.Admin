using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    /// <remarks>
    /// 2013-6-25 杨浩 创建
    /// </remarks>
    public class SyMenuBO:BOBase<SyMenuBO>
    {
        /// <summary>
        /// 获取系统用户菜单权限
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>菜单列表</returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public IList<Hyt.Model.SyMenu> GetList(int userSysNo)
        {
            return DataAccess.Sys.ISyMenuDao.Instance.GetList(userSysNo);
        }

        #region 菜单管理 查询

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单列表</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public IList<SyMenu> GetMenuTree()
        {
            return ISyMenuDao.Instance.GetAll();
        }

        /// <summary>
        /// 获取3级(不含)以下菜单
        /// </summary>
        /// <returns>菜单列表</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public IList<SyMenu> GetTopMenus()
        {
            //获取三级以下菜单
            var allMenus = GetGroupingMenus();
            return allMenus.Where(o => o.Level < 3).ToList();
        }

        /// <summary>
        /// 获取带等级的菜单
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-09-26 朱家宏 创建</remarks>
        public IList<SyMenu> GetGradingMenus()
        {
            var sourceMenus = ISyMenuDao.Instance.GetAll();
            var targetMenus = new List<SyMenu>();
            foreach (var menu in sourceMenus)
            {
                var level = 0;
                GradeMemu(menu, sourceMenus, ref level);
                menu.Level = level;
                targetMenus.Add(menu);
            }
            return targetMenus;
        }

        /// <summary>
        /// 菜单分组
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-09-26 朱家宏 创建</remarks>
        public IList<SyMenu> GetGroupingMenus()
        {
            var menus = GetGradingMenus();

            var parentSysNos = menus.OrderBy(o => o.ParentSysNo).Select(o => o.ParentSysNo).Distinct();

            var groupingMenus = menus.Where(o => o.ParentSysNo == 0).ToList();

            var subMenus = menus.Where(o => o.ParentSysNo != 0).ToList();

            foreach (var parentSysNo in parentSysNos)
            {
                if (parentSysNo == 0) continue;
                var childMenus = subMenus.Where(o => o.ParentSysNo == parentSysNo).ToList();
                var currentNode = groupingMenus.SingleOrDefault(o => o.SysNo == parentSysNo);
                groupingMenus.InsertRange(groupingMenus.IndexOf(currentNode) + 1, childMenus);
            }
            return groupingMenus;
        }

        /// <summary>
        /// 菜单分级
        /// </summary>
        /// <param name="menu">菜单</param>
        /// <param name="menus">菜单列表</param>
        /// <param name="level">计算结果</param>
        /// <returns>void</returns>
        /// <remarks>2013-09-26 朱家宏 创建</remarks>
        public void GradeMemu(SyMenu menu, IList<SyMenu> menus, ref int level)
        {
            level++;
            if (menu.ParentSysNo != 0)
            {
                GradeMemu(menus.First(o => o.SysNo == menu.ParentSysNo), menus, ref level);
            }
        }

        /// <summary>
        /// 模糊查询未使用过的权限(匹配权限名称/代码)
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>权限列表</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public IList<CBSyPrivilege> SearchUnUsedPrivileges(string keyword)
        {
            if (keyword != null)
            {
                keyword = keyword.Replace(" ", string.Empty).Replace("　", string.Empty);
            }
            var allPrivileges = ISyPrivilegeDao.Instance.Query(keyword);
            var unUsedPrivileges =
                allPrivileges.Where(o => o.MenuSysNo == 0 && o.Status == (int) SystemStatus.权限状态.启用).ToList();
            return unUsedPrivileges;
        }

        /// <summary>
        /// 获取已使用的权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>已使用的权限列表</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public IList<CBSyPrivilege> GetUsedPrivileges(int menuSysNo)
        {
            var allPrivileges = ISyPrivilegeDao.Instance.Query(null);
            var usedPrivileges =
                allPrivileges.Where(o => o.MenuSysNo == menuSysNo && o.Status == (int)SystemStatus.权限状态.启用).ToList();
            return usedPrivileges;
        }

        /// <summary>
        /// 通过sysNo获取菜单
        /// </summary>
        /// <param name="menuSysNo">sysNo</param>
        /// <returns>model</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public SyMenu GetMenu(int menuSysNo)
        {
            return ISyMenuDao.Instance.Select(menuSysNo);
        }

        /// <summary>
        /// 获取所有子菜单
        /// </summary>
        /// <param name="menuSysNo">当前菜单编号</param>
        /// <param name="children">输出结果</param>
        /// <remarks>2013-09-27 朱家宏 创建</remarks>
        public void DoChildNodeRead(int menuSysNo, ref List<SyMenu> children)
        {
            var menus = ISyMenuDao.Instance.GetAllByParentSysNo(menuSysNo);

            if (menus != null && menus.Any())
            {
                foreach (var menu in menus)
                {
                    children.Add(menu);
                    DoChildNodeRead(menu.SysNo, ref children);
                }
            }
        }

        #endregion

        #region 菜单管理 操作

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <param name="privilegeSysNoList">权限列表</param>
        /// <returns>Result类型</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public Result CreateMenu(SyMenu menu, List<int> privilegeSysNoList)
        {
            if (menu == null)
                throw new ArgumentNullException();

            privilegeSysNoList = privilegeSysNoList ?? new List<int>();

            var result = new Result();

            //创建菜单
            menu.CreatedDate = DateTime.Now;
            menu.LastUpdateDate = DateTime.Now;
            result.StatusCode = ISyMenuDao.Instance.Insert(menu);
            result.Status = result.StatusCode > 0;

            //添加菜单权限
            if (result.Status)
            {
                foreach (var privilegeSysNo in privilegeSysNoList)
                {
                    var menuPrivilege = new SyMenuPrivilege
                        {
                            CreatedBy = menu.CreatedBy,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = menu.CreatedBy,
                            LastUpdateDate = DateTime.Now,
                            MenuSysNo = result.StatusCode,
                            PrivilegeSysNo = privilegeSysNo
                        };
                    ISyMenuPrivilegeDao.Instance.Insert(menuPrivilege);
                }
            }

            return result;
        }

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <param name="privilegeSysNoList">权限列表</param>
        /// <returns>Result类型</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public Result SaveMenu(SyMenu menu, List<int> privilegeSysNoList)
        {
            if (menu == null)
                throw new ArgumentNullException();

            privilegeSysNoList = privilegeSysNoList ?? new List<int>();

            var result = new Result();

            var savingMenu = ISyMenuDao.Instance.Select(menu.SysNo);
            if (savingMenu.SysNo == 0)
                return result;

            //mapping
            savingMenu.DisplayOrder = menu.DisplayOrder;
            savingMenu.InNavigator = menu.InNavigator;
            savingMenu.LastUpdateBy = menu.LastUpdateBy;
            savingMenu.LastUpdateDate = menu.LastUpdateDate;
            savingMenu.MenuImage = menu.MenuImage;
            savingMenu.MenuName = menu.MenuName;
            savingMenu.MenuUrl = menu.MenuUrl;
            savingMenu.ParentSysNo = menu.ParentSysNo;
            savingMenu.Status = menu.Status;
            
            //更新菜单
            result.Status = ISyMenuDao.Instance.Update(savingMenu);

            if (result.Status)
            {
                //移出菜单权限
                var t = ISyMenuPrivilegeDao.Instance.DeleteByMenuSysNo(menu.SysNo);

                //添加菜单权限
                foreach (var privilegeSysNo in privilegeSysNoList)
                {
                    var menuPrivilege = new SyMenuPrivilege
                        {
                            CreatedBy = menu.CreatedBy,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = menu.CreatedBy,
                            LastUpdateDate = DateTime.Now,
                            MenuSysNo = menu.SysNo,
                            PrivilegeSysNo = privilegeSysNo
                        };
                    var s = ISyMenuPrivilegeDao.Instance.Insert(menuPrivilege);
                }
            }

            return result;
        }

        /// <summary>
        /// 菜单位置移动 （上移/下移）
        /// </summary>
        /// <param name="sourceNodeId">原节点</param>
        /// <param name="targetNodeId">目标节点</param>
        /// <param name="direction">移动方向</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public bool MoveTreeNode(int sourceNodeId, int targetNodeId, string direction)
        {
            if (sourceNodeId < 1 || targetNodeId < 1)
            {
                return false;
            }

            var sourceMenu = ISyMenuDao.Instance.Select(sourceNodeId);
            var targetMenu = ISyMenuDao.Instance.Select(targetNodeId);

            var sourceOrder = sourceMenu.DisplayOrder;
            var targetOrder = targetMenu.DisplayOrder;

            if (sourceOrder == targetOrder)
            {
                //相同序号下的移动
                var i = 0;
                var allMenus = ISyMenuDao.Instance.GetAllByParentSysNo(sourceMenu.ParentSysNo);

                var sourceIndex = allMenus.IndexOf(allMenus.Single(o => o.SysNo == sourceMenu.SysNo));
                var targetIndex = allMenus.IndexOf(allMenus.Single(o => o.SysNo == targetMenu.SysNo));

                allMenus[sourceIndex] = targetMenu;
                allMenus[targetIndex] = sourceMenu;

                foreach (var menu in allMenus)
                {
                    menu.DisplayOrder = i;
                    ISyMenuDao.Instance.Update(menu);
                    i++;
                }
                return true;
            }

            sourceMenu.DisplayOrder = targetOrder;
            targetMenu.DisplayOrder = sourceOrder;

            return ISyMenuDao.Instance.Update(sourceMenu) && ISyMenuDao.Instance.Update(targetMenu);
        }

        /// <summary>
        /// 切换菜单状态 (开启/禁用)
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public bool ChangeMenuStatus(int menuSysNo)
        {
            var allMenus = ISyMenuDao.Instance.GetAll();

            var selectedMenu = allMenus.SingleOrDefault(o => o.SysNo == menuSysNo);
            if (selectedMenu == null) return false;

            var changedStatus = selectedMenu.Status == (int) SystemStatus.菜单状态.禁用
                                    ? (int) SystemStatus.菜单状态.启用
                                    : (int) SystemStatus.菜单状态.禁用;

            selectedMenu.Status = changedStatus;
            var r = ISyMenuDao.Instance.Update(selectedMenu);

            if (r)
            {
                var subMenus = new List<SyMenu>();
                DoChildNodeRead(menuSysNo,ref subMenus);
                foreach (var m in subMenus)
                {
                    m.Status = changedStatus;
                    ISyMenuDao.Instance.Update(m);
                }
            }

            return r;
        }

        /// <summary>
        /// 是否在导航栏显示 (是/否)
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public bool ChangeInNavigatorStatus(int menuSysNo)
        {
            var menu = ISyMenuDao.Instance.Select(menuSysNo);

            menu.InNavigator = (menu.InNavigator == (int) SystemStatus.是否导航栏显示.是)
                                   ? (int) SystemStatus.是否导航栏显示.否
                                   : (int) SystemStatus.是否导航栏显示.是;

            return ISyMenuDao.Instance.Update(menu);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-08-08 朱家宏 创建</remarks>
        public bool RemoveMenu(int menuSysNo)
        {
            var childMenus = new List<SyMenu>();
            DoChildNodeRead(menuSysNo,ref childMenus);

            foreach (var child in childMenus)
            {
                ISyRoleMenuDao.Instance.DeleteByMenuSysNo(child.SysNo);             //删除子菜单角色菜单
                ISyMenuPrivilegeDao.Instance.DeleteByMenuSysNo(child.SysNo);        //删除子菜单权限
                ISyMenuDao.Instance.Delete(child.SysNo);                            //删除子菜单
            }

            ISyRoleMenuDao.Instance.DeleteByMenuSysNo(menuSysNo);                   //删除角色菜单
            ISyMenuPrivilegeDao.Instance.DeleteByMenuSysNo(menuSysNo);              //删除菜单权限
            var r = ISyMenuDao.Instance.Delete(menuSysNo);                          //删除菜单

            return r;
        }

        #endregion

    }
}
