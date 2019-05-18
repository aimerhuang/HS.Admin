using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Service.Contract;

namespace Hyt.Service.Implement
{
    /// <summary>
    /// 二维码扫描后显示的网站服务
    /// </summary>
    /// <remarks>
    /// 2013-10-24 郑荣华 创建
    /// </remarks>
    public class TwoDimensionCode : BaseService, ITwoDimensionCode
    {
        /// <summary>
        /// 获取app下载地址，依次为 商城Android，商城Ios，百城通
        /// </summary>
        /// <returns>最新版本列表</returns>
        /// <remarks>
        /// 2013-10-24 郑荣华 创建
        /// </remarks>
        public IList<string> GetAppUrl()
        {
            var list = Hyt.BLL.AppContent.AppContentBo.Instance.GetLastAppVersion();
            var android = list.SingleOrDefault(p => p.AppCode == (int)Hyt.Model.WorkflowStatus.AppStatus.App代码.商城Android);
            var ios = list.SingleOrDefault(p => p.AppCode == (int)Hyt.Model.WorkflowStatus.AppStatus.App代码.商城Ios);
            //var citypass = list.SingleOrDefault(p => p.AppCode == (int)Hyt.Model.WorkflowStatus.AppStatus.App代码.百城通);
            IList<string> rlist = new List<string>();
            rlist.Add(android == null ? "" : android.VersionLink);
            rlist.Add(ios == null ? "" : ios.VersionLink);
            //rlist.Add(citypass == null ? "" : citypass.VersionLink);

            return rlist;
        }

        /// <summary>
        /// 获取推荐周边产品，直接调用Product.svc的GetShake
        /// </summary>
        /// <param name="brand">手机品牌</param>
        /// <param name="type">手机型号</param>
        /// <returns>商品信息</returns>
        /// <remarks>
        /// 2013-10-12 郑荣华 创建
        /// </remarks>
        public ResultPager<IList<SimplProduct>> GetRelationProduct(string brand, string type)
        {
            var pd = new B2CApp.Product();
            return pd.GetShake(brand, type);
        }
    }
}
