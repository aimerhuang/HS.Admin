using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Icp;
using Hyt.Model.WorkflowStatus;
using System.IO;
using System.Data;
using Hyt.Util;
using Hyt.BLL.Product;
using Hyt.BLL.Log;

namespace Hyt.BLL.ApiIcq
{
    /// <summary>
    /// 商检信息
    /// </summary>
    /// <remarks>
    /// 2016-03-22 王耀发 创建
    /// </remarks>
    public class IcpBo : BOBase<IcpBo>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public Pager<CIcp> GetGoodsPagerList(ParaIcpGoodsFilter filter)
        {
            return IcpDao.Instance.GoodsQuery(filter);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public Pager<CIcp> GetOrderPagerList(ParaIcpGoodsFilter filter)
        {
            return IcpDao.Instance.OrderQuery(filter);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public Pager<CBIcpGoodsItem> IcpGoodsItemQuery(ParaIcpGoodsFilter filter)
        {
            return IcpDao.Instance.IcpGoodsItemQuery(filter);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public CIcp GetEntity(int sysNo)
        {
            return IcpDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public CIcp GetEntityByMessageIDType(string MessageID, string MessageType)
        {
            return IcpDao.Instance.GetEntityByMessageIDType(MessageID, MessageType);
        }
        /// <summary>
        /// 添加商检信息记录
        /// </summary>
        /// <param name="model">商检信息</param>
        /// <returns>返回新建记录的编号</returns>       
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public int Create(CIcp model)
        {
            return IcpDao.Instance.Insert(model);
        }

        /// <summary>
        /// 获取明细列表
        /// </summary>
        /// <param name="IcpGoodsSysNo">系统编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public List<CBIcpGoodsItem> GetListByIcpGoodsSysNo(int IcpGoodsSysNo)
        {
            return IcpDao.Instance.GetListByIcpGoodsSysNo(IcpGoodsSysNo);
        }
    
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public Pager<CBIcpGoodsItem> GetIcpProductList(ParaIcpGoodsItemFilter filter)
        {
            return IcpDao.Instance.GetIcpProductList(filter);
        }

        /// <summary>
        /// 更新接收回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="DocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateIcpDocRec(int SysNo, string DocRec)
        {
            IcpDao.Instance.UpdateIcpDocRec(SysNo, DocRec);
        }
        /// <summary>
        /// 更新单一窗口平台接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="DocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdatePlatDocRecByMessageID(string MessageID, string PlatDocRec, string PlatStatus)
        {
            IcpDao.Instance.UpdatePlatDocRecByMessageID(MessageID, PlatDocRec, PlatStatus);
        }
        /// <summary>
        /// 更新商检接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateCiqDocRecByMessageID(string MessageID, string CiqDocRec, string CiqStatus)
        {
            IcpDao.Instance.UpdateCiqDocRecByMessageID(MessageID, CiqDocRec, CiqStatus);
        }
        /// <summary>
        /// 更新商检接收状态
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateStatus(string MessageID, int Status)
        {
            IcpDao.Instance.UpdateStatus(MessageID, Status);
        }
        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CiqRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateIcpCiqRec(int SysNo, string CiqRec)
        {
            IcpDao.Instance.UpdateIcpCiqRec(SysNo, CiqRec);
        }
        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateIcpGoodsItemCIQ(int IcpType, string EntGoodsNo, string CIQGRegStatus, string CIQNotes)
        {
            IcpDao.Instance.UpdateIcpGoodsItemCIQ(IcpType,EntGoodsNo, CIQGRegStatus, CIQNotes);
        }
        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CusRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateIcpCusRec(int SysNo, string CusRec)
        {
            IcpDao.Instance.UpdateIcpCusRec(SysNo, CusRec);
        }
        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="OpResult">状态</param>
        /// <param name="CustomsNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateIcpGoodsItemCUS(int IcpType, string EntGoodsNo, string OpResult, string CustomsNotes)
        {
            IcpDao.Instance.UpdateIcpGoodsItemCUS(IcpType,EntGoodsNo, OpResult, CustomsNotes);
        }

        /// <summary>
        /// 保存白云机场的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        public Result SaveIcpBYJiChangGoodsInfo(IcpBYJiChangGoodsInfo model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            IcpBYJiChangGoodsInfo entity = IcpDao.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(model.ProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.UpdateIcpBYJiChangGoodsInfoEntity(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.InsertIcpBYJiChangGoodsInfo(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public IcpBYJiChangGoodsInfo GetIcpBYJiChangGoodsInfoEntityByPid(int ProductSysNo)
        {
            return IcpDao.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(ProductSysNo);
        }

        /// <summary>
        /// 保存广州南沙的商品配置信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarks>2016-04-05 王耀发 创建</remarks>
        public Result SaveIcpGZNanShaGoodsInfo(IcpGZNanShaGoodsInfo model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            IcpGZNanShaGoodsInfo entity = IcpDao.Instance.GetIcpGZNanShaGoodsInfoEntityByPid(model.ProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.UpdateIcpGZNanShaGoodsInfoEntity(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.InsertIcpGZNanShaGoodsInfo(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public IcpGZNanShaGoodsInfo GetIcpGZNanShaGoodsInfoEntityByPid(int ProductSysNo)
        {
            return IcpDao.Instance.GetIcpGZNanShaGoodsInfoEntityByPid(ProductSysNo);
        }
        /// <summary>
        /// 更新检验检疫商品备案编号
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateCIQGoodsNo(string EntGoodsNo, string CIQGoodsNo)
        {
            IcpDao.Instance.UpdateCIQGoodsNo(EntGoodsNo, CIQGoodsNo);
        }
        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="OpResult">状态</param>
        /// <param name="CustomsNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateCusGoodsNo(string EntGoodsNo, string CusGoodsNo)
        {
            IcpDao.Instance.UpdateCusGoodsNo(EntGoodsNo, CusGoodsNo);
        }
        /// <summary>
        /// 更新南沙检验检疫商品备案编号
        /// </summary>
        /// <param name="Gcode">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public void UpdateNSCIQGoodsNo(string Gcode, string CIQGoodsNo)
        {
            IcpDao.Instance.UpdateNSCIQGoodsNo(Gcode, CIQGoodsNo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public void UpdateEntGoodsNoByMessageID(string MessageID, string EntGoods)
        {
            IcpDao.Instance.UpdateEntGoodsNoByMessageID(MessageID, EntGoods);
        }

        #region 南沙商检商品备案导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMappingNS = new Dictionary<string, string>
            {               
                {"ErpCode", "商品编码"},
                {"Gcode", "商品货号"},
                {"Gname", "商品名称"},
                {"Spec", "规格型号"},
                {"HSCode", "HS编码"},
                {"Unit", "计量单位(最小)"},
                {"Brand","品牌"},
                {"AssemCountry","原产国/地区"},
                {"SellWebSite","销售网址"},
                {"GoodsBarcode","商品条形码"},
                {"GoodsDesc","商品描述"},
                {"ComName","生产企业名称"},
                {"Ingredient","成分"},
                {"Additiveflag","超范围使用食品添加剂"},
                {"Poisonflag","含有毒害物质"},
                {"Remark","备注"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcelNS(Stream stream, int operatorSysno)
        {
            DataTable dt = null;
            var cols = DicColsMappingNS.Select(p => p.Value).ToArray();

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
            var excellst = new List<IcpGZNanShaGoodsInfo>();
            var lstToInsert = new List<IcpGZNanShaGoodsInfo>();
            var lstToUpdate = new List<IcpGZNanShaGoodsInfo>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    //
                    if (j <= 8)
                    {
                        if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString())))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行第{1}列数据不能有空值", (excelRow + 1), (j + 1)),
                                Status = false
                            };
                        }
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMappingNS["ErpCode"]].ToString().Trim();
                PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                if (pEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 1)),
                        Status = false
                    };
                }
                //商品货号
                var Gcode = dt.Rows[i][DicColsMappingNS["Gcode"]].ToString().Trim();
                //商品名称
                var Gname = dt.Rows[i][DicColsMappingNS["Gname"]].ToString().Trim();
                //规格型号
                var Spec = dt.Rows[i][DicColsMappingNS["Spec"]].ToString().Trim();
                //HS编码
                var HSCode = dt.Rows[i][DicColsMappingNS["HSCode"]].ToString().Trim();
                //计量单位(最小)
                var Unit = dt.Rows[i][DicColsMappingNS["Unit"]].ToString().Trim();
                //品牌
                var Brand = dt.Rows[i][DicColsMappingNS["Brand"]].ToString().Trim();
                //原产国/地区
                var AssemCountry = dt.Rows[i][DicColsMappingNS["AssemCountry"]].ToString().Trim();
                //销售网址
                var SellWebSite = dt.Rows[i][DicColsMappingNS["SellWebSite"]].ToString().Trim();
                //商品条形码
                var GoodsBarcode = dt.Rows[i][DicColsMappingNS["GoodsBarcode"]].ToString().Trim();
                //商品描述
                var GoodsDesc = dt.Rows[i][DicColsMappingNS["GoodsDesc"]].ToString().Trim();
                //生产企业名称
                var ComName = dt.Rows[i][DicColsMappingNS["ComName"]].ToString().Trim();
                //成分
                var Ingredient = dt.Rows[i][DicColsMappingNS["Ingredient"]].ToString().Trim();
                //超范围使用食品添加剂
                var Additiveflag = dt.Rows[i][DicColsMappingNS["Additiveflag"]].ToString().Trim();
                //含有毒害物质
                var Poisonflag = dt.Rows[i][DicColsMappingNS["Poisonflag"]].ToString().Trim();
                //备注
                var Remark = dt.Rows[i][DicColsMappingNS["Remark"]].ToString().Trim();

                var model = new IcpGZNanShaGoodsInfo
                {
                    ProductSysNo = pEntity.SysNo,
                    Gcode = Gcode,
                    Gname = Gname,
                    Spec = Spec,
                    HSCode = HSCode,
                    Unit = Unit,
                    Brand = Brand,
                    AssemCountry = AssemCountry,

                    SellWebSite = SellWebSite,
                    GoodsBarcode = GoodsBarcode,
                    GoodsDesc = GoodsDesc,
                    ComName = ComName,
                    Ingredient = Ingredient,
                    Additiveflag = Additiveflag,
                    Poisonflag = Poisonflag,
                    Remark = Remark,

                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);
            }
            var lstExisted = DataAccess.Icp.IcpDao.Instance.GetAllGZNanShaGoodsInfo();
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
                DataAccess.Icp.IcpDao.Instance.CreateExcelGZNanShaGoodsInfo(lstToInsert);
                DataAccess.Icp.IcpDao.Instance.UpdateExcelGZNanShaGoodsInfo(lstToUpdate);
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

        /// <summary>
        /// 根据商品ID获取启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public IcpQiBangGoodsInfo GetIcpQiBangGoodsInfoEntityByPid(int ProductSysNo)
        {
            return IcpDao.Instance.GetIcpQiBangGoodsInfoEntityByPid(ProductSysNo);
        }
        /// <summary>
        /// 保存启邦商品备案信息
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public Result SaveIcpQiBangGoodsInfo(IcpQiBangGoodsInfo model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            IcpQiBangGoodsInfo entity = IcpDao.Instance.GetIcpQiBangGoodsInfoEntityByPid(model.ProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.UpdateIcpBYJiChangGoodsInfoEntity(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IcpDao.Instance.InsertIcpBYJiChangGoodsInfo(model);
                r.Status = true;
            }
            return r;
        }
    }
}
