using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 仓库覆盖地区信息接口类
    /// </summary>
    /// <remarks>2013-08-12 周瑜 创建</remarks>
    public abstract class IWhWarehouseAreaDao : DaoBase<IWhWarehouseAreaDao>
    {
        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>新增记录的系统编号</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public abstract int Insert(WhWarehouseArea warehouse);

        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public abstract int Update(WhWarehouseArea warehouse);

        /// <summary>
        /// 删除仓库覆盖地区
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public abstract int Delete(int areaSysNo, int warehouseSysNo);

        /// <summary>
        /// 将该仓库设为选中地区的默认发货仓库
        /// </summary>
        /// <param name="whWarehouseArea">地区仓库实体</param>
        /// <param name="status">是否默认仓库，默认：是</param>
        /// <returns>是否设置成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public abstract int SetDefault(WhWarehouseArea whWarehouseArea, WarehouseStatus.是否默认仓库 status);

        /// <summary>
        /// 根据地区编号查询覆盖该地区的所有仓库
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <returns>新增记录的系统编号</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        public abstract IList<CBWhWarehouse> GetWarehouseForArea(int areaSysNo);

        /// <summary>
        /// 获取所有仓库覆盖地区
        /// </summary>
        /// <returns>所有的仓库覆盖地区</returns>
        /// <remarks>2014-05-15 朱成果 创建</remarks>
        public abstract IList<WhWarehouseArea> GetAllWhWarehouseArea();
    }
}
