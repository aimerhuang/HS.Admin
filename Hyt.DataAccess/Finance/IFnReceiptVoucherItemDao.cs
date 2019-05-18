using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Finance
{
    /// <summary>
    /// 收款明细
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public abstract class IFnReceiptVoucherItemDao : DaoBase<IFnReceiptVoucherItemDao>
    {
        /// <summary>
        /// 插入收款明细
        /// </summary>
        /// <param name="entity">收款明细</param>
        /// <returns></returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public abstract int Insert(FnReceiptVoucherItem entity);

        /// <summary>
        /// 获取收款明细
        /// </summary>
        /// <param name="receiptNo">收款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-17 朱成果 创建</remarks>
        public abstract List<FnReceiptVoucherItem> GetListByReceiptNo(int receiptNo);

        /// <summary>
        /// 获取收款明细
        /// </summary>
        /// <param name="receiptVoucherSysNo">收款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-7-22 余勇 创建 </remarks>
        public abstract List<CBFnReceiptVoucherItem> GetVoucherItems(int receiptVoucherSysNo);

        /// <summary>
        /// 删除收款明细
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 删除收款单明细
        /// </summary>
        /// <param name="fnReceiptVoucherSysNo">收款单系统编号</param>
        /// <remarks>2013-07-26 黄伟 创建</remarks>
        public abstract int DeleteItemsBySysNo(int fnReceiptVoucherSysNo);

        /// <summary>
        /// 作废收款明细
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <param name="status">状态1有效0无效</param>
        /// <returns></returns>
        /// <remarks>2013-08-08 余勇 创建</remarks>
        public abstract int Invalid(int sysNo, int status);

        /// <summary>
        /// 获取收款明细实体
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2013-08-08 余勇 创建</remarks>
        public abstract FnReceiptVoucherItem Get(int sysNo);

        /// <summary>
        /// 修改收款单明细
        /// </summary>
        /// <param name="item">收款单明细实体</param>
        /// <returns>2013-08-07 黄伟 创建</returns>
        public abstract void Update(FnReceiptVoucherItem item);
    }
}
