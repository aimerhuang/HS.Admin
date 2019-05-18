using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 请求数据基类
    /// </summary>
    /// <remarks>2016-11-13 杨浩 创建</remarks>
    public class BaseRequest
    {
        /// <summary>
        /// 身份key值
        /// </summary>
        public string APP_Key
        {
            get { return ErpConfigs.Instance.GetErpConfig().AppKey; }
        }
        /// <summary>
        /// 身份效验码
        /// </summary>
        public string APP_scode
        {
            get { return ErpConfigs.Instance.GetErpConfig().AppScode; }
        }      
    }
}
