using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Front;
namespace Hyt.BLL.Web
{
    /// <summary>
    /// 新闻商品关联表业务逻辑层
    /// </summary>
    /// <remarks>2014－01-15 苟治国 创建</remarks>
    public class FeNewsProductAssociationBo : BOBase<FeNewsProductAssociationBo>
    {
        /// <summary>
        /// 根据条件获取新闻商品关联表
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <returns>新闻商品关联表列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public IList<Model.CBFeNewsProductAssociation> Seach(int newsSysNo)
        {
            return IFeNewsProductAssociationDao.Instance.Seach(newsSysNo);
        }
    }
}
