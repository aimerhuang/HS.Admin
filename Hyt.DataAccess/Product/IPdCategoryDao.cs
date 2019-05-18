using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品分类接口
    /// </summary>
    /// <remarks>2013-06-25 邵斌 创建</remarks>
    public abstract class IPdCategoryDao : DaoBase<IPdCategoryDao>
    {
        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="category">商品分类实体</param>
        /// <param name="attributeGroupAso">商品分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public abstract bool CreateCategory(PdCategory category, IList<PdCatAttributeGroupAso> attributeGroupAso);

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="category">商品分类实体</param>
        /// <param name="attributeGroupAso">商品分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public abstract bool EditCategory(PdCategory category, IList<PdCatAttributeGroupAso> attributeGroupAso);

        /// <summary>
        /// 读取所以商品分类包括无效分类
        /// </summary>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public abstract IList<PdCategory> GetAllCategory();

        /// <summary>
        /// 读取所以有效的商品分类,可以读取指定的商品分类类别
        /// </summary>
        /// <param name="sysNo">父节点商品分类编号</param>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public abstract IList<PdCategory> GetCategoryList(int? sysNo);

        /// <summary>
        /// 获取单个商品分类
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isGetParent">是否获取父级商品分类对象</param>
        /// <param name="context">共用数据库操作上下文</param>
        /// <returns>返回单个商品分类</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public abstract PdCategory GetCategory(int sysNo, bool isGetParent = false, IDbContext context = null);

        /// <summary>
        /// 更新商品分类状态
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">管理员账号，记录变更记录</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        public abstract bool ChangeStatus(int sysNo, ProductStatus.商品分类状态 status,int adminSysNo);

        /// <summary>
        /// 设置商品分类是否显示
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isOnline">是否前台展示 true:展示 false:隐藏</param>
        /// <param name="adminSysNo">管理员系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Obsolete]
        public abstract bool SetIsOnline(int sysNo, ProductStatus.是否前端展示 isOnline, int adminSysNo);

        /// <summary>
        /// 保存分类信息
        /// </summary>
        /// <param name="updateCategory">分类实体对象</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        public abstract bool Update(PdCategory updateCategory);

        /// <summary>
        /// 获取父节点下的空闲Code编号
        /// </summary>
        /// <param name="parentSysNo">父商品分类系统编号</param>
        /// <returns>返回 可用用的Code编号数字</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public abstract dynamic GetFreeCodeNum(int parentSysNo);
        /// <summary>
        /// 获取新的系统编号路由
        /// </summary>
        /// <param name="newParentSysNo">新的父级系统编号</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新的系统编号路由</returns>
        /// <remarks>2015-07-20 杨浩 创建</remarks>
        public abstract string GetNewSysNos(int newParentSysNo, int sysNo);

        /// <summary>
        /// 更新子分类编码
        /// </summary>
        /// <param name="oldParentCode">原父级编码</param>
        /// <param name="newParentCode">新父级编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public abstract bool UpdataChildrenCode(string oldParentCode, string newParentCode);
        /// <summary>
        /// 更新子分类系统编码路由
        /// </summary>
        /// <param name="oldParentSysNos">原父级系统编码路由</param>
        /// <param name="newParentSysNos">新父级系统编码路由</param>
        /// <param name="oldParentSysNo">原父级系统编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2015-07-30 杨浩 创建</remarks>
        public abstract bool UpdateChildrenSysNos(string oldParentSysNos, string newParentSysNos,int oldParentSysNo);

        /// <summary>
        /// 将分类排在父分类的末尾显示
        /// </summary>
        /// <param name="category">商品分类对象</param>
        /// <param name="context">供应数据库操作上线文</param>
        /// <returns></returns>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public abstract bool SetCategoryToLastShow(PdCategory category, IDbContext context = null);
        /// <summary>
        /// 设置分类系统编号路由
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2015-07-29 杨浩 创建</remarks>
        public abstract bool SetCateorySysNosBySysNo(int sysNo);

        #region 供应链商品分类
        /// <summary>
        /// 添加关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreateSupplyChain(PdCategoryRelatedSupplyChain model);
        /// <summary>
        /// 是否添加过
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public abstract bool IsExistsSupplyChain(string SCName, int SupplyChainCode);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract PdCategoryRelatedSupplyChain GetEntitySupplyChain(int SysNo);
        /// <summary>
        /// 根据供应链编号及供应链商品分类名称获取数据
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public abstract PdCategoryRelatedSupplyChain GetEntityByNameSupplyChain(string SCName, int SupplyChainCode);
        /// <summary>
        /// 根据分类ID及供应商编号获取数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public abstract PdCategoryRelatedSupplyChain GetEntityByCSysNoSupplyChain(int CategorySysNo, int SupplyChainCode);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool UpdateSupplyChain(PdCategoryRelatedSupplyChain model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract int DeleteSupplyChain(int sysNo);
        /// <summary>
        /// 根据类别ID查询数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <returns></returns>
        public abstract IList<PdCategoryRelatedSupplyChain> SelectAllSupplyChain(int CategorySysNo);
         /// <summary>
        /// 根据类别ID、供应链编号查询供应链绑定数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public abstract IList<PdCategoryRelatedSupplyChain> GetSupplyChainList(int CategorySysNo, int SupplyChainCode);
        #endregion


        public abstract IList<PdCategory> GetAllCategory(int sysNo);

        public abstract IList<PdCategory> GetCategoryListByParent(int[] parentSysNo);

        public abstract IList<PdCategory> GetCategoryListByParentName(int parentSysNo);
        
    }
}
