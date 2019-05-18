using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    ///分销商城类型维护数据访问层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsMallTypeDaoImpl : IDsMallTypeDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Create(DsMallType model)
        {
            return Context.Insert("DsMallType", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Update(DsMallType model)
        {
            return Context.Update("DsMallType", model)
                         .AutoMap(x => x.SysNo)
                         .Where(x => x.SysNo)
                         .Execute();
        }

        /// <summary>
        /// 分销商城类型状态更新
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <param name="status">分销商城类型状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int UpdateStatus(int sysNo, DistributionStatus.商城类型状态 status)
        {
            return Context.Sql("update DsMallType set status=@0 where sysno=@1", (int)status, sysNo)
                          .Execute();
        }
        #endregion

        #region 查询

        /// <summary>
        /// 获取分销商城类型信息
        /// </summary>
        /// <param name="mallCode">分销商城类型代号</param>
        /// <returns>分销商城类型信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 可用于重复性检查 代号唯一
        /// </remarks>
        public override DsMallType GetDsMallType(string mallCode)
        {
            const string sql = @"select t.* from DsMallType t where t.mallCode=@0";

            return Context.Sql(sql, mallCode)
                          .QuerySingle<DsMallType>();
        }

        /// <summary>
        /// 获取分销商城类型信息
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <returns>分销商城类型信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override DsMallType GetDsMallType(int sysNo)
        {
            const string sql = @"select t.* from DsMallType t where t.sysno=@0";

            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsMallType>();
        }

        /// <summary>
        /// 查询分销商城类型
        /// </summary>
        /// <param name="mallName">名称</param>
        /// <param name="isPreDeposit">是否使用预存款</param>
        /// <param name="status">状态</param>
        /// <returns>分销商城类型列表</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override IList<DsMallType> GetDsMallTypeList(string mallName, int? isPreDeposit, int? status)
        {
            const string sqlWhere = @"
                (@mallName is null or t.mallName like '%'+@mallName+'%') 
                and (@isPreDeposit is null or t.isPreDeposit= @isPreDeposit)
                and (@status is null or t.status= @status)            
               ";

            return Context.Select<DsMallType>("t.*")
                          .From("DsMallType t")
                          .Where(sqlWhere)
                          .Parameter("mallName", mallName)
                          .Parameter("isPreDeposit", isPreDeposit)
                          .Parameter("status", status)
                          .QueryMany();
        }

        #endregion
    }
}
