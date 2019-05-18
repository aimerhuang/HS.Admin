using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Util;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品品牌维护
    /// </summary>
    /// <remarks>2013-06-25 唐永勤 创建</remarks>    
    public class PdBrandDaoImpl : IPdBrandDao
    {
        /// <summary>
        /// 添加品牌
        /// </summary>
        /// <param name="model">品牌实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public override int Create(PdBrand model)
        {
            int id = Context.Insert<PdBrand>("pdbrand", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("Sysno");
            return id;
        }
        /// <summary>
        /// 添加b2b品牌
        /// </summary>
        /// <param name="model">品牌实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public override int CreateToB2B(PdBrand model)
        {
            int id = ContextB2B.Insert<PdBrand>("pdbrand", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("Sysno");
            return id;
        }
        /// <summary>
        /// 判断重复数据--品牌
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public override bool IsExists(string name, int brandSysNo)
        {
            bool result = false;
            PdBrand entity = Context.Select<PdBrand>("*")
                .From("PdBrand")
                .Where("name= @name")
                .Parameter("name", name)
                .QuerySingle();
            if (entity !=null && entity.SysNo > 0)
            {
                result = true;
                if (brandSysNo > 0) 
                {
                    if (brandSysNo == entity.SysNo)
                    {
                        result = false;
                    }
                }
            }
            return result;
 
        }

        /// <summary>
        /// 获取指定编号的品牌信息
        /// </summary>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public override PdBrand GetEntity(int brandSysNo)
        {
            PdBrand entity = Context.Select<PdBrand>("*")
                .From("PdBrand")
                .Where("sysno = @sysno")
                .Parameter("sysno", brandSysNo)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 获取指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override PdBrand GetEntityByName(string Name)
        {
            PdBrand entity = Context.Select<PdBrand>("*")
                .From("PdBrand")
                .Where("Name = @Name")
                .Parameter("Name", Name)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 获取b2b指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public override PdBrand GetB2BEntityByName(string Name)
        {
            PdBrand entity = ContextB2B.Select<PdBrand>("*")
                .From("PdBrand")
                .Where("Name = @Name")
                .Parameter("Name", Name)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 根据品牌ID更新品牌信息
        /// </summary>
        /// <param name="model">品牌实体信息,不包括状态，排序</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public override bool Update(PdBrand model)
        {
            int effect = Context.Update<PdBrand>("pdbrand", model)
                .AutoMap(x => x.SysNo,x=>x.DisplayOrder,x=>x.Status)
                .Where("sysno", model.SysNo)
                .Execute();
            return effect > 0;
        }

        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">品牌编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.品牌状态 status, List<int> listSysNo)
        {
            string Sql = string.Format("update pdbrand set Status = {0} where SysNo in ({1})", (int)status, listSysNo.Join(","));
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="pager">品牌查询条件</param>
        /// <returns>品牌列表</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public override Pager<PdBrand> GetPdBrandList(Pager<PdBrand> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@name is null or name like @name1) ";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<PdBrand>("pb.*")
                              .From("PdBrand pb")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              //.Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.Name)
                              .Parameter("name1", "%" + pager.PageFilter.Name + "%")
                              .OrderBy(" DisplayOrder desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("PdBrand")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              //.Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.Name)
                              .Parameter("name1", "%" + pager.PageFilter.Name + "%")
                              .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 品牌是否正在被使用
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public override bool BrandIsBeingUsed(int sysNo)
        {
            return Context.Sql("SELECT COUNT(1) FROM PdProduct WHERE BrandSysNo=" + sysNo).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 删除没被使用的品牌
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public override int DeleteBrandNotBeingUsed(int sysNo)
        {
            if (Context.Sql("SELECT COUNT(1) FROM PdProduct WHERE BrandSysNo=" + sysNo).QuerySingle<int>() > 0)
            {
                return 0;
            }
            return Context.Delete("PdBrand").Where("sysNo", sysNo).Execute();
        }
    }
}
