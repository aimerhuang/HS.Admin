using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品读取属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-18 唐永勤 创建
    /// </remarks>
    [Serializable]
    public class CBPdProductAtttributeRead
    {
        /// <summary>
        /// 属性编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 商品属性映射表编号
        /// </summary>
        public int ProductAttributeSysno { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public int AttributeType { get; set; }

        /// <summary>
        /// 属性文本值
        /// </summary>
        public string AttributeText { get; set; }

        /// <summary>
        /// 属性图片值
        /// </summary>
        public string AttributeImage { get; set; }

        /// <summary>
        /// 属性选项值
        /// </summary>
        public int AttributeOptionSysNo { get; set; }

        /// <summary>
        /// 属性组编号
        /// </summary>
        public int AttributeGroupSysNo { get; set; }

        /// <summary>
        /// 属性组名称
        /// </summary>
        public string AttributeGroupName { get; set; }

        /// <summary>
        /// 是否是搜索属性
        /// </summary>
        public int IsSearchKey { get; set; }

        /// <summary>
        /// 是否用做关联属性
        /// </summary>
        public int IsRelationFlag { get; set; }

        /// <summary>
        /// 属性选项卡列表
        /// </summary>
        public IList<PdAttributeOption> AttributeOptionList { get; set; }

        /// <summary>
        /// 商品属性状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
    }

    /// <summary>
    /// 分组属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-18 唐永勤 创建
    /// </remarks>
    [Serializable]
    public class CBPdProductAtttributeReadRelation
    {
        /// <summary>
        /// 属性组编号
        /// </summary>
        public int AttributeGroupSysNo { get; set; }

        /// <summary>
        /// 属性组名称
        /// </summary>
        public string AttributeGroupName { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        public IList<CBPdProductAtttributeRead> ProductAtttributeList { get; set; }
 
    }
}
