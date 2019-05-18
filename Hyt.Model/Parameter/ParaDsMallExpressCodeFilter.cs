
namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 经销商城快递代码查询参数
    /// </summary>
    /// <remarks>2015-1-19 缪竞华 创建</remarks>
    public class ParaDsMallExpressCodeFilter
    {
        private int _id;

        private int _pageSize;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id 
        {
            get
            {
                if (_id <= 0)
                {
                    _id = 1;
                }
                return _id;
            }
            set { _id = value; }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize <= 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 经销商城类型系统编号
        /// </summary>
        public int? MallTypeSysNo { get; set; }

        /// <summary>
        /// 配送方式系统编号
        /// </summary>
        public int? DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 第三方快递代码
        /// </summary>
        public string ExpressCode { get; set; }
    }
}
