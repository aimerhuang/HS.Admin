using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 代理商拓展
    /// </summary>
    /// <remarks> 2016-04-13 刘伟豪 创建</remarks>
    [Serializable]
    public class CBDsAgent : DsAgent
    {
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string LevelName { get; set; }

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
        /// 资金系统编号
        /// </summary>
        public int AgentPrePaymentSysNo { get; set; }
    }
}