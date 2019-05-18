using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Generated;
        
namespace Hyt.DataAccess.Atten
{
    public abstract class ClockTypeDao : DaoBase<ClockTypeDao>
    {
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract ASClockType Select(int sysNo);
        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract IList<ASClockType> SelectAll();
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <param name="keyword">名称</param>
        /// <returns>分页</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract Pager<ASClockType> SelectAll(int currentPage, int pageSize, int? status, string keyword);
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract int Insert(ASClockType model);
        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract bool Update(ASClockType model);
        /// <summary>
        /// 通过编号
        /// </summary>
        /// <param name="ClockTypeSysNo">编号</param>
        /// <returns>列表</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public abstract IList<ASClockType> SelectAllByClockTypeSysNo(int ClockTypeSysNo);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
