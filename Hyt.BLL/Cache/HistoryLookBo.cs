using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util;

namespace Hyt.BLL.Cache
{
    /// <summary>
    /// 浏览历史记录
    /// </summary>
    /// <remarks>2013-03-18 吴文强 创建</remarks>
    public class HistoryLookBo : BOBase<HistoryLookBo>
    {
        private const string COOKIE_HISTORYLOOK = "HistoryLook";

        /// <summary>
        /// [Cookie]设置产品浏览记录(写入Cookie)
        /// </summary>
        /// <param name="productSysNo">产品Id</param>
        /// <param name="productName">产品名称</param>
        /// <param name="count">浏览记录总数</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-03-18 吴文强 创建
        /// 2013-08-08 邵  斌 迁移,添加商品图片路径到cooki
        /// </remarks>
        public void SetHistoryLook(int productSysNo, string productName,int count,string productImage = null)
        {
            //清除名字中包含的@和;
            productName = productName.Replace("@", " ").Replace(";", " ");
            //新加入的产品(产品Id@产品名称)
            var value = string.Format("{0}@{1}@{2}", productSysNo, productName.Trim(), productImage);
            //历史产品
            var hostory = CookieUtil.Get(COOKIE_HISTORYLOOK);
            //新构造的产品集合(产品Id@产品名称),新添加的记录写入最后
            var listHostory = hostory.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            listHostory = listHostory.Where(
                s => s.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries)[0] != productSysNo.ToString()).ToList();
            if (listHostory.Count >= count)
            {
                listHostory.RemoveAt(0);
            }
            listHostory.Add(value);
            CookieUtil.SetCookie(COOKIE_HISTORYLOOK, string.Join(";", listHostory), DateTime.Now.AddYears(1));
        }

        /// <summary>
        /// [Cookie]获取产品浏览记录(cookie中获取)
        /// </summary>
        /// <param name="count">浏览记录总数</param>
        /// <returns>浏览记录集合</returns>
        /// <remarks>
        /// 2013-03-18 吴文强 创建
        /// 2013-08-08 邵  斌 迁移
        /// </remarks>
        public List<string> GetHistoryLook(int count)
        {
            //历史产品
            var hostory = CookieUtil.Get(COOKIE_HISTORYLOOK);
            var listHostory = hostory.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //倒序浏览记录
            var newHostory = new List<string>();
            for (int i = listHostory.Count - 1; i >= 0; i--)
            {
                if (newHostory.Count == count)
                {
                    break;
                }
                newHostory.Add(listHostory[i]);
            }
            return newHostory;
        }

        /// <summary>
        /// [Cookie]清除产品浏览记录
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2013-03-18 吴文强 创建
        /// 2013-08-08 邵  斌 迁移
        /// </remarks>
        public void ClearHistoryLook()
        {
            CookieUtil.SetCookie(COOKIE_HISTORYLOOK, string.Empty);
        }
    }
}