using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 优惠卡业务
    /// </summary>
    /// <remarks>2014-01-08  朱家宏 创建</remarks>
    public class SpCouponCardBo : BOBase<SpCouponCardBo>
    {
        #region 优惠卡绑定

        /// <summary>
        /// 通过优惠卡号获取优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>优惠卡</returns>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        public CBCouponCard GetAggregatedCouponCard(string couponCardNo)
        {
            if (string.IsNullOrWhiteSpace(couponCardNo))
                throw new ArgumentNullException("couponCardNo");

            var card = new CBCouponCard
                {
                    CouponCard = ISpCouponCardDao.Instance.Get(couponCardNo),
                    Coupons = new List<SpCoupon>()
                };

            if (card.CouponCard == null)
                throw new HytException("未找到优惠卡");

            card.CouponCardType =
                ISpCouponCardTypeDao.Instance.GetEntity(card.CouponCard.CardTypeSysNo) ?? new SpCouponCardType();
            card.Associations =
                ISpCouponCardAssociateDao.Instance.GetAllByCardTypeSysNo(
                    card.CouponCard.CardTypeSysNo) ?? new List<SpCouponCardAssociate>();

            foreach (var item in card.Associations)
            {
                card.Coupons.Add(ISpCouponDao.Instance.GetEntity(item.CouponSysNo));
            }

            return card;
        }

        /// <summary>
        /// 绑定优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="recordNum">操作次数</param>
        /// <param name="syUser">当前用户</param>
        /// <param name="action">绑定优惠卡回调</param>
        /// <returns>分配成功后的优惠卷新编号</returns>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        public int AssignToCustomer(string couponCardNo, int couponSysNo, int customerSysNo, int recordNum = 1, SyUser syUser = null, Action<int> action = null)
        {
            /*
             * 优惠卡、优惠卡类型条件：
             * 
             * 1.优惠卡状态为启用
             * 
             * 2.优惠卡类型状态为启用
             * 
             * 3.优惠卡类型有效日期大于当前
             * 
             * 4.未分配过给该客户的优惠卡
             * 
             * 5.更新优惠卡激活、中止日期
             */

            /*
             * 优惠卷条件：
             * 
             * 1.查询条件：所选优惠卷为系统、优惠卷状态为已审核、结束日期大于当前、允许使用数量>0
             * 
             * 2.复制后初始数据：优惠卷类型为私有，允许使用数量为1，已使用数量为0，客户系统编号为指定客户，生成新的优惠卷随机代码，
             *   状态为待审核，审核人、创建人为当前用户，审核日期、创建日期为当前日期、是优惠卡
             * 
             * 3.更新源优惠卷：已使用数量+1，更新人为当前用户，更新日期为当前日期
             * 
             * 4.完成复制后，对该优惠卷仅提供审核和作废
             * 
             * 5.操作权限，具备优惠卷操作权限的用户，或优惠卷的创建用户
             */

            #region 优惠卡、优惠卡类型数据校验

            if (string.IsNullOrWhiteSpace(couponCardNo))
                throw new ArgumentNullException("couponCardNo");

            var couponCard = ISpCouponCardDao.Instance.Get(couponCardNo);
            if (couponCard == null)
                throw new HytException("未找到优惠卡");

            if (couponCard.Status == (int)PromotionStatus.优惠券卡状态.禁用)
                throw new HytException("不能使用未启用的优惠卡号");

            var couponCardType = ISpCouponCardTypeDao.Instance.GetEntity(couponCard.CardTypeSysNo);
            if (couponCardType == null)
                throw new HytException("未找到优惠卡类型");

            if (couponCardType.Status == (int)PromotionStatus.优惠券卡类型状态.禁用)
                throw new HytException("不能使用未启用的优惠卡类型");

            if (couponCardType.EndTime < DateTime.Now)
                throw new HytException("优惠卡类型已过期");

            if (recordNum == 1)
            {
                var assigned = ISpCouponReceiveLogDao.Instance.HasGet(couponCardNo, customerSysNo);
                if (assigned)
                    throw new HytException("不能重复绑定优惠卡");

                if (couponCard.ActivationTime != DateTime.MinValue)
                    throw new HytException("优惠卡已经被其他用户绑定过，不能重复绑定");
            }

            var associations = ISpCouponCardAssociateDao.Instance.GetAllByCardTypeSysNo(couponCardType.SysNo);
            var couponCardAssociation = associations.First(o => o.CouponSysNo == couponSysNo);
            if (couponCardAssociation == null)
                throw new HytException("未能找到优惠卡、卷关联");

            #endregion

            var newCouponSysNo = 0;
            var coupon = PromotionBo.Instance.GetEntity(couponSysNo);

            #region 优惠卷数据校验

            if (coupon.WebPlatform == 0 && coupon.ShopPlatform == 0 && coupon.MallAppPlatform == 0 &&
                coupon.LogisticsAppPlatform == 0)
            {
                throw new HytException("优惠卷没有选择任何使用平台");
            }

            if (coupon.Status == (int)PromotionStatus.优惠券状态.作废)
            {
                throw new HytException("优惠卡已经作废,绑定失败");
            }

            if (coupon.Type != (int)PromotionStatus.优惠券类型.系统 ||
                coupon.Status != (int)PromotionStatus.优惠券状态.已审核 ||
                coupon.EndTime < DateTime.Now)
            {
                return newCouponSysNo;
            }

            #endregion

            #region 复制优惠卷，初始数据

            if (syUser == null)
            {
                syUser = AdminAuthenticationBo.Instance.Current.Base;
            }
            var bindNumber = couponCardAssociation.BindNumber;
            for (var i = 1; i <= bindNumber; i++)
            {

                var newCoupon = new SpCoupon
                    {
                        SysNo = 0,
                        Type = (int)PromotionStatus.优惠券类型.私有,
                        UseQuantity = 1,
                        UsedQuantity = 0,
                        CustomerSysNo = customerSysNo,
                        CouponCode = SpCouponBo.Instance.GenerateNewCouponCode(),
                        Status = (int)PromotionStatus.优惠券状态.待审核,

                        CouponAmount = coupon.CouponAmount,
                        Description = coupon.Description,
                        StartTime = coupon.StartTime,
                        EndTime = coupon.EndTime,
                        ParentSysNo = coupon.ParentSysNo,
                        PromotionSysNo = coupon.PromotionSysNo,
                        RequirementAmount = coupon.RequirementAmount,
                        SourceDescription = Constant.COUPONDESCRIPTION_BINDUSERCOUPON,
                        IsCouponCard = (int)PromotionStatus.是否优惠卡.是,
                        WebPlatform = coupon.WebPlatform,
                        ShopPlatform = coupon.ShopPlatform,
                        MallAppPlatform = coupon.MallAppPlatform,
                        LogisticsAppPlatform = coupon.LogisticsAppPlatform
                    };
                newCouponSysNo = PromotionBo.Instance.SaveCoupon(newCoupon, syUser, coupon, couponCardNo);
                if (action != null)
                {
                    action(newCouponSysNo);
                }

            }

            #endregion

            #region 更新原优惠卷

            //coupon.UsedQuantity++;
            //PromotionBo.Instance.SaveCoupon(coupon, AdminAuthenticationBo.Instance.Current.Base);

            #endregion

            #region 更新优惠卡激活、中止日期

            couponCard.ActivationTime = DateTime.Now;
            couponCard.TerminationTime = DateTime.Now;
            ISpCouponCardDao.Instance.Update(couponCard);

            #endregion

            return newCouponSysNo;
        }

        #endregion

        #region 优惠卡管理(查询、状态设置、导入）

        /// <summary>
        /// 分页获取优惠券卡号
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public Pager<CBSpCouponCard> DoCouponCardQuery(ParaCouponCard filter)
        {
            return DataAccess.Promotion.ISpCouponCardDao.Instance.GetCouponCard(filter);
        }

        /// <summary>
        /// 更新优惠券卡号状态
        /// </summary>
        /// <param name="sysNo">优惠券卡编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public int UpdateCouponCardStatus(int sysNo, int status)
        {
            return DataAccess.Promotion.ISpCouponCardDao.Instance.UpdateCouponCardStatus(sysNo, status);
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"CardTypeSysNo", "优惠券卡类型ID"},
                {"CouponCardNo", "卡号"},
                {"Status", "状态"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public Result ImportExcel(Stream stream, int operatorSysno)
        {
            DataTable dt = null;
            
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            
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
            var excellst = new List<SpCouponCard>();
            var lstToInsert = new List<SpCouponCard>();
            var lstToUpdate = new List<SpCouponCard>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;
                if (cols.Any(p => (dt.Rows[i][p] == null || string.IsNullOrEmpty(dt.Rows[i][p].ToString()))))
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行数据不能有空值", excelRow),
                            Status = false
                        };
                }
                var cardTypeList = SpCouponCardBo.Instance.GetAllTypeName().Select(p => p.SysNo);
                int typeSysNo = 0;
                var cardTypeSysNo = dt.Rows[i][DicColsMapping["CardTypeSysNo"]].ToString();
                if (!int.TryParse(cardTypeSysNo, out typeSysNo))
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行优惠券卡类型ID必须为整数", excelRow),
                            Status = false
                        };
                }
                if (!cardTypeList.Contains(typeSysNo))
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行优惠券卡类型ID不在优惠卡类型范围内", excelRow),
                            Status = false
                        };
                }
                var couponCardNo = dt.Rows[i][DicColsMapping["CouponCardNo"]].ToString().Trim();
                if (couponCardNo.Length > 10)
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行优惠券卡号长度不应大于10", excelRow),
                            Status = false
                        };
                }

                var statusName = dt.Rows[i][DicColsMapping["Status"]].ToString().Trim();
                if (statusName != "启用" && statusName != "禁用")
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行状态的值应为启用或禁用", excelRow),
                            Status = false
                        };
                }
                var status = statusName == "启用" ? 1 : 0;
                var model = new SpCouponCard
                    {
                        CardTypeSysNo = typeSysNo,
                        CouponCardNo = couponCardNo,
                        Status = status
                    };
                if (excellst.Any(p => p.CouponCardNo == couponCardNo))
                {
                    return new Result
                        {
                            Message = string.Format("excel表第{0}行优惠券卡号重复", excelRow),
                            Status = false
                        };
                }
                excellst.Add(model);
              
            }
            var lstExisted = DataAccess.Promotion.ISpCouponCardDao.Instance.GetAllSpCouponCard();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.CouponCardNo == excelModel.CouponCardNo))
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
                ISpCouponCardDao.Instance.CreateSpCouponCard(lstToInsert);
                ISpCouponCardDao.Instance.UpdateSpCouponCard(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入优惠卡号",
                                         LogStatus.系统日志目标类型.促销, 0, ex, null, operatorSysno);
                return new Result
                    {
                        Message = string.Format("数据更新错误:{0}", ex.Message),
                        Status = false
                    };
            }
            if (lstToInsert.Count == 0 && lstToUpdate.Count == 0)
            {
                return new Result
                    {
                        Message = "导入的数据为空!",
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

        #region 优惠卡类型

        /// <summary>
        /// 分页获取优惠卡类型
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>优惠卡类型分页数据</returns>
        /// <remarks>2014-01-08 朱成果 创建</remarks>
        public Pager<SpCouponCardType> GetCouponCardTypePageList(ParaCouponCardType filter)
        {
            return DataAccess.Promotion.ISpCouponCardTypeDao.Instance.Query(filter);
        }

        /// <summary>
        /// 获取优惠券（必须是系统和优惠卡)
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>获取优惠券（必须是系统和优惠卡)</returns>
        /// <remarks>2014-01-08 朱成果 创建</remarks>
        public Pager<CBSpCoupon> GetCouponsToCard(ParaCoupon filter)
        {
            if (filter == null)
                filter = new ParaCoupon();

            filter.Type = (int)PromotionStatus.优惠券类型.系统;
            filter.Status = (int)PromotionStatus.优惠券状态.已审核;
            filter.ExpiredTime = DateTime.Now;
            filter.IsCouponCard = PromotionStatus.是否优惠卡.是.GetHashCode();
            filter.UseQuantity = 1;
            return ISpCouponDao.Instance.GetCoupon(filter);
        }

        /// <summary>
        /// 获取所有启用的优惠券卡类型
        /// </summary>
        /// <returns>优惠券卡类型集合</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public IList<SpCouponCardType> GetAllTypeName()
        {
            return DataAccess.Promotion.ISpCouponCardTypeDao.Instance.GetAllTypeName();
        }


        /// <summary>
        /// 保存优惠卡类型
        /// </summary>
        /// <returns></returns>
        /// <param name="model">优惠卡类型 扩展</param>
        /// <remarks>2014-01-09 朱成果 创建</remarks>
        public void SaveCouponCardType(CBSpCouponCardType model)
        {
            if (IsExistsTypeName(model.SysNo, model.TypeName))
            {
                throw new Exception("类型名称已经存在，请使用其他名称");
            }
            if (model.SysNo < 1)
            {
                #region 新建优惠卡类型

                model.CreatedBy = model.LastUpdateBy;
                model.CreatedDate = model.LastUpdateDate;
                model.SysNo = Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.Insert(model);
                if (model.SysNo > 0 && model.AssociateItem != null)
                {
                    foreach (var item in model.AssociateItem)
                    {
                        item.CardTypeSysNo = model.SysNo;
                        Hyt.DataAccess.Promotion.ISpCouponCardAssociateDao.Instance.Insert(item);
                    }
                }

                #endregion
            }
            else
            {
                #region 编辑优惠卡类型

                var entity = Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.GetEntity(model.SysNo);
                if (entity != null)
                {
                    entity.LastUpdateBy = model.LastUpdateBy;
                    entity.LastUpdateDate = model.LastUpdateDate;
                    entity.StartTime = model.StartTime;
                    entity.EndTime = model.EndTime;
                    entity.Status = model.Status;
                    entity.TypeName = model.TypeName;
                    entity.TypeDescription = model.TypeDescription;
                    Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.Update(entity);
                    Hyt.DataAccess.Promotion.ISpCouponCardAssociateDao.Instance.DeleteByCardTypeSysNo(entity.SysNo);
                    if (entity.SysNo > 0 && model.AssociateItem != null)
                    {
                        foreach (var item in model.AssociateItem)
                        {
                            item.CardTypeSysNo = entity.SysNo;
                            Hyt.DataAccess.Promotion.ISpCouponCardAssociateDao.Instance.Insert(item);
                        }
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// 优惠卡类型名称是否存在
        /// </summary>
        /// <param name="sysNo">优惠卡类型编号（新建为0 ）</param>
        /// <param name="typeName">优惠卡类型名称</param>
        /// <returns>true/false</returns>
        /// <remarks>2014-01-09 朱成果 创建</remarks>
        public bool IsExistsTypeName(int sysNo, string typeName)
        {
            return Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.IsExistsTypeName(sysNo, typeName);
        }

        /// <summary>
        /// 获取优惠卡类型及其关联优惠券
        /// </summary>
        /// <param name="sysNo">优惠卡类型编号</param>
        /// <returns>优惠卡类型及其关联优惠券</returns>
        /// <remarks>2014-01-09 朱成果 创建</remarks>
        public CBSpCouponCardType GetCouponCardType(int sysNo)
        {
            CBSpCouponCardType oo = new CBSpCouponCardType();
            var model = Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.GetEntity(sysNo);
            if (model.SysNo > 0)
            {
                oo.TypeDescription = model.TypeDescription;
                oo.TypeName = model.TypeName;
                oo.StartTime = model.StartTime;
                oo.EndTime = model.EndTime;
                oo.SysNo = model.SysNo;
                oo.Status = model.Status;
                oo.CreatedBy = model.CreatedBy;
                oo.CreatedDate = model.CreatedDate;
                oo.AssociateItem =
                    Hyt.DataAccess.Promotion.ISpCouponCardAssociateDao.Instance.GetListByCardTypeSysNo(model.SysNo);
            }

            return oo;
        }

        /// <summary>
        /// 删除优惠卡类型
        /// </summary>
        /// <param name="sysNo">优惠卡类型编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-09 朱成果 创建</remarks>
        public void DeleteCouponCardType(int sysNo)
        {
            ParaCouponCard filter = new ParaCouponCard();
            filter.CardTypeSysNo = sysNo;
            filter.Id = 1;
            filter.PageSize = 1;
            var lst = ISpCouponCardDao.Instance.GetCouponCard(filter);
            if (lst.TotalRows > 0)
            {
                throw new Exception("当前优惠卡类型已经在使用");
            }
            else
            {
                Hyt.DataAccess.Promotion.ISpCouponCardAssociateDao.Instance.DeleteByCardTypeSysNo(sysNo);
                Hyt.DataAccess.Promotion.ISpCouponCardTypeDao.Instance.Delete(sysNo);
            }
        }

        #endregion

        /// <summary>
        /// 退回优惠卡
        /// </summary>
        /// <param name="customerAccount">客户编号</param>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2014-01-21 朱家宏 创建</remarks>
        public bool CancelCouponCard(string customerAccount, string couponCardNo)
        {
            if (string.IsNullOrWhiteSpace(customerAccount) || string.IsNullOrWhiteSpace(couponCardNo))
                throw new HytException("参数不能为空");

            var customer = CRM.CrCustomerBo.Instance.GetCrCustomer(customerAccount);
            if (customer == null)
                throw new HytException("未找到到该客户");

            var couponCard = ISpCouponCardDao.Instance.Get(couponCardNo);
            if (couponCard == null)
                throw new HytException("未找到优惠卡");

            if (couponCard.Status == (int)PromotionStatus.优惠券卡状态.禁用)
                throw new HytException("优惠卡状态为禁用，不能退回");

            var logs = ISpCouponReceiveLogDao.Instance.GetAll(couponCardNo, customer.SysNo);
            if (logs == null || !logs.Any())
                throw new HytException("客户帐号与优惠卡卡号不匹配");

            foreach (var log in logs)
            {
                var coupon = ISpCouponDao.Instance.GetCoupon(log.CouponSysNo);
                if (coupon != null && coupon.CustomerSysNo == log.RecipientSysNo && coupon.UsedQuantity == 0)
                {
                    coupon.Status = (int)PromotionStatus.优惠券状态.作废;
                    ISpCouponDao.Instance.Update(coupon);
                }
            }

            //系统日志
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "退回优惠卡", LogStatus.系统日志目标类型.优惠卡, couponCard.SysNo,
                                 AdminAuthenticationBo.Instance.Current.Base.SysNo);

            return ISpCouponCardDao.Instance.UpdateCouponCardStatus(couponCard.SysNo, (int)PromotionStatus.优惠券卡状态.禁用) > 0;
        }
    }
}
