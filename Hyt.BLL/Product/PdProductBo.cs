using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Product;
using System.Data;
using Hyt.Model.LogisApp;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;
using System.Transactions;
using System.IO;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Promotion;
using Hyt.Infrastructure.Memory;
using Hyt.Util;
using Hyt.BLL.Log;
using System.Web.Mvc;
using Hyt.BLL.Basic;
using Hyt.DataAccess.Procurement;
using Hyt.Model.ExcelTemplate;

namespace Hyt.BLL.Product
{
    public class PdProductBo : BOBase<PdProductBo>
    {
        /// <summary>
        /// 通过商品条形码获取商品系统编号
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns>商品系统编号</returns>
        /// <remarks>2016-05-29 杨浩 创建</remarks>
        public int GetProductSysNoByBarCode(string barCode)
        {
            return IPdProductDao.Instance.GetProductSysNoByBarCode(barCode);
        }

        /// <summary>
        /// 获取指定商品编码的商品信息利嘉版
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2017-05-28 罗勤尧 创建</remarks>
        public CBOutputPdProductsLijia GetEntityLiJiaByErpCode(string barCode)
        {
            return IPdProductDao.Instance.GetEntityLiJiaByErpCode(barCode);
        }
        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <param name="categoryAssociationList">待添加的商品分类关联关系</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        /// <remarks>2013-07-26 邵斌 重构</remarks>
        public int Create(PdProduct model, IList<PdCategoryAssociation> categoryAssociationList = null)
        {
            //结果对象
            Result result = new Result()
                {
                    Status = false,
                    StatusCode = 0
                };

            //初始化创建者和更新者
            model.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = model.CreatedBy;
            model.LastUpdateDate = model.CreatedDate;

            //添加到商品主表
            var sysNo = IPdProductDao.Instance.Create(model);

            //如果创建成功将添加商品的其他信息
            if (sysNo > 0)
            {
                result.Status = true;

                if (categoryAssociationList != null)
                {
                    //分类关系
                    foreach (PdCategoryAssociation pdCategoryAssociation in categoryAssociationList)
                    {
                        pdCategoryAssociation.ProductSysNo = sysNo;
                        pdCategoryAssociation.SysNo = PdCategoryAssociationBo.Instance.Create(pdCategoryAssociation);
                        result.Status = result.Status && (pdCategoryAssociation.SysNo > 0);
                        //添加商品分类对应信息

                        //如果添加失败将推出添加
                        if (!result.Status)
                        {
                            result.Status = false;
                            result.Message = "未能正确添加商品分类信息！！！";
                            break;
                        }
                    }
                }

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("新增商品{0}基本信息", sysNo), LogStatus.系统日志目标类型.商品基本信息, sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("新增商品{0}基本信息", sysNo), LogStatus.系统日志目标类型.商品基本信息, sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return sysNo;
        }

        /// <summary>
        /// 同步创建商品信息到B2B平台
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <param name="categoryAssociationList">待添加的商品分类关联关系</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>
        /// 2017-10-11 罗勤瑶 创建
        /// </remarks>
        public int CreateToB2B(PdProduct model, IList<PdCategoryAssociation> categoryAssociationList = null)
        {
            //结果对象
            Result result = new Result()
            {
                Status = false,
                StatusCode = 0
            };

            //初始化创建者和更新者
            model.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = model.CreatedBy;
            model.LastUpdateDate = model.CreatedDate;

            //添加到商品主表
            var sysNo = IPdProductDao.Instance.CreateToB2B(model);

            //如果创建成功将添加商品的其他信息
            if (sysNo > 0)
            {
                result.Status = true;

                if (categoryAssociationList != null)
                {
                    //分类关系
                    foreach (PdCategoryAssociation pdCategoryAssociation in categoryAssociationList)
                    {
                        pdCategoryAssociation.ProductSysNo = sysNo;
                        //var  category= IPdCategoryDao.Instance.GetCategory(pdCategoryAssociation.CategorySysNo);
                        //分类关系暂时不同步
                        //pdCategoryAssociation.SysNo = PdCategoryAssociationBo.Instance.CreateToB2B(pdCategoryAssociation);
                        result.Status = result.Status && (pdCategoryAssociation.SysNo > 0);
                        //添加商品分类对应信息

                        //如果添加失败将推出添加
                        if (!result.Status)
                        {
                            result.Status = false;
                            result.Message = "未能正确添加商品分类信息！！！";
                            break;
                        }
                    }
                }

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("新增商品{0}基本信息", sysNo), LogStatus.系统日志目标类型.商品基本信息, sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("新增商品{0}基本信息", sysNo), LogStatus.系统日志目标类型.商品基本信息, sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return sysNo;
        }

        /// <summary>
        /// 通过商品编号获取B2B商品信息
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>
        /// 2017-10-11 罗勤瑶 创建
        /// </remarks>
        public PdProduct GetB2BProductByErpCode(string erpCode)
        {
            return IPdProductDao.Instance.GetB2BProductByErpCode(erpCode);
        }
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <param name="categoryAssociationList">待添加的商品分类关联关系</param>
        /// <param name="deleteCategoryAssociationList">待删除商品分类关联关系</param>m>
        /// <param name="masterCategorySysNo">新主商品分类系统编号</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        /// <remarks>2013-07-26 邵斌 重构</remarks>
        /// <remarks>2014-08-11 余勇 重构 商品与分类的关联进行先删后增操作</remarks>
        public Result Update(PdProduct model, IList<PdCategoryAssociation> categoryAssociationList = null,
                             IList<int> deleteCategoryAssociationList = null, int masterCategorySysNo = 0)
        {
            Result result = new Result()
                {
                    Status = false,
                    StatusCode = 0
                };

            //判断时间戳
            if (!CheckProductStamp(model.SysNo, model.Stamp.Ticks))
            {
                //时间戳与数据库不对应返回错误结果
                result.Status = false;
                result.Message = "在您商品编辑的同时商品信息已经发生了改变，请刷新页面并从新编辑商品信息！！！";
            }
            else
            {
                model.LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Stamp = DateTime.Now;
                //更新商品信息
                result.Status = IPdProductDao.Instance.Update(model);

                //更新商品分类信息
                if (result.Status)
                {
                    //if (deleteCategoryAssociationList != null && deleteCategoryAssociationList.Count > 0)
                    //{
                    //删除移除的商品分类所有关联关系
                    if (
                        !PdCategoryAssociationBo.Instance.Delete(model.SysNo))
                    {

                        //用户操作日志
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新商品{0}分类失败", model.SysNo), LogStatus.系统日志目标类型.商品分类, model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

                        result.Status = false;
                        result.Message = "移除商品分类时出错！！！";
                    }
                    //}

                }
                else
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新商品{0}基本信息失败", model.SysNo), LogStatus.系统日志目标类型.商品基本信息, model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

                    result.Status = false;
                    result.Message = "修改商品基本信息是出错！！！";
                }
            }

            //将新增或修改有变动的商品分类进行添加操作
            //如果创建成功将添加商品的其他信息
            if (result.Status)
            {
                if (categoryAssociationList != null)
                {
                    //分类关系 保存新的商品关联
                    foreach (PdCategoryAssociation pdCategoryAssociation in categoryAssociationList)
                    {
                        pdCategoryAssociation.ProductSysNo = model.SysNo;
                        pdCategoryAssociation.SysNo = PdCategoryAssociationBo.Instance.Create(pdCategoryAssociation);
                        result.Status = result.Status && (pdCategoryAssociation.SysNo > 0);
                        //添加商品分类对应信息

                        //如果添加失败将推出添加
                        if (!result.Status)
                        {
                            //用户操作日志
                            Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新商品分类{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品分类, model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

                            result.Status = false;
                            result.Message = "未能正确添加商品分类信息！！！";
                            break;
                        }
                    }
                }
            }

            //对商品主分类进行单独操作，如果新商品主分类>0，将更新商品主分类信息    
            if (result.Status && masterCategorySysNo > 0)
            {
                result.Status = IPdCategoryAssociationDao.Instance.ChangeProductMasterCategory(model.SysNo, masterCategorySysNo);
                if (result.Status == false)
                {
                    //用户操作日志
                    Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新商品{0}主分类为{1}失败", model.SysNo, masterCategorySysNo),
                                              LogStatus.系统日志目标类型.商品主分类, model.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    result.Message = "更新商品主分类失败";
                }
                else
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新商品{0}主分类为{1}", model.SysNo, masterCategorySysNo), LogStatus.系统日志目标类型.商品主分类, model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }
            }

            //缓存清理
            if (result.Status)
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新商品{0}基本信息", model.SysNo), LogStatus.系统日志目标类型.商品基本信息, model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

                Cache.DeleteCache.ProductInfo(model.SysNo);
            }

            return result;
        }

        /// <summary>
        /// 检查商品时间戳
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="stampTicks">缓存时间戳</param>
        /// <returns>true：时间戳正确 false：时间戳不正确</returns>
        /// <remarks>2013-10-21 邵斌 创建</remarks>
        public bool CheckProductStamp(int productSysNo, long stampTicks)
        {
            return IPdProductDao.Instance.GetPdProductStamp(productSysNo).Ticks.Equals(stampTicks);
        }

        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public CBPdProduct GetProduct(int productSysNo)
        {

            //增加缓存 2014-09-10 何方
            return CacheManager.Get<CBPdProduct>(CacheKeys.Items.CBPdProduct_, productSysNo.ToString(), DateTime.Now.AddDays(1), () =>
            {
                //var product = IPdProductDao.Instance.GetProduct(productSysNo);
                //if (product != null)
                //{
                //    product.SeoTitle = product.SeoTitle ?? product.ProductName;
                //    product.SeoKeyword = product.SeoKeyword ?? product.ProductName;
                //    product.SeoDescription = product.SeoDescription ?? product.ProductName;
                //}

                //return product;
                return GetProductNoCache(productSysNo);
            });
        }

        /// <summary>
        /// 根据商品系统编号获取商品信息(修改产品时获取产品信息)
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-18 余勇 创建</remarks>
        public CBPdProduct GetProductNoCache(int productSysNo)
        {
            var product = IPdProductDao.Instance.GetProduct(productSysNo);
            if (product != null)
            {
                product.SeoTitle = product.SeoTitle ?? product.ProductName;
                product.SeoKeyword = product.SeoKeyword ?? product.ProductName;
                product.SeoDescription = product.SeoDescription ?? product.ProductName;
            }
            return product;
        }


        ///// <summary>
        ///// 根据条件获取商品列表
        ///// </summary>
        ///// <param name="filter">搜索条件</param>
        ///// <returns>商品列表</returns>
        ///// <remarks>2013-06-25 黄波 创建</remarks>
        //public IList<CBPdProduct> GetProducts(Model.Parameter.ParaProductFilter filter)
        //{
        //    return IPdProductDao.Instance.GetProducts(filter);
        //}

        /// <summary>
        /// 根据商品父分类ID获取子分类列表
        /// </summary>
        /// <param name="parentSysNo">父级分类编号</param>
        /// <returns>商品类别列表</returns>
        /// <remarks>2013-06-18 黄志勇 创建</remarks>
        public DataTable SearchProductCategory(int parentSysNo)
        {
            return IProduct.Instance.SearchProductCategory(parentSysNo);
        }

        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="pager">分页查询参数对象</param>
        /// <param name="pageList">查询结果集（输出参数）</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 邵斌 创建</remarks>
        /// <remarks>2013-12-03 邵斌 扩展：加入条件SyncWebFront 只显示前台同步能看到的商品</remarks>
        public void ProductSelectorProductSearch(ref Pager<ParaProductSearchFilter> pager,
                                                 out PagedList<ParaProductSearchFilter> pageList)
        {
            //初始化PageList
            pageList = new PagedList<ParaProductSearchFilter>();
            if (pager.PageSize < 1)
            {
                pager.PageSize = pageList.PageSize; //设置页记录数
            }

            //ToDO 针对不同业务功能可以在以下扩展针对性的过滤查询，但必须保证结果集是相同
            //判断是否是查询商品属性
            if (pager.PageFilter.RequiredFilterAttribute)
                IPdProductDao.Instance.SearchAttributeAssociationProduct(ref pager); //属性过滤
            else
                IPdProductDao.Instance.ProductSelectorProductSearch(ref pager); //普通查询

            //构造结果对象
            pageList.CurrentPageIndex = pager.CurrentPage;
            pageList.TData = pager.Rows;
            pageList.TotalItemCount = pager.TotalRows;
        }
        /// <summary>
        /// 选择经销商对应的商品
        /// 2015-12-25 
        /// 王耀发 创建
        /// </summary>
        /// <param name="pager"></param>
        public void DealerProductSearch(ref Pager<ParaProductSearchFilter> pager,
                                         out PagedList<ParaProductSearchFilter> pageList)
        {
            //初始化PageList
            pageList = new PagedList<ParaProductSearchFilter>();
            if (pager.PageSize < 1)
            {
                pager.PageSize = pageList.PageSize; //设置页记录数
            }

            IPdProductDao.Instance.DealerProductSearch(ref pager); //普通查询

            //构造结果对象
            pageList.CurrentPageIndex = pager.CurrentPage;
            pageList.TData = pager.Rows;
            pageList.TotalItemCount = pager.TotalRows;
        }

        /// <summary>
        /// 获取已经选择的商品列表
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        public IList<ParaProductSearchFilter> GetSelectedProductList(IList<int> productList)
        {

            return IPdProductDao.Instance.GetSelectedProductList(productList);
        }

        /// <summary>
        /// 获取已经选择商品的详细信息
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        public IList<CBPdProduct> GetSelectedProductInfo(IList<int> productList)
        {
            //return IPdProductDao.Instance.GetSelectedProductInfo(productList);

            IList<CBPdProduct> list = new List<CBPdProduct>();

            for (int i = 0; i < productList.Count; i++)
                list.Add(GetProduct(productList[i]));
            return list;
        }

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public void GetPdProductDetailList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition)
        {
            IPdProductDao.Instance.GetPdProductDetailList(ref pager, condition);
        }
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public Pager<PdProduct> GetPdProductList(Pager<PdProduct> pager)
        {
            return IPdProductDao.Instance.GetPdProductList(pager);
        }

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public Pager<CBPdProduct> GetCBPdProductList(Pager<CBPdProduct> pager)
        {
            return IPdProductDao.Instance.GetCBPdProductList(pager);
        }

