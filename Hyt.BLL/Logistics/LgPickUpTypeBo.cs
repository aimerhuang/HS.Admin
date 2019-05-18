using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 取货类型业务
    /// </summary>
    /// <remarks>2013-08-13 周唐炬 创建</remarks>
    public class LgPickUpTypeBo : BOBase<LgPickUpTypeBo>
    {
        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public LgPickupType GetPickupType(int pickupTypeSysNo)
        {
            return DataAccess.RMA.ILgPickUpDao.Instance.GetPickupType(pickupTypeSysNo);
        }

        /// <summary>
        /// 获取取件方式名称
        /// </summary>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns>获取取件方式名称</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public string GetPickupTypeName(int pickupTypeSysNo)
        {
            var pickupType = GetPickupType(pickupTypeSysNo);
            return pickupType == null ? "未知取件方式" : pickupType.PickupTypeName;
        }

        /// <summary>
        /// 获取所有取件方式
        /// </summary>
        /// <returns>所有取件方式</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public List<LgPickupType> GetLgPickupTypeList()
        {
            return DataAccess.RMA.ILgPickUpDao.Instance.GetLgPickupTypeList();
        }
    }
}
