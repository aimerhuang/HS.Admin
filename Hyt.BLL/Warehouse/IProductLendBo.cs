using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 借货单维护Bo
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public interface IProductLendBo
    {
        /// <summary>
        /// 商品还货
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="model">入库单实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        Result ProductReturn(int deliveryUserSysNo, WhStockIn model);

        /// <summary>
        /// 配送员借货额度结算
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="settlementPrice">借货金额</param>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        void DeliveryCreditCalculate(int deliveryUserSysNo, int warehouseSysNo, decimal settlementPrice, int sysno);

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        PagedList<WhProductLend> GetProductLendList(ParaProductLendFilter filter);

        /// <summary>
        /// 检查配送员在该仓库下是否有未完结借货单数量
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="messages">返回消息</param>
        /// <returns>结果</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        bool CheckWhProductLendWarehouse(int deliveryUserSysNo, int warehouseSysNo, ref string messages);

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        List<CBWhProductLend> WhProductLendExportExcel(ParaProductLendFilter filter);

        /// <summary>
        /// 通过借货单编号获取借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>借货单</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        WhProductLend GetWhProductLend(int sysNo);

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        int CreateWhProductLend(WhProductLend model);

        /// <summary>
        /// 更新借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        int UpdateWhProductLend(WhProductLend model);

        /// <summary>
        /// 作废借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>成功失败</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        Result CancelWhProductLend(int sysNo);

        /// <summary>
        /// 强制完结借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <param name="lastUpdateBySysNo">最后更新人系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks> 
        Result EndProductLend(int sysNo, int lastUpdateBySysNo);

        /// <summary>
        /// 通过过滤条件借货单编号获取借货明细列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货明细列表</returns>
        /// <remarks>2013-07-12 周唐炬 创建</remarks>
        PagedList<CBWhProductLendItem> GetWhProductLendItemList(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 创建借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        int CreateWhProductLendItem(WhProductLendItem model);

        /// <summary>
        /// 更新借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        int UpdateWhProductLendItem(WhProductLendItem model);

        /// <summary>
        /// 创建借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        int CreateWhProductLendPrice(WhProductLendPrice model);

        /// <summary>
        /// 更新借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        int UpdateWhProductLendPrice(WhProductLendPrice model);

        /// <summary>
        /// 检查本次借货商品价格与历史借货价格差异
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="deliveryUserSysNo">借货人系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        Result CheckProductPrice(int productSysNo, int deliveryUserSysNo);

        /// <summary>
        /// 检查本次借货商品价格与历史借货价格差异
        /// </summary>
        /// <param name="productSysNos">商品系统编号</param>
        /// <param name="deliveryUserSysNo">借货人系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        List<dynamic> CheckProductPrice(List<int> productSysNos, int deliveryUserSysNo);
    }
}
