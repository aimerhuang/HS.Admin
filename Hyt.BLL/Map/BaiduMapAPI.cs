using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hyt.Model.SystemPredefined;
using System.IO;
using Hyt.Model.Map;

namespace Hyt.BLL.Map
{
    /// <summary>
    /// 百度地图API业务逻辑
    /// http://developer.baidu.com/map/lbs-cloud.htm
    /// </summary>
    /// <remarks>2013-08-27 黄波 创建</remarks>
    public class BaiduMapAPI : BOBase<BaiduMapAPI>
    {
        #region 地理位置解析
        /// <summary>
        /// 地理位置解析
        /// 传入详细地址,返回地址经纬度信息
        /// </summary>
        /// <param name="address">详细地址</param>
        /// <param name="city">城市名称,用于精确查找,可以不传</param>
        /// <return>解析后的结果</return>
        /// <remarks>2013-08-27 黄波 创建</remarks>
        /// <remarks>2013-12-13 邵斌 优化百度API调用返回空的情况</remarks>
        public GeocoderResult Geocoder(string address, string city = "")
        {
            GeocoderResult returnValue = null;
            var apiURL = string.Format("http://api.map.baidu.com/geocoder/v2/?address={0}&city={1}&ak={2}&output={3}"
                , HttpUtility.UrlEncode(address.Trim())
                , HttpUtility.UrlEncode(city.Trim())
                , Constant.BAIDU_MAP_AK
                , "json");
            try
            {
                var webRequest = HttpWebRequest.Create(apiURL);
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);

                // 2013-12-13 邵斌 优化百度API调用返回空的情况
                if (jsonObject != null)
                {
                    //API说明 http://developer.baidu.com/map/webservice-geocoding.htm#.E6.8E.A5.E5.8F.A3.E5.8F.82.E6.95.B6
                    if ((jsonObject.Property("status") != null && jsonObject["status"].ToString() == "0")
                        && jsonObject["result"]["location"] != null)
                    {
                        returnValue = new GeocoderResult
                        {
                            Confidence = Convert.ToInt32(jsonObject["result"]["confidence"].ToString()),
                            Lat = Convert.ToDouble(jsonObject["result"]["location"]["lat"].ToString()),
                            Lng = Convert.ToDouble(jsonObject["result"]["location"]["lng"].ToString()),
                            Level = jsonObject["result"]["level"].ToString(),
                            Precise = Convert.ToInt32(jsonObject["result"]["precise"].ToString()),
                            Status = Convert.ToInt32(jsonObject["status"].ToString())
                        };
                    }
                    else
                    {
                        //TODO:API返回状态错误
                    }

                }

            }
            catch (Exception ex)
            {
                Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, ex.Message, ex);
            }
            return returnValue;
        }
        #endregion
    }
}