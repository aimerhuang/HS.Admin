using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Hyt.Model.SystemPredefined;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 公共组件控制器
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    [CustomActionFilter(false)]
    public class SharedController : BaseController
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="config">配置项名称</param>
        /// <returns>文件上传页面</returns>
        /// <remarks>2013-7-25 黄波 创建</remarks>
        [CustomActionFilter(true)]
        [Privilege(PrivilegeCode.CM1010001)]
        public ActionResult Upload(string config)
        {
            var uploadConfig = Hyt.BLL.Config.Config.Instance.GetUpLoadFileConfig();
            var useUploadConfigOption = ((List<Model.Common.FileConfigOption>)uploadConfig.OtherConfig).Find(o => o.EncryptAlias == config);
            if (useUploadConfigOption == null) useUploadConfigOption = uploadConfig.DefaultConfig;

            return View("_Upload", useUploadConfigOption);

        }

        /// <summary>
        /// 错误404提示页面
        /// </summary>
        /// <returns>Error404页面</returns>
        /// <remarks>2013-7-30 杨浩 创建</remarks>
        public ActionResult Error404()
        {
            return View("Error404");
        }

        /// <summary>
        /// 权限错误提示页面
        /// </summary>
        /// <returns>无权限提示页面</returns>
        /// <remarks>2013-7-30 杨浩 创建</remarks>
        public ActionResult ErrorPrivilege()
        {
            return View();
        }
    }

    /// <summary>
    /// Html生成类
    /// </summary>
    ///<remarks>2013-07-26 黄志勇 创建</remarks>
    public static class MvcCreateHtml
    {
        /// <summary>
        /// 根据SelectListItem列表生成html字符串
        /// </summary>
        /// <param name="items">SelectListItem列表</param>
        /// <param name="optionSelected">应选中的项</param>
        /// <param name="optionFilter">应过滤的项</param>
        /// <returns>html字符串</returns>
        /// <remarks>2013-07-26 黄志勇 创建</remarks>
        public static MvcHtmlString SelectItemListToString(this List<SelectListItem> items, Func<SelectListItem, bool> optionSelected, Func<SelectListItem, bool> optionFilter)
        {
            var htmlString = new StringBuilder();
            if (items != null && items.Count > 0)
            {
                items.ForEach(item =>
                    {
                        if (optionFilter == null || !optionFilter(item))
                        {
                            var option = new TagBuilder("option");
                            option.SetInnerText(item.Text);
                            option.MergeAttribute("value", item.Value, true);
                            if ((optionSelected != null && optionSelected(item)) || item.Selected)
                            {
                                option.MergeAttribute("selected", "selected", true);
                            }
                            htmlString.Append(option.ToString());
                        }
                    });
            }
            return new MvcHtmlString(htmlString.ToString());
        }

        /// <summary>
        /// 根据枚举类型生成html字符串
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="optionSelected">应选中的项</param>
        /// <param name="optionFilter">应过滤的项</param>
        /// <returns>html字符串</returns>
        /// <remarks>2013-07-26 黄志勇 创建</remarks>
        public static MvcHtmlString EnumToString<T>(Func<SelectListItem, bool> optionSelected, Func<SelectListItem, bool> optionFilter) where T : struct, IConvertible
        {
            var list = new List<SelectListItem>();
            Hyt.Util.EnumUtil.ToListItem<T>(ref list);
            return list.SelectItemListToString(optionSelected, optionFilter);
        }
    }
}
