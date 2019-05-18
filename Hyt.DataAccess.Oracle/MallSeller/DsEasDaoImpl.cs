using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// EAS数据库操作类
    /// </summary>
    /// <remarks>2013-10-10 黄志勇 创建</remarks>
    public class DsEasDaoImpl : IDsEasDao
    {
        /// <summary>
        /// 查询分销商EAS关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分销商EAS关联分页数据</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override Pager<CBDsEasAssociation> Query(ParaDsEasFilter filter)
        {
            const string sql =
                @"(SELECT a.SYSNO,a.DEALERMALLSYSNO,a.SELLERNICK,a.CODE,a.STATUS,a.CREATEDBY,a.CREATEDDATE,a.LASTUPDATEBY,a.LASTUPDATEDATE,b.SHOPNAME,b.SHOPACCOUNT,c.MALLNAME
                        FROM DSEASASSOCIATION a
                        LEFT JOIN DSDEALERMALL b
                        ON a.DEALERMALLSYSNO = b.SYSNO
                        LEFT JOIN DSMALLTYPE c
                        ON c.SYSNO = b.MALLTYPESYSNO
                                where 
                                (@0 is null or b.MallTypeSysNo=@0) and 
                                (@1 is null or a.Status=@1) and 
                                (@2 is null or charindex(b.ShopName,@2)>0) and 
                                (@3 is null or charindex(b.ShopAccount,@3)>0) and                                
                                (@4 is null or charindex(a.SellerNick,@4)>0) and  
                                (@5 is null or charindex(a.Code,@5)>0) and  
                                (@6 is null or a.CreatedDate>=@6) and --日期(起)
                                (@7 is null or a.CreatedDate<@7) --日期(止)                                
                                ) tb";

            var paras = new object[]
                {
                    filter.MallTypeSysNo,
                    filter.Status,
                    filter.ShopName,
                    filter.ShopAccount,
                    filter.SellerNick,
                    filter.Code,
                    filter.BeginDate,
                    filter.EndDate
                };

            var dataList = Context.Select<CBDsEasAssociation>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsEasAssociation>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.CreatedDate desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override int Insert(DsEasAssociation entity)
        {
            var sysNo = Context.Insert("DsEasAssociation", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override void Update(DsEasAssociation entity)
        {

            Context.Update("DsEasAssociation", entity)
                   .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-11 黄志勇 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsEasAssociation")
                   .Where("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override DsEasAssociation GetEntity(int sysNo)
        {
            const string sql = @"select * from DsEasAssociation
where SysNo = @SysNo";
            return Context.Sql(sql)
                          .Parameter("SysNo", sysNo)
                          .QuerySingle<DsEasAssociation>();
        }

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override DsEasAssociation Get(int dealerMallSysNo)
        {
            const string sql = @"select * from DsEasAssociation
where DealerMallSysNo = @DealerMallSysNo";
            return Context.Sql(sql)
                          .Parameter("DealerMallSysNo", dealerMallSysNo)
                          .QuerySingle<DsEasAssociation>();
        }

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="sellerNick">昵称</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-18 黄志勇 创建</remarks>
        public override DsEasAssociation Get(int dealerMallSysNo, string sellerNick)
        {
            const string sql = @"select * from DsEasAssociation
where DealerMallSysNo = @DealerMallSysNo and SellerNick = @SellerNick";
            return Context.Sql(sql)
                          .Parameter("DealerMallSysNo", dealerMallSysNo)
                          .Parameter("SellerNick", sellerNick)
                          .QuerySingle<DsEasAssociation>();
        }

        /// <summary>
        /// 获取全部商城类型
        /// </summary>
        /// <returns>分销商城类型列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override List<DsMallType> GetAllMallType()
        {
            return Context.Sql("select * from DsMallType").QueryMany<DsMallType>();
        }

        /// <summary>
        /// 获取全部商城
        /// </summary>
        /// <returns>分销商城列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override List<DsDealerMall> GetAllMall()
        {
            return Context.Sql("select * from DsDealerMall order by ShopName desc").QueryMany<DsDealerMall>();
        }

        /// <summary>
        /// 获取新增商城
        /// </summary>
        /// <returns>分销商城列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public override List<DsDealerMall> GetNewMall()
        {
            return Context.Sql(@"select a.* from DsDealerMall a
left join DsEasAssociation b
on a.SysNo = b.DealerMallSysNo
where b.DealerMallSysNo is null and a.status =1").QueryMany<DsDealerMall>();
        }

        /// <summary>
        /// 获取全部分销商EAS关联商城系统编号列表
        /// </summary>
        /// <returns>商城系统编号列表</returns>
        /// <remarks>2013-11-1 黄志勇 创建</remarks>
        public override List<int> GetAllDsEasAssociation()
        {
            return Context.Sql(@"select DealerMallSysNo from DsEasAssociation").QueryMany<int>();
        }
    }
}
