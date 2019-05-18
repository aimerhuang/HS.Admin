using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Memory;
using System.Transactions;
using Hyt.Model.ExpressList;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 配送方式业务类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class DeliveryTypeBo : BOBase<DeliveryTypeBo>
    {
        #region 操作

        /// <summary>
        /// 创建配送方式
        /// </summary>
        /// <param name="model">配送方式实体</param>
        /// <returns>创建的配送方式sysNo</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public int Create(LgDeliveryType model)
        {
            if (IsExistDeliveryType(model.DeliveryTypeName))
                return 0;
            var r = ILgDeliveryTypeDao.Instance.Create(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建配送方式", LogStatus.系统日志目标类型.配送方式, r);
            return r;
        }

        /// <summary>
        /// 更新配送方式
        /// </summary>
        /// <param name="model">配送方式实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// 2014-5-13 杨文兵 增加删除配送方式缓存代码
        /// </remarks>
        public bool Update(LgDeliveryType model)
        {
            var r = ILgDeliveryTypeDao.Instance.Update(model) > 0;
            if (r)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改配送方式", LogStatus.系统日志目标类型.配送方式, model.SysNo);
                var chackeKey = string.Format("CACHE_DELIVERYTYPE_{0}", model.SysNo);
                MemoryProvider.Default.Remove(chackeKey);
            }
            return r;
        }

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="sysNo">要删除的配送方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public bool Delete(int sysNo)
        {
            if (IsHaveSubDeliveryType(sysNo))
                return false;
            var r = ILgDeliveryTypeDao.Instance.Delete(sysNo) > 0;
            if (r)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除配送方式", LogStatus.系统日志目标类型.配送方式, sysNo);
            return r;
        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager)
        {
            ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeList(ref pager);
        }

        /// <summary>
        /// 查询配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager, ParaDeliveryTypeFilter filter)
        {
            ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeList(ref pager, filter);
        }
        public string GetorderId(int sysno)
        {
            return ILgDeliveryTypeDao.Instance.GetorderId(sysno);
        }
        /// <summary>
        /// 根据系统编号获取配送方式
        /// </summary>
        /// <param name="sysNo">配送方式系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// 2014-5-13 杨文兵 将配送方式数据加入缓存
        /// </remarks>
        public CBLgDeliveryType GetDeliveryType(int sysNo)
        {
            //note:缓存的是CB前缀对象
            var chackeKey = string.Format("CACHE_DELIVERYTYPE_{0}",sysNo);

            return MemoryProvider.Default.Get<CBLgDeliveryType>(chackeKey, 60 * 24, () => {
                return ILgDeliveryTypeDao.Instance.GetLgDeliveryType(sysNo);    
            }, CachePolicy.Absolute);
            
        }
        /// <summary>
        /// 批量创建配送单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliverTypeSysno">配送类型系统编号</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteSysNo">单据类型编号</param>    
        /// <param name="expressNo">快递单号</param>   
        /// <returns></returns>
        /// <remarks>2017-10-24 杨浩 创建</remarks>
        public Result<int> BatchCreateLgDelivery(int warehouseSysNo, int deliverTypeSysno, int noteType, int noteSysNo, string expressNo)
        {
            var result = new Result<int>();
            var itemList = new List<LgDeliveryItem>();
            string needInStock = string.Empty;

            if (string.IsNullOrWhiteSpace(expressNo))
            {
                result.Status = false;
                result.Message = "配送单明细数据错误,不能创建配送单";
                return result;
            }

            #region 判断快递单号是否重复
            if (!string.IsNullOrEmpty(expressNo) && noteType == (int)LogisticsStatus.配送单据类型.出库单)
            {
                var flg = Hyt.BLL.Logistics.LgDeliveryBo.Instance.IsExistsExpressNo(deliverTypeSysno, expressNo);
                if (flg)
                {
                    result.Status = false;
                    result.Message = "快递单号" + expressNo + "已经被使用，请更换快递单号";
                    return result;
                }
            }
            #endregion

            #region 配送单作废会生成出库单对应的入库单，再次将此入库单加入配送,需检查此入库单是否已经完成入库

            if (noteType == (int)LogisticsStatus.配送单据类型.出库单)
            {
                var rr = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CheckInStock(noteSysNo);
                if (rr.Status)
                {
                    if (!string.IsNullOrEmpty(needInStock))
                    {
                        needInStock += ",";
                    }
                    needInStock += rr.StatusCode;
                }
            }

            #endregion

            itemList.Add(new LgDeliveryItem()
            {
                DeliverySysNo = deliverTypeSysno,
                ExpressNo = expressNo,
                NoteType = noteType,
                NoteSysNo = noteSysNo
            });

            if (!string.IsNullOrEmpty(needInStock))//未入库的单子
            {
                result.Status = false;
                result.Message = "请将先前配送单作废，拒收，未送达生成的入库单(" + needInStock + ")完成入库";
                return result;
            }

            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            //配送方式  
            var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysno);
            int deliverySysno;
            var deliveryMsgs = new List<Hyt.BLL.Logistics.LgDeliveryBo.DeliveryMsg>();

            var currentUser=BLL.Authentication.AdminAuthenticationBo.Instance.Current;

            using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
            {
                deliverySysno = LgDeliveryBo.Instance.NewCreateLgDelivery(warehouseSysNo, -1,
                    delivertType,
                    (currentUser == null ? 0 : currentUser.Base.SysNo), itemList, true, ref deliveryMsgs, Hyt.Util.WebUtil.GetUserIp());


                //回填物流信息
                try
                {
                    LgDeliveryBo.Instance.BackFillLogisticsInfo(deliverySysno, deliverTypeSysno);
                }
                catch (Exception ex)
                {
                    //Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                }


                result.Status = true;
                result.Data = deliverySysno;
                result.Message = "确认发货完成";
                tran.Complete();
            }

            return result;
        }

        /// <summary>
        /// 根据系统编号获取快递单号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2017-08-15 吴琨 创建
        /// </remarks>
        public  string GetExpressNo(int sysNo)
        {
            return ILgDeliveryTypeDao.Instance.GetExpressNo(sysNo);
        }

        /// <summary>
        /// 根据系统编号获取快递单号，时间，配送方式名称
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2018-1-10 廖移凤 创建
        /// </remarks>
        public KuaiDi GetKuaidi(int sysNo)
        {
            return ILgDeliveryTypeDao.Instance.GetKuaidi(sysNo);
        }

        /// <summary>
        /// 根据系统编号获取配送方式名称
        /// </summary>
        /// <param name="sysNo">配送方式系统编号</param>
        /// <returns>返回配送方式名称</returns>
        /// <returns>2014-02-13 沈强 创建</returns>
        public string GetDeliveryTypeName(int sysNo)
        {
            var deliveryType = GetDeliveryType(sysNo);
            if (deliveryType!=null)
            {
                return deliveryType.DeliveryTypeName;
            }
            else
            {
                return "没有编号为：" + sysNo + "的配送名称！";
            }
        }

        /// <summary>
        /// 获取子配送方式
        /// </summary>
        /// <param name="sysNo">配送方式系统编号，为0时获取第一级配送方式</param>
        /// <returns>配送方式列表</returns>
        /// <remarks> 
        /// 2013-06-27 郑荣华 创建
        /// </remarks>
        public IList<LgDeliveryType> GetSubLgDeliveryTypeList(int sysNo)
        {
            return ILgDeliveryTypeDao.Instance.GetSubLgDeliveryTypeList(sysNo);
        }

        /// <summary>
        /// 根据仓库编号获取配送方式信息
        /// </summary>
        /// <param name="wareshouSysNo">仓库系统编号</param>
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-06-28 周瑜 创建
        /// </remarks>
        public IList<LgDeliveryType> GetLgDeliveryTypeByWarehouse(int wareshouSysNo)
        {
            var lgDelivery =
                ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeByWarehouse(wareshouSysNo)
                                  .Where(l => l.ParentSysNo != 0)
                                  .ToList();
            foreach (var lgDeliveryType in lgDelivery)
            {
                var delivery = DeliveryTypeBo.Instance.GetDeliveryType(lgDeliveryType.ParentSysNo);
                lgDeliveryType.DeliveryTypeName = delivery.DeliveryTypeName + "_" + lgDeliveryType.DeliveryTypeName;
            }
            return lgDelivery;
        }

        /// <summary>
        /// 查询所有的配送方式
        /// </summary>     
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-08-08 周瑜 创建
        /// </remarks>
        public IList<LgDeliveryType> GetLgDeliveryTypeList()
        {
            return ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeList();
        }

        /// <summary>
        /// 查询所有的配送方式
        /// </summary>     
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-08-08 周瑜 创建
        /// </remarks>
        public IList<LgDeliveryType> GetDeliveryTypeList()
        {
            var lgdelivery = ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeList()
                .Where(l => l.ParentSysNo != 0)
                                  .ToList();
            foreach (var lgDeliveryType in lgdelivery)
            {
                var delivery = DeliveryTypeBo.Instance.GetDeliveryType(lgDeliveryType.ParentSysNo);
                lgDeliveryType.DeliveryTypeName = delivery.DeliveryTypeName + "_" + lgDeliveryType.DeliveryTypeName;
            }
            return lgdelivery.OrderBy(o=>o.DeliveryTypeName).ToList();
        }

        /// <summary>
        /// 查询所有的父级配送方式
        /// </summary>     
        /// <returns>父级配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-09-18 黄伟 创建
        /// </remarks>
        public IList<LgDeliveryType> GetLgDeliveryTypeParent()
        {
            return ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeParent();
        }

        #endregion

        #region 判断

        /// <summary>
        /// 是否已使用配送方式
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>使用true,未使用false</returns>
        /// <remarks> 
        /// 2013-06-26 郑荣华 创建
        /// </remarks>
        public bool IsUsed(int deliveryTypeSysNo)
        {
            return ILgDeliveryTypeDao.Instance.GetWhWarehouseDeliveryType(deliveryTypeSysNo).Count > 0;
        }

        /// <summary>
        /// 判断是否已有此配送方式名称
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <returns>存在返回true,不存在返回false</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public bool IsExistDeliveryType(string deliveryTypeName)
        {
            return ILgDeliveryTypeDao.Instance.GetLgDeliveryType(deliveryTypeName) != null;
        }

        /// <summary>
        /// 除去修改中的配送方式外，是否还存在相同配送方式名称
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <param name="sysNo">正在修改的配送方式系统编号</param>
        /// <returns>存在返回true,不存在返回false</returns>
        /// <remarks> 
        /// 2013-07-02 郑荣华 创建
        /// </remarks>
        public bool IsExistDeliveryType(string deliveryTypeName, int sysNo)
        {
            return ILgDeliveryTypeDao.Instance.GetLgDeliveryTypeForUpdate(deliveryTypeName, sysNo) != null;
        }
        /// <summary>
        /// 是否有子配送方式
        /// </summary>
        /// <param name="sysNo">配送方式系统编号</param>
        /// <returns>有返回true,没有返回false</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        private bool IsHaveSubDeliveryType(int sysNo)
        {
            return GetSubLgDeliveryTypeList(sysNo).Count > 0;
        }

        /// <summary>
        /// 是否可以删除
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>可以返回true,不可以返回false</returns>
        /// <remarks> 
        /// 2013-07-02 郑荣华 创建
        /// </remarks>
        public bool IsCanDelete(int sysNo)
        {
            var isUsed = IsUsed(sysNo);
            var isHaveSub = IsHaveSubDeliveryType(sysNo);
            if (!isUsed && !isHaveSub)
                return true;
            return false;
        }

        #endregion

        public LgDeliveryType GetDeliveryTypeByCode(string typeCode)
        {
            return ILgDeliveryTypeDao.Instance.GetDeliveryTypeByCode(typeCode);
        }
    }
}
