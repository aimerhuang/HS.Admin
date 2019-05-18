using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 商品信息通知接口类
    /// </summary>
    /// <remarks>2013-08-09 杨晗 创建</remarks>
    public abstract class ICrNoticeDao : DaoBase<ICrNoticeDao>
    {
        /// <summary>
        /// 根据商品信息通知编号获取实体
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>商品信息通知实体</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract CrNotice GetModel(int sysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">商品信息通知类型</param>
        /// <param name="type">商品信息通知类型</param>
        /// <param name="count">抛出总条数</param>
        /// <returns>商品信息通知列表</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract IList<CBCrNotice> Seach(int pageIndex, int pageSize,int customerSysNo, CustomerStatus.通知类型 type,
                                                        out int count);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract int Insert(CrNotice model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract int Update(CrNotice model);
         /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public abstract int Update(string sysNoItems);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="CustomerSysNo">申请人</param>
        /// <param name="ProductSysNo">申请商品ID</param>
        /// <param name="type">通知方式</param>
        /// <param name="NoticeWay">发送方式</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏创建</returns>
        public abstract IList<CBCrNotice> List(int CustomerSysNo, int ProductSysNo, CustomerStatus.通知类型 type, int NoticeWay, Pager<CBCrNotice> pager);

        public abstract CBCrNotice Get();
    }
}
