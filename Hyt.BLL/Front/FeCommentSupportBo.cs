using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Front;
using Hyt.Model;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 评论支持业务逻辑类
    /// </summary>
    /// <remarks>2013-08-27 杨晗 创建</remarks>
    public class FeCommentSupportBo : BOBase<FeCommentSupportBo>
    {
        /// <summary>
        /// 增加评论支持
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>成功系统号记录</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public int Insert(FeCommentSupport model)
        {
            return IFeCommentSupportDao.Instance.Insert(model);
        }

        /// <summary>
        /// 根据评论系统号和用户系统号获取评论支持
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public IList<FeCommentSupport> GetFeCommentSupport(int productCommentSysNo, int customerSysNo)
        {
            return IFeCommentSupportDao.Instance.GetFeCommentSupport(productCommentSysNo, customerSysNo);
        }

        /// <summary>
        /// 验证用户是否对该评论发表了有用
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>有true没有false</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public bool IsSupportCount(int productCommentSysNo, int customerSysNo)
        {
            var items = GetFeCommentSupport(productCommentSysNo,customerSysNo);
            if (items == null || items.Count == 0)
            {
                return false;
            }
            var item = items.FirstOrDefault(c => c.SupportCount > 0);
            return item != null;
        }

        /// <summary>
        /// 验证用户是否对该评论发表了没用
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>有true没有false</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public bool IsUnSupportCount(int productCommentSysNo, int customerSysNo)
        {
            var items = GetFeCommentSupport(productCommentSysNo, customerSysNo);
            if (items == null || items.Count == 0)
            {
                return false;
            }
            var item = items.FirstOrDefault(c => c.UnSupportCount > 0);
            return item != null;
        }

        /// <summary>
        /// 操作有用或没用(有用SupportCount传1，UnSupportCount传0，否则相反)
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>操作成功true,否则false</returns>
        public string Step(FeCommentSupport model)
        {
            var m = IFeCommentSupportDao.Instance.GetModel(model.ProductCommentSysNo, model.CustomerSysNo);
            if (model.SupportCount==1)
            {
                if (IsSupportCount(model.ProductCommentSysNo,model.CustomerSysNo))
                {
                    return "您已经顶过了";
                }
                else
                {
                    if (m!=null)
                    {
                        m.SupportCount = 1;
                        int edit= IFeCommentSupportDao.Instance.Update(m);
                        return edit > 0
                                   ? "操作成功"
                                   : "操作失败";
                    }
                    else
                    {
                       int ins= IFeCommentSupportDao.Instance.Insert(model);
                       return ins > 0
                                 ? "操作成功"
                                 : "操作失败";
                    }
                }
            }
            else
            {
                if (IsUnSupportCount(model.ProductCommentSysNo, model.CustomerSysNo))
                {
                    return "您已经踩过了";
                }
                else
                {
                    if (m != null)
                    {
                        m.UnSupportCount = 1;
                        int edit = IFeCommentSupportDao.Instance.Update(m);
                        return edit > 0
                                   ? "操作成功"
                                   : "操作失败";
                    }
                    else
                    {
                        int ins = IFeCommentSupportDao.Instance.Insert(model);
                        return ins > 0
                                  ? "操作成功"
                                  : "操作失败";
                    }
                }
            }
        }
    }
}
