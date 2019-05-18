
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 商品关联关系对应表
    /// </summary>
    /// <remarks>2013-09-13  朱成果 创建</remarks>
    public abstract class IDsProductAssociationDao : DaoBase<IDsProductAssociationDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract int Insert(DsProductAssociation entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract void Update(DsProductAssociation entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract DsProductAssociation GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

       /// <summary>
       /// 获取分销产品和商城产品的对应关系
       /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
       /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract DsProductAssociation GetEntity(int dealerMallSysNo, string mallProductId);

        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public abstract CBDsProductAssociation GetHytProduct(int dealerMallSysNo, string mallProductId);

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public abstract void GetDealerMallProductList(ref Pager<CBPdProductDetail> pager, int dealerMallSysNo, ParaProductFilter condition);

        /// <summary>
        /// 更新商品状态值
        /// </summary>
        /// <param name="SysNo">商品关系编号</param>
        /// <param name="Status">状态值</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发  创建</remarks>
        public abstract void UpdateDealerMallProductStatus(int SysNo, int Status);
    }
}
