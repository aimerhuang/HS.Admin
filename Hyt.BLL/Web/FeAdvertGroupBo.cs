using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 广告组前台业务逻辑
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public class FeAdvertGroupBo : BOBase<FeAdvertGroupBo>
    {
        /// <summary>
        /// 根据广告组代码获取前台广告内容
        /// </summary>
        /// <param name="groupCode">广告组代码</param>
        /// <returns>广告项</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public IList<Model.FeAdvertItem> GetWebAdvertItems(string groupCode)
        {
            return GetWebAdvertItems(ForeStatus.广告组平台类型.PC网站, groupCode);
        }
        /// <summary>
        /// 根据广告组代码获取前台广告内容
        /// </summary>
        /// <param name="platformType">广告组平台类型</param>
        /// <param name="groupCode">广告组代码</param>
        /// <returns>广告项</returns>
        /// <param name="storeId">店铺编号默认为0</param>
        /// <param name="recordNum">读取条数99</param>
        /// <remarks>2015-12-16 杨浩 创建</remarks>
        public IList<Model.FeAdvertItem> GetWebAdvertItems(ForeStatus.广告组平台类型 platformType, string groupCode, int storeId = 0, int recordNum = 99)
        {
            var items = (List<FeAdvertItem>)CacheManager.Get(CacheKeys.Items.WebAdvertItem_, groupCode, () =>
            {
                var images = (List<FeAdvertItem>)DataAccess.Web.IFeAdvertItemDao.Instance.GetAdvertItems(platformType, groupCode);

                //格式化图片地址
                images.ForEach(o => o.ImageUrl = Hyt.BLL.Web.ProductImageBo.Instance.GetFeAdvertImagePath(o.ImageUrl));

                return images;
            }
                );
            //挑选出满足时间的项
            var nowDateTime = DateTime.Now;
            if (items != null)
            {
                //items = items.FindAll(o =>
                //{
                //    return o.BeginDate <= nowDateTime && o.EndDate >= nowDateTime && o.DealerSysNo == storeId;
                //});
                if (BLL.Config.Config.Instance.GetGeneralConfig().IsDealerMall == 1)
                {
                    return items.Where(o => o.BeginDate <= nowDateTime && o.EndDate >= nowDateTime).Take(recordNum).ToList();
                }
                return items.Where(o => o.BeginDate <= nowDateTime && o.EndDate >= nowDateTime && o.DealerSysNo == storeId).Take(recordNum).ToList();
            }
            return items;
        }
        /// <summary>
        /// 根据广告组代码获取前台广告内容
        /// </summary>
        /// <param name="platformType">广告组平台类型</param>
        /// <param name="groupCode">广告组代码</param>
        /// <returns>广告项</returns>
        /// <remarks>2013-08-21 周瑜 创建</remarks>
        public IList<Model.FeAdvertItem> GetWebAdvertItems(ForeStatus.广告组平台类型 platformType, string groupCode)
        {
            var items = (List<FeAdvertItem>)CacheManager.Get(CacheKeys.Items.WebAdvertItem_, groupCode, () =>
                {
                    return DataAccess.Web.IFeAdvertItemDao.Instance.GetAdvertItems(platformType, groupCode);
                }
                );
            //挑选出满足时间的项
            var nowDateTime = DateTime.Now;
            if (items != null)
            {
                items = items.FindAll(o =>
                {
                    return o.BeginDate <= nowDateTime && o.EndDate >= nowDateTime;
                });
                //格式化图片地址
                items.ForEach(o => o.ImageUrl = Hyt.BLL.Web.ProductImageBo.Instance.GetFeAdvertImagePath(o.ImageUrl));
            }
            return items;
        }

        public IList<Model.FeAdvertItem> GetWebAdvertItemsByGroupSysNo(int groupSysNo)
        {
            return DataAccess.Web.IFeAdvertItemDao.Instance.GetWebAdvertItemsByGroupSysNo(groupSysNo);
        }
    }
}
