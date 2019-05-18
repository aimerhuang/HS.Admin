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
    /// 入库单维护抽象
    /// </summary>
    /// <remarks>2013-06-08 周唐炬 创建</remarks>
    public abstract class IInStockDao : DaoBase<IInStockDao>
    {
        /// <summary>
        /// 插入入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract int InsertWhStockIn(WhStockIn model);

        /// <summary>
        /// 更新入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract int UpdateWhStockIn(WhStockIn model);

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract bool DelWhStockIn(int sysNo);

        /// <summary>
        /// 获取入库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public abstract Pager<WhStockIn> GetWhStockInList(ParaInStockFilter filter, int pageSize);

        /// <summary>
        /// 根据来源单据和类型获取入库单
        /// </summary>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceNo">来源单据系统编号</param>
        /// <returns>入库单</returns>
        /// <remarks>2013-9-3 黄伟 创建</remarks>
        public abstract WhStockIn GetStockInBySource(int sourceType, int sourceNo);

        /// <summary>
        /// 通过系统编号获取入库单明细
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public abstract WhStockIn GetWhStockIn(int sysNo);

        /// <summary>
        /// 通过事务编号获取入库单明细
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public abstract WhStockIn GetWhStockInByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public abstract int InsertWhStockInItem(WhStockInItem model);

        /// <summary>
        /// 更新商品入库信息
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public abstract int UpdateWhStockInItem(WhStockInItem model);

        /// <summary>
        /// 删除商品入库信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract bool DelWhStockInItem(int sysNo);

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract IList<WhStockInItem> GetWhStockInItemListByStockInSysNo(int stockInSysNo, int pageIndex, int pageSize);

        /// <summary>
        /// 通过入库单ID获取所有商品总数
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单所有商品总数</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract int GetWhStockInItemListByStockInSysNoCount(int stockInSysNo);

        /// <summary>
        /// 通过入库单系统编号获取入库商品信息
        /// </summary>
        /// <param name="stockInSysNo">入库单系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>入库商品信息</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract WhStockInItem GetWhStockInItemBySysNo(int stockInSysNo, int productSysNo);

        /// <summary>
        /// 通过入库明细系统编号获取入库明细
        /// </summary>
        /// <param name="sysNo">入库明细系统编号</param>
        /// <returns>入库明细</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public abstract WhStockInItem GetWhStockInItem(int sysNo);

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-24 郑荣华 创建</remarks>
        public abstract List<WhStockInItem> GetWhStockInItemList(int stockInSysNo);

        /// <summary>
        /// 根据单据来源获取入库单
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns>入库单</returns>
        /// <remarks>2013-7-25 朱家宏 创建 </remarks>
        public abstract WhStockIn GetWhStockInByVoucherSource(int source, int sourceSysNo);

        /// <summary>
        /// 根据单据来源和状态获取入库单
        /// </summary>
        /// <param name="sourceType">单据来源</param>
        /// <param name="sourceNo">单据编号</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        /// <remarks>2014-04-11 朱成果 创建 </remarks>
        public abstract WhStockIn GetStockInBySourceAndStatus(int sourceType, int sourceNo, int? Status);


        public abstract List<WhStockIn> GetStockInListByDate(DateTime dateTime);
    }
}
