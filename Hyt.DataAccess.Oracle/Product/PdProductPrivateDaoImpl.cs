using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 取定制商品数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class PdProductPrivateDaoImpl : IPdProductPrivateDao
    {
        /// <summary>
        /// 查询运费模板
        /// </summary>
        /// <param name="filter">查询运费模板实体</param>
        /// <returns>返回运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override Pager<PdProductPrivateList> GetPdProductPrivateList(ParaProductPrivateFilter filter)
        {
            const string sql = @"(select a.*,b.Name as BrandName
                    from PdProductPrivate a ,PdBrand b 
                    where a.BrandSysNo = b.SysNo and 
                    (@0 is null or charindex(b.Name,@1)>0) and
                    (@2 is null or a.Status = @3)
                                   ) tb";

            var dataList = Context.Select<PdProductPrivateList>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.Name, filter.Name,
                    filter.Status,filter.Status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<PdProductPrivateList>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 招商加盟申请列表（改）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override Pager<PdProductPrivateList> GetPdProductPrivateLists(ParaProductPrivateFilter filter)
        {
            const string sql = @"(select a.*
                    from PdProductPrivate a 
                    where (@0 is null or a.Status = @1)
                                   ) tb";

            var dataList = Context.Select<PdProductPrivateList>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.Status,filter.Status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<PdProductPrivateList>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override PdProductPrivateList GetEntity(int sysNo)
        {

            return Context.Sql("select a.*,b.Name as BrandName from PdProductPrivate a ,PdBrand b where a.BrandSysNo = b.SysNo and a.SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<PdProductPrivateList>();
        }
        /// <summary>
        /// 获取数据(改)
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override PdProductPrivateList GetEntitys(int sysNo)
        {

            return Context.Sql("select a.* from PdProductPrivate a where a.SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<PdProductPrivateList>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(PdProductPrivate entity)
        {
            entity.SysNo = Context.Insert("PdProductPrivate", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Update(PdProductPrivate entity)
        {

            return Context.Update("PdProductPrivate", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("PdProductPrivate")
                               .Where("SysNo", sysNo)
                               .Execute();
        }
        #endregion
    }
}
