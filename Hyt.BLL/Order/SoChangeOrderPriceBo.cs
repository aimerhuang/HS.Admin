using Hyt.DataAccess.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Order
{
    public class SoChangeOrderPriceBo : BOBase<SoChangeOrderPriceBo>
    {
        /// <summary>
        /// 添加订单调价的功能
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int Insert(Model.Generated.SoChangeOrderPrice mod)
        {
            return ISoChangeOrderPriceDao.Instance.Insert(mod);
        }
        /// <summary>
        /// 更新订单调价的功能
        /// </summary>
        /// <param name="mod"></param>
        public  void Update(Model.Generated.SoChangeOrderPrice mod)
        {
            ISoChangeOrderPriceDao.Instance.Update(mod);
        }
        /// <summary>
        /// 删除订单调价的功能
        /// </summary>
        /// <param name="SysNo"></param>
        public  void Delete(int SysNo)
        {
            ISoChangeOrderPriceDao.Instance.Delete(SysNo);
        }
        /// <summary>
        /// 获取订单调价的功能
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public  Model.Generated.SoChangeOrderPrice GetDataByOrderSysNo(int OrderSysNo)
        {
            return ISoChangeOrderPriceDao.Instance.GetDataByOrderSysNo(OrderSysNo);
        }
    }
}
