using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 借货单维护Dao
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public abstract class IProductLendDao : DaoBase<IProductLendDao>
    {
        /// <summary>
        /// 获取业务员库存
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>业务员库存</returns>
        /// <remarks>2013-12-11 周唐炬 创建</remarks>
        public abstract Pager<WhProductLendItem> GetInventoryProductList(ParaProductLendFilter filter); 

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract Pager<WhProductLend> GetWhProductLendList(ParaProductLendFilter filter);

        /// <summary>
        /// 获取配送员未完结借货单仓库系统编号列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号列表</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        public abstract List<int> GetWhProductLendWarehouseList(int deliveryUserSysNo);

        /// <summary>
        /// 借货单列表导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract List<CBWhProductLend> WhProductLendExportExcel(ParaProductLendFilter filter);

        /// <summary>
        /// 通过借货单编号获取借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>借货单</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract WhProductLend GetWhProductLend(int sysNo);

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract int CreateWhProductLend(WhProductLend model);

        /// <summary>
        /// 更新借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract int UpdateWhProductLend(WhProductLend model);

        /// <summary>
        /// 通过配送员系统编号、商品系统编号获取借货单明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public abstract List<int> GetWhProductLendItemList(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 通过借货单系统编号获取未还货或销售完成的借货单明细条数
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public abstract int GetWhProductLendItemListCount(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 通过借货单系统编号获取借货单明细列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract Pager<CBWhProductLendItem> GetWhProductLendItemPagerList(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 通过借货单编号获取借货单明细
        /// </summary>
        /// <param name="sysNo">借货单明细系统编号</param>
        /// <returns>借货单明细</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract WhProductLendItem GetWhProductLendItem(int sysNo);

        /// <summary>
        /// 获取带信用价格的借货单明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public abstract CBWhProductLendItem GetWhProductLendItemInfo(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 创建借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract int CreateWhProductLendItem(WhProductLendItem model);

        /// <summary>
        /// 更新借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public abstract int UpdateWhProductLendItem(WhProductLendItem model);

        /// <summary>
        /// 创建借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public abstract int CreateWhProductLendPrice(WhProductLendPrice model);

        /// <summary>
        /// 更新借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public abstract int UpdateWhProductLendPrice(WhProductLendPrice model);

        /// <summary>
        /// 获取历史借货商品价格列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>历史借货商品价格列表</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public abstract List<decimal> GetHistoryPrice(ParaWhProductLendItemFilter filter);

        /// <summary>
        /// 根据配送人员系统编号及商品系统编号获取仓库系统编号
        /// </summary>
        /// <param name="delSysNo">配送人员系统编号</param>
        /// <param name="pSysNo">商品系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-07-19 黄伟 创建</remarks>
        public abstract int GetWhSysNoByDelUserAndProduct(int delSysNo, int pSysNo);

        /// <summary>
        /// 根据配送员系统编号获取取件单
        /// </summary>
        /// <param name="deliverySysNo">配送员系统编号</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-09-28 周唐炬 创建</remarks>
        public abstract IList<WhProductLend> GetWhProductLendList(int deliverySysNo);
    }
}
