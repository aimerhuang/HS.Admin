using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Util;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Memory;
using Hyt.Model.Manual;
using System.Reflection;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 内存管理
    /// </summary>
    /// <remarks>2013-8-13 黄志勇 添加</remarks>
    public class MemoryBo : BOBase<MemoryBo>
    {
        #region 共用方法
        /// <summary>
        /// 对象是否可查看
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="type">memcached、memoryCache</param>
        /// <returns></returns>
        /// <remarks>2013-11-11  黄志勇 创建</remarks>
        public bool CanShow(string key, string type)
        {
            object obj;
            if (type != null && type.ToLower() == "memcached")
            {
                obj = CacheManager.Instance.Get<object>(key);
            }
            else
            {
                obj = MemoryProvider.Default.Get(key);
            }
            if (obj != null)
            {
                Type t = obj.GetType().GetGenericArguments().Length > 0
                             ? obj.GetType().GetGenericArguments()[0]
                             : obj.GetType();
                var list = GetPropertys(t);
                if (list != null && list.Count > 0) return true;
            }
            return false;
        }

        /// <summary>
        /// 根据属性名称和值获取分页数据
        /// </summary>
        /// <param name="value">对象列表</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        /// <remarks>2013-8-15 黄志勇 添加</remarks>
        public Pager<object> GetPagerDataRow(object value, string propertyName, string propertyValue, int size, int page)
        {
            var pager = new Pager<object>();
            var filter = FilerList(value, propertyName, propertyValue) as IList;
            if (value != null && filter.Count > 0)
            {
                //IList list = filter;// filterList.OrderBy(i => i.GetHashCode());
                DataTable dt = GetPageData(filter, size, page);
                var formatDataTable = FormatDataTable(dt, size*(page - 1) + 1);
                pager = new Pager<object>
                {
                    CurrentPage = page,
                    PageSize = size,
                    TotalRows = filter.Count,
                    Rows = formatDataTable.AsEnumerable().Select(i =>
                    {
                        return (object)i;
                    }).ToList()
                };
            }
            return pager;
        }

        /// <summary>
        /// 根据类型获取属性名称列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性名称列表</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public List<string> GetPropertys(Type type)
        {
            var list = new List<string>();
            PropertyInfo[] ps = type.GetProperties();
            list = ps.Where(i => i.PropertyType.IsValueType || i.PropertyType.Name.StartsWith("String")).Select(j => j.Name).ToList();
            return list;
        }

        /// <summary>
        /// 判断对象是否列表
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <returns>true:是 false：否</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public bool IsList(object obj)
        {
            if (obj == null) return false;
            Type t = obj.GetType();
            return !(t.IsValueType || t.Name == "String");
        }

        /// <summary>
        /// 获取集合数量
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public int GetItemCount(object obj)
        {
            var list = obj as IList;
            return list != null ? list.Count : obj == null ? 0 : 1;
        }

        /// <summary>
        /// 对象是否包含指定的属性名称和值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>true:包含 false:不包含</returns>
        /// <remarks>2013-8-14 黄志勇 添加</remarks>
        private bool IsContainProperty(object obj, string propertyName, object propertyValue)
        {
            if (obj != null)
            {
                PropertyInfo[] ps = obj.GetType().GetProperties();
                foreach (var p in ps)
                {
                    object value = p.GetValue(obj, null);
                    if (p.Name == propertyName && value.ToString().ToLower() == propertyValue.ToString().ToLower()) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定属性名称和值的列表
        /// </summary>
        /// <param name="obj">对象列表</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns></returns>
        /// <remarks>2013-8-15 黄志勇 添加</remarks>
        private object FilerList(object obj, string propertyName, string propertyValue)
        {
            IList list = obj as IList;
            if (list == null && obj != null) list = new List<object> {obj};
            if (list == null || list.Count == 0 || string.IsNullOrWhiteSpace(propertyName) ||
                string.IsNullOrWhiteSpace(propertyValue))
            {
                return list;
            }
            var result = new List<object>();
            foreach (var item in list)
            {
                if (IsContainProperty(item, propertyName, propertyValue)) result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 集合转成DataTable
        /// </summary>
        /// <param name="ps">属性集合</param>
        /// <param name="rows">列表</param>
        /// <returns>DataTable</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public DataTable ToDataTable(PropertyInfo[] ps,IList rows)
        {
            var dt = new DataTable();
            ps.Apply(p =>
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = p.Name;
                    Type type = p.PropertyType;
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                    }
                    dc.DataType = type;
                    dt.Columns.Add(dc);
                });
            //dt.Columns.AddRange(ps.Select(p => new DataColumn(p.Name,p.PropertyType)).ToArray());
            if (rows != null && rows.Count > 0)
            {
                IList list = rows as IList;
                foreach (var row in list)
                {
                    var arr = new ArrayList();
                    foreach (PropertyInfo p in ps)
                    {
                        object obj = p.GetValue(row, null);
                        arr.Add(obj ?? DBNull.Value);
                    }
                    dt.LoadDataRow(arr.ToArray(), true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 格式化DataTable
        /// </summary>
        /// <param name="oldDataTable">待处理DataTable</param>
        /// <param name="start">开始序号</param>
        /// <returns>加工后的DataTable</returns>
        /// <remarks>2013-8-14 黄志勇 添加</remarks>
        public DataTable FormatDataTable(DataTable oldDataTable, int start)
        {
            var newDataTable = new DataTable(oldDataTable.TableName);
            newDataTable.Columns.Add("序号", typeof(int)).SetOrdinal(0);
            //构造表头
            for (int i = 0; i < oldDataTable.Columns.Count; i++)
            {
                Type type = oldDataTable.Columns[i].DataType;
                DataColumn dc = oldDataTable.Columns[i];
                if (type.IsValueType || type.Name.StartsWith("String"))
                    newDataTable.Columns.Add(string.Format("{0}", dc.ColumnName, dc.DataType.Name), dc.DataType).SetOrdinal(i + 1);
                else
                    newDataTable.Columns.Add(string.Format("{0}", dc.ColumnName, "List"), typeof(int)).SetOrdinal(i + 1);
            }
            //构造数据行
            for (int i = 0; i < oldDataTable.Rows.Count; i++)
            {
                var dr = newDataTable.NewRow();
                dr[0] = start + i;
                for (int j = 0; j < oldDataTable.Columns.Count; j++)
                {
                    var obj = oldDataTable.Rows[i][j];
                    var type = oldDataTable.Columns[j].DataType;
                    if (type.IsValueType || type.Name.StartsWith("String"))
                    {
                        dr[j + 1] = obj;
                    }
                    else
                    {
                        var list = obj as ICollection;
                        dr[j + 1] = list == null ? 0 : list.Count;
                    }
                }
                newDataTable.Rows.Add(dr);
            }
            return newDataTable;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns>Memcache分页数据</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public DataTable GetPageData(IList list, int size, int page)
        {
           
            if (list != null && list.Count > 0)
            {
                var newList = new List<dynamic>();
                foreach (var item in list)
                {
                    newList.Add(item);
                }
                var rows = newList.Skip(size * (page - 1)).Take(size).ToList();
                Type t = rows[0].GetType();
                PropertyInfo[] ps = t.GetProperties();
                return ToDataTable(ps, rows);
            }
            return null;
        }
        #endregion

        #region 内存键值对
        //public List<string> MemcacheAllKeys = GetMemcacheAllKeys();
        /// <summary>
        /// 获取全部Memcache缓存key
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        private static List<string> GetMemcacheAllKeys()
        {
            return MemCachedUtil.GetllKeys();
        }

        /// <summary>
        /// 删除键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        /// <remarks>2013-8-19 黄志勇 添加</remarks>
        public bool DelMemcacheKey(string key)
        {
            var r = CacheManager.Instance.Delete(key);
            return r;
        }

        /// <summary>
        /// 删除所有键值对
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-11-20 余勇 添加</remarks>
        public void DelAllMemcache()
        {
            CacheManager.Instance.RemovAllCache();
            DelAllWebSiteCache();
        }
        /// <summary>
        /// 删除所有Web站点本地缓存
        /// </summary>
        /// <remarks>2016-12-26 杨浩 创建</remarks>
        public void DelAllWebSiteCache(int dealerSysNo=-1)
        {
            try
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.B2CApp.IProduct>())
                {
                    var response = service.Channel.ResetProductCache(dealerSysNo);
                }      
            }
            catch {}                                                
        }
        /// <summary>
        /// 根据Key前缀删除缓存
        /// </summary>
        /// <param name="prefix">键前缀</param>
        /// <returns></returns>
        /// <remarks>2013-8-19 黄志勇 添加</remarks>
        public bool DeleteByPrefix(string prefix )
        {
            try
            {
                var allKeys = GetMemcacheAllKeys().ToArray();
                allKeys.ApplyParallel(item =>
                {
                    if (item.StartsWith(prefix))
                    {
                        CacheManager.Instance.Delete(item);
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取全部Memcache缓存key总条数
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public int GetMemcacheKeyCount()
        {
            var allKeys = GetMemcacheAllKeys();
            return (allKeys != null ? allKeys.Count : 0);
        }

        /// <summary>
        /// 获取全部Memcache内存占用
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public long GetMemcacheTotal()
        {
            long count = 0;
            var allKeys = GetMemcacheAllKeys();
            if (allKeys != null && allKeys.Count > 0)
            {
                foreach (var key in allKeys)
                {
                    if (key.Contains("RegCommonUserInfo") || key.Contains("RegUserInfo")) continue;
                    var obj = CacheManager.Instance.Get<object>(key);
                    if (obj != null) count += DataUtil.ObjectSize(obj);
                }
            }
            return count;
        }

        /// <summary>
        /// 根据key获取Memcache列表分页信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前面</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<SyKeyInfo> GetMemcachePagerList(string key, int size, int page)
        {
            var result = new Pager<SyKeyInfo>();
            var keyFilter = GetMemcacheKey(key);
            if (keyFilter != null && keyFilter.Count() > 0)
            {
                var keys = keyFilter.OrderBy(i => i).Skip(size*(page-1)).Take(size);
                var list = new List<SyKeyInfo>();
                foreach (var k in keys)
                {
                    var info = GetMemcacheKeyInfo(k);
                    if(info != null) list.Add(info);
                }
               
                result = new Pager<SyKeyInfo>
                    {
                        CurrentPage = page,
                        PageSize = size,
                        TotalRows = keyFilter.Count(),
                        Rows = list.ToList()
                    };
            }
            return result;
        }

        /// <summary>
        /// 根据MemcacheKey模糊查询真实的key列表
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public IEnumerable<string> GetMemcacheKey(string key)
        {
            var allKeys = GetMemcacheAllKeys();
            if (!string.IsNullOrWhiteSpace(key) && allKeys != null && allKeys.Count > 0)
            {
                return allKeys.Where(i => i.StartsWith(key));
            }
            return null;
        }

        /// <summary>
        /// 根据key返回基本信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public SyKeyInfo GetMemcacheKeyInfo(string key)
        {
            SyKeyInfo info = null;
            var value = CacheManager.Instance.Get<object>(key);
            if (value != null)
            {
                info = new SyKeyInfo();
                info.Key = key;
                var index = key.IndexOf('_');
                var prefix = index < 0 ? key : key.Substring(0, index + 1);
                var item = (CacheKeys.Items) Enum.Parse(typeof (CacheKeys.Items), prefix);
                info.Desc = item.GetDescription();
                info.MemoryUsed = FormatUtil.FormatByteCount(DataUtil.ObjectSize(value));
                if (IsList(value))
                {
                    var isArray = value.GetType().IsArray;
                    info.KeyType = isArray ? "数组" : "列表";
                    info.Count = GetItemCount(value);
                    info.KeyValue = Hyt.Util.Serialization.JsonUtil.ToJson(value);
                    info.CanShow = !isArray && CanShow(key, "memcached") && info.Count > 0;
                }
                else
                {
                    info.KeyType =value.GetType().Name;
                    info.KeyValue = (value is DateTime)?((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"):value.ToString();
                }
            }
            return info;
        }

        /// <summary>
        /// 根据key获取Memcache分页数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns>Memcache分页数据</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<object> GetMemcacheData(string key, string propertyName, string propertyValue, int size, int page)
        {
            var value = CacheManager.Instance.Get<object>(key);
            return GetPagerDataRow(value, propertyName, propertyValue, size, page);
        }
        
        #endregion

        #region 内存数据库

        /// <summary>
        /// 获取全部Net缓存key总条数
        /// </summary>
        /// <returns>总条数</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public int GetNetKeyCount()
        {
            int count = 0;
            var allKeys = MemoryProvider.Default.GetAllKey();// GetNetAllKeys();
            if (allKeys != null && allKeys.Count > 0)
            {
                count = allKeys.Sum(i =>
                    {
                        return MemoryProvider.Default.Exists(i) ? 1 : 0;
                    });
            }
            return count;
        }

        /// <summary>
        /// 获取全部Net内存占用
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public long GetNetTotal()
        {
            long count = 0;
            var allKeys = MemoryProvider.Default.GetAllKey();// GetNetAllKeys();
            if (allKeys != null && allKeys.Count > 0)
            {
                foreach (var key in allKeys)
                {
                    var obj = MemoryProvider.Default.Get(key);
                    if (obj != null) count += DataUtil.ObjectSize(obj);
                }
            }
            return count;
        }

        /// <summary>
        /// 获取Net列表分页信息
        /// </summary>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前面</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<SyKeyInfo> GetNetPagerList(int size, int page)
        {
            var result = new Pager<SyKeyInfo>();
            var allKeys = MemoryProvider.Default.GetAllKey();// GetNetAllKeys();
            if (allKeys != null && allKeys.Count() > 0)
            {
                var keys = allKeys.OrderBy(i => i).Skip(size * (page-1)).Take(size);
                var list = new List<SyKeyInfo>();
                foreach (var key in keys)
                {
                    var info = GetNetKeyInfo(key);
                    if (info != null) list.Add(info);
                }
                result = new Pager<SyKeyInfo>
                {
                    CurrentPage = page,
                    PageSize = size,
                    TotalRows = allKeys.Count(),
                    Rows = list.ToList()
                };
            }
            return result;
        }

        /// <summary>
        /// 根据key、属性名称和值获取Net分页数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns>Memcache分页数据</returns>
        ///  <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<object> GetNetData(string key, string propertyName, string propertyValue, int size, int page)
        {
            var value = MemoryProvider.Default.Get(key);
            return GetPagerDataRow(value, propertyName, propertyValue, size, page);
        }

        /// <summary>
        /// 根据key返回基本信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public SyKeyInfo GetNetKeyInfo(string key)
        {
            SyKeyInfo info = null;
            var value = MemoryProvider.Default.Get<object>(key);
            if (value != null)
            {
                info = new SyKeyInfo();
                info.Key = key;
                //var item = (Keys)Enum.Parse(typeof(Keys), key);
                //info.Desc = item.GetDescription();
                info.MemoryUsed = FormatUtil.FormatByteCount(DataUtil.ObjectSize(value));
                if (IsList(value))
                {
                    var isArray = value.GetType().IsArray;
                    info.KeyType = isArray ? "数组" : "列表";
                    info.Count = GetItemCount(value);
                    info.KeyValue = Hyt.Util.Serialization.JsonUtil.ToJson(value);
                    info.CanShow = !isArray && CanShow(key, "netKey") && info.Count > 0;
                }
                else
                {
                    info.KeyType = value.GetType().Name;
                    info.KeyValue = value.ToString();
                }
            }
            return info;
        }

        /// <summary>
        ///清除所有缓存
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-11-20 余勇 添加</remarks>
        public void RemoveAllNetKey()
        {
            MemoryProvider.Default.Clear();
        }

        /// <summary>
        /// 根据Key前缀删除缓存
        /// </summary>
        /// <param name="prefix">键前缀</param>
        /// <returns></returns>
        /// <remarks>2013-8-19 黄志勇 添加</remarks>
        public bool DeleteByNetPrefix(string prefix)
        {
            var allKeys = MemoryProvider.Default.GetAllKey();
            allKeys.Apply(item =>
            {
                if (item.StartsWith(prefix))
                {
                    MemoryProvider.Default.Remove(item);
                }
            });
            return true;
        }
        #endregion
    }
}
