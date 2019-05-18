using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商商城数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-09-18 郑荣华 创建
    /// </remarks>
    public class DsDealerMallDaoImpl : IDsDealerMallDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商商城
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// 2017-05-5 罗勤尧修改 添加erp编号
        /// </remarks>
        public override int Create(DsDealerMall model)
        {
            return Context.Insert("DsDealerMall", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商商城,授权码为空不更新
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public override int Update(DsDealerMall model)
        {
            if (string.IsNullOrEmpty(model.AuthCode))
            {
                return Context.Update("DsDealerMall", model)
                      .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate, x => x.AuthCode)
                      .Where(x => x.SysNo)
                      .Execute();
            }
            return Context.Update("DsDealerMall", model)
                        .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                        .Where(x => x.SysNo)
                        .Execute();
        }



        /// <summary>
        /// 分销商商城状态更新
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <param name="status">分销商商城状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public override int UpdateStatus(int sysNo, DistributionStatus.分销商商城状态 status, int lastUpdateBy)
        {
            return Context.Sql("update DsDealerMall set status=@0,lastupdateby=@1,lastupdatedate=@2 where sysno=@3", (int)status, lastUpdateBy, DateTime.Now, sysNo)
                           .Execute();
        }
        #endregion
        /// <summary>
        /// 分销商授权码获取
        /// </summary>
        /// <param name="AuthCode">授权码</param>
        /// <param name="shopid">分销商编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2017-9-1 杨大鹏 创建</remarks>
        public override int UpdateAuthCode(string accessToken, int shopid)
        {

            return Context.Sql("update DsDealerMall set AuthCode =@0 where sysno=@1", accessToken,shopid)
                .Execute();
        
        }


        #region 查询
        /// <summary>
        /// 分页查询分销商商城信息列表
        /// </summary>
        /// <param name="pager">分销商商城信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public override void GetDsDealerMallList(ref Pager<CBDsDealerMall> pager, ParaDsDealerMallFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"t.*,a.dealername,b.mallcode,b.mallname,c.AppName";

                const string sqlFrom = @"DsDealerMall t
                                        left join DsDealer a on t.dealersysno=a.sysno
                                        left join DsMallType b on t.malltypesysno=b.sysno
                                        left join DsDealerApp c on t.DealerAppSysNo=c.sysno
                                        ";

                const string sqlWhere = @"(@ShopName is null or charindex(t.ShopName,@ShopName)>0)                
                                          and (@DealerSysNo is null or t.DealerSysNo= @DealerSysNo)
                                          and (@status is null or t.status= @status)
                                          and (@MallTypeSysNo is null or t.MallTypeSysNo=@MallTypeSysNo)
                                          and (@IsSelfSupport is null or t.IsSelfSupport=@IsSelfSupport)                 
                                          ";

                #region sqlcount

                const string sqlCount = @" select count(1) from DsDealerMall t where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("ShopName", filter.ShopName)
                                         .Parameter("DealerSysNo", filter.DealerSysNo)
                                         .Parameter("status", filter.Status)
                                         .Parameter("MallTypeSysNo", filter.MallTypeSysNo)
                                         .Parameter("IsSelfSupport", filter.IsSelfSupport)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBDsDealerMall>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("ShopName", filter.ShopName)
                                    .Parameter("DealerSysNo", filter.DealerSysNo)
                                    .Parameter("status", filter.Status)
                                    .Parameter("MallTypeSysNo", filter.MallTypeSysNo)
                                    .Parameter("IsSelfSupport", filter.IsSelfSupport)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="ridSysNo">排除的商城系统编号</param>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <param name="shopAccount">店铺账号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建 作唯一性检查使用（商城类型系统编号+店铺账号 唯一）
        /// </remarks>
        public override CBDsDealerMall GetDsDealerMall(int? ridSysNo, int mallTypeSysNo, string shopAccount)
        {
            const string sql = @"select t.* from DsDealerMall t ";
            const string sqlWhere = @" where (@SysNo is null or t.SysNo <> @SysNo)
                                        and (@MallTypeSysNo is null or t.MallTypeSysNo=@MallTypeSysNo)
                                        and (@ShopAccount is null or t.ShopAccount=@ShopAccount)                 
                                        ";
            return Context.Sql(sql + sqlWhere)
                          .Parameter("SysNo", ridSysNo)
                          .Parameter("MallTypeSysNo", mallTypeSysNo)
                          .Parameter("ShopAccount", shopAccount == "" ? " " : shopAccount)
                          .QuerySingle<CBDsDealerMall>();
        }
        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks> 
        /// 2016-07-06 杨浩 创建 
        /// </remarks>
        public override DsDealerMall GetDsDealerMall(int mallTypeSysNo, int dealerSysNo)
        {
            const string sql = @"select top 1 * from DsDealerMall";
            const string sqlWhere = @" where MallTypeSysNo=@MallTypeSysNo
                                        and dealerSysNo=@dealerSysNo)                 
                                        ";
            return Context.Sql(sql + sqlWhere)                       
                          .Parameter("MallTypeSysNo", mallTypeSysNo)
                          .Parameter("dealerSysNo", dealerSysNo)
                          .QuerySingle<DsDealerMall>();
        }
        /// <summary>
        /// 获取分销商商城信息 
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <returns>分销商商城息信息</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public override CBDsDealerMall GetDsDealerMall(int sysNo)
        {
            const string sql = @"select t.*,a.dealername,b.mallcode,b.mallname from DsDealerMall t
                                left join DsDealer a on t.dealersysno=a.sysno
                                left join DsMallType b on t.malltypesysno=b.sysno
                                where t.sysno=@0";

            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBDsDealerMall>();
        }
        /// <summary>
        /// 获取分销商商城列表
        /// </summary>
        /// <returns>分销商商城列表</returns>
        /// <remarks>
        /// 2014-02-18 朱成果  创建
        /// </remarks>
        public override List<DsDealerMall> GetList()
        {
            return Context.Sql("select * from DsDealerMall").QueryMany<DsDealerMall>();
        }

        /// <summary>
        /// 获取分销商商城实体
        /// </summary>
        /// <param name="sysNO">分销商商城编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-06-11 朱成果 创建
        /// </remarks>
        public override DsDealerMall GetEntity(int sysNO)
        {

            return Context.Sql("select * from DsDealerMall where SysNo=@SysNo")
               .Parameter("SysNo", sysNO).QuerySingle<DsDealerMall>(); 
        }

        /// <summary>
        /// 获取分销商商城实体
        /// </summary>
        /// <param name="DealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-06-11 朱成果 创建
        /// </remarks>
        //public override DsDealerMall GetEntityByDsId(int DealerSysNo)
        //{

        //    return Context.Sql("select t.* from DsDealerMall t  where t.DealerSysNo=@DealerSysNo")
        //       .Parameter("DealerSysNo", DealerSysNo).QuerySingle<DsDealerMall>();
        //}

        /// <summary>
        /// 通过DealerAppSysNo查找商城关联数量
        /// </summary>
        /// <param name="appSysNo">appkey系统编号</param>
        /// <returns>关联数量</returns>
        public override int  GetAppKeyUseNum(int appSysNo)
        {
            return Context.Sql("select count(1) from DsDealerMall where DealerAppSysNo=@appSysNo")
               .Parameter("appSysNo", appSysNo).QuerySingle<int>(); 
        }
        /// <summary>
        /// 获取分销商商城信息 
        /// </summary>
        /// <param name="DealerSysNo">分销商商城系统编号</param>
        /// <returns>分销商商城息信息</returns>
        /// <remarks>
        /// 2015-12-11 王耀发 创建
        /// </remarks>
        public override CBDsDealerMall GetDsDealerMallByDealerSysNo(int DealerSysNo)
        {
            const string sql = @"select t.*,a.dealername,b.mallcode,b.mallname from DsDealerMall t
                                left join DsDealer a on t.dealersysno=a.sysno
                                left join DsMallType b on t.malltypesysno=b.sysno
                                where t.DealerSysNo=@0";

            return Context.Sql(sql, DealerSysNo)
                          .QuerySingle<CBDsDealerMall>();
        }
        /// <summary>
        /// 根据商城类型获取所有商城
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-02 杨浩 创建</remarks>
        public override IList<DsDealerMall> GetDealerMallByMallTypeSysNo(int mallTypeSysNo)
        {
            return Context.Sql(@"select DealerSysNo,MallTypeSysNo,ShopAccount,ShopName,AuthCode,Status,IsSelfSupport,DealerAppSysNo,ErpSysNo,SysNo,DefaultWarehouse from DsDealerMall 
                                where Status=1 and mallTypeSysNo=@mallTypeSysNo ")
               .Parameter("mallTypeSysNo", mallTypeSysNo)
               .QueryMany<DsDealerMall>(); 
        }
        #endregion
    }
}
