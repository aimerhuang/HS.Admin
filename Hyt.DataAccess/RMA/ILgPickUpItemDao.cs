using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 取件单明细
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public abstract class ILgPickUpItemDao :DaoBase<ILgPickUpItemDao>
    {
        /// <summary>
        /// 添加取件单明细
        /// </summary>
        /// <param name="item">取件单明细</param>
        /// <returns>取件单明细编号</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public abstract int Insert(LgPickUpItem item);
    }
}
