using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.ValidateCodes
{
    /// <summary>
    /// 验证码Style管理
    /// </summary>
    /// <remarks>2014-01-10 杨浩 创建</remarks>
    public class VerifyCodeManger
    {
        private readonly static VerifyCodeManger _instance = new VerifyCodeManger();

        /// <summary>
        /// 验证码样式实例
        /// </summary>
        /// <remarks>2014-01-10 杨浩 创建</remarks>
        public static VerifyCodeManger Instance 
        {
            get { return _instance; }
        }

        /// <summary>
        /// 获取验证码对象
        /// </summary>
        /// <param name="imageWidth">验证码图片宽度</param>
        /// <param name="imageHeight">验证码图片高度</param>
        /// <param name="length">验证码长度</param>
        /// <param name="style">样式</param>
        /// <returns>验证码对象</returns>
        /// <remarks>2014-01-10 杨浩 创建</remarks>
        public CodeWrap GetValidateCodeType(int imageWidth, int imageHeight, int length = 4,CodeStyle style=CodeStyle.Default)
        {
            //默认样式
            ICode code = new ForeVerifyCodeStyle();
            if (CodeStyle.Fore == style)
            {
                code = new ForeVerifyCodeStyle();
            }
            if (CodeStyle.Admin == style)
            {
                code = new AdminVerifyCodeStyle();
            }

            return code.CreateCode(imageWidth, imageHeight, length);
        }
    }
}
