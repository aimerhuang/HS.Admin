

using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Sys;
namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 任务消息
    /// </summary>
    /// <remarks>2015-01-21  杨浩 创建</remarks>
    public class SyJobMessageDaoImpl : ISyJobMessageDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public override int Insert(SyJobMessage entity)
        {
            entity.SysNo = Context.Insert("SyJobMessage", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public override void Update(SyJobMessage entity)
        {

            Context.Update("SyJobMessage", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public override SyJobMessage GetEntity(int sysNo)
        {

            return Context.Sql("select * from SyJobMessage where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SyJobMessage>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SyJobMessage where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="messageType">类型编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public override List<SyJobMessage> GetListByMessageType(int? messageType)
        {
            if (messageType.HasValue)
            {
                return Context.Sql("select top 500 * from SyJobMessage where messagetype=@messagetype order  by sysno asc")
                       .Parameter("messagetype", messageType)
                  .QueryMany<SyJobMessage>();
            }
            else
            {
                return Context.Sql("select top 500 * from SyJobMessage order  by sysno asc")
                 .QueryMany<SyJobMessage>();
            }
        }
        #endregion

    }
}
