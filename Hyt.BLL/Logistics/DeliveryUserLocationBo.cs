using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.DataAccess.Logistics;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 配送员位置信息业务类
    /// </summary>
    /// <remarks>
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class DeliveryUserLocationBo : BOBase<DeliveryUserLocationBo>
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
        public int Create(LgDeliveryUserLocation model)
        {
            return ILgDeliveryUserLocationDao.Instance.Create(model);
        }

        #endregion

        #region 查询

        #region 业务员定位

        /// <summary>
        /// 获取仓库下多个配送员最近一次位置信息
        /// </summary>
        /// <param name="whWarehouseSysNo">仓库系统编号</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(int whWarehouseSysNo)
        {
            return ILgDeliveryUserLocationDao.Instance.GetLgDeliveryUserLastLocation(whWarehouseSysNo);
        }

        /// <summary>
        /// 根据配送员编号查询配送员最近一次定位信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员编号逗号分隔的字符串</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public IList<CBLgDeliveryUserLocation> GetLgDeliveryUserLastLocation(string deliveryUserSysNo)
        {
            return ILgDeliveryUserLocationDao.Instance.GetLgDeliveryUserLastLocation(deliveryUserSysNo);
        }

        /// <summary>
        /// 全国配送员最新位置
        /// </summary>
        /// <param name="idlist">仓库系统编号列表</param>
        /// <param name="status">状态(0-全部,1-30分钟活动,2-当日活动,3-不在线)</param>
        /// <returns>多个配送员最后一次位置信息</returns>
        /// <remarks>2014-03-10 周唐炬 创建</remarks>
        public List<CBLgDeliveryUserLocation> AllDeliveryUserLastLocation(List<int> idlist, int status)
        {
            return ILgDeliveryUserLocationDao.Instance.AllDeliveryUserLastLocation(idlist,status);
        }
        #endregion

        /// <summary>
        /// 获取配送人员定位信息
        /// </summary>
        /// <param name="delUserSysNo">配送人员系统编号</param>
        /// <returns>LgDeliveryUserLocation</returns>
        /// <remarks>2014-01-17 黄伟 创建</remarks>
        public LgDeliveryUserLocation GetLocationByUserSysNo(int delUserSysNo)
        {
            return ILgDeliveryUserLocationDao.Instance.GetLocationByUserSysNo(delUserSysNo);
        }

        #region 配送路径查询

        /// <summary>
        /// 获取某段时间内配送员位置信息
        /// </summary>
        /// <param name="deliveryUserSysno">配送员sysno</param>
        /// <param name="dateRange">时间范围</param>
        /// <returns>配送员位置信息 最大记录数24*60=1440条</returns>
        /// <remarks>
        /// 2013-06-08 郑荣华 创建
        /// </remarks>
        public IList<CBLgDeliveryUserLocation> GetLogisticsDeliveryUserLocation(int deliveryUserSysno,
                                                                                DateRange dateRange)
        {
            return ILgDeliveryUserLocationDao.Instance.GetLgDeliveryUserLocation(deliveryUserSysno, dateRange);
        }

        /// <summary>
        /// 根据时间，仓库，查询在定位信息表中有记录的配送员
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="dateRange">日期范围</param>
        /// <returns>配送员信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public Dictionary<int, string> GetDeliveryUserForMap(int whSysNo, DateRange dateRange)
        {
            var dic = new Dictionary<int, string>();
            //var list = ILgDeliveryUserLocationDao.Instance.GetDeliveryUserForMap(whSysNo, dateRange);
            IList<SyUser> deliverymans = BLL.Warehouse.WhWarehouseBo.Instance
                    .GetWhDeliveryUser(whSysNo, Model.WorkflowStatus.LogisticsStatus.配送员是否允许配送.是);

            var list = deliverymans.Where(d => d.Status == (int)Model.WorkflowStatus.SystemStatus.系统用户状态.启用).ToList();

            list.ForEach(p => dic.Add(p.SysNo, p.UserName));
            return dic;
        }

        /// <summary>
        /// 查询配送员在某段时间配送的配送单
        /// </summary>
        /// <param name="deliveryUserSysno">配送员系统编号</param>
        /// <param name="dateRange">日期时间范围</param>
        /// <returns>配送单信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        public IList<CBLgDeliveryInvoice> GetLgDeliveryForMap(int deliveryUserSysno, DateRange dateRange)
        {
            return ILgDeliveryUserLocationDao.Instance.GetLgDeliveryForMap(deliveryUserSysno, dateRange);
        }

        #endregion

        #endregion

    }
}
