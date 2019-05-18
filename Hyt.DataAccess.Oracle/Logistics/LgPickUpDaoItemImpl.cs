using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 取件单数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-07-12 何方 创建
    /// </remarks>
    public class LgPickUpDaoItemImpl : ILgPickUpItemDao
    {
        /// <summary>
        /// 创建取件单明细
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>返回系统编号</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public override int Create(LgPickUpItem model)
        {
            
                    var sysNo = Context.Insert<LgPickUpItem>("LgPickUpItem", model)
                   .AutoMap(x => x.SysNo)
                   .ExecuteReturnLastId<int>("SysNo");

                    return sysNo;
        }
    }
}
