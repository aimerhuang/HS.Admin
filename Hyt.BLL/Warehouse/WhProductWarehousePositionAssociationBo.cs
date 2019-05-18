using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 仓库库位产品关联表
    /// </summary>
    /// <remarks>
    /// 2016-06-15 王耀发 创建
    /// </remarks>
    public class WhProductWarehousePositionAssociationBo : BOBase<WhProductWarehousePositionAssociationBo>
    {

        #region 仓库库位关联
        /// <summary>
        /// 获取仓库库位关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(int productStockSysNo, int warehouseSysNo)
        {
            return IWhProductWarehousePositionAssociationDao.Instance.GetPositionAssociationDetail(productStockSysNo,warehouseSysNo);
        }

        /// <summary>
        /// 保存仓库库位列表信息
        /// </summary>
        /// <param name="sysNo">库存编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public Result SaveWarehousePositionAssociations(int sysNo, IList<WhProductWarehousePositionAssociation> listPositionAssociations)
        {
            Result result = new Result();
            result.Status = true;

            IList<WhProductWarehousePositionAssociation> list = new List<WhProductWarehousePositionAssociation>();
            foreach (WhProductWarehousePositionAssociation positionAssociation in listPositionAssociations)
            {
                WhProductWarehousePositionAssociation entity = new WhProductWarehousePositionAssociation
                {
                    SysNo = positionAssociation.SysNo,
                    WarehousePositionSysNo = positionAssociation.WarehousePositionSysNo,
                    ProductStockSysNo = positionAssociation.ProductStockSysNo,
                    CreatedDate = DateTime.Now,
                    CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateDate = DateTime.Now
                };
                list.Add(entity);
            }
            bool r = IWhProductWarehousePositionAssociationDao.Instance.SetWarehousePositionAssociations(sysNo, list);
            if (r == false)
            {
                result.Status = r;
                result.Message = "库位关联失败";
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 获取仓库库位关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号集合</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-7-19 杨云奕 创建</remarks>
        public IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(List<int> ProSysNos, int? warehouseSysNo)
        {
            return IWhProductWarehousePositionAssociationDao.Instance.GetPositionAssociationDetail(ProSysNos, warehouseSysNo);
        }


    }
}
