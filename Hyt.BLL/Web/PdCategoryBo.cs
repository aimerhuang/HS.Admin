using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Infrastructure.Caching;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 前台商品分类
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    public class PdCategoryBo : BOBase<PdCategoryBo>
    {
        #region 获取所有可用分类信息
        /// <summary>
        /// 获取所有可用分类信息
        /// </summary>
        /// <returns>分类信息</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public IList<CBPdCategory> GetAllCategory()
        {
            return CacheManager.Get<IList<CBPdCategory>>(CacheKeys.Items.AllCategory, () =>
            {
                IList<CBPdCategory> list = Hyt.DataAccess.Web.IPdCategoryDao.Instance.GetAllCategory();

                var rootCategoryList = list.Where(m => m.ParentSysNo == 0).OrderBy(m => m.DisplayOrder).ToList();
                CBPdCategory cbPdCategory;
                for (int i = 0; i < rootCategoryList.Count; i++)
                {
                    cbPdCategory = rootCategoryList[i];
                    cbPdCategory.Color = GetColor(i);
                    
                }

                return list;
            });
        }
        #endregion

        #region 获取当前分类的子分类
        /// <summary>
        /// 获取当前分类的子分类
        /// </summary>
        /// <param name="categorySysNo">分类编号</param>
        /// <returns>子分类列表</returns>
        /// <remarks>2013-08-21 黄波 创建</remarks>
        public IList<CBPdCategory> GetChildCategory(int categorySysNo)
        {
            return CacheManager.Get<IList<CBPdCategory>>(CacheKeys.Items.ChildCategory_, categorySysNo.ToString(), DateTime.Now.AddMinutes(10), () =>
            {
                var allCategory = (List<CBPdCategory>)GetAllCategory();
                if (allCategory != null && allCategory.Count > 0)
                {
                    return allCategory.FindAll(o => o.ParentSysNo == categorySysNo);
                }
                else
                {
                    return null;
                }
            });
        }
        #endregion

        #region 获取当前分类的所有子分类
        /// <summary>
        /// 获取当前分类的所有子分类
        /// </summary>
        /// <param name="categorySysNo">分类编号</param>
        /// <returns>子分类列表</returns>
        /// <remarks>2013-08-21 黄波 创建</remarks>
        public IList<CBPdCategory> GetChildAllCategory(int categorySysNo)
        {
            return CacheManager.Get<IList<CBPdCategory>>(CacheKeys.Items.ChildAllCategory_, categorySysNo.ToString(), DateTime.Now.AddMinutes(10), () =>
           {
               IList<CBPdCategory> _returnValue = new List<CBPdCategory>();
               _GetChildCategory(categorySysNo, (List<CBPdCategory>)GetAllCategory(), ref _returnValue);
               return _returnValue;
           });
        }

        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="categorySysNo">分类编号</param>
        /// <param name="source">数据源</param>
        /// <param name="returnValue">查询到的数据</param>
        /// <returns>void</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        private void _GetChildCategory(int categorySysNo, List<CBPdCategory> source, ref IList<CBPdCategory> returnValue)
        {
            if (source == null || source.Count <= 0) return;
            var tmp = source.FindAll(o => o.ParentSysNo == categorySysNo);

            foreach (var item in tmp)
            {
                returnValue.Add(item);
                _GetChildCategory(item.SysNo, source, ref returnValue);
            }
        }
        #endregion

        /// <summary>
        /// 生成前台页面导航字符（主要用于面包屑字符）
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="formatString">格式字符串</param>
        /// <param name="delimeter">中间分隔符</param>
        /// <param name="lastItem">最后一个字符串</param>
        /// <returns>返回字符串数组</returns>
        /// <remarks>2013-08-06 邵斌 创建</remarks>
        public string[] BuilderCategoryNavigationString(int categorySysNo, string formatString, string delimeter = "", string lastItem = "")
        {
            IList<string> list = CacheManager.Get(CacheKeys.Items.ProductCategoryNavigationString_, categorySysNo.ToString(), delegate
                {
                    //结果列表
                    IList<string> masterCategoryList = new List<string>();

                    if (categorySysNo == 0)
                    {
                        masterCategoryList.Add("产品列表");
                        return masterCategoryList;
                    }

                    //读取分了的回溯列表（递归查找父级分类）
                    IList<PdCategory> categories = Hyt.BLL.Product.PdCategoryBo.Instance.GetCategoryRouteList(categorySysNo);

                    

                    PdCategory category;
                    //遍历分类列表生产指点字符
                    for (int i = 0; i < categories.Count; i++)
                    {
                        category = categories[i];

                        //通过模板字符串和分隔符生产结果字符串，并保存在结果列表中，如果是最后一个将不插入风符，预期结果如： 分类1 > 分类2 > 分类3
                        masterCategoryList.Add(string.Format(formatString + (i < categories.Count - 1 ? delimeter : ""), category.SysNo, category.CategoryName));
                    }
                    
                    return masterCategoryList;

                });

            if (!string.IsNullOrWhiteSpace(lastItem))
            {
                list.Add(delimeter + lastItem);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 生成前台分类完整路径字符（主要用于列表Title字符）
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="formatString">格式字符串</param>
        /// <param name="delimeter">中间分隔符</param>
        /// <returns>返回字符串</returns>
        /// <remarks>2013-09-16 邵斌 创建</remarks>
        public string BuilderCategoryPathString(int categorySysNo, string formatString, string delimeter = "")
        {
            return CacheManager.Get(CacheKeys.Items.ProductCategoryPathString_, categorySysNo.ToString(), delegate
            {

                //读取分了的回溯列表（递归查找父级分类）
                IList<PdCategory> categories = Hyt.BLL.Product.PdCategoryBo.Instance.GetCategoryRouteList(categorySysNo);

                //结果列表
                StringBuilder result = new StringBuilder();

                PdCategory category;
                //遍历分类列表生产指点字符
                for (int i = 0; i < categories.Count; i++)
                {
                    category = categories[i];

                    //通过模板字符串和分隔符生产结果字符串，并保存在结果列表中，如果是最后一个将不插入风符，预期结果如： 分类1 > 分类2 > 分类3
                    result.Append(string.Format(formatString + (i < categories.Count - 1 ? delimeter : ""), category.CategoryName));
                }

                return result.ToString();

            });
        }

        #region 获取用于搜索的商品属性及属性值列表（商品分类搜索）

        /// <summary>
        /// 搜索功能中通过分类系统编号读取分类配置的属性参数
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <returns>属性值列表</returns>
        /// <remarks>2013-09-16 邵斌 创建</remarks>
        public IList<Hyt.Model.SearchAttributeAndOptions> GetSearchAttributeAndOptions(int categorySysNo)
        {
            //如果没有分类将返回空列表
            if (categorySysNo == 0)
            {
                return new List<SearchAttributeAndOptions>();
            }
            else
            {
                return CacheManager.Get(CacheKeys.Items.SearchCategoryAttributeOptionsList_, categorySysNo.ToString(),
                                        delegate
                                            {
                                                return PdAttributeBo.Instance.GetSearchAttributeAndOptions(categorySysNo);
                                            });
            }
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 获取分类背景颜色
        /// </summary>
        /// <param name="SysNO">分类系统编号</param>
        /// <remarks>
        /// 根据设计图取颜色值,使用分类ID对应
        /// </remarks>
        /// <returns>分类对应的16进制颜色字符</returns>
        /// <remarks>2013-11-20 邵斌 迁移</remarks>
        private string GetColor(int SysNO)
        {
            String _ReturnValue = string.Empty;

            switch (SysNO)
            {
                case 0:
                    _ReturnValue = "#009900";
                    break;
                case 1:
                    _ReturnValue = "#E15517";
                    break;
                case 2:
                    _ReturnValue = "#DA251C";
                    break;
                case 3:
                    _ReturnValue = "#F4AE00";
                    break;
                case 4:
                    _ReturnValue = "#663333";
                    break;
                case 5:
                    _ReturnValue = "#6B1C77";
                    break;
                case 6:
                    _ReturnValue = "#0187D0";
                    break;
                case 7:
                    _ReturnValue = "#585858";
                    break;
                default:
                    break;
            }

            return _ReturnValue;
        }

        #endregion
    }
}
