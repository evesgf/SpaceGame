using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class Bullet_04 : MonoBehaviour
    {
        public float lifeTime = 5f;
        public float flySpeed = 100f;

        public Transform flare;
        public Transform muzzle;
        public LineRenderer[] lineRenderers;
        public float lineScale=1f;
        public float uvSpeed = -5f;

        private float nowLifeTime;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Init()
        {
            nowLifeTime = 0;
            audioSource.Play();
        }

        private void OnFly(float time)
        {
            if (nowLifeTime >= lifeTime)
            {
                Destroy(gameObject);
            }

            flare.position += flare.forward * flySpeed * time;

            //计算Line长度
            float length = Vector3.Distance(flare.position,muzzle.position);
            foreach (var line in lineRenderers)
            {
                line.SetPosition(1, new Vector3(0, 0, length));
                line.material.SetTextureScale("_MainTex", new Vector2(length*(lineScale/10f), 1f));
                line.material.SetTextureOffset("_MainTex", new Vector2(Time.time * uvSpeed, 0f));
            }

            nowLifeTime += time;
        }

        private void FixedUpdate()
        {
            OnFly(Time.fixedDeltaTime);
        }
    }
}
