using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Service.Contract.WeiXin.Model
{
    /// <summary>
    /// 自定义主菜单
    /// </summary>
    /// <remarks>2015-01-9 杨浩 创建</remarks>
    public class ButtonMenu
    {
        /// <summary>
        /// 一级菜单数组，个数应为1~3个
        /// </summary>
        public List<SubButtonMenu> button { get; set; }
    }

    /// <summary>
    /// 自定义一级菜单
    /// </summary>
    /// <remarks>2015-01-9 杨浩 创建</remarks>
    public class CustomizeMenu
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        //public string key { get; set; }

        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }


    }
    /// <summary>
    /// 子菜单项
    /// </summary>
    /// 2015-01-9 杨浩 创建
    public class SubButtonMenu
    {
        public List<CustomizeMenu> sub_button { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
    }
}
