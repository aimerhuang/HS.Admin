using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 配送员定位信息抽象类
    /// </summary>
    /// <remarks>
    /// 2013-06-08 郑荣华 创建
    /// </remarks>
    public abstract class ILgDeliveryUserLocationDao : DaoBase<ILgDeliveryUserLocationDao>
    {
        #region 操作

        /// <summary>
        /// 添加配送员位置信息
        /// </summary>
        /// <param name="model">配送员位置信息实体</param>
        /// <returns>添加的配送员位置信息sysno</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public abstract int Create(LgDeliveryUserLocation model);

        #endregion

        #region 查询

        /// <summary>
        /// 获取单日内配送员位置信息
        /// </summary>
        /// <param name="deliveryUserSysno">配送员sysno</param>
        /// <param name="dateRange">时间范围</param>
        /// <returns>配送员位置信息 最大记录数24*60=1440条</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public abstract IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLocation(int deliveryUserSysno, DateRange dateRange);

        /// <summary>
        /// 获取同一仓库下多个配送员最后一次位置信息
        /// </summary>
        /// <param name="whWarehouseSysNo">仓库系统编号</param>
        /// <returns>同一仓库下多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public abstract IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(int whWarehouseSysNo);

        /// <summary>
        /// 根据配送员编号查询配送员最近一次定位信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员编号逗号分隔的字符串</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public abstract IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(string deliveryUserSysNo);

        /// <summary>
        /// 全国配送员最新位置
        /// </summary>
        /// <param name="idlist">仓库系统编号列表</param>
        /// <param name="status">状态(0-全部,1-30分钟活动,2-当日活动,3-不在线)</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>2014-03-10 周唐炬 创建</remarks>
        public abstract List<CBLgDeliveryUserLocation> AllDeliveryUserLastLocation(List<int> idlist, int status);

        /// <summary>
        /// 根据时间，仓库，查询在定位信息表中有记录的配送员
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="dateRange">日期范围</param>
        /// <returns>配送员信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public abstract IList<SyUser> GetDeliveryUserForMap(int whSysNo, DateRange dateRange);

        /// <summary>
        /// 查询配送员在某段时间配送的配送单
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="dateRange">日期时间范围</param>
        /// <returns>配送单信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public abstract IList<CBLgDeliveryInvoice> GetLgDeliveryForMap(int deliveryUserSysNo, DateRange dateRange);

        /// <summary>
        /// 获取配送人员定位信息
        /// </summary>
        /// <param name="delUserSysNo">配送人员系统编号</param>
        /// <returns>LgDeliveryUserLocation</returns>
        /// <remarks>2014-01-17 黄伟 创建</remarks>
        public abstract LgDeliveryUserLocation GetLocationByUserSysNo(int delUserSysNo);

        #endregion
    }
}
