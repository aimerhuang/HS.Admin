using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 友情链接接口抽象类
    /// </summary>
    /// <remarks>2013-12-09 苟治国 创建</remarks>
    public abstract class IMkBlogrollDao : DaoBase<IMkBlogrollDao>
    {
        /// <summary>
        /// 查看友情链接
        /// </summary>
        /// <param name="sysNo">友情链接编号</param>
        /// <returns>友情链接</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public abstract Model.MkBlogroll GetModel(int sysNo);

        /// <summary>
        /// 判断友情连接标题是否重复
        /// </summary>
        /// <param name="key">友情连接标题</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public abstract bool Verify(string key, int sysNo);

        /// <summary>
        /// 根据条件获取友情链接列表
        /// </summary>
        /// <param name="pager">友情链接查询条件</param>
        /// <returns>友情链接列表</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public abstract Pager<Model.MkBlogroll> Seach(Pager<MkBlogroll> pager);

        /// <summary>
        /// 插入友情链接
        /// </summary>
        /// <param name="model">友情链接明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public abstract int Insert(Model.MkBlogroll model);

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public abstract int Update(Model.MkBlogroll model);
    }
}
