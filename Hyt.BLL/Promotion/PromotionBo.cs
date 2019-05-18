using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Hyt.Util;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 促销业务
    /// </summary>
    /// <remarks>2013-06-26 吴文强 创建</remarks>
    public class PromotionBo : BOBase<PromotionBo>, IPromotionBo
    {
        #region 促销管理
        /// <summary>
        /// 分页获取促销
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>
        /// 2013-08-21 黄志勇 创建
        /// 2014-01-09 朱家宏 增加使用平台查询
        /// </remarks>
        public Pager<SpPromotion> DoPromotionQuery(ParaPromotion filter)
        {
            if (filter.UsePlatform != null)
            {
                switch (filter.UsePlatform)
                {
                    case (int)PromotionStatus.促销使用平台.PC商城:
                        filter.WebPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.门店:
                        filter.ShopPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.手机商城:
                        filter.MallAppPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.物流App:
                        filter.LogisticsAppPlatform = 1;
                        break;
                }
            }
            return ISpPromotionDao.Instance.GetPromotion(filter);
        }
        /// <summary>
        /// 分页获取促销(有分销商)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public Pager<ParaDealerPromotion> DoDealerPromotionQuery(ParaPromotionpager filter)
        {
            if (filter.UsePlatform != null)
            {
                switch (filter.UsePlatform)
                {
                    case (int)PromotionStatus.促销使用平台.PC商城:
                        filter.WebPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.门店:
                        filter.ShopPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.手机商城:
                        filter.MallAppPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.物流App:
                        filter.LogisticsAppPlatform = 1;
                        break;
                }
            }
            return ISpPromotionDao.Instance.GetDealerPromotion(filter);
        }

        /// <summary>
        /// 保存促销
        /// </summary>
        /// <param name="promotion">促销实体</param>
        /// <param name="user">操作者</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public int SavePromotion(SpPromotion promotion, SyUser user)
        {
            promotion.LastUpdateBy = user.SysNo;
            promotion.LastUpdateDate = DateTime.Now;
            if (promotion.SysNo > 0) return ISpPromotionDao.Instance.Update(promotion);
            promotion.CreatedBy = user.SysNo;
            promotion.CreatedDate = DateTime.Now;
            return ISpPromotionDao.Instance.Insert(promotion);
        }

        /// <summary>
        /// 根据编号取得促销实体
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 余勇 创建</remarks>
        public SpPromotion GetModel(int sysNo)
        {
            return ISpPromotionDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 保存促销
        /// </summary>
        /// <param name="cbSpPromotion">促销参数实体</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 余勇 创建</remarks>
        public Result Save(CBSpPromotion cbSpPromotion, SyUser user)
        {
            SpPromotion model = cbSpPromotion.Promotion;
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改

                var upModel = ISpPromotionDao.Instance.Get(model.SysNo);
                upModel.Description = model.Description;
                upModel.DisplayPrefix = model.DisplayPrefix;
                upModel.EndTime = model.EndTime;
                upModel.IsUsePromotionCode = model.IsUsePromotionCode;
                upModel.Name = model.Name;
                upModel.Priority = model.Priority;
                upModel.PromotionCode = model.PromotionCode;
                upModel.PromotionType = model.PromotionType;
                upModel.PromotionUseQuantity = model.PromotionUseQuantity;
                upModel.PromotionUsedQuantity = model.PromotionUsedQuantity;
                upModel.StartTime = model.StartTime;
                upModel.Status = model.Status;
                upModel.SubjectUrl = model.SubjectUrl;
                upModel.UserUseQuantity = model.UserUseQuantity;
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                upModel.WebPlatform = model.WebPlatform;            //2014-01-07 朱家宏 添加
                upModel.ShopPlatform = model.ShopPlatform;  
                upModel.MallAppPlatform = model.MallAppPlatform;
                upModel.LogisticsAppPlatform = model.LogisticsAppPlatform;
                ISpPromotionDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    ISpPromotionGiftDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionOverlayDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionRuleKeyValueDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionRuleConditionDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    if (cbSpPromotion.PromotionGifts != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionGifts)
                        {
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionGiftDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionOverlays != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionOverlays)
                        {
                            var m = new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = item };
                            ISpPromotionOverlayDao.Instance.Insert(m);
                        }
                        ISpPromotionOverlayDao.Instance.Insert(new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = model.SysNo });
                    }
                    if (cbSpPromotion.PromotionRuleKeyValues != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionRuleKeyValues)
                        {
                            if (item.RuleValue != null && item.RuleValue.EndsWith(";"))
                                item.RuleValue = item.RuleValue.TrimEnd(';');
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionRuleKeyValueDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionRule != null)
                    {
                        cbSpPromotion.PromotionRule.LastUpdateBy = user.SysNo;
                        cbSpPromotion.PromotionRule.LastUpdateDate = DateTime.Now;
                        var ruleCondition = new SpPromotionRuleCondition
                        {
                            PromotionSysNo = model.SysNo,
                            PromotionRuleSysNo = cbSpPromotion.PromotionRule.SysNo
                        };
                        ISpPromotionRuleConditionDao.Instance.Insert(ruleCondition);

                    }
                    res.Status = true;
                    res.StatusCode = model.SysNo;
                }
                #endregion
            }
            else //新增
            {
                #region 新增
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.CreatedDate = DateTime.Now;
                model.Status = (int)PromotionStatus.促销状态.待审;
                model.SysNo = ISpPromotionDao.Instance.Insert(model);
                if (model.SysNo > 0) //添加子表
                {
                    if (cbSpPromotion.PromotionGifts != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionGifts)
                        {
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionGiftDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionOverlays != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionOverlays)
                        {
                            var m = new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = item };
                            ISpPromotionOverlayDao.Instance.Insert(m);
                        }
                        ISpPromotionOverlayDao.Instance.Insert(new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = model.SysNo });
                    }
                    if (cbSpPromotion.PromotionRuleKeyValues != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionRuleKeyValues)
                        {
                            if (item.RuleValue != null && item.RuleValue.EndsWith(";"))
                                item.RuleValue = item.RuleValue.TrimEnd(';');
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionRuleKeyValueDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionRule != null)
                    {
                        cbSpPromotion.PromotionRule.LastUpdateBy = user.SysNo;
                        cbSpPromotion.PromotionRule.LastUpdateDate = DateTime.Now;
                        var ruleCondition = new SpPromotionRuleCondition
                        {
                            PromotionSysNo = model.SysNo,
                            PromotionRuleSysNo = cbSpPromotion.PromotionRule.SysNo
                        };
                        ISpPromotionRuleConditionDao.Instance.Insert(ruleCondition);

                    }
                    res.Status = true;
                    res.StatusCode = model.SysNo;
                }
                
                #endregion
            }
            return res;
        }

        /// <summary>
        /// 审核促销
        /// </summary>
        /// <param name="cbSpPromotion">促销参数实体</param>
        /// <param name="user">操作人</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 余勇 创建</remarks>
        public Result Audit(CBSpPromotion cbSpPromotion, SyUser user)
        {
            SpPromotion model = cbSpPromotion.Promotion;
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改
                var upModel = ISpPromotionDao.Instance.Get(model.SysNo);
                upModel.Name = model.Name;
                upModel.Description = model.Description;
                upModel.DisplayPrefix = model.DisplayPrefix;
                upModel.SubjectUrl = model.SubjectUrl;
                upModel.PromotionType = model.PromotionType;
                upModel.WebPlatform = model.WebPlatform;        //2014-01-07 朱家宏 添加
                upModel.ShopPlatform = model.ShopPlatform;
                upModel.MallAppPlatform = model.MallAppPlatform;
                upModel.LogisticsAppPlatform = model.LogisticsAppPlatform;        //2014-02-26 黄志勇 添加
                upModel.StartTime = model.StartTime;
                upModel.EndTime = model.EndTime;
                upModel.PromotionCode = model.PromotionCode;
                upModel.IsUsePromotionCode = model.IsUsePromotionCode;
                upModel.PromotionUseQuantity = model.PromotionUseQuantity;
                upModel.PromotionUsedQuantity = model.PromotionUsedQuantity;
                upModel.UserUseQuantity = model.UserUseQuantity;
                upModel.Priority = model.Priority;
                upModel.Status = (int)PromotionStatus.促销状态.已审;
                upModel.AuditorSysNo = user.SysNo;
                upModel.AuditDate = DateTime.Now;
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                ISpPromotionDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    ISpPromotionGiftDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionOverlayDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionRuleKeyValueDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    ISpPromotionRuleConditionDao.Instance.DeleteByPromotionSysNo(model.SysNo);
                    if (cbSpPromotion.PromotionGifts != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionGifts)
                        {
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionGiftDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionOverlays != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionOverlays)
                        {
                            var m = new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = item };
                            ISpPromotionOverlayDao.Instance.Insert(m);
                        }
                        ISpPromotionOverlayDao.Instance.Insert(new SpPromotionOverlay { OverlayCode = model.SysNo, PromotionSysNo = model.SysNo });
                    }
                    if (cbSpPromotion.PromotionRuleKeyValues != null)
                    {
                        foreach (var item in cbSpPromotion.PromotionRuleKeyValues)
                        {
                            if (item.RuleValue != null && item.RuleValue.EndsWith(";"))
                                item.RuleValue = item.RuleValue.TrimEnd(';');
                            item.PromotionSysNo = model.SysNo;
                            ISpPromotionRuleKeyValueDao.Instance.Insert(item);

                        }
                    }
                    if (cbSpPromotion.PromotionRule != null)
                    {
                        cbSpPromotion.PromotionRule.LastUpdateBy = user.SysNo;
                        cbSpPromotion.PromotionRule.LastUpdateDate = DateTime.Now;
                        var ruleCondition = new SpPromotionRuleCondition
                        {
                            PromotionSysNo = model.SysNo,
                            PromotionRuleSysNo = cbSpPromotion.PromotionRule.SysNo
                        };
                        ISpPromotionRuleConditionDao.Instance.Insert(ruleCondition);

                    }
                    res.Status = true;
                }
                #endregion
            }
            return res;
        }
        /// <summary>
        /// 确定促销审核
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <param name="user">系统用户</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-03-18 王耀发 注释</remarks>
        public Result AuditPromotion(int sysNo, SyUser user)
        {
            var res = new Result();
            var model = ISpPromotionDao.Instance.Get(sysNo);
            if (model != null)
            {
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.促销状态.已审;
                ISpPromotionDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }
       
        /// <summary>
        /// 取消促销审核
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <param name="user">系统用户</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-21 黄志勇 注释</remarks>
        public Result CalcelAuditPromotion(int sysNo, SyUser user)
        {
            var res = new Result();
            var model = ISpPromotionDao.Instance.Get(sysNo);
            if (model != null)
            {
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.促销状态.待审;
                ISpPromotionDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }

        /// <summary>
        /// 作废促销
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <param name="user">系统用户</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-21 黄志勇 注释</remarks>
        public Result Invalid(int sysNo, SyUser user)
        {
            var res = new Result();
            var model = ISpPromotionDao.Instance.Get(sysNo);
            if (model != null)
            {
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.促销状态.作废;
                ISpPromotionDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }

        /// <summary>
        /// 过期促销
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <param name="user"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public Result Expired(int sysNo, SyUser user)
        {
            var res = new Result();
            var model = ISpPromotionDao.Instance.Get(sysNo);
            if (model != null)
            {
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.促销状态.作废;
                ISpPromotionDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }

        /// <summary>
        /// 促销有效判断
        /// </summary>
        /// <param name="promotionSysNo">促销编号</param>
        /// <returns>t:有效 f:无效</returns>
        /// <remarks></remarks>
        public bool IsValidPromotion(int promotionSysNo)
        {
            //过期日大于等于当前时间且状态为审核通过
            var item = GetModel(promotionSysNo);
            if (item != null)
            {
                return (item.EndTime >= DateTime.Now) && (item.Status == (int) PromotionStatus.促销状态.已审);
            }
            return false;
        }

        /// <summary>
        /// 获取赠品列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-26  余勇 创建</remarks>
        public List<CBSpPromotionGift> GetListByPromotionSysNo(int promotionSysNo)
        {
            return ISpPromotionGiftDao.Instance.GetListByPromotionSysNo(promotionSysNo);
        }

        /// <summary>
        /// 获取促销叠加
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-26  余勇 创建</remarks>
        public List<SpPromotionOverlay> GetPromotionOverlayBySysNo(int promotionSysNo)
        {
            return ISpPromotionOverlayDao.Instance.GetListByPromotionSysNo(promotionSysNo);
        }

        /// <summary>
        /// 获取促销规则值列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-26  余勇 创建</remarks>
        public List<SpPromotionRuleKeyValue> GetRuleKeyValueListSysNo(int promotionSysNo)
        {
            return ISpPromotionRuleKeyValueDao.Instance.GetListByPromotionSysNo(promotionSysNo);
        }

        /// <summary>
        /// 保存促销赠品
        /// </summary>
        /// <param name="list">促销赠品列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public void SaveSpPromotionGift(IEnumerable<SpPromotionGift> list)
        {
            list.Apply(item => item.SysNo = ISpPromotionGiftDao.Instance.Insert(item));
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="list">促销赠品列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public void DelSpPromotionGift(IEnumerable<int> list)
        {
            list.Apply(item => ISpPromotionGiftDao.Instance.Delete(item));
        }

        /// <summary>
        /// 保存促销叠加
        /// </summary>
        /// <param name="list">促销叠加列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public void SavePromotionOverlay(IEnumerable<SpPromotionOverlay> list)
        {
            list.Apply(item => item.SysNo = ISpPromotionOverlayDao.Instance.Insert(item));
        }

        /// <summary>
        /// 删除促销叠加
        /// </summary>
        /// <param name="list">促销叠加列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public void DelPromotionOverlay(IEnumerable<SpPromotionOverlay> list)
        {
            list.Apply(item => ISpPromotionOverlayDao.Instance.Delete(item.SysNo));
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"ProductErpCode", "商品编号"},
                {"PurchasePrice", "加购价"},
                {"RequirementMinAmount", "所需最小金额"},
                {"RequirementMaxAmount", "所需最大金额"},
                {"MaxSaleQuantity", "最大销售数量"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public Result<List<CBSpPromotionGift>> ImportPromotionGiftExcel(Stream stream, int operatorSysno)
        {
            var res = new Result<List<CBSpPromotionGift>>() { Status = false };
            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                res.Message = string.Format("数据导入错误:{0}", ex.Message);
                return res;
            }
            if (dt == null)
            {
                //not all the cols mapped
                res.Message = string.Format("请选择正确的excel!");
                return res;
            }
            var lstToInsert = new List<CBSpPromotionGift>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;
                if (cols.Any(p => (dt.Rows[i][p] == null || string.IsNullOrEmpty(dt.Rows[i][p].ToString()))))
                {

                    res.Message = string.Format("excel表第{0}不能有空值", excelRow);
                     return res;
                }

                var erpCode = dt.Rows[i][DicColsMapping["ProductErpCode"]].ToString();
                var productModel= PdProductBo.Instance.GetProductByErpCode(erpCode);
                if (productModel==null)
                {
                    res.Message = string.Format("excel表第{0}行商品系统编号所属的商品不存在", excelRow);
                    return res;
                }
               
                var purchasePrice = dt.Rows[i][DicColsMapping["PurchasePrice"]].ToString().Trim();
                if (purchasePrice.Length > 8)
                {
                    res.Message = string.Format("excel表第{0}行加购价长度不应大于8", excelRow);
                    return res;
                }
                decimal dPurchasePrice;
                if (!decimal.TryParse(purchasePrice, out dPurchasePrice))
                {
                    res.Message = string.Format("excel表第{0}行加购价应为数值", excelRow);
                    return res;
                }

                var requirementMinAmount = dt.Rows[i][DicColsMapping["RequirementMinAmount"]].ToString().Trim();
                if (requirementMinAmount.Length >8)
                {
                    res.Message =  string.Format("excel表第{0}行所需最小金额长度不应大于8", excelRow);
                    return res;
                }

                decimal dRequirementMinAmount;
                if (!decimal.TryParse(requirementMinAmount, out dRequirementMinAmount))
                {
                    res.Message = string.Format("excel表第{0}行所需最小金额应为数值", excelRow);
                    return res;
                }

                var requirementMaxAmount = dt.Rows[i][DicColsMapping["RequirementMaxAmount"]].ToString().Trim();
                if (requirementMaxAmount.Length > 8)
                {
                    res.Message = string.Format("excel表第{0}行所需最大金额长度不应大于8", excelRow);
                    return res;
                }

                decimal dRequirementMaxAmount;
                if (!decimal.TryParse(requirementMaxAmount, out dRequirementMaxAmount))
                {
                    res.Message =  string.Format("excel表第{0}行所需最大金额应为数值", excelRow);
                    return res;
                }

                var maxSaleQuantity = dt.Rows[i][DicColsMapping["MaxSaleQuantity"]].ToString().Trim();
                if (maxSaleQuantity.Length > 8)
                {
                    res.Message =  string.Format("excel表第{0}行最大销售数量长度不应大于8", excelRow);
                    return res;
                }
                int dMaxSaleQuantity;
                if (!int.TryParse(maxSaleQuantity, out dMaxSaleQuantity))
                {
                    res.Message =  string.Format("excel表第{0}行最大销售数量应为整数", excelRow);
                    return res;
                }
                decimal dBasePrice = 0;
                var pdPrice = PdProductBo.Instance.SelectProductPrice(0, productModel.SysNo);
                if (pdPrice != null)
                {
                    dBasePrice = pdPrice.BasicPrice;
                }
                var model = new CBSpPromotionGift
                {
                    ProductErpCode=erpCode,
                    ProductSysNo=productModel.SysNo,
                    MaxSaleQuantity = dMaxSaleQuantity,
                    ProductName = productModel.EasName,
                    PurchasePrice= dPurchasePrice,
                    RequirementMinAmount = dRequirementMinAmount,
                    RequirementMaxAmount = dRequirementMaxAmount,
                    BasePrice  = dBasePrice
                };
                if (lstToInsert.Any(p => p.ProductSysNo == model.ProductSysNo))
                {
                    res.Message = string.Format("excel表第{0}行商品商品编号重复", excelRow);
                    return res;
                }
               lstToInsert.Add(model);
            }
          
            if (lstToInsert.Count == 0)
            {
                res.Message = "导入的数据为空!";
                return res;
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            res.Message = msg;
            res.Data = lstToInsert;
            res.Status = true;
            return res;
        }
        #endregion

        #region 优惠券管理
        /// <summary>
        /// 查询优惠券
        /// </summary>
        /// <param name="sysNo">SysNo</param>
        /// <returns>实体</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public SpCoupon GetEntity(int sysNo)
        {
            return ISpCouponDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 查询优惠券
        /// </summary>
        /// <param name="sysNo">SysNo</param>
        /// <returns>实体</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public CBSpCoupon GetCoupon(int sysNo)
        {
            return ISpCouponDao.Instance.GetCoupon(sysNo);
        }

        /// <summary>
        /// 分页获取优惠券
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>
        /// 2013-08-21 黄志勇 创建
        /// 2014-01-09 朱家宏 增加使用平台查询
        /// </remarks>
        public Pager<CBSpCoupon> DoCouponQuery(ParaCoupon filter)
        {
            if (filter.UsePlatform != null)
            {
                switch (filter.UsePlatform)
                {
                    case (int)PromotionStatus.促销使用平台.PC商城:
                        filter.WebPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.门店:
                        filter.ShopPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.手机商城:
                        filter.MallAppPlatform = 1;
                        break;
                    case (int)PromotionStatus.促销使用平台.物流App:
                        filter.LogisticsAppPlatform = 1;
                        break;
                }
            }
            return ISpCouponDao.Instance.GetCoupon(filter);
        }

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="coupon">促销规则实体</param>
        /// <param name="user">操作者</param>
        /// <param name="originalCoupon">原优惠卷(复制优惠卷时使用)</param>
        /// <param name="couponCardNo">优惠卡号(复制优惠卡时使用)</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-22 黄志勇 创建
        /// 日期 朱家宏 添加参数：originalCoupon/couponCardNo
        /// </remarks>
        public int SaveCoupon(SpCoupon coupon, SyUser user, SpCoupon originalCoupon = null, string couponCardNo = null)
        {
            coupon.LastUpdateBy = user.SysNo;
            coupon.LastUpdateDate = DateTime.Now;
            if (coupon.SysNo > 0)
                return ISpCouponDao.Instance.Update(coupon);

            coupon.CreatedBy = user.SysNo;
            coupon.CreatedDate = DateTime.Now;

            var couponSysNo = ISpCouponDao.Instance.Insert(coupon);

            //2013-12-30 朱家宏 私有优惠卷时写日志
            if (couponSysNo > 0 && coupon.Type == (int) PromotionStatus.优惠券类型.私有)
            {
                //需求为仅在新建(包括复制)私有优惠卷时写日志，其他情况下不管
                var originalCouponSysNo = (originalCoupon == null) ? 0 : originalCoupon.SysNo;
                var log = new SpCouponReceiveLog
                    {
                        CouponSysNo = couponSysNo,
                        OriginatorSysNo = user.SysNo,
                        RecipientSysNo = coupon.CustomerSysNo,
                        ReceiveTime = coupon.CreatedDate,
                        CouponCardNo = couponCardNo,
                        OriginalCouponSysNo = originalCouponSysNo
                    };
                SpCouponReceiveLogBo.Instance.Insert(log);
            }
            return couponSysNo;
        }

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="coupon">促销规则实体</param>
        /// <param name="userSysNo">操作者</param>
        /// <param name="originalCoupon">原优惠卷(复制优惠卷时使用)</param>
        /// <param name="couponCardNo">优惠卡号(复制优惠卡时使用)</param>
        /// <returns></returns>
        /// <remarks>2013-12-31 黄波 创建</remarks>
        public int SaveCoupon(SpCoupon coupon, int userSysNo, SpCoupon originalCoupon = null, string couponCardNo = null)
        {
            var syUser = new SyUser { SysNo = userSysNo };
            return SaveCoupon(coupon, syUser);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        public SpCoupon GetCoupon(string couponCode)
        {
            return ISpCouponDao.Instance.GetCoupon(couponCode);
        }

       
        #endregion

        #region 促销规则
        /// <summary>
        /// 分页获取促销规则
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public Pager<SpPromotionRule> DoPromotionRuleQuery(ParaPromotionRule filter)
        {
            return ISpPromotionRuleDao.Instance.GetPromotionRule(filter);
        }

        /// <summary>
        /// 保存促销规则
        /// </summary>
        /// <param name="promotionRule">促销规则实体</param>
        /// <param name="user">操作者</param>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public int SavePromotionRule(SpPromotionRule promotionRule, SyUser user)
        {
            promotionRule.LastUpdateBy = user.SysNo;
            promotionRule.LastUpdateDate = DateTime.Now;
            if (promotionRule.SysNo > 0)
                return ISpPromotionRuleDao.Instance.Update(promotionRule);
            else
            {
                promotionRule.CreatedBy = user.SysNo;
                promotionRule.CreatedDate = DateTime.Now;
                return ISpPromotionRuleDao.Instance.Insert(promotionRule);
            }

        }

        /// <summary>
        /// html转换
        /// </summary>
        /// <param name="adminHtml">html</param>
        /// <returns>键值对列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public List<KeyValuePair<string, string>> GetListByAdminHtml(string adminHtml)
        {
            char sp1 = ';';
            char sp2 = '@';
            var res = new List<KeyValuePair<string, string>>();
            if(!string.IsNullOrEmpty(adminHtml)){
                var arr= adminHtml.Split(sp1);
                foreach (var s in arr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        var arr2 = s.Split(sp2);
                        if(arr2.Length>1){
                            res.Add(new KeyValuePair<string, string>(arr2[0], arr2[1]));
                        }
                    }
                }
            }
            return res;
        }
        #endregion

        /// <summary>
        /// 查询团购和组合以及促销规则中包含的所有商品
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<int> GetAllPromotionProduct()
        {
            return ISpPromotionDao.Instance.GetAllPromotionProduct();
        }

        /// <summary>
        /// 生成新促销码
        /// </summary>
        /// <param></param>
        /// <returns>新代码(8位)</returns>
        /// <remarks>2013-12-09 朱家宏 创建</remarks>
        public string GenerateNewPromotionCode()
        {
            var newCode = RandomString.GetRndStrOnlyFor(8, false, true, true);
            return ExsitsPromotionCode(newCode) ? GenerateNewPromotionCode() : newCode;
        }

        /// <summary>
        /// 促销码是否存在
        /// </summary>
        /// <param name="promotionCode">促销码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>t:存在 f:不存在</returns>
        /// <remarks>2013-12-09 朱家宏 创建</remarks>
        public bool ExsitsPromotionCode(string promotionCode, int promotionSysNo = 0)
        {
            if (!string.IsNullOrEmpty(promotionCode))
                promotionCode = promotionCode.Replace(" ", "").Replace("　", "");
            var model = ISpPromotionDao.Instance.GetByPromotionCode(promotionCode);
            if (promotionSysNo == 0 && model != null)
                return true;
            if (promotionSysNo != 0 && model != null && model.SysNo != promotionSysNo)
                return true;
            return false;
        }
        /// <summary>
        /// 获得已审核，在有效时间内的促销
        /// </summary>
        /// <returns></returns>
        /// 王耀发 2016-1-15 创建
        public List<SpPromotion> GetSpPromotionList(int PromotionType)
        {
            return ISpPromotionDao.Instance.GetSpPromotionList(PromotionType);
        }
        /// <summary>
        /// 通过促销编号获取详情
        /// </summary>
        /// <param name="PromotionSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public SpPromotionDealer GetByPromotionSysNo(int PromotionSysNo)
        {
            return ISpPromotionDao.Instance.GetByPromotionSysNo(PromotionSysNo);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertPromotionDealer(SpPromotionDealer entity)
        {
            var dealerPromotion = GetByPromotionSysNo(entity.PromotionSysNo);
            if (dealerPromotion == null)
            {
                return ISpPromotionDao.Instance.InsertPromotionDealer(entity);
            }
            else
            {
                entity.SysNo = dealerPromotion.SysNo;
                ISpPromotionDao.Instance.UpdatePromotionDealer(entity);
            }
            return 0;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>2016-08-29 周 创建</remarks>
        public void UpdatePromotionDealer(SpPromotionDealer entity)
        {
            var dealerPromotion = GetByPromotionSysNo(entity.PromotionSysNo);
            if (dealerPromotion != null)
            {
                entity.SysNo = dealerPromotion.SysNo;
                ISpPromotionDao.Instance.UpdatePromotionDealer(entity);
            }
        }
    }
}