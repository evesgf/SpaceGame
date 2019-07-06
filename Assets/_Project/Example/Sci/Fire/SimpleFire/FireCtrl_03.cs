using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class FireCtrl_03 : MonoBehaviour
    {
        public LayerMask targetLayer;
        public float angleLimit=10f;

        public Bullet_03 bullet;

        public Transform Muzzle;

        internal bool isFire;

        private NormalTurretCtrl normalTurretCtrl;
        private Bullet_03 bulletObj;

        // Start is called before the first frame update
        void Start()
        {
            normalTurretCtrl = GetComponent<NormalTurretCtrl>();
            isFire = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireStart();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isFire)
                {
                    FireEnd();
                }
            }

            //if (normalTurretCtrl.GetAngleToTarget() > angleLimit)
            //{
            //    if (isFire)
            //    {
            //        foreach (var turret in normalTurretCtrl.NormalShipCtrl_Fire.turrets)
            //        {
            //            var t = turret.GetComponentInChildren<FireCtrl_03>();
            //            t.FireStart();
            //        }
            //    }

            //    FireEnd();
            //}
        }

        public void FireStart()
        {
            if (normalTurretCtrl.GetAngleToTarget() > angleLimit || bulletObj!=null) return;
            bulletObj = Instantiate(bullet, Muzzle.position, Muzzle.rotation);
            bulletObj.transform.parent = Muzzle;
            bulletObj.Init(normalTurretCtrl);
            isFire = true;
        }

        public void FireEnd()
        {
            if (bulletObj != null)
            {
                Destroy(bulletObj.gameObject);
                bulletObj = null;
            }

            isFire = false;
        }
    }
}
