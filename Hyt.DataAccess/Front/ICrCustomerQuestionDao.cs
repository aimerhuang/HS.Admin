using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 商品咨询接口抽象类
    /// </summary>
    /// <remarks>2013－06-25 苟治国 创建</remarks>
    public abstract class ICrCustomerQuestionDao : DaoBase<ICrCustomerQuestionDao>
    {
        /// <summary>
        /// 查看商品咨询 
        /// </summary>
        /// <param name="sysNo">商品咨询编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public abstract Model.CrCustomerQuestion GetModel(int sysNo);

        /// <summary>
        /// 集合查看商品咨询 
        /// </summary>
        /// <param name="sysNo">商品咨询编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public abstract Model.CBCrCustomerQuestion GetGroupModel(int sysNo);

        /// <summary>
        /// 查询会员咨询列表
        /// </summary>
        /// <param name="pager">会员咨询列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-08-21 郑荣华 创建
        /// </remarks>
        public abstract void GetCrCustomerQuestionList(ref Pager<CBCrCustomerQuestion> pager,
                                                       Hyt.Model.Parameter.ParaCrCustomerQuestionFilter filter);

        /// <summary>
        /// 根据条件获取会员咨询的列表
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="pageSize">页码</param>
        /// <param name="type">商品组编号</param>
        /// <param name="staus">状态</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>会员咨询列表</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public abstract IList<Model.CBCrCustomerQuestion> Seach(int pageIndex, int pageSize, int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null);

        /// <summary>
        /// 根据条件获取会员咨询的总条数
        /// </summary>
        /// <param name="type">会员咨询类型</param>
        /// <param name="staus">会员咨询状态</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public abstract int GetCount(int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null);

        /// <summary>
        /// 新增会员咨询
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public abstract int Insert(Model.CrCustomerQuestion model);

        /// <summary>
        /// 更新会员咨询
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public abstract int Update(Model.CrCustomerQuestion model);

        /// <summary>
        /// 删除会员咨询
        /// </summary>
        /// <param name="ID">商品咨询主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public abstract bool Delete(int ID);

        /// <summary>
        /// 获得对应状态的商品咨询数目
        /// </summary>
        /// <param name="Status">状态</param>
        /// <returns>数目</returns>
        /// <remarks>2016-03-19 王耀发 创建</remarks>
        public abstract int GetCusQuestionCounts(int Status);
        /// <summary>
        /// 分页获取申请列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>
        public abstract void GetDsDealerApplyWebList(ref Pager<DsDealerApplyWeb> pager, ParaDsDealerApplyWebFilter filter);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        public abstract void UpdateDsDealerApplyWeStatus(int SysNo, int Status);
    }
}
