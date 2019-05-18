using Hyt.BLL.Basic;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    //Bootstrap测试
    public class BootstrapController : BaseController
    {
        //
        // GET: /Bootstrap/
         [CustomActionFilter(false)]
        public ActionResult Index()
        {
            // //获取市
            //var all = BasicAreaBo.Instance.GetAllCity();
            //var success = false;
            //string msg = null;
            //var sysNo = 0;
            //int s = 398;
            //if (all != null&&all.Count>0)
            //{
            //    foreach(BsArea bs in all)
            //    {
            //        BsArea parentmodel = BasicAreaBo.Instance.GetArea(bs.SysNo);
            //        BsArea model =new BsArea()
            //            {
            //                AreaName="其他区",
            //                ParentSysNo=bs.SysNo,
            //                DisplayOrder=s
            //            };
            //         if (parentmodel == null)
            //    {
            //        model.AreaLevel = 0;
            //        model.DisplayOrder = 0;
            //    }
            //    else
            //    {
            //        model.AreaLevel = parentmodel.AreaLevel + 1;
            //    }
            //    char[] sep = { '（', '(' };
            //    model.NameAcronym = CHS2PinYin.Convert(model.AreaName.Split(sep)[0], false); //汉字转拼音，只保留（）以前的
            //    model.Status = (int)BasicStatus.地区状态.有效;
            //    model.CreatedBy = 530;
            //    model.CreatedDate = DateTime.Now;
            //    model.LastUpdateBy = 530;
            //    model.LastUpdateDate = DateTime.Now;
            //    sysNo = BasicAreaBo.Instance.Create(model);
            //    success = sysNo > 0;
            //    s++;
            //}
            
            //    }

            //else
            //{
            //    msg = "请正确输入地区基本信息";
            //}
               
            //return Json(new { success = success, msg = msg, newid = sysNo }, JsonRequestBehavior.AllowGet);
            return View();
        }

    }
}
