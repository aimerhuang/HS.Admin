using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 码表维护
    /// </summary>
    /// <remarks>2013-10-14 唐永勤 创建</remarks>
    public class BsCodeDaoImpl : IBsCodeDao
    {
        /// <summary>
        /// 添加码表记录
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>返回新建记录的编号</returns>       
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public override int Create(BsCode model)
        {
            return Context.Insert<BsCode>("BsCode", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 获取指定编号的码表信息
        /// </summary>
        /// <param name="sysNo">码表编号</param>
        /// <returns>码表实体信息</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public override BsCode GetEntity(int sysNo)
        {
            BsCode entity = Context.Select<BsCode>("*")
                .From("BsCode")
                .Where("sysno = @sysno")
                .Parameter("sysno", sysNo)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="sysNo">码表系统编号</param>
        /// <returns>影响行</returns>
        /// <remarks>2013-12-04 周唐炬 创建</remarks>
        public override int Remove(int sysNo)
        {
            return Context.Delete("BsCode").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 获取父级系统编号获取码值集合
        /// </summary>
        /// <param name="parentSysNo">父级系统编号</param>
        /// <returns>取码值集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override List<BsCode> GetListByParentSysNo(int parentSysNo)
        {
            var entity = Context.Select<BsCode>("*")
                .From("BsCode")
                .Where("parentSysNo = @parentSysNo and Status = @status")
                .Parameter("parentSysNo", parentSysNo)
                .Parameter("status", (int)BasicStatus.码表状态.启用)
                .QueryMany();

            return entity;
        }

        /// <summary>
        /// 根据码表编号更新码表信息
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public override bool Update(BsCode model)
        {
            int effect = Context.Update<BsCode>("BsCode", model)
               .AutoMap(x => x.SysNo)
               .Where("sysno", model.SysNo)
               .Execute();
            return effect > 0;
        }

        /// <summary>
        /// 更新码表状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">码表编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.BasicStatus.码表状态 status, int sysNo)
        {
            return Context.Update("BsCode")
                           .Column("Status", (int)status)
                           .Where("Sysno", sysNo)
                           .Execute();
        }

        /// <summary>
        /// 判断重复数据--码表
        /// </summary>
        /// <param name="model">码表实体</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-10-15 唐永勤 创建</remarks>
        public override bool IsExists(BsCode model)
        {
            bool result = false;
            var entity = Context.Select<BsCode>("*")
                .From("BsCode")
                .Where("CodeName = @name and parentSysNo=@parentSysNo")
                .Parameter("name", model.CodeName)
                .Parameter("parentSysNo", model.ParentSysNo)
                .QuerySingle();
            if (entity != null && entity.SysNo > 0)
            {
                result = true;
                if (model.SysNo > 0)
                {
                    if (model.SysNo == entity.SysNo)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取码表列表
        /// </summary>
        /// <param name="pager">码表查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public override void GetBsCodeList(ref Pager<BsCode> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@name is null or CodeName like @name1) ";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = Context.Select<BsCode>("bc.*")
                              .From("BsCode bc ")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.CodeName)
                              .Parameter("name1", "%" + pager.PageFilter.CodeName + "%")
                              .OrderBy("sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = Context.Select<int>("count(1)")
                              .From("BsCode")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.CodeName)
                              .Parameter("name1", "%" + pager.PageFilter.CodeName + "%")
                              .QuerySingle();

            }

        }
    }
}
