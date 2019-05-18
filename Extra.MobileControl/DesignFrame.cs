using Extra.MobileControl.Model;
using Hyt.BLL.Mobile;
using Hyt.Model;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.MobileControl
{
    public class DesignFrame
    {
        /// <summary>
        /// 获取页面的加载数据
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="pageText"></param>
        /// <param name="tipCode"></param>
        /// <param name="paramJsonData"></param>
        /// <returns></returns>
        public object GetDesignPageData(int customerSysNo, string  pageText,string tipCode,string paramJsonData)
        {
            List<Hyt.Model.Mobile.MBDesignFrame> list = MBDesignFrameBo.Instance.GetPageDataList(customerSysNo, pageText, tipCode);
            List<DesignDataMod> designModList = new List<DesignDataMod>();

            JObject jsonObj =null;
            Dictionary<string, JObject> GlobalData = new Dictionary<string, JObject>();
            if (!string.IsNullOrEmpty(paramJsonData))
            {
                jsonObj = JObject.Parse(paramJsonData);
            }
            //string content = System.Web.HttpUtility.UrlDecode("%3Cdiv%20style%3D%22text-align%3Acenter%3B%22%3E%0A%09%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/33b34001cfb2468db99de0867f60e849.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cspan%20style%3D%22line-height%3A1.5%3B%22%3E%3C/span%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/4d72fce3c0f641bea653b306abf2f4e0.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/e452569062b04916802783456e079b7d.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/c543dbef32fe49d1a2f8b88ee6681cb6.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/bfb01744ae2146359c0af7facee33620.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/b6a07fce2d264624a340e1ea46290409.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/ei/201609/f26630e535ad4a69a8a49fd1ab84bf49.jpg%22%20alt%3D%22%22%20/%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/a6e80a1d250d4fc4917b0f99b59a62f2.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3Ca%20href%3D%22http%3A//yoyo2o.com/NewsList/Index%22%20target%3D%22_blank%22%3E%3Cimg%20src%3D%22http%3A//img.yoyo2o.com/image/201608/2e284fa8187642f0a86f612ada39ed75.jpg%22%20alt%3D%22%22%20style%3D%22line-height%3A1.5%3B%22%20/%3E%3C/a%3E%20%0A%3C/div%3E"); 
            foreach(var mod in list )
            {
                if (!mod.DesignType.Equals("GlobalData"))
                {

                    DesignDataMod designMod = new DesignDataMod()
                    {
                        DesignType = mod.DesignType
                    };
                    if (!string.IsNullOrEmpty(mod.DesignDataPathParams) && jsonObj != null)
                    {
                        foreach (var key in jsonObj)
                        {
                            mod.DesignDataPathParams = mod.DesignDataPathParams.Replace("[=" + key.Key + "]", key.Value.ToString());
                        }
                    }
                    string txt = "";
                    if (mod.DataDesignType.Equals("Service") || mod.DataDesignType.Equals("Local"))
                    {
                        if (!string.IsNullOrEmpty(mod.DesignDataPath))
                        {
                            txt = "{" + mod.DesignAttr + ",\"" + mod.DesignDataAttr + "\":" + WebUtil.PostJson(mod.DesignDataPath, mod.DesignDataPathParams) + "}";
                        }
                        else if (!string.IsNullOrEmpty(mod.DesignDataAttr))
                        {
                            txt = "{" + mod.DesignAttr + ",\"" + mod.DesignDataAttr + "\":" + (mod.DesignDataAttr == "text" ? "\"" + mod.DesignDataPathParams + "\"" : mod.DesignDataPathParams) + "}";
                        }
                        else if (string.IsNullOrEmpty(mod.DesignDataAttr) && string.IsNullOrEmpty(mod.DesignDataPath))
                        {
                            txt = "{" + mod.DesignAttr + "}";
                        }
                    }
                    else if (mod.DataDesignType.Equals("Combination"))
                    {
                        JObject tempJsonData = JObject.Parse("{" + mod.DesignAttr+"}");
                        string[] strList = mod.DesignDataAttr.Split(',');
                        string[] valList = mod.BindDesignData.Split(',');
                        for (int i = 0; i < strList.Length; i++)
                        {
                            string valKeyData = valList[i];
                            JObject jsonData = GlobalData[valKeyData.Split('.')[0]];
                            tempJsonData.Add(strList[i],(jsonData.GetValue(valKeyData.Split('.')[1])));
                        }
                        txt = tempJsonData.ToString();
                    }
                    designMod.DesignText = txt;
                    designModList.Add(designMod);
                }
                else
                {
                    if (!string.IsNullOrEmpty(mod.DesignDataPathParams) && jsonObj != null)
                    {
                        foreach (var key in jsonObj)
                        {
                            mod.DesignDataPathParams = mod.DesignDataPathParams.Replace("[=" + key.Key + "]", key.Value.ToString());
                        }
                    }
                    GlobalData.Add(mod.DesignDataAttr, JObject.Parse(WebUtil.PostJson(mod.DesignDataPath, mod.DesignDataPathParams)));
                }
            }
            return designModList;
        }
        /// <summary>
        /// 获取广告图片数据连接
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public object GetFeAdvertImageByGroupSysNo(int groupSysNo,string hostName)
        {
            var list = Hyt.BLL.Web.FeAdvertGroupBo.Instance.GetWebAdvertItemsByGroupSysNo(groupSysNo);
            List<object> adList = new List<object>();
            int indx = 0;
            foreach(var item in list)
            {
                indx++;
                adList.Add(new {
                    Id=indx,
                    Url = item.ImageUrl.ToLower().IndexOf("http://") == -1 ? "http://" + hostName + "/" + item.ImageUrl : item.ImageUrl,
                    Content = item.Content,
                    Type = item.OpenType,
                    LinkUrl = item.LinkUrl
                });
            }
            return adList;
        }
        /// <summary>
        /// 获取广告商品数据
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public object GetFeProductDataByGroupSysNo(int groupSysNo, string hostName)
        {
            var list = Hyt.BLL.Front.FeProductGroupBo.Instance.GetModelByGroupSysNo(groupSysNo);
            List<object> adList = new List<object>();
            int indx = 0;
            foreach (var item in list)
            {
                indx++;
                adList.Add(new
                {
                    Id = indx,
                    SysNo = item.ProductSysNo,
                    ProductName = item.ProductName,
                    ProductImage = item.ProductImage.Replace("{0}", "http://" + hostName + "/").Replace("{1}", "Base"),
                    BasePrice = item.BasicPrice,
                    PromotionType="",
                    PromotionPrice=0,
                    userPrice=item.UserPriceList,
                    levelValues = item.LevelValueList,
                    EventName = "SysNo",
                    EventType = "ProductDetail",
                    OtherEventData=""
                });
            }
            return adList;
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="parentSysNo"></param>
        /// <returns></returns>
        public object GetCategoryList(int parentSysNo)
        {
            var list = Hyt.BLL.Product.PdCategoryBo.Instance.GetCategoryListByParent(new int[] { 0 });
            List<int> parentList = new List<int>();
            foreach(var mod in list)
            {
                parentList.Add(mod.SysNo);
            }
            var childList = Hyt.BLL.Product.PdCategoryBo.Instance.GetCategoryListByParent(parentList.ToArray());
            string jsonData = "";
            jsonData += "[";
            string oneJsonData = "";
            foreach (var mod in list)
            {
                var modChildList = childList.Where(p => p.ParentSysNo == mod.SysNo).ToList();
                if(!string.IsNullOrEmpty(oneJsonData))
                {
                    oneJsonData += ",";
                }
                oneJsonData += "{\"Name\":\"" + mod.CategoryName + "\" , \"list\":[ ";
                string twoJsonData = "";
                foreach (var twoMod in modChildList)
                {
                    if (!string.IsNullOrEmpty(twoJsonData))
                    {
                        twoJsonData += ",";
                    }
                    twoJsonData += "{\"urlText\":\"" + twoMod.CategoryName + "\",\"urlPath\":\"" + twoMod.CategoryImage + "\",\"categorySysNo\":\"" + twoMod.SysNo + "\"}";
                }
                oneJsonData += twoJsonData;
                oneJsonData += "]}";
            }
            jsonData += oneJsonData;
            jsonData += "]";
            return jsonData;
        }
        public object GetProductInfoList(int levelSysNo, int groupSysNo, string hostName)
        {
            var list = Hyt.BLL.Front.FeProductGroupBo.Instance.GetProductInfoList(levelSysNo, groupSysNo);
            List<object> adList = new List<object>();
            int indx = 0;
            foreach (var item in list)
            {
                indx++;
                adList.Add(new
                {
                    SysNo = item.ProductSysNo,
                    ProductName = item.ProductName,
                    ImagePath = item.ProductImage.Replace("{0}", "http://" + hostName + "/").Replace("{1}", "Base"),
                    BaseValue = item.BasicPrice,
                    proTips = "",
                    SaleNumber = 0,
                    SaleValue = Convert.ToDouble(item.UserPriceList),
                    levelValues = item.LevelValueList,
                    Discount = (Convert.ToDecimal(item.UserPriceList) / item.BasicPrice*10).ToString("0.0"),
                    orginImagePath = item.OrginImagePath.ToLower().IndexOf("http://") == -1 ? "http://" + hostName + "/" + item.OrginImagePath : item.OrginImagePath,
                    EventName = "SysNo",
                    EventType = "ProductDetail",
                    OtherEventData=""
                });
            }
            return adList;
        }
        public object GetProductDataBySysNo(int ProSysNo, int levelSysNo, string hostName)
        {
            CBPdProduct product = Hyt.BLL.Product.PdProductBo.Instance.GetProduct(ProSysNo);
            Origin orgin = Hyt.BLL.Basic.OriginBo.Instance.GetEntity(product.OriginSysNo);
            PdProductStatistics statisMod = Hyt.BLL.Product.PdProductStatisticsBo.Instance.Get(ProSysNo);

            PdPrice BaseCusPrice = new PdPrice();
            BaseCusPrice = product.PdPrice.Value.First(p => p.PriceSource == 0 && p.SourceSysNo == 0);
            PdPrice SaleCusPrice = new PdPrice();
            if(levelSysNo>0)
            {
                 SaleCusPrice =  product.PdPrice.Value.First(p => p.PriceSource == 10 && p.SourceSysNo == levelSysNo);
            }
            else
            {
                SaleCusPrice=BaseCusPrice;
            }
            List<object> Statistics= new List<object>();
            Statistics.Add(new { type = "文字", text = "销量", valueText = statisMod.Sales });
            Statistics.Add(new { type = "文字", text = "积分", valueText = Convert.ToInt32(SaleCusPrice.Price) });
            Statistics.Add(new { type = "图文", text = "http://m.yoyo2o.com/theme/images/icon-momey.png", valueText = "分享赚" });
            var article = Hyt.BLL.Front.FeArticleBo.Instance.GetModel(75);
            List<object> ProductDetail = new List<object>();
            ProductDetail.Add(new { title = "图文详情", selected = true, content = (product.ProductDesc) });
            ProductDetail.Add(new { title = "购买须知", selected = false, content = (article != null ? article.Content : "") });

           

            if(product!=null)
            {
                foreach (var img in product.PdProductImage.Value)
                {
                    img.ImageUrl = img.ImageUrl.Replace("{0}", "http://" + hostName + "/").Replace("{1}", "Base");
                }
                List<object> adImages = new List<object>();
                var imgList = product.PdProductImage.Value;
                foreach (var imgMod in imgList)
                {
                    adImages.Add(new
                    {
                        Id = imgMod.SysNo,
                        Url = imgMod.ImageUrl,
                        Content = "",
                        Type = "",
                        LinkUrl = ""
                    });
                }
                return new {
                    ProductName = product.ProductName,
                    OrginImage = orgin.Origin_Img.ToLower().IndexOf("http://") == -1 ? "http://" + hostName + "/" + orgin.Origin_Img : orgin.Origin_Img,
                    SalePrice = SaleCusPrice.Price,
                    BasePrice = BaseCusPrice.Price,
                    TipsData = (SaleCusPrice.Price != BaseCusPrice.Price ? (SaleCusPrice.Price / BaseCusPrice.Price*10).ToString("0.0")+"折" : ""),
                    Statistics = Statistics,
                    ProductImages = adImages,
                    ProductDetail = ProductDetail
                };
            }
            return new { };

        }
        public object GetProductHtmlInfo(Hyt.Model.PdProduct product)
        {
            return "";
        }
    }
}
 