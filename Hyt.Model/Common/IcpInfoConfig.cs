using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 商检配置
    /// </summary>
    /// <remarks>2016-3-20 杨浩 创建</remarks>
    [Serializable]
    public class IcpInfoConfig : ConfigBase
    {
        public GZJCIcpInfoTrade GZJCIcpInfoTrade { get; set; }

        public GZJCIcpInfoGuoJi GZJCIcpInfoGuoJi { get; set; }

        public NSIcpInfo NSIcpInfo { get; set; }

    }

    [Serializable]
    public class GZJCIcpInfoTrade
    {
        /// <summary>
        /// 商品备案报文类型
        /// </summary>
        public string GoodsMessageType { get; set; }
        /// <summary>
        /// 订单报文类型
        /// </summary>
        public string OrderMessageType { get; set; }
        /// <summary>
        /// 报文发送者标识
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 报文接收人标识
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpUrl { get; set; }

        /// <summary>
        /// FTP地址用户名
        /// </summary>
        public string FtpName { get; set; }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 业务类型 单向海关申报填CUS、单向国检申报填CIQ、同时发送可填BBC
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string SignerInfo { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 申报企业编号
        /// </summary>
        public string DeclEntNo { get; set; }

        /// <summary>
        /// 申报企业名称
        /// </summary>
        public string DeclEntName { get; set; }

        /// <summary>
        /// 电商企业编号
        /// </summary>
        public string EBEntNo { get; set; }

        /// <summary>
        /// 电商企业名称
        /// </summary>
        public string EBEntName { get; set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        public string OpType { get; set; }

        /// <summary>
        /// 主管海关代码
        /// </summary>
        public string CustomsCode { get; set; }

        /// <summary>
        /// 检验检疫机构代码
        /// </summary>
        public string CIQOrgCode { get; set; }

        /// <summary>
        /// 电商平台企业编号可空
        /// </summary>
        public string EBPEntNo { get; set; }

        /// <summary>
        /// 电商平台名称
        /// </summary>
        public string EBPEntName { get; set; }

        /// <summary>
        /// 物流企业编号
        /// </summary>
        public string EHSEntNo { get; set; }

        /// <summary>
        /// 物流企业名称
        /// </summary>
        public string EHSEntName { get; set; }

        /// <summary>
        /// 币制代码
        /// </summary>
        public string CurrCode { get; set; }
        /// <summary>
        /// 跨境业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 进出境标志
        /// </summary>
        public string IeFlag { get; set; }

        /// <summary>
        /// 报送者名称（个人）
        /// </summary>
        public string DeclPerson { get; set; }

        /// <summary>
        /// 报送者证件号码（个人）
        /// </summary>
        public string DeclPerNumber { get; set; }

        /// <summary>
        /// 报送者证件类型代码（个人）
        /// </summary>
        public string DeclPerTypeCode { get; set; }

        /// <summary>
        /// 电商平台互联网域名
        /// </summary>
        public string InternetDomainName { get; set; }
    }

    [Serializable]
    public class GZJCIcpInfoGuoJi
    {
        /// <summary>
        /// 商品备案报文类型
        /// </summary>
        public string GoodsMessageType { get; set; }
        /// <summary>
        /// 订单报文类型
        /// </summary>
        public string OrderMessageType { get; set; }
        /// <summary>
        /// 报文发送者标识
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 报文接收人标识
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpUrl { get; set; }

        /// <summary>
        /// FTP地址用户名
        /// </summary>
        public string FtpName { get; set; }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 业务类型 单向海关申报填CUS、单向国检申报填CIQ、同时发送可填BBC
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string SignerInfo { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 申报企业编号
        /// </summary>
        public string DeclEntNo { get; set; }

        /// <summary>
        /// 申报企业名称
        /// </summary>
        public string DeclEntName { get; set; }

        /// <summary>
        /// 电商企业编号
        /// </summary>
        public string EBEntNo { get; set; }

        /// <summary>
        /// 电商企业名称
        /// </summary>
        public string EBEntName { get; set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        public string OpType { get; set; }

        /// <summary>
        /// 主管海关代码
        /// </summary>
        public string CustomsCode { get; set; }

        /// <summary>
        /// 检验检疫机构代码
        /// </summary>
        public string CIQOrgCode { get; set; }

        /// <summary>
        /// 电商平台企业编号可空
        /// </summary>
        public string EBPEntNo { get; set; }

        /// <summary>
        /// 电商平台名称
        /// </summary>
        public string EBPEntName { get; set; }

        /// <summary>
        /// 物流企业编号
        /// </summary>
        public string EHSEntNo { get; set; }

        /// <summary>
        /// 物流企业名称
        /// </summary>
        public string EHSEntName { get; set; }

        /// <summary>
        /// 币制代码
        /// </summary>
        public string CurrCode { get; set; }
        /// <summary>
        /// 跨境业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 进出境标志
        /// </summary>
        public string IeFlag { get; set; }

        /// <summary>
        /// 报送者名称（个人）
        /// </summary>
        public string DeclPerson { get; set; }

        /// <summary>
        /// 报送者证件号码（个人）
        /// </summary>
        public string DeclPerNumber { get; set; }

        /// <summary>
        /// 报送者证件类型代码（个人）
        /// </summary>
        public string DeclPerTypeCode { get; set; }

        /// <summary>
        /// 电商平台互联网域名
        /// </summary>
        public string InternetDomainName { get; set; }
    }

    [Serializable]
    public class NSIcpInfo
    {
        /// <summary>
        /// 商品备案报文类型
        /// </summary>
        public string GoodsMessageType { get; set; }
        /// <summary>
        /// 订单报文类型
        /// </summary>
        public string OrderMessageType { get; set; }
        /// <summary>
        /// 报文发送者标识
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 报文接收人标识
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpUrl { get; set; }

        /// <summary>
        /// FTP地址用户名
        /// </summary>
        public string FtpName { get; set; }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 业务类型 单向海关申报填CUS、单向国检申报填CIQ、同时发送可填BBC
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string SignerInfo { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
    }

}
