using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品描述模板数据层接口类
    /// </summary>
    /// <remarks>2013-07-22 杨晗 创建</remarks>
    public abstract class IPdTemplateDao : DaoBase<IPdTemplateDao>
    {
        /// <summary>
        /// 根据商品描述模板系统编号获取模型
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>商品描述模板实体</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public abstract PdTemplate GetModel(int sysNo);

        /// <summary>
        /// 判断商品描述模板名称是否重复
        /// </summary>
        /// <param name="name">商品描述模板名称</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public abstract bool PdTemplateVerify(string name);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="type">商品描述模板类型</param>
        /// <param name="count">抛出总条数</param>
        /// <param name="searchName">商品描述模板名称</param>
        /// <returns>商品描述模板列表</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public abstract IList<CBPdTemplate> Seach(int pageIndex, int pageSize, ProductStatus.商品描述模板类型? type,out int count ,string searchName=null );

        /// <summary>
        /// 插入商品描述模板
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public abstract int Insert(PdTemplate model);

        /// <summary>
        /// 更新商品描述模板
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public abstract int Update(PdTemplate model);

        /// <summary>
        /// 删除商品描述模板
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
