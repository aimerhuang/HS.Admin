using Extra.Erp.Model;
using Extra.Erp.Model.BaseData;
using Hyt.DataAccess.Product;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Control
{
  
    public class ERPProductControl
    {
        //public string actionPath = "/base/itemList";
        /// <summary>
        /// 查询商品信息
        /// </summary>
        /// <param name="FName">商品名称，为空是查询全部</param>
        /// <returns></returns>
        public EasResult<List<EasProduct>> SearchStockProductItemList(string FName = "")
        {
            //APP_Key:\"" + Config.APP_Key + "\",APP_scode:\"" + Config.APP_scode + "\",
            string actionPath = "/base/itemList";
            string postDataPath = Config.root + actionPath;
            string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, "{ \"FName\":\"" + FName + "\"}");
            EasResult<List<EasProduct>> result = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult<List<EasProduct>>>(posData);
            return result;
        }

        /// <summary>
        /// 添加商品档案
        /// </summary>
        /// <param name="ProductSysNo">商品编码</param>
        /// <param name="StockSysNo">库存编码</param>
        /// <param name="StockSysName">仓库名称</param>
        /// <param name="actionType">操作编码 0-新增;1-修改;2-删除</param>
        /// <returns></returns>
        public Extra.Erp.Model.Result StockProductItemAdd(int ProductSysNo ,int StockSysNo, string StockSysName,int actionType=0 )
        {
            Extra.Erp.Model.Result result = new Model.Result();

            try
            {
                string actionPath = "/base/itemAdd";
                if(actionType==1)
                {
                    actionPath = "/base/itemModi"; 
                }
                else if (actionType > 1)
                {
                    result.Status = false;
                    result.Message = "actionType 操作编码有误，请核实";
                    return result;
                }
                string postDataPath = Config.root + actionPath;
                CBPdProduct product = IPdProductDao.Instance.GetProduct(ProductSysNo);
                EasProduct easProduct = new EasProduct();
                PdProductStock productStock = IPdProductStockDao.Instance.GetEntityByWP(StockSysNo, ProductSysNo);
                if (productStock==null)
                {
                    productStock = new PdProductStock();
                }
                easProduct.APP_Key = Config.APP_Key;
                easProduct.APP_scode = Config.APP_scode;
                easProduct.FDefaultLoc = StockSysName;
                easProduct.FNumber = product.ErpCode;
                easProduct.FName = "[" + StockSysNo + "]" + "[" + product.ErpCode + "]" + product.EasName;
                easProduct.FUnitName = product.SalesMeasurementUnit;
                easProduct.FSecInv = productStock.StockQuantity;
                easProduct.FSalePrice = product.PdPrice.Value.First(p => p.PriceSource == 0 && p.SourceSysNo == 0).Price;
                easProduct.FOrderPrice = product.CostPrice;
                easProduct.FGrossWeight = product.GrosWeight;
                easProduct.FNetWeight = product.NetWeight;
                easProduct.FSize = product.VolumeValue;
                easProduct.isParent = 10;

                string strProduct = Hyt.Util.Serialization.JsonUtil.ToJson(easProduct);
                strProduct = strProduct.Replace("\"APP_Key\":\"\",\"APP_scode\":\"\",","");
                string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath,
                    strProduct
                    );
                
                EasResult easResult = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult>(posData);
                if(easResult.success)
                {
                    result.Status = true;
                    result.Message = easResult.message;
                    ERPStockProduct erpStockProduct = new ERPStockProduct() {
                        ERPProductName = easProduct.FName,
                        FKFPeriod = easProduct.FKFPeriod,
                        ProductCode = easProduct.FNumber,
                        ProductSysNo = ProductSysNo,
                        StockSysNo = StockSysNo,
                        StockNum = easProduct.FSecInv 
                    };
                    ERPStockProduct erpProduct = IERPStockProductDao.Instance.GetMod(ProductSysNo, StockSysNo);
                    if (erpProduct==null)
                    {
                        IERPStockProductDao.Instance.InsertMod(erpStockProduct);
                    }
                    else
                    {
                        erpStockProduct.SysNo = erpProduct.SysNo;
                        IERPStockProductDao.Instance.UpdateMod(erpStockProduct);
                    }
                    
                }
                else
                {
                    if (easResult.message.IndexOf("同名称的商品已经存在") != -1)
                    {
                        ERPStockProduct erpStockProduct = new ERPStockProduct()
                        {
                            ERPProductName = easProduct.FName,
                            FKFPeriod = easProduct.FKFPeriod,
                            ProductCode = easProduct.FNumber,
                            ProductSysNo = ProductSysNo,
                            StockSysNo = StockSysNo,
                            StockNum = easProduct.FSecInv
                        };
                        ERPStockProduct erpProduct = IERPStockProductDao.Instance.GetMod(ProductSysNo, StockSysNo);
                        if (erpProduct == null)
                        {
                            IERPStockProductDao.Instance.InsertMod(erpStockProduct);
                        }
                        else
                        {
                            erpStockProduct.SysNo = erpProduct.SysNo;
                            IERPStockProductDao.Instance.UpdateMod(erpStockProduct);
                        }
                        result.Status = true;
                        result.Message = easResult.message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = easResult.message;
                    }
                }
                
            }catch(Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            result.StatusCode = ProductSysNo.ToString();
            return result;
        }

        /// <summary>
        /// 商品ERP上的商品
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <param name="StockSysNo">商品所在的仓库</param>
        /// <returns></returns>
        public Extra.Erp.Model.Result StockProductItemDelete(int ProductSysNo, int StockSysNo)
        {
            Extra.Erp.Model.Result result = new Model.Result();
            string actionPath = "/base/itemDel";
            ERPStockProduct erpProduct = IERPStockProductDao.Instance.GetMod(ProductSysNo, StockSysNo);
            if (erpProduct != null)
            {
                string postDataPath = Config.root + actionPath;
                string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, "{ \"FName\":\"" + erpProduct.ERPProductName + "\",\"FNumber\":\"" + erpProduct.ProductCode + "\"}");
                EasResult easResult = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult>(posData);

                result.Status = easResult.success;
                result.Message = easResult.message;

                if (result.Status)
                {
                    IERPStockProductDao.Instance.DeleteMod(erpProduct.SysNo);
                }
            }

            return result;
        }

        /// <summary>
        /// 查询仓库库存
        /// </summary>
        /// <param name="wareCode"></param>
        /// <param name="ErpCodeList"></param>
        /// <returns></returns>
        public Extra.Erp.Model.EasResult SearchInvList(string wareCode, string[] ErpCodeList)
        {
            //wareCode = "";
            Extra.Erp.Model.EasResult<List<EasStockProduct>> resultData = new EasResult<List<EasStockProduct>>();
            string actionPath = "/base/InvList";
            string postDataPath = Config.root + actionPath;
            string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, "{ \"FNumberS\":\"" + string.Join(",", ErpCodeList) + "\",\"FStockID\":\"" + wareCode + "\",\"FNumberE\":\"\",\"FKFDate\":\"\",\"FBatchNo\":\"\"}");
                resultData = Hyt.Util.Serialization.JsonUtil.ToObject<Extra.Erp.Model.EasResult<List<EasStockProduct>>>(posData);
            return resultData;
        }
    }
}
