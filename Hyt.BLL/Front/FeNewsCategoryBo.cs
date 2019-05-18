using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Front;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    public class FeNewsCategoryBo : BOBase<FeNewsCategoryBo>
    {
        /// <summary>
        /// 返回所以的新闻分类，无论状态
        /// </summary>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public IList<FeNewsCategory> GetAllCategory()
        {
            return IFeNewsCategoryDao.Instance.GetAllCategory();
        }

        /// <summary>
        /// 获取所有新闻分类列表JSON数据（主要用于ZTree）
        /// </summary>
        /// <returns>动态List</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public object GetAllCategoryJsonList(bool isHideDisable = false)
        {
            var allCategoryList = CacheManager.Get<IList<FeNewsCategory>>(CacheKeys.Items.BackendAllNewsCategoryZtreeNodeData,
                                                  delegate()
                                                  {
                                                      return GetAllCategory();
                                                  });
            if (isHideDisable)
                allCategoryList = allCategoryList.Where(c => c.Status == 1).ToList();

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
                            status = c.Status
                        };

            return nodes.ToList();
        }

        /// <summary>
        /// 读取所以有效的新闻分类,可以读取指定的新闻分类类别
        /// </summary>
        /// <param name="SysNo">新闻分类编号</param>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public IList<FeNewsCategory> GetCategoryList(int? sysNo)
        {
            return IFeNewsCategoryDao.Instance.GetCategoryList(sysNo);
        }

        /// <summary>
        /// 更新新闻分类状态
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">操作系统管理员的系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-07 罗远康 创建 </remarks>
        public bool ChangeStatus(int sysNo, NewsStatus.新闻分类状态 status, int adminSysNo)
        {
            //如果分类编号为0将更新失败
            if (sysNo == 0)
                return false;

            if (IFeNewsCategoryDao.Instance.ChangeStatus(sysNo, status, adminSysNo))
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("更新新闻分类{0}状态为{1}", sysNo, status.ToString()),
                                              LogStatus.系统日志目标类型.新闻帮助管理, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return true;
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("更新新闻分类{0}状态为{1}失败", sysNo, status.ToString()),
                                              LogStatus.系统日志目标类型.新闻帮助管理, sysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
                return false;
            }
        }

        /// <summary>
        /// 添加新闻分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <param name="attributeGroups">属性列表</param>
        /// <returns>true：创建成功 false：创建失败</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public bool CreateNewsCategory(FeNewsCategory category)
        {
            bool success = false;

            category.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
            category.CreatedDate = DateTime.Now;
            category.LastUpdateBy = category.CreatedBy;
            category.LastUpdateDate = category.CreatedDate;
            category.Status = (int)Model.WorkflowStatus.NewsStatus.新闻分类状态.有效;
            category.DealerSysNo = category.CreatedBy;


            //如果分类是子分类，则将继承父分类状态和显示样式
            if (category.ParentSysNo > 0)
            {
                category.SysNos = GetCategory(category.ParentSysNo).SysNos;
                FeNewsCategory parentCategory = IFeNewsCategoryDao.Instance.GetCategory(category.ParentSysNo);
                category.Status = parentCategory.Status;
            }


            success =
                IFeNewsCategoryDao.Instance.CreateCategory(category);

            //如果返回创建成功建提交事务
            if (success)
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("新增新闻分类{0}基本信息", category.SysNo),
                                              LogStatus.系统日志目标类型.新闻帮助管理, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("新增新闻分类{0}基本信息失败", category.SysNo),
                                              LogStatus.系统日志目标类型.新闻帮助管理, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }


            BLL.Cache.DeleteCache.NewsCategory(category.SysNo);

            return success;
        }

        /// <summary>
        /// 修改新闻分类
        /// </summary>
        /// <param name="category">新闻分类实体</param>
        /// <param name="attributeGroups">新闻分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public bool EditCategory(FeNewsCategory category)
        {
            int? oldParentSysNo = null;
            string oldSysNos = "";
            string newSysNos = "";
            bool success;
            //取得元数据
            FeNewsCategory updateCategory = GetCategory(category.SysNo);

            //判断是否需要格式化基础信息                    
            if (updateCategory.ParentSysNo != category.ParentSysNo)
            {
                oldParentSysNo = updateCategory.ParentSysNo;
                oldSysNos = updateCategory.SysNos;
                newSysNos = IFeNewsCategoryDao.Instance.GetNewSysNos(category.ParentSysNo, category.SysNo);
                updateCategory.SysNos = newSysNos;
            }

            //赋新值
            updateCategory.CategoryImage = category.CategoryImage;
            updateCategory.CategoryName = category.CategoryName;
            updateCategory.LastUpdateBy = category.LastUpdateBy;
            updateCategory.LastUpdateDate = category.LastUpdateDate;
            updateCategory.ParentSysNo = category.ParentSysNo;
            updateCategory.Remarks = category.Remarks;
            updateCategory.SeoDescription = category.SeoDescription;
            updateCategory.SeoKeyword = category.SeoKeyword;
            updateCategory.SeoTitle = category.SeoTitle;

            //更新新闻分类
            success = IFeNewsCategoryDao.Instance.EditCategory(updateCategory);

            //格式化信息
            if (success && oldParentSysNo != null)
            {

                //更新子分类系统编号路由
                success = success && IFeNewsCategoryDao.Instance.UpdateChildrenSysNos(oldSysNos, newSysNos, updateCategory.SysNo);
                //将该分类最近到新父分类末尾
                success = success && IFeNewsCategoryDao.Instance.SetCategoryToLastShow(updateCategory);
            }

            //提交事务
            if (success)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                              string.Format("更新新闻分类{0}基本信息", category.SysNo),
                                              LogStatus.系统日志目标类型.新闻帮助管理, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("更新新闻分类{0}基本信息失败", category.SysNo),
                                              LogStatus.系统日志目标类型.新闻帮助管理, category.SysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }



            BLL.Cache.DeleteCache.NewsCategory(category.SysNo);

            return true;
        }

        /// <summary>
        /// 交换显示分类的显示排序
        /// </summary>
        /// <param name="originalSysNo">交换源对象系统编号</param>
        /// <param name="objectiveSysNo">要进行位置交换的目标对象系统编号</param>
        /// <returns>返回： true 操作成功  false 操作失败</returns>
        /// <remarks>注意：该方法值适用于在同一父级中进行移动变更</remarks>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public bool SwapDisplayOrder(int originalSysNo, int objectiveSysNo)
        {
            bool success = false;
            FeNewsCategory original = IFeNewsCategoryDao.Instance.GetCategory(originalSysNo);
            FeNewsCategory objective = IFeNewsCategoryDao.Instance.GetCategory(objectiveSysNo);

            //显示位置交换
            var objectIndex = objective.DisplayOrder;
            objective.DisplayOrder = original.DisplayOrder;
            original.DisplayOrder = objectIndex;

            success = IFeNewsCategoryDao.Instance.Update(original);

            //源对象更是是否成功
            if (success)
            {
                //更新成功就更新目标对象
                success = IFeNewsCategoryDao.Instance.Update(objective);

            }
            else
            {
                //更新失败，数据还原
                original.DisplayOrder = objective.DisplayOrder;
                IFeNewsCategoryDao.Instance.Update(original);
            }

            if (success)
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                             string.Format("新闻分类{0}与新闻分类{1}互换顺序", originalSysNo, objectiveSysNo),
                                             LogStatus.系统日志目标类型.新闻帮助管理, originalSysNo,
                                             AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {

                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                                              string.Format("新闻分类{0}与新闻分类{1}互换顺序失败", originalSysNo, objectiveSysNo),
                                              LogStatus.系统日志目标类型.新闻帮助管理, originalSysNo,
                                              AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            //返回结果
            return success;
        }

        /// <summary>
        /// 获取单个新闻分类
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="isGetParent">是否获取父级新闻分类对象</param>
        /// <returns>返回单个新闻分类</returns>
        /// <remarks>2016-06-07 罗远康 创建</remarks>
        public FeNewsCategory GetCategory(int sysNo, bool isGetParent = false)
        {
            if (isGetParent)
            {
                return CacheManager.Get<FeNewsCategory>(CacheKeys.Items.NewsCategoryInfoWithParent_, sysNo.ToString(),
                                                    delegate()
                                                    {
                                                        return IFeNewsCategoryDao.Instance.GetCategory(sysNo,
                                                                                                   true);
                                                    });
            }
            else
            {
                return CacheManager.Get<FeNewsCategory>(CacheKeys.Items.SingleNewsCategoryInfo_, sysNo.ToString(),
                                                  delegate()
                                                  {
                                                      return IFeNewsCategoryDao.Instance.GetCategory(sysNo,
                                                                                                 false);
                                                  });
            }
        }
    }
}
