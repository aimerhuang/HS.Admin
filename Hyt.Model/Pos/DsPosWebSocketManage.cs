using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 网络socket控制表
    /// </summary>
    /// <remarks>2016-08-03 杨云奕 添加</remarks>
    public class DsPosWebSocketManage
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// socket通讯账户
        /// </summary>
        public string WS_PosNumber { get; set; }
        /// <summary>
        /// 收银机编码
        /// </summary>
        public int WS_PosManageSysNo { get; set; }
        /// <summary>
        ///推送账户描述名称
        /// </summary>
        public string WS_PosName { get; set; }
        /// <summary>
        /// 推送成功数
        /// </summary>
        public int WS_PosSuccessNum { get; set; }
        /// <summary>
        /// 推送失败数
        /// </summary>
        public int WS_PosErrorNum { get; set; }
        /// <summary>
        /// 推送的消息记录
        /// </summary>
        public string WS_PosMessage { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public int WS_Status { get; set; }
    }
}
