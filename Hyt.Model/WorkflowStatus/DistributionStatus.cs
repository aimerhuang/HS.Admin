using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 分销状态
    /// </summary>
    /// <remarks>2013-09-04 陶辉 创建</remarks>
    public class DistributionStatus
    {
        /// <summary>
        /// 经销商特殊价格状态
        /// 数据表：DsSpecialPrice 字段:Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 经销商特殊价格状态
        {
            启用 = 1,
            禁用 = 0
        }

        /// <summary>
        /// 分销商状态
        /// 数据表:DsDealer 字段:Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 分销商状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 分销商状态
        /// 数据表:DSUSER 字段:Status
        /// </summary>
        /// <remarks>2014-06-05 朱成果 创建</remarks>
        public enum 分销商账号状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 分销商App状态
        /// 数据表:DsDealerApp 字段:Status
        /// </summary>
        /// <remarks>2014-05-06 陶辉 创建</remarks>
        public enum 分销商App状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 分销商等级状态
        /// 数据表:DsDealerLevel 字段:Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 分销商等级状态
        {
            启用 = 1,
            禁用 = 0
        }

        /// <summary>
        /// 分销商特殊价格状态
        /// 数据表：DsSpecialPrice 字段:Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 分销商特殊价格状态
        {
            启用 = 1,
            禁用 = 0
        }

        /// <summary>
        /// 商城类型状态
        /// 数据表：DsMallType 字段：Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 商城类型状态
        {
            启用 = 1,
            禁用 = 0
        }

        /// <summary>
        /// 预存款明细状态
        /// 数据表：DsPrePaymentItem 字段：Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 预存款明细状态
        {
            冻结 = 10,
            完结 = 20,
            失败 = -10
        }

        /// <summary>
        /// 商城状态
        /// 数据表：DsDealerMall 字段：Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 分销商商城状态
        {
            启用 = 1,
            禁用 = 0
        }

        /// <summary>
        /// 商城是否使用预存款
        /// 数据表：DsMallType 字段：IsPreDeposit
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 商城是否使用预存款
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 商城是否自营
        /// 数据表：DsMallType 字段：IsSelfSupport
        /// </summary>
        /// <remarks>2013-9-16 陶辉 创建</remarks>
        public enum 商城是否自营
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 预存款明细来源
        /// 数据表：DsPrePaymentItem 字段：Source
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 预存款明细来源
        {
            预存款 = 10,
            订单作废 = 20,
            退款 = 30,
            订单消费 = 50,
            返利 = 60,
            提现 = 100

        }

        /// <summary>
        /// 商城类型预定义值
        /// </summary>
        /// <remarks>2013-9-9 杨浩 创建</remarks>
        public enum 商城类型预定义
        {
            天猫商城 = 1,
            淘宝分销 = 2,
            拍拍网购 = 3,
            亚马逊 = 4,
            百度众测 = 5,
            一号店 = 6,
            国美在线 = 7,
            百度微购 = 8,
            阿里巴巴经销批发 = 9,
            京东商城 = 10,
            有赞 = 11,
            苏宁易购 = 12,
            海带网 = 13,
            格格家 = 14,
            海拍客=15,
            国内货栈=16,
        }

        /// <summary>
        /// 升舱订单状态
        /// 数据表：DsOrder 字段：Status
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 升舱订单状态
        {
            升舱中 = 10,
            已发货 = 20,
            已完成 = 30,
            失败 = -10
        }

        /// <summary>
        /// 分销商商城店铺是否自营
        /// 数据表：DsDealerMall 字段：IsSelfSupport
        /// </summary>
        /// <remarks>2013-9-18 陶辉 创建</remarks>
        public enum 是否自营
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 分销商EAS关联状态
        /// 数据表：DsEasAssociation 字段：Status
        /// </summary>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public enum 分销商EAS关联状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 升舱第三方商城订单是否确认发货
        /// </summary>
        public enum 是否确认发货
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 买家是否备注
        /// </summary>
        /// <remarks>2013-9-12 陶辉 创建</remarks>
        public enum 买家是否备注
        {
            全部 = 0,
            是 = 1,
            否 = 2
        }

        /// <summary>
        /// 升舱订单交易状态
        /// 数据表：SoOrder 字段：PayStatus
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public enum 升舱订单交易状态
        {
            未支付 = 10,
            已支付 = 20,
            支付异常 = 30
        }

        /// <summary>
        /// 退换货状态
        /// 数据表:RcReturn 字段:Status
        /// </summary>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public enum 退换货状态
        {
            待审核 = 10,
            待入库 = 20,
            待退款 = 30,
            已完成 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 淘宝订单旗帜，可标识是否需要升舱
        /// </summary>
        /// <remarks>2013-9-12 陶辉 创建</remarks>
        public enum 淘宝订单旗帜
        {
            红 = 1,
            黄 = 2,
            绿 = 3,
            蓝 = 4,
            紫 = 5
        }

        /// <summary>
        /// 分销商升舱错误日志状态
        /// </summary>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public enum 分销商升舱错误日志状态
        {
            待解决 = 10,
            已解决 = 20
        }
        /// <summary>
        /// 分销商提现支付类型
        /// </summary>
        public enum 分销商提现支付类型
        {
            网银 = 10,
            支付宝 = 20
            //,
            //转账 = 30,
            //现金 = 40,
            //支票 = 50,
            //分销商预存 = 110
        }

        /// <summary>
        /// 代理商状态
        /// 数据表:DsAgent 字段:Status
        /// </summary>
        /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
        public enum 代理商状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 分销商申请状态
        /// 数据表:DsDealerApply 字段:Status
        /// </summary>
        /// <remarks>2016-4-18 王耀发 创建</remarks>
        public enum 分销商申请状态
        {
            作废 = -10,
            未审核 = 10,
            已审核 = 20,
        }
        public enum 升舱商城同步日志状态
        {
            等待 = 0,
            成功 = 10,
            失败 = 20,
            作废 = -10
        }
        public enum 升舱商城同步同步类型
        {
            发货 = 10,
            订单完成 = 20
        }

        #region 物流公司
        /// <summary>
        ///  京东物流公司 吴琨 2017/8/30 创建
        /// </summary>
        public enum 京东物流公司
        {
            顺丰快递 = 467,
            香港邮政 = 1274, //厂家自送
            百世汇通 = 1274,  //厂家自送
            EMS = 465,
            优速快递 = 1747,
            中通快递 = 1499,
            天天快递 = 1274,//厂家自送
            门店自取 = 1274,//厂家自送
            普通快递 = 1274,//厂家自送
            韵达快递 = 1327,
            快捷速递 = 2094,
            快捷快递 = 2094,
            圆通快递 = 463,
            申通快递 = 470,
            厂家自送=1274
            #region
            //安能快递 = 596494,
            //速尔物流 = 2464,
            //居家通 = 329192,
            //安得物流 = 247899,
            //中铁快运 = 466,
            //中铁CRE = 605050,
            //跨越速递 = 599866,
            //万家康 = 568096,
            //卡行天下 = 500043,
            //亚风快运 = 323141,
            //用户自提 = 332098,
            //京东大件开放承运商 = 336878,
            //上药物流 = 328977,
            //如风达 = 313214,
            //贝业新兄弟 = 222693,
            //易宅配物流 = 171686,
            //一智通物流 = 171683,
            //斑马物联网 = 6012,
            //中铁物流 = 5419,
            //安能物流 = 4832,
            //德邦快递 = 3046,
            //天地华宇 = 2462,
            //佳吉快运 = 2460,
            //新邦物流 = 2461,
            //国通快递 = 2465,
            //中国邮政挂号信 = 2171,
            //邮政快递包裹 = 2170,
            //速尔快递 = 2105,
            //嘉里大通物流=2101,
            //全一快递 = 2100,
            //联邦快递 = 2096,

            //龙邦快递 = 471,
            //德邦物流 = 2130,
            //全峰快递 = 2016,

            //宅急便 = 1549,

            //宅急送 = 1409,

            //厂家自送 = 1274,
            #endregion


        }

        /// <summary>
        /// 一号店物流公司枚举 吴琨 2017-8-31 创建
        /// </summary>
        public enum 一号店物流公司
        {
            顺丰快递 = 1756,
            香港邮政 = -1,
            百世汇通=-1,
            EMS = 1759,
            优速快递 = 17331,
            中通快递 = 1751,
            天天快递 = 10299,
            门店自取 = -1,
            普通快递=-1,
            韵达快递 = 1754,
            快捷速递 = 28324,
            快捷快递 = 2094,
            圆通快递=1755,
            #region
            //DHL快递 = 17313,
            //汇通快运 = 1760,
            #endregion


        }

        /// <summary>
        /// 海带物流公司 罗勤瑶 2017-10-31 创建
        /// </summary>
        public enum 海带物流公司
        {
            顺丰快递 = 1756,
            香港邮政 = -1,
            百世汇通 = -1,
            EMS = 1759,
            优速快递 = 17331,
            中通快递 = 1751,
            天天快递 = 10299,
            门店自取 = -1,
            普通快递 = -1,
            韵达快递 = 1754,
            快捷速递 = 28324,
            快捷快递 = 2094,
            圆通快递 = 1755,
            #region
            //DHL快递 = 17313,
            //汇通快运 = 1760,
            #endregion


        }

        /// <summary>
        /// 有赞物流公司
        /// </summary>
        /// 2017-8-31 吴琨 创建
        public enum 有赞物流公司
        {
            顺丰快递 = 42,
            香港邮政=101,
            百世汇通 = 6,
            EMS = 11,
            优速快递=38,
            中通快递 = 3,
            天天快递=5,
            门店自取 = -1,
            普通快递=-1,
            韵达快递=4,
            快捷速递 = 34,
            快捷快递 = 2094,
            圆通快递=2,
            申通快递 = 1
            #region
            //
            //圆通速递=2,
            //中通速递=3,
            //韵达快运=4,
            //天天快递=5,
            
            //顺丰速运=7,
            //邮政国内小包=8,
            //EMS经济快递=10,
            
            //邮政平邮=12,
            //德邦快递=13,
            //联昊通=16,
            //全峰快递=17,
            //全一快递=18,
            //城市100=19,
            //汇强快递=20,
            //广东EMS=21,
            //速尔=22,
            //飞康达速运=23,
            //宅急送=25,
            //联邦快递=27,
            //德邦物流=28,
            //中铁快运=30,
            //信丰物流=31,
            //龙邦速递=32,
            //天地华宇=33,
            //
            //新邦物流=36,
            //能达速递=37,
            //优速快递=38,
            //国通快递=40,
            //其他=41,
            
            //AAE=43,
            //安信达=44,
            //百福东方=45,
            //BHT=46,
            //邦送物流=47,
            //传喜物流=48,
            //大田物流=49,
            #endregion


        }

        #endregion




    }
}