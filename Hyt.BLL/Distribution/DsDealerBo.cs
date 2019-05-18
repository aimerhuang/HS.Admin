using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Memory;
using Hyt.Model.Generated;
using Grand.Platform.Api.Contract.DataContract;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商信息维护业务层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerBo : BOBase<DsDealerBo>
    {
        #region 操作
        /// <summary>
        /// 创建分销商,同时创建分销商预存款
        /// </summary>
        /// <param name="model">分销商 实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int Create(DsDealer model)
        {
            var sysNo = IDsDealerDao.Instance.Create(model);
            if (sysNo <= 0) return sysNo;//未成功直接返回
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商", LogStatus.系统日志目标类型.分销商, sysNo);
            var modelPre = new DsPrePayment //金额自动初始化为0
            {
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                DealerSysNo = sysNo,
                LastUpdateBy = model.LastUpdateBy,
                LastUpdateDate = model.LastUpdateDate
            };
            var preSysNo = DsPrePaymentBo.Instance.Create(modelPre);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商预存款", LogStatus.系统日志目标类型.分销商预存款, preSysNo);
            return sysNo;//成功则创建分销商预存款后 返回新加的系统编号

        }

        /// <summary>
        /// 修改分销商,同时修改关联账号状态,状态枚举相同
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int Update(DsDealer model)
        {
            var r = IDsDealerDao.Instance.Update(model);
            if (r > 0)
            {
                SyUserBo.Instance.UpdateSyUserStatus(model.UserSysNo, model.Status);
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商", LogStatus.系统日志目标类型.分销商, model.SysNo);
            }

            return r;
        }

        /// <summary>
        /// 分销商状态更新，同时修改关联账号状态,状态枚举相同
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="status">分销商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int UpdateStatus(int sysNo, DistributionStatus.分销商状态 status, int lastUpdateBy)
        {
            var r = IDsDealerDao.Instance.UpdateStatus(sysNo, status, lastUpdateBy);
            if (r > 0)
            {
                var userSysNo = GetDsDealer(sysNo).UserSysNo;
                var userStatus = status == DistributionStatus.分销商状态.启用 ? 1 : 0;
                SyUserBo.Instance.UpdateSyUserStatus(userSysNo, userStatus);
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商状态", LogStatus.系统日志目标类型.分销商, sysNo);
            }

            return r;
        }

        /// <summary>
        /// 分销商充值
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="amount">金额</param>
        /// <param name="syUser">操作者</param>
        /// <param name="remarks">备注</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        public void Prepaid(int sysNo, decimal amount, SyUser syUser, string remarks)
        {
            if (!CheckDealerStatus(sysNo)) throw new HytException("非法操作，经销禁用时不能充值!");
            var model = DsPrePaymentBo.Instance.GetDsPrePayment(sysNo);
            var prePaymentSysNo = 0;
            if (model != null)
            {
                //model.TotalPrestoreAmount += amount;
                //model.LastUpdateBy = syUser.SysNo;
                //model.LastUpdateDate = DateTime.Now;
                //IDsPrePaymentDao.Instance.Update(model);

                model.AvailableAmount += amount;
                IDsPrePaymentDao.Instance.AddTotalPrestoreAmount(sysNo, amount, syUser.SysNo);
                IDsPrePaymentDao.Instance.AddAvailableAmount(sysNo, amount, syUser.SysNo);
                prePaymentSysNo = model.SysNo;
            }
            else
            {
                model = new DsPrePayment()
                    {
                        DealerSysNo = sysNo,
                        TotalPrestoreAmount = amount,
                        AvailableAmount = amount
                    };
                model.CreatedBy = model.LastUpdateBy = syUser.SysNo;
                model.CreatedDate = model.LastUpdateDate = DateTime.Now;
                prePaymentSysNo = IDsPrePaymentDao.Instance.Insert(model);
            }
            var itemModel = new DsPrePaymentItem()
                {
                    PrePaymentSysNo = prePaymentSysNo,
                    Source = (int)DistributionStatus.预存款明细来源.预存款,
                    SourceSysNo = model.SysNo,
                    Increased = amount,
                    Decreased = decimal.Zero,
                    Surplus = model.AvailableAmount,
                    Status = (int)DistributionStatus.预存款明细状态.完结,
                    Remarks = remarks
                };
            itemModel.CreatedBy = itemModel.LastUpdateBy = syUser.SysNo;
            itemModel.CreatedDate = itemModel.LastUpdateDate = DateTime.Now;
            IDsPrePaymentItemDao.Instance.Insert(itemModel);
        }

        /// <summary>
        /// 分销商提现
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="amount">金额</param>
        /// <param name="syUser">操作者</param>
        /// <param name="remarks">备注</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        public int Withdraw(int sysNo, decimal amount, SyUser syUser, string remarks)
        {
            int ItemSysNo = 0;
            if (!CheckDealerStatus(sysNo)) throw new HytException("非法操作，经销禁用时不能提现!");
            var model = DsPrePaymentBo.Instance.GetDsPrePayment(sysNo);
            if (model == null)
            {
                throw new HytException("未找到分销商充值记录!");
            }
            else
            {
                if (model.AvailableAmount >= amount)
                {
                    //model.LastUpdateBy = syUser.SysNo;
                    //model.LastUpdateDate = DateTime.Now;
                    //DsPrePaymentBo.Instance.Update(model);

                    IDsPrePaymentDao.Instance.SubtractAvailableAmount(sysNo, amount, syUser.SysNo);
                    model.AvailableAmount -= amount;
                    var itemModel = new DsPrePaymentItem()
                        {
                            PrePaymentSysNo = model.SysNo,
                            Source = (int)DistributionStatus.预存款明细来源.提现,
                            SourceSysNo = model.SysNo,
                            Increased = decimal.Zero,
                            Decreased = amount,
                            Surplus = model.AvailableAmount,
                            Status = (int)DistributionStatus.预存款明细状态.冻结,
                            Remarks = "分销返利",
                        };
                    itemModel.CreatedBy = itemModel.LastUpdateBy = syUser.SysNo;
                    itemModel.CreatedDate = itemModel.LastUpdateDate = DateTime.Now;
                    ItemSysNo = IDsPrePaymentItemDao.Instance.Insert(itemModel);
                }
                else
                {
                    throw new HytException("提取金额超过预存款可用余额!");
                }
            }
            return ItemSysNo;
        }

        /// <summary>
        /// 检查分销商是否可以冲值或提现
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>结果</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        private bool CheckDealerStatus(int sysNo)
        {
            var model = IDsDealerDao.Instance.GetDsDealer(sysNo);
            return model != null && model.Status == (int)DistributionStatus.分销商状态.启用;
        }

        #endregion

        #region 查询
        /// <summary>
        /// 检查分销商erp代码
        /// </summary>
        /// <param name="erpCode"></param>
        /// <returns></returns>
        /// <remarks>2017-09-22 杨浩 创建</remarks>
        public bool CheckErpCode(string erpCode)
        {
            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IErpService>())
            {
                var requset=new  IsTItemFNumberRequest()
                {
                    FNumber=erpCode
                };

                var response=service.Channel.IsExistTItemFNumber(requset);
                return response.IsExist;
            }
        }
        /// <summary>
        /// 根据企业编号获取加盟商信息
        /// </summary>
        /// <param name="enterpriseID">企业编号</param>
        /// <returns>经销商信息</returns>
        /// <remarks>2015-01-29 杨浩 创建</remarks>
        public DsDealer GetDealerByEnterpriseID(int enterpriseID)
        {
            return IDsDealerDao.Instance.GetDealerByEnterpriseID(enterpriseID);
        }
        /// <summary>
        /// 通过过滤条件获取分销商列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商列表</returns>
        ///<remarks>2013-09-03 周唐炬 创建</remarks>
        public PagedList<CBDsDealer> GetDealerList(ParaDealerFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<CBDsDealer>();
                filter.PageSize = model.PageSize;
                var pager = IDsDealerDao.Instance.GetDealerList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }

        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public CBDsDealer GetDsDealer(int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealer(sysNo);
        }
        /// <summary>
        /// 根据订单编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-05-19 罗勤尧 创建
        /// </remarks>
        public DsDealerLiJiaEdit GetCBDsDealerByOrderId(int sysNo)
        {
            return IDsDealerDao.Instance.GetCBDsDealerByOrderId(sysNo);
        }
        /// <summary>
        /// 根据订单编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-05-19 罗勤尧 创建
        /// </remarks>
        public DsDealer GetDsDealerByOrderSysNo(int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerByOrderSysNo(sysNo);
        }
        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public DsDealerLiJiaEdit GetDsDealerLiJia(int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerLiJia(sysNo);
        }
        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public CBDsDealer GetCBDsDealer(int sysNo)
        {
            return IDsDealerDao.Instance.GetCBDsDealer(sysNo);
        }
        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public void GetDsDealerList(ref Pager<CBDsDealer> pager, ParaDsDealerFilter filter)
        {
            IDsDealerDao.Instance.GetDsDealerList(ref pager, filter);
        }

        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>   
        public IList<CBDsDealer> GetDsDealerList(ParaDsDealerFilter filter)
        {
            return IDsDealerDao.Instance.GetDsDealerList(filter);
        }

        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>分销商信息列表，0或1条记录正常</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 可用于重复性检查 一个系统用户编号不能用于多个分销商
        /// </remarks>      
        public IList<CBDsDealer> GetDsDealerList(int userSysNo)
        {
            var filter = new ParaDsDealerFilter { UserSysNo = userSysNo };
            return GetDsDealerList(filter);
        }

        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public IList<CBDsDealer> GetDsDealerList()
        {
            return IDsDealerDao.Instance.GetDsDealerList();
        }

        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2015-12-31 王耀发 创建 
        /// </remarks>      
        public IList<DsDealer> GetDsDealersList()
        {
            return IDsDealerDao.Instance.GetDsDealersList();
        }
        /// <summary>
        /// 获得当前用户有权限看到的分销商
        /// 2016-1-4 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<DsDealer> GetDealersListByCurUser(ParaDsDealerFilter filter)
        {
            return IDsDealerDao.Instance.GetDealersListByCurUser(filter);
        }

        /// <summary>
        /// 获得创建用户对应的分销商
        /// 2016-1-29 王耀发 创建
        /// </summary>
        /// <param name="DealerCreatedBy"></param>
        /// <param name="Type">当前登录账号类型 F：为分销商</param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        public IList<DsDealer> GetDealersListByCreatedBy(int DealerCreatedBy, string Type, int TypeSysNo)
        {
            return IDsDealerDao.Instance.GetDealersListByCreatedBy(DealerCreatedBy,Type, TypeSysNo);
        }

        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="dealerName">分销商名称</param>
        /// <returns>分销商信息列表，0或1条记录正常</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 可用于重复性检查 分销商名称唯一
        /// </remarks>      
        public IList<CBDsDealer> GetDsDealerList(string dealerName)
        {
            var filter = new ParaDsDealerFilter { DealerName = dealerName };
            return GetDsDealerList(filter);
        }
        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="AppID">AppID</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2015-12-22 王耀发 创建 可用于重复性检查 分销商名称唯一
        /// </remarks>   
        public IList<CBDsDealer> GetDsDealerListByAppID(string AppID)
        {
            var filter = new ParaDsDealerFilter { AppID = AppID };
            return GetDsDealerList(filter);
        }
        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2015-12-22 王耀发 创建 可用于重复性检查 分销商名称唯一
        public IList<CBDsDealer> GetDsDealerListByAppSecret(string AppSecret)
        {
            var filter = new ParaDsDealerFilter { AppSecret = AppSecret };
            return GetDsDealerList(filter);
        }
        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="WeiXinNum">微信公众好</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建 可用于重复性检查 分销商名称唯一
        public IList<CBDsDealer> GetDsDealerListByWeiXinNum(string WeiXinNum)
        {
            var filter = new ParaDsDealerFilter { WeiXinNum = WeiXinNum };
            return GetDsDealerList(filter);
        }
        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="DomainName">域名</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建 可用于重复性检查 分销商名称唯一
        public IList<CBDsDealer> GetDsDealerListByDomainName(string DomainName)
        {
            var filter = new ParaDsDealerFilter { DomainName = DomainName };
            return GetDsDealerList(filter);
        }

        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询分销商信息
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的分销商系统编号</param>
        /// <returns>分销商信息列表，0条记录可更新</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建 
        /// </remarks>   
        public IList<CBDsDealer> GetDsDealerList(int userSysNo, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerList(userSysNo, sysNo);
        }

        public IList<CBDsDealer> GetDsDealerListByAppID(string AppID, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerListByAppID(AppID, sysNo);
        }

        public IList<CBDsDealer> GetDsDealerListByAppSecret(string AppSecret, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerListByAppSecret(AppSecret, sysNo);
        }

        public IList<CBDsDealer> GetDsDealerListByWeiXinNum(string WeiXinNum, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerListByWeiXinNum(WeiXinNum, sysNo);
        }

        public IList<CBDsDealer> GetDsDealerListByDomainName(string DomainName, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerListByDomainName(DomainName, sysNo);
        }

        public IList<CBDsDealer> GetDsDealerListByDealerName(string DealerName, int sysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerListByDealerName(DealerName, sysNo);
        }

        /// <summary>
        /// 根据系统用户编号获取分销商信息
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-09 余勇 创建
        /// </remarks>
        /// 
        public CBDsDealer GetDsDealerByUserNo(int userSysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerByUserNo(userSysNo);
        }

        /// <summary>
        /// 根据分销商用户编号获取分销商信息
        /// </summary>
        /// <param name="dsUserSysNo">分销商用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public DsDealer GetDsDealerByDsUser(int dsUserSysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerByDsUser(dsUserSysNo);
        }

        /// <summary>
        /// 根据仓库编号获取分销商信息
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public  DsDealer GetDsDealerByWarehousSysNo(int warehousSysNo)
        {
            return IDsDealerDao.Instance.GetDsDealerByWarehousSysNo(warehousSysNo);
        }
        /// <summary>
        /// 根据名称获取分销商
        /// </summary>
        /// <param name="DealerName"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-06-09 王耀发 创建
        /// </remarks> 
        public DsDealer GetDsDealerByName(string DealerName)
        {
            return IDsDealerDao.Instance.GetDsDealerByName(DealerName);
        }

        /// <summary>
        /// 获取所有分销商信息
        /// </summary>
        /// <returns>分销商数据列表</returns>
        /// 2015-09-19 王耀发 创建
        public IList<DsDealer> GetAllDealerList()
        {
            var list = MemoryProvider.Default.Get(KeyConstant.DealerList, () => IDsDealerDao.Instance.GetAllDealerList());
            return list;
        }

        /// <summary>
        /// 获取用户有可管理的所有分销商
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>分销商集合</returns>
        /// <remarks>
        /// 2015-09-19 王耀发 创建
        /// </remarks>
        public IList<DsDealer> GetUserDealerList(int userSysNo)
        {
            return IDsDealerDao.Instance.GetUserDealerList(userSysNo);
        }
        #endregion

        /// <summary>
        /// 获得当前经销商树形图
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// 2016-1-20 王耀发 创建
        public IList<CBDsDealer> GetDealerTreeList(string Type, int TypeSysNo)
        {
            return IDsDealerDao.Instance.GetDealerTreeList(Type, TypeSysNo);
        }
        /// <summary>
        /// 获得代理商列表
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        /// 2016-1-29 王耀发 创建
        public IList<CBDsDealer> GetDaiLiShangList(string Type, int TypeSysNo)
        {
            return IDsDealerDao.Instance.GetDaiLiShangList(Type, TypeSysNo);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏 创建</returns>
        public IList<DsApplyStore> List(int State, PagedList<DsApplyStore> pager)
        {

            Pager<DsApplyStore> transPager = new Pager<DsApplyStore>();
            transPager.CurrentPage = pager.CurrentPageIndex;
            transPager.PageSize = pager.PageSize;
            IDsDealerDao.Instance.List(State, transPager);
            pager.TData = transPager.Rows;
            pager.TotalItemCount = transPager.TotalRows;
            return pager.TData;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public int Update(string sysNoItems)
        {
            return IDsDealerDao.Instance.Update(sysNoItems);
        }

        /// <summary>
        /// 更新微信AppID、AppSecret
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2016-05-10 王耀发 创建
        /// </remarks>
        public int UpdateAppIDandSecret(int SysNo, string AppID, string AppSecret)
        {
            return IDsDealerDao.Instance.UpdateAppIDandSecret(SysNo, AppID, AppSecret);
        }
        public int UpdateLiJiaSysNo(int LiJiaSysNo, int SysNo)
        {
            return IDsDealerDao.Instance.UpdateLiJiaSysNo(LiJiaSysNo, SysNo);
        }

        /// <summary>
        /// 根据分销商获取店铺
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public IList<WhWarehouse> WhWarehouseList(int DealerSysNo)
        {
            return IDsDealerDao.Instance.WhWarehouseList(DealerSysNo);
        }
    }
}
