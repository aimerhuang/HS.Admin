﻿using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 日志状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class LogStatus
    {
        #region 系统日志

        /// <summary>
        /// 系统日志来源
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 系统日志来源
        {
            前台 = 10,
            后台 = 20,
            商城IphoneApp = 31,
            商城AndroidApp = 32,
            微信商城 = 34,
            物流App = 33,
            外部应用 = 50,
            分销工具 = 60,
            百城通 = 70
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum SysLogLevel
        {
            Debug = 10,
            Info = 20,
            Warn = 30,
            Error = 40,
            Fatal = 50,
        }

        /// <summary>
        /// 系统日志目标类型(更多类型可在开发过程中添加此枚举)
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 系统日志目标类型
        {

            用户 = 10,
            订单 = 20,          
            订单明细 = 21,
            系统配置功能 = 22,
            订单支付报关=23,
            订单支付申报国检=24,
            支付方式 = 30,
            组织机构 = 40,
            任务池 = 50,
            码表 = 60,
            登录 = 70,
            //基础
            配送方式 = 110,
            配送员信用额度 = 120,
            百城当日达区域 = 130,
            配送方式支付方式关联 = 140,
            地区信息 = 150,
            仓库取件方式 = 160,
            仓库快递方式 = 170,
            联盟网站 = 180,

            仓库 = 300,
            出库单 = 310,
            入库单 = 320,
            补单 = 330,
            商品还货 = 340,
            收款账目管理 = 350,//huangwei
            发票 = 360,

            //物流 =400,
            配送单 = 410,
            取件单 = 420,
            结算单 = 430,
            借货单 = 440,
            调拨单 = 450,

            分销商预存款 = 500,
            分销商商城 = 510,
            分销商等级 = 520,
            分销商 = 530,
            分销商商城类型 = 540,
            分销商等级价格 = 550,
            分销商特殊价格 = 560,
            分销商升舱错误日志 = 570,
            分销商商城地区关联 = 580,
            分销商支付方式 = 590,
            经销商城快递代码 = 600,
            代理商 = 1101,
            代理商预存款 = 1102,

            #region 商品基本信息管理 700-720

            商品基本信息 = 700,
            商品分类 = 701,
            商品主分类 = 702,
            商品描述 = 705,
            商品关联 = 706,
            商品搭配销售 = 707,
            商品同步失败 = 708,
            商品库存信息=709,
            #endregion

            #region 商品分类 730-740

            商品分类基本信息 = 730,
            商品分类排序 = 731,
            商品分类状态 = 732,
            商品分类在线显示状态 = 732,

            #endregion

            #region 商品价格管理 750

            商品调价申请 = 750,
            商品调价状态 = 751,
            商品描述模版模块 = 710,
            #endregion

            #region 商品图片管理 760

            商品图片 = 760,
            商品图片排序 = 761,
            商品封面图片 = 762,

            #endregion

            ///WeChat huangwei
            /// <summary>
            /// 微信自动回复配置
            /// </summary>
            WeChatAutoReplyConfig = 800,
            /// <summary>
            /// 微信关键词管理
            /// </summary>
            WeChatKeyWordsMgm = 810,

            /// <summary>
            /// 微信
            /// </summary>
            微信 = 850,

            #region App推送信息

            推送信息 =  860,
            
            #endregion

            //app版本 huangwei
            App版本 = 900,
            ExcelExporting = 910,
            EAS = 1000,
            促销 = 2000,
            优惠卡 = 2100,

            #region 前台管理
            新闻帮助管理 = 600,
            评价晒单管理 = 610,
            搜索关键字 = 620,
            广告组展示 = 630,
            广告项展示 = 640,
            商品组展示 = 650,
            商品项展示 = 660,
            客户投诉 = 670,
            商品咨询 = 680,
            短信咨询 = 681,
            客户短信咨询 = 682,
            大宗采购 = 690,
            客户等级 = 691,
            客户管理 = 692,
            #endregion

            #region 财务管理 250-260
            收款单 = 250,
            收款单明细 = 251,
            网上支付 = 252,
            付款单 = 260,
            付款单明细 = 261,
            #endregion

            #region 系统管理 1100
            系统管理 = 1100,
            #endregion

            /// <summary>
            /// 未捕获异常
            /// </summary>
            未捕获异常 = 9999,
            #region 电子面单管理

            电子面单账号管理 = 1200,
            电子面单账号关联仓库 = 1201,

            #endregion

            #region 调货配置

            调货配置管理 = 1300,

            #endregion

            导入会员 = 1400
            
        }

        #endregion

        #region 绩效日志

        /// <summary>
        /// 业务类型
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 业务类型
        {
            评论审核 = 10,
            晒单审核 = 20,
            咨询回复 = 30,
            电话客服 = 40,
            客服下单 = 50,
            订单审核 = 60,
            客服出库 = 70,
            仓库出库 = 80,
            业务员配送 = 90,
            门店下单 = 100,
            业务员下单 = 110,
            财务结算 = 120,
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 绩效日志状态
        {
            待审核 = 10,
            已审核 = 20,
            作废 = -10,
        }
        #endregion
    }
}
