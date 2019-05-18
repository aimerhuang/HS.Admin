using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Union
{
    /// <summary>
    /// WeChat Dao
    /// </summary>
    /// <remarks>黄伟 2013-10-23 创建</remarks>
    public abstract class IWeChatDao:DaoBase<IWeChatDao>
    {
        #region 微信自动回复配置

        /// <summary>
        /// create
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <returns>SysNo of the created</returns>
        /// <remarks>黄伟 2013-10-23 创建</remarks>
        public abstract int CreateConfiguration(MkWeixinConfig model);

        /// <summary>
        ///     update
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <remarks>黄伟 2013-11-15 创建</remarks>
        public abstract int UpdateConfiguration(MkWeixinConfig model);

        /// <summary>
        /// 获取微信自动回复配置
        /// </summary>
        /// <param name=" "></param>
        /// <returns>MkWeixinConfig</returns>
        /// <remarks>2013-11-14 陶辉 创建</remarks>
        public abstract MkWeixinConfig GetMkWeixinConfig();

        /// <summary>
        /// 获取对应分销商微信自动回复配置
        /// </summary>
        /// <returns>MkWeixinConfig</returns>
        /// <remarks>2013-11-14 陶辉 创建</remarks>
        public abstract MkWeixinConfig GetMkWeixinConfig(int AgentSysNo, int DealerSysNo);

        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2016-04-28 王耀发 创建
        /// </remarks>
        public abstract void GetMkWeixinConfigList(ref Pager<CBMkWeixinConfig> pager, ParaMkWeixinConfigFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public abstract MkWeixinConfig GetEntity(int SysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Insert(MkWeixinConfig entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(MkWeixinConfig entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);
        #endregion

        #region 微信关键字

        /// <summary>
        /// query 微信关键字列表
        /// </summary>
        /// <param name="para">MkWeixinKeywords</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywords</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public abstract Dictionary<int, List<CBMkWeixinKeywords>> QueryKeyWords(ParaMkWeixinKeywords para, int id = 1, int pageSize = 10);

        /// <summary>
        /// 根据系统编号获取MkWeixinKeywords
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>MkWeixinKeywords</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract MkWeixinKeywords GetMkWeixinKeywordsBySysNo(int sysNo);

        /// <summary>
        /// 根据关键词系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract List<MkWeixinKeywordsReply> GetContentByKeyWords(int sysNo);

        /// <summary>
        /// 新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param> 
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract int CreateKeyWords(MkWeixinKeywords model, int operatorSysNo);

        /// <summary>
        /// 更新微信关键字
        /// </summary>
        /// <param name="models">list of MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void UpdateKeyWords(List<MkWeixinKeywords> models, int operatorSysNo);

        /// <summary>
        /// 删除微信关键字
        /// </summary>
        /// <param name="lstDelSysNos">要删除的微信关键字编号集合</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void DeleteKeyWords(List<int> lstDelSysNos);

        /// <summary>
        /// 设置微信关键字状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywords Sysno</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void SetKeyWordsStatus(int sysNo, int status, int operatorSysNo);

        #endregion

        #region 微信关键字对应内容

        /// <summary>
        /// query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public abstract Dictionary<int, List<MkWeixinKeywordsReply>> QueryKeyWordsContent(MkWeixinKeywordsReply para, int id = 1, int pageSize = 10);

        /// <summary>
        /// query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public abstract List<MkWeixinKeywordsReply> QueryKeyWordsContentAll(MkWeixinKeywordsReply para);

        /// <summary>
        /// 根据系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywordsReply</param>
        /// <returns>MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract MkWeixinKeywordsReply GetMkWeixinKeywordsReplyBySysNo(int sysNo);

        /// <summary>
        /// 新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract int CreateKeyWordsContent(MkWeixinKeywordsReply model, int operatorSysNo);

        /// <summary>
        /// 更新微信关键字对应内容
        /// </summary>
        /// <param name="models">list of MkWeixinKeywordsReply</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void UpdateKeyWordsContent(List<MkWeixinKeywordsReply> models, int operatorSysNo);

        /// <summary>
        /// 删除微信关键字
        /// </summary>
        /// <param name="lstDelSysNos">要删除的微信关键字编号集合</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void DeleteKeyWordsContent(List<int> lstDelSysNos);

        /// <summary>
        /// 设置微信关键字回复内容条目状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywords Sysno</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract void SetKeyWordsContentStatus(int sysNo, int status, int operatorSysNo);

        /// <summary>
        /// 检查关键词是否存在
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <returns>true:exist;false:not  exist</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public abstract bool IsKeyWordsExist(string keyWords);

        #endregion

        #region for interface method in iwechatbo
        
        /// <summary>
        /// 根据关键词获取回复内容
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <returns>MkWeixinKeywordsReply</returns>
        /// <remarks>2013-11-04 黄伟 创建</remarks>
        public abstract List<MkWeixinKeywordsReply> GetMkWeixinReplyByKeyWords(string keyWords);

        #endregion
    }
}
