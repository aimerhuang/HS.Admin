using Hyt.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// 单据编号
    /// </summary>
    /// <remarks>2016-1-1 杨浩 创建</remarks>
    public abstract class IReceiptNumberDao : DaoBase<IReceiptNumberDao>
    {
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="type">编号类型</param>
        /// <param name="code">编号前缀</param>
        /// <returns></returns>
        /// <remarks>2016-1-1 杨浩 创建</remarks>
        public abstract string GetNumber(int type, string code);
    }
}
