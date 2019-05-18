using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T">行数据泛型对象</typeparam>
    /// <remarks>
    /// 2013-03-20 何方 创建
    /// 2013-06-04 余勇 添加分页条件实体类属性
    /// </remarks>
    public class Pager<T> where T: new() 
    {
        //结果集
        private IList<T> _list;
        //结果集id
        private List<int> _idlist;
        //记录数
        private int _totalRows;
        //每页多少条数据
        private int _pageSize;
        //第几页
        private int _currentPage;
        //分页条件实体类
        private T _pageFilter;
       
        /// <summary>
        ///初始化一个新的实例  Initializes a new instance of the <see cref="Pager{T}"/> class.
        ///默认currentPage = 1,pageSize = 10;
        /// </summary>
        /// <remarks>2013-03-20 何方 创建</remarks>
        public Pager()
        {
            _currentPage = 1;
            _pageSize = 10;
            _list = new List<T>();
            _pageFilter = new T();
        }

        /// <summary>
        /// 初始化一个新的实例 Initializes a new instance of the <see cref="PageModel{T}"/> class.
        /// </summary>p
        /// <param name="list">数据集 The list.</param>
        /// <param name="totalRows">总行数 The total rows.</param>
        /// <param name="currentPage">当前页 The current page.</param>
        /// <param name="pageSize">页大小 The page size.</param>
        /// <remarks>2013-03-20 何方 创建</remarks>
        public Pager(IList<T> list, int totalRows, int currentPage = 1, int pageSize = 10)
        {
            _list = list;
            _totalRows = totalRows;
            _currentPage = currentPage;
            _pageSize = pageSize;
        }

        //分页条件实体类
        public T PageFilter
        {
            get { return _pageFilter; }
            set { _pageFilter = value; }
        } 

        /// <summary>
        /// 读取设置当前页面数据集
        /// </summary>
        public IList<T> Rows
        {
            get { return _list; }
            set { _list = value; }
        }
        /// <summary>
        /// 读取设置当前页面id数据集
        /// </summary>
        public List<int> IdRows
        {
            get { return _idlist; }
            set { _idlist = value; }
        }
        /// <summary>
        /// 读取总页数
        /// </summary>
        public int TotalPages
        {
            get { return (_totalRows + _pageSize - 1) / _pageSize; }
        }

        /// <summary>
        /// 读取总记录数
        /// </summary>
        public int TotalRows
        {
            get { return _totalRows; }
            set { _totalRows = value; }
        }

        /// <summary>
        /// 读取设置每页行数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value <= 0 ? 10 : value; }
        }

        /// <summary>
        /// 读取设置当前页
        /// </summary>
        /// <remarks>
        /// 2013-06-21 郑荣华 
        /// 读取时 总页数为0直接返回当前页
        /// 总页数大于0 则（当前页大于总页数则返回第一页）
        /// </remarks>
        public int CurrentPage
        {
            get
            {
                if (TotalPages > 0) return _currentPage > TotalPages ? 1 : _currentPage;
                return _currentPage;
            }
            set { _currentPage = value < 1 ? 1 : value; }
        }

        /// <summary>
        /// 读取第一页
        /// </summary>
        public int FirstPage
        {
            get { return 1; }
        }

        /// <summary>
        /// 读取上一页
        /// </summary>
        public int PreviousPage
        {
            get { return _currentPage <= 1 ? 1 : _currentPage - 1; }
        }

        /// <summary>
        /// 读取下一页
        /// </summary>
        public int NextPage
        {
            get { return _currentPage + 1 >= TotalPages ? TotalPages : _currentPage + 1; }
        }

        /// <summary>
        /// 读取最后一页
        /// </summary>
        public int LastPage
        {
            get { return TotalPages; }
        }
    }
}
