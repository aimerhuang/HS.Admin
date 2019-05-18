using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 搜索关键字接口抽象类
    /// </summary>
    /// <remarks>2013-06-27 杨晗 创建</remarks>
    public abstract class IFeSearchKeywordDao : DaoBase<IFeSearchKeywordDao>
    {
        /// <summary>
        /// 根据搜索关键字系统号获取实体
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>搜索关键字实体</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract FeSearchKeyword GetModel(int sysNo);

        /// <summary>
        /// 判断搜索关键字是否重复
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public abstract bool FeSearchKeywordVerify(string keyword);

        /// <summary>
        ///     搜索关键字分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">状态</param>
        /// <param name="hitsCount">点击次数</param>
        /// <param name="createdDateStart">创建时间</param>
        /// <param name="createdDateEnd">创建时间</param>
        /// <param name="searchName">文章标题名称</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract IList<FeSearchKeyword> Seach(int pageIndex, int pageSize,
                                                     int? status, int? hitsCount,
                                                     DateTime? createdDateStart,
                                                     DateTime? createdDateEnd, string searchName = null);

        /// <summary>
        ///     根据条件获取文章的总条数
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="hitsCount">点击次数</param>
        /// <param name="createdDateStart">创建时间</param>
        /// <param name="createdDateEnd">创建时间</param>
        /// <param name="searchName">文章标题名称</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract int GetCount(int? status, int? hitsCount, DateTime? createdDateStart,
                                     DateTime? createdDateEnd, string searchName = null);

        /// <summary>
        ///     插入搜索关键字
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract int Insert(FeSearchKeyword model);

        /// <summary>
        ///     更新搜索关键字
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract int Update(FeSearchKeyword model);

        /// <summary>
        ///     删除搜索关键字
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}