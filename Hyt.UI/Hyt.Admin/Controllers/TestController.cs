using Hyt.BLL.ApiHaiDai;
using Hyt.Model.LiJiaModel;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        //测试用
        public ActionResult TestHaiDai()
        {
            //HaiDaiBll.Instance.Test();
            //return Content("");

            //根据手机号查询利嘉会员信息
            LiJiaMemberSearch serch = new LiJiaMemberSearch();
            serch.rules = new List<LiJiaSearch>();
            LiJiaSearch se = new LiJiaSearch();
            se.data = "15112458070";
            se.field = "CellPhone";
            serch.rules.Add(se);
            LiJiaSearch sels = new LiJiaSearch();
            sels.data = "级分销";
            sels.field = "MemberName";
            serch.rules.Add(sels);
            var res = BLL.Order.LiJiaSoOrderSynchronize.SeachLiJiaMemerber(serch);

            var me = res.rows.First(i => i.CellPhone.Contains("15112458070"));
            return Content(JsonHelper.ToJson(res));
        }


        //public ActionResult Trst() {
        //    OrderNumber on = new OrderNumber();

        //    return View();
        //}
    }
}
