using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 投诉回复数据访问  
    /// </summary>
    /// <remarks>2013－07-09 苟治国 创建</remarks>
    public class CrComplaintReplyDaoImpl : ICrComplaintReplyDao
    {
        /// <summary>
        /// 查看会员投诉回复
        /// </summary>
        /// <param name="sysNo">会员投诉回复编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override Model.CrComplaintReply GetModel(int sysNo)
        {
            return Context.Sql(@"select * from CrComplaintReply where SysNO = @0", sysNo).QuerySingle<Model.CrComplaintReply>();
        }

        /// <summary>
        /// 查看会员投诉回复等于会员投诉最后一条记录是否为会员
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override Model.CrComplaintReply GetReplyTop(int sysNo)
        {
            return Context.Sql(@"select *  from CrComplaintReply where  rownum = 1 and ComplaintSysNo=@0 order by SysNo DESC", sysNo).QuerySingle<Model.CrComplaintReply>();
        }

        /// <summary>
        /// 根据条件获取会员投诉回复的列表
        /// </summary>
        /// <param name="sysNo">会员投诉系统编号</param>
        /// <returns>会员投诉回复列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override IList<Model.CBCrComplaintReply> Seach(int sysNo)
        {
            #region sql
            // select ccp.*,case when replyertype=10 then (select s.UserName from SyUser s where s.sysNo=ReplyerSysNo) when replyertype=20 then (select c.Name from CrCustomer c where c.sysNo=ReplyerSysNo) end as UserName from CrComplaintReply ccp where ccp.complaintsysno='1001'
            #endregion

            #region sql条件
            string sqlWhere = "(@ComplaintSysNo is null or ccp.ComplaintSysNo =@ComplaintSysNo)";
            #endregion

            var list = Context.Select<CBCrComplaintReply>("ccp.*,case when replyertype=10 then (select s.UserName from SyUser s where s.sysNo=ReplyerSysNo) when replyertype=20 then (select c.Name from CrCustomer c where c.sysNo=ReplyerSysNo) end as UserName")
                        .From("CrComplaintReply ccp")
                        .Where(sqlWhere)
                        .Parameter("complaintsysno", sysNo)
                        .OrderBy("ccp.replydate desc").QueryMany();
            return list;
        }

        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override int Insert(Model.CrComplaintReply model)
        {
            var result = Context.Insert<CrComplaintReply>("CrComplaintReply", model)
                    .AutoMap(x => x.SysNo)
                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override int Update(Model.CrComplaintReply model)
        {
            int rowsAffected = Context.Update<Model.CrComplaintReply>("CrComplaintReply", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
            return rowsAffected;
        }
    }
}
