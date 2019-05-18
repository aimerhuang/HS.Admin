using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using Hyt.Util;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 货物档案数据列表
    /// </summary>
    /// <remarks>
    /// 2015-5-17 杨云奕 添加
    /// </remarks>
    public class DsWhGoodsManagementBo : BOBase<DsWhGoodsManagementBo>
    {
        /// <summary>
        /// 添加主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        /// <returns></returns>
        public int InsertMod(DsWhGoodsManagement mod)
        {
            return IDsWhGoodsManagementDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 更新主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        public void UpdateMod(DsWhGoodsManagement mod)
        {
            IDsWhGoodsManagementDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 删除主表数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public  void DeleteMod(int SysNo)
        {
            IDsWhGoodsManagementDao.Instance.DeleteMod(SysNo);
        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public DsWhGoodsManagement GetModBySysNo(int SysNo)
        {
            return IDsWhGoodsManagementDao.Instance.GetModBySysNo(SysNo);
        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public CBDsWhGoodsManagement GetExtendsModBySysNo(int SysNo)
        {
            return IDsWhGoodsManagementDao.Instance.GetExtendsModBySysNo(SysNo);
        }
        /// <summary>
        /// 添加货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        /// <returns></returns>
        public  int InsertModList(DsWhGoodsManagementList mod)
        {
            return IDsWhGoodsManagementDao.Instance.InsertModList(mod);
        }
        /// <summary>
        /// 更新货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        public void UpdateModList(DsWhGoodsManagementList mod)
        {
            IDsWhGoodsManagementDao.Instance.UpdateModList(mod);
        }
        /// <summary>
        /// 删除货物档案明细
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public void DeleteModList(int SysNo)
        {
            IDsWhGoodsManagementDao.Instance.DeleteModList(SysNo);
        }
        /// <summary>
        /// 获取某一数据实体
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public DsWhGoodsManagementList GetModListBySysNo(int SysNo)
        {
            return IDsWhGoodsManagementDao.Instance.GetModListBySysNo(SysNo);
        }
        /// <summary>
        /// 通过父编号获取商品明细集合
        /// </summary>
        /// <param name="PSysNo">父id编号</param>
        /// <returns></returns>
        public List<DsWhGoodsManagementList> GetModListByPSysNo(int PSysNo)
        {
            return IDsWhGoodsManagementDao.Instance.GetModListByPSysNo(PSysNo);
        }

        /*
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicGoodsColsMapping = new Dictionary<string, string>
            {
                {"SysNo", "单号"},
                {"CustomerCode", "会员号"},
                {"Dis", "说明"},
                {"SendUser", "发件人姓名"},
                {"SendTelephone", "发件人电话"},
                {"SendAddress", "发件人地址"},
                {"SendMall", "发件人邮编"},
                {"Receipter","收件人姓名"},
                {"IDCard","收件人身份证号"},
                {"ReceiptTele","收件人电话"},
                {"City","城市"},
                {"Provinces","省份"},
                {"ReceiptMall","收件人邮编"},
                {"ReceiptAddress","收件人地址"},
                {"WeightValue","重量"},
                {"Valuation","价值"},
                {"ServiceType","服务类型"},
                {"GoodsName","货物名称"},
                {"GoodsNumber","件数"},
                {"FrantIDCard","身份证正面"},
                {"BackIDCard","身份证反面"}

            };
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicItemColsMapping = new Dictionary<string, string>
            {
                {"SysNo", "货号"},
                {"AssNumber","关联单号"},
                {"ProductName", "物品名称"},
                {"Quantiyt", "物品数量"},
                {"GoodsUnit", "单位"},
                {"GoodsPrice", "物品单价"},
                {"GoodsWeight", "物品总净重(KG)"},
                {"GoodsPostTax","行邮税号"},
                {"GoodsSpec","规格"},
                {"BrandName","品牌"},
                {"GoodsCusCode","清关行货号"}
            };
        /// <summary>
        /// 货物商品导入
        /// </summary>
        /// <param name="stream">数据源</param>
        /// <param name="SysNo">客户编号</param>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="CusCode">客户编码</param>
        /// <returns></returns>
        public Model.Result ImportExcel(System.IO.Stream stream, int SysNo, int DsSysNo, string CusCode)
        {
            DataTable dt = null;
            DataTable dt_Item = new DataTable();
            var cols = DicGoodsColsMapping.Select(p => p.Value).ToArray();
            var cols_Item = DicItemColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<DsWhProduct>();
            try
            {
                HSSFWorkbook hssfwb = null;
                dt = ExcelUtil.ImportExcel(stream,ref hssfwb, cols);

                //dt_Item = ExcelUtil.ImportExcel(stream, 1, hssfwb, cols_Item);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            List<DsWhGoodsManagement> managementList = new List<DsWhGoodsManagement>();
            List<DsWhGoodsManagementList> managementItemList = new List<DsWhGoodsManagementList>();
            string batchNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            int indx = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal totalWeight = 0;
                decimal.TryParse(row[DicGoodsColsMapping["WeightValue"]].ToString().Trim(), out totalWeight);
                decimal Valuation=0;
                decimal.TryParse(row[DicGoodsColsMapping["Valuation"]].ToString().Trim(), out Valuation);

                DsWhGoodsManagement mod = new DsWhGoodsManagement();
                mod.BatchNumber = batchNumber;
                mod.CustomerCode = string.IsNullOrEmpty((row[DicGoodsColsMapping["CustomerCode"]].ToString().Trim())) ? 
                    CusCode : (row[DicGoodsColsMapping["CustomerCode"]].ToString().Trim());
                mod.StatusCode = "0";
                mod.SysNo = indx;
                //mod.AssNumber = (row[DicGoodsColsMapping["AssNumber"]].ToString().Trim());
                mod.Dis = (row[DicGoodsColsMapping["Dis"]].ToString().Trim());
                mod.SendUser = (row[DicGoodsColsMapping["SendUser"]].ToString().Trim());
                mod.SendTelephone = (row[DicGoodsColsMapping["SendTelephone"]].ToString().Trim());
                mod.SendAddress = (row[DicGoodsColsMapping["SendAddress"]].ToString().Trim());
                mod.Receipter = (row[DicGoodsColsMapping["Receipter"]].ToString().Trim());
                mod.IDCard = (row[DicGoodsColsMapping["IDCard"]].ToString().Trim());
                mod.ReceiptTele = (row[DicGoodsColsMapping["ReceiptTele"]].ToString().Trim());
                //mod.ReceiptCity = (row[DicGoodsColsMapping["Provinces"]].ToString().Trim()) + "-" + (row[DicGoodsColsMapping["City"]].ToString().Trim());
                mod.ReceiptMall = (row[DicGoodsColsMapping["ReceiptMall"]].ToString().Trim());
                mod.ReceiptAddress = (row[DicGoodsColsMapping["ReceiptAddress"]].ToString().Trim());
                mod.Valuation = Valuation;
                mod.WeightValue = totalWeight;
                //mod.Currency = row[DicGoodsColsMapping["Currency"]].ToString().Trim();
                mod.ServiceType = row[DicGoodsColsMapping["ServiceType"]].ToString().Trim();
                mod.CreateTime = DateTime.Now;
                managementList.Add(mod);

                if (string.IsNullOrEmpty(mod.SendUser))
                {
                    return new Model.Result
                    {
                        Message = string.Format("发件人不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(mod.SendTelephone))
                {
                    return new Model.Result
                    {
                        Message = string.Format("发件人电话不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(mod.Receipter))
                {
                    return new Model.Result
                    {
                        Message = string.Format("收件人不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(mod.ReceiptTele))
                {
                    return new Model.Result
                    {
                        Message = string.Format("收件人电话不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(mod.ReceiptAddress))
                {
                    return new Model.Result
                    {
                        Message = string.Format("收件人地址不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(row[DicGoodsColsMapping["GoodsName"]].ToString().Trim()))
                {
                    return new Model.Result
                    {
                        Message = string.Format("商品不能为空!"),
                        Status = false
                    };
                }
                if (string.IsNullOrEmpty(mod.ServiceType))
                {
                    return new Model.Result
                    {
                        Message = string.Format("服务类型不能为空!"),
                        Status = false
                    };
                }
                if(string.IsNullOrEmpty(mod.IDCard))
                {
                    return new Model.Result
                    {
                        Message = string.Format("身份证不能为空!"),
                        Status = false
                    };
                }
                string[] strItems = row[DicGoodsColsMapping["GoodsName"]].ToString().Trim().Replace("，",",").Split(',');
                foreach (string strRow in strItems)
                {
                    string proName = strRow.Substring(0, strRow.LastIndexOf("*"));
                    int quantity = 0;
                    int.TryParse(strRow.Replace(proName + "*","").Trim(), out quantity);
                    if (quantity > 0)
                    {
                        DsWhGoodsManagementList item = new DsWhGoodsManagementList();
                        item.PSysNo = indx;
                        item.GoodsName = proName;
                        item.Quantiyt = quantity;

                        managementItemList.Add(item);
                    }
                }
                indx++;

            }

           

            using (var tran = new TransactionScope())
            { 
                foreach(var mod in managementList)
                {
                    
                    var sysNo = mod.SysNo;
                    mod.SysNo = 0;
                    List<DsWhGoodsManagementList> itemList = managementItemList.FindAll(p => p.PSysNo == sysNo);
                    mod.SysNo = InsertMod(mod);
                    int totalPageNum=0;
                    string PageName = "";
                    decimal weight = 0;
                    foreach(var item in itemList)
                    {
                        item.PSysNo = mod.SysNo;
                        InsertModList(item);
                        totalPageNum+=item.Quantiyt;
                        if (!string.IsNullOrEmpty(PageName.Trim()))
                        {
                            PageName += ",";
                        }
                        PageName += item.GoodsName + "*" + item.Quantiyt+"  ";
                        //weight += item.GoodsWeight;
                    }
                    mod.ReceiptPageNum = totalPageNum.ToString();
                    mod.ReceiptPageName = PageName;
                   
                    UpdateMod(mod);
                }
                tran.Complete();
            }

            return new Model.Result<string>
            {
                Message = "操作成功，通过商品编码检查导入的商品新增更新与否-" + batchNumber,
                Data = batchNumber,
                Status = true
            };
        }

        */

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicGoodsColsMapping = new Dictionary<string, string>
            {
                {"SysNo", "货号"},
                {"AssNumber", "关联单号"},
                {"Dis", "备注"},
                {"SendUser", "寄件人"},
                {"SendTelephone", "寄件人电话"},
                {"SendAddress", "寄件人地址"},
                {"Receipter","收件人"},
                {"IDCard","收件人身份证号"},
                {"ReceiptTele","收件人电话"},
                {"City","城市"},
                {"Provinces","省份"},
                {"ReceiptMall","邮编"},
                {"ReceiptAddress","收件人地址"},
                {"WeightValue","本包裹总毛重量(KG)"},
                {"Currency","币别"},
                {"ServiceType","服务类型"},
                {"GoodsList","物品"}
            };
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicItemColsMapping = new Dictionary<string, string>
            {
                {"SysNo", "货号"},
                {"AssNumber","关联单号"},
                {"ProductName", "物品名称"},
                {"Quantiyt", "物品数量"},
                {"GoodsUnit", "单位"},
                {"GoodsPrice", "物品单价"},
                {"GoodsWeight", "物品总净重(KG)"},
                {"GoodsPostTax","行邮税号"},
                {"GoodsSpec","规格"},
                {"BrandName","品牌"},
                {"GoodsCusCode","清关行货号"}
            };

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicLogicColsMapping = new Dictionary<string, string>
            {
                {"LogicCompany", "快递公司编号"},
                {"LogicNumber","快递单号"},
                {"GoodsNumber", "运单号"}
            };
        /// <summary>
        /// 货物商品导入
        /// </summary>
        /// <param name="stream">数据源</param>
        /// <param name="SysNo">客户编号</param>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="CusCode">客户编码</param>
        /// <returns></returns>
        public Model.Result ImportLogicExcel(System.IO.Stream stream)
        {
            DataTable dt = null;
            //DataTable dt_Item = null;
            var cols = DicLogicColsMapping.Select(p => p.Value).ToArray();

            var existlst = new List<DsWhProduct>();
            try
            {
                HSSFWorkbook hssfwb = null;
                dt = ExcelUtil.ImportExcel(stream, ref hssfwb, cols);

                // dt_Item = ExcelUtil.ImportExcel(stream, 1, hssfwb, cols_Item);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            List<string> CourierNumbers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                CourierNumbers.Add((row[DicLogicColsMapping["GoodsNumber"]].ToString().Trim()));
            }
            List<DsWhGoodsManagement> goodsList = DsWhGoodsManagementBo.Instance.GetDsWhGoodsManagementByCourierNumbers(CourierNumbers);

            string msg = "";
            try
            {

                using (var tran = new TransactionScope())
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        DsWhGoodsManagement mod = goodsList.Find(p => p.CourierNumber == (row[DicLogicColsMapping["GoodsNumber"]].ToString().Trim()));
                        if (mod != null)
                        {
                            mod.ServiceType = row[DicLogicColsMapping["LogicCompany"]].ToString().Trim();
                            mod.AssNumber = row[DicLogicColsMapping["LogicNumber"]].ToString().Trim();
                            DsWhGoodsManagementBo.Instance.UpdateMod(mod);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(msg))
                            {
                                msg += "<br/>";
                            }
                            msg += "运单：" + row[DicLogicColsMapping["GoodsNumber"]].ToString().Trim() +
                                    "绑定国内物流：" + row[DicLogicColsMapping["LogicCompany"]].ToString().Trim() + "(" + row[DicLogicColsMapping["LogicNumber"]].ToString().Trim() + ")" +
                                    "失败，找不到当前运单号。";
                        }
                    }
                    tran.Complete();
                }
                msg = "成功-" + msg;
            }
            catch (Exception e)
            {
                msg = "失败-" + e.Message;
            }

            return new Model.Result<string>
            {
                Message = msg,
                Data = "",
                Status = true
            };
        }
        /// <summary>
        /// 货物商品导入
        /// </summary>
        /// <param name="stream">数据源</param>
        /// <param name="SysNo">客户编号</param>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="CusCode">客户编码</param>
        /// <returns></returns>
        public Model.Result ImportExcel(System.IO.Stream stream, int SysNo, int DsSysNo, string CusCode)
        {
            DataTable dt = null;
            //DataTable dt_Item = null;
            var cols = DicGoodsColsMapping.Select(p => p.Value).ToArray();
            var cols_Item = DicItemColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<DsWhProduct>();
            try
            {
                HSSFWorkbook hssfwb = null;
                dt = ExcelUtil.ImportExcel(stream, ref hssfwb, cols);

               // dt_Item = ExcelUtil.ImportExcel(stream, 1, hssfwb, cols_Item);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            List<DsWhGoodsManagement> managementList = new List<DsWhGoodsManagement>();
            List<DsWhGoodsManagementList> managementItemList = new List<DsWhGoodsManagementList>();
            string batchNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            int indx = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row[DicGoodsColsMapping["GoodsList"]].ToString().Trim()))
                {
                    continue;
                }
                indx++;
                decimal totalWeight = 0;
                decimal.TryParse(row[DicGoodsColsMapping["WeightValue"]].ToString().Trim(), out totalWeight);
                DsWhGoodsManagement mod = new DsWhGoodsManagement();
                mod.BatchNumber = batchNumber;
                mod.CustomerCode = CusCode;
                mod.StatusCode = "0";
                mod.SysNo = indx; //Convert.ToInt32(row[DicGoodsColsMapping["SysNo"]].ToString().Trim());
                mod.CourierNumber = (row[DicGoodsColsMapping["AssNumber"]].ToString().Trim());
                mod.Dis = (row[DicGoodsColsMapping["Dis"]].ToString().Trim());
                mod.SendUser = (row[DicGoodsColsMapping["SendUser"]].ToString().Trim());
                mod.SendTelephone = (row[DicGoodsColsMapping["SendTelephone"]].ToString().Trim());
                mod.SendAddress = (row[DicGoodsColsMapping["SendAddress"]].ToString().Trim());
                mod.Receipter = (row[DicGoodsColsMapping["Receipter"]].ToString().Trim());
                mod.IDCard = (row[DicGoodsColsMapping["IDCard"]].ToString().Trim());
                mod.ReceiptTele = (row[DicGoodsColsMapping["ReceiptTele"]].ToString().Trim());
                mod.ReceiptCity = (row[DicGoodsColsMapping["Provinces"]].ToString().Trim()) + "-" + (row[DicGoodsColsMapping["City"]].ToString().Trim());
                mod.ReceiptMall = (row[DicGoodsColsMapping["ReceiptMall"]].ToString().Trim());
                mod.ReceiptAddress = (row[DicGoodsColsMapping["ReceiptAddress"]].ToString().Trim());
                mod.WeightValue = totalWeight;
                mod.Currency = row[DicGoodsColsMapping["Currency"]].ToString().Trim();
                mod.ServiceType = row[DicGoodsColsMapping["ServiceType"]].ToString().Trim();
                mod.CreateTime = DateTime.Now;
                managementList.Add(mod);

                string goodsListText = row[DicGoodsColsMapping["GoodsList"]].ToString().Replace("，",",").Trim();
                string[] rowsList = goodsListText.Split(',');
                foreach (string subTRow in rowsList)
                {
                    string[] rowInfo = subTRow.Split('*');

                    int quantity = 0;
                    decimal price = 0;
                    decimal weight = 0;
                    int.TryParse(rowInfo[1].Trim(), out quantity);
                    //decimal.TryParse(row[DicItemColsMapping["GoodsWeight"]].ToString().Trim(), out weight);

                    DsWhGoodsManagementList item = new DsWhGoodsManagementList();
                    item.PSysNo = indx;
                    item.GoodsName = rowInfo[0];
                    item.Quantiyt = quantity;
                    item.GoodsPrice = price;
                    item.GoodsUnit = "";
                    item.GoodsWeight = weight;
                    //item.GoodsPostTax = row[DicItemColsMapping["GoodsPostTax"]].ToString().Trim();
                    item.GoodsSpec = "";//row[DicItemColsMapping["GoodsSpec"]].ToString().Trim()
                    item.BrandName = "";//row[DicItemColsMapping["BrandName"]].ToString().Trim()
                    //item.GoodsCusCode = row[DicItemColsMapping["GoodsCusCode"]].ToString().Trim();
                    managementItemList.Add(item);
                }
                /*string goodsListText = row[DicGoodsColsMapping["GoodsList"]].ToString().Trim();
                string[] rowsList = goodsListText.Replace("||||||", "░").Split('░');
                foreach(string subTRow in rowsList)
                {
                    string[] rowInfo = subTRow.Replace("::::::", "∷").Split('∷');

                    int quantity = 0;
                    decimal price = 0;
                    decimal weight = 0;
                    int.TryParse(rowInfo[5].Trim(), out quantity);
                    decimal.TryParse(rowInfo[3], out price);
                    //decimal.TryParse(row[DicItemColsMapping["GoodsWeight"]].ToString().Trim(), out weight);

                    DsWhGoodsManagementList item = new DsWhGoodsManagementList();
                    item.PSysNo = indx;
                    item.GoodsName = rowInfo[0];
                    item.Quantiyt = quantity;
                    item.GoodsPrice = price;
                    item.GoodsUnit = rowInfo[4];
                    item.GoodsWeight = weight;
                    //item.GoodsPostTax = row[DicItemColsMapping["GoodsPostTax"]].ToString().Trim();
                    item.GoodsSpec = rowInfo[2];//row[DicItemColsMapping["GoodsSpec"]].ToString().Trim()
                    item.BrandName = rowInfo[1];//row[DicItemColsMapping["BrandName"]].ToString().Trim()
                    //item.GoodsCusCode = row[DicItemColsMapping["GoodsCusCode"]].ToString().Trim();
                    managementItemList.Add(item);
                }*/
                

            }

            //foreach (DataRow row in dt_Item.Rows)
            //{
            //    int quantity = 0;
            //    decimal price = 0;
            //    decimal weight = 0;
            //    int.TryParse(row[DicItemColsMapping["Quantiyt"]].ToString().Trim(), out quantity);
            //    decimal.TryParse(row[DicItemColsMapping["GoodsPrice"]].ToString().Trim(), out price);
            //    decimal.TryParse(row[DicItemColsMapping["GoodsWeight"]].ToString().Trim(), out weight);

            //    DsWhGoodsManagementList item = new DsWhGoodsManagementList();
            //    item.PSysNo = Convert.ToInt32(row[DicItemColsMapping["SysNo"]].ToString().Trim());
            //    item.GoodsName = row[DicItemColsMapping["ProductName"]].ToString().Trim();
            //    item.Quantiyt = quantity;
            //    item.GoodsUnit = row[DicItemColsMapping["GoodsUnit"]].ToString().Trim();
            //    item.GoodsPrice = price;
            //    item.GoodsUnit = row[DicItemColsMapping["GoodsUnit"]].ToString().Trim();
            //    item.GoodsWeight = weight;
            //    item.GoodsPostTax = row[DicItemColsMapping["GoodsPostTax"]].ToString().Trim();
            //    item.GoodsSpec = row[DicItemColsMapping["GoodsSpec"]].ToString().Trim();
            //    item.BrandName = row[DicItemColsMapping["BrandName"]].ToString().Trim();
            //    item.GoodsCusCode = row[DicItemColsMapping["GoodsCusCode"]].ToString().Trim();
            //    managementItemList.Add(item);
            //}

            using (var tran = new TransactionScope())
            {
                foreach (var mod in managementList)
                {
                    
                    var sysNo = mod.SysNo;
                    mod.SysNo = 0;
                    mod.PayStatus = "0";
                    List<DsWhGoodsManagementList> itemList = managementItemList.FindAll(p => p.PSysNo == sysNo);
                    mod.SysNo = InsertMod(mod);
                    int totalPageNum = 0;
                    string PageName = "";
                    decimal weight = 0;
                    foreach (var item in itemList)
                    {
                        item.PSysNo = mod.SysNo;
                        InsertModList(item);
                        totalPageNum += item.Quantiyt;
                        PageName += item.GoodsName + "*" + item.Quantiyt + "  ";
                        weight += item.GoodsWeight;
                    }
                    mod.ReceiptPageNum = totalPageNum.ToString();
                    mod.ReceiptPageName = PageName;
                   // mod.WeightValue = weight;
                    UpdateMod(mod);
                }
                tran.Complete();
            }

            return new Model.Result<string>
            {
                Message = "操作成功，通过商品编码检查导入的商品新增更新与否-" + batchNumber,
                Data = batchNumber,
                Status = true
            };
        }
        public List<DsWhGoodsManagement> GetModListByBatchNumber(string batchNumber, bool IsBindAllDealer, bool IsBindDealer,
            bool IsCustomer, int DsSysNo, string CusCode)
        {
            return IDsWhGoodsManagementDao.Instance.GetModListByBatchNumber(batchNumber, IsBindAllDealer, IsBindDealer,
             IsCustomer, DsSysNo, CusCode);
        }

        public void GoodsManagePager(ref Model.Pager<CBDsWhGoodsManagement> pageCusList,
            bool IsBindAllDealer, bool IsBindDealer, bool IsCustomer,
            int DsSysNo, string CusCode, string OrderByKey, string OrderbyType)
        {
            IDsWhGoodsManagementDao.Instance.GoodsManagePager(ref  pageCusList,
             IsBindAllDealer, IsBindDealer, IsCustomer,
             DsSysNo, CusCode, OrderByKey, OrderbyType);
        }

        public DsWhGoodsManagement GetModListByCourierNumber(string CourierNumber)
        {
             return IDsWhGoodsManagementDao.Instance.GetModListByCourierNumber(CourierNumber);
        }

        public List<DsWhGoodsManagement> GetDsWhGoodsManagementByCourierNumbers(List<string> packNumList)
        {
            return IDsWhGoodsManagementDao.Instance.GetDsWhGoodsManagementByCourierNumbers(packNumList);
        }

        public void UpdateModByWaybill(string MawbNumber,string SatausCode, List<string> waybillnumberList)
        {
             IDsWhGoodsManagementDao.Instance.UpdateModByWaybill(MawbNumber,SatausCode, waybillnumberList);
        }

        public List<CBDsWhGoodsManagement> GetAllGoodsManageBySearch(CBDsWhGoodsManagement cbGoodsMan)
        {
            return IDsWhGoodsManagementDao.Instance.GetAllGoodsManageBySearch(cbGoodsMan);
        }

        public List<CBDsWhGoodsManagement> GetDsWhGoodsManagementByTotalWaybillNum(string WayWillNum)
        {
            return IDsWhGoodsManagementDao.Instance.GetDsWhGoodsManagementByTotalWaybillNum(WayWillNum);
        }

        public void OrderManagerGroupPager(ref Model.Pager<WhGoodsManagementGroup> pageCusList)
        {
             IDsWhGoodsManagementDao.Instance.OrderManagerGroupPager(pageCusList);
        }

        public List<CBDsWhGoodsManagement> GetAllGoodsManageByCreateTime(string crateTime)
        {
            return IDsWhGoodsManagementDao.Instance.GetAllGoodsManageByCreateTime(crateTime);
        }


        public List<CBDsWhGoodsManagement> GetExtendsModBySysNos(string gmSysNos)
        {
            return IDsWhGoodsManagementDao.Instance.GetExtendsModBySysNos(gmSysNos);
        }

        public List<CBDsWhGoodsManagement> GetGoodsManageBySearchText(bool cb_Select,string ipt_Batch, string ipt_StartOrder, string ipt_EndOrder, string ipt_OutStockDate, string sel_Stautus)
        {
            return IDsWhGoodsManagementDao.Instance.GetGoodsManageBySearchText(cb_Select,ipt_Batch, ipt_StartOrder, ipt_EndOrder, ipt_OutStockDate, sel_Stautus);
        }

        public DsWhGMReport GetDsWhReportData()
        {
            return IDsWhGoodsManagementDao.Instance.GetDsWhReportData();
        }
    }
}
