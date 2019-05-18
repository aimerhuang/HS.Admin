using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Service.Contract.MallSeller.Model
{
    public class BaseRequest
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationParameters AuthInfo { get; set; }
    }
}
