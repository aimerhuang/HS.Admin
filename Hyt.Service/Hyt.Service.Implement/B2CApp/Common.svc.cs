using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Hyt.BLL.Basic;
using Hyt.BLL.Feedback;
using Hyt.BLL.Front;
using Hyt.BLL.Version;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.B2CApp;
using Hyt.Util.Log;

namespace Hyt.Service.Implement.B2CApp
{
    public class Common : BaseService, ICommon
    {
        /// <summary>
        /// 获取版本更新
        /// </summary>
        /// <returns>版本更新信息</returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        /// <remarks>2013-08-20 周唐炬 实现</remarks>
        public Result<Model.B2CApp.Version> GetVersion(AppEnum.AppType type)
        {
            var result = new Result<Model.B2CApp.Version>() {StatusCode = -1};
            var data = VersionBo.Instance.GetApVersion(type);
            if (null != data)
            {
                var model = new Model.B2CApp.Version()
                    {
                        VersionNumber = data.VersionNumber,
                        VersionLink = data.VersionLink,
                        UpgradeInfo = data.UpgradeInfo
                    };
                result.Data = model;
                result.Status = true;
                result.StatusCode = 0;
            }
            else
                result.Message = "暂无最新更新！";
            return result;
        }

        /// <summary>
        /// 获取帮助信息
        /// </summary>
        /// <returns>帮助信息</returns>
        /// <remarks> 2013-7-15 杨浩 创建</remarks>
        /// <remarks>2013-08-19 周唐炬 实现</remarks>
        public Result<IList<FeArticle>> GetHelps()
        {
            var result = new Result<IList<FeArticle>>() {StatusCode = -1};
            var category = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.APP帮助).FirstOrDefault();
            if (null != category)
            {
                var list = FeArticleBo.Instance.GetListByCategory(category.SysNo);
                if (null != list)
                {
                    result.Data = list;
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = "暂无数据!";
                }
            }
            return result;
        }

        /// <summary>
        /// 更新用户网络类型
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="networkType">网络类型</param>
        /// <returns>更新结果</returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        public Result UpdateNetworkType(string account, AppEnum.NetworkType networkType)
        {
            return new Result {Status = true, StatusCode = 1};
        }

        /// <summary>
        /// 获取反馈类型
        /// </summary>
        /// <param name="source">来源:商城(10),IphoneApp(20),AndroidApp(30)</param>
        /// <returns>反馈类型</returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        /// <remarks>2013-08-19 周唐炬 实现</remarks>
        public Result<IList<FeedbackType>> GetFeedbackType(CustomerStatus.意见反馈类型来源 source)
        {
            var result = new Result<IList<FeedbackType>>() {StatusCode = -1};
            var list = FeedbackTypeBo.Instance.GetFeedbackTypeList(source);
            if (list != null)
            {
                result.Data = list.Select(x => new FeedbackType
                    {
                        SysNo = x.SysNo,
                        Name = x.Name
                    }).ToList();
                result.StatusCode = 0;
                result.Status = true;
            }
            else
            {
                result.Message = "暂无数据!";
            }
            return result;
        }

        /// <summary>
        /// 新增反馈信息
        /// </summary>
        /// <param name="feedback">反馈信息</param>
        /// <returns>结果</returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        /// <remarks>2013-08-19 周唐炬 实现</remarks>
        public Result AddFeedback(Feedback feedback)
        {
            var model = new CrFeedback
                {
                    CustomerSysNo = feedback.CustomerSysNo,
                    FeedbackTypeSysNo = feedback.FeedbackTypeSysNo,
                    Name = feedback.Name,
                    Phone = feedback.Phone,
                    Email = feedback.Email,
                    Content = feedback.Content,
                    Source = feedback.Source,
                    CreatedDate = DateTime.Now
                };
            var result = new Result() {StatusCode = -1};
            model.CreatedDate = DateTime.Now;
            if (FeedbackBo.Instance.Create(model) > 0)
            {
                result.StatusCode = 1;
                result.Status = true;
            }
            else
            {
                result.Message = "添加反馈信息出错,请稍后在试!";
            }
            return result;
        }

