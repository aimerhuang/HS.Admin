using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Notification;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Notification
{
    /// <summary>
    /// 短信实现类
    /// </summary>
    /// <remarks>2013-10-8 陶辉 创建</remarks>
    public class NcSmsDaoImpl:INcSmsDao
    {

        /// <summary>
        /// 创建短信
        /// </summary>
        /// <param name="model">短信实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override int Create(NcSms model)
        {
            return Context.Insert("NcSms", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新短信
        /// </summary>
        /// <param name="model">短信实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override int Update(NcSms model)
        {
            return Context.Update("NcSms", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取短信信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override NcSms GetNcSms(int sysNo)
        {
            const string sql = "select * from NcSms where sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<NcSms>();
        }

        /// <summary>
        /// 根据短信发送状态获取短信列表
        /// </summary>
        /// <param name="status">短信发送状态</param>
        /// <returns>短信列表</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public override List<NcSms> GetNcSmsListByStatus(Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态 status)
        {
            const string sql = "select * from NcSms where status=@0 ";
            return Context.Sql(sql, status)
                          .QueryMany<NcSms>();
        }
    }
}
