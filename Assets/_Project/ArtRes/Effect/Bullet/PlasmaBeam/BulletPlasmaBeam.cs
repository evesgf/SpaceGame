using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class BulletPlasmaBeam : BulletBase
    {
        public LayerMask layerMask;

        public float maxRayLength=100f;             //最大射程

        public float hurtInterval = 1f;             //伤害间隔

        public Transform flare;                     //末端效果
        public Transform muzzle;                    //炮口效果
        public LineRenderer[] lineRenderers;        //射线本体
        public float lineScale = 1f;                //射线贴图缩放
        public float uvSpeed = -5f;                 //射线贴图UV变化速度

        internal Vector3 target;
        internal RaycastHit hit;

        private float nowLifeTime;
        private float nowRayLength;

        private AudioSource audioSource;
        private float nowHurtInterval;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public override void OnStart()
        {
            gameObject.SetActive(true);
            flare.gameObject.SetActive(false);
            nowLifeTime = 0;
            audioSource.Play();
        }

        public void CheckHurt(float time)
        {
            nowHurtInterval += time;
            if (nowHurtInterval >= hurtInterval)
            {
                //伤害处理
                print("hurt!");

                nowHurtInterval = 0;
            }
        }

        private void OnFire(float time)
        {
            //射线检测
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRayLength))
            {
                flare.position = hit.point;
                flare.gameObject.SetActive(true);

                //计算lineRenderers长度
                nowRayLength = flare.localPosition.z;

                //计算伤害间隔
                CheckHurt(time);
            }
            else
            {
                flare.position = muzzle.forward * maxRayLength;
                flare.gameObject.SetActive(false);

                nowRayLength = maxRayLength;

                //归零间隔
                nowHurtInterval = 0;
            }

            foreach (var line in lineRenderers)
            {
                line.SetPosition(1, new Vector3(0, 0, nowRayLength));
                line.material.SetTextureScale("_MainTex", new Vector2(nowRayLength * (lineScale / 10f), 1f));
                line.material.SetTextureOffset("_MainTex", new Vector2(Time.time * uvSpeed, 0f));
            }

            nowLifeTime += time;
        }

        private void FixedUpdate()
        {
            OnFire(Time.fixedDeltaTime);
        }
    }
}
