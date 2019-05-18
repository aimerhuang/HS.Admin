using Hyt.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 插入出库单
    /// </summary>
    /// <param name="model">出库单明细</param>
    /// <returns>出库单系统编号</returns>
    /// <remarks>2016-06-24 王耀发 创建</remarks>
    public abstract class IWhInventoryOutDao : DaoBase<IWhInventoryOutDao>
    {
        /// <summary>
        /// 插入出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract int InsertWhInventoryOut(WhInventoryOut model);
        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract int InsertWhInventoryOutItem(WhInventoryOutItem model);
        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract int UpdateWhInventoryOut(WhInventoryOut model);

        ///// <summary>
        ///// 删除入库单
        ///// </summary>
        ///// <param name="sysNo">入库单系统编号</param>
        ///// <returns>成功返回true,失败返回false</returns>
        ///// <remarks>2013-06-09 周唐炬 创建</remarks>
        //public abstract bool DelWhStockIn(int sysNo);

        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract Pager<WhInventoryOut> GetWhInventoryOutList(ParaInventoryOutFilter filter, int pageSize);


        public abstract Pager<WhInventoryOut> GetWhInventoryOutListTo(ParaInventoryOutFilter filter, int pageSize);

        ///// <summary>
        ///// 根据来源单据和类型获取入库单
        ///// </summary>
        ///// <param name="sourceType">来源类型</param>
        ///// <param name="sourceNo">来源单据系统编号</param>
        ///// <returns>入库单</returns>
        ///// <remarks>2013-9-3 黄伟 创建</remarks>
        //public abstract WhStockIn GetStockInBySource(int sourceType, int sourceNo);

        /// <summary>
        /// 通过系统编号获取入库单明细
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract WhInventoryOut GetWhInventoryOut(int sysNo);

        ///// <summary>
        ///// 通过事务编号获取入库单明细
        ///// </summary>
        ///// <param name="transactionSysNo">事务编号</param>
        ///// <returns>返回入库单明细,包含入库商品列表</returns>
        ///// <remarks>2013-06-08 周唐炬 创建</remarks>
        //public abstract WhStockIn GetWhStockInByTransactionSysNo(string transactionSysNo);

        ///// <summary>
        ///// 商品入库
        ///// </summary>
        ///// <param name="model">入库单明细</param>
        ///// <returns>返回受影响行</returns>
        ///// <remarks>2013-06-08 周唐炬 创建</remarks>
        //public abstract int InsertWhStockInItem(WhStockInItem model);

        /// <summary>
        /// 更新商品出库信息
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract int UpdateWhInventoryOutItem(WhInventoryOutItem model);

        ///// <summary>
        ///// 删除商品入库信息
        ///// </summary>
        ///// <param name="sysNo">系统编号</param>
        ///// <returns>成功返回true,失败返回false</returns>
        ///// <remarks>2013-06-09 周唐炬 创建</remarks>
        //public abstract bool DelWhStockInItem(int sysNo);

        /// <summary>
        /// 通过出库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">出库单系统SysNO</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回出库单商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract IList<WhInventoryOutItem> GetWhInventoryOutItemListByInventoryOutSysNo(int InventoryOutSysNo, int pageIndex, int pageSize);

        /// <summary>
        /// 通过出库单ID获取所有商品总数
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回出库单所有商品总数</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract int GetWhInventoryOutItemListByInventoryOutSysNoCount(int InventoryOutSysNo);

        ///// <summary>
        ///// 通过入库单系统编号获取入库商品信息
        ///// </summary>
        ///// <param name="stockInSysNo">入库单系统编号</param>
        ///// <param name="productSysNo">商品系统编号</param>
        ///// <returns>入库商品信息</returns>
        ///// <remarks>2013-06-09 周唐炬 创建</remarks>
        //public abstract WhStockInItem GetWhStockInItemBySysNo(int stockInSysNo, int productSysNo);

        /// <summary>
        /// 通过出库明细系统编号获取入库明细
        /// </summary>
        /// <param name="sysNo">出库明细系统编号</param>
        /// <returns>出库明细</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public abstract WhInventoryOutItem GetWhInventoryOutItem(int sysNo);

        ///// <summary>
        ///// 通过入库单ID获取所有商品列表
        ///// </summary>
        ///// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        ///// <returns>返回入库单商品列表</returns>
        ///// <remarks>2013-06-24 郑荣华 创建</remarks>
        //public abstract List<WhStockInItem> GetWhStockInItemList(int stockInSysNo);

        ///// <summary>
        ///// 根据单据来源获取入库单
        ///// </summary>
        ///// <param name="source">单据来源</param>
        ///// <param name="sourceSysNo">单据编号</param>
        ///// <returns>入库单</returns>
        ///// <remarks>2013-7-25 朱家宏 创建 </remarks>
        //public abstract WhStockIn GetWhStockInByVoucherSource(int source, int sourceSysNo);

        ///// <summary>
        ///// 根据单据来源和状态获取入库单
        ///// </summary>
        ///// <param name="sourceType">单据来源</param>
        ///// <param name="sourceNo">单据编号</param>
        ///// <param name="Status">状态</param>
        ///// <returns></returns>
        ///// <remarks>2014-04-11 朱成果 创建 </remarks>
        //public abstract WhStockIn GetStockInBySourceAndStatus(int sourceType, int sourceNo, int? Status);


        public abstract List<WhInventoryOut> GetWhInventoryOutList(DateTime dateTime);


        /// <summary>
        /// 通过来源单号获取入库单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2018-01-16 罗熙 创建</returns>
        public abstract WhInventoryOut GetWhInventoryOutToSourceSysNo(int sysNo);
    }
}
