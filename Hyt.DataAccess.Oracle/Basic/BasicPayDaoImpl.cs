using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.BaseInfo;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 支付Impl
    /// </summary>
    /// <remarks>2013-09-06 周唐炬 创建</remarks>
    public class BasicPayDaoImpl : IBasicPayDao
    {
        /// <summary>
        /// 添加新支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int PaymentTypeCreate(BsPaymentType model)
        {
            return Context.Insert<BsPaymentType>("BsPaymentType", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int PaymentTypeUpdate(BsPaymentType model)
        {
            return Context.Update<BsPaymentType>("BsPaymentType", model).AutoMap(x => x.SysNo).Where(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 获取全部支付方式
        /// </summary>
        /// <returns>支付方式列表</returns>
        /// <remarks>2013－06-13 黄志勇 创建</remarks>
        public override IList<Model.BsPaymentType> LoadAllPayType()
        {
            return Context.Sql("select * from BsPaymentType order by displayorder").QueryMany<Model.BsPaymentType>();
        }

        /// <summary>
        /// 获取所有支付方式,可自由配置关联查询
        /// </summary>
        /// <returns>配送方式</returns>
        /// <remarks>
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        public override IList<CBBsPaymentType> GetAll()
        {
            return Context.Sql("select * from BsPaymentType order by displayorder").QueryMany<CBBsPaymentType>();
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>
        /// 2013-08-20 郑荣华 创建
        /// </remarks>
        /// <remarks>2013-09-09 周唐炬 加入支付名称PaymentName模糊查询</remarks>
        public override IList<CBBsPaymentType> GetPaymentTypeList(ParaPaymentTypeFilter filter)
        {
            //            const string sqlWhere = @"
            //                (@PaymentName IS NULL OR REGEXP_charindex(t.PaymentName, @PaymentName, 1, 1, 1, 'i') > 0)
            //                AND (@IsOnlinePay is null or t.IsOnlinePay= @IsOnlinePay)
            //                and (@isonlinevisible is null or t.isonlinevisible= @isonlinevisible)
            //                and (@status is null or t.status= @status)
            //                and (@PaymentType is null or t.PaymentType=@PaymentType)
            //                and (@RequiredCardNumber is null or t.RequiredCardNumber=@RequiredCardNumber)
            //               ";

            const string sqlWhere = @"
                (@PaymentName IS NULL OR t.PaymentName=@PaymentName)
                AND (@IsOnlinePay is null or t.IsOnlinePay= @IsOnlinePay)
                and (@isonlinevisible is null or t.isonlinevisible= @isonlinevisible)
                and (@status is null or t.status= @status)
                and (@PaymentType is null or t.PaymentType=@PaymentType)
                and (@RequiredCardNumber is null or t.RequiredCardNumber=@RequiredCardNumber)
               ";


            return Context.Select<CBBsPaymentType>("t.*")
                                .From("BsPaymentType t")
                                .Where(sqlWhere)
                                .Parameter("PaymentName", filter.PaymentName)
                //.Parameter("PaymentName", filter.PaymentName)
                                .Parameter("IsOnlinePay", filter.IsOnlinePay)
                //.Parameter("IsOnlinePay", filter.IsOnlinePay)
                                .Parameter("isonlinevisible", filter.IsOnlineVisible)
                //.Parameter("isonlinevisible", filter.IsOnlineVisible)
                                .Parameter("status", filter.Status)
                //.Parameter("status", filter.Status)
                                .Parameter("PaymentType", filter.PaymentType)
                //.Parameter("PaymentType", filter.PaymentType)
                                .Parameter("RequiredCardNumber", filter.RequiredCardNumber)
                //.Parameter("RequiredCardNumber", filter.RequiredCardNumber)
                                .QueryMany();
        }

        /// <summary>
        /// 根据配送方式获取对应的支付方式列表
        /// </summary>
        /// <param name="deliverySysNo">配送方式</param>
        /// <returns>根据配送方式获取对应的支付方式列表</returns>
        /// <remarks>
        /// 2013-06-17 朱成果 创建
        /// </remarks>
        public override IList<Model.BsPaymentType> LoadPayTypeListByDeliverySysNo(int deliverySysNo)
        {
            return Context.Select<Model.BsPaymentType>("t1.*")
                          .From("BsPaymentType  t1 inner join BsDeliveryPayment  t2 on t1.SysNo=t2.PaymentSysNo")
                          .Where("t2.DeliverySysNo=@deliverySysNo")
                          .Parameter("deliverySysNo", deliverySysNo)
                          .QueryMany();
        }

        /// <summary>
        /// 根据主键获取支付类型
        /// </summary>
        /// <param name="sysNo">支付系统编号</param>
        /// <returns>支付类型</returns>
        /// <remarks>2013-06-20 朱家宏 创建</remarks>
        public override Model.BsPaymentType GetPaymentType(int sysNo)
        {
            return Context.Sql("select * from bspaymenttype where SysNo=@0", sysNo).QuerySingle<Model.BsPaymentType>();
        }

        /// <summary>
        /// 支付名称验证
        /// </summary>
        /// <param name="paymentName">支付名称</param>
        /// <param name="sysNo">支付方式系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int PaymentTypeVerify(string paymentName, int? sysNo)
        {
            const string sql = @"select count(1) from BsPaymentType where PaymentName = @0 AND (@1 IS NULL OR SYSNO<>@1)";
            var paras = new object[]
                {
                    paymentName,
                    sysNo
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<int>();
        }
    }
}
