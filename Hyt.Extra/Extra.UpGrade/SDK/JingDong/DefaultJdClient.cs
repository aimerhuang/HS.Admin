using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Jayrock.Json.Conversion;
using Extra.UpGrade.SDK.JingDong.Parser;
using Extra.UpGrade.SDK.JingDong.Request;
using Extra.UpGrade.SDK.JingDong.Util;
using System.Text;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace Extra.UpGrade.SDK.JingDong
{
	/// <summary>
	/// 基于REST的JD客户端。
	/// </summary>
	public class DefaultJdClient : IJdClient
	{
		public const string APP_KEY = "app_key";
		public const string FORMAT = "format";
		public const string METHOD = "method";
		public const string TIMESTAMP = "timestamp";
		public const string VERSION = "v";
		public const string SIGN = "sign";
		public const string ACCESS_TOKEN = "access_token";
		public const string PARAM_JSON = "360buy_param_json";
		public const string FORMAT_XML = "xml";
		public const string SDK_VERSION = "jos_net_sdk";
		private string serverUrl;
		private string appKey;
		private string appSecret;
		private string format = "json";
		private WebUtils webUtils;
		private IJdLogger JdLogger;
		private bool disableParser = false;
		// 禁用响应结果解释
		private bool disableTrace = false;
		// 禁用日志调试功能
		private IDictionary<string, string> systemParameters;
		// 设置所有请求共享的系统级参数

		#region DefaultJdClient Constructors

		public DefaultJdClient (string serverUrl, string appKey, string appSecret)
		{
			this.appKey = appKey;
			this.appSecret = appSecret;
			this.serverUrl = serverUrl;
			this.webUtils = new WebUtils ();
			this.JdLogger = new DefaultJdLogger ();
		}

		public DefaultJdClient (string serverUrl, string appKey, string appSecret, string format)
            : this (serverUrl, appKey, appSecret)
		{
			this.format = format;
		}

		#endregion

		public void SetJdLogger (IJdLogger JdLogger)
		{
			this.JdLogger = JdLogger;
		}

		public void SetTimeout (int timeout)
		{
			this.webUtils.Timeout = timeout;
		}

		public void SetDisableParser (bool disableParser)
		{
			this.disableParser = disableParser;
		}

		public void SetDisableTrace (bool disableTrace)
		{
			this.disableTrace = disableTrace;
		}

		public void SetSystemParameters (IDictionary<string, string> systemParameters)
		{
			this.systemParameters = systemParameters;
		}

		#region IJdClient Members

		public T Execute<T> (IJdRequest<T> request) where T : JdResponse
		{
			return Execute<T> (request, null);
		}

		public T Execute<T> (IJdRequest<T> request, string session) where T : JdResponse
		{
			return Execute<T> (request, session, DateTime.Now);
		}

		public T Execute<T> (IJdRequest<T> request, string session, DateTime timestamp) where T : JdResponse
		{
			return DoExecute<T> (request, session, timestamp);
		}

		#endregion

		private T DoExecute<T> (IJdRequest<T> request, string access_token, DateTime timestamp) where T : JdResponse
		{
			// 提前检查业务参数
			try {
				request.Validate ();
			} catch (JdException e) {
				return createErrorResponse<T> (e.ErrorCode, e.ErrorMsg);
			}

			// 添加协议级请求参数
			JdDictionary txtParams = new JdDictionary ();
			txtParams.Add (METHOD, request.GetApiName ());
			txtParams.Add (VERSION, "2.0");
			txtParams.Add (APP_KEY, appKey);
			txtParams.Add (TIMESTAMP, timestamp);
			if (access_token != null) {
				txtParams.Add (ACCESS_TOKEN, access_token);
			}
			txtParams.Add (PARAM_JSON, JsonConvert.ExportToString (request.GetParameters ()));
			// 添加签名参数
			txtParams.Add (SIGN, JdUtils.SignJdRequest (txtParams, appSecret, true));
			string reqUrl = webUtils.BuildGetUrl (this.serverUrl, txtParams);
			try {
				string body;
				if (request is IJdUploadRequest<T>) { // 是否需要上传文件
					IJdUploadRequest<T> uRequest = (IJdUploadRequest<T>)request;
					IDictionary<string, FileItem> fileParams = JdUtils.CleanupDictionary (uRequest.GetFileParameters ());
					body = webUtils.DoPost (this.serverUrl, txtParams, fileParams);
				} else {
					body = webUtils.DoPost (this.serverUrl, txtParams);
				}

				// 解释响应结果
				T rsp;
				if (disableParser) {
					rsp = Activator.CreateInstance<T> ();
					rsp.Body = body;
				} else {
					if (FORMAT_XML.Equals (format)) {
						IJdParser tp = new JdXmlParser ();
						rsp = tp.Parse<T> (body);
					} else {
						IJdParser tp = new JdJsonParser ();
						rsp = tp.Parse<T> (body);         
					}
				}

				// 追踪错误的请求
				if (!disableTrace && rsp.IsError) {
					StringBuilder sb = new StringBuilder (reqUrl).Append (" response error!\r\n").Append (rsp.Body);
					JdLogger.Warn (sb.ToString ());
				}
				return rsp;
			} catch (Exception e) {
				if (!disableTrace) {
					StringBuilder sb = new StringBuilder (reqUrl).Append (" request error!\r\n").Append (e.StackTrace);
					JdLogger.Error (sb.ToString ());
				}
				throw e;
			}
		}

		private T createErrorResponse<T> (string errCode, string errMsg) where T : JdResponse
		{
			T rsp = Activator.CreateInstance<T> ();
			rsp.ErrCode = errCode;
			rsp.ErrMsg = errMsg;

			if (FORMAT_XML.Equals (format)) {
				XmlDocument root = new XmlDocument ();
				XmlElement bodyE = root.CreateElement ("error_response");
				XmlElement codeE = root.CreateElement ("code");
				codeE.InnerText = errCode;
				bodyE.AppendChild (codeE);
				XmlElement msgE = root.CreateElement ("msg");
				msgE.InnerText = errMsg;
				bodyE.AppendChild (msgE);
				root.AppendChild (bodyE);
				rsp.Body = root.OuterXml;
			} else {
				IDictionary<string, object> errObj = new Dictionary<string, object> ();
				errObj.Add ("code", errCode);
				errObj.Add ("msg", errMsg);
				IDictionary<string, object> root = new Dictionary<string, object> ();
				root.Add ("error_response", errObj);

				string body = JsonConvert.ExportToString (root);
				rsp.Body = body;
			}
			return rsp;
		}

	}
}
