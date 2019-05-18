using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Finance;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Finance
{
    /// <summary>
    /// 收款科目关联
    /// </summary>
    /// <remarks>2013-11-11  朱成果 创建</remarks>
    public  class FnReceiptTitleAssociationDaoImpl:IFnReceiptTitleAssociationDao
    {

        /// <summary>
        /// 获取收款科目关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="PaymentTypeSysNo">支付方式编号</param>
        /// <returns>收款科目关联列表</returns>
        /// <remarks>
        /// 2013-11-11 朱成果 创建
        /// </remarks>
        public override List<FnReceiptTitleAssociation> GetList(int warehouseSysNo, int paymentTypeSysNo)
        {
          return  
                 Context.Sql("select * from FnReceiptTitleAssociation where WarehouseSysNo=@warehouseSysNo and PaymentTypeSysNo=@paymentTypeSysNo order by IsDefault desc")
                .Parameter("warehouseSysNo", warehouseSysNo)
                .Parameter("paymentTypeSysNo", paymentTypeSysNo).QueryMany<FnReceiptTitleAssociation>();
        }
    }
}
