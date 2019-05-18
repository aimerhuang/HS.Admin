using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 仓库数据层
    /// </summary>
    /// <remarks>2013-08-28 邵斌 创建</remarks>
    public class WhWarehouseDaoImpl : IWhWarehouseDao
    {
        /// <summary>
        /// 根据用户区域编号和取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <returns>返回仓库列表和联系人等</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public override IList<CBWhWarehouse> GetWarehouse(int areaSysNo)
        {

            //公用数据库连接
            using (var context = Context.UseSharedConnection(true))
            {

                #region 测试SQL  查询仓库信息
                /*
                 select 
                     w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                from 
                     WhWarehouseArea  wwa
                     inner join BsArea ba on ba.sysno = wwa.areasysno
                     inner join whwarehouse w on w.sysno = wwa.warehousesysno
                where 
                     wwa.areasysno=901019 and ba.Status = 1
                 */
                #endregion

                //查询仓库基本信息
                IList<CBWhWarehouse> result = context.Sql(@"
                select 
                     w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                from 
                     WhWarehouseArea  wwa
                     inner join BsArea ba on ba.sysno = wwa.areasysno
                     inner join whwarehouse w on w.sysno = wwa.warehousesysno
                where 
                     wwa.areasysno=@0 and ba.Status = @1 and w.Status = @2
            ", areaSysNo, (int)WarehouseStatus.仓库支持的取件方式状态.有效,(int)BasicStatus.地区状态.有效).QueryMany<CBWhWarehouse>();

                //遍历查询出的仓库查询他们完整的地区地质信息,为了避免详细地质没有填写省市区信息,并找出改仓库支持的所以取件方式
                IList<BsArea> areas;
                foreach (var cbWhWarehouse in result)
                {
                    #region 测试SQL 递归查询地区完整信息
                    /*
                     select * from BsArea 
                        where status=1
                        start with sysno = 901019 
                        connect by prior parentsysno = sysno; 
                     */
                    #endregion

                    //查询结果默认是倒序区-市-县
                    areas = context.Sql(@"
                        select * from BsArea 
                        where status=@0
                        start with sysno = @1
                        connect by prior parentsysno = sysno
                    ", (int)BasicStatus.地区状态.有效, cbWhWarehouse.AreaSysNo).QueryMany<BsArea>();

                    //遍历地区信息填入仓库对应的地址信息
                    for (int i = 0; i < areas.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                //填充区
                                cbWhWarehouse.AreaSysNo = areas[i].SysNo;
                                cbWhWarehouse.AreaName = areas[i].AreaName;
                                break;
                            case 1:
                                //填充市
                                cbWhWarehouse.CitySysNo = areas[i].SysNo;
                                cbWhWarehouse.CityName = areas[i].AreaName;
                                break;
                            case 2:
                                //填充省
                                cbWhWarehouse.ProvinceSysNo = areas[i].SysNo;
                                cbWhWarehouse.ProvinceName = areas[i].AreaName;
                                break;
                        }
                    }

                    #region 测试SQL 读取仓库支持的取货方式

                    /*
                     select 
                       WhWarehousePickupType.Pickuptypesysno
                     from 
                       WhWarehousePickupType 
                       inner join LgPickupType on WhWarehousePickupType.Pickuptypesysno = LgPickupType.sysno   
                     where 
                    WhWarehousePickupType.Warehousesysno=1078 and WhWarehousePickupType.status = 1 and LgPickupType.Status = 1
                     */

                    #endregion

                    cbWhWarehouse.PickUpType = context.Sql(@"
                        select 
                           WhWarehousePickupType.Pickuptypesysno
                         from 
                           WhWarehousePickupType 
                           inner join LgPickupType on WhWarehousePickupType.Pickuptypesysno = LgPickupType.sysno   
                         where 
                        WhWarehousePickupType.Warehousesysno=@0 and WhWarehousePickupType.status = @1 and LgPickupType.Status = @2
                    ", cbWhWarehouse.SysNo, (int)WarehouseStatus.仓库支持的取件方式状态.有效, (int)LogisticsStatus.取件方式状态.有效).QueryMany<int>();

                }

                return result;
            }

        }

        /// <summary>
        /// 周围是否有仓库支持取货方式
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="pickupType">取件方式</param>
        /// <returns>返回 true:有仓库取货 false:不支持</returns>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public override bool AroundHasWarehouseSupportPickUp(int areaSysNo, int pickupType)
        {
            #region 测试SQL  查询仓库信息
            /*
                 select 
                   distinct wall.sysno,wall.warehousename,wall.areasysno,wall.areaname,wall.streetaddress,wall.contact,wall.phone   
                 from 
                   WhWarehousePickupType 
                   inner join LgPickupType on WhWarehousePickupType.Pickuptypesysno = LgPickupType.sysno
                   inner join (
                                --查找区域下的仓库
                                select 
                                     w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                                from 
                                     WhWarehouseArea  wwa
                                     inner join BsArea ba on ba.sysno = wwa.areasysno
                                     inner join whwarehouse w on w.sysno = wwa.warehousesysno
                                where 
                                     wwa.areasysno=901019 and ba.Status = 1
                               ) wall on WhWarehousePickupType.Warehousesysno = wall.sysno
   
                 where 
                 --判断区域下的仓库是否支持指定的取件方式
                 WhWarehousePickupType.status = 1 and (LgPickupType.sysno=1 or LgPickupType.Parentsysno=1) and LgPickupType.Status = 1
                 */
            #endregion

            //查询仓库基本信息
            return Context.Sql(@"
                select 
                   count(wall.sysno)  
                 from 
                   WhWarehousePickupType 
                   inner join LgPickupType on WhWarehousePickupType.Pickuptypesysno = LgPickupType.sysno
                   inner join (
                                select 
                                     w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                                from 
                                     WhWarehouseArea  wwa
                                     inner join BsArea ba on ba.sysno = wwa.areasysno
                                     inner join whwarehouse w on w.sysno = wwa.warehousesysno
                                where 
                                     wwa.areasysno=@0 and ba.Status = @1
                               ) wall on WhWarehousePickupType.Warehousesysno = wall.sysno
   
                 where 
                 WhWarehousePickupType.status = @2 and (LgPickupType.sysno=@3 or LgPickupType.Parentsysno=@4) and LgPickupType.Status = @5
            ", areaSysNo, (int)Model.WorkflowStatus.BasicStatus.地区状态.有效, (int)WarehouseStatus.仓库支持的取件方式状态.有效, pickupType,
                            pickupType, (int)LogisticsStatus.取件方式状态.有效)
                       .QuerySingle<int>() > 0;

        }

        /// <summary>
        /// 周围是否有仓库支持配送方式
        /// </summary>
        /// <param name="areaSysNo"></param>
        /// <param name="lgDeliveryType"></param>
        /// <returns>返回 True:支持配送方式 False:不支持配送</returns>
        /// <remarks>2013-09-05 邵斌 创建</remarks>
        public override bool AroundHasWarehouseSupportDelivery(int areaSysNo, int lgDeliveryType)
        {
            #region 测试SQL查询

            /*
            select 
              distinct wall.sysno,wall.warehousename,wall.areasysno,wall.areaname,wall.streetaddress,wall.contact,wall.phone   
            from 
              WhWarehouseDeliveryType 
              inner join LgDeliveryType on WhWarehouseDeliveryType.Deliverytypesysno = LgDeliveryType.sysno
              inner join (
                        --查找区域下的仓库
                        select 
                             w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                        from 
                             WhWarehouseArea  wwa
                             inner join BsArea ba on ba.sysno = wwa.areasysno
                             inner join whwarehouse w on w.sysno = wwa.warehousesysno
                        where 
                             wwa.areasysno=901019 and ba.Status = 1
                       ) wall on WhWarehouseDeliveryType.Warehousesysno = wall.sysno
   
            where 
               --判断区域下的仓库是否支持指定的取件方式
               WhWarehouseDeliveryType.status = 1 and (LgDeliveryType.sysno=1 or LgDeliveryType.Parentsysno=1) and LgDeliveryType.Status = 1
             */

            //查询仓库基本信息
            return Context.Sql(@"
                    select 
                      distinct wall.sysno,wall.warehousename,wall.areasysno,wall.areaname,wall.streetaddress,wall.contact,wall.phone   
                    from 
                      WhWarehouseDeliveryType 
                      inner join LgDeliveryType on WhWarehouseDeliveryType.Deliverytypesysno = LgDeliveryType.sysno
                      inner join (
                                --查找区域下的仓库
                                select 
                                     w.sysno,w.warehousename,w.areasysno,ba.areaname,w.streetaddress,w.contact,w.phone
                                from 
                                     WhWarehouseArea  wwa
                                     inner join BsArea ba on ba.sysno = wwa.areasysno
                                     inner join whwarehouse w on w.sysno = wwa.warehousesysno
                                where 
                                     wwa.areasysno=:0 and ba.Status = :1
                               ) wall on WhWarehouseDeliveryType.Warehousesysno = wall.sysno
   
                    where 
                       WhWarehouseDeliveryType.status = :2 and (LgDeliveryType.sysno=:3 or LgDeliveryType.Parentsysno=:4) and LgDeliveryType.Status = :5
            ", areaSysNo, (int)Model.WorkflowStatus.BasicStatus.地区状态.有效, (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送方式状态.启用, lgDeliveryType,
                            lgDeliveryType, (int)Model.WorkflowStatus.LogisticsStatus.配送方式状态.启用)
                       .QuerySingle<int>() > 0;

            #endregion
        }

        /// <summary>
        /// 读取默认仓库（暂定为成都）
        /// </summary>
        /// <param name="isPickUp">是否是取货仓库： true 取货，false 收货</param>
        /// <param name="optionType">取送货类型</param>
        /// <returns>返回仓库系统编号</returns>
        /// <remarks>2013-09-05 邵斌 创建</remarks>
        public override int GetDefaultWarehouse(bool isPickUp, int optionType)
        {
            string sql;

            //是否是查询取件仓库
            if (isPickUp)
            {
                #region 测试SQL  查询成都默认取货方式的第一个仓库
                /*
                      select wh.sysno,max(wwa.isdefault) as t 
                        from
       
                               WhWarehouseArea wwa
                               inner join (select sysno from BsArea
                                           where status=1 
                                           start with sysno = (select sysno from BsArea where charindex(areaname,'成都') > 0 and rownum = 1)
                                           connect by prior sysno = parentsysno                          
                                ) a on wwa.areasysno = a.sysno
                               inner join whwarehouse wh on wh.sysno = wwa.warehousesysno
                               inner join WhWarehouseDeliveryType wdt on wdt.warehousesysno = wh.sysno
                        where 
                         wh.status = 1 and wdt.status = 1 and wdt.deliverytypesysno = 1   and rownum=1
                        group by wh.sysno,wwa.isdefault
                        order by t desc
                      */
                #endregion

                sql = @"
                        select wh.sysno,max(wwa.isdefault) as t  
                        from
       
                               WhWarehouseArea wwa
                               inner join (select sysno from BsArea
                                           where status=@0 
                                           start with sysno = (select sysno from BsArea where charindex(areaname,'成都') > 0 and rownum = 1)
                                           connect by prior sysno = parentsysno                          
                                ) a on wwa.areasysno = a.sysno
                               inner join whwarehouse wh on wh.sysno = wwa.warehousesysno
                               inner join WhWarehouseDeliveryType wdt on wdt.warehousesysno = wh.sysno
                        where 
                         wh.status = @1 and wdt.status = @2 and wdt.deliverytypesysno = @3  and rownum=1
                        group by wh.sysno,wwa.isdefault
                        order by t desc
                    ";
            }
            else
            {

                #region 测试SQL  查询成都默认送货方式的第一个仓库
                /*
                      select wh.sysno,max(wwa.isdefault) as t 
                        from
       
                               WhWarehouseArea wwa
                               inner join (select sysno from BsArea
                                           where status=1 
                                           start with sysno = (select sysno from BsArea where charindex(areaname,'成都') > 0 and rownum = 1)
                                           connect by prior sysno = parentsysno                          
                                ) a on wwa.areasysno = a.sysno
                               inner join whwarehouse wh on wh.sysno = wwa.warehousesysno
                               inner join WhWarehousePickupType wpt on wpt.warehousesysno = wh.sysno
                        where 
                         wh.status = 1 and wpt.status = 1 and wpt.pickuptypesysno = 1 and rownum=1
                        group by wh.sysno,wwa.isdefault
                        order by t desc
                      */
                #endregion

                sql = @"
                     select wh.sysno,max(wwa.isdefault) as t  
                        from
       
                               WhWarehouseArea wwa
                               inner join (select sysno from BsArea
                                           where status=@0 
                                           start with sysno = (select sysno from BsArea where charindex(areaname,'成都') > 0 and rownum = 1)
                                           connect by prior sysno = parentsysno                          
                                ) a on wwa.areasysno = a.sysno
                               inner join whwarehouse wh on wh.sysno = wwa.warehousesysno
                               inner join WhWarehousePickupType wpt on wpt.warehousesysno = wh.sysno
                        where 
                         wh.status = @1 and wpt.status = @2 and wpt.pickuptypesysno = @3 and rownum=1
                        group by wh.sysno,wwa.isdefault
                        order by t desc
                ";
            }

           return Context.Sql(sql,
                (int)BasicStatus.地区状态.有效,
                (int)WarehouseStatus.仓库状态.启用,
                (isPickUp == true ? (int)WarehouseStatus.仓库支持的取件方式状态.有效 : (int)WarehouseStatus.仓库支持的取件方式状态.有效),
                optionType).QuerySingle<int>();
        }

        /// <summary>
        /// 根据仓库ERp编号获取仓库信息
        /// </summary>
        /// <param name="ErpCode"></param>
        /// <returns></returns>
        public override WhWarehouse GetModelErpCode(string ErpCode)
        {
           return Context.Sql("select * from WhWarehouse where ErpCode=@ErpCode").Parameter("ErpCode", ErpCode).QuerySingle<WhWarehouse>();
        }

        /// <summary>
        /// 查询仓库基本信息
        /// </summary>
        /// <remarks>2013-08-28 邵斌 创建</remarks>
        public override WhWarehouse GetModel(int sysno)
        {
            //查询仓库基本信息
            return Context.Sql(@"select *  from   WhWarehouse  where sysno=" + sysno + "")
                       .QuerySingle<WhWarehouse>();

        }
    }
}
