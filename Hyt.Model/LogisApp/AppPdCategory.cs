using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP使用商品分类
    /// </summary>
    /// <remarks>2013-07-31 周唐炬 创建</remarks>
    [DataContract]
    public class AppPdCategory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        [DataMember]
        public int ParentSysNo { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类子节点
        /// </summary>
        [DataMember]
        public List<AppPdCategory> ItemList { get; set; }
    }
}
