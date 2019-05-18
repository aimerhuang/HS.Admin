
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Distribution
{
    public class DsDealerPayTypeBo : BOBase<DsDealerPayTypeBo>
    {
        #region 操作
        /// <summary>
        /// 创建分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public int Create(DsDealerPayType model)
        {
            return IDsDealerPayTypeDao.Instance.Create(model);     
        }

        /// <summary>
        /// 修改分销商支付方式
        /// </summary>
        /// <param name="model">分销商支付方式实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public int Update(DsDealerPayType model)
        {
           return IDsDealerPayTypeDao.Instance.Update(model);      
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
        public bool IsOnlyAppKey(string appKey, int dealerSysNo=0)
        {
            return IDsDealerPayTypeDao.Instance.IsOnlyAppKey(appKey,dealerSysNo);
        }
        /// <summary>
        /// 根据分销商系统编号获取分销商支付方式实体
        /// </summary>
        /// <param name="iDealerSysNo">分销商系统编号</param>
        /// <returns>分销商支付方式实体</returns>
        /// <remarks>2016-05-11 杨浩 创建</remarks>
        public DsDealerPayType GetByDealerSysNo(int iDealerSysNo)
        {
           return IDsDealerPayTypeDao.Instance.GetByDealerSysNo(iDealerSysNo);
        }
        /// <summary>
        /// 根据支付商户编号获取分销商支付类型详情
        /// </summary>
        /// <param name="merchantId">商户编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-19 杨浩 创建</remarks>
        public DsDealerPayType GetDealerPayTypeByMerchantId(string merchantId)
        {
            return IDsDealerPayTypeDao.Instance.GetDealerPayTypeByMerchantId(merchantId);
        }
        #endregion
    }
}
