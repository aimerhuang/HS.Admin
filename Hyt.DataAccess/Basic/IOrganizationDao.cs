using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// 组织机构Dao
    /// </summary>
    /// <remarks>2013-10-08 周唐炬 创建</remarks>
    public abstract class IOrganizationDao : DaoBase<IOrganizationDao>
    {
        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int OrganizationCreate(BsOrganization model);

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int OrganizationUpdate(BsOrganization model);

        /// <summary>
        /// 根据编号获取组织机构
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <returns>组织机构实体</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract BsOrganization GetEntity(int sysNo);

        /// <summary>
        /// 根据父级组织机构系统编号获取所有子节点
        /// </summary>
        /// <param name="parentSysNo">父级组织机构系统编号</param>
        /// <returns>组织机构实体</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract List<BsOrganization> GetEntityByParentSysNo(int parentSysNo);

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="sysNo">组织机构系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract int OrganizationRemove(int sysNo);

        /// <summary>
        /// 获取所有组织机构
        /// </summary>
        /// <returns>所有组织机构</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract List<BsOrganization> GetAll();

        /// <summary>
        /// 根据组织机构系统编号获取所有仓库关联
        /// </summary>
        /// <param name="organizationSysNo">组织机构系统编号</param>
        /// <returns>仓库关联列表</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract List<BsOrganizationWarehouse> GetItemsByOrganizationSysNo(int organizationSysNo);

        /// <summary>
        /// 根据编号获取关联仓库列表
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>关联仓库列表</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract Pager<BsOrganizationWarehouse> GetItems(int sysNo, int currentPage, int pageSize);

        /// <summary>
        /// 添加组织机构关联仓库
        /// </summary>
        /// <param name="model">关联实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract void OrganizationItemCreate(BsOrganizationWarehouse model);

        /// <summary>
        /// 删除组织机构关联仓库
        /// </summary>
        /// <param name="sysNo">关联表系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract int OrganizationItemRemove(int sysNo);

        /// <summary>
        /// 通过组织机构系统编号、仓库系统编号获取组织机构仓库
        /// </summary>
        /// <param name="organizationSysNo">织机构系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>组织机构仓库</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public abstract BsOrganizationWarehouse GetItem(int organizationSysNo, int warehouseSysNo);

        /// <summary>
        /// 根据仓库获取组织机构
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>组织机构</returns>
        /// <remarks>2013-11-26 吴文强 创建</remarks>
        public abstract BsOrganization GetOrganization(int warehouseSysNo);

        /// <summary>
        /// 获取组织机构代码
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-5 杨浩 创建</remarks>
        public abstract string GetCode(int sysno);
    }
}
