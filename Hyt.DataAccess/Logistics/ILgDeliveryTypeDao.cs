using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.ExpressList;
namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 配送方式抽象类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public abstract class ILgDeliveryTypeDao : DaoBase<ILgDeliveryTypeDao>
    {
        #region 操作

        /// <summary>
        /// 创建配送方式
        /// </summary>
        /// <param name="model">配送方式实体</param>
        /// <returns>创建的配送方式sysNo</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract int Create(LgDeliveryType model);

        /// <summary>
        /// 更新配送方式
        /// </summary>
        /// <param name="model">配送方式实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract int Update(LgDeliveryType model);

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="sysNo">要删除的配送方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        #endregion

        #region 查询

        /// <summary>
        /// 获取配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager);

        /// <summary>
        /// 查询配送方式列表
        /// </summary>
        /// <param name="pager">配送方式列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public abstract void GetLgDeliveryTypeList(ref Pager<CBLgDeliveryType> pager, ParaDeliveryTypeFilter filter);

        /// <summary>
        /// 根据名称获取配送方式信息
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <returns>单个配送方式信息</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract LgDeliveryType GetLgDeliveryType(string deliveryTypeName);

        /// <summary>
        /// 除去传入的系统编号，根据名称获取其它配送方式信息
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <param name="sysNo">除去的配送方式系统</param>
        /// <returns>单个配送方式信息</returns>
        /// <remarks> 
        /// 2013-07-02 郑荣华 创建
        /// </remarks>
        public abstract LgDeliveryType GetLgDeliveryTypeForUpdate(string deliveryTypeName,int sysNo);

        /// <summary>
        /// 获取子配送方式
        /// </summary>
        /// <param name="sysNo">配送方式系统编号，为0时获取第一级配送方式</param>
        /// <returns>子配送方式列表</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        public abstract IList<LgDeliveryType> GetSubLgDeliveryTypeList(int sysNo = 0);

        /// <summary>
        /// 根据系统编号获取配送方式
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        public abstract CBLgDeliveryType GetLgDeliveryType(int sysNo);


        /// <summary>
        /// 根据系统编号获取快递单号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 
        /// 2017-08-15 吴琨 创建
        /// </remarks>
        public abstract string GetExpressNo(int sysNo);

        /// <summary>
        /// 根据系统编号获取快递单号，时间，配送方式名称
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2018-1-10 廖移凤 创建
        /// </remarks>
        public abstract KuaiDi GetKuaidi(int sysNo);

        /// <summary>
        /// 根据出库单号获取订单编号
        /// </summary>
        /// <param name="sysNo">出库单号</param>
        /// <returns>获取订单编号</returns>
        /// <remarks> 
        /// 2017-07-10 罗熙 创建
        /// </remarks>
        public abstract string GetorderId(int sysNo);
        /// <summary>
        /// 获取仓库与配送方式对应关系信息
        /// 最后放到对应类，目前未建类
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>仓库与配送方式对应关系列表</returns>
        /// <remarks> 
        /// 2013-06-26 郑荣华 创建
        /// </remarks>
        public abstract IList<WhWarehouseDeliveryType> GetWhWarehouseDeliveryType(int deliveryTypeSysNo);

        /// <summary>
        /// 根据仓库编号获取配送方式信息
        /// </summary>
        /// <param name="wareshouSysNo">仓库系统编号</param>
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-06-28 周瑜 创建
        /// </remarks>
        public abstract IList<LgDeliveryType> GetLgDeliveryTypeByWarehouse(int wareshouSysNo);

        /// <summary>
        /// 查询所有的配送方式
        /// </summary>     
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-08-08 周瑜 创建
        /// </remarks>
        public abstract IList<LgDeliveryType> GetLgDeliveryTypeList();

        /// <summary>
        /// 查询所有的父级配送方式
        /// </summary>     
        /// <returns>父级配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-09-18 黄伟 创建
        /// </remarks>
        public abstract IList<LgDeliveryType> GetLgDeliveryTypeParent();

        #endregion


        public abstract LgDeliveryType GetDeliveryTypeByCode(string typeCode);
    }
}