        /// <summary>
        /// 获取省市区
        /// </summary>
        /// <param name="parentSysNo">区域上级系统号</param>
        /// <returns>省市区数据列表</returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        public Result<IList<BsArea>> GetArea(int parentSysNo)
        {
            var data = BasicAreaBo.Instance.SelectArea(parentSysNo);

            return new Result<IList<BsArea>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取附近门店信息
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-7-15 杨浩 创建 
        /// 2013-09-11 郑荣华 实现
        /// </remarks>
        public ResultPager<IList<NWarehouse>> GetNearbyStores(double longitude, double latitude)
        {
            //0.1°=11.131955km,0.05°=5565m ,只搜索经纬都在0.05°以内的门店
            var list = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseByMap(latitude, longitude, 0.05);
            if (null == list || list.Count < 1)
            {
                return new ResultPager<IList<NWarehouse>>
                    {
                        Message = "附近未找到门店",
                        Data = null,
                        Status = false,
                        HasMore = false
                    };
            }
            var newdata = new List<NWarehouse>();
            foreach (var item in list)
            {
                var d = new NWarehouse();
                double lat = item.Latitude;
                double lon = item.Longitude;
                string agent = HttpContext.Current.Request.UserAgent;
                if (agent != null && (agent.Contains("iPhone") || agent.Contains("iPad") || agent.Contains("iPod")))
                {
                    bd_decrypt(item.Latitude, item.Longitude, out lat, out lon);
                }

                d.Longitude = item.Longitude;
                d.Latitude = item.Latitude;
                d.WarehouseName = item.WarehouseName;
                d.AreaName = item.AreaName;
                d.StreetAddress = item.StreetAddress;
                d.Phone = item.Phone;
                d.Distance = item.Distance;
                d.SysNo = item.SysNo;
                newdata.Add(d);
            }

            return new ResultPager<IList<NWarehouse>>
                {
                    Data = newdata,
                    Status = true,
                    HasMore = false
                };
        }

        //百度地图坐标转谷歌地图坐标
        private void bd_decrypt(double bd_lat, double bd_lon,out double gg_lat,out double gg_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x*x + y*y) - 0.00002*Math.Sin(y*Math.PI);
            double theta = Math.Atan2(y, x) - 0.000003*Math.Cos(x*Math.PI);
            gg_lon = z*Math.Cos(theta);
            gg_lat = z*Math.Sin(theta);
        }

