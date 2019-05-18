using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Extra.SMS.Mandao
{
    /// <summary>
    /// 发送短信
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    internal class SmsProvider : ISmsProvider
    {
        private static mandao.WebService service = new mandao.WebService();
        private static string sn = "SDK-WSS-010-02708";
        private static string pwd = "8c@eaf4@";

        static SmsProvider()
        {
            pwd = getMD5(sn + pwd);
        }

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
            string result="";

            if (sendTime.HasValue)
                result = service.mt(sn, pwd, mobile, msg, "", sendTime.Value.ToString("yyyy-MM-dd hh:mm:ss"), "");
            else
                result = service.mt(sn, pwd, mobile, msg, "", "", "");

            return GetResult(result);
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
            string result = "";

            if (sendTime.HasValue)
                result = service.mt(sn, pwd, mobile, msg, "", sendTime.Value.ToString("yyyy-MM-dd hh:mm:ss"), "");
            else
                result = service.mt(sn, pwd, mobile, msg, "", "", "");

            return GetResult(result);
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
            StringBuilder mobiles = new StringBuilder();

            for (int i = 0; i < table.Rows.Count;i++ )
            {
                mobiles.Append(table.Rows[i]["Phone"].ToString());

                if (i < table.Rows.Count - 1)
                    mobiles.Append(",");
            }

            string result = service.mt(sn, pwd, mobiles.ToString(), msg, "", "", "");

            return GetResult(result);
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="table"></param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public SmsResult BatchSend(System.Data.DataTable table)
        {
            List<SmsResult> list = new List<SmsResult>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string mobile=table.Rows[i]["Phone"].ToString();
                string msg= table.Rows[i]["Msg"].ToString();
                DateTime? sendTime=table.Rows[i]["SendDate"].Equals(DBNull.Value)?new Nullable<DateTime>():Convert.ToDateTime(table.Rows[i]["SendDate"]);
                SmsResult result = Send(mobile, msg, sendTime);

                list.Add(result);
            }

            return GetResult(list);
        }

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns>短信剩余条数</returns>
        /// <remarks>2014-04-01 余勇 添加</remarks>
        public string Balance()
        {
            string result = service.balance(sn, pwd);
            if (result.StartsWith("-"))
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// 将短信接口状态转换为系统状态
        /// </summary>
        /// <param name="str">短信接口状态</param>
        /// <returns>系统状态</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        private SmsResult GetResult(string str)
        {
            SmsResult result = new SmsResult();

            if (str.StartsWith("-") || str.Equals(""))
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount = 0;
            }
            else
            {
                result.Status = SmsResultStatus.Success;
                result.RowCount = 1;
            }

            return result;
        }

        /// <summary>
        /// 合并批量发送的状态
        /// </summary>
        /// <param name="list">批量发送状态</param>
        /// <returns>合并后的状态</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        private SmsResult GetResult(List<SmsResult> list)
        {
            SmsResult result = new SmsResult();

            int failueCount=list.Count(model=>model.Status==SmsResultStatus.Failue);
            if (failueCount>0)
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount = failueCount;
            }
            else
            {
                result.Status = SmsResultStatus.Success;
                result.RowCount = list.Count;
            }

            return result;
        }

        /// <summary>
        /// 获取md5码
        /// </summary>
        /// <param name="source">待转换字符串</param>
        /// <returns>md5加密后的字符串</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        private static string getMD5(string source)
        {
            string result = "";
            try
            {
                MD5 getmd5 = new MD5CryptoServiceProvider();
                byte[] targetStr = getmd5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(source));
                result = BitConverter.ToString(targetStr).Replace("-", "");
                return result;
            }
            catch (Exception)
            {
                return "0";
            }

        }
    }
}
