using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 投诉数据访问  
    /// </summary>
    /// <remarks>2013-08-06 苟治国 创建</remarks>
    public class ComplaintDaoImpl : IComplaintDao
    {
        /// <summary>
        /// 获取指定会员订单列表
        /// </summary>
        /// <param name="pager">订单查询条件</param>
        /// <returns>会员订单列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public override Pager<SoOrder> GetOrder(Pager<SoOrder> pager, ParaOrderFilter orderFilter)
        {
            #region sql条件
            string sqlWhere = @"(:customersysnos=-1 or customersysno =:customersysno) and (:Status=0 or Status =:Status) and (:beginTime is null or createDate>=:beginTime) and (:endTime is null or createDate<=:endTime)";
            #endregion
            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<SoOrder>("s.*")
                                    .From("soorder s")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("beginTime", orderFilter.BeginDate)
                                    .Parameter("endTime", orderFilter.EndDate)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("s.createdate desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("soorder s")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("beginTime", orderFilter.BeginDate)
                                    .Parameter("endTime", orderFilter.EndDate)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pager">会员投诉查询条件</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public override Pager<Model.CBCrComplaint> GetComplaintList(Pager<CBCrComplaint> pager)
        {
            #region sql条件
            string sqlWhere = @"(@CustomerSysNo=-1 or cp.CustomerSysNo =@CustomerSysNo) and (@Status=-1 or cp.Status =@Status)";
            #endregion
            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CBCrComplaint>("cp.*,cc.MobilePhoneNumber,cc.Name")
                                    .From("CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cp.sysNO desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("CrComplaint cp left join CrCustomer cc on cp.customersysno=cc.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .QuerySingle();
            }
            return pager;
        }
        
        /// <summary>
        /// 根据订单号获取产品图片
        /// </summary>
        /// <param name="ordersysNo">订单编号</param>
        /// <returns>订单所属图片集</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public override IList<Model.PdProduct> GetProductImage(int ordersysNo)
        {
            //select pd.sysno,pd.productimage from SoOrderItem so left join PdProduct pd on so.productsysno=pd.sysno where so.ordersysno=64
            #region sql条件
            string sqlWhere = @"(@ordersysno=-1 or so.ordersysno =@ordersysno)";
            #endregion

            var list = Context.Select<PdProduct>("pd.sysno,pd.productname,pd.productimage")
                                .From("SoOrderItem so left join PdProduct pd on so.productsysno=pd.sysno")
                                .Where(sqlWhere)
                                .Parameter("ordersysno", ordersysNo)
                                .OrderBy("so.sysno").QueryMany();

            return list;
        }

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
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
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public override int Update(Model.CrComplaint model)
        {
            int rowsAffected = Context.Update<Model.CrComplaint>("CrComplaint", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
    }
}