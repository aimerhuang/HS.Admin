using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商信息维护数据访问层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerDaoImpl : IDsDealerDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Create(DsDealer model)
        {
            return Context.Insert("DsDealer", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Update(DsDealer model)
        {
            return Context.Update("DsDealer", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 分销商状态更新
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="status">分销商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int UpdateStatus(int sysNo, DistributionStatus.分销商状态 status, int lastUpdateBy)
        {
            return Context.Sql("update DsDealer set status=@0,lastupdateby=@1,lastupdatedate=@2 where sysno=@3", (int)status, lastUpdateBy, DateTime.Now, sysNo)
                           .Execute();
        }
        #endregion

        #region 查询
        /// 根据企业编号获取加盟商信息
        /// </summary>
        /// <param name="enterpriseID">企业编号</param>
        /// <returns>经销商信息</returns>
        /// <remarks>2015-01-29 杨浩 创建</remarks>
        public override DsDealer GetDealerByEnterpriseID(int enterpriseID)
        {
            //单个企业编号应对应一个企业
            const string sql = @"select t.* from dsdealer t where t.enterpriseID=@enterpriseID";
            return Context.Sql(sql, enterpriseID)
                          .QuerySingle<DsDealer>();

        }
        /// <summary>
        /// 通过过滤条件获取分销商列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商列表</returns>
        ///<remarks>2013-09-03 周唐炬 创建</remarks>
        public override Pager<CBDsDealer> GetDealerList(ParaDealerFilter filter)
        {
            string sqlWhere = "1=1";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and A.SysNo = @3";
                }
                else
                {
                    sqlWhere += " and A.CreatedBy = @4";
                }
            }
            if (filter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and A.SysNo = @5";
            }
            string sql = @"(SELECT A.*
                                    ,B.SysNo As PrePaymentSysNo
	                                ,B.TOTALPRESTOREAMOUNT
	                                ,B.AVAILABLEAMOUNT
	                                ,B.FROZENAMOUNT
                                FROM DsDealer A
                                LEFT JOIN DsPrePayment B ON A.Sysno = B.dealersysno
                                WHERE (@0 IS NULL OR A.AREASYSNO = @0)
	                                AND (@1 IS NULL OR A.STATUS = @1)
	                                AND (@2 IS NULL OR charindex(A.DEALERNAME, @2) > 0)
                                    AND " + sqlWhere + ") tb";

            var dataList = Context.Select<CBDsDealer>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                   filter.AddressSysNo,
                   filter.Status,
                   filter.DealerName,
                   filter.DealerSysNo,
                   filter.DealerCreatedBy,
                   filter.SelectedDealerSysNo
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsDealer>()
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }

        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override CBDsDealer GetDsDealer(int sysNo)
        {
            const string sql = @"select t.*,a.levelname,b.account,b.username,dbo.func_getaereapath(t.areasysno) AreaAllName, 
                                     c.TOTALPRESTOREAMOUNT
	                                ,c.AVAILABLEAMOUNT
	                                ,c.FROZENAMOUNT       
                                    ,[dbo].[func_GetCustomerNums](t.sysno) as CustomerNums
                                from DsDealer t 
                                left join dsdealerlevel a on t.levelsysno=a.sysno
                                left join syuser b on t.usersysno=b.sysno
                                left join DsPrePayment c on t.Sysno = c.dealersysno
                                where t.sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBDsDealer>();
        }

        /// <summary>
        /// 根据系统编号获取分销商信息利嘉模板
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override DsDealerLiJiaEdit GetDsDealerLiJia(int sysNo)
        {
            const string sql = @"select b.sysno as MemberId ,t.DealerName as MemberName,t .Contact as Contact,t.PhoneNumber as PhoneNumber ,b.MobilePhoneNumber as CellPhone ,'' as Fax,t.EmailAddress as Email,q2.AreaName as Province ,q1.AreaName as City,q.AreaName as District,t.StreetAddress as AddressLine,'' as BankAccountdbo,'' as QQ,t.DealerName as UserName  
                                ,'' as BrandName from DsDealer t 
                                left join dsdealerlevel a on t.levelsysno=a.sysno
                                left join syuser b on t.usersysno=b.sysno
                                left join DsPrePayment c on t.Sysno = c.dealersysno
                                left join BsArea q on t.AreaSysNo=q.SysNo
								   left join BsArea q1 on q.ParentSysNo=q1.SysNo
								   left join BsArea q2 on q1.ParentSysNo=q2.SysNo            
                                where t.usersysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsDealerLiJiaEdit>();
        }
        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override CBDsDealer GetCBDsDealer(int sysNo)
        {
            const string sql = @"select t.*,a.levelname,b.account,b.username,dbo.func_getaereapath(t.areasysno) AreaAllName, 
                                     c.TOTALPRESTOREAMOUNT
	                                ,c.AVAILABLEAMOUNT
	                                ,c.FROZENAMOUNT
                                    ,[dbo].[func_GetCustomerNums](t.sysno) as CustomerNums
                                    ,[dbo].[func_GetCustomerDealerNums](t.sysno) as CustomerDealerNums                               
                                from DsDealer t 
                                left join dsdealerlevel a on t.levelsysno=a.sysno
                                left join syuser b on t.usersysno=b.sysno
                                left join DsPrePayment c on t.Sysno = c.dealersysno
                                where t.sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBDsDealer>();
        }

        /// <summary>
        /// 根据订单号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2017-05-19 罗勤尧 创建
        /// </remarks>
        public override DsDealerLiJiaEdit GetCBDsDealerByOrderId(int sysNo)
        {
            const string sql = @"select b.sysno as MemberId ,t.DealerName as MemberName,t .Contact as Contact,t.PhoneNumber as PhoneNumber ,b.MobilePhoneNumber as CellPhone ,'' as Fax,t.EmailAddress as Email,q2.AreaName as Province ,q1.AreaName as City,q.AreaName as District,t.StreetAddress as AddressLine,'' as BankAccountdbo,'' as QQ,t.DealerName as UserName  
                                ,'' as BrandName from soOrder a  left join DsDealer t on a.DealerSysNo = t.SysNo
 left join syuser b on t.usersysno=b.sysno
 left join BsArea q on t.AreaSysNo=q.SysNo
								   left join BsArea q1 on q.ParentSysNo=q1.SysNo
								   left join BsArea q2 on q1.ParentSysNo=q2.SysNo  
 where a.SysNo=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsDealerLiJiaEdit>();
        }

        /// <summary>
        /// 根据订单号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2017-05-19 罗勤尧 创建
        /// </remarks>
        public override DsDealer GetDsDealerByOrderSysNo(int sysNo)
        {
            const string sql = @"select t.* from DsDealer t  left join soOrder a on a.DealerSysNo = t.SysNo
 where a.SysNo=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsDealer>();
        }
        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override void GetDsDealerList(ref Pager<CBDsDealer> pager, ParaDsDealerFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                //2015-12-19 王耀发 创建
                //var DealerSysNoList = string.Empty;
                //if (null != filter.DealerSysNoList)
                //{
                //    DealerSysNoList = string.Join(",", filter.DealerSysNoList);
                //}    
                //dbo.func_getaereapath(t.areasysno)
                const string sqlSelect = @"t.*,a.levelname,b.account,b.username,'' as AreaAllName,
                                           c.totalprestoreamount,c.availableamount,c.frozenamount";

                const string sqlFrom = @"DsDealer t left join dsdealerlevel a on t.levelsysno=a.sysno
                                         left join syuser b on t.usersysno=b.sysno 
                                         left join DsPrePayment c on t.sysno=c.dealersysno";
                string sqlWhere = "1=1";

                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        sqlWhere += " and t.SysNo = @DealerSysNo";
                    }
                    else if (filter.DealerCreatedBy>=0)
                    {
                        sqlWhere += " and t.CreatedBy = @DealerCreatedBy";
                    }               
                }

                sqlWhere += @" and (@DealerName is null or charindex(@DealerName,t.DealerName)>0)                
                                          and (@LevelSysNo is null or t.LevelSysNo= @LevelSysNo)
                                          and (@status is null or t.status= @status)
                                          and (@UserSysNo is null or t.UserSysNo=@UserSysNo)
                                          and (@ProductSysNo is null or not exists(select 1 from DsSpecialPrice s where t.sysno=s.dealersysno and s.productsysno=@ProductSysNo))              
                                          ";

                #region sqlcount

                string sqlCount = @" select count(1) from DsDealer t where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("DealerSysNo", filter.DealerSysNo)
                                         .Parameter("DealerCreatedBy", filter.DealerCreatedBy)
                                         .Parameter("DealerName", filter.DealerName)
                                         .Parameter("LevelSysNo", filter.LevelSysNo)
                                         .Parameter("status", filter.Status)
                                         .Parameter("UserSysNo", filter.UserSysNo)
                                         .Parameter("ProductSysNo", filter.ProductSysNo)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBDsDealer>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("DealerSysNo", filter.DealerSysNo)
                                    .Parameter("DealerCreatedBy", filter.DealerCreatedBy)
                                    .Parameter("DealerName", filter.DealerName)
                                    .Parameter("LevelSysNo", filter.LevelSysNo)
                                    .Parameter("status", filter.Status)
                                    .Parameter("UserSysNo", filter.UserSysNo)
                                    .Parameter("ProductSysNo", filter.ProductSysNo)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>   
        public override IList<CBDsDealer> GetDsDealerList(ParaDsDealerFilter filter)
        {
            //dbo.func_getaereapath(t.areasysno)
            const string sqlSelect = @"t.*,a.levelname,b.account,b.username,'' as AreaAllName,
                                       c.totalprestoreamount,c.availableamount,c.frozenamount";

            const string sqlFrom = @"DsDealer t left join dsdealerlevel a on t.levelsysno=a.sysno
                                     left join syuser b on t.usersysno=b.sysno 
                                     left join DsPrePayment c on t.sysno=c.dealersysno";

            const string sqlWhere = @"
                (@DealerName is null or t.DealerName=@DealerName) 
                and (@LevelSysNo is null or t.LevelSysNo= @LevelSysNo)
                and (@status is null or t.status= @status)
                and (@UserSysNo is null or t.UserSysNo=@UserSysNo)     
                and (@AppID is null or t.AppID=@AppID)    
                and (@AppSecret is null or t.AppSecret=@AppSecret)    
                and (@WeiXinNum is null or t.WeiXinNum=@WeiXinNum)    
                and (@DomainName is null or t.DomainName=@DomainName)    
               ";

            return Context.Select<CBDsDealer>(sqlSelect)
                                .From(sqlFrom)
                                .Where(sqlWhere)
                                .Parameter("DealerName", filter.DealerName)
                                .Parameter("LevelSysNo", filter.LevelSysNo)
                                .Parameter("status", filter.Status)
                                .Parameter("UserSysNo", filter.UserSysNo)
                                .Parameter("AppID", filter.AppID)
                                .Parameter("AppSecret", filter.AppSecret)
                                .Parameter("WeiXinNum", filter.WeiXinNum)
                                .Parameter("DomainName", filter.DomainName)
                                .QueryMany();
        }
        /// <summary>
        /// 判断是否有分销商重复
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public override IList<CBDsDealer> GetDsDealerListByDealerName(string DealerName, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.AppID=@1";
            return Context.Sql(sql, sysNo, DealerName)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询分销商信息
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的分销商系统编号</param>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建 
        /// </remarks>   
        public override IList<CBDsDealer> GetDsDealerList(int userSysNo, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.usersysno=@1";
            return Context.Sql(sql, sysNo, userSysNo)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 判断是否有AppID重复
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public override IList<CBDsDealer> GetDsDealerListByAppID(string AppID, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.AppID=@1";
            return Context.Sql(sql, sysNo, AppID)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 判断是否有AppSecret重复
        /// </summary>
        /// <param name="AppSecret"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public override IList<CBDsDealer> GetDsDealerListByAppSecret(string AppSecret, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.AppSecret=@1";
            return Context.Sql(sql, sysNo, AppSecret)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 判断是否有WeiXinNum重复
        /// </summary>
        /// <param name="WeiXinNum"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        public override IList<CBDsDealer> GetDsDealerListByWeiXinNum(string WeiXinNum, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.WeiXinNum=@1";
            return Context.Sql(sql, sysNo, WeiXinNum)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 判断是否有DomainName重复
        /// </summary>
        /// <param name="DomainName"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        public override IList<CBDsDealer> GetDsDealerListByDomainName(string DomainName, int sysNo)
        {
            const string sql = @"select t.* from dsdealer t where t.sysno<>@0 and t.DomainName=@1";
            return Context.Sql(sql, sysNo, DomainName)
                          .QueryMany<CBDsDealer>();
        }
        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public override IList<CBDsDealer> GetDsDealerList()
        {
            const string sql = @"select t.* from dsdealer t order by t.DealerName";
            return Context.Sql(sql)
                          .QueryMany<CBDsDealer>();
        }

        /// <summary>
        /// 根据系统用户编号获取分销商信息
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-09 余勇 创建
        /// </remarks>
        public override CBDsDealer GetDsDealerByUserNo(int userSysNo)
        {
            const string sql = @"select t.*,a.levelname,b.account,b.username,dbo.func_getaereapath(t.areasysno) AreaAllName
                                from DsDealer t 
                                left join dsdealerlevel a on t.levelsysno=a.sysno
                                left join syuser b on t.usersysno=b.sysno 
                                where t.UserSysNo=@0";
            return Context.Sql(sql, userSysNo)
                          .QuerySingle<CBDsDealer>();

        }

        /// <summary>
        /// 根据分销商用户编号获取分销商信息
        /// </summary>
        /// <param name="dsUserSysNo">分销商用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public override DsDealer GetDsDealerByDsUser(int dsUserSysNo)
        {
            const string sql = @"select dd.* from DsUser du 
                                    inner join  DsDealer dd on du.dealersysno=dd.sysno
                                    where du.SysNo=@0";
            return Context.Sql(sql, dsUserSysNo)
                          .QuerySingle<CBDsDealer>();

        }

        /// <summary>
        /// 根据仓库编号获取分销商信息
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public override DsDealer GetDsDealerByWarehousSysNo(int warehousSysNo)
        {
            const string sql = @"select c.* FROM  DsDealerWharehouse b 
                                 left join DsDealer c on b.dealersysno=c.sysno
                                    where b.WarehouseSysNo=@0";
            return Context.Sql(sql, warehousSysNo)
                          .QuerySingle<DsDealer>();

        }
        /// <summary>
        /// 根据名称获取分销商
        /// </summary>
        /// <param name="DealerName"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-06-09 王耀发 创建
        /// </remarks> 
        public override DsDealer GetDsDealerByName(string DealerName)
        {
            const string sql = @"select * from  DsDealer where DealerName=@0";
            return Context.Sql(sql, DealerName)
                          .QuerySingle<DsDealer>();

        }
        #endregion

        /// <summary>
        /// 获取所有分销商信息
        /// </summary>
        /// <returns>分销商数据列表</returns>
        /// 2015-09-19 王耀发 创建
        public override IList<DsDealer> GetAllDealerList()
        {
            return Context.Sql("select * from DsDealer order by DealerName Collate Chinese_PRC_Stroke_ci_as").QueryMany<DsDealer>();
        }

        /// <summary>
        /// 获取用户有可管理的所有分销商
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>分销商集合</returns>
        /// <remarks>
        /// 2015-09-19 王耀发 创建
        /// </remarks>
        public override IList<DsDealer> GetUserDealerList(int userSysNo)
        {
            const string sql =
                @"select * from DsDealer where CreatedBy=@UserSysNo 
                            order by DealerName collate chinese_prc_cs_as_ks_ws ";
            var list = Context.Sql(sql)
                .Parameter("UserSysNo", userSysNo)
                .QueryMany<DsDealer>();
            return list;
        }

        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2015-12-31 王耀发 创建 
        /// </remarks>      
        public override IList<DsDealer> GetDsDealersList()
        {
            const string sql = @"select t.* from dsdealer t where t.[Status] = 1 order by t.DealerName";
            return Context.Sql(sql)
                          .QueryMany<DsDealer>();
        }
        /// <summary>
        /// 获得当前用户有权限看到的分销商
        /// 2016-1-4 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override IList<DsDealer> GetDealersListByCurUser(ParaDsDealerFilter filter)
        {
            var sql = @"Select d.* From DsDealer d Where {0}";
            var where = "d.[Status] = 1 ";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and d.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and d.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            where += " order by d.SysNo desc ";
            sql = string.Format(sql, where);

            return Context.Sql(sql)
                          .QueryMany<DsDealer>();
        }

        /// <summary>
        /// 获得创建用户对应的分销商
        /// 2016-1-29 王耀发 创建
        /// </summary>
        /// <param name="DealerCreatedBy"></param>
        /// <param name="Type">当前登录账号类型 F：为分销商</param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        public override IList<DsDealer> GetDealersListByCreatedBy(int DealerCreatedBy, string Type, int TypeSysNo)
        {
            var sql = @"Select d.* From DsDealer d Where {0}";
            var where = "1 = 1 ";
            if (Type == "F")
            {
                where += " and d.SysNo = " + TypeSysNo.ToString();
            }
            else
            {
                where += " and d.CreatedBy = " + DealerCreatedBy.ToString() + " and d.[Status] = 1 ";
            }
            where += " order by d.SysNo asc ";
            sql = string.Format(sql, where);
            return Context.Sql(sql)
                          .QueryMany<DsDealer>();
        }

        /// <summary>
        /// 获得当前经销商树形图
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        /// 2016-1-20 王耀发 创建
        public override IList<CBDsDealer> GetDealerTreeList(string Type, int TypeSysNo)
        {
            string Sql = string.Format("pro_GetDealerTreeList");
            var result = Context.StoredProcedure(Sql)
            .Parameter("Type", Type)
            .Parameter("TypeSysNo", TypeSysNo)
            .QueryMany<CBDsDealer>();
            return result;
        }

        /// <summary>
        /// 获得代理商列表
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        /// 2016-1-29 王耀发 创建
        public override IList<CBDsDealer> GetDaiLiShangList(string Type, int TypeSysNo)
        {
            string Sql = string.Format("pro_GetDaiLiShangList");
            var result = Context.StoredProcedure(Sql)
            .Parameter("Type", Type)
            .Parameter("TypeSysNo", TypeSysNo)
            .QueryMany<CBDsDealer>();
            return result;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏 创建</returns>
        public override IList<DsApplyStore> List(int State, Pager<DsApplyStore> pager)
        {
            using (var content = Context.UseSharedConnection(true))
            {
                string sql = "";
                if (State != -1)
                    sql += string.Format("and State={0}", State);

                pager.Rows =
                    content.Select<DsApplyStore>("dsa.*")
                           .From("DsApplyStore dsa")
                           .Where("dsa.Isdelete=0 " + sql + "")
                           .OrderBy("dsa.ApplicantTime desc")
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .QueryMany();

                pager.TotalRows =
                    content.Sql(@"select count(dsa.sysno) from DsApplyStore dsa where dsa.Isdelete=0 " + sql + "")
                           .QuerySingle<int>();
            }

            return pager.Rows;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public override int Update(string sysNoItems)
        {
            return Context.Sql("update DsApplyStore set State=1 where sysNo in(" + sysNoItems + ")")
                //.Parameter("SysNo", sysNoItems)
                   .Execute();
        }

        /// <summary>
        /// 更新微信AppID、AppSecret
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2016-05-10 王耀发 创建
        /// </remarks>
        public override int UpdateAppIDandSecret(int SysNo, string AppID, string AppSecret)
        {
            return Context.Sql("update DsDealer set AppID=@0,AppSecret=@1 where SysNo=@2", AppID, AppSecret, SysNo)
                           .Execute();
        }
        /// <summary>
        /// 更新微信利嘉会员id
        /// </summary>
        /// <param name="LiJiaSysNo">利嘉会员系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2017-05-19 罗勤尧 创建
        /// </remarks>
        public override int UpdateLiJiaSysNo(int LiJiaSysNo ,int SysNo)
        {
            return Context.Sql("update DsDealer set LiJiaSysNo=@0 where SysNo=@1", LiJiaSysNo, SysNo)
                           .Execute();
        }

        /// <summary>
        /// 根据分销商获取店铺
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public override IList<WhWarehouse> WhWarehouseList(int DealerSysNo)
        {
            var sql = @"select w.* from WhWarehouse w left join  DsDealerWharehouse d on w.SysNo=d.WarehouseSysNo where w.status=1 and d.DealerSysNo=" + DealerSysNo+" order by w.SysNo asc ";
            return Context.Sql(sql)
                          .QueryMany<WhWarehouse>();
        }

    }

}
