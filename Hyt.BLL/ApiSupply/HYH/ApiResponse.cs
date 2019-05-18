using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Hyt.Model;
using Hyt.Model.Common;

namespace Hyt.BLL.ApiSupply.HYH
{
    public class ApiResponse
    {
        public bool State { get; set; }

        public object Content { get; set; }
        
    }

    public class ApiConfig
    {
        public static CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.七号洋行; }
        }
        protected static SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        public static string ApiUrl = Config.GatewayUrl;

        public static string MerchantId = Config.Account;

        public static string AccessSecret = Config.Secert;
    }

    public class CommonUtils
    {
        private static string GetSign(SortedDictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in parameters)
            {
                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            return Md5(sb.ToString(0, sb.Length - 1).ToLower() + ApiConfig.AccessSecret);
        }

        public static string GetUrlParameter(SortedDictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new SortedDictionary<string, string>();
            }
            parameters.Add("MerchantId", ApiConfig.MerchantId);
            parameters.Add("sign", GetSign(parameters));
            StringBuilder sb = new StringBuilder();
            foreach (var item in parameters)
            {
                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            return sb.ToString(0, sb.Length - 1);
        }


        public static string Md5(string input)
        {
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in data)
                sBuilder.Append(b.ToString("x2"));
            return sBuilder.ToString();
        }
    }

}
