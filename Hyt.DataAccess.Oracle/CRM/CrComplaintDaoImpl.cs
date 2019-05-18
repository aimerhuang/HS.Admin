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
    /// 投诉数据访问  
    /// </summary>
    /// <remarks>2013－07-09 苟治国 创建</remarks>
    public class CrComplaintDaoImpl : ICrComplaintDao
    {
        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override Model.CrComplaint GetModelSingle(int sysNo)
        {
            const string strSql = @"select * from CrComplaint where SysNO = @sysNO";
            var result = Context.Sql(strSql)
                                .Parameter("sysNO", sysNo)
                                .QuerySingle<CrComplaint>();
            return result;
        }

        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override Model.CBCrComplaint GetModel(int sysNo)
        {
            const string strSql = @"select cp.*,cc.name,cc.MobilePhoneNumber from CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno left join CrComplaintReply cr on cp.customersysno=cc.sysno where cp.sysNO = @sysNO";
            var result = Context.Sql(strSql)
                                .Parameter("sysNO", sysNo)
                                .QuerySingle<CBCrComplaint>();
            return result;
        }

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pager">会员投诉查询条件</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override Pager<Model.CBCrComplaint> Seach(Pager<CBCrComplaint> pager)
        {
            //select cp.*,cc.name,cc.MobilePhoneNumber from CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno left join CrComplaintReply cr on cp.customersysno=cc.sysno
            //where cp.Status='1' and cp.ComplainType='1' and cp.OrderSysNo='1' and cc.MobilePhoneNumber='13002882316' or  cr.ReplyerType='2'
            #region sql条件
            string sqlWhere = @"(@Status=-1 or cp.Status =@Status)
                         and (@ComplainType=-1 or cp.ComplainType =@ComplainType)
                         and (@OrderSysNo=0 or cp.OrderSysNo =@OrderSysNo)
                         and (@MobilePhoneNumber is null or cc.MobilePhoneNumber =@MobilePhoneNumber)
                         and (@customersysno=-1 or cp.customersysno =@customersysno)";
            // and (:ReplyerType=-1 or cr.ReplyerType =:ReplyerType)
            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCrComplaint>("cp.*,cc.MobilePhoneNumber,cc.Name")
                                    .From("CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno")// left join CrComplaintReply cr on cp.customersysno=cc.sysno
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("ComplainType", pager.PageFilter.ComplainType)
                                    .Parameter("OrderSysNo", pager.PageFilter.OrderSysNo)
                                    .Parameter("MobilePhoneNumber", pager.PageFilter.MobilePhoneNumber)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cp.sysNO desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From("CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno")// left join CrComplaintReply cr on cp.customersysno=cc.sysno
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("ComplainType", pager.PageFilter.ComplainType)
                                    .Parameter("OrderSysNo", pager.PageFilter.OrderSysNo)
                                    .Parameter("MobilePhoneNumber", pager.PageFilter.MobilePhoneNumber)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override int Insert(Model.CrComplaint model)
        {
            var result = Context.Insert<CrComplaint>("CrComplaint", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override int Update(Model.CrComplaint model)
        {
            int rowsAffected = Context.Update<Model.CrComplaint>("CrComplaint", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新广告组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override int UpdateStatus(string status, string strWhere)
        {
            string sql = string.Format("update CrComplaint set Status = {0} where SysNo in ({1})", status, strWhere);
            var rowsAffected = Context.Sql(sql)
                .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("CrComplaint")
                                      .Where("SYSNO", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}
