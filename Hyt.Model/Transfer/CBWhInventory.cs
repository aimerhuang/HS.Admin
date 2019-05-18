using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    public class CBWhInventory : WhInventory
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 快速查询条件
        /// </summary>
        public string Condition { get; set; }
    }
}
