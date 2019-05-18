using System;

namespace Hyt.Util.Extension
{
    /// <summary>
    /// 数字计算
    /// </summary>
    /// <remarks>2014-01-22 吴文强 创建</remarks>
    public static class MathExtension
    {
        /// <summary>
        /// 将标签替换成相应的值
        /// </summary>
        /// <param name="unitText">标签名称</param>
        /// <param name="Formula">计算公式</param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        /// <remarks>2015-11-28 杨云奕 添加</remarks>
        public static string FormulaChange(string unitText, string Formula, decimal value)
        {
            if (Formula.IndexOf(unitText) != -1)
            {
                Formula = Formula.Replace(unitText, value.ToString());
            }
            return Formula;
        }
            /// <summary>
        /// 执行公式计算
        /// </summary>
        /// <param name="Formula">计算公式</param>
        /// <param name="outMsg">返回信息。成功 则计算通过。</param>
        /// <returns></returns>
        public static decimal FormulaCalculate(string Formula, ref string outMsg)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                decimal value= Convert.ToDecimal(dt.Compute(Formula, "false"));
                outMsg="成功";
                return value;
            }
            catch (Exception e)
            {
                outMsg = e.Message;
                return 0;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Formula"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool CheckFormulaTips(string Formula, string unit)
        {
            if (Formula.IndexOf(unit) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 只舍不入 (eg:0.156 取2位小数时，0.15)
        /// </summary>
        /// <param name="d">要舍入的小数。</param>
        /// <param name="decimals">返回值中的小数位数。</param>
        /// <returns>最接近 d 的 decimals 位小数的数字。如果 d 的小数数字小于 decimals，则返回的 d 保持不变。</returns>
        /// <remarks>2014-01-22 吴文强 创建</remarks>
        public static decimal RoundToShe(this decimal d, int decimals = 0)
        {
            var arrD = d.ToString().Split('.');
            if (arrD.Length == 1 || arrD.Length == 2 && arrD[1].TrimEnd('0').Length <= decimals)
            {
                return d;
            }

            var str = "0.";
            for (var j = 0; j < decimals; j++)
            {
                str += "0";
            }
            str += "5";
            var dec = Convert.ToDecimal(str);
            return RoundToAwayFromZero(d - dec, decimals);
        }

        /// <summary>
        /// 只入不舍 (eg:0.151 取2位小数时，0.16)
        /// </summary>
        /// <param name="d">要舍入的小数。</param>
        /// <param name="decimals">返回值中的小数位数。</param>
        /// <returns>最接近 d 的 decimals 位小数的数字。如果 d 的小数数字小于 decimals，则返回的 d 保持不变。</returns>
        /// <remarks>2014-01-22 吴文强 创建</remarks>
        public static decimal RoundToRu(this decimal d, int decimals = 0)
        {
            var arrD = d.ToString().Split('.');
            if (arrD.Length == 1 || arrD.Length == 2 && arrD[1].TrimEnd('0').Length <= decimals)
            {
                return d;
            }

            var str = "0.";
            for (var j = 0; j < decimals; j++)
            {
                str += "0";
            }
            str += "4";
            var dec = Convert.ToDecimal(str);
            return RoundToAwayFromZero(d + dec, decimals);
        }

        /// <summary>
        /// 四舍五入 (eg:0.155 取2位小数时，0.16)
        /// </summary>
        /// <param name="d">要舍入的小数。</param>
        /// <param name="decimals">返回值中的小数位数。</param>
        /// <returns>最接近 d 的 decimals 位小数的数字。如果 d 的小数数字小于 decimals，则返回的 d 保持不变。</returns>
        /// <remarks>2014-01-22 吴文强 创建</remarks>
        public static decimal RoundToAwayFromZero(this decimal d, int decimals)
        {
            return Math.Round(d, decimals, MidpointRounding.AwayFromZero);
        }
    }
}
