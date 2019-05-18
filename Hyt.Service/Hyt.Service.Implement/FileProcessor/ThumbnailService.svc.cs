using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.IO;

namespace Hyt.Service.Implement.FileProcessor
{
    /// <summary>
    /// 略缩图服务
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ThumbnailService : Hyt.Service.Contract.FileProcessor.IThumbnailService
    {
        /// <summary>
        /// 产品略缩图处理
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>true:成功 false:失败</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        public bool ProductThumbnailProcessor(string fileName, int productSysNo)
        {
            var result = true;
            try
            {
                string productImageFolderPath = System.Configuration.ConfigurationManager.AppSettings["ProductImagePath"];  //图片存放目录
                var productImageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig(); //商品缩略图配置信息

                //{0}Product/{1}/{2}
                var productBaseImageFullPath = string.Format(productImageConfig.ProductImagePathFormat, productImageFolderPath, productImageConfig.BaseFolder, fileName); //产品原图完整路径

                using (var productBaseImage = System.IO.File.Open(productBaseImageFullPath, FileMode.Open, FileAccess.Read))
                {
                    string productThumbnailImagePath = string.Empty;
                    var productThumbnailName = productSysNo + ".jpg";
                    foreach (var productConfig in productImageConfig.Thumbnail)
                    {
                        productThumbnailImagePath = string.Format(productImageConfig.ProductImagePathFormat, productImageFolderPath, productConfig.Folder, "");
                        if (!Directory.Exists(productThumbnailImagePath))
                            Directory.CreateDirectory(productThumbnailImagePath);

                        using (var productFaceImage = Hyt.Util.ImageUtil.CreateThumbnail(productBaseImage,
                                                                              productConfig.Width,
                                                                              productConfig.Height,
                                                                              Hyt.Util.ImageUtil.ThumbnailMode.Cut))
                        {

                            using (Stream thumbnailFile = new FileStream(productThumbnailImagePath + "/" + productThumbnailName, FileMode.Create))
                            {
                                productFaceImage.WriteTo(thumbnailFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.LocalLogBo.Instance.Write(ex);
                result = false;
            }

            return result;
        }
    }
}
