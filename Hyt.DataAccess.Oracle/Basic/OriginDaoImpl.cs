using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 取国家数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class OriginDaoImpl : IOriginDao
    {
        /// <summary>
        /// 国家信息
        /// </summary>
        /// <param name="filter">国家信息</param>
        /// <returns>返回国家信息</returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        public override Pager<Origin> GetOriginList(ParaOriginFilter filter)
        {
            const string sql = @"(select a.* from Origin a 
                    where          
                    (@0 is null or charindex(a.Origin_Name,@1)>0) and 
                    (@2 is null or charindex(a.Origin_Describe,@3)>0) 
                                   ) tb";

            var dataList = Context.Select<Origin>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.Origin_Name,filter.Origin_Name,
                    filter.Origin_Describe,filter.Origin_Describe
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<Origin>
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
        /// 获得国家列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        public override List<Origin> GetOrigin()
        {
            return Context.Sql(@"select a.* from Origin a")
                .QueryMany<Origin>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public override Origin GetEntity(int SysNo)
        {

            return Context.Sql("select a.* from Origin a where a.SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<Origin>();
        }
        /// <summary>
        /// 获取指定名称的国家信息
        /// </summary>
        /// <param name="name">国家名称</param>
        /// <returns>国家实体信息</returns>
        /// <remarks>2015-12-5 王耀发 创建</remarks>
        public override Origin GetEntityByName(string Origin_Name)
        {
            Origin entity = Context.Select<Origin>("*")
                .From("Origin")
                .Where("Origin_Name = @Origin_Name")
                .Parameter("Origin_Name", Origin_Name)
                .QuerySingle();
            return entity;
        }
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(Origin entity)
        {
            entity.SysNo = Context.Insert("Origin", entity)
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
        public override int Update(Origin entity)
        {

            return Context.Update("Origin", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 删除国家
        /// </summary>
        /// <param name="sysNo">国家编号</param>
        /// <returns>删除国家记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("Origin")
                               .Where("SysNo", sysNo)
                               .Execute();
        }
        #endregion
    }
}
