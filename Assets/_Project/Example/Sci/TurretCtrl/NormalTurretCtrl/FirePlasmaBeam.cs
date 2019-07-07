using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class FirePlasmaBeam : FireBase
    {
        public readonly string POOLNAME_OPENAUDIO = "FirePlasmaBeamOpenAudio";
        public readonly string POOLNAME_CLOSEAUDIO = "FirePlasmaBeamCloseAudio";

        public LayerMask targetLayer;                   //目标层级
        public BulletBase bullet;                       //子弹
        public AudioSource openAudio;                   //开火音效
        public AudioSource closeAudio;                  //收枪音效

        public Transform Muzzle;                        //枪口位置

        internal bool isFire = false;

        private void Start()
        {
            bullet.gameObject.SetActive(false);

            PoolManager.Instance.CreatePool(POOLNAME_OPENAUDIO, openAudio.GetComponent<PoolObject>(), 0, 20f, 20f);
            PoolManager.Instance.CreatePool(POOLNAME_CLOSEAUDIO, closeAudio.GetComponent<PoolObject>(), 0, 20f, 20f);
        }

        public override void OnFire()
        {
            
        }

        public override void StartFire()
        {
            if (isFire) return;
            bullet.OnStart();

            //生成开火音效
            PoolManager.Instance.GetPool(POOLNAME_OPENAUDIO).Spawn((PoolObject po) => {
                po.transform.position = Muzzle.position;
                po.OnDesSpawned();
            });

            isFire = true;
        }

        public override void EndFire()
        {
            bullet.gameObject.SetActive(false);

            //生成收枪音效
            PoolManager.Instance.GetPool(POOLNAME_CLOSEAUDIO).Spawn((PoolObject po) => {
                po.transform.position = Muzzle.position;
                po.OnDesSpawned();
            });

            isFire = false;
        }
    }
}
