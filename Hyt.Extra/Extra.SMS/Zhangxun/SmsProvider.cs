using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Extra.SMS.Zhangxun
{
    /// <summary>
    /// 发送短信
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    internal class SmsProvider : ISmsProvider
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult Send(string mobile, string msg, DateTime? SendTime)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Phone", typeof(string));//号码 必填
            table.Columns.Add("Msg", typeof(string));//短信正文 必填
            table.Columns.Add("priority", typeof(int));//短信优先级别 可null
            table.Columns.Add("SendDate", typeof(DateTime));//定时发送时间 可null
            table.Rows.Add(mobile, msg, null, SendTime);

            return BatchSend(table);
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
        public SmsResult DealerSend(string mobile, string dealername, string msg, DateTime? SendTime)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Phone", typeof(string));//号码 必填
            table.Columns.Add("Msg", typeof(string));//短信正文 必填
            table.Columns.Add("priority", typeof(int));//短信优先级别 可null
            table.Columns.Add("SendDate", typeof(DateTime));//定时发送时间 可null
            table.Rows.Add(mobile, msg, null, SendTime);

            return BatchSend(table);
        }
        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="table"></param>
        /// <param name="msg"></param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult BatchSend(DataTable table, string msg)
        {
            int rowCount = DatabaseAccess.ExecuteSql(table, msg, "sp_sms_zhagnxun_same", "SendSms_same");

            SmsResult result = GetResult(rowCount);

            return result;
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="table"></param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult BatchSend(DataTable table)
        {
            int rowCount = DatabaseAccess.ExecuteSql(table, "sp_sms_zhagnxun_different", "SendSms_different");

            SmsResult result = GetResult(rowCount);

            return result;
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

        /// <summary>
        /// 转换结果
        /// </summary>
        /// <param name="rowCount">影响行数</param>
        /// <returns>SmsResult</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        private SmsResult GetResult(int rowCount)
        {
            SmsResult result = new SmsResult();

            if (rowCount == -1)
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount = 0;
            }
            else
            {
                result.Status = SmsResultStatus.Success;
                result.RowCount = rowCount;
            }

            return result;
        }
    }
}
