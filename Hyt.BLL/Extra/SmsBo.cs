using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.SMS;
using Hyt.DataAccess;
using Hyt.Model;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Hyt.Model.Common;

namespace Hyt.BLL.Extras
{
    /// <summary>
    /// 手机短信业务对象
    /// </summary>
    /// <remarks>
    /// 2013-02-26 杨浩 创建
    /// </remarks>
    public class SmsBO : BOBase<SmsBO>
    {
        #region 短信模板

        //短信模板缓存池
        private static readonly Dictionary<string, SmsTemplateInfo> SmsList = new Dictionary<string, SmsTemplateInfo>();

        /// <summary>
        /// 获得短信模板内容
        /// </summary>
        /// <param name="key">短信模板标示键</param>
        /// <returns></returns>
        /// <remarks>2016-1-20 杨浩 创建</remarks>
        public string GetSmsContent(string key)
        {
            if (!SmsList.Keys.Contains(key))
            {
                var templates = Hyt.BLL.Config.Config.Instance.GetSmsTemplateConfig().SmsTemplates;

                if (templates == null)
                    return "";

                var templateInfo = templates.Where(x => x.Key == key).SingleOrDefault();
                if (templateInfo != null)
                    SmsList.Add(key, templateInfo);
                else
                    return "";
                return SmsList[key].Content;
            }

            return SmsList[key].Content;
        }
        #endregion

        #region 发送一条短信

