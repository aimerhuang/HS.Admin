using Hyt.Model.Common;
using Hyt.Model.ExpressList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Express.Provider
{
    /// <summary>
    /// 快递100电子面单接口
    /// </summary>
    /// <remarks> 2017-12-14 廖移凤创建</remarks>
   public abstract class IKd100Express
    {
        /// <summary>
        /// 快递配置
        /// </summary>
        protected ExpressConfig Config
        {
            get
            {
                return Hyt.BLL.Config.Config.Instance.GetExpressConfig();
            }
        }

        /// <summary>
        /// 快递100电子面单
        /// </summary>
        /// <param name="pms">参数</param>
        /// <returns></returns>
        /// <remarks> 2017-12-13 廖移凤</remarks>
        public abstract KdOrderNums OrderTracesSubByJson(KdOrderParam pms); 


    }
}
