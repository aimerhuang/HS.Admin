using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiWarehouse
{
    /// <summary>
    /// 仓库接口
    /// </summary>
    /// <remarks>2017-07-07 杨浩 创建</remarks>
    public abstract class IWarehouseProvider
    {
        /// <summary>
        /// 仓库标识
        /// </summary>
        /// <remarks> </remarks>
        public abstract Hyt.Model.CommonEnum.仓库代码 Code { get; }
        /// <summary>
        /// 订单号前缀（10000：b2c ）
        /// </summary>
        public string OrderNoPrefix
        {
            get { return "10000"; }
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-07-07 杨浩 创建</remarks>
        public abstract void ExportOrder(object data);
    }
}
