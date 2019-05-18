using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.AppContent;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.AppContent
{
    /// <summary>
    /// 前台管理
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public class AppContentDaoImpl : IAppContentDao
    {
        #region 商品浏览历史

        /// <summary>
        /// 删除用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public override int DeleteHistory(int customerSysNo)
        {
            var rowsAffected = Context.Delete("CrBrowseHistory").Where("customersysno", customerSysNo).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 客户商品浏览历史记录查询
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="customerLevel">客户等级</param>
        /// <returns>客户商品浏览历史记录查询列表</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public override IList<SimplProduct> GetProBroHistory(int customerSysNo, int customerLevel)
        {
            const string sql = @"SELECT a.productsysno AS SysNo
	                                    ,c.productname AS ProductName
	                                    ,d.price AS Price
	                                    ,isnull(e.price, - 1) AS LevelPrice
	                                    ,c.productimage AS Thumbnail
	                                    ,cast(NULL AS NVARCHAR2(10)) AS Icon
	                                    ,cast(NULL AS NVARCHAR2(10)) AS Specification
                                    FROM CrBrowseHistory a
                                    INNER JOIN CrCustomer b ON b.sysno = a.customersysno
	                                    AND b.sysno = @0
                                    INNER JOIN PdProduct c ON c.sysno = a.productsysno
	                                    AND c.STATUS = @1
                                    INNER JOIN pdprice d ON d.productsysno = a.productsysno
	                                    AND d.STATUS = @2
	                                    AND d.pricesource = @3
                                    left JOIN pdprice e ON e.productsysno = a.productsysno
	                                    AND e.STATUS = @1
	                                    AND e.sourcesysno = @4
                                        and e.pricesource = @5";
            var paras = new object[]
                {
                    customerSysNo,
                    (int)ProductStatus.产品上线状态.有效,
                    (int)ProductStatus.产品价格状态.有效,
                    (int)ProductStatus.产品价格来源.基础价格,
                    customerLevel,
                    (int)ProductStatus.产品价格来源.会员等级价
                };
            var list = Context.Sql(sql).Parameters(paras).QueryMany<SimplProduct>();
            return list;
        }

        /// <summary>
        /// 商品浏览历史记录查询
        /// </summary>
        /// <param name="para">CBCrBrowseHistory</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Pager-CBCrBrowseHistory分页对象</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        /// <remarks>2013-11-07 郑荣华 商品名称改为模糊查询</remarks>
        public override Dictionary<int, IList<CBCrBrowseHistory>> QueryProBroHistory(CBCrBrowseHistory para, int currPageIndex = 1,
                                                                    int pageSize = 10)
        {
            string sqlSelect = @"h.*,c.account as CustomerAccount,p.easname as ProductName,p.ErpCode",
                   sqlFrom = @"CrBrowseHistory h
                             left join CrCustomer c on h.customersysno=c.sysno
                             left join PdProduct p on h.productsysno=p.sysno",
                   sqlCondition = @"(@account is null or c.account=@account)
                                    and (@productname is null or charindex(p.easname,@productname)>0)";
            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<CBCrBrowseHistory>(sqlSelect)
                         .From(sqlFrom).
                          AndWhere(sqlCondition)
                         .Parameter("account", para.CustomerAccount)
                         .Parameter("productname", para.ProductName)
                         .Paging(currPageIndex, pageSize)//index从1开始
                         .OrderBy("h.sysno desc")
                         .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom).
                                    AndWhere(sqlCondition)
                                   .Parameter("account", para.CustomerAccount)
                                   .Parameter("productname", para.ProductName)
                                   .QuerySingle();
                return new Dictionary<int, IList<CBCrBrowseHistory>> { { count, lstResult } };
            }

        }

        /// <summary>
        /// 删除浏览历史记录
        /// </summary>
        /// <param name="lstDelSysNos">要删除的历史记录编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public override void DeleteBrowseHistory(List<int> lstDelSysNos)
        {
            Context.Sql("delete CrBrowseHistory where sysno in(@lstDelSysnos)")
                          .Parameter("lstDelSysnos", lstDelSysNos)
                          .Execute();
        }

        #endregion

        #region app版本管理

        /// <summary>
        /// app版本管理
        /// </summary>
        /// <param name="para">CBApVersion</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dictionary-CBApVersion</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public override Dictionary<int, IList<CBApVersion>> QueryAppVersion(CBApVersion para, int currPageIndex = 1,
                                                                            int pageSize = 10)
        {
            string sqlSelect = @"a.*",
                   sqlFrom = @"ApVersion a",
                   sqlCondition = @"(:versionnumber is null or a.versionnumber=:versionnumber)";
            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<CBApVersion>(sqlSelect)
                                       .From(sqlFrom).
                                        AndWhere(sqlCondition)
                                       .Parameter("versionnumber", para.VersionNumber)
                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("a.sysno desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom).
                                    AndWhere(sqlCondition)
                                   .Parameter("versionnumber", para.VersionNumber)
                                   .QuerySingle();
                return new Dictionary<int, IList<CBApVersion>> { { count, lstResult } };
            }
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="lstDelSysNos">要删除的版本编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public override void DeleteVersion(List<int> lstDelSysNos)
        {
            Context.Sql("delete ApVersion where sysno in(@lstDelSysnos)")
                          .Parameter("lstDelSysnos", lstDelSysNos)
                          .Execute();
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public override int CreateVersion(CBApVersion model, int operatorSysNo)
        {
            model.CreatedBy = operatorSysNo;
            model.CreatedDate = DateTime.Now;
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert("ApVersion", model).AutoMap(m => m.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>the rows affected</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public override int UpdateVersion(CBApVersion model, int operatorSysNo)
        {
            model.LastUpdateBy = operatorSysNo;
            model.LastUpdateDate = DateTime.Now;

            return Context.Update("ApVersion", model)
                .Column(m => m.AppCode)
                .Column(m => m.VersionNumber)
                .Column(m => m.VersionLink)
                .Column(m => m.UpgradeInfo)
                .Column(m => m.Description)
                .Column(m => m.LastUpdateBy)
                .Column(m => m.LastUpdateDate)
                .Where(m => m.SysNo).Execute();
        }

        /// <summary>
        /// 根据App代码分组获取最新版本
        /// </summary>
        /// <returns>最新版本列表</returns>
        /// <remarks>
        /// 2013-10-24 郑荣华 创建
        /// </remarks>
        public override IList<CBApVersion> GetLastAppVersion()
        {
            const string sql =
               @"select t.* from apversion t inner join 
                (select max(versionnumber) versionnumber,appcode from apversion group by appcode)a
                on t.appcode=a.appcode and t.versionnumber=a.versionnumber";
            return Context.Sql(sql).QueryMany<CBApVersion>();
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>版本</returns>
        /// <remarks>
        /// 2013-11-27 郑荣华 创建
        /// </remarks>
        public override CBApVersion GetAppVersion(int sysNo)
        {
            const string sql = @"select t.* from apversion t where sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBApVersion>();
        }
        #endregion

        #region APP推送服务

        /// <summary>
        /// 创建推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public override bool CreateApPushService(CBApPushService model)
        {
            bool result = false;
            using (var _context = Context.UseSharedConnection(true))
            {
                //如果是给给人推送消息，将检查个人账号是否设置正确
                if (model.ServiceType == (int)AppStatus.App推送服务类型.个人消息 && !string.IsNullOrEmpty(model.CustomerAccount))
                {
                    //获取个人信息
                    var user = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomer(model.CustomerAccount);
                    if (user == null)
                    {
                        return false;
                    }

                    model.CustomerSysNo = user.SysNo;
                }
                else if (model.ServiceType == (int)AppStatus.App推送服务类型.App浏览器 && !string.IsNullOrEmpty(model.Url))
                {
                    model.Parameter = model.Url;
                }
                if (model.BeginTime == DateTime.MinValue)
                {
                    model.BeginTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                if (model.EndTime == DateTime.MinValue)
                {
                    model.EndTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                model.SysNo = _context.Insert("ApPushService", model as ApPushService).AutoMap(m => m.SysNo).ExecuteReturnLastId<int>("SysNo");
                result = model.SysNo > 0;
            }

            return result;
        }

        /// <summary>
        /// 获取单个推送服务对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回推送对象模型</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public override CBApPushService GetApPushService(int sysNo)
        {
            return Context.Select<CBApPushService>(" APS.*,CR.ACCOUNT AS CustomerAccount, CR.NAME AS CustomerName")
                       .From("ApPushService APS LEFT JOIN CRCUSTOMER CR ON CR.SYSNO = APS.CUSTOMERSYSNO")
                       .Where("APS.sysno=@sysno")
                       .Parameter("sysno", sysNo)
                       .QuerySingle();
        }

        /// <summary>
        /// 更新推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:更新成功 false:更新失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public override bool UpdateApPushService(CBApPushService model)
        {
            //如果是给给人推送消息，将检查个人账号是否设置正确
            if (model.ServiceType == (int)AppStatus.App推送服务类型.个人消息 && !string.IsNullOrEmpty(model.CustomerAccount))
            {
                //获取个人信息
                var user = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomer(model.CustomerAccount);
                if (user == null)
                {
                    return false;
                }

                model.CustomerSysNo = user.SysNo;
            }
            else if (model.ServiceType == (int)AppStatus.App推送服务类型.App浏览器 && !string.IsNullOrEmpty(model.Url))
            {
                model.Parameter = model.Url;
            }

            return Context.Update("ApPushService", (ApPushService)model)
                .AutoMap(m => m.SysNo)
                .Where("SysNo", model.SysNo)
                .Execute() > 0;
        }

        /// <summary>
        /// 更新推送消息状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">更新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public override bool UpdateApPushServiceStatus(int sysNo, AppStatus.App推送服务状态 status, int updateBy)
        {
            return
                Context.Sql("update ApPushService set status=@0,LastUpdateBy=@1,LastUpdateDate=@2 where sysno=@3")
                        .Parameters((int)status, updateBy, DateTime.Now, sysNo)
                        .Execute() > 0;
        }

        /// <summary>
        /// 读取推送消息列表
        /// </summary>
        /// <param name="para">分页分页参数，并返回结果到对象</param>
        /// <returns></returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public override void GetApPushService(ref Pager<CBApPushService> para)
        {
            #region 测试SQL

            /*
             --查询推送数据
            select  
             aps.*,cr.account as CustomerAccount,cr.nickname as CustomerName,su.account as LastUpdateUser 
            from 
            ApPushService aps
            left join SyUser su on aps.lastupdateby = su.sysno
            left join Crcustomer cr on aps.customersysno = cr.sysno
            where 
            ( 0=0 or aps.status = 20)                                      --状态过滤 
            and (0=0 or aps.AppType=1)                                     --App类型过滤
            and (0=0 or aps.ServiceType=1)                                 --服务类型过滤
            and ((null is null or charindex(lower(aps.Title),null)>0)          --推送标题过滤
            or (null is null or charindex(lower(aps.Content),null)>0)          --推送内容过滤
            or (null is null or cr.Account = null))                        --客户账号过滤
             * */

            using (var _context = Context.UseSharedConnection(true))
            {
                para.Rows = _context.Select<CBApPushService>("aps.*,cr.account as CustomerAccount,cr.nickname as CustomerName,su.account as LastUpdateUser")
                                    .From("ApPushService aps left join SyUser su on aps.lastupdateby = su.sysno left join Crcustomer cr on aps.customersysno = cr.sysno")
                                    .Where(@"( @status=0 or aps.status = @status)
                                    and (@AppType=90 or aps.AppType=@AppType)
                                    and (@ServiceType=0 or aps.ServiceType=@ServiceType)
                                    and ((@Title is null or charindex(lower(aps.Title),@Title)>0)
                                    or (@Content is null or charindex(lower(aps.Content),@Content)>0)
                                    or (@Account is null or cr.Account = @Account))")
                                    .Parameter("status", para.PageFilter.Status)
                                    .Parameter("AppType", para.PageFilter.AppType)
                                    .Parameter("ServiceType", para.PageFilter.ServiceType)
                                    .Parameter("Title", para.PageFilter.Title)
                                    .Parameter("Content", para.PageFilter.Content)
                                    .Parameter("Account", para.PageFilter.CustomerAccount)
                                    .OrderBy("aps.LastUpdateDate desc")
                                    .Paging(para.CurrentPage, para.PageSize)
                                    .QueryMany();

                para.TotalRows = _context.Sql(@"select count(aps.sysno)
                                     from 
                                     ApPushService aps left join SyUser su on aps.lastupdateby = su.sysno left join Crcustomer cr on aps.customersysno = cr.sysno
                                    where
                                    ( @status=0 or aps.status = @status)
                                    and (@AppType=90 or aps.AppType=@AppType)
                                    and (@ServiceType=0 or aps.ServiceType=@ServiceType)
                                    and ((@Title is null or charindex(lower(aps.Title),@Title)>0)
                                    or (@Content is null or charindex(lower(aps.Content),@Content)>0)
                                    or (@Account is null or cr.Account = @Account))")
                                    .Parameter("status", para.PageFilter.Status)
                                    .Parameter("AppType", para.PageFilter.AppType)
                                    .Parameter("ServiceType", para.PageFilter.ServiceType)
                                    .Parameter("Title", para.PageFilter.Title)
                                    .Parameter("Content", para.PageFilter.Content)
                                    .Parameter("Account", para.PageFilter.CustomerAccount)
                             .QuerySingle<int>();
            }

            #endregion

        }

        #endregion

    }
}
