using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Service.Contract.Base;
using Hyt.Infrastructure.Pager;
using Hyt.Model.UpGrade;

namespace Hyt.Service.Contract.MallSeller
{
    /// <summary>
    /// 账户信息接口
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    [ServiceContract]
    public interface IMallAccount : IBaseServiceContract
    {
        /// <summary>
        /// 分销商商城账号登录
        /// </summary>
        /// <param name="account">分销商商城后台账号</param>
        /// <param name="password">分销商商城后台密码</param>
        /// <returns>当前账号信息以及授权信息</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method="POST",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<CBDsDealerMall> HytLogin(string account, string password);
        
        /// <summary>
        /// 第三方账号登录成功回调获取分销商所有商城授权信息
        /// </summary>
        /// <param name="mallAccount">第三方账号</param>
        /// <param name="mallType">第三方商城类型</param>
        /// <param name="authorizationCode">授权码</param>
        /// <returns>当前账号信息以及授权信息</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method="GET",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsDealerMall> MallLogin(string mallAccount, int mallType, string authorizationCode);
        
        /// <summary>
        /// 查询分销商商城交易明细
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <param name="startDate">交易查询起始日期</param>
        /// <param name="endDate">交易查询截止日期</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns>商城交易明细</returns>
        /// <remarks>2013-08-29 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        //[CustomOperationBehavior(false)]
        //[OperationContract]
        //Result<List<TransactionDetailInfo>> GetTransactionDetails(int sysNo,DateTime startDate, DateTime endDate,int pageIndex,int pageSize);

        /// <summary>
        /// 根据店铺账号获取分销商商城
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商商城</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        //[WebInvoke(Method="GET",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsDealerMall> GetDsDealerMallByShopAccount(string shopAccount, int mallTypeSysNo);
        
        /// <summary>
        /// 根据分销商编号获取分销商商城列表
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>分销商商城列表</returns>
        /// <remarks>2013-09-13 黄志勇 创建</remarks>
        //[WebInvoke(Method="GET",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<DsDealerMall>> GetDsAuthorizations(int dealerSysNo);

        /// <summary>
        /// 更新分销商商城表
        /// </summary>
        /// <param name="info">分销商商城</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        //[WebInvoke(Method="GET",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        int UpdateDsAuthorization(DsDealerMall info);

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        ///<returns></returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        //[WebInvoke(Method="GET",ResponseFormat=WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeAccount> GetAccountInfo(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 分页查询分销商预存款往来账明细
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-9-6 黄志勇 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<PagedList<DsPrePaymentItem>> GetDsPrePaymentItemList(MallAccountParameters param);


        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeAuthorization> SetLoginInfo(string shopAccount, int mallTypeSysNo);
        /// <summary>
        /// 获取分销商预存款账务信息
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 朱成果 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsPrePayment> GetDsPrePayment(int dealerSysNo);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="oldPassword">旧密码（已加密）</param>
        /// <param name="newPassword">新密码（已加密）</param>
        /// <param name="dsUserSysNo">分销商用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-5 黄志勇 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result ModifyPassword(int dealerSysNo, string oldPassword, string newPassword, int dsUserSysNo=0);



        /// <summary>
        /// 更新分销商预存款余额提醒额度
        /// </summary>
        /// <param name="alertAmount">余额提示额</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>t:设置成功 f:失败</returns>
        /// <remarks>2014-03-21 朱家宏 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result SaveAlertSetting(decimal alertAmount, int dealerSysNo);


        /// <summary>
        /// 获取商城类型详情
        /// </summary>
        /// <param name="sysNo">商城类型编号</param>
        /// <returns>获取商城类型详情</returns>
        /// <remarks>2014-06-11 朱成果 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsMallType> GetDsMallType(int sysNo);


        /// <summary>
        /// 获取分销商商城信息(授权码已加密)
        /// </summary>
        /// <param name="sysNo">商城编号</param>
        /// <returns>分销商商城信息</returns>
        /// <remarks>2014-06-11 朱成果 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsDealerMall> GetDsDealerMall(int  sysNo); 
      
    }

    #region 交易记录查询参数

    /// <summary>
    /// 交易记录查询参数
    /// </summary>
    /// <remarks>2013-9-6 黄志勇 创建</remarks>
    public class MallAccountParameters
    {
        private int _pageSize;
        private DateTime? _endDate;

        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int HytDealerSysNo { get; set; }

        /// <summary>
        /// 创建时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 创建时间(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?) null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }


        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
    }

    #endregion
}
