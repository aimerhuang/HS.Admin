
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商支付方式维护数据访问层
    /// </summary>
    /// <remarks>2016-05-11 杨浩 创建</remarks>
    public class DsDealerPayTypeDaoImpl : IDsDealerPayTypeDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public override int Create(DsDealerPayType model)
        {
            return Context.Insert("DsDealerPayType", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public override int Update(DsDealerPayType model)
        {
            return Context.Update("DsDealerPayType", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 是否唯一AppKey
        /// </summary>
        /// <param name="appKey">分销商商户号</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-23 杨浩 创建</remarks>
        public override bool IsOnlyAppKey(string appKey, int dealerSysNo)
        {
            var count=Context.Sql("select count(1) from DsDealerPayType where AppKey=@AppKey and DealerSysNo<>@DealerSysNo")
                       .Parameter("AppKey", appKey)
                       .Parameter("DealerSysNo", dealerSysNo)
                       .QuerySingle<int>();
            return count == 0;
        }
        /// <summary>
        /// 根据分销商系统编号获取分销商支付方式实体
        /// </summary>
        /// <param name="iDealerSysNo">分销商系统编号</param>
        /// <returns>分销商支付方式实体</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public override DsDealerPayType GetByDealerSysNo(int iDealerSysNo)
        {
            return Context.Select<DsDealerPayType>("*")
                          .From("DsDealerPayType")
                          .Where("DealerSysNo=@DealerSysNo")
                          .Parameter("DealerSysNo", iDealerSysNo)
                          .QuerySingle();
        }
        /// <summary>
        /// 根据支付商户编号获取分销商支付类型详情
        /// </summary>
        /// <param name="merchantId">商户编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-19 杨浩 创建</remarks>
        public override DsDealerPayType GetDealerPayTypeByMerchantId(string merchantId)
        {
            return Context.Sql("select top 1 * from DsDealerPayType where AppKey=@AppKey")                             
                         .Parameter("AppKey", merchantId)
                         .QuerySingle<DsDealerPayType>();
        }
        #endregion
    }
}
