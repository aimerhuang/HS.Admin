using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Hyt.BLL.Log;
using Hyt.DataAccess.Express;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.ExpressList;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Hyt.BLL.Express
{
    public class ElectronicsSurfaceBo : BOBase<ElectronicsSurfaceBo>
    {
        #region 操作

        #region 电子面单账号-添加或更新
        /// <summary>
        /// 创建电子面单账号
        /// </summary>
        /// <param name="model">物流公司账号实体</param>
        /// <returns>是否创建成功（Result中 StatusCode==1为添加 StatusCode==2为更新）</returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public Result AddOrUpdate(LgDeliveryCompanyAccount model, ref Result result, int CurrentUserSysNo)
        {
            model.AccountName = model.AccountName.Trim();
            //创建
            if (model.SysNo == 0)
            {
                //检测名称是否存在

                bool isExist = ILgDeliveryCompanyAccountDao.Instance.IsExistName(model);
                if (isExist)
                {
                    result.Message = "该名称已存在";
                    return result;
                }

                model.CreateBy = CurrentUserSysNo;
                model.CreateDate = DateTime.Now;
                model.LastUpdateBy = CurrentUserSysNo;
                model.LastUpdateDate = DateTime.Now;

                //执行添加操作
                result.Status = ILgDeliveryCompanyAccountDao.Instance.Insert(model) > 0;
                result.StatusCode = 1;
                return result;
            }
            //更新
            else
            {
                //检测名称是否存在
                bool isExist = ILgDeliveryCompanyAccountDao.Instance.IsExistNameWithOutSelf(model);
                if (isExist)
                {
                    result.Message = "该名称已存在";
                    return result;
                }
                LgDeliveryCompanyAccount entity = ILgDeliveryCompanyAccountDao.Instance.GetEntity(model.SysNo);
                if (entity == null)
                {
                    result.Message = "未找到该记录";
                    return result;
                }
                entity.AccountName = model.AccountName;
                entity.DeliveryTypeSysNo = model.DeliveryTypeSysNo;
                entity.AccountId = model.AccountId;
                entity.AccountSecretKey = model.AccountSecretKey;
                entity.LastUpdateBy = CurrentUserSysNo;
                entity.LastUpdateDate = DateTime.Now;
                //执行更新操作
                result.Status = ILgDeliveryCompanyAccountDao.Instance.Update(entity) > 0;
                result.StatusCode = 2;
                return result;
            }

        }
        #endregion

        #region 电子面单账号-删除

        /// <summary>
        /// 删除电子面单账号
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public bool Delete(int sysNo, ref Result result)
        {
            var list = ILgStoreLogisticsCompanyDao.Instance.GetRelateWarehouseListByAccountSysNo(sysNo);
            if (list != null && list.Any())
            {
                result.Message = "该记录下存在仓库，不允许删除！";
                return false;
            }
            return ILgDeliveryCompanyAccountDao.Instance.Delete(sysNo) > 0;
        }

        #endregion

        #region 电子面单账号关联仓库-添加或移除
        /// <summary>
        /// 电子面单账号关联仓库-添加或移除
        /// </summary>
        /// <param name="AccountSysNo"></param>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="result"></param>
        /// <param name="CurrentUserSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public Result AddOrDeleteConnect(int AccountSysNo, int WarehouseSysNo, ref Result result, int CurrentUserSysNo)
        {
            LgStoreLogisticsCompany model = new LgStoreLogisticsCompany()
            {
                AccountSysNo = AccountSysNo,
                WarehouseSysNo = WarehouseSysNo,
                CreateBy = CurrentUserSysNo,
                CreateDate = DateTime.Now
            };

            var entity = ILgStoreLogisticsCompanyDao.Instance.IsExistRecord(model);
            if (entity == null)
            {
                //一个仓库只能关联一个配送方式，一个配送方式只能关联一个仓库

                //根据仓库编号返回已关联的配送方式
                var _entity = this.GetEntityByWarehouseSysNo(WarehouseSysNo);
                if (_entity != null)
                {
                    result.Status = false;
                    result.StatusCode = 3;   //该仓库已关联其他配送方式
                    result.Message = "该仓库已关联:" + _entity.AccountName;
                    return result;
                }
                else
                {
                    //执行添加操作
                    result.Status = ILgStoreLogisticsCompanyDao.Instance.Insert(model) > 0;
                    result.StatusCode = 1;   //添加操作
                    return result;
                }
            }
            else
            {
                //执行删除操作
                int result_count = ILgStoreLogisticsCompanyDao.Instance.Delete(entity.SysNo);
                result.Status = result_count > 0;
                if (result.Status)
                {
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("删除电子面单仓库关联-WarehouseSysNo：{0},AccountSysNo:{1}", entity.WarehouseSysNo, entity.AccountSysNo), LogStatus.系统日志目标类型.电子面单账号关联仓库, CurrentUserSysNo, result_count);
                }
                result.StatusCode = 2;   //删除操作

                return result;
            }
        }

        #endregion

        #endregion

        #region 查询

        #region 电子面单账号-分页列表
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="filter">查询过滤</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public Pager<CBLgDeliveryCompanyAccount> GetPagerList(ParaElectronicsSurfaceFilter filter)
        {
            return ILgDeliveryCompanyAccountDao.Instance.GetElectronicsSurfaceList(filter);
        }
        #endregion

        #region 电子面单账号-查询单个实体
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public LgDeliveryCompanyAccount QuerySingle(int sysNo)
        {
            return ILgDeliveryCompanyAccountDao.Instance.GetEntity(sysNo);
        }
        #endregion

        #region 返回配送方式
        /// <summary>
        /// 返回配送方式
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public IList<SelectListItem> DeliveryTypeList()
        {
            var list = BLL.Logistics.DeliveryTypeBo.Instance.GetLgDeliveryTypeList().Where(t => t.ParentSysNo == 3);
            IList<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in list)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.DeliveryTypeName,
                    Value = item.SysNo.ToString()
                });
            }
            return listItem;
        }

        #endregion

        #region 已关联仓库编号列表
        /// <summary>
        /// 已关联仓库编号列表
        /// </summary>
        /// <param name="accountSysNo">物流账号编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-13 王江 创建</remarks>
        public string GetRelateWarehouseListByAccountSysNo(int accountSysNo)
        {
            return string.Join(",", ILgStoreLogisticsCompanyDao.Instance.GetRelateWarehouseListByAccountSysNo(accountSysNo).Select(t => t.WarehouseSysNo).AsEnumerable());
        }

        #endregion

        #region 根据仓库编号获取-物流公司账号表实体
        /// <summary>
        /// 根据仓库编号获取-物流公司账号表实体
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号(如果为null默认百世汇通电子面单）</param>
        /// <returns></returns>
        /// <remarks>2015-10-13 王江 创建</remarks>
        public LgDeliveryCompanyAccount GetEntityByWarehouseSysNo(int warehouseSysNo, int? deliveryTypeSysNo = null)
        {
            if (deliveryTypeSysNo == null)
            {
                deliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.百世汇通电子面单;
            }
            return ILgDeliveryCompanyAccountDao.Instance.GetEntityByWarehouseNoAndDeliveryTypeNo(warehouseSysNo, deliveryTypeSysNo.Value);
        }

        #endregion

        #endregion


        #region 对接快递100电子面单API 廖移凤 2017-11-21

        /// <summary>  
        /// 电子面单  
        /// </summary>  
        /// <returns></returns>  
        public KdOrderNums OrderTracesSubByJson(KdOrderParam pms)
        {
            return null;
            //            //电商ID  
            // string key = "ntSFshOu7046";
            ////电商加密私钥 
            // string secret = "2adc1f47936543debdf2bff890828dbb";
            ////请求url
            // string ReqURL = "http://api.kuaidi100.com/eorderapi.do?method=getElecOrder";
            //    string pm = ObjectToJson(pms);
            //    long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//时间戳
            //    Dictionary<string, string> param = new Dictionary<string, string>();
            //    string dataSign = Encrypt(pm, epoch, key,secret, "UTF-8");
            //    param.Add("sign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
            //    param.Add("key", key);
            //    param.Add("t", epoch.ToString());
            //    param.Add("param", pm);
            //    string result = SendPost(ReqURL, param);
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(result);//解析JSON字符串
            //    var result1 = jo["data"];
            //    var templateurl = result1[0]["templateurl"];
            //    KdOrderNums kn = new KdOrderNums()
            //    {
            //        destCode = result1[0]["destCode"].ToString(),
            //        destSortingCode = result1[0]["destSortingCode"].ToString(),
            //        expressCode = result1[0]["expressCode"].ToString(),
            //        kdOrderNum = result1[0]["kdOrderNum"].ToString(),
            //        kuaidinum = result1[0]["kuaidinum"].ToString(),
            //        payaccount = result1[0]["payaccount"].ToString()
            //    };
            //    if (templateurl != null)
            //    {
            //        kn.templateurl = templateurl.ToString();
            //    }
            //    return kn;




            //}
            //public static string SendPost(string url, Dictionary<string, string> param)
            //{
            //    string result = "";
            //    StringBuilder postData = new StringBuilder();
            //    if (param != null && param.Count > 0)
            //    {
            //        foreach (var p in param)
            //        {
            //            if (postData.Length > 0)
            //            {
            //                postData.Append("&");
            //            }
            //            postData.Append(p.Key);
            //            postData.Append("=");
            //            postData.Append(p.Value);
            //        }
            //    }
            //    byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            //    try
            //    {  //发送请求
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //        request.ContentType = "application/x-www-form-urlencoded";
            //        //request.Referer = url;
            //        request.Timeout = 30 * 1000;
            //        request.Method = "POST";
            //        Stream stream = request.GetRequestStream();
            //        stream.Write(byteData, 0, byteData.Length);
            //        //发送成功后接收返回的XML信息
            //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //        Stream backStream = response.GetResponseStream();
            //        StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
            //        result = sr.ReadToEnd();

            //    }
            //    catch (Exception ex)
            //    {
            //        result = ex.Message;
            //    }
            //    return result;
            //}


            ///// <summary>
            /////Sign 
            ///// <param name="content">内容</param>
            ///// <param name="epoch">时间戳</param>
            ///// <param name="keyValue">key</param>
            ///// <param name="secret">secret</param>
            ///// <param name="charset">URL编码</param>
            ///// <returns>Sign签名</returns>
            //public static string Encrypt(String content, long epoch, String keyValue, string secret, String charset)
            //{
            //    if (keyValue != null)
            //    {
            //        return MD5(content + epoch + keyValue + secret, 32);
            //    }
            //    return MD5(content, 32);
            //}
            /////<summary>  
            ///// 字符串MD5加密（大寫） 
            /////</summary>  
            /////<param name="str">要加密的字符串</param>  
            /////<param name="code">加密成多少位</param>  
            /////<returns>密文</returns>
            //public static string MD5(string str, int code)
            //{
            //    if (code == 16) //16位MD5加密（取32位加密的9~25字符）   
            //    {
            //        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper().Substring(8, 16);
            //    }
            //    else//32位加密   
            //    {
            //        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
            //    }
            //}


            ///// <summary>
            ///// 从一个对象信息生成Json串
            ///// </summary>
            ///// <param name="obj">对象</param>
            ///// <returns>Json串</returns>
            //public static string ObjectToJson(object obj)
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //    MemoryStream stream = new MemoryStream();
            //    serializer.WriteObject(stream, obj);
            //    byte[] dataBytes = new byte[stream.Length];
            //    stream.Position = 0;
            //    stream.Read(dataBytes, 0, (int)stream.Length);
            //    return Encoding.UTF8.GetString(dataBytes);
            //}

            ///// <summary>
            ///// 从一个Json串生成对象信息
            ///// </summary>
            ///// <param name="jsonString">Json串</param>
            ///// <param name="obj">对象</param>
            ///// <returns>对象</returns>
            //public static object JsonToObject(string jsonString, object obj)
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //    MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            //    return serializer.ReadObject(mStream);
            //}

        }

        #endregion


    }

}
