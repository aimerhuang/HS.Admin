using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.BLL.Warehouse;
using Hyt.Model;
using Hyt.Model.MallEhking;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hyt.BLL.Extras
{
    /// <summary>
    /// 易扫购
    /// </summary>
    /// <remarks>2016-5-9 杨浩 创建</remarks>
    public class MallEhkingClientBo : BOBase<MallEhkingClientBo>
    {
        private Dictionary<string, string> goodsType = new Dictionary<string, string>()
        {
            {"服饰","01"},
            {"鞋靴","02"},
            {"母婴","03"},
            {"美妆洗护","04"},
            {"首饰","05"},
            {"箱包","06"},
            {"配件","07"},
            {"保健","08"},
            {"电器数码","09"},
            {"其他","10"},
            {"日用品","11"},
            {"食品","12"}
        };

        /// <summary>
        /// 同步商品到易扫购，以修改方式
        /// </summary>
        /// <param name="url">商品修改地址</param>
        /// <param name="merchantId">商户编号</param>
        /// <param name="customerId">商户角色编号</param>
        /// <param name="key">商户密钥</param>
        /// <param name="model">商品实体</param>
        /// <returns></returns>
        /// <remarks>2016-05-18 杨浩 创建</remarks>
        public Result AsynProductUpdate(string merchantId, string goodsId, string key, ParaProductSearchFilter model, int dealerSysNo)
        {
            var result = new Result()
            {
                Status=true
            };

            var wareHouseInfo = BLL.Distribution.DsDealerWharehouseBo.Instance.GetByDsUserSysNo(dealerSysNo);
            if (wareHouseInfo == null)
            {
                result.Status = false;
                result.Message = "没有绑定仓库";
                return result;
            }

            string url = "https://mall.ehking.com/api/goods/modify";
           //PdProductStock productStock = PdProductStockBo.Instance.GetEntityByWP(wareHouseInfo.WarehouseSysNo, 0);

            var pager=new Pager<CBPdProductDetail>();
            var condition=new ParaProductFilter();
            condition.DealerSysNo = dealerSysNo;
            pager.PageSize = int.MaxValue;
            pager.CurrentPage = 1;
            BLL.Distribution.DsSpecialPriceBo.Instance.GetSpecialPriceProductList(ref pager, condition);
            foreach (var row in pager.Rows)
            {

            }
            var product = new MEProductUpdate();
            product.merchantId = merchantId;
            product.goodsId = goodsId;
            product.goodsName = model.ProductName;
            product.goodsCode = model.ErpCode;
            product.goodsAmount = (int)(model.Price * 100);
            product.goodsType = goodsType["其他"];



            decimal inventoryCount = 0;
            //if (productStock != null)
            //{
            //    inventoryCount = productStock.StockQuantity;
            //}

            product.inventoryCount = (int)inventoryCount;
            product.status = model.Status;
            product.goodsDescribe = model.ProductSummary;

            IList<PdProductImage> imageList = new List<PdProductImage>();
            imageList = PdProductImageBo.Instance.GetProductImg(model.SysNo).Take(12).ToList();
            StringBuilder imageJsonArray = new StringBuilder();

            product.pictureList = new List<MEProductUpdatePicture>();
            int orderNum = 1;

            foreach (PdProductImage imageItem in imageList)
            {
                string filePath = imageItem.ImageUrl.Replace('/', '\\');

                filePath = string.Format(filePath, @"E:\StaticFiles\SG\Images\", "Base");

                MEProductUpdatePicture picture = new MEProductUpdatePicture();
                picture.baseCode = imageToBase64(filePath);
                picture.orderNum = orderNum++;
                picture.fileType = getExtensionName(filePath);
                product.pictureList.Add(picture);
            }

            string hmac = product.merchantId + product.goodsId + product.goodsName + product.goodsCode + product.goodsAmount + product.goodsType + product.inventoryCount + product.status;

            product.hmac = Security.HmacSign(hmac, key);

            string param = LitJson.JsonMapper.ToJson(product);
            string jsonString = Send(url, param);

            if (jsonString.Contains(@"""status"":""ERROR"""))
            {
                result.Status = false;
                result.Message = jsonString;
            }
            else
            {
                result.Status = true;
            }

            return result;
        }
        /// <summary>
        /// 同步商品到易扫购
        /// </summary>
        /// <param name="url">商品提交地址</param>
        /// <param name="merchantId">商户编号</param>
        /// <param name="customerId">商户角色编号</param>
        /// <param name="list">商品列表</param>
        /// <param name="key">商户密钥</param>
        /// <returns></returns>
        public Result AsynProduct(string merchantId, string customerId, string key, SyUser operatorUser, IList<ParaProductSearchFilter> list)
        {
            string apiUrl = "https://mall.ehking.com/api/publishGoods";
            Result result = new Result();
            result.Status = true;
            int success = 0;
            int fail = 0;

            foreach (ParaProductSearchFilter item in list)
            {
                StringBuilder param = new StringBuilder();

                param.Append("{");
                param.AppendFormat(@"merchantId:""{0}"",", merchantId); //商户编号   非空     varchar(9)
                param.AppendFormat(@"customerId:""{0}"",", customerId); //客户编号   可为空   varchar(9) 该商品所对应的商户号，如为空则使用merchantId
                param.AppendFormat(@"goodsName:""{0}"",", item.ProductName);  //商品名     非空	 varchar(50)
                param.AppendFormat(@"goodsCode:""{0}"",", item.ErpCode);  //商品编码	非空	 varchar(20)
                param.AppendFormat(@"goodsAmount:{0},", (item.Price * 100).ToString("F0"));//商品价格	非空	 Integer(20) 单位：分
                param.AppendFormat(@"goodsType:""{0}"",", goodsType["其他"]);  //商品类别	非空	 varchar(2) 参考下面的商品类别说明

                PdProductStock productStock = PdProductStockBo.Instance.GetProductStockListByProductSysNo(item.SysNo).FirstOrDefault();
                decimal inventoryCount = 0;
                if (productStock != null)
                {
                    inventoryCount = productStock.StockQuantity;
                }

                param.AppendFormat(@"inventoryCount:{0},", inventoryCount.ToString("F0"));//库存数量	非空	Integer(10)
                param.AppendFormat(@"status:{0},", item.Status.ToString());        //商品状态	可为空	Integer（1） 1：上架0：下架，默认为1
                param.AppendFormat(@"goodsDescribe:""{0}"",", item.ProductSummary); //商品描述	可为空	varchar(200)

                var imageList = PdProductImageBo.Instance.GetProductImg(item.SysNo).Take(12).ToList();

                StringBuilder imageJsonArray = new StringBuilder();

                imageJsonArray.Append("[");

                foreach (PdProductImage imageItem in imageList)
                {
                    string filePath = imageItem.ImageUrl.Replace('/', '\\');

                    filePath = string.Format(filePath, @"E:\StaticFiles\SG\Images\", "Base");

                    if (imageJsonArray.ToString() != "[")
                    {
                        imageJsonArray.Append(",");
                    }

                    imageJsonArray.Append(@"{""baseCode"":");
                    imageJsonArray.AppendFormat(@"""{0}"",", imageToBase64(filePath));
                    imageJsonArray.Append(@"""fileType"":");
                    imageJsonArray.AppendFormat(@"""{0}""", getExtensionName(filePath));
                    imageJsonArray.Append("}");
                }

                imageJsonArray.Append("]");

                param.AppendFormat(@"pictureList:{0},", imageJsonArray.ToString());  //图片列表	    可为空	数组 最多12张，显示顺序与上传顺序一致，格式为JSONArray

                string hmac = merchantId + customerId + item.ProductName + item.ErpCode + (item.Price * 100).ToString("F0") + goodsType["其他"] + inventoryCount.ToString("F0") + item.Status.ToString();
                param.AppendFormat(@"hmac:""{0}""", Security.HmacSign(hmac, key));   //参数签名	    非空
                param.Append("}");

                string jsonString = Send(apiUrl,param.ToString());

                if (jsonString.Contains(@"""status"":""ERROR"""))
                {
                    result.Status = false;
                    fail += 1;

                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品编号：" + item.ErpCode + "     同步失败\n" + jsonString,
                    LogStatus.系统日志目标类型.商品同步失败, operatorUser.SysNo, null, null, operatorUser.SysNo);
                }
                else
                {
                    success += 1;
                }
            }

            if (success != list.Count)
            {
                result.Message = "同步成功 （" + success.ToString() + "） 个,失败 （" + fail + "）个,具体信息请查看系统日志！";
            }

            return result;
        }

        private string Send(string url, string param)
        {
            return WebUtil.PostString(url, param, "application/vnd.ehking.mall-v1.0+json", "application/vnd.ehking.mall-v1.0+json");
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public string getExtensionName(string filename)
        {
            string fileType = "";

            if ((filename != null) && (filename.Length > 0))
            {
                fileType = Path.GetExtension(filename);

                if (0 < fileType.Length)
                {
                    fileType = fileType.Substring(1, fileType.Length - 1);
                }
                else
                {
                    fileType = "";
                }
            }

            return fileType;
        }

        /// <summary>
        /// 将图片文件转化为字节数组，并对其进行Base64编码处理
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String imageToBase64(string filePath)
        {
            FileStream inputFile = null;
            try
            {
                inputFile = File.OpenRead(filePath);

                long len = inputFile.Length;
                byte[] buffer = new byte[len];
                inputFile.Read(buffer, 0, buffer.Length);
                inputFile.Close();

                return Security.EncodeBase64(buffer);
            }
            catch (FileNotFoundException e)
            {
                return "";
            }
            catch (IOException e)
            {
                return "";
            }
        }
    }
}