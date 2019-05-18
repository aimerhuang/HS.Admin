using Hyt.DataAccess.Logistics;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 仓库当日达
    /// </summary>
    /// <remarks>2014-10-09 朱成果 创建</remarks>
    public class WhDeliveryScopeBo : BOBase<WhDeliveryScopeBo>
    {
        /// <summary>
        /// 根据仓库编号删除数据
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public  void DeleteByWarehouseSysNo(int warehouseSysNo)
        {
            MemoryProvider.Default.Remove(KeyConstant.ALLWhDeliveryScope);//清除缓存
            IWhDeliveryScopeDao.Instance.DeleteByWarehouseSysNo(warehouseSysNo);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public  int Insert(WhDeliveryScope entity)
        {
            MemoryProvider.Default.Remove(KeyConstant.ALLWhDeliveryScope);//清除缓存
            return IWhDeliveryScopeDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 获取所有仓库配送范围
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public List<CBWhDeliveryScope> GetList()
        {
            var list = MemoryProvider.Default.Get(KeyConstant.ALLWhDeliveryScope, () => IWhDeliveryScopeDao.Instance.GetList());
            return list;
        }

        /// <summary>
        /// 根据条件筛选仓库来设置配送范围
        /// </summary>
        /// <param name="cityNo">所在城市编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <param name="deliveryTypeSysNo">配送方式</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public  List<WhWarehouse> GetWhWarehouseForDeliveryScope(int cityNo, int? warehouseType, int? isSelfSupport, int? deliveryTypeSysNo)
        {
            return IWhDeliveryScopeDao.Instance.GetWhWarehouseForDeliveryScope(cityNo, warehouseType, isSelfSupport, deliveryTypeSysNo);
        }

        /// <summary>
        /// 根据城市获取仓库配送范围
        /// </summary>
        /// <param name="cityNo">城市编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public List<CBWhDeliveryScope> GetListByCity(int cityNo)
        {
            var lst = GetList().Where(m => m.CityNo == cityNo).ToList();
            return lst;
        }
        /// <summary>
        /// 判断坐标是否在加盟商和百城的配送范围内
        /// </summary>
        /// <param name="cityNo">城市编号</param>
        /// <param name="x">经度</param>
        /// <param name="y">维度</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public Result<WhWarehouse> IsInScope(int cityNo,double x,double y)
        {
            Coordinate coordinate = new Coordinate() { X = x, Y = y };
            Result<WhWarehouse> result = new Result<WhWarehouse>(){ Status=false};
            var lst = GetListByCity(cityNo);
            if(lst!=null)
            {
                var coorlist = new List<Coordinate>();
                foreach(var item in lst)
                {
                    var positions = item.MapScope.TrimEnd('\n').TrimEnd(';').Split(';');
                    if (positions != null && positions.Length > 0)
                    {
                        coorlist.AddRange(positions.Select(i => i.Split(',')).Select(a => new Coordinate { X = double.Parse(a[0]), Y = double.Parse(a[1]) }));
                    }
                    if (LgDeliveryScopeBo.Instance.InPointFeaces(coorlist, coordinate))//如果在范围内
                    {
                        result.Status = true;
                        result.Data = new WhWarehouse() { SysNo = item.WarehouseSysNo, WarehouseName = item.WarehouseName };
                        break;
                    }
                    coorlist.Clear();
                }
            }
            if (!result.Status)//不再加盟商配送范围内
            {
                result.Status = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(cityNo, coordinate);//百城当日达
            }
            return result;
        }
    }
}
