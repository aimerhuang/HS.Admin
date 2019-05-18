using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Web
{
        /// <summary>
    /// 你他购（利嘉）与信营数据绑定
    /// </summary>
    public abstract class IMKNitagoDateBindDao : DaoBase<IMKNitagoDateBindDao>
    {
        /// <summary>
        /// 查询，根据类型，信营绑定数据查询
        /// </summary>
        /// <param name="BindDateTepy">类型</param>
        /// <param name="XinyingDateSysNo">信营绑定数据</param>
        /// <returns></returns>
        public abstract MKNitagoDateBind Select(int BindDateTepy, int XinyingDateSysNo);
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">你他购，信营绑定表</param>
        /// <returns></returns>
        public abstract int Insert(MKNitagoDateBind model);
    }
}
