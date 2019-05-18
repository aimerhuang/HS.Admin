using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Memory;
using Hyt.BLL.Log;
using Hyt.Model.Map;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 百城当日达区域信息维护业务类
    /// </summary>
    /// <remarks> 
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public class LgDeliveryScopeBo : BOBase<LgDeliveryScopeBo>
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
        public int Create(LgDeliveryScope model)
        {
            var r = ILgDeliveryScopeDao.Instance.Create(model);

            if (r > 0)
                MemoryProvider.Default.Remove(string.Format(KeyConstant.LgDeliveryScopeList, model.AreaSysNo));

            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建百城当日达区域", LogStatus.系统日志目标类型.百城当日达区域, r);
            return r;
        }

        /// <summary>
        /// 更新百城当日达区域信息
        /// </summary>
        /// <param name="model">百城当日达区域信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Update(LgDeliveryScope model)
        {
            var r = ILgDeliveryScopeDao.Instance.Update(model) > 0;
            if (r)
            {
                MemoryProvider.Default.Remove(string.Format(KeyConstant.LgDeliveryScopeList, model.AreaSysNo));

                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改百城当日达区域", LogStatus.系统日志目标类型.百城当日达区域, model.SysNo);
            }

            return r;
        }

        /// <summary>
        /// 删除百城当日达区域信息
        /// </summary>
        /// <param name="sysNo">要删除的百城当日达区域信息系统编号</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Delete(int sysNo)
        {
            var r = ILgDeliveryScopeDao.Instance.Delete(sysNo) > 0;
            if (r)
            {
                //2014-05-14 朱家宏 移出缓存
                var model = ILgDeliveryScopeDao.Instance.GetDeliveryScopeBySysNo(sysNo);
                MemoryProvider.Default.Remove(string.Format(KeyConstant.LgDeliveryScopeList, model.AreaSysNo));

                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改百城当日达区域", LogStatus.系统日志目标类型.百城当日达区域, sysNo);
            }
            return r;
        }

        /// <summary>
        /// 根据城市系统编号删除百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">要删除的城市的系统编号</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool DeleteByAreaSysNo(int areaSysNo)
        {
            var r = ILgDeliveryScopeDao.Instance.DeleteByAreaSysNo(areaSysNo) > 0;
            if (r)
            {
                //2014-05-14 朱家宏 移出缓存
                MemoryProvider.Default.Remove(string.Format(KeyConstant.LgDeliveryScopeList, areaSysNo));
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除百城当日达区域（城市编号）", LogStatus.系统日志目标类型.百城当日达区域, areaSysNo);
            }

            return r;
        }

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
        public IList<LgDeliveryScope> GetDeliveryScope(int areaSysNo)
        {
            //return ILgDeliveryScopeDao.Instance.GetDeliveryScope(areaSysNo);
            //缓存时间12小时  2014-05-14 朱家宏 修改
            return MemoryProvider.Default.Get(string.Format(KeyConstant.LgDeliveryScopeList, areaSysNo), 60 * 12,
                () => ILgDeliveryScopeDao.Instance.GetDeliveryScope(areaSysNo));
        }

        #endregion

        #region

        /// <summary>
        /// 根据城市编号获取仓库信息，用于百度地图显示
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2015-08-06 LYK 创建
        /// </remarks>
        public IList<WhWarehouse> GetWhWarehouseDiTu(int areaSysNo)
        {
            //缓存时间12小时 
            return MemoryProvider.Default.Get(string.Format(KeyConstant.LgDeliveryScopeList, areaSysNo), 60 * 12,
                () => ILgDeliveryScopeDao.Instance.GetWhWarehouseDiTu(areaSysNo));
        }
        #endregion

        /// <summary>
        /// 判断是否在百城当日达区域内
        /// </summary>
        /// <param name="areaSysNo">城市系统编号</param>
        /// <param name="coordinate">地图坐标（经度x，纬度y）</param>
        /// <returns>是返回true；否返回false</returns>
        /// <remarks> 
        /// 2013-08-14 郑荣华 创建
        /// 2013-09-16 郑荣华 测试完善
        /// </remarks>
        public bool IsInScope(int areaSysNo, Coordinate coordinate)
        {
            try
            {
                var list = GetDeliveryScope(areaSysNo);
                if (list == null || list.Count == 0) return false;
                var coorlist = new List<Coordinate>();
                foreach (var positions in list.Select(item => item.MapScope.TrimEnd('\n').TrimEnd(';').Split(';')))
                {
                    coorlist.AddRange(positions.Select(i => i.Split(',')).Select(a => new Coordinate { X = double.Parse(a[0]), Y = double.Parse(a[1]) }));
                   // if (InPolygon(coorlist, coordinate)) return true;  //弧长法
                    if (InPointFeaces(coorlist, coordinate)) return true; //射线法 2013-12-17 朱成果
                    coorlist.Clear();
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否在百城当日达区域内
        /// </summary>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-12-09 杨浩 封装</remarks>
        public bool IsInScope(int receiveAddressSysNo)
        {
            var isInScope = false;
            var address = Hyt.BLL.Web.CrCustomerBo.Instance.GetCustomerReceiveAddressBySysno(receiveAddressSysNo);
            if (address != null)
            {
                isInScope = MemoryProvider.Default.Get<bool>(string.Format(KeyConstant.LongitudeAndLatitude,address.City+address.StreetAddress), () =>
                    {
                        var gercoderResult = Hyt.BLL.Map.BaiduMapAPI.Instance.Geocoder(address.StreetAddress,address.City);
                        return (gercoderResult != null && this.IsInScope(address.CitySysNo,new Coordinate {X = gercoderResult.Lng, Y = gercoderResult.Lat}));
                    });
            }
            return isInScope;
        }

        /// <summary>
        /// 判断是否在百城当日达区域内
        /// </summary>
        /// <param name="model">收货地址实体</param>
        /// <returns>true:在 false:不在</returns>
        /// <remarks>2013-12-09 黄波 封装</remarks>
        public bool IsInScope(Model.Transfer.CBCrReceiveAddress model)
        {
            if (model == null) return false;
            var gercoderResult = Hyt.BLL.Map.BaiduMapAPI.Instance.Geocoder(model.StreetAddress, model.City);
            return (gercoderResult != null && this.IsInScope(model.CitySysNo, new Coordinate { X = gercoderResult.Lng, Y = gercoderResult.Lat }));
        }

        /// <summary>
        /// 弧长法判断点是否在多边形内
        /// 第一个是若P[i]的某个坐标为0时，一律当正号处理；第二点是若被测点和多边形的顶点重合时要特殊处理。   
        /// </summary>
        /// <param name="p">多边形顶点集合，有序</param>
        /// <param name="corrdinate">要判断的点</param>
        /// <returns>是否在多边形内</returns>
        /// <remarks> 
        /// 2013-08-14 郑荣华 创建 
        /// 2013-09-16 郑荣华 测试完善
        /// </remarks>
        public bool InPolygon(IList<Coordinate> p, Coordinate corrdinate)
        {
            foreach (var item in p)
            {
                item.Y = item.Y - corrdinate.Y;
                item.X = item.X - corrdinate.X;
            }
            //移除相邻相同象限的
            for (var i = p.Count - 2; i > 0; i--)
            {
                if (p[i - 1].Quadrant == p[i].Quadrant)
                    p.RemoveAt(i);
            }

            //最后一个和第一个点连，将第一个点加到最后
            p.Add(p[0]);

            var count = 0;
            for (var i = 0; i <= p.Count - 2; i++)
            {
                if (p[i].X == 0 && p[i].Y == 0) return true;//顶点重合

                var f = p[i].X * p[i + 1].Y - p[i].Y * p[i + 1].X;
                if (f == 0) return true;//在边上,特殊情况，如边(3,0)-(-3,0)

                var t = p[i + 1].Quadrant - p[i].Quadrant;
                if (Math.Abs(t) == 2) t = f > 0 ? 2 : -2;
                if (t == -3) t = 1;//4->1（1-4=-3） =1
                if (t == 3) t = -1;//1->4（4-1=3）=-1
                count += t;

            }
            if (Math.Abs(count) == 2) return true;//在边上
            if (Math.Abs(count) == 4) return true;//在内部
            if (count == 0) return false;//在外部
            return false;
        }

        /// <summary>
        /// 射线法判断点是否在多边形内
        /// </summary>
        /// <param name="pntlist">多边形顶点集合，有序</param>
        /// <param name="pnt">要判断的点</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-12-17 朱成果 创建 
        /// </remarks>
        public bool InPointFeaces(IList<Coordinate> pntlist, Coordinate pnt)
        {
            if (pntlist == null)
            {
                return false;
            }
            int j = 0, cnt = 0;
            for (int i = 0; i < pntlist.Count; i++)
            {
                j = (i == pntlist.Count - 1) ? 0 : j + 1;
                if ((pntlist[i].Y != pntlist[j].Y) && (((pnt.Y >= pntlist[i].Y) && (pnt.Y < pntlist[j].Y)) || ((pnt.Y >= pntlist[j].Y) && (pnt.Y < pntlist[i].Y))) && (pnt.X < (pntlist[j].X - pntlist[i].X) * (pnt.Y - pntlist[i].Y) / (pntlist[j].Y - pntlist[i].Y) + pntlist[i].X)) cnt++;
            }
            return (cnt % 2 > 0) ? true : false;
        }


        /// <summary>
        /// 查询地址的百城配送信息
        /// </summary>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="address">详细地址</param>
        /// <param name="maptype">地图 百度=10 高德=20</param>
        /// <param name="isqjw">是否是千机网</param>
        /// <returns></returns>
        /// <remarks>2014-11-03 杨浩 创建</remarks>
        public MapBaiChengScope GetBaiChengInfo(int areaSysNo, string address, int maptype = 10, int isqjw = 0)
        {
            MapBaiChengScope result = null;

            try
            {
                result = new MapBaiChengScope();
                //using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Pisen.Service.EC.Core.TMS.Contract.ITMSService>())
                //{
                //    var serviceResult = service.Channel.InDeliverScope(
                //     new Pisen.Service.EC.Core.TMS.Contract.DataContract.InDeliverScopeRequest
                //     {
                //         AreaSysNo = areaSysNo,
                //         Address = address,
                //         MapType = maptype, //地图类型,百度=10 高德=20
                //         IsQJW = isqjw
                //     });

                    //if (!serviceResult.IsError)
                    //{
                        //result = new MapBaiChengScope { IsInScope = serviceResult.IsInScope, Lat = serviceResult.Lat, Lng = serviceResult.Lng, WarehouseName = serviceResult.WarehouseName, WarehouseNo = serviceResult.WarehouseNo };
                    //}
                    //else
                    //{
                       // //SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台,string.Format("{0}-{1}-{2}调用百城服务出现错误,接口名称:InDeliverScope,错误信息:{3}",areaSysNo,maptype,address,serviceResult.ErrMsg),LogStatus.系统日志目标类型.百城当日达区域, 0);
                   // }
                //}
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "查询地址的百城配送信息",
                                         LogStatus.系统日志目标类型.百城当日达区域, 0, ex);
            }

            return result;
        }

    }
}
