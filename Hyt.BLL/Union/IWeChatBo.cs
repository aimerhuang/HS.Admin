using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Weixin;

namespace Hyt.BLL.Union
{
    /// <summary>
    /// 微信服务接口
    /// </summary>
    /// <remarks>陶辉 2013-10-25 创建</remarks>
    public interface IWeChatBo
    {
        /// <summary>
        /// 判断产品真伪
        /// </summary>
        /// <param name="code">产品防伪编码</param>
        /// <returns>是否真品</returns>
        /// <remarks>2013-10-25 陶辉 创建</remarks>
        Result<WeChatValidation> CheckProduct(string code);

        /// <summary>
        /// 根据关键字获取自动回复列表
        /// </summary>
        /// <param name="content">客户咨询内容</param>
        /// <returns>自动回复列表</returns>
        /// <remarks>2013-10-25 陶辉  创建</remarks>
        List<MkWeixinKeywordsReply> GetAutoReplys(string content);
    }
}
