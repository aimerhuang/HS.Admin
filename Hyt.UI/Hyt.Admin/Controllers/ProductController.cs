using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Infrastructure.Pager;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Newtonsoft.Json;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;
using System.Text;
using System.Web.Script.Serialization;
using Hyt.BLL.Authentication;
using Hyt.Model.Parameter;
using System.Web;
using Hyt.Util.Extension;
using Hyt.BLL.Distribution;
using Hyt.BLL.ApiIcq;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Logistics;
using Hyt.Model.PdPackaged;
using Hyt.BLL.PdPackaged;
using Hyt.Model.InventorySheet;
using Hyt.BLL.InventorySheet;
using Newtonsoft.Json.Linq;
using Hyt.DataAccess.Product;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 商品管理
    /// </summary>
    /// <remarks>2013-06-18 邵斌 创建</remarks>
    public class ProductController : BaseController
    {
        /// <summary>
        /// 获取商品类别树
        /// </summary>
        /// <returns>返回视图</returns>
        /// <remarks>2013-06-18 黄志勇 创建</remarks>
        public ActionResult CategoryTree()
        {
            return View();
        }

        #region 商品分类管理功能

        /// <summary>
        /// 商品分类列表选择器
        /// </summary>
        /// <param name="isMultipleSelect">分类选择是否是多选</param>
        /// <param name="width">选择器宽度</param>
        /// <param name="height">选择器高度</param>
        /// <param name="onlyLeaftSelect">只能选择叶子节点</param>
        /// <returns>返回视图</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        /// <remarks>2013-07-05 邵斌 重构 增加是否指先只节点</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult ProductCategorySelector(bool? isMultipleSelect, int? width, int? height,
                                                    bool onlyLeaftSelect = true)
        {
            ViewBag.IsMultipleSelect = isMultipleSelect.HasValue ? isMultipleSelect.Value : false;
            ViewBag.Width = width.HasValue ? width.Value : 290;
            ViewBag.Height = height.HasValue ? height.Value : 290;
            ViewBag.OnlyLeaftSelect = onlyLeaftSelect;

            return View();
        }

        /// <summary>
        /// 商品分类列表选择器
        /// </summary>
        /// <param name="isMultipleSelect">分类选择是否是多选</param>
        /// <param name="width">选择器宽度</param>
        /// <param name="height">选择器高度</param>
        /// <param name="onlyLeaftSelect">只能选择叶子节点</param>
        /// <returns>返回视图</returns>
        /// <remarks>2016-05-05 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult BatchCategorySelector(bool? isMultipleSelect, int? width, int? height, bool onlyLeaftSelect = true)
        {
            ViewBag.IsMultipleSelect = isMultipleSelect.HasValue ? isMultipleSelect.Value : false;
            ViewBag.Width = width.HasValue ? width.Value : 290;
            ViewBag.Height = height.HasValue ? height.Value : 290;
            ViewBag.OnlyLeaftSelect = onlyLeaftSelect;

            return View();
        }

        /// <summary>
        /// 商品分类列表管理
        /// </summary>
        /// <returns>返回视图</returns>
        /// <remarks>2013-07-03 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007001, PrivilegeCode.PD1007201, PrivilegeCode.PD1007601)]
        public ActionResult ProductCategory()
        {
            return View();
        }

        /// <summary>
        /// 编辑商品分类
        /// </summary>
        /// <param name="model">商品分类信息</param>
        /// <param name="attributeGroups">商品分类对应属性组</param>
        /// <returns>编辑是否成功</returns>
        /// <remarks>2013-07-03 邵斌 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1007201)]
        public JsonResult EditProductCategory(PdCategory model, IList<PdAttributeGroup> attributeGroups)
        {

            PdCategory category = model;

            bool success = false;
            string msg = null;

            if (category != null)
            {
                category.LastUpdateBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                category.LastUpdateDate = DateTime.Now;

                success = PdCategoryBo.Instance.EditCategory(category, attributeGroups);

            }
            else
            {
                msg = "请正确设置商品分类信息";
            }

            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <param name="attributeGroups">属性列表</param>
        /// <returns>返回创建是否成功状态对象</returns>
        /// <remarks>2013-07-29 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007201)]
        public JsonResult CreateProductCategory(PdCategory category, IList<PdAttributeGroup> attributeGroups)
        {
            bool success = false;
            string msg = null;

            if (category != null)
            {
                category.LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
                category.LastUpdateDate = DateTime.Now;

                success = PdCategoryBo.Instance.CreateProductCategory(category, attributeGroups);

            }
            else
            {
                msg = "请正确设置商品分类信息";
            }
            return Json(new { success = success, msg = msg, newid = category.SysNo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 改变商品分类状态
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="enabled">是否启用 true：启用 false:停用</param>
        /// <returns>返回: true:成功  false:失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007601)]
        public JsonResult ChangeProductCategoryStatus(int sysNo, bool enabled)
        {
            bool success = false;

            //判断商品分类系统编号是否有效
            if (sysNo > 0)
            {
                ProductStatus.商品分类状态 status = ProductStatus.商品分类状态.有效;
                if (!enabled)
                    status = ProductStatus.商品分类状态.无效;

                success = PdCategoryBo.Instance.ChangeStatus(sysNo, status,
                                                             AdminAuthenticationBo.Instance.GetAuthenticatedUser()
                                                                                  .SysNo);
            }

            return Json(success, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 设置商品分类是否显示
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isOnline">是否前台展示 true:展示 false:隐藏</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Obsolete]
        [Privilege(PrivilegeCode.PD1007201)]
        public JsonResult SetProdcutCateogryIsOnline(int sysNo, ProductStatus.是否前端展示 isOnline)
        {

            bool success = false;

            //判断商品分类系统编号是否有效
            if (sysNo > 0)
            {
                success = PdCategoryBo.Instance.SetIsOnline(sysNo, isOnline,
                                                            AdminAuthenticationBo.Instance.GetAuthenticatedUser()
                                                                                 .SysNo);
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 交换显示分类的显示排序
        /// </summary>
        /// <param name="originalSysNo">交换源对象系统编号</param>
        /// <param name="objectiveSysNo">要进行位置交换的目标对象系统编号</param>
        /// <returns>返回： true 操作成功  false 操作失败</returns>
        /// <remarks>注意：该方法值适用于在同一父级中进行移动变更</remarks>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007201)]
        public JsonResult CategorySwapDisplayOrder(int originalSysNo, int objectiveSysNo)
        {
            string msg = "";
            bool success = false;

            if (originalSysNo <= 0 || objectiveSysNo <= 0)
            {
                msg = "请指定要进行位置交换的目标分类和原分类";
            }
            else
            {
                success = PdCategoryBo.Instance.SwapDisplayOrder(originalSysNo, objectiveSysNo);
            }

            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改商品分类的父节点
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="parentSysNo">父节点系统编号</param>
        /// <returns>修改父级分类是否成功状态</returns>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1007201)]
        public JsonResult CategoryChangeParentSysNo(int sysNo, int parentSysNo)
        {
            string msg = "";
            bool success = (sysNo != 0); //商品分类系统编号是否为0

            //商品为0则操作失败
            if (!success)
            {
                msg = "请指定商品分类";
            }
            else
            {

            }
            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 商品调价管理功能

        /// <summary>
        /// 商品调价管理首页
        /// </summary>
        /// <param name="id">起始页码</param>
        /// <param name="status">审批状态</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="productName">商品名称</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        /// <remarks>2013-07-17 杨晗 修改</remarks>
        [Privilege(PrivilegeCode.PD1003001)]
        public ActionResult ProductPriceHistory(int? id, int? status, int? erpCode, string productName = null)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ProductStatus.产品价格变更状态));
            ViewBag.DictList = dictList;
            status = status ?? 10;
            var model = PdPriceHistoryBo.Instance.GetPriceHistorieList(id, (int)status, erpCode, productName);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxProductPriceHistory", model);
            }
            return View(model);
        }

        /// <summary>
        /// 审核商品调价视图
        /// </summary>
        /// <param name="relationCode">调价关系码</param>
        /// <returns>审核失败或成功信息视图</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.PD1003601)]
        public ActionResult ProductPriceHistoryAudit(string relationCode)
        {
            var model = PdPriceHistoryBo.Instance.GetPriceHistorieListByRelationCode(relationCode);
            return View(model);
        }

        /// <summary>
        /// 审核商品调价
        /// </summary>
        /// <param name="relationCode">调价关系码</param>
        /// <param name="status">状态</param>
        /// <param name="opinion">意见</param>
        /// <returns>审核失败或成功信息</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1003601)]
        public ActionResult ProductPriceHistoryAudit(string relationCode, int status, string opinion)
        {
            bool isPass = true;
            var model = PdPriceHistoryBo.Instance.GetPriceHistorieListByRelationCode(relationCode);
            //查询对应分销商商品状态是否下架下架
            int DealerSysNo = CurrentUser.Dealer.SysNo;
            var data = DsSpecialPriceBo.Instance.GetEntityByDPSysNo(DealerSysNo, model[0].ProductSysNo);
            if (data == null)
            {
                int u = PdPriceHistoryBo.Instance.Update(relationCode, opinion, status,
                                                          CurrentUser.Base.SysNo);
                if (status == (int)ProductStatus.产品价格变更状态.已审)
                {
                    isPass = PdPriceBo.Instance.UpdateApplyPriceToPdPrice(model);
                    //立即更新索引和缓存并在前台展示
                    if (isPass)
                    {
                        int productSysNo = model[0].ProductSysNo;
                        //缓存清理
                        Hyt.BLL.Cache.DeleteCache.ProductInfo(productSysNo);
                        //更新索引
                        BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(productSysNo);
                    }
                }
                isPass = u > 0 && isPass;
            }
            else if (data.Status == 1)
            {
                //更改调价后，对应分销商商品状态为下架
                var result = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.下架, model[0].ProductSysNo);
                if (result.Status)
                {
                    int u = PdPriceHistoryBo.Instance.Update(relationCode, opinion, status,
                                                             CurrentUser.Base.SysNo);
                    if (status == (int)ProductStatus.产品价格变更状态.已审)
                    {
                        isPass = PdPriceBo.Instance.UpdateApplyPriceToPdPrice(model);
                        //立即更新索引和缓存并在前台展示
                        if (isPass)
                        {
                            int productSysNo = model[0].ProductSysNo;
                            //缓存清理
                            Hyt.BLL.Cache.DeleteCache.ProductInfo(productSysNo);
                            //更新索引
                            BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(productSysNo);
                        }
                    }
                    isPass = u > 0 && isPass;
                }
                else
                {
                    isPass = false;
                }
            }
            else
            {
                int u = PdPriceHistoryBo.Instance.Update(relationCode, opinion, status,
                                                           CurrentUser.Base.SysNo);
                if (status == (int)ProductStatus.产品价格变更状态.已审)
                {
                    isPass = PdPriceBo.Instance.UpdateApplyPriceToPdPrice(model);
                    //立即更新索引和缓存并在前台展示
                    if (isPass)
                    {
                        int productSysNo = model[0].ProductSysNo;
                        //缓存清理
                        Hyt.BLL.Cache.DeleteCache.ProductInfo(productSysNo);
                        //更新索引
                        BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(productSysNo);
                    }
                }
                isPass = u > 0 && isPass;
            }
            ////更改调价后，分销商商品状态为下架
            var res = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.下架, model[0].ProductSysNo);

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 添加调价申请
        /// </summary>
        /// <param name="productSysNoList">申请调价的商品编号数组</param>
        /// <param name="priceSourceType">调价价格类型列表</param>
        /// <returns>返回视图</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001202)]
        public ActionResult AddPriceHistory(int[] productSysNoList, int[] priceSourceType = null)
        {
            ////设置用于显示的价格来源类型
            priceSourceType = priceSourceType ?? new int[0]; //设置默认值

            //如果没有设置来源参数将返回所有价格
            if (priceSourceType == null || priceSourceType.Length == 0)
            {
                //将枚举转化成数据字典
                IDictionary<int, string> priceTypeList = EnumUtil.ToDictionary(typeof(ProductStatus.产品价格来源));

                priceSourceType = priceTypeList.Keys.ToArray<int>();
            }
            //返回给前台作为ajax请求的参数
            //生产以逗号“，”分隔的字符串
            ViewBag.PriceType = priceSourceType.AsDelimited(",");

            //序列化成数组功前台调用Ajax方法来读取价格
            ViewBag.Products = JsonConvert.SerializeObject(productSysNoList);
            return View();
        }

        #endregion

        #region 搭配销售商品功能

        /// <summary>
        /// 更新搭配销售商品列表
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(作为code查询条件)</param>
        /// <param name="removeProductSysNoList">要从搭配商品列表中移除的商品列表</param>
        /// <param name="newProductSysNoList">要被加入到搭配商品中的商品列表</param>
        /// <returns>返回是否更新成功状态</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public JsonResult UpdatePdProductCollocation(int masterProductSysNo, int[] removeProductSysNoList,
                                                     int[] newProductSysNoList)
        {
            bool success = false;
            string msg = "";

            //检查要被添加的商品是否已经在商品类别中
            if (!PdProductCollocationBo.Instance.IsExist(masterProductSysNo, newProductSysNoList))
            {

                IList<PdProductCollocation> list = new List<PdProductCollocation>();

                for (int i = 0; i < newProductSysNoList.Length; i++)
                {
                    list.Add(new PdProductCollocation()
                    {
                        Code = masterProductSysNo,
                        ProductSysNo = newProductSysNoList[i],
                        CreatedBy = AdminAuthenticationBo.Instance.Current.Base.SysNo,
                        CreatedDate = DateTime.Now
                    });
                }
            }
            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 品牌管理

        /// <summary>
        /// 品牌管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.CM1010002, PrivilegeCode.PD1005001)]
        public ActionResult Brand(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.品牌状态.启用;
            }
            var list = new PagedList<PdBrand>();

            var pager = new Pager<PdBrand>
            {
                CurrentPage = pageIndex,
                PageFilter = new PdBrand { Status = status, Name = name },
                PageSize = list.PageSize
            };

            pager = BLL.Product.PdBrandBo.Instance.GetPdBrandList(pager);

            if (!string.IsNullOrEmpty(selector) && selector == "selector") //品牌组件view层
            {
                return PartialView("_AjaxBrandPagerSelector", pager.Map());
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxBrandPager", pager.Map());
            }
            return View();
        }

        /// <summary>
        /// 品牌添加界面
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1005201)]
        public ActionResult BrandCreate()
        {
            PdBrand model = new PdBrand();
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = BLL.Product.PdBrandBo.Instance.GetEntity(sysno);
            }
            return View(model);
        }


        /// <summary>
        /// 品牌添加界面
        /// </summary>
        /// <param name="model">品牌实体</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>        
        [HttpPost]
        [Privilege(PrivilegeCode.PD1005201)]
        public ActionResult BrandCreate(PdBrand model)
        {
            string des = model.SysNo > 0 ? "修改属性" : "创建属性";
            Result result = BLL.Product.PdBrandBo.Instance.BrandSave(model);
            if (result.Status)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param></param>
        /// <returns>返回成功的行数</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1005601)]
        public ActionResult UpdateBrandStatus()
        {
            int status = 0, _result = 0;
            int.TryParse(Request.Params["status"], out status);
            List<int> listSysno = Request.Params["sysno"].Split(',').ToList<string>().ConvertAll<int>(x => int.Parse(x));
            //转化为int防止注入
            if (listSysno.Count > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                _result = BLL.Product.PdBrandBo.Instance.UpdateStatus((Hyt.Model.WorkflowStatus.ProductStatus.品牌状态)status, listSysno);
                if (_result > 0)
                {
                    string des = Hyt.Model.WorkflowStatus.ProductStatus.品牌状态.启用.GetHashCode() == status ? "启用品牌" : "禁用品牌";
                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, listSysno[0], CurrentUser.Base.SysNo);
                }
            }
            return Json(new { result = _result });
        }

        /// <summary>
        /// 品牌选择组件
        /// </summary>
        /// <param></param>
        /// <returns>展示组件模型</returns>
        /// <remarks>2013-06-26 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.CM1010002)]
        public ActionResult BrandSelector()
        {
            return View();
        }

        #endregion

        #region 属性组管理


        /// <summary>
        /// 属性组管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1004001, PrivilegeCode.CM1010003)]
        public ActionResult ProductAttributeGroup(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }

            PagedList<PdAttributeGroup> list = new PagedList<PdAttributeGroup>();
            Pager<PdAttributeGroup> pager = new Pager<PdAttributeGroup>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new PdAttributeGroup { Status = status, Name = name };
            pager.PageSize = list.PageSize;

            pager = BLL.Product.PdAttributeGroupBo.Instance.GetPdAttributeGroupList(pager);

            list = new PagedList<PdAttributeGroup>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize

            };
            if (!string.IsNullOrEmpty(selector) && selector == "selector") //属性组组件view层
            {
                return PartialView("_AjaxProductAttributeGroupSelector", list);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxProductAttributeGroupPager", list);
            }
            return View();
        }

        /// <summary>
        /// 属性组添加界面
        /// </summary>
        /// <param>分页页码</param>
        /// <returns>属性组创建页面</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1004201)]
        public ActionResult ProductAttributeGroupCreate()
        {
            PdAttributeGroup model = new PdAttributeGroup();
            ViewBag.ListAttribute = new List<PdAttribute>();
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = BLL.Product.PdAttributeGroupBo.Instance.GetEntity(sysno);
                ViewBag.ListAttribute = BLL.Product.PdAttributeGroupBo.Instance.GetAttributes(sysno);
            }

            return View(model);
        }

        /// <summary>
        /// 属性组添加界面
        /// </summary>
        /// <param name="model">属性组实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1004201)]
        public ActionResult ProductAttributeGroupCreate(PdAttributeGroup model)
        {
            IList<int> listAttributeSysno = new List<int>();
            if (!string.IsNullOrEmpty(Request.Params["attributes"]))
            {
                listAttributeSysno =
                    Request.Params["attributes"].Split(',').ToList<string>().ConvertAll<int>(x => int.Parse(x));
                //转化为int防止注入
            }

            Result result;

            string des = model.SysNo > 0 ? "修改属性组" : "创建属性组";
            result = BLL.Product.PdAttributeGroupBo.Instance.SavePdAttributeGroup(model, listAttributeSysno);
            if (result.Status)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }

            return Json(result);
        }

        /// <summary>
        /// 更新属性组状态
        /// </summary>
        /// <param>属性组实体</param>
        /// <returns>返回成功的行数</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1004601)]
        public ActionResult UpdateProductAttributeGroupStatus()
        {
            int status = 0, _result = 0;
            int.TryParse(Request.Params["status"], out status);
            List<int> listSysno = Request.Params["sysno"].Split(',').ToList<string>().ConvertAll<int>(x => int.Parse(x));
            //转化为int防止注入
            if (listSysno.Count > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                _result = BLL.Product.PdAttributeGroupBo.Instance.UpdateStatus((Hyt.Model.WorkflowStatus.ProductStatus.商品属性分组状态)status, listSysno);
                if (_result > 0)
                {
                    string des = Hyt.Model.WorkflowStatus.ProductStatus.商品属性分组状态.启用.GetHashCode() == status ? "启用属性组" : "禁用属性组";
                    foreach (int sysno in listSysno)
                    {
                        BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                    }
                }
            }
            return Json(new { result = _result });
        }

        /// <summary>
        /// 查出已选的属性组
        /// </summary>
        /// <param></param>
        /// <returns>已选属性组列表页</returns>
        /// <remarks>2013-07-12 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CM1010003)]
        public ActionResult SelectedProductAttributeGroup()
        {
            List<int> listSysno = new List<int>();
            if (!string.IsNullOrEmpty(Request["listSysno"]))
            {
                string strSysno = Request["listSysno"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                listSysno = serializer.Deserialize<List<int>>(strSysno);
            }

            PagedList<PdAttributeGroup> list = new PagedList<PdAttributeGroup>
            {
                TData = BLL.Product.PdAttributeGroupBo.Instance.GetSelectedAttributeGroups(listSysno),
                PageSize = 0,
                CurrentPageIndex = 0,
                TotalItemCount = 0
            };
            return PartialView("_AjaxProductAttributeGroupSelector", list);
        }


        /// <summary>
        /// 获取单个属性信息
        /// </summary>
        /// <param></param>
        /// <returns>单个属性信息</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1002001, PrivilegeCode.PD1002201)]
        public ActionResult GetAttributeRecord()
        {
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            PdAttribute entity = BLL.Product.PdAttributeBo.Instance.GetEntity(sysno);
            return Json(entity);
        }

        #endregion

        #region 属性管理

        /// <summary>
        /// 搜索属性列表
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2013-07-08 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.CM1010004, PrivilegeCode.PD1002001)]
        public ActionResult ProductAttribute(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"] != null ? Server.UrlDecode(Request.Params["name"]) : null;
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性状态.启用;
            }

            PagedList<PdAttribute> list = new PagedList<PdAttribute>();
            Pager<PdAttribute> pager = new Pager<PdAttribute>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new PdAttribute { Status = status, AttributeName = name };
            pager.PageSize = list.PageSize;

            BLL.Product.PdAttributeBo.Instance.GetPdAttributeList(ref pager);

            list = new PagedList<PdAttribute>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };
            if (!string.IsNullOrEmpty(selector) && selector == "selector") //属性组组件view层
            {
                return PartialView("_AjaxPorductAttributeSelector", list);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxProductAttributePager", list);
            }
            return View();
        }

        /// <summary>
        /// 查出已选的属性
        /// </summary>
        /// <param></param>
        /// <returns>已选属性列表页</returns>
        /// <remarks>2013-07-10 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CM1010004)]
        public ActionResult SelectedProductAttribute()
        {
            List<int> listSysno = new List<int>();
            if (!string.IsNullOrEmpty(Request["listSysno"]))
            {
                string strSysno = Request["listSysno"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                listSysno = serializer.Deserialize<List<int>>(strSysno);
            }

            PagedList<PdAttribute> list = new PagedList<PdAttribute>
            {
                TData = BLL.Product.PdAttributeBo.Instance.GetSelectedAttributes(listSysno),
                PageSize = 0,
                CurrentPageIndex = 0,
                TotalItemCount = 0
            };
            return PartialView("_AjaxPorductAttributeSelector", list);
        }

        /// <summary>
        /// 更新属性状态
        /// </summary>
        /// <param></param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-07-05 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1002601)]
        public ActionResult UpdateProductAttributeStatus()
        {
            int status = 0;
            int.TryParse(Request.Params["status"], out status);
            List<int> listSysno = Request.Params["sysno"].Split(',').ToList<string>().ConvertAll<int>(x => int.Parse(x));
            //转化为int防止注入
            Result result = new Result();
            if (listSysno.Count > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                result = BLL.Product.PdAttributeBo.Instance.UpdateStatus((Hyt.Model.WorkflowStatus.ProductStatus.商品属性状态)status, listSysno);
                if (result.Status)
                {
                    string des = Hyt.Model.WorkflowStatus.ProductStatus.商品属性状态.启用.GetHashCode() == status ? "启用属性" : "禁用属性";
                    foreach (int sysno in listSysno)
                    {
                        BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                    }
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 判断属性选项是否被商品使用
        /// </summary>
        /// <param name="sysNo">选项编号</param>
        /// <returns>被使用返回true，未被使用返回false</returns>
        /// <remarks>2013-07-30 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1004201)]
        public ActionResult IsAttributeOptionsInProduct(int sysNo)
        {
            Result result = new Result();
            result.Status = BLL.Product.PdAttributeBo.Instance.IsAttributeOptionsInProduct(sysNo);
            if (result.Status)
            {
                result.Message = "属性选项已被使用，不能删除该属性";
            }
            else
            {
                result.Message = "属性选项未被使用，可以删除";
            }
            return Json(result);

        }

        /// <summary>
        /// 属性详细页面展示
        /// </summary>
        /// <returns>属性模型</returns>
        /// <param name="model">属性实体</param>
        /// <remarks>2013-07-06 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1002201)]
        public ActionResult ProductAttributeCreate(PdAttribute model)
        {
            //保存逻辑
            if (!string.IsNullOrEmpty(model.AttributeName))
            {
                IList<PdAttributeOption> listOptions = new List<PdAttributeOption>();
                if (model.AttributeType == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品属性类型.选项类型)
                {
                    if (!string.IsNullOrEmpty(Request["items"]))
                    {
                        string items = Request["items"];
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        listOptions = serializer.Deserialize<List<PdAttributeOption>>(items);
                    }
                }

                Result result;

                string des = model.SysNo > 0 ? "修改属性" : "创建属性";
                result = BLL.Product.PdAttributeBo.Instance.SavePdAttribute(model, listOptions);
                if (result.Status)
                {
                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
                }

                return Json(result);
            }

            ViewBag.ListAttributeOptions = new List<PdAttributeOption>();
            //查看逻辑
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = BLL.Product.PdAttributeBo.Instance.GetEntity(sysno);
                ViewBag.ListAttributeOptions = BLL.Product.PdAttributeBo.Instance.GetAttributeOptions(sysno);
            }
            return View(model);
        }

        #endregion

        #region 商品管理

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="productDetail">商品参数过滤类</param>
        /// <returns>列表视图</returns>
        /// <remarks>2013-07-14 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult ProductList(ParaProductFilter productDetail)
        {
            int pageIndex = productDetail.id ?? 1;
            int status = 0;
            int IsFrontDisplay = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["IsFrontDisplay"], out IsFrontDisplay);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }
            status = -1;
            IsFrontDisplay = 1;
            var list = new PagedList<CBPdProductDetail>();
            list.PageSize = 15;
            var pager = new Pager<CBPdProductDetail>
            {
                CurrentPage = pageIndex,
                PageSize = list.PageSize
            };

            //当前用户对应分销商，2016-3-7 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                productDetail.DealerSysNo = DealerSysNo;
                productDetail.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            productDetail.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            productDetail.DealerCreatedBy = CurrentUser.Base.SysNo;

            // 原产地列表
            ParaOriginFilter oFilter = new ParaOriginFilter();
            oFilter.Id = 1;
            oFilter.PageSize = 300;
            ViewBag.OriginList = BLL.Basic.OriginBo.Instance.GetOriginList(oFilter).Rows;

            if (Request.IsAjaxRequest())
            {
                BLL.Product.PdProductBo.Instance.GetPdProductDetailList(ref pager, productDetail);

                return PartialView("_AjaxProductPager", pager.Map());
            }

            return View(list);
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        //// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult GetPdProductList(int? id)
        {


            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string GroupSysNoList = Request.Params["GroupSysNoList"];
            if (string.IsNullOrWhiteSpace(GroupSysNoList))
            {
                GroupSysNoList = null;
            }
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }
            var list = new PagedList<CBPdProduct>();

            var pager = new Pager<CBPdProduct>
            {
                CurrentPage = pageIndex,
                PageFilter = new CBPdProduct { Status = status, ErpCode = name, EasName = name, GroupSysNoList = GroupSysNoList },
                PageSize = list.PageSize
            };

            pager = PdProductBo.Instance.GetCBPdProductList(pager);

            if (!string.IsNullOrEmpty(selector) && selector == "selector") //品牌组件view层
            {
                return PartialView("_AjaxProductPagerSelector", pager.Map());
            }
            return View();
        }

        /// <summary>
        /// 分销商可添加的商品详细信息列表
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        //// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult GetDealerMallProductList(int? id)
        {
            //当前用户对应商城SysNo
            CBDsDealerMall dmModel = DsDealerMallBo.Instance.GetDsDealerMallByDealerSysNo(CurrentUser.Dealer.SysNo);
            int dealerMallSysNo = dmModel.SysNo;
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string GroupSysNoList = Request.Params["GroupSysNoList"];
            if (string.IsNullOrWhiteSpace(GroupSysNoList))
            {
                GroupSysNoList = null;
            }
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }
            var list = new PagedList<CBPdProduct>();

            var pager = new Pager<CBPdProduct>
            {
                CurrentPage = pageIndex,
                PageFilter = new CBPdProduct { Status = status, ErpCode = name, EasName = name, GroupSysNoList = GroupSysNoList },
                PageSize = list.PageSize
            };

            pager = PdProductBo.Instance.GetDealerMallProductList(pager, dealerMallSysNo);

            if (!string.IsNullOrEmpty(selector) && selector == "selector") //品牌组件view层
            {
                return PartialView("_AjaxProductPagerSelector", pager.Map());
            }
            return View();
        }

        public static bool _starting;
        /// <summary>
        /// 导入商品excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult ImportExcel()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {
                        result = PdProductBo.Instance.ImportXinYingExcel(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);//PdProductBo.Instance.ImportXinYingExcelOneTimeMethod(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);//
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                        result.Status = false;
                    }
                    finally
                    {
                        _starting = false;
                    }
                }
                else
                {
                    result.Message = string.Format("正在导入数据，请稍后再操作");
                    result.Status = false;

                }
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            //return to excute the page script
            return View();

        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param></param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-07-16 唐永勤 创建</remarks>
        /// <remarks>2017-09-15 罗勤尧 重构</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001601)]
        public ActionResult UpdateProductStatus()
        {
            int status = 0, sysno = 0, dealersysno = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["sysno"], out sysno);
            int.TryParse(Request.Params["dealersysno"], out dealersysno);

            //转化为int防止注入
            Result result = new Result();

            if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                result = BLL.Product.PdProductBo.Instance.UpdateStatus(status, sysno);
                if (result.Status)
                {
                    //强行上下架分销商商品，0 下架，1 上架
                    //2017-9-15 罗勤尧 创建
                    //dealersysno=0 总部商品上架，总部直营店分销商商品上架，同步到分销商商品表中
                    List<int> dealersysnolist = new List<int>();
                    dealersysnolist.Add(347);
                    dealersysnolist.Add(336);
                    dealersysnolist.Add(44);
                    dealersysnolist.Add(14);
                    dealersysnolist.Add(0);
                    if (status == 1)
                    {
                        //获得该商品对应的会员价
                        PdPrice Price = PdPriceBo.Instance.GetSalesPrice(sysno, (int)ProductStatus.产品价格来源.会员等级价.GetHashCode());
                        foreach (int ds in dealersysnolist)
                        {
                            //查询分销商特殊价格
                            DsSpecialPrice entity = DsSpecialPriceBo.Instance.GetEntityByDPSysNo(ds, sysno);
                            if (entity == null)
                            {
                                DsSpecialPrice model = new DsSpecialPrice();
                                model.DealerSysNo = ds;
                                model.ProductSysNo = sysno;
                                model.Price = Price.Price;
                                model.Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode();
                                model.CreatedBy = CurrentUser.Base.SysNo;
                                model.CreatedDate = DateTime.Now;
                                model.LastUpdateBy = CurrentUser.Base.SysNo;
                                model.LastUpdateDate = DateTime.Now;
                                DsSpecialPriceBo.Instance.Create(model);
                            }
                            else
                            {
                                DsSpecialPriceBo.Instance.UpdatePriceStatus(Price.Price, Price.Price, DistributionStatus.分销商特殊价格状态.启用.GetHashCode(), entity.SysNo);
                            }
                            //分销商商品状态为上架
                            var res = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.上架, sysno, ds);
                        }
                    }
                    else
                    {
                        foreach (int ds in dealersysnolist)
                        {
                            //分销商商品状态为下架
                            var res = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.下架, sysno, ds);
                        }
                    }
               
                    string des = Hyt.Model.WorkflowStatus.ProductStatus.产品上线状态.有效.GetHashCode() == status ? "商品上线" : "商品下线";
                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                    BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(sysno);
                    var cacheKey = string.Format(KeyConstant.ProductEasName, sysno);
                    MemoryProvider.Default.Remove(cacheKey);
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 批量更新商品状态
        /// </summary>
        /// <param></param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001601)]
        public ActionResult UpdatePLProductStatus()
        {
            string productSysNoList = Request.Params["productSysNoList"];
            string strstatus = Request.Params["status"];
            string[] ProductSysNo = productSysNoList.Split(',');
            Result result = new Result();
            for (int i = 0; i < ProductSysNo.Length; i++)
            {
                int status = 0, sysno = 0;
                int.TryParse(strstatus, out status);
                int.TryParse(ProductSysNo[i], out sysno);

                //转化为int防止注入
                if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
                {
                    result = BLL.Product.PdProductBo.Instance.UpdateStatus(status, sysno);
                    if (result.Status)
                    {
                        //强行上下架分销商商品，0 下架，1 上架
                        //2017-9-15 罗勤尧 创建
                        //dealersysno=0 总部商品上架，总部直营店分销商商品上架，同步到分销商商品表中
                        List<int> dealersysnolist = new List<int>();
                        dealersysnolist.Add(347);
                        dealersysnolist.Add(336);
                        dealersysnolist.Add(44);
                        dealersysnolist.Add(14);
                        dealersysnolist.Add(0);
                        if (status == 1)
                        {
                            //获得该商品对应的会员价
                            PdPrice Price = PdPriceBo.Instance.GetSalesPrice(sysno, (int)ProductStatus.产品价格来源.会员等级价.GetHashCode());
                            foreach (int ds in dealersysnolist)
                            {
                                //查询分销商特殊价格
                                DsSpecialPrice entity = DsSpecialPriceBo.Instance.GetEntityByDPSysNo(ds, sysno);
                                if (entity == null)
                                {
                                    DsSpecialPrice model = new DsSpecialPrice();
                                    model.DealerSysNo = ds;
                                    model.ProductSysNo = sysno;
                                    model.Price = Price.Price;
                                    model.Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode();
                                    model.CreatedBy = CurrentUser.Base.SysNo;
                                    model.CreatedDate = DateTime.Now;
                                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                                    model.LastUpdateDate = DateTime.Now;
                                    DsSpecialPriceBo.Instance.Create(model);
                                }
                                else
                                {
                                    DsSpecialPriceBo.Instance.UpdatePriceStatus(Price.Price, Price.Price, DistributionStatus.分销商特殊价格状态.启用.GetHashCode(), entity.SysNo);
                                }
                                //分销商商品状态为上架
                                var res = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.上架, sysno, ds);
                            }
                        }
                        else
                        {
                            foreach (int ds in dealersysnolist)
                            {
                                //分销商商品状态为下架
                                var res = DsSpecialPriceBo.Instance.UpdatePriceStatusByPro((int)ProductStatus.商品状态.下架, sysno, ds);
                            }
                        }
                        //获得该商品对应的会员价
                        //PdPrice Price = PdPriceBo.Instance.GetSalesPrice(sysno, (int)ProductStatus.产品价格来源.会员等级价.GetHashCode());
                        //IList<DsDealer> Dealers = DsDealerBo.Instance.GetDsDealersList();
                        //foreach (DsDealer Dealer in Dealers)
                        //{
                        //    int DealerSysNo = Dealer.SysNo;
                        //    DsSpecialPrice model = new DsSpecialPrice();
                        //    model.DealerSysNo = DealerSysNo;
                        //    model.ProductSysNo = sysno;
                        //    model.Price = Price.Price;
                        //    model.Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode();
                        //    model.CreatedBy = CurrentUser.Base.SysNo;
                        //    model.CreatedDate = DateTime.Now;
                        //    model.LastUpdateBy = CurrentUser.Base.SysNo;
                        //    model.LastUpdateDate = DateTime.Now;
                        //    DsSpecialPriceBo.Instance.Create(model);
                        //}

                        string des = Hyt.Model.WorkflowStatus.ProductStatus.产品上线状态.有效.GetHashCode() == status ? "商品上线" : "商品下线";
                        BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                        BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(sysno);
                        var cacheKey = string.Format(KeyConstant.ProductEasName, sysno);
                        MemoryProvider.Default.Remove(cacheKey);
                    }
                }
            }

            return Json(result);
        }

        /// <summary>
        /// 作废商品状态/不作废商品状态
        /// </summary>
        /// <param></param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2016-3-29 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001701)]
        public ActionResult CancelOrCreateProductStatus()
        {
            int status = 0, sysno = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["sysno"], out sysno);
            Result result = new Result();
            if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                //判断作废的商品是否还有库存 王耀发 2016-6-2 创建
                IList<PdProductStockList> StockData = PdProductStockBo.Instance.GetProStockListByProductSysNo(sysno);
                string reStr = "";
                if (StockData.Count > 0)
                {
                    foreach (var item in StockData)
                    {
                        reStr += "仓库：" + item.BackWarehouseName + "&nbsp;&nbsp;&nbsp;库存数量：" + item.StockQuantity.ToString() + "<br/>";
                    }
                    reStr = "商品有库存不能作废：<br/>" + reStr;
                    result.Status = false;
                    result.Message = reStr;
                    return Json(result);
                }
                result = BLL.Product.PdProductBo.Instance.UpdateStatus(status, sysno);
            }
            return Json(result);
        }

        /// <summary>
        /// 克隆商品
        /// </summary>
        /// <param name="sysNo">商品编号</param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2013-07-16 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public ActionResult CopyProduct(int sysNo)
        {
            return View(sysNo);
        }

        /// <summary>
        /// 克隆商品
        /// </summary>
        /// <param name="product">克隆商品的信息</param>
        /// <returns>克隆结果信息</returns>
        /// <remarks>2013-07-25 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001201)]
        public ActionResult CloneProduct(PdProduct product)
        {
            Result result = new Result();
            result.Status = true;

            #region 数据验证

            //检查商品名称编号
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                result.Status = false;
                result.Message = "商品编号已经被其他商品使用";
            }

            //检查商品编号
            if (result.Status && !PdProductBo.Instance.CheckERPCode(product.ErpCode, product.SysNo))
            {
                result.Status = false;
                result.Message = "商品编号已经被其他商品使用";
            }

            #endregion

            if (result.Status)
            {


                //生产拼音
                try
                {
                    product.NameAcronymy = Hyt.Util.CHS2PinYin.Convert(product.ProductName, true);
                    product.NameAcronymy = product.NameAcronymy.Length > 600
                                                  ? product.NameAcronymy.Substring(0, 600)
                                                  : product.NameAcronymy;
                }
                catch
                {
                    ;
                }

                result = PdProductBo.Instance.CloneProduct(product);


            }
            return Json(result);
        }

        /// <summary>
        /// 读取商品价格信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回Pop弹出成视图</returns>
        /// <remarks>2013-10-10 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001202, PrivilegeCode.PD1003001)]
        public ActionResult GetProductPriceInfo(int productSysNo)
        {

            //读取所以类型的价格类型列表
            IDictionary<int, string> typeList = Hyt.Util.EnumUtil.ToDictionary(typeof(ProductStatus.产品价格来源));

            //读取所有价格
            var priceList = GetProductPriceAllInfo(productSysNo);

            //商品价格结果集
            var prices = new List<dynamic>();

            dynamic obj;

            //更具商品类型来对商品价格进行归类
            //为了在前台能够使用动态类型所以要将linq生产的匿名类用真正的dynamic对象来包装一次，否则前台无法使用动态类
            dynamic tempPriceObj;
            List<dynamic> tempPriceList;
            foreach (var ptype in typeList)
            {
                obj = new ExpandoObject();
                IList<dynamic> tempPrices = priceList.Where(p => p.priceTypeSysNo == ptype.Key).ToList();   //查找统一类型的商品价格

                //只有当找到有价格时才对价格进行dynamic包装
                if (tempPrices.Count > 0)
                {
                    tempPriceList = new List<dynamic>();
                    obj.priceType = ptype.Value;

                    //将价格添加到分类下的价格
                    foreach (var tempPrice in tempPrices)
                    {
                        tempPriceObj = new ExpandoObject();
                        tempPriceObj.priceName = tempPrice.priceName;
                        tempPriceObj.price = tempPrice.price;
                        tempPriceList.Add(tempPriceObj);
                    }

                    //添加到结果集中
                    obj.prices = tempPriceList;
                    prices.Add(obj);
                }

            }

            ViewBag.Pices = prices;

            return PartialView("_AjaxPopProductPriceInfo");
        }

        #endregion

        #region 商品选择  属性选择  属性组选择

        /// <summary>
        /// 商品选择
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns>商品现在页面</returns>
        /// <remarks>2013-06-28 黄波  创建</remarks>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult SelectProduct(int? id)
        {
            /*
             * 商品选择组件只提供商品的简单选择和筛选。由于该组件同时被应用到了关联商品选择
             * 所以扩展了特殊筛选方式(通过关联属性进行筛选)
             *  productsysno：商品系统编号
             *      category:商品分类系统编号
             *     subfilter:特殊过滤关键字
             *            rc:过滤关键字值
             *        single：
             *selectedIsReadOnly:已选对象为只读
             */

            int productSysNo = 0;

            //商品系统编号过滤,转换商品系统编号
            int.TryParse(Request.Params["productsysno"], out productSysNo);
            ViewBag.ProductSysNo = productSysNo;

            //商品分类过滤
            int category;
            int.TryParse(Request.Params["category"], out category);
            if (category > 0)
            {
                PdCategory categoryModel = PdCategoryBo.Instance.GetCategory(category);
                if (categoryModel != null)
                {
                    ViewBag.CategoryModel = categoryModel;
                }
            }

            //特殊过滤筛选
            string subfilter = Request.Params["subfilter"] ?? "";
            ViewBag.SubFilter = subfilter;

            //对应分销商 王耀发 2016-2-2 创建
            string dealerSysNo = Request.Params["dealerSysNo"] ?? "";
            ViewBag.DealerSysNo = dealerSysNo;

            //对应仓库 王耀发 2016-2-2 创建
            string warehouseSysNo = Request.Params["warehouseSysNo"] ?? "";
            ViewBag.WarehouseSysNo = warehouseSysNo;

            //更具筛选关键字进行缓存筛选值，这个缓存的筛选值将被用在分页时一同带入
            switch (subfilter.ToLower())
            {
                case "associationattribute":
                    ViewBag.SubFilterValue = "rc='" + (Request.Params["rc"] ?? "") + "'";
                    break;
            }

            //是否是单商品选择，一次只能选择一个商品
            ViewBag.SingleSelect = (Request.Params["single"] != null && Request.Params["single"].ToString().ToLower() == "true");

            //已选对象只读
            ViewBag.SelectedIsReadOnly = Request.Params["selectedIsReadOnly"] != null;

            //展示商品同步前台展示商品，只看前台能看到的商品
            ViewBag.SyncWebFront = Request.Params["syncWebFront"] != null;

            return View("_SelectProduct");
        }

        /// <summary>
        /// 促销商品选择
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns>促销商品选择页面</returns>
        /// <remarks>2013-09-23 余勇  创建</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult SelectPromotionProduct(int? id)
        {
            int productSysNo = 0;
            int customerSysNo = 0;
            int warehouseSysNo = 0;
            int dealerSysNo = 0;
            int.TryParse(Request.Params["warehouseSysNo"], out warehouseSysNo);
            ViewBag.WarehouseSysNo = warehouseSysNo;

            int.TryParse(Request.Params["dealerSysNo"], out dealerSysNo);
            ViewBag.dealerSysNo = dealerSysNo;

            int.TryParse(Request.Params["productsysno"], out productSysNo);
            ViewBag.ProductSysNo = productSysNo;

            int.TryParse(Request.Params["customerSysNo"], out customerSysNo);
            ViewBag.CustomerSysNo = customerSysNo;

            string subfilter = Request.Params["subfilter"] ?? "";
            ViewBag.SubFilter = subfilter;
            switch (subfilter.ToLower())
            {
                case "associationattribute":
                    ViewBag.SubFilterValue = "rc='" + (Request.Params["rc"] ?? "") + "'";
                    break;
            }

            int category;
            int.TryParse(Request.Params["category"], out category);
            if (category > 0)
            {
                PdCategory categoryModel = PdCategoryBo.Instance.GetCategory(category);
                if (categoryModel != null)
                {
                    ViewBag.CategoryModel = categoryModel;
                }
            }

            ViewBag.SingleSelect = (Request.Params["single"] != null && Request.Params["single"].ToString().ToLower() == "true");

            ViewBag.Data = Request.Params["data"];
            return View("_SelectPromotionProduct");
        }

        /// <summary>
        /// 商品查询
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        /// <remarks>2013-12-03 邵斌 扩展：加入条件SyncWebFront 只显示前台同步能看到的商品</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult SearchProduct(int? id)
        {
            /*
             * 生产前台搜索查询URL地址
             */
            return SearchProductByUrl(id, "_AjaxProductSelectorPager");
        }

        /// <summary>
        /// 促销商品查询
        /// </summary>
        /// <param name="id">页索引 从1开始.</param>
        /// <returns>
        /// 返回促销商品列表
        /// </returns>
        /// <remarks>
        /// 2013-09-23 余勇
        /// </remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult SearchProductPromotion(int? id)
        {
            return SearchProductByUrl(id, "_AjaxProductPromotionSelectorForPager");
        }

        /// <summary>
        /// 商品查询
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <param name="url">加载url  或者 为响应呈现的视图的名称 </param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-12-03 邵斌 扩展：加入条件SyncWebFront 只显示前台同步能看到的商品</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult SearchProductByUrl(int? id, string url)
        {
            int pageIndex = id ?? 1; //设置默认当前页面
            int customerSysNo = 0;
            int productSysNo = 0;
            int warehouseSysNo = 0;
            int dealerSysNo = 0;
            int.TryParse(Request.Params["productsysno"], out productSysNo);
            ViewBag.ProductSysNo = productSysNo;

            int.TryParse(Request.Params["customerSysNo"], out customerSysNo);
            ViewBag.CustomerSysNo = customerSysNo;

            int.TryParse(Request.Params["warehouseSysNo"], out warehouseSysNo);
            //判断是否是经销商调用 王耀发 2016-2-4 创建
            string strDealerSysNo = Request.Params["dealerSysNo"];
            bool isDealer = true;

            if (string.IsNullOrEmpty(strDealerSysNo))
            {
                isDealer = false;//没有传入经销商
            }
            else
            {
                int.TryParse(strDealerSysNo, out dealerSysNo);
            }

            //是否带有订单经销商系统编号 王耀发 2015-12-25 创建


            #region 商品选择调用

            //判断是否是选择组件查询
            bool isSelector; //是否是商品选择组件调用标志
            bool.TryParse(Request.Params["isSelector"], out isSelector);
            //判断是现在器查询还是管理页查询
            if (isSelector)
            {
                string stringFilter = Request.Params["filter"]; //商品名称或商品商品编号 
                //2015-11-19 王耀发 添加
                if (stringFilter == "后台显示名称/商品编号") stringFilter = null;

                stringFilter = string.IsNullOrWhiteSpace(stringFilter) ? null : stringFilter.Trim(); //清除头尾空格

                //商品分类系统编号
                int categorySysNo;
                int.TryParse(Request.Params["categorySysNo"], out categorySysNo);

                //返回结果
                var pager = new Pager<ParaProductSearchFilter>();
                pager.CurrentPage = pageIndex;

                //设置查询条件
                pager.PageFilter = new ParaProductSearchFilter
                {
                    ProductName = stringFilter,
                    ErpCode = stringFilter,
                    WarehouseSysNo = warehouseSysNo,
                    ProductCategorySysNo = categorySysNo,
                    DealerSysNo = dealerSysNo
                };

                PagedList<ParaProductSearchFilter> list = null; //分页结果集

                //定制子查询过滤商品
                string subFilter = Request.Params["subfilter"] ?? "";
                switch (subFilter.ToLower())
                {
                    case "associationattribute":
                        pager.PageFilter.SysNo = productSysNo;
                        pager.PageFilter.RequiredFilterAttribute = true;

                        //获取关联关系码
                        var relationCode = (Request.Params["rc"] == null) ? "" : Request.Params["rc"].ToString();

                        //如果没有关联关系码将从商品中读取
                        if (string.IsNullOrWhiteSpace(relationCode))
                        {
                            relationCode = PdProductAssociationBo.Instance.GetRelationCode(productSysNo);
                        }

                        pager.PageFilter.RelationCode = relationCode;
                        break;
                }

            #endregion

                bool syncWebFront = false;
                if (Request.Params["syncWebFront"] != null)
                {
                    bool.TryParse(Request.Params["syncWebFront"].ToString(), out syncWebFront);
                }
                pager.PageFilter.SyncWebFront = syncWebFront;

                try
                {
                    pager.PageFilter.SelectStockProduct = false;
                    //if (!isDealer)
                    //{
                    PdProductBo.Instance.ProductSelectorProductSearch(ref pager, out list); //执行查询
                    //}
                    // else
                    // {
                    //     PdProductBo.Instance.DealerProductSearch(ref pager, out list); //执行查询 
                    //}
                    list.Style = PagedList.StyleEnum.Mini; //设置分页样式为mini样式
                }
                catch (Exception ex)
                {
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品查询错误:" + ex.Message,
                               LogStatus.系统日志目标类型.订单, 0, ex, null, 0);
                }

                ViewBag.Single = bool.Parse(Request.Params["single"].ToString());
                return PartialView(url, list);

            }
            return null;
        }

        /// <summary>
        /// 获取已经选择商品的详细信息
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <param name="subFilter">特殊筛选过滤调价</param>
        /// <returns>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public JsonResult GetSelectedProductInfo(IList<int> productList, string subFilter = "")
        {
            IList<CBPdProduct> list = PdProductBo.Instance.GetSelectedProductInfo(productList);
            IList<dynamic> result = new List<dynamic>();
            int levelEnum = (int)ProductStatus.产品价格来源.会员等级价;
            int basePriceEnum = (int)ProductStatus.产品价格来源.基础价格;
            int courierPriceEnum = (int)ProductStatus.产品价格来源.配送员进货价;

            dynamic obj;
            foreach (CBPdProduct p in list)
            {
                obj = new ExpandoObject();

                obj.name = p.EasName;
                obj.pid = p.SysNo;
                obj.erp = p.ErpCode;
                obj.price = p.PdPrice.Value.Where(x => x.PriceSource.Equals(levelEnum))
                             .Select(x => new { leave = x.SourceSysNo, price = x.Price })
                             .ToList();
                obj.baseprice = p.PdPrice.Value.Where(x => x.PriceSource.Equals(basePriceEnum))
                                 .Select(x => x.Price)
                                 .FirstOrDefault();
                obj.courier = p.PdPrice.Value.Where(x => x.PriceSource.Equals(courierPriceEnum))
                               .Select(x => x.Price)
                               .FirstOrDefault();
                obj.category = p.PdCategory.Value.Where(c => c.IsMaster.Equals(true)).Select(c => new
                {
                    name = c.CategoryName,
                    sysno = c.SysNo
                });
                obj.attributes = null;

                //定制子查询过滤商品
                switch (subFilter.ToLower())
                {
                    case "associationattribute":
                        obj.attributes = PdProductBo.Instance.GetProductAttributeByProductSysno(p.SysNo, true);
                        break;
                }

                result.Add(new
                {
                    name = obj.name,
                    pid = obj.pid,
                    erp = obj.erp,
                    price = obj.price,
                    baseprice = obj.baseprice,
                    courier = obj.courier,
                    category = obj.category,
                    attributes = obj.attributes,
                    Barcode = PdProductBo.Instance.GetProductBarcode(obj.pid)
                });

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取已经选择的商品列表
        /// </summary>
        /// <returns>>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CM1005801, PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult GetGetSelectedProductList()
        {
            IList<int> products;

            //反序列化传入的已选择商品列表
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            products = JsonConvert.DeserializeObject<List<int>>(Request["selectedProductList"]);

            products = products ?? new List<int>();

            //结果集
            PagedList<ParaProductSearchFilter> list = new PagedList<ParaProductSearchFilter>
            {
                TData = PdProductBo.Instance.GetSelectedProductList(products), //执行查询
                PageSize = 0,
                CurrentPageIndex = 0,
                TotalItemCount = 0
            };
            return PartialView("_AjaxProductSelectorPager", list);
        }

        /// <summary>
        /// 商品基础属性选择
        /// </summary>
        /// <param name="id">当前页索引号</param>
        /// <returns>属性选择页面</returns>
        /// <remarks>2013-06-28 黄波  创建</remarks>
        public ActionResult SelectAttribute(int? id)
        {
            int pageIndex = id ?? 1;
            //取分页查询数据
            Pager<PdAttribute> pager = new Pager<PdAttribute>
            {
                CurrentPage = pageIndex,
                PageFilter = new PdAttribute
                {
                    Status = (int)ProductStatus.商品属性状态.启用
                },
                PageSize = 15
            };
            PdAttributeBo.Instance.GetPdAttributeList(ref pager);
            //包装数据
            var model = new PagedList<PdAttribute>
            {
                TotalItemCount = pager.TotalRows,
                CurrentPageIndex = pageIndex,
                TData = pager.Rows,
                PageSize = pager.PageSize
            };
            //如果是AJAX请求 则返回分部视图
            if (Request.IsAjaxRequest())
            {
                System.Threading.Thread.Sleep(2000);
                return PartialView("_AjaxSelectAttrbute", model);
            }
            return View("_SelectAttribute", model);
        }

        /// <summary>
        /// 属性选择组件
        /// </summary>
        /// <param></param>
        /// <returns>展示组件模型</returns>
        /// <remarks>2013-07-10 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.CM1010004)]
        public ActionResult ProductAttributeSelector()
        {
            return View();
        }

        /// <summary>
        /// 属性组选择组件
        /// </summary>
        /// <param></param>
        /// <returns>展示组件模型</returns>
        /// <remarks>2013-07-11 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.CM1010003)]
        public ActionResult ProductAttributeGroupSelector()
        {
            return View();
        }

        /// <summary>
        /// 商品基础属性选择
        /// </summary>
        /// <param></param>
        /// <returns>属性选择页面</returns>
        /// <remarks>2013-06-28 黄波  创建</remarks>
        public ActionResult SelectAttributiGroup()
        {
            return View("_SelectAttributiGroup");
        }

        #endregion

        #region 商品信息添加/修改管理

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult AddProduct()
        {
            List<SelectListItem> pTypeList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<ProductStatus.商品类型>(ref pTypeList);
            pTypeList.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "-1"
            });
            ViewBag.ProductTypeList = pTypeList;
            return View("ProductAddAndUpdate");
        }

        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult CreateProductQRCode(string sysNos)
        {
            ViewBag.sysNos = sysNos.Split(',');
            return View("ProductQRCodeCreate");
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns>返回视图</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public ActionResult EditProduct(int sysNo)
        {
            //检查商品系统
            if (sysNo == 0)
                sysNo = -1;
            ViewBag.ProductSysNo = sysNo;
            return AddProduct();
        }

        /// <summary>
        /// 查看商品
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns>返回视图</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult ViewProduct(int sysNo)
        {
            ViewBag.ReadOnly = true;

            //检查商品系统
            if (sysNo == 0)
            {
                sysNo = -1;
            }
            else
            {
                var product = BLL.Web.PdProductBo.Instance.GetProduct(sysNo);
                //如果用户有修改权限将还是当做修改商品处理
                if (product != null && product.Status == (int)Model.WorkflowStatus.ProductStatus.商品状态.下架 && !AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.PD1001201))
                {
                    ViewBag.ReadOnly = true;
                }
            }

            ViewBag.ProductSysNo = sysNo;
            return AddProduct();
        }

        /// <summary>
        /// 商品属性添加页面
        /// </summary>
        /// <param></param>
        /// <returns>商品属性列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult AddProductAttribute()
        {
            //页面初始化获取数据
            IList<CBPdProductAtttributeRead> list = new List<CBPdProductAtttributeRead>();

            List<int> listSysNo = new List<int>();
            if (!string.IsNullOrEmpty(Request["listSysno"]))
            {
                string strSysNo = Request["listSysno"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                listSysNo = serializer.Deserialize<List<int>>(strSysNo);
                list = PdProductBo.Instance.GetProductAttributeByAttributeSysNo(listSysNo);

            }
            return PartialView("_GenerateProductAttribute", list);
        }

        /// <summary>
        /// 商品属性选项卡,_UpdateProductAttributeInfo页面的数据展示
        /// </summary>
        /// <param></param>
        /// <returns>商品属性</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult UpdateProductAttributeInfo()
        {
            //页面初始化获取数据
            IList<CBPdProductAtttributeReadRelation> list = new List<CBPdProductAtttributeReadRelation>();
            int productSysNo = 0;
            if (!string.IsNullOrEmpty(Request["sysNo"]))
            {
                int.TryParse(Request.Params["sysNo"], out productSysNo);
            }

            //根据商品获取已有属性
            if (productSysNo > 0)
            {
                list = PdProductBo.Instance.GetProductAttributeByProductSysNo(productSysNo);
                return PartialView("_UpdateProductAttributeInfo", list);
            }

            //根据属性组获取属性
            List<int> listSysno = new List<int>();
            if (!string.IsNullOrEmpty(Request["listSysno"]))
            {
                string strSysno = Request["listSysno"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                listSysno = serializer.Deserialize<List<int>>(strSysno);
                list = PdProductBo.Instance.GetProductAttributeByGroupSysno(listSysno);
                return PartialView("_GenerateProductAttributeGroup", list);
            }
            //ajax代表是属性组
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GenerateProductAttributeGroup", list);
            }

            return PartialView("_UpdateProductAttributeInfo", list);
        }

        /// <summary>
        /// 根据商品分类获取属性
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.PD1007001, PrivilegeCode.PD1007201)]
        public ActionResult GetCategoryProductAttributeByProductSysNo()
        {
            IList<CBPdProductAtttributeReadRelation> list = new List<CBPdProductAtttributeReadRelation>();
            int productSysNo = 0;
            if (!string.IsNullOrEmpty(Request["productSysno"]))
            {
                int.TryParse(Request.Params["productSysno"], out productSysNo);
            }
            if (productSysNo > 0)
            {
                list = PdProductBo.Instance.GetCategoryProductAttributeByProductSysNo(productSysNo);
            }
            return PartialView("_GenerateProductAttributeGroup", list);
        }

        /// <summary>
        /// 商品属性保存
        /// </summary>
        /// <param></param>
        /// <returns>结果对象</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001201)]
        public ActionResult SaveProductAttribute()
        {
            IList<PdProductAttribute> list = new List<PdProductAttribute>();
            Result result = new Result();
            int productSysno = 0;
            //属性项对应图片
            string attributeOptionImageList = Request["attributeOptionImageList"];

            if (!string.IsNullOrEmpty(Request["productSysno"]))
            {
                int.TryParse(Request.Params["productSysno"], out productSysno);
            }

            if (!string.IsNullOrEmpty(Request["items"]))
            {
                string items = Request["items"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                list = serializer.Deserialize<List<PdProductAttribute>>(items);
                result = PdProductBo.Instance.SaveProductAttribute(productSysno, list);
                //保存属性项对应图片 2016-6-13 王耀发 创建
                if (result.Status)
                {
                    if (!string.IsNullOrEmpty(attributeOptionImageList))
                    {
                        string[] ImageListArray = attributeOptionImageList.Split('|');
                        foreach (var ImageList in ImageListArray)
                        {
                            string[] ImageArray = ImageList.Split(';');
                            foreach (var Image in ImageArray)
                            {
                                string[] ImageOptionArray = Image.Split(',');
                                var productsysno = ImageOptionArray[0];
                                var attributeoptionsysno = ImageOptionArray[1];
                                var isimage = ImageOptionArray[2];
                                var imgsrc = ImageOptionArray[3];
                                if (String.IsNullOrEmpty(imgsrc))
                                    imgsrc = null;

                                PdProductAttributeOption Entity = new PdProductAttributeOption();
                                Entity.ProductSysNo = int.Parse(productsysno);
                                Entity.AttributeOptionSysNo = int.Parse(attributeoptionsysno);
                                Entity.IsImage = int.Parse(isimage);
                                if (isimage != "0")
                                {
                                    Entity.AttributeOptionImage = imgsrc;

                                }
                                else
                                {
                                    Entity.AttributeOptionImage = null;
                                }
                                Entity.LastUpdateBy = CurrentUser.Base.SysNo;
                                Entity.LastUpdateDate = DateTime.Now;
                                PdProductAttributeOption Data = PdProductAttributeOptionBo.Instance.GetByProOptionSysNo(int.Parse(productsysno), int.Parse(attributeoptionsysno));
                                if (Data != null)
                                {
                                    Entity.SysNo = Data.SysNo;
                                    Entity.CreatedBy = Data.CreatedBy;
                                    Entity.CreatedDate = Data.CreatedDate;
                                    PdProductAttributeOptionBo.Instance.Update(Entity);
                                }
                                else
                                {
                                    Entity.CreatedBy = CurrentUser.Base.SysNo;
                                    Entity.CreatedDate = DateTime.Now;
                                    PdProductAttributeOptionBo.Instance.Insert(Entity);
                                }
                            }
                        }
                    }
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 商品描述文本编辑器页面
        /// </summary>
        /// <returns>商品描述编辑器视图</returns>
        /// <remarks>2013-07-29 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductDescriptionEditor()
        {
            return View("_ProductDescriptionEditor");
        }

        /// <summary>
        /// 更新商品描述文档
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="html">商品描述</param>
        /// <returns>返回更新结果对象</returns>
        /// <remarks>2013-07-25 邵斌 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult UpdateProductDescription(int productSysNo, string html)
        {
            Result result = new Result()
            {
                Status = true
            };
            //判断商品系统编号是否有效
            if (productSysNo == 0)
            {
                result.Status = false;
                result.Message = "请正确选择商品";
            }
            else
            {

                //更新商品描述
                result.Status = PdProductBo.Instance.UpdateProductDescription(productSysNo, html);
                try {
                    //同步更新B2B平台
                    PdProductBo.Instance.UpdateB2BProductDescription(productSysNo, html);
                }
                catch(Exception e)
                {
                    ;
                }
                //如果不成功就设置失败信息，如果成功就提交事务
                if (!result.Status)
                {
                    result.Message = "更新失败";
                }

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新商品价格
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult UpdateProductPriceInfo(int? sysNo)
        {
            //价格对象动态列表
            IList<dynamic> priceList = null;

            //如果有系统标号，表示是修改商品所以要读取原正式价格，否则值读取价格的框架集各种价格类型
            if (sysNo.HasValue)
            {
                //根据商品编号读取价格
                priceList = GetProductPriceAllInfo(sysNo.Value);

                //如果商品价格列表为0表示可能这个商品没有设置过价格,将价格列表设置为null以便下面初始化价格列表
                if (priceList.Count == 0)
                    priceList = null;
            }

            if (priceList == null)
            {
                //读取所以类型的价格类型列表
                IList<PdPriceType> typeList = PdPriceBo.Instance.GetPriceTypeItems();

                //同步有价格的商品对象类型以便下面处理
                var query = from t in typeList
                            select new
                            {
                                priceTypeSysNo = t.PriceSource,
                                priceSourceSysNo = t.SourceSysNo,
                                priceType = ((ProductStatus.产品价格来源)t.PriceSource).ToString(),
                                priceName = t.TypeName,
                                price = 0,
                                priceSysNo = 0,
                                status = (int)ProductStatus.产品价格状态.无效
                            };
                //返回结果集
                priceList = query.ToList<dynamic>();

            }

            //商品结果结果集
            var prices = new List<dynamic>();
            dynamic obj;
            //为了在前台能够使用动态类型所以要将linq生产的匿名类用真正的dynamic对象来包装一次，否则前台无法使用动态类
            foreach (var o in priceList)
            {
                obj = new ExpandoObject();
                obj.priceTypeSysNo = o.priceTypeSysNo;
                obj.priceSourceSysNo = o.priceSourceSysNo;
                obj.priceType = o.priceType;
                obj.priceName = o.priceName;
                obj.price = o.price;
                obj.isAdded = false;
                obj.priceSysNo = o.priceSysNo;
                obj.status = o.status;

                prices.Add(obj);
            }

            //价格类型结果集
            var categorys = new List<dynamic>();

            //为了在前台能够使用动态类型所以要将linq生产的匿名类用真正的dynamic对象来包装一次，否则前台无法使用动态类
            var categoryList = (from p in prices
                                group p by p.priceType
                                    into pg
                                    select new { pg }).ToList();

            foreach (var o in categoryList)
            {
                obj = new ExpandoObject();
                obj.priceType = o.pg.Key;
                categorys.Add(obj);
            }

            //返回对象
            dynamic Result = new ExpandoObject();
            Result.Prices = prices;
            Result.Categorys = categorys;

            return PartialView("_UpdateProductPriceInfo", Result);
        }

        /// <summary>
        /// 创建新商品
        /// </summary>
        /// <param name="productModel">商品模型</param>
        /// <param name="categoryAssociationList">待添加的商品分类关联关系</param>
        /// <param name="deleteCategoryAssociationList">待删除商品分类关联关系</param>
        /// <param name="masterCategorySysNo">新主商品分类系统编号</param>
        /// <param name="stamp">时间戳</param>
        /// <param name="PdProductSpec">商品规格  2017/9/1 吴琨添加</param>
        /// <returns>返回创建结果和新商品系统编号</returns>
        /// <remarks>2013-07-19 邵斌 创建</remarks>
        ///  2017/9/1 吴琨修改 增加PdProductSpec 参数 用于接收商品规格
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult UpdateProduct(PdProduct productModel, IList<PdCategoryAssociation> categoryAssociationList,
                                        IList<int> deleteCategoryAssociationList, int masterCategorySysNo, string stamp, IList<PdProductSpecValues> PdProductSpec)
        {

            var oldProduct = BLL.Product.PdProductBo.Instance.GetProductNoCache(productModel.SysNo);

            oldProduct = oldProduct ?? new CBPdProduct();

            //初始化环境
            StringBuilder msg = new StringBuilder();
            Result result = new Result()
            {
                Status = false,
                StatusCode = 0
            };

            //初始化部分特殊字段
            oldProduct.BrandSysNo = productModel.BrandSysNo;
            oldProduct.ProductType = productModel.ProductType;
            oldProduct.ErpCode = productModel.ErpCode.Trim();
            oldProduct.EasName = productModel.EasName;
            oldProduct.OriginSysNo = productModel.OriginSysNo;
            oldProduct.GrosWeight = productModel.GrosWeight;
            oldProduct.NetWeight = productModel.NetWeight;
            oldProduct.SalesMeasurementUnit = productModel.SalesMeasurementUnit;
            oldProduct.ProductDeclare = productModel.ProductDeclare;
            oldProduct.SalesAddress = productModel.SalesAddress;
            oldProduct.Barcode = productModel.Barcode;
            oldProduct.QRCode = productModel.QRCode;
            oldProduct.ProductName = productModel.ProductName.Trim();
            oldProduct.ValueUnit = productModel.ValueUnit;
            oldProduct.Volume = productModel.Volume;
            oldProduct.VolumeUnit = productModel.VolumeUnit;
            oldProduct.Tax = productModel.Tax;
            oldProduct.VolumeValue = productModel.VolumeValue;
            oldProduct.Rate = productModel.Rate;
            oldProduct.Purchasing = productModel.Purchasing;
            //格格家的参数
            oldProduct.GeGeJiaSpec = productModel.GeGeJiaSpec == 0 ? 1 : productModel.GeGeJiaSpec;
            oldProduct.GeGeJiaSupplyPrice = productModel.GeGeJiaSupplyPrice;
            oldProduct.GeGeJiaSupplyPriceSpecTwo = productModel.GeGeJiaSupplyPriceSpecTwo;
            oldProduct.GeGeJiaSupplyPriceSpecThree = productModel.GeGeJiaSupplyPriceSpecThree;
            if (String.IsNullOrEmpty(productModel.FreightFlag))
            {
                oldProduct.FreightFlag = "N";
            }
            else
            {
                oldProduct.FreightFlag = productModel.FreightFlag;
            }
            if (String.IsNullOrEmpty(productModel.Freight.ToString()))
            {
                oldProduct.Freight = 0;
            }
            else
            {
                oldProduct.Freight = productModel.Freight;
            }

            oldProduct.PriceRate = productModel.PriceRate;
            oldProduct.PriceValue = productModel.PriceValue;
            oldProduct.DealerPriceValue = productModel.DealerPriceValue;
            oldProduct.CostPrice = productModel.CostPrice;
            oldProduct.TradePrice = productModel.TradePrice;
            try
            {
                oldProduct.NameAcronymy = Hyt.Util.CHS2PinYin.Convert(oldProduct.ProductName, true);
                oldProduct.NameAcronymy = oldProduct.NameAcronymy.Length > 600
                                              ? oldProduct.NameAcronymy.Substring(0, 600)
                                              : oldProduct.NameAcronymy;
            }
            catch
            {
                ;
            }

            oldProduct.ProductSubName = productModel.ProductSubName;
            oldProduct.ProductShortTitle = productModel.ProductShortTitle;
            oldProduct.ProductSlogan = productModel.ProductSlogan;
            oldProduct.ProductSummary = string.IsNullOrEmpty(productModel.ProductSummary)
                                            ? ""
                                            : productModel.ProductSummary.Trim();
            oldProduct.PackageDesc = string.IsNullOrWhiteSpace(productModel.PackageDesc) ? "" : productModel.PackageDesc.Trim();
            oldProduct.CanFrontEndOrder = productModel.CanFrontEndOrder;
            oldProduct.IsFrontDisplay = productModel.IsFrontDisplay;

            oldProduct.AgentSysNo = productModel.AgentSysNo;
            oldProduct.DealerSysNo = productModel.DealerSysNo;

            oldProduct.Stamp = string.IsNullOrWhiteSpace(stamp) ? DateTime.Now : new DateTime(long.Parse(stamp));           //转换时间戳为日期

            productModel = oldProduct;                   //完成数据交换

            result = VaildProductBaseInfo(productModel); //验证字段值

            //定义商品运费model
            //PdProductFreight pfmodel = new PdProductFreight();
            //string[] pfArray = moduleList.Split(',');

            //如果字段检查通过将进行入库操作
            if (result.Status)
            {

                //如果商品系统编号大于0表示是修改商品，否则就是添加商品。
                if (productModel.SysNo > 0)
                {
                    //更新商品信息
                    result = PdProductBo.Instance.Update(productModel, categoryAssociationList,
                                                         deleteCategoryAssociationList, masterCategorySysNo);
                    #region 更新规格
                    if (PdProductSpec != null && PdProductSpec.Count > 0)
                    {
                        var SpecValues = JsonHelper.ListToJson<PdProductSpecValues>(PdProductSpec);
                        var SpecModel = PdProductSpecBo.Instance.IsPdProductSpec(productModel.SysNo);
                        var addpd = new PdProductSpec()
                        {
                            ProductSysNo = productModel.SysNo,
                            SpecValues = SpecValues
                        };
                        if (SpecModel != null) //存在就修改 
                        {
                            PdProductSpecBo.Instance.UpdPdProductSpec(addpd);
                        }
                        else
                        {
                            PdProductSpecBo.Instance.AddPdProductSpec(addpd);
                        }
                    }
                    #endregion

                    //保存前删除商品原有的运费模板

                    //保存商品运费信息                       
                    //pfmodel.PdProductSysNo = productModel.SysNo;
                    //if (pfArray[0] == "")  //对应模板SysNo为空
                    //{
                    //    pfmodel.Freigh = decimal.Parse(pfArray[1]);
                    //    pfmodel.LgFreightModuleSysNo = null;
                    //    pfmodel.ValuationValue = null;
                    //}
                    //else  //对应模板SysNo不为空
                    //{
                    //    pfmodel.Freigh = null;
                    //    pfmodel.LgFreightModuleSysNo = int.Parse(pfArray[0]);
                    //    pfmodel.ValuationValue = decimal.Parse(pfArray[1]);
                    //}
                    //PdProductFreightBo.Instance.DeleteByPdProductSysNo(productModel.SysNo);
                    //PdProductFreightBo.Instance.SavePdProductFreight(pfmodel, CurrentUser.Base);

                }
                else
                {
                    //创建商品主表信息
                    productModel.SysNo = PdProductBo.Instance.Create(productModel, categoryAssociationList);
                    if (productModel.SysNo > 0)
                    {
                        //创建B2B商品主表信息
                        try
                        {
                            var pd = PdProductBo.Instance.GetB2BProductByErpCode(productModel.ErpCode);
                            if (pd != null && pd.SysNo > 0)
                            {
                                //B2B平台存在相同编号的商品无法创建
                            }
                            else
                            {
                                var brandB2C = BLL.Product.PdBrandBo.Instance.GetEntity(productModel.BrandSysNo);
                                var brandB2B = BLL.Product.PdBrandBo.Instance.GetB2BEntityByName(brandB2C!=null?brandB2C.Name:"");
                                if (brandB2B==null)
                                {
                                   brandB2C.SysNo= IPdBrandDao.Instance.CreateToB2B(brandB2C);
                                }
                                else
                                {
                                    brandB2C.SysNo = brandB2B.SysNo;
                                }
                                PdProduct b2bModel = new PdProduct()
                                {
                                    BrandSysNo = brandB2C.SysNo,
                                    ProductType = productModel.ProductType,
                                    ErpCode = productModel.ErpCode.Trim(),
                                    EasName = productModel.EasName,
                                    OriginSysNo = productModel.OriginSysNo,
                                    GrosWeight = productModel.GrosWeight,
                                    NetWeight = productModel.NetWeight,
                                    SalesMeasurementUnit = productModel.SalesMeasurementUnit,
                                    ProductDeclare = productModel.ProductDeclare,
                                    SalesAddress = productModel.SalesAddress,
                                    Barcode = productModel.Barcode,
                                    QRCode = productModel.QRCode,
                                    ProductName = productModel.ProductName.Trim(),
                                    ValueUnit = productModel.ValueUnit,
                                    Volume = productModel.Volume,
                                    VolumeUnit = productModel.VolumeUnit,
                                    Tax = productModel.Tax,
                                    VolumeValue = productModel.VolumeValue,
                                    Rate = productModel.Rate,
                                    Purchasing = productModel.Purchasing,
                                    FreightFlag = oldProduct.FreightFlag,
                                    Freight = oldProduct.Freight,
                                    PriceRate = oldProduct.PriceRate,
                                    PriceValue = oldProduct.PriceValue,
                                    DealerPriceValue = oldProduct.DealerPriceValue,
                                    CostPrice = oldProduct.CostPrice,
                                    TradePrice = oldProduct.TradePrice,
                                    NameAcronymy = oldProduct.NameAcronymy,
                                    ProductSubName = oldProduct.ProductSubName,
                                    ProductShortTitle = oldProduct.ProductShortTitle,
                                    ProductSlogan = oldProduct.ProductSlogan,
                                    ProductSummary = oldProduct.ProductSummary,
                                    PackageDesc = oldProduct.PackageDesc,
                                    CanFrontEndOrder = oldProduct.CanFrontEndOrder,
                                    IsFrontDisplay = oldProduct.IsFrontDisplay,
                                    AgentSysNo = oldProduct.AgentSysNo,
                                    DealerSysNo = oldProduct.DealerSysNo,
                                    Stamp = oldProduct.Stamp,
                                };
                                 b2bModel.SysNo = PdProductBo.Instance.CreateToB2B(b2bModel, categoryAssociationList);
                            }

                        }
                        catch (Exception e)
                        {
                            ;
                        }

                        #region 添加规格
                        if (PdProductSpec.Count > 0)
                        {
                            var SpecValues = JsonHelper.ListToJson<PdProductSpecValues>(PdProductSpec);
                            var addpd = new PdProductSpec()
                            {
                                ProductSysNo = productModel.SysNo,
                                SpecValues = SpecValues
                            };
                            PdProductSpecBo.Instance.AddPdProductSpec(addpd);
                            //创建B2B规格信息
                            try
                            {
                                //PdProductSpecBo.Instance.AddPdProductSpecToB2B(addpd);
                            }
                            catch (Exception e)
                            {
                                ;
                            }
                        }
                        #endregion

                        result.Status = true;
                        //保存商品运费信息                       
                        //pfmodel.PdProductSysNo = productModel.SysNo;
                        //if (pfArray[0] == "")  //对应模板SysNo为空
                        //{
                        //    pfmodel.Freigh = decimal.Parse(pfArray[1]);
                        //    pfmodel.LgFreightModuleSysNo = null;
                        //    pfmodel.ValuationValue = null;
                        //}
                        //else  //对应模板SysNo不为空
                        //{
                        //    pfmodel.Freigh = null;
                        //    pfmodel.LgFreightModuleSysNo = int.Parse(pfArray[0]);
                        //    pfmodel.ValuationValue = decimal.Parse(pfArray[1]);
                        //}
                        //PdProductFreightBo.Instance.SavePdProductFreight(pfmodel, CurrentUser.Base);
                    }
                    else
                    {
                        result.Status = false;
                    }
                }

            }
            CBPdProduct newProduct = PdProductBo.Instance.GetProductNoCache(productModel.SysNo);
            return Json(new { result = result, product = newProduct, stamp = newProduct.Stamp.Ticks.ToString() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取商品运费信息
        /// <param name="PdProductSysNo">商品编号</param>
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-08-14 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult GetEntityByPdProductSysNo()
        {
            int PdProductSysNo = int.Parse(this.Request["PdProductSysNo"]);
            PdProductFreight data = PdProductFreightBo.Instance.GetEntityByPdProductSysNo(PdProductSysNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 商品维护是申请调价
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <param name="prices">商品价格集合</param>
        /// <returns></returns>
        /// <remarks>通过对比原价和申请价格两个集合来找出有变动的价格来进行有针对性的调整</remarks>
        /// <remarks>2013-07-19 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001202)]
        public JsonResult UpdateProductPrice(int sysNo, IList<PdPrice> prices)
        {
            //返回结果集
            Result result = new Result()
            {
                Status = true,
                StatusCode = 0,
                Message = ""
            };


            result = PdPriceBo.Instance.UpdateProductPrice(sysNo, prices);



            return Json(new { Result = result, Prices = prices }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 读取指定商品主表信息，如果id为空就视为新建对象
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <returns>返回商品模型</returns>
        /// <remarks>2013-07-16 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetUpdateProductMainInfo(int? id)
        {
            CBPdProduct product = new CBPdProduct();
            if (id.HasValue)
            {
                product = PdProductBo.Instance.GetProductNoCache(id.Value);
            }
            return Json(new { Product = product, Stamp = product.Stamp.Ticks.ToString() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 读取单个商品全部价格信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品价格信息列表</returns>
        /// <remarks>2013-07-19 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public IList<dynamic> GetProductPriceAllInfo(int productSysNo)
        {
            IList<PdPriceType> typeList = PdPriceBo.Instance.GetPriceTypeItems(); //读取价格类型
            IList<PdPrice> prices = PdPriceBo.Instance.GetProductPriceByStatus(productSysNo); //读取商品的价格

            //将价格和价格类型组合成用于显示的数据结构，及包含价格和价格类型名称
            var query = from p in typeList
                        join pt in prices on new { p.PriceSource, p.SourceSysNo } equals new { pt.PriceSource, pt.SourceSysNo } into g
                        from x in g.DefaultIfEmpty()
                        orderby p.PriceSource
                        select new
                        {
                            priceTypeSysNo = p.PriceSource,
                            priceSourceSysNo = p.SourceSysNo,
                            priceType = ((ProductStatus.产品价格来源)p.PriceSource).ToString(),
                            priceName = p.TypeName,
                            price = (x != null) ? x.Price : 0,
                            priceSysNo = (x != null) ? x.SysNo : 0,
                            status = (x != null) ? x.Status : 0
                        };

            //返回动态价格类型
            return query.ToList<dynamic>();
        }

        /// <summary>
        /// 通过商品Id获取商品的关联列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品集合</returns>
        /// <returns>2013-07-23 邵斌 创建</returns>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetProductAssociationProductList(int productSysNo)
        {
            //读取关联商品
            IList<CBProductAssociation> list = PdProductAssociationBo.Instance.ProductList(productSysNo);

            //结果对象
            var result = new List<dynamic>();
            IList<int> productSysNoList = new List<int>();  //商品系统编号列表，该列表将用于筛选管理属性

            //遍历结果关联商品
            foreach (var item in list)
            {
                //如果管理商品已经存在结果集中将不在添加避免重复添加商品到结果集中
                if (result.Where(r => r.pid == item.ProductSysNo).FirstOrDefault() == null && item.ProductSysNo != productSysNo)
                {
                    result.Add(new
                    {
                        pid = item.ProductSysNo,
                        name = item.ProductName,
                        erp = item.ERPCode,
                        isfrontdisplay = item.IsFrontDisplay,
                        attributes = list.Where(l => l.ProductSysNo == item.ProductSysNo).Select(l => l).ToList()   //找打该商品的所以关联属性值
                    });
                    productSysNoList.Add(item.ProductSysNo);
                }
            }

            //如果结果没有数据，将本商品添加到结果集中，用于筛选属性
            if (productSysNoList.Count == 0)
            {
                productSysNoList.Add(productSysNo);
            }

            //获取用于关联商品的关联属性
            IList<PdAttribute> attributes = PdAttributeBo.Instance.GetProductAssociationAttribute(productSysNoList.ToArray());

            return Json(new { result = result, attributes = attributes, relationcode = PdProductAssociationBo.Instance.GetRelationCode(productSysNo) ?? "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存商品关联列表
        /// </summary>
        /// <param name="mainProductSysNo">主商品系统编号</param>
        /// <param name="productSysNoList">商品关联系统编号列表</param>
        /// <param name="relationCode">管理关系码</param>
        /// <returns></returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult SaveProductAssociation(int mainProductSysNo, IList<int> productSysNoList, string relationCode = null)
        {
            bool result = true;

            //如果关联商品列表为空，表示是清除所以关联商品
            if (productSysNoList == null)
            {
                PdProductAssociationBo.Instance.Clear(mainProductSysNo);
            }
            else
            {
                productSysNoList.Add(mainProductSysNo);
                result = PdProductAssociationBo.Instance.Create(mainProductSysNo, productSysNoList.ToArray(), relationCode);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新单个商品价格状态
        /// </summary>
        /// <param name="priceSysNo">商品价格系统编号</param>
        /// <param name="status">商品状态</param>
        /// <returns>返回更新结果JSON数据</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult UpdatePriceStatus(int priceSysNo, int status)
        {
            Result result = new Result();
            ProductStatus.产品价格状态 changeStatus = ProductStatus.产品价格状态.无效;

            //验证转换状态是否能被正确转换
            try
            {
                changeStatus = (ProductStatus.产品价格状态)status;
                result.Status = true;
            }
            catch
            {
                result.Status = false;
                result.Message = "要改变的状态无效";
            }

            //如果转换正确，将进行更新
            if (result.Status && priceSysNo > 0)
            {
                result = PdPriceBo.Instance.UpdatePriceStatus(priceSysNo, changeStatus);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据商品系统编号获取商品关联关系吗
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回关联关系码</returns> 
        /// <remarks>2013-08-20 邵斌 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetRelationCode(int productSysNo)
        {

            //读取关联商品
            IList<CBProductAssociation> list = PdProductAssociationBo.Instance.ProductList(productSysNo);
            var result = new List<dynamic>();

            //遍历结果关联商品
            foreach (var item in list)
            {
                //如果管理商品已经存在结果集中将不在添加避免重复添加商品到结果集中
                if (result.Where(r => r.pid == item.ProductSysNo).FirstOrDefault() == null && item.ProductSysNo != productSysNo)
                {
                    result.Add(new
                    {
                        pid = item.ProductSysNo,
                        name = item.ProductName,
                        erp = item.ERPCode,
                        attributes = list.Where(l => l.ProductSysNo == item.ProductSysNo).Select(l => l).ToList()   //找打该商品的所以关联属性值
                    });
                }
            }

            return Json(new { RelationCode = PdProductAssociationBo.Instance.GetRelationCode(productSysNo), products = result }, JsonRequestBehavior.AllowGet);
        }

        #region 商品基本信息有效验证

        /// <summary>
        /// 验证商品信息字段有效性
        /// </summary>
        /// <param name="product">商品对象</param>
        /// <returns>返回 true:数据有效 false：数据无效</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201, PrivilegeCode.PD1001001)]
        private Result VaildProductBaseInfo(PdProduct product)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = 0
            };

            //检查商品编号
            if (result.Status && !PdProductBo.Instance.CheckERPCode(product.ErpCode, product.SysNo))
            {
                result.Status = false;
                result.Message = "商品编号已经被其他商品使用";
            }

            ////检查条形码
            //if (result.Status && !PdProductBo.Instance.CheckBarCode(product.Barcode, product.SysNo))
            //{
            //    result.Status = false;
            //    result.Message = "条形码已经被其他商品使用";
            //}

            //检查二维码（二维码不为空是才进行检查）
            if (result.Status && (!string.IsNullOrEmpty(product.QRCode) && !PdProductBo.Instance.CheckQRCode(product.QRCode.Trim(), product.SysNo)))
            {
                result.Status = false;
                result.Message = "二维码已经被其他商品使用";
            }

            return result;
        }

        /// <summary>
        /// 检查商品编号
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:能用， false:不能用</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201, PrivilegeCode.PD1001001)]
        public JsonResult CheckERPCode(string erpCode, int productSysNo)
        {
            //string erpCode = Request["erpCode"].ToString();
            //int productSysNo = Request["productSysNo"]==null?0: int.Parse(Request["productSysNo"].ToString());
            return Json(PdProductBo.Instance.CheckERPCode(erpCode.Trim(), productSysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查条形码
        /// </summary>
        /// <param name="barcode">条形码</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:能用， false:不能用</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201, PrivilegeCode.PD1001001)]
        public JsonResult CheckBarCode(string barcode, int productSysNo)
        {
            return Json(PdProductBo.Instance.CheckBarCode(barcode.Trim(), productSysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查二维码
        /// </summary>
        /// <param name="qrCode">二维码</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:能用， false:不能用</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201, PrivilegeCode.PD1001001)]
        public JsonResult CheckQRCode(string qrCode, int productSysNo)
        {
            return Json(PdProductBo.Instance.CheckQRCode(qrCode.Trim(), productSysNo), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 搭配销售商品

        /// <summary>
        /// 根据商品分类获取属性
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        /// <remarks>2013-07-22 邵  斌 修改</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult PdProductCollocation(int productSysNo)
        {
            IList<CBProductListItem> list = new List<CBProductListItem>();
            list = BLL.Product.PdProductCollocationBo.Instance.GetList(productSysNo);

            var result = from p in list
                         select
                             new
                             {
                                 pid = p.SysNo,
                                 erp = p.ErpCode,
                                 name = p.ProductName,
                                 baseprice = p.Price,
                                 categoryName = p.CategoryName
                             };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 添加搭配商品
        /// </summary>
        /// <param name="collocation">商品编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public ActionResult PdProductCollocationAdd(IList<PdProductCollocation> collocation)
        {
            bool result = false;
            string info = "操作失败";
            int productSysno = Convert.ToInt32(Request.Params["productSysno"]);
            if (PdProductCollocationBo.Instance.Insert(collocation, productSysno) > 0)
            {
                result = true;
                info = "添加搭配商品成功！";
            }
            return Json(new { result = result, info = info });
        }

        #endregion

        #region 产品图片

        /// <summary>
        /// 产品图片列表
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>产品图片列表</returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductImagesInfo(int productSysNo)
        {
            ProductImageConfig imageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig();
            AttachmentConfig attrConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();

            ViewBag.BaseFolder = imageConfig.BaseFolder;
            ViewBag.ImageServer = attrConfig.FileServer;

            IList<PdProductImage> list = new List<PdProductImage>();
            list = BLL.Product.PdProductImageBo.Instance.GetProductImg(productSysNo);
            return PartialView("_AjaxProductImagesInfo", list);
        }

        /// <summary>
        /// 设置图片排序
        /// </summary>
        /// <param name="images">图片数</param>
        /// <returns></returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductImagesInfoDisplayOrder(IList<PdProductImage> images)
        {
            if (Hyt.BLL.Product.PdProductImageBo.Instance.DisplayOrder(images))
                return Json(new { result = true, info = " 操作成功！" });
            else
                return Json(new { result = false, info = "操作失败！" });

        }

        /// <summary>
        /// 批量保存商品图片
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductImagesInfoAdd(IList<PdProductImage> images)
        {

            bool result = false;
            string info = "操作失败";
            result = BLL.Product.PdProductImageBo.Instance.AddProductImage(images);
            try {
                result = BLL.Product.PdProductImageBo.Instance.AddProductImageToB2B(images);
            }
            catch( Exception E)
            {
                Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品添加图片到B2B失败"), LogStatus.系统日志目标类型.商品图片, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return Json(new { result = result, info = "保存图片成功" });
        }

        /// <summary>
        /// 设置封面
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductImagesInfoSetHome(IList<PdProductImage> images)
        {
            Result result = new Result();

            //产品图片编号
            int sysNo = images[0].SysNo;
            //产品图片名称
            string fileName = images[0].ImageUrl;
            //产品编号
            int productSysNo = images[0].ProductSysNo;

            var status = true;
            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.FileProcessor.IThumbnailService>())
            {
                status = service.Channel.ProductThumbnailProcessor(fileName.Substring(fileName.LastIndexOf('/') + 1), productSysNo);
            }

            if (status)
            {
                if (productSysNo > 0)
                {
                    PdProductImageBo.Instance.UpdateStatus(productSysNo, (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.隐藏);
                    var productIamge = PdProductImageBo.Instance.GetModel(sysNo);
                    productIamge.Status = (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.显示;
                    PdProductImageBo.Instance.Update(productIamge);

                    result.Status = true;
                    result.Message = "设置商品封面成功！";
                }
                else
                {
                    result.Status = false;
                    result.Message = "设置商品封面失败！";
                }
            }
            else
            {
                result.Status = false;
                result.Message = "设置商品封面失败！";
            }

            return Json(new { result });
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="images">图片实体</param>
        /// <param name="productImageSysNo">图片编号</param>
        /// <returns>返回成功、失败</returns>
        /// <remarks>2013-07-22 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductImagesInfoDel(IList<PdProductImage> images, int productImageSysNo)
        {
            bool result = false;
            string info = "操作失败";
            result = BLL.Product.PdProductImageBo.Instance.Delete(images, productImageSysNo);
            if (result)
            {
                info = "操作成功！";
            }
            return Json(new { result = result, info = info });
        }

        #endregion

        #region 商品描述模板

        /// <summary>
        /// 商品描述模板视图
        /// </summary>
        /// <param name="id">起始页数</param>
        /// <param name="type">商品描述模板类型</param>
        /// <param name="searchName">商品描述模板名称</param>
        /// <returns>商品描述模板视图</returns>
        /// <remarks>2013-07-22 杨晗 修改</remarks>
        [Privilege(PrivilegeCode.PD1006001)]
        public ActionResult PdTemplate(int? id, ProductStatus.商品描述模板类型? type, string searchName = null)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ProductStatus.商品描述模板类型));
            ViewBag.DictList = dictList;

            var model = PdTemplateBo.Instance.Seach(id, type, searchName);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxPdTemplate", model);
            }
            return View(model);
        }

        /// <summary>
        /// 商品描述模板增加修改视图
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <param name="type">模板类型</param>
        /// <returns>商品描述模板增加修改视图</returns>
        /// <remarks>2013-07-22 杨晗 修改</remarks>
        [Privilege(PrivilegeCode.PD1006201)]
        public ActionResult PdTemplateAddOrEdit(int? sysNo, int? type)
        {
            type = type ?? (int)ProductStatus.商品描述模板类型.模块;
            var model = new PdTemplate();
            if (sysNo != null)
            {
                model = PdTemplateBo.Instance.GetModel((int)sysNo);
                type = model.Type;
            }
            ViewBag.Type = type;
            return View(model);
        }

        /// <summary>
        /// 商品描述模板增加修改
        /// </summary>
        /// <returns>商品描述模板增加修改的成功或失败信息</returns>
        /// <remarks>2013-07-22 杨晗 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1006201)]
        public ActionResult PdTemplateAddOrEdit()
        {
            bool bl;
            string hidSysNo = Request.Form["hidSysNo"];
            string type = Request.Form["searchType"];
            string txtName = Request.Form["txtName"];
            string txtIcon = Request.Form["txtIcon"];
            string txtRemark = Request.Form["txtRemark"];
            string txtContent = Request.Form["txtContent"];
            var model = new PdTemplate();
            if (!string.IsNullOrEmpty(hidSysNo) && hidSysNo != "0")
            {
                model = PdTemplateBo.Instance.GetModel(Convert.ToInt32(hidSysNo));
            }
            else
            {
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
            }
            model.Type = Convert.ToInt32(type);
            model.Name = txtName;
            model.Icon = txtIcon;
            model.Remark = txtRemark;
            model.Content = txtContent;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            if (!string.IsNullOrEmpty(hidSysNo) && hidSysNo != "0")
            {
                int u = PdTemplateBo.Instance.Update(model);
                bl = u > 0;
            }
            else
            {
                int i = PdTemplateBo.Instance.Insert(model);
                bl = i > 0;
            }
            return Json(new { IsPass = bl });
        }

        /// <summary>
        /// 商品描述模板验证是否重复
        /// </summary>
        /// <returns>重复为false,否则为true</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.PD1006201)]
        public ActionResult PdTemplateVerify()
        {
            var txtName = Request.Form["txtName"];
            var sysNo = Request.Form["hidSysNo"];
            bool bl;
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                var model = PdTemplateBo.Instance.GetModel(Convert.ToInt32(sysNo));
                bl = txtName != model.Name && PdTemplateBo.Instance.PdTemplateVerify(txtName);
            }
            else
            {
                bl = PdTemplateBo.Instance.PdTemplateVerify(txtName);
            }
            return Json(!bl, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除商品描述模板
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>删除成功或失败信息</returns>
        /// <remarks>2013-07-23 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.PD1006401)]
        public ActionResult PdTemplateDelete(int sysNo)
        {
            var bl = PdTemplateBo.Instance.Delete(sysNo);
            return Json(new { IsPass = bl });
        }

        /// <summary>
        /// 选择商品模版
        /// </summary>
        /// <param name="id">起始页</param>
        /// <param name="type">类型</param>
        /// <param name="searchName">模板名称</param>
        /// <returns>被选择的列表</returns>
        /// <remarks>2013-07-24 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.CM1010005)]
        public ActionResult SelectPdTemplate(int? id, ProductStatus.商品描述模板类型 type, string searchName = null)
        {
            ViewBag.Type = (int)type;
            var model = PdTemplateBo.Instance.Seach(id, type, searchName);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxSelectPdTemplate", model);
            }
            return View(model);
        }

        #endregion

        #region 商品私人定制
        /// <summary>
        /// 商品私人定制列表
        /// </summary>
        /// <returns>分页</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1006503)]
        public ActionResult ProductPrivateList()
        {
            return View();
        }

        [Privilege(PrivilegeCode.PD1006503)]
        public ActionResult DoPdProductPrivateQuery(ParaProductPrivateFilter filter)
        {
            filter.PageSize = 10;
            var pager = PdProductPrivateBo.Instance.GetPdProductPrivateList(filter);
            var list = new PagedList<PdProductPrivateList>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PdProductPrivateListPager", list);
        }

        /// <summary>
        /// 商品私人定制编辑
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1006503)]
        public ActionResult ProductPrivateCreate(int? id)
        {
            PdProductPrivateList model;
            if (id.HasValue)
            {
                model = PdProductPrivateBo.Instance.GetEntity(id.Value);
            }
            else
            {
                model = new PdProductPrivateList();
            }
            return View(model);
        }
        /// <summary>
        /// 获得商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>2015-09-09 王耀发 创建</returns>
        [Privilege(PrivilegeCode.PD1001001)]
        public JsonResult GetProduct()
        {
            int SysNo = int.Parse(this.Request["SysNo"]);
            CBPdProduct model = PdProductBo.Instance.GetProduct(SysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核商品私人定制
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1006503)]
        public ActionResult AuditProductPrivate(int id)
        {
            string AuditOpinion = this.Request["AuditOpinion"];
            PdProductPrivate model = new PdProductPrivate();
            model.SysNo = id;
            model.Status = 1;
            model.AuditOpinion = AuditOpinion;
            Result r = new Result();
            try
            {
                r = PdProductPrivateBo.Instance.SavePdProductPrivate(model, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废商品私人定制
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1006503)]
        public ActionResult CancelProductPrivate(int id)
        {
            string AuditOpinion = this.Request["AuditOpinion"];
            PdProductPrivate model = new PdProductPrivate();
            model.SysNo = id;
            model.Status = -1;
            model.AuditOpinion = AuditOpinion;
            Result r = new Result();
            try
            {
                r = PdProductPrivateBo.Instance.SavePdProductPrivate(model, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除作废商品私人定制
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1006503)]
        public JsonResult DeleteProductPrivate(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    result = PdProductPrivateBo.Instance.Delete(id);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商品
        /// </summary>
        /// <param></param>
        /// <returns>展示商品模型</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult ProductSelector()
        {
            return View();
        }
        /// <summary>
        /// 商品
        /// </summary>
        /// <param></param>
        /// <returns>展示商品模型</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public ActionResult DealerMallProductSelector()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// 更新商品前台显示字段
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="IsFrontDisplay">前台显示字段</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2015-12-24 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult UpdateProductIsFrontDisplay(int productSysNo, int IsFrontDisplay)
        {
            Result result = new Result()
            {
                Status = true
            };
            //判断商品系统编号是否有效
            if (productSysNo == 0)
            {
                result.Status = false;
                result.Message = "请正确选择商品";
            }
            else
            {

                //更新商品前台显示
                result.Status = PdProductBo.Instance.UpdateProductIsFrontDisplay(productSysNo, IsFrontDisplay);

                //如果不成功就设置失败信息，如果成功就提交事务
                if (!result.Status)
                {
                    result.Message = "更新失败";
                }

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region 批量同步商品到B2B
        /// <summary>
        /// 批量同步商品到B2B
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public JsonResult SynProductsToB2B(
            string productSysNos,
            string ProductName,
            string Status,
            string CanFrontEndOrder,
            string IsFrontDisplay,
            string SelectedAgentSysNo,
            string SelectedDealerSysNo,
            string StartTime,
            string EndTime,
            string CreateStartTime,
            string CreateEndTime,
            string SysNo,
            string ProductType,
            string ErpCode,
            string Barcode,
            string ProductCategorySysno,
            string OriginSysNo
            )
        {
            Result result = new Result()
            {
                Status = true
            };
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(productSysNos) && productSysNos != "[]")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(productSysNos);
            }


            ParaProductFilter productDetail = new ParaProductFilter();

            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                productDetail.DealerSysNo = DealerSysNo;
                productDetail.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            productDetail.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            productDetail.DealerCreatedBy = CurrentUser.Base.SysNo;

            productDetail.ProductName = ProductName.Replace("undefined", "");
            productDetail.Status = Convert.ToInt32(Status);
            productDetail.CanFrontEndOrder = Convert.ToInt32(CanFrontEndOrder);
            productDetail.IsFrontDisplay = Convert.ToInt32(IsFrontDisplay);
            productDetail.SelectedAgentSysNo = Convert.ToInt32(SelectedAgentSysNo);
            productDetail.SelectedDealerSysNo = Convert.ToInt32(SelectedDealerSysNo);
            try
            {
                if (!string.IsNullOrEmpty(StartTime))
                {
                    productDetail.StartTime = Convert.ToDateTime(StartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(EndTime))
                {
                    productDetail.EndTime = Convert.ToDateTime(EndTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateStartTime))
                {
                    productDetail.CreateStartTime = Convert.ToDateTime(CreateStartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateEndTime))
                {
                    productDetail.CreateEndTime = Convert.ToDateTime(CreateEndTime);
                }
            }
            catch { }

            int checkSysNo = 0;
            int.TryParse(SysNo, out checkSysNo);

            productDetail.SysNo = Convert.ToInt32(checkSysNo);
            productDetail.ProductType = Convert.ToInt32(ProductType);
            if (!string.IsNullOrEmpty(ErpCode))
            {
                productDetail.ErpCode = ErpCode.Replace("undefined", "");
            }
            if (!string.IsNullOrEmpty(Barcode))
            {
                productDetail.Barcode = Barcode.Replace("undefined", "");
            }

            ///商品编号
            if (ProductCategorySysno == "NaN")
            {
                ProductCategorySysno = null;
            }
            productDetail.ProductCategorySysno = Convert.ToInt32(ProductCategorySysno);
            productDetail.OriginSysNo = Convert.ToInt32(OriginSysNo);
            //if (sysNos != null)
            //{
            PdProductBo.Instance.XinYingProductsToB2B(sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, productDetail);
            return Json(result, JsonRequestBehavior.AllowGet);
            //}
        }

        #endregion

        #region 导出商品

        /// <summary>
        /// 商品导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        /// <remarks>2017-3-3 杨云奕 修改导出</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public void ExportProducts(
            string productSysNos,
            string ProductName,
            string Status,
            string CanFrontEndOrder,
            string IsFrontDisplay,
            string SelectedAgentSysNo,
            string SelectedDealerSysNo,
            string StartTime,
            string EndTime,
            string CreateStartTime,
            string CreateEndTime,
            string SysNo,
            string ProductType,
            string ErpCode,
            string Barcode,
            string ProductCategorySysno,
            string OriginSysNo
            )
        {
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(productSysNos) && productSysNos != "[]")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(productSysNos);
            }


            ParaProductFilter productDetail = new ParaProductFilter();

            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                productDetail.DealerSysNo = DealerSysNo;
                productDetail.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            productDetail.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            productDetail.DealerCreatedBy = CurrentUser.Base.SysNo;

            productDetail.ProductName = ProductName.Replace("undefined", "");
            productDetail.Status = Convert.ToInt32(Status);
            productDetail.CanFrontEndOrder = Convert.ToInt32(CanFrontEndOrder);
            productDetail.IsFrontDisplay = Convert.ToInt32(IsFrontDisplay);
            productDetail.SelectedAgentSysNo = Convert.ToInt32(SelectedAgentSysNo);
            productDetail.SelectedDealerSysNo = Convert.ToInt32(SelectedDealerSysNo);
            try
            {
                if (!string.IsNullOrEmpty(StartTime))
                {
                    productDetail.StartTime = Convert.ToDateTime(StartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(EndTime))
                {
                    productDetail.EndTime = Convert.ToDateTime(EndTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateStartTime))
                {
                    productDetail.CreateStartTime = Convert.ToDateTime(CreateStartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateEndTime))
                {
                    productDetail.CreateEndTime = Convert.ToDateTime(CreateEndTime);
                }
            }
            catch { }

            int checkSysNo = 0;
            int.TryParse(SysNo, out checkSysNo);

            productDetail.SysNo = Convert.ToInt32(checkSysNo);
            productDetail.ProductType = Convert.ToInt32(ProductType);
            if (!string.IsNullOrEmpty(ErpCode))
            {
                productDetail.ErpCode = ErpCode.Replace("undefined", "");
            }
            if (!string.IsNullOrEmpty(Barcode))
            {
                productDetail.Barcode = Barcode.Replace("undefined", "");
            }

            ///商品编号
            if (ProductCategorySysno == "NaN")
            {
                ProductCategorySysno = null;
            }
            productDetail.ProductCategorySysno = Convert.ToInt32(ProductCategorySysno);
            productDetail.OriginSysNo = Convert.ToInt32(OriginSysNo);
            //if (sysNos != null)
            //{
            PdProductBo.Instance.XinYingExportProducts(sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, productDetail);
            //}
        }


        /// <summary>
        /// 商品导出接口规范
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-5-16 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001)]
        public void ExportProductsJiaXin(
            string productSysNos,
            string ProductName,
            string Status,
            string CanFrontEndOrder,
            string IsFrontDisplay,
            string SelectedAgentSysNo,
            string SelectedDealerSysNo,
            string StartTime,
            string EndTime,
            string CreateStartTime,
            string CreateEndTime,
            string SysNo,
            string ProductType,
            string ErpCode,
            string Barcode,
            string ProductCategorySysno,
            string OriginSysNo
            )
        {
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(productSysNos) && productSysNos != "[]")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(productSysNos);
            }


            ParaProductFilter productDetail = new ParaProductFilter();

            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                productDetail.DealerSysNo = DealerSysNo;
                productDetail.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            productDetail.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            productDetail.DealerCreatedBy = CurrentUser.Base.SysNo;

            productDetail.ProductName = ProductName.Replace("undefined", "");
            productDetail.Status = Convert.ToInt32(Status);
            productDetail.CanFrontEndOrder = Convert.ToInt32(CanFrontEndOrder);
            productDetail.IsFrontDisplay = Convert.ToInt32(IsFrontDisplay);
            productDetail.SelectedAgentSysNo = Convert.ToInt32(SelectedAgentSysNo);
            productDetail.SelectedDealerSysNo = Convert.ToInt32(SelectedDealerSysNo);
            try
            {
                if (!string.IsNullOrEmpty(StartTime))
                {
                    productDetail.StartTime = Convert.ToDateTime(StartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(EndTime))
                {
                    productDetail.EndTime = Convert.ToDateTime(EndTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateStartTime))
                {
                    productDetail.CreateStartTime = Convert.ToDateTime(CreateStartTime);
                }
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(CreateEndTime))
                {
                    productDetail.CreateEndTime = Convert.ToDateTime(CreateEndTime);
                }
            }
            catch { }

            int checkSysNo = 0;
            int.TryParse(SysNo, out checkSysNo);

            productDetail.SysNo = Convert.ToInt32(checkSysNo);
            productDetail.ProductType = Convert.ToInt32(ProductType);
            if (!string.IsNullOrEmpty(ErpCode))
            {
                productDetail.ErpCode = ErpCode.Replace("undefined", "");
            }
            if (!string.IsNullOrEmpty(Barcode))
            {
                productDetail.Barcode = Barcode.Replace("undefined", "");
            }

            ///商品编号
            if (ProductCategorySysno == "NaN")
            {
                ProductCategorySysno = null;
            }
            productDetail.ProductCategorySysno = Convert.ToInt32(ProductCategorySysno);
            productDetail.OriginSysNo = Convert.ToInt32(OriginSysNo);
            //if (sysNos != null)
            //{
            PdProductBo.Instance.ExportProductsLiJia(sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, productDetail);
            //}
        }
        //public void ExportProducts(string productSysNos = "")
        //{
        //    List<int> sysNos = null;
        //    if (!string.IsNullOrWhiteSpace(productSysNos))
        //    {
        //        sysNos = new JavaScriptSerializer().Deserialize<List<int>>(productSysNos);
        //    }

        //    //if (sysNos != null)
        //    //{
        //    PdProductBo.Instance.XinYingExportProducts(sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        //    //}
        //}

        #endregion

        /// <summary>
        /// 批量修改商品分类
        /// </summary>
        /// <param name="productSysNos"></param>
        /// <param name="selectedCategory"></param>
        /// <param name="deleteCategory"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.PD1001201)]
        public JsonResult BatchUpdateProductsCategories(string productSysNos, string selectedCategory, string deleteCategory)
        {
            List<int> productSysNoList = new List<int>();
            List<int> selectCateSysNoList = new List<int>();
            List<int> deleteCateSysNoList = new List<int>();
            if (!string.IsNullOrWhiteSpace(productSysNos))
            {
                productSysNoList = (from sysno in TConvert.ToString(productSysNos).Split(',').ToList() select TConvert.ToInt32(sysno)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(selectedCategory))
            {
                selectCateSysNoList = (from sysno in TConvert.ToString(selectedCategory).Split(',').ToList() select TConvert.ToInt32(sysno)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(deleteCategory))
            {
                deleteCateSysNoList = (from sysno in TConvert.ToString(deleteCategory).Split(',').ToList() select TConvert.ToInt32(sysno)).ToList();
            }
            if (productSysNoList.Count == 0 || (selectCateSysNoList.Count == 0 && deleteCateSysNoList.Count == 0))
            {
                return new JsonResult();
            }
            var result = BLL.Product.PdProductBo.Instance.BatchUpdateProductsCategories(productSysNoList, selectCateSysNoList, deleteCateSysNoList);
            return Json(result);
        }

        /// <summary>
        /// 商品备案信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2016-04-02 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public ActionResult ProductIcpInfo(int id)
        {
            ViewBag.ProductSysNo = id;
            return View("ProductIcpInfo");
        }
        /// <summary>
        /// 保存白云机场的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult UpdateBYJiChangIcpInfo(IcpBYJiChangGoodsInfo icpInfoModel)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = 0
            };
            try
            {
                result = IcpBo.Instance.SaveIcpBYJiChangGoodsInfo(icpInfoModel, CurrentUser.Base);
                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取白云机场的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetIcpBYJiChangGoodsInfo(int? id)
        {
            IcpBYJiChangGoodsInfo info = new IcpBYJiChangGoodsInfo();
            if (id.HasValue)
            {
                info = IcpBo.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(id.Value);
            }
            return Json(new { Info = info }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存广州南沙的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult UpdateIcpGZNanShaGoodsInfo(IcpGZNanShaGoodsInfo icpInfoModel)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = 0
            };
            try
            {
                result = IcpBo.Instance.SaveIcpGZNanShaGoodsInfo(icpInfoModel, CurrentUser.Base);
                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取白云机场的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetIcpGZNanShaGoodsInfo(int? id)
        {
            IcpGZNanShaGoodsInfo info = new IcpGZNanShaGoodsInfo();
            if (id.HasValue)
            {
                info = IcpBo.Instance.GetIcpGZNanShaGoodsInfoEntityByPid(id.Value);
            }
            return Json(new { Info = info }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 保存高捷物流的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-05-27 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult UpdateLgGaoJieGoodsInfo(LgGaoJieGoodsInfo lgInfoModel)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = 0
            };
            try
            {
                result = LogisticsBo.Instance.SaveLgGaoJieGoodsInfo(lgInfoModel, CurrentUser.Base);
                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取高捷物流的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PD1001001, PrivilegeCode.PD1001201)]
        public JsonResult GetLgGaoJieGoodsInfoEntityByPid(int? id)
        {
            LgGaoJieGoodsInfo info = new LgGaoJieGoodsInfo();
            if (id.HasValue)
            {
                info = LogisticsBo.Instance.GetLgGaoJieGoodsInfoEntityByPid(id.Value);
            }
            return Json(new { Info = info }, JsonRequestBehavior.AllowGet);
        }

        #region  门店下单使用扫描枪进行商品下单

        /// <summary>
        /// 通过商品条形码获取商品系统编号
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns>商品系统编号</returns>
        /// <remarks>2016-05-25 杨浩 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1004201)]
        public int GetProductSysNoByBarCode(string barCode)
        {
            var sysNo = PdProductBo.Instance.GetProductSysNoByBarCode(barCode);
            return sysNo;
        }
        #endregion

        #region 商品条码

        /// <summary>
        /// 商品条码
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.PD1006504, PrivilegeCode.PD1006504)]
        public ActionResult PdProductBarcode(int? id, string Barcode = "")
        {
            int pageIndex = id ?? 1;
            string selector = Request.Params["selector"];
            PagedList<PdProductBarcode> list = new PagedList<PdProductBarcode>();
            Pager<PdProductBarcode> pager = new Pager<PdProductBarcode>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new PdProductBarcode { Barcode = Barcode };
            pager.PageSize = list.PageSize;

            pager = BLL.Product.PdProductBo.Instance.GetPdProductBarcodeList(pager);

            list = new PagedList<PdProductBarcode>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize

            };
            if (!string.IsNullOrEmpty(selector) && selector == "selector") //属性组组件view层
            {
                return PartialView("_AjaxProductProductBarcodePagerSelector", list);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxProductProductBarcodePager", list);
            }
            return View();
        }
        /// <summary>
        /// 创建/修改商品条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult ProductBarcodeCreate()
        {
            PdProductBarcode model = new PdProductBarcode();
            ViewBag.isexis = false;
            //查看逻辑
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = BLL.Product.PdProductBo.Instance.GetProductBarcodeEntity(sysno);
                if (BLL.Product.PdProductBo.Instance.IsExistsPdProductBarcode(model.Prefix + model.Barcode) > 0)
                {
                    ViewBag.isexis = true;
                }
            }
            return View(model);
        }

        /// <summary>
        /// 属性组添加界面
        /// </summary>
        /// <param name="model">属性组实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult ProductBarcodeCreate(PdProductBarcode model)
        {
            Result result;
            string des = model.SysNo > 0 ? "修改条码" : "创建条码";
            result = BLL.Product.PdProductBo.Instance.SaveProductBarcode(model);
            if (result.Status)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }

            return Json(result);
        }

        /// <summary>
        /// 查询条形码列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2016-09-01 周 创建</remarks>
        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult BarCodeList()
        {
            return View();
        }
        /// <summary>
        /// 条形码分页查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult BarCodeQuery(int? id, string keyword)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = PdProductBo.Instance.BarcodeQuery(keyword, currentPage, pageSize);

            var list = new PagedList<PdProductBarcode>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_BarCodeQueryPager", list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult PdProductList()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>

        [Privilege(PrivilegeCode.PD1006504)]
        public ActionResult PdProductListQuery(int? id, string keyword)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = PdProductBo.Instance.ProductListQuery(keyword, currentPage, pageSize);

            var list = new PagedList<PdProduct>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PdProductListQueryPager", list);
        }
        #endregion


        #region 商品套装管理
        /// <summary>
        /// 商品套装管理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PdPackagedGoodsList(int? id, int? status, string keyWord)
        {
            if (Request.IsAjaxRequest())
            {
                var filter = new Pager<PdPackagedGoods>();
                filter.CurrentPage = (int)id;
                filter.PageFilter.Status = status;
                filter.PageFilter.Code = keyWord;
                var pager = PdPackagedGoodsBo.Instance.GetPageList(filter, 0);
                var list = new PagedList<PdPackagedGoods>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
                return PartialView("_PdPackagedGoodsList", list);
            }
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.PdPackaged.PdPackagedGoods.PdPackagedGoodsStatusEnum>(null, null).ToString());
            ViewBag.PageIndex = id == null ? 1 : id;
            return View();
        }


        /// <summary>
        /// 编辑套装商品
        /// </summary>
        /// <param name="model">套装商品实体</param>
        /// <param name="PdCode">产品代码</param>
        /// <param name="PdSysNo">产品系统编码</param>
        /// <param name="PdName">产品名称</param>
        /// <param name="Company">单位</param>
        /// <param name="UnitPrice">单价</param>
        /// <param name="Count">套装数量</param>
        /// <param name="WarehouseSysNo">WarehouseSysNo</param>
        /// <param name="WarehouseName">WarehouseName</param>
        /// <param name="Remarks">Remarks</param>
        /// <param name="PageIndex">返回分页页面数</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditPdPackagedGoods(PdPackagedGoods model, string PdCode, string PdSysNo, string PdName, string Company, string UnitPrice, string Count, string WarehouseSysNo, string WarehouseName, string RemarksTo, int? PageIndex = 1)
        {
            #region 提交
            if (Request.IsAjaxRequest())
            {
                Result<object> result = new Result<object>();
                //添加
                List<PdPackagedGoodsEntry> list = new List<PdPackagedGoodsEntry>();
                for (int i = 0; i < PdCode.Trim(',').Split(',').Length; i++)
                {
                    var pd = new PdPackagedGoodsEntry()
                    {
                        PdSysNo = Convert.ToInt32(PdSysNo.Trim(',').Split(',')[i]),
                        PdCode = PdCode.Trim(',').Split(',')[i],
                        PdName = PdName.Trim(',').Split(',')[i],
                        Company = Company.Trim(',').Split(',')[i],
                        UnitPrice = Convert.ToDecimal(UnitPrice.Trim(',').Split(',')[i]),
                        Count = Convert.ToDecimal(Count.Trim(',').Split(',')[i]),
                        WarehouseSysNo = Convert.ToInt32(WarehouseSysNo.Trim(',').Split(',')[i]),
                        WarehouseName = WarehouseName.Trim(',').Split(',')[i],
                        Remarks = RemarksTo.Trim('^').Split('^')[i],
                    };
                    list.Add(pd);
                }
                model.Status = (int)Hyt.Model.PdPackaged.PdPackagedGoods.PdPackagedGoodsStatusEnum.待审核;
                model.Code = PdPackagedGoodsBo.Instance.GetSetCode();
                if (PdPackagedGoodsBo.Instance.Add(model, list))
                {
                    result.Status = true;
                    result.Message = "成功!";
                    return Json(result);
                }
                else
                {
                    result.Status = true;
                    result.Message = "异常,创建失败!异常原因:请检查单据编号与套装代码是否已存在!";
                    return Json(result);
                }
            }
            #endregion
            #region 初始化进入
            else
            {
                #region 查看进入
                if (model.SysNo > 0)
                {
                    model = PdPackagedGoodsBo.Instance.GetPageModels((int)model.SysNo, 1);
                }
                #endregion
                #region 创建进入
                else
                {
                    model = new PdPackagedGoods();
                }
                #endregion
            }
            #endregion

            ViewBag.AdminName = CurrentUser.Base.UserName;
            ViewBag.AdminSysNo = CurrentUser.Base.SysNo;
            ViewBag.PageIndex = PageIndex;
            return View(model);
        }

        #region 导出Excel
        /// <summary>
        /// 导出套装商品模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2017-08-17 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void ExportTemplate()
        {
            ExportExcel(@"\Templates\Excel\PdPackagedGoods.xls", "套装商品模板");
        }
        /// <summary>
        /// 导出excel模板文件
        /// </summary>
        /// <param name="tmpPath">模板路径</param>
        /// <param name="excelFileName">导出文件名</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private void ExportExcel(string tmpPath, string excelFileName)
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), tmpPath, 1, excelFileName, null, false);
        }
        #endregion

        #region 导入Excel
        public static bool _startingTo;
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult ImportWhInventoryExcel()
        {
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Resuldt<PdPackagedGoodsEntry>();
                if (!_startingTo)
                {
                    _startingTo = true;
                    try
                    {
                        result = PdPackagedGoodsBo.Instance.ImportExcel(httpPostedFileBase.InputStream,
                            CurrentUser.Base.SysNo);
                    }
                    catch (Exception e)
                    {
                        result.Message = string.Format(e.Message);
                        result.Status = false;
                    }
                    finally
                    {
                        _startingTo = false;
                    }
                }
                else
                {
                    result.Message = string.Format("正在导入数据，请稍后再操作");
                    result.Status = false;
                }
                ViewBag.result = HttpUtility.UrlEncode(JsonConvert.SerializeObject(result).ToString());// HttpUtility.UrlEncode();
            }
            return View();
        }
        #endregion

        #region 套装商品审核、作废
        /// <summary>
        /// 套装商品审核、作废
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// 吴琨 2017/8/28 创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditPdPackagedGoodsStatus(int sysNo, int status)
        {
            Result<object> result = new Result<object>();
            if (PdPackagedGoodsBo.Instance.UpdateStatus(sysNo, status, CurrentUser.Base.SysNo, CurrentUser.Base.UserName))
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            else
            {
                result.Status = true;
                result.Message = "异常,创建失败!";
                return Json(result);
            }
        }
        #endregion

        #endregion

        #region 商品供货规格
        /// <summary>
        /// 添加供货规格
        /// </summary>
        /// <returns>2017/9/1 吴琨  创建</returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AddPdProductSpec()
        {

            return View();
        }


        /// <summary>
        /// 编辑供货规格报价
        /// </summary>
        /// <param name="WarehouseSysNo">仓库系统编号</param>
        /// <param name="PdProductSysNo">商品系统编号</param>
        /// <returns></returns>
        /// 2017/9/1 吴琨  创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditPdProductSpecPrices(IList<PdProductSpecPrices> list, int? WarehouseSysNo, int? PdProductSysNo)
        {
            if (Request.IsAjaxRequest())
            {
                Result result = new Result()
                {
                    Message = "编辑成功!",
                    Status = true
                };
                foreach (var item in list)
                {
                    item.SpecValue = "{\"PdProductSpecValues\":{\"unit\":\"" + item.SpecValueList.unit + "\",\"spec\":\"" + item.SpecValueList.spec + "\",\"price\":\"" + item.SpecValueList.price + "\"}}";
                }
                if (!PdProductSpecBo.Instance.AddPdProductSpecPrices(list))
                {
                    result.Message = "编辑失败!";
                    result.Status = false;
                }
                return Json(result);
            }
            else
            {
                if (WarehouseSysNo == null || PdProductSysNo == null)
                    return View();
                PdProductSpec SpecModel = PdProductSpecBo.Instance.IsPdProductSpec((int)PdProductSysNo);
                ViewBag.SpecModel = SpecModel != null ? SpecModel.SpecValues : "";
                ViewBag.WarehouseSysNo = WarehouseSysNo;
                ViewBag.PdProductSysNo = PdProductSysNo;
                list = PdProductSpecBo.Instance.GetPdProductSpecPrices((int)PdProductSysNo, (int)WarehouseSysNo);
                foreach (var item in list)
                {
                    var json = JObject.Parse(item.SpecValue);

                    item.SpecValueList = new PdProductSpecValues()
                    {
                        unit = json["PdProductSpecValues"]["unit"].ToString(),
                        spec = json["PdProductSpecValues"]["spec"].ToString(),
                        price = Convert.ToDecimal(json["PdProductSpecValues"]["price"])
                    };
                }


                return View(list);
            }
        }

        /// <summary>
        /// 批量调整商品规格销售价
        /// </summary>
        /// <returns></returns>
        /// 2017-9-06 吴琨创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditSalesPrice(string pids, int warehouse, decimal SalesPrice)
        {
            Result result = new Result()
            {
                Message = "调价成功!",
                Status = true
            };

            if (string.IsNullOrEmpty(pids) || warehouse == 0 || SalesPrice == 0)
            {
                result.Message = "失败";
                result.Status = false;
                return Json(result);
            }

            for (int i = 0; i < pids.Split(',').Length; i++)
            {
                var list = PdProductSpecBo.Instance.GetPdProductSpecPrices(Convert.ToInt32(pids.Split(',')[i]), warehouse);
                if (list != null)
                {
                    foreach (PdProductSpecPrices item in list)
                    {
                        if (SalesPrice > 0)
                        {
                            item.SalesPrice = item.EquivalentPrice + (item.EquivalentPrice * (SalesPrice / 100));
                        }
                        else
                        {
                            item.SalesPrice = item.EquivalentPrice - (item.EquivalentPrice * ((SalesPrice / 100) * -1));
                        }
                        PdProductSpecBo.Instance.UpdateSpecPrices(item);
                    }
                }
            }
            return Json(result);
        }
        #endregion

    }
}