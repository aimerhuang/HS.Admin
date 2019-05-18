using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.Transactions;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品属性
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>
    public class PdAttributeDaoImpl : IPdAttributeDao
    {
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override int Create(PdAttribute model)
        {
            int sysno = 0;
            sysno = Context.Insert<PdAttribute>("PdAttribute", model)
                        .AutoMap(x=>x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");            
            return sysno;      
        }

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-12-04 唐永勤 创建</remarks>
        public override bool IsExists(PdAttribute model)
        {
            bool result = false;
            PdAttribute entity = Context.Select<PdAttribute>("*")
                .From("PdAttribute")
                .Where("AttributeName= @name and BackEndName=@backName and AttributeType=@attributeType and sysno != @sysno")
                .Parameter("name", model.AttributeName)
                .Parameter("backName", model.BackEndName)
                .Parameter("attributeType", model.AttributeType)
                .Parameter("sysno", model.SysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取指定ID的属性信息
        /// </summary>
        /// <param name="sysNo">属性编号</param>
        /// <returns>属性实体信息</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override PdAttribute GetEntity(int sysNo)
        {
            PdAttribute entity = Context.Select<PdAttribute>("*")
                .From("PdAttribute")
                .Where("sysno = @sysno")
                .Parameter("sysno", sysNo)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 根据属性编号更新属性信息
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override bool Update(PdAttribute model)
        {
            int effect = Context.Update<PdAttribute>("PdAttribute", model)
                .AutoMap(x => x.SysNo, x => x.CreatedBy,x=>x.CreatedDate, x => x.Status)
                .Where("sysno", model.SysNo)
                .Execute();
            return effect > 0 ;
           
        }

        /// <summary>
        /// 更新属性状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性状态 status, List<int> listSysNo)
        {

            string Sql = string.Format("update PdAttribute set Status = {0} where SysNo in ({1})", (int)status, listSysNo.Join(","));
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="pager">属性查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override void GetPdAttributeList(ref Pager<PdAttribute> pager)
        {
           
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@name is null or charindex(AttributeName,@name)  >0 or  charindex(BackEndName,@name)>0) ";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<PdAttribute>("p.*")
                              .From("pdattribute p ")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                             // .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.AttributeName)
                             // .Parameter("name", pager.PageFilter.AttributeName)
                            //  .Parameter("name", pager.PageFilter.AttributeName)
                              .OrderBy("sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("PdAttribute")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              //.Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.AttributeName)
                             // .Parameter("name", pager.PageFilter.AttributeName)
                             // .Parameter("name", pager.PageFilter.AttributeName)
                              .QuerySingle();

            }

        }

        /// <summary>
        /// 设置属性选项
        /// </summary>
        /// <param name="sysNo">属性编号</param>
        /// <param name="listAttributeOptions">属性选项列表</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-07-06 唐永勤 创建</remarks>
        public override bool SetAttributeOptions(int sysNo, IList<PdAttributeOption> listAttributeOptions)
        {
            bool result = false;
            if (sysNo > 0)
            {
                using (var _context = Context.UseSharedConnection(true))
                {
                    if (sysNo > 0)
                    {
                        IList<int> listSysno = new List<int>();
                        //再添加
                        foreach (PdAttributeOption entity in listAttributeOptions)
                        {
                            entity.AttributeSysNo = sysNo;
                            if (entity.SysNo > 0)
                            {
                                _context.Update<PdAttributeOption>("PdAttributeOption", entity)
                                       .AutoMap(x => x.SysNo)
                                       .Where("SysNo", entity.SysNo)
                                       .Execute();
                                listSysno.Add(entity.SysNo);
                            }
                            else
                            {
                                int newSysno = _context.Insert<PdAttributeOption>("PdAttributeOption", entity)
                                                      .AutoMap(x => x.SysNo)
                                                      .ExecuteReturnLastId<int>("SysNo");
                                listSysno.Add(newSysno);
                            }
                        }
                        ////再删除
                        if (listSysno.Count == 0)
                        {
                            listSysno.Add(0);
                        }
                        string Sql = string.Format("delete PdAttributeOption where AttributeSysNo = {0} and sysno not in ({1})", sysNo, listSysno.Join(","));
                        _context.Sql(Sql).Execute();
                    }
                }
                return true;
            }
            return result;
        }

        /// <summary>
        /// 获取属性所有选项
        /// </summary>
        /// <param name="attributeSysNo">属性编号</param>
        /// <returns>属性选项列表</returns>
        /// <remarks>2013-07-09 唐永勤 创建</remarks>
        public override IList<PdAttributeOption> GetAttributeOptions(int attributeSysNo)
        {
            IList<PdAttributeOption> list = new List<PdAttributeOption>();
            if (attributeSysNo > 0)
            {
                list = Context.Select<PdAttributeOption>("*")
                    .From("PdAttributeOption")
                    .Where("AttributeSysNo=@AttributeSysNo")
                    .Parameter("AttributeSysNo", attributeSysNo)
                    .OrderBy("DisplayOrder asc")
                    .QueryMany();
            }
            return list;
        }

        // <summary>
        /// 获取已选的属性列表
        /// </summary>
        /// <param name="listSysNo">属性编号列表</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-10 唐永勤 创建</remarks>
        public override IList<PdAttribute> GetSelectedAttributes(IList<int> listSysNo)
        {
            IList<PdAttribute> list = new List<PdAttribute>();
            if (listSysNo.Count > 0)
            {
                string sql = string.Format("select * from  PdAttribute where SysNo in ({0})", listSysNo.Join(","));
                list = Context.Sql(sql).QueryMany<PdAttribute>();
            }
            return list;
        }

        /// <summary>
        /// 通过商品系统编号获取商品的关联属性
        /// </summary>
        /// <param name="productSysNoList">商品系统编号</param>
        /// <param name="context">数据库操作上线文</param>
        /// <returns>返回属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public override IList<PdAttribute> GetProductAssociationAttribute(int[] productSysNoList, IDbContext context = null)
        {
            #region 测试 SQL 通过商品编号查找商品能用做关联的属性

            /*
            select 
                *
            from 
                pdattribute attr
                inner join (
                    select
                        ppa.attributesysno 
                    from 
                        pdproduct p
                        inner join PdProductAttribute ppa on ppa.productsysno = p.sysno
                        inner join pdattribute pa on pa.sysno = ppa.attributesysno
                    where 
                        p.sysno in(640,1962,1928)
                        and pa.IsRelationFlag = 1
                        and ppa.status = 1 
                    group by  ppa.attributesysno
                    order by ppa.attributesysno
                ) r1 on attr.sysno = r1.attributesysno
            where 
              attr.status = 1
                */

            #endregion

            context = context ?? Context;

            var result = context.Sql(@"
                                select 
                                    attr.*
                                from 
                                    pdattribute attr
                                    inner join (
                                        select
                                            ppa.attributesysno 
                                        from 
                                            pdproduct p
                                            inner join PdProductAttribute ppa on ppa.productsysno = p.sysno
                                            inner join pdattribute pa on pa.sysno = ppa.attributesysno
                                        where 
                                            p.sysno in(" + productSysNoList.Join(",") + @")
                                            and pa.IsRelationFlag = @0
                                        group by  ppa.attributesysno
                                    ) r1 on attr.sysno = r1.attributesysno
                                where 
                                  attr.status = @1 order by r1.attributesysno
                ", (int)ProductStatus.是否用做关联属性.是, (int)ProductStatus.商品属性状态.启用)
                             .QueryMany<PdAttribute>();
            return result;
        }

        /// <summary>
        /// 判断属性选项是否被商品使用
        /// </summary>
        /// <param name="sysNo">选项编号</param>
        /// <returns>被使用返回true，未被使用返回false</returns>
        /// <remarks>2013-07-30 唐永勤 创建</remarks>
        public override bool IsAttributeOptionsInProduct(int sysNo)
        {
            bool result = false;
            int numbers = Context.Select<int>("count(1)")
                                 .From("PdProductAttribute")
                                 .Where("AttributeOptionSysNo = @sysno")
                                 .Parameter("sysno", sysNo)
                                 .QuerySingle();
            if (numbers > 0)
            {
                result = true;
            }
            return result; 
        }

        /// <summary>
        /// 获取商品属性列表
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <returns>商品属性列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public override IList<PdAttribute> GetPdAttributeList(int categorySysNo)
        {
            const string sql = @"select distinct t.* from PdAttribute t 
                                inner join PdAttributeGroupAssociation a on t.sysno=a.attributesysno
                                inner join PdAttributeGroup b on a.attributegroupsysno=b.sysno
                                inner join PdCatAttributeGroupAso c on b.sysno=c.attributegroupsysno
                                where c.productcategorysysno=@0";
            return Context.Sql(sql, categorySysNo)
                          .QueryMany<PdAttribute>();
        }

        /// <summary>
        /// 获取商品属性关联
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品属性关联列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public override IList<PdProductAttribute> GetPdProductAttributeList(int pdSysNo)
        {
            const string sql = @"select t.* from PdProductAttribute t where t.ProductSysNo=@0";
            return Context.Sql(sql, pdSysNo)
                          .QueryMany<PdProductAttribute>();
        }
    }
}
