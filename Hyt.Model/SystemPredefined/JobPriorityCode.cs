namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 任务池优先级编码
    /// </summary>
    /// <remarks>2014-02-28 余勇 创建</remarks>
    public enum JobPriorityCode
    {
        /// <summary>
        /// 普通
        /// </summary>
        PC001,

        /// <summary>
        /// 普通百城当日达
        /// </summary>
        PC010,

        /// <summary>
        /// 商城-第三方快递
        /// </summary>
        PC020,

        /// <summary>
        /// 商城-百城当日达
        /// </summary>
        PC030,

        /// <summary>
        /// 退回订单 
        /// </summary>
        PC040,

        /// <summary>
        /// 门店自提订单 
        /// </summary>
        PC050,

        /// <summary>
        /// 旗舰店当日达 
        /// </summary>
        PC035,

        /// <summary>
        /// 旗舰店第三方快递 
        /// </summary>
        PC025
        
    }
}
