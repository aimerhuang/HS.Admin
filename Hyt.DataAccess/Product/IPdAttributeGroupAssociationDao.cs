using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品属性组属性关联表维护
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>    
    public abstract class IPdAttributeGroupAssociationDao : DaoBase<IPdAttributeGroupAssociationDao>
    {
        /// <summary>
        /// 添加属性组属性关联表
        /// </summary>
        /// <param name="model">属性组属性关联表实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract int Create(PdAttributeGroupAssociation model);

        /// <summary>
        /// 获取指定ID的属性组属性关联表信息
        /// </summary>
        /// <param name="sysNo">属性组属性关联表编号</param>
        /// <returns>属性组属性关联表实体信息</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract PdAttributeGroupAssociation GetEntity(int sysNo);

        /// <summary>
        /// 获取属性组所有属性
        /// </summary>
        /// <param name="attributeGroupSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract IList<PdAttribute> GetAttributes(int attributeGroupSysNo);

        /// <summary>
        /// 删除属性组某一关联属性
        /// </summary>
        /// <param name="sysNo">属性组属性关联表编号</param>
        /// <returns>删除成功返回true,删除失败返回false</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 上下移动记录顺序
        /// </summary>
        /// <param name="sysNo">主键编号</param>
        /// <param name="orderType">排序类型：向上(1)，向下(2)</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract bool ChangeOrder(int sysNo, int orderType);
    }
}
