using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.InventorySheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.InventorySheet
{
    /// <summary>
    /// 盘点作业
    /// </summary>
    public abstract class IWhInventoryDao : DaoBase<IWhInventoryDao>
    {
        /// <summary>
        /// 分页获取盘点作业单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public abstract Pager<Hyt.Model.InventorySheet.WhInventory> GetSoOrders(Pager<Hyt.Model.InventorySheet.WhInventory> pager);

        /// <summary>
        /// 创建盘点作业
        /// </summary>
        /// <param name="model">盘点实体</param>
        /// <param name="productModel">盘点商品实体</param>
        /// <returns></returns>
        public abstract int AddWhInventory(Hyt.Model.InventorySheet.WhInventory model, List<WhInventoryProduct> productModel);


        /// <summary>, 
        /// 查询当天的盘点单总数
        /// </summary>
        /// <returns></returns>
        public abstract int GetWhInventoryCount();


        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract Pager<Hyt.Model.InventorySheet.WhInventoryDetail> GetWhInventoryDetail(int PageIndex,int sysNo);


        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract Hyt.Model.InventorySheet.WhInventoryDetail GetWhInventoryDetail(int sysNo);
       
        /// <summary>
        /// 根据仓库id查询仓库名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract string GetWhWarehouseName(int sysNo);


        /// <summary>
        /// 根据商品编号查询商品名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract PdProduct GetProductName(int sysNo);


        /// <summary>
        /// 根据品牌编号查询品牌名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract string GetBrandName(int sysNo);

        /// <summary>
        /// 更新盘点库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract bool UploadPDQuantity(int sysNo, decimal Quantity, decimal ZhangCunQuantity);
        

        /// <summary>
        /// 更新调整数量/实际库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract bool UploadSJQuantity(int sysNo, decimal Quantity);


        /// <summary>
        /// 更新盘点单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract bool UploadStatus(int sysNo, int status);


        /// <summary>
        /// 生成盘点报告单
        /// </summary>
        /// <returns></returns>
        public abstract bool AddWhInventoryRepor(WhInventoryRepor reporModl, List<WhIReporPrDetails> productModel);


        /// <summary>
        /// 分页获取盘点报告单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public abstract Pager<WhInventoryRepor> GetWhInventoryReporPage(Pager<WhInventoryRepor> pager);

        /// <summary>
        /// 根据id获取盘点报告单
        /// </summary>
        /// <returns></returns>
        public abstract WhInventoryRepor GetWhInventoryRepor(int sysNo);
      
        /// <summary>
        /// 根据id获取盘点报告单明细
        /// </summary>
        /// <returns></returns>
        public abstract WhInventoryRepor GetWhInventoryReporModel(int sysNo, int PageType);


        /// <summary>
        /// 根据id获取是否已生成了盈亏报告单
        /// </summary>
        /// <param name="sysNo">盘点单id</param>
        /// <param name="status">盈亏状态  1盈 2亏</param>
        /// <returns></returns>
        public abstract bool GetIsWhInventoryRepor(string Code, int status);


        /// <summary>
        /// 更新盘点报告单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract bool UploadWhInventoryReporStatus(int sysNo, int status);


        /// <summary>
        /// 更新盘点报告单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract bool UploadWhInventoryRepor(WhInventoryRepor model);


        /// <summary>
        /// 根据盘点报告单系统编号 查询盘点商品报告单列表
        /// </summary>
        /// <param name="sysNo">盘点单id</param>
        /// <returns></returns>
        public abstract List<WhIReporPrDetails> GetWhIReporPrDetailsPid(int sysNo, int status);


        /// <summary>
        /// 更新产品库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool UpdatePdProductStock(List<WhIReporPrDetails> model);

        /// <summary>
        /// 根据商品id获取商品和商品对应仓库信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract List<uditPdProductStock> GetuditPdProductStock(int SysNo, int? whSysId);


        /// <summary>
        /// 根据商品编码和仓库编码获取商品和商品对应仓库信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract uditPdProductStock GetuditPdProductStockTo(string SysNo, string whSysId);



        #region 其他出入库

        /// <summary>
        /// 分页获取其他出入库
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dataType">1查询全部 2查询其他出库 3查询其他入库</param>
        /// <returns></returns>
        /// 2017-8-07
        public abstract Pager<OtherOutOfStorage> GetOtherOutOfStoragePage(Pager<OtherOutOfStorage> pager, int? dataTyp = 1);


        #region 创建其他入库
        /// <summary>
        /// 添加其他入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int AddOtherOutOfStorage(OtherOutOfStorage model);
        #endregion


        #region 根据id获取其他出入库明细
        /// <summary>
        /// 根据id获取其他出入库明细
        /// </summary>
        /// <returns></returns>
        public abstract OtherOutOfStorage GetOtherOutOfStorageModel(int sysNo);

        #endregion 其他出入库更新库存
        /// <summary>
        /// 其他出入库更新库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool UpdateOtherOutPdProductStock(OtherOutOfStorage model);
        #endregion



    }
}
