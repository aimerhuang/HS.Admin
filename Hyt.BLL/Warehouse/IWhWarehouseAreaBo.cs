using System.Collections.Generic;
using Hyt.Model;

namespace Hyt.BLL.Warehouse
{
    public interface IWhWarehouseAreaBo
    {
        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        bool Insert(WhWarehouseArea warehouse);

        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        bool Update(WhWarehouseArea warehouse);

        /// <summary>
        /// 删除仓库覆盖地区
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        bool Delete(int areaSysNo, int warehouseSysNo);
    }
}