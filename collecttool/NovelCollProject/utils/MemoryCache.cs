using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NovelCollProjectutils
{
    class CacheUnit
    {
        public DateTime ExpireTime { get; set; }
        public object Data { get; set; }

        public bool IsEnabled()
        {
            return DateTime.Now <= ExpireTime;
        }
    }

    public class MemoryCache
    {
        private static readonly Hashtable _cacheDictionary = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 获取内存缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object Get(string cacheKey)
        {
            //如果正在垃圾回收，请等待
            while (isGcing) Thread.Sleep(10);

            if (!_cacheDictionary.ContainsKey(cacheKey)) return null;

            CacheUnit unit = (CacheUnit)_cacheDictionary[cacheKey];
            if (unit != null && unit.IsEnabled()) return unit.Data;

            _cacheDictionary.Remove(cacheKey);
            return null;
        }

        /// <summary>
        /// 设置内存缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="secondDelay">缓存秒钟</param>
        /// <param name="data">不支持null</param>
        /// <param name="resetDelyIfExist"></param>
        public static void Set(string cacheKey, double secondDelay, object data, bool resetDelyIfExist=true)
        {
            if (data == null) return;
            gcSchedule();

            //如果正在垃圾回收，请等待
            while (isGcing) Thread.Sleep(5);

            if (_cacheDictionary.ContainsKey(cacheKey))
            {
                CacheUnit cache = (CacheUnit) _cacheDictionary[cacheKey];
                if (resetDelyIfExist) cache.ExpireTime = DateTime.Now.AddSeconds(secondDelay);
                cache.Data = data;
            }
            else
            {
                CacheUnit cache = new CacheUnit()
                {
                    Data = data,
                    ExpireTime = DateTime.Now.AddSeconds(secondDelay)
                };
                _cacheDictionary[cacheKey] = cache;
            }
        }

        public static bool Exist(string cacheKey)
        {
            //如果正在垃圾回收，请等待
            while (isGcing) Thread.Sleep(5);

            return _cacheDictionary.ContainsKey(cacheKey);
        }

        public static void Clear(string cacheKey)
        {
            //如果正在垃圾回收，请等待
            while (isGcing) Thread.Sleep(5);

            if (_cacheDictionary.ContainsKey(cacheKey))
                _cacheDictionary.Remove(cacheKey);
        }

        public static bool ClearLike(string cacheKey)
        {
            //如果正在垃圾回收，请等待
            while (isGcing) Thread.Sleep(5);

            string realKey = null;
            foreach (DictionaryEntry entry in _cacheDictionary)
            {
                if (entry.Key.ToString().IndexOf(cacheKey, StringComparison.Ordinal) > -1)
                {
                    realKey = entry.Key.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(realKey))
            {
                _cacheDictionary.Remove(realKey);
                return true;
            }
            return false;
        }


        #region 内存回收机制

        private static bool isGcing;
        private static Thread gcWorker;
        private static readonly object initerLocker=new {};
        private static void gcSchedule()
        {
            if (gcWorker != null) return;

            lock (initerLocker)
            {
                if (gcWorker != null) return;

                gcWorker = new Thread(_gc) { IsBackground = true };
                gcWorker.Start();
            }
        }

        private static void _gc()
        {
            while (true)
            {
                Thread.Sleep(60 * 60000);

                isGcing = true;


                IEnumerable<object> needReceive = (from DictionaryEntry entry in _cacheDictionary where DateTime.Now > ((CacheUnit)entry.Value).ExpireTime select entry.Key);
                List<object> temp = new List<object>();
                foreach (var item in needReceive)
                {
                    temp.Add(item);
                }

                temp?.ForEach(_cacheDictionary.Remove);

                isGcing = false;
            }
        }
        #endregion

    }
    
}
