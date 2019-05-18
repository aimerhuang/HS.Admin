using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 优惠卡卡号
    /// </summary>
    /// <remarks>2014-01-08  朱家宏 创建</remarks>
    public class SpCouponCardDaoImpl : ISpCouponCardDao
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override int Insert(SpCouponCard entity)
        {
            entity.SysNo = Context.Insert("SpCouponCard", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override void Update(SpCouponCard entity)
        {
            Context.Update("SpCouponCard", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override SpCouponCard Get(int sysNo)
        {
            return Context.Sql("select * from SpCouponCard where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpCouponCard>();
        }

        /// <summary>
        /// 根据优惠卡号获取单条记录
        /// </summary>
        /// <param name="couponCardNo">优惠卡号码</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override SpCouponCard Get(string couponCardNo)
        {
            return Context.Sql("select * from SpCouponCard where couponCardNo=@couponCardNo")
                   .Parameter("couponCardNo", couponCardNo)
              .QuerySingle<SpCouponCard>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpCouponCard where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 分页获取优惠券卡号
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>
        /// 2014-01-08 余勇 创建
        /// </remarks>
        public override Pager<CBSpCouponCard> GetCouponCard(ParaCouponCard filter)
        {
            var sql =
                @"SpCouponCard a 
                  left join SpCouponCardType b on a.CardTypeSysNo = b.SysNo
                  where {0} ";

            #region 构造sql

            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if (filter.CardTypeSysNo != null)
            {
                //优惠券类型
                where += " and a.CardTypeSysNo=@p0p"+i;
                paras.Add(filter.CardTypeSysNo);
                i++;
            }

            if (!string.IsNullOrEmpty(filter.StartCardNo) && !string.IsNullOrEmpty(filter.EndCardNo))
            {
                //起始卡号和结束卡号都不为空
                where += " and a.CouponCardNo >= @p0p" + i;
                i++;
                where += " and a.CouponCardNo <= @p0p" + i;
                i++;
                paras.Add(filter.StartCardNo);
                paras.Add(filter.EndCardNo);
            }
            else if (!string.IsNullOrEmpty(filter.StartCardNo))
            {
                where += " and a.CouponCardNo = @p0p" + i;
                paras.Add(filter.StartCardNo);
                i++;
            }
            else if (!string.IsNullOrEmpty(filter.EndCardNo))
            {
                where += " and a.CouponCardNo = @p0p" + i;
                paras.Add(filter.EndCardNo);
                i++;
            }
            sql = string.Format(sql, where);

            #endregion

            var dataList =
                Context.Select<CBSpCouponCard>(
                    @"a.SysNo, a.CouponCardNo, a.ActivationTime, a.TerminationTime, 
                                            a.Status, b.TypeName").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<CBSpCouponCard>
            {
                CurrentPage = filter.Id.Value,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }

        /// <summary>
        /// 更新优惠券卡号状态
        /// </summary>
        /// <param name="sysNo">优惠券卡编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public override int UpdateCouponCardStatus(int sysNo, int status)
        {
            int rowsAffected = Context.Update("SpCouponCard")
            .Column("Status", status)
            .Where("SysNo", sysNo)
            .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取所有优惠券卡号
        /// </summary>
        /// <returns>优惠券卡号集合</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public override IList<SpCouponCard> GetAllSpCouponCard()
        {
            const string strSql = @"select SysNo,CardTypeSysNo,CouponCardNo,ActivationTime,TerminationTime,Status from SpCouponCard";
            var entity = Context.Sql(strSql)
                                .QueryMany<SpCouponCard>();
            return entity;
        }

        /// <summary>
        /// 新增优惠券卡号
        /// </summary>
        /// <param name="models">优惠券卡号列表</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public override void CreateSpCouponCard(List<SpCouponCard> models)
        {
            var sql = @"if not exists(select * from SpCouponCard where couponcardno = @0 ) insert into SpCouponCard(cardtypesysno,couponcardno,[status]) values(@1,@2,@3)";
            models.ForEach(model => Context.Sql(sql, model.CouponCardNo,model.CardTypeSysNo, model.CouponCardNo, model.Status).Execute());

        }

        /// <summary>
        /// 更新优惠券卡号
        /// </summary>
        /// <param name="models">优惠券卡号列表</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public override void UpdateSpCouponCard(List<SpCouponCard> models)
        {
            models.ForEach(model => Context.Update("SpCouponCard")
                                           //.Column("CardTypeSysNo", model.CardTypeSysNo)
                                           .Column("Status", model.Status)
                                           .Where("CouponCardNo", model.CouponCardNo).Execute());
        }
    }
}
