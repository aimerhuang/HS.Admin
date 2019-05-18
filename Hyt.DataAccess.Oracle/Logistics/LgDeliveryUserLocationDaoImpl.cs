using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 配送员位置信息
    /// </summary>
    /// <remarks>
    /// 2013-06-08 郑荣华 创建
    /// </remarks>
    public class LgDeliveryUserLocationDaoImpl : ILgDeliveryUserLocationDao
    {
        #region 操作

        /// <summary>
        /// 添加配送员位置信息
        /// </summary>
        /// <param name="model">配送员位置信息实体</param>
        /// <returns>添加的配送员位置信息Sysno</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public override int Create(LgDeliveryUserLocation model)
        {
            return Context.Insert("LgDeliveryUserLocation", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        #endregion

        #region 查询

        #region 业务员定位

        /// <summary>
        /// 获取仓库下多个配送员最后一次位置信息
        /// </summary>
        /// <param name="whWarehouseSysNo">仓库系统编号</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public override IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(int whWarehouseSysNo)
        {
            #region Sql

            const string sql = @"select a.* ,b.username from LgDeliveryUserLocation a                            
                                 inner join syuser b  on a.DeliveryUserSysno=b.sysno 
                                 where a.sysno in 
                                 (select max(sysno) from LgDeliveryUserLocation                                  
                                 where (latitude<>0 or longitude<>0) 
                                 and DeliveryUserSysno in  
                                 (select usersysno from syuserwarehouse                                
                                 where warehousesysno =@0)          --参数：仓库系统编号
                                 group by DeliveryUserSysno )";
            #endregion

            return Context.Sql(sql, whWarehouseSysNo).QueryMany<CBLgDeliveryUserLocation>();
        }

        /// <summary>
        /// 根据配送员编号查询配送员最近一次定位信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员编号逗号分隔的字符串</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public override IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(string deliveryUserSysNo)
        {
            #region Sql

            const string sqla = @"select a.*,b.username from LgDeliveryUserLocation a                        
                                  inner join syuser b on a.DeliveryUserSysno=b.sysno                             
                                  where a.sysno in 
                                  (select max(sysno) from LgDeliveryUserLocation                                  
                                  where (latitude<>0 or longitude<>0) 
                                  and DeliveryUserSysno in  ({0})  --参数：配送员编号逗号分隔的字符串                                          
                                  group by DeliveryUserSysno )";
            #endregion

            string sql = string.Format(sqla, deliveryUserSysNo);

            return Context.Sql(sql).QueryMany<CBLgDeliveryUserLocation>();
        }

        /// <summary>
        /// 全国配送员最新位置
        /// </summary>
        /// <param name="idlist">仓库系统编号列表</param>
        /// <param name="status">状态(0-全部,1-30分钟活动,2-当日活动,3-不在线)</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>2014-03-10 周唐炬 创建</remarks>
        public override List<CBLgDeliveryUserLocation> AllDeliveryUserLastLocation(List<int> idlist, int status)
        {
            var sb = new StringBuilder();
            sb.Append(@"select t.sysno,
                               t.deliveryusersysno,
                               t.latitude,
                               t.longitude,
                               t.gpsdate,
                               t.locationtype,
                               t.radius,
                               t.createddate,s.username
                          from (select deliveryusersysno, max(gpsdate) gpsdate
                                  from lgdeliveryuserlocation");
            //加入仓库列表
            if (idlist != null && idlist.Any())
            {
                sb.AppendFormat(@" where deliveryusersysno in
                           (select usersysno
                              from SYUSERWAREHOUSE
                             where warehousesysno in ({0})) group by deliveryusersysno) max_list", string.Join(",", idlist));
            }
            else
            {
                sb.Append(@" group by deliveryusersysno) max_list");
            }
            sb.Append(@" inner join lgdeliveryuserlocation t
                            on t.deliveryusersysno = max_list.deliveryusersysno
                           and t.gpsdate = max_list.gpsdate
                         inner join syuser s
                            on max_list.DeliveryUserSysno = s.sysno
                         where (t.latitude <> 0 or t.longitude <> 0)");
            //加入时间段1-30分钟以内，2-当日活动，3-当日无活动
            switch (status)
            {
                case 1:
                    sb.Append(@" and t.gpsdate >= sysdate - 1 / 48");
                    break;
                case 2:
                    sb.Append(@" and t.gpsdate >= trunc(sysdate) and t.gpsdate < trunc(sysdate) + 1");
                    break;
                case 3:
                    sb.Append(@" and t.gpsdate <= trunc(sysdate)");
                    break;
            }
            return Context.Sql(sb.ToString()).QueryMany<CBLgDeliveryUserLocation>();
        }

        /// <summary>
        /// 获取配送人员定位信息
        /// </summary>
        /// <param name="delUserSysNo">配送人员系统编号</param>
        /// <returns>LgDeliveryUserLocation</returns>
        /// <remarks>2014-01-17 黄伟 创建</remarks>
        public override LgDeliveryUserLocation GetLocationByUserSysNo(int delUserSysNo)
        {
            return Context.Sql("select * from LgDeliveryUserLocation where deliveryUsersysno=@0",
                                    delUserSysNo).QuerySingle<LgDeliveryUserLocation>();
        }

        #endregion

        #region 配送路径查询

        /// <summary>
        /// 获取单日内配送员位置信息
        /// </summary>
        /// <param name="deliveryUserSysno">配送员sysno</param>
        /// <param name="dateRange">时间范围</param>
        /// <returns>配送员位置信息 最大记录数24*60=1440条</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public override IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLocation(int deliveryUserSysno, DateRange dateRange)
        {
            const string sql = @"select t.*,b.username from LgDeliveryUserLocation t 
                                inner join syuser b on t.DeliveryUserSysno=b.sysno 
                                where t.DeliveryUserSysno=@deliveryUserSysno
                                and (latitude<>0 or longitude<>0) 
                                and t.gpsdate >= @StartTime
                                and t.gpsdate <= @EndTime
                                order by t.gpsdate ";

            return Context.Sql(sql)
                          .Parameter("deliveryUserSysno", deliveryUserSysno)
                          .Parameter("StartTime", dateRange.StartTime)
                          .Parameter("EndTime", dateRange.EndTime)
                          .QueryMany<CBLgDeliveryUserLocation>();

        }

        /// <summary>
        /// 根据时间，仓库，查询在定位信息表中有记录的配送员
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="dateRange">日期范围</param>
        /// <returns>配送员信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public override IList<SyUser> GetDeliveryUserForMap(int whSysNo, DateRange dateRange)
        {

            const string sql = @"select * from syuser where sysno in
                                 (select a.deliveryusersysno from  lgdeliveryuserlocation a 
                                 inner join syuserwarehouse b on a.deliveryusersysno=b.usersysno
                                 where b.warehousesysno= @warehousesysno
                                 and a.gpsdate >= @StartTime
                                 and a.gpsdate <= @EndTime)";

            return Context.Sql(sql)
                          .Parameter("warehousesysno", whSysNo)
                          .Parameter("StartTime", dateRange.StartTime)
                          .Parameter("EndTime", dateRange.EndTime)
                          .QueryMany<SyUser>();

        }

        /// <summary>
        /// 查询配送员在某段时间配送的配送单
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="dateRange">日期时间范围</param>
        /// <returns>配送单信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public override IList<CBLgDeliveryInvoice> GetLgDeliveryForMap(int deliveryUserSysNo, DateRange dateRange)
        {
            //            const string sql = @"select a.sysno,a.createddate,nvl(b.createddate,:EndTime) enddate from lgdelivery a 
            //                                left join LgSettlementItem b on a.sysno=b.deliverysysno
            //                                where a.deliveryUserSysNo=:deliveryUserSysno
            //                                and a.createddate >= :StartTime
            //                                and a.createddate <= :EndTime";
            const string sql = @"select a.sysno,a.createddate,@EndTime as enddate from lgdelivery a 
                                where a.deliveryUserSysNo=@deliveryUserSysno
                                and a.createddate >= @StartTime
                                and a.createddate <= @EndTime";

            //return Context.Sql(sql)
            //    .Parameter("EndTime", dateRange.EndTime)
            //         .Parameter("deliveryUserSysno", deliveryUserSysno)
            //         .Parameter("StartTime", dateRange.StartTime)
            //         .Parameter("EndTime", dateRange.EndTime)
            //         .QueryMany<CBLgDeliveryInvoice>();
            return Context.Sql(sql).Parameter("EndTime", dateRange.EndTime)
                    .Parameter("deliveryUserSysno", deliveryUserSysNo)
                    .Parameter("StartTime", dateRange.StartTime)
                    .Parameter("EndTime", dateRange.EndTime)
                    .QueryMany<CBLgDeliveryInvoice>();
        }

        #endregion

        #endregion
    }
}
