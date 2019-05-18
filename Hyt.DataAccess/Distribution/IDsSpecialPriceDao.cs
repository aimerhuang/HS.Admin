using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections.Generic;
namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商产品特殊价格
    /// </summary>
    /// <remarks>
    /// 2013-09-04 周瑜 创建
    /// </remarks>
    public abstract class IDsSpecialPriceDao : DaoBase<IDsSpecialPriceDao>
    {
        /// <summary>
        /// 创建分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public abstract int Create(DsSpecialPrice model);

        /// <summary>
        /// 更新分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public abstract int Update(DsSpecialPrice model);

        /// <summary>
        /// 修改特殊价格状态: 禁用/启用
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public abstract int UpdateStatus(DsSpecialPrice model);

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public abstract Pager<CBDsSpecialPrice> QuickSearch(DsSpecialPriceSearchCondition condition, int pageIndex, int pageSize);

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周瑜 创建</remarks>
        public abstract int UpdatePrice(DsSpecialPrice model);

        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱成果 创建</remarks>
        public abstract DsSpecialPrice GetEntity(int dealerSysNo, int productSysNo);

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public abstract void GetSpecialPriceProductList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition);

        /// <summary>
        /// 经销商升舱订单
        /// </summary>
        /// <returns>2017-8-21 罗熙 创建</returns>
        public abstract void GetDealerOrder(ref Pager<DsOrder> pager, ParaDsOrderFilter dsDetail);

        /// <summary>
        /// 经销商退换货订单
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dsRMADetail">2017-8-29 罗熙 创建</param>
        public abstract void GetDsRMAorder(ref Pager<DsReturn> pager, ParaDsReturnFilter dsRMADetail);
        
        /// <summary>
        /// 获取商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract string GetmallName(int sysNo);

        /// <summary>
        /// 获取分销商商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract string GetdealerName(int sysNo);

        /// <summary>
        /// 获取升舱订单号
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract int GetorderSysNo(int sysNo); 

        /// <summary>
        /// 获取分销商商城id
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract string GetdealerSysNo(int sysNo);

        /// <summary>
        /// 查看分销商详情
        /// </summary>
        /// <param name="sysNo">分销商id</param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract CBDsDealer GetDealerInfo(int sysNo);

        /// <summary>
        /// 查看经销商退换货订单信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-29 罗熙 创建</returns>
        public abstract DsReturnItem GetRMADealerInfo(int sysNo);
        
        /// <summary>
        /// 升舱订单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        public abstract List<DsOrderItem> GetDealerOrderPdInfo(int sysNo);

        /// 查看升舱订单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public abstract CBDsOrder GetUpOrderInfo(int sysNo);

        /// <summary>
        /// 获取升舱订单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2047-8-24 罗熙 创建</returns>
        public abstract DsOrder GetUpOrderModel(int sysNo);

        /// <summary>
        /// 删除特殊价格信息
        /// </summary>
        /// <param name="sysNo">特殊价格编号</param>
        /// <returns>删除特殊价格信息</returns>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);
        /// <summary>
        /// 删除分销商商品信息
        /// </summary>
        /// <param name="sysNo">特殊价格编号</param>
        /// <returns>删除特殊价格信息</returns>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public abstract int DeleteByProSysNo(int ProductSysNo);

        /// <summary>
        /// 删除分销商商品信息
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="ProductSysNo">商品系统编号</param>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public abstract int DeleteDealerByProSysNo(int DealerSysNo, int ProductSysNo);

        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public abstract DsSpecialPrice GetEntityByDPSysNo(int dealerSysNo, int productSysNo);

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public abstract Result UpdateSSPriceStatus(int status, int sysNo);

        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-12 王耀发 创建</remarks>
        public abstract Result UpdatePriceStatusByPro(int status, int productSysNo);

        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        ///  <param name="ProductSysNo">分销商编号编号</param>s
        /// <returns>更新行数</returns>
        /// <remarks>2017-9-12 罗勤尧 创建</remarks>
        public abstract Result UpdatePriceStatusByPro(int status, int productSysNo, int DealerSysNo);

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public abstract Result UpdatePriceStatus(decimal price, int status, int sysNo);

        /// <summary>
        /// 未选中时更新全部分销商商品状态
        /// </summary>
        /// <param name="DealerSysNo">分销商编号</param>
        /// <param name="status">状态</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-9-8 罗远康 创建</remarks>
        public abstract Result UpdateAllPriceStatus(int DealerSysNo, int status);
        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 王耀发 2016-1-5 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public abstract int ProCreateSpecialPrice(int DealerSysNo, int CreatedBy);

        /// <summary>
        /// 更新分销商商品价格
        /// </summary>
        /// <param name="ProductSysNos">分销商选中商品组</param>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="Percentage">修改价格百分比（传入值为除以100的值）</param>
        /// <returns>2016-09-06 罗远康 创建</returns>
        public abstract int ProUpdateSpecialPrice(string ProductSysNos, int DealerSysNo, decimal Percentage);
        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="SysNo">分销商商品系统编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-2-24 王耀发 创建</remarks>
        public abstract DsSpecialPrice GetEntityBySysNo(int SysNo);

        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 2016-1-5 杨云奕 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public abstract Result UpdatePriceStatus(decimal price, decimal shopPrice, int status, int sysNo);
        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 2016-1-5 杨云奕 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public abstract Result UpdatePriceStatus(decimal price, decimal shopPrice, decimal wholesalePrice, int status, int sysNo);

        public abstract List<DsSpecialPrice> GetAllProductDsSpecialPrice();
    }
}
