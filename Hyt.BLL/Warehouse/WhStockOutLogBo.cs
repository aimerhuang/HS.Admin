using Hyt.DataAccess.Log;
using Hyt.Model;
using System;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 出库单详细日志
    /// </summary>
    /// <remarks>2015-1-15 杨浩 创建</remarks>
    public class WhStockOutLogBo : BOBase<WhStockOutLogBo>
    {
        /// <summary>
        /// 写出库单日志
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <param name="content">内容</param>
        /// <param name="userSysNo">系统用户的系统编号</param>
        /// <remarks>2015-1-15 缪竞华 创建</remarks>
        public void WriteLog(int stockOutSysNo, string content, int userSysNo)
        {
            SyUser user = BLL.Sys.SyUserBo.Instance.GetSyUser(userSysNo);
            WriteLog(stockOutSysNo, content, user);
        }

        /// <summary>
        /// 写出库单日志
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <param name="content">内容</param>
        /// <param name="user">系统用户</param>
        /// <remarks>2015-1-15 杨浩 创建</remarks>
        public void WriteLog(int stockOutSysNo, string content, SyUser user)
        {
            string oper = user != null && !string.IsNullOrEmpty(user.UserName) ? user.UserName : "";
            WriteLog(stockOutSysNo, content, oper);
        }

        /// <summary>
        /// 写出库单日志
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <param name="content">内容</param>
        /// <remarks>2015-1-15 杨浩 创建</remarks>
        public void WriteLog(int stockOutSysNo, string content)
        { 
            var current = Authentication.AdminAuthenticationBo.Instance.Current;
            var oper = (current != null && current.Base != null && !string.IsNullOrEmpty(current.Base.UserName)) ? current.Base.UserName : "";
            WriteLog(stockOutSysNo, content, oper);
        }

        /// <summary>
        /// 写出库单日志
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <param name="content">内容</param>
        /// <param name="oper">操作人</param>
        /// <remarks>2014-12-19 杨浩 创建</remarks>
        /// <remarks>2015-1-15 杨浩 创建</remarks>
        private void WriteLog(int stockOutSysNo, string content, string oper = "")
        {
            var log = new WhStockOutLog
            {
                WhStockOutSysNo = stockOutSysNo,
                LogContent = content,
                OperateDate = DateTime.Now,
                Operator = oper
            };
            IWhStockOutLogDao.Instance.CreateWhStockOutLog(log);
        }
    }
}
