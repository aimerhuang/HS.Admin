using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// 分销商城快递代码操作类
    /// </summary>
    /// <remarks>2014-03-25 唐文均 创建</remarks>
    public class DsMallExpressCodeDaoImpl : IDsMallExpressCodeDao
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型</param>
        /// <param name="deliveryType">快递方式</param>
        /// <returns>分销商城快递代码</returns>
        /// <remarks>2014-03-25 唐文均 创建</remarks>
        public override DsMallExpressCode GetEntity(int mallTypeSysNo, int deliveryType)
        {
            const string sql = @"select * from DsMallExpressCode
where DeliveryType = @DeliveryType and MallTypeSysNo = @MallTypeSysNo";
            return Context.Sql(sql)
                          .Parameter("DeliveryType", deliveryType)
                          .Parameter("MallTypeSysNo", mallTypeSysNo)
                          .QuerySingle<DsMallExpressCode>();
        }


        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        public override DsMallExpressCode Get(int sysNo)
        {
            return Context.Sql("select * from DsMallExpressCode where SysNo=@SysNo")
                       .Parameter("SysNo", sysNo)
                       .QuerySingle<DsMallExpressCode>();
        }

        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <param name="expressCode">第三方快递代码</param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public override DsMallExpressCode Get(int mallTypeSysNo, int deliveryType, string expressCode)
        {
            const string sql = @"select * from DsMallExpressCode 
                                    where DeliveryType=@DeliveryType and MallTypeSysNo=@MallTypeSysNo and lower(ExpressCode)=lower(@ExpressCode)";
            return Context.Sql(sql)
                          .Parameter("DeliveryType", deliveryType)
                          .Parameter("MallTypeSysNo", mallTypeSysNo)
                          .Parameter("ExpressCode", expressCode)
                          .QuerySingle<DsMallExpressCode>();
        }
        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-20 罗勤瑶 创建</remarks>
        public override DsMallExpressCode Get(int mallTypeSysNo, int deliveryType)
        {
            const string sql = @"select top 1 * from DsMallExpressCode 
                                    where DeliveryType=@DeliveryType and MallTypeSysNo=@MallTypeSysNo ";
            return Context.Sql(sql)
                          .Parameter("DeliveryType", deliveryType)
                          .Parameter("MallTypeSysNo", mallTypeSysNo)
                        
                          .QuerySingle<DsMallExpressCode>();
        }
        /// <summary>
        /// 查询经销商城快递代码
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>经销商城快递代码分页数据</returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public override Pager<CBDsMallExpressCode> Query(ParaDsMallExpressCodeFilter filter)
        {
            string sqlSelect = @"mec.sysno,mec.expresscode,mt.mallcode,mt.mallname,dt.deliverytypename";

            string sqlFrom = @"DsMallExpressCode mec left join DsMallType mt on mec.malltypesysno=mt.sysno
                               left join LgDeliveryType dt on mec.deliverytype=dt.sysno
                              ";

            var parameters = new ArrayList();
            System.Text.StringBuilder strWhere = new System.Text.StringBuilder("1=1");
            if (filter.MallTypeSysNo.HasValue && filter.MallTypeSysNo.Value > 0) //经销商城类型
            {
                strWhere.AppendFormat(" and mt.SysNo={0}", filter.MallTypeSysNo.Value);
                //parameters.Add(filter.MallTypeSysNo.Value);
            }
            if (filter.DeliveryTypeSysNo.HasValue && filter.DeliveryTypeSysNo.Value > 0) //配送方式
            {
                strWhere.AppendFormat(" and dt.SysNo={0}", filter.DeliveryTypeSysNo.Value);
                //parameters.Add(filter.DeliveryTypeSysNo.Value);
            }
            if (!string.IsNullOrWhiteSpace(filter.ExpressCode)) //第三方快递代码
            {
                strWhere.Append(" and CHARINDEX(lower(@0),lower(mec.ExpressCode)) > 0"); //and instr(lower(mec.ExpressCode),lower(:ExpressCode)) > 0
                parameters.Add(filter.ExpressCode);
            }

            var pager = new Pager<Model.Transfer.CBDsMallExpressCode>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };

            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<Model.Transfer.CBDsMallExpressCode>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(strWhere.ToString())
                                       .Parameters(parameters.ToArray())
                                       .Paging(filter.Id, filter.PageSize) //index从1开始
                                       .OrderBy("mec.sysno desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(strWhere.ToString())
                                   .Parameters(parameters.ToArray())
                                   .QuerySingle();
                pager.Rows = lstResult;
                pager.TotalRows = count;
            }

            return pager;
        }

        /// <summary>
        /// 插入分销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public override int Insert(DsMallExpressCode model)
        {
            var sysNo = Context.Insert("DsMallExpressCode", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新分销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public override int Update(DsMallExpressCode model)
        {
            return Context.Update("DsMallExpressCode", model)
                       .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate)
                       .Where("SysNo", model.SysNo)
                       .Execute();
        }

        /// <summary>
        /// 根据sysNo删除DsMallExpressCode表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsMallExpressCode")
                    .Where("SysNo", sysNo)
                    .Execute();
        }
    }
}
