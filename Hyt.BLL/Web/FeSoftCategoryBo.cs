using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 软件分类业务逻辑层
    /// </summary>
    /// <remarks>2014-01-20 苟治国 创建</remarks>
    public class FeSoftCategoryBo : BOBase<FeSoftCategoryBo>
    {
        /// <summary>
        /// 获取指定编号的软件分类信息
        /// </summary>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-20 苟治国 创建</remarks>
        public FeSoftCategory GetEntity(int sysNo)
        {
            return IFeSoftCategoryDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="pager">软件分类查询条件</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-20 苟治国 创建</remarks>
        public Pager<FeSoftCategory> GetFeSoftCategoryList(Pager<FeSoftCategory> pager)
        {
            return IFeSoftCategoryDao.Instance.GetFeSoftCategoryList(pager);
        }
    }
}
