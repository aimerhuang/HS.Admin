using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 组织机构Bo
    /// </summary>
    /// <remarks>2013-10-08 周唐炬 创建</remarks>
    public class OrganizationBo : BOBase<OrganizationBo>
    {
        /// <summary>
        /// 顺序调整
        /// </summary>
        /// <param name="sourceNodeId">原节点</param>
        /// <param name="targetNodeId">目标节点</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public void OrganizationDisplayOrderMove(int sourceNodeId, int targetNodeId)
        {
            if (sourceNodeId < 1 || targetNodeId < 1) return;

            var sourceMenu = IOrganizationDao.Instance.GetEntity(sourceNodeId);
            var targetMenu = IOrganizationDao.Instance.GetEntity(targetNodeId);
            if (sourceMenu == null || targetMenu == null) throw new HytException("未找到对应该组织机构，请刷新重试！");
            var sourceOrder = sourceMenu.DisplayOrder;
            var targetOrder = targetMenu.DisplayOrder;

            if (sourceOrder == targetOrder)
            {
                //相同序号下的移动
                var i = 0;
                var allMenus = IOrganizationDao.Instance.GetEntityByParentSysNo(sourceMenu.ParentSysNo);

                var sourceIndex = allMenus.IndexOf(allMenus.Single(o => o.SysNo == sourceMenu.SysNo));
                var targetIndex = allMenus.IndexOf(allMenus.Single(o => o.SysNo == targetMenu.SysNo));

                allMenus[sourceIndex] = targetMenu;
                allMenus[targetIndex] = sourceMenu;

                foreach (var menu in allMenus)
                {
                    menu.DisplayOrder = i;
                    IOrganizationDao.Instance.OrganizationUpdate(menu);
                    i++;
                }
            }
            sourceMenu.DisplayOrder = targetOrder;
            targetMenu.DisplayOrder = sourceOrder;
            IOrganizationDao.Instance.OrganizationUpdate(sourceMenu);
            IOrganizationDao.Instance.OrganizationUpdate(targetMenu);
        }

        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public int OrganizationCreate(BsOrganization model)
        {
            return IOrganizationDao.Instance.OrganizationCreate(model);
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public int OrganizationUpdate(BsOrganization model)
        {
            var entity = IOrganizationDao.Instance.GetEntity(model.SysNo);
            if (entity != null)
            {
                model.CreatedBy = entity.CreatedBy;
                model.CreatedDate = entity.CreatedDate;
            }
            else
            {
                throw new HytException("该组织机构不存在,请检查!");
            }
            return IOrganizationDao.Instance.OrganizationUpdate(model);
        }

        /// <summary>
        /// 根据编号获取组织机构
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <returns>组织机构实体</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public BsOrganization GetEntity(int sysNo)
        {
            return IOrganizationDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="sysNo">组织机构系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public void OrganizationRemove(int sysNo)
        {
            var model = IOrganizationDao.Instance.GetEntity(sysNo);
            if (model == null) throw new HytException("未找该组织机构，请刷新重试！");
            var list = IOrganizationDao.Instance.GetEntityByParentSysNo(model.SysNo);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    ItemRemoveByOrganization(item.SysNo);
                    IOrganizationDao.Instance.OrganizationRemove(item.SysNo);
                }
            }
            ItemRemoveByOrganization(model.SysNo);
            IOrganizationDao.Instance.OrganizationRemove(sysNo);
        }

        /// <summary>
        /// 删除组织机构仓库关联
        /// </summary>
        /// <param name="organizationSysNo">组织机构系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        private void ItemRemoveByOrganization(int organizationSysNo)
        {
            var items = IOrganizationDao.Instance.GetItemsByOrganizationSysNo(organizationSysNo);
            if (items == null || !items.Any()) return;
            foreach (var item in items)
            {
                IOrganizationDao.Instance.OrganizationItemRemove(item.SysNo);
            }
        }

        /// <summary>
        /// 获取所有组织机构
        /// </summary>
        /// <returns>所有组织机构</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public List<BsOrganization> GetAll()
        {
            return IOrganizationDao.Instance.GetAll();
        }

        /// <summary>
        /// 组织机构状态变更
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public void OrganizationStatusChange(int sysNo)
        {
            var model = IOrganizationDao.Instance.GetEntity(sysNo);
            if (model == null) throw new HytException("未找该组织机构，请刷新重试！");
            switch (model.Status)
            {
                case (int)BasicStatus.组织机构状态.启用:
                    model.Status = (int)BasicStatus.组织机构状态.禁用;
                    break;
                case (int)BasicStatus.组织机构状态.禁用:
                    model.Status = (int)BasicStatus.组织机构状态.启用;
                    break;
            }
            IOrganizationDao.Instance.OrganizationUpdate(model);
        }

        /// <summary>
        /// 根据编号获取关联仓库列表
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">每页笔数</param>
        /// <returns>关联仓库列表</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public PagedList<BsOrganizationWarehouse> GetItems(int sysNo, int currentPage,int pageSize=7)
        {
            var model = new PagedList<BsOrganizationWarehouse>();
            var pager = IOrganizationDao.Instance.GetItems(sysNo, currentPage,pageSize);
            if (null != pager)
            {
                model.TData = pager.Rows;
                model.TotalItemCount = pager.TotalRows;
                model.CurrentPageIndex = currentPage;
                model.PageSize = pageSize;
            }

            return model;
        }

        /// <summary>
        /// 批量添加关联仓库
        /// </summary>
        /// <param name="organizationSysNo">组织机构编号</param>
        /// <param name="whList">关联仓库列表</param>
        /// <param name="createBy">创建人</param>
        /// <returns></returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public void OrganizationItemAddRange(int organizationSysNo, List<int> whList, int createBy)
        {
            if (whList == null || !whList.Any()) return;
            foreach (var item in whList)
            {
                var model = IOrganizationDao.Instance.GetItem(organizationSysNo, item);
                if (model != null) { model = null; continue; }
                var oldOrg = GetOrganization(item);//获取仓库已经关联的一个组织机构
                if (oldOrg != null && oldOrg.SysNo != organizationSysNo)
                {
                   var strName= Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(item);//仓库名称
                   string strWarn = string.Format("{0}已经被{1}关联", strName, oldOrg.Name);//警告信息
                   throw new HytException(strWarn);
                }
                model = new BsOrganizationWarehouse()
                    {
                        OrganizationSysNo = organizationSysNo,
                        WarehouseSysNo = item,
                        CreatedBy = createBy,
                        CreatedDate = DateTime.Now
                    };
                IOrganizationDao.Instance.OrganizationItemCreate(model);
            }
        }

        /// <summary>
        /// 删除关联仓库
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public void ItemRemove(int sysNo)
        {
            IOrganizationDao.Instance.OrganizationItemRemove(sysNo);
        }

        /// <summary>
        /// 根据仓库获取组织机构
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>组织机构</returns>
        /// <remarks>2013-11-26 吴文强 创建</remarks>
        public BsOrganization GetOrganization(int warehouseSysNo)
        {
            return IOrganizationDao.Instance.GetOrganization(warehouseSysNo);
        }
    }
}
