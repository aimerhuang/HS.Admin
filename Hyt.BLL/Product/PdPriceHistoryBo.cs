using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Product;
using Hyt.Model.Transfer;
using Hyt.BLL.Authentication;
using Hyt.Model.WorkflowStatus;
using System.Collections;

namespace Hyt.BLL.Product
{
    public class PdPriceHistoryBo : BOBase<PdPriceHistoryBo>
    {
        /// <summary>
        /// 保存调价申请
        /// </summary>
        /// <param name="priceHistory">调价申请对象</param>
        /// <returns>返回保存操作是否成功 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public bool SavePdPriceHistory(params PdPriceHistory[] priceHistory)
        {
            DateTime aployDate = DateTime.Now; //申请时间
            SyUser aployUser = AdminAuthenticationBo.Instance.GetAuthenticatedUser(); //获取当前登录用户
            IList<PdPriceHistory> expectPriceHistory; //避免数据的隐式转换，并附初值
            string groupCode = Guid.NewGuid().ToString("N"); //调价分组码

            ////初始化申请默认数据
            var query = from p in priceHistory
                        select new PdPriceHistory()
                            {
                                ApplyDate = aployDate
                                ,
                                ApplySysNo = aployUser.SysNo
                                ,
                                Status = (int) ProductStatus.产品价格变更状态.待审
                                ,
                                OriginalPrice = p.OriginalPrice
                                ,
                                ApplyPrice = p.ApplyPrice
                                ,
                                PriceSysNo = p.PriceSysNo,
                                RelationCode = groupCode
                            };

            expectPriceHistory = query.ToList();

            ////初始化申请默认数据
            //foreach (PdPriceHistory price in priceHistory)
            //{
            //    price.ApplyDate = aployDate;
            //    price.ApplySysNo = aployUser.SysNo;
            //    price.Status = (int)ProductStatus.产品价格变更状态.待审;
            //    price.AuditDate = new DateTime();
            //    price.Opinion = null;
            //}

            return IPdPriceHistoryDao.Instance.SavePdPriceHistory(expectPriceHistory.ToArray());
        }

        /// <summary>
        /// 保存多商品联合调价
        /// </summary>
        /// <param name="expectPrice">预期调价值</param>
        /// <param name="productSysNoList">商品系统编号列表</param>
        /// <returns>返回保存操作是否成功 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-06-28 邵斌 创建</remarks>
        public Result SavePdPriceHistories(IList<CBPdPriceHistory> expectPrice, int[] productSysNoList)
        {
            Result result = new Result();

            //预期要处理的商品调价申请单
            IList<PdPriceHistory> expectPriceHistory = new List<PdPriceHistory>();

            #region 通过商品系统编号读取商品价格并组合成待处理队列

            DateTime aployDate = DateTime.Now; //申请时间
            SyUser aployUser = AdminAuthenticationBo.Instance.GetAuthenticatedUser(); //获取当前登录用户
            //IList<PdPriceHistory> tempPdPriceHistory;
            IList<PdPrice> OriginalPriceList; //原价格列表
            string groupCode; //调价分组码

            //根据商品系统编号操作所以商品价格并更具要修改的结构进行计算新价格
            for (int i = 0; i < productSysNoList.Length; i++)
            {
                //读取商品原价格
                OriginalPriceList = IPdPriceDao.Instance.GetProductPrice(productSysNoList[i]);

                groupCode = Guid.NewGuid().ToString("N"); //生成关联关系码

                //将价格申请添加到结果集中
                foreach (PdPrice p in OriginalPriceList)
                {
                    //通过商品价格的来源和来源编号对应到要修改的价格列表中
                    var query = from ep in expectPrice
                                where
                                    ep.PriceSource.Equals(p.PriceSource) && ep.SourceSysNo.Equals(p.SourceSysNo)
                                //新统一调整的价格不能为0 0：表示价格没有变动
                                select
                                    new PdPriceHistory()
                                        {
                                            ApplyDate = aployDate
                                            ,
                                            ApplySysNo = aployUser.SysNo
                                            ,
                                            Status = (int) ProductStatus.产品价格变更状态.待审
                                            ,
                                            OriginalPrice = p.Price
                                            ,
                                            ApplyPrice = (p.Price + ep.ApplyPrice)
                                            ,
                                            PriceSysNo = p.SysNo,
                                            RelationCode = groupCode
                                        };

                    //判断是否有数据
                    if (query.Count() > 0)
                    {
                        var tempApplyPriceObject = query.FirstOrDefault();
                        if (tempApplyPriceObject.ApplyPrice < 0)
                        {
                          
                            //OriginalPriceList.GroupBy(x=>new {x.SourceSysNo,x.PriceSource,x.ProductSysNo});

                            PdPriceBo.Instance.DeleleRepeatPrice(p.ProductSysNo);//删除重复的价格
                            result.Status = false;
                            result.Message = "申请的商品["+IPdProductDao.Instance.GetProduct(p.ProductSysNo).ProductName+"]价格不得小于0，请重试！";
                            return result;
                        }
                        expectPriceHistory.Add(tempApplyPriceObject);
                    }
                }
            }

            #endregion

            result.Status = IPdPriceHistoryDao.Instance.SavePdPriceHistory(expectPriceHistory.ToArray());
            return result;
        }

        /// <summary>
        /// 获取指定商品的分销商等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="sourceSysNo">来源系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-09-17 周瑜 创建</remarks>
        public  IList<CBPdPrice> GetDealerProductLevelPrice(int productSysNo, int sourceSysNo)
        {
            return IPdPriceDao.Instance.GetDealerProductLevelPrice(productSysNo, sourceSysNo);
        }

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="status">审批状态</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="productName">商品名称</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        public PagedList<CBPdPriceHistory> GetPriceHistorieList(int? pageIndex, int status,int? erpCode, string productName = null)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBPdPriceHistory>();
            int count;
            var list = IPdPriceHistoryDao.Instance.GetPriceHistorieList((int)pageIndex, model.PageSize, status, erpCode,
                                                                        out count, productName);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            return model;
        }

        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-18 杨晗 创建</remarks>
        public IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode)
        {
            IList<CBPdPriceHistory> model = new List<CBPdPriceHistory>();

            var list = IPdPriceHistoryDao.Instance.GetPriceHistorieListByRelationCode(relationCode);
            if (list != null && list.Any())
            {
                model = list;
            }
            return model;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="relationCode">关系码</param>
        /// <param name="opinion">意见</param>
        /// <param name="status">状态</param>
        /// <param name="auditor">审批人</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Update(string relationCode, string opinion, int status, int auditor)
        {
            string msg;
            if (status == (int) ProductStatus.产品价格变更状态.已审)
            {
                msg = "审核了商品调价";
            }
            else
            {
                msg = "拒绝了商品调价";
            }
            int success= IPdPriceHistoryDao.Instance.Update(relationCode, opinion, status, auditor);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 msg, LogStatus.系统日志目标类型.商品调价申请, success, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);

            return success;
        }
    }
}
