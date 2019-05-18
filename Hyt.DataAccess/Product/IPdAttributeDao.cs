using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品属性维护
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>    
    public abstract class IPdAttributeDao : DaoBase<IPdAttributeDao>
    {
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract int Create(PdAttribute model);

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-12-04 唐永勤 创建</remarks>
        public abstract bool IsExists(PdAttribute model);

        /// <summary>
        /// 获取指定编号的属性信息
        /// </summary>
        /// <param name="sysNo">属性编号</param>
        /// <returns>属性实体信息</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract PdAttribute GetEntity(int sysNo);

        /// <summary>
        /// 根据属性编号更新属性信息
        /// </summary>
        /// <param name="model">属性实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract bool Update(PdAttribute model);

        /// <summary>
        /// 更新属性状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性状态 status, List<int> listSysNo);

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="pager">属性查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract void GetPdAttributeList(ref Pager<PdAttribute> pager);
       
        /// <summary>
        /// 设置属性选项
        /// </summary>
        /// <param name="sysNo">属性编号</param>
        /// <param name="listAttributeOptions">属性选项列表</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-07-06 唐永勤 创建</remarks>
        public abstract bool SetAttributeOptions(int sysNo, IList<PdAttributeOption> listAttributeOptions);

        /// <summary>
        /// 获取属性所有选项
        /// </summary>
        /// <param name="attributeSysNo">属性编号</param>
        /// <returns>属性选项列表</returns>
        /// <remarks>2013-07-09 唐永勤 创建</remarks>
        public abstract IList<PdAttributeOption> GetAttributeOptions(int attributeSysNo);

        /// <summary>
        /// 获取已选的属性列表
        /// </summary>
        /// <param name="listSysNo">属性编号列表</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-10 唐永勤 创建</remarks>
        public abstract IList<PdAttribute> GetSelectedAttributes(IList<int> listSysNo);

        /// <summary>
        /// 通过商品系统编号获取商品的关联属性
        /// </summary>
        /// <param name="productSysNoList">商品系统编号</param>
        /// <param name="context">数据库操作上线文</param>
        /// <returns>返回属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public abstract IList<PdAttribute> GetProductAssociationAttribute(int[] productSysNoList, IDbContext context = null);

        /// <summary>
        /// 判断属性选项是否被商品使用
        /// </summary>
        /// <param name="sysNo">选项编号</param>
        /// <returns>被使用返回true，未被使用返回false</returns>
        /// <remarks>2013-07-30 唐永勤 创建</remarks>
        public abstract bool IsAttributeOptionsInProduct(int sysNo);

        /// <summary>
        /// 获取商品属性列表
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <returns>商品属性列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public abstract IList<PdAttribute> GetPdAttributeList(int categorySysNo);

        /// <summary>
        /// 获取商品属性关联
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品属性关联列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public abstract IList<PdProductAttribute> GetPdProductAttributeList(int pdSysNo);
    }
}
