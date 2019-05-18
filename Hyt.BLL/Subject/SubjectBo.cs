using Hyt.BLL.Promotion;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Subject
{
    /// <summary>
    /// 主题业务逻辑
    /// </summary>
    /// <remarks>2013-12-30 黄波 创建</remarks>
    public class SubjectBo : BOBase<SubjectBo>
    {
        #region 抽奖
        /// <summary>
        /// 缓存关键字
        /// </summary>
        private string cacheKey = "prizeList_2014_1_{0}_{1}";//{0}号  {1}小时

        /// <summary>
        /// 没有奖项的编号容器
        /// </summary>
        private int[] invalidNumberWrap = new int[] { 1, 5, 7, 9 };

        /// <summary>
        /// 永远不能抽中的号码容器
        /// </summary>
        private int[] disableNumberWrap = new int[] { 2, 4 };

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="status">状态(1:不能抽奖 2:已抽 3:中奖 4:未中)</param>
        /// <param name="lotteryNumber">抽奖号码</param>
        /// <param name="message">消息</param>
        /// <param name="prizeSign">奖品标识</param>
        /// <param name="customerSysNo">用户编号(登录才传)</param>
        /// <returns>是否中奖</returns>
        /// <remarks>2013-12-30 黄波 创建</remarks>
        public void Lottery(out int status, out int lotteryNumber, out string message, ref string prizeSign, ref int prizeType, int customerSysNo = -1)
        {
            message = "";
            status = 1;
            lotteryNumber = GetLotteryNumber(false);

            var beginDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = new DateTime(2014, 1, 25, 0, 0, 0);
            var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
            #region 判断活动是否在有效期内
            //如果当前时间小于活动开始时间 或者 大于等于结束时间
            if (nowDate < beginDate)
            {
                status = 1;
                message = "活动还未开始!";
                lotteryNumber = GetLotteryNumber(false);
                return;
            }
            if (nowDate >= endDate)
            {
                status = 1;
                message = "活动已结束!";
                lotteryNumber = GetLotteryNumber(false);
                return;
            }
            #endregion

            #region 判断是否可以抽奖

            if (!CheckLetteryDate(nowDate))
            {
                status = 1;
                message = "抽奖在10-21点整点开始，开始后10分钟内可以抽奖！";
                lotteryNumber = GetLotteryNumber(false);
                return;
            }

            if (customerSysNo != -1)
            {
                if (Hyt.BLL.Promotion.SpCouponReceiveLogBo.Instance.HasGet(customerSysNo, "201401", nowDate))
                {
                    status = 2;
                    lotteryNumber = GetLotteryNumber(false);
                    return;
                }
            }
            #endregion

            var prizeList = GetPrizeList(nowDate.Day, nowDate.Hour);
            #region 获取奖池奖品以及数量

            //检查是否有奖可抽
            if (!prizeList.Any(o => { return o.PrizeNumber > 0; }))
            {
                status = 1;
                message = "奖池已被洗劫一空,请下个抽奖时段再来试试!";
                lotteryNumber = GetLotteryNumber(false);
                return;
            }
            #endregion

            #region 摇奖
            //奖号与奖品类型对应  奖号  类型
            var t_lotteryNumber = GetLotteryNumber(true);
            lotteryNumber = t_lotteryNumber;

            var prize = prizeList.Find(o => { return o.Number == t_lotteryNumber; });
            if (prize != null && prize.PrizeNumber > 0)
            {
                status = 3;
                prizeType = prize.PrizeType;
                prizeSign = Guid.NewGuid().ToString("N");
                Hyt.Infrastructure.Caching.CacheManager.Instance.Set(prizeSign, new LotterySign() { CouponSysNo = prize.CouponSysNo }, nowDate.AddDays(1));
                prize.PrizeNumber = prize.PrizeNumber - 1;
                SetPrize(nowDate.Day, nowDate.Hour, prizeList);
            }
            else
            {
                status = 4;
                lotteryNumber = GetLotteryNumber(false);
            }
            #endregion
        }

        /// <summary>
        /// 获取摇奖号码
        /// </summary>
        /// <param name="ernie">是否摇奖 如果为false,则永远返回invalidNumberWrap中的号码</param>
        /// <returns>摇奖号码</returns>
        /// <remarks>2013-12-30 黄波 创建</remarks>
        private int GetLotteryNumber(bool ernie = true)
        {
            var lotteryNumber = 0;
            if (ernie)
            {
                var lotteryRange = 20;//数字越大,几率越小 不能小于11

                lotteryNumber = new Random().Next(1, lotteryRange);//摇奖
                while (disableNumberWrap.Contains(lotteryNumber))
                {
                    lotteryNumber = new Random().Next(1, lotteryRange);
                }
            }
            if (lotteryNumber > 10 || !ernie)
            {
                lotteryNumber = invalidNumberWrap[new Random().Next(0, invalidNumberWrap.Length)];
            }
            return lotteryNumber;
        }

        /// <summary>
        /// 判断时间是否在抽奖范围
        /// </summary>
        /// <param name="dateTime">当前时间</param>
        /// <returns></returns>
        public bool CheckLetteryDate(DateTime dateTime)
        {
            var lotteryHour = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, -1, -1 };//开始时间段
            if (lotteryHour[dateTime.Hour] != dateTime.Hour || dateTime.Minute > 10)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从缓存获取某天某时段奖池剩余的奖品
        /// </summary>
        /// <param name="day">号数</param>
        /// <param name="hour">小时</param>
        /// <returns>奖品列表</returns>
        /// <remarks>2013-12-30 黄波 创建</remarks>
        private List<LotteryNumberWarp> GetPrizeList(int day, int hour)
        {
            return Hyt.Infrastructure.Caching.CacheManager.Get<List<LotteryNumberWarp>>(string.Format(cacheKey, day, hour), () =>
            {
                //优惠卷类型 优惠卷数量
                return Hyt.BLL.Config.Config.Instance.GetConfig<List<LotteryNumberWarp>>("LotteryNumberWarp.config");
            });
        }

        /// <summary>
        /// 设置奖品缓存
        /// </summary>
        /// <param name="day">号数</param>
        /// <param name="hour">小时</param>
        /// <param name="value">奖品对象</param>
        /// <returns></returns>
        /// <remarks>2013-12-30 黄波 创建</remarks>
        private void SetPrize(int day, int hour, List<LotteryNumberWarp> value)
        {
            Hyt.Infrastructure.Caching.CacheManager.Instance.Set(string.Format(cacheKey, day, hour), value);
        }

        /// <summary>
        /// 为客户分配优惠卷
        /// </summary>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>分配成功后的优惠卷新编号</returns>
        /// <remarks>2013/12/05 朱家宏 创建</remarks>
        /// <remarks>2013/12/31 黄波 复制</remarks>
        public int AssignToCustomer(int couponSysNo, int customerSysNo)
        {
            /*
             * 1.查询条件：所选优惠卷为系统、优惠卷状态为已审核、结束日期大于当前、允许使用数量>0
             * 
             * 2.复制后初始数据：优惠卷类型为私有，允许使用数量为1，已使用数量为0，客户系统编号为指定客户，生成新的优惠卷随机代码，
             *   状态为待审核，审核人、创建人为当前用户，审核日期、创建日期为当前日期
             * 
             * 3.更新源优惠卷：已使用数量+1，更新人为当前用户，更新日期为当前日期
             * 
             * 4.完成复制后，对该优惠卷仅提供审核和作废
             * 
             * 5.操作权限，具备优惠卷操作权限的用户，或优惠卷的创建用户
             */
            var newCouponSysNo = 0;
            var coupon = PromotionBo.Instance.GetEntity(couponSysNo);

            #region 数据校验

            if (coupon.Type != (int)PromotionStatus.优惠券类型.系统 ||
                coupon.Status != (int)PromotionStatus.优惠券状态.已审核 ||
                coupon.EndTime < DateTime.Now ||
                coupon.UseQuantity <= 0 ||
                coupon.UsedQuantity >= coupon.UseQuantity)
            {
                return newCouponSysNo;
            }

            #endregion

            #region 复制优惠卷，初始数据

            var newCoupon = new SpCoupon
            {
                SysNo = 0,
                Type = (int)PromotionStatus.优惠券类型.私有,
                UseQuantity = 1,
                UsedQuantity = 0,
                CustomerSysNo = customerSysNo,
                CouponCode = Hyt.BLL.Promotion.SpCouponBo.Instance.GenerateNewCouponCode(),
                Status = (int)PromotionStatus.优惠券状态.已审核,

                CouponAmount = coupon.CouponAmount,
                Description = coupon.Description,
                StartTime = coupon.StartTime,
                EndTime = coupon.EndTime,
                ParentSysNo = coupon.ParentSysNo,
                PromotionSysNo = coupon.PromotionSysNo,
                RequirementAmount = coupon.RequirementAmount,
                SourceDescription = Model.SystemPredefined.Constant.COUPONDESCRIPTION_BINDUSERCOUPON,
                IsCouponCard = coupon.IsCouponCard,
                //2014-01-07 朱家宏 添加：优惠卷表新增字段
                WebPlatform = coupon.WebPlatform,
                ShopPlatform = coupon.ShopPlatform,
                MallAppPlatform = coupon.MallAppPlatform,
                LogisticsAppPlatform = coupon.LogisticsAppPlatform
            };
            newCouponSysNo = PromotionBo.Instance.SaveCoupon(newCoupon, 0, coupon);

            #endregion

            #region 更新原优惠卷

            coupon.UsedQuantity++;
            PromotionBo.Instance.SaveCoupon(coupon, 0);

            #endregion

            return newCouponSysNo;
        }
        #endregion
    }

    /// <summary>
    /// 抽奖奖池信息
    /// </summary>
    /// <remarks>2013-12-31 黄波 创建</remarks>
    [Serializable]
    public class LotteryNumberWarp
    {
        /// <summary>
        /// 抽奖号码
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 奖品类型
        /// </summary>
        public int PrizeType { get; set; }

        /// <summary>
        /// 奖品数量
        /// </summary>
        public int PrizeNumber { get; set; }

        /// <summary>
        /// 优惠券系统编号
        /// </summary>
        public int CouponSysNo { get; set; }
    }

    /// <summary>
    /// 抽奖标记
    /// </summary>
    /// <remarks>2013-1-2 黄波 创建</remarks>
    [Serializable]
    public class LotterySign
    {
        /// <summary>
        /// 优惠券系统编号
        /// </summary>
        public int CouponSysNo { get; set; }
    }
}