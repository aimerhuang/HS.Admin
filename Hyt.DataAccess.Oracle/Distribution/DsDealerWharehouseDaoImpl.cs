using System.Collections;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商仓库关系
    /// </summary>
    /// <remarks>2014-10-15 余勇 创建</remarks>
    public class DsDealerWharehouseDaoImpl : IDsDealerWharehouse
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public override int Insert(DsDealerWharehouse entity)
        {
            var sysNo = Context.Insert("DsDealerWharehouse", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>null</returns>
       /// <remarks>2014-10-15 余勇 创建</remarks>
        public override void Update(DsDealerWharehouse entity)
        {

            Context.Update("DsDealerWharehouse", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsDealerWharehouse")
                   .Where("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>model</returns>
       /// <remarks>2014-10-15 余勇 创建</remarks>
        public override DsDealerWharehouse Get(int sysNo)
        {
            return Context.Sql("select * from DsDealerWharehouse where sysNo=@sysNo")
                          .Parameter("sysNo", sysNo)
                          .QuerySingle<DsDealerWharehouse>();
        }

        /// <summary>
        /// 通过仓库编号取单条数据
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public override DsDealerWharehouse GetByWarehousSysNo(int warehousSysNo)
        {
            return Context.Sql("select * from DsDealerWharehouse where WarehouseSysNo=@warehousSysNo")
                          .Parameter("warehousSysNo", warehousSysNo)
                          .QuerySingle<DsDealerWharehouse>();
        }

        /// <summary>
        /// 通过经销商编号或库房ID
        /// </summary>
        /// <param name="UserSysNo"></param>
        /// <returns></returns>
        public override DsDealerWharehouse GetByDsUserSysNo(int UserSysNo)
        {
            string sql = "select * from DsDealerWharehouse where DealerSysNo =" + UserSysNo;
            return Context.Sql(sql).QuerySingle<DsDealerWharehouse>();
        }
        /// <summary>
        /// 获取所有分销商数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2016-03-14 杨云奕 添加
        /// </remarks>
        public override System.Collections.Generic.List<CBDsDealerWharehouse> GetAllDealerWarehouse()
        {
            string sql = @" select DsDealerWharehouse.*,DsDealer.DealerName,WhWarehouse.WarehouseName 
                            from DsDealerWharehouse inner join DsDealer on DsDealer.SysNo=DsDealerWharehouse.DealerSysNo inner join WhWarehouse on WhWarehouse.SysNo=DsDealerWharehouse.WarehouseSysNo ";
            return Context.Sql(sql).QueryMany<CBDsDealerWharehouse>();
        }
    }
}
