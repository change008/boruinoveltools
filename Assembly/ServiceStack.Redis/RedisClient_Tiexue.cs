using System;
using System.Collections.Generic;
using System.Collections;

using ServiceStack.Common.Extensions;
using ServiceStack.Common.Utils;

using System.Linq;
using System.Text;
using ServiceStack.Text;
using ServiceStack.Redis.Pipeline;

namespace ServiceStack.Redis
{
    /// <summary>
    /// 
    /// Customize By Tiexue.net LTD
    ///  Author: ligaoren
    ///  Date : 2012-3-2 
    ///  Redis target Version:2.4.8
    /// </summary>
    public partial class RedisClient
    {
        /// <summary>
        /// 转value值转成二进制存进redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetRawString(string key, string value)
        {
            byte[] bvalue = System.Text.Encoding.UTF8.GetBytes(value);
            this.Set<byte[]>(key, bvalue);
        }
        /// <summary>
        /// 将二进制存进去的value转成string读取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetRawString(string key)
        {
            var bvalue = this.Get(key);
            if (bvalue != null)
            {
                return System.Text.Encoding.UTF8.GetString(bvalue);
            }
            return null;
        }

        #region POCO Store & Get
        /// <summary>
        /// Store object with customize defined object key and urnkey
        /// </summary>
        public T Store<T>(T entity, string objectid, string urnKey)
            where T : class, new()
        {
            var valueString = JsonSerializer.SerializeToString(entity);
            this.SetEntry(urnKey, valueString);
            RegisterTypeId(entity, objectid);
            return entity;
        }





        /// <summary>
        /// Store object with customize defined object key and urnkey
        /// Create index in set that id is setid
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="objectid">object id</param>
        /// <param name="urnKey">urn key</param>
        /// <param name="setid">group id </param>
        /// <returns></returns>
        public T Store<T>(T entity, string objectid, string urnKey, string setid) where T : class, new()
        {

            this.Store<T>(entity, objectid, urnKey);
            this.AddItemToSet(setid, objectid);
            return entity;
        }

        /// <summary>
        /// Register object id ( Add object id to TypeIdSet)
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">entity</param>
        /// <param name="objectid">object id (Notice:Not urnkey)</param>
        internal void RegisterTypeId<T>(T value, string objectid)
        {

            //var typeIdsSetKey = GetTypeIdsSetKey<T>();

            //if (this.Pipeline != null)
            //{
            //    var registeredTypeIdsWithinPipeline = GetRegisteredTypeIdsWithinPipeline(typeIdsSetKey);
            //    registeredTypeIdsWithinPipeline.Add(objectid);
            //}
            //else
            //{
            //    this.AddItemToSet(typeIdsSetKey, objectid);
            //}
        }
        /// <summary>
        /// Store all entities with customizen objectid&urnkey generate rule
        /// </summary>
        /// <typeparam name="TEntity">Type of Entity</typeparam>
        /// <param name="entities">entities</param>
        /// <param name="getObjectidFunc">e.g: n=>n.ID</param>
        /// <param name="getUrnKeyFunc">e.g:"123".ToUrnKey() </param>
        public void StoreAll<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, string> getObjectidFunc, Func<TEntity, string> getUrnKeyFunc)
            where TEntity : class, new()
        {
            _StoreAll(entities, getObjectidFunc, getUrnKeyFunc);
        }

