using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 测试数据分页
    /// </summary>
    /// <remarks>
    /// 杨浩 创建 2013-6-15
    /// </remarks>
    public class PageTest:Hyt.Model.BaseEntity
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}