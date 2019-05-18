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
    /// 权限
    /// </summary>
    /// <remarks>2013-06-28 杨浩 创建</remarks>
    public class SyPrivilegeBo : BOBase<SyPrivilegeBo>
    {
        /// <summary>
        /// 根据用户SysNo获取权限列表
        /// </summary>
        /// <param name="userSysNo">用户系统号</param>
        /// <returns>权限列表</returns>
        /// <remarks>2013-06-28 杨浩 创建</remarks>
        public IList<Model.SyPrivilege> GetList(int userSysNo)
        {
            return DataAccess.Sys.ISyPrivilegeDao.Instance.GetList(userSysNo);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">权限状态</param>
        /// <param name="keyword">权限名称/权限代码</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public Pager<SyPrivilege> GetPagerList(int currentPage, int pageSize, int? status, string keyword)
        {
            return ISyPrivilegeDao.Instance.SelectAll(currentPage, pageSize, status, keyword);
        }

        /// <summary>
        /// 查询未使用过的权限(匹配权限名称/代码)
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public Pager<CBSyPrivilege> SearchUnUsedPrivileges(string keyword, int currentPage, int pageSize)
        {
            const int menuSysNo = 0;
            const int status = (int) SystemStatus.权限状态.启用;
            return ISyPrivilegeDao.Instance.Query(keyword, menuSysNo, status, currentPage, pageSize);
        }

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="privilege">权限</param>
        /// <returns>Result</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public Result CreatePrivilege(SyPrivilege privilege)
        {
            if (privilege == null)
                throw new ArgumentNullException();

            var result = new Result();

            if (ExistsPrivilege(privilege.Name, privilege.Code))
            {
                result.Status = false;
                result.Message = "不能存在相同的权限名称或代码。";
                return result;
            }

            //创建
            result.StatusCode = ISyPrivilegeDao.Instance.Insert(privilege);
            result.Status = result.StatusCode > 0;
            result.Message = "创建成功。";

            return result;
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="privilege">权限</param>
        /// <returns>Result</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public Result SavePrivilege(SyPrivilege privilege)
        {
            if (privilege == null)
                throw new ArgumentNullException();

            var result = new Result();

            if (ExistsPrivilege(privilege.Name, privilege.Code, privilege.SysNo))
            {
                result.Status = false;
                result.Message = "不能存在相同的权限名称或代码。";
                return result;
            }

            var savingModel = ISyPrivilegeDao.Instance.Select(privilege.SysNo);

            savingModel.Code = privilege.Code;
            savingModel.Description = privilege.Description;
            savingModel.Name = privilege.Name;
            savingModel.Status = privilege.Status;

            result.Status = ISyPrivilegeDao.Instance.Update(savingModel);
            result.Message = "保存成功。";

            return result;
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="sysNo">权限编号</param>
        /// <returns>Result</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public Result RemovePrivilege(int sysNo)
        {
            var existsRolePrivilege = ISyRolePrivilegeDao.Instance.SelectAllByPrivilegeSysNo(sysNo).Count > 0;
            var existsMenuPrivilege = ISyMenuPrivilegeDao.Instance.SelectAllByPrivilegeSysNo(sysNo).Count > 0;

            var result = new Result {Status = false};
            if (existsMenuPrivilege || existsRolePrivilege)
            {
                result.Message = "存在关联数据，不允许删除。";
            }
            else
            {
                result.Status = ISyPrivilegeDao.Instance.Delete(sysNo);
                result.Message = "删除成功。";
            }

            return result;
        }

        /// <summary>
        /// 切换权限状态 (启用/禁用)
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public bool ChangePrivilegeStatus(int sysNo)
        {
            var allPrivileges = ISyPrivilegeDao.Instance.SelectAll();

            var selectedPrivilege = allPrivileges.SingleOrDefault(o => o.SysNo == sysNo);
            if (selectedPrivilege == null) return false;

            var changedStatus = selectedPrivilege.Status == (int)SystemStatus.权限状态.禁用
                                    ? (int)SystemStatus.权限状态.启用
                                    : (int)SystemStatus.权限状态.禁用;

            selectedPrivilege.Status = changedStatus;
            var r = ISyPrivilegeDao.Instance.Update(selectedPrivilege);

            return r;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="sysNo">权限编码</param>
        /// <returns>model</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public SyPrivilege Get(int sysNo)
        {
            return ISyPrivilegeDao.Instance.Select(sysNo);
        }

        /// <summary>
        /// 已存在的权限检查
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="code">权限代码</param>
        /// <param name="exceptedSysNo">排除的权限编号</param>
        /// <returns>t:存在 f:不存在</returns>
        /// <remarks>2013-08-12 朱家宏 创建</remarks>
        public bool ExistsPrivilege(string name = null, string code=null, int exceptedSysNo=0)
        {
            var allPrivileges = ISyPrivilegeDao.Instance.SelectAll();

            var existedPrivilege =
                allPrivileges.Where(o => (o.Name == name || o.Code == code) && o.SysNo != exceptedSysNo);

            return existedPrivilege.Any();
        }

    }
}
