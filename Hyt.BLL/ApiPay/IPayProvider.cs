using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System.IO;
using Hyt.Model.Common;

namespace Hyt.BLL.ApiPay
{
    /// <summary>
    /// 物流推送
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public abstract class IPayProvider
    {
        /// <summary>
        /// 支付企业内部代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract CommonEnum.PayCode Code { get; }       
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public abstract Result ApplyToCustoms(SoOrder order);
        /// <summary>
        /// 海关支付报关查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>
        public abstract Result CustomsQuery(int orderId);
        /// <summary>
        /// 异步回执
        /// </summary>
        /// <param name="requestStr">http请求信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public virtual Result NotifyReceipt(string requestStr) 
        {
            return  new Result()
            {
                Status=false
            };
        }

        /// <summary>
        /// 广州电子口岸海关申报
        /// </summary>
        /// <returns></returns>
        public virtual Result ApplyToCustomsDZ30(SoOrder order)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 广州电子口岸海关申报异步回调
        /// </summary>
        /// <returns></returns>
        public virtual Result NotifyReceiptDZ30(string requestStr)
        {
            return new Result()
            {
                Status = false
            };
        }

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-20 杨浩 创建</remarks> 
        public virtual Result QueryOrderState(string orderId)
        {
            return new Result()
            {
                Status = false
            };
        }

        public virtual Result RestartPushCustomsOrder(int orderId)
        {
            return new Result()
            {
                Status = false
            };
        }

        #region 批量支付
        /// <summary>
        /// 会员卡创建
        /// </summary>
        /// <param name="account">用户信息</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardCreate(object obj)
        {
            return new Result()
            {
                Status = false
            };

        }
        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="rechargeNo">充值流水号</param>
        /// <param name="cardId">会员卡号</param>
        /// <param name="money">充值金额</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardRecharge(string rechargeNo, int cardId, decimal money)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 充值异步
        /// </summary>
        /// <param name="sPara">参数</param>
        /// <param name="notifyId"></param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardRechargeNotify(SortedDictionary<string, string> sPara, string rechargeNo, string sign)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 充值结果
        /// </summary>
        /// <param name="rechargeNo">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardRechargeResult(string rechargeNo)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 会员卡查询
        /// </summary>
        /// <param name="obj">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardQuery(object obj)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 会员卡提现
        /// </summary>
        /// <param name="withdrawNo">提现流水号，要求唯一性 </param>
        /// <param name="cardId">卡号 </param>
        /// <param name="money">提现金额</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public virtual Result VipCardWithdraw(string withdrawNo, int cardId, decimal money)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 会员卡消费
        /// </summary>
        /// <param name="noOrder">商户订单号</param>
        /// <param name="cardId">会员卡号</param>
        /// <returns></returns>
        /// <remarks>2017-4-2 杨浩 创建</remarks>
        public virtual Result VipCardConsume(string noOrder, string cardId)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 提交订单到支付方
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <param name="orderPayLogSysNo">订单支付系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-04-02 杨浩 创建</remarks>
        public virtual Result SubmitOrderToPay(SoOrder order, int orderPayLogSysNo = 0)
        {
            return new Result()
            {
                Status = false
            };
        }
        /// <summary>
        /// 支付结果异步
        /// </summary>
        /// <param name="sPara">参数</param>
        /// <returns></returns>
        /// <remarks>2017-04-02 杨浩 创建</remarks>
        public virtual Result PayNotify(SortedDictionary<string, string> sPara)
        {
            return new Result()
            {
                Status = false
            };
        }
        #endregion

        #region 属性

        /// <summary>
        /// 海关代码
        /// </summary>
        /// <remarks>2016-1-28 杨浩 创建</remarks>
        public virtual string this[int index]
        {
            get
            {
                switch (index)
                {
                    case (int)CommonEnum.海关.广州机场海关:
                        return "GUANGZHOU";
                    default:
                        return "GUANGZHOU";
                }
            }
        }

        private int customs = (int)CommonEnum.海关.广州机场海关;
        /// <summary>
        /// 海关内部编号
        /// </summary>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public int Customs
        {
            get { return customs; }
            set { customs = value; }
        } 
        #endregion
        /// <summary>
        /// 支付配置信息
        /// </summary>
        protected static PayConfig payConfig = Hyt.BLL.Config.Config.Instance.GetPayConfig();
    }
   
}
