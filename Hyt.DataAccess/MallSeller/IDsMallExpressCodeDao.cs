using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 分销商城快递代码
    /// </summary>
    /// <remarks>2014-03-25 唐文均 创建</remarks>
    public abstract class IDsMallExpressCodeDao : DaoBase<IDsMallExpressCodeDao>
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型</param>
        /// <param name="deliveryType">快递方式</param>
        /// <returns>分销商城快递代码</returns>
        /// <remarks>2014-03-25 唐文均 创建</remarks>
        public abstract DsMallExpressCode GetEntity(int mallTypeSysNo, int deliveryType);

        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        public abstract DsMallExpressCode Get(int sysNo);

        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <param name="expressCode">第三方快递代码</param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public abstract DsMallExpressCode Get(int mallTypeSysNo, int deliveryType, string expressCode);

        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="deliveryType">配送方式系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-20 罗勤瑶 创建</remarks>
        public abstract DsMallExpressCode Get(int mallTypeSysNo, int deliveryType);

        /// <summary>
        /// 查询经销商城快递代码
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>经销商城快递代码分页数据</returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public abstract Model.Pager<Model.Transfer.CBDsMallExpressCode> Query(Model.Parameter.ParaDsMallExpressCodeFilter filter);

        /// <summary>
        /// 插入分销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public abstract int Insert(DsMallExpressCode model);

        /// <summary>
        /// 更新分销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public abstract int Update(DsMallExpressCode model);

        /// <summary>
        /// 根据sysNo删除DsMallExpressCode表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public abstract int Delete(int sysNo);
    }
}
