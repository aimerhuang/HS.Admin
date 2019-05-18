using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.ValidateCodes
{
    /// <summary>
    /// 验证码对象
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class CodeWrap
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 生成图片
        /// </summary> 
        /// <returns></returns>
        public byte[] Image { get; set; }
    }

    /// <summary>
    /// 验证码样式
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public enum CodeStyle
    {
        /// <summary>
        /// 前台
        /// </summary>
        Fore,

        /// <summary>
        /// 后台
        /// </summary>
        Admin,

        /// <summary>
        /// 默认
        /// </summary>
        Default
    }
}
