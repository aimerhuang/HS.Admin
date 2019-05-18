using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.LevelPoint;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Exception;
using Hyt.BLL.Extras;

namespace Hyt.BLL.LevelPoint
{
    /// <summary>
    /// 积分业务
    /// </summary>
    /// <remarks>2013-07-01 吴文强 创建</remarks>
    /// <remarks>2013-07-10 黄波 重构</remarks>
    public class PointBo : BOBase<PointBo>
    {
        #region 等级积分日志
        /// <summary>
        /// 查看会员等级积分日志最新一条记录
        /// </summary>
        /// <param name="customerysno">客户编号</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public CrLevelPointLog GetLevelPointList(int customerysno)
        {
            return IPointDao.Instance.GetLevelPointList(customerysno);
        }

        /// <summary>
        /// 获取最后一次一条经验积分日志
        /// </summary>
        /// <param name="customerysno">客户编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public CrExperiencePointLog GetExperiencePointLog(int customerysno)
        {
            return IPointDao.Instance.GetExperiencePointLog(customerysno);
        }

        /// <summary>
        /// 获取规定日期范围发内的经验积分日志列表
        /// </summary>
        /// <param name="customerysno">客户编号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>经验积分日志列表</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public IList<CrExperiencePointLog> GetExperiencePointLog(int customerysno, DateTime beginTime, DateTime endTime)
        {
            return IPointDao.Instance.GetExperiencePointLog(customerysno, beginTime, endTime);
        }

        /// <summary>
        /// 汇总客等级积分日志 增加积分、减少积分
        /// </summary>
        /// <param name="customerysno">客户编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public CBCrLevelPointLog GetLevelPointLog(int customerysno, DateTime startTime, DateTime endTime)
        {
            return IPointDao.Instance.GetLevelPointLog(customerysno, startTime, endTime);
        }
        #endregion

        #region 获取实体

        /// <summary>
        /// 查看会员等级
        /// </summary>
        /// <param name="sysNo">等级编号</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CBCrLevelLog GetLevelLogModel(int sysNo)
        {
            return IPointDao.Instance.GetLevelLogModel(sysNo);
        }

        /// <summary>
        /// 查看会员等级积分日志
        /// </summary>
        /// <param name="sysNo">等级积分日志编号</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CBCrLevelPointLog GetGetLevelPointLogModel(int sysNo)
        {
            return IPointDao.Instance.GetLevelPointLogModel(sysNo);
        }

        /// <summary>
        /// 查看经验积分日志
        /// </summary>
        /// <param name="sysNo">经验积分日志编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CBCrExperiencePointLog GetCrExperiencePointLogModel(int sysNo)
        {
            return IPointDao.Instance.GetCrExperiencePointLogModel(sysNo);
        }
        /// <summary>
        /// 查看用户积分日志
        /// </summary>
        /// <param name="sysNo">积分日志编号</param>
        /// <returns>积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CrAvailablePointLog GetCrAvailablePointLogModel(int sysNo)
        {
            return IPointDao.Instance.GetCrAvailablePointLogModel(sysNo);
        }
        /// <summary>
        /// 获取惠源币日志模型
        /// </summary>
        /// <param name="sysNo">惠源币日志系统编号</param>
        /// <returns>惠源币日志模型</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        public CBCrExperienceCoinLog GetCbCrExperienceCoinLog(int sysNo)
        {
            return IPointDao.Instance.GetCbCrExperienceCoinLog(sysNo);
        }
        #endregion

        #region 获取日志

