using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GPL
{

    public class BulletPlasmaGun : BulletBase
    {
        public readonly string POOLNAME_BULLETBOOM = "FirePlasmaGunBulletBoom";
        public readonly string POOLNAME_BOOMAUDIO = "FirePlasmaGunBoomAudio";

        public LayerMask layerMask;

        public float lifeTime = 5f;
        public float flySpeed = 100f;

        private float nowLifeTime;
        private PoolObject poolObject;

        private Vector3 oldPos;
        private RaycastHit hit;
        private SphereCollider sphereCollider;

        private void Awake()
        {
            poolObject = GetComponent<PoolObject>();
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.enabled = false;
        }

        public override void OnStart()
        {
            nowLifeTime = 0;
        }

        private void OnFly(float time)
        {
            //生命周期检测
            if (nowLifeTime >= lifeTime)
            {
                OnDesSpawn(transform.position);
            }

            //飞行位置改变
            transform.position += transform.forward * flySpeed * time;
            //碰撞检测
            if (Physics.SphereCast(oldPos+ sphereCollider.center, sphereCollider.radius, transform.forward,out hit, flySpeed * time, layerMask))
            {
                OnDesSpawn(hit.point);
            }

            oldPos = transform.position;
            nowLifeTime += time;
        }

        /// <summary>
        /// 当碰撞或被回收时执行爆炸
        /// </summary>
        /// <param name="boomPos"></param>
        private void OnDesSpawn(Vector3 boomPos)
        {
            //TODO:计算伤害

            //生成碰撞特效
            PoolManager.Instance.GetPool(POOLNAME_BULLETBOOM).Spawn((PoolObject po) => {
                po.transform.position = boomPos;
                po.OnDesSpawned();
            });
            //生成碰撞音效
            PoolManager.Instance.GetPool(POOLNAME_BOOMAUDIO).Spawn((PoolObject po) => {
                po.transform.position = boomPos;
                po.OnDesSpawned();
            });

            //回收自身
            poolObject.DesSpawn();
        }

        private void FixedUpdate()
        {
            OnFly(Time.fixedDeltaTime);
        }
    }
}
