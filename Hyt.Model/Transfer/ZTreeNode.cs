using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// ZTree节点
    /// </summary>
    /// <remarks>
    /// 2013-06-21 何方 创建
    /// </remarks>
    public class ZTreeNode
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 节点选择次数（权值）
        /// </summary>
        //public int ex_weight { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 记录 treeNode 节点的 展开 / 折叠 状态
        /// </summary>
        /// <remarks>true 表示节点为 展开 状态,false 表示节点为 折叠 状态</remarks>
        public bool open { get; set; }
        /// <summary>
        /// 判断 treeNode 节点是否被隐藏 
        /// </summary>
        /// <remarks>true 表示被隐藏,false 表示被显示</remarks>
        public bool isHidden { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        /// <remarks>0-地区,1-仓库</remarks>
        public int nodetype { get; set; }
        /// <summary>
        /// 状态：启用、禁用
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool nocheck { get; set; }
    }
}
