using Extra.UpGrade.HaiDaiModel;
using Hyt.Model;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Extra.UpGrade.Api
{
    public class HaiDaiApi
    {
        
		private const string FORMAT_JSON = "json";
		private const string CHARSET = "utf-8";
		private const string SIGN = "topSign";
       
		private string method = "post";
        //
        private string AppSecret;
        private string appkey;
        private string username;
        private string password;
        //private string accessToken;
		private WebUtils webUtils;
        public HaiDaiApi(string AppSecret, string appkey, string username, string password)
		{
            this.AppSecret = AppSecret;
            this.appkey = appkey;
            this.username = username;
            this.password = password;
			this.webUtils = new WebUtils();
		}
        
        /// <summary>  
        /// 登录获取token
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public HaiDaiResultLogin LoginApi(string serverUrl)
        {
            HaiDaiResultLogin login = new HaiDaiResultLogin();
            IDictionary<string, string> txtParams =  new Dictionary<string, string>(); 
            if (!txtParams.ContainsKey("timestamp"))
            {
                txtParams.Add("timestamp", this.GetTime());
            }

            if (!txtParams.ContainsKey("username"))
            {
                txtParams.Add("username", this.username.ToString());
            }

            if (!txtParams.ContainsKey("password"))
            {
                txtParams.Add("password", EncryptWithMd5(this.password));
            }
            if (!txtParams.ContainsKey("appkey"))
            {
                txtParams.Add("appkey", this.appkey);
            }
            if (serverUrl.Contains("?"))
            {
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
            }
            Uri uri = new Uri(serverUrl);
            string absolutePath = uri.AbsolutePath;
            string encoding = "utf-8";
            //txtParams.TryGetValue("charset", out encoding);
            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))
            {
                txtParams.Add("topSign", sign);
            }
            string text = string.Empty;
           
          
                text = this.webUtils.DoPost(serverUrl, txtParams);
                login = JsonSerializationHelper.JsonToObject<HaiDaiResultLogin>(text);
                return login;
        }
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        /// <remarks>2017－10-08 罗勤瑶 创建</remarks>
        public static string EncryptWithMd5(String str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
        }
        /// <summary>  
        /// 获取订单列表
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <param name="txtParams">参数</param>  
        /// <param name="fileParams">参数</param> 
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public HaiDaiResultLogin InvokeOpenApi(string serverUrl, IDictionary<string, string> txtParams, IDictionary<string, FileItem> fileParams)
		{
            
            if (!txtParams.ContainsKey("timestamp"))
            {
                txtParams.Add("timestamp", this.GetTime());
            }
            if (!txtParams.ContainsKey("appkey"))
            {
                txtParams.Add("appkey", this.appkey);
            }
			if (serverUrl.Contains("?"))
			{
				serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
			}
			Uri uri = new Uri(serverUrl);
			string absolutePath = uri.AbsolutePath;
			string encoding = "utf-8";
            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))
            {
                txtParams.Add("topSign", sign);
            }
			string text = string.Empty;
			if (fileParams != null && fileParams.Count > 0)
			{
				text = this.webUtils.DoPost(serverUrl, txtParams, fileParams);
			}
			else
			{
				text = this.webUtils.DoPost(serverUrl, txtParams);
			}
            return JsonSerializationHelper.JsonToObject<HaiDaiResultLogin>(text);
		}
        /// <summary>  
        /// 接单
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <param name="txtParams">参数</param>  
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public HaiDaiResultOrder InvokeOpenApi(string serverUrl, IDictionary<string, string> txtParams)
        {

            if (!txtParams.ContainsKey("timestamp"))
            {
                txtParams.Add("timestamp", this.GetTime());
            }
            if (!txtParams.ContainsKey("appkey"))
            {
                txtParams.Add("appkey", this.appkey);
            }
            if (serverUrl.Contains("?"))
            {
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
            }
            Uri uri = new Uri(serverUrl);
            string absolutePath = uri.AbsolutePath;
            string encoding = "utf-8";
            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))
            {
                txtParams.Add("topSign", sign);
            }
            string text = string.Empty;
          
                text = this.webUtils.DoPost(serverUrl, txtParams);

                return JsonSerializationHelper.JsonToObject<HaiDaiResultOrder>(text);
        }
        /// <summary>  
        /// 发货
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <param name="txtParams">参数</param>  
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public ShipGoods ShipGoods(string serverUrl, IDictionary<string, string> txtParams)
        {

            if (!txtParams.ContainsKey("timestamp"))
            {
                txtParams.Add("timestamp", this.GetTime());
            }
            if (!txtParams.ContainsKey("appkey"))
            {
                txtParams.Add("appkey", this.appkey);
            }
            if (serverUrl.Contains("?"))
            {
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
            }
            Uri uri = new Uri(serverUrl);
            string absolutePath = uri.AbsolutePath;
            string encoding = "utf-8";
            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))
            {
                txtParams.Add("topSign", sign);
            }
            string text = string.Empty;

            text = this.webUtils.DoPost(serverUrl, txtParams);

            return JsonSerializationHelper.JsonToObject<ShipGoods>(text);
        }
        /// <summary>  
        /// 订单详情
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <param name="txtParams">参数</param>  
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public Result<DataDetail> OrderDetail(string serverUrl, IDictionary<string, string> txtParams)
        {
            var result = new Result<DataDetail>() { Status = false };

            if (!txtParams.ContainsKey("timestamp"))          
                txtParams.Add("timestamp", this.GetTime());
            
            if (!txtParams.ContainsKey("appkey"))            
                txtParams.Add("appkey", this.appkey);
            
            if (serverUrl.Contains("?"))           
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
            
            var uri = new Uri(serverUrl);
            string absolutePath = uri.AbsolutePath;
            string encoding = "utf-8";

            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))            
                txtParams.Add("topSign", sign);

            var responseStr = this.webUtils.DoPost(serverUrl, txtParams);
            var  response=JObject.Parse(responseStr);
            if (response["result"].ToString() == "1")
            {
                result.Status = true;
                result.Data = JsonSerializationHelper.JsonToObject<DataDetail>(response["data"].ToString());
                return result;
            }

            result.Message = response["data"].ToString();
                    
            return result;
        }

        /// <summary>  
        /// 快递信息
        /// </summary>  
        /// <param name="serverUrl">登录URL</param>  
        /// <param name="txtParams">参数</param>  
        /// <returns>返回</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        public HaiDaiResultKuaiDi KuaiDiDetail(string serverUrl, IDictionary<string, string> txtParams)
        {

            if (!txtParams.ContainsKey("timestamp"))
            {
                txtParams.Add("timestamp", this.GetTime());
            }
            if (!txtParams.ContainsKey("appkey"))
            {
                txtParams.Add("appkey", this.appkey);
            }
            if (serverUrl.Contains("?"))
            {
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf('?') + 1);
            }
            Uri uri = new Uri(serverUrl);
            string absolutePath = uri.AbsolutePath;
            string encoding = "utf-8";
            string sign = this.GetSign(this.method, absolutePath, txtParams, encoding);
            if (!txtParams.ContainsKey("topSign"))
            {
                txtParams.Add("topSign", sign);
            }
            string text = string.Empty;

            text = this.webUtils.DoPost(serverUrl, txtParams);

            return JsonSerializationHelper.JsonToObject<HaiDaiResultKuaiDi>(text);
        }
        /// <summary>  
        /// 获取时间戳
        /// </summary>  
        /// <returns>返回时间戳</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
		private string GetTime()
		{
			DateTime d = new DateTime(1970, 1, 1);
			return ((long)((DateTime.Now.ToUniversalTime() - d).TotalMilliseconds + 0.5)).ToString();
		}
		private int GetRandomValue()
		{
			Random random = new Random();
			return random.Next(1, 2147483647);
		}

        /// <summary>  
        /// 指定秘钥加密
        /// </summary>  
        /// <returns>返回时间戳</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
		private string GetSign(string method, string url_path, IDictionary<string, string> parameters, string secret, string encoding)
		{
			string source = this.GetSource(method, url_path, parameters, encoding);
			byte[] bytes = Encoding.Default.GetBytes(secret);
			HMACSHA1 hMACSHA = new HMACSHA1(bytes);
			hMACSHA.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(source));
			byte[] hash = hMACSHA.Hash;
			return Convert.ToBase64String(hash);
		}
        /// <summary>  
        /// SHA1加密签名
        /// </summary>  
        /// <returns>返回加密字符串</returns>  
        /// <remarks>2017-6-13 罗勤尧 创建</remarks>
        private string GetSign(string method, string url_path, IDictionary<string, string> parameters,string encoding)
        {
            string source = this.GetSource(method, url_path, parameters, encoding);
            //首尾加上AppSecret
            string sourcenew = this.AppSecret + source + this.AppSecret;

            return SHA1(sourcenew, Encoding.UTF8); ;
        }
       
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }  
		private string EncodeUrl(string input, string encoding)
		{
			return this.webUtils.UrlEncode(input, Encoding.GetEncoding(encoding));
		}
        /// <summary>  
        /// 排序
        /// </summary>  
        /// <param name="parameters">键值对</param>  
        /// <param name="encode">编码</param>  
        /// <returns>返回排序后字符串</returns>  
		private string GetSource(string method, string url_path, IDictionary<string, string> parameters, string encoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			//stringBuilder.Append(method.ToUpper()).Append("&").Append(this.EncodeUrl(url_path, encoding)).Append("&");
			IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
			IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
			StringBuilder stringBuilder2 = new StringBuilder();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
                    stringBuilder2.Append(key).Append("=").Append(webUtils.UrlEncode(value, Encoding.UTF8));
					stringBuilder2.Append("&");
				}
			}
			if (stringBuilder2.Length > 1)
			{
				stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
			}
			stringBuilder.Append(this.EncodeUrl(stringBuilder2.ToString(), encoding));
            return stringBuilder2.ToString();
		}
		private IDictionary<string, T> CleanupDictionary<T>(IDictionary<string, T> dict)
		{
			IDictionary<string, T> dictionary = new Dictionary<string, T>(dict.Count);
			IEnumerator<KeyValuePair<string, T>> enumerator = dict.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, T> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				T value = current.Value;
				if (value != null)
				{
					dictionary.Add(key, value);
				}
			}
			return dictionary;
		}        
    }
}
