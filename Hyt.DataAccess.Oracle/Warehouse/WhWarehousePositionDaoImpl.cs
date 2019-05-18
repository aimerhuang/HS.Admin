using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 取仓库库位数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class WhWarehousePositionDaoImpl : IWhWarehousePositionDao
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override int Insert(WhWarehousePosition entity)
        {
            entity.SysNo = Context.Insert("WhWarehousePosition", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override void Update(WhWarehousePosition entity)
        {
            Context.Update("WhWarehousePosition", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取仓库库位
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override IList<WhWarehousePosition> GetWarehousePositions(int warehouseSysNo)
        {
            IList<WhWarehousePosition> list = new List<WhWarehousePosition>();
            if (warehouseSysNo > 0)
            {
                list = Context.Select<WhWarehousePosition>("*")
                    .From("WhWarehousePosition")
                    .Where("WarehouseSysNo=@WarehouseSysNo")
                    .Parameter("WarehouseSysNo", warehouseSysNo)
                    .OrderBy("LastUpdateDate desc")
                    .QueryMany();
            }
            return list;
        }

        /// <summary>
        /// 获取出库商品库位列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="warehouseSysNo">商品编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override IList<WhWarehousePosition> GetWPositionsByWsysNoAndProSysNo(int warehouseSysNo, int productSysNo)
        {

            return Context.Sql(@"select * from WhWarehousePosition
                        where SysNo in
                        (
                        select WarehousePositionSysNo from WhProductWarehousePositionAssociation
                        where ProductStockSysNo in 
                        (
                        select SysNo from PdProductStock 
                        where WarehouseSysNo = @WarehouseSysNo and PdProductSysNo = @PdProductSysNo
                        )
                        )")
                .Parameter("WarehouseSysNo", warehouseSysNo)
                .Parameter("PdProductSysNo", productSysNo)
                 .QueryMany<WhWarehousePosition>();

        }

        /// <summary>
        /// 保存仓库库位列表信息
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override bool SetWarehousePositions(int sysNo, IList<WhWarehousePosition> listWarehousePositions)
        {
            bool result = false;
            if (sysNo > 0)
            {
                using (var _context = Context.UseSharedConnection(true))
                {
                    if (sysNo > 0)
                    {
                        IList<int> listSysno = new List<int>();
                        //再添加
                        foreach (WhWarehousePosition entity in listWarehousePositions)
                        {
                            if (entity.SysNo > 0)
                            {
                                _context.Update<WhWarehousePosition>("WhWarehousePosition", entity)
                                       .AutoMap(x => x.SysNo)
                                       .Where("SysNo", entity.SysNo)
                                       .Execute();
                                listSysno.Add(entity.SysNo);
                            }
                            else
                            {
                                int newSysno = _context.Insert<WhWarehousePosition>("WhWarehousePosition", entity)
                                                      .AutoMap(x => x.SysNo)
                                                      .ExecuteReturnLastId<int>("SysNo");
                                listSysno.Add(newSysno);
                            }
                        }
                        ////再删除
                        if (listSysno.Count == 0)
                        {
                            listSysno.Add(0);
                        }
                        string Sql = string.Format("delete WhWarehousePosition where WarehouseSysNo = {0} and sysno not in ({1})", sysNo, listSysno.Join(","));
                        _context.Sql(Sql).Execute();
                    }
                }
                return true;
            }
            return result;
        }
    }
}
