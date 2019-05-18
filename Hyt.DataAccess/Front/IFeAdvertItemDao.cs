using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 广告项接口抽象类
    /// </summary>
    /// <remarks>2013－06-14 苟治国 创建</remarks>
    public abstract class IFeAdvertItemDao : DaoBase<IFeAdvertItemDao>
    {
        /// <summary>
        /// 查看广告项
        /// </summary>
        /// <param name="sysNo">广告项编号</param>
        /// <returns>广告项</returns>
        /// <remarks>2013－06-14 苟治国 创建</remarks>
        public abstract Model.FeAdvertItem GetModel(int sysNo);

        /// <summary>
        /// 根据条件获取广告项的列表
        /// </summary>
        /// <param name="pager">广告项查询条件</param>
        /// <param name="para">参数</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        public abstract Pager<Model.CBFeAdvertItem> Seach(Pager<CBFeAdvertItem> pager,ParaFeAdvertItem para);

        /// <summary>
        /// 根据广告组获取所有广告项分类
        /// </summary>
        /// <param name="groupSysNo">广告组编号</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract IList<FeAdvertItem> GetListByGroup(int groupSysNo);

        /// <summary>
        /// 插入广告项
        /// </summary>
        /// <param name="model">广告项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract int Insert(Model.FeAdvertItem model);

        /// <summary>
        /// 更新广告项
        /// </summary>
        /// <param name="model">广告项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract int Update(Model.FeAdvertItem model);

        /// <summary>
        /// 删除广告项
        /// </summary>
        /// <param name="sysNo">广告项主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);
        /// <summary>
        /// 同步总部已审核的广告
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public abstract int ProCreateFeAdvertItem(int GroupSysNo, int DealerSysNo, int CreatedBy);
    }
}
