using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 取件单抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-05 郑荣华 创建
    /// </remarks>
    public abstract class ILgPickUpItemDao : DaoBase<ILgPickUpItemDao>
    {
        
        /// <summary>创建取件单明细
        /// </summary>
        /// <param name="model">实体.</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public abstract  int Create(LgPickUpItem model);

        

    }
}
