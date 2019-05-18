using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 软件列表抽象类
    /// </summary>
    /// <remarks>2014-01-15 唐永勤 创建</remarks>
    public abstract class IFeSoftwareListDao : DaoBase<IFeSoftwareListDao>
    {
        /// <summary>
        /// 添加软件列表
        /// </summary>
        /// <param name="model">软件列表实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract int Create(FeSoftwareList model);

        /// <summary>
        /// 获取指定编号的软件列表项信息
        /// </summary>
        /// <param name="sysNo">软件列表编号</param>
        /// <returns>软件列表实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract FeSoftwareList GetEntity(int sysNo);

        /// <summary>
        /// 批量更新软件列表
        /// </summary>
        /// <param name="softwareSysNo">软件编号</param>
        /// <param name="list">软件列表</param>
        /// <returns>是否更新成功</returns>
        /// <remarks>2014-01-20 唐永勤 创建</remarks>
        public abstract bool SaveList(int softwareSysNo, IList<FeSoftwareList> list);

        /// <summary>
        /// 根据软件编号获取软件列表
        /// </summary>
        /// <param name="softwareSysNo">软件编号</param>
        /// <returns>软件列表</returns>
        /// <remarks>2014-01-20 唐永勤 创建</remarks>
        public abstract IList<FeSoftwareList> GetListBySoftwareSysNo(int softwareSysNo);
    }
}
