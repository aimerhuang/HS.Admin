using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 业务员逻辑接口
    /// </summary>
    /// <remarks>
    /// 2013/6/27 何方 创建
    /// </remarks>
    interface IDeliveryUserBo
    {

        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        Dictionary<int, string> GetWhDeliveryUserDict(int warehouseSysNo);

        /// <summary>
        /// 获取未录入信用信息的仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        Dictionary<int, string> GetWhDeliveryUserDictForCredit(int warehouseSysNo);
    }
}
