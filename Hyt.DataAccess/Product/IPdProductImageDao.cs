using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品图片维护数据接口
    /// </summary>
    /// <remarks>2013-07-22 苟治国 创建</remarks>
    public abstract class IPdProductImageDao : DaoBase<IPdProductImageDao>
    {
        /// <summary>
        /// 查看商品图片
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>商品图片</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public abstract Model.PdProductImage GetModel(int sysNo);

        /// <summary>
        /// 获取指定商品的图片列表
        /// </summary>
        /// <param name="productsysNo">商品系统编号</param>
        /// <returns>获取指定商品的图片列表</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public abstract IList<PdProductImage> GetProductImg(int productsysNo);

        /// <summary>
        /// 获取B2B指定商品的图片列表
        /// </summary>
        /// <param name="productsysNo">商品系统编号</param>
        /// <returns>获取指定商品的图片列表</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        public abstract IList<PdProductImage> GetB2BProductImg(int productsysNo);

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public abstract int Insert(Model.PdProductImage model);
        /// <summary>
        /// B2B图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        public abstract int InsertB2B(Model.PdProductImage model);

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public abstract int Update(Model.PdProductImage model);

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="productSysNo">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public abstract int UpdateStatus(int productSysNo, int status);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-23 苟治国 创建</remarks>
        public abstract bool Delete(int productSysNo);
        /// <summary>
        /// 获取封面图
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract Model.PdProductImage GetModelByPdSysNo(int sysNo);
    }
}
