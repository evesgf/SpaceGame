using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class PoolObject : MonoBehaviour
    {
        public string poolName;                 //对象池名
        public float desSpawnDelay;             //回收时间

        internal float inDesSpawnTime;            //放入回收池的时间

        public void OnSpwan(string poolName)
        {
            this.poolName = poolName;
        }

        /// <summary>
        /// 倒计时回收对象
        /// </summary>
        public void OnDesSpawned()
        {
            Invoke("DesSpawn", desSpawnDelay);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void DesSpawn()
        {
            PoolManager.Instance.GetPool(poolName).DesSpawn(this);
        }
    }
}
