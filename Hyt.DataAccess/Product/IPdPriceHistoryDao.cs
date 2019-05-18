using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品历史调价接口
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    public abstract class IPdPriceHistoryDao : DaoBase<IPdPriceHistoryDao>
    {
        /// <summary>
        /// 保存调价申请
        /// </summary>
        /// <param name="priceHistory">调价申请对象</param>
        /// <returns>是否保存成功</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        public abstract bool SavePdPriceHistory(params PdPriceHistory[] priceHistory);

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">审批状态</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="count">抛出总数</param>
        /// <param name="productName">商品名称</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        public abstract IList<CBPdPriceHistory> GetPriceHistorieList(int pageIndex, int pageSize, int status, int? erpCode, out int count, string productName = null);

        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-18 杨晗 创建</remarks>
        public abstract IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="relationCode">关系码</param>
        /// <param name="opinion">意见</param>
        /// <param name="status">状态</param>
        /// <param name="auditor">审批人</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Update(string relationCode, string opinion, int status, int auditor);
    }
}
