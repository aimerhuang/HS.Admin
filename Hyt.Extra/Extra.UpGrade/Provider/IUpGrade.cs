using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.UpGrade.Model;
using Hyt.Model;
using Hyt.Model.UpGrade;

namespace Extra.UpGrade.Provider
{
    #region 升舱底层接口
    /// <summary>
    /// 升舱底层接口
    /// </summary>
    /// <remarks>2014-01-07 陶辉 重构</remarks>
    public interface IUpGrade
    {
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth);

        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth);

        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth);

        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth);

        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2103-9-5 陶辉 创建</remarks>
        Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth);

        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <remarks>2013-9-9 陶辉 创建</remarks>
        Result<AccessTokenResult> GetAuthorizationCode(string code);

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <returns>2014-03-25</returns>
        Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth);
        /// <summary>
        ///获取第三方支持的快递
        /// </summary>
        /// <param name="auth">授权参数</param>
        /// <returns>2018-03-25 罗勤尧</returns>
        Result<UpGradeExpress> GetExpress(AuthorizationParameters auth);

    }

    #endregion
}
