using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 商城退货单实体
    /// </summary>
    /// <remarks>2013-8-29 陶辉 创建</remarks>
    public class UpGradeRma
    {
        /// <summary>
        /// 商城类型
        /// </summary>
        public int MallType { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// 商城退货单号
        /// </summary>
        public int HytRmaId { get; set; }

        /// <summary>
        /// 第三方退货单号
        /// </summary>
        public string MallRmaId { get; set; }

        /// <summary>
        /// 第三方买家昵称
        /// </summary>
        public string MallBuyerName { get; set; }

        /// <summary>
        /// 买家申请退货时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 商城退货状态
        /// </summary>
        public string HytRmaStatus { get; set; }

        /// <summary>
        /// 第三方退款金额
        /// </summary>
        public decimal MallRefundFee { get; set; }

        /// <summary>
        /// 买家退货原因
        /// </summary>
        public string BuyerRmaReason { get; set; }

        /// <summary>
        /// 买家退货留言
        /// </summary>
        public string BuyerRmaMessage { get; set; }

        /// <summary>
        /// 第三方退货留言
        /// </summary>
        public string MallRmaMessage { get; set; }

        /// <summary>
        /// 分销商商城系统编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 退货商品明细
        /// </summary>
        public List<UpGradeRmaItem> RmaItems { get; set; }

        /// <summary>
        /// 分销商系统编号
        /// </summary>
        /// <remarks>2013-09-12 朱家宏 添加</remarks>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 图片信息(多张图片逗号分隔)
        /// </summary>
        public string ImgPaths { get; set; }
    }

    /// <summary>
    /// hyt退换货日志
    /// </summary>
    /// <remarks>2013-09-10 朱家宏 创建</remarks>
    public class HytRmaLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 退换货系统编号
        /// </summary>
        public int ReturnSysNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDate { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperatorName { get; set; }
    }
}