        /// <summary>
        /// 获取惠源币日志
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="changeType">惠源币更改类型</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>惠源币日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 杨晗 修改</remarks>
        public PagedList<CBCrExperienceCoinLog> GetExperienceCoinLog(int customerSysNo, int? changeType, int? pageIndex)
        {
            var returnValue = new PagedList<CBCrExperienceCoinLog>();

            var pager = new Pager<CBCrExperienceCoinLog>
                {
                    PageFilter = new CBCrExperienceCoinLog
                        {
                            CustomerSysNo = customerSysNo,
                            ChangeType = changeType ?? 0
                        },
                    CurrentPage = pageIndex ?? 1,
                    PageSize = returnValue.PageSize
                };
            IPointDao.Instance.GetExperienceCoinLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        /// <summary>
        /// 获取经验积分日志
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="pointType">经验积分更改类型</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public PagedList<CBCrExperiencePointLog> GetExperiencePointLog(int customerSysNo, int? pointType, int? pageIndex)
        {
            if (pointType == null)
            {
                pointType = -1;
            }
            var returnValue = new PagedList<CBCrExperiencePointLog>();

            var pager = new Pager<CBCrExperiencePointLog>
            {
                PageFilter = new CBCrExperiencePointLog
                {
                    CustomerSysNo = customerSysNo,
                    PointType = pointType ?? 0
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            Hyt.DataAccess.LevelPoint.IPointDao.Instance.GetExperiencePointLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        /// <summary>
        /// 获取用户积分日志
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="pointType">用户积分更改类型</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>用户积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public PagedList<CrAvailablePointLog> GetCrAvailablePointLog(int customerSysNo, int? pointType, int? pageIndex)
        {
            if (pointType == null)
            {
                pointType = -1;
            }
            var returnValue = new PagedList<CrAvailablePointLog>();

            var pager = new Pager<CrAvailablePointLog>
            {
                PageFilter = new CrAvailablePointLog
                {
                    CustomerSysNo = customerSysNo,
                    PointType = pointType ?? 0
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            Hyt.DataAccess.LevelPoint.IPointDao.Instance.GetCrAvailablePointLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        /// <summary>
        /// 获取等级积分日志
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="changeType">等级积分更改类型</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public PagedList<CBCrLevelPointLog> GetLevelPointLog(int customerSysNo, int? changeType, int? pageIndex)
        {
            if (changeType == null)
            {
                changeType = -1;
            }
            var returnValue = new PagedList<CBCrLevelPointLog>();

            var pager = new Pager<CBCrLevelPointLog>
            {
                PageFilter = new CBCrLevelPointLog
                {
                    CustomerSysNo = customerSysNo,
                    ChangeType = changeType ?? 0
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            Hyt.DataAccess.LevelPoint.IPointDao.Instance.GetLevelPointLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        /// <summary>
        /// 获取等级日志
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="changeType">等级日志更改类型</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public PagedList<CBCrLevelLog> GetLevelLog(int customerSysNo, int? changeType, int? pageIndex)
        {
            //if (changeType ==null)
            //{
            //    changeType = -1;
            //}
            var returnValue = new PagedList<CBCrLevelLog>();

            var pager = new Pager<CBCrLevelLog>
            {
                PageFilter = new CBCrLevelLog
                {
                    CustomerSysNo = customerSysNo,
                    ChangeType = changeType ?? -1
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };

            Hyt.DataAccess.LevelPoint.IPointDao.Instance.GetLevelLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        #endregion

        #region 积分 惠源币转换
        /// <summary>
        /// 金额转积分
        /// 1:1 (获取扣回都是舍去小数位)
        /// </summary>
        /// <param name="amount">金额</param>
        /// <returns>积分数量</returns>
        /// <remarks>2013-09-10 黄波 创建</remarks>
        /// <remarks>2013-10-28 吴文强 修改，更新1:1</remarks>
        public int MoneyToPoint(decimal amount)
        {
            const decimal conversionRate = 1M;
            return (int)Math.Floor(amount / conversionRate);
        }

        /// <summary>
        /// 积分转金额(退货时,现金补偿,公司有点小亏)
        /// </summary>
        /// <param name="point">积分</param>
        /// <returns>金额</returns>
        /// <remarks>2013-09-10 黄波 创建</remarks>
        public decimal PointToMoney(int point)
        {
            const decimal conversionRate = 50M;
            return Math.Floor(point / conversionRate);
        }

        /// <summary>
        /// 金额转惠源币
        /// 1:1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <returns>惠源币数量</returns>
        /// <remarks>2013-09-10 黄波 创建</remarks>
        /// <remarks>2013-10-28 吴文强 修改，更新1:1</remarks>
        public decimal MoneyToExperienceCoin(decimal amount)
        {
            const int conversionRate = 1;
            return amount / conversionRate;
        }

        /// <summary>
        /// 惠源币转金额
        /// 1:1
        /// </summary>
        /// <param name="experienceCoinQuantity">惠源币数量</param>
        /// <returns>金额</returns>
        /// <remarks>2013-09-10 黄波 创建</remarks>
        public decimal ExperienceCoinToMoney(decimal experienceCoinQuantity)
        {
            var conversionRate = 1;
            return experienceCoinQuantity / conversionRate;
        }

        /// <summary>
        /// 积分转惠源币
        /// 50:1
        /// </summary>
        /// <param name="point">转换的积分数</param>
        /// <param name="modPoint">兑换后剩余的积分数</param>
        /// <returns>惠源币数量</returns>
        /// <remarks>2013-10-17 黄波 创建</remarks>
        public int PointToCoin(int point, ref int modPoint)
        {
            var conversionRate = 50M;

            modPoint = (int)(point % conversionRate);
            int a = (int)Math.Floor((decimal)point / conversionRate);
            return (int)Math.Floor((decimal)point / conversionRate);
        }
        #endregion

        #region 订单积分/惠源币相关
        /// <summary>
        /// 订单送积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="point">积分数量(正整数)</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void OrderIncreasePoint(int customerSysNo, int orderSysNo, int point, string transactionSysNo)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);

            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (string.IsNullOrWhiteSpace(transactionSysNo))
            {
                transactionSysNo = Guid.NewGuid().ToString("N");
            }
            point = Math.Abs(point);

            try
            {
                var logMessage = "交易获得,订单号:" + BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo).OrderNo;
                if (!CheckExperiencePointIsFixed(customerModel) && !HasExperiencePoint((int)CustomerStatus.经验积分变更类型.交易变更, transactionSysNo))
                {
                    UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.交易变更, point, logMessage, transactionSysNo);
                    //获取订单地址信息
                    SoReceiveAddress srEnity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(orderSysNo);
                    //2016-1-18 王耀发 屏蔽
                    //SmsBO.Instance.发送购物获得积分短信(srEnity.MobilePhoneNumber, point);
                }

                if (!CheckLevelIsFixed(customerModel) && !HasLevelPoint((int)CustomerStatus.等级积分日志变更类型.交易变更, transactionSysNo))
                {
                    UpdateLevelPoint(customerModel, 0, CustomerStatus.等级积分日志变更类型.交易变更, point, logMessage, transactionSysNo);
                }
            }
            catch { }
        }

        /// <summary>
        /// 防止重复增加
        /// </summary>
        /// <param name="pointType"></param>
        /// <param name="transactionSysNo"></param>
        /// <returns></returns>
        public bool HasExperiencePoint(int pointType, string transactionSysNo)
        {
            return IPointDao.Instance.HasExperiencePoint(pointType, transactionSysNo);
        }

        /// <summary>
        /// 防止重复增加
        /// </summary>
        /// <param name="pointType"></param>
        /// <param name="transactionSysNo"></param>
        /// <returns></returns>
        public bool HasLevelPoint(int pointType, string transactionSysNo)
        {
            return IPointDao.Instance.HasLevelPoint(pointType, transactionSysNo);
        }

        /// <summary>
        /// 订单消费惠源币
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="amount">消费数量 不能为0</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <exception cref="Exception"></exception>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void OrderDeductionExperienceCoin(int customerSysNo, int orderSysNo, int amount, string transactionSysNo)
        {
            if (amount == 0) throw new Exception("惠源币数量不能为0.");
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);

            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (customerModel.ExperienceCoin < amount)
            {
                throw new NotEnoughExperienceCoinException();
            }

            if (!CheckExperienceCoinIsFixed(customerModel))
            {
                UpdateExperienceCoin(customerModel, 0, CustomerStatus.惠源币变更类型.交易变更, -Math.Abs(amount), "订单交易使用,订单号:" + orderSysNo.ToString(), transactionSysNo);
                SmsBO.Instance.发送使用惠源币购物短信(customerModel.Account, amount, amount);
            }
        }

        /// <summary>
        /// 取消订单退还惠源币
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="amount">惠源币金额</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public void CancelOrderIncreaseExperienceCoin(int customerSysNo, int orderSysNo, int amount, string transactionSysNo)
        {
            if (amount == 0) throw new Exception("惠源币数量不能为0.");
            
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (!CheckExperienceCoinIsFixed(customerModel))
            {
                UpdateExperienceCoin(customerModel, 0, CustomerStatus.惠源币变更类型.交易变更, Math.Abs(amount), "取消订单退还,订单号:" + orderSysNo.ToString(), transactionSysNo);
            }
        }

        /// <summary>
        /// 作废订单退还会员币
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="amount">惠源币金额</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public void CancelOrderIncreaseExperienceCoin(int customerSysNo, int userSysNo, int orderSysNo, int amount, string transactionSysNo)
        {
            if (amount == 0) throw new Exception("会员币数量不能为0.");
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (!CheckExperienceCoinIsFixed(customerModel))
            {
                UpdateExperienceCoin(customerModel, userSysNo, CustomerStatus.惠源币变更类型.交易变更, Math.Abs(amount), "作废订单退还,订单号:" + orderSysNo.ToString(), transactionSysNo);
            }
        }

        /// <summary>
        /// 退货减积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="amount">用户消费的惠源币数量(正整数)</param>
        /// <param name="point">积分数量(正整数)</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void RMADecreasePoint(int customerSysNo, int orderSysNo, int amount, int point, string transactionSysNo)
        {
            if (amount < 0 || point < 0)
            {
                throw new Exception("会员币或积分数不能小于0.");
            }

            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            point = -point;
            if (customerModel.ExperiencePoint + point < 0)
            {
                throw new Exception("经验积分余额不足.");
            }

            try
            {
                var logMessage = "退货扣减,订单号:" + BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo).OrderNo;
                if (!CheckExperiencePointIsFixed(customerModel))
                {
                    UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.交易变更, point, logMessage, transactionSysNo);
                }
                if (!CheckLevelIsFixed(customerModel))
                {
                    UpdateLevelPoint(customerModel, 0, CustomerStatus.等级积分日志变更类型.交易变更, point, logMessage, transactionSysNo);
                }
                if (!CheckExperienceCoinIsFixed(customerModel) && amount > 0)
                {
                    UpdateExperienceCoin(customerModel, 0, CustomerStatus.惠源币变更类型.交易变更, amount, "退货返还,订单号:" + orderSysNo.ToString(), transactionSysNo);
                }
            }
            catch { }
        }

        #endregion



        #region 活动积分相关

        /// <summary>
        /// 活动赠送积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="point">积分数量(正整数)</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void ActivityIncreasePoint(int customerSysNo, string activityName, int point)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.参与活动, Math.Abs(point), "参加\"" + activityName + "\"活动获得.", string.Empty);
            SmsBO.Instance.发送参加活动获得积分短信(customerModel.Account, activityName, point);

        }
        #endregion

        #region 用户操作积分相关

        /// <summary>
        /// 注册送经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void ResisterIncreasePoint(int customerSysNo)
        {
            int point = 10;

            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }
            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.系统赠送, point, "注册系统赠送.", string.Empty);
        }

        /// <summary>
        /// 登录送经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void LoginIncreasePoint(int customerSysNo)
        {
            int point = 10;

            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.系统赠送, point, "登录系统赠送.", string.Empty);
        }

        /// <summary>
        /// 评论送经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void CommentIncreasePoint(int customerSysNo, int productSysNo)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            int point = 10;
            var topFive = false;
            var productStatistics = BLL.Web.PdProductBo.Instance.GetProductStatistics(productSysNo);

            //前五个评论双倍积分
            if (productStatistics != null && productStatistics.Comments < 5)
            {
                topFive = true;
                point = point * 2;
            }

            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.系统赠送, point, string.Format("发表商品评论获得{0}.", topFive ? "(前五位双倍积分)" : ""), string.Empty);
            SmsBO.Instance.发送评价商品获得积分短信(customerModel.Account, point);
        }

