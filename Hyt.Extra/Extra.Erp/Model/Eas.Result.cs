using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    public class EasResult
    {
        public string message { get; set; }
        public int error_code { get; set; }
        public bool success { get; set; }
    }
    public class EasResult<T> : EasResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T data;
    }
}
