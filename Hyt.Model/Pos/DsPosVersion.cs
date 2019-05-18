using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// Pos机版本实体类
    /// </summary>
    /// <remarks>
    /// 2016-02-24 杨云奕
    /// </remarks>
    public class DsPosVersion
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSsyNo { get; set; }
        /// <summary>
        /// 版本编号吗
        /// </summary>
        public string DsVersion { get; set; }
        /// <summary>
        /// 版本软件下载地址
        /// </summary>
        public string DsFilePath { get; set; }
        /// <summary>
        /// 添加创建时间
        /// </summary>
        public DateTime DsCreateTime { get; set; }

        /// <summary>
        /// 获取文件描述名称
        /// </summary>
        public string DsFileDis { get; set; }
    }
}
