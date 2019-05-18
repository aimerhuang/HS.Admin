using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.Sys;
using Hyt.Model.SystemPredefined;

namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper扩展
    /// </summary>
    /// <remarks>2013-09-25 黄波 创建</remarks>
    public static class PrivilegeControlHelper
    {
        /// <summary>
        /// 使用权限代码控制html元素显示
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="privilegeCode">权限代码</param>
        /// <param name="classSelector">
        /// 完整的css class选择器
        /// 该选择器将选中在没有权限时需要隐藏的html元素(应该尽量使用高权重的class选择器)
        /// </param>
        /// <exception cref="用户未登录的情况下会发生异常"></exception>
        /// <returns>css样式表</returns>
        /// <remarks>2013-09-25 黄波 创建</remarks>
        public static MvcHtmlString PrivilegeControl(this HtmlHelper helper, PrivilegeCode privilegeCode, params string[] classSelector)
        {
            var privilegeList = AdminAuthenticationBo.Instance.Current.PrivilegeList;
            var tag = new TagBuilder("style");
            tag.MergeAttribute("type", "text/css");

            if (privilegeList == null || privilegeList.FirstOrDefault(item=>item.Code == privilegeCode.ToString()) == null)
            {
                tag.InnerHtml += "\r\n";
                foreach (var selector in classSelector)
                {
                    tag.InnerHtml += (selector + " {display:none !important;}\r\n");
                }
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 使用权限代码控制html元素显示
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="privilegeCode">权限数组,匹配到任何一个都认为有权限</param>
        /// <param name="classSelector">
        /// 完整的css class选择器
        /// 该选择器将选中在没有权限时需要隐藏的html元素(应该尽量使用高权重的class选择器)
        /// </param>
        /// <returns>css样式表</returns>
        /// <exception cref="用户未登录的情况下会发生异常"></exception>
        ///<remarks>2013-11-22 朱成果 创建</remarks>
        public static MvcHtmlString PrivilegeCheckControl(this HtmlHelper helper, PrivilegeCode[] privilegeCode, params string[] classSelector)
        {
            var privilegeList = AdminAuthenticationBo.Instance.Current.PrivilegeList;
            var tag = new TagBuilder("style");
            tag.MergeAttribute("type", "text/css");
            var lst = privilegeCode.Select(m => m.ToString()).AsEnumerable();
            if (privilegeList == null || privilegeList.FirstOrDefault(m=>lst.Contains(m.Code))==null)
            {
                tag.InnerHtml += "\r\n";
                foreach (var selector in classSelector)
                {
                    tag.InnerHtml += (selector + " {display:none !important;}\r\n");
                }
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 通过任务池控制html元素显示
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="taskType">任务类型</param>
        /// <param name="taskSysNo">任务编号</param>
        /// <param name="classSelector">
        /// 完整的css class选择器
        /// 该选择器将选中在没有权限时需要隐藏的html元素(应该尽量使用高权重的class选择器)
        /// </param>
        /// <returns>css样式表</returns>
        ///<remarks>2013-11-30 余勇 创建</remarks>
        public static MvcHtmlString JobPoolCheckControl(this HtmlHelper helper, int taskType,int taskSysNo, params string[] classSelector)
        {
            var privilegeList = AdminAuthenticationBo.Instance.Current.PrivilegeList;
            var tag = new TagBuilder("style");
            tag.MergeAttribute("type", "text/css");
            var cBProcJobPool = SyJobPoolManageBo.Instance.ProcJobPool(taskType, taskSysNo,AdminAuthenticationBo.Instance.Current.Base.SysNo);
            if (cBProcJobPool.IsDisabled || cBProcJobPool.IsLocked)
            {
                tag.InnerHtml += "\r\n";
                foreach (var selector in classSelector)
                {
                    tag.InnerHtml += (selector + " {display:none !important;}\r\n");
                }
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #region  自动补全 模糊查询会员和商品
        /// <summary>
        /// 自动补全 模糊查询会员和商品 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">文本id名称</param>
        /// <param name="HiddenId">隐藏文本id名 用于存放选中的id号</param>
        /// <param name="Type">查询类型 1会员 2商品</param>
        /// <param name="value">初始化绑定值</param>
        /// <param name="HiddenValue">初始化绑定隐藏文本的值</param>
        /// <param name="classStr">文本的clss样式</param>
        /// <returns>2017-8-21 吴琨 创建 </returns>
        public static MvcHtmlString UtilLike(this HtmlHelper helper, string id,string HiddenId, int Type,string value,int HiddenValue,string classStr)
        {
            var htmlStr = new StringBuilder();
            htmlStr.Append("<input type=\"text\" value=\"" + value + "\" id=\"" + id + "\" name=\"" + id + "\" class=\"" + classStr + "\"  oninput=\"fn_" + id + "()\" onpropertychange=\"fn_" + id + "()\"  onblur=\"blur_" + id + "(this,'"+Type+"')\"   style=\" width:176px;\" />");
            htmlStr.Append("<input type=\"hidden\" id=\"" + HiddenId + "\" name=\"" + HiddenId + "\" value=\"" + HiddenValue + "\" />");
            #region 会员
            if (Type==1)
            {
                htmlStr.Append("\r\n");
                htmlStr.Append("<button id=\"search_customer\" class=\"btn btn_ht26\" title=\"搜索\" type=\"button\" onclick=\"popUserInfo(this)\">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<div class=\"icon_search\"></div>");
                htmlStr.Append("\r\n");
                htmlStr.Append("</button>");
                htmlStr.Append("\r\n");
                #region html文本
                htmlStr.Append("<div id=\"div_"+ id+"\" style=\"width: 420px; overflow-y: scroll; height: 240px; z-index: 100; background-color: #ffffff; position: absolute; border: #e1e1e1 solid 1px; text-align: center; display:none;   \">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%;\">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<tr style=\"background: url(/Theme/images/boxbg.png) repeat-x 0 -40px; line-height: 34px; \">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>账号</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>姓名</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>手机</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>会员ID</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("</tr>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<tbody>");
                htmlStr.Append("</tbody>");
                htmlStr.Append("\r\n");
                htmlStr.Append("</table>\r\n</div>");
                htmlStr.Append("\r\n");
                #endregion
            }
            #endregion

            #region 商品
            else
            {
                #region html文本
                htmlStr.Append("<div id=\"div_" + id + "\" style=\"width: 420px; overflow-y: scroll; height: 240px; z-index: 100; background-color: #ffffff; position: absolute; border: #e1e1e1 solid 1px; text-align: center; display:none;   \">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%;\">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<tr style=\"background: url(/Theme/images/boxbg.png) repeat-x 0 -40px; line-height: 34px; \">");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>编号</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>条码</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>后台名称</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<th>商品ID</th>");
                htmlStr.Append("\r\n");
                htmlStr.Append("</tr>");
                htmlStr.Append("\r\n");
                htmlStr.Append("<tbody>");
                htmlStr.Append("</tbody>");
                htmlStr.Append("\r\n");
                htmlStr.Append("</table>\r\n</div>");
                htmlStr.Append("\r\n");
                #endregion
            }
            #endregion

            #region js事件
            var tag = new TagBuilder("script");
            tag.MergeAttribute("type", "text/javascript");
            //文本改变启动
            tag.InnerHtml += "\r\n";
            tag.InnerHtml += "var timer = true;";
            tag.InnerHtml += "function fn_" + id + "()";
            tag.InnerHtml += "{";
            tag.InnerHtml += "\r\n";
            tag.InnerHtml += " $(\"#div_" + id + "\").show();  ";
            htmlStr.Append("\r\n");
            tag.InnerHtml += "if(timer&&$(\"#" + id + "\").val().length>0){ timer=false; ";
            tag.InnerHtml += "setTimeout(function(){ timer=true; var val=$(\"#" + id + "\").val();  GetUtilLike(val," + Type + ",'div_" + id + "'); },1000)";
            tag.InnerHtml += "}";
            tag.InnerHtml += "\r\n";
            tag.InnerHtml += "}";
            tag.InnerHtml += "\r\n";
            //失去焦点启动
            tag.InnerHtml += "function blur_" + id + "(th,type)";
            tag.InnerHtml += "{";
            tag.InnerHtml += "\r\n";
            tag.InnerHtml += "setTimeout(function(){ $(\"#div_" + id + "\").hide(); inspect(th,type); },500)";
            tag.InnerHtml += "\r\n";
            tag.InnerHtml += "}";
            #endregion
            return MvcHtmlString.Create(htmlStr.ToString() + tag.ToString(TagRenderMode.Normal));
        }
        #endregion
    }
}