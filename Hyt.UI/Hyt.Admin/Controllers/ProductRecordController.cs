using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyt.Model.SystemPredefined;
using Hyt.Model;
using Hyt.Util;

namespace Hyt.Admin.Controllers
{
    public class ProductRecordController : BaseController
    {
        /// <summary>
        /// 导出订单模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportTemplateNS()
        {
            ExportExcelNS(@"\Templates\Excel\IcpGZNanShaGoodsInfo.xls", "南沙商检商品备案信息");
        }
        /// <summary>
        /// 导出excel模板文件
        /// </summary>
        /// <param name="tmpPath">模板路径</param>
        /// <param name="excelFileName">导出文件名</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private void ExportExcelNS(string tmpPath, string excelFileName)
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), tmpPath, 1, excelFileName, null, false);
        }
        /// <summary>
        /// 导出订单模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportTemplateGJ()
        {
            ExportExcelGJ(@"\Templates\Excel\LgGaoJieGoodsInfo.xls", "高捷商品备案信息");
        }
        /// <summary>
        /// 导出excel模板文件
        /// </summary>
        /// <param name="tmpPath">模板路径</param>
        /// <param name="excelFileName">导出文件名</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private void ExportExcelGJ(string tmpPath, string excelFileName)
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), tmpPath, 1, excelFileName, null, false);
        }
        // GET: /ProductRecord/商品备案管理
        [Privilege(PrivilegeCode.PR1001001)]
        public ActionResult ProductRecordList()
        {
            return View();
        }

        public static bool _starting;
        /// <summary>
        /// 导入库存
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.PR1001001)]
        public ActionResult ImportExcel()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    switch(httpPostedFileBase.FileName)
                    {
                        case "高捷商品备案信息.xls":
                            _starting = true;
                            try
                            {
                                result = Hyt.BLL.Logistics.LogisticsBo.Instance.ImportExcelGaoJie(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                _starting = false;
                            }
                            break;
                        case "南沙商检商品备案信息.xls":
                            _starting = true;
                            try
                            {
                                result = Hyt.BLL.ApiIcq.IcpBo.Instance.ImportExcelNS(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                _starting = false;
                            }
                            break;
                        default:
                            result.Message = string.Format("导入的文件名必须为： 南沙商检商品备案信息.xls,  高捷商品备案信息.xls 之一");
                            result.Status = false;
                            break;
                    }
                }
                else
                {
                    result.Message = string.Format("正在导入数据，请稍后再操作");
                    result.Status = false;

                }
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();

        }
    }
}
