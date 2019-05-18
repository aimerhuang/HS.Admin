using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Generated;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 你他购（利嘉）与信营数据绑定
    /// </summary>
    public class MKNitagoDateBindBo : BOBase<MKNitagoDateBindBo>
    {
        /// <summary>
        /// 查询，根据类型，信营绑定数据查询
        /// </summary>
        /// <param name="BindDateTepy">类型</param>
        /// <param name="XinyingDateSysNo">信营绑定数据</param>
        /// <returns></returns>
        public MKNitagoDateBind Select(int BindDateTepy, int XinyingDateSysNo)
        {
            return DataAccess.Web.IMKNitagoDateBindDao.Instance.Select(BindDateTepy, XinyingDateSysNo);
        }
        /// <summary>
        /// 插入你他购（利嘉）与信营数据绑定
        /// </summary>
        /// <param name="model">你他购，信营绑定表</param>
        /// <returns></returns>
        public int Insert(MKNitagoDateBind model)
        {
            return DataAccess.Web.IMKNitagoDateBindDao.Instance.Insert(model);
        }
    }
}