        /// <summary>
        /// 获取分销商可添加的商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public Pager<CBPdProduct> GetDealerMallProductList(Pager<CBPdProduct> pager, int dealerMallSysNo)
        {
            return IPdProductDao.Instance.GetDealerMallProductList(pager, dealerMallSysNo);
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>
        /// 2013-07-16 唐永勤 创建
        /// 2013-08-07 邵  斌 清理缓存
        /// </remarks>
        public Result UpdateStatus(int status, int sysNo)
        {
            Result result = new Result();

            //如果商品编号小于1，表示没有正确选择商品，将直接返回
            if (sysNo < 1)
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "未设置要更新的记录";
                return result;
            }

            //更新状态
            result = IPdProductDao.Instance.UpdateStatus(status, sysNo);

            //清理缓存
            if (result.Status)
            {
                Cache.DeleteCache.ProductInfo(sysNo);
            }

            return result;
        }

        /// <summary>
        /// 检查商品编号是否重复
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public bool CheckERPCode(string erpCode, int sourceProductSysNo = 0)
        {
            return IPdProductDao.Instance.CheckERPCode(erpCode, sourceProductSysNo);
        }

        /// <summary>
        /// 检查商品编号是否重复
        /// </summary>
        /// <param name="qrCode">二维码编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public bool CheckQRCode(string qrCode, int sourceProductSysNo = 0)
        {
            return IPdProductDao.Instance.CheckQRCode(qrCode, sourceProductSysNo);
        }

        /// <summary>
        /// 检查条行码是否重复
        /// </summary>
        /// <param name="barcode">条行码</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public bool CheckBarCode(string barcode, int sourceProductSysNo = 0)
        {
            return IPdProductDao.Instance.CheckBarCode(barcode, sourceProductSysNo);
        }

        /// <summary>
        /// 根据属性组编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public IList<CBPdProductAtttributeReadRelation> GetProductAttributeByGroupSysno(IList<int> listSysNo)
        {
            IList<CBPdProductAtttributeRead> list =
                IPdProductAttributeDao.Instance.GetProductAttributeByGroupSysNo(listSysNo);
            return GenerateRelation(list);
        }

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public IList<CBPdProductAtttributeReadRelation> GetProductAttributeByProductSysNo(int productSysNo)
        {
            IList<CBPdProductAtttributeRead> list =
                IPdProductAttributeDao.Instance.GetProductAttributeByProductSysNo(productSysNo);
            return GenerateRelation(list);
        }

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="onlyAssociationAttribute">只读取关联属性</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>    
        public IList<PdProductAttribute> GetProductAttributeByProductSysno(int productSysNo,
                                                                           bool onlyAssociationAttribute)
        {
            IList<PdProductAttribute> list =
                IPdProductAttributeDao.Instance.GetProductAttributeByProductSysNo(productSysNo, onlyAssociationAttribute);
            return list;
        }

        /// <summary>
        /// 根据商品分类获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>    
        public IList<CBPdProductAtttributeReadRelation> GetCategoryProductAttributeByProductSysNo(int productSysNo)
        {
            IList<CBPdProductAtttributeRead> list =
                IPdProductAttributeDao.Instance.GetCategoryProductAttributeByProductSysNo(productSysNo);
            return GenerateRelation(list);
        }

        /// <summary>
        /// 生成属性关系列表，映射数据库关系及前台业务逻辑
        /// </summary>
        /// <param name="list">属性传输类列表</param>
        /// <returns>属性关系列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>    
        protected IList<CBPdProductAtttributeReadRelation> GenerateRelation(IList<CBPdProductAtttributeRead> list)
        {
            list.Reverse();
            IList<CBPdProductAtttributeReadRelation> listRelation = new List<CBPdProductAtttributeReadRelation>();
            foreach (var option in list)
            {
                if (option.AttributeType == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品属性类型.选项类型)
                {
                    option.AttributeOptionList = IPdAttributeDao.Instance.GetAttributeOptions(option.SysNo);
                }
                bool mark = false;
                foreach (var itemRelation in listRelation)
                {
                    if (itemRelation.AttributeGroupSysNo == option.AttributeGroupSysNo)
                    {
                        mark = true;
                        itemRelation.ProductAtttributeList.Add(option);
                        break;
                    }
                }
                if (!mark)
                {

                    var itemRelation = new CBPdProductAtttributeReadRelation
                        {
                            AttributeGroupName = option.AttributeGroupName,
                            AttributeGroupSysNo = option.AttributeGroupSysNo,
                            ProductAtttributeList = new List<CBPdProductAtttributeRead>()
                        };
                    if (!string.IsNullOrEmpty(option.AttributeName))
                    {
                        itemRelation.ProductAtttributeList.Add(option);
                    }
                    listRelation.Add(itemRelation);
                }
            }
            return listRelation;
        }

        /// <summary>
        /// 根据编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性编号集</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>    
        public IList<CBPdProductAtttributeRead> GetProductAttributeByAttributeSysNo(IList<int> listSysNo)
        {
            IList<CBPdProductAtttributeRead> list =
                IPdProductAttributeDao.Instance.GetProductAttributeByAttributeSysNo(listSysNo);
            foreach (var option in list)
            {
                if (option.AttributeType == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品属性类型.选项类型)
                {
                    option.AttributeOptionList = IPdAttributeDao.Instance.GetAttributeOptions(option.SysNo);
                }
            }
            return list;
        }

        /// <summary>
        /// 保存商品属性
        /// </summary>
        /// <param name="productSysno">商品编号</param>
        /// <param name="list">商品属性列表</param>
        /// <returns>保存是否成功</returns>
        /// <remarks>
        /// 2013-07-19 唐永勤 创建
        /// 2013-08-07 邵  斌 加入清理缓存
        /// </remarks> 
        public Result SaveProductAttribute(int productSysno, IList<PdProductAttribute> list)
        {
            Result result = new Result();
            if (productSysno < 1)
            {
                result.StatusCode = -1;
            }
            else
            {
                foreach (PdProductAttribute item in list)
                {
                    item.DisplayOrder = 0;
                    item.Status = 1;
                    item.ProductSysNo = productSysno;
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    item.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    item.LastUpdateDate = DateTime.Now;
                }
                bool isSuccess = IPdProductAttributeDao.Instance.SaveProductAttribute(productSysno, list);
                if (isSuccess)
                {
                    result.Status = true;
                    result.StatusCode = 1;

                    Cache.DeleteCache.ProductInfo(productSysno);
                }
                else
                {
                    result.StatusCode = -3;
                    result.Message = "保存产品属性失败";
                }
            }

            return result;

        }

        /// <summary>
        /// 克隆商品
        /// </summary>
        /// <param name="product">商品实体</param>
        /// <returns>克隆结果</returns>
        /// <remarks>2013-07-25 唐永勤 创建</remarks> 
        public Result CloneProduct(PdProduct product)
        {
            //初始化
            Result result = new Result();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "克隆商品失败";

            if (product.SysNo > 0)
            {


                //克隆基础信息
                CBPdProduct cbProduct = IPdProductDao.Instance.GetProduct(product.SysNo);
                PdProduct p = (PdProduct)cbProduct;
                p.ProductName = product.ProductName;
                p.EasName = product.EasName;
                p.ErpCode = product.ErpCode;
                p.QRCode = product.QRCode;
                p.Barcode = "";
                p.NameAcronymy = product.NameAcronymy;
                p.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                p.CreatedDate = DateTime.Now;
                p.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                p.LastUpdateDate = DateTime.Now;
                p.Status = (int)Model.WorkflowStatus.ProductStatus.商品状态.下架;

                var sysNo = IPdProductDao.Instance.Create(p);
                if (sysNo > 0)
                {

                    //克隆价格和调价历史
                    //IList<CBPdPriceHistory> pdPriceHistoryList = new List<CBPdPriceHistory>();
                    //IList<PdPrice> pdPriceHistoryList = new List<PdPrice>();
                    string groupCode = Guid.NewGuid().ToString("N"); //生成关联关系码

                    IList<PdPriceHistory> pdPriceHistoryList = new List<PdPriceHistory>();

                    foreach (PdPrice price in cbProduct.PdPrice.Value)
                    {
                        price.ProductSysNo = sysNo;
                        price.Status = (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格状态.无效;
                        price.SysNo = IPdPriceDao.Instance.Create(price);

                        pdPriceHistoryList.Add(new PdPriceHistory()
                            {
                                ApplyDate = DateTime.Now,
                                ApplyPrice = price.Price,
                                ApplySysNo = p.CreatedBy,
                                OriginalPrice = price.Price,
                                RelationCode = groupCode,
                                PriceSysNo = price.SysNo,
                                Status = (int)Model.WorkflowStatus.ProductStatus.产品价格变更状态.待审
                            });
                    }

                    //申请新的调价
                    if (pdPriceHistoryList.Count > 0)
                    {

                        result.Status = IPdPriceHistoryDao.Instance.SavePdPriceHistory(pdPriceHistoryList.ToArray());
                        //result = PdPriceBo.Instance.UpdateProductPrice(sysNo, pdPriceHistoryList);

                        //result = PdPriceHistoryBo.Instance.SavePdPriceHistories(
                        //    pdPriceHistoryList.ToArray(), new int[] { sysNo });
                    }

                    //克隆商品属性
                    foreach (PdProductAttribute attribute in cbProduct.PdProductAttribute.Value)
                    {
                        attribute.ProductSysNo = sysNo;
                        IPdProductAttributeDao.Instance.Create(attribute);
                    }

                    //克隆商品分类关联
                    foreach (PdCategoryAssociation categoryAssociation in cbProduct.PdCategoryAssociation.Value)
                    {
                        categoryAssociation.ProductSysNo = sysNo;
                        IPdCategoryAssociationDao.Instance.Create(categoryAssociation);
                    }

                    ////克隆商品图片
                    //foreach (PdProductImage image in cbProduct.PdProductImage.Value)
                    //{
                    //    image.ProductSysNo = sysNo;
                    //    IPdProductImageDao.Instance.Insert(image);
                    //}

                }
                result.Status = true;
                result.StatusCode = sysNo;
                result.Message = "克隆商品成功";


            }
            else
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "未设置要克隆的商品";
            }
            return result;

        }

        /// <summary>
        /// 更新商品描述文档
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns></returns>
        /// <remarks>2013-07-25 邵斌 创建</remarks>
        public bool UpdateProductDescription(int productSysNo, string html, string phoneHtml = "")
        {
            Cache.DeleteCache.ProductInfo(productSysNo);

            var result = IPdProductDao.Instance.UpdateProductDescription(productSysNo, html, phoneHtml);

            //根据状态写日志
            if (!result)
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新商品{0}商品描述失败", productSysNo),
                                              LogStatus.系统日志目标类型.商品描述,
                                              productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //写成功日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新商品{0}商品描述", productSysNo), LogStatus.系统日志目标类型.商品描述, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

            }

            return result;
        }

        /// <summary>
        /// 更新B2B商品描述文档
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns></returns>
        ///  <remarks>2017-10-12 罗勤瑶 创建</remarks>
        public bool UpdateB2BProductDescription(int productSysNo, string html, string phoneHtml = "")
        {
            // Cache.DeleteCache.ProductInfo(productSysNo);
            var product = IPdProductDao.Instance.GetProduct(productSysNo);
            var productB2B = IPdProductDao.Instance.GetB2BProductByErpCode(product.ErpCode);
            var result = IPdProductDao.Instance.UpdateB2BProductDescription(productB2B.SysNo, html, phoneHtml);

            //根据状态写日志
            if (!result)
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新B2B商品{0}商品描述失败", productSysNo),
                                              LogStatus.系统日志目标类型.商品描述,
                                              productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //写成功日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新B2B商品{0}商品描述", productSysNo), LogStatus.系统日志目标类型.商品描述, productSysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);

            }

