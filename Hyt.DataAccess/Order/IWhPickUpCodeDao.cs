using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 门店提货验证码
    /// </summary>
    /// <remarks>2013-07-03 朱成果 创建</remarks>
    public abstract class IWhPickUpCodeDao : DaoBase<IWhPickUpCodeDao>
    {
        /// <summary>
        /// 获取提货验证码
        /// </summary>
        /// <param name="stockOutSysNo">出库单号</param>
        /// <returns></returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks>
        public abstract WhPickUpCode GetEntityByStockOutNo(int stockOutSysNo);

        /// <summary>
        /// 插入验证码数据
        /// </summary>
        /// <param name="entity">验证码实体</param>
        /// <returns></returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks> 
        public abstract int InsertEntity(WhPickUpCode entity);

        /// <summary>
        /// 更新提货验证码数据
        /// </summary>
        /// <param name="entity">提货验证码实体</param>
        /// <returns></returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks> 
        public abstract  void  UpdateEntity(WhPickUpCode entity);

        /// <summary>
        /// 提货码及验证码分页查询
        /// </summary>
        /// <param name="pager">分页列表</param>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2013-12-3 余勇 创建</remarks>
        public abstract void GetPickUpSmsList(ref Pager<CBWhPickUpCode> pager, ParaWhPickUpCodeFilter filter);
    }
}
