using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    /// <summary>
    /// 仓库实体
    /// </summary>
    public class EasStock : EasBaseAccount
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string FNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string FFullNumber { get; set; }
        /// <summary>
        /// 全名称
        /// </summary>
        public string FFullName { get; set; }
        /// <summary>
        /// 层次
        /// </summary>
        public string FLevel { get; set; }
        /// <summary>
        /// 是否明细数据
        /// </summary>
        public string fdetail { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string FAddress { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string FPhone { get; set; }
        /// <summary>
        /// 是否允许负结存
        /// </summary>
        public string FUnderStock { get; set; }
        public int isParent { get; set; }
        public int intType { get; set; }
    }
}
