using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiLogistics.HYH
{
    /// <summary>
    /// 表示一个操作结果
    /// </summary>
    public class ActionResponse
    {
        public static int SuccessCode = 200;
        public static int FailCode = 400;

        /// <summary>
        /// 0=失败
        /// 1=成功
        /// 其它自定义
        /// </summary>
        public int ResultCode
        {
            get;
            set;
        }

        /// <summary>
        /// 该操作结果所返回的信息，一般在操作失败的情况下返回失败的原因
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 该操作结果所返回对象，一般在操作成功的情况下需要的数据
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 在分页的时候返回总记录数
        /// </summary>
        public int TotalRecord
        {
            get;
            set;
        }

        /// <summary>
        /// 返回该操作的执行结果
        /// </summary>
        public bool Success=true;


        public static ActionResponse CreateSuccessResponse(object tag)
        {
            return CreateActionResponse(SuccessCode, tag, null);
        }

        public static ActionResponse CreateSuccessResponse(object tag, string message)
        {
            return CreateActionResponse(SuccessCode, tag, message);
        }

        public static ActionResponse CreateSuccessResponse(object tag, int totalRecord)
        {
            return CreateActionResponse(SuccessCode, tag, null, totalRecord);
        }


        public static ActionResponse CreateFailResponse(string message, params object[] args)
        {
            var s = string.Format(message, args);
            return CreateActionResponse(FailCode, null, s);
        }

        public static ActionResponse CreateActionResponse(int resultCode, object tag, string message, int totalRecord = 0)
        {
            ActionResponse ar = new ActionResponse() { ResultCode = resultCode, Tag = tag, Message = message, TotalRecord = totalRecord };
            return ar;
        }


    }

}
