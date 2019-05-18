using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品关注
    /// </summary>
    /// <remarks>2013-09-12 邵斌 创建</remarks>
    public class CrFavoritesBo : BOBase<CrFavoritesBo>
    {
        /// <summary>
        /// 添加关注商品
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关注实体</returns>
        /// <remarks>2013-09-12 邵斌 创建</remarks>
        public Result AddToFavorite(int customerSysNo, int productSysNo)
        {
            Result result = new Result();

            //检查商品是否已经加关注
            var oldFavorite = DataAccess.CRM.ICrFavoritesDao.Instance.GetFavorites(customerSysNo, productSysNo);

            //判读是否已经关注过，如果有对象则表示关注过了
            if (oldFavorite != null)
            {
                //已经关注过
                result.Status = false;
                result.StatusCode = 1;
                result.Message = "已经关注该商品";
            }
            else
            {
                //没有关注,建立关注
                result.Status = DataAccess.CRM.ICrFavoritesDao.Instance.Create(new CrFavorites()
                    {
                        CreateDate =  DateTime.Now,
                        CustomerSysNo = customerSysNo,
                        ProductSysNo =  productSysNo
                    }) > 0;
            }

            return result;
        }

        /// <summary>
        ///  添加喜欢商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>添加喜欢是否成功</returns>
        /// <remarks>2013-09-12 邵斌 创建</remarks>
        public Result AddToLiking(int productSysNo)
        {
            Result result = new Result();

            var model = Product.PdProductStatisticsBo.Instance.Get(productSysNo);
            model.Liking++;
            
            try
            {
                result.StatusCode = Product.PdProductStatisticsBo.Instance.Update(model);
                result.Status = result.StatusCode > 0;
            }
            catch
            {
                result.StatusCode = -1;
                result.Status = false;
            }

            return result;
        }
    }
}
