using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 投诉业务逻辑层
    /// </summary>
    /// <remarks>2013－08-05 苟治国 创建</remarks>
    public class ComplaintBo : BOBase<ComplaintBo>
    {
        /// <summary>
        /// 获取指定会员订单列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>会员订单列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public PagedList<SoOrder> GetOrder(int pageIndex, int customersysno, DateTime beginTime, DateTime endTime)
        {
            var list = new PagedList<SoOrder>();
            var pager = new Pager<SoOrder>();

            var orderFilter = new ParaOrderFilter();
            orderFilter.BeginDate = beginTime;
            orderFilter.EndDate = endTime;

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new SoOrder
            {
                Status = 100,
                CustomerSysNo = (int)customersysno
            };
            pager.PageSize = list.PageSize;
            pager = IComplaintDao.Instance.GetOrder(pager, orderFilter);
            list = new PagedList<SoOrder>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                IsLoading = false,
                Style = PagedList.StyleEnum.WebSmall
            };
            return list;
        }

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pageIndex">会员投诉查询条件</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="status">状态</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public PagedList<Model.CBCrComplaint> GetComplaintList(int pageIndex, int customersysno, int? status)
        {
            var list = new PagedList<CBCrComplaint>();
            var pager = new Pager<CBCrComplaint>();

            if (status == null)
            {
                status = 10;
            }

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBCrComplaint
            {
                CustomerSysNo = (int)customersysno,
                Status = (int)status
            };
            pager.PageSize = list.PageSize;
            pager = IComplaintDao.Instance.GetComplaintList(pager);
            list = new PagedList<CBCrComplaint>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                IsLoading = false,
                Style = PagedList.StyleEnum.WebSmall
            };
            return list;
        }

        /// <summary>
        /// 根据订单号获取产品图片
        /// </summary>
        /// <param name="ordersysNo">订单编号</param>
        /// <returns>订单所属图片集</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public IList<Model.PdProduct> GetProductImage(int ordersysNo)
        {
            return IComplaintDao.Instance.GetProductImage(ordersysNo);
        }

        /// <summary>
        /// 获取投诉信息
        /// </summary>
        /// <param name="sysNo">投诉系统编号</param>
        /// <returns>投诉信息</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public CBCrComplaint GetModel(int sysNo)
        {
            return BLL.CRM.CrComplaintBo.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="sysNo">投诉系统编号</param>
        /// <returns>投诉信息</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public SoOrder GetOrderModel(int sysNo)
        {
            return Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public int Insert(Model.CrComplaint model)
        {
            return IComplaintDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public int Update(Model.CrComplaint model)
        {
            return IComplaintDao.Instance.Update(model);
        }
    }
}
