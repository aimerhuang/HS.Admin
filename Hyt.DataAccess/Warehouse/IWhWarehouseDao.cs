using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 仓库信息接口类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public abstract class IWhWarehouseDao : DaoBase<IWhWarehouseDao>
    {
        ///<summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNo">当前用户编号</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2016-05-27 杨浩 创建</remarks>
        public abstract Pager<WhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize, int? userSysNo);
        /// <summary>
        /// 获取仓库物流运费模板关联列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public abstract List<WhWarehouseDeliveryType> GetWarehouseDeliveryTypeList(int warehouseSysNo);
        /// <summary>
        /// <summary>
        /// 更新仓库配送方式关联运费模板
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public abstract int UpdateWarehouseDeliveryTypeAssoFreightModule(int warehouseSysNo, int deliveryTypeSysNo, int freightModuleSysNo);
        /// <summary>
        /// 更新仓库利嘉返回对应仓库编号
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="LiJiaStoreCode">利嘉返回对应仓库编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-25 罗勤尧 创建</remarks>
        public abstract int UpdateLiJiaStoreCode(int warehouseSysNo, string LiJiaStoreCode);
        /// <summary>
        /// 获取仓库物流运费模板关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public abstract WhWarehouseDeliveryType GetWarehouseDeliveryType(int warehouseSysNo, int deliveryTypeSysNo);
        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员列表信息</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public abstract IList<SyUser> GetWhDeliveryUser(int warehouseSysNo);

        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="isDelivery">是否允许借货</param>
        /// <returns>配送员列表信息</returns>
        /// <remarks> 
        /// 2013-07-03 沈强 创建
        /// </remarks>
        public abstract IList<SyUser> GetWhDeliveryUser(int warehouseSysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送员是否允许配送 isDelivery);

        /// <summary>
        /// 获取未录入信用信息的仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public abstract IList<SyUser> GetWhDeliveryUserForCredit(int warehouseSysNo);

        /// <summary>
        ///根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseListByArea(int areaSysNo, int? warehouseType, int? deliveryType = null);

        /// <summary>
        /// 根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns>匹配仓库数据列表</returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseListByDeliveryType(int deliveryType);
        /// <summary>
        /// 根据地区、仓库类型、取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="pickupType">取件方式编号</param>
        /// <returns>返回仓库列表</returns>
        /// <remarks>2013-09-13 周唐炬 创建</remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseList(int areaSysNo, int? warehouseType, int pickupType);

        /// <summary>
        ///根据地区信息获取仓库信息
        /// </summary>
        /// <param name="area">地区信息</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-09-11 杨晗 创建
        /// </remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseListByArea(List<int> area);

        /// <summary>
        /// 获取所有仓库信息
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        public abstract IList<WhWarehouse> GetAllWarehouseList();
        
        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>返回仓库详情</returns>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        public abstract WhWarehouse GetWarehouseEntity(int sysNo);

        public abstract string GetStockQuantity(int sysNo, int whsysNo);
        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库名称</param>
        /// <returns>返回仓库详情</returns>
        /// <remarks> 2015-12-15 王耀发 创建</remarks>
        public abstract WhWarehouse GetWarehouseByName(string BackWarehouseName);

        /// <summary>
        /// 通过仓库类型获取仓库列表
        /// </summary>
        /// <param name="warehouseType">仓库类型编号</param>
        /// <returns>仓库数据列表</returns>
        /// <remarks> 2013-06-27 余勇 创建</remarks>
        public abstract IList<WhWarehouse> GetWhWarehouseListByType(int warehouseType);

        /// <summary>
        /// 获取配送员仓库
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-08-07 周唐炬 创建</remarks>
        public abstract int GetDeliveryUserWarehouseSysNo(int deliveryUserSysNo);

        /// <summary>
        /// 获取用户有可管理的所有仓库
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>仓库集合</returns>
        /// <remarks>
        /// 2013/7/4 周唐炬 创建
        /// </remarks>
        public abstract IList<WhWarehouse> GetUserWarehuoseList(int userSysNo);

        /// <summary>
        /// 获取多个仓库的配送员
        /// </summary>
        /// <param name="warehouseSysNos">The warehouse sys nos.</param>
        /// <returns>用户集合</returns>
        /// <remarks>
        /// 2013/7/4 周唐炬 创建
        /// </remarks>
        public abstract IList<SyUser> GetDeliveryUserList(IList<int> warehouseSysNos);

        /// <summary>
        /// 搜索仓库
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-21 何方 创建
        /// </remarks>
        //public abstract IList<ZTreeNode> SearchWharehouse(string keyword);

        #region 仓库快递方式维护

         /// <summary>
        /// 获取所有的仓库快递方式
        /// </summary>
        /// <returns></returns>
        /// <remarks> 
        /// 2014-05-14 朱成果 创建
        /// </remarks>
        public abstract List<WhWarehouseDeliveryType> GetWhWarehouseDeliveryTypeList();

        public abstract List<WhWarehouse> GetWhWarehouseList();

        /// <summary>
        /// 添加仓库快递方式
        /// </summary>
        /// <param name="model">仓库快递方式实体</param>        
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public abstract int CreateWareHouseDeliveryType(WhWarehouseDeliveryType model);

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库快递方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public abstract int DeleteWareHouseDeliveryType(int sysNo);

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">快递方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public abstract int DeleteWareHouseDeliveryType(int whSysNo, int deliveryTypeSysNo);

        /// <summary>
        /// 获取仓库快递方式列表信息
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>仓库快递方式列表信息</returns>
        /// <remarks>
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public abstract IList<CBWhWarehouseDeliveryType> GetLgDeliveryType(ParaWhDeliveryTypeFilter filter);

        #endregion
        #region 仓库取件方式
        /// <summary>
        /// 仓库取件方式
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <returns>取件方式列表</returns>
        /// <remarks>  2013-07-11 朱成果 创建</remarks>
        public abstract IList<LgPickupType> GetPickupTypeListByWarehouse(int WarehouseSysNo);

        /// <summary>
        /// 添加仓库取件方式
        /// </summary>
        /// <param name="model">仓库取件方式实体</param>        
        /// <returns>添加后的系统编号</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public abstract int CreateWareHousePickUpType(WhWarehousePickupType model);

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库取件方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public abstract int DeleteWareHousePickUpType(int sysNo);

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="pickUpTypeSysNo">取件方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public abstract int DeleteWareHousePickUpType(int whSysNo, int pickUpTypeSysNo);
        #endregion

        /// <summary>
        /// 获取借货单明细中的商品
        /// </summary>
        /// <param name="deliverymanSysNo">配送员系统编号</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <returns>借货单中的商品</returns>
        /// <remarks>2013-07-11 沈强 创建</remarks>
        public abstract IList<Model.CBPdProductJson> GetProductLendGoods(int deliverymanSysNo, int? userGrade);

        /// <summary>
        /// 获取借货单明细中的商品
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>借货单中的商品</returns>
        /// <remarks>2013-07-18 沈强 创建</remarks>
        public abstract IList<Model.CBPdProductJson> GetProductLendGoods(int deliveryUserSysNo, int? userGrade, int? warehouseSysNo,
                                                                         WarehouseStatus.借货单状态 productLendStatus,
                                                                         ProductStatus.产品价格来源 priceSource);

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract Pager<CBWhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize);
        
        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract int Insert(WhWarehouse warehouse);
        
        /// <summary>
        /// 修改仓库信息
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract int Update(WhWarehouse warehouse);
        
        /// <summary>
        /// 修改仓库状态
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract int UpdateStatus(WhWarehouse warehouse);
        
        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <remarks> 2013-08-07 周瑜 创建</remarks>
        public abstract CBWhWarehouse GetWarehouse(int sysNo);

        /// <summary>
        /// 查询每个地区下的所有仓库
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        /// <remarks>2013-08-14 周瑜 创建</remarks>
        public abstract Pager<CBAreaWarehouse> GetWarehouseForArea(int[] areaSysNo, int pageIndex, int pageSize);

        /// <summary>
        /// 获取支持该服务地区的对应的仓库
        /// </summary>
        /// <param name="supportArea">支持地区</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <returns></returns>
        /// <remarks>2013-08-28 朱成果 创建</remarks>
        public abstract List<WhWarehouse> GetWhWarehouseBySupportArea(int supportArea, int? warehouseType);

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <param name="maxLatAngle">纬度相差最大角度</param>
        /// <param name="maxLngAngle">经度相差最大角度</param>
        /// <returns>门店列表</returns>
        /// <remarks>
        /// 2013-09-11 郑荣华 创建
        /// </remarks>
        public abstract IList<CBWhWarehouse> GetWarehouseByMap(double lat, double lng, double maxLatAngle,
                                                             double maxLngAngle);

        /// <summary>
        /// 根据服务地区,仓库类型,支持的物流类型获取仓库
        /// </summary>
        /// <param name="supportArea">服务覆盖地区.</param>
        /// <param name="warehouseType">类型,门店仓库.</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public abstract List<WhWarehouse> GetWhWareHouse(int? supportArea = null, WarehouseStatus.仓库类型? warehouseType = null, int? deliveryType = null, WarehouseStatus.仓库状态? status = null);

        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="productSysNos">产品系统编号数组</param>
        /// <returns>返回入库单筛选字段集合</returns>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public abstract IList<ParaProductSearchFilter> ProductSelector(List<int> productSysNos);

        /// <summary>
        /// 获取仓库by erpcode
        /// </summary>
        /// <param name="erpCode">erpCode</param>
        /// <returns>WhWarehouse</returns>
        /// <remarks>2013-11-13 huangwei 创建</remarks>
        public abstract WhWarehouse GetWhWareHouseByErpCode(string erpCode);

       
        /// <summary>
        /// 查询第三方配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        public abstract Pager<CBWhStockOut> SearchThirdPartyStockOut(int stockOutStatus, int userSysNo, bool isHasAllWarehouse, int pageIndex, int pageSize, int orderSysNo, int warehouseSysNo, string sort, string sortBy);

        /// <summary>
        /// 查询配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-09-24 朱成果 创建</remarks>
        public abstract Pager<CBWhStockOut> SearchDRDStockOut(int stockOutStatus, int userSysNo, bool isHasAllWarehouse, int pageIndex, int pageSize);

        /// <summary>
        /// 检查仓库名称是否存在
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2015-8-8 陈海裕 创建
        /// </remarks>
        public abstract bool CheckWarehouseName(WarehouseSearchCondition condition);

        /// <summary>
        /// 获取对应仓库列表
        /// 王耀发 2016-1-23 创建
        /// </summary>
        /// <returns></returns>
        public abstract List<WhWarehouse> GetWhWareHouseList();
         /// <summary>
        /// 是否关联过仓库
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public abstract int ExitWarehouse(int DealerSysNo);

        public abstract List<WhWarehouse> GetAllWarehouseListBySysNos(string sysNos);
    }
}
