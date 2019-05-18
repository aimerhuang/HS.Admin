using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品属性数据操作接口
    /// </summary>
    /// <remarks>2013-07-18 唐永勤 创建</remarks>
    public abstract class IPdProductAttributeDao : DaoBase<IPdProductAttributeDao>
    {
        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="model">商品属性实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-07-24 唐永勤 创建</remarks>
        public abstract int Create(PdProductAttribute model);

        /// <summary>
        /// 根据属性组编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性组编号集</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public abstract IList<CBPdProductAtttributeRead> GetProductAttributeByGroupSysNo(IList<int> listSysNo);

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public abstract IList<CBPdProductAtttributeRead> GetProductAttributeByProductSysNo(int productSysNo);

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="onlyAssociationAttribute">只读取关联属性</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>    
        public abstract IList<PdProductAttribute> GetProductAttributeByProductSysNo(int productSysNo, bool onlyAssociationAttribute);

        /// <summary>
        /// 根据商品分类获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>    
        public abstract IList<CBPdProductAtttributeRead> GetCategoryProductAttributeByProductSysNo(int productSysNo);

        /// <summary>
        /// 根据编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性编号集</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public abstract IList<CBPdProductAtttributeRead> GetProductAttributeByAttributeSysNo(IList<int> listSysNo);

        /// <summary>
        /// 保存商品属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="list">商品属性列表</param>
        /// <returns>保存是否成功</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks> 
        public abstract bool SaveProductAttribute(int productSysNo, IList<PdProductAttribute> list);
    }
}
