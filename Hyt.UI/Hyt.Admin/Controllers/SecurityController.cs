using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 验证码控制器
    /// </summary>
    /// <remarks>2013-06-07 杨晗 创建</remarks>
    public class SecurityController : BaseController
    {
        /// <summary>
        /// 生成验证码(输出图片格式)
        /// </summary>
        /// <returns>生成的验证码(图片格式)</returns>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf 2013-06-07 杨晗 修改
        /// </remarks>
        [CustomActionFilter(false)]
        public ActionResult VerifyCode()
        {
            //生成图片
            var code = Hyt.Util.ValidateCodes.VerifyCodeManger.Instance.GetValidateCodeType(120, 40, 4, Util.ValidateCodes.CodeStyle.Admin);
            //保存验证码到session
            Session["verifycode"] = code.Code;
            return File(code.Image, "image/JPEG");
        }
    }
}
