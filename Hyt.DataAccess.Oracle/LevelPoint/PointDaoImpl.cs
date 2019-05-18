using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.LevelPoint;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.LevelPoint
{
    /// <summary>
    /// 积分业务
    /// </summary>
    /// <remarks>2013-07-10 黄波 创建</remarks>
    public class PointDaoImpl : IPointDao
    {
        #region 等级
        /// <summary>
        /// 查看会员等级日志最新一条记录
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public override CrLevelPointLog GetLevelPointList(int customerSysNo)
        {
            const string strSql = @"select * from (select * from CrLevelPointLog where Increased>0 and customersysno=@customersysno order by CreatedDate desc) where rownum=1";
            var result = Context.Sql(strSql)
                                .Parameter("customersysno", customerSysNo)
                                .QuerySingle<CrLevelPointLog>();
            return result;
        }

        /// <summary>
        /// 获取最后一次一条经验积分日志(增加积分)
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public override CrExperiencePointLog GetExperiencePointLog(int customerSysNo)
        {
            const string strSql = @"select * from (select * from CrExperiencePointLog where Increased>0 and customersysno=@customersysno order by changedate desc) where rownum=1";
            var result = Context.Sql(strSql)
                                .Parameter("customersysno", customerSysNo)
                                .QuerySingle<CrExperiencePointLog>();
            return result;
        }

        /// <summary>
        /// 获取规定日期范围发内的经验积分日志列表(增加积分)
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>经验积分日志列表</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public override IList<CrExperiencePointLog> GetExperiencePointLog(int customerSysNo, DateTime beginTime, DateTime endTime)
        {
            //const string strSql = @"select * from CrExperiencePointLog where customersysno=:customersysno and (changedate>=:beginTime and changedate<=:endTime)";
            #region sql条件
            string sql = @"customersysno=@customersysno and Increased>0 and (changedate>=@beginTime and changedate<=@endTime)";
            #endregion
            var list = Context.Select<CrExperiencePointLog>("cep.*")
                                .From("CrExperiencePointLog cep")
                                .Where(sql)
                                .Parameter("customersysno", customerSysNo)
                                .Parameter("beginTime", beginTime)
                                .Parameter("endTime", endTime)
                                .QueryMany();
            return list;
        }

        /// <summary>
        /// 汇总客等级积分日志 增加积分、减少积分
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public override CBCrLevelPointLog GetLevelPointLog(int customerSysNo, DateTime startTime, DateTime endTime)
        {
            const string strSql = @"select sum(Increased) as IncreasedSum,sum(Decreased) as DecreasedSum from CrLevelPointLog where customersysno=@customersysno and (CreatedDate>= @startTime and CreatedDate<=@endTime)";
            var result = Context.Sql(strSql)
                                .Parameter("customersysno", customerSysNo)
                                .Parameter("startTime", startTime)
                                .Parameter("endTime", endTime)
                                .QuerySingle<CBCrLevelPointLog>();
            return result;
        }
        #endregion

        #region 获取实体

        /// <summary>
        /// 查看会员等级
        /// </summary>
        /// <param name="sysNo">等级编号</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public override CBCrLevelLog GetLevelLogModel(int sysNo)
        {
            const string strSql = @"
select 
cll.*,(select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.oldlevelsysno) as oldLevelName,(select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.newlevelsysno) as newLevelName,su.UserName
from CrLevelLog cll left join SyUser su on cll.CreatedBy=su.sysno where cll.sysno=@sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CBCrLevelLog>();
            return result;
        }

        /// <summary>
        /// 查看会员等级积分日志
        /// </summary>
        /// <param name="sysNo">等级积分日志编号</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public override CBCrLevelPointLog GetLevelPointLogModel(int sysNo)
        {
            const string strSql =
                @"select cp.*,su.UserName,(select account from crcustomer where sysno=cp.customersysno) as CustomerAccount from CrLevelPointLog cp left join SyUser su on cp.lastupdateby=su.sysno where cp.sysno=@sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CBCrLevelPointLog>();
            return result;
        }

        /// <summary>
        /// 查看经验积分日志
        /// </summary>
        /// <param name="sysNo">经验积分日志编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public override CBCrExperiencePointLog GetCrExperiencePointLogModel(int sysNo)
        {
            const string strSql = @"select cp.*,su.UserName,(select account from crcustomer where sysno=cp.customersysno) as CustomerAccount from CrExperiencePointLog cp left join SyUser su on cp.CreatedBy=su.sysno where cp.sysno=@sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CBCrExperiencePointLog>();
            return result;
        }
        /// <summary>
        /// 查看用户积分日志
        /// </summary>
        /// <param name="sysNo">积分日志编号</param>
        /// <returns>积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public override CrAvailablePointLog GetCrAvailablePointLogModel(int sysNo)
        {
            const string strSql = @"select cp.*,su.UserName,(select account from crcustomer where sysno=cp.customersysno) as CustomerAccount from CrAvailablePointLog cp left join SyUser su on cp.CreatedBy=su.sysno where cp.sysno=@sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CrAvailablePointLog>();
            return result;
        }
        /// <summary>
        /// 获取惠源币日志模型
        /// </summary>
        /// <param name="sysNo">惠源币日志系统编号</param>
        /// <returns>惠源币日志模型</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        public override CBCrExperienceCoinLog GetCbCrExperienceCoinLog(int sysNo)
        {
            const string strSql = @"select cr.*,(select name from crcustomer where sysno=cr.customersysno) as CustomerName,
(select account from crcustomer where sysno=cr.customersysno) as CustomerAccount,
(select username from syuser where sysno=cr.CreatedBy) as CreatedByName
from CrExperienceCoinLog cr where cr.sysNo=@sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CBCrExperienceCoinLog>();
            return result;
        }

        #endregion

        #region 获取调整日志

        /// <summary>
        /// 获取惠源币日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>惠源币日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 杨晗 修改</remarks>
        public override void GetExperienceCoinLog(ref Pager<CBCrExperienceCoinLog> pager)
        {
            #region sql条件

            string sqlWhere =
                @"(@customersysno=0 or customersysno=@customersysno) and (@changetype=0 or changetype=@changetype)";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows =
                    context.Select<CBCrExperienceCoinLog>(
                        "cr.*,(select name from crcustomer where sysno=cr.customersysno) as CustomerName,(select account from crcustomer where sysno=cr.customersysno) as CustomerAccount,(select username from syuser where sysno=cr.CreatedBy) as CreatedByName")
                           .From("CrExperienceCoinLog cr")
                           .Where(sqlWhere)
                           .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                           .Parameter("changetype", pager.PageFilter.ChangeType)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("changedate desc")
                           .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrExperienceCoinLog cr")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("changetype", pager.PageFilter.ChangeType)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取经验积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-10 苟治国 修改</remarks>
        public override void GetExperiencePointLog(ref Model.Pager<CBCrExperiencePointLog> pager)
        {
            #region sql条件
            string sqlWhere = @"(@customersysno=-1 or cp.customersysno=@customersysno) and (@pointType=-1 or cp.pointType=@pointType)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCrExperiencePointLog>("cp.*,su.UserName")
                                    .From("CrExperiencePointLog cp left join SyUser su on cp.CreatedBy=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("pointType", pager.PageFilter.PointType)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("changedate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrExperiencePointLog cp left join SyUser su on cp.CreatedBy=su.sysno")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("pointType", pager.PageFilter.PointType)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取用户积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-10 苟治国 修改</remarks>
        public override void GetCrAvailablePointLog(ref Model.Pager<CrAvailablePointLog> pager)
        {
            #region sql条件
            string sqlWhere = @"(@customersysno=-1 or cp.customersysno=@customersysno) and (@pointType=-1 or cp.pointType=@pointType)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrAvailablePointLog>("cp.*,su.UserName")
                                    .From("CrAvailablePointLog cp left join SyUser su on cp.CreatedBy=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("pointType", pager.PageFilter.PointType)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("changedate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrAvailablePointLog cp left join SyUser su on cp.CreatedBy=su.sysno")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("pointType", pager.PageFilter.PointType)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取等级积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public override void GetLevelPointLog(ref Model.Pager<CBCrLevelPointLog> pager)
        {
            #region 原sql

            //select cp.*,su.UserName from CrLevelPointLog cp left join SyUser su on cp.customersysno=su.sysno 

            #endregion

            #region sql条件

            string sqlWhere = @"(@customersysno=-1 or cp.customersysno=@customersysno) and (@changetype=-1 or cp.changetype=@changetype)";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCrLevelPointLog>("cp.*,su.UserName")
                                    .From("CrLevelPointLog cp left join SyUser su on cp.LastUpdateBy=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("ChangeType", pager.PageFilter.ChangeType)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("cp.SysNo desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrLevelPointLog cp left join SyUser su on cp.LastUpdateBy=su.sysno")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("ChangeType", pager.PageFilter.ChangeType)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取等级日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public override void GetLevelLog(ref Model.Pager<CBCrLevelLog> pager)
        {
            #region 原Sql

            /*
            select 
            cll.*,
            (select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.oldlevelsysno) as oldLevelName,
            (select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.newlevelsysno) as newLevelName,
            su.UserName
            from CrLevelLog cll left join SyUser su on cll.CreatedBy=su.sysno
             * */

            #endregion

            #region sql条件
            string sqlWhere = @"(@customersysno is null or cll.customersysno=@customersysno) and (@changetype=-1 or cll.changetype=@changetype)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCrLevelLog>("cll.*,(select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.oldlevelsysno) as oldLevelName,(select LevelName from CrCustomerLevel ccl where ccl.sysno=cll.newlevelsysno) as newLevelName,su.UserName")
                           .From("CrLevelLog cll left join SyUser su on cll.CreatedBy=su.sysno")
                           .Where(sqlWhere)
                           .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                           .Parameter("ChangeType", pager.PageFilter.ChangeType)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("cll.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From("CrLevelLog cll left join SyUser su on cll.customersysno=su.sysno")
                           .Where(sqlWhere)
                           .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                           .Parameter("ChangeType", pager.PageFilter.ChangeType)
                           .QuerySingle();
            }
        }

        #endregion

        #region 更新用户积分相关

        /// <summary>
        /// 更新用户会员币并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="amount">调整惠源币金额数(正数:增加 负数:减少)</param>
        /// <param name="model">惠源币日志实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public override void AdjustExperienceCoin(int customerSysNo, decimal amount, CrExperienceCoinLog model)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Insert<Model.CrExperienceCoinLog>("CrExperienceCoinLog", model)
                       .AutoMap(o => o.SysNo)
                       .Execute();

                context.Sql("update CrCustomer set ExperienceCoin=ExperienceCoin+@ExperienceCoin where sysno=@sysno")
                       .Parameter("ExperienceCoin", amount)
                       .Parameter("sysno", customerSysNo)
                       .Execute();
            }
        }

        /// <summary>
        /// 更新用户经验积分并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">调整经验积分数(正数:增加 负数:减少)</param>
        /// <param name="experiencePointLogModel">经验积分日志实体</param>
        /// <param name="availablePointLogModel">可用积分日志实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public override void AdjustExperiencePoint(int customerSysNo, int point, CrExperiencePointLog experiencePointLogModel, CrAvailablePointLog availablePointLogModel)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Insert<Model.CrAvailablePointLog>("CrAvailablePointLog", availablePointLogModel)
                    .AutoMap(o => o.SysNo)
                    .Execute();

                context.Insert<Model.CrExperiencePointLog>("CrExperiencePointLog", experiencePointLogModel)
                       .AutoMap(o => o.SysNo)
                       .Execute();

                context.Sql("update CrCustomer set ExperiencePoint=ExperiencePoint+@ExperiencePoint,AvailablePoint=AvailablePoint+@AvailablePoint where sysno=@sysno")
                       .Parameter("ExperiencePoint", point)
                       .Parameter("AvailablePoint", point)
                       .Parameter("sysno", customerSysNo)
                       .Execute();
            }
        }

        /// <summary>
        /// 更新用户可用积分并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">积分数</param>
        /// <param name="model">可用积分日志实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-30 黄波 创建</remarks>
        public override void UpdateAvailablePoint(int customerSysNo, int point, CrAvailablePointLog model)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Insert<Model.CrAvailablePointLog>("CrAvailablePointLog", model)
                        .AutoMap(o => o.SysNo)
                        .Execute();
                context.Sql("update CrCustomer set AvailablePoint=AvailablePoint+@AvailablePoint where sysno=@sysNo")
                    .Parameter("AvailablePoint", point)
                    .Parameter("sysNo", customerSysNo)
                    .Execute();
            }
        }

        /// <summary>
        /// 调整用户等级积分并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">调整等级积分数(正数:增加 负数:减少)</param>
        /// <param name="model">等级积分日志实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public override void AdjustLevelPoint(int customerSysNo, int point, CrLevelPointLog model)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Insert<Model.CrLevelPointLog>("CrLevelPointLog", model)
                       .AutoMap(o => o.SysNo)
                       .Execute();

                context.Sql("update CrCustomer set LevelPoint=LevelPoint+@LevelPoint where sysno=@sysno")
                       .Parameter("LevelPoint", point)
                       .Parameter("sysno", customerSysNo)
                       .Execute();
            }
        }

        #endregion

        #region 更新用户等级
        /// <summary>
        /// 更新用户等级
        /// 用于注册用户
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-30 黄波 创建</remarks>
        public override void UpdateCustomerLevel(int customerSysNo)
        {
            var _sql = @"update crcustomer set levelsysno=
                                    (select top (1) crcl.sysno from crcustomer crc 
                                                                    left join crcustomerlevel crcl 
                                                                        on (lowerlimit<=crc.levelpoint and upperlimit>=crc.levelpoint
                                    ) 
                         where crc.sysno=@sysno) where sysno=@sysno";
            Context.Sql(_sql)
                               .Parameter("sysno", customerSysNo)
                               .Execute();
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">等级积分日志变更类型</param>
        /// <param name="description">等级变更说明</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public override void UpdateCustomerLevel(int customerSysNo, int userSysNo, CustomerStatus.等级积分日志变更类型 changeType,
                                                 string description)
        {
            var customer = Context.Select<CrCustomer>("c.*")
                                  .From("CrCustomer c")
                                  .Where("c.sysno=@sysno")
                                  .Parameter("sysno", customerSysNo)
                                  .QuerySingle();
            if (customer != null)
            {
                //根据积分总数取等级编号
                var levelSysNo = Context.Select<int>("sysno")
                                        .From("crcustomerlevel")
                                        .Where("lowerlimit<=@point and upperlimit>=@point")
                                        .Parameter("point", customer.LevelPoint)
                                        .OrderBy("upperlimit desc")
                                        .QuerySingle();
                //根据等级编号判断是否需要更新等级
                if (levelSysNo!=0 && levelSysNo != customer.LevelSysNo)
                {
                    //写入等级变更日志
                    var levelLogModel = new CrLevelLog
                        {
                            ChangeDate = DateTime.Now,
                            CreatedBy = userSysNo,
                            CreatedDate = DateTime.Now,
                            CustomerSysNo = customerSysNo,
                            NewLevelSysNo = levelSysNo,
                            OldLevelSysNo = customer.LevelSysNo
                        };
                    switch (changeType)
                    {
                        case CustomerStatus.等级积分日志变更类型.交易变更:
                            levelLogModel.ChangeType = (int)CustomerStatus.等级日志变更类型.交易变更;
                            levelLogModel.ChangeDescription = levelLogModel.NewLevelSysNo > levelLogModel.OldLevelSysNo ? "经验升级" : "交易取消降级";
                            break;
                        default:
                            levelLogModel.ChangeType = (int)CustomerStatus.等级日志变更类型.客服调整;
                            levelLogModel.ChangeDescription = description;
                            break;
                    }
                    using (var context = Context.UseSharedConnection(true))
                    {
                        context.Sql("update crcustomer set levelsysno=@levelsysno where sysno=@sysno")
                               .Parameter("levelsysno", levelSysNo)
                               .Parameter("sysno", customerSysNo)
                               .Execute();
                        context.Insert<CrLevelLog>("CrLevelLog", levelLogModel)
                               .AutoMap(o => o.SysNo)
                               .Execute();
                    }
                }
            }
        }

        #endregion

        public override bool HasExperiencePoint(int pointType, string transactionSysNo)
        {
            return Context.Sql(@"Select Count(0) From CrExperiencePointLog Where PointType=@0 And TransactionSysNo=@1", pointType, transactionSysNo).QuerySingle<int>() > 0;
        }

        public override bool HasLevelPoint(int pointType, string transactionSysNo)
        {
            return Context.Sql(@"Select Count(0) From CrLevelPointLog Where ChangeType=@0 And TransactionSysNo=@1", pointType, transactionSysNo).QuerySingle<int>() > 0;
        }
    }
}
