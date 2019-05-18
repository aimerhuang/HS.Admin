using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.SystemPredefined;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 仓库接口
    /// </summary>
    /// <remarks>2013-08-28 邵斌 创建</remarks>
    public abstract class IWhWarehouseDao : DaoBase<IWhWarehouseDao>
    {
        /// <summary>
        /// 根据用户区域编号和取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <returns>返回仓库列表和联系人等</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public abstract IList<CBWhWarehouse> GetWarehouse(int areaSysNo);

        /// <summary>
        /// 周围是否有仓库支持取货方式
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="pickupType">取件方式</param>
        /// <returns>返回 true:有仓库取货 false:不支持</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public abstract bool AroundHasWarehouseSupportPickUp(int areaSysNo,int pickupType);

        /// <summary>
        /// 周围是否有仓库支持配送方式
        /// </summary>
        /// <param name="areaSysNo"></param>
        /// <param name="lgDeliveryType"></param>
        /// <returns>返回 True:支持配送方式 False:不支持配送</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public abstract bool AroundHasWarehouseSupportDelivery(int areaSysNo, int lgDeliveryType);

        /// <summary>
        /// 读取默认仓库（暂定为成都）
        /// </summary>
        /// <param name="isPickUp">是否是取货仓库： true 取货，false 收货</param>
        /// <param name="optionType">取送货类型</param>
        /// <returns>返回仓库系统编号</returns>
        /// <remarks>2013-09-05 邵斌 创建</remarks>
        public abstract int GetDefaultWarehouse(bool isPickUp, int optionType);


        /// <summary>
        /// 根据仓库ERp编号获取仓库信息
        /// </summary>
        /// <param name="ErpCode"></param>
        /// <returns></returns>
        public abstract WhWarehouse GetModelErpCode(string ErpCode);


        /// <summary>
        /// 查询仓库基本信息
        /// </summary>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public abstract WhWarehouse GetModel(int sysno);
    }
}
