using System.Collections.Generic;
using Hyt.Model;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 仓库业务
    /// </summary>
    /// <remarks>2014-1-7 沈强 添加</remarks>
    public interface IWhWarehouseBo
    {
        #region 商品库存

        /// <summary>
        /// 更新多个商品数量
        /// </summary>
        /// <param name="stockoutSysNo">出库单系统编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-26 吴文强 创建
        /// </remarks>
        void UpdateErpProductNumber(int stockoutSysNo);

        #endregion

        #region 仓库

        /// <summary>
        /// 根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        ///  <param name="warehouseType">仓库类型</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        IList<WhWarehouse> GetWhWarehouseListByArea(int areaSysNo, int? warehouseType, int? deliveryType = null);

        /// <summary>
        /// 根据地区、仓库类型、取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="pickupType">取件方式编号</param>
        /// <returns>返回仓库列表</returns>
        /// <remarks>2013-09-13 周唐炬 创建</remarks>
        IList<WhWarehouse> GetWhWarehouseList(int areaSysNo, int? warehouseType, int pickupType);

        /// <summary>
        /// 获取所有仓库信息
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        IList<WhWarehouse> GetAllWarehouseList();

        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <remarks> 2013-06-18 朱成果 创建</remarks>
        WhWarehouse GetWarehouseEntity(int sysNo);

        /// <summary>
        /// 获取仓库名称
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>仓库名称</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        string GetWarehouseName(int sysNo);

        /// <summary>
        /// 搜索仓库
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <param name="areaNoCheck">地区节点是否可被选中</param>
        /// <param name="isRma">是否Rma仓</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <returns>ZTree列表</returns>
        /// <remarks>
        /// 2013-6-21 何方 创建
        /// 2013-06-24 周唐炬 修改查询业务,节点递归
        /// 2013-10-22 郑荣华 增加只能查询有权限的仓库
        /// 2013-10-24 黄志勇 筛选Rma仓库
        /// </remarks>
        IList<ZTreeNode> SearchWharehouseNew(string keyword, bool areaNoCheck, bool isRma,int? isSelfSupport=null);

        #endregion

        #region 用户与仓库

        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        IList<SyUser> GetWhDeliveryUser(int warehouseSysNo);

        /// <summary>
        /// 获取未录入信用信息的仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        Dictionary<int, string> GetWhDeliveryUserDictForCredit(int warehouseSysNo);

        /// <summary>
        /// 获取配送员仓库
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-08-07 周唐炬 创建</remarks>
        int GetDeliveryUserWarehouseSysNo(int deliveryUserSysNo);

        /// <summary>
        /// 获取用户有可管理的所有仓库
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>仓库集合</returns>
        /// <remarks>
        /// 2013/7/3 何方 创建
        /// </remarks>
        IList<WhWarehouse> GetUserWarehouseList(int userSysNo);

        /// <summary>
        /// 获取多个仓库的配送员
        /// </summary>
        /// <param name="warehouseSysNos">The warehouse sys nos.</param>
        /// <returns>用户集合</returns>
        /// <remarks>
        /// 2013/7/3 何方 创建
        /// </remarks>
        IList<SyUser> GetDeliveryUserList(IList<int> warehouseSysNos);

        /// <summary>
        /// 通过配送员系统编号获取借货单商品列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        IList<CBPdProductJson> GetProductLendItmeList(int deliveryUserSysNo, int customerSysNo,
                                                      Model.WorkflowStatus.WarehouseStatus.借货单状态 productLendStatus,
                                                      Model.WorkflowStatus.ProductStatus.产品价格来源 priceSource);

        /// <summary>
        /// 通过配送员系统编号获取借货单商品列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        IList<CBPdProductJson> GetProductLendItmeList(int deliveryUserSysNo, int? warehouseSysNo, Model.WorkflowStatus.WarehouseStatus.借货单状态 productLendStatus,
                                                      Model.WorkflowStatus.ProductStatus.产品价格来源 priceSource);
        #endregion
    }
}