            return result;
        }


        /// <summary>
        /// 获取ErpCode
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>产品商品编号</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public string GetProductErpCode(int productSysNo)
        {
            return MemoryProvider.Default.Get(string.Format(KeyConstant.ProductErpCode, productSysNo), () => IPdProductDao.Instance.GetProductErpCode(productSysNo));
        }

        /// <summary>
        /// 获取条形码Barcode
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>Barcode</returns>
        /// <remarks>2016-09-27 罗远康 创建</remarks>
        public string GetProductBarcode(int productSysNo)
        {
            return IPdProductDao.Instance.GetProductBarcode(productSysNo);
        }

        /// <summary>
        /// 获取EASName
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>EasName</returns>
        /// <remarks>2013-12-16 何永东 创建</remarks>
        public string GetProductEasName(int productSysNo)
        {

            var easName = MemoryProvider.Default.Get(string.Format(KeyConstant.ProductEasName, productSysNo), () => IPdProductDao.Instance.GetProductEasName(productSysNo));
            return easName;

        }

        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <returns>返回商品信息和指定的会员等级价格</returns>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        public Pager<CBProductListItem> GetProductListAndPartPrice(int categorySysNo, int customerLevelSysNo,
                                                                   string keyword,
                                                                   int currentPageIndex, int pageSize)
        {
            return IPdProductDao.Instance.GetProductListAndPartPrice(categorySysNo, customerLevelSysNo, keyword,
                                                                     currentPageIndex,
                                                                     pageSize);
        }

        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格（物流App使用）
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>返回物流APP商品信息和指定的会员等级价格</returns>
        /// <remarks>2014-06-05 余勇 创建</remarks>
        public Pager<AppProduct> GetAppProductListAndPartPrice(int categorySysNo, int customerLevelSysNo,
            string keyword,
            int currentPageIndex, int pageSize)
        {
            return IPdProductDao.Instance.GetAppProductListAndPartPrice(categorySysNo, customerLevelSysNo, keyword,
                                                                     currentPageIndex,
                                                                     pageSize);
        }

        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="pager">分页查询参数对象</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        public void ProductSelectorProductSearch(ref Pager<ParaProductSearchFilter> pager)
        {
            IPdProductDao.Instance.ProductSelectorProductSearch(ref pager);
        }


        /// <summary>
        /// 获取单个商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="procutSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        public IList<PdProductIndex> GetAllProduct(List<int> procutSysNos)
        {
            var result = IPdProductDao.Instance.GetAllProduct(procutSysNos);

            return result;
        }

        /// <summary>
        /// 获取单个商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="procutSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        public PdProductIndex GetAllProduct(int procutSysNo)
        {
            var result = IPdProductDao.Instance.GetAllProduct(procutSysNo);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        public IList<PdProductIndex> GetAllProduct()
        {
            return IPdProductDao.Instance.GetAllProduct();
        }

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品统计信息</returns>
        /// <remarks>2013-08-26 郑荣华 创建</remarks>
        public PdProductStatistics GetPdProductStatistics(int pdSysNo)
        {
            return IPdProductDao.Instance.GetPdProductStatistics(pdSysNo);
        }

        /// <summary>
        /// 获取商品详情包括商品类型、价格等
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>返回商品详情</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public ParaProductSearchFilter GetPdProductBySysNo(int pdSysNo)
        {
            return IPdProductDao.Instance.GetPdProductBySysNo(pdSysNo);
        }

        /// <summary>
        /// 通过商品编号获取商品信息
        /// </summary>
        /// <param name="sysNo">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-11-15 余勇 创建</remarks>
        public PdProduct GetProductBySysNo(int sysNo)
        {
            return IPdProductDao.Instance.GetProductBySysNo(sysNo);
        }

        /// <summary>
        /// 通过商品编号获取商品信息
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public PdProduct GetProductByErpCode(string erpCode)
        {
            return IPdProductDao.Instance.GetProductByErpCode(erpCode);
        }

        /// <summary>
        /// 获取商品会员等级价格，若无会员等级则取基础价格
        /// </summary>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>返回商品详情</returns>
        /// <remarks>2013-11-22 余勇 创建</remarks>
        public CBPdProductDetail SelectProductPrice(int customerLevelSysNo, int productSysNo)
        {
            return IPdProductDao.Instance.SelectProductPrice(customerLevelSysNo, productSysNo);
        }

        /// <summary>
        /// 从缓存中获取商品支持的促销提示信息
        /// </summary>
        /// <param name="platformType">促销使用平台</param>
        /// <param name="productSysno">商品sysno.</param>
        /// <returns>商品支持的促销提示信息</returns>
        /// <remarks>
        /// 2013-11-22 杨文兵 创建
        /// </remarks>
        public IList<SpPromotionHint> GetProductPromotionHintsFromCache(PromotionStatus.促销使用平台[] platformType, int productSysno)
        {
            string type = string.Empty;
            if (platformType != null)
            {
                foreach (var pp in platformType)
                {
                    if (!string.IsNullOrEmpty(type))
                    {
                        type += "-";
                    }
                    type += pp.GetHashCode().ToString();
                }
            }
            return Hyt.Infrastructure.Memory.MemoryProvider.Default.Get<IList<SpPromotionHint>>(string.Format(KeyConstant.ProductPromotionHints, productSysno, type), 30, () =>
            {
                return SpPromotionEngineBo.Instance.CheckPromotionHints(platformType, productSysno, true);
            });
        }

        /// <summary>
        /// 获取当前商品集合中上架商品系统编号
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品</param>
        /// <returns>上架商品系统编号</returns>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public IList<int> GetOnlineProduct(int[] productSysNo, bool isFrontProduct = true)
        {
            return IPdProductDao.Instance.GetOnlineProduct(productSysNo, isFrontProduct);
        }

        /// <summary>
        /// 获取当前商品集合
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public IList<int> GetOnlineProduct(int[] productSysNo)
        {
            return IPdProductDao.Instance.GetOnlineProduct(productSysNo);
        }
        #region 商品导入




        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "前台显示名称"},
                {"EasName", "后台显示名称"},
                {"CategoryName", "分类"},
                {"BrandName", "品牌"},
                {"TypeName","类型"},
                {"OriginName","原产地"},
                {"Barcode","条形码"},
                {"GrosWeight","毛重"},
                {"Tax","税率"},
                {"PriceRate","直营利润比例"},
                {"PriceValue","直营分销商利润金额"},
                {"Price","商品价格"},
                {"SalePrice","会员价"},
                {"TradePrice","批发价"},
                {"StoreSalePrice","门店销售价"},
                {"VIPPrice","VIP会员价"},
                {"DiamondPrice","钻石会员价"},
                {"SaleUserPrice","销售合伙人"},
                {"VirtualSales","销量"}
            };
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMappingByYD = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "前台显示名称"},
                {"EasName", "后台显示名称"},
                {"CategoryName", "分类"},
                {"BrandName", "品牌"},
                {"TypeName","类型"},
                {"OriginName","原产地"},
                {"Barcode","条形码"},
                {"GrosWeight","毛重"},
                {"Tax","税率"},
                {"PriceRate","直营利润比例"},
                {"PriceValue","直营分销商利润金额"},
                {"Price","商品价格"},
                {"SalePrice","会员价"},
                {"TradePrice","批发价"},
                {"StoreSalePrice","门店销售价"},
                {"CostPrice","成本价"},
                {"Status","上下架(1上0下)"},
                {"CanFrontEndOrder","是否允许前台下单(1是0否)"},
                {"IsFrontDisplay","是否前台显示(1是0否)"}
            };
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMappingByYS = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "前台显示名称"},
                {"EasName", "后台显示名称"},
                {"CategoryName", "分类"},
                {"BrandName", "品牌"},
                {"TypeName","类型"},
                {"OriginName","原产地"},
                {"Barcode","条形码"},
                {"GrosWeight","毛重"},
                {"Tax","税率"},
                {"PriceRate","直营利润比例"},
                {"PriceValue","直营分销商利润金额"},
                {"Price","商品价格"},
                {"SalePrice","会员价"},
                {"TradePrice","批发价"},
                {"StoreSalePrice","门店销售价"},
                {"ProductShortTitle","商品简称"}
            };

        /// <summary>
        /// 设置价格
        /// </summary>
        /// <param name="price">价格值</param>
        /// <param name="priceSource">价格源</param>
        /// <param name="sourceSysNo">价格源编号</param>
        /// <returns></returns>
        /// <remarks>2016-09-26 杨浩 创建</remarks>
        private PdPrice SetPriceModel(decimal price, int priceSource, int sourceSysNo)
        {
            var model = new PdPrice();
            model.ProductSysNo = 1;
            model.Price = price;
            model.PriceSource = priceSource;
            model.SourceSysNo = sourceSysNo;
            model.Status = 1;
            return model;
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 杨浩 创建</remarks>
        public Result ImportExcel<T>(Stream stream) where T : ITemplateBase
        {
            var result = new Result();

            DataTable dt = null;

            var dicColsMapping = ExcelUtil.GetDicColsMapping<T>();

            var cols = dicColsMapping.Select(p => p.Value).ToArray();

            var current = BLL.Authentication.AdminAuthenticationBo.Instance.Current;

            int operatorSysno = current == null ? 0 : current.Base.SysNo;

            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][dicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][dicColsMapping["Barcode"]].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ErpCode) || string.IsNullOrWhiteSpace(Barcode))
                {
                    result.Message = string.Format("excel表第{0}行的商品编码或条形码不能有空值", excelRow);
                    result.Status = false;
                    return result;
                }

                var dataRows = dt.Select(dicColsMapping["ErpCode"].ToString() + "='" + ErpCode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品编码【" + ErpCode + "】有重复请修改再导入！");
                    return result;
                }

                dataRows = dt.Select(dicColsMapping["Barcode"].ToString() + "='" + Barcode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品条码【" + Barcode + "】有重复请修改再导入！");
                    return result;
                }



                //前台显示名称
                var ProductName = dt.Rows[i][dicColsMapping["ProductName"]].ToString().Trim();
                //后台显示名称
                var EasName = dt.Rows[i][dicColsMapping["EasName"]].ToString().Trim();
                //商品类目
                var CategoryName = dt.Rows[i][dicColsMapping["CategoryName"]].ToString().Trim();
                PdCategorySql PdCmodel = new PdCategorySql();
                PdCmodel.ParentSysNo = 0;
                PdCmodel.CategoryName = CategoryName;
                PdCmodel.Code = "";
                PdCmodel.SeoTitle = CategoryName;
                PdCmodel.SeoKeyword = CategoryName;
                PdCmodel.SeoDescription = CategoryName;
                PdCmodel.TemplateSysNo = 0;
                PdCmodel.IsOnline = 1;
                PdCmodel.Status = 1;
                PdCmodel.CreatedBy = operatorSysno;
                PdCmodel.CreatedDate = DateTime.Now;
                PdCmodel.LastUpdateBy = operatorSysno;
                PdCmodel.LastUpdateDate = DateTime.Now;

                PdCategoryAssociation PdCAmodel = new PdCategoryAssociation();
                PdCAmodel.IsMaster = 1;
                PdCAmodel.CreatedBy = operatorSysno;
                PdCAmodel.CreatedDate = DateTime.Now;
                PdCAmodel.LastUpdateBy = operatorSysno;
                PdCAmodel.LastUpdateDate = DateTime.Now;
                //品牌
                int BrandSysNo;
                var BrandName = dt.Rows[i][dicColsMapping["BrandName"]].ToString();
                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                //判断商品品牌是否存在
                if (pEnity != null)
                {
                    BrandSysNo = pEnity.SysNo;
                }
                else
                {
                    var pmodel = new PdBrand();
                    pmodel.Name = BrandName;
                    pmodel.Status = 1;
                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                }
                //类型
                int productType = 0;
                var TypeName = dt.Rows[i][dicColsMapping["TypeName"]].ToString();
                var pTypeList = new List<SelectListItem>();

                Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
                var typeInfo = pTypeList.Where(x => x.Text == TypeName).FirstOrDefault();

                if (typeInfo == null)
                {
                    result.Message = string.Format("excel表第{0}行类型不存在", excelRow);
                    result.Status = false;
                    return result;
                }

                productType = int.Parse(typeInfo.Value);


                //原产地
                int OriginSysNo;
                var OriginName = dt.Rows[i][dicColsMapping["OriginName"]].ToString();
                Origin oEnity = OriginBo.Instance.GetEntityByName(OriginName);
                //判断国家是否存在
                if (oEnity != null)
                {
                    OriginSysNo = oEnity.SysNo;
                }
                else
                {
                    var omodel = new Origin();
                    omodel.Origin_Name = OriginName;
                    omodel.Origin_Img = "";
                    omodel.CreatedDate = DateTime.Now;
                    omodel.CreatedBy = operatorSysno;
                    omodel.LastUpdateBy = operatorSysno;
                    omodel.LastUpdateDate = DateTime.Now;
                    OriginSysNo = IOriginDao.Instance.Insert(omodel);
                }
                //毛重 	
                var GrosWeight = dt.Rows[i][dicColsMapping["GrosWeight"]].ToString();
                Decimal typeGrosWeight = 0;
                if (!Decimal.TryParse(GrosWeight, out typeGrosWeight))
                {
                    result.Message = string.Format("excel表第{0}行毛重必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //税率 	
                var Tax = dt.Rows[i][dicColsMapping["Tax"]].ToString();
                Decimal typeTax = 0;
                if (!Decimal.TryParse(Tax, out typeTax))
                {
                    result.Message = string.Format("excel表第{0}行税率必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营利润比例 	
                var PriceRate = dt.Rows[i][dicColsMapping["PriceRate"]].ToString();
                Decimal typePriceRate = 0;
                if (!Decimal.TryParse(PriceRate, out typePriceRate))
                {
                    result.Message = string.Format("excel表第{0}行直营利润比例必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营分销商利润金额 	
                var PriceValue = dt.Rows[i][dicColsMapping["PriceValue"]].ToString();
                Decimal typePriceValue = 0;
                if (!Decimal.TryParse(PriceValue, out typePriceValue))
                {
                    result.Message = string.Format("excel表第{0}行直营分销商利润金额必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //商品价格 	
                var Price = dt.Rows[i][dicColsMapping["Price"]].ToString();
                Decimal typePrice = 0;
                if (!Decimal.TryParse(Price, out typePrice))
                {
                    result.Message = string.Format("excel表第{0}行商品价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                Price = decimal.Round(decimal.Parse(Price), 0).ToString();

                var prmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格, 0);// new PdPrice();


                //会员价格 	
                var SalePrice = dt.Rows[i][dicColsMapping["SalePrice"]].ToString();
                Decimal typeSalePrice = 0;
                if (!Decimal.TryParse(SalePrice, out typeSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sprmodel = SetPriceModel(typeSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 1);// new PdPrice();

                string VIPPrice = "0";
                if (dt.Columns.Contains("VIPPrice"))
                    VIPPrice = dt.Rows[i][dicColsMapping["VIPPrice"]].ToString();//VIP会员价              

                PdPrice Viprmodel = null;
                if (!string.IsNullOrEmpty(VIPPrice))
                {
                    Decimal typeVIPPrice = 0;
                    if (!Decimal.TryParse(VIPPrice, out typeVIPPrice))
                    {
                        result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                        result.Status = false;
                        return result;
                    }
                    Viprmodel = SetPriceModel(typeVIPPrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 6);// new PdPrice();
                }

                var DiamondPrice = "0";
                if (dt.Columns.Contains("DiamondPrice"))
                    DiamondPrice = dt.Rows[i][dicColsMapping["DiamondPrice"]].ToString(); //钻石会员价

                PdPrice dprmodel = null;
                if (!string.IsNullOrEmpty(DiamondPrice))
                {
                    Decimal typeDiamondPrice = 0;

                    if (!Decimal.TryParse(DiamondPrice, out typeDiamondPrice))
                    {
                        result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                        result.Status = false;
                        return result;
                    }
                    dprmodel = SetPriceModel(typeDiamondPrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 7);// new PdPrice();
                }

                var TradePrice = "0";
                if (dt.Columns.Contains("TradePrice"))
                    TradePrice = dt.Rows[i][dicColsMapping["TradePrice"]].ToString();//批发价

                Decimal typeTradePrice = 0;
                if (!Decimal.TryParse(TradePrice, out typeTradePrice))
                {

                    result.Message = string.Format("excel表第{0}行批发价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }


                var StoreSalePrice = "0";
                if (dt.Columns.Contains("StoreSalePrice"))
                    TradePrice = dt.Rows[i][dicColsMapping["StoreSalePrice"]].ToString();//门店销售价

                Decimal typeStoreSalePrice = 0;
                if (!Decimal.TryParse(StoreSalePrice, out typeStoreSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行门店销售价必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sspmodel = SetPriceModel(typeStoreSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.线下门店价, 0);// new PdPrice();


                var sales = "0";

                if (dt.Columns.Contains("VirtualSales"))
                    sales = dt.Rows[i][dicColsMapping["VirtualSales"]].ToString();

                Decimal typeSales = 0;
                Decimal.TryParse(sales, out typeSales);


                var currentInfo = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                int agentSysNo = 1;
                int dealerSysNo = 0;
                if (currentInfo.Dealer != null)
                {
                    dealerSysNo = currentInfo.Dealer.SysNo;
                    agentSysNo = currentInfo.DealerCreatedBy;
                }

                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    Sales = Convert.ToInt32(typeSales),
                    ProductName = ProductName,
                    EasName = EasName,
                    BrandSysNo = BrandSysNo,
                    ProductType = productType,
                    OriginSysNo = OriginSysNo,
                    Barcode = Barcode,
                    GrosWeight = Decimal.Parse(GrosWeight),
                    Tax = Tax,
                    PriceRate = Decimal.Parse(PriceRate),
                    PriceValue = Decimal.Parse(PriceValue),
                    TradePrice = Decimal.Parse(TradePrice),
                    PdPrice = prmodel,
                    PdVIPPrice = Viprmodel,
                    PdDiamondPrice = dprmodel,
                    PdSalePrice = sprmodel,
                    PdStoreSalePrice = sspmodel,
                    PdCategorySql = PdCmodel,
                    PdCategoryAssociation = PdCAmodel,
                    DealerSysNo = dealerSysNo,
                    AgentSysNo = agentSysNo,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);

            }

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();

            foreach (var excelModel in excellst)
            {
                var productInfo = lstExisted.Where(x => x.ErpCode == excelModel.ErpCode).FirstOrDefault();
                var barcodeList = lstExisted.Where(x => x.Barcode == excelModel.Barcode).ToList();

                if (productInfo != null)
                {
                    if (barcodeList.Count > 1)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }

                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    if (barcodeList.Count > 0)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IPdProductDao.Instance.CreatePdProduct(lstToInsert);
                IPdProductDao.Instance.UpdateExcelProduct(lstToUpdate, lstExisted);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);

                result.Message = string.Format("数据更新错误:{0}", ex.Message);
                result.Status = false;
                return result;
            }

            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                result.Message = "导入的数据为空!";
                result.Status = false;
                return result;
            }

            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            result.Message = msg;
            result.Status = true;
            return result;
        }


        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcel(Stream stream, int operatorSysno)
        {
            var result = new Result();

            DataTable dt = null;

            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][DicColsMapping["Barcode"]].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ErpCode) || string.IsNullOrWhiteSpace(Barcode))
                {
                    result.Message = string.Format("excel表第{0}行的商品编码或条形码不能有空值", excelRow);
                    result.Status = false;
                    return result;
                }

                var dataRows = dt.Select(DicColsMapping["ErpCode"].ToString() + "='" + ErpCode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品编码【" + ErpCode + "】有重复请修改再导入！");
                    return result;
                }

                dataRows = dt.Select(DicColsMapping["Barcode"].ToString() + "='" + Barcode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品条码【" + Barcode + "】有重复请修改再导入！");
                    return result;
                }


                //前台显示名称
                var ProductName = dt.Rows[i][DicColsMapping["ProductName"]].ToString().Trim();
                //后台显示名称
                var EasName = dt.Rows[i][DicColsMapping["EasName"]].ToString().Trim();
                //商品类目
                var CategoryName = dt.Rows[i][DicColsMapping["CategoryName"]].ToString().Trim();
                PdCategorySql PdCmodel = new PdCategorySql();
                PdCmodel.ParentSysNo = 0;
                PdCmodel.CategoryName = CategoryName;
                PdCmodel.Code = "";
                PdCmodel.SeoTitle = CategoryName;
                PdCmodel.SeoKeyword = CategoryName;
                PdCmodel.SeoDescription = CategoryName;
                PdCmodel.TemplateSysNo = 0;
                PdCmodel.IsOnline = 1;
                PdCmodel.Status = 1;
                PdCmodel.CreatedBy = operatorSysno;
                PdCmodel.CreatedDate = DateTime.Now;
                PdCmodel.LastUpdateBy = operatorSysno;
                PdCmodel.LastUpdateDate = DateTime.Now;

                PdCategoryAssociation PdCAmodel = new PdCategoryAssociation();
                PdCAmodel.IsMaster = 1;
                PdCAmodel.CreatedBy = operatorSysno;
                PdCAmodel.CreatedDate = DateTime.Now;
                PdCAmodel.LastUpdateBy = operatorSysno;
                PdCAmodel.LastUpdateDate = DateTime.Now;
                //品牌
                int BrandSysNo;
                var BrandName = dt.Rows[i][DicColsMapping["BrandName"]].ToString();
                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                //判断商品品牌是否存在
                if (pEnity != null)
                {
                    BrandSysNo = pEnity.SysNo;
                }
                else
                {
                    var pmodel = new PdBrand();
                    pmodel.Name = BrandName;
                    pmodel.Status = 1;
                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                }
                //类型
                int productType = 0;
                var TypeName = dt.Rows[i][DicColsMapping["TypeName"]].ToString();
                var pTypeList = new List<SelectListItem>();

                Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
                var typeInfo = pTypeList.Where(x => x.Text == TypeName).FirstOrDefault();

                if (typeInfo == null)
                {
                    result.Message = string.Format("excel表第{0}行类型不存在", excelRow);
                    result.Status = false;
                    return result;
                }

                productType = int.Parse(typeInfo.Value);


                //原产地
                int OriginSysNo;
                var OriginName = dt.Rows[i][DicColsMapping["OriginName"]].ToString();
                Origin oEnity = OriginBo.Instance.GetEntityByName(OriginName);
                //判断国家是否存在
                if (oEnity != null)
                {
                    OriginSysNo = oEnity.SysNo;
                }
                else
                {
                    var omodel = new Origin();
                    omodel.Origin_Name = OriginName;
                    omodel.Origin_Img = "";
                    omodel.CreatedDate = DateTime.Now;
                    omodel.CreatedBy = operatorSysno;
                    omodel.LastUpdateBy = operatorSysno;
                    omodel.LastUpdateDate = DateTime.Now;
                    OriginSysNo = IOriginDao.Instance.Insert(omodel);
                }
                //毛重 	
                var GrosWeight = dt.Rows[i][DicColsMapping["GrosWeight"]].ToString();
                Decimal typeGrosWeight = 0;
                if (!Decimal.TryParse(GrosWeight, out typeGrosWeight))
                {
                    result.Message = string.Format("excel表第{0}行毛重必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //税率 	
                var Tax = dt.Rows[i][DicColsMapping["Tax"]].ToString();
                Decimal typeTax = 0;
                if (!Decimal.TryParse(Tax, out typeTax))
                {
                    result.Message = string.Format("excel表第{0}行税率必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营利润比例 	
                var PriceRate = dt.Rows[i][DicColsMapping["PriceRate"]].ToString();
                Decimal typePriceRate = 0;
                if (!Decimal.TryParse(PriceRate, out typePriceRate))
                {
                    result.Message = string.Format("excel表第{0}行直营利润比例必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营分销商利润金额 	
                var PriceValue = dt.Rows[i][DicColsMapping["PriceValue"]].ToString();
                Decimal typePriceValue = 0;
                if (!Decimal.TryParse(PriceValue, out typePriceValue))
                {
                    result.Message = string.Format("excel表第{0}行直营分销商利润金额必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //商品价格 	
                var Price = dt.Rows[i][DicColsMapping["Price"]].ToString();
                Decimal typePrice = 0;
                if (!Decimal.TryParse(Price, out typePrice))
                {
                    result.Message = string.Format("excel表第{0}行商品价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                Price = decimal.Round(decimal.Parse(Price), 0).ToString();

                var prmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格, 0);// new PdPrice();


                //会员价格 	
                var SalePrice = dt.Rows[i][DicColsMapping["SalePrice"]].ToString();
                Decimal typeSalePrice = 0;
                if (!Decimal.TryParse(SalePrice, out typeSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sprmodel = SetPriceModel(typeSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 1);// new PdPrice();

                //VIP会员价 	
                var VIPPrice = dt.Rows[i][DicColsMapping["VIPPrice"]].ToString();
                PdPrice Viprmodel = null;
                if (!string.IsNullOrEmpty(VIPPrice))
                {
                    Decimal typeVIPPrice = 0;
                    if (!Decimal.TryParse(VIPPrice, out typeVIPPrice))
                    {
                        result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                        result.Status = false;
                        return result;
                    }
                    Viprmodel = SetPriceModel(typeVIPPrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 6);// new PdPrice();
                }
                //钻石会员价	
                var DiamondPrice = dt.Rows[i][DicColsMapping["DiamondPrice"]].ToString();
                PdPrice dprmodel = null;
                if (!string.IsNullOrEmpty(DiamondPrice))
                {
                    Decimal typeDiamondPrice = 0;

                    if (!Decimal.TryParse(DiamondPrice, out typeDiamondPrice))
                    {
                        result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                        result.Status = false;
                        return result;
                    }
                    dprmodel = SetPriceModel(typeDiamondPrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 7);// new PdPrice();
                }

                //钻石会员价	
                var SaleUserPrice = dt.Rows[i][DicColsMapping["SaleUserPrice"]].ToString();
                PdPrice suprmodel = null;
                if (!string.IsNullOrEmpty(DiamondPrice))
                {
                    Decimal typeDiamondPrice = 0;

                    if (!Decimal.TryParse(SaleUserPrice, out typeDiamondPrice))
                    {
                        result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                        result.Status = false;
                        return result;
                    }
                    suprmodel = SetPriceModel(typeDiamondPrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 8);// new PdPrice();
                }
                //批发价
                var TradePrice = dt.Rows[i][DicColsMapping["TradePrice"]].ToString();
                Decimal typeTradePrice = 0;
                if (!Decimal.TryParse(TradePrice, out typeTradePrice))
                {

                    result.Message = string.Format("excel表第{0}行批发价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                //门店销售价 	
                var StoreSalePrice = dt.Rows[i][DicColsMapping["StoreSalePrice"]].ToString();
                Decimal typeStoreSalePrice = 0;
                if (!Decimal.TryParse(StoreSalePrice, out typeStoreSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行门店销售价必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sspmodel = SetPriceModel(typeStoreSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.线下门店价, 0);// new PdPrice();


                var sales = dt.Rows[i][DicColsMapping["VirtualSales"]].ToString();
                Decimal typeSales = 0;
                Decimal.TryParse(sales, out typeSales);


                var currentInfo = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                int agentSysNo = 1;
                int dealerSysNo = 0;
                if (currentInfo.Dealer != null)
                {
                    dealerSysNo = currentInfo.Dealer.SysNo;
                    agentSysNo = currentInfo.DealerCreatedBy;
                }

                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    Sales = Convert.ToInt32(typeSales),
                    ProductName = ProductName,
                    EasName = EasName,
                    BrandSysNo = BrandSysNo,
                    ProductType = productType,
                    OriginSysNo = OriginSysNo,
                    Barcode = Barcode,
                    GrosWeight = Decimal.Parse(GrosWeight),
                    Tax = Tax,
                    PriceRate = Decimal.Parse(PriceRate),
                    PriceValue = Decimal.Parse(PriceValue),
                    TradePrice = Decimal.Parse(TradePrice),
                    PdPrice = prmodel,
                    PdVIPPrice = Viprmodel,
                    PdDiamondPrice = dprmodel,
                    SaleUserPrice = suprmodel,
                    PdSalePrice = sprmodel,
                    PdStoreSalePrice = sspmodel,
                    PdCategorySql = PdCmodel,
                    PdCategoryAssociation = PdCAmodel,
                    DealerSysNo = dealerSysNo,
                    AgentSysNo = agentSysNo,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);

            }

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();

            foreach (var excelModel in excellst)
            {
                var productInfo = lstExisted.Where(x => x.ErpCode == excelModel.ErpCode).FirstOrDefault();
                var barcodeList = lstExisted.Where(x => x.Barcode == excelModel.Barcode).ToList();

                if (productInfo != null)
                {
                    if (barcodeList.Count > 1)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }

                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    if (barcodeList.Count > 0)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IPdProductDao.Instance.CreatePdProduct(lstToInsert);
                IPdProductDao.Instance.UpdateExcelProduct(lstToUpdate, lstExisted);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);

                result.Message = string.Format("数据更新错误:{0}", ex.Message);
                result.Status = false;
                return result;
            }

            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                result.Message = "导入的数据为空!";
                result.Status = false;
                return result;
            }

            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            result.Message = msg;
            result.Status = true;
            return result;
        }

        #endregion

        #region 洋食网导入
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcelYS(Stream stream, int operatorSysno)
        {
            var result = new Result();

            DataTable dt = null;

            var cols = DicColsMappingByYS.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMappingByYS["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][DicColsMappingByYS["Barcode"]].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ErpCode) || string.IsNullOrWhiteSpace(Barcode))
                {
                    result.Message = string.Format("excel表第{0}行的商品编码或条形码不能有空值", excelRow);
                    result.Status = false;
                    return result;
                }

                var dataRows = dt.Select(DicColsMappingByYS["ErpCode"].ToString() + "='" + ErpCode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品编码【" + ErpCode + "】有重复请修改再导入！");
                    return result;
                }

                dataRows = dt.Select(DicColsMappingByYS["Barcode"].ToString() + "='" + Barcode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品条码【" + Barcode + "】有重复请修改再导入！");
                    return result;
                }


                //前台显示名称
                var ProductName = dt.Rows[i][DicColsMappingByYS["ProductName"]].ToString().Trim();
                //后台显示名称
                var EasName = dt.Rows[i][DicColsMappingByYS["EasName"]].ToString().Trim();
                //商品类目
                var CategoryName = dt.Rows[i][DicColsMappingByYS["CategoryName"]].ToString().Trim();
                PdCategorySql PdCmodel = new PdCategorySql();
                PdCmodel.ParentSysNo = 0;
                PdCmodel.CategoryName = CategoryName;
                PdCmodel.Code = "";
                PdCmodel.SeoTitle = CategoryName;
                PdCmodel.SeoKeyword = CategoryName;
                PdCmodel.SeoDescription = CategoryName;
                PdCmodel.TemplateSysNo = 0;
                PdCmodel.IsOnline = 1;
                PdCmodel.Status = 1;
                PdCmodel.CreatedBy = operatorSysno;
                PdCmodel.CreatedDate = DateTime.Now;
                PdCmodel.LastUpdateBy = operatorSysno;
                PdCmodel.LastUpdateDate = DateTime.Now;

                PdCategoryAssociation PdCAmodel = new PdCategoryAssociation();
                PdCAmodel.IsMaster = 1;
                PdCAmodel.CreatedBy = operatorSysno;
                PdCAmodel.CreatedDate = DateTime.Now;
                PdCAmodel.LastUpdateBy = operatorSysno;
                PdCAmodel.LastUpdateDate = DateTime.Now;
                //品牌
                int BrandSysNo;
                var BrandName = dt.Rows[i][DicColsMappingByYS["BrandName"]].ToString();
                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                //判断商品品牌是否存在
                if (pEnity != null)
                {
                    BrandSysNo = pEnity.SysNo;
                }
                else
                {
                    var pmodel = new PdBrand();
                    pmodel.Name = BrandName;
                    pmodel.Status = 1;
                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                }
                //类型
                int productType = 0;
                var TypeName = dt.Rows[i][DicColsMappingByYS["TypeName"]].ToString();
                var pTypeList = new List<SelectListItem>();

                Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
                var typeInfo = pTypeList.Where(x => x.Text == TypeName).FirstOrDefault();

                if (typeInfo == null)
                {
                    result.Message = string.Format("excel表第{0}行类型不存在", excelRow);
                    result.Status = false;
                    return result;
                }

                productType = int.Parse(typeInfo.Value);


                //原产地
                int OriginSysNo;
                var OriginName = dt.Rows[i][DicColsMappingByYS["OriginName"]].ToString();
                Origin oEnity = OriginBo.Instance.GetEntityByName(OriginName);
                //判断国家是否存在
                if (oEnity != null)
                {
                    OriginSysNo = oEnity.SysNo;
                }
                else
                {
                    var omodel = new Origin();
                    omodel.Origin_Name = OriginName;
                    omodel.Origin_Img = "";
                    omodel.CreatedDate = DateTime.Now;
                    omodel.CreatedBy = operatorSysno;
                    omodel.LastUpdateBy = operatorSysno;
                    omodel.LastUpdateDate = DateTime.Now;
                    OriginSysNo = IOriginDao.Instance.Insert(omodel);
                }
                //毛重 	
                var GrosWeight = dt.Rows[i][DicColsMappingByYS["GrosWeight"]].ToString();
                Decimal typeGrosWeight = 0;
                if (!Decimal.TryParse(GrosWeight, out typeGrosWeight))
                {
                    result.Message = string.Format("excel表第{0}行毛重必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //税率 	
                var Tax = dt.Rows[i][DicColsMappingByYS["Tax"]].ToString();
                Decimal typeTax = 0;
                if (!Decimal.TryParse(Tax, out typeTax))
                {
                    result.Message = string.Format("excel表第{0}行税率必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营利润比例 	
                var PriceRate = dt.Rows[i][DicColsMappingByYS["PriceRate"]].ToString();
                Decimal typePriceRate = 0;
                if (!Decimal.TryParse(PriceRate, out typePriceRate))
                {
                    result.Message = string.Format("excel表第{0}行直营利润比例必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营分销商利润金额 	
                var PriceValue = dt.Rows[i][DicColsMappingByYS["PriceValue"]].ToString();
                Decimal typePriceValue = 0;
                if (!Decimal.TryParse(PriceValue, out typePriceValue))
                {
                    result.Message = string.Format("excel表第{0}行直营分销商利润金额必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //商品价格 	
                var Price = dt.Rows[i][DicColsMappingByYS["Price"]].ToString();
                Decimal typePrice = 0;
                if (!Decimal.TryParse(Price, out typePrice))
                {
                    result.Message = string.Format("excel表第{0}行商品价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                Price = decimal.Round(decimal.Parse(Price), 0).ToString();

                var prmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格, 0);// new PdPrice();


                //会员价格 	
                var SalePrice = dt.Rows[i][DicColsMappingByYS["SalePrice"]].ToString();
                Decimal typeSalePrice = 0;
                if (!Decimal.TryParse(SalePrice, out typeSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sprmodel = SetPriceModel(typeSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 1);// new PdPrice();

                //批发价
                var TradePrice = dt.Rows[i][DicColsMappingByYS["TradePrice"]].ToString();
                Decimal typeTradePrice = 0;
                if (!Decimal.TryParse(TradePrice, out typeTradePrice))
                {

                    result.Message = string.Format("excel表第{0}行批发价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                //门店销售价 	
                var StoreSalePrice = dt.Rows[i][DicColsMappingByYS["StoreSalePrice"]].ToString();
                Decimal typeStoreSalePrice = 0;
                if (!Decimal.TryParse(StoreSalePrice, out typeStoreSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行门店销售价必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sspmodel = SetPriceModel(typeStoreSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.线下门店价, 0);// new PdPrice();
                //商品简称
                var ProductShortTitle = dt.Rows[i][DicColsMappingByYS["ProductShortTitle"]].ToString();

                var currentInfo = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                int agentSysNo = 1;
                int dealerSysNo = 0;
                if (currentInfo.Dealer != null)
                {
                    dealerSysNo = currentInfo.Dealer.SysNo;
                    agentSysNo = currentInfo.DealerCreatedBy;
                }

                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    ProductName = ProductName,
                    EasName = EasName,
                    BrandSysNo = BrandSysNo,
                    ProductType = productType,
                    OriginSysNo = OriginSysNo,
                    Barcode = Barcode,
                    GrosWeight = Decimal.Parse(GrosWeight),
                    Tax = Tax,
                    PriceRate = Decimal.Parse(PriceRate),
                    PriceValue = Decimal.Parse(PriceValue),
                    TradePrice = Decimal.Parse(TradePrice),
                    PdPrice = prmodel,
                    PdSalePrice = sprmodel,
                    PdStoreSalePrice = sspmodel,
                    PdCategorySql = PdCmodel,
                    PdCategoryAssociation = PdCAmodel,
                    DealerSysNo = dealerSysNo,
                    AgentSysNo = agentSysNo,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now,
                    ProductShortTitle = ProductShortTitle
                };
                excellst.Add(model);

            }

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();

            foreach (var excelModel in excellst)
            {
                var productInfo = lstExisted.Where(x => x.ErpCode == excelModel.ErpCode).FirstOrDefault();
                var barcodeList = lstExisted.Where(x => x.Barcode == excelModel.Barcode).ToList();

                if (productInfo != null)
                {
                    if (barcodeList.Count > 1)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }

                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    if (barcodeList.Count > 0)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IPdProductDao.Instance.CreatePdProduct(lstToInsert);
                IPdProductDao.Instance.UpdateExcelProductByYS(lstToUpdate, lstExisted);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);

                result.Message = string.Format("数据更新错误:{0}", ex.Message);
                result.Status = false;
                return result;
            }

            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                result.Message = "导入的数据为空!";
                result.Status = false;
                return result;
            }

            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            result.Message = msg;
            result.Status = true;
            return result;
        }


        #endregion

        #region 壹号洋店导入

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcelByYD(Stream stream, int operatorSysno)
        {
            var result = new Result();

            DataTable dt = null;

            var cols = DicColsMappingByYD.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMappingByYD["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][DicColsMappingByYD["Barcode"]].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ErpCode) || string.IsNullOrWhiteSpace(Barcode))
                {
                    result.Message = string.Format("excel表第{0}行的商品编码或条形码不能有空值", excelRow);
                    result.Status = false;
                    return result;
                }

                var dataRows = dt.Select(DicColsMappingByYD["ErpCode"].ToString() + "='" + ErpCode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品编码【" + ErpCode + "】有重复请修改再导入！");
                    return result;
                }

                dataRows = dt.Select(DicColsMappingByYD["Barcode"].ToString() + "='" + Barcode + "'");
                if (dataRows.Length > 1)
                {
                    result.Message = string.Format("导入数据商品条码【" + Barcode + "】有重复请修改再导入！");
                    return result;
                }


                //前台显示名称
                var ProductName = dt.Rows[i][DicColsMappingByYD["ProductName"]].ToString().Trim();
                //后台显示名称
                var EasName = dt.Rows[i][DicColsMappingByYD["EasName"]].ToString().Trim();
                //商品类目
                var CategoryName = dt.Rows[i][DicColsMappingByYD["CategoryName"]].ToString().Trim();
                PdCategorySql PdCmodel = new PdCategorySql();
                PdCmodel.ParentSysNo = 0;
                PdCmodel.CategoryName = CategoryName;
                PdCmodel.Code = "";
                PdCmodel.SeoTitle = CategoryName;
                PdCmodel.SeoKeyword = CategoryName;
                PdCmodel.SeoDescription = CategoryName;
                PdCmodel.TemplateSysNo = 0;
                PdCmodel.IsOnline = 1;
                PdCmodel.Status = 1;
                PdCmodel.CreatedBy = operatorSysno;
                PdCmodel.CreatedDate = DateTime.Now;
                PdCmodel.LastUpdateBy = operatorSysno;
                PdCmodel.LastUpdateDate = DateTime.Now;

                PdCategoryAssociation PdCAmodel = new PdCategoryAssociation();
                PdCAmodel.IsMaster = 1;
                PdCAmodel.CreatedBy = operatorSysno;
                PdCAmodel.CreatedDate = DateTime.Now;
                PdCAmodel.LastUpdateBy = operatorSysno;
                PdCAmodel.LastUpdateDate = DateTime.Now;
                //品牌
                int BrandSysNo;
                var BrandName = dt.Rows[i][DicColsMappingByYD["BrandName"]].ToString();
                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                //判断商品品牌是否存在
                if (pEnity != null)
                {
                    BrandSysNo = pEnity.SysNo;
                }
                else
                {
                    var pmodel = new PdBrand();
                    pmodel.Name = BrandName;
                    pmodel.Status = 1;
                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                }
                //类型
                int productType = 0;
                var TypeName = dt.Rows[i][DicColsMappingByYD["TypeName"]].ToString();
                var pTypeList = new List<SelectListItem>();

                Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
                var typeInfo = pTypeList.Where(x => x.Text == TypeName).FirstOrDefault();

                if (typeInfo == null)
                {
                    result.Message = string.Format("excel表第{0}行类型不存在", excelRow);
                    result.Status = false;
                    return result;
                }

                productType = int.Parse(typeInfo.Value);


                //原产地
                int OriginSysNo;
                var OriginName = dt.Rows[i][DicColsMappingByYD["OriginName"]].ToString();
                Origin oEnity = OriginBo.Instance.GetEntityByName(OriginName);
                //判断国家是否存在
                if (oEnity != null)
                {
                    OriginSysNo = oEnity.SysNo;
                }
                else
                {
                    var omodel = new Origin();
                    omodel.Origin_Name = OriginName;
                    omodel.Origin_Img = "";
                    omodel.CreatedDate = DateTime.Now;
                    omodel.CreatedBy = operatorSysno;
                    omodel.LastUpdateBy = operatorSysno;
                    omodel.LastUpdateDate = DateTime.Now;
                    OriginSysNo = IOriginDao.Instance.Insert(omodel);
                }
                //毛重 	
                var GrosWeight = dt.Rows[i][DicColsMappingByYD["GrosWeight"]].ToString();
                Decimal typeGrosWeight = 0;
                if (!Decimal.TryParse(GrosWeight, out typeGrosWeight))
                {
                    result.Message = string.Format("excel表第{0}行毛重必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //税率 	
                var Tax = dt.Rows[i][DicColsMappingByYD["Tax"]].ToString();
                Decimal typeTax = 0;
                if (!Decimal.TryParse(Tax, out typeTax))
                {
                    result.Message = string.Format("excel表第{0}行税率必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营利润比例 	
                var PriceRate = dt.Rows[i][DicColsMappingByYD["PriceRate"]].ToString();
                Decimal typePriceRate = 0;
                if (!Decimal.TryParse(PriceRate, out typePriceRate))
                {
                    result.Message = string.Format("excel表第{0}行直营利润比例必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //直营分销商利润金额 	
                var PriceValue = dt.Rows[i][DicColsMappingByYD["PriceValue"]].ToString();
                Decimal typePriceValue = 0;
                if (!Decimal.TryParse(PriceValue, out typePriceValue))
                {
                    result.Message = string.Format("excel表第{0}行直营分销商利润金额必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //商品价格 	
                var Price = dt.Rows[i][DicColsMappingByYD["Price"]].ToString();
                Decimal typePrice = 0;
                if (!Decimal.TryParse(Price, out typePrice))
                {
                    result.Message = string.Format("excel表第{0}行商品价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }


                var prmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格, 0);// new PdPrice();


                //会员价格 	
                var SalePrice = dt.Rows[i][DicColsMappingByYD["SalePrice"]].ToString();
                Decimal typeSalePrice = 0;
                if (!Decimal.TryParse(SalePrice, out typeSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sprmodel = SetPriceModel(typeSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 1);// new PdPrice();


                //批发价
                var TradePrice = dt.Rows[i][DicColsMappingByYD["TradePrice"]].ToString();
                Decimal typeTradePrice = 0;
                if (!Decimal.TryParse(TradePrice, out typeTradePrice))
                {

                    result.Message = string.Format("excel表第{0}行批发价格必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                //门店销售价 	
                var StoreSalePrice = dt.Rows[i][DicColsMappingByYD["StoreSalePrice"]].ToString();
                Decimal typeStoreSalePrice = 0;
                if (!Decimal.TryParse(StoreSalePrice, out typeStoreSalePrice))
                {
                    result.Message = string.Format("excel表第{0}行门店销售价必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //成本价 	
                var CostPrices = dt.Rows[i][DicColsMappingByYD["CostPrice"]].ToString();
                Decimal typeCostPrice = 0;
                if (!Decimal.TryParse(CostPrices, out typeCostPrice))
                {
                    result.Message = string.Format("excel表第{0}行成本价必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                //商品上下架 	
                var Status = dt.Rows[i][DicColsMappingByYD["Status"]].ToString();
                int typeStatus = 0;
                if (!int.TryParse(Status, out typeStatus))
                {
                    result.Message = string.Format("excel表第{0}行上下架必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                //前台是否允许下单 	
                var CanFrontEndOrder = dt.Rows[i][DicColsMappingByYD["CanFrontEndOrder"]].ToString();
                int typeCanFrontEndOrder = 0;
                if (!int.TryParse(CanFrontEndOrder, out typeCanFrontEndOrder))
                {
                    result.Message = string.Format("excel表第{0}行前台是否允许下单必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }
                //前台是否显示
                var IsFrontDisplay = dt.Rows[i][DicColsMappingByYD["IsFrontDisplay"]].ToString();
                int typeIsFrontDisplay = 0;
                if (!int.TryParse(IsFrontDisplay, out typeIsFrontDisplay))
                {
                    result.Message = string.Format("excel表第{0}行前台是否显示必须为数值", excelRow);
                    result.Status = false;
                    return result;
                }

                var sspmodel = SetPriceModel(typeStoreSalePrice, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.线下门店价, 0);// new PdPrice();

                var currentInfo = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                int agentSysNo = 1;
                int dealerSysNo = 0;
                if (currentInfo.Dealer != null)
                {
                    dealerSysNo = currentInfo.Dealer.SysNo;
                    agentSysNo = currentInfo.DealerCreatedBy;
                }

                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    ProductName = ProductName,
                    EasName = EasName,
                    BrandSysNo = BrandSysNo,
                    ProductType = productType,
                    OriginSysNo = OriginSysNo,
                    Barcode = Barcode,
                    GrosWeight = Decimal.Parse(GrosWeight),
                    Tax = Tax,
                    PriceRate = Decimal.Parse(PriceRate),
                    PriceValue = Decimal.Parse(PriceValue),
                    TradePrice = Decimal.Parse(TradePrice),
                    PdPrice = prmodel,
                    PdSalePrice = sprmodel,
                    PdStoreSalePrice = sspmodel,
                    CostPrice = Decimal.Parse(CostPrices),
                    Status = int.Parse(Status),
                    CanFrontEndOrder = int.Parse(CanFrontEndOrder),
                    IsFrontDisplay = int.Parse(IsFrontDisplay),
                    PdCategorySql = PdCmodel,
                    PdCategoryAssociation = PdCAmodel,
                    DealerSysNo = dealerSysNo,
                    AgentSysNo = agentSysNo,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);

            }

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();

            foreach (var excelModel in excellst)
            {
                var productInfo = lstExisted.Where(x => x.ErpCode == excelModel.ErpCode).FirstOrDefault();
                var barcodeList = lstExisted.Where(x => x.Barcode == excelModel.Barcode).ToList();

                if (productInfo != null)
                {
                    if (barcodeList.Count > 1)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }

                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    if (barcodeList.Count > 0)
                    {
                        result.Message = string.Format("导入的数据条码【" + excelModel.Barcode + "】在系统中有重复，请修改再导入！");
                        return result;
                    }
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IPdProductDao.Instance.CreatePdProduct(lstToInsert);
                IPdProductDao.Instance.UpdateExcelProductByYD(lstToUpdate, lstExisted);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);

                result.Message = string.Format("数据更新错误:{0}", ex.Message);
                result.Status = false;
                return result;
            }

            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                result.Message = "导入的数据为空!";
                result.Status = false;
                return result;
            }

            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            result.Message = msg;
            result.Status = true;
            return result;
        }
        #endregion

        #region 信营商品导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> XYDicColsMapping = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "前台显示名称"},
                {"EasName", "后台显示名称"},
                {"CategoryName", "分类"},
                {"BrandName", "品牌"},
                {"TypeName","类型"},
                {"OriginName","原产地"},
                {"Barcode","条形码"},
                {"GrosWeight","毛重"},
                {"NetWeight","净重"},
                {"Tax","税率"},
                {"PriceRate","直营利润比例"},
                {"PriceValue","直营分销商利润金额"},
                {"Price","商品价格"},
                {"SalePrice","会员价"},
                {"TradePrice","批发价"},
                {"StoreSalePrice","门店销售价"},
                {"","利润率"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportXinYingExcel(Stream stream, int operatorSysno)
        {
            DataTable dt = null;
            var cols = XYDicColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);

            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            bool isContinue = false;
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                #region 数据校验

                int excelRow = i + 2;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        isContinue = true;
                        break;
                        //return new Result
                        //{
                        //    Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1)),
                        //    Status = false
                        //};
                    }
                }

                if (isContinue)
                {
                    isContinue = false;
                    continue;
                }


                //商品编号
                var ErpCode = dt.Rows[i][XYDicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][XYDicColsMapping["Barcode"]].ToString();

                var dataRows = dt.Select(XYDicColsMapping["ErpCode"].ToString() + "='" + ErpCode + "'");
                if (dataRows.Length > 1)
                {
                    return new Result
                    {
                        Message = string.Format("导入数据商品编码【" + ErpCode + "】有重复请修改再导入！"),
                        Status = false
                    };
                }
                dataRows = dt.Select(XYDicColsMapping["Barcode"].ToString() + "='" + Barcode + "'");
                if (dataRows.Length > 1)
                {
                    return new Result
                    {
                        Message = string.Format("导入数据商品条码【" + Barcode + "】有重复请修改再导入！"),
                        Status = false
                    };
                }
                #endregion

                #region 产品、类别、品牌 赋值
                //前台显示名称
                var ProductName = dt.Rows[i][XYDicColsMapping["ProductName"]].ToString().Trim();
                //后台显示名称
                var EasName = dt.Rows[i][XYDicColsMapping["EasName"]].ToString().Trim();
                //商品类目
                var CategoryName = dt.Rows[i][XYDicColsMapping["CategoryName"]].ToString().Trim();
                PdCategorySql PdCmodel = new PdCategorySql();
                PdCmodel.ParentSysNo = 0;
                PdCmodel.CategoryName = CategoryName;
                PdCmodel.Code = "";
                PdCmodel.SeoTitle = CategoryName;
                PdCmodel.SeoKeyword = CategoryName;
                PdCmodel.SeoDescription = CategoryName;
                PdCmodel.TemplateSysNo = 0;
                PdCmodel.IsOnline = 1;
                PdCmodel.Status = 1;
                PdCmodel.CreatedBy = operatorSysno;
                PdCmodel.CreatedDate = DateTime.Now;
                PdCmodel.LastUpdateBy = operatorSysno;
                PdCmodel.LastUpdateDate = DateTime.Now;

                var PdCAmodel = new PdCategoryAssociation();
                PdCAmodel.IsMaster = 1;
                PdCAmodel.CreatedBy = operatorSysno;
                PdCAmodel.CreatedDate = DateTime.Now;
                PdCAmodel.LastUpdateBy = operatorSysno;
                PdCAmodel.LastUpdateDate = DateTime.Now;
                //品牌
                int BrandSysNo;
                var BrandName = dt.Rows[i][XYDicColsMapping["BrandName"]].ToString();
                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                //判断商品品牌是否存在
                if (pEnity != null)
                {
                    BrandSysNo = pEnity.SysNo;
                }
                else
                {
                    PdBrand pmodel = new PdBrand();
                    pmodel.Name = BrandName;
                    pmodel.Status = 1;
                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                }
                //类型
                int ProductType = 0;
                var TypeName = dt.Rows[i][XYDicColsMapping["TypeName"]].ToString();
                List<SelectListItem> pTypeList = new List<SelectListItem>();
                Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
                foreach (SelectListItem item in pTypeList)
                {
                    if (item.Text == TypeName)
                    {
                        ProductType = int.Parse(item.Value);
                        break;
                    }
                }
                if (ProductType == 0)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行类型不存在", excelRow),
                        Status = false
                    };
                }
                //原产地
                int OriginSysNo;
                var OriginName = dt.Rows[i][XYDicColsMapping["OriginName"]].ToString();
                Origin oEnity = OriginBo.Instance.GetEntityByName(OriginName);
                //判断国家是否存在
                if (oEnity != null)
                {
                    OriginSysNo = oEnity.SysNo;
                }
                else
                {
                    Origin omodel = new Origin();
                    omodel.Origin_Name = OriginName;
                    omodel.Origin_Img = "";
                    omodel.CreatedDate = DateTime.Now;
                    omodel.CreatedBy = operatorSysno;
                    omodel.LastUpdateBy = operatorSysno;
                    omodel.LastUpdateDate = DateTime.Now;
                    OriginSysNo = IOriginDao.Instance.Insert(omodel);
                }
                //毛重 	
                var GrosWeight = dt.Rows[i][XYDicColsMapping["GrosWeight"]].ToString();
                Decimal typeGrosWeight = 0;
                if (!Decimal.TryParse(GrosWeight, out typeGrosWeight))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行毛重必须为数值", excelRow),
                        Status = false
                    };
                }
                //净重 	
                var NetWeight = dt.Rows[i][XYDicColsMapping["NetWeight"]].ToString();
                Decimal typeNetWeight = 0;
                if (!Decimal.TryParse(NetWeight, out typeNetWeight))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行净重必须为数值", excelRow),
                        Status = false
                    };
                }
                //税率 	
                var Tax = dt.Rows[i][XYDicColsMapping["Tax"]].ToString();
                Decimal typeTax = 0;
                if (!Decimal.TryParse(Tax, out typeTax))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行税率必须为数值", excelRow),
                        Status = false
                    };
                }
                //直营利润比例 	
                var PriceRate = dt.Rows[i][XYDicColsMapping["PriceRate"]].ToString();
                Decimal typePriceRate = 0;
                if (!Decimal.TryParse(PriceRate, out typePriceRate))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行直营利润比例必须为数值", excelRow),
                        Status = false
                    };
                }
                //直营分销商利润金额 	
                var PriceValue = dt.Rows[i][XYDicColsMapping["PriceValue"]].ToString();
                Decimal typePriceValue = 0;
                if (!Decimal.TryParse(PriceValue, out typePriceValue))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行直营分销商利润金额必须为数值", excelRow),
                        Status = false
                    };
                }
                //商品价格 	
                var Price = dt.Rows[i][XYDicColsMapping["Price"]].ToString();
                Decimal typePrice = 0;
                if (!Decimal.TryParse(Price, out typePrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品价格必须为数值", excelRow),
                        Status = false
                    };
                }


                PdPrice prmodel = new PdPrice();
                prmodel.ProductSysNo = 1;
                prmodel.Price = Decimal.Parse(Price);
                prmodel.PriceSource = 0;
                prmodel.SourceSysNo = 0;
                prmodel.Status = 1;


                //会员价格 	
                var SalePrice = dt.Rows[i][XYDicColsMapping["SalePrice"]].ToString();
                Decimal typeSalePrice = 0;
                if (!Decimal.TryParse(SalePrice, out typeSalePrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行会员价格必须为数值", excelRow),
                        Status = false
                    };
                }
                PdPrice sprmodel = new PdPrice();
                sprmodel.ProductSysNo = 1;
                sprmodel.Price = Decimal.Parse(SalePrice);
                sprmodel.PriceSource = 10;
                sprmodel.SourceSysNo = 1;
                sprmodel.Status = 1;

                //批发价
                var TradePrice = dt.Rows[i][XYDicColsMapping["TradePrice"]].ToString();
                Decimal typeTradePrice = 0;
                if (!Decimal.TryParse(TradePrice, out typeTradePrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行批发价格必须为数值", excelRow),
                        Status = false
                    };
                }

                //门店销售价 	
                var StoreSalePrice = dt.Rows[i][XYDicColsMapping["StoreSalePrice"]].ToString();
                Decimal typeStoreSalePrice = 0;
                if (!Decimal.TryParse(StoreSalePrice, out typeStoreSalePrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行门店销售价必须为数值", excelRow),
                        Status = false
                    };
                }
                var sspmodel = new PdPrice();
                sspmodel.ProductSysNo = 1;
                sspmodel.Price = Decimal.Parse(StoreSalePrice);
                sspmodel.PriceSource = 90;
                sspmodel.SourceSysNo = 0;
                sspmodel.Status = 1;



                #endregion

                #region 拼装导入数据
                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    ProductName = ProductName,
                    EasName = EasName,
                    BrandSysNo = BrandSysNo,
                    ProductType = ProductType,
                    OriginSysNo = OriginSysNo,
                    Barcode = Barcode,
                    GrosWeight = Decimal.Parse(GrosWeight),
                    NetWeight = Decimal.Parse(NetWeight),
                    Tax = Tax,
                    PriceRate = Decimal.Parse(PriceRate),
                    PriceValue = Decimal.Parse(PriceValue),
                    TradePrice = Decimal.Parse(TradePrice),
                    PdPrice = prmodel,
                    PdSalePrice = sprmodel,
                    PdStoreSalePrice = sspmodel,
                    PdCategorySql = PdCmodel,
                    PdCategoryAssociation = PdCAmodel,
                    AgentSysNo = 1,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);
                #endregion

                #region 分配到新增和修改的队列
                var productInfo = lstExisted.Where(x => x.ErpCode == ErpCode).FirstOrDefault();

                if (productInfo == null)
                    lstToInsert.Add(model);
                else
                    lstToUpdate.Add(model);
                #endregion

                var productList = lstExisted.Where(x => x.Barcode == Barcode).ToList();
                if (productList.Count > 1)
                {
                    return new Result
                    {
                        Message = string.Format("导入的数据条码【" + Barcode + "】在数据库有重复，请修改再导入！"),
                        Status = false
                    };
                }
            }

            try
            {
                IPdProductDao.Instance.CreateXinYingPdProduct(lstToInsert);
                IPdProductDao.Instance.UpdateXinYingExcelPdProduct(lstToUpdate);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                return new Result
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }


        private static readonly Dictionary<string, string> XYDicColsMappingOneTimeMethod = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"Barcode","条形码"}
            };
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportXinYingExcelOneTimeMethod(Stream stream, int operatorSysno)
        {
            DataTable dt = null;

            var cols = XYDicColsMappingOneTimeMethod.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            var excellst = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                //int excelRow = i + 2;
                //for (var j = 0; j < 16; j++)
                //{
                //    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                //    {
                //        return new Result
                //        {
                //            Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1)),
                //            Status = false
                //        };
                //    }
                //}
                //商品编号
                var ErpCode = dt.Rows[i][XYDicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var Barcode = dt.Rows[i][XYDicColsMapping["Barcode"]].ToString();

                //判断该商品编号的条形码是否有相同
                //2016-3-29 王耀发 创建
                //if (!string.IsNullOrEmpty(Barcode))
                //{
                //    PdProduct proEntity = IPdProductDao.Instance.GetEntityByBarcode(ErpCode, Barcode);
                //    if (proEntity != null)
                //    {
                //        existlst.Add(proEntity);
                //        continue;
                //        //return new Result
                //        //{
                //        //    Message = string.Format("excel表第{0}行数据的条形码已存在", excelRow),
                //        //    Status = false
                //        //};
                //    }
                //}


                var model = new PdProductList
                {
                    ErpCode = ErpCode,
                    Barcode = Barcode,
                    AgentSysNo = 1,//默认为总部代理商
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);

            }

            var strExist = "";
            //foreach (PdProduct pro in existlst)
            //{
            //    strExist += "\r\n" + pro.Barcode + "\t" + pro.ProductName;
            //}

            var lstExisted = DataAccess.Product.IPdProductDao.Instance.GetAllPdProduct();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.Barcode == excelModel.Barcode))
                {
                    lstToUpdate.Add(excelModel);
                }
            }
            try
            {
                IPdProductDao.Instance.UpdateXinYingExcelPdProduct(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            if (lstToUpdate.Count == 0)
            {
                return new Result
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            var msg = lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }


        #endregion

        /// <summary>
        /// 更新商品前台显示字段
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="IsFrontDisplay">前台显示字段</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2015-12-24 王耀发 创建</remarks>
        public bool UpdateProductIsFrontDisplay(int productSysNo, int IsFrontDisplay)
        {
            var result = IPdProductDao.Instance.UpdateProductIsFrontDisplay(productSysNo, IsFrontDisplay);
            return result;
        }

        #region 批量同步商品到B2B
        /// <summary>
        /// 批量同步商品到B2B
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public void XinYingProductsToB2B(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBXinyingSynPdProductsB2B> exportProducts = BLL.Product.PdProductBo.Instance.XinYingSynProductList(productSysNos, productDetail);
                foreach (CBXinyingSynPdProductsB2B item in exportProducts)
                {
                    // var product = IPdProductDao.Instance.GetProduct(item.ProductSysNo);
                    // var product = IPdProductDao.Instance.GetProduct(item.SysNo);
                    var productB2B = IPdProductDao.Instance.GetB2BProductByErpCode(item.ErpCode);
                    if (productB2B == null)
                    {
                        //continue;
                        var product = IPdProductDao.Instance.GetProduct(item.SysNo);
                        var brandB2C = BLL.Product.PdBrandBo.Instance.GetEntity(product.BrandSysNo);
                        var brandB2B = BLL.Product.PdBrandBo.Instance.GetB2BEntityByName(brandB2C != null ? brandB2C.Name : "");
                        if (brandB2B == null)
                        {
                            brandB2C.SysNo = IPdBrandDao.Instance.CreateToB2B(brandB2C);
                        }
                        else
                        {
                            brandB2C.SysNo = brandB2B.SysNo;
                        }
                        product.BrandSysNo = brandB2C.SysNo;
                      var isok=  IPdProductDao.Instance.CreateToB2B(product);
                      productB2B = IPdProductDao.Instance.GetB2BProductByErpCode(item.ErpCode);
                    }

                    //else
                    //{
                        var result = IPdProductDao.Instance.UpdateB2BProductDescription(productB2B.SysNo, item.ProductDesc, item.ProductPhoneDesc);
                        var list2b = IPdProductImageDao.Instance.GetB2BProductImg(productB2B.SysNo);
                        if (list2b.Count >= 5)
                        {
                            continue;
                        }
                        var list = IPdProductImageDao.Instance.GetProductImg(item.SysNo);
                        item.PdProductImage = list;

                        // var resultImage = BLL.Product.PdProductImageBo.Instance.AddProductImageToB2B(list);
                        WebResponse response = null;
                        Stream stream = null;
                        Stream streamBig = null;
                        Stream streamSmall = null;
                        string imgUrl = "";
                        string imgUrlBig = "";
                        string imgUrlSmall = "";
                        try
                        {
                            foreach (PdProductImage itemimg in list)
                            {
                                //读取原图片文件流
                                imgUrl = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(itemimg.ImageUrl, Hyt.BLL.Web.ProductThumbnailType.Base);

                                stream = GetStream(stream, imgUrl);

                                //读取大图片文件流
                                imgUrlBig = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(itemimg.ImageUrl, Hyt.BLL.Web.ProductThumbnailType.Big);

                                streamBig = GetStream(streamBig, imgUrlBig);

                                //读取xiao图片文件流
                                imgUrlSmall = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(itemimg.ImageUrl, Hyt.BLL.Web.ProductThumbnailType.Small);

                                streamSmall = GetStream(streamSmall, imgUrlSmall);
                              
                                var uploadConfig = Hyt.BLL.Config.Config.Instance.GetUpLoadFileConfig();
                                Model.Common.FileConfigOption useUploadConfigOption = null;
                                if (useUploadConfigOption == null) useUploadConfigOption = uploadConfig.DefaultConfig;

                                //文件名
                                string fileName = NewFileName(".jpg");

                                var fileSamillName = fileName;// + ".small.jpg";

                                var productImageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig();

                                string yearMonth = DateTime.Now.ToString("yyyyMM");

                                #region 路径
                                string basePath = string.Format(
                                        productImageConfig.ProductImagePathFormat,
                                        "",
                                        productImageConfig.BaseFolder,
                                        ""
                                        );
                                string imageBigPath = string.Format(
                                        productImageConfig.ProductImagePathFormat,
                                        "",
                                        productImageConfig.BigFloder,
                                        ""
                                        );
                                string imageSmallPath = string.Format(
                                        productImageConfig.ProductImagePathFormat,
                                        "",
                                        productImageConfig.SmallFloder,
                                        ""
                                        );
                                #endregion

                                #region 缩略图
                                //转换原图
                               var baseImage = Hyt.Util.ImageUtil.ConvertToJpgB2B(stream);
                                //生成Big
                               var imageBig = Hyt.Util.ImageUtil.CreateThumbnailB2B(streamBig,
                                    productImageConfig.BigWidth,
                                    productImageConfig.BigHeight,
                                    Hyt.Util.ImageUtil.ThumbnailMode.Cut);
                                //生成small
                               var imageSmall = Hyt.Util.ImageUtil.CreateThumbnailB2B(streamSmall,
                                    productImageConfig.SmallWidth,
                                    productImageConfig.SmallHeight,
                                    Hyt.Util.ImageUtil.ThumbnailMode.Cut);
                                #endregion

                                var status = true;
                                var B2Bstatus = true;
                                try
                                {

                                    ////image.Dispose();
                                    ////上传图片到B2B的服务器  210101010
                                    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<B2B.Service.Img.FileProcessor.IUploadServiceForB2B>())
                                    {
                                        B2Bstatus = service.Channel.UploadFile(basePath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(baseImage));
                                        B2Bstatus = service.Channel.UploadFile(imageBigPath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageBig));
                                        B2Bstatus = service.Channel.UploadFile(imageSmallPath, fileSamillName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageSmall));
                                    }
                                    baseImage.Dispose();
                                    imageBig.Dispose();
                                    imageSmall.Dispose();
                                    stream.Dispose();
                                    streamBig.Dispose();
                                    streamSmall.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    Hyt.BLL.Log.LocalLogBo.Instance.Write(ex);
                                }
                                //if (!B2Bstatus)
                                //    throw new Exception("文件上传失败!");

                                string fileurl = string.Format(productImageConfig.ProductImagePathFormat, "{0}", "{1}", fileName);//attachmentConfig.FileServer

                                if (B2Bstatus)
                                {
                                    var productImage = new PdProductImage();
                                    productImage.ProductSysNo = productB2B.SysNo;
                                    productImage.ImageUrl = fileurl;
                                    productImage.Status = (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.隐藏;
                                    var resultdata = IPdProductImageDao.Instance.InsertB2B(productImage);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ;
                        }

                   // }


                }

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品批量同步",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品批量同步",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        private static Stream GetStream(Stream stream, string imgUrl)
        {
            Uri mUri = new Uri(imgUrl);
            HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(mUri);
            mRequest.Method = "GET";
            mRequest.Timeout = 200;
            mRequest.ContentType = "text/html;charset=utf-8";
            HttpWebResponse mResponse = (HttpWebResponse)mRequest.GetResponse();
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            // response = mRequest.GetResponse();
            stream = mResponse.GetResponseStream();
            return stream;
        }

        /// <summary>
        /// 生成新文件名称
        /// </summary>
        /// <param name="fileExtension">文件类型（带点）</param>
        /// <returns>新的文件名称</returns>
        /// <returns>2013-6-13 黄波 创建</returns>
        private string NewFileName(string fileExtension)
        {
            return Guid.NewGuid().ToString("N")
                // + DateTime.Now.ToString("hhsMmmyysyyMssdsd") 图片路径太长，去掉
                + fileExtension;
        }
        #endregion


        #region 商品同步
        /// <summary>
        /// 商品同步
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns>2017 010 10 罗勤瑶</returns>
        public List<CBXinyingSynPdProductsB2B> XinYingSynProductList(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetXinYingSynProductList(sysNos, productDetail);
        }
        #endregion

        #region 商品导出

        /// <summary>
        /// 商品导出
        /// </summary>
        /// <param name="dt">需要导出的数据</param>
        /// <remarks>2016-11-28 杨浩 创建</remarks>
        public void ExportProducts<T>(System.Data.DataTable dt) where T : ITemplateBase
        {
            try
            {
                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<T>(dt, fileName);

            }
            catch (Exception ex)
            {
                var current = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                    LogStatus.系统日志目标类型.ExcelExporting, 0, ex, Hyt.Util.WebUtil.GetUserIp(), (current != null ? current.Base.SysNo : 0));

            }
        }



        /// <summary>
        /// 商品导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportProducts(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProducts> exportProducts = BLL.Product.PdProductBo.Instance.GetExportProductList(productSysNos, productDetail);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 商品编码
                 * 前台显示名称
                 * 后台显示名称
                 * 分类
                 * 品牌
                 * 类型
                 * 原产地
                 * 条形码
                 * 毛重
                 * 税率
                 * 直营利润比例
                 * 直营分销商利润金额
                 * 商品价格
                 * 会员价
                 * 批发价
                 * 门店销售价
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProducts>(exportProducts,
                    new List<string> { "自动编码", "商品编码", "前台显示名称", "后台显示名称", "分类", "品牌", "类型", "原产地", "条形码", "毛重", "税率", "直营利润比例", "直营分销商利润金额", "商品价格", "会员价", "批发价", "门店销售价", "VIP会员价", "钻石会员价", "销售合伙人", "销量" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 商品导出利嘉模板
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-5-16 罗勤尧 创建</remarks>
        public void ExportProductsLiJia(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductsLijia> exportProducts = BLL.Product.PdProductBo.Instance.GetExportProductListLiJia(productSysNos, productDetail);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));



                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductsLijia>(exportProducts,
                    new List<string> { "商品编码", "名称", "商品分类", "品牌", "商品简称", "规格颜色", "长", "宽", "高", "直径", "重量", "供应商", "供应商地址", "供应商联系方式", "采购地", "批采价格条目", "采购周期", "是否快速单", "包装内含", "描述", "英文名称", "英文描述", "备注" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 商品导出（信营）
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void XinYingExportProducts(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBXinyingOutputPdProducts> exportProducts = BLL.Product.PdProductBo.Instance.GetXinYingExportProductList(productSysNos, productDetail);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 商品编码
                 * 前台显示名称
                 * 后台显示名称
                 * 分类
                 * 品牌
                 * 类型
                 * 原产地
                 * 条形码
                 * 毛重
                 * 净重
                 * 税率
                 * 直营利润比例
                 * 直营分销商利润金额
                 * 商品价格
                 * 会员价
                 * 批发价
                 * 门店销售价
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBXinyingOutputPdProducts>(exportProducts,
                    new List<string> { "自动编码", "商品编码", "前台显示名称", "后台显示名称", "分类", "品牌", "类型", "原产地", "条形码", "毛重", "净重", "税率", "直营利润比例", "直营分销商利润金额", "商品价格", "会员价", "批发价", "门店销售价", "利润率" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }



        /// <summary>
        /// 商品导出(无净重，商品简介)
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportProductsExcel(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductsExcel> exportProducts = BLL.Product.PdProductBo.Instance.GetExportProductListExcel(productSysNos, productDetail);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 商品编码
                 * 前台显示名称
                 * 后台显示名称
                 * 分类
                 * 品牌
                 * 类型
                 * 原产地
                 * 条形码
                 * 毛重
                 * 税率
                 * 直营利润比例
                 * 直营分销商利润金额
                 * 商品价格
                 * 会员价
                 * 批发价
                 * 门店销售价
                 * 商品简称
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductsExcel>(exportProducts,
                    new List<string> { "自动编码", "商品编码", "前台显示名称", "后台显示名称", "分类", "品牌", "类型", "原产地", "条形码", "毛重", "税率", "直营利润比例", "直营分销商利润金额", "商品价格", "会员价", "批发价", "门店销售价", "商品简称" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 商品导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportProductsByYD(List<int> productSysNos, string userIp, int operatorSysno, ParaProductFilter productDetail = null)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductsByYD> exportProducts = BLL.Product.PdProductBo.Instance.GetExportProductListByYD(productSysNos, productDetail);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 商品编码
                 * 前台显示名称
                 * 后台显示名称
                 * 分类
                 * 品牌
                 * 类型
                 * 原产地
                 * 条形码
                 * 毛重
                 * 税率
                 * 直营利润比例
                 * 直营分销商利润金额
                 * 商品价格
                 * 会员价
                 * 批发价
                 * 门店销售价
                 * 成本价
                 * 上下架
                 * 是否允许前台下单
                 * 是否前台显示
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductsByYD>(exportProducts,
                    new List<string> { "自动编码", "商品编码", "前台显示名称", "后台显示名称", "分类", "品牌", "类型", "原产地", "条形码", "毛重", "税率", "直营利润比例", "直营分销商利润金额", "商品价格", "会员价", "批发价", "门店销售价", "成本价", "上下架(1上0下)", "是否允许前台下单(1是0否)", "是否前台显示(1是0否)" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputPdProducts> GetExportProductList(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetExportProductList(sysNos, productDetail);
        }

        /// <summary>
        /// 查询导出商品列表利嘉模板
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputPdProductsLijia> GetExportProductListLiJia(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetXinYingExportProductListLiJia(sysNos, productDetail);
        }
        /// <summary>
        /// 获取导出商品信息
        /// </summary>
        /// <param name="sysNos">商品系统编号集合</param>
        /// <param name="productDetail">查询条件</param>
        /// <returns></returns>
        /// <remarks>2016-11-28 杨浩 创建</remarks>
        public System.Data.DataTable GetExportProductToDataTable(List<int> sysNos, ParaProductFilter productDetail)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetExportProductToDataTable(sysNos, productDetail);
        }
        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputPdProductsByYD> GetExportProductListByYD(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetExportProductListByYD(sysNos, productDetail);
        }
        /// <summary>
        /// 查询导出商品列表(信营)
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        public List<CBXinyingOutputPdProducts> GetXinYingExportProductList(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetXinYingExportProductList(sysNos, productDetail);
        }
        /// <summary>
        /// 查询导出商品列表（无净重）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <param name="productDetail"></param>
        /// <returns></returns>
        public List<CBOutputPdProductsExcel> GetExportProductListExcel(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            return DataAccess.Product.IPdProductDao.Instance.GetExportProductListExcel(sysNos, productDetail);
        }
        #endregion

        /// <summary>
        /// 获取南沙商品备案信息
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回备案信息</returns>
        /// <remarks>2016-4-4 王耀发  实现功能</remarks>
        public IList<IcpGZNanShaGoodsInfo> GetIcpGZNanShaGoodsInfoList(IList<int> productList)
        {
            return IPdProductDao.Instance.GetIcpGZNanShaGoodsInfoList(productList);
        }
        /// <summary>
        /// 通过 商品ip集合获取商品数据详情
        /// </summary>
        /// <param name="proIdList"></param>
        /// <returns></returns>
        public IList<CBPdProduct> GetProductInfoList(IList<int> proIdList)
        {
            return IPdProductDao.Instance.GetProductInfoList(proIdList);
        }
        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2016-04-25 王耀发 创建</remarks>
        public int CreateProduct(PdProduct model)
        {
            return IPdProductDao.Instance.CreateProduct(model);
        }

        /// <summary>
        /// 获取商品所属分类
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        public IList<int> GetProductsCategories(IList<int> productSysNoList)
        {
            return IPdProductDao.Instance.GetProductsCategories(productSysNoList);
        }

        /// <summary>
        /// 批量修改商品分类
        /// </summary>
        /// <param name="productSysNos"></param>
        /// <param name="selectedCategory"></param>
        /// <param name="deleteCategory"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        public Result BatchUpdateProductsCategories(List<int> productSysNos, List<int> selectedCategory, List<int> deleteCategory)
        {
            Result result = new Result();
            result.Status = true;
            result.Message = "更新成功";
            try
            {
                productSysNos = productSysNos.Where(p => BLL.Product.PdProductBo.Instance.GetProductBySysNo(p).Status == 0).ToList();
                // 删除分类
                if (deleteCategory.Count > 0)
                {
                    foreach (int sysNo in productSysNos)
                    {
                        DataAccess.Product.IPdCategoryAssociationDao.Instance.Delete(sysNo, deleteCategory, true);
                    }
                }

                // 添加分类
                List<PdCategoryAssociation> categoryAssociationList = new List<PdCategoryAssociation>();
                foreach (int sysNo in productSysNos)
                {
                    foreach (var cateSysNo in selectedCategory)
                    {
                        categoryAssociationList.Add(new PdCategoryAssociation { ProductSysNo = sysNo, CategorySysNo = cateSysNo, IsMaster = 0 });
                    }
                }
                foreach (var pdCategoryAssociation in categoryAssociationList)
                {
                    PdCategoryAssociationBo.Instance.Create(pdCategoryAssociation);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.StackTrace.ToString();
                result.Status = false;
                return result;
            }

            if (result.Status == true)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("批量更新商品{0}分类", productSysNos.Join(",")), LogStatus.系统日志目标类型.商品分类, 0, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return result;
        }

        public IList<Model.Procurement.CBPmProcurementOrderItem> GetPdProductByProcurementList(string proIdList, int cgSysNo)
        {
            return IPmProcurementDao.Instance.GetCBProcurementOrderItemListByProList(proIdList, cgSysNo);
        }
        /// <summary>
        /// 判断是否存在商品图片
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <remarks>2016-06-8 王耀发 创建</remarks>
        public bool IsExistProductImag(string uri)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "HEAD";
                req.Timeout = 100;
                res = (HttpWebResponse)req.GetResponse();
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }
        }
        /// <summary>
        /// 通过条码获取商品档案
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>商品实体</returns>
        public PdProduct GetProductByBarcode(string barcode)
        {
            return IPdProductDao.Instance.GetProductByBarcode(barcode);
        }

        /// <summary>
        /// 更新商品档案时间和日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="p"></param>
        public void UpdateLastTimeOrUser(int productSysNo, DateTime dateTime, int UserSysNo)
        {
            IPdProductDao.Instance.UpdateLastTimeOrUser(productSysNo, dateTime, UserSysNo);
        }
        /// <summary>
        /// 分页查询商品条码列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-08-31 周 创建</remarks>
        public Pager<PdProductBarcode> GetPdProductBarcodeList(Pager<PdProductBarcode> pager)
        {
            return IPdProductDao.Instance.GetPdProductBarcodeList(pager);
        }
        /// <summary>
        /// 保存条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result SaveProductBarcode(PdProductBarcode model)
        {
            Result result = new Result();
            if (model == null || string.IsNullOrEmpty(model.Barcode))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IPdProductDao.Instance.IsExistsProductBarcode(model);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "该条码已存在，请修改";
                    return result;
                }

                //数据操作
                if (model.SysNo > 0)
                {
                    var models = new PdProductBarcode();
                    models.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    models.LastUpdateDate = DateTime.Now;
                    models.ProductQuantity = model.ProductQuantity;
                    models.Barcode = model.Barcode;
                    models.SysNo = model.SysNo;
                    models.Prefix = Hyt.Model.SystemPredefined.Constant.CUSTOMIZE_BARCODE;
                    models.CreatedDate = model.CreatedDate;
                    models.CreatedBy = model.CreatedBy;
                    models.ProductSysNo = model.ProductSysNo;
                    result.Status = IPdProductDao.Instance.UpdateProductBarcode(models) == true;
                }
                else
                {
                    var models = new PdProductBarcode();
                    models.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    models.CreatedDate = DateTime.Now;
                    models.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    models.LastUpdateDate = DateTime.Now;
                    models.Barcode = model.Barcode;
                    models.ProductSysNo = model.ProductSysNo;
                    models.ProductQuantity = model.ProductQuantity;
                    models.Prefix = Hyt.Model.SystemPredefined.Constant.CUSTOMIZE_BARCODE;
                    models.SysNo = IPdProductDao.Instance.CreateProductBarcode(models);
                    result.Status = models.SysNo > 0 ? true : false;
                    if (models.SysNo > 0)
                    {
                        result.StatusCode = 1;
                    }
                }
                if (result.Status)
                {
                    result.StatusCode = 1;
                    result.Message = "保存成功";
                }
                else
                {
                    result.StatusCode = -3;
                    result.Message = "保存失败";
                }
            }
            return result;

        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public PdProductBarcode GetProductBarcodeEntity(int sysNo)
        {
            return IPdProductDao.Instance.GetProductBarcodeEntity(sysNo);
        }
        /// <summary>
        /// 条码在商品列表中是否存在
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public int IsExistsPdProductBarcode(string Barcode)
        {
            return IPdProductDao.Instance.IsExistsPdProductBarcode(Barcode);
        }
        /// <summary>
        /// 分页查询条形码列表，商品已存在条码不显示
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Pager<PdProductBarcode> BarcodeQuery(string keyword, int currentPage, int pageSize)
        {
            return IPdProductDao.Instance.BarcodeQuery(keyword, currentPage, pageSize);
        }
        /// <summary>
        /// 通过条码获取条形码详情
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>商品实体</returns>
        public PdProductBarcode GetProductBarcodeByBarcode(string barcode)
        {
            return IPdProductDao.Instance.GetProductBarcodeByBarcode(barcode);
        }

        /// <summary>
        /// 更新条形码商品库存数量
        /// </summary>
        /// <param name="ProductSysno">商品编号</param>
        /// <param name="WarehouseSysno">仓库编号</param>
        /// <param name="ProductQuantity">商品数量</param>
        /// 未理顺问题：
        /// 第一：虚拟商品和实际商品分别入了两个仓库时出现无法判断和无法减库存
        /// 第二：如果客户下单了后，条形码数量被修改，出库的商品数量将不一致
        /// 第三：下单后，如果虚拟条形码被修改后无法找到正确商品
        public void ProductWarehouseQuantity(ref int ProductSysno, ref int WarehouseSysno, ref int ProductQuantity)
        {
            try
            {
                PdProduct ProductInfo = BLL.Product.PdProductBo.Instance.GetProductBySysNo(ProductSysno);
                if (ProductInfo != null && ProductInfo.Barcode != null)
                {
                    var barcode = ProductInfo.Barcode;
                    string[] strArray = barcode.Split('-');
                    var barcodeSplit = strArray[0] + "-";
                    if (barcodeSplit.Trim() == Model.SystemPredefined.Constant.CUSTOMIZE_BARCODE)//判断条形码是否有前缀
                    {
                        barcode = strArray[1];
                        var ProductBarcode = BLL.Product.PdProductBo.Instance.GetProductBarcodeByBarcode(barcode);
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(WarehouseSysno, ProductSysno, ProductQuantity);//减虚拟商品库存（不真实出库）
                        ProductQuantity = ProductBarcode.ProductQuantity * ProductQuantity;//条形码绑定商品减少的数量
                        ProductSysno = ProductBarcode.ProductSysNo;
                    }
                }
            }
            catch
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单虚拟商品库存出库异常！",
                                              LogStatus.系统日志目标类型.出库单,
                                              ProductSysno, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
        }
        /// <summary>
        /// 查询条形码实际商品及数量
        /// </summary>
        /// <param name="RefProductSysno">商品编号</param>
        /// <param name="RefProductQuantity">商品数量</param>
        /// <param name="Order">订单编号</param>
        public void RefProductQuantity(ref int RefProductSysno, ref int RefProductQuantity, ref string RefProductName, int Order)
        {
            try
            {
                PdProduct ProductInfo = BLL.Product.PdProductBo.Instance.GetProductBySysNo(RefProductSysno);
                if (ProductInfo != null && ProductInfo.Barcode != null)
                {
                    var barcode = ProductInfo.Barcode;
                    string[] strArray = barcode.Split('-');
                    var barcodeSplit = strArray[0] + "-";
                    if (barcodeSplit.Trim() == Model.SystemPredefined.Constant.CUSTOMIZE_BARCODE)//判断条形码是否有前缀
                    {
                        barcode = strArray[1];
                        var ProductBarcode = BLL.Product.PdProductBo.Instance.GetProductBarcodeByBarcode(barcode);
                        RefProductQuantity = ProductBarcode.ProductQuantity * RefProductQuantity;//条形码绑定商品减少的数量
                        RefProductSysno = ProductBarcode.ProductSysNo;//条形码绑定是商品编号
                        PdProduct RefProductInfo = BLL.Product.PdProductBo.Instance.GetProductBySysNo(ProductBarcode.ProductSysNo);
                        if (RefProductInfo != null)
                        {
                            RefProductName = RefProductInfo.ProductName;//条形码绑定是商品名称
                        }

                    }
                }
            }
            catch
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单虚拟商品库存出库异常！订单编号：" + Order,
                                              LogStatus.系统日志目标类型.出库单,
                                              Order, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
        }
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Pager<PdProduct> ProductListQuery(string keyword, int currentPage, int pageSize)
        {
            return IPdProductDao.Instance.ProductListQuery(keyword, currentPage, pageSize);
        }

        #region 供应链商品数据
        /// <summary>
        /// 添加供应链商品数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreatePdProductForSupplyChain(PdProductForSupplyChain model)
        {
            return IPdProductDao.Instance.CreatePdProductForSupplyChain(model);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public PdProductForSupplyChain GetPdProductForSupplyChainEntity(int SupplyChainCode)
        {
            return IPdProductDao.Instance.GetPdProductForSupplyChainEntity(SupplyChainCode);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdatePdProductForSupplyChain(PdProductForSupplyChain model)
        {
            return IPdProductDao.Instance.UpdatePdProductForSupplyChain(model);
        }
        #endregion

        /// <summary>
        /// 批量更新产品状态
        /// </summary>
        /// <param name="productSysNoList">产品系统编号列表</param>
        /// <param name="status">产品状态</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-10-09 杨浩 创建</remarks>
        public int BatchUpdateProductStatus(string productSysNoList, int status, int dealerSysNo)
        {
            return IPdProductDao.Instance.BatchUpdateProductStatus(productSysNoList, status, dealerSysNo);
        }

        internal List<PdProduct> GetAllProductDataBase()
        {
            return IPdProductDao.Instance.GetAllProductDataBase();
        }

        /// <summary>
        /// 根据产品系统编号列列表获取产品
        /// </summary>
        /// <param name="productSysnoList">产品系统编号列表</param>
        /// <returns></returns>
        /// <remarks>2017-06-30 杨浩 创建</remarks>
        public IList<PdProduct> GetProductListBySysnoList(List<int> productSysnoList)
        {
            return IPdProductDao.Instance.GetProductListBySysnoList(productSysnoList);
        }

        /// <summary>
        /// 根据ErpCode获取产品列表
        /// </summary>
        /// <param name="productErpCodes">产品编码集合</param>
        /// <returns></returns>
        /// <remarks>2017-10-18 杨浩 创建</remarks>
        public IList<PdProduct> GetProductListByErpCode(IList<string> productErpCodes)
        {
            return IPdProductDao.Instance.GetProductListByErpCode(productErpCodes);
        }

        /// <summary>
        /// 根据商品与仓库获取待配送的商品数量
        /// </summary>
        /// <returns></returns>
        public  int GetPdPending(int pdSysNo, int whSysNo)
        {
            return IPdProductDao.Instance.GetPdPending(pdSysNo, whSysNo);
        }
    }
}
