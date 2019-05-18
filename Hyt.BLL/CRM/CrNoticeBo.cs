using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 商品信息通知业务类
    /// </summary>
    /// <remarks>2013-08-09 杨晗 创建</remarks>
    public class CrNoticeBo : BOBase<CrNoticeBo>
    {
        /// <summary>
        /// 根据商品信息通知编号获取实体
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>商品信息通知实体</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public CrNotice GetModel(int sysNo)
        {
            return ICrNoticeDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="customerSysNo">商品信息通知类型</param>
        /// <param name="type">商品信息通知类型</param>
        /// <returns>商品信息通知列表</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public PagedList<CBCrNotice> Seach(int? pageIndex,int customerSysNo, CustomerStatus.通知类型 type)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBCrNotice>();
            var count = 0;
            var list = ICrNoticeDao.Instance.Seach((int)pageIndex, model.PageSize,customerSysNo, type, out count);

            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int)pageIndex;
            model.Style = PagedList.StyleEnum.WebSmall; 
            return model;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="CustomerSysNo">申请人</param>
        /// <param name="ProductSysNo">申请商品ID</param>
        /// <param name="type">通知方式</param>
        /// <param name="NoticeWay">发送方式</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏创建</returns>
        public IList<CBCrNotice> List(int CustomerSysNo, int ProductSysNo, CustomerStatus.通知类型 type, int NoticeWay,PagedList<CBCrNotice> pager)
        {

            Pager<CBCrNotice> transPager = new Pager<CBCrNotice>();
            transPager.CurrentPage = pager.CurrentPageIndex;
            transPager.PageSize = pager.PageSize;
            ICrNoticeDao.Instance.List(CustomerSysNo, ProductSysNo, type, NoticeWay, transPager);
            pager.TData = transPager.Rows;
            pager.TotalItemCount = transPager.TotalRows;
            return pager.TData;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public int Insert(CrNotice model)
        {
            return ICrNoticeDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public int Update(CrNotice model)
        {
            return ICrNoticeDao.Instance.Update(model);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public int Update(string sysNoItems)
        {
            return ICrNoticeDao.Instance.Update(sysNoItems);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return ICrNoticeDao.Instance.Delete(sysNo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CBCrNotice Get()
        {
            return ICrNoticeDao.Instance.Get();
        }
    }
}
