using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Generated;
using Hyt.DataAccess.Atten;

namespace Hyt.DataAccess.Oracle.Atten
{
    public class ClockTypeDaoImpl : ClockTypeDao
    {
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override ASClockType Select(int sysNo)
        {
            return Context.Sql("select * from ASClockType where SysNo=@0", sysNo)
                          .QuerySingle<ASClockType>();
        }
        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override IList<ASClockType> SelectAll()
        {
            const string sql = @"select * from ASClockType where IsDelete=0 order by TypeSort desc,SysNo asc";
            return Context.Sql(sql).QueryMany<ASClockType>();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <param name="keyword">名称</param>
        /// <returns>分页</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override Pager<ASClockType> SelectAll(int currentPage, int pageSize, int? status, string keyword)
        {
            const string sql = @"(select * from ASClockType a 
                                where (@0 is null or (charindex(a.TypeName,@0)>0)) and 
                                (@1 is null or a.TypeState=@1) and IsDelete=0
                                ) tb";

            var paras = new object[]
                {
                    (keyword==""?null:keyword),//     keyword,   keyword,
                    status//,      status
                };

            var dataList = Context.Select<ASClockType>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<ASClockType>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.TypeSort desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override int Insert(ASClockType model)
        {
            return Context.Insert("ASClockType", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override bool Update(ASClockType model)
        {
            var r = Context.Update("ASClockType", model)
                           .AutoMap(o => o.SysNo)
                           .Where("SysNo", model.SysNo).Execute();
            return r > 0;
        }
        /// <summary>
        /// 通过编号查询
        /// </summary>
        /// <param name="privilegeSysNo">权限编号</param>
        /// <returns>列表</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override IList<ASClockType> SelectAllByClockTypeSysNo(int ClockTypeSysNo)
        {
            const string sql = @"select * from ASClockType where SysNo=@0 and IsDelete=1 order by LastUpdateDate desc";
            return Context.Sql(sql, ClockTypeSysNo).QueryMany<ASClockType>();
        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Sql("Update ASClockType set IsDelete = 1 where sysno = @sysno")
                                       .Parameter("sysno", sysNo)
                                       .Execute();
            return rowsAffected>0;
        }
    }
}
