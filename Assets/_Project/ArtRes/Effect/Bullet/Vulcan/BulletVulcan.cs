using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{

    public class BulletVulcan : BulletBase
    {
        public readonly string POOLNAME_BULLETBOOM = "FireVulcanBulletBoom";
        public readonly string POOLNAME_BOOMAUDIO = "FireVulcanBoomAudio";

        public LayerMask layerMask;

        public float lifeTime = 5f;
        public float flySpeed = 100f;

        private float nowLifeTime;
        private PoolObject poolObject;

        private Vector3 oldPos;
        private RaycastHit hit;

        private void Awake()
        {
            poolObject = GetComponent<PoolObject>();
        }

        public override void Init()
        {
            nowLifeTime = 0;
        }

        private void OnFly(float time)
        {
            //生命周期检测
            if (nowLifeTime >= lifeTime)
            {
                poolObject.DesSpawn();
            }

            //飞行位置改变
            transform.position += transform.forward * flySpeed * time;
            //碰撞检测
            if (Physics.Linecast(oldPos, transform.position,out hit, layerMask))
            {
                //TODO:计算伤害

                //生成碰撞特效
                PoolManager.Instance.GetPool(POOLNAME_BULLETBOOM).Spawn((PoolObject po) => {
                    po.transform.position = hit.point;
                    po.OnDesSpawned();
                });
                //生成碰撞音效
                PoolManager.Instance.GetPool(POOLNAME_BOOMAUDIO).Spawn((PoolObject po) => {
                    po.transform.position = hit.point;
                    po.OnDesSpawned();
                });

                //回收自身
                poolObject.DesSpawn();
            }

            oldPos = transform.position;
            nowLifeTime += time;
        }

        private void FixedUpdate()
        {
            OnFly(Time.fixedDeltaTime);
        }
    }
}
