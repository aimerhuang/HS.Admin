using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using Extra.Erp;
using Extra.Erp.Model.Borrowing;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.Product;
using Hyt.BLL.Sys;
using Hyt.DataAccess.Logistics;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 借货单维护Bo
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public class ProductLendBo : BOBase<ProductLendBo>, IProductLendBo
    {
        /// <summary>
        /// 商品还货
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="model">入库单实体</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public Result ProductReturn(int deliveryUserSysNo, WhStockIn model)
        {
            var result = new Result() { StatusCode = -1 };
            if (null != model.ItemList && model.ItemList.Count > 0)
            {
                var itemList = new List<WhStockInItem>();
                model.ItemList.ForEach(itemList.Add);//克隆坨新的出来
                //借货单列表
                var productLendList = new List<WhProductLend>();

                #region 商品还货
                //遍历所有还货商品
                itemList.ForEach(item =>
                    {
                        //还货数量小于1跳过该商品
                        if (item.RealStockInQuantity <= 0) return;
                        //该商品本次还货数量
                        var returnNum = item.RealStockInQuantity;
                        //找到配送人员指定商品借货列表，按借货单SysNo最小升序,时间最早还货
                        var productLengItemList =
                            IProductLendDao.Instance.GetWhProductLendItemList(new ParaWhProductLendItemFilter()
                            {
                                DeliveryUserSysNo = deliveryUserSysNo,
                                ProductSysNo = item.ProductSysNo,
                                Status = WarehouseStatus.借货单状态.已出库.GetHashCode()
                            });

                        if (null == productLengItemList) return;
                        foreach (var sysno in productLengItemList)
                        {
                            //检查本次还货数量是否大于0,小于等于0项目直接跳过
                            if (returnNum <= 0) break;
                            //重新获取到包含信用等级价格的借货明细
                            var modelItem = IProductLendDao.Instance.GetWhProductLendItemInfo(new ParaWhProductLendItemFilter()
                            {
                                SysNo = sysno,
                                PriceSource = ProductStatus.产品价格来源.配送员进货价.GetHashCode()
                            });
                            if (null == modelItem) continue;

                            //该商品总金额
                            var price = decimal.Zero;
                            //本次应还货数量  
                            //应还货数量＝借货数数量-已销售数量－已还货数量
                            var returnQuantity = modelItem.LendQuantity - modelItem.SaleQuantity - modelItem.ReturnQuantity;
                            int currectReturnQuantity = 0;//本次还货数量 2014-04-08 朱成果
                            if (returnNum >= returnQuantity) //本次还货数量大于应还货数量
                            {
                                //入库数量等于计算出的应还货数量,加上历史还货数据
                                modelItem.ReturnQuantity += returnQuantity;
                                currectReturnQuantity = returnQuantity;//记录本次还货数量 2014-04-08 朱成果
                                //从本次还货数中减去该商品应还货数量
                                returnNum = returnNum - returnQuantity;
                                //还货信用价格
                                price += returnQuantity * modelItem.Price;
                            }
                            else//本次还货数量小于应还货数量
                            {
                                modelItem.ReturnQuantity += returnNum;
                                currectReturnQuantity = returnNum;//记录本次还货数量 2014-04-08 朱成果
                                //还货信用价格
                                price += returnNum * modelItem.Price;
                                //还货数量已用完，置位0
                                returnNum = 0;
                            }
                            //更新还货信息,借货单
                            modelItem.LastUpdateDate = DateTime.Now;
                            modelItem.LastUpdateBy = model.LastUpdateBy;
                            var id = IProductLendDao.Instance.UpdateWhProductLendItem(modelItem);//更新数据库
                            if (id <= 0) continue;

                            modelItem.Remarks = currectReturnQuantity.ToString();//将本次还货数量以备注信息传入下一个方法CreateInStock 2014-04-08 朱成果
                            //借货单是否在借货列表中
                            var productLend = productLendList.SingleOrDefault(x => x.SysNo == modelItem.ProductLendSysNo);
                            if (null != productLend)//已经存在，添加借货明细
                            {
                                productLend.ItemList.Add(modelItem);
                            }
                            else//不存在，添加借货单跟明细
                            {
                                productLendList.Add(new WhProductLend()
                                {
                                    SysNo = modelItem.ProductLendSysNo,
                                    ItemList = new List<WhProductLendItem> { modelItem }
                                });
                            }
                            //更新配送员信用
                            DeliveryUserCreditBo.Instance.UpdateRemaining(model.WarehouseSysNo, deliveryUserSysNo, 0, price, "商品还货，借货单号：" + sysno);
                        }
                    });
                #endregion

                #region 商品入库，借货单完结
                if (productLendList.Any())
                {
                    productLendList.ForEach(productLend =>
                        {
                            #region 完结借货单

                            CompleteProductLend(productLend.SysNo, model.LastUpdateBy);

                            #endregion

                            #region 商品入库

                            CreateInStock(model, productLend);

                            #endregion
                        });
                }
                #endregion

                result.StatusCode = 0;
                result.Status = true;
            }
            else
            {
                result.Message = "还货商品不能为空!";
            }
            return result;
        }

        /// <summary>
        ///创建一个以已经入库的入库单
        /// </summary>
        /// <param name="model">入库实体</param>
        /// <param name="productLend">借货单 明细备注信息为对应的当前还货数量</param>
        /// <returns></returns>
        /// <remarks>2013-07-23 周唐炬 创建</remarks>
        private void CreateInStock(WhStockIn model, WhProductLend productLend)
        {
            if (null == productLend.ItemList || productLend.ItemList.Count <= 0) return;
            //来源编号为借货单编号
            model.SourceSysNO = productLend.SysNo;
            model.Status = (int)WarehouseStatus.入库单状态.已入库;
            model.SourceType = (int)WarehouseStatus.入库单据类型.借货单;
            model.IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否;
            model.DeliveryType = (int)WarehouseStatus.入库物流方式.还货;
            model.CreatedDate = model.LastUpdateDate = DateTime.Now;
            model.ItemList = new List<WhStockInItem>();
            foreach (var item in productLend.ItemList)
            {
                 model.ItemList.Add(new WhStockInItem
                 {
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = model.LastUpdateBy,
                        LastUpdateDate = DateTime.Now,
                        ProductName = item.ProductName,
                        ProductSysNo = item.ProductSysNo,
                        SourceItemSysNo = item.SysNo,//记录入库单明细来源单号（借货单明细编号)
                        StockInQuantity = int.Parse(item.Remarks), //备注信息是当前还货数量，是一个中转参数 2014-04-08 朱成果
                        RealStockInQuantity = int.Parse(item.Remarks)//备注信息是当前还货数量，是一个中转参数 2014-04-08 朱成果
                         
                   });
             }
            InStockBo.Instance.CreateStockIn(model);
        }

        /// <summary>
        /// 配送员借货额度结算
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="settlementPrice">借货金额</param>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>/ 
        public void DeliveryCreditCalculate(int deliveryUserSysNo, int warehouseSysNo, decimal settlementPrice, int sysno)
        {
            var model = ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo, warehouseSysNo);
            if (null == model) return;
            if (settlementPrice > model.RemainingBorrowingCredit)
            {
                if (!AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.WH0012))
                {
                    throw new HytException("已选商品总额超过配送员当前借货可用额度，权限不足非法操作！");
                }
            }
            DeliveryUserCreditBo.Instance.UpdateRemaining(warehouseSysNo, deliveryUserSysNo, 0, -settlementPrice, "创建借货单，单号：" + sysno);
            //model.RemainingBorrowingCredit -= settlementPrice;
            //ILgDeliveryUserCreditDao.Instance.Update(model);
        }

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public PagedList<WhProductLend> GetProductLendList(ParaProductLendFilter filter)
        {
            PagedList<WhProductLend> pageList = null;
            if (null != filter)
            {
                pageList = new PagedList<WhProductLend>();
                var pager = IProductLendDao.Instance.GetWhProductLendList(filter);
                if (pager != null)
                {
                    pageList.TData = pager.Rows;
                    pageList.TotalItemCount = pager.TotalRows;
                    pageList.CurrentPageIndex = filter.CurrentPage;
                    pageList.PageSize = filter.PageSize;
                }
            }
            return pageList;
        }

        /// <summary>
        /// 业务员库存查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>业务员库存</returns>
        /// <remarks>2013-12-11 周唐炬 创建</remarks>
        public PagedList<WhProductLendItem> GetInventoryProductList(ParaProductLendFilter filter)
        {
            PagedList<WhProductLendItem> pageList = null;
            if (null != filter)
            {
                if (!filter.WarehouseSysNo.HasValue)
                {
                    throw new HytException("必须指定仓库!");
                }
                if (!filter.DeliveryUserSysNo.HasValue)
                {
                    throw new HytException("必须指定业务员!");
                }
                filter.Status = WarehouseStatus.借货单状态.已出库.GetHashCode();
                pageList = new PagedList<WhProductLendItem>();
                filter.PageSize = pageList.PageSize;

                var pager = IProductLendDao.Instance.GetInventoryProductList(filter);
                if (pager != null)
                {
                    pageList.TData = pager.Rows;
                    pageList.TotalItemCount = pager.TotalRows;
                    pageList.CurrentPageIndex = filter.CurrentPage;
                }
            }
            return pageList;
        }

        /// <summary>
        /// 检查配送员在该仓库下是否有未完结借货单数量
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="messages">返回消息</param>
        /// <returns>结果</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        public bool CheckWhProductLendWarehouse(int deliveryUserSysNo, int warehouseSysNo, ref string messages)
        {
            var list = IProductLendDao.Instance.GetWhProductLendWarehouseList(deliveryUserSysNo);
            var result = list.Any(x => x != warehouseSysNo);
            if (result)
            {
                var whlist = list.Where(x => x != warehouseSysNo);
                var msgs = (from item in whlist
                            select WhWarehouseBo.Instance.GetWarehouse(item)
                                into wh
                                where wh != null
                                select wh.WarehouseName
                                ).ToList();
                messages = string.Format("配送员在{0}借货，请先还货完结！", string.Join("，", msgs.Distinct().ToArray()));
            }
            return result;
        }

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public List<CBWhProductLend> WhProductLendExportExcel(ParaProductLendFilter filter)
        {
            return IProductLendDao.Instance.WhProductLendExportExcel(filter);
        }

        /// <summary>
        /// 通过借货单编号获取借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>借货单</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public WhProductLend GetWhProductLend(int sysNo)
        {
            try
            {
                return IProductLendDao.Instance.GetWhProductLend(sysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.借货单, sysNo, ex);
            }
            return null;
        }

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public int CreateWhProductLend(WhProductLend model)
        {
            var price = decimal.Zero;
            var id = IProductLendDao.Instance.CreateWhProductLend(model);
            if (id <= 0) throw new HytException("创建借货单异常！");
            CreateProductLendItem(id, model.DeliveryUserSysNo, model.CreatedBy, model.ItemList, ref price);
            //检查前台传入价格是否跟后台计算数据一致
            if (model.Amount != price)
            {
                model.Amount = price;
                UpdateWhProductLend(model);
            }
            //配送员借货额度结算
            DeliveryCreditCalculate(model.DeliveryUserSysNo, model.WarehouseSysNo, model.Amount, id);

            var borrowInfos = (from item in model.ItemList
                               let product = PdProductBo.Instance.GetProduct(item.ProductSysNo)//商品
                               let warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo)//仓库
                               where product != null && warehouse != null
                               select new BorrowInfo()
                                   {
                                       ErpCode = product.ErpCode,
                                       Quantity = item.LendQuantity,
                                       WarehouseNumber = warehouse.ErpCode,
                                       Amount = model.Amount,
                                       Remark = item.Remarks,
                                       WarehouseSysNo = model.WarehouseSysNo
                                   }).ToList();

            var syUser = SyUserBo.Instance.GetSyUser(model.DeliveryUserSysNo);
            var client = EasProviderFactory.CreateProvider();

            //销售出库接口的摘要格式：JC[Hyt借货单系统编号]-[借货员姓名]
            client.Borrow(borrowInfos, string.Format("JC[{0}]-[{1}]", id, syUser != null ? syUser.UserName : model.DeliveryUserSysNo.ToString()),id.ToString());

            return id;
        }

        /// <summary>
        /// 创建借货单明细
        /// </summary>
        /// <param name="productLendSysNo">借货单系统编号</param>
        /// <param name="currentUserSysNo">当前操作人系统编号</param>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="itemList">借货单明细实体集合</param>
        /// <param name="price">商品金额</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        private void CreateProductLendItem(int productLendSysNo, int currentUserSysNo, int deliveryUserSysNo,
            ICollection<WhProductLendItem> itemList, ref decimal price)
        {
            if (itemList == null || !itemList.Any()) return;
            //取跟历史价格一致的借货商品
            var list = itemList.Where(item => !CheckProductPrice(item.ProductSysNo, deliveryUserSysNo).Status);
            var quantity = 0;
            foreach (var item in list.Where(item => item.LendQuantity > 0))
            {
                quantity += item.LendQuantity;
                item.ProductLendSysNo = productLendSysNo;
                item.LastUpdateBy = item.CreatedBy = currentUserSysNo;
                item.LastUpdateDate = item.CreatedDate = DateTime.Now;
                var itemId = ProductLendBo.Instance.CreateWhProductLendItem(item);
                if (itemId <= 0) continue;
                var tmpPrice = CreateProductLendPrice(item.ProductSysNo, itemId);
                //汇总金额
                price += tmpPrice * item.LendQuantity;
            }
            if (quantity <= 0)
            {
                throw new HytException("所有借货商品数量总和应大于0，排法操作或请升级您的浏览器到最新版本！");
            }
        }

        /// <summary>
        /// 检查本次借货商品价格与历史借货价格差异
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>有不一致的历史价格是返回结果true</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        public Result CheckProductPrice(int productSysNo, int deliveryUserSysNo)
        {
            var result = new Result() { StatusCode = -1 };
            //获得商品配送员价格
            var product = PdProductBo.Instance.GetProduct(productSysNo);
            if (product != null && product.Status == (int)ProductStatus.商品状态.上架)
            {
                var priceList = product.PdPrice.Value;
                if (null != priceList)
                {
                    const int status = (int)WarehouseStatus.借货单状态.已出库;
                    const int priceSource = (int)ProductStatus.产品价格来源.配送员进货价;
                    //取得当前商品对应配送员时货价
                    var pdPrice = priceList.SingleOrDefault(item => item.PriceSource == priceSource);
                    if (pdPrice != null && pdPrice.Status == (int)ProductStatus.产品价格状态.有效)
                    {
                        var price = pdPrice.Price;
                        //获取历史借货商品价格列表
                        var historyPriceList = IProductLendDao.Instance.GetHistoryPrice(new ParaWhProductLendItemFilter()
                        {
                            ProductSysNo = productSysNo,
                            DeliveryUserSysNo = deliveryUserSysNo,
                            Status = status,
                            PriceSource = priceSource
                        });
                        if (null != historyPriceList)
                        {
                            //检查价格是否有不一致的记录
                            result.Status = historyPriceList.Any(x => x != price);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 检查本次借货商品价格与历史借货价格差异
        /// </summary>
        /// <param name="productSysNos">商品系统编号列表</param>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>有不一致的历史价格是返回结果true</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        public List<dynamic> CheckProductPrice(List<int> productSysNos, int deliveryUserSysNo)
        {
            var list = new List<dynamic>();
            if (productSysNos != null && productSysNos.Any())
            {
                productSysNos.ForEach(x =>
                    {
                        //获得商品配送员价格
                        var product = PdProductBo.Instance.GetProduct(x);
                        if (product == null || product.Status != (int)ProductStatus.商品状态.上架) return;
                        var priceList = product.PdPrice.Value;
                        if (null == priceList) return;
                        const int status = (int)WarehouseStatus.借货单状态.已出库;
                        const int priceSource = (int)ProductStatus.产品价格来源.配送员进货价;
                        //取得当前商品对应配送员时货价
                        var pdPrice = priceList.SingleOrDefault(p => p.PriceSource == priceSource);
                        if (pdPrice == null || pdPrice.Status != (int)ProductStatus.产品价格状态.有效) return;
                        var price = pdPrice.Price;
                        //获取历史借货商品价格列表
                        var historyPriceList = IProductLendDao.Instance.GetHistoryPrice(new ParaWhProductLendItemFilter()
                            {
                                ProductSysNo = x,
                                DeliveryUserSysNo = deliveryUserSysNo,
                                Status = status,
                                PriceSource = priceSource
                            });
                        var priceChanges = false;
                        //检查价格是否有不一致的记录
                        if (null != historyPriceList)
                        {
                            if (historyPriceList.Any(y => y != price))
                            {
                                priceChanges = true;
                            }
                        }
                        list.Add(new
                            {
                                pid = x,
                                name = product.ProductName,
                                courier = price,
                                lendQuantity = 1,
                                priceChanges = priceChanges
                            });
                    });
            }
            return list;
        }

        /// <summary>
        /// 创建借货商品价
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="productLendItemSysNo">借货单明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        private decimal CreateProductLendPrice(int productSysNo, int productLendItemSysNo)
        {
            decimal price = 0;
            //价格过滤
            var showPriceType = new ProductStatus.产品价格来源[]
                                    {
                                        ProductStatus.产品价格来源.基础价格, 
                                        ProductStatus.产品价格来源.会员等级价, 
                                        ProductStatus.产品价格来源.配送员进货价
                                    };
            var priceList = PdPriceBo.Instance.GetProductPrice(productSysNo, showPriceType);
            if (priceList != null && priceList.Count > 0)
            {
                foreach (var item in priceList)
                {
                    var model = new WhProductLendPrice
                    {
                        ProductLendItemSysNo = productLendItemSysNo,
                        Price = item.Price,
                        PriceSource = item.PriceSource,
                        SourceSysNo = item.SourceSysNo
                    };
                    if (item.PriceSource == (int)ProductStatus.产品价格来源.配送员进货价)
                    {
                        price = item.Price;
                    }
                    ProductLendBo.Instance.CreateWhProductLendPrice(model);
                }
            }
            return price;
        }

        /// <summary>
        /// 更新借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public int UpdateWhProductLend(WhProductLend model)
        {
            return IProductLendDao.Instance.UpdateWhProductLend(model); ;
        }

        /// <summary>
        /// 作废借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>成功失败</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public Result CancelWhProductLend(int sysNo)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = GetWhProductLend(sysNo);
                if (model != null)
                {
                    var list = IProductLendDao.Instance.GetWhProductLendItemPagerList(new ParaWhProductLendItemFilter()
                        {
                            ProductLendSysNo = model.SysNo
                        });
                    //计算配送员借货额度
                    if (list.Rows.Count > 0)
                    {
                        var price = list.Rows.Sum(item => item.Price);
                        //UpdateDeliveryUserCredit(model.DeliveryUserSysNo, model.WarehouseSysNo, price);
                        DeliveryUserCreditBo.Instance.UpdateRemaining(model.WarehouseSysNo, model.DeliveryUserSysNo, 0, price, "作废借货单，单号：" + sysNo);
                    }
                    model.Status = (int)WarehouseStatus.借货单状态.作废;
                    var id = IProductLendDao.Instance.UpdateWhProductLend(model);
                    if (id > 0)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = string.Format("借货单{0}作废失败!", sysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.借货单, sysNo, ex);
            }
            return result;
        }

        /// <summary>
        /// 强制完结借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <param name="lastUpdateBySysNo">最后更新人系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks> 
        public Result EndProductLend(int sysNo, int lastUpdateBySysNo)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = GetWhProductLend(sysNo);
                if (model != null)
                {
                    var pager =
                        IProductLendDao.Instance.GetWhProductLendItemPagerList(new ParaWhProductLendItemFilter
                            {
                                ProductLendSysNo = sysNo
                            });
                    if (null != pager.Rows)
                    {
                        foreach (var item in pager.Rows)
                        {
                            //计算数量
                            var count = item.SaleQuantity + item.ReturnQuantity + item.ForceCompleteQuantity;
                            if (count >= item.LendQuantity) continue;
                            //计算强制完结数量
                            item.ForceCompleteQuantity = item.LendQuantity - item.SaleQuantity - item.ReturnQuantity;
                            IProductLendDao.Instance.UpdateWhProductLendItem(item);
                        }
                    }
                    result = CompleteProductLend(sysNo, lastUpdateBySysNo);
                }
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.借货单, sysNo, ex);
            }
            return result;
        }

        /// <summary>
        /// 完结借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <param name="lastUpdateBySysNo">最后更新人系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks> 
        public Result CompleteProductLend(int sysNo, int lastUpdateBySysNo)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = GetWhProductLend(sysNo);
                if (model != null)
                {
                    model.LastUpdateBy = lastUpdateBySysNo;
                    model.LastUpdateDate = DateTime.Now;

                    //通过借货单系统编号获取未还货或销售完成的借货单明细条数
                    var count = IProductLendDao.Instance.GetWhProductLendItemListCount(new ParaWhProductLendItemFilter()
                                {
                                    ProductLendSysNo = sysNo,
                                    Status = WarehouseStatus.借货单状态.已出库.GetHashCode()
                                });
                    if (count == 0)//所有借货商品都已还货或销售完成
                    {
                        model.Status = WarehouseStatus.借货单状态.已完成.GetHashCode();
                    }
                    else
                    {
                        result.Message = "该借货单不能完结，还有未还货或销售完的商品！";
                    }
                    var id = IProductLendDao.Instance.UpdateWhProductLend(model);
                    if (id > 0)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = string.Format("借货单{0}完结失败!", sysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.借货单, sysNo, ex);
            }
            return result;
        }

        /// <summary>
        /// 通过过滤条件借货单编号获取借货明细列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货明细列表</returns>
        /// <remarks>2013-07-12 周唐炬 创建</remarks>
        public PagedList<CBWhProductLendItem> GetWhProductLendItemList(ParaWhProductLendItemFilter filter)
        {
            PagedList<CBWhProductLendItem> pagedList = null;
            try
            {
                if (null != filter)
                {
                    pagedList = new PagedList<CBWhProductLendItem>();
                    var pager = IProductLendDao.Instance.GetWhProductLendItemPagerList(filter);
                    if (pager != null)
                    {
                        pagedList.TData = pager.Rows;
                        pagedList.TotalItemCount = pager.TotalRows;
                        pagedList.CurrentPageIndex = filter.CurrentPage;
                        pagedList.PageSize = filter.PageSize;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.借货单, 0, ex);
            }
            return pagedList;
        }

        /// <summary>
        /// 创建借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public int CreateWhProductLendItem(WhProductLendItem model)
        {
            return IProductLendDao.Instance.CreateWhProductLendItem(model);
        }

        /// <summary>
        /// 更新借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public int UpdateWhProductLendItem(WhProductLendItem model)
        {
            return IProductLendDao.Instance.UpdateWhProductLendItem(model);
        }

        /// <summary>
        /// 创建借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public int CreateWhProductLendPrice(WhProductLendPrice model)
        {
            return IProductLendDao.Instance.CreateWhProductLendPrice(model);
        }

        /// <summary>
        /// 更新借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public int UpdateWhProductLendPrice(WhProductLendPrice model)
        {
            return IProductLendDao.Instance.UpdateWhProductLendPrice(model);
        }

        /// <summary>
        /// 根据配送人员系统编号及商品系统编号获取仓库系统编号
        /// </summary>
        /// <param name="delSysNo">配送人员系统编号</param>
        /// <param name="pSysNo">商品系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-07-19 黄伟 创建</remarks>
        public int GetWhSysNoByDelUserAndProduct(int delSysNo, int pSysNo)
        {
            return IProductLendDao.Instance.GetWhSysNoByDelUserAndProduct(delSysNo, pSysNo);
        }
    }
}
