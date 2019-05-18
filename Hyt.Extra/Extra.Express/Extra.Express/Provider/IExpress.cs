using Extra.Express.Model;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.ExpressList;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Express.Provider
{
    /// <summary>
    /// 快递接口
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public abstract class IExpress
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
        /// 获取快递单号
        /// </summary>
        /// <param name="ro">快递接口参数</param>
        /// <returns></returns>
        /// <remarks>2017-12-13 杨浩 创建</remarks>
        public abstract Result<string> GetCourierNo(ExpressParameters param);
        /// <summary>
        /// 跨境订单下单
        /// </summary>
        /// <param name="twbnr">参数</param>
        /// <returns></returns>
        /// <remarks> 2017-12-13 廖移凤</remarks>
        public abstract Result CreateCrossOrder(ExpressParameters param);

       
     
    }
}
