using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.BLL.Cache;
using Hyt.DataAccess.Product;
using Hyt.Model.B2CApp;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Caching;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 获取指定商品的关联商品列表
    /// </summary>
    /// <returns>关联商品列表</returns>
    /// <remarks>2013-07-23 邵斌 创建</remarks>
    public class PdProductAssociationBo : BOBase<PdProductAssociationBo>
    {
        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关联商品列表</returns>
        /// <remarks>2013-07-23 邵斌 创建</remarks>
        public IList<CBProductAssociation> ProductList(int productSysNo)
        {
            if (productSysNo <= 0)
                return new List<CBProductAssociation>();

            return IPdProductAssociationDao.Instance.ProductList(productSysNo);
        }

        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回B2CAPP接口转用商品对象</returns>
        /// <remarks>2013-08-23 邵斌 创建</remarks>
        public IList<Model.B2CApp.ProductAttribute> GetProductList(int productSysNo)
        {
            IList<CBProductAssociation> list = IPdProductAssociationDao.Instance.GetProductList(productSysNo);  //读取与当前商品关联的商品列表
            IList<Model.B2CApp.ProductAttribute> result = new List<ProductAttribute>();                         //结果集

            var attributeSysNoList = list.GroupBy(p => p.AttributeSysNo).Select(p => p.Key).ToList();           //分组商品属性

            ProductAttribute tempAttribute;

            //关联遍历属性
            foreach (var item in attributeSysNoList)
            {
                tempAttribute = null;
                //分组属性值
                var attributes = from a in list
                                 where a.AttributeSysNo.Equals(item)
                                 select a;

                

                //属性值选项去重,并设置选择值和对应的商品
                foreach (var cbProductAssociation in attributes)
                {
                    if (tempAttribute == null)
                    {
                        tempAttribute = new ProductAttribute();
                        tempAttribute.AttributeName = cbProductAssociation.AttributeName;
                        tempAttribute.SysNo = cbProductAssociation.AttributeSysNo;
                        tempAttribute.AttributeOptions = new List<AttributeOption>();
                    }

                    if (tempAttribute.AttributeOptions.FirstOrDefault(p => p.AttributeText == cbProductAssociation.AttributeText) == null)
                    {
                        //读取当前商品属性，属性以当前商品为准
                        var currentProductAttribute = attributes.FirstOrDefault(p => p.ProductSysNo == productSysNo && p.AttributeText == cbProductAssociation.AttributeText && p.AttributeSysNo == cbProductAssociation.AttributeSysNo);

                        //初始化数据
                        tempAttribute.AttributeOptions.Add(new AttributeOption()
                            {
                                AttributeOptionSysNo = cbProductAssociation.AttributeOptionSysNo,
                                AttributeText = cbProductAssociation.AttributeText,
                                ProductSysNo = (currentProductAttribute == null) ? cbProductAssociation.ProductSysNo : productSysNo //如果属性值等于当前商品的，那商品的系统编号就以当前商品为准
                            });
                    }

                }
                result.Add(tempAttribute);
            }

            return result;
        }

        /// <summary>
        /// 清理指定商品的关联关系
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        /// <remarks>2013-08-07 邵斌 添加缓存操作</remarks>
        public void Clear(int productSysNo)
        {
            string relationCode = GetRelationCode(productSysNo);
            DeleteCache.ProductInfo(productSysNo);
            if (!IPdProductAssociationDao.Instance.Clear(relationCode))
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("清除商品{0}关联关系商品", productSysNo), LogStatus.系统日志目标类型.商品关联, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
        }

        /// <summary>
        /// 创建一组商品的商品关联关系
        /// </summary>
        /// <param name="mainProductSysNo">主商品系统编号</param>
        /// <param name="associationProductSysNoList">关联商品系统编号</param>
        /// <param name="relationCode">更新操作人</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public bool Create(int mainProductSysNo, int[] associationProductSysNoList, string relationCode)
        {
            /* 添加过程为先删后增的方式 
             * 添加请先清理所以主商品现有的商品管理关系，
             * 然后再添加新的关联关系到数据库 
             */

            //读取关联关系
            if (string.IsNullOrWhiteSpace(relationCode))
            {
                relationCode = IPdProductAssociationDao.Instance.GetRelactionCode(mainProductSysNo);
            }

            //判断是否有关联关系码，如果没有奖生产一个
            if (string.IsNullOrWhiteSpace(relationCode))
            {
                //如果没有关联关系码首先默认是用选择的商品中第一个有关联关系码的商品关联码，如果都没有就生产一个
                if (associationProductSysNoList.Length > 0)
                {
                    //遍历选择的商品，找到第一个有关联关系码的商品
                    foreach (var sysno in associationProductSysNoList)
                    {
                        relationCode = IPdProductAssociationDao.Instance.GetRelactionCode(mainProductSysNo);
                        //如果有关联关系吗则立即结束遍历进入下一步
                        if (!string.IsNullOrWhiteSpace(relationCode))
                        {
                            break;
                        }
                    }
                }
                relationCode = relationCode ?? Guid.NewGuid().ToString("N"); //生产GUI关联关系码
            }

            //清理关联关系
            Clear(mainProductSysNo);

            //如果被关联列表为空则直接返回成功
            if (associationProductSysNoList.Length == 0)
                return true;

            //将关联商品添加到数据库
            var result =  IPdProductAssociationDao.Instance.Create(mainProductSysNo, associationProductSysNoList,
                                                     BLL.Authentication.AdminAuthenticationBo.Instance
                                                        .GetAuthenticatedUser().SysNo, relationCode);

            if (result)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品{0}创建关联关系商品", mainProductSysNo),
                                             LogStatus.系统日志目标类型.商品关联, mainProductSysNo,
                                             AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("商品{0}创建关联关系商品失败", mainProductSysNo),
                                             LogStatus.系统日志目标类型.商品关联, mainProductSysNo,
                                             AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            if (result)
            {
                //associationProductSysNoList
                //清理缓存
                for (int i = 0; i < associationProductSysNoList.Length; i++)
                {
                    try
                    {
                        Cache.DeleteCache.ProductInfo(associationProductSysNoList[i]);
                    }
                    catch
                    {
                        ;
                    }
                    
                }
            }

            return result;

        }

        /// <summary>
        /// 获取商品关联关系码
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品关联关系码</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public string GetRelationCode(int productSysNo)
        {
            //将关联商品添加到数据库
            return IPdProductAssociationDao.Instance.GetRelactionCode(productSysNo);
        }

    }
}
