using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 仓库信息数据访问类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class WhWarehouseDaoImpl : IWhWarehouseDao
    {

        /// <summary>
        /// 获取仓库物流运费模板关联列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public override List<WhWarehouseDeliveryType> GetWarehouseDeliveryTypeList(int warehouseSysNo)
        {
            const string sql = @"select 
                                [SysNo],[DeliveryTypeSysNo],[WarehouseSysNo],[FreightModuleSysNo],[Status],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate] 
                                from WhWarehouseDeliveryType where WarehouseSysNo=@0 and FreightModuleSysNo<>0";
            return Context.Sql(sql, warehouseSysNo).QueryMany<WhWarehouseDeliveryType>();
        }
        /// <summary>
        /// 更新仓库配送方式关联运费模板
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public override int UpdateWarehouseDeliveryTypeAssoFreightModule(int warehouseSysNo, int deliveryTypeSysNo, int freightModuleSysNo)
        {
            const string sql = @" UPDATE WhWarehouseDeliveryType SET FreightModuleSysNo=@0	WHERE WarehouseSysNo=@1 and DeliveryTypeSysNo=@2  ";

            return Context.Sql(sql, freightModuleSysNo, warehouseSysNo, deliveryTypeSysNo).Execute();
        }

        /// <summary>
        /// 更新仓库利嘉返回对应仓库编号
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="LiJiaStoreCode">利嘉返回对应仓库编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-25 罗勤尧 创建</remarks>
        public override int UpdateLiJiaStoreCode(int warehouseSysNo, string LiJiaStoreCode)
        {
            const string sql = @" UPDATE WhWarehouse SET LiJiaStoreCode=@0	WHERE SysNo=@1  ";

            return Context.Sql(sql, LiJiaStoreCode, warehouseSysNo).Execute();
        }
        /// <summary>
        /// 获取仓库物流运费模板关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public override WhWarehouseDeliveryType GetWarehouseDeliveryType(int warehouseSysNo, int deliveryTypeSysNo)
        {
            const string sql = @"select 
                                [SysNo],[DeliveryTypeSysNo],[WarehouseSysNo],[FreightModuleSysNo],[Status],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate] 
                                from WhWarehouseDeliveryType where WarehouseSysNo=@0 and DeliveryTypeSysNo=@1";
            return Context.Sql(sql, warehouseSysNo, deliveryTypeSysNo).QuerySingle<WhWarehouseDeliveryType>();
        }
        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信息列表</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public override IList<SyUser> GetWhDeliveryUser(int warehouseSysNo)
        {
            //            const string sql = @"select t.* from syuser t where t.sysno in 
            //                                (select distinct a.usersysno from syuserwarehouse a 
            //                                inner join sygroupuser b on a.usersysno=b.usersysno
            //                                where a.warehousesysno=:0   --仓库编号
            //                                and b.groupsysno=:1) and t.status=:2
            //                                order by NLSSORT(t.username,'NLS_SORT=SCHINESE_PINYIN_M')
            //                                ";  //业务组编号
            const string sql = @"select t.* from syuser t where t.sysno in 
                                (select distinct a.usersysno from syuserwarehouse a 
                                inner join sygroupuser b on a.usersysno=b.usersysno
                                where a.warehousesysno=@0   --仓库编号
                                and b.groupsysno=@1) and t.status=@2
                                order by t.username collate chinese_prc_cs_as_ks_ws 
                                ";  //业务组编号
            return Context.Sql(sql, warehouseSysNo, Hyt.Model.SystemPredefined.UserGroup.业务员组, SystemStatus.系统用户状态.启用)
                          .QueryMany<SyUser>();
        }

        /// <summary>
        /// 获取未录入信用信息的仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信息</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public override IList<SyUser> GetWhDeliveryUserForCredit(int warehouseSysNo)
        {
            //            const string sql = @"select a.* from syuser a inner join  
            //                          (select distinct a.usersysno from syuserwarehouse a 
            //                              inner join sygroupuser b on a.usersysno=b.usersysno
            //                              where a.warehousesysno=:0 --仓库编号
            //                              and b.groupsysno=:1
            //                              and a.usersysno not in 
            //                              (select deliveryUsersysno from lgdeliveryUsercredit where warehousesysno=:2)
            //                           ) b on a.sysno=b.usersysno where a.status=:3 
            //                            order by NLSSORT(a.username,'NLS_SORT=SCHINESE_PINYIN_M')
            //                             ";
            const string sql = @"select a.* from syuser a inner join  
                          (select distinct a.usersysno from syuserwarehouse a 
                              inner join sygroupuser b on a.usersysno=b.usersysno
                              where a.warehousesysno=@0 --仓库编号
                              and b.groupsysno=@1
                              and a.usersysno not in 
                              (select deliveryUsersysno from lgdeliveryUsercredit where warehousesysno=@2)
                           ) b on a.sysno=b.usersysno where a.status=@3 
                            order by a.username collate chinese_prc_cs_as_ks_ws
                             ";
            return Context.Sql(sql, warehouseSysNo, Hyt.Model.SystemPredefined.UserGroup.业务员组, warehouseSysNo, SystemStatus.系统用户状态.启用)
                          .QueryMany<SyUser>();

        }

        /// <summary>
        /// 根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns>匹配仓库数据列表</returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public override IList<WhWarehouse> GetWhWarehouseListByArea(int areaSysNo, int? warehouseType, int? deliveryType = null)
        {

            return Context.Sql(@"select distinct whwarehouse.*,whwarehouse.warehousename Collate Chinese_PRC_Stroke_ci_as from whwarehouse 
                left outer join WhWarehouseDeliveryType
                on WhWarehouseDeliveryType.WarehouseSysNo = WhWarehouse.SysNo
                where whwarehouse.Status=1 and AreaSysNo=@AreaSysNo and 
                (@warehousetype  is null  or warehousetype=@warehousetype)
                and 
                (@DeliveryTypeSysNo  is null  or WhWarehouseDeliveryType.Deliverytypesysno=@DeliveryTypeSysNo)
                order by whwarehouse.warehousename Collate Chinese_PRC_Stroke_ci_as")
                .Parameter("AreaSysNo", areaSysNo)
                .Parameter("warehousetype", warehouseType)
                .Parameter("DeliveryTypeSysNo", deliveryType)
                 .QueryMany<WhWarehouse>();

        }

        /// <summary>
        /// 根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns>匹配仓库数据列表</returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public override IList<WhWarehouse> GetWhWarehouseListByDeliveryType(int deliveryType)
        {

            return Context.Sql(@"select distinct whwarehouse.*,whwarehouse.warehousename Collate Chinese_PRC_Stroke_ci_as from whwarehouse 
                left outer join WhWarehouseDeliveryType
                on WhWarehouseDeliveryType.WarehouseSysNo = WhWarehouse.SysNo
                where whwarehouse.Status=1 
                and 
                (WhWarehouseDeliveryType.Deliverytypesysno=@DeliveryTypeSysNo)
                order by whwarehouse.warehousename Collate Chinese_PRC_Stroke_ci_as")
                .Parameter("DeliveryTypeSysNo", deliveryType)
                 .QueryMany<WhWarehouse>();

        }

        /// <summary>
        /// 根据地区、仓库类型、取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="pickupType">取件方式编号</param>
        /// <returns>返回仓库列表</returns>
        /// <remarks>2013-09-13 周唐炬 创建</remarks>
        public override IList<WhWarehouse> GetWhWarehouseList(int areaSysNo, int? warehouseType, int pickupType)
        {
            const string sql = @"SELECT A.*
                                          FROM whwarehouse A
                                          INNER JOIN WhWarehousePickupType B
                                            ON B.Warehousesysno = A.Sysno
                                           AND B.Pickuptypesysno = @0
                                         WHERE A.AreaSysNo = @1
                                           AND (@2 IS NULL OR A.warehousetype=@2)
                                        order by a.warehousename Collate Chinese_PRC_Stroke_ci_as";
            var parameters = new object[]
                {
                    pickupType,
                    areaSysNo,
                    warehouseType
                };
            return Context.Sql(sql).Parameters(parameters).QueryMany<WhWarehouse>();
        }

        /// <summary>
        ///根据地区信息获取仓库信息
        /// </summary>
        /// <param name="area">地区信息</param>
        /// <returns>仓库列表</returns>
        /// <remarks> 
        /// 2013-09-11 杨晗 创建
        /// </remarks>
        public override IList<WhWarehouse> GetWhWarehouseListByArea(List<int> area)
        {
            string sql = @"select * from whwarehouse where AreaSysNo in {0} and warehousetype=@warehousetype and Status=@status order by warehousename Collate Chinese_PRC_Stroke_ci_as";
            sql = string.Format(sql, "(" + string.Join(",", area) + ")");
            return Context.Sql(sql)
                .Parameter("warehousetype", (int)WarehouseStatus.仓库类型.门店)
                .Parameter("status", (int)WarehouseStatus.仓库状态.启用)
                .QueryMany<WhWarehouse>();
        }

        /// <summary>
        /// 获取所有仓库信息
        /// </summary>
        /// <returns>仓库数据列表</returns>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        public override IList<WhWarehouse> GetAllWarehouseList()
        {
            return Context.Sql("select * from whwarehouse order by warehousename Collate Chinese_PRC_Stroke_ci_as").QueryMany<WhWarehouse>();
        }

        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>返回仓库详情</returns>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        public override WhWarehouse GetWarehouseEntity(int sysNo)
        {
            return Context.Sql("select * from whwarehouse where SysNo=@0", sysNo).QuerySingle<WhWarehouse>();
        }
        /// <summary>
        /// 获取实际仓库库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="whsysNo"></param>
        /// <returns></returns>
        public override string GetStockQuantity(int sysNo,int whsysNo)
        {
            return Context.Sql("select StockQuantity from PdProductStock where PdProductSysNo = @0 and WarehouseSysNo=@1", sysNo,whsysNo).QuerySingle<string>();
        }

        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库名称</param>
        /// <returns>返回仓库详情</returns>
        /// <remarks> 2015-12-15 王耀发 创建</remarks>
        public override WhWarehouse GetWarehouseByName(string BackWarehouseName)
        {
            return Context.Sql("select * from whwarehouse where BackWarehouseName=@0", BackWarehouseName).QuerySingle<WhWarehouse>();
        }


        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>返回仓库扩展实体</returns>
        /// <remarks> 2013-08-07 周瑜 创建</remarks>
        public override CBWhWarehouse GetWarehouse(int sysNo)
        {
            return Context.Sql(@"select a.*,
b.areaname as AreaName,
c.SysNo as CitySysNo,c.areaname as CityName,
d.sysno as ProvinceSysNo,d.areaname as ProvinceName 
from whwarehouse a
left join bsarea b on a.areasysno = b.sysno
left join bsarea c on b.parentsysno = c.sysno
left join bsarea d on c.parentsysno = d.sysno 
where a.SysNo=@0", sysNo).QuerySingle<CBWhWarehouse>();
        }

        /// <summary>
        /// 通过仓库类型获取仓库列表
        /// </summary>
        /// <param name="warehouseType">仓库类型编号</param>
        /// <returns>仓库数据列表</returns>
        /// <remarks> 2013-06-27 余勇 创建</remarks>
        public override IList<WhWarehouse> GetWhWarehouseListByType(int warehouseType)
        {
            return Context.Sql("select * from whwarehouse where WarehouseType=@0 order by warehousename Collate Chinese_PRC_Stroke_ci_as", warehouseType).QueryMany<WhWarehouse>();
        }

        /// <summary>
        /// 获取仓库下面是否允许借货的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="isDelivery">是否允许借货</param>
        /// <returns>配送员列表信息</returns>
        /// <remarks> 
        /// 2013-07-03 沈强 创建
        /// </remarks>
        public override IList<SyUser> GetWhDeliveryUser(int warehouseSysNo, Model.WorkflowStatus.LogisticsStatus.配送员是否允许配送 isDelivery)
        {
            const string sql = @"select * from syuser d where d.sysno in 
                                (select a.usersysno from syuserwarehouse a 
                                left join sygroupuser b on a.usersysno=b.usersysno
                                where a.warehousesysno=@0   --仓库编号
                                and b.groupsysno=@1) and (select count(1) from LgDeliveryUserCredit c where c.deliveryusersysno = d.sysno and c.isallowdelivery = @2) > 0
                                order by d.username Collate Chinese_PRC_Stroke_ci_as";

            return Context.Sql(sql, warehouseSysNo, Hyt.Model.SystemPredefined.UserGroup.业务员组, (int)isDelivery)
                          .QueryMany<SyUser>();
        }

        /// <summary>
        /// 获取配送员仓库
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-08-07 周唐炬 创建</remarks>
        public override int GetDeliveryUserWarehouseSysNo(int deliveryUserSysNo)
        {
            const string sql =
                            @"SELECT warehousesysno
                                FROM SYUSERWAREHOUSE
                                WHERE usersysno=@UserSysNo";
            return Context.Sql(sql).Parameter("UserSysNo", deliveryUserSysNo).QuerySingle<int>();
        }

        /// <summary>
        /// 获取用户有可管理的所有仓库
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>仓库集合</returns>
        /// <remarks>
        /// 2013/7/4 何方 创建
        /// </remarks>
        public override IList<WhWarehouse> GetUserWarehuoseList(int userSysNo)
        {
            //            const string sql =
            //                            @"select w.*
            //                            from WHWAREHOUSE w
            //                            inner join SYUSERWAREHOUSE s on s.warehousesysno=w.sysno
            //                            and s.usersysno=:UserSysNo 
            //                            order by NLSSORT(w.warehousename,'NLS_SORT=SCHINESE_PINYIN_M')";
            //mssql 按拼音排序  w.warehousename collate chinese_prc_cs_as_ks_ws 
            const string sql =
                @"select w.*
                            from WHWAREHOUSE w
                            inner join SYUSERWAREHOUSE s on s.warehousesysno=w.sysno
                            and s.usersysno=@UserSysNo 
                            order by w.warehousename collate chinese_prc_cs_as_ks_ws ";
            var list = Context.Sql(sql)
                .Parameter("UserSysNo", userSysNo)
                .QueryMany<WhWarehouse>();
            return list;
        }

        /// <summary>
        /// 获取多个仓库的配送员
        /// </summary>
        /// <param name="warehouseSysNos">仓库SysNO集合</param>
        /// <returns>用户集合</returns>
        /// <remarks>
        /// 2013/7/4 何方 创建
        /// </remarks>
        public override IList<SyUser> GetDeliveryUserList(IList<int> warehouseSysNos)
        {
            //            const string sql =
            //                        @"select s.* 
            //                            from SYUSER s
            //                            where s.sysno in(
            //                                            select a.usersysno from SYUSERWAREHOUSE a
            //                                             where a.warehousesysno in(:WarehouseSysNos))
            //                            order by NLSSORT(s.username,'NLS_SORT=SCHINESE_PINYIN_M')
            //                        ";
            const string sql =
                       @"select s.* 
                            from SYUSER s
                            where s.sysno in(
                                            select a.usersysno from SYUSERWAREHOUSE a
                                             where a.warehousesysno in(@WarehouseSysNos))
                            order by s.username collate chinese_prc_cs_as_ks_ws 
                        ";
            var list = Context.Sql(sql).Parameter("WarehouseSysNos", warehouseSysNos).QueryMany<SyUser>();
            return list;
        }

        #region 仓库快递方式维护

        /// <summary>
        /// 获取所有的仓库快递方式
        /// </summary>
        /// <returns></returns>
        /// <remarks> 
        /// 2014-05-14 朱成果 创建
        /// </remarks>
        public override List<WhWarehouseDeliveryType> GetWhWarehouseDeliveryTypeList()
        {
            return Context.Sql("select * from WhWarehouseDeliveryType").QueryMany<WhWarehouseDeliveryType>();
        }

        public override List<WhWarehouse> GetWhWarehouseList()
        {
            return Context.Sql("select  * from [WhWarehouse] where sysno not in(select distinct [WarehouseSysNo] from [WhWarehouseDeliveryType])").QueryMany<WhWarehouse>();
        }

        /// <summary>
        /// 添加仓库快递方式
        /// </summary>
        /// <param name="model">仓库快递方式实体</param>        
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public override int CreateWareHouseDeliveryType(WhWarehouseDeliveryType model)
        {
            return Context.Insert<WhWarehouseDeliveryType>("whwarehousedeliverytype", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库快递方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public override int DeleteWareHouseDeliveryType(int sysNo)
        {
            return Context.Delete("whwarehousedeliverytype")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">快递方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public override int DeleteWareHouseDeliveryType(int whSysNo, int deliveryTypeSysNo)
        {
            return Context.Sql("delete from whwarehousedeliverytype where WareHouseSysNo = @0 and DeliveryTypeSysNo = @1", whSysNo, deliveryTypeSysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取仓库快递方式列表信息
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>仓库快递方式列表信息</returns>
        /// <remarks>
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public override IList<CBWhWarehouseDeliveryType> GetLgDeliveryType(ParaWhDeliveryTypeFilter filter)
        {
            const string sql = @"select t.*,b.warehousename,c.deliverytypename,c.deliverylevel,c.deliverytime,c.freight 
                                from whwarehousedeliverytype t left join whwarehouse b on t.warehousesysno=b.sysno
                                left join lgdeliverytype c on t.deliverytypesysno=c.sysno 
                                where (@whSysNo is null or t.warehousesysno=@whSysNo)";

            var list = Context.Sql(sql)
                              .Parameter("whSysNo", filter.WareHouseSysNo)
                              .QueryMany<CBWhWarehouseDeliveryType>();
            return list;
        }

        #endregion

        #region 仓库取件方式
        /// <summary>
        /// 仓库取件方式
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <returns>取件方式列表</returns>
        /// <remarks>  2013-07-11 朱成果 创建</remarks>
        public override IList<LgPickupType> GetPickupTypeListByWarehouse(int WarehouseSysNo)
        {
            return Context.Sql(@"
                            select t1.* from LgPickupType t1
                            inner join WhWarehousePickupType t2
                            on t1.sysno=t2.PickupTypeSysNo 
                            where t2.Status=1 and t2.warehousesysno=@WarehouseSysNo")
                .Parameter("WarehouseSysNo", WarehouseSysNo).QueryMany<LgPickupType>();
        }

        /// <summary>
        /// 添加仓库取件方式
        /// </summary>
        /// <param name="model">仓库取件方式实体</param>        
        /// <returns>添加后的系统编号</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public override int CreateWareHousePickUpType(WhWarehousePickupType model)
        {
            return Context.Insert("WhWarehousePickupType", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库取件方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public override int DeleteWareHousePickUpType(int sysNo)
        {
            return Context.Delete("WhWarehousePickupType")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="pickUpTypeSysNo">取件方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public override int DeleteWareHousePickUpType(int whSysNo, int pickUpTypeSysNo)
        {
            return Context.Sql("delete from WhWarehousePickupType where WareHouseSysNo = @0 and PickUpTypeSysNo = @1", whSysNo, pickUpTypeSysNo)
                          .Execute();
        }

        #endregion

        #region 用于借货单中商品查询
        /// <summary>
        /// 获取借货单明细中的商品
        /// </summary>
        /// <param name="deliverymanSysNo">配送员系统编号</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <returns>借货单中的商品</returns>
        /// <remarks>2013-07-11 沈强 创建</remarks>
        /// <remarks>2013-07-23 周唐炬 重构SQL添加为空条件</remarks>
        public override IList<CBPdProductJson> GetProductLendGoods(int deliverymanSysNo, int? userGrade)
        {
            #region selectSql
            string sql = @"select 
                             d.productsysno--商品系统编号
                            ,d.productname --商品名称
                            ,d.productNum  --商品存货数量
                            ,d.price       --会员等级价格
                             from
                            (
                                select g.productsysno
                                     , g.productname
                                     , sum(g.productNum) as productNum
                                     , g.price from 
                                    (
                                           select b.sysno
                                           ,b.productsysno
                                           ,b.productname  
                                           --计算借货单中的实际商品数
                                          ,isnull((b.lendquantity - b.salequantity - b.returnquantity - b.forcecompletequantity),0) as productNum
                                          ,isnull(c.price,-1) as price --没有对应的等级价格
                                          from WhProductLendItem b left join WhProductLendPrice c on b.sysno = c.productlenditemsysno
                                          where exists
                                          (
                                                select 1 from WhProductLend a 
                                                    where a.deliveryusersysno = @0 
                                                          and a.status = @1 
                                                          and a.sysno = b.productlendsysno
                                          ) 
                                            and c.pricesource = @2 
                                            and (@3 is null or c.sourcesysno = @3)
                                    )g group by g.productsysno, g.productname, g.price
                            )d 
                            where productNum > 0";
            #endregion

            var status = (int)Model.WorkflowStatus.WarehouseStatus.借货单状态.已出库;
            var pricesource = (int)Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价;
            var paras = new object[]
                {
                    deliverymanSysNo,
                    status,
                    pricesource,
                    userGrade
                };

            return Context.Sql(sql).Parameters(paras).QueryMany<CBPdProductJson>();
        }

        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="productSysNos">产品系统编号数组</param>
        /// <returns>返回入库单筛选字段集合</returns>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public override IList<ParaProductSearchFilter> ProductSelector(List<int> productSysNos)
        {
            return Context.Sql(@"select p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName from 
                                     pdproduct p 
                                     left join PdCategoryAssociation pa on p.sysno = pa.productsysno
                                     left join pdcategory c on pa.categorysysno = c.sysno
                                      where p.sysno in(@0)", productSysNos).QueryMany<ParaProductSearchFilter>();
        }
        #endregion

        /// <summary>
        /// 获取借货单明细中的商品
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>借货单中的商品</returns>
        /// <remarks>2013-07-18 沈强 创建</remarks>
        /// <remarks>2013-07-18 周唐炬 重构SQL</remarks>
        public override IList<CBPdProductJson> GetProductLendGoods(int deliveryUserSysNo, int? userGrade, int? warehouseSysNo,
                                                                   WarehouseStatus.借货单状态 productLendStatus,
                                                                   ProductStatus.产品价格来源 priceSource)
        {
            #region selectSql WarehouseSysNo

            const string sql = @"SELECT tb.productsysno     --商品系统编号
	                                ,tb.productname         --商品名称
	                                ,tb.price               --商品价格	                                
	                                ,Sum(tb.productNum) as productnum   --商品存货数量
                                FROM (
	                                SELECT b.productsysno
		                                ,b.productname
		                                ,nvl((b.lendquantity - b.salequantity - b.returnquantity - b.forcecompletequantity), 0) AS productnum
		                                ,c.price
	                                FROM WhProductLendItem b
	                                LEFT JOIN WhProductLendPrice c ON b.sysno = c.productlenditemsysno
                                        AND c.pricesource = @0    
		                                AND (@1 IS NULL OR c.sourcesysno = @1)
	                                WHERE EXISTS (
			                                SELECT 1
			                                FROM WhProductLend a
			                                WHERE a.deliveryusersysno = @2
				                                AND a.STATUS = @3
				                                AND a.sysno = b.productlendsysno
                                                AND (@4 IS NULL OR a.warehouseSysNo=@4)
			                                )                                        
	                                ) tb
                                WHERE tb.productNum > 0
                                GROUP BY tb.productsysno
	                                ,tb.productname
	                                ,tb.price
                                ORDER BY tb.productsysno";

            #endregion
            var paras = new object[]
                {
                    (int)priceSource,
                    userGrade,
                    deliveryUserSysNo,
                    (int)productLendStatus,
                    warehouseSysNo
                };
            return Context.Sql(sql).Parameters(paras).QueryMany<CBPdProductJson>();
        }
        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <param name="userSysNo">当前用户编号</param>  
        /// <remarks>2016-5-27 杨浩 创建</remarks>
        public override Pager<WhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize, int? userSysNo)
        {
            var pager = new Pager<WhWarehouse>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string from = "WhWarehouse a";
                string where = " 1=1 ";            
                if (!string.IsNullOrEmpty(condition.WarehouseName))//仓库名称不为空
                {
                    where += " and (a.ErpCode=@condition or  charindex(@condition,a.WarehouseName)>0) ";                
                }
                if (condition.Status.HasValue)//仓库状态
                {
                    where += " and (a.Status=@Status)";
                   
                }

                if (condition.IsSelfSupport.HasValue)//是否自营
                {
                    where += " and (a.IsSelfSupport=@IsSelfSupport)";
                  
                }

                if (condition.WarehouseType.HasValue)//仓库类型
                {
                    where += " and (a.WarehouseType=@WarehouseType)";
                 
                }

                if (condition.DeliveryType.HasValue)//配送方式
                {
                    where += " and exists(select 1 from WhWarehouseDeliveryType whd where whd.warehousesysno=a.sysno and whd.deliverytypesysno=@deliverytypesysno)";
                 
                }

                if (condition.AreaSysNo.HasValue)//仓库地区
                {
                    where += " and a.areasysno in (select sysno from BsArea bs start with bs.sysno =@AreaSysNo connect by prior sysno = ParentSysNo)";
                 
                }
                if (userSysNo.HasValue)
                {
                    where += string.Format(" and exists (select 1 from SyUserWarehouse f  where f.WarehouseSysNo = a.sysno and f.UserSysNo = {0})", userSysNo);
                }
                pager.TotalRows = Context.Select<int>("count(1)")
                       .From(from)
                       .Where(where)
                       .Parameter("condition", condition.WarehouseName)
                       .Parameter("IsSelfSupport", condition.IsSelfSupport)
                       .Parameter("WarehouseType", condition.WarehouseType)
                       .Parameter("deliverytypesysno", condition.DeliveryType)
                       .Parameter("AreaSysNo", condition.AreaSysNo)
                       .Parameter("Status", condition.Status)
                       .QuerySingle();
                pager.Rows = context.Select<WhWarehouse>("a.*")
                                         .From(from)
                                         .Where(where)
                                         .Parameter("condition",condition.WarehouseName)
                                         .Parameter("IsSelfSupport",condition.IsSelfSupport)
                                         .Parameter("WarehouseType", condition.WarehouseType)
                                         .Parameter("deliverytypesysno", condition.DeliveryType)
                                         .Parameter("AreaSysNo", condition.AreaSysNo)
                                         .Parameter("Status", condition.Status)
                                         .OrderBy("a.warehousename collate Chinese_PRC_Stroke_ci_as")
                                         .Paging(pageIndex, pageSize)
                                         .QueryMany();
            }
            return pager;
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>
        /// 2013-08-12 周瑜 创建
        /// 2016-9-27 杨浩 增加仓库权限
        /// </remarks>
        public override Pager<CBWhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize)
        {
            var pager = new Pager<CBWhWarehouse>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string @where = @"(@0 is null or (a.ErpCode = @0 or charindex(@0,a.BackWarehouseName)> 0))
                                        and(@1 is null or a.Status = @1)
                                        and (@2 is null or (areasysno in (select c.sysno from bsarea a  
                                        inner join bsarea b on a.sysno = b.parentsysno
                                        inner join bsarea c on b.sysno = c.parentsysno
                                        where (a.SysNo = @2 or b.sysno = @2 or c.SysNo = @2))))
                                        and (@3 is null or a.warehousetype = @3)
                                        ";
               
                //检查是否拥有所有仓库权限，否则按权限内的仓库筛选
                if (!condition.IsAllWarehouse&&condition.Warehouses!=null)
                {
                    where += " and a.sysNo in ("+(condition.Warehouses.Count>0?string.Join(",",condition.Warehouses.ToDictionary(x=>x.SysNo).Keys):"-1")+") ";
                }
                
                var backwarehousename = condition.BackWarehouseName == null ? null : condition.BackWarehouseName.Trim();
                var parms = new object[]
                    {
                        backwarehousename,
                        condition.Status,
                        condition.AreaSysNo,
                        condition.WarehouseType
                    };

                pager.TotalRows = context.Sql(@"select count(1) from whwarehouse a where " + where)
                                         .Parameters(parms)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBWhWarehouse>("a.*,(select sum(StockQuantity) from PdProductStock  inner join pdproduct on PdProductStock.PdProductSysNo=pdproduct.SysNo  where PdProductStock.WarehouseSysNo = a.SysNo) as SumStockQuantity")
                                                      .From(@"whwarehouse a ")
                                                      .AndWhere(where)
                                                      .Parameters(parms)
                                                      .OrderBy("a.backwarehousename collate Chinese_PRC_Stroke_ci_as")
                                                      .Paging(pageIndex, pageSize)
                                                      .QueryMany();

            }
            return pager;
        }

        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns>仓库编号</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override int Insert(WhWarehouse warehouse)
        {
            if (warehouse.CreatedDate == DateTime.MinValue)
            {
                warehouse.CreatedDate = (DateTime)DateTime.Now;
            }
            if (warehouse.LastUpdateDate == DateTime.MaxValue)
            {
                warehouse.LastUpdateDate = (DateTime)DateTime.Now;
            }
            return Context.Insert("WhWarehouse", warehouse)
                .AutoMap(x => x.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改仓库信息
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override int Update(WhWarehouse warehouse)
        {
            if (warehouse.CreatedDate == DateTime.MinValue)
            {
                warehouse.CreatedDate = (DateTime)DateTime.Now;
            }
            if (warehouse.LastUpdateDate == DateTime.MaxValue)
            {
                warehouse.LastUpdateDate = (DateTime)DateTime.Now;
            }
            return Context.Update("WhWarehouse", warehouse)
                   .AutoMap(x => x.SysNo)
                   .Where(x => x.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 修改仓库状态
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override int UpdateStatus(WhWarehouse warehouse)
        {
            if (warehouse.CreatedDate == DateTime.MinValue)
            {
                warehouse.CreatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (warehouse.LastUpdateDate == DateTime.MinValue)
            {
                warehouse.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Sql("update WhWarehouse set Status = @Status, LastUpdateBy = @LastUpdateBy, LastUpdateDate = @LastUpdateDate where SysNo = @SysNo")
                          .Parameter("Status", warehouse.Status)
                          .Parameter("LastUpdateBy", warehouse.LastUpdateBy)
                          .Parameter("LastUpdateDate", warehouse.LastUpdateDate)
                          .Parameter("SysNo", warehouse.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 查询每个地区下的所有仓库
        /// </summary>
        /// <param name="areaSysNos">地区系统编号</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>所有仓库列表</returns>
        /// <remarks>2013-08-14 周瑜 创建</remarks>
        public override Pager<CBAreaWarehouse> GetWarehouseForArea(int[] areaSysNos, int pageIndex, int pageSize)
        {
            var pager = new Pager<CBAreaWarehouse>() { PageSize = pageSize, CurrentPage = pageIndex };

            #region  debug sql
            /*
               select a.sysno areasysno,a.areaname,wa.warehousesysno defaultwarehousesysno,
(select wm_concat(wa.warehousesysno) from whwarehousearea wa
 where wa.areasysno=a.sysno
 )as AllWarehouseSysNo
from bsarea a
left join whwarehousearea wa on a.sysno=wa.areasysno and wa.isdefault=1
where   a.sysno in (5150)
order by a.sysno desc
            
             */
            #endregion
            /*    //源代码
            var allareasysno = string.Join(",", areaSysNos);


            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string where = string.IsNullOrWhiteSpace(allareasysno)
                                   ? " a.arealevel=3"
                                   : string.Format("  a.arealevel=3 and a.sysno in ({0})", allareasysno);
                pager.Rows =
                    context.Select<CBAreaWarehouse>(@"a.sysno as  areasysno,
                            a.status,a.areaname,
                            wa.warehousesysno defaultwarehousesysno,
                                (select wm_concat(wa.warehousesysno) from whwarehousearea wa
                                    where wa.areasysno=a.sysno ) as AllWarehouseSysNo"
                        )
                           .From(@" bsarea a left join whwarehousearea wa on a.sysno=wa.areasysno and wa.isdefault=1")
                           .Where(where)
                           .OrderBy(" NLSSORT(a.areaname,'NLS_SORT=SCHINESE_PINYIN_M')")
                           .Paging(pager.CurrentPage, pager.PageSize).QueryMany();

                pager.TotalRows = context.Select<int>(@"count(1) ")
                                         .From(
                                             @"bsarea a left join whwarehousearea wa on a.sysno=wa.areasysno and wa.isdefault=1")
                                         .Where(where)
                                         .QuerySingle();
            }
            return pager;
        }*/


            var allareasysno = string.Join(",", areaSysNos);


            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string where = string.IsNullOrWhiteSpace(allareasysno)
                                   ? " a.arealevel=3"
                                   : string.Format("  a.arealevel=3 and a.sysno in ({0})", allareasysno);
                pager.Rows =
                    //                    context.Select<CBAreaWarehouse>(@"a.sysno as  areasysno,
                    //                            a.status,a.areaname,
                    //                            wa.warehousesysno defaultwarehousesysno,
                    //                                (select wa.warehousesysno from whwarehousearea wa
                    //                                    where wa.areasysno=a.sysno ) as AllWarehouseSysNo"
                    //                        )
                        context.Select<CBAreaWarehouse>(@"a.sysno as  areasysno,
                            a.status,a.areaname,
                            wa.warehousesysno defaultwarehousesysno,
                                (select cast( wa.warehousesysno as nvarchar(100))+',' from whwarehousearea wa where wa.areasysno=a.sysno for xml path('')) as AllWarehouseSysNo"
                        )
                           .From(@" bsarea a left join whwarehousearea wa on a.sysno=wa.areasysno and wa.isdefault=1")
                           .Where(where)
                           .OrderBy(" a.areaname Collate Chinese_PRC_Stroke_ci_as ")
                           .Paging(pager.CurrentPage, pager.PageSize).QueryMany();

                pager.TotalRows = context.Select<int>(@"count(1) ")
                                         .From(
                                             @"bsarea a left join whwarehousearea wa on a.sysno=wa.areasysno and wa.isdefault=1")
                                         .Where(where)
                                         .QuerySingle();
            }
            return pager;
        }




        /// <summary>
        /// 获取支持该服务地区的对应的仓库
        /// </summary>
        /// <param name="supportArea">支持地区</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-08-28 朱成果 创建</remarks>
        public override List<WhWarehouse> GetWhWarehouseBySupportArea(int supportArea, int? warehouseType)
        {
            string sql = @"with tb as
                        (
                              select  t1. * from WhWarehouse t1
                              left outer join WhWarehouseArea  t2
                              on t1.sysno=t2.warehousesysno
                              where (@WarehouseType is null  or t1.warehousetype=@WarehouseType) and (t2.areasysno=@areasysno or t1.areasysno=@areasysno)
                              order by t2.isdefault desc
                        ) 
                        select distinct * from tb order  by warehousename Collate Chinese_PRC_Stroke_ci_as)

                        ";

            return Context.Sql(sql)
                .Parameter("WarehouseType", warehouseType)
                //.Parameter("WarehouseType", warehouseType)
                .Parameter("areasysno", supportArea)
                //.Parameter("areasysno", supportArea)
                .QueryMany<WhWarehouse>();

        }

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <param name="maxLatAngle">纬度相差最大角度</param>
        /// <param name="maxLngAngle">经度相差最大角度</param>
        /// <returns>门店列表</returns>
        /// <remarks>
        /// 2013-09-11 郑荣华 创建
        /// </remarks>
        /// <remarks>2014-07-03 周唐炬 过滤仓库，只获取门店</remarks>
        public override IList<CBWhWarehouse> GetWarehouseByMap(double lat, double lng, double maxLatAngle,
                                                             double maxLngAngle)
        {
            const string sql = @"select t.*,a.areaname from whwarehouse t left join bsarea a on t.areasysno=a.sysno
                                 where abs(t.latitude-@lat)<=@maxLatAngle and abs(t.longitude-@lng)<=@maxLngAngle
                                       and t.WarehouseType=@WarehouseType
                                  order  by t.warehousename Collate Chinese_PRC_Stroke_ci_as
                                 ";

            return Context.Sql(sql)
                            .Parameter("lat", lat)
                            .Parameter("maxLatAngle", maxLatAngle)
                            .Parameter("lng", lng)
                            .Parameter("maxLngAngle", maxLngAngle)
                            .Parameter("WarehouseType", WarehouseStatus.仓库类型.门店.GetHashCode())
                            .QueryMany<CBWhWarehouse>();
        }

        /// <summary>
        /// 根据服务地区,仓库类型,支持的物流类型获取仓库
        /// </summary>
        /// <param name="supportArea">服务覆盖地区.</param>
        /// <param name="warehouseType">类型,门店仓库.</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <param name="status">仓库状态: 禁用/启用</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public override List<WhWarehouse> GetWhWareHouse(int? supportArea = null, WarehouseStatus.仓库类型? warehouseType = null, int? deliveryType = null, WarehouseStatus.仓库状态? status = null)
        {
            //dintinct 会打乱原有的数据的排序，所以不用dintinct 用 Row_Number()+min达到效果
            const string sql = @"
                  with tb as (
                                   select  t1.sysNo,Row_Number() over(order by  t2.isdefault desc,warehousename Collate Chinese_PRC_Stroke_ci_as) as rum  
                                   from WhWarehouse t1
                                   left join WhWarehouseArea  t2 on t1.sysno=t2.warehousesysno
                                   left join whwarehousedeliverytype t3 on t1.sysno = t3.warehousesysno
                                   where 1 = 1 and (@WarehouseType is null  or t1.warehousetype=@WarehouseType) 
                                               and (@areasysno is null or t2.areasysno=@areasysno)
                                               and (@DeliveryTypeSysNo is null or t3.DeliveryTypeSysNo = @DeliveryTypeSysNo)
                                               and (@Status is null or t1.Status = @Status)
                                
                             )
                select WhWarehouse.* from WhWarehouse 
                inner join 
                (
                    select sysNo,min(rum) as rum
                    from tb 
                    group by sysNo
                ) tb1 
                on WhWarehouse.Sysno=tb1.sysNo
                order by rum asc ";
            var lst = Context.Sql(sql)
                          .Parameter("WarehouseType", warehouseType)
                          .Parameter("areasysno", supportArea)
                          .Parameter("DeliveryTypeSysNo", deliveryType)
                          .Parameter("Status", status)
                          .QueryMany<WhWarehouse>();
            return lst;
        }

        /// <summary>
        /// 获取仓库by erpcode
        /// </summary>
        /// <param name="erpCode">erpCode</param>
        /// <returns>WhWarehouse</returns>
        /// <remarks>2013-11-13 huangwei 创建</remarks>
        public override WhWarehouse GetWhWareHouseByErpCode(string erpCode)
        {
            return
                Context.Sql("select * from  whwarehouse where erpcode=@erpCode")
                       .Parameter("erpCode", erpCode)
                       .QuerySingle<WhWarehouse>();
        }

        /// <summary>
        /// 查询第三方配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 杨浩 创建</remarks>
        public override Pager<CBWhStockOut> SearchThirdPartyStockOut(int stockOutStatus, int userSysNo, bool isHasAllWarehouse, int pageIndex, int pageSize, int orderSysNo, int warehouseSysNo, string sort, string sortBy)
        {
            var pager = new Pager<CBWhStockOut>() { PageSize = pageSize, CurrentPage = pageIndex };
            string from = @"WhStockOut a 
                          inner join WhWarehouse b on a.WarehouseSysNo=b.SysNo
                          inner join SoReceiveAddress c on c.SysNo=a.ReceiveAddressSysNo
                          inner join SoOrder  d on d.SysNo=a.OrderSysNO";
            string select = "a.*,b.WarehouseName,c.Name as ReceiverName,d.OrderSource,d.CreateDate as SoCreateDate,d.CustomerMessage as SoCustomerMessage,d.InternalRemarks as SoInternalRemarks";
            string where = " (a.InvoiceSysNo = 0 or a.InvoiceSysNo is null)";
            //罗勤尧增加排序条件
            string orderby = "";
            if (sort == "desc" && sortBy == "SysNo")
            {
                orderby = "d.SysNo desc";
            }
            else if (sort == "ASC" && sortBy == "SysNo")
            {
                orderby = "a.SysNo ASC";
            }
            else if (sort == "desc" && sortBy == "createtime")
            {
                orderby = "a.CreatedDate desc";
            }

            else if (sort == "ASC" && sortBy == "createtime")
            {
                orderby = "a.CreatedDate ASC";
            }else
            {
                orderby = "d.SysNo desc";
            }
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                if (!isHasAllWarehouse)
                {
                    where += string.Format(" and exists (select 1 from SyUserWarehouse f  where f.WarehouseSysNo = a.WarehouseSysNo and f.UserSysNo = {0})", userSysNo);
                }

                if ( orderSysNo > 0)
                {
                    where += " and d.SysNo=" + orderSysNo;
                }
                if (stockOutStatus > 0)
                {
                    where += " and a.Status=" + stockOutStatus;
                }

                if (warehouseSysNo > 0)
                {
                    where += " and a.warehouseSysNo=" + warehouseSysNo;
                }


                pager.Rows = context.Select<CBWhStockOut>(select).From(from)
                    .Where(where).OrderBy(orderby).Paging(pageIndex, pageSize).QueryMany();
                pager.TotalRows = context.Select<int>("count(1)").From(from)
                    .Where(where).QuerySingle();
            } 
            return pager;
        }

        /// <summary>
        /// / 查询当日达配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-09-24 朱成果 创建</remarks>
        public override Pager<CBWhStockOut> SearchDRDStockOut(int stockOutStatus, int userSysNo, bool isHasAllWarehouse, int pageIndex, int pageSize)
        {
            var pager = new Pager<CBWhStockOut>() { PageSize = pageSize, CurrentPage = pageIndex };
            string from = @"WhStockOut a 
                          inner join WhWarehouse b on a.WarehouseSysNo=b.SysNo
                          inner join SoReceiveAddress c on c.SysNo=a.ReceiveAddressSysNo
                          inner join SoOrder  d on d.SysNo=a.OrderSysNO";
            string select = "a.*,b.WarehouseName,c.Name as ReceiverName,d.OrderSource,d.CreateDate as SoCreateDate,d.CustomerMessage as SoCustomerMessage,d.InternalRemarks as SoInternalRemarks";
            string where = "a.Status=" + stockOutStatus + " and a.DeliveryTypeSysNo in (select SysNo from LgDeliveryType where ParentSysNo=" + Hyt.Model.SystemPredefined.DeliveryType.百城当日达 + ")  and (a.InvoiceSysNo = 0 or a.InvoiceSysNo is null)";
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                if (!isHasAllWarehouse)
                {
                    where += string.Format(" and exists (select 1 from SyUserWarehouse f  where f.WarehouseSysNo = a.WarehouseSysNo and f.UserSysNo = {0})", userSysNo);
                }
                pager.Rows = context.Select<CBWhStockOut>(select).From(from)
                    .Where(where).OrderBy("a.CreatedDate asc").Paging(pageIndex, pageSize).QueryMany();
                pager.TotalRows = context.Select<int>("count(1)").From(from)
                    .Where(where).QuerySingle(); ;
            }
            return pager;
        }

        public override bool CheckWarehouseName(WarehouseSearchCondition condition)
        {
            string sqlText = @"select COUNT(1) from whwarehouse WH where (@0 is null or (WH.ErpCode = @0 or @0=WH.WarehouseName))
                                        and(@1 is null or WH.Status = @1)
                                        and (@2 is null or (areasysno in (select AR3.sysno from bsarea AR1  
                                        inner join bsarea AR2 on AR1.sysno = AR2.parentsysno
                                        inner join bsarea AR3 on AR2.sysno = AR3.parentsysno
                                        where (AR1.SysNo = @2 or AR2.sysno = @2 or AR3.SysNo = @2))))
                                        and (@3 is null or WH.warehousetype = @3)";
            var parameters = new object[]
                    {
                        condition.WarehouseName,
                        condition.Status,
                        condition.AreaSysNo,
                        condition.WarehouseType
                    };
            int RowCount = 0;
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                RowCount = context.Sql(sqlText).Parameters(parameters).QuerySingle<int>();
            }

            return RowCount > 0 ? true : false;
        }
        /// <summary>
        /// 获取对应仓库列表
        /// 王耀发 2016-1-23 创建
        /// </summary>
        /// <returns></returns>
        public override List<WhWarehouse> GetWhWareHouseList()
        {
            return Context.Sql(@"select a.SysNo,a.BackWarehouseName from WhWarehouse a where a.Status = 1")
                         .QueryMany<WhWarehouse>();
        }
        /// <summary>
        /// 是否关联过仓库
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public override int ExitWarehouse(int DealerSysNo)
        {
            string sql = "SELECT count(*) from [DsDealerWharehouse] where  DealerSysNo<>0 and DealerSysNo<>108 and DealerSysNo=" + DealerSysNo;
            return Context.Sql(sql).QuerySingle<int>();
        }

        public override List<WhWarehouse> GetAllWarehouseListBySysNos(string sysNos)
        {
            if(!string.IsNullOrEmpty(sysNos.Trim()))
            {
                string sql = " select * from WhWarehouse where SysNo in (" + sysNos + ") ";
                return Context.Sql(sql).QueryMany<WhWarehouse>();
            }
            else
            {
                return new List<WhWarehouse>();
            }
        }
    }
}
