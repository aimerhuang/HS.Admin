using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Finance
{
    /// <summary>
    /// 收款单
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public abstract class IFnReceiptVoucherDao : DaoBase<IFnReceiptVoucherDao>
    {
        /// <summary>
        /// 添加收款单
        /// </summary>
        /// <param name="entity">收款单实体</param>
        /// <returns>收款单编号</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public abstract int Insert(FnReceiptVoucher entity);

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="source">收款单来源</param>
        /// <param name="sourceSysNo">收款单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public abstract FnReceiptVoucher GetEntity(int source, int sourceSysNo);

        /// <summary>
        /// 获取收款单详情(不包括明细)
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns>收款单详情</returns>
        /// <remarks>2013-7-22 余勇 创建 </remarks>
        public abstract CBFnReceiptVoucher GetEntity(int sysNo);
       
        /// <summary>
        /// 更新收款单
        /// </summary>
        /// <param name="entity">收款单</param>
        /// <returns></returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public abstract void Update(FnReceiptVoucher entity);

        /// <summary>
        /// 删除收款单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public abstract void DeleteBySource(int SourceSysNo, int Source);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        public abstract Pager<CBFnReceiptVoucher> GetAll(ParaVoucherFilter filter);

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-30 黄伟 创建</remarks>
        public abstract FnReceiptVoucher GetReceiptVoucherByOrder(int orderSysNo);

        /// <summary>
        /// 分页查询收款单
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="currentUserSysNo">当前登录用户系统编号</param>
        /// <returns>查询收款单列表</returns>
        /// <remarks>2013-10-14 沈强 修改</remarks>
        public abstract Pager<CBFnReceiptVoucher> GetFnReceipt(ParaWarehouseFilter filter, int currentUserSysNo);

    }
}
