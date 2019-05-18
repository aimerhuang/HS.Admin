using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// CommonEnum
    /// </summary>
    /// <remarks>2013-08-29 杨浩 创建</remarks>
    public class CommonEnum
    {
        /// <summary>
        /// 仓库代码
        /// </summary>
        ///  <remarks>2015-10-12 杨浩 创建</remarks>
        public enum 仓库代码
        {
            #region
            ////[Description("FuJian")]
            ////福建 = 7,
            //#region 五洲仓
            //[Description("WuZhou")]
            //前海五洲自选仓 = 69,
            //[Description("WuZhou")]
            //五洲组合仓 = 64,
            //#endregion

            //#region 兴业仓库
            //[Description("XingYe")]
            //兴业国际 = 72,

            //[Description("XingYe")]
            //兴业国际凹凸仓 = 70,

            //[Description("XingYe")]
            //兴业嘉退换货仓 = 75,

            //[Description("XingYe")]
            //兴业嘉仓 = 65,
            //#endregion

            //#region 卓鼎仓
            //[Description("ZhuoDing")]
            //前海卓鼎自选仓 = 68,

            //[Description("ZhuoDing")]
            //卓鼎组合仓 = 63,

            //#endregion

            //#region 满江红
            //[Description("ManJiangHong")]
            //满疆红仓 = 74,
            //#endregion

            //#region 通用仓库
            //[Description("TongYong")]
            //东方国际 = 83,
            //#endregion

            //#region 沙田仓
            //[Description("ShaTian")]
            //沙田自选仓 = 71,

            //[Description("ShaTian")]
            //沙田组合仓 = 62,

            //[Description("ShaTian")]
            //沙田海外直邮仓 = 78,
            //#endregion

            //#region 深圳仓
            //[Description("ShenZhen")]
            //深圳完税仓 = 66,

            //[Description("ShenZhen")]
            //深圳近期凹凸仓 = 77,
            //#endregion

            //#region 聚美仓
            //[Description("JuMei")]
            //五洲云仓 = 85,
            //#endregion
            #endregion

            [Description("ShenZhen")]
            深圳仓 = 1,
            [Description("WuZhou")]
            五洲仓 = 2,
            [Description("ZhuoDing")]
            卓鼎仓 = 3,
            [Description("XingYe")]
            兴业仓 = 4,
            [Description("ShaTian")]
            沙田仓 = 5,
            [Description("JuMei")]
            聚美 = 6,
            [Description("FuJian")]
            福建 = 7,
            [Description("ManJiangHong")]
            满江红 = 8,
            [Description("TongYong")]
            通用 = 10
        }

        /// <summary>
        /// 供应链代码
        /// </summary>
        ///  <remarks>2015-10-12 杨浩 创建</remarks>
        public enum 供应链代码
        {
            客比邻 = 10,
            跨境翼 = 20,
            前海洋行 = 30,
            海豚 = 40,
            啪啪购 = 50,
            七号洋行=60,
            又一城 = 70,
        }
        /// <summary>
        /// 物流企业代码
        /// </summary>
        ///  <remarks>2015-10-12 杨浩 创建</remarks>
        public enum 物流代码
        {
            心怡 = 10,
            威时沛 = 20,
            海淘一号仓 = 30,
            [Description("广州机场有信达BBC")]
            有信达 = 40,
            广州机场中远BC = 60,
            广州机场有信达BC = 70,
            广州恒汇生鲜BBC = 80,
            重庆玛斯特BBC = 90,
            广州EMS = 100,
            E贸易 = 110,
            八达通 = 120,
            [Description("高捷BC")]
            高捷 = 130,
            高捷个人物品=131,
            珠海易跨境 = 140,
            联港物流 = 150,
            信营=160,
            七号洋行=170,
            东方物通科技=180,
            启邦国际物流=190,
            五洲四海商务=200,
            [Description(" 东方之箭跨境物流WCF")]
            广州桥集拉德国际货运代理 = 210,
        }
        /// <summary>
        /// 支付企业内部代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public enum PayCode
        {
            支付宝 = 6,
            微信 = 11,
            易宝 = 12,
            银联 = 5,
            通联支付 = 13,
            钱袋宝 = 16,
            易票联=17,
            开通联=18,
            汇付天下=19,
            汇聚支付=21,
        }
        /// <summary>
        /// 海关内部编号
        /// </summary>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public enum 海关
        {
            广州机场海关 = 0,
            广州南沙海关 = 1,
            深圳前海海关 = 2,
            重庆西永海关 = 3,
            重庆渝北海关 = 4,
            郑州海关 = 5,
            天津海关 = 6,
            上海海关 = 7,
            海关总署 = 8,
            [Description("跨境3.0")]
            跨境3=9,
            [Description("开联通企业报关平台")]
            开联通=90,
            深圳跨境贸易电子商务通关服务平台=10,

        }
        /// <summary>
        /// 商检内部编号
        /// </summary>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public enum 商检
        {
            [Description("广州南沙")]
            广州南沙 = 0,
            广州白云机场 = 2,
            深圳 = 3,
            宁波 = 4,
            重庆 = 5,
            郑州 = 6,
            天津 = 7,
            上海 = 8,
        }
        /// <summary>
        /// 送货时间
        /// </summary>
        /// <remarks>2013-08-29 黄波 创建</remarks>
        public enum DeliveryTime
        {
            /// <summary>
            /// 一周之内全天可送达
            /// </summary>
            [Description("一周之内全天可送达")]
            EveryDay = 10,
            /// <summary>
            /// 周一到周五送货
            /// </summary>
            [Description("周一至周五送货")]
            WorkDay = 20,
            /// <summary>
            /// 双休日及公众假期送货
            /// </summary>
            [Description("双休日及公众假期送货")]
            HoliDay = 30,
        }

        /// <summary>
        /// lucene产品索引排序类型
        /// </summary>
        /// <remarks>2013-09-13 邵斌 创建</remarks>
        public enum LuceneProductSortType
        {
            销量 = 1,
            价格 = 2,
            评分 = 3,
            上架时间 = 4,
            默认匹配度 = 5
        }

        public  enum 电商平台
        {
            广州华迅捷通电子商务有限公司=10,
        }
        /// <summary>
        /// 公用状态
        /// </summary>
        /// <remarks>2017-11-04 杨浩 创建</remarks>
        public enum 状态
        {
            成功=10,
            失败=20,
            作废=-10,
        }


    }
}
