using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Hyt.BLL.Log;
using Hyt.DataAccess.Union;
using Hyt.DataAccess.Weixin;
using Hyt.Model;
using Hyt.Model.Weixin;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Union
{
    /// <summary>
    ///     微信BO
    /// </summary>
    /// <remarks>黄伟 2013-10-23 创建</remarks>
    public class WeChatBo : BOBase<WeChatBo>, IWeChatBo
    {
        private const string UrlAshx = "http://www.pisen.com.cn/tools/Security.ashx?code=";

        #region interface methods

        /// <summary>
        ///     判断产品真伪
        /// </summary>
        /// <param name="code">产品防伪编码</param>
        /// <returns>是否真品</returns>
        /// <remarks>2013-11-4 黄伟 创建</remarks>
        public Result<WeChatValidation> CheckProduct(string code)
        {
            string strRtn = null;

            try
            {
                strRtn = GetStrUsingStream(code);
            }
            catch (Exception ex)
            {
                return new Result<WeChatValidation>
                    {
                        Status = false,
                        Message = "访问ashx出现异常"
                    };
            }

            var weChatVal = new JavaScriptSerializer().Deserialize<WeChatValidation>(strRtn);
            return new Result<WeChatValidation>
        {
            Status = true,
            Data = weChatVal
        };
        }

        /// <summary>
        /// using WebClient to read
        /// </summary>
        /// <param name="urlAshx">url of ashx</param>
        /// <returns>string</returns>
        /// <remarks>2013-11-4 黄伟 创建</remarks>
        public string GetStrUsingWebClient(string urlAshx)
        {
            var client = new WebClient();
            client.Encoding = Encoding.Unicode;
            var s = new WebClient().DownloadString(urlAshx); //not utf8
            return s;
        }

        /// <summary>
        /// using Stream to read
        /// </summary>
        /// <param name="code">code to verify</param>
        /// <returns>string</returns>
        /// <remarks>2013-11-4 黄伟 创建</remarks>
        public string GetStrUsingStream(string code = "")
        {
            var request =
                (HttpWebRequest)
                WebRequest.Create(UrlAshx + code);
            var response = (HttpWebResponse)request.GetResponse();

            // Gets the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encode);

            char[] read = new Char[256];
            // Reads 256 characters at a time into char[]     
            int count = readStream.Read(read, 0, 256); //return the chars count existed
            var sb = new StringBuilder();
            //
            sb.Append(new string(read, 0, count));
            //read the end,there is more than 256 chars
            while (count == 256)
            {
                count = readStream.Read(read, 0, 256);
                sb.Append(new String(read, 0, count));
            }

            // Releases the resources of the response.
            response.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            return sb.ToString();
        }

        /// <summary>
        ///     根据关键字获取自动回复列表
        /// </summary>
        /// <param name="content">客户咨询内容</param>
        /// <returns>自动回复列表</returns>
        /// <remarks>2013-11-4 黄伟 创建</remarks>
        public List<MkWeixinKeywordsReply> GetAutoReplys(string content)
        {
            return IWeChatDao.Instance.GetMkWeixinReplyByKeyWords(content);
        }

        #endregion

        #region 微信自动回复配置

        /// <summary>
        ///     create
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <param name="userIp">user Ip</param>
        /// <param name="operatorSysno">operator Sysno</param>
        /// <returns>SysNo of the created</returns>
        /// <remarks>黄伟 2013-10-23 创建</remarks>
        public Result<int> Create(MkWeixinConfig model, string userIp, int operatorSysno)
        {
            var sysNo = model.SysNo;
            try
            {
                var weChatConfig = IWeChatDao.Instance.GetMkWeixinConfig(model.AgentSysNo,model.DealerSysNo);
                if (weChatConfig == null)
                {
                    model.CreatedBy = operatorSysno;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = operatorSysno;
                    model.LastUpdateDate = DateTime.Now;
                    sysNo = IWeChatDao.Instance.CreateConfiguration(model);
                    return new Result<int> { Data = sysNo, Status = true, Message = "保存成功!" };
                }
                //exist: timestamp changed
                if (!model.LastUpdateDate.Equals(weChatConfig.LastUpdateDate))
                {
                    return new Result<int> { Data = 0, Status = false, Message = string.Format("保存失败:{0}", "数据已被更改,请刷新后重试.") };
                }
                //update
                if (weChatConfig != null)
                {
                    model.LastUpdateBy = operatorSysno;
                    model.LastUpdateDate = DateTime.Now;
                    IWeChatDao.Instance.UpdateConfiguration(model);
                }          
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "创建微信自动回复配置",
                                         LogStatus.系统日志目标类型.WeChatAutoReplyConfig, 0, ex, userIp, operatorSysno);
                return new Result<int> { Data = 0, Status = false, Message = string.Format("保存失败:{0}", ex.Message) };
            }

            return new Result<int> { Data = sysNo, Status = true, Message = "保存成功!" };
        }

        /// <summary>
        /// 获取微信自动回复配置
        /// </summary>
        /// <returns>微信自动回复配置</returns>
        /// <remarks>2013-11-14 陶辉 创建</remarks>
        public MkWeixinConfig GetWeixinConfig()
        {
            return IWeChatDao.Instance.GetMkWeixinConfig();
        }

        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2016-04-28 王耀发 创建
        /// </remarks>
        public void GetMkWeixinConfigList(ref Pager<CBMkWeixinConfig> pager, ParaMkWeixinConfigFilter filter)
        {
            IWeChatDao.Instance.GetMkWeixinConfigList(ref pager, filter);
        }

        /// <summary>
        /// 保存回复配置
        /// </summary>
        /// <param name="model">回复配置</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveMkWeixinConfig(MkWeixinConfig model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            MkWeixinConfig entity = IWeChatDao.Instance.GetEntity(model.SysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IWeChatDao.Instance.Update(model);
                r.Status = true;
            }
            else
            {
                var weChatConfig = IWeChatDao.Instance.GetMkWeixinConfig(model.AgentSysNo, model.DealerSysNo);
                if (weChatConfig == null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user.SysNo;
                    model.LastUpdateBy = user.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    IWeChatDao.Instance.Insert(model);
                    r.Status = true;
                }
                else
                {
                    r.Message = "已存在该分销商的记录";
                    r.Status = false;
                }

            }
            return r;
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public MkWeixinConfig GetEntity(int SysNo)
        {
            return IWeChatDao.Instance.GetEntity(SysNo);
        }
        /// <summary>
        /// 删除国家
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IWeChatDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        #endregion

        #region 微信关键字

        /// <summary>
        ///     query 微信关键字列表
        /// </summary>
        /// <param name="para">MkWeixinKeywords</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywords</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public Dictionary<int, List<CBMkWeixinKeywords>> QueryKeyWords(ParaMkWeixinKeywords para, int id = 1,
                                                                     int pageSize = 10)
        {
            return IWeChatDao.Instance.QueryKeyWords(para, id, pageSize);
        }

        /// <summary>
        ///     根据系统编号获取MkWeixinKeywords
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>MkWeixinKeywords</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public MkWeixinKeywords GetMkWeixinKeywordsBySysNo(int sysNo)
        {
            return IWeChatDao.Instance.GetMkWeixinKeywordsBySysNo(sysNo);
        }

        /// <summary>
        ///     根据关键词系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public List<MkWeixinKeywordsReply> GetContentByKeyWords(int sysNo)
        {
            return IWeChatDao.Instance.GetContentByKeyWords(sysNo);
        }

        /// <summary>
        ///     新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result<int> CreateKeyWords(MkWeixinKeywords model, string userIp, int operatorSysno)
        {
            if (IsKeyWordsExist(model.Keywords))
            {
                return new Result<int> { Message = "关键词已存在,请重新输入!", Status = false };
            }
            int sysNo = 0;
            try
            {
                sysNo = IWeChatDao.Instance.CreateKeyWords(model, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "新增微信关键字",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result<int> { Message = string.Format("保存失败:{0}", ex.Message), Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "新增微信关键字",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result<int> { Data = sysNo, Message = "保存成功!", Status = true };
        }

        /// <summary>
        ///     删除微信关键字
        /// </summary>
        /// <param name="lstDelSysnos">要删除的微信关键字编号集合</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result DeleteKeyWords(List<int> lstDelSysnos, string userIp, int operatorSysno)
        {
            try
            {
                IWeChatDao.Instance.DeleteKeyWords(lstDelSysnos);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除微信关键字",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result { Message = string.Format("删除失败:{0}", ex.Message), Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除微信关键字",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result { Message = "删除成功!", Status = true };
        }

        /// <summary>
        ///     更新微信关键字
        /// </summary>
        /// <param name="models">list of MkWeixinKeywords</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result UpdateKeyWords(List<MkWeixinKeywords> models, string userIp, int operatorSysno)
        {
            var flagModified = false;

            try
            {
                models.ForEach(p =>
                    {
                        var modelNow = GetMkWeixinKeywordsBySysNo(p.SysNo);
                        //deleted or updated-time comparing
                        if (modelNow == null || !p.LastUpdateDate.Equals(modelNow.LastUpdateDate))
                        {
                            flagModified = true;
                        }
                    });
                if (flagModified)
                    return new Result { Message = "保存失败!数据已更改,请重新获取后再试!", Status = false };

                IWeChatDao.Instance.UpdateKeyWords(models, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新微信关键字",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result { Message = "更新失败!", Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新微信关键字",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result { Message = "更新成功!", Status = true };
        }

        /// <summary>
        ///     设置微信关键字状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywords SysNo</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result SetKeyWordsStatus(int sysNo, int status, string userIp, int operatorSysNo)
        {
            try
            {
                IWeChatDao.Instance.SetKeyWordsStatus(sysNo, status, operatorSysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新微信关键字状态",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysNo);
                return new Result { Message = "更新失败!", Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新微信关键字状态",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysNo);

            return new Result { Message = "更新成功!", Status = true };
        }

        #endregion

        #region 微信关键字对应内容

        /// <summary>
        ///     query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public Dictionary<int, List<MkWeixinKeywordsReply>> QueryKeyWordsContent(MkWeixinKeywordsReply para,
                                                                                 int id = 1, int pageSize = 10)
        {
            return IWeChatDao.Instance.QueryKeyWordsContent(para, id, pageSize);
        }

        /// <summary>
        ///     query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public List<MkWeixinKeywordsReply> QueryKeyWordsContentAll(MkWeixinKeywordsReply para)
        {
            return IWeChatDao.Instance.QueryKeyWordsContentAll(para);
        }

        /// <summary>
        ///     根据系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywordsReply</param>
        /// <returns>MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public MkWeixinKeywordsReply GetMkWeixinKeywordsReplyBySysNo(int sysNo)
        {
            return IWeChatDao.Instance.GetMkWeixinKeywordsReplyBySysNo(sysNo);
        }

        /// <summary>
        ///     新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result<int> CreateKeyWordsContent(MkWeixinKeywordsReply model, string userIp, int operatorSysno)
        {
            var sysNo = 0;
            try
            {
                sysNo = IWeChatDao.Instance.CreateKeyWordsContent(model, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "新增微信关键字对应内容",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result<int> { Message = string.Format("保存失败:{0}", ex.Message), Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "新增微信关键字对应内容",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result<int> { Data = sysNo, Message = "保存成功!", Status = true };
        }

        /// <summary>
        ///     更新微信关键字对应内容
        /// </summary>
        /// <param name="models">list of MkWeixinKeywordsReply</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result Entity</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result UpdateKeyWordsContent(List<MkWeixinKeywordsReply> models, string userIp, int operatorSysno)
        {
            var flagModified = false;

            try
            {
                models.ForEach(p =>
                    {
                        var modelNow = GetMkWeixinKeywordsReplyBySysNo(p.SysNo);
                        //deleted or updated-time comparing
                        //if (modelNow == null || !p.LastUpdateDate.Equals(modelNow.LastUpdateDate))
                        if (modelNow == null || !(p.LastUpdateDate.ToString() == modelNow.LastUpdateDate.ToString()))
                        {
                            flagModified = true;
                        }
                    });
                if (flagModified)
                    return new Result { Message = "保存失败!数据已更改,请重新获取后再试!", Status = false };

                IWeChatDao.Instance.UpdateKeyWordsContent(models, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新微信关键字对应内容",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result { Message = "更新失败!", Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新微信关键字对应内容",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result { Message = "更新成功!", Status = true };
        }

        /// <summary>
        ///     删除微信关键字
        /// </summary>
        /// <param name="lstDelSysnos">要删除的微信关键字编号集合</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result DeleteKeyWordsContent(List<int> lstDelSysnos, string userIp, int operatorSysno)
        {
            try
            {
                IWeChatDao.Instance.DeleteKeyWordsContent(lstDelSysnos);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除微信关键字对应内容",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result { Message = "删除失败!", Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除微信关键字对应内容",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result { Message = "删除成功!", Status = true };
        }

        /// <summary>
        ///     设置微信关键字回复内容条目状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywordsReply Sysno</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public Result SetKeyWordsContentStatus(int sysNo, int status, string userIp, int operatorSysno)
        {
            try
            {
                IWeChatDao.Instance.SetKeyWordsContentStatus(sysNo, status, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新微信关键字对应内容的状态",
                                         LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, ex, userIp, operatorSysno);
                return new Result { Message = "更新失败!", Status = false };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新微信关键字对应内容的状态",
                                     LogStatus.系统日志目标类型.WeChatKeyWordsMgm, 0, null, userIp, operatorSysno);

            return new Result { Message = "更新成功!", Status = true };
        }

        /// <summary>
        ///     检查关键词是否存在
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <returns>true:exist;false:not  exist</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public bool IsKeyWordsExist(string keyWords)
        {
            return IWeChatDao.Instance.IsKeyWordsExist(keyWords);
        }

        #endregion

        /// <summary>
        /// 微信咨询消息状态更新
        /// </summary>
        /// <param name="weixinId">微信号</param>
        /// <param name="status">微信咨询消息状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public int UpdateStatus(string weixinId, MarketingStatus.微信咨询消息状态 status = MarketingStatus.微信咨询消息状态.已读)
        {
            return IMkWeixinQuestionDao.Instance.UpdateStatus(weixinId, status);
        }

        #region 查询

        /// <summary>
        /// 根据微信号获取微信咨询信息
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="weixinId">微信号</param>
        /// <returns>微信咨询列表信息</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public void GetMkWeixinQuestionList(ref Pager<CBMkWeixinQuestion> pager, string weixinId)
        {
            IMkWeixinQuestionDao.Instance.GetMkWeixinQuestionList(ref pager, weixinId);

            if (pager.Rows != null)
            {
                foreach (var item in pager.Rows)
                {
                    var userInfo = Hyt.BLL.Weixin.CallCenterReplyBo.Instance.GetUserInfo(item.WeixinId);
                    item.ShowName = userInfo == null ? item.WeixinId : userInfo.nickname;
                }
            }
        }

        /// <summary>
        /// 分页查询微信咨询信息（统计）列表
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks> 
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public void GetMkWeixinQuestionStaticsList(ref Pager<CBMkWeixinQuestion> pager,
                                                   ParaMkWeixinQuestionFilter filter)
        {
            IMkWeixinQuestionDao.Instance.GetMkWeixinQuestionStaticsList(ref pager, filter);

            if (pager.Rows != null)
            {
                foreach (var item in pager.Rows)
                {
                    var userInfo = Hyt.BLL.Weixin.CallCenterReplyBo.Instance.GetUserInfo(item.WeixinId);
                    item.ShowName = userInfo == null ? item.WeixinId : userInfo.nickname;
                }

                #region 微信ID展示为实际昵称，数据库里记录的为OpenId，查询需特殊处理  2013-12-31 陶辉 添加

                if (!string.IsNullOrEmpty(filter.WeixinId))
                {
                    var rowsList = new List<CBMkWeixinQuestion>(pager.Rows);
                    rowsList = rowsList.FindAll(r => r.ShowName == filter.WeixinId);
                    pager.Rows = rowsList;
                    pager.TotalRows = rowsList.Count;
                }

                #endregion
            }
        }
        #endregion
    }
}