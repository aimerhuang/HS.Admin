using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 取件单业务类
    /// </summary>
    /// <remarks>
    /// 2013-07-04 郑荣华 创建
    /// </remarks>
    public class LgPickUpBo : BOBase<LgPickUpBo>
    {

        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="filter">查询条件实体</param>
        /// <returns>返回取件单列表</returns>
        /// <remarks>2013-08-12 周唐炬 创建</remarks>
        public PagedList<LgPickUp> GetPickUpList(ParaPickUpFilter filter)
        {
            PagedList<LgPickUp> model = null;
            if (filter != null)
            {
                model = new PagedList<LgPickUp>();
                var pager = ILgPickUpDao.Instance.GetPickUpList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
            }
            return model;
        }

        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="filter">查询条件实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-04 郑荣华 创建
        /// </remarks>
        public void GetPickUpList(ref Pager<CBLgPickUp> pager, ParaPickUpFilter filter)
        {
            ILgPickUpDao.Instance.GetPickUpList(ref pager, filter);

        }

        /// <summary>
        /// 修改取件单状态
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号.</param>
        /// <param name="status">状态.</param>
        /// <param name="operatorSysNo">The operator sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-14 何方 创建
        /// </remarks>
        public bool UpdatePickUpStatus(int pickUpSysNo, LogisticsStatus.取件单状态 status, int operatorSysNo)
        {
            return ILgPickUpDao.Instance.UpdateStatus(pickUpSysNo, status);

        }

        /// <summary>
        /// 获取取件单
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号.</param>
        /// <returns>返回取件单实体</returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public LgPickUp GetPickUp(int pickUpSysNo)
        {
            return ILgPickUpDao.Instance.GetPickUp(pickUpSysNo);
        }

        /// <summary>
        /// 获取取件单商品项
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号</param>
        /// <returns>取件单商品项</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public List<LgPickUpItem> GetLgPickUpItem(int pickUpSysNo)
        {
            return ILgPickUpDao.Instance.GetLgPickUpItem(pickUpSysNo);
        }

        /// <summary>
        /// 获取取件单
        /// </summary>
        /// <param name="pickUpSysNos">取件单系统编号集合.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public IList<LgPickUp> GetWhStockOutListBySysNos(int[] pickUpSysNos)
        {
            return ILgPickUpDao.Instance.GetPickUp(pickUpSysNos);

        }
    }
}
