using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Util;
using System.Transactions;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品属性组维护接口实现
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>
    public class PdAttributeGroupDaoImpl : IPdAttributeGroupDao
    {
        /// <summary>
        /// 添加属性组
        /// </summary>
        /// <param name="model">属性组实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public override int Create(PdAttributeGroup model)
        {
            int sysno = 0;
            sysno = Context.Insert<PdAttributeGroup>("PdAttributeGroup", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }

        /// <summary>
        /// 判断重复数据--属性组
        /// </summary>
        /// <param name="name">属性组名称</param>
        /// <param name="backName">后台显示名称</param>
        /// <param name="sysNo">属性组编号</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public override bool IsExists(string name, string backName, int sysNo)
        {
            bool result = false;
            PdAttributeGroup entity = Context.Select<PdAttributeGroup>("*")
                .From("PdAttributeGroup")
                .Where("name= @name and BackEndName=@backName and sysno !=@sysno")
                .Parameter("name", name)
                .Parameter("backName", backName)
                .Parameter("sysno", sysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取指定编号的属性组信息
        /// </summary>
        /// <param name="sysNo">属性组编号</param>
        /// <returns>属性组实体信息</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public override PdAttributeGroup GetEntity(int sysNo)
        {
            PdAttributeGroup entity = Context.Select<PdAttributeGroup>("*")
                .From("PdAttributeGroup")
                .Where("sysno = @sysno")
                .Parameter("sysno", sysNo)
                .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 根据属性组ID更新属性组信息
        /// </summary>
        /// <param name="model">属性组实体信息,不包括状态，排序</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public override bool Update(PdAttributeGroup model)
        {
            int effect = Context.Update<PdAttributeGroup>("PdAttributeGroup", model)
                .AutoMap(x => x.SysNo, x => x.DisplayOrder, x => x.Status)
                .Where("sysno", model.SysNo)
                .Execute();

            return effect > 0;
        }

        /// <summary>
        /// 更新属性组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性组编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public override int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性分组状态 status, List<int> listSysNo)
        {
            string Sql = string.Format("update PdAttributeGroup set Status = {0} where SysNo in ({1})", (int)status, listSysNo.Join(","));
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取属性组列表
        /// </summary>
        /// <param name="pager">属性组查询条件</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public override Pager<PdAttributeGroup> GetPdAttributeGroupList(Pager<PdAttributeGroup> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@name is null or name like @name1 or BackEndName like @name1) ";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<PdAttributeGroup>("p.*")
                              .From("PdAttributeGroup p")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                    // .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.Name)
                              .Parameter("name1", "%" + pager.PageFilter.Name + "%")
                    // .Parameter("name", "%" + pager.PageFilter.Name + "%")
                              .OrderBy(" DisplayOrder,sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("PdAttributeGroup")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                    // .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("name", pager.PageFilter.Name)
                              .Parameter("name1", "%" + pager.PageFilter.Name + "%")
                    //  .Parameter("name", "%" + pager.PageFilter.Name + "%")
                              .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 获取属性组所有属性
        /// </summary>
        /// <param name="attributeGroupSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public override IList<PdAttribute> GetAttributes(int attributeGroupSysNo)
        {
            #region sql条件
            string sql = @" select b.* from (select * from pdattributegroupassociation where attributegroupsysno = @attributegroupsysno) a left join pdattribute b on a.attributesysno=b.sysno  order by a.displayorder asc";
            #endregion
            IList<PdAttribute> list = Context.Sql(sql)
                .Parameter("attributegroupsysno", attributeGroupSysNo)
                .QueryMany<PdAttribute>();
            return list;
        }

        /// <summary>
        /// 设置属性组属性
        /// </summary>
        /// <param name="sysNo">属性组编号</param>
        /// <param name="listAttribute">属性列表</param>
        /// <returns>成功返回true，失败返回false</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        /// <remarks>2013-08-01 邵斌 重构 修改使用共享连接</remarks>
        public override bool SetAttributes(int sysNo, IList<PdAttributeGroupAssociation> listAttribute)
        {

            if (sysNo > 0)
            {
                using (var currentContext = Context.UseSharedConnection(true))
                {
                    //先删除
                    currentContext.Delete("PdAttributeGroupAssociation")
                                  .Where("AttributeGroupSysNo", sysNo)
                                  .Execute();

                    //再添加
                    foreach (PdAttributeGroupAssociation entity in listAttribute)
                    {
                        entity.AttributeGroupSysNo = sysNo;
                        currentContext.Insert<PdAttributeGroupAssociation>("PdAttributeGroupAssociation", entity)
                                        .AutoMap(x => x.SysNo)
                                        .Execute();
                    }


                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 读取商品分类对应的属性组
        /// </summary>
        /// <param name="pdCategorySysNo">商品分类编号</param>
        /// <returns>商品分类下所有的属性组列表</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public override IList<PdAttributeGroup> GetPdCategoryAttributeGroupList(int pdCategorySysNo)
        {
            #region 测试SQL
            /*
            select 
                pg.* 
            from 
                PdAttributeGroup pg
                inner join PdCatAttributeGroupAso paga on pg.sysno = paga.attributegroupsysno
                inner join  pdcategory pc on paga.productcategorysysno = pc.sysno
            where 
                pc.sysno = 1
             */

            #endregion

            //管理查询，通过分类ID->分类属性组管理表->属性组
            return Context.Select<PdAttributeGroup>("pg.*")
                    .From(@"
                        PdAttributeGroup pg
                        inner join PdCatAttributeGroupAso paga on pg.sysno = paga.attributegroupsysno
                        inner join  pdcategory pc on paga.productcategorysysno = pc.sysno
                    ")
                     .Where("pc.sysno = @sysno")
                     .Parameter("sysno", pdCategorySysNo, Base.DataTypes.Int32)
                     .QueryMany();
        }

        /// <summary>
        /// 获取已选的属性组列表
        /// </summary>
        /// <param name="listSysNo">属性组编号列表</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-07-12 唐永勤 创建</remarks>
        public override IList<PdAttributeGroup> GetSelectedAttributeGroups(IList<int> listSysNo)
        {
            IList<PdAttributeGroup> list = new List<PdAttributeGroup>();
            if (listSysNo.Count > 0)
            {
                string sql = string.Format("select * from  PdAttributeGroup where SysNo in ({0})", listSysNo.Join(","));
                list = Context.Sql(sql).QueryMany<PdAttributeGroup>();
            }
            return list;
        }
    }
}
