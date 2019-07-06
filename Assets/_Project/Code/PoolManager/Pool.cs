using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class Pool : MonoBehaviour
    {
        public string poolName;                 //对象池名

        private PoolObject poolObj;
        private List<PoolObject> spawns;
        private List<PoolObject> desSpawns;

        private float autoReleaseInterval;
        private float capacity;
        private float expireTime;

        private float autoReleaseTime = 0f;

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="poolName">对象池名字</param>
        /// <param name="poolObj">对象池对象</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">自动释放间隔秒数</param>
        /// <param name="expireTime">对象过期秒数</param>
        /// <returns></returns>
        public void Init(string poolName, PoolObject poolObj, int capacity, float autoReleaseInterval = 60f, float expireTime = 60f)
        {
            this.poolName = poolName;
            this.poolObj = poolObj;
            spawns = new List<PoolObject>();
            desSpawns = new List<PoolObject>();

            this.autoReleaseInterval = autoReleaseInterval;
            this.capacity = capacity;
            this.expireTime = expireTime;

            autoReleaseTime = 0f;

            //填池
            for (int i = 0; i < capacity; i++)
            {
                Spawn();
            }
            DesAllSpawn();
        }

        /// <summary>
        /// 返回对象池中对象的数量
        /// </summary>
        public int Count
        {
            get
            {
                return spawns.Count;
            }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="action">对象创建后的动作</param>
        /// <returns></returns>
        public PoolObject Spawn(Action<PoolObject> action=null)
        {
            if (poolObj == null)
            {
                Debug.LogError("Spawn poolObj is null!");
                return null;
            }

            PoolObject po;
            //检查是否有闲置对象
            if (desSpawns.Count > 0)
            {
                po = desSpawns[0];
                desSpawns.RemoveAt(0);
                po.gameObject.SetActive(true);
            }
            else
            {
                po=Instantiate(poolObj);
                if (po == null)
                {
                    Debug.LogError("Instantiate poolObj is null!");
                    return null;
                }

                po.OnSpwan(this.poolName);

                spawns.Add(po);
            }

            //检查是否超出数量
            if (Count > capacity)
            {
                Release();
            }

            //随后执行相关方法
            if (action != null) action.Invoke(po);

            return po;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj"></param>
        public bool DesSpawn(PoolObject obj, Action<PoolObject> action=null)
        {
            bool isDesSpawn = false;
            if (obj == null)
            {
                Debug.LogError("UnSpawn obj is null!");
                return isDesSpawn;
            }

            foreach (var po in spawns)
            {
                if (po == obj)
                {
                    po.gameObject.SetActive(false);
                    po.CancelInvoke();
                    po.inDesSpawnTime = Time.time;
                    desSpawns.Add(obj);

                    //检查是否超出数量
                    if (Count > capacity)
                    {
                        Release();
                    }

                    if (action != null) action.Invoke(obj);

                    isDesSpawn = true;
                    return isDesSpawn;
                }
            }

            return isDesSpawn;
        }

        /// <summary>
        /// 释放可供释放的对象
        /// </summary>
        public void Release()
        {
            for (int i = 0; i < desSpawns.Count; i++)
            {
                if ((Time.time - desSpawns[i].inDesSpawnTime) > expireTime)
                {
                    Destroy(desSpawns[i].gameObject);
                    desSpawns.RemoveAt(i);
                }
            }
            autoReleaseTime = 0;
        }

        /// <summary>
        /// 释放所有未使用的对象
        /// </summary>
        public void ReleaseAllUnUsed()
        {
            //TODO:释放所有未使用的对象
            autoReleaseTime = 0;
        }

        /// <summary>
        /// 回收所有对象
        /// </summary>
        public void DesAllSpawn()
        {
            foreach (var obj in spawns)
            {
                DesSpawn(obj);
            }
        }

        internal void Update()
        {
            autoReleaseTime += Time.deltaTime;
            if (autoReleaseTime < autoReleaseInterval) return;

            Release();
        }
    }
}
