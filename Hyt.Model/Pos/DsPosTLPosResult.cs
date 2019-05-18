using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class DsPosTLPosResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trxcode { get; set; }
        /// <summary>
        /// 通联分配的appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string cusid { get; set; }
        /// <summary>
        /// 调用时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string randomstr { get; set; }
        /// <summary>
        /// sign校验码
        /// </summary>
        public string sign { get; set; }
        // <summary>
        /// 业务流水
        /// </summary>
        public string bizseq { get; set; }
        /// <summary>
        /// 交易结果码
        /// </summary>
        public string trxstatus { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string trxid { get; set; }
        /// <summary>
        /// 原交易流水
        /// </summary>
        public string srctrxid { get; set; }
        /// <summary>
        /// 交易请求日期
        /// </summary>
        public string trxday { get; set; }
        /// <summary>
        /// 交易完成时间
        /// </summary>
        public string paytime { get; set; }
        /// <summary>
        /// 终端编码
        /// </summary>
        public string termid { get; set; }
        /// <summary>
        /// 终端批次号
        /// </summary>
        public string termbatchid { get; set; }
        /// <summary>
        /// 终端流水
        /// </summary>
        public string traceno { get; set; }
        /// <summary>
        /// 业务关联内容
        /// </summary>
        public string trxreserve { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string stauts { get; set; }
        /// <summary>
        /// 收银机
        /// </summary>
        public string PosKey { get; set; }
        /// <summary>
        /// 管理的订单编码
        /// </summary>
        public string OrderSysNo { get; set; }
        /// <summary>
        /// 销售类型
        /// </summary>
        public int SellType { get; set; }
    }
}
