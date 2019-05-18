using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;
using System.IO;
using System.Data;
using Hyt.Util;
using Hyt.BLL.Product;
using Hyt.BLL.Log;


namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 商检信息
    /// </summary>
    /// <remarks>
    /// 2016-03-22 王耀发 创建
    /// </remarks>
    public class LogisticsBo : BOBase<LogisticsBo>
    {
 
        /// <summary>
        /// 保存高捷商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        public Result SaveLgGaoJieGoodsInfo(LgGaoJieGoodsInfo model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            LgGaoJieGoodsInfo entity = ILogisticsDao.Instance.GetLgGaoJieGoodsInfoEntityByPid(model.ProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                ILogisticsDao.Instance.UpdateLgGaoJieGoodsInfoEntity(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                ILogisticsDao.Instance.InsertLgGaoJieGoodsInfoEntity(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public LgGaoJieGoodsInfo GetLgGaoJieGoodsInfoEntityByPid(int ProductSysNo)
        {
            return ILogisticsDao.Instance.GetLgGaoJieGoodsInfoEntityByPid(ProductSysNo);
        }
        /// <summary>
        /// 保存高捷商品推送信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        public Result InsertLgGaoJiePushInfoEntity(LgGaoJiePushInfo model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = user.SysNo;
            model.LastUpdateBy = user.SysNo;
            model.LastUpdateDate = DateTime.Now;
            ILogisticsDao.Instance.InsertLgGaoJiePushInfoEntity(model);
            r.Status = true;
            return r;
        }
        #region 高捷商品备案导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMappingGaoJie = new Dictionary<string, string>
            {               
                {"ErpCode", "商品编码"},
                {"goods_name", "物品名称"},
                {"goods_ptcode", "税号"},
                {"brand", "品牌"},
                {"goods_spec", "规格型号"},
                {"ycg_code", "原产国代码"},
                {"hs_code","HS编码"},
                {"goods_barcode","条形码"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcelGaoJie(Stream stream, int operatorSysno)
        {
            DataTable dt = null;
            var cols = DicColsMappingGaoJie.Select(p => p.Value).ToArray();

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            var excellst = new List<LgGaoJieGoodsInfo>();
            var lstToInsert = new List<LgGaoJieGoodsInfo>();
            var lstToUpdate = new List<LgGaoJieGoodsInfo>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    //HS编码、条形码可以为空
                    if (j != 6 && j != 7)
                    {
                        if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString())))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行数据不能有空值", (excelRow + 1)),
                                Status = false
                            };
                        }
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMappingGaoJie["ErpCode"]].ToString().Trim();
                PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                if (pEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 1)),
                        Status = false
                    };
                }
                //物品名称
                var goods_name = dt.Rows[i][DicColsMappingGaoJie["goods_name"]].ToString().Trim();
                //税号
                var goods_ptcode = dt.Rows[i][DicColsMappingGaoJie["goods_ptcode"]].ToString().Trim();
                //品牌
                var brand = dt.Rows[i][DicColsMappingGaoJie["brand"]].ToString().Trim();
                //规格型号
                var goods_spec = dt.Rows[i][DicColsMappingGaoJie["goods_spec"]].ToString().Trim();
                //原产国代码
                var ycg_code = dt.Rows[i][DicColsMappingGaoJie["ycg_code"]].ToString().Trim();
                //HS编码
                var hs_code = dt.Rows[i][DicColsMappingGaoJie["hs_code"]].ToString().Trim();
                //条形码
                var goods_barcode = dt.Rows[i][DicColsMappingGaoJie["goods_barcode"]].ToString().Trim();

                var model = new LgGaoJieGoodsInfo
                {
                    ProductSysNo = pEntity.SysNo,
                    goods_name = goods_name,
                    goods_ptcode = goods_ptcode,
                    brand = brand,
                    goods_spec = goods_spec,
                    hs_code = hs_code,
                    ycg_code = ycg_code,
                    goods_barcode = goods_barcode,
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);
            }
            var lstExisted = DataAccess.Logistics.ILogisticsDao.Instance.GetAllGaoJieGoodsInfo();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.ProductSysNo == excelModel.ProductSysNo))
                {
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                DataAccess.Logistics.ILogisticsDao.Instance.CreateExcelGaoJieGoodsInfo(lstToInsert);
                DataAccess.Logistics.ILogisticsDao.Instance.UpdateExcelGaoJieGoodsInfo(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入高捷商品备案信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }

        #endregion
    }
}
