using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class PoolManager : SingletonMono<PoolManager>
    {
        private Dictionary<string, Pool> pools;             //当前创建的对象池

        private void Awake()
        {
            //初始化单例
            PoolManager.Create();

            pools = new Dictionary<string, Pool>();
        }

        #region 对象池相关
        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="poolName">对象池名字</param>
        /// <param name="poolObj">对象池对象</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">自动释放间隔秒数</param>
        /// <param name="expireTime">对象过期秒数</param>
        /// <returns></returns>
        public Pool CreatePool(string poolName,PoolObject poolObj, int capacity, float autoReleaseInterval=60f,  float expireTime=60f)
        {
            if (CheckHasPool(poolName)) return GetPool(poolName);

            GameObject go = new GameObject(poolName);
            go.transform.parent = transform;
            var pool = go.AddComponent<Pool>();
            pool.Init(poolName,poolObj, capacity, autoReleaseInterval, expireTime);

            pools.Add(poolName, pool);

            return pool;
        }

        public bool CheckHasPool(Pool pool)
        {
            return CheckHasPool(pool);
        }
        public bool CheckHasPool(string poolName)
        {
            return pools.ContainsKey(poolName);
        }

        /// <summary>
        /// 根据对象池名字返回相应对象池
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public Pool GetPool(string poolName)
        {
            return pools[poolName];
        }
        #endregion
    }
}