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
    /// 仓库库位
    /// </summary>
    /// <remarks>
    /// 2016-06-15 王耀发 创建
    /// </remarks>
    public class WhWarehousePositionBo : BOBase<WhWarehousePositionBo>
    {

        #region 仓库库位
        /// <summary>
        /// 获取仓库库位
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public IList<WhWarehousePosition> GetWarehousePositions(int warehouseSysNo)
        {
            return IWhWarehousePositionDao.Instance.GetWarehousePositions(warehouseSysNo);
        }
        /// <summary>
        /// 获取仓库库位
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="positionName">库位名称</param>
        /// <param name="status">状态</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-7-3 杨浩 创建</remarks>
        public IList<WhWarehousePosition> WarehousePositionsFilter(int warehouseSysNo, string positionName, int status)
        {
           var warehousePositions =IWhWarehousePositionDao.Instance.GetWarehousePositions(warehouseSysNo);

           if (warehousePositions!=null&&string.IsNullOrWhiteSpace(positionName))
           {
              return warehousePositions.Where(x => x.WarehousePositionName.Contains(positionName)).ToList();
           }
           
           return warehousePositions;
        }
        /// <summary>
        /// 获取出库商品库位列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="warehouseSysNo">商品编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public IList<WhWarehousePosition> GetWPositionsByWsysNoAndProSysNo(int warehouseSysNo, int productSysNo)
        {
            return IWhWarehousePositionDao.Instance.GetWPositionsByWsysNoAndProSysNo(warehouseSysNo, productSysNo);

        }
        /// <summary>
        /// 保存仓库库位列表信息
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public Result SaveWarehousePositions(int sysNo,IList<WhWarehousePosition> listWarehousePositions)
        {
            Result result = new Result();
            result.Status = true;

            IList<WhWarehousePosition> list = new List<WhWarehousePosition>();
            foreach (WhWarehousePosition warehousePosition in listWarehousePositions)
            {
                WhWarehousePosition entity = new WhWarehousePosition
                {
                    SysNo = warehousePosition.SysNo,
                    WarehouseSysNo = warehousePosition.WarehouseSysNo,
                    WarehousePositionName = warehousePosition.WarehousePositionName,
                    Description = warehousePosition.Description,
                    Status = warehousePosition.Status,
                    CreatedDate = DateTime.Now,
                    CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateDate = DateTime.Now
                };
                list.Add(entity);
            }
            bool r = IWhWarehousePositionDao.Instance.SetWarehousePositions(sysNo, list);
            if(r == false)
            {
                result.Status = r;
                result.Message = "库位保存失败"; 
            }
                
            return result;
        }
        #endregion
    }
}
