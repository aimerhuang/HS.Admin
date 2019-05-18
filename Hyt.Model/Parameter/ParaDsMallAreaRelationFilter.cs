
namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 商城地区关联查询参数
    /// </summary>
    /// <remarks>2014-10-14 缪竞华 创建</remarks>
    public struct ParaDsMallAreaRelationFilter
    {
        private int _pageSize;
        
        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 商城地区名称
        /// </summary>
        public string MallAreaName { get; set; }

        /// <summary>
        /// 商城地区名称
        /// </summary>
        public string AreaName { get; set; }
    }
}
