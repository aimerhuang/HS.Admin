using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.ExpressList;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 配送方式访问类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class LgDeliveryTypeDaoImpl : ILgDeliveryTypeDao
    {
        #region 操作

        /// <summary>
        /// 创建配送方式
        /// </summary>
        /// <param name="model">配送方式实体</param>
        /// <returns>创建的配送方式SysNo</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override int Create(LgDeliveryType model)
        {
            return Context.Insert("LgDeliveryType", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新配送方式
        /// </summary>
        /// <param name="model">配送方式实体，根据SysNo</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override int Update(LgDeliveryType model)
        {
            return Context.Update("LgDeliveryType", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="sysNo">要删除的配送方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("LgDeliveryType")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql("select count(1) from LgDeliveryType").QuerySingle<int>();

                #region 调试Sql

                /* 
             select t.*  from LgDeliveryType t order by deliveryLevel,DisplayOrder
             */

                #endregion

                pager.Rows = context.Select<CBLgDeliveryType>("t.*,b.deliveryTypeName ParentName")
                                    .From("LgDeliveryType t left join LgDeliveryType b on t.parentsysno=b.sysno")
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }
        public override string GetorderId(int sysNo)
        {
            return Context.Sql("select OrderSysNO from WhStockOut where SysNo =@0",sysNo).QuerySingle<string>();
        }
        /// <summary>
        /// 查询配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public override void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager, ParaDeliveryTypeFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
               
//                const string sqlWhere = @"
//                (@deliverytypename is null or charindex(t.deliverytypename,@deliverytypename)>0) 
//                and (@isonlinevisible is null or t.isonlinevisible= @isonlinevisible)
//                and (@status is null or t.status= @status)
//                and (@parentsysno is null or t.parentsysno=@parentsysno)
//                and (@SysNoFilter is null or not exists(select 1 from table(splitstr(@SysNoFilter,',')) z where z.column_value = t.sysno))                       
//               ";
                const string sqlWhere = @"
                (@deliverytypename is null or charindex(t.deliverytypename,@deliverytypename)>0) 
                and (@isonlinevisible is null or t.isonlinevisible= @isonlinevisible)
                and (@status is null or t.status= @status)
                and (@parentsysno is null or t.parentsysno=@parentsysno)         
               ";
                

                #region sqlcount

                const string sqlCount = @" select count(1) from lgdeliverytype t where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                       .Parameter("deliverytypename", filter.DeliveryTypeName)
                                       //.Parameter("deliverytypename", filter.DeliveryTypeName)
                                       .Parameter("isonlinevisible", filter.IsOnlineVisible)
                                       //.Parameter("isonlinevisible", filter.IsOnlineVisible)
                                       .Parameter("status", filter.Status)
                                       //.Parameter("status", filter.Status)
                                       .Parameter("parentsysno", filter.ParentSysNo)
                                       //.Parameter("parentsysno", filter.ParentSysNo)
                                       .Parameter("SysNoFilter", filter.SysNoFilter)
                                       //.Parameter("SysNoFilter", filter.SysNoFilter)
                                       .QuerySingle<int>();
                #endregion

                #region 调试Sql

                /* 
             select t.*  from LgDeliveryType t where DeliveryTypeName like :DeliveryTypeName order by deliveryLevel,DisplayOrder
             */

                #endregion

                pager.Rows = context.Select<CBLgDeliveryType>("t.*,b.deliveryTypeName ParentName")
                                    .From("LgDeliveryType t left join LgDeliveryType b on t.parentsysno=b.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("deliverytypename", filter.DeliveryTypeName)
                                    //.Parameter("deliverytypename", filter.DeliveryTypeName)
                                    .Parameter("isonlinevisible", filter.IsOnlineVisible)
                                    //.Parameter("isonlinevisible", filter.IsOnlineVisible)
                                    .Parameter("status", filter.Status)
                                    //.Parameter("status", filter.Status)
                                    .Parameter("parentsysno", filter.ParentSysNo)
                                    //.Parameter("parentsysno", filter.ParentSysNo)
                                    .Parameter("SysNoFilter", filter.SysNoFilter)
                                    //.Parameter("SysNoFilter", filter.SysNoFilter)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 根据名称获取配送方式信息
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <returns>单个配送方式信息</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override LgDeliveryType GetLgDeliveryType(string deliveryTypeName)
        {
            return Context.Sql("select * from LgDeliveryType where deliveryTypeName=@0", deliveryTypeName)
                          .QuerySingle<LgDeliveryType>();
        }

        /// <summary>
        /// 除去传入的系统编号，根据名称获取其它配送方式信息
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <param name="sysNo">除去的配送方式系统</param>
        /// <returns>单个配送方式信息</returns>
        /// <remarks> 
        /// 2013-07-02 郑荣华 创建
        /// </remarks>
        public override LgDeliveryType GetLgDeliveryTypeForUpdate(string deliveryTypeName, int sysNo)
        {
            return Context.Sql("select * from LgDeliveryType where deliveryTypeName=@0 and sysNo<>@1", deliveryTypeName, sysNo)
                          .QuerySingle<LgDeliveryType>();
        }

        /// <summary>
        /// 获取子配送方式
        /// </summary>
        /// <param name="sysNo">配送方式系统编号，为0时获取第一级配送方式</param>
        /// <returns>子配送方式列表</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// 2014-08-11 余勇 修改 过滤状态为禁用的配送方式
        /// </remarks>
        public override IList<LgDeliveryType> GetSubLgDeliveryTypeList(int sysNo = 0)
        {
            return Context.Sql("select * from LgDeliveryType where ParentSysNo=@ParentSysNo and Status=@Status")
                          .Parameter("ParentSysNo", sysNo)
                          .Parameter("Status", (int)LogisticsStatus.配送方式状态.启用)
                          .QueryMany<LgDeliveryType>();
        }

        /// <summary>
        /// 根据系统编号获取配送方式
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        public override CBLgDeliveryType GetLgDeliveryType(int sysNo)
        {
            const string sql = @"select t.*,b.deliveryTypeName ParentName from LgDeliveryType t 
                                 left join LgDeliveryType b on t.parentsysno=b.sysno where t.sysno=@sysNo ";
            return Context.Sql(sql)
                           .Parameter("sysno", sysNo)
                          .QuerySingle<CBLgDeliveryType>();
        }



        /// <summary>
        /// 根据系统编号获取快递单号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2017-08-15 吴琨 创建
        /// </remarks>
        public override string GetExpressNo(int sysNo)
        {
            return Context.Sql("select  ExpressNo  from LgDeliveryItem  where NoteSysNo=@0", sysNo).QuerySingle<string>();
        }

        /// <summary>
        /// 根据系统编号获取快递单号，时间，配送方式名称
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2018-1-10 廖移凤 创建
        /// </remarks>
        public override KuaiDi GetKuaidi(int sysNo)
        {
            return Context.Sql("select l.ExpressNo,l.CreatedDate,ldt.DeliveryTypeName from LgDeliveryItem  l,LgDelivery ld,LgDeliveryType ldt where l.DeliverySysNo=ld.SysNo and ld.DeliveryTypeSysNo=ldt.SysNo and NoteSysNo=@0", sysNo).QuerySingle<KuaiDi>();
        }


        /// <summary>
        /// 获取仓库与配送方式对应关系信息
        /// 最后放到对应类，目前未建类
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>仓库与配送方式对应关系列表</returns>
        /// <remarks> 
        /// 2013-06-26 郑荣华 创建
        /// </remarks>
        public override IList<WhWarehouseDeliveryType> GetWhWarehouseDeliveryType(int deliveryTypeSysNo)
        {
            return Context.Sql("select * from WhWarehouseDeliveryType where DeliveryTypeSysNo=@0 ", deliveryTypeSysNo)
                          .QueryMany<WhWarehouseDeliveryType>();
        }

        /// <summary>
        /// 根据仓库编号获取第三方快递配送方式信息
        /// </summary>
        /// <param name="wareshouSysNo">仓库系统编号</param>
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-06-28 周瑜 创建
        /// 2014-08-11 余勇 修改 过滤状态为禁用的配送方式
        /// </remarks>
        public override IList<LgDeliveryType> GetLgDeliveryTypeByWarehouse(int wareshouSysNo)
        {
            return Context.Sql(@"select b.* from WhWarehouseDeliveryType a inner join LgDeliveryType b
                                on a.DeliveryTypeSysNo = b.sysno
                                where  a.warehousesysno = @warehousesysno and b.Status=@Status")
                          .Parameter("warehousesysno", wareshouSysNo)
                          .Parameter("Status", (int)LogisticsStatus.配送方式状态.启用)
                          .QueryMany<LgDeliveryType>();
        }

        /// <summary>
        /// 查询所有的配送方式
        /// </summary>     
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-08-08 周瑜 创建
        /// 2014-08-11 余勇 修改 过滤状态为禁用的配送方式
        /// </remarks>
        public override IList<LgDeliveryType> GetLgDeliveryTypeList()
        {
            return Context.Sql(@"select * from LgDeliveryType where Status=@Status order by displayorder")
                          .Parameter("Status", (int)LogisticsStatus.配送方式状态.启用)
                          .QueryMany<LgDeliveryType>();
        }

        /// <summary>
        /// 查询所有的父级配送方式
        /// </summary>     
        /// <returns>父级配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-09-18 黄伟 创建
        /// 2014-08-11 余勇 修改 过滤状态为禁用的配送方式
        /// </remarks>
        public override IList<LgDeliveryType> GetLgDeliveryTypeParent()
        {
            return Context.Sql(@"select * from LgDeliveryType where parentsysno=0 and Status=@Status")
                          .Parameter("Status", (int)LogisticsStatus.配送方式状态.启用)
                          .QueryMany<LgDeliveryType>();
        }

        #endregion

        /// <summary>
        /// 获取物流类型
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public override LgDeliveryType GetDeliveryTypeByCode(string typeCode)
        {
            string sql = " select * from  LgDeliveryType where OverseaCarrier = '" + typeCode + "' ";
            return Context.Sql(sql).QuerySingle<LgDeliveryType>();
        }
    }
}
