using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Logistics
{

    /// <summary>
    /// 百城当日达区域信息抽象类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public abstract class ILgDeliveryScopeDao : DaoBase<ILgDeliveryScopeDao>
    {
        #region 操作

        /// <summary>
        /// 创建百城当日达区域信息
        /// </summary>
        /// <param name="model">百城当日达区域信息实体</param>
        /// <returns>创建的百城当日达区域信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Create(LgDeliveryScope model);

        /// <summary>
        /// 更新百城当日达区域信息
        /// </summary>
        /// <param name="model">百城当日达区域信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Update(LgDeliveryScope model);

        /// <summary>
        /// 删除百城当日达区域信息
        /// </summary>
        /// <param name="sysNo">要删除的百城当日达区域信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 根据城市系统编号删除百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">要删除的城市的系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int DeleteByAreaSysNo(int areaSysNo);

        #endregion

        #region 查询

        /// <summary>
        /// 根据城市编号获取百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract IList<LgDeliveryScope> GetDeliveryScope(int areaSysNo);

        
        /// <summary>
        /// 根据城市编号获取仓库信息，用于百度地图显示
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2015-08-06 LYK 创建
        /// </remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseDiTu(int areaSysNo);

        /// <summary>
        /// 根据系统编号获取百城当日达区域信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>实体</returns>
        /// <remarks>
        /// 2014-05-14 朱家宏 创建
        /// </remarks>
        public abstract LgDeliveryScope GetDeliveryScopeBySysNo(int sysNo);

        #endregion

    }

}