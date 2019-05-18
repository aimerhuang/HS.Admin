using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.SMS.Strategy
{
    /// <summary>
    /// 短信空策略
    /// </summary>
    /// <remarks>2013-9-28 杨浩 添加 </remarks>
    public class SmsNullProvider : ISmsProvider
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult Send(string mobile, string msg, DateTime? sendTime)
        {
            return new SmsResult
                {
                    RowCount = 0,
                    Status = SmsResultStatus.Failue
                };
        }
        public SmsResult Send(string mobile, string msg, DateTime? sendTime, string smsTemplateCode)
        {
            return new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };
        }
        /// <summary>
        /// 各分销商发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="dealername">分销商</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// 王耀发 2016-1-18 创建
        /// <returns>执行结果</returns>
        public SmsResult DealerSend(string mobile, string dealername, string msg, DateTime? sendTime)
        {
            return new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };
        }
        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="table"></param>
        /// <param name="msg"></param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult BatchSend(System.Data.DataTable table, string msg)
        {
            return new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="table"></param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult BatchSend(System.Data.DataTable table)
        {
            return new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };
        }

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns>短信剩余条数</returns>
        /// <remarks>2014-04-01 余勇 添加</remarks>
        public string Balance()
        {
            return string.Empty;
        }
    }
}
