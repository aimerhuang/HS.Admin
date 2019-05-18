using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// 码表维护
    /// </summary>
    /// <remarks>2013-10-14 唐永勤 创建</remarks>
    public abstract class IBsCodeDao : DaoBase<IBsCodeDao>
    {
        /// <summary>
        /// 添加码表记录
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>返回新建记录的编号</returns>       
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public abstract int Create(BsCode model);

        /// <summary>
        /// 获取指定编号的码表信息
        /// </summary>
        /// <param name="sysNo">码表编号</param>
        /// <returns>码表实体信息</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public abstract BsCode GetEntity(int sysNo);

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="sysNo">码表系统编号</param>
        /// <returns>影响行</returns>
        /// <remarks>2013-12-04 周唐炬 创建</remarks>
        public abstract int Remove(int sysNo);

        /// <summary>
        /// 获取父级系统编号获取码值集合
        /// </summary>
        /// <param name="parentSysNo">父级系统编号</param>
        /// <returns>取码值集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract List<BsCode> GetListByParentSysNo(int parentSysNo);

        /// <summary>
        /// 根据码表编号更新码表信息
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public abstract bool Update(BsCode model);

        /// <summary>
        /// 更新码表状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">码表编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.BasicStatus.码表状态 status, int sysNo);

        /// <summary>
        /// 获取码表列表
        /// </summary>
        /// <param name="pager">码表查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public abstract void GetBsCodeList(ref Pager<BsCode> pager);

        /// <summary>
        /// 判断重复数据--码表
        /// </summary>
        /// <param name="model">码表实体</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-10-15 唐永勤 创建</remarks>
        public abstract bool IsExists(BsCode model);

    }
}
