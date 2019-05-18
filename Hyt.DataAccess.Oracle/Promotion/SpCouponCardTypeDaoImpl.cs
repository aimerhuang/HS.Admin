
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 优惠券卡类型
    /// </summary>
    /// <remarks>2014-01-08  朱成果 创建</remarks>
    public class SpCouponCardTypeDaoImpl : ISpCouponCardTypeDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override int Insert(SpCouponCardType entity)
        {
            entity.SysNo = Context.Insert("SpCouponCardType", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override void Update(SpCouponCardType entity)
        {

            Context.Update("SpCouponCardType", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override SpCouponCardType GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpCouponCardType where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpCouponCardType>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpCouponCardType where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页查询数据</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override Pager<SpCouponCardType> Query(ParaCouponCardType filter)
        {
            string sql = @"
              (
              select * from SpCouponCardType t where (@0 is null or t.Status=@0) and (@1 is null or charindex(t.TypeName,@1)>0)
              ) tb";
            var paras = new object[]
                {
                    filter.Status,
                    filter.TypeName
                };
            var dataList = Context.Select<SpCouponCardType>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<SpCouponCardType>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };
            return pager;
        }

        /// <summary>
        /// 获取所有启用的优惠券卡类型
        /// </summary>
        /// <returns>优惠券卡类型集合</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public override IList<SpCouponCardType> GetAllTypeName()
        {
            const string strSql = @"select SysNo,TypeName from SpCouponCardType where Status=1";
            var entity = Context.Sql(strSql)
                                .QueryMany<SpCouponCardType>();
            return entity;
        }

        /// <summary>
        /// 判断类型名称是否存在
        /// </summary>
        /// <param name="sysNo">类型编号</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>true/false</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override bool IsExistsTypeName(int sysNo, string typeName)
        {
            string sql = "select count(0) from SpCouponCardType where SysNo<>@SysNo and TypeName=@TypeName";
            return
                Context.Sql(sql)
                .Parameter("SysNo", sysNo)
                .Parameter("TypeName", typeName)
                .QuerySingle<int>() > 0;
        }
    }
}
