using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Module
{
    /// <summary>
    /// 选择会员
    /// </summary>
    /// <remarks>2013-06-19 吴文强 创建</remarks>
    public interface ISelectCustomerBo
    {
        /// <summary>
        /// 根据筛选条件获取会员集合
        /// </summary>
        /// <param name="filter">筛选条件(手机/账号/姓名任选其一,精准查询)</param>
        /// <returns>会员集合</returns>
        /// <remarks>2013-06-19 吴文强 创建</remarks>
        IList<CrCustomer> GetCustomers(string filter);
    }
}
