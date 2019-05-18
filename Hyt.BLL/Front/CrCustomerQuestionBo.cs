using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 会员咨询业务层
    /// </summary>
    /// <remarks>2013-06-20 苟治国 创建</remarks>
    public class CrCustomerQuestionBo : BOBase<CrCustomerQuestionBo>
    {
        /// <summary>
        /// 查看会员咨询 
        /// </summary>
        /// <param name="sysNo">会员咨询编号</param>
        /// <returns>会员咨询</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public CrCustomerQuestion GetModel(int sysNo)
        {
            return ICrCustomerQuestionDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 集合查看商品咨询 
        /// </summary>
        /// <param name="sysNo">会员咨询编号</param>
        /// <returns>会员咨询</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public CBCrCustomerQuestion GetGroupModel(int sysNo)
        {
            return ICrCustomerQuestionDao.Instance.GetGroupModel(sysNo);
        }

        /// <summary>
        /// 获取会员咨询列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="pageIndex">页面</param>
        /// <returns>会员咨询列表</returns>
        /// <remarks>
        /// 2013-08-21 郑荣华 创建
        /// </remarks>
        public IList<CBCrCustomerQuestion> GetListByPdSysNo(int productSysNo, int pageIndex)
        {
            var pager = new Pager<CBCrCustomerQuestion> {CurrentPage = pageIndex};

            var filter = new Model.Parameter.ParaCrCustomerQuestionFilter { ProductSysNo = productSysNo, Status = (int)CustomerStatus.会员咨询状态.已回复 };

            ICrCustomerQuestionDao.Instance.GetCrCustomerQuestionList(ref pager, filter);

            return pager.Rows;
        }

        /// <summary>
        /// 获取会员咨询列表数量
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>      
        /// <returns>会员咨询列表数量</returns>
        /// <remarks>
        /// 2013-08-21 郑荣华 创建
        /// </remarks>
        public int GetListCountByPdSysNo(int productSysNo)
        {
            var pager = new Pager<CBCrCustomerQuestion> { CurrentPage = 1 };

            var filter = new Model.Parameter.ParaCrCustomerQuestionFilter { ProductSysNo = productSysNo };

            ICrCustomerQuestionDao.Instance.GetCrCustomerQuestionList(ref pager, filter);

            return pager.TotalRows;
        }

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
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束时期</param>
        /// <returns>会员咨询列表</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public IList<Model.CBCrCustomerQuestion> Seach(int pageIndex, int pageSize, int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null)
        {
            return ICrCustomerQuestionDao.Instance.Seach(pageIndex, pageSize, type, staus, customersysno, productSysNo, productName, beginDate, endDate);
        }

        /// <summary>
        /// 根据商品项条件获取产品组的总条数
        /// </summary>
        /// <param name="type">会员咨询类型</param>
        /// <param name="staus">会员咨询状态</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int GetCount(int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null)
        {
            return ICrCustomerQuestionDao.Instance.GetCount(type, staus, customersysno, productSysNo, productName, beginDate, endDate);
        }

        /// <summary>
        /// 新增会员咨询(后台使用)
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public int Insert(CrCustomerQuestion model)
        {
            int result = ICrCustomerQuestionDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("创建会员咨询{0}",model.SysNo), LogStatus.系统日志目标类型.商品咨询, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 更新会员咨询
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public int Update(Model.CrCustomerQuestion model)
        {
            int result = ICrCustomerQuestionDao.Instance.Update(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改会员咨询{0}",model.SysNo), LogStatus.系统日志目标类型.商品咨询, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 删除会员咨询
        /// </summary>
        /// <param name="sysNo">会员咨询编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public bool Delete(int sysNo)
        {
            return ICrCustomerQuestionDao.Instance.Delete(sysNo);
        }
        /// <summary>
        /// 获得对应状态的商品咨询数目
        /// </summary>
        /// <param name="Status">状态</param>
        /// <returns>数目</returns>
        /// <remarks>2016-03-19 王耀发 创建</remarks>
        public int GetCusQuestionCounts(int Status)
        {
            return ICrCustomerQuestionDao.Instance.GetCusQuestionCounts(Status);
        }
        /// <summary>
        /// 分页获取申请列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>
        public void GetDsDealerApplyWebList(ref Pager<DsDealerApplyWeb> pager, ParaDsDealerApplyWebFilter filter)
        {
            ICrCustomerQuestionDao.Instance.GetDsDealerApplyWebList(ref pager, filter);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        public void UpdateDsDealerApplyWeStatus(int SysNo, int Status)
        {
            ICrCustomerQuestionDao.Instance.UpdateDsDealerApplyWeStatus(SysNo, Status);
        }
    }
}
