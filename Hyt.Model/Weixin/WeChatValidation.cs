using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Weixin
{
    /// <summary>
    /// 微信真伪验证,返回的对象
    /// </summary>
    /// <remarks>2013-12-05 陶辉 创建</remarks>
    public class WeChatValidation : BaseEntity
    {
        /*sample
         {"cus":0,"msg":"品胜防伪数据中心无法查询你所提供的防伪码；<br>请确认你提供的防伪码是否正确。<br>或该产品不是品胜正品。<br>如有其他问题请致电品胜客服中心：400-088-9898"}
         */

        /// <summary>
        /// 0:false;others:true
        /// </summary>
        public int Cus { get; set; }
        
        /// <summary>
        ///related msgs 
        /// </summary>
        public string Msg { get; set; }

    }
}
