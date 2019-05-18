using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Infrastructure.Pager;
using Hyt.DataAccess.Tuan;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Caching;

namespace Hyt.BLL.Tuan
{
    /// <summary>
    /// 团购业务逻辑
    /// </summary>
    /// <remarks>2013-09-02 黄波 创建</remarks>
    public class GsGroupShoppingBo : BOBase<GsGroupShoppingBo>
    {
        /// <summary>
        /// 获取一条团购记录
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-09-05 苟治国 创建</remarks>
        public GsGroupShopping Get(int sysNo)
        {
            return Hyt.BLL.Promotion.GroupShoppingBo.Instance.Get(sysNo);
            //return CacheManager.Get<Model.GsGroupShopping>(CacheKeys.Items.GroupShoppingDetail_, sysNo.ToString(), delegate()
            //{
            //    return Hyt.BLL.Promotion.GroupShoppingBo.Instance.Get(sysNo);
            //});
        }

        /// <summary>
        /// 今日团购推荐列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public PagedList<GsGroupShopping> GetToDay(int pageIndex,int pageSize, DateTime? startTime, DateTime? endTime)
        {
            var pager = new Pager<GsGroupShopping>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new GsGroupShopping
            {
                GroupType = (int)GroupShoppingStatus.团购类型.商城,
                Status = (int)GroupShoppingStatus.团购状态.已审,
                StartTime = startTime,
                EndTime = endTime
            };
            pager.PageSize = pageSize;
            pager = IGsGroupShoppingDao.Instance.GetPagingList(pager);

            var list = new PagedList<GsGroupShopping>();
            list = new PagedList<GsGroupShopping>
            {
                PageSize = pageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return list;
        }

        /// <summary>
        /// 团购分页列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public PagedList<GsGroupShopping> GetList(int pageIndex,int pageSize, DateTime? startTime, DateTime? endTime)
        {
            var list = new PagedList<GsGroupShopping>();
            var pager = new Pager<GsGroupShopping>();

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new GsGroupShopping
            {
                GroupType = (int)GroupShoppingStatus.团购类型.商城,
                Status = (int)GroupShoppingStatus.团购状态.已审,
                StartTime = startTime,
                EndTime = endTime
            };

            pager.PageSize = pageSize;
            pager = IGsGroupShoppingDao.Instance.GetList(pager);

            list = new PagedList<GsGroupShopping>
            {
                PageSize = pageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
               
               
            };
            return list;
        }

        /// <summary>
        /// 根据截至时间获取已审核团购分页列表 
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="groupType"></param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        public PagedList<GsGroupShopping> GetGroupShoppingList(int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, GroupShoppingStatus.团购类型 groupType)
        {
            var list = new PagedList<GsGroupShopping>();
            var pager = new Pager<GsGroupShopping>();

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new GsGroupShopping
            {
                GroupType = (int)groupType,
                Status = (int)PromotionStatus.促销状态.已审,
                StartTime = startTime,
                EndTime = endTime
            };

            pager.PageSize = pageSize;
            pager = IGsGroupShoppingDao.Instance.GetGroupShoppingList(pager);

            list = new PagedList<GsGroupShopping>
            {
                PageSize = pageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows

            };
            return list;
        }

        /// <summary>
        /// 获取团购数量
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>团购数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public int GetCount(DateTime? startTime, DateTime? endTime)
        {
            return IGsGroupShoppingDao.Instance.GetCount(GroupShoppingStatus.团购类型.商城, GroupShoppingStatus.团购状态.已审,
                                                         startTime, endTime);
        }

        /// <summary>
        /// 为手机客户端查询最新的一条团购
        /// </summary>
        /// <returns>最新的一条团购信息</returns>
        /// <remarks>2013-09-24 周瑜 创建</remarks>
        public GsGroupShopping GetNewGroupShoppingForApp()
        {
            return IGsGroupShoppingDao.Instance.GetNewGroupShoppingForApp();
        }

        /// <summary>
        /// 今日推荐数量
        /// </summary>
        /// <returns>数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public int TodayCount()
        {
            DateTime startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 0:0:0"));
            DateTime endTime = DateTime.Now;
            var model = GetToDay(1, 9, startTime, endTime);
            return model.TotalItemCount;        
        }

        /// <summary>
        /// 往日团购数量
        /// </summary>
        /// <returns>数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public int ForeCount()
        {
            DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 0:0:0"));
            return GetCount(null, endTime);
        }

        /// <summary>
        /// 即将开团数量
        /// </summary>
        /// <returns>数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public int SoonCount()
        {
            DateTime startTime = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 0:0:0"));
            return GetCount(startTime, null);
        }

        /// <summary>
        /// 根据商品系统编号 获取团购编号
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>团购编号</returns>
        /// <remarks>2013-09-10 杨浩 创建</remarks>
        public int GetGroupSysNo(int productSysNo)
        {
            return IGsGroupShoppingDao.Instance.GetGroupShoppingSysNoByProduct(productSysNo);
        }

        /// <summary>
        /// 根据商品系统编号 获取团购价格
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns>团购价格</returns>
        /// <remarks>2013-10-31 杨浩 创建</remarks>
        public decimal GetGroupShoppingPrice(int sysNo)
        {
            return IGsGroupShoppingDao.Instance.GetGroupShoppingPrice(sysNo);
        }
    }
}
