using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    /// <summary>
    /// 收件人档案实名认证
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhRecipienter
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 收件人档案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收件人身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 上传文件
        /// </summary>
        public string UploadFile { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadDateTime { get; set; }
    }
}
