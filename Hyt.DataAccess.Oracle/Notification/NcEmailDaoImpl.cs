using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Notification;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Notification
{
    /// <summary>
    /// 邮件实现类
    /// </summary>
    /// <remarks>2013-10-8 陶辉 创建</remarks>
    public class NcEmailDaoImpl:INcEmailDao
    {
        /// <summary> 
        /// 创建邮件
        /// </summary>
        /// <param name="model">邮件实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override int Create(NcEmail model)
        {
            return Context.Insert("NcEmail", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新邮件
        /// </summary>
        /// <param name="model">邮件实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override int Update(NcEmail model)
        {
            return Context.Update("NcEmail", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取邮件信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>邮件实体</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override NcEmail GetNcEmail(int sysNo)
        {
            const string sql = "select * from NcEmail where sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<NcEmail>();
        }

        /// <summary>
        /// 根据邮件发送状态获取邮件列表
        /// </summary>
        /// <param name="status">邮件发送状态</param>
        /// <returns>邮件列表</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override List<NcEmail> GetNcEmailListByStatus(Hyt.Model.WorkflowStatus.NotificationStatus.邮件发送状态 status)
        {
            const string sql = "select * from NcEmail where status=@0 ";
            return Context.Sql(sql, status)
                          .QueryMany<NcEmail>();
        }
    }
}
