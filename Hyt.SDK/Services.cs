using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyt.SDK
{
    /// <summary>
    /// 服务
    /// </summary>
    /// <remarks>
    /// 客户端需要在web.config appSettings节点中配置"SDK_HytApiUrl"对应的值 只需要配置域名
    /// </remarks>
    public static class Services
    {        
        private static readonly string _apiUrl = ConfigurationManager.AppSettings["SDK_HytApiUrl"];
        //private static readonly string _key = "HFUYF^&^&^%";  //秘匙 关于请求加密未实现 不管是否加密不影响接口的调用方式
        
        /// <summary>
        /// 升舱订单处理
        /// </summary>
        /// <param name="orderSysNo">升舱订单的系统编号</param>
        /// <returns></returns>
        public static bool UpgradeOrderHandler(string orderSysNo) {

            var param = new Dictionary<string,string>();
            param.Add("orderSysNo", orderSysNo);

            var result = NetWork.MakeRequest(GetUrl("UpgradeOrderHandler"), param, null, "get", "http");

            //System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "sdk.log", orderSysNo + ":" + result.Msg + Environment.NewLine);

            if (result.Msg.ToLower() == "ok") return true;

            return false;
        }


        /// <summary>
        /// 获取具体服务对应的URL
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        private static string GetUrl(string name) {
            return string.Format("{0}/api/{1}", _apiUrl, name).Replace("//api/", "/api/");
        }


    }
}
