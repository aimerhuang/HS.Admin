using Hyt.DataAccess.Allocation;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Allocation
{
    /// <summary>
    /// 调拨单
    /// </summary>
    /// <remarks>2016-6-23 杨浩 创建</remarks>
    public class AllocationBo : BOBase<AllocationBo>
    {
        /// <summary>
        /// 添加调拨单
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public int Add(AtAllocation model)
        {
           return IAllocationDao.Instance.Add(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public  int Update(AtAllocation model)
        {
            return IAllocationDao.Instance.Update(model);
        }
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public PagedList<CBAtAllocation> Query(ParaAtAllocationFilter para)
        {
            PagedList<CBAtAllocation> model = null;
            if (para != null)
            {
                model = new PagedList<CBAtAllocation>();
                var pager = IAllocationDao.Instance.Query(para);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = para.Id;
                }
            }
            return model;
        }
    }
}
