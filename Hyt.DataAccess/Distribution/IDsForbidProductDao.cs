using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 禁止升舱管理接口层
    /// </summary>
    /// <remarks>
    /// 2014-03-21 余勇 创建
    /// </remarks>
    public abstract class IDsForbidProductDao : DaoBase<IDsForbidProductDao>
    {
        #region 操作

        /// <summary>
        /// 创建禁止升舱商品
        /// </summary>
        /// <param name="model">禁止升舱商品</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2014-03-21 余勇 创建
        /// </remarks>
        public abstract int Create(DsForbidProduct model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>
        /// 2014-03-21 余勇 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 通过商品编号取得实体
        /// </summary>
        /// <param name="sysNo">商品编号</param>
        /// <returns>禁止升舱商品</returns>
        /// <remarks>
        /// 2014-03-21 余勇 创建
        /// </remarks>
        public abstract DsForbidProduct GetByProductSysNo(int sysNo);

        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取禁止升舱商品列表
        /// </summary>
        /// <param name="product">商品名称或编号</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页列表</returns>
        ///<remarks>2014-03-21 余勇 创建</remarks>
        public abstract Pager<DsForbidProduct> Query(string product, int currentPage, int pageSize);

        #endregion
    }
}
