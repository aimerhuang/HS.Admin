using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 表单参数枚举
    /// </summary>
    /// <remarks>2016-4-16 杨浩 创建</remarks>
    public enum ParamType
    {
        File,
        Text,
    }
    /// <summary>
    /// 表单项（用于模拟表单提交）
    /// </summary>
    /// <remarks>2016-4-16 杨浩 创建</remarks>
    public class FormItem
    {
        /// <summary>
        /// 表单字段名称
        /// </summary>
        public string Name{get;set;}
        /// <summary>
        /// 表单值
        /// </summary>
        public string Value{get;set;}
        /// <summary>
        /// 表单类型
        /// </summary>
        public ParamType ParamType{get;set;}
        /// <summary>
        /// 表单文件流
        /// </summary>
        public Stream FileStream{get; set;}
    }
   
}
