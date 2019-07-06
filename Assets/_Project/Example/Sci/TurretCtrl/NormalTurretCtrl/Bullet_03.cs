using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class Bullet_03 : MonoBehaviour
    {
        public float lifeTime = Mathf.Infinity;
        public float rayLength = 100f;

        public Transform flare;
        public Transform muzzle;
        public LineRenderer[] lineRenderers;
        public float lineScale = 1f;
        public float uvSpeed = -5f;

        private float nowLifeTime;
        private AudioSource audioSource;
        private NormalTurretCtrl normalTurretCtrl;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Init(NormalTurretCtrl normalTurretCtrl)
        {
            nowLifeTime = 0;
            audioSource.Play();
            this.normalTurretCtrl = normalTurretCtrl;
        }

        private void OnFly(float time)
        {
            if (nowLifeTime >= lifeTime)
            {
                Destroy(gameObject);
            }

            //射线检测
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, rayLength))
            {
                flare.position = hit.point;
            }
            else
            {
                flare.position = muzzle.forward * rayLength;
            }
            //计算Line长度
            float length = Vector3.Distance(flare.position, muzzle.position);
            foreach (var line in lineRenderers)
            {
                line.SetPosition(1, new Vector3(0, 0, length));
                line.material.SetTextureScale("_MainTex", new Vector2(length * (lineScale / 10f), 1f));
                line.material.SetTextureOffset("_MainTex", new Vector2(Time.time * uvSpeed, 0f));
            }

            transform.LookAt(this.normalTurretCtrl.target);

            nowLifeTime += time;
        }

        private void FixedUpdate()
        {
            OnFly(Time.fixedDeltaTime);
        }
    }
}
