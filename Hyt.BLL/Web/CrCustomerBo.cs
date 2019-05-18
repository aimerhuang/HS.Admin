using System;
using System.Collections.Generic;
using System.IO;
using Extra.SMS;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Caching;
using Hyt.DataAccess.Web;
using Hyt.BLL.Extras;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 会员业务逻辑层
    /// </summary>
    /// <remarks>2013－08-05 苟治国 创建</remarks>
    public class CrCustomerBo : BOBase<CrCustomerBo>
    {
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public CBCrCustomer GetModel(int sysNo)
        {
            return Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(sysNo);
        }

        ///<summary>
        ///根据手机获取用户
        /// </summary>
        /// <param name="mobile">手机.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-12 苟治国 创建
        /// </remarks>
        public CrCustomer GetCustomerByCellphone(string mobile)
        {
            return Hyt.DataAccess.Web.ICrCustomerDao.Instance.GetCustomerByCellphone(mobile);
        }

        ///<summary>
        ///根据呢称获取用户
        /// </summary>
        /// <param name="nickName">呢称.</param>
        /// <param name="sysNo">客户编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-16 苟治国 创建
        /// </remarks>
        public CrCustomer GetCustomerByName(string nickName, int sysNo)
        {
            return Hyt.DataAccess.Web.ICrCustomerDao.Instance.GetCustomerByName(nickName, sysNo);
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="model">客户资料</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-11 苟治国 创建</remarks>
        public int Update(Model.CrCustomer model)
        {
            return Hyt.DataAccess.Web.ICrCustomerDao.Instance.Update(model);
        }

        /// <summary>
        /// 更新登录时间、Ip
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="lastLoginIp">客户ip</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-20 苟治国 创建</remarks>
        public void UpdateDateTimeAndIp(int sysNo, string lastLoginIp)
        {
            Hyt.DataAccess.Web.ICrCustomerDao.Instance.UpdateDateTimeAndIp(sysNo, lastLoginIp);
        }

        #region 邮箱
        /// <summary>
        /// 获取修改邮箱验证码
        /// </summary>
        /// <returns>修改邮箱验证码</returns>
        /// <remarks>2013-08-09 苟治国 创建</remarks>
        public string GetModifyMailVcode(int sysNo)
        {
            return CacheManager.Get<string>(CacheKeys.Items.EmailVerificationCode_, sysNo.ToString(),
            delegate()
            {
                return "";
            }
            );
        }

        /// <summary>
        /// 设置修改邮箱随机码
        /// </summary>
        /// <param name="randkey">邮箱随机数</param>
        /// <param name="sysNo">用户编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-09 苟治国 创建</remarks>
        public void SetVerifyMail(string randkey, int sysNo)
        {
            CacheManager.Instance.Set(CacheKeys.Items.EmailVerificationCode_ + sysNo.ToString(), randkey, DateTime.Now.AddMinutes(10));
        }
        #endregion

        #region 找回密码验证手机
        /// <summary>
        /// 获取每天修改手机的次数
        /// </summary>
        /// <param name="phone">用户编号</param>
        /// <returns>当天手机和邮箱的修改次数</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public int GetFindPassPhoneVerifyCodeCount(string phone)
        {
            return CacheManager.Get<int>(CacheKeys.Items.FindPassPhoneCount_, phone,
            delegate()
            {
                return 0;
            }
            );
        }

        /// <summary>
        /// 设置每天修改手机次数
        /// </summary>
        /// <param name="phone">用户编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public void SetFindPassPhoneVerifyCodeCount(string phone)
        {
            int num = GetFindPassPhoneVerifyCodeCount(phone);
            num++;

            CacheManager.Instance.Set(CacheKeys.Items.FindPassPhoneCount_ + phone, num, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 0:0:0")));
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="second">缓存时间(秒)</param>
        /// <returns>空</returns>
        /// <remarks>2013-01-07 苟治国 创建</remarks>
        public void SendFindPassPhoneVerifyCode(string phone, int second)
        {
            SetFindPassPhoneVerifyCodeCount(phone);
            var verifyCode = CacheManager.Get<string>(CacheKeys.Items.FindPassPhoneVerifyCode_, phone, DateTime.Now.AddSeconds(second),
            delegate()
            {
                return new Random().Next(100000, 999999).ToString();
            });

            SmsBO.Instance.发送手机验证短信(phone, verifyCode);
        }

        /// <summary>
        /// 获取修改手机验证码
        /// </summary>
        /// <param name="phone">手机号.</param>
        /// <returns>修改手机验证码</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public string GetFindPassPhoneVerifyCode(string phone)
        {
            return CacheManager.Get<string>(CacheKeys.Items.FindPassPhoneVerifyCode_, phone, null);
        }

        /// <summary>
        /// 删除手机验证码
        /// </summary>
        /// <param name="phone">手机号.</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-03 苟治国 创建</remarks>
        public void DelFindPassPhoneVerifyCode(string phone)
        {
            CacheManager.Instance.Delete(CacheKeys.Items.FindPassPhoneVerifyCode_ + phone);
        }

        /// <summary>
        /// 设置手机为可修改状态
        /// </summary>
        /// <param name="phone">手机号.</param>
        /// <param name="second">过期时间.</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-03 苟治国 创建</remarks>
        public void SetFindPassStatus(string phone, int second)
        {
            CacheManager.Instance.Set(CacheKeys.Items.FindPassPhoneStatus_ + phone, true, DateTime.Now.AddSeconds(second));
        }

        /// <summary>
        /// 获取手机为是否可修改状态
        /// </summary>
        /// <param name="phone">手机号.</param>
        /// <returns>返回true/false</returns>
        /// <remarks>2013-12-03 苟治国 创建</remarks>
        public bool GetFindPassStatus(string phone)
        {
            return CacheManager.Get<Boolean>(CacheKeys.Items.FindPassPhoneStatus_, phone,
            delegate()
            {
                return false;
            });
        }

        #endregion

        #region 安全中心验证手机
        /// <summary>
        /// 获取每天修改手机的次数
        /// </summary>
        /// <returns>当天手机和邮箱的修改次数</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public int GetSafePhoneVerifyCodeCount(int sysNo)
        {
            return CacheManager.Get<int>(CacheKeys.Items.ModifySafePhoneVerifyCount_, sysNo.ToString(),
            delegate()
            {
                return 0;
            }
            );
        }

        /// <summary>
        /// 设置每天修改手机次数
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public void SetSafePhoneVerifyCodeCount(int sysNo)
        {
            int num = GetSafePhoneVerifyCodeCount(sysNo);
            num++;

            CacheManager.Instance.Set(CacheKeys.Items.ModifySafePhoneVerifyCount_ + sysNo.ToString(), num, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 0:0:0")));
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="sysNo">客户编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public void SendSafePhoneVerifyCode(string phone, int sysNo)
        {
            SetSafePhoneVerifyCodeCount(sysNo);
            var verifyCode = CacheManager.Get<string>(CacheKeys.Items.ModifySafePhoneVerifyCode_, sysNo.ToString(), DateTime.Now.AddMinutes(10),
            delegate()
            {
                return new Random().Next(100000, 999999).ToString();
            });
            SmsBO.Instance.发送手机验证短信(phone, verifyCode);
        }

        /// <summary>
        /// 获取修改手机验证码
        /// </summary>
        /// <param name="sysNo">会员编号.</param>
        /// <returns>修改手机验证码</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public string GetSafePhoneVerifyCode(int sysNo)
        {
            return CacheManager.Get<string>(CacheKeys.Items.ModifySafePhoneVerifyCode_, sysNo.ToString(),
            delegate()
            {
                return string.Empty;
            }
            );
        }
        #endregion

        #region 用户注册手机短信验证

        /// <summary>
        /// 获取每天修改手机的次数
        /// </summary>
        /// <param name="phone">用户编号</param>
        /// <returns>当天手机和邮箱的修改次数</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public int GetRegisterPhoneVerifyCodeCount(string phone)
        {
            return CacheManager.Get<int>(CacheKeys.Items.RegisterPhoneCount_, phone,
            delegate()
            {
                return 0;
            }
            );
        }

        /// <summary>
        /// 设置每天修改手机次数
        /// </summary>
        /// <param name="phone">用户编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public void SetRegisterPhoneVerifyCodeCount(string phone)
        {
            int num = GetRegisterPhoneVerifyCodeCount(phone);
            num++;

            CacheManager.Instance.Set(CacheKeys.Items.RegisterPhoneCount_ + phone, num, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 0:0:0")));
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="second">缓存时间(秒)</param>
        /// <returns>验证码</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public string SendRegisterPhoneVerifyCode(string phone, int second, string sign = "")
        {
            //SetRegisterPhoneVerifyCodeCount(phone);
            //var verifyCode = CacheManager.Get<string>(CacheKeys.Items.RegisterPhoneVerifyCode_, phone, DateTime.Now.AddSeconds(second),
            //delegate()
            //{
            //    return new Random().Next(100000, 999999).ToString();
            //});
            //SmsBO.Instance.发送手机验证短信(phone, verifyCode);
            //return verifyCode;
            sign = sign == "" ? "" : sign;
            SetRegisterPhoneVerifyCodeCount(phone);
            var verifyCode = new Random().Next(100000, 999999).ToString();
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Set(CacheKeys.Items.RegisterPhoneVerifyCode_ + phone, verifyCode, 2);
            SmsBO.Instance.发送手机验证短信(phone, verifyCode, sign);
            return verifyCode;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>2016-6-13 陈海裕 创建</remarks>
        public SmsResult Send(string mobile, string content, string sign = "")
        {
            return SmsBO.Instance.发送手机短信(mobile, content, sign);
        }

        /// <summary>
        /// 获取修改手机验证码
        /// </summary>
        /// <param name="phone">手机号.</param>
        /// <returns>修改手机验证码</returns>
        /// <remarks>2013-08-13 苟治国 创建</remarks>
        public string GetRegisterPhoneVerifyCode(string phone)
        {
            return CacheManager.Get<string>(CacheKeys.Items.RegisterPhoneVerifyCode_, phone, null);
        }
        #endregion

        /// <summary>
        /// 发送管理员密码短信
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="sign">短信签名</param>
        /// <remarks>2016-3-4 刘伟豪 创建</remarks>
        public bool SendSyUserPassword(string phone, string code, string sign = "")
        {
            SmsResult reuslt = SmsBO.Instance.发送管理员密码短信(phone, code, sign);
            if (reuslt.Status == SmsResultStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendVerifycodeToUser(string phone, string code, string sign = "")
        {
            SmsResult reuslt = SmsBO.Instance.积分门店使用验证短信(phone, code, sign);
            if (reuslt.Status == SmsResultStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 用户收货地址
        /// <summary>
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="customerSysno">用户编号</param>       
        /// <returns>收货地址列表</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public IList<CBCrReceiveAddress> GetCustomerReceiveAddress(int customerSysno)
        {
            return ICrCustomerDao.Instance.GetCustomerReceiveAddress(customerSysno);
        }

        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="receiveSysno">收货地址编号</param>       
        /// <returns>收货地址</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public CBCrReceiveAddress GetCustomerReceiveAddressBySysno(int receiveSysno)
        {
            return ICrCustomerDao.Instance.GetCustomerReceiveAddressBySysno(receiveSysno);
        }
        #endregion

        /// <summary>
        /// 更新会员密码
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="passWord">密码</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－09-24 苟治国 创建</remarks>
        public int UpdatePassWord(int sysno, string passWord)
        {
            return Hyt.DataAccess.Web.ICrCustomerDao.Instance.UpdatePassWord(sysno, passWord);
        }
        ///<summary>
        /// 更新分销商等级
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="gradeId">分销等级</param>
        /// <returns></returns>
        /// <remarks>2016-12-10 杨浩 创建</remarks>
        public  bool UpdateSellBusinessGradeId(int sysno, int gradeId)
        {
            return Hyt.DataAccess.Web.ICrCustomerDao.Instance.UpdateSellBusinessGradeId(sysno,gradeId)>0;
        }
    }
}
