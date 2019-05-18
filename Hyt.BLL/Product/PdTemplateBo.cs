using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Product;
using Hyt.Model.Transfer;
using Hyt.BLL.Authentication;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品描述模板业务逻辑类
    /// </summary>
    /// <remarks>2013-07-22 杨晗 创建</remarks>
    public class PdTemplateBo : BOBase<PdTemplateBo>
    {
        /// <summary>
        /// 根据商品描述模板系统编号获取模型
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>商品描述模板实体</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public PdTemplate GetModel(int sysNo)
        {
            var model = new PdTemplate();
            if (IPdTemplateDao.Instance.GetModel(sysNo)!=null)
            {
                model = IPdTemplateDao.Instance.GetModel(sysNo);
            }
            return model;
        }

        /// <summary>
        /// 判断商品描述模板名称是否重复
        /// </summary>
        /// <param name="name">商品描述模板名称</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public bool PdTemplateVerify(string name)
        {
            return IPdTemplateDao.Instance.PdTemplateVerify(name);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="type">商品描述模板类型</param>
        /// <param name="searchName">商品描述模板名称</param>
        /// <returns>商品描述模板列表</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public PagedList<CBPdTemplate> Seach(int? pageIndex, ProductStatus.商品描述模板类型? type,
                                       string searchName=null)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBPdTemplate>();
            int count;
            var list = IPdTemplateDao.Instance.Seach((int)pageIndex, model.PageSize, type,out count ,searchName );
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int)pageIndex;
            return model;
        }

        /// <summary>
        /// 插入商品描述模板
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public int Insert(PdTemplate model)
        {
            int i= IPdTemplateDao.Instance.Insert(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "新增商品描述模块/模版"+model.Name, LogStatus.系统日志目标类型.商品调价申请, i, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return i;
        }

        /// <summary>
        /// 更新商品描述模板
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public int Update(PdTemplate model)
        {
            int u= IPdTemplateDao.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                "修改商品描述模块/模版" + model.Name, LogStatus.系统日志目标类型.商品调价申请, u, null, "",
                                Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除商品描述模板
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return IPdTemplateDao.Instance.Delete(sysNo);
        }
    }
}
