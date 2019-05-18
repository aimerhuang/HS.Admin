using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.QJT;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.BLL.QJT
{
    /// <summary>
    /// 千机团串码设置
    /// </summary>
    /// <remarks>2016-02-17 杨浩 创建</remarks>    
    public class QJTProductImeiBo : BOBase<QJTProductImeiBo>
    {
        /// <summary>
        /// 添加千机团串码
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>       
        /// <remarks>2016-02-17 杨浩 创建</remarks>
        public int Add(QJTProductImei model)
        {
            return IQJTProductImeiDao.Instance.Create(model);
        }

        /// <summary>
        /// 根据商品编号判断商品是否属于千机团需要添加串码的商品
        /// </summary>
        /// <param name="productSysno">商品编号</param>
        /// <returns>是串码商品返回true,否则返回false</returns>
        /// <remarks>2016-02-18 杨浩 创建</remarks>
        public bool IsImeiProduct(int productSysno)
        {
            return IQJTProductImeiDao.Instance.IsImeiProduct(productSysno);
        }

        /// <summary>
        /// 获取设置列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2016-02-18 朱成果 创建</remarks>
        public Pager<CBQJTProductImei>  GetList(ParProductImeiFilter filter)
        {
            return IQJTProductImeiDao.Instance.GetList(filter);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <returns></returns>
        /// <remarks>2016-02-18 朱成果 创建</remarks>
        public int Delete(int sysno)
        {
            return IQJTProductImeiDao.Instance.Delete(sysno);
        }
    }
}
