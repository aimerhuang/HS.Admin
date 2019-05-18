using Hyt.DataAccess.FreightModule;
using Hyt.Model;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.FreightModule
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>2015-9-9 杨浩 创建</remarks>
    public class FreightModuleDaoBo : BOBase<FreightModuleDaoBo>
    {
        /// <summary>
        /// 获取仓库地址所对应的运费模板(如果没有找到则返回默认模板)
        /// </summary>
        /// <param name="productAddress">仓库地址编号</param>
        /// <returns>运费模板</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public LgFreightModule GetFreightModuleByProductAddress(int addressSysNo)
        {
            IList<LgFreightModule> freightModules = IFreightModuleDao.Instance.GetFreightModuleByProductAddress(addressSysNo);
            if (freightModules.Count > 1)
                return freightModules.Single((freight) => freight.ProductAddress == addressSysNo);
            return freightModules.Single();
        }
        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <param name="productSysNoAndNumber">商品系统编号和购买数量组合（商品系统编号_购买数量,商品系统编号_购买数量...）</param>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public IList<FareTotal> GetFareTotal(int addressSysNo, int freightModuleSysNo, string productSysNoAndNumber)
        {
            return IFreightModuleDao.Instance.GetFareTotal(addressSysNo, freightModuleSysNo, productSysNoAndNumber);
        }

        /// <summary>
        /// 获取仓库物流费用
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="productSysNoAndNumbers">产品编号和数量组合的字符串(产品编号_产品数量,....,产品编号_产品数量)</param>
        /// <param name="deliverySysNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-12 杨浩 创建</remarks>
        public FareTotal GetFareTotal(int addressSysNo, int warehouseSysNo, string productSysNoAndNumbers, int deliverySysNo)
        {
            //获取仓库物流详情
            var warehouseDeliveryType = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseDeliveryType(warehouseSysNo, deliverySysNo);
            //获取指定配送方式详情
            var deliveryTypeInfo = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(deliverySysNo);

            if (warehouseDeliveryType == null || (warehouseDeliveryType != null && warehouseDeliveryType.FreightModuleSysNo == 0))
            {
                return new FareTotal() { Freigh = -1, DeliveryTypeSysNo = deliverySysNo };
            }
            else
            {
                //计算仓库物流的商品总运费
                var _fareTotal = GetFareTotal(addressSysNo, warehouseDeliveryType.FreightModuleSysNo, productSysNoAndNumbers.Trim(','));

                return _fareTotal.Select(
                        x => new FareTotal
                        {
                            Name = deliveryTypeInfo.DeliveryTypeName,
                            Freigh = x.Freigh,
                            DeliveryTypeSysNo = x.DeliveryTypeSysNo
                        }
                   ).Single();
            }
        }

        /// <summary>
        /// 获取仓库运费列表
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="productSysNoAndNumbers">产品编号和数量组合的字符串(产品编号_产品数量,....,产品编号_产品数量)</param>
        /// <returns></returns>
        /// <remarks>2015-11-12 杨浩 创建</remarks>
        public IList<FareTotal> GetWarehouseFareList(int addressSysNo, int warehouseSysNo, string productSysNoAndNumbers)
        {
            //定义运费列表
            var fareTotal = new List<FareTotal>();

            //获取仓库所有的配送方式
            List<WhWarehouseDeliveryType> warehouseDeliveryTypeList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseDeliveryTypeList(warehouseSysNo);
     

            #region 获取仓库所有配送方式的运费
            foreach (WhWarehouseDeliveryType deliveryType in warehouseDeliveryTypeList)
            {
                fareTotal.Add(GetFareTotal(addressSysNo, warehouseSysNo, productSysNoAndNumbers, deliveryType.DeliveryTypeSysNo));
            }
            #endregion

            return fareTotal;

        }

    }
}
