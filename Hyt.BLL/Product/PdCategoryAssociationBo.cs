using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.Model;
using Hyt.DataAccess.Product;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品与商品分类关联关系
    /// </summary>
    /// <remarks>2013-07-18 邵斌 创建</remarks>
    public class PdCategoryAssociationBo : BOBase<PdCategoryAssociationBo>
    {
        /// <summary>
        /// 添加商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public int Create(PdCategoryAssociation newPdCategoryAssociation)
        {
            //判断对应的商品与分类的对应关系是否已经存在，如果存在就不做任何操作
            if (IsExist(newPdCategoryAssociation.ProductSysNo, newPdCategoryAssociation.CategorySysNo))
            {
                return 0;
            }

            newPdCategoryAssociation.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            newPdCategoryAssociation.CreatedDate = DateTime.Now;
            newPdCategoryAssociation.LastUpdateBy = newPdCategoryAssociation.CreatedBy;
            newPdCategoryAssociation.LastUpdateDate = newPdCategoryAssociation.CreatedDate;

            return IPdCategoryAssociationDao.Instance.Create(newPdCategoryAssociation);
        }

        /// <summary>
        /// 添加B2B商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public int CreateToB2B(PdCategoryAssociation newPdCategoryAssociation)
        {
            //判断对应的商品与分类的对应关系是否已经存在，如果存在就不做任何操作
            if (IsExistInB2B(newPdCategoryAssociation.ProductSysNo, newPdCategoryAssociation.CategorySysNo))
            {
                return 0;
            }

            newPdCategoryAssociation.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            newPdCategoryAssociation.CreatedDate = DateTime.Now;
            newPdCategoryAssociation.LastUpdateBy = newPdCategoryAssociation.CreatedBy;
            newPdCategoryAssociation.LastUpdateDate = newPdCategoryAssociation.CreatedDate;

            return IPdCategoryAssociationDao.Instance.CreateB2B(newPdCategoryAssociation);
        }
        /// <summary>
        /// 删除指定的商品分类关联关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联关系对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public bool Delete(PdCategoryAssociation newPdCategoryAssociation)
        {
            return IPdCategoryAssociationDao.Instance.Delete(newPdCategoryAssociation);
        }

        /// <summary>
        /// 删除指定商品的指定关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public bool Delete(int productSysNo, int categorySysNo)
        {

            return IPdCategoryAssociationDao.Instance.Delete(productSysNo, categorySysNo);
           
        }

        /// <summary>
        /// 删除指定商品的一组关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="categorySysNoList">商品分类编号列表</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public bool Delete(int productSysNo, IList<int> categorySysNoList)
        {
            return IPdCategoryAssociationDao.Instance.Delete(productSysNo, categorySysNoList);
        }

        /// <summary>
        /// 删除指定商品的指定的所有关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2014-08-08 余勇 创建</remarks>
        public bool Delete(int productSysNo)
        {
            return IPdCategoryAssociationDao.Instance.Delete(productSysNo);
        }

        /// <summary>
        /// 检查和商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:包含 false：不包含</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public bool IsExist(int productSysNo, int categorySysNo)
        {
            return IPdCategoryAssociationDao.Instance.IsExist(productSysNo, categorySysNo);

        }

        /// <summary>
        /// 检查b2b商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:包含 false：不包含</returns>
        /// <remarks>2017-010-11 罗勤瑶 创建</remarks>
        public bool IsExistInB2B(int productSysNo, int categorySysNo)
        {
            return IPdCategoryAssociationDao.Instance.IsExistInB2B(productSysNo, categorySysNo);

        } 
    }
}
