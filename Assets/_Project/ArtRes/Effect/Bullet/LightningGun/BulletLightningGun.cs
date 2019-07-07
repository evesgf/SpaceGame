using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class BulletLightningGun : BulletBase
    {
        public LayerMask layerMask;
        public FireLightningGun fireLightningGun;

        public float lifeTime = 5f;
        public float flySpeed = 100f;
        public float radius = 1.5f;

        public Transform flare;                     //末端效果
        public Transform muzzle;                    //炮口效果
        public LineRenderer[] lineRenderers;        //射线本体
        public float lineScale = 1f;                //射线贴图缩放
        public float uvSpeed = -5f;                 //射线贴图UV变化速度

        internal RaycastHit hit;

        private float nowLifeTime;
        private float length;

        private AudioSource audioSource;
        private Vector3 oldPos;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public override void OnStart()
        {
            gameObject.SetActive(true);
            nowLifeTime = 0;
            audioSource.Play();

            foreach (var line in lineRenderers)
            {
                line.SetPosition(1, new Vector3(0, 0, 0));
            }
            flare.localPosition = Vector3.zero;
        }

        public override void OnEnd()
        {
            gameObject.SetActive(false);

            //生成收枪音效
            PoolManager.Instance.GetPool(FireLightningGun.POOLNAME_BOOMAUDIO).Spawn((PoolObject po) => {
                po.transform.position = flare.position;
                po.OnDesSpawned();
            });
        }


        private void OnFly(float time)
        {
            if (nowLifeTime >= lifeTime)
            {
                fireLightningGun.EndFire();
            }

            flare.position += flare.forward * flySpeed * time;

            //碰撞检测
            if (Physics.SphereCast(oldPos, radius, transform.forward, out hit, flySpeed * time, layerMask))
            {
                //TODO:计算伤害

                //EndFire
                fireLightningGun.EndFire();
            }

            //计算lineRenderers长度
            length = flare.localPosition.z;
            foreach (var line in lineRenderers)
            {
                line.SetPosition(1, new Vector3(0, 0, length));
                line.material.SetTextureScale("_MainTex", new Vector2(length * (lineScale / 10f), 1f));
                line.material.SetTextureOffset("_MainTex", new Vector2(Time.time * uvSpeed, 0f));
            }

            oldPos = flare.position;
            nowLifeTime += time;
        }

        private void FixedUpdate()
        {
            OnFly(Time.fixedDeltaTime);
        }
    }
}
