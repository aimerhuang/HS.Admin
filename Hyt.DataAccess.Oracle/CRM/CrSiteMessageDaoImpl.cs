using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 站内信数据层实现类
    /// </summary>
    /// <remarks>2013-08-07 杨晗 创建</remarks>
    public class CrSiteMessageDaoImpl : ICrSiteMessageDao
    {
        /// <summary>
        /// 获取站内信分页方法
        /// </summary>
        /// <param name="customersysNo">用户系统编号</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="ids">站内信状态集合</param>
        /// <param name="messageCount">抛出总数</param>
        /// <returns>站内信列表</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public override IList<CrSiteMessage> GetPage(int customersysNo, int pageIndex, int pageSize,
                                                     CustomerStatus.站内信状态[] ids,
                                                     out int messageCount)
        {
            #region sql条件

            string sql = @"customersysno = @customersysno and Status in {0}";

            #endregion

            sql = string.Format(sql, "(" + string.Join(",", ids.Select(x => (int) x).ToArray()) + ")");
            IList<CrSiteMessage> countBuilder;
            using (var _context = Context.UseSharedConnection(true))
            {
                messageCount = _context.Select<int>("count(1)")
                                       .From("CrSiteMessage cm")
                                       .Where(sql)
                                       .Parameter("customersysno", customersysNo)
                                       .QuerySingle();

                countBuilder = _context.Select<CrSiteMessage>("cm.*")
                                       .From("CrSiteMessage cm")
                                       .Where(sql)
                                       .Parameter("customersysno", customersysNo)
                                       .Paging(pageIndex, pageSize).OrderBy("SendDate desc").QueryMany();
            }
            return countBuilder;
        }

        /// <summary>
        /// 根据接收人获取所有未删除站内信数量
        /// </summary>
        /// <param name="customerSysNo">接收人id</param>
        /// <returns>所有未删除的站内信数量</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public override Dictionary<int, int> GetCount(int customerSysNo)
        {
            var dictionary = new Dictionary<int, int>
                {
                    {(int) CustomerStatus.站内信状态.已读, 0},
                    {(int) CustomerStatus.站内信状态.未读, 0},
                    {-1, 0}
                };

            #region sql条件

            string sql = @"select count(1) from CrSiteMessage where customerSysNo=@customerSysNo and Status = @Status";

            #endregion

            int countRead, countUnRead = 0;
            using (var _context = Context.UseSharedConnection(true))
            {
                countRead = _context.Sql(sql)
                                       .Parameter("customerSysNo", customerSysNo)
                                       .Parameter("Status", (int) CustomerStatus.站内信状态.已读)
                                       .QuerySingle<int>();
                countUnRead = _context.Sql(sql)
                                         .Parameter("customerSysNo", customerSysNo)
                                         .Parameter("Status", (int) CustomerStatus.站内信状态.未读)
                                         .QuerySingle<int>();
            }
            dictionary[(int) CustomerStatus.站内信状态.已读] = countRead;
            dictionary[(int) CustomerStatus.站内信状态.未读] = countUnRead;
            dictionary[-1] = countRead + countUnRead;
            return dictionary;
        }

        /// <summary>
        /// 获取站内信单条数据
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>站内信单条数据</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public override CrSiteMessage GetSiteMessage(int sysNo)
        {
            return
                Context.Sql(@"select * from CrSiteMessage where SysNO = @0", sysNo).QuerySingle<CrSiteMessage>();
        }

        /// <summary>
        /// 根据站内消息系统编号把消息设置为已读或删除
        /// </summary>
        /// <param name="sysNo">站内消息系统编号</param>
        /// <param name="status">站内消息状态</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public override int SetMessageStatus(int sysNo, int status)
        {
            int rowsAffected =
                Context.Sql(
                    "update CrSiteMessage set status=@status where sysNo=@sysNo")
                       .Parameter("status", status)
                       .Parameter("sysNo", sysNo)
                       .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除站内信
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("CrSiteMessage")
                          .Where("Sysno", sysNo)
                          .Execute();
        }
    }
}
