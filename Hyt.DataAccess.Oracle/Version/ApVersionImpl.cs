using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Version;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Version
{
    /// <summary>
    /// APP版本信息
    /// </summary>
    /// <remarks>2013-08-20 周唐炬 创建</remarks>
    public class ApVersionImpl : IApVersionDao
    {
        /// <summary>
        /// 获取最新APP版本信息
        /// </summary>
        /// <param name="type">App系统类型</param>
        /// <returns>最新APP版本信息</returns>
        /// <remarks>2013-08-20 周唐炬 创建</remarks>
        public override ApVersion GetApVersion(Model.B2CApp.AppEnum.AppType type)
        {
            return Context.Sql(@"SELECT *
                                    FROM (
	                                    SELECT t.*
	                                    FROM ApVersion t
	                                    WHERE t.appcode = @appcode
	                                    ORDER BY t.createddate
	                                    )
                                    WHERE rownum = 1").Parameter("appcode", (int)type).QuerySingle<ApVersion>();
        }
    }
}
