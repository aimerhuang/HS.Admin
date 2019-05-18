using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.UpGrades
{
    /// <summary>
    /// 融e购接口实现
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RongEgouUpGrade : IUpGrade
    {
        /// <summary>
        /// 融e购接口配置信息
        /// </summary>
        private static readonly RongEgouConfig config = UpGradeConfig.GetRongEgouConfig();

        private static string date = DateTime.Now.ToString();

        public Result<RongEgouResultParameters> GetSign()
        {
            string req_data = "<?Xml version=\"1.0\" encoding=\"UTF-8\"?><body><create_start_time>" + date + "</create_start_time><create_end_time>" + date + "</create_end_time><modify_time_from></modify_time_from><modify_time_to></modify_time_to><order_status>2</order_status></body>";
            string content = "app_key=" + config.app_key + "&auth_code=" + config.auth_code + "&req_data=" + req_data;
            //byte[] bytes = System.Security.Cryptography.HMACSHA256.Create(Encoding.UTF8.GetBytes(content), config.app_secret);
            return null;
        }

        public Result<List<Hyt.Model.UpGrade.UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }

        public Result<List<Hyt.Model.UpGrade.UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }

        public Result<Hyt.Model.UpGrade.UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }

        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }

        public Result<List<Hyt.Model.UpGrade.UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }

        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            throw new NotImplementedException();
        }

        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }


        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }
}
