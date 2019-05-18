using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 系统配置功能Impl
    /// </summary>
    /// <remarks>2014-01-20 周唐炬 创建</remarks>
    public class SyConfigDaoImpl : ISyConfigDao
    {
        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override SyConfig GetModel(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM SyConfig WHERE Sysno =@SysNo").Parameter("SysNo", sysNo).QuerySingle<SyConfig>();
        }

        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="category">分类系统</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-02-11 周唐炬 创建</remarks>
        public override SyConfig GetModel(string key, SystemStatus.系统配置类型 category)
        {
            const string sql = "SELECT * FROM SyConfig WHERE  [KEY]=@0 AND [CATEGORYID]=@1";
            var paras = new object[]
                {
                    key,
                    category.GetHashCode()
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<SyConfig>();
        }

        /// <summary>
        /// 创建系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>系统配置功能实体系统编号</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override int Create(SyConfig model)
        {
            return Context.Insert<SyConfig>("SyConfig", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override int Update(SyConfig model)
        {
            return Context.Update("SyConfig", model).AutoMap(x => x.SysNo).Where(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 删除系统配置项
        /// </summary>
        /// <param name="sysNo">系统配置系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override int Remove(int sysNo)
        {
            return Context.Delete("SyConfig").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 更根条件获取系统配置功能列表
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>系统配置功能列表</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override Pager<SyConfig> GetList(Model.Parameter.ParaSyConfigFilter filter)
        {
            const string sql = @"(SELECT * FROM SyConfig WHERE (@0 IS NULL OR CategoryId = @0)) tb";

            var dataList = Context.Select<SyConfig>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                    filter.CategoryId
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<SyConfig>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }

        /// <summary>
        /// 验证系统配置功能是否已经存在
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="categoryId">分类</param>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public override int SyConfigVerify(string key, int categoryId, int? sysNo)
        {
            const string sql = "SELECT COUNT(1) FROM SyConfig WHERE  [KEY]=@0 AND [CATEGORYID]=@1 AND (@2 IS NULL OR SYSNO<>@2) ";
            var paras = new object[]
                {
                    key,
                    categoryId,
                    sysNo
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<int>();
        }
    }
}
