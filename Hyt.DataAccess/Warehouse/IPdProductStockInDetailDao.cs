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
    /// 运费模板 抽象类
    /// </summary>
    /// <remarks>
    /// 015-08-06 王耀发 创建
    /// </remarks>
    public abstract class IPdProductStockInDetailDao : Hyt.DataAccess.Base.DaoBase<IPdProductStockInDetailDao>
    {
        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="sysNo">运费模板系统编号</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract Pager<PdProductStockInDetailList> GetPdProductStockInDetailList(ParaProductStockInDetailFilter filter);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract PdProductStockInDetailList GetEntity(int sysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(PdProductStockInDetail entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(PdProductStockInDetail entity);
        /// <summary>
        /// 查询当前仓库未入库商品
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public abstract List<PdProduct> GetNotStockInPd(int WarehouseSysNo);
        public abstract void DeleteByProductStockInSysNo(int ProductStockInSysNo);

        public abstract List<PdProductStockInDetailList> GetProductStockInDetailBy(int ProductStockInSysNo);

        public abstract List<PdProductStockInDetailList> GetAduitProductStockInDetailBy(int ProductStockInSysNo);

        public abstract List<PdProductStockInDetail> GetProductStockInDetail(int ProductStockInSysNo);

        /// <summary>
        /// 获得推送入库单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public abstract IList<SendSoOrderModel> GetSendSoOrderModelByStockInSysNo(int ProductStockInSysNo);

        public abstract List<PdProductStock> GetSelectedPdProductStock(List<int> sysNoList);
    }
}

