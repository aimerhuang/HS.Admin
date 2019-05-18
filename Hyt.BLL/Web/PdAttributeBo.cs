using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品属性业务逻辑
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    public class PdAttributeBo : BOBase<PdAttributeBo>
    {
        /// <summary>
        /// 根据分类编号获取分类下所有可作为搜索选项的属性以及属性选项列表
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="isfilterOverTwoOptions">是否过滤超过2个选项值得属性</param>
        /// <return>属性及属性选项列表</return>
        /// <remarks>2013-08-22 黄波 创建</remarks>
        public IList<SearchAttributeAndOptions> GetSearchAttributeAndOptions(int categorySysNo,bool isfilterOverTwoOptions = false)
        {
            IList<SearchAttributeAndOptions> reslut = new List<SearchAttributeAndOptions>();
            //如果没有分类将返回空列表
            if (categorySysNo != 0)
            {
                reslut= CacheManager.Get(CacheKeys.Items.SearchCategoryAttributeOptionsList_, categorySysNo.ToString(),
                                        delegate
                                        {
                                            var tempAttribList = Hyt.DataAccess.Web.IPdAttributeDao.Instance.GetSearchAttributeAndOptions(categorySysNo);

                                            if (isfilterOverTwoOptions)
                                            {
                                                //过滤掉属性值为空或只有一个参数的对象
                                                IList<SearchAttributeAndOptions> optionsOne = tempAttribList.Where(o => o.Options.Count < 2).ToList();

                                                //循环移除被过滤的集合
                                                foreach (var searchAttributeAndOptionse in optionsOne)
                                                {
                                                    tempAttribList.Remove(searchAttributeAndOptionse);
                                                }
                                            }

                                            return tempAttribList;
                                        });
              
            }

            return reslut;

        }
    }
}
