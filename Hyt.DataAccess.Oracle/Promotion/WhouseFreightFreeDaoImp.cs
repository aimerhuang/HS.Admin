using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 仓库免运费
    /// </summary>
    /// <remarks>2016-04-20 王耀发 创建</remarks>
    public class WhouseFreightFreeDaoImp : IWhouseFreightFreeDao
    {
        /// <summary>
        /// 免运费信息
        /// </summary>
        /// <param name="filter">免运费信息</param>
        /// <returns>返回免运费信息</returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        public override Pager<CBWhouseFreightFree> GetWhouseFreightFreeList(ParaWhouseFreightFreeFilter filter)
        {
            string sql = @"(select w.SysNo as WhSysNo, w.ErpCode,w.WarehouseName,w.BackWarehouseName,wf.* 
                            from WhWarehouse w left join WhouseFreightFree wf on wf.WarehouseSysNo = w.SysNo
                            where
                            w.Status = 1 and     
                            (@BackWarehouseName is null or w.BackWarehouseName like @BackWarehouseName)) tb ";

            var dataList = Context.Select<CBWhouseFreightFree>("tb.*").From(sql)
                .Parameter("BackWarehouseName", "%" + filter.BackWarehouseName + "%");
            var dataCount = Context.Select<int>("count(1)").From(sql)
                .Parameter("BackWarehouseName", "%" + filter.BackWarehouseName + "%");

            var pager = new Pager<CBWhouseFreightFree>
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
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public override int Insert(WhouseFreightFree entity)
        {
            entity.SysNo = Context.Insert("WhouseFreightFree", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public override int Update(WhouseFreightFree entity)
        {

            return Context.Update("WhouseFreightFree", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public override WhouseFreightFree GetEntity(int SysNo)
        {

            return Context.Sql("select a.* from WhouseFreightFree a where a.SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<WhouseFreightFree>();
        }
    }
}
