using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Finance
{
    /// <summary>
    /// 收款科目关联
    /// </summary>
    ///<remarks> 2013-11-11 朱成果 创建</remarks>
    public abstract class IFnReceiptTitleAssociationDao : DaoBase<IFnReceiptTitleAssociationDao>
    {
        /// <summary>
        /// 获取收款科目关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="PaymentTypeSysNo">支付方式编号</param>
        /// <returns>收款科目关联列表</returns>
        ///<remarks> 2013-11-11 朱成果 创建</remarks>
        public abstract List<FnReceiptTitleAssociation> GetList(int warehouseSysNo, int paymentTypeSysNo);
    }
}
