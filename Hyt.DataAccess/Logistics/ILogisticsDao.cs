using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;


namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 取商检数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public abstract class ILogisticsDao : DaoBase<ILogisticsDao>
    {
 
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract int InsertLgGaoJieGoodsInfoEntity(LgGaoJieGoodsInfo entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public abstract int UpdateLgGaoJieGoodsInfoEntity(LgGaoJieGoodsInfo entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract LgGaoJieGoodsInfo GetLgGaoJieGoodsInfoEntityByPid(int ProductSysNo);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract int InsertLgGaoJiePushInfoEntity(LgGaoJiePushInfo entity);
        /// <summary>
        /// 获取所有商品备案信息
        /// </summary>
        /// <returns>商品备案信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public abstract IList<LgGaoJieGoodsInfo> GetAllGaoJieGoodsInfo();

        /// <summary>
        /// 新增商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void CreateExcelGaoJieGoodsInfo(List<LgGaoJieGoodsInfo> models);

        /// <summary>
        /// 更新商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelGaoJieGoodsInfo(List<LgGaoJieGoodsInfo> models);
    }
}