using System;
using System.Collections.Generic;
using Hyt.BLL.Log;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    ///     搜索关键字业务逻辑层
    /// </summary>
    /// <remarks>2013－06-27 杨晗 创建</remarks>
    public class FeSearchKeywordBo : BOBase<FeSearchKeywordBo>
    {
        /// <summary>
        ///     根据搜索关键字系统号获取实体
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>搜索关键字实体</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public FeSearchKeyword GetModel(int sysNo)
        {
            return IFeSearchKeywordDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 判断搜索关键字是否重复
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public bool FeSearchKeywordVerify(string keyword)
        {
            return IFeSearchKeywordDao.Instance.FeSearchKeywordVerify(keyword);
        }

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
        public IList<FeSearchKeyword> Seach(int pageIndex, int pageSize,
                                            int? status, int? hitsCount, DateTime? createdDateStart,
                                            DateTime? createdDateEnd, string searchName = null)
        {
            return IFeSearchKeywordDao.Instance.Seach(pageIndex, pageSize, status, hitsCount, createdDateStart,
                                                      createdDateEnd, searchName);
        }

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
        public int GetCount(int? status, int? hitsCount, DateTime? createdDateStart,
                            DateTime? createdDateEnd, string searchName = null)
        {
            return IFeSearchKeywordDao.Instance.GetCount(status, hitsCount, createdDateStart,
                                                         createdDateEnd, searchName);
        }

        /// <summary>
        ///     插入搜索关键字
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public int Insert(FeSearchKeyword model)
        {
            int i= IFeSearchKeywordDao.Instance.Insert(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "新增关键字" + model.keyword, LogStatus.系统日志目标类型.搜索关键字, i, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return i;
        }

        /// <summary>
        ///     更新搜索关键字
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public int Update(FeSearchKeyword model)
        {
            int u= IFeSearchKeywordDao.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "修改关键字" + model.keyword, LogStatus.系统日志目标类型.搜索关键字, u, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        ///     删除搜索关键字
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "删除系统号为"+sysNo+"的关键字", LogStatus.系统日志目标类型.搜索关键字, sysNo, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return IFeSearchKeywordDao.Instance.Delete(sysNo);
        }
    }
}