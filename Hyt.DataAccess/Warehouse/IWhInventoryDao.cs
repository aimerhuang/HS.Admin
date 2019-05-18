using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Warehouse
{
    public abstract class IWhInventoryDao : DaoBase<IWhInventoryDao>
    {
        /// <summary>
        /// 创建盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public abstract WhInventory CreateWhInventory(WhInventory model);

        /// <summary>
        /// 分页查询盘点单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public abstract Pager<CBWhInventory> QueryWhInventoryPager(Pager<CBWhInventory> pager);

        /// <summary>
        /// 获取盘点单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public abstract WhInventory GetWhInventoryEntity(int sysNo);

        /// <summary>
        /// 获取库存盘点单商品列表（添加盘点商品用）
        /// </summary>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public abstract List<WhInventoryDetail> GetWhInventoryProducts(int inventorySysNo);

        /// <summary>
        /// 获取库存盘点单商品列表（单个）
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-21 陈海裕 创建</remarks>
        public abstract WhInventoryDetail GetWhInventoryDetailEntity(int productSysNo, int inventorySysNo);

        /// <summary>
        /// 更新盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 陈海裕 创建</remarks>
        public abstract int UpdateWhInventory(WhInventory model);

        /// <summary>
        /// 添加盘点单明细
        /// </summary>
        /// <param name="inventoryDetail"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public abstract int AddWhInventoryDetail(WhInventoryDetail inventoryDetail);

        /// <summary>
        /// 分页查询盘点单明细
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public abstract Pager<CBWhInventoryDetail> QueryWhInventoryDetailPager(Pager<CBWhInventoryDetail> pager, int whPositionSysNo = 0, string searchKeyWord = "");

        /// <summary>
        /// 删除盘点单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-23 陈海裕 创建</remarks>
        public abstract int DeleteWhInventoryDetail(int sysNo);

        /// <summary>
        /// 更新盘点单商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-23 陈海裕 创建</remarks>
        public abstract int UpdateWhInventoryDetail(WhInventoryDetail model);

        /// <summary>
        /// 批量插入导入盘点记录
        /// </summary>
        /// <param name="lstToInsert">待插入数据</param>
        /// <remarks>2016-08-19 刘伟豪 创建</remarks>
        public abstract void InsertWhInventoryDetail(List<WhInventoryDetail> models);

        /// <summary>
        /// 批量更新导入盘点记录
        /// </summary>
        /// <param name="lstToUpdate">待更新数据</param>
        /// <remarks>2016-08-19 刘伟豪 创建</remarks>
        public abstract void UpdateWhInventoryDetail(List<WhInventoryDetail> models);
        /// <summary>
        /// 通过日期获取所有的盘点单数据
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks>2016-12-26 杨云奕 添加</remarks>
        public abstract List<WhInventory> GetAllWhInventoryListByDate(DateTime dateTime);
    }
}