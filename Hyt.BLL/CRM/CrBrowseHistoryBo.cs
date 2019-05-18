using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 商品浏览记录业务类
    /// </summary>
    /// <remarks>
    /// 2013-09-10 郑荣华 创建
    /// </remarks>
    public class CrBrowseHistoryBo : BOBase<CrBrowseHistoryBo>
    {
        /// <summary>
        /// 新加商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public int Create(CrBrowseHistory model)
        {
            return ICrBrowseHistoryDao.Instance.Create(model);
        }

        /// <summary>
        /// 更新商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public int Update(CrBrowseHistory model)
        {
            return ICrBrowseHistoryDao.Instance.Update(model);
        }

        /// <summary>
        /// 获取商品浏览记录
        /// </summary>
        /// <param name="filter">商品浏览历史查询条件实体</param>
        /// <returns>商品浏览记录</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public CrBrowseHistory GetCrBrowseHistory(ParaCrBrowseHistoryFilter filter)
        {
            return ICrBrowseHistoryDao.Instance.GetCrBrowseHistory(filter);
        }

        /// <summary>
        /// 有则浏览记录数+1，无则新加
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public bool AddCrBrowseHistory(CrBrowseHistory model)
        {
            var filter = new ParaCrBrowseHistoryFilter
                {
                    BrowseType = model.BrowseType,
                    CategorySysNo = model.CategorySysNo,
                    CustomerSysNo = model.CustomerSysNo,
                    IsPageDown = model.IsPageDown,
                    KeyWord = model.KeyWord ?? "",
                    ProductSysNo = model.ProductSysNo
                };//所有查询字段不能为null

            var r = GetCrBrowseHistory(filter);
            if (null == r)
            {
                //新加商品浏览记录
                model.BrowseNum = 1;
                return Create(model) > 0;
            }
            //浏览次数加1
            model.BrowseNum = r.BrowseNum + 1;
            model.SysNo = r.SysNo;
            return Update(model) > 0;

        }

    }
}
