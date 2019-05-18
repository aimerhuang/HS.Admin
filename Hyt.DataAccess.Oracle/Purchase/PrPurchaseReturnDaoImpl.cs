using Hyt.DataAccess.Purchase;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Purchase
{
    /// <summary>
    /// 采购退货单
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class PrPurchaseReturnDaoImpl : IPrPurchaseReturnDao
    {
        /// <summary>
        /// 添加采购退货单
        /// </summary>
        /// <param name="PurchaseReturn">采购退货单实体类对象</param>
        /// <returns></returns>
        public override int AddPurchaseReturn(PrPurchaseReturn PurchaseReturn)
        {
            PurchaseReturn.SysNo = Context.Insert("PrPurchaseReturn", PurchaseReturn)
                                       .AutoMap(o => o.SysNo, o => o.PurchaseReturnDetails)
                                       .ExecuteReturnLastId<int>("SysNo");
            return PurchaseReturn.SysNo;
        }

        /// <summary>
        /// 更新采购退货单
        /// </summary>
        /// <param name="purchase">采购退货单实体类对象</param>
        /// <returns></returns>
        public override int UpdatePurchaseReturn(PrPurchaseReturn PurchaseReturn)
        {
            int rows = Context.Update("PrPurchaseReturn", PurchaseReturn)
                   .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate, o => o.Status, o => o.PurchaseReturnDetails)
                   .Where("SysNo", PurchaseReturn.SysNo)
                   .Execute();
            return rows;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  杨浩 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from PrPurchaseReturn where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNos"></param>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public override void Delete(string sysNos)
        {
            Context.Sql("Delete from PrPurchaseReturn where SysNo in(" + sysNos + ")")
            .Execute();
        }
        /// <summary>
        /// 获取采购退货单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public override PrPurchaseReturn GetPrPurchaseReturn(int sysNo)
        {
            return Context.Sql("select * from PrPurchaseReturn where SysNo=@SysNo")
                  .Parameter("SysNo", sysNo)
             .QuerySingle<PrPurchaseReturn>();
        }
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 增加查询条件</remarks>
        public override Pager<CBPrPurchaseReturn> QueryPrPurchaseReturn(ParaPrPurchaseReturnFilter para)
        {
            var paras = new List<object>();

            string whereStr = " where 1=1 ";
            if (para.WarehouseSysNo > 0)
            {
                whereStr += " and phr.WarehouseSysNo=@" + paras.Count;
                paras.Add(para.WarehouseSysNo);
            }
            if (!string.IsNullOrEmpty(para.PurchaseCode) && para.PurchaseCode != "")
            {
                whereStr += " and ph.PurchaseCode=@" + paras.Count;
                paras.Add(para.PurchaseCode);
            }
            if (para.Status != 0)
            {
                whereStr += " and phr.Status=@" + paras.Count;
                paras.Add(para.Status);
            }

            //if (para.CreatedDate.HasValue)
            //{
            //    whereStr += " and phr.CreatedDate=@" + paras.Count;
            //    paras.Add(para.CreatedDate);
            //}

            string sql = @"
              (
              select phr.*,wh.BackWarehouseName,ph.PurchaseCode from PrPurchaseReturn as phr 
                left join WhWarehouse as wh on wh.SysNo=phr.WarehouseSysNo
				left join PrPurchase ph on phr.PurchaseSysNo = ph.SysNo " + whereStr + ") tb";



            var dataList = Context.Select<CBPrPurchaseReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras.ToArray());
            dataCount.Parameters(paras.ToArray());
            var pager = new Pager<CBPrPurchaseReturn>
            {
                PageSize = para.PageSize,
                CurrentPage = para.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(para.Id, para.PageSize).QueryMany()
            };
            return pager;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <param name="statusType">状态 :待审核（10）、已审核（20）、作废（-10）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override bool UpdateStatus(int sysNo, int status)
        {
            int rows = Context.Sql("Update PrPurchaseReturn set Status = @Status where SysNo=@SysNo")
                .Parameter("Status", status)
                .Parameter("SysNo", sysNo)
             .Execute();
            return rows > 0;
        }
        /// <summary>
        /// 更新采购退货单已出库数
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public override bool UpdateOutQuantity(int sysNo)
        {
            return Context.Sql(string.Format(@"
            update PrPurchaseReturn set OutQuantity = 
            (
               select sum(OutQuantity) from PrPurchaseReturnDetails where PurchaseReturnSysNo = {0}
            )
            where SysNo ={1}", sysNo, sysNo))
            .Execute() > 0;
        }
    }
}
