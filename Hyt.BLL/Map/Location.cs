using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hyt.Model.Map;
using Hyt.Model;

namespace Hyt.BLL.Map
{
    /// <summary>
    /// 位置信息类
    /// </summary>
    /// <remarks>2013-12-16 黄波 创建</remarks>
    public class LocationBo : BOBase<LocationBo>
    {
        /// <summary>
        /// 获取IP地址所在位置
        /// 地区->城市->国家(取精确位置)
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>IP所在位置</returns>
        /// <remarks>2013-12-16 黄波 创建</remarks>
        /// <remarks>2014-02-07 邵斌 修改：因第三方服务请链接不稳定，进行兼容处理</remarks>
        public string GetIPCity(string ipAddress)
        {
                var location = GetIPLocation(ipAddress);
                if (!string.IsNullOrWhiteSpace(location.County)) return location.County;
                if (!string.IsNullOrWhiteSpace(location.City)) return location.City;
                if (!string.IsNullOrWhiteSpace(location.Region)) return location.Region;
                return null;            //兼容错误 2014-02-07 邵斌 修改: 原来默认返回为""（空字符串）这样将影响后面判断地址是否成功应为后面用“??”运算符他不判读空字符串。
        }

        /// <summary>
        /// 获取IP地址所在位置
        /// 地区->城市->国家(取精确位置)
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="defArea">默认区域</param>
        /// <returns>IP所在位置</returns>
        /// <remarks>2013-12-16 黄波 创建</remarks>
        public string GetIPCity(string ipAddress,string defArea)
        {
            return GetIPCity(ipAddress) ?? defArea;
        }

        /// <summary>
        /// 获取IP地址所在位置
        /// 省份->城市->区县(取大概位置)
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>IP所在位置</returns>
        /// <remarks>2013-12-16 黄波 创建</remarks>
        public string GetIPRegion(string ipAddress)
        {
            var location = GetIPLocation(ipAddress);
            if (!string.IsNullOrWhiteSpace(location.Region)) return location.Region;
            if (!string.IsNullOrWhiteSpace(location.City)) return location.City;
            if (!string.IsNullOrWhiteSpace(location.County)) return location.County;
            return "";
        }

        /// <summary>
        /// 根据IP地址获取该IP所在位置信息
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>IP所在位置</returns>
        /// <remarks>2013-12-16 黄波 创建</remarks>
        /// <remarks>2014-02-07 邵斌 修改：因第三方服务请链接不稳定，进行兼容处理</remarks>
        private IPLocation GetIPLocation(string ipAddress)
        {
            var result = new IPLocation();

            try
            {
                var jsonStr =
                    NetUtil.GetHttpRequest(string.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", ipAddress));
                var jsonObject = JObject.Parse(jsonStr);
                if (jsonObject != null && (jsonObject.Property("code") != null && jsonObject["code"].ToString() == "0"))
                {
                    result.Area = jsonObject["data"]["area"].ToString();
                    result.City = jsonObject["data"]["city"].ToString();
                    result.Country = jsonObject["data"]["country"].ToString();
                    result.County = jsonObject["data"]["county"].ToString();
                    ;
                    result.Region = jsonObject["data"]["region"].ToString();
                }
            }
            catch
            {
                //兼容错误 2014-02-07 邵斌 修改
                return result;
            }
            return result;
        }
    }
}
