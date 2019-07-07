using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class FirePlasmaGun : FireBase
    {
        public readonly string POOLNAME_BULLET = "FirePlasmaGunBullet";
        public readonly string POOLNAME_MUZZLEFIRE = "FirePlasmaGunMuzzleFire";
        public readonly string POOLNAME_BULLETBOOM = "FirePlasmaGunBulletBoom";
        public readonly string POOLNAME_BULLETAUDIO = "FirePlasmaGunBulletAudio";
        public readonly string POOLNAME_BOOMAUDIO = "FirePlasmaGunBoomAudio";

        public LayerMask targetLayer;                   //目标层级
        public BulletBase bullet;                       //子弹
        public GameObject muzzleFire;                   //枪口火焰
        public AudioSource fireAudio;                   //开火音效
        public GameObject bulletBoom;                   //爆炸特效
        public AudioSource boomAudio;                   //爆炸音效

        public float fireInterval = 0.1f;               //开火间隔

        public Transform[] Muzzles;                     //枪口位置

        private float lastFireTime;
        private int muzzleFlag = 0;

        private Transform muzzlePos;

        private void Start()
        {
            PoolManager.Instance.CreatePool(POOLNAME_BULLET, bullet.GetComponent<PoolObject>(), 0, 20f, 20f);

            PoolManager.Instance.CreatePool(POOLNAME_MUZZLEFIRE, muzzleFire.GetComponent<PoolObject>(), 0, 20f, 20f);

            PoolManager.Instance.CreatePool(POOLNAME_BULLETBOOM, bulletBoom.GetComponent<PoolObject>(), 0, 20f, 20f);

            PoolManager.Instance.CreatePool(POOLNAME_BULLETAUDIO, fireAudio.GetComponent<PoolObject>(), 0, 20f, 20f);

            PoolManager.Instance.CreatePool(POOLNAME_BOOMAUDIO, boomAudio.GetComponent<PoolObject>(), 0, 20f, 20f);
        }

        public override void OnFire()
        {
            if ((Time.time - lastFireTime) < fireInterval) return;

            //切换枪口位置
            muzzlePos = Muzzles[muzzleFlag];
            muzzleFlag = (muzzleFlag + 1) < Muzzles.Length ? muzzleFlag + 1 : 0;

            //生成子弹
            PoolManager.Instance.GetPool(POOLNAME_BULLET).Spawn((PoolObject po) => {
                po.GetComponent<BulletBase>().OnStart();
                po.transform.position = muzzlePos.position;
                po.transform.rotation = muzzlePos.rotation;
            });

            //生成火焰
            PoolManager.Instance.GetPool(POOLNAME_MUZZLEFIRE).Spawn((PoolObject po) => {
                po.transform.parent = muzzlePos;
                po.transform.localPosition = Vector3.zero;
                po.transform.localRotation = Quaternion.identity;
                po.GetComponent<PoolObject>().OnDesSpawned();
            });

            //生成音效
            PoolManager.Instance.GetPool(POOLNAME_BULLETAUDIO).Spawn((PoolObject po) => {
                po.transform.parent = muzzlePos;
                po.transform.localPosition = Vector3.zero;
                po.transform.localRotation = Quaternion.identity;
                po.GetComponent<PoolObject>().OnDesSpawned();
            });

            lastFireTime = Time.time;
        }
    }
}