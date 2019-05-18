using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// mock calling center para
    /// 呼叫中心集成-兴龙
    /// </summary>
    /// <remarks>2013-08-02 黄伟 创建</remarks>
    public class ParaCallCenterIndex : BaseEntity
    {
        /// <summary>
        /// 来电号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 为该来电的通话记录ID，可以根据需要决定是否作处理
        /// </summary>
        public string Callrecid { get; set; }

        /// <summary>
        /// 来电所属地
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 进线号码
        /// </summary>
        public string Accnum { get; set; }

        /// <summary>
        /// 用户IVR按键节点
        /// </summary>
        public string IvrNode { get; set; }

        /// <summary>
        /// 认证参数。统一小写。MD5(key+sso_data)
        /// key="15ba577241cf21c237d80d3695a98596"
        /// </summary>
        public string SSO_auth { get; set; }

        /// <summary>
        /// json字符串。包含了一些登录账号的相关信息。统一小写
        /// </summary>
        public SSOData SSO_data { get; set; }

    }

    /// <summary>
    /// json字符串。包含了一些登录账号的相关信息。统一小写
    /// </summary>
    /// <remarks>2013-08-02 黄伟 创建</remarks>
    public class SSOData : BaseEntity
    {
        /// <summary>
        /// 公司名
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 工号的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工号所在的组号
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 坐席电话
        /// </summary>
        public string Clgnumber { get; set; }

        /// <summary>
        /// 发起请求的当前时间戳
        /// </summary>
        public long Ts { get; set; }

    }
}