        /// <summary>
        /// 发送一条短信
        /// 先调用网关发送短信，然后将短信记录到数据库，如果发送失败，则由后台进程重发。
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="msg">70字（包含签名）一条短信，超出则按此规则分割成多条短信</param>
        /// <param name="sendTime">定时发送(精确到秒)，为空不需要定时</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建 
        /// 2014-07-30 黄波 修改 将短信发送放在TASK中执行
        /// </remarks>
        private SmsResult Send(string mobile, string msg, DateTime? sendTime)
        {
            var user = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(mobile);
            if (user != null && user.IsReceiveShortMessage == (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.否)
                return new SmsResult
                    {
                        RowCount = 0,
                        Status = SmsResultStatus.Failue
                    };
            var result = new SmsResult { Status = SmsResultStatus.Success };


            //添加判断是否为手机号 余勇 2014-06-18
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(msg) && VHelper.ValidatorRule(new Rule_Mobile(mobile)).IsPass && mobile.Length >= 11)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    try
                    {
                        result = SmsProviderFactory.CreateProvider().Send(mobile, msg, sendTime);
                        SaveToDb(mobile, msg, result);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "短信发送异常", ex);
                    }
                });
            }
            return result;
        }

        /// <summary>
        /// 发送一条短信，发送给客户，判断客户手机是否存在
        /// 先调用网关发送短信，然后将短信记录到数据库，如果发送失败，则由后台进程重发。
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="msg">短信中动态参数值多个逗号分隔</param>
        /// <param name="sendTime">定时发送(精确到秒)，为空不需要定时</param>
        /// <param name="sign">短信签名</param>
        /// <param name="type">短信类型</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>
        /// Create By Lwh 2016-3-8
        /// </remarks>
        private SmsResult SendToCustoms(string mobile, string msg, DateTime? sendTime, string sign = "", string type = "发送手机验证短信")
        {
            var user = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(mobile);

            if (user == null || user.IsReceiveShortMessage == (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.否)
                return new SmsResult
                {
                    RowCount = 0,
                    Status = SmsResultStatus.Failue
                };
            var result = new SmsResult();

            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(msg))
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Grand.Service.EC.Core.SMS.Contract.ISMSService>())
                {
                    var response = service.Channel.SendMessage(type, mobile, msg, sign);
                    if (response.IsError)
                        result.Status = SmsResultStatus.Failue;
                    else
                        result.Status = SmsResultStatus.Success;
                }
                //result = SmsProviderFactory.CreateProvider().Send(mobile, msg, sendTime, sign);
                //SaveToDb(mobile, msg, result);
            }
            return result;
        }

        /// <summary>
        /// 发送一条短信，发送给后台管理员，判断管理员手机是否存在
        /// 先调用网关发送短信，然后将短信记录到数据库，如果发送失败，则由后台进程重发。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        /// <param name="sendTime"></param>
        /// <param name="sign"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private SmsResult SendToSyUser(string mobile, string msg, DateTime? sendTime, string sign = "", string type = "发送管理员密码短信")
        {
            var userList = Hyt.BLL.Sys.SyUserBo.Instance.GetUserListWithoutSysNoList(new int[] { -1 });
            var user = userList.FirstOrDefault(u => u.MobilePhoneNumber == mobile);

            var result = new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };

            if (user == null || user.Status == (int)Hyt.Model.WorkflowStatus.SystemStatus.系统用户状态.禁用)
                return result;

            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(msg))
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Grand.Service.EC.Core.SMS.Contract.ISMSService>())
                {
                    var response = service.Channel.SendMessage(type, mobile, msg, sign);
                    if (response.IsError)
                        result.Status = SmsResultStatus.Failue;
                    else
                        result.Status = SmsResultStatus.Success;
                }
            }
            return result;
        }

        private SmsResult SendVerifycodeToShopUser(string mobile, string msg, DateTime? sendTime, string sign = "", string type = "积分门店使用验证短信")
        {
            var user = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(mobile);

            if (user == null || user.IsReceiveShortMessage == (int)Hyt.Model.WorkflowStatus.CustomerStatus.是否接收短信.否)
                return new SmsResult
                {
                    RowCount = 0,
                    Status = SmsResultStatus.Failue
                };
            var result = new SmsResult();

            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(msg))
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Grand.Service.EC.Core.SMS.Contract.ISMSService>())
                {
                    var response = service.Channel.SendMessage(type, mobile, msg, sign);
                    if (response.IsError)
                        result.Status = SmsResultStatus.Failue;
                    else
                        result.Status = SmsResultStatus.Success;
                }
                //result = SmsProviderFactory.CreateProvider().Send(mobile, msg, sendTime, sign);
                //SaveToDb(mobile, msg, result);
            }
            return result;
        }
        #endregion

        #region 把短信记录到数据库

        /// <summary>
        /// 把短信记录到数据库
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="msg">短信内容</param>
        /// <param name="result">短信结果</param>
        /// <returns></returns>
        /// <remarks>2013-02-26 罗雄伟 创建</remarks>
        private void SaveToDb(string mobile, string msg, SmsResult result)
        {
            //短信记录到数据库 陶辉

            var model = new NcSms()
            {
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
                LastUpdateBy = 0,
                LastUpdateDate = DateTime.Now,
                ExpectSendTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ErrorQuantity = result.Status == SmsResultStatus.Success ? 0 : 1,
                HandleTime = DateTime.Now,
                MobilePhoneNumber = mobile,
                SmsContent = msg,
                Status = result.Status == SmsResultStatus.Success ? (int)Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态.已发 : (int)Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态.待发,
                Priority = result.Status == SmsResultStatus.Success ? 0 : 1
            };

            Hyt.DataAccess.Notification.INcSmsDao.Instance.Create(model);
        }

        #endregion

        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">短信内容</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-10-29 黄波 创建
        /// 2016-06-13 陈海裕 修改
        /// </remarks>
        public SmsResult 发送手机短信(string mobile, string content, string sign = "")
        {
            return SendToCustoms(mobile, content, null, sign, "后台群发短信");
        }

        /// <summary>
        /// 发送手机验证短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">手机验证码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送手机验证短信(string mobile, string code, string sign = "")
        {
            string msg = GetSmsContent("发送手机验证短信");
            return Send(mobile, string.Format(msg, code), null);
        }

        /// <summary>
        /// 发送管理员密码短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">密码</param>
        /// <param name="sign">短信签名</param>
        /// <remarks>
        /// 2016-3-4 刘伟豪 创建
        /// </remarks>
        public SmsResult 发送管理员密码短信(string mobile, string code, string sign = "")
        {
            return SendToSyUser(mobile, code, null, sign, "发送管理员密码短信");
        }

        /// <summary>
        /// 发送管理员密码短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">密码</param>
        /// <param name="sign">短信签名</param>
        /// <remarks>
        /// 2016-3-4 刘伟豪 创建
        /// </remarks>
        public SmsResult 积分门店使用验证短信(string mobile, string code, string sign = "")
        {
            return SendVerifycodeToShopUser(mobile, code, null, sign, "积分门店使用验证短信");
        }

        /// <summary>
        /// 发送重置密码短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">随机码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送重置密码短信(string mobile, string code)
        {
            string msg = GetSmsContent("发送重置密码短信");
            return Send(mobile, string.Format(msg, code), null);
        }

        /// <summary>
        /// 发送新密码短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="name">用户姓名</param>
        /// <param name="password">用户密码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送新密码短信(string mobile, string name, string password)
        {
            string msg = GetSmsContent("发送新密码短信");
            return Send(mobile, string.Format(msg, password), null);
        }

        /// <summary>
        /// 发送注册成功短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="name">用户姓名</param>
        /// <param name="password">用户密码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送注册成功短信(string mobile, string name, string password)
        {
            string msg = GetSmsContent("发送注册成功短信");
            return Send(mobile, string.Format(msg, name, password), null);
        }

        /// <summary>
        /// 发送第三方物流发货通知短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送第三方物流发货通知短信(string mobile, string orderId)
        {

            string msg = GetSmsContent("发送第三方物流发货通知短信");
            return Send(mobile, string.Format(msg, orderId, "T" + orderId.PadLeft(15, '0')), null);
        }

        /// <summary>
        /// 发送自建物流发货通知短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="courierName">送货员姓名</param>
        /// <param name="courierPhone">送货员电话</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送自建物流发货通知短信(string mobile, string orderId, string courierName, string courierPhone)
        {
            string msg = GetSmsContent("发送自建物流发货通知短信");
            return Send(mobile, string.Format(msg, orderId, courierName, courierPhone), null);
        }

        /// <summary>
        /// 发送自提通知短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="shopAddress">门店地址</param>
        /// <param name="shopPhone">门店电话</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送自提通知短信(string mobile, string orderId, string shopAddress, string shopPhone)
        {
            string msg = GetSmsContent("发送自提通知短信");
            return Send(mobile, string.Format(msg, orderId, shopAddress, shopPhone), null);
        }

        /// <summary>
        /// 发送自提通知短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="shopAddress">门店地址</param>
        /// <param name="shopPhone">门店电话</param>
        /// <param name="code">自提验证码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-09-25 陶辉 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送自提通知短信带验证码(string mobile, string orderId, string shopAddress, string shopPhone, string code)
        {
            string msg = GetSmsContent("发送自提通知短信带验证码");
            return Send(mobile, string.Format(msg, orderId, shopAddress, code, shopPhone), null);
        }

        /// <summary>
        /// 发送退货提交通知短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="returnTime">退货申请时间:yyyy年MM月dd日</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送退货提交通知短信(string mobile, string returnTime)
        {
            string msg = GetSmsContent("发送退货提交通知短信");
            return Send(mobile, string.Format(msg, returnTime), null);
        }

        /// <summary>
        /// 发送换货受理审核通过短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="billId">退货单号</param>
        /// <param name="address">换货商品寄回地址</param>
        /// <param name="username">换货商品寄回收件人</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送退货受理审核通过短信(string mobile, string billId, string address, string username)
        {
            string msg = GetSmsContent("发送退货受理审核通过短信");
            return Send(mobile, string.Format(msg, billId, address, username), null);
        }

        /// <summary>
        /// 发送退货受理审核未通过短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送退货受理审核未通过短信(string mobile)
        {
            string msg = GetSmsContent("发送退货受理审核未通过短信");
            return Send(mobile, msg, null);
        }

        /// <summary>
        /// 发送换货受理审核通过短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="billId">退货单号</param>
        /// <param name="address">换货商品寄回地址</param>
        /// <param name="username">换货商品寄回收件人</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送换货受理审核通过短信(string mobile, string billId, string address, string username)
        {
            string msg = GetSmsContent("发送换货受理审核通过短信");
            return Send(mobile, string.Format(msg, billId, address, username), null);
        }

        /// <summary>
        /// 发送换货受理审核通过短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="exchangeOrderId">换货订单号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送换货完成短信(string mobile, string exchangeOrderId)
        {
            string msg = GetSmsContent("发送换货完成短信");
            return Send(mobile, string.Format(msg, exchangeOrderId), null);
        }

        /// <summary>
        /// 发送换货受理审核未通过短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送换货受理审核未通过短信(string mobile)
        {
            string msg = GetSmsContent("发送换货受理审核未通过短信");
            return Send(mobile, msg, null);
        }

        /// <summary>
        /// 发送退款完成短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="returnBillId">退货单号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-02-26 罗雄伟 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送退款完成短信(string mobile, string returnBillId)
        {
            string msg = GetSmsContent("发送退款完成短信");
            return Send(mobile, string.Format(msg, returnBillId), null);
        }

        /// <summary>
        /// 发送门店退款完成短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="returnBillId">退货单号</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2014-03-20 余勇 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送门店退款完成短信(string mobile, string returnBillId)
        {
            string msg = GetSmsContent("发送门店退款完成短信");
            return Send(mobile, string.Format(msg, returnBillId), null);
        }

        /// <summary>
        /// 发送升舱成功短信(新用户)
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="mallName">升舱来源商城名称</param>
        /// <param name="mallOrderId">来源商城订单号</param>
        /// <param name="account">当日达账号</param>
        /// <param name="password">当日达账号密码</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>
        /// 2013-9-24 陶辉 创建
        /// 2014-06-24 余勇 修改短信内容
        /// </remarks>
        public SmsResult 发送升舱成功短信(string mobile, string mallName, string mallOrderId, string account, string password)
        {
            string msg = GetSmsContent("发送升舱成功短信");
            return Send(mobile, string.Format(msg, mallName, mallOrderId, account, password), null);
        }

        /// <summary>
        /// 参加活动获得积分短信通知
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="point">积分数量</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送参加活动获得积分短信(string mobile, string activityName, int point)
        {
            string msg = GetSmsContent("发送参加活动获得积分短信");
            return Send(mobile, string.Format(msg, activityName, point.ToString()), null);
        }

        /// <summary>
        /// 购物获得积分短信通知
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="point">积分数量</param>
        /// <returns></returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送购物获得积分短信(string mobile, int point)
        {
            string msg = GetSmsContent("发送购物获得积分短信");
            return Send(mobile, string.Format(msg, point.ToString()), null);
        }

        /// <summary>
        /// 完善个人资料获得积分短信通知
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="point">积分数量</param>
        /// <returns></returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送完善个人资料获得积分短信(string mobile, int point)
        {
            string msg = GetSmsContent("发送完善个人资料获得积分短信");
            return Send(mobile, string.Format(msg, point.ToString()), null);
        }

        /// <summary>
        /// 发送参加活动使用积分短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="point">积分数量</param>
        /// <returns>短信发送状态</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送参加活动使用积分短信(string mobile, string activityName, int point)
        {
            string msg = GetSmsContent("发送参加活动使用积分短信");
            return Send(mobile, string.Format(msg, activityName, point.ToString()), null);
        }

        /// <summary>
        /// 发送评价商品获得积分短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="point">积分数量</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送评价商品获得积分短信(string mobile, int point)
        {
            string msg = GetSmsContent("发送评价商品获得积分短信");
            return Send(mobile, string.Format(msg, point.ToString()), null);
        }

        /// <summary>
        /// 发送晒单获得积分短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="point">积分数量</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送晒单获得积分短信(string mobile, int point)
        {
            string msg = GetSmsContent("发送晒单获得积分短信");
            return Send(mobile, string.Format(msg, point.ToString()), null);
        }

        /// <summary>
        /// 发送积分兑换礼品短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="point">积分数量</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送积分兑换礼品短信(string mobile, int point)
        {
            string msg = GetSmsContent("发送积分兑换礼品短信");
            return Send(mobile, string.Format(msg, point.ToString()), null);
        }

        /// <summary>
        /// 发送积分兑换优惠券短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="couponName">优惠券名称</param>
        /// <param name="point">积分数量</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送积分兑换优惠券短信(string mobile, string couponName, int point)
        {
            string msg = GetSmsContent("发送积分兑换优惠券短信");
            return Send(mobile, string.Format(msg, point.ToString(), couponName), null);
        }

        /// <summary>
        /// 发送积分兑换惠源币短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="coin">惠源币数量</param>
        /// <param name="point">积分数量</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送积分兑换惠源币短信(string mobile, int coin, int point)
        {
            string msg = GetSmsContent("发送积分兑换惠源币短信");
            return Send(mobile, string.Format(msg, point.ToString(), coin.ToString()), null);
        }

        /// <summary>
        /// 发送使用惠源币购物短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="coin">惠源币数量</param>
        /// <param name="money">抵扣金额</param>
        /// <returns>void</returns>
        /// <remarks>2014-03-17 黄波 创建</remarks>
        public SmsResult 发送使用惠源币购物短信(string mobile, int coin, int money)
        {
            string msg = GetSmsContent("发送使用惠源币购物短信");
            return Send(mobile, string.Format(msg, coin.ToString(), money.ToString()), null);
        }
        /// <summary>
        /// 群发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        /// <param name="sendTime"></param>
        /// <remarks>2015-09-12 王耀发 创建</remarks>
        public SmsResult SendMsg(string mobile, string msg, DateTime? sendTime)
        {
            var result = new SmsResult { Status = SmsResultStatus.Success };
            result = SmsProviderFactory.CreateProvider().Send(mobile, msg, sendTime);
            SaveToDb(mobile, msg, result);
            return result;
        }
        /// <summary>
        /// 各分销商发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="dealername">分销商</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// 王耀发 2016-1-18 创建
        /// <returns>执行结果</returns>
        public SmsResult DealerSendMsg(string mobile, string dealername, string msg, DateTime? sendTime)
        {
            var result = new SmsResult { Status = SmsResultStatus.Success };
            try
            {            
                result = SmsProviderFactory.CreateProvider().DealerSend(mobile, dealername, msg, sendTime);
                SaveToDb(mobile, msg, result);              
            }
            catch (Exception ex)
            {
                result.Status = SmsResultStatus.Failue;
            }

            return result;
          
        }
    }
}
