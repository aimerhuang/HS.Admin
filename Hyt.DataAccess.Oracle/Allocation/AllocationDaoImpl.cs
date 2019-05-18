using Hyt.DataAccess.Allocation;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hyt.DataAccess.Oracle.Allocation
{
    /// <summary>
    /// 调拨单
    /// </summary>
    /// <remarks>2016-6-23 杨浩 创建</remarks>
    public class AllocationDaoImpl : IAllocationDao
    {
        /// <summary>
        /// 添加调拨单
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public override int Add(AtAllocation model)
        {
            model.SysNo = Context.Insert("AtAllocation", model)
                                    .AutoMap(o => o.SysNo)
                                    .ExecuteReturnLastId<int>("SysNo");
            return model.SysNo;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public override int Update(AtAllocation model)
        {
            int rows = Context.Update("AtAllocation", model)
                  .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate)
                  .Where("SysNo", model.SysNo)
                  .Execute();
            return rows;
        }
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public override Pager<CBAtAllocation> Query(ParaAtAllocationFilter para)
        {
            var paras = new List<object>();

            string whereStr = " where 1=1 ";
            //if (para.WarehouseSysNo > 0)
            //{
            //    whereStr += " and ph.WarehouseSysNo=@" + paras.Count;
            //    paras.Add(para.WarehouseSysNo);
            //}
            //if (!string.IsNullOrEmpty(para.PurchaseCode) && para.PurchaseCode != "")
            //{
            //    whereStr += " and ph.PurchaseCode=@" + paras.Count;
            //    paras.Add(para.PurchaseCode);
            //}
            //if (para.Status != 0)
            //{
            //    whereStr += " and ph.Status=@" + paras.Count;
            //    paras.Add(para.Status);
            //}

            //if (para.CreatedDate.HasValue)
            //{
            //    whereStr += " and ph.CreatedDate=@" + paras.Count;
            //    paras.Add(para.CreatedDate);
            //}

            string sql = @"
              (
              select aa.* from AtAllocation as aa  " + whereStr + ") tb";

            var dataList = Context.Select<CBAtAllocation>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras.ToArray());
            dataCount.Parameters(paras.ToArray());
            var pager = new Pager<CBAtAllocation>
            {
                PageSize = para.PageSize,
                CurrentPage = para.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(para.Id, para.PageSize).QueryMany()
            };
            return pager;
        }
    }
}
