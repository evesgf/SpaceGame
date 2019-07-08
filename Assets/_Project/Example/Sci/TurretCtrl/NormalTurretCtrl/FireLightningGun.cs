using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class FireLightningGun : FireBase
    {
        public readonly string POOLNAME_FIREAUDIO = "FireLightningGunFireAudio";
        public static readonly string POOLNAME_BOOMAUDIO = "FireLightningGunBoomAudio";

        public LayerMask targetLayer;                   //目标层级
        public BulletBase bullet;                       //子弹
        public AudioSource fireAudio;                   //开火音效
        public AudioSource boomAudio;                  //爆炸音效

        public Transform Muzzle;                        //枪口位置

        internal bool isFire = false;

        private NormalTurretCtrl normalTurretCtrl;

        private void Start()
        {
            normalTurretCtrl = GetComponent<NormalTurretCtrl>();

            bullet.gameObject.SetActive(false);

            PoolManager.Instance.CreatePool(POOLNAME_FIREAUDIO, fireAudio.GetComponent<PoolObject>(), 0, 20f, 20f);
            PoolManager.Instance.CreatePool(POOLNAME_BOOMAUDIO, boomAudio.GetComponent<PoolObject>(), 0, 20f, 20f);
        }

        public override void OnFire()
        {

        }

        public override void StartFire()
        {
            base.EndFire();

            if (isFire) return;
            bullet.OnStart();

            //生成开火音效
            PoolManager.Instance.GetPool(POOLNAME_FIREAUDIO).Spawn((PoolObject po) => {
                po.transform.position = Muzzle.position;
                po.OnDesSpawned();
            });

            isFire = true;
            normalTurretCtrl.isLockRotate = true;
        }

        public override void EndFire()
        {
            base.EndFire();
        }

        public void OnEndFire()
        {
            isFire = false;
            normalTurretCtrl.isLockRotate = false;
        }
    }
}
