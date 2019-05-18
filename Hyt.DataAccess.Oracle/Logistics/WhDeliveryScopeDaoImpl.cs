using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Logistics;
using Hyt.Model.Transfer;
using System.Collections;
using Hyt.Model.SystemPredefined;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 仓库当日达配送范围
    /// </summary>
    /// <remarks>2014-10-09  朱成果 创建</remarks>
    public class WhDeliveryScopeDaoImpl : IWhDeliveryScopeDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override int Insert(WhDeliveryScope entity)
        {
            entity.SysNo = Context.Insert("WhDeliveryScope", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override void Update(WhDeliveryScope entity)
        {

            Context.Update("WhDeliveryScope", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override WhDeliveryScope GetEntity(int sysNo)
        {

            return Context.Sql("select * from WhDeliveryScope where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<WhDeliveryScope>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from WhDeliveryScope where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 根据仓库编号删除数据
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override  void DeleteByWarehouseSysNo(int warehouseSysNo)
        {
            Context.Sql("Delete from WhDeliveryScope where WarehouseSysNo=@WarehouseSysNo")
                 .Parameter("WarehouseSysNo", warehouseSysNo)
            .Execute();
        }

        /// <summary>
        /// 获取仓库配送区域列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override List<CBWhDeliveryScope> GetList()
        {
            return Context.Sql(@"select a.*,
                                c.SysNo as AreaNo,c.AreaName,
                                d.SysNo as CityNo,d.AreaName as CityName,
                                b.WarehouseName 
                                from
                                   WhDeliveryScope a
                                   inner  join WhWarehouse b
                                   on a.warehousesysno=b.sysno
                                   left outer join BsArea c
                                   on c.SysNo=b.areasysno
                                   left outer join BsArea d
                                   on c.ParentSysNo=d.SysNo
                                   where b.status=1 and exists(select 1 from WhWarehouseDeliveryType w where w.warehousesysno=b.sysno and w.deliverytypesysno=@deliverytypesysno)
                             ")
                              .Parameter("deliverytypesysno", DeliveryType.小区配送)
                              .QueryMany<CBWhDeliveryScope>();
        }

        /// <summary>
        /// 根据条件筛选仓库来设置配送范围
        /// </summary>
        /// <param name="cityNo">所在城市编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <param name="deliveryTypeSysNo">配送方式</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public override List<WhWarehouse> GetWhWarehouseForDeliveryScope(int cityNo,int? warehouseType, int? isSelfSupport, int? deliveryTypeSysNo)
        {
            string from = @"WhWarehouse p
                            left outer join BsArea b 
                            on p.areasysno=b.sysno";
            string where = " p.Status=1 ";
            var paras = new ArrayList();
            where += " and b.AreaCode=@p0p0";
            paras.Add(cityNo);
            if(warehouseType.HasValue)
            {
                where += " and p.WarehouseType=@p0p1";
                paras.Add(warehouseType.Value);
            }
            if (isSelfSupport.HasValue)
            {
                where += " and p.IsSelfSupport=@p0p2";
                paras.Add(isSelfSupport.Value);
            }
            if(deliveryTypeSysNo.HasValue)
            {
                where += " and  exists(select 1 from WhWarehouseDeliveryType w where w.warehousesysno=p.sysno and w.deliverytypesysno=@p0p3)";
                paras.Add(deliveryTypeSysNo.Value);
            }
          return  
                Context.Select<WhWarehouse>("p.*")
                .From(from)
                .Where(where)
                .Parameters(paras).QueryMany();
        }
        #endregion

    }
}
