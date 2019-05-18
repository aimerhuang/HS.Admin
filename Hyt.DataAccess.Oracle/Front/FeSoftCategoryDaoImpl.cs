using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;

namespace Hyt.DataAccess.Oracle.Front
{
    public class FeSoftCategoryDaoImpl : IFeSoftCategoryDao
    {
        /// <summary>
        /// 添加软件分类
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override int Create(FeSoftCategory model)
        {
            int sysno = 0;
            sysno = Context.Insert<FeSoftCategory>("FeSoftCategory", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override bool IsExists(FeSoftCategory model)
        {
            bool result = false;
            FeSoftCategory entity = Context.Select<FeSoftCategory>("*")
                .From("FeSoftCategory")
                .Where("Name= @Name and Sysno != @Sysno")
                .Parameter("Name", model.Name)
                .Parameter("Sysno", model.SysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取指定软件分类信息
        /// </summary>
        /// <param name="sysno">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override FeSoftCategory GetEntity(int sysno)
        {
            FeSoftCategory entity = Context.Select<FeSoftCategory>("*")
               .From("FeSoftCategory")
               .Where("sysno = @sysno")
               .Parameter("sysno", sysno)
               .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 根据软件分类编号更新软件分类信息
        /// </summary>
        /// <param name="model">软件分类实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override bool Update(FeSoftCategory model)
        {
            int effect = Context.Update<FeSoftCategory>("FeSoftCategory", model)
                .AutoMap(x => x.SysNo, x => x.Status)
                .Where("sysno", model.SysNo)
                .Execute();
            return effect > 0;
        }

        /// <summary>
        /// 更新软件分类状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysno">分类编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态 status, int sysno)
        {
            string Sql = string.Format("update FeSoftCategory set Status = {0} where SysNo = {1}", (int)status, sysno);
            return Context.Sql(Sql).Execute();
        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="pager">软件分类查询条件</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-26 唐永勤 创建</remarks>
        public override Pager<FeSoftCategory> GetFeSoftCategoryList(Pager<FeSoftCategory> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@Name is null or name like @Name1) ";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<FeSoftCategory>("fsc.*")
                              .From("FeSoftCategory fsc")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("Name", pager.PageFilter.Name)
                              .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                              .OrderBy(" DisplayOrder desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("FeSoftCategory")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("Name", pager.PageFilter.Name)
                              .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                              .QuerySingle();
            }
            return pager;

        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-03-04 苟治国 创建</remarks>
        public override List<FeSoftCategory> GetFeSoftCategoryList()
        {
            #region sql条件
            string sql = string.Format("select * from FeSoftCategory where Status ={0}", (int)Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态.启用);
            #endregion

            var list = Context.Sql(sql)
                       .QueryMany<FeSoftCategory>();
            return list;
        }
    }
}
