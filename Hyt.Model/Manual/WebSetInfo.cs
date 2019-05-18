using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 网站基础配置
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public class WebSetInfo
    {

        /// <summary>
        /// 网站根路径
        /// </summary>
        /// <remarks>2013-4-11 添加 何方 </remarks>
        public string WebRoot { get; set; }
        /// <summary>
        /// Ftp图片服务器(上传图片使用)
        /// </summary>
        public string FtpImageServer { get; set; }

        private string _ftpUserName;
        /// <summary>
        /// ftp用户名
        /// </summary>
        public string FtpUserName
        {
            get { return _ftpUserName; }
            set { _ftpUserName = value; }
        }
        private string _ftpPassword;
        /// <summary>
        /// ftp密码
        /// </summary>
        public string FtpPassword
        {
            get { return _ftpPassword; }
            set { _ftpPassword = value; }
        }

        private string _imagePath;
        /// <summary>
        /// 图片服务器地址
        /// </summary>
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        private string _lucenePath;
        /// <summary>
        /// 索引路径
        /// </summary>
        public string LucenePath
        {
            get { return _lucenePath; }
            set { _lucenePath = value; }
        }

        private string _errorSysPath;

        /// <summary>
        /// 系统错误日志路径
        /// </summary>
        public string ErrorSysPath
        {
            get { return _errorSysPath; }
            set { _errorSysPath = value; }
        }

        #region 支付宝参数
        /// <summary>
        /// 支付宝合作伙伴ID
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPayPartnerID { get; set; }

        /// <summary>
        /// 支付宝异步对账地址
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPayAsyncReturnUrl { get; set; }

        /// <summary>
        /// 支付宝同步回调页面URL
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPaySyncReturnUrl { get; set; }

        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPaySellerEmail { get; set; }

        /// <summary>
        /// 支付宝网关地址
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPayGateway { get; set; }

        /// <summary>
        /// 支付宝安全校验码
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string AliPaykey { get; set; }

        #endregion

        #region 网银在线参数
        /// <summary>
        /// 网银在线商户号
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string ChinaBankMID { get; set; }

        /// <summary>
        /// 网银在线异步对账地址
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string ChinaBankAsyncReturnUrl { get; set; }

        /// <summary>
        /// 网银在线同步回调页面URL
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string ChinaBankSyncReturnUrl { get; set; }

        /// <summary>
        /// 网银在线网关地址
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string ChinaBankGateway { get; set; }

        /// <summary>
        /// 网银在线安全校验码MD5
        /// </summary>
        /// <remarks>
        /// 2012-03-29 郑荣华 创建
        /// </remarks>
        public string ChinaBankkey { get; set; }

        /// <summary>
        /// 网银在线来源地址,请求本网站所用网址MD5
        /// </summary>
        public string ChinaBankFromUrl { get; set; }
        #endregion

        #region 61活动
        /// <summary>
        /// 获取或设置开始时间
        /// </summary>
        public string Active61StartTime { get; set; }
        /// <summary>
        /// 获取或设置结束时间
        /// </summary>
        public string Active61EndTime { get; set; }
        /// <summary>
        /// 6.1活动优惠券批次号
        /// </summary>
        public int Active61BatchNo { get; set; }
        #endregion
    }
}
