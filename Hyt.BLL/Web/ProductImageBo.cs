using System;
using System.Web;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品图片
    /// </summary>
    /// <remarks>2013-11-17 苟治国 创建</remarks>
    public class ProductImageBo : BOBase<ProductImageBo>
    {
        private static ProductImageConfig productImageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig();
        private static AttachmentConfig attachmentConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();

        /// <summary>
        /// 根据商品编号获取图片
        /// </summary>
        /// <param name="type">缩略图类型</param>
        /// <param name="sysNo">产品编号</param>
        /// <returns>图片完整路径</returns>
        /// <remarks>2013-09-18 苟治国 创建</remarks>
        public string GetProductImagePath(ProductThumbnailType type, int sysNo)
        {
            return string.Format(productImageConfig.ProductImagePathFormat, attachmentConfig.FileServer, type.ToString(), sysNo + ".jpg");
        }

        /// <summary>
        /// 根据商品获取图片
        /// </summary>
        /// <param name="pathFormat">缩略图格式路径</param>
        /// <param name="type">缩略图类型</param>
        /// <returns>图片完整路径</returns>
        /// <remarks>2013-10-31 苟治国 创建</remarks>
        public string GetProductImagePath(string pathFormat,ProductThumbnailType type)
        {
            if (!string.IsNullOrEmpty(pathFormat))
                return string.Format(pathFormat, attachmentConfig.FileServer, type.ToString());
            else
                return "";
        }

        /// <summary>
        /// 获取购团商品图片
        /// </summary>
        /// <param name="pathFormat">路径</param>
        /// <returns>图片URL地址</returns>
        /// <remarks>2013-10-17 苟治国 创建</remarks>
        public string GetGroupShoppingImagePath(string pathFormat)
        {
            if (pathFormat != "")
                return attachmentConfig.FileServer + pathFormat;
            else
                return "";
        }

        /// <summary>
        /// 获取广告图片
        /// </summary>
        /// <param name="pathFormat">路径</param>
        /// <returns>图片URL地址</returns>
        /// <remarks>2013-11-17 苟治国 创建</remarks>
        public string GetFeAdvertImagePath(string pathFormat)
        {
            if (pathFormat != "")
                return attachmentConfig.FileServer + pathFormat;
            else
                return "";
        }

        /// <summary>
        /// 根据编号获取客户头像
        /// </summary>
        /// <param name="pathType">图片类型</param>
        /// <param name="sysNo">客户编号</param>
        /// <returns>图片URL地址</returns>
        /// <remarks>2013-08-23 苟治国 迁移</remarks>
        public string GetHeadImagePath(ProductThumbnailType pathType, int sysNo)
        {
            return attachmentConfig.FileServer + "/" + pathType.ToString() + "/" + sysNo + ".jpg";
        }

        /// <summary>
        /// 获取商品描述图片路径地址
        /// </summary>
        /// <param name="imageUrl">图片地址</param>
        /// <returns>图片URL地址</returns>
        /// <remarks>2013-08-23 苟治国 创建</remarks>
        public string GetProductDescriptionImagePath(string imageUrl)
        {
            return attachmentConfig.FileServer + imageUrl;
        }
    }

    #region 缩略图类型
    /// <summary>
    /// 产品缩略图类型
    /// </summary>
    /// <remarks>2013-08-23 苟治国 创建</remarks>
    public enum ProductThumbnailType
    {
        /// <summary>
        /// 原图,无压缩
        /// </summary>
        Base,
        /// <summary>
        /// 主图 800X800
        /// </summary>
        Prime,
        /// <summary>
        /// Big
        /// </summary>
        Big,
        /// <summary>
        /// Small
        /// </summary>
        Small,
        /// <summary>
        /// 460
        /// </summary>
        Image460,
        /// <summary>
        /// 240
        /// </summary>
        Image240,
        /// <summary>
        /// 200
        /// </summary>
        Image200,
        /// <summary>
        /// 180
        /// </summary>
        Image180,
        /// <summary>
        /// 120
        /// </summary>
        Image120,
        /// <summary>
        /// 100
        /// </summary>
        Image100,
        /// <summary>
        /// 80
        /// </summary>
        Image80,
        /// <summary>
        /// 60
        /// </summary>
        Image60,
        /// <summary>
        /// 用户头像
        /// </summary>
        CustomerFace,
        /// <summary>
        /// 晒单(做为Ftp文件夹名使用)
        /// </summary>
        ShowOrder,
    }
    #endregion
}
