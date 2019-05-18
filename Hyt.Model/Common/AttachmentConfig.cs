using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 附件FTP信息
    /// </summary>
    /// <returns>2014-1-20 黄波 创建</returns>
    public class AttachmentConfig : ConfigBase
    {
        /// <summary>
        /// ftp地址
        /// </summary>
        public string FtpImageServer { get; set; }

        /// <summary>
        /// ftp帐号
        /// </summary>
        public string FtpUserName { get; set; }

        /// <summary>
        /// ftp密码
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 图片服务器地址
        /// </summary>
        public string FileServer { get; set; }

        /// <summary>
        /// 编辑器上传图片存放文件夹名称
        /// </summary>
        public string EditorImage { get; set; }

        /// <summary>
        /// 图片生成工具地址
        /// </summary>
        public string SyncCreateThumbnailToolAddress { get; set; }
    }
}
