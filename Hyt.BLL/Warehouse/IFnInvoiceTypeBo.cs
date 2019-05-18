using Hyt.Infrastructure.Pager;
using Hyt.Model;

namespace Hyt.BLL.Warehouse
{
    public interface IFnInvoiceTypeBo
    {
        /// <summary>
        /// 根据系统编号获取发票类型
        /// </summary>
        /// <param name="sysno">发票类型系统编号.</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-07-10 周瑜 创建 </remarks>
        FnInvoiceType GetModel(int sysno);
    }
}
