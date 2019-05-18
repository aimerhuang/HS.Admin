using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    public class AtAllocationDaoImpl : IAtAllocationDao
    {
        /// <summary>
        /// 分页查询调拨单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public override Pager<CBAtAllocation> QueryAtAllocationPager(Pager<CBAtAllocation> pager)
        {
            string sqlCountText = "SELECT COUNT(1) FROM AtAllocation";
            string sqlWhere = "";
            if (pager.PageFilter.EnterWarehouseSysNo>0)
            {
                sqlWhere += " EnterWarehouseSysNo = '" + pager.PageFilter.EnterWarehouseSysNo + "' ";
            }
            if (pager.PageFilter.EnterWarehouseSysNo > 0)
            {
                if(!string.IsNullOrEmpty(sqlWhere))
                {
                    sqlWhere += " or ";
                }
                sqlWhere += " OutWarehouseSysNo = '" + pager.PageFilter.OutWarehouseSysNo + "' ";
            }
            if (string.IsNullOrEmpty(sqlWhere))
            {
                sqlWhere = " 1=1 ";
            }
            pager.Rows = Context.Select<CBAtAllocation>("*").From("AtAllocation").Where(sqlWhere).OrderBy("SysNo DESC").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Sql(sqlCountText).QuerySingle<int>();

            return pager;
        }

        /// <summary>
        /// 获取调拨单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override AtAllocation GetAtAllocationEntity(int sysNo)
        {
            return Context.Sql("SELECT * FROM AtAllocation WHERE SysNo=" + sysNo).QuerySingle<AtAllocation>();
        }

        /// <summary>
        /// 创建调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override AtAllocation CreateAtAllocation(AtAllocation model)
        {
            model.SysNo = Context.Insert<AtAllocation>("AtAllocation", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
            return model;
        }

        /// <summary>
        /// 更新调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        public override int UpdateAtAllocation(AtAllocation model)
        {
            return Context.Update("AtAllocation", model).AutoMap(o => o.SysNo).Where("SysNo", model.SysNo).Execute();
        }

        /// <summary>
        /// 新增调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override int AddAtAllocationItem(AtAllocationItem model)
        {
            return Context.Insert<AtAllocationItem>("AtAllocationItem", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取库存调拨单商品列表（添加调拨商品用）
        /// </summary>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override List<AtAllocationItem> GetAtAllocationProducts(int atAllocationSysNo)
        {
            string sqlText = "SELECT * FROM AtAllocationItem WHERE AllocationSysNo=" + atAllocationSysNo;
            return Context.Sql(sqlText).QueryMany<AtAllocationItem>();
        }

        /// <summary>
        /// 删除调拨单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override int DeleteAtAllocationItem(int sysNo)
        {
            return Context.Delete("AtAllocationItem").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 分页查询调拨单明细列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override Pager<AtAllocationItem> QueryAtAllocationItemPager(Pager<AtAllocationItem> pager)
        {
            string sqlCountText = "SELECT COUNT(1) FROM AtAllocationItem WHERE AllocationSysNo=" + pager.PageFilter.AllocationSysNo;

            pager.Rows = Context.Select<AtAllocationItem>("*").From("AtAllocationItem")
                .Where("AllocationSysNo=" + pager.PageFilter.AllocationSysNo)
                .OrderBy("SysNo DESC").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Sql(sqlCountText).QuerySingle<int>();

            return pager;
        }

        /// <summary>
        /// 更新调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public override int UpdateAtAllocationItem(AtAllocationItem model)
        {
            return Context.Update("AtAllocationItem", model).AutoMap(o => o.SysNo).Where("SysNo", model.SysNo).Execute();
        }

        public override List<DBAtAllocationItem> GetByDBAtAllocationItem(int outWareSysNo, int SysNo)
        {
            string sql = @" SELECT AtAllocationItem.*,PdProductStock.CostPrice,CostAmount=PdProductStock.CostPrice*AtAllocationItem.Quantity
                          FROM  AtAllocationItem inner join [AtAllocation] on AllocationSysNo=AtAllocation.SysNo inner join PdProductStock on AtAllocation.OutWarehouseSysNo=PdProductStock.WarehouseSysNo   and AtAllocationItem.ProductSysNo=PdProductStock.PdProductSysNo ";
            sql += " where  AllocationSysNo = '" + SysNo + "' and AtAllocation.OutWarehouseSysNo='" + outWareSysNo + "' ";

            return Context.Sql(sql).QueryMany<DBAtAllocationItem>();
        }

        public override DBAtAllocation GetDBAtAllocationEntity(int SysNo)
        {
            return Context.Sql(@"
                                SELECT AtAllocation.*,outWare.ErpCode,inWare.ErpCode
                                 FROM AtAllocation inner join WhWarehouse outWare on AtAllocation.OutWarehouseSysNo=outWare.SysNo inner join WhWarehouse inWare on AtAllocation.EnterWarehouseSysNo=inWare.SysNo  WHERE AtAllocation.SysNo=" + SysNo)
                                                                                                                                                                                                                                             .QuerySingle<DBAtAllocation>();
        }
        /// <summary>
        /// 检查调拨单产品是否有0的数量
        /// </summary>
        /// <param name="atAllocationSysNo">调拨单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public override bool ExistAtAllocationProductQtyZero(int atAllocationSysNo)
        {
            string sqlText = "SELECT count(1) FROM AtAllocationItem WHERE AllocationSysNo=@AllocationSysNo and Quantity<=0";
            return Context.Sql(sqlText)
                   .Parameter("AllocationSysNo", atAllocationSysNo)
                   .QuerySingle<int>() > 0;
        }

    }
}
