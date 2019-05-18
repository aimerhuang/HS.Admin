using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.LogisApp;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Data;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品基础信息
    /// </summary>
    /// <remarks>2013-06-25 黄波 创建</remarks>
    public abstract class IPdProductDao : DaoBase<IPdProductDao>
    {
        /// <summary>
        /// 通过商品条形码获取商品系统编号
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns>商品系统编号</returns>
        /// <remarks>2016-05-25 杨浩 创建</remarks>
        public abstract int GetProductSysNoByBarCode(string barCode);
        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public abstract int Create(PdProduct model);

        /// <summary>
        /// 同步创建商品信息到B2B平台
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>
        /// 2017-10-11 罗勤瑶 创建
        /// </remarks>
        public abstract int CreateToB2B(PdProduct model);

        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public abstract bool Update(PdProduct model);

        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public abstract CBPdProduct GetProduct(int productSysNo);

        /// <summary>
        /// 根据条件获取商品列表
        /// </summary>
        /// <param name="filter">搜索条件</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public abstract IList<CBPdProduct> GetProducts(ParaProductFilter filter);

        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="pager">分页查询参数对象</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 邵斌 创建</remarks>
        public abstract void ProductSelectorProductSearch(ref Pager<ParaProductSearchFilter> pager);
        /// <summary>
        /// 选择经销商对应的商品
        /// 2015-12-25 
        /// 王耀发 创建
        /// </summary>
        /// <param name="pager"></param>
        public abstract void DealerProductSearch(ref Pager<ParaProductSearchFilter> pager);

        /// <summary>
        /// 选择属性关联商品查询
        /// </summary>
        /// <param name="pager">查询条件：必须含有商品属性</param>
        /// <returns>返回可用的商品系统编号列表</returns>
        /// <remarks>2013-07-22 邵斌  实现功能</remarks>
        public abstract void SearchAttributeAssociationProduct(ref Pager<ParaProductSearchFilter> pager);

        ///// <summary>
        ///// 获取已经选择商品的详细信息
        ///// </summary>
        ///// <param name="productList">商品列表</param>
        ///// <returns>返回 商品详细信息，包括所有价格</returns>
        ///// <remarks>2013-07-11 邵斌  实现功能</remarks>
        //public abstract IList<CBPdProduct> GetSelectedProductInfo(IList<int> productList);

        /// <summary>
        /// 获取已经选择的商品列表
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        public abstract IList<ParaProductSearchFilter> GetSelectedProductList(IList<int> productList);

        /// <summary>
        /// 获取商品主表时间戳
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回时间戳</returns>
        /// <remarks>2013-10-21 邵斌 创建</remarks>
        public abstract DateTime GetPdProductStamp(int productSysNo);

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public abstract void GetPdProductDetailList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition);

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public abstract Pager<PdProduct> GetPdProductList(Pager<PdProduct> pager);

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public abstract Pager<CBPdProduct> GetCBPdProductList(Pager<CBPdProduct> pager);

        /// <summary>
        /// 获取分销商可添加的商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public abstract Pager<CBPdProduct> GetDealerMallProductList(Pager<CBPdProduct> pager, int dealerMallSysNo);

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-07-16 唐永勤 创建</remarks>
        public abstract Result UpdateStatus(int status, int sysNo);

        /// <summary>
        /// 检查商品编号是否重复
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>如果是修改商品时检查必须设置原商品系统编号</remarks>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public abstract bool CheckERPCode(string erpCode, int sourceProductSysNo = 0);

        /// <summary>
        /// 检查商品编号是否重复
        /// </summary>
        /// <param name="qrCode">二维码编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public abstract bool CheckQRCode(string qrCode, int sourceProductSysNo = 0);

        /// <summary>
        /// 检查条行码是否重复
        /// </summary>
        /// <param name="barcode">条行码</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public abstract bool CheckBarCode(string barcode, int sourceProductSysNo = 0);

        /// <summary>
        /// 更新商品描述文档
        /// </summary>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns></returns>
        /// <remarks>2013-07-25 邵斌 创建</remarks>
        public abstract bool UpdateProductDescription(int productSysNo, string html, string phoneHtml = "");

        /// <summary>
        /// 更新B2B商品描述文档
        /// </summary>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns></returns>
        ///  <remarks>2017-10-12 罗勤瑶 创建</remarks>
        public abstract bool UpdateB2BProductDescription(int productSysNo, string html, string phoneHtml = "");

        /// <summary>
        /// 获取产品后台显示名称
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>产品Eas名称</returns>
        /// <remarks>2013-12-16 何永东 创建</remarks>
        public abstract string GetProductEasName(int productSysNo);

        /// <summary>
        /// 获取产品Erp编码
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>产品商品编号</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public abstract string GetProductErpCode(int productSysNo);
        /// <summary>
        /// 获取条形码Barcode
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>Barcode</returns>
        /// <remarks>2016-09-27 罗远康 创建</remarks>
        public abstract string GetProductBarcode(int productSysNo);

        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>返回商品信息和指定的会员等级价格</returns>
        /// <remarks>2013-07-30 邵斌 创建</remarks>
        /// <remarks>2013-08-01 周唐炬 加入分页 关键字查询</remarks>
        public abstract Pager<CBProductListItem> GetProductListAndPartPrice(int categorySysNo, int customerLevelSysNo, string keyword, int currentPageIndex, int pageSize);
 
        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        public abstract IList<PdProductIndex> GetAllProduct(List<int> paroductSysNos);
        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        public abstract IList<PdProductIndex> GetAllProduct(int paroductSysNo = -1);
        /// <summary>
        /// 获取分销商所有商品信息
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-01-04 杨浩 创建</remarks>
        public abstract DataTable GetDealerAllProductToDataTable(int dealerSysNo);

        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-02 杨浩 创建</remarks>
        public abstract DataTable GetAllProductToDataTable();

        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格（物流App使用）
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>返回物流APP商品信息和指定的会员等级价格</returns>
        /// <remarks>2014-06-05 余勇 创建</remarks>
        public abstract Pager<AppProduct> GetAppProductListAndPartPrice(int categorySysNo, int customerLevelSysNo, string keyword,
            int currentPageIndex, int pageSize);

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品统计信息</returns>
        /// <remarks>2013-08-26 郑荣华 创建</remarks>
        public abstract PdProductStatistics GetPdProductStatistics(int pdSysNo);

        /// <summary>
        /// 获取商品详情包括商品类型、价格等
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>返回商品详情</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public abstract ParaProductSearchFilter GetPdProductBySysNo(int pdSysNo);

        /// <summary>
        /// 通过商品编号获取商品信息
        /// </summary>
        /// <param name="sysNo">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public abstract PdProduct GetProductBySysNo(int sysNo);

        /// <summary>
        /// 获取商品会员等级价格，若无会员等级则取基础价格
        /// </summary>
        /// <param name="customerLevelSysNo">会员等级</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-22 余勇 创建</remarks>
        public abstract CBPdProductDetail SelectProductPrice(int customerLevelSysNo, int productSysNo);

        /// <summary>
        /// 获取当前商品集合中上架商品系统编号
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品</param>
        /// <returns>上架商品系统编号</returns>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public abstract IList<int> GetOnlineProduct(int[] productSysNo, bool isFrontProduct = true);
        /// <summary>
        /// 获取当前商品集合
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public abstract IList<int> GetOnlineProduct(int[] productSysNo);

        /// <summary>
        /// 通过商品编号获取商品信息
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public abstract PdProduct GetProductByErpCode(string erpCode);

        /// <summary>
        /// 通过商品编号获取b2b商品信息
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <returns>商品信息</returns>
        /// <returns>罗勤瑶 2017-10-11</returns>
        public abstract PdProduct GetB2BProductByErpCode(string erpCode);

        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <returns>商品信息集合</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract IList<PdProduct> GetAllPdProduct();

        /// <summary>
        /// 新增商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void CreatePdProduct(List<PdProductList> models);

        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdatePdProduct(List<PdProductList> models);

        /// <summary>
        /// 更新EXCEL中导入的商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelPdProduct(List<PdProductList> models);
         /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelProductByYD(List<PdProductList> models, IList<PdProduct> productList);
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelProductByYS(List<PdProductList> models, IList<PdProduct> productList);
        /// <summary>
        /// 更新EXCEL中导入的商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <param name="productList">系统已有产品列表</param>
        /// <returns></returns>
        /// <remarks>2016-09-23 杨浩 创建</remarks>
        public abstract void UpdateExcelProduct(List<PdProductList> models,IList<PdProduct> productList);

        #region 信营导入
          /// <summary>
        /// 新增商品信息（信营）
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        public abstract void CreateXinYingPdProduct(List<PdProductList> models);
        /// <summary>
        /// 更新商品信息（信营）
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        public abstract void UpdateXinYingExcelPdProduct(List<PdProductList> models);
        #endregion 

        /// <summary>
        /// 更新商品前台显示字段
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="IsFrontDisplay">前台显示字段</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2015-12-24 王耀发 创建</remarks>
        public abstract bool UpdateProductIsFrontDisplay(int productSysNo, int IsFrontDisplay);

        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputPdProducts> GetExportProductList(List<int> sysNos, ParaProductFilter productDetail);
        /// <summary>
        /// 查询导出商品列表利嘉模板
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputPdProductsLijia> GetExportProductListLiJia(List<int> sysNos, ParaProductFilter productDetail);
        /// <summary>
        /// 获取导出商品信息
        /// </summary>
        /// <param name="sysNos">商品系统编号集合</param>
        /// <param name="productDetail">查询条件</param>
        /// <returns></returns>
        /// <remarks>2016-11-28 杨浩 创建</remarks>
        public abstract System.Data.DataTable GetExportProductToDataTable(List<int> sysNos, ParaProductFilter productDetail);
         /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputPdProductsByYD> GetExportProductListByYD(List<int> sysNos, ParaProductFilter productDetail);
        /// <summary>
        /// 查询导出商品列表(信营)
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        public abstract List<CBXinyingOutputPdProducts> GetXinYingExportProductList(List<int> sysNos, ParaProductFilter productDetail);

        /// <summary>
        /// 商品同步信息
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns>2017 010 10 罗勤瑶</returns>
        public abstract List<CBXinyingSynPdProductsB2B> GetXinYingSynProductList(List<int> sysNos, ParaProductFilter productDetail);

        /// <summary>
        /// 查询导出商品列表(信营)
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        public abstract List<CBOutputPdProductsLijia> GetXinYingExportProductListLiJia(List<int> sysNos, ParaProductFilter productDetail);

        /// <summary>
        /// 查询导出商品列表（无净重，商品简介）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <param name="productDetail"></param>
        /// <returns></returns>
        public abstract List<CBOutputPdProductsExcel> GetExportProductListExcel(List<int> sysNos, ParaProductFilter productDetail = null);

        /// <summary>
        /// 获取指定条形码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <param name="Barcode">条形码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public abstract PdProduct GetEntityByBarcode(string ErpCode, string Barcode);
        /// <summary>
        /// 获取指定条形码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <param name="Barcode">条形码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public abstract PdProduct GetEntityByBarcode(string Barcode);
         /// <summary>
        /// 获取指定商品编码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public abstract PdProduct GetEntityByErpCode(string ErpCode);

        /// <summary>
        /// 获取指定商品编码的商品信息利嘉版
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2017-05-28 罗勤尧 创建</remarks>
        public abstract CBOutputPdProductsLijia GetEntityLiJiaByErpCode(string ErpCode);
        /// <summary>
        /// 获取南沙商品备案信息
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回备案信息</returns>
        /// <remarks>2016-4-4 王耀发  实现功能</remarks>
        public abstract IList<IcpGZNanShaGoodsInfo> GetIcpGZNanShaGoodsInfoList(IList<int> productList);

        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2016-04-25 王耀发 创建</remarks>
        public abstract int CreateProduct(PdProduct model);

        /// <summary>
        /// 通过商品编号集合获取商品数据详情
        /// </summary>
        /// <param name="proIdList">商品编号集合</param>
        /// <returns></returns>
        public abstract IList<CBPdProduct> GetProductInfoList(IList<int> proIdList);

        /// <summary>
        /// 获取商品所属分类
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        public abstract IList<int> GetProductsCategories(IList<int> productSysNoList);
        /// <summary>
        /// 通过条码获取商品实体
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        /// <remarks> 杨云奕 创建</remarks>
        public abstract PdProduct GetProductByBarcode(string barcode);

       /// <summary>
       /// 更新商品档案中的时间和用户
       /// </summary>
       /// <param name="productSysNo">商品编号</param>
       /// <param name="dateTime">更新时间</param>
       /// <param name="UserSysNo">更新人员</param>
       /// <remarks>2016-06-16 杨云奕 创建</remarks>
        public abstract void UpdateLastTimeOrUser(int productSysNo, DateTime dateTime, int UserSysNo);
         /// <summary>
        /// 分页查询商品条码列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-08-31 周 创建</remarks>
        public abstract Pager<PdProductBarcode> GetPdProductBarcodeList(Pager<PdProductBarcode> pager);
         /// <summary>
        /// 查询是否重复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool IsExistsProductBarcode(PdProductBarcode model);
         /// <summary>
        /// 更新商品条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool UpdateProductBarcode(PdProductBarcode model);
         /// <summary>
        /// 创建商品条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreateProductBarcode(PdProductBarcode model);
         /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract PdProductBarcode GetProductBarcodeEntity(int sysNo);
        /// <summary>
        /// 条码在商品列表中是否存在
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public abstract int IsExistsPdProductBarcode(string Barcode);
         /// <summary>
        /// 分页查询条形码列表，商品已存在条码不显示
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public abstract Pager<PdProductBarcode> BarcodeQuery(string keyword, int currentPage, int pageSize);

        /// <summary>
        /// 通过条码获取条形码详情
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>商品实体</returns>
        public abstract PdProductBarcode GetProductBarcodeByBarcode(string barcode);
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        public abstract Pager<PdProduct> ProductListQuery(string keyword, int currentPage, int pageSize);

        #region 供应链商品数据
        /// <summary>
        /// 添加供应链商品数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreatePdProductForSupplyChain(PdProductForSupplyChain model);
          /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public abstract PdProductForSupplyChain GetPdProductForSupplyChainEntity(int SupplyChainCode);
          /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool UpdatePdProductForSupplyChain(PdProductForSupplyChain model);
        #endregion

        /// <summary>
        /// 批量更新产品状态
        /// </summary>
        /// <param name="productSysNoList">产品系统编号列表</param>
        /// <param name="status">产品状态</param>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-10-09 杨浩 创建</remarks>
        public abstract int BatchUpdateProductStatus(string productSysNoList, int status, int dealerSysNo);

        public abstract List<PdProduct> GetAllProductDataBase();
        /// <summary>
        /// 根据产品系统编号列列表获取产品
        /// </summary>
        /// <param name="productSysnoList">产品系统编号列表</param>
        /// <returns></returns>
        /// <remarks>2017-06-30 杨浩 创建</remarks>
        public abstract IList<PdProduct> GetProductListBySysnoList(List<int> productSysnoList);

        /// <summary>
        /// 根据ErpCode获取产品列表
        /// </summary>
        /// <param name="productErpCodes">产品编码集合</param>
        /// <returns></returns>
        /// <remarks>2017-10-18 杨浩 创建</remarks>
        public abstract IList<PdProduct> GetProductListByErpCode(IList<string> productErpCodes);


        /// <summary>
        /// 根据商品与仓库获取待配送的商品数量
        /// </summary>
        /// <returns></returns>
        public abstract int GetPdPending(int pdSysNo, int whSysNo);
       
    }
}
