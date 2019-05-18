using Extra.UpGrade.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 淘宝备注信息
    /// </summary>
    /// <remarks>2014-03-25 黄波 创建</remarks>
    public class TaobaoRemarksParameters : IRemarksParameters
    {
        /// <summary>
        /// 备注旗帜类型
        /// </summary>
        public FlagType Flag { get; set; }

        /// <summary>
        /// 是否对memo的值置空
        /// true:则不管传入的memo字段的值是否为空，都将会对已有的memo值清空
        /// false:若memo为空则忽略对已有memo字段的修改，若memo非空，则使用新传入的memo覆盖已有的memo的值
        /// </summary>
        public bool Reset { get; set; }
    }

    /// <summary>
    /// 淘宝订单备注旗帜类型
    /// </summary>
    /// <remarks>2014-03-25 黄波 创建</remarks>
    public enum FlagType
    {
        灰色旗帜 = 0,
        红色旗帜,
        黄色旗帜,
        绿色旗帜,
        蓝色旗帜,
        /// <summary>
        /// 已升舱标识旗
        /// </summary>
        粉红色旗帜
    }
}
