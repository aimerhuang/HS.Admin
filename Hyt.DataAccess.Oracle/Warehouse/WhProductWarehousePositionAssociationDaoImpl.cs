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
    /// 取仓库库位产品关联表数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class WhProductWarehousePositionAssociationDaoImpl : IWhProductWarehousePositionAssociationDao
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override int Insert(WhProductWarehousePositionAssociation entity)
        {
            entity.SysNo = Context.Insert("WhProductWarehousePositionAssociation", entity)
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
        public override void Update(WhProductWarehousePositionAssociation entity)
        {
            Context.Update("WhProductWarehousePositionAssociation", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 获取关联列表
        /// </summary>
        /// <param name="productStockSysNo">库存系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        public override IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(int productStockSysNo, int warehouseSysNo)
        {
            IList<CBWhProductWarehousePositionAssociation> list = new List<CBWhProductWarehousePositionAssociation>();
            if (warehouseSysNo > 0 && productStockSysNo > 0)
            {
                list = Context.Select<CBWhProductWarehousePositionAssociation>("wpa.SysNo as AssociationSysNo,wp.SysNo as PositionSysNo,wp.WarehousePositionName")
                    .From("WhWarehousePosition wp left join WhProductWarehousePositionAssociation wpa on wp.SysNo = wpa.WarehousePositionSysNo and wpa.ProductStockSysNo = @ProductStockSysNo")
                    .Where("wp.WarehouseSysNo=@WarehouseSysNo and wp.Status = 1")
                    .Parameter("ProductStockSysNo", productStockSysNo)
                    .Parameter("WarehouseSysNo", warehouseSysNo)
                    .OrderBy("wpa.LastUpdateDate desc,wp.SysNo asc")
                    .QueryMany();
            }
            return list;
        }

        /// <summary>
        /// 保存库位关联列表信息
        /// </summary>
        /// <param name="sysNo">库存编号</param>
        /// <returns>库位关联列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override bool SetWarehousePositionAssociations(int sysNo, IList<WhProductWarehousePositionAssociation> listPositionAssociations)
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
                        foreach (WhProductWarehousePositionAssociation entity in listPositionAssociations)
                        {
                            if (entity.SysNo > 0)
                            {
                                _context.Update<WhProductWarehousePositionAssociation>("WhProductWarehousePositionAssociation", entity)
                                       .AutoMap(x => x.SysNo)
                                       .Where("SysNo", entity.SysNo)
                                       .Execute();
                                listSysno.Add(entity.SysNo);
                            }
                            else
                            {
                                int newSysno = _context.Insert<WhProductWarehousePositionAssociation>("WhProductWarehousePositionAssociation", entity)
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
                        string Sql = string.Format("delete WhProductWarehousePositionAssociation where ProductStockSysNo = {0} and sysno not in ({1})", sysNo, listSysno.Join(","));
                        _context.Sql(Sql).Execute();
                    }
                }
                return true;
            }
            return result;
        }

        /// <summary>
        /// 获取仓库库位关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号集合</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-7-19 杨云奕 创建</remarks>
        public override IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(List<int> ProSysNos, int? warehouseSysNo)
        {
            IList<CBWhProductWarehousePositionAssociation> list = new List<CBWhProductWarehousePositionAssociation>();
            if (warehouseSysNo > 0 && ProSysNos.Count > 0)
            {
                list = Context.Select<CBWhProductWarehousePositionAssociation>("wpa.SysNo as AssociationSysNo,wp.SysNo as PositionSysNo,wp.WarehousePositionName,PdProductStock.PdProductSysNo  ")
                    .From("WhWarehousePosition wp inner join WhProductWarehousePositionAssociation wpa on wp.SysNo = wpa.WarehousePositionSysNo inner join PdProductStock on PdProductStock.SysNo= wpa.ProductStockSysNo and PdProductStock.PdProductSysNo in (" + string.Join(",", ProSysNos.ToArray()) + ")")
                    .Where("wp.WarehouseSysNo=@WarehouseSysNo and wp.Status = 1")
                    .Parameter("WarehouseSysNo", warehouseSysNo)
                    .OrderBy("wpa.LastUpdateDate desc,wp.SysNo asc")
                    .QueryMany();
            }
            return list;
        }

    }
}
