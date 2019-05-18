using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Data;
using System.Configuration;
using System.Web;
namespace Extra.IISHelper
{
    /// <summary>
    /// iis帮助类
    /// </summary>
    /// <remarks>2016-1-11 杨浩 创建</remarks>
    public class IISHelper
    {
        #region 临时模拟IIS管理员用户

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        static WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domain"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool impersonateValidUser(String userName)
        {
            string domain = "administrator";
            string password = "123";
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        /// <summary>
        /// 撤消模拟
        /// </summary>
        private static void undoImpersonation()
        {
            impersonationContext.Undo();
        }

        #endregion


        /// <summary>
        /// 获取一个网站的编号。根据网站的ServerBindings或者ServerComment来确定网站编号
        /// </summary>
        /// <param name="serverName">服务器名称(默认为localhost，可以为IP地址)</param>
        /// <param name="siteName">站点名称</param>
        /// <returns>返回网站的编号</returns>
        private static int GetWebSiteID(string serverName, string siteName)
        {
            int siteID = -1;

            Regex regex = new Regex(siteName);

            if (serverName.Length < 1)
                serverName = "localhost";

            string tmpStr;
            string entPath = String.Format("IIS://{0}/w3svc", serverName);
            DirectoryEntry ent = new DirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        tmpStr = child.Properties["ServerBindings"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            siteID = int.Parse(child.Name);
                            break;
                        }
                    }
                    if (child.Properties["ServerComment"].Value != null)
                    {
                        tmpStr = child.Properties["ServerComment"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            siteID = int.Parse(child.Name);
                            break;
                        }
                    }
                }
            }
            return siteID;
        }

        /// <summary>
        /// 添加域名绑定
        /// </summary>
        /// <param name="siteName">站点名称</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void AddHostHeader(string siteName, string ip, int port, string domain)
        {
            AddHostHeader("localhost", siteName, ip, port, domain);
        }

        /// <summary>
        /// 添加域名绑定
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="siteName">站点名称</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void AddHostHeader(string serverName, string siteName, string ip, int port, string domain)
        {
            if (impersonateValidUser("dsiis"))
            {
                //Insert your code that runs under the security context of a specific user here.
                int siteID = GetWebSiteID(serverName, siteName);
                if (siteID < 1)
                    siteID = 1;

                AddHostHeader(serverName, siteID, ip, port, domain);

                undoImpersonation();
            }
            else
            {
                //"临时模拟IIS管理员用户"失败;
            }
        }

        /// <summary>
        /// 添加域名绑定
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="siteid">站点编号</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void AddHostHeader(string serverName, int siteid, string ip, int port, string domain)
        {
            DirectoryEntry site = new DirectoryEntry("IIS://" + serverName + "/W3SVC/" + siteid);
            PropertyValueCollection serverBindings = site.Properties["ServerBindings"];
            string headerStr = string.Format("{0}:{1}:{2}", ip, port, domain);
            if (!serverBindings.Contains(headerStr))
            {
                serverBindings.Add(headerStr);
            }
            //默认情况下，对缓存进行本地更改。修改或添加节点之后，
            //必须调用 CommitChanges 方法，以便提交更改，将它们保存到树中。
            //如果在调用 CommitChanges 之前调用 RefreshCache，则将丢失对属性缓存的任何未提交的更改。
            site.CommitChanges();
        }

        /// <summary>
        /// 删除域名绑定
        /// </summary>
        /// <param name="siteName">站点名称</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void DeleteHostHeader(string siteName, string ip, int port, string domain)
        {
            DeleteHostHeader("localhost", siteName, ip, port, domain);
        }

        /// <summary>
        /// 删除域名绑定
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="siteName">站点名称</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void DeleteHostHeader(string serverName, string siteName, string ip, int port, string domain)
        {
            if (impersonateValidUser("dsiis"))
            {
                //Insert your code that runs under the security context of a specific user here.
                int siteID = GetWebSiteID(serverName, siteName);
                if (siteID < 1)
                    siteID = 1;

                DeleteHostHeader(serverName, siteID, ip, port, domain);

                undoImpersonation();
            }
            else
            {
                //Your impersonation failed. Therefore, include a fail-safe mechanism here.
            }
        }

        /// <summary>
        /// 删除域名绑定
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="siteid">站点编号</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        /// <param name="domain">域名</param>
        public static void DeleteHostHeader(string serverName, int siteid, string ip, int port, string domain)
        {
            DirectoryEntry site = new DirectoryEntry("IIS://" + serverName + "/W3SVC/" + siteid);
            PropertyValueCollection serverBindings = site.Properties["ServerBindings"];
            string headerStr = string.Format("{0}:{1}:{2}", ip, port, domain);
            if (serverBindings.Contains(headerStr))
            {
                serverBindings.Remove(headerStr);
            }
            site.CommitChanges();
        }

        public static List<IISWebSite> LoadIISWebSiteData(string serverName)
        {
            List<IISWebSite> list = new List<IISWebSite>();
            int siteID;
            string siteName;

            string entPath = String.Format("IIS://{0}/w3svc", serverName);
            DirectoryEntry ent = new DirectoryEntry(entPath);

            foreach (DirectoryEntry child in ent.Children)
            {
                string HostName = ConfigurationManager.AppSettings["RencaiHost"].ToString();
                siteName = child.Properties["ServerComment"].Value.ToString();
                //判断当前站点，是否为所指示的站点。
                if (child.SchemaClassName == "IIsWebServer" && siteName == HostName)
                {
                    siteID = int.Parse(child.Name);
                    list.Add(new IISWebSite(siteID, siteName));
                    HttpCookie Cookie = new HttpCookie("IIsWebServerName");
                    Cookie.Value = siteID.ToString();
                    HttpContext.Current.Response.Cookies.Add(Cookie);
                    break;
                }
            }
            return list;

        }
        public static List<IISWebSiteHostHeader> LoadHostHeaderList(string serverName, int siteID)
        {
            List<IISWebSiteHostHeader> list = new List<IISWebSiteHostHeader>();
            //连接到 Web 目录。例如“IIS://LocalHost/W3SVC/1/ROOT/<Web 目录名>”。
            DirectoryEntry site = new DirectoryEntry("IIS://" + serverName + "/W3SVC/" + siteID);
            PropertyValueCollection serverBindings = site.Properties["ServerBindings"];

            if (serverBindings != null && serverBindings.Value != null)
            {
                foreach (string str in serverBindings)
                {
                    list.Add(new IISWebSiteHostHeader(siteID, str.Split(':')[0], int.Parse(str.Split(':')[1]), str.Split(':')[2]));
                }
            }
            return list;
        }

        /// <summary>
        /// 在指定IIS服务器中检测指定主机头实例是否存在，存在返回true，否则返回false
        /// </summary>
        /// <param name="serverName">IIS服务器</param>
        /// <param name="newHostHeader">待检测的主机头实例</param>
        /// <returns></returns>
        public static bool IsHostHeaderExists(string serverName, IISWebSiteHostHeader newHostHeader)
        {
            bool isFind = false;

            List<IISWebSite> siteList = LoadIISWebSiteData(serverName);
            foreach (IISWebSite site in siteList)
            {
                List<IISWebSiteHostHeader> headerList = LoadHostHeaderList(serverName, site.SiteID);
                foreach (IISWebSiteHostHeader header in headerList)
                {
                    if (newHostHeader.Equals(header))
                    {
                        isFind = true;
                        break;
                    }
                }
            }
            return isFind;
        }

        /// <summary>
        /// 在现有绑定主机头列表中搜索指定的主机头
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="hostHeader"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static List<IISWebSiteHostHeader> SearchHostHeaderList(string serverName, string hostHeader)
        {
            List<IISWebSiteHostHeader> list = new List<IISWebSiteHostHeader>();
            List<IISWebSite> siteList = LoadIISWebSiteData(serverName);
            foreach (IISWebSite site in siteList)
            {
                List<IISWebSiteHostHeader> headerList = LoadHostHeaderList(serverName, site.SiteID);
                foreach (IISWebSiteHostHeader header in headerList)
                {
                    if (hostHeader.Equals(header.HostHeader, StringComparison.CurrentCultureIgnoreCase))
                        list.Add(header);
                }
            }
            return list;
        }
    }

    /// <summary>
    /// IIS站点主机头实体类
    /// </summary>
    public class IISWebSiteHostHeader
    {
        public int SiteID { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string HostHeader { get; set; }

        public IISWebSiteHostHeader(int siteID, string ip, int port, string hostHeader)
        {
            this.SiteID = siteID;
            this.IP = ip;
            this.Port = port;
            this.HostHeader = hostHeader;
        }

        /// <summary>
        /// 比较两个站点主机头实体是否相等，不比较站点ID(SiteID)，仅比较IP/Port/HostHeader3项。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            IISWebSiteHostHeader header = obj as IISWebSiteHostHeader;
            if (header != null)
            {
                if (header.IP == this.IP && header.Port == this.Port && header.HostHeader == this.HostHeader)
                    return true;
                else
                    return false;
            }

            return base.Equals(obj);
        }
    }

    /// <summary>
    /// IIS站点实体类
    /// </summary>
    public class IISWebSite
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public IISWebSite(int siteID, string siteName)
        {
            this.SiteID = siteID;
            this.SiteName = siteName;
        }
    }
}
