using System.Collections.Generic;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 发票相关业务处理
    /// </summary>
    /// <remarks>2013-07-09 周瑜 创建</remarks>
    public class FnInvoiceTypeBo : BOBase<FnInvoiceTypeBo>, IFnInvoiceTypeBo
    {
        /// <summary>
        /// 根据主键获取发票
        /// </summary>
        /// <param name="sysno">发票系统编号.</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-07-10 周瑜 创建 </remarks>
        public FnInvoiceType GetModel(int sysno)
        {
            return IFnInvoiceTypeDao.Instance.GetModel(sysno);
        }

        /// <summary>
        /// 获取所有发票类型
        /// </summary>
        /// <returns>所有发票类型</returns>
        /// <param></param>
        /// <remarks>2014-01-07 周唐炬 创建</remarks>
        public IList<FnInvoiceType> GetAll()
        {
            return IFnInvoiceTypeDao.Instance.GetAll();
        }
    }
}
