using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 二次销售数据访问层
    /// </summary>
    /// <remarks>2014-9-23  朱成果 创建</remarks>
    public abstract class ITwoSaleDao : DaoBase<ITwoSaleDao>
    {
        /// <summary>
        /// 添加业务员二次销售收款记录
        /// </summary>
        /// <param name="model">模型</param>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        public abstract void InsertTwoSaleCashHistory(Rp_业务员二次销售 model);

    }

}
