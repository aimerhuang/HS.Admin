using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Product;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品分类业务逻辑
    /// </summary>
    /// <remarks>2013-06-25 邵斌 创建</remarks>
    public class PdCategoryBo : BOBase<PdCategoryBo>
    {
        /// <summary>
        /// 返回所以的商品分类，无论状态
        /// </summary>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        public IList<PdCategory> GetAllCategory()
        {
           
                return IPdCategoryDao.Instance.GetAllCategory();
        }

        /// <summary>
        /// 获取所有商品分类列表JSON数据（主要用于ZTree）
        /// </summary>
        /// <returns>动态List</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        public object GetAllCategoryJsonList(bool isHideDisable = false)
        {
            var allCategoryList = CacheManager.Get<IList<PdCategory>>(CacheKeys.Items.BackendAllPdCategoryZtreeNodeData,
                                                  delegate()
                                                  {
                                                      return GetAllCategory();
                                                  });
            if (isHideDisable)
            {
                allCategoryList = allCategoryList.Where(c => c.Status == 1).OrderBy(x => x.DisplayOrder).ToList();
            }
               

            var nodes = from c in allCategoryList
                        select new
                        {
                            id = c.SysNo
                               ,
                            name = c.CategoryName
                               ,
                            open = false
                               ,
                            pId = c.ParentSysNo
                                ,
                            status = c.Status,
                            isOnline = c.IsOnline
                        };

            return nodes.ToList();
        }

        /// <summary>
        /// 读取所以有效的商品分类,可以读取指定的商品分类类别
        /// </summary>
        /// <param name="SysNo">商品分类编号</param>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public IList<PdCategory> GetCategoryList(int? sysNo)
        {
            return IPdCategoryDao.Instance.GetCategoryList(sysNo);
        }

        /// <summary>
        /// 获取单个商品分类
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isGetParent">是否获取父级商品分类对象</param>
        /// <returns>返回单个商品分类</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public PdCategory GetCategory(int sysNo, bool isGetParent = false)
        {
            if (isGetParent)
            {
                return CacheManager.Get<PdCategory>(CacheKeys.Items.CategoryInfoWithParent_, sysNo.ToString(),
                                                    delegate()
                                                    {
                                                        return IPdCategoryDao.Instance.GetCategory(sysNo,
                                                                                                   true);
                                                    });
            }
            else
            {
                return CacheManager.Get<PdCategory>(CacheKeys.Items.SingleCategoryInfo_, sysNo.ToString(),
                                                  delegate()
                                                  {
                                                      return IPdCategoryDao.Instance.GetCategory(sysNo,
                                                                                                 false);
                                                  });
            }
        }

        /// <summary>
        /// 获取单个商品分类路径信息，并用分隔符分隔
        /// </summary>
        /// <param name="sysNo">分类系统编号</param>
        /// <param name="delimeter">分隔字符</param>
        /// <returns>返回完整分类溪经字符串</returns>
        /// <remarks>2013-08-06 邵斌 创建</remarks>
        public string GetCategoryRouteString(int sysNo, string delimeter)
        {
            //通过缓存读取数据
            return CacheManager.Get<string>(CacheKeys.Items.ProductMasterCategoryRoute_, sysNo.ToString(), delegate()
                {
                    //递归读取指定的分类所以上级分类
                    PdCategory category = GetCategory(sysNo, true);
                    StringBuilder sb = new StringBuilder();

                    //循环拼接字符串，直到分类么有父级分类
                    do
                    {
                        //添加分类名称到结果字符串
                        sb.Append(category.CategoryName);

                        //如果存在下级分类将添加分隔符到字符串前面
                        if (category.ParentCategory != null)
                            sb.Append(delimeter);
                        else
                            break;

                        //设置新分类
                        category = category.ParentCategory;

                    } while (true);
                    return sb.ToString();
                });
        }

        /// <summary>
        /// 获取单个商品分类回溯分类列表
        /// </summary>
        /// <param name="sysNo">分类系统编号</param>
        /// <returns>返回完整回溯分类列表</returns>
        /// <remarks>2013-08-06 邵斌 创建</remarks>
        public IList<PdCategory> GetCategoryRouteList(int sysNo)
        {
            PdCategory category = GetCategory(sysNo, true);
            IList<PdCategory> result = new List<PdCategory>();

            int displayOrderIndex = 0;
            while (category != null)
            {
                category.DisplayOrder = displayOrderIndex++;
                result.Add(category);
                category = category.ParentCategory;
            }
            return result.OrderByDescending(c => c.DisplayOrder).ToList();
        }

        /// <summary>
        /// 更新商品分类状态
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">操作系统管理员的系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建 </remarks>
        public bool ChangeStatus(int sysNo, ProductStatus.商品分类状态 status, int adminSysNo)
        {
            //如果分类编号为0将更新失败
            if (sysNo == 0)
                return false;

            if (IPdCategoryDao.Instance.ChangeStatus(sysNo, status, adminSysNo))
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}状态为{1}", sysNo, status.ToString()),
                                              LogStatus.系统日志目标类型.商品分类状态, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}状态为{1}失败", sysNo, status.ToString()),
                                              LogStatus.系统日志目标类型.商品分类状态, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 设置商品分类是否显示
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isOnline">要变更的状态</param>
        /// <param name="adminSysNo">管理员系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建 </remarks>
        [Obsolete]
        public bool SetIsOnline(int sysNo, ProductStatus.是否前端展示 isOnline, int adminSysNo)
        {
            //如果分类编号为0将更新失败
            if (sysNo == 0)
                return false;

            if (IPdCategoryDao.Instance.SetIsOnline(sysNo, isOnline, adminSysNo))
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}在线状态为{1}", sysNo,isOnline.ToString()),
                                              LogStatus.系统日志目标类型.商品分类在线显示状态, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}在线状态为{1}失败", sysNo, isOnline.ToString()),
                                              LogStatus.系统日志目标类型.商品分类在线显示状态, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="category">商品分类实体</param>
        /// <param name="attributeGroups">商品分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2015-08-31 杨浩 创建</remarks>
        public bool EditCategory(PdCategory category, IList<PdAttributeGroup> attributeGroups)
        {
            //string oldCode = null;
            int? oldParentSysNo = null;
            string oldSysNos = "";
            string newSysNos = "";
            bool success;
            //取得元数据
            PdCategory updateCategory = GetCategory(category.SysNo);

            //判断是否需要格式化基础信息                    
            if (updateCategory.ParentSysNo != category.ParentSysNo)
            {
                oldParentSysNo = updateCategory.ParentSysNo;
                //oldCode = updateCategory.;
                //updateCategory.Code = GetFreeCodeNum(category.ParentSysNo);
                //updateCategory.SysNos=
                oldSysNos = updateCategory.SysNos;

                newSysNos =IPdCategoryDao.Instance.GetNewSysNos(category.ParentSysNo,category.SysNo);
                updateCategory.SysNos = newSysNos;
            }

            //赋新值
            updateCategory.CategoryImage = category.CategoryImage;
            updateCategory.CategoryName = category.CategoryName;
            updateCategory.LastUpdateBy = category.LastUpdateBy;
            updateCategory.LastUpdateDate = category.LastUpdateDate;
           

            //稍后实现系统分类code的维护
            updateCategory.ParentSysNo = category.ParentSysNo;
            updateCategory.SeoDescription = category.SeoDescription;
            updateCategory.SeoKeyword = category.SeoKeyword;
            updateCategory.SeoTitle = category.SeoTitle;

            IList<PdCatAttributeGroupAso> attributeGroupAso = new List<PdCatAttributeGroupAso>();
            //如果设有分类属性组，将添加设置的属性组
            if (attributeGroups != null && attributeGroups.Count > 0)
            {
                //遍历商品分类下挂的属性组，并初始化数据
                foreach (var group in attributeGroups)
                {
                    attributeGroupAso.Add(new PdCatAttributeGroupAso()
                        {
                            CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo
                            ,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
                            LastUpdateDate = DateTime.Now,
                            ProductCategorySysNo = category.SysNo,
                            SysNo = 0,
                            AttributeGroupSysNo = group.SysNo
                        });
                }

            }

            
                    //更新商品分类
                    success = IPdCategoryDao.Instance.EditCategory(updateCategory,attributeGroupAso);

                    //格式化信息
                    if (success && oldParentSysNo!=null)
                    {
                        //读取新分类下的空去位置，如果没有空缺位置就追加
                       // success = success && IPdCategoryDao.Instance.UpdataChildrenCode(oldCode,updateCategory.Code);

                        //更新子分类系统编号路由
                        success = success && IPdCategoryDao.Instance.UpdateChildrenSysNos(oldSysNos,newSysNos,updateCategory.SysNo);
                        //将该分类最近到新父分类末尾
                        success = success && IPdCategoryDao.Instance.SetCategoryToLastShow(updateCategory);
                    }

                    //提交事务
                    if (success)
                    {
                        //用户操作日志
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                                      string.Format("更新商品分类{0}基本信息", category.SysNo),
                                                      LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                      AdminAuthenticationBo.Instance.Current.Base.SysNo);                        
                    }
                    else
                    {
                        //用户操作日志
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                                      string.Format("更新商品分类{0}基本信息失败", category.SysNo),
                                                      LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                      AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    }
               
            

            BLL.Cache.DeleteCache.ProductCategory(category.SysNo);

            return true;
        }
        /// <summary>
        /// 修改商品分类(返回ID)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="attributeGroups"></param>
        /// <returns></returns>
        public int EditPdCategory(PdCategory category, IList<PdAttributeGroup> attributeGroups)
        {
            //string oldCode = null;
            int? oldParentSysNo = null;
            string oldSysNos = "";
            string newSysNos = "";
            bool success;
            //取得元数据
            PdCategory updateCategory = GetCategory(category.SysNo);

            //判断是否需要格式化基础信息                    
            if (updateCategory.ParentSysNo != category.ParentSysNo)
            {
                oldParentSysNo = updateCategory.ParentSysNo;
                //oldCode = updateCategory.;
                //updateCategory.Code = GetFreeCodeNum(category.ParentSysNo);
                //updateCategory.SysNos=
                oldSysNos = updateCategory.SysNos;

                newSysNos = IPdCategoryDao.Instance.GetNewSysNos(category.ParentSysNo, category.SysNo);
                updateCategory.SysNos = newSysNos;
            }

            //赋新值
            updateCategory.CategoryImage = category.CategoryImage;
            updateCategory.CategoryName = category.CategoryName;
            updateCategory.LastUpdateBy = category.LastUpdateBy;
            updateCategory.LastUpdateDate = category.LastUpdateDate;


            //稍后实现系统分类code的维护
            updateCategory.ParentSysNo = category.ParentSysNo;
            updateCategory.SeoDescription = category.SeoDescription;
            updateCategory.SeoKeyword = category.SeoKeyword;
            updateCategory.SeoTitle = category.SeoTitle;

            IList<PdCatAttributeGroupAso> attributeGroupAso = new List<PdCatAttributeGroupAso>();
            //如果设有分类属性组，将添加设置的属性组
            if (attributeGroups != null && attributeGroups.Count > 0)
            {
                //遍历商品分类下挂的属性组，并初始化数据
                foreach (var group in attributeGroups)
                {
                    attributeGroupAso.Add(new PdCatAttributeGroupAso()
                    {
                        CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo
                        ,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
                        LastUpdateDate = DateTime.Now,
                        ProductCategorySysNo = category.SysNo,
                        SysNo = 0,
                        AttributeGroupSysNo = group.SysNo
                    });
                }

            }


            //更新商品分类
            success = IPdCategoryDao.Instance.EditCategory(updateCategory, attributeGroupAso);

            //格式化信息
            if (success && oldParentSysNo != null)
            {
                //读取新分类下的空去位置，如果没有空缺位置就追加
                // success = success && IPdCategoryDao.Instance.UpdataChildrenCode(oldCode,updateCategory.Code);

                //更新子分类系统编号路由
                success = success && IPdCategoryDao.Instance.UpdateChildrenSysNos(oldSysNos, newSysNos, updateCategory.SysNo);
                //将该分类最近到新父分类末尾
                success = success && IPdCategoryDao.Instance.SetCategoryToLastShow(updateCategory);
            }

            //提交事务
            if (success)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}基本信息", category.SysNo),
                                              LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("更新商品分类{0}基本信息失败", category.SysNo),
                                              LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }



            BLL.Cache.DeleteCache.ProductCategory(category.SysNo);

            return category.SysNo;
        }


        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <param name="attributeGroups">属性列表</param>
        /// <returns>true：创建成功 false：创建失败</returns>
        /// <remarks>2013-07-29 邵斌 创建</remarks>
        public bool CreateProductCategory(PdCategory category, IList<PdAttributeGroup> attributeGroups)
        {
            bool success = false;

            IList<PdCatAttributeGroupAso> attributeGroupAso = new List<PdCatAttributeGroupAso>();

            //如果设有分类属性组，将添加设置的属性组
            if (attributeGroups != null && attributeGroups.Count > 0)
            {
                //遍历商品分类下对应的属性分组，并初始化数据
                foreach (var group in attributeGroups)
                {
                    attributeGroupAso.Add(new PdCatAttributeGroupAso()
                    {
                        CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo
                        ,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
                        LastUpdateDate = DateTime.Now,
                        ProductCategorySysNo = category.SysNo,
                        SysNo = 0,
                        AttributeGroupSysNo = group.SysNo
                    });
                }
            }

            category.Code = ""; //GetFreeCodeNum(category.ParentSysNo);
            category.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            category.CreatedDate = DateTime.Now;
            category.LastUpdateBy = category.CreatedBy;
            category.LastUpdateDate = category.CreatedDate;
            category.Status = (int)Model.WorkflowStatus.ProductStatus.商品分类状态.有效;
            
           

            //如果分类是子分类，则将继承父分类状态和显示样式
            if (category.ParentSysNo > 0)
            {
                category.SysNos =GetCategory(category.ParentSysNo).SysNos;
                PdCategory parentCategory = IPdCategoryDao.Instance.GetCategory(category.ParentSysNo);
                category.Status = parentCategory.Status;
                category.IsOnline = parentCategory.IsOnline;
            }

            
                success =
                    IPdCategoryDao.Instance.CreateCategory(category,
                                                            attributeGroupAso);

                //如果返回创建成功建提交事务
                if (success)
                {
                    
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                                  string.Format("新增商品分类{0}基本信息", category.SysNo),
                                                  LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                  AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }
                else
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                                  string.Format("新增商品分类{0}基本信息失败", category.SysNo),
                                                  LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                  AdminAuthenticationBo.Instance.Current.Base.SysNo); 
                }
            

            BLL.Cache.DeleteCache.ProductCategory(category.SysNo);

            return success;
        }

        /// <summary>
        /// 添加商品分类（返回ID）
        /// </summary>
        /// <param name="category"></param>
        /// <param name="attributeGroups"></param>
        /// <returns></returns>
        public int CreatePdCategory(PdCategory category, IList<PdAttributeGroup> attributeGroups)
        {
            bool success = false;

            IList<PdCatAttributeGroupAso> attributeGroupAso = new List<PdCatAttributeGroupAso>();

            //如果设有分类属性组，将添加设置的属性组
            if (attributeGroups != null && attributeGroups.Count > 0)
            {
                //遍历商品分类下对应的属性分组，并初始化数据
                foreach (var group in attributeGroups)
                {
                    attributeGroupAso.Add(new PdCatAttributeGroupAso()
                    {
                        CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo
                        ,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
                        LastUpdateDate = DateTime.Now,
                        ProductCategorySysNo = category.SysNo,
                        SysNo = 0,
                        AttributeGroupSysNo = group.SysNo
                    });
                }
            }

            category.Code = ""; //GetFreeCodeNum(category.ParentSysNo);
            category.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            category.CreatedDate = DateTime.Now;
            category.LastUpdateBy = category.CreatedBy;
            category.LastUpdateDate = category.CreatedDate;
            category.Status = (int)Model.WorkflowStatus.ProductStatus.商品分类状态.有效;



            //如果分类是子分类，则将继承父分类状态和显示样式
            if (category.ParentSysNo > 0)
            {
                category.SysNos = GetCategory(category.ParentSysNo).SysNos;
                PdCategory parentCategory = IPdCategoryDao.Instance.GetCategory(category.ParentSysNo);
                category.Status = parentCategory.Status;
                category.IsOnline = parentCategory.IsOnline;
            }


            success =
                IPdCategoryDao.Instance.CreateCategory(category,
                                                        attributeGroupAso);

            //如果返回创建成功建提交事务
            if (success)
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("新增商品分类{0}基本信息", category.SysNo),
                                              LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("新增商品分类{0}基本信息失败", category.SysNo),
                                              LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }


            BLL.Cache.DeleteCache.ProductCategory(category.SysNo);

            return category.SysNo;
        }

        /// <summary>
        /// 设置分类系统编号路由
        /// </summary>
        /// <param name="sysNos">系统编号路由</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2015-07-29 杨浩 创建</remarks>
        public bool SetCateorySysNosBySysNo(int sysNo)
        {
            return IPdCategoryDao.Instance.SetCateorySysNosBySysNo(sysNo);
        }
        /// <summary>
        /// 交换显示分类的显示排序
        /// </summary>
        /// <param name="originalSysNo">交换源对象系统编号</param>
        /// <param name="objectiveSysNo">要进行位置交换的目标对象系统编号</param>
        /// <returns>返回： true 操作成功  false 操作失败</returns>
        /// <remarks>注意：该方法值适用于在同一父级中进行移动变更</remarks>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        public bool SwapDisplayOrder(int originalSysNo, int objectiveSysNo)
        {
            bool success = false;
            PdCategory original = IPdCategoryDao.Instance.GetCategory(originalSysNo);
            PdCategory objective = IPdCategoryDao.Instance.GetCategory(objectiveSysNo);

            //显示位置交换
            var objectIndex = objective.DisplayOrder;
            objective.DisplayOrder = original.DisplayOrder;
            original.DisplayOrder = objectIndex;

            success = IPdCategoryDao.Instance.Update(original);

            //源对象更是是否成功
            if (success)
            {
                //更新成功就更新目标对象
                success = IPdCategoryDao.Instance.Update(objective);

            }
            else
            {
                //更新失败，数据还原
                original.DisplayOrder = objective.DisplayOrder;
                IPdCategoryDao.Instance.Update(original);
            }

            if (success)
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                             string.Format("商品分类{0}与商品分类{1}互换顺序", originalSysNo, objectiveSysNo),
                                             LogStatus.系统日志目标类型.商品分类排序, originalSysNo,
                                             AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("商品分类{0}与商品分类{1}互换顺序失败", originalSysNo, objectiveSysNo),
                                              LogStatus.系统日志目标类型.商品分类排序, originalSysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            //返回结果
            return success;
        }

        /// <summary>
        /// 修改商品分类的父节点
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="parentSysNo">父节点系统编号</param>
        /// <returns>true：修改成功 false:修改失败</returns>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        public bool CategoryChangeParentSysNo(int sysNo, int parentSysNo)
        {
            PdCategory category = GetCategory(sysNo);

            //判断商品分类是否存在
            if (category == null)
            {   
                return false;
            }
            else
            {

                if (IPdCategoryDao.Instance.Update(category))
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                                  string.Format("更新商品分类{0}父节点为{1}", sysNo,parentSysNo),
                                                  LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                  AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    return true;
                }
                else
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                                  string.Format("更新商品分类{0}父节点为{1}失败", sysNo, parentSysNo),
                                                  LogStatus.系统日志目标类型.商品分类基本信息, category.SysNo,
                                                  AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取空闲的CodeNum
        /// </summary>
        /// <param name="parentSysNo">商品系统编号</param>
        /// <returns>Code 数组字符</returns>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        public string GetFreeCodeNum(int parentSysNo)
        {
            return IPdCategoryDao.Instance.GetFreeCodeNum(parentSysNo).AUTOCODE;
        }

        #region 供应链商品分类
        /// <summary>
        /// 添加关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateSupplyChain(PdCategoryRelatedSupplyChain model)
        {
            return IPdCategoryDao.Instance.CreateSupplyChain(model);
        }
        /// <summary>
        /// 是否添加过
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public bool IsExistsSupplyChain(string SCName, int SupplyChainCode)
        {
            return IPdCategoryDao.Instance.IsExistsSupplyChain(SCName, SupplyChainCode);

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public PdCategoryRelatedSupplyChain GetEntitySupplyChain(int SysNo)
        {
            return IPdCategoryDao.Instance.GetEntitySupplyChain(SysNo);
        }
        /// <summary>
        /// 根据供应链编号及供应链商品分类名称获取数据
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public PdCategoryRelatedSupplyChain GetEntityByNameSupplyChain(string SCName, int SupplyChainCode)
        {
            return IPdCategoryDao.Instance.GetEntityByNameSupplyChain(SCName, SupplyChainCode);
        }
        /// <summary>
        /// 根据分类ID及供应商编号获取数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public PdCategoryRelatedSupplyChain GetEntityByCSysNoSupplyChain(int CategorySysNo, int SupplyChainCode)
        {
            return IPdCategoryDao.Instance.GetEntityByCSysNoSupplyChain(CategorySysNo, SupplyChainCode);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSupplyChain(PdCategoryRelatedSupplyChain model)
        {
            return IPdCategoryDao.Instance.UpdateSupplyChain(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public int DeleteSupplyChain(int sysNo)
        {
            return IPdCategoryDao.Instance.DeleteSupplyChain(sysNo);
        } 
        /// <summary>
        /// 根据类别ID查询数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <returns></returns>
        public IList<PdCategoryRelatedSupplyChain> SelectAllSupplyChain(int CategorySysNo)
        {
            return IPdCategoryDao.Instance.SelectAllSupplyChain(CategorySysNo);
        }
         /// <summary>
        /// 根据类别ID、供应链编号查询供应链绑定数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public IList<PdCategoryRelatedSupplyChain> GetSupplyChainList(int CategorySysNo, int SupplyChainCode)
        {
            return IPdCategoryDao.Instance.GetSupplyChainList(CategorySysNo, SupplyChainCode);
        }
        #endregion


        public IList<PdCategory> GetAllCategory(int sysNo)
        {
            return IPdCategoryDao.Instance.GetAllCategory(sysNo);
        }

        public IList<PdCategory> GetCategoryListByParent(int[] parentSysNo)
        {
            return IPdCategoryDao.Instance.GetCategoryListByParent(parentSysNo);
        }

        public IList<PdCategory> GetCategoryListByParentName(int parentSysNo)
        {
            return IPdCategoryDao.Instance.GetCategoryListByParentName(parentSysNo);
        }
    }
}
