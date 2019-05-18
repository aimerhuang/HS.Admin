using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商产品等级价格维护
    /// </summary>
    /// <remarks>
    /// 2013-09-04 周瑜 创建
    /// </remarks>
    public abstract class IDsLevelPriceDao : DaoBase<IDsLevelPriceDao>
    {
        /// <summary>
        /// 获取分销商等级
        /// </summary>
        /// <param></param>
        /// <returns>分销商等级</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public abstract IList<DsDealerLevel> GetDsDealerLevel();

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">审批状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <param name="erpCode">商品编号</param>
        /// <param name="count">抛出总数</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public abstract IList<CBPdPriceHistory> GetPriceHistorieList(int pageIndex, int pageSize, int? status, int? sysNo, string erpCode, out int count);
        
        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public abstract IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="relationCode">关系码</param>
        /// <param name="opinion">意见</param>
        /// <param name="status">状态</param>
        /// <param name="auditor">审批人</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public abstract int Update(string relationCode, string opinion, int status, int auditor);
    }
}