        /// <summary>
        /// 获取支付宝配置信息
        /// </summary> 
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        public Result<AlipayConfig> GetAlipayConfig()
        {
            //以下配置信息真实暂不改动
            var data = new AlipayConfig
                {
                    #region 测试支付账号
                    //Partner = "2088011259183897",
                    //RsaAlipayPublic = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDTCiTXjQfWwAjq/ELWrzLZY0JWwrJ571GUTdF9 kIwe0a1UYQWtJl3vanZzG0hjkaZ72DYchf96rj3N+oT6ODsEG/O/dvIgQ7u654iCvmFkPChZwj1v UlbfnyW83L/UoyEdMO6aUJlJ75PH/pWifDwlSuhlnL6TbgBkHix/2kl+0QIDAQAB",
                    //RsaPrivate = @"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAOJH/vVAARaFMdIIwNtpZ2Yxg8KS1fabOuffucw7xVDM+qAF8qyw2UyYbfihTJI//pprr+jQHpBQCv8CckAtqb2HIY65jvY728bMYrYpmmxE83bo2p/982JHsIobqvCQQmYBpPzWp6gyh3ZHvXq6AN/JjbUfeGFxafCIIgzg4S95AgMBAAECgYEA02nu1Nby7UaOsK0K+zS0ra9PaohkLh8EX0YRQrcJeJ+6/a3w+NXWgJDeFnElqHLv/vjkBsID/FM8ott3QW0oazDCbdQumJeiPRYCwhUN04NfMJIBt8sF549uGTlgbWmCY7znu6wHWcOCfZMnG8Impot6ZaW/JDginwXer983zF0CQQDziErV1FLDjzJ77OqmIhE5mI67uj+DkHnlTDDvFPXNS3KyRvt+Tmw2vhiU3aR9QL/qg6F044txQBgoSxtUgOOLAkEA7d2cpbQr8hoHhqhbtpPkjY696S1uMUWxhXVh+iUXnTl/IVigT9YQYKDpUAkJJImJMQS6m86YVebP9uz0/mlJiwJAPf1RX0CemJzYsubJi+RcEhcdLffotuqNvKo6QoFJWCp+VZbv6WS48u5Mf4gzmJZkw6Mh9Aj/s6InBAqqOEZepQJARvPtDyygUzZaeltsCBkzetpSYeTQUthELNgn4rL/yUFmX68NGuNyvTAiiGI5nPF9v+Z2N5W3sSAJdtGV/vGvIwJAcnXVFf5c4X7H70SZg7/swWOa3TtqF9VF7dDrUGE+thIVE2MozEQjOYYEejjE1mQzPzsfvZMOZ8apF1VhrCHq+Q==",
                    //Seller = "wx@pisen.com.cn",
                    //AliNotifyUrl = "http://web.v3.huiyuanti.com:9211/OnlinePay/AliPay_AsyncReturn_App"
                    #endregion

                    #region 正式支付账号

                    Partner = "2088801438289580",
                    Seller = "huiyuanti@163.com",
                    RsaAlipayPublic =
                        "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD5wDXJVH0PJo/1/cjLjRdRKnLNpGczx1UQhfd2 v7OMssbtqN5kwq3MoFMP5sjbmmY6+aJHfHUV9LrX1+1I5ptkG/j0XBE1X3OIcEDY3UdV5z/8Erjz ++eQwEn0aOY59BNTXm8AhsDg1NXCbf6ohaIIZJzk+p18NuP/PPh84o0SxQIDAQAB",
                    RsaPrivate =
                        "MIIBVQIBADANBgkqhkiG9w0BAQEFAASCAT8wggE7AgEAAkEA14eQ4CkjCrm7TbawhZNiQjKIGtM/EgPwPBwO3WHpsYL/PgPeqPTJVdszN8PGxNFhMeafcRcq/r1y1sdyBR3obwIDAQABAkBdR6MAIU21OqVCASts5FjGZbBaS8skNOgoW8xjLBlEZa5UoLYX+f2I8DFBPWOWUTS2ahCsLU8UVneTceQgHQAhAiEA7TcqnSFN3X+SFgjherrsjlXmbbJ6/eUbJyIIifih3N0CIQDomMgw8+vmEgTceanl6JB/d4omEDgSXqWwqpZ52NkvuwIgJoqICe+XTx2jBqF6wQ4N0TJzte8BvK60sq785C3ZXqECIQDn+chfGkcBdKMoiKarx6CloEFF6RMjLwYtBlWBPIaPlQIhAKSE4AyKK3pYFNO5EtJ9PDv5udEFY1BpgdHiSoWpdkkz",
                    AliNotifyUrl = ConfigurationManager.AppSettings["AliNotifyUrl"]

                    #endregion
                };
            return new Result<AlipayConfig>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取启动屏时的广告
        /// </summary>
        /// <param name="type">平台类型：IphoneApp = 31,AndroidApp = 32</param>
        /// <returns></returns>
        /// <remarks> 2014-01-09 杨浩 创建 </remarks>
        public Result<string> GetStartAd(int type)
        {
            var ad = Hyt.BLL.Web.FeAdvertGroupBo.Instance.GetWebAdvertItems(ForeStatus.广告组平台类型.手机商城,"AppStartScreenAd").FirstOrDefault();
            if (ad != null)
            {
                return new Result<string>
                    {
                        Data =FileServer+ ad.ImageUrl,
                        Status = true
                    };
            }

            return new Result<string>
                {
                    Status = false
                };
        }

        /// <summary>
        /// 手机广告列表
        /// </summary>
        /// <param name="type">类型内容</param>
        /// <param name="storeId">门店id</param>
        /// <returns>广告集合</returns>
        public List<Model.FeAdvertItem> GetStartAdvertItems(string type, int storeId)
        {
            IList<Model.FeAdvertItem> iItem=Hyt.BLL.Web.FeAdvertGroupBo.Instance.GetWebAdvertItems(ForeStatus.广告组平台类型.商城AndroidApp, type, storeId);
            List<Model.FeAdvertItem> items = new List<FeAdvertItem>();
            foreach (FeAdvertItem item in iItem)
            {
                items.Add(item);
            }
            return items;
        }
        /// <summary>
        /// 手机广告列表
        /// </summary>
        /// <param name="type">类型内容</param>
        /// <param name="storeId">门店id</param>
        /// <param name="number">数量</param>
        /// <returns>广告集合</returns>
        public List<Model.FeAdvertItem> GetStartAdvertItemsByThrie(string type, int storeId, int number)
        {
            IList<Model.FeAdvertItem> iItem = Hyt.BLL.Web.FeAdvertGroupBo.Instance.GetWebAdvertItems(ForeStatus.广告组平台类型.商城AndroidApp, type, storeId, number);
            List<Model.FeAdvertItem> items = new List<FeAdvertItem>();
            foreach (FeAdvertItem item in iItem)
            {
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// 获取商品数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="customerLevel">等级</param>
        /// <returns>2016-07-19 杨云奕 创建</returns>
        public List<Model.PdProductIndex> GetSaleGoodsPdProductIndex(string type, int customerLevel, int storeId, int totalNum)
        {
            return Hyt.BLL.Web.FeProductItemBo.Instance.GetFeProductItems(type, customerLevel, storeId).Take(totalNum).ToList();//爆款热卖（沙总）
        }
    }
}
