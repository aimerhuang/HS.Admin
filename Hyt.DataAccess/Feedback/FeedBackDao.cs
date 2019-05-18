using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Feedback
{
    public abstract class FeedBackDao : DaoBase<FeedBackDao>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="State">状态</param>
        /// <param name="count">抛出总条数</param>
        /// <returns>商品信息通知列表</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract IList<FFeedBack> Seach(int State, Pager<FFeedBack> pager);
         /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public abstract int Update(string sysNoItems);
    }
}
