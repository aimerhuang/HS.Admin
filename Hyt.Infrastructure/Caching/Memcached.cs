using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Enyim.Caching.Memcached;
using MemcachedClient = Enyim.Caching.MemcachedClient;

namespace Hyt.Infrastructure.Caching
{
    /// <summary>
    /// Memcached的缓存策略，实现了缓存策略接口
    /// </summary>
    /// 杨浩
    /// <remarks>2013-08-01 黄波 重构</remarks>
    public class Memcached : ICache
    {
        MemcachedClient mc = new MemcachedClient();

        public T Get<T>(string key)
        {
            return mc.Get<T>(key);
        }

        public void Set(string key, object value)
        {
            mc.Store(StoreMode.Set, key, value);
        }

        public void Set(string key, object value, DateTime expiry)
        {
            mc.Store(StoreMode.Set, key, value, expiry);
        }

        public void Update(string key, object value)
        {
            mc.Store(StoreMode.Replace, key, value);
        }

        public void Update(string key, object value, DateTime expiry)
        {
            mc.Store(StoreMode.Replace, key, value, expiry);
        }

        public bool Delete(string key)
        {
            return mc.Remove(key);
        }

        public void RemovAllCache()
        {
            mc.FlushAll();
        }

        public List<string> GetAllKeys()
        {

            //string test =
            //    "OrderList_1870,OrderList_1869,OrderList_1868,OrderList_1867,OrderList_1866,OrderList_1865,OrderList_1864,OrderList_1863,OrderList_1862,OrderList_1861,OrderList_1860,OrderList_1859,OrderList_1857,OrderList_1858,OrderList_1855,OrderList_1856,OrderList_1854,OrderList_1853,OrderList_1852,OrderList_1851,OrderList_1849,OrderList_1850,OrderList_1848,OrderList_1847,OrderList_1846,OrderList_1845,OrderList_1844,OrderList_1843,OrderList_1842,OrderList_1841,OrderList_1840,OrderList_1835,OrderList_1834,OrderList_1833,OrderList_1832,OrderList_1831,OrderList_1830,OrderList_1828,OrderList_1827,OrderList_1815,OrderList_1814,OrderList_1805,OrderList_1804,OrderList_1801,OrderList_1800,OrderList_1799,OrderList_1796,OrderList_1795,OrderList_1794,OrderList_1793,OrderList_1791,OrderList_1785,OrderList_1780,OrderList_1778,OrderList_1769,OrderList_1762,OrderList_1761,OrderList_1749,OrderList_1748,OrderList_1747,OrderList_1745,OrderList_1744,OrderList_1740,OrderList_1739,OrderList_1732,OrderList_1723,OrderList_1705,OrderList_1691,OrderList_1689,OrderList_1636,OrderList_1614,OrderList_1610,OrderList_1605,IntTest,DateTest";
            //return new List<string>(test.Split(','));
            
            //List<string> list = MemCachedUtil.GetllKeys();
            //return list;

            return mc.Get("keys") as List<string>;
        }

        public bool IsExists(string key)
        {
            return (mc.Get(key) != null);
        }

        public bool DeleteByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }
    }
}
