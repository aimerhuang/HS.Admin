using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.SellBusiness
{
    /// <summary>
    /// 返利记录数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-09-15 王耀发 创建
    /// </remarks>
    public class CrCustomerRebatesRecordDaoImpl : ICrCustomerRebatesRecordDao
    {
        
        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返回返利记录信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override Pager<CBCrCustomerRebatesRecord> GetCrCustomerRebatesRecordList(ParaCustomerRebatesRecordFilter filter)
        {
            string where="";
            if (!string.IsNullOrWhiteSpace(filter.SysNoList))
            {
                where += " and a.sysNo in("+filter.SysNoList+") ";
            }
            string sql = @"(select a.*, b.Account as RecommendAccount,b.Name as RecommendName,c.Account as ComplyAccount,c.Name as ComplyName,rd.DealerName as RDealerName  
                            ,so.OrderAmount+so.OrderDiscountAmount as OrderAmount,ware.BackWarehouseName as WarehouseName,
                            so.ProductAmount,so.ProductChangeAmount,so.FreightAmount,so.CouponAmount
                    from CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo 
                    left join CrCustomer c on a.ComplySysNo = c.SysNo 
                    left join DsDealer rd on b.DealerSysNo = rd.SysNo   
                    left join SoOrder so on so.SysNo=a.OrderSysNo 
                    left join WhWarehouse ware on so.DefaultWarehouseSysNo=ware.SysNo
                    where (@0 = 0 or a.OrderSysNo = @1)  
                    " + where+@"
                                   ) tb";

            var dataList = Context.Select<CBCrCustomerRebatesRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.OrderSysNo,filter.OrderSysNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBCrCustomerRebatesRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.RebatesTime desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 获得返利实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CrCustomerRebatesRecord GetEntity(int SysNo)
        {

            return Context.Sql("select a.* from CrCustomerRebatesRecord a where a.SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<CrCustomerRebatesRecord>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(CrCustomerRebatesRecord entity)
        {
            entity.SysNo = Context.Insert("CrCustomerRebatesRecord", entity)
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
        public override int Update(CrCustomerRebatesRecord entity)
        {

            return Context.Update("CrCustomerRebatesRecord", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("CrCustomerRebatesRecord")
                               .Where("SysNo", sysNo)
                               .Execute();
        }
       
        #endregion

        #region 获取返利信息

        /// <summary>
        /// 获得返利记录列表(按订单号升序排列)排除没有完成的退换货订单
        /// </summary>
        /// <param name="delayDay">获取可以执行返利的记录的天数</param>
        /// <remarks>2015-10-19 杨云奕 添加</remarks>
        public override List<CrCustomerRebatesRecord> GetRebatesRecordList(int delayDay, int dealerSysNo = -1, int orderSysNo = 0)
        {
            string whereStr = "";
            if (dealerSysNo >= 0)
                whereStr = string.Format(" and b.DealerSysNo={0}", dealerSysNo);
            if (orderSysNo > 0)
                whereStr += string.Format(" and b.SysNo={0}", orderSysNo);
            string sql = @"select a.* from CrCustomerRebatesRecord a 
                           left join SoOrder b on a.OrderSysNo = b.SysNo 
                           where b.ReceivingConfirmDate<'" + DateTime.Now.AddDays(-1 * delayDay).ToString() + "' and a.Status=0 "+whereStr+"  order by a.OrderSysNo asc";
            return Context.Sql(sql).QueryMany<CrCustomerRebatesRecord>();
        }
        /// <summary>
        /// 设置返利佣金操作
        /// </summary>
        /// <param name="SysNo">返利佣金列表</param>
        /// <remarks>2015-10-19 杨云奕 添加</remarks>
        public override void SetCrCustomerRebatesRecordToCustomerBrokerage(int SysNo)
        {
            string sql = "select * from CrCustomerRebatesRecord where SysNo=" + SysNo;
            CrCustomerRebatesRecord recordMod = Context.Sql(sql).QuerySingle<CrCustomerRebatesRecord>();
            //获取返利列表集合
            List<CBRcReturn> list = Hyt.DataAccess.RMA.IRcReturnDao.Instance.GetRmaReturnListByOrderSysNo(recordMod.OrderSysNo);
            //获取客户信息
            CBCrCustomer customer = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetModel(recordMod.RecommendSysNo);
            if (list.Count > 0)
            {
                //更新返利列表状态，作废的部分
                recordMod.Status = "-1";
                Update(recordMod);
                //减去用户的冻结金额部分
                customer.BrokerageFreeze -= recordMod.Rebates;
                Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Update(customer);
            }
            else
            {
                //更新返利列表状态，完成的部分
                recordMod.Status = "1";
                Update(recordMod);
                //增加用户的资金
                customer.BrokerageFreeze -= recordMod.Rebates;
                //customer.BrokerageTotal += recordMod.Rebates;
                customer.Brokerage += recordMod.Rebates;
                Hyt.DataAccess.CRM.ICrCustomerDao.Instance.Update(customer);
            }
        }
        #endregion
        /// <summary>
        /// 获取订单可返点的状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public override int GetOrderRebatesStatus(int orderSysNo)
        {
            var result = Context.StoredProcedure("pro_GetOrderRebatesStatus")
               .Parameter("orderSysNo",orderSysNo)             
               .QuerySingle<int>();
            return result;
        }
    }
}