        /// <summary>
        /// Store All entities using Customized id/urnkey rules 
        /// Ya Version is using dynamic,refactor Func for performance concern
        /// 
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entities">entity</param>
        /// <param name="getObjectid">e.g: n=>n.ID</param>
        /// <param name="getUrnKey">e.g:"123".ToUrnKey() </param>
        internal void _StoreAll<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, string> getObjectId, Func<TEntity, string> getUrnkey)
        {
            if (entities == null) return;

            var entitiesList = entities.ToList();
            var len = entitiesList.Count;
            if (len == 0) return;

            var keys = new byte[len][];
            var values = new byte[len][];

            List<string> ids = new List<string>();
            for (var i = 0; i < len; i++)
            {
                ids.Add(getObjectId(entitiesList[i]));
                keys[i] = getUrnkey(entitiesList[i]).ToUtf8Bytes();
                values[i] = SerializeToUtf8Bytes(entitiesList[i]);
            }

            base.MSet(keys, values);

            var typeIdsSetKey = GetTypeIdsSetKey<TEntity>();
            if (this.Pipeline != null)
            {
                var registeredTypeIdsWithinPipeline = GetRegisteredTypeIdsWithinPipeline(typeIdsSetKey);
                ids.ForEach(x => registeredTypeIdsWithinPipeline.Add(x));
            }
            else
            {
                AddRangeToSet(typeIdsSetKey, ids);
            }
        }




        /// <summary>
        ///Get Object By Urnkeys 
        /// </summary>
        public IList<T> GetByUrnKeys<T>(List<string> urnKeys)
            where T : class, new()
        {
            if (urnKeys == null || urnKeys.Count == 0)
                return new List<T>();
            return GetValues<T>(urnKeys);
        }

        #endregion

        #region Hash Object Store & Get

        /// <summary>
        /// Store Entity as Hash
        /// Q: Why not using StoreAsHash?
        /// A: There will be an Error when do entity.ToJson().ToDictionary data disorderd
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="urnKey">urnKey</param>
        /// <param name="objectid">object id</param>
        /// <param name="entity">Entity</param>
        /// <param name="entityToDictionary">using this func to build dictionary from entity</param>
        public void StoreAsHash<T>(string urnKey, string objectid, T entity, Func<T, Dictionary<string, string>> entityToDictionary) where T : class, new()
        {
            SetRangeInHash(urnKey, entityToDictionary(entity));
            RegisterTypeId(entity, objectid);
        }


        /// <summary>
        /// Store as Hash and  Create index in set which id is setid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urnKey"></param>
        /// <param name="objectid"></param>
        /// <param name="entity"></param>
        /// <param name="entityToDictionary"></param>
        /// <param name="setid"></param>
        public void StoreAsHash<T>(string urnKey, string objectid, T entity, Func<T, Dictionary<string, string>> entityToDictionary, string setid) where T : class, new()
        {
            this.StoreAsHash<T>(urnKey, objectid, entity, entityToDictionary);
            this.AddItemToSet(setid, objectid);
        }

        /// <summary>
        /// Get Values From Hash 
        /// </summary>
        /// <param name="hashId">Hash ID</param>
        /// <param name="keys">keys </param>
        /// <returns>{ {key1,value1},{key2,value2},{key3,value3}}</returns>
        public Dictionary<string, string> GetValuesFromHash(string hashId, List<string> keys)
        {

            if (keys == null) return new Dictionary<string, string>();
            if (keys.Count() == 0) return new Dictionary<string, string>();
            var keyBytes = ConvertToBytes(keys.ToArray());
            var multiDataList = base.HMGet(hashId, keyBytes).ToStringList();
            int length = keys.Count();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                dict.Add(keys[i], multiDataList[i]);
            }
            return dict;
        }


        /// <summary>
        /// Get entity From Hash by urnkey
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="urnKey">urnkey</param>
        /// <param name="dictionaryToEntity">using this func to build entity from dictionary</param>
        /// <returns></returns>
        public T GetFromHash<T>(string urnKey, Func<Dictionary<string, string>, T> dictionaryToEntity) where T : class, new()
        {
            return dictionaryToEntity(GetAllEntriesFromHash(urnKey));
        }

        /// <summary>
        /// Get entities by urnkeys when objects stored as hash.
        /// Performance Notice:refactor this when redis support hash-multi-get 
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="urnKeys"></param>
        /// <param name="convertdictToEntity">using this func to build entity from dictionary,e.g:  Post:ToPost()</param>
        /// <returns></returns>
        public List<T> GetHashObjectListByUrnKeys<T>(List<string> urnKeys, Func<Dictionary<string, string>, T> convertdictToEntity) where T : class, new()
        {
            if (urnKeys == null) throw new ArgumentNullException("keys");
            if (urnKeys.Count == 0) return new List<T>();

            var results = new List<T>();

            foreach (var item in urnKeys)
            {
                results.Add(convertdictToEntity(GetAllEntriesFromHash(item)));
            }
            return results;
        }

        /// <summary>
        /// Store all entities as Hash
        /// Q: Why not using StoreAsHash?
        /// A: There will be an Error when do entity.ToJson().ToDictionary data disorderd
        /// 
        /// Performance Notice:refactor this code when redis support hash-multi-set
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="entities">Entity object</param>
        /// <param name="getObjectid">e.g: n=>n.ID</param>
        /// <param name="getUrnKey">e.g:"123".ToUrnKey() </param>
        /// <param name="entityToDictionary">using this func to build dictionary from entity </param>
        public void StoreAllAsHash<T>(IEnumerable<T> entities, Func<T, string> getObjectid, Func<T, string> getUrnKey, Func<T, Dictionary<string, string>> entityToDictionary) where T : class, new()
        {
            _StoreAllAsHash<T>(entities, getObjectid, getUrnKey, entityToDictionary);
        }



        internal void _StoreAllAsHash<T>(IEnumerable<T> entities, Func<T, string> getObjectidFunc, Func<T, string> getUrnKeyFunc, Func<T, Dictionary<string, string>> entityToDictionary) where T : class, new()
        {
            if (entities == null) return;
            foreach (var item in entities)
            {
                StoreAsHash<T>(getUrnKeyFunc(item), getObjectidFunc(item), item, entityToDictionary);
            }
        }


        #endregion




        #region Pipeline Extension





        #endregion


        #region Set Extension

        /// <summary>
        /// 调用Redis的SAdd命令
        /// </summary>
        /// <param name="setId">Set的Key</param>
        /// <param name="item">Set中的Mermber</param>
        /// <returns>添加成功返回1，失败返回0.</returns>
        public int SAdd(string setId, string item)
        {
            return base.SAdd(setId, item.ToUtf8Bytes());
        }

        #endregion

    }
}
