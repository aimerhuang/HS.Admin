using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Service.Contract.Base;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.SystemPredefined;
using Hyt.Infrastructure;

namespace Hyt.Service.Contract.B2CApp
{
    /// <summary>
    /// 商品相关
    /// </summary>
    /// <remarks> 2013-7-5 杨浩 创建 </remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IProduct :IBaseServiceContract
    {
        #region Common

        /// <summary>
        /// 获取所有产品类别
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<ParentClassApp>> GetCategories();

        /// <summary>
        /// 获取热门搜索关键字
        /// </summary>
        /// <returns></returns>
         /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<string>> GetHotSearchKeys();

        /// <summary>
        /// 获取搜索推荐关键字
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<string> GetSearchKeys();

        /// <summary>
        /// 获取用户搜索历史
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<string>> GetHistorySearch(int customerSysNo);

        /// <summary>
        /// 获取商品组
        /// </summary>
        /// <param name="code">商品组编码</param>
        /// <returns></returns>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Model.B2CApp.ProductGroup>> GetAdvertProducts(string code);

        /// <summary>
        /// 获取广告组
        /// </summary>
        /// <param name="code">广告组编码</param>
        /// <returns></returns>
        /// <remarks>2013-7-17 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Model.B2CApp.AdvertGroup>> GetAdverts(string code);

        /// <summary>
        /// App 首页数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-7-17 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<Home> Home();

        /// <summary>
        /// 查看首页更多
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <remarks>2013-7-17 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<HomeMore> GetHomeMore(string code);

        /// <summary>
        /// 查看用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        /// <remarks> 2013-9-3 杨浩 创建 </remarks>
        [CustomOperationBehavior(true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<SimplProduct>> GetHistory(int customerSysNo);

        /// <summary>
        /// 删除用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        /// <remarks> 2013-9-3 杨浩 创建 </remarks>
        [CustomOperationBehavior(true)]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result DeleteHistory(int customerSysNo);

        /// <summary>
        /// 摇一摇
        /// </summary>
        /// <param name="brand">手机品牌</param>
        /// <param name="type">手机型号</param>
        /// <returns></returns>
        /// <remarks> 2013-9-3 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<SimplProduct>> GetShake(string brand, string type);

        /// <summary>
        /// 获取分享内容
        /// </summary>
        /// <param name="sysNo">商品系统号</param>
        /// <param name="productName">商品名称</param>
        /// <returns></returns>
        /// <remarks> 2013-10-10 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<Share> GetShare(int sysNo, string productName);

        /// <summary>
        /// 获取团购分享内容
        /// </summary>
        /// <param name="sysNo">团购系统号</param>
        /// <returns></returns>
        /// <remarks> 2013-10-10 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<Share> GetGroupShare(int sysNo);

        #endregion

        #region 商品检索
        /// <summary>
        /// 重置产品缓存
        /// </summary>
        /// <param name="dealerSysNo">店铺编号</param>
        /// <remarks>2013-7-20 杨浩 创建</remarks>
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result ResetProductCache(int dealerSysNo);
        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="categorySysNo">分类编号</param>
        /// <param name="attributes">属性列表</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageCount">分页总数</param>
        /// <param name="recCount">总记录数</param>
        /// <param name="highLight">是否高亮关键字</param>
        /// <param name="sort">排序(0:相关度 1:销量 2:价格 3:评分 4:上架时间)</param>
        /// <param name="isDescending">true 为降序 false为升序</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="priceSourceSysNo">产品价格来源编号(会员等级编号)</param>
        /// <param name="showNotFrontEndOrder">搜索前台不能下单的商品</param>
        /// <returns>商品列表</returns>
        /// <remarks> 2013-7-20 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<PdProductIndex>> SearchFromDataBase(string key
            , int categorySysNo
            , string attributes
            , int pageSize
            , int pageIndex
            , bool highLight = false
            , int sort = 1
            , bool isDescending = false
            , int productSysNo = 0
            , int priceSource=10
            , int priceSourceSysNo = CustomerLevel.初级
            , bool showNotFrontEndOrder = false
            , int brandSysNo = 0
            , string ProductGroupCode = null
            , int OriginSysNo = 0
            , int DealerSysNo = 0
            , int WarehouseSysNo = 0
            , int productType = 0
            );
        /// <summary>
        /// 根据产品编号搜索产品
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="productSysNoList">产品编号列表</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="priceSourceSysNo">客户等级编号</param>
        /// <param name="isFrontDisplay">前台显示</param>
        /// <returns></returns>
        /// <remarks>2017-1-12 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContractAttribute(Name = "SearchByProductSysNo")]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<PdProductIndex>> Search(int dealerSysNo, string productSysNoList, int priceSource, int priceSourceSysNo,int isFrontDisplay=1);
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="sysNo">分类系统号</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="sort">排序(1：销量，2：价格，3：好评.[正数为降序,负数升序])</param>
        /// <param name="attributeOptionSysNo">商品属性值编号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-17 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/List/{sysNo}/?pageIndex={pageIndex}&sort={sort}&attributeOptionSysNo={attributeOptionSysNo}"
                 , Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json
                 , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<SimplProduct>> GetProductList(string sysNo, int pageIndex, int sort, string attributeOptionSysNo);

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="sort">排序(1：销量，2：价格，3：好评.[正数为降序,负数升序])</param>
        /// <param name="attributeOptionSysNo">商品属性值编号字串</param>
        /// <returns></returns>
        /// <remarks> 2013-7-20 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<PdSearch> Search(string key, int pageIndex, int sort, string attributeOptionSysNo);

        /// <summary>
        /// 根据产品类别获取筛选属性
        /// </summary>
        /// <param name="categorySysNo">产品类别编号</param>
        /// <returns></returns>
        /// <remarks> 2013-8-7 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<ProductAttribute>> GetFilterAttribute(int categorySysNo);

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="sysNo">产品系统号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-17 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Details/{sysNo}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<Model.B2CApp.ProductDetail> GetProductDetails(string sysNo);

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="sysNo">产品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-17 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Model.B2CApp.Specification>> GetSpecification(int sysNo);

        #endregion

        #region 商品咨询

        /// <summary>
        /// 获取商品咨询列表
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Question/List/{productSysNo}?pageIndex={pageIndex}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<CustomerQuestion>> GetProductQuestions(string productSysNo, int pageIndex);
        
        /// <summary>
        /// 添加商品咨询
        /// </summary>
        /// <param name="question">商品咨询模型</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(true)]
        [WebInvoke(UriTemplate = "/Question/Add",Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddProductQuestions(CustomerQuestionAdd question);

        /// <summary>
        /// 获取商品咨询类型
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Question/GetType", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IDictionary<int, string>> GetProductQuestionType();

        #endregion

        #region 商品评价

        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="productSysNo">产品号</param>
        /// <param name="type">5(All) 10（满意） 15(一般) 20(不满意)</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Comment/List/{productSysNo}?type={type}&pageIndex={pageIndex}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<PdCommentTotal> GetProductComments(string productSysNo,int type, int pageIndex);

        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="comment">商品评论模型</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [WebInvoke(UriTemplate = "/Comment/Add/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result AddComment(PdComment comment);

        #endregion

        #region 商品晒单

        /// <summary>
        /// 获取商品晒单列表
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Share/List/{productSysNo}?pageIndex={pageIndex}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<ShareOrderList>> GetShareOrders(string productSysNo, int pageIndex);

        /// <summary>
        /// 新增晒单
        /// </summary>
        /// <param name="share">新增晒单模型</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Share/Add/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddShareOrders(PostShareOrder share);

        /// <summary>
        /// 获取商品晒单详细
        /// </summary>
        /// <param name="sysNo">晒单系统编号</param>
        /// <returns></returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/Share/Details/{sysNo}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<ShareOrderDetails> GetShareOrderDetails(string sysNo);

        #endregion

        #region 商品团购

        /// <summary>
        /// 获取团购列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        /// <remarks> 2013-8-12 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/GroupBuy/List?pageIndex={pageIndex}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultPager<IList<GroupShopping>> GetGroupList(int pageIndex);

        /// <summary>
        /// 获取团购详情
        /// </summary>
        /// <param name="sysNo">团购编号或商品编号</param>
        /// <param name="type">类型：5：团购，10：商品</param>
        /// <returns></returns>
        /// <remarks> 2013-9-6 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(UriTemplate = "/GroupBuy/Details/{sysNo}?type={type}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<GroupShoppingDetails> GetGroupDetails(string sysNo,int type);

        #endregion

        #region 组合商品

        /// <summary>
        /// 获取组合商品
        /// </summary>
        /// <param name="sysNo">主商品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-9-9 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<Combo>> GetCombo(int sysNo);

        #endregion
    }
}
