using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 门店提货验证码
    /// </summary>
    /// <remarks>2013-07-03 朱成果 创建</remarks>
    public class WhPickUpCodeDaoImpl : IWhPickUpCodeDao
    {
        /// <summary>
        /// 获取提货验证码
        /// </summary>
        /// <param name="stockOutSysNo">出库单号</param>
        /// <returns>提货验证码</returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks>
        public override WhPickUpCode GetEntityByStockOutNo(int stockOutSysNo)
        {
            return Context.Sql("select * from WhPickupCode where StockOutSysNo=@StockOutSysNo").Parameter("StockOutSysNo", stockOutSysNo).QuerySingle<WhPickUpCode>();
        }

        /// <summary>
        /// 插入验证码数据
        /// </summary>
        /// <param name="entity">验证码实体</param>
        /// <returns>最新编号</returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks> 
        public override int InsertEntity(WhPickUpCode entity)
        {
            var sysNo = Context.Insert("WhPickupCode", entity)
                                  .AutoMap(o => o.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新提货验证码数据
        /// </summary>
        /// <param name="entity">提货验证码实体</param>
        /// <returns></returns>
        ///<remarks>2013-07-06 朱成果 创建</remarks> 
        public override void UpdateEntity(WhPickUpCode entity)
        {
            Context.Update("WhPickupCode", entity)
                 .AutoMap(x => x.SysNo)
                 .Where("SysNo", entity.SysNo)
                 .Execute();
        }

        /// <summary>
        /// 提货码及验证码分页查询
        /// </summary>
        /// <param name="pager">分页列表</param>
        /// <param name="filter">筛选条件</param>
        /// <returns>提货码及验证码分页</returns>
        /// <remarks>2013-12-3 余勇 创建</remarks>
        public override void GetPickUpSmsList(ref Pager<CBWhPickUpCode> pager, ParaWhPickUpCodeFilter filter)
        {
            string sqlField;
            string sqlFrom;
            var paras = new object[]
            {
                filter.MobilePhoneNumber,filter.MobilePhoneNumber
            };
            if (filter.SearchType == 1)
            {
                var quickSearchKeyword = filter.MobilePhoneNumber;
                sqlFrom =
                 @"whpickupcode                                                                                                    
                  where (@MobilePhoneNumber is null or MobilePhoneNumber=@MobilePhoneNumber)
                    and (@StockOutSysNo is null or StockOutSysNo=@StockOutSysNo)";
                sqlField = "SysNo,MobilePhoneNumber,Code as Content,CreatedDate,20 as Status,StockOutSysNo";
                //手机号
                if (VHelper.ValidatorRule(new Rule_Mobile(quickSearchKeyword)).IsPass && quickSearchKeyword.Length >= 11)
                {
                    filter.StockOutSysNo = null;
                }
                //出库单号
                else if (VHelper.ValidatorRule(new Rule_Number(quickSearchKeyword)).IsPass)
                {
                    filter.StockOutSysNo = int.Parse(quickSearchKeyword);
                    filter.MobilePhoneNumber = null;
                }
                else
                {
                    filter.StockOutSysNo = null;
                    filter.MobilePhoneNumber = null;
                }
                paras = new object[] { filter.MobilePhoneNumber, filter.StockOutSysNo };
            }
            else
            {
                sqlFrom =
                   @"ncsms                                                                                                    
                  where @MobilePhoneNumber is null or MobilePhoneNumber=@MobilePhoneNumber";
                sqlField = "SysNo,MobilePhoneNumber,SMSCONTENT as Content,CreatedDate,Status,'' as StockOutSysNo";
            }
            var dataList = Context.Select<CBWhPickUpCode>(sqlField).From(sqlFrom);
            var dataCount = Context.Select<int>("count(0)").From(sqlFrom);

            dataList.Parameter("MobilePhoneNumber", paras[0]);
            dataList.Parameter("StockOutSysNo", paras[1]);

            dataCount.Parameter("MobilePhoneNumber", paras[0]);
            dataCount.Parameter("StockOutSysNo", paras[1]);

            var totalRows = dataCount.QuerySingle();
            List<CBWhPickUpCode> rows = dataList.OrderBy("CREATEDDATE desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

        }
    }
}
