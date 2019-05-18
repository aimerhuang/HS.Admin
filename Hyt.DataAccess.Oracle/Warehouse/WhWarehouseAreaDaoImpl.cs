using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 仓库覆盖地区信息数据访问类
    /// </summary>
    /// <remarks>2013-08-12 周瑜 创建</remarks>
    public class WhWarehouseAreaDaoImpl : IWhWarehouseAreaDao
    {
        /// <summary>
        /// 根据地区编号查询覆盖该地区的所有仓库
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <returns>新增记录的系统编号</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        public override IList<CBWhWarehouse> GetWarehouseForArea(int areaSysNo)
        {
            return
                Context.Sql(
                    @"select a.areasysno,b.sysno,b.warehousename,b.backwarehousename,b.streetaddress,a.isdefault,c.areaname,d.areaname as CityName,e.areaname as ProvinceName 
from whwarehousearea a 
inner join whwarehouse b on a.warehousesysno = b.sysno 
inner join bsarea c on a.areasysno = c.sysno
inner join bsarea d on c.parentsysno = d.sysno
inner join bsarea e on d.parentsysno = e.sysno
where a.areasysno = @areasysno")
                       .Parameter("areasysno", areaSysNo)
                       .QueryMany<CBWhWarehouse>();
        }

        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>新增记录的系统编号</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public override int Insert(WhWarehouseArea warehouse)
        {

            int id = 0;
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                var count =
                    context.Sql(
                        "select count(*) from whwarehousearea where areasysno = @areasysno and warehousesysno = @warehousesysno")
                           .Parameter("areasysno", warehouse.AreaSysNo)
                           .Parameter("warehousesysno", warehouse.WarehouseSysNo)
                           .QuerySingle<int>();
                if (count == 0)
                {
                    id = context.Insert("WhWarehouseArea", warehouse)
                .AutoMap(x => x.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
                }

            }
            return id;
        }

        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public override int Update(WhWarehouseArea warehouse)
        {
            return Context.Update("WhWarehouseArea", warehouse)
                   .AutoMap(x => x.SysNo)
                   .Where(x => x.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 删除仓库覆盖地区
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public override int Delete(int areaSysNo, int warehouseSysNo)
        {
            return Context.Delete("WhWarehouseArea")
                          .Where("AreaSysNo", areaSysNo)
                          .Where("WarehouseSysNo", warehouseSysNo)
                          .Execute();
        }

        /// <summary>
        /// 将该仓库设为选中地区的默认发货仓库
        /// </summary>
        /// <param name="whWarehouseArea">地区仓库实体</param>
        /// <param name="status">是否默认仓库，默认：是</param>
        /// <returns>是否设置成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        /// <remarks>2013-11-06 郑荣华 重构</remarks>
        public override int SetDefault(WhWarehouseArea whWarehouseArea, WarehouseStatus.是否默认仓库 status)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                if (status == WarehouseStatus.是否默认仓库.是)
                {
                    const string sql = @"update WhWarehouseArea set IsDefault = @status ,
lastupdateby = @lastupdateby, lastupdatedate = @lastupdatedate
where areasysno = @areasysno ";
                    //先全改为否
                    context.Sql(sql)
                           .Parameter("status", (int)WarehouseStatus.是否默认仓库.否)
                           .Parameter("lastupdateby", whWarehouseArea.LastUpdateBy)
                           .Parameter("lastupdatedate", whWarehouseArea.LastUpdateDate)
                           .Parameter("areasysno", whWarehouseArea.AreaSysNo)
                           .Execute();
                }

                const string strSql = @"update WhWarehouseArea set IsDefault = @status ,
lastupdateby = @lastupdateby, lastupdatedate = @lastupdatedate
where areasysno = @areasysno and warehousesysno = @warehousesysno";
                return context.Sql(strSql)
                      .Parameter("status", (int)status)
                      .Parameter("lastupdateby", whWarehouseArea.LastUpdateBy)
                      .Parameter("lastupdatedate", whWarehouseArea.LastUpdateDate)
                      .Parameter("areasysno", whWarehouseArea.AreaSysNo)
                      .Parameter("warehousesysno", whWarehouseArea.WarehouseSysNo)
                      .Execute();

            }

        }

        /// <summary>
        /// 获取所有仓库覆盖地区
        /// </summary>
        /// <returns>所有的仓库覆盖地区</returns>
        /// <remarks>2014-05-15 朱成果 创建</remarks>
        public override  IList<WhWarehouseArea> GetAllWhWarehouseArea()
        {
           return  Context.Sql("select * from WhWarehouseArea").QueryMany<WhWarehouseArea>();
        }
    }
}
