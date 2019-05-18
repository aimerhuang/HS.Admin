using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 短信模板
    /// </summary>
    /// <remarks>2016-1-20 杨浩 创建</remarks>
    [Serializable]
    public class SmsTemplateConfig : ConfigBase
    {
        /// <summary>
        /// 短信模板列表
        /// </summary>
        public List<SmsTemplateInfo> SmsTemplates { get; set; }
    }

    /// <summary>
    /// 短信模板详情
    /// </summary>
    [Serializable] 
    public class SmsTemplateInfo
    {
        /// <summary>
        /// 短信模板版key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 短信模板内容
        /// </summary>
        public string Content { get; set; }
    }
}
