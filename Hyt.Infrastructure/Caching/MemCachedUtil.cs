using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using System.Configuration;

namespace Hyt.Infrastructure.Caching
{
    /// <summary>
    /// 程序
    /// </summary>
    public class MemCachedUtil
    {
        /// <summary>
        /// 主入口
        /// </summary>
        /// <param name="args">参数</param>
        public static List<string> GetllKeys()
        {
            MemcachedClientSection config = ConfigurationManager.GetSection("enyim.com/memcached") as MemcachedClientSection;
            if (config != null && config.Servers.Count > 0)
            {
                var server = config.Servers.ToIPEndPointCollection()[0];
                string ipAddress = server.Address.ToString();
                var port = server.Port;

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
                var slabIdIter = QuerySlabId(socket);
                var keyIter = QueryKeys(socket, slabIdIter);
                socket.Close();

                return keyIter.ToList();
            }
            return null;
        }

        /// <summary>
        /// 执行返回字符串标量
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="command">命令</param>
        /// <returns>执行结果</returns>
        static String ExecuteScalarAsString(Socket socket, String command)
        {
            var sendNumOfBytes = socket.Send(Encoding.UTF8.GetBytes(command));
            var bufferSize = 0x1000;
            var buffer = new Byte[bufferSize];
            var readNumOfBytes = 0;
            var sb = new StringBuilder();

            while (true)
            {
                readNumOfBytes = socket.Receive(buffer);
                sb.Append(Encoding.UTF8.GetString(buffer));

                if (readNumOfBytes < bufferSize)
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 查询slabId
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>slabId遍历器</returns>
        static IEnumerable<String> QuerySlabId(Socket socket)
        {
            var command = "stats items STAT items:0:number 0 \r\n";
            var contentAsString = ExecuteScalarAsString(socket, command);

            return ParseStatsItems(contentAsString);
        }

        /// <summary>
        /// 解析STAT items返回slabId
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>slabId遍历器</returns>
        static IEnumerable<String> ParseStatsItems(String contentAsString)
        {
            var slabIds = new List<String>();
            var separator = "\r\n";
            var separator2 = ':';
            var items = contentAsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            for(Int32 i = 0; i < items.Length; i++)
            {
                var itemParts = items[i].Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                if (itemParts.Length < 3)
                    continue;

                if (!slabIds.Contains(itemParts[1])) slabIds.Add(itemParts[1]);
            }
            return slabIds;
        }

        /// <summary>
        /// 查询键
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="slabIdIter">被查询slabId</param>
        /// <returns>键遍历器</returns>
        static IEnumerable<String> QueryKeys(Socket socket, IEnumerable<String> slabIdIter)
        {
            var keys = new List<String>();
            var cmdFmt = "stats cachedump {0} 200000 ITEM views.decorators.cache.cache_header..cc7d9 [6 b; 1256056128 s] \r\n";
            var contentAsString = String.Empty;

            foreach (String slabId in slabIdIter)
            {
                contentAsString = ExecuteScalarAsString(socket, String.Format(cmdFmt, slabId));
                keys.AddRange(ParseKeys(contentAsString));
            }

            return keys;
        }

        /// <summary>
        /// 解析stats cachedump返回键
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>键遍历器</returns>
        static IEnumerable<String> ParseKeys(String contentAsString)
        {
            var keys = new List<String>();
            var separator = "\r\n";
            var separator2 = ' ';
            var prefix = "ITEM";
            var items = contentAsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                var itemParts = item.Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                if ((itemParts.Length < 3) || !String.Equals(itemParts.FirstOrDefault(), prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                keys.Add(itemParts[1]);
            }

            return keys;
        }
    }

    /// <summary>
    /// String扩展函数
    /// </summary>
    static class StringExtension
    {
        /// <summary>
        /// 切割
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="options">选项</param>
        /// <returns>切割结果</returns>
        public static String[] Split(this String str, Char separator, StringSplitOptions options)
        {
            return str.Split(new Char[] { separator }, options);
        }

        /// <summary>
        /// 切割
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="options">选项</param>
        /// <returns>切割结果</returns>
        public static String[] Split(this String str, String separator, StringSplitOptions options)
        {
            return str.Split(new String[] { separator }, options);
        }
    }
}