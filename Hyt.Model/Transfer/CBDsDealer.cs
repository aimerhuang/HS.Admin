using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商信息
    /// </summary>
    /// <remarks>2014-01-09 周唐炬 添加注释</remarks>
    public class CBDsDealer : DsDealer
    {
        /// <summary>
        /// 累积预存金额
        /// </summary>
        public decimal TotalPrestoreAmount { get; set; }

        /// <summary>
        /// 预存款可用余额
        /// </summary>
        public decimal AvailableAmount { get; set; }

        /// <summary>
        /// 预存款冻结金额
        /// </summary>
        public decimal FrozenAmount { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 地区全称（区,市,省）
        /// </summary>
        public string AreaAllName
        {
            get { return _areaAllName; }
            set { _areaAllName = Reverse(value ?? ""); }
        }

        /// <summary>
        /// （区,市,省）变为（省市区）
        /// </summary>
        /// <returns>省市区</returns>
        /// <remarks>2014-01-09 周唐炬 添加注释</remarks>
        private static string Reverse(string x)
        {
            var t = x.Split(',').ToList();
            var r = "";
            for (var i = t.Count - 1; i >= 0; i--)
            {
                r += t[i];
            }
            return r;
        }

        private string _areaAllName;

        /// <summary>
        /// 原始编号
        /// </summary>
        public int oSysNo { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public int PSysNo { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 微信公众账号
        /// </summary>
        public string WeiXinNum { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal PayableAmount { get; set; }
        /// <summary>
        /// 收款人开户行
        /// </summary>
        public string RefundBank { get; set; }
        /// <summary>
        /// 收款人开户姓名
        /// </summary>
        public string RefundAccountName { get; set; }
        /// <summary>
        /// 收款人银行账号
        /// </summary>
        public string RefundAccount { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 资金系统编号
        /// </summary>
        public int PrePaymentSysNo { get; set; }

        /// <summary>
        /// 经销商对应会员数
        /// </summary>
        public int CustomerNums { get; set; }
        /// <summary>
        /// 会员级别分销商数
        /// </summary>
        public int CustomerDealerNums { get; set; }
    }
}
