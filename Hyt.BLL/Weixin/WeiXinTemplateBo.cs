using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Weixin
{
    /// <summary>
    /// 微信模板
    /// </summary>
    /// <remarks>2016-4-15 杨浩 创建</remarks>
    public class WeiXinTemplateBo : BOBase<WeiXinTemplateBo>
    {
        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="template_id"></param>
        /// <param name="url"></param>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <param name="keyword3"></param>
        /// <param name="keyword4"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public string GetNewOrderNotice(string touser, string template_id, string url, string first, string keyword1, string keyword2, string keyword3, string keyword4, string remark)
        {
            string data = "{";
            data += "             \"touser\":\"" + touser + "\",";
            data += "              \"template_id\":\"" + template_id + "\",";
            data += "              \"url\":\"" + url + "\",";
            data += "              \"topcolor\":\"#FF0000\",";
            data += "              \"data\":{";
            data += "                  \"first\": {\"value\":\"" + first + "\",\"color\":\"#173177\"},";
            data += "                  \"keyword1\": {\"value\":\"" + keyword1 + "\",\"color\":\"#173177\"},";
            data += "                  \"keyword2\": {\"value\":\"" + keyword2 + "\",\"color\":\"#173177\"},";
            data += "                  \"keyword3\": {\"value\":\"" + keyword3 + "\",\"color\":\"#173177\"},";
            data += "                  \"keyword4\": {\"value\":\"" + keyword4 + "\",\"color\":\"#173177\"},";
            data += "                  \"remark\": {\"value\":\"" + remark + "\",\"color\":\"#000\"}";
            data += "              }";
            data += "      }";

            return data;
        }
    }
}
