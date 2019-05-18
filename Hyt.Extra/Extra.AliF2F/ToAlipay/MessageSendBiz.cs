using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.AliF2F.ToAlipay
{
    public class MessageSendBiz
    {
        public MessageSendBiz()
        {
        }


        //消息异步单发接口
        public static string CustomSend(string biz_content)
        {
            AlipayMobilePublicMessageCustomSendRequest pushRequst = new AlipayMobilePublicMessageCustomSendRequest();
            pushRequst.BizContent = biz_content;

            //Response.Output.WriteLine(biz_content);
            // Response.End();

            Dictionary<string, string> paramsDict = (Dictionary<string, string>)pushRequst.GetParameters();
            IAopClient client = new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key);
            AlipayMobilePublicMessageCustomSendResponse pushResponse = client.Execute(pushRequst);
            return pushResponse.Body;
        }

        //消息异步群发接口，所有关注用户都能够收到
        public static string TotalSend(string biz_content)
        {
            AlipayMobilePublicMessageTotalSendRequest pushRequst = new AlipayMobilePublicMessageTotalSendRequest();
            pushRequst.BizContent = biz_content;

            //Response.Output.WriteLine(biz_content);
            // Response.End();

            Dictionary<string, string> paramsDict = (Dictionary<string, string>)pushRequst.GetParameters();
            IAopClient client = new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key);
            AlipayMobilePublicMessageTotalSendResponse pushResponse = client.Execute(pushRequst);
            return pushResponse.Body;
        }
    }
}
