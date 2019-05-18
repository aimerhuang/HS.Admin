using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.SellBusiness
{
    /// <summary>
    /// 分销商等级数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-09-15 王耀发 创建
    /// </remarks>
    public class CrSellBusinessGradeDaoImpl : ICrSellBusinessGradeDao
    {
        
        /// <summary>
        /// 分销商等级信息
        /// </summary>
        /// <param name="filter">分销商等级信息</param>
        /// <returns>返回分销商等级信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override Pager<CrSellBusinessGrade> GetCrSellBusinessGradeList(ParaSellBusinessGradeFilter filter)
        {
            const string sql = @"(select a.* from CrSellBusinessGrade a 
                    where          
                    (@0 is null or charindex(a.Name,@1)>0) 
                                   ) tb";

            var dataList = Context.Select<CrSellBusinessGrade>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.Name,filter.Name
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CrSellBusinessGrade>
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
        /// 获得等级实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CrSellBusinessGrade GetEntity(int SysNo)
        {

            return Context.Sql("select a.* from CrSellBusinessGrade a where a.SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<CrSellBusinessGrade>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(CrSellBusinessGrade entity)
        {
            entity.SysNo = Context.Insert("CrSellBusinessGrade", entity)
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
        public override int Update(CrSellBusinessGrade entity)
        {

            return Context.Update("CrSellBusinessGrade", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("CrSellBusinessGrade")
                               .Where("SysNo", sysNo)
                               .Execute();
        }
       
        #endregion

    }
}
