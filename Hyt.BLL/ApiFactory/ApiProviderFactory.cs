using Hyt.BLL.ApiCustoms;
using Hyt.BLL.ApiElectricBusiness;
using Hyt.BLL.ApiIcq;
using Hyt.BLL.ApiLogistics;
using Hyt.BLL.ApiPay;
using Hyt.BLL.ApiSupply;
using Hyt.BLL.ApiWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiFactory
{
    /// <summary>
    /// api工厂类
    /// </summary>
    /// <remarks>2015-12-26 杨浩 创建</remarks>
    public class ApiProviderFactory 
    {      
        /// <summary>
        /// 获取三方物流API的类实例
        /// </summary>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        ///<remarks>2015-10-12 杨浩 创建</remarks>
        public static ILogisticsProvider GetLogisticsInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<ILogisticsProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiLogistics");
        }

        /// <summary>
        /// 获取支付API的类实例
        /// </summary>
        /// <param name="code">支付企业内部代码</param>
        /// <returns></returns>
        ///<remarks>2015-10-12 杨浩 创建</remarks>
        public static IPayProvider GetPayInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<IPayProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiPay");
        }
        /// <summary>
        /// 获取海关的类实例
        /// </summary>
        /// <param name="code">海关内部代码</param>
        /// <returns></returns>
        ///<remarks>2016-1-2 杨浩 创建</remarks>
        public static ICustomsProvider GetCustomsInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<ICustomsProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiCustoms");
        }

        /// <summary>
        /// 获取供应链的类实例
        /// </summary>
        /// <param name="code">供应链内部代码</param>
        /// <returns></returns>
        ///<remarks>2016-3-8 杨浩 创建</remarks>
        public static ISupplyProvider GetSupplyInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<ISupplyProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiSupply");
        }

        /// <summary>
        /// 获取商检API的类实例
        /// </summary>
        /// <param name="code">商检代码</param>
        /// <returns></returns>
        ///<remarks>2016-3-19 杨浩 创建</remarks>
        public static IIcqProvider GetIcqInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<IIcqProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiIcq");
        }

        /// <summary>
        /// 获取电商API的类实例
        /// </summary>
        /// <param name="code">电商平台代码</param>
        /// <returns></returns>
        ///<remarks>2016-3-19 杨浩 创建</remarks>
        public static IElectricBusinessProvider GetElectricBusinessInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<IElectricBusinessProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiElectricBusiness");
        }

        /// <summary>
        /// 获取电商API的类实例
        /// </summary>
        /// <param name="code">电商平台代码</param>
        /// <returns></returns>
        ///<remarks>2016-3-19 杨浩 创建</remarks>
        public static IWarehouseProvider GetWarehousInstance(int code)
        {
            return Hyt.BLL.Base.InstanceProviderBo<IWarehouseProvider>.GetInstance(code.ToString(), "Hyt.BLL.ApiWarehouse");
        }
    }
}