using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 商品组接口抽象类
    /// </summary>
    /// <remarks>2013－06-20 苟治国 创建</remarks>
    public abstract class IFeProductGroupDao : DaoBase<IFeProductGroupDao>
    {
        /// <summary>
        /// 查看商品项
        /// </summary>
        /// <param name="sysNo">商品项编号</param>
        /// <returns>商品项</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public abstract Model.FeProductGroup GetModel(int sysNo);

        /// <summary>
        /// 验证商品组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>条数</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public abstract int FeProductGroupChk(string key, int sysNo);

        /// <summary>
        /// 根据条件获取产品组的列表
        /// </summary>
        /// <param name="pager">广告组查询条件</param>
        /// <returns>产品组列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract Pager<FeProductGroup> Seach(Pager<FeProductGroup> pager);

        /// <summary>
        /// 新增产品组
        /// </summary>
        /// <param name="model">产品组明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public abstract int Insert(Model.FeProductGroup model);

        /// <summary>
        /// 更新产品组
        /// </summary>
        /// <param name="model">产品组明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract int Update(Model.FeProductGroup model);

        /// <summary>
        /// 删除产品组
        /// </summary>
        /// <param name="sysNo">广告项编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 根据商品组编号查询商品组
        /// </summary>
        /// <param name="code">商品组编号</param>
        /// <param name="platform">商品组平台类型</param>
        /// <returns>商品组</returns>
        /// <remarks>2013－08-21 周瑜 创建</remarks>
        public abstract IList<FeProductGroup> GetModelByGroupcode(string code, ForeStatus.商品组平台类型 platform);
        /// <summary>
        /// 获取商品广告组数据
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public abstract List<CBFeProductItem> GetModelByGroupSysNo(int groupSysNo);


        /// <summary>
        /// 获取广告组商品信息内容
        /// </summary>
        /// <param name="levelSysNo"></param>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public abstract List<CBFeProductItem> GetProductInfoList(int levelSysNo, int groupSysNo);
    }
}
