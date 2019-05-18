using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 优惠券筛选字段
    /// </summary>
    /// <remarks>2013-08-21 黄志勇 创建</remarks>
    public class ParaCoupon
    {
        private DateTime? _endTime;

        /// <summary>
        /// 当前页
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 来源描述
        /// </summary>
        public string SourceDescription { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 有效时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 有效时间结束
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                //结束日期+1
                return _endTime == null ? (DateTime?) null : _endTime.Value.AddDays(1);
            }
            set { _endTime = value; }
        }

        /// <summary>
        /// 过期时间（>=该参数的数据）
        /// </summary>
        /// <remarks>2013-12-6 朱家宏 添加</remarks>
        public DateTime? ExpiredTime { get; set; }
        /// <summary>
        /// 优惠卷代码
        /// </summary>
        /// <remarks>2013-12-6 朱家宏 添加</remarks>
        public string CouponCode { get; set; }
        /// <summary>
        /// 允许使用数量（>=该参数的数据 同时 已使用数小于允许使用数）
        /// </summary>
        /// <remarks>2013-12-6 朱家宏 添加</remarks>
        public int? UseQuantity { get; set; }
        ///// <summary>
        ///// 允许绑定的数据()
        ///// </summary>
        ///// <remarks>2013-12-10 朱家宏 添加</remarks>
        //public bool CanBeAssigned { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// 2013-12-30 朱家宏 添加
        public string Description { get; set; }
        /// <summary>
        /// 使用平台
        /// </summary>
        /// <remarks>2013-12-30 朱家宏 添加</remarks>
        public int? UsePlatform { get; set; }
        /// <summary>
        /// 优惠卡
        /// </summary>
        /// <remarks>2013-12-30 朱家宏 添加</remarks>
        public int? IsCouponCard { get; set; }
        /// <summary>
        /// 可用平台列表
        /// </summary>
        /// <remarks>2013-01-02 朱家宏 添加</remarks>
        //public IList<string> UsePlatformList { get; set; }
        /// <summary>
        /// 网站使用
        /// </summary>
        public int? WebPlatform { get; set; }
        /// <summary>
        /// 门店使用
        /// </summary>
        public int? ShopPlatform { get; set; }
        /// <summary>
        /// 手机商城使用
        /// </summary>
        public int? MallAppPlatform { get; set; }
        /// <summary>
        /// 物流App使用
        /// </summary>
        public int? LogisticsAppPlatform { get; set; }
        /// <summary>
        /// 查询权限范围
        /// </summary>
        public bool[] Permissions { get; set; }
    }
}
