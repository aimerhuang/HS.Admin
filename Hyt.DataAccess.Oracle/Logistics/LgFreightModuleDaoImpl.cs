using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.Common;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 取运费模板数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class LgFreightModuleDaoImpl : ILgFreightModuleDao
    {
        /// <summary>
        /// 查询运费模板
        /// </summary>
        /// <param name="filter">查询运费模板实体</param>
        /// <returns>返回运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override Pager<LgFreightModule> GetLgFreightModuleList(ParaFreightModule filter)
        {
            const string sql = @"(select a.*
                    from LgFreightModule a 
                    where (@0 is null or charindex(a.ModuleName,@1)>0) and                                                                                                                                --促销名称
                    (@2 is null or charindex(a.ModuleCode,@3)>0) and
                    (@4 is null or Status = @5)
                                   ) tb";

            var dataList = Context.Select<LgFreightModule>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.ModuleName, filter.ModuleName,
                    filter.ModuleCode, filter.ModuleCode,
                    filter.Status,filter.Status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<LgFreightModule>
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
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override LgFreightModule GetEntity(int sysNo)
        {

            return Context.Sql("select a.* from LgFreightModule a where a.SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<LgFreightModule>();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductAddress">商品地址编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override LgFreightModule GetEntityByProductAddress(int ProductAddress)
        {

            return Context.Sql("select a.* from LgFreightModule a where a.ProductAddress=@ProductAddress")
                   .Parameter("ProductAddress", ProductAddress)
              .QuerySingle<LgFreightModule>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(LgFreightModule entity)
        {
            entity.SysNo = Context.Insert("LgFreightModule", entity)
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
        public override int Update(LgFreightModule entity)
        {

            return Context.Update("LgFreightModule", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }


        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="pager">运费模板查询条件</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-05-12 王耀发 创建</remarks>
        public override Pager<LgFreightModule> GetFreightModuleList(Pager<LgFreightModule> pager)
        {
            #region sql条件
            string sql = @" (@Status=-1 or Status =@Status) and (@ModuleCode is null or ModuleCode like @ModuleCode1) and (@ModuleName is null or ModuleName like @ModuleName1)";
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<LgFreightModule>("lfm.*")
                              .From("LgFreightModule lfm")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ModuleCode", pager.PageFilter.ModuleCode)
                              .Parameter("ModuleCode1", "%" + pager.PageFilter.ModuleCode + "%")
                              .Parameter("ModuleName", pager.PageFilter.ModuleName)
                              .Parameter("ModuleName1", "%" + pager.PageFilter.ModuleName + "%")
                              .OrderBy(" LastUpdateDate desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("LgFreightModule")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ModuleCode", pager.PageFilter.ModuleCode)
                              .Parameter("ModuleCode1", "%" + pager.PageFilter.ModuleCode + "%")
                              .Parameter("ModuleName", pager.PageFilter.ModuleName)
                              .Parameter("ModuleName1", "%" + pager.PageFilter.ModuleName + "%")

                              .QuerySingle();
            }
            return pager;
        }


        public const string freightMouleFiled = "[SysNo],[ModuleCode],[ModuleName],[ProductAddress],[DeliveryTime] ,[IsPost],[ValuationStyle],[Express],[EMS],[SurfaceMail],[Status],[AuditorSysNo],[AuditDate],[CreatedBy] ,[CreatedDate],[LastUpdateBy],[LastUpdateDate]";
        /// <summary>
        /// 获取仓库地址所对应的运费模板
        /// </summary>
        /// <param name="productAddress">仓库地址编号</param>
        /// <returns>运费模板</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override IList<LgFreightModule> GetFreightModuleByProductAddress(int addressSysNo)
        {
            var strSql = string.Format("SELECT {0} FROM  LgFreightModule WHERE ProductAddress={1} or  ProductAddress=0 and Status=20 ", freightMouleFiled, addressSysNo);
            return Context.Sql(strSql).QueryMany<LgFreightModule>();
        }
        /// <summary>
        /// 获取所有运费模板
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override List<LgFreightModule> GetFreightModuleList()
        {
            return Context.Sql("select a.* from LgFreightModule a where Status=20 ").QueryMany<LgFreightModule>();
        }
        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <param name="productSysNoAndNumber">商品系统编号和购买数量组合（商品系统编号_购买数量,商品系统编号_购买数量...）</param>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override IList<FareTotal> GetFareTotal(int addressSysNo, int freightModuleSysNo, string productSysNoAndNumber)
        {
            return Context.StoredProcedure("pro_GetProductListFreight")
                .Parameter("DArea", addressSysNo)
                .Parameter("FreightModuleSysNo", freightModuleSysNo)
                .Parameter("ProductSysNoNumList", productSysNoAndNumber)
                .QueryMany<FareTotal>();
        }
        #endregion
    }
}
