using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;

namespace Hyt.DataAccess.Oracle.Front
{
    public class FeSoftwareDaoImpl : IFeSoftwareDao
    {
        /// <summary>
        /// 添加软件
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>返回新建记录的sysNo</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override int Create(FeSoftware model)
        {
            int sysno = 0;
            sysno = Context.Insert<FeSoftware>("FeSoftware", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }


        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override bool IsExists(FeSoftware model)
        {
            bool result = false;
            FeSoftware entity = Context.Select<FeSoftware>("*")
                .From("FeSoftware")
                .Where("Title= @Title and Sysno != @Sysno")
                .Parameter("Name", model.Title)
                .Parameter("Sysno", model.SysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取指定编号的软件信息
        /// </summary>
        /// <param name="sysNo">软件编号</param>
        /// <returns>软件实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override FeSoftware GetEntity(int sysNo)
        {
            FeSoftware entity = Context.Select<FeSoftware>("*")
              .From("FeSoftware")
              .Where("sysno = @sysno")
              .Parameter("sysno", sysNo)
              .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 根据软件编号更新软件信息
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override bool Update(FeSoftware model)
        {
            int effect = Context.Update<FeSoftware>("FeSoftware", model)
               .AutoMap(x => x.SysNo, x => x.Status)
               .Where("sysno", model.SysNo)
               .Execute();
            return effect > 0;
        }

        /// <summary>
        /// 更新软件状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">软件编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态 status, int sysNo)
        {
            string Sql = string.Format("update FeSoftware set Status = {0} where SysNo = {1}", (int)status, sysNo);
            return Context.Sql(Sql).Execute();
        }

        /// <summary>
        /// 获取软件下载列表
        /// </summary>
        /// <param name="pager">软件下载列表查询条件</param>
        /// <returns>软件下载列表</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        public override Pager<FeSoftware> GetList(Pager<FeSoftware> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@Title is null or Title like @Title1) and (@SoftCategorySysNo=0 or SoftCategorySysNo =@SoftCategorySysNo)";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<FeSoftware>("fs.*")
                              .From("FeSoftware fs")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("Title", pager.PageFilter.Title)
                              .Parameter("Title1", "%" + pager.PageFilter.Title + "%")
                              .Parameter("SoftCategorySysNo", pager.PageFilter.SoftCategorySysNo)
                              .OrderBy("DisplayOrder desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("FeSoftware")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("Title", pager.PageFilter.Title)
                              .Parameter("Title1", "%" + pager.PageFilter.Title + "%")
                              .Parameter("SoftCategorySysNo", pager.PageFilter.SoftCategorySysNo)
                              .QuerySingle();
            }
            return pager;
        }
    }
}
