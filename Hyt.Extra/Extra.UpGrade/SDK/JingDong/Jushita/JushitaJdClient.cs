using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Request;

namespace Extra.UpGrade.SDK.JingDong.Jushita
{
    /// <summary>
    /// 聚石塔专用Jd客户端。
    /// </summary>
    public class JushitaJdClient
    {
        private const string SYNC_CENTER_URL = "http://eai.taobao.com/api";

        private DefaultJdClient JdClient;

        public JushitaJdClient(string serverUrl, string appKey, string appSecret)
        {
            this.JdClient = new DefaultJdClient(serverUrl, appKey, appSecret);
            this.JdClient.SetDisableParser(true);
        }

        public JushitaJdClient(string appKey, string appSecret)
            : this(SYNC_CENTER_URL, appKey, appSecret)
        {
        }

        public JushitaJdClient(string serverUrl, string appKey, string appSecret, int timeout)
            : this(serverUrl, appKey, appSecret)
        {
            this.JdClient.SetTimeout(timeout);
        }

        public string execute(string apiName, IDictionary<string, string> parameters, string session)
        {
            JushitaRequest request = new JushitaRequest();
            request.ApiName = apiName;
            request.Parameters = parameters;
            JushitaResponse response = JdClient.Execute(request, session);
            return response.Body;
        }
    }

    public class JushitaRequest : IJdRequest<JushitaResponse>
    {
        public string ApiName { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public string GetApiName()
        {
            return this.ApiName;
        }

        public IDictionary<string, string> GetParameters()
        {
            return this.Parameters;
        }

        public void Validate()
        {
        }
    }

    public class JushitaResponse : JdResponse
    {
    }
}
