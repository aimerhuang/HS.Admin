using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 商品项接口抽象类
    /// </summary>
    /// <remarks>2013－06-20 苟治国 创建</remarks>
    public abstract class IFeProductItemDao : DaoBase<IFeProductItemDao>
    {
        /// <summary>
        /// 根跟商品项编号搜索 
        /// </summary>
        /// <param name="sysNo">商品项编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public abstract Model.FeProductItem GetModel(int sysNo);

        /// <summary>
        /// 根据条件获取广告项的列表
        /// </summary>
        /// <param name="pager">广告项查询条件</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        public abstract Pager<Model.CBFeProductItem> Seach(Pager<CBFeProductItem> pager);

        /// <summary>
        /// 根据商品组获取所有商品项分类
        /// </summary>
        /// <param name="groupSysNo">商品组编号</param>
        /// <returns>商品项列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract IList<FeProductItem> GetListByGroup(int groupSysNo, int dealersysno);

        /// <summary>
        /// 查看在当前类型中是否有相同产品称
        /// </summary>
        /// <param name="mid">产品组编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>总数</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract int GetCount(int mid, int productSysNo);

        /// <summary>
        /// 新增商品项
        /// </summary>
        /// <param name="model">商品项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public abstract int Insert(Model.FeProductItem model);

        /// <summary>
        /// 更新商品项
        /// </summary>
        /// <param name="model">商品项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract int Update(Model.FeProductItem model);

        /// <summary>
        /// 删除商品项
        /// </summary>
        /// <param name="sysNo">广告项主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);
        /// <summary>
        /// 同步总部已审核的商品项
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public abstract int ProCreateFeProductItem(int GroupSysNo, int DealerSysNo, int CreatedBy);
    }
}
