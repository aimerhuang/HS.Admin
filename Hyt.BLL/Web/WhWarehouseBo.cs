using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 仓库业务逻辑
    /// </summary>
    /// <remarks></remarks>
    public class WhWarehouseBo : BOBase<WhWarehouseBo>
    {
        /// <summary>
        /// 根据用户区域编号和取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public IList<CBWhWarehouse> GetWarehouse(int areaSysNo)
        {
            return Hyt.Infrastructure.Caching.CacheManager.Get<IList<CBWhWarehouse>>(
                     Infrastructure.Caching.CacheKeys.Items.WhWarehouseListByArea_, areaSysNo.ToString() , delegate()
                         {
                             return Hyt.DataAccess.Web.IWhWarehouseDao.Instance.GetWarehouse(areaSysNo);
                         }
                );
        }

        /// <summary>
        /// 周围是否有仓库支持上门取货
        /// </summary>
        /// <param name="areaSysNo"></param>
        /// <param name="pickupType">取件方式</param>
        /// <returns>返回 true:有仓库取货 false:不支持</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public bool AroundHasWarehouseSupportPickUp(int areaSysNo, int pickupType)
        {
            return Hyt.DataAccess.Web.IWhWarehouseDao.Instance.AroundHasWarehouseSupportPickUp(areaSysNo, pickupType);
        }

        /// <summary>
        /// 周围是否有仓库支持配送方式
        /// </summary>
        /// <param name="areaSysNo"></param>
        /// <param name="lgDeliveryType"></param>
        /// <returns>True：有支持的仓库 False：没有仓库</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public bool AroundHasWarehouseSupportDelivery(int areaSysNo, int lgDeliveryType)
        {
            return Hyt.DataAccess.Web.IWhWarehouseDao.Instance.AroundHasWarehouseSupportDelivery(areaSysNo, lgDeliveryType);
        }


        /// <summary>
        /// 根据仓库ERp编号获取仓库信息
        /// </summary>
        /// <param name="ErpCode"></param>
        /// <returns></returns>
        public  WhWarehouse GetModelErpCode(string ErpCode)
        {
            return Hyt.DataAccess.Web.IWhWarehouseDao.Instance.GetModelErpCode(ErpCode);
        }

        /// <summary>
        /// 查询仓库基本信息
        /// </summary>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public WhWarehouse GetModel(int sysno)
        {
            return Hyt.DataAccess.Web.IWhWarehouseDao.Instance.GetModel(sysno);
        }
    }
}
