
using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商信息维护接口层
    /// </summary>
    /// <remarks>2016-05-11 杨浩 创建</remarks>
    public abstract class IDsDealerPayTypeDao : DaoBase<IDsDealerPayTypeDao>
    {
        #region 操作

        /// <summary>
        /// 创建分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public abstract int Create(DsDealerPayType model);

        /// <summary>
        /// 修改分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2016-05-11 杨浩  创建</remarks>
        public abstract int Update(DsDealerPayType model);
        /// <summary>
        /// 是否唯一AppKey
        /// </summary>
        /// <param name="appKey">分销商商户号</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-23 杨浩 创建</remarks>
        public abstract bool IsOnlyAppKey(string appKey,int dealerSysNo);
        #endregion

        #region 查询

        /// <summary>
        /// 根据分销商系统编号获取分销商支付方式实体
        /// </summary>
        /// <param name="iDealerSysNo">分销商系统编号</param>
        /// <returns>分销商支付方式实体</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public abstract DsDealerPayType GetByDealerSysNo(int iDealerSysNo);
        /// <summary>
        /// 根据支付商户编号获取分销商支付类型详情
        /// </summary>
        /// <param name="merchantId">商户编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-19 杨浩 创建</remarks>
        public abstract DsDealerPayType GetDealerPayTypeByMerchantId(string merchantId);

        #endregion
    }
}
