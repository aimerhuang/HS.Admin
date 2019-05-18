using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品图片维护
    /// </summary>
    /// <remarks>2013-07-22 苟治国 创建</remarks>  
    public class PdProductImageBo : BOBase<PdProductImageBo>
    {
        /// <summary>
        /// 查看商品图片
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>商品图片</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public Model.PdProductImage GetModel(int sysNo)
        {
            return IPdProductImageDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 获取指定商品的图片列表
        /// </summary>
        /// <param name="productsysNo">商品系统编号</param>
        /// <returns>获取指定商品的图片列表</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public IList<PdProductImage> GetProductImg(int productsysNo)
        {
            return IPdProductImageDao.Instance.GetProductImg(productsysNo);
        }

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public int Insert(Model.PdProductImage model)
        {
            return IPdProductImageDao.Instance.Insert(model);
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="images">商品图片实体</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        /// <remarks>2013-11-7 邵斌 添加日志记录</remarks>
        public bool DisplayOrder(IList<PdProductImage> images)
        {
            int result = 0;
            int productSysNo = 0;
            for (int i = 0; i < images.Count; i++)
            {
                PdProductImage productImage = GetModel(images[i].SysNo);
                productImage.DisplayOrder = images[i].DisplayOrder;
                productSysNo = productImage.ProductSysNo;
                result = Update(productImage);
            }

            if (result > 0)
            {
                //用户操作日志
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品{0}图片调整顺序", productSysNo), LogStatus.系统日志目标类型.商品图片排序, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                //用户操作日志
                Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("商品{0}图片调整顺序失败", productSysNo), LogStatus.系统日志目标类型.商品图片排序, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="images">商品分类实体</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-07-23 苟治国 创建</remarks>
        public bool AddProductImage(IList<PdProductImage> images)
        {
            int result = 0;
            int productSysNo = 0;
            foreach (var imageItem in images)
            {
                if (!string.IsNullOrEmpty(imageItem.ImageUrl))
                {
                    var list = IPdProductImageDao.Instance.GetProductImg(imageItem.ProductSysNo);
                    if (list.Count>=5)
                    {
                        continue;
                    }
                    var productImage = new PdProductImage();
                    productSysNo = imageItem.ProductSysNo;
                    productImage.ProductSysNo = imageItem.ProductSysNo;
                    productImage.ImageUrl = imageItem.ImageUrl;
                    productImage.Status = (int) Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.隐藏;
                    result = IPdProductImageDao.Instance.Insert(productImage);
                }

            }

            if (result > 0)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品{0}添加图片", productSysNo), LogStatus.系统日志目标类型.商品图片, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("商品{0}添加图片失败", productSysNo), LogStatus.系统日志目标类型.商品图片, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }


        /// <summary>
        /// 图片添加到B2B对应的商品
        /// </summary>
        /// <param name="images">商品分类实体</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        public bool AddProductImageToB2B(IList<PdProductImage> images)
        {
            int result = 0;
            int productSysNo = 0;
            foreach (var imageItem in images)
            {
                if (!string.IsNullOrEmpty(imageItem.ImageUrl))
                {
                    var product = IPdProductDao.Instance.GetProduct(imageItem.ProductSysNo);
                    var productB2B = IPdProductDao.Instance.GetB2BProductByErpCode(product.ErpCode);
                    if (productB2B==null)
                    {
                        continue;
                    }
                    var list = IPdProductImageDao.Instance.GetB2BProductImg(productB2B.SysNo);
                    if (list.Count >= 5)
                    {
                        continue;
                    }
                    var productImage = new PdProductImage();
                    productSysNo = productB2B.SysNo;
                    productImage.ProductSysNo = productB2B.SysNo;
                    productImage.ImageUrl = imageItem.ImageUrl;
                    productImage.Status = (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.隐藏;
                    result = IPdProductImageDao.Instance.InsertB2B(productImage);
                }

            }

            if (result > 0)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品{0}添加图片到B2B", productSysNo), LogStatus.系统日志目标类型.商品图片, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("商品{0}添加图片到B2B失败", productSysNo), LogStatus.系统日志目标类型.商品图片, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public int Update(Model.PdProductImage model)
        {
            return IPdProductImageDao.Instance.Update(model);

        }

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="productsysno">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public int UpdateStatus(int productsysno, int status)
        {
            return IPdProductImageDao.Instance.UpdateStatus(productsysno, status);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="images">商品图片</param>
        /// <param name="productImageSysNo">商品图片编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-23 苟治国 创建</remarks>
        public bool Delete(IList<PdProductImage> images, int productImageSysNo)
        {
            int result = 0;
            if (IPdProductImageDao.Instance.Delete(productImageSysNo))
            {
                result = 1;
                //删除图片操作
            }
            
            
            if (result > 0)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("删除商品图片{0}", productImageSysNo), LogStatus.系统日志目标类型.商品图片, productImageSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("删除商品图片{0}失败", productImageSysNo), LogStatus.系统日志目标类型.商品图片, productImageSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 获取封面图
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Model.PdProductImage GetModelByPdSysNo(int sysNo)
        {
            return IPdProductImageDao.Instance.GetModelByPdSysNo(sysNo);
        }
    }
}
