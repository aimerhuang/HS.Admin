using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Hyt.BLL.Log;
using Hyt.DataAccess.Report;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Hyt.BLL.Sys;
using System.Data;

namespace Hyt.BLL.Report
{
    /// <summary>
    /// ReportBO
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public class ReportBO : BOBase<ReportBO>
    {
        #region 升舱明细

        /// <summary>
        /// 升舱明细查询
        /// </summary>
        /// <param name="para">CBReportDsorderDetail</param>
        /// <param name="totalAmount"></param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,升舱明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public Dictionary<int, List<ReportDsorderDetail>> QueryUpgradeDetails(CBReportDsorderDetail para, ref decimal totalAmount, int currPageIndex, int pageSize = 10)
        {
            //全部选择
            if (para.MallType == -1)
            {
                para.MallType = null;
            }

            return IReportDao.Instance.QueryUpgradeDetails(para, ref totalAmount, currPageIndex, pageSize);
        }

        /// <summary>
        /// 升舱明细查询
        /// </summary>
        /// <returns>DsMallType集合</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public List<DsMallType> GetMallType()
        {
            return IReportDao.Instance.GetMallType();
        }

        /// <summary>
        /// 升舱明细-导出excel
        /// </summary>
        /// <param name="para">CBReportDsorderDetail</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public byte[] UpgradeDetailsToExcel(string userIp, int operatorSysno)
        {
            var para = new CBReportDsorderDetail();
            #region set params from Request
            var startDate = HttpContext.Current.Request.Params["OrderBeginDate"];
            if (!string.IsNullOrEmpty(startDate))
            {
                var temp = startDate.Split('-');
                para.OrderBeginDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
            }
            var endDate = HttpContext.Current.Request.Params["OrderEndDate"];
            if (!string.IsNullOrEmpty(endDate))
            {
                var temp = endDate.Split('-');
                para.OrderEndDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["MallType"]))
                para.MallType = int.Parse(HttpContext.Current.Request.Params["MallType"]);
            #endregion
            var totalAmount = decimal.Zero;
            #region 批量导出
            int maxSheet = 8;
            int currectpageindex = 1;
            int currectPageSize = 20000;//一次性从数据库获取20000条记录
            int maxExcelRow = 65000;//单个sheet最大行
            int currectRow = 1;//当前行
            ISheet sheet = null;
            IRow excelRow = null;
            ICell cell = null;
            int sheetNum = 0;//总的sheet数
            using (MemoryStream stream = new MemoryStream())
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ICellStyle cellstyle = workbook.CreateCellStyle();
                IFont fontTitle = workbook.CreateFont();
                fontTitle.FontHeightInPoints = 20;
                fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.WrapText = true;
                cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                Dictionary<int, List<ReportDsorderDetail>> diclst = QueryUpgradeDetails(para, ref totalAmount, currectpageindex, currectPageSize);
                while (diclst.Any() && diclst.First().Value != null && diclst.First().Value.Count > 0)
                {
                    List<ReportDsorderDetail> items = diclst.First().Value;
                    for (int row = 0; row < items.Count; row++)
                    {
                        var data = items[row];
                        if (currectRow == 1 || currectRow > maxExcelRow)//创建Sheet
                        {
                            #region 创建sheet
                            sheetNum++;
                            if (sheetNum > maxSheet)//超过允许的最大sheet数退出
                            {
                                workbook.Write(stream);
                                return stream.ToArray();
                            }
                            currectRow = 1;
                            sheet = workbook.CreateSheet();
                            sheet.SetColumnWidth(0, 15 * 256);
                            sheet.SetColumnWidth(1, 20 * 256);
                            sheet.SetColumnWidth(2, 20 * 256);
                            sheet.SetColumnWidth(3, 20 * 256);
                            sheet.SetColumnWidth(4, 18 * 256);
                            sheet.SetColumnWidth(5, 15 * 256);
                            sheet.SetColumnWidth(6, 15 * 256);
                            sheet.SetColumnWidth(7, 20 * 256);
                            sheet.SetColumnWidth(8, 20 * 256);
                            sheet.SetColumnWidth(9, 30 * 256);
                            sheet.SetColumnWidth(10, 15 * 256);
                            sheet.SetColumnWidth(11, 20 * 256);
                            sheet.SetColumnWidth(12, 15 * 256);
                            sheet.SetColumnWidth(13, 25 * 256);
                            excelRow = sheet.CreateRow(currectRow);
                            excelRow.HeightInPoints = 20;
                            cell = excelRow.CreateCell(0);
                            cell.SetCellValue("订单编号");
                            cell.CellStyle = cellstyle;
                            cell = excelRow.CreateCell(1);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("第三方订单号");
                            cell = excelRow.CreateCell(2);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("升舱付款时间");
                            cell = excelRow.CreateCell(3);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("商城订单时间");
                            cell = excelRow.CreateCell(4);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("所属分支机构");
                            cell = excelRow.CreateCell(5);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("客户所在城市");
                            cell = excelRow.CreateCell(6);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("商城名称");
                            cell = excelRow.CreateCell(7);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("升舱来源店面");
                            cell = excelRow.CreateCell(8);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("物流类型");
                            cell = excelRow.CreateCell(9);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("产品名称");
                            cell = excelRow.CreateCell(10);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("付款金额");
                            cell = excelRow.CreateCell(11);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("发货时间");
                            cell = excelRow.CreateCell(12);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("未发货原因");
                            cell = excelRow.CreateCell(13);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("备注");
                            #endregion
                        }
                        currectRow++;
                        excelRow = sheet.CreateRow(currectRow);
                        cell = excelRow.CreateCell(0);
                        cell.SetCellValue(data.订单编号);//订单编号
                        cell.CellStyle = cellstyle;
                        cell = excelRow.CreateCell(1);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.第三方订单号);//第三方订单号
                        cell = excelRow.CreateCell(2);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.升舱付款时间.ToString("yyyy-MM-dd HH:mm"));//升舱付款时间
                        cell = excelRow.CreateCell(3);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.商城订单时间.ToString("yyyy-MM-dd HH:mm"));//商城订单时间
                        cell = excelRow.CreateCell(4);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.所属分支机构);//所属分支机构
                        cell = excelRow.CreateCell(5);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.客户所在城市);//客户所在城市
                        cell = excelRow.CreateCell(6);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.商城名称);//商城名称
                        cell = excelRow.CreateCell(7);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.升舱来源店面);//升舱来源店面
                        cell = excelRow.CreateCell(8);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.物流类型);//物流类型
                        cell = excelRow.CreateCell(9);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.产品名称); //产品名称
                        cell = excelRow.CreateCell(10);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.付款金额.ToString()); //付款金额
                        cell = excelRow.CreateCell(11);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.发货时间); //发货时间
                        cell = excelRow.CreateCell(12);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.未发货原因); //未发货原因
                        cell = excelRow.CreateCell(13);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.备注); //备注
                    }
                    currectpageindex++;//下一页
                    diclst = QueryUpgradeDetails(para, ref totalAmount, currectpageindex, currectPageSize);//下一页数据
                }
                workbook.Write(stream);
                return stream.ToArray();
            }
            #endregion
        }
        #endregion

        #region 销售明细

        /// <summary>
        /// 销售明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,销售明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public Dictionary<int, List<RP_销售明细>> QuerySaleDetails(ref List<CBRptPaymentRecord>  PaymentRecords, SalesRmaParams para, List<int> whSelected, int userSysNo,
                                                                          int currPageIndex = 1,
                                                                          int pageSize = 10)
        {


            if (whSelected == null || !whSelected.Any())
            {
                whSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
            }
            if (!whSelected.Any())
            {
                //throw new HytException("没任何仓库的权限不能查询报表");
                return new Dictionary<int, List<RP_销售明细>>
                    {
                        {0, new List<RP_销售明细>{}}
                    };
            }

            return IReportDao.Instance.QuerySaleDetails(ref PaymentRecords, para, whSelected, userSysNo, currPageIndex, pageSize);
        }

        /// <summary>
        /// 销售明细-导出excel
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public void ExportSaleDetails(SalesRmaParams para, string userIp, int operatorSysno, List<int> whSelected)
        {
            try
            {
                var PaymentRecords = new List<CBRptPaymentRecord>();
                #region set params from Request
                //if (para.BeginDate == null && para.EndDate == null)
                //{
                //    para.BeginDate = DateTime.Now.Date;
                //    para.EndDate = DateTime.Now.Date.AddDays(1);
                //}
                #endregion

                if (whSelected == null || !whSelected.Any())
                {
                    whSelected =
                        Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                }
                var dic = new Dictionary<int, List<RP_销售明细>>
                    {
                        {0, new List<RP_销售明细> {}}
                    };
                if (whSelected.Any())
                {
                    dic = QuerySaleDetails(ref PaymentRecords, para, whSelected, operatorSysno, 1, int.MaxValue);
                }

                if (!dic.Any())
                {
                    return;
                    //return new Result { Status = false, Message = "没有数据!" };
                }
                //转换格式
                var excel = dic.First().Value.ToList();
                string mallOrderId;
                var excelExport = excel.Select(p => new CBReportSaleExport
                    {
                        下单日期 = p.下单日期,
                        订单号 = p.订单号 + "",
                        订单来源 = ((OrderStatus.销售单来源)p.订单来源).ToString(),
                        下单门店 = p.下单门店,
                        出库日期 = p.出库日期,
                        会员名 = p.会员名,
                        ERP编码 = p.ERP编码,
                        产品名称 = p.产品名称,
                        数量 = p.数量,
                        单价 = p.单价,
                        优惠 = p.优惠,
                        销售金额 = p.销售金额,
                        实收金额 = p.实收金额,
                        出入库仓库 = p.出库仓库,
                        收款方式 = p.收款方式,
                        //退款方式 = p.退款方式,
                        配送方式 = p.配送方式,
                        快递单号=p.快递单号,
                        //售后方式 = p.售后方式,
                        //发票号 = p.发票号,
                        联系电话 = p.收货电话,
                        送货员 = p.送货员,
                        客服 = p.客服,
                        结算状态 = p.结算状态,
                        店铺名称 = p.店铺名称,
                        商城订单号 = p.商城订单号,
                        收货人 = p.收货人,
                        省 = p.省,
                        市 = p.市,
                        区 = p.区,
                        收货地址 = p.收货地址,
                        对内备注 = p.对内备注,
                        出库单号 = p.出库单号,
                        发货日期 = p.发货日期

                        //结算状态 = Enum.Parse(typeof(WarehouseStatus.退换货单状态), p.结算状态).ToString()
                    }).ToList();

                var fileName = string.Format("销售明细报表({0}至{1})",
                                        para.BeginDate.HasValue ? para.BeginDate.Value.ToString("yy-MM-dd") : "",
                                        (para.EndDate.HasValue ? para.EndDate.Value : DateTime.Now).ToString("yy-MM-dd"));
                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportLargeData<CBReportSaleExport>(excelExport, new List<string>
                    {
                        "下单日期","订单号","订单来源","下单门店","出库日期","会员名","ERP编码","产品名称","数量","单价","优惠","销售金额","实收金额","出库仓库","收款方式","配送方式","快递单号",
"联系电话","送货员","客服","结算状态","店铺名称","商城订单号","收货人","省","市","区","收货地址","对内备注","出库单号","发货日期"
                    }, fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售明细导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售明细导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
                //return new Result { Status = false, Message = "导出excel发生错误!" };

            }

            //return new Result { Status = true, Message = "导出excel完成!" };
        }

        #endregion

        #region 退换货明细

        /// <summary>
        /// 退换货明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,退换货明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public Dictionary<int, List<RP_退换货明细>> QueryRmaDetails(SalesRmaParams para, List<int> whSelected, int userSysNo,
                                                                          int currPageIndex = 1,
                                                                          int pageSize = 10)
        {
            if (whSelected == null || !whSelected.Any())
            {
                whSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
            }
            if (!whSelected.Any())
            {
                //throw new HytException("没任何仓库的权限不能查询报表");
                return new Dictionary<int, List<RP_退换货明细>>
                    {
                        {0, new List<RP_退换货明细>{}}
                    };
            }

            return IReportDao.Instance.QueryRmaDetails(para, whSelected, userSysNo, currPageIndex, pageSize);
        }

        /// <summary>
        /// 退换货明细-导出excel
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public void ExportRmaDetails(SalesRmaParams para, string userIp, int operatorSysno, List<int> whSelected)
        {
            try
            {

                #region set params from Request
                //if (para.BeginDate == null && para.EndDate == null)
                //{
                //    para.BeginDate = DateTime.Now.Date;
                //    para.EndDate = DateTime.Now.Date.AddDays(1);
                //}
                #endregion

                var dic = new Dictionary<int, List<RP_退换货明细>>
                    {
                        {0, new List<RP_退换货明细>{}}
                    };

                if (whSelected == null || !whSelected.Any())
                {
                    whSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                }
                if (whSelected.Any())
                {
                    dic = QueryRmaDetails(para, whSelected, operatorSysno, 1, int.MaxValue);
                }


                if (!dic.Any())
                {
                    return;
                    //return new Result { Status = false, Message = "没有数据!" };
                }
                //转换格式
                var excel = dic.First().Value.ToList();

                var excelExport = excel.Select(p => new CBReportRmaExport
                {
                    //下单日期 = p.下单日期,
                    订单号 = p.订单号 + "",
                    订单来源 = ((OrderStatus.销售单来源)p.订单来源).ToString(),
                    申请日期 = p.申请日期,
                    //下单门店=p.下单门店,
                    入库日期 = p.入库日期,
                    会员名 = p.会员名,
                    ERP编码 = p.ERP编码,
                    产品名称 = p.产品名称,
                    数量 = p.数量,
                    单价 = p.单价,
                    优惠 = p.优惠,
                    退款金额 = p.退款金额,
                    实退金额 = p.实退金额,
                    入库仓库 = p.入库仓库,
                    收款方式 = p.收款方式,
                    退款方式 = ((RmaStatus.退换货退款方式)p.退款方式).ToString(),
                    配送方式 = p.配送方式,
                    售后方式 = p.售后方式,
                    联系电话 = p.收货电话,
                    结算状态 = p.结算状态,
                    入库单号 = p.入库单号,
                    下单门店=p.下单门店
                }).ToList();

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBReportRmaExport>(excelExport, new List<string>
                    {
                        "订单号","订单来源","申请日期","入库日期","会员名","ERP编码","产品名称","数量","单价","优惠","退款金额","实退金额","入库仓库","收款方式","退款方式","配送方式","售后方式","联系电话","结算状态","入库单号","下单门店"
                    });

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "退换货明细导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "退换货明细导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
                //return new Result { Status = false, Message = "导出excel发生错误!" };

            }

            //return new Result { Status = true, Message = "导出excel完成!" };
        }

        #endregion

        #region 市场部赠送明细

        /// <summary>
        /// 市场部赠送明细
        /// </summary>
        /// <param name="para">ReportMarketDepartmentSale</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,市场部赠送明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public Dictionary<int, List<ReportMarketDepartmentSale>> QueryMarketPresentDetails(
            CBReportMarketDepartmentSale para,
            int currPageIndex = 1,
            int pageSize = 10)
        {
            return IReportDao.Instance.QueryMarketPresentDetails(para, currPageIndex, pageSize);
        }

        /// <summary>
        /// 市场部赠送明细-导出excel
        /// </summary>
        /// <param name="para">CBReportMarketDepartmentSale</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public void ExportMarketPresentDetails(string userIp, int operatorSysno)
        {
            try
            {
                var para = new CBReportMarketDepartmentSale();

                #region set params from Request
                var startDate = HttpContext.Current.Request.Params["BeginDate"];
                if (!string.IsNullOrEmpty(startDate))
                {
                    var temp = startDate.Split('-');
                    para.BeginDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
                }

                var endDate = HttpContext.Current.Request.Params["EndDate"];
                if (!string.IsNullOrEmpty(endDate))
                {
                    var temp = endDate.Split('-');
                    para.EndDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
                }

                #endregion

                var dic = QueryMarketPresentDetails(para, 1, int.MaxValue);
                if (!dic.Any())
                {
                    return;
                    //return new Result { Status = false, Message = "没有数据!" };
                }
                //转换格式
                var excel = dic.First().Value.ToList();

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export(excel);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
                //return new Result { Status = false, Message = "导出excel发生错误!" };

            }

            //return new Result { Status = true, Message = "导出excel完成!" };
        }

        #endregion

        #region 业务员绩效

        /// <summary>
        /// 业务员绩效查询
        /// </summary>
        /// <param name="para">RP_绩效_业务员</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,业务员绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public Dictionary<int, List<RP_绩效_业务员>> QueryBusinessManPerformance(ParaBusinessManPerformance para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10)
        {

            if (warehouseSysNos == null || !warehouseSysNos.Any())
            {
                warehouseSysNos = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
            }
            if (!warehouseSysNos.Any())
            {
                return new Dictionary<int, List<RP_绩效_业务员>>
                    {
                        {0, new List<RP_绩效_业务员>{}}
                    };
            }

            para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;
            return IReportDao.Instance.QueryBusinessManPerformance(para, warehouseSysNos, currPageIndex, pageSize);
        }

        /// <summary>
        /// 业务员绩效-导出excel
        /// </summary>
        /// <param name="para">ParaBusinessManPerformance</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public void ExportBusinessManPerformance(ParaBusinessManPerformance para, string userIp, int operatorSysno, List<int> warehouseSysNos)
        {
            try
            {

                if (warehouseSysNos == null || !warehouseSysNos.Any())
                {
                    warehouseSysNos =
                        Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                }
                var dic = new Dictionary<int, List<RP_绩效_业务员>>
                    {
                        {0, new List<RP_绩效_业务员> {}}
                    };
                if (warehouseSysNos.Any())
                {
                    dic = QueryBusinessManPerformance(para, warehouseSysNos, 1, int.MaxValue);
                }

                if (!dic.Any())
                {
                    return;
                    //return new Result { Status = false, Message = "没有数据!" };
                }
                //转换格式
                var excel = dic.First().Value.ToList();

                var excelExport = excel.Select(p => new CBBusManPerformanceExport
                {
                    姓名 = p.姓名,
                    仓库 = p.仓库,
                    配送次数白班 = p.配送次数_白班,
                    配送次数夜班 = p.配送次数_夜班,
                    配送单量白班 = p.配送单量_白班,
                    配送单量夜班 = p.配送单量_夜班,
                    升舱 = p.配送金额_升舱,
                    商城 = p.配送金额_商城,
                    自销金额 = p.自销金额,

                }).ToList();
                para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;
                //业务员绩效报表(13-12-01至13-12-31)
                string dateStart = para.统计日期.Split('-')[0].Substring(2, 2) + "-" + para.统计日期.Split('-')[1] + "-" + "01",
                    lastDayInMonth = DateTime.DaysInMonth(int.Parse(para.统计日期.Split('-')[0]), int.Parse(para.统计日期.Split('-')[1])) + "",
                    dateEnd = para.统计日期.Split('-')[0].Substring(2, 2) + "-" + para.统计日期.Split('-')[1] + "-" + lastDayInMonth;

                var fileName = "业务员绩效报表(" + dateStart + "至" + dateEnd + ")";
                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportFromTemplate(excelExport, @"\Templates\Excel\BusinessManPerformance.xls", 3, fileName);
                //Util.ExcelUtil.Export(excelExport, new List<string>
                //    {
                //        "姓名","仓库","配送次数(白班)","配送次数(夜班)","配送单量(白班)","配送单量(白班)","配送金额","自销金额"
                //    }, fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "业务员绩效导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "业务员绩效导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
                //return new Result { Status = false, Message = "导出excel发生错误!" };

            }

            //return new Result { Status = true, Message = "导出excel完成!" };
        }

        #endregion


        #region 办事处绩效

        /// <summary>
        /// 办事处绩效查询
        /// </summary>
        /// <param name="para">rp_绩效_办事处</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,办事处绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public Dictionary<int, List<rp_绩效_办事处>> QueryOfficePerformance(rp_绩效_办事处 para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10)
        {
            para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;
            return IReportDao.Instance.QueryOfficePerformance(para, warehouseSysNos, currPageIndex, pageSize);
        }

        /// <summary>
        /// 办事处绩效-导出excel
        /// </summary>
        /// <param name="para">ParaBusinessManPerformance</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <param name="warehouseSysNos">选中的仓库</param>
        /// <returns></returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public void ExportOfficePerformance(rp_绩效_办事处 para, string userIp, int operatorSysno, List<int> warehouseSysNos)
        {
            try
            {
                var dic = QueryOfficePerformance(para, warehouseSysNos, 1, int.MaxValue);
                if (!dic.Any())
                {
                    return;
                    //return new Result { Status = false, Message = "没有数据!" };
                }
                //转换格式
                var excel = dic.First().Value.ToList();

                var excelExport = excel.Select(p => new CBOfficePerformanceExport()
                {
                    办事处 = p.办事处,
                    配送_升舱_百城达 = p.升舱百城达,
                    配送_升舱_第三方 = p.升舱第三方,
                    配送_信营全球购B2B2C_百城达 = p.商城百城达,
                    配送_信营全球购B2B2C_第三方 = p.商城第三方,
                    配送_代发_百城达 = p.代发百城达,
                    配送_代发_第三方 = p.代发第三方
                }).ToList();

                para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;

                string dateStart = para.统计日期.Split('-')[0].Substring(2, 2) + "-" + para.统计日期.Split('-')[1] + "-" + "01",
                                    lastDayInMonth = DateTime.DaysInMonth(int.Parse(para.统计日期.Split('-')[0]), int.Parse(para.统计日期.Split('-')[1])) + "",
                                    dateEnd = para.统计日期.Split('-')[0].Substring(2, 2) + "-" + para.统计日期.Split('-')[1] + "-" + lastDayInMonth;

                var fileName = "办事处绩效报表(" + dateStart + "至" + dateEnd + ")";
                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportFromTemplate(excelExport, @"\Templates\Excel\OfficePerformance.xls", 4, fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "办事处绩效导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "办事处绩效导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
                //return new Result { Status = false, Message = "导出excel发生错误!" };

            }

            //return new Result { Status = true, Message = "导出excel完成!" };
        }

        #endregion

        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="condition">出库单查询条件</param>
        /// <returns></returns>
        public  DataTable GetStockOutList(StockOutSearchCondition condition)
        {
            return IReportDao.Instance.GetStockOutList(condition);
        }
        #region 支付记录统计报表
        /// <summary>
        /// 支付记录
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns>返回支付记录信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public Pager<CBRptPaymentRecord> GetMethodPaymentRecordList(MethodPaymentRecordFilter filter)
        {
            return IReportDao.Instance.GetMethodPaymentRecordList(filter);
        }
        /// <summary>
        /// 支付记录导出
        /// </summary>
        ///<param name="filter">筛选信息</param>
        /// <returns></returns>
        ///  <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public List<CBRptPaymentRecord> GetExportMethodPaymentRecordList(MethodPaymentRecordFilter filter)
        {
            return IReportDao.Instance.GetExportMethodPaymentRecordList(filter);
        }
        /// <summary>
        ///  支付记录导出
        /// </summary>
        ///  <param name="filter">筛选信息</param>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public void ExportMethodPaymentRecord(MethodPaymentRecordFilter filter, string userIp, int operatorSysno)
        {
            try
            {
                // 查询
                List<CBRptPaymentRecord> exportRebatesRecord = ReportBO.Instance.GetExportMethodPaymentRecordList(filter);

                var fileName = string.Format("支付方式总金额统计报表({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 经销商名称
                 * 会员数
                 * 营业额
                 */
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBRptPaymentRecord>(exportRebatesRecord,
                    new List<string> { "支付方式系统编号", "支付方式", "总金额", "起止时间" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "支付方式总金额统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "支付方式总金额统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        #endregion

        /// <summary>
        /// 销售排行统计
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-10-22 朱家宏 创建</remarks>
        public IList<RptSalesRanking> GetSalesRanking(ParaRptSalesRankingFilter filter)
        {
            filter.TakingCount = 5000;
            if (filter.BeginDate == null && filter.EndDate == null)
            {
                var today = DateTime.Today;
                switch (filter.DateRange)
                {
                    case (int)ParaDateRange.今天: //今天
                        filter.BeginDate = today;
                        break;
                    case (int)ParaDateRange.本周: //本周
                        var monday = today.AddDays(1 - Convert.ToInt32(today.DayOfWeek)); //本周一
                        filter.BeginDate = monday;
                        break;
                    case (int)ParaDateRange.本月: //本月
                        var firstDayOfmonth = today.AddDays(1 - today.Day); //本月初
                        filter.BeginDate = firstDayOfmonth;
                        break;
                }
            }
            var allPdCategories = DataAccess.Product.IPdCategoryDao.Instance.GetAllCategory();
            if (filter.ProductCategories != null && filter.ProductCategories.Any())
            {
               
                var children = new List<PdCategory>();
                foreach (var c in filter.ProductCategories)
                {
                    if (c > 0)
                    {
                        var currentNode = DataAccess.Product.IPdCategoryDao.Instance.GetCategory(c);
                        if (currentNode != null)
                        {
                            children.Add(currentNode);
                        }
                        DoChildNodeRead(c, allPdCategories, ref children);
                    }
                }
                filter.ProductCategories.Clear();
                foreach (var c in children)
                {
                    filter.ProductCategories.Add(c.SysNo);
                }
                if (!filter.ProductCategories.Any())
                    filter.ProductCategories = null;
            }

            IList<RptSalesRanking> webSale = new List<RptSalesRanking>();
            IList<RptSalesRanking> posSale = new List<RptSalesRanking>();
            if (string.IsNullOrEmpty(filter.ProductSaleType) || filter.ProductSaleType == "网店销售")
            {
                webSale = IReportDao.Instance.SelectSalesRanking(filter);
                for (int i = 0; i < webSale.Count; i++)
                {
                    if (string.IsNullOrEmpty(filter.ProductSaleType))
                    {
                        webSale[i].SaleProType += "网店销售：" + webSale[i].SalesQuantity+"，";
                    }
                    else
                    {
                        webSale[i].SaleProType = "网店销售";
                    }

                    var _categorySysNos = webSale[i].ProductCategorySysNos == null ? "" : webSale[i].ProductCategorySysNos;
                    string[] CategorySysNos = _categorySysNos.Split(',');
                    string cateName = "";
                    foreach(string str in CategorySysNos)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            var tempCategory = allPdCategories.First(p => p.SysNo.ToString() == str);
                            if (!string.IsNullOrEmpty(cateName))
                            {
                                cateName += "→";
                            }
                            cateName += tempCategory.CategoryName;
                        }
                    }
                    webSale[i].ProductCategoryName = cateName;
                }
            }

            if (string.IsNullOrEmpty(filter.ProductSaleType) || filter.ProductSaleType == "门店销售")
            {
                try
                {
                    posSale = GetLineOutSaleRanking(filter);
                }
                catch
                {

                }
            }
            foreach (RptSalesRanking item in posSale)
            {
                List<RptSalesRanking> checkWebSale = webSale.Where(p => p.ProductSysNo == item.ProductSysNo).ToList();
                if (checkWebSale.Count > 0)
                {
                    checkWebSale[0].SalesQuantity += item.SalesQuantity;
                    checkWebSale[0].SalesAmount += item.SalesAmount;
                    if( checkWebSale[0].SaleProType.IndexOf("门店销售")==-1)
                    {
                        checkWebSale[0].SaleProType += "门店销售：" + item.SalesQuantity;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(filter.ProductSaleType))
                    {
                        item.SaleProType += "门店销售：" + item.SalesQuantity;
                    }
                    else
                    {
                        item.SaleProType = "门店销售";
                    }

                    string[] CategorySysNos = item.ProductCategorySysNos.Split(',');
                    string cateName = "";
                    foreach (string str in CategorySysNos)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            var tempCategory = allPdCategories.First(p => p.SysNo.ToString() == str);
                            if (!string.IsNullOrEmpty(cateName))
                            {
                                cateName += "→";
                            }
                            cateName += tempCategory.CategoryName;
                        }
                    }
                    item.ProductCategoryName = cateName;

                    webSale.Add(item);
                }
            }
            webSale = webSale.OrderByDescending(p => p.SalesAmount).ToList();
            for (int i = 0; i < webSale.Count; i++)
            {
                webSale[i].RowNumber = i + 1;
            }
            return webSale;
        }

        /// <summary>
        /// 筛选线下收银的商品销售统计
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2016-08-05 杨云奕 添加</remarks>
        public IList<RptSalesRanking> GetLineOutSaleRanking(ParaRptSalesRankingFilter filter)
        {
            filter.TakingCount = 5000;
            if (filter.BeginDate == null && filter.EndDate == null)
            {
                var today = DateTime.Today;
                switch (filter.DateRange)
                {
                    case (int)ParaDateRange.今天: //今天
                        filter.BeginDate = today;
                        break;
                    case (int)ParaDateRange.本周: //本周
                        var monday = today.AddDays(1 - Convert.ToInt32(today.DayOfWeek)); //本周一
                        filter.BeginDate = monday;
                        break;
                    case (int)ParaDateRange.本月: //本月
                        var firstDayOfmonth = today.AddDays(1 - today.Day); //本月初
                        filter.BeginDate = firstDayOfmonth;
                        break;
                }
            }

            if (filter.ProductCategories != null && filter.ProductCategories.Any())
            {
                var allPdCategories = DataAccess.Product.IPdCategoryDao.Instance.GetCategoryList(null);
                var children = new List<PdCategory>();
                foreach (var c in filter.ProductCategories)
                {
                    if (c > 0)
                    {
                        var currentNode = DataAccess.Product.IPdCategoryDao.Instance.GetCategory(c);
                        if (currentNode != null)
                        {
                            children.Add(currentNode);
                        }
                        DoChildNodeRead(c, allPdCategories, ref children);
                    }
                }
                filter.ProductCategories.Clear();
                foreach (var c in children)
                {
                    filter.ProductCategories.Add(c.SysNo);
                }
                if (!filter.ProductCategories.Any())
                    filter.ProductCategories = null;
            }
            return IReportDao.Instance.GetLineOutSaleRanking(filter);
        }

        /// <summary>
        /// 筛选线下收银的商品销售统计
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2016-08-05 杨云奕 添加</remarks>
        public List<RptSalesRanking> GetLineOutSaleRankingByWarehouse(ParaWarehouseProductSalesFilter filter)
        {
            filter.TakingCount = 5000;
           

            if (filter.ProductCategories != null && filter.ProductCategories.Any())
            {
                var allPdCategories = DataAccess.Product.IPdCategoryDao.Instance.GetCategoryList(null);
                var children = new List<PdCategory>();
                foreach (var c in filter.ProductCategories)
                {
                    if (c > 0)
                    {
                        var currentNode = DataAccess.Product.IPdCategoryDao.Instance.GetCategory(c);
                        if (currentNode != null)
                        {
                            children.Add(currentNode);
                        }
                        DoChildNodeRead(c, allPdCategories, ref children);
                    }
                }
                filter.ProductCategories.Clear();
                foreach (var c in children)
                {
                    filter.ProductCategories.Add(c.SysNo);
                }
                if (!filter.ProductCategories.Any())
                    filter.ProductCategories = null;
            }
            return IReportDao.Instance.GetLineOutSaleRankingByWarehouse(filter);
        }

        /// <summary>
        /// 获取商品分类子类
        /// </summary>
        /// <param name="categorySysNo">分类编号</param>
        /// <param name="allCategories">类库</param>
        /// <param name="children">子类</param>
        /// <returns></returns>
        /// <remarks>2013-12-17 朱家宏 创建</remarks>
        public void DoChildNodeRead(int categorySysNo, IList<PdCategory> allCategories, ref List<PdCategory> children)
        {
            var childrenNodes = allCategories.Where(o => o.ParentSysNo == categorySysNo).ToList();

            if (childrenNodes.Any())
            {
                foreach (var node in childrenNodes)
                {
                    children.Add(node);
                    DoChildNodeRead(node.SysNo, allCategories, ref children);
                }
            }
        }

        /// <summary>
        /// 运营综述日报
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-17 余勇 创建</remarks>
        public IList<CBReportBusinessSummary> QueryBusinessSummary(ParaRptBusinessSummaryFilter filter)
        {
            return IReportDao.Instance.QueryBusinessSummary(filter);
        }

        /// <summary>
        /// 运营综述月报
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        public IList<CBReportBusinessSummary> QueryBusinessSummaryMonthly(ParaRptBusinessSummaryFilter filter)
        {
            return IReportDao.Instance.QueryBusinessSummaryMonthly(filter);
        }

        /// <summary>
        /// 门店会员消费报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        public Pager<CBReportShopCustomerConsume> QueryShopCustomerConsume(ParaRptShopCustomerConsumeFilter filter)
        {
            return IReportDao.Instance.QueryShopCustomerConsume(filter);
        }

        /// <summary>
        /// 系统用户分页列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>用户分页列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public Pager<RP_绩效_电商中心> GetListEBusinessCenter(ParaEBusinessCenterPerformanceFilter filter)
        {
            var pager = IReportDao.Instance.GetListEBusinessCenter(filter);
            FillMallInfo(pager);
            return pager;
        }

        /// <summary>
        /// 填充店铺名称和商城类型
        /// </summary>
        /// <param name="pager">绩效_电商中心分页数据</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private void FillMallInfo(Pager<RP_绩效_电商中心> pager)
        {
            var allMall = MallSeller.DsEasBo.Instance.GetAllMall();
            pager.Rows.ApplyParallel(item =>
            {
                var dealerMall = allMall.SingleOrDefault(i => i.SysNo == item.分销商商城编号);
                if (dealerMall != null)
                {
                    item.店铺名称 = dealerMall.ShopName;
                    item.商城类型 = Enum.Parse(typeof(DistributionStatus.商城类型预定义), dealerMall.MallTypeSysNo.ToString()).ToString();
                }
            });
        }

        /// <summary>
        /// 客服绩效分页列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>客服绩效列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public Pager<RP_绩效_客服> GetListServicePerformance(ParaServicePerformanceFilter filter)
        {
            var pager = IReportDao.Instance.GetListServicePerformance(filter);
            return pager;
        }

        /// <summary>
        /// 门店新增会员分页列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>门店新增会员列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public Pager<RP_绩效_门店新增会员> GetListShopNewCustomer(ParaShopNewCustomerFilter filter)
        {
            var pager = IReportDao.Instance.GetListShopNewCustomer(filter);
            return pager;
        }

        /// <summary>
        /// 门店新增会员明细分页列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>门店新增会员列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public Pager<rp_ShopNewCustomerDetail> GetListShopNewCustomerDetail(ParaShopNewCustomerFilter filter)
        {
            var pager = IReportDao.Instance.GetListShopNewCustomerDetail(filter);
            return pager;
        }

        /// <summary>
        /// 查询门店新增会员明细
        /// </summary>
        /// <param name="customerSysno">客户编号</param>
        /// <returns>门店新增会员明细</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public rp_ShopNewCustomerDetail SelectShopNewCustomerDetail(int customerSysno)
        {
            return IReportDao.Instance.SelectShopNewCustomerDetail(customerSysno);
        }

        /// <summary>
        /// 新增门店新增会员明细
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public int InsertShopNewCustomerDetail(rp_ShopNewCustomerDetail entity)
        {
            return IReportDao.Instance.InsertShopNewCustomerDetail(entity);
        }

        /// <summary>
        /// 更新门店新增会员明细消费金额
        /// </summary>
        /// <param name="model">门店新增会员明细实体</param>
        /// <param name="amount">消费金额</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public int UpdateShopNewCustomerDetail(rp_ShopNewCustomerDetail model, decimal amount)
        {
            return IReportDao.Instance.UpdateShopNewCustomerDetail(model.CustomerSysno, model.Amount + amount);
        }

        /// <summary>
        /// 获取全部客服
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public List<SyUser> GetAllService()
        {
            return IReportDao.Instance.GetAllService();
        }

        /// <summary>
        /// 获取仓库内勤绩效报表
        /// </summary>
        /// <param name="pagerFilter">页面传入的过滤条件</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否具有全部仓库权限</param>
        /// <returns></returns>
        /// <remarks>2013-12-11 沈强 创建</remarks>
        public Pager<rp_仓库内勤> SearchWarehouseInsideStaff(
            Pager<ParaWarehouseInsideStaffFilter> pagerFilter, int currentUserSysNo,
            bool hasAllWarehouse)
        {
            return IReportDao.Instance.SearchWarehouseInsideStaff(pagerFilter, currentUserSysNo, hasAllWarehouse);
        }
        #region 配送单报表
        /// <summary>
        /// 获取分销商配送单报表数据
        /// </summary>
        /// <param name="dealershopNos">分销商商城编号</param>
        /// <param name="yyyymm">年月</param>
        /// <returns>配送报表数据</returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks>
        public List<CBPickingReportItem> GetPickingReport(string yyyymm, int pageindex)
        {
            return Hyt.DataAccess.Report.IReportDao.Instance.GetPickingReport(yyyymm, pageindex);
        }
        private int rownum = 1;//记录总的记录数

        /// <summary>
        /// 导出分销商配送单报表Excel
        /// </summary>
        /// <param name="yyyymm">年月</param>
        /// <returns>true/false</returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks> 
        public void ExportPickingReportExcle(string yyyymm)
        {
            List<CBPickingReportItem> lst = null;
            List<CBPickingReportItem> sublst = new List<CBPickingReportItem>();
            int pageindex = 1;//分页获取数据库数据
            int stockoutno = 0;//当前出库单号
            int shopno = 0;//当前商铺
            string shopname = string.Empty;//店铺名称
            ISheet sheet = null;
            int rowindex = 0;//读写开始行号
            int maxindex = 65000;//最大行
            var configPath = System.Configuration.ConfigurationManager.AppSettings["reportfilepath"];
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = "~/download/reportfile/";
            }
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath(configPath) + "\\" + yyyymm + ".xls";
            using (FileStream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ICellStyle cellstyle = workbook.CreateCellStyle();
                ICellStyle cellstyle1 = workbook.CreateCellStyle();
                ICellStyle cellstyle2 = workbook.CreateCellStyle();
                IFont fontTitle = workbook.CreateFont();
                fontTitle.FontHeightInPoints = 10;
                fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.WrapText = true;
                cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle1.CloneStyleFrom(cellstyle);
                cellstyle1.SetFont(fontTitle);
                cellstyle2.CloneStyleFrom(cellstyle);
                cellstyle2.Alignment = HorizontalAlignment.Left;
                while (pageindex == 1 || (lst != null && lst.Count > 0))
                {
                    lst = GetPickingReport(yyyymm, pageindex);//分页获取数据，防止一次性加载内存溢出
                    foreach (CBPickingReportItem item in lst)
                    {
                        if (sublst.Count == 0 || item.stockoutno == stockoutno)
                        {
                            sublst.Add(item);
                        }
                        else
                        {
                            if (sublst[0].shopno != shopno || rowindex > maxindex)
                            {
                                #region 创建sheet
                                shopno = sublst[0].shopno;
                                shopname = sublst[0].shopname;
                                if (rowindex > maxindex)//超过2003允许的最大行
                                {
                                    sheet = workbook.CreateSheet(sheet.SheetName + "_1");
                                }
                                else
                                {
                                    sheet = workbook.CreateSheet("(" + sublst[0].shopno + ")" + sublst[0].shopname + yyyymm);
                                }
                                sheet.SetColumnWidth(0, 5 * 256);
                                sheet.SetColumnWidth(1, 10 * 256);
                                sheet.SetColumnWidth(2, 40 * 256);
                                sheet.SetColumnWidth(3, 15 * 256);
                                sheet.SetColumnWidth(4, 15 * 256);
                                sheet.SetColumnWidth(5, 15 * 256);
                                sheet.SetColumnWidth(6, 15 * 256);
                                sheet.SetColumnWidth(7, 20 * 256);
                                rowindex = 0;
                                rownum = 1;
                                #endregion
                            }
                            rowindex = CreatePickingReportExcelRow(sheet, rowindex, sublst, cellstyle, cellstyle1, cellstyle2);
                            sublst.Clear();
                            sublst.Add(item);
                        }
                        stockoutno = item.stockoutno;//当前出库单
                    }
                    pageindex++;//转到下一页
                }
                if (sublst.Count > 0)//未处理的数据
                {
                    if (sublst[0].shopno != shopno || rowindex > maxindex)
                    {
                        #region 创建sheet
                        shopno = sublst[0].shopno;
                        shopname = sublst[0].shopname;
                        if (rowindex > maxindex)//超过2003允许的最大行
                        {
                            sheet = workbook.CreateSheet(sheet.SheetName + "_1");
                        }
                        else
                        {
                            sheet = workbook.CreateSheet("(" + sublst[0].shopno + ")" + sublst[0].shopname + yyyymm);
                        }
                        sheet.SetColumnWidth(0, 5 * 256);
                        sheet.SetColumnWidth(1, 10 * 256);
                        sheet.SetColumnWidth(2, 40 * 256);
                        sheet.SetColumnWidth(3, 15 * 256);
                        sheet.SetColumnWidth(4, 15 * 256);
                        sheet.SetColumnWidth(5, 15 * 256);
                        sheet.SetColumnWidth(6, 15 * 256);
                        sheet.SetColumnWidth(7, 20 * 256);
                        rowindex = 0;
                        rownum = 1;
                        #endregion
                    }
                    rowindex = CreatePickingReportExcelRow(sheet, rowindex, sublst, cellstyle, cellstyle1, cellstyle2);
                    sublst.Clear();
                }
                workbook.Write(fileStream);
            }
        }

        /// <summary>
        /// 生成出库单对应的配送单数据
        /// </summary>
        /// <param name="sheet">sheet</param>
        /// <param name="startindex">开始行</param>
        /// <param name="sublst">配送单数据</param>
        /// <param name="cellstyle">单元格样式</param>
        /// <param name="cellstyle1">单元格样式1</param>
        /// <param name="cellstyle2">单元格样式2</param>
        /// <returns>新的开始行</returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks> 
        private int CreatePickingReportExcelRow(ISheet sheet, int startindex, List<CBPickingReportItem> sublst, ICellStyle cellstyle, ICellStyle cellstyle1, ICellStyle cellstyle2)
        {
            int top = startindex;
            var first = sublst.FirstOrDefault();
            IRow row = sheet.CreateRow(startindex);
            row.HeightInPoints = 30;
            ICell cell = row.CreateCell(0);
            cell.CellStyle = cellstyle;
            cell.SetCellValue(rownum.ToString());
            cell = row.CreateCell(1);
            cell.CellStyle = cellstyle;
            cell.SetCellValue(first.ordersysno);
            cell = row.CreateCell(2);
            cell.CellStyle = cellstyle;
            cell.SetCellValue(first.shopname);
            cell = row.CreateCell(3);
            cell.CellStyle = cellstyle2;
            string local = string.Empty;
            if (!string.IsNullOrEmpty(first.areaallname))
            {
                string[] ff = first.areaallname.Split(',');
                for (int j = ff.Length - 1; j > -1; j--)
                {
                    local += ff[j] + " ";
                }
            }
            cell.SetCellValue(first.name + "  " + first.mobilephonenumber + " " + local + "\n" + first.streetaddress);
            cell = row.CreateCell(7);
            cell.CellStyle = cellstyle2;
            startindex++;
            row = sheet.CreateRow(startindex);
            row.HeightInPoints = 20;
            cell = row.CreateCell(0);
            cell.CellStyle = cellstyle;
            cell = row.CreateCell(1);
            cell.CellStyle = cellstyle;
            cell = row.CreateCell(2);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("商品名称");
            cell = row.CreateCell(3);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("数量");
            cell = row.CreateCell(4);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("单价");
            cell = row.CreateCell(5);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("小计");
            cell = row.CreateCell(6);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("支付方式");
            cell = row.CreateCell(7);
            cell.CellStyle = cellstyle1;
            cell.SetCellValue("物流信息");
            foreach (CBPickingReportItem item in sublst)
            {
                startindex++;
                row = sheet.CreateRow(startindex);
                cell = row.CreateCell(0);
                cell.CellStyle = cellstyle;
                cell = row.CreateCell(1);
                cell.CellStyle = cellstyle;
                cell = row.CreateCell(2);
                cell.CellStyle = cellstyle2;
                cell.SetCellValue(item.erpcode + "★" + item.mallorderid + "\n" + item.ProductName);
                cell = row.CreateCell(3);
                cell.CellStyle = cellstyle;
                cell.SetCellValue(item.ProductQuantity);
                cell = row.CreateCell(4);
                cell.CellStyle = cellstyle;
                cell.SetCellValue(item.OriginalPrice.ToString());
                cell = row.CreateCell(5);
                cell.CellStyle = cellstyle;
                cell.SetCellValue(item.RealSalesAmount.ToString());
                cell = row.CreateCell(6);
                cell.CellStyle = cellstyle;
                cell.SetCellValue("分销商预存");
                cell = row.CreateCell(7);
                cell.CellStyle = cellstyle;
                if (string.IsNullOrEmpty(item.expressno))
                {
                    cell.SetCellValue(item.deliverytypename);
                }
                else
                {
                    cell.SetCellValue(item.deliverytypename + "\n※" + item.expressno);
                }
            }

            startindex++;
            row = sheet.CreateRow(startindex);
            for (int i = 0; i <= 7; i++)
            {
                cell = row.CreateCell(i);
                cell.CellStyle = cellstyle;
                if (i == 2)
                {
                    cell.SetCellValue("出库单金额: " + first.StockOutAmount + " 运费：" + first.freightamount + " 应收: " + first.Receivable + " 实收: ");
                }
            }
            CellRangeAddress cellRangeAddress = new CellRangeAddress(startindex, startindex, 2, 7);
            sheet.AddMergedRegion(cellRangeAddress);
            startindex++;
            row = sheet.CreateRow(startindex);
            for (int i = 0; i <= 7; i++)
            {
                cell = row.CreateCell(i);
                if (i == 2)
                {
                    cell.SetCellValue("备注: " + first.remarks);
                }
                cell.CellStyle = cellstyle2;
            }
            cellRangeAddress = new CellRangeAddress(startindex, startindex, 2, 7);
            sheet.AddMergedRegion(cellRangeAddress);
            cellRangeAddress = new CellRangeAddress(top, startindex, 1, 1);
            sheet.AddMergedRegion(cellRangeAddress);
            cellRangeAddress = new CellRangeAddress(top, startindex, 0, 0);
            sheet.AddMergedRegion(cellRangeAddress);
            cellRangeAddress = new CellRangeAddress(top, top, 3, 7);
            sheet.AddMergedRegion(cellRangeAddress);
            startindex++;
            rownum++;
            return startindex;
        }
        #endregion

        #region 优惠卡统计表

        /// <summary>
        /// 优惠卡统计表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-02-26 朱家宏 创建</remarks>
        public IList<CBRptCouponCard> GetCouponCards(ParaRptCouponCardFilter filter)
        {
            return IReportDao.Instance.QueryCouponCards(filter);
        }

        /// <summary>
        /// 优惠卡统计表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-04-02 朱家宏 创建</remarks>
        public Pager<CBRptCouponCard> GetCouponCardsNew(ParaRptCouponCardFilter filter)
        {
            return IReportDao.Instance.QueryCouponCardsNew(filter);
        }

        #endregion

        #region 仓库销售排行报表
        /// <summary>
        /// 仓库销售排行报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>数据</returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        public List<RptSalesRanking> QueryWarehouseProductSales(ParaWarehouseProductSalesFilter filter)
        {
            var allPdCategories = DataAccess.Product.IPdCategoryDao.Instance.GetCategoryList(null);
            if (filter.ProductCategories != null && filter.ProductCategories.Any())
            {
               
                var children = new List<PdCategory>();
                foreach (var c in filter.ProductCategories)
                {
                    if (c > 0)
                    {
                        var currentNode = DataAccess.Product.IPdCategoryDao.Instance.GetCategory(c);
                        if (currentNode != null)
                        {
                            children.Add(currentNode);
                        }
                        DoChildNodeRead(c, allPdCategories, ref children);
                    }
                }
                filter.ProductCategories.Clear();
                foreach (var c in children)
                {
                    filter.ProductCategories.Add(c.SysNo);
                }
                if (!filter.ProductCategories.Any())
                    filter.ProductCategories = null;
            }
            List<RptSalesRanking> webSale = new List<RptSalesRanking>();
            List<RptSalesRanking> posSale = new List<RptSalesRanking>();
            if (string.IsNullOrEmpty(filter.ProductSaleType) || filter.ProductSaleType == "网店销售")
            {
                webSale = IReportDao.Instance.QueryWarehouseProductSales(filter);
                Dictionary<int, RptSalesRanking> saleWarehouse = new Dictionary<int, RptSalesRanking>();
                foreach (RptSalesRanking tmod in webSale)
                {
                    if(saleWarehouse.ContainsKey(tmod.ProductSysNo))
                    {
                        saleWarehouse[tmod.ProductSysNo].SalesQuantity += tmod.SalesQuantity;
                    }
                    else
                    {
                        saleWarehouse.Add(tmod.ProductSysNo, tmod);
                    }
                }
                webSale = new List<RptSalesRanking>();
                foreach(int key in saleWarehouse.Keys)
                {
                    webSale.Add(saleWarehouse[key]);
                }


                for (int i = 0; i < webSale.Count; i++)
                {
                    if (string.IsNullOrEmpty(filter.ProductSaleType))
                    {
                        webSale[i].SaleProType += "网店销售：" + webSale[i].SalesQuantity + "，";
                    }
                    else
                    {
                        webSale[i].SaleProType = "网店销售";
                    }
                    string[] CategorySysNos = webSale[i].ProductCategorySysNos.Split(',');
                    string cateName = "";
                    foreach (string str in CategorySysNos)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            var tempCategory = allPdCategories.First(p => p.SysNo.ToString() == str);
                            if (!string.IsNullOrEmpty(cateName))
                            {
                                cateName += "→";
                            }
                            cateName += tempCategory.CategoryName;
                        }
                    }
                    webSale[i].ProductCategoryName = cateName;
                }

            }

            

            if (string.IsNullOrEmpty(filter.ProductSaleType) || filter.ProductSaleType == "门店销售")
            {
                try
                {
                    posSale = GetLineOutSaleRankingByWarehouse(filter);
                }
                catch
                {

                }
            }
            foreach (RptSalesRanking item in posSale)
            {
               /* List<RptSalesRanking> checkWebSale = webSale.Where(p => p.ProductSysNo == item.ProductSysNo).ToList();
                if (checkWebSale.Count > 0)
                {
                    checkWebSale[0].SalesQuantity += item.SalesQuantity;
                    checkWebSale[0].SalesAmount += item.SalesAmount;
                }
                else
                {
                    webSale.Add(item);
                }*/
                List<RptSalesRanking> checkWebSale = webSale.Where(p => p.ProductSysNo == item.ProductSysNo).ToList();
                if (checkWebSale.Count > 0)
                {
                    checkWebSale[0].SalesQuantity += item.SalesQuantity;
                    checkWebSale[0].SalesAmount += item.SalesAmount;
                    if (checkWebSale[0].SaleProType.IndexOf("门店销售") == -1)
                    {
                        checkWebSale[0].SaleProType += "门店销售：" + item.SalesQuantity;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(filter.ProductSaleType))
                    {
                        item.SaleProType += "门店销售：" + item.SalesQuantity;
                    }
                    else
                    {
                        item.SaleProType = "门店销售";
                    }

                    string[] CategorySysNos = item.ProductCategorySysNos.Split(',');
                    string cateName = "";
                    foreach (string str in CategorySysNos)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            var tempCategory = allPdCategories.First(p => p.SysNo.ToString() == str);
                            if (!string.IsNullOrEmpty(cateName))
                            {
                                cateName += "→";
                            }
                            cateName += tempCategory.CategoryName;
                        }
                    }
                    item.ProductCategoryName = cateName;

                    webSale.Add(item);
                }
            }
            webSale = webSale.OrderByDescending(p => p.SalesQuantity).ToList();
            for (int i = 0; i < webSale.Count; i++)
            {
                webSale[i].RowNumber = i + 1;
            }
            return webSale;//IReportDao.Instance.QueryWarehouseProductSales(filter);
        }
        #endregion

        #region 销量统计报表

        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>报表</returns>
        /// <remarks>2014-04-09 朱家宏 创建</remarks>
        public Pager<CBRptSales> GetSales(ParaRptSalesFilter filter)
        {
            return IReportDao.Instance.QuerySales(filter);
        }

        public Pager<CBRptSales> GetSalesByDay(ParaRptSalesFilter filter)
        {
            return IReportDao.Instance.QuerySalesByDay(filter);
        }
        #endregion

        #region 升舱销量统计报表

        /// <summary>
        /// 升舱销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>报表</returns>
        /// <remarks>2014-04-06 朱家宏 创建</remarks>
        public Pager<CBRptUpgradeSales> GetUpgradeSales(ParaRptUpgradeSalesFilter filter)
        {
            return IReportDao.Instance.QueryUpgradeSales(filter);
        }

        #endregion

        #region 快递100服务相关
        /// <summary>
        /// 获取快递100服务月统计报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public List<CBMonthExpress> GetMonthExpressListByYear(int year)
        {
            return IReportDao.Instance.GetMonthExpressListByYear(year);
        }

        /// <summary>
        /// 获取快递100服务明细
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public Pager<CBLgExpressDetail> GetLgExpressList(ParaExpressInfoFilter filter)
        {
            return Hyt.DataAccess.Report.IReportDao.Instance.GetLgExpressList(filter);
        }

        /// <summary>
        /// 快递100报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        [Obsolete]
        public IList<CBRptLgExpressInfo> QueryLgExpress(ParaRptLgExpressFilter filter)
        {
            return IReportDao.Instance.QueryLgExpress(filter);
        }

        /// <summary>
        /// 快递100报表明细查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        [Obsolete]
        public Pager<LgExpressInfo> QueryLgExpressDetail(ParaRptLgExpressFilter filter)
        {
            return IReportDao.Instance.QueryLgExpressDetail(filter);
        }
        #endregion

        #region 收款单查询数据导出
        /// <summary>
        /// 收款单查询数据导出
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="maxSheet">最大的sheet数</param>
        /// <remarks>2014-07-02 朱成果 创建</remarks>
        public byte[] ExportReceiptVoucher(ParaVoucherFilter filter, int maxSheet)
        {
            filter.Id = 1;
            filter.PageSize = 5000;//一次性从数据库获取5000条记录
            int maxExcelRow = 65000;//单个sheet最大行
            int currectRow = 1;//当前行
            ISheet sheet = null;
            IRow excelRow = null;
            ICell cell = null;
            int sheetNum = 0;//总的sheet数
            using (MemoryStream stream = new MemoryStream())
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ICellStyle cellstyle = workbook.CreateCellStyle();
                IFont fontTitle = workbook.CreateFont();
                fontTitle.FontHeightInPoints = 20;
                fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.WrapText = true;
                cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                Pager<CBFnReceiptVoucher> items = Hyt.BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(filter);
                while (items != null && items.TotalPages >= filter.Id && items.Rows.Count > 0)
                {
                    for (int row = 0; row < items.Rows.Count; row++)
                    {
                        var data = items.Rows[row];
                        if (currectRow == 1 || currectRow > maxExcelRow)//创建Sheet
                        {
                            #region 创建sheet
                            sheetNum++;
                            if (sheetNum > maxSheet)//超过允许的最大sheet数退出
                            {
                                workbook.Write(stream);
                                return stream.ToArray();
                            }
                            currectRow = 1;
                            sheet = workbook.CreateSheet();
                            sheet.SetColumnWidth(0, 15 * 256);
                            sheet.SetColumnWidth(1, 15 * 256);
                            sheet.SetColumnWidth(2, 15 * 256);
                            sheet.SetColumnWidth(3, 15 * 256);
                            sheet.SetColumnWidth(4, 15 * 256);
                            sheet.SetColumnWidth(5, 15 * 256);
                            sheet.SetColumnWidth(6, 15 * 256);
                            sheet.SetColumnWidth(7, 20 * 256);
                            sheet.SetColumnWidth(8, 20 * 256);
                            sheet.SetColumnWidth(9, 15 * 256);
                            excelRow = sheet.CreateRow(currectRow);
                            excelRow.HeightInPoints = 20;
                            cell = excelRow.CreateCell(0);
                            cell.SetCellValue("收款单编号");
                            cell.CellStyle = cellstyle;
                            cell = excelRow.CreateCell(1);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("收入类型");
                            cell = excelRow.CreateCell(2);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("单据来源");
                            cell = excelRow.CreateCell(3);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("单据来源编号");
                            cell = excelRow.CreateCell(4);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("应收金额");
                            cell = excelRow.CreateCell(5);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("实收金额");
                            cell = excelRow.CreateCell(6);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("确认人");
                            cell = excelRow.CreateCell(7);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("确认时间");
                            cell = excelRow.CreateCell(8);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("创建时间");
                            cell = excelRow.CreateCell(9);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue("状态");
                            #endregion
                        }
                        currectRow++;
                        excelRow = sheet.CreateRow(currectRow);
                        cell = excelRow.CreateCell(0);
                        cell.SetCellValue(data.SysNo);//收款单编号
                        cell.CellStyle = cellstyle;
                        cell = excelRow.CreateCell(1);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.FinanceStatus.收款单收入类型), data.IncomeType));//收入类型
                        cell = excelRow.CreateCell(2);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型), data.Source));//单据来源
                        cell = excelRow.CreateCell(3);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.SourceSysNo);//单据来源编号
                        cell = excelRow.CreateCell(4);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.IncomeAmount.ToString());//应收金额
                        cell = excelRow.CreateCell(5);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.ReceivedAmount.ToString());//实收金额
                        cell = excelRow.CreateCell(6);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.ConfirmedDate > DateTime.MinValue ? data.Confirmer : string.Empty);//确认人
                        cell = excelRow.CreateCell(7);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.ConfirmedDate == DateTime.MinValue ? string.Empty : data.ConfirmedDate.ToString("yyyy-MM-dd HH:mm"));//确认时间
                        cell = excelRow.CreateCell(8);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(data.CreatedDate == DateTime.MinValue ? string.Empty : data.CreatedDate.ToString("yyyy-MM-dd HH:mm"));//创建时间
                        cell = excelRow.CreateCell(9);
                        cell.CellStyle = cellstyle;
                        cell.SetCellValue(Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.FinanceStatus.收款单状态), data.Status)); //状态
                    }
                    filter.Id++;//下一页
                    items = Hyt.BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(filter);//下一页数据
                }
                workbook.Write(stream);
                return stream.ToArray();
            }
        }
        #endregion

        #region 区域销售统计报表
        /// <summary>
        /// 区域销售统计报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="lstResultAll">合计</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-11 余勇 创建
        /// </remarks>
        public Pager<CBRptRegionalSales> GetRegionalSales(ParaRptRegionalSales filter,
            out CBRptRegionalSales lstResultAll)
        {
            return IReportDao.Instance.QueryRegionalSales(filter, out lstResultAll);
        }

        /// <summary>
        /// 区域销售统计报表-导出excel
        /// </summary>
        /// <param name="para">ParaRptRegionalSales</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>空</returns>
        /// <remarks>2014-08-11 余勇 创建</remarks>
        public void ExportRegionalSales(ParaRptRegionalSales para, string userIp, int operatorSysno)
        {
            try
            {
                var result = new List<CBRptRegionalSalesExport>();
                para.PageSize = int.MaxValue;
                para.Id = 1;
                CBRptRegionalSales lstResultAll;
                var r = GetRegionalSales(para, out lstResultAll);
                if (r != null && r.Rows.Count > 0)
                {
                    result = r.Rows.Select(p => new CBRptRegionalSalesExport()
                    {
                        RowNumber = p.RowNumber,
                        AreaName = p.AreaName,
                        Province = p.Province,
                        City = p.City,
                        Area = p.Area,
                        CountOfHytBcd = p.CountOfHytBcd,
                        SummationOfHytBcd = p.SummationOfHytBcd,
                        CountOfHytDsf = p.CountOfHytDsf,
                        SummationOfHytDsf = p.SummationOfHytDsf
                    }).ToList();
                }
                else
                {
                    return;
                }

                para.Month = para.Month ?? DateTime.Now.Year + "-" + DateTime.Now.Month;

                var fileName = "区域销售统计报表(" + para.Month + ")";
                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportFromTemplate(result, @"\Templates\Excel\RegionalSales.xls", 3, fileName);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "区域销售统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        #endregion

        #region 加盟商报表
        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public Pager<RP_非自营销售明细> GetFranchiseesSaleDetail(ParaFranchiseesSaleDetail filter)
        {
            if (filter.WhSelected == null || !filter.WhSelected.Any())
            {
                filter.WhSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
            }
            if (!filter.WhSelected.Any())
            {
                //throw new HytException("没任何仓库的权限不能查询报表");
                return new Pager<RP_非自营销售明细>();
            }
            return IReportDao.Instance.GetFranchiseesSaleDetail(filter);
        }

        /// <summary>
        /// 导出加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="userIp">ip</param>
        /// <param name="operatorSysno">操作人</param>
        /// <returns>空</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public void ExportFranchiseesSaleDetail(ParaFranchiseesSaleDetail para, string userIp, int operatorSysno)
        {
            try
            {
                if (para.WhSelected == null || !para.WhSelected.Any())
                {
                    para.WhSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                }
                if (!para.WhSelected.Any())
                {
                    //throw new HytException("没任何仓库的权限不能查询报表");
                    return;
                }
                para.PageSize = int.MaxValue;
                para.Id = 1;
                var r = GetFranchiseesSaleDetail(para);
                if (r == null || r.Rows.Count == 0)
                {
                    return;
                }
                para.Month = para.Month ?? DateTime.Now.Year + "-" + DateTime.Now.Month;

                var fileName = "加盟商当日达对账报表(" + para.Month + ")";

                var excelExport = r.Rows.Select(p => new CBFranchiseesSaleDetail
                {
                    订单号 = p.订单号 + "",
                    出库日期 = !p.出库日期.Equals(default(DateTime)) ? p.出库日期.ToString("d") : "",
                    商城订单号 = p.商城订单号,
                    商城名称 = p.商城名称,
                    ERP编码 = p.ERP编码,
                    产品名称 = p.产品名称,
                    数量 = p.数量,
                    单价 = p.单价,
                    优惠 = p.优惠,
                    销售金额 = p.销售金额,
                    实收金额 = p.实收金额,
                    下单门店 = p.下单门店,
                    订单来源 = ((OrderStatus.销售单来源)p.订单来源).ToString(),
                    订单状态 = ((Hyt.Model.WorkflowStatus.OrderStatus.销售单状态)p.订单状态).ToString(),
                    出库仓库 = p.出库仓库,
                    加盟商ERP编号 = p.加盟商ERP编号,
                    加盟商ERP名称 = p.加盟商ERP名称,
                    收款方式 = p.收款方式,
                    配送方式 = p.配送方式,
                    出库单状态 = ((Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态)p.出库单状态).ToString(),

                    结算状态 = p.结算状态,
                    结算日期 = !p.结算日期.Equals(default(DateTime)) ? p.结算日期.ToString("d") : "",
                    升舱时间 = !p.升舱时间.Equals(default(DateTime)) ? p.升舱时间.ToString("d") : "",
                    收款单状态 = p.收款单状态,
                    收款日期 = !p.收款单确认时间.Equals(default(DateTime)) ? p.收款单确认时间.ToString("d") : ""
                }).ToList();

                //导出Excel
                Util.ExcelUtil.ExportLargeData<CBFranchiseesSaleDetail>(excelExport, null, fileName);

            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "加盟商当日达对账报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
            }
        }

        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public Pager<RP_非自营退换货明细> GetFranchiseesRmaDetail(ParaFranchiseesSaleDetail filter)
        {
            if (filter.WhSelected == null || !filter.WhSelected.Any())
            {
                filter.WhSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
            }
            if (!filter.WhSelected.Any())
            {
                //throw new HytException("没任何仓库的权限不能查询报表");
                return new Pager<RP_非自营退换货明细>();
            }
            return IReportDao.Instance.GetFranchiseesRmaDetail(filter);
        }

        /// <summary>
        /// 导出加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="userIp">ip</param>
        /// <param name="operatorSysno">操作人</param>
        /// <returns>空</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public void ExportFranchiseesRmaDetail(ParaFranchiseesSaleDetail para, string userIp, int operatorSysno)
        {
            try
            {
                if (para.WhSelected == null || !para.WhSelected.Any())
                {
                    para.WhSelected = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                }
                if (!para.WhSelected.Any())
                {
                    //throw new HytException("没任何仓库的权限不能查询报表");
                    return;
                }
                para.PageSize = int.MaxValue;
                para.Id = 1;
                var r = GetFranchiseesRmaDetail(para);
                if (r == null || r.Rows.Count == 0)
                {
                    return;
                }
                para.Month = para.Month ?? DateTime.Now.Year + "-" + DateTime.Now.Month;

                var fileName = "加盟商退换货明细对账报表(" + para.Month + ")";

                var excelExport = r.Rows.Select(p => new CBFranchiseesRmaDetail
                {
                    订单号 = p.订单号 + "",
                    订单来源 = ((OrderStatus.销售单来源)p.订单来源).ToString(),
                    申请日期 = !p.申请日期.Equals(default(DateTime)) ? p.申请日期.ToString("d") : "",
                    入库日期 = !p.入库日期.Equals(default(DateTime)) ? p.入库日期.ToString("d") : "",
                    商城订单号 = p.商城订单号,
                    商城名称 = p.商城名称,
                    ERP编码 = p.ERP编码,
                    产品名称 = p.产品名称,
                    数量 = p.数量,
                    单价 = p.单价,
                    优惠 = p.优惠,
                    退款金额 = p.退款金额,
                    实退金额 = p.实退金额,
                    下单门店 = p.下单门店,
                    源单出库仓库=p.源单出库仓库,
                    入库仓库 = p.入库仓库,
                    加盟商ERP编号=p.加盟商ERP编号,
                    加盟商ERP名称=p.加盟商ERP名称,
                    收款方式 = p.收款方式,
                    退款方式 = ((RmaStatus.退换货退款方式)p.退款方式).ToString(),
                    配送方式 = p.配送方式,
                    售后方式 = p.售后方式,
                    结算状态 = ((WarehouseStatus.退换货单状态)p.结算状态).ToString(),
                    收款单状态 = p.收款单状态,
                    收款时间 = !p.收款时间.Equals(default(DateTime)) ? p.收款时间.ToString("d") : ""
                }).ToList();

                //导出Excel
                Util.ExcelUtil.ExportLargeData<CBFranchiseesRmaDetail>(excelExport, null, fileName);

            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "加盟商当日达对账报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
            }
        }
        #endregion

        #region 办事处快递发货量统计

        /// <summary>
        /// 办事处快递发货量统计查询
        /// </summary>
        /// <param name="para">CBRptExpressLgDelivery</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,CBRptExpressLgDelivery)</returns>
        /// <remarks>2014-09-24 余勇 创建</remarks>
        public Dictionary<int, List<CBRptExpressLgDelivery>> QueryExpressLgDelivery(CBRptExpressLgDelivery para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10)
        {
            para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;
            return IReportDao.Instance.QueryExpressLgDelivery(para, warehouseSysNos, currPageIndex, pageSize);
        }

        /// <summary>
        /// 办事处快递发货量统计-导出excel
        /// </summary>
        /// <param name="para">CBRptExpressLgDelivery</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <param name="warehouseSysNos">选中的仓库</param>
        /// <returns>空</returns>
        /// <remarks>2014-09-24 余勇 创建</remarks>
        public void ExpressLgDeliveryToExcel(CBRptExpressLgDelivery para, string userIp, int operatorSysno, List<int> warehouseSysNos)
        {
            try
            {
                para.统计日期 = para.统计日期 ?? DateTime.Now.Year + "-" + DateTime.Now.Month;
                var dic = QueryExpressLgDelivery(para, warehouseSysNos, 1, int.MaxValue);

              
                //转换格式
                var list = dic.First().Value;
                if (list != null && !list.Any())
                {
                    return;
                }

                var fileName = "办事处快递发货量统计报表(" + para.统计日期 + ")";

                //导出Excel
                Util.ExcelUtil.ExportLargeData<CBRptExpressLgDelivery>(list, null, fileName);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "办事处快递发货量统计导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        #endregion

        #region 二次销售相关报表

        /// <summary>
        /// 获取二次销售报表相关数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="operatorSysno">操作人</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public Pager<CBTwoSale> GetTwoSaleList(ParaTwoSaleFilter filter,int operatorSysno)
        {
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(operatorSysno);///是否拥有所有仓库的权限

            if (filter.WarehouseSysNos == null || !filter.WarehouseSysNos.Any())
            {
                if (!hasAllWarehouse)
                    {
                        filter.WarehouseSysNos = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses.Select(x => x.SysNo).ToList();
                        if(filter.WarehouseSysNos == null || !filter.WarehouseSysNos.Any())
                        {
                            return new Pager<CBTwoSale>() { CurrentPage = filter.Id, Rows = new List<CBTwoSale>() };
                        }
                    } 
            }
            return IReportDao.Instance.GetTwoSaleList(filter);
        }


        /// <summary>
        /// 获取二次销售详情
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public Pager<CBTwoSaleDetail> GetTwoSaleDetailList(ParaTwoSaleFilter filter)
        {
            return IReportDao.Instance.GetTwoSaleDetailList(filter);
        }

        /// <summary>
        /// 导出二次销售报表对账数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="operatorSysno">操作人</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public byte[] ExporTwoSale(ParaTwoSaleFilter filter, int operatorSysno)
        {
           
                filter.Id = 1;
                filter.PageSize = int.MaxValue;
                var lst = GetTwoSaleList(filter, operatorSysno);
                using (MemoryStream stream = new MemoryStream())
                {
                    ISheet sheet = null;
                    IRow excelRow = null;
                    ICell cell = null;
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    ICellStyle cellstyle = workbook.CreateCellStyle();
                    IFont fontTitle = workbook.CreateFont();
                    fontTitle.FontHeightInPoints = 20;
                    fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    cellstyle.Alignment = HorizontalAlignment.Center;
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    cellstyle.WrapText = true;
                    cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    int currectRow = 1;
                    sheet = workbook.CreateSheet();
                    sheet.SetColumnWidth(0, 15 * 256);
                    sheet.SetColumnWidth(1, 15 * 256);
                    sheet.SetColumnWidth(2, 15 * 256);
                    sheet.SetColumnWidth(3, 15 * 256);
                    sheet.SetColumnWidth(4, 15 * 256);
                    sheet.SetColumnWidth(5, 15 * 256);
                    sheet.SetColumnWidth(6, 15 * 256);
                    sheet.SetColumnWidth(7, 20 * 256);
                    sheet.SetColumnWidth(8, 20 * 256);
                    sheet.SetColumnWidth(9, 15 * 256);
                    excelRow = sheet.CreateRow(currectRow);
                    excelRow.HeightInPoints = 20;
                    cell = excelRow.CreateCell(0);
                    cell.SetCellValue("下单时间");
                    cell.CellStyle = cellstyle;
                    cell = excelRow.CreateCell(1);
                    cell.CellStyle = cellstyle;
                    cell.SetCellValue("仓库");
                    cell = excelRow.CreateCell(2);
                    cell.CellStyle = cellstyle;
                    cell.SetCellValue("业务员");
                    cell = excelRow.CreateCell(3);
                    cell.CellStyle = cellstyle;
                    cell.SetCellValue("单量");
                    cell = excelRow.CreateCell(4);
                    cell.CellStyle = cellstyle;
                    cell.SetCellValue("金额");

                    if (lst != null)
                    {
                        foreach (var item in lst.Rows)
                        {
                            currectRow++;
                            excelRow = sheet.CreateRow(currectRow);
                            cell = excelRow.CreateCell(0);
                            cell.SetCellValue(item.CreateDate);//下单时间
                            cell.CellStyle = cellstyle;
                            cell = excelRow.CreateCell(1);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue(item.WarehouseName);//仓库
                            cell = excelRow.CreateCell(2);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue(item.UserName);//业务员
                            cell = excelRow.CreateCell(3);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue(item.OrderCount);//单量
                            cell = excelRow.CreateCell(4);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue(item.OrderCash.ToString());//金额
                        }
                    }

                    workbook.Write(stream);
                    return stream.ToArray();
                }

        }



        #endregion

        #region 会员涨势统计报表
        /// <summary>
        /// 会员涨势信息
        /// </summary>
        /// <param name="filter">会员涨势信息</param>
        /// <returns>返回会员涨势信息</returns>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        public Pager<CBRptDealerSales> GetDealerSalesList(ParaRptDealerSalesFilter filter)
        {
            return IReportDao.Instance.GetDealerSalesList(filter);
        }

        /// <summary>
        /// 会员涨势信息导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportDealerSales(ParaRptDealerSalesFilter filter, List<int> dealerSysNos, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<CBOutputRptDealerSales> exportDealerSales = ReportBO.Instance.GetExportDealerSalesList(filter, dealerSysNos);

                var fileName = string.Format("会员涨势统计报表({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 经销商名称
                 * 会员数
                 * 营业额
                 */
                string strDate = "";
                //开始日期
                if (!string.IsNullOrEmpty(filter.BeginDate))
                {
                    strDate = filter.BeginDate + "~";
                }
                else
                {
                    strDate = "起始日期" + "~";
                }
                //结束日期
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                     strDate += filter.EndDate;
                }
                else
                {
                    strDate += "至今";
                }
                if (strDate != "")
                {
                    strDate = "(" + strDate + ")";
                }
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputRptDealerSales>(exportDealerSales,
                    new List<string> { "经销商名称", "会员数" + strDate, "营业额" + strDate },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "会员涨势统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "会员涨势统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputRptDealerSales> GetExportDealerSalesList(ParaRptDealerSalesFilter filter, List<int> sysNos)
        {
            return IReportDao.Instance.GetExportDealerSalesList(filter, sysNos);
        }

        #endregion

        /// <summary>
        /// 经销商总销售量排名
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>
        /// 经销商总销量排序列表
        /// </returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public List<Model.Common.ReportMod> DistributorSalesOrderReport(DateTime? startTime, DateTime? endTime, string orderBy)
        {
            return IReportDao.Instance.DistributorSalesOrderReport(startTime, endTime, orderBy); 
        }

        /// <summary>
        /// 经销商商品销售排行
        /// </summary>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// 2016-04-09 杨云奕 添加
        /// </returns>
        public List<Model.Common.ReportMod> DistributorSalesProductReport(int DsSysNo, DateTime? startTime, DateTime? endTime, string orderBy)
        {
            return IReportDao.Instance.DistributorSalesProductReport(DsSysNo,startTime, endTime, orderBy); 
        }

        /// 单品经销商的销售排名
        /// </summary>
        /// <param name="ProSysNo">商品编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public List<Model.Common.ReportMod> SalesProductDistributorReport(int ProSysNo, DateTime? startTime, DateTime? endTime, string orderBy)
        {
            return IReportDao.Instance.SalesProductDistributorReport(ProSysNo, startTime, endTime, orderBy); 
        }

        public List<Model.Manual.AnnualMod> AnnualSalesStatistics(int year, int DsSysNo)
        {
            return IReportDao.Instance.AnnualSalesStatistics(year, DsSysNo);
        }

        /// <summary>
        /// 获取实体店统计
        /// </summary>
        /// <param name="defaultWareSysNo">默认仓库编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<Model.Manual.EntityStatisticMod> SearchEntityShopStatistics(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime)
        {
            return IReportDao.Instance.SearchEntityShopStatistics(defaultWareSysNo, startTime, endTime);
        }
        /// <summary>
        /// 客户购买量统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<Model.Manual.EntityStatisticMod> SearchCustomerPurchasesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            
            return IReportDao.Instance.SearchCustomerPurchasesStatistics(startTime, endTime,DsSysNo);
        }

        /// <summary>
        /// 分销商购买量统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<Model.Manual.EntityStatisticMod> SearchDistributorsStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            return IReportDao.Instance.SearchDistributorsStatistics(startTime, endTime, DsSysNo);
        }

        /// <summary>
        /// 网上购买量统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<Model.Manual.EntityStatisticMod> SearchOnlineSalesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            return IReportDao.Instance.SearchOnlineSalesStatistics(startTime, endTime, DsSysNo);
        }


        /// <summary>
        /// 保税商品销售统计表
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<Model.Manual.EntityStatisticMod> SearchBondedSalesStatistics(DateTime? startTime, DateTime? endTime)
        {
            return IReportDao.Instance.SearchBondedSalesStatistics(startTime, endTime);
        }

        public List<Model.Manual.EntityStatisticMod> PrecisionMarketReport(string proCateSysNos, int? ShopSysNo, DateTime? startTime, DateTime? endTime)
        {
            return IReportDao.Instance.PrecisionMarketReport(proCateSysNos, ShopSysNo, startTime, endTime);

        }

        #region 返利记录统计报表
        /// <summary>
        /// 会员涨势信息
        /// </summary>
        /// <param name="filter">会员涨势信息</param>
        /// <returns>返回会员涨势信息</returns>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        public Pager<CBRptRebatesRecord> GetRebatesRecordList(ParaRptRebatesRecordFilter filter)
        {
            return IReportDao.Instance.GetRebatesRecordList(filter);
        }

        /// <summary>
        /// 会员涨势信息导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportRebatesRecord(ParaRptRebatesRecordFilter filter, List<int> sysNos, string userIp, int operatorSysno)
        {
            try
            {
                // 查询
                List<CBOutputRptRebatesRecord> exportRebatesRecord = ReportBO.Instance.GetExportRebatesRecordList(filter, sysNos);

                var fileName = string.Format("会员返利统计报表({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 经销商名称
                 * 会员数
                 * 营业额
                 */
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputRptRebatesRecord>(exportRebatesRecord,
                    new List<string> { "订单ID", "订单编号","订单日期", "购买人ID", "购买人账号", "返利人ID", "返利人账号", "返利人名称", "订单利润", "返利金额", "服务费", "实际返利" 
                    , "订单商品金额", "订单运费", "订单总额", "返利状态", "返利类型", "分销等级ID", "返利人分销等级"},
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "会员返利统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "会员返利统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出返利记录信息列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public List<CBOutputRptRebatesRecord> GetExportRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos)
        {
            return IReportDao.Instance.GetExportRebatesRecordList(filter, sysNos);
        }


        /// <summary>
        /// 分销商返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返利记录信息</returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public Pager<CBRptRebatesRecord> GetDealerRebatesRecordList(ParaRptRebatesRecordFilter filter)
        {
            return IReportDao.Instance.GetDealerRebatesRecordList(filter);
        }

        /// <summary>
        /// 分销商返利记录信息导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2015-12-29 王耀发 创建</remarks>
        public void ExportDealerRebatesRecord(ParaRptRebatesRecordFilter filter, List<int> sysNos, string userIp, int operatorSysno)
        {
            try
            {
                // 查询
                List<CBOutputRptRebatesRecord> exportRebatesRecord = ReportBO.Instance.GetExportDealerRebatesRecordList(filter, sysNos);

                var fileName = string.Format("分销商返利统计报表({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 经销商
                 * 推荐人
                 * 消费者
                 */
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputRptRebatesRecord>(exportRebatesRecord,
                    new List<string> { "订单ID", "订单编号","订单日期", "购买人ID", "购买人账号", "返利人ID", "返利人账号", "返利人名称", "订单利润", "返利金额", "服务费", "实际返利" 
                    , "订单商品金额", "订单运费", "订单总额", "返利状态", "返利类型", "分销等级ID", "返利人分销等级"},
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "分销商返利统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商返利统计报表导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出分销商返利记录
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputRptRebatesRecord> GetExportDealerRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos)
        {
            return IReportDao.Instance.GetExportDealerRebatesRecordList(filter, sysNos);
        }
        #endregion

        /// <summary>
        /// 同步销售单
        /// </summary>
        /// <returns>王耀发 2016-6-4 创建</returns>
        public int ProCreateSaleDetail()
        {
            return IReportDao.Instance.ProCreateSaleDetail();
        }


        /// <summary>
        /// 同步退换货单
        /// </summary>
        /// <returns>吴琨 2017-9-27 创建</returns>
        public int SynchronousRma()
        {
            return IReportDao.Instance.SynchronousRma();
        }
    }

}
