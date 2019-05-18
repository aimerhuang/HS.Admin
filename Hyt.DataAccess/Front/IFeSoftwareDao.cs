using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 软件下载抽象类
    /// </summary>
    /// <remarks>2014-01-15 唐永勤 创建</remarks>
    public abstract class IFeSoftwareDao : DaoBase<IFeSoftwareDao>
    {
        /// <summary>
        /// 添加软件
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract int Create(FeSoftware model);

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract bool IsExists(FeSoftware model);

        /// <summary>
        /// 获取指定编号的软件信息
        /// </summary>
        /// <param name="sysNo">软件编号</param>
        /// <returns>软件实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract FeSoftware GetEntity(int sysNo);

        /// <summary>
        /// 根据软件编号更新软件信息
        /// </summary>
        /// <param name="model">软件实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract bool Update(FeSoftware model);

        /// <summary>
        /// 更新软件状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">软件编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态 status, int sysNo);

        /// <summary>
        /// 获取软件下载列表
        /// </summary>
        /// <param name="pager">软件下载列表查询条件</param>
        /// <returns>软件下载列表</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        public abstract Pager<FeSoftware> GetList(Pager<FeSoftware> pager);
    }
}
