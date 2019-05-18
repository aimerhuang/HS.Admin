using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 组织机构Impl
    /// </summary>
    /// <remarks>2013-10-08 周唐炬 创建</remarks>
    public class OrganizationDaoImpl : IOrganizationDao
    {
        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int OrganizationCreate(BsOrganization model)
        {
            return Context.Insert<BsOrganization>("BsOrganization", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int OrganizationUpdate(BsOrganization model)
        {
            return Context.Update<BsOrganization>("BsOrganization", model).AutoMap(x => x.SysNo).Where("SysNo", model.SysNo).Execute();
        }

        /// <summary>
        /// 根据编号获取组织机构
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <returns>组织机构实体</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override BsOrganization GetEntity(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM BsOrganization WHERE SYSNO =@sysNo").Parameter("sysNo", sysNo).QuerySingle<BsOrganization>();
        }

        /// <summary>
        /// 根据父级组织机构系统编号获取所有子节点
        /// </summary>
        /// <param name="parentSysNo">父级组织机构系统编号</param>
        /// <returns>组织机构实体</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override List<BsOrganization> GetEntityByParentSysNo(int parentSysNo)
        {
            return Context.Sql(@"SELECT * FROM BsOrganization WHERE parentSysNo =@parentSysNo")
                       .Parameter("parentSysNo", parentSysNo)
                       .QueryMany<BsOrganization>();
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="sysNo">组织机构系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override int OrganizationRemove(int sysNo)
        {
            return Context.Delete("BsOrganization").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 获取所有组织机构
        /// </summary>
        /// <param></param>
        /// <returns>所有组织机构</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override List<BsOrganization> GetAll()
        {
            return Context.Sql(@"SELECT * FROM BsOrganization ORDER BY DisplayOrder").QueryMany<BsOrganization>();
        }

        /// <summary>
        /// 根据组织机构系统编号获取所有仓库关联
        /// </summary>
        /// <param name="organizationSysNo">组织机构系统编号</param>
        /// <returns>仓库关联列表</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override List<BsOrganizationWarehouse> GetItemsByOrganizationSysNo(int organizationSysNo)
        {
            return Context.Sql(@"SELECT * FROM BsOrganizationWarehouse WHERE organizationSysNo =@organizationSysNo")
                       .Parameter("organizationSysNo", organizationSysNo)
                       .QueryMany<BsOrganizationWarehouse>();
        }

        /// <summary>
        /// 根据编号获取关联仓库列表
        /// </summary>
        /// <param name="sysNo">组织机构编号</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>关联仓库列表</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override Pager<BsOrganizationWarehouse> GetItems(int sysNo, int currentPage, int pageSize)
        {
//            const string sql = @"(SELECT A.*
//                                  FROM BsOrganizationWarehouse A
//                                 WHERE A.Organizationsysno IN (SELECT B.SYSNO--, B.Name, B.Parentsysno
//                                                              FROM BsOrganization B
//                                                             START WITH B.SYSNO = @SYSNO
//                                                            connect by prior B.SYSNO = B.PARENTSYSNO)
//                                    ) tb";

//            const string sql = @"(SELECT A.*
//                                  FROM BsOrganizationWarehouse A
//                                 WHERE A.Organizationsysno IN (with MyTable as(select * from BsOrganization where SYSNO =@0
//                                                              union all select B.* from BsOrganization B inner join MyTable M  on M.SYSNO = B.ParentSysNo)
//                                                              select * from MyTable)
//                                    ) tb";
            const string sql = @"WITH BOrg
                                AS
                                (
                                select SYSNO from BsOrganization where BsOrganization.SYSNO = 1
                                union all
                                select b.SYSNO from BsOrganization as b join BOrg on b.ParentSysNo = BOrg.SYSNO
                                )
                                SELECT A.*
                                FROM BsOrganizationWarehouse A
                                WHERE A.Organizationsysno IN (select BOrg.SYSNO from BOrg)";

            var dataList = Context.Sql(sql).QueryMany<BsOrganizationWarehouse>();
            //var dataCount = Context.Select<int>("count(1)").From(sql);

            //var dataCount = Context.Sql(sql)
            //var dataCount = Context.StoredProcedure("GetBsOrganizationWarehouseCount")
            //    .Parameter("SYSNO", sysNo)
            //    .QueryMany <BsOrganizationWarehouse>();
            //var paras = new object[] { sysNo };

            //dataList.Parameters(paras);
            //dataCount.Parameters(paras);
            Hyt.Util.ListPageUtil<BsOrganizationWarehouse> pages = new Util.ListPageUtil<BsOrganizationWarehouse>(dataList, pageSize);
           
            var pager = new Pager<BsOrganizationWarehouse>()
            {
               // Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(currentPage, pageSize).QueryMany(),
                Rows = pages.GetDate(currentPage),
                TotalRows = dataList.Count()
            };
            return pager;
        }

        /// <summary>
        /// 添加组织机构关联仓库
        /// </summary>
        /// <param name="model">关联实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override void OrganizationItemCreate(BsOrganizationWarehouse model)
        {
            Context.Insert<BsOrganizationWarehouse>("BsOrganizationWarehouse", model).AutoMap(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 删除组织机构关联仓库
        /// </summary>
        /// <param name="sysNo">关联表系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override int OrganizationItemRemove(int sysNo)
        {
            return Context.Delete("BsOrganizationWarehouse").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 通过组织机构系统编号、仓库系统编号获取组织机构仓库
        /// </summary>
        /// <param name="organizationSysNo">织机构系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>组织机构仓库</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        public override BsOrganizationWarehouse GetItem(int organizationSysNo, int warehouseSysNo)
        {
            return Context.Sql(@"SELECT * FROM BsOrganizationWarehouse WHERE OrganizationSysNo =@OrganizationSysNo AND WarehouseSysNo=@WarehouseSysNo")
                .Parameter("OrganizationSysNo", organizationSysNo)
                .Parameter("WarehouseSysNo", warehouseSysNo)
                .QuerySingle<BsOrganizationWarehouse>();
        }

        /// <summary>
        /// 根据仓库获取组织机构
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>组织机构</returns>
        /// <remarks>2013-11-26 吴文强 创建</remarks>
        public override BsOrganization GetOrganization(int warehouseSysNo)
        {
            string sql = @"
                select o.* 
                from BsOrganization o
                  left join BsOrganizationWarehouse ow on o.Sysno = ow.organizationsysno 
                where ow.WarehouseSysNo = @warehouseSysNo
            ";
            return Context.Sql(sql).Parameter("warehouseSysNo", warehouseSysNo).QuerySingle<BsOrganization>();
        }


        /// <summary>
        /// 获取组织机构代码
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-5 杨浩 创建</remarks>
        public override string GetCode(int sysno)
        {
            return Context.Sql("select top 1 Code  from BsOrganization where sysno=" + sysno)
                .QuerySingle<string>();
        }
    }
}
