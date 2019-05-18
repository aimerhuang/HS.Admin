using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyt.Model.Manual
{
    /// <summary>
    /// 内存数据库
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public class SyKeyInfo
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// Key类型
        /// </summary>
        public string KeyType { get; set; }
        /// <summary>
        /// Key值
        /// </summary>
        public string KeyValue { get; set; }
        /// <summary>
        /// 存储对象个数
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 内存占用
        /// </summary>
        public string MemoryUsed { get; set; }
        /// <summary>
        /// 对象是否可查看
        /// </summary>
        public bool CanShow { get; set; }
    }
}
