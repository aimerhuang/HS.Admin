using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 对List数据进行分页处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPageUtil<T> {
        private IList<T> _list;
        private int _pageSize = 1;
        private int _pageCount = 0;

        /// <summary>
        /// 对List数据进行分页处理
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="pageSize">页大小</param>
        public ListPageUtil(IList<T> list, int pageSize)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (pageSize < 1) throw new ArgumentException("pageSize不能小于零");
            _list = list;
            _pageSize = pageSize;
            _pageCount = (list.Count + _pageSize - 1) / _pageSize; 
        }

        /// <summary>
        /// 获取 页总数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
        }

        /// <summary>
        /// 获取/设置 页大小
        /// </summary>
        public int PageSize {
            get {
                return _pageSize;
            }
            set {
                if (value < 1) throw new ArgumentException("PageSize不能小于零");
                _pageSize = value;
                _pageCount = (_list.Count + _pageSize - 1) / _pageSize; 
            }
        }
        /// <summary>
        /// 获取 第几页的数据
        /// </summary>
        /// <param name="pageIndex">页索引 从1开始</param>
        /// <returns></returns>
        public IList<T> GetDate(int pageIndex) {
            if (pageIndex < 1) throw new ArgumentException("pageIndex应该从1开始"); 
            var result = new List<T>();
            var begin = (pageIndex - 1) * PageSize;
            var end = pageIndex * PageSize;
            if (end > _list.Count) end = _list.Count;
            for (; begin < end; begin++)
            {
                result.Add(_list[begin]);
            }
            return result;
        }

    }

}
