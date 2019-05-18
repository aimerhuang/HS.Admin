using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Infrastructure.Memory
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    /// <remarks>2013-6-26 杨浩 创建</remarks>
    public class MemoryProvider
    {
        private static readonly ICache Cache = new MemoryCacheProvider();

        /// <summary>
        /// 提供默认缓存实例
        /// </summary>
        public static ICache Default
        {
            get { return Cache; }
        }
    }
}
