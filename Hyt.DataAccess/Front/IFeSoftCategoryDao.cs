using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 软件分类抽象类
    /// </summary>
    /// <remarks>2014-01-15 唐永勤 创建</remarks>
    public abstract class IFeSoftCategoryDao : DaoBase<IFeSoftCategoryDao>
    {
        /// <summary>
        /// 添加软件分类
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract int Create(FeSoftCategory model);

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract bool IsExists(FeSoftCategory model);

        /// <summary>
        /// 获取指定软件分类信息
        /// </summary>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract FeSoftCategory GetEntity(int sysNo);

        /// <summary>
        /// 根据软件分类编号更新软件分类信息
        /// </summary>
        /// <param name="model">软件分类实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract bool Update(FeSoftCategory model);

        /// <summary>
        /// 更新软件分类状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">分类编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态 status, int sysNo);

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="pager">软件分类查询条件</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-26 唐永勤 创建</remarks>
        public abstract Pager<FeSoftCategory> GetFeSoftCategoryList(Pager<FeSoftCategory> pager);

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-03-04 苟治国 创建</remarks>
        public abstract List<FeSoftCategory> GetFeSoftCategoryList();


    }
}
