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
    public abstract class IPdProductAttributeOptionDao : DaoBase<IPdProductAttributeOptionDao>
    {

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public abstract int Insert(PdProductAttributeOption model);

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public abstract int Update(PdProductAttributeOption model);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public abstract bool Delete(int productSysNo);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="AttributeOptionSysNo"></param>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public abstract PdProductAttributeOption GetByProOptionSysNo(int ProductSysNo, int AttributeOptionSysNo);
    }
}
