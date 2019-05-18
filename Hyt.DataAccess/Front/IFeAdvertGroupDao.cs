using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 广告组接口抽象类
    /// </summary>
    /// <remarks>2013－06-14 苟治国 创建</remarks>
    public abstract class IFeAdvertGroupDao : DaoBase<IFeAdvertGroupDao>
    {
        /// <summary>
        /// 查看广告组
        /// </summary>
        /// <param name="sysNo">广告组编号号</param>
        /// <returns>广告组</returns>
        /// <remarks>
        /// 2013－06-14 苟治国 创建
        /// </remarks>
        public abstract Model.FeAdvertGroup GetModel(int sysNo);

        /// <summary>
        /// 验证广告组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>数据</returns>
        /// <remarks>2013－06-14 苟治国 创建</remarks>
        public abstract int FeAdvertGroupChk(string key, int sysNo);

        /// <summary>
        /// 根据条件获取广告组的列表
        /// </summary>
        /// <param name="pager">广告组查询条件</param>
        /// <returns>广告组列表</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public abstract Pager<Model.FeAdvertGroup> Seach(Pager<FeAdvertGroup> pager);

        /// <summary>
        /// 根据条件获取广告组的总条数
        /// </summary>
        /// <param name="type">广告类型</param>
        /// <param name="platformType">广告组平台类型</param>
        /// <param name="status">广告状态</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>总数</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract int GetCount(int? type, int? platformType, int? status, string key = null);

        /// <summary>
        /// 插入广告组
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract int Insert(Model.FeAdvertGroup model);

        /// <summary>
        /// 更新广告组
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract int Update(Model.FeAdvertGroup model);

        /// <summary>
        /// 删除广告组
        /// </summary>
        /// <param name="sysNo">广告主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 添加店铺关联广告项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public abstract int InsertDealerFeAdvertItem(DsDealerFeAdvertItem model);
        /// <summary>
        /// 更新店铺关联广告项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public abstract int UpdateDealerFeAdvertItem(Model.DsDealerFeAdvertItem model);
        /// <summary>
        /// 获取店铺关联广告项表信息
        /// </summary>
        /// <param name="FeAdvertGroupSysNO"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public abstract DsDealerFeAdvertItem GetModelDealerFeAdvertItem(int FeAdvertGroupSysNO);
        /// <summary>
        /// 删除店铺关联广告项表信息
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public abstract bool DeleteDealerFeAdvertItem(int FeAdvertItemSysNO);
         /// <summary>
        /// 是否存在店铺广告项
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public abstract int IsExistenceDealerFeAdvertItem(int FeAdvertItemSysNO);
    }
}
