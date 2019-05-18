using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Extra.SMS
{
    /// <summary>
    /// 短信发送
    /// </summary>
    /// <remarks>2013-6-26 杨浩 创建</remarks>
    public interface ISmsProvider
    {
        /// <summary>
        /// 向服务终端发送一条短信
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="msg">消息(62字以内)</param>
        /// <param name="SendTime">定时发送(精确到秒)</param>
        /// <returns>大于0的数字,发送成功;-1、帐号未注册;-2、其他错误;-3、密码错误;-4、手机号格式不对;-5、余额不足;-6、定时发送时间不是有效的时间格式</returns>
        //int SendASmsByWS(string mobile, string msg, DateTime SendTime);

        /// <summary>
        /// 发送一条短信
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="msg">70字（包含签名）一条短信，超出则按此规则分割成多条短信</param>
        /// <param name="SendTime">定时发送(精确到秒)，为空不需要定时</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        SmsResult Send(string mobile, string msg, DateTime? sendTime);

        /// <summary>
        /// 发送一条短信
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="msg">70字（包含签名）一条短信，超出则按此规则分割成多条短信</param>
        /// <param name="sendTime">定时发送(精确到秒)，为空不需要定时</param>
        /// <param name="smsTemplateCode">短信模板Code（阿里大鱼）</param>
        /// <returns></returns>
        SmsResult Send(string mobile, string msg, DateTime? sendTime, string smsTemplateCode);

        /// <summary>
        /// 各分销商发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="dealername">分销商</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// 王耀发 2016-1-18 创建
        /// <returns>执行结果</returns>
        SmsResult DealerSend(string mobile, string dealername, string msg, DateTime? sendTime);
        /// <summary>
        /// 发送多条短信
        /// </summary>
        /// <param name="mobiles">手机号码List集合</param>
        /// <param name="msg">内容（62字以内,包括符号）</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        //int SendMSmsByDB(IList<String> mobiles, string msg);

        /// <summary>
        /// 发送多条短信，短信内容相同
        /// </summary>
        /// <param name="table">
        /// 短信表
        /// 列名:
        /// Phone(string not null)
        /// Priority(int null)，值来源于SmsPriority枚举
        /// SendDate(datetime null))
        /// </param>
        /// <param name="msg">70字（包含签名）一条短信，超出则按此规则分割成多条短信</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        SmsResult BatchSend(DataTable table, string msg);

        /// <summary>
        /// 发送多条短信，短信内容不同
        /// </summary>
        /// <param name="table">
        /// 短信表
        /// 列名:
        /// Phone(string not null)
        /// Msg(string not null)
        /// Priority(int null)，值来源于SmsPriority枚举
        /// SendDate(datetime null))
        /// </param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        SmsResult BatchSend(DataTable table);

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns>短信剩余条数</returns>
        /// <remarks>2014-04-01 余勇 添加</remarks>
        string Balance();
    }
}
