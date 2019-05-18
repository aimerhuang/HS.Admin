using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品属性组维护接口
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>
    public abstract class IPdAttributeGroupDao : DaoBase<IPdAttributeGroupDao>
    {
        /// <summary>
        /// 添加属性组
        /// </summary>
        /// <param name="model">属性组实体信息</param>
        /// <returns>返回新建记录的编号</returns>       
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public abstract int Create(PdAttributeGroup model);

        /// <summary>
        /// 判断重复数据--属性组
        /// </summary>
        /// <param name="name">属性组名称</param>
        /// <param name="backName">后台显示名称</param>
        /// <param name="sysNo">属性组编号</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public abstract bool IsExists(string name, string backName, int sysNo);

        /// <summary>
        /// 获取指定ID的属性组信息
        /// </summary>
        /// <param name="sysNo">属性组编号</param>
        /// <returns>属性组实体信息</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public abstract PdAttributeGroup GetEntity(int sysNo);

        /// <summary>
        /// 根据属性组编号更新属性组信息
        /// </summary>
        /// <param name="model">属性组实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public abstract bool Update(PdAttributeGroup model);

        /// <summary>
        /// 更新属性组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性组编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性分组状态 status, List<int> listSysNo);

        /// <summary>
        /// 获取属性组列表
        /// </summary>
        /// <param name="pager">属性组查询条件</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-06-27 唐永勤 创建</remarks>
        public abstract Pager<PdAttributeGroup> GetPdAttributeGroupList(Pager<PdAttributeGroup> pager);

        /// <summary>
        /// 获取属性组所有属性
        /// </summary>
        /// <param name="attributeGroupSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract IList<PdAttribute> GetAttributes(int attributeGroupSysNo);

        /// <summary>
        /// 设置属性组属性
        /// </summary>
        /// <param name="sysNo">属性组编号</param>
        /// <param name="listAttribute">属性列表</param>
        /// <returns>成功返回true，失败返回false</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public abstract bool SetAttributes(int sysNo, IList<PdAttributeGroupAssociation> listAttribute);

        /// <summary>
        /// 读取商品分类对应的属性组
        /// </summary>
        /// <param name="pdCategorySysNo">商品分类编号</param>
        /// <returns>商品分类下所有的属性组列表</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public abstract IList<PdAttributeGroup> GetPdCategoryAttributeGroupList(int pdCategorySysNo);

        /// <summary>
        /// 获取已选的属性组列表
        /// </summary>
        /// <param name="listSysNo">属性组编号列表</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-07-12 唐永勤 创建</remarks>
        public abstract IList<PdAttributeGroup> GetSelectedAttributeGroups(IList<int> listSysNo);
    }
}