        /// <summary>
        /// 晒单送经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void ShareOrderIncreasePoint(int customerSysNo, int orderSysNo)
        {
            int point = 10;

            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.系统赠送, point, "对订单号:" + orderSysNo.ToString() + "晒单获得.", string.Empty);
            SmsBO.Instance.发送晒单获得积分短信(customerModel.Account, point);
        }

        /// <summary>
        /// 完善个人信息送经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void PersonalInfomationIncreasePoint(int customerSysNo)
        {
            int point = 10;

            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            UpdateExperiencePoint(customerModel, 0, CustomerStatus.经验积分变更类型.系统赠送, point, "完善个人信息获得.", string.Empty);
            SmsBO.Instance.发送完善个人资料获得积分短信(customerModel.Account, point);
        }

        public void OutLineSellOrderPoint()
        {

        }

        #endregion

        #region 客服直接调整

        /// <summary>
        /// 调整惠源币
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="amount">惠源币数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Hyt.Model.Exception.UserNotMatchException"></exception>
        /// <returns>调整结果</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void AdjustExperienceCoin(int customerSysNo, int userSysNo, int amount, string description)
        {
            if (amount == 0)
            {
                throw new Exception("惠源币数量不能为0.");
            }
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }
            if (customerModel.ExperienceCoin + amount < 0)
            {
                throw new Exception("惠源币数量不能小于0.");
            }
            UpdateExperienceCoin(customerModel, userSysNo, CustomerStatus.惠源币变更类型.充值, amount, description, string.Empty);
        }

        /// <summary>
        /// 调整经验积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="point">积分数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <returns>调整结果</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void AdjustExperiencePoint(int customerSysNo, int userSysNo, int point, string description)
        {
            if (point == 0)
            {
                throw new Exception("积分数量不能为0.");
            }
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }
            if (customerModel.ExperiencePoint + point < 0)
            {
                throw new Exception("用户经验积分不能小于0.");
            }
            UpdateExperiencePoint(customerModel, userSysNo, CustomerStatus.经验积分变更类型.客服调整, point, description, string.Empty);
        }

        /// <summary>
        /// 调整等级积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="point">积分数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void AdjustLevelPoint(int customerSysNo, int userSysNo, int point, string description)
        {
            if (point == 0)
            {
                throw new Exception("积分数量不能为0.");
            }
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }
            if (customerModel.LevelPoint + point < 0)
            {
                throw new Exception("用户等级积分不能小于0.");
            }
            UpdateLevelPoint(customerModel, userSysNo, CustomerStatus.等级积分日志变更类型.客服调整, point, description, string.Empty);
        }

        #endregion

        #region 使用积分
        /// <summary>
        /// 积分转换惠源币
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="point">转换数量(具体扣除的数量根据兑换比例向下取整)</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <remarks>2013-10-30 黄波 创建</remarks>
        public void AvailablePointConvertToExperienceCoin(int customerSysNo, int point, string transactionSysNo)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            if (customerModel.AvailablePoint < point)
            {
                throw new Exception("可用积分余额不足.");
            }
            var modPoint = 0;
            var experienceCoin = PointToCoin(point, ref modPoint);
            if (experienceCoin > 0)
            {
                UpdateExperienceCoin(customerModel, customerSysNo, CustomerStatus.惠源币变更类型.积分兑换, experienceCoin, "积分兑换", transactionSysNo);
                UpdateAvailablePoint(customerModel, customerSysNo, CustomerStatus.可用积分变更类型.积分兑换, -Math.Abs((point - modPoint)), "积分兑换", transactionSysNo);
                SmsBO.Instance.发送积分兑换惠源币短信(customerModel.Account, experienceCoin, point - modPoint);
            }
        }

        /// <summary>
        /// 活动消费积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="point">积分数量(正整数)</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        public void ActivityDecreaseAvailablePoint(int customerSysNo, string activityName, int point)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (CheckExperiencePointIsFixed(customerModel)) return;

            if (customerModel.AvailablePoint < point)
            {
                throw new Exception("可用积分余额不足.");
            }
            UpdateAvailablePoint(customerModel, 0, CustomerStatus.可用积分变更类型.参与活动, -Math.Abs(point), "参加\"" + activityName + "\"活动使用.", string.Empty);
            SmsBO.Instance.发送参加活动使用积分短信(customerModel.Account, activityName, point);
        }
        #endregion

        #region 调整积分
        /// <summary>
        /// 调整会员币
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">惠源币变更类型</param>
        /// <param name="amount">会员币数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        private void UpdateExperienceCoin(CrCustomer customer, int userSysNo, CustomerStatus.惠源币变更类型 changeType, int amount, string description, string transactionSysNo)
        {
            var customerSysNo = customer.SysNo;

            var model = new CrExperienceCoinLog
            {
                TransactionSysNo = transactionSysNo,
                CustomerSysNo = customerSysNo,
                ChangeDate = DateTime.Now,
                ChangeDescription = description,
                ChangeType = (int)changeType,
                CreatedBy = userSysNo,
                CreatedDate = DateTime.Now,
                Surplus = customer.ExperienceCoin + amount,
                Increased = amount > 0 ? amount : 0,
                Decreased = amount > 0 ? 0 : amount
            };

            //更新用户会员币
            IPointDao.Instance.AdjustExperienceCoin(customerSysNo, amount, model);
        }

        /// <summary>
        /// 调整经验积分
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">经验积分变更类型</param>
        /// <param name="point">积分数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        private void UpdateExperiencePoint(CrCustomer customer, int userSysNo, CustomerStatus.经验积分变更类型 changeType, int point, string description, string transactionSysNo)
        {
            var customerSysNo = customer.SysNo;
            var nowDate = DateTime.Now;
            var experiencePointCahngeType = (int)changeType;
            var availablePointChangeType = 0;
            if (experiencePointCahngeType == (int)CustomerStatus.经验积分变更类型.参与活动)
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.参与活动;
            }
            else if (experiencePointCahngeType == (int)CustomerStatus.经验积分变更类型.过期调整)
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.过期调整;
            }
            else if (experiencePointCahngeType == (int)CustomerStatus.经验积分变更类型.积分兑换)
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.积分兑换;
            }
            else if (experiencePointCahngeType == (int)CustomerStatus.经验积分变更类型.交易变更)
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.交易变更;
            }
            else if (experiencePointCahngeType == (int)CustomerStatus.经验积分变更类型.客服调整)
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.客服调整;
            }
            else
            {
                availablePointChangeType = (int)CustomerStatus.可用积分变更类型.系统赠送;
            }

            var experiencePointLogmodel = new CrExperiencePointLog
                {
                    TransactionSysNo = transactionSysNo,
                    CustomerSysNo = customerSysNo,
                    ChangeDate = nowDate,
                    CreatedBy = userSysNo,
                    CreatedDate = nowDate,
                    Surplus = customer.ExperiencePoint + point,
                    PointType = experiencePointCahngeType,
                    PointDescription = description,
                    Increased = point > 0 ? point : 0,
                    Decreased = point > 0 ? 0 : point
                };
            var availablePointLogModel = new CrAvailablePointLog
            {
                TransactionSysNo = transactionSysNo,
                CustomerSysNo = customerSysNo,
                ChangeDate = nowDate,
                CreatedBy = userSysNo,
                CreatedDate = nowDate,
                Surplus = customer.AvailablePoint + point,
                PointType = availablePointChangeType,
                PointDescription = description,
                Increased = point > 0 ? point : 0,
                Decreased = point > 0 ? 0 : point
            };

            IPointDao.Instance.AdjustExperiencePoint(customerSysNo, point, experiencePointLogmodel, availablePointLogModel);
        }

        /// <summary>
        /// 调整可用积分
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">积分变更类型</param>
        /// <param name="point">积分数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public void UpdateAvailablePoint(CrCustomer customer, int userSysNo, CustomerStatus.可用积分变更类型 changeType, int point, string description, string transactionSysNo)
        {
            if(string.IsNullOrEmpty(description))
            {
                description = "积分兑换";
            }
            var nowDate = DateTime.Now;
            var crAvailablePointLogmodel = new CrAvailablePointLog
            {
                TransactionSysNo = transactionSysNo,
                CustomerSysNo = customer.SysNo,
                ChangeDate = nowDate,
                CreatedBy = userSysNo,
                CreatedDate = nowDate,
                Surplus = customer.AvailablePoint + point,
                PointType = (int)changeType,
                PointDescription = description,//"积分兑换",
                Increased = point > 0 ? point : 0,
                Decreased = point > 0 ? 0 : point
            };
            IPointDao.Instance.UpdateAvailablePoint(customer.SysNo, point, crAvailablePointLogmodel);
        }

        /// <summary>
        /// 调整等级积分
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">等级积分变更类型</param>
        /// <param name="point">积分数量(正数:增加;负数:减少)</param>
        /// <param name="description">变更说明</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        private void UpdateLevelPoint(CrCustomer customer, int userSysNo, CustomerStatus.等级积分日志变更类型 changeType, int point, string description, string transactionSysNo)
        {
            var customerSysNo = customer.SysNo;

            var levelPointModel = new CrLevelPointLog
            {
                ChangeDescription = description,
                ChangeType = (int)changeType,
                LastUpdateDate = DateTime.Now,
                CreatedBy = userSysNo,
                LastUpdateBy = userSysNo,
                CreatedDate = DateTime.Now,
                CustomerSysNo = customerSysNo,
                Increased = point > 0 ? point : 0,
                Decreased = point > 0 ? 0 : point,
                TransactionSysNo = transactionSysNo
            };

            //更新等级积分并记录日志
            IPointDao.Instance.AdjustLevelPoint(customerSysNo, point, levelPointModel);
            //更新等级
            IPointDao.Instance.UpdateCustomerLevel(customerSysNo, userSysNo, changeType, description);
        }

        #endregion

        #region 验证订单惠源币支付比例

        /// <summary>
        /// 验证等级惠源币支付比例是否有效
        /// </summary>
        /// <param name="cart">购物车对象</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="experienceCoin">惠源币</param>
        /// <returns>是否有效</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public bool ExperienceCoinScaleIsValid(Hyt.Model.CrShoppingCart cart, int customerSysNo, int experienceCoin)
        {
            return experienceCoin <= SettleAccountsUseExperienceCoinQuantity(cart, customerSysNo);
        }

        /// <summary>
        /// 验证等级惠源币支付比例是否有效
        /// </summary>
        /// <param name="cart">购物车对象</param>
        /// <param name="customerModel">客户实体</param>
        /// <param name="experienceCoin">惠源币</param>
        /// <returns>是否有效</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public bool ExperienceCoinScaleIsValid(Hyt.Model.CrShoppingCart cart, CrCustomer customerModel, int experienceCoin)
        {
            return experienceCoin <= SettleAccountsUseExperienceCoinQuantity(cart, customerModel);
        }

        /// <summary>
        /// 计算订单能使用多少惠源币
        /// </summary>
        /// <param name="cart">购物车对象</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>惠源币数量</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public int SettleAccountsUseExperienceCoinQuantity(Hyt.Model.CrShoppingCart cart, int customerSysNo)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            return SettleAccountsUseExperienceCoinQuantity(cart, customerModel);
        }

        /// <summary>
        /// 计算订单能使用多少惠源币
        /// </summary>
        /// <param name="cart">购物车对象</param>
        /// <param name="customerModel">客户实体</param>
        /// <returns>惠源币数量</returns>
        /// <remarks>2014-1-3 黄波 创建</remarks>
        public int SettleAccountsUseExperienceCoinQuantity(Hyt.Model.CrShoppingCart cart, CrCustomer customerModel)
        {
            var allowMaxExperienceCoin = 0;
            try
            {
                //判断顾客是否能使用惠源币
                if (customerModel.IsExperienceCoinFixed == 1) return 0;

                var level = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCustomerLevel(customerModel.LevelSysNo);

                if (level.CanPayForProduct == 1)//惠源币可用于支付货款
                {
                    var productPaymentUseExperienceCoin = (int)Math.Floor(((decimal)(cart.SettlementAmount - (cart.FreightAmount - cart.FreightDiscountAmount)) * level.ProductPaymentPercentage / 100));
                    allowMaxExperienceCoin += Math.Min(productPaymentUseExperienceCoin, level.ProductPaymentUpperLimit);
                }
                if (level.CanPayForService == 1)//惠源币可用于支付服务
                {
                    var servicePaymentUseExperienceCoin = (int)Math.Floor(((decimal)(cart.FreightAmount - cart.FreightDiscountAmount) * level.ServicePaymentPercentage / 100));
                    allowMaxExperienceCoin += Math.Min(servicePaymentUseExperienceCoin, level.ServicePaymentUpperLimit);
                }
            }
            catch (Exception ex)
            {
                Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }

            return allowMaxExperienceCoin;
        }

        #endregion

        #region 检查用户固定项
        /// <summary>
        /// 检查用户等级是否固定
        /// </summary>
        /// <param name="customer">用户实体</param>
        /// <returns>是否</returns>
        /// <remarks>2014-03-04 黄波 创建</remarks>
        private bool CheckLevelIsFixed(CrCustomer customer)
        {
            return customer.IsLevelFixed == (int)CustomerStatus.等级是否固定.固定;
        }

        /// <summary>
        /// 检查用户惠源币是否固定
        /// </summary>
        /// <param name="customer">用户实体</param>
        /// <returns>是否</returns>
        /// <remarks>2014-03-04 黄波 创建</remarks>
        private bool CheckExperienceCoinIsFixed(CrCustomer customer)
        {
            return customer.IsExperienceCoinFixed == (int)CustomerStatus.惠源币是否固定.固定;
        }

        /// <summary>
        /// 检查用户经验积分是否固定
        /// </summary>
        /// <param name="customer">用户实体</param>
        /// <returns>是否</returns>
        /// <remarks>2014-03-04 黄波 创建</remarks>
        private bool CheckExperiencePointIsFixed(CrCustomer customer)
        {
            return customer.IsExperiencePointFixed == (int)CustomerStatus.经验积分是否固定.固定;
        }

        
        #endregion

        /// <summary>
        /// 帐号积分是否可以使用
        /// </summary>
        /// <param name="customer">用户实体</param>
        /// <returns>是否</returns>
        /// <remarks>2014-03-07 黄波 创建</remarks>
        public bool AccountPointDisable(CrCustomer customer)
        {
            return customer.IsExperiencePointFixed == (int)CustomerStatus.经验积分是否固定.固定 || customer.IsExperienceCoinFixed == (int)CustomerStatus.惠源币是否固定.固定;
        }

        /// <summary>
        /// 退货返还积分
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="orderSysNo"></param>
        /// <param name="amount"></param>
        /// <param name="transactionSysNo"></param>
        /// <remarks>2016-07-14 陈海裕 创建</remarks>
        public void ReturnAvailablePoint(int customerSysNo, int orderSysNo, int amount, string transactionSysNo)
        {
            var customerModel = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);

            if (customerModel == null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (!CheckExperienceCoinIsFixed(customerModel))
            {
                try
                {
                    UpdateAvailablePoint(customerModel, 0, CustomerStatus.可用积分变更类型.交易变更, -Math.Abs(amount), "取消订单,返还积分,订单号:" + BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo).OrderNo, transactionSysNo);
                }
                catch { }
            }
        }
        /// <summary>
        /// 会员币转积分
        /// </summary>
        /// <param name="coin">会员币</param>
        /// <returns></returns>
        /// <remarks>2016-12-8 杨浩 创建</remarks>
        public int CoinToPoint(int coin)
        {
            int conversionRate = BLL.Config.Config.Instance.GetGeneralConfig().PointToCoinRate;
            return coin * conversionRate;
        }
    }
}