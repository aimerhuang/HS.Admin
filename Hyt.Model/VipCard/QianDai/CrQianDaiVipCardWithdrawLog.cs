﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.VipCard.QianDai
{
    /// <summary>
    /// 钱袋宝会员卡提现日志
    /// </summary>
    /// <remarks>2017-04-02 杨浩 创建</remarks>
    public class CrQianDaiVipCardWithdrawLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 提现流水号
        /// </summary>
        public string WithdrawNo { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 状态,0 :接收成功，-1:失败,1:成功
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 返回描述
        /// </summary>
        public string RetMessage { get; set; }
        /// <summary>
        /// 充值后卡余额（不代表最新余额）
        /// </summary>
        public decimal CardBalance { get; set; }
        /// <summary>
        /// 充值后商户余额（不代表最新余额）
        /// </summary>
        public decimal ShopBalance { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }
    }
}
