using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Generated;
using Hyt.DataAccess.Web;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 你他购（利嘉）与信营数据绑定
    /// </summary>
    public class MKNitagoDateBindDaoIml : IMKNitagoDateBindDao
    {
        /// <summary>
        /// 查询，根据类型，信营绑定数据查询
        /// </summary>
        /// <param name="BindDateTepy">类型</param>
        /// <param name="XinyingDateSysNo">信营绑定数据</param>
        /// <returns></returns>
        public override MKNitagoDateBind Select(int BindDateTepy, int XinyingDateSysNo)
        {
            string sql = @"SELECT * FROM MKNitagoDateBind WHERE BindDateTepy=@BindDateTepy AND XinyingDateSysNo=@XinyingDateSysNo";
            return Context.Sql(sql)
                .Parameter("BindDateTepy", BindDateTepy)
                .Parameter("XinyingDateSysNo", XinyingDateSysNo)
                .QuerySingle<MKNitagoDateBind>();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">你他购，信营绑定表</param>
        /// <returns></returns>
        public override int Insert(MKNitagoDateBind model)
        {
            return Context.Insert<MKNitagoDateBind>("MKNitagoDateBind", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
    }
}
