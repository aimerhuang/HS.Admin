using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;
using Hyt.Util;

namespace Hyt.Service.Contract.Base
{
    /// <summary>
    /// 安全令牌
    /// </summary>
    /// <remarks> 2013-7-10 杨浩 创建 </remarks>
    public class ContractToken
    {
        /// <summary>
        /// 令牌哈希仓储
        /// </summary>
        private static readonly Hashtable _hashtable = new Hashtable();

        #region 方法
        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-10  杨浩 添加 </remarks>
        public static string GetToken()
        {
            //rest
            string token = HttpContext.Current.Request.Params["token"] ?? HttpContext.Current.Request.Headers["token"];
            if (string.IsNullOrEmpty(token))
            {
                if (OperationContext.Current != null)
                {
                    //soap
                    if (!OperationContext.Current.RequestContext.RequestMessage.IsEmpty)
                    {
                        var headers = OperationContext.Current.IncomingMessageHeaders;
                        var index = headers.FindHeader("token", "http://tempuri.org");
                        if (index > -1)
                            return headers.GetHeader<string>(index);
                        else
                            return null;  
                    }
                }
            }
            return token;
        }

        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        public static string CreateToken(int sysNo, string account, string password)
        {
            string token = string.Empty;
            string key = account + "," + sysNo;
            token += key.GetHashCode() + password.GetHashCode();
 
            token += "-" + DateTime.Now.ToString("HHmmss");
            token = EncryptionUtil.EncryptWithMd5(token);
            if (!_hashtable.ContainsKey(key))
                _hashtable.Add(key, token);
            else
                _hashtable[key] = token;
            return token;
        }

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="token">Md5加密后的令牌码</param>
        /// <returns></returns>
        /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        public static bool CheckToken(string token)
        {
            if (!_hashtable.ContainsValue(token))
                return false;
            return true;
        }

        /// <summary>
        /// 作废令牌
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        public static void InvalidToken(string account,string token)
        {
            if ((string) _hashtable[account] == token)
                _hashtable.Remove(account);
        }

        /// <summary>
        /// 根据token获取当前key
        /// </summary>
        /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        static string[] TokenKey 
        {
            get
            {
                foreach (DictionaryEntry item in _hashtable)
                {
                    if (item.Value.ToString() == GetToken())
                    {
                        return item.Key.ToString().Split(',');
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 当前账号
        /// </summary>
        public static string Account
        {
            get { return TokenKey != null ? TokenKey[0] : null; }
        }

        /// <summary>
        /// 当前客户编号
        /// </summary>
        public static int CustomerSysNo
        {
            get { return TokenKey != null ? int.Parse(TokenKey[1]) : default(int); }
        }

        #endregion
    }
}
